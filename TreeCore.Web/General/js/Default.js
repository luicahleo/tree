var TituloModulo = "Default";
var seleccionado = null;
var versionseleccionada = null;

function Errores(msg) {
    Ext.Msg.show({ title: 'Atención', msg: msg, buttons: Ext.Msg.OK, icon: Ext.MessageBox.WARNING });
}

// Arbol --------------------------------------------------

function CargarArbol() {
    TreeCenter.RecargarArbol({
        success: function (result) {
            var nodes = eval(result);
            if (nodes.length > 0) {
                App.tree.setRootNode(nodes[0]);
            }
            else {
                App.tree.getRootNode().removeAll();
            }
        },
        eventMask:
        {
            showMask: true,
            msg: "Procesando"
        }
    });
}

function DeshabilitarTodo() {

    App.btnAgregar.disable();
    App.btnDesplegar.disable();
    App.btnEditar.disable();
    App.btnEliminar.disable();

}

// Carpetas y Arbol ----------------------------

function fnAgregarNodo() {
    if (NodoSeleccionado()) {
        App.formArbol.getForm().reset();
        App.txtCodigo.focus(false, 500);
        App.winArbol.setTitle("Agregar Nodo");
        App.btnGuardarArbol.disable();
        // podemos agregar

        if (NodoSeleccionadoID() == "ROOT") {
            FormularioCenter();
            App.winArbol.center();
            App.winArbol.show();
        }
        else if (NodoSeleccionadoID().startsWith('CENTER')) {


            // podemos eliminar
            Ext.Msg.show(
                {
                    title: 'Agregar Nodo',
                    msg: '¿Desea agregar un SERVIDOR HARDWARE ó LÓGICO?',
                    buttons: Ext.Msg.YESNO,
                    buttonText: {
                        yes: 'SERVIDOR HARDWARE',
                        no: 'SERVIDOR LOGICO'
                    },
                    fn: function (btn, text) {
                        if (btn == 'yes' || btn == 'si') {

                            hdHardware.value = '1';
                            FormularioServerHardware();
                            App.winArbol.center();
                            App.winArbol.show();

                        }
                        else if (btn == 'no') {
                            hdHardware.value = '0';
                            FormularioServer();
                            App.winArbol.center();
                            App.winArbol.show();
                        }
                    },
                    icon: Ext.MessageBox.QUESTION
                });

        }
        else if (NodoSeleccionadoID().startsWith('HARDWARE')) {
            FormularioServer();
            App.winArbol.center();
            App.winArbol.show();
        }

    }
    else {
        Errores("Debe seleccionar una carpeta o el nodo raiz donde agregar la nueva unidad");
    }
}

function fnEditarNodo() {
    if (NodoSeleccionado()) {


        if (NodoSeleccionadoID().startsWith("CENTER")) {
            FormularioCenter();
        }
        else if (NodoSeleccionadoID().startsWith('SERVER')) {

            FormularioServer();
        }
        else if (NodoSeleccionadoID().startsWith('HARDWARE')) {

            FormularioServerHardware();
        }


        App.formArbol.getForm().reset();
        TreeCenter.MostrarEditarNodo(NodoSeleccionadoID(), {
            success: function (result) {
                if (result.Success) {
                    App.txtCodigo.focus(false, 500);
                    App.winArbol.setTitle("Editar Nodo");
                    // podemos agregar
                    App.winArbol.center();
                    App.winArbol.show();
                }
                else {
                    Errores(result.Result);
                }
            },
            failure: function (result) {
                Errores("Error durante la solicitud al servidor");
            },
            eventMask:
            {
                showMask: true,
                msg: "Procesando"
            }
        });
    }
    else {
        Errores("Debe seleccionar un nodo para editar");
    }
}

function fnEliminarNodo() {
    if (NodoSeleccionado()) {

        if (NodoSeleccionadoID() == "ROOT") {
            Errores("No es posible eliminar esta carpeta");
        }
        else {
            var nodo = App.tree.getSelectionModel().getSelection();
            if (nodo[0].childNodes.length > 0) {
                Errores("No es posible eliminar esta carpeta, tiene subcarpetas");
            }
            else {
                // podemos eliminar
                Ext.Msg.show(
                    {
                        title: 'Eliminar Nodo',
                        msg: '¿Desea eliminar el nodo?',
                        buttons: Ext.Msg.YESNO,
                        fn: function (btn, text) {
                            if (btn == 'yes' || btn == 'si') {
                                TreeCenter.EliminarNodo(NodoSeleccionadoID(), {
                                    success: function (result) {
                                        if (result.Success) {
                                            nodo[0].remove();
                                        }
                                        else {
                                            Errores(result.Result);
                                        }
                                    },
                                    failure: function (result) {
                                        Errores("Error durante la solicitud al servidor");
                                    },
                                    eventMask:
                                    {
                                        showMask: true,
                                        msg: "Procesando"
                                    }
                                });
                            }
                        },
                        icon: Ext.MessageBox.QUESTION
                    });
            }
        }
    }
    else {
        Errores("Debe seleccionar una carpeta para eliminar");
    }
}

