
function RecargarPlantilla() {
    recargarCombos([App.cmbPlantillas]);
    App.cmbAuxiliar.allowBlank = true;
    App.cmbAuxiliar.reset();
    App.cmbAuxiliar.disable();
}

function SeleccionarPlantilla() {
    App.cmbPlantillas.getTrigger(0).show();
    if (App.cmbPlantillas.selection.data.RutaDocumento == "ExportarModeloInventario" || App.cmbPlantillas.selection.data.RutaDocumento == "ExportarModeloSubcategorias") {
        App.cmbAuxiliar.getStore().reload();
        App.cmbAuxiliar.setFieldLabel(jsCategoria);
        App.cmbAuxiliar.allowBlank = false;
        App.cmbAuxiliar.enable();
        App.cmbAuxiliar.reset();
        FormularioValidoDownload(false);
    } else {
        App.cmbAuxiliar.allowBlank = true;
        App.cmbAuxiliar.disable();
        FormularioValidoDownload(true);
    }
}

function SeleccionarComboAuxiliar(sender) {
    if (App.cmbPlantillas.selection != null) {
        return ((App.cmbPlantillas.selection.data.RutaDocumento == "ExportarModeloInventario" || App.cmbPlantillas.selection.data.RutaDocumento == "ExportarModeloSubcategorias") && App.cmbAuxiliar.getSelectedValues().length < App.cmbAuxiliar.maxSelection);
    }
}

function FormularioValidoDownload(valid) {
    if (valid) {
        App.btnDescargar.setDisabled(false);
    }
    else {
        App.btnDescargar.setDisabled(true);
    }
}

function DescargarPlantilla() {
    var plantilla = App.cmbPlantillas.value;

    TreeCore.ExistePlantilla(plantilla,
        {
            isUpload: true,
            error: function (a) {
                console.log(a)
            },
            success: function (result) {
                if (result.Success != null && result.Success == false) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
        });
    App.WinConfirmExport.hide();
}

function winGestionBotonSubirPlantilla() {
    TreeCore.AgregarPlantilla(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winUploadTemplate.hide();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function abrirGridTemplate(sender, registro, index) {
    parent.addTab(parent.App.tabPpal, jsCargas, jsCargas, "/Modulos/ImportExport/DataUploadGrid.aspx");
}
