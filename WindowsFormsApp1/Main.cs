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
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dishes ing = new Dishes();
            ing.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Ingredients ing = new Ingredients();
            ing.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Unit u = new Unit();
            u.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Orders ch = new Orders();
            ch.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Report report = new Report();
            report.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Calculation calc = new Calculation();
            calc.Show();
        }
    }
}
