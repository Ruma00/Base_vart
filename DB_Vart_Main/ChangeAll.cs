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
    public partial class ChangeAll : Form
    {
        SqlConnection connection;
        public ChangeAll(SqlConnection connection)
        {
            InitializeComponent();
            this.connection = connection;
            textBoxDate.KeyDown += new KeyEventHandler(tbS_KeyDown);
            textBoxSum.KeyDown += new KeyEventHandler(tbS_KeyDown);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT Contract_num, Monthly_fee FROM Main";
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();
            Dictionary<string, string> contract = new Dictionary<string, string>();

            while (reader.Read())
            {
                contract.Add(reader.GetString(0), reader.GetString(1));
            }
            reader.Close();

            foreach (string s in contract.Keys)
            {
                List<Dat> list = new List<Dat>();
                string[] vs = contract[s].Split(',');
                DateTime dd = Convert.ToDateTime(vs[0].Split('_')[1]);
                foreach (string a in vs)
                {
                    string[] ar = a.Split('_');
                    list.Add(new Dat(Convert.ToInt32(ar[0]), Convert.ToDateTime(ar[1])));
                }
                command.CommandText = "SELECT Contract_end FROM ContractInf WHERE Contract_num = '" + s + "'";
                reader = command.ExecuteReader();
                reader.Read();
                DateTime arr = reader.GetDateTime(0);
                reader.Close();

                if (!Program.CheckInputDate(textBoxDate.Text) || !Program.CheckInputNum(textBoxSum.Text))
                {
                    MessageBox.Show("Неверный формат суммы и/или даты");
                    return;
                }

                Act act = new Act(s, "", "", connection);

                if (arr.Year != 1900 && Convert.ToDateTime(textBoxDate.Text) > arr)
                {
                    /*command.CommandText = "SELECT Monthly_fee FROM Main WHERE Contract_num = '" + s + "'";
                    reader = command.ExecuteReader();*/
                    //DebtCalc(reader, s);
                    act.Calc(connection, s);
                    act.Dispose();
                    continue;
                }

                list.Add(new Dat(Convert.ToInt32(textBoxSum.Text), Convert.ToDateTime(textBoxDate.Text)));
                list.Sort(new ChangeComparer());

                if (list[0].Date < dd)
                    list.RemoveAt(0);

                string com = list[0].Fee + "_" + list[0].Date.ToShortDateString();
                for (int i = 1; i < list.Count; i++)
                {
                    if (i != list.Count - 1 && list[i].Date == list[i + 1].Date)
                    {
                        com += "," + list[i + 1].Fee + "_" + list[i].Date.ToShortDateString();
                        continue;
                    }
                    else if (list[i].Date == list[i - 1].Date)
                        continue;
                    com += "," + list[i].Fee + "_" + list[i].Date.ToShortDateString();
                }

                command.CommandText = "UPDATE Main SET Monthly_fee = '" + com + "' WHERE Contract_num = '" + s + "'";
                command.ExecuteNonQuery();
                /*command.CommandText = "SELECT Monthly_fee FROM Main WHERE Contract_num = '" + s + "'";
                reader = command.ExecuteReader();*/
                //DebtCalc(reader, s);
                
                act.Calc(connection, s);
                act.Dispose();
                Program.form.setButtonAct(true);
                list.Clear();
            }

            this.Close();
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

        private void ChangeAll_Load(object sender, EventArgs e)
        {

        }

        void tbS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonGo_Click(sender, e);
                //buttonAct.Focus();
            }
        }
    }
}
