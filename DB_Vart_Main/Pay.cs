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

            dataGridViewInf.RowTemplate.Height = 43;
            dataGridViewInf.AllowUserToAddRows = true;

            this.contract = contract;
            this.connection = connection;

            this.FormClosing += new FormClosingEventHandler(Pay_FormClosing);

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
                    else
                        inf = new PayInf(Convert.ToDateTime(h[0]), Convert.ToDouble(h[1]), h[2]);
                    list.Add(inf);
                    //count++;
                }
                YearComparer yc = new YearComparer();
                list.Sort(yc);
                for (int i = 0; i < list.Count; i++)
                    dataGridViewInf.Rows.Add(list[i].Date, list[i].Pay, list[i].Notice);
            }
            reader.Close();
        }



        public void Pay_FormClosing(object sender, FormClosingEventArgs e)
        {
            YearComparer yc = new YearComparer();
            list.Sort(yc);
            String str = "";
            for (int i = 0; i < list.Count; i++)
            {
                str += list[i].Date.ToShortDateString() + "_" + list[i].Pay.ToString();
                if (list[i].Notice != "")
                    str += "_" + list[i].Notice;
            }
        }
    }
}
