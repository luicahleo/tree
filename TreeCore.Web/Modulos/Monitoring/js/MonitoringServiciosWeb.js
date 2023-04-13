var Agregar = false;
var seleccionado;

function makeSafe() {
    document.getElementById('txaParametro').value = window.escape(document.getElementById('txaParametro').value);
};

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

// FIN GESTION 

// INICIO CLIENTES


function RecargarClientes() {
    recargarCombos([App.cmbClientes]);
    App.hdCliID.setValue(0);
    CargarStores();
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

// FIN CLIENTES

function SeleccionarComando(command, value) {
    if (command == "ParametroEntrada") {
        if (value.data != null) {
            TreeCore.MostrarParametros(true, value.id,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                    },
                    eventMask:
                    {
                        showMask: true,
                        msg: jsMensajeProcesando
                    }
                });
        }
    }
    else if (command == "ParametroSalida") {
        if (value.data != null) {
            TreeCore.MostrarParametros(false, value.id,
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                    },
                    eventMask:
                    {
                        showMask: true,
                        msg: jsMensajeProcesando
                    }
                });
        }
    }
}

