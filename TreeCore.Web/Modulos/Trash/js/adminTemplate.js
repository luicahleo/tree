
setTimeout(function () {

    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
    App.CenterPanelMain.update();
    ActiveResizer();
    VisorMode("ModoVisor");


}, 200);

// #region FUNCION COLUMNAS DINAMICAS PARA GRID, INSERTAR COMO LISTENER ON RESIZE Y AFTER RENDER EN EL PROPIO GRID


// LAS COLUMNAS DE DICHOS GRIDS TIENEN QUE TENER EL ATRIBUTO MINWIDTH DEFININO EN TODAS LAS COLS

//LA COLMORE DEBE SER APROX MINW 90 Y MAXW90

function GridColHandler(grid) {
    // Con esta variable se controla si la columna more esta visible siempre o no
    var ForceShowColmore = false;

    //Variables de entorno(no editar)
    var gridW = grid.getWidth();
    const colArray = grid.columns;
    const colArrayNoColMore = grid.columns.slice(0);
    var LastCol = colArrayNoColMore[colArrayNoColMore.length - 1];
    var AllcolsMinWTotal = 0;
    var visiblecolsMinWTotal = 0;


    // Se crea un array sin la columna More
    if (LastCol.initialCls == "col-More") {
        colArrayNoColMore.pop();
    }

    // CALCULO DE MINWIDTHS TOTALES Y QUITAMOS LA COLMORE DEL CALCULO

    colArrayNoColMore.forEach(function (colArrayNoColMore) {
        if (colArrayNoColMore.hidden != true) {
            visiblecolsMinWTotal = visiblecolsMinWTotal + colArrayNoColMore.minWidth;
        }

        AllcolsMinWTotal = AllcolsMinWTotal + colArrayNoColMore.minWidth;


    });


    //Controles de anchura y hide (aqui esta el tema)

    for (let i = 0; i < 18 && visiblecolsMinWTotal <= gridW + 90; i++) {


        var HiddenCols = colArrayNoColMore.filter(x => {
            return x.hidden == true;
        })


        if (HiddenCols.length > 0) {
            var FirstHiddenColIndex = HiddenCols[0].fullColumnIndex;
            grid.columns[FirstHiddenColIndex].show();

            //// Se Suma la anchuraminima del computo de las columnas visibles que Hay

            var minWLastShownCol = HiddenCols[0].minWidth;
            visiblecolsMinWTotal = visiblecolsMinWTotal + minWLastShownCol;
        }


    }


    while (visiblecolsMinWTotal >= gridW - 100) {



        var VisibleCols = colArrayNoColMore.filter(x => {
            return x.hidden != true;
        })



        if (VisibleCols.length > 0) {



            //Se oculta Ultima Columna de las VISIBLES
            var LastVisibleColIndex = VisibleCols.length - 1;
            grid.columns[LastVisibleColIndex].hide();

            // Se resta la anchuraminima del computo de las columnas visibles que quedan
            var minWLastCol = VisibleCols[VisibleCols.length - 1].minWidth
            visiblecolsMinWTotal = visiblecolsMinWTotal - minWLastCol;

        } else {
            break
        }


    }



    // #region AQUI SE ESCONDE LA COLUMNA MORE (debe ser la ultima) POR DEFECTO!
    //Index colmore
    var colMoreIndex = grid.columns.length - 1;



    if (AllcolsMinWTotal < gridW - 70) {
        grid.columns[colMoreIndex].hide();

    } else if (visiblecolsMinWTotal <= gridW + 90) {
        grid.columns[colMoreIndex].show();

    }

    if (ForceShowColmore == true) {

        grid.columns[colMoreIndex].show();
    }


    //#endregion

}

// #endregion

// #region newResponsiveControl



var colOverride = "2cols";  // LOS MODOS SON 1cols 2cols 3cols

var colMode = "3colmode";
var isOnColmbl = 1;
var isOnColNormal = 1;

