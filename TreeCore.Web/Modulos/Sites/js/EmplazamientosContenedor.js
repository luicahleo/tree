let mapForm = [];
var stPn = 1;
var filtrosAplicados = [];
var camposListados = [];
var idTemp = 1;
var constTemp = "temp-";
var dataTab = "data-tab";
var pnLateralAbierto;

var tiposDinamicos = [
    "Operador",
    "MonedaID",
    "EstadoGlobalID",
    "EmplazamientoCategoriaSitioID",
    "EmplazamientoTamanoID",
    "EmplazamientoTipoID",
    "EmplazamientoTipoEdificioID",
    "EmplazamientoTipoEstructuraID",
    "RegionID",
    "PaisID",
    "RegionPaisID",
    "ProvinciaID",
    "MunicipioID",
    "ContactoTipoID",
    "InventarioCategoriaID",
    "OperadorInventarioID",
    "InventarioElementoAtributoEstadoID",
    "DocumentTipoID"
];
var editandoMiFiltro = null;
var sitesVisible = true;
var idShowHiddenSites = "showHiddenSites";
var sJsonFiltrosAplicados = "";
var desdeTab = false;
var tabToUpdate = 'lnkSites';

//#region FILTERS
var bindInitialFilter = function () {

    let tab = getTabSelected();
    var fa = JSON.stringify({ items: filtrosAplicados, tab: tab, visible: sitesVisible });
    sJsonFiltrosAplicados = fa;
    App.hdFiltrosAplicados.setValue(fa);

    //ajaxAplicarFiltro(filtrosAplicados);
}

function updateHdFiltrosAplicadosHijos(sender, registro, index) {

    if (tabToUpdate != '') {
        if (tabToUpdate == 'lnkSites' && document.querySelector('iframe[name="hugeCt_IFrame"]')) {
            document.querySelector('iframe[name="hugeCt_IFrame"]').contentWindow.updateFiltrosAplicados();
        }
        else if (tabToUpdate == 'lnkLocation' && document.querySelector('iframe[name="hugeCt2_IFrame"]')) {
            document.querySelector('iframe[name="hugeCt2_IFrame"]').contentWindow.updateFiltrosAplicados();
        }
        else if (tabToUpdate == 'lnkAtributos' && document.querySelector('iframe[name="hugeCt3_IFrame"]')) {
            document.querySelector('iframe[name="hugeCt3_IFrame"]').contentWindow.updateFiltrosAplicados();
        }
        else if (tabToUpdate == 'lnkMaps' && document.querySelector('iframe[name="hugeCt4_IFrame"]')) {
            document.querySelector('iframe[name="hugeCt4_IFrame"]').contentWindow.updateFiltrosAplicados();
        }
        else if (tabToUpdate == 'lnkDocumentos' && document.querySelector('iframe[name="hugeCt5_IFrame"]')) {
            document.querySelector('iframe[name="hugeCt5_IFrame"]').contentWindow.updateFiltrosAplicados();
        }
        else if (tabToUpdate == 'lnkInventory' && document.querySelector('iframe[name="hugeCt6_IFrame"]')) {
            document.querySelector('iframe[name="hugeCt6_IFrame"]').contentWindow.updateFiltrosAplicados();
        }
        else if (tabToUpdate == 'lnkContactos' && document.querySelector('iframe[name="hugeCt7_IFrame"]')) {
            document.querySelector('iframe[name="hugeCt7_IFrame"]').contentWindow.updateFiltrosAplicados();
        }
        else {

        }
    }
}

function NavegacionTabs(sender) {
    var senderid = sender.id;
    tabToUpdate = senderid;

    document.getElementById('lnkSites').classList.remove("navActivo");
    document.getElementById('lnkSites').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkLocation').classList.remove("navActivo");
    document.getElementById('lnkLocation').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkInventory').classList.remove("navActivo");
    document.getElementById('lnkInventory').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkAtributos').classList.remove("navActivo");
    document.getElementById('lnkAtributos').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkMaps').classList.remove("navActivo");
    document.getElementById('lnkMaps').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkDocumentos').classList.remove("navActivo");
    document.getElementById('lnkDocumentos').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkContactos').classList.remove("navActivo");
    document.getElementById('lnkContactos').childNodes[1].classList.remove("navActivo");

    if (senderid == 'lnkSites') {
        document.getElementById('lnkSites').classList.add("navActivo");
        document.getElementById('lnkSites').childNodes[1].classList.add("navActivo");
        App.ctMain1.show();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.hide();
        App.ctMain5.hide();
        App.ctMain6.hide();
        App.ctMain7.hide();
    }
    else if (senderid == 'lnkLocation') {
        document.getElementById('lnkLocation').classList.add("navActivo");
        document.getElementById('lnkLocation').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.show();
        App.ctMain3.hide();
        App.ctMain4.hide();
        App.ctMain5.hide();
        App.ctMain6.hide();
        App.ctMain7.hide();
    }
    else if (senderid == 'lnkInventory') {
        document.getElementById('lnkInventory').classList.add("navActivo");
        document.getElementById('lnkInventory').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.hide();
        App.ctMain5.hide();
        App.ctMain6.show();
        App.ctMain7.hide();
    }
    else if (senderid == 'lnkAtributos') {
        document.getElementById('lnkAtributos').classList.add("navActivo");
        document.getElementById('lnkAtributos').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.show();
        App.ctMain4.hide();
        App.ctMain5.hide();
        App.ctMain6.hide();
        App.ctMain7.hide();
    }
    else if (senderid == 'lnkMaps') {
        document.getElementById('lnkMaps').classList.add("navActivo");
        document.getElementById('lnkMaps').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.show();
        App.ctMain5.hide();
        App.ctMain6.hide();
        App.ctMain7.hide();
    }
    else if (senderid == 'lnkDocumentos') {
        document.getElementById('lnkDocumentos').classList.add("navActivo");
        document.getElementById('lnkDocumentos').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.hide();
        App.ctMain5.show();
        App.ctMain6.hide();
        App.ctMain7.hide();
    }
    else if (senderid == 'lnkContactos') {
        document.getElementById('lnkContactos').classList.add("navActivo");
        document.getElementById('lnkContactos').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.hide();
        App.ctMain5.hide();
        App.ctMain6.hide();
        App.ctMain7.show();
    }
    else {

    }

    let tab = getTabSelected();
    var fa = JSON.stringify({ items: filtrosAplicados, tab: tab, visible: sitesVisible });
    App.hdFiltrosAplicados.setValue(fa);

}

