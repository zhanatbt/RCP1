using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
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
        private bool markupColumnReady = false;
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
            markupColumnReady = EnsureDishMarkupColumnSupportsAutoPricing();
            DB db = new DB();
            MarkupPercent.Text = "30";

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
            if (!TryGetMarkupPercent(out decimal markupPercent))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(name.Text) || comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Заполните название и выберите тип");
            }
            else
            {
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
                    int cost = 0; // no ingredients yet
                    if (markupColumnReady)
                        command = new SqlCommand("INSERT INTO Dishes (Name_of_dish,Type_of_dish,Cost,MarkupPercent) VALUES (@name , @type , @cost, @markup)", db.getConnection());
                    else
                        command = new SqlCommand("INSERT INTO Dishes (Name_of_dish,Type_of_dish,Cost) VALUES (@name , @type , @cost)", db.getConnection());

                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name_dish;
                    command.Parameters.Add("@type", SqlDbType.NVarChar).Value = id_type;
                    command.Parameters.Add("@cost", SqlDbType.Int).Value = cost;
                    if (markupColumnReady)
                    {
                        var markup = command.Parameters.Add("@markup", SqlDbType.Decimal);
                        markup.Precision = 5;
                        markup.Scale = 2;
                        markup.Value = markupPercent;
                    }

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
            DB db = new DB();
            try
            {
                if (!TryResolveSelectedDishId(db, out int dishId))
                {
                    MessageBox.Show("Сначала выберите блюдо в таблице");
                    return;
                }

                db.openConnection();

                if (IsDishInActiveOrders(db, dishId))
                {
                    MessageBox.Show("Нельзя удалить: блюдо есть в активных заказах");
                    return;
                }

                SqlCommand delCalc = new SqlCommand("DELETE FROM Calculation WHERE ID_dish=@id", db.getConnection());
                delCalc.Parameters.Add("@id", SqlDbType.Int).Value = dishId;
                delCalc.ExecuteNonQuery();

                SqlCommand command = new SqlCommand("DELETE FROM Dishes WHERE ID_dish=@id", db.getConnection());
                command.Parameters.Add("@id", SqlDbType.Int).Value = dishId;

                if (command.ExecuteNonQuery() == 1)
                    MessageBox.Show("Блюдо удалено");
                else
                    MessageBox.Show("Блюдо не найдено");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Нельзя удалить: есть связанные записи.\n" + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.closeConnection();
            }

            RefreshDishesGrid();
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
                LoadCostPrice(id_dish);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!TryGetMarkupPercent(out decimal markupPercent))
            {
                return;
            }

            if (id_dish == 0)
            {
                MessageBox.Show("Сначала выберите блюдо в таблице");
                return;
            }
            else
            {
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

                decimal costPrice = GetDishCostPrice(id_dish, db);
                int cost = Convert.ToInt32(Math.Round(costPrice + (costPrice * markupPercent / 100m), 0, MidpointRounding.AwayFromZero));

                SqlCommand command = new SqlCommand("UPDATE Dishes SET Name_of_dish=@name,Type_of_dish=@type,Cost=@cost WHERE ID_dish=@id", db.getConnection());
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id_dish;
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name.Text;
                command.Parameters.Add("@type", SqlDbType.NVarChar).Value = id_type;
                command.Parameters.Add("@cost", SqlDbType.Int).Value = cost;
                if (markupColumnReady)
                {
                    var markup = command.Parameters.Add("@markup", SqlDbType.Decimal);
                    markup.Precision = 5;
                    markup.Scale = 2;
                    markup.Value = markupPercent;
                    command.CommandText = "UPDATE Dishes SET Name_of_dish=@name,Type_of_dish=@type,Cost=@cost,MarkupPercent=@markup WHERE ID_dish=@id";
                }

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

        private void MarkupPercent_TextChanged(object sender, EventArgs e)
        {
            RecalculatePriceFromCostPrice();
        }

        private void CostPrice_TextChanged(object sender, EventArgs e)
        {
            RecalculatePriceFromCostPrice();
        }

        private bool TryParseDecimal(string text, out decimal value)
        {
            value = 0m;
            if (string.IsNullOrWhiteSpace(text))
                return false;

            string normalized = text.Trim().Replace(',', '.');
            return decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }

        private void RecalculatePriceFromCostPrice()
        {
            if (!TryParseDecimal(CostPrice.Text, out decimal costPrice))
            {
                Cost.Text = string.Empty;
                return;
            }

            if (!TryParseDecimal(MarkupPercent.Text, out decimal markupPercent) || markupPercent < 0)
            {
                Cost.Text = string.Empty;
                return;
            }

            decimal finalPrice = costPrice + (costPrice * markupPercent / 100m);
            Cost.Text = Math.Round(finalPrice, 0, MidpointRounding.AwayFromZero).ToString();
        }

        private bool TryGetDishPrice(out int dishPrice)
        {
            dishPrice = 0;
            if (string.IsNullOrWhiteSpace(Cost.Text) || !int.TryParse(Cost.Text, out dishPrice) || dishPrice < 0)
            {
                MessageBox.Show("Проверьте себестоимость и процент наценки");
                return false;
            }

            return true;
        }

        private bool TryGetMarkupPercent(out decimal markupPercent)
        {
            markupPercent = 0m;
            if (!TryParseDecimal(MarkupPercent.Text, out markupPercent) || markupPercent < 0)
            {
                MessageBox.Show("Введите корректный процент наценки");
                return false;
            }

            return true;
        }

        private void LoadCostPrice(int dishId)
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

                object result = command.ExecuteScalar();
                decimal costPrice = (result == null || result == DBNull.Value) ? 0m : Convert.ToDecimal(result);
                CostPrice.Text = costPrice.ToString("0.###", CultureInfo.InvariantCulture);

                if (markupColumnReady)
                {
                    SqlCommand markupCommand = new SqlCommand("SELECT ISNULL(MarkupPercent,30) FROM Dishes WHERE ID_dish=@id", db.getConnection());
                    markupCommand.Parameters.Add("@id", SqlDbType.Int).Value = dishId;
                    object markupValue = markupCommand.ExecuteScalar();
                    if (markupValue != null && markupValue != DBNull.Value)
                    {
                        MarkupPercent.Text = Convert.ToDecimal(markupValue).ToString("0.##", CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    MarkupPercent.Text = "30";
                }

                RecalculatePriceFromCostPrice();
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

        private void RefreshDishesGrid()
        {
            DB db = new DB();
            try
            {
                db.openConnection();
                dataGridView1.Rows.Clear();
                SqlCommand command = new SqlCommand(
                    "SELECT Name_of_dish,Cost,ISNULL(TypeOfDish,'') AS TypeOfDish FROM Dishes LEFT JOIN Type_of_Dishes on Dishes.Type_of_dish=Type_of_Dishes.Type_of_d",
                    db.getConnection());
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

        private bool TryResolveSelectedDishId(DB db, out int dishId)
        {
            dishId = id_dish;
            if (dishId > 0)
                return true;

            string dishName = name.Text?.Trim();
            if (string.IsNullOrWhiteSpace(dishName))
                return false;

            db.openConnection();
            SqlCommand command = new SqlCommand("SELECT TOP 1 ID_dish FROM Dishes WHERE Name_of_dish=@name", db.getConnection());
            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = dishName;
            object value = command.ExecuteScalar();
            db.closeConnection();

            if (value == null || value == DBNull.Value)
                return false;

            dishId = Convert.ToInt32(value);
            id_dish = dishId;
            return true;
        }

        private bool IsDishInActiveOrders(DB db, int dishId)
        {
            SqlCommand command = new SqlCommand(
                "SELECT COUNT(*) " +
                "FROM Orders o " +
                "WHERE o.ID_dish=@id " +
                "AND NOT EXISTS (SELECT 1 FROM Check_Form c WHERE c.ID_order=o.ID_order)",
                db.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = dishId;
            int activeCount = Convert.ToInt32(command.ExecuteScalar());
            return activeCount > 0;
        }

        private decimal GetDishCostPrice(int dishId, DB db)
        {
            SqlCommand command = new SqlCommand(
                "SELECT ISNULL(SUM(CAST(Calculation.Amount AS decimal(18,3)) * CAST(Products.Cost AS decimal(18,2))), 0) " +
                "FROM Calculation INNER JOIN Products ON Calculation.ID_product = Products.ID_product " +
                "WHERE Calculation.ID_dish=@id",
                db.getConnection());
            command.Parameters.Add("@id", SqlDbType.Int).Value = dishId;
            object result = command.ExecuteScalar();
            return (result == null || result == DBNull.Value) ? 0m : Convert.ToDecimal(result);
        }

        private bool EnsureDishMarkupColumnSupportsAutoPricing()
        {
            DB db = new DB();
            try
            {
                db.openConnection();
                SqlCommand command = new SqlCommand(
                    "IF COL_LENGTH('Dishes', 'MarkupPercent') IS NULL " +
                    "BEGIN " +
                    "ALTER TABLE Dishes ADD MarkupPercent decimal(5,2) NULL; " +
                    "UPDATE Dishes SET MarkupPercent = 30 WHERE MarkupPercent IS NULL; " +
                    "ALTER TABLE Dishes ALTER COLUMN MarkupPercent decimal(5,2) NOT NULL; " +
                    "END " +
                    "ELSE " +
                    "BEGIN " +
                    "UPDATE Dishes SET MarkupPercent = 30 WHERE MarkupPercent IS NULL; " +
                    "END",
                    db.getConnection());
                command.ExecuteNonQuery();
                return DishMarkupColumnExists(db);
            }
            catch
            {
                return false;
            }
            finally
            {
                db.closeConnection();
            }
        }

        private bool DishMarkupColumnExists(DB db)
        {
            SqlCommand check = new SqlCommand(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Dishes' AND COLUMN_NAME='MarkupPercent'",
                db.getConnection());
            int count = Convert.ToInt32(check.ExecuteScalar());
            return count > 0;
        }
    }
}
