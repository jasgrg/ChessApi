using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChessApi.Models;
using ChessApi.Services.Pieces;
using ChessApi.Enums;

namespace ChessApi.Tests.Services.Pieces
{
    [TestClass]
    public class QueenTests
    {
        private Board _board;

        [TestInitialize]
        public void Setup()
        {
            _board = new Board();
        }

        [TestMethod]
        public void CannotMoveThroughPieceOfSameColor()
        {
            var q = new Queen(null, Player.Dark);
            _board.SetPiece(3, 3, q);

            _board.SetPiece(2, 3, new Pawn(null, Player.Dark));
            _board.SetPiece(4, 3, new Pawn(null, Player.Dark));

            _board.SetPiece(3, 2, new Pawn(null, Player.Dark));
            _board.SetPiece(3, 4, new Pawn(null, Player.Dark));

            _board.SetPiece(2, 2, new Pawn(null, Player.Dark));
            _board.SetPiece(4, 2, new Pawn(null, Player.Dark));

            _board.SetPiece(2, 4, new Pawn(null, Player.Dark));
            _board.SetPiece(4, 4, new Pawn(null, Player.Dark));

            var moves = q.GetPossibleMoves(_board);
            Assert.AreEqual(0, moves.Count());

        }
    }
}
