var Agregar = false;
var seleccionado;


//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnDefecto.enable();
        App.btnActivar.enable();
        App.btnAgregarInformacionAdicional.enable();
        App.btnAreasOYM.enable();

        App.btnAnadir.setTooltip(jsAnadir);
        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnAnadir.enable();
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDefecto.setTooltip(jsDefecto);
        App.btnAreasOYM.setTooltip(jsAreasOYM);
        App.btnAgregarInformacionAdicional.setTooltip(jsAgregarInformacionAdicional);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }
    }
}

function DeseleccionarGrilla() {
    if (App.hdCliID.value == 0 || App.hdCliID.value == undefined) {
        App.btnAnadir.disable();
    } else {
        App.btnAnadir.enable();
    }
    App.GridRowSelect.clearSelections();
    App.btnEditar.disable();
    App.btnActivar.disable();
    App.btnEliminar.disable();
    App.btnDefecto.disable();
    App.btnAgregarInformacionAdicional.disable();
    App.btnAreasOYM.disable();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

var handlePageSizeSelectAreasOYM = function (item, records) {
    var curPageSize = App.storeAreasOYM.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeAreasOYM.pageSize = wantedPageSize;
        App.storeAreasOYM.load();
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

function AgregarEditar() {
    VaciarFormulario();
    App.numRadio.focus(false, 200);
    App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
    Agregar = true;
    App.winGestion.show();
}


function winGestionBotonGuardar() {
    if (App.formGestion.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsTieneRegistros, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditar() {
    TreeCore.AgregarEditar(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestion.hide();
                    App.storePrincipal.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function MostrarEditar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winGestion.setTitle(jsEditar + " " + jsTituloModulo);

    TreeCore.MostrarEditar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.numRadio.focus(false, 200);
                App.storePrincipal.reload();
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function Activar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        if (seleccionado.Defecto) {
            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsDesactivarPorDefecto, buttons: Ext.Msg.OK });
        } else {
            ajaxActivar();
        }
    }
}

function ajaxActivar() {

    TreeCore.Activar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storePrincipal.reload();
                }

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function Eliminar() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsTituloModulo,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminar(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.Eliminar({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storePrincipal.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}





function Defecto() {
    if (registroSeleccionado(App.grid) && seleccionado != null) {
        if (!seleccionado.Activo) {
            Ext.Msg.alert(
                {
                    title: jsDefecto + ' ' + jsTituloModulo,
                    msg: jsRegistroInactivoPorDefecto,
                    buttons: Ext.Msg.YESNO,
                    fn: ajaxDefecto,
                    icon: Ext.MessageBox.QUESTION
                });
        } else {
            ajaxDefecto('yes');
        }
    }
}

function ajaxDefecto(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.AsignarPorDefecto({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storePrincipal.reload();
                }
            },
            eventMask: {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    }
}

function Refrescar() {
    App.storePrincipal.reload();
    App.GridRowSelect.clearSelections();
}

// FIN GESTION 

//INICIO GESTION ADICIONAL

function BotonAgregarInformacionAdicional() {
    if (registroSeleccionado != null && seleccionado != null) {
        ajaxAgregarInformacionAdicional();
    }
    App.winGestionAdicional.setTitle(jsAgregar + ' ' + jsInformacionAdicional);
}

function ajaxAgregarInformacionAdicional() {
    App.winGestionAdicional.setTitle(jsAgregar + ' ' + jsInformacionAdicional);
    TreeCore.MostrarEditarAgregarAdicional(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                //txtModulo.focus(false, 500);
                App.storePrincipal.reload();
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function FormularioValidoGestionAdicional(valid) {
    if (valid) {
        App.btnGuardarAdicional.setDisabled(false);
    }
    else {
        App.btnGuardarAdicional.setDisabled(true);
    }
}

function winGestionBotonGuardarAdicional() {
    if (App.FormPanelAdicional.getForm().isValid()) {
        ajaxAgregarEditarAdicional();
    }
    else {
        Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditarAdicional() {
    TreeCore.AgregarEditarAdicional(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAlerta, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestionAdicional.hide();
                    App.storePrincipal.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

//FIN GESTION ADICIONAL

//INICIO GESTION AREAOYM

function BotonAreasOYM() {
    MostrarAreas();
}

function MostrarAreas() {
    App.storeAreasOYM.reload();
    App.winAreasOYM.show();
}

function DeseleccionarGrillaAreaOYM() {
    App.SeleccionarAreaOYMRowSelection.clearSelections();

}

function Grid_RowSelectAreaOYM(sender, registro, index) {
    var datos = registro.data;
    console.log(datos);
}

function GridAreaOYMLibresSeleccionar_RowSelect(sender, registro, index) {
    var datos = registro.data;

}

function BotonAgregarAreaOYM() {
    MostrarAreasLibres();
}

function MostrarAreasLibres() {
    App.storeAreasOYMLibres.reload();
    App.winAreasOYMLibres.show();
}



function BotonEliminarAreaOYM() {
    if (registroSeleccionado(App.gridAreasOYM) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsAreaOYM,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarAreaOYM,
                icon: Ext.MessageBox.QUESTION
            });
    }
    App.storeAreasOYM.reload();
}


function ajaxEliminarAreaOYM(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.QuitarArea({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeAreasOYM.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function BotonGuardarAreaOYMLibres() {
    TreeCore.AgregarArea({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsAlerta, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                App.storeAreasOYM.reload();
                App.winAreasOYMLibres.hide();
            }
        },
        eventMask:
        {
            showMask: true,
            msg: jsMensajeProcesando
        }
    });
}

function RefrescarAreaOYM() {
    App.storeAreasOYM.reload();
    App.SeleccionarAreaOYMRowSelection.clearSelections();
}

//FIN GESTION AREAOYM

//INICIO GESTION RADIO

function ActivaRadio() {
    if (App.storePrincipal.data.items.length > 0) {
        App.btnRadio.enable();
    } else {
        App.btnRadio.disable();
    }
}

function BotonRadio() {
    if (App.storePrincipal.data.items.length == 0) {
        Ext.Msg.show(
            {
                title: jsAsignarRadio,
                msg: jsAsignarRadioTodos,
                buttons: Ext.Msg.YESNO,
                fn: MostrarRadio,
                icon: Ext.MessageBox.QUESTION
            });
    } else if (App.storePrincipal.data.items.length > 1) {
        Ext.Msg.show(
            {
                title: jsAsignarRadio,
                msg: jsAsignarRadioSeleccionados,
                buttons: Ext.Msg.YESNO,
                fn: MostrarRadio,
                icon: Ext.MessageBox.QUESTION
            })
    } else {
        MostrarEditarRadio()
    }
}
function MostrarEditarRadio() {
    TreeCore.MostrarEditarRadio({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}
function MostrarRadio(button) {
    if (button == 'yes' || button == 'si') {
        App.formRadio.getForm().reset();
        App.winRadio.show();
    }

}
function winRadioBotonGuardar() {
    TreeCore.AsignarRadio({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            App.storePrincipal.reload();
            App.winRadio.hide();
        },
        eventMask:
        {
            showMask: true,
            msg: jsMensajeProcesando
        }
    });
}
function FormularioValidoRadio(valid) {
    if (valid) {
        App.btnGuardarRadio.setDisabled(false);
    }
    else {
        App.btnGuardarRadio.setDisabled(true);
    }
}

//FIN GESTION RADIO

//INICIO TRIGERS

function RecargarRegiones() {
    App.cmbRegion.clearValue();
    App.storeRegiones.reload();
}

var TriggerRegiones = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbRegion.clearValue();
            App.cmbPais.clearValue();
            App.cmbRegionPais.clearValue();
            App.cmbProvincia.clearValue();
            App.cmbMunicipio.clearValue();
            App.cmbMunicipalidad.clearValue();
            App.cmbPartido.clearValue();
            App.storePaises.reload();
            App.storeRegionesPaises.reload();
            App.storeProvincias.reload();
            App.storeMunicipios.reload();
            App.storeMunicipalidad.reload();
            App.storePartidos.reload();
            App.hdRegionID.setValue(null);
            App.hdPaisID.setValue(null);
            App.hdRegionPaisID.setValue(null);
            App.hdProvinciaID.setValue(null);
            App.hdPartidoID.setValue(null);
            App.storePrincipal.reload();
            break;
        case 1:
            RecargarRegiones();
            break;
    }
}

function RecargarPais() {
    App.cmbPais.clearValue();
    App.storePaises.reload();
}

var TriggerPais = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbPais.clearValue();
            App.cmbRegionPais.clearValue();
            App.cmbProvincia.clearValue();
            App.cmbMunicipio.clearValue();
            App.cmbMunicipalidad.clearValue();
            App.cmbPartido.clearValue();
            App.storeRegionesPaises.reload();
            App.storeProvincias.reload();
            App.storeMunicipios.reload();
            App.storeMunicipalidad.reload();
            App.storePartidos.reload();
            App.hdPaisID.setValue(null);
            App.hdRegionPaisID.setValue(null);
            App.hdProvinciaID.setValue(null);
            App.hdPartidoID.setValue(null);
            App.storePrincipal.reload();
            break;
        case 1:
            RecargarPais();
            break;
    }
}

function RecargarRegionPais() {
    App.cmbRegionPais.clearValue();
    App.storeRegionesPaises.reload();
}

var TriggerRegionPais = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbRegionPais.clearValue();
            App.cmbProvincia.clearValue();
            App.cmbMunicipio.clearValue();
            App.cmbMunicipalidad.clearValue();
            App.cmbPartido.clearValue();
            App.storeProvincias.reload();
            App.storeMunicipios.reload();
            App.storeMunicipalidad.reload();
            App.storePartidos.reload();
            App.hdRegionPaisID.setValue(null);
            App.hdProvinciaID.setValue(null);
            App.hdPartidoID.setValue(null);
            App.storePrincipal.reload();
            break;
        case 1:
            RecargarRegionPais();
            break;
    }
}

function RecargarProvincia() {
    App.cmbProvincia.clearValue();
    App.storeProvincias.reload();
}

var TriggerProvincia = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbProvincia.clearValue();
            App.cmbMunicipio.clearValue();
            App.cmbMunicipalidad.clearValue();
            App.cmbPartido.clearValue();
            App.storeMunicipios.reload();
            App.storeMunicipalidad.reload();
            App.storePartidos.reload();
            App.hdPartidoID.setValue(null);
            App.storePrincipal.reload();
            break;
        case 1:
            RecargarProvincia();
            break;
    }
}

function RecargarMunicipio() {
    App.cmbMunicipio.clearValue();
    App.storeMunicipios.reload();
}

var TriggerMunicipio = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbMunicipio.clearValue();
            App.cmbMunicipalidad.clearValue();
            App.cmbPartido.clearValue();
            App.storeMunicipalidad.reload();
            App.storePartidos.reload();
            App.hdPartidoID.setValue(null);
            App.storePrincipal.reload();
            break;
        case 1:
            RecargarMunicipio();
            break;
    }
}

