var bindParams = function () {
    App.hdCliID.setValue(parent.document.getElementById('hdCliID').value);
    App.hdStringBuscador.setValue(parent.document.getElementById('hdStringBuscador').value);
    App.hdIDEmplazamientoBuscador.setValue(parent.document.getElementById('hdIDEmplazamientoBuscador').value);
    App.hdFiltrosAplicados.setValue(parent.document.getElementById('hdFiltrosAplicados').value);

    App.hdidsResultados.setValue(parent.document.getElementById('hdidsResultados').value);
    App.hdnameIndiceID.setValue(parent.document.getElementById('hdnameIndiceID').value);

    //TreeCore.GenerarGridDinamico({
    //    success: function (result) {
    //        if (!result.Success) {
    //            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
    //        } else {
                App.storeEmplazamientosAtributos.reload();
    //        }
    //    }
    //});
}


function DeseleccionarGrillaEmplazamientosAtributos(sender, registro, index) {
    App.GridRowSelectEmplazamientosAtributos.clearSelections();
}

function Grid_RowSelectEmplazamientosAtributos(sender, registro, index) {
    parent.CargarPanelMoreInfo(registro.data.EmplazamientoID, false);
}

function refrescarEmplazamientosAtributos(sender, registro, index) {

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App.txtSearch.setValue("");
    LimpiarFiltroBusquedaEmplazamientosAtributos();
    App.storeEmplazamientosAtributos.reload();
}

var pageSelectAtributos = function (item) {
    let store = this.up('gridpanel').store;
    var curPageSize = store.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);
    if (wantedPageSize != curPageSize) {
        store.pageSize = wantedPageSize;
        store.load({
            callback: function (r, options, success) {
                nfPaginNumberFirstload = true;
                updatePaginAtributos();
            }
        });
    }
}

//Fin Ventana Modal Gestion Form
var stP = 0;
var stBtn = 0;
function showPnAsideR(btnId) {
    if (stP == 0) {
        let pnR = document.getElementById('pnAsideR');
        pnR.style.width = '400px';
        App.pnAsideR.show();
        stP = 1;
    }

    switch (btnId) {
        case 'btnBuscar':
            App.btnFiltros.pressed = false;
            App.lblHeadSearch.show();
            App.pnSearch.show();
            App.lblHeadFilters.hide();
            App.pnCreateFltr.hide();
            App.pnMyFltr.hide();
            if (stBtn == 1) {
                hidePnR();
                stP = 0;
                stBtn = 0;
            }
            else {
                stBtn = 1;
            }
            break;
        case 'btnFiltros':
            App.btnBuscar.pressed = false;
            App.lblHeadFilters.show();
            App.pnCreateFltr.show();
            App.pnMyFltr.show();
            let ctn = document.getElementById('pnMyFltr_Content');
            ctn.style.display = 'block';;
            App.lblHeadSearch.hide();
            App.pnSearch.hide();
            if (stBtn == 2) {
                hidePnR();
                stP = 0;
                stBtn = 0;
            }
            else {
                stBtn = 2;
            }
            break;
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
    App.grdEmplazamientosAtributos.store.reload();
}

function FiltrarColumnasEmplazamientosAtributos(sender, registro) {

    App.btnDescargar.disable();
    var tree = App.grdEmplazamientosAtributos;
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


function LimpiarFiltroBusquedaEmplazamientosAtributos(sender, registro) {

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App.txtSearch.setValue("");

    App.storeEmplazamientosAtributos.clearFilter();
    App.storeEmplazamientosAtributos.reload();
    pagingInitAtributos();
    App.btnDescargar.enable();
}


function BorrarFiltrosEmplazamientosAtributos(sender, registro) {

    var tree = App.grdEmplazamientosAtributos;
    tree.filters.clearFilters();
}

// #endregion


function winResiz(obj) {

    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);

    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);

    }

    if (res <= 670) {
        obj.setWidth(500);

    }

    obj.center();
}


// #endregion

// #region Paginación
var nfPaginNumberFirstload = true;
var currentPage = 1;

function filtrarEmplazamientosPorBuscador(sender, registro, index) {
    if (registro.charCode == 13) {
        App.hdStringBuscador.setValue(sender.value);
        App.storeEmplazamientosAtributos.reload();
    }
}

function LimpiarFiltroBusqueda(sender, registro) {

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App.txtSearch.setValue("");

    logic.clearFilter();
    App._mnuDwldExcel.enable();

}

function nfPaginNumberBeforeRenderAtributos(sender, registro) {

    if (nfPaginNumberFirstload) {
        nfPaginNumberFirstload = false;
        App.nfPaginNumber.setValue(currentPage);
    }
}

function pagingInitAtributos() {

    currentPage = 1;
    App.storeEmplazamientosAtributos.loadPage(currentPage);
    updatePaginAtributos();
}

function pagingPreAtributos(sender) {

    currentPage--;
    App.storeEmplazamientosAtributos.loadPage(currentPage);
    updatePaginAtributos();
}

function paginGoToAtributos(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageAtributos()) {

        currentPage = sender.value;
        App.storeEmplazamientosAtributos.loadPage(sender.value);
        updatePaginAtributos();
    }
}

function paginNextAtributos(sender) {

    currentPage++;
    App.storeEmplazamientosAtributos.loadPage(currentPage);
    updatePaginAtributos();
}

function paginLastAtributos(sender) {

    currentPage = getLastPageAtributos(sender);
    App.storeEmplazamientosAtributos.loadPage(currentPage);
    updatePaginAtributos();
}

function getLastPageAtributos(sender) {

    let total = 0;
    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeEmplazamientosAtributos.getTotalCount();
    }

    let pageSize = App.storeEmplazamientosAtributos.pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

function updatePaginAtributos(sender) {
    let lastPage = getLastPageAtributos(sender);
    let total = 0;
    var pageSize = App.storeEmplazamientosAtributos.pageSize;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeEmplazamientosAtributos.getTotalCount();
    }

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    if (pageSize < currentPage) {
        currentPage = 1;
        App.storeEmplazamientosAtributos.loadPage(currentPage);
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
    }
    else {
        App.btnPaginNext.setDisabled(false);
        App.btnPaginLast.setDisabled(false);
        App.btnPagingInit.setDisabled(false);
        App.btnPagingPre.setDisabled(false);
    }

    updateDisplayingAtributos(sender);
}

function updateDisplayingAtributos(sender) {
    var txtDisplaying = "";
    var total = 0;
    var pageSize = App.storeEmplazamientosAtributos.pageSize;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storeEmplazamientosAtributos.getTotalCount();
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

    if (currentPage == getLastPageAtributos(sender)) {
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

function setNumMaxPageAtributos(sender) {
    updateDisplayingAtributos();
    updatePaginAtributos();
}
// #endRegion

// #region Buscador
var dataGridEmplazamientos = [];
function ajaxGetDatosBuscadorAtributos() {

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
                                    App.storeEmplazamientosAtributos.reload();

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

// #endRegion


function Grid_RowSelectEmplazamiento(sender, registro, index) {
}
function MostrarPanelMoreInfo(sender) {
    parent.CargarPanelMoreInfo(sender.$widgetRecord.data.EmplazamientoID, true);
}