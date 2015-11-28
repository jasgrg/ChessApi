using ChessApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessApi.Enums;
using ChessApi.Models;

namespace ChessApi.Services.Pieces
{
    public class Bishop : Piece, IPiece
    {
        public Bishop(Coordinate location, Player player) : base(location, player, PieceType.Bishop) { }

        public IEnumerable<Move> GetPossibleMoves(Board board)
        {
            var result = new List<Move>();
            //diag forward left;
            TryMoveAlongSlope(board, result, new Move(Location, new Coordinate(-1, 1)));
            //diag forward right;
            TryMoveAlongSlope(board, result, new Move(Location, new Coordinate(1, 1)));
            //diag backward left;
            TryMoveAlongSlope(board, result, new Move(Location, new Coordinate(-1, -1)));
            //diag backward right;
            TryMoveAlongSlope(board, result, new Move(Location, new Coordinate(1, -1)));
            return result;
        }
    }
}