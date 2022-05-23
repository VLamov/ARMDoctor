using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace ARMDoctor
{
    public partial class treatment_methods : Form
    {
        private DataTable dt;
        private DataRow row;
        private String t_tm, sql;
        private String k = DB.k();
        private const String tablename = "Treatment_methods";
        private const String id = "Treatment_number";
        private const String name = "Treatment_number_name";
        private const String nname = "Метод лечения";
        private int rowIndex;
        private int lastIndex;

        public treatment_methods()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Boolean CorrectField()
        {
            //Boolean AllNice = false;
            if (string.IsNullOrEmpty(textBox.Text.Trim()))
            {
                textBox.Select();
                string head = "Внимание! "+ nname+" не указан";
                string str = "Введите, пожалуйста " + nname.ToLower();
                MessageBox.Show(str, head,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;

        }

        private void listBoxDT_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox.Text = (sender as ListBox).Text.Trim();
            if ((sender as ListBox).SelectedIndex == 0)
                rowIndex = 1;
            else
            {
                rowIndex = Int32.Parse((sender as ListBox).SelectedValue.ToString());
            }
        }

        private void Execute(string sql, string parameters)
        {
            DB db = new DB();
            DB.command = new NpgsqlCommand(sql, db.getConnection());
            addparameters(parameters);
        }

        private void addparameters(string parameters)
        {
            DB.command.Parameters.Clear();
            DB.command.Parameters.AddWithValue("@name", textBox.Text.Trim());
            if (parameters == "Update" || parameters == "Delete" && !string.IsNullOrEmpty(rowIndex.ToString()))
            {
                DB.command.Parameters.AddWithValue("@id", rowIndex);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            String text = textBox.Text.Trim();
            if (CorrectField())
            {
                rowIndex = listBoxDT.FindString(text);
                if (rowIndex != -1)
                {
                    MessageBox.Show("Такой элемент уже имеется в списке", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    lastIndex = (int)dt.Compute("Max("+id+")", "");
                    rowIndex = lastIndex + 1;
                    row = dt.NewRow();
                    row[id] = rowIndex;
                    row[name] = text;
                    dt.Rows.Add(row);
                    dt.DefaultView.Sort = name;
                    int index = listBoxDT.FindString(text);
                    listBoxDT.SetSelected(index, true);
                    DB.sql = "INSERT INTO " + k + t_tm + k +
                       "VALUES  (@id, @name)";
                    Execute(DB.sql, "Insert");
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            String text = textBox.Text.Trim();
            if (CorrectField())
            {
                rowIndex = listBoxDT.SelectedIndex;
                if (rowIndex == -1)
                {
                    MessageBox.Show("Ой где это я?", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    row = dt.NewRow();
                    row[id] = rowIndex;
                    row[name] = text;
                    dt.Rows.InsertAt(row, rowIndex);
                    dt.Rows.RemoveAt(rowIndex);
                    dt.DefaultView.Sort = name;
                    int index = listBoxDT.FindString(text);
                    listBoxDT.SetSelected(index, true);
                    DB.sql = "UPDATE " + k + t_tm + k + "SET " +
                       k + name + k + " = @name " +
                       "WHERE " + k + id + k + " == @id";
                    Execute(DB.sql, "Update");
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (rowIndex == -1)
            {
                MessageBox.Show("Ой где это я?", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dt.Rows.RemoveAt(rowIndex);
                rowIndex = listBoxDT.SelectedIndex;
                DB.sql = "DELETE FROM " + k + t_tm + k + "(" +
                   k + id + k + ", " +
                   k + name + k + ", " +
                   "VALUES  @id, @name";
                Execute(DB.sql, "Delete");
            }
        }

        private void treatment_methods_Load(object sender, EventArgs e)
        {
            t_tm = k + tablename + k;
            sql = "Select DISTINCT * from " + t_tm +
                "ORDER by " + k + name + k;
            dt = DB.SQLGetTable(sql);
            listBoxDT.DataSource = dt;
            listBoxDT.DisplayMember = name;
            listBoxDT.ValueMember = id;
            rowIndex = 1;
            lastIndex = listBoxDT.Items.Count;
        }
    }
}
