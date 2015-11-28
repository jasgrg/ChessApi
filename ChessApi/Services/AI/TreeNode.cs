using ChessApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessApi.Services.AI
{
    public class TreeNode
    {
        public TreeNode(Move move, Double score)
        {
            Score = score;
            Move = move;
        }
        public Double Score { get; set; }
        public Move Move { get; set; }
    }
}