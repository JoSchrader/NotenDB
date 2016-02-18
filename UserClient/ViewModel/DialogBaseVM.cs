using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UserClient.Helper;

namespace UserClient.ViewModel
{
    public abstract class DialogBaseVM : ViewModelBase
    {
        protected bool isEdit;
        protected bool isAdd;
        protected bool okClicked;
        protected bool cancelClicked;

        public DialogBaseVM(bool isEdit, bool isAdd)
        {
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

        public abstract void ClosingExecute(System.ComponentModel.CancelEventArgs e);

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
