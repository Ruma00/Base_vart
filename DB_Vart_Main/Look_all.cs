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
        public Look_all(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM Main", connection);
            SqlDataReader reader = command.ExecuteReader();

            InitializeComponent();
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
                    object notice = reader.GetValue(7);

                    ListViewItem item = new ListViewItem(new string[] { adress.ToString(), section.ToString(), apartment.ToString(), surname.ToString(),
                        contract_num.ToString(), debt.ToString(), monthly_fee.ToString(), notice.ToString() });
                    listViewLook.Items.Add(item);
                }
            }
            reader.Close();
        }
    }
}
