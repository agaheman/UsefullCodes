﻿namespace CodeTest
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
            this.Btn_Tuple = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Btn_SqlClass = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Btn_Tuple
            // 
            this.Btn_Tuple.Location = new System.Drawing.Point(123, 116);
            this.Btn_Tuple.Name = "Btn_Tuple";
            this.Btn_Tuple.Size = new System.Drawing.Size(75, 23);
            this.Btn_Tuple.TabIndex = 0;
            this.Btn_Tuple.Text = "TupleTest";
            this.Btn_Tuple.UseVisualStyleBackColor = true;
            this.Btn_Tuple.Click += new System.EventHandler(this.Btn_Tuple_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(42, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(92, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Tyler Durden";
            // 
            // Btn_SqlClass
            // 
            this.Btn_SqlClass.Location = new System.Drawing.Point(42, 179);
            this.Btn_SqlClass.Name = "Btn_SqlClass";
            this.Btn_SqlClass.Size = new System.Drawing.Size(75, 23);
            this.Btn_SqlClass.TabIndex = 0;
            this.Btn_SqlClass.Text = "SqlClass";
            this.Btn_SqlClass.UseVisualStyleBackColor = true;
            this.Btn_SqlClass.Click += new System.EventHandler(this.Btn_SqlClass_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Btn_SqlClass);
            this.Controls.Add(this.Btn_Tuple);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_Tuple;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Btn_SqlClass;
    }
}

