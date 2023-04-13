var Agregar = false;
var seleccionado;

//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnDefecto.enable();
        App.btnActivar.enable();
        App.btnBloquear.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDefecto.setTooltip(jsDefecto);
        App.btnBloquear.setTooltip(jsBloquear);

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
    App.btnBloquear.disable();
    App.btnBloquear.setTooltip(jsBloquear);
}

function Grid_RowSelectProyectosTipos(sender, registro, index) {
    var datos = registro.data;
    if (datos != null) {
        seleccionado = datos;

        App.btnQuitar.enable();
    }
}

function DeseleccionarGrillaProyectosTipos() {
    App.GridRowSelectProyectosTipos.clearSelections();
    App.btnQuitar.disable();
}

function Grid_RowSelectProyectoTipoLibre(sender, registro, index) {

    var datos = registro.data;
    if (datos != null) {
        seleccionado = datos;

        App.btnGuardarProyectosTiposLibre.enable();
    }
}

function DeseleccionarGrillaProyectosTiposLibres() {
    App.GridRowSelectProyectoTipoLibre.clearSelections();
    App.btnGuardarProyectosTiposLibre.disable();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}



var EstadoGlobalRender = function (value) {
    if (value != null) {
        if (value == "ico-damaged.svg") {
            var Plantilla = '<span> <img src="../../ima/ico-damaged.svg"></span>';
            return Plantilla;
        }
        else if (value == "ico-outofservice.svg") {
            var Plantilla = '<span> <img src="../../ima/ico-outofservice.svg"></span>';
            return Plantilla;
        }
        else if (value == "ico-onAir.svg") {
            var Plantilla = '<span> <img src="../../ima/ico-onAir.svg"></span>';
            return Plantilla;
        }
        else if (value == "ico-blocked.svg") {
            var Plantilla = '<span> <img src="../../ima/ico-blocked.svg"></span>';
            return Plantilla;
        }
        else if (value == "ico-trash.svg") {
            var Plantilla = '<span> <img src="../../ima/ico-trash.svg"></span>';
            return Plantilla;
        }
    }
    else {
        return '';
    }

}

function changeCmbImage(sender) {
    if (sender.valueCollection.getCount() > 0) {
        sender.setIconCls(sender.valueCollection.getAt(0).get('id'));
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

    showLoadMask(App.grid, function (load) {
        App.txtEstadoGlobal.focus(false, 200);
        App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
        Agregar = true;
        App.winGestion.show();
        load.hide();
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

function BotonBloquear() {
    showLoadMask(App.grid, function (load) {
        CargarStoresSerie([App.storeProyectosTipos], function (Fin) {
            if (Fin) {
                App.winProyectosTipos.show();
                load.hide();
            }
        });
    });
}

function BotonAgregarProyectoTipo() {
    showLoadMask(App.grid, function (load) {
        CargarStoresSerie([App.storeProyectosTiposLibres], function (Fin) {
            if (Fin) {
                App.winProyectosTiposLibres.show();
                load.hide();
            }
        });
    });
}


function BotonEliminarProyectoTipo() {
    if (registroSeleccionado(App.gridProyectosTipos) && seleccionado != null) {
        Ext.Msg.show(
            {
                title: jsEliminar + ' ' + jsTituloModulo,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxQuitarProyectosTipos,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxQuitarProyectosTipos(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.QuitarProyectosTipos({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
    App.storeProyectosTipos.reload();
}

function BotonGuardarProyectosTiposLibres() {
    ajaxAgregarProyectosTipos();

}

function ajaxAgregarProyectosTipos() {
    TreeCore.AgregarProyectosTipos(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winProyectosTiposLibres.hide();
                    App.storeProyectosTipos.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
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

    showLoadMask(App.grid, function (load) {
        TreeCore.MostrarEditar(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    App.txtEstadoGlobal.focus(false, 200);
                    App.storePrincipal.reload();
                    load.hide();
                }
            });
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