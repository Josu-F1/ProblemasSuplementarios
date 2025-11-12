using System;
using System.Threading;
using System.Threading.Tasks;
using Problema4_Sudoku.Models;

namespace Problema4_Sudoku.Strategies;

public interface ISearchStrategy
{
    Task<SudokuBoard?> SolveAsync(
        SudokuBoard start,
        IProgress<SudokuBoard>? stepProgress,
        CancellationToken ct);
}

