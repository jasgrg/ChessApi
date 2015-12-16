using ChessApi.Interfaces;
using System;
using ChessApi.Models;
using ChessApi.Services.AI;
using System.Linq;

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
                return GetLeafNode(board, maximizer);
            }
            var pieces = board.GetPiecesForPlayer(board.PlayerTurn);
            var bestScore = maximizer ? _minScore : _maxScore;
            var bestMove = default(Move);

            var moves = pieces.SelectMany(x => x.GetPossibleMoves(board));
            if (!moves.Any())
            {
                if (board.PlayerHasCheck(board.PlayerTurn))
                {
                    return new TreeNode(null, maximizer ? _maxScore : _minScore);
                }
            }

            foreach (var mv in moves)
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
            return new TreeNode(bestMove, bestScore); 
        }

        private TreeNode GetLeafNode(Board board, Boolean maximizer)
        {
            var status = board.GetBoardStatus();
            var score = 0d;

            if (status == Enums.BoardStatus.Checkmate)
            {
                score = maximizer ? _maxScore : _minScore;
            }
            else if (status == Enums.BoardStatus.Stalemate)
            {
                score = 0;
            }
            else
            {
                score = _evalService.Evaluate(board);
            }
            return new TreeNode(null, score);

        }
    }
}