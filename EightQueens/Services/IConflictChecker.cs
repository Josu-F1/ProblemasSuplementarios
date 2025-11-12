using EightQueens.Models;

namespace EightQueens.Services
{
    /// <summary>
    /// Interface para verificar conflictos entre reinas
    /// Implementa DIP: depende de abstracciones, no de concreciones
    /// </summary>
    public interface IConflictChecker
    {
        bool HasConflict(Board board, int row, int col);
        bool IsValidBoard(Board board);
    }
}