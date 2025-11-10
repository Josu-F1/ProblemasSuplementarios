using System.Collections.Generic;
using System.Linq;

namespace Problema4_Sudoku.Models;

public sealed class SudokuBoard
{
    public const int Size = 9;
    public Cell[,] Cells { get; }

    public SudokuBoard()
    {
        Cells = new Cell[Size, Size];
        for (int r=0; r<Size; r++)
            for (int c=0; c<Size; c++)
                Cells[r,c] = new Cell(r,c,0,false);
    }

    public SudokuBoard(int[,] matrix, bool fixedAsGiven = true) : this()
    {
        for (int r=0; r<Size; r++)
            for (int c=0; c<Size; c++)
                Cells[r,c] = new Cell(r,c, matrix[r,c], fixedAsGiven && matrix[r,c]!=0);
    }

    public int Get(int r,int c) => Cells[r,c].Value;
    public void Set(int r,int c,int v) => Cells[r,c].Value = v;

    public SudokuBoard Clone()
    {
        var copy = new SudokuBoard();
        for (int r=0;r<Size;r++)
            for (int c=0;c<Size;c++)
                copy.Cells[r,c] = Cells[r,c].Clone();
        return copy;
    }

    public int[][] ToMatrix()
        => Enumerable.Range(0, Size)
            .Select(r => Enumerable.Range(0, Size).Select(c => Cells[r,c].Value).ToArray())
            .ToArray();

    public static SudokuBoard FromMatrix(int[][] m, bool fixedAsGiven=false)
    {
        var board = new SudokuBoard();
        for (int r=0; r<Size; r++)
            for (int c=0; c<Size; c++)
                board.Cells[r,c] = new Cell(r,c, m[r][c], fixedAsGiven && m[r][c]!=0);
        return board;
    }
}

