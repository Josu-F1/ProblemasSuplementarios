using EightQueens.Models;

namespace EightQueens.Display
{
    /// <summary>
    /// Interface para renderizar el tablero
    /// Implementa DIP: depende de abstracciones
    /// </summary>
    public interface IBoardRenderer
    {
        void Render(Board board);
    }
}