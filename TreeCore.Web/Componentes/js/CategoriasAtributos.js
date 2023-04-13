var Atributos = new Object();

/* Orden Categorias */
function OrdenMasCategoria(sender, registro, index) {
    showLoadMaskCategorias(function (load) {
        TreeCore[sender.config.id.split('_')[0]].ModificarOrden(true,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    TreeCore.PintarCategorias(true, true, {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            load.hide();
                            DragDropCategorias();
                            DragDropAtributosCategorias();
                        }
                    });
                }
            });
    });
}

function OrdenMenosCategoria(sender, registro, index) {
    showLoadMaskCategorias(function (load) {
        TreeCore[sender.config.id.split('_')[0]].ModificarOrden(false,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    TreeCore.PintarCategorias(true, true, {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            load.hide();
                            DragDropCategorias();
                            DragDropAtributosCategorias();
                        }
                    });
                }
            });
    });
}

/* Fin Orden Categorias */

/* Atributos */

function SelectItemNuevoAtributo(sender, registro, index) {
    var ruta = getIdComponente(sender);
    IdTipoDato = registro.id.split('_').pop();
    showLoadMaskCategorias(function (load) {
        TreeCore[ruta].AñadirNuevoAtributo(IdTipoDato,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    load.hide();
                    DragDropCategorias();
                    DragDropAtributosCategorias();
                }
            });
    });
}

/* Fin Atributos */

function actualizarAtributos(ids, idObjeto) {
    Atributos[idObjeto] = ids;
}

function cargarAtributos(idObjeto, ids) {
    try {
        ids = ((Atributos[idObjeto] != undefined) ? (Atributos[idObjeto]) : "");
    } catch (e) {

    }
}

var ruta;

function EliminarCategoria(sender) {
    ruta = getIdComponente(sender);
    Ext.Msg.alert(
        {
            title: jsEliminar + ' ' + jsTituloModulo,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxEliminar,
            icon: Ext.MessageBox.QUESTION
        });
    
}

function ajaxEliminar(button) {
    if (button == 'yes' || button == 'si') {
        showLoadMaskCategorias(function (load) {
        TreeCore[ruta].EliminarCategoria(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        $('#' + ruta + '_containerAttributes').parent().css('display', 'none');
                        try {
                            App.storeCategoriasLibres.reload();
                        } catch (e) {

                        }
                    }
                    load.hide();
                }
            });
    });
    }
}