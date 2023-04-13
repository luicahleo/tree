

// #region RESPONSIVE PAGINA

var PuntoCorteL = 900;
var PuntoCorteS = 512;
var selectedCol = 0;
var isOnMobC = 0;
var editar = true;

var DefaultRender = function (value) {
    if (value == "true" || value == 1) {
        return '<span class="ico-defaultGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}
var PrecioRender = function (sender, registro, value) {
    var dato = value.data;
    App.colPrecio.value = dato.Simbolo;
    return '<span>' + dato.Precio + ' ' + dato.Simbolo + '</span>';
}

// #region DISEÑO

var bShowPrincipal = true;
var bShowOnlySecundary = false;
var iSelectedPanel = 0;

function showPanelsByWindowSize() {

    let puntoCorte = 512;
    var tmn = App.CenterPanelMain.getWidth();

    if (tmn < puntoCorte) {
        App.tbFiltrosYSliders.show();
        App.btnPrev.show();
        App.btnNext.show();
        loadPanelByBtns("");
    }
    else {
        App.tbFiltrosYSliders.hide()
        App.btnPrev.hide();
        App.btnNext.hide();
        loadPanels();
    }
}

function loadPanels() {

    if (bShowOnlySecundary) {
        App.gridCatalogos.hide();
        App.btnCloseShowVisorTreeP.setIconCls('ico-moverow-gr');
    }
    else {

        App.pnCol1.show();

        if (bShowPrincipal) {
            App.gridCatalogos.show();
        }
        else {
            App.gridCatalogos.hide();
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
        App.gridCatalogos.show();
        App.pnCol1.hide();
    }
    if (iSelectedPanel == 1) {
        App.gridCatalogos.hide();
        App.pnCol1.show();
    }
}

function showOnlySecundary() {

    bShowOnlySecundary = !bShowOnlySecundary;
    loadPanels();
}

// #endregion

//  #region RESIZERS PARA VENTANAS MODALES (CALCULO EXTERNO)

function winFormCenterSimple(obj) {
    obj.center();
    obj.updateLayout();

}

function winFormResize(obj) {

    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(750);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }
    obj.center();
    obj.updateLayout();

}

function winFormResizeDockBot(obj) {

    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(750);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }
    obj.center();
    obj.updateLayout();

    //AQUI SE SETEA EL CENTER ABAJO

    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);
    obj.setY(vh - obj.height);

    obj.updateLayout();
}

function formResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(750);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(620);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
    obj.center();
    obj.updateLayout();

}

window.addEventListener('resize', function () {
    var dv = document.querySelectorAll('div.winForm-respSimple');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormCenterSimple(obj);
    }

    var dv = document.querySelectorAll('div.winForm-resp');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResize(obj);
    }

    var frm = document.querySelectorAll('div.ctForm-resp');
    for (i = 0; i < frm.length; i++) {
        var obj = Ext.getCmp(frm[i].id);
        formResize(obj);
    }

    var dv = document.querySelectorAll('div.winForm-respDockBot');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResizeDockBot(obj);
    }

    //ESCONDER CENTRO CUANDO ASIDE PISA MUCHO EL CONTENIDO PARA SER UTIL
    //var pnCentral = document.getElementsByClassName("pnCentralWrap");
    //var winsize = window.innerWidth;
    //var asideR = Ext.getCmp('pnAsideR');

    //if (winsize < 520 && asideR.collapsed == false) {
    //    App.CenterPanelMain.hide();
    //    App.pnAsideR.setWidth(winsize);
    //}
    //else {
    //    App.CenterPanelMain.show();
    //    App.pnAsideR.setWidth(380);
    //}

});

// #endregion

var handlePageSizeSelectServicios = function (item, records) {
    var curPageSize = App.storeServiciosAsignados.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeServiciosAsignados.pageSize = wantedPageSize;
        App.storeServiciosAsignados.load();
    }

}

// #endregion

//// #region PANEL LATERAL

//var panelAbierto = false;

//function hidePanelMoreInfoCatalog(panel, service, registro) {
//    let registroSeleccionado = panel.$widgetRecord;
//    let ColumnaSeleccionado = panel.$widgetColumn;
//    //App.btnCollapseAsR.show();

//    var asideR = Ext.getCmp('pnAsideR');

//    if (panelAbierto == false) {
//        App.btnCollapseAsR.show();
//        parent.App.pnAsideR.expand();
//    }

//    if (service == false) {
//        parent.App.pnMoreInfoCatalog.show();
//        parent.App.pnMoreInfoService.hide();
//        let grid = App.gridCatalogos;
//        cargarDatosPanelMoreInfoCatalogo(registroSeleccionado, grid);
//    }
//    else {
//        parent.App.pnMoreInfoCatalog.hide();
//        parent.App.pnMoreInfoService.show();
//        let grid = App.gridServiciosCatalogos;
//        cargarDatosPanelMoreInfoServicio(registroSeleccionado, grid);
//    }
//    //GridColHandler();



//    window.dispatchEvent(new Event('resizePlantilla'));
//}


//function cargarDatosPanelMoreInfoCatalogo(registro, Grid) {
//    html = '';
//    let tabla = document.getElementById('bodyTablaInfoElementos');
//    let grid;
//    tabla.innerHTML = "";

//    grid = Grid.columnManager.getColumns();

