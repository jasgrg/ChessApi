using ChessApi.Enums;
using ChessApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessApi.Models
{
    public class MoveResult
    {
        public MoveResultType Type { get; set; }
        public IPiece CapturedPiece { get; set; }
    }
}