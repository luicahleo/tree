var allowSelect = false;
function DeseleccionarGrilla() {
    App.GridRowSelect.clearSelections();
    App.btnEditar.disable();
    App.btnCopiar.disable();
    App.btnMover.disable();
    App.btnClonar.disable();
    App.btnEliminar.disable();
    App.btnEditar.setTooltip(jsEditar);
    App.btnEliminar.setTooltip(jsEliminar);
    App.btnCopiar.setTooltip(jsCopiar);
    App.btnMover.setTooltip(jsMover);
    App.btnClonar.setTooltip(jsClonar);
}

function VerMas(sender, registro, index) {
    parent.parent.CargarPanelMoreInfo(sender.$widgetRecord.data.InventarioElementoID, true);
}

function Grid_RowSelect(sender, registro, index) {
    if (App.hdCategoriaActiva.value == 'Activa') {
        App.btnEditar.enable();
        App.btnCopiar.enable();
        App.btnClonar.enable();
        App.btnMover.enable();
    }
    App.btnEliminar.enable();
    if (sender.selected.items.length > 1) {
        App.btnEditar.disable();
        App.btnCopiar.disable();
        App.btnEliminar.disable();
    }
    App.btnEditar.setTooltip(jsEditar);
    App.btnEliminar.setTooltip(jsEliminar);
    App.btnCopiar.setTooltip(jsCopiar);
    App.btnMover.setTooltip(jsMover);
    App.btnClonar.setTooltip(jsClonar);
    parent.parent.CargarPanelMoreInfo(registro.data.InventarioElementoID, false);
}

//#region Botones Toolbar

var bAgregar;

var ContadorIntentos = 100;

function AgregarEditar() {
    ContadorIntentos = 100;
    bAgregar = true;
    showLoadMask(App.grid, function (load) {
        if (App.cmbEstado.store.data.items.length == 0 && App.hdCategoriaID.value != '0') {
            recargarCombos(storesACargar, function Fin(fin) {
                if (fin) {
                    App.cmbEstado.resetOriginalValue();
                    App.cmbCategoriaElemento.resetOriginalValue();
                    App.cmbOperador.resetOriginalValue();
                    App.winGestion.show();
                    CargarFormulario(function Fin(fin2) {
                        if (fin2) {
                            if (App.hdVistaPlantilla.value != "" && App.hdVistaPlantilla.value != undefined) {
                                App.winGestion.setTitle(jsAgregar + ' ' + jsPlantilla);
                            } else {
                                App.winGestion.setTitle(jsAgregar + ' ' + jsInventarioElemento);
                            }
                            FormularioValidoInventario(App.pnConfigurador, true, "");
                            load.hide();
                        }
                    });
                }
            });
        } else {
            App.winGestion.show();
            CargarFormulario(function Fin(fin) {
                if (fin) {
                    if (App.hdVistaPlantilla.value != "" && App.hdVistaPlantilla.value != undefined) {
                        App.winGestion.setTitle(jsAgregar + ' ' + jsPlantilla);
                    } else {
                        App.winGestion.setTitle(jsAgregar + ' ' + jsInventarioElemento);
                    }
                    FormularioValidoInventario(App.pnConfigurador, true, "");
                    load.hide();
                }
            });
        }
    });
}