function RecargarMunicipalidad() {
    App.cmbMunicipalidad.clearValue();
    App.storeMunicipalidad.reload();
}

var TriggerMunicipalidad = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbMunicipalidad.clearValue();
            App.cmbPartido.clearValue();
            App.storePartidos.reload();
            App.hdPartidoID.setValue(null);
            App.storePrincipal.reload();
            break;
        case 1:
            RecargarMunicipalidad();
            break;
    }
}

function RecargarPartido() {
    App.cmbPartido.clearValue();
    App.storePartidos.reload();
}

var TriggerPartido = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbPartido.clearValue();
            App.hdPartidoID.setValue(null);
            App.storePrincipal.reload();
            break;
        case 1:
            RecargarPartido();
            break;
    }
}

function DeletePaises() {
    App.cmbPais.clearValue();
    App.cmbRegionPais.clearValue();
    App.cmbProvincia.clearValue();
    App.cmbMunicipio.clearValue();
    App.cmbMunicipalidad.clearValue();
    App.cmbPartido.clearValue();
    App.storeRegionesPaises.reload();
    App.storeProvincias.reload();
    App.storeMunicipios.reload();
    App.storeMunicipalidad.reload();
    App.storePartidos.reload();
    App.btnAnadir.disable();
    App.btnDescargar.disable();
    App.storePrincipal.reload();
}

