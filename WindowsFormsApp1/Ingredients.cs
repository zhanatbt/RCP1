using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Ingredients : Form
    {
        int selectedDishId = 0;
        int selectedProductId = 0;

        public Ingredients()
        {
            InitializeComponent();
            UIStyle.Apply(this);
        }

        private void Ingredients_Load(object sender, EventArgs e)
        {
            EnsureAmountColumnSupportsKg();
            LoadDishesCombo();
            LoadProductsCombo();
            labelDishCost.Text = "0.00";
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
                comboBox2.DisplayMember = "Name_of_dish";
                comboBox2.ValueMember = "ID_dish";
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

        private void LoadProductsCombo()
        {
            DB db = new DB();
            DataTable table = new DataTable();
            try
            {
                db.openConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Products", db.getConnection());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                comboBox3.DisplayMember = "Name_of_prod";
                comboBox3.ValueMember = "ID_product";
                comboBox3.DataSource = table;
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
        //кнопка выбрать

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            dataGridView1.Rows.Clear();
            string dish_name = comboBox2.Text;
            dataGridView1.AllowUserToAddRows = false;
            int id_dish = 0;
            DB db = new DB();

            if (string.IsNullOrWhiteSpace(dish_name))
            {
                MessageBox.Show("Выберите блюдо");
                return;
            }

            db.openConnection();


            SqlCommand cmd = new SqlCommand("select ID_dish from dishes where Name_of_dish=@name", db.getConnection());
            cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = dish_name;

            SqlDataReader reader2 = cmd.ExecuteReader();


            while (reader2.Read())
            {
                id_dish = Convert.ToInt32(reader2[0]);
            }

            reader2.Close();
            selectedDishId = id_dish;

            SqlCommand command = new SqlCommand("SELECT Products.Name_of_prod , Calculation.Amount ,Unit, Products.Cost FROM Calculation JOIN Dishes ON Calculation.ID_dish=Dishes.ID_dish JOIN Products ON Calculation.ID_product=Products.ID_product JOIN Unit ON Products.ID_unit=Unit.ID_unit WHERE Calculation.ID_dish=@id", db.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = id_dish;

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
            UpdateDishCostLabel(id_dish);
        }

        //кнопка добавить

        private void button2_Click(object sender, EventArgs e)
        {
            decimal amount = 0m;
            if (!TryParseAmount(textBox1.Text, out amount) || amount <= 0)
                MessageBox.Show("Введите корректное количество (можно в кг, например 0.35)");
            else
            {
                int id_dish = 0;
                int id_prod = 0;
                string dish_name = comboBox2.Text;
                string prod_name = comboBox3.Text;

                if (string.IsNullOrWhiteSpace(dish_name) || string.IsNullOrWhiteSpace(prod_name))
                {
                    MessageBox.Show("Выберите блюдо и продукт");
                    return;
                }

                DB db = new DB();

                db.openConnection();

                DataTable table = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand cmd = new SqlCommand("select ID_dish from dishes where Name_of_dish=@name", db.getConnection());
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = dish_name;

                SqlDataReader reader2 = cmd.ExecuteReader();

                while (reader2.Read())
                {
                    id_dish = Convert.ToInt32(reader2[0]);
                }

                reader2.Close();

                SqlCommand cmd2 = new SqlCommand("select ID_product from products where Name_of_prod=@name", db.getConnection());
                cmd2.Parameters.Add("@name", SqlDbType.NVarChar).Value = prod_name;

                SqlDataReader reader22 = cmd2.ExecuteReader();

                while (reader22.Read())
                {
                    id_prod = Convert.ToInt32(reader22[0]);
                }

                reader22.Close();

                SqlCommand command = new SqlCommand("SELECT Products.Name_of_prod , Calculation.Amount ,Unit, Products.Cost FROM Calculation JOIN Dishes ON Calculation.ID_dish=Dishes.ID_dish JOIN Products ON Calculation.ID_product=Products.ID_product JOIN Unit ON Products.ID_unit=Unit.ID_unit WHERE Calculation.ID_dish=@id AND Calculation.ID_product=@id_prod", db.getConnection());
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id_dish;
                command.Parameters.Add("@id_prod", SqlDbType.NVarChar).Value = id_prod;

                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                    MessageBox.Show("Такой ингридиент уже есть");
                else
                {
                    cmd = new SqlCommand("INSERT INTO Calculation (ID_dish,ID_product,Amount) VALUES (@id_dish , @id_product , @amount)", db.getConnection());
                    cmd.Parameters.Add("@id_dish", SqlDbType.Int).Value = id_dish;
                    cmd.Parameters.Add("@id_product", SqlDbType.Int).Value = id_prod;
                    SqlParameter amountParam = cmd.Parameters.Add("@amount", SqlDbType.Decimal);
                    amountParam.Precision = 18;
                    amountParam.Scale = 3;
                    amountParam.Value = amount;
                    if (cmd.ExecuteNonQuery() == 1)
                        MessageBox.Show("Добавлено");
                    else
                        MessageBox.Show("Ошибка! Ингридиент не добавлен");
                }

                dataGridView1.Rows.Clear();

                command = new SqlCommand("SELECT Products.Name_of_prod , Calculation.Amount ,Unit, Products.Cost FROM Calculation JOIN Dishes ON Calculation.ID_dish=Dishes.ID_dish JOIN Products ON Calculation.ID_product=Products.ID_product JOIN Unit ON Products.ID_unit=Unit.ID_unit WHERE Calculation.ID_dish=@id", db.getConnection());
                command.Parameters.Add("@id", SqlDbType.Int).Value = id_dish;

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
                UpdateDishCostLabel(id_dish);

                db.closeConnection();
            }
        }

        //кнопка возврата на гл. меню

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //кнопка удалить

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            String prod_name = comboBox3.Text;
            int id_dish = 0;
            int id_prod = 0;
            string dish_name = comboBox2.Text;

            if (string.IsNullOrWhiteSpace(dish_name) || string.IsNullOrWhiteSpace(prod_name))
            {
                MessageBox.Show("Выберите блюдо и продукт");
                return;
            }

            DB db = new DB();

            db.openConnection();

            SqlCommand cmd = new SqlCommand("select ID_dish from dishes where Name_of_dish=@name", db.getConnection());
            cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = dish_name;

            SqlDataReader reader2 = cmd.ExecuteReader();

            while (reader2.Read())
            {
                id_dish = Convert.ToInt32(reader2[0]);
            }

            reader2.Close();

            SqlCommand cmd2 = new SqlCommand("select ID_product from products where Name_of_prod=@name", db.getConnection());
            cmd2.Parameters.Add("@name", SqlDbType.NVarChar).Value = prod_name;

            SqlDataReader reader22 = cmd2.ExecuteReader();

            while (reader22.Read())
            {
                id_prod = Convert.ToInt32(reader22[0]);
            }

            reader22.Close();


            SqlCommand command = new SqlCommand("DELETE FROM Calculation WHERE id_product=@id_prod AND ID_dish=@id_dish", db.getConnection());
            command.Parameters.Add("@id_prod", SqlDbType.NVarChar).Value = id_prod;
            command.Parameters.Add("@id_dish", SqlDbType.NVarChar).Value = id_dish;


            if (command.ExecuteNonQuery() == 1)
                MessageBox.Show("Удалено");
            else
                MessageBox.Show("Error...");

            dataGridView1.Rows.Clear();

            command = new SqlCommand("SELECT Products.Name_of_prod , Calculation.Amount ,Unit, Products.Cost FROM Calculation JOIN Dishes ON Calculation.ID_dish=Dishes.ID_dish JOIN Products ON Calculation.ID_product=Products.ID_product JOIN Unit ON Products.ID_unit=Unit.ID_unit WHERE Calculation.ID_dish=@id", db.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = id_dish;

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
            UpdateDishCostLabel(id_dish);

            db.closeConnection();
        }

        //кнопка изменить

        private void button4_Click(object sender, EventArgs e)
        {
            decimal amount = 0m;
            if (!TryParseAmount(textBox1.Text, out amount) || amount <= 0)
                MessageBox.Show("Введите корректное количество (можно в кг, например 0.35)");
            else
            {
                String prod_name = comboBox3.Text;
                int id_ing = 0; int id_dish = 0;
                string dish_name = comboBox2.Text;

                if (string.IsNullOrWhiteSpace(dish_name) || string.IsNullOrWhiteSpace(prod_name))
                {
                    MessageBox.Show("Выберите блюдо и продукт");
                    return;
                }

                if (selectedProductId == 0)
                {
                    MessageBox.Show("Выберите строку в таблице для изменения");
                    return;
                }

                DB db = new DB();

                db.openConnection();



                SqlCommand cmd = new SqlCommand("select ID_dish from dishes where Name_of_dish=@name", db.getConnection());
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = dish_name;

                SqlDataReader reader2 = cmd.ExecuteReader();

                while (reader2.Read())
                {
                    id_dish = Convert.ToInt32(reader2[0]);
                }

                reader2.Close();



                SqlCommand comm = new SqlCommand("select ID_product from products where name_of_prod=@name", db.getConnection());
                comm.Parameters.Add("@name", SqlDbType.NVarChar).Value = comboBox3.Text;

                SqlDataReader read = comm.ExecuteReader();

                while (read.Read())
                {
                    id_ing = Convert.ToInt32(read[0]);
                }

                read.Close();


                SqlCommand command = new SqlCommand("UPDATE Calculation SET ID_product=@prod,Amount=@amount WHERE ID_Dish=@id_dish AND ID_product=@old_prod", db.getConnection());
                command.Parameters.Add("@prod", SqlDbType.Int).Value = id_ing;
                SqlParameter amountParam = command.Parameters.Add("@amount", SqlDbType.Decimal);
                amountParam.Precision = 18;
                amountParam.Scale = 3;
                amountParam.Value = amount;
                command.Parameters.Add("@id_dish", SqlDbType.Int).Value = id_dish;
                command.Parameters.Add("@old_prod", SqlDbType.Int).Value = selectedProductId;


                if (command.ExecuteNonQuery() == 1)
                    MessageBox.Show("Изменено");
                else
                    MessageBox.Show("Error...");

                dataGridView1.Rows.Clear();

                command = new SqlCommand("SELECT Products.Name_of_prod , Calculation.Amount ,Unit, Products.Cost FROM Calculation JOIN Dishes ON Calculation.ID_dish=Dishes.ID_dish JOIN Products ON Calculation.ID_product=Products.ID_product JOIN Unit ON Products.ID_unit=Unit.ID_unit WHERE Calculation.ID_dish=@id", db.getConnection());
                command.Parameters.Add("@id", SqlDbType.Int).Value = id_dish;

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
                UpdateDishCostLabel(id_dish);

                db.closeConnection();
            }
        }



        private void comboBox2_MouseHover(object sender, EventArgs e)
        {
            LoadDishesCombo();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
            string dish_name = comboBox2.Text;
            int id_dish = 0;
            int selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                comboBox3.Text = row.Cells[0].Value.ToString();
                textBox1.Text = row.Cells[1].Value.ToString();
                
                String prod_name = comboBox3.Text;


                DB db = new DB();

                db.openConnection();

                SqlCommand cmd = new SqlCommand("select ID_dish from dishes where Name_of_dish=@name", db.getConnection());
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = dish_name;

                SqlDataReader reader2 = cmd.ExecuteReader();


                while (reader2.Read())
                {
                    id_dish = Convert.ToInt32(reader2[0]);
                }

                reader2.Close();
                selectedDishId = id_dish;

                SqlCommand command = new SqlCommand("SELECT ID_product FROM Products WHERE Name_of_prod=@name", db.getConnection());
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = prod_name;

                 reader2 = command.ExecuteReader();

                int id_prod = 0;

                while (reader2.Read())
                {
                     id_prod = Convert.ToInt32(reader2[0]);
                }

                selectedProductId = id_prod;

                reader2.Close();


                dataGridView1.Rows.Clear();

                dataGridView1.AllowUserToAddRows = false;
                String name_dish = textBox1.Text;


                command = new SqlCommand("SELECT Products.Name_of_prod , Calculation.Amount ,Unit, Products.Cost FROM Calculation JOIN Dishes ON Calculation.ID_dish=Dishes.ID_dish JOIN Products ON Calculation.ID_product=Products.ID_product JOIN Unit ON Products.ID_unit=Unit.ID_unit WHERE Calculation.ID_dish=@id", db.getConnection());
                command.Parameters.Add("@id", SqlDbType.Int).Value = id_dish;


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
                UpdateDishCostLabel(id_dish);

                db.closeConnection();
            }
            }

        
       

        private void comboBox3_MouseHover(object sender, EventArgs e)
        {
            LoadProductsCombo();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Products products = new Products();
            this.Close();
            products.Show();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private bool TryParseAmount(string raw, out decimal amount)
        {
            amount = 0m;
            if (string.IsNullOrWhiteSpace(raw))
                return false;

            string normalized = raw.Trim().Replace(',', '.');
            return decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out amount);
        }

        private void EnsureAmountColumnSupportsKg()
        {
            DB db = new DB();
            try
            {
                db.openConnection();
                SqlCommand checkCmd = new SqlCommand(
                    "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Calculation' AND COLUMN_NAME='Amount'",
                    db.getConnection());
                object typeObj = checkCmd.ExecuteScalar();
                string dataType = (typeObj == null || typeObj == DBNull.Value) ? string.Empty : typeObj.ToString();

                if (string.Equals(dataType, "int", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(dataType, "smallint", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(dataType, "tinyint", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(dataType, "bigint", StringComparison.OrdinalIgnoreCase))
                {
                    SqlCommand alterCmd = new SqlCommand(
                        "ALTER TABLE Calculation ALTER COLUMN Amount decimal(18,3) NOT NULL",
                        db.getConnection());
                    alterCmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // If schema migration is not allowed in this environment, app keeps existing behavior.
            }
            finally
            {
                db.closeConnection();
            }
        }

        private void UpdateDishCostLabel(int dishId)
        {
            DB db = new DB();
            try
            {
                db.openConnection();
                SqlCommand command = new SqlCommand(
                    "SELECT ISNULL(SUM(CAST(Calculation.Amount AS decimal(18,3)) * CAST(Products.Cost AS decimal(18,2))), 0) " +
                    "FROM Calculation INNER JOIN Products ON Calculation.ID_product = Products.ID_product " +
                    "WHERE Calculation.ID_dish=@id",
                    db.getConnection());
                command.Parameters.Add("@id", SqlDbType.Int).Value = dishId;

                object value = command.ExecuteScalar();
                decimal cost = (value == null || value == DBNull.Value) ? 0m : Convert.ToDecimal(value);
                labelDishCost.Text = cost.ToString("0.00", CultureInfo.InvariantCulture);
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
    }
    }
    


