using Problema3_CookieGame.Models;
using static Problema3_CookieGame.Models.LineModel;

namespace Problema3_CookieGame.Services;

public class AIPlayer
{
    private readonly int _maxDepth;
    private readonly string _aiPlayerName;
    private readonly string _humanPlayerName;

    public AIPlayer(string aiPlayerName, string humanPlayerName, int maxDepth = 4)
    {
        _aiPlayerName = aiPlayerName;
        _humanPlayerName = humanPlayerName;
        _maxDepth = maxDepth;
    }

    public (int x1, int y1, int x2, int y2)? GetBestMove(GameState gameState)
    {
        Console.WriteLine($"=== AIPlayer.GetBestMove ===");
        Console.WriteLine($"AI: {_aiPlayerName}, Human: {_humanPlayerName}");
        Console.WriteLine($"CurrentPlayer: {gameState.CurrentPlayer}");

        var possibleMoves = GetPossibleMoves(gameState);
        Console.WriteLine($"Movimientos posibles: {possibleMoves.Count}");

        if (possibleMoves.Count == 0)
        {
            Console.WriteLine("❌ No hay movimientos disponibles");
            return null;
        }

        int bestScore = int.MinValue;
        (int x1, int y1, int x2, int y2)? bestMove = null;

        foreach (var move in possibleMoves)
        {
            // Crear una copia del estado del juego para simular el movimiento
            var gameCopy = CloneGameState(gameState);
            var line = new LineModel
            {
                X1 = move.x1,
                Y1 = move.y1,
                X2 = move.x2,
                Y2 = move.y2,
                Owner = _aiPlayerName
            };

            // Simular el movimiento
            int squaresBefore = gameCopy.Squares.Count;
            gameCopy.Lines.Add(line);
            SimulateMove(gameCopy, line);
            int squaresAfter = gameCopy.Squares.Count;
            int squaresClosed = squaresAfter - squaresBefore;

            // Si cierra cuadrados, mantener el turno (maximizar)
            // Si no cierra cuadrados, cambiar el turno (minimizar)
            string nextPlayer = squaresClosed > 0 ? _aiPlayerName : _humanPlayerName;
            gameCopy.CurrentPlayer = nextPlayer;

            // Evaluar el movimiento con Minimax
            int score = Minimax(gameCopy, _maxDepth - 1, false, int.MinValue, int.MaxValue);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private int Minimax(GameState gameState, int depth, bool isMaximizing, int alpha, int beta)
    {
        // Condiciones de terminación
        if (depth == 0 || gameState.GameOver)
        {
            return Evaluate(gameState);
        }

        var possibleMoves = GetPossibleMoves(gameState);
        
        if (possibleMoves.Count == 0)
        {
            return Evaluate(gameState);
        }

        if (isMaximizing) // Turno de la IA
        {
            int maxScore = int.MinValue;
            
            foreach (var move in possibleMoves)
            {
                var gameCopy = CloneGameState(gameState);
                var line = new LineModel
                {
                    X1 = move.x1,
                    Y1 = move.y1,
                    X2 = move.x2,
                    Y2 = move.y2,
                    Owner = _aiPlayerName
                };

                int squaresBefore = gameCopy.Squares.Count;
                gameCopy.Lines.Add(line);
                SimulateMove(gameCopy, line);
                int squaresAfter = gameCopy.Squares.Count;
                int squaresClosed = squaresAfter - squaresBefore;

                string nextPlayer = squaresClosed > 0 ? _aiPlayerName : _humanPlayerName;
                gameCopy.CurrentPlayer = nextPlayer;

                int score = Minimax(gameCopy, depth - 1, squaresClosed > 0, alpha, beta);
                maxScore = Math.Max(maxScore, score);
                alpha = Math.Max(alpha, score);

                if (beta <= alpha)
                    break; // Poda alpha-beta
            }

            return maxScore;
        }
        else // Turno del jugador humano
        {
            int minScore = int.MaxValue;
            
            foreach (var move in possibleMoves)
            {
                var gameCopy = CloneGameState(gameState);
                var line = new LineModel
                {
                    X1 = move.x1,
                    Y1 = move.y1,
                    X2 = move.x2,
                    Y2 = move.y2,
                    Owner = _humanPlayerName
                };

                int squaresBefore = gameCopy.Squares.Count;
                gameCopy.Lines.Add(line);
                SimulateMove(gameCopy, line);
                int squaresAfter = gameCopy.Squares.Count;
                int squaresClosed = squaresAfter - squaresBefore;

                string nextPlayer = squaresClosed > 0 ? _humanPlayerName : _aiPlayerName;
                gameCopy.CurrentPlayer = nextPlayer;

                int score = Minimax(gameCopy, depth - 1, squaresClosed > 0, alpha, beta);
                minScore = Math.Min(minScore, score);
                beta = Math.Min(beta, score);

                if (beta <= alpha)
                    break; // Poda alpha-beta
            }

            return minScore;
        }
    }

    private int Evaluate(GameState gameState)
    {
        int aiScore = gameState.Player1.Name == _aiPlayerName 
            ? gameState.Player1.Score 
            : gameState.Player2.Score;
        
        int humanScore = gameState.Player1.Name == _humanPlayerName 
            ? gameState.Player1.Score 
            : gameState.Player2.Score;

        // Diferencia de puntos (positivo es bueno para la IA)
        int scoreDifference = aiScore - humanScore;

        // Bonus por cuadrados casi cerrados (heurística)
        int potentialSquares = CountPotentialSquares(gameState);
        scoreDifference += potentialSquares / 2;

        // Si el juego terminó, dar un gran bonus/penalización
        if (gameState.GameOver)
        {
            if (aiScore > humanScore)
                return 1000 + scoreDifference * 10;
            else if (humanScore > aiScore)
                return -1000 + scoreDifference * 10;
            else
                return 0; // Empate
        }

        return scoreDifference;
    }

    private int CountPotentialSquares(GameState gameState)
    {
        int count = 0;
        var validPoints = gameState.CookiePoints.ToHashSet();
        var edgeLines = GetEdgeLines(gameState);

        foreach (var point in gameState.CookiePoints)
        {
            int x = point.x;
            int y = point.y;

            // Verificar si este punto puede ser esquina de un cuadrado
            var candidates = new List<(int x, int y)>
            {
                (x, y),
                (x - 1, y),
                (x, y - 1),
                (x - 1, y - 1)
            };

            foreach (var (sqX, sqY) in candidates)
            {
                if (sqX < 0 || sqY < 0 || sqX >= gameState.GridSize - 1 || sqY >= gameState.GridSize - 1)
                    continue;

                if (gameState.Squares.Any(s => s.X == sqX && s.Y == sqY))
                    continue;

                // Contar cuántas líneas faltan para cerrar el cuadrado
                int linesCount = CountSquareLines(gameState, sqX, sqY, edgeLines);
                if (linesCount == 3) // Solo falta una línea
                {
                    count++;
                }
            }
        }

        return count;
    }

    private int CountSquareLines(GameState gameState, int x, int y, HashSet<(int x1, int y1, int x2, int y2)> edgeLines)
    {
        int count = 0;
        var lines = gameState.Lines;

        // Verificar las 4 líneas del cuadrado
        if (HasLine(lines, edgeLines, x, y, x + 1, y)) count++;
        if (HasLine(lines, edgeLines, x, y + 1, x + 1, y + 1)) count++;
        if (HasLine(lines, edgeLines, x, y, x, y + 1)) count++;
        if (HasLine(lines, edgeLines, x + 1, y, x + 1, y + 1)) count++;

        return count;
    }

    private bool HasLine(List<LineModel> lines, HashSet<(int x1, int y1, int x2, int y2)> edgeLines, 
                         int x1, int y1, int x2, int y2)
    {
        // Verificar línea dibujada
        bool hasDrawnLine = lines.Any(l =>
            (l.X1 == x1 && l.Y1 == y1 && l.X2 == x2 && l.Y2 == y2) ||
            (l.X1 == x2 && l.Y1 == y2 && l.X2 == x1 && l.Y2 == y1));

        if (hasDrawnLine)
            return true;

        // Verificar línea del borde
        return edgeLines.Contains((x1, y1, x2, y2)) || edgeLines.Contains((x2, y2, x1, y1));
    }

    private HashSet<(int x1, int y1, int x2, int y2)> GetEdgeLines(GameState gameState)
    {
        var edgeLines = new HashSet<(int x1, int y1, int x2, int y2)>();
        var validPoints = gameState.CookiePoints.ToHashSet();

        foreach (var point in gameState.CookiePoints)
        {
            int x = point.x;
            int y = point.y;

            var neighbors = new List<(int dx, int dy)> { (-1, 0), (1, 0), (0, -1), (0, 1) };

            foreach (var (dx, dy) in neighbors)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (!validPoints.Contains((nx, ny)) &&
                    nx >= 0 && ny >= 0 &&
                    nx < gameState.GridSize && ny < gameState.GridSize)
                {
                    edgeLines.Add((x, y, nx, ny));
                    edgeLines.Add((nx, ny, x, y));
                }
            }
        }

        return edgeLines;
    }

    private List<(int x1, int y1, int x2, int y2)> GetPossibleMoves(GameState gameState)
    {
        var moves = new List<(int x1, int y1, int x2, int y2)>();
        var validPoints = gameState.CookiePoints.ToHashSet();
        var existingLines = gameState.Lines;

        foreach (var point1 in gameState.CookiePoints)
        {
            var neighbors = new List<(int dx, int dy)> { (-1, 0), (1, 0), (0, -1), (0, 1) };

            foreach (var (dx, dy) in neighbors)
            {
                int x2 = point1.x + dx;
                int y2 = point1.y + dy;

                if (validPoints.Contains((x2, y2)))
                {
                    // Verificar que la línea no exista
                    bool exists = existingLines.Any(l =>
                        (l.X1 == point1.x && l.Y1 == point1.y && l.X2 == x2 && l.Y2 == y2) ||
                        (l.X1 == x2 && l.Y1 == y2 && l.X2 == point1.x && l.Y2 == point1.y));

                    if (!exists)
                    {
                        moves.Add((point1.x, point1.y, x2, y2));
                    }
                }
            }
        }

        return moves;
    }

    private GameState CloneGameState(GameState original)
    {
        return new GameState
        {
            GridSize = original.GridSize,
            Lines = new List<LineModel>(original.Lines.Select(l => new LineModel
            {
                X1 = l.X1,
                Y1 = l.Y1,
                X2 = l.X2,
                Y2 = l.Y2,
                Owner = l.Owner
            })),
            Squares = new List<Square>(original.Squares.Select(s => new Square
            {
                X = s.X,
                Y = s.Y,
                Owner = s.Owner
            })),
            Player1 = new PlayerModel
            {
                Name = original.Player1.Name,
                Color = original.Player1.Color,
                Score = original.Player1.Score
            },
            Player2 = new PlayerModel
            {
                Name = original.Player2.Name,
                Color = original.Player2.Color,
                Score = original.Player2.Score
            },
            CurrentPlayer = original.CurrentPlayer,
            GameOver = original.GameOver
        };
    }

    private void SimulateMove(GameState gameState, LineModel line)
    {
        // Simular la detección de cuadrados cerrados (versión simplificada)
        var validPoints = gameState.CookiePoints.ToHashSet();
        var edgeLines = GetEdgeLines(gameState);

        int x1 = line.X1;
        int y1 = line.Y1;
        int x2 = line.X2;
        int y2 = line.Y2;

        var candidates = new List<(int x, int y)>();

        if (x1 == x2) // línea vertical
        {
            int x = x1;
            int yMin = Math.Min(y1, y2);
            candidates.Add((x - 1, yMin));
            candidates.Add((x, yMin));
        }
        else if (y1 == y2) // línea horizontal
        {
            int y = y1;
            int xMin = Math.Min(x1, x2);
            candidates.Add((xMin, y - 1));
            candidates.Add((xMin, y));
        }

        foreach (var (x, y) in candidates)
        {
            if (IsValidSquare(gameState, x, y) && IsSquareClosed(gameState, x, y, edgeLines) &&
                !gameState.Squares.Any(s => s.X == x && s.Y == y))
            {
                gameState.Squares.Add(new Square
                {
                    X = x,
                    Y = y,
                    Owner = line.Owner
                });

                if (line.Owner == gameState.Player1.Name)
                {
                    gameState.Player1.Score++;
                }
                else
                {
                    gameState.Player2.Score++;
                }
            }
        }

        // Verificar fin de juego
        if (gameState.Squares.Count >= gameState.TotalPossibleTabless)
        {
            gameState.GameOver = true;
        }
    }

    private bool IsValidSquare(GameState gameState, int x, int y)
    {
        var validPoints = gameState.CookiePoints.ToHashSet();
        
        bool v1 = validPoints.Contains((x, y));
        bool v2 = validPoints.Contains((x + 1, y));
        bool v3 = validPoints.Contains((x, y + 1));
        bool v4 = validPoints.Contains((x + 1, y + 1));
        
        int verticesInside = (v1 ? 1 : 0) + (v2 ? 1 : 0) + (v3 ? 1 : 0) + (v4 ? 1 : 0);
        
        if (verticesInside == 4)
            return true;
        
        if (verticesInside >= 2)
        {
            bool v1Valid = v1 || (x >= 0 && x < gameState.GridSize && y >= 0 && y < gameState.GridSize);
            bool v2Valid = v2 || (x + 1 >= 0 && x + 1 < gameState.GridSize && y >= 0 && y < gameState.GridSize);
            bool v3Valid = v3 || (x >= 0 && x < gameState.GridSize && y + 1 >= 0 && y + 1 < gameState.GridSize);
            bool v4Valid = v4 || (x + 1 >= 0 && x + 1 < gameState.GridSize && y + 1 >= 0 && y + 1 < gameState.GridSize);
            
            if (v1Valid && v2Valid && v3Valid && v4Valid)
            {
                return true;
            }
        }
        
        return false;
    }

    private bool IsSquareClosed(GameState gameState, int x, int y, HashSet<(int x1, int y1, int x2, int y2)> edgeLines)
    {
        var lines = gameState.Lines;

        bool HasLine(int x1, int y1, int x2, int y2)
        {
            bool hasDrawnLine = lines.Any(l =>
                (l.X1 == x1 && l.Y1 == y1 && l.X2 == x2 && l.Y2 == y2) ||
                (l.X1 == x2 && l.Y1 == y2 && l.X2 == x1 && l.Y2 == y1));
            
            if (hasDrawnLine)
                return true;
            
            return edgeLines.Contains((x1, y1, x2, y2)) || edgeLines.Contains((x2, y2, x1, y1));
        }

        return HasLine(x, y, x + 1, y) &&
               HasLine(x, y + 1, x + 1, y + 1) &&
               HasLine(x, y, x, y + 1) &&
               HasLine(x + 1, y, x + 1, y + 1);
    }
}

