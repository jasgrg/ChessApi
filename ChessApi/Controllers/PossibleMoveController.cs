using System.Web.Http;
using ChessApi.Models;
using System.Linq;
using System.Diagnostics;
using System.Net;
using System.Collections.Generic;

namespace ChessApi.Controllers
{
    public class PossibleMoveController : ApiController
    {
        public IEnumerable<Coordinate> Post([FromUri] short x, [FromUri] short y, [FromBody] Board board)
        {
            board.Init();
            Debug.WriteLine("** Input **");
            Debug.WriteLine(board.ToString());

            var pc = board.GetSquare(x, y).Piece;

            if(pc == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var moves = pc.GetPossibleMoves(board);
            
            
            Debug.WriteLine("** Output ** ");
            Debug.WriteLine(board.ToString());
            return moves.Select(m => new Coordinate(m.To.X, m.To.Y)).ToList();
        }
    }
}
