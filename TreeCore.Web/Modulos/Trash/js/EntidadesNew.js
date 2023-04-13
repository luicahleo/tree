



setTimeout(function () {

    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
    App.pnWrapMainLayout.update();
    App.CenterPanelMain.update();
    ActiveResizer();



}, 400);


// #region newResponsiveControl

var colOverride = "1cols";  // LOS MODOS SON 1cols 2cols 3cols

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

                App.btnNextSldr.show();
                App.btnPrevSldr.show();


            }
            else {

                App.btnNextSldr.hide();
                App.btnPrevSldr.hide();

            }
        }

        else if (colOverride == "1cols") {


            App.btnNextSldr.hide();
            App.btnPrevSldr.hide();

            //App.ctMain2.setHidden(true);
            //App.ctMain3.setHidden(true);
        }
    }
    //App.MainVwP.center();
    //App.MainVwP.update();

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


            App.ctMain1.show();

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


                App.btnNextSldr.show();
                App.btnPrevSldr.show();


                App.ctMain1.setHidden(false);
                App.ctMain2.setHidden(true);
                App.ctMain3.setHidden(true);



                App.btnPrevSldr.disable();
                App.btnNextSldr.enable();

                colMode = "1colmode";

            }

            else if ((res <= 991 && res > 576 && colMode != "2colmode") && (colOverride == "2cols" || colOverride == "3cols")) {

                App.btnNextSldr.show();
                App.btnPrevSldr.show();


                App.ctMain1.setHidden(false);
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

                App.ctMain1.setHidden(false);
                App.ctMain2.setHidden(false);
                App.ctMain3.setHidden(false);


                App.btnNextSldr.enable();
                App.btnPrevSldr.disable();

                colMode = "3colmode";

                isOnColmbl = 1;
                isOnColNormal = 1;

            }

        }

        ColOverrideControl();

    }

);

function ActiveResizer() {

    var el = document.getElementById('CenterPanelMain');


    if (el != null) {
        var res = document.getElementById('CenterPanelMain').offsetWidth;


        if (res <= 576 && colMode != "1colmode") {

            App.btnNextSldr.show();
            App.btnPrevSldr.show();



            App.ctMain1.setHidden(false);
            App.ctMain2.setHidden(true);
            App.ctMain3.setHidden(true);



            App.btnPrevSldr.disable();
            App.btnNextSldr.enable();

            colMode = "1colmode";

        }

        else if ((res <= 991 && res > 576 && colMode != "2colmode") && (colOverride == "2cols" || colOverride == "3cols")) {

            App.btnNextSldr.show();
            App.btnPrevSldr.show();


            App.ctMain1.setHidden(false);
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


            App.ctMain1.setHidden(false);
            App.ctMain2.setHidden(false);
            App.ctMain3.setHidden(false);


            App.btnNextSldr.enable();
            App.btnPrevSldr.disable();

            colMode = "3colmode";

            isOnColmbl = 1;
            isOnColNormal = 1;

        }

        ColOverrideControl();


    }




}


var spPnLite = 0;
function hidePnLite() {
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (spPnLite == 0) {
        btn.style.transform = 'rotate(-180deg)';
        spPnLite = 1;
    }
    else {
        btn.style.transform = 'rotate(0deg)';
        spPnLite = 0;
    }
}



var spPnLiteD = 0;
function hidePnLiteDirect() {
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (spPnLiteD == 0) {
        btn.style.transform = 'rotate(-180deg)';
        spPnLiteD = 1;
    }
    else {
        btn.style.transform = 'rotate(0deg)';
        spPnLiteD = 0;
    }
}



function RowSelectAsideShowFilters() {

    TreeCore.DirectShowHidePnAsideRFilters();

}


function RowSelectAsideShowInfo() {

    TreeCore.DirectShowHidePnAsideRInfo();

}




function displayMenu(btn) {


    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnCFilters.hide();
    App.pnGridsAsideMyFilters.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();

}


function displayMenuPnInfo(btn) {


    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnGridInfoSeparated.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();

}


// #endregion

// #region RENDERS

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