function SeleccionarCarpeta(node, event) {
    //alert(node.id);
    var carpeta = event.id;
    App.hdID.value = carpeta;
    // hay que hacer la comprobación de si es root, tree center o tree server
    App.btnAgregar.disable();
    App.btnDesplegar.disable();
    App.btnEditar.disable();
    App.btnEliminar.disable();
    if (carpeta.startsWith("ROOT")) {
        // tenemos el ROOT seleccionado
        App.btnAgregar.enable();
    }
    if (carpeta.startsWith("CENTER")) {
        // tenemos una TREE CENTER selecionado
        App.btnAgregar.enable();
        App.btnDesplegar.disable();
        App.btnEditar.enable();
        App.btnEliminar.enable();
    }

    if (carpeta.startsWith("HARDWARE")) {
        // tenemos TREE SERVERHARDWARE selecionado
        App.btnAgregar.enable();
        App.btnDesplegar.enable();
        App.btnEditar.enable();
        App.btnEliminar.enable();
    }

    if (carpeta.startsWith("SERVER")) {
        // tenemos TREE SERVER selecionado
        App.btnEditar.enable();
        App.btnEliminar.enable();
    }

}

function Recargar() {
    CargarArbol();
    hdID.value = "";
    DeshabilitarTodo();

}

function NodoSeleccionadoID() {
    var nodo = App.tree.getSelectionModel().getSelection();
    return nodo[0].id;
}

function NodoSeleccionadoNombre() {
    var nodo = App.tree.getSelectionModel().getSelection();
    return nodo[0].text;
}

function NodoSeleccionado() {
    if (App.tree.getSelectionModel().getSelection() != null) {
        return true;
    }
    else {
        return false;
    }
}

// Funciones para DragDrop

// devolvemos false para indicar que ahi no se puede soltar el objeto
function ComprobarNodoParaDrop(dragOverEvent) {

    if (dragOverEvent.target.id.startsWith("AGENTE")) {
        // dragOverEvent.cancel = true;
        return false;
    }

    // el point es append, above or below
    if (dragOverEvent.point == "append") {
        return true;
    }
    else {
        // no permitimos soltar en sitios intermedios, solo en sitios donde se agregue
        return false;
    }

}

function ComprobarNodo(drogEvent) {
    //alert("Agente: " + drogEvent.dropNode.id);
    //alert("Carpeta: " + drogEvent.target.id);
    //ajaxEditarArbol(drogEvent.target.id, drogEvent.dropNode.id);

}

function ajaxEditarArbol(carpeta, agente) {

    TreeCenter.EditarArbol(carpeta, agente,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: strAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask:
            {
                showMask: true,
                msg: MensajeProcesando
            }
        });
}


function FormularioValidoArbol(valid) {
    if (valid) {
        App.btnGuardarArbol.setDisabled(false);
    }
    else {
        App.btnGuardarArbol.setDisabled(true);
    }
}

//CLIENTES
var TriggerClientes = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbCliente.clearValue();
            break;
        case 1:
            App.storeClientes.reload();
            break;
    }
}

//VERTICALES
var TriggerVerticales = function (el, trigger, index) {
    switch (index) {
        case 0:
            App.cmbVertical.clearValue();
            break;
        case 1:
            App.storeVerticales.reload();
            break;
    }
}


//ACTIVAR CAMPOS DE LOS FORMULARIOS
function FormularioCenter() {

    App.txtURL.show();
    App.txtNombreResponsable.show();
    App.txtEmailResponsable.show();

    App.txtAPI.hide();
    App.txtUserAPI.hide();
    App.txtClaveAPI.hide();
    App.cmbCliente.hide();
    App.cmbVertical.hide();
    App.txtVersion.hide();
    App.txtContactoAdministrativo.hide();
    App.txtContactoAdministrativoEmail.hide();
    App.txtContactoTecnico.hide();
    App.txtContactoTecnicoEmail.hide();

    App.numPrimerOcteto.allowBlank = true;
    App.numSegundoOcteto.allowBlank = true;
    App.numTercerOcteto.allowBlank = true;
    App.numCuartoOcteto.allowBlank = true;

    App.txtAPI.allowBlank = true;
    App.txtUserAPI.allowBlank = true;
    App.txtClaveAPI.allowBlank = true;
    App.cmbCliente.allowBlank = true;
    App.cmbVertical.allowBlank = true;

}

function FormularioServerHardware() {

    App.txtURL.show();
    App.txtNombreResponsable.show();
    App.txtEmailResponsable.show();

    App.txtAPI.hide();
    App.txtUserAPI.hide();
    App.txtClaveAPI.hide();
    App.cmbCliente.hide();
    App.cmbVertical.hide();
    App.txtVersion.hide();
    App.txtContactoAdministrativo.hide();
    App.txtContactoAdministrativoEmail.hide();
    App.txtContactoTecnico.hide();
    App.txtContactoTecnicoEmail.hide();

    App.numPrimerOcteto.allowBlank = true;
    App.numSegundoOcteto.allowBlank = true;
    App.numTercerOcteto.allowBlank = true;
    App.numCuartoOcteto.allowBlank = true;

    App.txtAPI.allowBlank = true;
    App.txtUserAPI.allowBlank = true;
    App.txtClaveAPI.allowBlank = true;
    App.cmbCliente.allowBlank = true;
    App.cmbVertical.allowBlank = true;
}

