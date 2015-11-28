using ChessApi.Enums;
using ChessApi.Interfaces;
using System.Runtime.Serialization;

namespace ChessApi.Models
{
    [DataContract]
    public class Square
    {
        [DataMember]
        public Coordinate Location { get; set; }
        [DataMember]
        public Color Color { get; set; }

        private IPiece _piece;
        [DataMember]
        public IPiece Piece { get { return _piece; } set { _piece = value; } }

        public Square() { }
        public Square(short x, short y, IPiece piece)
        {
            this.Location = new Coordinate(x, y);
            this.Piece = piece;

            var isEven = ((y * 8) + x) % 2 == 0;
            if (Location.Y % 2 == 0)
            {
                this.Color = isEven ? Color.Black : Color.White;
            }
            else
            {
                this.Color = isEven ? Color.White : Color.Black;
            }
        }
    }
}