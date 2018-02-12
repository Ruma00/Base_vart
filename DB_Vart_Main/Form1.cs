﻿using System;
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
    public partial class Main_form : Form
    {
        SqlConnection sqlConnection = new SqlConnection();
        public Main_form()
        {
            InitializeComponent();
            sqlConnection.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Base;Integrated Security=true";
            sqlConnection.Open();

            comboBoxRc.SelectedIndex = 0;

            textBoxSD.Text = "Введите № договора"; textBoxSD.ForeColor = Color.Gray;
            textBoxSA.Text = "Введите адрес и кв"; textBoxSA.ForeColor = Color.Gray;
            textBoxPayCH.Text = "Смена аб. платы"; textBoxPayCH.ForeColor = Color.Gray;
            textBoxAddP.Text = "Введите адрес или № договора"; textBoxAddP.ForeColor = Color.Gray;
            textBoxDel.Text = "Введите адрес или № договора"; textBoxDel.ForeColor = Color.Gray;
            textBoxAdr.Text = "Введите адрес"; textBoxAdr.ForeColor = Color.Gray;
            textBoxFam.Text = "Введите фамилию"; textBoxFam.ForeColor = Color.Gray;
            textBoxCtr.Text = "Введите № договора"; textBoxCtr.ForeColor = Color.Gray;
            textBoxPhn.Text = "Введите телефон"; textBoxPhn.ForeColor = Color.Gray;
            textBoxPt.Text = "Введите данные паспорта"; textBoxPt.ForeColor = Color.Gray;
            textBoxDate.Text = "Введите дату"; textBoxDate.ForeColor = Color.Gray;
            textBoxPay.Text = "Введите аб. плату"; textBoxPay.ForeColor = Color.Gray;
            textBoxAdr2.Text = "Введите кв, подъезд"; textBoxAdr2.ForeColor = Color.Gray;

            textBoxAdr.Enter += new EventHandler(textBoxAdr_Enter);
            textBoxAdr.Leave += new EventHandler(textBoxAdr_Leave);
            textBoxFam.Enter += new EventHandler(textBoxFam_Enter);
            textBoxFam.Leave += new EventHandler(textBoxFam_Leave);
            textBoxCtr.Enter += new EventHandler(textBoxCtr_Enter);
            textBoxCtr.Leave += new EventHandler(textBoxCtr_Leave);
            textBoxPhn.Enter += new EventHandler(textBoxPhn_Enter);
            textBoxPhn.Leave += new EventHandler(textBoxPhn_Leave);
            textBoxPt.Enter += new EventHandler(textBoxPt_Enter);
            textBoxPt.Leave += new EventHandler(textBoxPt_Leave);
            textBoxDate.Enter += new EventHandler(textBoxDate_Enter);
            textBoxDate.Leave += new EventHandler(textBoxDate_Leave);
            textBoxPay.Enter += new EventHandler(textBoxPay_Enter);
            textBoxPay.Leave += new EventHandler(textBoxPay_Leave);
            textBoxAdr2.Enter += new EventHandler(textBoxAdr2_Enter);
            textBoxAdr2.Leave += new EventHandler(textBoxAdr2_Leave);

            textBoxSD.Enter += new EventHandler(textBoxSD_Enter);
            textBoxSD.Leave += new EventHandler(textBoxSD_Leave);
            textBoxSA.Enter += new EventHandler(textBoxSA_Enter);
            textBoxSA.Leave += new EventHandler(textBoxSA_Leave);
            textBoxPayCH.Enter += new EventHandler(textBoxPayCH_Enter);
            textBoxPayCH.Leave += new EventHandler(textBoxPayCH_Leave);
            textBoxAddP.Enter += new EventHandler(textBoxAddP_Enter);
            textBoxAddP.Leave += new EventHandler(textBoxAddP_Leave);
            textBoxDel.Enter += new EventHandler(textBoxDel_Enter);
            textBoxDel.Leave += new EventHandler(textBoxDel_Leave);

            //columnHeader1.TextAlign = HorizontalAlignment.Center;
        }

        string[] sqlExpressions = new string[] { "SELECT Adress, Section, Apartment, Surname, Contract_num, Debt, Monthly_fee, Notice FROM ", "Main ", "Debtors ",
            "UPDATE Main SET ", "INSERT INTO Main VALUES ", "WHERE Contract_num=" };

        private void button1_Click(object sender, EventArgs e)
        {

        }

        //--------------------------textBoxes-------------------------------------------------
        private void textBoxSD_Enter(object sender, EventArgs e)
        {
            if (textBoxSD.Text == "Введите № договора")
            {
                textBoxSD.Clear();
                textBoxSD.ForeColor = Color.Black;
            }
        }

        private void textBoxSD_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSD.Text))
            {
                textBoxSD.Text = "Введите № договора";
                textBoxSD.ForeColor = Color.Gray;
            }
        }

        private void textBoxSA_Enter(object sender, EventArgs e)
        {
            if (textBoxSA.Text == "Введите адрес и кв")
            {
                textBoxSA.Clear();
                textBoxSA.ForeColor = Color.Black;
            }
        }

        private void textBoxSA_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSA.Text))
            {
                textBoxSA.Text = "Введите адрес и кв";
                textBoxSA.ForeColor = Color.Gray;
            }
        }

        private void textBoxPayCH_Enter(object sender, EventArgs e)
        {
            if (textBoxPayCH.Text == "Смена аб. платы")
            {
                textBoxPayCH.Clear();
                textBoxPayCH.ForeColor = Color.Black;
            }
        }

        private void textBoxPayCH_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxPayCH.Text))
            {
                textBoxPayCH.Text = "Смена аб. платы";
                textBoxPayCH.ForeColor = Color.Gray;
            }
        }

        private void textBoxAddP_Enter(object sender, EventArgs e)
        {
            if (textBoxAddP.Text == "Введите адрес или № договора")
            {
                textBoxAddP.Clear();
                textBoxAddP.ForeColor = Color.Black;
            }
        }

        private void textBoxAddP_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxAddP.Text))
            {
                textBoxAddP.Text = "Введите адрес или № договора";
                textBoxAddP.ForeColor = Color.Gray;
            }
        }

        private void textBoxDel_Enter(object sender, EventArgs e)
        {
            if (textBoxDel.Text == "Введите адрес или № договора")
            {
                textBoxDel.Clear();
                textBoxDel.ForeColor = Color.Black;
            }
        }

        private void textBoxDel_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxDel.Text))
            {
                textBoxDel.Text = "Введите адрес или № договора";
                textBoxDel.ForeColor = Color.Gray;
            }
        }

        private void textBoxAdr_Enter(object sender, EventArgs e)
        {
            if (textBoxAdr.Text == "Введите адрес")
            {
                textBoxAdr.Clear();
                textBoxAdr.ForeColor = Color.Black;
            }
        }

        private void textBoxAdr_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxAdr.Text))
            {
                textBoxAdr.Text = "Введите адрес";
                textBoxAdr.ForeColor = Color.Gray;
            }
        }

        private void textBoxFam_Enter(object sender, EventArgs e)
        {
            if (textBoxFam.Text == "Введите фамилию")
            {
                textBoxFam.Clear();
                textBoxFam.ForeColor = Color.Black;
            }
        }

        private void textBoxFam_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxFam.Text))
            {
                textBoxFam.Text = "Введите фамилию";
                textBoxFam.ForeColor = Color.Gray;
            }
        }

        private void textBoxCtr_Enter(object sender, EventArgs e)
        {
            if (textBoxCtr.Text == "Введите № договора")
            {
                textBoxCtr.Clear();
                textBoxCtr.ForeColor = Color.Black;
            }
        }

        private void textBoxCtr_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxCtr.Text))
            {
                textBoxCtr.Text = "Введите № договора";
                textBoxCtr.ForeColor = Color.Gray;
            }
        }

        private void textBoxPhn_Enter(object sender, EventArgs e)
        {
            if (textBoxPhn.Text == "Введите телефон")
            {
                textBoxPhn.Clear();
                textBoxPhn.ForeColor = Color.Black;
            }
        }

        private void textBoxPhn_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxPhn.Text))
            {
                textBoxPhn.Text = "Введите телефон";
                textBoxPhn.ForeColor = Color.Gray;
            }
        }

        private void textBoxPt_Enter(object sender, EventArgs e)
        {
            if (textBoxPt.Text == "Введите данные паспорта")
            {
                textBoxPt.Clear();
                textBoxPt.ForeColor = Color.Black;
            }
        }

        private void textBoxPt_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxPt.Text))
            {
                textBoxPt.Text = "Введите данные паспорта";
                textBoxPt.ForeColor = Color.Gray;
            }
        }

        private void textBoxDate_Enter(object sender, EventArgs e)
        {
            if (textBoxDate.Text == "Введите дату")
            {
                textBoxDate.Clear();
                textBoxDate.ForeColor = Color.Black;
            }
        }

        private void textBoxDate_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxDate.Text))
            {
                textBoxDate.Text = "Введите дату";
                textBoxDate.ForeColor = Color.Gray;
            }
        }

        private void textBoxPay_Enter(object sender, EventArgs e)
        {
            if (textBoxPay.Text == "Введите аб. плату")
            {
                textBoxPay.Clear();
                textBoxPay.ForeColor = Color.Black;
            }
        }

        private void textBoxPay_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxPay.Text))
            {
                textBoxPay.Text = "Введите аб. плату";
                textBoxPay.ForeColor = Color.Gray;
            }
        }

        private void textBoxAdr2_Enter(object sender, EventArgs e)
        {
            if (textBoxAdr2.Text == "Введите кв, подъезд")
            {
                textBoxAdr2.Clear();
                textBoxAdr2.ForeColor = Color.Black;
            }
        }

        private void textBoxAdr2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxAdr2.Text))
            {
                textBoxAdr2.Text = "Введите кв, подъезд";
                textBoxAdr2.ForeColor = Color.Gray;
            }
        }

        //--------------------------comboBoxes------------------------------------------------
        private void comboBoxRc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRc.SelectedIndex == 3)
                textBoxRc.Enabled = true;
            else
                textBoxRc.Enabled = false;
        }

        //----------------------------Buttons-------------------------------------------------
        private void buttonSD_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(sqlExpressions[0] + sqlExpressions[1] + sqlExpressions[5] + textBoxSD.Text, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read()) // построчно считываем данные
                {
                    object adress = reader.GetValue(0);
                    object section = reader.GetValue(1);
                    object apartment = reader.GetValue(2);
                    object contract_num = reader.GetValue(3);
                    object surname = reader.GetValue(4);
                    //object phone = reader.GetValue(5);
                    object debt = reader.GetValue(5);
                    //object passport = reader.GetValue(7);
                    //object date_of_contract = reader.GetValue(8);
                    object monthly_fee = reader.GetValue(6);
                    object notice = reader.GetValue(7);

                    foreach (ListViewItem it in listViewS.Items)
                        listViewS.Items.Remove(it);

                    ListViewItem item = new ListViewItem(new string[] { adress.ToString(), section.ToString(), apartment.ToString(), contract_num.ToString(),
                        surname.ToString(), debt.ToString(), monthly_fee.ToString(), notice.ToString() });
                    listViewS.Items.Add(item);
                }
            }
            reader.Close();

            command.CommandText = "SELECT List FROM " + sqlExpressions[2] + sqlExpressions[5] + textBoxSD.Text;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string[] arr = reader.GetValue(0).ToString().Split(',');

                    foreach (ListViewItem it in listViewDets.Items)
                        listViewDets.Items.Remove(it);

                    foreach (string str in arr)
                    {
                        string[] a = str.Split('_');
                        ListViewItem item = new ListViewItem(a);str.Split('_');
                        listViewDets.Items.Add(item);
                    }
                }
            }

            reader.Close();

            textBoxSD.Text = "Введите № договора"; textBoxSD.ForeColor = Color.Gray;
        }

        private void buttonSA_Click(object sender, EventArgs e)
        {
            string[] arr = textBoxSA.Text.Split(';');
            SqlCommand command = new SqlCommand(sqlExpressions[0] + sqlExpressions[1] + "WHERE Adress=" + '\'' + arr[0] + '\'' + " AND Apartment=" + arr[1], sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read()) // построчно считываем данные
                {
                    object adress = reader.GetValue(0);
                    object section = reader.GetValue(1);
                    object apartment = reader.GetValue(2);
                    object contract_num = reader.GetValue(3);
                    object surname = reader.GetValue(4);
                    //object phone = reader.GetValue(5);
                    object debt = reader.GetValue(5);
                    //object passport = reader.GetValue(7);
                    //object date_of_contract = reader.GetValue(8);
                    object monthly_fee = reader.GetValue(6);
                    object notice = reader.GetValue(7);

                    foreach(ListViewItem it in listViewS.Items)
                        listViewS.Items.Remove(it);
                    ListViewItem item = new ListViewItem(new string[] { adress.ToString(), section.ToString(), apartment.ToString(), contract_num.ToString(),
                        surname.ToString(), debt.ToString(), monthly_fee.ToString(), notice.ToString() });
                    listViewS.Items.Add(item);
                }
            }
            reader.Close();

            command.CommandText = "SELECT List FROM " + sqlExpressions[2] + sqlExpressions[5] + listViewS.Items[0].SubItems[4].Text;
            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string[] ar = reader.GetValue(0).ToString().Split(',');

                    foreach (ListViewItem it in listViewDets.Items)
                        listViewS.Items.Remove(it);

                    foreach (string str in ar)
                    {
                        string[] a = str.Split('_');
                        ListViewItem item = new ListViewItem(a); str.Split('_');
                        listViewDets.Items.Add(item);
                    }
                }
            }

            reader.Close();
            textBoxSA.Text = "Введите адрес и кв"; textBoxSA.ForeColor = Color.Gray;
        }
    }
}
