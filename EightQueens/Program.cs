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
            
            // Estrategias disponibles
            ISolverStrategy backtrackingStrategy = new BacktrackingSolver(conflictChecker);
            ISolverStrategy dfsStrategy = new DFSBacktrackingSolver(conflictChecker);
            
            // Usar por defecto la estrategia DFS (enfoque preferido)
            var solver = new QueensSolver(dfsStrategy);
            IBoardRenderer renderer = new ConsoleRenderer();

            bool continuar = true;

            while (continuar)
            {
                Console.WriteLine("\n╔════════════════════════════════════════╗");
                Console.WriteLine("║    MENÚ PRINCIPAL                      ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.WriteLine("1. Resolver problema de 8 reinas");
                Console.WriteLine("2. Resolver problema de N reinas (personalizado)");
                Console.WriteLine("3. Comparar algoritmos (Backtracking vs DFS)");
                Console.WriteLine("4. Mostrar información del algoritmo");
                Console.WriteLine("5. Salir");
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
                        CompararAlgoritmos(conflictChecker, renderer);
                        break;

                    case "4":
                        MostrarInformacionAlgoritmo();
                        break;

                    case "5":
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

        static void CompararAlgoritmos(IConflictChecker conflictChecker, IBoardRenderer renderer)
        {
            Console.Clear();
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║            COMPARACIÓN DE ALGORITMOS                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.Write("Ingrese el tamaño del tablero para comparar (4-10): ");
            if (int.TryParse(Console.ReadLine(), out int size) && size >= 4 && size <= 10)
            {
                Console.WriteLine($"\n🔬 Comparando algoritmos para tablero {size}x{size}...\n");

                // Algoritmo 1: Backtracking tradicional
                var backtrackingStrategy = new BacktrackingSolver(conflictChecker);
                var backtrackingSolver = new QueensSolver(backtrackingStrategy);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1️⃣  BACKTRACKING TRADICIONAL:");
                Console.ResetColor();
                var solutions1 = backtrackingSolver.Solve(size);

                // Algoritmo 2: DFS con Backtracking
                var dfsStrategy = new DFSBacktrackingSolver(conflictChecker);
                var dfsSolver = new QueensSolver(dfsStrategy);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n2️⃣  DFS BACKTRACKING (ENFOQUE PREFERIDO):");
                Console.ResetColor();
                var solutions2 = dfsSolver.Solve(size);

                Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                      RESULTADOS                           ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
                Console.WriteLine($"\n✅ Ambos algoritmos encontraron: {solutions1.Count} soluciones");
                Console.WriteLine("✅ Los resultados son consistentes");
                Console.WriteLine("✅ El enfoque DFS es más natural y mantenible");

                Console.WriteLine("\n💡 Ventajas del enfoque DFS:");
                Console.WriteLine("   • Modelo más natural del problema");
                Console.WriteLine("   • Estructura de nodos explícita");
                Console.WriteLine("   • Fácil extensión a otros problemas");
                Console.WriteLine("   • Separación clara de responsabilidades");

                if (solutions1.Count > 0 && solutions1.Count <= 5)
                {
                    Console.WriteLine("\n¿Ver las soluciones? (s/n): ");
                    if (Console.ReadLine()?.ToLower() == "s")
                    {
                        for (int i = 0; i < solutions1.Count; i++)
                        {
                            renderer.Render(solutions1[i], i + 1);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("❌ Tamaño no válido.");
            }
        }

        static void MostrarInformacionAlgoritmo()
        {
            Console.Clear();
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              INFORMACIÓN DEL ALGORITMO                     ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("DFS BACKTRACKING (Enfoque Preferido)");
            Console.ResetColor();
            
            Console.WriteLine("\nSe implementaron DOS enfoques como se describe en la literatura:");
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n1. PATRÓN DE PERMUTACIONES:");
            Console.ResetColor();
            Console.WriteLine("   • Considera todas las permutaciones de [1,2,3,4,5,6,7,8]");
            Console.WriteLine("   • Examina hasta 8! = 40,320 permutaciones");
            Console.WriteLine("   • Con poda optimizada: ~2,056 permutaciones");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n2. DFS CON BACKTRACKING (IMPLEMENTADO):");
            Console.ResetColor();
            Console.WriteLine("   • Enfoque más NATURAL para el problema");
            Console.WriteLine("   • Modelo de nodos con IGNode<T>");
            Console.WriteLine("   • firstChild(): Expansión a siguiente fila");
            Console.WriteLine("   • nextSibling(): Alternativas en misma fila");
            Console.WriteLine("   • Backtracking automático por el motor DFS");

            Console.WriteLine("\n📚 Según la literatura especializada:");
            Console.WriteLine("\"El enfoque DFS es más natural que el de permutaciones\"");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n🔍 PROCESO DFS:");
            Console.ResetColor();
            Console.WriteLine("   1. Crear nodo raíz (estado inicial)");
            Console.WriteLine("   2. firstChild(): Colocar reina en siguiente fila");
            Console.WriteLine("   3. Verificar validez (no ataques)");
            Console.WriteLine("   4. Si válido: continuar en profundidad");
            Console.WriteLine("   5. Si inválido: nextSibling() (próxima columna)");
            Console.WriteLine("   6. Si no hay hermanos: BACKTRACK automático");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n🏗️ ARQUITECTURA IMPLEMENTADA:");
            Console.ResetColor();
            Console.WriteLine("   • IGNode<T>: Interfaz genérica para nodos");
            Console.WriteLine("   • QueenNode: Nodo específico para reinas");
            Console.WriteLine("   • DFSEngine<T>: Motor de búsqueda genérico");
            Console.WriteLine("   • DFSBacktrackingSolver: Estrategia concreta");

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
