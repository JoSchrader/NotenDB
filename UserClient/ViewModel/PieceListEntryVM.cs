using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClient.Helper;

namespace UserClient.ViewModel
{
    class PieceListEntryVM : ViewModelBase
    {
        private int piece_id;
        private string piece_title;
        private string interpret_name;
        private string genre_name;
        private string instrument_name;

        public PieceListEntryVM()
        {
        }

        public PieceListEntryVM(int piece_id, string piece_title, string interpret_name, string genre_name, string instrument_name)
        {
            this.Piece_id = piece_id;
            this.Piece_title = piece_title;
            this.Interpret_name = interpret_name;
            this.Genre_name = genre_name;
            this.Instrument_name = instrument_name;
        }

        public int Piece_id
        {
            get { return piece_id; }
            set { piece_id = value; this.NotifyOfPropertyChange(() => this.Piece_id); }
        }

        public string Piece_title
        {
            get { return piece_title; }
            set { piece_title = value; this.NotifyOfPropertyChange(() => this.Piece_title); }
        }

        public string Interpret_name
        {
            get { return interpret_name; }
            set { interpret_name = value; this.NotifyOfPropertyChange(() => this.Interpret_name); }
        }

        public string Genre_name
        {
            get { return genre_name; }
            set { genre_name = value; this.NotifyOfPropertyChange(() => this.Genre_name); }
        }

        public string Instrument_name
        {
            get { return instrument_name; }
            set { instrument_name = value; this.NotifyOfPropertyChange(() => this.Instrument_name); }
        }
    }
}
