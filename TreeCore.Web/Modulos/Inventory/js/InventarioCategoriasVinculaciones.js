
function CargarBuscadorPredictivo(sender, registro) {
    forzarCargaBuscadorPredictivo = true;
    BuscadorPredictivo(sender, registro);
}

function DeseleccionarGrilla() {
    App.btnAnadir.enable();
    App.GridRowSelect.clearSelections();
    App.btnEditar.disable();
    App.btnActivar.disable();
    App.btnEliminar.disable();
}

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnActivar.enable();

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

//#region GESTION GRID

function RecargarTipoEmplazamientos() {
    recargarCombos([App.cmbTipoEmplazamientos]);
    App.lbRutaEmplazamientoTipo.setText(jsComun);
    IrRutaRaiz();
    App.storePrincipal.reload();
    App.storeCategorias.reload();
}

function SeleccionarTipoEmplazamientos(sender) {
    App.cmbTipoEmplazamientos.getTrigger(0).show();
    IrRutaRaiz();
    App.lbRutaEmplazamientoTipo.setText(sender.selection.data.Tipo);
    App.storePrincipal.reload();
    App.storeCategorias.reload();
}

//#region FILTROS

function BuscarCategoria(sender, registro) {
    var valido = false;
    if (registro.getKey() == Ext.EventObject.ENTER) {
        for (var cat in App.storeCategorias.data.items) {
            if (App.storeCategorias.data.items[cat].data.InventarioCategoria == sender.value) {
                LimpiarRuta();
                App.hdCategoriaPadre.setValue(App.storeCategorias.data.items[cat].data.InventarioCategoriaID);
                App.storePrincipal.reload();
                App.lbRutaCategoria.show();
                App.lbRutaCategoria.setText(App.storeCategorias.data.items[cat].data.InventarioCategoria);
                for (var i = 0; i < listaRuta.length; i++) {
                    if (listaRuta[i].idUnico == select.IDUnico) {
                        listaRuta = listaRuta.splice(0, ++i);
                    }
                }
                App.menuRuta.items.clear();
                GenerarPadres();
                GenerarRuta();
                valido = true;
                sender.clear();
            }
        }
        if (!valido) {
            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsNoResultados, buttons: Ext.Msg.OK });
        }
    }
}

function LimpiarBusqueda(sender) {
    sender.clear();
}

// #endregion

function AbrirCatPadres(column, cmp, record) {
    cmp.menu.items.clear();
    try {
        document.getElementById(cmp.menu.id + "-targetEl").innerHTML = '';
    } catch (e) {

    }
    var jsonPadres = JSON.parse(record.data.Padres);
    for (var item in jsonPadres) {
        cmp.menu.add(new Ext.menu.TextItem({ text: jsonPadres[item].InventarioCategoria, InventarioCategoriaID: jsonPadres[item].InventarioCategoriaID, IDUnico: GenerarID() }));
    }
    cmp.setText('' + cmp.menu.items.items.length + '');
    if (cmp.menu.items.items.length > 0) {
        cmp.show();
    } else {
        cmp.hide();
    }
}


function AbrirTiposVinculaciones(column, cmp, record) {
    cmp.menu.items.clear();
    try {
        document.getElementById(cmp.menu.id + "-targetEl").innerHTML = '';
    } catch (e) {

    }
    var jsonPadres = JSON.parse(record.data.Vinculaciones);
    for (var item in jsonPadres) {
        cmp.menu.add(new Ext.menu.TextItem({ text: jsonPadres[item].Nombre, InventarioTipoVinculacionID: jsonPadres[item].InventarioTipoVinculacionID, IDUnico: GenerarID() }));
    }
    cmp.setText('' + cmp.menu.items.items.length + '');
}

//#endregion

//#region FORMULARIO

var Agregar;

function VaciarFormulario() {

    recargarCombos([App.cmbCategorias, App.cmbTipoVinculaciones], function Fin(fin) {
        if (fin) {
            App.formGestion.getForm().reset();
        }
    });
}

function winGestionBotonGuardar() {
    TreeCore.AgregarEditarVinculacion(Agregar,
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

function FormularioValido(valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }

}

//#endregion

//#region DIRECT METHOD

function AgregarEditar() {
    VaciarFormulario();
    App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
    App.cmbCategorias.enable();
    Agregar = true;
    App.winGestion.show();
}

function MostrarEditar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        //VaciarFormulario();
        App.cmbCategorias.disable();
        Agregar = false;
        App.winGestion.setTitle(jsEditar + " " + jsTituloModulo);

        recargarCombos([App.cmbCategorias, App.cmbTipoVinculaciones], function Fin(fin) {
            if (fin) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            App.winGestion.show();
                        },
                        eventMask:
                        {
                            showMask: true,
                            msg: jsMensajeProcesando
                        }
                    });
            }
        });
    }
}

