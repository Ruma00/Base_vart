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
            //See(connection, contract);
            Calc(connection, contract);
            Program.form.setButtonAct(false);
            this.FormClosing += new FormClosingEventHandler(Act_FormClosing);
        }

        /*string[] a = { };

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

            reader.Close();

            return dateTime;
        }

        /*void See(SqlConnection connection, string contract) //TODO
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
            //MessageBox.Show(pay);
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

            int sumFee = 0, sumPay = 0;

            int u = 0, debt = 0;
            DateTime dat = new DateTime();
            if (endYear.Year == 1900)
                endYear = DateTime.Today;
            if (startYear.Year < 2015)
            {
                dat = startYear;
                startYear = new DateTime(2015, 1, 1);

                *while (dat < startYear)
                {
                    u = Convert.ToInt32(fee[j]);
                    DateTime date = Convert.ToDateTime(fee[j + 1]);
                    DateTime end = Convert.ToDateTime(fee[j + 3]);
                    MessageBox.Show(end.ToShortDateString());
                    while (date < end && date < startYear)
                    {
                        debt += u;
                        date = date.AddMonths(1);
                        //MessageBox.Show(date.ToShortDateString());
                    }
                }*
            }

            
            int sumFeeYear = 0, sumPayYear = 0;
            int f = 0;

            //
            

            for (int i = startYear.Year; i <= endYear.Year; i++)
            {
                ListViewItem item;
                *if (startYear.Year > 2015)
                    item = new ListViewItem(new string[] { "-------------------" + (startYear.Year).ToString() + "--------------------" });
                else
                    item = new ListViewItem(new string[] { "-------------------" + (2015 + i).ToString() + "--------------------" });*

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
                    item = new ListViewItem(new string[] { "Долг на 31.12." + i.ToString(), (sumFee - sumPay).ToString() });
                    listViewAct.Items.Add(item);
                }
            }

            ListViewItem item1 = new ListViewItem(new string[] { "Итого долг на " + DateTime.Today.ToString("d"),
                                                                                        (sumFee - sumPay).ToString() });
            listViewAct.Items.Add(item1);
            reader.Close();
        }*/

        private void labelAdress_Click(object sender, EventArgs e)
        {

        }

        private void Act_Load(object sender, EventArgs e)
        {

        }

        private void Calc(SqlConnection connection, String contract)
        {
            SqlCommand command = new SqlCommand("SELECT Monthly_fee FROM Main WHERE Contract_num = '" + contract + "'", connection);
            SqlDataReader reader = command.ExecuteReader();

            double debt = 0;
            int i = 0;
            List<Dat> list = new List<Dat>();

            reader.Read();
            String str = reader.GetString(0);
            reader.Close();

            Dat dat;
            String[] vb = str.Split(',');
            for (i = 0; i < vb.Length; i++)
            {
                dat = new Dat();
                String[] h = vb[i].Split('_');
                dat.Fee = Convert.ToInt32(h[0]);
                dat.Date = Convert.ToDateTime(h[1]);
                list.Add(dat);
            }

            //debt before 01.01.2015 & read pays
            int k = 0, c = 0;
            int u = 0;

            double sumFeeYear = 0, sumPayYear = 0;
            double sumFee = 0, sumPay = 0;

            bool flag = true, pFlag = true;

            DateTime date, end;

            command.CommandText = "SELECT Contract_start, Contract_end FROM ContractInf WHERE Contract_num = " + contract;
            DateTime startYear, endYear;
            reader = command.ExecuteReader();
            reader.Read();

            startYear = reader.GetDateTime(0);
            endYear = reader.GetDateTime(1);
            reader.Close();
            
            //++↓↓↓++
            command = new SqlCommand("SELECT List FROM Payments WHERE Contract_num = " + contract, connection);
            reader = command.ExecuteReader();
            reader.Read();
            str = reader.GetString(0);
            reader.Close();

            String[] vs = str.Split(',');
            List<Dat> dats = new List<Dat>();

            if (vs.Length > 0)
            {
                for (i = 0; i < vs.Length; i++)
                {
                    String[] s = vs[i].Split('_');
                    //MessageBox.Show(d.ToString());
                    Dat d = new Dat();
                    d.Fee = Convert.ToInt32(s[1]);
                    d.Date = Convert.ToDateTime(s[0]);
                    dats.Add(d);
                }
            }

            if (dats.Count == 0)
                flag = false;

            if (endYear.Year == 1900)
                endYear = DateTime.Today;
            if (startYear.Year < 2015)
            {
                DateTime d = startYear;
                startYear = new DateTime(2015, 1, 1);

                for (i = 0; (i <= list.Count - 2) && (d < startYear); i++)
                {
                    u = list[i].Fee;
                    date = Convert.ToDateTime(list[i].Date);
                    end = Convert.ToDateTime(list[i + 1].Date);
                    MessageBox.Show(end.ToShortDateString());
                    while (date < end && d < startYear)
                    {
                        debt += u;
                        date = date.AddMonths(1);
                        d = d.AddMonths(1);
                        MessageBox.Show(date.ToShortDateString() + " " + debt.ToString());
                    }
                    //debt -= u;
                }

                for (c = 0; c < dats.Count; c++)
                {
                    if (dats[c].Date.Year < 2015)
                        debt -= dats[c].Fee;
                    else
                        break;
                }
                //MessageBox.Show(debt.ToString());

                sumFee += debt;

                ListViewItem item = new ListViewItem(new string[] { "Долг на 01.01.2015" + i.ToString(), debt.ToString() });
                listViewAct.Items.Add(item);
            }

            //calculate and write to listView

            string[] monthes = DateTimeFormatInfo.CurrentInfo.MonthNames;

            int f = 0;

            k = i;
            if (list[k].Date > startYear)
                k--;

            list.Add(new Dat(list[list.Count - 1].Fee, DateTime.Today));

            for (i = startYear.Year; i <= endYear.Year; i++)
            {
                ListViewItem item;

                item = new ListViewItem(new string[] { "-------------------" + i.ToString() + "-------------------" });
                listViewAct.Items.Add(item);

                for (int j = 1; j <= 12; j++)
                {
                    if ((j == DateTime.Today.Month) && (i == endYear.Year))
                        break;
                    if (startYear.Year == i && j < startYear.Month)
                        continue;

                    int pay = list[k].Fee;

                    if (pFlag && i == list[k + 1].Date.Year && j == list[k + 1].Date.Month)
                    {
                        k++;
                        pay = list[k].Fee;
                        if (k == list.Count)
                            pFlag = false;
                    }
                    //item = new ListViewItem(new string[] { "", monthes[j - 1], fee[f], a[u + 1], Convert.ToDateTime(a[u]).ToString("d") });

                    //
                    if (flag && i == dats[c].Date.Year && j == dats[c].Date.Month)
                    {
                        item = new ListViewItem(new string[] { "", monthes[j - 1], pay.ToString(),
                                            dats[c].Fee.ToString(), dats[c].Date.ToShortDateString() });
                        sumPayYear += dats[c].Fee;
                        c++;
                        if (c == dats.Count)
                            flag = false;
                    }
                    else
                        item = new ListViewItem(new string[] { "", monthes[j - 1], pay.ToString(), "", "" });

                    listViewAct.Items.Add(item);

                    sumFeeYear += pay;
                }

                item = new ListViewItem(new string[] { "", "Обороты за период:", sumFeeYear.ToString(), sumPayYear.ToString(), "" });
                item.BackColor = Color.Gainsboro;
                listViewAct.Items.Add(item);
                sumFee += sumFeeYear; sumPay += sumPayYear;
                sumFeeYear = 0; sumPayYear = 0;
                if (i != endYear.Year)
                {
                    item = new ListViewItem(new string[] { "Долг на 31.12." + i.ToString(), (sumFee - sumPay).ToString() });
                    listViewAct.Items.Add(item);
                }
            }
        }

        private void Act_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.form.setButtonAct(true);
        }
    }
}