function btnCopiarElementos() {
    bAgregar = true;
    showLoadMask(App.grid, function (load) {
        if (App.cmbEstado.store.data.items.length == 0 && App.hdCategoriaID.value != '0') {
            recargarCombos(storesACargar, function Fin(fin) {
                if (fin) {
                    App.cmbEstado.resetOriginalValue();
                    App.cmbCategoriaElemento.resetOriginalValue();
                    App.cmbOperador.resetOriginalValue();
                    CargarFormulario(function Fin(fin) {
                        if (fin) {
                            TreeCore.MostrarEditar(
                                {
                                    success: function (result) {
                                        if (!result.Success) {
                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                            load.hide();
                                        }
                                        else {
                                            MostrarEditarDato(result.Result);
                                            App.winGestion.setTitle(jsCopiar + ' ' + jsInventarioElemento);
                                            App.txtNombreElemento.setValue('');
                                            App.txtCodigoElemento.setValue('');
                                            GenerarCodigoInventario(function GenerarCodigo(codigoGenerado) {
                                                if (codigoGenerado) {
                                                    load.hide();
                                                    App.winGestion.show();
                                                    FormularioValidoInventario(App.pnConfigurador, true, "");
                                                }
                                                else {
                                                    load.hide();
                                                    App.winGestion.show();
                                                    FormularioValidoInventario(App.pnConfigurador, true, "");
                                                }
                                            });
                                        }
                                    }
                                });
                        }
                    }, false);
                }
            });
        } else {
            CargarFormulario(function Fin(fin) {
                if (fin) {
                    TreeCore.MostrarEditar(
                        {
                            success: function (result) {
                                if (!result.Success) {
                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                    load.hide();
                                }
                                else {
                                    MostrarEditarDato(result.Result);
                                    App.winGestion.setTitle(jsCopiar + ' ' + jsInventarioElemento);
                                    App.txtNombreElemento.setValue('');
                                    App.txtCodigoElemento.setValue('');
                                    GenerarCodigoInventario(function GenerarCodigo(codigoGenerado) {
                                        if (codigoGenerado) {
                                            load.hide();
                                            App.winGestion.show();
                                            FormularioValidoInventario(App.pnConfigurador, true, "");
                                        }
                                        else {
                                            load.hide();
                                            App.winGestion.show();
                                            FormularioValidoInventario(App.pnConfigurador, true, "");
                                        }
                                    });
                                }
                            }
                        });
                }
            }, false);
        }
    });
}
function MostrarEditar() {
    bAgregar = false;
    showLoadMask(App.grid, function (load) {
        if (App.cmbEstado.store.data.items.length == 0 && App.hdCategoriaID.value != '0') {
            recargarCombos(storesACargar, function Fin(fin) {
                if (fin) {
                    App.cmbEstado.resetOriginalValue();
                    App.cmbCategoriaElemento.resetOriginalValue();
                    App.cmbOperador.resetOriginalValue();
                    CargarFormulario(function Fin(fin) {
                        if (fin) {
                            TreeCore.MostrarEditar(
                                {
                                    success: function (result) {
                                        if (!result.Success) {
                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                            load.hide();
                                        }
                                        else {
                                            MostrarEditarDato(result.Result);
                                            if (App.hdVistaPlantilla.value != "" && App.hdVistaPlantilla.value != undefined) {
                                                App.winGestion.setTitle(jsEditar + ' ' + jsPlantilla);
                                            } else {
                                                App.winGestion.setTitle(jsEditar + ' ' + jsInventarioElemento);
                                            }
                                            load.hide();
                                            App.winGestion.show();
                                            FormularioValidoInventario(App.pnConfigurador, true, "");
                                        }
                                    }
                                });
                        }
                    }, false);
                }
            });
        } else {
            CargarFormulario(function Fin(fin) {
                if (fin) {
                    TreeCore.MostrarEditar(
                        {
                            success: function (result) {
                                if (!result.Success) {
                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                    load.hide();
                                }
                                else {
                                    MostrarEditarDato(result.Result);
                                    if (App.hdVistaPlantilla.value != "" && App.hdVistaPlantilla.value != undefined) {
                                        App.winGestion.setTitle(jsEditar + ' ' + jsPlantilla);
                                    } else {
                                        App.winGestion.setTitle(jsEditar + ' ' + jsInventarioElemento);
                                    }
                                    load.hide();
                                    App.winGestion.show();
                                    FormularioValidoInventario(App.pnConfigurador, true, "");
                                }
                            }
                        });
                }
            }, false);
        }
    });
}

function MostrarEditarDato(elemento) {
    App.txtNombreElemento.setValue(elemento.Nombre);
    App.txtCodigoElemento.setValue(elemento.NumeroInventario);
    App.cmbEstado.setValue(elemento.EstadoID);
    App.cmbOperador.setValue(elemento.OperadorID);
    for (var prop in elemento.JsonAtributosDinamicos) {
        SeleccionarValorMostrarEditar(elemento.JsonAtributosDinamicos[prop].ControlID, elemento.JsonAtributosDinamicos[prop].Valor);
    }
}

function Eliminar() {
    if (App.hdVistaPlantilla.value != "" && App.hdVistaPlantilla.value != undefined) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsPlantilla,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION
            });
    } else {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsInventarioElemento,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminar(button) {
    if (button == 'yes' || button == 'si') {
        showLoadMask(App.grid, function (load) {
            TreeCore.EliminarElemento(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        CargarStoresSerie([App.storePrincipal], function Fin(fin) {
                            if (fin) {
                                load.hide();
                            }
                        });
                    }
                });
        });
    }
}

