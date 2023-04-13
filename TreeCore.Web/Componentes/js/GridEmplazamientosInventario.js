function DeseleccionarGrillaInventarioElementos (sender, registro, index) {
    var ruta = sender.config.storeId.split('_');
    ruta.pop();
    ruta = ruta.join('_');

    App[ruta + '_' + 'GridRowSelectInventarioElementos'].clearSelections();
}

function Grid_RowSelectInventarioElementos(sender, registro, index) {

}

function refrescarInventarioElementos(sender, registro, index) {
    ajaxAplicarFiltro(filtrosAplicados);
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

function FiltrarColumnasInventarioElementos(sender, registro) {

    var idComponente = sender.id.split('_');
    idComponente.pop();
    App[idComponente + "_btnDescargar"].disable();
    var tree = App[idComponente + "_grdInventarioElementosEmplazamientos"];
    var store = tree.store,
        logic = store,
        text = sender.getRawValue();

    logic.clearFilter();

    // This will ensure after clearing the filter the auto-expanded nodes will be collapsed again
    //tree.collapseAll();

    if (Ext.isEmpty(text, false)) {
        App[idComponente + "_btnDescargar"].enable();
        return;
    }

    filtroBuscador(logic, tree, text);
}


function LimpiarFiltroBusquedaInventarioElementos(sender, registro) {
    var idComponente = sender.id.split('_');
    idComponente.pop();

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App[idComponente + "_txtSearch"].setValue("");

    logic.clearFilter();
    App[idComponente + "_btnDescargar"].enable();
}


function BorrarFiltrosInventarioElementos(sender, registro) {
    var idComponente = sender.id.split('_');
    idComponente.pop();
    var tree = App[idComponente + "_grdInventarioElementosEmplazamientos"];
    tree.filters.clearFilters();
}

// #endregion

// #region Paginación
var nfPaginNumberFirstload = true;
var currentPage = 1;

function nfPaginNumberBeforeRenderInventario(sender, registro) {
    let ruta = getIdComponente(sender);
    if (nfPaginNumberFirstload) {

        nfPaginNumberFirstload = false;

        App[`${ruta}_nfPaginNumber`].setValue(currentPage);

        //updateDisplaying();
    }
}

function pagingInitInventario(sender) {
    let ruta = getIdComponente(sender);
    currentPage = 1;
    App[`${ruta}_storeInventarioElementosEmplazamientos`].loadPage(currentPage);
    updatePaginInventario();
}

function pagingPreInventario(sender) {
    let ruta = getIdComponente(sender);
    currentPage--;
    App[`${ruta}_storeInventarioElementosEmplazamientos`].loadPage(currentPage);
    updatePaginInventario();
}

function paginGoToInventario(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageInventario()) {
        let ruta = getIdComponente(sender);
        currentPage = sender.value;
        App[`${ruta}_storeInventarioElementosEmplazamientos`].loadPage(sender.value);
        updatePaginInventario();
    }
}

function paginNextInventario(sender) {
    let ruta = getIdComponente(sender);
    currentPage++;
    App[`${ruta}_storeInventarioElementosEmplazamientos`].loadPage(currentPage);
    updatePaginInventario();
}

function paginLastInventario(sender) {
    let ruta = getIdComponente(sender);
    currentPage = getLastPageInventario(sender);
    App[`${ruta}_storeInventarioElementosEmplazamientos`].loadPage(currentPage);
    updatePaginInventario();
}

