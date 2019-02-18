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
    public partial class Look_all : Form
    {
        SqlConnection connection;
        int count;
        ContextMenuStrip menu;
        public Look_all(SqlConnection connection)
        {
            this.connection = connection;
            InitializeComponent();
            comboBoxSort.SelectedIndex = 0;
            Program.form.setButtonWrAll(false);
            this.FormClosing += new FormClosingEventHandler(LookAll_FormClosing);
            //listViewLook.DoubleClick += new EventHandler(listViewLook_DoubleClick);
            listViewLook.Click += new EventHandler(listLook_ItemClick);
            Output("");

            List<String> ar = new List<string>();
            ar.Add("Адрес");
            ar.Add("Фамилия");
            ar.Add("Договор");
            ar.Add("Долг");

            ToolStripMenuItem[] items = new ToolStripMenuItem[4];
            for (int i = 0; i < 4; i++)
            {
                items[i] = new ToolStripMenuItem();
                items[i].Text = ar[i];
            }

            menu = new ContextMenuStrip();
            menu.Items.AddRange(items);
            menu.Name = "Копировать";
            menu.ItemClicked += MenuItemClick0;
        }

        private List<BaseElement> list = new List<BaseElement>();

        private void Output(string str)
        {
            if (str == "")
                str = "SELECT Adress, Section, Apartment, Surname, Contract_num, Debt, Monthly_fee, Notice FROM Main";
            SqlCommand command = new SqlCommand(str, connection);
            SqlDataReader reader = command.ExecuteReader();

            foreach (ListViewItem it in listViewLook.Items)
                listViewLook.Items.Remove(it);
            list.Clear();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    BaseElement element = new BaseElement();
                    element.Adress = reader.GetValue(0).ToString();
                    element.Section = reader.GetValue(1).ToString();
                    element.Apartment = reader.GetValue(2).ToString();
                    element.Surname = reader.GetValue(3).ToString();
                    element.Contract = reader.GetValue(4).ToString();
                    element.Debt = reader.GetValue(5).ToString();
                    string fee = reader.GetString(6);
                    string[] temp = fee.Split(',');
                    element.Monthly_fee = temp[temp.Length - 1].Split('_')[0];
                    element.Notice = reader.GetValue(7).ToString();

                    list.Add(element);
                }
            }
            reader.Close();

            int i;
            for (i = 0; i < 20; i++)
            {
                if (i >= list.Count)
                {
                    count = i;
                    buttonNext.Enabled = false;
                    break;
                }
                ListViewItem item = new ListViewItem(new string[] { list[i].Adress, list[i].Section, list[i].Apartment, list[i].Surname,
                        list[i].Contract, list[i].Debt, list[i].Monthly_fee, list[i].Notice });
                listViewLook.Items.Add(item);
            }
            count = i + 1;
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
                ListViewItem item = new ListViewItem(new string[] { list[count].Adress, list[count].Surname, list[count].Contract, list[count].Debt });
                count++;
                listViewLook.Items[i] = item;
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
                case 0: Output("SELECT Adress, Section, Apartment, Surname, Contract_num, Debt, Monthly_fee, Notice FROM Main ORDER BY Adress, Apartment ASC"); break;
                case 1: Output("SELECT Adress, Section, Apartment, Surname, Contract_num, Debt, Monthly_fee, Notice FROM Main ORDER BY Surname ASC"); break;
                case 2: Output("SELECT Adress, Section, Apartment, Surname, Contract_num, Debt, Monthly_fee, Notice FROM Main ORDER BY Contract_num ASC"); break;
                default: Output("SELECT Adress, Section, Apartment, Surname, Contract_num, Debt, Monthly_fee, Notice FROM Main ORDER BY Adress, Apartment ASC"); break;
            }
        }

        public void LookAll_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.form.setButtonWrAll(true);
        }

        private void listViewLook_DoubleClick(object sender, EventArgs e)
        {
            if (listViewLook.SelectedItems.Count > 0)
                Clipboard.SetText(listViewLook.SelectedItems[0].SubItems[4].Text);
        }

        private void listLook_ItemClick(object sender, EventArgs e)
        {
            menu.Show(listViewLook, listViewLook.PointToClient(Cursor.Position));//, Cursor.Position);
        }

        private void MenuItemClick0(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            ListView view = menu.SourceControl as ListView;
            switch (item.Text)
            {
                case "Адрес":
                    Clipboard.SetText(view.SelectedItems[0].SubItems[0].Text + ";" + view.SelectedItems[0].SubItems[2].Text);
                    break;
                case "Фамилия":
                    Clipboard.SetText(view.SelectedItems[0].SubItems[3].Text);
                    break;
                case "Договор":
                    Clipboard.SetText(view.SelectedItems[0].SubItems[4].Text);
                    break;
                case "Долг":
                    Clipboard.SetText(view.SelectedItems[0].SubItems[5].Text);
                    break;
            }
        }
    }
}
