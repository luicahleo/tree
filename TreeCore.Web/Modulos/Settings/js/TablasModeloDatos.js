var seleccionadoDetalle;
var seleccionado;




/* INICIO GRID */

/* INICIO MAESTRO */

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;
    App.hdMaestroID.setValue(1);

    if (datos != null) {
        seleccionado = datos;
        App.storeDetalle.reload();

        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnActivar.enable();

        App.btnDescargar.enable();
        App.btnAnadirDetalle.enable();
        App.btnRefrescarDetalle.enable();
        App.btnDescargarDetalle.enable();

        App.btnDescargar.setTooltip(jsDescargar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }

        App.hdDatabase.setValue(seleccionado.NombreTabla);

        App.hdDatabaseID.setValue(seleccionado.TablaModeloDatosID);

    }
    TreeCore.mostrarDetalle(seleccionado.TablaModeloDatosID);


}

function DeseleccionarGrilla() {
    App.hdMaestroID.setValue(0);
    App.storeDetalle.reload();
    App.GridRowSelect.clearSelections();

    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnAnadirDetalle.disable();
    App.btnRefrescarDetalle.disable();
    App.btnDescargarDetalle.disable();
    App.btnActivar.disable();

}

/* FIN MAESTRO */

/* INICIO DETALLE */

function Grid_RowSelect_Detalle(sender, registro, index) {

    var datos = registro.data;

    if (datos != null) {
        seleccionadoDetalle = datos;


        App.btnEliminarDetalle.enable();
        App.btnEditarDetalle.enable();
        App.btnActivarDetalle.enable();



        App.btnEditarDetalle.setTooltip(jsEditar);
        App.btnEliminarDetalle.setTooltip(jsEliminar);
        App.btnAnadirDetalle.setTooltip(jsAgregar);
        App.btnRefrescarDetalle.setTooltip(jsRefrescar);
        App.btnDescargarDetalle.setTooltip(jsDescargar);

        if (seleccionadoDetalle.Activo) {
            App.btnActivarDetalle.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivarDetalle.setTooltip(jsActivar);
        }

        App.hdColumnaID.setValue(seleccionadoDetalle.ColumnaModeloDatosID);
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

/* FIN DETALLE */

/* FIN GRID */

/* INICIO GESTION GRIDS */

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}


var handlePageSizeSelectDetalle = function (item, records) {
    var curPageSize = App.storeDetalle.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeDetalle.pageSize = wantedPageSize;
        App.storeDetalle.load();
    }
}

/* FIN GESTION GRIDS */

/* INICIO GESTION */

function RecargarPrincipal() {
    App.storePrincipal.reload();
}


/* INICIO GESTION MAESTRO */

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
    App.winGestion.setTitle(jsAgregar + " " + jsTablasModeloDatos);

    App.storeTablas.reload();
    App.storeControladores.reload();
    App.storeModulos.reload();
    App.winGestion.show();
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

    var Agregar = false;

    if (App.winGestion.title.startsWith(jsAgregar)) {
        Agregar = true;
    }

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
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    App.storeTablas.reload();
    App.storeModulos.reload();
    App.storeControladores.reload();
    App.winGestion.setTitle(jsEditar + " " + jsTablasModeloDatos);
    TreeCore.MostrarEditar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                App.storePrincipal.reload();

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function Eliminar() {
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
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
function Activar() {
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
        ajaxActivar();
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

function Refrescar() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

/* FIN GESTION MAESTRO */


/* INICIO GESTION DETALLE */

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
    App.winGestionDetalle.setTitle(jsAgregar);
    App.storeColumnas.reload();
    App.winGestionDetalle.show();

    App.cmbDataSource.disable();
    App.cmbDataSource.allowBlank = true;
    App.cmbDataSource.hide();

    App.cmbForeignKey.disable();
    App.cmbForeignKey.allowBlank = true;
    App.cmbForeignKey.hide();
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

    App.storeDetalle.reload();

}

function MostrarEditarDetalle() {
    if (registroSeleccionado(App.GridDetalle) && seleccionadoDetalle != null) {
        ajaxEditarDetalle();
    }
}

function ajaxEditarDetalle() {
    VaciarFormulario();


    showLoadMask(App.GridDetalle, function (load) {

        recargarCombos([App.cmbValue, App.cmbTipoDato, App.cmbDataSource], function Fin(fin) {

            if (fin) {
                TreeCore.MostrarEditarDetalle(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {

                                App.storeTipoDato.reload();
                                App.storeColumnasFK.reload();
                                App.storeTablasModelosDatos.reload();
                                App.winGestionDetalle.show();
                                App.winGestionDetalle.setTitle(jsEditar);
                                App.formGestionDetalle.show();
                            }

                            load.hide();
                        }
                    });
            }
        });
    });
}

