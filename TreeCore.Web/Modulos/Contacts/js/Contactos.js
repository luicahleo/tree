
// INICIO CLIENTES


//function CargarStores() {
//    App.storePrincipal.reload();
//    DeseleccionarGrilla();
//}

//function RecargarClientes() {
//    recargarCombos([App.cmbClientes]);
//    App.hdCliID.setValue(0);
//    CargarStores();
//}

//function SeleccionarCliente() {
//    App.cmbClientes.getTrigger(0).show();
//    App.hdCliID.setValue(App.cmbClientes.value);
//    CargarStores();
//}

// FIN CLIENTES

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

var bindParams = function () {

    TreeCore.LoadPrefijos();

}