using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UserClient.Helper;
using UserClient.Dialogs;

namespace UserClient.ViewModel
{
    public class EditPieceVM : ViewModelBase
    {
        int id;
        string title;
        string comment;

        ObservableCollection<EditPieceInterpretVM> interprets;
        EditPieceInterpretVM selectedInterpret;

        ObservableCollection<Genre> genres;
        Genre selectedGenre;

        ObservableCollection<EditPiecePartVM> parts;
        EditPiecePartVM selectedPart;

        int selectedArchiveType;
        string archiveNumber;

        string selectedGenreValue;
        Genre selectedGenreItem;

        string selectedInterpretValue;
        Interpret selectedInterpretItem;

        public EditPieceVM(int id)
        {
            this.id = id;
            this.Setup();
        }

        public EditPieceVM() 
        {
            id = MySqlHelper.GetNextPrimaryKey("piece");
            title = "Neues Stück";
            MySqlHelper.AddCMD("INSERT INTO Piece(piece_id, piece_title) VALUES(" + id + ", '" + title + "');");
            this.Setup();
        }

        private void Setup()
        {
            AddGenreCmd = new RelayCommand(x => AddGenreCmdExecute());
            AddInterpretCmd = new RelayCommand(x => AddInterpretCmdExecute());
            DeleteGenreCmd = new RelayCommand(x => DeleteGenreCmdExecute());
            DeleteInterpretCmd = new RelayCommand(x => DeleteInterpretCmdExecute());
            ClosingCmd = new ActionCommand<System.ComponentModel.CancelEventArgs>(ClosingExecute);
            EditPartCmd = new RelayCommand(x => EditPartCmdExecute());
            MySqlHelper.RefreshAll += Refresh;
            Refresh();
        }

        private void EditPartCmdExecute()
        {
            if (selectedPart != null)
            {
                EditPartVM vm = new EditPartVM(selectedPart.ID);
                EditPartDialog view = new EditPartDialog(ref vm.loadPdf);
                view.DataContext = vm;
                vm.Setup();
                view.Show();
            }
        }

        private void AddGenreCmdExecute()
        {
            if (selectedGenreItem != null)
            {
                MySqlHelper.AddCMD("INSERT INTO Genre_Piece(genre_id,piece_id) VALUES(" + selectedGenreItem.ID + "," + this.id + ");");
            }
            else if (!string.IsNullOrWhiteSpace(selectedGenreValue))
            {
                MessageBoxResult result = MessageBox.Show("Das Genre '" + selectedGenreValue + "' gibt es nocht nicht.\r\n'" + selectedGenreValue + "' erstellen?", "Genre erstellen?", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    int newID = MySqlHelper.GetNextPrimaryKey("genre");
                    MySqlHelper.AddCMD("INSERT INTO Genre(genre_id, genre_name) VALUES(" + newID + ",'" + selectedGenreValue + "');");
                    MySqlHelper.AddCMD("INSERT INTO Genre_Piece(genre_id,piece_id) VALUES(" + newID + "," + this.id + ");");

                    GlobalResources.Refresh();
                }
            }
            this.Refresh();
        }

        private void AddInterpretCmdExecute()
        {
            if (selectedInterpretItem != null)
            {
                MySqlHelper.AddCMD("INSERT INTO Interpret_Piece(interpret_id, piece_id) VALUES(" + SelectedInterpretItem.ID + "," + this.id + ");");
            }
            else if (!string.IsNullOrWhiteSpace(selectedInterpretValue))
            {
                MessageBoxResult result = MessageBox.Show("Den Interpret '" + selectedInterpretValue + "' gibt es nocht nicht.\r\n'" + selectedInterpretValue + "' erstellen?", "Interpret erstellen?", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    int newID = MySqlHelper.GetNextPrimaryKey("interpret");
                    MySqlHelper.AddCMD("INSERT INTO Interpret(interpret_id, interpret_name) VALUES(" + newID + ",'" + selectedInterpretValue + "');");
                    MySqlHelper.AddCMD("INSERT INTO Interpret_Piece(interpret_id, piece_id) VALUES(" + newID + "," + this.id + ");");
                    GlobalResources.Refresh();
                }
            }
            this.Refresh();
        }

        private void DeleteGenreCmdExecute()
        {
            if (selectedGenre != null)
            {
                MySqlHelper.DeleteCMD("DELETE from genre_piece where genre_id=" + selectedGenre.ID + " and piece_id=" + this.id + ";");
                this.Refresh();
            }
        }

        private void DeleteInterpretCmdExecute()
        {
            if (selectedInterpret != null)
            {
                MySqlHelper.DeleteCMD("DELETE from interpret_piece where interpret_id=" + selectedInterpret.ID + " and piece_id=" + this.id + ";");
                this.Refresh();
            }

        }

        public void ClosingExecute(System.ComponentModel.CancelEventArgs e)
        {
            MySqlHelper.RefreshAll -= Refresh;
        }

        public int ID
        {
            get { return id; }
            set { id = value; this.NotifyOfPropertyChange(() => this.ID); }
        }

