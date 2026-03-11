using System.Drawing;
using System.Windows.Forms;

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
    }
}