function ColOverrideControl() {



    if (document.getElementById('CenterPanelMain') != null) {

        var res = document.getElementById('CenterPanelMain').offsetWidth;


        if (colOverride == "3cols") {

        }

        else if (colOverride == "2cols") {

            if (App.ctMain3 != null) {
                App.ctMain3.hide();
            }


            if (res < 576) {

                if (ctmain1ForcedHide != true) {
                    App.btnNextSldr.show();
                    App.btnPrevSldr.show();
                }



            }
            else {

                App.btnNextSldr.hide();
                App.btnPrevSldr.hide();

            }
        }

        else if (colOverride == "1cols") {


            App.btnNextSldr.hide();
            App.btnPrevSldr.hide();

            App.ctMain2.setHidden(true);
            App.ctMain3.setHidden(true);
        }
    }
    //App.MainVwP.center();

}

function moveCtSldr(btn) {

    var res = document.getElementById('CenterPanelMain').offsetWidth;
    var btnPrssd = btn.id;



    if (res < 576 && colMode == "1colmode") { // MODO 1 COLUMNA



        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.hide();

        if (btnPrssd == 'btnNextSldr' && isOnColmbl == 1 && colMode == "1colmode") {
            App.ctMain1.hide();
            App.ctMain2.hide();
            App.ctMain3.hide();

            App.ctMain2.show();


            App.btnNextSldr.enable();
            App.btnPrevSldr.enable();




            isOnColmbl = 2;

            if (colOverride == "2cols") {
                App.btnNextSldr.disable();
                App.btnPrevSldr.enable();

            }


        }

        else if (btnPrssd == 'btnNextSldr' && isOnColmbl == 2 && colMode == "1colmode") {
            App.ctMain1.hide();
            App.ctMain2.hide();
            App.ctMain3.hide();

            App.ctMain3.show();


            App.btnNextSldr.disable();
            App.btnPrevSldr.enable();




            isOnColmbl = 3;




        }



        if (btnPrssd == 'btnPrevSldr' && isOnColmbl == 3 && colMode == "1colmode") {


            App.ctMain2.show();


            App.btnNextSldr.enable();
            App.btnPrevSldr.enable();

            isOnColmbl = 2;


        }

        else if (btnPrssd == 'btnPrevSldr' && isOnColmbl == 2 && colMode == "1colmode") {





            if (ctmain1ForcedHide != true) {
                App.ctMain1.show();
            }


            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();


            isOnColmbl = 1;

        }





    }

    else if (res <= 991 && res > 576 && colMode == "2colmode") { // MODO 2 COLS

        App.ctMain2.hide();
        App.ctMain3.hide();

        if (btnPrssd == 'btnNextSldr' && colMode == "2colmode") {

            App.ctMain3.show();


            App.btnNextSldr.disable();
            App.btnPrevSldr.enable();



        }



        if (btnPrssd == 'btnPrevSldr' && colMode == "2colmode") {


            App.ctMain2.show();


            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();


        }
    }

}

