using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Main : Form
    {
        private Form activeForm;

        public Main()
        {
            InitializeComponent();
            OpenChildForm(new Dashboard());
        }

        public void OpenChildForm(Form child)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = child;
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;
            panelMain.Controls.Clear();
            panelMain.Controls.Add(child);
            child.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Dishes());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Ingredients());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Unit());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Orders());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Report());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Calculation());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Products());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Dashboard());
        }
    }
}
