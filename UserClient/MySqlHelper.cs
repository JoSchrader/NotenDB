using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using UserClient.ViewModel;

namespace UserClient
{
    public class MySqlHelper
    {
        MySqlConnection connection;
        RefreshDelegate refreshAll;

        static MySqlHelper instance;

        private MySqlHelper()
        {
            this.refreshAll += new RefreshDelegate(delegate () { });
        }

        private static void Open()
        {
            instance.connection.Open();
        }

        private static void Close()
        {
            instance.connection.Close();
        }

        private static string GenerateOR(Dictionary<string, string> filters)
        {
            int filterAmount = 0;
            List<string> filterexpressions = new List<string>();

            foreach (KeyValuePair<string, string> filter in filters)
            {
                string[] splittedFilter = filter.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                filterAmount += splittedFilter.Length;

                foreach (string curFilter in splittedFilter)
                    filterexpressions.Add(filter.Key + " LIKE '%" + curFilter + "%'");
            }

            if (filterAmount > 0)
                return "where " + String.Join(" OR ", filterexpressions);
            else
                return "";
        }

        public static ObservableCollection<GenreListEntryVM> GetGenres()
        {
            Open();
            ObservableCollection<GenreListEntryVM> ret = new ObservableCollection<GenreListEntryVM>();

            string command = "Select genre_id, genre_name from genre;";

            MySqlCommand cmd = new MySqlCommand(command, instance.connection);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ret.Add(new GenreListEntryVM((int)rdr.GetUInt32(0), rdr.GetString(1)));
            }
            Close();

            Log.Add("Genre List Query Executed");
            return ret;
        }

        public static ObservableCollection<PieceListEntryVM> GetPieceList(string pieceTitle = "", string interpret = "", string genre = "")
        {
            Open();
            ObservableCollection<PieceListEntryVM> ret = new ObservableCollection<PieceListEntryVM>();

            string filterquery = "SELECT piece_id, piece_title ";
            // filterquery += "from interpret natural join Interpret_Piece natural join piece natural join Genre_Piece natural join genre ";
            filterquery += "from piece left join interpret_piece using (piece_id) left join interpret using(interpret_id) left join genre_piece using(piece_id) left join genre using(genre_id) ";
            filterquery += GenerateOR(new Dictionary<string, string>() { { "piece_title", pieceTitle }, { "interpret_name", interpret }, { "genre_name", genre } });

            string namingquery = "SELECT piece_id, GROUP_CONCAT(DISTINCT interpret_name ORDER BY interpret_name DESC SEPARATOR ', ') AS interpret_grp, GROUP_CONCAT(DISTINCT genre_name ORDER BY genre_name DESC SEPARATOR ', ') AS genre_grp ";
            namingquery += "from interpret natural join Interpret_Piece natural join piece natural join Genre_Piece natural join genre ";
            namingquery += "GROUP BY piece_id";

            string fullquery = "SELECT piece_id, piece_title, interpret_grp, genre_grp from (" + filterquery + ") AS filterquery left join (" + namingquery + ") AS namingquery using(piece_id) GROUP BY piece_id;";

            MySqlCommand cmd = new MySqlCommand(fullquery, instance.connection);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                object[] vals = rdr.GetAllValues();
                ret.Add(new PieceListEntryVM( (int)vals[0], (string)vals[1], (string)vals[2], (string)vals[3]));
            }
            Close();

