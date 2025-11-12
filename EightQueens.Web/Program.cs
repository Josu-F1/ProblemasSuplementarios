using EightQueens.Services;
using EightQueens.Strategies;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Registrar dependencias del dominio de EightQueens
builder.Services.AddScoped<IConflictChecker, QueenConflictChecker>();
builder.Services.AddScoped<ISolverStrategy, BacktrackingSolver>();
builder.Services.AddScoped<QueensSolver>();

var app = builder.Build();

// Configurar el pipeline de peticiones HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configurar rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=EightQueens}/{action=Index}/{id?}");

app.Run();