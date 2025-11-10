using EightQueens.Models;

namespace EightQueens.Web.Models
{
    /// <summary>
    /// ViewModel para la página principal de las 8 reinas
    /// Patrón: MVVM (Model-View-ViewModel)
    /// </summary>
    public class EightQueensViewModel
    {
        /// <summary>
        /// Lista de todas las soluciones encontradas
        /// </summary>
        public List<BoardViewModel> Solutions { get; set; } = new();

        /// <summary>
        /// Índice de la solución actual siendo mostrada
        /// </summary>
        public int CurrentSolutionIndex { get; set; } = 0;

        /// <summary>
        /// Número total de soluciones
        /// </summary>
        public int TotalSolutions => Solutions.Count;

        /// <summary>
        /// Tiempo que tomó resolver el problema
        /// </summary>
        public double ExecutionTimeMs { get; set; }

        /// <summary>
        /// Tamaño del tablero (por defecto 8x8)
        /// </summary>
        public int BoardSize { get; set; } = 8;

        /// <summary>
        /// Algoritmo utilizado para resolver
        /// </summary>
        public string Algorithm { get; set; } = "Backtracking";

        /// <summary>
        /// Estado del proceso de resolución
        /// </summary>
        public bool IsSolved { get; set; } = false;

        /// <summary>
        /// Mensaje de estado para mostrar al usuario
        /// </summary>
        public string StatusMessage { get; set; } = "";
    }

    /// <summary>
    /// ViewModel para representar un tablero de ajedrez
    /// </summary>
    public class BoardViewModel
    {
        /// <summary>
        /// Tamaño del tablero
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Posiciones de las reinas [fila] = columna
        /// </summary>
        public int[] QueenPositions { get; set; } = Array.Empty<int>();

        /// <summary>
        /// Número de la solución
        /// </summary>
        public int SolutionNumber { get; set; }

        /// <summary>
        /// Constructor desde el modelo Board
        /// </summary>
        public BoardViewModel(Board board, int solutionNumber)
        {
            Size = board.Size;
            QueenPositions = board.Queens;
            SolutionNumber = solutionNumber;
        }

        /// <summary>
        /// Constructor vacío para el model binding
        /// </summary>
        public BoardViewModel() { }

        /// <summary>
        /// Verifica si hay una reina en la posición especificada
        /// </summary>
        public bool HasQueenAt(int row, int column)
        {
            return row < QueenPositions.Length && QueenPositions[row] == column;
        }

        /// <summary>
        /// Obtiene las coordenadas de todas las reinas como string
        /// </summary>
        public string GetQueenCoordinates()
        {
            var coordinates = new List<string>();
            for (int i = 0; i < QueenPositions.Length; i++)
            {
                coordinates.Add($"({i},{QueenPositions[i]})");
            }
            return string.Join(", ", coordinates);
        }
    }
}