function btnTgSitesVisible(sender, registro, index) {
    sitesVisible = !sender.pressed;

    if (sitesVisible) {
        //Eliminar el filtro

        filtrosAplicados = filtrosAplicados.filter(function (obj) {
            return obj.id !== idShowHiddenSites;
        });

    } else {
        let myFilter = {
            id: idShowHiddenSites,
            name: jsMostrarEmplazamientosOcultos,
            guardado: false,
            filters: []
        }
        filtrosAplicados.push(myFilter);
    }

    ajaxAplicarFiltro(filtrosAplicados);
    mostrarFiltrosCabecera();
}

function filtrarEmplazamientosPorBuscador(sender) {
    if (!desdeTab) {
        App.hdStringBuscador.setValue(sender.value);
        ajaxAplicarFiltro(filtrosAplicados);
    }
    desdeTab = false;
}

var cmbFieldCargado = false;
function focusCmbField() {
    if (!cmbFieldCargado) {
        cmbFieldCargado = true;
        App.storeCampos.reload();
    }
}

function hidePnFilters(onlyShow) {
    let pn = App.pnAsideR;
    let btn = document.getElementById('btnCollapseAsR');
    if (stPn == 0 || onlyShow == true) {
        if (onlyShow != true) {
            displayMenuSites('pnCFilters');
        }
        pn.expand();
        btn.style.transform = 'rotate(-0deg)';
        stPn = 1;
    }
    else {
        pn.collapse();
        btn.style.transform = 'rotate(-180deg)';
        stPn = 0;
    }

}

function MostrarPnFiltros() {

    if (App.pnAsideR.collapsed) {
        pnLateralAbierto = 'Filtros';
        displayMenuSites('pnCFilters');
        hidePnFilters(true);
    }
    else {
        pnLateralAbierto = null;
        hidePnFilters(false);
    }
}

function displayMenuSites(btn) {

    if (pnLateralAbierto == 'Filtros') {
        //ocultar todos los paneles
        //App.pnAsideR.expand();
        App.pnMapFilters.hide();
        App.pnCFilters.hide();
        App.pnGridsAsideMyFilters.hide();
        App.ctAsideRInfo.hide();
        App.ctAsideR.show();
        App.lblAsideNameR.show();
        App.lblAsideNameInfo.hide();
        //App.lbButtonSitesVisibles.show();
        App.btnTgSitesVisible.show();

        if (btn == "pnGridsAsideMyFilters") {
            App.GridMyFilters.getStore().reload();
        }

        //GET componente a mostrar desde el boton por ID
        if (btn != null) {

            App[btn].show();

        }
    }
    else {
        //ocultar todos los paneles
        hidePnFilters(true);
        App.pnInfoSite.hide();
        App.pnInfoLocation.hide();
        App.pnInfoAtributos.hide();
        App.pnInfoElemento.hide();
        App.pnInfoDocumento.hide();
        App.ctAsideR.hide();
        App.ctAsideRInfo.show();
        App.lblAsideNameR.hide();
        App.lblAsideNameInfo.show();
        //App.lbButtonSitesVisibles.hide();
        App.btnTgSitesVisible.hide();
        //App.btnMoreInfoElemento.hide();
        //App.btnMoreInfoDocumento.hide();

        //App.btnMoreInfoElemento.show();

        App.pnInfoSite.show();
        //GET componente a mostrar desde el boton por ID
        if (btn != null) {

            App[btn].show();

        }
    }

}

function displayMenuSitesInfo(btn, esAbrir) {


    //ocultar todos los paneles
    hidePnFilters(true);
    App.pnInfoSite.hide();
    App.pnInfoLocation.hide();
    App.pnInfoAtributos.hide();
    App.pnInfoElemento.hide();
    App.pnInfoDocumento.hide();
    App.pnInfoContacto.hide();
    App.ctAsideR.hide();
    App.ctAsideRInfo.show();
    App.lblAsideNameR.hide();
    App.lblAsideNameInfo.show();
    //App.lbButtonSitesVisibles.hide();
    App.btnTgSitesVisible.hide();
    if (esAbrir) {
        App.btnMoreInfoElemento.hide();
        App.btnMoreInfoDocumento.hide();
        App.btnMoreInfoContacto.hide();

        if (btn == "pnInfoElemento") {
            App.btnMoreInfoElemento.show();
        }
        if (btn == "pnInfoDocumento") {
            App.btnMoreInfoDocumento.show();
        }
        if (btn == "pnInfoContacto") {
            App.btnMoreInfoContacto.show();
        }
    }

    //GET componente a mostrar desde el boton por ID
    if (btn != null) {

        App[btn].show();

    }
}

function newFilter(sender, registro, index) {
    idTemp++;
    camposListados = [];
    App.pnNewFilter.setValue("");
    App.cmbField.store.reload();
    App.textInputSearch.setValue("");
    App.pnTagContainer.removeAll();
    editandoMiFiltro = null;
}

