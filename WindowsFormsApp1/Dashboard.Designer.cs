namespace WindowsFormsApp1
{
    partial class Dashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.groupStats = new System.Windows.Forms.GroupBox();
            this.lblTotalDishesValue = new System.Windows.Forms.Label();
            this.lblTotalDishes = new System.Windows.Forms.Label();
            this.lblRevenueTodayValue = new System.Windows.Forms.Label();
            this.lblRevenueToday = new System.Windows.Forms.Label();
            this.lblActiveOrdersValue = new System.Windows.Forms.Label();
            this.lblActiveOrders = new System.Windows.Forms.Label();
            this.lblTodayOrdersValue = new System.Windows.Forms.Label();
            this.lblTodayOrders = new System.Windows.Forms.Label();
            this.groupActions = new System.Windows.Forms.GroupBox();
            this.btnTodayReport = new System.Windows.Forms.Button();
            this.btnAddProduct = new System.Windows.Forms.Button();
            this.btnAddDish = new System.Windows.Forms.Button();
            this.btnCreateOrder = new System.Windows.Forms.Button();
            this.groupRecent = new System.Windows.Forms.GroupBox();
            this.dgvRecentChecks = new System.Windows.Forms.DataGridView();
            this.colOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupPopular = new System.Windows.Forms.GroupBox();
            this.dgvPopular = new System.Windows.Forms.DataGridView();
            this.colDish = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSales = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupStats.SuspendLayout();
            this.groupActions.SuspendLayout();
            this.groupRecent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentChecks)).BeginInit();
            this.groupPopular.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPopular)).BeginInit();
            this.SuspendLayout();
            // 
            // groupStats
            // 
            this.groupStats.Controls.Add(this.lblTotalDishesValue);
            this.groupStats.Controls.Add(this.lblTotalDishes);
            this.groupStats.Controls.Add(this.lblRevenueTodayValue);
            this.groupStats.Controls.Add(this.lblRevenueToday);
            this.groupStats.Controls.Add(this.lblActiveOrdersValue);
            this.groupStats.Controls.Add(this.lblActiveOrders);
            this.groupStats.Controls.Add(this.lblTodayOrdersValue);
            this.groupStats.Controls.Add(this.lblTodayOrders);
            this.groupStats.Location = new System.Drawing.Point(20, 20);
            this.groupStats.Name = "groupStats";
            this.groupStats.Size = new System.Drawing.Size(827, 90);
            this.groupStats.TabIndex = 0;
            this.groupStats.TabStop = false;
            this.groupStats.Text = "Статистика";
            // 
            // lblTotalDishesValue
            // 
            this.lblTotalDishesValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotalDishesValue.Location = new System.Drawing.Point(640, 50);
            this.lblTotalDishesValue.Name = "lblTotalDishesValue";
            this.lblTotalDishesValue.Size = new System.Drawing.Size(150, 20);
            this.lblTotalDishesValue.TabIndex = 7;
            this.lblTotalDishesValue.Text = "0";
            this.lblTotalDishesValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalDishes
            // 
            this.lblTotalDishes.AutoSize = true;
            this.lblTotalDishes.Location = new System.Drawing.Point(640, 25);
            this.lblTotalDishes.Name = "lblTotalDishes";
            this.lblTotalDishes.Size = new System.Drawing.Size(66, 13);
            this.lblTotalDishes.TabIndex = 6;
            this.lblTotalDishes.Text = "Всего блюд";
            // 
            // lblRevenueTodayValue
            // 
            this.lblRevenueTodayValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lblRevenueTodayValue.Location = new System.Drawing.Point(420, 50);
            this.lblRevenueTodayValue.Name = "lblRevenueTodayValue";
            this.lblRevenueTodayValue.Size = new System.Drawing.Size(150, 20);
            this.lblRevenueTodayValue.TabIndex = 5;
            this.lblRevenueTodayValue.Text = "0";
            this.lblRevenueTodayValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRevenueToday
            // 
            this.lblRevenueToday.AutoSize = true;
            this.lblRevenueToday.Location = new System.Drawing.Point(420, 25);
            this.lblRevenueToday.Name = "lblRevenueToday";
            this.lblRevenueToday.Size = new System.Drawing.Size(94, 13);
            this.lblRevenueToday.TabIndex = 4;
            this.lblRevenueToday.Text = "Выручка сегодня";
            // 
            // lblActiveOrdersValue
            // 
            this.lblActiveOrdersValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lblActiveOrdersValue.Location = new System.Drawing.Point(210, 50);
            this.lblActiveOrdersValue.Name = "lblActiveOrdersValue";
            this.lblActiveOrdersValue.Size = new System.Drawing.Size(150, 20);
            this.lblActiveOrdersValue.TabIndex = 3;
            this.lblActiveOrdersValue.Text = "0";
            this.lblActiveOrdersValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblActiveOrders
            // 
            this.lblActiveOrders.AutoSize = true;
            this.lblActiveOrders.Location = new System.Drawing.Point(210, 25);
            this.lblActiveOrders.Name = "lblActiveOrders";
            this.lblActiveOrders.Size = new System.Drawing.Size(98, 13);
            this.lblActiveOrders.TabIndex = 2;
            this.lblActiveOrders.Text = "Активные заказы";
            // 
            // lblTodayOrdersValue
            // 
            this.lblTodayOrdersValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lblTodayOrdersValue.Location = new System.Drawing.Point(10, 50);
            this.lblTodayOrdersValue.Name = "lblTodayOrdersValue";
            this.lblTodayOrdersValue.Size = new System.Drawing.Size(150, 20);
            this.lblTodayOrdersValue.TabIndex = 1;
            this.lblTodayOrdersValue.Text = "0";
            this.lblTodayOrdersValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTodayOrders
            // 
            this.lblTodayOrders.AutoSize = true;
            this.lblTodayOrders.Location = new System.Drawing.Point(10, 25);
            this.lblTodayOrders.Name = "lblTodayOrders";
            this.lblTodayOrders.Size = new System.Drawing.Size(94, 13);
            this.lblTodayOrders.TabIndex = 0;
            this.lblTodayOrders.Text = "Сегодня заказов";
            // 
            // groupActions
            // 
            this.groupActions.Controls.Add(this.btnTodayReport);
            this.groupActions.Controls.Add(this.btnAddProduct);
            this.groupActions.Controls.Add(this.btnAddDish);
            this.groupActions.Controls.Add(this.btnCreateOrder);
            this.groupActions.Location = new System.Drawing.Point(20, 120);
            this.groupActions.Name = "groupActions";
            this.groupActions.Size = new System.Drawing.Size(827, 70);
            this.groupActions.TabIndex = 1;
            this.groupActions.TabStop = false;
            this.groupActions.Text = "Быстрые действия";
            // 
            // btnTodayReport
            // 
            this.btnTodayReport.Location = new System.Drawing.Point(624, 25);
            this.btnTodayReport.Name = "btnTodayReport";
            this.btnTodayReport.Size = new System.Drawing.Size(180, 30);
            this.btnTodayReport.TabIndex = 3;
            this.btnTodayReport.Text = "Отчет за сегодня";
            this.btnTodayReport.UseVisualStyleBackColor = true;
            this.btnTodayReport.Click += new System.EventHandler(this.btnTodayReport_Click);
            // 
            // btnAddProduct
            // 
            this.btnAddProduct.Location = new System.Drawing.Point(420, 25);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(180, 30);
            this.btnAddProduct.TabIndex = 2;
            this.btnAddProduct.Text = "Добавить продукт";
            this.btnAddProduct.UseVisualStyleBackColor = true;
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            // 
            // btnAddDish
            // 
            this.btnAddDish.Location = new System.Drawing.Point(214, 25);
            this.btnAddDish.Name = "btnAddDish";
            this.btnAddDish.Size = new System.Drawing.Size(180, 30);
            this.btnAddDish.TabIndex = 1;
            this.btnAddDish.Text = "Добавить блюдо";
            this.btnAddDish.UseVisualStyleBackColor = true;
            this.btnAddDish.Click += new System.EventHandler(this.btnAddDish_Click);
            // 
            // btnCreateOrder
            // 
            this.btnCreateOrder.Location = new System.Drawing.Point(10, 25);
            this.btnCreateOrder.Name = "btnCreateOrder";
            this.btnCreateOrder.Size = new System.Drawing.Size(180, 30);
            this.btnCreateOrder.TabIndex = 0;
            this.btnCreateOrder.Text = "Создать заказ";
            this.btnCreateOrder.UseVisualStyleBackColor = true;
            this.btnCreateOrder.Click += new System.EventHandler(this.btnCreateOrder_Click);
            // 
            // groupRecent
            // 
            this.groupRecent.Controls.Add(this.dgvRecentChecks);
            this.groupRecent.Location = new System.Drawing.Point(20, 200);
            this.groupRecent.Name = "groupRecent";
            this.groupRecent.Size = new System.Drawing.Size(400, 260);
            this.groupRecent.TabIndex = 2;
            this.groupRecent.TabStop = false;
            this.groupRecent.Text = "Последние оплаченные заказы";
            // 
            // dgvRecentChecks
            // 
            this.dgvRecentChecks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecentChecks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOrder,
            this.colDate,
            this.colSum});
            this.dgvRecentChecks.Location = new System.Drawing.Point(10, 19);
            this.dgvRecentChecks.Name = "dgvRecentChecks";
            this.dgvRecentChecks.RowHeadersWidth = 51;
            this.dgvRecentChecks.Size = new System.Drawing.Size(368, 241);
            this.dgvRecentChecks.TabIndex = 0;
            this.dgvRecentChecks.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRecentChecks_CellContentClick);
            // 
            // colOrder
            // 
            this.colOrder.HeaderText = "№ заказа";
            this.colOrder.MinimumWidth = 6;
            this.colOrder.Name = "colOrder";
            this.colOrder.Width = 90;
            // 
            // colDate
            // 
            this.colDate.HeaderText = "Дата";
            this.colDate.MinimumWidth = 6;
            this.colDate.Name = "colDate";
            this.colDate.Width = 110;
            // 
            // colSum
            // 
            this.colSum.HeaderText = "Сумма";
            this.colSum.MinimumWidth = 6;
            this.colSum.Name = "colSum";
            this.colSum.Width = 120;
            // 
            // groupPopular
            // 
            this.groupPopular.Controls.Add(this.dgvPopular);
            this.groupPopular.Location = new System.Drawing.Point(447, 200);
            this.groupPopular.Name = "groupPopular";
            this.groupPopular.Size = new System.Drawing.Size(400, 260);
            this.groupPopular.TabIndex = 3;
            this.groupPopular.TabStop = false;
            this.groupPopular.Text = "Популярные блюда";
            // 
            // dgvPopular
            // 
            this.dgvPopular.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPopular.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDish,
            this.colSales});
            this.dgvPopular.Location = new System.Drawing.Point(0, 19);
            this.dgvPopular.Name = "dgvPopular";
            this.dgvPopular.RowHeadersWidth = 51;
            this.dgvPopular.Size = new System.Drawing.Size(380, 241);
            this.dgvPopular.TabIndex = 0;
            // 
            // colDish
            // 
            this.colDish.HeaderText = "Блюдо";
            this.colDish.MinimumWidth = 6;
            this.colDish.Name = "colDish";
            this.colDish.Width = 170;
            // 
            // colSales
            // 
            this.colSales.HeaderText = "Продажи";
            this.colSales.MinimumWidth = 6;
            this.colSales.Name = "colSales";
            this.colSales.Width = 120;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 679);
            this.Controls.Add(this.groupPopular);
            this.Controls.Add(this.groupRecent);
            this.Controls.Add(this.groupActions);
            this.Controls.Add(this.groupStats);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.groupStats.ResumeLayout(false);
            this.groupStats.PerformLayout();
            this.groupActions.ResumeLayout(false);
            this.groupRecent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentChecks)).EndInit();
            this.groupPopular.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPopular)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupStats;
        private System.Windows.Forms.Label lblTodayOrders;
        private System.Windows.Forms.Label lblTodayOrdersValue;
        private System.Windows.Forms.Label lblActiveOrders;
        private System.Windows.Forms.Label lblActiveOrdersValue;
        private System.Windows.Forms.Label lblRevenueToday;
        private System.Windows.Forms.Label lblRevenueTodayValue;
        private System.Windows.Forms.Label lblTotalDishes;
        private System.Windows.Forms.Label lblTotalDishesValue;
        private System.Windows.Forms.GroupBox groupActions;
        private System.Windows.Forms.Button btnCreateOrder;
        private System.Windows.Forms.Button btnAddDish;
        private System.Windows.Forms.Button btnAddProduct;
        private System.Windows.Forms.Button btnTodayReport;
        private System.Windows.Forms.GroupBox groupRecent;
        private System.Windows.Forms.DataGridView dgvRecentChecks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSum;
        private System.Windows.Forms.GroupBox groupPopular;
        private System.Windows.Forms.DataGridView dgvPopular;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDish;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSales;
    }
}
