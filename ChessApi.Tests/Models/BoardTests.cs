﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChessApi.Models;
using ChessApi.Enums;
using ChessApi.Services.Pieces;

namespace ChessApi.Tests.Models
{
    [TestClass]
    public class BoardTests
    {
        private Board _board;

        [TestInitialize]
        public void Setup()
        {
            _board = new Board();
        }

        [TestMethod]
        public void ConstructorMustInitializeSquares()
        {
            Assert.IsNotNull(_board.Squares);
        }

        [TestMethod]
        public void GetPieceMustReturnCorrectPiece()
        {
            _board.SetPiece(1, 2, new Pawn(null, Player.Dark));
            _board.SetPiece(2, 1, new Rook(null, Player.Light));

            var result = _board.GetPiece(1, 2);
            Assert.AreEqual(PieceType.Pawn, result.PieceType);
        }

        [TestMethod]
        public void CheckMoveForCheckMustNotAlterBoard()
        {
            _board.Initialize();


            var origBoard = _board.Copy();
            var mv = _board.CheckMoveForCheck(new Move(new Coordinate(0, 1), new Coordinate(0, 3)));

            Assert.IsTrue(_board.Equals(origBoard));
        }
    }
}
