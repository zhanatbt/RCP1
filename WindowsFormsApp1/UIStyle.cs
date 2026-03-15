using System.Drawing;
using System.Windows.Forms;
using System;

namespace WindowsFormsApp1
{
    public static class UIStyle
    {
        public static void Apply(Control root)
        {
            if (root == null) return;

            Color bg = Color.FromArgb(247, 248, 250);
            Color surface = Color.White;
            Color text = Color.FromArgb(45, 55, 72);
            Color border = Color.FromArgb(229, 231, 235);
            if (root is Form f)
            {
                f.BackColor = bg;
                f.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            }

            foreach (Control c in root.Controls)
            {
                if (c is Panel panel)
                {
                    if (panel.Name.ToLower().Contains("sidebar"))
                        panel.BackColor = Color.FromArgb(241, 243, 245);
                    else if (panel.Name.ToLower().Contains("header"))
                        panel.BackColor = Color.FromArgb(250, 251, 252);
                    else
                        panel.BackColor = surface;
                }
                else if (c is GroupBox gb)
                {
                    gb.ForeColor = text;
                    gb.BackColor = surface;
                }
                else if (c is Label lbl)
                {
                    lbl.ForeColor = text;
                }
                else if (c is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Standard;
                    btn.UseVisualStyleBackColor = true;
                    btn.ForeColor = Color.Black;
                }
                else if (c is DataGridView dgv)
                {
                    dgv.BackgroundColor = surface;
                    dgv.GridColor = border;
                    dgv.EnableHeadersVisualStyles = false;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 248);
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = text;
                    dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
                    dgv.DefaultCellStyle.ForeColor = text;
                    dgv.DefaultCellStyle.BackColor = surface;
                    dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
                }
                else if (c is TextBox tb)
                {
                    tb.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (c is ComboBox cb)
                {
                    cb.FlatStyle = FlatStyle.Flat;
                }

                if (c.HasChildren)
                    Apply(c);
            }
        }

        public static void AddRefreshButton(Form form, Func<Form> refreshFactory = null)
        {
            if (form == null) return;

            const string refreshButtonName = "buttonRefreshPage";
            Button button = form.Controls[refreshButtonName] as Button;
            if (button == null)
            {
                button = new Button();
                button.Name = refreshButtonName;
                button.Text = "Обновить";
                button.Size = new Size(90, 28);
                button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                button.Location = new Point(Math.Max(10, form.ClientSize.Width - button.Width - 10), 10);
                button.Click += RefreshButton_Click;
                form.Controls.Add(button);
                button.BringToFront();
            }

            button.Tag = refreshFactory;
        }

        private static void RefreshButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Form current = button.FindForm();
            if (current == null) return;

            Func<Form> factory = button.Tag as Func<Form>;
            if (factory == null)
            {
                try
                {
                    factory = () => (Form)Activator.CreateInstance(current.GetType());
                }
                catch
                {
                    MessageBox.Show("Не удалось обновить форму");
                    return;
                }
            }

            Form refreshed;
            try
            {
                refreshed = factory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            Main host = FindMainHost(current);
            if (host != null && !current.TopLevel)
            {
                host.OpenChildForm(refreshed);
                return;
            }

            if (current.Owner != null)
            {
                refreshed.StartPosition = current.StartPosition;
                refreshed.Show(current.Owner);
                current.Close();
                return;
            }

            refreshed.StartPosition = current.StartPosition;
            refreshed.Show();
            current.Close();
        }

        private static Main FindMainHost(Control control)
        {
            Control current = control;
            while (current != null)
            {
                if (current is Main main)
                    return main;

                current = current.Parent;
            }

            return null;
        }
    }
}
