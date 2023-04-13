var mapForm = [];

var bindParams = function () {
    App.hdCliID.setValue(parent.document.getElementById('hdCliID').value);
    //App.hdClienteID.setValue(parent.document.getElementById('hdCliID').value);

    App.hdStringBuscador.setValue(parent.document.getElementById('hdStringBuscador').value);
    App.hdIDEmplazamientoBuscador.setValue(parent.document.getElementById('hdIDEmplazamientoBuscador').value);
    App.hdFiltrosAplicados.setValue(parent.document.getElementById('hdFiltrosAplicados').value);

    App.hdidsResultados.setValue(parent.document.getElementById('hdidsResultados').value);
    App.hdnameIndiceID.setValue(parent.document.getElementById('hdnameIndiceID').value);

    App.storeEmplazamientos.reload();

    TreeCore.LoadPrefijos();
}

function actualizar() {
    nfPaginNumberFirstload = true;
    currentPage = 1;
    LimpiarFiltroBusqueda();
    App.storeEmplazamientos.reload();
}

// #region Contactos

function agregarContactoEmplazamiento(sender, registro, index) {

    var empID = App.GridRowSelect.selected.items[0].data.EmplazamientoID;
    App.hdEmplazamientoSeleccionado.setValue(empID);
    App.winGestionContactoEmplazamiento.setTitle(jsAgregar);
    App.winGestionContactoEmplazamiento.show();

    VaciarFormularioContacto('formAgregarEditarContactoEmplazamiento');
    cambiarATapContacto('formAgregarEditarContactoEmplazamiento', 0);
    CargarVentanaContactos(sender, 'formAgregarEditarContactoEmplazamiento', true, function Fin(fin) { });

}

function editarContactoEmplazamiento(sender, registro, index) {

    App.winGestionContactoEmplazamiento.setTitle(jsEditar);
    App.winGestionContactoEmplazamiento.show();

    showLoadMask(App.winGestionContactoEmplazamiento, function (load) {

        VaciarFormularioContacto('formAgregarEditarContactoEmplazamiento');
        DefinirDatosContacto('formAgregarEditarContactoEmplazamiento', sender.$widgetRecord.data);
        cambiarATapContacto('formAgregarEditarContactoEmplazamiento', 0);

        TreeCore.MostrarEditarContacto(sender.$widgetRecord.data.ContactoGlobalID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        CargarVentanaContactos(sender, 'formAgregarEditarContactoEmplazamiento', true);
                        load.hide();
                    }
                }
            });

    });
}

function Grid_RowSelectContactos(sender, registro, index) {

}

function closeWindowContactoEmplazamiento(sender, registro, index) {
    App.winFormContacto.hide();
}

var SetToggleValue = function (column, cmp, record) {

    var lEmpSelecID = App.GridRowSelect.selected.items[0].data.EmplazamientoID;

    var lEmpID = record.get('EmplazamientoID');

    if (lEmpSelecID == lEmpID) {
        cmp.setPressed(true);
    }
    else {
        cmp.setPressed(false);
    }

    cmp.updateLayout();

}

var ContactoID, EmplazamientosID, Toogle;

function ajaxCambiarAsignacion(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.AsignarEmplazamiento(ContactoID, EmplazamientosID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        App.storeContactosGlobalesEmplazamientosVinculados.reload();
                    }
                },
                eventMask:
                {
                    showMask: true,
                }
            });
    } else {
        Toogle.setPressed(true);
    }
}


function cambiarAsignacion(sender, registro, index) {
    ContactoID = sender.record.get('ContactoGlobalID');
    EmplazamientosID = sender.record.get('EmplazamientoID');
    Toogle = sender;
    if (!sender.pressed) {
        Ext.Msg.alert(
            {
                title: jsDesactivar + ' ' + jsContacto,
                msg: jsMensajeDesactivar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxCambiarAsignacion,
                icon: Ext.MessageBox.QUESTION
            });
    } else {
        ajaxCambiarAsignacion('yes');
    }
}

function buscador(sender, registro, index) {
    App.storeContactosGlobalesEmplazamientosVinculados.reload();
}


// #endregion

// #region Botón Derecho 

function ShowRightClickMenu(item, record, node, index, e) {

    var menu = App.ContextMenu;

    e.preventDefault();
    menu.dataRecord = record.data;
    menu.showAt(getActualXY(menu, e));
}

