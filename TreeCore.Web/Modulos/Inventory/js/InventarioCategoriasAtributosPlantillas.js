var Agregar = false;
var seleccionado;

//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnAnadirPlantilla.enable();
        App.btnEliminarPlantilla.disable();
        App.btnEditarPlantilla.disable();

        App.btnEditarPlantilla.setTooltip(jsEditar);
        App.btnEliminarPlantilla.setTooltip(jsEliminar);
        App.btnAnadirPlantilla.setTooltip(jsAgregar);

        AnadirScriptjs('../../Componentes/js/GestionCategoriasAtributos.js');
        AnadirScriptjs('../../Componentes/js/GestionAtributos.js');
        App.hdCatSelect.setValue(seleccionado.InventarioAtributoCategoriaID);
        App.hdCatConfSelect.setValue(seleccionado.CoreInventarioCategoriaAtributoCategoriaConfiguracionID);
        showLoadMaskCategorias(function (load) {
            TreeCore.PintarCategorias(true,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        App.winGestion.updateLayout();
                        var numCols = App.grid.columnManager.columns.length;
                        for (var i = 1; i < numCols; i++) {
                            App.grid.removeColumn(1, false);
                        }
                        TreeCore.GenerarGridCat(
                            {
                                success: function (result) {
                                    if (result.Result != null && result.Result != '') {
                                        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                    }
                                    App.storePlantillas.reload();
                                    load.hide();
                                }
                            });
                    }
                });
        });
    }
}

function DeseleccionarGrilla() {
    App.GridRowSelect.clearSelections();
    App.btnAnadirPlantilla.disable();
    App.btnEditarPlantilla.disable();
    App.btnEliminarPlantilla.disable();
    App.hdCatSelect.setValue(0);
    App.hdCatConfSelect.setValue(0);
}

function Grid_RowSelectPlantillas(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditarPlantilla.enable();
        App.btnEliminarPlantilla.enable();

        App.btnEditarPlantilla.setTooltip(jsEditar);
        App.btnEliminarPlantilla.setTooltip(jsEliminar);
        App.btnAnadirPlantilla.setTooltip(jsAgregar);
    }
}

function DeseleccionarGrillaPlantillas() {
    App.GridRowSelectPlantillas.clearSelections();
    App.btnAnadirPlantilla.enable();
    App.btnEditarPlantilla.disable();
    App.btnEliminarPlantilla.disable();
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

// INICIO DIRECT METHOD 



function Refrescar() {
    App.storePrincipal.reload();
    App.GridRowSelect.clearSelections();
}

function VerActivos() {
    if (App.colActivo.hidden) {
        App.colActivo.show();
    } else {
        App.colActivo.hide();
    }
    Refrescar();
}

function AgregarEditarPlantilla() {
    App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
    VaciarFormulario();
    FormularioValido();
    App.winGestion.show();
    Agregar = true;
}

function MostrarEditarPlantilla() {
    Agregar = false;
    VaciarFormulario();
    showLoadMaskCategorias(function (load) {
        TreeCore.MostrarEditar(
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        for (var prop in result.Result) {
                            SeleccionarValorMostrarEditar(prop, result.Result[prop]);
                        }
                        FormularioValido();
                        App.winGestion.setTitle(jsEditar + ' ' + jsTituloModulo);
                        App.winGestion.show();
                    }
                    load.hide();
                }
            });
    });
}

function EliminarPlantilla() {
    Ext.Msg.alert(
        {
            title: jsEliminar + ' ' + jsPlantilla,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: AjaxEliminarPlantilla,
            icon: Ext.MessageBox.QUESTION
        });
}

function AjaxEliminarPlantilla(button) {
    if (button == 'yes' || button == 'si') {
        showLoadMaskCategorias(function (load) {
            TreeCore.Eliminar(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        load.hide();
                        App.storePlantillas.reload();
                    }
                });
        });
    }
}

function RefrescarPlantilla() {
    App.storePlantillas.reload();
}

// FIN DIRECT METHOD 

// INICIO CLIENTES

function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

function RecargarClientes() {
    recargarCombos([App.cmbClientes]);
}

function SeleccionarCliente() {
    App.cmbClientes.getTrigger(0).show();
    App.hdCliID.setValue(App.cmbClientes.value);
    CargarStores();
}

// FIN CLIENTES

function showLoadMaskCategorias(callback) {
    var myMask = new Ext.LoadMask({
        msg: App.jsCargando,
        target: App.vwContenedor
    });

    myMask.show();
    callback(myMask, null);
}

//#region Formulario

function winGestionBotonGuardar() {
    showLoadMask(App.winGestion, function (load) {
        TreeCore.AgregarEditar(Agregar,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    load.hide();
                    App.winGestion.hide();
                    App.storePlantillas.reload();
                }
            });
    });
}

function FormularioValido(sender, valid, aux, ruta) {
    try {
        App.btnGuardar.setDisabled(false);
        Ext.each(App.formGestion.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnGuardar.setDisabled(true);

                if (c.triggerWrap != undefined) {
                    c.triggerWrap.removeCls("itemForm-novalid");
                }

                if (!c.allowBlank && c.xtype != "checkboxfield") {
                    c.addListener("change", anadirClsNoValido, false);
                    c.addListener("focusleave", anadirClsNoValido, false);

                    c.removeCls("ico-exclamacion-10px-red");
                    c.addCls("ico-exclamacion-10px-grey");
                }

                if ((c.allowBlank && c.cls == 'txtContainerCategorias') || c.xtype == "checkboxfield") {
                    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                }
            }
        });
    } catch (e) {

    }
}
function VaciarFormulario() {
    try {
        App.btnGuardar.setDisabled(false);
        Ext.each(App.formGestion.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && c.isFormField) {
                c.reset();
            }
        });
    } catch (e) {

    }
}

//#endregion