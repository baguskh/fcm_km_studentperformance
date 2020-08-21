using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace kfcm
{
    
    public partial class input_fcm : Form
    {
        public input_fcm()
        {
            InitializeComponent();
        }
        
        konek koneksi = new konek();
        string query = "select * from biodata";
        string queryfcm = "SELECT b.NPM, b.Nama, b.IPK, b.Lama_Studi, b.Asal_Sekolah, h.cluster_fcm FROM biodata b INNER JOIN hasil_cluster h ON h.NPM= b.NPM";
        kelasQuery nk;


        public void fcms()
        {
            // PROSES FCM BEGIN
            konek koneksi = new konek();
            string query = "select * from biodata";
            string queryfcm = "SELECT b.NPM, b.Nama, b.IPK, b.Lama_Studi, b.Asal_Sekolah, h.cluster_fcm FROM biodata b INNER JOIN hasil_cluster h ON h.NPM= b.NPM";
            kelasQuery nk;
            // Inisialisasi
            double epsmin = double.Parse(textBox10.Text); //Nilai epsilon minimum yg ditentukan
            int iter = 1; //Iterasi pertama mulai dari 1
            int cluster; // Untuk proses memasukan data ke cluster setelah menghitung matriks partisi baru
            int jumdat = 1; //Untuk menentukan jumlah data
            int c = Convert.ToInt32(textBox1.Text); //Jumlah cluster yg diinginkan
            double w = Convert.ToDouble(textBox2.Text); //Bobot Pemangkat
            int maxiter = Convert.ToInt32(textBox3.Text); //Iterasi maksimum yang ditentukan
            int k, t, i, j, b, ib, ic, id, ia;
                       
            double galat_sebelum=0; //Inisialisasi Galat Sebelum
            double epsilon = 1000; //Inisialisasi epsilon

            // MENGHITUNG JUMLAH DATA
            koneksi.vKoneksiBuka();
            MySqlCommand myCm = new MySqlCommand(query, koneksi.conn);
            MySqlDataReader myRea;
            myRea = myCm.ExecuteReader();
            while (myRea.Read())
            {
                jumdat++; //Menghitung Jumlah Data Mahasiswa di Database
            }
            koneksi.vKoneksiTutup();

            double[,] A = new double[jumdat, 5]; //kolom 1 utk ipk, 2 utk lma_std, 3 utk cluster, 4 utk npm

            // MEMASUKAN DATA KEDALAM MATRIKS
            koneksi.vKoneksiBuka();
            MySqlCommand myCmd = new MySqlCommand(query, koneksi.conn);
            MySqlDataReader myRead;
            myRead = myCmd.ExecuteReader();
            i = 1;
            while (myRead.Read())
            {
                A[i, 1] = double.Parse(myRead.GetString(2));// kolom ke-1 utk ipk
                A[i, 2] = double.Parse(myRead.GetString(5));// kolom ke-2 utk lm_std
                A[i, 4] = double.Parse(myRead.GetString(0));// kolom ke-3 utk NPM
                i++;
            }
            koneksi.vKoneksiTutup();

            // MEMBANGKITKAN MATRIKS PARTISI ACAK
            double[,] miu = new double[jumdat, c + 1]; 
 
            Random bilangan_random = new Random(5);

            for (i = 1; i < jumdat; i++)
            {
                for (j = 1; j <= c; j++)
                {
                    double isi_elemen_miu;
                    isi_elemen_miu = bilangan_random.NextDouble();
                    miu[i, j] = isi_elemen_miu;

                }
            }

            //NORMALISASI MIU
            double[] sumiu = new double[jumdat]; //array untuk jumlah masing2 kolom di miu
            for (i = 1; i < jumdat; i++)
            {
                sumiu[i] = 0;
                for (j = 1; j < c + 1; j++)
                {
                    sumiu[i] = miu[i, j] + sumiu[i]; //untuk mendapat jumlah sum(Q) per baris
                }
            }
            for (i = 1; i < jumdat; i++)
            {
                for (j = 1; j < c + 1; j++)
                {
                    miu[i, j] = miu[i, j] / sumiu[i]; //miu[i,j]/sum(Q)
                }
            }

            // Iterasi Dimulai!!!!!!!!!!!

            while (epsilon > epsmin && iter <= maxiter) //Berhenti looping jika epsilon<epsilon min atau jika iterasi>maksimum iterasi yg ditentukan
            {               
                // MENGHITUNG PUSAT CLUSTER
                double[,] V = new double[c + 1, 3]; //array baris sebesar k, kolom sebesar j dimana k=1..jumlah cluster, j=(1=IPK,2=lamastudi)
                for (k = 1; k <= c; k++)
                {
                    for (j = 1; j <= 2; j++)
                    {
                        double atas = 0;
                        double bawah = 0;
                        for (i = 1; i < jumdat; i++)
                        {
                            atas += Math.Pow(miu[i, k], w) * A[i, j];
                            bawah += Math.Pow(miu[i, k], w);
                        }
                        V[k, j] = Math.Round((atas / bawah), 6);
                    }
                } 

                //Menghitung devc (selisih antara elemen X dan V dikuadratkan - ada dalam rumus)
                double[,] devc = new double[jumdat, c * 2 + 1]; //array untuk selisih antara seluruh elemen X dan V dikuadratkan)

                for (ia = 1; ia <= c; ia++)
                {
                    id = ia - 1;
                    ib = id * 2 + 1;
                    for (i = 1; i <= 2; i++)
                    {
                        for (j = 1; j < jumdat; j++)
                        {
                            devc[j, ib] = Math.Pow((A[j, i] - V[ia, i]), 2);
                        }
                        ib++;
                    }
                }
                
                // MENGHITUNG setiap elemen rumus devc dikali miu, jika nanti dijumlahkan setiap elemennya akan menjadi nilai galat pada iterasi ini
                double[,] devcmiukuadrat = new double[jumdat, c * 2 + 1]; //array untuk selisih antara seluruh elemen X dan V dikuadratkan dan dikali miu^w
                double galat_sekarang = 0;
                for (ia = 1; ia <= c; ia++)
                {
                    id = ia - 1;
                    ib = id * 2 + 1; //Agar setiap i dilooping kolom akan start dari nilai 1,3,5,7,.. (tergantung jumlah cluster yg diinginkan)
                    for (i = 1; i <= 2; i++)
                    {
                        for (j = 1; j < jumdat; j++)
                        {
                            devcmiukuadrat[j, ib] = devc[j, ib] * Math.Pow(miu[j, ia], w);
                            galat_sekarang = devcmiukuadrat[j, ib] + galat_sekarang;
                        }
                        ib++;
                    }
                }

                //menghitung galat iterasi ke-t dengan menjumlahkan semua elemen devcmiukuadrat
                /*
                for (i = 1; i < jumdat; i++)
                {
                    for (j = 1; j <= c * 2; j++)
                    {
                        galat_sekarang = devcmiukuadrat[i, j] + galat_sekarang;
                    }
                }
                */
                //MENGHITUNG MATRIKS PARTISI BARU
                double[,] sumw = new double[jumdat, c + 1]; //menyimpan matriks partisi yang belum dinormalisasi
                double[] sumtot = new double[jumdat]; // untuk membagi sumw agar matriks partisi ternormalisasi
                for (i = 1; i < jumdat; i++)
                {

                    for (j = 1; j <= c; j++)
                    {
                        ia = j - 1;
                        ib = 2 * ia + 1; //Agar setiap i dilooping kolom miu akan start dari nilai 1,3,5,7,.. (tergantung jumlah cluster yg diinginkan)
                        sumw[i, j] = Math.Pow(devc[i, ib] + devc[i, ib + 1], (-1 / (w - 1)));
                        sumtot[i] = sumw[i, j] + sumtot[i]; // Menghitung total elemen miu per-baris utk dinormalisasi
                    }
                }

                //MENORMALISASIKAN MIU
                for (i = 1; i < jumdat; i++)
                {
                    for (j = 1; j <= c; j++)
                    {
                        miu[i, j] = sumw[i, j] / sumtot[i];
                    }
                }

                //MENCARI DERAJAT KEANGGOTAAN YANG PALING TINGGI DI TIAP DATA
                for (i = 1; i < jumdat; i++)
                {
                    double max = 0;
                    for (j = 1; j <= c; j++)
                    {

                        if (miu[i, j] > max)
                        {
                            max = miu[i, j];
                            cluster = j;
                            A[i, 3] = cluster;
                        }
                    }
                }               

                //Menghitung galat iterasi
                epsilon = Math.Abs(galat_sebelum - galat_sekarang);
                galat_sebelum = galat_sekarang;
                iter++;

            }

            //TAMPILKAN DI TABEL
            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("NPM");
            listView1.Columns.Add("IPK");
            listView1.Columns.Add("Lama Studi");
            listView1.Columns.Add("Cluster");
            k = 0;
            t = 1;
            while (k < jumdat - 1)
            {

                listView1.Items.Add(A[t, 4].ToString()); //
                listView1.Items[k].SubItems.Add(A[t, 1].ToString());
                listView1.Items[k].SubItems.Add(A[t, 2].ToString());
                listView1.Items[k].SubItems.Add(A[t, 3].ToString());
                k++;
                t++;
            }

            MessageBox.Show("Proses Selesai", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            // MEMASUKA HASIL CLUSTER KE KE DATABASE DI MYSQL
            t = 1;
            while (t < jumdat)
            {
                nk = new kelasQuery();
                nk.QUERY("update hasil_cluster set cluster_fcm='" + A[t, 3] + "' where NPM='" + A[t, 4] + "'");
                t++;
            }

            //Menghitung jumlah cluster per mahasiswa
            double[] jumclusma = new double[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (i = 1; i <= c; i++)
            {
                for (j = 1; j < jumdat; j++)
                {
                    if (A[j, 3] == i)
                    {
                        jumclusma[i] = 1 + jumclusma[i];
                    }
                }
            }

            //MENGHITUNG CENTROID di masing2 cluster

            double[,] CENTROID = new double[10, 8];
            for(i=1;i<=8;i++)
            {
                for(j=0;j<=1;j++)
                {
                    CENTROID[i, j] = 0; //iniialisasi nilai semua centroid
                }
            }

            double[,] sumxy = new double[jumdat, 4];
            for (i = 1; i < jumdat; i++)
            {
                sumxy[Convert.ToInt32(A[i, 3]), 0] = A[i, 1] + sumxy[Convert.ToInt32(A[i, 3]), 0]; //Jumlah IPK di masing2 cluster
                sumxy[Convert.ToInt32(A[i, 3]), 1] = A[i, 2] + sumxy[Convert.ToInt32(A[i, 3]), 1]; //Jumlah Lama Studi di masing2 cluster
                sumxy[Convert.ToInt32(A[i, 3]), 2] = 1 + sumxy[Convert.ToInt32(A[i, 3]), 2]; //Jumlah Data  di masing2 cluster
            }
            for (i = 1; i <= c; i++)
            {
                CENTROID[i, 0] = ((sumxy[i, 0]) / (sumxy[i, 2])); //Rata2 IPK di masing2 cluster
                CENTROID[i, 1] = ((sumxy[i, 1]) / (sumxy[i, 2])); //Rata2 Lama Studi di masing2 cluster
            }
            label12.Text = "jumlah looping = " + (iter - 1) + "";
            label13.Text = "jumlah data = " + (jumdat - 1) + "";
            label23.Text = "" + jumclusma[1] + " , (" + Math.Round(CENTROID[1, 0], 2) + "),(" + Math.Round(CENTROID[1, 1], 2) + ")";
            label24.Text = "" + jumclusma[2] + " , (" + Math.Round(CENTROID[2, 0], 2) + "),(" + Math.Round(CENTROID[2, 1], 2) + ")";
            label25.Text = "" + jumclusma[3] + " , (" + Math.Round(CENTROID[3, 0], 2) + "),(" + Math.Round(CENTROID[3, 1], 2) + ")";
            label26.Text = "" + jumclusma[4] + " , (" + Math.Round(CENTROID[4, 0], 2) + "),(" + Math.Round(CENTROID[4, 1], 2) + ")";
            label27.Text = "" + jumclusma[5] + " , (" + Math.Round(CENTROID[5, 0], 2) + "),(" + Math.Round(CENTROID[5, 1], 2) + ")";
            label28.Text = "" + jumclusma[6] + " , (" + Math.Round(CENTROID[6, 0], 2) + "),(" + Math.Round(CENTROID[6, 1], 2) + ")";
            label29.Text = "" + jumclusma[7] + " , (" + Math.Round(CENTROID[7, 0], 2) + "),(" + Math.Round(CENTROID[7, 1], 2) + ")";
            label30.Text = "" + jumclusma[8] + " , (" + Math.Round(CENTROID[8, 0], 2) + "),(" + Math.Round(CENTROID[8, 1], 2) + ")";

        }

            
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Columns.Clear();
            listView1.Items.Clear();
            listView1.Columns.Add("npm");
            listView1.Columns.Add("nama");
            listView1.Columns.Add("ipk");
            listView1.Columns.Add("lama_studi");
            listView1.Columns.Add("cluster");

            koneksi.vKoneksiBuka();
            MySqlCommand myCmd = new MySqlCommand(queryfcm, koneksi.conn);
            MySqlDataReader myRead;
            myRead = myCmd.ExecuteReader();
            int i = 0;
            while (myRead.Read())
            {
                listView1.Items.Add(myRead.GetString(0)); //
                listView1.Items[i].SubItems.Add(myRead.GetString(1));
                listView1.Items[i].SubItems.Add(myRead.GetString(2));
                listView1.Items[i].SubItems.Add(myRead.GetString(3));
                listView1.Items[i].SubItems.Add(myRead.GetString(5));        
                i++;
            }

            koneksi.vKoneksiTutup();


        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            menuadmin input_fcm = new menuadmin();
            input_fcm.Show();
        }

        private void button1_Click(object sender, EventArgs e) //BUTTON UTK EKSEKUSI
        {
            if (Convert.ToInt32(textBox1.Text) >= 2 && Convert.ToInt32(textBox1.Text) <= 8)
            {
                try
                {
                    fcms();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Masukan Informasi dengan benar!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);               
                }
            }
               else
            {
                MessageBox.Show("Masukan Informasi dengan benar!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*listView1.Columns.Clear();
            listView1.Items.Clear();
            listView1.Columns.Add("npm");
            listView1.Columns.Add("nama");
            listView1.Columns.Add("ipk");
            listView1.Columns.Add("lama_studi");
            listView1.Columns.Add("cluster");*/
            koneksi.vKoneksiBuka();
            MySqlCommand myCmd = new MySqlCommand("select b.*, h.cluster_kmeans, h.cluster_fcm from biodata b INNER JOIN hasil_cluster h on h.NPM= b.NPM where h.NPM= '" + textBox4.Text + "'", koneksi.conn);
            MySqlDataReader myRead;
            myRead = myCmd.ExecuteReader();
            /*int i = 0;
            while (myRead.Read())
            {
                listView1.Items.Add(myRead.GetString(0)); //
                listView1.Items[i].SubItems.Add(myRead.GetString(1));
                listView1.Items[i].SubItems.Add(myRead.GetString(2));
                listView1.Items[i].SubItems.Add(myRead.GetString(5));
                listView1.Items[i].SubItems.Add(myRead.GetString(14));
            }*/
            myRead.Read();
            textBox5.Text = (myRead.GetString(1));
            textBox6.Text = (myRead.GetString(2));
            textBox7.Text = (myRead.GetString(5));
            textBox8.Text = (myRead.GetString(14));
        }

        private void button5_Click(object sender, EventArgs e) //UTK MENENTUKAN NILAI ACAK
        {
            
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
        



    }
}
