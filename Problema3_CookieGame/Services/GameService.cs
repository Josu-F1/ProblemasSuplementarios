using Problema3_CookieGame.Models;
using System.Drawing;
using static Problema3_CookieGame.Models.CuadroModel;
using static Problema3_CookieGame.Models.LineModel;

namespace Problema3_CookieGame.Services;


public class GameService
{
    private GameState _game;

    public GameService()
    {
        _game = new GameState();
    }

    public GameState GetGame() => _game;

    public void ResetGame()
    {
        _game = new GameState();
    }

    public void AddLine(int x1, int y1, int x2, int y2)
    {
        var line = new LineModel
        {
            X1 = x1,
            Y1 = y1,
            X2 = x2,
            Y2 = y2,
            Owner = _game.CurrentPlayer
        };

        bool isStraight = (x1 == x2) || (y1 == y2);
        bool alreadyExists = _game.Lines.Any(l =>
            (l.X1 == x1 && l.Y1 == y1 && l.X2 == x2 && l.Y2 == y2) ||
            (l.X1 == x2 && l.Y1 == y2 && l.X2 == x1 && l.Y2 == y1));

        if (isStraight && _game.IsValidLine(line) && !alreadyExists)
        {
            _game.Lines.Add(line);
            DetectClosedSquares(line);
            _game.CurrentPlayer = _game.CurrentPlayer == _game.Player1.Name ? _game.Player2.Name : _game.Player1.Name;
        }
    }

    private void DetectClosedSquares(LineModel newLine)
    {
        int x1 = newLine.X1;
        int y1 = newLine.Y1;
        int x2 = newLine.X2;
        int y2 = newLine.Y2;

        var candidates = new List<(int x, int y)>();

        if (x1 == x2) // línea vertical
        {
            int x = x1;
            int yMin = Math.Min(y1, y2);
            candidates.Add((x - 1, yMin)); // cuadrado a la izquierda
            candidates.Add((x, yMin));     // cuadrado a la derecha
        }
        else if (y1 == y2) // línea horizontal
        {
            int y = y1;
            int xMin = Math.Min(x1, x2);
            candidates.Add((xMin, y - 1)); // cuadrado arriba
            candidates.Add((xMin, y));     // cuadrado abajo
        }

        foreach (var (x, y) in candidates)
        {
            // ✅ Elimina el filtro restrictivo si ya estás validando con IsSquareClosed
            if (IsSquareClosed(x, y) &&
                !_game.Squares.Any(s => s.X == x && s.Y == y))
            {
                _game.Squares.Add(new Square
                {
                    X = x,
                    Y = y,
                    Owner = newLine.Owner
                });
            }
        }
    }

    private bool IsSquareClosed(int x, int y)
    {
        var lines = _game.Lines;

        return
            lines.Any(l => l.X1 == x && l.Y1 == y && l.X2 == x + 1 && l.Y2 == y) &&
            lines.Any(l => l.X1 == x && l.Y1 == y + 1 && l.X2 == x + 1 && l.Y2 == y + 1) &&
            lines.Any(l => l.X1 == x && l.Y1 == y && l.X2 == x && l.Y2 == y + 1) &&
            lines.Any(l => l.X1 == x + 1 && l.Y1 == y && l.X2 == x + 1 && l.Y2 == y + 1);
    }


    





}

