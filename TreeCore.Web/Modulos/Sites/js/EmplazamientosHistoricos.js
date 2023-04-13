

// #region CONTROLES BASICOS PAGINA (ASIDE ETC)

function hideAsideR(panel) {

    App.btnCollapseAsRClosed.show();

    var asideR = Ext.getCmp('pnAsideR');
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (asideR.collapsed == false) {

        btn.style.transform = 'rotate(-180deg)';
        App.pnAsideR.collapse();
    }
    else {
        btn.style.transform = 'rotate(0deg)';
        App.pnAsideR.expand();

    }

    if (panel != null) {

        App.WrapFilterControls.hide();
        App.WrapGestionColumnas.hide();
        App.pnMoreInfo.hide();

        switch (panel) {

            case "panelMore":
                App.pnMoreInfo.show();

                btn.style.transform = 'rotate(0deg)';
                App.pnAsideR.expand();

                break;

            case "panelFiltros":
                App.WrapFilterControls.show();

                btn.style.transform = 'rotate(0deg)';
                App.pnAsideR.expand();

                break;

            case "panelColumnas":

                App.WrapGestionColumnas.show();

                btn.style.transform = 'rotate(0deg)';
                App.pnAsideR.expand();

                break;

        }

    }
    GridColHandler();

    window.dispatchEvent(new Event('resize'));

}

