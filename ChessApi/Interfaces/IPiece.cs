using ChessApi.Enums;
using ChessApi.Models;
using System;
using System.Collections.Generic;

namespace ChessApi.Interfaces
{
    public interface IPiece
    {
        PieceType PieceType { get; }
        Boolean HasMoved { get; set; }
        Coordinate Location { get; set; }
        Player Player { get; }
        IEnumerable<Move> GetPossibleMoves(Board board);
        short Rank { get; }
    }
}
