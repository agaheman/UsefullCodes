namespace ServiceStateObserver
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
            this.txt_ServiceName = new System.Windows.Forms.TextBox();
            this.btn_StartObserver = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_ServiceName
            // 
            this.txt_ServiceName.Location = new System.Drawing.Point(66, 12);
            this.txt_ServiceName.Name = "txt_ServiceName";
            this.txt_ServiceName.Size = new System.Drawing.Size(150, 20);
            this.txt_ServiceName.TabIndex = 0;
            this.txt_ServiceName.Text = "SQLSERVERAGENT";
            this.txt_ServiceName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn_StartObserver
            // 
            this.btn_StartObserver.Location = new System.Drawing.Point(95, 38);
            this.btn_StartObserver.Name = "btn_StartObserver";
            this.btn_StartObserver.Size = new System.Drawing.Size(86, 23);
            this.btn_StartObserver.TabIndex = 1;
            this.btn_StartObserver.Text = "Start Observer";
            this.btn_StartObserver.UseVisualStyleBackColor = true;
            this.btn_StartObserver.Click += new System.EventHandler(this.Btn_StartObserver_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 71);
            this.Controls.Add(this.btn_StartObserver);
            this.Controls.Add(this.txt_ServiceName);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_ServiceName;
        private System.Windows.Forms.Button btn_StartObserver;
    }
}

