// #region DISEÑO
//ESCONDER CENTRO CUANDO ASIDE PISA MUCHO EL CONTENIDO PARA SER UTIL

var GridPnl = document.getElementsByClassName("gridSimpleMenu");
var TreepnL = document.getElementsByClassName("TreePnl");
var forcedVisor = false;
var PuntoCorteS = 512;

function VisorSwitch(sender) {

    var asideL = Ext.getCmp(['grdMenuKPICategory']);
    var btnclose = Ext.getCmp(['btnCloseShowVisorTreeP']);

    if (asideL.hidden == true) {
        forcedVisor = false;
        btnclose.setIconCls('ico-hide-menu');
        App.grdMenuKPICategory.show();

    }
    else {
        forcedVisor = true;
        btnclose.setIconCls('ico-moverow-gr');
        App.grdMenuKPICategory.hide();

    }

}

function ControlPaneles(sender) {

    var winsize = window.innerWidth;

    var containerSize = App.CenterPanelMain.getWidth();

    if (forcedVisor != true) {

        //HIDE PANEL2
        if (containerSize < 480) {
            GridPnl[0].classList.remove("TreePL");
            App.grdMenuKPICategory.maxWidth = 9999;
            App.gridMain1.hide();

        }
        else {
            GridPnl[0].classList.add("TreePL");
            App.grdMenuKPICategory.maxWidth = 300;
            App.gridMain1.show();

        }

        //HIDE PANEL1
        if (containerSize < 120) {
            App.grdMenuKPICategory.hide();

        }
        else {
            App.grdMenuKPICategory.show();

        }

    }
    else {

        if (containerSize < 160) {
            App.gridMain1.hide();
            GridPnl[0].classList.remove("TreePL");

        }
        else {
            App.gridMain1.show();
            GridPnl[0].classList.add("TreePL");

        }
    }

    if (containerSize < PuntoCorteS) {

        App.tbSliders.show();

        if (App.grdMenuKPICategory.hidden == false && App.gridMain1.hidden == true) {
            App.btnPrev.disable();

        }
    }
    else {
        App.tbSliders.hide();
        App.btnPrev.disable();
        App.btnNext.enable();

    }
}

function SliderMove(NextOrPrev) {

    if (NextOrPrev == 'Next') {
        forcedVisor = true;
        App.grdMenuKPICategory.hide();
        App.gridMain1.show();

        App.btnPrev.enable();
        App.btnNext.disable();

    }
    else {
        forcedVisor = false;
        App.grdMenuKPICategory.show();
        App.gridMain1.hide();

        App.btnPrev.disable();
        App.btnNext.enable();

    }
}

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

    //ESCONDER CENTRO CUANDO ASIDE PISA MUCHO EL CONTENIDO PARA SER UTIL
    //var pnCentral = document.getElementsByClassName("pnCentralWrap");
    var winsize = window.innerWidth;
    var asideR = Ext.getCmp('gridMain1');


    if (winsize < 540 && asideR.collapsed == false) {
        App.grdMenuKPICategory.hide();
        App.gridMain1.setWidth(winsize);
    }
    else {
        App.grdMenuKPICategory.show();
        App.gridMain1.setWidth(380);

    }

});

// #endregion

function displayMenuKPI(btn) {


    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnNotificationsFull.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();
    PanelAMostrar.updateLayout();

}
var spPnLite = 0;
function hidePnLite() {
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (spPnLite == 0) {
        btn.style.transform = 'rotate(0deg)';
        spPnLite = 1;
    }
    else {
        btn.style.transform = 'rotate(-180deg)';
        spPnLite = 0;

    }
}

// #region CONTROL RESPONSIVE TOOLBAR 2 COMBOS


