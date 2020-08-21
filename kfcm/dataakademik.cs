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
   
   

    public partial class dataakademik : Form
    {
        public dataakademik(int trufal)
        {
            
            InitializeComponent();
            if (trufal == 1)
            {
                button3.Enabled = true;
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
                
                keluar = trufal;                             
            }
            else if (trufal == 0)
            {
                button2.Enabled = false;
                button3.Enabled = false;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                
                keluar = trufal;
            }
           
        }
        int keluar;
        kelasQuery k;
        konek koneksi = new konek();
        string query = "select b.*, h.cluster_kmeans, h.cluster_fcm from biodata b INNER JOIN hasil_cluster h on h.NPM= b.NPM";
        string querymat = "select b.*, h.cluster_kmeans, h.cluster_fcm from biodata b INNER JOIN hasil_cluster h on h.NPM= b.NPM where LEFT(h.npm , 4)=1401";
        string querykim = "select b.*, h.cluster_kmeans, h.cluster_fcm from biodata b INNER JOIN hasil_cluster h on h.NPM= b.NPM where LEFT(h.npm , 4)=1402";
        string queryfis = "select b.*, h.cluster_kmeans, h.cluster_fcm from biodata b INNER JOIN hasil_cluster h on h.NPM= b.NPM where LEFT(h.npm , 4)=1403";
        string querybio = "select b.*, h.cluster_kmeans, h.cluster_fcm from biodata b INNER JOIN hasil_cluster h on h.NPM= b.NPM where LEFT(h.npm , 4)=1404";
        string querystat = "select b.*, h.cluster_kmeans, h.cluster_fcm from biodata b INNER JOIN hasil_cluster h on h.NPM= b.NPM where LEFT(h.npm , 4)=1406";

        private void button4_Click(object sender, EventArgs e)
        {
            if (keluar == 1)
            {
                this.Hide();
                menuadmin dataakademik = new menuadmin();
                dataakademik.Show();
            }
            else
            {
                this.Hide();
                Form1 dataakademik = new Form1();
                dataakademik.Show();
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[,] A = new double[100000, 5]; //y1 utk km y2 utk fcm y3 utk ipk y4 utk lmastudi
            //menghitung centroid
            double[,] cenkm = new double[9, 3];
            double[,] cenfcm = new double[9, 3];
            koneksi.vKoneksiBuka();
            MySqlCommand mycmdquery = new MySqlCommand(query, koneksi.conn);
            MySqlDataReader myReads;
            myReads = mycmdquery.ExecuteReader();
            int jumdat = 1;
            int i = 0;
            listView1.Items.Clear();
            while (myReads.Read())
            {

                A[i + 1, 1] = double.Parse(myReads.GetString(2)); //utk ipk
                A[i + 1, 2] = double.Parse(myReads.GetString(5)); //utk lmstd
                A[i + 1, 3] = double.Parse(myReads.GetString(13)); //utk km
                A[i + 1, 4] = double.Parse(myReads.GetString(14)); //utk fcm
                i++;
                jumdat++;
            }
            
            double[,] sumkm = new double[jumdat, 4];
            double[,] sumfcm = new double[jumdat, 4];
            for (i = 1; i < jumdat; i++)
            {
                sumkm[Convert.ToInt32(A[i, 3]), 1] = A[i, 1] + sumkm[Convert.ToInt32(A[i, 3]), 1]; //utkipk km
                sumkm[Convert.ToInt32(A[i, 3]), 2] = A[i, 2] + sumkm[Convert.ToInt32(A[i, 3]), 2]; //utklmastudi km
                sumkm[Convert.ToInt32(A[i, 3]), 3] = 1 + sumkm[Convert.ToInt32(A[i, 3]), 3]; //utk total km                
            }
            for (i = 1; i < jumdat; i++)
            {
                sumfcm[Convert.ToInt32(A[i, 4]), 1] = A[i, 1] + sumfcm[Convert.ToInt32(A[i, 4]), 1]; //utkipk fcm
                sumfcm[Convert.ToInt32(A[i, 4]), 2] = A[i, 2] + sumfcm[Convert.ToInt32(A[i, 4]), 2]; //utklmastudi fcm
                sumfcm[Convert.ToInt32(A[i, 4]), 3] = 1 + sumfcm[Convert.ToInt32(A[i, 4]), 3]; //utk total fcm             
            }
            for (i = 1; i <= 8; i++)
            {
                cenkm[i, 1] = ((sumkm[i, 1]) / (sumkm[i, 3]));
                cenkm[i, 2] = ((sumkm[i, 2]) / (sumkm[i, 3]));
            }
            for (i = 1; i <= 8; i++)
            {
                cenfcm[i, 1] = ((sumfcm[i, 1]) / (sumfcm[i, 3]));
                cenfcm[i, 2] = ((sumfcm[i, 2]) / (sumfcm[i, 3]));
            }

            listView1.Columns.Clear();
            listView1.Items.Clear();
            listView1.Columns.Add("npm");
            listView1.Columns.Add("nama");
            listView1.Columns.Add("ipk");
            listView1.Columns.Add("masuk");
            listView1.Columns.Add("lulus");
            listView1.Columns.Add("lama_studi");
            listView1.Columns.Add("JK");
            listView1.Columns.Add("tempat_lahir");
            listView1.Columns.Add("tanggal_lahir");
            listView1.Columns.Add("alamat");
            listView1.Columns.Add("no_telp");
            listView1.Columns.Add("kota asal sekolah");
            listView1.Columns.Add("sma");
            listView1.Columns.Add("clusterkmeans");
            listView1.Columns.Add("clusterfcm");

            double[] jumkm = new double[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] jumfcm = new double[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            
            int j = 0;
            int k = 0;
            i=0;
            if (comboBox1.SelectedItem.ToString() == "MATEMATIKA")
            {
                koneksi.vKoneksiBuka();
                MySqlCommand myCmd = new MySqlCommand(querymat, koneksi.conn);
                MySqlDataReader myRead;
                myRead = myCmd.ExecuteReader();
                i = 0;

                listView1.Items.Clear();
                while (myRead.Read())
                {

                    listView1.Items.Add(myRead.GetString(0)); //
                    listView1.Items[i].SubItems.Add(myRead.GetString(1));
                    listView1.Items[i].SubItems.Add(myRead.GetString(2));
                    listView1.Items[i].SubItems.Add(myRead.GetString(3));
                    listView1.Items[i].SubItems.Add(myRead.GetString(4));
                    listView1.Items[i].SubItems.Add(myRead.GetString(5));
                    listView1.Items[i].SubItems.Add(myRead.GetString(6));
                    listView1.Items[i].SubItems.Add(myRead.GetString(7));
                    listView1.Items[i].SubItems.Add(myRead.GetString(8));
                    listView1.Items[i].SubItems.Add(myRead.GetString(9));
                    listView1.Items[i].SubItems.Add(myRead.GetString(10));
                    listView1.Items[i].SubItems.Add(myRead.GetString(11));
                    listView1.Items[i].SubItems.Add(myRead.GetString(12));
                    listView1.Items[i].SubItems.Add(myRead.GetString(13));
                    listView1.Items[i].SubItems.Add(myRead.GetString(14));
                    A[i + 1, 1] = double.Parse(myRead.GetString(13));
                    A[i + 1, 2] = double.Parse(myRead.GetString(14));
                    i++;
                }
                koneksi.vKoneksiTutup();                
            }

            if (comboBox1.SelectedItem.ToString() == "KIMIA")
            {
                koneksi.vKoneksiBuka();
                MySqlCommand myCmd = new MySqlCommand(querykim, koneksi.conn);
                MySqlDataReader myRead;
                myRead = myCmd.ExecuteReader();
                i= 0;
                listView1.Items.Clear();
                while (myRead.Read())
                {
                    listView1.Items.Add(myRead.GetString(0)); //
                    listView1.Items[i].SubItems.Add(myRead.GetString(1));
                    listView1.Items[i].SubItems.Add(myRead.GetString(2));
                    listView1.Items[i].SubItems.Add(myRead.GetString(3));
                    listView1.Items[i].SubItems.Add(myRead.GetString(4));
                    listView1.Items[i].SubItems.Add(myRead.GetString(5));
                    listView1.Items[i].SubItems.Add(myRead.GetString(6));
                    listView1.Items[i].SubItems.Add(myRead.GetString(7));
                    listView1.Items[i].SubItems.Add(myRead.GetString(8));
                    listView1.Items[i].SubItems.Add(myRead.GetString(9));
                    listView1.Items[i].SubItems.Add(myRead.GetString(10));
                    listView1.Items[i].SubItems.Add(myRead.GetString(11));
                    listView1.Items[i].SubItems.Add(myRead.GetString(12));
                    listView1.Items[i].SubItems.Add(myRead.GetString(13));
                    listView1.Items[i].SubItems.Add(myRead.GetString(14));
                    A[i + 1, 1] = double.Parse(myRead.GetString(13));
                    A[i + 1, 2] = double.Parse(myRead.GetString(14));
                    i++;
                }

                koneksi.vKoneksiTutup();

            }

            if (comboBox1.SelectedItem.ToString() == "FISIKA")
            {
                koneksi.vKoneksiBuka();
                MySqlCommand myCmd = new MySqlCommand(queryfis, koneksi.conn);
                MySqlDataReader myRead;
                myRead = myCmd.ExecuteReader();
                i = 0;
                listView1.Items.Clear();
                while (myRead.Read())
                {
                    listView1.Items.Add(myRead.GetString(0)); //
                    listView1.Items[i].SubItems.Add(myRead.GetString(1));
                    listView1.Items[i].SubItems.Add(myRead.GetString(2));
                    listView1.Items[i].SubItems.Add(myRead.GetString(3));
                    listView1.Items[i].SubItems.Add(myRead.GetString(4));
                    listView1.Items[i].SubItems.Add(myRead.GetString(5));
                    listView1.Items[i].SubItems.Add(myRead.GetString(6));
                    listView1.Items[i].SubItems.Add(myRead.GetString(7));
                    listView1.Items[i].SubItems.Add(myRead.GetString(8));
                    listView1.Items[i].SubItems.Add(myRead.GetString(9));
                    listView1.Items[i].SubItems.Add(myRead.GetString(10));
                    listView1.Items[i].SubItems.Add(myRead.GetString(11));
                    listView1.Items[i].SubItems.Add(myRead.GetString(12));
                    listView1.Items[i].SubItems.Add(myRead.GetString(13));
                    listView1.Items[i].SubItems.Add(myRead.GetString(14));
                    A[i + 1, 1] = double.Parse(myRead.GetString(13));
                    A[i + 1, 2] = double.Parse(myRead.GetString(14));
                    i++;
                }

                koneksi.vKoneksiTutup();

            }

            if (comboBox1.SelectedItem.ToString() == "BIOLOGI")
            {
                koneksi.vKoneksiBuka();
                MySqlCommand myCmd = new MySqlCommand(querybio, koneksi.conn);
                MySqlDataReader myRead;
                myRead = myCmd.ExecuteReader();
                i = 0;
                listView1.Items.Clear();
                while (myRead.Read())
                {
                    listView1.Items.Add(myRead.GetString(0)); //
                    listView1.Items[i].SubItems.Add(myRead.GetString(1));
                    listView1.Items[i].SubItems.Add(myRead.GetString(2));
                    listView1.Items[i].SubItems.Add(myRead.GetString(3));
                    listView1.Items[i].SubItems.Add(myRead.GetString(4));
                    listView1.Items[i].SubItems.Add(myRead.GetString(5));
                    listView1.Items[i].SubItems.Add(myRead.GetString(6));
                    listView1.Items[i].SubItems.Add(myRead.GetString(7));
                    listView1.Items[i].SubItems.Add(myRead.GetString(8));
                    listView1.Items[i].SubItems.Add(myRead.GetString(9));
                    listView1.Items[i].SubItems.Add(myRead.GetString(10));
                    listView1.Items[i].SubItems.Add(myRead.GetString(11));
                    listView1.Items[i].SubItems.Add(myRead.GetString(12));
                    listView1.Items[i].SubItems.Add(myRead.GetString(13));
                    listView1.Items[i].SubItems.Add(myRead.GetString(14));
                    A[i + 1, 1] = double.Parse(myRead.GetString(13));
                    A[i + 1, 2] = double.Parse(myRead.GetString(14));
                    i++;
                }

                koneksi.vKoneksiTutup();

            }

            if (comboBox1.SelectedItem.ToString() == "STATISTIKA")
            {
                koneksi.vKoneksiBuka();
                MySqlCommand myCmd = new MySqlCommand(querystat, koneksi.conn);
                MySqlDataReader myRead;
                myRead = myCmd.ExecuteReader();
                i = 0;
                listView1.Items.Clear();
                while (myRead.Read())
                {
                    listView1.Items.Add(myRead.GetString(0)); //
                    listView1.Items[i].SubItems.Add(myRead.GetString(1));
                    listView1.Items[i].SubItems.Add(myRead.GetString(2));
                    listView1.Items[i].SubItems.Add(myRead.GetString(3));
                    listView1.Items[i].SubItems.Add(myRead.GetString(4));
                    listView1.Items[i].SubItems.Add(myRead.GetString(5));
                    listView1.Items[i].SubItems.Add(myRead.GetString(6));
                    listView1.Items[i].SubItems.Add(myRead.GetString(7));
                    listView1.Items[i].SubItems.Add(myRead.GetString(8));
                    listView1.Items[i].SubItems.Add(myRead.GetString(9));
                    listView1.Items[i].SubItems.Add(myRead.GetString(10));
                    listView1.Items[i].SubItems.Add(myRead.GetString(11));
                    listView1.Items[i].SubItems.Add(myRead.GetString(12));
                    listView1.Items[i].SubItems.Add(myRead.GetString(13));
                    listView1.Items[i].SubItems.Add(myRead.GetString(14));
                    A[i + 1, 1] = double.Parse(myRead.GetString(13));
                    A[i + 1, 2] = double.Parse(myRead.GetString(14));
                    i++;
                }

                koneksi.vKoneksiTutup();

            }
            if (comboBox1.SelectedItem.ToString() == "SEMUA")
            {
                koneksi.vKoneksiBuka();
                MySqlCommand myCmd = new MySqlCommand(query, koneksi.conn);
                MySqlDataReader myRead;
                myRead = myCmd.ExecuteReader();
                i = 0;
                listView1.Items.Clear();
                while (myRead.Read())
                {
                    listView1.Items.Add(myRead.GetString(0)); //
                    listView1.Items[i].SubItems.Add(myRead.GetString(1));
                    listView1.Items[i].SubItems.Add(myRead.GetString(2));
                    listView1.Items[i].SubItems.Add(myRead.GetString(3));
                    listView1.Items[i].SubItems.Add(myRead.GetString(4));
                    listView1.Items[i].SubItems.Add(myRead.GetString(5));
                    listView1.Items[i].SubItems.Add(myRead.GetString(6));
                    listView1.Items[i].SubItems.Add(myRead.GetString(7));
                    listView1.Items[i].SubItems.Add(myRead.GetString(8));
                    listView1.Items[i].SubItems.Add(myRead.GetString(9));
                    listView1.Items[i].SubItems.Add(myRead.GetString(10));
                    listView1.Items[i].SubItems.Add(myRead.GetString(11));
                    listView1.Items[i].SubItems.Add(myRead.GetString(12));
                    listView1.Items[i].SubItems.Add(myRead.GetString(13));
                    listView1.Items[i].SubItems.Add(myRead.GetString(14));
                    A[i + 1, 1] = double.Parse(myRead.GetString(13));
                    A[i + 1, 2] = double.Parse(myRead.GetString(14));
                    i++;
                }

                koneksi.vKoneksiTutup();

            }
            label14.Text = "Jumlah Data = " + i + "";
            // menghitung jumlah mahasiswa per cluster
            for (j = 1; j <= 9; j++)
            {
                for (k = 1; k < i + 1; k++)
                {
                    if (A[k, 1] == j)
                    {
                        jumkm[j] = 1 + jumkm[j];
                    }
                    if (A[k, 2] == j)
                    {
                        jumfcm[j] = 1 + jumfcm[j];
                    }
                }
            }

            
            label27.Text = jumkm[1].ToString(); //utk jumlah mahasiswa percluster km
            label28.Text = jumkm[2].ToString();
            label29.Text = jumkm[3].ToString();
            label30.Text = jumkm[4].ToString();
            label31.Text = jumkm[5].ToString();
            label32.Text = jumkm[6].ToString();
            label33.Text = jumkm[7].ToString();
            label34.Text = jumkm[8].ToString();
            label35.Text = jumfcm[1].ToString();
            label36.Text = jumfcm[2].ToString(); //utk jumlah mahasiswa percluster fm
            label37.Text = jumfcm[3].ToString();
            label38.Text = jumfcm[4].ToString();
            label39.Text = jumfcm[5].ToString();
            label40.Text = jumfcm[6].ToString();
            label41.Text = jumfcm[7].ToString();
            label42.Text = jumfcm[8].ToString();
            label53.Text = "(" + Math.Round(cenkm[1, 1], 2) + "),(" + Math.Round(cenkm[1, 2], 2) + ")"; //utk centroid km
            label54.Text = "(" + Math.Round(cenkm[2, 1], 2) + "),(" + Math.Round(cenkm[2, 2], 2) + ")";
            label55.Text = "(" + Math.Round(cenkm[3, 1], 2) + "),(" + Math.Round(cenkm[3, 2], 2) + ")";
            label56.Text = "(" + Math.Round(cenkm[4, 1], 2) + "),(" + Math.Round(cenkm[4, 2], 2) + ")";
            label57.Text = "(" + Math.Round(cenkm[5, 1], 2) + "),(" + Math.Round(cenkm[5, 2], 2) + ")";
            label58.Text = "(" + Math.Round(cenkm[6, 1], 2) + "),(" + Math.Round(cenkm[6, 2], 2) + ")";
            label59.Text = "(" + Math.Round(cenkm[7, 1], 2) + "),(" + Math.Round(cenkm[7, 2], 2) + ")";
            label60.Text = "(" + Math.Round(cenkm[8, 1], 2) + "),(" + Math.Round(cenkm[8, 2], 2) + ")";
            label61.Text = "(" + Math.Round(cenfcm[1, 1], 2) + "),(" + Math.Round(cenfcm[1, 2], 2) + ")"; //utk centroid fcm
            label62.Text = "(" + Math.Round(cenfcm[2, 1], 2) + "),(" + Math.Round(cenfcm[2, 2], 2) + ")";
            label63.Text = "(" + Math.Round(cenfcm[3, 1], 2) + "),(" + Math.Round(cenfcm[3, 2], 2) + ")";
            label64.Text = "(" + Math.Round(cenfcm[4, 1], 2) + "),(" + Math.Round(cenfcm[4, 2], 2) + ")";
            label65.Text = "(" + Math.Round(cenfcm[5, 1], 2) + "),(" + Math.Round(cenfcm[5, 2], 2) + ")";
            label66.Text = "(" + Math.Round(cenfcm[6, 1], 2) + "),(" + Math.Round(cenfcm[6, 2], 2) + ")";
            label67.Text = "(" + Math.Round(cenfcm[7, 1], 2) + "),(" + Math.Round(cenfcm[7, 2], 2) + ")";
            label68.Text = "(" + Math.Round(cenfcm[8, 1], 2) + "),(" + Math.Round(cenfcm[8, 2], 2) + ")";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                k = new kelasQuery();
                k.QUERY("insert into biodata(NPM,Nama,IPK,Tanggal_Masuk,Tanggal_Lulus,Lama_Studi,Jenis_Kelamin,Tempat_Lahir,Tanggal_Lahir,Alamat,No_Telpon,Asal_Sekolah,SMA) values('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "', '" + textBox6.Text + "', '" + textBox7.Text + "', '" + textBox8.Text + "', '" + textBox9.Text + "', '" + textBox10.Text + "', '" + textBox11.Text + "', '" + textBox12.Text + "', '" + textBox13.Text + "')");
                k.QUERY("update hasil_cluster set cluster_kmeans = 0 ,cluster_fcm = 0 where NPM = '" + textBox1.Text + "'");
                MessageBox.Show("Insert Berhasil");
            }
            else if(radioButton3.Checked == true)
            {
                k = new kelasQuery();
                k.QUERY("update biodata set Nama='" + textBox2.Text + "', IPK='" + textBox3.Text + "', Tanggal_Masuk='" + textBox4.Text + "', Tanggal_Lulus='" + textBox5.Text + "', Lama_Studi='" + textBox6.Text + "', Jenis_Kelamin='" + textBox7.Text + "', Tempat_Lahir='" + textBox8.Text + "', Tanggal_Lahir='" + textBox9.Text + "' ,Alamat='" + textBox10.Text + "', No_Telpon='" + textBox11.Text + "', Asal_Sekolah='" + textBox12.Text + "', SMA='" + textBox13.Text + "' where NPM='" + textBox1.Text + "'");
                MessageBox.Show("Update Berhasil");
            }
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            k = new kelasQuery();
            k.QUERY("delete from biodata where NPM = '"+ textBox1.Text +"'");
            MessageBox.Show("delet Berhasil");
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listView1.Columns.Clear();
            listView1.Items.Clear();
            listView1.Columns.Add("npm");
            listView1.Columns.Add("nama");
            listView1.Columns.Add("ipk");
            listView1.Columns.Add("masuk");
            listView1.Columns.Add("lulus");
            listView1.Columns.Add("lama_studi");
            listView1.Columns.Add("JK");
            listView1.Columns.Add("tempat_lahir");
            listView1.Columns.Add("tanggal_lahir");
            listView1.Columns.Add("alamat");
            listView1.Columns.Add("no_telp");
            listView1.Columns.Add("kota asal sekolah");
            listView1.Columns.Add("sma"); 
            listView1.Columns.Add("cluster km");
            listView1.Columns.Add("cluster fcm");


            koneksi.vKoneksiBuka();
            MySqlCommand myCmd = new MySqlCommand("select b.*, h.cluster_kmeans, h.cluster_fcm from biodata b INNER JOIN hasil_cluster h on h.NPM= b.NPM where h.NPM= '" + textBox15.Text + "'", koneksi.conn);
            MySqlDataReader myRead;
            myRead = myCmd.ExecuteReader();
            int i = 0;
            while (myRead.Read())
            {
                listView1.Items.Add(myRead.GetString(0)); //
                listView1.Items[i].SubItems.Add(myRead.GetString(1));
                listView1.Items[i].SubItems.Add(myRead.GetString(2));
                listView1.Items[i].SubItems.Add(myRead.GetString(3));
                listView1.Items[i].SubItems.Add(myRead.GetString(4));
                listView1.Items[i].SubItems.Add(myRead.GetString(5));
                listView1.Items[i].SubItems.Add(myRead.GetString(6));
                listView1.Items[i].SubItems.Add(myRead.GetString(7));
                listView1.Items[i].SubItems.Add(myRead.GetString(8));
                listView1.Items[i].SubItems.Add(myRead.GetString(9));
                listView1.Items[i].SubItems.Add(myRead.GetString(10));
                listView1.Items[i].SubItems.Add(myRead.GetString(11));
                listView1.Items[i].SubItems.Add(myRead.GetString(12));
                listView1.Items[i].SubItems.Add(myRead.GetString(13));
                listView1.Items[i].SubItems.Add(myRead.GetString(14));
                textBox1.Text=(myRead.GetString(0));
                textBox2.Text=(myRead.GetString(1));
                textBox3.Text=(myRead.GetString(2));
                textBox4.Text=(myRead.GetString(3));
                textBox5.Text=(myRead.GetString(4));
                textBox6.Text=(myRead.GetString(5));
                textBox7.Text=(myRead.GetString(6));
                textBox8.Text=(myRead.GetString(7));
                textBox9.Text=(myRead.GetString(8));
                textBox10.Text=(myRead.GetString(9));
                textBox11.Text=(myRead.GetString(10));
                textBox12.Text=(myRead.GetString(11));
                textBox13.Text=(myRead.GetString(12));           
            }


            koneksi.vKoneksiTutup();
        }

        private void button7_Click(object sender, EventArgs e)
        {
           

        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            textBox6.ReadOnly = true;
            textBox7.ReadOnly = true;
            textBox8.ReadOnly = true;
            textBox9.ReadOnly = true;
            textBox10.ReadOnly = true;
            textBox11.ReadOnly = true;
            textBox12.ReadOnly = true;
            textBox13.ReadOnly = true;
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 dataakademik = new Form1();
            dataakademik.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                label15.Visible = false;
                label16.Visible = false;
                button2.Enabled = false; 
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
                textBox5.ReadOnly = false;
                textBox6.ReadOnly = false;
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                textBox9.ReadOnly = false;
                textBox10.ReadOnly = false;
                textBox11.ReadOnly = false;
                textBox12.ReadOnly = false;
                textBox13.ReadOnly = false;
                label15.Visible = true;
                label16.Visible = false;
                button2.Enabled = true; 
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                textBox2.ReadOnly = false;
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
                textBox5.ReadOnly = false;
                textBox6.ReadOnly = false;
                textBox7.ReadOnly = false;
                textBox8.ReadOnly = false;
                textBox9.ReadOnly = false;
                textBox10.ReadOnly = false;
                textBox11.ReadOnly = false;
                textBox12.ReadOnly = false;
                textBox13.ReadOnly = false;
                label15.Visible = false;
                label16.Visible = true;
                button2.Enabled = true;
            }
        }
    }
}
