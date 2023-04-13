function SeleccionarPlantillaSubCat(sender) {
    sender.getTrigger(0).show();
    var ruta = getIdComponente(sender);
    try {
        showLoadMask(sender.up().up().up().up().up(), function (load) {
            TreeCore[ruta].SeleccionarPlantilla(
                {
                    success: function (result) {
                        if (!result.Success) {
                            Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        } else {
                            for (var prop in result.Result) {
                                SeleccionarValorMostrarEditar(prop, result.Result[prop]);
                            }
                            load.hide();
                        }
                    }
                });
        });
    } catch (e) {
        TreeCore[ruta].SeleccionarPlantilla(
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        for (var prop in result.Result) {
                            SeleccionarValorMostrarEditar(prop, result.Result[prop]);
                        }
                    }
                }
            });
    }
}


function LimpiarCategoria(sender) {
    sender.getTrigger(0).hide();
    sender.reset();
    var ruta = getIdComponente(sender);
    Ext.each(App[ruta + '_containerAttributes'].body.query('*'), function (item) {
        var c = X.getCmp(item.id);
        if (c && c.isFormField) {
            c.reset();
        }
    });
    FormularioValido(sender, true);
}

function SetTriggerCmbPlantilla(sender) {
    if (sender.rawValue != "") {
        sender.getTrigger(0).show();
    }
}

function CargarStorePlantilla(sender) {
    var ruta = getIdComponente(sender);
    if (App[ruta + '_' +'hdEsPlantilla'].value == 1) {
        storesACargar.push(App[ruta + '_' + 'cmbPlantilla']);
    }
}