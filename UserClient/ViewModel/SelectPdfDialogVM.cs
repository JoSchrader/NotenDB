using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClient.Helper;
using System.IO;
using System.Windows.Input;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace UserClient.ViewModel
{
    class SelectPdfDialogVM : ViewModelBase
    {
        int partID;
        ObservableCollection<File> importFiles;
        File selectedImportFile;
        ObservableCollection<File> selectedFiles;
        File selectedSelectedFile;
        string previewPath;

        public StringDelegate loadPdf;

        public SelectPdfDialogVM(int partID)
        {
            this.partID = partID;
            Setup();
            this.loadPdf = new StringDelegate(delegate (string a) { });
        }

        public void Setup()
        {
            SelectedFiles = new ObservableCollection<File>();
            ImportCmd = new RelayCommand(x => ImportCmdExecute());
            OKCmd = new RelayCommand(x => OKCmdExecute());
            SelectedSelectedFileDownCmd = new RelayCommand(x => SelectedSelectedFileDownCmdExecute());
            SelectedSelectedFileUpCmd = new RelayCommand(x => SelectedSelectedFileUpCmdExecute());
            ImportFileMouseClickCmd = new ActionCommand<MouseButtonEventArgs>(ImportFileMouseClickCmdExecute);
            ClosingCmd = new ActionCommand<System.ComponentModel.CancelEventArgs>(ClosingExecute);
            MySqlHelper.RefreshAll += Refresh;
            Refresh();
        }

        public void Refresh()
        {
            ImportFiles = new ObservableCollection<File>();
            foreach (string file in System.IO.Directory.GetFiles(Settings.importFolder))
            {
                ImportFiles.Add(new File(file));
            }

            File[] files = selectedFiles.ToArray();
            foreach (File file in files)
            {
                if (!System.IO.File.Exists(file.Path))
                {
                    selectedFiles.Remove(file);
                }
            }
        }

        private string GetSavePath()
        {
            string[][] data = MySqlHelper.ExecuteQuery("select piece_id from part where part_id=" + this.partID + ";");
            int pieceID = int.Parse(data[0][0]);
            return Settings.pdfRoot + "\\" + pieceID.ToString("D6") + "\\" + partID.ToString("D6") + ".pdf";     
        }

        public void OKCmdExecute()
        {
            if (System.IO.File.Exists(GetSavePath()))
            {
                DialogResult result = MessageBox.Show("Exist, wanna replace?", "Exist, wanna replace?", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    System.IO.File.Delete(GetSavePath());
                }
                else if(result == DialogResult.No)
                {
                    return;
                }
            }
            
            List<PdfDocument> documents = new List<PdfDocument>();            
            PdfDocument outPdf = new PdfDocument();            
            try
            {
                foreach (File file in selectedFiles)
                {
                    PdfDocument pdf = PdfReader.Open(file.Path, PdfDocumentOpenMode.Import);
                    documents.Add(pdf);
                    CopyPages(pdf, outPdf);
                }
                Directory.CreateDirectory(Path.GetDirectoryName(GetSavePath()));
                outPdf.Save(GetSavePath());
            }
            catch (Exception e)
            {
                outPdf.Close();
                return;
            }
            
            outPdf.Close();

            MySqlHelper.RefreshAll();
        }

        void CopyPages(PdfDocument from, PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }

        public void ImportCmdExecute()
        {
            OpenFileDialog filedlg = new OpenFileDialog();
            filedlg.Multiselect = true;
            filedlg.Filter = "PDF Files (*.pdf)|*.pdf|All files (*.*)|*.*";
            DialogResult result = filedlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (string file in filedlg.FileNames)
                {
                    string toPath = Settings.importFolder + "\\" + System.IO.Path.GetFileName(file);
                    System.IO.File.Copy(file, toPath);
                }
            }
            Refresh();
        }

        public void SelectedSelectedFileUpCmdExecute()
        {
            if (selectedSelectedFile != null)
            {
                int index = selectedFiles.IndexOf(selectedSelectedFile);
                if (index > 0)
                {
                    File file = selectedFiles[index];
                    selectedFiles.RemoveAt(index);
                    SelectedFiles.Insert(index - 1, file);
                    SelectedSelectedFile = file;
                }
            }
        }

        public void SelectedSelectedFileDownCmdExecute()
        {
            if (selectedSelectedFile != null)
            {
                int index = selectedFiles.IndexOf(selectedSelectedFile);
                if (index < selectedFiles.Count - 1)
                {
                    File file = selectedFiles[index];
                    selectedFiles.RemoveAt(index);
                    SelectedFiles.Insert(index + 1, file);
                    SelectedSelectedFile = file;
                }
            }
        }

        public void ImportFileMouseClickCmdExecute(MouseButtonEventArgs e)
        {
            selectedFiles.Add(selectedImportFile.Copy());
        }

        public void ClosingExecute(System.ComponentModel.CancelEventArgs e)
        {
            MySqlHelper.RefreshAll -= Refresh;
        }

        public ICommand ClosingCmd
        {
            get;
            private set;
        }

        public ICommand ImportFileMouseClickCmd
        {
            get;
            private set;
        }

        public ICommand SelectedSelectedFileUpCmd
        {
            get;
            private set;
        }

        public ICommand SelectedSelectedFileDownCmd
        {
            get;
            private set;
        }

        public ICommand ImportCmd
        {
            get;
            private set;
        }

        public ICommand OKCmd
        {
            get;
            private set;
        }

        public ObservableCollection<File> ImportFiles
        {
            get { return importFiles; }
            set { importFiles = value; this.NotifyOfPropertyChange(() => this.ImportFiles); }
        }

        public File SelectedImportFile
        {
            get { return selectedImportFile; }
            set { selectedImportFile = value; this.NotifyOfPropertyChange(() => this.SelectedImportFile); if (value != null) PreviewPath = selectedImportFile.Path; }
        }

        public ObservableCollection<File> SelectedFiles
        {
            get { return selectedFiles; }
            set { selectedFiles = value; this.NotifyOfPropertyChange(() => this.SelectedFiles); }
        }

        public File SelectedSelectedFile
        {
            get { return selectedSelectedFile; }
            set { selectedSelectedFile = value; this.NotifyOfPropertyChange(() => this.SelectedSelectedFile); if (value != null) PreviewPath = selectedSelectedFile.Path; }
        }

        public string PreviewPath
        {
            get { return previewPath; }
            set { previewPath = value; this.NotifyOfPropertyChange(() => this.PreviewPath); this.loadPdf(previewPath); }
        }
    }

    public class File : ViewModelBase
    {
        string path;
        string displayPath;

        public File(string path)
        {
            this.path = path;
            this.displayPath = System.IO.Path.GetFileName(path);
        }

        public string DisplayPath
        {
            get { return displayPath; }
            set { displayPath = value; this.NotifyOfPropertyChange(() => this.DisplayPath); }
        }

        public string Path
        {
            get { return path; }
            set { path = value; this.NotifyOfPropertyChange(() => this.Path); }
        }

        public File Copy()
        {
            return new File(path);
        }
    }
}
