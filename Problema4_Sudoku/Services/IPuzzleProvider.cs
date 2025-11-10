using Problema4_Sudoku.Models;

namespace Problema4_Sudoku.Services;

public interface IPuzzleProvider
{
    SudokuBoard GetDemo(int idx=0);
}

