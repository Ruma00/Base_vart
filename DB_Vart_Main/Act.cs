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
    public partial class Act : Form
    {
        public Act(string contract, string name, SqlConnection connection)
        {
            InitializeComponent();
            labelTitle.Text = labelTitle.Text + name + "л/с: " + contract;
            See(connection);
        }

        void See(SqlConnection connection)
        {
            //
        }
    }
}