function Refrescar() {
    showLoadMask(App.grid, function (load) {
        App.grid.store.clearFilter();
        App.hdOperador.setValue(undefined);
        App.hdEstado.setValue(undefined);
        App.hdUsuario.setValue(undefined);
        App.hdFechaMinCrea.setValue(undefined);
        App.hdFechaMaxCrea.setValue(undefined);
        App.hdFechaMinMod.setValue(undefined);
        App.hdFechaMaxMod.setValue(undefined);
        CargarStoresSerie([App.storePrincipal], function Fin(fin) {
            if (fin) {
                load.hide();
            }
        });
    });
}

function cargarInventario(grid) {
    MontarGrid();
}

function DescargarTotal() {
    if (App.hdEmplazamientoID.value != 0 && App.hdEmplazamientoID.value != "") {
        parent.parent.ExportarCategorias(0);
    } else {
        parent.parent.ExportarCategorias(App.hdCliID.value);
    }
}

function Exportar() {
    Cookies.set('DescargaInventarioFiltros', App.hdFiltros.value);
    Cookies.set('DescargaInventarioGrid', App.hdViewJson.value);
    ExportarDatos('InventarioCategoryViewVistaCategoria', '', App.grid,
        '\'\'&EmplazamientoID=' + App.hdEmplazamientoID.value + '&CategoriaID=' + App.hdCategoriaID.value + '&VistaPlantilla=' + App.hdVistaPlantilla.value + '&VistaInventario=' + App.hdVistaInventario.value +
        '&Operadores=' + App.hdOperador.value +
        '&Estados=' + App.hdEstado.value +
        '&Usuarios=' + App.hdUsuario.value +
        '&FechaCreacionMinima=' + App.hdFechaMinCrea.value +
        '&FechaCreacionMaxima=' + App.hdFechaMaxCrea.value +
        '&FechaModificacionMinima=' + App.hdFechaMinMod.value +
        '&FechaModificacionMaxima=' + App.hdFechaMaxMod.value/* +
        '&Filtros=' + App.hdFiltros.value+
        '&Grid=' + App.hdViewJson.value*/
    );
}
function ExportarNoColumnModel() {
    Cookies.set('DescargaInventarioFiltros', App.hdFiltros.value);
    Cookies.set('DescargaInventarioGrid', App.hdViewJson.value);
    ExportarDatos('InventarioCategoryViewVistaCategoria', '', App.grid,
        '\'\'&EmplazamientoID=' + App.hdEmplazamientoID.value + '&CategoriaID=' + App.hdCategoriaID.value + '&VistaPlantilla=' + App.hdVistaPlantilla.value + '&VistaInventario=' + App.hdVistaInventario.value +
        '&Operadores=' + App.hdOperador.value +
        '&Estados=' + App.hdEstado.value +
        '&Usuarios=' + App.hdUsuario.value +
        '&FechaCreacionMinima=' + App.hdFechaMinCrea.value +
        '&FechaCreacionMaxima=' + App.hdFechaMaxCrea.value +
        '&FechaModificacionMinima=' + App.hdFechaMinMod.value +
        '&FechaModificacionMaxima=' + App.hdFechaMaxMod.value /*+
        '&Filtros=' + App.hdFiltros.value+
        '&Grid=' + App.hdViewJson.value*/, 'EXPORTARNOCOLUMNMODEL'
    );
}

//#endregion


var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

function MontarGrid() {
    showLoadMask(App.grid, function (load) {
        TreeCore.MontarGridDinamico(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    }

                    GridColHandlerDinamicoV2(App.grid);
                    load.hide();
                }
            });

        CargarStoresSerie([App.storePrincipal], function Fin(fin) {
            if (fin) {
                load.hide();
            }
        });

    });
}

//#region Formulario Elemento
function btnCancelarFormElementos() {
    App.winGestion.close();
}
//#endregion

function sitesInfo(sender, registro, index) {
    if (sender != undefined) {
        parent.parent.CargarPanelMoreInfo(sender.$widgetRecord.data.EmplazamientoID, 'Sites');
    }
}


//#region Formulario Elemento


function winFormResize(obj) {

    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }

    obj.center();
}

function formResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(620);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }


}


window.addEventListener('resize', function () {
    var dv = document.querySelectorAll('div.winForm-resp');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResize(obj);
    }

    var frm = document.querySelectorAll('div.ctForm-resp');
    for (i = 0; i < frm.length; i++) {
        var obj = Ext.getCmp(frm[i].id);
        formResize(obj);
    }

});

//#endregion


function winFormResize(obj) {
    var resheight = window.innerHeight;
    var res = window.innerWidth;

    var winheight = Ext.getCmp('winGestion').getSize().height;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }

    if (resheight <= winheight) {
        obj.setHeight(resheight - 50);
    }

    obj.center();
    obj.update();

}

