namespace FarsiLibrary
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
            this.faDatePicker1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.faDayView1 = new FarsiLibrary.Win.Controls.FADayView();
            this.SuspendLayout();
            // 
            // faDatePicker1
            // 
            this.faDatePicker1.Location = new System.Drawing.Point(70, 28);
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.Size = new System.Drawing.Size(120, 20);
            this.faDatePicker1.TabIndex = 0;
            // 
            // faDayView1
            // 
            this.faDayView1.Location = new System.Drawing.Point(59, 83);
            this.faDayView1.Name = "faDayView1";
            this.faDayView1.SelectedDateTime = new System.DateTime(2018, 10, 7, 0, 0, 0, 0);
            this.faDayView1.TabIndex = 1;
            this.faDayView1.Text = "faDayView1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.faDayView1);
            this.Controls.Add(this.faDatePicker1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Win.Controls.FADatePicker faDatePicker1;
        private Win.Controls.FADayView faDayView1;
    }
}

