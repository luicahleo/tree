
function ClickShowProjectsD(sender, registro, index) {

    var idComponente = sender.record.store.config.storeId.split('_')[0];

    App[idComponente + "_WinProjectsDetails"].showAt(getActualXY(App[idComponente + "_WinProjectsDetails"], registro.event));
    App[idComponente + "_WinProjectsDetails"].focus();

    showLoadMask(App[idComponente + "_GridProjectsD"], function (load) {

        TreeCore[idComponente].MostrarProyectosAsignados(sender.record.get('EmplazamientoID'),
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        load.hide();
                    }
                }
            });
    });



}

function ClickShowContractD(sender, registro, index) {

    var idComponente = sender.record.store.config.storeId.split('_')[0];

    App[idComponente + "_WinContractDetails"].showAt(getActualXY(App[idComponente + "_WinContractDetails"], registro.event));
    App[idComponente + "_WinContractDetails"].focus();

    showLoadMask(App[idComponente + "_GridContractsD"], function (load) {

        TreeCore[idComponente].MostrarContratosAsignados(sender.record.get('EmplazamientoID'),
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        load.hide();
                    }
                }
            });
    });
}

function HideWinContract() {

    var idComponente = this.config.id.split('_')[0];
    App[idComponente + "_WinContractDetails"].hide();
}

function HideWinProjects() {
    var idComponente = this.config.id.split('_')[0];
    App[idComponente + "_WinProjectsDetails"].hide();
}

function actualizar() {
    //this.up('gridpanel').store.reload();
    //forzarCargaBuscadorPredictivo = true;
    //ajaxAplicarFiltro(filtrosAplicados);

    nfPaginNumberFirstload = true;
    currentPage = 1;
    showForms(App.lnkSites, '../../Componentes/GridEmplazamientos.ascx', 'GridEmplazamientos', '/Componentes/js/GridEmplazamientos.js');
}

var pageSelect = function (item) {

    let store = this.up('gridpanel').store;

    var curPageSize = store.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        store.pageSize = wantedPageSize;
        store.load({
            callback: function (r, options, success) {
                nfPaginNumberFirstload = true;
                updatePaginEmplazamientos();
            }
        });
    }
}

function limpiar(value) {
    value.setValue("");
    App["UCGridEmplazamientos_storeContactosGlobalesEmplazamientosVinculados"].reload();
}

function CargarGrid() {
    this.up('gridpanel').store.reload();
}

function Grid_RowSelectEmplazamiento(sender, registro, index) {
    var ruta = sender.config.proxyId.split('_');
    ruta.pop();
    ruta = ruta.join('_');

    if (sender.selected.items.length == 1) {
        App[ruta + '_' + 'btnEditar'].enable();
        App[ruta + '_btnContactos'].enable();
        if (parent != null) {
            App[ruta + '_' + 'btnEditar'].setTooltip(parent.jsEditar);
            App[ruta + '_btnContactos'].setTooltip(parent.jsContacto);
        }
        App[ruta + '_hdEmplazamientoSeleccionado'].setValue(registro.data.EmplazamientoID);
    } else {
        App[ruta + '_' + 'btnEditar'].disable();
        App[ruta + '_btnContactos'].disable();
    }
}

function DeseleccionarGrilla(sender, registro, index) {
    var ruta = sender.config.proxyId.split('_');
    ruta.pop();
    ruta = ruta.join('_');

    if (sender.selected.items.length == 1) {
        App[ruta + '_' + 'btnEditar'].enable();
        App[ruta + '_btnContactos'].enable();
        App[ruta + '_hdEmplazamientoSeleccionado'].setValue(registro.data.EmplazamientoID);
    }
    else {

        if (App.hdCliID.value == 0 ||
            App.hdCliID.value == undefined) {
            App[ruta + '_' + 'btnAnadir'].disable();
        } else {
            App[ruta + '_' + 'btnAnadir'].enable();
        }

        App[ruta + '_' + 'btnEditar'].disable();
        App[ruta + '_' + 'btnContactos'].disable();

        if (sender.selected.items.length == 0) {
            App[ruta + '_' + 'GridRowSelect'].clearSelections();
            App[ruta + '_hdEmplazamientoSeleccionado'].setValue("");
        }
    }
}

// #region Formulario Emplazamientos

function agregar(sender, registro, inde) {

    var idComponente = sender.id.split('_')[0];
    App[idComponente + "_winGestion"].setTitle(jsAgregar + " " + jsEmplazamiento);
    App[idComponente + "_winGestion"].show();

    showLoadMask(App[idComponente + "_winGestion"], function (load) {

        cambiarATapEmplazamiento(idComponente + '_formAgregarEditar', 0);
        VaciarFormularioEmplazamiento(idComponente + '_formAgregarEditar');
        CargarVentanaEmplazamiento(sender, 'formAgregarEditar', true, function Fin(fin) {
            load.hide();
        });

    });

}

