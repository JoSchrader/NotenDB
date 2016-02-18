using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UserClient.Helper;
using System.Windows.Input;

namespace UserClient.ViewModel
{
    class EditGenreVM : ViewModelBase
    {
        string name;
        string newName;
        bool isEdit;
        bool isAdd;
        bool okClicked;
        bool cancelClicked;


        public EditGenreVM( string name)
        {
            this.name = name;
            this.isEdit = true;
            this.isAdd = false;

            this.OKCmd = new RelayCommand(OKCmdExecute);
            this.CancelCmd = new RelayCommand(CancelCmdExecute);
            this.EscapeCmd = new RelayCommand(EscapeExecute);
            this.ClosingCmd = new ActionCommand<System.ComponentModel.CancelEventArgs>(this.ClosingExecute);
        }

        public EditGenreVM()
        {
            this.isEdit = false;
            this.isAdd = true;

            this.OKCmd = new RelayCommand(OKCmdExecute);
            this.CancelCmd = new RelayCommand(CancelCmdExecute);
            this.EscapeCmd = new RelayCommand(EscapeExecute);
            this.ClosingCmd = new ActionCommand<System.ComponentModel.CancelEventArgs>(this.ClosingExecute);
        }

        private void OKCmdExecute(object arg)
        {
            this.okClicked = true;
            ((Window)arg).Close();
        }

        private void CancelCmdExecute(object arg)
        {
            this.cancelClicked = true;
            ((Window)arg).Close();
        }

        private void EscapeExecute(object arg)
        {
            ((Window)arg).Close();
        }

        private void ClosingExecute(System.ComponentModel.CancelEventArgs e)
        {
            if (isEdit && okClicked)
            {
                MySqlHelper.UpdateCMD("UPDATE Genre SET genre_name ='" + newName + "' WHERE genre_name='" + name + "'");
            }
            else if (isAdd && okClicked)
            {
                MySqlHelper.AddCMD("INSERT INTO genre(genre_name) VALUES ('" + newName + "')");
            }
        }
        
        public string NewName
        {
            get { return newName; }
            set { newName = value; this.NotifyOfPropertyChange(() => this.NewName); }
        }

        public ICommand OKCmd
        {
            get;
            private set;
        }

        public ICommand CancelCmd
        {
            get;
            private set;
        }

        public ICommand EscapeCmd
        {
            get;
            private set;
        }

        public ICommand ClosingCmd
        {
            get;
            private set;
        }
    }
}