function FormularioServer() {

    App.txtURL.hide();
    App.txtNombreResponsable.hide();
    App.txtEmailResponsable.hide();

    App.txtAPI.show();
    App.txtUserAPI.show();
    App.txtClaveAPI.show();
    App.cmbCliente.show();
    App.cmbVertical.show();
    App.txtVersion.show();
    App.txtContactoAdministrativo.show();
    App.txtContactoAdministrativoEmail.show();
    App.txtContactoTecnico.show();
    App.txtContactoTecnicoEmail.show();

    App.numPrimerOcteto.allowBlank = false;
    App.numSegundoOcteto.allowBlank = false;
    App.numTercerOcteto.allowBlank = false;
    App.numCuartoOcteto.allowBlank = false;

    App.txtAPI.allowBlank = false;
    App.txtUserAPI.allowBlank = false;
    App.txtClaveAPI.allowBlank = false;
    App.cmbCliente.allowBlank = false;
    App.cmbVertical.allowBlank = false;

    App.storeClientes.reload();
    App.storeVerticales.reload();
}

// OCULTAR MENU PPAL

//var menState = 0;
//function miniMenu() {
//    var client = document.getElementById('cliente');
//    var infoEntorno = document.getElementById('infoEntorno');
//    var asd = document.getElementById('asideLeft');
//    var tit = document.querySelectorAll('div .x-title-text');
//    var sp = document.querySelectorAll('#mainMenu span');
//    var itmAdm = document.querySelectorAll('#mnuAdmistracion div.x-menu-item');
//    var btnMas = document.getElementById('btnMasMenuAdm');
//    var btn = document.getElementById('btnNoAside');

//    switch (menState) {
//        case 0:
//            sp.forEach(function (el) {
//                el.style.visibility = 'hidden';
//            })

//            tit.forEach(function (el) {
//                el.style.visibility = 'hidden';
//            })


//            if (itmAdm.length >= 8) {

//                for (var i = 8; i < itmAdm.length; i++) {
//                    itmAdm[i].style.display = 'none';
//                }

//                btnMas.style.display = 'block';
//                btnMas.style.top = '258px';
//            }


//            client.style.visibility = 'hidden';
//            infoEntorno.style.visibility = 'hidden';
//            asd.style.overflowX = 'hidden';
//            asd.style.width = '50px';
//            btn.style.transform = 'rotate(-180deg)';

//            setTimeout(function () {
//                client.style.height = '0px';
//            }, 300)

//            menState = 1;

//            break;

//        case 1:
//            client.style.height = '90px';
//            sp.forEach(function (el) {
//                el.style.visibility = 'visible';
//            })

//            tit.forEach(function (el) {
//                el.style.visibility = 'visible';
//            })


//            if (itmAdm.length >= 8) {

//                for (var i = 8; i < itmAdm.length; i++) {
//                    itmAdm[i].style.display = 'block';
//                }

//                btnMas.style.display = 'none';
//                btnMas.style.top = '258px';
//            }


//            asd.style.width = '240px';
//            btn.style.transform = 'rotate(360deg)';

//            setTimeout(function () {
//                client.style.visibility = 'visible';
//                infoEntorno.style.visibility = 'visible';
//                asd.style.overflowX = 'visible';
//            }, 300);


//            menState = 0;
//            break;
//    }


//}

//FIN OCULTAR MENU PPAL

//OCULTAR HEADER

var headState = 0;
function noHeader() {
    var hder = document.getElementById('hdDefault');
    var cont = document.getElementById('ctDefault');
    var ifrm = document.querySelector('#tabPpal iframe');
    //var mod = document.getElementById('lblModNoHeader');
    var btn = document.getElementById('btnNoHeader');


    switch (headState) {
        case 0:
            hder.style.display = 'none';
            cont.style.top = '0';
            btn.style.transform = 'rotate(-180deg)';
           // mod.style.display = 'block';

            if (ifrm != null) {
                ifrm.style.height = '95vh';
            }
            headState = 1;
            break;

        case 1:
            hder.style.display = 'flex';
            cont.style.top = '56px';
            btn.style.transform = 'rotate(360deg)';
           // mod.style.display = 'none';

            if (ifrm != null) {
                ifrm.style.height = '90vh';
            }
            headState = 0;
            break;

    }

}

//FIN OCULTAR HEADER

// INICIO BOTONES

function BotonInflaciones() {

    addTab(App.tabPpal, "Inflaciones", "Inflaciones", "pages/GlobalMaestroDetalle.aspx");
}

function BotonConflictividad() {
    addTab(App.tabPpal, "Conflictividad", "Conflictividad", "pages/GlobalGestionBasica.aspx");
}

function BotonMapas() {
    addTab(App.tabPpal, "Mapas", "Mapas", "ModGlobal/pages/Monedas.aspx");
}

// FIN BOTONES