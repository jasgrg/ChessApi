using ChessApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessApi.Enums;
using ChessApi.Models;

namespace ChessApi.Services.Pieces
{
    public class King : Piece, IPiece
    {
        public King(Coordinate location, Player player) : base(location, player, PieceType.King) { }

        public bool IsUnderAttack(Board board)
        {
            var underAttack = IsResultValid(TryAttackAlongSlope(board, new Move(Location, new Coordinate(0, 1))), PieceType.Rook, PieceType.Queen);
            underAttack = underAttack || IsResultValid(TryAttackAlongSlope(board, new Move(Location, new Coordinate(1, 0))), PieceType.Rook, PieceType.Queen);
            underAttack = underAttack || IsResultValid(TryAttackAlongSlope(board, new Move(Location, new Coordinate(0, -1))), PieceType.Rook, PieceType.Queen);
            underAttack = underAttack || IsResultValid(TryAttackAlongSlope(board, new Move(Location, new Coordinate(-1, 0))), PieceType.Rook, PieceType.Queen);
            underAttack = underAttack || IsResultValid(TryAttackAlongSlope(board, new Move(Location, new Coordinate(1, 1))), PieceType.Bishop, PieceType.Queen);
            underAttack = underAttack || IsResultValid(TryAttackAlongSlope(board, new Move(Location, new Coordinate(-1, -1))), PieceType.Bishop, PieceType.Queen);
            underAttack = underAttack || IsResultValid(TryAttackAlongSlope(board, new Move(Location, new Coordinate(-1, 1))), PieceType.Bishop, PieceType.Queen);
            underAttack = underAttack || IsResultValid(TryAttackAlongSlope(board, new Move(Location, new Coordinate(1, -1))), PieceType.Bishop, PieceType.Queen);

            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X + 2, Location.Y - 1))), PieceType.Knight);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X + 2, Location.Y + 1))), PieceType.Knight);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X - 2, Location.Y - 1))), PieceType.Knight);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X - 2, Location.Y + 1))), PieceType.Knight);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X + 1, Location.Y + 2))), PieceType.Knight);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X - 1, Location.Y + 2))), PieceType.Knight);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X + 1, Location.Y - 2))), PieceType.Knight);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X - 1, Location.Y - 2))), PieceType.Knight);

            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X - 1, Location.Y + Direction))), PieceType.King, PieceType.Pawn);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X, Location.Y + Direction))), PieceType.King);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X + 1, Location.Y + Direction))), PieceType.King, PieceType.Pawn);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X - 1, Location.Y))), PieceType.King);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X + 1, Location.Y))), PieceType.King);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X - 1, Location.Y - Direction))), PieceType.King);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X, Location.Y - Direction))), PieceType.King);
            underAttack = underAttack || IsResultValid(TryAttackSquare(board, new Move(Location, new Coordinate(Location.X + 1, Location.Y - Direction))), PieceType.King); 

            return underAttack;
        }

        private bool IsResultValid(MoveResult result, params PieceType[] types)
        {
            return (result.Type == MoveResultType.Attack && types.Contains(result.CapturedPiece.PieceType));
        }

        public IEnumerable<Move> GetPossibleMoves(Board board)
        {
            var result = new List<Move>();
            short x = -1;
            short y = -1;

            for (x = -1; x <= 1; x++)
            {
                for (y = -1; y <= 1; y++)
                {
                    if (!(x == 0 && y == 0))
                    {
                        TryMoveToSquare(board, result, new Move(this.Location, new Coordinate(Location.X + x, Location.Y + y)), MoveType.CanAttack);
                    }
                }
            }

            if (!HasMoved)
            {
                // Can't move to escape check
                if (!IsUnderAttack(board))
                {
                    var oneRight = new Coordinate(Location.X + 1, Location.Y);
                    var twoRight = new Coordinate(Location.X + 2, Location.Y);
                    var rook = board.GetPiece((short)(Location.X + 3), Location.Y);

                    if(board.GetPiece(oneRight.X, oneRight.Y) == null && board.GetPiece(twoRight.X, twoRight.Y) == null &&
                        rook != null && rook.PieceType == PieceType.Rook && rook.HasMoved == false)
                    {
                        if(!board.CheckMoveForCheck(new Move(Location, oneRight)) && 
                            !board.CheckMoveForCheck(new Move(Location, twoRight)))
                        {
                            result.Add(new Move(Location, twoRight, rook.Location, oneRight));
                        }
                    }

                    var oneLeft = new Coordinate(Location.X - 1, Location.Y);
                    var twoLeft = new Coordinate(Location.X - 2, Location.Y);
                    var threeLeft = new Coordinate(Location.X - 3, Location.Y);
                    rook = board.GetPiece((short)(Location.X - 4), Location.X);
                    if (board.GetPiece(oneLeft.X, oneLeft.Y) == null && board.GetPiece(twoLeft.X, twoLeft.Y) == null && board.GetPiece(threeLeft.X, threeLeft.Y) == null
                        && rook != null && rook.PieceType == PieceType.Rook && rook.HasMoved == false)
                    {
                        if (!board.CheckMoveForCheck(new Move(Location, oneLeft)) &&
                            !board.CheckMoveForCheck(new Move(Location, twoLeft)) &&
                            !board.CheckMoveForCheck(new Move(Location, threeLeft)))
                        {
                            result.Add(new Move(Location, twoLeft, rook.Location, oneLeft));
                        }
                    }
                }
            }
            return result;
        }
    }
}