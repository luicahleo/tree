var bindParams = function () {
    App.hdCliID.setValue(parent.document.getElementById('hdCliID').value);
    App.hdStringBuscador.setValue(parent.document.getElementById('hdStringBuscador').value);
    App.hdIDEmplazamientoBuscador.setValue(parent.document.getElementById('hdIDEmplazamientoBuscador').value);
    App.hdFiltrosAplicados.setValue(parent.document.getElementById('hdFiltrosAplicados').value);

    App.hdidsResultados.setValue(parent.document.getElementById('hdidsResultados').value);
    App.hdnameIndiceID.setValue(parent.document.getElementById('hdnameIndiceID').value);
}

function DeseleccionarGrillaInventarioElementos(sender, registro, index) {
    App.GridRowSelectInventarioElementos.clearSelections();
}

function Grid_RowSelectInventarioElementos(sender, registro) {
    parent.CargarPanelMoreInfoElemento(registro.data.EmplazamientoID, registro.data.InventarioElementoID, false);
}


function refrescarInventarioElementos() {
    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App.txtSearch.setValue("");
    LimpiarFiltroBusquedaInventarioElementos();
}

var pageSelectInventario = function (item) {
    let store = this.up('gridpanel').store;

    var curPageSize = store.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        store.pageSize = wantedPageSize;
        store.load({
            callback: function (r, options, success) {
                nfPaginNumberFirstload = true;
                updatePaginInventario();
            }
        });
    }

}

//#region FILTROS

function updateFiltrosAplicados() {
    console.log("Filtro actualizado")
    App.hdFiltrosAplicados.setValue(parent.document.getElementById('hdFiltrosAplicados').value);
    App.grdInventarioElementosEmplazamientos.store.reload();
}

function FiltrarColumnasInventario(sender, registro) {

    App.btnDescargar.disable();
    var tree = App.grdInventarioElementosEmplazamientos;
    var store = tree.store,
        logic = store,
        text = sender.getRawValue();

    logic.clearFilter();

    // This will ensure after clearing the filter the auto-expanded nodes will be collapsed again
    //tree.collapseAll();

    if (Ext.isEmpty(text, false)) {
        App.btnDescargar.enable();
        return;
    }

    filtroBuscador(logic, tree, text);
}


function LimpiarFiltroBusquedaInventarioElementos(sender, registro) {

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App.txtSearch.setValue("");

    App.storeInventarioElementosEmplazamientos.clearFilter();
    App.storeInventarioElementosEmplazamientos.reload();
    pagingInitInventario();
    App.btnDescargar.enable();
}


function BorrarFiltrosInventarioElementos(sender, registro) {
    var tree = App.grdInventarioElementosEmplazamientos;
    tree.filters.clearFilters();
}

// #endregion

// #region Paginación
var nfPaginNumberFirstload = true;
var currentPage = 1;

function filtrarEmplazamientosPorBuscador(sender, registro, index) {
    if (registro.charCode == 13) {
        App.hdStringBuscador.setValue(sender.value);
        App.storeInventarioElementosEmplazamientos.reload();
    }
}

function nfPaginNumberBeforeRenderInventario(sender, registro) {
    if (nfPaginNumberFirstload) {

        nfPaginNumberFirstload = false;

        App.nfPaginNumber.setValue(currentPage);

    }
}

function pagingInitInventario() {
    currentPage = 1;
    App.storeInventarioElementosEmplazamientos.loadPage(currentPage);
    updatePaginInventario();
}

function pagingPreInventario() {
    currentPage--;
    App.storeInventarioElementosEmplazamientos.loadPage(currentPage);
    updatePaginInventario();
}

function paginGoToInventario(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageInventario()) {
        currentPage = sender.value;
        App.storeInventarioElementosEmplazamientos.loadPage(sender.value);
        updatePaginInventario();
    }
}

function paginNextInventario() {
    currentPage++;
    App.storeInventarioElementosEmplazamientos.loadPage(currentPage);
    updatePaginInventario();
}

function paginLastInventario() {

    currentPage = getLastPageInventario();
    App.storeInventarioElementosEmplazamientos.loadPage(currentPage);
    updatePaginInventario();
}

function getLastPageInventario() {
    let total = 0;
    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeInventarioElementosEmplazamientos.getTotalCount();
    }

    let pageSize = App.storeInventarioElementosEmplazamientos.pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

