using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessApi.Models
{
    public class MoveResponse
    {
        public Move Move { get; set; }
        public Board Board { get; set; }
    }
}