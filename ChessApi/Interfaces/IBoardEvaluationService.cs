using ChessApi.Models;
using System;

namespace ChessApi.Interfaces
{
    public interface IBoardEvaluationService
    {
        Double Evaluate(Board board);
    }
}
