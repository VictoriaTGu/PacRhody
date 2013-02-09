using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace WindowsApplication22
{
    public partial class Scores : Form
    {
        Form1 mainForm;

        public Scores(Form1 mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }

        public void Display()
        {
            listBox1.Items.Clear();
            string[] readText = File.ReadAllLines("log.txt");
            for (int i = 0; i < readText.Length; i++)
                listBox1.Items.Add(readText[i]);
            all_scores.BackColor = Color.AliceBlue;
            high_scores.BackColor = Color.LightSeaGreen;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainForm.clearwindow = true;
            Application.Restart();
        }

        private void high_scores_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string[] highScores = File.ReadAllLines("top_scores.txt");
            for (int i = 0; i < highScores.Length; i++)
                listBox1.Items.Add(highScores[i]);
            all_scores.BackColor = Color.MediumOrchid;
            high_scores.BackColor = Color.AliceBlue;
            
        }

        private void all_scores_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string[] highScores = File.ReadAllLines("log.txt");
            for (int i = 0; i < highScores.Length; i++)
                listBox1.Items.Add(highScores[i]);
            all_scores.BackColor = Color.AliceBlue;
            high_scores.BackColor = Color.LightSeaGreen;
            
        }

        private void clear_scores_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Are you sure you want to clear all scores?", "PacRhody", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
                return;
            if (result == DialogResult.Yes)
            {
                listBox1.Items.Clear();
                File.Delete("log.txt");
                File.Delete("top_scores.txt");
            }
        }
    }
}