function editar(sender, registro, index) {

    var idComponente = getIdComponente(sender);

    App[idComponente + "_winGestion"].setTitle(jsEditar + " " + jsEmplazamiento);
    App[idComponente + "_winGestion"].show();

    showLoadMask(App[idComponente + "_winGestion"], function (load) {

        VaciarFormularioEmplazamiento(idComponente + '_formAgregarEditar');
        cambiarATapEmplazamiento(idComponente + '_formAgregarEditar', 0);
        
        TreeCore[idComponente].MostrarEditar(App[idComponente + '_GridRowSelect'].selected.items[0].data.EmplazamientoID,
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    } else {
                        for (var prop in result.Result) {
                            SeleccionarValorMostrarEditar(prop, result.Result[prop]);
                        }
                        CargarVentanaEmplazamiento(sender, 'formAgregarEditar', false, function Fin(fin) {
                            load.hide();
                        });
                    }
                }
            });
            
    });

}

// #endregion

// #region Contactos

function agregarContactoEmplazamiento(sender, registro, index) {
    var idComponente = sender.id.split('_')[0];

    var empID = App[idComponente + "_GridRowSelect"].selected.items[0].data.EmplazamientoID;
    App[idComponente + '_hdEmplazamientoSeleccionado'].setValue(empID);
    App[idComponente + "_winGestionContactoEmplazamiento"].setTitle(jsAgregar);
    App[idComponente + "_winGestionContactoEmplazamiento"].show();

    VaciarFormularioContacto(idComponente + '_formAgregarEditarContactoEmplazamiento');
    cambiarATapContacto(idComponente + '_formAgregarEditarContactoEmplazamiento', 0);
    CargarVentanaContactos(sender, 'formAgregarEditarContactoEmplazamiento', true, function Fin(fin) { });

}

function editarContactoEmplazamiento(sender, registro, index) {
    var idComponente = this.$widgetColumn.id.split('_')[0];

    App[idComponente + "_winGestionContactoEmplazamiento"].setTitle(jsEditar);
    App[idComponente + "_winGestionContactoEmplazamiento"].show();

    showLoadMask(App[idComponente + "_winGestionContactoEmplazamiento"], function (load) {

        VaciarFormularioContacto(idComponente + '_formAgregarEditarContactoEmplazamiento');
        DefinirDatosContacto(idComponente, sender.$widgetRecord.data);
        cambiarATapContacto(idComponente + '_formAgregarEditarContactoEmplazamiento', 0);

        TreeCore[idComponente].MostrarEditarContacto(sender.$widgetRecord.data.ContactoGlobalID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        CargarVentanaContactos(sender, 'formAgregarEditarContactoEmplazamiento', true, function Fin(fin) {
                            load.hide();
                        });
                    }
                }
            });

    });
}

function Grid_RowSelectContactos(sender, registro, index) {
    var ruta = sender.config.proxyId.split('_');
    ruta.pop();
    ruta = ruta.join('_');
}

function closeWindowContactoEmplazamiento(sender, registro, index) {
    var ruta = getIdComponente(sender);
    App[ruta + "_winFormContacto"].hide();
}

var SetToggleValue = function (column, cmp, record) {

    var idComponente = record.store.config.storeId.split('_')[0];
    var lEmpSelecID = App[idComponente + '_GridRowSelect'].selected.items[0].data.EmplazamientoID;

    var lEmpID = record.get('EmplazamientoID');

    if (lEmpSelecID == lEmpID) {
        cmp.setPressed(true);
    }
    else {
        cmp.setPressed(false);
    }

    cmp.updateLayout();

}

