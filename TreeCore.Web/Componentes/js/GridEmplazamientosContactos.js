let mapForm = [];

// #region DIRECT METHOD CONTACTOS
function DeseleccionarGrillaContactos(sender, registro, index) {
    var ruta = sender.config.storeId.split('_');
    ruta.pop();
    ruta = ruta.join('_');

    App[ruta + '_' + 'GridRowSelectContacto'].clearSelections();
    App[ruta + "_btnEditar"].disable();
    App[ruta + "_btnEliminar"].disable();
    App[ruta + "_btnActivar"].disable();

    App[ruta + "_btnEditar"].setTooltip(jsEditar);
    App[ruta + "_btnEliminar"].setTooltip(jsEliminar);
    App[ruta + "_btnDescargar"].setTooltip(jsDescargar);   
}

function Grid_RowSelectContactos(sender, registro, index) {
    var ruta = sender.config.proxyId.split('_');
    ruta.pop();
    ruta = ruta.join('_');

    if (registro.data != null) {
        App[ruta + "_btnEditar"].enable();
        App[ruta + "_btnActivar"].enable();

        if (registro.data.Activo) {
            App[ruta + '_' + 'btnActivar'].setTooltip(jsDesactivar);
        }
        else {
            App[ruta + '_' + 'btnActivar'].setTooltip(jsActivar);
        }
    }

    App[ruta + "_btnEliminar"].enable();
    App[ruta + "_btnEditar"].setTooltip(jsEditar);
    App[ruta + "_btnEliminar"].setTooltip(jsEliminar);   
}

function agregarContacto(sender, registro, index) {
    var idComponente = sender.id.split('_')[0];
    var ControlURL = '/Componentes/FormContactos.ascx';
    var NombreControl = 'FormContactos';
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
                    CargarVentanaContactos(sender, 'formAgregarEditarContacto', true);
                    App[idComponente + "_winGestionContacto"].setTitle(jsAgregar);
                    App[idComponente + "_winGestionContacto"].show();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

function editarContacto(sender, registro, index) {
    var idComponente = getIdComponente(sender);
    TreeCore[idComponente].MostrarEditarContacto(App[idComponente + '_GridRowSelectContacto'].selected.items[0].data.ContactoGlobalID,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    cambiarATapContacto(idComponente + '_formAgregarEditarContacto', 0);
                    CargarVentanaContactos(sender, 'formAgregarEditarContacto', false);
                    App[idComponente + "_winGestionContacto"].setTitle(jsEditar);
                    App[idComponente + "_winGestionContacto"].show();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

function activarContacto(sender, registro, index) {
    var idComponente = getIdComponente(sender);
    TreeCore[idComponente].Activar(App[idComponente + '_GridRowSelectContacto'].selected.items[0].data.ContactoGlobalID,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App[idComponente + "_storeContactosGlobalesEmplazamientos"].reload();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

function eliminarContacto(sender, registro, index, button) {
    var idComponente = getIdComponente(sender);
    if (App[idComponente + '_GridRowSelectContacto'].selected != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION,
                ID: idComponente
            });
    }
}

function ajaxEliminar(button, a, idComponente) {
    if (button == 'yes' || button == 'si') {
        TreeCore[idComponente.ID].Eliminar(App[idComponente.ID + '_GridRowSelectContacto'].selected.items[0].data.ContactoGlobalID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        App[idComponente.ID + "_storeContactosGlobalesEmplazamientos"].reload();
                    }
                },
                eventMask:
                {
                    showMask: true,
                }
            });
    }
}

