using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            LoadStats();
            LoadRecentChecks();
            LoadPopularDishes();
        }

        private void LoadStats()
        {
            DB db = new DB();
            try
            {
                db.openConnection();

                SqlCommand cmdTodayOrders = new SqlCommand(
                    "SELECT COUNT(DISTINCT ID_order) FROM Check_Form WHERE CAST(Date_of_order AS DATE) = CAST(GETDATE() AS DATE)",
                    db.getConnection());
                SqlCommand cmdActiveOrders = new SqlCommand(
                    "SELECT COUNT(DISTINCT ID_order) FROM Orders",
                    db.getConnection());
                SqlCommand cmdRevenueToday = new SqlCommand(
                    "SELECT SUM(Summ) FROM Check_Form WHERE CAST(Date_of_order AS DATE) = CAST(GETDATE() AS DATE)",
                    db.getConnection());
                SqlCommand cmdTotalDishes = new SqlCommand(
                    "SELECT COUNT(*) FROM Dishes",
                    db.getConnection());

                lblTodayOrdersValue.Text = Convert.ToInt32(cmdTodayOrders.ExecuteScalar()).ToString();
                lblActiveOrdersValue.Text = Convert.ToInt32(cmdActiveOrders.ExecuteScalar()).ToString();

                object revenueObj = cmdRevenueToday.ExecuteScalar();
                decimal revenue = 0m;
                if (revenueObj != DBNull.Value && revenueObj != null)
                    revenue = Convert.ToDecimal(revenueObj);
                lblRevenueTodayValue.Text = revenue.ToString("0.##");

                lblTotalDishesValue.Text = Convert.ToInt32(cmdTotalDishes.ExecuteScalar()).ToString();
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

        private void LoadRecentChecks()
        {
            DB db = new DB();
            try
            {
                dgvRecentChecks.Rows.Clear();
                dgvRecentChecks.ReadOnly = true;
                dgvRecentChecks.AllowUserToAddRows = false;

                db.openConnection();
                SqlCommand cmd = new SqlCommand(
                    "SELECT TOP 10 ID_order, Date_of_order, Summ FROM Check_Form ORDER BY Date_of_order DESC",
                    db.getConnection());

                SqlDataReader reader = cmd.ExecuteReader();
                List<string[]> list = new List<string[]>();
                while (reader.Read())
                {
                    list.Add(new string[3]);
                    list[list.Count - 1][0] = reader[0].ToString();
                    list[list.Count - 1][1] = Convert.ToDateTime(reader[1]).ToString("dd.MM.yyyy");
                    list[list.Count - 1][2] = reader[2].ToString();
                }
                reader.Close();

                foreach (string[] s in list)
                {
                    dgvRecentChecks.Rows.Add(s);
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

        private void LoadPopularDishes()
        {
            DB db = new DB();
            try
            {
                dgvPopular.Rows.Clear();
                dgvPopular.ReadOnly = true;
                dgvPopular.AllowUserToAddRows = false;

                db.openConnection();
                SqlCommand cmd = new SqlCommand(
                    "SELECT Dishes.Name_of_dish, SUM(Orders.Amount) AS Sales FROM Orders JOIN Dishes ON Orders.ID_dish = Dishes.ID_dish GROUP BY Dishes.Name_of_dish ORDER BY Sales DESC",
                    db.getConnection());

                SqlDataReader reader = cmd.ExecuteReader();
                List<string[]> list = new List<string[]>();
                while (reader.Read())
                {
                    list.Add(new string[2]);
                    list[list.Count - 1][0] = reader[0].ToString();
                    list[list.Count - 1][1] = reader[1].ToString();
                }
                reader.Close();

                foreach (string[] s in list)
                {
                    dgvPopular.Rows.Add(s);
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

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            var main = this.ParentForm as Main;
            if (main != null)
                main.OpenChildForm(new Orders());
        }

        private void btnAddDish_Click(object sender, EventArgs e)
        {
            var main = this.ParentForm as Main;
            if (main != null)
                main.OpenChildForm(new Dishes());
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            var main = this.ParentForm as Main;
            if (main != null)
                main.OpenChildForm(new Products());
        }

        private void btnTodayReport_Click(object sender, EventArgs e)
        {
            var main = this.ParentForm as Main;
            if (main != null)
                main.OpenChildForm(new Report());
        }

        private void dgvRecentChecks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