function DeleteRegionPais() {
    App.cmbRegionPais.clearValue();
    App.cmbProvincia.clearValue();
    App.cmbMunicipio.clearValue();
    App.cmbMunicipalidad.clearValue();
    App.cmbPartido.clearValue();
    App.storeProvincias.reload();
    App.storeMunicipios.reload();
    App.storeMunicipalidad.reload();
    App.storePartidos.reload();
    App.btnAnadir.disable();
    App.btnDescargar.disable();
    App.storePrincipal.reload();
}

function DeleteProvincia() {
    App.cmbProvincia.clearValue();
    App.cmbMunicipio.clearValue();
    App.cmbMunicipalidad.clearValue();
    App.cmbPartido.clearValue();
    App.storeMunicipios.reload();
    App.storeMunicipalidad.reload();
    App.storePartidos.reload();
    App.btnAnadir.disable();
    App.btnDescargar.disable();
    App.storePrincipal.reload();
}

function DeleteMunicipio() {
    App.cmbMunicipio.clearValue();
    App.cmbMunicipalidad.clearValue();
    App.cmbPartido.clearValue();
    App.storeMunicipalidad.reload();
    App.storePartidos.reload();
    App.btnAnadir.disable();
    App.btnDescargar.disable();
    App.storePrincipal.reload();
}