function StyleOnResize(sender) {
    var tbFiltros = document.getElementsByClassName("tlbGridRes");
    var ruta = getIdComponente(sender);

    //CONTROL PARA CUANDO NO HAYA BOTONES, SOLO COMBO FILTROS
    var GrupoBtnFilters = document.getElementsByClassName("GrupoBtnFilters");
    var BotonesVisibles = GrupoBtnFilters[0].style.display;

    var cmbMisfiltros = document.getElementsByClassName("cmbMisfiltros");
    var cmbProjects = document.getElementsByClassName("cmbProjects");

    if (BotonesVisibles == "none") {
        cmbMisfiltros[0].classList.add("cmbMisfiltrosNoBtns");
    }
    if (BotonesVisibles == "none") {
        cmbProjects[0].classList.add("cmbMisfiltrosNoBtns");
    }

    // ------------------------------------------

    for (i = 0; i < tbFiltros.length; i++) {

        if (tbFiltros[i] != null) {

            if (tbFiltros[i].clientWidth < 612) {

                tbFiltros[i].classList.add("tlbGridResMid");

            }
            else {

                tbFiltros[i].classList.remove("tlbGridResMid");

            }

            if (tbFiltros[i].clientWidth < 460) {

                tbFiltros[i].classList.add("tlbGridResMini");

            }
            else {
                tbFiltros[i].classList.remove("tlbGridResMini");

            }
            App[ruta + 'tbFiltros'].updateLayout();

        }

    }

}

// #endregion

// #endregion

// #region PANEL IZQUIERDO

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.hdCategoria.setValue(seleccionado.Categoria);
        App.lblTituloGrid.setText(seleccionado.Categoria);
        App.storeDQKpisMonitoring.reload();
    }
}

function DeseleccionarGrilla() {
    App.GridRowSelect.clearSelections();
}

function Refrescar() {
    App.storePrincipal.reload();
}

function asignarColumna(sender, registro, index) {
    if (index != undefined && index.data.ID == 1) {
        return (index.data.Categoria);
    }
    else {
        return (index.data.Categoria + '(' + index.data.Valores + ')');
    }
}

// #endregion

// #region PANEL CENTRAL

var ColorRender = function (value) {
    if (value == "green") {
        return '<span class="green">&nbsp;</span>'
    }
    else if (value == "yellow") {
        return '<span class="yellow">&nbsp;</span>'
    }
    else if (value == "red") {
        return '<span class="red">&nbsp;</span>'
    }
    else if (value == "error") {
        return '<span class="gen_Inactivo">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storeDQKpiGroupMonitoring.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeDQKpiGroupMonitoring.pageSize = wantedPageSize;
        App.storeDQKpiGroupMonitoring.load();
    }
}

function Grid_RowSelectGrilla(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadaGrilla = datos;
        App.btnTime.enable();
        App.btnDetalleKPI.enable();
        App.btnTime.setTooltip(jsEjecutar);
        App.btnDetalleKPI.setTooltip(jsFiltros);

        let registroSeleccionado = registro;
        let GridSeleccionado = App.gridMain1;
        parent.cargarDatosPanelMoreInfoCalidad(registroSeleccionado, GridSeleccionado);
        parent.App.hdKPISeleccionado.setValue(seleccionadaGrilla.DQKpiID);
        parent.cargarDatosPanelLateral(seleccionadaGrilla.DQKpiID);
    }
}

function DeseleccionarGrillaMonitoring() {
    App.GridRowSelectGrilla.clearSelections();
    App.btnTime.disable();
    App.btnDetalleKPI.disable();
}

function RefrescarGrid() {
    App.storeDQKpisMonitoring.reload();
}

function ejecutarKPI(sender, registro, index) {
    TreeCore.ejecutarKPI(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    if (result.Success) {
                        Ext.Msg.alert({
                            title: jsEjecucion,
                            icon: Ext.MessageBox.INFO,
                            msg: jsEjecucionRealizada
                        });
                    }
                    else {
                        Ext.Msg.alert({
                            title: jsEjecucion,
                            icon: Ext.MessageBox.WARNING,
                            msg: jsEjecucionFallida
                        });
                    }

                    RefrescarGrid();
                }
            },
            eventMask: {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

// #endregion

// #region PANEL DERECHO

function ExpOpenExport() {
    App.winGestion.show();
}

// #endregion

