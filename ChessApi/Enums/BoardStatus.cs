using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessApi.Enums
{
    public enum BoardStatus
    {
        Normal,
        Check,
        Checkmate,
        Stalemate
    }
}