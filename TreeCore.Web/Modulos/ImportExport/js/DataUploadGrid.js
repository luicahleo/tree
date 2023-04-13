// #region DIRECT METHOD

var seleccionado;

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

function Grid_RowSelect(sender, registro, index) {

    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        let registroSeleccionado = registro;

        App.btnEliminar.enable();
        //if (seleccionado.Processed == true) {
            //App.btnEditar.disable();
        //}
        //else {
            //App.btnEditar.enable();
            //App.txtNombre.setValue(seleccionado.Code);
        //}

        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);

        if (seleccionado.Document.Document != null && seleccionado.Document.Document != "") {
            App.ShowTemplate.enable();
            App.btnDescargarTemplate.enable();
            App.btnDescargarTemplate.setTooltip(jsDescargarPlantilla);
        }
        else {
            App.ShowTemplate.disable();
            App.btnDescargarTemplate.disable();
        }

        if (seleccionado.LogFile != null && seleccionado.LogFile != "") {
            App.ShowLog.enable();
            App.btnDescargarLog.enable();
            App.btnDescargarLog.setTooltip(jsDescargarLog);
        }
        else {
            App.ShowLog.disable();
            App.btnDescargarLog.disable();
        }
    }
}

function DeseleccionarGrilla() {
    App.GridRowSelect.deselectAll();
    App.GridRowSelect.clearSelections();
    App.btnEliminar.disable();
    App.ShowLog.disable();
    App.ShowTemplate.disable();
    App.btnDescargarLog.disable();
    App.btnDescargarTemplate.disable();
    App.btnEditar.setHidden(true);
    //App.btnEditar.disable();
}

function AgregarEditar() {
    VaciarFormulario();
    App.txtNombre.setDisabled(false);
    App.UploadF.show();
    App.cmbPlantillas.show();
    App.WinConfirmExport.setTitle(parent.jsAgregar);
    App.tmHoraCarga.setDisabled(true);
    Agregar = true;

    Ext.each(App.WinConfirmExport.body.query('*'), function (value) {
        Ext.each(value, function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && c.isFormField) {
                c.reset();

                if (c.triggerWrap != undefined) {
                    c.triggerWrap.removeCls("itemForm-novalid");
                }

                if (!c.allowBlank && c.xtype != "checkboxfield" && c.xtype != "hidden") {
                    c.addListener("change", anadirClsNoValido, false);
                    c.addListener("focusleave", anadirClsNoValido, false);

                    c.removeCls("ico-exclamacion-10px-red");
                    c.addCls("ico-exclamacion-10px-grey");
                }

                if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                }
            }
        });
    });

    ValidarFormulario();
    App.WinConfirmExport.show();
}

function winGestionBotonGuardar() {
    ajaxAgregar();
}

function ajaxAgregar() {

    TreeCore.Agregar(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.WinConfirmImport.hide();
                    App.WinConfirmExport.hide();
                    Refrescar();
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
    if (seleccionado != null) {
        Ext.Msg.alert(
            {
                title: parent.jsEliminar,
                msg: parent.jsMensajeEliminar,
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
                    Refrescar();
                }
            },
            eventMask: {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    }
}

function DescargarPlantilla() {

    TreeCore.ExistePlantilla(seleccionado.Document.Document,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    if (result) {
                        window.open("DataUploadGrid.aspx?DescargarPlantilla=" + seleccionado.Document.Document);
                    }
                }
            }
        });
}

function DescargarLog() {
    TreeCore.ExisteLog(seleccionado.LogFile,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    if (result) {
                        window.open("DataUploadGrid.aspx?DescargarLog=" + seleccionado.LogFile);
                    }
                }
            },
            eventMask: {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function limpiar(value) {
    value.setValue("");
}

function Refrescar() {
    DeseleccionarGrilla();
    App.storePrincipal.reload();
}

function buscador() {
    App.storePrincipal.reload();
}

function RecargarProyecto() {
    recargarCombos([App.cmbProyectos]);
    App.storePrincipal.reload();
}

function SeleccionarProyecto() {
    App.cmbProyectos.getTrigger(0).show();
    App.storePrincipal.reload();
}

function FormularioValidoUpload(valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function VaciarFormulario() {


    Ext.each(App.formUpload.body.query('*'), function (item) {
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



                c.addListener("change", cambiarLiteral, false);
                c.addListener("validityChange", ValidarFormulario, false);



                c.addCls("ico-exclamacion-10px-red");
                c.removeCls("ico-exclamacion-10px-grey");
            }



            if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
            }
        }
    });

    //App.formUpload.getForm().reset();
}