function updatePaginInventario() {
    let lastPage = getLastPageInventario();
    let total = 0;
    var pageSize = App.storeInventarioElementosEmplazamientos.pageSize;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeInventarioElementosEmplazamientos.getTotalCount();
    }

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    if (pageSize < currentPage) {
        currentPage = 1;
        App.storeInventarioElementosEmplazamientos.loadPage(currentPage);
    }

    App.nfPaginNumber.setValue(`${currentPage}`);
    App.lbNumberPages.setText(`${lastPage}`);

    if (currentPage == 1 && lastPage == 1) {
        App.btnPaginNext.setDisabled(true);
        App.btnPaginLast.setDisabled(true);
        App.btnPagingInit.setDisabled(true);
        App.btnPagingPre.setDisabled(true);
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

    updateDisplayingInventario();
}

function updateDisplayingInventario() {
    var txtDisplaying = "";
    var total = 0;
    let lastPage = getLastPageInventario();
    var pageSize = App.storeInventarioElementosEmplazamientos.pageSize;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeInventarioElementosEmplazamientos.getTotalCount();
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

    if (currentPage == getLastPageInventario()) {
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

function setNumMaxPageInventario() {
    updateDisplayingInventario();
    updatePaginInventario();
}
// #endRegion

// #region Buscador
var dataGridEmplazamientos = [];
function ajaxGetDatosBuscadorInventario() {

    if (dataGridEmplazamientos.length == 0 || forzarCargaBuscadorPredictivo) {
        dataGridEmplazamientos = [];

        TreeCore.GetDatosBuscador({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    result.Result.forEach(emp => {

                        Object.entries(emp).forEach(p => {
                            if (p[0] != "EmplazamientoID" && p[0] != "Codigo" && p[0] != "NombreSitio") {
                                if (p[1] != "" && p[1] != null && !dataGridEmplazamientos.some(a => a.id === emp.EmplazamientoID)) {
                                    dataGridEmplazamientos.push({
                                        key: emp.NombreEmplazamiento.toLowerCase(),
                                        key2: emp.Codigo.toLowerCase(),
                                        key3: emp.NombreElemento.toLowerCase(),
                                        key4: emp.NumeroInventario.toLowerCase(),
                                        nombre: emp.NombreEmplazamiento,
                                        codigo: emp.Codigo,
                                        nombreElemento: emp.NombreElemento,
                                        numeroInventario: emp.NumeroInventario,
                                        id: emp.EmplazamientoID
                                    });


                                }
                            }
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
                                    let value3 = value.key3;
                                    let value4 = value.key4;

                                    return (matcher.test(value1) || matcher.test(normalize(value1)) ||
                                        matcher.test(value2) || matcher.test(normalize(value2)) ||
                                        matcher.test(value3) || matcher.test(normalize(value3)) ||
                                        matcher.test(value4) || matcher.test(normalize(value4)));
                                });

                                response(results.slice(0, 10));

                                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                                    var idEmplazamientoBuscador = $(e.currentTarget).attr("data-emplazamientoID");
                                    App.hdStringBuscador.setValue("");
                                    App.hdIDEmplazamientoBuscador.setValue(idEmplazamientoBuscador);
                                    App.storeInventarioElementosEmplazamientos.reload();

                                });
                            }
                        }).autocomplete("instance")._renderItem = function (ul, item) {
                            let title = boldQuery(item.nombre, textBuscado) + " - " + boldQuery(item.codigo, textBuscado);
                            return $("<li>")
                                .append(`<div class="document-item" data-emplazamientoID="${item.id}">` +
                                    `<div class="item-Buscador">` +
                                    `<div class="title">${title}</div>` +
                                    `</div>` +
                                    `<div class="description">${boldQuery(item.nombreElemento, textBuscado)}</div>` +
                                    `<div class="description">${boldQuery(item.numeroInventario, textBuscado)}</div>` +
                                    "</div>")
                                .appendTo(ul);
                        };
                    });
                }
            }
        });
    }
}

function MostrarPanelMoreInfo(sender) {
    parent.CargarPanelMoreInfoElemento(sender.$widgetRecord.data.EmplazamientoID, sender.$widgetRecord.data.InventarioElementoID, true);
}

// #endRegion

function CargarGridInventario(sender) {
    //TreeCore.CargarGrid();
}

