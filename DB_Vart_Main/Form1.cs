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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Base;Integrated Security=true";
            sqlConnection.Open();
        }

        string[] sqlExpressions = new string[] { "SELECT * FROM Main", "UPDATE Main SET ", "INSERT INTO Main VALUES ", "WHERE Contract_num=" };

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