        public ObservableCollection<EditPieceInterpretVM> Interprets
        {
            get { return interprets; }
            set { interprets = value; this.NotifyOfPropertyChange(() => this.Interprets); }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value.Trim(); this.NotifyOfPropertyChange(() => this.Title);
                MySqlHelper.UpdateCMD("UPDATE piece set piece_title='" + title + "' where piece_id=" + id + ";");
            }
        }

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value; this.NotifyOfPropertyChange(() => this.Comment);
                MySqlHelper.UpdateCMD("UPDATE piece set piece_comment='" + comment + "' where piece_id=" + id + ";");
            }
        }

        public EditPieceInterpretVM SelectedInterpret
        {
            get { return selectedInterpret; }
            set { selectedInterpret = value; this.NotifyOfPropertyChange(() => this.SelectedInterpret); }
        }

        public ObservableCollection<Genre> Genres
        {
            get { return genres; }
            set { genres = value; this.NotifyOfPropertyChange(() => this.Genres); }
        }

        public Genre SelectedGenre
        {
            get { return selectedGenre; }
            set { selectedGenre = value; this.NotifyOfPropertyChange(() => this.SelectedGenre); }
        }

        public ObservableCollection<EditPiecePartVM> Parts
        {
            get { return parts; }
            set { parts = value; this.NotifyOfPropertyChange(() => this.Parts); }
        }

        public EditPiecePartVM SelectedPart
        {
            get { return selectedPart; }
            set { selectedPart = value; this.NotifyOfPropertyChange(() => this.SelectedPart); }
        }

        public ObservableCollection<ArchiveType> ArchiveTypes
        {
            get { return GlobalResources.ArchiveTypes; }
        }

        public int SelectedArchiveType
        {
            get { return selectedArchiveType; }
            set
            {
                selectedArchiveType = value; this.NotifyOfPropertyChange(() => this.SelectedArchiveType);
                MySqlHelper.UpdateCMD("UPDATE piece set archivetype_id='" + selectedArchiveType + "' where piece_id=" + id + ";");
            }
        }

        public string ArchiveNumber
        {
            get { return archiveNumber; }
            set
            {
                int number = 0;
                if (int.TryParse(value, out number) && number >= 0)
                {
                    archiveNumber = value;
                    MySqlHelper.UpdateCMD("UPDATE piece set archive_number='" + number + "' where piece_id=" + id + ";");
                }
                this.NotifyOfPropertyChange(() => this.ArchiveNumber);
            }
        }

        public ObservableCollection<Genre> AllGenres
        {
            get { return GlobalResources.Genres; }
        }

        public ObservableCollection<Interpret> AllInterprets
        {
            get { return GlobalResources.Interprets; }
        }

        public string SelectedGenreValue
        {
            get { return selectedGenreValue; }
            set { selectedGenreValue = value; }
        }

        public Genre SelectedGenreItem
        {
            get { return selectedGenreItem; }
            set { selectedGenreItem = value; }
        }

        public string SelectedInterpretValue
        {
            get { return selectedInterpretValue; }
            set { selectedInterpretValue = value; }
        }

        public Interpret SelectedInterpretItem
        {
            get { return selectedInterpretItem; }
            set { selectedInterpretItem = value; }
        }

        public ICommand AddGenreCmd
        {
            get;
            private set;
        }

        public ICommand AddInterpretCmd
        {
            get;
            private set;
        }

        public ICommand DeleteGenreCmd
        {
            get;
            private set;
        }

        public ICommand DeleteInterpretCmd
        {
            get;
            private set;
        }

        public ICommand ClosingCmd
        {
            get;
            private set;
        }

        public ICommand EditPartCmd
        {
            get;
            private set;
        }

        public void Refresh()
        {
            string[][] data = MySqlHelper.ExecuteQuery("Select piece_title, piece_comment, archivetype_id, archive_number from piece natural join archivetype where piece_id=" + id + ";");
            this.title = data[0][0];
            this.comment = data[0][1];
            this.selectedArchiveType = int.Parse(data[0][2]);
            this.archiveNumber = data[0][3];
            this.NotifyOfPropertyChange(() => this.Title);
            this.NotifyOfPropertyChange(() => this.Comment);
            this.NotifyOfPropertyChange(() => this.SelectedArchiveType);
            this.NotifyOfPropertyChange(() => this.ArchiveNumber);

            this.Interprets = MySqlHelper.GetInterpretsForPiece(this.id);
            this.SelectedInterpret = null;
            this.Genres = MySqlHelper.GetGenresForPiece(this.id);
            this.SelectedGenre = null;
            this.Parts = MySqlHelper.GetPartsForPiece(this.id);
            this.SelectedPart = null;
        }
    }

    public class EditPiecePartVM : ViewModelBase
    {
        int id;
        string instrument;
        string comment;

        public EditPiecePartVM(int id, string instrument)
        {
            this.ID = id;
            this.Instrument = instrument;
        }

        public int ID
        {
            get { return id; }
            set { id = value; this.NotifyOfPropertyChange(() => this.ID); }
        }

        public string Instrument
        {
            get { return instrument; }
            set { instrument = value; this.NotifyOfPropertyChange(() => this.Instrument); }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; this.NotifyOfPropertyChange(() => this.Comment); }
        }
    }

    public class EditPieceInterpretVM : ViewModelBase
    {
        int pieceID;
        int id;
        string name;
        string role;

        public EditPieceInterpretVM(int id, string name, string role, int pieceID)
        {
            this.id = id;
            this.name = name;
            this.role = role;
            this.pieceID = pieceID;
        }

        public int ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Role
        {
            get { return role; }
            set
            {
                role = value; this.NotifyOfPropertyChange(() => this.Role);
                MySqlHelper.UpdateCMD("UPDATE interpret_piece set role='" + role + "' where piece_id=" + pieceID + " and interpret_id=" + id + ";");
            }
        }
    }
}
