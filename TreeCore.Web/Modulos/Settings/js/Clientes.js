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
        App.btnProjects.enable();
        App.btnAnonimizar.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDefecto.setTooltip(jsDefecto);
        App.btnDefecto.setTooltip(jsDefecto);
        App.btnProjects.setTooltip(jsProyectosTipos);
        App.btnAnonimizar.setTooltip(jsAnonimizar);

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
        App.btnAnadir.enable();
    } else {
        App.btnAnadir.disable();
    }
    App.GridRowSelect.clearSelections();
    App.btnEditar.disable();
    App.btnActivar.disable();
    App.btnEliminar.disable();
    App.btnDefecto.disable();
    App.btnProjects.disable();
    App.btnAnonimizar.disable();
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
    App.logoCliente.Hidden = true;
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
        recargarCombos([App.cmbOperadores, App.cmbMoneda], function (Fin) {
            if (Fin) {
                App.txtCliente.focus(false, 200);
                App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
                Agregar = true;
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
        recargarCombos([App.cmbOperadores, App.cmbMoneda], function (Fin) {
            if (Fin) {
                if (seleccionado.Operador != null) {
                    App.cmbOperadores.getTrigger(0).show();
                }
                if (seleccionado.MonedaID != null) {
                    App.cmbMoneda.getTrigger(0).show();
                }
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            App.txtCliente.focus(false, 200);
                            App.storePrincipal.reload();
                            load.hide();
                        }
                    });
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
                msg: jsMensajeEliminar ,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

var ImageRender = function (value) {
    if (value != null) {
        return '<span class="gen_activo" >&nbsp;</span>';
    }
    else {
        return '<span>&nbsp;</span>';
    }
}

function CargarImagen(input) {
    App.logoCliente.hide();
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

//Proyectos Tipos

function Grid_RowSelectProyectosTipos(sender, registro, index) {
    var datos = registro.data;
    if (datos != null) {
        seleccionado = datos;

        App.btnQuitarProyectosTipos.enable();
    }
}

function DeseleccionarGrillaProyectosTipos(sender, index, registro, d) {
    App.GridRowSelectProyectosTipos.clearSelections();
    App.btnQuitarProyectosTipos.disable();
}

function BotonProyectosTipos() {
    App.storeProyectosTipos.reload();
    App.winProyectosTipos.show();
}

function GridProyectosTiposLibresSeleccionar_RowSelect(sender, registro, index) {
    var datos = registro.data;
    if (datos != null) {
        seleccionado = datos;

        App.btnGuardarProyectosTiposLibre.enable();
    }
}

function DeseleccionarGrillaProyectosTiposLibres() {
    App.GridRowSelectProyectosTiposLibres.clearSelections();
}

function BotonAgregarProyectosTipos() {
    App.storeProyectosTiposLibres.reload();
    App.winProyectosTiposLibres.show();
}

function BotonEliminarProyectosTipos() {
    if (registroSeleccionado(App.gridProyectosTipos) && seleccionado != null) {
        Ext.Msg.show(
            {
                title: jsEliminar + ' ' + jsProyectosTipos,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxQuitarProyectosTipos,
                icon: Ext.MessageBox.QUESTION
            });
    }
}


function ajaxQuitarProyectosTipos(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.QuitarProyectoTipo({
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
    TreeCore.AgregarProyectoTipo(
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

function BotonAnonimizar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        Ext.Msg.show(
            {
                title: jsAnonimizar + ' ' + jsTituloModulo,
                msg: jsMensajeAnonimizar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxAnonimizar,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function winanonimizarguardar() {

    ajaxAnonimizar();

}

function ajaxAnonimizar(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.Anonimizar(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                }
            });
    }
    App.storePrincipal.reload();
}
/* Trigger Combos FormGestion */

function RecargarOperadores() {
    recargarCombos([App.cmbOperadores]);
}

function SeleccionarOperadores() {
    App.cmbOperadores.getTrigger(0).show();
}

function RecargarMoneda() {
    recargarCombos([App.cmbMoneda]);
}

function SeleccionarMoneda() {
    App.cmbMoneda.getTrigger(0).show();
}

/* Fin Trigger Combos FormGestion */