function DeleteMunicipalidad() {
    App.cmbMunicipalidad.clearValue();
    App.cmbPartido.clearValue();
    App.storePartidos.reload();
    App.btnAnadir.disable();
    App.btnDescargar.disable();
    App.storePrincipal.reload();
}

function DeletePartido() {
    App.cmbPartido.clearValue();
    App.btnAnadir.disable();
    App.btnDescargar.disable();
    App.storePrincipal.reload();
}

function RegionSeleccionar() {
    DeletePaises();
    DeleteRegionPais();
    DeleteProvincia();
    DeleteMunicipio();
    DeleteMunicipalidad();
    DeletePartido();
    App.hdRegionID.setValue(App.cmbRegion.value);

    App.storePaises.reload();
}

function PaisSeleccionar() {
    DeleteRegionPais();
    DeleteProvincia();
    DeleteMunicipio();
    DeleteMunicipalidad();
    DeletePartido();
    App.hdPaisID.setValue(App.cmbPais.value);

    App.storeRegionesPaises.reload();
}

function RegionPaisSeleccionar() {
    DeleteProvincia();
    DeleteMunicipio();
    DeleteMunicipalidad();
    DeletePartido();
    App.hdRegionPaisID.setValue(App.cmbRegionPais.value);

    App.storeProvincias.reload();
}

function ProvinciaSeleccionar() {
    DeleteMunicipio();
    DeleteMunicipalidad();
    DeletePartido();
    App.hdProvinciaID.setValue(App.cmbProvincia.value);

    App.storeMunicipios.reload();
}

function MunicipioSeleccionar() {
    DeleteMunicipalidad();
    DeletePartido();
    App.hdProvinciaID.setValue(App.cmbProvincia.value);

    App.storeMunicipalidad.reload();
}

function MunicipalidadSeleccionar() {
    DeletePartido();
    App.hdProvinciaID.setValue(App.cmbProvincia.value);

    App.storePartidos.reload();
}

function PartidoSeleccionar() {
    if (App.cmbPartido.getValue() != '') {
        App.hdPartidoID.setValue(App.cmbPartido.value);
        App.btnAnadir.enable();
        App.btnDescargar.enable();
    }
    else {
        App.btnAnadir.disable();
        App.btnDescargar.disable()
    }
    App.storePrincipal.reload();
}

//FIN TRIGERS

// INICIO CLIENTES


function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
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