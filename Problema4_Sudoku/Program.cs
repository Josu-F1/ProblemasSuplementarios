namespace Problema4_Sudoku
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// DI de servicios (DIP + Strategy)
builder.Services.AddSingleton<Problema4_Sudoku.Services.ISudokuValidator, Problema4_Sudoku.Services.SudokuValidator>();
builder.Services.AddSingleton<Problema4_Sudoku.Services.IPuzzleProvider, Problema4_Sudoku.Services.PuzzleProvider>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sudoku}/{action=Index}/{id?}");

app.Run();
        }
    }
}
