//#region Control Sesion

var Ausente = false;
var AusenteTimer;
var LogoutTimer;

var NombreCookie = "VentanasTree";
var NombreCookieExit = "VentanasTreeExit";

$(document).ready(function ConfigurarToast() {
    if (document.title != "" && isNaN(document.title)) {
        CrearAgregarSesion(document.title);
    }
    Toast.enableTimers(true);
    Toast.setTheme(TOAST_THEME.LIGHT);
    Toast.setPlacement(TOAST_PLACEMENT.BOTTOM_RIGHT);
    Toast.setMaxCount(2);

    Cookies.set(NombreCookieExit, true);

    window.onfocus = function () {
        clearTimeout(AusenteTimer);
        if (Cookies.get(NombreCookieExit) == "false") {
            location.reload();
        } else if (document.title != "" && isNaN(document.title)) {
            ActivarSesion(document.title);
            if (Ausente) {
                var list = document.getElementsByClassName('close')
                for (var toast in list) {
                    try {
                        list[toast].click();
                    } catch (e) {

                    }
                }
                Toast.create(jsInfo, jsReanudarActividad, TOAST_STATUS.SUCCESS, 30000);
                Ausente = false;
                clearTimeout(LogoutTimer);
            }
        }
    };

    window.onblur = function () {
        if (document.title != "" && isNaN(document.title)) {
            AusenteTimer = setTimeout(function () { EntraAusente(document.title); }, TiempoSesion);
        }
    };


    window.onbeforeunload = function (event) {
        if (document.title != "" && isNaN(document.title)) {
            DesactivarSesion(document.title);
        }
    };
})

//#region Cookie Sesiones

function EntraFocus() {
    clearTimeout(AusenteTimer);
    if (Cookies.get(NombreCookieExit) == "false") {
        location.reload();
    } else if (document.title != "" && isNaN(document.title)) {
        ActivarSesion(document.title);
        if (Ausente) {
            var list = document.getElementsByClassName('close')
            for (var toast in list) {
                try {
                    list[toast].click();
                } catch (e) {

                }
            }
            Toast.create(jsInfo, jsReanudarActividad, TOAST_STATUS.SUCCESS, 30000);
            Ausente = false;
            clearTimeout(LogoutTimer);
        }
    }
};

function SaleFocus() {
    if (document.title != "" && isNaN(document.title)) {
        AusenteTimer = setTimeout(function () { EntraAusente(document.title); }, TiempoSesion);
    }
};

function EntraAusente(nombre) {
    if (document.title != "" && isNaN(document.title) && nombre == document.title) {
        DesactivarSesion(document.title);
        Toast.create(jsAtencion, jsInactividad, TOAST_STATUS.WARNING);
        Ausente = true;
        LogoutTimer = setTimeout(function () { TimerLogout(nombre); }, 300000);
    }
}

function TimerLogout(nombre) {
    if (document.title != "" && isNaN(nombre)) {
        if (nombre == document.title) {
            if (TodasAusentes()) {
                TreeCore.LogoutInicio(
                    {
                        success: function (result) {
                            if (!result.Success) {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {
                                Cookies.set(NombreCookieExit, false);
                                window.location.reload();
                            }
                        }
                    });
                Ausente = false;
            }
        }
    }
}

function CrearAgregarSesion(nombreVentana) {
    if (isNaN(nombreVentana)) {
        if (Cookies.get(NombreCookie) == undefined) {
            Cookies.set(NombreCookie, '{"' + nombreVentana + '": true}');
        } else {
            try {
                var jsonAux = JSON.parse(Cookies.get(NombreCookie));
            } catch (e) {
                var jsonAux = {};
            }
            jsonAux[nombreVentana] = true;
            Cookies.set(NombreCookie, JSON.stringify(jsonAux));
        }
    }
}

function ActivarSesion(nombreVentana) {
    if (isNaN(nombreVentana)) {
        try {
            var jsonAux = JSON.parse(Cookies.get(NombreCookie));
        } catch (e) {
            var jsonAux = {};
        }
        jsonAux[nombreVentana] = true;
        Cookies.set(NombreCookie, JSON.stringify(jsonAux));
    }
}

function DesactivarSesion(nombreVentana) {
    if (isNaN(nombreVentana)) {
        try {
            var jsonAux = JSON.parse(Cookies.get(NombreCookie));
        } catch (e) {
            var jsonAux = {};
        }
        jsonAux[nombreVentana] = false;
        Cookies.set(NombreCookie, JSON.stringify(jsonAux));
    }
}

function TodasAusentes() {
    var AllAusente = true;
    var jsonAux = JSON.parse(Cookies.get(NombreCookie));
    for (var ventana in jsonAux) {
        if (jsonAux[ventana]) {
            AllAusente = false;
        }
    }
    return AllAusente;
}

function GetSesiones() {

}

//#endregion

//#endregion

// #region CAMBIAR CLAVE

function winCambiarClaveBotonCambiar() {
    if (App.formClave.getForm().isValid()) {
        ajaxCambiarClave();
    }
    else {
        Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxCambiarClave() {
    var clave = false;

    TreeCore.CambiarClave(
        {
            success: function (result) {

                if (result.Result == "Cambio") {
                    App.winCambiarClave.hide();
                }
                else {
                    App.txtCambiarClave.clear();
                    App.txtCambiarClave2.clear();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function FormularioValidoCambiarClave(valid) {
    if (valid) {
        App.btnCambiarGuardar.setDisabled(false);
    }
    else {
        App.btnCambiarGuardar.setDisabled(true);
    }
}

// #endregion


//#region Load Incio

function LoadInicioGlobal() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "/Modulos/Global/Inicio.aspx");
}
function LoadInicioSettings() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "/Modulos/Settings/SettingsInicio.aspx");
}
function LoadInicioSites() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "/Modulos/Sites/SitesInicio.aspx");
}
function LoadInicioInventory() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "/Modulos/Inventory/InvInicio.aspx");
}
function LoadInicioFiles() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "/Modulos/Files/FilesInicio.aspx");
}
function LoadInicioMonitoring() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "/Modulos/Monitoring/MonitoringInicio.aspx");
}
function LoadInicioDataQuality() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "/Modulos/DataQuality/DQInicio.aspx");
}
function LoadInicioThirdParty() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "/Modulos/ThirdParty/ThirdInicio.aspx");
}
function LoadInicioImportExport() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "/Modulos/ImportExport/DataUpload.aspx");
}
function LoadInicioWorkOrders() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "/Modulos/WorkOrder/WOInicio.aspx");

}
function LoadInicioContracts() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "/Modulos/Contracts/ContractsInicio.aspx");
}
function LoadInicioContacts() {
    addTabUnclosable(App.tabPpal, jsInicio, jsInicio, "/Modulos/Contacts/ContactInicio.aspx");
}
function LoadNotFound() {
    addTabUnclosable(App.tabPpal, jsPaginaNoEncontrada, jsPaginaNoEncontrada, "General/PaginaNoEncontrada.aspx");
}

//#endregion