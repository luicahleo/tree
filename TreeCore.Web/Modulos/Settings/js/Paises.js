var Agregar = false;
var seleccionado;


//#region INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnDefecto.enable();
        App.btnActivar.enable();

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
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

function renderIconoBandera(value, registro, index) {
    return `<span class="${value}" style="width: 20px; display: inline-block;">&nbsp;</span>`;
}

//#endregion FIN GESTION GRID 

//#region INICIO GESTION 

function VaciarFormulario() {
    App.formGestion.getForm().reset();

    Ext.each(App.winGestion.body.query('*'), function (value) {
        Ext.each(value, function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && c.isFormField) {
                c.reset();

                if (c.triggerWrap != undefined) {
                    c.triggerWrap.removeCls("itemForm-novalid");
                    c.triggerWrap.addCls("itemForm-valid");
                }

                if (!c.allowBlank && c.xtype != "checkboxfield") {
                    c.addListener("change", anadirClsNoValido, false);
                    c.addListener("focusleave", anadirClsNoValido, false);

                    c.addCls("ico-exclamacion-10px-red");
                    c.removeCls("ico-exclamacion-10px-grey");
                }

                if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                }
            }
        });
    });
}

function FormularioValido(valid) {

    if (valid == true) {
        App.btnGuardar.setDisabled(false);

        Ext.each(App['formGestion'].body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnGuardar.setDisabled(true);
            }
        });
    }
    else {
        App.btnGuardar.setDisabled(true);
    }

}

function AgregarEditar() {
    VaciarFormulario();

    showLoadMask(App.grid, function (load) {
        recargarCombos([App.cmbRegion, App.cmbIconos], function (Fin) {
            if (Fin) {
                App.cmbRegion.focus(false, 200);
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

    showLoadMask(App.grid, function (load) {
        recargarCombos([App.cmbRegion, App.cmbIconos], function (Fin) {
            if (Fin) {
                if (seleccionado.RegionID != null) {
                    App.cmbRegion.getTrigger(0).show();
                }
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            App.cmbRegion.focus(false, 200);
                            //App.storePrincipal.reload();
                            App.storeRegiones.reload();
                            load.hide();
                        },
                        eventMask:
                        {
                            showMask: true,
                            msg: jsMensajeProcesando
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

//#endregion FIN GESTION 

//#region INICIO REGIONES

function RecargarRegion() {
    recargarCombos([App.cmbRegion]);
}

function SeleccionarRegion() {
    App.cmbRegion.getTrigger(0).show();
}

//#endregion FIN REGIONES

//#region INICIO CLIENTES


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
// #endregion