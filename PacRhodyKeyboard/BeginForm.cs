using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication22
{
    public partial class BeginForm : Form
    {
        Form1 mainForm;


        public BeginForm(Form1 mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            mainForm.name = textBox1.Text;
        }

        private void begin_button_Click(object sender, EventArgs e)
        {
            this.Close();
            mainForm.ReadName();
            mainForm.timer1.Start();
        }

        private void training_button_Click(object sender, EventArgs e)
        {
            mainForm.TrainingForm();
        }


    }
}
