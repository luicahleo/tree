function CargarStores() {
    TreeCore.CargarStores({
        success: function (resultado) {
            if (!resultado.Success) {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            } else {
                jQuery.each(resultado.Result.listas, function (i, val) {
                    CargarStore(i, val);
                });
            }
        }
    })
}

function CargarStore(nombre, data) {
    var store = App[nombre];
    jQuery.each(data, function (i, val) {
        store.add(val);
    })
}

//CAMBIAR LISTA

function CambiarListaJS() {
    parent.App.ctMain2.show();
    parent.App.ctMain1.hide();
}

//#endregion

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
    var btnclose = Ext.getCmp(['btnCloseShowVisorTreeP']);

    if (bShowOnlySecundary) {
        App.TreePanel2.hide();
        btnclose.setIconCls('ico-moverow-gr');
    }
    else {

        App.visorInsidePn.show();

        if (bShowPrincipal) {
            App.TreePanel2.show();
            btnclose.setIconCls('ico-hide-menu');
        }
        else {
            App.TreePanel2.hide();
            btnclose.setIconCls('ico-moverow-gr');
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
        App.TreePanel2.show();
        App.visorInsidePn.hide();
    }
    if (iSelectedPanel == 1) {
        App.TreePanel2.hide();
        App.visorInsidePn.show();
    }
}

function showOnlySecundary() {

    bShowOnlySecundary = !bShowOnlySecundary;
    loadPanels();
}

// #endregion

function renderClosed(valor, id) {
    let imag = document.getElementById('imClsd' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';
    }



}

function renderMultiflow(valor, id) {

    let imag = document.getElementById('imMultiflow' + id);

    if (valor == false) {
        imag.src = '';

    }

    else {
        imag.src = '../../ima/ico-subprocess.svg';
    }


}

function renderCommercial(valor, id) {

    let imag = document.getElementById('imCommercial' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-vendor.svg';

    }


}

function renderInactive(valor, id) {
    let imag = document.getElementById('imInactive' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-cancel.svg';

    }


}

function renderProgBar(valor, id) {
    let bar = document.getElementById('progBar' + id);
    let ancho = valor;

    bar.style.width = ancho * 100 + "%";


}

function renderRegion(valor, id) {
    let imag = document.getElementById('imRegion' + id);

    if (valor == false) {
        imag.src = '';
    }

    else {
        imag.src = '../../ima/ico-region-gr.svg';

    }


}

function renderAuthorized(valor, id) {
    let imag = document.getElementById('imAuthorized' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }


}

function renderStaff(valor, id) {
    let imag = document.getElementById('imStaff' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }


}

function renderSupport(valor, id) {
    let imag = document.getElementById('imSupport' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }

}

function renderLDAP(valor, id) {
    let imag = document.getElementById('imLDAP' + id);

    if (valor == false) {
        imag.src = '../../ima/ico-cancel.svg';
    }

    else {
        imag.src = '../../ima/ico-accept.svg';

    }

}

function grdInsideH() {
    let pH = App.pnComboGrdVisor.height;
    App.grdInsidePn.height = pH;
}

function visorResizer() {

    setTimeout(function () {
        let pH = App.pnComboGrdVisor.height - 46;
        App.pnVisor.setHeight(pH);

    }, 100);


}

function displayMenu(btn) {

    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnGridsAsideR.hide();
    App.pnCFilters.hide();
    App.pnGridsAsideMyFilters.hide();
    App.pnMapFilters.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();

}

function ShowPanelAddMeds() {
    App.tbPanelAdd.show();
}
function HidePanelAddMeds() {
    App.tbPanelAdd.hide();
}

function SelectItemMenu(sender, item, id) {
    let ruta = "InventarioCategoryViewVistaCategoria.aspx?" + "EmplazamientoID=" + App.hdEmplazamientoID.value + "&CategoriaID=" + item.id + "&VistaPlantilla=" + App.hdVistaPlantilla.value;

    //Filtro de resultados por KPI
    if (App.hdIdsResultados.value && App.hdNameIndiceID.value) {
        ruta += "&idsResultados=" + App.hdIdsResultados.value + "&nameIndiceID=" + App.hdNameIndiceID.value;
    }

    addTab(App.InventoryTabPanel, item.id.toString(), item.data.InventarioCategoria, ruta);
    App.GridRowSelectArbol.clearSelections();
}

function SeleccionarCliente(sender, registro, index) {
    sender.getTrigger(0).show();
    showLoadMask(App.MainVwP, function myfunction(load) {
        App.hdCliID.setValue(registro[0].data.ClienteID);
        recargarCombos([App.cmbTipoEmplazamientos], function Fin(fin) {
            if (fin) {
                App.storeCategorias.reload();
                load.hide();
            }
        })
    })
}

function RecargarClientes(sender, registro, index) {
    showLoadMask(App.MainVwP, function myfunction(load) {
        App.hdCliID.setValue(0);
        recargarCombos([App.cmbClientes, App.cmbTipoEmplazamientos], function Fin(fin) {
            if (fin) {
                App.storeCategorias.reload();
                load.hide();
            }
        })
    })
}

function SeleccionarTipoEmplazamientos(sender, registro, index) {
    sender.getTrigger(0).show();
    showLoadMask(App.MainVwP, function myfunction(load) {
        App.storeCategorias.reload();
        load.hide();
    })
}

function RecargarTipoEmplazamientos(sender, registro, index) {
    showLoadMask(App.MainVwP, function myfunction(load) {
        recargarCombos([App.cmbTipoEmplazamientos], function Fin(fin) {
            if (fin) {
                App.storeCategorias.reload();
                load.hide();
            }
        })
    })
}

function CargarTabPrincipal(sender) {
    if (App.hdEmplazamientoID.value != 0) {
        let ruta = "InventarioCategoryViewVistaCategoria.aspx?" + "EmplazamientoID=" + App.hdEmplazamientoID.value + "&VistaInventario=" + true;

        //Filtro de resultados por KPI
        if (App.hdIdsResultados.value && App.hdNameIndiceID.value) {
            ruta += "&idsResultados=" + App.hdIdsResultados.value + "&nameIndiceID=" + App.hdNameIndiceID.value;
        }

        addTabUnClosable(App.InventoryTabPanel, "TabComun", jsTodasEntidades, ruta);
    }
}

function CambiarTabFiltros() {
    parent.hidePnTab();
    parent.App.hdCategoriaActiva.setValue(App.InventoryTabPanel.activeTab.id);
    parent.App.hdNombreCategoriaActiva.setValue(App.InventoryTabPanel.activeTab.title);
    parent.mostrarFiltrosCabecera();
}