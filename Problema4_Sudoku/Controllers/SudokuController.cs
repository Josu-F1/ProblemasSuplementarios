
using Microsoft.AspNetCore.Mvc;
using Problema4_Sudoku.Models;
using Problema4_Sudoku.Services;
using Problema4_Sudoku.Strategies;


namespace Problema4_Sudoku.Controllers;

public sealed class SudokuController : Controller
{
    private readonly ISudokuValidator _validator;
    private readonly IPuzzleProvider _provider;

    public SudokuController(ISudokuValidator validator, IPuzzleProvider provider)
    {
        _validator = validator;
        _provider = provider;
    }

    public IActionResult Index()
    {
        var board = _provider.GetDemo();
        return View(board);
    }

    [HttpPost]
    public IActionResult LoadDemo(int idx = 0)
    {
        var board = _provider.GetDemo(idx);
        return PartialView("_Board", board);
    }

    public record SolveRequest(int[][] Board);
    public record SolveResponse(bool solved, int[][]? board, string? message);

    [HttpPost]
    public async Task<IActionResult> SolveInstant([FromBody] SolveRequest req, CancellationToken ct)
    {
        var board = SudokuBoard.FromMatrix(req.Board);
        var solver = new SudokuSolver(new BacktrackingStrategy(_validator, stepDelayMs: 0));
        var solved = await solver.SolveAsync(board, null, ct);
        if (solved is null)
            return Json(new SolveResponse(false, null, "No se encontró solución"));

        return Json(new SolveResponse(true, solved.ToMatrix(), null));
    }

    public record StepResponse(bool solved, List<int[][]> steps, string? message);

    [HttpPost]
    public async Task<IActionResult> SolveSteps([FromBody] SolveRequest req, CancellationToken ct)
    {
        var steps = new List<int[][]>();
        var progress = new Progress<SudokuBoard>(b => steps.Add(b.ToMatrix()));
        var board = SudokuBoard.FromMatrix(req.Board);
        var solver = new SudokuSolver(new BacktrackingStrategy(_validator, stepDelayMs: 0));
        var solved = await solver.SolveAsync(board, progress, ct);

        if (solved is null)
            return Json(new StepResponse(false, steps, "No se encontró solución"));

        return Json(new StepResponse(true, steps, null));
    }
}

