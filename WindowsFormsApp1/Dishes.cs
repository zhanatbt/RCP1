using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Dishes : Form
    {
        int id_dish = 0;int id_type = 0;
        string data;
        public Dishes(string data)
        {
            InitializeComponent();
            UIStyle.Apply(this);
            this.data = data;
        }

        public Dishes()
        {
            InitializeComponent();
            UIStyle.Apply(this);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Dishes_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            String name_dish = this.data;
            DB db = new DB();

            LoadTypesCombo();

            try
            {
                db.openConnection();

                SqlCommand command = new SqlCommand("SELECT Name_of_dish,Cost,ISNULL(TypeOfDish,'') AS TypeOfDish FROM Dishes LEFT JOIN Type_of_Dishes on Dishes.Type_of_dish=Type_of_Dishes.Type_of_d ", db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                List<string[]> list = new List<string[]>();


                while (reader.Read())
                {
                    list.Add(new string[3]);

                    list[list.Count - 1][0] = reader[0].ToString();
                    list[list.Count - 1][1] = reader[1].ToString();
                    list[list.Count - 1][2] = reader[2].ToString();
                }

                reader.Close();

                foreach (string[] s in list)
                {
                    dataGridView1.Rows.Add(s);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.closeConnection();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int cost = 0;
            if (string.IsNullOrWhiteSpace(name.Text) || comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Заполните название и выберите тип");
            }
            else if (Convert.ToInt32(Cost.Text) < 0)
                MessageBox.Show("Введено отрицательное значение");
            else
            {
                cost = Convert.ToInt32(Cost.Text);
                String name_dish = name.Text;
                int id_type = Convert.ToInt32(comboBox1.SelectedValue);

                DB db = new DB();

                DataTable table = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand command = new SqlCommand("SELECT * FROM Dishes WHERE Name_of_dish=@uD", db.getConnection());
                command.Parameters.Add("@uD", SqlDbType.NVarChar).Value = name_dish;



                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                    MessageBox.Show("Такое блюдо уже есть");
                else
                {
                    command = new SqlCommand("INSERT INTO Dishes (Name_of_dish,Type_of_dish,Cost) VALUES (@name , @type , @cost)", db.getConnection());

                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name_dish;
                    command.Parameters.Add("@type", SqlDbType.NVarChar).Value = id_type;
                    command.Parameters.Add("@cost", SqlDbType.Int).Value = cost;

                    db.openConnection();

                    if (command.ExecuteNonQuery() == 1)
                        MessageBox.Show("Блюдо добавлено");
                    else
                        MessageBox.Show("Error...");



                    db.closeConnection();
                }
                db.openConnection();

                dataGridView1.Rows.Clear();

                command = new SqlCommand("SELECT Name_of_dish,Cost,ISNULL(TypeOfDish,'') AS TypeOfDish FROM Dishes LEFT JOIN Type_of_Dishes on Dishes.Type_of_dish=Type_of_Dishes.Type_of_d", db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                List<string[]> list = new List<string[]>();


                while (reader.Read())
                {
                    list.Add(new string[3]);

                    list[list.Count - 1][0] = reader[0].ToString();
                    list[list.Count - 1][1] = reader[1].ToString();
                    list[list.Count - 1][2] = reader[2].ToString();
                }

                reader.Close();

                db.closeConnection();
                foreach (string[] s in list)
                {
                    dataGridView1.Rows.Add(s);
                }
            }
        }

        private void comboBox1_MouseHover(object sender, EventArgs e)
        {
            LoadTypesCombo();
        }

        private void LoadTypesCombo()
        {
            DB db = new DB();
            DataTable table = new DataTable();
            try
            {
                db.openConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Type_of_Dishes", db.getConnection());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                comboBox1.DisplayMember = "TypeOfDish";
                comboBox1.ValueMember = "Type_of_d";
                comboBox1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.closeConnection();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String name_dish = name.Text;


            DB db = new DB();


            SqlCommand command = new SqlCommand("DELETE FROM Dishes WHERE Name_of_dish=@name", db.getConnection());
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name_dish;


            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
                MessageBox.Show("Блюдо удалено");
            else
                MessageBox.Show("Error...");


            dataGridView1.Rows.Clear();

            command = new SqlCommand("SELECT Name_of_dish,Cost,ISNULL(TypeOfDish,'') AS TypeOfDish FROM Dishes LEFT JOIN Type_of_Dishes on Dishes.Type_of_dish=Type_of_Dishes.Type_of_d ", db.getConnection());

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> list = new List<string[]>();


            while (reader.Read())
            {
                list.Add(new string[3]);

                list[list.Count - 1][0] = reader[0].ToString();
                list[list.Count - 1][1] = reader[1].ToString();
                list[list.Count - 1][2] = reader[2].ToString();
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

                name.Text = row.Cells[0].Value.ToString();

                comboBox1.Text = row.Cells[2].Value.ToString();

                Cost.Text = row.Cells[1].Value.ToString();

                String dish = name.Text;


                DB db = new DB();

                db.openConnection();

                SqlCommand command = new SqlCommand("SELECT ID_dish FROM Dishes WHERE Name_of_dish=@name", db.getConnection());
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name.Text;

                SqlDataReader reader2 = command.ExecuteReader();

                int id = 0;

                while (reader2.Read())
                {
                    id = Convert.ToInt32(reader2[0]);
                }

                reader2.Close();

                id_dish = id;

                 command = new SqlCommand("SELECT Type_of_dish FROM Dishes WHERE Name_of_dish=@name", db.getConnection());
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name.Text;

                 reader2 = command.ExecuteReader();

                int type_id = 0;

                while (reader2.Read())
                {
                    type_id = Convert.ToInt32(reader2[0]);
                }

                id_type= type_id;

                db.closeConnection();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int cost = 0;
            if (Convert.ToInt32(Cost.Text) < 0)
                MessageBox.Show("Введено отрицательное значение");
            else
            {
                cost = Convert.ToInt32(Cost.Text);
                String name_dish = name.Text;
                int id_type= 0;



                DB db = new DB();

                db.openConnection();

                SqlCommand command2 = new SqlCommand("SELECT Type_of_d FROM Type_Of_Dishes WHERE TypeOfDish=@name", db.getConnection());
                command2.Parameters.Add("@name", SqlDbType.NVarChar).Value = comboBox1.Text;

                SqlDataReader reader2 = command2.ExecuteReader();

                while (reader2.Read())
                {
                    id_type = Convert.ToInt32(reader2[0]);
                }

                reader2.Close();

                SqlCommand command = new SqlCommand("UPDATE Dishes SET Name_of_dish=@name,Type_of_dish=@type,Cost=@cost WHERE ID_dish=@id", db.getConnection());
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id_dish;
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name.Text;
                command.Parameters.Add("@type", SqlDbType.NVarChar).Value = id_type;
                command.Parameters.Add("@cost", SqlDbType.Int).Value =Convert.ToInt32(Cost.Text);

                if (command.ExecuteNonQuery() == 1)
                    MessageBox.Show("Блюдо changed");
                else
                    MessageBox.Show("Error...");

                dataGridView1.Rows.Clear();

                command = new SqlCommand("SELECT Name_of_dish,Cost,ISNULL(TypeOfDish,'') AS TypeOfDish FROM Dishes LEFT JOIN Type_of_Dishes on Dishes.Type_of_dish=Type_of_Dishes.Type_of_d ", db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                List<string[]> list = new List<string[]>();


                while (reader.Read())
                {
                    list.Add(new string[3]);

                    list[list.Count - 1][0] = reader[0].ToString();
                    list[list.Count - 1][1] = reader[1].ToString();
                    list[list.Count - 1][2] = reader[2].ToString();
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
}
