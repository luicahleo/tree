
// #region DIRECT METHOD

var Agregar = false;
var seleccionado;
//INICIO GESTION GRID 
function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;

        let registroSeleccionado = registro;
        let GridSeleccionado = App.grid;
        cargarDatosPanelMoreInfoGrid(registroSeleccionado, GridSeleccionado);
    }
}

function RecargarClientes() {
    recargarCombos([App.cmbClientes]);
    App.hdCliID.setValue(0);
    CargarStores();
}

function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

function SeleccionarCliente() {
    App.cmbClientes.getTrigger(0).show();
    App.hdCliID.setValue(App.cmbClientes.value);
    CargarStores();
}

function Refrescar() {
    forzarCargaBuscadorPredictivo = true;
    App.storePrincipal.reload();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

function asignarTraduccion(sender, registro, index) {

    if (index != undefined && index.data.AliasModulo != "" && index.data.AliasModulo.includes("str")) {
        var valor = array.find(x => x.key == index.data.AliasModulo).valor;
        return valor;
    }
    else if (sender != "") {
        var valor = array.find(x => x.key == sender).valor;
        return valor;
    }
}

function asignarCodigo(sender, registro, index) {

    if (index != undefined && index.data.CodigoAccion != "" && index.data.CodigoAccion.includes("str")) {
        var valor = array.find(x => x.key == index.data.CodigoAccion).valor;
        return valor;
    }
    else if (sender != "") {
        var valor = array.find(x => x.key == sender).valor;
        return valor;
    }
}

// #endregion

