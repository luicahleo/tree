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

function DeseleccionarGrilla() {
    App.GridRowSelect.clearSelections();
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

function VaciarFormulario() {
    App.formGestion.getForm().reset();
}

function FormularioValido(valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function Refrescar() {
    forzarCargaBuscadorPredictivo = true;
    App.storePrincipal.reload();
    App.GridRowSelect.clearSelections();
}

// FIN GESTION 

// INICIO STORES

function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

// FIN STORES