//    for (var columna of grid) {
//        if (columna.cls != 'NoOcultar col-More' && columna.cls != "excluirPnInfo") {
//            if (columna.config.text != undefined && (columna.renderer == false || columna.xtype == 'datecolumn')) {
//                if (registro.get(columna.dataIndex) != undefined && columna.xtype != 'datecolumn') {
//                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex) + '</span></td></tr>';
//                }
//                else if (registro.get(columna.dataIndex) != undefined && columna.xtype == 'datecolumn') {
//                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex).toLocaleDateString() + '</span></td></tr>';
//                }
//                else {
//                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd"></span></td></tr>';
//                }
//            }
//            else {
//                if (columna.tooltip != undefined) {
//                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.tooltip + ' : </span><span class="dataGrd">' + this[columna.renderer.name](columna.rendered) + '</span></td></tr>';
//                }
//                else if (columna.renderer.name.includes("render") || columna.renderer.name.includes("Render")) {
//                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.get(columna.dataIndex)) + '</span></td></tr>';
//                }
//                else {
//                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.get(columna.dataIndex)) + '</span></td></tr>';
//                }
//            }
//        }
//    }

//    tabla.innerHTML = html;
//}

//function cargarDatosPanelMoreInfoServicio(registro, Grid) {
//    html = '';
//    let tabla = document.getElementById('bodyTablaInfoElementosService');
//    let grid;
//    tabla.innerHTML = "";

//    grid = Grid.columnManager.getColumns();

//    for (var columna of grid) {
//        if (columna.cls != 'NoOcultar col-More' && columna.cls != "excluirPnInfo") {
//            if (columna.config.text != undefined && (columna.renderer == false || columna.xtype == 'datecolumn')) {
//                if (registro.get(columna.dataIndex) != undefined && columna.xtype != 'datecolumn') {
//                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex) + '</span></td></tr>';
//                }
//                else if (registro.get(columna.dataIndex) != undefined && columna.xtype == 'datecolumn') {
//                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + registro.get(columna.dataIndex).toLocaleDateString() + '</span></td></tr>';
//                }
//                else {
//                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd"></span></td></tr>';
//                }
//            }
//            else {
//                if (columna.tooltip != undefined) {
//                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.tooltip + ' : </span><span class="dataGrd">' + this[columna.renderer.name](columna.rendered) + '</span></td></tr>';
//                }
//                else if (columna.renderer.name.includes("render") || columna.renderer.name.includes("Render")) {
//                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.get(columna.dataIndex)) + '</span></td></tr>';
//                }
//                else {
//                    html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + columna.text + ' : </span><span class="dataGrd">' + this[columna.renderer.name](registro.get(columna.dataIndex)) + '</span></td></tr>';
//                }
//            }
//        }
//    }

//    tabla.innerHTML = html;
//}

//function hideAsideR(panel) {

//    //App.btnCollapseAsRClosed.show();

//    var asideR = Ext.getCmp('pnAsideR');

//    parent.App.pnAsideR.collapse();
//    App.btnCollapseAsR.hide();
//    panelAbierto = false;
//    //GridColHandler();

//    window.dispatchEvent(new Event('resizePlantilla'));
//}

//// #endregion

// #region CONTROL NAVEGACION WIN product catalog

var TabN = 1;
var classActivo = "navActivo";
var classBtnActivo = "btn-ppal-winForm";
var classBtnDesactivo = "btn-secondary-winForm";

function getPanelActual(sender, registro, index) {
    var panelActual;
    var panels = App.TbNavegacionTabs.ariaEl.getFirstChild().getFirstChild().dom.children;
    for (let i = 0; i < panels.length; i++) {
        let cmp = Ext.getCmp(panels[i].id);
        if (document.getElementById(cmp.id).lastChild.classList.contains(classActivo)) {
            panelActual = i;
        }
    }
    return panelActual;
}

var clase = 'navActivo';

function NavegacionWinGestion(who) {
    App.pnFormProductCatalogPacks.hide();
    App.pnServicios.hide();
    if (who != undefined) {
        var LNo = who.textEl;



        switch (who.id) {
            case 'lnkProduct':
                ChangeTab(LNo);
                App.pnFormProductCatalogPacks.show();
                var i = 0;

                var j = 0;
                Ext.each(App.pnFormProductCatalogPacks.query('*'), function (value) {
                    var c = Ext.getCmp(value.id);

                    if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                        if (!c.isValid()) {
                            j++;
                        }
                    }
                });

                if (j == 0) {
                    App.lnkProduct.removeCls(classMandatory);
                }
                else {
                    App.lnkProduct.addCls(classMandatory);
                }
                break;

            case 'lnkServicios':
                if (App.pnFormProductCatalogPacks.getForm().isValid()) {
                    FormularioValidoCatalogo(null, false);
                    App.storeProductCatalogServicios.reload()
                    ChangeTab(LNo);
                    App.pnFormProductCatalogPacks.hide();
                    App.pnServicios.show();
                } else {
                    NavegacionWinGestion();
                }
                break;
            default:

                ChangeTab(LNo);
                App.pnFormProductCatalogPacks.show();
                break;
        }
    } else {

        ChangeTab();
        document.getElementById('lnkProduct').lastChild.classList.add(clase);
        App.pnFormProductCatalogPacks.show();
    }

}
function ChangeTab(vago) {
    let ct = document.getElementById('TbNavegacionTabs-innerCt');
    let aLinks = ct.querySelectorAll('a');

    aLinks.forEach(function (itm) {
        itm.classList.remove("navActivo");
    });

    if (vago != undefined) {
        vago.classList.add('navActivo');
    }

}


// #endregion

// #region GRID PRODUCTCATALOG
var seleccionado;

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    App.btnEditar.setDisabled(false);
    App.btnEliminar.setDisabled(false);
    App.btnRefrescarServicio.setDisabled(false);
    App.btnDescargarServicios.setDisabled(false);


    if (datos != null) {

        seleccionado = datos;
        App.hdProductCatalogID.setValue(seleccionado.Code);
        if (parent.App.pnAsideR.collapsed == false) {
            let registroSeleccionado = registro;
            let GridSeleccionado = App.gridCatalogos;
            parent.App.pnMoreInfoCatalog.hide();
            parent.App.pnMoreInfoService.hide();
            parent.App.pnMoreInfoPack.show();

            parent.App.btnClausulas.hide();
            parent.App.btnLink.hide();
            parent.App.btnPrecios.hide();
            parent.App.btnEstados.hide();
            //parent.cargarDatosPanelMoreInfoCatalogo(registroSeleccionado, App.storePrincipal, '');


            parent.displayMenu('pnMoreInfoPack', App.storePrincipal, registroSeleccionado);
        }
        App.storeServiciosAsignados.reload()

    }

}

