using EightQueens.Models;

namespace EightQueens.Display
{
    /// <summary>
    /// Renderizador de consola para el tablero de ajedrez
    /// Implementa SRP: responsabilidad única de mostrar el tablero
    /// </summary>
    public class ConsoleBoardRenderer : IBoardRenderer
    {
        public void Render(Board board)
        {
            Console.WriteLine();
            
            // Mostrar números de columna
            Console.Write("   ");
            for (int col = 0; col < board.Size; col++)
            {
                Console.Write($" {col + 1} ");
            }
            Console.WriteLine();

            // Mostrar línea superior
            Console.Write("   ");
            for (int col = 0; col < board.Size; col++)
            {
                Console.Write("---");
            }
            Console.WriteLine();

            // Mostrar filas del tablero
            for (int row = 0; row < board.Size; row++)
            {
                Console.Write($"{row + 1} |");
                
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.HasQueen(row, col))
                    {
                        Console.Write(" ♛ ");
                    }
                    else
                    {
                        // Patrón de tablero de ajedrez
                        Console.Write((row + col) % 2 == 0 ? " □ " : " ■ ");
                    }
                }
                Console.WriteLine($"| {row + 1}");
            }

            // Mostrar línea inferior
            Console.Write("   ");
            for (int col = 0; col < board.Size; col++)
            {
                Console.Write("---");
            }
            Console.WriteLine();

            // Mostrar números de columna nuevamente
            Console.Write("   ");
            for (int col = 0; col < board.Size; col++)
            {
                Console.Write($" {col + 1} ");
            }
            Console.WriteLine();

            // Mostrar posiciones de las reinas
            var queens = board.GetQueenPositions();
            if (queens.Count > 0)
            {
                Console.WriteLine($"\n👑 Posiciones de las reinas: ");
                for (int i = 0; i < queens.Count; i++)
                {
                    Console.Write($"({queens[i].row + 1},{queens[i].col + 1})");
                    if (i < queens.Count - 1) Console.Write(", ");
                }
                Console.WriteLine();
            }
        }
    }
}