function cambiarAsignacion(sender, registro, index) {

    var idComponente = sender.record.store.config.storeId.split('_')[0];

    TreeCore[idComponente].AsignarEmplazamiento(sender.record.get('ContactoGlobalID'),
        sender.record.get('EmplazamientoID'),
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App[idComponente + "_storeContactosGlobalesEmplazamientosVinculados"].reload();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

function buscador(sender, registro, index) {
    var idComponente = sender.id.split('_')[0];
    App[idComponente + "_storeContactosGlobalesEmplazamientosVinculados"].reload();
}

// #endregion

// #region Botón Derecho 

function ShowRightClickMenu(item, record, node, index, e) {

    var idComponente = record.store.config.storeId.split('_')[0];
    var menu = App[idComponente + "_ContextMenu"];

    e.preventDefault();
    menu.dataRecord = record.data;
    menu.showAt(getActualXY(menu, e));
}

function OpcionSeleccionada(sender, registro, event) {

    var opcion = registro.id;
    ruta = getIdComponente(sender);
    var emplazamientoID = sender.dataRecord.EmplazamientoID;
    var emplazamiento = sender.dataRecord.Codigo;

    if (opcion.includes("ShowMap")) {
        parent.addTab(parent.App.tabPpal, registro.text + emplazamientoID, registro.text + '-' + App[ruta + '_' + 'GridRowSelect'].selected.items[0].data.Codigo, "../../PaginasComunes/MapaEmplazamientosCercanos.aspx?EmplazamientoID=" + emplazamientoID)
    }

    if (opcion.includes("ShowInventory")) {
        parent.addTab(parent.App.tabPpal, registro.text + emplazamientoID, registro.text + '-' + App[ruta + '_' + 'GridRowSelect'].selected.items[0].data.Codigo, "../ModInventario/pages/InventarioGestionContenedor.aspx?EmplazamientoID=" + emplazamientoID);
    }

    if (opcion.includes("ShowHistorial")) {
        parent.addTab(parent.App.tabPpal, registro.text + emplazamientoID, registro.text + '-' + App[ruta + '_' + 'GridRowSelect'].selected.items[0].data.Codigo, "../ModGlobal/pages/EmplazamientosHistoricos.aspx?EmplazamientoID=" + emplazamientoID);
    }

    else if (opcion.includes("ShowDocuments")) {
        var pagina = "../PaginasComunes/DocumentosVista.aspx?ObjetoID=" + emplazamientoID + "&ObjetoTipo=Emplazamiento&ProyectoTipo=GLOBAL";
        parent.addTab(parent.App.tabPpal, registro.text + emplazamientoID, registro.text + " " + emplazamiento, pagina);
    }
}

// #endregion

//#region FILTROS

function FiltrarColumnas(sender, registro) {

    var idComponente = sender.id.split('_');
    idComponente.pop();
    App[idComponente + "_mnuDwldExcel"].disable();
    var tree = App[idComponente + "_gridEmplazamientos"];
    var store = tree.store,
        logic = store,
        text = sender.getRawValue();

    logic.clearFilter();

    // This will ensure after clearing the filter the auto-expanded nodes will be collapsed again
    //tree.collapseAll();

    if (Ext.isEmpty(text, false)) {
        App[idComponente + "_mnuDwldExcel"].enable();
        return;
    }

    filtroBuscador(logic, tree, text);
}


function LimpiarFiltroBusqueda(sender, registro) {
    var idComponente = sender.id.split('_');
    idComponente.pop();

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App[idComponente + "_txtSearch"].setValue("");
    
    logic.clearFilter();
    App[idComponente + "_mnuDwldExcel"].enable();

}

// #endregion

//Order column
/*
function showMenuOrderColumn() {
    let columns = App.UCGridEmplazamientos_gridEmplazamientos.columns;
    let destino = columns[3];
    let columnaAmover = columns[5]

    App.UCGridEmplazamientos_gridEmplazamientos.headerCt.moveAfter(destino, columnaAmover);
    //App.UCGridEmplazamientos_gridEmplazamientos.
}*/


//End Order column

function ClearFormulario(sender) {
    let ruta = getIdComponente(sender);
    VaciarFormularioEmplazamiento(ruta + '_' +'formAgregarEditar');
}

// #region Paginación
var nfPaginNumberFirstload = true;
var currentPage = 1;

function nfPaginNumberBeforeRenderEmplazamientos(sender, registro) {
    if (nfPaginNumberFirstload) {

        nfPaginNumberFirstload = false;

        App.UCGridEmplazamientos_nfPaginNumber.setValue(currentPage);

        //updateDisplaying();
    }
}

function pagingInitEmplazamientos() {
    currentPage = 1;
    App.UCGridEmplazamientos_storeEmplazamientos.loadPage(currentPage);
    updatePaginEmplazamientos();
}

function pagingPreEmplazamientos() {
    currentPage--;
    App.UCGridEmplazamientos_storeEmplazamientos.loadPage(currentPage);
    updatePaginEmplazamientos();
}

function paginGoToEmplazamientos(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageEmplazamientos()) {
        currentPage = sender.value;
        App.UCGridEmplazamientos_storeEmplazamientos.loadPage(sender.value);
        updatePaginEmplazamientos();
    }
}

function paginNextEmplazamientos() {
    currentPage++;
    App.UCGridEmplazamientos_storeEmplazamientos.loadPage(currentPage);
    updatePaginEmplazamientos();
}

function paginLastEmplazamientos() {
    
    currentPage = getLastPageEmplazamientos();
    App.UCGridEmplazamientos_storeEmplazamientos.loadPage(currentPage);
    updatePaginEmplazamientos();
}

function getLastPageEmplazamientos() {
    let total = 0;
    if (App.UCGridEmplazamientos_hdTotalCountGrid.value) {
        total = App.UCGridEmplazamientos_hdTotalCountGrid.value;
    } else {
        total = App.UCGridEmplazamientos_storeEmplazamientos.getTotalCount();
    }

    let pageSize = App.UCGridEmplazamientos_storeEmplazamientos.pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

function updatePaginEmplazamientos() {
    let lastPage = getLastPageEmplazamientos();
    let total = 0;

    if (App.UCGridEmplazamientos_hdTotalCountGrid.value) {
        total = App.UCGridEmplazamientos_hdTotalCountGrid.value;
    } else {
        total = App.UCGridEmplazamientos_storeEmplazamientos.getTotalCount();
    }

    if (total / lastPage < currentPage) {
        currentPage = 1;
        App.UCGridEmplazamientos_storeEmplazamientos.loadPage(currentPage);
    }

    App.UCGridEmplazamientos_nfPaginNumber.setValue(currentPage);
    App.UCGridEmplazamientos_lbNumberPages.setText(lastPage);

    if (currentPage == 1 && lastPage == 1) {
        App.UCGridEmplazamientos_btnPagingInit.setDisabled(true);
        App.UCGridEmplazamientos_btnPagingPre.setDisabled(true);
        App.UCGridEmplazamientos_btnPaginNext.setDisabled(true);
        App.UCGridEmplazamientos_btnPaginLast.setDisabled(true);
    }
    else if (currentPage <= 1) {
        App.UCGridEmplazamientos_btnPagingInit.setDisabled(true);
        App.UCGridEmplazamientos_btnPagingPre.setDisabled(true);
        App.UCGridEmplazamientos_btnPaginNext.setDisabled(false);
        App.UCGridEmplazamientos_btnPaginLast.setDisabled(false);
    }
    else if (currentPage >= lastPage) {
        App.UCGridEmplazamientos_btnPaginNext.setDisabled(true);
        App.UCGridEmplazamientos_btnPaginLast.setDisabled(true);
        App.UCGridEmplazamientos_btnPagingInit.setDisabled(false);
        App.UCGridEmplazamientos_btnPagingPre.setDisabled(false);
    } else {
        App.UCGridEmplazamientos_btnPaginNext.setDisabled(false);
        App.UCGridEmplazamientos_btnPaginLast.setDisabled(false);
        App.UCGridEmplazamientos_btnPagingInit.setDisabled(false);
        App.UCGridEmplazamientos_btnPagingPre.setDisabled(false);
    }

    updateDisplayingEmplazamientos();
}

function updateDisplayingEmplazamientos() {
    var txtDisplaying = "";
    var total = 0;
    let lastPage = getLastPageEmplazamientos();
    var pageSize = App.UCGridEmplazamientos_storeEmplazamientos.pageSize;

    if (App.UCGridEmplazamientos_hdTotalCountGrid.value) {
        total = App.UCGridEmplazamientos_hdTotalCountGrid.value;
    } else {
        total = App.UCGridEmplazamientos_storeEmplazamientos.getTotalCount();
    }

    if (total / lastPage < currentPage) {
        currentPage = 1;
    }

    let firstItem = ((currentPage - 1) * pageSize) + 1;
    let lastItem = currentPage * pageSize;

    if (currentPage == getLastPageEmplazamientos()) {
        lastItem = total;
    }

    txtDisplaying = `${jsMostrando} ${firstItem} - ${lastItem} ${jsDe} ${total}`;
    App.UCGridEmplazamientos_lbDisplaying.setText(txtDisplaying);
}

function setNumMaxPageEmplazamientos() {
    updateDisplayingEmplazamientos();
    updatePaginEmplazamientos();
}
// #endRegion

// #region Buscador
var dataGridEmplazamientos = [];
function ajaxGetDatosBuscadorEmplazamientos() {

    updatePaginEmplazamientos()

    if (dataGridEmplazamientos.length == 0 || forzarCargaBuscadorPredictivo) {
        dataGridEmplazamientos = [];

        TreeCore["UCGridEmplazamientos"].GetDatosBuscador({
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

                    var idComponente = "UCGridEmplazamientos";
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

function CargarGridEmplazamientos(sender) {
    let ruta = getIdComponente(sender);
    TreeCore[ruta].CargarGrid();
}