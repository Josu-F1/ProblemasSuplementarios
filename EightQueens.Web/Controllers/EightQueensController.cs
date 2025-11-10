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
        private readonly QueensSolver _solver;

        public EightQueensController()
        {
            // Inyección de dependencias manual (se puede mejorar con DI Container)
            IConflictChecker conflictChecker = new QueenConflictChecker();
            ISolverStrategy strategy = new BacktrackingSolver(conflictChecker);
            _solver = new QueensSolver(strategy);
        }

        /// <summary>
        /// GET: Página principal
        /// </summary>
        public IActionResult Index()
        {
            var viewModel = new EightQueensViewModel
            {
                StatusMessage = "¡Bienvenido al solucionador de las 8 Reinas! Haz clic en 'Resolver' para comenzar."
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

                // Medir tiempo de ejecución
                var stopwatch = Stopwatch.StartNew();
                var solutions = _solver.Solve(boardSize);
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
                    StatusMessage = solutions.Count > 0 
                        ? $"✅ ¡Éxito! Se encontraron {solutions.Count} soluciones en {stopwatch.Elapsed.TotalMilliseconds:F2} ms"
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
                var solutions = _solver.Solve(boardSize);
                
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
        /// Página de información sobre el algoritmo
        /// </summary>
        public IActionResult About()
        {
            return View();
        }
    }
}