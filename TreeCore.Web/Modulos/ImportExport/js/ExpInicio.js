let path = "";

//#region RESIZER JS PARA PANELES Y GRID
setTimeout(function () {

    //REFRESH FORZADO PARA QUE SE ADAPTEN LAS COLUMNAS

    App.pnNotificationsFull.update();

}, 100);

function RpanelBTNAlert() {

    App.pnNotificationsFull.show();
    App.pnNotesFull.hide();

}

function RpanelBTNNotes() {

    App.pnNotesFull.show();
    App.pnNotificationsFull.hide();

}

//#endregion END RIGHT PANEL TAB BUTTONS

//#region VENTANA FORM RESPONSIVE
function winFormResize() {
    var res = window.innerWidth;

    if (res > 1024) {
        App.winExportImportProgress.setWidth(872);
        App.WinConfirmExport.setWidth(672);
        App.WinConfirmImport.setWidth(672);
    }

    if (res <= 1024 && res > 670) {
        App.winExportImportProgress.setWidth(650);
        App.WinConfirmExport.setWidth(600);
        App.WinConfirmImport.setWidth(600);

    }

    if (res <= 670) {
        App.winExportImportProgress.setWidth(380);
        App.WinConfirmExport.setWidth(380);
        App.WinConfirmImport.setWidth(380);

    }

    App.WinConfirmExport.center();
    App.winExportImportProgress.center();
    App.WinConfirmImport.center();
}




function winFormCenterSimple(obj) {


    obj.center();
    obj.update();

}

// #endregion

function ExpOpenExport() {

    App.winModuloExport.show();

}

function OpenImport() {

    App.WinConfirmImport.show();

}

function RecargarProyectoTipo() {
    recargarCombos([App.MCModulos]);
    TextArea.Text = "";
}

function RecargarProyectoTipoImp() {
    recargarCombos([App.cmbTipe]);
    App.UploadF.reset();
    
}

function SeleccionarProyectoTipo() {
    App.MCModulos.getTrigger(0).show();
}

function SeleccionarProyectoTipoImp() {
    App.cmbTipe.getTrigger(0).show();
}

function FormularioValidoImport(valid) {
    if (valid) {
        App.btnImportarTemp.setDisabled(false);
    }
    else {
        App.btnImportarTemp.setDisabled(true);
    }
}

function FormularioValidoDownload(valid) {
    if (valid) {
        App.btnExportarTemp.setDisabled(false);
    }
    else {
        App.btnExportarTemp.setDisabled(true);
    }
}

function DescargarPlantilla() {

    TreeCore.ExistePlantilla(path,
        {
            success: function (result) {
                if (result.Result == null && result.Result == '') {
                    Ext.Msg.alert({ title: parent.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    if (result) {

                        window.open("ExpInicio.aspx?Ruta=" + path + "&DescargarPlantilla=");
                    }

                    App.winExportImportProgress.hide();
                    RecargarProyectoTipo();
                    App.winModuloExport.hide();
                }
            },
            eventMask: {
                showMask: true,
                msg: parent.jsMensajeProcesando
            }
        });
}

function ExportarLogMigrador() {
    ajaxExportarLogMigrador();
}


function ajaxExportarLogMigrador() {
    TreeCore.ExportarLogMigrador(path,
        {
            success: function (result) {
                if (result.Result == null && result.Result == '') {
                    Ext.Msg.alert({ title: parent.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    if (result) {

                        window.open("ExpInicio.aspx?Ruta=" + path + "&DescargarPlantilla=");
                    }

                    
                    RecargarProyectoTipo();
                    
                }
            },
            eventMask: {
                showMask: true,
                msg: parent.jsMensajeProcesando
            }
        });
}

function btnExportarConfiguracion() {
    ajaxExportarConfiguracion();
}

function ajaxExportarConfiguracion() {
    var valor = App.MCModulos.value;
    App.backNewUsers.setText(parent.jsCerrar);
    App.btnDescargarPlantilla.show();
    App.Button10.hide();

    TreeCore.ExportarConfiguraciones(valor,
        {
            success: function (result) {
                if (result.Result == null && result.Result == '') {
                    Ext.Msg.show({ title: parent.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

                else {
                    if (result) {
                        path = result.Result;
                    }
                }
                App.WinConfirmExport.hide();
                App.winExportImportProgress.show();
                RecargarProyectoTipo();

            },
            eventMask:
            {
                showMask: true,
                
            }
        });
}

function btnImportarConfiguracion() {
    ajaxImportacionConfiguracion();
}

function ajaxImportacionConfiguracion() {

    App.backNewUsers.setText(parent.jsCerrar);
    App.btnDescargarPlantilla.show();
    App.Button10.show();

    TreeCore.ImportarConfiguraciones(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: parent.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                
                App.WinConfirmImport.hide();
                App.winExportImportProgress.show();
                App.backNewUsers.setText(parent.jsCerrar);
                App.btnDescargarPlantilla.hide();
                RecargarProyectoTipoImp();
            },
            eventMask:
            {
                showMask: true,

            }
        });
}