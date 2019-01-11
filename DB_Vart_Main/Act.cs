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
            Calc(connection, contract);
            Program.form.setButtonAct(false);
            this.FormClosing += new FormClosingEventHandler(Act_FormClosing);
        }

        private void labelAdress_Click(object sender, EventArgs e)
        {

        }

        private void Act_Load(object sender, EventArgs e)
        {

        }

        public void Calc(SqlConnection connection, String contract)
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
            list.Add(new Dat(list[list.Count - 1].Fee, DateTime.Today));

            int k = 0, c = 0;
            int u = 0;
            int f = 0;

            double sumFeeYear = 0, sumPayYear = 0;
            double sumFee = 0, sumPay = 0;

            bool flag = true, pFlag = true;
            bool[] sFlag = { true, true };

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
                    if (s.Length == 1)
                        break;
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

            string[] monthes = DateTimeFormatInfo.CurrentInfo.MonthNames;

            f = 0;
            for (i = startYear.Year; i <= endYear.Year; i++)
            {
                ListViewItem item;

                item = new ListViewItem(new string[] { "-------------------" + i.ToString() + "-------------------" });
                listViewAct.Items.Add(item);

                for (int j = 1; j <= 12; j++)
                {
                    if (startYear.Year == i && j < startYear.Month)
                        continue;

                    if ((j == DateTime.Today.Month) && (i == endYear.Year))
                    {
                        item = new ListViewItem(new string[] { "", monthes[j - 1] });
                        listViewAct.Items.Add(item);
                        for (int o = c; o < dats.Count; o++)
                        {
                            sumPayYear += dats[o].Fee;
                            item = new ListViewItem(new string[] { "", "", "", dats[o].Fee.ToString(), dats[o].Date.ToShortDateString() });
                            listViewAct.Items.Add(item);
                        }
                        break;
                    }
                    //


                    int pay = list[k].Fee;

                    if (sFlag[0] && startYear.Day > 15)
                    {
                        f = pay / 2;
                        sumFeeYear -= f;
                    }

                    if (pFlag && i == list[k + 1].Date.Year && j == list[k + 1].Date.Month)
                    {
                        k++;
                        pay = list[k].Fee;
                        if (k == list.Count)
                            pFlag = false;
                    }

                    if (flag && i == dats[c].Date.Year && j == dats[c].Date.Month)
                    {
                        item = new ListViewItem(new string[] { "", monthes[j - 1], pay.ToString(),
                                            dats[c].Fee.ToString(), dats[c].Date.ToShortDateString() });
                        sumPayYear += dats[c].Fee;
                        listViewAct.Items.Add(item);
                        c++;

                        if (c == dats.Count)
                            flag = false;

                        while (flag && i == dats[c].Date.Year && j == dats[c].Date.Month)
                        {
                            item = new ListViewItem(new string[] { "", "", "", dats[c].Fee.ToString(), dats[c].Date.ToShortDateString() });
                            listViewAct.Items.Add(item);
                            sumPayYear += dats[c].Fee;
                            c++;
                            if (c == dats.Count)
                                flag = false;
                        }

                        if (sFlag[0])
                        {
                            sFlag[0] = false;
                            item = new ListViewItem(new string[] { "", "", "", f.ToString(), "-----" });
                            listViewAct.Items.Add(item);
                        }

                        if (c == dats.Count)
                            flag = false;
                    }
                    else if (sFlag[0])
                    {
                        sFlag[0] = false;
                        item = new ListViewItem(new string[] { "", monthes[j - 1], pay.ToString(), f.ToString(), "" });
                        listViewAct.Items.Add(item);
                    }
                    else
                    { 
                        item = new ListViewItem(new string[] { "", monthes[j - 1], pay.ToString(), "", "" });
                        listViewAct.Items.Add(item);
                    }

                    sumFeeYear += pay;

                    if (j == endYear.Month && i == endYear.Year)
                    {
                        f = u / 2;
                        item = new ListViewItem(new string[] { "", monthes[j - 1], pay.ToString(), f.ToString(), "-----" });
                        break;
                    }
                }

                item = new ListViewItem(new string[] { "", "Обороты за период:", sumFeeYear.ToString(), sumPayYear.ToString(), "" });
                item.BackColor = Color.Gainsboro;
                listViewAct.Items.Add(item);
                sumFee += sumFeeYear; sumPay += sumPayYear;
                sumFeeYear = 0; sumPayYear = 0;
                if (i != endYear.Year)
                {
                    item = new ListViewItem(new string[] { "Долг на 01.01." + (i + 1).ToString(), (sumFee - sumPay).ToString() });
                    listViewAct.Items.Add(item);
                }
                else
                {
                    item = new ListViewItem(new string[] { "Долг на " + DateTime.Today.ToShortDateString(),
                                                                (sumFee - sumPay).ToString() });
                    item.BackColor = Color.PeachPuff;
                    listViewAct.Items.Add(item);
                }
            }
            command.CommandText = "UPDATE Main SET Debt = " + (sumFee - sumPay).ToString() + " WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();
            command.CommandText = "UPDATE ToExcel SET Debt = " + (sumFee - sumPay).ToString() + " WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();
            Program.form.setButtonAct(true);
        }

        private void Act_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.form.setButtonAct(true);
        }
    }
}