function Activar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        TreeCore.ActivarVinculacion(
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
function Eliminar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsTituloModulo,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarVinculacion,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarVinculacion(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarVinculacion(
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
}
function VerActivos() {
    if (App.colActivo.hidden) {
        App.colActivo.show();
    } else {
        App.colActivo.hide();
    }
    Refrescar();
}

//#endregion

//#region GRID

var listaRuta = [];

function AccederVinculacion(sender, registro) {
    if (App.GridRowSelect.selected.items[0] != undefined) {
        App.hdCategoriaPadre.setValue(App.GridRowSelect.selected.items[0].data.InventarioCategoriaID);
        App.storePrincipal.reload();
        App.lbRutaCategoria.show();
        App.btnCarpetaActual.show();
        App.lbRutaCategoria.setText(App.GridRowSelect.selected.items[0].data.InventarioCategoria);
        listaRuta.push({ nombre: App.GridRowSelect.selected.items[0].data.InventarioCategoria, invID: App.GridRowSelect.selected.items[0].data.InventarioCategoriaID, idUnico: GenerarID() });
        App.menuRuta.items.clear();
        GenerarPadres();
        GenerarRuta();
    }
}

function LimpiarRuta() {
    App.btnMenuRuta.hide();
    App.btnRaizCarpeta.hide();
    App.lbRutaCategoria.hide();
    App.btnCarpetaActual.hide();
    App.menuRuta.items.clear();
    listaRuta = [];
    App.hdCategoriaPadre.setValue(0);
}

function GenerarPadres() {
    try {
        document.getElementById('menuPadreCatActual-targetEl').innerHTML = '';
    } catch (e) {

    }
    App.menuPadreCatActual.items.clear();
    TreeCore.GetPadres(
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    var jsonPadres = JSON.parse(result.Result);
                    for (var item in jsonPadres) {
                        App.menuPadreCatActual.add(new Ext.menu.TextItem({ text: jsonPadres[item].InventarioCategoria, InventarioCategoriaID: jsonPadres[item].InventarioCategoriaID, IDUnico: GenerarID() }));
                    }
                    if (App.menuPadreCatActual.items.items.length > 1) {
                        App.btnPadreCatActucal.show();
                    } else {
                        App.btnPadreCatActucal.hide();
                    }
                }
            }
        });
    App.btnPadreCatActucal.show();
}

function GenerarRuta() {
    App.btnMenuRuta.show();
    App.btnRaizCarpeta.show();
    try {
        document.getElementById('menuRuta-targetEl').innerHTML = '';
    } catch (e) {

    }
    for (var item in listaRuta) {
        App.menuRuta.add(new Ext.menu.TextItem({ text: listaRuta[item].nombre, InventarioCategoriaID: listaRuta[item].invID, IDUnico: listaRuta[item].idUnico }))
    }
    if (App.menuRuta.items.items.length > 1) {
        App.menuRuta.items.last().hide();
    } else {
        App.btnMenuRuta.hide();
        App.btnRaizCarpeta.hide();
    }
}
function CortarRuta() {

}

function SeleccionarRuta(sender, select) {
    App.hdCategoriaPadre.setValue(select.InventarioCategoriaID);
    App.storePrincipal.reload();
    App.lbRutaCategoria.show();
    App.btnCarpetaActual.show();
    App.lbRutaCategoria.setText(select.text);
    for (var i = 0; i < listaRuta.length; i++) {
        if (listaRuta[i].idUnico == select.IDUnico) {
            listaRuta = listaRuta.splice(0, ++i);
        }
    }
    App.menuRuta.items.clear();
    GenerarPadres();
    GenerarRuta();
}

function GenerarID() {
    return '_' + Math.random().toString(36).substr(2, 9);
}
function IrRutaRaiz() {
    LimpiarRuta();
    App.btnPadreCatActucal.hide();
    App.storePrincipal.reload();
}

function SeleccionarPadre(sender, select) {
    LimpiarRuta();
    App.hdCategoriaPadre.setValue(select.InventarioCategoriaID);
    App.storePrincipal.reload();
    App.lbRutaCategoria.show();
    App.btnCarpetaActual.show();
    App.lbRutaCategoria.setText(select.text);
    GenerarPadres();
}

function VolverAtras(sender) {
    if (listaRuta.length >= 2) {
        var ElePadre = listaRuta[listaRuta.length - 2];
        listaRuta.pop();
        App.hdCategoriaPadre.setValue(ElePadre.invID);
        App.storePrincipal.reload();
        App.lbRutaCategoria.show();
        App.lbRutaCategoria.setText(ElePadre.nombre);
        App.menuRuta.items.clear();
        GenerarPadres();
        GenerarRuta();
    } else {
        IrRutaRaiz();
    }
}

//#endregion

//#region CLIENTES

function CargarStores() {
    App.storeTipoEmplazamientos.reload();
    App.storePrincipal.reload();
    App.storeCategorias.reload();
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

//#endregion

