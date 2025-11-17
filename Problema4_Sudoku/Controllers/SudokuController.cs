
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
    public record ValidateRequest(int[][] Board);
    public record CellError(int r, int c, string reason);
    public record ValidateResponse(bool valid, bool solved, List<CellError> errors);

    [HttpPost]
    public IActionResult Validate([FromBody] ValidateRequest req)
    {
        var board = SudokuBoard.FromMatrix(req.Board);
        var errors = new List<CellError>();

        // Revisa cada celda no-vacía con las reglas del validador
        for (int r = 0; r < SudokuBoard.Size; r++)
        {
            for (int c = 0; c < SudokuBoard.Size; c++)
            {
                var v = board.Get(r, c);
                if (v == 0) continue;

                // Temporalmente vacía la celda para no “verse a sí misma”
                board.Set(r, c, 0);
                var isOk = _validator.IsValidPlacement(board, r, c, v);
                board.Set(r, c, v);

                if (!isOk)
                    errors.Add(new CellError(r, c, "Conflicto en fila/columna/cuadrante"));
            }
        }

        var solved = errors.Count == 0 && _validator.IsSolved(board);
        var valid = errors.Count == 0;

        return Json(new ValidateResponse(valid, solved, errors));
    }

}

