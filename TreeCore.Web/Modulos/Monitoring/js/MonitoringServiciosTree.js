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
        //App.btnEditar.enable();
        //App.btnEliminar.enable();
        //App.btnDefecto.enable();
        //App.btnActivar.enable();

        //App.btnEditar.setTooltip(jsEditar);
        //App.btnEliminar.setTooltip(jsEliminar);
        //App.btnDefecto.setTooltip(jsDefecto);

        //if (seleccionado.Activo) {
        //    App.btnActivar.setTooltip(jsDesactivar);
        //}
        //else {
        //    App.btnActivar.setTooltip(jsActivar);
        //}
    }
}

function DeseleccionarGrilla() {
    //App.btnAnadir.enable();
    //App.GridRowSelect.clearSelections();
    //App.btnEditar.disable();
    //App.btnActivar.disable();
    //App.btnEliminar.disable();
    //App.btnDefecto.disable();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

//FIN GESTION GRID 

// INICIO GESTION 


function Refrescar() {
    forzarCargaBuscadorPredictivo = true;
    App.storePrincipal.reload();
    App.GridRowSelect.clearSelections();
}

// FIN GESTION 

// INICIO CLIENTES


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

// FIN CLIENTES
