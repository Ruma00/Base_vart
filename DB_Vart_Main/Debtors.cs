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
    public partial class Debtors : Form
    {
        public Debtors(SqlConnection connection)
        {
            InitializeComponent();
            this.connection = connection;
            comboBoxSort.SelectedIndex = 0;
            Program.form.setButtonDt(false);
            this.FormClosing += new FormClosingEventHandler(Debtors_FormClosing);
            listViewDeb.DoubleClick += new EventHandler(listViewDeb_DoubleClick);
            Output();
        }

        ~Debtors()
        {
            Program.form.setButtonDt(true);
        }

        private SqlConnection connection;
        private int count = 0;

        private List<BaseElement> list = new List<BaseElement>();

        private void Output(string str = "SELECT Adress, Apartment, Surname, Contract_num, Debt, Notice FROM Main WHERE Is_Deleted = 1")
        {

            SqlCommand command = new SqlCommand(str, connection);
            SqlDataReader reader = command.ExecuteReader();

            foreach (ListViewItem it in listViewDeb.Items)
                listViewDeb.Items.Remove(it);
            list.Clear();
            if (reader.HasRows)
            {
                while (reader.Read()) // построчно считываем данные
                {
                    BaseElement element = new BaseElement();
                    element.Adress = reader.GetValue(0).ToString() + " кв " + reader.GetValue(1).ToString();
                    element.Surname = reader.GetValue(2).ToString();
                    element.Contract = reader.GetValue(3).ToString();
                    element.Debt = reader.GetValue(4).ToString();
                    element.Notice = reader.GetString(5);

                    list.Add(element);
                }
            }
            reader.Close();

            for (int i = 0; i < 20; i++)
            {
                if (i >= list.Count)
                {
                    count -= i;
                    buttonNext.Enabled = false;
                    break;
                }
                ListViewItem item = new ListViewItem(new string[] { list[i].Adress, list[i].Surname, list[i].Contract, list[i].Debt, list[i].Notice });
                listViewDeb.Items.Add(item);
            }
            count = 20;
        }

        private void Write()
        {
            for (int i = 0; i < 25; i++)
            {
                if (count >= list.Count)
                {
                    count -= i;
                    buttonNext.Enabled = false;
                    break;
                }
                ListViewItem item = new ListViewItem(new string[] { list[count].Adress, list[count].Surname, list[count].Contract, list[count].Debt, list[i].Notice });
                count++;
                listViewDeb.Items[i] = item;
            }
            labelSort.Text = (count - 25 + 1).ToString() + " - " + count.ToString();
            buttonPrev.Enabled = true;
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            count -= 50;
            if (count == 0)
                buttonPrev.Enabled = false;
            Write();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            Write();
        }

        private void buttonSort_Click(object sender, EventArgs e) 
        {
            buttonPrev.Enabled = false;
            switch (comboBoxSort.SelectedIndex)
            {
                case 0: Output(str : "SELECT Adress, Apartment, Surname, Contract_num, Debt, Notice FROM Main ORDER BY Adress ASC"); break;
                case 1: Output(str : "SELECT Adress, Apartment, Surname, Contract_num, Debt, Notice FROM Main ORDER BY Surname ASC"); break;
                case 2: Output(str : "SELECT Adress, Apartment, Surname, Contract_num, Debt, Notice FROM Main ORDER BY Contract_num ASC"); break;
            }
        }

        private void Debtors_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.form.setButtonDt(true);
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;

            for (int i = 0; i < listViewDeb.SelectedItems.Count; i++)
            {
                ListViewItem item = listViewDeb.SelectedItems[i];
                //command.CommandText += item.SubItems[2].Text;

                command.CommandText = "UPDATE Main SET Is_Deleted = 0 WHERE Contract_num = " + item.SubItems[2].Text;
                command.ExecuteNonQuery();
            }

            Output();
        }

        private void listViewDeb_DoubleClick(object sender, EventArgs e)
        {
            if (listViewDeb.SelectedItems.Count > 0)
                Clipboard.SetText(listViewDeb.SelectedItems[0].SubItems[4].Text);
        }
    }
}
