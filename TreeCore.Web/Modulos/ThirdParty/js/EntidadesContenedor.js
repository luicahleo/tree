var stPn = 1;
var pnLateralAbierto;

function displayMenuE(btn, label) {

    //ocultar todos los paneles
    hidePnFilters(true);
    App.WrapCompany.hide();

    App.lblAsideNameInfo.hide();

    //GET componente a mostrar desde el boton por ID
    if (btn != null) {
        App[btn].show();
    }
    //if (label != null) {
    //    App[label].show();
    //}

    displayMenu('pnMoreInfo');
}

function displayMenu(btn) {
    var name = '#' + btn;
    let tabla;
    let label;
    let grid;

    //ocultar todos los paneles
    App.pnOwner.hide();
    App.pnBusinessPartner.hide();
    App.pnSupplier.hide();
    App.pnOperator.hide();
    App.pnMoreInfo.hide();

    if (btn == 'pnOwner') {
        tabla = document.getElementById('bodyTablaInfoOwner');
        //grid = pageActual.gridEstadosGlobales;
        label = App.lblTotalOwner;
        App.pnOwner.show();
    }
    else if (btn == 'pnBusinessPartner') {
        tabla = document.getElementById('bodyTablaInfoBusinessPartner');
        label = App.lblTotalBusinessPartner;
        App.pnBusinessPartner.show();
    }
    else if (btn == 'pnSupplier') {
        tabla = document.getElementById('bodyTablaInfoSupplier');
        //grid = pageActual.gridEstadosSiguientes;
        label = App.lblTotalSupplier;
        App.pnSupplier.show();
    }
    else if (btn == 'pnOperator') {
        tabla = document.getElementById('bodyTablaInfoOperator');
        label = App.lblTotalOperator;
        App.pnOperator.show();
    }
    else if (btn == 'pnMoreInfo') {
        App.pnMoreInfo.show();
        //grid = App.gridMain1;
    }

    if (btn != undefined && btn != null) {
        //TreeCore.RecargarPanelLateral(btn,
            //{
            //    success: function (result) {
            //        if (result.Success != null && !result.Success) {
            //            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Success, buttons: Ext.Msg.OK });
            //        }
            //        else {

            //            lista = JSON.parse(result.Result);

            //            //if (btn == 'pnMoreInfo') {
            //            //    //cargarDatosPanelMoreInfoGridEstados(lista, grid);
            //            //    CargarPanelMoreInfo('WrapCompany', 'lblEstados');
            //            //}
            //            //else
            //            if (btn == 'pnNotificaciones') {
            //                cargarDatosPanelNotificacionesEstados(tabla, lista, label);
            //            }
            //            else if (btn != 'pnMoreInfo') {
            //                cargarDatosPanelMoreInfoEstados(tabla, grid, lista, label);
            //            }
            //        }
            //    },
            //    eventMask: {
            //        showMask: true,
            //    }
            //});
    }

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();
    PanelAMostrar.updateLayout();


}

function hidePnFilters(onlyShow) {
    let pn = App.pnAsideR;
    let btn = document.getElementById('btnCollapseAsR');
    if (stPn == 0 || onlyShow == true) {
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
        displayMenuEInfo('pnMoreInfo', true);
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

    document.getElementById('lnkEntidades').classList.remove("navActivo");
    document.getElementById('lnkEntidades').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkEntidadesContactos').classList.remove("navActivo");
    document.getElementById('lnkEntidadesContactos').childNodes[1].classList.remove("navActivo");

    if (senderid == 'lnkEntidades') {
        document.getElementById('lnkEntidades').classList.add("navActivo");
        document.getElementById('lnkEntidades').childNodes[1].classList.add("navActivo");
        App.ctMain1.show();
        App.ctMain2.hide();
    }
    else if (senderid == 'lnkEntidadesContactos') {
        document.getElementById('lnkEntidadesContactos').classList.add("navActivo");
        document.getElementById('lnkEntidadesContactos').childNodes[1].classList.add("navActivo");
        App.ctMain1.hide();
        App.ctMain2.show();
    }
    else {

    }

    //let tab = getTabSelected();
    //var fa = JSON.stringify({ items: filtrosAplicados, tab: tab, visible: sitesVisible });
    //App.hdFiltrosAplicados.setValue(fa);

}