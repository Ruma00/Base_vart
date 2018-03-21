using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Xps.Packaging;

namespace DB_Vart_Main
{
    public partial class ViewReceipts : Form
    {
        System.Windows.Controls.DocumentViewer viewer;
        public ViewReceipts()
        {
            InitializeComponent();
            viewer = new System.Windows.Controls.DocumentViewer();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            XpsDocument doc = new XpsDocument(dialog.FileName, FileAccess.Read);
            viewer.Document = doc.GetFixedDocumentSequence();
            elementHost1.Child = viewer;
        }

        private void ViewReceipts_Load(object sender, EventArgs e)
        {
            
        }
    }
}
