using EightQueens.Models;

namespace EightQueens.Services
{
    /// <summary>
    /// Interfaz para validar conflictos entre reinas
    /// Principio: Dependency Inversion Principle (DIP)
    /// </summary>
    public interface IConflictChecker
    {
        /// <summary>
        /// Verifica si es seguro colocar una reina en la posición especificada
        /// </summary>
        bool IsSafe(Board board, int row, int column);
    }
}
