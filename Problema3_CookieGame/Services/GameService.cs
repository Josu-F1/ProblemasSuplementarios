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
            // Validar si el cuadrado es válido (puede estar en el borde de la galleta)
            bool isValidSquare = IsValidSquare(x, y);
            
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

    private bool IsValidSquare(int x, int y)
    {
        var validPoints = _game.CookiePoints.ToHashSet();
        
        // Un cuadrado es válido si:
        // 1. Todos sus vértices están dentro del área válida, O
        // 2. Al menos 2 vértices están dentro y el cuadrado está delimitado por el borde de la galleta
        
        bool v1 = validPoints.Contains((x, y));
        bool v2 = validPoints.Contains((x + 1, y));
        bool v3 = validPoints.Contains((x, y + 1));
        bool v4 = validPoints.Contains((x + 1, y + 1));
        
        int verticesInside = (v1 ? 1 : 0) + (v2 ? 1 : 0) + (v3 ? 1 : 0) + (v4 ? 1 : 0);
        
        // Si todos los vértices están dentro, es válido
        if (verticesInside == 4)
            return true;
        
        // Si al menos 2 vértices están dentro, verificar si está delimitado por el borde
        if (verticesInside >= 2)
        {
            // Verificar si los vértices fuera están en el borde del grid (dentro de GridSize)
            bool v1Valid = v1 || (x >= 0 && x < _game.GridSize && y >= 0 && y < _game.GridSize);
            bool v2Valid = v2 || (x + 1 >= 0 && x + 1 < _game.GridSize && y >= 0 && y < _game.GridSize);
            bool v3Valid = v3 || (x >= 0 && x < _game.GridSize && y + 1 >= 0 && y + 1 < _game.GridSize);
            bool v4Valid = v4 || (x + 1 >= 0 && x + 1 < _game.GridSize && y + 1 >= 0 && y + 1 < _game.GridSize);
            
            // Si todos los vértices están dentro del grid, el cuadrado puede estar en el borde
            if (v1Valid && v2Valid && v3Valid && v4Valid)
            {
                // Verificar que al menos una línea del cuadrado esté dentro del área válida
                // o que el cuadrado esté adyacente al área válida
                return true;
            }
        }
        
        return false;
    }
    
    private HashSet<(int x1, int y1, int x2, int y2)> GetEdgeLines()
    {
        var edgeLines = new HashSet<(int x1, int y1, int x2, int y2)>();
        var validPoints = _game.CookiePoints.ToHashSet();

        foreach (var point in _game.CookiePoints)
        {
            int x = point.x;
            int y = point.y;

            var neighbors = new List<(int dx, int dy)>
            {
                (-1, 0), (1, 0), (0, -1), (0, 1)
            };

            foreach (var (dx, dy) in neighbors)
            {
                int nx = x + dx;
                int ny = y + dy;
                var neighbor = (nx, ny);

                // Si el vecino está fuera del área válida pero dentro del grid, es una línea del borde
                if (!validPoints.Contains(neighbor) &&
                    nx >= 0 && ny >= 0 &&
                    nx < _game.GridSize && ny < _game.GridSize)
                {
                    // Agregar en ambas direcciones para facilitar la búsqueda
                    edgeLines.Add((x, y, nx, ny));
                    edgeLines.Add((nx, ny, x, y));
                }
            }
        }

        return edgeLines;
    }
    
    private bool IsSquareClosed(int x, int y)
    {
        var lines = _game.Lines;
        var edgeLines = GetEdgeLines();

        // Función helper para verificar si existe una línea entre dos puntos (en cualquier dirección)
        bool HasLine(int x1, int y1, int x2, int y2)
        {
            // Verificar si hay una línea dibujada por los jugadores
            bool hasDrawnLine = lines.Any(l =>
                (l.X1 == x1 && l.Y1 == y1 && l.X2 == x2 && l.Y2 == y2) ||
                (l.X1 == x2 && l.Y1 == y2 && l.X2 == x1 && l.Y2 == y1));
            
            if (hasDrawnLine)
                return true;
            
            // Verificar si es una línea del borde de la galleta
            bool isEdgeLine = edgeLines.Contains((x1, y1, x2, y2)) || 
                             edgeLines.Contains((x2, y2, x1, y1));
            
            return isEdgeLine;
        }

        // Un cuadrado está cerrado si tiene las 4 líneas (dibujadas o del borde):
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

