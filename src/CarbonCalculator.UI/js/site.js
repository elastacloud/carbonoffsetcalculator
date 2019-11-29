function drawMonthly(canvasElement) {
    let ctx = canvasElement.getContext("2d");
    ctx.fillRect(0,0, 100, 100);
    ctx.fillRect(100, 100, 100, 100);
}

drawMonthly(document.getElementById("monthly"));