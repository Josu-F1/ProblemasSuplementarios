using EightQueens.Display;
using EightQueens.Models;
using EightQueens.Services;
using EightQueens.Strategies;

namespace EightQueens
{
    /// <summary>
    /// PROBLEMA DE LAS 8 REINAS
    /// Solución implementada con:
    /// - Programación Orientada a Objetos (POO)
    /// - Principios SOLID (SRP, OCP, DIP)
    /// - Patrón de Diseño: Strategy
    /// - Algoritmo de IA: Backtracking (Vuelta Atrás)
    /// 
    /// El problema consiste en colocar 8 reinas en un tablero de ajedrez 8x8
    /// de tal manera que ninguna reina amenace a otra.
    /// Una reina amenaza en su fila, columna y diagonales.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Problema de las 8 Reinas - Backtracking";

            MostrarBienvenida();

            // Inyección de dependencias (DIP)
            IConflictChecker conflictChecker = new QueenConflictChecker();
            ISolverStrategy strategy = new BacktrackingSolver(conflictChecker);
            var solver = new QueensSolver(strategy);
            IBoardRenderer renderer = new ConsoleRenderer();

            bool continuar = true;

            while (continuar)
            {
                Console.WriteLine("\n╔════════════════════════════════════════╗");
                Console.WriteLine("║    MENÚ PRINCIPAL                      ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.WriteLine("1. Resolver problema de 8 reinas");
                Console.WriteLine("2. Resolver problema de N reinas (personalizado)");
                Console.WriteLine("3. Mostrar información del algoritmo");
                Console.WriteLine("4. Salir");
                Console.Write("\nSeleccione una opción: ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        ResolverProblema(solver, renderer, 8);
                        break;

                    case "2":
                        ResolverProblemaPersonalizado(solver, renderer);
                        break;

                    case "3":
                        MostrarInformacionAlgoritmo();
                        break;

                    case "4":
                        continuar = false;
                        Console.WriteLine("\n¡Gracias por usar el programa!");
                        break;

                    default:
                        Console.WriteLine("\nOpción no válida. Intente nuevamente.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void MostrarBienvenida()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║        PROBLEMA DE LAS 8 REINAS                            ║");
            Console.WriteLine("║        Algoritmo: Backtracking (Vuelta Atrás)              ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine("\nPropuesto por Max Bezzel en 1848");
            Console.WriteLine("Implementación con POO, SOLID y Patrón Strategy\n");
        }

        static void ResolverProblema(QueensSolver solver, IBoardRenderer renderer, int size)
        {
            Console.Clear();
            Console.WriteLine($"\n╔════════════════════════════════════════╗");
            Console.WriteLine($"║  RESOLVIENDO PROBLEMA DE {size} REINAS      ║");
            Console.WriteLine($"╚════════════════════════════════════════╝\n");

            var solutions = solver.Solve(size);

            if (solutions.Count == 0)
            {
                Console.WriteLine("No se encontraron soluciones.");
                return;
            }

            Console.Write($"\n¿Desea ver todas las {solutions.Count} soluciones? (s/n): ");
            var respuesta = Console.ReadLine()?.ToLower();

            if (respuesta == "s")
            {
                for (int i = 0; i < solutions.Count; i++)
                {
                    renderer.Render(solutions[i], i + 1);

                    if ((i + 1) % 3 == 0 && i < solutions.Count - 1)
                    {
                        Console.WriteLine("Presione cualquier tecla para ver más soluciones...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
            else
            {
                Console.Write($"\n¿Cuántas soluciones desea ver? (1-{solutions.Count}): ");
                if (int.TryParse(Console.ReadLine(), out int cantidad))
                {
                    cantidad = Math.Min(cantidad, solutions.Count);
                    for (int i = 0; i < cantidad; i++)
                    {
                        renderer.Render(solutions[i], i + 1);
                    }
                }
            }
        }

        static void ResolverProblemaPersonalizado(QueensSolver solver, IBoardRenderer renderer)
        {
            Console.Clear();
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║  PROBLEMA DE N REINAS PERSONALIZADO    ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            Console.Write("Ingrese el tamaño del tablero (N): ");
            if (int.TryParse(Console.ReadLine(), out int size) && size >= 4 && size <= 12)
            {
                ResolverProblema(solver, renderer, size);
            }
            else
            {
                Console.WriteLine("\nTamaño no válido. Debe ser entre 4 y 12.");
            }
        }

        static void MostrarInformacionAlgoritmo()
        {
            Console.Clear();
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              INFORMACIÓN DEL ALGORITMO                     ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("BACKTRACKING (Vuelta Atrás)");
            Console.ResetColor();

            Console.WriteLine("\nEs un algoritmo de Inteligencia Artificial que explora");
            Console.WriteLine("sistemáticamente todas las posibles soluciones mediante:");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n1. EXPLORACIÓN:");
            Console.ResetColor();
            Console.WriteLine("   - Intenta colocar una reina en cada fila");
            Console.WriteLine("   - Prueba cada columna de la fila actual");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n2. VALIDACIÓN:");
            Console.ResetColor();
            Console.WriteLine("   - Verifica que la reina no amenace a otras");
            Console.WriteLine("   - Comprueba fila, columna y diagonales");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n3. BACKTRACKING:");
            Console.ResetColor();
            Console.WriteLine("   - Si no hay solución, retrocede (vuelta atrás)");
            Console.WriteLine("   - Prueba con otra posición");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nCOMPLEJIDAD:");
            Console.ResetColor();
            Console.WriteLine("   - Temporal: O(N!)");
            Console.WriteLine("   - Espacial: O(N)");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nPATRÓN DE DISEÑO:");
            Console.ResetColor();
            Console.WriteLine("   - Strategy: Permite cambiar el algoritmo fácilmente");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nPRINCIPIOS SOLID APLICADOS:");
            Console.ResetColor();
            Console.WriteLine("   - SRP: Cada clase tiene una única responsabilidad");
            Console.WriteLine("   - OCP: Abierto a extensión, cerrado a modificación");
            Console.WriteLine("   - DIP: Dependencia de abstracciones, no de implementaciones");
        }
    }
}
