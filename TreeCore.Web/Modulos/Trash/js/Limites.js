var seleccionadoDetalle;
var seleccionado;



// INICIO MAESTRO

// INICIO GESTION GRID

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

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnAnadir.enable();
        App.btnEliminar.setTooltip(jsEliminar);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }

        App.ModuloID.setValue(seleccionado.GlobalLimiteID);
    }

    TreeCore.mostrarDetalle(seleccionado.GlobalLimiteID);
}

function DeseleccionarGrilla() {

    App.storeDetalle.reload();
    App.GridRowSelect.clearSelections();
    App.ModuloID.setValue("");

    App.btnEditar.disable();
    App.btnEliminar.disable();

    App.btnAnadirDetalle.disable();
    App.btnRefrescarDetalle.disable();
    App.btnDescargarDetalle.disable();

}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

// FIN GESTION GRID

function RecargarPrincipal() {
    App.storePrincipal.reload();
}

function VaciarFormulario() {
    App.formGestion.getForm().reset();
}

function FormularioValido(valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function AgregarEditar() {
    showLoadMask(App.vwContenedor, function (load) {
        recargarCombos([App.cmbTipoCampoAsociado, App.cmbProyectosTipos], function (Fin) {
            if (Fin) {
                VaciarFormulario();
                App.txtLimite.focus(false, 500);
                App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
                App.winGestion.show();
                load.hide();
            }
        });
    });
}

function winGestionBotonGuardar() {
    if (App.formGestion.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditar() {

    var Agregar = false;

    if (App.winGestion.title.startsWith(jsAgregar)) {
        Agregar = true;
    }

    TreeCore.AgregarEditar(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestion.hide();
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
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();

    App.winGestion.setTitle(jsEditar + ' ' + jsTituloModulo);

    showLoadMask(App.vwContenedor, function (load) {
        recargarCombos([App.cmbTipoCampoAsociado], function (Fin) {
            if (Fin) {
                if (seleccionado.Vista != null) {
                    SeleccionarTipoCampoAsociado();
                }
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }

                            App.txtLimite.focus(false, 500);
                            App.storePrincipal.reload();
                            load.hide();
                        }
                    });
            }
        });
    });
}

function Activar() {
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
        if (seleccionado.Defecto) {
            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsDesactivarPorDefecto, buttons: Ext.Msg.OK });
        } else {
            ajaxActivar();
        }
    }
}

function ajaxActivar() {
    TreeCore.Activar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
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

function Eliminar() {
    if (registroSeleccionado(App.gridMaestro) && seleccionado.Defecto != true) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsTituloModulo,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION
            });
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsPorDefecto, buttons: Ext.Msg.OK });
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

function Defecto() {
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
        if (!seleccionado.Activo) {
            Ext.Msg.alert(
                {
                    title: jsDefecto + ' ' + jsTituloModulo,
                    msg: jsRegistroInactivoPorDefecto,
                    buttons: Ext.Msg.YESNO,
                    fn: ajaxDefecto,
                    icon: Ext.MessageBox.QUESTION
                });
        } else {
            Ext.Msg.alert(
                {
                    title: jsDefecto + ' ' + jsTituloModulo,
                    msg: jsPonerPorDefecto,
                    buttons: Ext.Msg.YESNO,
                    fn: ajaxDefecto,
                    icon: Ext.MessageBox.QUESTION
                });
        }
    }
}

function ajaxDefecto(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.AsignarPorDefecto({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storePrincipal.reload();
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
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

// FIN MAESTRO

// INICIO DETALLE

// INICIO GESTION GRID

function Grid_RowSelect_Detalle(sender, registro, index) {

    var datos = registro.data;

    if (datos != null) {

        seleccionadoDetalle = datos;
        App.btnEliminarDetalle.enable();
        App.btnEditarDetalle.enable();
        App.btnActivarDetalle.enable();

        App.btnEditarDetalle.setTooltip(jsEditar);
        App.btnEliminarDetalle.setTooltip(jsEliminar);
        App.btnAnadirDetalle.setTooltip(jsAgregar);
        App.btnRefrescarDetalle.setTooltip(jsRefrescar);
        App.btnDescargarDetalle.setTooltip(jsDescargar);

        if (seleccionadoDetalle.Activo) {
            App.btnActivarDetalle.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivarDetalle.setTooltip(jsActivar);
        }
    }
}

function DeseleccionarGrillaDetalle() {
    App.GridRowSelectDetalle.clearSelections();
    App.btnEditarDetalle.disable();
    App.btnEliminarDetalle.disable();
    App.btnActivarDetalle.disable();

    App.btnAnadirDetalle.setTooltip(jsAgregar);
    App.btnEditarDetalle.setTooltip(jsEditar);
    App.btnEliminarDetalle.setTooltip(jsEliminar);
    App.btnActivarDetalle.setTooltip(jsActivar);
    App.btnRefrescarDetalle.setTooltip(jsRefrescar);
    App.btnDescargarDetalle.setTooltip(jsDescargar);

}

var handlePageSizeSelectDetalle = function (item, records) {
    var curPageSize = App.storeDetalle.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeDetalle.pageSize = wantedPageSize;
        App.storeDetalle.load();
    }
}

// FIN GESTION GRID

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

function AgregarEditarDetalle() {
    showLoadMask(App.vwContenedor, function (load) {
        recargarCombos([App.cmbTipoCampoAsociadoCondiciones, App.cmbCamposAsociados, App.cmbTiposDatos, App.cmbOperaciones], function (Fin) {
            if (Fin) {
                VaciarFormularioDetalle();
                App.txtCriterio.focus(false, 500);
                App.winGestionDetalle.setTitle(jsAgregar + ' ' + jsCriterio);
                App.winGestionDetalle.show();
                load.hide();
            }
        });
    });
}

function winGestionBotonGuardarDetalle() {


    if (App.formGestionDetalle.getForm().isValid()) {
        ajaxAgregarEditarDetalle();
    }
    else {

        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }

}

function ajaxAgregarEditarDetalle() {
    var Agregar = false;

    if (App.winGestionDetalle.title.startsWith(jsAgregar)) {
        Agregar = true;
    }
    TreeCore.AgregarEditarDetalle(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestionDetalle.hide();
                    App.storeDetalle.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function MostrarEditarDetalle() {
    if (registroSeleccionado(App.GridDetalle) && seleccionadoDetalle != null) {
        ajaxEditarDetalle();
    }
}

function ajaxEditarDetalle() {
    VaciarFormularioDetalle();
    App.winGestionDetalle.setTitle(jsEditar + ' ' + jsCriterio);

    showLoadMask(App.vwContenedor, function (load) {
        recargarCombos([App.cmbTipoCampoAsociadoCondiciones, App.cmbCamposAsociados, App.cmbTiposDatos, App.cmbOperaciones], function (Fin) {
            if (Fin) {
                if (seleccionadoDetalle.Campo != null) {
                    SeleccionarTipoCampoAsociadoCondiciones();
                    SeleccionarCamposAsociados();
                }
                if (seleccionadoDetalle.TipoDatoID != null) {
                    SeleccionarTiposDatos();
                }
                if (seleccionadoDetalle.Operador != null) {
                    SeleccionarOperaciones();
                }
                TreeCore.MostrarEditarDetalle(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            App.storeDetalle.reload();
                            load.hide();
                        }
                    });
            }
        });
    });
}

function EliminarDetalle() {
    if (registroSeleccionado(App.GridDetalle) && seleccionadoDetalle.Defecto != true) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsCriterio,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarDetalle,
                icon: Ext.MessageBox.QUESTION
            });
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsPorDefecto, buttons: Ext.Msg.OK });
    }
}

