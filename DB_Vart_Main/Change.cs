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
    public partial class Change : Form
    {
        SqlCommand command;
        SqlConnection connection;
        String contract;
        ListView list;
        int count = 0;

        public Change(string contract, SqlConnection connection, ref ListView list)
        {
            InitializeComponent();

            dataGridViewInf.RowTemplate.Height = 43;
            dataGridViewInf.AllowUserToAddRows = true;

            this.contract = contract;
            this.list = list;
            this.connection = connection;

            this.FormClosing += new FormClosingEventHandler(Change_FormClosing);

            command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT Monthly_fee FROM Main WHERE Contract_num = " + contract;
            SqlDataReader reader = command.ExecuteReader();

            See(reader);
        }

        public void See(SqlDataReader reader)
        {
            while (reader.Read())
            {
                String s = reader.GetString(0);
                String[] str = s.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    String[] h = str[i].Split('_');
                    dataGridViewInf.Rows.Add(h[0], h[1]);
                    //count++;
                }
            }
            reader.Close();
        }

        private double DebtCalc(SqlDataReader reader, string contract)
        {
            reader.Read();
            String str = reader.GetString(0);
            reader.Close();

            List<Dat> list = new List<Dat>();

            Dat dat;
            String[] vb = str.Split(',');
            for (int i = 0; i < vb.Length; i++)
            {
                dat = new Dat();
                String[] h = vb[i].Split('_');
                dat.Fee = Convert.ToInt32(h[0]);
                dat.Date = Convert.ToDateTime(h[1]);
                list.Add(dat);
            }

            DateTime date, end;
            int u = 0;
            double debt = 0;
            for (int i = 0; i <= list.Count - 2; i++)
            {
                u = list[i].Fee;
                date = Convert.ToDateTime(list[i].Date);

                if (date.Day > 15)
                {
                    debt += u / 2;
                    date = date.AddMonths(1);
                }

                end = Convert.ToDateTime(list[i + 1].Date);

                while (date < end)
                {
                    debt += u;
                    date = date.AddMonths(1);
                }
            }

            SqlCommand command = new SqlCommand("SELECT Contract_end FROM ContractInf WHERE Contract_num = '" + contract + "'", connection);
            reader = command.ExecuteReader();
            reader.Read();

            end = reader.GetDateTime(0);
            if (end.Year == 1900)
                end = DateTime.Today;
            reader.Close();

            date = list[list.Count - 1].Date;
            u = list[list.Count - 1].Fee;
            while (date < end)
            {
                debt += u;
                date = date.AddMonths(1);
            }

            debt -= u;

            command = new SqlCommand("SELECT List FROM Payments WHERE Contract_num = " + contract, connection);
            reader = command.ExecuteReader();
            reader.Read();
            str = reader.GetString(0);
            reader.Close();

            if (str == null || str == "")
            {
                command.CommandText = "UPDATE Main SET Debt = " + debt + "WHERE Contract_num = '" + contract + "'";
                command.ExecuteNonQuery();
                return debt;
            }

            String[] vs = str.Split(',');

            for (int i = 0; i < vs.Length; i++)
            {
                double d = Convert.ToDouble(vs[i].Split('_')[1]);
                debt -= d;
            }

            if (end != DateTime.Today && end.Day < 16)
                debt -= u / 2;

            command.CommandText = "UPDATE Main SET Debt = " + debt + "WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();

            return debt;
        }

        public void Change_FormClosing(object sender, FormClosingEventArgs e)
        {
            String str = dataGridViewInf.Rows[0].Cells[0].Value.ToString() + "_" + dataGridViewInf.Rows[0].Cells[1].Value.ToString();
            for (int i = 1; i < dataGridViewInf.Rows.Count; i++)
            {
                if (dataGridViewInf.Rows[i].Cells[0].Value == null || dataGridViewInf.Rows[i].Cells[0].Value.ToString() == "")
                    continue;
                str += "," + dataGridViewInf.Rows[i].Cells[0].Value.ToString() + "_" + dataGridViewInf.Rows[i].Cells[1].Value.ToString();
                count = Convert.ToInt32(dataGridViewInf.Rows[i].Cells[0].Value);
            }
            command.CommandText = "UPDATE Main SET Monthly_fee = '" + str + "' WHERE Contract_num = '" + contract + "'";
            command.ExecuteNonQuery();

            command.CommandText = "SELECT Monthly_fee FROM Main WHERE Contract_num = '" + contract + "'";
            SqlDataReader reader = command.ExecuteReader();
            double dd = DebtCalc(reader, contract);
            list.Items[0].SubItems[5].Text = dd.ToString();
            list.Items[0].SubItems[6].Text = count.ToString();
            Program.form.buttonChgAP.Enabled = true;
        }
    }
}