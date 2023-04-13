let mapForm = [];
var stPn = 0;
var filtrosAplicados = [];
var camposListados = [];
var idTemp = 1;
var constTemp = "temp-";
var dataTab = "data-tab";
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

Ext.onReady(function () {
    App.lnkSites.fireEvent("click");

    //Tabs
    App.lnkSites.ariaEl.dom.setAttribute(dataTab, "Sites");
    App.lnkLocation.ariaEl.dom.setAttribute(dataTab, "Localizaciones");
    App.lnkMaps.ariaEl.dom.setAttribute(dataTab, "Maps");
    App.lnkContacts.ariaEl.dom.setAttribute(dataTab, "Contactos");
    App.lnkContracts.ariaEl.dom.setAttribute(dataTab, "Sites");
    App.lnkInventario.ariaEl.dom.setAttribute(dataTab, "Inventario");
    App.lnkAtributos.ariaEl.dom.setAttribute(dataTab, "Atributos");
    App.lnkDocumentos.ariaEl.dom.setAttribute(dataTab, "Documentos");

    App.storeCampos.reload();

    //Panel filtros
    let pn = document.getElementById('pnAsideR');
    let btn = document.getElementById('btnCollapseAsR');
    pn.style.marginRight = '-360px';
    btn.style.transform = 'rotate(-180deg)';
    stPn = 1;
})



function ResizerAside(pn) {
    var elmnt = document.getElementById("vwResp-innerCt");

    if (elmnt != null) {
        var HeightVisorPadre = elmnt.offsetHeight;
        if (App != null) {
            App.pnAsideR.setHeight(HeightVisorPadre + 60);
            // App.pnAsideR.update();
            // App.ctAsideR.setHeight(HeightVisorPadre + 40);


        }
    }

}




// #region RESIZER JS PARA PANELES Y GRID
setTimeout(function () {

    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS
    GridResizer();


    // #region REFRESH PARA DOCKEAR WINDOW AL BOTTOM DE LA PAGINA
    var dv = document.querySelectorAll('div.winForm-respDockBot');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResizeDockBot(obj);
    }
    // #endregion

    //if (App.hugeCt != undefined) {
    //    App.hugeCt.update();
    //}
    //if (App.pnCFilters != undefined) {
    //    App.pnCFilters.update();
    //}
    //if (App.pnGridsAsideMyFilters != undefined) {
    //    App.pnGridsAsideMyFilters.update();
    //}
}, 200);


function GridResizer() {


    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);

    //const offsetHeight = document.getElementById('vwResp').offsetHeight;
    var ifrm = document.querySelector('#tabPpal iframe');
    var calcdH = vh;

    if (App.hugeCt != null) {
        //PANELES A CONTROLAR
        App.hugeCt.height = calcdH;
        if (App.pnCFilters != undefined) {
            App.pnCFilters.height = calcdH;
        }
        if (App.pnGridsAsideMyFilters != undefined) {
            App.pnGridsAsideMyFilters.height = calcdH;
        }

        // RECALC POR LAS TOOLBARS O ESPACIADO SUPERIOR
        App.hugeCt.height = calcdH - 140;
        if (App.pnCFilters != undefined) {
            App.pnCFilters.height = calcdH - 55;
        }
        if (App.pnGridsAsideMyFilters != undefined) {
            App.pnGridsAsideMyFilters.height = calcdH - 55;
        }
    }
}

// #endregion


// #region REsizer parawindow MODALES INTERNOS PARA CLASE GRID (DIRECCIONES CONTACTOS ETC)
function winFormResizeDirs(obj) {

    var res = obj.width;
    let ele = document.getElementById('FormAddDirecciones-innerCt');
    let ele2 = document.getElementById('FormAcceso-innerCt');


    if (res <= 670) {



        ele.classList.add('grid1col');
        ele2.classList.add('grid1col');
        App.winAddDirecciones.update();

    }
    else {


        ele.classList.remove('grid1col');
        ele2.classList.remove('grid1col');
        App.winAddDirecciones.update();



    }



}


