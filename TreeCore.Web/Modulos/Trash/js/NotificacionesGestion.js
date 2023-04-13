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
        App.btnEnviar.enable();
        App.btnActivar.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnAnadir.enable();
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnEnviar.setTooltip(jsEnviar);

        App.hdModuloID.setValue(seleccionado.NotificacionID);
    }

    TreeCore.mostrarDetalle(seleccionado.NotificacionID);
}

function DeseleccionarGrilla() {

    App.storeDetalle.reload();
    App.GridRowSelect.clearSelections();
    App.hdModuloID.setValue("");

    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnEnviar.disable();
    App.btnActivar.disable();
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
    VaciarFormulario();

    var storesACargar = [App.storeNotificacionesGruposCriterios, App.storeNotificacionesCadencias];

    showLoadMask(App.gridMaestro, function (load) {
        CargarStoresSerie(storesACargar, function (Fin) {
            if (Fin) {
                App.txtFechaActivacion.focus(false, 500);
                App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
                App.winGestion.show();
            }
            load.hide();
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

    var storesACargar = [App.storeNotificacionesGruposCriterios, App.storeNotificacionesCadencias];

    showLoadMask(App.gridMaestro, function (load) {
        CargarStoresSerie(storesACargar, function (Fin) {
            if (Fin) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }

                            App.cmbCadencias.getTrigger(0).show();
                            App.cmbGrupo.getTrigger(0).show();
                            App.txtFechaActivacion.focus(false, 500);
                            App.storePrincipal.reload();

                        }
                    });
            }
            load.hide();
        });
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

function Refrescar() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

function SeleccionarGrupo() {
    App.cmbGrupo.getTrigger(0).show();
}

function RecargarGrupos() {
    recargarCombos([App.cmbGrupo]);
}

function SeleccionarCadencia() {
    App.cmbCadencias.getTrigger(0).show();
}

function RecargarCadencias() {
    recargarCombos([App.cmbCadencias]);
}

// FIN MAESTRO

// INICIO DETALLE

// INICIO GESTION GRID

function Grid_RowSelect_Detalle(sender, registro, index) {

    var datos = registro.data;

    if (datos != null) {

        seleccionado = datos;

        seleccionadoDetalle = datos;
        App.btnEliminarDetalle.enable();
        App.btnEditarDetalle.enable();
        App.btnActivarDetalle.enable();

        App.btnEditarDetalle.setTooltip(jsEditar);
        App.btnEliminarDetalle.setTooltip(jsEliminar);
        App.btnAnadirDetalle.setTooltip(jsAgregar);
        App.btnRefrescarDetalle.setTooltip(jsRefrescar);
        App.btnDescargarDetalle.setTooltip(jsDescargar);

        if (seleccionado.Activo) {
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
    VaciarFormularioDetalle();

    var storesACargar = [App.storeProyectosTipos, App.storePerfiles];

    showLoadMask(App.GridDetalle, function (load) {
        CargarStoresSerie(storesACargar, function (Fin) {
            if (Fin) {
                App.txtCorreo.focus(false, 500);
                App.winGestionDetalle.setTitle(jsAgregar);
                App.winGestionDetalle.show();
            }
            load.hide();
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
    if (registroSeleccionado(App.GridDetalle) && seleccionado != null) {
        ajaxEditarDetalle();
    }
}

function ajaxEditarDetalle() {
    VaciarFormularioDetalle();
    App.winGestionDetalle.setTitle(jsEditar);

    var storesACargar = [App.storeProyectosTipos, App.storePerfiles];

    showLoadMask(App.GridDetalle, function (load) {
        CargarStoresSerie(storesACargar, function (Fin) {
            if (Fin) {
                TreeCore.MostrarEditarDetalle(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {
                                App.cmbProyectosTipos.getTrigger(0).show();
                                App.cmbPerfiles.getTrigger(0).show();
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
            load.hide();
        });
    });
}

function EliminarDetalle() {
    if (registroSeleccionado(App.GridDetalle) && seleccionado != null) {
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

function ActivarDetalle() {
    if (registroSeleccionado(App.GridDetalle) && seleccionado != null) {
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

function RefrescarDetalle() {
    App.storeDetalle.reload();
    DeseleccionarGrillaDetalle();
}

function SeleccionarPerfil() {
    App.cmbPerfiles.getTrigger(0).show();
}

function RecargarPerfiles() {
    recargarCombos([App.cmbPerfiles]);
}

function SeleccionarProyectoTipo() {
    RecargarPerfiles();
    App.cmbProyectosTipos.getTrigger(0).show();
}

function RecargarProyectosTipos() {
    recargarCombos([App.cmbProyectosTipos]);
    RecargarPerfiles();
}

// FIN DETALLE


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

//BOTON ENVIAR
function BotonNotificar() {
    TreeCore.EnviarNotificaciones(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}
//FIN BOTON ENVIAR
