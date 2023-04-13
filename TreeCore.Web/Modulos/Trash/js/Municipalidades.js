var seleccionadoDetalle;
var seleccionado;




/* INICIO GRID */

/* INICIO MAESTRO */

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;
    App.hdMaestroID.setValue(0);

    if (datos != null) {
        seleccionado = datos;
        App.storeDetalle.reload();

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
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnRadio.setTooltip(jsRadio);
        App.btnAgregarInformacionComarch.setTooltip(jsInformacionComarch);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }

        App.ModuloID.setValue(seleccionado.GlobalMunicipalidadID);

    }
    TreeCore.mostrarDetalle(seleccionado.GlobalMunicipalidadID);


}

function DeseleccionarGrilla() {
    App.hdMaestroID.setValue(1);
    App.storeDetalle.reload();
    App.GridRowSelect.clearSelections();
    //App.ModuloID.setValue("");

    App.btnDescargar.disable();
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



        App.btnRadioDetalle.setTooltip(jsRadio);
        App.btnEditarDetalle.setTooltip(jsEditar);
        App.btnEliminarDetalle.setTooltip(jsEliminar);
        App.btnDefectoDetalle.setTooltip(jsDefecto);
        App.btnAnadirDetalle.setTooltip(jsAgregar);
        App.btnRefrescarDetalle.setTooltip(jsRefrescar);
        App.btnDescargarDetalle.setTooltip(jsDescargar);

        if (seleccionadoDetalle.Activo) {
            App.btnActivarDetalle.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivarDetalle.setTooltip(jsActivar);
        }

        App.hdGlopalPartidoID.setValue(seleccionadoDetalle.GlobalPartidoID);
    }
}

function DeseleccionarGrillaDetalle() {
    App.GridRowSelectDetalle.clearSelections();
    App.btnEditarDetalle.disable();
    App.btnEliminarDetalle.disable();
    App.btnActivarDetalle.disable();
    App.btnRadioDetalle.disable();
    App.btnDefectoDetalle.disable();

    App.btnAnadirDetalle.setTooltip(jsAgregar);
    App.btnEditarDetalle.setTooltip(jsEditar);
    App.btnEliminarDetalle.setTooltip(jsEliminar);

    App.btnActivarDetalle.setTooltip(jsActivar);

    App.btnRefrescarDetalle.setTooltip(jsRefrescar);
    App.btnDescargarDetalle.setTooltip(jsDescargar);


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
    App.storePaises.reload();
}

function SeleccionarMunicipio(sender, registro, index) {
    this.getTrigger(0).show();
    App.storePrincipal.reload();
}

function RecargarMunicipio(sender, registro, index) {
    let idComponente = getIdComponente(sender);
    let combos = [Ext.getCmp(idComponente + '_cmbMunicipio')];
    recargarCombos(combos);
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
    App.winGestion.setTitle(jsAgregar + " " + jsMunicipio);
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

    App.winGestion.setTitle(jsEditar + " " + jsMunicipio);
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
                title: jsEliminar,
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
        if (!seleccionado.Activo) {
            Ext.Msg.alert(
                {
                    title: jsDefecto,
                    msg: jsRegistroInactivoPorDefecto,
                    buttons: Ext.Msg.YESNO,
                    fn: ajaxDefecto,
                    icon: Ext.MessageBox.QUESTION
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
    App.winGestionDetalle.setTitle(jsAgregar + " " + jsPartido);
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
    App.winGestionDetalle.setTitle(jsEditar + " " + jsPartido);

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
        if (!seleccionadoDetalle.Activo) {
            Ext.Msg.alert(
                {
                    title: "",
                    msg: "",
                    buttons: Ext.Msg.YESNO,
                    fn: ajaxDefectoDetalle,
                    icon: Ext.MessageBox.QUESTION
                });
        }
        else {
            ajaxDefectoDetalle('yes');
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
}

function SeleccionarPais() {
    App.cmbPais.getTrigger(0).show();
    RecargarRegiones();
}

function RecargarRegiones() {
    recargarCombos([App.cmbRegiones, App.cmbProvincia]);
}

function SeleccionarRegiones() {
    App.cmbRegiones.getTrigger(0).show();
    RecargarProvincias();
}

function SeleccionarProvincias() {
    App.cmbProvincia.getTrigger(0).show();
    RecargarMunicipios();
}

function RecargarProvincias() {
    recargarCombos([App.cmbProvincia, App.cmbMunicipio]);
}

function RecargarMunicipios() {
    recargarCombos([App.cmbMunicipio]);
    App.storePrincipal.reload();
    App.btnAnadir.disable();
}

function SeleccionarMunicipios() {
    App.cmbMunicipio.getTrigger(0).show();
    CargarStores();
    App.btnAnadir.enable();
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
                title: jsAsignar + " " + jsRadio,
                msg: jsAsignarRadioMensaje,
                buttons: Ext.Msg.YESNO,
                fn: MostrarRadio,
                icon: Ext.MessageBox.QUESTION
            });
    } else if (App.storePrincipal.data.items.length > 1) {
        Ext.Msg.show(
            {

                title: jsAsignar + " " + jsRadio,
                msg: jsAsignarRadioMensaje,
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
                title: jsAsignar + " " + jsRadio,
                msg: jsAsignarRadioMensaje,
                buttons: Ext.Msg.YESNO,
                fn: MostrarRadioDetalle,
                icon: Ext.MessageBox.QUESTION
            });
    } else if (App.storeDetalle.data.items.length > 1) {
        Ext.Msg.show(
            {

                title: jsAsignar + " " + jsRadio,
                msg: jsAsignarRadioMensaje,
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
    App.winGestionComarch.setTitle(jsAgregar + " " + jsInformacionComarch);

}

function VaciarFormularioGestion() {
    App.FormPanelComarch.getForm().reset();
    nRound = 0;
}

function ajaxAgregarInformacionComarch() {


    App.txtCategoriaDivipola.focus(false, 200);
    App.winGestionComarch.setTitle(jsAgregar + " " + jsInformacionComarch);

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

    App.txtCategoriaDivipola.focus(false, 500);
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