// INCIO MENU CLICK DERECHO

function ShowRightClickMenu(item, record, node, index, e) {

    var menu = App["ContextMenu"];

    e.preventDefault();
    menu.dataRecord = record.data;
    menu.showAt(getActualXY(menu, e));
}

function OpcionSeleccionada(sender, registro, event) {

    var opcion = registro.id;
    var invElementoID = sender.dataRecord.InventarioElementoID;
    var padre = parent.parent.parent;

    if (opcion.includes("ShowHistorial")) {

        var pagina = "/Modulos/Inventory/InventarioElementoHistoricos.aspx?InventarioElementoID=" + invElementoID;
        padre.addTab(padre.App.tabPpal, registro.text + invElementoID, registro.text + '-' + sender.dataRecord.NumeroInventario, pagina);
    }
    else if (opcion.includes("ShowDocuments")) {
        var pagina = "/Modulos/Files/DocumentosVista.aspx?ObjetoID=" + invElementoID + "&ObjetoTipo=InventarioElemento&ProyectoTipo=GLOBAL";
        parent.addTab(padre.App.tabPpal, registro.text + invElementoID, registro.text + " - " + sender.dataRecord.NumeroInventario, pagina);
    }

}

// FIN MENU CLICK DERECHO

//#region FILTROS

var idApp;

function AddGrid() {
    idApp = parent.parent.AddReferencias(App, App.hdCategoriaID.value);
}


function CargarFiltrosAplicados(sender) {
    if (sender.value != null && sender.value != '') {
        var filtros = JSON.parse(sender.value);
        filtros.forEach(filtro => {
            parent.parent.AñadirFiltro(filtro);
        });
    }
}

$(document).ready(function CargarGrids() {
    window.onbeforeunload = function (event) {
        parent.parent.RemoveReferencias(idApp);
    };
});

function AplicarFiltrosOperadores(tf, e) {
    var tree = App.grid,
        store = tree.store,
        logic = store,
        text = tf.value;

    logic.clearFilter();

    // This will ensure after clearing the filter the auto-expanded nodes will be collapsed again
    tree.collapseAll();

    if (Ext.isEmpty(text, false)) {
        return;
    }

    try {
        var re = new RegExp(".*" + text + ".*", "i");
    } catch (err) {
        return;
    }
    var valido;
    logic.filterBy(function (node) {
        valido = false;
        if (re.test(node.data.OperadorID)) {
            valido = true;
        }
        return valido;
    });
}

//#endregion

function ClearFilter(sender, aux, aux2) {
    if (sender.filters.length > 1) {
        sender.filters.items.forEach(filtro => {
            if (!(filtro.isGridFilter != undefined && filtro.isGridFilter)) {
                App.grid.clearFilters();
                sender.clearFilter();
                App.hdOperador.setValue(undefined);
                App.hdEstado.setValue(undefined);
                App.hdFechaMinCrea.setValue(undefined);
                App.hdFechaMaxCrea.setValue(undefined);
                App.hdFechaMinMod.setValue(undefined);
                App.hdFechaMaxMod.setValue(undefined);
                App.hdUsuario.setValue(undefined);
            }
        });
    }
}

//#region Fomulario

var pautasGeneracion;

function CargarFormulario(callback, recargar = true) {
    FormularioValidoInventario(App.pnConfigurador, true, "");

    if (recargar) {
        GenerarCodigoInventario(function GenerarCodigo(codigoGenerado) {
            if (codigoGenerado) {
                callback(true, null);
            }
            else {
                callback(true, null);
            }
        })
    } else {
        pautasGeneracion = null;
        callback(true, null);
    }
}

function GenerarCodigoInventario(callback) {
    TreeCore.GenerarCamposInventario(true, true, {
        success: function (result) {
            if (!result.Success) {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                callback(true, null);
            }
            else {
                pautasGeneracion = result.Result;
                GenerarCodigo(App.txtCodigoElemento, App.pnConfigurador, pautasGeneracion[0], App.hdCodigoAutogenerado);
                GenerarCodigo(App.txtNombreElemento, App.pnConfigurador, pautasGeneracion[1], App.hdNombreAutogenerado);
                callback(true, null);
            }
        }
    });
}

//#region Combos

