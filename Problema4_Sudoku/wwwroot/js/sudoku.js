const q = (s, r=document) => r.querySelector(s);
const qa = (s, r=document) => Array.from(r.querySelectorAll(s));

function readBoard() {
  const m = Array.from({length:9}, () => Array(9).fill(0));
  qa('.cell-input').forEach(inp => {
    const r = +inp.dataset.r, c = +inp.dataset.c;
    const v = parseInt(inp.value, 10);
    m[r][c] = (Number.isInteger(v) && v>=1 && v<=9) ? v : 0;
  });
  return m;
}

function writeBoard(matrix) {
  qa('.cell-input').forEach(inp => {
    const r=+inp.dataset.r, c=+inp.dataset.c;
    const v=matrix[r][c];
    inp.value = v === 0 ? "" : String(v);
  });
}

function setBusy(busy, msg) {
  q('#btnDemo').disabled = busy;
  q('#btnClear').disabled = busy;
  q('#btnSolve').disabled = busy;
  q('#btnStep').disabled = busy;
  q('#status').textContent = msg || (busy ? 'Procesando...' : 'Listo.');
}

async function loadDemo() {
  const res = await fetch('/Sudoku/LoadDemo', { method:'POST' });
  const html = await res.text();
  q('#boardContainer').innerHTML = html;
}

function clearBoard() {
  qa('.cell-input').forEach(inp => { if(!inp.classList.contains('fixed')) inp.value = ''; inp.classList.remove('valid','invalid'); });
  q('#status').textContent = 'Tablero limpiado.';
}

async function solveInstant() {
  setBusy(true, 'Resolviendo...');
  const board = readBoard();
  const res = await fetch('/Sudoku/SolveInstant', {
    method:'POST',
    headers: {'Content-Type':'application/json'},
    body: JSON.stringify({ board })
  });
  const data = await res.json();
  if (data.solved) {
    writeBoard(data.board);
    q('#status').textContent = '¡Resuelto!';
  } else {
    q('#status').textContent = data.message || 'No se pudo resolver.';
  }
  setBusy(false);
}

async function solveSteps() {
  setBusy(true, 'Resolviendo paso a paso...');
  const board = readBoard();
  const res = await fetch('/Sudoku/SolveSteps', {
    method:'POST',
    headers: {'Content-Type':'application/json'},
    body: JSON.stringify({ board })
  });
  const data = await res.json();
  if (!data.steps || data.steps.length === 0) {
    q('#status').textContent = data.message || 'Sin pasos.';
    setBusy(false);
    return;
  }
  // Animación en el cliente
  let i = 0;
  const tick = () => {
    writeBoard(data.steps[i]);
    i++;
    if (i < data.steps.length) {
      requestAnimationFrame(() => setTimeout(tick, 25));
    } else {
      q('#status').textContent = data.solved ? '¡Resuelto!' : (data.message || 'No se pudo resolver.');
      setBusy(false);
    }
  };
  tick();
}

document.addEventListener('click', (e) => {
  if (e.target.id === 'btnDemo') loadDemo();
  if (e.target.id === 'btnClear') clearBoard();
  if (e.target.id === 'btnSolve') solveInstant();
  if (e.target.id === 'btnStep') solveSteps();
});

// Validación ligera (solo 1..9)
document.addEventListener('input', (e) => {
  if (!e.target.classList.contains('cell-input')) return;
  const val = e.target.value.trim();
  if (val === '') { e.target.classList.remove('valid','invalid'); return; }
  if (!/^[1-9]$/.test(val)) { e.target.value = ''; e.target.classList.remove('valid','invalid'); }
});
