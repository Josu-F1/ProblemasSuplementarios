using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Problema4_Sudoku.Models;
using Problema4_Sudoku.Services;

namespace Problema4_Sudoku.Strategies;

// Backtracking + MRV (elige la celda vacía con menos candidatos)
public sealed class BacktrackingStrategy : ISearchStrategy
{
    private readonly ISudokuValidator _validator;
    private readonly int _stepDelayMs;

    public BacktrackingStrategy(ISudokuValidator validator, int stepDelayMs = 0)
    {
        _validator = validator;
        _stepDelayMs = stepDelayMs;
    }

    public Task<SudokuBoard?> SolveAsync(
        SudokuBoard start,
        IProgress<SudokuBoard>? stepProgress,
        CancellationToken ct)
    {
        return Task.Run(() =>
        {
            var board = start.Clone();
            return Backtrack(board, stepProgress, ct) ? board : null;
        }, ct);
    }

    private bool Backtrack(SudokuBoard b, IProgress<SudokuBoard>? progress, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        (int r, int c, List<int> cand)? next = null;
        for (int r=0;r<SudokuBoard.Size;r++)
        {
            for (int c=0;c<SudokuBoard.Size;c++)
            {
                if (b.Get(r,c) != 0) continue;
                var cand = Enumerable.Range(1,9)
                    .Where(v => _validator.IsValidPlacement(b,r,c,v))
                    .ToList();
                if (cand.Count == 0) return false;
                if (next is null || cand.Count < next.Value.cand.Count)
                    next = (r,c,cand);
                if (next.Value.cand.Count == 1) break;
            }
            if (next?.cand.Count == 1) break;
        }

        if (next is null) return _validator.IsSolved(b);

        foreach (var v in next.Value.cand)
        {
            ct.ThrowIfCancellationRequested();
            b.Set(next.Value.r, next.Value.c, v);
            progress?.Report(b.Clone());
            if (_stepDelayMs > 0) Thread.Sleep(_stepDelayMs);

            if (Backtrack(b,progress,ct)) return true;

            b.Set(next.Value.r, next.Value.c, 0);
            progress?.Report(b.Clone());
            if (_stepDelayMs > 0) Thread.Sleep(_stepDelayMs);
        }
        return false;
    }
}