window.addEventListener('resize',

    function PassiveResizer() {

        var el = document.getElementById('CenterPanelMain');

        if (el != null) {
            var res = document.getElementById('CenterPanelMain').offsetWidth;


            if (res <= 576 && colMode != "1colmode") {

                if (ctmain1ForcedHide != true) {
                    App.btnNextSldr.show();
                    App.btnPrevSldr.show();
                }


                if (ctmain1ForcedHide != true) {
                    App.ctMain1.setHidden(false);
                    App.ctMain2.setHidden(true);
                }


                App.ctMain3.setHidden(true);

                App.btnPrevSldr.disable();
                App.btnNextSldr.enable();

                colMode = "1colmode";

            }

            else if ((res <= 991 && res > 576 && colMode != "2colmode") && (colOverride == "2cols" || colOverride == "3cols")) {
                if (ctmain1ForcedHide != true) {
                    App.btnNextSldr.show();
                    App.btnPrevSldr.show();
                }






                if (ctmain1ForcedHide != true) {
                    App.ctMain1.setHidden(false);
                }

                App.ctMain2.setHidden(false);
                App.ctMain3.setHidden(true);

                colMode = "2colmode";

                isOnColmbl = 1;

                App.btnNextSldr.enable();
                App.btnPrevSldr.disable();

            }


            else if (res > 991 && colOverride == "3cols") {


                App.btnNextSldr.hide();
                App.btnPrevSldr.hide();


                if (ctmain1ForcedHide != true) {
                    App.ctMain1.setHidden(false);

                }

                if (ctmain1ForcedHide == true) {

                    App.btnNextSldr.hide();
                    App.btnPrevSldr.hide();
                }


                App.ctMain2.setHidden(false);
                App.ctMain3.setHidden(false);


                App.btnNextSldr.enable();
                App.btnPrevSldr.disable();

                colMode = "3colmode";

                isOnColmbl = 1;
                isOnColNormal = 1;

            }


            else if (res > 991 && colOverride == "2cols") {

                App.ctMain2.setHidden(false);

            }

        }

        ColOverrideControl();

        if (ctmain1ForcedHide == true) {

            App.btnNextSldr.hide();
            App.btnPrevSldr.hide();

        }


    }

);

function ActiveResizer() {

    var el = document.getElementById('CenterPanelMain');


    if (el != null) {
        var res = document.getElementById('CenterPanelMain').offsetWidth;


        if (res <= 576 && colMode != "1colmode") {
            if (ctmain1ForcedHide != true) {
                App.btnNextSldr.show();
                App.btnPrevSldr.show();
            }






            if (ctmain1ForcedHide != true) {
                App.ctMain1.setHidden(false);
                App.ctMain2.setHidden(true);


            }

            App.ctMain3.setHidden(true);



            App.btnPrevSldr.disable();
            App.btnNextSldr.enable();

            colMode = "1colmode";

        }

        else if ((res <= 991 && res > 576 && colMode != "2colmode") && (colOverride == "2cols" || colOverride == "3cols")) {

            App.btnNextSldr.show();
            App.btnPrevSldr.show();

            if (ctmain1ForcedHide != true) {
                App.ctMain1.setHidden(false);

            }

            App.ctMain2.setHidden(false);
            App.ctMain3.setHidden(true);

            colMode = "2colmode";

            isOnColmbl = 1;

            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();

        }


        else if (res > 991 && colOverride == "3cols") {

            App.btnNextSldr.hide();
            App.btnPrevSldr.hide();


            if (ctmain1ForcedHide != true) {
                App.ctMain1.setHidden(false);

            }

            App.ctMain2.setHidden(false);
            App.ctMain3.setHidden(false);


            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();

            colMode = "3colmode";

            isOnColmbl = 1;
            isOnColNormal = 1;

        }

        ColOverrideControl();
        //App.pnComboGrdVisor.update();



    }

}


var spPnLiteExpanded = 0;
function hidePnLite() {
    let btn = document.getElementById('btnCollapseAsRClosed');



    if (spPnLiteExpanded == 0) {
        btn.style.transform = 'rotate(-180deg)';
        spPnLiteExpanded = 1;
        App.pnAsideR.expand(true);

    }
    else if (spPnLiteExpanded == 1) {
        btn.style.transform = 'rotate(0deg)';
        spPnLiteExpanded = 0;
        App.pnAsideR.collapse(true);

    }
    App.btnCollapseAsRClosed.show();

}



function hidePnForced(ForcedExCol) {
    let btn = document.getElementById('btnCollapseAsRClosed');



    if (ForcedExCol == "Expand") {
        btn.style.transform = 'rotate(-180deg)';
        spPnLiteExpanded = 1;
        App.pnAsideR.expand(true);

    }
    else if (ForcedExCol == "Collapse") {
        btn.style.transform = 'rotate(0deg)';
        spPnLiteExpanded = 0;
        App.pnAsideR.collapse(true);

    }

}