// #endregion

//  #region RESIZERS PARA VENTANAS MODALES (CALCULO EXTERNO)




function winFormCenterSimple(obj) {


    obj.center();
    obj.update();

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
    obj.update();

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
    obj.update();


    //AQUI SE SETEA EL CENTER ABAJO

    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);
    obj.setY(vh - obj.height);


    obj.update();


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
    obj.update();



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
    //if (LastCol.id == "ColMore") {
    //    colArrayNoColMore.pop();
    //}

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




// #region NAVEGACION TABS SUPERIORES

function NavegacionTabs(who) {
    var LNo = who.textEl;


    App.gridEntities.hide();
    App.gridMain2.hide();


    switch (who.id) {
        case 'lnkPrimero':
            ChangeTab(LNo);
            App.gridEntities.show();

            break;

        case 'lnkSegundo':
            ChangeTab(LNo);
            App.gridMain2.show();

            break;

    }
}

function ChangeTab(vago) {
    let ct = document.getElementById('TbNavegacionTabs-innerCt');
    let aLinks = ct.querySelectorAll('a');

    aLinks.forEach(function (itm) {
        itm.classList.remove("navActivo");
    });

    vago.classList.add('navActivo');
}


// #endregion

//ENTIDADES JS ANTIGUO


setTimeout(resizeWinForm, 100);

// #region GESTION GRID
var oOperador = null;
var oProveedor = null;
var oEmpresaProveedora = null;
function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;

        App.btnAnadir.enable();
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnGestionContactos.enable();

        App.hdEntidadID.setValue(seleccionado.EntidadID);
    }
}

function DeseleccionarGrilla() {
    App.hdEntidadID.setValue("");
    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnGestionContactos.disable();
    App.GridRowSelect.clearSelections();


}

function Refrescar() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

function RecargarGrilla(sender, registro, index) {

    App.storePrincipal.reload();
}