function refrescarContacto(sender, registro, index) {
    

    var ruta = getIdComponente(sender);

    if (ruta == "gridContactos") {
        App.gridContactos_grdContactos.store.reload();
    } else {
        ajaxAplicarFiltro(filtrosAplicados);
    }

    App[ruta + '_' + 'GridRowSelectContacto'].clearSelections();
    App[ruta + "_btnEditar"].disable();
    App[ruta + "_btnEliminar"].disable();
    App[ruta + "_btnActivar"].disable();

    App[ruta + "_btnEditar"].setTooltip(jsEditar);
    App[ruta + "_btnEliminar"].setTooltip(jsEliminar);
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

function descargarContacto(sender, registro, index) {
}

function cargarFiltros(sender, registro, index) {
}

function recargarStore(sender, registro, index) {
    var ruta = sender.id.split('_')[0];
    if (ruta == "gridContactos") {
        sender.store.reload();
    }
}

// #endregion
var linkRendererTrack = function (value, metadata, record) {
    return Ext.String.format("<a href=\"javascript:addUserTabTrack('{0}','{1}');\">{1}<a/>", record.data.id, record.data.Code);
}

function addUserTabTrack(id, title) {
    var tabPanel = window.parent.App.tabPpal; //get from iframe  tabpanel located in parent window 
    var tab = tabPanel.getComponent('user_' + id);
    var url1 = '/PaginasComunes/Seguimiento.aspx';
    if (!tab) {
        tab = tabPanel.add({
            //id: 'user_' + id,
            title: title,
            iconCls: '#TextSignature',
            closable: true,
            //menuItem : menuItem,
            loader: {
                url: url1,
                renderer: "frame",
                loadMask: {
                    showMask: true,
                    msg: "Cargando Seguimiento..  "
                    //msg: jsCargandoMapa
                }
            }
        });
    }
    tabPanel.setActiveTab(tab);
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

function FiltrarColumnasContactos(sender, registro) {

    var idComponente = sender.id.split('_');
    idComponente.pop();
    App[idComponente + "_btnDescargar"].disable();
    var tree = App[idComponente + "_grdContactos"];
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


function LimpiarFiltroBusquedaContactos(sender, registro) {
    var idComponente = sender.id.split('_');
    idComponente.pop();

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");
    App[idComponente + "_txtSearchContactos"].setValue("");

    logic.clearFilter();
    App[idComponente + "_btnDescargar"].enable();

}


// #endregion

// #region Paginación
var nfPaginNumberFirstload = true;
var currentPage = 1;

function nfPaginNumberBeforeRenderContactos(sender, registro) {
    let ruta = getIdComponente(sender);
    if (nfPaginNumberFirstload) {

        nfPaginNumberFirstload = false;

        App[`${ruta}_nfPaginNumber`].setValue(currentPage);

        //updateDisplaying();
    }
}

function pagingInitContactos(sender) {
    let ruta = getIdComponente(sender);
    currentPage = 1;
    App[`${ruta}_storeContactosGlobalesEmplazamientos`].loadPage(currentPage);
    updatePaginContactos();
}

function pagingPreContactos(sender) {
    let ruta = getIdComponente(sender);
    currentPage--;
    App[`${ruta}_storeContactosGlobalesEmplazamientos`].loadPage(currentPage);
    updatePaginContactos();
}

function paginGoToContactos(sender, registro) {
    if (sender.value && sender.value >= 1 && sender.value <= getLastPageContactos()) {
        let ruta = getIdComponente(sender);
        currentPage = sender.value;
        App[`${ruta}_storeContactosGlobalesEmplazamientos`].loadPage(sender.value);
        updatePaginContactos();
    }
}

function paginNextContactos(sender) {
    let ruta = getIdComponente(sender);
    currentPage++;
    App[`${ruta}_storeContactosGlobalesEmplazamientos`].loadPage(currentPage);
    updatePaginContactos();
}

function paginLastContactos(sender) {
    let ruta = getIdComponente(sender);
    currentPage = getLastPageContactos(sender);
    App[`${ruta}_storeContactosGlobalesEmplazamientos`].loadPage(currentPage);
    updatePaginContactos();
}

function getLastPageContactos(sender) {
    let ruta;
    if (sender) {
        ruta = getIdComponente(sender);
    }
    else {
        ruta = "UCGridEmplazamientosContactos";
    }
    let total = 0;
    if (App[`${ruta}_hdTotalCountGrid`].value) {
        total = App[`${ruta}_hdTotalCountGrid`].value;
    } else {
        total = App[`${ruta}_storeContactosGlobalesEmplazamientos`].getTotalCount();
    }

    let pageSize = App[`${ruta}_storeContactosGlobalesEmplazamientos`].pageSize;

    pageSize = total / pageSize;
    if (pageSize % 1 != 0) {
        pageSize = Math.trunc(pageSize);
        pageSize++;
    }

    return pageSize;
}

function updatePaginContactos(sender) {
    let lastPage = getLastPageContactos(sender);
    let ruta;
    
    if (sender) {
        ruta = getIdComponente(sender);
    }
    else {
        ruta = "UCGridEmplazamientosContactos";
    }
    let total = 0;
    if (App[`${ruta}_hdTotalCountGrid`].value) {
        total = App[`${ruta}_hdTotalCountGrid`].value;
    } else {
        total = App[`${ruta}_storeContactosGlobalesEmplazamientos`].getTotalCount();
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

    updateDisplayingContactos(sender);
}

function updateDisplayingContactos(sender) {
    var txtDisplaying = "";
    var total = 0;
    let ruta; 
    if (sender) {
        ruta = getIdComponente(sender); 
    }
    else {
        ruta = "UCGridEmplazamientosContactos";
    }
    
    var pageSize = App[`${ruta}_storeContactosGlobalesEmplazamientos`].pageSize;

    if (App[`${ruta}_hdTotalCountGrid`].value) {
        total = App[`${ruta}_hdTotalCountGrid`].value;
    } else {
        total = App[`${ruta}_storeContactosGlobalesEmplazamientos`].getTotalCount();
    }

    if (total / pageSize < currentPage) {
        currentPage = 1;
    }

    let firstItem = ((currentPage - 1) * pageSize) + 1;
    let lastItem = currentPage * pageSize;

    if (currentPage == getLastPageContactos(sender)) {
        lastItem = total;
    }

    txtDisplaying = `${jsMostrando} ${firstItem} - ${lastItem} ${jsDe} ${total}`;
    App[`${ruta}_lbDisplaying`].setText(txtDisplaying);
}

function setNumMaxPageContactos(sender) {
    updateDisplayingContactos();
    updatePaginContactos();
}
// #endRegion

// #region Buscador
var dataGridEmplazamientos = [];
function ajaxGetDatosBuscadorContactos() {

    if (dataGridEmplazamientos.length == 0 || forzarCargaBuscadorPredictivo) {
        dataGridEmplazamientos = [];

        TreeCore["UCGridEmplazamientosContactos"].GetDatosBuscador({
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

                    var idComponente = "UCGridEmplazamientosContactos";
                    var nameSearchBox = "txtSearchContactos";
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

// #endRegion

function CargarGridContactos(sender) {
    let ruta = getIdComponente(sender);
    TreeCore[ruta].CargarGrid();
}