function SeleccionarCategoria(sender) {
    showLoadMask(this.winGestion, function (load) {
        sender.getTrigger(0).show();
        App.hdCatID.setValue(sender.value);
        recargarCombos([App.cmbPlantilla]);
        try {
            TreeCore.PintarCategorias(true,
                {
                    success: function (result) {
                        try {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {
                                GenerarCodigo(App.txtCodigoElemento, App.pnConfigurador, pautasGeneracion[0], App.hdCodigoAutogenerado);
                                GenerarCodigo(App.txtNombreElemento, App.pnConfigurador, pautasGeneracion[1], App.hdNombreAutogenerado);
                                try {
                                    App.contenedorCategorias.updateLayout();
                                } catch (e) {
                                    console.log('error1');
                                }
                            }
                            load.hide();
                        } catch (e) {
                            console.log('error2');
                        }
                    }
                });
        } catch (e) {
            console.log('error3');
        }
    });
}

function RecargarCategoria(sender) {
    showLoadMask(this.winGestion, function (load) {
        recargarCombos([App.cmbCategoriaElemento], function Fin(fin) {
            if (fin) {
                App.hdCatID.setValue('');
                TreeCore.PintarCategorias(true,
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {
                                GenerarCodigo(App.txtCodigoElemento, App.pnConfigurador, pautasGeneracion[0], App.hdCodigoAutogenerado);
                                GenerarCodigo(App.txtNombreElemento, App.pnConfigurador, pautasGeneracion[1], App.hdNombreAutogenerado);
                            }
                            load.hide();
                        }
                    });
            }
        })
    });
}

function SeleccionarPlantilla(sender) {
    sender.getTrigger(0).show();
    showLoadMask(App.winGestion, function (load) {
        TreeCore.SeleccionarPlantilla(
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        for (var prop in result.Result) {
                            SeleccionarValorMostrarEditar(prop, result.Result[prop]);
                        }
                    }
                    load.hide();
                }
            });
    });
}

function RecargarPlantilla(sender) {
    recargarCombos([sender]);
}

//#endregion

//#region Botones Guardar/Cancelar

function FormularioValidoInventario(sender, valid, aux) {
    try {
        if (valid == true && App.hdVistaPlantilla != undefined && (App.hdVistaPlantilla.value != undefined && App.hdVistaPlantilla.value != "")) {
            App.btnGuardarAgregarEditar.setDisabled(false);
        }
        else {
            App.btnGuardarAgregarEditar.setDisabled(false);
            Ext.each(App.pnConfigurador.body.query('*'), function (item) {
                var c = Ext.getCmp(item.id);
                if (c && c.isFormField) {
                    if ((c.allowBlank == false && (c.rawValue == undefined || c.rawValue == "")) || !c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField]))) {
                        App.btnGuardarAgregarEditar.setDisabled(true);

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
                    if (!c.isValid() && (c.value == undefined || c.value == "")) {
                        c.clearInvalid();
                    }
                }
            });
        }
    } catch (e) {

    }
}

function FormularioValido(sender, valid, aux) {
    FormularioValidoInventario(sender, valid, aux);
}


let VarRegistroInventario;
let VarIndexInventario;
let VarSenderInventario;


