setTimeout(function () {

//REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
	GridResizer();
	App.ctSlider.update();  

}, 100);


function GridResizer() {
	const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);

	var offsetHeight = document.getElementById('ctSlider').offsetHeight;

	var calcdH = vh;

	//PANELES A CONTROLAR
	App.GridPanelMain.height = calcdH;
	App.grdTracking.height = calcdH ;


	// CUANDO EL SLIDER APARECE ARRIBA HAY QUE RECALCULAR TENIENDOLO EN CUENTA (PARA PRESUPUESTO OCURRE EN 480PX "NORMALMENTE 1024" al SER 2 PANELES)
	if (window.innerWidth < 480) {
		App.GridPanelMain.height = calcdH - 25;
		App.grdTracking.height = calcdH - 25;


    }

}



function ShowHoverChanges() {

	App.WinTrackChanges.show();
	var x = event.clientX, y = event.clientY,
		elementMouseIsOver = document.elementFromPoint(x, y);

	App.WinTrackChanges.setY(y );
	App.WinTrackChanges.setX(x - 210);

}

function HideHoverChanges() {
	App.WinTrackChanges.hide();
}


function ShowHoverDocum() {

	App.WinTrackDocum.show();
	var x = event.clientX, y = event.clientY,
		elementMouseIsOver = document.elementFromPoint(x, y);

	App.WinTrackDocum.setY(y);
	App.WinTrackDocum.setX(x - 380);

}

function HideHoverDocum() {
	App.WinTrackDocum.hide();
}




var stPn = 0;
function hidePn() {
	let pn = document.getElementById('pnAsideR');
	let btn = document.getElementById('btnCollapseAsR');
	if (stPn == 0) {
		pn.style.marginRight = '-360px';
		btn.style.transform = 'rotate(-180deg)';
		stPn = 1;
	}
	else {
		pn.style.marginRight = '0';
		btn.style.transform = 'rotate(0deg)';
		stPn = 0;
	}

}

// ESTE JS HA SIDO MODIFICADO PARA QUE FUNCIONE CON UN LAYOUT DE 2 PANELES
var stSldr = 0;
var stSldrMbl = 0;
function moveCtSldr(btn) {
	let btnPrssd = btn.id;
	let ct1 = document.getElementById('ctMain1');
	let ct2 = document.getElementById('ctMain2');
	var res = window.innerWidth;


	if (res > 480) {


		if (stSldr == 0 && btnPrssd == 'btnPrevSldr') {
			App.ctMain2.hide();
			App.btnPrevSldr.disable();
			App.btnNextSldr.enable();

			stSldr = 1;

		}
		else if (stSldr != 0 && btnPrssd == 'btnNextSldr') {
			App.ctMain2.show();
			App.btnPrevSldr.enable();
			App.btnNextSldr.disable();
			stSldr = 0;

		}

		

	}

	else if (res <= 480) {
				
			if (stSldrMbl == 0 && btnPrssd == 'btnPrevSldr') {
				App.ctMain1.hide();
				App.btnNextSldr.enable();
				App.btnPrevSldr.disable();
				stSldrMbl = 1;

			}

		//DISABLES PARA EL PANEL 3 QUE NO EXISTE AHORA

			else if (stSldrMbl == 1 && btnPrssd == 'btnNextSldr') {
				App.ctMain1.show();
				App.btnPrevSldr.enable();
				App.btnNextSldr.disable();
				stSldrMbl = 0;

			}
		
	}

	
}

// MODIFICACION PARA 2 PANELES

window.addEventListener('resize', function () {
	var resol = window.innerWidth;
	if (resol > 480) {
		App.ctMain1.show();
		App.btnPrevSldr.enable();
		App.btnNextSldr.disable();
		stSldrMbl = 0;
		stSldr = 0;
	}

});






//FUNCIONES BOTON FILTROS

function ShowPanelAddMeds() {
	App.tbPanelAdd.show();
}

function HidePanelAddMeds() {
	App.tbPanelAdd.hide();
}

function ShowFiltersB() {

	BotonToggleFiltrosB();
	if (App.toolbarFiltrosP2.hidden == true) {
		App.toolbarFiltrosP2.show();
	}
	else {
		App.toolbarFiltrosP2.hide();
	}

}

function BotonToggleFiltrosB() {

	if (App.ToggleFiltrosP2.text == "Filtros") {
		App.ToggleFiltrosP2.setText('Aplicar');

	}
	else {
		App.ToggleFiltrosP2.setText('Filtros');

	}

}





//P3
function ShowFiltersP3() {

	BotonToggleFiltrosP3();
	if (App.toolbarFiltrosP3N1.hidden == true) {
		App.toolbarFiltrosP3N1.show();
		App.toolbarFiltrosP3N2.show();
	}
	else {
		App.toolbarFiltrosP3N1.hide();
		App.toolbarFiltrosP3N2.hide();
	}





	function BotonToggleFiltrosP3() {

		if (App.btnToggleFiltrosP3.text == "Filtros") {
			App.btnToggleFiltrosP3.setText('Aplicar');

		}
		else {
			App.btnToggleFiltrosP3.setText('Filtros');

		}

	}






}


//FUNCIONES JS PANEL PRINCIPAL P1


function ShowMapPn() {
	if (App.PanelP2.hidden == true) {
		App.PanelP2.show();
		App.PanelP3.show();
		App.pnMaps1.hide();
	}
	else {
		App.pnMaps1.show();
		App.PanelP2.hide();
		App.PanelP3.hide();


	}

}


function ShowFiltersTb() {

	BotonToggleFiltrosMain();
	if (App.toolbarfiltros1.hidden == true) {
		App.toolbarfiltros1.show();
		App.toolbarfiltros2.show();
	}
	else {
		App.toolbarfiltros1.hide();
		App.toolbarfiltros2.hide();
	}

}

function BotonToggleFiltrosMain() {

	if (App.btnFiltrosMain.text == "Filtros") {
		App.btnFiltrosMain.setText('Aplicar');

	}
	else {
		App.btnFiltrosMain.setText('Filtros');

	}

}

function BotonClear() {

	App.winClearFields.show();

}

function ShowFiltersP2() {

	BotonToggleFiltrosP2();
	if (App.toolbarFiltrosP2.hidden == true) {
		App.toolbarFiltrosP2.show();
	}
	else {
		App.toolbarFiltrosP2.hide();
	}

}



function BotonDoneP2() {

	BotonToggleP2();
	App.winUndone.show();

}


function BotonToggleP2() {
	if (App.btnDondeUndoneP2.text == "Done") {
		App.btnDondeUndoneP2.setText('Undone');

	}
	else {
		App.btnDondeUndoneP2.setText('Done');

	}

}