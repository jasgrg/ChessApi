using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ChessApi.Models
{
    [DataContract]
    public class Coordinate
    {
        [DataMember]
        public short X { get; set; }
        [DataMember]
        public short Y { get; set; }

        public Coordinate(short x, short y)
        {
            this.X = x;
            this.Y = y;
        }

        public Coordinate(int x, int y) : this((short)x, (short)y) { }
        public Coordinate(short x, int y) : this(x, (short)y) { }
        public Coordinate(int x, short y) : this((short)x, y) { }

        public bool IsValid { get { return X >= 0 && X <= 7 && Y >= 0 && Y <= 7; } }

        public Coordinate() { }
    }
}