using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserClient.Controls;

namespace UserClient.Dialogs
{
    /// <summary>
    /// Interaction logic for SelectPdfDialog.xaml
    /// </summary>
    public partial class SelectPdfDialog : Window
    {
        public SelectPdfDialog(ref StringDelegate loadPdfDelegate)
        {
            InitializeComponent();

            var uc = new PdfHost();
            this.WindowsFormHost.Child = uc;            

            loadPdfDelegate += uc.LoadPdf;
        }        
    }
}