function winFormResizeContacts(obj) {

    var res = obj.width;
    let ele = document.getElementById('FormContacto-innerCt');
    let ele2 = document.getElementById('FormDirContacto-innerCt');


    if (res <= 670) {



        ele.classList.add('grid1col');
        ele2.classList.add('grid1col');
        App.WinAddContactos.update();

    }
    else {


        ele.classList.remove('grid1col');
        ele2.classList.remove('grid1col');
        App.WinAddContactos.update();



    }

}



function winFormResizeIncProjects(obj) {

    var res = obj.width;
    let ele = document.getElementById('btnActivos');
    let ele2 = document.getElementById('cmbProyectosFiltros');

    if (res <= 670 && res > 400) {

        ele.classList.add('PosUnsFloatL');
        ele.classList.remove('PosUnsFloatR');
        ele2.classList.remove('repostion-cmbitem');

        App.winIncludeProjects.update();

    }



    else if (res <= 400) {


        ele.classList.add('PosUnsFloatL');
        ele.classList.remove('PosUnsFloatR');
        ele2.classList.add('repostion-cmbitem');

        App.winIncludeProjects.update();

    }

    else {


        ele.classList.remove('PosUnsFloatL');
        ele.classList.add('PosUnsFloatR');

        ele2.classList.remove('repostion-cmbitem');

        App.winIncludeProjects.update();



    }





}




function winFormResizeWinIncludeNewProjForm(obj) {

    var res = obj.width;
    let ele = document.getElementById('FormAddProyectoIncluido-innerCt');
    let ele2 = document.getElementById('txtareaComments');
    let ele3 = document.getElementById('Toolbar3');

    ele2.style.width = "200%";

    if (res <= 670) {

        ele.classList.add('grid1col');
        ele2.style.width = "100% ";
        ele3.classList.add('tbExtraBottomSpaceRes');


    }
    else {


        ele.classList.remove('grid1col');
        ele2.style.width = "200%";
        ele3.classList.remove('tbExtraBottomSpaceRes');

    }

}


// #endregion


//  #region RESIZERS PARA VENTANAS MODALES (CALCULO EXTERNO)

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

}



function winFormResizeDockBot(obj) {


    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }
    obj.center();


    //AQUI SE SETEA EL CENTER ABAJO

    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);
    obj.setY(vh - obj.height);


    //obj.update();


}

function formResize(obj) {
    var res = window.innerWidth;


    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(620);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
    obj.center();


}



window.addEventListener('resize', function () {
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


    //var frm = document.querySelectorAll('div.formResp');
    //for (i = 0; i < frm.length; i++) {
    //    var form = Ext.getCmp(frm[i].id);
    //    var win = Ext.getCmp(dv[i].id);
    //    WinFormResize(form, win)

    //}



    var dv = document.querySelectorAll('div.winForm-respDockBot');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResizeDockBot(obj);
    }



});


// #endregion

function hidePnFilters() {
    let pn = document.getElementById('pnAsideR');
    let btn = document.getElementById('btnCollapseAsR');
    if (stPn == 0) {
        pn.style.marginRight = '-360px';
        btn.style.transform = 'rotate(-180deg)';
        stPn = 1;
    }
    else {
        pn.style.marginRight = '0';
        btn.style.transform = 'rotate(0deg)';
        stPn = 0;
    }

}