function displayMenu(btn) {


    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnGridInfo.hide();
    App.pnInfoVersiones.hide();
    App.pnMetaData.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();

}



// #endregion

// #region Renders Grid
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

// #endregion

// #region CONTROL VISORMODE


function VisorIconReset() {
    TreeCore.vwReset();

}

var ctmain1ForcedHide = false;


function FHideCtMain1() {

    if (ctmain1ForcedHide == false) {
        ctmain1ForcedHide = true;
    }
    else {
        ctmain1ForcedHide = false;
    }
}


function VisorMode(btn) {
    if (btn == "ModoVisor") {

        colOverride = "2cols";
        //App.ctMain2.show();



        ctmain1ForcedHide = false;


        //document.getElementById('hdVisor').setAttribute("Value", 1);
    }
    else if (btn == "ModoGrid") {
        colOverride = "1cols";
        App.ctMain2.hide();
        App.ctMain1.show();



        ctmain1ForcedHide = false;

        VisorIconReset();


        document.getElementById('hdVisor').setAttribute("Value", 0);

    }
    //App.storePrincipal.reload();
}

setTimeout(function () {

    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
    //GridResizer();
    //App.pnComboGrdVisor.update();
    //App.CenterPanelMain.update();

    //ActiveResizer();


}, 100);

function GridResizer() {
    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);


    // var offsetHeight = document.getElementById('PanelMainGrid').offsetHeight;

    var calcdH = vh;

    //PANELES A CONTROLAR
    //App.pnComboGrdVisor.height = vh;
    //App.grdTask.height = vh;




    // USAR CUANDO EL SLIDER APARECE ARRIBA HAY QUE RECALCULAR TENIENDOLO EN CUENTA

    //App.pnComboGrdVisor.height = vh - 67;
    //App.grdTask.height = vh - 95;




    // #region CONTROL COLUMNA COLMORE MORE 


    const vw = Math.max(document.documentElement.clientWidth || 0, window.innerWidth || 0);

    if (vw < 1024 && vw > 480) {
        TreeCore.ColumnHider('Hide', '1024');
        //App.pnComboGrdVisor.update();
    }
    else if (vw < 480) {
        TreeCore.ColumnHider('Hide', '480');
        //App.pnComboGrdVisor.update();

    }
    else if (vw > 1024) {
        TreeCore.ColumnHider('Show', '');
        //App.pnComboGrdVisor.update();
    }

    // #endregion



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

function grdInsideH() {
    let pH = App.pnComboGrdVisor.height;
    App.grdInsidePn.height = pH;
}

function ShowTreeGrd() {
    App.TreePanel2.show();
    App.PanelVisorMain.show();


    App.CenterPanelMain.hide();



}

function HideTreeGrd() {


    //var west = Ext.getCmp('TreePanel2'); // this is west panel region
    //west.hide();


    App.TreePanel2.hide();
    App.PanelVisorMain.hide();


    App.CenterPanelMain.show();



}

function ShowPanelAddMeds() {
    App.tbPanelAdd.show();
}

function HidePanelAddMeds() {
    App.tbPanelAdd.hide();
}

// #endregion

//  #region RESIZERS PARA VENTANAS MODALES (CALCULO EXTERNO)




function winFormCenterSimple(obj) {


    obj.center();
    //obj.update();

}


function winFormResize(obj) {

    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }
    obj.center();
    //obj.update();

}



function winFormResizeDockBot(obj) {


    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }
    obj.center();
    //obj.update();


    //AQUI SE SETEA EL CENTER ABAJO

    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);
    obj.setY(vh - obj.height);


    //obj.update();


}

function formResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(620);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
    obj.center();
    //obj.update();



}




window.addEventListener('resize', function () {


    var dv = document.querySelectorAll('div.winForm-respSimple');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormCenterSimple(obj);
    }


    var dv = document.querySelectorAll('div.winForm-resp');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResize(obj);
    }

    var frm = document.querySelectorAll('div.ctForm-resp');
    for (i = 0; i < frm.length; i++) {
        var obj = Ext.getCmp(frm[i].id);
        formResize(obj);
    }



    var dv = document.querySelectorAll('div.winForm-respDockBot');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResizeDockBot(obj);
    }



});


