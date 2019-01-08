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
        String[] vs;
        SqlCommand command;
        bool[] flag = { false, false };
        public Information(string contract, SqlConnection connection)
        {
            InitializeComponent();

            dataGridViewInf.RowTemplate.Height = 43;
            dataGridViewInf.AllowUserToAddRows = false;

            Program.form.setButtonCtrInf(false);

            /*SqlCommand */
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM ContractInf WHERE Contract_num = " + contract;

            /*SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet data = new DataSet();
            adapter.Fill(data);
            //dataGridViewInf.DataSource = data.Tables[0];*/

            
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
            //object[] arr = data.Tables[0].Rows[0].ItemArray;
                vs = new String[10];
                String s;
                dataGridViewInf.Rows.Add("№ договора:", reader.GetValue(0).ToString());
                dataGridViewInf.Rows.Add("Дата заключения:", reader.GetDateTime(1).ToShortDateString());//reader.GetValue(1).ToString());
                dataGridViewInf.Rows.Add("ФИО:", reader.GetValue(2).ToString());
                dataGridViewInf.Rows.Add("Паспорт:", reader.GetValue(3).ToString());
                DateTime ttt = reader.GetDateTime(4);
                if (ttt.Year == 1900)
                    s = "";
                else
                    s = ttt.ToShortDateString();
                dataGridViewInf.Rows.Add("Дата выдачи:", s);//reader.GetValue(4).ToString());
                dataGridViewInf.Rows.Add("Выдан:", reader.GetValue(5).ToString());
                dataGridViewInf.Rows.Add("Телефон:", reader.GetValue(6).ToString());
                ttt = reader.GetDateTime(7);
                if (ttt.Year == 1900)
                    s = "";
                else
                    s = ttt.ToShortDateString();
                dataGridViewInf.Rows.Add("Дата рождения:", s);//reader.GetValue(7).ToString());
                dataGridViewInf.Rows.Add("Место рождения:", reader.GetValue(8).ToString());
                ttt = reader.GetDateTime(9);
                if (ttt.Year == 1900)
                    dataGridViewInf.Rows.Add("Дата расторжения:", "");
                else
                    dataGridViewInf.Rows.Add("Дата расторжения:", ttt.ToShortDateString());
            }
            reader.Close();

            command.CommandText = "SELECT Notice FROM Main WHERE Contract_num = '" + contract + "'";
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                richTextBox1.Text = reader.GetString(0);
            }
            reader.Close();
            dataGridViewInf.ClearSelection();

            for (int i = 0; i < 10; i++)
                vs[i] = dataGridViewInf.Rows[i].Cells[1].Value.ToString();

            this.FormClosing += new FormClosingEventHandler(Information_FormClosing);
            dataGridViewInf.CellEndEdit += new DataGridViewCellEventHandler(dataGridViewInf_CellEndEdit);
            richTextBox1.LostFocus += new EventHandler(richTextBox1_LostFocus);
        }

        public void dataGridViewInf_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            flag[0] = true;
            if (dataGridViewInf.CurrentCell.Value == null)
                dataGridViewInf.CurrentCell.Value = "";
            vs[dataGridViewInf.CurrentCell.RowIndex] = dataGridViewInf.CurrentCell.Value.ToString();
        }

        public void richTextBox1_LostFocus(object sender, EventArgs e)
        {
            flag[1] = true;
        }

        public void Information_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flag[0])
            {
                command.CommandText = "UPDATE ContractInf SET Contract_start = '" + vs[1].Replace('.', '-') + "', FIO = '" + vs[2] +
                "', Passport = '" + vs[3] + "', Pt_when = '" + vs[4].Replace('.', '-') + "', Pt_who = '" + vs[5] + "', Phone = '" + vs[6] +
                "', Birth_date = '" + vs[7].Replace('.', '-') + "', Birth_place = '" + vs[8] + "', Contract_end = '" + vs[9].Replace('.', '-') +
                "' WHERE Contract_num = '" + vs[0] + "'";

                command.ExecuteNonQuery();
            }

            if (flag[1])
            {
                command.CommandText = "UPDATE Main SET Notice = '" + richTextBox1.Text + "' WHERE Contract_num = '" + vs[0] + "'";

                command.ExecuteNonQuery();
            }

            //Program.form.listViewS.

            Program.form.setButtonCtrInf(true);
        }
    }
}
