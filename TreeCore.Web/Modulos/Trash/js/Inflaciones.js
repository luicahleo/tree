var seleccionadoDetalle;
var seleccionado;

// INICIO MAESTRO

function RecargarPrincipal() {
    App.storePrincipal.reload();
}

function VaciarFormulario() {
    App.formGestion.getForm().reset();
}

function winGestionBotonGuardar() {
    if (App.formGestion.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: App.jsTieneRegistros, buttons: Ext.Msg.OK });
    }
}

function DeseleccionarGrilla() {

    DeseleccionarGrillaDetalle();
    App.storeDetalle.reload();
    App.GridRowSelect.clearSelections();
    App.ModuloID.setValue("");
    App.btnEditar.disable();
    App.btnAnadirDetalle.disable();
    App.btnRefrescarDetalle.disable();
    App.btnDescargarDetalle.disable();
    App.btnEliminar.disable();
}

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;
    if (datos != null) {
        seleccionado = datos;
        App.storeDetalle.reload();
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnAnadirDetalle.enable();
        App.btnRefrescarDetalle.enable();
        App.btnDescargarDetalle.enable();

        App.ModuloID.setValue(seleccionado.InflacionID);
    }

    TreeCore.mostrarDetalle(seleccionado.InflacionID);
}

function ajaxEditar() {
    VaciarFormulario();

    App.winGestion.setTitle(App.jsEditar + ' ' + App.jsTituloModulo);
    TreeCore.MostrarEditar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                App.txtInflacion.focus(false, 500);
                App.storePrincipal.reload();

            },
            eventMask:
            {
                showMask: true,
                msg: App.jsMensajeProcesando
            }
        });
}

function ajaxAgregarEditar() {

    var Agregar = false;

    if (App.winGestion.title.startsWith(App.jsAgregar)) {
        Agregar = true;
    }

    TreeCore.AgregarEditar(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestion.hide();
                    App.storePrincipal.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: App.jsMensajeProcesando
            }
        });
}

function ajaxEliminar(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.Eliminar({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storePrincipal.reload();
                }
            },
            eventMask: { showMask: true, msg: App.jsMensajeProcesando }
        });
    }
}

function FormularioValido(valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function MostrarEditar() {
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
        ajaxEditar();
    }
}

function Eliminar() {
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: App.jsEliminar + ' ' + App.jsTituloModulo,
                msg: App.jsMensajeEliminar + ': ' + seleccionado.Inflacion + ' ?',
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function AgregarEditar() {
    VaciarFormulario();
    App.txtInflacion.focus(false, 500);
    App.winGestion.setTitle(App.jsAgregar + ' ' + App.jsTituloModulo);
    App.winGestion.show();
}

function refrescar() {
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

// FIN MAESTRO

// INICIO DETALLE

function AgregarEditarDetalle() {
    VaciarFormularioDetalle();
    App.txtValor.enable();
    App.txtValor.focus(false, 500);
    App.winGestionDetalle.setTitle(App.jsAgregar);
    App.winGestionDetalle.show();
}

function ajaxActivarDetalle() {

    TreeCore.ActivarDetalle(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeDetalle.reload();
                }

            },
            eventMask:
            {
                showMask: true,
                msg: App.jsMensajeProcesando
            }
        });
}

function ActivarDetalle() {
    if (registroSeleccionado(App.GridDetalle) && seleccionado != null) {
        ajaxActivarDetalle();
    }
}

function VaciarFormularioDetalle() {
    App.formGestionDetalle.getForm().reset();
}

function FormularioValidoDetalle(valid) {
    if (valid) {
        App.btnGuardarDetalle.setDisabled(false);
    }
    else {
        App.btnGuardarDetalle.setDisabled(true);
    }
}

function ajaxEditarDetalle() {
    VaciarFormularioDetalle();
    App.winGestionDetalle.setTitle(App.jsEditar);

    TreeCore.MostrarEditarDetalle(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeDetalle.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: App.jsMensajeProcesando
            }
        });
}

