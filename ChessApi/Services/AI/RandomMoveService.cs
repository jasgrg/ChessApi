using ChessApi.Interfaces;
using System;
using ChessApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace ChessApi.Services.AI
{
    public class RandomMoveService : IAIService
    {
        public Move GetNextMove(Board board)
        {
            var allMoves = new List<Move>();
            var pcs = board.GetPiecesForPlayer(board.PlayerTurn);
            foreach(var pc in pcs)
            {
                var moves = pc.GetPossibleMoves(board);
                allMoves.AddRange(moves);
            }

            var randomIndex = new Random().Next(allMoves.Count);
            return allMoves[randomIndex];
        }
    }
}
