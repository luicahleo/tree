function DeseleccionarGrillaEmplazamientosAtributos (sender, registro, index) {
    var ruta = sender.config.storeId.split('_');
    ruta.pop();
    ruta = ruta.join('_');

    App[ruta + '_' + 'GridRowSelectEmplazamientosAtributos'].clearSelections();
}

function Grid_RowSelectEmplazamientosAtributos(sender, registro, index) {
    var ruta = sender.config.proxyId.split('_');
    ruta.pop();
    ruta = ruta.join('_');
}

function refrescarEmplazamientosAtributos(sender, registro, index) {
    ajaxAplicarFiltro(filtrosAplicados);
}

function mostrarFiltros(sender, registro, index) {
    var idComponente = sender.id.split('_')[0];
    var ControlURL = '/Componentes/toolbarFiltros.ascx';
    var NombreControl = 'toolbarFiltros';
    var achjs = ControlURL.split('/');
    achjs.pop(); achjs = achjs.join('/') + '/js/' + NombreControl + '.js';
    AñadirScriptjs(achjs);
    App[idComponente + "_hdControlURL"].value = ControlURL;
    App[idComponente + "_hdControlName"].value = NombreControl;
    TreeCore[idComponente].LoadUserControl(ControlURL, NombreControl, true,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    setClienteIDComponentes();
                    App[idComponente + "_hugeCt"].show();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

function cargarFiltros(sender, registro, index) {
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


//#region FILTROS

function FiltrarColumnasEmplazamientosAtributos(sender, registro) {

    var idComponente = sender.id.split('_');
    idComponente.pop();
    App[idComponente + "_btnDescargar"].disable();
    var tree = App[idComponente + "_grdEmplazamientosAtributos"];
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


function LimpiarFiltroBusquedaEmplazamientosAtributos(sender, registro) {
    
    var idComponente = sender.id.split('_');
    idComponente.pop();

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App[idComponente + "_txtSearch"].setValue("");

    logic.clearFilter();
    App[idComponente + "_btnDescargar"].enable();
}


function BorrarFiltrosEmplazamientosAtributos(sender, registro) {
    var idComponente = sender.id.split('_');
    idComponente.pop();
    var tree = App[idComponente + "_grdEmplazamientosAtributos"];
    tree.filters.clearFilters();

    //LimpiarFiltroBusqueda(sender, "");

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

function nfPaginNumberBeforeRenderAtributos(sender, registro) {
    let ruta = getIdComponente(sender);
    if (nfPaginNumberFirstload) {

        nfPaginNumberFirstload = false;

        App[`${ruta}_nfPaginNumber`].setValue(currentPage);

        //updateDisplaying();
    }
}

function pagingInitAtributos(sender) {
    let ruta = getIdComponente(sender);
    currentPage = 1;
    App[`${ruta}_storeEmplazamientosAtributos`].loadPage(currentPage);
    updatePaginAtributos();
}

function pagingPreAtributos(sender) {
    let ruta = getIdComponente(sender);
    currentPage--;
    App[`${ruta}_storeEmplazamientosAtributos`].loadPage(currentPage);
    updatePaginAtributos();
}

function paginGoToAtributos(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageAtributos()) {
        let ruta = getIdComponente(sender);
        currentPage = sender.value;
        App[`${ruta}_storeEmplazamientosAtributos`].loadPage(sender.value);
        updatePaginAtributos();
    }
}

function paginNextAtributos(sender) {
    let ruta = getIdComponente(sender);
    currentPage++;
    App[`${ruta}_storeEmplazamientosAtributos`].loadPage(currentPage);
    updatePaginAtributos();
}

function paginLastAtributos(sender) {
    let ruta = getIdComponente(sender);
    currentPage = getLastPageAtributos(sender);
    App[`${ruta}_storeEmplazamientosAtributos`].loadPage(currentPage);
    updatePaginAtributos();
}

function getLastPageAtributos(sender) {
    let ruta;
    if (sender) {
        ruta = getIdComponente(sender);
    }
    else {
        ruta = "UCGridEmplazamientosAtributos";
    }
    let total = 0;
    if (App[`${ruta}_hdTotalCountGrid`].value) {
        total = App[`${ruta}_hdTotalCountGrid`].value;
    } else {
        total = App[`${ruta}_storeEmplazamientosAtributos`].getTotalCount();
    }

    let pageSize = App[`${ruta}_storeEmplazamientosAtributos`].pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

function updatePaginAtributos(sender) {
    let lastPage = getLastPageAtributos(sender);
    let ruta;
    if (sender) {
        ruta = getIdComponente(sender);
    }
    else {
        ruta = "UCGridEmplazamientosAtributos";
    }
    let total = 0;
    if (App[`${ruta}_hdTotalCountGrid`].value) {
        total = App[`${ruta}_hdTotalCountGrid`].value;
    } else {
        total = App[`${ruta}_storeEmplazamientosAtributos`].getTotalCount();
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

    updateDisplayingAtributos(sender);
}

function updateDisplayingAtributos(sender) {
    var txtDisplaying = "";
    var total = 0;
    let ruta;
    if (sender) {
        ruta = getIdComponente(sender);
    }
    else {
        ruta = "UCGridEmplazamientosAtributos";
    }

    var pageSize = App[`${ruta}_storeEmplazamientosAtributos`].pageSize;

    if (App[`${ruta}_hdTotalCountGrid`].value) {
        total = App[`${ruta}_hdTotalCountGrid`].value;
    } else {
        total = App[`${ruta}_storeEmplazamientosAtributos`].getTotalCount();
    }

    if (total / pageSize < currentPage) {
        currentPage = 1;
    }

    let firstItem = ((currentPage - 1) * pageSize) + 1;
    let lastItem = currentPage * pageSize;

    if (currentPage == getLastPageAtributos(sender)) {
        lastItem = total;
    }

    txtDisplaying = `${jsMostrando} ${firstItem} - ${lastItem} ${jsDe} ${total}`;
    App[`${ruta}_lbDisplaying`].setText(txtDisplaying);
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

        TreeCore["UCGridEmplazamientosAtributos"].GetDatosBuscador({
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
                                        nombre: emp.NombreSitio,
                                        codigo: emp.Codigo,
                                        id: emp.EmplazamientoID
                                    });


                                }
                            }
                        });
                    });

                    dataGridEmplazamientos = dataGridEmplazamientos.sort(function (a, b) {
                        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
                    });

                    var idComponente = "UCGridEmplazamientosAtributos";
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

                                    return matcher.test(value1) || matcher.test(normalize(value1)) || matcher.test(value2) || matcher.test(normalize(value2));
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

function CargarGridAtributos(sender) {
    let ruta = getIdComponente(sender);
    TreeCore[ruta].CargarGrid();
}
