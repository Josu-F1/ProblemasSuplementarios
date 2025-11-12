using Problema4_Sudoku.Models;

namespace Problema4_Sudoku.Services;

public interface ISudokuValidator
{
    bool IsValidPlacement(SudokuBoard board, int row, int col, int value);
    bool IsSolved(SudokuBoard board);
}

