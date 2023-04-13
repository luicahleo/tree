var nRound = 0;

function PasswordField_Render(sender) {

    var elp = sender.el.findParent('.x-form-element', 5, true);
    objMeter = elp.createChild({ tag: "div", id: sender.id + 'objMeter', 'class': "nwf_strengthMeter" });

    objMeter.setWidth(180);
    scoreBar = objMeter.createChild({ tag: "div", id: sender.id + 'scoreBar', 'class': "nwf_scoreBar" });

    if (Ext.isIE && !Ext.isIE7) { // Fix style for IE6
        objMeter.setStyle('margin-left', '3px');
    }
}

function PasswordField_KeyUp(sender, e) {
    scoreBar = Ext.get(sender.id + 'scoreBar');

    var p = e.target.value;

    var maxWidth = objMeter.getWidth() - 2;

    nRound = calcStrength(p);

    var scoreWidth = (maxWidth / 100) * nRound;
    scoreBar.setWidth(scoreWidth, true);
}

function PasswordField_KeyUp1(sender) {
    scoreBar = Ext.get(sender.id + 'scoreBar');

    var maxWidth = objMeter.getWidth() - 2;

    nRound = 0;

    var scoreWidth = (maxWidth / 100) * nRound;
    scoreBar.setWidth(scoreWidth, true);
}

function PasswordField_Focus(sender) {
    if (!Ext.isOpera) { // don't touch in Opera
        objMeter = Ext.get(sender.id + 'objMeter');
        objMeter.addClass('nwf_strengthMeter-focus');
    }
}

function PasswordField_Blur(sender) {
    if (!Ext.isOpera) { // don't touch in Opera
        objMeter = Ext.get(sender.id + 'objMeter');
        objMeter.removeClass('nwf_strengthMeter-focus');
    }
}

function PasswordField_Resize(sender) {
    objMeter = Ext.get(sender.id + 'objMeter');
    objMeter.setWidth(180);
}

function FormularioValidoClave(valid) {
    if (nRound < complejidad.getValue()) {
        valid = false;
        App.txtCambiarClave.markInvalid(jsClaveValidacionMsg1.value + ' ' + nRound + ' ' + jsClaveValidacionMsg2.value + complejidad.getValue());
    }
    else {
        App.txtCambiarClave.clearInvalid();
    }
    if (txtCambiarClave.getValue() != txtCambiarClave2.getValue()) {
        valid = false;
        App.txtCambiarClave2.markInvalid(jsClaveNoCorrespondencia.value);
    }
    else {
        App.txtCambiarClave2.clearInvalid();
    }
    if (valid) {
        App.btnCambiar.setDisabled(false);
    }
    else {
        App.btnCambiar.setDisabled(true);
    }
}

function winCambiarClaveBotonCambiar() {
    Sites.ResetearContraseña(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion.Value, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.INFO, msg: jsContraseñaModificada, buttons: Ext.Msg.OK });
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}