function EliminarDetalle() {
    if (registroSeleccionado(App.GridDetalle) && seleccionadoDetalle != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
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
    if (registroSeleccionado(App.GridDetalle) && seleccionadoDetalle != null) {
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
/* FIN GESTION DETALL E*/

/* INICIO CLIENTE */

function RecargarClientes() {
    recargarCombos([App.cmbClientes]);
    App.hdCliID.setValue(0);
    RecargarPais();
}

function CargarStores() {
    App.storePrincipal.reload();
    DeseleccionarGrilla();
}

function SeleccionarCliente() {
    App.cmbClientes.getTrigger(0).show();
    App.hdCliID.setValue(App.cmbClientes.value);
    RecargarPais();
}

/* FIN CLIENTE */

/* MAESTRO */
function RecargarTablas() {
    App.storeTablas.reload();
    App.cmbTable.setValue("");
    App.cmbControlador.setValue("");
    App.cmbIndice.setValue("");
    App.cmbModulo.setValue("");
}

function SeleccionarTable() {
    App.cmbTable.getTrigger(0).show();
    App.hdDatabase.setValue(App.cmbTable.value);
    RecargarColumnaPadre();
}

function RecargarControladores() {
    App.storeControladores.reload();
    App.cmbControlador.setValue("");
    App.cmbIndice.setValue("");
    App.cmbModulo.setValue("");
}


function SeleccionarControlador() {
    App.cmbControlador.getTrigger(0).show();
}

function SeleccionarColumnaPadre() {
    App.cmbIndice.getTrigger(0).show();
}

function RecargarColumnaPadre() {
    App.storeColumnas.reload();
    App.cmbIndice.setValue("");
    App.cmbModulo.setValue("");
}

function RecargarModulos() {
    App.storeModulos.reload();
    App.cmbModulo.setValue("");
}

function SeleccionarModulo() {
    App.cmbModulo.getTrigger(0).show();
}


/* FIN MAESTRO */

function SeleccionarValue() {
    App.cmbValue.getTrigger(0).show();
    App.storeTipoDato.reload();

    App.cmbTipoDato.enable();
    App.cmbTipoDato.setValue(null);

    App.cmbDataSource.setValue(null);
    App.cmbDataSource.hide();
    App.cmbDataSource.allowBlank = true;

    App.cmbForeignKey.setValue(null);
    App.cmbForeignKey.hide();
    App.cmbForeignKey.allowBlank = true;
}

function SeleccionarTipoDato(sender, registro, index) {

    let Codigo = sender.selection.data.Codigo;

    App.cmbTipoDato.getTrigger(0).show();
    if (Codigo == "LISTA" || Codigo == "LISTAMULTIPLE") {
        App.cmbDataSource.enable();
        App.cmbDataSource.show();
        App.cmbDataSource.allowBlank = false;
        App.storeTablasModelosDatos.reload();
    }
    else {
        App.cmbDataSource.disable();
        App.cmbDataSource.hide();
    }
}

function RecargarDataSource() {
    App.storeTablasModelosDatos.reload();
    App.cmbDataSource.setValue("");
    App.cmbForeignKey.setValue("");
    App.cmbForeignKey.hide();
}

function SeleccionarDataSource() {
    App.cmbDataSource.getTrigger(0).show();

    if (App.cmbDataSource.rawValue.toString() != "") {

        var Columna = App.cmbValue.rawValue.toString();
        var EsID = null;
        try {
            var tam_var = Columna.length;
            EsID = Columna.substring(tam_var - 2);

        }
        catch {
            Var_Sub = "";
        }

        if (EsID == "ID") {
            App.hdDatabaseFK.setValue(App.cmbDataSource.value);
            App.cmbForeignKey.enable();
            App.cmbForeignKey.show();
            App.cmbForeignKey.allowBlank = false;
            App.storeColumnasFK.reload();
        }
        else {
            App.cmbForeignKey.disable();
            App.cmbForeignKey.allowBlank = true;
            App.cmbForeignKey.hide();
        }
    }

}

function RecargarForeignKey() {
    App.storeColumnasFK.reload();
    App.cmbForeignKey.setValue("");
}

function SeleccionarForeignKey() {
    App.cmbForeignKey.getTrigger(0).show();
}

function RecargarTipoDato() {
    App.storeTipoDato.reload();
    App.cmbTipoDato.setValue("");
    App.cmbDataSource.setValue("");
    App.cmbForeignKey.setValue("");
    App.cmbDataSource.hide();
    App.cmbForeignKey.hide();
}

function RecargarColumnas() {
    App.storeColumnas.reload();
    App.cmbValue.setValue("");
    App.cmbTipoDato.setValue("");
    App.cmbDataSource.setValue("");
    App.cmbForeignKey.setValue("");

    App.cmbDataSource.hide();
    App.cmbForeignKey.hide();
    App.cmbTipoDato.disable();
}

function RecargarCombos() {
}
/* FIN COMBOS TOOLBAR */

var template = '<span style="color:{0};">{1}</span>';

var change = function (value) {
    return Ext.String.format(template, (value > 0) ? "green" : "red", value);
};

var pctChange = function (value) {
    return Ext.String.format(template, (value > 0) ? "green" : "red", value + "%");
};
