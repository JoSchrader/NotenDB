using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClient.Helper;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UserClient.Dialogs;

namespace UserClient.ViewModel
{
    public class PieceListVM : ViewModelBase
    {
        ObservableCollection<PieceListEntryVM> pieces;
        PieceListEntryVM selectedPiece;

        public string filterTitle;
        public string filterInterpret;
        public string filterGenre;

        public PieceListVM()
        {
            filterTitle = "";
            filterInterpret = "";
            filterGenre = "";

            MouseDoubleClickCmd = new ActionCommand<MouseButtonEventArgs>(MouseDoubleClickCmdExecute);
            NewPieceCmd = new RelayCommand(x => NewPieceCmdExecute());
            KeyUpCmd = new ActionCommand<KeyEventArgs>(KeyUpCmdExecute);
            DeletePieceCmd = new RelayCommand(x => DeletePieceCmdExecute());

            MySqlHelper.RefreshAll += Refresh;
        }

        public void DeletePieceCmdExecute()
        {
          /*  if (selectedInterpret != null)
            {
                MySqlHelper.DeleteCMD("delete from interpret where interpret_id=" + selectedInterpret.ID + ";");
            }*/
        }

        public void KeyUpCmdExecute(KeyEventArgs args)
        {
            if (args.Key == Key.Delete)
            {
                DeletePieceCmdExecute();
            }
        }

        public void MouseDoubleClickCmdExecute(MouseButtonEventArgs args)
        {
            if (selectedPiece != null && (args == null || args.ChangedButton == MouseButton.Left))
            {
                EditPieceDialog dlg = new EditPieceDialog();
                EditPieceVM vm = new EditPieceVM(selectedPiece.ID);
                dlg.DataContext = vm;
                dlg.Show();
            }
        }

        public void NewPieceCmdExecute()
        {
            EditPieceDialog dlg = new EditPieceDialog();
            EditPieceVM vm = new EditPieceVM();
            dlg.DataContext = vm;
            dlg.Show();
        }

        public ObservableCollection<PieceListEntryVM> Pieces
        {
            get { return pieces; }
            set { pieces = value; this.NotifyOfPropertyChange(() => this.Pieces); }
        }

        public string FilterTitle
        {
            get { return filterTitle; }
            set { filterTitle = value; this.NotifyOfPropertyChange(() => this.FilterTitle); Refresh(); }
        }

        public string FilterInterpret
        {
            get { return filterInterpret; }
            set { filterInterpret = value; this.NotifyOfPropertyChange(() => this.FilterInterpret); Refresh(); }
        }

        public string FilterGenre
        {
            get { return filterGenre; }
            set { filterGenre = value; this.NotifyOfPropertyChange(() => this.FilterGenre); Refresh(); }
        }

        public void UpdateFilterGUI()
        {
            this.NotifyOfPropertyChange(() => this.FilterTitle);
            this.NotifyOfPropertyChange(() => this.FilterInterpret);
            this.NotifyOfPropertyChange(() => this.FilterGenre);
        }

        public ICommand MouseDoubleClickCmd
        {
            get;
            private set;
        }

        public ICommand NewPieceCmd
        {
            get;
            private set;
        }

        public ICommand KeyUpCmd
        {
            get;
            private set;
        }

        public ICommand DeletePieceCmd
        {
            get;
            private set;
        }

        public PieceListEntryVM SelectedPiece
        {
            get { return selectedPiece; }
            set { selectedPiece = value; this.NotifyOfPropertyChange(() => this.SelectedPiece); }
        }

        public void Refresh()
        {
            Pieces = MySqlHelper.GetPieceList(FilterTitle, FilterInterpret, FilterGenre);
        }
    }

    public class PieceListEntryVM : ViewModelBase
    {
        private int id;
        private string piece_title;
        private string interpret_name;
        private string genre_name;

        public PieceListEntryVM()
        {
        }

        public PieceListEntryVM(int id, string piece_title, string interpret_name, string genre_name)
        {
            this.id = id;
            this.piece_title = piece_title;
            this.interpret_name = interpret_name;
            this.genre_name = genre_name;
        }

        public int ID
        {
            get { return id; }
            //set { piece_id = value; this.NotifyOfPropertyChange(() => this.Piece_id); }
        }

        public string Piece_title
        {
            get { return piece_title; }
            set { piece_title = value; this.NotifyOfPropertyChange(() => this.Piece_title); }
        }

        public string Interpret_name
        {
            get { return interpret_name; }
            set { interpret_name = value; this.NotifyOfPropertyChange(() => this.Interpret_name); }
        }

        public string Genre_name
        {
            get { return genre_name; }
            set { genre_name = value; this.NotifyOfPropertyChange(() => this.Genre_name); }
        }
    }
}