function MostrarEditarDetalle() {
    if (registroSeleccionado(App.GridDetalle) && seleccionado != null) {
        ajaxEditarDetalle();
    }
}

function winGestionBotonGuardarDetalle() {
    if (App.txtMes.value == 0 || App.txtMes.value == "") {
        App.txtMes.value = null;
    }
    if (App.txtMes.value == null || App.txtMes.value == 1 || App.txtMes.value == 2 || App.txtMes.value == 3 || App.txtMes.value == 4 || App.txtMes.value == 5 || App.txtMes.value == 6 || App.txtMes.value == 7 || App.txtMes.value == 8 || App.txtMes.value == 9 || App.txtMes.value == 10 || App.txtMes.value == 11 || App.txtMes.value == 12) {
        if (App.formGestionDetalle.getForm().isValid()) {
            ajaxAgregarEditarDetalle();
        }
        else {

            Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: App.jsFormularioNoValido, buttons: Ext.Msg.OK });
        }
    }
    else {

        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: App.jsFormularioNoValido, buttons: Ext.Msg.OK });

    }
}

function ajaxAgregarEditarDetalle() {
    var Agregar = false;

    if (App.winGestionDetalle.title.startsWith(App.jsAgregar)) {
        Agregar = true;
    }
    TreeCore.AgregarEditarDetalle(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestionDetalle.hide();
                    App.storeDetalle.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: App.jsMensajeProcesando
            }
        });
}

function EliminarDetalle() {
    if (registroSeleccionado(App.GridDetalle) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: App.jsEliminar,
                msg: App.jsMensajeEliminar + '?',
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarDetalle,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarDetalle(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarDetalle({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeDetalle.reload();
                }
            },
            eventMask: { showMask: true, msg: App.jsMensajeProcesando }
        });
    }
}

function Grid_RowSelect_Detalle(sender, registro, index) {

    var datos = registro.data;

    if (datos != null) {

        seleccionado = datos;
        //if (seleccionado.Activo) {
        //    App.btnActivarDetalle.setText('Desactivar');
        //}
        //else {
        //    App.btnActivarDetalle.setText('Activar');
        //}
        seleccionadoDetalle = datos;
        App.btnAnadirDetalle.enable();
        App.btnEliminarDetalle.enable();
        App.btnEditarDetalle.enable();
        App.btnActivarDetalle.enable();
    }
}

function DeseleccionarGrillaDetalle() {
    App.GridRowSelectDetalle.clearSelections();
    App.btnEditarDetalle.disable();
    App.btnEliminarDetalle.disable();
    App.btnActivarDetalle.disable();
}

function refrescarDetalle() {
    App.storeDetalle.reload();
    DeseleccionarGrillaDetalle();
}

var handlePageSizeSelectDetalle = function (item, records) {
    var curPageSize = App.storeDetalle.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeDetalle.pageSize = wantedPageSize;
        App.storeDetalle.load();
    }
}

// FIN DETALLE

// INICIO PAISES

function RecargarPaises() {
    DeletePaises();
    App.storePaises.reload();
}

function DeletePaises() {
    App.cmbPais.clearValue();
}

function CargarStores() {
    RecargarPaises();
    RecargarPrincipal();
}

var TriggerPaises = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbPais.clearValue();
            break;
        case 1:
            RecargarPaises();
            break;
    }
}

// FIN PAISES

// INICIO CLIENTES

function RecargarClientes() {

    App.storeClientes.reload();
    CargarStores();
}

function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

function SeleccionarCliente() {
    CargarStores();
    App.hdCliID.setValue(App.cmbClientes.value);
}

// FIN CLIENTES

var template = '<span style="color:{0};">{1}</span>';

var change = function (value) {
    return Ext.String.format(template, (value > 0) ? "green" : "red", value);
};

var pctChange = function (value) {
    return Ext.String.format(template, (value > 0) ? "green" : "red", value + "%");
};