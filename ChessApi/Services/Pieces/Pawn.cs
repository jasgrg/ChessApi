using ChessApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessApi.Enums;
using ChessApi.Models;

namespace ChessApi.Services.Pieces
{
    public class Pawn : Piece, IPiece
    {

        public Pawn(Coordinate location, Player player) : base(location, player, PieceType.Pawn) { }

        public IEnumerable<Move> GetPossibleMoves(Board board)
        {
            var result = new List<Move>();

            if (TryMoveToSquare(board, result, new Move(this.Location, new Coordinate(Location.X, Location.Y + Direction)), MoveType.CannotAttack).Type == MoveResultType.Move) {
                if ((Player == Player.Light && Location.Y == 6)|| (Player == Player.Dark && Location.Y == 1))
                {
                    TryMoveToSquare(board, result, new Move(this.Location, new Coordinate(Location.X, Location.Y + (2 * Direction))), MoveType.CannotAttack);
                }
            }
            TryMoveToSquare(board, result, new Move(this.Location, new Coordinate(Location.X - 1, Location.Y + Direction)), MoveType.MustAttack);
            TryMoveToSquare(board, result, new Move(this.Location, new Coordinate(Location.X + 1, Location.Y + Direction)), MoveType.MustAttack);

            return result;
        }
    }
}