// #endregion

// #region
function winFormResizeAddNewDocument(obj) {

    var res = obj.width;
    let ele = document.getElementById('winAddDocument-innerCt');




    if (ele != null) {
        if (res <= 670) {



            ele.classList.add('grid1colAdv5Rows');



            //App.winAddDocument.update();

        }
        else {


            ele.classList.remove('grid1colAdv5Rows');


            //.winAddDocument.update();


        }

    }
}

function Grid_RowSelectArbol(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.AddDocuMainPanel.enable();
    }
}

function DeseleccionarGrilla() {
    App.AddDocuMainPanel.disable();
}

function AgregarDocumento() {
    App.winAddDocument.show();
}

// #endregion

// #region DIRECT METHOD TEMPLATES
var seleccionado;

var handlePageSizeSelectTemplate = function (item, records) {
    var curPageSize = App.storeDocumentosCargaPlantillas.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeDocumentosCargaPlantillas.pageSize = wantedPageSize;
        App.storeDocumentosCargaPlantillas.load();
    }
}

function Grid_RowSelectTemplate(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnActivar.enable();
        App.btnDescargarTemplate.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDescargarTemplate.setTooltip(jsDescargarPlantilla);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }

        if (seleccionado.RutaDocumento != "") {
            App.UploadF.hide();
            App.txtFuncion.hide();
            App.txtRuta.setValue(seleccionado.RutaDocumento);
            App.txtRuta.show();
            App.btnEditarRuta.show();
        }
        else {
            App.txtRuta.hide();
            App.txtRuta.setValue("");
            App.btnEditarRuta.hide();
            App.UploadF.show();
            App.txtFuncion.show();
        }

        App.hdPlantillaSeleccionada.setValue(seleccionado.DocumentoCargaPlantillaID);
        App.hdLabelTab.setValue(seleccionado.DocumentoCargaPlantilla);
        //App.storeDocumentosCargaPlantillasTabs.reload();
    }
}

function DeseleccionarGrilla() {
    App.GridRowSelectTemplate.clearSelections();
    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnActivar.disable();
    App.btnDescargarTemplate.disable();
    App.hdPlantillaSeleccionada.setValue("");
    App.hdLabelTab.setValue("");
}

function VaciarFormulario() {
    App.formTemplate.getForm().reset();
}

function AgregarEditar() {
    VaciarFormulario();
    App.winSaveNewTabCols.setTitle(parent.jsAgregar);
    Agregar = true;
    App.winSaveNewTabCols.show();
    App.txtRuta.hide();
    App.txtRuta.setValue("");
    App.btnEditarRuta.hide();
    App.UploadF.show();
    App.txtFuncion.show();
}

function FormularioValidoTemplate(valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function winGestionBotonGuardarTemplate() {
    if (App.formTemplate.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditar() {
    TreeCore.AgregarEditar(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winSaveNewTabCols.hide();
                    Refrescar();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function MostrarEditar() {
    if (seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winSaveNewTabCols.setTitle(parent.jsEditar);

    if (seleccionado.RutaDocumento != "") {
        App.UploadF.hide();
        App.txtFuncion.hide();
        App.txtRuta.setValue(seleccionado.RutaDocumento);
        App.txtRuta.show();
        App.btnEditarRuta.show();
    }

    TreeCore.MostrarEditar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            }
        });
}

function Activar() {
    if (seleccionado != null) {
        ajaxActivar();
    }
}

function ajaxActivar() {
    TreeCore.Activar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                Refrescar();
            }
        });
}

