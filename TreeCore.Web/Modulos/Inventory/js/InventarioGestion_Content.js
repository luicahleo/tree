forzarCargaBuscadorPredictivo = true;
var seleccionado;

function CargarBuscadorPredictivo(sender, registro) {
    forzarCargaBuscadorPredictivo = true;
    BuscadorPredictivo(sender, registro);
}

function GethdEmplazamientoID() {
    //App.hdEmplazamientoID.setValue(parent.App.hdEmplazamientoID.getValue());
}
function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;
    if (datos != null) {
        if (datos.id == 'root') {
            App.btnAnadir.enable();
            App.btnEliminar.disable();
        } else {
            App.btnEliminar.enable();
            App.btnAnadir.enable();
            //parent.MostrarInfoElemento(datos.InventarioElementoID, false);
        }

        seleccionado = datos;
        App.btnAnadir.setTooltip(jsAgregar);
        parent.CargarPanelMoreInfo(registro.data.InventarioElementoID, false);

    }
}

function DeseleccionarGrilla() {
    App.btnAnadir.enable();
    App.GridRowSelect.clearSelections();
    App.btnEliminar.disable();
}



// INCIO MENU CLICK DERECHO

function ShowRightClickMenu(item, record, node, index, e) {

    let bHideMenu = document.getElementById("hdHideMenuClick").value;
    if (bHideMenu == "false") {

        var menu = App["ContextMenu"];
        e.preventDefault();
        menu.dataRecord = record.data;
        menu.showAt(getActualXY(menu, e));
    }
}

function OpcionSeleccionada(sender, registro, event) {

    var opcion = registro.id;
    var id = sender.dataRecord.id;

    if (opcion.includes("ShowHistorial")) {
        var pagina = "../ModInventario/pages/InventarioElementoHistoricos.aspx?InventarioElementoID=" + id;
        parent.parent.addTab(parent.parent.App.tabPpal, registro.text + id, registro.text + '-' + sender.dataRecord.NumeroInventario, pagina);
    }

    if (opcion.includes("ShowDocuments")) {
        var pagina = "../PaginasComunes/DocumentosVista.aspx?ObjetoID=" + id + "&ObjetoTipo=InventarioElemento&ProyectoTipo=GLOBAL";
        parent.parent.addTab(parent.parent.App.tabPpal, registro.text + id, registro.text + "-" + sender.dataRecord.NumeroInventario, pagina);
    }
}

// FIN MENU CLICK DERECHO

//#region GESTION GRID

//#region FILTROS

