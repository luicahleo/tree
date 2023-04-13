var Agregar = false;
var seleccionado;


//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnActivar.enable();
        App.btnAsignarFrecuencias.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnAsignarFrecuencias.setTooltip(jsAsignarFrecuencia);

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
    App.btnAsignarFrecuencias.disable();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

var handlePageSizeSelectAsignadas = function (item, records) {
    var curPageSize = App.storeFrecuenciasAsignadas.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeFrecuenciasAsignadas.pageSize = wantedPageSize;
        App.storeFrecuenciasAsignadas.load();
    }
}

var handlePageSizeSelectLibres = function (item, records) {
    var curPageSize = App.storeFrecuenciasLibres.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeFrecuenciasLibres.pageSize = wantedPageSize;
        App.storeFrecuenciasLibres.load();
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
    App.txtNombre.focus(false, 200);
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
                App.txtNombre.focus(false, 200);
                App.storePrincipal.reload();
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
                msg: jsMensajeEliminar ,
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

function BotonAsignarFrecuencias() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        MostrarAsignarFrecuencias();
    }
}

function MostrarAsignarFrecuencias() {
    CargarStoresSerie([App.storeFrecuenciasAsignadas], function myfunction(Fin) {
        if (Fin) {
            App.winFrecuenciasAsignadas.show();
        }
    })
}

function GridRowSelectFrecuenciasLibresRowSeleccionar_RowSelect(sender, registro, index) {
    var datos = registro.data;
    if (datos != null) {
        seleccionadoFun = datos;
        App.btnAgregarFrecuenciaLibres.enable();
    }
}

function DeseleccionarGrillaFrecuenciasLibres() {
    App.btnAgregarFrecuenciaLibres.disable();
}

function BotonAgregarFrecuenciaLibres() {
    ajaxAgregarFrecuencia();
}


function GridRowSelectFrecuenciasAsignadaRowSeleccionar_RowSelect(sender, registro, index) {
    var datos = registro.data;
    if (datos != null) {
        seleccionadoFun = datos;
        App.btnQuitarFrecuenciasAsignada.enable();
    }
}

function DeseleccionarGrillaFrecuenciasAsignadas() {
    App.btnQuitarFrecuenciasAsignada.disable();
}



function BotonAgregarFrecuenciaAsignada() {
    CargarStoresSerie([App.storeFrecuenciasLibres], function myfunction(Fin) {
        if (Fin) {
            App.winFrecuenciasLibres.show();
        }
    })
}

function ajaxAgregarFrecuencia() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        TreeCore.AgregarFrecuenciaATipoTecTelco({
            success: function (result) {
                App.winFrecuenciasLibres.hide();
                App.storeFrecuenciasAsignadas.reload();
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    }
}

function BotonEliminarFrecuenciaAsignada() {
    if (registroSeleccionado(App.GridPanelFrecuenciasAsignadas) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarFrecuencia,
                icon: Ext.MessageBox.QUESTION
            });
    }
    App.storeFrecuenciasAsignadas.reload();
    App.GridRowSelectFrecuenciasAsignadasRowSelection.clearSelections();
}


function ajaxEliminarFrecuencia(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarFrecuenciaATipoTecTelco({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    App.storeFrecuenciasAsignadas.reload();
                }
                else {
                    App.storeFrecuenciasAsignadas.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function BotonGuardarFrecuenciasAsignada() {
    App.winFrecuenciasAsignadas.hide();
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