using EightQueens.Models;

namespace EightQueens.Display
{
    /// <summary>
    /// Renderizador de tablero para consola
    /// Responsabilidad: Mostrar visualmente el tablero en la consola
    /// </summary>
    public class ConsoleRenderer : IBoardRenderer
    {
        /// <summary>
        /// Renderiza el tablero en la consola con formato visual
        /// </summary>
        public void Render(Board board, int solutionNumber)
        {
            Console.WriteLine($"\n═══════════════════════════════════════");
            Console.WriteLine($"        SOLUCIÓN #{solutionNumber}");
            Console.WriteLine($"═══════════════════════════════════════\n");

            // Mostrar coordenadas superiores
            Console.Write("   ");
            for (int i = 0; i < board.Size; i++)
            {
                Console.Write($" {i} ");
            }
            Console.WriteLine();

            // Línea superior del tablero
            Console.Write("   ┌");
            for (int i = 0; i < board.Size; i++)
            {
                Console.Write("───");
                if (i < board.Size - 1) Console.Write("┬");
            }
            Console.WriteLine("┐");

            // Filas del tablero
            for (int row = 0; row < board.Size; row++)
            {
                Console.Write($" {row} │");
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.HasQueenAt(row, col))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" ♛ ");
                        Console.ResetColor();
                    }
                    else
                    {
                        // Patrón de tablero de ajedrez
                        if ((row + col) % 2 == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write(" · ");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write("   ");
                        }
                    }
                    Console.Write("│");
                }
                Console.WriteLine();

                // Líneas intermedias o inferior
                if (row < board.Size - 1)
                {
                    Console.Write("   ├");
                    for (int i = 0; i < board.Size; i++)
                    {
                        Console.Write("───");
                        if (i < board.Size - 1) Console.Write("┼");
                    }
                    Console.WriteLine("┤");
                }
            }

            // Línea inferior del tablero
            Console.Write("   └");
            for (int i = 0; i < board.Size; i++)
            {
                Console.Write("───");
                if (i < board.Size - 1) Console.Write("┴");
            }
            Console.WriteLine("┘");

            // Mostrar posiciones en formato compacto
            Console.Write("\nPosiciones (fila,columna): ");
            for (int i = 0; i < board.Size; i++)
            {
                Console.Write($"({i},{board.GetQueenColumn(i)})");
                if (i < board.Size - 1) Console.Write(", ");
            }
            Console.WriteLine("\n");
        }
    }
}
