using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EightQueens.Web.Controllers
{
    public class ExternalController : Controller
    {
        private readonly IConfiguration _configuration;

        public ExternalController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Redirige al juego de la galleta; la URL base se configura en appsettings.json
        public IActionResult CookieGame()
        {
            // Key: CookieGame:BaseUrl (por ejemplo "http://localhost:5147")
            var baseUrl = _configuration["CookieGame:BaseUrl"] ?? "http://localhost:5147";
            var url = baseUrl.TrimEnd('/') + "/Game";
            return Redirect(url);
        }
    }
}