var numMaximo = function (value) {
    if (value != 0) {
        return value;
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var ownerRender = function (value) {
    if (value) {
        return '<span class="ico-owner-grid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var supplierRender = function (value) {
    if (value) {
        return '<span class="ico-supplier-grid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var companyRender = function (value) {
    if (value) {
        return '<span class="ico-company-grid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var operatorRender = function (value) {
    if (value) {
        return '<span class="ico-operator-grid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

function ClickShowContactsD(sender, registro, index) {
    let entidadID = this.getWidgetRecord().id;
    App.hdEntidadID.setValue(entidadID);
    var el = sender.id;
    var pos = document.getElementById(el).getBoundingClientRect();
    App.storeContactosGlobalesEntidad.reload();
    App.WinContactsDetails.show();
    App.WinContactsDetails.setX(pos.right - 480);
    App.WinContactsDetails.setY(pos.top);
}

function ClickShowModulos(sender, registro, index) {
    let entidadID = this.getWidgetRecord().id;
    App.hdEntidadID.setValue(entidadID);
    var el = sender.id;
    var pos = document.getElementById(el).getBoundingClientRect();
    App.storeModulosEmpresaProveedora.reload();
    App.WinModulosEmpresaProveedoras.show();
    App.WinModulosEmpresaProveedoras.setX(pos.right - 480);
    App.WinModulosEmpresaProveedoras.setY(pos.top);

}

// #endregion

// #region AGREGAR/EDITAR/ELIMINAR

function VaciarFormulario() {
    LimpiarFormularioVentana(App.WinGestionEntidad);
    App.formGestionContactos.getForm().reset();
    App.formCompaniaProveedora.getForm().reset();
    App.FormGestionOperator.getForm().reset();
    App.btnOperador.setPressed(false);
    App.btnPropietario.setPressed(false);
    App.btnCompania.setPressed(false);
    App.btnProveedor.setPressed(false);

}

function VaciarFormularioOperador() {
    App.FormGestionOperator.getForm().reset();

}

function VaciarFormularioEmpresaProveedora() {
    App.formCompaniaProveedora.getForm().reset();

}

function VaciarFormularioProveedor() {
    App.FormProveedor.getForm().reset();

}

function FormularioValidoEntidadesV1(sender, valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function FormularioValidoIntermedio(sender) {
    FormularioValidoEntidades(sender, sender.isValid());
}

function FormularioValidoEntidades(sender, valid) {
    if (valid) {
        App['btnGuardar'].setDisabled(false);
        Ext.each(App['formGestionEntidad'].body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && c.isFormField && !c.isValid()) {
                App['btnGuardar'].setDisabled(true);
            }
        });
    }
    else {
        App['btnGuardar'].setDisabled(true);
    }
}

function FormularioValidoOperador(valid) {
    if (valid) {
        App.btnGuardarOperador.setDisabled(false);
    }
    else {
        App.btnGuardarOperador.setDisabled(true);
    }
}

function FormularioValidoProveedor(valid) {
    if (valid) {
        App.btnGuardarProveedor.setDisabled(false);
    }
    else {
        App.btnGuardarProveedor.setDisabled(true);
    }
}

function FormularioValidoCompaniaProveedora(valid) {
    if (valid) {
        App.btnGuardarCompaniaProveedora.setDisabled(false);
    }
    else {
        App.btnGuardarCompaniaProveedora.setDisabled(true);
    }
}

function AgregarEditar() {

    VaciarFormulario();
    oOperador = null;
    oProveedor = null;
    oEmpresaProveedora = null;
    var combos = [App.cmbTipoEntidad];
    RecargarCombosLocalizaciones('locGeografica')
    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.btnEditarCompania.setDisabled(true)
                App.btnEditarProveedor.setDisabled(true)
                App.btnEditarOperador.setDisabled(true)
                App.WinGestionEntidad.setTitle(jsAgregar + ' ' + jsTituloEntidades);
                App.btnGuardar.setDisabled(true);
                App.hdControlFormulario.setValue('agregar')
                App.WinGestionEntidad.show();
                load.hide()
            }

        });
    })

}

function winGestionBotonGuardar() {
    if (App.formGestionEntidad.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditar() {

    var Agregar = false;

    if (App.WinGestionEntidad.title.startsWith(jsAgregar)) {
        Agregar = true;
    }

    TreeCore.AgregarEditar(Agregar, oOperador, oProveedor, oEmpresaProveedora,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.WinGestionEntidad.hide();
                    App.storePrincipal.reload();
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
    if (registroSeleccionado(App.gridEntities) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {

    VaciarFormulario();
    var combos = [App.cmbTipoEntidad];

    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.WinGestionEntidad.setTitle(jsEditar + ' ' + jsTituloEntidades);
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            App.hdControlFormulario.setValue('editar')
                            App.WinGestionEntidad.show();
                            load.hide()
                        }
                    }
                );
            }

        });
    })

}

function Eliminar() {
    if (registroSeleccionado(App.gridEntities) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsTituloEntidades,
                msg: jsMensajeEliminar,
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
                    App.storePrincipal.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

// #region Contactos

function GestionarContacto() {
    //App.txtBuscarMail.focus(false, 200);
    App.storeContactosGlobalesEntidadesVinculadas.reload();
    App.WinGestionContactos.show();
}

function buscador() {
    App.storeContactosGlobalesEntidadesVinculadas.reload();
}

function limpiar(value) {
    value.setValue("");
}

// #endregion

// #region Operador

function toogleOperador() {

    if (App.btnOperador.pressed == true) {
        App.btnEditarOperador.enable();
        App.winGestionOperador.show();
    } else {
        App.btnEditarOperador.disable();
        App.hdControlOperador.setValue("false");
        VaciarFormularioOperador();
        TreeCore.EliminarOperador(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    }
}

function GestionarComoOperador() {
    TreeCore.MostrarEditarOperador(oOperador,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                App.winGestionOperador.show();
            }
        }
    );
}

function cerrarWinOperador() {
    if (App.hdControlOperador.value == "false") {
        App.btnOperador.setPressed(false)
        App.btnEditarOperador.disable();
        VaciarFormularioOperador();
    } else if (App.hdControlOperador.value == "salvar") {
        TreeCore.MostrarEditarOperador(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                    }
                }
            }
        );
        App.hdControlOperador.setValue("false");
    }
}

