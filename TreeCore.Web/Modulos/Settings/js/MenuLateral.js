var Agregar = false;
var seleccionado;
var NivelSeleccionado = 0;


//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        hd_MenuSeleccionado.value = seleccionado.id;
        NivelSeleccionado = seleccionado.NivelSeleccionado

        if (NivelSeleccionado != undefined) {
            App.btnEditar.enable();
            App.btnEliminar.enable();
            App.btnActivar.enable();
            App.btnAnadir.enable();

            App.btnEditar.setTooltip(jsEditar);
            App.btnAnadir.setTooltip(jsAgregar);
            App.btnEliminar.setTooltip(jsEliminar);


            if (seleccionado.Activo) {
                App.btnActivar.setTooltip(jsDesactivar);
            }
            else {
                App.btnActivar.setTooltip(jsActivar);
            }
        }
        else {
            App.btnEditar.disable();
            App.btnEliminar.disable();
            App.btnAnadir.enable();

            App.btnEditar.setTooltip(jsEditar);
            App.btnAnadir.setTooltip(jsAgregar);
            App.btnEliminar.setTooltip(jsEliminar);
        }
    }
}
/*
function SeleccionarCarpeta(node, event) {
    var carpeta = node.id;
    var carpetaNombre = node.text;º
    hd_MenuSeleccionado.setValue(carpeta);

    seleccionado = datos;
    App.btnEditar.enable();
    App.btnEliminar.enable();
    App.btnActivar.enable();

    App.btnEditar.setTooltip(jsEditar);
    App.btnAnadir.setTooltip(jsAgregar);
    App.btnAnadir.enable();

    if (seleccionado.Activo) {
        App.btnActivar.setTooltip(jsDesactivar);
    }
    else {
        App.btnActivar.setTooltip(jsActivar);
    }
}*/

function DeseleccionarGrilla() {
    
    App.btnEditar.disable();
    App.btnActivar.disable();
    App.btnEliminar.disable();
    
}

function AfterRender() {
    App.Tree.setRootNode(null);
}

//DeseleccionarGrilla();

//FIN GESTION GRID 

// INICIO GESTION 

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

    if (NivelSeleccionado == undefined || NivelSeleccionado < hd_NivelMaxPermitido.value) {
        var storesACargar = [App.cmbModulo, App.cmbPaginaModulo, App.cmbIcono];

        showLoadMask(App.Tree, function (load) {
            recargarCombos(storesACargar, function (Fin) {
                if (Fin) {
                    App.txtNombre.focus(false, 200);
                    App.winGestion.setTitle(jsAgregar + ' ' + jsTituloModulo);
                    Agregar = true;
                    App.winGestion.show();
                }
                load.hide();
            });
        });
    }
    else
    {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsNivelMenuNoPermitido, buttons: Ext.Msg.OK });
    }
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
    TreeCore.AgregarEditar(Agregar,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winGestion.hide();
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

function MostrarEditar() {
    if (registroSeleccionado(App.Tree) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    VaciarFormulario();
    Agregar = false;
    App.winGestion.setTitle(jsEditar + " " + jsTituloModulo);

    var storesACargar = [App.storeModulo, App.storePaginaModulo, App.storeIcono];

    showLoadMask(App.Tree, function (load) {
        CargarStoresSerie(storesACargar, function (Fin) {
            if (Fin) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            App.txtNombre.focus(false, 200);
                            Refrescar();
                            load.hide();
                        },
                        eventMask:
                        {
                            showMask: true,
                            msg: jsMensajeProcesando
                        }
                    });
            }
            load.hide();
        });
    });
}

