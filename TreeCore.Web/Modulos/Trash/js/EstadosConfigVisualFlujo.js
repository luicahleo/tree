
setTimeout(function () {
	let winH = window.innerHeight;
	let pnH = winH - 70;
	App.pnFlujoConfig.setHeight(pnH);
	App.ctDrawingFlow.setHeight(pnH);
	App.ctDrawingFlow.setHeight(pnH - 140);
	document.getElementById('ctDrawingFlow_Content').style.height = pnH - 190 + "px";
}, 100);


window.addEventListener('resize', function () {
	let winH = window.innerHeight;
	let pnH = winH - 70;
	App.pnFlujoConfig.setHeight(pnH);
	App.ctDrawingFlow.setHeight(pnH - 140);
	document.getElementById('ctDrawingFlow_Content').style.height = pnH - 190 + "px";
});