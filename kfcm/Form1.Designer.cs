namespace kfcm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.buttonkmeans = new System.Windows.Forms.Button();
            this.buttonfcm = new System.Windows.Forms.Button();
            this.buttonexit = new System.Windows.Forms.Button();
            this.buttonadmin = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PROGRAM CLUSTERING K-MEANS DAN FUZZY C-MEANS";
            // 
            // buttonkmeans
            // 
            this.buttonkmeans.Location = new System.Drawing.Point(163, 40);
            this.buttonkmeans.Name = "buttonkmeans";
            this.buttonkmeans.Size = new System.Drawing.Size(128, 45);
            this.buttonkmeans.TabIndex = 1;
            this.buttonkmeans.Text = "USER";
            this.buttonkmeans.UseVisualStyleBackColor = true;
            this.buttonkmeans.Click += new System.EventHandler(this.buttonkmeans_Click);
            // 
            // buttonfcm
            // 
            this.buttonfcm.Location = new System.Drawing.Point(45, 176);
            this.buttonfcm.Name = "buttonfcm";
            this.buttonfcm.Size = new System.Drawing.Size(126, 28);
            this.buttonfcm.TabIndex = 2;
            this.buttonfcm.Text = "About";
            this.buttonfcm.UseVisualStyleBackColor = true;
            this.buttonfcm.Click += new System.EventHandler(this.buttonfcm_Click);
            // 
            // buttonexit
            // 
            this.buttonexit.Location = new System.Drawing.Point(202, 176);
            this.buttonexit.Name = "buttonexit";
            this.buttonexit.Size = new System.Drawing.Size(128, 28);
            this.buttonexit.TabIndex = 3;
            this.buttonexit.Text = "EXIT";
            this.buttonexit.UseVisualStyleBackColor = true;
            this.buttonexit.Click += new System.EventHandler(this.buttonexit_Click);
            // 
            // buttonadmin
            // 
            this.buttonadmin.Location = new System.Drawing.Point(6, 40);
            this.buttonadmin.Name = "buttonadmin";
            this.buttonadmin.Size = new System.Drawing.Size(126, 45);
            this.buttonadmin.TabIndex = 4;
            this.buttonadmin.Text = "ADMIN";
            this.buttonadmin.UseVisualStyleBackColor = true;
            this.buttonadmin.Click += new System.EventHandler(this.buttonadmin_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonkmeans);
            this.groupBox1.Controls.Add(this.buttonadmin);
            this.groupBox1.Location = new System.Drawing.Point(39, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 111);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Masuk Sebagai";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 235);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonexit);
            this.Controls.Add(this.buttonfcm);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Menu Awal";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonkmeans;
        private System.Windows.Forms.Button buttonfcm;
        private System.Windows.Forms.Button buttonexit;
        private System.Windows.Forms.Button buttonadmin;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

