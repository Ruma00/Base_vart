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

            dataGridViewInf.RowTemplate.Height = 43;
            dataGridViewInf.AllowUserToAddRows = false;

            Program.form.setButtonCtrInf(false);

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM ContractInf WHERE Contract_num = " + contract;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dataGridViewInf.Rows.Add("№ договора:", reader.GetValue(0).ToString());
                dataGridViewInf.Rows.Add("Дата заключения:", reader.GetValue(1).ToString());
                dataGridViewInf.Rows.Add("ФИО:", reader.GetValue(2).ToString());
                dataGridViewInf.Rows.Add("Паспорт:", reader.GetValue(3).ToString());
                dataGridViewInf.Rows.Add("Дата выдачи:", reader.GetValue(4).ToString());
                dataGridViewInf.Rows.Add("Выдан:", reader.GetValue(5).ToString());
                dataGridViewInf.Rows.Add("Телефон:", reader.GetValue(6).ToString());
                dataGridViewInf.Rows.Add("Дата рождения:", reader.GetValue(7).ToString());
                dataGridViewInf.Rows.Add("Место рождения:", reader.GetValue(8).ToString());
                DateTime temp = reader.GetDateTime(9);
                if (temp.Year == 1)
                    dataGridViewInf.Rows.Add("Дата расторжения:", "---------------");
                else
                    dataGridViewInf.Rows.Add("Дата расторжения:", temp.ToString());
            }
            reader.Close();
            dataGridViewInf.ClearSelection();

            this.FormClosing += new FormClosingEventHandler(Information_FormClosing);
        }

        public void Information_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.form.setButtonCtrInf(true);
        }
    }
}
