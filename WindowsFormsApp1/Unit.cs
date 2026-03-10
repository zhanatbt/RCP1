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
    public partial class Unit : Form
    {
        int id_unit = 0;
        public Unit()
        {
            InitializeComponent();
            UIStyle.Apply(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String name_unit = textBox1.Text;
           
            DB db = new DB();

            DataTable table = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter();

            SqlCommand command = new SqlCommand("SELECT * FROM Unit WHERE Unit=@un", db.getConnection());
            command.Parameters.Add("@un", SqlDbType.NVarChar).Value = name_unit;



            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
                MessageBox.Show("Такая запись уже есть");
            else
            {
                command = new SqlCommand("INSERT INTO Unit (Unit) VALUES (@name)", db.getConnection());

                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name_unit;


                db.openConnection();

                if (command.ExecuteNonQuery() == 1)
                    MessageBox.Show("Добавлено");
                else
                    MessageBox.Show("Error...");

                dataGridView1.Rows.Clear();

                dataGridView1.AllowUserToAddRows = false;
                String name_dish = textBox1.Text;
                

                 command = new SqlCommand("SELECT * FROM Unit", db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                List<string[]> list = new List<string[]>();


                while (reader.Read())
                {
                    list.Add(new string[2]);

                    list[list.Count - 1][0] = reader[1].ToString();
                    list[list.Count - 1][1] = reader[0].ToString();

                }

                reader.Close();

                db.closeConnection();
                foreach (string[] s in list)
                {
                    dataGridView1.Rows.Add(s);
                }

                db.closeConnection();
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

    

        private void button3_Click(object sender, EventArgs e)
        {
            String unit = textBox1.Text;


            DB db = new DB();


            SqlCommand command = new SqlCommand("DELETE FROM Unit WHERE Unit=@name", db.getConnection());
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = unit;


            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Запись удалена");
                dataGridView1.Refresh();
            }
            else
                MessageBox.Show("Error...");

            dataGridView1.Rows.Clear();

            dataGridView1.AllowUserToAddRows = false;
            String name_dish = textBox1.Text;


            command = new SqlCommand("SELECT * FROM Unit", db.getConnection());

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> list = new List<string[]>();


            while (reader.Read())
            {
                list.Add(new string[2]);

                list[list.Count - 1][0] = reader[1].ToString();
                list[list.Count - 1][1] = reader[0].ToString();

            }

            reader.Close();

            db.closeConnection();
            foreach (string[] s in list)
            {
                dataGridView1.Rows.Add(s);
            }

            db.closeConnection();
        }

        private void Unit_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            String name_dish = textBox1.Text;
            DB db = new DB();

            db.openConnection();

            SqlCommand command = new SqlCommand("SELECT * FROM Unit", db.getConnection());

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> list = new List<string[]>();


            while (reader.Read())
            {
                list.Add(new string[2]);

                list[list.Count - 1][0] = reader[1].ToString();
                list[list.Count - 1][1] = reader[0].ToString();

            }

            reader.Close();

            db.closeConnection();
            foreach (string[] s in list)
            {
                dataGridView1.Rows.Add(s);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id = id_unit;
            String unit = textBox1.Text;


            DB db = new DB();

            SqlCommand command = new SqlCommand("UPDATE Unit SET Unit=@name WHERE id_unit=@id", db.getConnection());
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = textBox1.Text;
            command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;


            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                dataGridView1.Refresh();
            }
            else
                MessageBox.Show("Error...");

            dataGridView1.Rows.Clear();

            dataGridView1.AllowUserToAddRows = false;
            String name_dish = textBox1.Text;


            command = new SqlCommand("SELECT * FROM Unit", db.getConnection());

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> list = new List<string[]>();


            while (reader.Read())
            {
                list.Add(new string[2]);

                list[list.Count - 1][0] = reader[1].ToString();
                list[list.Count - 1][1] = reader[0].ToString();

            }

            reader.Close();

            db.closeConnection();
            foreach (string[] s in list)
            {
                dataGridView1.Rows.Add(s);
            }

            db.closeConnection();
        }



        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox1.Text = row.Cells[0].Value.ToString();

                String unit = textBox1.Text;


                DB db = new DB();


                db.openConnection();


                SqlCommand command = new SqlCommand("SELECT ID_unit FROM Unit WHERE Unit=@name", db.getConnection());
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = unit;

                SqlDataReader reader2 = command.ExecuteReader();

                int id_un = 0;

                while (reader2.Read())
                {
                    id_un = Convert.ToInt32(reader2[0]);
                }

                id_unit = id_un;

                reader2.Close();

                db.closeConnection();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