function winGestionOperadorGuardar() {
    if (App.hdControlFormulario.value == 'agregar') {
        var Friendly = App.chkFriendly.checked;
        var Torrero = App.chkTorre.checked;
        var EsCliente = App.chkCliente.checked;
        oOperador = [Friendly, Torrero, EsCliente];
    }
    App.hdControlOperador.setValue("true");
    TreeCore.AgregarEditarOperador(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.winGestionOperador.hide();
            }
        }
    );
}

// #endregion

// #region Empresa Proveedora

function tooglebEditarCompania() {

    if (App.btnCompania.pressed == true) {
        App.btnEditarCompania.enable();
        GestionarComoCompania();
    }
    else {
        App.btnEditarCompania.disable();
        App.hdControlEmpresaProveedora.setValue('false');
        VaciarFormularioEmpresaProveedora();
        TreeCore.EliminarEmpresaProveedora(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    }
}

function GestionarComoCompania() {

    var combos = [App.cmbModulos];

    showLoadMask(App.WinGestionEntidad, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {

                App.btnEditarCompania.enable();
                App.winGestionCompaniaProveedora.show();
                load.hide();
            }
        });
    })

}

function GestionarComoEmpresaProveedoraMostrar() {
    var combos = [App.cmbModulos];

    if (App.hdControlEmpresaProveedora.value == "false") {
        showLoadMask(App.WinGestionEntidad, function (load) {
            recargarCombos(combos, function (fin) {
                if (fin) {
                    TreeCore.MostrarEditarEmpresaProveedora(oEmpresaProveedora,
                        {
                            success: function (result) {
                                if (result.Result != null && result.Result != '') {
                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                }

                                App.btnEditarCompania.enable();
                                App.winGestionCompaniaProveedora.show();
                                load.hide()
                            }
                        }
                    );


                }
            });
        })
    } else {
        App.winGestionCompaniaProveedora.show();
    }


}

function winGestionCompaniaProveedoraGuardar() {
    if (App.hdControlFormulario.value == 'agregar') {
        var Modulos = App.cmbModulos.value;
        oEmpresaProveedora = Modulos;
    }
    App.hdControlEmpresaProveedora.setValue("true")
    TreeCore.AgregarEditarEmpresaProveedora(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.hdControlEmpresaProveedora.setValue("false");
                App.winGestionCompaniaProveedora.hide();
            }
        }
    );

}

function cerrarWinEmpresaProveedora() {
    App.storePrincipal.reload();
    if (hdControlFormulario.value == "agregar") {
        if (oEmpresaProveedora == null) {
            App.btnCompania.setPressed(false);
            App.btnEditarCompania.disable();
            VaciarFormularioEmpresaProveedora();
        } else {
            TreeCore.MostrarEditarEmpresaProveedora(oEmpresaProveedora,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                        }
                    }
                }
            );
            App.hdControlEmpresaProveedora.setValue("false");
        }
    } else {
        if (App.hdControlEmpresaProveedora.value == "false") {
            TreeCore.MostrarEditarEmpresaProveedora(oEmpresaProveedora,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                        }
                    }
                }
            );

        }

    }

}

// #endregion

// #region Proveedor

function GestionarComoProveedorMostrar() {
    var combos = [App.cmbMetodoPago,
    App.cmbTipoContribuyente,
    App.cmbIdentificacion,
    App.cmbTratamiento,
    App.cmbGrupoCuenta,
    App.cmbCuenta,
    App.cmbClaveClasificacion,
    App.cmbTesoreria,
    App.cmbCondicionPago];

    if (App.hdControlProveedor.value == "false") {
        showLoadMask(App.WinGestionEntidad, function (load) {
            recargarCombos(combos, function (fin) {
                if (fin) {
                    TreeCore.MostrarEditarProveedor(oProveedor,
                        {
                            success: function (result) {
                                if (result.Result != null && result.Result != '') {
                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                }

                                App.btnEditarProveedor.enable();
                                App.winGestionProveedor.show();
                                load.hide()
                            }
                        }
                    );


                }
            });
        })
    } else {
        App.winGestionProveedor.show();
    }


}

