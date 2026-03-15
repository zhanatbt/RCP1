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
    public partial class Products : Form
    {
        private int selectedProductId = 0;

        public Products()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            UIStyle.AddRefreshButton(this, () => new Products());
        }

        private void Products_Load(object sender, EventArgs e)
        {
            LoadUnitsCombo();
            LoadProductsGrid();
        }

        private void LoadUnitsCombo()
        {
            DB db = new DB();
            DataTable table = new DataTable();
            try
            {
                db.openConnection();
                SqlCommand command = new SqlCommand("SELECT * FROM Unit", db.getConnection());
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                comboBox1.DisplayMember = "Unit";
                comboBox1.ValueMember = "ID_unit";
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

        private void LoadProductsGrid()
        {
            DB db = new DB();
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.CellClick -= dataGridView1_CellClick;
                dataGridView1.CellClick += dataGridView1_CellClick;

                db.openConnection();
                SqlCommand command = new SqlCommand("SELECT Products.ID_product, Products.Name_of_prod, Unit.Unit, Products.Cost FROM Products JOIN Unit ON Products.ID_unit = Unit.ID_unit", db.getConnection());
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
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Выберите продукт");
                return;
            }

            object idObj = dataGridView1.CurrentRow.Cells[0].Value;
            int id;
            if (idObj == null || !int.TryParse(idObj.ToString(), out id))
            {
                MessageBox.Show("Неверный идентификатор продукта");
                return;
            }

            DB db = new DB();
            try
            {
                db.openConnection();
                if (IsProductUsed(db, id))
                {
                    MessageBox.Show("Нельзя удалить: продукт используется в блюдах");
                    return;
                }
                SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ID_product=@id", db.getConnection());
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                int rows = cmd.ExecuteNonQuery();
                if (rows >= 1)
                    MessageBox.Show("Продукт удален");
                else
                    MessageBox.Show("Продукт не найден");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.closeConnection();
            }

            LoadProductsGrid();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selectedProductId == 0)
            {
                MessageBox.Show("Выберите продукт в таблице");
                return;
            }

            string name = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введите наименование");
                return;
            }
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Выберите единицу измерения");
                return;
            }
            if (!decimal.TryParse(textBox2.Text, out decimal cost) || cost < 0)
            {
                MessageBox.Show("Введите корректную цену");
                return;
            }

            int unitId = Convert.ToInt32(comboBox1.SelectedValue);

            DB db = new DB();
            try
            {
                db.openConnection();
                SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Products WHERE Name_of_prod=@name AND ID_product<>@id", db.getConnection());
                check.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
                check.Parameters.Add("@id", SqlDbType.Int).Value = selectedProductId;
                int exists = Convert.ToInt32(check.ExecuteScalar());
                if (exists > 0)
                {
                    MessageBox.Show("Продукт с таким названием уже есть");
                    return;
                }

                SqlCommand cmd = new SqlCommand("UPDATE Products SET Name_of_prod=@name, ID_unit=@unit, Cost=@cost WHERE ID_product=@id", db.getConnection());
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
                cmd.Parameters.Add("@unit", SqlDbType.Int).Value = unitId;
                var pCost = cmd.Parameters.Add("@cost", SqlDbType.Decimal);
                pCost.Precision = 18;
                pCost.Scale = 2;
                pCost.Value = cost;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = selectedProductId;

                if (cmd.ExecuteNonQuery() == 1)
                    MessageBox.Show("Продукт изменен");
                else
                    MessageBox.Show("Ошибка обновления");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.closeConnection();
            }

            LoadProductsGrid();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            SelectProductFromRow(row);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Выберите продукт");
                return;
            }
            SelectProductFromRow(dataGridView1.CurrentRow);
        }

        private void SelectProductFromRow(DataGridViewRow row)
        {
            if (row == null || row.IsNewRow) return;
            if (row.Cells[0].Value == null) return;
            if (!int.TryParse(row.Cells[0].Value.ToString(), out int id)) return;

            selectedProductId = id;
            textBox1.Text = row.Cells[1].Value?.ToString();
            comboBox1.Text = row.Cells[2].Value?.ToString();
            textBox2.Text = row.Cells[3].Value?.ToString();
            tabControl1.SelectedTab = tabPageAdd;
        }


        private bool IsProductUsed(DB db, int id)
        {
            SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Calculation WHERE ID_product=@id", db.getConnection());
            check.Parameters.Add("@id", SqlDbType.Int).Value = id;
            object res = check.ExecuteScalar();
            int count = (res == null || res == DBNull.Value) ? 0 : Convert.ToInt32(res);
            return count > 0;
        }


        private void comboBox1_MouseHover(object sender, EventArgs e)
        {
            LoadUnitsCombo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string name = textBox1.Text.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Введите наименование");
                    return;
                }
                if (comboBox1.SelectedValue == null)
                {
                    MessageBox.Show("Выберите единицу измерения");
                    return;
                }
                decimal cost;
                if (!decimal.TryParse(textBox2.Text, out cost) || cost < 0)
                {
                    MessageBox.Show("Введите корректную цену");
                    return;
                }
                int unitId = Convert.ToInt32(comboBox1.SelectedValue);

                DB db = new DB();

                db.openConnection();

                SqlCommand command = new SqlCommand("SELECT Name_of_prod FROM Products WHERE Name_of_prod=@name", db.getConnection());
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;

                SqlDataAdapter adapter = new SqlDataAdapter();

                DataTable table = new DataTable();

                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                    MessageBox.Show("Такой ингридиент уже есть");
                else
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Products VALUES(@Name_of_prod, @id_unit , @cost)", db.getConnection());
                    cmd.Parameters.Add("@Name_of_prod", SqlDbType.NVarChar).Value = name;
                    cmd.Parameters.Add("@id_unit", SqlDbType.Int).Value = unitId;
                    var pCost = cmd.Parameters.Add("@cost", SqlDbType.Decimal);
                    pCost.Precision = 18;
                    pCost.Scale = 2;
                    pCost.Value = cost;
                    if (cmd.ExecuteNonQuery() == 1)
                        MessageBox.Show("Добавлено");
                    else
                        MessageBox.Show("Ошибка! Продукт не добавлен");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            LoadProductsGrid();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
