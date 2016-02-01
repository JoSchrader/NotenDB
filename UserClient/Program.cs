using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClient.Helper;
using UserClient.ViewModel;
using UserClient.View;
using MySql.Data.MySqlClient;

namespace UserClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MySqlConnectionStringBuilder myBuilder = new MySqlConnectionStringBuilder();
            myBuilder.Database = "world";
            myBuilder.Server = "localhost";
            myBuilder.Port = 3306;
            myBuilder.UserID = "client";
            myBuilder.Password = "1122";

            MySqlConnection myconn = new MySqlConnection(myBuilder.ConnectionString);
            myconn.Open();

            MySqlCommand command = new MySqlCommand("Select piece_title, interpret_name, genre_name, instrument_name from interpret natural join Interpret_Piece natural join piece natural join Piece_Genre natural join genre natural join part natural join part_instrument natural join instrument;", myconn);
            MySqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                string row = rdr.GetString(0) + ": " + rdr.GetString(1) + ": " + rdr.GetString(2) + ": " + rdr.GetString(3);
            }
            rdr.Close();
            myconn.Close();

            MainWindowView mwv = new MainWindowView();
            MainWindowVM mwvm = new MainWindowVM();
            mwv.DataContext = mwvm;

            mwv.ShowDialog();
        }
    }
}
