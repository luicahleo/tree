var seleccionadoDetalle;
var seleccionado;




/* INICIO GRID */

/* INICIO MAESTRO */

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.storeDetalle.reload();

        App.btnAnadir.enable();
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnRadio.enable();
        App.btnActivar.enable();
        App.btnDefecto.enable();

        App.btnDescargar.enable();
        App.btnAnadirDetalle.enable();
        App.btnRefrescarDetalle.enable();
        App.btnDescargarDetalle.enable();
        App.btnRadioDetalle.enable();
        App.btnAgregarInformacionComarch.enable();

        App.btnDescargar.setTooltip(jsDescargar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnDefecto.setTooltip(jsDefecto);
        App.btnAnadir.enable();
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnRadio.setTooltip(jsRadio);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }

        App.btnAgregarInformacionComarch.setTooltip(jsInformacionComarch);

        App.ModuloID.setValue(seleccionado.ProvinciaID);

    }
    TreeCore.mostrarDetalle(seleccionado.ProvinciaID);


}

function DeseleccionarGrilla() {

    App.storeDetalle.reload();
    App.GridRowSelect.clearSelections();
    App.ModuloID.setValue("");

    App.btnDescargar.disable();
    App.btnAnadir.disable();
    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnRadio.disable();
    App.btnAgregarInformacionComarch.disable();
    App.btnAnadirDetalle.disable();
    App.btnRefrescarDetalle.disable();
    App.btnDescargarDetalle.disable();
    App.btnRadioDetalle.disable();
    App.btnActivar.disable();
    App.btnDefecto.disable();

    if (App.cmbRegiones.getValue() != null && App.cmbRegiones.getValue() != "") {
        App.btnAnadir.enable();
    }
}

/* FIN MAESTRO */

/* INICIO DETALLE */

function Grid_RowSelect_Detalle(sender, registro, index) {

    var datos = registro.data;

    if (datos != null) {
        seleccionadoDetalle = datos;
        App.btnEliminarDetalle.enable();
        App.btnEditarDetalle.enable();
        App.btnActivarDetalle.enable();
        App.btnRadioDetalle.enable();
        App.btnDefectoDetalle.enable();
        App.btnAgregarInformacionComarchDetalle.enable();



        App.btnRadioDetalle.setTooltip(jsRadio);
        App.btnEditarDetalle.setTooltip(jsEditar);
        App.btnEliminarDetalle.setTooltip(jsEliminar);
        App.btnDefectoDetalle.setTooltip(jsDefecto);
        App.btnAnadirDetalle.setTooltip(jsAgregar);
        App.btnRefrescarDetalle.setTooltip(jsRefrescar);
        App.btnDescargarDetalle.setTooltip(jsDescargar);
        App.btnAgregarInformacionComarchDetalle.setTooltip(jsInformacionComarch);

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
    App.btnRadioDetalle.disable();
    App.btnDefectoDetalle.disable();
    App.btnAgregarInformacionComarchDetalle.disable();

    App.btnAnadirDetalle.setTooltip(jsAgregar);
    App.btnEditarDetalle.setTooltip(jsEditar);
    App.btnEliminarDetalle.setTooltip(jsEliminar);

    App.btnActivarDetalle.setTooltip(jsActivar);

    App.btnRefrescarDetalle.setTooltip(jsRefrescar);
    App.btnDescargarDetalle.setTooltip(jsDescargar);
    App.btnAgregarInformacionComarchDetalle.setTooltip(jsInformacionComarch);


}

/* FIN DETALLE */

/* FIN GRID */

/* INICIO GESTION GRIDS */

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}


var handlePageSizeSelectDetalle = function (item, records) {
    var curPageSize = App.storeDetalle.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeDetalle.pageSize = wantedPageSize;
        App.storeDetalle.load();
    }
}

/* FIN GESTION GRIDS */

/* INICIO GESTION */

function RecargarPrincipal() {
    App.storePrincipal.reload();
}

/* INICIO GESTION MAESTRO */

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
    VaciarFormulario();
    App.txtProvincia.focus(false, 500);
    App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
    App.winGestion.show();
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
    TreeCore.MostrarEditar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                App.txtProvincia.focus(false, 500);
                App.storePrincipal.reload();

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function Eliminar() {
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsTituloModulo,
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
function Activar() {
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
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

function Defecto() {
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
        if (App.cmbRegiones.selection.data.Defecto) {
            if (!seleccionado.Activo) {
                Ext.Msg.alert(
                    {
                        title: jsDefecto + ' ' + jsTituloModulo,
                        msg: jsRegistroInactivoPorDefecto,
                        buttons: Ext.Msg.YESNO,
                        fn: ajaxDefecto,
                        icon: Ext.MessageBox.QUESTION
                    });
            }
            else {
                ajaxDefecto('yes');
            }
        } else {
            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsErrorDefectoMaestro, buttons: Ext.Msg.OK });
        }
    }
}

