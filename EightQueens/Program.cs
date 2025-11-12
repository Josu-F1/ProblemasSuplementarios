using EightQueens.Display;
using EightQueens.Services;
using EightQueens.Strategies;

namespace EightQueens
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("8 Reinas - Universidad Técnica de Ambato");
            var renderer = new ConsoleBoardRenderer();
            var checker = new ConflictChecker();
            var strategy = new DFSBacktrackingSolver(checker);
            var solver = new QueensSolver(strategy, renderer);
            var solutions = solver.Solve(8);
            Console.WriteLine("Presiona Enter para salir...");
            Console.ReadLine();
        }
    }
}
