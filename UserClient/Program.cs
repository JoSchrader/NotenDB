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
            MySqlHelper helper = new MySqlHelper("localhost", 3306, "client", "1122", "world");
            helper.Open();
            var list = helper.GetPieceList();
            helper.Close();

            MainWindowView mwv = new MainWindowView();
            MainWindowVM mwvm = new MainWindowVM();
            mwvm.PieceList = list;
            mwv.DataContext = mwvm;

            mwv.ShowDialog();
        }
    }
}
