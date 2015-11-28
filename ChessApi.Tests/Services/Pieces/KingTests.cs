using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChessApi.Models;
using ChessApi.Services.Pieces;
using ChessApi.Enums;

namespace ChessApi.Tests.Services.Pieces
{
    /// <summary>
    /// Summary description for KingTests
    /// </summary>
    [TestClass]
    public class KingTests
    {
        private Board _board;

        [TestInitialize]
        public void Setup()
        {
            _board = new Board(true);
            _board.SetPlayer(Player.Light);
        }

        [TestMethod]
        public void CannotMoveIntoCheck()
        {
            var king = new King(null, Player.Light);
            _board.SetPiece(1, 1, king);
            _board.SetPiece(0, 4, new Knight(null, Player.Dark));
            var result = _board.CheckMoveForCheck(new Move(new Coordinate(1, 1), new Coordinate(1, 2)));
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void PossibleMovesDoesNotReturnMovesResultingInCheckByKnight()
        {
            var king = new King(null, Player.Light);
            _board.SetPiece(1, 1, king);
            _board.SetPiece(0, 4, new Knight(null, Player.Dark));
            var moves = king.GetPossibleMoves(_board);
            Assert.IsFalse(moves.Any(x => x.To.X == 1 && x.To.Y == 2));
            
        }

        [TestMethod]
        public void PossibleMovesDoesNotReturnMovesResultingInCheckByPawn()
        {
            var king = new King(null, Player.Light);
            _board.SetPiece(6, 6, king);
            _board.SetPiece(5, 4, new Pawn(null, Player.Dark));
            var moves = king.GetPossibleMoves(_board);
            Assert.IsFalse(moves.Any(x => x.To.X == 6 && x.To.Y == 5));

        }
    }
}
