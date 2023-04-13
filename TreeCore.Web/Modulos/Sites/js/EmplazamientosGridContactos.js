var bindParams = function () {
    App.hdCliID.setValue(parent.document.getElementById('hdCliID').value);
    App.hdStringBuscador.setValue(parent.document.getElementById('hdStringBuscador').value);
    App.hdIDEmplazamientoBuscador.setValue(parent.document.getElementById('hdIDEmplazamientoBuscador').value);
    App.hdFiltrosAplicados.setValue(parent.document.getElementById('hdFiltrosAplicados').value);

    App.hdidsResultados.setValue(parent.document.getElementById('hdidsResultados').value);
    App.hdnameIndiceID.setValue(parent.document.getElementById('hdnameIndiceID').value);
}

function DeseleccionarGrillaContactos(sender, registro, index) {

    App.GridRowSelectContacto.clearSelections();
}

function Grid_RowSelectContactos(sender, registro) {
    parent.CargarPanelMoreInfoContacto(registro.data.EmplazamientoID, registro.data.ContactoGlobalID, false);
}


function refrescarContactos() {
    nfPaginNumberFirstload = true;
    currentPage = 1;
    LimpiarFiltroBusquedaContactos();
    App.storeContactosGlobalesEmplazamientos.reload();
}

var pageSelectContactos = function (item) {
    let store = this.up('gridpanel').store;

    var curPageSize = store.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        store.pageSize = wantedPageSize;
        store.load({
            callback: function (r, options, success) {
                nfPaginNumberFirstload = true;
                updatePaginContactos();
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
    App.hdFiltrosAplicados.setValue(parent.document.getElementById('hdFiltrosAplicados').value);
    App.grdContactos.store.reload();
}

function FiltrarColumnasContactos(sender, registro) {

    App.btnDescargar.disable();
    var tree = App.grdContactos;
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


function LimpiarFiltroBusquedaContactos(sender, registro) {

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App.txtSearch.setValue("");

    App.storeContactosGlobalesEmplazamientos.clearFilter();
    App.storeContactosGlobalesEmplazamientos.reload();
    pagingInitContactos();
    App.btnDescargar.enable();
}


function BorrarFiltrosLozalizaciones(sender, registro) {
    var tree = App.grdContactos;
    tree.filters.clearFilters();
}

// #endregion

// #region Paginación
var nfPaginNumberFirstload = true;
var currentPage = 1;

function filtrarEmplazamientosPorBuscador(sender, registro, index) {
    if (registro.charCode == 13) {
        App.hdStringBuscador.setValue(sender.value);
        App.storeContactosGlobalesEmplazamientos.reload();
    }
}


function nfPaginNumberBeforeRenderContactos(sender, registro) {
    if (nfPaginNumberFirstload) {

        nfPaginNumberFirstload = false;

        App.nfPaginNumber.setValue(currentPage);

    }
}

function pagingInitContactos() {
    currentPage = 1;
    App.storeContactosGlobalesEmplazamientos.loadPage(currentPage);
    updatePaginContactos();
}

function pagingPreContactos() {
    currentPage--;
    App.storeContactosGlobalesEmplazamientos.loadPage(currentPage);
    updatePaginContactos();
}

function paginGoToContactos(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageContactos()) {
        currentPage = sender.value;
        App.storeContactosGlobalesEmplazamientos.loadPage(sender.value);
        updatePaginContactos();
    }
}

function paginNextContactos() {
    currentPage++;
    App.storeContactosGlobalesEmplazamientos.loadPage(currentPage);
    updatePaginContactos();
}

function paginLastContactos() {

    currentPage = getLastPageContactos();
    App.storeContactosGlobalesEmplazamientos.loadPage(currentPage);
    updatePaginContactos();
}

function getLastPageContactos() {
    let total = 0;
    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeContactosGlobalesEmplazamientos.getTotalCount();
    }

    let pageSize = App.storeContactosGlobalesEmplazamientos.pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

function updatePaginContactos() {
    let lastPage = getLastPageContactos();
    let total = 0;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeContactosGlobalesEmplazamientos.getTotalCount();
    }

    if (total / lastPage < currentPage) {
        currentPage = 1;
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

    updateDisplayingContactos();
}

function updateDisplayingContactos() {
    var txtDisplaying = "";
    var total = 0;
    let lastPage = getLastPageContactos();
    var pageSize = App.storeContactosGlobalesEmplazamientos.pageSize;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeContactosGlobalesEmplazamientos.getTotalCount();
    }

    if (total / lastPage < currentPage) {
        currentPage = 1;
    }

    let firstItem = ((currentPage - 1) * pageSize) + 1;
    let lastItem = currentPage * pageSize;

    if (currentPage == getLastPageContactos()) {
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

function setNumMaxPageContactos() {
    updateDisplayingContactos();
    updatePaginContactos();
}
// #endregion

// #region Buscador
var dataGridEmplazamientos = [];
function ajaxGetDatosBuscadorContactos() {

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
                                        key: emp.NombreSitio.toLowerCase(),
                                        key2: emp.Codigo.toLowerCase(),
                                        key3: emp.Nombre.toLowerCase(),
                                        key4: emp.Apellidos.toLowerCase(),
                                        key5: emp.Email.toLowerCase(),
                                        key6: emp.Direccion.toLowerCase(),
                                        key7: emp.CP.toLowerCase(),
                                        key8: emp.Telefono.toLowerCase(),
                                        key9: emp.Telefono2.toLowerCase(),
                                        nombre: emp.NombreSitio,
                                        codigo: emp.Codigo,
                                        nombreContacto: emp.Nombre,
                                        apellidos: emp.Apellidos,
                                        email: emp.Email,
                                        direccion: emp.Direccion,
                                        cp: emp.CP,
                                        telefono: emp.Telefono.toLowerCase(),
                                        telefono2: emp.Telefono2.toLowerCase(),
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
                                    let value5 = value.key5;
                                    let value6 = value.key6;
                                    let value7 = value.key7;
                                    let value8 = value.key8;
                                    let value9 = value.key9;

                                    return (matcher.test(value1) || matcher.test(normalize(value1)) ||
                                        matcher.test(value2) || matcher.test(normalize(value2)) ||
                                        matcher.test(value3) || matcher.test(normalize(value3)) ||
                                        matcher.test(value4) || matcher.test(normalize(value4)) ||
                                        matcher.test(value5) || matcher.test(normalize(value5)) ||
                                        matcher.test(value6) || matcher.test(normalize(value6)) ||
                                        matcher.test(value7) || matcher.test(normalize(value7)) ||
                                        matcher.test(value8) || matcher.test(normalize(value8)) ||
                                        matcher.test(value9) || matcher.test(normalize(value9)));
                                });

                                response(results.slice(0, 10));

                                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                                    var idEmplazamientoBuscador = $(e.currentTarget).attr("data-emplazamientoID");
                                    App.hdStringBuscador.setValue("");
                                    App.hdIDEmplazamientoBuscador.setValue(idEmplazamientoBuscador);
                                    App.storeContactosGlobalesEmplazamientos.reload();

                                });
                            }
                        }).autocomplete("instance")._renderItem = function (ul, item) {
                            let title = boldQuery(item.nombre, textBuscado) + " - " + boldQuery(item.codigo, textBuscado);
                            return $("<li>")
                                .append(`<div class="document-item" data-emplazamientoID="${item.id}">` +
                                    `<div class="item-Buscador">` +
                                    `<div class="title">${title}</div>` +
                                    `</div>` +
                                    `<div class="description">${boldQuery(item.nombreContacto, textBuscado)} ${boldQuery(item.apellidos, textBuscado)}</div>` +
                                    `<div class="description">${boldQuery(item.email, textBuscado)}</div>` +
                                    `<div class="description">${boldQuery(item.telefono, textBuscado)} / ${boldQuery(item.telefono2, textBuscado)}</div>` +
                                    `<div class="description">${boldQuery(item.cp, textBuscado)}</div>` +
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

// #endregion

function showMap(sender, registro, event) {
    let emplazamientoID = event.data.EmplazamientoID;
    let emplazamiento = event.data.Codigo;

    parent.parent.addTab(parent.parent.App.tabPpal, registro + emplazamientoID, registro + '-' + emplazamiento, "../../PaginasComunes/MapaEmplazamientosCercanos.aspx?EmplazamientoID=" + emplazamientoID)
}

function MostrarPanelMoreInfo(sender) {
    parent.CargarPanelMoreInfoContacto(sender.$widgetRecord.data.EmplazamientoID, sender.$widgetRecord.data.ContactoGlobalID, true);
}