using System;
using System.Threading;
using System.Threading.Tasks;
using Problema4_Sudoku.Models;
using Problema4_Sudoku.Strategies;

namespace Problema4_Sudoku.Services;

public sealed class SudokuSolver : ISudokuSolver
{
    private readonly ISearchStrategy _strategy;
    public SudokuSolver(ISearchStrategy strategy) => _strategy = strategy;

    public Task<SudokuBoard?> SolveAsync(
        SudokuBoard board,
        IProgress<SudokuBoard>? stepProgress = null,
        CancellationToken ct = default)
        => _strategy.SolveAsync(board, stepProgress, ct);
}
