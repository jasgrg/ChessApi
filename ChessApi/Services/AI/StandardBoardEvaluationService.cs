using System;
using ChessApi.Interfaces;
using ChessApi.Models;
using ChessApi.Enums;
using System.Collections.Generic;
using System.Linq;

namespace ChessApi.Services.AI
{
    public class StandardBoardEvaluationService : IBoardEvaluationService
    {
        private const double doubledPawnPenalty = -12;
        private const double isolatedPawnPenalty = -12;
        private const double pawnMaterial = 100;
        private const double pawnRankWeight = 1;

        private const double knightMaterial = 330;
        private const double knightDistanceToKingWeight = -1;
        private const double knightDistanceToCenterWeight = 10;

        private const double bishopMaterial = 330;
        private const double bishopDistanceToCenterWeight = 10;
        private const double bishopMobility = 1;

        private const double rookMaterial = 500;
        private const double rookKingProximity = 20;

        private const double queenMaterial = 900;
        private const double queenKingProximity = 20;
        private const double queenCenterProximity = 10;

        public double Evaluate(Board board)
        {

            Double score = 0;

            var playerPieces = board.GetPiecesForPlayer(board.PlayerTurn);
            var opposingPieces = board.GetPiecesForPlayer(board.OpposingPlayer());

            score = GetScore(playerPieces, board);
            score -= GetScore(opposingPieces, board);

            //score = 200 * (CountPiecesByType(playerPieces, PieceType.King) - CountPiecesByType(opposingPieces, PieceType.King));
            //score += 9 * (CountPiecesByType(playerPieces, PieceType.Queen) - CountPiecesByType(opposingPieces, PieceType.Queen));
            //score += 5 * (CountPiecesByType(playerPieces, PieceType.Rook) - CountPiecesByType(opposingPieces, PieceType.Rook));
            //score += 3 * ((CountPiecesByType(playerPieces, PieceType.Bishop) - CountPiecesByType(opposingPieces, PieceType.Bishop)) +
            //                (CountPiecesByType(playerPieces, PieceType.Knight) - CountPiecesByType(opposingPieces, PieceType.Knight)));
            //score += 1 * (CountPiecesByType(playerPieces, PieceType.Pawn) - (CountPiecesByType(opposingPieces, PieceType.Pawn)));

            //score -= .5 * ((DoubledPawns(playerPieces) - DoubledPawns(opposingPieces)) +
            //                (BlockedPawns(playerPieces) - BlockedPawns(opposingPieces)) +
            //                (IsolatedPawns(playerPieces) - IsolatedPawns(opposingPieces)));
            //score += .1 * (CountMoves(board, playerPieces) - CountMoves(board, opposingPieces));
            return score;
        }

        private double GetScore(List<IPiece> playerPieces, Board board)
        {
            Dictionary<short, short> pawnCoords = new Dictionary<short, short>();

            Double score = 0;
            foreach (var pc in playerPieces)
            {
                switch (pc.PieceType)
                {
                    case PieceType.Queen:
                        score += queenMaterial;
                        score += queenKingProximity * (7 - GetChebyShevDistance(pc.Location, board.GetKingPosition(board.OpposingPlayer())));
                        score += queenCenterProximity * (3 - GetChebyShevDistance(pc.Location, board.GetCenter()));
                        break;
                    case PieceType.Bishop:
                        score += bishopMaterial;
                        score += bishopDistanceToCenterWeight * (4 - GetChebyShevDistance(pc.Location, board.GetCenter()));
                        // mobility and xray mobility
                        break;
                    case PieceType.Knight:
                        score += knightMaterial;
                        score += knightDistanceToKingWeight * GetManhattanDistance(pc.Location, board.GetKingPosition(board.OpposingPlayer()));
                        score += knightDistanceToKingWeight * GetManhattanDistance(pc.Location, board.GetKingPosition(board.PlayerTurn));
                        score += knightDistanceToCenterWeight * (4 - GetChebyShevDistance(pc.Location, board.GetCenter()));
                        break;
                    case PieceType.Rook:
                        score += rookMaterial;
                        score += rookKingProximity * (14 - GetManhattanDistance(pc.Location, board.GetKingPosition(board.OpposingPlayer())));
                        //mobility and xray mobility
                        // friendly pawns.
                        break;
                    case PieceType.Pawn:
                        score += pawnMaterial;
                        score += pawnRankWeight * pc.Rank;
                        if (pawnCoords.ContainsKey(pc.Location.X))
                        {
                            pawnCoords[pc.Location.X] = Math.Min(pc.Location.Y, pawnCoords[pc.Location.X]);
                            score -= doubledPawnPenalty;
                        }
                        else
                        {
                            pawnCoords.Add(pc.Location.X, pc.Location.Y);
                        }
                        break;
                }
            }

            score += IsolatedPawns(pawnCoords);

            return score;
        }

        private double GetManhattanDistance(Coordinate from, Coordinate to)
        {
            return (Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y));
        }

        private double GetChebyShevDistance(Coordinate from, Coordinate to)
        {
            var dx = Math.Abs(from.X - to.X);
            var dy = Math.Abs(from.Y - to.Y);
            return dx < dy ? dx : dy;
        }

        private double IsolatedPawns(Dictionary<short, short> pawnCoords)
        {
            var score = 0d;
            foreach(var x in pawnCoords.Keys)
            {
                if(!pawnCoords.Keys.Any(k => k == x - 1) && !pawnCoords.Keys.Any(k => k == x + 1))
                {
                    score += isolatedPawnPenalty;
                }
            }
            return score;
        }
    }
}
