namespace EightQueens.Models
{
    /// <summary>
    /// Representa el tablero de ajedrez con las reinas colocadas
    /// Responsabilidad: Gestionar el estado del tablero
    /// </summary>
    public class Board
    {
        private readonly int _size;
        private readonly int[] _queens;

        public int Size => _size;
        public int[] Queens => (int[])_queens.Clone();

        public Board(int size = 8)
        {
            _size = size;
            _queens = new int[size];
            for (int i = 0; i < size; i++)
            {
                _queens[i] = -1; // -1 indica que no hay reina en esa fila
            }
        }

        /// <summary>
        /// Coloca una reina en la posición especificada
        /// </summary>
        public void PlaceQueen(int row, int column)
        {
            if (row < 0 || row >= _size || column < 0 || column >= _size)
                throw new ArgumentOutOfRangeException("Posición fuera del tablero");

            _queens[row] = column;
        }

        /// <summary>
        /// Remueve la reina de una fila específica
        /// </summary>
        public void RemoveQueen(int row)
        {
            if (row < 0 || row >= _size)
                throw new ArgumentOutOfRangeException("Fila fuera del tablero");

            _queens[row] = -1;
        }

        /// <summary>
        /// Obtiene la columna donde está colocada la reina en una fila
        /// </summary>
        public int GetQueenColumn(int row)
        {
            if (row < 0 || row >= _size)
                throw new ArgumentOutOfRangeException("Fila fuera del tablero");

            return _queens[row];
        }

        /// <summary>
        /// Verifica si hay una reina en la posición especificada
        /// </summary>
        public bool HasQueenAt(int row, int column)
        {
            return _queens[row] == column;
        }

        /// <summary>
        /// Clona el tablero actual
        /// </summary>
        public Board Clone()
        {
            var newBoard = new Board(_size);
            for (int i = 0; i < _size; i++)
            {
                newBoard._queens[i] = _queens[i];
            }
            return newBoard;
        }
    }
}
