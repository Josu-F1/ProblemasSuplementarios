using EightQueens.Models;
using EightQueens.Services;

namespace EightQueens.Services
{
    /// <summary>
    /// Verificador de conflictos para el problema de las 8 reinas
    /// Implementa SRP: responsabilidad única de verificar conflictos
    /// </summary>
    public class ConflictChecker : IConflictChecker
    {
        public bool HasConflict(Board board, int row, int col)
        {
            // Verificar fila
            for (int c = 0; c < board.Size; c++)
            {
                if (c != col && board.HasQueen(row, c))
                    return true;
            }

            // Verificar columna
            for (int r = 0; r < board.Size; r++)
            {
                if (r != row && board.HasQueen(r, col))
                    return true;
            }

            // Verificar diagonal principal (arriba-izquierda a abajo-derecha)
            for (int i = 1; i < board.Size; i++)
            {
                // Hacia arriba-izquierda
                if (row - i >= 0 && col - i >= 0 && board.HasQueen(row - i, col - i))
                    return true;
                
                // Hacia abajo-derecha
                if (row + i < board.Size && col + i < board.Size && board.HasQueen(row + i, col + i))
                    return true;
            }

            // Verificar diagonal secundaria (arriba-derecha a abajo-izquierda)
            for (int i = 1; i < board.Size; i++)
            {
                // Hacia arriba-derecha
                if (row - i >= 0 && col + i < board.Size && board.HasQueen(row - i, col + i))
                    return true;
                
                // Hacia abajo-izquierda
                if (row + i < board.Size && col - i >= 0 && board.HasQueen(row + i, col - i))
                    return true;
            }

            return false;
        }

        public bool IsValidBoard(Board board)
        {
            var queens = board.GetQueenPositions();
            
            foreach (var (row, col) in queens)
            {
                if (HasConflict(board, row, col))
                    return false;
            }

            return true;
        }
    }
}