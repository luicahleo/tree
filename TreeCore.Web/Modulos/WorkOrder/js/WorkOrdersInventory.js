var Agregar = false;
var seleccionado;

//#region DIRECT METHODS

function AgregarEditar() {
}

function winGestionBotonGuardar() {

}

function ajaxAgregarEditar() {

}

function MostrarEditar() {

}

function ajaxEditar() {

}

function Eliminar() {

}

function ajaxEliminar(button) {

}

function Refrescar() {

}

function openReinvestment() {
    App.winReinvestmentProcess.show();
}

//#endregion

function MostrarPanelMoreInfo(sender) {
    parent.CargarPanelMoreInfo(sender.$widgetRecord.data.Code, true);
}

// #region DISEÑO

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

    if (bShowOnlySecundary) {
        App.gridWOInventory.hide();
        App.btnCloseShowVisorTreeP.setIconCls('ico-moverow-gr');
    }
    else {
        App.pnCol1.show();
        App.gridWOInventory.show();
        App.btnCloseShowVisorTreeP.setIconCls('ico-hide-menu');
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
        App.gridWOInventory.show();
        App.pnCol1.hide();
        App.btnCloseShowVisorTreeP.setIconCls('ico-hide-menu');
    }
    if (iSelectedPanel == 1) {
        App.gridWOInventory.hide();
        App.pnCol1.show();
        App.btnCloseShowVisorTreeP.setIconCls('ico-moverow-gr');
    }
}

function showOnlySecundary() {
    let puntoCorte = 512;
    var tmn = App.CenterPanelMain.getWidth();

    if (tmn < puntoCorte) {
        bShowOnlySecundary = false;
        loadPanelByBtns('Prev')
    }
    else {
        bShowOnlySecundary = !bShowOnlySecundary;
        loadPanels();
    }
}

// #endregion

// #region RENDERS

var StatusRender = function (value) {
    var percent = value.split("% ");
    valor = parseInt(percent[0]);
    dato = percent[1].toString();

    if (valor <= "15") {
        return '<span class="ico-progressRedGrid">&nbsp;' + dato + '</span>'
    }
    else if (valor > "15" && valor <= "80") {
        return '<span class="ico-progressYellowGrid">&nbsp;' + dato + '</span>'
    }
    else if (valor > "80" && valor <= "100") {
        return '<span class="ico-progressBlueGrid">&nbsp;' + dato + '</span>'
    }
    else {
        return '<span>' + value + '</span>'
    }
}

// #endregion

// #region CONTROL NAVEGACION WIN SYNDATES

var clase = 'navActivo';

function NavegacionWinSyndates(who) {
    App.ctElement.hide();
    App.pnSchedule.hide();

    if (who != undefined) {
        var LNo = who.textEl;

        switch (who.id) {
            case 'lnkElement':
                ChangeTab(LNo);
                App.lblSyndates.show()
                App.ctElement.show();
                App.pnSchedule.hide();
                break;
            case 'lnkSchedule':
                ChangeTab(LNo);
                App.ctElement.hide();
                App.lblSyndates.hide()
                App.pnSchedule.show();
                break;
            default:
                ChangeTab(LNo);
                App.ctElement.show();
                break;
        }
    } else {

        ChangeTab();
        document.getElementById('lnkElement').lastChild.classList.add(clase);
        App.ctElement.show();
    }

}

function ChangeTab(vago) {
    let ct = document.getElementById('TbNavegacionTabs-innerCt');
    let aLinks = ct.querySelectorAll('a');

    aLinks.forEach(function (itm) {
        itm.classList.remove("navActivo");
    });

    if (vago != undefined) {
        vago.classList.add('navActivo');
    }

}

// #endregion

function winFormCenterSimple(obj) {
    obj.center();
    obj.updateLayout();
}

function winFormResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(650);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }

    obj.center();
}

function formResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(650);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(620);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
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

});