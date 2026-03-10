using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Orders : Form
    {
        public Orders()
        {
            InitializeComponent();
            UIStyle.Apply(this);
        }

        private void Orders_Load(object sender, EventArgs e)
        {
            LoadDishesCombo();
            LoadOpenOrdersCombo();
        }

        private void LoadDishesCombo()
        {
            DB db = new DB();
            DataTable table = new DataTable();
            try
            {
                db.openConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Dishes", db.getConnection());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                comboBox1.DisplayMember = "Name_of_dish";
                comboBox1.ValueMember = "ID_dish";
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

        private void LoadOpenOrdersCombo()
        {
            DB db = new DB();
            DataTable table = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand("SELECT Orders.ID_order FROM Orders LEFT JOIN Check_Form ON Orders.ID_order = Check_Form.ID_order WHERE Check_Form.ID_order IS NULL GROUP BY Orders.ID_order", db.getConnection());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                comboBox2.DisplayMember = "ID_order";
                comboBox2.ValueMember = "ID_order";
                comboBox2.DataSource = table;
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

        //Создать

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count <= 1)
            {
                MessageBox.Show("Добавьте блюда в заказ");
                return;
            }

            int id_dish = 0; int cost = 0; int id_order = 1;
            int k = 0;

            DB db = new DB();

            db.openConnection();

            SqlCommand cmd = new SqlCommand("SELECT TOP(1) ID_order FROM ORDERS ORDER BY ID DESC", db.getConnection());

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                id_order = (int)reader[0] + 1;
            }

            reader.Close();

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {

                cmd = new SqlCommand("SELECT Id_dish,Cost FROM Dishes where Name_of_dish=@name", db.getConnection());
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = dataGridView1[0, i].Value;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    id_dish = Convert.ToInt32(reader[0].ToString());
                    cost = Convert.ToInt32(reader[1].ToString());
                }

                reader.Close();



                SqlCommand command = new SqlCommand("INSERT INTO Orders (Date_of_order,ID_dish,Amount,Cost,ID_order) VALUES (GETDATE() , @id_dish , @amount ,@cost,@id_order)", db.getConnection());
                command.Parameters.Add("@id_dish", SqlDbType.NVarChar).Value = id_dish;
                command.Parameters.Add("@cost", SqlDbType.Int).Value = cost;
                command.Parameters.Add("@id_order", SqlDbType.Int).Value = id_order;
                command.Parameters.Add("@amount", SqlDbType.Int).Value = dataGridView1[2, i].Value;



                if (command.ExecuteNonQuery() == 1)
                    k++;
                else
                    MessageBox.Show("Error...");
            }
            if (k > 0)
                MessageBox.Show(id_order.ToString() + " Заказ создан");

        }

        private void comboBox2_MouseHover(object sender, EventArgs e)
        {
            LoadOpenOrdersCombo();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DB db = new DB();

            db.openConnection();



            SqlCommand command = new SqlCommand("SELECT Name_of_dish,TypeOfDish,ORDERS.Amount,Dishes.Cost FROM ORDERS JOIN Dishes ON ORDERS.ID_dish=Dishes.ID_dish JOIN Type_of_Dishes ON Dishes.Type_of_dish=Type_of_Dishes.Type_of_d WHERE ID_order=@id_ord", db.getConnection());
            command.Parameters.Add("@id_ord", SqlDbType.Int).Value = Convert.ToInt32(comboBox2.Text);


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

        private void comboBox1_MouseHover(object sender, EventArgs e)
        {
            LoadDishesCombo();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                MessageBox.Show("Выберите заказ");
                return;
            }
            int ord_id = Convert.ToInt32(comboBox2.Text);

            DB db = new DB();

            db.openConnection();

            SqlCommand cmd = new SqlCommand("DELETE FROM ORDERS WHERE ID_order=@id", db.getConnection());
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = ord_id;

            if (cmd.ExecuteNonQuery() >= 1)
                MessageBox.Show("Заказ удален");
            else
                MessageBox.Show("Error...");


            dataGridView1.Rows.Clear();


            cmd = new SqlCommand("SELECT Name_of_dish,TypeOfDish,ORDERS.Amount,Dishes.Cost FROM ORDERS JOIN Dishes ON ORDERS.ID_dish=Dishes.ID_dish JOIN Type_of_Dishes ON Dishes.Type_of_dish=Type_of_Dishes.Type_of_d WHERE ID_order=@id_ord", db.getConnection());
            cmd.Parameters.Add("@id_ord", SqlDbType.Int).Value = ord_id;


            SqlDataReader reader = cmd.ExecuteReader();

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

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                MessageBox.Show("Выберите заказ");
                return;
            }
            int ord_id = Convert.ToInt32(comboBox2.Text);
            int id_dish = 0;
            int dish_cost = 0;
            string name_dish = comboBox1.Text;
            int check_id = 0;
            if (string.IsNullOrWhiteSpace(name_dish))
            {
                MessageBox.Show("Выберите блюдо");
                return;
            }
            if ((textBox1.Text == "") || (Convert.ToInt32(textBox1.Text) < 0))
            {
                MessageBox.Show("Введено неверное значение");
                return;
            }
            int amount = Convert.ToInt32(textBox1.Text);

            DB db = new DB();

            db.openConnection();


            SqlCommand cmd = new SqlCommand("SELECT ID_dish,Cost FROM Dishes WHERE Name_of_dish=@name", db.getConnection());
            cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name_dish;

            SqlDataReader reader2 = cmd.ExecuteReader();

            while (reader2.Read())
            {
                id_dish = Convert.ToInt32(reader2[0]);
                dish_cost = Convert.ToInt32(reader2[1]);
            }

            reader2.Close();

            cmd = new SqlCommand("SELECT ID_check FROM Check_Form where ID_order=@id_ch", db.getConnection());
            cmd.Parameters.Add("@id_ch", SqlDbType.NVarChar).Value = ord_id;

            SqlDataReader reader3 = cmd.ExecuteReader();


            if (reader3.HasRows)
            {
                reader3.Close();
                MessageBox.Show("Чек закрыт");
            }
            else
            {
                reader3.Close();
                SqlCommand command = new SqlCommand("INSERT INTO ORDERS (Date_of_order,ID_dish,Amount,Cost,ID_Order) VALUES ( GETDATE() , @id , @amount , @cost, @id_ch)", db.getConnection());

                command.Parameters.Add("@id_ch", SqlDbType.NVarChar).Value = ord_id;
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id_dish;
                command.Parameters.Add("@cost", SqlDbType.Int).Value = dish_cost;
                command.Parameters.Add("@amount", SqlDbType.Int).Value = amount;



                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Блюдо добавлено");
                    dataGridView1.Rows.Clear();


                    command = new SqlCommand("SELECT Name_of_dish,TypeOfDish,ORDERS.Amount,Dishes.Cost FROM ORDERS JOIN Dishes ON ORDERS.ID_dish=Dishes.ID_dish JOIN Type_of_Dishes ON Dishes.Type_of_dish=Type_of_Dishes.Type_of_d WHERE ID_order=@id_ord", db.getConnection());
                    command.Parameters.Add("@id_ord", SqlDbType.Int).Value = ord_id;


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
                else
                    MessageBox.Show("Error...");
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id_check = 1; int id_dish = 0; int cost = 0; int k = 0;
            int Sum = 0;
            string s = "";
            int ord_id = 0;

            DB db = new DB();

            db.openConnection();

            SqlCommand cmd3 = new SqlCommand("SELECT TOP(1) ID from Orders ORDER BY ID DESC", db.getConnection());

            SqlDataReader reader3 = cmd3.ExecuteReader();

            if (comboBox2.Text == "")
            {


                while (reader3.Read())
                {
                    ord_id = Convert.ToInt32(reader3[0]) + 1;
                }

                reader3.Close();


                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {

                    SqlCommand cmd5 = new SqlCommand("SELECT Id_dish,Cost FROM Dishes where Name_of_dish=@name", db.getConnection());
                    cmd5.Parameters.Add("@name", SqlDbType.NVarChar).Value = dataGridView1[0, i].Value;

                    SqlDataReader reader5 = cmd5.ExecuteReader();

                    while (reader5.Read())
                    {
                        id_dish = Convert.ToInt32(reader5[0].ToString());
                        cost = Convert.ToInt32(reader5[1].ToString());
                    }

                    reader5.Close();




                    SqlCommand command = new SqlCommand("INSERT INTO Orders (Date_of_order,ID_dish,Amount,Cost,ID_order) VALUES (GETDATE() , @id_dish , @amount ,@cost,@id_order)", db.getConnection());
                    command.Parameters.Add("@id_dish", SqlDbType.NVarChar).Value = id_dish;
                    command.Parameters.Add("@cost", SqlDbType.Int).Value = cost;
                    command.Parameters.Add("@id_order", SqlDbType.Int).Value = ord_id;
                    command.Parameters.Add("@amount", SqlDbType.Int).Value = dataGridView1[2, i].Value;



                    if (command.ExecuteNonQuery() == 1)
                        k++;
                    else
                        MessageBox.Show("Error...");
                }
                reader3.Close();
                if (k > 0)
                    MessageBox.Show("Заказ создан");
            }
            else
                ord_id = Convert.ToInt32(comboBox2.Text);

            reader3.Close();


            SqlCommand cmd = new SqlCommand("SELECT TOP(1) ID_check from Check_Form ORDER BY ID_check DESC", db.getConnection());

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                id_check = Convert.ToInt32(reader[0]);
            }

            reader.Close();


            SqlCommand command2 = new SqlCommand("SELECT ID_order, SUM(Cost*Amount) FROM ORDERS WHERE ID_Order = @id GROUP BY ID_Order", db.getConnection());
            command2.Parameters.Add("@id", SqlDbType.NVarChar).Value = ord_id;

            SqlCommand cmd2 = new SqlCommand("DELETE FROM ORDERS WHERE ID_order=@id", db.getConnection());
            cmd2.Parameters.Add("@id", SqlDbType.Int).Value = ord_id;


            SqlDataReader reader2 = command2.ExecuteReader();

            while (reader2.Read())
            {
                Sum = Convert.ToInt32(reader2[1]);
                s = "Чек №" + ord_id.ToString() + " К оплате-" + reader2[1].ToString();
            }
            reader2.Close();



            cmd = new SqlCommand("INSERT INTO Check_Form(Date_of_order,ID_order,Summ) VALUES (GETDATE(),@id_o,@sum)", db.getConnection());
            cmd.Parameters.Add("@id_o", SqlDbType.Int).Value = ord_id;
            cmd.Parameters.Add("@sum", SqlDbType.Int).Value = Sum;


            if (cmd.ExecuteNonQuery() >= 1)
            {
                MessageBox.Show(s);
            }
            else
                MessageBox.Show("Error...");

            dataGridView1.Rows.Clear();


            db.closeConnection();
        }

        //Выбрать

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Выберите блюдо");
            }
            else if (string.IsNullOrWhiteSpace(textBox1.Text) || (Convert.ToInt32(textBox1.Text) < 0))
                MessageBox.Show("Введено неверное значение");
            else
            {
                string dish_name = comboBox1.Text;
                int amount = Convert.ToInt32(textBox1.Text);

                DB db = new DB();

                db.openConnection();

                SqlCommand cmd = new SqlCommand("SELECT Name_of_dish,TypeOfDish,Cost FROM Dishes JOIN Type_of_Dishes on Dishes.Type_of_dish=Type_of_Dishes.Type_of_d where Name_of_dish=@name", db.getConnection());
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = dish_name;

                SqlDataReader reader = cmd.ExecuteReader();

                List<string[]> list = new List<string[]>();


                while (reader.Read())
                {
                    list.Add(new string[4]);

                    list[list.Count - 1][0] = reader[0].ToString();
                    list[list.Count - 1][1] = reader[1].ToString();
                    list[list.Count - 1][2] = amount.ToString();
                    list[list.Count - 1][3] = reader[2].ToString();

                }


                reader.Close();

                db.closeConnection();
                foreach (string[] s in list)
                {
                    bool merged = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].IsNewRow)
                            continue;
                        var existingName = dataGridView1.Rows[i].Cells[0].Value?.ToString();
                        if (string.Equals(existingName, s[0], StringComparison.OrdinalIgnoreCase))
                        {
                            int existingAmount = 0;
                            int.TryParse(dataGridView1.Rows[i].Cells[2].Value?.ToString(), out existingAmount);
                            int addAmount = 0;
                            int.TryParse(s[2], out addAmount);
                            dataGridView1.Rows[i].Cells[2].Value = (existingAmount + addAmount).ToString();
                            merged = true;
                            break;
                        }
                    }
                    if (!merged)
                        dataGridView1.Rows.Add(s);
                }

                db.closeConnection();
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
