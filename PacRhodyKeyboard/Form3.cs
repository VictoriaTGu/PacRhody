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
    public partial class Form3 : Form
    {
        Form1 mainForm;

        public Form3(Form1 mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void send_button_Click(object sender, EventArgs e)
        {
            if (this.richTextBox1.Text != string.Empty)
            {
                mainForm.listBox1.Items.Clear();

                string[] stringsEntered = richTextBox1.Lines;

                for (int i = 0; i < stringsEntered.Length; i++)
                    mainForm.listBox1.Items.Add(stringsEntered[i]);
            }

            this.Close();
        }
    }
}
