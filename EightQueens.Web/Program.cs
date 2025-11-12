var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllersWithViews();

// Ya no se necesitan dependencias inyectadas - el controlador crea las estrategias directamente
// para evitar interacción de consola del QueensSolver

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