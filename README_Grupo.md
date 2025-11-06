# 🎓 PROBLEMAS SUPLEMENTARIOS - TRABAJO EN GRUPO

## 👥 Información del Equipo

**Repositorio:** ProblemasSuplementarios  
**Propietario:** Josu-F1  
**Fecha:** Noviembre 2025

---

## 📋 Descripción del Proyecto

Resolución de problemas de **Inteligencia Artificial con C#** aplicando:

- ✅ **Programación Orientada a Objetos (POO)**
- ✅ **Principios SOLID**
- ✅ **Patrones de Diseño**
- ✅ **Algoritmos de IA**
- ✅ **Código limpio y buenas prácticas**

---

## 🗂️ Estructura del Repositorio

```
ProblemasSuplementarios/
│
├── EightQueens/                   # Problema 1: 8 Reinas (Josu)
│   ├── Models/
│   ├── Services/
│   ├── Strategies/
│   ├── Display/
│   ├── Examples/
│   └── Program.cs
│
├── Problema2_8Puzzle/             # Problema 2: 8 Puzzle (Marlon Guevara)
│   └── ...
│
├── Problema3_JuegoGalleta/        # Problema 3: Juego de la Galleta (Hiedi)
│   └── ...
│
├── Problema4_Sudoku/              # Problema 4: Sudoku (Viviana)
│   └── ...
│
├── README.md                      # README principal del proyecto
├── README_Grupo.md                # Este archivo (guía de colaboración)
├── DOCUMENTACION_TECNICA.md       # Documentación técnica general
└── .gitignore
```

---

## 🌿 Estrategia de Ramas (Branching Strategy)

Cada integrante trabaja en **su propia rama** y **su propia carpeta**:

### Nomenclatura de Ramas:

```
main                           # Rama principal (protegida)
├── ramaJosu                   # Problema 1: 8 Reinas (Josu)
├── ramaMarlon                 # Problema 2: 8 Puzzle (Marlon Guevara)
├── ramaHiedi                  # Problema 3: Juego de la Galleta (Hiedi)
└── ramaViviana                # Problema 4: Sudoku (Viviana)
```

---

## 🚀 Guía de Trabajo para Integrantes

### 📝 PASO 1: Clonar el Repositorio

```bash
# Clonar el repositorio
git clone https://github.com/Josu-F1/ProblemasSuplementarios.git

# Entrar al directorio
cd ProblemasSuplementarios
```

### 🌿 PASO 2: Crear Tu Rama Personal

```bash
# Crear y cambiar a tu rama (reemplaza [TuNombre] con tu nombre)
git checkout -b rama[TuNombre]

# Ejemplo:
git checkout -b ramaCarlos
git checkout -b ramaMaria
```

### 📁 PASO 3: Crear Tu Carpeta de Trabajo

```bash
# Crear tu carpeta para el problema asignado
mkdir Problema[Numero]_[NombreProblema]

# Ejemplo:
mkdir Problema2_Sudoku
mkdir Problema3_Laberinto

# Entrar a tu carpeta
cd Problema[Numero]_[NombreProblema]
```

### 💻 PASO 4: Desarrollar Tu Solución

Dentro de tu carpeta, organiza tu proyecto con la siguiente estructura:

```
Problema[X]_[Nombre]/
│
├── Models/                 # Clases de modelo/dominio
├── Services/              # Servicios y lógica de negocio
├── Strategies/            # Implementación del patrón Strategy
├── Display/               # Renderizado/Visualización
├── Program.cs             # Punto de entrada
├── [NombreProyecto].csproj
└── README.md              # Documentación específica de tu problema
```

**Requisitos obligatorios:**
- ✅ POO (Clases, herencia, polimorfismo, encapsulamiento)
- ✅ Principios SOLID aplicados
- ✅ Al menos 1 patrón de diseño
- ✅ Algoritmo de IA
- ✅ Código limpio y comentado

### 💾 PASO 5: Hacer Commits Regulares

```bash
# Ver archivos modificados
git status

# Agregar archivos al staging
git add .

# Hacer commit con mensaje descriptivo
git commit -m "feat: implementar algoritmo de backtracking"

# Más ejemplos de mensajes:
git commit -m "feat: agregar clase Board con validaciones"
git commit -m "fix: corregir validación de diagonales"
git commit -m "docs: agregar documentación del algoritmo"
git commit -m "refactor: aplicar principio SRP en ConflictChecker"
```

**Convenciones de commits:**
- `feat:` Nueva funcionalidad
- `fix:` Corrección de bug
- `docs:` Documentación
- `refactor:` Refactorización sin cambiar funcionalidad
- `test:` Agregar o modificar tests
- `style:` Cambios de formato/estilo

### ⬆️ PASO 6: Subir Cambios a Tu Rama

```bash
# Subir cambios a tu rama remota
git push origin rama[TuNombre]

# Primera vez (establecer upstream):
git push -u origin rama[TuNombre]

# Siguientes veces:
git push
```

### 🔄 PASO 7: Mantener Tu Rama Actualizada

```bash
# Obtener últimos cambios del repositorio
git fetch origin

# Ver qué ramas existen
git branch -a

# Actualizar tu rama con cambios de main (si los hay)
git checkout rama[TuNombre]
git merge origin/main
```