function DeseleccionarGrilla() {
    App.hdProductCatalogID.setValue(0);
    App.GridRowSelect.clearSelections();
    App.btnEditar.setDisabled(true);
    App.btnEliminar.setDisabled(true);
    App.btnRefrescarServicio.setDisabled(true);
    App.btnDescargarServicios.setDisabled(true);

}

function refrescar() {
    App.GridRowSelect.clearSelections();
    nfPaginNumberFirstload = true;
    forzarCargaBuscadorPredictivo = true;
    App.btnEditar.setDisabled(true);
    App.btnEliminar.setDisabled(true);
    App.storePrincipal.reload();
    App.storeServiciosAsignados.reload();
}

// #region BUSCADOR

function filtrarCatalogosPorBuscador(sender, registro, index) {
    if (registro.charCode == 13) {
        App.hdStringBuscador.setValue(sender.value);
        App.storePrincipal.reload();
    }
}

function LimpiarFiltroBusqueda(sender, registro) {

    App.hdStringBuscador.setValue("");
    App.hdIDCatalogoBuscador.setValue("");
    App.txtSearch.setValue("");

    App.storePrincipal.clearFilter();
    App.storePrincipal.reload();
    //App._mnuDwldExcel.enable();

}
var dataGridCatalogos = [];

function ajaxGetDatosBuscadorCatalogos() {

    updatePaginCatalogos()

    if (dataGridCatalogos.length == 0 || forzarCargaBuscadorPredictivo) {
        dataGridCatalogos = [];

        TreeCore.GetDatosBuscador({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    result.Result.forEach(serv => {
                        dataGridCatalogos.push({
                            key: serv.Nombre,
                            key2: serv.Codigo,
                            nombre: serv.Nombre,
                            codigo: serv.Codigo,
                            id: serv.CoreProductCatalogPackID
                        });
                    });

                    dataGridCatalogos = dataGridCatalogos.sort(function (a, b) {
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
                                let results = $.grep(dataGridCatalogos, function (value) {
                                    let value1 = value.key;
                                    let value2 = value.key2;

                                    return matcher.test(value1) || matcher.test(normalize(value1)) || matcher.test(value2) || matcher.test(normalize(value2));
                                });

                                response(results.slice(0, 10));

                                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                                    var idServicioBuscador = $(e.currentTarget).attr("data-emplazamientoID");
                                    App.hdStringBuscador.setValue("");
                                    App.hdIDCatalogoBuscador.setValue(idServicioBuscador);
                                    App.storePrincipal.reload();

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



var nfPaginNumberFirstload = true;
var currentPage = 1;

var pageSelect = function (item) {

    let store = this.up('gridpanel').store;

    var curPageSize = store.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        store.pageSize = wantedPageSize;
        store.load({
            callback: function (r, options, success) {
                nfPaginNumberFirstload = true;
                updatePaginCatalogos();
            }
        });
    }
}

function pagingInitCatalogos() {
    currentPage = 1;
    App.storePrincipal.loadPage(currentPage);
    updatePaginCatalogos();
}
function pagingPreCatalogos() {
    currentPage--;
    App.storePrincipal.loadPage(currentPage);
    updatePaginCatalogos();
}

function paginGoToCatalogos(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageCatalogos()) {
        currentPage = sender.value;
        App.storePrincipal.loadPage(sender.value);
        updatePaginCatalogos();
    }
}

function paginNextCatalogos() {
    currentPage++;
    App.storePrincipal.loadPage(currentPage);
    updatePaginCatalogos();
}

function paginLastCatalogos() {

    currentPage = getLastPageCatalogos();
    App.storePrincipal.loadPage(currentPage);
    updatePaginCatalogos();

}

function nfPaginNumberBeforeRenderCatalogos(sender, registro) {
    if (nfPaginNumberFirstload) {

        nfPaginNumberFirstload = false;

        App.nfPaginNumber.setValue(currentPage);
    }
}

function updatePaginCatalogos() {
    let lastPage = getLastPageCatalogos();
    let total = 0;
    var pageSize = App.storePrincipal.pageSize;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storePrincipal.getTotalCount();
    }

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    if (pageSize < currentPage) {
        currentPage = 1;
        App.storePrincipal.loadPage(currentPage);
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

    updateDisplayingCatalogos();
}

function updateDisplayingCatalogos() {
    var txtDisplaying = "";
    var total = 0;
    let lastPage = getLastPageCatalogos();
    var pageSize = App.storePrincipal.pageSize;

    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storePrincipal.getTotalCount();
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

    if (currentPage == getLastPageCatalogos()) {
        lastItem = total;
    }

    if (total == 0) {
        txtDisplaying = jsSinDatosMostrar;
    }
    else {
        txtDisplaying = `${jsMostrando} ${firstItem} - ${lastItem} ${jsDe} ${total}`;
    }

    App.lbDisplaying.setText(txtDisplaying);
    forzarCargaBuscadorPredictivo = true;
}

function setNumMaxPageCatalogos() {
    updateDisplayingCatalogos();
    updatePaginCatalogos();
}

function getLastPageCatalogos() {
    let total = 0;
    if (App.hdTotalCountGrid.value) {
        total = App.hdTotalCountGrid.value;
    } else {
        total = App.storePrincipal.getTotalCount();
    }

    let pageSize = App.storePrincipal.pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

// #endregion
// #endregion

// #region AGREGAR/EDITAR/ELIMINAR ProductCatalog

var Agregar = false;

// #region AGREGAR/EDITAR CATALOGO

function AgregarEditar(sender, registro, index) {
    TreeCore.GenerarCodigoCatalogo(
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    pautasCodigo = result.Result;
                    GenerarCodigo(App.txtCodigo, App.pnFormProductCatalogPacks, pautasCodigo, App.hdCodigoCatalogoAutogenerado);
                }
            }
        });
    VaciarFormularioCatalogo();
    //App.storeCoreFrecuencias.reload();
    //App.storeCoreUnidades.reload();


    var combos = [App.cmbEntidad, App.cmbProductCatalogTipo];
    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.winGestion.setTitle(jsAgregar);
                App.lnkServicios.disable();
                //App.storeProductCatalogServicios.reload();
                //App.txtFechaInicio.show();
                //App.txtFechaInicio.enable();
                //App.btnAgregarProductCatalogPacks.hide();
                App.winGestion.show();
                Agregar = true;
                load.hide()
            }

        });
    })

}
var ContadorIntentos;
function winGestionGuardar() {
    ContadorIntentos = 100;
    showLoadMask(App.winGestion, function (load) {
        TreeCore.ComprobarCatalogoExiste(
            {
                success: function (result) {
                    if (result.Result == 'Codigo') {
                        Ext.Msg.alert(
                            {
                                title: jsControlCodigo,
                                msg: jsComprobarCodigoGenerado,
                                buttons: Ext.Msg.YESNO,
                                buttonText: {
                                    no: jsCodigoManual,
                                    yes: jsGenerarCodigo
                                },
                                fn: ajaxGenerarNuevoCodigo,
                                cls: 'winFormEditado',
                                width: '500px'
                            });
                        load.hide();

                    }
                    else if (result.Result != null && result.Result != '') {
                        if (App.pnFormProductCatalogPacks.getForm().isValid()) {
                            ajaxAgregarEditar();
                        }
                        else {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
                            //NavegacionWinGestion();

                        }
                    }
                    else {
                        ajaxAgregarEditar();

                    }

                    load.hide();
                }
            });
    });

}