function getLastPageInventario(sender) {
    let ruta;
    if (sender) {
        ruta = getIdComponente(sender);
    }
    else {
        ruta = "UCGridEmplazamientosInventario";
    }
    let total = 0;
    if (App[`${ruta}_hdTotalCountGrid`].value) {
        total = App[`${ruta}_hdTotalCountGrid`].value;
    } else {
        total = App[`${ruta}_storeInventarioElementosEmplazamientos`].getTotalCount();
    }

    let pageSize = App[`${ruta}_storeInventarioElementosEmplazamientos`].pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

function updatePaginInventario(sender) {
    let lastPage = getLastPageInventario(sender);
    let ruta;
    
    if (sender) {
        ruta = getIdComponente(sender);
    }
    else {
        ruta = "UCGridEmplazamientosInventario";
    }
    let total = 0;
    if (App[`${ruta}_hdTotalCountGrid`].value) {
        total = App[`${ruta}_hdTotalCountGrid`].value;
    } else {
        total = App[`${ruta}_storeInventarioElementosEmplazamientos`].getTotalCount();
    }

    if (total / lastPage < currentPage) {
        currentPage = 1;
    }

    App[`${ruta}_nfPaginNumber`].setValue(currentPage);
    App[`${ruta}_lbNumberPages`].setText(lastPage);

    if (currentPage == 1 && lastPage == 1) {
        App[`${ruta}_btnPaginNext`].setDisabled(true);
        App[`${ruta}_btnPaginLast`].setDisabled(true);
        App[`${ruta}_btnPagingInit`].setDisabled(true);
        App[`${ruta}_btnPagingPre`].setDisabled(true);
    }
    else if (currentPage <= 1) {
        App[`${ruta}_btnPagingInit`].setDisabled(true);
        App[`${ruta}_btnPagingPre`].setDisabled(true);
        App[`${ruta}_btnPaginNext`].setDisabled(false);
        App[`${ruta}_btnPaginLast`].setDisabled(false);
    }
    else if (currentPage >= lastPage) {
        App[`${ruta}_btnPaginNext`].setDisabled(true);
        App[`${ruta}_btnPaginLast`].setDisabled(true);
        App[`${ruta}_btnPagingInit`].setDisabled(false);
        App[`${ruta}_btnPagingPre`].setDisabled(false);
    }
    else {
        App[`${ruta}_btnPaginNext`].setDisabled(false);
        App[`${ruta}_btnPaginLast`].setDisabled(false);
        App[`${ruta}_btnPagingInit`].setDisabled(false);
        App[`${ruta}_btnPagingPre`].setDisabled(false);
    }

    updateDisplayingInventario(sender);
}

function updateDisplayingInventario(sender) {
    var txtDisplaying = "";
    var total = 0;
    let ruta;
    if (sender) {
        ruta = getIdComponente(sender);
    }
    else {
        ruta = "UCGridEmplazamientosInventario";
    }

    var pageSize = App[`${ruta}_storeInventarioElementosEmplazamientos`].pageSize;

    if (App[`${ruta}_hdTotalCountGrid`].value) {
        total = App[`${ruta}_hdTotalCountGrid`].value;
    } else {
        total = App[`${ruta}_storeInventarioElementosEmplazamientos`].getTotalCount();
    }

    if (total / pageSize < currentPage) {
        currentPage = 1;
    }

    let firstItem = ((currentPage - 1) * pageSize) + 1;
    let lastItem = currentPage * pageSize;

    if (currentPage == getLastPageInventario(sender)) {
        lastItem = total;
    }

    txtDisplaying = `${jsMostrando} ${firstItem} - ${lastItem} ${jsDe} ${total}`;
    App[`${ruta}_lbDisplaying`].setText(txtDisplaying);
}

function setNumMaxPageInventario(sender) {
    updateDisplayingInventario();
    updatePaginInventario();
}
// #endRegion

// #region Buscador
var dataGridEmplazamientos = [];
function ajaxGetDatosBuscadorInventario() {

    if (dataGridEmplazamientos.length == 0 || forzarCargaBuscadorPredictivo) {
        dataGridEmplazamientos = [];

        TreeCore["UCGridEmplazamientosInventario"].GetDatosBuscador({
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

                    var idComponente = "UCGridEmplazamientosInventario";
                    var nameSearchBox = "txtSearch";
                    var selectorSearchBox = `#${idComponente}_${nameSearchBox}-inputEl`;

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

// #endRegion

function CargarGridInventario(sender) {
    let ruta = getIdComponente(sender);
    TreeCore[ruta].CargarGrid();
}
