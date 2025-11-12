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
                    winner = winner
                });
            }
            
            return PartialView("_GameBoard", game);
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
                winner = winner
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
