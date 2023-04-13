
setTimeout(function () {

    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
    GridResizer();
    App.hugeCt.update();
    App.pnCFilters.update();
    App.pnGridsAsideMyFilters.update();
    App.pnMapFilters.update();

}, 100);


function GridResizer() {
    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);

    //const offsetHeight = document.getElementById('vwResp').offsetHeight;

    var calcdH = vh;

    //PANELES A CONTROLAR
    App.hugeCt.height = calcdH;
    App.pnCFilters.height = calcdH;
    App.pnGridsAsideMyFilters.height = calcdH;

    // RECALC POR LAS TOOLBARS O ESPACIADO SUPERIOR
    App.hugeCt.height = calcdH - 100;
    App.pnCFilters.height = calcdH - 55;
    App.pnGridsAsideMyFilters.height = calcdH - 55;
    App.pnMapFilters.height = calcdH - 55;

}


//RENDERS ASIDE y MAS

function renderAuthorized(valor, id) {
    let imag = document.getElementById('imAuthorized' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }


}

function renderClosed(valor, id) {
    let imag = document.getElementById('imClsd' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';
    }



}

function renderMultiflow(valor, id) {

    let imag = document.getElementById('imMultiflow' + id);

    if (valor == false) {
        imag.src = '';

    }

    else {
        imag.src = '../../ima/ico-subprocess.svg';
    }


}

function renderCommercial(valor, id) {

    let imag = document.getElementById('imCommercial' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-vendor.svg';

    }


}

function renderInactive(valor, id) {
    let imag = document.getElementById('imInactive' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-cancel.svg';

    }


}

function renderProgBar(valor, id) {
    let bar = document.getElementById('progBar' + id);
    let ancho = valor;

    bar.style.width = ancho * 100 + "%";


}

function renderRegion(valor, id) {
    let imag = document.getElementById('imRegion' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-region-gr.svg';

    }


}

function renderStaff(valor, id) {
    let imag = document.getElementById('imStaff' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }


}

function renderSupport(valor, id) {
    let imag = document.getElementById('imSupport' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }

}

function renderLDAP(valor, id) {
    let imag = document.getElementById('imLDAP' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }

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



//BOTONES NAV

function habilitaLnk(vago) {
	let ct = document.getElementById('cntNavVistasForm-innerCt');
	let aLinks = ct.querySelectorAll('a');

	aLinks.forEach(function (itm) {
		itm.classList.remove("navActivo");
	});

	vago.classList.add('navActivo');
}


function showForms(who) {
	var LNo = who.textEl;


	switch (who.id) {
		case 'lnkSiteF':
			habilitaLnk(LNo);
			App.formSite.show();
			App.formLocation.hide();
			App.formAdditional.hide();
			break;

		case 'lnkLocationF':

			habilitaLnk(LNo);
			App.formLocation.show();
			App.formSite.hide();
			App.formAdditional.hide();
			break;

		case 'lnkAdditionalF':
			habilitaLnk(LNo);
			App.formAdditional.show();
			App.formLocation.hide();
			App.formSite.hide();
			break;

	}
}


function displayMenu(btn) {

    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnCFilters.hide();
    App.pnGridsAsideMyFilters.hide();
    App.pnMapFilters.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();

}


