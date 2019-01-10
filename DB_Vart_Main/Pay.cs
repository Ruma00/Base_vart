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
    public partial class Pay : Form
    {
        string contract;
        SqlConnection connection;
        SqlCommand command;
        List<PayInf> list = new List<PayInf>();

        public Pay(string contract, SqlConnection connection)
        {
            InitializeComponent();

            dataGridViewInf.RowTemplate.MinimumHeight = 43;
            dataGridViewInf.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewInf.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewInf.AllowUserToAddRows = true;

            this.contract = contract;
            this.connection = connection;

            this.FormClosing += new FormClosingEventHandler(Pay_FormClosing);
            dataGridViewInf.CellEndEdit += new DataGridViewCellEventHandler(dataGridViewInf_CellEndEdit);

            command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT List FROM Payments WHERE Contract_num = '" + contract + "'";
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
                    PayInf inf;
                    String[] h = str[i].Split('_');
                    if (h.Length == 2)
                        inf = new PayInf(Convert.ToDateTime(h[0]), Convert.ToDouble(h[1]));
                    else if (h.Length > 2)
                        inf = new PayInf(Convert.ToDateTime(h[0]), Convert.ToDouble(h[1]), h[2]);
                    else
                        break;
                    list.Add(inf);
                    //count++;
                }
                YearComparer yc = new YearComparer();
                list.Sort(yc);
                for (int i = 0; i < list.Count; i++)
                    dataGridViewInf.Rows.Add(list[i].Date.ToShortDateString(), list[i].Pay, list[i].Notice);
            }
            reader.Close();
        }

        public void dataGridViewInf_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int u = dataGridViewInf.CurrentCell.ColumnIndex;
            int i = dataGridViewInf.CurrentCell.RowIndex;
            DataGridViewCell cell = dataGridViewInf.CurrentCell;
            if (i == list.Count)
                list.Add(new PayInf());
            switch(u)
            {
                case 0:
                    if (cell.Value != null && cell.Value.ToString() != "")
                        list[i].Date = Convert.ToDateTime(cell.Value);
                    else
                    {
                        list.RemoveAt(i);
                        dataGridViewInf.Rows.RemoveAt(i);
                    }
                    break;
                case 1:
                    if (cell.Value != null && cell.Value.ToString() == "")
                        list[i].Pay = 0;
                    else
                        list[i].Pay = Convert.ToDouble(cell.Value); break;
                case 2:
                    if (cell.Value == null)
                        list[i].Notice = "";
                    else
                        list[i].Notice = cell.Value.ToString();
                    break;
            }
        }

        public void Pay_FormClosing(object sender, FormClosingEventArgs e)
        {
            YearComparer comparer = new YearComparer();
            list.Sort(comparer);

            String str = "'";
            if (list.Count > 0)
            {
                str += list[0].Date.ToShortDateString() + "_" + list[0].Pay.ToString();
                if (list[0].Notice != "")
                    str += "_" + list[0].Notice;
            }
            for (int i = 1; i < list.Count; i++)
            {
                str += "," + list[i].Date.ToShortDateString() + "_" + list[i].Pay.ToString();
                if (list[i].Notice != "")
                    str += "_" + list[i].Notice;
            }
            str += "'";
            command.CommandText = "UPDATE Payments SET List = " + str + " WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();

            command.CommandText = "SELECT List FROM Payments WHERE Contract_num = '" + contract + "'";
            SqlDataReader reader = command.ExecuteReader();
            Program.form.SqlReadDate(reader);
        }
    }
}
