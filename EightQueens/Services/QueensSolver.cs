using EightQueens.Models;
using EightQueens.Strategies;

namespace EightQueens.Services
{
    /// <summary>
    /// Contexto del patrón Strategy
    /// Responsabilidad: Coordinar la resolución del problema usando la estrategia seleccionada
    /// </summary>
    public class QueensSolver
    {
        private ISolverStrategy _strategy;

        public QueensSolver(ISolverStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        /// <summary>
        /// Cambia la estrategia de resolución en tiempo de ejecución
        /// </summary>
        public void SetStrategy(ISolverStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        /// <summary>
        /// Resuelve el problema usando la estrategia actual
        /// </summary>
        public List<Board> Solve(int boardSize = 8)
        {
            Console.WriteLine($"Resolviendo con algoritmo: {_strategy.AlgorithmName}");
            var startTime = DateTime.Now;

            var solutions = _strategy.Solve(boardSize);

            var endTime = DateTime.Now;
            var elapsed = endTime - startTime;

            Console.WriteLine($"Soluciones encontradas: {solutions.Count}");
            Console.WriteLine($"Tiempo de ejecución: {elapsed.TotalMilliseconds} ms\n");

            return solutions;
        }
    }
}
