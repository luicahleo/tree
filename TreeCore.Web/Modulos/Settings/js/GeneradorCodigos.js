var Agregar = false;
var seleccionado;

// #region Diseño

var bShowOnlySecundary = false;
var iSelectedPanel = 0;

function showPanelsByWindowSize() {

    let puntoCorte = 600;
    var tmn = App.CenterPanelMain.getWidth();

    if (tmn < puntoCorte) {
        App.tbFiltrosYSliders.show();
        App.btnPrev.show();
        App.btnNext.show();
        //App.btnCloseShowVisorTreeP.disable();
        loadPanelByBtns("");
    }
    else {
        App.tbFiltrosYSliders.hide()
        App.btnPrev.hide();
        App.btnNext.hide();
        //App.btnCloseShowVisorTreeP.enable();
        loadPanels();
    }
}

function loadPanels() {
    var btnclose = Ext.getCmp(['btnCloseShowVisorTreeP']);

    if (bShowOnlySecundary) {
        App.ctMain1.hide();
        //btnclose.setIconCls('ico-moverow-gr');
    }
    else {
        App.ctMain2.show();
        App.ctMain1.show();
        //btnclose.setIconCls('ico-hide-menu');
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
    if (iSelectedPanel == 1) {
        App.btnPrev.enable();
        App.btnNext.disable();
    }
    else {
        App.btnPrev.disable();
        App.btnNext.enable();
    }

    // LOAD PANEL
    if (iSelectedPanel == 0) {
        App.ctMain1.show();
        App.ctMain2.hide();
    }
    if (iSelectedPanel == 1) {
        App.ctMain1.hide();
        App.ctMain2.show();
    }

}

function showOnlySecundary() {
    bShowOnlySecundary = !bShowOnlySecundary;
    loadPanels();
}

setTimeout(function () {

    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
    winFormResize();

}, 1000);

function winFormResize() {

    var res = window.innerWidth;
    try {

        if (res > 1024) {
            App.winGestion.setWidth(862)
        }

        if (res <= 1024 && res > 670) {
            App.winGestion.setWidth(445)
            App.winGestion.setHeight(450)
        }

        if (res <= 670) {
            App.winGestion.setWidth(425)
            App.winGestion.setHeight(430)
        }
    } catch (e) {

    }
}

window.addEventListener('resize', function () {
    winFormResize();
});

function resizeCenterPanel(sender) {
    SetMaxHeightSuperiorCenter(App.CenterPanelMain);
}

function SetMaxHeightSuperiorCenter(sender) {
    var tamCenter = sender.getHeight();
    var tamPadre = sender.up().getHeight();

    if (App.tbCollapseAsR.hidden == false) {
        sender.setMinHeight(tamCenter - 32);
        sender.setMaxHeight(tamCenter - 32);
        sender.updateLayout();
    }
    else if (App.tbFiltrosYSliders.hidden == false) {
        sender.setMinHeight(tamCenter - 30);
        sender.setMaxHeight(tamCenter - 30);
        sender.updateLayout();
    }
    else {
        sender.setMinHeight(tamPadre - 25);
        sender.setMaxHeight(tamPadre - 25);
        sender.updateLayout();
    }
}

// #endregion

var stPn = 1;

function hidePnFilters() {
    let pn = App.pnAsideR;
    if (stPn == 0) {
        pn.expand();
        App.tbCollapseAsR.show();
        App.btnCollapseAsRClosed.show();
        btnCollapsedHide = false;
        stPn = 1;
    }
    else {
        pn.collapse();
        App.tbCollapseAsR.hide();
        App.btnCollapseAsRClosed.hide();
        btnCollapsedHide = true;
        stPn = 0;
    }

}

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




// #endregion

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

window.addEventListener('resize', function () {
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

});

// #region Refrescar
function Refrescar() {
    ajaxRefreshArbol();

    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnDefecto.disable();

    App.btnAnadirDetalle.disable();
    App.btnRefrescarDetalle.disable();
    App.btnDescargarDetalle.disable();

    App.btnSimularCodigo.disable();
    App.txtSimulacionCodidoSiguinete.setValue("");
    App.txtSimulacionCodido.setValue("");
    App.hd_MenuSeleccionado.setValue(0);
    App.storeGlobalCondicionReglaConfiguracion.reload();
}

function RefrescarDetalle() {
    App.storeGlobalCondicionReglaConfiguracion.reload();
}
// #endregion

// #region Agregar

function AgregarEditar() {
    VaciarFormulario();

    Agregar = true;

    recargarCombos([App.cmbModulo], function Fin(fin) {
        if (fin) {
            App.winSaveNewTabCols.setTitle(jsAgregar + " " + jsPatron);
            App.winSaveNewTabCols.show();
            load.hide();
        }
    });
}

function AgregarEditarDetalle() {
    VaciarFormulario()
    App.storeProyectosTipos.reload();
    Agregar = true;
    App.winGestion.setTitle(jsAgregar + " " + jsTituloModulo);
    App.cmbFormulario.hide();
    App.cmbColumnaFormulario.hide();
    App.cmbTabla.hide();
    App.cmbColumnaTabla.hide();
    App.txtValor.hide();
    App.txtLongitud.hide();
    App.cmbTipoCondicion.enable();
    App.winGestion.show();
}

function BotonGuardar() {

    TreeCore.AgregarEditar(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {

                    App.winSaveNewTabCols.hide();
                }
                ajaxRefreshArbol();
            }
        });

}

