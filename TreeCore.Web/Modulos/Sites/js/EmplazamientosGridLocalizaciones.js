var markersForm = [];
var mapForm = [];

var bindParams = function () {
    App.hdCliID.setValue(parent.document.getElementById('hdCliID').value);
    App.hdStringBuscador.setValue(parent.document.getElementById('hdStringBuscador').value);
    App.hdIDEmplazamientoBuscador.setValue(parent.document.getElementById('hdIDEmplazamientoBuscador').value);
    App.hdFiltrosAplicados.setValue(parent.document.getElementById('hdFiltrosAplicados').value);

    App.hdidsResultados.setValue(parent.document.getElementById('hdidsResultados').value);
    App.hdnameIndiceID.setValue(parent.document.getElementById('hdnameIndiceID').value);
}

function DeseleccionarGrillaLocalizaciones(sender, registro, index) {

    App.GridRowSelectLocalizaciones.clearSelections();
}

function Grid_RowSelectLocalizaciones(sender, registro) {
    parent.CargarPanelMoreInfo(registro.data.EmplazamientoID, false);
}


function refrescarLocalizaciones() {
    nfPaginNumberFirstload = true;
    currentPage = 1;
    LimpiarFiltroBusquedaLocalizaciones();
    App.storeEmplazamientosLocalizaciones.reload();
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
    App.gridEmplazamientosLocalizaciones.store.reload();
}

function FiltrarColumnasLocalizaciones(sender, registro) {

    App.btnDescargar.disable();
    var tree = App.gridEmplazamientosLocalizaciones;
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


function LimpiarFiltroBusquedaLocalizaciones(sender, registro) {

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App.txtSearch.setValue("");

    App.storeEmplazamientosLocalizaciones.clearFilter();
    App.storeEmplazamientosLocalizaciones.reload();
    pagingInitLocalizaciones();
    App.btnDescargar.enable();
}


function BorrarFiltrosLozalizaciones(sender, registro) {
    var tree = App.gridEmplazamientosLocalizaciones;
    tree.filters.clearFilters();
}

// #endregion

// #region Paginación
var nfPaginNumberFirstload = true;
var currentPage = 1;

function filtrarEmplazamientosPorBuscador(sender, registro, index) {
    if (registro.charCode == 13) {
        App.hdStringBuscador.setValue(sender.value);
        App.storeEmplazamientosLocalizaciones.reload();
    }
}


function nfPaginNumberBeforeRenderLocalizaciones(sender, registro) {
    if (nfPaginNumberFirstload) {

        nfPaginNumberFirstload = false;

        App.nfPaginNumber.setValue(currentPage);

    }
}

function pagingInitLocalizaciones() {
    currentPage = 1;
    App.storeEmplazamientosLocalizaciones.loadPage(currentPage);
    updatePaginLocalizaciones();
}

function pagingPreLocalizaciones() {
    currentPage--;
    App.storeEmplazamientosLocalizaciones.loadPage(currentPage);
    updatePaginLocalizaciones();
}

function paginGoToLocalizaciones(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageLocalizaciones()) {
        currentPage = sender.value;
        App.storeEmplazamientosLocalizaciones.loadPage(sender.value);
        updatePaginLocalizaciones();
    }
}

function paginNextLocalizaciones() {
    currentPage++;
    App.storeEmplazamientosLocalizaciones.loadPage(currentPage);
    updatePaginLocalizaciones();
}

function paginLastLocalizaciones() {

    currentPage = getLastPageLocalizaciones();
    App.storeEmplazamientosLocalizaciones.loadPage(currentPage);
    updatePaginLocalizaciones();
}

function getLastPageLocalizaciones() {
    let total = 0;
    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeEmplazamientosLocalizaciones.getTotalCount();
    }

    let pageSize = App.storeEmplazamientosLocalizaciones.pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

function updatePaginLocalizaciones() {
    let lastPage = getLastPageLocalizaciones();
    var pageSize = App.storeEmplazamientosLocalizaciones.pageSize;
    let total = 0;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeEmplazamientosLocalizaciones.getTotalCount();
    }

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    if (pageSize < currentPage) {
        currentPage = 1;
        App.storeEmplazamientosLocalizaciones.loadPage(currentPage);
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

    updateDisplayingLocalizaciones();
}

function updateDisplayingLocalizaciones() {
    var txtDisplaying = "";
    var total = 0;
    let lastPage = getLastPageLocalizaciones();
    var pageSize = App.storeEmplazamientosLocalizaciones.pageSize;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeEmplazamientosLocalizaciones.getTotalCount();
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

    if (currentPage == getLastPageLocalizaciones()) {
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

function setNumMaxPageLocalizaciones() {
    updateDisplayingLocalizaciones();
    updatePaginLocalizaciones();
}
// #endregion

// #region Buscador
var dataGridEmplazamientos = [];
function ajaxGetDatosBuscadorLocalizaciones() {

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
                            key3: emp.Direccion.toLowerCase(),
                            key4: emp.CodigoPostal.toLowerCase(),
                            nombre: emp.NombreSitio,
                            codigo: emp.Codigo,
                            direccion: emp.Direccion,
                            codigoPostal: emp.CodigoPostal,
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
                                    App.storeEmplazamientosLocalizaciones.reload();

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

// #endregion

function CargarGridLocalizaciones(sender) {
    TreeCore.CargarGrid();
}


function Grid_RowSelectEmplazamiento(sender, registro, index) {
    parent.CargarPanelMoreInfo(registro.data.EmplazamientoID, false);
}
function MostrarPanelMoreInfo(sender) {
    parent.CargarPanelMoreInfo(sender.$widgetRecord.data.EmplazamientoID, true);
}
function showMap(sender, registro, index) {

    let emplazamientoID = index.data.EmplazamientoID;
    let CodigoSitio = index.data.Codigo;
    let NombreSitio = index.data.NombreSitio;

    App.contPosicion.setTitle(NombreSitio);
    

    Latitud = "";
    Longitud = "";

    // #region LATITUD, LONGITUD
    if (index.data.Latitud != 0 && index.data.Longitud != 0) {
        Latitud = index.data.Latitud;
        Longitud = index.data.Longitud;
    }
    if (index.data.Direccion != "") {
        direccion = index.data.Direccion;
    }

    mapID = document.getElementById("MapaTabLocation");
    App.contPosicion.show();
    // #endregion

    markersForm.forEach((marker) => {
        marker.setMap(null);
    });
    markersForm = [];

    // #region CENTRO

    pCentro = new Promise((resolve, reject) => {
        
        centro = {
            lat: parseFloat(Latitud),
            lng: parseFloat(Longitud)
        };
        resolve(centro);
    });

    // #endregion

    pCentro.then((centro) => {

        if (mapForm[mapID.id] === undefined) {
            mapForm[mapID.id] = new google.maps.Map(document.getElementById(mapID.id), {
                center: centro,
                zoom: 12
            });
        }

        var marker = new google.maps.Marker({
            position: centro,
            map: mapForm[mapID.id],
            draggable: true
        });

        markersForm.push(marker);
        mapForm[mapID.id].setCenter(centro);
    });

}

