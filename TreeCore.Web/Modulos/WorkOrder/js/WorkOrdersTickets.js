function openForm() {
    App.winGestionWorkOrders.show();
}

function openReinvestment() {
    App.winReinvestmentProcess.show();
}

function cerrarWinGestion() {
    App.winGestionWorkOrders.hide();
}

function ticketsRender() {
    var total = document.getElementsByClassName('contItem');

    if (total.length == 0) {
        App.btnSelectAll.disable();
    }
    else {
        App.btnSelectAll.enable();
    }
}

function selectionAll() {
    var tickets = document.getElementsByClassName("contItem");
    var ticketsSelect = document.getElementsByClassName("contItem contItemSelect");

    if (ticketsSelect.length == tickets.length) {
        for (var i = 0; i < tickets.length; i++) {
            tickets[i].classList.remove("contItemSelect");
        }
        App.btnSelectAll.setText("Select All");
        App.btnEliminar.disable();
    }
    else {
        for (var i = 0; i < tickets.length; i++) {
            tickets[i].classList.add("contItemSelect");
        }
        App.btnSelectAll.setText("Deselect All");
        App.btnEliminar.enable();
    }
}

function marcarTickets(sender) {
    var classActivoCont = "contItemSelect";
    var tickets = sender.parentElement.parentElement.parentElement;

    if (tickets.classList != "contItem contItemSelect") {
        tickets.classList.add(classActivoCont);
        App.btnEliminar.enable();

        var marcados = document.getElementsByClassName(classActivoCont);
        var total = document.getElementsByClassName('contItem');

        if (marcados.length == total.length) {
            App.btnSelectAll.setText("Deselect All");
        }
    }
    else {
        tickets.classList.remove(classActivoCont);
        App.btnSelectAll.setText("Select All");

        var marcados = document.getElementsByClassName(classActivoCont);
        if (marcados.length == 0) {
            App.btnEliminar.disable();
        }
    }
}

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

//#region DIRECT METHODS

function Eliminar() {
    var chequeado = document.getElementsByClassName('contItemSelect');

    if (chequeado.length > 0) {
        for (var i = chequeado.length - 1; i >= 0; i--) {
            chequeado[i].remove();
        }
        App.btnSelectAll.setText("Select All");
    }
    ticketsRender();

    var marcados = document.getElementsByClassName('contItemSelect');
    if (marcados.length == 0) {
        App.btnEliminar.disable();
    }
}

function ajaxEliminar(button) {

}

function Refrescar() {
    location.reload();
}

//#endregion