function ajaxAgregarEditar() {
    showLoadMask(App.vwResp, function (load) {
        TreeCore.AgregarEditar(Agregar,
            {
                success: function (result) {
                    if (result.Result == 'Codigo') {
                        Ext.Msg.alert(
                            {
                                title: jsControlCodigo,
                                msg: jsComprobarCodigoGenerado,
                                buttons: Ext.Msg.YESNO,
                                buttonText: {
                                    no: jsCodigoManual,
                                    yes: jsGenerarCodigo
                                },
                                fn: ajaxGenerarNuevoCodigo,
                                cls: 'winFormEditado',
                                width: '500px'
                            });
                    }
                    else if (result.Result != null && !result.Success) {
                        //NavegacionWinGestion();
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                    }
                    else {
                        //App.storeProductCatalogServicios.reload();
                        //forzarCargaBuscadorPredictivo = true;
                        //App.lnkServicios.enable();

                        App.btnAgregarProductCatalogPacks.setText(jsGuardado);
                        App.btnAgregarProductCatalogPacks.removeCls("animation-text");
                        App.btnAgregarProductCatalogPacks.setIconCls("ico-tic-wh");
                        App.btnAgregarProductCatalogPacks.disable();
                        App.lnkServicios.setDisabled(false);
                        TreeCore.MostrarEditar(
                            {
                                success: function (result) {
                                    if (result.Result != null && result.Result != '') {
                                        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                                    }
                                    Agregar = false;
                                    App.winGestion.setTitle(jsEditar);
                                    App.btnAgregarProductCatalogPacks.setText(jsGuardado);
                                    App.lnkServicios.enable();
                                    App.btnAgregarProductCatalogPacks.disable();
                                    App.winGestion.show();

                                    editar = false;
                                    load.hide();
                                },
                                eventMask:
                                {
                                    showMask: true,
                                    msg: jsMensajeProcesando
                                }
                            }
                        );


                        setTimeout(function () {
                            App.btnAgregarProductCatalogPacks.addCls("animation-text");
                        }, 250);
                    }
                    load.hide();
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    });
}

function MostrarEditar(sender, registro, index) {
    if (registroSeleccionado(App.gridCatalogos) && seleccionado != null) {
        ajaxEditar();
    }
}

function cambiarLiteral() {
    App.btnAgregarProductCatalogPacks.enable();
    App.btnAgregarProductCatalogPacks.setText(jsGuardar);
    App.btnAgregarProductCatalogPacks.setIconCls("");
    App.btnAgregarProductCatalogPacks.removeCls("btnDisableClick");
    App.btnAgregarProductCatalogPacks.addCls("btnEnableClick");
    App.btnAgregarProductCatalogPacks.removeCls("animation-text");
}

function ajaxEditar(sender, registro, index) {
    VaciarFormularioCatalogo();
    Agregar = false;
    App.winGestion.setTitle(jsEditar);
    var combos = [App.cmbEntidad, App.cmbProductCatalogTipo];
    //App.storeProductCatalogServicios.reload()
    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                            }
                            App.btnAgregarProductCatalogPacks.setText(jsGuardado);
                            App.btnAgregarProductCatalogPacks.setDisabled(true);
                            //App.txtFechaInicio.disable();
                            App.lnkServicios.enable();
                            App.lnkProduct.removeCls(classMandatory);
                            App.lnkProduct.setDisabled(false);
                            App.winGestion.show();

                            editar = false;
                            load.hide();
                        },
                        eventMask:
                        {
                            showMask: true,
                            msg: jsMensajeProcesando
                        }
                    }
                );
            }

        });
    });
}

