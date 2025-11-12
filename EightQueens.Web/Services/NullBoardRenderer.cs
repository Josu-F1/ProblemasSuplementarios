using EightQueens.Models;
using EightQueens.Display;

namespace EightQueens.Web.Services
{
    /// <summary>
    /// Renderer nulo para uso en aplicaciones web donde no se necesita salida de consola
    /// </summary>
    public class NullBoardRenderer : IBoardRenderer
    {
        public void Render(Board board)
        {
            // No hace nada - es para uso en web donde no se necesita output de consola
        }
    }
}