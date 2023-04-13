function Seleccion(sender, registro, index) {
    var datos = registro.TipoReajuste;
    ruta = getIdComponente(sender);
    //vaciarFormulario(ruta);
    if (!editar) {
        vaciarFormulario(ruta);

    } else {
        editar = false;
    }
    switch (datos) {
        case "1":


            App[ruta + '_' + 'txtFechaInicioRevision'].hide();
            App[ruta + '_' + 'txtFechaInicioRevision'].allowBlank = true;

            App[ruta + '_' + 'txtFechaProxima'].hide();
            App[ruta + '_' + 'txtFechaProxima'].allowBlank = true;

            App[ruta + '_' + 'chkControlFechaFin'].hide();

            App[ruta + '_' + 'txtFechaFinRevision'].hide();
            App[ruta + '_' + 'txtFechaFinRevision'].allowBlank = true;

            App[ruta + '_' + 'cmbInflaciones'].hide();
            App[ruta + '_' + 'cmbInflaciones'].allowBlank = true;

            App[ruta + '_' + 'txtCadencia'].hide();
            App[ruta + '_' + 'txtCadencia'].allowBlank = true;

            App[ruta + '_' + 'txtPorcentaje'].hide();
            App[ruta + '_' + 'txtPorcentaje'].allowBlank = true;

            App[ruta + '_' + 'txtCantidad'].hide();
            App[ruta + '_' + 'txtCantidad'].allowBlank = true;

            break;
        case "2":
            App[ruta + '_' + 'txtFechaInicioRevision'].show();
            if (App.winGestion.title.startsWith(jsEditar)) {

                App[ruta + '_' + 'txtFechaInicioRevision'].disable();
                App[ruta + '_' + 'txtFechaInicioRevision'].allowBlank = false;

                App[ruta + '_' + 'txtFechaProxima'].show();
                App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;

            }

            if (App.winGestion.title.startsWith(jsAgregar)) {
                App[ruta + '_' + 'txtFechaProxima'].hide();
                App[ruta + '_' + 'txtFechaInicioRevision'].allowBlank = false;
                App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;

                App['txtFechaInicio'].enable();
            }

            App[ruta + '_' + 'chkControlFechaFin'].show();

            if (!App[ruta + '_' + 'chkControlFechaFin'].checked) {
                App[ruta + '_' + 'txtFechaFinRevision'].show();
                App[ruta + '_' + 'txtFechaFinRevision'].allowBlank = false;
            }
            RecargarInflaciones(sender, registro, index);

            App[ruta + '_' + 'cmbInflaciones'].show();
            App[ruta + '_' + 'cmbInflaciones'].allowBlank = false;

            App[ruta + '_' + 'txtCadencia'].hide();
            App[ruta + '_' + 'txtCadencia'].allowBlank = true;

            App[ruta + '_' + 'txtPorcentaje'].hide();
            App[ruta + '_' + 'txtPorcentaje'].allowBlank = true;

            App[ruta + '_' + 'txtCantidad'].hide();
            App[ruta + '_' + 'txtCantidad'].allowBlank = true;

            if (App[ruta + '_' + 'hdRadio'].value == 2) {
                App[ruta + '_' + 'txtFechaProxima'].setValue(App[ruta + '_' + 'hdProximaFecha'].value);
                App[ruta + '_' + 'txtFechaFinRevision'].setValue(App[ruta + '_' + 'hdUltimaFecha'].value);
                App[ruta + '_' + 'cmbInflaciones'].setValue(App[ruta + '_' + 'hdInflacion'].value);
            }

            break;
        case "3":
            App[ruta + '_' + 'txtFechaInicioRevision'].show();
            if (App.winGestion.title.startsWith(jsEditar)) {

                App[ruta + '_' + 'txtFechaInicioRevision'].disable();
                App[ruta + '_' + 'txtFechaInicioRevision'].allowBlank = false;

                App[ruta + '_' + 'txtFechaProxima'].show();
                App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;

                App[ruta + '_' + 'txtFechaInicioRevision'].show();

            }

            if (App.winGestion.title.startsWith(jsAgregar)) {
                App[ruta + '_' + 'txtFechaInicioRevision'].allowBlank = false;
                App[ruta + '_' + 'txtFechaProxima'].hide();
                App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;
                App['txtFechaInicio'].enable();
            }

            App[ruta + '_' + 'chkControlFechaFin'].show();

            if (!App[ruta + '_' + 'chkControlFechaFin'].checked) {
                App[ruta + '_' + 'txtFechaFinRevision'].show();
                App[ruta + '_' + 'txtFechaFinRevision'].allowBlank = false;
            }

            App[ruta + '_' + 'cmbInflaciones'].hide();
            App[ruta + '_' + 'cmbInflaciones'].allowBlank = true;

            App[ruta + '_' + 'txtCadencia'].show();
            App[ruta + '_' + 'txtCadencia'].allowBlank = false;

            App[ruta + '_' + 'txtPorcentaje'].hide();
            App[ruta + '_' + 'txtPorcentaje'].allowBlank = true;

            App[ruta + '_' + 'txtCantidad'].show();
            App[ruta + '_' + 'txtCantidad'].allowBlank = false;


            if (App[ruta + '_' + 'hdRadio'].value == 3) {
                App[ruta + '_' + 'txtFechaProxima'].setValue(App[ruta + '_' + 'hdProximaFecha'].value);
                App[ruta + '_' + 'txtFechaFinRevision'].setValue(App[ruta + '_' + 'hdUltimaFecha'].value);
                App[ruta + '_' + 'txtCadencia'].setValue(App[ruta + '_' + 'hdCadencia'].value);
                App[ruta + '_' + 'txtCantidad'].setValue(App[ruta + '_' + 'hdValor'].value);
            }

            break;
        case "4":
            App[ruta + '_' + 'txtFechaInicioRevision'].show();
            if (App.winGestion.title.startsWith(jsEditar)) {

                App[ruta + '_' + 'txtFechaInicioRevision'].disable();
                App[ruta + '_' + 'txtFechaInicioRevision'].allowBlank = false;

                App[ruta + '_' + 'txtFechaProxima'].show();
                App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;

            }
            if (App.winGestion.title.startsWith(jsAgregar)) {
                App[ruta + '_' + 'txtFechaInicioRevision'].allowBlank = false;
                App[ruta + '_' + 'txtFechaProxima'].hide();
                App[ruta + '_' + 'txtFechaProxima'].allowBlank = false;
                App['txtFechaInicio'].enable();

            }


            App[ruta + '_' + 'chkControlFechaFin'].show();

            if (!App[ruta + '_' + 'chkControlFechaFin'].checked) {
                App[ruta + '_' + 'txtFechaFinRevision'].show();
                App[ruta + '_' + 'txtFechaFinRevision'].allowBlank = false;
            }

            App[ruta + '_' + 'cmbInflaciones'].hide();
            App[ruta + '_' + 'cmbInflaciones'].allowBlank = true;

            App[ruta + '_' + 'txtCadencia'].show();
            App[ruta + '_' + 'txtCadencia'].allowBlank = false;

            App[ruta + '_' + 'txtPorcentaje'].show();
            App[ruta + '_' + 'txtPorcentaje'].allowBlank = false;

            App[ruta + '_' + 'txtCantidad'].hide();
            App[ruta + '_' + 'txtCantidad'].allowBlank = true;


            if (App[ruta + '_' + 'hdRadio'].value == 4) {
                App[ruta + '_' + 'txtFechaProxima'].setValue(App[ruta + '_' + 'hdProximaFecha'].value);
                App[ruta + '_' + 'txtFechaFinRevision'].setValue(App[ruta + '_' + 'hdUltimaFecha'].value);
                App[ruta + '_' + 'txtCadencia'].setValue(App[ruta + '_' + 'hdCadencia'].value);
                App[ruta + '_' + 'txtPorcentaje'].setValue(App[ruta + '_' + 'hdValor'].value);
            }
            break;
    }
    marcarObligatorio(App[ruta + '_' + 'txtCantidad']);
    marcarObligatorio(App[ruta + '_' + 'cmbInflaciones']);
    marcarObligatorio(App[ruta + '_' + 'txtCadencia']);
    marcarObligatorio(App[ruta + '_' + 'txtPorcentaje']);

    FormularioValido(null, true);


}
function RecargarInflaciones(sender, registro, index) {
    ruta = getIdComponente(sender);
    recargarCombos([App[ruta + '_' + 'cmbInflaciones']]);
}

