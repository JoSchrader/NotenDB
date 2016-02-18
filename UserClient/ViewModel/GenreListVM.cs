using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UserClient.Helper;
using UserClient.Dialogs;

namespace UserClient.ViewModel
{

    public class GenreListVM : ViewModelBase
    {
        ObservableCollection<GenreListEntryVM> genres;
        GenreListEntryVM selectedGenre;


        public GenreListVM()
        {
            MySqlHelper.RefreshAll += Refresh;
            MouseDoubleClickCmd = new ActionCommand<MouseButtonEventArgs>(MouseDoubleClickCmdExecute);
            ShowPiecesCmd = new RelayCommand(x => ShowPiecesCmdExecute());
            NewGenreCmd = new RelayCommand(x => NewGenreCmdExecute());
            KeyUpCmd = new ActionCommand<KeyEventArgs>(KeyUpCmdExecute);
            DeleteGenreCmd = new RelayCommand(x => DeleteGenreCmdExecute());
        }

        public void KeyUpCmdExecute(KeyEventArgs args)
        {
            if (args.Key == Key.Delete)
            {
                DeleteGenreCmdExecute();
            }
        }

        public void DeleteGenreCmdExecute()
        {
            if (selectedGenre != null)
            {
                MySqlHelper.DeleteCMD("delete from genre where genre_name='" + selectedGenre.Name + "';");
            }
        }


        public void MouseDoubleClickCmdExecute(MouseButtonEventArgs args)
        {
            if (selectedGenre != null && (args == null || args.ChangedButton == MouseButton.Left))
            {
                EditGenreDialog dlg = new EditGenreDialog();
                EditGenreVM vm = new EditGenreVM(selectedGenre.Name);
                dlg.DataContext = vm;
                dlg.ShowDialog();
            }
        }

        public void NewGenreCmdExecute()
        {
            EditGenreDialog dlg = new EditGenreDialog();
            EditGenreVM vm = new EditGenreVM();
            dlg.DataContext = vm;
            dlg.ShowDialog();
        }

        public void ShowPiecesCmdExecute()
        {
            MainWindowVM.instance.PieceListVM.filterTitle = "";
            MainWindowVM.instance.PieceListVM.filterGenre = selectedGenre.Name;
            MainWindowVM.instance.PieceListVM.filterInterpret = "";
            MainWindowVM.instance.PieceListVM.UpdateFilterGUI();
            MainWindowVM.instance.PieceListVM.Refresh();
            MainWindowVM.instance.OpenPieces();
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

        public ICommand NewGenreCmd
        {
            get;
            private set;
        }

        public ICommand KeyUpCmd
        {
            get;
            private set;
        }

        public ICommand DeleteGenreCmd
        {
            get;
            private set;
        }

        public ObservableCollection<GenreListEntryVM> Genres
        {
            get { return genres; }
            set { genres = value; this.NotifyOfPropertyChange(() => this.Genres); }
        }

        public GenreListEntryVM SelectedGenre
        {
            get { return selectedGenre; }
            set { selectedGenre = value; this.NotifyOfPropertyChange(() => this.SelectedGenre); }
        }

        public void Refresh()
        {
            this.Genres = MySqlHelper.GetGenres();
        }
    }

    public class GenreListEntryVM : ViewModelBase
    {
        string name;
        int id;

        public GenreListEntryVM(int id, string name)
        {
            this.name = name;
            this.id = id;
        }

        public string Name
        {
            get { return name; }
            set { name = value; this.NotifyOfPropertyChange(() => this.Name); }
        }
    }
}