function Activar() {
    if (registroSeleccionado(App.Tree) && seleccionado != null) {
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
    if (registroSeleccionado(App.Tree) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsTituloModulo,
                msg: jsMensajeEliminar ,
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
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function SeleccionaProyectosTipos() {
    App.cmbProyectosTipos.getTrigger(0).show();
}

function RecargarProyectosTipos() {
    recargarCombos([App.cmbProyectosTipos]);
}

function SeleccionaModulos() {
    App.cmbModulo.getTrigger(0).show();
}

function RecargarModulos() {
    recargarCombos([App.cmbModulo]);
}

function SeleccionaMenuModulos() {
    App.btnAnadir.enable();
    App.btnAnadir.setTooltip(jsAgregar);

    App.cmbMenuModulo.getTrigger(0).show();
    Refrescar();
}

function RecargarMenuModulos() {
    recargarCombos([App.cmbMenuModulo]);
    Refrescar();
}


function RecargarPaginaModulo() {
    recargarCombos([App.cmbPaginaModulo]);
}

function SeleccionaPaginaModulo() {
    App.cmbPaginaModulo.getTrigger(0).show();
}

function RecargarIconos() {
    recargarCombos([App.cmbIcono]);
}

function SeleccionaIconos() {
    App.cmbIcono.getTrigger(0).show();
}

function ajaxRefreshArbol() {
    TreeCore.RefreshMenu({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else
            {
                var nodes = eval(result);

                if (App.Tree.getRootNode() != null) {
                    App.Tree.getRootNode().removeAll();
                }
                
                App.Tree.setRootNode(nodes[0]);
                App.Tree.getRootNode().expand();

                if (App.Tree.getRootNode() != null &&
                    App.Tree.getRootNode().childNodes != null &&
                    App.Tree.getRootNode().childNodes.length > 0)
                {
                    App.Tree.expand();
                }
                else if (!App.Tree.getRootNode().data.rootVacio)
                {
                    App.Tree.setRootNode(null);
                }
                
                ///

                ///
            }
        },
        eventMask: { showMask: true, msg: jsMensajeProcesando }
    });
}

function Refrescar() {
    App.btnAnadir.disable();
    App.btnEditar.disable();
    App.btnActivar.disable();
    App.btnEliminar.disable();

    ajaxRefreshArbol();
}

function printIconOnColum(value) {
    if (value != undefined && value != "") {
        return '<img src="' + value + '"/>';
    }
    else {
        return '<span>&nbsp;</span> ';
    }
}

var filtrarTree = function (tf, e) {
    var tree = App.Tree,
        logic = tree,
        text = tf.getRawValue();

    logic.clearFilter();
    tree.collapseAll();

    if (Ext.isEmpty(text, false)) {
        return;
    }

    if (e.getKey() === e.ESC) {
        clearFiltro();
    }
    else {
        try {
            var re = new RegExp(".*" + text + ".*", "i");
        }
        catch (err) {
            return;
        }

        logic.filterBy(function (node) {
            valid = false;

            if (re.test(node.data.Alias)) {
                valid = true;
            }
            if (re.test(node.data.text)) {
                valid = true;
            }
            if (re.test(node.data.NombreModulo)) {
                valid = true;
            }
            if (re.test(node.data.Pagina)) {
                valid = true;
            }
            if (re.test(node.data.Parametro)) {
                valid = true;
            }
            if (re.test(node.data.Nombre)) {
                valid = true;
            }
            return valid;
        });
    }
};

var clearFilter = function () {
    var field = App.txtBuscar,
        tree = App.Tree,
        logic = tree;

    field.setValue("");
    logic.clearFilter(true);
    tree.getView().focus();
    tree.expandAll();
};

// FIN GESTION 

// INICIO DRAG AND DROP

function BeforeDropNodo(node, data, overModel, dropPosition, dropHandlers) {

    var targetNodeID = data.records[0].getId();
    var destinationNodeID = overModel.getId();

    dropHandlers.wait = true;

    TreeCore.BeforeDropNodo(targetNodeID, destinationNodeID,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    dropHandlers.cancelDrop();
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    dropHandlers.processDrop();
                }
            },
        });
}

// FIN DRAG AND DROP