function marcarObligatorio(c) {

    //if (c.triggerWrap != undefined) {
    //    c.triggerWrap.removeCls("itemForm-novalid");
    //}

    //if (!c.allowBlank && c.xtype != "checkboxfield") {
    //    c.addListener("change", anadirClsNoValido, false);
    //    c.addListener("focusleave", anadirClsNoValido, false);

    //    c.removeCls("ico-exclamacion-10px-red");
    //    c.addCls("ico-exclamacion-10px-grey");
    //}

    //if (!c.allowBlank && c.cls == 'txtContainerCategorias') {
    //    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
    //}

    if (c.triggerWrap != undefined) {
        c.triggerWrap.removeCls("itemForm-novalid");
        c.triggerWrap.addCls("itemForm-valid");
    }

    if (!c.allowBlank && c.xtype != "checkboxfield") {
        c.addListener("change", anadirClsNoValido, false);
        c.addListener("focusleave", anadirClsNoValido, false);

        c.removeCls("ico-exclamacion-10px-grey");
        c.addCls("ico-exclamacion-10px-red");
    }

    if (c.allowBlank && c.cls == 'txtContainerCategorias') {
        App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
    }

}

function validarFechaInicioRevision(sender, registro, index) {
    ruta = getIdComponente(sender);

    App[ruta + '_' + 'txtFechaFinRevision'].setMinValue(App[ruta + '_' + 'txtFechaInicioRevision'].value);

}