var stSldr = 0;
var stSldrMbl = 0;
function moveCtSldr(btn) {
    let btnPrssd = btn.id;
    let ct1 = document.getElementById('ctMain1');
    let ct2 = document.getElementById('ctMain2');
    let ct3 = document.getElementById('ctMain3');
    var res = window.innerWidth;


    if (res > 480) {


        if (stSldr == 0 && btnPrssd == 'btnPrevSldr') {
            App.ctMain2.hide();
            App.btnPrevSldr.disable();
            App.btnNextSldr.enable();

            stSldr = 1;

        }
        else if (stSldr != 0 && btnPrssd == 'btnNextSldr') {
            App.ctMain2.show();
            App.btnPrevSldr.enable();
            App.btnNextSldr.disable();
            stSldr = 0;

        }

    }

    else if (res <= 480) {

        if (stSldrMbl == 0 && btnPrssd == 'btnPrevSldr') {
            App.ctMain1.hide();
            App.btnNextSldr.enable();
            stSldrMbl = 1;

        }

        else if (stSldrMbl == 1 && btnPrssd == 'btnPrevSldr') {
            App.ctMain2.hide();
            App.btnPrevSldr.disable();
            App.btnNextSldr.enable();
            stSldrMbl = 2;

        }

        else if (stSldrMbl == 1 && btnPrssd == 'btnNextSldr') {
            App.ctMain1.show();
            App.btnPrevSldr.enable();
            App.btnNextSldr.disable();
            stSldrMbl = 0;

        }

        else if (stSldrMbl == 2 && btnPrssd == 'btnNextSldr') {
            App.ctMain2.show();
            App.btnPrevSldr.disable();
            App.btnNextSldr.enable();
            stSldrMbl = 1;

        }
    }


}


//#region BOTONES NAV



function ajaxLoadPageTab(PaginaACargar, NombrePagina) {
    let tab = getTabSelected();
    var achjs = PaginaACargar.split('/');
    achjs.pop(); achjs = achjs.join('/') + '/js/' + NombrePagina + '.js';
    AñadirScriptjs(achjs);
    hdTabName.value = PaginaACargar;
    hdPageName.value = NombrePagina;
    TreeCore.LoadUserControl(PaginaACargar, NombrePagina, true,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    setClienteIDComponentes();

                    var fa = JSON.stringify({ items: filtrosAplicados, tab: tab, visible: sitesVisible });
                    sJsonFiltrosAplicados = fa;

                    ajaxAplicarFiltro(filtrosAplicados);
                }
            },
            eventMask:
            {
                showMask: true,
                // msg: jsMensajeProcesando
            }
        });
}

function habilitaLnk(vago) {
    let ct = document.getElementById('conNavVistas-innerCt');
    let aLinks = ct.querySelectorAll('a');

    aLinks.forEach(function (itm) {
        itm.classList.remove("navActivo");
    });

    vago.classList.add('navActivo');
}


function showForms(who, PaginaACargar, NombrePagina, script) {
    var LNo = who.textEl;

    //HIDE AL BOTON DE FILTROS MAPAS
    App.btnMapFilters.hide();
    App.pnMapFilters.hide();
    map = null;
    searchBox = null;
    desdeTab = true;

    App.hdStringBuscador.setValue("");
    App.hdIDEmplazamientoBuscador.setValue("");

    switch (who.id) {
        case 'lnkSites':
        case 'lnkContacts':
        case 'lnkLocation':
        case 'lnkDocumentos':
        case 'lnkAtributos':
        case 'lnkInventario':
            AñadirScriptjs(script, function () {

                habilitaLnk(LNo);
                ajaxLoadPageTab(PaginaACargar, NombrePagina);
                mapForm = [];

            });
            break;

        case 'lnkContracts':
        case 'lnkAdditional':
            habilitaLnk(LNo);
            ajaxLoadPageTab(PaginaACargar, NombrePagina);
            break;

        case 'lnkMaps':
            AñadirScriptjs(script, function () {
                habilitaLnk(LNo);
                ajaxLoadPageTab(PaginaACargar, NombrePagina);
                //App.btnMapFilters.show();
            });
            break;

    }
}



//#endregion


function showWinForms(who) {
    var LNo = who.textEl;


    App.FormAddDirecciones.hide();
    App.FormAcceso.hide();
    App.FormComentarios.hide();

    App.BtnAtras.hide();
    App.BtnGuardarDir.hide();
    App.BtnSiguiente.show();


    switch (who.id) {
        case 'lnkAddDirecciones':
            ChangeWinFormTab(LNo);
            App.FormAddDirecciones.show();
            break;

        case 'lnkAccesos':

            ChangeWinFormTab(LNo);
            App.FormAcceso.show();
            App.BtnAtras.show();

            break;

        case 'lnkComentarios':
            ChangeWinFormTab(LNo);
            App.FormComentarios.show();
            App.BtnGuardarDir.show();
            App.BtnAtras.show();
            App.BtnSiguiente.hide();


            break;

    }
}

