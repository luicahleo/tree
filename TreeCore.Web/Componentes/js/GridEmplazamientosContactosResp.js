let mapForm = [];

// #region DIRECT METHOD CONTACTOS

function DeseleccionarGrillaContactos(sender, registro, index) {
    var ruta = sender.config.storeId.split('_');
    ruta.pop();
    ruta = ruta.join('_');

    App[ruta + '_' + 'GridRowSelectContacto'].clearSelections();
    App[ruta + "_btnEditar"].disable();
    App[ruta + "_btnEliminar"].disable();
    App[ruta + "_btnActivar"].disable();

    App[ruta + "_btnEditar"].setTooltip(jsEditar);
    App[ruta + "_btnEliminar"].setTooltip(jsEliminar);
    App[ruta + "_btnDescargar"].setTooltip(jsDescargar);
}

function Grid_RowSelectContactos(sender, registro, index) {
    var ruta = sender.config.proxyId.split('_');
    ruta.pop();
    ruta = ruta.join('_');

    if (registro.data != null) {
        App[ruta + "_btnEditar"].enable();
        App[ruta + "_btnActivar"].enable();

        if (registro.data.Activo) {
            App[ruta + '_' + 'btnActivar'].setTooltip(jsDesactivar);
        }
        else {
            App[ruta + '_' + 'btnActivar'].setTooltip(jsActivar);
        }   
    }

    App[ruta + "_btnEliminar"].enable();
    App[ruta + "_btnEditar"].setTooltip(jsEditar);
    App[ruta + "_btnEliminar"].setTooltip(jsEliminar); 
}

function agregarContacto(sender, registro, index) {

    var idComponente = sender.id.split('_')[0];

    App[idComponente + "_winGestionContacto"].setTitle(jsAgregar);
    App[idComponente + "_winGestionContacto"].show();

    VaciarFormularioContacto(idComponente + '_formAgregarEditarContacto');
    cambiarATapContacto(idComponente + '_formAgregarEditarContacto', 0);
    CargarVentanaContactos(sender, 'formAgregarEditarContacto', true, function Fin(fin) { });

}

function editarContacto(sender, registro, index) {
    var idComponente = getIdComponente(sender);
    var ruta = undefined;
    var valido = true;

    App[idComponente + "_winGestionContacto"].setTitle(jsEditar);
    App[idComponente + "_winGestionContacto"].show();

    showLoadMask(App[idComponente + "_winGestionContacto"], function (load) {

        VaciarFormularioContacto(idComponente + '_formAgregarEditarContacto');
        DefinirDatosContacto(idComponente, App[idComponente + '_GridRowSelectContacto'].selected.items[0].data);
        cambiarATapContacto(idComponente + '_formAgregarEditarContacto', 0);

        TreeCore[idComponente].MostrarEditarContacto(App[idComponente + '_GridRowSelectContacto'].selected.items[0].data.ContactoGlobalID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        CargarVentanaContactos(sender, 'formAgregarEditarContacto', true, function Fin(fin) {
                            
                            load.hide();
                        });                       
                    }
                },
            });
        load.hide();
    });

}

function activarContacto(sender, registro, index) {
    var idComponente = getIdComponente(sender);
    TreeCore[idComponente].Activar(App[idComponente + '_GridRowSelectContacto'].selected.items[0].data.ContactoGlobalID,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App[idComponente + "_storeContactosGlobalesEmplazamientos"].reload();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

function eliminarContacto(sender, registro, index, button) {
    var idComponente = getIdComponente(sender);
    if (App[idComponente + '_GridRowSelectContacto'].selected != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION,
                ID: idComponente
            });
    }
}

function ajaxEliminar(button, a, idComponente) {
    if (button == 'yes' || button == 'si') {
        TreeCore[idComponente.ID].Eliminar(App[idComponente.ID + '_GridRowSelectContacto'].selected.items[0].data.ContactoGlobalID,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        forzarCargaBuscadorPredictivo = true;
                        App[idComponente.ID + "_storeContactosGlobalesEmplazamientos"].reload();
                    }
                },
                eventMask:
                {
                    showMask: true,
                }
            });
    }
}

function refrescarContacto(sender, registro, index) {
    var ruta = getIdComponente(sender);

    if (ruta == "gridContactos") {
        App.gridContactos_grdContactos.store.reload();
    } else {
        forzarCargaBuscadorPredictivo = true;
        ajaxAplicarFiltro(filtrosAplicados);
    }

    App[ruta + '_' + 'GridRowSelectContacto'].clearSelections();
    App[ruta + "_btnEditar"].disable();
    App[ruta + "_btnEliminar"].disable();
    App[ruta + "_btnActivar"].disable();

    App[ruta + "_btnEditar"].setTooltip(jsEditar);
    App[ruta + "_btnEliminar"].setTooltip(jsEliminar);
}

function mostrarFiltros(sender, registro, index) {
    var idComponente = sender.id.split('_')[0];
    var ControlURL = '/Componentes/toolbarFiltros.ascx';
    var NombreControl = 'toolbarFiltros';
    var achjs = ControlURL.split('/');
    achjs.pop(); achjs = achjs.join('/') + '/js/' + NombreControl + '.js';
    AñadirScriptjs(achjs);
    App[idComponente + "_hdControlURL"].value = ControlURL;
    App[idComponente + "_hdControlName"].value = NombreControl;
    TreeCore[idComponente].LoadUserControl(ControlURL, NombreControl, true,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    setClienteIDComponentes();
                    App[idComponente + "_hugeCt"].show();
                }
            },
            eventMask:
            {
                showMask: true,
            }
        });
}

function descargarContacto(sender, registro, index) {
}

function cargarFiltros(sender, registro, index) {
}

function recargarStore(sender, registro, index) {
    var ruta = sender.id.split('_')[0];
    if (ruta == "gridContactos") {
        forzarCargaBuscadorPredictivo = true;
        sender.store.reload();
    }
}

var linkRendererTrack = function (value, metadata, record) {
    return Ext.String.format("<a href=\"javascript:addUserTabTrack('{0}','{1}');\">{1}<a/>", record.data.id, record.data.Code);
}

function addUserTabTrack(id, title) {
    var tabPanel = window.parent.App.tabPpal; //get from iframe  tabpanel located in parent window 
    var tab = tabPanel.getComponent('user_' + id);
    var url1 = '/PaginasComunes/Seguimiento.aspx';
    if (!tab) {
        tab = tabPanel.add({
            //id: 'user_' + id,
            title: title,
            iconCls: '#TextSignature',
            closable: true,
            //menuItem : menuItem,
            loader: {
                url: url1,
                renderer: "frame",
                loadMask: {
                    showMask: true,
                    msg: "Cargando Seguimiento..  "
                    //msg: jsCargandoMapa
                }
            }
        });
    }
    tabPanel.setActiveTab(tab);
}

var handlePageSizeSelect = function (item, records) {
    var idComponente = item.id.split('_')[0];
    var curPageSize = App[idComponente + "_storeContactosGlobalesEmplazamientos"].pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App[idComponente + "_storeContactosGlobalesEmplazamientos"].pageSize = wantedPageSize;
        App[idComponente + "_storeContactosGlobalesEmplazamientos"].load();
    }
}

// #endregion