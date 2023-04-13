var stPn = 0;



setTimeout(function () {

	//REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
	GridResizer();
    App.ctSlider.update();

    //AQUI SETEO EL PANEL A ESCONDIDO POR DEFECTO EN LA CARGA DE LA PAGINA (HAY UNA PROPIEDAD MARGINSPEC EN ASPX TAMBIEN)
    stPn = 1;
    let btn = document.getElementById('btnCollapseAsR');
    btn.style.transform = 'rotate(-180deg)';

}, 100);


function GridResizer() {
	const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);

    //const offsetHeight = document.getElementById('vwResp').offsetHeight;

    var calcdH = vh;

	//PANELES A CONTROLAR
	App.GridBusqMain.height = calcdH;


	// RECALC POR LAS TOOLBARS
		App.GridBusqMain.height = calcdH - 185;


    // #region CONTROL COLUMNA COLMORE MORE 

    var vw = Math.max(document.documentElement.clientWidth || 0, window.innerWidth || 0);


    if (vw < 1024 && vw > 480)
    {
        TreeCore.ColumnHider('Hide', '1024');

    }
    else if (vw < 480)
    {
        TreeCore.ColumnHider('Hide', '480');


    }
    else if (vw > 1024)
    {
        TreeCore.ColumnHider('Show', '');
    }


    // #endregion

}




function cmbAreaBusquedaAdjust()
{

    document.getElementById("ComboPagBusq-inputEl").style.paddingLeft = "40px";
    document.getElementById("ComboPagBusq-inputEl").style.paddingTop = "8px";
}




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




// Renders Grid
var barGrid = function (value) {

    let colorBar;

    if (value > 0 && value < 0.20) {
        colorBar = 'barRed-grid';
    }
    else if (value >= 0.20 && value < 0.45) {
        colorBar = 'barYellow-grid';
    }

    else if (value >= 0.45 && value < 0.80) {
        colorBar = 'barBlue-grid';
    }

    else if (value >= 0.80 && value <= 1) {
        colorBar = 'barGreen-grid';
    }
    return `<div class="x-progress x-progress-default" style="margin:2px 1px 1px 1px;width:50px;">
				<div class="x-progress-text x-progress-text-back" style="width:50px;">${value * 100}%</div>
				<div class="x-progress-bar x-progress-bar-default ${colorBar}" style="width: ${value * 100}%;"><div class="x-progress-text" style="width:40px;"><div>${value * 100} %</div></div></div></div>`

}

var rojoRender = function (value) {
    let valorRojo = value;

    if (value != null || value != "") {
        return '<span class="dataRed">' + valorRojo + '</span>';
    }
    else {
        return '<span>&nbsp;</span>';
    }
}

var amarilloRender = function (value) {
    let valorAmarillo = value;

    if (value != null || value != "") {
        return '<span class="dataYellow">' + valorAmarillo + '</span>';
    }
    else {
        return '<span>&nbsp;</span>';
    }
}

var DefectoRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-defaultGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var DocsRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-docsGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var FunctRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-functionalityGrid">&nbsp;</span>'
    }
    else {
        //return '<span>&nbsp;</span> '
        return '<span class="ico-functionalityGrid">&nbsp;</span>'

    }
}

var MoreInfoRender = function (value) {

    return '<span class="ico-moreInfo">&nbsp;</span>'

}


var LinkRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-linkGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var NotifRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-notificationGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var SubprocessRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-subprocessGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}


// FIN Renders Grid


//RENDERS GRID PRUEBA

function renderAuthorized(valor, id) {
    let imag = document.getElementById('imAuthorized' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

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

//FIN RENDERS GRID PRUEBA