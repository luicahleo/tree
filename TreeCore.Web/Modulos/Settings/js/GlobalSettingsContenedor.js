var stPn = 1;
var pnLateralAbierto;

function MostrarPnFiltros() {
    pnLateralAbierto = 'Filtros';
    displayMenuGS('pnCFilters');
    hidePnFilters(true);
}

function displayMenuGS(btn) {

    if (pnLateralAbierto == 'Filtros') {
        //ocultar todos los paneles
        //App.pnAsideR.expand();
        App.pnQuickFilters.hide();
        App.pnCFilters.hide();
        App.pnGridsAsideMyFilters.hide();
        App.ctAsideRInfo.hide();
        App.WrapFilterControls.show();
        App.lblAsideNameR.show();
        App.lblAsideNameInfo.hide();
        //App.lbButtonSitesVisibles.show();

        if (btn == "pnGridsAsideMyFilters") {
            App.GridMyFilters.getStore().reload();
        }

        //GET componente a mostrar desde el boton por ID
        if (btn != null) {
            App[btn].show();
        }
    } else {
        //ocultar todos los paneles
        hidePnFilters(true);
        App.pnInfo.hide();
        App.WrapFilterControls.hide();
        App.ctAsideRInfo.show();
        App.lblAsideNameR.hide();
        App.lblAsideNameInfo.show();

        App.pnInfo.show();
        //GET componente a mostrar desde el boton por ID
        if (btn != null) {
            App[btn].show();
        }
    }
}

function displayMenuGSInfo(btn, esAbrir) {

    //ocultar todos los paneles
    hidePnFilters(true);
    App.pnInfo.hide();
    App.WrapFilterControls.hide();
    App.ctAsideRInfo.show();
    App.lblAsideNameR.hide();
    App.lblAsideNameInfo.show();

    //GET componente a mostrar desde el boton por ID
    if (btn != null) {

        App[btn].show();

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
        btn.style.opacity = 1;
        stPn = 1;
    }
    else {
        pn.collapse();
        btn.style.opacity = 0;
        stPn = 0;
    }

}

function CargarPanelMoreInfo(EmplazamientoID, isMore) {
    if (!App.pnAsideR.collapsed || isMore) {
        displayMenuGSInfo('pnInfo', true);
    }

}

function NavegacionTabs(sender) {
    var senderid = sender.id;
    tabToUpdate = senderid;

    document.getElementById('lnkLoginSettings').classList.remove("navActivo");
    document.getElementById('lnkLoginSettings').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkServicesSettings').classList.remove("navActivo");
    document.getElementById('lnkServicesSettings').childNodes[1].classList.remove("navActivo");

    if (senderid == 'lnkLoginSettings') {
        document.getElementById('lnkLoginSettings').classList.add("navActivo");
        document.getElementById('lnkLoginSettings').childNodes[1].classList.add("navActivo");
        App.ctMain1.show();
        App.ctMain2.hide();
    }
    else if (senderid == 'lnkServicesSettings') {
        document.getElementById('lnkServicesSettings').classList.add("navActivo");
        document.getElementById('lnkServicesSettings').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.show();
    }
    else {

    }

    //let tab = getTabSelected();
    //var fa = JSON.stringify({ items: filtrosAplicados, tab: tab, visible: sitesVisible });
    //App.hdFiltrosAplicados.setValue(fa);

}