function OpcionSeleccionada(sender, registro, event) {

    var emplazamientoID = sender.dataRecord.EmplazamientoID;
    var emplazamiento = sender.dataRecord.Codigo;

    if (registro.id == "ShowMap") {
        parent.parent.addTab(parent.parent.App.tabPpal, registro.text + emplazamientoID, registro.text + '-' + App.GridRowSelect.selected.items[0].data.Codigo, "/Modulos/Sites/MapaEmplazamientosCercanos.aspx?EmplazamientoID=" + emplazamientoID)
    }

    if (registro.id == "ShowInventory") {
        parent.parent.addTab(parent.parent.App.tabPpal, registro.text + emplazamientoID, registro.text + '-' + App.GridRowSelect.selected.items[0].data.Codigo, "/Modulos/Inventory/InventarioGestionContenedor.aspx?EmplazamientoID=" + emplazamientoID);
    }

    if (registro.id == "ShowHistorial") {
        parent.parent.addTab(parent.parent.App.tabPpal, registro.text + emplazamientoID, registro.text + '-' + App.GridRowSelect.selected.items[0].data.Codigo, "/Modulos/Sites/EmplazamientosHistoricos.aspx?EmplazamientoID=" + emplazamientoID);
    }

    if (registro.id == "ShowDocuments") {
        var pagina = "/Modulos/Files/DocumentosVista.aspx?ObjetoID=" + emplazamientoID + "&ObjetoTipo=Emplazamiento&ProyectoTipo=GLOBAL";
        parent.parent.addTab(parent.parent.App.tabPpal, registro.text + emplazamientoID, registro.text + " " + emplazamiento, pagina);
    }

    if (registro.id == "AddContract") {
        var pagina = "/Modulos/Contracts/FormContratos.aspx?SiteCode=" + emplazamiento;
        parent.parent.addTab(parent.parent.App.tabPpal, registro.text + emplazamientoID, registro.text + " " + emplazamiento, pagina);
    }
}

// #endregion

//#region FILTROS

function FiltrarColumnas(sender, registro) {

    var tree = App.gridEmplazamientos;
    var store = tree.store,
        logic = store,
        text = sender.getRawValue();

    logic.clearFilter();

    if (Ext.isEmpty(text, false)) {
        App.mnuDwldExcel.enable();
        return;
    }

    filtroBuscador(logic, tree, text);
}

var pageSelect = function (item) {

    let store = this.up('gridpanel').store;

    var curPageSize = store.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        store.pageSize = wantedPageSize;
        store.load({
            callback: function (r, options, success) {
                nfPaginNumberFirstload = true;
                updatePaginEmplazamientos();
            }
        });
    }
}

function Grid_RowSelectEmplazamiento(sender, registro, index) {

    if (sender.selected.items.length == 1) {
        App.btnEditar.enable();
        App.btnContactos.enable();
        if (parent != null) {
            App.btnEditar.setTooltip(parent.jsEditar);
            App.btnContactos.setTooltip(parent.jsContacto);
        }
        App.hdEmplazamientoSeleccionado.setValue(registro.data.EmplazamientoID);
    } else {
        App.btnEditar.disable();
        App.btnContactos.disable();
    }
    parent.CargarPanelMoreInfo(registro.data.EmplazamientoID, false);
}

function DeseleccionarGrilla(sender, registro, index) {

    if (sender.selected.items.length == 1) {
        App.btnEditar.enable();
        App.btnContactos.enable();
        App.hdEmplazamientoSeleccionado.setValue(registro.data.EmplazamientoID);
    }
    else {

        if (App.hdCliID.value == 0 ||
            App.hdCliID.value == undefined) {
            App.btnAnadir.disable();
        } else {
            App.btnAnadir.enable();
        }

        App.btnEditar.disable();
        App.btnContactos.disable();

        if (sender.selected.items.length == 0) {
            App.GridRowSelect.clearSelections();
            App.hdEmplazamientoSeleccionado.setValue("");
        }
    }
}

// #endregion


// #region FILTROS

function updateFiltrosAplicados() {
    //console.log("Filtro actualizado")
    App.hdFiltrosAplicados.setValue(parent.document.getElementById('hdFiltrosAplicados').value);
    App.storeEmplazamientos.reload();
}

var nfPaginNumberFirstload = true;
var currentPage = 1;

function filtrarEmplazamientosPorBuscador(sender, registro, index) {
    if (registro.charCode == 13) {
        App.hdStringBuscador.setValue(sender.value);
        App.storeEmplazamientos.reload();
    }
}

function BorrarFiltrosEmplazamientos(sender, registro) {
    var tree = App.gridEmplazamientos;
    tree.filters.clearFilters();
}

function LimpiarFiltroBusqueda(sender, registro) {

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App.txtSearch.setValue("");

    App.storeEmplazamientos.clearFilter();
    App.storeEmplazamientos.reload();
    //App._mnuDwldExcel.enable();

}

//function hidePnFilters() {
//    let pn = parent.document.getElementById('pnAsideR');
//    //let btn = document.getElementById('btnCollapseAsR');
//    if (stPn == 0) {
//        pn.style.marginRight = '-360px';
//        //btn.style.transform = 'rotate(-180deg)';
//        stPn = 1;
//    }
//    else {
//        pn.style.marginRight = '0';
//        //btn.style.transform = 'rotate(0deg)';
//        stPn = 0;
//    }
//}

