function DeseleccionarGrillaLocalizaciones(sender, registro, index) {

    var ruta = sender.config.storeId.split('_');
    ruta.pop();
    ruta = ruta.join('_');

    App[ruta + '_' + 'GridRowSelectLocalizaciones'].clearSelections();
}

function Grid_RowSelectLocalizaciones() { }


function refrescarLocalizaciones() {
    ajaxAplicarFiltro(filtrosAplicados);
}

var pageSelectLocalizaciones = function (item) {
    let store = this.up('gridpanel').store;

    var curPageSize = store.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        store.pageSize = wantedPageSize;
        store.load({
            callback: function (r, options, success) {
                nfPaginNumberFirstload = true;
                updatePaginLocalizaciones();
            }
        });
    }

}

//#region FILTROS

function FiltrarColumnasLocalizaciones(sender, registro) {

    var idComponente = sender.id.split('_');
    idComponente.pop();
    App[idComponente + "_btnDescargar"].disable();
    var tree = App[idComponente + "_gridEmplazamientosLocalizaciones"];
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


function LimpiarFiltroBusquedaLocalizaciones(sender, registro) {

    var idComponente = sender.id.split('_');
    idComponente.pop();

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App[idComponente + "_txtSearch"].setValue("");

    logic.clearFilter();
    App[idComponente + "_btnDescargar"].enable();
}


function BorrarFiltrosLozalizaciones(sender, registro) {
    var idComponente = sender.id.split('_');
    idComponente.pop();
    var tree = App[idComponente + "_gridEmplazamientosLocalizaciones"];
    tree.filters.clearFilters();
}

// #endregion

// #region Paginación
var nfPaginNumberFirstload = true;
var currentPage = 1;

function nfPaginNumberBeforeRenderLocalizaciones(sender, registro) {
    if (nfPaginNumberFirstload) {

        nfPaginNumberFirstload = false;

        App.UCGridEmplazamientosLocalizaciones_nfPaginNumber.setValue(currentPage);

        //updateDisplaying();
    }
}

function pagingInitLocalizaciones() {
    currentPage = 1;
    App.UCGridEmplazamientosLocalizaciones_storeEmplazamientosLocalizaciones.loadPage(currentPage);
    updatePaginLocalizaciones();
}

function pagingPreLocalizaciones() {
    currentPage--;
    App.UCGridEmplazamientosLocalizaciones_storeEmplazamientosLocalizaciones.loadPage(currentPage);
    updatePaginLocalizaciones();
}

function paginGoToLocalizaciones(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageLocalizaciones()) {
        let ruta = getIdComponente(sender);
        currentPage = sender.value;
        App[`${ruta}_storeEmplazamientosLocalizaciones`].loadPage(sender.value);
        updatePaginLocalizaciones();
    }
}

function paginNextLocalizaciones() {
    currentPage++;
    App.UCGridEmplazamientosLocalizaciones_storeEmplazamientosLocalizaciones.loadPage(currentPage);
    updatePaginLocalizaciones();
}

function paginLastLocalizaciones() {

    currentPage = getLastPageLocalizaciones();
    App.UCGridEmplazamientosLocalizaciones_storeEmplazamientosLocalizaciones.loadPage(currentPage);
    updatePaginLocalizaciones();
}

