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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonadmin_Click(object sender, EventArgs e)
        {
            this.Hide();
            login Form1 = new login();
            Form1.Show();
        }

        private void buttonexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonkmeans_Click(object sender, EventArgs e)
        {        
            this.Hide();
            dataakademik Form1 = new dataakademik(0);
            Form1.Show();            
        }

        private void buttonfcm_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bagus Kharismawan, NPM:140110090031", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
