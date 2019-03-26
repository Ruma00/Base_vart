using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Vart_Main
{
    public partial class Information : Form
    {
        String[] vs;
        SqlCommand command;
        bool[] flag = { false, false };
        object backup;

        String contract;
        //SqlConnection connection;
        ListView view;

        public Information(string contract, SqlConnection connection, ref ListView listViewS)
        {
            InitializeComponent();

            dataGridViewInf.RowTemplate.Height = 43;
            dataGridViewInf.AllowUserToAddRows = false;
            view = listViewS;

            this.contract = contract;

            Program.form.setButtonCtrInf(false);

            command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM ContractInf WHERE Contract_num = '" + contract + "'";
            
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                vs = new String[10];
                String s;
                dataGridViewInf.Rows.Add("№ договора:", reader.GetValue(0).ToString());
                dataGridViewInf.Rows.Add("Дата заключения:", reader.GetDateTime(1).ToShortDateString());
                dataGridViewInf.Rows.Add("ФИО:", reader.GetValue(2).ToString());
                dataGridViewInf.Rows.Add("Паспорт:", reader.GetValue(3).ToString());
                DateTime ttt = reader.GetDateTime(4);
                if (ttt.Year == 1900)
                    s = "";
                else
                    s = ttt.ToShortDateString();
                dataGridViewInf.Rows.Add("Дата выдачи:", s);
                dataGridViewInf.Rows.Add("Выдан:", reader.GetValue(5).ToString());
                dataGridViewInf.Rows.Add("Телефон:", reader.GetValue(6).ToString());
                ttt = reader.GetDateTime(7);
                if (ttt.Year == 1900)
                    s = "";
                else
                    s = ttt.ToShortDateString();
                dataGridViewInf.Rows.Add("Дата рождения:", s);
                dataGridViewInf.Rows.Add("Место рождения:", reader.GetValue(8).ToString());
                ttt = reader.GetDateTime(9);
                if (ttt.Year == 1900)
                    dataGridViewInf.Rows.Add("Дата расторжения:", "");
                else
                    dataGridViewInf.Rows.Add("Дата расторжения:", ttt.ToShortDateString());
            }
            reader.Close();

            command.CommandText = "SELECT Notice FROM Main WHERE Contract_num = '" + contract + "'";
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                richTextBox1.Text = reader.GetString(0);
            }
            reader.Close();
            dataGridViewInf.ClearSelection();

            for (int i = 0; i < 10; i++)
                vs[i] = dataGridViewInf.Rows[i].Cells[1].Value.ToString();

            command.CommandText = "SELECT Adress, Section, Apartment FROM Main WHERE Contract_num = '" + contract + "'";
            reader = command.ExecuteReader();
            reader.Read();
            dataGridViewInf.Rows.Add("Адрес", reader.GetString(0) + "!" + reader.GetString(1) + "!" + reader.GetInt32(2));
            reader.Close();

            command.CommandText = "SELECT Old_Debt FROM Main WHERE Contract_num = '" + contract + "'";
            reader = command.ExecuteReader();
            reader.Read();
            int old_debt = reader.GetInt32(0);
            reader.Close();

            dataGridViewInf.Rows.Add("Старый долг:", old_debt);

            this.FormClosing += new FormClosingEventHandler(Information_FormClosing);
            dataGridViewInf.CellEndEdit += new DataGridViewCellEventHandler(dataGridViewInf_CellEndEdit);
            richTextBox1.LostFocus += new EventHandler(richTextBox1_LostFocus);
            dataGridViewInf.CellBeginEdit += new DataGridViewCellCancelEventHandler(dataGridViewInf_CellBeginEdit);
        }

        public void dataGridViewInf_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            backup = dataGridViewInf.CurrentCell.Value;
        }

        public void dataGridViewInf_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewInf.CurrentCell.RowIndex == vs.Length + 1 || dataGridViewInf.CurrentCell.RowIndex == vs.Length)
                return;
            flag[0] = true;
            if (dataGridViewInf.CurrentCell.Value == null)
                dataGridViewInf.CurrentCell.Value = "";

            DataGridViewCell cell = dataGridViewInf.CurrentCell;
            switch (dataGridViewInf.CurrentCell.RowIndex)
            {
                case 1:
                    if (!Program.CheckInputDate(cell.Value.ToString()) && dataGridViewInf.CurrentCell.Value.ToString() != "")
                    {
                        MessageBox.Show("Неверный формат даты, восстановлено предыдущее значение");
                        dataGridViewInf.CurrentCell.Value = backup;
                        return;
                    } break;
                case 4:
                    if (!Program.CheckInputDate(cell.Value.ToString()) && dataGridViewInf.CurrentCell.Value.ToString() != "")
                    {
                        MessageBox.Show("Неверный формат даты, восстановлено предыдущее значение");
                        dataGridViewInf.CurrentCell.Value = backup;
                        return;
                    } break;
                case 7:
                    if (!Program.CheckInputDate(cell.Value.ToString()) && dataGridViewInf.CurrentCell.Value.ToString() != "")
                    {
                        MessageBox.Show("Неверный формат даты, восстановлено предыдущее значение");
                        dataGridViewInf.CurrentCell.Value = backup;
                        return;
                    } break;
                case 9:
                    if (!Program.CheckInputDate(cell.Value.ToString()) && dataGridViewInf.CurrentCell.Value.ToString() != "")
                    {
                        MessageBox.Show("Неверный формат даты, восстановлено предыдущее значение");
                        dataGridViewInf.CurrentCell.Value = backup;
                        return;
                    } break;

                case 3:
                    string pattern = @"\d{4}\s\d{6}";
                    if (dataGridViewInf.CurrentCell.Value.ToString() == "")
                        break;
                    if (!Regex.IsMatch(dataGridViewInf.CurrentCell.Value.ToString(), pattern))
                    {
                        MessageBox.Show("Неверный формат паспортных данных, восстановлено предыдущее значение");
                        dataGridViewInf.CurrentCell.Value = backup;
                        return;
                    } break;
            }

            vs[dataGridViewInf.CurrentCell.RowIndex] = dataGridViewInf.CurrentCell.Value.ToString();
        }

        public void richTextBox1_LostFocus(object sender, EventArgs e)
        {
            flag[1] = true;
        }

        public void Information_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flag[0])
            {
                command.CommandText = "UPDATE ContractInf SET Contract_start = '" + vs[1].Replace('.', '-') + "', FIO = '" + vs[2] +
                "', Passport = '" + vs[3] + "', Pt_when = '" + vs[4].Replace('.', '-') + "', Pt_who = '" + vs[5] + "', Phone = '" + vs[6] +
                "', Birth_date = '" + vs[7].Replace('.', '-') + "', Birth_place = '" + vs[8] + "', Contract_end = '" + vs[9].Replace('.', '-') +
                "' WHERE Contract_num = '" + vs[0] + "'";

                command.ExecuteNonQuery();
            }

            if (flag[1])
            {
                command.CommandText = "UPDATE Main SET Notice = '" + richTextBox1.Text + "' WHERE Contract_num = '" + vs[0] + "'";

                command.ExecuteNonQuery();
            }

            int i = dataGridViewInf.Rows.Count - 2;
            string[] str = dataGridViewInf.Rows[i].Cells[1].Value.ToString().Split('!');

            string ctr = dataGridViewInf.Rows[0].Cells[1].Value.ToString();

            command.CommandText = "UPDATE Main SET Adress = '" + str[0] + "', Section = '" + str[1] + "', Apartment = '" + str[2] +
                                            "' WHERE Contract_num = '" + ctr + "'";
            command.ExecuteNonQuery();

            command.CommandText = "UPDATE Main SET Contract_num = '" + ctr +
                            "' WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();

            command.CommandText = "UPDATE ContractInf SET Contract_num = '" + ctr +
                            "' WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();

            command.CommandText = "UPDATE ToExcel SET Contract_num = '" + ctr +
                "' WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();

            command.CommandText = "UPDATE Payments SET Contract_num = '" + ctr +
                "' WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();
            view.Items[0].SubItems[4].Text = ctr;

            i = Convert.ToInt32(dataGridViewInf.Rows[dataGridViewInf.Rows.Count - 1].Cells[1].Value);
            command.CommandText = "UPDATE Main SET Old_Debt = " + i + " WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();

            Act act = new Act(contract, command.Connection);
            Program.form.setButtonAct(true);

            Program.form.setButtonCtrInf(true);
        }
    }
}