function BotonGuardarDetalle() {

    TreeCore.AgregarEditarDetalle(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestion.hide();
                    VaciarFormulario();
                }

                App.storeGlobalCondicionReglaConfiguracion.reload();
            }
        });

}


// #endregion

function VaciarFormulario() {

    App.formGestion.getForm().reset();
    App.FormSaveNewTabCols.getForm().reset();

    Ext.each(App.formGestion.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.reset();

            if (c.triggerWrap != undefined) {
                c.triggerWrap.removeCls("itemForm-novalid");
            }

            if (!c.allowBlank && c.xtype != "checkboxfield") {
                c.addListener("change", anadirClsNoValido, false);
                c.addListener("focusleave", anadirClsNoValido, false);
                //c.addListener("select", FormularioValidoEntidades, false);

                c.removeCls("ico-exclamacion-10px-red");
                c.addCls("ico-exclamacion-10px-grey");
            }

        }
    });

    SeleccionarTipoCondicion();
    App.btnGuardarDetalle.disable();
}

function SeleccionarTipoCondicion() {



    if (App.cmbTipoCondicion.getValue() == 'Formulario') {

        App.txtNombreCodigo.allowBlank = false;
        App.txtNombreCodigo.enable();


        App.cmbFormulario.clearValue();
        RecargarFormularios();
        App.cmbFormulario.allowBlank = false;
        App.cmbFormulario.show();

        App.txtValor.setValue("");
        App.txtValor.allowBlank = true;
        App.txtValor.hide();


        App.cmbColumnaFormulario.clearValue();
        RecargarColumnaFormulario();
        App.cmbColumnaFormulario.allowBlank = false;
        App.cmbColumnaFormulario.show();

        //FIN Ocultar y limpiar

        App.cmbTabla.clearValue();
        App.cmbTabla.allowBlank = true;
        App.cmbTabla.hide();

        App.txtLongitud.setValue("");
        App.txtLongitud.allowBlank = false;
        App.txtLongitud.show();

        App.cmbColumnaTabla.clearValue();
        App.cmbColumnaTabla.allowBlank = true;
        App.cmbColumnaTabla.hide();

        //INICIO Ocultar y limpiar

    }
    else if (App.cmbTipoCondicion.getValue() == 'Tabla') {

        App.txtNombreCodigo.allowBlank = false;
        App.txtNombreCodigo.enable();

        //INICIO Ocultar y limpiar

        App.cmbFormulario.clearValue();
        App.cmbFormulario.allowBlank = true;
        App.cmbFormulario.hide();

        App.txtValor.setValue("");
        App.txtValor.allowBlank = true;
        App.txtValor.hide();

        App.cmbColumnaFormulario.clearValue();
        App.cmbColumnaFormulario.allowBlank = true;
        App.cmbColumnaFormulario.hide();
        //FIN Ocultar y limpiar


        //INICIO MOSTRAR Y RECARGAR
        App.cmbTabla.clearValue();
        RecargarTablas();
        App.cmbTabla.allowBlank = false;
        App.cmbTabla.show();

        App.txtLongitud.setValue("");
        App.txtLongitud.allowBlank = false;
        App.txtLongitud.show();

        App.cmbColumnaTabla.clearValue();
        RecargarColumnaTabla();
        App.cmbColumnaTabla.allowBlank = false;
        App.cmbColumnaTabla.show();
        //FIN MOSTRAR Y RECARGAR
    }
    else {

        App.txtValor.setValue("");
        App.txtValor.allowBlank = false;
        App.txtValor.show();

        App.txtValor.addListener("change", anadirClsNoValido, false);
        App.txtValor.addListener("focusleave", anadirClsNoValido, false);
        App.txtValor.removeCls("ico-exclamacion-10px-red");
        App.txtValor.addCls("ico-exclamacion-10px-grey");

        App.cmbFormulario.clearValue();
        App.cmbFormulario.allowBlank = true;
        App.cmbFormulario.hide();

        App.cmbColumnaFormulario.clearValue();
        App.cmbColumnaFormulario.allowBlank = true;
        App.cmbColumnaFormulario.hide();

        App.cmbTabla.clearValue();
        App.cmbTabla.allowBlank = true;
        App.cmbTabla.hide();

        App.txtLongitud.setValue("");
        App.txtLongitud.allowBlank = true;
        App.txtLongitud.hide();

        App.cmbColumnaTabla.clearValue();
        App.cmbColumnaTabla.allowBlank = true;
        App.cmbColumnaTabla.hide();

        if (App.cmbTipoCondicion.getValue() == 'Auto_Numerico') {
            TreeCore.RegexTextFieldNumerico(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
                        }
                    }
                });

            //App.txtValor.regex = "^[0-9]*$", "i";
            //App.txtValor.regexText = "HOLA";
            //App.formGestion.reload();
        }

        else if (App.cmbTipoCondicion.getValue() == 'Auto_Caracter') {
            TreeCore.RegexTextFieldCaracter(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
                        }
                    }
                });

            //App.txtValor.regex = "^[a-zA-Z]*$", "i";
            //App.txtValor.regexText = "HOLA";
        }

        else if (App.cmbTipoCondicion.getValue() == 'Separador') {
            TreeCore.RegexTextFieldSeparador(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
                        }
                    }
                });

            //App.txtValor.regex = "[^A-Za-z0-9_]", "i";
            //App.txtValor.regexText = "HOLA";
        }

        else if (App.cmbTipoCondicion.getValue() == 'Constante') {
            TreeCore.RegexTextFieldConstante(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
                        }
                    }
                });

            //App.txtValor.regex = null, "i";
            //App.txtValor.regexText = null;
        }



    }
}

