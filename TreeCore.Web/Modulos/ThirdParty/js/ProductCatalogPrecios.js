// #region DIRECT METHOD

var bindParamsPrecios = function () {

    showLoadMask(App.MainVwP, function (load) {
        App.hdCliID.setValue(parent.document.getElementById('hdCliID').value);
        App.hdStringBuscadorPrecios.setValue(parent.document.getElementById('hdStringBuscadorPrecios').value);
        App.hdIDPrecioBuscador.setValue(parent.document.getElementById('hdIDPrecioBuscador').value);
        App.hdStringBuscadorTraduccion.setValue(parent.document.getElementById('hdStringBuscadorTraduccion').value);
        App.hdIDTraduccionBuscador.setValue(parent.document.getElementById('hdIDTraduccionBuscador').value);
        App.storeCoreProductCatalog.reload();
        App.storeTraducciones.reload();

        load.hide();
    });
}

function agregarColumnaDinamica(oEstadoActualJSON, codigo, cabecera, entidad) {

    if (document.getElementById("columna_historico_" + codigo) == null) {

        if (document.getElementById("eventID_" + codigo) != null) {
            document.getElementById("eventID_" + codigo).classList.replace("x-mcombo-item-unchecked", "x-mcombo-item-checked");
        }

        var sHtml = "<div id=columna_historico_" + codigo + " class='historico_columna historico_columna_dinamico'>";
        sHtml += "<table>";
        // CABECERA
        sHtml += "<th class='historico_columna_cabecera'>" + cabecera + ' - ' + entidad;

        // BOTON "CLOSE" HISTORIAL
        sHtml += "<button onclick=\"cerrarHistorico('" + codigo + "')\" class='boton_cerrar_historico' style='margin-left: 15px;' /></th>";

        // INCIO CARGA ELEMENTOS LISTA
        sHtml += "<tbody class='historico_columna_cuerpo'>";

        for (var key in oEstadoActualJSON) {
            var valorEstadoActual = oEstadoActualJSON[key];

            var claseCSS = "historico_columna_valor";

            sHtml += "<tr class='" + claseCSS + "'><td>" + valorEstadoActual + "</ td></tr>";
        }

        sHtml += "</tbody>";
        // FIN CARGA ELEMENTOS LISTA

        sHtml += "</table></div>";

        document.getElementById("historico_contenedor").innerHTML += sHtml;
    }
    else {
        cerrarHistorico(codigo);
    }
}

function agregarColumnaDinamicaDesdeStore(sender, registro, index, oEstadoActualJSON) {

    if (registro.store != undefined) {
        var elementoCabecera = registro.store.config.readParameters("Cabecera").apply["Cabecera"];
        var cabecera = registro.data[elementoCabecera].toString();
        var lastIndex = registro.data[elementoCabecera].toLocaleString().split(" ").length - 1;
        var codigo = registro.data[elementoCabecera].toLocaleString().split(" ")[lastIndex];
        var entidad = registro.data['NombreEntidad'];

        agregarColumnaDinamica(oEstadoActualJSON, codigo, cabecera, entidad);
    }
}

function cerrarHistorico(codigo) {

    var id = "columna_historico_" + codigo;
    var top = document.getElementById("historico_contenedor");
    var historico = document.getElementById(id);

    if (document.getElementById("eventID_" + codigo) != null) {
        if (document.getElementById("eventID_" + codigo).classList.contains("x-mcombo-item-checked")) {
            document.getElementById("eventID_" + codigo).classList.replace("x-mcombo-item-checked", "x-mcombo-item-unchecked");
        }
    }

    if (historico != null) {
        top.removeChild(historico);
    }

    for (var valor = 0; valor < App.GridRowSelectPrecio.selected.items.length; valor++) {
        if (App.GridRowSelectPrecio.selected.items[valor].data.Codigo == codigo) {
            App.GridRowSelectPrecio.deselect(App.GridRowSelectPrecio.selected.items[valor]);
        }
    }

    App.storeCoreProductCatalog.reload();
}

