using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
            UIStyle.Apply(this);
            UIStyle.AddRefreshButton(this, () => new Report());
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Format = DateTimePickerFormat.Short;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.AllowUserToAddRows = false;
            DateTime dateTime1 = dateTimePicker1.Value.Date;
            DateTime dateTime2 = dateTimePicker2.Value.Date;
            if (dateTime1 > dateTime2)
            {
                MessageBox.Show("Дата начала больше даты конца");
                return;
            }
            DB db = new DB();

            db.openConnection();

            SqlCommand command = new SqlCommand("SELECT CAST(Date_of_order as date) AS OrderDate, SUM(Summ) as Summa FROM Check_Form WHERE Date_of_order >= @dt1 AND Date_of_order < DATEADD(day,1,@dt2) GROUP BY CAST(Date_of_order as date) ORDER BY CAST(Date_of_order as date)", db.getConnection());

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

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Сначала сформируйте отчет");
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel CSV (*.csv)|*.csv";
            save.FileName = "orders_report_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            if (save.ShowDialog() != DialogResult.OK)
                return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Дата;Сумма");

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                string date = row.Cells[0].Value?.ToString() ?? string.Empty;
                string sum = row.Cells[1].Value?.ToString() ?? string.Empty;
                sb.AppendLine(date.Replace(";", ",") + ";" + sum.Replace(";", ","));
            }

            File.WriteAllText(save.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("Экспортировано в Excel-файл:\n" + save.FileName);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
