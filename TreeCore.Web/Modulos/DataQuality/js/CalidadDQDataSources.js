var DefectoRender = function (value) {
    if (value == "icon") {
        return '<span class="ico-defaultGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

function Refrescar() {
    App.storePrincipal.reload();
    App.GridRowSelect.clearSelections();
}

function AgregarEditar() {

    VaciarFormulario();

    App.storeTablas.reload();

    App.winGestion.setTitle(jsAgregar + ' ' + jsFuenteDatos);

    Agregar = true;
    App.winGestion.show();
}

function winFormCenterSimple(obj) {
    obj.center();
    obj.update();
}

function RecargarTablas() {
    recargarCombos([App.cmbTablas]);
}

function SeleccionarTablas() {
    App.cmbTablas.getTrigger(0).show();
}

function DeseleccionarGrilla() {
    App.GridRowSelect.clearSelections();

    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnActivar.disable();

}

function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;

        App.btnAnadir.enable()
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnActivar.enable();

        App.btnDescargar.enable();
        App.btnDescargar.setTooltip(jsDescargar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEditar.setTooltip(jsEditar);
        App.btnEliminar.setTooltip(jsEliminar);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }

    }

}

function LimpiarFiltroBusqueda(sender, registro) {
    var idComponente = sender.id.split('_');
    idComponente.pop();
    var tree = App.grid;
    var store = tree.store,
        logic = store,
        field = App.txtSearch;

    field.setValue("");
    logic.clearFilter();
    App.btnDescargar.enable();

}

function FiltrarColumnas(sender, registro) {
    App.btnDescargar.disable();
    var idComponente = sender.id.split('_');
    idComponente.pop();
    var tree = App.grid;
    var store = tree.store,
        logic = store,
        text = sender.getRawValue();

    logic.clearFilter();

    if (Ext.isEmpty(text, false)) {
        return;
    }

    if (registro.getKey() === registro.ESC) {
        clearFilter();
    } else {
        try {
            var re = new RegExp(".*" + text + ".*", "i");
        } catch (err) {
            return;
        }

        logic.filterBy(function (node) {
            var correcto = false;
            tree.columns.forEach(valores => {
                if (!correcto)
                    correcto = re.test(node.data[valores.dataIndex])

            });
            return correcto;
        });
    }
}

function VaciarFormulario() {
    App.formGestion.getForm().reset();
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
                title: jsEliminar + ' ' + jsFuenteDatos,
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

function MostrarEditar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winGestion.setTitle(jsEditar + " " + jsFuenteDatos);

    showLoadMask(App.grid, function (load) {
        TreeCore.MostrarEditar(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    App.storeTablas.reload();
                    load.hide();
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    });
}

function winGestionBotonGuardar() {
    if (App.formGestion.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsMensajeGenerico, buttons: Ext.Msg.OK });
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

function FormularioValidoGestion(valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}
