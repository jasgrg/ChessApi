using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChessApi.Services;
using ChessApi.Models;

namespace ChessApi.Tests.Services.AI
{
    [TestClass]
    public class MiniMaxServiceTests
    {
        private MiniMaxService _miniMaxService;
        
        private Board _board;

        [TestInitialize]
        public void Setup()
        {
            _miniMaxService = new MiniMaxService();
        }

        [TestMethod]
        public void GetNextMoveMustReturnIn15Seconds()
        {
            var totalMoves = 3d;
            _board = new Board();
            _board.Initialize();
            
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < totalMoves; i++)
            {
                var mv = _miniMaxService.GetNextMove(_board);
                _board.MakeMove(mv);
            }

            
            stopWatch.Stop();
            var secondsPer = stopWatch.ElapsedMilliseconds / totalMoves / 1000d;
            Assert.IsTrue(secondsPer < 15);
        }

        [TestMethod]
        public void GetNextMoveMustNotAlterBoard()
        {
            _board = new Board();
            _board.Initialize();


            for(int i = 0; i < 10; i++)
            {
                var origBoard = _board.Copy();
                var mv = _miniMaxService.GetNextMove(_board);
                Assert.IsTrue(_board.Equals(origBoard));
                _board.MakeMove(mv);
            }
        }
    }
}
