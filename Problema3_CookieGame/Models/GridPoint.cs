namespace Problema3_CookieGame.Models;

public class GridPoint
{
    public int X { get; set; } // grid row
    public int Y { get; set; } // grid col

    public GridPoint() { }
    public GridPoint(int x, int y) { X = x; Y = y; }

    public override bool Equals(object obj)
    {
        return obj is GridPoint p && p.X == X && p.Y == Y;
    }

    public override int GetHashCode() => (X, Y).GetHashCode();
}
