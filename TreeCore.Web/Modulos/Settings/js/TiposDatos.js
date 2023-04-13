var Agregar = false;
var seleccionado;


//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;

        App.storeDetalle.reload();
        App.storeTiposDatosOperadores.reload();

        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnDefecto.enable();
        App.btnActivar.enable();

        App.btnAnadirDetalle.enable();
        App.btnDescargarDetalle.enable();
        App.btnRefrescarDetalle.enable();

        App.btnAnadirOperador.enable();
        App.btnDescargarOperador.enable();
        App.btnRefrescarOperador.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDefecto.setTooltip(jsDefecto);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }

        App.ModuloID.setValue(seleccionado.TipoDatoID);
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

    App.btnAnadirDetalle.disable();
    App.btnEditarDetalle.disable();
    App.btnEliminarDetalle.disable();
    App.btnActivarDetalle.disable();
    App.btnDescargarDetalle.disable();
    App.btnRefrescarDetalle.disable();
    App.btnAnadirOperador.disable();
    App.btnEditarOperador.disable();
    App.btnEliminarOperador.disable();
    App.btnActivarOperador.disable();
    App.btnDescargarOperador.disable();
    App.btnRefrescarOperador.disable();
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


function AgregarEditar() {
    VaciarFormulario();

    showLoadMask(App.grid, function (load) {
        App.txtTipoDato.focus(false, 200);
        App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
        Agregar = true;
        App.winGestion.show();
        load.hide();
    });
}


