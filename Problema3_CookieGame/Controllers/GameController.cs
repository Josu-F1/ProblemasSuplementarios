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
            return PartialView("_GameBoard", _gameService.GetGame());
        }

        [HttpPost]
        public IActionResult Reset()
        {
            _gameService.ResetGame();
            return RedirectToAction("Index");
        }
    }
}
