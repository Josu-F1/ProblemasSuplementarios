namespace EightQueens.Strategies
{
    /// <summary>
    /// Interfaz genérica para nodos en un grafo de búsqueda DFS
    /// Patrón: Template Method + Strategy
    /// </summary>
    /// <typeparam name="T">Tipo del nodo</typeparam>
    public interface IGNode<T> where T : class
    {
        /// <summary>
        /// Obtiene el primer hijo (próxima expansión) del nodo actual
        /// </summary>
        /// <returns>Primer hijo válido o null si no hay expansión posible</returns>
        T? FirstChild();

        /// <summary>
        /// Obtiene el siguiente hermano (alternativa) del nodo actual
        /// </summary>
        /// <returns>Siguiente hermano válido o null si no hay más alternativas</returns>
        T? NextSibling();

        /// <summary>
        /// Verifica si el nodo representa una solución completa
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// Verifica si el estado del nodo es válido
        /// </summary>
        bool IsValid { get; }
    }
}