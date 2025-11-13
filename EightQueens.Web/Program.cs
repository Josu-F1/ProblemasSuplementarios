using System.IO;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
// Registrar controladores (y agregar controladores del proyecto CookieGame mediante ApplicationPart)
builder.Services.AddControllersWithViews()
    .AddApplicationPart(typeof(Problema3_CookieGame.Controllers.GameController).Assembly)
    ;

// Registrar el GameService del proyecto CookieGame (era singleton en su propio Program.cs)
builder.Services.AddSingleton<Problema3_CookieGame.Services.GameService>();

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

app.UseRouting();
app.UseAuthorization();

// Configurar rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=EightQueens}/{action=Index}/{id?}");

app.Run();