function ajaxDefecto(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.AsignarPorDefecto(
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
}

function Refrescar() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

/* FIN GESTION MAESTRO */


/* INICIO GESTION DETALLE */

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
    VaciarFormularioDetalle();
    App.txtMunicipio.focus(false, 500);
    App.winGestionDetalle.setTitle(jsAgregar);
    App.winGestionDetalle.show();
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
    App.winGestionDetalle.setTitle(jsEditar);

    TreeCore.MostrarEditarDetalle(
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

function EliminarDetalle() {
    if (registroSeleccionado(App.GridDetalle) && seleccionadoDetalle != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
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
        ajaxActivarDetalle();
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
    if (registroSeleccionado(App.GridDetalle) && seleccionadoDetalle != null) {
        if (seleccionado.Defecto) {
            if (!seleccionadoDetalle.Activo) {
                Ext.Msg.alert(
                    {
                        title: jsDefecto + ' ' + jsTituloModulo,
                        msg: jsRegistroInactivoPorDefecto,
                        buttons: Ext.Msg.YESNO,
                        fn: ajaxDefectoDetalle,
                        icon: Ext.MessageBox.QUESTION
                    });
            }
            else {
                ajaxDefectoDetalle('yes');
            }
        } else {
            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsErrorDefectoDetalle, buttons: Ext.Msg.OK });
        }
    }
}

function ajaxDefectoDetalle(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.AsignarPorDefectoDetalle(
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
}

function RefrescarDetalle() {
    App.storeDetalle.reload();
    DeseleccionarGrillaDetalle();
}
/* FIN GESTION DETALL E*/

/* INICIO CLIENTE */

function RecargarClientes() {
    recargarCombos([App.cmbClientes]);
    App.hdCliID.setValue(0);
    RecargarPais();
}

function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

function SeleccionarCliente() {
    App.cmbClientes.getTrigger(0).show();
    App.hdCliID.setValue(App.cmbClientes.value);
    RecargarPais();
}

/* FIN CLIENTE */

/* INICIO COMBOS TOOLBAR */

function RecargarPais() {
    recargarCombos([App.cmbPais, App.cmbRegiones]);
    App.storePaises.reload();
    CargarStores();
}

function SeleccionarPais() {
    App.cmbPais.getTrigger(0).show();
    RecargarRegiones();
}

function RecargarRegiones() {
    recargarCombos([App.cmbRegiones]);
    CargarStores();
}

function SeleccionarRegiones() {
    App.cmbRegiones.getTrigger(0).show();
    CargarStores();
}

/* FIN COMBOS TOOLBAR */

var template = '<span style="color:{0};">{1}</span>';

var change = function (value) {
    return Ext.String.format(template, (value > 0) ? "green" : "red", value);
};

var pctChange = function (value) {
    return Ext.String.format(template, (value > 0) ? "green" : "red", value + "%");
};

//////////////////// REGIÓN RADIO ////////////////////

function ActivaRadio() {
    if (App.storePrincipal.data.items.length > 0) {
        App.btnRadio.enable();
    } else {
        App.btnRadio.disable();
    }
}

function BotonRadio() {

    if (App.storePrincipal.data.items.length == 0) {
        Ext.Msg.show(
            {
                title: jsAsignarRadio,
                msg: jsAsignarRadioTodos,
                buttons: Ext.Msg.YESNO,
                fn: MostrarRadio,
                icon: Ext.MessageBox.QUESTION
            });
    } else if (App.storePrincipal.data.items.length > 1) {
        Ext.Msg.show(
            {

                title: jsAsignarRadio,
                msg: jsAsignarRadioSeleccionados,
                buttons: Ext.Msg.YESNO,
                fn: MostrarRadio,
                icon: Ext.MessageBox.QUESTION
            })
    } else {
        MostrarEditarRadio()
    }
}
function MostrarEditarRadio() {
    App.formRadio.getForm().reset();
    TreeCore.MostrarEditarRadio({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}
function MostrarRadio(button) {
    if (button == 'yes' || button == 'si') {
        App.formRadio.getForm().reset();
        App.winRadio.show();
    }

}
function winRadioBotonGuardar() {

    TreeCore.AsignarRadio(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.storeRegiones.reload();
                App.winRadio.hide();
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    App.storePrincipal.reload();
}
function FormularioValidoRadio(valid) {
    if (valid) {
        App.btnGuardarRadio.setDisabled(false);
    }
    else {
        App.btnGuardarRadio.setDisabled(true);
    }
}


function BotonRadioDetalle() {

    if (App.storeDetalle.data.items.length == 0) {
        Ext.Msg.show(
            {
                title: jsAsignarRadio,
                msg: jsAsignarRadioTodos,
                buttons: Ext.Msg.YESNO,
                fn: MostrarRadioDetalle,
                icon: Ext.MessageBox.QUESTION
            });
    } else if (App.storeDetalle.data.items.length > 1) {
        Ext.Msg.show(
            {

                title: jsAsignarRadio,
                msg: jsAsignarRadioSeleccionados,
                buttons: Ext.Msg.YESNO,
                fn: MostrarRadioDetalle,
                icon: Ext.MessageBox.QUESTION
            })
    } else {
        MostrarEditarRadioDetalle();
    }
}

function ActivaRadioDetalle() {
    if (App.storeDetalle.data.items.length > 0) {
        App.btnRadioDetalle.enable();
    } else {
        App.btnRadioDetalle.disable();
    }
}
function MostrarRadioDetalle(button) {
    if (button == 'yes' || button == 'si') {
        App.formRadioDetalle.getForm().reset();
        App.winRadioDetalle.show();
    }

}
function MostrarEditarRadioDetalle() {
    App.formRadioDetalle.getForm().reset();
    TreeCore.MostrarEditarRadioDetalle({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
    App.storeDetalle.reload();
}

function FormularioValidoRadioDetalle(valid) {
    if (valid) {
        App.btnGuardarRadioDetalle.setDisabled(false);
    }
    else {
        App.btnGuardarRadioDetalle.setDisabled(true);
    }
}
function winRadioBotonGuardarDetalle() {

    TreeCore.AsignarRadioDetalle(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                App.winRadioDetalle.hide();
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    App.storeDetalle.reload();
}

////ZONA GESTION COMARCH


//EDITAR


function BotonAgregarInformacionComarch() {

    if (registroSeleccionado != null && seleccionado != null) {
        ajaxAgregarInformacionComarch();
    }
    App.winGestionComarch.setTitle(jsAgregar + ' ' + jsInformacionComarch);

}

function VaciarFormularioGestion() {
    App.FormPanelComarch.getForm().reset();
    nRound = 0;
}

function ajaxAgregarInformacionComarch() {


    App.txtRegionComercial.focus(false, 200);
    App.winGestionComarch.setTitle(jsAgregar + ' ' + jsInformacionComarch);

    TreeCore.MostrarEditarAgregarComarch(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                //txtModulo.focus(false, 500);
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}


function FormularioValidoGestionComarch(valid) {
    if (valid) {
        App.btnGuardarComarch.setDisabled(false);
    }
    else {
        App.btnGuardarComarch.setDisabled(true);
    }
}

function winGestionBotonGuardarComarch() {
    if (App.FormPanelComarch.getForm().isValid()) {
        ajaxAgregarEditarComarch();
    }
    else {
        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

//ACCION AGREGAR O EDITAR
function ajaxAgregarEditarComarch() {

    App.txtRegionComercial.focus(false, 500);
    TreeCore.AgregarEditarComarch(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestionComarch.hide();

                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    App.storeDetalle.reload();

}

////DETALLE COMARCH


function BotonAgregarInformacionComarchDetalle() {

    if (registroSeleccionado != null && seleccionadoDetalle != null) {


        ajaxAgregarInformacionComarchDetalle();

    }


    App.winGestionComarchDetalle.setTitle(jsAgregar + ' ' + jsInformacionComarch);

}

function VaciarFormularioGestionDetalle() {
    App.FormPanelComarchDetalle.getForm().reset();
    nRound = 0;
}

function ajaxAgregarInformacionComarchDetalle() {


    App.txtZonaCrc.focus(false, 200);
    App.winGestionComarchDetalle.setTitle(jsAgregar + ' ' + jsInformacionComarch);

    TreeCore.MostrarEditarAgregarComarchDetalle(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                //txtModulo.focus(false, 500);
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    App.storeDetalle.reload();

}


function FormularioValidoGestionComarchDetalle(valid) {
    if (valid) {
        App.btnGuardarComarchDetalle.setDisabled(false);
    }
    else {
        App.btnGuardarComarchDetalle.setDisabled(true);
    }
}

function winGestionBotonGuardarComarchDetalle() {
    if (App.FormPanelComarchDetalle.getForm().isValid()) {
        ajaxAgregarEditarComarchDetalle();
    }
    else {
        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

//ACCION AGREGAR O EDITAR
function ajaxAgregarEditarComarchDetalle() {
    App.txtZonaCrc.focus(false, 500);
    ajaxAgregarInformacionComarchDetalle();
    TreeCore.AgregarEditarComarchDetalle(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestionComarchDetalle.hide();

                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });

}