function saveFilter(sender, registro, index) {
    if (App.pnNewFilter.value != "" && camposListados.length > 0) {

        let filtroGuardar;

        if (editandoMiFiltro == null) {
            if (filtrosAplicados.find(filt => filt.id == constTemp + idTemp)) {
                filtrosAplicados.forEach(filt => {
                    if (filt.id == constTemp + idTemp) {
                        filt.filters = camposListados;
                        filt.guardado = true;

                        filtroGuardar = filt;
                    }
                });
            }
            else {
                filtroGuardar = {
                    id: constTemp + idTemp,
                    name: App.pnNewFilter.value,
                    filters: camposListados,
                    guardado: true
                };

                filtrosAplicados.push(filtroGuardar);
            }

            mostrarFiltrosCabecera();

            newFilter();
        } else {
            //filtroGuardar = editandoMiFiltro;
            filtroGuardar = {
                id: editandoMiFiltro.id,
                name: App.pnNewFilter.value,
                filters: camposListados,
                guardado: true
            };

            filtrosAplicados.forEach(filt => {
                if (filt.id == editandoMiFiltro.id) {
                    filt.filters = camposListados;
                    filt.guardado = true;
                }
            });

            mostrarFiltrosCabecera();
        }

        ajaxAgregarFiltro(filtroGuardar);
        ajaxAplicarFiltro(filtrosAplicados);
    }
    else {
        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsAsigneNombreYCamposAFiltro, buttons: Ext.Msg.OK });
    }
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
        filtrosAplicados.push(myFilter);

        mostrarFiltrosCabecera();
        ajaxAplicarFiltro(filtrosAplicados);
    }
}

function aplyFilter(sender, registro, index) {

    if (camposListados.length > 0) {

        if (App.pnNewFilter.value == "") {
            App.pnNewFilter.setValue(jsFiltroSinNombre + " " + idTemp);
        }

        if (filtrosAplicados.find(filt => filt.id == constTemp + idTemp)) {
            filtrosAplicados.forEach(filt => {
                if (filt.id == constTemp + idTemp) {
                    filt.filters = camposListados;
                    filt.name = App.pnNewFilter.value;
                }
            });
        }
        else {
            let guardado = false;
            let id = constTemp + idTemp;

            if (editandoMiFiltro != null) {
                guardado = true;
                id = editandoMiFiltro.id;
            }

            if (filtrosAplicados.find(filt => filt.id == id)) {
                filtrosAplicados.forEach(filt => {
                    if (filt.id == id) {
                        filt.filters = camposListados;
                        filt.name = App.pnNewFilter.value;
                        guardado: guardado;
                    }
                });
            } else {
                filtrosAplicados.push({
                    id: id,
                    name: App.pnNewFilter.value,
                    guardado: guardado,
                    filters: camposListados
                });
            }
        }

        mostrarFiltrosCabecera();
        ajaxAplicarFiltro(filtrosAplicados);
    }
    else {
        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsAsigneNombreYCamposAFiltro, buttons: Ext.Msg.OK });
    }

}

function addElementFilter(sender, registro, index) {
    if (App.cmbField.selection != undefined &&
        (
            (App.textInputSearch.value != null && App.textInputSearch.value != "") ||
            (App.dateInputSearch.value != null && App.dateInputSearch.value != "") ||
            (App.numberInputSearch.value != null && App.numberInputSearch.value != "") ||
            (App.cmbTiposDinamicos.lastSelectedRecords != null)
        )) {

        let multi = false;
        let name = App.cmbField.selection.data.Name;
        let Id = App.cmbField.selection.data.Id;
        let typeData = App.cmbField.selection.data.typeData;
        let value = "";
        let valueLabel = "";
        let operator = App.cmbOperatorField.getValue();
        let requireOperador = false;

        if (App.cmbTiposDinamicos.lastSelectedRecords != null) {
            multi = true;
            App.cmbTiposDinamicos.lastSelectedRecords.forEach(x => {
                value += x.data.Id + ";";
                valueLabel += x.data.Name + ";";
            })
        }
        else if (typeData.includes("System.Int64") || typeData.includes("System.Double")) {
            value = App.numberInputSearch.value;
            valueLabel = App.numberInputSearch.value;
            requireOperador = true;
        }
        else if (typeData.includes("System.DateTime")) {
            let fecha = App.dateInputSearch.value;
            value = fecha.getDate() + "/" + (fecha.getMonth() + 1) + "/" + fecha.getFullYear();
            valueLabel = fecha.getDate() + "/" + (fecha.getMonth() + 1) + "/" + fecha.getFullYear();
            requireOperador = true;
        }
        else {
            value = App.textInputSearch.value;
            valueLabel = App.textInputSearch.value;
        }

        if (requireOperador && (App.cmbOperatorField.getValue() == null || App.cmbOperatorField.getValue() == "")) {
            Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsDebeSeleccionarOperador, buttons: Ext.Msg.OK });
        } else {
            if (requireOperador) {
                valueLabel = `${App.cmbOperatorField.selection.data.field2} ${valueLabel}`;
            }

            if (!camposListados.find(element => element.Id == Id)) {
                showNewItemFilter(Id, name, valueLabel)

                camposListados.push({
                    Name: name,
                    Value: value,
                    valueLabel: valueLabel,
                    Id: Id,
                    typeData: typeData,
                    operator: operator,
                    multi: multi,
                });

                App.cmbField.removeByValue(Id);
                App.cmbField.renderData;
                App.cmbField.setValue("");

                ocultarYResetearCampos();
                App.textInputSearch.show();
            }
        }
    }
    else {
        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsSeleccionaCampoValorFiltro, buttons: Ext.Msg.OK });
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

