var stPn = 0;
function hidePn(onlyshow = false) {
    let btn = document.getElementById('btnCollapseAsR');
    let btnVista1 = document.getElementById('btnGestionContentToCategoryView');
    let btnVista2 = document.getElementById('btnCategoryViewToGestionContent');
    if (onlyshow == true) {
        App.pnAsideR.expand();
        btn.style.opacity = 1;
        App.pnAsideR.updateLayout();
        //RpanelBTNNotes();

        stPn = 0;
    } else {
        if (stPn == 0) {
            App.pnAsideR.collapse();
            btn.style.opacity = 0;

            stPn = 1;
        }
        else {
            App.pnAsideR.expand();
            btn.style.opacity = 1;
            App.pnAsideR.updateLayout();
            //RpanelBTNNotes();

            stPn = 0;
        }
    }

}

function hidePnTab() {
    let btn = document.getElementById('btnCollapseAsR');
    App.pnAsideR.collapse();
    btn.style.opacity = 0;
    btn.style.marginRight = '';
    stPn = 1;
}

//function AbrirFiltros() {
//    let btn = document.getElementById('btnCollapseAsR');
//    let btnVista1 = document.getElementById('btnGestionContentToCategoryView');
//    let btnVista2 = document.getElementById('btnCategoryViewToGestionContent');
//    btn.style.marginRight = '380px';
//    btn.style.transform = 'rotate(0deg)';
//    App.pnAsideR.expand();
//    App.pnAsideR.updateLayout();
//    //RpanelBTNAlert();

//    stPn = 0;
//}


Ext.onReady(function () {


    hidePn();
})

//#region FILTERS

var cmbFieldCargado = false;
var pnLateralAbierto;

function focusCmbField() {
    if (!cmbFieldCargado) {
        cmbFieldCargado = true;
        App.storeCampos.reload();
    }
}

function MostrarPnFiltros(lCategoriaID, Columnas) {
    pnLateralAbierto = 'Filtros';
    displayMenuInventary('pnCFilters');
    hidePn(true);
    App.hdCategoriaActiva.setValue(lCategoriaID);
    App.hdColumnas.setValue(Columnas);
    App.storeCampos.reload();
    App.storeViews.reload();
}

function displayMenuInventary(btn, clear = true) {

    if (pnLateralAbierto == 'Filtros') {
        //ocultar todos los paneles
        //App.pnAsideR.expand();
        App.pnCFilters.hide();
        App.pnGridsAsideMyFilters.hide();
        App.pnQuickFilters.hide();
        App.pnGridsAsideMyFilters.hide();
        App.pnGridsAsideMyViews.hide();
        App.ctAsideRInfo.hide();
        App.ctAsideR.show();
        App.lblAsideNameR.show();
        App.lblAsideNameInfo.hide();

        if (btn == "pnGridsAsideMyFilters") {
            App.GridMyFilters.getStore().reload();
        }

        if (btn == "pnCFilters") {
            newFilter(clear);
        }

        if (btn == "pnGridsAsideMyViews") {
            //PintarColumnas();
            if (App.pnGridsAsideMyViews.hidden) {
                App.cmbViews.setValue(listaPaginas["App" + hdCategoriaActiva.value].cmbViews.value);
            }
        }

        //GET componente a mostrar desde el boton por ID
        if (btn != null) {

            App[btn].show();

        }
    } else {
        //ocultar todos los paneles
        hidePn(true);
        App.pnGridInfo.hide();
        App.pnGridVin.hide();
        App.ctAsideR.hide();
        App.ctAsideRInfo.show();
        App.lblAsideNameR.hide();
        App.lblAsideNameInfo.show();

        App.pnGridInfo.show();
        //GET componente a mostrar desde el boton por ID
        if (btn != null) {
            App[btn].show();
        }
    }

}

function displayMenuInventaryInfo(btn) {

    //ocultar todos los paneles
    hidePn(true);
    if (btn == "pnGridInfo") {
        App.pnGridInfo.show();
        App.pnGridVin.hide();
        App.pnSites.hide();
        App.pnInfoLocation.hide();
        App.pnInfoAtributos.hide();

        App.btnMoreInfo.show();
        App.btnMoreInfoVin.show();

        App.btnMoreInfoSites.hide();
        App.btnMoreInfoLocation.hide();
        App.btnMoreInfoAdicional.hide();
    }
    else if (btn == "pnGridVin") {
        App.pnGridVin.show();
        App.pnGridInfo.hide();
        App.pnSites.hide();
        App.pnInfoLocation.hide();
        App.pnInfoAtributos.hide();

        App.btnMoreInfo.show();
        App.btnMoreInfoVin.show();

        App.btnMoreInfoSites.hide();
        App.btnMoreInfoLocation.hide();
        App.btnMoreInfoAdicional.hide();
    }
    else if (btn == "pnSites") {
        App.pnGridInfo.hide();
        App.pnGridVin.hide();
        App.pnInfoLocation.hide();
        App.pnInfoAtributos.hide();

        App.btnMoreInfo.hide();
        App.btnMoreInfoVin.hide();

        App.pnSites.show();
        App.pnSites.updateLayout();

        App.btnMoreInfoSites.show();
        App.btnMoreInfoLocation.show();
        App.btnMoreInfoAdicional.show();
    }
    else if (btn == "pnInfoLocation") {
        App.pnGridInfo.hide();
        App.pnGridVin.hide();
        App.pnSites.hide();
        App.pnInfoAtributos.hide();

        App.btnMoreInfo.hide();
        App.btnMoreInfoVin.hide();

        App.pnInfoLocation.show();
        App.pnInfoLocation.updateLayout();

        App.btnMoreInfoSites.show();
        App.btnMoreInfoLocation.show();
        App.btnMoreInfoAdicional.show();
    }
    else if (btn == "pnInfoAtributos") {
        App.pnGridInfo.hide();
        App.pnGridVin.hide();
        App.pnInfoLocation.hide();
        App.pnSites.hide();

        App.btnMoreInfo.hide();
        App.btnMoreInfoVin.hide();

        App.pnInfoAtributos.show();
        App.pnInfoAtributos.updateLayout();

        App.btnMoreInfoSites.show();
        App.btnMoreInfoLocation.show();
        App.btnMoreInfoAdicional.show();
    }

    App.ctAsideR.hide();
    App.ctAsideRInfo.show();
    App.lblAsideNameR.hide();
    App.lblAsideNameInfo.show();
}

