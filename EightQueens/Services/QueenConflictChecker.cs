using EightQueens.Models;

namespace EightQueens.Services
{
    /// <summary>
    /// Implementación del validador de conflictos para el problema de las 8 reinas
    /// Responsabilidad: Verificar si una reina puede colocarse sin amenazar a otras
    /// </summary>
    public class QueenConflictChecker : IConflictChecker
    {
        /// <summary>
        /// Verifica si es seguro colocar una reina en la posición (row, column)
        /// Una reina amenaza en la misma fila, columna y diagonales
        /// </summary>
        public bool IsSafe(Board board, int row, int column)
        {
            // Verificar columna: no debe haber otra reina en la misma columna
            for (int i = 0; i < row; i++)
            {
                if (board.GetQueenColumn(i) == column)
                    return false;
            }

            // Verificar diagonal izquierda superior
            for (int i = row - 1, j = column - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (board.GetQueenColumn(i) == j)
                    return false;
            }

            // Verificar diagonal derecha superior
            for (int i = row - 1, j = column + 1; i >= 0 && j < board.Size; i--, j++)
            {
                if (board.GetQueenColumn(i) == j)
                    return false;
            }

            return true;
        }
    }
}
