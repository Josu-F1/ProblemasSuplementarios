using System.Numerics;
using static Problema3_CookieGame.Models.CuadroModel;
using static Problema3_CookieGame.Models.LineModel;

namespace Problema3_CookieGame.Models;

public class GameState
{

    public int GridSize { get; set; } = 7;

    public List<LineModel> Lines { get; set; } = new();
    public List<Square> Squares { get; set; } = new();

    public PlayerModel Player1 { get; set; } = new() { Name = "Jugador1", Color = "#ff0000" }; // rojo
    public PlayerModel Player2 { get; set; } = new() { Name = "Jugador2", Color = "#0000ff" }; // azul


    public string CurrentPlayer { get; set; } = "Jugador1";
    public bool GameOver { get; set; } = false;
    public bool IsAIMode { get; set; } = false;
    public string AIPlayerName { get; set; } = "IA";

    public List<(int x, int y)> CookiePoints => GenerateCookiePoints();

    private List<(int x, int y)> GenerateCookiePoints()
    {
        var points = new List<(int x, int y)>();
        int center = GridSize / 2;

        for (int y = 0; y < GridSize; y++)
        {
            for (int x = 0; x < GridSize; x++)
            {
                int dx = Math.Abs(x - center);
                int dy = Math.Abs(y - center);
                if (dx + dy <= center) // diamante escalonado
                    points.Add((x, y));
            }
        }

        return points;
    }


    public bool IsValidLine(LineModel line)
    {
        var valid = CookiePoints.ToHashSet();
        return valid.Contains((line.X1, line.Y1)) && valid.Contains((line.X2, line.Y2));
    }

    public int TotalPossibleTabless
    {
        get
        {
            var valid = CookiePoints.ToHashSet();
            var countedSquares = new HashSet<(int x, int y)>();
            int count = 0;

            // Contar cuadrados completamente dentro del área válida
            foreach (var (x, y) in CookiePoints)
            {
                if (valid.Contains((x + 1, y)) &&
                    valid.Contains((x, y + 1)) &&
                    valid.Contains((x + 1, y + 1)))
                {
                    if (!countedSquares.Contains((x, y)))
                    {
                        countedSquares.Add((x, y));
                        count++;
                    }
                }
            }

            // Contar cuadrados en el borde (que tienen al menos 2 vértices dentro del área válida)
            foreach (var (x, y) in CookiePoints)
            {
                // Verificar cuadrados adyacentes que pueden estar en el borde
                var candidates = new List<(int x, int y)>
                {
                    (x - 1, y),     // cuadrado a la izquierda
                    (x, y - 1),     // cuadrado arriba
                    (x - 1, y - 1)  // cuadrado diagonal arriba-izquierda
                };

                foreach (var (sqX, sqY) in candidates)
                {
                    if (countedSquares.Contains((sqX, sqY)))
                        continue;

                    // Verificar si este cuadrado tiene al menos 2 vértices dentro del área válida
                    int verticesInside = 0;
                    if (valid.Contains((sqX, sqY))) verticesInside++;
                    if (valid.Contains((sqX + 1, sqY))) verticesInside++;
                    if (valid.Contains((sqX, sqY + 1))) verticesInside++;
                    if (valid.Contains((sqX + 1, sqY + 1))) verticesInside++;

                    // Si tiene al menos 2 vértices dentro y todos están dentro del grid, es un cuadrado válido del borde
                    if (verticesInside >= 2)
                    {
                        bool allInGrid = sqX >= 0 && sqX < GridSize - 1 && 
                                        sqY >= 0 && sqY < GridSize - 1;
                        
                        if (allInGrid)
                        {
                            countedSquares.Add((sqX, sqY));
                            count++;
                        }
                    }
                }
            }

            return count;
        }
    }


}
