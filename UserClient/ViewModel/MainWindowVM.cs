using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClient.Helper;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Controls;
using UserClient.Dialogs;

namespace UserClient.ViewModel
{
    class MainWindowVM : ViewModelBase
    {
        int selectedTab;

        PieceListVM pieceListVM;
        InterpretListVM interpretListVM;
        GenreListVM genreListVM;

        public static MainWindowVM instance;

        public MainWindowVM()
        {
            pieceListVM = new PieceListVM();
            interpretListVM = new InterpretListVM();
            GenreListVM = new GenreListVM();
            KeyDownCmd = new ActionCommand<KeyEventArgs>(this.KeyDownCmdExecute);
            NewInterpretCmd = new RelayCommand(x => NewInterpretCmdExecute());
            instance = this;
        }

        private void KeyDownCmdExecute(KeyEventArgs args)
        {
            if (args.Key == Key.F9)
            {
                Log.SetupView();
                Log.ToogleVisibility();
            }

            if (args.Key == Key.F5)
            {
                MySqlHelper.RefreshAll();
            }
        }

        private void NewInterpretCmdExecute()
        {
            EditInterpretDialog dlg = new EditInterpretDialog();
            EditInterpretVM vm = new EditInterpretVM(null, "", false, true);
            dlg.DataContext = vm;
            dlg.ShowDialog();
        }

        public void OpenPieces()
        {
            this.SelectedTab = 0;
        }

        public void OpenInterpret()
        {
            this.SelectedTab = 3;
        }

        public PieceListVM PieceListVM
        {
            get { return pieceListVM; }
            set { pieceListVM = value; this.NotifyOfPropertyChange(() => this.PieceListVM); }
        }

        public InterpretListVM InterpretListVM
        {
            get { return interpretListVM; }
            set { interpretListVM = value; this.NotifyOfPropertyChange(() => this.InterpretListVM); }
        }

        public GenreListVM GenreListVM
        {
            get { return genreListVM; }
            set { genreListVM = value; this.NotifyOfPropertyChange(() => this.GenreListVM); }
        }

        public ICommand KeyDownCmd
        {
            get;
            private set;
        }

        public ICommand NewInterpretCmd
        {
            get;
            private set;
        }

        public int SelectedTab
        {
            get { return selectedTab; }
            set { selectedTab = value; this.NotifyOfPropertyChange(() => this.SelectedTab); }
        }
    }
}
