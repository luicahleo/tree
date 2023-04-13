////var seleccionadoDetalle;
var seleccionado;
var Editando = "";

// INICIO MAESTRO

// INICIO GESTION GRID

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        //App.storeDetalle.reload();

        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnActivar.enable();
        App.btnDefecto.enable();
        //App.btnGlobales.enable();

        //App.btnAnadirDetalle.enable();
        //App.btnRefrescarDetalle.enable();
        //App.btnDescargarDetalle.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDefecto.setTooltip(jsDefecto);
        //App.btnGlobales.setTooltip(jsGlobales);

        //App.btnRefrescarDetalle.setTooltip(jsRefrescar);
        //App.btnDescargarDetalle.setTooltip(jsDescargar);

        if (seleccionado.Active) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }

        App.ModuloID.setValue(seleccionado.MonedaID);
        //TreeCore.mostrarDetalle(seleccionado.MonedaID);
    }
}

function DeseleccionarGrilla() {

    //App.storeDetalle.reload();
    App.GridRowSelect.clearSelections();
    App.ModuloID.setValue("");

    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnActivar.disable();
    App.btnDefecto.disable();


    //App.btnAnadirDetalle.disable();
    //App.btnRefrescarDetalle.disable();
    //App.btnDescargarDetalle.disable();

}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

// FIN GESTION GRID

function RecargarPrincipal() {
    App.storePrincipal.reload();
}

function VaciarFormulario() {
    Ext.each(App.formGestion.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.reset();

            if (c.triggerWrap != undefined) {
                c.triggerWrap.removeCls("itemForm-novalid");
                c.triggerWrap.addCls("itemForm-valid");
            }

            if (!c.allowBlank && c.xtype != "checkboxfield") {
                c.addListener("change", anadirClsNoValido, false);
                c.addListener("focusleave", anadirClsNoValido, false);
                c.addListener('validitychange', FormularioValido);
                c.addListener('change', FormularioValido);

                c.addListener("change", cambiarLiteral, false);

                c.addCls("ico-exclamacion-10px-red");
                c.removeCls("ico-exclamacion-10px-grey");
            }
            else if (c.allowBlank) {
                c.addListener("focus", cambiarBoton, true);
            }
        }
    });

    App.hdEditando.setValue('');
}

function cambiarLiteral(sender, registro, index) {
    if (App.btnGuardar != undefined) {
        App.btnGuardar.setText(jsGuardar);
        App.btnGuardar.setIconCls("");
        App.btnGuardar.removeCls("btnDisableClick");
        App.btnGuardar.addCls("btnEnableClick");
        App.btnGuardar.removeCls("animation-text");
    }
    else {
        App.btnGuardar.setText(jsGuardado);
        App.btnGuardar.addCls("btnDisableClick");
        App.btnGuardar.removeCls("btnEnableClick");
        App.btnGuardar.setIconCls("ico-tic-wh");
    }
}

function cambiarBoton(sender, registro, index) {
    App.btnGuardar.setText(jsGuardar);
    App.btnGuardar.setIconCls("");
    App.btnGuardar.removeCls("btnDisableClick");
    App.btnGuardar.addCls("btnEnableClick");
    App.btnGuardar.removeCls("animation-text");
}

function FormularioValido(sender, registro, index) {
    Editando = App.hdEditando.getValue();

    App.btnGuardar.setDisabled(false);

    if (Editando == "Editar") {
        Ext.each(App.formGestion.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && !c.hidden && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnGuardar.setDisabled(true);
                App.btnGuardar.setText(jsGuardar);
                App.btnGuardar.setIconCls("");
                App.btnGuardar.removeCls("btnDisableClick");
                App.btnGuardar.addCls("btnEnableClick");
                App.btnGuardar.removeCls("animation-text");
            }
        });
    }
    else {
        App.btnGuardar.setText(jsGuardar);
        App.btnGuardar.setIconCls("");
        App.btnGuardar.removeCls("btnDisableClick");
        App.btnGuardar.addCls("btnEnableClick");
        App.btnGuardar.removeCls("animation-text");

        Ext.each(App.formGestion.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && !c.hidden && c.isFormField &&
                (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") &&
                    (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnGuardar.setDisabled(true);
            }
        });
    }
}

function AgregarEditar() {
    showLoadMask(App.gridMaestro, function (load) {
        VaciarFormulario();
        App.txtMoneda.focus(false, 500);
        App.winGestion.setTitle(jsAgregar);
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

    App.winGestion.setTitle(jsEditar);

    showLoadMask(App.gridMaestro, function (load) {
        TreeCore.MostrarEditar(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }

                    App.txtMoneda.focus(false, 500);
                    App.storePrincipal.reload();
                    load.hide();

                }
            });
    });
}