### 🎯 PASO 8: Preparar para el Merge Final

Antes del merge final, asegúrate de:

1. ✅ Tu código compila sin errores
2. ✅ El programa ejecuta correctamente
3. ✅ Tienes documentación (README.md en tu carpeta)
4. ✅ Aplicaste principios SOLID
5. ✅ Implementaste al menos un patrón de diseño
6. ✅ El código está comentado y limpio

```bash
# Verificar que todo compile
dotnet build

# Ejecutar tu programa
dotnet run

# Ver el estado de tu rama
git status

# Ver historial de commits
git log --oneline
```

---

## 🔀 Proceso de Merge Final

### Para el Coordinador del Grupo:

```bash
# 1. Asegurarse de estar en main
git checkout main
git pull origin main

# 2. Hacer merge de cada rama
git merge rama[Integrante1] --no-ff -m "Merge: Problema 1 - Ocho Reinas"
git merge rama[Integrante2] --no-ff -m "Merge: Problema 2 - [Nombre]"
git merge rama[Integrante3] --no-ff -m "Merge: Problema 3 - [Nombre]"
git merge rama[Integrante4] --no-ff -m "Merge: Problema 4 - [Nombre]"

# 3. Resolver conflictos si existen (debería haber pocos o ninguno)

# 4. Subir los cambios
git push origin main
```

---

## 🎨 Ejemplo Completo: Problema de las 8 Reinas

**Rama:** `ramaJosu`  
**Carpeta:** `Problema1_OchoReinas/` → `EightQueens/`  

**Características implementadas:**
- ✅ Algoritmo: Backtracking (IA)
- ✅ Patrón: Strategy
- ✅ SOLID: SRP, OCP, DIP
- ✅ POO completa
- ✅ Renderizado visual en consola
- ✅ 92 soluciones encontradas en ~40ms

**Archivos creados:**
- `Models/Board.cs`
- `Services/IConflictChecker.cs`
- `Services/QueenConflictChecker.cs`
- `Services/QueensSolver.cs`
- `Strategies/ISolverStrategy.cs`
- `Strategies/BacktrackingSolver.cs`
- `Display/IBoardRenderer.cs`
- `Display/ConsoleRenderer.cs`
- `Program.cs`

---

## 📚 Problemas Asignados al Grupo

### Problema 1: ♟️ Las 8 Reinas (Josu)
- **Descripción:** Colocar 8 reinas en un tablero de ajedrez sin que se amenacen
- **Algoritmo:** Backtracking (Vuelta Atrás)
- **Patrón:** Strategy
- **Integrante:** Josu
- **Rama:** `ramaJosu`
- **Carpeta:** `EightQueens/`
- **Estado:** ✅ Completado

### Problema 2: 🧩 8 Puzzle (Marlon Guevara)
- **Descripción:** Resolver el rompecabezas deslizante de 8 piezas
  - Permitir que el usuario resuelva interactivamente el problema
  - Hacer que el computador resuelva el problema automáticamente
- **Algoritmo:** Métodos de búsqueda (A*, BFS, etc.)
- **Patrón:** Strategy o State
- **Integrante:** Marlon Guevara
- **Rama:** `ramaMarlon`
- **Carpeta:** `Problema2_8Puzzle/`
- **Estado:** ⏳ Pendiente

### Problema 3: 🎲 Juego de la Galleta (Hiedi)
- **Descripción:** Programar el juego de la galleta siguiendo las reglas del juego
- **Algoritmo:** Métodos de búsqueda (Minimax, Alpha-Beta Pruning, etc.)
- **Patrón:** Strategy o State
- **Integrante:** Hiedi
- **Rama:** `ramaHiedi`
- **Carpeta:** `Problema3_JuegoGalleta/`
- **Estado:** ⏳ Pendiente

### Problema 4: 🔢 Sudoku (Viviana)
- **Descripción:** Resolver el juego del Sudoku
  - Permitir jugar al usuario
  - Resolver el problema automáticamente por la computadora (pausar para poder mirar la solución)
- **Algoritmo:** Backtracking o Constraint Propagation
- **Patrón:** Strategy o Template Method
- **Integrante:** Viviana
- **Rama:** `ramaViviana`
- **Carpeta:** `Problema4_Sudoku/`
- **Estado:** ⏳ Pendiente

---

## ⚠️ Reglas Importantes

### ✅ HACER:
- ✅ Trabajar solo en tu rama personal
- ✅ Hacer commits frecuentes con mensajes claros
- ✅ Trabajar solo dentro de tu carpeta asignada
- ✅ Documentar tu código
- ✅ Aplicar principios SOLID
- ✅ Implementar al menos un patrón de diseño
- ✅ Probar que tu código funciona antes de hacer push

### ❌ NO HACER:
- ❌ NO modificar archivos fuera de tu carpeta
- ❌ NO hacer push directamente a `main`
- ❌ NO modificar el código de otros integrantes
- ❌ NO hacer merge sin coordinación
- ❌ NO subir archivos compilados (`bin/`, `obj/`)
- ❌ NO copiar código sin entender

---
