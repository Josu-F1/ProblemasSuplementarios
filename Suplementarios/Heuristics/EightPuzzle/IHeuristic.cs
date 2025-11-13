using EightQueens.Web.Models.EightPuzzle;

namespace EightQueens.Web.Heuristics.EightPuzzle
{
    public interface IHeuristic
    {
        int Estimate(Board board);
    }
}
