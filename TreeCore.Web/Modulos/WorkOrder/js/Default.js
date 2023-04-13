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