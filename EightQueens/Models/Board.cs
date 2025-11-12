namespace EightQueens.Models
{
    /// <summary>
    /// Representa el tablero de ajedrez para el problema de las 8 reinas
    /// Implementa SRP: responsabilidad única de manejar el estado del tablero
    /// </summary>
    public class Board
    {
        private readonly int[,] _board;
        private readonly int _size;

        public int Size => _size;
        public int[,] BoardArray => (int[,])_board.Clone();

        public Board(int size = 8)
        {
            _size = size;
            _board = new int[size, size];
        }

        public Board(Board other)
        {
            _size = other._size;
            _board = (int[,])other._board.Clone();
        }

        public bool PlaceQueen(int row, int col)
        {
            if (row < 0 || row >= _size || col < 0 || col >= _size)
                return false;

            _board[row, col] = 1;
            return true;
        }

        public bool RemoveQueen(int row, int col)
        {
            if (row < 0 || row >= _size || col < 0 || col >= _size)
                return false;

            _board[row, col] = 0;
            return true;
        }

        public bool HasQueen(int row, int col)
        {
            if (row < 0 || row >= _size || col < 0 || col >= _size)
                return false;

            return _board[row, col] == 1;
        }

        public void Clear()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _board[i, j] = 0;
                }
            }
        }

        public List<(int row, int col)> GetQueenPositions()
        {
            var positions = new List<(int row, int col)>();
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_board[i, j] == 1)
                    {
                        positions.Add((i, j));
                    }
                }
            }
            return positions;
        }

        public override string ToString()
        {
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    result.Append(_board[i, j] == 1 ? "♛ " : "· ");
                }
                result.AppendLine();
            }
            return result.ToString();
        }
    }
}