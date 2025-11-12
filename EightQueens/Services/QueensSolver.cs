using EightQueens.Models;
using EightQueens.Services;
using EightQueens.Display;

namespace EightQueens.Services
{
    /// <summary>
    /// Servicio principal para resolver el problema de las 8 reinas
    /// Implementa Strategy Pattern para diferentes algoritmos
    /// </summary>
    public class QueensSolver
    {
        private readonly ISolverStrategy _strategy;
        private readonly IBoardRenderer _renderer;

        public QueensSolver(ISolverStrategy strategy, IBoardRenderer renderer)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public List<Board> Solve(int boardSize = 8)
        {
            Console.WriteLine($"=== PROBLEMA DE LAS {boardSize} REINAS ===");
            Console.WriteLine($"Algoritmo: {_strategy.GetAlgorithmName()}");
            Console.WriteLine($"Descripción: {_strategy.GetDescription()}");
            Console.WriteLine(new string('=', 50));

            var startTime = DateTime.Now;
            var solutions = _strategy.Solve(boardSize);
            var endTime = DateTime.Now;

            Console.WriteLine($"\n⏱️ Tiempo de ejecución: {(endTime - startTime).TotalMilliseconds:F2} ms");
            Console.WriteLine($"🎯 Soluciones encontradas: {solutions.Count}");

            if (solutions.Count > 0)
            {
                Console.WriteLine("\n🏆 Primera solución encontrada:");
                _renderer.Render(solutions[0]);

                if (solutions.Count > 1)
                {
                    Console.WriteLine("\n¿Desea ver más soluciones? (s/n)");
                    var response = Console.ReadKey().KeyChar;
                    if (response == 's' || response == 'S')
                    {
                        ShowAllSolutions(solutions);
                    }
                }
            }
            else
            {
                Console.WriteLine("❌ No se encontraron soluciones para este tamaño de tablero.");
            }

            return solutions;
        }

        public void SolveAndDisplay(int boardSize = 8)
        {
            var solutions = Solve(boardSize);
            // Display logic is already handled in Solve method
        }

        private void ShowAllSolutions(List<Board> solutions)
        {
            for (int i = 1; i < solutions.Count; i++)
            {
                Console.WriteLine($"\n🎯 Solución {i + 1}:");
                _renderer.Render(solutions[i]);
                
                if ((i + 1) % 5 == 0 && i < solutions.Count - 1)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }
    }
}