function getLastPageLocalizaciones() {
    let total = 0;
    if (App.UCGridEmplazamientosLocalizaciones_hdTotalCountGrid.value) {
        total = App.UCGridEmplazamientosLocalizaciones_hdTotalCountGrid.value;
    } else {
        total = App.UCGridEmplazamientosLocalizaciones_storeEmplazamientosLocalizaciones.getTotalCount();
    }

    let pageSize = App.UCGridEmplazamientosLocalizaciones_storeEmplazamientosLocalizaciones.pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

function updatePaginLocalizaciones() {
    let lastPage = getLastPageLocalizaciones();
    let total = 0;

    if (App.UCGridEmplazamientosLocalizaciones_hdTotalCountGrid.value) {
        total = App.UCGridEmplazamientosLocalizaciones_hdTotalCountGrid.value;
    } else {
        total = App.UCGridEmplazamientosLocalizaciones_storeEmplazamientosLocalizaciones.getTotalCount();
    }

    if (total / lastPage < currentPage) {
        currentPage = 1;
    }

    App.UCGridEmplazamientosLocalizaciones_nfPaginNumber.setValue(currentPage);
    App.UCGridEmplazamientosLocalizaciones_lbNumberPages.setText(lastPage);

    if (currentPage == 1 && lastPage == 1) {
        App.UCGridEmplazamientosLocalizaciones_btnPaginNext.setDisabled(true);
        App.UCGridEmplazamientosLocalizaciones_btnPaginLast.setDisabled(true);
        App.UCGridEmplazamientosLocalizaciones_btnPagingInit.setDisabled(true);
        App.UCGridEmplazamientosLocalizaciones_btnPagingPre.setDisabled(true);
    }
    else if (currentPage <= 1) {
        App.UCGridEmplazamientosLocalizaciones_btnPagingInit.setDisabled(true);
        App.UCGridEmplazamientosLocalizaciones_btnPagingPre.setDisabled(true);
        App.UCGridEmplazamientosLocalizaciones_btnPaginNext.setDisabled(false);
        App.UCGridEmplazamientosLocalizaciones_btnPaginLast.setDisabled(false);
    }
    else if (currentPage >= lastPage) {
        App.UCGridEmplazamientosLocalizaciones_btnPaginNext.setDisabled(true);
        App.UCGridEmplazamientosLocalizaciones_btnPaginLast.setDisabled(true);
        App.UCGridEmplazamientosLocalizaciones_btnPagingInit.setDisabled(false);
        App.UCGridEmplazamientosLocalizaciones_btnPagingPre.setDisabled(false);
    } else {
        App.UCGridEmplazamientosLocalizaciones_btnPaginNext.setDisabled(false);
        App.UCGridEmplazamientosLocalizaciones_btnPaginLast.setDisabled(false);
        App.UCGridEmplazamientosLocalizaciones_btnPagingInit.setDisabled(false);
        App.UCGridEmplazamientosLocalizaciones_btnPagingPre.setDisabled(false);
    }

    updateDisplayingLocalizaciones();
}

function updateDisplayingLocalizaciones() {
    var txtDisplaying = "";
    var total = 0;
    let lastPage = getLastPageLocalizaciones();
    var pageSize = App.UCGridEmplazamientosLocalizaciones_storeEmplazamientosLocalizaciones.pageSize;

    if (App.UCGridEmplazamientosLocalizaciones_hdTotalCountGrid.value) {
        total = App.UCGridEmplazamientosLocalizaciones_hdTotalCountGrid.value;
    } else {
        total = App.UCGridEmplazamientosLocalizaciones_storeEmplazamientosLocalizaciones.getTotalCount();
    }

    if (total / lastPage < currentPage) {
        currentPage = 1;
    }

    let firstItem = ((currentPage - 1) * pageSize) + 1;
    let lastItem = currentPage * pageSize;

    if (currentPage == getLastPageLocalizaciones()) {
        lastItem = total;
    }

    txtDisplaying = `${jsMostrando} ${firstItem} - ${lastItem} ${jsDe} ${total}`;
    App.UCGridEmplazamientosLocalizaciones_lbDisplaying.setText(txtDisplaying);
}

function setNumMaxPageLocalizaciones() {
    updateDisplayingLocalizaciones();
    updatePaginLocalizaciones();
}
// #endRegion

// #region Buscador
var dataGridEmplazamientos = [];
function ajaxGetDatosBuscadorLocalizaciones() {

    if (dataGridEmplazamientos.length == 0 || forzarCargaBuscadorPredictivo) {
        dataGridEmplazamientos = [];

        TreeCore["UCGridEmplazamientosLocalizaciones"].GetDatosBuscador({
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
                                        key: emp.NombreSitio.toLowerCase(),
                                        key2: emp.Codigo.toLowerCase(),
                                        key3: emp.Direccion.toLowerCase(),
                                        key4: emp.CodigoPostal.toLowerCase(),
                                        nombre: emp.NombreSitio,
                                        codigo: emp.Codigo,
                                        direccion: emp.Direccion,
                                        codigoPostal: emp.CodigoPostal,
                                        id: emp.EmplazamientoID
                                    });


                                }
                            }
                        });
                    });

                    dataGridEmplazamientos = dataGridEmplazamientos.sort(function (a, b) {
                        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
                    });

                    var idComponente = "UCGridEmplazamientosLocalizaciones";
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
                                            `<div class="description">${boldQuery(item.codigoPostal, textBuscado)}</div>` +
                                            `<div class="description">${boldQuery(item.direccion, textBuscado)}</div>` +
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

function CargarGridLocalizaciones(sender) {
    let ruta = getIdComponente(sender);
    TreeCore[ruta].CargarGrid();
}