function btnGuardarFormElementos(sender, registro, index) {

    VarRegistroInventario = registro;
    VarIndexInventario = index;
    VarSenderInventario = sender;

    TreeCore.GuardarValor(bAgregar, false,
        {
            success: function (result) {
                if (result.Result == 'Editado') {
                    Ext.Msg.alert(
                        {
                            title: jsControlEdicion,
                            msg: jsComprobarEdicionRegistro,
                            buttons: Ext.Msg.YESNO,
                            buttonText: {
                                no: jsRecargarFormulario,
                                yes: jsSobrescribir
                            },
                            fn: ajaxAgregarEditarInventarioEditado,
                            cls: 'winFormEditado',
                            width: '500px'
                        });
                } else if (result.Result == 'Codigo') {
                    Ext.Msg.alert(
                        {
                            title: jsControlCodigo,
                            msg: jsComprobarCodigoGenerado,
                            buttons: Ext.Msg.YESNO,
                            buttonText: {
                                no: jsCodigoManual,
                                yes: jsGenerarCodigo
                            },
                            fn: ajaxGenerarNuevoCodigo,
                            cls: 'winFormDuplicateCode',
                            width: '458px'


                        });
                } else if (result.Result == 'ErrorPageLoad') {
                    winErrorTimeout();
                }
                else if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    Refrescar();
                    App.winGestion.close();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function ajaxComprobarHdEditado() {
    if (ContadorIntentos == 0) {
        Ext.Msg.alert(
            {
                title: jsControlCodigo,
                msg: jsLimiteIntentosCodigo,
                buttons: Ext.Msg.OK,
                cls: 'winFormEditado',
                width: '500px'
            });
    }
    //else if (App.hdCodigoDuplicado.Value == "Duplicado" && ContadorIntentos > 0) {
    //    ajaxGenerarNuevoCodigo()
    //}
}

function ajaxGenerarNuevoCodigo(sender) {

    if (sender == "yes") {
        showLoadMask(App.winGestion, function (load) {

            TreeCore.GenerarCamposInventario(true, false, {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        pautasGeneracion = result.Result;
                        GenerarCodigoDuplicado(App.txtCodigoElemento, App.pnConfigurador, pautasGeneracion[0], App.hdCodigoAutogenerado);
                        ContadorIntentos = ContadorIntentos - 1;
                        TreeCore.ComprobarCodigoInventarioDuplicado(
                            {
                                success: function (result) {
                                    if (result.Result != null && result.Result != '') {
                                        App.txtCodigoElemento.setValue(App.hdCodigoAutogenerado.value.toString());
                                    }
                                },
                                eventMask:
                                {                                    
                                    showMask: true,
                                    msg: jsMensajeProcesando
                                }
                            });

                        ajaxComprobarHdEditado();
                    }

                    load.hide();
                 }
            });
        });
    }
    else {
        App.txtCodigoElemento.setValue("");
        App.txtCodigoElemento.setEmptyText("");
    }
}

function ajaxAgregarEditarInventarioEditado(sender, registro, index) {

    if (sender == "yes") {
        TreeCore.GuardarValor(bAgregar, true,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        if (result.Result == 'ErrorPageLoad') {
                            winErrorTimeout();
                        }
                        Refrescar();
                        App["winGestion"].close();
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    }
    else {
        MostrarEditar();
    }

}

function DisabledControl(sender) {
    sender.disable();
}
//#endregion

function RecargarComboInventario(sender, registro, index) {
    recargarCombos([sender]);
    if (pautasGeneracion != null) {
        GenerarCodigo(App.txtCodigoElemento, App.pnConfigurador, pautasGeneracion[0], App.hdCodigoAutogenerado);
        GenerarCodigo(App.txtNombreElemento, App.pnConfigurador, pautasGeneracion[1], App.hdNombreAutogenerado);
    }
}

function SeleccionarComboInventario(sender, registro, index) {
    sender.getTrigger(0).show();

    if (pautasGeneracion != null) {
        GenerarCodigo(App.txtCodigoElemento, App.pnConfigurador, pautasGeneracion[0], App.hdCodigoAutogenerado);
        GenerarCodigo(App.txtNombreElemento, App.pnConfigurador, pautasGeneracion[1], App.hdNombreAutogenerado);
    }

    FormularioValidoInventario(sender, true, undefined);
}

function LimpiarFormulario() {
    Ext.each(App.pnConfigurador.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && c.isFormField) {
            if (c.value != null || c.value != undefined) {
                c.reset();
            }
        }
    });
}

function PintarCategorias(sender) {

    if (App.hdCategoriaID.getValue() != "0") {

        showLoadMask(App.winGestion, function (load) {
            TreeCore.PintarCategorias(true,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        App.winGestion.updateLayout();
                        load.hide();
                    }
                });
        });
    }
}

var storesACargar = [];
function CargarStoresFormulario() {
    if (App.hdCategoriaActiva.value == 'NoActiva' || App.hdCategoriaActiva.value == '') {
        App.btnDescargarTodo.show();
        App.btnDescargarNoColumnModel.hide();
    } else {
        App.btnDescargarTodo.hide();
    }
    if (App.cmbViews.store.data.items.length == 0) {
        storesACargar.push(App.cmbEstado);
        storesACargar.push(App.cmbOperador);
        recargarCombos([App.cmbViews], function Fin(fin) {
            if (fin) {
                App.btnFiltros.enable();
                App.cmbViews.enable();
            }
        });
    } else {
        //App.cmbViews.store.reload();
        App.btnFiltros.enable();
        App.cmbViews.enable();
    }
}

//#endregion

