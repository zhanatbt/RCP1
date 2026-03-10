using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Main main = new Main();
            main.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.AllowUserToAddRows = false;
            DateTime dateTime1 = Convert.ToDateTime(dateTimePicker1.Text);
            DateTime dateTime2 = Convert.ToDateTime(dateTimePicker2.Text);
            DB db = new DB();

            db.openConnection();

            SqlCommand command = new SqlCommand("SELECT CAST(Date_of_order as date),Sum(Summ) as Summa FROM Check_Form WHERE Date_of_order BETWEEN @dt1 AND @dt2+' 23:59:59' GROUP BY CAST(Date_of_order as date) with rollup", db.getConnection());

            command.Parameters.Add("@dt1", SqlDbType.DateTime).Value = dateTime1;
            command.Parameters.Add("@dt2", SqlDbType.DateTime).Value = dateTime2;

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> list = new List<string[]>();


            while (reader.Read())
            {
                list.Add(new string[2]);

                list[list.Count - 1][0] = reader[0].ToString();
                list[list.Count - 1][1] = reader[1].ToString();
            }

            reader.Close();

            db.closeConnection();
            foreach (string[] s in list)
            {
                dataGridView1.Rows.Add(s);
            }
        }
    }
}
