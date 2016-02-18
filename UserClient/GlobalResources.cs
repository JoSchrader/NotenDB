using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserClient
{

    public class GlobalResources
    {
        ObservableCollection<Interpret> interprets;
        ObservableCollection<Genre> genres;
        ObservableCollection<ArchiveType> archiveTypes;

        static GlobalResources instance;

        private GlobalResources()
        {
            interprets = new ObservableCollection<Interpret>();
            genres = new ObservableCollection<Genre>();
            archiveTypes = new ObservableCollection<ArchiveType>();
            MySqlHelper.RefreshAll += Refresh_Instance;
            Refresh_Instance();
        }

        public void Refresh_Instance()
        {
            interprets.Clear();
            genres.Clear();
            archiveTypes.Clear();

            foreach (string[] row in MySqlHelper.ExecuteQuery("select genre_id, genre_name from genre"))
            {
                genres.Add(new Genre(int.Parse(row[0]), row[1]));
            }

            foreach (string[] row in MySqlHelper.ExecuteQuery("select interpret_id, interpret_name from interpret"))
            {
                interprets.Add(new Interpret(int.Parse(row[0]), row[1]));
            }

            foreach (string[] row in MySqlHelper.ExecuteQuery("select archivetype_id, archivetype_name from archivetype"))
            {
                archiveTypes.Add(new ArchiveType(int.Parse(row[0]), row[1]));
            }
        }

        public static void Refresh()
        {
            instance.Refresh_Instance();
        }

        public static ObservableCollection<Interpret> Interprets
        {
            get { return instance.interprets; }
        }

        public static ObservableCollection<Genre> Genres
        {
            get { return instance.genres; }
        }

        public static ObservableCollection<ArchiveType> ArchiveTypes
        {
            get { return instance.archiveTypes; }
        }

        public static void Setup()
        {
            if (instance == null)
            {
                instance = new GlobalResources();
            }
        }
    }

    public abstract class DataBase : UserClient.Helper.ViewModelBase
    {
        readonly int id;
        string name;

        public int ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public DataBase(int id, string name)
        {
            this.id = id;
            this.Name = name;
        }
    }

    public class Interpret : DataBase
    {
        public Interpret(int id, string name) : base(id, name)
        {
        }
    }

    public class Genre : DataBase
    {
        public Genre(int id, string name) : base(id, name)
        {
        }
    }

    public class ArchiveType : DataBase
    {
        public ArchiveType(int id, string name) : base(id, name)
        {
        }
    }

    public class Instrument : DataBase
    {
        ObservableCollection<Instrument> childs;
        bool isSelected;
        bool isPlayable;
        bool isExpanded;
        int partID;


        public ObservableCollection<Instrument> Childs
        {
            get { return childs; }
            set { childs = value; this.NotifyOfPropertyChange(() => this.Childs); }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value; this.NotifyOfPropertyChange(() => this.IsSelected);
                if (isSelected == false)
                    MySqlHelper.DeleteCMD("Delete from part_instrument where instrument_id=" + this.ID + " and part_id=" + partID + ";");
                if (isSelected == true)
                    MySqlHelper.AddCMD("INSERT INTO Part_instrument VALUES("+partID+","+ID+");");
            }
        }

        public bool IsPlayable
        {
            get { return isPlayable; }
            set { isPlayable = value; this.NotifyOfPropertyChange(() => this.IsPlayable); }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set { isExpanded = value; this.NotifyOfPropertyChange(() => this.IsExpanded); }
        }

        public Instrument(int id, string name, bool isPlayable, int partID) : base(id, name)
        {
            this.isPlayable = isPlayable;
            this.partID = partID;
            Childs = new ObservableCollection<Instrument>();
            string[][] data = MySqlHelper.ExecuteQuery("select instrument_id, instrument_name, isPlayable from instrument where parent=" + id + ";");
            foreach (string[] row in data)
            {
                Childs.Add(new Instrument(int.Parse(row[0]), row[1], bool.Parse(row[2]), partID));
            }

            if (MySqlHelper.ExecuteQuery("Select * from part_instrument where part_id=" + partID + " and instrument_id=" + id + ";").Length > 0)
                isSelected = true;

            isExpanded = true;
        }
    }

    public class InstrumentTree : ObservableCollection<Instrument>
    {
        int partID;

        public InstrumentTree(int partID)
        {
            this.partID = partID;
            string[][] data = MySqlHelper.ExecuteQuery("select instrument_id, instrument_name, isPlayable from instrument where parent is null;");
            foreach (string[] row in data)
            {
                this.Add(new Instrument(int.Parse(row[0]), row[1], bool.Parse(row[2]), partID));
            }
        }
    }
}
