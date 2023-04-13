

var ComboNumber = 4;




function StyleOnResize(sender) {
    var tbFiltros = document.getElementsByClassName("tlbGridRes");
    var ruta = getIdComponente(sender);

    //CONTROL PARA CUANDO NO HAYA BOTONES, SOLO COMBO FILTROS
    var GrupoBtnFilters = document.getElementsByClassName("GrupoBtnFilters");
    var BotonesVisibles = GrupoBtnFilters[0].style.display;

    var cmbMisfiltros = document.getElementsByClassName("cmbMisfiltros");

    if (BotonesVisibles == "none") {
        cmbMisfiltros[0].classList.add("cmbMisfiltrosNoBtns");
    }

    // ------------------------------------------

    for (i = 0; i < tbFiltros.length; i++) {

        if (tbFiltros[i] != null) {

            if (tbFiltros[i].clientWidth < 612) {

                tbFiltros[i].classList.add("tlbGridResMid");

            }
            else {

                tbFiltros[i].classList.remove("tlbGridResMid");


            }


            if (tbFiltros[i].clientWidth < 460) {

                tbFiltros[i].classList.add("tlbGridResMini");

            }
            else {
                tbFiltros[i].classList.remove("tlbGridResMini");


            }

            if (tbFiltros[i].clientWidth == 0) {

                tbFiltros[i].classList.remove("tlbGridResMini");
                tbFiltros[i].classList.remove("tlbGridResMid");

            }

            App[ruta + '_' + 'tbFiltros'].updateLayout();


        }


    }


}



function loadcssfile(filetype) {

    if (ComboNumber == 1) {
        if (filetype == "css") { //if filename is an external CSS file
            var fileref = document.createElement("link")
            fileref.setAttribute("rel", "stylesheet")
            fileref.setAttribute("type", "text/css")
            fileref.setAttribute("href", "../../Componentes/css/toolbarFiltros1Combo.css")
        }
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref)
    }

    if (ComboNumber == 2) {
        if (filetype == "css") { //if filename is an external CSS file
            var fileref = document.createElement("link")
            fileref.setAttribute("rel", "stylesheet")
            fileref.setAttribute("type", "text/css")
            fileref.setAttribute("href", "../../Componentes/css/toolbarFiltros2Combo.css")
        }
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref)
    }



    if (ComboNumber == 3) {
        if (filetype == "css") { //if filename is an external CSS file
            var fileref = document.createElement("link")
            fileref.setAttribute("rel", "stylesheet")
            fileref.setAttribute("type", "text/css")
            fileref.setAttribute("href", "../../Componentes/css/toolbarFiltros3Combo.css")
        }
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref)
    }

    if (ComboNumber == 4) {
        if (filetype == "css") { //if filename is an external CSS file
            var fileref = document.createElement("link")
            fileref.setAttribute("rel", "stylesheet")
            fileref.setAttribute("type", "text/css")
            fileref.setAttribute("href", "../../Componentes/css/toolbarFiltros4Combo.css")
        }
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref)
    }


}

loadcssfile("css") ////dynamically load and add this .css file

function SelectItemMenu(sender, registro, index) {
    var seleccionado = registro.data;

    if (seleccionado != null &&
        seleccionado.RutaPagina != null && seleccionado.RutaPagina != "") {

        addTab(App.tabPpal, seleccionado.text, seleccionado.text, seleccionado.RutaPagina);
        this.getSelectionModel().deselectAll();
    }
}

function CargarCmbEstatico() {
    TreeCore.CargarCmbEstatico(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
        });
}

function LimpiarCmbEstatico(sender, registro, index) {
    App[sender.id].clearValue();
    App[sender.id].getTrigger(0).hide();
    CargarStores(sender, registro, index);
}

function SelectCmbEstatico(sender, registro, index) {
    App[sender.id].getTrigger(0).show();
    CargarStores(sender, registro, index);
}

function CargarStores(sender, registro, index) {
    
    var idComponente = sender.id.split('_');
    idComponente.pop();
    var stores = App[idComponente + "_hdStoresID"].value;
    App[stores].reload();
    //CargarStoresSerie(stores);
}

var getGridLocal = function getGrid() {

    var myComponentes = Ext.ComponentQuery.query('gridpanel(true)');
    var grid;

    myComponentes.forEach(function (item) {

        grid = Ext.getCmp(item.id);
        if (grid != undefined) {
            this.grid = grid;
        }
    });

    return grid;
}

//#region FILTROS

function FieldSearch(sender, registro) {
    var iconClear = sender.getTrigger("_trigger2");
    iconClear.hide();
}

function FiltrarColumnas(sender, registro) {
    var idPadre = getIdComponentePadre(sender);
    var idComponente = getIdComponente(sender);

    if (idPadre != "") {
        App[idPadre + "_btnDescargar"].disable();
    }
    else {
        if (App.btnDescargar != undefined) {
            App.btnDescargar.disable();
        }
        
    }

    var grid = App[idComponente + "_hdGrid"].value;

    if (idPadre != "") {
        var tree = App[idPadre + "_" + grid];
    }
    else {
        var tree = App[grid];
    }
    
    var store = tree.store,
        logic = store,
        text = sender.getRawValue();

    logic.clearFilter();

    var iconSearch = sender.getTrigger("_trigger1");
    var iconClear = sender.getTrigger("_trigger2");

    if (Ext.isEmpty(text, false)) {
        iconClear.hide();
        iconSearch.show();
        if (idPadre != "") {
            App[idPadre + "_btnDescargar"].enable();
        }
        else {
            App.btnDescargar.enable();
        }

        return;
    }

    if (!Ext.isEmpty(text, false)) {
        iconSearch.hide();
        iconClear.show();
    }

    filtroBuscador(logic, tree, text);
}

function LimpiarFiltroBusqueda(sender, registro) {
    var idPadre = getIdComponentePadre(sender);
    var idComponente = getIdComponente(sender);

    var grid = App[idComponente + "_hdGrid"].value;

    if (idPadre != "") {
        var tree = App[idPadre + "_" + grid];
    }
    else {
        var tree = App[grid];
    }

    var store = tree.store,
        logic = store,
        field = App[idComponente + "_txtSearch"];

    field.setValue("");
    logic.clearFilter();

    if (idPadre != "") {
        App[idPadre + "_btnDescargar"].enable();
    }
    else {
        if (App.btnDescargar != undefined) {
            App.btnDescargar.disable();
        }
    }
}

// #endregion

function ocultarIcon(sender, registro, icon) {
    var text = sender.getRawValue();

    if (Ext.isEmpty(text, false)) {

        icon.show();

        return;
    }
}