using ChessApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessApi.Enums;
using ChessApi.Models;

namespace ChessApi.Services.Pieces
{
    public class Queen : Piece, IPiece
    {
        public Queen(Coordinate location, Player player) : base(location, player, PieceType.Queen) { }

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