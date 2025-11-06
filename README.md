# Problemas Suplementarios - Inteligencia Artificial con C#

## 📋 Descripción

Solución al **Problema de las 8 Reinas** utilizando:
- ✅ **C#** y **.NET 8**
- ✅ **Programación Orientada a Objetos (POO)**
- ✅ **Principios SOLID**
- ✅ **Patrón de Diseño Strategy**
- ✅ **Algoritmo de IA: Backtracking**
- ✅ **Código limpio y buenas prácticas**

---

## 🎯 Problema de las 8 Reinas

El **problema de las ocho reinas** es un acertijo clásico de ajedrez propuesto por Max Bezzel en 1848. Consiste en colocar 8 reinas en un tablero de ajedrez 8×8 de tal manera que **ninguna reina amenace a otra**.

Una reina amenaza a cualquier pieza que se encuentre en:
- Su misma **fila**
- Su misma **columna**
- Sus **diagonales**

---

## 🏗️ Arquitectura del Proyecto

```
EightQueens/
│
├── Models/
│   └── Board.cs                    # Modelo del tablero
│
├── Services/
│   ├── IConflictChecker.cs         # Interfaz para validación
│   ├── QueenConflictChecker.cs     # Validador de conflictos
│   └── QueensSolver.cs             # Contexto del patrón Strategy
│
├── Strategies/
│   ├── ISolverStrategy.cs          # Interfaz Strategy
│   └── BacktrackingSolver.cs       # Implementación Backtracking
│
├── Display/
│   ├── IBoardRenderer.cs           # Interfaz para renderizado
│   └── ConsoleRenderer.cs          # Renderizador de consola
│
└── Program.cs                      # Punto de entrada
```

---

## 🎨 Principios SOLID Aplicados

### 1. **SRP (Single Responsibility Principle)**
Cada clase tiene una única responsabilidad:
- `Board`: Gestiona el estado del tablero
- `QueenConflictChecker`: Valida conflictos entre reinas
- `BacktrackingSolver`: Implementa el algoritmo de backtracking
- `ConsoleRenderer`: Renderiza el tablero en consola

### 2. **OCP (Open/Closed Principle)**
El sistema está abierto a extensión pero cerrado a modificación:
- Se pueden agregar nuevas estrategias de resolución sin modificar código existente
- Se pueden agregar nuevos renderizadores sin cambiar la lógica del solver

### 3. **DIP (Dependency Inversion Principle)**
Las clases dependen de abstracciones, no de implementaciones concretas:
- `QueensSolver` depende de `ISolverStrategy`
- `BacktrackingSolver` depende de `IConflictChecker`

---

## 🔧 Patrón de Diseño: Strategy

El **patrón Strategy** permite:
- Definir una familia de algoritmos (diferentes estrategias de resolución)
- Encapsular cada algoritmo
- Hacer que sean intercambiables

**Implementación:**
```csharp
ISolverStrategy strategy = new BacktrackingSolver(conflictChecker);
QueensSolver solver = new QueensSolver(strategy);
// Se puede cambiar la estrategia en tiempo de ejecución
solver.SetStrategy(new OtraEstrategia());
```

---

## 🧠 Algoritmo: Backtracking (Vuelta Atrás)

El **Backtracking** es un algoritmo de IA que funciona así:

1. **Exploración**: Intenta colocar una reina en cada fila
2. **Validación**: Verifica que no amenace a otras reinas
3. **Recursión**: Avanza a la siguiente fila si la posición es válida
4. **Vuelta atrás**: Si no hay solución, retrocede y prueba otra posición

### Pseudocódigo:
```
function Backtracking(tablero, fila):
    si fila == N:
        guardar_solución(tablero)
        return
    
    para cada columna en [0..N-1]:
        si es_seguro(tablero, fila, columna):
            colocar_reina(fila, columna)
            Backtracking(tablero, fila + 1)
            remover_reina(fila, columna)  # Backtracking
```

### Complejidad:
- **Temporal**: O(N!) - explora todas las permutaciones posibles
- **Espacial**: O(N) - profundidad de la recursión

---

## 🚀 Compilación y Ejecución

### Requisitos Previos
- **.NET 8 SDK** instalado

### Compilar el proyecto
```powershell
cd EightQueens
dotnet build
```

### Ejecutar el programa
```powershell
dotnet run
```

---

## 💻 Uso del Programa

El programa ofrece un menú interactivo:

1. **Resolver problema de 8 reinas**: Encuentra todas las 92 soluciones posibles
2. **Resolver problema de N reinas**: Personaliza el tamaño del tablero (4-12)
3. **Mostrar información del algoritmo**: Detalles sobre Backtracking
4. **Salir**: Cierra el programa

### Ejemplo de salida:
```
═══════════════════════════════════════
        SOLUCIÓN #1
═══════════════════════════════════════

    0  1  2  3  4  5  6  7 
   ┌───┬───┬───┬───┬───┬───┬───┬───┐
 0 │ ♛ │   │ · │   │ · │   │ · │   │
   ├───┼───┼───┼───┼───┼───┼───┼───┤
 1 │   │ · │   │ · │   │ ♛ │   │ · │
   ├───┼───┼───┼───┼───┼───┼───┼───┤
 2 │ · │   │ · │   │ ♛ │   │ · │   │
   ...

Posiciones (fila,columna): (0,0), (1,4), (2,7), ...
```

---

## 📊 Resultados

Para un tablero de 8×8, el algoritmo encuentra:
- **92 soluciones únicas**
- Tiempo de ejecución: < 50ms (dependiendo del hardware)

---

## 🎓 Conceptos de POO Aplicados

- **Encapsulamiento**: Propiedades privadas con acceso controlado
- **Abstracción**: Interfaces que definen contratos
- **Polimorfismo**: Múltiples implementaciones de las interfaces
- **Composición**: Las clases se componen de otras clases

---

## 📚 Referencias

- **Max Bezzel** (1848) - Propuesta original del problema
- **Algoritmo Backtracking** - Técnica de IA para búsqueda exhaustiva
- **Principios SOLID** - Robert C. Martin
- **Patrón Strategy** - Gang of Four (GoF)

---

## 👥 Autor

Desarrollado como parte de los Problemas Suplementarios - Grupo de IA con C#

---

## 📝 Licencia

Este proyecto es de uso educativo.
