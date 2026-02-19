namespace RazboiCarti
{
    partial class Form1
    {
        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.butonRundaJ1 = new System.Windows.Forms.Button();
            this.butonRundaJ2 = new System.Windows.Forms.Button();
            this.butonRestart = new System.Windows.Forms.Button();
            this.imagineJucator1 = new System.Windows.Forms.PictureBox();
            this.imagineJucator2 = new System.Windows.Forms.PictureBox();
            this.textCastigator = new System.Windows.Forms.Label();
            this.cartiRamaseJ1 = new System.Windows.Forms.Label();
            this.cartiRamaseJ2 = new System.Windows.Forms.Label();
            this.rundeJ1 = new System.Windows.Forms.Label();
            this.rundeJ2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imagineJucator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagineJucator2)).BeginInit();
            this.SuspendLayout();
            // 
            // butonRundaJ1
            // 
            this.butonRundaJ1.Location = new System.Drawing.Point(80, 420);
            this.butonRundaJ1.Name = "butonRundaJ1";
            this.butonRundaJ1.Size = new System.Drawing.Size(220, 45);
            this.butonRundaJ1.TabIndex = 0;
            this.butonRundaJ1.Text = "Începe runda J1";
            this.butonRundaJ1.UseVisualStyleBackColor = true;
            this.butonRundaJ1.Click += new System.EventHandler(this.butonRundaJ1_Click);
            // 
            // butonRundaJ2
            // 
            this.butonRundaJ2.Location = new System.Drawing.Point(580, 420);
            this.butonRundaJ2.Name = "butonRundaJ2";
            this.butonRundaJ2.Size = new System.Drawing.Size(220, 45);
            this.butonRundaJ2.TabIndex = 1;
            this.butonRundaJ2.Text = "Începe runda J2";
            this.butonRundaJ2.UseVisualStyleBackColor = true;
            this.butonRundaJ2.Click += new System.EventHandler(this.butonRundaJ2_Click);
            // 
            // butonRestart
            // 
            this.butonRestart.Location = new System.Drawing.Point(365, 475);
            this.butonRestart.Name = "butonRestart";
            this.butonRestart.Size = new System.Drawing.Size(170, 45);
            this.butonRestart.TabIndex = 2;
            this.butonRestart.Text = "Restart joc";
            this.butonRestart.UseVisualStyleBackColor = true;
            this.butonRestart.Click += new System.EventHandler(this.butonRestart_Click);
            // 
            // imagineJucator1
            // 
            this.imagineJucator1.Location = new System.Drawing.Point(100, 60);
            this.imagineJucator1.Name = "imagineJucator1";
            this.imagineJucator1.Size = new System.Drawing.Size(200, 300);
            this.imagineJucator1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imagineJucator1.TabIndex = 3;
            this.imagineJucator1.TabStop = false;
            // 
            // imagineJucator2
            // 
            this.imagineJucator2.Location = new System.Drawing.Point(600, 60);
            this.imagineJucator2.Name = "imagineJucator2";
            this.imagineJucator2.Size = new System.Drawing.Size(200, 300);
            this.imagineJucator2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imagineJucator2.TabIndex = 4;
            this.imagineJucator2.TabStop = false;
            // 
            // textCastigator
            // 
            this.textCastigator.Font = new System.Drawing.Font("Arial", 16F);
            this.textCastigator.Location = new System.Drawing.Point(200, 0);
            this.textCastigator.Name = "textCastigator";
            this.textCastigator.Size = new System.Drawing.Size(500, 60);
            this.textCastigator.TabIndex = 5;
            this.textCastigator.Text = "Start joc!";
            this.textCastigator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cartiRamaseJ1
            // 
            this.cartiRamaseJ1.AutoSize = true;
            this.cartiRamaseJ1.Location = new System.Drawing.Point(100, 370);
            this.cartiRamaseJ1.Name = "cartiRamaseJ1";
            this.cartiRamaseJ1.Size = new System.Drawing.Size(124, 16);
            this.cartiRamaseJ1.TabIndex = 6;
            this.cartiRamaseJ1.Text = "Cărți Jucător 1: 26";
            // 
            // cartiRamaseJ2
            // 
            this.cartiRamaseJ2.AutoSize = true;
            this.cartiRamaseJ2.Location = new System.Drawing.Point(600, 370);
            this.cartiRamaseJ2.Name = "cartiRamaseJ2";
            this.cartiRamaseJ2.Size = new System.Drawing.Size(124, 16);
            this.cartiRamaseJ2.TabIndex = 7;
            this.cartiRamaseJ2.Text = "Cărți Jucător 2: 26";
            // 
            // rundeJ1
            // 
            this.rundeJ1.AutoSize = true;
            this.rundeJ1.Location = new System.Drawing.Point(100, 395);
            this.rundeJ1.Name = "rundeJ1";
            this.rundeJ1.Size = new System.Drawing.Size(77, 16);
            this.rundeJ1.TabIndex = 8;
            this.rundeJ1.Text = "Runde J1: 0";
            // 
            // rundeJ2
            // 
            this.rundeJ2.AutoSize = true;
            this.rundeJ2.Location = new System.Drawing.Point(600, 395);
            this.rundeJ2.Name = "rundeJ2";
            this.rundeJ2.Size = new System.Drawing.Size(77, 16);
            this.rundeJ2.TabIndex = 9;
            this.rundeJ2.Text = "Runde J2: 0";
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(900, 550);
            this.Controls.Add(this.rundeJ2);
            this.Controls.Add(this.rundeJ1);
            this.Controls.Add(this.cartiRamaseJ2);
            this.Controls.Add(this.cartiRamaseJ1);
            this.Controls.Add(this.textCastigator);
            this.Controls.Add(this.imagineJucator2);
            this.Controls.Add(this.imagineJucator1);
            this.Controls.Add(this.butonRestart);
            this.Controls.Add(this.butonRundaJ2);
            this.Controls.Add(this.butonRundaJ1);
            this.Name = "Form1";
            this.Text = "Joc de Război - LAN";
            ((System.ComponentModel.ISupportInitialize)(this.imagineJucator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagineJucator2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button butonRundaJ1;
        private System.Windows.Forms.Button butonRundaJ2;
        private System.Windows.Forms.Button butonRestart;
        private System.Windows.Forms.PictureBox imagineJucator1;
        private System.Windows.Forms.PictureBox imagineJucator2;
        private System.Windows.Forms.Label textCastigator;
        private System.Windows.Forms.Label cartiRamaseJ1;
        private System.Windows.Forms.Label cartiRamaseJ2;
        private System.Windows.Forms.Label rundeJ1;
        private System.Windows.Forms.Label rundeJ2;
    }
}
