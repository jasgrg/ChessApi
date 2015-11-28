using ChessApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessApi.Enums;
using ChessApi.Models;

namespace ChessApi.Services.Pieces
{
    public class Rook : Piece, IPiece
    {
        public Rook(Coordinate location, Player player) : base(location, player, PieceType.Rook) { }

        public IEnumerable<Move> GetPossibleMoves(Board board)
        {
            var result = new List<Move>();
            //straight forward
            TryMoveAlongSlope(board, result, new Move(Location, new Coordinate(0, 1)));
            // straight left
            TryMoveAlongSlope(board, result, new Move(Location, new Coordinate(-1, 0)));
            //straight right
            TryMoveAlongSlope(board, result, new Move(Location, new Coordinate(1, 0)));
            //straigh back
            TryMoveAlongSlope(board, result, new Move(Location, new Coordinate(0, -1)));
            return result;
        }
    }
}