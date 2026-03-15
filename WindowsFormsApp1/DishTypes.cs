using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class DishTypes : Form
    {
        private readonly DataGridView dgvTypes = new DataGridView();
        private readonly DataGridView dgvPopularTypes = new DataGridView();
        private readonly TextBox txtTypeName = new TextBox();
        private readonly Button btnAdd = new Button();
        private readonly Button btnUpdate = new Button();
        private readonly Button btnDelete = new Button();
        private readonly Button btnBack = new Button();
        private readonly Label lblList = new Label();
        private readonly Label lblActions = new Label();
        private readonly Label lblPopular = new Label();
        private readonly Label lblTypeName = new Label();

        private int selectedTypeId = 0;

        public DishTypes()
        {
            InitializeUi();
            UIStyle.Apply(this);
            UIStyle.AddRefreshButton(this, () => new DishTypes());
            Load += DishTypes_Load;
        }

        private void InitializeUi()
        {
            Text = "Типы блюд";
            ClientSize = new Size(1050, 680);

            btnBack.Text = "Назад";
            btnBack.Location = new Point(16, 14);
            btnBack.Size = new Size(70, 30);
            btnBack.Click += (s, e) => Close();

            lblList.Text = "Список типов блюд";
            lblList.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            lblList.Location = new Point(20, 56);
            lblList.AutoSize = true;

            dgvTypes.Location = new Point(20, 80);
            dgvTypes.Size = new Size(370, 560);
            dgvTypes.ReadOnly = true;
            dgvTypes.AllowUserToAddRows = false;
            dgvTypes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTypes.MultiSelect = false;
            dgvTypes.Columns.Add("colTypeId", "ID");
            dgvTypes.Columns["colTypeId"].Width = 60;
            dgvTypes.Columns.Add("colTypeName", "Тип блюда");
            dgvTypes.Columns["colTypeName"].Width = 260;
            dgvTypes.EnableHeadersVisualStyles = false;
            dgvTypes.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgvTypes.ColumnHeadersDefaultCellStyle.BackColor;
            dgvTypes.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgvTypes.ColumnHeadersDefaultCellStyle.ForeColor;
            dgvTypes.CellClick += DgvTypes_CellClick;

            lblActions.Text = "Действия";
            lblActions.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            lblActions.Location = new Point(420, 80);
            lblActions.AutoSize = true;

            lblTypeName.Text = "Название типа";
            lblTypeName.Location = new Point(420, 116);
            lblTypeName.AutoSize = true;

            txtTypeName.Location = new Point(420, 138);
            txtTypeName.Size = new Size(250, 22);

            btnAdd.Text = "Добавить";
            btnAdd.Location = new Point(420, 180);
            btnAdd.Size = new Size(120, 34);
            btnAdd.Click += BtnAdd_Click;

            btnUpdate.Text = "Изменить";
            btnUpdate.Location = new Point(550, 180);
            btnUpdate.Size = new Size(120, 34);
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete.Text = "Удалить";
            btnDelete.Location = new Point(680, 180);
            btnDelete.Size = new Size(120, 34);
            btnDelete.Click += BtnDelete_Click;

            lblPopular.Text = "Популярные типы за месяц";
            lblPopular.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            lblPopular.Location = new Point(420, 250);
            lblPopular.AutoSize = true;

            dgvPopularTypes.Location = new Point(420, 274);
            dgvPopularTypes.Size = new Size(390, 366);
            dgvPopularTypes.ReadOnly = true;
            dgvPopularTypes.AllowUserToAddRows = false;
            dgvPopularTypes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPopularTypes.MultiSelect = false;
            dgvPopularTypes.Columns.Add("colPopType", "Тип блюда");
            dgvPopularTypes.Columns["colPopType"].Width = 230;
            dgvPopularTypes.Columns.Add("colPopSales", "Продано");
            dgvPopularTypes.Columns["colPopSales"].Width = 120;
            dgvPopularTypes.EnableHeadersVisualStyles = false;
            dgvPopularTypes.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgvPopularTypes.ColumnHeadersDefaultCellStyle.BackColor;
            dgvPopularTypes.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgvPopularTypes.ColumnHeadersDefaultCellStyle.ForeColor;

            Controls.Add(btnBack);
            Controls.Add(lblList);
            Controls.Add(dgvTypes);
            Controls.Add(lblActions);
            Controls.Add(lblTypeName);
            Controls.Add(txtTypeName);
            Controls.Add(btnAdd);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);
            Controls.Add(lblPopular);
            Controls.Add(dgvPopularTypes);
        }

        private void DishTypes_Load(object sender, EventArgs e)
        {
            EnsureUnknownTypeExists();
            LoadTypes();
            LoadPopularTypes();
        }

        private void DgvTypes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvTypes.Rows[e.RowIndex];
            if (row.Cells[0].Value == null) return;
            if (!int.TryParse(row.Cells[0].Value.ToString(), out int id)) return;

            selectedTypeId = id;
            txtTypeName.Text = row.Cells[1].Value?.ToString();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string typeName = txtTypeName.Text.Trim();
            if (string.IsNullOrWhiteSpace(typeName))
            {
                MessageBox.Show("Введите название типа");
                return;
            }

            DB db = new DB();
            try
            {
                db.openConnection();
                SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Type_of_Dishes WHERE TypeOfDish=@name", db.getConnection());
                check.Parameters.Add("@name", SqlDbType.NVarChar).Value = typeName;
                int exists = Convert.ToInt32(check.ExecuteScalar());
                if (exists > 0)
                {
                    MessageBox.Show("Такой тип уже есть");
                    return;
                }

                SqlCommand insert = new SqlCommand("INSERT INTO Type_of_Dishes(TypeOfDish) VALUES (@name)", db.getConnection());
                insert.Parameters.Add("@name", SqlDbType.NVarChar).Value = typeName;
                insert.ExecuteNonQuery();
                MessageBox.Show("Тип блюда добавлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.closeConnection();
            }

            txtTypeName.Clear();
            selectedTypeId = 0;
            LoadTypes();
            LoadPopularTypes();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedTypeId <= 0)
            {
                MessageBox.Show("Выберите тип в таблице");
                return;
            }

            string newName = txtTypeName.Text.Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Введите название типа");
                return;
            }

            DB db = new DB();
            try
            {
                db.openConnection();
                SqlCommand check = new SqlCommand(
                    "SELECT COUNT(*) FROM Type_of_Dishes WHERE TypeOfDish=@name AND Type_of_d<>@id",
                    db.getConnection());
                check.Parameters.Add("@name", SqlDbType.NVarChar).Value = newName;
                check.Parameters.Add("@id", SqlDbType.Int).Value = selectedTypeId;
                int exists = Convert.ToInt32(check.ExecuteScalar());
                if (exists > 0)
                {
                    MessageBox.Show("Тип с таким названием уже есть");
                    return;
                }

                SqlCommand update = new SqlCommand(
                    "UPDATE Type_of_Dishes SET TypeOfDish=@name WHERE Type_of_d=@id",
                    db.getConnection());
                update.Parameters.Add("@name", SqlDbType.NVarChar).Value = newName;
                update.Parameters.Add("@id", SqlDbType.Int).Value = selectedTypeId;
                update.ExecuteNonQuery();
                MessageBox.Show("Тип блюда изменен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.closeConnection();
            }

            LoadTypes();
            LoadPopularTypes();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedTypeId <= 0)
            {
                MessageBox.Show("Выберите тип в таблице");
                return;
            }

            DB db = new DB();
            try
            {
                db.openConnection();
                int unknownId = EnsureUnknownTypeExists(db);

                if (selectedTypeId == unknownId)
                {
                    MessageBox.Show("Тип 'Неизвестно' удалить нельзя");
                    return;
                }

                SqlCommand moveDishes = new SqlCommand(
                    "UPDATE Dishes SET Type_of_dish=@unknownId WHERE Type_of_dish=@deleteId",
                    db.getConnection());
                moveDishes.Parameters.Add("@unknownId", SqlDbType.Int).Value = unknownId;
                moveDishes.Parameters.Add("@deleteId", SqlDbType.Int).Value = selectedTypeId;
                moveDishes.ExecuteNonQuery();

                SqlCommand deleteType = new SqlCommand(
                    "DELETE FROM Type_of_Dishes WHERE Type_of_d=@id",
                    db.getConnection());
                deleteType.Parameters.Add("@id", SqlDbType.Int).Value = selectedTypeId;
                deleteType.ExecuteNonQuery();

                MessageBox.Show("Тип удален. Блюда перенесены в тип 'Неизвестно'.");
                selectedTypeId = 0;
                txtTypeName.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                db.closeConnection();
            }

            LoadTypes();
            LoadPopularTypes();
        }

        private void LoadTypes()
        {
            dgvTypes.Rows.Clear();
            DB db = new DB();
            try
            {
                db.openConnection();
                SqlCommand command = new SqlCommand(
                    "SELECT Type_of_d, TypeOfDish FROM Type_of_Dishes ORDER BY TypeOfDish ASC",
                    db.getConnection());

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dgvTypes.Rows.Add(reader[0].ToString(), reader[1].ToString());
                }
                reader.Close();
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

        private void LoadPopularTypes()
        {
            dgvPopularTypes.Rows.Clear();
            DB db = new DB();
            try
            {
                db.openConnection();
                SqlCommand command = new SqlCommand(
                    "SELECT ISNULL(t.TypeOfDish, N'Неизвестно') AS DishType, SUM(o.Amount) AS Sales " +
                    "FROM Orders o " +
                    "INNER JOIN Dishes d ON o.ID_dish=d.ID_dish " +
                    "LEFT JOIN Type_of_Dishes t ON d.Type_of_dish=t.Type_of_d " +
                    "WHERE YEAR(o.Date_of_order)=YEAR(GETDATE()) AND MONTH(o.Date_of_order)=MONTH(GETDATE()) " +
                    "GROUP BY ISNULL(t.TypeOfDish, N'Неизвестно') " +
                    "ORDER BY Sales DESC, DishType ASC",
                    db.getConnection());

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dgvPopularTypes.Rows.Add(reader[0].ToString(), reader[1].ToString());
                }
                reader.Close();
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

        private void EnsureUnknownTypeExists()
        {
            DB db = new DB();
            try
            {
                db.openConnection();
                EnsureUnknownTypeExists(db);
            }
            catch
            {
                // ignore
            }
            finally
            {
                db.closeConnection();
            }
        }

        private int EnsureUnknownTypeExists(DB db)
        {
            SqlCommand find = new SqlCommand(
                "SELECT TOP 1 Type_of_d FROM Type_of_Dishes WHERE TypeOfDish=N'Неизвестно'",
                db.getConnection());
            object existing = find.ExecuteScalar();
            if (existing != null && existing != DBNull.Value)
                return Convert.ToInt32(existing);

            SqlCommand insert = new SqlCommand(
                "INSERT INTO Type_of_Dishes(TypeOfDish) VALUES (N'Неизвестно'); SELECT CAST(SCOPE_IDENTITY() AS int);",
                db.getConnection());
            object id = insert.ExecuteScalar();
            return (id == null || id == DBNull.Value) ? 0 : Convert.ToInt32(id);
        }
    }
}
