namespace PDF_Generator
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
            this.Btn_PDFGen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Btn_PDFGen
            // 
            this.Btn_PDFGen.Location = new System.Drawing.Point(104, 226);
            this.Btn_PDFGen.Name = "Btn_PDFGen";
            this.Btn_PDFGen.Size = new System.Drawing.Size(75, 23);
            this.Btn_PDFGen.TabIndex = 0;
            this.Btn_PDFGen.Text = "Make PDF";
            this.Btn_PDFGen.UseVisualStyleBackColor = true;
            this.Btn_PDFGen.Click += new System.EventHandler(this.Btn_PDFGen_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.Btn_PDFGen);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_PDFGen;
    }
}

