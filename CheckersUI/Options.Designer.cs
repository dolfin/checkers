namespace CheckersUI
{
    partial class Options
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdExtreme = new System.Windows.Forms.RadioButton();
            this.rdHard = new System.Windows.Forms.RadioButton();
            this.rdMedium = new System.Windows.Forms.RadioButton();
            this.rdEasy = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdComputer = new System.Windows.Forms.RadioButton();
            this.rdHuman = new System.Windows.Forms.RadioButton();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdExtreme);
            this.groupBox1.Controls.Add(this.rdHard);
            this.groupBox1.Controls.Add(this.rdMedium);
            this.groupBox1.Controls.Add(this.rdEasy);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(111, 119);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Difficulty";
            // 
            // rdExtreme
            // 
            this.rdExtreme.AutoSize = true;
            this.rdExtreme.Location = new System.Drawing.Point(6, 88);
            this.rdExtreme.Name = "rdExtreme";
            this.rdExtreme.Size = new System.Drawing.Size(63, 17);
            this.rdExtreme.TabIndex = 3;
            this.rdExtreme.Text = "Extreme";
            this.rdExtreme.UseVisualStyleBackColor = true;
            // 
            // rdHard
            // 
            this.rdHard.AutoSize = true;
            this.rdHard.Location = new System.Drawing.Point(6, 65);
            this.rdHard.Name = "rdHard";
            this.rdHard.Size = new System.Drawing.Size(48, 17);
            this.rdHard.TabIndex = 2;
            this.rdHard.Text = "Hard";
            this.rdHard.UseVisualStyleBackColor = true;
            // 
            // rdMedium
            // 
            this.rdMedium.AutoSize = true;
            this.rdMedium.Location = new System.Drawing.Point(6, 42);
            this.rdMedium.Name = "rdMedium";
            this.rdMedium.Size = new System.Drawing.Size(62, 17);
            this.rdMedium.TabIndex = 1;
            this.rdMedium.Text = "Medium";
            this.rdMedium.UseVisualStyleBackColor = true;
            // 
            // rdEasy
            // 
            this.rdEasy.AutoSize = true;
            this.rdEasy.Checked = true;
            this.rdEasy.Location = new System.Drawing.Point(6, 19);
            this.rdEasy.Name = "rdEasy";
            this.rdEasy.Size = new System.Drawing.Size(48, 17);
            this.rdEasy.TabIndex = 0;
            this.rdEasy.TabStop = true;
            this.rdEasy.Text = "Easy";
            this.rdEasy.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdComputer);
            this.groupBox2.Controls.Add(this.rdHuman);
            this.groupBox2.Location = new System.Drawing.Point(131, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(112, 119);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "First Player";
            // 
            // rdComputer
            // 
            this.rdComputer.AutoSize = true;
            this.rdComputer.Location = new System.Drawing.Point(7, 44);
            this.rdComputer.Name = "rdComputer";
            this.rdComputer.Size = new System.Drawing.Size(70, 17);
            this.rdComputer.TabIndex = 1;
            this.rdComputer.Text = "Computer";
            this.rdComputer.UseVisualStyleBackColor = true;
            // 
            // rdHuman
            // 
            this.rdHuman.AutoSize = true;
            this.rdHuman.Checked = true;
            this.rdHuman.Location = new System.Drawing.Point(7, 20);
            this.rdHuman.Name = "rdHuman";
            this.rdHuman.Size = new System.Drawing.Size(59, 17);
            this.rdHuman.TabIndex = 0;
            this.rdHuman.TabStop = true;
            this.rdHuman.Text = "Human";
            this.rdHuman.UseVisualStyleBackColor = true;
            // 
            // btnOkay
            // 
            this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkay.Location = new System.Drawing.Point(251, 12);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 2;
            this.btnOkay.Text = "Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(250, 42);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 145);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.Text = "Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdExtreme;
        private System.Windows.Forms.RadioButton rdHard;
        private System.Windows.Forms.RadioButton rdMedium;
        private System.Windows.Forms.RadioButton rdEasy;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdComputer;
        private System.Windows.Forms.RadioButton rdHuman;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnCancel;
    }
}