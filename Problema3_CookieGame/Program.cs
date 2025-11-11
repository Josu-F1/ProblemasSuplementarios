using Problema3_CookieGame.Services;

namespace Problema3_CookieGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<GameService>();

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Game}/{action=Index}/{id?}");

            app.Run();

        }
    }
}
