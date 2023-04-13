var Agregar = false;
var seleccionado;
var seleccionadoUsuario;


//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnDefecto.enable();
        App.btnActivar.enable();
        App.btnUsuarios.enable();

        App.btnUsuarios.setTooltip(jsUsuarios)
        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDefecto.setTooltip(jsDefecto);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }
    }
}

function DeseleccionarGrilla() {
    if (App.hdCliID.value == 0 || App.hdCliID.value == undefined) {
        App.btnAnadir.disable();
    } else {
        App.btnAnadir.enable();
    }
    App.GridRowSelect.clearSelections();
    App.btnEditar.disable();
    App.btnActivar.disable();
    App.btnEliminar.disable();
    App.btnDefecto.disable();
    App.btnUsuarios.disable();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}



//FIN GESTION GRID 

// INICIO GESTION 

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
    App.txtGrupoAcceso.focus(false, 200);
    App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
    Agregar = true;
    App.winGestion.show();
}


function winGestionBotonGuardar() {
    if (App.formGestion.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsTieneRegistros, buttons: Ext.Msg.OK });
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
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winGestion.setTitle(jsEditar + " " + jsTituloModulo);

    TreeCore.MostrarEditar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.txtGrupoAcceso.focus(false, 200);
                
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function Activar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
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
    if (registroSeleccionado(App.grid) && seleccionado != null) {
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

function Defecto() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
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
            ajaxDefecto('yes');
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
    App.GridRowSelect.clearSelections();
}

// FIN GESTION 

// INICIO CLIENTES


function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

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

// FIN CLIENTES

/* INICIO USUARIO */
function BotonGestionUsuarios() {
    showLoadMask(App.grid, function (load) {
        CargarStoresSerie([App.storeUsuarios], function (Fin) {
            if (Fin) {
                App.winUsuarios.show();
                load.hide();
            }
        });
    });
}

function BotonAgregarUsuario() {
    showLoadMask(App.winUsuarios, function (load) {
        CargarStoresSerie([App.storeUsuariosLibres], function (Fin) {
            if (Fin) {
                DeseleccionarGrillaUsuario();
                DeseleccionarGrillaUsuariosLibres();
                App.winUsuariosLibres.show();
                load.hide();
            }
        });
    });
}

function BotonEliminarUsuario() {
    if (registroSeleccionado(App.GridUsuarios) && seleccionado != null) {
        Ext.Msg.show(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarUsuario,
                icon: Ext.MessageBox.QUESTION
            });
    }
}


function ajaxEliminarUsuario(button) {
    if (button == 'yes' || button == 'si') {
        showLoadMask(App.grid, function (load) {
            TreeCore.EliminarUsuario({
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                
                        CargarStoresSerie([App.storeUsuarios], function (Fin) {
                            if (Fin) {
                                DeseleccionarGrillaUsuario();
                                load.hide();
                            }
                    });
                }
            });
        });
    }
}


//winGestionUsuariosGrupos

function BotonAgregarUsuariosGrupo() {
    TreeCore.AgregarUsuario({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                App.storeUsuarios.reload();
                App.winGestion.hide();
            }
        },
        eventMask:
        {
            showMask: true,
            msg: jsMensajeProcesando
        }
    });
}
function Grid_RowSelectUsuario(sender, registro, index) {
    var datos = registro.data;
    if (datos != null) {
        seleccionadoUsuario = datos;
        App.btnEliminarUsuario.enable();
    }
}

function DeseleccionarGrillaUsuario() {
    App.btnAgregarUsuario.enable();
    App.RowSelectionModelUsuario.clearSelections();
    App.btnEliminarUsuario.disable();
}

function Grid_RowSelectUsuariosLibres(sender, registro, index) {
    var datos = registro.data;
    if (datos != null) {
        seleccionadoUsuarioGrupo = datos;
        App.btnAgregarUsuariosLibres.enable();
    }
}

function DeseleccionarGrillaUsuariosLibres() {
    App.btnCancelarUsuariosLibres.enable();
    App.RowSelectionModelUsuariosLibres.clearSelections();
    App.btnAgregarUsuariosLibres.disable();
}


/* FIN USUARIO */