function BuscarElemento(sender, registro) {
    var valido = false;
    if (registro.getKey() == Ext.EventObject.ENTER) {
        for (var cat in App.storeElementos.data.items) {
            if (App.storeElementos.data.items[cat].data.NumeroInventario == sender.value) {
                LimpiarRuta();
                App.hdElementoPadre.setValue(App.storeElementos.data.items[cat].data.InventarioElementoID);
                App.storePrincipal.reload();
                App.lbRutaElemento.show();
                App.btnCarpetaActual.show();
                App.lbRutaElemento.setText(App.storeElementos.data.items[cat].data.NumeroInventario);
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

function AbrirElePadres(column, cmp, record) {
    cmp.menu.items.clear();
    try {
        document.getElementById(cmp.menu.id + "-targetEl").innerHTML = '';
    } catch (e) {

    }
    var jsonPadres = JSON.parse(record.data.Padres);
    for (var item in jsonPadres) {
        cmp.menu.add(new Ext.menu.TextItem({ text: jsonPadres[item].NumeroInventario, InventarioElementoID: jsonPadres[item].InventarioElementoID, IDUnico: GenerarID() }));
    }
    cmp.setText('' + cmp.menu.items.items.length + '');
    if (cmp.menu.items.items.length > 0) {
        cmp.show();
    } else {
        cmp.hide();
    }
}

//#endregion

//#region FORMULARIO

var Agregar;

function VaciarFormulario(callback) {

    recargarCombos([App.cmbElementos], function Fin(fin) {
        if (fin) {
            App.formGestion.getForm().reset();
            if (callback != null) {
                callback(true, null);
            }
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
    VaciarFormulario(function callback(fin) {
        if (fin) {
            App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
            Agregar = true;
            App.winGestion.show();
        }
    });
}

function MostrarEditar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        //VaciarFormulario();
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
        App.hdElementoPadre.setValue(App.GridRowSelect.selected.items[0].data.InventarioElementoID);
        App.storePrincipal.reload();
        App.lbRutaElemento.show();
        App.btnCarpetaActual.show();
        App.lbRutaElemento.setText(App.GridRowSelect.selected.items[0].data.NumeroInventario);
        listaRuta.push({ nombre: App.GridRowSelect.selected.items[0].data.NumeroInventario, invID: App.GridRowSelect.selected.items[0].data.InventarioElementoID, idUnico: GenerarID() });
        App.menuRuta.items.clear();
        GenerarPadres();
        GenerarRuta();
    }
}

function LimpiarRuta() {
    App.btnMenuRuta.hide();
    App.btnRaizCarpeta.hide();
    App.lbRutaElemento.hide();
    App.btnCarpetaActual.hide();
    App.menuRuta.items.clear();
    listaRuta = [];
    App.hdElementoPadre.setValue(0);
}

function GenerarPadres() {
    try {
        document.getElementById('menuPadreEleActual-targetEl').innerHTML = '';
    } catch (e) {

    }
    App.menuPadreEleActual.items.clear();
    TreeCore.GetPadres(App.hdElementoPadre.value,
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    var jsonPadres = JSON.parse(result.Result);
                    for (var item in jsonPadres) {
                        App.menuPadreEleActual.add(new Ext.menu.TextItem({ text: jsonPadres[item].NumeroInventario, InventarioElementoID: jsonPadres[item].InventarioElementoID, IDUnico: GenerarID() }));
                    }
                    if (App.menuPadreEleActual.items.items.length > 1) {
                        App.btnPadreEleActucal.show();
                    } else {
                        App.btnPadreEleActucal.hide();
                    }
                }
            }
        });
    App.btnPadreEleActucal.show();
}

function GenerarRuta() {
    App.btnMenuRuta.show();
    App.btnRaizCarpeta.show();
    try {
        document.getElementById('menuRuta-targetEl').innerHTML = '';
    } catch (e) {

    }
    for (var item in listaRuta) {
        App.menuRuta.add(new Ext.menu.TextItem({ text: listaRuta[item].nombre, InventarioElementoID: listaRuta[item].invID, IDUnico: listaRuta[item].idUnico }))
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
    App.hdElementoPadre.setValue(select.InventarioElementoID);
    App.storePrincipal.reload();
    App.lbRutaElemento.show();
    App.btnCarpetaActual.show();
    App.lbRutaElemento.setText(select.text);
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
    App.btnPadreEleActucal.hide();
    App.storePrincipal.reload();
}

function SeleccionarPadre(sender, select) {
    LimpiarRuta();
    App.hdElementoPadre.setValue(select.InventarioElementoID);
    App.storePrincipal.reload();
    App.lbRutaElemento.show();
    App.btnCarpetaActual.show();
    App.lbRutaElemento.setText(select.text);
    GenerarPadres();
}

function VolverAtras(sender) {
    if (listaRuta.length >= 2) {
        var ElePadre = listaRuta[listaRuta.length - 2];
        listaRuta.pop();
        App.hdElementoPadre.setValue(ElePadre.invID);
        App.storePrincipal.reload();
        App.lbRutaElemento.show();
        App.lbRutaElemento.setText(ElePadre.nombre);
        App.menuRuta.items.clear();
        GenerarPadres();
        GenerarRuta();
    } else {
        IrRutaRaiz();
    }
}

//#endregion

function VerMas(sender, registro, index) {
    if (sender.$widgetRecord.data.InventarioElementoID != '0') {
        parent.CargarPanelMoreInfo(sender.$widgetRecord.data.InventarioElementoID, true);
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

//#region FILTROS

var idApp;

function AddGrid() {
    idApp = parent.AddReferencias(App);
}

$(document).ready(function CargarGrids() {
    window.onbeforeunload = function (event) {
        parent.RemoveReferencias(idApp);
    };
});

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