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

}
