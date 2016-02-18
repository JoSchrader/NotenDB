using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using UserClient.Helper;
using UserClient.Dialogs;
using System.Windows.Input;
using System.Windows;

namespace UserClient.ViewModel
{
    public class InterpretListVM : ViewModelBase
    {
        ObservableCollection<InterpretListEntryVM> interprets = new ObservableCollection<InterpretListEntryVM>();
        InterpretListEntryVM selectedInterpret;
        InterpretListEntryVM lastSelectedInterpret;        

        public InterpretListVM()
        {
            MySqlHelper.RefreshAll += Refresh;
            MouseDoubleClickCmd = new ActionCommand<MouseButtonEventArgs>(MouseDoubleClickCmdExecute);
            ShowPiecesCmd = new RelayCommand( x => ShowPiecesCmdExecute(x));
            NewInterpretCmd = new RelayCommand(x => NewInterpretCmdExecute());
            KeyUpCmd = new ActionCommand<KeyEventArgs>(KeyUpCmdExecute);
            DeleteInterpretCmd = new RelayCommand(x => DeleteInterpretCmdExecute());
        }

        public void DeleteInterpretCmdExecute()
        {
            if (selectedInterpret != null)
            {
                MySqlHelper.DeleteCMD("delete from interpret where interpret_id=" + selectedInterpret.ID + ";");
            }
        }

        public void KeyUpCmdExecute(KeyEventArgs args)
        {
            if(args.Key == Key.Delete)
            {
                DeleteInterpretCmdExecute();
            }
        }

        public void MouseDoubleClickCmdExecute(MouseButtonEventArgs args)
        {
            if (selectedInterpret != null && (args==null || args.ChangedButton == MouseButton.Left))
            {
                EditInterpretDialog dlg = new EditInterpretDialog();
                EditInterpretVM vm = new EditInterpretVM(selectedInterpret.ID, selectedInterpret.Name, true, false);
                dlg.DataContext = vm;
                dlg.ShowDialog();
            }
        }

        public void NewInterpretCmdExecute()
        {
            EditInterpretDialog dlg = new EditInterpretDialog();
            EditInterpretVM vm = new EditInterpretVM(null, "", false, true);
            dlg.DataContext = vm;
            dlg.ShowDialog();
        }

        public void ShowPiecesCmdExecute(object sender)
        {
            MainWindowVM.instance.PieceListVM.filterTitle = "";
            MainWindowVM.instance.PieceListVM.filterGenre = "";
            MainWindowVM.instance.PieceListVM.filterInterpret = lastSelectedInterpret.Name;
            MainWindowVM.instance.PieceListVM.UpdateFilterGUI();
            MainWindowVM.instance.PieceListVM.Refresh();
            MainWindowVM.instance.OpenPieces();
        }

        public ObservableCollection<InterpretListEntryVM> Interprets
        {
            get { return interprets; }
            set { interprets = value; this.NotifyOfPropertyChange(() => this.Interprets); }
        }

        public InterpretListEntryVM SelectedInterpret
        {
            get { return selectedInterpret; }
            set { selectedInterpret = value; this.NotifyOfPropertyChange(() => this.SelectedInterpret);  if (value != null) lastSelectedInterpret = value; }
        }

        public ICommand MouseDoubleClickCmd
        {
            get;
            private set;
        }

        public ICommand ShowPiecesCmd
        {
            get;
            private set;
        }

        public ICommand NewInterpretCmd
        {
            get;
            private set;
        }

        public ICommand KeyUpCmd
        {
            get;
            private set;
        }

        public ICommand DeleteInterpretCmd
        {
            get;
            private set;
        }

        void Refresh()
        {
            this.Interprets = MySqlHelper.GetInterpretList();
        }
    }

    public class InterpretListEntryVM : ViewModelBase
    {
        string name;
        int id;

        public InterpretListEntryVM(int id, string name)
        {
            this.name = name;
            this.id = id;
        }

        public string Name
        {
            get { return name; }
            set { name = value; this.NotifyOfPropertyChange(() => this.Name); }
        }

        public int ID
        {
            get { return id; }
            set { id = value; this.NotifyOfPropertyChange(() => this.ID); }
        }
    }
}