function Activar() {
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
        if (seleccionado.Default) {
            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsDesactivarPorDefecto, buttons: Ext.Msg.OK });
        }
        else {
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

function Defecto() {
    if (registroSeleccionado(App.gridMaestro) && seleccionado != null) {
        if (!seleccionado.Active) {
            Ext.Msg.alert(
                {
                    title: jsDefecto,
                    msg: jsRegistroInactivoPorDefecto,
                    buttons: Ext.Msg.YESNO,
                    fn: ajaxDefecto,
                    icon: Ext.MessageBox.QUESTION
                });
        }
        else {
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
    DeseleccionarGrilla();
}

// FIN MAESTRO

// INICIO DETALLE

// INICIO GESTION GRID

//function Grid_RowSelect_Detalle(sender, registro, index) {

//    var datos = registro.data;

//    if (datos != null) {

//        seleccionado = datos;

//        seleccionadoDetalle = datos;
//        App.btnEliminarDetalle.enable();
//        App.btnEditarDetalle.enable();

//        App.btnEditarDetalle.setTooltip(jsEditar);
//        App.btnEliminarDetalle.setTooltip(jsEliminar);
//        App.btnAnadirDetalle.setTooltip(jsAgregar);

//    }
//}

//function DeseleccionarGrillaDetalle() {
//    App.GridRowSelectDetalle.clearSelections();
//    App.btnEditarDetalle.disable();
//    App.btnEliminarDetalle.disable();

//    App.btnAnadirDetalle.setTooltip(jsAgregar);
//    App.btnEditarDetalle.setTooltip(jsEditar);
//    App.btnEliminarDetalle.setTooltip(jsEliminar);

//}

//var handlePageSizeSelectDetalle = function (item, records) {
//    var curPageSize = App.storeDetalle.pageSize,
//        wantedPageSize = parseInt(item.getValue(), 10);

//    if (wantedPageSize != curPageSize) {
//        App.storeDetalle.pageSize = wantedPageSize;
//        App.storeDetalle.load();
//    }
//}


// FIN GESTION GRID

// INICIO GESTION DETALLE

//function VaciarFormularioDetalle() {
//    App.formGestionDetalle.getForm().reset();
//}

//function FormularioValidoDetalle(valid) {
//    if (valid) {
//        App.btnGuardarDetalle.setDisabled(false);
//    }
//    else {
//        App.btnGuardarDetalle.setDisabled(true);
//    }
//}

//function AgregarEditarDetalle() {
//    VaciarFormularioDetalle();
//    App.txtDolarDetalle.focus(false, 500);
//    App.winGestionDetalle.setTitle(jsAgregar);
//    App.winGestionDetalle.show();
//}

//function winGestionBotonGuardarDetalle() {
//    if (App.formGestionDetalle.getForm().isValid()) {
//        ajaxAgregarEditarDetalle();
//    }
//    else {

//        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
//    }

//}

//function ajaxAgregarEditarDetalle() {
//    var Agregar = false;

//    if (App.winGestionDetalle.title.startsWith(jsAgregar)) {
//        Agregar = true;
//    }
//    TreeCore.AgregarEditarDetalle(Agregar,
//        {
//            success: function (result) {
//                if (result.Result != null && result.Result != '') {
//                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
//                }
//                else {
//                    App.winGestionDetalle.hide();
//                    App.storeDetalle.reload();
//                }
//            },
//            eventMask:
//            {
//                showMask: true,
//                msg: jsMensajeProcesando
//            }
//        });
//}

//function MostrarEditarDetalle() {
//    if (registroSeleccionado(App.GridDetalle) && seleccionado != null) {
//        ajaxEditarDetalle();
//    }
//}

//function ajaxEditarDetalle() {
//    VaciarFormularioDetalle();
//    Agregar = false;
//    App.winGestionDetalle.setTitle(jsEditar);

//    TreeCore.MostrarEditarDetalle(
//        {
//            success: function (result) {
//                if (result.Result != null && result.Result != '') {
//                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
//                }
//                App.txtDolarDetalle.focus(false, 200);
//                App.storeDetalle.reload();
//            },
//            eventMask:
//            {
//                showMask: true,
//                msg: jsMensajeProcesando
//            }
//        });
//}

//function EliminarDetalle() {
//    if (registroSeleccionado(App.GridDetalle) && seleccionado != null) {
//        Ext.Msg.alert(
//            {
//                title: jsEliminar,
//                msg: jsMensajeEliminar,
//                buttons: Ext.Msg.YESNO,
//                fn: ajaxEliminarDetalle,
//                icon: Ext.MessageBox.QUESTION
//            });
//    }
//}

//function ajaxEliminarDetalle(button) {
//    if (button == 'yes' || button == 'si') {
//        TreeCore.EliminarDetalle({
//            success: function (result) {
//                if (result.Result != null && result.Result != '') {
//                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
//                }
//                else {
//                    App.storeDetalle.reload();
//                }
//            },
//            eventMask: { showMask: true, msg: jsMensajeProcesando }
//        });
//    }
//}

//function RefrescarDetalle() {
//    App.storeDetalle.reload();
//    DeseleccionarGrillaDetalle();
//}

//function abrirMonedas() {
//    parent.addTab(parent.App.tabPpal, parent.jsMonedasGlobales, parent.jsMonedasGlobales, "../../Modulos/Settings/MonedasGlobales.aspx");
//}

// FIN DETALLE