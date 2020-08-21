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
    public partial class input_km : Form
    {
        public input_km()
        {
            InitializeComponent();
        }

        konek koneksi = new konek();
        string query = "select * from biodata";
        string queryfcm = "SELECT b.NPM, b.Nama, b.IPK, b.Lama_Studi, b.Asal_Sekolah, h.cluster_kmeans FROM biodata b INNER JOIN hasil_cluster h ON h.NPM= b.NPM";
        kelasQuery nk;

        public void kms()
        {
            string query = "select * from biodata";
            string queryfcm = "SELECT b.NPM, b.Nama, b.IPK, b.Lama_Studi, b.Asal_Sekolah, h.cluster_kmeans FROM biodata b INNER JOIN hasil_cluster h ON h.NPM= b.NPM";
            kelasQuery nk;
            konek koneksi = new konek();
            int i, j;
            // PROSES K-MEANS BEGIN

            // MENGHITUNG ARRAY YANG DIBUTUHKAN

            int jumdat = 1;
            int jumcen = Convert.ToInt32(textBox1.Text);
            koneksi.vKoneksiBuka();
            MySqlCommand myCm = new MySqlCommand(query, koneksi.conn);
            MySqlDataReader myRea;
            myRea = myCm.ExecuteReader();
            while (myRea.Read())
            {
                jumdat++;
            }
            koneksi.vKoneksiTutup();
            double[,] A = new double[jumdat, 4]; //jumlah data = jumdat-1
            label20.Text = (jumdat - 1).ToString();

            // MEMASUKAN DATA KEDALAM MATRIKS
            koneksi.vKoneksiBuka();
            MySqlCommand myCmd = new MySqlCommand(query, koneksi.conn);
            MySqlDataReader myRead;
            myRead = myCmd.ExecuteReader();
            int ia = 1;
            while (myRead.Read())
            {
                A[ia, 0] = double.Parse(myRead.GetString(2));
                A[ia, 1] = double.Parse(myRead.GetString(5));
                A[ia, 3] = double.Parse(myRead.GetString(0));// buat npm
                ia++;
            }
            koneksi.vKoneksiTutup();

            //Masukan Centroid ke sistem
            double[,] CENTROID = new double[10, 8];                       
            CENTROID[1, 0] = double.Parse(textBox7.Text);
            CENTROID[1, 1] = double.Parse(textBox8.Text);
            CENTROID[2, 0] = double.Parse(textBox9.Text);
            CENTROID[2, 1] = double.Parse(textBox10.Text);
            CENTROID[3, 0] = double.Parse(textBox11.Text);
            CENTROID[3, 1] = double.Parse(textBox12.Text);
            CENTROID[4, 0] = double.Parse(textBox13.Text);
            CENTROID[4, 1] = double.Parse(textBox14.Text);
            CENTROID[5, 0] = double.Parse(textBox15.Text);
            CENTROID[5, 1] = double.Parse(textBox16.Text);
            CENTROID[6, 0] = double.Parse(textBox17.Text);
            CENTROID[6, 1] = double.Parse(textBox18.Text);
            CENTROID[7, 0] = double.Parse(textBox19.Text);
            CENTROID[7, 1] = double.Parse(textBox20.Text);
            CENTROID[8, 0] = double.Parse(textBox21.Text);
            CENTROID[8, 1] = double.Parse(textBox22.Text);
            
            
            double x, y;
            double min;
            int cluster = 0;
            double d;
            double jarak_ipk;
            double jarak_lmstd;
            double[,] SUMXY = new double[jumdat + 1, 3];

            for (i = 1; i < jumdat; i++) //menghitung jarak dari data pertama sampai data terakhir
            {

                min = 100000; //nilai awal jarak minimum
                x = A[i, 0]; // Nilai IPK objek data ke-i
                y = A[i, 1]; // Nilai Lama Studi objek data ke-i
                for (j = 1; j <= jumcen; j++) // menghitung jarak objek data ke-i dengan cluster pertama sampai terakhir
                {
                    jarak_ipk = Math.Pow(Math.Abs(x - CENTROID[j, 0]), 2);
                    jarak_lmstd = Math.Pow(Math.Abs(y - CENTROID[j, 1]), 2);
                    d = Math.Sqrt(jarak_ipk + jarak_lmstd); //Hasil jarak data objek data ke-i dengan centroid cluster ke-j
                    if (d < min)      // Jika jarak jarak data objek data ke-i dengan centroid cluster ke-j lebih kecil dari crntroid cluster sebelumnya
                    {                 
                        min = d;      // jarak terkecil akan dimasukan jarak proses perhitungan sebelumnya
                        cluster = j;  // variabel cluster akan diisi oleh nilai j
                    } 
                }
                A[i, 2] = cluster; //Data akan dimasukan ke dalam cluster dengan nulai sebesar variabel cluster
            }

            //Iterasi Ke-2

            bool IsStillMoving;
            IsStillMoving = true;
            int itung = 1;
            do
            {

                itung = 1 + itung;
                double[,] sumxy = new double[jumdat, 4];
                for (i = 1; i < jumdat; i++)
                {
                    sumxy[Convert.ToInt32(A[i, 2]), 0] = A[i, 0] + sumxy[Convert.ToInt32(A[i, 2]), 0]; //Jumlah IPK di masing2 cluster
                    sumxy[Convert.ToInt32(A[i, 2]), 1] = A[i, 1] + sumxy[Convert.ToInt32(A[i, 2]), 1]; //Jumlah Lama Studi di masing2 cluster
                    sumxy[Convert.ToInt32(A[i, 2]), 2] = 1 + sumxy[Convert.ToInt32(A[i, 2]), 2]; //Jumlah Data  di masing2 cluster
                }
                for (i = 1; i <= jumcen; i++)
                {
                    CENTROID[i, 0] = ((sumxy[i, 0]) / (sumxy[i, 2])); //Rata2 IPK di masing2 cluster
                    CENTROID[i, 1] = ((sumxy[i, 1]) / (sumxy[i, 2])); //Rata2 Lama Studi di masing2 cluster
                }
                IsStillMoving = false;
                for (i = 1; i < jumdat; i++)
                {
                    min = 100000;
                    x = A[i, 0];
                    y = A[i, 1];
                    for (j = 1; j <= jumcen; j++)
                    {
                        jarak_ipk = Math.Pow(Math.Abs(x - CENTROID[j, 0]), 2);
                        jarak_lmstd = Math.Pow(Math.Abs(y - CENTROID[j, 1]), 2);
                        d = Math.Sqrt(jarak_ipk + jarak_lmstd);
                        if (d < min)
                        {
                            min = d;
                            cluster = j;
                        }
                    }
                    if (A[i, 2] != cluster)
                    {
                        A[i, 2] = cluster;
                        IsStillMoving = true;
                    }
                }
                itung++;
                label20.Text = "dilooping sebanyak = ";
                label21.Text = itung.ToString();
            } while (IsStillMoving);

            MessageBox.Show("Proses Selesai", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
            // Masukan ke tabel
            listView1.Columns.Clear();
            listView1.Items.Clear();
            listView1.Columns.Add("NPM");
            listView1.Columns.Add("IPK");
            listView1.Columns.Add("Lama Studi");
            listView1.Columns.Add("Cluster");
            
            int k = 0;
            int t = 1;
            while (k < jumdat - 1)
            {

                listView1.Items.Add(A[t, 3].ToString()); //NPM
                listView1.Items[k].SubItems.Add(A[t, 0].ToString()); //IPK
                listView1.Items[k].SubItems.Add(A[t, 1].ToString()); //Lama Studi
                listView1.Items[k].SubItems.Add(A[t, 2].ToString()); //Cluster
                k++;
                t++;
            }

            // BACKUP KE MYSQL

            t = 1;
            while (t < jumdat)
            {
                nk = new kelasQuery();
                nk.QUERY("update hasil_cluster set cluster_kmeans='" + A[t, 2] + "' where NPM='" + A[t, 3] + "'");
                t++;
            }

            //MENAMPILKAN jumlah cluster per mahasiswa
            double[] jumclusma = new double[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (i = 1; i <= jumcen; i++)
            {
                for (j = 1; j < jumdat; j++)
                {
                    if (A[j, 2] == i)
                    {
                        jumclusma[i] = 1 + jumclusma[i]; //ditambah jika cluster sesuai
                    }
                }
            }

            label29.Text = "" + jumclusma[1] + " , (" + Math.Round(CENTROID[1, 0], 2) + "),(" + Math.Round(CENTROID[1, 1], 2) + ")";
            label30.Text = "" + jumclusma[2] + " , (" + Math.Round(CENTROID[2, 0], 2) + "),(" + Math.Round(CENTROID[2, 1], 2) + ")";
            label31.Text = "" + jumclusma[3] + " , (" + Math.Round(CENTROID[3, 0], 2) + "),(" + Math.Round(CENTROID[3, 1], 2) + ")";
            label32.Text = "" + jumclusma[4] + " , (" + Math.Round(CENTROID[4, 0], 2) + "),(" + Math.Round(CENTROID[4, 1], 2) + ")";
            label33.Text = "" + jumclusma[5] + " , (" + Math.Round(CENTROID[5, 0], 2) + "),(" + Math.Round(CENTROID[5, 1], 2) + ")";
            label34.Text = "" + jumclusma[6] + " , (" + Math.Round(CENTROID[6, 0], 2) + "),(" + Math.Round(CENTROID[6, 1], 2) + ")";
            label35.Text = "" + jumclusma[7] + " , (" + Math.Round(CENTROID[7, 0], 2) + "),(" + Math.Round(CENTROID[7, 1], 2) + ")";
            label36.Text = "" + jumclusma[8] + " , (" + Math.Round(CENTROID[8, 0], 2) + "),(" + Math.Round(CENTROID[8, 1], 2) + ")";
        }


        private void input_km_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                kms();
            }
            catch (FormatException)
            {
                MessageBox.Show("Masukan Informasi dengan benar!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);               
            }       
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            button7.Enabled = true;
            if (textBox1.Text == "2")
            {
                textBox7.Text = 2.9.ToString();
                textBox8.Text = 4.1.ToString();
                textBox9.Text = 3.3.ToString();
                textBox10.Text = 3.9.ToString();
                textBox11.Text = 0.ToString();
                textBox12.Text = 0.ToString();
                textBox13.Text = 0.ToString();
                textBox14.Text = 0.ToString();
                textBox15.Text = 0.ToString();
                textBox16.Text = 0.ToString();
                textBox17.Text = 0.ToString();
                textBox18.Text = 0.ToString();
                textBox19.Text = 0.ToString();
                textBox20.Text = 0.ToString();
                textBox21.Text = 0.ToString();
                textBox22.Text = 0.ToString();
            }
            else if (textBox1.Text == "3")
            {
                textBox7.Text = 2.9.ToString();
                textBox8.Text = 4.ToString();
                textBox9.Text = 3.3.ToString();
                textBox10.Text = 4.1.ToString();
                textBox11.Text = 3.3.ToString();
                textBox12.Text = 3.9.ToString();
                textBox13.Text = 0.ToString();
                textBox14.Text = 0.ToString();
                textBox15.Text = 0.ToString();
                textBox16.Text = 0.ToString();
                textBox17.Text = 0.ToString();
                textBox18.Text = 0.ToString();
                textBox19.Text = 0.ToString();
                textBox20.Text = 0.ToString();
                textBox21.Text = 0.ToString();
                textBox22.Text = 0.ToString();          
            }
            else if (textBox1.Text == "4")
            {
                textBox7.Text = 2.9.ToString();
                textBox8.Text = 3.9.ToString();
                textBox9.Text = 2.9.ToString();
                textBox10.Text = 4.1.ToString();
                textBox11.Text = 3.1.ToString();
                textBox12.Text = 3.9.ToString();
                textBox13.Text = 3.1.ToString();
                textBox14.Text = 4.1.ToString();
                textBox15.Text = 0.ToString();
                textBox16.Text = 0.ToString();
                textBox17.Text = 0.ToString();
                textBox18.Text = 0.ToString();
                textBox19.Text = 0.ToString();
                textBox20.Text = 0.ToString();
                textBox21.Text = 0.ToString();
                textBox22.Text = 0.ToString();
            }
            else if (textBox1.Text == "5")
            {
                textBox7.Text = 2.9.ToString();
                textBox8.Text = 3.9.ToString();
                textBox9.Text = 2.9.ToString();
                textBox10.Text = 4.1.ToString();
                textBox11.Text = 3.3.ToString();
                textBox12.Text = 3.9.ToString();
                textBox13.Text = 3.3.ToString();
                textBox14.Text = 4.1.ToString();
                textBox15.Text = 3.ToString();
                textBox16.Text = 4.ToString();
                textBox17.Text = 0.ToString();
                textBox18.Text = 0.ToString();
                textBox19.Text = 0.ToString();
                textBox20.Text = 0.ToString();
                textBox21.Text = 0.ToString();
                textBox22.Text = 0.ToString();

            }
            else if (textBox1.Text == "6")
            {
                textBox7.Text = 2.9.ToString();
                textBox8.Text = 3.9.ToString();
                textBox9.Text = 2.9.ToString();
                textBox10.Text = 4.1.ToString();
                textBox11.Text = 3.2.ToString();
                textBox12.Text = 3.9.ToString();
                textBox13.Text = 3.2.ToString();
                textBox14.Text = 4.1.ToString();
                textBox15.Text = 3.45.ToString();
                textBox16.Text = 3.9.ToString();
                textBox17.Text = 3.45.ToString();
                textBox18.Text = 4.1.ToString();
                textBox19.Text = 0.ToString();
                textBox20.Text = 0.ToString();
                textBox21.Text = 0.ToString();
                textBox22.Text = 0.ToString();
            }
            else if (textBox1.Text == "7")
            {

                textBox7.Text = 2.9.ToString();
                textBox8.Text = 3.9.ToString();
                textBox9.Text = 2.9.ToString();
                textBox10.Text = 4.1.ToString();
                textBox11.Text = 3.2.ToString();
                textBox12.Text = 3.9.ToString();
                textBox13.Text = 3.2.ToString();
                textBox14.Text = 4.1.ToString();
                textBox15.Text = 3.45.ToString();
                textBox16.Text = 3.9.ToString();
                textBox17.Text = 3.45.ToString();
                textBox18.Text = 4.1.ToString();
                textBox19.Text = 3.ToString();
                textBox20.Text = 4.ToString();
                textBox21.Text = 0.ToString();
                textBox22.Text = 0.ToString();
            }
            else if (textBox1.Text == "8")
            {

                textBox7.Text = 2.9.ToString();
                textBox8.Text = 3.9.ToString();
                textBox9.Text = 2.9.ToString();
                textBox10.Text = 4.1.ToString();
                textBox11.Text = 3.1.ToString();
                textBox12.Text = 3.9.ToString();
                textBox13.Text = 3.1.ToString();
                textBox14.Text = 4.1.ToString();
                textBox15.Text = 3.3.ToString();
                textBox16.Text = 3.9.ToString();
                textBox17.Text = 3.3.ToString();
                textBox18.Text = 4.1.ToString();
                textBox19.Text = 3.45.ToString();
                textBox20.Text = 3.9.ToString();
                textBox21.Text = 3.45.ToString();
                textBox22.Text = 4.1.ToString();
            }
            else
            {
                MessageBox.Show("Masukan antara 2-8 !!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            menuadmin input_km = new menuadmin();
            input_km.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox7.ReadOnly = true;
            textBox8.ReadOnly = true;
            textBox9.ReadOnly = true;
            textBox10.ReadOnly = true;
            textBox11.ReadOnly = true;
            textBox12.ReadOnly = true;
            textBox13.ReadOnly = true;
            textBox14.ReadOnly = true;
            textBox15.ReadOnly = true;
            textBox16.ReadOnly = true;
            textBox17.ReadOnly = true;
            textBox18.ReadOnly = true;
            textBox19.ReadOnly = true;
            textBox20.ReadOnly = true;
            textBox21.ReadOnly = true;
            textBox22.ReadOnly = true;
            textBox1.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox15.Clear();
            textBox16.Clear();
            textBox17.Clear();
            textBox18.Clear();
            textBox19.Clear();
            textBox20.Clear();
            textBox21.Clear();
            textBox22.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Enabled = true;
            if (textBox1.Text == "2")
            {
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                textBox9.ReadOnly = false;
                textBox10.ReadOnly = false;
            }
            else if (textBox1.Text == "3")
            {
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                textBox9.ReadOnly = false;
                textBox10.ReadOnly = false;
                textBox11.ReadOnly = false;
                textBox12.ReadOnly = false;

            }
            else if (textBox1.Text == "4")
            {
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                textBox9.ReadOnly = false;
                textBox10.ReadOnly = false;
                textBox11.ReadOnly = false;
                textBox12.ReadOnly = false;
                textBox13.ReadOnly = false;
                textBox14.ReadOnly = false;
            }
            else if (textBox1.Text == "5")
            {
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                textBox9.ReadOnly = false;
                textBox10.ReadOnly = false;
                textBox11.ReadOnly = false;
                textBox12.ReadOnly = false;
                textBox13.ReadOnly = false;
                textBox14.ReadOnly = false;
                textBox15.ReadOnly = false;
                textBox16.ReadOnly = false;

            }
            else if (textBox1.Text == "6")
            {
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                textBox9.ReadOnly = false;
                textBox10.ReadOnly = false;
                textBox11.ReadOnly = false;
                textBox12.ReadOnly = false;
                textBox13.ReadOnly = false;
                textBox14.ReadOnly = false;
                textBox15.ReadOnly = false;
                textBox16.ReadOnly = false;
                textBox17.ReadOnly = false;
                textBox18.ReadOnly = false;
            }
            else if (textBox1.Text == "7")
            {
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                textBox9.ReadOnly = false;
                textBox10.ReadOnly = false;
                textBox11.ReadOnly = false;
                textBox12.ReadOnly = false;
                textBox13.ReadOnly = false;
                textBox14.ReadOnly = false;
                textBox15.ReadOnly = false;
                textBox16.ReadOnly = false;
                textBox17.ReadOnly = false;
                textBox18.ReadOnly = false;
                textBox19.ReadOnly = false;
                textBox20.ReadOnly = false;
            }
            else if (textBox1.Text == "8")
            {
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                textBox9.ReadOnly = false;
                textBox10.ReadOnly = false;
                textBox11.ReadOnly = false;
                textBox12.ReadOnly = false;
                textBox13.ReadOnly = false;
                textBox14.ReadOnly = false;
                textBox15.ReadOnly = false;
                textBox16.ReadOnly = false;
                textBox17.ReadOnly = false;
                textBox18.ReadOnly = false;
                textBox19.ReadOnly = false;
                textBox20.ReadOnly = false;
                textBox21.ReadOnly = false;
                textBox22.ReadOnly = false;
            }
            else
            {
                MessageBox.Show("Masukan antara 2-8 !!");
            }
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
            int jumdat = 0;
            while (myRead.Read())
            {
                listView1.Items.Add(myRead.GetString(0)); //
                listView1.Items[i].SubItems.Add(myRead.GetString(1));
                listView1.Items[i].SubItems.Add(myRead.GetString(2));
                listView1.Items[i].SubItems.Add(myRead.GetString(3));
                listView1.Items[i].SubItems.Add(myRead.GetString(5));
                i++;
                jumdat++;
            }
            label21.Text = jumdat.ToString();
            koneksi.vKoneksiTutup();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Columns.Clear();
            listView1.Items.Clear();
            listView1.Columns.Add("npm");
            listView1.Columns.Add("nama");
            listView1.Columns.Add("ipk");
            listView1.Columns.Add("lama_studi");
            listView1.Columns.Add("cluster");
            koneksi.vKoneksiBuka();
            MySqlCommand myCmd = new MySqlCommand("select b.*, h.cluster_kmeans, h.cluster_fcm from biodata b INNER JOIN hasil_cluster h on h.NPM= b.NPM where h.NPM= '" + textBox2.Text + "'", koneksi.conn);
            MySqlDataReader myRead;
            myRead = myCmd.ExecuteReader();
            int i = 0;
            while (myRead.Read())
            {
                listView1.Items.Add(myRead.GetString(0)); //
                listView1.Items[i].SubItems.Add(myRead.GetString(1));
                listView1.Items[i].SubItems.Add(myRead.GetString(2));
                listView1.Items[i].SubItems.Add(myRead.GetString(5));
                listView1.Items[i].SubItems.Add(myRead.GetString(13));

                textBox3.Text = (myRead.GetString(1));
                textBox4.Text = (myRead.GetString(2));
                textBox5.Text = (myRead.GetString(5));
                textBox6.Text = (myRead.GetString(13));

            }

        }
    }
}
