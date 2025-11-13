using EightQueens.Web.Models.EightPuzzle;
using EightQueens.Web.Heuristics.EightPuzzle;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EightQueens.Web.Solvers.EightPuzzle
{
    public class AStarSolver
    {
        private readonly IHeuristic heuristic;
        public AStarSolver(IHeuristic heuristic) { this.heuristic = heuristic ?? throw new ArgumentNullException(nameof(heuristic)); }

        record Node(Board board, int g, int f, List<Direction> path);

        public List<Direction>? Solve(Board start)
        {
            var goalKey = Board.Goal().ToKey();
            if (start.ToKey() == goalKey) return new List<Direction>();

            var open = new PriorityQueue<Node, int>();
            var openSet = new Dictionary<string, int>();
            var closed = new HashSet<string>();

            int h(Board b) => heuristic.Estimate(b);

            var startNode = new Node(start.Copy(), 0, h(start), new List<Direction>());
            open.Enqueue(startNode, startNode.f);
            openSet[start.ToKey()] = startNode.f;

            while (open.Count > 0)
            {
                var current = open.Dequeue();
                var key = current.board.ToKey();
                if (key == goalKey) return current.path;
                closed.Add(key);

                foreach (var (nb, move) in Expand(current))
                {
                    var nKey = nb.ToKey();
                    if (closed.Contains(nKey)) continue;
                    var g = current.g + 1;
                    var f = g + h(nb);
                    if (openSet.TryGetValue(nKey, out var existing) && existing <= f) continue;
                    var path = new List<Direction>(current.path) { move };
                    var node = new Node(nb, g, f, path);
                    open.Enqueue(node, f);
                    openSet[nKey] = f;
                }
            }
            return null;
        }

        IEnumerable<(Board, Direction)> Expand(Node n)
        {
            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                if (n.board.TryMove(d, out var nb)) yield return (nb, d);
            }
        }
    }
}