// #region GENERAR CODIGO

var pautasCodigo;

function ajaxComprobarHdEditado() {
    if (ContadorIntentos == 0) {
        Ext.Msg.alert(
            {
                title: jsControlCodigo,
                msg: jsLimiteIntentosCodigo,
                buttons: Ext.Msg.OK,
                cls: 'winFormEditado',
                width: '500px'
            });
    }
}

function ajaxGenerarNuevoCodigo(sender) {

    if (sender == "yes") {

        TreeCore.GenerarCodigoCatalogo({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    pautasCodigo = result.Result;

                    GenerarCodigoDuplicado(App.txtCodigo, App.pnFormProductCatalogPacks, pautasCodigo, App.hdCodigoCatalogoAutogenerado);

                    ContadorIntentos = ContadorIntentos - 1;

                    TreeCore.ComprobarCodigoCatalogoDuplicado(
                        {
                            success: function (result) {
                                if (result.Result != null && result.Result != '') {
                                    App.txtCodigo.setValue(App.hdCodigoCatalogoAutogenerado.value.toString());
                                }
                            },
                            eventMask:
                            {
                                showMask: true,
                                msg: jsMensajeProcesando
                            }
                        });

                    ajaxComprobarHdEditado();
                }
            }
        });
    }
    else {
        App.txtCodigo.setValue("");
        App.txtCodigo.setEmptyText("");
    }
}


// #endregion



// #endregion

// #region ELIMINAR


function Eliminar() {
    if (registroSeleccionado(App.gridCatalogos) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminar(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.Eliminar({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    forzarCargaBuscadorPredictivo = true;
                    App.storePrincipal.reload();
                    App.storeServiciosAsignados.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

// #endregion

// #region RECARGAR COMBOS

function RecargarEmpresaProveedoraAsociada() {
    recargarCombos([App.cmbEntidad]);
}


// #endregion

function SeleccionarMoneda(sender, registro, index) {
    var simbolo = registro[0].data.Simbolo;

    App.txtPrecio;
    for (var i = 0; i < App.gridWinServicios.config.columns.items.length; i++) {
        var valor = App.gridWinServicios.config.columns.items[i];
        if (valor.xtype == "componentcolumn") {
            numberField = valor.component()[0];
            numberField.indicatorText = simbolo;
        }
    }


}



// #endregion

// #region CONTROL FORMULARIO

function VaciarFormularioCatalogo() {

    TreeCore.GenerarCodigoCatalogo(
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    pautasCodigo = result.Result;
                    GenerarCodigo(App.txtCodigo, App.pnFormProductCatalogPacks, pautasCodigo, App.hdCodigoCatalogoAutogenerado);
                }
            }
        });
    App.pnFormProductCatalogPacks.getForm().reset();
    NavegacionWinGestion();

    Ext.each(App.pnFormProductCatalogPacks.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.reset();

            if (c.triggerWrap != undefined) {
                c.triggerWrap.removeCls("itemForm-novalid");
                c.triggerWrap.addCls("itemForm-valid");
            }

            if (!c.allowBlank && c.xtype != "checkboxfield") {
                c.addListener("change", anadirClsNoValido, false);
                c.addListener("focusleave", anadirClsNoValido, false);

                c.addListener("change", cambiarLiteral, false);
                c.addListener("validityChange", FormularioValidoCatalogo, false);

                c.addCls("ico-exclamacion-10px-red");
                c.removeCls("ico-exclamacion-10px-grey");
            }

            if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
            }
        }
    });

}

function cerrarWinGestion() {

    App.winGestion.hide();
    VaciarFormularioCatalogo();
    App.storePrincipal.reload();
    App.storeServiciosAsignados.reload();
    editar = true;
}

var classMandatory = "ico-formview-mandatory";
var i = 0;

// #region FORMEMPLAZAMIENTOS

function FormularioValidoCatalogo(sender, valid) {

    var i = 0;
    Ext.each(App.pnFormProductCatalogPacks.query('*'), function (value) {
        var c = Ext.getCmp(value.id);

        if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
            if (!c.isValid()) {
                i++;
            }
        }
    });

    if (i == 0) {
        App.lnkProduct.removeCls(classMandatory);
    }
    else {
        App.lnkProduct.addCls(classMandatory);
    }

    if (valid == true) {
        App.btnAgregarProductCatalogPacks.enable();
        Ext.each(App.pnFormProductCatalogPacks.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && !c.isHidden() && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnAgregarProductCatalogPacks.disable();
            }
        });
    } else {
        App.btnAgregarProductCatalogPacks.disable();
    }
}


// #endregion

// #region AGREGAR/ELIMINAR SERVICIO

