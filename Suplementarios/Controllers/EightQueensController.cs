using EightQueens.Services;
using EightQueens.Strategies;
using EightQueens.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EightQueens.Web.Controllers
{
    /// <summary>
    /// Controlador principal para el problema de las 8 reinas
    /// Patrón: MVC (Model-View-Controller)
    /// Responsabilidad: Manejar las peticiones web y coordinar la lógica de negocio
    /// </summary>
    public class EightQueensController : Controller
    {
        // Ya no necesita QueensSolver - usa estrategias directamente para evitar interacción de consola
        public EightQueensController()
        {
        }

        /// <summary>
        /// GET: Página principal
        /// </summary>
        public IActionResult Index()
        {
            var viewModel = new EightQueensViewModel
            {
                StatusMessage = "🧠 Bienvenido al solucionador DFS de las 8 Reinas! Usa el algoritmo preferido según literatura especializada.",
                Algorithm = "DFS Backtracking (Depth First Search)"
            };
            return View(viewModel);
        }

        /// <summary>
        /// GET: Página del juego de las 8 reinas
        /// </summary>
        public IActionResult Game()
        {
            var viewModel = new EightQueensViewModel
            {
                StatusMessage = "🧠 Bienvenido al solucionador DFS de las 8 Reinas! Selecciona el tamaño del tablero y resuelve el problema.",
                Algorithm = "DFS Backtracking (Depth First Search)"
            };
            return View(viewModel);
        }

        /// <summary>
        /// POST: Resolver el problema de las N reinas
        /// </summary>
        [HttpPost]
        public IActionResult Solve(int boardSize = 8)
        {
            try
            {
                // Validar entrada
                if (boardSize < 4 || boardSize > 12)
                {
                    var errorModel = new EightQueensViewModel
                    {
                        BoardSize = boardSize,
                        StatusMessage = "❌ El tamaño del tablero debe estar entre 4 y 12.",
                        IsSolved = false
                    };
                    return View("Index", errorModel);
                }

                // Medir tiempo de ejecución - Usar estrategia directamente sin interacción
                var conflictChecker = new ConflictChecker();
                var strategy = new DFSBacktrackingSolver(conflictChecker);
                
                var stopwatch = Stopwatch.StartNew();
                var solutions = strategy.Solve(boardSize);
                stopwatch.Stop();

                // Convertir a ViewModels
                var boardViewModels = solutions.Select((board, index) => 
                    new BoardViewModel(board, index + 1)).ToList();

                var viewModel = new EightQueensViewModel
                {
                    Solutions = boardViewModels,
                    BoardSize = boardSize,
                    ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds,
                    IsSolved = true,
                    Algorithm = "DFS Backtracking (Depth First Search)",
                    StatusMessage = solutions.Count > 0 
                        ? $"✅ ¡Éxito! Algoritmo DFS encontró {solutions.Count} soluciones en {stopwatch.Elapsed.TotalMilliseconds:F2} ms"
                        : "❌ No se encontraron soluciones para este tamaño de tablero.",
                    CurrentSolutionIndex = 0
                };

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                var errorModel = new EightQueensViewModel
                {
                    BoardSize = boardSize,
                    StatusMessage = $"❌ Error al resolver el problema: {ex.Message}",
                    IsSolved = false
                };
                return View("Index", errorModel);
            }
        }

        /// <summary>
        /// API endpoint para obtener una solución específica
        /// </summary>
        [HttpGet]
        public IActionResult GetSolution(int boardSize, int solutionIndex)
        {
            try
            {
                // Usar estrategia directamente para evitar interacción de consola
                var conflictChecker = new ConflictChecker();
                var strategy = new DFSBacktrackingSolver(conflictChecker);
                var solutions = strategy.Solve(boardSize);
                
                if (solutionIndex >= 0 && solutionIndex < solutions.Count)
                {
                    var boardViewModel = new BoardViewModel(solutions[solutionIndex], solutionIndex + 1);
                    return Json(new { 
                        success = true, 
                        board = boardViewModel,
                        totalSolutions = solutions.Count 
                    });
                }

                return Json(new { 
                    success = false, 
                    message = "Índice de solución no válido" 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
        }

        /// <summary>
        /// Página de información sobre el algoritmo DFS
        /// </summary>
        public IActionResult About()
        {
            var algorithmInfo = new AlgorithmInfoViewModel
            {
                AlgorithmName = "DFS Backtracking (Depth First Search)",
                Description = "Implementación del enfoque preferido según la literatura especializada para resolver el problema de las N reinas.",
                Advantages = new List<string>
                {
                    "Modelo más natural del problema que las permutaciones",
                    "Estructura de nodos explícita con IGNode<T>",
                    "Backtracking automático manejado por el motor DFS",
                    "Separación clara de responsabilidades",
                    "Fácil extensión a otros problemas de satisfacción de restricciones",
                    "Arquitectura genérica y reutilizable"
                },
                Steps = new List<string>
                {
                    "1. Crear nodo raíz (estado inicial sin reinas)",
                    "2. firstChild(): Expandir a la siguiente fila",
                    "3. Colocar reina en primera columna válida",
                    "4. Verificar validez (no ataques diagonales)",
                    "5. Si válido: continuar en profundidad",
                    "6. nextSibling(): Probar siguiente columna",
                    "7. Si no hay hermanos: Backtrack automático",
                    "8. Repetir hasta encontrar todas las soluciones"
                },
                TechnicalDetails = new Dictionary<string, string>
                {
                    {"Patrón Principal", "Strategy + Template Method + DFS"},
                    {"Interfaz Genérica", "IGNode<T> para nodos de búsqueda"},
                    {"Motor de Búsqueda", "DFSEngine<T> genérico y reutilizable"},
                    {"Complejidad Temporal", "O(N!) en el peor caso"},
                    {"Complejidad Espacial", "O(N) para la profundidad de recursión"},
                    {"Optimización", "Poda temprana con IsValid"},
                    {"Literatura", "Enfoque preferido sobre permutaciones"},
                    {"Tecnología", "ASP.NET Core MVC con C# .NET 8"}
                }
            };

            return View(algorithmInfo);
        }

        /// <summary>
        /// API AJAX para resolver el problema de las N reinas
        /// </summary>
        [HttpPost]
        public IActionResult SolveAjax(int boardSize = 8)
        {
            try
            {
                // Validar entrada
                if (boardSize < 4 || boardSize > 10)
                {
                    return Json(new { success = false, message = "El tamaño del tablero debe estar entre 4 y 10." });
                }

                // Usar Backtracking clásico para mayor velocidad - SIN INTERACCIÓN DE CONSOLA
                var conflictChecker = new ConflictChecker();
                var strategy = new BacktrackingSolver(conflictChecker);

                var stopwatch = Stopwatch.StartNew();
                // Usar directamente la estrategia para evitar interacción de consola
                var solutions = strategy.Solve(boardSize);
                stopwatch.Stop();

                // Limitar a las primeras 10 soluciones para evitar timeouts
                var limitedSolutions = solutions.Take(10).ToList();

                var result = new
                {
                    success = true,
                    boardSize = boardSize,
                    solutionsCount = solutions.Count,
                    executionTimeMs = stopwatch.Elapsed.TotalMilliseconds,
                    algorithm = strategy.GetAlgorithmName(),
                    solutions = limitedSolutions.Select((board, index) => new
                    {
                        solutionNumber = index + 1,
                        queens = board.GetQueenPositions().Select(pos => new { row = pos.row, col = pos.col }).ToArray()
                    }).ToArray()
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// API para comparar rendimiento de algoritmos
        /// </summary>
        [HttpPost]
        public IActionResult CompareAlgorithms(int boardSize = 8)
        {
            try
            {
                var results = new
                {
                    boardSize = boardSize,
                    algorithms = new[]
                    {
                        MeasureAlgorithm("Backtracking Tradicional", 
                            new BacktrackingSolver(new ConflictChecker()), boardSize),
                        MeasureAlgorithm("DFS Backtracking (Preferido)", 
                            new DFSBacktrackingSolver(new ConflictChecker()), boardSize)
                    }
                };

                return Json(new { success = true, data = results });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private object MeasureAlgorithm(string name, ISolverStrategy strategy, int boardSize)
        {
            var renderer = new Services.NullBoardRenderer();
            var solver = new QueensSolver(strategy, renderer);
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var solutions = solver.Solve(boardSize);
            stopwatch.Stop();

            return new
            {
                name = name,
                solutionsFound = solutions.Count,
                executionTimeMs = stopwatch.Elapsed.TotalMilliseconds,
                algorithm = strategy.GetAlgorithmName()
            };
        }
    }
}