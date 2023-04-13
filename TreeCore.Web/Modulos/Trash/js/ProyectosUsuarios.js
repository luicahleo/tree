var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storeUsuarios.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeUsuarios.pageSize = wantedPageSize;
        App.storeUsuarios.load();
    }

}

function DeseleccionarGrilla() {
    App.GridRowSelect.clearSelections();
    App.btnEliminar.setDisabled(true);
}


function Refrescar() {
    App.storeUsuarios.reload();
    

}
function SeleccionarProyecto() {
    App.cmbProyectos.getTrigger(0).show();
    App.storeUsuarios.reload();
}

function Grid_RowSelect(sender, registro, index) {
    let registroSeleccionado = registro;
    let GridSeleccionado = App.gridPrincipal;
    parent.cargarDatosPanelMoreInfoGrid(registroSeleccionado, GridSeleccionado);

    var datos = registro.data;
    if (datos != null) {
        App.hdUsuarioID.setValue(datos.UsuarioID);
    }

    App.btnEliminar.setDisabled(false);
}

function VerMas(sender, registro, index) {
    parent.hidePanelMoreInfo(sender, registro);
}

var cargarProyectoID = function () {
    return parent.App.hdProyectoID.value;
}

// #region USUARIOS PROYECTOS

function RecargarComboUsuarios() {
    recargarCombos([App.cmbUsuarios]);
}


function GestionarUsuarios() {
    RecargarComboUsuarios()
    App.winGestionUsuarios.show();
}


function cambiarAsignacion(sender, registro, index) {
    let usuarioID = App.hdUsuarioID.value;

    TreeCore.AsignarProyecto(usuarioID, 0,
        "eliminar",
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeUsuarios.reload();
                    RecargarComboUsuarios()
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

function winAgregarUsuarios(sender, registro, index) {
    var item = App.cmbUsuarios.selection;
    //App.hdClienteID.value = item.data.ClienteID;
    if (item.data.Tipo == "Entidades") {
        TreeCore.AsignarUsuariosEntidades(item.data.UsuariosProyectosLibresID, item.data.ClienteID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        RecargarComboUsuarios()
                        App.storeUsuarios.reload();
                    }
                },
                eventMask:
                {
                    showMask: true,
                }
            });
    } else if (item.data.Tipo == "Usuarios") {
        TreeCore.AsignarProyecto(item.data.UsuariosProyectosLibresID, item.data.ClienteID, "agregar",
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        RecargarComboUsuarios();
                        App.storeUsuarios.reload();
                    }
                },
                eventMask:
                {
                    showMask: true,
                }
            });

    } else {
        TreeCore.AsignarUsuariosDepartamentos(item.data.UsuariosProyectosLibresID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        RecargarComboUsuarios()
                        App.storeUsuarios.reload();
                    }
                },
                eventMask:
                {
                    showMask: true,
                }
            });
    }
}

// #endregion