function ajaxEliminarDetalle(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarDetalle({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeDetalle.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function ActivarDetalle() {
    if (registroSeleccionado(App.GridDetalle) && seleccionadoDetalle != null) {
        if (seleccionadoDetalle.Defecto) {
            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsDesactivarPorDefecto, buttons: Ext.Msg.OK });
        } else {
            ajaxActivarDetalle();
        }
    }
}

function ajaxActivarDetalle() {

    TreeCore.ActivarDetalle(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeDetalle.reload();
                }

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function DefectoDetalle() {
    if (registroSeleccionado(App.GridDetalle) && seleccionado != null) {
        if (!seleccionadoDetalle.Activo) {
            Ext.Msg.alert(
                {
                    title: jsDefecto + ' ' + jsCriterio,
                    msg: jsRegistroInactivoPorDefecto,
                    buttons: Ext.Msg.YESNO,
                    fn: ajaxDefectoDetalle,
                    icon: Ext.MessageBox.QUESTION
                });
        } else {
            Ext.Msg.alert(
                {
                    title: jsDefecto + ' ' + jsTituloModulo,
                    msg: jsPonerPorDefecto,
                    buttons: Ext.Msg.YESNO,
                    fn: ajaxDefectoDetalle,
                    icon: Ext.MessageBox.QUESTION
                });
        }
    }
}

function ajaxDefectoDetalle(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.AsignarPorDefectoDetalle({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeDetalle.reload();
                }
            },
            eventMask: {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    }
}

function RefrescarDetalle() {
    App.storeDetalle.reload();
    DeseleccionarGrillaDetalle();
}

// FIN DETALLE

//INICIO TRIGGERS


function RecargarTipoCampoAsociado() {
    recargarCombos([App.cmbTipoCampoAsociado]);
}

function SeleccionarTipoCampoAsociado() {
    App.cmbTipoCampoAsociado.getTrigger(0).show();
}

function RecargarProyectosTipos() {
    recargarCombos([App.cmbProyectosTipos]);
}

function SeleccionarProyectosTipos() {
    App.cmbProyectosTipos.getTrigger(0).show();
}

function RecargarTipoCampoAsociadoCondiciones() {
    recargarCombos([App.cmbTipoCampoAsociadoCondiciones]);
}

function SeleccionarTipoCampoAsociadoCondiciones() {
    App.cmbTipoCampoAsociadoCondiciones.getTrigger(0).show();
}

function RecargarCamposAsociados() {
    recargarCombos([App.cmbCamposAsociados]);
}

function SeleccionarCamposAsociados() {
    App.cmbCamposAsociados.getTrigger(0).show();
}

function RecargarTiposDatos() {
    recargarCombos([App.cmbTiposDatos]);
}

function SeleccionarTiposDatos() {
    App.cmbTiposDatos.getTrigger(0).show();
}

function RecargarOperaciones() {
    recargarCombos([App.cmbOperaciones]);
}

function SeleccionarOperaciones() {
    App.cmbOperaciones.getTrigger(0).show();
}

//FIN TRIGGERS

// INICIO CLIENTES

function RecargarClientes() {
    recargarCombos([App.cmbClientes]);
    App.hdCliID.setValue(0);
    CargarStores();
}

function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

function SeleccionarCliente() {
    App.cmbClientes.getTrigger(0).show();
    App.hdCliID.setValue(App.cmbClientes.value);
    CargarStores();
}

// FIN CLIENTES

var template = '<span style="color:{0};">{1}</span>';

var change = function (value) {
    return Ext.String.format(template, (value > 0) ? "green" : "red", value);
};

var pctChange = function (value) {
    return Ext.String.format(template, (value > 0) ? "green" : "red", value + "%");
};