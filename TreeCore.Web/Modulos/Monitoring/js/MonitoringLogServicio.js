var Agregar = false;
var seleccionado;


//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
    }
}

function DeseleccionarGrilla() {

    App.btnError.enable();
    App.btnError.setTooltip(jsError);

    App.btnInfo.enable();
    App.btnInfo.setTooltip(jsInfo);

    App.btnRefrescar.setTooltip(jsRefrescar);
    App.btnDescargar.setTooltip(jsDescargar);
    App.hdCarpeta.setValue("");
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

function SeleccionarComando(command, value, carpetaValue, tipo) {

    if (command == "DescargarLog") {
        TreeCore.TieneContenido(value, carpetaValue, tipo,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        if (result.Result == true) {

                            window.open("MonitoringLogServicio.aspx?DescargarLog=" + value + "&hdCarpeta=" + carpetaValue);

                        }
                        else {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsLogVacio, buttons: Ext.Msg.OK });
                        }
                    }

                    else {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsLogVacio, buttons: Ext.Msg.OK });
                    }
                }
            });

    }
    else if (command == "VerLog") {
        TreeCore.MostrarLog(value, carpetaValue, tipo,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        if (tipo != null && tipo != "") {

                            if (tipo == "Error") {
                                sIcon = Ext.MessageBox.ERROR;
                            }
                            else if (tipo == "Warning") {
                                sIcon = Ext.MessageBox.WARNING;
                            }
                            else if (tipo == "Debug") {
                                sIcon = Ext.MessageBox.DEBUG;
                            }
                            else if (tipo == "Fatal") {
                                sIcon = Ext.MessageBox.FATAL;
                            }
                            else if (tipo == "Information" || tipo == "Información") {
                                sIcon = Ext.MessageBox.INFO;
                            }

                            var msg = '';
                            sTipo = tipo + ' - ' + value;
                            texto = result.Result.split('\n');

                            texto.forEach(function (element) {
                                if (msg != '') {
                                    msg += '<br/><br>';
                                }
                                msg += element;
                            });

                            msg = Ext.Msg.show({ title: sTipo, icon: sIcon, msg: msg, minWidth: 900 });
                        }
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

function Logs(tab) {
    forzarCargaBuscadorPredictivo = true;
    App.storePrincipal.reload();
}
