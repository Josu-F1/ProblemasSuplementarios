using EightQueens.Models;
using EightQueens.Services;

namespace EightQueens.Strategies
{
    /// <summary>
    /// Implementación del algoritmo DFS con Backtracking para resolver el problema de las N reinas
    /// Patrón: Strategy + Template Method + DFS
    /// Algoritmo: Depth First Search con Backtracking (enfoque preferido según la literatura)
    /// </summary>
    public class DFSBacktrackingSolver : ISolverStrategy
    {
        private readonly IConflictChecker _conflictChecker;
        private readonly DFSEngine<QueenNode> _dfsEngine;

        public string AlgorithmName => "DFS Backtracking (Búsqueda en Profundidad)";

        public DFSBacktrackingSolver(IConflictChecker conflictChecker)
        {
            _conflictChecker = conflictChecker ?? throw new ArgumentNullException(nameof(conflictChecker));
            _dfsEngine = new DFSEngine<QueenNode>();
        }

        /// <summary>
        /// Resuelve el problema de las N reinas usando DFS con Backtracking
        /// Este enfoque es más natural que el de permutaciones según la literatura especializada
        /// </summary>
        public List<Board> Solve(int boardSize = 8)
        {
            // Crear nodo raíz (estado inicial antes de colocar cualquier reina)
            var rootNode = new QueenNode(boardSize, _conflictChecker);

            // Usar el motor DFS para encontrar todas las soluciones
            var solutionNodes = _dfsEngine.FindAllSolutions(rootNode);

            // Convertir los nodos solución a Board objects
            var boards = solutionNodes.Select(node => node.ToBoard()).ToList();

            return boards;
        }

        /// <summary>
        /// Encuentra solo la primera solución (más eficiente cuando solo se necesita una)
        /// </summary>
        public Board? FindFirstSolution(int boardSize = 8)
        {
            var rootNode = new QueenNode(boardSize, _conflictChecker);
            var solutionNode = _dfsEngine.FindFirstSolution(rootNode);
            return solutionNode?.ToBoard();
        }

        /// <summary>
        /// Obtiene estadísticas detalladas del proceso de resolución
        /// </summary>
        public SolutionStatistics GetStatistics(int boardSize = 8)
        {
            var startTime = DateTime.Now;
            var boards = Solve(boardSize);
            var endTime = DateTime.Now;

            return new SolutionStatistics
            {
                BoardSize = boardSize,
                TotalSolutions = boards.Count,
                ExecutionTimeMs = (endTime - startTime).TotalMilliseconds,
                AlgorithmUsed = AlgorithmName
            };
        }

        public string GetAlgorithmName()
        {
            return AlgorithmName;
        }

        public string GetDescription()
        {
            return "Algoritmo de Búsqueda en Profundidad (DFS) con Backtracking. " +
                   "Explora el espacio de estados en profundidad, construyendo soluciones " +
                   "parciales y retrocediendo cuando encuentra callejones sin salida. " +
                   "Este enfoque es más natural que las permutaciones según la literatura especializada.";
        }
    }

    /// <summary>
    /// Estadísticas del proceso de resolución
    /// </summary>
    public class SolutionStatistics
    {
        public int BoardSize { get; set; }
        public int TotalSolutions { get; set; }
        public double ExecutionTimeMs { get; set; }
        public string AlgorithmUsed { get; set; } = "";
        public int NodesExplored { get; set; }
        public int NodesBacktracked { get; set; }
    }
}