function agregarColumnaDinamicaPrecio(sender, registro, index, lista) {

    if (registro != undefined) {
        TreeCore.GetEstadoActualPrecioJSON(false, null,
            {
                success: function (result) {
                    if (result.Result != null) {
                        let oEstadoActualJSON = result.Result;
                        agregarColumnaDinamicaDesdeStore(sender, registro, index, oEstadoActualJSON);
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    }
    else if (lista != null) {
        var codigoFinal;
        var nombreFinal;
        var vID;
        var elementoCabecera = App.storeCoreProductCatalog.config.readParameters("Cabecera").apply["Cabecera"];
        var elementoCodigo = App.storeCoreProductCatalog.config.readParameters("Codigo").apply["Codigo"];
        var nombreEntidad = App.storeCoreProductCatalog.config.readParameters("Nombre").apply["Nombre"];

        for (var i = 0; i < App.storeCoreProductCatalog.data.items.length; i++) {

            var lastIndex = App.storeCoreProductCatalog.data.items[i].data[elementoCabecera].toLocaleString().split(" ").length - 1;
            var codigo = App.storeCoreProductCatalog.data.items[i].data[elementoCabecera].toLocaleString().split(" ")[lastIndex];
            var id = App.storeCoreProductCatalog.data.items[i].data[elementoCodigo].toLocaleString().split(" ")[lastIndex];

            var items = App.hdListaCatalogos.getValue().split(',');
            for (var j = 0; j < items.length; j++) {
                if (id == items[j]) {
                    lastIndex = App.storeCoreProductCatalog.data.items[i].data[elementoCabecera].toLocaleString().split(" ").length - 1;
                    codigoFinal = App.storeCoreProductCatalog.data.items[i].data[elementoCabecera].toLocaleString().split(" ")[lastIndex];
                    nombreFinal = App.storeCoreProductCatalog.data.items[i].data[nombreEntidad].toLocaleString().split(" ")[lastIndex];
                    vID = id;
                }    
            }

            cerrarHistorico(codigo);
        }

        if (codigoFinal != undefined && nombreFinal != undefined) {
            agregarColumnaDinamica(lista, codigoFinal, codigoFinal, nombreFinal);
            App.hdListaCatalogos.setValue(vID);
        }
    }
}

function Grid_RowSelectPrecio(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;

        App.hdServicioPrecioID.setValue(seleccionado.CoreProductCatalogID);
        parent.App.hdServicioPadreID.setValue(seleccionado.CoreProductCatalogID);

        if (App.hdListaCatalogos.getValue() == '') {
            App.hdListaCatalogos.setValue(seleccionado.CoreProductCatalogID);
        }
        else {
            App.hdListaCatalogos.setValue(App.hdListaCatalogos.getValue() + ',' + seleccionado.CoreProductCatalogID);
        }

        if (parent.App.pnAsideR.collapsed == false) {
            parent.App.btnEstados.hide();
            parent.App.btnLink.hide();
            parent.App.btnPrecios.hide();
            parent.App.MenuNavPnServicios.updateLayout();

            parent.App.btnClausulas.removeCls('btnEnableClick');
            parent.App.btnClausulas.addCls('btnDisableClick');
            parent.displayMenu('pnClausulas', App.storeCoreProductCatalog, registro);
        }

        App.storeTraducciones.reload();
    }
}

function DeseleccionarGrillaPrecio(sender, registro, index) {

    if (registro != undefined && registro.data != undefined) {
        cerrarHistorico(registro.data.Codigo);
        App.hdServicioPrecioID.setValue('');
        App.hdListaCatalogos.setValue('');
    }
}

function RefrescarPrecio(sender, registro, index) {
    nfPaginNumberFirstloadPrecios = true;
    currentPagePrecios = 1;
    LimpiarFiltroBusquedaPrecios();
    App.storeCoreProductCatalog.reload();
    App.GridRowSelectPrecio.clearSelections();
    App.hdListaCatalogos.setValue('');
    App.hdServicioPrecioID.setValue('');

    var elementoCabecera = App.storeCoreProductCatalog.config.readParameters("Cabecera").apply["Cabecera"];
    for (var i = 0; i < App.storeCoreProductCatalog.data.items.length; i++) {

        var lastIndex = App.storeCoreProductCatalog.data.items[i].data[elementoCabecera].toLocaleString().split(" ").length - 1;
        var codigo = App.storeCoreProductCatalog.data.items[i].data[elementoCabecera].toLocaleString().split(" ")[lastIndex];

        cerrarHistorico(codigo);
    }

}

function RefrescarContenido() {
    App.storeTraducciones.reload();
    App.hdListaCatalogos.setValue('');
}

var onShow = function (toolTip, grid) {
    var view = grid.getView(),
        store = grid.getStore(),
        record = view.getRecord(view.findItemByChild(toolTip.triggerElement)),
        column = view.getHeaderByCell(toolTip.triggerElement),
        data = record.get(column.dataIndex);

    if (data.length > 20) {
        toolTip.show();
        toolTip.update(data);
    }
    else {
        toolTip.close();
    }
};

// #endregion

// #region COMBOS

function RecargarEntidad() {
    recargarCombos([App.cmbEntidad]);
}

function SeleccionarEntidad() {
    App.cmbEntidad.getTrigger(0).show();
}

// #endregion

// #region DISEÑO

var bShowPrincipal = true;
var bShowOnlySecundary = false;
var iSelectedPanel = 0;

function showPanelsByWindowSize() {

    let puntoCorte = 512;
    var tmn = App.CenterPanelMain.getWidth();

    if (tmn < puntoCorte) {
        App.tbFiltersLabels.show();
        App.btnPrev.show();
        App.btnNext.show();
        loadPanelByBtns("");
    }
    else {
        App.tbFiltersLabels.hide()
        App.btnPrev.hide();
        App.btnNext.hide();
        loadPanels();
    }
}

function loadPanels() {

    if (bShowOnlySecundary) {
        App.gridPrecios.hide();
        App.btnCloseShowVisorTreeP.setIconCls('ico-moverow-gr');
    }
    else {

        App.pnSecundario.show();

        if (bShowPrincipal) {
            App.gridPrecios.show();
        }
        else {
            App.gridPrecios.hide();
        }

        App.btnCloseShowVisorTreeP.setIconCls('ico-hide-menu');
    }
}

function loadPanelByBtns(pressedBtn) {

    // CHECK FOR A PRESSED BTN
    if (pressedBtn != "") {
        if (pressedBtn == "Next") {
            iSelectedPanel++;
        }
        else {
            iSelectedPanel--;
        }
    }

    // CHECK FOR DISABLED BUTTONS
    if (iSelectedPanel == 1) {
        App.btnPrev.enable();
        App.btnNext.disable();
    }
    else {
        App.btnPrev.disable();
        App.btnNext.enable();
    }

    // LOAD PANEL
    if (iSelectedPanel == 0) {
        App.gridPrecios.show();
        App.pnSecundario.hide();
    }
    if (iSelectedPanel == 1) {
        App.gridPrecios.hide();
        App.pnSecundario.show();
    }
}

function showOnlySecundary() {

    bShowOnlySecundary = !bShowOnlySecundary;
    loadPanels();
}

// #endregion

// #region BUSCADOR CATALOGOS

function filtrarPorBuscadorPrecios(sender, registro, index) {
    if (registro.charCode == 13) {
        App.hdStringBuscadorPrecios.setValue(sender.value);
        App.txtSearchPrecios.setValue(sender.value);
        App.storeCoreProductCatalog.reload();
    }
}

function BorrarFiltrosPrecios(sender, registro) {
    var tree = App.gridPrecios;
    tree.filters.clearFilters();
    LimpiarFiltroBusquedaPrecios();
}

function LimpiarFiltroBusquedaPrecios(sender, registro) {

    App.hdStringBuscadorPrecios.setValue("");
    App.hdIDPrecioBuscador.setValue("");
    App.txtSearchPrecios.setValue("");

    App.storeCoreProductCatalog.clearFilter();
    App.storeCoreProductCatalog.reload();
}

var dataGridPrecios = [];

function ajaxGetDatosBuscadorPrecios() {

    updatePaginPrecios();

    if (dataGridPrecios.length == 0 || forzarCargaBuscadorPredictivo) {
        dataGridPrecios = [];

        TreeCore.GetDatosBuscadorPrecios({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    result.Result.forEach(serv => {
                        dataGridPrecios.push({
                            key: serv.NombreEntidad.toLowerCase(),
                            key2: serv.Codigo.toLowerCase(),
                            nombre: serv.NombreEntidad,
                            codigo: serv.Codigo,
                            id: serv.CoreProductCatalogID
                        });
                    });

                    dataGridPrecios = dataGridPrecios.sort(function (a, b) {
                        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
                    });

                    var nameSearchBox = "txtSearchPrecios";
                    var selectorSearchBox = `#${nameSearchBox}-inputEl`;

                    $(function () {
                        let textBuscado = "";
                        $(selectorSearchBox).autocomplete({
                            source: function (request, response) {
                                textBuscado = request.term;
                                var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                                let results = $.grep(dataGridPrecios, function (value) {
                                    let value1 = value.key;
                                    let value2 = value.key2;

                                    return matcher.test(value1) || matcher.test(normalize(value1)) || matcher.test(value2) || matcher.test(normalize(value2));
                                });

                                response(results.slice(0, 10));

                                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                                    var idServicioBuscador = $(e.currentTarget).attr("data-emplazamientoID");
                                    App.hdStringBuscadorPrecios.setValue("");
                                    App.hdIDPrecioBuscador.setValue(idServicioBuscador);
                                    App.storeCoreProductCatalog.reload();
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

// #region BUSCADOR PRECIOS

function filtrarPorBuscadorTraduccion(sender, registro, index) {
    if (registro.charCode == 13) {
        App.hdStringBuscadorTraduccion.setValue(sender.value);
        App.txtSearchTraduccion.setValue(sender.value);
        App.storeTraducciones.reload();
    }
}

function BorrarFiltrosTraduccion(sender, registro) {
    var tree = App.pnColumnaTraducciones;
    tree.filters.clearFilters();
    LimpiarFiltroBusquedaTraduccion();
}

function LimpiarFiltroBusquedaTraduccion(sender, registro) {

    App.hdStringBuscadorTraduccion.setValue("");
    App.hdIDTraduccionBuscador.setValue("");
    App.txtSearchTraduccion.setValue("");

    App.storeTraducciones.clearFilter();
    App.storeTraducciones.reload();

    App.hdListaCatalogos.setValue('');

    var elementoCabecera = App.storeCoreProductCatalog.config.readParameters("Cabecera").apply["Cabecera"];
    for (var i = 0; i < App.storeCoreProductCatalog.data.items.length; i++) {

        var lastIndex = App.storeCoreProductCatalog.data.items[i].data[elementoCabecera].toLocaleString().split(" ").length - 1;
        var codigo = App.storeCoreProductCatalog.data.items[i].data[elementoCabecera].toLocaleString().split(" ")[lastIndex];

        cerrarHistorico(codigo);
    }
}

var dataGridTraduccion = [];

function ajaxGetDatosBuscadorTraduccion() {

    updatePaginTraduccion();

    if (dataGridTraduccion.length == 0 || forzarCargaBuscadorPredictivo) {
        dataGridTraduccion = [];

        TreeCore.GetDatosBuscadorTraduccion({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    result.Result.forEach(serv => {
                        dataGridTraduccion.push({
                            key: serv.Nombre.toLowerCase(),
                            key2: serv.Codigo.toLowerCase(),
                            nombre: serv.Nombre,
                            codigo: serv.Codigo,
                            id: serv.CoreProductCatalogServicioID
                        });
                    });

                    dataGridTraduccion = dataGridTraduccion.sort(function (a, b) {
                        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
                    });

                    var nameSearchBoxTraduccion = "txtSearchTraduccion";
                    var selectorSearchBoxTraduccion = `#${nameSearchBoxTraduccion}-inputEl`;

                    $(function () {
                        let textBuscado = "";
                        $(selectorSearchBoxTraduccion).autocomplete({
                            source: function (request, response) {
                                textBuscado = request.term;
                                var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                                let results = $.grep(dataGridTraduccion, function (value) {
                                    let value1 = value.key;
                                    let value2 = value.key2;

                                    return matcher.test(value1) || matcher.test(normalize(value1)) || matcher.test(value2) || matcher.test(normalize(value2));
                                });

                                response(results.slice(0, 10));

                                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                                    var idTraduccionBuscador = $(e.currentTarget).attr("data-emplazamientoID");
                                    App.hdStringBuscadorTraduccion.setValue("");
                                    App.hdIDTraduccionBuscador.setValue(idTraduccionBuscador);
                                    App.storeTraducciones.reload();

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

// #region PAGINACION CATALOGOS

var nfPaginNumberFirstloadPrecios = true;
var currentPagePrecios = 1;

var pageSelectPrecios = function (item) {

    let storePrecios = this.up('gridpanel').store;

    var curPageSizePrecios = storePrecios.pageSize,
        wantedPageSizePrecios = parseInt(item.getValue(), 10);

    if (wantedPageSizePrecios != curPageSizePrecios) {
        storePrecios.pageSize = wantedPageSizePrecios;
        storePrecios.load({
            callback: function (r, options, success) {
                nfPaginNumberFirstloadPrecios = true;
                updatePaginPrecios();
            }
        });
    }
}

function nfPaginNumberBeforeRenderPrecios(sender, registro) {
    if (nfPaginNumberFirstloadPrecios) {

        nfPaginNumberFirstloadPrecios = false;

        App.nfPaginNumberPrecios.setValue(currentPagePrecios);
    }
}

function pagingInitPrecios() {
    currentPagePrecios = 1;
    App.storeCoreProductCatalog.loadPage(currentPagePrecios);
    updatePaginPrecios();
}

function pagingPrePrecios() {
    currentPagePrecios--;
    App.storeCoreProductCatalog.loadPage(currentPagePrecios);
    updatePaginPrecios();
}

function paginGoToPrecios(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPagePrecios()) {
        currentPagePrecios = sender.value;
        App.storeCoreProductCatalog.loadPage(sender.value);
        updatePaginPrecios();
    }
}

function paginNextPrecios() {
    currentPagePrecios++;
    App.storeCoreProductCatalog.loadPage(currentPagePrecios);
    updatePaginPrecios();
}

function paginLastPrecios() {

    currentPagePrecios = getLastPagePrecios();
    App.storeCoreProductCatalog.loadPage(currentPagePrecios);
    updatePaginPrecios();
}

function getLastPagePrecios() {
    let totalPrecios = 0;

    if (App.hdTotalCountGridPrecios.value) {
        totalPrecios = App.hdTotalCountGridPrecios.value;
    }
    else {
        totalPrecios = App.storeCoreProductCatalog.getTotalCount();
    }

    let pageSizePrecios = App.storeCoreProductCatalog.pageSize;

    pageSizePrecios = totalPrecios / pageSizePrecios;

    if (pageSizePrecios % 1 != 0) {
        pageSizePrecios = Math.trunc(pageSizePrecios);
        pageSizePrecios++;
    }

    return pageSizePrecios;
}

function updatePaginPrecios() {
    let lastPagePrecios = getLastPagePrecios();
    let totalPrecios = 0;
    var pageSizePrecios = App.storeCoreProductCatalog.pageSize;

    if (App.hdTotalCountGridPrecios.value) {
        totalPrecios = App.hdTotalCountGridPrecios.value;
    } else {
        totalPrecios = App.storeCoreProductCatalog.getTotalCount();
    }

    pageSizePrecios = totalPrecios / pageSizePrecios;

    if (pageSizePrecios % 1 != 0) {
        pageSizePrecios = Math.trunc(pageSizePrecios);
        pageSizePrecios++;
    }

    if (pageSizePrecios < currentPagePrecios) {
        currentPagePrecios = 1;
        App.storeCoreProductCatalog.loadPage(currentPagePrecios);
    }

    App.nfPaginNumberPrecios.setValue(`${currentPagePrecios}`);
    App.lbNumberPagesPrecios.setText(`${lastPagePrecios}`);

    if (currentPagePrecios == 1 && lastPagePrecios == 1) {
        App.btnPagingInitPrecios.setDisabled(true);
        App.btnPagingPrePrecios.setDisabled(true);
        App.btnPaginNextPrecios.setDisabled(true);
        App.btnPaginLastPrecios.setDisabled(true);
    }
    else if (currentPagePrecios <= 1) {
        App.btnPagingInitPrecios.setDisabled(true);
        App.btnPagingPrePrecios.setDisabled(true);
        App.btnPaginNextPrecios.setDisabled(false);
        App.btnPaginLastPrecios.setDisabled(false);
    }
    else if (currentPagePrecios >= lastPagePrecios) {
        App.btnPaginNextPrecios.setDisabled(true);
        App.btnPaginLastPrecios.setDisabled(true);
        App.btnPagingInitPrecios.setDisabled(false);
        App.btnPagingPrePrecios.setDisabled(false);
    } else {
        App.btnPaginNextPrecios.setDisabled(false);
        App.btnPaginLastPrecios.setDisabled(false);
        App.btnPagingInitPrecios.setDisabled(false);
        App.btnPagingPrePrecios.setDisabled(false);
    }

    updateDisplayingPrecios();
}

function updateDisplayingPrecios() {
    var txtDisplayingPrecios = "";
    var totalPrecios = 0;
    let lastPagePrecios = getLastPagePrecios();
    var pageSizePrecios = App.storeCoreProductCatalog.pageSize;

    if (App.hdTotalCountGridPrecios.value) {
        totalPrecios = App.hdTotalCountGridPrecios.value;
    } else {
        totalPrecios = App.storeCoreProductCatalog.getTotalCount();
    }

    lastPagePrecios = totalPrecios / pageSizePrecios;

    if (lastPagePrecios % 1 != 0) {
        lastPagePrecios = Math.trunc(lastPagePrecios);
        lastPagePrecios++;
    }

    if (lastPagePrecios < currentPagePrecios) {
        currentPagePrecios = 1;
    }

    let firstItem = ((currentPagePrecios - 1) * pageSizePrecios) + 1;
    let lastItem = currentPagePrecios * pageSizePrecios;

    if (currentPagePrecios == getLastPagePrecios()) {
        lastItem = totalPrecios;
    }

    if (totalPrecios == 0) {
        txtDisplayingPrecios = jsSinDatosMostrar;
    }
    else {
        txtDisplayingPrecios = `${jsMostrando} ${firstItem} - ${lastItem} ${jsDe} ${totalPrecios}`;
    }

    App.lbDisplayingPrecios.setText(txtDisplayingPrecios);
    forzarCargaBuscadorPredictivo = true;
}

function setNumMaxPagePrecios() {
    updateDisplayingPrecios();
    updatePaginPrecios();
}


// #endregion

// #region PAGINACION PRECIOS

var nfPaginNumberFirstloadTraduccion = true;
var currentPageTraduccion = 1;

var pageSelectTraduccion = function (item) {

    let storeTraduccion = this.up('gridpanel').store;

    var curPageSizeTraduccion = storeTraduccion.pageSize,
        wantedPageSizeTraduccion = parseInt(item.getValue(), 10);

    if (wantedPageSizeTraduccion != curPageSizeTraduccion) {
        storeTraduccion.pageSize = wantedPageSizeTraduccion;
        storeTraduccion.load({
            callback: function (r, options, success) {
                nfPaginNumberFirstloadTraduccion = true;
                updatePaginTraduccion();
            }
        });
    }
}

function nfPaginNumberBeforeRenderTraduccion(sender, registro) {
    if (nfPaginNumberFirstloadTraduccion) {

        nfPaginNumberFirstloadTraduccion = false;

        App.nfPaginNumberTraduccion.setValue(currentPageTraduccion);
    }
}

function pagingInitTraduccion() {
    currentPageTraduccion = 1;
    App.storeTraduccion.loadPage(currentPageTraduccion);
    updatePaginTraducciones();
}

function pagingPreTraduccion() {
    currentPageTraduccion--;
    App.storeTraducciones.loadPage(currentPageTraduccion);
    updatePaginTraduccion();
}

function paginGoToTraduccion(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageTraduccion()) {
        currentPageTraduccion = sender.value;
        App.storeTraducciones.loadPage(sender.value);
        updatePaginTraduccion();
    }
}

function paginNextTraduccion() {
    currentPageTraduccion++;
    App.storeTraducciones.loadPage(currentPageTraduccion);
    updatePaginTraduccion();
}

function paginLastTraduccion() {

    currentPageTraduccion = getLastPageTraduccion();
    App.storeTraducciones.loadPage(currentPageTraduccion);
    updatePaginTraduccion();
}

function getLastPageTraduccion() {
    let totalTraduccion = 0;

    if (App.hdTotalCountGridTraduccion.value) {
        totalTraduccion = App.hdTotalCountGridTraduccion.value;
    }
    else {
        totalTraduccion = App.storeTraducciones.getTotalCount();
    }

    let pageSizeTraduccion = App.storeTraducciones.pageSize;

    pageSizeTraduccion = totalTraduccion / pageSizeTraduccion;

    if (pageSizeTraduccion % 1 != 0) {
        pageSizeTraduccion = Math.trunc(pageSizeTraduccion);
        pageSizeTraduccion++;
    }

    return pageSizeTraduccion;
}

function updatePaginTraduccion() {
    let lastPageTraduccion = getLastPageTraduccion();
    let totalTraduccion = 0;
    var pageSizeTraduccion = App.storeTraducciones.pageSize;

    if (App.hdTotalCountGridTraduccion.value) {
        totalTraduccion = App.hdTotalCountGridTraduccion.value;
    } else {
        totalTraduccion = App.storeTraducciones.getTotalCount();
    }

    pageSizeTraduccion = totalTraduccion / pageSizeTraduccion;

    if (pageSizeTraduccion % 1 != 0) {
        pageSizeTraduccion = Math.trunc(pageSizeTraduccion);
        pageSizeTraduccion++;
    }

    if (pageSizeTraduccion < currentPageTraduccion) {
        currentPageTraduccion = 1;
        App.storeTraducciones.loadPage(currentPageTraduccion);
    }

    App.nfPaginNumberTraduccion.setValue(`${currentPageTraduccion}`);
    App.lbNumberPagesTraduccion.setText(`${lastPageTraduccion}`);

    if (currentPageTraduccion == 1 && lastPageTraduccion == 1) {
        App.btnPagingInitTraduccion.setDisabled(true);
        App.btnPagingPreTraduccion.setDisabled(true);
        App.btnPaginNextTraduccion.setDisabled(true);
        App.btnPaginLastTraduccion.setDisabled(true);
    }
    else if (currentPageTraduccion <= 1) {
        App.btnPagingInitTraduccion.setDisabled(true);
        App.btnPagingPreTraduccion.setDisabled(true);
        App.btnPaginNextTraduccion.setDisabled(false);
        App.btnPaginLastTraduccion.setDisabled(false);
    }
    else if (currentPageTraduccion >= lastPageTraduccion) {
        App.btnPaginNextTraduccion.setDisabled(true);
        App.btnPaginLastTraduccion.setDisabled(true);
        App.btnPagingInitTraduccion.setDisabled(false);
        App.btnPagingPreTraduccion.setDisabled(false);
    } else {
        App.btnPaginNextTraduccion.setDisabled(false);
        App.btnPaginLastTraduccion.setDisabled(false);
        App.btnPagingInitTraduccion.setDisabled(false);
        App.btnPagingPreTraduccion.setDisabled(false);
    }

    updateDisplayingTraduccion();
}

function updateDisplayingTraduccion() {
    var txtDisplayingTraduccion = "";
    var totalTraduccion = 0;
    let lastPageTraduccion = getLastPageTraduccion();
    var pageSizeTraduccion = App.storeTraducciones.pageSize;

    if (App.hdTotalCountGridTraduccion.value) {
        totalTraduccion = App.hdTotalCountGridTraduccion.value;
    } else {
        totalTraduccion = App.storeTraducciones.getTotalCount();
    }

    lastPageTraduccion = totalTraduccion / pageSizeTraduccion;

    if (lastPageTraduccion % 1 != 0) {
        lastPageTraduccion = Math.trunc(lastPageTraduccion);
        lastPageTraduccion++;
    }

    if (lastPageTraduccion < currentPageTraduccion) {
        currentPageTraduccion = 1;
    }

    let firstItem = ((currentPageTraduccion - 1) * pageSizeTraduccion) + 1;
    let lastItem = currentPageTraduccion * pageSizeTraduccion;

    if (currentPageTraduccion == getLastPageTraduccion()) {
        lastItem = totalTraduccion;
    }

    if (totalTraduccion == 0) {
        txtDisplayingTraduccion = jsSinDatosMostrar;
    }
    else {
        txtDisplayingTraduccion = `${jsMostrando} ${firstItem} - ${lastItem} ${jsDe} ${totalTraduccion}`;
    }

    App.lbDisplayingTraduccion.setText(txtDisplayingTraduccion);
    forzarCargaBuscadorPredictivo = true;
}

function setNumMaxPageTraduccion() {
    updateDisplayingTraduccion();
    updatePaginTraduccion();
}


// #endregion