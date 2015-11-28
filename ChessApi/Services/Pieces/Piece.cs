using ChessApi.Enums;
using ChessApi.Interfaces;
using ChessApi.Models;
using System.Collections.Generic;

namespace ChessApi.Services.Pieces
{
    public class Piece
    {
        protected int Direction = 1;
        public PieceType PieceType { get; set; }
        public bool HasMoved { get; set; }
        public Coordinate Location { get; set; }
        public Player Player { get; set; }

        public short Rank { get { return Player == Player.Light ? (short)(8 - (Location.Y + 1)) : Location.Y; } }
        public Piece(Coordinate location, Player player, PieceType type)
        {
            Location = location;
            Player = player;
            HasMoved = false;
            PieceType = type;

            if (player == Player.Light)
            {
                Direction *= -1;
            }
        }

        public Piece() { }

        protected MoveResult TryMoveToSquare(Board board, List<Move> possibleMoves, Move move, MoveType moveType)
        {

            if (move.To.IsValid)
            {
                var targetPiece = board.GetPiece(move.To.X, move.To.Y);
                if (moveType != MoveType.MustAttack && targetPiece == null)
                {
                    if(!board.CheckMoveForCheck(move))
                    {
                        possibleMoves.Add(move);
                    }
                    return new MoveResult() { Type = MoveResultType.Move };
                }
                else if(moveType != MoveType.CannotAttack && targetPiece != null && targetPiece.Player != Player)
                {
                    if(!board.CheckMoveForCheck(move))
                    {
                        possibleMoves.Add(move);
                    }
                    return new MoveResult() { Type = MoveResultType.Attack, CapturedPiece = targetPiece };
                }
            }
            return new MoveResult() { Type = MoveResultType.Invalid };
        }

        protected MoveResult TryAttackSquare(Board board, Move move)
        {
            if (move.To.IsValid)
            {
                var targetPiece = board.GetPiece(move.To.X, move.To.Y);
                if(targetPiece != null)
                {
                    if(targetPiece.Player != Player)
                    {
                        return new MoveResult() { Type = MoveResultType.Attack, CapturedPiece = targetPiece };
                    }
                }
                else
                {
                    return new MoveResult() { Type = MoveResultType.Move };
                }
            }
            return new MoveResult() { Type = MoveResultType.Invalid };
        }

        protected void TryMoveAlongSlope(Board board, List<Move> possibleMoves, Move slope)
        {
            var curX = slope.From.X;
            var curY = slope.From.Y;
            do
            {
                curX += slope.To.X;
                curY += slope.To.Y;
            } while (TryMoveToSquare(board, possibleMoves, new Move(slope.From, new Coordinate(curX, curY)), MoveType.CanAttack).Type == MoveResultType.Move);
        }

        protected MoveResult TryAttackAlongSlope(Board board, Move slope)
        {
            var curX = slope.From.X;
            var curY = slope.From.Y;
            var result = default(MoveResult);
            do
            {
                curX += slope.To.X;
                curY += slope.To.Y;
                result = TryAttackSquare(board, new Move(slope.From, new Coordinate(curX, curY)));
            } while (result.Type == MoveResultType.Move);
            return result;
        }

        public static IPiece GetPiece(PieceType pieceType, Player player, Coordinate location)
        {
            switch (pieceType)
            {
                case PieceType.Pawn:
                    return new Pawn(location, player);
                case PieceType.Rook:
                    return new Rook(location, player);
                case PieceType.Knight:
                    return new Knight(location, player);
                case PieceType.Bishop:
                    return new Bishop(location, player);
                case PieceType.Queen:
                    return new Queen(location, player);
                case PieceType.King:
                    return new King(location, player);
                default:
                    throw new System.Exception("Unrecognized piece type");
            }

        }

    }
}