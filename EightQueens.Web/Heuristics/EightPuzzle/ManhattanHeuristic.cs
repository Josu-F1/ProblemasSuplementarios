using EightQueens.Web.Models.EightPuzzle;
using System;

namespace EightQueens.Web.Heuristics.EightPuzzle
{
    public class ManhattanHeuristic : IHeuristic
    {
        public int Estimate(Board board)
        {
            var tiles = board.Tiles;
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int v = tiles[i];
                if (v == 0) continue;
                int r = i / 3, c = i % 3;
                int goalR = (v - 1) / 3, goalC = (v - 1) % 3;
                sum += Math.Abs(r - goalR) + Math.Abs(c - goalC);
            }
            return sum;
        }
    }
}
