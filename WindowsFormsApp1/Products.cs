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
        public Products()
        {
            InitializeComponent();
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

        private void comboBox1_MouseHover(object sender, EventArgs e)
        {
            LoadUnitsCombo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                decimal cost = Convert.ToDecimal(textBox2.Text);
                string name = textBox1.Text;
                if (comboBox1.SelectedValue == null)
                {
                    MessageBox.Show("Выберите единицу измерения");
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
