using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Vart_Main
{
    public partial class Act : Form
    {
        public Act(string contract, string name, SqlConnection connection)
        {
            InitializeComponent();
            labelTitle.Text = labelTitle.Text + name + "л/с: " + contract;
            See(connection, contract);
        }

        private DateTime[] SqlReadDate(SqlDataReader reader) //TODO
        {
            string[] a = { };
            if (reader.HasRows)
            {
                string[] arr = reader.GetValue(0).ToString().Split(',');
                a = arr[0].Split('_');
            }

            DateTime[] dateTime = new DateTime[a.Length];
            for (int i = 0; i < a.Length; i++)
                dateTime[i] = Convert.ToDateTime(a[i]);

            reader.Close();

            return dateTime;
        }

        void See(SqlConnection connection, string contract) //TODO
        {
            SqlCommand command = new SqlCommand("SELECT List FROM Payments WHERE Contract_num = " + contract);
            command.Connection = connection;
            DateTime[] dates = SqlReadDate(command.ExecuteReader());
            string[] monthes = DateTimeFormatInfo.CurrentInfo.MonthNames;
            string pay = "";

            for (int i = 0; i < DateTime.Today.Year - 2015; i++)
            {
                ListViewItem item = new ListViewItem(new string[] { "2016:" });
                listViewAct.Items.Add(item);
                for (int j = 0; j < 12; j++)
                {
                    item = new ListViewItem(new string[] { monthes[i], "", "", "", "" });
                }
            }
        }
    }
}
