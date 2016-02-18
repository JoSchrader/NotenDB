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
    class EditInterpretVM : ViewModelBase
    {
        int? id;
        string name;
        bool isEdit;
        bool isAdd;
        bool okClicked;
        bool cancelClicked;
        

        public EditInterpretVM(int? id, string name, bool isEdit, bool isAdd)
        {
            this.id = id;
            this.name = name;
            this.isEdit = isEdit;
            this.isAdd = isAdd;

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
                MySqlHelper.UpdateCMD("UPDATE Interpret SET interpret_name ='" + name + "' WHERE interpret_id='" + id + "'");
            }
            else if(isAdd && okClicked)
            {
                MySqlHelper.AddCMD("INSERT INTO Interpret(interpret_name) VALUES ('" + name + "')");
            }
        }

        public int? ID
        {
            get { return id; }
            set { id = value; this.NotifyOfPropertyChange(() => this.ID); }
        }

        public string Name
        {
            get { return name; }
            set { name = value; this.NotifyOfPropertyChange(() => this.Name); }
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
