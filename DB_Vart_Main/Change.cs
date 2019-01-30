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

namespace DB_Vart_Main
{
    public partial class Change : Form
    {
        bool cansel = false;
        SqlCommand command;
        SqlConnection connection;
        String contract;
        ListView list;
        List<Dat> sorted = new List<Dat>();
        int count = 0;
        object backup;

        public Change(string contract, SqlConnection connection, ref ListView list)
        {
            InitializeComponent();

            dataGridViewInf.RowTemplate.Height = 43;
            dataGridViewInf.AllowUserToAddRows = true;

            this.contract = contract;
            this.list = list;
            this.connection = connection;

            this.FormClosing += new FormClosingEventHandler(Change_FormClosing);
            dataGridViewInf.CellEndEdit += new DataGridViewCellEventHandler(dataGrid_CellEndEdit);
            dataGridViewInf.CellBeginEdit += new DataGridViewCellCancelEventHandler(dataGrid_CellBeginEdit);

            command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT Monthly_fee FROM Main WHERE Contract_num = " + contract;
            SqlDataReader reader = command.ExecuteReader();

            See(reader);
        }

        public void See(SqlDataReader reader)
        {
            while (reader.Read())
            {
                String s = reader.GetString(0);
                String[] str = s.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    String[] h = str[i].Split('_');
                    //
                    Dat r = new Dat(Convert.ToInt32(h[0]), Convert.ToDateTime(h[1]));
                    sorted.Add(r);
                    dataGridViewInf.Rows.Add(h[0], h[1]);
                    //count++;
                }
            }
            reader.Close();
        }

        private double DebtCalc(SqlDataReader reader, string contract)
        {
            reader.Read();
            String str = reader.GetString(0);
            reader.Close();

            List<Dat> list = new List<Dat>();

            Dat dat;
            String[] vb = str.Split(',');
            for (int i = 0; i < vb.Length; i++)
            {
                dat = new Dat();
                String[] h = vb[i].Split('_');
                dat.Fee = Convert.ToInt32(h[0]);
                dat.Date = Convert.ToDateTime(h[1]);
                list.Add(dat);
            }

            DateTime date, end;
            int u = 0;
            double debt = 0;
            for (int i = 0; i <= list.Count - 2; i++)
            {
                u = list[i].Fee;
                date = Convert.ToDateTime(list[i].Date);

                if (date.Day > 15)
                {
                    debt += u / 2;
                    date = date.AddMonths(1);
                }

                end = Convert.ToDateTime(list[i + 1].Date);

                while (date < end)
                {
                    debt += u;
                    date = date.AddMonths(1);
                }
            }

            SqlCommand command = new SqlCommand("SELECT Contract_end FROM ContractInf WHERE Contract_num = '" + contract + "'", connection);
            reader = command.ExecuteReader();
            reader.Read();

            end = reader.GetDateTime(0);
            if (end.Year == 1900)
                end = DateTime.Today;
            reader.Close();

            date = list[list.Count - 1].Date;
            u = list[list.Count - 1].Fee;
            while (date < end)
            {
                debt += u;
                date = date.AddMonths(1);
            }

            debt -= u;

            command = new SqlCommand("SELECT List FROM Payments WHERE Contract_num = " + contract, connection);
            reader = command.ExecuteReader();
            reader.Read();
            str = reader.GetString(0);
            reader.Close();

            if (str == null || str == "")
            {
                command.CommandText = "UPDATE Main SET Debt = " + debt + "WHERE Contract_num = '" + contract + "'";
                command.ExecuteNonQuery();
                return debt;
            }

            String[] vs = str.Split(',');

            for (int i = 0; i < vs.Length; i++)
            {
                double d = Convert.ToDouble(vs[i].Split('_')[1]);
                debt -= d;
            }

            if (end != DateTime.Today && end.Day < 16)
                debt -= u / 2;

            command.CommandText = "UPDATE Main SET Debt = " + debt + "WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();
            command.CommandText = "UPDATE ToExcel SET Debt = " + debt + "WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();

            return debt;
        }

        public void dataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int u = dataGridViewInf.CurrentCell.ColumnIndex;
            int i = dataGridViewInf.CurrentCell.RowIndex;
            DataGridViewCell cell = dataGridViewInf.CurrentCell;

            if (i == 0)
                dataGridViewInf.CancelEdit();
            if (i == sorted.Count)
                sorted.Add(new Dat());

            switch (u)
            {
                case 0:
                    if (cell.Value != null && cell.Value.ToString() != "" && !Program.CheckInputNum(cell.Value.ToString()))
                    {
                        MessageBox.Show("Неверный формат суммы, восстановлено предыдущее значение");
                        dataGridViewInf.CurrentCell.Value = backup;
                        return;
                    }
                    if (cell.Value != null && cell.Value.ToString() != "")
                        sorted[i].Fee = Convert.ToInt32(cell.Value);
                    else
                        sorted[i].Fee = 0; dataGridViewInf.CurrentCell.Value = "0"; break;
                case 1:
                    if (cell.Value != null && cell.Value.ToString() != "" && !Program.CheckInputDate(cell.Value.ToString()))
                    {
                        MessageBox.Show("Неверный формат даты, восстановлено предыдущее значение");
                        dataGridViewInf.CurrentCell.Value = backup;
                        return;
                    }
                    if (cell.Value != null && cell.Value.ToString() != "")
                        sorted[i].Date = Convert.ToDateTime(cell.Value);
                    else
                    {
                        sorted.RemoveAt(i);
                        dataGridViewInf.Rows.RemoveAt(i);
                    }
                    break;
            }
        }

        public void dataGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            backup = dataGridViewInf.CurrentCell.Value;
        }

        public void Change_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChangeComparer comparer = new ChangeComparer();
            sorted.Sort(comparer);
            String str = "'";
            if (sorted.Count > 0)
            {
                str += sorted[0].Fee.ToString() + "_" + sorted[0].Date.ToShortDateString();
            }

            for (int i = 1; i < sorted.Count; i++)
            {
                if (sorted[i].Date == sorted[i - 1].Date)
                    continue;
                str += "," + sorted[i].Fee.ToString() + "_" + sorted[i].Date.ToShortDateString();
            }
            str += "'";
            /*String str = dataGridViewInf.Rows[0].Cells[0].Value.ToString() + "_" + dataGridViewInf.Rows[0].Cells[1].Value.ToString();
            for (int i = 1; i < dataGridViewInf.Rows.Count; i++)
            {
                if (dataGridViewInf.Rows[i].Cells[0].Value == null || dataGridViewInf.Rows[i].Cells[0].Value.ToString() == "")
                    continue;
                str += "," + dataGridViewInf.Rows[i].Cells[0].Value.ToString() + "_" + dataGridViewInf.Rows[i].Cells[1].Value.ToString();
                count = Convert.ToInt32(dataGridViewInf.Rows[i].Cells[0].Value);
            }*/
            command.CommandText = "UPDATE Main SET Monthly_fee = " + str + " WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();

            /*command.CommandText = "SELECT Monthly_fee FROM Main WHERE Contract_num = '" + contract + "'";
            SqlDataReader reader = command.ExecuteReader();*/
            /*double dd = DebtCalc(reader, contract);
            list.Items[0].SubItems[5].Text = dd.ToString();
            list.Items[0].SubItems[6].Text = sorted[sorted.Count - 1].Fee.ToString();*/
            Act act = new Act(contract, "", "", connection);
            //act.Calc(connection, contract);
            act.Dispose();
            Program.form.setButtonAct(true);
            Program.form.buttonChgAP.Enabled = true;
        }
    }
}
