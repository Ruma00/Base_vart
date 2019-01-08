using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Vart_Main
{
    public partial class TxtImport : Form
    {
        public TxtImport()
        {
            InitializeComponent();

            listViewImport.DoubleClick += new EventHandler(listViewImport_DoubleClick);

            using (StreamReader sr = new StreamReader("impErr.txt", Encoding.Default))
            {
                string line = "";
                ListViewItem item;
                string[] arr;

                while ((line = sr.ReadLine()) != null)
                {
                    arr = line.Split(' ');
                    item = new ListViewItem(new string[] { arr[0], arr[1], arr[2], arr[3] } );
                    listViewImport.Items.Add(item);
                }
            }
        }

        private void listViewImport_DoubleClick(object sender, EventArgs e)
        {
            if (listViewImport.SelectedItems.Count > 0)
                Clipboard.SetText(listViewImport.SelectedItems[0].SubItems[4].Text);
        }
    }
}
