// #region RESPONSIVE PAGINA

var PuntoCorteL = 900;
var PuntoCorteS = 512;
var selectedCol = 0;
var isOnMobC = 0;
var editar = true;
const fecha = new Date();

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
    App.colPrecio.value = dato.Price;
    return '<span>' + dato.Price + ' ' + dato.Currency + '</span>';
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
    }
    else {

        App.pnServiciosCatalogo.show();

        if (bShowPrincipal) {
            App.gridCatalogos.show();
        }
        else {
            App.gridCatalogos.hide();
        }

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
        App.pnServiciosCatalogo.hide();
    }
    if (iSelectedPanel == 1) {
        App.gridCatalogos.hide();
        App.pnServiciosCatalogo.show();
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

// #region CONTROL NAVEGACION

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

    App.ctFormProduct.hide();
    App.pnReajustes.hide();
    App.pnServicios.hide();

    if (who != undefined) {
        var LNo = who.textEl;

        switch (who.id) {
            case 'lnkProduct':
                ChangeTab(LNo);
                App.ctFormProduct.show();
                App.pnReajustes.hide();
                var i = 0;

                Ext.each(App.pnReajustes.query('*'), function (value) {
                    var c = Ext.getCmp(value.id);

                    if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                        if (!c.isValid()) {
                            i++;
                        }
                    }
                });

                if (i == 0) {
                    if (App.cmpReajustes_RBSinIncremento.checked == true) {
                        App.lnkUpdate.removeCls(classMandatory);
                    }
                    else {
                        App.cmpReajustes_txtFechaProxima.setDisabled(true);
                        if (App.cmpReajustes_txtFechaInicioRevision.enable() == true && App.cmpReajustes_txtFechaInicioRevision.value >= fecha) {
                            if (!App.cmpReajustes_txtFechaFinRevision.value == null) {

                                if (App.cmpReajustes_txtFechaFinRevision.value >= fecha && App.cmpReajustes_txtFechaFinRevision.value >= App.cmpReajustes_txtFechaInicioRevision.value) {
                                    App.lnkUpdate.removeCls(classMandatory);
                                }
                            }
                            else {
                                App.lnkUpdate.removeCls(classMandatory);
                            }
                        }
                        else if (App.cmpReajustes_txtFechaInicioRevision.enable() == true && !App.cmpReajustes_txtFechaInicioRevision.value >= fecha) {
                            App.lnkUpdate.addCls(classMandatory);
                        }
                        else if (App.cmpReajustes_txtFechaFinRevision.value >= fecha && App.cmpReajustes_txtFechaFinRevision.value >= App.cmpReajustes_txtFechaInicioRevision.value) {
                            App.lnkUpdate.removeCls(classMandatory);
                        }
                        else if (App.cmpReajustes_txtFechaInicioRevision.enable() == false && App.cmpReajustes_txtFechaFinRevision.value == null) {
                            App.lnkUpdate.removeCls(classMandatory);
                        }
                    }

                    if (App.lnkProduct.ariaEl.dom.classList.contains('ico-formview-mandatory') || App.lnkUpdate.ariaEl.dom.classList.contains('ico-formview-mandatory')) {
                        App.btnAgregarProductCatalog.setDisabled(true);
                    }
                    else {
                        App.btnAgregarProductCatalog.setDisabled(false);
                        cambiarLiteralboton();
                    }
                }
                else {
                    App.lnkUpdate.addCls(classMandatory);
                }
                if (App.txtFechaInicio.value < fecha) {
                    App.txtFechaFin.setMinValue(fecha);
                }
                var j = 0;
                Ext.each(App.pnFormProductCatalog.query('*'), function (value) {
                    var c = Ext.getCmp(value.id);

                    if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                        if (!c.isValid()) {
                            j++;
                        }
                    }
                });

                if (j == 0) {

                    if (App.txtFechaFin.value >= fecha && App.txtFechaFin.value >= App.txtFechaInicio.value) {
                        App.lnkProduct.removeCls(classMandatory);
                    }
                    else if (App.txtFechaFin.value == null) {
                        App.lnkProduct.removeCls(classMandatory);
                    }
                }
                else {
                    App.lnkProduct.addCls(classMandatory);
                }
                break;

            case 'lnkUpdate':
                var i = 0;
                Ext.each(App.pnReajustes.query('*'), function (value) {
                    var c = Ext.getCmp(value.id);

                    if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                        if (!c.isValid()) {
                            i++;
                        }
                    }
                });

                if (i == 0) {
                    if (App.cmpReajustes_RBSinIncremento.checked == true) {
                        App.lnkUpdate.removeCls(classMandatory);
                    }
                    else {
                        App.cmpReajustes_txtFechaProxima.setDisabled(true);
                        if (App.cmpReajustes_txtFechaInicioRevision.enable() == true && App.cmpReajustes_txtFechaInicioRevision.value >= fecha) {
                            if (!App.cmpReajustes_txtFechaFinRevision.value == null) {

                                if (App.cmpReajustes_txtFechaFinRevision.value >= fecha && App.cmpReajustes_txtFechaFinRevision.value >= App.cmpReajustes_txtFechaInicioRevision.value) {
                                    App.lnkUpdate.removeCls(classMandatory);
                                }
                            }
                            else {
                                App.lnkUpdate.removeCls(classMandatory);
                            }
                        }
                        else if (App.cmpReajustes_txtFechaInicioRevision.enable() == true && !App.cmpReajustes_txtFechaInicioRevision.value >= fecha) {
                            App.lnkUpdate.addCls(classMandatory);
                        }
                        else if (App.cmpReajustes_txtFechaInicioRevision.enable() == false && App.cmpReajustes_txtFechaFinRevision.value == null) {
                            App.lnkUpdate.removeCls(classMandatory);
                        }
                    }

                    if (App.lnkProduct.ariaEl.dom.classList.contains('ico-formview-mandatory') || App.lnkUpdate.ariaEl.dom.classList.contains('ico-formview-mandatory')) {
                        App.btnAgregarProductCatalog.setDisabled(true);
                    }
                    else {
                        App.btnAgregarProductCatalog.setDisabled(false);
                        cambiarLiteralboton();
                    }
                }
                else {
                    App.lnkUpdate.addCls(classMandatory);
                }
                if (App.txtFechaInicio.value < fecha) {
                    App.txtFechaFin.setMinValue(fecha);
                }
                var j = 0;
                Ext.each(App.pnFormProductCatalog.query('*'), function (value) {
                    var c = Ext.getCmp(value.id);

                    if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                        if (!c.isValid()) {
                            j++;
                        }
                    }
                });

                if (j == 0) {

                    if (App.txtFechaFin.value >= fecha && App.txtFechaFin.value >= App.txtFechaInicio.value && App.txtFechaInicio.value >= fecha) {
                        App.lnkProduct.removeCls(classMandatory);
                    }
                    else if (App.txtFechaFin.value == null && App.txtFechaInicio.value >= fecha) {
                        App.lnkProduct.removeCls(classMandatory);
                    }
                }
                else {
                    App.lnkProduct.addCls(classMandatory);
                }

                addlistenerValidacionFormReajustes();
                ChangeTab(LNo);
                App.ctFormProduct.hide();
                App.pnReajustes.show();
                break;
            case 'lnkServicios':
                App.cmpColumn.resumeEvents();
                App.storeProductCatalogServicios.reload()
                ChangeTab(LNo);
                App.ctFormProduct.hide();
                App.pnServicios.show();
                break;
            default:
                ChangeTab(LNo);
                App.ctFormProduct.show();
                break;
        }
    }
    else {

        ChangeTab();
        document.getElementById('lnkProduct').lastChild.classList.add(clase);
        App.ctFormProduct.show();
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

// #region GRID CATALOG
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
        //if (parent.App.pnAsideR.collapsed == false) {
        //    let registroSeleccionado = registro;
        //    let GridSeleccionado = App.gridCatalogos;
        //    parent.App.pnMoreInfoCatalog.show();
        //    parent.App.pnMoreInfoService.hide();

        //    parent.App.btnClausulas.removeCls('btnEnableClick');
        //    parent.App.btnClausulas.addCls('btnDisableClick');
        //    parent.displayMenu('pnMoreInfoCatalog', App.storePrincipal, registroSeleccionado);
        //}
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

    App.hdIDCatalogoBuscador2.setValue("");
    App.hdProductCatalogID.setValue("");
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
                            key: serv.Name.toLowerCase(),
                            key2: serv.Code.toLowerCase(),
                            nombre: serv.Name,
                            codigo: serv.Code,
                            id: serv.CoreProductCatalogID
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

// #region AGREGAR/EDITAR/ELIMINAR

var Agregar = false;

// #region AGREGAR/EDITAR CATALOGO

function AgregarEditar(sender, registro, index) {

    App.txtFechaInicio.allowBlank = false;
    App.txtFechaInicio.setMinValue(fecha);

    //TreeCore.GenerarCodigoCatalogo(
    //    {
    //        success: function (result) {
    //            if (!result.Success) {
    //                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
    //            } else {
    //                pautasCodigo = result.Result;
    //                GenerarCodigo(App.txtCodigo, App.pnFormProductCatalog, pautasCodigo, App.hdCodigoCatalogoAutogenerado);
    //            }
    //        }
    //    });
    VaciarFormularioCatalogo();
    reiniciarFormulario("cmpReajustes");

    var combos = [App.cmbProductCatalogTipo, App.cmbMonedas, App.cmbEstadosGlobales];
    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.winGestion.setTitle(jsAgregar);
                App.txtFechaInicio.setMinValue(fecha);
                App.txtFechaInicio.setMaxValue(null);
                App['cmpReajustes_txtFechaInicioRevision'].enable();
                App.storeProductCatalogServicios.reload();
                App.txtFechaInicio.show();
                App.txtFechaInicio.enable();
                App.winGestion.show();
                Agregar = true;
                load.hide()
            }
        });
    });
}
var ContadorIntentos;
function winGestionGuardar() {
    ContadorIntentos = 100;
    showLoadMask(App.winGestion, function (load) {
        ajaxAgregarEditar();
        load.hide();
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
                    else if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                    }
                    else {
                        App.btnAgregarProductCatalog.setText(jsGuardado);
                        App.btnAgregarProductCatalog.removeCls("animation-text");
                        App.btnAgregarProductCatalog.removeCls("btnEnableClick");
                        App.btnAgregarProductCatalog.addCls("btnDisableClick");
                        App.btnAgregarProductCatalog.setIconCls("ico-tic-wh");
                        Agregar = false;
                        App.txtFechaInicio.disable();
                        App.lnkServicios.enable();
                        if (!App.cmpReajustes_RBSinIncremento.checked == true) {
                            App.cmpReajustes_txtFechaProxima.show();
                            App.cmpReajustes_txtFechaProxima.setDisabled(true);
                        }
                        App.storePrincipal.reload();
                        App.lnkProduct.setDisabled(false);
                        editar = false;
                        load.hide();
                        setTimeout(function () {
                            App.btnAgregarProductCatalog.addCls("animation-text");
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
    App.btnAgregarProductCatalog.setText(jsGuardar);
    App.btnAgregarProductCatalog.setIconCls("");
    App.btnAgregarProductCatalog.removeCls("btnDisableClick");
    App.btnAgregarProductCatalog.addCls("btnEnableClick");
    App.btnAgregarProductCatalog.removeCls("animation-text");

    if (App.lnkProduct.ariaEl.dom.classList.contains('ico-formview-mandatory') || App.lnkUpdate.ariaEl.dom.classList.contains('ico-formview-mandatory')) {
        App.btnAgregarProductCatalog.setDisabled(true);
    }
    else {
        App.btnAgregarProductCatalog.setDisabled(false);
    }
}

function cambiarLiteralboton() {
    App.btnAgregarProductCatalog.setText(jsGuardar);
    App.btnAgregarProductCatalog.setIconCls("");
    App.btnAgregarProductCatalog.removeCls("btnDisableClick");
    App.btnAgregarProductCatalog.addCls("btnEnableClick");
    App.btnAgregarProductCatalog.removeCls("animation-text");
}

function ajaxEditar(sender, registro, index) {
    VaciarFormularioCatalogo();
    Agregar = false;
    App.winGestion.setTitle(jsEditar);
    App.txtFechaInicio.allowBlank = true;
    App.txtFechaInicio.setMinValue(null);
    var combos = [App.cmbProductCatalogTipo, App.cmbMonedas, App.cmbEstadosGlobales];
    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                            }
                            App.txtFechaInicio.disable();
                            App.lnkServicios.enable();
                            App.lnkProduct.setDisabled(false);
                            FormularioValido(null, true);
                            App.btnAgregarProductCatalog.setText(jsGuardado);
                            App.btnAgregarProductCatalog.removeCls("animation-text");
                            App.btnAgregarProductCatalog.removeCls("btnEnableClick");
                            App.btnAgregarProductCatalog.addCls("btnDisableClick");
                            App.btnAgregarProductCatalog.setIconCls("ico-tic-wh");
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

                    GenerarCodigoDuplicado(App.txtCodigo, App.pnFormProductCatalog, pautasCodigo, App.hdCodigoCatalogoAutogenerado);

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
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

// #endregion

// #region RECARGAR COMBOS

function RecargarMonedas() {
    recargarCombos([App.cmbMonedas]);
}
function RecargarProductCatalogTipo() {
    recargarCombos([App.cmbProductCatalogTipo]);
}

function RecargarEstadosGlobales() {
    recargarCombos([App.cmbEstadosGlobales]);
}

// #endregion

// #endregion

// #region CONTROL FORMULARIO

function VaciarFormularioCatalogo() {

    //TreeCore.GenerarCodigoCatalogo(
    //    {
    //        success: function (result) {
    //            if (!result.Success) {
    //                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
    //            } else {
    //                pautasCodigo = result.Result;
    //                GenerarCodigo(App.txtCodigo, App.pnFormProductCatalog, pautasCodigo, App.hdCodigoCatalogoAutogenerado);
    //            }
    //        }
    //    });
    App.pnFormProductCatalog.getForm().reset();
    NavegacionWinGestion();
    ruta = 'cmpReajustes';

    App[ruta + '_' + 'txtFechaInicioRevision'].setValue('');
    App[ruta + '_' + 'txtFechaProxima'].setValue('');
    App[ruta + '_' + 'txtFechaFinRevision'].setValue('');
    App[ruta + '_' + 'cmbInflaciones'].setValue(undefined);
    App[ruta + '_' + 'txtCadencia'].setValue(undefined);
    App[ruta + '_' + 'txtPorcentaje'].setValue(undefined);
    App[ruta + '_' + 'txtCantidad'].setValue(undefined);

    Ext.each(App.pnFormProductCatalog.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.reset();

            if (c.triggerWrap != undefined) {
                c.triggerWrap.removeCls("itemForm-novalid");
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

    App.hdStringBuscador3.setValue("");
    App.hdIDCatalogoBuscador3.setValue("");
    App.txtSearch3.setValue("");

    App.hdStringBuscadorPack.setValue("");
    App.hdIDPackBuscador.setValue("");
}

function cerrarWinGestion() {

    if (App.btnAgregarProductCatalog.getText() == jsGuardar && !App.btnAgregarProductCatalog.disabled) {
        Ext.Msg.alert(
            {
                title: jsAtencion,
                msg: jsMensajeCerrar,
                buttons: Ext.Msg.YESNO,
                fn: function (btn) {
                    if (btn == 'yes' || btn == 'si') {
                        if (App.winGestion.title.startsWith(jsEditar)) {
                            Agregar = false;

                        } else if (App.winGestion.title.startsWith(jsAgregar)) {
                            Agregar = true;

                        }

                        ajaxAgregarEditar();
                    }
                    App.winGestion.hide();
                    VaciarFormularioCatalogo();
                    App.cmpColumn.suspendEvents();
                    App.storePrincipal.reload();
                    App.storeServiciosAsignados.reload();
                    editar = true;
                }
            });
    }
    else {
        App.winGestion.hide();
        VaciarFormularioCatalogo();
        App.cmpColumn.suspendEvents();
        App.storePrincipal.reload();
        App.storeServiciosAsignados.reload();
        editar = true;
    }
}

var classMandatory = "ico-formview-mandatory";
var i = 0;

function FormularioValidoCatalogo(sender, valid) {

    if (App.txtFechaInicio.value < fecha) {
        App.txtFechaFin.setMinValue(fecha);
    }

    var i = 0;
    Ext.each(App.pnFormProductCatalog.query('*'), function (value) {
        var c = Ext.getCmp(value.id);

        if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
            if (!c.isValid()) {
                i++;
            }
        }
    });

    if (i == 0) {

        if (App.txtFechaFin.value >= fecha && App.txtFechaFin.value >= App.txtFechaInicio.value) {
            App.lnkProduct.removeCls(classMandatory);

            if (App.lnkProduct.ariaEl.dom.classList.contains('ico-formview-mandatory') || App.lnkUpdate.ariaEl.dom.classList.contains('ico-formview-mandatory')) {
                App.btnAgregarProductCatalog.setDisabled(true);
            }
            else {
                App.btnAgregarProductCatalog.setDisabled(false);
                cambiarLiteralboton();
            }
        }
        else if (App.txtFechaFin.value == null) {
            App.lnkProduct.removeCls(classMandatory);

            if (App.lnkProduct.ariaEl.dom.classList.contains('ico-formview-mandatory') || App.lnkUpdate.ariaEl.dom.classList.contains('ico-formview-mandatory')) {
                App.btnAgregarProductCatalog.setDisabled(true);
            }
            else {
                App.btnAgregarProductCatalog.setDisabled(false);
                cambiarLiteralboton();
            }
        }
    }
    else {
        App.lnkProduct.addCls(classMandatory);
    }

    if (valid == true) {
        if (App.txtFechaFin.isValid() && App.txtFechaFin.value != null) {
            App.lnkProduct.removeCls(classMandatory);
        }

        if (App.lnkProduct.ariaEl.dom.classList.contains('ico-formview-mandatory') || App.lnkUpdate.ariaEl.dom.classList.contains('ico-formview-mandatory')) {
            App.btnAgregarProductCatalog.setDisabled(true);
        }
        else {
            App.btnAgregarProductCatalog.setDisabled(false);
            cambiarLiteralboton();
        }

        Ext.each(App.pnFormProductCatalog.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && !c.isHidden() && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnAgregarProductCatalog.disable();
            }
        });
    } else {
        if (!App.txtFechaInicio.isValid() || !App.txtFechaFin.isValid()) {
            App.btnAgregarProductCatalog.disable();
            App.lnkProduct.addCls(classMandatory);
        }
    }
}

function FormularioValido(sender, valid) {
    var i = 0;
    Ext.each(App.pnReajustes.query('*'), function (value) {
        var c = Ext.getCmp(value.id);

        if (c && !c.isHidden() && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
            if (!c.isValid()) {
                i++;
            }
        }
    });

    if (i == 0) {
        App.lnkUpdate.removeCls(classMandatory);

        if (App.cmpReajustes_RBSinIncremento.checked == true) {
            App.lnkUpdate.removeCls(classMandatory);
        }
        else {
            App.cmpReajustes_txtFechaProxima.setDisabled(true);
            if (App.cmpReajustes_txtFechaInicioRevision.enable() == true && App.cmpReajustes_txtFechaInicioRevision.value >= fecha) {
                if (!App.cmpReajustes_txtFechaFinRevision.value == null) {

                    if (App.cmpReajustes_txtFechaFinRevision.value >= fecha && App.cmpReajustes_txtFechaFinRevision.value >= App.cmpReajustes_txtFechaInicioRevision.value) {
                        App.lnkUpdate.removeCls(classMandatory);
                    }
                }
                else {
                    App.lnkUpdate.removeCls(classMandatory);
                }
            }
            else if (App.cmpReajustes_txtFechaInicioRevision.enable() == true && !App.cmpReajustes_txtFechaInicioRevision.value >= fecha) {
                App.lnkUpdate.addCls(classMandatory);
            }
            else if (App.cmpReajustes_txtFechaFinRevision.value >= fecha && App.cmpReajustes_txtFechaFinRevision.value >= App.cmpReajustes_txtFechaInicioRevision.value) {
                App.lnkUpdate.removeCls(classMandatory);
            }
            else if (App.cmpReajustes_txtFechaInicioRevision.enable() == false && App.cmpReajustes_txtFechaFinRevision.value == null) {
                App.lnkUpdate.removeCls(classMandatory);
            }

        }

        if (App.lnkProduct.ariaEl.dom.classList.contains('ico-formview-mandatory') || App.lnkUpdate.ariaEl.dom.classList.contains('ico-formview-mandatory')) {
            App.btnAgregarProductCatalog.setDisabled(true);
        }
        else {
            App.btnAgregarProductCatalog.setDisabled(false);
            cambiarLiteralboton();
        }
    }
    else {
        App.lnkUpdate.addCls(classMandatory);
    }

    Ext.each(App.pnReajustes.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && !c.isHidden() && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {

            App.btnAgregarProductCatalog.setDisabled(true);
            App.lnkUpdate.addCls(classMandatory);
        }
    });
}

function addlistenerValidacionFormReajustes() {

    Ext.each(App.pnReajustes.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.addListener('change', FormularioValido);

            if (App.cmpReajustes_RBSinIncremento.checked == true) {
                App.lnkUpdate.removeCls(classMandatory);
            }
            else {
                App.cmpReajustes_txtFechaProxima.setDisabled(true);
                if (App.cmpReajustes_txtFechaInicioRevision.enable() == true && App.cmpReajustes_txtFechaInicioRevision.value >= fecha) {
                    if (!App.cmpReajustes_txtFechaFinRevision.value == null) {

                        if (App.cmpReajustes_txtFechaFinRevision.value >= fecha && App.cmpReajustes_txtFechaFinRevision.value >= App.cmpReajustes_txtFechaInicioRevision.value) {
                            App.lnkUpdate.removeCls(classMandatory);
                        }
                    }
                    else {
                        App.lnkUpdate.removeCls(classMandatory);
                    }
                }
                else if (App.cmpReajustes_txtFechaInicioRevision.enable() == true && !App.cmpReajustes_txtFechaInicioRevision.value >= fecha) {
                    App.lnkUpdate.addCls(classMandatory);
                }
                else if (App.cmpReajustes_txtFechaInicioRevision.enable() == false && App.cmpReajustes_txtFechaFinRevision.value == null) {
                    App.lnkUpdate.removeCls(classMandatory);
                }
            }
        }
    });
}