function MostrarPanelMoreInfo(sender) {
    parent.CargarPanelMoreInfo(sender.$widgetRecord.data.EmplazamientoID, true);
}

function nfPaginNumberBeforeRenderEmplazamientos(sender, registro) {
    if (nfPaginNumberFirstload) {

        nfPaginNumberFirstload = false;

        App.nfPaginNumber.setValue(currentPage);
    }
}

function pagingInitEmplazamientos() {
    currentPage = 1;
    App.storeEmplazamientos.loadPage(currentPage);
    updatePaginEmplazamientos();
}

function pagingPreEmplazamientos() {
    currentPage--;
    App.storeEmplazamientos.loadPage(currentPage);
    updatePaginEmplazamientos();
}

function paginGoToEmplazamientos(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageEmplazamientos()) {
        currentPage = sender.value;
        App.storeEmplazamientos.loadPage(sender.value);
        updatePaginEmplazamientos();
    }
}

function paginNextEmplazamientos() {
    currentPage++;
    App.storeEmplazamientos.loadPage(currentPage);
    updatePaginEmplazamientos();
}

function paginLastEmplazamientos() {

    currentPage = getLastPageEmplazamientos();
    App.storeEmplazamientos.loadPage(currentPage);
    updatePaginEmplazamientos();
}

function getLastPageEmplazamientos() {
    let total = 0;
    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeEmplazamientos.getTotalCount();
    }

    let pageSize = App.storeEmplazamientos.pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

function updatePaginEmplazamientos() {
    let lastPage = getLastPageEmplazamientos();
    let total = 0;
    var pageSize = App.storeEmplazamientos.pageSize;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeEmplazamientos.getTotalCount();
    }

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    if (pageSize < currentPage) {
        currentPage = 1;
        App.storeEmplazamientos.loadPage(currentPage);
    }

    App.nfPaginNumber.setValue(`${currentPage}`);
    App.lbNumberPages.setText(`${lastPage}`);

    if (currentPage == 1 && lastPage == 1) {
        App.btnPagingInit.setDisabled(true);
        App.btnPagingPre.setDisabled(true);
        App.btnPaginNext.setDisabled(true);
        App.btnPaginLast.setDisabled(true);
    }
    else if (currentPage <= 1) {
        App.btnPagingInit.setDisabled(true);
        App.btnPagingPre.setDisabled(true);
        App.btnPaginNext.setDisabled(false);
        App.btnPaginLast.setDisabled(false);
    }
    else if (currentPage >= lastPage) {
        App.btnPaginNext.setDisabled(true);
        App.btnPaginLast.setDisabled(true);
        App.btnPagingInit.setDisabled(false);
        App.btnPagingPre.setDisabled(false);
    } else {
        App.btnPaginNext.setDisabled(false);
        App.btnPaginLast.setDisabled(false);
        App.btnPagingInit.setDisabled(false);
        App.btnPagingPre.setDisabled(false);
    }

    updateDisplayingEmplazamientos();
}

function updateDisplayingEmplazamientos() {
    var txtDisplaying = "";
    var total = 0;
    let lastPage = getLastPageEmplazamientos();
    var pageSize = App.storeEmplazamientos.pageSize;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeEmplazamientos.getTotalCount();
    }

    lastPage = total / pageSize;
    if (lastPage % 1 != 0) {
        lastPage = Math.trunc(lastPage);
        lastPage++;
    }

    if (lastPage < currentPage) {
        currentPage = 1;
    }

    let firstItem = ((currentPage - 1) * pageSize) + 1;
    let lastItem = currentPage * pageSize;

    if (currentPage == getLastPageEmplazamientos()) {
        lastItem = total;
    }

    if (total == 0) {
        txtDisplaying = jsSinDatosMostrar;
    }
    else {
        txtDisplaying = `${jsMostrando} ${firstItem} - ${lastItem} ${jsDe} ${total}`;
    }

    App.lbDisplaying.setText(txtDisplaying);
}

function setNumMaxPageEmplazamientos() {
    updateDisplayingEmplazamientos();
    updatePaginEmplazamientos();
}

// #endregion

// #region Buscador
var dataGridEmplazamientos = [];

function ajaxGetDatosBuscadorEmplazamientos() {

    updatePaginEmplazamientos()

    if (dataGridEmplazamientos.length == 0 || forzarCargaBuscadorPredictivo) {
        dataGridEmplazamientos = [];

        TreeCore.GetDatosBuscador({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    result.Result.forEach(emp => {
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

                    var nameSearchBox = "txtSearch";
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
                                    App.storeEmplazamientos.reload();

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
            }
        });
    }
}

// #endregion

function agregar(sender, registro, inde) {
    parent.Agregar(App.storeEmplazamientos);
}

function editar() {
    parent.Editar(App.storeEmplazamientos, App.gridEmplazamientos.selection.id);
}
function limpiar(sender) {
    sender.reset();
}