function mostrarFiltrosCabecera() {
    App.tagsContainer.removeAll();
    filtrosAplicados.forEach(fl => {
        let panelFiltroCabecera;

        if (fl.guardado) {
            panelFiltroCabecera = new Ext.Panel({
                Hidden: "false",
                idTree: fl.id,
                items: [
                    {
                        xtype: "netlabel",
                        text: fl.name,
                        cls: "TagSaved",
                        iconCls: "ico-filters-16px",
                    },
                    {
                        xtype: "button",
                        cls: "CloseSaved",
                        focusable: false,
                        pressedCls: "none",
                        focusCls: "none",
                        handler: desaplicarFiltroGuardado,
                    },
                ]
            });
        }
        else {
            panelFiltroCabecera = new Ext.Panel({
                Hidden: "false",
                idTree: fl.id,
                items: [
                    {
                        xtype: "netlabel",
                        text: fl.name,
                        cls: "TagTemp",
                        iconCls: "ico-filters-16px",
                    },
                    {
                        xtype: "button",
                        cls: "CloseTemp",
                        focusable: false,
                        pressedCls: "none",
                        FocusCls: "none",
                        handler: desaplicarFiltroTemporal,
                    },
                ]
            });
        }


        App.tagsContainer.add(panelFiltroCabecera);
    });
}

