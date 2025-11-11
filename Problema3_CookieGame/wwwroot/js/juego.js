document.addEventListener("DOMContentLoaded", () => {
    const svg = document.getElementById("gameSvg");
    const resetBtn = document.getElementById("resetBtn");
    let firstPoint = null;

    svg.addEventListener("click", (e) => {
        const rect = svg.getBoundingClientRect();
        const x = Math.round((e.clientX - rect.left) / (rect.width / 5));
        const y = Math.round((e.clientY - rect.top) / (rect.height / 5));

        if (!firstPoint) {
            firstPoint = { x, y };
        } else {
            const dx = Math.abs(firstPoint.x - x);
            const dy = Math.abs(firstPoint.y - y);

            if ((dx === 1 && dy === 0) || (dx === 0 && dy === 1)) {
                fetch("/Game/AddLine", {
                    method: "POST",
                    headers: { "Content-Type": "application/x-www-form-urlencoded" },
                    body: `x1=${firstPoint.x}&y1=${firstPoint.y}&x2=${x}&y2=${y}`
                })
                    .then((r) => r.text())
                    .then((html) => {
                        document.getElementById("gameBoard").innerHTML = html;
                    });
            }
            firstPoint = null;
        }
    });

    resetBtn.addEventListener("click", () => {
        fetch("/Game/Reset", { method: "POST" }).then(() => location.reload());
    });
});