function toogleProveedor() {

    if (App.btnProveedor.pressed == true) {
        GestionarComoProveedor();
    }
    else {
        App.btnEditarProveedor.disable();
        App.hdControlProveedor.setValue('false');
        VaciarFormularioProveedor();
        TreeCore.EliminarProveedor(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            }
        );
    }
}

function GestionarComoProveedor() {

    var combos = [App.cmbMetodoPago,
    App.cmbTipoContribuyente,
    App.cmbIdentificacion,
    App.cmbTratamiento,
    App.cmbGrupoCuenta,
    App.cmbCuenta,
    App.cmbClaveClasificacion,
    App.cmbTesoreria,
    App.cmbCondicionPago];

    showLoadMask(App.WinGestionEntidad, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.btnEditarProveedor.enable();
                App.winGestionProveedor.show();
                load.hide()
            }
        });
    })
}

function winGestionProveedorGuardar() {
    if (App.hdControlFormulario.value == 'agregar') {
        var MetodoPago = App.cmbMetodoPago.value;
        var TipoContribuyente = App.cmbTipoContribuyente.value;
        var Identificacion = App.cmbIdentificacion.value;
        var Tratamiento = App.cmbTratamiento.value;
        var GrupoCuenta = App.cmbGrupoCuenta.value;
        var Cuenta = App.cmbCuenta.value;
        var ClaveClasificacion = App.cmbClaveClasificacion.value;
        var Tesoreria = App.cmbTesoreria.value;
        var CondicionPago = App.cmbCondicionPago.value;
        oProveedor = [MetodoPago, TipoContribuyente, Identificacion, Tratamiento, GrupoCuenta, Cuenta, ClaveClasificacion, Tesoreria, CondicionPago];
    }
    App.hdControlProveedor.setValue("true");
    TreeCore.AgregarEditarProveedor(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.hdControlProveedor.setValue("false");
                App.winGestionProveedor.hide();
            }
        }
    );

}

function cerrarWinProveedor() {
    if (hdControlFormulario.value == "agregar") {
        if (oProveedor == null) {
            App.btnProveedor.setPressed(false);
            App.btnEditarProveedor.disable();
        } else {
            TreeCore.MostrarEditarProveedor(oProveedor,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                        }
                    }
                }
            );
            App.hdControlProveedor.setValue("false");
        }
    } else {
        if (App.hdControlProveedor.value == "false") {
            TreeCore.MostrarEditarProveedor(oProveedor,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                        }
                    }
                }
            );

        }

    }

}


// #endregion

// #endregion


// #region Propietario
function togglePropietario() {
    if (App.btnPropietario.pressed == false) {
        TreeCore.EliminarPropietario(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    } else {
        if (App.hdControlFormulario.value != 'agregar') {
            TreeCore.AñadirPropietario(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                        }
                    },
                    eventMask:
                    {
                        showMask: true,
                        msg: jsMensajeProcesando
                    }
                });
        }

    }
}
// #endregion

