var jsonComentarios = {}
var oDropzone;
var oTemplate;

$(document).ready(function ConfigurarDropzone() {
    Dropzone.autoDiscover = false;
    oTemplate = $('#dz-preview-template').html();
    oDropzone = Dropzone.getElement('#dZUpload').dropzone;
    //oDropzone.dropzone.options.addRemoveLinks = true;
    oDropzone.options.previewTemplate = oTemplate;
    //oDropzone.dropzone.options.previewsContainer = "#template-preview";
});

function mostrarSeguimientos(sender) {
    if (sender.parentElement.getElementsByClassName('contSeguimientos')[0].classList.contains('hidden')) {
        sender.parentElement.getElementsByClassName('contSeguimientos')[0].classList.remove('hidden')
    }
    else {
        sender.parentElement.getElementsByClassName('contSeguimientos')[0].classList.add('hidden')
    }
    App.contGeneral.updateLayout();
}

function mostrarComentarios(sender) {
    if (sender.parentElement.parentElement.getElementsByClassName('contSubSeguimiento')[0].classList.contains('hidden')) {
        sender.parentElement.parentElement.getElementsByClassName('contSubSeguimiento')[0].classList.remove('hidden')
    } else {
        sender.parentElement.parentElement.getElementsByClassName('contSubSeguimiento')[0].classList.add('hidden')
    }
    App.contGeneral.updateLayout();
}

function activarEditar(sender) {
    if (sender.parentElement.parentElement.classList.contains('editar')) {
        sender.parentElement.parentElement.classList.remove('editar')
    } else {
        sender.parentElement.parentElement.classList.add('editar')
    }
    if (sender.parentElement.parentElement.getElementsByClassName('descripcionSeguimiento')[0].contentEditable == "true") {
        sender.parentElement.parentElement.getElementsByClassName('descripcionSeguimiento')[0].contentEditable = "false"
    } else {
        sender.parentElement.parentElement.getElementsByClassName('descripcionSeguimiento')[0].contentEditable = "true"
    }
    App.contGeneral.updateLayout();
    jsonComentarios[sender.parentElement.getAttribute('segid')] = sender.parentElement.parentElement.getElementsByClassName('descripcionSeguimiento')[0].textContent;
}

var SegSelect;