function CargarFiltros() {
    var filtros = [];
    parent.parent.FiltrosAplicados.forEach(filtro => {
        if (filtro.InventarioCategoriaID == hdCategoriaID.value) {
            filtros.push(filtro);
        }
    });
    App.hdFiltros.setValue(JSON.stringify(filtros));

    if (parent.parent.App.cmbOperadores.value != null) {
        App.hdOperador.setValue(parent.parent.App.cmbOperadores.value.join(','));
    } else {
        App.hdOperador.setValue("");
    }
    if (parent.parent.App.cmbEstados.value != null) {
        App.hdEstado.setValue(parent.parent.App.cmbEstados.value.join(','));
    } else {
        App.hdEstado.setValue("");
    }
    if (parent.parent.App.datMinDateCrea.rawValue != "") {
        App.hdFechaMinCrea.setValue(getFormattedDate(parent.parent.App.datMinDateCrea.value));
    } else {
        App.hdFechaMinCrea.setValue("");
    }
    if (parent.parent.App.datMaxDateCrea.rawValue != "") {
        App.hdFechaMaxCrea.setValue(getFormattedDate(parent.parent.App.datMaxDateCrea.value));
    } else {
        App.hdFechaMaxCrea.setValue("");
    }
    if (parent.parent.App.datMinDateMod.rawValue != "") {
        App.hdFechaMinMod.setValue(getFormattedDate(parent.parent.App.datMinDateMod.value));
    } else {
        App.hdFechaMinMod.setValue("");
    }
    if (parent.parent.App.datMaxDateMod.rawValue != "") {
        App.hdFechaMaxMod.setValue(getFormattedDate(parent.parent.App.datMaxDateMod.value));
    } else {
        App.hdFechaMaxMod.setValue("");
    }
    if (parent.parent.App.cmbUsuarios.value != null) {
        App.hdUsuario.setValue(parent.parent.App.cmbUsuarios.value.join(','));
    } else {
        App.hdUsuario.setValue("");
    }

    function getFormattedDate(date) {
        var year = date.getFullYear();

        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;

        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;

        return day + '/' + month + '/' + year;
    }

}

function AplicarView(sender) {
    TreeCore.GenerarGrid(sender.value, false, false,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Refrescar();
                    App["winGestion"].close();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function SelectView(sender) {
    if (!parent.parent.App.pnAsideR.collapsed && !parent.parent.App.pnGridsAsideMyViews.hidden) {
        parent.parent.App.cmbViews.setValue(sender.value);
    } else {
        App.hdViewJson.setValue(sender.selection.data.JsonColumnas);
        if (parent.parent.App.btnFiltosActivos.pressed) {
            if (App.cmbViews.selection.data.JsonFiltros != "") {
                var json = JSON.parse(sender.selection.data.JsonFiltros);
                $.each(json, function (i, jsonAux) {
                    jsonAux["NombreCategoria"] = parent.parent.App.hdNombreCategoriaActiva.value;
                    parent.parent.AñadirFiltro(jsonAux);
                });
            }
        }
    }
}

function BorrarFiltrosCategoria() {
    parent.parent.BorrarFiltrosCategoria();
}

var dataGridEmplazamientos = [];

function CargarBuscadorPredictivo() {
    JsonEmplazamientos.forEach(emp => {
        dataGridEmplazamientos.push({
            key: emp.NombreSitio.toLowerCase(),
            key2: emp.Codigo.toLowerCase(),
            nombre: emp.NombreSitio,
            codigo: emp.Codigo,
            id: emp.EmplazamientoID
        });
    });

    dataGridEmplazamientos = dataGridEmplazamientos.sort(function (a, b) {
        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
    });

    var nameSearchBox = "txtFilterEmplazamientos";
    var selectorSearchBox = `#${nameSearchBox}-inputEl`;

    $(function () {
        let textBuscado = "";
        $(selectorSearchBox).autocomplete({
            source: function (request, response) {
                textBuscado = request.term;
                var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                let results = $.grep(dataGridEmplazamientos, function (value) {
                    let value1 = value.key;
                    let value2 = value.key2;

                    return matcher.test(value1) || matcher.test(normalize(value1)) || matcher.test(value2) || matcher.test(normalize(value2));
                });

                response(results.slice(0, 10));

                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                    var idEmplazamientoBuscador = $(e.currentTarget).attr("data-emplazamientoID");
                    App.hdStringBuscador.setValue("");
                    App.hdIDEmplazamientoBuscador.setValue(idEmplazamientoBuscador);
                    filterEmplazamientos();
                });
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            let title = boldQuery(item.nombre, textBuscado) + " - " + boldQuery(item.codigo, textBuscado);
            return $("<li>")
                .append(`<div class="document-item" data-emplazamientoID="${item.id}">` +
                    `<div class="item-Buscador">` +
                    `<div class="title">${title}</div>` +
                    `</div>` +
                    `<div class="description"></div>` +
                    "</div>")
                .appendTo(ul);
        };
    });
}

var JsonEmplazamientos, Accion, SelEmplazamientoID, objCheck;

function btnMoverElementos(sender) {
    showLoadMask(App.grid, function (load) {
        Accion = "Mover";
        App.winGestionElementos.setTitle(jsMover);
        App.gridEmplazamientos.setHeight(400);
        if (!JsonEmplazamientos) {
            CargarEmplazamientos(function Fin(fin) {
                if (fin) {
                    CargarBuscadorPredictivo();
                    App.storeEmplazamientos.add(JsonEmplazamientos);
                    App.storeEmplazamientos.data.removeAtKey(App.hdEmplazamientoID.value);
                    App.nbCopias.hide();
                    App.btnGuardarGestionElementos.hide();
                    App.winGestionElementos.show();
                    load.hide();
                }
            })
        } else {
            App.storeEmplazamientos.add(JsonEmplazamientos);
            App.storeEmplazamientos.data.removeAtKey(App.hdEmplazamientoID.value);
            App.nbCopias.hide();
            App.btnGuardarGestionElementos.hide();
            App.winGestionElementos.show();
            load.hide();
        }
    });
}

function btnClonarElementos(sender) {
    Accion = "Copiar";
    App.nbCopias.reset();
    App.winGestionElementos.setTitle(jsClonar);
    App.gridEmplazamientos.setHeight(300);
    if (!JsonEmplazamientos) {
        CargarEmplazamientos(function Fin(fin) {
            if (fin) {
                CargarBuscadorPredictivo();
                App.storeEmplazamientos.add(JsonEmplazamientos);
                App.nbCopias.show();
                App.btnGuardarGestionElementos.show();
                App.winGestionElementos.show();
                load.hide();
            }
        })
    } else {
        App.storeEmplazamientos.add(JsonEmplazamientos);
        App.nbCopias.show();
        App.btnGuardarGestionElementos.show();
        App.winGestionElementos.show();
        load.hide();
    }
}

function SelEmplazamiento(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.MoverElementos(SelEmplazamientoID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        Refrescar();
                        App.winGestionElementos.close();
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    } else {
        objCheck.checked = false;
    }
}

function CerrarSeleccionarEmplazamientos(sender) {
    sender.reset();
    filterEmplazamientos();
}

function CargarEmplazamientos(callback) {
    TreeCore.CargarEmplazamientos(
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    JsonEmplazamientos = JSON.parse(result.Result);
                    callback(true, null);
                }
            }
        });
}

