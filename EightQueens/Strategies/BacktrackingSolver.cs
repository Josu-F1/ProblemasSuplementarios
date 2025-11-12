using EightQueens.Models;
using EightQueens.Services;

namespace EightQueens.Strategies
{
    /// <summary>
    /// Implementación del algoritmo de Backtracking para resolver el problema de las 8 reinas
    /// Implementa ISolverStrategy - Strategy Pattern
    /// </summary>
    public class BacktrackingSolver : ISolverStrategy
    {
        private readonly IConflictChecker _conflictChecker;

        public BacktrackingSolver(IConflictChecker conflictChecker)
        {
            _conflictChecker = conflictChecker ?? throw new ArgumentNullException(nameof(conflictChecker));
        }

        public List<Board> Solve(int boardSize)
        {
            var solutions = new List<Board>();
            var board = new Board(boardSize);
            
            SolveRecursive(board, 0, solutions);
            
            return solutions;
        }

        private void SolveRecursive(Board board, int col, List<Board> solutions)
        {
            // Caso base: si hemos colocado todas las reinas
            if (col >= board.Size)
            {
                // Verificar que la solución es válida y agregar una copia
                if (_conflictChecker.IsValidBoard(board))
                {
                    solutions.Add(new Board(board));
                }
                return;
            }

            // Probar colocar una reina en cada fila de la columna actual
            for (int row = 0; row < board.Size; row++)
            {
                // Verificar si es seguro colocar la reina en esta posición
                if (!_conflictChecker.HasConflict(board, row, col))
                {
                    // Colocar la reina
                    board.PlaceQueen(row, col);
                    
                    // Recurrir para la siguiente columna
                    SolveRecursive(board, col + 1, solutions);
                    
                    // Backtrack: quitar la reina (vuelta atrás)
                    board.RemoveQueen(row, col);
                }
            }
        }

        public string GetAlgorithmName()
        {
            return "Backtracking Clásico";
        }

        public string GetDescription()
        {
            return "Algoritmo de vuelta atrás que explora todas las posibilidades sistemáticamente, " +
                   "colocando reinas columna por columna y retrocediendo cuando encuentra conflictos.";
        }
    }
}