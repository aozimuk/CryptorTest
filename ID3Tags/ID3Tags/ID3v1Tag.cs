using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3Tags
{
    class ID3v1Tag
    {
        private const byte HEADERTAGLENGTH = 3;
        private const byte TITLETAGLENGTH = 30;
        private const byte ARTISTTAGLENGTH = 30;
        private const byte ALBUMTAGLENGTH = 30;
        private const byte YEARTAGLENGTH = 4;
        private const byte COMMENTTAGLENGTH = 30;
        private const byte GENRETAGLENGTH = 1;

        private string _header;
        private string _title;
        private string _artist;
        private string _album;
        private string _year;
        private string _comment;
        private string _genre;

        public ID3v1Tag()
        {
            _header = "";
            _title = "";
            _artist = "";
            _album = "";
            _year = "";
            _comment = "";
            _genre = "";
        }

        public string Header
        {
            get { return _header.TrimEnd('\0', ' ', '\a'); }
            set { _header = value; }
        }
        public string Title
        {
            get { return _title.TrimEnd('\0', ' ', '\a'); }
            set { _title = value; }
        }
    
        public string Artist
        {
            get { return _artist.TrimEnd('\0', ' ', '\a'); }
            set { _artist = value; }
        }
        public string Album
        {
            get { return _album.TrimEnd('\0', ' ', '\a'); }
            set { _album = value; }
        }
        public string Year
        {
            get { return _year.TrimEnd('\0', ' ', '\a'); }
            set { _year = value; } 
        }
        public string Comment
        {
            get { return _comment.TrimEnd('\0', ' ','\a'); }
            set { _comment = value; }
        }
        public string Genre
        {
            get { return _genre.TrimEnd('\0', ' ', '\a'); }
            set { _genre = value; }
        }
    }                                          
}                            
