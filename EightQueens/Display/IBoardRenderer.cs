using EightQueens.Models;

namespace EightQueens.Display
{
    /// <summary>
    /// Interfaz para renderizar el tablero
    /// Principio: Dependency Inversion Principle (DIP)
    /// </summary>
    public interface IBoardRenderer
    {
        void Render(Board board, int solutionNumber);
    }
}
