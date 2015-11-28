using ChessApi.Models;

namespace ChessApi.Interfaces
{
    public interface IAIService
    {
        Move GetNextMove(Board board);
    }
}
