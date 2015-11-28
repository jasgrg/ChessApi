using ChessApi.Interfaces;
using System;
using ChessApi.Models;
using ChessApi.Services.AI;

namespace ChessApi.Services
{
    public class MiniMaxService : IAIService
    {
        StandardBoardEvaluationService _evalService = new StandardBoardEvaluationService();
        Int32 _maxDepth = 5;
        Double _minScore = -9999999;
        Double _maxScore = 9999999;

        public Move GetNextMove(Board board)
        {
            var newBoard = board.Copy();

            return MiniMax(board, _maxDepth, _minScore, _maxScore, true).Move;
        }

        private TreeNode MiniMax(Board board, Double depth, Double alpha, Double beta, Boolean maximizer)
        {
            if(depth == 0)
            {
                return new TreeNode(null, _evalService.Evaluate(board));
            }

            var pieces = board.GetPiecesForPlayer(board.PlayerTurn);
            var bestScore = maximizer ? _minScore : _maxScore;
            var bestMove = default(Move);
            foreach(var pc in pieces)
            {
                var moves = pc.GetPossibleMoves(board);
                foreach(var mv in moves)
                {
                    var newBoard = board.Copy();
                    newBoard.MakeMove(mv);
                    var newNode = MiniMax(newBoard, depth - 1, alpha, beta, !maximizer);
                    if (maximizer)
                    {
                        if(newNode.Score > bestScore)
                        {
                            bestScore = newNode.Score;
                            bestMove = mv;
                        }
                        alpha = Math.Max(alpha, bestScore);
                        if(beta <= alpha)
                        {
                            return new TreeNode(bestMove, bestScore);
                        }
                    }
                    else
                    {
                        if(newNode.Score < bestScore)
                        {
                            bestScore = newNode.Score;
                            bestMove = mv;
                        }
                        beta = Math.Min(beta, bestScore);
                        if(beta <= alpha) 
                        {
                            return new TreeNode(bestMove, bestScore);
                        }
                    }
                }
            }
            return new TreeNode(bestMove, bestScore); 


        }
    }
}