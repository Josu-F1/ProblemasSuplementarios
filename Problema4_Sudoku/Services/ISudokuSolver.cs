using System;
using System.Threading;
using System.Threading.Tasks;
using Problema4_Sudoku.Models;

namespace Problema4_Sudoku.Services;

public interface ISudokuSolver
{
    Task<SudokuBoard?> SolveAsync(
        SudokuBoard board,
        IProgress<SudokuBoard>? stepProgress = null,
        CancellationToken ct = default);
}