function desaplicarFiltroTemporal(sender, registro, index) {
    filtrosAplicados = filtrosAplicados.filter(function (obj) {
        return obj.id !== sender.getBubbleParent().idTree;
    });

    if (sender.getBubbleParent().idTree == idShowHiddenSites) {
        App.btnTgSitesVisible.setPressed(false);
        btnTgSitesVisible(App.btnTgSitesVisible);
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
        else if (activeButton[x].parentElement.id == "lnkInventory") {
            return "Inventario";
        }
        else if (activeButton[x].parentElement.id == "lnkAtributos") {
            return "Atributos";
        }
        else if (activeButton[x].parentElement.id == "lnkMaps") {
            return "Maps";
        } else if (activeButton[x].parentElement.id == "lnkDocumentos") {
            return "Documentos";
        } else if (activeButton[x].parentElement.id == "lnkContactos") {
            return "Contactos";
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

//#region FORMULARIO EMPLAZAMIENTOS

function cambiarATapEmplazamiento(sender, index) {
    var classActivo = "navActivo";
    var classBtnActivo = "btn-ppal-winForm";
    var classBtnDesactivo = "btn-secondary-winForm";

    var arrayBotones = Ext.getCmp("cntNavVistasFormEmplazamiento").ariaEl.getFirstChild().getFirstChild().dom.children;


    if (index >= 0 && index < arrayBotones.length) {
        for (let i = 0; i < arrayBotones.length; i++) {
            let cmp = Ext.getCmp(arrayBotones[i].id);
            document.getElementById(cmp.id).lastChild.classList.remove(classActivo);
            cmp.removeCls(classActivo);
            if (index == i) {
                document.getElementById(cmp.id).lastChild.classList.add(classActivo);
            }
        }

        var panels = document.getElementsByClassName("winGestion-paneles");
        for (let i = 0; i < panels.length; i++) {
            Ext.getCmp(panels[i].id).hide();
        }
        Ext.getCmp(panels[index].id).show();
        Ext.getCmp(panels[index].id).up('panel').update();

        if (index == 1) {
            App.btnGuardarAgregarEditarEmplazamiento.addCls("btnDisableClick");
        }
    }

    App.btnGuardarAgregarEditarEmplazamiento.addCls(classBtnActivo);
    App.btnGuardarAgregarEditarEmplazamiento.removeCls(classBtnDesactivo);
    FormEmplazamientosValido(sender, true, undefined);

    if (index == arrayBotones.length - 1 && App.hdAdicionalCargado.value == 'false') {
        document.getElementById('formAdditional').style.opacity = '0';
        showLoadMask(App.winGestion, function (load) {
            TreeCore.PintarCategorias(true,
                {
                    success: function (result) {
                        if (!result.Success) {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        } else {
                            document.getElementById('formAdditional').style.opacity = '1'
                            App.hdAdicionalCargado.setValue('true');
                            VaciarFormularioEmplazamiento(false);
                            load.hide();
                        }
                    }
                });
        });
    }
}

function showFormsFormEmplazamientos(sender, registro, inde) {
    var classActivo = "navActivo";
    var index = 0;

    var arrayBotones = sender.ariaEl.getParent().dom.children;
    for (let i = 0; i < arrayBotones.length; i++) {
        let cmp = Ext.getCmp(arrayBotones[i].id);
        if (cmp.id == sender.id) {
            index = i;
        }
    }
    cambiarATapEmplazamiento(sender, index);
}

function FormEmplazamientosValido(sender, valido, aux) {

    try {

        var formPanel = App.containerFormEmplazamiento;
        App.btnGuardarAgregarEditarEmplazamiento.setDisabled(false);

        if (hdEmplazamientoID.value != "" && hdEmplazamientoID.value != null) {
            Ext.each(formPanel.body.query('*'), function (item) {
                var c = Ext.getCmp(item.id);
                if (c != undefined && !c.hidden && c.isFormField &&
                    (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") &&
                        (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                    App.btnGuardarAgregarEditarEmplazamiento.setDisabled(true);
                }
            });
        }
        else {
            App.btnGuardarAgregarEditarEmplazamiento.setText(jsGuardar);
            App.btnGuardarAgregarEditarEmplazamiento.setIconCls("");
            App.btnGuardarAgregarEditarEmplazamiento.removeCls("btnDisableClick");
            App.btnGuardarAgregarEditarEmplazamiento.addCls("btnEnableClick");
            App.btnGuardarAgregarEditarEmplazamiento.removeCls("animation-text");

            Ext.each(formPanel.body.query('*'), function (item) {
                var c = Ext.getCmp(item.id);
                if (c != undefined && !c.hidden && c.isFormField &&
                    (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") &&
                        (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                    App.btnGuardarAgregarEditarEmplazamiento.setDisabled(true);
                }
            });
        }

        var classMandatory = "ico-formview-mandatory";

        // #region FORMEMPLAZAMIENTOS
        var i = 0;
        Ext.each(App.formSite.query('*'), function (value) {
            var c = Ext.getCmp(value.id);

            if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                if (!c.isValid()) {
                    i++;
                }
            }
        });

        if (i == 0) {
            App.lnkFormSite.removeCls(classMandatory);
        }
        else {
            App.lnkFormSite.addCls(classMandatory);
        }

        // #endregion

        // #region FORMLOCATION
        var j = 0;

        if (!App.geoEmplazamiento_txtDireccion.isValid()) {
            j++;
        }
        else if (!App.geoEmplazamiento_cmbMunicipioProvincia.isValid()) {
            j++;
        }
        else if (!App.geoEmplazamiento_txtLatitud.isValid()) {
            j++;
        }
        else if (!App.geoEmplazamiento_txtLongitud.isValid()) {
            j++;
        }

        if (j == 0) {
            App.lnkFormLocation.removeCls(classMandatory);
        }
        else {
            App.lnkFormLocation.addCls(classMandatory);
        }

        // #endregion

        // #region FORMADDITIONAL
        var m = 0;
        Ext.each(App.containerFormEmplazamiento.body.query('*'), function (value) {
            var n = Ext.getCmp(value.id);

            if (n != undefined && n.isFormField && !n.allowBlank && !n.hidden && n.cls == 'txtContainerCategorias' && n.xtype != 'checkboxfield') {
                if (!n.isValid()) {
                    m++;
                    App[getIdComponente(n) + '_lbNombreAtr'].removeCls("ico-exclamacion-10px-grey");
                    App[getIdComponente(n) + '_lbNombreAtr'].addCls("ico-exclamacion-10px-red");                    
                }
                else {
                    App[getIdComponente(n) + '_lbNombreAtr'].removeCls("ico-exclamacion-10px-red");
                    App[getIdComponente(n) + '_lbNombreAtr'].addCls("ico-exclamacion-10px-grey");
                }
            }
        });

        if (m == 0) {
            App.lnkFormAdditional.removeCls(classMandatory);
        }
        else if (App.hdAllowBlank.getValue() == 'true') {
            App.lnkFormAdditional.addCls(classMandatory);
        }
        else {
            App.lnkFormAdditional.addCls(classMandatory);
        }

        // #endregion


    } catch (e) {

    }
}

function RecargarComboEmplazamiento(sender, registro, index) {
    recargarCombos([sender]);
    GenerarCodigo(App.txtCodigo, App.containerFormEmplazamiento, pautasCodigo, App.hdCodigoEmplazamientoAutogenerado);
}

function SeleccionarComboEmplazamiento(sender, registro, index) {
    sender.getTrigger(0).show();
    GenerarCodigo(App.txtCodigo, App.containerFormEmplazamiento, pautasCodigo, App.hdCodigoEmplazamientoAutogenerado);
    FormEmplazamientosValido(sender, true, undefined);
}

function addlistenerValidacionFormEmplazamientos() {

    var formPanel = App.containerFormEmplazamiento;

    Ext.each(formPanel.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.addListener('change', FormEmplazamientosValido);
        }
    });

}

function btnPrevEmplazamiento(sender, registro, index) {
    var panelActual = getPanelActualEmplazamiento(sender, registro, index);
    cambiarATapEmplazamiento(sender, --panelActual);
}

function btnNextEmplazamiento(sender, registro, index) {
    var panelActual = getPanelActualEmplazamiento(sender, registro, index);
    cambiarATapEmplazamiento(sender, ++panelActual);

}

function getPanelActualEmplazamiento(sender, registro, index) {
    var panelActual;
    var panels = document.getElementsByClassName("winGestion-paneles");
    for (let i = 0; i < panels.length; i++) {
        if (!Ext.getCmp(panels[i].id).hidden) {
            panelActual = i;
        }
    }
    return panelActual;
}

function closeWindowEmplazamiento(sender, registro, inde) {
    VaciarFormularioEmplazamiento();
}

//#endregion

//#region Formulario

var storeGrid, pautasCodigo;

function VaciarFormularioEmplazamiento(limpiar = true) {   
    App.cmbOperadores.getTrigger(0).hide();
    App.cmbEstadosGlobales.getTrigger(0).hide();
    App.cmbCategorias.getTrigger(0).hide();
    App.cmbTipos.getTrigger(0).hide();
    App.cmbTamanos.getTrigger(0).hide();
    App.cmbMonedas.getTrigger(0).hide();
    App.cmbTiposEstructuras.getTrigger(0).hide();
    App.cmbTiposEdificios.getTrigger(0).hide();
    App.hdEmplazamientoID.setValue('');

    var formPanel = App.containerFormEmplazamiento;

    Ext.each(formPanel.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            if (c.triggerWrap != undefined && c.getValue() != null && c.getValue() != "") {
                c.triggerWrap.removeCls("itemForm-novalid");
            }
            else if (c.triggerWrap != undefined && !c.allowBlank) {
                c.triggerWrap.removeCls("itemForm-valid");
                c.triggerWrap.addCls("itemForm-novalid");
            }

            if (!c.allowBlank && c.xtype != "checkboxfield") {
                c.addListener("change", anadirClsNoValido, false);
                c.addListener("focusleave", anadirClsNoValido, false);

                c.addListener("focus", cambiarLiteral, false);

                if (c.getValue() != null && c.getValue() != "") {
                    c.removeCls("ico-exclamacion-10px-red");
                    c.addCls("ico-exclamacion-10px-grey");
                }
                else {
                    c.addCls("ico-exclamacion-10px-red");
                    c.removeCls("ico-exclamacion-10px-grey");
                }
            }
            else if (c.allowBlank) {
                c.addListener("focus", cambiarBoton, true);
            }

            if (c.allowBlank != undefined && !c.allowBlank && c.cls == 'txtContainerCategorias' && c.xtype != 'checkboxfield') {
                App[getIdComponente(c) + '_lbNombreAtr'].addCls("ico-exclamacion-10px-red");
                App[getIdComponente(c) + '_lbNombreAtr'].removeCls("ico-exclamacion-10px-grey");

                c.addListener("change", cambiarMandatory, false);
                c.reset();
            }

            if (limpiar) {
                try {
                    App.txtCodigo.setValue();
                    App.txtNombre.setValue();
                    c.reset();
                } catch (e) {

                }
            }
        }
    });

    App['geoEmplazamiento' + '_' + 'cmbMunicipioProvincia'].addListener("select", SeleccionarComboEmplazamiento, false);
}

function CargarVentanaEmplazamiento(vaciar, callback) {
    try {
        if (vaciar) {
            TreeCore.GenerarCodigoEmplazamiento(
                {
                    success: function (result) {
                        if (!result.Success) {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        } else {
                            pautasCodigo = result.Result;
                            GenerarCodigo(App.txtCodigo, App.containerFormEmplazamiento, pautasCodigo, App.hdCodigoEmplazamientoAutogenerado);
                        }
                        callback(true, null);
                    }
                });
        }
        else {
            pautasCodigo = {};
            callback(true, null);
        }

        var classMandatory = "ico-formview-mandatory";
        var i = 0;

        // #region FORMEMPLAZAMIENTOS
        var i = 0;
        Ext.each(App.formSite.query('*'), function (value) {
            var c = Ext.getCmp(value.id);

            if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                if (!c.isValid()) {
                    i++;
                }
            }
        });

        if (i == 0) {
            App.lnkFormSite.removeCls(classMandatory);
        }
        else {
            App.lnkFormSite.addCls(classMandatory);
        }

        // #endregion

        // #region FORMLOCATION
        var j = 0;
        ruta = getIdComponente(sender);

        if (!App.geoEmplazamiento_txtDireccion.isValid()) {
            j++;
        }
        else if (!App.geoEmplazamiento_cmbMunicipioProvincia.isValid()) {
            j++;
        }
        else if (!App.geoEmplazamiento_txtLatitud.isValid()) {
            j++;
        }
        else if (!App.geoEmplazamiento_txtLongitud.isValid()) {
            j++;
        }

        if (j == 0) {
            App.lnkFormLocation.removeCls(classMandatory);
        }
        else {
            App.lnkFormLocation.addCls(classMandatory);
        }

        // #endregion

        if (App.hdAllowBlank.getValue() == 'true') {
            App.lnkFormAdditional.addCls(classMandatory);
        }
        else {
            App.lnkFormAdditional.removeCls(classMandatory);
        }
    }
    catch (e) {
        pautasCodigo = {};
        callback(true, null);
    }

}

function Agregar(store) {
    storeGrid = store;
    App.lnkFormSite.click()
    App.winGestion.setTitle(jsAgregar + " " + jsEmplazamiento);
    App.winGestion.show();
    showLoadMask(App.winGestion, function (load) {
        setClienteIDComponentes();
        cambiarATapEmplazamiento(App.formAgregarEditar, 0);
        VaciarFormularioEmplazamiento();
        CargarVentanaEmplazamiento(true, function Fin(fin) {
            App.btnGuardarAgregarEditarEmplazamiento.setText(jsGuardar);
            App.btnGuardarAgregarEditarEmplazamiento.setIconCls("");
            App.btnGuardarAgregarEditarEmplazamiento.removeCls("btnDisableClick");
            App.btnGuardarAgregarEditarEmplazamiento.addCls("btnEnableClick");
            App.btnGuardarAgregarEditarEmplazamiento.removeCls("animation-text");

            load.hide();
        });

    });

}

function Editar(store, EmplazamientoID) {
    storeGrid = store;
    App.lnkFormSite.click()
    App.hdEmplazamientoID.setValue(hdEmplazamientoID);
    App.winGestion.setTitle(jsEditar + " " + jsEmplazamiento);
    App.winGestion.show();

    App.btnGuardarAgregarEditarEmplazamiento.setText(jsGuardado);
    App.btnGuardarAgregarEditarEmplazamiento.addCls("btnDisableClick");
    App.btnGuardarAgregarEditarEmplazamiento.removeCls("btnEnableClick");
    App.btnGuardarAgregarEditarEmplazamiento.setIconCls("ico-tic-wh");

    showLoadMask(App.winGestion, function (load) {
        TreeCore.MostrarEditar(EmplazamientoID,
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        for (var prop in result.Result) {
                            SeleccionarValorMostrarEditar(prop, result.Result[prop]);
                        }
                        FormEmplazamientosValido("", true);
                        load.hide();
                    }
                }
            });
    });
}

