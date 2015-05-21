using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsGraphica
{
    public partial class Form2 : Form
    {
        public int Nomer
        {
            get { return (int)comboBox1.SelectedItem; }
            //set { numericUpDown1.Value = value; }
        }
        public List<int> SpisokNedovalennihUzlov;

        public Form2()
        {
            InitializeComponent();
            
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            foreach (int i in SpisokNedovalennihUzlov)
            {
                comboBox1.Items.Add(i);
            }
            comboBox1.SelectedIndex = 0;
        }
    }
}
