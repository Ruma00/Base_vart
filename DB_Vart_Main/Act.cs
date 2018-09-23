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
        public Act(string contract, string name, string adress, SqlConnection connection)
        {
            InitializeComponent();
            labelTitle.Text = labelTitle.Text + " " + name + " л/с: " + contract;
            labelAdress.Text = labelAdress.Text + adress;
            See(connection, contract);
            //Main_form.buttonAct
            Program.form.setButtonAct(false);
            this.FormClosing += new FormClosingEventHandler(Act_FormClosing);
        }

        string[] a = { };

        private DateTime[] SqlReadDate(SqlDataReader reader) //TODO
        {
            String[] arr = { };
            DateTime[] dateTime = { };
            int j;
            if (reader.HasRows)
            {
                try
                {
                    reader.Read();
                    arr = reader.GetValue(0).ToString().Split(',');
                    if (arr.Length == 1 && arr[0] == "")
                    {
                        reader.Close();
                        return dateTime;
                    }
                    a = new String[arr.Length * 2];
                    j = 0;
                    for(int i = 0; i < arr.Length; i++, j += 2)
                    {
                        String[] temp;
                        temp = arr[i].Split('_');
                        a[j] = temp[0];
                        a[j + 1] = temp[1];
                    }

                    dateTime = new DateTime[a.Length / 2];
                    j = 0;
                    for (int i = 0; i < dateTime.Length; i++)
                    {
                        dateTime[i] = Convert.ToDateTime(a[j]);
                        j += 2;
                    }
                }
                catch
                {
                    //
                }
            }

            /*DateTime[] dateTime = new DateTime[a.Length / 2];
            j = 0;
            for (int i = 0; i < dateTime.Length; i++)
            {
                dateTime[i] = Convert.ToDateTime(a[j]);
                j += 2;
            }*/

            reader.Close();

            return dateTime;
        }

        void See(SqlConnection connection, string contract) //TODO
        {
            SqlCommand command = new SqlCommand("SELECT List FROM Payments WHERE Contract_num = " + contract);
            command.Connection = connection;
            DateTime[] dates = SqlReadDate(command.ExecuteReader());

            //------------------------------------
            command.CommandText = "SELECT Monthly_fee FROM Main WHERE Contract_num = " + contract;

            string pay = "";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                pay = reader.GetValue(0).ToString();
            }
            reader.Close();

            string[] fe = pay.Split(',');
            pay = fe[0];
            int o = 0;
            string[] fee = new string[fe.Length * 2];
            for (int i = 0; i < fe.Length * 2; i += 2)
            {
                string[] ar = fe[o].Split('_');
                fee[i] = ar[0];
                fee[i + 1] = ar[1];
                o++;
            }
            //------------------------------------
            //
            
            command.CommandText = "SELECT Contract_start, Contract_end FROM ContractInf WHERE Contract_num = " + contract;
            DateTime startYear = DateTime.Today, endYear = DateTime.Today;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                startYear = reader.GetDateTime(0);
                endYear = reader.GetDateTime(1);
            }

            string[] monthes = DateTimeFormatInfo.CurrentInfo.MonthNames;
            //string pay = fee.ToString();

            int u = 0;
            DateTime dat = new DateTime();
            if (endYear.Year == 1)
                endYear = DateTime.Today;
            if (startYear.Year < 2015)
                startYear = new DateTime(2015, 1, 1);

            int sumFee = 0, sumPay = 0;
            int sumFeeYear = 0, sumPayYear = 0;
            int f = 0;
            //while (i < dat)
            for (int i = startYear.Year; i <= endYear.Year; i++)
            {
                ListViewItem item;
                /*if (startYear.Year > 2015)
                    item = new ListViewItem(new string[] { "-------------------" + (startYear.Year).ToString() + "--------------------" });
                else
                    item = new ListViewItem(new string[] { "-------------------" + (2015 + i).ToString() + "--------------------" });*/

                item = new ListViewItem(new string[] { "-------------------" + i.ToString() + "-------------------" });
                listViewAct.Items.Add(item);

                for (int j = 1; j <= 12; j++)
                {
                    if ((j == DateTime.Today.Month) && (i == endYear.Year))
                        break;
                    if (startYear.Year == i && j < startYear.Month)
                        continue;
                    if (fee.Length > 2 && f + 3 <= fee.Length)
                    {
                        if (Convert.ToDateTime(fee[f + 3]).Year == i && Convert.ToDateTime(fee[f + 3]).Month == j)
                        {
                            pay = fee[f + 2];
                            f += 2;
                        }
                    }

                    int paym = 0;
                    if (a.Length > 1)
                        paym = Convert.ToInt32(a[u + 1]);
                    //if (u < dates.Length && dates[u].Year - 2015 == i && dates[u].Month == j + 1)
                    while (u + 2 < a.Length && Convert.ToDateTime(a[u]).Month == j && Convert.ToDateTime(a[u]).Year == i && 
                        j == Convert.ToDateTime(a[u + 2]).Month && Convert.ToDateTime(a[u + 2]).Year == i)
                    {
                        paym += Convert.ToInt32(a[u + 3]);
                        u += 2;
                    }
                    //u += 2;
                    
                    if (u < a.Length && Convert.ToDateTime(a[u]).Year == i && Convert.ToDateTime(a[u]).Month == j)
                    {
                        if (paym == 0)
                            item = new ListViewItem(new string[] { "", monthes[j - 1], fee[f], a[u + 1], Convert.ToDateTime(a[u]).ToString("d") });
                        else
                            item = new ListViewItem(new string[] { "", monthes[j - 1], fee[f], paym.ToString(), Convert.ToDateTime(a[u]).ToString("d") });
                        if (paym == 0)
                            sumPayYear += Convert.ToInt32(a[u + 1]);
                        else
                            sumPayYear += paym;
                        //u++;
                        u += 2;
                    }
                    else
                        item = new ListViewItem(new string[] { "", monthes[j - 1], fee[f], "", "" });
                    sumFeeYear += Convert.ToInt32(fee[f]);
                    listViewAct.Items.Add(item);
                }

                item = new ListViewItem(new string[] { "", "Обороты за период:", sumFeeYear.ToString(), sumPayYear.ToString(), "" });
                listViewAct.Items.Add(item);
                sumFee += sumFeeYear; sumPay += sumPayYear;
                sumFeeYear = 0; sumPayYear = 0;
                if (i != endYear.Year)
                {
                    item = new ListViewItem(new string[] { "Долг на 01.01." + i.ToString(), (sumFee - sumPay).ToString() });
                    listViewAct.Items.Add(item);
                }
            }

            ListViewItem item1 = new ListViewItem(new string[] { "Итого долг на " + DateTime.Today.ToString("d"),
                                                                                        (sumFee - sumPay).ToString() });
            listViewAct.Items.Add(item1);
            reader.Close();
        }

        private void labelAdress_Click(object sender, EventArgs e)
        {

        }

        private void Act_Load(object sender, EventArgs e)
        {

        }

        private void Act_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.form.setButtonAct(true);
        }
    }
}
