function SeleccionarValorMostrarEditar(id, valor) {
    try {
        App[id].setValue(valor);
    } catch (e) {

    }
}

function AnadirListener(sender) {
    try {
        this.child().addListener('validitychange', FormularioValidoAtributo);
    } catch (e) {

    }
}

function FormularioValidoAtributo(sender) {
    FormularioValido(sender, sender.isValid(), undefined, sender.id.split('_')[0]);
}

function EsconderPadre(sender, registro, index) {
    contenedor = document.getElementById(sender.id);
    contenedorPadre = contenedor.parentElement;
    contenedorPadre.style.display = "none";
}

function IsHidden(sender) {
    if (sender.config.hidden == true) {
        sender.hide();
        contenedor = document.getElementById(sender.id);
        contenedorPadre = contenedor.parentElement;
        contenedorPadre.style.display = "none";
    }

    Ext.each(App[sender.config.id].getBody().query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.reset();

            if (c.triggerWrap != undefined) {
                c.triggerWrap.removeCls("itemForm-novalid");
            }

            if (!c.allowBlank && c.xtype != "checkboxfield") {
                c.addListener("change", anadirClsNoValido, false);
                c.addListener("focusleave", anadirClsNoValido, false);

                c.removeCls("ico-exclamacion-10px-red");
                c.addCls("ico-exclamacion-10px-grey");
            }

            if ((c.allowBlank && c.cls == 'txtContainerCategorias') || c.xtype == "checkboxfield") {
                App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
            }
        }
    });
}