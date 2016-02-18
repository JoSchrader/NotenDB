using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UserClient.Helper;
using UserClient.Dialogs;
using System.ComponentModel;
using System.Windows.Input;

namespace UserClient.ViewModel
{
    public class Log : ViewModelBase
    {
        static Log instance;
        static LogView logview = null;

        string logtext = "";
        int count = 0;
        Visibility visibility;
        ActionCommand<CancelEventArgs> closingCmd;
        ActionCommand<KeyEventArgs> keyDownCmd;

        private Log()
        {
            Visibility = Visibility.Hidden;
            closingCmd = new ActionCommand<CancelEventArgs>(ClosingCmdExecute);
            keyDownCmd = new ActionCommand<KeyEventArgs>(keyDownCmdExecute);
        }

        private void ClosingCmdExecute(CancelEventArgs args)
        {
            args.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        public void keyDownCmdExecute(KeyEventArgs args)
        {
            if (args.Key == Key.F9)
            {
                Log.SetupView();
                Log.ToogleVisibility();
            }
        }

        public static void ToogleVisibility()
        {
            if (Instance.Visibility == Visibility.Hidden)
                Instance.Visibility = Visibility.Visible;
            else
                Instance.Visibility = Visibility.Hidden;
        }
        public static void Add(string text)
        {
            Instance.Logtext += instance.count++ + "  " + text + "\r\n";
        }

        public string Logtext
        {
            get { return logtext; }
            set { logtext = value; this.NotifyOfPropertyChange(() => this.Logtext); }
        }

        public Visibility Visibility
        {
            get { return visibility; }
            set { visibility = value; this.NotifyOfPropertyChange(() => this.Visibility); }
        }

        public ActionCommand<CancelEventArgs> ClosingCmd
        {
            get { return closingCmd; }
        }

        public ActionCommand<KeyEventArgs> KeyDownCmd
        {
            get { return keyDownCmd; }
        }

        public static void SetupView()
        {
            if (logview == null)
            {
                logview = new LogView();
                logview.DataContext = Log.instance;
                logview.Show();
            }
        }

        public static Log Instance
        {
            get
            {
                if (instance == null)
                {
                    Log.instance = new Log();
                    Log.Add("Log initialized");
                }
                return instance;
            }
        }
    }
}
