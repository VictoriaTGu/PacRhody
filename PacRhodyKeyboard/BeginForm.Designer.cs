namespace WindowsApplication22
{
    partial class BeginForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.begin_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.training_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(116, 62);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // begin_button
            // 
            this.begin_button.Location = new System.Drawing.Point(116, 117);
            this.begin_button.Name = "begin_button";
            this.begin_button.Size = new System.Drawing.Size(100, 23);
            this.begin_button.TabIndex = 1;
            this.begin_button.Text = "Begin";
            this.begin_button.UseVisualStyleBackColor = true;
            this.begin_button.Click += new System.EventHandler(this.begin_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SlateBlue;
            this.label1.Location = new System.Drawing.Point(113, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter Name";
            // 
            // training_button
            // 
            this.training_button.Location = new System.Drawing.Point(116, 88);
            this.training_button.Name = "training_button";
            this.training_button.Size = new System.Drawing.Size(100, 23);
            this.training_button.TabIndex = 3;
            this.training_button.Text = "Training";
            this.training_button.UseVisualStyleBackColor = true;
            this.training_button.Click += new System.EventHandler(this.training_button_Click);
            // 
            // BeginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 178);
            this.Controls.Add(this.training_button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.begin_button);
            this.Controls.Add(this.textBox1);
            this.Location = new System.Drawing.Point(150, 150);
            this.Name = "BeginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "BeginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button begin_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button training_button;
    }
}