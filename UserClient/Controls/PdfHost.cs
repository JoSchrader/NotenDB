using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserClient.Controls
{
    public partial class PdfHost : UserControl
    {
        public PdfHost()
        {
            InitializeComponent();
        }
        public PdfHost(string path)
        {
            InitializeComponent();
            LoadPdf(path);
        }

        public void LoadPdf(string path)
        {
            this.axAcroPDF1.LoadFile(path);
        }
    }
}
