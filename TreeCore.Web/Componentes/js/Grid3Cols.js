


// #region Control Slider y BOTONES SLIDER
var PuntoCorteL = 900;
var PuntoCorteS = 512;


var selectedCol = 0;
var isOnMobC = 0;

function SliderMove(NextOrPrev) {
    var containerSize = Ext.get('CenterPanelMain').getWidth();


    var btnPrevSldr = Ext.getCmp('btnPrevSldr');
    var btnNextSldr = Ext.getCmp('btnNextSldr');


    var pnmain = Ext.getCmp(ruta + '_' + 'gridMain1');
    var col1 = Ext.getCmp(ruta + '_' + 'pnCol1');
    var col2 = Ext.getCmp(ruta + '_' + 'pnCol2');

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




function ControlSlider(sender) {
    ruta = getIdComponente(sender);
    var containerSize = Ext.get('CenterPanelMain').getWidth();


    var pnmain = Ext.getCmp(ruta + '_' + 'gridMain1');
    var col2 = Ext.getCmp(ruta + '_' + 'pnCol1');
    var col3 = Ext.getCmp(ruta + '_' + 'pnCol2');
    var tbsliders = Ext.getCmp('tbSliders');
    var btnPrevSldr = Ext.getCmp('btnPrevSldr');
    var btnNextSldr = Ext.getCmp('btnNextSldr');


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




function selectRowDetalle() {
    var containerSize = Ext.get('CenterPanelMain').getWidth();

    var pnmain = Ext.getCmp(ruta + '_' + 'gridMain1');
    var col1 = Ext.getCmp(ruta + '_' + 'pnCol1');
    var col2 = Ext.getCmp(ruta + '_' + 'pnCol2');


    var btnPrevSldr = Ext.getCmp('btnPrevSldr');
    var btnNextSldr = Ext.getCmp('btnNextSldr');


    if (containerSize < PuntoCorteS) {
        pnmain.hide();
        col1.show();
        col2.hide();
        selectedCol = 2;

        btnPrevSldr.enable();
        btnNextSldr.enable();
    }

}


// #endregion