function seleccionarHistorico(sender, registro, index) {

    TreeCore.JSONtoEmplazamiento(registro.data['DatosJSON'],
        {
            success: function (result) {

                if (result.Result != null && result.Result != '') {

                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask:
            {

                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

// #endregion

//  #region RESIZERS PARA VENTANAS MODALES (CALCULO EXTERNO)

function winFormCenterSimple(obj) {

    obj.center();
    obj.updateLayout();

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
    obj.updateLayout();

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
    obj.updateLayout();

    //AQUI SE SETEA EL CENTER ABAJO

    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);
    obj.setY(vh - obj.height);

    obj.updateLayout();

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
    obj.updateLayout();

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
    var asideR = Ext.getCmp('pnAsideR');

    if (winsize < 520 && asideR.collapsed == false) {
        App.CenterPanelMain.hide();
        App.pnAsideR.setWidth(winsize);
    }
    else {
        App.CenterPanelMain.show();
        App.pnAsideR.setWidth(380);

    }

});

// #endregion

function displayMenu(btn) {

    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnCFilters.hide();
    App.pnGridsAsideMyFilters.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();
    PanelAMostrar.updateLayout();

}

function showwinAddTab() {
    App.winAddTabFilter.show();

}

function showwinSaveQF() {
    App.winSaveQF.show();

}

// #region BTNS NAV

// #endregion

// #region EMPLAZAMIENTOS HISTORICOS

// ESTADO POR DEFECTO DE LOS PANELES
var bShowPrincipal = true;
var bShowOnlySecundary = false;
var iSelectedPanel = 0;

function showPanelsByWindowSize() {

    let puntoCorte = 512;
    var tmn = App.CenterPanelMain.getWidth();

    if (tmn < puntoCorte) {

        App.btnPrev.show();
        App.btnNext.show();
        App.btnCalendarToGrid.hide();
        loadPanelByBtns("");
    }
    else {

        App.btnPrev.hide();
        App.btnNext.hide();
        App.btnCalendarToGrid.show();
        loadPanels();
    }
}

function loadPanels() {

    if (bShowOnlySecundary) {

        App.pnCalendario.hide();
        App.gridPrincipal.hide();
        App.btnCalendarToGrid.setIconCls('btnCalendar');
        App.btnCalendarToGrid.setTooltip(jsCalendario);
        App.btnCloseShowVisorTreeP.setIconCls('ico-moverow-gr');
    }
    else {

        App.pnSecundario.show();

        if (bShowPrincipal) {

            App.pnCalendario.hide();
            App.gridPrincipal.show();
            App.btnCalendarToGrid.setIconCls('btnCalendar');
            App.btnCalendarToGrid.setTooltip(jsCalendario);
        }
        else {

            App.pnCalendario.show();
            App.gridPrincipal.hide();
            App.btnCalendarToGrid.setIconCls('btnLista');
            App.btnCalendarToGrid.setTooltip(jsVistaCategoria);
        }

        App.btnCloseShowVisorTreeP.setIconCls('ico-close-gr');
    }
}

function loadPanelByBtns(pressedBtn) {

    // CHECK FOR A PRESSED BTN
    if (pressedBtn != "") {
        if (pressedBtn == "Next") {
            iSelectedPanel++;
        }
        else {
            iSelectedPanel--;
        }
    }

    // CHECK FOR DISABLED BUTTONS
    if (iSelectedPanel == 2) {
        App.btnNext.disable();
    }
    else if (iSelectedPanel == 0) {
        App.btnPrev.disable();
    }
    else {
        App.btnPrev.enable();
        App.btnNext.enable();
    }

    // LOAD PANEL
    if (iSelectedPanel == 0) {
        App.gridPrincipal.show();
        App.pnCalendario.hide();
        App.pnSecundario.hide();
    }
    if (iSelectedPanel == 1) {
        App.gridPrincipal.hide();
        App.pnCalendario.show();
        App.pnSecundario.hide();
    }
    if (iSelectedPanel == 2) {
        App.gridPrincipal.hide();
        App.pnCalendario.hide();
        App.pnSecundario.show();
    }
}

function showPrincipalOrCalendar() {

    if (bShowOnlySecundary) {

        bShowPrincipal = false;
        bShowOnlySecundary = false;
    }
    else {

        bShowPrincipal = !bShowPrincipal;
    }

    loadPanels();
}

function showOnlySecundary() {

    bShowOnlySecundary = !bShowOnlySecundary;
    loadPanels();
}

function crearColumnaEstadoActualEmplazamientos() {

    var main = TreeCore.GetEstadoActualEmplazamientoJSON(
        {

            success: function (result) {

                if (result.Result != null) {

                    crearColumnaEstadoActual(result.Result);
                }
            }
        });

    Ext.onReady(main);
}

function agregarColumnaDinamicaEmplazamientos(sender, registro, index) {

    TreeCore.GetEstadoActualEmplazamientoJSON(
        {

            success: function (result) {

                if (result.Result != null) {

                    let oEstadoActualJSON = result.Result;
                    agregarColumnaDinamicaDesdeStore(sender, registro, index, oEstadoActualJSON);
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function agregarColumnaDinamicaEmplazamientosCalendario(data, event) {

    TreeCore.GetEstadoActualEmplazamientoJSON(
        {

            success: function (result) {

                if (result.Result != null) {

                    let oEstadoActualJSON = result.Result;
                    agregarColumnaDinamicaDesdeCalendario(data, event, oEstadoActualJSON);
                }
            }
        });
}

function cargarEventosCalendario(sender, registro, index) {

    var events = [];
    for (i = 0; i < registro.length; i++) {

        var event = {

            id: registro[i].data.HistoricoCoreEmplazamientoID,
            date: registro[i].data.FechaModificacion,
            name: registro[i].data.FechaModificacion.toLocaleString(),
            type: 'event',

            json: JSON.stringify(registro[i].data.DatosJSON),  // CUSTOM PARAM
            creator: App.gridPrincipal.getRowsValues()[i].NombreCompleto // CUSTOM PARAM
        }

        events.push(event);
    }

    $('#evoCalendar').evoCalendar({

        calendarEvents: events,
    }).on('selectEvent', function (data, event) {

        agregarColumnaDinamicaEmplazamientosCalendario(data, event);
    });
}

// #endregion

function Refrescar() {
    App.storePrincipal.reload();
}

function Descargar() {
    let historicos = [];
    for (var item in App.gridPrincipal.store.data.items) {
        historicos.push(App.gridPrincipal.store.data.items[item].data.HistoricoCoreEmplazamientoID);
    }
    TreeCore.ExportarEmplazamientos(historicos,
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}
