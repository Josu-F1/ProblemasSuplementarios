using Problema4_Sudoku.Models;

namespace Problema4_Sudoku.Services;

public sealed class SudokuValidator : ISudokuValidator
{
    public bool IsValidPlacement(SudokuBoard b, int row, int col, int value)
    {
        if (value is < 1 or > 9) return false;

        for (int c=0;c<SudokuBoard.Size;c++)
            if (c!=col && b.Get(row,c)==value) return false;

        for (int r=0;r<SudokuBoard.Size;r++)
            if (r!=row && b.Get(r,col)==value) return false;

        int sr=(row/3)*3, sc=(col/3)*3;
        for (int r=sr;r<sr+3;r++)
            for (int c=sc;c<sc+3;c++)
                if ((r!=row || c!=col) && b.Get(r,c)==value) return false;

        return true;
    }

    public bool IsSolved(SudokuBoard b)
    {
        for (int r=0;r<SudokuBoard.Size;r++)
            for (int c=0;c<SudokuBoard.Size;c++)
            {
                var v=b.Get(r,c);
                if (v==0 || !IsValidPlacement(b,r,c,v)) return false;
            }
        return true;
    }
}