function CargarStore(nombre, data) {
    var store = App[nombre];
    jQuery.each(data, function (i, val) {
        store.add(val);
    })
}

function SetDefectos(nombre, data) {
    var combo = App[nombre];
    combo.setValue(data);
    combo.resetOriginalValue();
}

function CargarStores() {
    TreeCore.CargarStores({
        success: function (resultado) {
            if (!resultado.Success) {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            } else {
                jQuery.each(resultado.Result.listas, function (i, val) {
                    CargarStore(i, val);
                });
                jQuery.each(resultado.Result.defectos, function (i, val) {
                    SetDefectos(i, val);
                });

                addlistenerValidacionFormEmplazamientos();
            }
        }
    })
}

var ContadorIntentos;

function winGestionBotonGuardarEmplazamiento() {

    ContadorIntentos = 100;

    TreeCore.AgregarEditar(false,
        {
            success: function (result) {

                if (result.Result == 'Editado') {
                    Ext.Msg.alert(
                        {
                            title: jsControlEdicion,
                            msg: jsComprobarEdicionRegistro,
                            buttons: Ext.Msg.YESNO,
                            buttonText: {
                                no: jsRecargarFormulario,
                                yes: jsSobrescribir
                            },
                            fn: ajaxAgregarEditarEmplazamientoEditado,
                            cls: 'winFormEditado',
                            width: '500px'
                        });
                } else if (result.Result == 'Codigo') {
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
                } else {
                    //App.winGestion.hide();
                    //storeGrid.reload();
                    setTimeout(function () {
                        App.btnGuardarAgregarEditarEmplazamiento.setText(jsGuardado);
                        App.btnGuardarAgregarEditarEmplazamiento.setIconCls("ico-tic-wh");
                        App.btnGuardarAgregarEditarEmplazamiento.addCls("animation-text");
                        App.btnGuardarAgregarEditarEmplazamiento.addCls("btnDisableClick");
                        App.btnGuardarAgregarEditarEmplazamiento.removeCls("btnEnableClick");
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

        TreeCore.GenerarCodigoEmplazamiento({
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    pautasCodigo = result.Result;

                    GenerarCodigoDuplicado(App.txtCodigo, App.containerFormEmplazamiento, pautasCodigo, App.hdCodigoEmplazamientoAutogenerado);

                    ContadorIntentos = ContadorIntentos - 1;

                    TreeCore.ComprobarCodigoEmplazamientoDuplicado(
                        {
                            success: function (result) {
                                if (result.Result != null && result.Result != '') {
                                    App.txtCodigo.setValue(App.hdCodigoEmplazamientoAutogenerado.value.toString());
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

function ajaxAgregarEditarEmplazamientoEditado(sender, registro, index, ruta) {

    if (sender == "yes") {
        TreeCore.AgregarEditar(true,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '' && result.Result != "Codigo" && result.Result != "Editado") {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        setTimeout(function () {
                            App.btnGuardarAgregarEditarEmplazamiento.setText(jsGuardado);
                            App.btnGuardarAgregarEditarEmplazamiento.setIconCls("ico-tic-wh");
                            App.btnGuardarAgregarEditarEmplazamiento.addCls("animation-text");
                            App.btnGuardarAgregarEditarEmplazamiento.addCls("btnDisableClick");
                            App.btnGuardarAgregarEditarEmplazamiento.removeCls("btnEnableClick");
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
    else {
        TreeCore.MostrarEditar(App.hdEmplazamientoID.value,
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    } else {
                        for (var prop in result.Result) {
                            SeleccionarValorMostrarEditar(prop, result.Result[prop]);
                        }
                        /*CargarVentanaEmplazamiento(sender, 'formAgregarEditar', false, function Fin(fin) {
                            load.hide();
                        });*/
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

function CargarPanelAdicional() {
    if (!App.hdAdicionalCargado.value) {
        TreeCore.PintarCategorias(true);
        hdAdicionalCargado.setValue(true);
    }
}

function guardarCambios(sender, registro, index) {
    ruta = getIdComponente(sender);

    showLoadMask(App.winGestion, function (load) {
        TreeCore.ComprobarEmplazamientoExiste(
            {
                success: function (result) {
                    if (result.Result == 'Editado') {
                        ajaxAgregarEditarEmplazamientoEditado('yes', registro, index, ruta);
                        load.hide();
                    }
                    else if (result.Result == 'Codigo') {
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
                    else {
                        ajaxAgregarEditarEmplazamientoEditado('yes', registro, index, ruta);
                        load.hide();
                    }

                }
            });
    });
}

function cerrarWindow(sender, registro, index) {
    ruta = getIdComponente(sender);
    forzarCargaBuscadorPredictivo = true;

    if (App.btnGuardarAgregarEditarEmplazamiento.getText() == jsGuardar && !App.btnGuardarAgregarEditarEmplazamiento.disabled) {
        Ext.Msg.alert(
            {
                title: jsAtencion,
                msg: jsMensajeCerrar,
                buttons: Ext.Msg.YESNO,
                fn: function (btn) {
                    if (btn == 'yes' || btn == 'si') {
                        ajaxAgregarEditarEmplazamientoEditado('yes', registro, index, ruta);
                    }
                    App.winGestion.hide();
                    VaciarFormularioEmplazamiento();
                }
            });
    }
    else {
        App.winGestion.hide();
        VaciarFormularioEmplazamiento();
    }
}

function cambiarBoton(sender, registro, index) {
    App.btnGuardarAgregarEditarEmplazamiento.setText(jsGuardar);
    App.btnGuardarAgregarEditarEmplazamiento.setIconCls("");
    App.btnGuardarAgregarEditarEmplazamiento.removeCls("btnDisableClick");
    App.btnGuardarAgregarEditarEmplazamiento.addCls("btnEnableClick");
    App.btnGuardarAgregarEditarEmplazamiento.removeCls("animation-text");
}

function cambiarLiteral(sender, registro, index) {

    if (App.btnGuardarAgregarEditarEmplazamiento != undefined && App[sender.id].getValue() != "") {
        App.btnGuardarAgregarEditarEmplazamiento.setText(jsGuardar);
        App.btnGuardarAgregarEditarEmplazamiento.setIconCls("");
        App.btnGuardarAgregarEditarEmplazamiento.removeCls("btnDisableClick");
        App.btnGuardarAgregarEditarEmplazamiento.addCls("btnEnableClick");
        App.btnGuardarAgregarEditarEmplazamiento.removeCls("animation-text");
    }
}

function cambiarMandatory(sender, registro, index) {
    var classMandatory = "ico-formview-mandatory";

    if (App[sender.id].getValue() != "" && App[sender.id].isValid() && App[sender.id].xtype != 'checkboxfield') {
        App.lnkFormAdditional.removeCls(classMandatory);
        App[getIdComponente(sender) + '_lbNombreAtr'].removeCls("ico-exclamacion-10px-red");
        App[getIdComponente(sender) + '_lbNombreAtr'].addCls("ico-exclamacion-10px-grey");
    }
    else if (App[sender.id].getValue() != "" && !App[sender.id].isValid()) {
        App.lnkFormAdditional.addCls(classMandatory);
    }

    if (App.btnGuardarAgregarEditarEmplazamiento != undefined && App[sender.id].getValue() != "") {
        App.btnGuardarAgregarEditarEmplazamiento.setText(jsGuardar);
        App.btnGuardarAgregarEditarEmplazamiento.setIconCls("");
        App.btnGuardarAgregarEditarEmplazamiento.removeCls("btnDisableClick");
        App.btnGuardarAgregarEditarEmplazamiento.addCls("btnEnableClick");
        App.btnGuardarAgregarEditarEmplazamiento.removeCls("animation-text");
        FormEmplazamientosValido();
    }
}

//#endregion

//#region More Info

function GenerarTab(tabla, datos) {
    var html = '';
    jQuery.each(datos, function (i, val) {
        html += '<tr class="tmpCol-tr"><td class="tmpCol-td" colspan = "3" > <span class="lblGrd">' + i + '</span><span class="dataGrd">' + ((val == null) ? '' : val) + '</span></td></tr>'
    });
    document.getElementById(tabla).innerHTML = html;
}

function CargarPanelMoreInfo(EmplazamientoID, isMore) {
    if (!App.pnAsideR.collapsed || isMore) {
        TreeCore.MostrarInfoEmplazamiento(EmplazamientoID,
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        GenerarTab('bodyTablaInfoEmplazamiento', result.Result.General);
                        GenerarTab('bodyTablaInfoLocation', result.Result.Localizacion);
                        GenerarTab('bodyTablaInfoAtributos', result.Result.Adicional);
                        displayMenuSitesInfo('pnInfoSite', true);
                        pnLateralAbierto = 'MasInfo'
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

function CargarPanelMoreInfoElemento(EmplazamientoID, ElementoID, isMore) {
    if (!App.pnAsideR.collapsed || isMore) {
        TreeCore.MostrarInfoEmplazamiento(EmplazamientoID,
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        GenerarTab('bodyTablaInfoEmplazamiento', result.Result.General);
                        GenerarTab('bodyTablaInfoLocation', result.Result.Localizacion);
                        GenerarTab('bodyTablaInfoAtributos', result.Result.Adicional);
                        TreeCore.CargarDatosElementos(ElementoID,
                            {
                                success: function (result2) {
                                    if (!result2.Success) {
                                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                    } else {
                                        GenerarTab('bodyTablaInfoElemento', result2.Result);
                                        displayMenuSitesInfo('pnInfoElemento', true);
                                    }
                                },
                                eventMask:
                                {
                                    showMask: true,
                                    msg: jsMensajeProcesando
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

}

function CargarPanelMoreInfoContacto(EmplazamientoID, ContactoID, isMore) {
    if (!App.pnAsideR.collapsed || isMore) {
        TreeCore.MostrarInfoEmplazamiento(EmplazamientoID,
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        GenerarTab('bodyTablaInfoEmplazamiento', result.Result.General);
                        GenerarTab('bodyTablaInfoLocation', result.Result.Localizacion);
                        GenerarTab('bodyTablaInfoAtributos', result.Result.Adicional);
                        TreeCore.CargaDatosContacto(ContactoID,
                            {
                                success: function (result2) {
                                    if (!result2.Success) {
                                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                    } else {
                                        GenerarTab('bodyTablaInfoContacto', result2.Result);
                                        displayMenuSitesInfo('pnInfoContacto', true);
                                    }
                                },
                                eventMask:
                                {
                                    showMask: true,
                                    msg: jsMensajeProcesando
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

}

function CargarPanelMoreInfoDocumento(EmplazamientoID, DocumentoID, isMore) {
    if (!App.pnAsideR.collapsed || isMore) {
        TreeCore.MostrarInfoEmplazamiento(EmplazamientoID,
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        GenerarTab('bodyTablaInfoEmplazamiento', result.Result.General);
                        GenerarTab('bodyTablaInfoLocation', result.Result.Localizacion);
                        GenerarTab('bodyTablaInfoAtributos', result.Result.Adicional);
                        TreeCore.CargarDatosDocumento(DocumentoID,
                            {
                                success: function (result2) {
                                    if (!result2.Success) {
                                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result2.Result, buttons: Ext.Msg.OK });
                                    } else {
                                        GenerarTab('bodyTablaInfoDocumento', result2.Result);
                                        displayMenuSitesInfo('pnInfoDocumento', true);
                                    }
                                },
                                eventMask:
                                {
                                    showMask: true,
                                    msg: jsMensajeProcesando
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

}

//#endregion


