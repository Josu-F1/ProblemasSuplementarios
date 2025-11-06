using EightQueens.Models;
using EightQueens.Services;

namespace EightQueens.Strategies
{
    /// <summary>
    /// Implementación del algoritmo de Backtracking para resolver el problema de las 8 reinas
    /// Patrón: Strategy
    /// Algoritmo: Inteligencia Artificial - Backtracking (búsqueda con vuelta atrás)
    /// </summary>
    public class BacktrackingSolver : ISolverStrategy
    {
        private readonly IConflictChecker _conflictChecker;
        private readonly List<Board> _solutions;

        public string AlgorithmName => "Backtracking (Vuelta Atrás)";

        public BacktrackingSolver(IConflictChecker conflictChecker)
        {
            _conflictChecker = conflictChecker ?? throw new ArgumentNullException(nameof(conflictChecker));
            _solutions = new List<Board>();
        }

        /// <summary>
        /// Resuelve el problema de las N reinas usando backtracking
        /// </summary>
        public List<Board> Solve(int boardSize = 8)
        {
            _solutions.Clear();
            var board = new Board(boardSize);
            SolveRecursive(board, 0);
            return _solutions;
        }

        /// <summary>
        /// Método recursivo que implementa el algoritmo de backtracking
        /// </summary>
        /// <param name="board">Tablero actual</param>
        /// <param name="row">Fila actual donde intentar colocar una reina</param>
        private void SolveRecursive(Board board, int row)
        {
            // Caso base: Si hemos colocado todas las reinas, guardamos la solución
            if (row == board.Size)
            {
                _solutions.Add(board.Clone());
                return;
            }

            // Intentar colocar una reina en cada columna de la fila actual
            for (int column = 0; column < board.Size; column++)
            {
                // Verificar si es seguro colocar la reina en esta posición
                if (_conflictChecker.IsSafe(board, row, column))
                {
                    // Colocar la reina (paso hacia adelante)
                    board.PlaceQueen(row, column);

                    // Recursión: intentar colocar reinas en las siguientes filas
                    SolveRecursive(board, row + 1);

                    // Backtracking: remover la reina y probar otra posición
                    board.RemoveQueen(row);
                }
            }
        }
    }
}
