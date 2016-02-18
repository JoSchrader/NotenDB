using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClient.Helper;
using UserClient.ViewModel;
using UserClient.View;
using MySql.Data.MySqlClient;
using UserClient.Dialogs;
using System.Threading;
using System.Windows.Threading;

namespace UserClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Settings.Load();
            MySqlHelper.Setup("localhost", 3306, "root", "1122", "world");
            GlobalResources.Setup();

            MainWindowView mwv = new MainWindowView();
            MainWindowVM mwvm = new MainWindowVM();
            mwv.DataContext = mwvm;

            MySqlHelper.RefreshAll();
            mwv.ShowDialog();

            Settings.Save();
            Environment.Exit(0);
        }
    }
}