function GuardarSeguimiento(sender) {
    TreeCore.AñadirNuevoComentario(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeSeguimientos.reload();
                    App.txtaComentario.reset();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function SubirNuevosDocumentos(sender) {
    SubirDocumentos();
    App.hdSegSeleccionadoID.setValue(sender.parentElement.getAttribute('SegID'));
    App.txtComentarioDocumento.setValue(sender.parentElement.parentElement.getElementsByClassName('descripcionSeguimiento')[0].innerText);
}

function EliminarSeguimiento(sender) {
    SegSelect = sender.parentElement.getAttribute('segid');
    Ext.Msg.alert(
        {
            title: jsEliminar + ' ' + jsComentario,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: AjaxEliminarSeguimiento,
            icon: Ext.MessageBox.QUESTION
        });
}

function AjaxEliminarSeguimiento(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarComentario(SegSelect,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        App.storeSeguimientos.reload();
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

function EditarSeguimiento(sender) {
    activarEditar(sender);
    TreeCore.EditarComentario(sender.parentElement.getAttribute('segid'), sender.parentElement.parentElement.getElementsByClassName('descripcionSeguimiento')[0].textContent,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storeSeguimientos.reload();
                    App.txtaComentario.reset();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function CancelarEditar(sender) {
    sender.parentElement.parentElement.getElementsByClassName('descripcionSeguimiento')[0].textContent = jsonComentarios[sender.parentElement.getAttribute('segid')];
    activarEditar(sender);
}

//#region SubSeguimientos

function GuardarRespuesta(sender) {
    if (sender.parentElement.parentElement.getElementsByClassName('panelTextoResp')[0].value != "") {
        TreeCore.AñadirNuevoSubComentario(sender.getAttribute('segid'), sender.parentElement.parentElement.getElementsByClassName('panelTextoResp')[0].value,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        App.storeSeguimientos.reload();
                        sender.parentElement.parentElement.getElementsByClassName('panelTextoResp')[0].value = "";
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

//#endregion


function BtnEstadoActual(sender) {
    if (App.pnlseguimientosActual.hidden) {
        App.pnlseguimientosActual.show();
    } else {
        App.pnlseguimientosActual.hide();
    }
}

function hidePn(sender) {
    if (App.pnLateral.collapsed == false) {
        App.pnLateral.collapse();
        sender.addCls('btnRotate');
    } else {
        App.pnLateral.expand();
        sender.removeCls('btnRotate');
    }
}

function CambiarEstadoWorkOrder() {
    document.getElementById('cmbEstadosWorkOrder').style.backgroundColor = App.cmbEstadosWorkOrder.selection.data.Color;
}
function SeleccionarEstadoWorkOrder() {
    TreeCore.CambiarEstadoWorkOrder();
}

function SubirArchivoNuevoComentario() {
    App.winGestion.show();
}

function SeleccionarFiltro(sender, TipoFiltro) {
    App.btnTodos.setPressed(false);
    App.btnDocumentos.setPressed(false);
    App.btnCambios.setPressed(false);
    App.btnComentarios.setPressed(false);
    sender.setPressed(true);
    App.hdTipoVista.setValue(TipoFiltro);
    App.storeSeguimientos.reload();
    App.storeSeguimientosAnteriores.reload();
}

function SiguienteEstado(sender) {
    App.hdEstadoSiguienteID.setValue(sender.estadoID);
    App.formSiguienteEstado.reset();
    App.winSiguienteEstado.setTitle(jsAgregar + ' ' + jsComentario);
    App.winSiguienteEstado.show();
}

function winSiguienteEstadoBotonGuardar() {
    TreeCore.PasarEstadoWorkOrder(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winSiguienteEstado.hide();
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

function ReasignarUsuario(sender) {
    App.formReasignar.reset();
    App.storeUsuariosReasignar.reload();
    App.winReasignar.show();
}

function winReasignarBotonGuardar() {
    TreeCore.CambiarUsuarioEstado(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.winReasignar.hide();
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

function Refrescar(sender) {
    showLoadMask(App.vwContenedor, function (load) {
        TreeCore.CargarEstadoActual(parseInt(App.hdWorkOrderID.value),
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        load.hide();
                    }
                    else {
                        CargarStoresSerie([App.storeTareas, App.storeWorkOrderEstados, App.storeSeguimientos, App.storeSeguimientosAnteriores], function Fin(fin) {
                            if (fin) {
                                load.hide();
                            }
                        });
                    }
                }
            });
    });
}

function CargarInfo(sender) {

}

function FormularioSiguienteEstadoValido(valid) {
    if (valid) {
        App.btnGuardarSiguienteEstado.enable();
    } else {
        App.btnGuardarSiguienteEstado.disable();
    }
}

function FormularioReasignarValido(valid) {
    if (valid) {
        App.btnGuardarReasignar.enable();
    } else {
        App.btnGuardarReasignar.disable();
    }
}

function SubirDocumentos() {
    oDropzone.removeAllFiles();
    App.hdSegPadreID.setValue(0);
    App.hdSegSeleccionadoID.setValue(0);
    App.txtComentarioDocumento.setValue('');
    App.cmbTiposDocumentos.reset();
    oDropzone.disable();
    App.winDocumentos.setTitle(jsAgregar + ' ' + jsComentario);
    App.winDocumentos.show();
    ValidarSubirDocumentos();
}

function SeleccionarTipoDocumento(sender, reg) {
    SeleccionarCombo(sender);
    oDropzone.removeAllFiles();
    oDropzone.destroy();
    oDropzone = new Dropzone('#dZUpload', {
        acceptedFiles: App.cmbTiposDocumentos.selection.data.Extensiones,
        previewTemplate: oTemplate
    });
    oDropzone.on("complete", file => {
        ValidarSubirDocumentos();
    });
    oDropzone.on("removedfile", file => {
        ValidarSubirDocumentos();
    });
    ValidarSubirDocumentos();
}

function RecargarTipoDocumento(sender, reg) {
    RecargarCombo(sender);
    oDropzone.removeAllFiles();
    oDropzone.destroy();
    oDropzone = new Dropzone('#dZUpload', {
        previewTemplate: oTemplate
    });
    oDropzone.disable();
    ValidarSubirDocumentos();
}

function ValidarSubirDocumentos() {
    App.btnGuardarDocumentos.enable();
    if (App.txtComentarioDocumento.value == '') {
        App.btnGuardarDocumentos.disable();        
    }
    if (App.cmbTiposDocumentos.value == undefined || App.cmbTiposDocumentos.value == '') {
        App.btnGuardarDocumentos.disable();        
    }
    if (oDropzone.files.length == 0) {
        App.btnGuardarDocumentos.disable();
    }
    oDropzone.files.forEach(function (item) {
        if (!item.accepted) {
            App.btnGuardarDocumentos.disable();
        }
    });
}

function GuardarDocumentos() {
    var data = {
        SegSelect: App.hdSegSeleccionadoID.value,
        Comentario: App.txtComentarioDocumento.value,
        SeguimientoEstadoID: App.hdEstadoActID.value,
        TipoDocumentoID: App.cmbTiposDocumentos.value,
        UsuarioID: App.hdUsuarioID.value,
        SegPadreID: App.hdSegPadreID.value,
        "CodeLanguage": App.hdCulture.value,
    };
    var searchParams = new URLSearchParams(data);
    oDropzone.options.url = '/PaginasComunes/SeguimientoDocumentos.ashx?'+searchParams;
    oDropzone.uploadFiles(oDropzone.files)
}

var DocSelect;

function EliminarDocumento(sender) {
    DocSelect = sender;
    Ext.Msg.alert(
        {
            title: jsEliminar + ' ' + jsComentario,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxEliminarDocumentos,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxEliminarDocumentos(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.DesactivarDocumento(parseInt(DocSelect.getAttribute('documentoid')),
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        DocSelect.parentElement.style.display = 'none';
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

function SubirDocumentoRespuesta(sender) {
    SubirDocumentos();
    App.hdSegPadreID.setValue(sender.parentElement.parentElement.parentElement.parentElement.getAttribute('SegID'));
}