// #endregion

// #region AGREGAR/ELIMINAR SERVICIO

function guardarServicio(dato) {
    var oDato = dato.record.data;
    if (oDato.CodeProduct != undefined) {
        App.hdServicioAsignadoID.setValue(App.hdServicioAsignadoID.getValue() + ',' + oDato.CodeProduct + ":" + oDato.Price);
    }
}

var marcarCHK = function (column, cmp, record) {
    var bAsign = record.get('EsAsignado');

    if (bAsign) {
        cmp.setPressed(true);
    }
    else {
        cmp.setPressed(false);
    }

    cmp.updateLayout();
}

// #endregion

// #region GRID SERVICIO


function RefrescarServiciosAsignados() {
    App.hdEsPack.setValue("");
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
                            key: serv.NombreCatalogServicio.toLowerCase(),
                            nombre: serv.NombreCatalogServicio,
                            id: serv.CoreProductCatalogServicioID,
                            idPack: serv.CoreProductCatalogPackID,
                            bPack: serv.esPack
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

                                    return matcher.test(value1) || matcher.test(normalize(value1));
                                });

                                response(results.slice(0, 10));

                                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {

                                    App.hdStringBuscador2.setValue("");

                                    $.grep(datagridServiciosCatalogos, function (value) {
                                        var idServicioBuscador = $(e.currentTarget).attr("data-emplazamientoID");

                                        if (idServicioBuscador != "") {
                                            App.hdIDCatalogoBuscador2.setValue(idServicioBuscador);
                                        }
                                        else {
                                            App.hdEsPack.setValue(value.bPack);
                                            App.hdIDCatalogoBuscador2.setValue(value.idPack);
                                        }
                                    });

                                    App.storeServiciosAsignados.reload();

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
    App.hdEsPack.setValue("");
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

function ajaxGetDatosBuscadorFormulario() {


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

        if (registro.data.CoreProductCatalogServicioID != "") {
            parent.displayMenu('pnMoreInfoService', App.storeServiciosAsignados, registroSeleccionado);
        }
        else {
            parent.displayMenu('pnMoreInfoPack', App.storeServiciosAsignados, registroSeleccionado);
        }
    }
}

// #endregion

// #region PANEL LATERAL

function cargarDatosPanelMoreInfoGridServicio(tabla, lista, grid) {
    html = '';
    tabla.innerHTML = "";
    let nombreGrid;

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
    }
}

// #endregion

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