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
    public partial class Calculation : Form
    {
        public Calculation()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            UIStyle.AddRefreshButton(this, () => new Calculation());
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Format = DateTimePickerFormat.Short;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // In this form dateTimePicker2 is visually "С", dateTimePicker1 is "По".
            DateTime dateFrom = dateTimePicker2.Value.Date;
            DateTime dateTo = dateTimePicker1.Value.Date;

            if (dateFrom > dateTo)
            {
                MessageBox.Show("Дата начала больше даты конца");
                return;
            }

            dataGridView1.Rows.Clear();
            dataGridView1.AllowUserToAddRows = false;
            DB db = new DB();

            db.openConnection();

            SqlCommand command = new SqlCommand(
                "SELECT d.Name_of_dish, p.Name_of_prod, " +
                "SUM(CAST(c.Amount AS decimal(18,3)) * o.Amount) AS AmountSpent, " +
                "SUM(CAST(p.Cost AS decimal(18,2)) * CAST(c.Amount AS decimal(18,3)) * o.Amount) AS CostSpent, " +
                "u.Unit " +
                "FROM Check_Form cf " +
                "INNER JOIN Orders o ON cf.ID_order=o.ID_order AND CAST(cf.Date_of_order AS date)=CAST(o.Date_of_order AS date) " +
                "INNER JOIN Dishes d ON o.ID_dish=d.ID_dish " +
                "INNER JOIN Calculation c ON d.ID_dish=c.ID_dish " +
                "INNER JOIN Products p ON c.ID_product=p.ID_product " +
                "INNER JOIN Unit u ON p.ID_unit=u.ID_unit " +
                "WHERE cf.Date_of_order >= @dtFrom AND cf.Date_of_order < DATEADD(day,1,@dtTo) " +
                "AND o.Date_of_order >= @dtFrom AND o.Date_of_order < DATEADD(day,1,@dtTo) " +
                "GROUP BY d.Name_of_dish, p.Name_of_prod, u.Unit " +
                "ORDER BY d.Name_of_dish, p.Name_of_prod",
                db.getConnection());
            command.Parameters.Add("@dtFrom", SqlDbType.DateTime).Value = dateFrom;
            command.Parameters.Add("@dtTo", SqlDbType.DateTime).Value = dateTo;

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> list = new List<string[]>();


            while (reader.Read())
            {
                list.Add(new string[5]);

                list[list.Count - 1][0] = reader[0].ToString();
                list[list.Count - 1][1] = reader[1].ToString();
                list[list.Count - 1][2] = reader[2].ToString();
                list[list.Count - 1][3] = reader[3].ToString();
                list[list.Count - 1][4] = reader[4].ToString();
            }

            reader.Close();

            db.closeConnection();
            foreach (string[] s in list)
            {
                dataGridView1.Rows.Add(s);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Сначала сформируйте отчет");
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel CSV (*.csv)|*.csv";
            save.FileName = "product_report_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            if (save.ShowDialog() != DialogResult.OK)
                return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ОТЧЕТ ПО ПРОДУКТАМ");
            DateTime dateFrom = dateTimePicker2.Value.Date;
            DateTime dateTo = dateTimePicker1.Value.Date;
            sb.AppendLine("Период: " + dateFrom.ToString("dd.MM.yyyy") + " - " + dateTo.ToString("dd.MM.yyyy"));
            sb.AppendLine();

            for (int c = 0; c < dataGridView1.Columns.Count; c++)
            {
                if (c > 0) sb.Append(';');
                sb.Append(dataGridView1.Columns[c].HeaderText);
            }
            sb.AppendLine();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                for (int c = 0; c < dataGridView1.Columns.Count; c++)
                {
                    if (c > 0) sb.Append(';');
                    string value = row.Cells[c].Value?.ToString() ?? string.Empty;
                    sb.Append(value.Replace(";", ","));
                }
                sb.AppendLine();
            }

            File.WriteAllText(save.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("Экспортировано в Excel-файл:\n" + save.FileName);
        }
    }
}
