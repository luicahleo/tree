var stPn = 1;
var pnLateralAbierto;

function MostrarPnFiltros() {
    pnLateralAbierto = 'Filtros';
    displayMenuWO('pnCFilters');
    hidePnFilters(true);
}

function displayMenuWO(btn) {

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
        App.pnInfoWO.hide();
        App.pnInfoWOSites.hide();
        App.pnInfoWOInventory.hide();
        App.pnInfoWOTickets.hide();
        App.pnInfoWOCalendar.hide();
        App.WrapFilterControls.hide();
        App.ctAsideRInfo.show();
        App.lblAsideNameR.hide();
        App.lblAsideNameInfo.show();

        App.pnInfoWO.show();
        //GET componente a mostrar desde el boton por ID
        if (btn != null) {
            App[btn].show();
        }
    }
}

function displayMenuWOInfo(btn, esAbrir) {


    //ocultar todos los paneles
    hidePnFilters(true);
    App.pnInfoWO.hide();
    App.pnInfoWOSites.hide();
    App.pnInfoWOInventory.hide();
    App.pnInfoWOTickets.hide();
    App.pnInfoWOCalendar.hide();
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
        displayMenuWOInfo('pnInfoWO', true);
        //    TreeCore.MostrarInfoEmplazamiento(EmplazamientoID,
        //        {
        //            success: function (result) {
        //                if (!result.Success) {
        //                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
        //                } else {
        //                    GenerarTab('bodyTablaInfoWorkOrders', result.Result.General);
        //                    GenerarTab('bodyTablaInfoWOSite', result.Result.WOSites);
        //                    GenerarTab('bodyTablaInfoWOInventory', result.Result.WOInventory);
        //                    GenerarTab('bodyTablaInfoWOTickets', result.Result.WOTickets);
        //                    GenerarTab('bodyTablaInfoWOCalendar', result.Result.WOCalendar);
        //                    displayMenuWOInfo('pnInfoSite', true);
        //                    pnLateralAbierto = 'MasInfo'
        //                }
        //            },
        //            eventMask:
        //            {
        //                showMask: true,
        //                msg: jsMensajeProcesando
        //            }
        //        });
    }

}

function NavegacionTabs(sender) {
    var senderid = sender.id;
    tabToUpdate = senderid;

    document.getElementById('lnkWorkOrders').classList.remove("navActivo");
    document.getElementById('lnkWorkOrders').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkWorkOrdersSites').classList.remove("navActivo");
    document.getElementById('lnkWorkOrdersSites').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkWorkOrdersInventory').classList.remove("navActivo");
    document.getElementById('lnkWorkOrdersInventory').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkWorkOrdersTickets').classList.remove("navActivo");
    document.getElementById('lnkWorkOrdersTickets').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkWorkOrdersCalendar').classList.remove("navActivo");
    document.getElementById('lnkWorkOrdersCalendar').childNodes[1].classList.remove("navActivo");

    if (senderid == 'lnkWorkOrders') {
        document.getElementById('lnkWorkOrders').classList.add("navActivo");
        document.getElementById('lnkWorkOrders').childNodes[1].classList.add("navActivo");
        App.ctMain1.show();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.hide();
        App.ctMain5.hide();
    }
    else if (senderid == 'lnkWorkOrdersSites') {
        document.getElementById('lnkWorkOrdersSites').classList.add("navActivo");
        document.getElementById('lnkWorkOrdersSites').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.show();
        App.ctMain3.hide();
        App.ctMain4.hide();
        App.ctMain5.hide();
    }
    else if (senderid == 'lnkWorkOrdersInventory') {
        document.getElementById('lnkWorkOrdersInventory').classList.add("navActivo");
        document.getElementById('lnkWorkOrdersInventory').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.show();
        App.ctMain4.hide();
        App.ctMain5.hide();
    }
    else if (senderid == 'lnkWorkOrdersTickets') {
        document.getElementById('lnkWorkOrdersTickets').classList.add("navActivo");
        document.getElementById('lnkWorkOrdersTickets').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.show();
        App.ctMain5.hide();
    }
    else if (senderid == 'lnkWorkOrdersCalendar' || senderid.includes('btnCalendar')) {
        document.getElementById('lnkWorkOrdersCalendar').classList.add("navActivo");
        document.getElementById('lnkWorkOrdersCalendar').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.hide();
        App.ctMain3.hide();
        App.ctMain4.hide();
        App.ctMain5.show();
    }
    else {

    }

    //let tab = getTabSelected();
    //var fa = JSON.stringify({ items: filtrosAplicados, tab: tab, visible: sitesVisible });
    //App.hdFiltrosAplicados.setValue(fa);

}