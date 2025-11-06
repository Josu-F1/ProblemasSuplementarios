using EightQueens.Models;

namespace EightQueens.Strategies
{
    /// <summary>
    /// Interfaz para el patrón Strategy
    /// Permite implementar diferentes algoritmos de resolución
    /// Principio: Open/Closed Principle (OCP)
    /// </summary>
    public interface ISolverStrategy
    {
        /// <summary>
        /// Resuelve el problema y retorna todas las soluciones encontradas
        /// </summary>
        List<Board> Solve(int boardSize = 8);

        /// <summary>
        /// Obtiene el nombre del algoritmo
        /// </summary>
        string AlgorithmName { get; }
    }
}
