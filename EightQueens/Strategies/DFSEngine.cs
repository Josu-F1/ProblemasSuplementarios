using EightQueens.Models;
using EightQueens.Services;

namespace EightQueens.Strategies
{
    /// <summary>
    /// Motor de búsqueda DFS (Depth First Search) genérico con Backtracking
    /// Implementa el patrón de búsqueda en profundidad para resolver problemas de satisfacción de restricciones
    /// </summary>
    /// <typeparam name="T">Tipo de nodo que implementa IGNode</typeparam>
    public class DFSEngine<T> where T : class, IGNode<T>
    {
        /// <summary>
        /// Encuentra todas las soluciones usando DFS con Backtracking
        /// </summary>
        /// <param name="root">Nodo raíz desde donde iniciar la búsqueda</param>
        /// <returns>Lista de todas las soluciones encontradas</returns>
        public List<T> FindAllSolutions(T root)
        {
            var solutions = new List<T>();
            DFSRecursive(root, solutions);
            return solutions;
        }

        /// <summary>
        /// Encuentra la primera solución usando DFS con Backtracking
        /// </summary>
        /// <param name="root">Nodo raíz desde donde iniciar la búsqueda</param>
        /// <returns>Primera solución encontrada o null si no hay solución</returns>
        public T? FindFirstSolution(T root)
        {
            return DFSFirst(root);
        }

        /// <summary>
        /// Implementación recursiva de DFS para encontrar todas las soluciones
        /// </summary>
        /// <param name="current">Nodo actual</param>
        /// <param name="solutions">Lista donde almacenar las soluciones</param>
        private void DFSRecursive(T current, List<T> solutions)
        {
            if (!current.IsValid)
                return; // Poda: no explorar ramas inválidas

            if (current.IsComplete)
            {
                solutions.Add(current);
                return;
            }

            // Expansión en profundidad (FirstChild)
            var child = current.FirstChild();
            while (child != null)
            {
                DFSRecursive(child, solutions); // Recursión en profundidad
                child = child.NextSibling(); // Explorar hermanos (backtracking implícito)
            }
        }

        /// <summary>
        /// Implementación de DFS para encontrar solo la primera solución
        /// </summary>
        /// <param name="current">Nodo actual</param>
        /// <returns>Primera solución o null</returns>
        private T? DFSFirst(T current)
        {
            if (!current.IsValid)
                return null;

            if (current.IsComplete)
                return current;

            // Expansión en profundidad
            var child = current.FirstChild();
            while (child != null)
            {
                var solution = DFSFirst(child);
                if (solution != null)
                    return solution; // Retornar tan pronto como se encuentre una solución

                child = child.NextSibling();
            }

            return null; // No se encontró solución en esta rama
        }
    }

    /// <summary>
    /// Nodo que representa una reina en el problema de las N reinas
    /// Implementa el patrón DFS con nodos genéricos
    /// </summary>
    public class QueenNode : IGNode<QueenNode>
    {
        private readonly IConflictChecker _conflictChecker;
        private readonly int _boardSize;
        private readonly Board _board;
        private readonly int _currentRow;
        private int _currentColumn;

        /// <summary>
        /// Número máximo de filas (tamaño del tablero)
        /// </summary>
        public int MaxRows => _boardSize;

        /// <summary>
        /// Fila actual donde se está colocando la reina
        /// </summary>
        public int Row => _currentRow;

        /// <summary>
        /// Columna actual donde se está colocando la reina
        /// </summary>
        public int Column => _currentColumn;

        /// <summary>
        /// Referencia al nodo padre (reina anterior)
        /// </summary>
        public QueenNode? Parent { get; private set; }

        /// <summary>
        /// Tablero actual con las reinas colocadas
        /// </summary>
        public Board Board => _board;

        /// <summary>
        /// Constructor para nodo raíz (antes de colocar la primera reina)
        /// </summary>
        public QueenNode(int boardSize, IConflictChecker conflictChecker)
        {
            _boardSize = boardSize;
            _conflictChecker = conflictChecker;
            _board = new Board(boardSize);
            _currentRow = -1; // Nodo raíz, no representa una reina real
            _currentColumn = -1;
            Parent = null;
        }

        /// <summary>
        /// Constructor para nodos que representan una reina en una posición específica
        /// </summary>
        private QueenNode(QueenNode parent, int row, int column)
        {
            _boardSize = parent._boardSize;
            _conflictChecker = parent._conflictChecker;
            _board = parent._board.Clone();
            _currentRow = row;
            _currentColumn = column;
            Parent = parent;

            // Colocar la reina en el tablero
            if (row >= 0 && row < _boardSize)
            {
                _board.PlaceQueen(row, column);
            }
        }

        /// <summary>
        /// Verifica si este nodo representa una solución completa
        /// </summary>
        public bool IsComplete => _currentRow == _boardSize - 1;

        /// <summary>
        /// Verifica si la posición actual es válida (no hay conflictos)
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (_currentRow < 0) return true; // Nodo raíz es siempre válido
                return _conflictChecker.IsSafe(_board, _currentRow, _currentColumn);
            }
        }

        /// <summary>
        /// Obtiene el primer hijo: intenta colocar una reina en la siguiente fila
        /// </summary>
        public QueenNode? FirstChild()
        {
            int nextRow = _currentRow + 1;
            
            if (nextRow >= _boardSize)
                return null; // No hay más filas

            // Intentar colocar reina en la primera columna de la siguiente fila
            for (int col = 0; col < _boardSize; col++)
            {
                var childNode = new QueenNode(this, nextRow, col);
                if (childNode.IsValid)
                {
                    return childNode;
                }
            }

            return null; // No se pudo colocar ninguna reina válida en la siguiente fila
        }

        /// <summary>
        /// Obtiene el siguiente hermano: prueba la siguiente columna en la misma fila
        /// </summary>
        public QueenNode? NextSibling()
        {
            if (_currentRow < 0) return null; // El nodo raíz no tiene hermanos
            if (Parent == null) return null;

            // Buscar la siguiente columna válida en la misma fila
            for (int col = _currentColumn + 1; col < _boardSize; col++)
            {
                var siblingNode = new QueenNode(Parent, _currentRow, col);
                if (siblingNode.IsValid)
                {
                    return siblingNode;
                }
            }

            return null; // No hay más columnas válidas en esta fila
        }

        /// <summary>
        /// Obtiene el camino completo desde la raíz hasta este nodo
        /// </summary>
        public List<(int row, int col)> GetSolutionPath()
        {
            var path = new List<(int row, int col)>();
            var current = this;

            while (current != null && current._currentRow >= 0)
            {
                path.Insert(0, (current._currentRow, current._currentColumn));
                current = current.Parent;
            }

            return path;
        }

        /// <summary>
        /// Convierte el nodo a un Board para compatibilidad con el sistema existente
        /// </summary>
        public Board ToBoard()
        {
            return _board.Clone();
        }

        public override string ToString()
        {
            if (_currentRow < 0) return "Root";
            return $"Queen at ({_currentRow}, {_currentColumn})";
        }
    }
}