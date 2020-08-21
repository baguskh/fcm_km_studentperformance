using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace kfcm
{
    class konek
    {
        public MySqlConnection conn;
        public void vKoneksiBuka()
        {
            try
            {
                string stringconn = "server=localhost; uid=root;pwd=1234; database=data_mahasiswa";
                conn = new MySqlConnection(stringconn);
                conn.Open();
            }

            catch (Exception)
            {
                MessageBox.Show("Database disconected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                conn.Close();
            }
        }

        public void vKoneksiTutup()
        {
            conn.Close();
        }
    }
}