function DeleteFilter(sender, registro, index) {
    Ext.Msg.alert(
        {
            title: jsEliminar + ' ' + sender.$widgetRecord.data.NombreFiltro,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: FnEliminar,
            icon: Ext.MessageBox.QUESTION,
            GestionFiltroID: sender.$widgetRecord.data.GestionFiltroID
        });
}

function FnEliminar(button, a, sender) {
    if (button == 'yes' || button == 'si') {
        ajaxEliminarFiltro(sender.GestionFiltroID);
    }
}

function ajaxEliminarFiltro(idFiltro) {
    TreeCore.EliminarFiltro(idFiltro,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    filtrosAplicados = filtrosAplicados.filter(function (obj) {
                        return obj.id !== idFiltro;
                    });

                    App.GridMyFilters.getStore().reload();

                    ajaxAplicarFiltro(filtrosAplicados);
                    mostrarFiltrosCabecera();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function MostrarEditarFiltroGuardado(sender, registro, index) {
    newFilter();
    let myFilterData = sender.$widgetRecord.data;

    let myFilter = {
        id: myFilterData.GestionFiltroID,
        name: myFilterData.NombreFiltro,
        guardado: true,
        filters: JSON.parse(myFilterData.JsonItemsFiltro)
    }
    editandoMiFiltro = myFilter;

    camposListados = myFilter.filters;

    camposListados.forEach(item => { showNewItemFilter(item.Id, item.Name, item.valueLabel); });

    App.pnNewFilter.setValue(myFilter.name);

    displayMenuSites('pnCFilters');

}

function EditFilterSaved(sender, registro, index) {
    alert("Editar filtro");
}

function AplyFilterSaved(sender, registro, index) {
    let myFilterData = sender.$widgetRecord.data;

    let myFilter = {
        id: myFilterData.GestionFiltroID,
        name: myFilterData.NombreFiltro,
        guardado: true,
        filters: JSON.parse(myFilterData.JsonItemsFiltro)
    }

    if (filtrosAplicados.filter(f => f.id == myFilter.id).length == 0) {
        AñadirFiltro(myFilter);

        mostrarFiltrosCabecera();
        ajaxAplicarFiltro(filtrosAplicados);
    }
}

function showNewItemFilter(Id, name, valueLabel) {
    let panelElementoFiltro = new Ext.Panel({
        Cls: "pntag",
        Hidden: "false",
        treeField: Id,
        items: [
            {
                xtype: "netlabel",
                text: name,
                cls: "pnTagField",
            },
            {
                xtype: "netlabel",
                text: valueLabel,
                cls: "pnTagSearch",
            },
            {
                xtype: "button",
                cls: "btnCloseTag",
                handler: removeElementFilter,
            },
        ]
    });

    App.pnTagContainer.add(panelElementoFiltro);
}

function removeElementFilter(sender, registro, index) {
    let field = sender.getBubbleParent().treeField;
    //sender.getBubbleParent().items.items[0].text
    //sender.getBubbleParent().items.items[1].text

    reloadFields(field);

    //sender.getBubbleParent().removeAll();
    sender.getBubbleParent().destroy()
}

function reloadFields(field) {
    App.cmbField.store.reload();

    camposListados = camposListados.filter(function (obj) {
        return obj.Id !== field;
    });
}

function beforeLoadCmbField(sender, registro, index) {
    camposListados.forEach(camp => App.cmbField.removeByValue(camp.Id));
}

function desaplicarFiltroTemporal(sender, registro, index) {
    filtrosAplicados = filtrosAplicados.filter(function (obj) {
        return obj.id !== sender.getBubbleParent().idTree;
    });

    if (sender.getBubbleParent().idTree == idShowHiddenSites) {
    } else {
        ajaxAplicarFiltro(filtrosAplicados);
        mostrarFiltrosCabecera();
    }
}

function desaplicarFiltroGuardado(sender, registro, index) {
    filtrosAplicados = filtrosAplicados.filter(function (obj) {
        return obj.id !== sender.getBubbleParent().idTree;
    });

    ajaxAplicarFiltro(filtrosAplicados);
    mostrarFiltrosCabecera();
}

function selectField(sender, registro, index) {
    ocultarYResetearCampos();

    if (tiposDinamicos.includes(sender.selection.data.Id)) {
        App.cmbTiposDinamicos.show();
        App.storeTiposDinamicos.reload();
    }
    else {
        let typeData = sender.selection.data.typeData;



        if (typeData.includes("System.Int64") || typeData.includes("System.Double")) {
            App.cmbOperatorField.show();
            App.numberInputSearch.show();
        }
        else if (typeData.includes("System.DateTime")) {
            App.cmbOperatorField.show();
            App.dateInputSearch.show();
        }
        /*else if (typeData.includes("System.String")) {
            App.cmbOperatorField.hide();
            App.textInputSearch.show();
        }*/
        else {
            App.cmbOperatorField.hide();
            App.textInputSearch.show();
        }
    }

}

function ocultarYResetearCampos() {
    //Ocultar campos
    App.dateInputSearch.hide();
    App.textInputSearch.hide();
    App.numberInputSearch.hide();
    App.cmbOperatorField.hide();
    App.cmbTiposDinamicos.hide();

    //Resetear campos
    App.cmbOperatorField.setValue();
    App.cmbTiposDinamicos.setSelectedItems();
    App.dateInputSearch.setValue();
    App.textInputSearch.setValue();
    App.numberInputSearch.setValue();
}

function ajaxAgregarFiltro(filtroGuardar) {
    TreeCore.AgregarFiltro(JSON.stringify(filtroGuardar),
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    if (result.Result != "") {
                        let jsonResult = JSON.parse(result.Result);
                        if (jsonResult && jsonResult.oldID) {
                            let oldID = jsonResult.oldID;
                            let newID = jsonResult.newID;

                            filtrosAplicados.forEach(filtro => {

                                if (filtro.id == oldID) {
                                    filtro.id = newID;
                                }
                            });

                            ajaxAplicarFiltro(filtrosAplicados);
                            mostrarFiltrosCabecera();
                        }
                    }
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function ajaxAplicarFiltro(filtrosAplicados) {
    let tab = getTabSelected();
    forzarCargaBuscadorPredictivo = true;

    var fa = JSON.stringify({ items: filtrosAplicados, tab: tab, visible: sitesVisible });
    var curPage = 1;

    App.hdFiltrosAplicados.setValue(fa);
    //curPage = App.cmbNumRegistros.value;
    sJsonFiltrosAplicados = fa;


    TreeCore.AplicarFiltro(fa, null, curPage, 0,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {

                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function getTabSelected() {

    var activeButton = document.getElementsByClassName("lnk-navView lnk-noLine navActivo");

    for (var x = 0; x < activeButton.length; x++) {
        if (activeButton[x].parentElement.id == "lnkSites") {
            return "Sites";
        }
        else if (activeButton[x].parentElement.id == "lnkLocation") {
            return "Localizaciones";
        }
    }
}

function ResizerAside(pn) {
    var elmnt = document.getElementById("vwContenedor-innerCt");

    if (elmnt != null) {
        var HeightVisorPadre = elmnt.offsetHeight;
        if (App != null) {
            App.pnAsideR.setHeight(HeightVisorPadre + 60);
        }
    }

}

//#endregion END FILTERS


//RIGHT PANEL TAB BUTTONS

//function RpanelBTNAlert() {

//    App.pnFiltros.show();
//    App.pnGridInfo.hide();
//    App.pnGridVin.hide();

//}

//function RpanelBTNNotes() {

//    App.pnGridInfo.show();
//    App.pnFiltros.hide();
//    App.pnGridVin.hide();

//}

//function RpanelBTNVin() {

//    App.pnGridInfo.hide();
//    App.pnFiltros.hide();
//    App.pnGridVin.show();

//}

//END RIGHT PANEL TAB BUTTONS

function NavegacionTabs(sender) {
    var senderid = sender.id;
    tabToUpdate = senderid;

    document.getElementById('HyperlinkButton3').classList.remove("navActivo");
    document.getElementById('HyperlinkButton3').childNodes[1].classList.remove("navActivo");
    document.getElementById('HyperlinkButton4').classList.remove("navActivo");
    document.getElementById('HyperlinkButton4').childNodes[1].classList.remove("navActivo");

    if (senderid == 'HyperlinkButton3') {
        document.getElementById('HyperlinkButton3').classList.add("navActivo");
        document.getElementById('HyperlinkButton3').childNodes[1].classList.add("navActivo");
        App.ctMain1.show();
        App.ctMain2.hide();
    }
    else if (senderid == 'HyperlinkButton4') {
        document.getElementById('HyperlinkButton4').classList.add("navActivo");
        document.getElementById('HyperlinkButton4').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.show();
    }

}

function ExportarCategorias() {
    showLoadMask(App.vwContenedor, function (load) {
        recargarCombos([App.cmbCategorias], function Fin(fin) {
            if (fin) {
                App.WinCategorias.show();
                load.hide();
            }
        });
    });
}

function DescargarPlantilla() {
    TreeCore.ExportarModeloDatos(App.cmbCategorias.value,
        {
            success: function (result) {
                if (result.Success != null && !result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.WinCategorias.hide();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function GenerarTab(tabla, datos) {
    var html = '';
    jQuery.each(datos, function (i, val) {
        html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + i + '</span><span class="dataGrd">' + ((val == null) ? '' : val) + '</span></td></tr>'
    });
    document.getElementById(tabla).innerHTML = html;
}

function CargarPanelMoreInfo(catID, bMostrar) {
    html = '';
    tablaBody = document.getElementById('bodyTablaInfoElementos');
    tablaBody.innerHTML = '';
    if (bMostrar != 'Sites' && (bMostrar || !App.pnAsideR.collapsed)) {
        TreeCore.CargarDatosElementos(catID, 'Inventario',
            {
                success: function (result) {
                    if (result.Success != null && !result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        lista = result.Result;
                        for (var dato in lista) {
                            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + dato + '</span><span class="dataGrd">' + ((lista[dato] == null) ? '' : lista[dato]) + '</span></td></tr>'
                        }
                        tablaBody.innerHTML = html;
                        TreeCore.CargarVinculacionesElementos(catID,
                            {
                                success: function (result) {
                                    if (result.Success != null && !result.Success) {
                                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                    }
                                    else {
                                        lista = result.Result;
                                        html = '<p class="lblHeadAside">' + jsElementosPadres + '</p>';
                                        tablaBody = document.getElementById('bodyTablaInfoVinPadres');
                                        tablaBody.innerHTML = '';
                                        for (var dato in lista["Padres"]) {
                                            dato = lista["Padres"][dato];
                                            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3">' +
                                                '<span class="lblGrd">' + dato.Categoria + '</span>' + '<br>' +
                                                '<span class="lblGrd">' + dato.Operador + '</span>' + '<br>' +
                                                '<span class="lblGrd">' + dato.Estado + '</span>' + '<br>' +
                                                '<span class="lblGrd">' + dato.Nombre + '</span>' + '<br>' +
                                                '<span class="dataGrd">' + dato.Codigo + '</span>' +
                                                '<span class="lblGrd">' + dato.Vinculaciones + '</span></td></tr>';
                                        }
                                        tablaBody.innerHTML = html;
                                        html = '<p class="lblHeadAside">' + jsElementosHijos + '</p>';
                                        tablaBody = document.getElementById('bodyTablaInfoVinHijas');
                                        tablaBody.innerHTML = '';
                                        for (var dato in lista["Hijas"]) {
                                            dato = lista["Hijas"][dato];
                                            html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3">' +
                                                '<span class="lblGrd">' + dato.Categoria + '</span>' + '<br>' +
                                                '<span class="lblGrd">' + dato.Operador + '</span>' + '<br>' +
                                                '<span class="lblGrd">' + dato.Estado + '</span>' + '<br>' +
                                                '<span class="lblGrd">' + dato.Nombre + '</span>' + '<br>' +
                                                '<span class="dataGrd">' + dato.Codigo + '</span>' +
                                                '<span class="lblGrd">' + dato.Vinculaciones + '</span></td></tr>';
                                        }
                                        tablaBody.innerHTML = html;
                                        App.pnAsideR.updateLayout()
                                        //if (bMostrar) {
                                        //    hidePn(true);
                                        //}
                                    }
                                }
                            });
                    }
                }
            });
        if (bMostrar) {
            displayMenuInventaryInfo('pnGridInfo');
        }
    }
    else if (bMostrar == 'Sites') {
        TreeCore.CargarDatosElementos(catID, 'Sites',
            {
                success: function (result) {
                    if (result.Success != null && !result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        GenerarTab('bodyTablaInfoSites', result.Result.General);
                        GenerarTab('bodyTablaInfoLocation', result.Result.Localizacion);
                        GenerarTab('bodyTablaInfoAtributos', result.Result.Adicional);

                        App.pnAsideR.updateLayout();
                    }
                }
            });
        displayMenuInventaryInfo('pnSites');
    }
}

function RecargarClientes() {
    recargarCombos([App.cmbClientes]);
    App.hdCliID.setValue(0);
}

function SeleccionarCliente() {
    App.cmbClientes.getTrigger(0).show();
    App.hdCliID.setValue(App.cmbClientes.value);
}

var listaPaginas = {};

function AddReferencias(espacio, CategoriaID) {
    listaPaginas['App' + CategoriaID] = espacio;
    return 'App' + CategoriaID;
}

function RemoveReferencias(id) {
    delete listaPaginas[id];
}

function AplicaFiltros() {
    for (var pag in listaPaginas) {
        var AppAux = listaPaginas[pag];
        //AplicarFiltros(AppAux);
        AppAux.grid.store.reload();
    }
}

function AplicarFiltros(AppAux) {
    var tree = AppAux.grid,
        store = tree.store,
        logic = tree;

    tree.clearFilter();
    logic.clearFilter();

    try {
        var reOperador = new RegExp(AppAux.hdOperador.value.join('|'), "i");
    } catch (e) {

    }
    try {
        var reEstado = new RegExp(AppAux.hdEstado.value.join('|'), "i");
    } catch (e) {

    }
    try {
        var reUsuario = new RegExp(AppAux.hdUsuario.value.join('|'), "i");
    } catch (e) {

    }
    var valido;
    logic.filterBy(function (node) {
        valido = true;
        if (!reOperador.test(node.data.OperadorID)) {
            valido = false;
        }
        if (!reEstado.test(node.data.EstadoID)) {
            valido = false;
        }
        if (!reUsuario.test(node.data.CreadorID)) {
            valido = false;
        }
        if (!(((AppAux.hdFechaMinCrea.value != undefined) ? AppAux.hdFechaMinCrea.value <= new Date(node.data.FechaAlta) : true) && ((AppAux.hdFechaMaxCrea.value != undefined) ? AppAux.hdFechaMaxCrea.value >= new Date(node.data.FechaAlta) : true))) {
            valido = false;
        }
        if (!(((AppAux.hdFechaMinMod.value != undefined) ? AppAux.hdFechaMinMod.value <= new Date(node.data.FechaMod) : true) && ((AppAux.hdFechaMaxMod.value != undefined) ? AppAux.hdFechaMaxMod.value >= new Date(node.data.FechaMod) : true))) {
            valido = false;
        }
        return valido;
    });
}

function getFormattedDate(date) {
    var year = date.getFullYear();

    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;

    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;

    return day + '/' + month + '/' + year;
}

function ValidarCmbCategorias(sender) {
    return (App.cmbCategorias.getSelectedValues().length < App.cmbCategorias.maxSelection);
}

function FormValid(valid) {
    if (valid) {
        App.btnDescargar.setDisabled(false);
    }
    else {
        App.btnDescargar.setDisabled(true);
    }
}

//#region FILTROS
var FiltrosAplicados = [];
var Filtros = [];

function ocultarYResetearCampos() {
    //Ocultar campos
    App.dateInputSearch.hide();
    App.textInputSearch.hide();
    App.numberInputSearch.hide();
    App.cmbOperatorField.hide();
    App.cmbTiposDinamicos.hide();
    App.chkTiposDinamicos.hide();

    //Resetear campos
    App.cmbOperatorField.reset();
    App.cmbTiposDinamicos.reset();
    App.dateInputSearch.reset();
    App.textInputSearch.reset();
    App.chkTiposDinamicos.reset();
    App.numberInputSearch.reset();
}

function newFilter(clear = true) {
    App.pnNewFilterNombre.reset();
    App.cmbField.reset();
    ocultarYResetearCampos();
    if (clear) {
        App.storeFiltros.clearData()
        App.storeFiltros.reload();
    }
    NombreFiltroValido();
    App.hdFiltroID.setValue(0);
    Filtros = [];
}

function beforeLoadCmbField() {
    ocultarYResetearCampos();
}

function selectField(sender) {
    ocultarYResetearCampos();
    let typeData = sender.selection.data.TypeData;

    if (typeData == "NUMERICO") {
        App.cmbOperatorField.show();
        App.cmbOperatorField.setValue('IGUAL');
        App.numberInputSearch.show();
        document.getElementById("btnAdd").style.marginTop = '30px';
    }
    else if (typeData == "FECHA") {
        App.cmbOperatorField.show();
        App.cmbOperatorField.setValue('IGUAL');
        App.dateInputSearch.show();
        document.getElementById("btnAdd").style.marginTop = '30px';
    }
    else if (typeData == "LISTA" || typeData == "LISTAMULTIPLE") {
        App.hdQuery.setValue(sender.selection.data.QueryValores);
        App.cmbTiposDinamicos.show();
        document.getElementById("btnAdd").style.marginTop = '6px';
        App.storeTiposDinamicos.reload();
    }
    else if (typeData == "BOOLEAN") {
        App.chkTiposDinamicos.show();
        document.getElementById("btnAdd").style.marginTop = '6px';
    }
    else {
        App.textInputSearch.show();
        document.getElementById("btnAdd").style.marginTop = '6px';
    }
}

function addElementFilter() {
    if (App.cmbField.selection) {
        var newFilter;
        let campo = App.cmbField.selection.data;
        let typeData = App.cmbField.selection.data.TypeData;

        for (var x = 0; x < App.storeFiltros.data.length; x++) {
            if (App.storeFiltros.data.items[x].data.Name  == campo.Name) {
                App.storeFiltros.remove(App.storeFiltros.data.items[x]);

                var vBusqueda = Filtros.find(x => x.Name == campo.Name);
                if (vBusqueda != undefined) {
                    Filtros.splice(vBusqueda.id);
                }
            }
        }

        if (typeData == "NUMERICO") {
            newFilter = {
                "Name": campo.Name,
                "Campo": campo.Campo,
                "Value": App.numberInputSearch.value,
                "DisplayValue": App.cmbOperatorField.rawValue + ' ' + App.numberInputSearch.value,
                "TypeData": campo.TypeData,
                "Operador": App.cmbOperatorField.rawValue,
                "TipoCampo": campo.TipoCampo
            };
        }
        else if (typeData == "FECHA") {
            newFilter = {
                "Name": campo.Name,
                "Campo": campo.Campo,
                "Value": getFormattedDate(App.dateInputSearch.value),
                "DisplayValue": App.cmbOperatorField.rawValue + ' ' + getFormattedDate(App.dateInputSearch.value),
                "TypeData": campo.TypeData,
                "Operador": App.cmbOperatorField.rawValue,
                "TipoCampo": campo.TipoCampo
            };
        }
        else if (typeData == "LISTA" || typeData == "LISTAMULTIPLE") {
            newFilter = {
                "Name": campo.Name,
                "Campo": campo.Campo,
                "Value": App.cmbTiposDinamicos.getSelectedValues().join(),
                "DisplayValue": App.cmbTiposDinamicos.getSelectedText().join(),
                "TypeData": campo.TypeData,
                "TipoCampo": campo.TipoCampo
            };
        }
        else if (typeData == "BOOLEAN") {
            newFilter = {
                "Name": campo.Name,
                "Campo": campo.Campo,
                "Value": App.chkTiposDinamicos.value,
                "DisplayValue": App.chkTiposDinamicos.value,
                "TypeData": campo.TypeData,
                "TipoCampo": campo.TipoCampo
            };
        }
        else {
            newFilter = {
                "Name": campo.Name,
                "Campo": campo.Campo,
                "Value": App.textInputSearch.value,
                "DisplayValue": App.textInputSearch.value,
                "TypeData": campo.TypeData,
                "TipoCampo": campo.TipoCampo
            };
        }
        Filtros.push(newFilter);
        App.storeFiltros.add(newFilter);
        NombreFiltroValido();
    }
}

function aplyFilter() {
    var Filtro = {
        "InventarioCategoriaID": hdCategoriaActiva.value,
        "NombreFiltro": App.pnNewFilterNombre.value,
        "listaFiltros": Filtros,
        "Saved": false,
        "NombreCategoria": App.hdNombreCategoriaActiva.value
    };
    AñadirFiltro(Filtro);
    //newFilter();
}

function saveFilter() {
    var Filtro = {
        "InventarioCategoriaID": hdCategoriaActiva.value,
        "NombreFiltro": App.pnNewFilterNombre.value,
        "listaFiltros": Filtros,
        "Saved": true,
        "NombreCategoria": App.hdNombreCategoriaActiva.value
    };
    TreeCore.GuardarFiltro(JSON.stringify(Filtro), App.pnNewFilterNombre.value,
        {
            success: function (result) {
                if (result.Success != null && !result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    AñadirFiltro(Filtro);
                    newFilter();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function NombreFiltroValido() {
    if (App.pnNewFilterNombre.isValid() && App.storeFiltros.data.items.length > 0) {
        App.btnSaveFilter.enable();
        App.btnAplyFilter.enable();
    } else {
        App.btnSaveFilter.disable();
        App.btnAplyFilter.disable();
    }
}
function mostrarFiltrosCabecera() {
    App.tagsContainer.removeAll();
    FiltrosAplicados.forEach(fl => {
        let panelFiltroCabecera;

        if (fl.Saved) {
            panelFiltroCabecera = new Ext.Panel({
                Hidden: "false",
                idTree: fl.InventarioCategoriaID + fl.NombreFiltro + fl.Saved,
                items: [
                    {
                        xtype: "netlabel",
                        text: fl.NombreCategoria + ': ',
                        cls: "TagSavedCatInv",
                    },
                    {
                        xtype: "netlabel",
                        text: fl.NombreFiltro,
                        cls: "TagSavedInv",
                    },
                    {
                        xtype: "button",
                        cls: "CloseSavedInv",
                        focusable: false,
                        pressedCls: "none",
                        focusCls: "none",
                        handler: desaplicarFiltro,
                    },
                ]
            });
        }
        else {
            panelFiltroCabecera = new Ext.Panel({
                Hidden: "false",
                idTree: fl.InventarioCategoriaID + fl.NombreFiltro + fl.Saved,
                items: [
                    {
                        xtype: "netlabel",
                        text: fl.NombreCategoria + ': ',
                        cls: "TagTempCatInv",
                    },
                    {
                        xtype: "netlabel",
                        text: fl.NombreFiltro,
                        cls: "TagTempInv",
                    },
                    {
                        xtype: "button",
                        cls: "CloseTempInv",
                        focusable: false,
                        pressedCls: "none",
                        FocusCls: "none",
                        handler: desaplicarFiltro,
                    },
                ]
            });
        }

        App.hdFiltros.setValue(JSON.stringify(FiltrosAplicados));

        if (fl.InventarioCategoriaID != App.hdCategoriaActiva.value) {
            panelFiltroCabecera.disable = true;
            panelFiltroCabecera.style = 'opacity: .5; pointer-events: none;';
        }

        App.tagsContainer.add(panelFiltroCabecera);
    });
}

function desaplicarFiltro(sender, registro, index) {
    FiltrosAplicados = FiltrosAplicados.filter(function (obj) {
        if ((obj.InventarioCategoriaID + obj.NombreFiltro + obj.Saved) == sender.getBubbleParent().idTree && listaPaginas["App" + obj.InventarioCategoriaID] != undefined) {
            listaPaginas["App" + obj.InventarioCategoriaID].storePrincipal.reload();
        }
        return (obj.InventarioCategoriaID + obj.NombreFiltro + obj.Saved) !== sender.getBubbleParent().idTree;
    });
    mostrarFiltrosCabecera();
}

function EliminarFiltro(sender, registro, index) {
    App.storeFiltros.data.removeAtKey(index.id);
    Filtros = Filtros.filter(function (obj) {
        return obj.Campo !== index.id;
    });
    NombreFiltroValido();
}

function AplicarFiltroGuardado(filtro) {
    var json = JSON.parse(filtro.getAttribute("jsonfiltros"));
    json["NombreCategoria"] = App.hdNombreCategoriaActiva.value;
    AñadirFiltro(json);
}

function EliminarFiltroGuardado(filtro) {
    App.hdFiltroID.setValue(filtro.getAttribute("filtroid"));
    Ext.Msg.alert(
        {
            title: jsEliminar + ' ' + jsFiltros,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxEliminarFiltroGuardado,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxEliminarFiltroGuardado(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarFiltro(
            {
                success: function (result) {
                    if (result.Success != null && !result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        App.GridMyFilters.store.reload();
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

function EditarFiltro(sender) {
    var nombre = sender.getAttribute("nombre");
    var jsonFiltros = sender.getAttribute("jsonfiltros");
    var filtroID = sender.getAttribute("filtroid");
    displayMenuInventary('pnCFilters', false);
    App.pnNewFilterNombre.setValue(nombre);
    App.hdFiltroID.setValue(filtroID);
    App.storeFiltros.load({
        callback: function (r, options, success) {
            if (success === true) {
                JSON.parse(jsonFiltros).listaFiltros.forEach(x => { App.storeFiltros.add(x) });
                NombreFiltroValido();
            }
        }
    });
}

function AñadirFiltro(filtro) {
    FiltrosAplicados = FiltrosAplicados.filter(function (obj) {
        if (filtro.Saved) {
            var valido;
            valido = (obj.InventarioCategoriaID + obj.NombreFiltro + obj.Saved) !== (filtro.InventarioCategoriaID + filtro.NombreFiltro + filtro.Saved);
            if (valido) {
                return (obj.InventarioCategoriaID + obj.NombreFiltro + obj.Saved) !== (filtro.InventarioCategoriaID + filtro.NombreFiltro + !filtro.Saved);
            }
            return valido;
        } else {
            return (obj.InventarioCategoriaID + obj.NombreFiltro + obj.Saved) !== (filtro.InventarioCategoriaID + filtro.NombreFiltro + filtro.Saved);
        }
    });
    FiltrosAplicados.push(filtro);
    mostrarFiltrosCabecera();
    if (listaPaginas["App" + filtro.InventarioCategoriaID] != undefined) {
        listaPaginas["App" + filtro.InventarioCategoriaID].storePrincipal.reload();
    }
}

//#endregion

//#region Columnas

function PintarColumnas() {
    var json = JSON.parse(App.hdColumnas.value);
    var lista = document.getElementById('contOdernacionColumnas');
    var html = "";
    var numCol = 0;
    json.forEach(col => {
        html += '<li class="filaColumna" columna="' + col.Columna + '"><div class="contenedorColumna"><figure class="icoDrag ico-drag-vertical"></figure><p class="nombreColumna">' + col.NombreColumna + '</p><input class="chkVisible" type="checkbox" id="chkCol' + numCol + '"' + ((col.Visible) ? '' : 'checked') + '><label class="lbchkVisible ico-visible" for="chkCol' + numCol + '"></label></div></li>';
        numCol++;
    });
    lista.innerHTML = html;
    $("#contOdernacionColumnas").sortable({
        group: 'columnas',
        handle: ".icoDrag",
        pullPlaceholder: false,
    });
    $("#contOdernacionColumnas").disableSelection();
}

function GetColumnView() {
    var listaCol = [];
    var jsonAux;
    $('#contOdernacionColumnas').children().each(function (x, item) {
        jsonAux = {
            "Orden": x,
            "NombreColumna": item.getElementsByClassName("nombreColumna")[0].innerHTML,
            "Columna": item.getAttribute('columna'),
            "Visible": !item.getElementsByClassName("chkVisible")[0].checked
        };
        listaCol.push(jsonAux);
    });
    return listaCol;
}

function AplicarView(sender) {
    listaPaginas["App" + hdCategoriaActiva.value].hdViewJson.setValue(JSON.stringify(GetColumnView()));
    if (App.btnFiltosActivos.pressed) {
        if (App.cmbViews.selection.data.JsonFiltros != "") {
            var json = JSON.parse(App.cmbViews.selection.data.JsonFiltros);
            $.each(json, function (i, jsonAux) {
                jsonAux["NombreCategoria"] = App.hdNombreCategoriaActiva.value;
                AñadirFiltro(jsonAux);
            });
        }
    }
}

function GuardarView(sender) {
    var filtros = [];
    FiltrosAplicados.forEach(filtro => {
        if (filtro.InventarioCategoriaID == hdCategoriaActiva.value) {
            filtros.push(filtro);
        }
    });
    TreeCore.GuardarView(JSON.stringify(GetColumnView()), JSON.stringify(filtros),
        {
            success: function (result) {
                if (result.Success != null && !result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    CargarStoresSerie([App.storeViews], function Fin(fin) {
                        if (fin) {
                            listaPaginas["App" + hdCategoriaActiva.value].hdViewJson.setValue(JSON.stringify(GetColumnView()));
                        }
                    });
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function SelectView(sender) {
    if (sender.selection.data.Defecto) {
        App.mnuIsDefault.show();
        App.mnuDefault.hide();
    } else {
        App.mnuIsDefault.hide();
        App.mnuDefault.show();
    }
    if (App.btnFiltosActivos.pressed && sender.selection.data.JsonFiltros != '') {
        var json = JSON.parse(sender.selection.data.JsonFiltros);
        $.each(json, function (i, jsonAux) {
            jsonAux["NombreCategoria"] = App.hdNombreCategoriaActiva.value;
            AñadirFiltro(jsonAux);
        });
    }
    listaPaginas["App" + hdCategoriaActiva.value].cmbViews.setValue(sender.value);
    AplicarView();
}

function ChangeView(sender) {
    if (sender.selection != null) {
        App.hdColumnas.setValue(sender.selection.data.JsonColumnas);
        PintarColumnas();
    }
}

function SaveAsValid(sender, valid) {
    if (valid) {
        App.btnSaveSaveAs.enable();
    } else {
        App.btnSaveSaveAs.disable();
    }
}

function SaveAs() {
    var newName = App.txtNameSaveAs.value;
    var filtros = [];
    App.mnuView.hide();
    showLoadMask(App.vwContenedor, function (load) {
        FiltrosAplicados.forEach(filtro => {
            if (filtro.InventarioCategoriaID == hdCategoriaActiva.value) {
                filtros.push(filtro);
            }
        });
        TreeCore.GuardarNuevaView(newName, JSON.stringify(GetColumnView()), JSON.stringify(filtros),
            {
                success: function (result) {
                    if (result.Success != null && !result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    }
                    else {
                        CargarStoresSerie([App.storeViews, listaPaginas["App" + App.hdCategoriaActiva.value].storeViews], function Fin(fin) {
                            if (fin) {
                                listaPaginas["App" + hdCategoriaActiva.value].hdViewJson.setValue(JSON.stringify(GetColumnView()));
                                App.txtNameSaveAs.reset();
                                App.cmbViews.setValue(result.Result);
                                listaPaginas["App" + App.hdCategoriaActiva.value].cmbViews.setValue(result.Result);
                                App.mnuIsDefault.hide();
                                App.mnuDefault.show();
                                load.hide();
                            }
                        });
                    }
                }
            });
    });
}

function RenameValid(sender, valid) {
    if (valid) {
        App.btnSaveRename.enable();
    } else {
        App.btnSaveRename.disable();
    }
}

function Rename() {
    App.mnuView.hide();
    var newName = App.txtNameRename.value;
    showLoadMask(App.vwContenedor, function (load) {
        TreeCore.RenameView(newName,
            {
                success: function (result) {
                    if (result.Success != null && !result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    }
                    else {
                        CargarStoresSerie([App.storeViews, listaPaginas["App" + App.hdCategoriaActiva.value].storeViews], function Fin(fin) {
                            if (fin) {
                                App.txtNameRename.reset();
                                App.cmbViews.setValue(App.cmbViews.value);
                                listaPaginas["App" + App.hdCategoriaActiva.value].cmbViews.setValue(App.cmbViews.value);
                                load.hide();
                            }
                        });
                    }
                }
            });
    });

}

function SetDefault() {
    App.mnuView.hide();
    TreeCore.SetDefaultView(
        {
            success: function (result) {
                if (result.Success != null && !result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    App.mnuIsDefault.show();
                    App.mnuDefault.hide();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });

}

function DeleteView() {
    App.mnuView.hide();
    Ext.Msg.alert(
        {
            title: jsEliminar + ' ' + jsVista,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxDeleteView,
            icon: Ext.MessageBox.QUESTION
        });

}

function ajaxDeleteView(button) {
    if (button == 'yes' || button == 'si') {

        showLoadMask(App.vwContenedor, function (load) {
            TreeCore.DeleteView(
                {
                    success: function (result) {
                        if (result.Success != null && !result.Success) {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            load.hide();
                        }
                        else {

                            CargarStoresSerie([App.storeViews, listaPaginas["App" + App.hdCategoriaActiva.value].storeViews], function Fin(fin) {
                                if (fin) {
                                    listaPaginas["App" + App.hdCategoriaActiva.value].cmbViews.setValue(result.Result);
                                    listaPaginas["App" + App.hdCategoriaActiva.value].cmbViews.value = (result.Result);
                                    App.cmbViews.setValue(result.Result);
                                    AplicarView();
                                    load.hide();
                                }
                            });
                        }
                    }
                });
        });
    }
}

//#endregion

function BorrarFiltrosCategoria() {
    FiltrosAplicados = FiltrosAplicados.filter(function (obj) {
        return obj.InventarioCategoriaID !== App.hdCategoriaActiva.value;
    });
    mostrarFiltrosCabecera();
    listaPaginas["App" + App.hdCategoriaActiva.value].storePrincipal.reload();
}