            Log.Add("Piece List Query Executed");
            return ret;
        }

        public static ObservableCollection<EditPieceInterpretVM> GetInterpretsForPiece(int piece_id)
        {
            Open();
            ObservableCollection<EditPieceInterpretVM> ret = new ObservableCollection<EditPieceInterpretVM>();

            string query = "SELECT interpret_id, interpret_name, role from interpret natural join interpret_piece where piece_id=" + piece_id + ";";

            MySqlCommand cmd = new MySqlCommand(query, instance.connection);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                
                ret.Add(new EditPieceInterpretVM(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), piece_id));
            }
            Close();

            Log.Add("Interprets for piece Query Executed");
            return ret;
        }

        public static ObservableCollection<Genre> GetGenresForPiece(int piece_id)
        {
            Open();
            ObservableCollection<Genre> ret = new ObservableCollection<Genre>();

            string query = "SELECT genre_id, genre_name from genre natural join genre_piece where piece_id=" + piece_id + ";";

            MySqlCommand cmd = new MySqlCommand(query, instance.connection);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ret.Add(new Genre(rdr.GetInt32(0), rdr.GetString(1)));
            }
            Close();

            Log.Add("Genres for piece Query Executed");
            return ret;
        }

        public static ObservableCollection<EditPiecePartVM> GetPartsForPiece(int piece_id)
        {
            Open();
            ObservableCollection<EditPiecePartVM> ret = new ObservableCollection<EditPiecePartVM>();

            string query = "SELECT part_id, GROUP_CONCAT(DISTINCT instrument_name ORDER BY instrument_name DESC SEPARATOR ', ') AS instrument_grp";
            query+=   " from part natural join part_instrument natural join instrument where piece_id=" + piece_id + " group by part_id;";

            MySqlCommand cmd = new MySqlCommand(query, instance.connection);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                
                ret.Add(new EditPiecePartVM(rdr.GetInt32(0), rdr.GetString(1)));
            }
            Close();

            Log.Add("Genres for piece Query Executed");
            return ret;
        }


        public static int GetNextPrimaryKey(string tableName)
        {
            Open();
            int ret = -1;

            string query = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'world' AND TABLE_NAME = '"+tableName+"';";

            MySqlCommand cmd = new MySqlCommand(query, instance.connection);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {               
                ret = rdr.GetInt32(0);
            }
            Close();

            Log.Add("GetNextPrimaryKey Query Executed");
            return ret;

        }

        public static string[][] ExecuteQuery(string query)
        {
            Open();
            List<string[]> data = new List<string[]>();

            MySqlCommand cmd = new MySqlCommand(query, instance.connection);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string[] row = new string[rdr.FieldCount];
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    row[i] = rdr.GetString(i);
                }
                data.Add(row);
            }
            Close();

            Log.Add(query + "Executed");
            return data.ToArray();
        }

        public static ObservableCollection<InterpretListEntryVM> GetInterpretList()
        {
            Open();
            ObservableCollection<InterpretListEntryVM> ret = new ObservableCollection<InterpretListEntryVM>();

            string command = "Select interpret_id, interpret_name from interpret;";

            MySqlCommand cmd = new MySqlCommand(command, instance.connection);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ret.Add(new InterpretListEntryVM((int)rdr.GetUInt32(0), rdr.GetString(1)));
            }
            Close();

            Log.Add("Interpret List Query Executed");
            return ret;
        }

        public static void UpdateCMD(string command)
        {
            Open();
            MySqlCommand cmd = new MySqlCommand(command, instance.connection);
            cmd.ExecuteNonQuery();
            Close();
            RefreshAll();
            Log.Add("Update Executed");
        }

        public static void AddCMD(string command)
        {
            Open();
            MySqlCommand cmd = new MySqlCommand(command, instance.connection);
            cmd.ExecuteNonQuery();
            Close();
            RefreshAll();
            Log.Add("Add Executed");
        }

        public static void DeleteCMD(string command)
        {
            Open();
            MySqlCommand cmd = new MySqlCommand(command, instance.connection);
            cmd.ExecuteNonQuery();
            Close();
            RefreshAll();
            Log.Add("Delete Executed");
        }

        public static RefreshDelegate RefreshAll
        {
            get { return instance.refreshAll; }
            set { instance.refreshAll = value; }
        }

        public static void Setup(string server, uint port, string user, string password, string database)
        {
            MySqlHelper instance = new MySqlHelper();
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = server;
            builder.Port = port;
            builder.UserID = user;
            builder.Password = password;
            builder.Database = database;
            instance.connection = new MySqlConnection(builder.ConnectionString);

            MySqlHelper.instance = instance;
        }
    }

    public static class ExtensionMethods
    {
        public static object[] GetAllValues(this MySqlDataReader reader)
        {
            object[] array = new object[reader.FieldCount];
            reader.GetValues(array);
            for(int i=0;i<reader.FieldCount;i++)
            {
                if (array[i] is DBNull)
                    array[i] = null;
            }
            return array;
        }
    }
}