// #region Editar

function MostrarEditar() {
    ajaxEditar();
}

function ajaxEditar() {

    Agregar = false;

    recargarCombos([App.cmbModulo], function Fin(fin) {
        if (fin) {
            TreeCore.MostrarEditar(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        else {
                            App.winSaveNewTabCols.setTitle(jsEditar + " " + jsPatron);
                            App.winSaveNewTabCols.show();
                        }
                    }
                });
        }
    });
}

function MostrarEditarDetalle() {
    ajaxEditarDetalle();
}

function ajaxEditarDetalle() {


    Agregar = false;
    App.txtLongitud.hide();
    App.txtLongitud.allowBlank = true;

    showLoadMask(App.gridReglasCodigos, function (load) {
        recargarCombos([App.cmbFormulario, App.cmbColumnaFormulario]);

        TreeCore.MostrarEditarDetalle(
            {
                success: function (result) {

                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {

                        //CargarStoresSerie([App.storeGlobalCondicionesFormularios, App.storeGlobalCondicionesColumnasFormularios],
                        //    function Fin(fin) {
                        //        //App.cmbColumnaFormulario.setValue(hdFormulario.value);
                                App.winGestion.setTitle(jsEditar + " " + jsTituloModulo);
                                App.winGestion.show();
                            //});
                    }
                    App.storeGlobalCondicionReglaConfiguracion.reload();
                    load.hide();
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    });
}

// #endregion

// #region Eliminar

function Eliminar() {
    if (seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + " " + jsTituloModulo,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION,
                cls: 'winAlert'
            });
    }
}

function ajaxEliminar(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.Eliminar({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {

                    ajaxRefreshArbol();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function EliminarDetalle() {
    if (registroSeleccionado(App.gridReglasCodigos) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + " " + jsTituloModulo,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarDetalle,
                icon: Ext.MessageBox.QUESTION,
                cls: 'winAlert'
            });
    }
}

function ajaxEliminarDetalle(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarDetalle({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    ajaxRefreshArbol();

                    App.storeGlobalCondicionReglaConfiguracion.reload();

                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }

}

// #endregion


// #region CLIENTES

function RecargarClientes() {
    recargarCombos([App.cmbClientes]);
    App.hdCliID.setValue(0);
    CargarStores();
}

function SeleccionarCliente() {
    App.cmbClientes.getTrigger(0).show();
    App.hdCliID.setValue(App.cmbClientes.value);
    CargarStores();
}

// #endregion

function CargarStores() {
    App.storeGlobalCondicionReglaConfiguracion.reload();
    App.storeProyectosTipos.reload();
    App.storeGlobalCondicionesColumnasFormularios.reload();
    App.storeGlobalCondicionesFormularios.reload();
}

function DeseleccionarGrilla() {

    App.GridRowSelectGrid.clearSelections();

}

function AfterRender() {
    App.Tree.setRootNode(null);
}

function RecargarProyectoTipo() {
    recargarCombos([App.cmbModulo]);
}

function SeleccionarProyectoTipo() {
    App.cmbModulo.getTrigger(0).show();
}

function SeleccionarFormulario() {
    App.cmbFormulario.getTrigger(0).show();
    App.storeGlobalCondicionesColumnasFormularios.reload();
    App.cmbColumnaFormulario.clearValue();
    App.btnGuardarDetalle.disable();
}

function SeleccionarColumnaFormulario() {
    App.cmbColumnaFormulario.getTrigger(0).show();

    if (App.cmbFormulario.getValue() != null) {
        App.btnGuardarDetalle.enable();
    }
}

function SeleccionarTabla() {
    App.cmbTabla.getTrigger(0).show();
    App.storeGlobalCondicionesColumnasTablas.reload();
    App.cmbColumnaTabla.clearValue();
    App.btnGuardarDetalle.disable();
}

function SeleccionarColumnaTabla() {
    App.cmbColumnaTabla.getTrigger(0).show();

    if (App.cmbTabla.getValue() != null) {
        App.btnGuardarDetalle.enable();
    }
}

function SelectItemMenu(sender, registro, index) {
    //var seleccionado = registro.data;

    //if (seleccionado != null &&
    //    seleccionado.RutaPagina != null && seleccionado.RutaPagina != "") {

    //    addTab(App.tabPpal, seleccionado.text, seleccionado.text, seleccionado.RutaPagina);
    //    this.getSelectionModel().deselectAll();
    //}
    //App.storeGlobalCondicionReglaConfiguracion.reload();
}

function Grid_RowSelectGrid(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;

        App.btnEditarDetalle.enable();
        App.btnEliminarDetalle.enable();
        App.btnDescargarDetalle.enable();

        App.btnEditarDetalle.setTooltip(jsEditar);
        App.btnAnadirDetalle.setTooltip(jsAgregar);
        App.btnEliminarDetalle.setTooltip(jsEliminar);
        App.btnAnadirDetalle.enable();

        if (!App.pnAsideR.collapsed) {
            cargarDatosPanelMoreInfoGrid(registro, App.gridReglasCodigos);
        }
    }
}


function Grid_RowSelect(sender, registro, index) {
    App.GridRowSelectGrid.clearSelections();

    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;

        if (registro.data.Tipo == "Padre") {

            App.hdCampoDestino.setValue(seleccionado.id);
            App.hdRaiz.setValue(registro.data.id);

            App.btnAnadir.enable();
            App.btnEditar.disable();
            App.btnEliminar.disable();
            App.btnDefecto.disable();

            App.btnAnadirDetalle.disable();
            App.btnEditarDetalle.disable();
            App.btnEliminarDetalle.disable();
            App.btnRefrescarDetalle.disable();
            App.btnDescargarDetalle.disable();
            App.btnSimularCodigo.disable();
            App.hd_MenuSeleccionado.setValue(0);

            App.txtSimulacionCodidoSiguinete.setValue("");
            App.txtSimulacionCodido.setValue("");

            App.storeGlobalCondicionReglaConfiguracion.reload();
        }

        else if (registro.data.Tipo == "Hijo") {
            App.btnAnadir.disable();
            App.btnEditar.enable();
            App.btnEliminar.enable();
            App.btnDefecto.enable();

            App.btnAnadirDetalle.enable();
            App.btnRefrescarDetalle.enable();
            App.btnDescargarDetalle.enable();
            App.btnSimularCodigo.enable();
            App.hd_MenuSeleccionado.setValue(seleccionado.id);
            App.hdCampoDestino.setValue(seleccionado.CampoDestino);

            App.txtSimulacionCodidoSiguinete.setValue("");
            App.txtSimulacionCodido.setValue("");

            App.storeGlobalCondicionReglaConfiguracion.reload();

            App.hdLongitudMaxima.setValue(registro.data.Maximo);
        }

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnDefecto.setTooltip(jsDefecto);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnAnadirDetalle.setTooltip(jsAgregar);
        App.btnRefrescarDetalle.setTooltip(jsRefrescar);
        App.btnDescargarDetalle.setTooltip(jsDescargar);

    }


}



function ajaxRefreshArbol() {
    TreeCore.RefreshMenu({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                var nodes = eval(result);

                if (App.TreePanelSideL.getRootNode() != null) {
                    App.TreePanelSideL.getRootNode().removeAll();
                }

                App.TreePanelSideL.setRootNode(nodes[0]);
                App.TreePanelSideL.getRootNode().expand();

                if (App.TreePanelSideL.getRootNode() != null &&
                    App.TreePanelSideL.getRootNode().childNodes != null &&
                    App.TreePanelSideL.getRootNode().childNodes.length > 0) {
                    App.TreePanelSideL.expand();
                }
                else if (!App.TreePanelSideL.getRootNode().data.rootVacio) {
                    App.TreePanelSideL.setRootNode(null);
                }
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

function FormularioValido(valid) {

    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}


function FormularioValidoDetalle(valid) {

    if (valid) {
        App.btnGuardarDetalle.setDisabled(false);
    }
    else {
        App.btnGuardarDetalle.setDisabled(true);
    }
}


//SIMULADOR DE CODIGO
function BotonSimularCodigo() {

    TreeCore.SimularCodigo({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });

}

//DRAG AND DROP
function trackDragDrop() {

    let idFilas = [];

    App.storeGlobalCondicionReglaConfiguracion.data.items.forEach(col => {
        idFilas.push(col.id);
    });
    TreeCore.CambiarOrden(idFilas, {
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });

}

var handleDrop = function (node, data, overModel, dropPosition) {
    var droppedRec = data.records[0],
        items = droppedRec.store.data.items,
        dropPos = -1;

    for (var i = 0; i < items.length; i++) {
        if (items[i].internalId == droppedRec.internalId) {
            dropPos = i;
            break;
        }
    }

    if (dropPos < 0) {
        Ext.Error("Unknown drop position in grid view.");
        return false;
    }


}


//#region FILTROS

function FiltrarColumnas(sender, registro) {

    var idComponente = sender.id.split('_');
    idComponente.pop();
    var tree = App.gridReglasCodigos;
    var store = tree.store,
        logic = store,
        text = sender.getRawValue();

    App.btnDescargarDetalle.disable();

    logic.clearFilter();

    if (Ext.isEmpty(text, false)) {
        App.btnDescargarDetalle.enable();
        return;
    }

    if (registro.getKey() === registro.ESC) {
        clearFilter();
    } else {
        try {
            var re = new RegExp(".*" + text + ".*", "i");
        } catch (err) {
            return;
        }

        logic.filterBy(function (node) {
            var correcto = false;
            tree.columns.forEach(valores => {
                if (!correcto)
                    correcto = re.test(node.data[valores.dataIndex])

            });
            return correcto;
        });
    }
}


function LimpiarFiltroBusqueda(sender, registro) {
    var idComponente = sender.id.split('_');
    idComponente.pop();
    var tree = App.gridReglasCodigos;
    var store = tree.store,
        logic = store,
        field = App.txtSearch;

    field.setValue("");
    logic.clearFilter();
    App.btnDescargarDetalle.enable();
}
// #endregion

function RecargarFormularios() {
    recargarCombos([App.cmbFormulario]);
    RecargarColumnaFormulario();
}

function RecargarColumnaFormulario() {
    recargarCombos([App.cmbColumnaFormulario]);
    App.btnGuardarDetalle.disable();
}

function RecargarTablas() {
    recargarCombos([App.cmbTabla]);
    RecargarColumnaTabla();
}

function RecargarColumnaTabla() {
    recargarCombos([App.cmbColumnaTabla]);
    App.btnGuardarDetalle.disable();
}

// #region DEFECTO

function Defecto() {
    if (App.hdRaiz && seleccionado != null) {
        if (!seleccionado.Activo) {
            Ext.Msg.alert(
                {
                    title: jsDefecto + " " + jsTituloModulo,
                    msg: jsRegistroInactivoPorDefecto,
                    buttons: Ext.Msg.YESNO,
                    fn: ajaxDefecto,
                    icon: Ext.MessageBox.QUESTION,
                    cls: 'winAlert'
                });
        }
        else {
            ajaxDefecto('yes');
        }
    }
}

function ajaxDefecto(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.AsignarPorDefecto(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, cls: 'winAlert', msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        ajaxRefreshArbol();
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

// #endregion

function FormularioValidoGeneradorCodigo(sender, valid) {
    if (valid == true) {
        App.btnGuardar.setDisabled(false);

        Ext.each(App.formGestion.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && !c.isHidden() && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnGuardarDetalle.setDisabled(true);
            }
        });
    }
    else {
        App.btnGuardarDetalle.setDisabled(true);
    }
}

function hidePanelMoreInfoCodigos(panel, registro) {
    let registroSeleccionado = panel.$widgetRecord;
    let ColumnaSeleccionado = panel.$widgetColumn;

    var asideR = Ext.getCmp('pnAsideR');
    let btn = document.getElementById('btnCollapseAsRClosed');

    if (asideR.collapsed == false) {
        App.tbCollapseAsR.hide();
        App.btnCollapseAsRClosed.hide();
        btn.style.transform = 'rotate(-180deg)';
        App.pnAsideR.collapse();
        stPn = 0;
    }
    else {
        App.tbCollapseAsR.show();
        App.btnCollapseAsRClosed.show();
        btn.style.transform = 'rotate(0deg)';
        App.pnAsideR.expand();
        App.pnMoreInfo.show();
        cargarDatosPanelMoreInfo(registroSeleccionado, ColumnaSeleccionado);
        GridColHandler();
        stPn = 1;
    }

    if (App.WrapGestionColumnas != undefined) {
        App.WrapGestionColumnas.hide();
    }

    if (App.WrapFilterControls != undefined) {
        App.WrapFilterControls.hide();
    }



    window.dispatchEvent(new Event('resizePlantilla'));
}
function Activar() {
    TreeCore.Activar(
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