function Eliminar() {
    if (seleccionado != null) {
        Ext.Msg.alert(
            {
                title: parent.jsEliminar,
                msg: parent.jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminar(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.Eliminar({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Refrescar();
                }
            },
            eventMask: {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    }
}

function Refrescar() {
    DeseleccionarGrilla();
    App.storeDocumentosCargaPlantillas.reload();
}

function RecargarProyectoTipo() {
    recargarCombos([App.cmbProyectosTipos]);
}

function SeleccionarProyectoTipo() {
    App.cmbProyectosTipos.getTrigger(0).show();
}

function DescargarPlantilla() {
    TreeCore.ExistePlantilla(seleccionado.RutaDocumento,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    if (result) {
                        window.open("adminTemplate.aspx?DescargarPlantilla=" + seleccionado.RutaDocumento);
                    }
                }
            },
            eventMask: {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function limpiar(value) {
    value.setValue("");
}

function buscador() {
    App.storeDocumentosCargaPlantillas.reload();
}

function editarRutaDocumento() {
    App.txtRuta.hide();
    App.txtRuta.setValue("");
    App.UploadF.show();
    App.txtFuncion.show();
    App.btnEditarRuta.hide();
}

var DefectoRender = function (value) {
    if (value == true || value == 1) {
        return '<span class="ico-defaultGrid">&nbsp;</span>'

    }
    else {
        return '<span>&nbsp;</span> '
    }
}

// #endregion

// #region DIRECT METHOD TABS
var seleccionadoTab;

var handlePageSizeSelectTab = function (item, records) {
    var curPageSize = App.storeDocumentosCargaPlantillasTabs.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeDocumentosCargaPlantillasTabs.pageSize = wantedPageSize;
        App.storeDocumentosCargaPlantillasTabs.load();
    }
}

function Grid_RowSelectTab(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoTab = datos;
        App.btnEditarGrid.enable();
        App.btnEliminarGrid.enable();
        App.btnActivarGrid.enable();

        App.btnEditarGrid.setTooltip(jsEditar);
        App.btnAnadirGrid.setTooltip(jsAgregar);
        App.btnEliminarGrid.setTooltip(jsEliminar);

        if (seleccionadoTab.Activo) {
            App.btnActivarGrid.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivarGrid.setTooltip(jsActivar);
        }
    }
}

function DeseleccionarGrillaTabs() {
    App.GridRowSelectTab.clearSelections();
    App.btnEditarGrid.disable();
    App.btnEliminarGrid.disable();
    App.btnActivarGrid.disable();
}

function VaciarFormularioTab() {
    App.formTab.getForm().reset();
}

function AgregarEditarTab() {
    VaciarFormularioTab();
    App.winTabRules.setTitle(parent.jsAgregar);
    Agregar = true;
    App.winTabRules.show();
}

function FormularioValidoTab(valid) {
    if (valid) {
        App.btnGuardarTab.setDisabled(false);
    }
    else {
        App.btnGuardarTab.setDisabled(true);
    }
}

function winGestionBotonGuardarTab() {
    if (App.formTab.getForm().isValid()) {
        ajaxAgregarEditarTab();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditarTab() {
    TreeCore.AgregarEditarTab(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winTabRules.hide();
                    RefrescarTab();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function MostrarEditarTab() {
    if (seleccionadoTab != null) {
        ajaxEditarTab();
    }
}

function ajaxEditarTab() {
    Agregar = false;
    App.winTabRules.setTitle(parent.jsEditar);
    TreeCore.MostrarEditarTab(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            }
        });
}

function ActivarTab() {
    if (seleccionadoTab != null) {
        ajaxActivarTab();
    }
}

function ajaxActivarTab() {
    TreeCore.ActivarTab(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                RefrescarTab();
            }
        });
}

function EliminarTab() {
    if (seleccionadoTab != null) {
        Ext.Msg.alert(
            {
                title: parent.jsEliminar,
                msg: parent.jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarTab,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarTab(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarTab({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    RefrescarTab();
                }
            },
            eventMask: {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    }
}

function RefrescarTab() {
    DeseleccionarGrillaTabs();
    App.storeDocumentosCargaPlantillasTabs.reload();
}

// #endregion

