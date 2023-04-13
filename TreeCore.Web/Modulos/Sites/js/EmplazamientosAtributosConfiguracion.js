Ext.onReady(function () {
    DragDropCategorias();
    DragDropAtributosCategorias();
});

// INICIO CLIENTES

function CargarStores() {

}

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

// FIN CLIENTES

function SeleccionarCategoriaLibres(sender, registro) {
    var CatID = registro[0].data.EmplazamientoAtributoCategoriaID;
    TreeCore.SeleccionarNuevaCategoria(CatID,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                sender.reset();
                DragDropCategorias();
                DragDropAtributosCategorias();
                App.storeCategoriasLibres.reload();
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}
function LimpiarcmbCategoriaLibres(sender) {
    sender.reset();
}

function showLoadMaskCategorias(callback) {
    var myMask = new Ext.LoadMask({
        msg: App.jsCargando,
        target: App.vwSites
    });

    myMask.show();
    callback(myMask, null);
}

function CambiarRestriccionDefecto(sender) {
    if (sender != App.btnRestriccionActive) {
        App.btnRestriccionActive.enable();
    }
    if (sender != App.btnRestriccionDisabled) {
        App.btnRestriccionDisabled.enable();
    }
    if (sender != App.btnRestriccionHidden) {
        App.btnRestriccionHidden.enable();
    }
    sender.disable();
    TreeCore.CambiarRestriccionDefecto(sender.modo);
}