using ChessApi.Models;
using ChessApi.Services;
using System.Web.Http;

namespace ChessApi.Controllers
{
    public class BoardController : ApiController
    {
        public BoardController()
        {
        }

        public Board Get()
        {
            var board = new Board();
            board.Initialize();
            return board;
        }
    }
}
