var bindParams = function () {
    App.hdCliID.setValue(parent.document.getElementById('hdCliID').value);
    App.hdStringBuscador.setValue(parent.document.getElementById('hdStringBuscador').value);
    App.hdIDEmplazamientoBuscador.setValue(parent.document.getElementById('hdIDEmplazamientoBuscador').value);
    App.hdFiltrosAplicados.setValue(parent.document.getElementById('hdFiltrosAplicados').value);

    App.hdidsResultados.setValue(parent.document.getElementById('hdidsResultados').value);
    App.hdnameIndiceID.setValue(parent.document.getElementById('hdnameIndiceID').value);
}

function DeseleccionarGrillaDocumentos(sender, registro, index) {

    App.GridRowSelectDocumentos.clearSelections();
}

function Grid_RowSelectDocumentos(sender, registro) {
    parent.CargarPanelMoreInfoDocumento(registro.data.EmplazamientoID, registro.data.DocumentoID, false);
}


function refrescarDocumentos() {
    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App.txtSearch.setValue("");
    LimpiarFiltroBusquedaDocumentos();
    App.storeDocumentosEmplazamientos.reload();
}

var pageSelectDocumentos = function (item) {
    let store = this.up('gridpanel').store;

    var curPageSize = store.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        store.pageSize = wantedPageSize;
        store.load({
            callback: function (r, options, success) {
                nfPaginNumberFirstload = true;
                updatePaginDocumentos();
            }
        });
    }

}

function hidePnFilters() {
    //let btn = document.getElementById('btnCollapseAsR');
    if (!parent.App.pnAsideR.collapsed) {
        //btn.style.transform = 'rotate(-180deg)';
        parent.App.pnAsideR.setCollapsed(true);
        App.btnFiltros.setPressed(false);
        stPn = 1;
    }
    else {
        //btn.style.transform = 'rotate(0deg)';
        parent.App.pnAsideR.setCollapsed(false);
        App.btnFiltros.setPressed(true);
        stPn = 0;
    }
}

//#region FILTROS

function updateFiltrosAplicados() {
    console.log("Filtro actualizado")
    App.hdFiltrosAplicados.setValue(parent.document.getElementById('hdFiltrosAplicados').value);
    App.grdDocumentosEmplazamientos.store.reload();
}

function FiltrarColumnasDocumentos(sender, registro) {

    App.btnDescargar.disable();
    var tree = App.grdDocumentosEmplazamientos;
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


function LimpiarFiltroBusquedaDocumentos(sender, registro) {

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App.txtSearch.setValue("");

    App.storeDocumentosEmplazamientos.clearFilter();
    App.storeDocumentosEmplazamientos.reload();
    pagingInitDocumentos();
    App.btnDescargar.enable();
}


function BorrarFiltrosDocumentos(sender, registro) {
    var tree = App.grdDocumentosEmplazamientos;
    tree.filters.clearFilters();
}

// #endregion

// #region Paginación
var nfPaginNumberFirstload = true;
var currentPage = 1;

function filtrarEmplazamientosPorBuscador(sender, registro, index) {
    if (registro.charCode == 13) {
        App.hdStringBuscador.setValue(sender.value);
        App.storeDocumentosEmplazamientos.reload();
    }
}

function LimpiarFiltroBusqueda(sender, registro) {

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App.txtSearch.setValue("");

    logic.clearFilter();
    App._mnuDwldExcel.enable();

}


function nfPaginNumberBeforeRenderDocumentos(sender, registro) {
    if (nfPaginNumberFirstload) {

        nfPaginNumberFirstload = false;

        App.nfPaginNumber.setValue(currentPage);

    }
}

function pagingInitDocumentos() {
    currentPage = 1;
    App.storeDocumentosEmplazamientos.loadPage(currentPage);
    updatePaginDocumentos();
}

function pagingPreDocumentos() {
    currentPage--;
    App.storeDocumentosEmplazamientos.loadPage(currentPage);
    updatePaginDocumentos();
}

function paginGoToDocumentos(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageDocumentos()) {
        currentPage = sender.value;
        App.storeDocumentosEmplazamientos.loadPage(sender.value);
        updatePaginDocumentos();
    }
}

function paginNextDocumentos() {
    currentPage++;
    App.storeDocumentosEmplazamientos.loadPage(currentPage);
    updatePaginDocumentos();
}

function paginLastDocumentos() {

    currentPage = getLastPageDocumentos();
    App.storeDocumentosEmplazamientos.loadPage(currentPage);
    updatePaginDocumentos();
}

function getLastPageDocumentos() {
    let total = 0;
    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeDocumentosEmplazamientos.getTotalCount();
    }

    let pageSize = App.storeDocumentosEmplazamientos.pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

function updatePaginDocumentos() {
    let lastPage = getLastPageDocumentos();
    let total = 0;
    var pageSize = App.storeDocumentosEmplazamientos.pageSize;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeDocumentosEmplazamientos.getTotalCount();
    }

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    if (pageSize < currentPage) {
        currentPage = 1;
        App.storeDocumentosEmplazamientos.loadPage(currentPage);
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

    updateDisplayingDocumentos();
}

function updateDisplayingDocumentos() {
    var txtDisplaying = "";
    var total = 0;
    let lastPage = getLastPageDocumentos();
    var pageSize = App.storeDocumentosEmplazamientos.pageSize;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeDocumentosEmplazamientos.getTotalCount();
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

    if (currentPage == getLastPageDocumentos()) {
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

function setNumMaxPageDocumentos() {
    updateDisplayingDocumentos();
    updatePaginDocumentos();
}
// #endRegion

// #region Buscador
var dataGridEmplazamientos = [];
function ajaxGetDatosBuscadorDocumentos() {

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
                            key3: emp.Documento.toLowerCase(),
                            nombre: emp.NombreSitio,
                            codigo: emp.Codigo,
                            documento: emp.Documento,
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
                                    let value3 = value.key3;

                                    return (matcher.test(value1) || matcher.test(normalize(value1)) ||
                                        matcher.test(value2) || matcher.test(normalize(value2)) ||
                                        matcher.test(value3) || matcher.test(normalize(value3)));
                                });

                                response(results.slice(0, 10));

                                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                                    var idEmplazamientoBuscador = $(e.currentTarget).attr("data-emplazamientoID");
                                    App.hdStringBuscador.setValue("");
                                    App.hdIDEmplazamientoBuscador.setValue(idEmplazamientoBuscador);
                                    App.storeDocumentosEmplazamientos.reload();

                                });
                            }
                        }).autocomplete("instance")._renderItem = function (ul, item) {
                            let title = boldQuery(item.nombre, textBuscado) + " - " + boldQuery(item.codigo, textBuscado);
                            return $("<li>")
                                .append(`<div class="document-item" data-emplazamientoID="${item.id}">` +
                                    `<div class="item-Buscador">` +
                                    `<div class="title">${title}</div>` +
                                    `</div>` +
                                    `<div class="description">${boldQuery(item.documento, textBuscado)}</div>` +
                                    "</div>")
                                .appendTo(ul);
                        };
                    });
                }
            }
        });
    }
}

// #endRegion

function CargarGridDocumentos(sender) {
    TreeCore.CargarGrid();
}

function MostrarPanelMoreInfo(sender) {
    parent.CargarPanelMoreInfoDocumento(sender.$widgetRecord.data.EmplazamientoID, sender.$widgetRecord.data.DocumentoID, true);
}