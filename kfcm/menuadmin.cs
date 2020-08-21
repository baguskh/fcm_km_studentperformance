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
    public partial class menuadmin : Form
    {
        public menuadmin()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            input_fcm menuadmin = new input_fcm();
            menuadmin.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            input_km menuadmin = new input_km();
            menuadmin.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            dataakademik menuadmin = new dataakademik(1);
            menuadmin.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 menuadmin = new Form1();
            menuadmin.Show();
        }
    }
}
