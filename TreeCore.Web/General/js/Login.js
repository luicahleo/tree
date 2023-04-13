var numeros = new Array(3);
var clickCount = 1;
var cliente = 'telefonica';

// LOGIN
function pulsointro(caja, e) {
    if (e.getKey() == 13) {
        LoginClick();
    }
}

function LoginClick() {
    App.txtUserName.disabled = true;
    App.txtPassword.disabled = true;
    TreeCore.Login_Click(
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.show(
                        {
                            title: jsAvisos,
                            msg: result.Result,
                            buttons: Ext.Msg.OK,
                            fn: Terminar,
                            icon: Ext.MessageBox.WARNING
                        });
                }
            },
            eventMask:
            {
                showMask: true
            }

        });
}

function Terminar() {
    App.txtUserName.enabled = true;
    App.txtPassword.enabled = true;
    App.txtUserName.setValue("");
    App.txtPassword.setValue("");
    App.txtUserName.focus(false, 200);
}

function BorrarClick() {
    App.txtUserName.setValue("");
    App.txtPassword.setValue("");
    App.txtUserName.focus(false, 200);
}

function FormularioValido(valid) {
    if (valid) {
        App.btnAcceder.setDisabled(false);
    }
    else {
        App.btnAcceder.setDisabled(true);
    }
}

function VaciarFormCodigo() {
    ajaxwinCodigoVaciar();
}

function CierraCodigo() {
    Sites.CierraCodigo();
}

function Recargar() {
    window.location.reload();
}

function pasaLogin(a) {
    TreeCore.pasaLogin(a,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAvisos, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
}

var onAfterRenderLabel = function (lbl) {

    var label = lbl;
    label.getEl().on("mousedown", function (event) {
        if ((event.button) === 2) {
            if (clickCount == 3) {
                App.btnDesarrollador.show();
            }
            else
                clickCount = clickCount + 1;
        }
    }, true);

    label.getEl().on("contextmenu", function () {

    },
        lbl);

};

var showResult = function (btn) {
    window.location = "Login.aspx";
};
// FIN LOGIN

// CAMBIO DE CLAVE
function EnviarCorreoCambioClave() {

    App.winCambiarClave.show();
}

function winCambiarClaveBotonCambiar() {
    TreeCore.RecuperarContra(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAvisos, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.winCambiarClave.hide();
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function FormularioValido(valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}
// FIN CAMBIO DE CLAVE

// INICIO MENU
function logoCliente() {

    var dvLogo = document.getElementById('logoLogin');

    if (cliente == 'telefonica') {
        dvLogo.classList.add('clientTelef-login');
    }
    else {
        dvLogo.classList.add('clientDemo-login');
    }

}

function MostrarMenu(cadena) {

    //OcultarMenu();

    if (cadena != "") {
        for (var i = 0; i < App.cycleIdiomas.menu.items.items.length; i++) {

            if (IncluyeMenu(PasarCadenatoArray(cadena), App.cycleIdiomas.menu.items.items[i].id)) {
                App.cycleIdiomas.menu.items.items[i].show();

            }

        }
    }
    else {

    }
}

function IncluyeMenu(array, objeto) {
    return (array.indexOf(objeto) != -1);
}

function PasarCadenatoArray(cadena) {
    return cadena.split("|")
}

//function OcultarMenu() {

//    App.btnIdiomaPortugues.hide();
//    App.btnIdiomaCAM.hide();
//    App.btnIdiomaChile.hide();
//    App.btnIdiomaAleman.hide();
//    App.btnIdiomaEcuador.hide();
//    App.btnIdiomaEspanol.hide();
//    App.btnIdiomaFrances.hide();
//    App.btnIdiomaIngles.hide();
//    App.btnIdiomaArgentina.hide();
//    App.btnIdiomaColombia.hide();
//    App.btnIdiomaPeru.hide();
//    App.btnIdiomaUruguay.hide();
//    App.btnIdiomaVenezuela.hide();
//    App.btnIdiomaNoruego.hide();
//}

function ObtenerMenu() {
    TreeCore.ListaMenu({
        success: function (result) {
            if (result.Result == null && result.Result == '') {
                // no hacemos nada, no hay datos para mostrar
            } else {
                MostrarMenu(result.Result);
            }
        },
        eventMask: {
            showMask: true,
            msg: jsMensajeProcesando
        }
    });
}
// FIN MENU

function CambioIdioma(sender, registro, index) {
    TreeCore.CambioIdioma(registro.id);


}