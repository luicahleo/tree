//#region Botones Toolbar Grid

function RefrescarWO() {
    App.storeWO.reload();
}

function ReloadCmbFiltro(sender) {
    sender.reset();
    App.storeWO.reload();
    recargarCombos([sender]);
}

function SelectCmbFiltro(sender) {
    sender.getTrigger(0).show();
    App.storeWO.reload();
}

function ClearFilters() {
    App.cmbFiltroTipos.reset();
    App.cmbFiltroTipos.getTrigger(0).hide();
    App.storeWO.reload();
}

//#endregion
//#region Botones Accion Grid

function DetalleWO() {

}

function AccionesWO() {

}

//#endregion

//#region Renders

function RenderProgress(value) {
    if (value != undefined && value != "") {
        return '<div class="progressBar">' +
            '<div class="bar" style="width:' + value + '%">' + value + '%</div>' +
            '</div>';
    }
    return "";
}

//#endregion