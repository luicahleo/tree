//  #region RESIZERS PARA VENTANAS MODALES (CALCULO EXTERNO)

function winFormCenterSimple(obj) {


    obj.center();
    obj.updateLayout();

}

function winFormResize(obj) {

    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }
    obj.center();
    obj.updateLayout();


}

function winFormResizeDockBot(obj) {


    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }
    obj.center();
    obj.updateLayout();




    //AQUI SE SETEA EL CENTER ABAJO

    const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);
    obj.setY(vh - obj.height);

    obj.updateLayout();


}

function formResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(872);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(620);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
    obj.center();
    obj.updateLayout();




}

window.addEventListener('resize', function () {


    var dv = document.querySelectorAll('div.winForm-respSimple');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormCenterSimple(obj);
    }


    var dv = document.querySelectorAll('div.winForm-resp');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResize(obj);
    }

    var frm = document.querySelectorAll('div.ctForm-resp');
    for (i = 0; i < frm.length; i++) {
        var obj = Ext.getCmp(frm[i].id);
        formResize(obj);
    }



    var dv = document.querySelectorAll('div.winForm-respDockBot');
    for (i = 0; i < dv.length; i++) {
        var obj = Ext.getCmp(dv[i].id);
        winFormResizeDockBot(obj);
    }






});

// #endregion

// #region DISEÑO
function displayMenu(btn) {


    //ocultar todos los paneles
    var name = '#' + btn;
    App.pnCFilters.hide();
    App.pnGridsAsideMyFilters.hide();

    //GET componente a mostrar desde el boton por ID

    var PanelAMostrar = Ext.ComponentQuery.query(name)[0];
    PanelAMostrar.show();
    PanelAMostrar.updateLayout();

}

function showwinAddTab() {
    App.winAddTabFilter.show();

}

function showwinSaveQF() {
    App.winSaveQF.show();

}

// #endregion

// #region DIRECT METHOD

var ValorMinimo = 0;
var ValorMaximo = 100;
var medioOrange = 25;
var medioRed = 80;

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;

        App.btnEliminar.enable();
        App.btnActivar.enable();
        App.btnEditar.enable();

        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEditar.setTooltip(jsEditar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnDescargar.setTooltip(jsDescargar);

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }
    }
}

function DeseleccionarGrilla() {
    App.GridRowSelect.clearSelections();
    App.btnActivar.disable();
    App.btnEditar.disable();
    App.btnEliminar.disable();
}

function VaciarFormulario() {
    App.formGestn.getForm().reset();
}

function AgregarEditar() {
    VaciarFormulario();
    App.winSaveSemaforo.setTitle(jsAgregar);
    Agregar = true;
    App.txtMedio.setValue("25%");
    App.txtMedio2.setValue("80%");
    medioOrange = 25;
    medioRed = 80;
    slider();
    App.winSaveSemaforo.show();
}

function winGestionBotonGuardar() {
    if (App.formGestn.getForm().isValid()) {
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
                    App.winSaveSemaforo.hide();
                    App.storePrincipal.reload();
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
    if (registroSeleccionado(App.gridSemaforos) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {
    Agregar = false;
    App.winSaveSemaforo.setTitle(jsEditar);

    TreeCore.MostrarEditar(
        {
            success: function (result) {
                medioOrange = App.txtMedio.value.split("%", length - 1)[0];
                medioRed = App.txtMedio2.value.split("%", length - 1)[0];
                slider();
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                App.storePrincipal.reload();
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function Activar() {
    if (registroSeleccionado(App.gridSemaforos) && seleccionado != null) {
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
                    App.storePrincipal.reload();
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
    if (registroSeleccionado(App.gridSemaforos) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
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
                    App.storePrincipal.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function Refrescar() {
    DeseleccionarGrilla();
    App.storePrincipal.reload();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }
}

function FormularioValidoGestion(valid) {
    if (valid) {
        App.btnGuardar.setDisabled(false);
    }
    else {
        App.btnGuardar.setDisabled(true);
    }
}

function slider() {
    //$(function () {
    //    $("#slider-range").slider({
    //        range: true,
    //        min: 0,
    //        max: 100,
    //        values: [25, 80],
    //        suffix: "%",
    //        slide: function (event, ui) {
    //            App.txtInicio.setValue("0%");
    //            App.txtMedio.setValue(ui.values[0] + "%");
    //            App.txtMedio2.setValue(ui.values[1] + "%");
    //            App.txtFin.setValue("100%");
    //        }
    //    });

    //    App.txtInicio.setValue("0%");
    //    App.txtMedio.setValue($("#slider-range").slider("values", 0) + "%");
    //    App.txtMedio2.setValue($("#slider-range").slider("values", 1) + "%");
    //    App.txtFin.setValue("100%");
    //});

    $(function () {

        // create slider  
        $("#slider-range").slider2({
            // set min and maximum values
            // day hours in this example
            min: ValorMinimo,
            max: ValorMaximo,
            // step
            // quarter of an hour in this example
            step: 1,
            // range
            range: true,
            // show tooltips
            tooltips: true,
            // current data
            handles: [{
                value: ValorMinimo,
                type: "Green"
            }, {
                value: medioOrange,
                type: "Orange"
            }, {
                value: medioRed,
                type: "Red"
            }, {
                value: ValorMaximo,
                type: "Red"
            }],
            ModCalidad: true,
            // display type names
            showTypeNames: true,
            typeNames: {
                'Red': 'Red',
                'Green': 'Green',
                'Orange': 'Orange'
            },
            // main css class (of unset data)
            //mainClass: 'Red',
            // slide callback
            slide: function (e, ui) {
                App.txtInicio.setValue("0%");
                App.txtMedio.setValue(ui.values[1] + "%");
                App.txtMedio2.setValue(ui.values[2] + "%");
                App.txtFin.setValue("100%");
                //console.log(e, ui);
            },
            // handle clicked callback
            handleActivated: function (event, handle) {
                // get select element
                var select = $(this).parent().find('.slider-controller select');
                // set selected option
                select.val(handle.type);
            }

        });

        // when clicking on handler
        $(document).on('click', '.slider a', function () {
            var select = $('.slider-controller select');
            // enable if disabled
            //select.attr('disabled', false);
            alert($(this).attr('data-type'));
            select.val($(this).attr('data-type'));
            /*if ($(this).parent().find('a.ui-state-active').length)
              $(this).toggleClass('ui-state-active');*/
        });

        App.txtInicio.setValue("0%");
        App.txtMedio.setValue($("#slider-range").slider2("values", 1) + "%");
        App.txtMedio2.setValue($("#slider-range").slider2("values", 2) + "%");
        App.txtFin.setValue("100%");
    });
}

// #endregion