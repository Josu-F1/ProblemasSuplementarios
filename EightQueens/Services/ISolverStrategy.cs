using EightQueens.Models;

namespace EightQueens.Services
{
    /// <summary>
    /// Interface para estrategias de resolución del problema de las 8 reinas
    /// Implementa OCP: abierto para extensión, cerrado para modificación
    /// </summary>
    public interface ISolverStrategy
    {
        List<Board> Solve(int boardSize);
        string GetAlgorithmName();
        string GetDescription();
    }
}