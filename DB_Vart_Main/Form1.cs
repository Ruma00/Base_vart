using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Xps.Packaging;

namespace DB_Vart_Main
{
    public partial class Main_form : Form
    {
        SqlConnection sqlConnection = new SqlConnection();
        public Main_form()
        {
            try
            {
                StreamReader reader = new StreamReader("dat.txt");
                string line = reader.ReadLine();
                reader.Close();
                int month = Convert.ToInt32(line);
                if (month != DateTime.Today.Month)
                {
                    StreamWriter writer = new StreamWriter("dat.txt", false);
                    writer.WriteLine(DateTime.Today.Month);
                    for (int i = 0; i < DateTime.Today.Month - month; i++)
                    {
                        SqlCommand command = new SqlCommand("UPDATE Main SET Debt -= 80");
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                FileStream file = new FileStream("dat.txt", FileMode.OpenOrCreate);
                byte[] array = Encoding.Default.GetBytes(DateTime.Today.Month.ToString());
                file.Write(array, 0, array.Length);
                file.Close();
            }

            InitializeComponent();
            sqlConnection.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Base;Integrated Security=true";
            sqlConnection.Open();

            comboBoxRc.SelectedIndex = 0;

            listViewS.DoubleClick += new EventHandler(listViewS_DoubleClick);
            listViewAddP.DoubleClick += new EventHandler(listViewAddP_DoubleClick);
            listViewDel.DoubleClick += new EventHandler(listViewDel_DoubleClick);
            listViewRc.DoubleClick += new EventHandler(listViewRc_DoubleClick);

            dataGridViewAddP.Rows.Add();
            dataGridViewAddP.ClearSelection();
            dataGridViewAddP.Rows[0].Cells[0].Selected = false;

            textBoxSD.Text = "Введите № договора"; textBoxSD.ForeColor = Color.Gray;
            textBoxSA.Text = "Введите адрес и кв"; textBoxSA.ForeColor = Color.Gray;
            textBoxPayCH.Text = "Смена аб. платы"; textBoxPayCH.ForeColor = Color.Gray;
            textBoxAddP.Text = "Введите адрес или № договора"; textBoxAddP.ForeColor = Color.Gray;
            textBoxDel.Text = "Введите адрес или № договора"; textBoxDel.ForeColor = Color.Gray;
            /*textBoxAdr.Text = "Введите адрес"; textBoxAdr.ForeColor = Color.Gray;
            textBoxFam.Text = "Введите фамилию"; textBoxFam.ForeColor = Color.Gray;
            textBoxCtr.Text = "Введите № договора"; textBoxCtr.ForeColor = Color.Gray;
            textBoxPhn.Text = "Введите телефон"; textBoxPhn.ForeColor = Color.Gray;
            textBoxPt.Text = "Введите данные паспорта"; textBoxPt.ForeColor = Color.Gray;
            textBoxDate.Text = "Введите дату"; textBoxDate.ForeColor = Color.Gray;
            textBoxPay.Text = "Введите аб. плату"; textBoxPay.ForeColor = Color.Gray;
            textBoxAdr2.Text = "Введите подъезд, кв"; textBoxAdr2.ForeColor = Color.Gray;
            textBoxDebt.Text = "0"; textBoxDebt.ForeColor = Color.Gray;*/

            /*textBoxAdr.Enter += new EventHandler(textBoxAdr_Enter);
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
            textBoxAdr2.Leave += new EventHandler(textBoxAdr2_Leave);*/

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
            dataGridViewAddP.Rows[0].Cells[0].Value = "";
            dataGridViewAddP.Rows[0].Cells[1].Value = "";
            dataGridViewAddP.Rows[0].Cells[2].Value = "";
            dataGridViewAddP.Rows[0].Cells[3].Value = "";

            dataGridViewAddP.ClearSelection();
        }

        string[] sqlExpressions = new string[] { "SELECT Adress, Section, Apartment, Surname, Contract_num, Debt, Monthly_fee, Notice FROM ", "Main ", "Payments ",
            "UPDATE Main SET ", "INSERT INTO Main VALUES ", "WHERE Contract_num=", "Debtors" };

        Regex regex1 = new Regex(@"\d*"); //numeric
        Regex regex2 = new Regex(@"\w*"); //alpha-numeric

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
            if (textBoxAdr2.Text == "Введите подъезд, кв")
            {
                textBoxAdr2.Clear();
                textBoxAdr2.ForeColor = Color.Black;
            }
        }

        private void textBoxAdr2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxAdr2.Text))
            {
                textBoxAdr2.Text = "Введите подъезд, кв";
                textBoxAdr2.ForeColor = Color.Gray;
            }
        }

        private void listViewS_DoubleClick(object sender, EventArgs e)
        {
            if (listViewS.SelectedItems.Count > 0)
                Clipboard.SetText(listViewS.SelectedItems[0].SubItems[4].Text);
        }

        private void listViewAddP_DoubleClick(object sender, EventArgs e)
        {
            if (listViewAddP.SelectedItems.Count > 0)
                Clipboard.SetText(listViewAddP.SelectedItems[0].SubItems[4].Text);
        }
        private void listViewDel_DoubleClick(object sender, EventArgs e)
        {
            if (listViewDel.SelectedItems.Count > 0)
                Clipboard.SetText(listViewDel.SelectedItems[0].SubItems[4].Text);
        }
        private void listViewRc_DoubleClick(object sender, EventArgs e)
        {
            if (listViewRc.SelectedItems.Count > 0)
                Clipboard.SetText(listViewRc.SelectedItems[0].SubItems[4].Text);
        }

        //--------------------------comboBoxes------------------------------------------------
        private void comboBoxRc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRc.SelectedIndex == 3)
                textBoxRc.Enabled = true;
            else
                textBoxRc.Enabled = false;
        }

        private void SqlReader(SqlDataReader reader, ListView list)
        {
            foreach (ListViewItem it in list.Items)
                list.Items.Remove(it);
            if (reader.HasRows)
            {
                while (reader.Read()) // построчно считываем данные
                {
                    object adress = reader.GetValue(0);
                    object section = reader.GetValue(1);
                    object apartment = reader.GetValue(2);
                    object surname = reader.GetValue(3);
                    object contract_num = reader.GetValue(4);
                    //object phone = reader.GetValue(5);
                    object debt = reader.GetValue(5);
                    //object passport = reader.GetValue(7);
                    //object date_of_contract = reader.GetValue(8);
                    object monthly_fee = reader.GetValue(6);
                    string[] temp = monthly_fee.ToString().Split(',');
                    object notice = reader.GetValue(7);

                    string[] tmp = temp[temp.Length - 1].Split('_');
                    ListViewItem item = new ListViewItem(new string[] { adress.ToString(), section.ToString(), apartment.ToString(), surname.ToString(),
                        contract_num.ToString(), debt.ToString(), tmp[0], notice.ToString() });
                    list.Items.Add(item);
                }
            }
            reader.Close();
        }

        private void SqlReadDate(SqlDataReader reader)
        {
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
                        ListViewItem item = new ListViewItem(a); //str.Split('_');
                        listViewDets.Items.Add(item);
                    }
                }
            }

            reader.Close();
        }

        private bool CheckAdd(string[] adr)
        {
            if (adr.Length == 1)
            {
                MessageBox.Show("Введите корректные данные: подъезд и кв");
                return false;
            }
            if (textBoxPhn.Text.Length > 11)
            {
                MessageBox.Show("Введите корректные данные: телефон");
                return false;
            }
            if (textBoxCtr.Text.Length > 15)
            {
                MessageBox.Show("Введите корректные данные: номер договора");
                return false;
            }
            if (textBoxPt.Text.Length > 11)
            {
                MessageBox.Show("Введите корректные данные: паспортные данные");
                return false;
            }
            return true;
        }

        private void Search(TextBox textBox, ListView list)
        {
            string[] split = textBox.Text.Split(';');
            if (split.Length > 1)
            {
                SqlCommand command = new SqlCommand(sqlExpressions[0] + sqlExpressions[1] + "WHERE Adress='" + split[0] + "' AND Apartment=" + split[1], sqlConnection);
                SqlReader(command.ExecuteReader(), list);
                command.CommandText = "SELECT List FROM " + sqlExpressions[2] + sqlExpressions[5] + list.Items[0].SubItems[4].Text;
                SqlReadDate(command.ExecuteReader());
            }
            else
            {
                SqlCommand command = new SqlCommand(sqlExpressions[0] + sqlExpressions[1] + sqlExpressions[5] + textBox.Text, sqlConnection);

                SqlReader(command.ExecuteReader(), list);
            }
        }

        //----------------------------Buttons-------------------------------------------------
        private void buttonSD_Click(object sender, EventArgs e)
        {
            if (textBoxSA.Text != "Введите адрес и кв")
            {
                string[] arr = textBoxSA.Text.Split(';');
                SqlCommand command = new SqlCommand(sqlExpressions[0] + sqlExpressions[1] + "WHERE Adress=" + '\'' + arr[0] + '\'' + " AND Apartment=" + arr[1], sqlConnection);

                SqlReader(command.ExecuteReader(), listViewS);

                command.CommandText = "SELECT List FROM " + sqlExpressions[2] + sqlExpressions[5] + listViewS.Items[0].SubItems[4].Text;

                SqlReadDate(command.ExecuteReader());

                textBoxSA.Text = "Введите адрес и кв"; textBoxSA.ForeColor = Color.Gray;
            }
            else if (textBoxSD.Text != "Введите № договора")
            {
                SqlCommand command = new SqlCommand(sqlExpressions[0] + sqlExpressions[1] + sqlExpressions[5] + textBoxSD.Text, sqlConnection);

                SqlReader(command.ExecuteReader(), listViewS);

                command.CommandText = "SELECT List FROM " + sqlExpressions[2] + sqlExpressions[5] + textBoxSD.Text;

                SqlReadDate(command.ExecuteReader());

                textBoxSD.Text = "Введите № договора"; textBoxSD.ForeColor = Color.Gray;
            }
            if (listViewS.Items.Count == 1)
            {
                buttonCtrInf.Enabled = true;
                buttonAct.Enabled = true;
            }
        }

        private void buttonSA_Click(object sender, EventArgs e)
        {
            string[] arr = textBoxSA.Text.Split(';');
            SqlCommand command = new SqlCommand(sqlExpressions[0] + sqlExpressions[1] + "WHERE Adress=" + '\'' + arr[0] + '\'' + " AND Apartment=" + arr[1], sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            SqlReader(reader, listViewS);

            command.CommandText = "SELECT List FROM " + sqlExpressions[2] + sqlExpressions[5] + listViewS.Items[0].SubItems[4].Text;
            reader = command.ExecuteReader();

            SqlReadDate(reader);

            textBoxSA.Text = "Введите адрес и кв"; textBoxSA.ForeColor = Color.Gray;
        }

        private void buttonChgAP_Click(object sender, EventArgs e)
        {
            string contract = listViewS.Items[0].SubItems[4].Text;
            string[] str = richTextBoxNcFee.Text.Split(';');

            SqlCommand command = new SqlCommand(sqlExpressions[3] + " Monthly_fee += '," + textBoxPayCH.Text + 
                '_' + str[0] + "' " + sqlExpressions[5] + contract, sqlConnection);
            MessageBox.Show(command.CommandText);
            command.ExecuteNonQuery();

            command.CommandText = sqlExpressions[3] + " Notice += '" + str[1] + "'";
            MessageBox.Show(command.CommandText);
            command.ExecuteNonQuery();
            
            //command.CommandText = sqlExpressions[0] + sqlExpressions[1] + sqlExpressions[5] + contract;

            //SqlDataReader reader = command.ExecuteReader();

            //SqlReader(reader, listViewS);
            listViewS.Items[0].SubItems[6].Text = textBoxPayCH.Text;

            textBoxPayCH.Text = "Смена аб. платы"; textBoxPayCH.ForeColor = Color.Gray;
        }

        private void buttonAddAb_Click(object sender, EventArgs e)
        {
            string[] adr = textBoxAdr2.Text.Split(',');
            if (!CheckAdd(adr))
                return;

            int debt = Convert.ToInt32(textBoxDebt.Text);
            DateTime date = Convert.ToDateTime(textBoxDate.Text);
            DateTime today = DateTime.Today;

            int u, month;
            //string[] temp = textBoxPay.Text.Split('_');
            int pay = Convert.ToInt32(textBoxPay.Text);
            if (date.Year < 2015)
            {
                u = today.Year - 2015;
                month = today.Month - 1;
            }
            else
            {
                u = today.Year - date.Year - 1;
                if (u < 0)
                {
                    u = 0;
                    month = today.Month - date.Month;
                }
                else
                    month = 12 - date.Month + today.Month;
            }
            debt += pay * (u * 12 + month);

            SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;
            //SqlCommand command = new SqlCommand(sqlExpressions[4] + "('" + textBoxAdr.Text + "'," + adr[0] + "," + adr[1] + ",'" + textBoxFam.Text + "','" + textBoxCtr.Text +
            //    "','" + textBoxPhn.Text + "'," + debt.ToString() + ",'" + textBoxPt.Text + "','" + textBoxDate.Text + "'," + textBoxPay.Text + ",'" + richTextBoxNote.Text + "')", sqlConnection);
            command.CommandText = sqlExpressions[4] + "('" + textBoxAdr.Text + "'," + adr[0] + "," + adr[1] + ",'" + textBoxFam.Text + "','" + textBoxCtr.Text +
                                "','" + textBoxPay.Text + "_" + textBoxDate.Text + "','" + debt.ToString() + "','" + richTextBoxNote.Text + "',0)";
            command.ExecuteNonQuery();

            //String adress = textBoxAdr.Text + " кв " + adr[1];
            command.CommandText = "INSERT INTO ContractInf VALUES ('" + textBoxCtr.Text + "','" + textBoxDate.Text + "','" +
                richTextBoxFIO.Text + "','" + textBoxPt.Text + "','" + textBoxPt_Date.Text + "','" + richTextBoxPtWho.Text + "','" +
                textBoxPhn.Text + "','" + textBoxBrDt.Text + "','" + richTextBoxBrPl.Text + "','01.01.0001')";
            command.ExecuteNonQuery();
            //command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Payments VALUES ('" + textBoxCtr.Text + "','')";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO ToExcel VALUES ('" + textBoxFam.Text + "','" + textBoxAdr.Text + ", " + adr[1] +
                                                "','" + textBoxCtr.Text + "','" + textBoxDebt.Text + "')";
            command.ExecuteNonQuery();

            /*textBoxAdr.Text = "Введите адрес"; textBoxAdr.ForeColor = Color.Gray;
            textBoxFam.Text = "Введите фамилию"; textBoxFam.ForeColor = Color.Gray;
            textBoxCtr.Text = "Введите № договора"; textBoxCtr.ForeColor = Color.Gray;
            textBoxPhn.Text = "Введите телефон"; textBoxPhn.ForeColor = Color.Gray;
            textBoxPt.Text = "Введите данные паспорта"; textBoxPt.ForeColor = Color.Gray;
            textBoxDate.Text = "Введите дату"; textBoxDate.ForeColor = Color.Gray;
            textBoxPay.Text = "Введите аб. плату"; textBoxPay.ForeColor = Color.Gray;
            textBoxAdr2.Text = "Введите кв, подъезд"; textBoxAdr2.ForeColor = Color.Gray;
            textBoxDebt.Text = "0"; textBoxDebt.ForeColor = Color.Gray;*/
            textBoxAdr.Text = "";           textBoxPt_Date.Text = "";
            textBoxFam.Text = "";           richTextBoxFIO.Text = "";
            textBoxCtr.Text = "";           richTextBoxPtWho.Text = "";
            textBoxPhn.Text = "";           textBoxBrDt.Text = "";
            textBoxPt.Text = "";            richTextBoxBrPl.Text = "";
            textBoxDate.Text = "";
            textBoxPay.Text = "";
            textBoxAdr2.Text = "";
            textBoxDebt.Text = "";
            richTextBoxNote.Text = "";
        }

        

        private void buttonAddPS_Click(object sender, EventArgs e)
        {
            Search(textBoxAddP, listViewAddP);

            textBoxAddP.Text = "Введите адрес или № договора"; textBoxAddP.ForeColor = Color.Gray;
            dataGridViewAddP.ClearSelection();
        }

        private void buttonAddP_Click(object sender, EventArgs e)
        {
            //string str = dataGridViewAddP[0, 0].Value.ToString() + "_" + dataGridViewAddP[1, 0].Value.ToString();
            string str = dataGridViewAddP.Rows[0].Cells[0].Value.ToString() + "_" + dataGridViewAddP.Rows[0].Cells[1].Value.ToString();
            string contract_num = listViewAddP.Items[0].SubItems[4].Text;

            SqlCommand command = new SqlCommand("SELECT List FROM Payments WHERE Contract_num='" + contract_num + '\'', sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            string arr = "";
            //MessageBox.Show(reader.HasRows.ToString());
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string temp = reader.GetValue(0).ToString();
                    if (temp == "")
                        arr = "'" + str + "'";
                    else
                        arr = "'" + temp + "," + str + "'";
                    string[] ar = temp.Split(',');

                    foreach (ListViewItem it in listViewDets.Items)
                        listViewDets.Items.Remove(it);

                    foreach (string st in ar)
                    {
                        string[] a = st.Split('_');
                        ListViewItem item = new ListViewItem(a); //st.Split('_');
                        listViewDets.Items.Add(item);
                    }
                }
            }
            reader.Close();

            dataGridViewAddP.ClearSelection();

            command.CommandText = "UPDATE Payments SET List=" + arr + " WHERE Contract_num='" + contract_num + '\'';
            command.ExecuteNonQuery();

            command.CommandText = "SELECT Notice FROM Main WHERE Contract_num='" + contract_num + "'";
            reader = command.ExecuteReader();
            reader.Read();
            string notice = "";
            if (reader.GetValue(0) == null)
                notice = "";
            else if (dataGridViewAddP.Rows[0].Cells[3].Value.ToString() != "" || dataGridViewAddP.Rows[0].Cells[2].Value.ToString() != "")
            {
                if (reader.GetValue(0).ToString() == "")
                    notice = dataGridViewAddP.Rows[0].Cells[3].Value.ToString() + "(" + dataGridViewAddP.Rows[0].Cells[2].Value.ToString() + ")";
                else
                    notice = reader.GetValue(0).ToString() + "," + dataGridViewAddP.Rows[0].Cells[3].Value.ToString() + "(" +
                                                                            dataGridViewAddP.Rows[0].Cells[2].Value.ToString() + ")";
            }
            reader.Close();
            command.CommandText = "UPDATE Main SET Notice='" + notice + "' WHERE Contract_num='" + contract_num + '\'';
            command.ExecuteNonQuery();

            command.CommandText = "SELECT List FROM Payments WHERE Contract_num='" + contract_num + '\'';
            SqlReadDate(command.ExecuteReader());

            string strCom = "UPDATE Main SET Debt -= " + dataGridViewAddP.Rows[0].Cells[1].Value.ToString() + " WHERE Contract_num = '" + contract_num + "'";
            command.CommandText = strCom;
            command.ExecuteNonQuery();

            command.CommandText = sqlExpressions[0] + "Main WHERE Contract_num='" + contract_num + "'";
            SqlReader(command.ExecuteReader(), listViewAddP);
            SqlReader(command.ExecuteReader(), listViewS);

            dataGridViewAddP.Rows.Clear();
            dataGridViewAddP.Rows.Add();
            dataGridViewAddP.ClearSelection();

            dataGridViewAddP.Rows[0].Cells[2].Value = "";
            dataGridViewAddP.Rows[0].Cells[3].Value = "";
        }

        private void buttonDelS_Click(object sender, EventArgs e)
        {
            Search(textBoxDel, listViewDel);

            textBoxDel.Text = "Введите адрес или № договора"; textBoxDel.ForeColor = Color.Gray;
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;

            string str = richTextBoxNcDel.Text;
            richTextBoxNcDel.Text = "";
            command.CommandText = "UPDATE Main SET Notice += '" + str + "' WHERE Contract_num = " + listViewDel.Items[0].SubItems[4].Text;
            command.ExecuteNonQuery();


            if (Convert.ToInt16(listViewDel.Items[0].SubItems[5].Text) > 0)
            {
                /*command.CommandText = "INSERT INTO Debtors VALUES ('" + listViewDel.Items[0].SubItems[0].Text + "_" + listViewDel.Items[0].SubItems[2].Text +
                    "','" + listViewDel.Items[0].SubItems[3].Text + "','" + listViewDel.Items[0].SubItems[4].Text + "'," + listViewDel.Items[0].SubItems[5].Text + ")";
                //command.Connection = sqlConnection;
                command.ExecuteNonQuery();*/
                command.CommandText = "UPDATE Main SET Is_Deleted = 1 WHERE Contract_num = " + listViewDel.Items[0].SubItems[4].Text;
                command.ExecuteNonQuery();

                MessageBox.Show("Оставлен для суда из-за наличия долга");
            }
            else
            {
                string ctrNum = listViewDel.Items[0].SubItems[4].Text;
                command.CommandText = "DELETE FROM Main WHERE Contract_num = " + ctrNum;
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM Payments WHERE Contract_num = " + ctrNum;
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM ToExcel WHERE Contract_num = " + ctrNum;
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM ContractInf WHERE Contract_num = " + ctrNum;
                command.ExecuteNonQuery();

                MessageBox.Show("Удалён");
            }

            foreach (ListViewItem it in listViewDel.Items)
                listViewDel.Items.Remove(it);
            listViewS.Items.Clear();
            buttonAct.Enabled = false;
            buttonCtrInf.Enabled = false;
        }

        private void buttonDt_Click(object sender, EventArgs e)
        {
            Debtors Debtors = new Debtors(sqlConnection);
            Debtors.Show();
        }

        private void buttonExp_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("SELECT Surname, Adress, Apartment, Contract_num, Debt FROM Main", sqlConnection);

            FolderBrowserDialog dialog = new FolderBrowserDialog();


            //TODO
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;
            string path = dialog.SelectedPath;

            /*FileInfo file = new FileInfo(path + "\\export.txt");
            if (!file.Exists)
                file.Create();*/
            

            StreamWriter writer = new StreamWriter(path + "\\export.txt", false, Encoding.Default);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    object surname = reader.GetValue(0);
                    object adress = reader.GetValue(1);
                    object apartment = reader.GetValue(2);
                    object contruct_num = reader.GetValue(3);
                    object debt = reader.GetValue(4);

                    string expStr = surname.ToString().ToUpper() + ";НИЖНЕВАРТОВСК," + adress.ToString().ToUpper() + "," + apartment.ToString().ToUpper() + ";" +
                                contruct_num.ToString().ToUpper() + ";" + debt.ToString().ToUpper() + ".00;;;;";

                    writer.WriteLine(expStr);
                }
                writer.Close();
            }
            reader.Close();
        }

        private void buttonIm_Click(object sender, EventArgs e)
        {
            //TODO
            /*FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();

            string path = dialog.SelectedPath + "\\import.txt";

            FileInfo file = new FileInfo(path);
            if (!file.Exists)
                file.Create();*/

            bool flag = false;

            //File file = new FileInfo("impErr.txt");
            if (!File.Exists("impErr.txt"))
            {
                //File.Delete("inpErr.txt");
                File.Create("inpErr.txt").Close();
            }
            //else
                //file.Create();
            
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Выберите файл импорта";

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;
            StreamReader fileReader = new StreamReader(dialog.FileName, Encoding.Default);

            string line = "";
            //------------------------------------------------WRITER----------------------------------------------------
            using (StreamWriter writer = new StreamWriter("impErr.txt", false, Encoding.Default))
            {
                while ((line = fileReader.ReadLine()) != null)
                {
                    if (line == "" || line[0] == '~')
                        continue;
                    string[] split = line.Split(';');
                    /*string[] splUse = new string[] { split[2], split[4], split[5].Replace("ЛИЦЕВОЙ СЧЕТ: ", ""), split[6].Replace(" ФИО: ", ""),
                                split[7].Replace(" АДРЕС: НИЖНЕВАРТОВСК,", ""), split[8].Replace(" К_ОПЛАТЕ: ", "") };*/

                    string[] splUse = new string[] { split[2].Replace("/", "."), split[4].Replace(".00", ""), split[5].Replace("ЛИЦЕВОЙ СЧЕТ: ", "") };

                    SqlCommand command = new SqlCommand();
                    command.Connection = sqlConnection;

                    //
                    command.CommandText = "SELECT COUNT(Contract_num) FROM Main WHERE Contract_num = " + splUse[2];
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    //

                    if (reader.GetInt32(0) == 0)
                    {
                        writer.WriteLine(split[6].Replace(" ФИО: ", "") + " " + splUse[0] + " " + splUse[1] + " " + splUse[2]);
                        reader.Close();
                        flag = true;
                        continue;
                    }
                    else
                        reader.Close();
                    //

                    command.CommandText = "SELECT List FROM Payments WHERE Contract_num = '" + splUse[2] + "'";
                    reader = command.ExecuteReader();

                    List<string> list = new List<string>();
                    while (reader.Read())
                    {
                        object value = reader.GetValue(0);
                        if (value.ToString() != "")
                            line = value.ToString() + "," + splUse[0] + "_" + splUse[1];
                        else
                            line = splUse[0] + "_" + splUse[1];
                        list.Add("UPDATE Payments SET List = '" + line + "' WHERE Contract_num = '" + splUse[2] + "'");
                        list.Add("UPDATE Main SET Debt -= " + splUse[1]);
                        /*SqlCommand update = new SqlCommand("UPDATE Payments SET List = '" + line + "' WHERE Contract_num = '" + splUse[2] + "'", sqlConnection);
                        update.ExecuteNonQueryAsync();*/
                        line = "";
                    }
                    reader.Close();

                    foreach (string str in list)
                    {
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                    }
                }
            }

            if (flag)
            {
                DialogResult res = MessageBox.Show("Обнаружены отсутствующие записи\nПросмотреть?", "Ошибки импорта", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    TxtImport form = new TxtImport();
                    form.Show();
                }
                //FileDialog dialog = new FileDialog
            }
        }

        private void buttonCtrInf_Click(object sender, EventArgs e)
        {
            string contract = listViewS.Items[0].SubItems[4].Text;
            Information information = new Information(contract, sqlConnection);
            information.Show();
        }

        private void buttonWrAll_Click(object sender, EventArgs e)
        {
            Look_all look_All = new Look_all(sqlConnection);
            look_All.Show();
        }

        private void listViewRc_ItemClick(object sender, EventArgs e)
        {
            //TODO
        }

        private void buttonRcPrint_Click(object sender, EventArgs e)
        {
            ViewReceipts receipts = new ViewReceipts();
            receipts.Show();
        }

        private void buttonAct_Click(object sender, EventArgs e)
        {
            string contract = listViewS.Items[0].SubItems[4].Text;
            string name = listViewS.Items[0].SubItems[3].Text;
            string adress = listViewS.Items[0].SubItems[0].Text + " кв. " + listViewS.Items[0].SubItems[2].Text;
            Act act = new Act(contract, name, adress, sqlConnection);
            act.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void setButtonAct(bool set)
        {
            buttonAct.Enabled = set;
        }

        public void setButtonCtrInf(bool set)
        {
            buttonCtrInf.Enabled = set;
        }

        public void setButtonDt(bool set)
        {
            buttonDt.Enabled = set;
        }

        public void setButtonWrAll(bool set)
        {
            buttonWrAll.Enabled = set;
        }

        private void listViewRc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void labelRcpt_Click(object sender, EventArgs e)
        {

        }

        private void textBoxRc_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonRc_Click(object sender, EventArgs e)
        {
            string count;
            switch (comboBoxRc.SelectedIndex)
            {
                case 0: count = "200"; break;
                case 1: count = "500"; break;
                case 2: count = "1000"; break;
                case 3: count = textBoxRc.Text; break;
                default: count = "200"; break;
            }

            SqlCommand command = new SqlCommand(sqlExpressions[0] + sqlExpressions[1] + "WHERE Debt>=" + count, sqlConnection);

            SqlReader(command.ExecuteReader(), listViewRc);
            textBoxRc.Text = ""; textBoxRc.Enabled = false;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
