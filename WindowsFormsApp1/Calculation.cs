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
    public partial class Calculation : Form
    {
        public Calculation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Main main = new Main();
            main.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime dateTime1 = Convert.ToDateTime(dateTimePicker1.Text);
            DateTime dateTime2 = Convert.ToDateTime(dateTimePicker2.Text);
            dataGridView1.Rows.Clear();
            dataGridView1.AllowUserToAddRows = false;
            DB db = new DB();

            db.openConnection();

            SqlCommand command = new SqlCommand("SELECT Products.Name_of_prod,Sum(Calculation.Amount*Orders.Amount) AS Amount,Sum(CAST(Products.Cost AS decimal(18,2))*Calculation.Amount*Orders.Amount) AS 'Cost',Unit FROM Check_Form\r\nINNER JOIN Orders ON Check_Form.ID_order=Orders.ID_order\r\nINNER JOIN Dishes ON Orders.ID_dish=Dishes.ID_dish\r\nINNER JOIN Calculation ON Dishes.ID_dish=Calculation.ID_dish\r\nINNER JOIN Products ON Calculation.ID_product=Products.ID_product\r\nINNER JOIN Unit ON Products.ID_unit=Unit.ID_unit\r\nWHERE Check_Form.Date_of_order BETWEEN @dt1 AND @dt2+' 23:59:59' \r\nGroup by Products.Name_of_prod,Unit", db.getConnection());
            command.Parameters.Add("@dt1", SqlDbType.DateTime).Value = dateTime1;
            command.Parameters.Add("@dt2", SqlDbType.DateTime).Value = dateTime2;

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> list = new List<string[]>();


            while (reader.Read())
            {
                list.Add(new string[4]);

                list[list.Count - 1][0] = reader[0].ToString();
                list[list.Count - 1][1] = reader[1].ToString();
                list[list.Count - 1][2] = reader[2].ToString();
                list[list.Count - 1][3] = reader[3].ToString();
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