function showWinFormContacts(who) {
    var LNo = who.textEl;


    App.FormContacto.hide();
    App.FormDirContacto.hide();
    App.FormNotasContacto.hide();



    App.backContacts.hide();
    App.saveContacts.hide();
    App.nextContacts.show();


    switch (who.id) {
        case 'lnkContactos':
            ChangeWinFormTabContact(LNo);
            App.FormContacto.show();
            break;

        case 'lnkDirContactos':

            ChangeWinFormTabContact(LNo);
            App.FormDirContacto.show();
            App.backContacts.show();

            break;

        case 'lnkNotasContactos':
            ChangeWinFormTabContact(LNo);
            App.FormNotasContacto.show();
            App.saveContacts.show();
            App.backContacts.show();
            App.nextContacts.hide();


            break;

    }
}


//#region FILTERS

function displayMenuSites(btn) {


    //ocultar todos los paneles

    App.pnMapFilters.hide();
    App.pnCFilters.hide();
    App.pnGridsAsideMyFilters.hide();

    if (btn == "pnGridsAsideMyFilters") {
        App.GridMyFilters.getStore().reload();
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

            filtrosAplicados.push({
                id: id,
                name: App.pnNewFilter.value,
                guardado: guardado,
                filters: camposListados
            });
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

function ajaxAplicarFiltro(filtrosAplicados) {
    let tab = getTabSelected();
    forzarCargaBuscadorPredictivo = true;

    var fa = JSON.stringify({ items: filtrosAplicados, tab: tab, visible: sitesVisible });
    var curPage = 1;

    if (App.UCGridEmplazamientos_hdFiltrosAplicados && App.UCGridEmplazamientos_cmbNumRegistros) {
        App.UCGridEmplazamientos_hdFiltrosAplicados.setValue(fa);
        curPage = App.UCGridEmplazamientos_cmbNumRegistros.value;
    }
    else if (App.UCGridEmplazamientosLocalizaciones_hdFiltrosAplicados && App.UCGridEmplazamientosLocalizaciones_cmbNumRegistros) {
        App.UCGridEmplazamientosLocalizaciones_hdFiltrosAplicados.setValue(fa);
        curPage = App.UCGridEmplazamientosLocalizaciones_cmbNumRegistros.value;
    }
    else if (App.UCGridEmplazamientosContactos_hdFiltrosAplicados && App.UCGridEmplazamientosContactos_cmbNumRegistros) {
        App.UCGridEmplazamientosContactos_hdFiltrosAplicados.setValue(fa);
        curPage = App.UCGridEmplazamientosContactos_cmbNumRegistros.value;
    }
    else if (App.UCGridEmplazamientosInventario_hdFiltrosAplicados && App.UCGridEmplazamientosInventario_cmbNumRegistros) {
        App.UCGridEmplazamientosInventario_hdFiltrosAplicados.setValue(fa);
        curPage = App.UCGridEmplazamientosInventario_cmbNumRegistros.value;
    }
    else if (App.UCGridEmplazamientosDocumentos_hdFiltrosAplicados && App.UCGridEmplazamientosDocumentos_cmbNumRegistros) {
        App.UCGridEmplazamientosDocumentos_hdFiltrosAplicados.setValue(fa);
        curPage = App.UCGridEmplazamientosDocumentos_cmbNumRegistros.value;
    }
    else if (App.UCGridEmplazamientosAtributos_hdFiltrosAplicados && App.UCGridEmplazamientosAtributos_cmbNumRegistros) {
        App.UCGridEmplazamientosAtributos_hdFiltrosAplicados.setValue(fa);
        curPage = App.UCGridEmplazamientosAtributos_cmbNumRegistros.value;
    }
    
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
    return document.getElementById("conNavVistas-innerCt").querySelector("a.navActivo").parentNode.getAttribute("data-tab");
}

//#endregion END FILTERS

function GeneralPruebaExcel() {
    TreeCore.GenerarPlantillaModeloRegional(
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