function SeleccionarEmplazamiento(sender) {
    objCheck = sender;
    if (Accion == "Copiar") {
        App.storeEmplazamientos.getById(sender.getAttribute('EmplazamientoID')).data.Seleccionado = sender.checked;
        App.storeEmplazamientos.sort();
    } else if (Accion == "Mover") {
        SelEmplazamientoID = sender.getAttribute('EmplazamientoID');
        Ext.Msg.alert(
            {
                title: jsMover + ' ' + jsInventarioElemento,
                msg: jsConfirmacionMover,
                buttons: Ext.Msg.YESNO,
                fn: SelEmplazamiento,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function RenderCheckBox(sender, registro) {
    return '<input type="checkbox" ' + ((sender) ? 'checked' : '') + ' onchange="SeleccionarEmplazamiento(this)" class="chkDone" EmplazamientoID="' + registro.record.id + '"';
}

function GetEmplazamientosSeleccionados() {
    App.storeEmplazamientos.data.items.filter(function (element) {
        return element.data.Seleccionado;
    });
}

function filterEmplazamientos() {
    var logic = App.storeEmplazamientos,
        text = App.txtFilterEmplazamientos.getRawValue();

    logic.clearFilter();

    if (Ext.isEmpty(text, false)) {
        return;
    }

    // this will allow invalid regexp while composing, for example "(examples|grid|color)"
    try {
        var re = new RegExp(".*" + text + ".*", "i");
    } catch (err) {
        return;
    }

    if (App.hdIDEmplazamientoBuscador.value != '') {
        var EmplazamientosID = App.hdIDEmplazamientoBuscador.value;
        App.hdIDEmplazamientoBuscador.setValue('');
        logic.filterBy(function (node) {
            var valido = false;
            if (node.data.Seleccionado) {
                valido = true;
            }
            if (EmplazamientosID == node.id.toString()) {
                valido = true;
            }
            return valido;
        });
    } else {
        logic.filterBy(function (node) {
            var valido = false;
            if (node.data.Seleccionado) {
                valido = true;
            }
            if (re.test(node.data.Codigo)) {
                valido = true;
            }
            if (re.test(node.data.NombreSitio)) {
                valido = true;
            }
            return valido;
        });

    }


}

function btnGuardarGestionElementos() {

}

function ComprobarFormulario() {
    if (hdErrorCarga.value == 'true') {
        winErrorTimeout();
        App.winGestion.hide();
    }
}