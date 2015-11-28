using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessApi.Models
{
    public class Move
    {
        public Coordinate From { get; set; }
        public Coordinate To { get; set; }

        // Used for castle-ing where two pieces are moved in one move.
        public Coordinate From2 { get; set; }
        public Coordinate To2 { get; set; }

        public Move(Coordinate from, Coordinate to)
        {
            From = from;
            To = to;
        }

        public Move(Coordinate from, Coordinate to, Coordinate from2, Coordinate to2) : this(from, to)
        {
            From2 = from2;
            To2 = to2;
        }

        public Move() { }
    }
}