function validarFechaFinRevision(sender, registro, index) {
    ruta = getIdComponente(sender);

    App[ruta + '_' + 'txtFechaInicioRevision'].setMaxValue(App[ruta + '_' + 'txtFechaFinRevision'].value);

}

function OcultarFechaFin(sender, registro, index) {
    ruta = getIdComponente(sender);
    if (App[ruta + '_' + 'chkControlFechaFin'].checked) {
        App[ruta + '_' + 'txtFechaFinRevision'].hide();
    } else {
        App[ruta + '_' + 'txtFechaFinRevision'].show();
        App[ruta + '_' + 'txtFechaFinRevision'].allowBlank = false;
    }
}

function reiniciarFormulario(ruta) {
    App[ruta + '_' + 'RGTipo'].reset();
    App[ruta + '_' + 'txtFechaInicioRevision'].hide();
    App[ruta + '_' + 'txtFechaInicioRevision'].allowBlank = true;

    App[ruta + '_' + 'txtFechaProxima'].hide();
    App[ruta + '_' + 'txtFechaProxima'].allowBlank = true;

    App[ruta + '_' + 'chkControlFechaFin'].hide();

    App[ruta + '_' + 'txtFechaFinRevision'].hide();
    App[ruta + '_' + 'txtFechaFinRevision'].allowBlank = true;

    App[ruta + '_' + 'cmbInflaciones'].hide();
    App[ruta + '_' + 'cmbInflaciones'].allowBlank = true;

    App[ruta + '_' + 'txtCadencia'].hide();
    App[ruta + '_' + 'txtCadencia'].allowBlank = true;

    App[ruta + '_' + 'txtPorcentaje'].hide();
    App[ruta + '_' + 'txtPorcentaje'].allowBlank = true;

    App[ruta + '_' + 'txtCantidad'].hide();
    App[ruta + '_' + 'txtCantidad'].allowBlank = true;
}

function vaciarFormulario(ruta) {
    App[ruta + '_' + 'txtFechaProxima'].setValue('');
    App[ruta + '_' + 'txtFechaFinRevision'].setValue('');
    App[ruta + '_' + 'cmbInflaciones'].setValue(undefined);
    App[ruta + '_' + 'txtCadencia'].setValue(undefined);
    App[ruta + '_' + 'txtPorcentaje'].setValue(undefined);
    App[ruta + '_' + 'txtCantidad'].setValue(undefined);
}