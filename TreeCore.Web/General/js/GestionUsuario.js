
function clicBotonGuardar() {
    $uploadCrop.croppie('result', {
        type: 'canvas',
        size: 'viewport'
    }).then(function (resp) {
        App.hdSRC.setValue(resp);
        GestionBotonGuardar();
        if (hdTieneImagen.value != "") {
            html = '<img src="' + resp + '" />';
            document.getElementById("ContenedorImagen").style.display = "none";
            $("#imgUser").html(html);

        }
    });
}


function Unmasker() {
    txtPassRepeat.passwordMask.setMode('showall');
}

function RecargarInicio() {
    parent.window.location.reload();
}

function mandatoryTooltip(sender, registro, index) {
    if (sender.value == '') {
        sender.setActiveErrors('This field is mandatory');
    }
    else if (!sender.isValid()) {
        sender.setActiveErrors(App.hdMensajePassword.value);
    }
}

function ajaxCierreSesion(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.Logout(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '' && result.Result != 'ClaveRepetida' && result.Result != 'ClaveIncorrecta') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        if (result.Result == 'ClaveRepetida') {
                            Ext.Msg.alert(
                                {
                                    title: App.hdEditarUsuario.value,
                                    cls: 'winFormEditado',
                                    icon: Ext.MessageBox.WARNING,
                                    msg: App.hdMensajeClaveRepetida.value,
                                    buttons: Ext.Msg.OK,
                                    buttonText: { ok: 'OK' }
                                });
                        }
                        else if (result.Result == 'ClaveIncorrecta') {
                            Ext.Msg.alert(
                                {
                                    title: App.hdEditarUsuario.value,
                                    icon: Ext.MessageBox.WARNING,
                                    cls: 'winFormEditado',
                                    msg: App.hdMensajeClaveIncorrecta.value,
                                    buttons: Ext.Msg.OK,
                                    buttonText: { ok: 'OK' }
                                });
                        }
                        else {
                            RecargarInicio();
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

function GestionBotonGuardar() {
    winAsignarImagenGuardar();
    TreeCore.EditarPerfil({
        success: function (result) {
            if (result.Result != null && result.ErrorMessage != '' && result.ErrorMessage != 'logout' && result.ErrorMessage != 'ClaveDistinta' && result.ErrorMessage != 'ClaveIncorrecta') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                parent.document.getElementById("ComponenteHeader_lblNombre-textEl").innerHTML = App.txtName.value + " " + App.txtApellidos.value;
                parent.document.getElementById("ComponenteHeader_lblEmail-textEl").innerHTML = App.txtEmail.value;

                if (result.ErrorMessage == 'logout') {
                    Ext.Msg.alert(
                        {
                            title: App.hdEditarUsuario.value,
                            msg: App.hdMensajeEditarUsuario.value,
                            buttons: Ext.Msg.YESNO,
                            buttonText: {
                                yes: App.hdYes.value
                            },
                            fn: ajaxCierreSesion,
                            cls: 'winFormEditado',
                            icon: Ext.MessageBox.QUESTION
                        });
                }
                else if (result.ErrorMessage == 'ClaveDistinta') {
                    Ext.Msg.alert(
                        {
                            title: App.hdEditarUsuario.value,
                            icon: Ext.MessageBox.WARNING,
                            cls: 'winFormEditado',
                            msg: App.hdMensajeClaveDiferente.value,
                            buttons: Ext.Msg.OK,
                            buttonText: { ok: 'OK' }
                        });
                }
                else if (result.ErrorMessage == 'ClaveIncorrecta') {
                    Ext.Msg.alert(
                        {
                            title: App.hdEditarUsuario.value,
                            icon: Ext.MessageBox.WARNING,
                            cls: 'winFormEditado',
                            msg: App.hdMensajeClaveIncorrecta.value,
                            buttons: Ext.Msg.OK,
                            buttonText: { ok: 'OK' }
                        });
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

function validarExtensionImagen(sender, registro, index) {

    let sp = sender.value.split(".");
    let extension = sp[sp.length - 1];

    if (extension.toLowerCase() == "svg" || extension.toLowerCase() == "png" || extension.toLowerCase() == "jpg" || extension.toLowerCase() == "jpeg") {

        previewFile(sender);

    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormatoImagenNoPermitido, buttons: Ext.Msg.OK });

    }
}


function previewFile(sender) {

    const dom = Ext.getDom(sender.fileInputEl);
    const reader = new FileReader();
    reader.onload = e => document.getElementById("imgUser").childNodes[0].src = e.target.result;
    reader.readAsDataURL(dom.files[0]);

}




function winAsignarImagenGuardar() {
    ajaxAsignarImagen();
}

function ajaxAsignarImagen() {

    TreeCore.AsignarImagenUsuario(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });

                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });

}


function ajaxAsignarImag() {
    App.txtPasswordConfirm.setValue(App.txtPasswordField.value);

    TreeCore.AsignarSrc(
        {
            success: function (result) {
                if (result.Success == false) {
                    validarExtensionImagen();
                }

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });

}