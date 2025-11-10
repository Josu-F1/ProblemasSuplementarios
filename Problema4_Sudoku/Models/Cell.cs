namespace Problema4_Sudoku.Models;

public sealed class Cell
{
    public int Row { get; }
    public int Col { get; }
    public int Value { get; set; } // 0 = vacío
    public bool IsFixed { get; }

    public Cell(int row, int col, int value, bool isFixed = false)
    {
        Row = row; Col = col; Value = value;
        IsFixed = isFixed && value != 0;
    }

    public Cell Clone() => new(Row, Col, Value, IsFixed);
}
