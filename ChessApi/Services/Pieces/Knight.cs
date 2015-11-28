using ChessApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessApi.Enums;
using ChessApi.Models;

namespace ChessApi.Services.Pieces
{
    public class Knight : Piece, IPiece
    {
        public Knight(Coordinate location, Player player) : base(location, player, PieceType.Knight) { }

        public IEnumerable<Move> GetPossibleMoves(Board board)
        {
            var result = new List<Move>();

            TryMoveToSquare(board, result, new Move(Location, new Coordinate(Location.X - 1, Location.Y + 2)), MoveType.CanAttack);
            TryMoveToSquare(board, result, new Move(Location, new Coordinate(Location.X + 1, Location.Y + 2)), MoveType.CanAttack);

            TryMoveToSquare(board, result, new Move(Location, new Coordinate(Location.X - 1, Location.Y - 2)), MoveType.CanAttack);
            TryMoveToSquare(board, result, new Move(Location, new Coordinate(Location.X + 1, Location.Y - 2)), MoveType.CanAttack);

            TryMoveToSquare(board, result, new Move(Location, new Coordinate(Location.X - 2, Location.Y + 1)), MoveType.CanAttack);
            TryMoveToSquare(board, result, new Move(Location, new Coordinate(Location.X - 2, Location.Y - 1)), MoveType.CanAttack);

            TryMoveToSquare(board, result, new Move(Location, new Coordinate(Location.X + 2, Location.Y + 1)), MoveType.CanAttack);
            TryMoveToSquare(board, result, new Move(Location, new Coordinate(Location.X + 2, Location.Y - 1)), MoveType.CanAttack);
            return result;
        }
    }
}