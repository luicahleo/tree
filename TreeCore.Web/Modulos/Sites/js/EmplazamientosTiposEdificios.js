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
        App.btnTiposEstructuras.enable();
        App.btnCosteDesmantelamiento.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDefecto.setTooltip(jsDefecto);
        App.btnTiposEstructuras.setTooltip(jsTiposEstructuras);
        App.btnCosteDesmantelamiento.setTooltip(jsCosteDesmantelamiento);


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
    App.btnTiposEstructuras.disable();
    App.btnCosteDesmantelamiento.disable();

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
    App.cmbPais.getTrigger(0).hide();
    App.cmbMoneda.getTrigger(0).hide();
    App.cmbAnualidad.getTrigger(0).hide();
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
    App.txtTipoEdificio.focus(false, 200);
    App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
    Agregar = true;
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
                App.txtTipoEdificio.focus(false, 200);
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
        }
        else {
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

// INICIO TIPO ESTRUCTURA

function BotonTiposEstructuras() {
    App.storeTiposEstructuras.reload();
    App.winTiposEstructuras.show();
}

function BotonAgregarTipoEstructura() {
    App.storeTiposEstructurasLibres.reload();
    App.winTiposEstructurasLibres.show();
}

function Grid_RowSelectTiposEstructuras(sender, registro, index ) {
    var datos = registro.data;
    if (datos != null) {
        seleccionado = datos;

        App.btnQuitar.enable();
        App.btnQuitar.setTooltip(jsEliminar);
    }
}

function BotonEliminarTipoEstructura() {
    if (registroSeleccionado(App.gridTiposEstructuras) && seleccionado != null) {
        Ext.Msg.show(
            {
                title: jsEliminar + ' ' + jsTituloModulo,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxQuitarTiposEstructuras,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxQuitarTiposEstructuras(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.QuitarTipoEstructura({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
    App.storeTiposEstructuras.reload();
}

function BotonRefrescarTipoEstructura() {
    App.storeTiposEstructurasLibres.reload();
}

// FIN TIPO ESTRUCTURA

// INICIO COSTES

function BotonAsignarCoste() {
    App.storeTiposEdificiosPaises.reload();
    showLoadMask(App.grid, function (load) {
        recargarCombos([App.cmbPais], function (Fin) {
            if (Fin) {
                App.winTipoEdificiosPaises.show();
                load.hide();
            }
        });
    });
}

function BotonAgregarCoste() {
    VaciarFormularioCostesDesmantelamiento();
    App.winCostesDesmantelamiento.setTitle(jsAgregar + ' ' + jsTituloModuloCC);
    App.winCostesDesmantelamiento.show();

}

function VaciarFormularioCostesDesmantelamiento() {
    App.formCostesDesmantelamiento.getForm().reset();

}

function FormularioValidoCosteDesmantelamiento(valid) {
    if (valid) {
        App.btnGuardarCostes.setDisabled(false);
    }
    else {
        App.btnGuardarCostes.setDisabled(true);
    }
}

function winCostesBotonGuardar() {
    if (App.formCostesDesmantelamiento.getForm().isValid()) {
        ajaxGuardarCuentaCorriente();
    }
    else {
        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxGuardarCuentaCorriente() {

    var Agregar = false;

    if (App.winCostesDesmantelamiento.title.startsWith(jsAgregar)) {
        Agregar = true;
    }
    TreeCore.GurardarCostesDesmantelamiento(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winCostesDesmantelamiento.hide();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    App.storeTiposEdificiosPaises.reload();
}

function BotonAgregarCoste() {
    VaciarFormularioCostesDesmantelamiento();
    App.winCostesDesmantelamiento.setTitle(jsAgregar + ' ' + jsTituloModuloCC);
    App.winCostesDesmantelamiento.show();

}

function BotonEditarCoste() {
    if (registroSeleccionado(App.GridTipoEdificiosPaises) && seleccionadoCC != null) {
        ajaxEditarCoste();
    }
}

function ajaxEditarCoste() {
    VaciarFormularioCostesDesmantelamiento();
    App.winCostesDesmantelamiento.setTitle(jsEditar + ' ' + jsTituloModuloCC);

    TreeCore.MostrarEditarCoste(
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
    App.storeTiposEdificiosPaises.reload();

}

function BotonQuitarCoste() {
    if (registroSeleccionado(App.GridTipoEdificiosPaises) && seleccionadoCC != null) {
        Ext.Msg.show(
            {
                title: 'Quitar ',
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxQuitarCostes,
                icon: Ext.MessageBox.QUESTION
            });
    }

}

function ajaxQuitarCostes(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.QuitarCoste({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
    App.storeTiposEdificiosPaises.reload();

}

function BotonActivarDesactivarCoste(button) {
    if (registroSeleccionado(App.GridTipoEdificiosPaises) && seleccionadoCC != null) {
        if (seleccionadoCC.Defecto) {
            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsDesactivarPorDefecto, buttons: Ext.Msg.OK });
        }
        else {
            ajaxActivarCoste();
        }
    }
}

function ajaxActivarCoste() {
    TreeCore.ActivarDesactivarCostes({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: 'Atención', icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                App.storeTiposEdificiosPaises.reload();
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

function BotonPorDefectoCoste() {
    if (registroSeleccionado(App.GridTipoEdificiosPaises) && seleccionadoCC != null) {
        if (!seleccionadoCC.Activo) {
            Ext.Msg.alert(
                {
                    title: jsDefecto + ' ' + jsTituloModulo,
                    msg: jsRegistroInactivoPorDefecto,
                    buttons: Ext.Msg.YESNO,
                    fn: ajaxDefectoCoste,
                    icon: Ext.MessageBox.QUESTION
                });
        }
        else {
            ajaxDefectoCoste('yes');
        }
    }
}

function ajaxDefectoCoste(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.AsignarPorDefectoCoste(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        App.storeTiposEdificiosPaises.reload();
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

function BotonRefrescarCoste() {
    App.storeTiposEdificiosPaises.reload();
}

// FIN COSTES

// INICIO TIPO ESTRUCTURA LIBRE

function GridTiposEstructurasLibresSeleccionar_RowSelect(sender, index, registro) {
    var datos = registro.data;
    if (datos != null) {
        seleccionado = datos;

        App.btnGuardarTiposEstructurasLibre.enable();
    }
}

function BotonGuardarTiposEstructurasLibres() {
    ajaxAgregarTiposEstructuras();
}

function ajaxAgregarTiposEstructuras() {
    TreeCore.AgregarTiposEstructuras(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winTiposEstructurasLibres.hide();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    App.storeTiposEstructuras.reload();
}

// FIN TIPO ESTRUCTURA LIBRE

// INICIO PAISES

function GridTipoEdificiosPaises_RowSelect(sender,  registro, index) {
    var datosCC = registro.data;
    if (datosCC != null) {
        seleccionadoCC = datosCC;
        App.btnQuitarCoste.enable();
        App.btnActivarDesactivarCoste.enable();
        App.btnPorDefecto.enable();
        App.btnEditarCoste.enable();

        if (seleccionadoCC.Activo) {
            App.btnActivarDesactivarCoste.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivarDesactivarCoste.setTooltip(jsActivar);
        }
    }
}

function SeleccionarPais() {
    App.cmbPais.getTrigger(0).show();
}

function RecargarPais() {
    recargarCombos([App.cmbPais]);
}

// FIN PAISES

// INICIO MONEDAS

function SeleccionarMoneda() {
    App.cmbMoneda.getTrigger(0).show();
}

function RecargarMoneda() {
    recargarCombos([App.cmbMoneda]);
}

//FIN MONEDAS

// INICIO ANUALIDAD

function SeleccionarAnualidad() {
    App.cmbAnualidad.getTrigger(0).show();
}

function RecargarAnualidad() {
    recargarCombos([App.cmbAnualidad]);
}

// FIN ANUALIDAD

// INICIO TIPO EDIFICIOS

function DeseleccionarTiposEdificiosPaises() {

    App.btnAgregarCoste.enable();
    App.TipoEdificiosPaisesRowSelection.clearSelections();
    App.btnQuitarCoste.disable();
    App.btnActivarDesactivarCoste.disable();
    App.btnPorDefecto.disable();
    App.btnEditarCoste.disable();
}

// FIN TIPO EDIFICIOS