function guardarServicio(dato) {
    var oDato = dato.record.data;
    if (oDato.CoreProductCatalogServicioPackAsignadoID != undefined) {
        App.hdServicioAsignadoID.setValue(oDato.CoreProductCatalogServicioPackAsignadoID);

    }
    App.hdServicioID.setValue(oDato.CoreProductCatalogServicioID);
    App.hdPrecio.setValue(oDato.Precio);
    if (dato.pressed == undefined) {
        TreeCore.AgregarServicio(true,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        App.storeProductCatalogServicios.reload();
                        App.btnAgregarProductCatalogPacks.removeCls("animation-text");
                        App.btnAgregarProductCatalogPacks.setIconCls("ico-tic-wh");
                        setTimeout(function () {
                            App.btnAgregarProductCatalogPacks.addCls("animation-text");
                        }, 250);
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    } else {
        TreeCore.AgregarServicio(dato.pressed,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        App.storeProductCatalogServicios.reload();
                        App.btnAgregarProductCatalogPacks.removeCls("animation-text");
                        App.btnAgregarProductCatalogPacks.setIconCls("ico-tic-wh");
                        setTimeout(function () {
                            App.btnAgregarProductCatalogPacks.addCls("animation-text");
                        }, 250);
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    }

}

var marcarCHK = function (value) {
    var oDato = value.record.data;
    if (oDato.CoreProductCatalogServicioPackAsignadoID != null && oDato.CoreProductCatalogServicioPackAsignadoID != 0) {
        Ext.getCmp(value.id).setPressed(true);
    }
    else {
        Ext.getCmp(value.id).setPressed(false);
    }
}

// #endregion


// #region GRID SERVICIO


function RefrescarServiciosAsignados() {
    App.storeServiciosAsignados.reload();
}

// #region BUSCADOR + FILTROS

function filtrarPorBuscadorServicios(sender, registro, index) {
    if (registro.charCode == 13) {
        App.hdStringBuscador2.setValue(sender.value);
        App.storeServiciosAsignados.reload();
    }
}

function BorrarFiltrosServicios(sender, registro) {
    var tree = App.gridServiciosCatalogos;
    tree.filters.clearFilters();
    LimpiarFiltroBusquedaServicios();
}

function LimpiarFiltroBusquedaServicios(sender, registro) {

    App.hdStringBuscador2.setValue("");
    App.hdIDCatalogoBuscador2.setValue("");
    App.txtSearch2.setValue("");

    App.storeServiciosAsignados.clearFilter();
    App.storeServiciosAsignados.reload();
}

var datagridServiciosCatalogos = [];

function ajaxGetDatosBuscadorServicios() {

    updatePaginServicios();

    if (datagridServiciosCatalogos.length == 0 || forzarCargaBuscadorPredictivo) {
        datagridServiciosCatalogos = [];

        TreeCore.GetDatosBuscador2({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    result.Result.forEach(serv => {
                        datagridServiciosCatalogos.push({
                            key: serv.Nombre.toLowerCase(),
                            key2: serv.Codigo.toLowerCase(),
                            nombre: serv.Nombre,
                            codigo: serv.Codigo,
                            id: serv.CoreProductCatalogServicioID
                        });
                    });

                    datagridServiciosCatalogos = datagridServiciosCatalogos.sort(function (a, b) {
                        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
                    });

                    var nameSearchBox = "txtSearch2";
                    var selectorSearchBox = `#${nameSearchBox}-inputEl`;

                    $(function () {
                        let textBuscado = "";
                        $(selectorSearchBox).autocomplete({
                            source: function (request, response) {
                                textBuscado = request.term;
                                var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                                let results = $.grep(datagridServiciosCatalogos, function (value) {
                                    let value1 = value.key;
                                    let value2 = value.key2;

                                    return matcher.test(value1) || matcher.test(normalize(value1)) || matcher.test(value2) || matcher.test(normalize(value2));
                                });

                                response(results.slice(0, 10));

                                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                                    var idServicioBuscador = $(e.currentTarget).attr("data-emplazamientoID");
                                    App.hdStringBuscador2.setValue("");
                                    App.hdIDCatalogoBuscador2.setValue(idServicioBuscador);
                                    App.storeServiciosAsignados.reload();

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

// #region PAGINACION

var nfPaginNumberFirstload2 = true;
var currentPage2 = 1;

var pageSelect2 = function (item) {

    let store = this.up('gridpanel').store;

    var curPageSize = store.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        store.pageSize = wantedPageSize;
        store.load({
            callback: function (r, options, success) {
                nfPaginNumberFirstload = true;
                updatePaginServicios();
            }
        });
    }
}

function nfPaginNumberBeforeRenderServicios(sender, registro) {
    if (nfPaginNumberFirstload2) {

        nfPaginNumberFirstload2 = false;

        App.nfPaginNumber2.setValue(currentPage2);
    }
}

function pagingInitServicios() {
    currentPage2 = 1;
    App.storeServiciosAsignados.loadPage(currentPage2);
    updatePaginServicios();
}

function pagingPreServicios() {
    currentPage2--;
    App.storeServiciosAsignados.loadPage(currentPage2);
    updatePaginServicios();
}

function paginGoToServicios(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageServicios()) {
        currentPage2 = sender.value;
        App.storeServiciosAsignados.loadPage(sender.value);
        updatePaginServicios();
    }
}

function paginNextServicios() {
    currentPage2++;
    App.storeServiciosAsignados.loadPage(currentPage2);
    updatePaginServicios();
}

function paginLastServicios() {

    currentPage2 = getLastPageServicios();
    App.storeServiciosAsignados.loadPage(currentPage2);
    updatePaginServicios();
}

function getLastPageServicios() {
    let total = 0;
    if (App.hdTotalCountGrid2.value) {
        total = App.hdTotalCountGrid2.value;
    } else {
        total = App.storeServiciosAsignados.getTotalCount();
    }

    let pageSize = App.storeServiciosAsignados.pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

function updatePaginServicios() {
    let lastPage = getLastPageServicios();
    let total = 0;
    var pageSize = App.storeServiciosAsignados.pageSize;

    if (App.hdTotalCountGrid2.value) {
        total = App.hdTotalCountGrid2.value;
    } else {
        total = App.storeServiciosAsignados.getTotalCount();
    }

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    if (pageSize < currentPage2) {
        currentPage2 = 1;
        App.storeServiciosAsignados.loadPage(currentPage2);
    }

    App.nfPaginNumber2.setValue(`${currentPage2}`);
    App.lbNumberPages2.setText(`${lastPage}`);

    if (currentPage2 == 1 && lastPage == 1) {
        App.btnPagingInit2.setDisabled(true);
        App.btnPagingPre2.setDisabled(true);
        App.btnPaginNext2.setDisabled(true);
        App.btnPaginLast2.setDisabled(true);
    }
    else if (currentPage2 <= 1) {
        App.btnPagingInit2.setDisabled(true);
        App.btnPagingPre2.setDisabled(true);
        App.btnPaginNext2.setDisabled(false);
        App.btnPaginLast2.setDisabled(false);
    }
    else if (currentPage2 >= lastPage) {
        App.btnPaginNext2.setDisabled(true);
        App.btnPaginLast2.setDisabled(true);
        App.btnPagingInit2.setDisabled(false);
        App.btnPagingPre2.setDisabled(false);
    } else {
        App.btnPaginNext2.setDisabled(false);
        App.btnPaginLast2.setDisabled(false);
        App.btnPagingInit2.setDisabled(false);
        App.btnPagingPre2.setDisabled(false);
    }

    updateDisplayingServicios();
}

function updateDisplayingServicios() {
    var txtDisplaying = "";
    var total = 0;
    let lastPage = getLastPageServicios();
    var pageSize = App.storeServiciosAsignados.pageSize;

    if (App.hdTotalCountGrid2.value) {
        total = App.hdTotalCountGrid2.value;
    } else {
        total = App.storeServiciosAsignados.getTotalCount();
    }

    lastPage = total / pageSize;
    if (lastPage % 1 != 0) {
        lastPage = Math.trunc(lastPage);
        lastPage++;
    }

    if (lastPage < currentPage2) {
        currentPage2 = 1;
    }

    let firstItem = ((currentPage2 - 1) * pageSize) + 1;
    let lastItem = currentPage2 * pageSize;

    if (currentPage2 == getLastPageServicios()) {
        lastItem = total;
    }

    if (total == 0) {
        txtDisplaying = jsSinDatosMostrar;
    }
    else {
        txtDisplaying = `${jsMostrando} ${firstItem} - ${lastItem} ${jsDe} ${total}`;
    }

    App.lbDisplaying2.setText(txtDisplaying);
    forzarCargaBuscadorPredictivo = true;
}

function setNumMaxPageServicios() {
    updateDisplayingServicios();
    updatePaginServicios();
}

// #endregion

function filtrarPorBuscadorFormulario(sender, registro, index) {
    if (registro.charCode == 13) {
        App.hdStringBuscador3.setValue(sender.value);
        App.storeProductCatalogServicios.reload();
    }
}

function BorrarFiltrosFormulario(sender, registro) {
    var tree = App.gridServiciosCatalogos;
    tree.filters.clearFilters();
    LimpiarFiltroBusquedaFormulario();
}

function LimpiarFiltroBusquedaFormulario(sender, registro) {

    App.hdStringBuscador3.setValue("");
    App.hdIDCatalogoBuscador3.setValue("");
    App.txtSearch3.setValue("");

    App.storeProductCatalogServicios.clearFilter();
    App.storeProductCatalogServicios.reload();
}

var dataGridFormulario = [];

function ajaxGetDatosBuscadorFormulario(sender, registro, index) {

    //if (App.txtSearch3.value != null && App.txtSearch3.value != '') {
    //    App.hdStringBuscador3.setValue(sender.value);
    //}


    if (dataGridFormulario.length == 0 || forzarCargaBuscadorPredictivo) {
        dataGridFormulario = [];

        TreeCore.GetDatosBuscador3({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    result.Result.forEach(serv => {
                        dataGridFormulario.push({
                            key: serv.NombreCatalogServicio,
                            nombre: serv.NombreCatalogServicio,
                            id: serv.CoreProductCatalogServicioID
                        });
                    });

                    dataGridFormulario = dataGridFormulario.sort(function (a, b) {
                        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
                    });

                    var nameSearchBox = "txtSearch3";
                    var selectorSearchBox = `#${nameSearchBox}-inputEl`;

                    $(function () {
                        let textBuscado = "";
                        $(selectorSearchBox).autocomplete({
                            source: function (request, response) {
                                textBuscado = request.term;
                                var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                                let results = $.grep(dataGridFormulario, function (value) {
                                    let value1 = value.key;

                                    return matcher.test(value1) || matcher.test(normalize(value1));
                                });

                                response(results.slice(0, 10));

                                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                                    var idServicioBuscador = $(e.currentTarget).attr("data-emplazamientoID");
                                    App.hdStringBuscador3.setValue("");
                                    App.hdIDCatalogoBuscador3.setValue(idServicioBuscador);
                                    App.storeProductCatalogServicios.reload();

                                });
                            }
                        }).autocomplete("instance")._renderItem = function (ul, item) {
                            let title = boldQuery(item.nombre, textBuscado);
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


function grid_rowSelectServicios(sender, registro, index) {


    if (parent.App.pnAsideR.collapsed == false) {
        let registroSeleccionado = registro;
        let GridSeleccionado = App.gridServiciosCatalogos;
        parent.App.pnMoreInfoCatalog.hide();
        parent.App.pnMoreInfoService.show();
        parent.App.btnClausulas.hide();
        parent.App.btnLink.hide();
        parent.App.btnPrecios.hide();
        parent.App.MenuNavPnServicios.updateLayout();

        parent.App.btnEstados.removeCls('btnDisableClick');
        parent.App.btnEstados.addCls('btnEnableClick');

        parent.cargarDatosPanelMoreInfoServicio(registroSeleccionado, GridSeleccionado);
        parent.cargarDatosPanelLateral(registroSeleccionado.data.CoreProductCatalogServicioID);

        //parent.displayMenu('pnServicios', GridSeleccionado, registroSeleccionado);
    }
}

// #endregion

// #region PANEL LATERAL




function cargarDatosPanelMoreInfoGridServicio(tabla, lista, grid) {
    html = '';
    tabla.innerHTML = "";
    let nombreGrid;

    //if (slickCreado) {
    //    $('.single-item').slick('unslick');
    //}

    //$('.single-item').empty();

    if (grid != undefined) {
        nombreGrid = grid.columnManager.getColumns();

        for (var i = 0; i < lista.length; i++) {

            html += '<div class="tmpCol-td" colspan = "3">';

            for (var prop of Object.keys(lista[i])) {
                for (var columna of nombreGrid) {
                    if (columna.cls != 'col-More' && columna.cls != "excluirPnInfo") {
                        if (columna.config.text != undefined && (columna.renderer == false || columna.xtype == 'datecolumn')) {
                            if (prop == columna.dataIndex) {
                                html += '<div><span class="lblGrd">' + columna.config.text + ' : </span><span class="dataGrd">' + lista[i][prop] + '</span></div>';
                            }
                        }
                    }
                }
            }

            html += '</div>';
        }

        tabla.innerHTML = html;

        //if (label != undefined && label != "" && label != null && lista.length != 0) {
        //    $('.single-item').on('init', function (event, slick, currentSlide, nextSlide) {
        //        label.setText(1 + '/' + lista.length);
        //    });

        //    $('.single-item').on('beforeChange', function (event, slick, currentSlide, nextSlide) {
        //        label.setText((nextSlide + 1) + '/' + lista.length);
        //    });
        //}

        //$('.single-item').slick({
        //    dots: false,
        //    infinite: true,
        //    arrows: true,
        //    slidesToShow: 1,
        //    slidesToScroll: 1,
        //});

        //slickCreado = true;
    }
}

// #endregion


function RecargarProductCatalogTipo() {
    recargarCombos([App.cmbProductCatalogTipo]);
}


//#region ICONOS BUSCADOR

function FieldSearch(sender, registro) {
    var iconClear = sender.getTrigger("_trigger2");
    iconClear.hide();
}

function FiltrarColumnas(sender, registro) {

    var iconSearch = sender.getTrigger("_trigger1");
    var iconClear = sender.getTrigger("_trigger2");
    var text = sender.getRawValue();

    if (Ext.isEmpty(text, false)) {
        iconClear.hide();
        iconSearch.show();
    }

    if (!Ext.isEmpty(text, false)) {
        iconSearch.hide();
        iconClear.show();
    }
}

//#endregion

//function pulsoRadio(sender, registro, index) {
//    if (registro.FrecuenciasUnidades == 1) {
//        App.cmbFrecuencias.enable();
//        App.cmbFrecuencias.allowBlank = false;

//        App.cmbUnidades.disable();
//        App.cmbUnidades.reset();
//        App.cmbUnidades.allowBlank = true;
//        App.cmbUnidades.removeCls("ico-exclamacion-10px-grey");
//        App.cmbUnidades.removeCls("ico-exclamacion-10px-red");
//        App.cmbUnidades.triggerWrap.removeCls("itemForm-novalid");
//        App.cmbUnidades.removeListener("change", FormularioValidoCatalogo, false);

//        App.cmbFrecuencias.addListener("change", anadirClsNoValido, false);
//        App.cmbFrecuencias.addListener("focusleave", anadirClsNoValido, false);
//        App.cmbFrecuencias.addListener("change", FormularioValidoCatalogo, false);

//        App.cmbFrecuencias.addCls("ico-exclamacion-10px-red");
//    }
//    else if (registro.FrecuenciasUnidades == 2) {
//        App.cmbUnidades.enable();
//        App.cmbUnidades.allowBlank = false;

//        App.cmbFrecuencias.reset();
//        App.cmbFrecuencias.disable();
//        App.cmbFrecuencias.allowBlank = true;
//        App.cmbFrecuencias.removeCls("ico-exclamacion-10px-grey");
//        App.cmbFrecuencias.removeCls("ico-exclamacion-10px-red");
//        App.cmbFrecuencias.triggerWrap.removeCls("itemForm-novalid");
//        App.cmbFrecuencias.removeListener("change", FormularioValidoCatalogo, false);

//        App.cmbUnidades.addListener("change", anadirClsNoValido, false);
//        App.cmbUnidades.addListener("focusleave", anadirClsNoValido, false);
//        App.cmbUnidades.addListener("change", FormularioValidoCatalogo, false);

//        App.cmbUnidades.addCls("ico-exclamacion-10px-red");
//    }
//    else if (index == 1) {
//        App.cmbFrecuencias.enable();
//        App.cmbFrecuencias.allowBlank = false;

//        App.cmbUnidades.disable();
//        App.cmbUnidades.reset();
//        App.cmbUnidades.allowBlank = true;
//        App.cmbUnidades.removeCls("ico-exclamacion-10px-grey");
//        App.cmbUnidades.removeCls("ico-exclamacion-10px-red");
//        App.cmbUnidades.triggerWrap.removeCls("itemForm-novalid");
//        App.cmbUnidades.removeListener("change", FormularioValidoCatalogo, false);

//        App.cmbFrecuencias.addListener("change", anadirClsNoValido, false);
//        App.cmbFrecuencias.addListener("focusleave", anadirClsNoValido, false);
//        App.cmbFrecuencias.addListener("change", FormularioValidoCatalogo, false);

//        App.cmbFrecuencias.addCls("ico-exclamacion-10px-red");
//    }
//}

//function RecargarFrecuencia() {
//    recargarCombos([App.cmbFrecuencias]);
//    FormularioValidoCatalogo('', false);
//}

//function SeleccionarFrecuencia() {
//    App.cmbFrecuencias.getTrigger(0).show();
//    FormularioValidoCatalogo('', true);
//}

//function RecargarUnidad() {
//    recargarCombos([App.cmbUnidades]);
//    FormularioValidoCatalogo('', false);
//}

//function SeleccionarUnidad() {
//    App.cmbUnidades.getTrigger(0).show();
//    FormularioValidoCatalogo('', true);
//}