function GenerarPlantillaEntidades() {
    TreeCore.GenerarPlantillaEntidades(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function NavegacionTabs(who, PaginaACargar, NombrePagina, script) {
    var LNo = who.textEl;
    var test = PaginaACargar;

    switch (who.id) {
        case 'lnkEntities':
            App.ctHuge.show();
            App.hugeCt.hide();
            document.getElementById("lnkContacts").lastChild.classList.remove("navActivo")
            document.getElementById("lnkEntities").lastChild.classList.add("navActivo")
            break;

        case 'lnkContacts':

            App.ctHuge.hide();
            App.hugeCt.show();
            document.getElementById("lnkEntities").lastChild.classList.remove("navActivo")
            document.getElementById("lnkContacts").lastChild.classList.add("navActivo");
            break;

    }
}

// #region GESTIÓN CONTACTOS

function cambiarAsignacion(sender, registro, index) {

    var idComponente = sender.record.store.config.storeId.split('_')[0];

    TreeCore.AsignarEntidad(sender.record.get('ContactoGlobalID'),
        sender.record.get('EntidadID'),
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeContactosGlobalesEntidadesVinculadas.reload();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

var SetToggleValue = function (column, cmp, record) {

    var lEntSelecID = App.GridRowSelect.selected.items[0].data.EntidadID;

    var lEntID = record.get('EntidadID');

    if (lEntSelecID == lEntID) {
        cmp.setPressed(true);
    }
    else {
        cmp.setPressed(false);
    }

    cmp.updateLayout();

}

function Grid_RowSelectContactos(sender, registro, index) {
    var ruta = sender.config.proxyId.split('_');
    ruta.pop();
    ruta = ruta.join('_');
}

function agregarContactoEntidad(sender, registro, index) {
    var idComponente = sender.id.split('_')[0];
    var ControlURL = '/Componentes/FormContactos.ascx';
    var NombreControl = 'FormContactos';

    var achjs = ControlURL.split('/');
    achjs.pop(); achjs = achjs.join('/') + '/js/' + NombreControl + '.js';
    AñadirScriptjs(achjs);
    App.hdControlURL.value = ControlURL;
    App.hdControlName.value = NombreControl;

    CargarVentanaContactos(sender, 'formAgregarEditarContacto');
    App.winAgregarContacto.setTitle(jsAgregar);
    App.winAgregarContacto.show();

}

function editarContactoEntidad(sender, registro, index) {
    //DefinirDatosContacto(idComponente, App.GridRowSelectContacto.selected.items[0].data);
    let contactoID = this.getWidgetRecord().id;
    TreeCore.MostrarEditarContacto(contactoID,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    CargarVentanaContactos(undefined, 'formAgregarEditarContacto');
                    App.winAgregarContacto.setTitle(jsEditar);
                    App.winAgregarContacto.show();
                }
            },
            eventMask:
            {
                showMask: true
            }
        });
}




// #endregion

// #region RESIZER

function winFormResize(obj) {
    var res = window.innerWidth;

    if (res > 670) {
        obj.setWidth(600);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }

    obj.center();
}

function formResize(obj) {
    var res = window.innerWidth;

    if (res > 670) {
        obj.setWidth(570);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
}

function winFormContactosResize(obj) {
    var res = window.innerWidth;

    if (res > 670) {
        obj.setWidth(500);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }

    obj.center();
}

function formContactosResize(obj) {
    var res = window.innerWidth;

    if (res > 670) {
        obj.setWidth(500);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
}

function winFormProveedorResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(800);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }

    obj.center();
}

function formProveedorResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(800);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(620);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
}

function winFormResizeDockBot(obj) {

    //AQUI SE SETEA EL CENTER ABAJO

    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);
    obj.setY(vh - obj.height);

    //obj.update();
}

function winCenter(obj) {
    obj.center();
}

function resizeWinForm() {
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

    var dv = document.querySelectorAll('div.WinFormContactos-resp');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormContactosResize(obj);
    }

    var frm = document.querySelectorAll('div.ctFormContactos-resp');
    for (i = 0; i < frm.length; i++) {
        var obj = Ext.getCmp(frm[i].id);
        formContactosResize(obj);
    }

    var dv = document.querySelectorAll('div.WinFormProveedor-resp');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormProveedorResize(obj);
    }

    var frm = document.querySelectorAll('div.ctFormProveedor-resp');
    for (i = 0; i < frm.length; i++) {
        var obj = Ext.getCmp(frm[i].id);
        formProveedorResize(obj);
    }

    var dv = document.querySelectorAll('div.winForm-respDockBot');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResizeDockBot(obj);
    }

    var frm = document.querySelectorAll('div.winFormCenter');
    for (i = 0; i < frm.length; i++) {
        var obj = Ext.getCmp(frm[i].id);
        winCenter(obj);
    }
}

window.addEventListener('resize', resizeWinForm);

// #endregion
