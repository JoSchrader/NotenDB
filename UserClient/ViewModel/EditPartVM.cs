using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClient.Helper;
using System.IO;
using System.Windows.Input;
using UserClient.Dialogs;

namespace UserClient.ViewModel
{
    class EditPartVM : ViewModelBase
    {
        int id;
        int pieceID;
        string pieceTitle;
        InstrumentTree instrumentTree;
        string pdfPath;
        string comment;

        public StringDelegate loadPdf;

        public EditPartVM(int id)
        {
            this.id = id;
            loadPdf = new StringDelegate(delegate (string a) { });
        }

        public EditPartVM()
        {
            loadPdf = new StringDelegate(delegate (string a) { });
        }

        public void Setup()
        {
            SelectPdfCmd = new RelayCommand(x => SelectPdfCmdExecute());
            ClosingCmd = new ActionCommand<System.ComponentModel.CancelEventArgs>(ClosingExecute);
            MySqlHelper.RefreshAll += Refresh;
            Refresh();
        }

        private string PredictedPdfPath()
        {
            return Settings.pdfRoot + "\\" + pieceID.ToString("D6") + "\\" + this.id.ToString("D6") + ".pdf";
        }

        public void Refresh()
        {
            this.instrumentTree = new InstrumentTree(id);
            string[][] data = MySqlHelper.ExecuteQuery("select piece_id, part_comment from part where part_id=" + id + ";");
            pieceID = int.Parse(data[0][0]);
            comment = data[0][1];

            data = MySqlHelper.ExecuteQuery("select piece_title from piece where piece_id=" + pieceID + ";");
            PieceTitle = data[0][0];

            if (System.IO.File.Exists(PredictedPdfPath()))
                PdfPath = PredictedPdfPath();
            else
                PdfPath = null;

            this.NotifyOfPropertyChange(() => this.PieceTitle);
            this.NotifyOfPropertyChange(() => this.Comment);
            this.NotifyOfPropertyChange(() => this.InstrumentTree);
        }

        public void ClosingExecute(System.ComponentModel.CancelEventArgs e)
        {
            this.PdfPath = null;
            MySqlHelper.RefreshAll -= Refresh;
        }

        public void SelectPdfCmdExecute()
        {
            SelectPdfDialogVM vm = new SelectPdfDialogVM(this.id);
            SelectPdfDialog dlg = new SelectPdfDialog(ref vm.loadPdf);
            dlg.DataContext = vm;
            dlg.Show();
        }

        public InstrumentTree InstrumentTree
        {
            get { return instrumentTree; }
            set { instrumentTree = value; this.NotifyOfPropertyChange(() => this.InstrumentTree); }
        }

        public string PdfPath
        {
            get { return pdfPath; }

            set
            {
                pdfPath = value;
                this.NotifyOfPropertyChange(() => this.PdfPath); this.loadPdf(pdfPath);
            }
        }

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value; this.NotifyOfPropertyChange(() => this.Comment);
                MySqlHelper.UpdateCMD("UPDATE part set part_comment where part_id=" + id + ";");
            }
        }

        public string PieceTitle
        {
            get { return pieceTitle; }
            set { pieceTitle = value; }
        }

        public ICommand ClosingCmd
        {
            get;
            private set;
        }

        public ICommand SelectPdfCmd
        {
            get;
            private set;
        }

    }
}
