using System.IO;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
// Registrar controladores (y agregar controladores del proyecto CookieGame y Sudoku mediante ApplicationPart)
builder.Services.AddControllersWithViews()
    .AddApplicationPart(typeof(Problema3_CookieGame.Controllers.GameController).Assembly)
    .AddApplicationPart(typeof(Problema4_Sudoku.Controllers.SudokuController).Assembly)
    ;

// Configurar ubicaciones de vistas para incluir vistas de otros proyectos
builder.Services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
});

// Registrar el GameService del proyecto CookieGame (era singleton en su propio Program.cs)
builder.Services.AddSingleton<Problema3_CookieGame.Services.GameService>();

// Registrar servicios del Sudoku (DI de servicios - DIP + Strategy)
builder.Services.AddSingleton<Problema4_Sudoku.Services.ISudokuValidator, Problema4_Sudoku.Services.SudokuValidator>();
builder.Services.AddSingleton<Problema4_Sudoku.Services.IPuzzleProvider, Problema4_Sudoku.Services.PuzzleProvider>();

var app = builder.Build();

// Configurar el pipeline de peticiones HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Servir archivos estáticos del propio proyecto
app.UseStaticFiles();

// Además, servir archivos estáticos del proyecto Problema3_CookieGame (para css/js del juego)
var cookieWww = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, "..", "Problema3_CookieGame", "wwwroot"));
if (Directory.Exists(cookieWww))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(cookieWww),
        // Sin RequestPath para permitir las mismas rutas (~/lib, ~/css, ~/js) usadas por la vista del juego
        RequestPath = ""
    });
}

// Servir archivos estáticos del proyecto Problema4_Sudoku
var sudokuWww = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, "..", "Problema4_Sudoku", "wwwroot"));
if (Directory.Exists(sudokuWww))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(sudokuWww),
        RequestPath = ""
    });
}

app.UseRouting();
app.UseAuthorization();

// Configurar rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=EightQueens}/{action=Index}/{id?}");

app.Run();