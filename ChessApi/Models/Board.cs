using ChessApi.Enums;
using ChessApi.Interfaces;
using ChessApi.Services.Pieces;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;

namespace ChessApi.Models
{
    [DataContract]
    public class Board
    {
        private King _lightKing;
        private King _darkKing;

        [DataMember]
        public Square[,] Squares { get; set; }
        [DataMember]
        public Player PlayerTurn { get; set; }

        public Board()
        {
            Squares = new Square[8,8];
        }

        public Board(bool clear) : this()
        {
            if (clear)
            {
                for (short y = 0; y < 8; y++)
                {
                    for (short x = 0; x < 8; x++)
                    {
                        SetPiece(x, y, null);
                    }
                }

            }
        }

        public void Initialize()
        {
            SetPiece(0, 0, Piece.GetPiece(PieceType.Rook, Player.Dark, null));
            SetPiece(1, 0, Piece.GetPiece(PieceType.Knight, Player.Dark, null));
            SetPiece(2, 0, Piece.GetPiece(PieceType.Bishop, Player.Dark, null));
            SetPiece(3, 0, Piece.GetPiece(PieceType.Queen, Player.Dark, null));
            SetPiece(4, 0, Piece.GetPiece(PieceType.King, Player.Dark, null));
            SetPiece(5, 0, Piece.GetPiece(PieceType.Bishop, Player.Dark, null));
            SetPiece(6, 0, Piece.GetPiece(PieceType.Knight, Player.Dark, null));
            SetPiece(7, 0, Piece.GetPiece(PieceType.Rook, Player.Dark, null));

            SetPiece(0, 7, Piece.GetPiece(PieceType.Rook, Player.Light, null));
            SetPiece(1, 7, Piece.GetPiece(PieceType.Knight, Player.Light, null));
            SetPiece(2, 7, Piece.GetPiece(PieceType.Bishop, Player.Light, null));
            SetPiece(3, 7, Piece.GetPiece(PieceType.Queen, Player.Light, null));
            SetPiece(4, 7, Piece.GetPiece(PieceType.King, Player.Light, null));
            SetPiece(5, 7, Piece.GetPiece(PieceType.Bishop, Player.Light, null));
            SetPiece(6, 7, Piece.GetPiece(PieceType.Knight, Player.Light, null));
            SetPiece(7, 7, Piece.GetPiece(PieceType.Rook, Player.Light, null));

            for (short y = 2; y < 6; y++)
            {
                for (short x = 0; x < 8; x++)
                {
                    SetPiece(x, y, null);
                }
            }

            for (short i = 0; i < 8; i++)
            {
                SetPiece(i, 1, Piece.GetPiece(PieceType.Pawn, Player.Dark, null));
                SetPiece(i, 6, Piece.GetPiece(PieceType.Pawn, Player.Light, null));
            }


        }

        public Board Copy()
        {
            var dest = new Board();
            foreach (var sq in Squares)
            {
                IPiece pc = null;
                if (sq.Piece != null)
                {
                    pc = Piece.GetPiece(sq.Piece.PieceType, sq.Piece.Player, sq.Piece.Location);
                    pc.HasMoved = sq.Piece.HasMoved;
                }
                dest.SetPiece(sq.Location.X, sq.Location.Y, pc);
            }
            dest.PlayerTurn = PlayerTurn;
            return dest;
        }

        public void SetPlayer(Player player)
        {
            PlayerTurn = player;
        }

        public IPiece GetPiece(short x, short y)
        {
            return Squares[y, x].Piece;
        }

        public void SetPiece(short x, short y, IPiece piece)
        {
            if (piece != null)
            {
                piece.Location = new Coordinate(x, y);
            }
            if (Squares[y, x] == null)
            {
                Squares[y, x] = new Square(x, y, piece);
            }
            else
            {
                Squares[y, x].Piece = piece;
            }
            if (piece != null)
            {
                if (piece.PieceType == PieceType.King)
                {
                    if (piece.Player == Player.Light)
                        _lightKing = (King)piece;
                    else
                        _darkKing = (King)piece;
                }
            }
        }

        public Square GetSquare(short x, short y)
        {
            return Squares[y, x];
        }

        public void MakeMove(Move mv)
        {
            MovePiece(mv);
            
        }

        private void MovePiece(Move mv)
        {
            var movedpiece = GetPiece(mv.From.X, mv.From.Y);
            var capturedPiece = GetPiece(mv.To.X, mv.To.Y);
            
            if(capturedPiece != null)
            {
                switch (capturedPiece.PieceType)
                {
                    case PieceType.King:
                        throw new System.Exception("Cannot capture the king.");
                }
            }

            if(movedpiece != null)
            {
                movedpiece.HasMoved = true;
            }

            SetPiece(mv.To.X, mv.To.Y, movedpiece);
            SetPiece(mv.From.X, mv.From.Y, null);

            PlayerTurn = OpposingPlayer();
        }

        public Player OpposingPlayer()
        {
            return PlayerTurn == Player.Light ? Player.Dark : Player.Light;
        }
        
        public Coordinate GetCenter()
        {
            return new Coordinate(4, 4);
        }

        public Coordinate GetKingPosition(Player player)
        {
            return player == Player.Light ? _lightKing.Location : _darkKing.Location;
        }

        public List<IPiece> GetPiecesForPlayer(Player player)
        {
            var result = new List<IPiece>();
            foreach (var sq in Squares)
            {
                if (sq.Piece != null && sq.Piece.Player == player)
                {
                    result.Add(sq.Piece);
                }
            }
            return result;
        }

        public bool PlayerHasCheck(Player player)
        {
            if(player == Player.Light)
            {
                return _lightKing.IsUnderAttack(this);
            }
            else
            {
                return _darkKing.IsUnderAttack(this);
            }
        }
        
        public bool CheckMoveForCheck(Move move)
        {
            var newBoard = Copy();
            var currentPlayer = newBoard.PlayerTurn;
            newBoard.MovePiece(move);
            return newBoard.PlayerHasCheck(currentPlayer);
        }

        public void FindKings()
        {
            if(_darkKing == null)
            {
                _darkKing = (King)GetPiecesForPlayer(Player.Dark).Where(x => x.PieceType == PieceType.King).Single();
            }
            if (_lightKing == null)
            {
                _lightKing = (King)GetPiecesForPlayer(Player.Light).Where(x => x.PieceType == PieceType.King).Single();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (short y = 0; y < 8; y++)
            {
                for (short x = 0; x < 8; x++)
                {
                    var pc = GetSquare(x, y).Piece;
                    sb.Append("|" + ((pc != null) ? GetPieceString(pc) : " "));
                }
                sb.Append("|\n");
            }
            return sb.ToString();
        }

        private string GetPieceString(IPiece pc)
        {
            switch (pc.PieceType)
            {
                case PieceType.Pawn:
                    return pc.Player == Player.Light ? "P" : "p";
                case PieceType.Bishop:
                    return pc.Player == Player.Light ? "B" : "b";
                case PieceType.King:
                    return pc.Player == Player.Light ? "K" : "k";
                case PieceType.Queen:
                    return pc.Player == Player.Light ? "Q" : "q";
                case PieceType.Knight:
                    return pc.Player == Player.Light ? "N" : "n";
                case PieceType.Rook:
                    return pc.Player == Player.Light ? "R" : "r";
                default:
                    return "?";

            }
        }

    }
}