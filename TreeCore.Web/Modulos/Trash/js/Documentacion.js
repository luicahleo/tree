
setTimeout(function () {

    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
    GridResizer();
    App.ctSlider.update();

}, 100);


function GridResizer() {
    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);

   // var offsetHeight = document.getElementById('ctSlider').offsetHeight;

    var calcdH = vh;

    //PANELES A CONTROLAR
    App.TreePanelMain.height = calcdH;
    App.gridMain1.height = calcdH;


    // CUANDO EL SLIDER APARECE ARRIBA HAY QUE RECALCULAR TENIENDOLO EN CUENTA (PARA PRESUPUESTO OCURRE EN 480PX "NORMALMENTE 1024" al SER 2 PANELES)
    if (window.innerWidth < 480) {
        App.TreePanelMain.height = calcdH - 25;
        App.gridMain1.height = calcdH - 25;


    }

}


// #region RENDERERS GRIDMAIN


var DocIconRender = function (value) {
    let valorDocuIco = value;

    if (value != null && value == "wordico") {
        return '<span class="DocWord">' + '</span>';

    }
    else if (value != null && value == "PDFico") {

        return '<span class="DocPDF">' + '</span>';
    }

    else if (value != null && value == "excelico") {

        return '<span class="DocExcel">' + '</span>';
    }

    else if (value != null && value == "OtherDoc") {

        return '<span class="DocOther">' + '</span>';
    }

    else if (value != null && value == "PowerPico") {

        return '<span class="DocPowerPoint">' + '</span>';
    }
    else {
        return '<span>&nbsp;</span>';
    }
}



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


//#endregion




function ShowPanelAddMeds() {
    App.tbPanelAdd.show();
}

function HidePanelAddMeds() {
    App.tbPanelAdd.hide();
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