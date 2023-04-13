// #region RENDERS
function renderDefault(valor, id) {

    let imag = document.getElementById('imDefault' + id);

    if (valor == false) {
        imag.src = '';

    }

    else {
        imag.src = '../../ima/ico-default.svg';
    }


}

function renderStop(valor, id) {

    let imag = document.getElementById('imStop' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-stop.svg';

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

function renderRegion(valor, id) {
    let imag = document.getElementById('imRegion' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-region-gr.svg';

    }


}

function renderActive(valor, id) {
    let imag = document.getElementById('imActive' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-cancel.svg';

    }


}

function renderReport(valor, id) {
    let imag = document.getElementById('imReport' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-invisible.svg';

    }


}

function renderContacts(valor, id) {
    let imag = document.getElementById('imContacts' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-contacts-gr.svg';

    }


}

// #endregion

var PuntoCorteL = 900;
var PuntoCorteS = 512;
var selectedCol = 0;
var isOnMobC = 0;


function ControlSlider() {
    var containerSize = parent.App.CenterPanelMain.getWidth();


    var pnmain = App.grdSLA;
    var col2 = App.pnCol1;
    var col3 = App.pnCol2;
    var tbsliders = App.tbSliders;
    var btnPrevSldr = App.btnPrevGrid;
    var btnNextSldr = App.btnNextGrid;


    //state 2 cols

    if (containerSize > PuntoCorteL) {
        pnmain.show();
        col2.show();
        col3.show();
        selectedCol = 1;

        isOnMobC = 0;

    }

    if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {
        pnmain.show();
        if (selectedCol == 3) {
            col2.hide();
            col3.show();
        }
        else {
            col2.show();
            col3.hide();
        }
        isOnMobC = 0;

    }


    // state 1 col
    if (containerSize < PuntoCorteS && isOnMobC == 0) {
        pnmain.show();
        col2.hide();
        col3.hide();

        btnPrevSldr.disable();
        btnNextSldr.enable();

        selectedCol = 1;

        isOnMobC = 1;
    }



    //CONTROL SHOW OR HIDE BOTONES SLIDER
    if (pnmain.hidden == true || col2.hidden == true || col3.hidden == true) {

        tbsliders.show();

        if (pnmain.hidden != true && col2.hidden == false && col3.hidden == true) {
            btnPrevSldr.disable();

        }

    }
    else {


        tbsliders.hide();
        btnPrevSldr.disable();
        btnNextSldr.enable();


    }

}

function SliderMove(NextOrPrev) {

    var containerSize = parent.App.CenterPanelMain.getWidth();

    var pnmain = App.grdSLA;
    var col1 = App.pnCol1;
    var col2 = App.pnCol2;
    var btnPrevSldr = App.btnPrevGrid;
    var btnNextSldr = App.btnNextGrid;

    //SELECCION EN 2  PANELES
    if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {

        if (NextOrPrev == 'Next') {
            col1.hide();
            col2.show();
            selectedCol = 3;

            btnPrevSldr.enable();
            btnNextSldr.disable();

        }
        else if (NextOrPrev == 'Prev') {

            col1.show();
            col2.hide();
            selectedCol = 2;

            btnPrevSldr.disable();
            btnNextSldr.enable();

        }
    }

    //SELECCION EN 1  PANEL
    else {

        if (NextOrPrev == 'Next' && selectedCol == 1) {
            pnmain.hide();
            col1.show();
            col2.hide();
            selectedCol = 2;

            btnPrevSldr.enable();
            btnNextSldr.enable();

        }
        else if (NextOrPrev == 'Prev' && selectedCol == 2) {
            pnmain.show();
            col1.hide();
            col2.hide();
            selectedCol = 1;

            btnPrevSldr.disable();
            btnNextSldr.enable();

        }
        else if (NextOrPrev == 'Next' && selectedCol == 2) {
            pnmain.hide();
            col1.hide();
            col2.show();
            selectedCol = 3;

            btnPrevSldr.enable();
            btnNextSldr.disable();

        }

        else if (NextOrPrev == 'Prev' && selectedCol == 3) {
            pnmain.hide();
            col1.show();
            col2.hide();
            selectedCol = 2;

            btnPrevSldr.enable();
            btnNextSldr.enable();

        }


    }


}
