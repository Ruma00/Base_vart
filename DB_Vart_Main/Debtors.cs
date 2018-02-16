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
            Output();
        }

        private SqlConnection connection;

        private void Output()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM Debtors", connection);
            SqlDataReader reader = command.ExecuteReader();

            foreach (ListViewItem it in listViewDeb.Items)
                listViewDeb.Items.Remove(it);
            if (reader.HasRows)
            {
                while (reader.Read()) // построчно считываем данные
                {
                    object adress = reader.GetValue(0);
                    object surname = reader.GetValue(1);
                    object contract_num = reader.GetValue(2);
                    object debt = reader.GetValue(3);

                    ListViewItem item = new ListViewItem(new string[] { adress.ToString(), surname.ToString(), contract_num.ToString(), debt.ToString() });
                    listViewDeb.Items.Add(item);
                }
            }
            reader.Close();
        }
    }
}
