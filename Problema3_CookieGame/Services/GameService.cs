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

    public GameState GetGame()
    {
        // Verificar el estado del juego antes de devolverlo
        if (!_game.GameOver)
        {
            CheckGameOver();
        }
        return _game;
    }

    public void ResetGame()
    {
        _game = new GameState();
        _game.Player1.Score = 0;
        _game.Player2.Score = 0;
        _game.GameOver = false;
        _game.CurrentPlayer = _game.Player1.Name;
    }

    public void AddLine(int x1, int y1, int x2, int y2)
    {
        // No permitir jugadas si el juego ya terminó
        if (_game.GameOver)
        {
            return;
        }
        
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
            int squaresClosed = DetectClosedSquares(line);
            
            // Si se cerró al menos un cuadrado, el jugador mantiene el turno
            // Si no se cerró ningún cuadrado, cambia el turno
            // Pero si el juego terminó, no cambiar el turno
            if (squaresClosed == 0 && !_game.GameOver)
            {
                _game.CurrentPlayer = _game.CurrentPlayer == _game.Player1.Name ? _game.Player2.Name : _game.Player1.Name;
            }
        }
    }

    private int DetectClosedSquares(LineModel newLine)
    {
        int x1 = newLine.X1;
        int y1 = newLine.Y1;
        int x2 = newLine.X2;
        int y2 = newLine.Y2;

        var candidates = new List<(int x, int y)>();
        var validPoints = _game.CookiePoints.ToHashSet();

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

        int squaresClosed = 0;
        foreach (var (x, y) in candidates)
        {
            // Validar que el cuadrado esté dentro del área válida de la galleta
            bool isValidSquare = validPoints.Contains((x, y)) &&
                                 validPoints.Contains((x + 1, y)) &&
                                 validPoints.Contains((x, y + 1)) &&
                                 validPoints.Contains((x + 1, y + 1));
            
            if (isValidSquare && IsSquareClosed(x, y) &&
                !_game.Squares.Any(s => s.X == x && s.Y == y))
            {
                _game.Squares.Add(new Square
                {
                    X = x,
                    Y = y,
                    Owner = newLine.Owner
                });
                squaresClosed++;
                
                // Actualizar puntuación del jugador
                if (newLine.Owner == _game.Player1.Name)
                {
                    _game.Player1.Score++;
                }
                else
                {
                    _game.Player2.Score++;
                }
            }
        }
        
        // Verificar si el juego terminó después de cerrar cuadrados
        CheckGameOver();
        
        return squaresClosed;
    }
    
    private void CheckGameOver()
    {
        // Verificar si todos los cuadrados posibles están cerrados
        int totalPossible = _game.TotalPossibleTabless;
        int squaresClosed = _game.Squares.Count;
        
        if (squaresClosed >= totalPossible)
        {
            _game.GameOver = true;
        }
    }

    private bool IsSquareClosed(int x, int y)
    {
        var lines = _game.Lines;

        // Función helper para verificar si existe una línea entre dos puntos (en cualquier dirección)
        bool HasLine(int x1, int y1, int x2, int y2)
        {
            return lines.Any(l =>
                (l.X1 == x1 && l.Y1 == y1 && l.X2 == x2 && l.Y2 == y2) ||
                (l.X1 == x2 && l.Y1 == y2 && l.X2 == x1 && l.Y2 == y1));
        }

        // Un cuadrado está cerrado si tiene las 4 líneas:
        // 1. Línea superior horizontal: (x, y) -> (x+1, y)
        // 2. Línea inferior horizontal: (x, y+1) -> (x+1, y+1)
        // 3. Línea izquierda vertical: (x, y) -> (x, y+1)
        // 4. Línea derecha vertical: (x+1, y) -> (x+1, y+1)
        return HasLine(x, y, x + 1, y) &&           // Línea superior
               HasLine(x, y + 1, x + 1, y + 1) &&   // Línea inferior
               HasLine(x, y, x, y + 1) &&          // Línea izquierda
               HasLine(x + 1, y, x + 1, y + 1);    // Línea derecha
    }


    





}

