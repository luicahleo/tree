

//CONTROL EXTENSION PERMANENTE COMBO SEARCH GLOBAL

var estadoCombo;

function ExtendCombo() {
    estadoCombo = 1;

    var el = document.getElementById("ComponenteHeader_btnSearchTB");

    el.style.width = "95%";
    el.style.opacity = 1;

    var el2 = document.getElementById("ComponenteHeader_btnSearchTB-bodyEl");
    el2.style.opacity = 1;

    if (document.documentElement.clientWidth <= 320) {
        ocultarBotonesCabecera();
    }
    else if (document.documentElement.clientWidth <= 580) {
        ocultarLblModulo();
    }
    else if (document.documentElement.clientWidth <= 768) {
        el.style.paddingLeft = "15%";
    }

    App.ComponenteHeader_ComboTipoSearch.show();

    //CAMBIAR EL ICONO A CRUZ DURANTE LA EXTENSION DEL COMBO

    document.getElementById("icosearchtopbar").src = "/ima/ico-close-topbar.svg";
}

function HideCombo() {
    estadoCombo = 0;

    var el = document.getElementById("ComponenteHeader_btnSearchTB");

    el.style.width = "0px";
    el.style.opacity = 0;
    el.style.paddingLeft = "0";

    var el2 = document.getElementById("ComponenteHeader_btnSearchTB-bodyEl");
    el2.style.opacity = 0;

    if (document.documentElement.clientWidth <= 320) {
        mostrarBotonesCabecera();
    }
    else {
        mostrarLblModulo();
    }

    App.ComponenteHeader_ComboTipoSearch.hide();
    App.ComponenteHeader_btnSearchTB.clearValue();
    App.ComponenteHeader_ComboTipoSearch.clearValue();

    //RESET DEL ICO AL DEFECTO

    var eldef = document.getElementById("ComponenteHeader_ComboTipoSearch-trigger-picker");

    var icoClass = "ico-columnas-yesno";

    var urlstring = "url('/ima/" + icoClass + ".svg";

    eldef.style.backgroundImage = urlstring;

    document.getElementById("icosearchtopbar").src = "/ima/ico-search-topbar.svg";

}

function UpdateIcon() {

    //AQUI CAMBIAMOS EL ICON SELECCIONADO DEL COMBO AL DEL ITEM QUE PICAMOS

    var el = document.getElementById("ComponenteHeader_ComboTipoSearch-trigger-picker");

    var icoClass = App.ComponenteHeader_ComboTipoSearch.getValue();

    var urlstring = "url('/ima/" + icoClass + ".svg";

    el.style.backgroundImage = urlstring;
}


// FIN BOTONES

function RecargarInicio() {
    window.location.reload();
}

function ocultarBotonesCabecera() {
    document.getElementById("logoTree").style.display = "none";
    document.getElementById("liModulos").style.display = "none";
    document.getElementById("liUsuarios").style.display = "none";
    document.getElementById("logoTree").style.width = "45px";
}

function ocultarLblModulo() {
    document.getElementById("ComponenteHeader_lblModulo").style.display = "none";
}

function mostrarBotonesCabecera() {
    document.getElementById("logoTree").style.display = "inherit";
    document.getElementById("logoTree").style.width = "108px";
    document.getElementById("liModulos").style.display = "inherit";
    document.getElementById("liModulos").style.width = "52px";
    document.getElementById("liUsuarios").style.display = "inherit";
    document.getElementById("liUsuarios").style.width = "52px";
}

function mostrarLblModulo() {
    document.getElementById("ComponenteHeader_lblModulo").style.display = "inherit";
    document.getElementById("ComponenteHeader_lblModulo").style.width = "55";
}

window.addEventListener("resize", function () {
    if (estadoCombo == 1) {
        if (document.documentElement.clientWidth <= 320) {
            ocultarBotonesCabecera();
        }
        else if (document.documentElement.clientWidth <= 768) {
            mostrarBotonesCabecera();
            ocultarLblModulo();
        }
        else {
            mostrarBotonesCabecera();
            mostrarLblModulo();
        }
    }
    else if (estadoCombo == 0) {
        if (document.documentElement.clientWidth <= 320) {
            mostrarBotonesCabecera();
        }
        else {
            mostrarLblModulo();
        }
    }
});

function BotonUsuarios() {
    parent.addTab(parent.App.tabPpal, parent.jsGestionUsuario, parent.jsGestionUsuario, "../General/GestionUsuario.aspx");
}

function AcercaDe(sender, registro, index) {

    var idComponente = sender.id.split('_');
    idComponente.pop();
    var win = idComponente + "_winacerca";
    App[win].show();
    //CargarStoresSerie(stores);
}

function AcercaDeCerrar(sender, registro, index) {

    var idComponente = sender.id.split('_');
    idComponente.pop();
    var win = idComponente + "_winacerca";
    App[win].hide();
    //CargarStoresSerie(stores);
}

function openGlobal() {

    let path = window.location.pathname;

    /*if (path !== "/Default.aspx" && path !== "/") {
        OpenWindowWithPost('../Default.aspx', '', 'Global', '');
    }
    else {
        window.location.reload()
    }*/

    window.location.replace("/");

}

var mostrarElement = false;
function mostrarElementos(sender, registro, index) {

    ruta = getIdComponente(sender);
    var elementos = document.getElementById(ruta + '_ctMenuModulos-innerCt').children;
    var tmn = 9;

    if (mostrarElement == true) {

        App.ComponenteHeader_btnMostrar.setText(jsMostrarMenos);

        for (var e = tmn; e < elementos.length; e++) {
            elementos[e].style.display = "flex";
        }

        App.ComponenteHeader_btnMostrar.show();
        mostrarElement = false;
    }
    else {

        App.ComponenteHeader_btnMostrar.setText(jsMostrarMas);

        for (var e = tmn; e < elementos.length - 1; e++) {

            elementos[e].style.display = "none";
        }
        for (var e = 0; e < tmn; e++) {
            if (elementos && elementos[e]) {
                elementos[e].style.height = "80px";
            }
        }

        App.ComponenteHeader_btnMostrar.show();
        mostrarElement = true;
    }
}

function ajaxPintarMenuModulos(sender, registro, index) {

    ruta = getIdComponente(sender);

    TreeCore[ruta].PintarMenuModulos(
        {

            success: function (result) {

                result.Result.forEach((y, x) => {
                    let sRutaModulo = y.RutaModulo + '/Default.aspx';
                    let url = `/${y.Modulo}`;

                    let btn = Ext.create('Ext.Button',
                        {
                            sModulo: y.Modulo,
                            sRutaModulo: sRutaModulo,

                            id: 'btn' + y.Modulo,
                            cls: 'btn-trans ' + 'btn' + y.Modulo,
                            text: y.str,
                            href: url,
                            preventDefault: true,

                            handler: function (btn) {
                                window.location.replace(url);
                            }
                        }
                    )

                    if (y.disable) {

                        btn.disable();
                        btn.addCls('modulos-icon-disable');
                    }

                    App[ruta + "_ctMenuModulos"].add(btn);
                });

                mostrarElementos(sender, registro, index);
            },
        });
}

