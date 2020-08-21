using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kfcm
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void buttonlogin_Click(object sender, EventArgs e)
        {
            if ((namadmin.Text == "") && (passadmin.Text == ""))
            {
                MessageBox.Show("Login Sukses", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                menuadmin login = new menuadmin();
                login.Show();

            }
            else
            {
                MessageBox.Show("You have entered the wrong username and password! Try again");
                namadmin.Clear();
                passadmin.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }
    }
}
