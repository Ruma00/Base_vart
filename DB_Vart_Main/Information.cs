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
    public partial class Information : Form
    {
        public Information(string contract, SqlConnection connection)
        {
            InitializeComponent();

            string comText = "SELECT Contract_num, Surname, Date_of_contract, Phone, Passport FROM Main WHERE Contract_num = '" + contract + "'";
            SqlCommand command = new SqlCommand(comText, connection);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    labelCtrN.Text = reader.GetValue(0).ToString();
                    labelSurN.Text = reader.GetValue(1).ToString();
                    labelDate.Text = reader.GetValue(2).ToString().Split(' ')[0];
                    labelPhn.Text = reader.GetValue(3).ToString();
                    labelPt.Text = reader.GetValue(4).ToString();
                }
            }
            reader.Close();
        }
    }
}
