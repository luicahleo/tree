var seleccionado;

//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEliminar.enable();
        App.btnActivar.enable();

        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);

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
    App.btnActivar.disable();
    App.btnEliminar.disable();
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

function Agregar() {
    VaciarFormulario();
    App.txtCodigoLicencia.focus(false, 200);
    App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
    App.winGestion.show();
}

function winGestionBotonGuardar() {
    if (App.formGestion.getForm().isValid()) {
        ajaxAgregar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregar() {
    TreeCore.Agregar(
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

function Activar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
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

// INICIO TIPO LICENCIAS

function SeleccionarTipoLicencia() {
    App.cmbTiposLicencias.getTrigger(0).show();
}

function RecargarTipoLicencia() {
    recargarCombos([App.cmbTiposLicencias]);
}

function winGenerarBotonGuardar() {
    Ext.Msg.show(
        {
            title: jsTituloModulo,
            msg: jsCodigoMsg,
            buttons: Ext.Msg.YESNO,
            fn: ajaxGenerarCodigo,
            icon: Ext.MessageBox.QUESTION
        });
}

function BotonGenerarCodigo() {
    VaciarFormularioGenerar();
    showLoadMask(App.grid, function (load) {
        recargarCombos([App.cmbTiposLicencias], function (Fin) {
            if (Fin) {
                App.winGenerar.setTitle(jsAgregar + ' ' + jsTituloModulo);
                App.winGenerar.show();
                load.hide();
            }
        });
    });
}

function ajaxGenerarCodigo(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.GenerarCodigo({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGenerar.hide();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function VaciarFormularioGenerar() {
    App.FormPanelGenerar.getForm().reset();
    nRound = 0;
    App.cmbTiposLicencias.getTrigger(0).hide();
}

function FormularioValidoGenerar(valid) {
    if (valid) {
        App.btnGuardarGenerar.setDisabled(false);
    }
    else {
        App.btnGuardarGenerar.setDisabled(true);
    }
}

function BotonTiposLicencias() {
    parent.addTab(parent.App.tabPpal, jsLicenciasTipoTitle, jsLicenciasTipoTitle, "ModGlobal/pages/LicenciasTipos.aspx");
}

// FIN TIPO LICENCIAS