function winGestionBotonGuardar() {
    if (App.formGestion.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
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
    showLoadMask(App.grid, function (load) {
        TreeCore.MostrarEditar(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    App.txtTipoDato.focus(false, 200);
                    App.storePrincipal.reload();
                    load.hide();
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
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

// INICIO DETALLE

// INICIO GESTION GRID

function Grid_RowSelect_Detalle(sender, registro, index) {

    var datos = registro.data;

    if (datos != null) {

        seleccionado = datos;

        seleccionadoDetalle = datos;
        App.btnEliminarDetalle.enable();
        App.btnEditarDetalle.enable();
        App.btnActivarDetalle.enable();

        App.btnEditarDetalle.setTooltip(jsEditar);
        App.btnEliminarDetalle.setTooltip(jsEliminar);
        App.btnAnadirDetalle.setTooltip(jsAgregar);
        App.btnRefrescarDetalle.setTooltip(jsRefrescar);
        App.btnDescargarDetalle.setTooltip(jsDescargar);

        if (seleccionado.Activo) {
            App.btnActivarDetalle.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivarDetalle.setTooltip(jsActivar);
        }
    }
}
function Grid_RowSelect_Operadores(sender, registro, index) {

    var datos = registro.data;

    if (datos != null) {

        seleccionado = datos;

        seleccionadoDetalle = datos;
        App.btnEditarOperador.enable();
        App.btnEliminarOperador.enable();
        App.btnActivarOperador.enable();

        App.btnEditarOperador.setTooltip(jsEditar);
        App.btnEliminarOperador.setTooltip(jsEliminar);
        App.btnAnadirOperador.setTooltip(jsAgregar);
        App.btnRefrescarOperador.setTooltip(jsRefrescar);
        App.btnDescargarOperador.setTooltip(jsDescargar);

        if (seleccionado.Activo) {
            App.btnActivarOperador.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivarOperador.setTooltip(jsActivar);
        }
    }
}

function DeseleccionarGrillaDetalle() {
    App.GridRowSelectDetalle.clearSelections();
    App.btnEditarDetalle.disable();
    App.btnEliminarDetalle.disable();
    App.btnActivarDetalle.disable();

    App.btnAnadirDetalle.setTooltip(jsAgregar);
    App.btnEditarDetalle.setTooltip(jsEditar);
    App.btnEliminarDetalle.setTooltip(jsEliminar);
    App.btnActivarDetalle.setTooltip(jsActivar);
    App.btnRefrescarDetalle.setTooltip(jsRefrescar);
    App.btnDescargarDetalle.setTooltip(jsDescargar);

}

function DeseleccionarGrillaOperadores() {
    App.GridRowSelectOperadores.clearSelections();
    App.btnEditarOperador.disable();
    App.btnEliminarOperador.disable();
    App.btnActivarOperador.disable();

    App.btnAnadirOperador.setTooltip(jsAgregar);
    App.btnEditarOperador.setTooltip(jsEditar);
    App.btnEliminarOperador.setTooltip(jsEliminar);
    App.btnActivarOperador.setTooltip(jsActivar);
    App.btnRefrescarOperador.setTooltip(jsRefrescar);
    App.btnDescargarOperador.setTooltip(jsDescargar);

}

var handlePageSizeSelectDetalle = function (item, records) {
    var curPageSize = App.storeDetalle.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeDetalle.pageSize = wantedPageSize;
        App.storeDetalle.load();
    }
}

var handlePageSizeSelectOperador = function (item, records) {
    var curPageSize = App.storeTiposDatosOperadores.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeTiposDatosOperadores.pageSize = wantedPageSize;
        App.storeTiposDatosOperadores.load();
    }
}

// FIN GESTION GRID

// #region COMBOS GESTION DETALLES

function RecargarCombo() {
    recargarCombos([this]);
}

function SeleccionarCombo() {
    this.getTrigger(0).show();
}


// #endregion

function VaciarFormularioDetalle() {
    App.formGestionDetalle.getForm().reset();
}

function FormularioValidoDetalle(valid) {
    if (valid) {
        App.btnGuardarDetalle.setDisabled(false);
    }
    else {
        App.btnGuardarDetalle.setDisabled(true);
    }
}

function AgregarEditarDetalle() {
    VaciarFormularioDetalle();
    showLoadMask(App.gridDetalle, function (load) {
        recargarCombos([App.cmbTiposValores, App.cmbTiposPropiedades], function Fin(fin) {
            if (fin) {
                App.winGestionDetalle.setTitle(jsAgregarPropiedad);
                App.winGestionDetalle.show();
                load.hide();
            }
        });
    });
}

function winGestionBotonGuardarDetalle() {
    if (App.formGestionDetalle.getForm().isValid()) {
        ajaxAgregarEditarDetalle();
    }
    else {

        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }

}

function ajaxAgregarEditarDetalle() {
    var Agregar = false;

    if (App.winGestionDetalle.title.startsWith(jsAgregar)) {
        Agregar = true;
    }
    TreeCore.AgregarEditarDetalle(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestionDetalle.hide();
                    App.storeDetalle.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function MostrarEditarDetalle() {
    if (registroSeleccionado(App.gridDetalle) && seleccionado != null) {
        ajaxEditarDetalle();
    }
}

function ajaxEditarDetalle() {
    VaciarFormularioDetalle();
    App.winGestionDetalle.setTitle(jsEditarPropiedad);
    showLoadMask(App.gridDetalle, function (load) {
        recargarCombos([App.cmbTiposValores, App.cmbTiposPropiedades], function Fin(fin) {
            if (fin) {
                TreeCore.MostrarEditarDetalle(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            App.storeDetalle.reload();
                            load.hide();
                        }
                    });
            }
        });
    });
}

function EliminarDetalle() {
    if (registroSeleccionado(App.gridDetalle) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + " " + jsPropiedad,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarDetalle,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarDetalle(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarDetalle({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeDetalle.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function ActivarDetalle() {
    if (registroSeleccionado(App.gridDetalle) && seleccionado != null) {
        ajaxActivarDetalle();
    }
}

function ajaxActivarDetalle() {

    TreeCore.ActivarDetalle(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeDetalle.reload();
                }

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function RefrescarDetalle() {
    App.storeDetalle.reload();
    DeseleccionarGrillaDetalle();
}

// FIN DETALLE


// #region DATO OPERADOR

function VaciarFormularioOperador() {
    App.formGestionOperador.getForm().reset();
}

function FormularioValidoOperador(valid) {
    if (valid) {
        App.btnGuardarOperador.setDisabled(false);
    }
    else {
        App.btnGuardarOperador.setDisabled(true);
    }
}

function AgregarEditarOperador() {
    VaciarFormularioOperador();
    showLoadMask(App.gridOperadores, function (load) {
        App.winGestionOperador.setTitle(jsAgregar);
        App.winGestionOperador.show();
        load.hide();
    });
}

function winGestionBotonGuardarOperador() {
    if (App.formGestionOperador.getForm().isValid()) {
        ajaxAgregarEditarOperador();
    }
    else {

        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }

}

function ajaxAgregarEditarOperador() {
    var Agregar = false;

    if (App.winGestionOperador.title.startsWith(jsAgregar)) {
        Agregar = true;
    }
    TreeCore.AgregarEditarOperador(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestionOperador.hide();
                    App.storeTiposDatosOperadores.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function MostrarEditarOperador() {
    if (registroSeleccionado(App.gridOperadores) && seleccionado != null) {
        ajaxEditarOperador();
    }
}

function ajaxEditarOperador() {
    VaciarFormularioOperador();
    App.winGestionOperador.setTitle(jsEditar);
    TreeCore.MostrarEditarOperador(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.storeTiposDatosOperadores.reload();
            }
        });
}

function EliminarOperador() {
    if (registroSeleccionado(App.gridOperadores) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + " " + jsOperador,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarOperador,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarOperador(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarOperador({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeTiposDatosOperadores.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function ActivarOperador() {
    if (registroSeleccionado(App.gridOperadores) && seleccionado != null) {
        ajaxActivarOperador();
    }
}

function ajaxActivarOperador() {

    TreeCore.ActivarOperador(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeTiposDatosOperadores.reload();
                }

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function RefrescarOperador() {
    App.storeTiposDatosOperadores.reload();
    DeseleccionarGrillaOperadores();
}



//#endregion


// #region PLANTILLA
var PuntoCorteL = 900;
var PuntoCorteS = 512;

var selectedCol = 0;
var isOnMobC = 0;

function ControlSlider(sender) {
    var containerSize = Ext.get('CenterPanelMain').getWidth();


    var pnmain = App.grid;
    var col2 = Ext.getCmp('pnCol1');
    var tbsliders = Ext.getCmp('tbSliders');
    var btnPrevSldr = Ext.getCmp('btnPrevSldr');
    var btnNextSldr = Ext.getCmp('btnNextSldr');


    //state 2 cols

    if (containerSize > PuntoCorteL) {
        pnmain.show();
        col2.show();
        selectedCol = 1;

        isOnMobC = 0;

    }

    if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {
        pnmain.show();



        if (selectedCol == 3) {
            col2.hide();
        }
        else {
            col2.show();
        }
        isOnMobC = 0;




    }


    // state 1 col
    if (containerSize < PuntoCorteS && isOnMobC == 0) {
        pnmain.show();
        col2.hide();

        btnPrevSldr.disable();
        btnNextSldr.enable();

        selectedCol = 1;

        isOnMobC = 1;
    }



    //CONTROL SHOW OR HIDE BOTONES SLIDER
    if (pnmain.hidden == true || col2.hidden == true) {

        tbsliders.show();

        if (pnmain.hidden != true && col2.hidden == false) {
            btnPrevSldr.disable();

        }

    }
    else {


        tbsliders.hide();
        btnPrevSldr.disable();
        btnNextSldr.enable();


    }

}


function SliderMove(NextOrPrev) {
    var containerSize = Ext.get('CenterPanelMain').getWidth();


    var btnPrevSldr = Ext.getCmp('btnPrevSldr');
    var btnNextSldr = Ext.getCmp('btnNextSldr');


    var pnmain = Ext.getCmp('grid');
    var col1 = Ext.getCmp('pnCol1');

    //SELECCION EN 2  PANELES
    if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {

        if (NextOrPrev == 'Next') {
            col1.hide();
            selectedCol = 3;

            btnPrevSldr.enable();
            btnNextSldr.disable();

        }
        else if (NextOrPrev == 'Prev') {

            col1.show();
            selectedCol = 2;

            btnPrevSldr.disable();
            btnNextSldr.enable();

        }
    }

    //SELECCION EN 1  PANEL
    else {

        if (NextOrPrev == 'Next' && selectedCol == 1) {
            pnmain.hide();
            col1.show();
            selectedCol = 2;

            btnPrevSldr.enable();
            btnNextSldr.disable();

        }
        else if (NextOrPrev == 'Prev' && selectedCol == 2) {
            pnmain.show();
            col1.hide();
            selectedCol = 1;

            btnPrevSldr.disable();
            btnNextSldr.enable();

        }


    }


}

// #endregion