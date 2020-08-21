using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace kfcm
{
    class kelasQuery
    {
         //Membangun koneksi ke database dbsample MySql
        MySqlConnection cn = new MySqlConnection("server=localhost;uid=root;pwd=1234;database=data_mahasiswa;");

        //perintah untuk mengeksekusi command
        public void QUERY(String sql)
        {
            //membuka koneksi
            cn.Open();
            try
            {
                MySqlCommand cm = new MySqlCommand(sql, cn);
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Query");
            }
            finally
            {
                //menutup koneksi
                cn.Close();
            }
        }
    }
}