function cambiarLiteral() {
    App.btnGuardar.setText(jsGuardar);
    App.btnGuardar.setIconCls("");
    App.btnGuardar.removeCls("btnDisableClick");
    App.btnGuardar.addCls("btnEnableClick");
    App.btnGuardar.removeCls("animation-text");
}

function RecargarProyectoUpload() {
    recargarCombos([App.cmbProyectosUpload]);
}

function SeleccionarProyectoUpload() {
    App.cmbProyectosUpload.getTrigger(0).show();
    App.storeProyectos.reload();
}

function RecargarOperadores() {
    recargarCombos([App.cmbOperadores]);
}

function SeleccionarOperadores() {
    App.cmbOperadores.getTrigger(0).show();
    App.storeOperadores.reload();
}

function RecargarPlantillas() {
    recargarCombos([App.cmbPlantillas]);
}

function SeleccionarPlantillas() {
    App.cmbPlantillas.getTrigger(0).show();

    App.storePrincipalPlantillas.reload();
    if (App.cmbPlantillas.value == 'SITES' || App.cmbPlantillas.value == 'INVENTORY') {
        App.tmHoraCarga.setValue('0:00');
        App.tmHoraCarga.setDisabled(true);
    }
    else {
        
        App.tmHoraCarga.setDisabled(false);
    }
}

var Uploadrender = function (value) {
    if (value == true || value == 1) {
        return '<span class="uploadgrid">&nbsp;</span>'

    }
    else {
        return '<span>&nbsp;</span> '
    }
}

var DefectoRender = function (value) {
    if (value == true || value == 1) {
        return '<span class="ico-defaultGrid">&nbsp;</span>'
    }
    else {
        return '<span>&nbsp;</span> '
    }
}

// #endregion

// #region Frecuencias
function showFormsFormImportar(sender, registro, inde) {
    var classActivo = "navActivo";
    var index = 0;

    var arrayBotones = sender.ariaEl.getParent().dom.children;
    for (let i = 0; i < arrayBotones.length; i++) {
        let cmp = Ext.getCmp(arrayBotones[i].id);
        if (cmp.id == sender.id) {
            index = i;
        }
    }
}

function ValidarFormulario(sender, valido, aux) {

    try {

        var formPanel = App.formUpload;
        App.btnGuardar.setDisabled(false);

        App.btnGuardar.setText(jsGuardar);
        App.btnGuardar.setIconCls("");
        App.btnGuardar.removeCls("btnDisableClick");
        App.btnGuardar.addCls("btnEnableClick");
        App.btnGuardar.removeCls("animation-text");

        if (Agregar == true) {

            Ext.each(formPanel.body.query('*'), function (item) {
                var c = Ext.getCmp(item.id);
                if (c != undefined && !c.hidden && c.isFormField &&
                    (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") &&
                        (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                    App.btnGuardar.setDisabled(true);
                }
            });


            var classMandatory = "ico-formview-mandatory";
            var i = 0;

            // #region Validación Tabs
            var i = 0;
            Ext.each(App.formImport.query('*'), function (value) {
                var c = Ext.getCmp(value.id);

                if (c != undefined && c.isFormField && !c.allowBlank && !c.hidden) {
                    if (!c.isValid()) {
                        i++;
                    }
                }
            });

            // #endregion
        }

        else if (Agregar == false) {

            Ext.each(formPanel.body.query('*'), function (item) {
                var c = Ext.getCmp(item.id);
                if (c != undefined && !c.hidden && c.isFormField &&
                    (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo" || c.xtype == "combo") &&
                        (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                    App.btnGuardar.setDisabled(true);
                }
            });

            // #endregion
        }


    } catch (e) {

    }
}

//#endregion

function MostrarEditar() {

    ajaxEditar();

}

function ajaxEditar() {

    //VaciarFormulario();
    App.WinConfirmExport.setTitle(parent.jsEditar);
    Agregar = false;
    App.txtNombre.setDisabled(true);
    App.UploadF.hide();
    App.cmbPlantillas.hide();

    //ValidarFormulario();
    App.WinConfirmExport.show();
    App.WinConfirmExport.hide();

    TreeCore.MostrarEditar(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                forzarCargaBuscadorPredictivo = true;

            }
        });

}