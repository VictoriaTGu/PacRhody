namespace WindowsApplication22
{
    partial class Scores
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.no = new System.Windows.Forms.Button();
            this.high_scores = new System.Windows.Forms.Button();
            this.all_scores = new System.Windows.Forms.Button();
            this.clear_scores = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "Scores"});
            this.listBox1.Location = new System.Drawing.Point(66, 79);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(419, 537);
            this.listBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(268, 644);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Yes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(118, 634);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 33);
            this.label2.TabIndex = 3;
            this.label2.Text = "Play Again?";
            // 
            // no
            // 
            this.no.Location = new System.Drawing.Point(349, 644);
            this.no.Name = "no";
            this.no.Size = new System.Drawing.Size(75, 23);
            this.no.TabIndex = 4;
            this.no.Text = "No";
            this.no.UseVisualStyleBackColor = true;
            this.no.Click += new System.EventHandler(this.button2_Click);
            // 
            // high_scores
            // 
            this.high_scores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.high_scores.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.high_scores.Location = new System.Drawing.Point(212, 27);
            this.high_scores.Name = "high_scores";
            this.high_scores.Size = new System.Drawing.Size(120, 38);
            this.high_scores.TabIndex = 5;
            this.high_scores.Text = "High Scores";
            this.high_scores.UseVisualStyleBackColor = false;
            this.high_scores.Click += new System.EventHandler(this.high_scores_Click);
            // 
            // all_scores
            // 
            this.all_scores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.all_scores.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.all_scores.Location = new System.Drawing.Point(66, 27);
            this.all_scores.Name = "all_scores";
            this.all_scores.Size = new System.Drawing.Size(119, 38);
            this.all_scores.TabIndex = 6;
            this.all_scores.Text = "All Scores";
            this.all_scores.UseVisualStyleBackColor = false;
            this.all_scores.Click += new System.EventHandler(this.all_scores_Click);
            // 
            // clear_scores
            // 
            this.clear_scores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.clear_scores.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clear_scores.Location = new System.Drawing.Point(360, 27);
            this.clear_scores.Name = "clear_scores";
            this.clear_scores.Size = new System.Drawing.Size(125, 38);
            this.clear_scores.TabIndex = 7;
            this.clear_scores.Text = "Clear Scores";
            this.clear_scores.UseVisualStyleBackColor = false;
            this.clear_scores.Click += new System.EventHandler(this.clear_scores_Click);
            // 
            // Scores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(554, 705);
            this.Controls.Add(this.clear_scores);
            this.Controls.Add(this.all_scores);
            this.Controls.Add(this.high_scores);
            this.Controls.Add(this.no);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Name = "Scores";
            this.Text = "Scores";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button no;
        private System.Windows.Forms.Button high_scores;
        private System.Windows.Forms.Button all_scores;
        private System.Windows.Forms.Button clear_scores;
    }
}