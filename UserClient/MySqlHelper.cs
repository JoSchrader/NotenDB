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
    class MySqlHelper
    {
        MySqlConnection connection;

        public MySqlHelper(string server, uint port, string user, string password, string database)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = server;
            builder.Port = port;
            builder.UserID = user;
            builder.Password = password;
            builder.Database = database;
            connection = new MySqlConnection(builder.ConnectionString);
        }

        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        public ObservableCollection<PieceListEntryVM> GetPieceList()
        {
            ObservableCollection<PieceListEntryVM> ret = new ObservableCollection<PieceListEntryVM>();

            MySqlCommand command = new MySqlCommand("Select piece_id, piece_title, interpret_name, genre_name, instrument_name from interpret natural join Interpret_Piece natural join piece natural join Piece_Genre natural join genre natural join part natural join part_instrument natural join instrument;", connection);
            MySqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                ret.Add(new PieceListEntryVM(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4)));
            }

            return ret;
        }
    }
}
