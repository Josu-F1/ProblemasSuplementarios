using Microsoft.AspNetCore.Mvc;
using Problema3_CookieGame.Models;
using Problema3_CookieGame.Services;
using static Problema3_CookieGame.Models.LineModel;

namespace Problema3_CookieGame.Controllers
{
    public class GameController : Controller
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        public IActionResult Index()
        {
            var game = _gameService.GetGame();
            return View(game);
        }

        [HttpPost]
        public IActionResult AddLine(int x1, int y1, int x2, int y2)
        {
            // Si las coordenadas son inválidas (petición para actualizar tablero), solo devolver el tablero
            if (x1 == -1 && y1 == -1 && x2 == -1 && y2 == -1)
            {
                var currentGame = _gameService.GetGame();
                return PartialView("_GameBoard", currentGame);
            }
            
            _gameService.AddLine(x1, y1, x2, y2);
            var game = _gameService.GetGame();
            
            // Si la petición acepta JSON, devolver JSON con el estado del juego
            var acceptHeader = Request.Headers["Accept"].ToString();
            var requestedWith = Request.Headers["X-Requested-With"].ToString();
            
            if (acceptHeader.Contains("application/json") || requestedWith == "XMLHttpRequest")
            {
                string? winner = null;
                if (game.GameOver)
                {
                    if (game.Player1.Score > game.Player2.Score)
                        winner = game.Player1.Name;
                    else if (game.Player2.Score > game.Player1.Score)
                        winner = game.Player2.Name;
                    else
                        winner = "Empate";
                }
                
                return Json(new
                {
                    currentPlayer = game.CurrentPlayer,
                    player1Score = game.Player1.Score,
                    player2Score = game.Player2.Score,
                    player1Name = game.Player1.Name,
                    player2Name = game.Player2.Name,
                    player1Color = game.Player1.Color,
                    player2Color = game.Player2.Color,
                    gameOver = game.GameOver,
                    winner = winner,
                    isAIMode = game.IsAIMode,
                    isAITurn = _gameService.IsAITurn()
                });
            }
            
            return PartialView("_GameBoard", game);
        }
        
        [HttpPost]
        public IActionResult SetAIMode(bool enabled)
        {
            _gameService.SetAIMode(enabled);
            // Reiniciar el juego cuando se cambia el modo
            _gameService.ResetGame();
            var game = _gameService.GetGame();
            
            return Json(new
            {
                isAIMode = game.IsAIMode,
                player2Name = game.Player2.Name,
                player2Color = game.Player2.Color,
                currentPlayer = game.CurrentPlayer,
                player1Score = game.Player1.Score,
                player2Score = game.Player2.Score
            });
        }
        
        [HttpPost]
        [HttpPost]
        public IActionResult MakeAIMove()
        {
            Console.WriteLine("=== MakeAIMove llamado ===");
            Console.WriteLine($"IsAIMode: {_gameService.GetGame().IsAIMode}");
            Console.WriteLine($"GameOver: {_gameService.GetGame().GameOver}");
            Console.WriteLine($"CurrentPlayer: {_gameService.GetGame().CurrentPlayer}");

            if (!_gameService.IsAITurn())
            {
                Console.WriteLine("❌ No es turno de la IA");
                return Json(new { error = "No es turno de la IA" });
            }

            Console.WriteLine("✅ Es turno de la IA, obteniendo movimiento...");
            var aiMove = _gameService.GetAIMove();

            if (!aiMove.HasValue)
            {
                Console.WriteLine("❌ No hay movimientos disponibles");
                return Json(new { error = "No hay movimientos disponibles" });
            }

            Console.WriteLine($"✅ IA eligió: ({aiMove.Value.x1},{aiMove.Value.y1}) -> ({aiMove.Value.x2},{aiMove.Value.y2})");

            _gameService.AddLine(aiMove.Value.x1, aiMove.Value.y1, aiMove.Value.x2, aiMove.Value.y2);
            var game = _gameService.GetGame();

            Console.WriteLine($"Estado después de jugada IA: CurrentPlayer={game.CurrentPlayer}, P1Score={game.Player1.Score}, P2Score={game.Player2.Score}");

            string? winner = null;
            if (game.GameOver)
            {
                if (game.Player1.Score > game.Player2.Score)
                    winner = game.Player1.Name;
                else if (game.Player2.Score > game.Player1.Score)
                    winner = game.Player2.Name;
                else
                    winner = "Empate";
            }

            return Json(new
            {
                x1 = aiMove.Value.x1,
                y1 = aiMove.Value.y1,
                x2 = aiMove.Value.x2,
                y2 = aiMove.Value.y2,
                currentPlayer = game.CurrentPlayer,
                player1Score = game.Player1.Score,
                player2Score = game.Player2.Score,
                player1Name = game.Player1.Name,
                player2Name = game.Player2.Name,
                player1Color = game.Player1.Color,
                player2Color = game.Player2.Color,
                gameOver = game.GameOver,
                winner = winner,
                isAITurn = _gameService.IsAITurn()
            });
        }

        [HttpGet]
        public IActionResult GetGameState()
        {
            var game = _gameService.GetGame();
            
            string? winner = null;
            if (game.GameOver)
            {
                if (game.Player1.Score > game.Player2.Score)
                    winner = game.Player1.Name;
                else if (game.Player2.Score > game.Player1.Score)
                    winner = game.Player2.Name;
                else
                    winner = "Empate";
            }
            
            return Json(new
            {
                currentPlayer = game.CurrentPlayer,
                player1Score = game.Player1.Score,
                player2Score = game.Player2.Score,
                player1Name = game.Player1.Name,
                player2Name = game.Player2.Name,
                player1Color = game.Player1.Color,
                player2Color = game.Player2.Color,
                gameOver = game.GameOver,
                winner = winner,
                isAIMode = game.IsAIMode,
                isAITurn = _gameService.IsAITurn()
            });
        }

        [HttpPost]
        public IActionResult Reset()
        {
            _gameService.ResetGame();
            return RedirectToAction("Index");
        }
    }
}
