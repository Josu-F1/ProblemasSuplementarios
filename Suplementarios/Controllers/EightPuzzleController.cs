using Microsoft.AspNetCore.Mvc;
using EightQueens.Web.Models.EightPuzzle;
using EightQueens.Web.Heuristics.EightPuzzle;
using EightQueens.Web.Solvers.EightPuzzle;
using System.Linq;
using System.Collections.Generic;

namespace EightQueens.Web.Controllers
{
    public class EightPuzzleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Solve([FromBody] int[] tiles)
        {
            if (tiles == null || tiles.Length != 9) return BadRequest("tiles must be array of 9 ints");
            var board = Board.FromArray(tiles);
            if (!board.IsSolvable()) return BadRequest("State not solvable");

            var solver = new AStarSolver(new ManhattanHeuristic());
            var moves = solver.Solve(board);
            if (moves == null) return BadRequest("No solution found");
            var res = moves.Select(m => m.ToString()).ToArray();
            return Json(new
            {
                moves = res,
                algorithm = "A*",
                heuristic = "Manhattan",
                pattern = "Strategy (IHeuristic)"
            });
        }
    }
}
