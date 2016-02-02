using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClient.Helper;
using System.Collections.ObjectModel;

namespace UserClient.ViewModel
{
    class MainWindowVM: ViewModelBase
    {
        ObservableCollection<PieceListEntryVM> pieceList;

        public ObservableCollection<PieceListEntryVM> PieceList
        {
            get
            {
                return pieceList;
            }
            set
            {
                pieceList = value;
                this.NotifyOfPropertyChange(() => this.PieceList);
            }
        }
    }
}
