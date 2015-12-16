using System.Web.Http;
using ChessApi.Models;
using ChessApi.Services.AI;
using ChessApi.Services;
using System.Diagnostics;

namespace ChessApi.Controllers
{
    public class MoveController : ApiController
    {
        public MoveResponse PostNextMove([FromBody] Board board)
        {
            board.Init();
            Debug.WriteLine("** Input **");
            Debug.WriteLine(board.ToString());
            var aiService = new MiniMaxService();

            var move = aiService.GetNextMove(board);
            board.MakeMove(move);
            
            Debug.WriteLine("** Output ** ");
            Debug.WriteLine(board.ToString());
            return new MoveResponse() { Board = board, Move = move };
        }
    }
}
