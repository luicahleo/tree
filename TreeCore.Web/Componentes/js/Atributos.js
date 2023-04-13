/* Orden Categorias */
function OrdenMasAtributo(sender, registro, index) {
    ruta = getIdComponente(sender);
    rutaPadre = getIdComponentePadre(sender);

    showLoadMaskCategorias(function (load) {
        TreeCore[ruta].ModificarOrden(true,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    TreeCore[rutaPadre].PintarAtributos(true, true, {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            DragDropCategorias();
                            DragDropAtributosCategorias();
                            load.hide();
                        }
                    });
                }
            });
    });
}

function OrdenMenosAtributo(sender, registro, index) {
    ruta = getIdComponente(sender);
    rutaPadre = getIdComponentePadre(sender);
    showLoadMaskCategorias(function (load) {
        TreeCore[ruta].ModificarOrden(false,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    TreeCore[rutaPadre].PintarAtributos(true, true, {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            DragDropCategorias();
                            DragDropAtributosCategorias();
                            load.hide();
                        }
                    });
                }
            });
    });
}

/* Funciones Stores */

function RecargarCombos(sender, registro, index) {
    showLoadMaskCategorias(function (load) {
        recargarCombos([sender], function Fin(fin) {
            if (fin) {
                load.hide();
            }
        });
    });
}

var NombreModificado;

function ModificarNombreAtributo(sender) {
    NombreModificado = sender.id;
}

function CambiarNombreAtributo(sender, registro, index) {
    if (!sender.wasValid) {
        Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsNombreInvalido, buttons: Ext.Msg.OK });
        sender.setValue(sender.originalValue);
    } else if (NombreModificado == sender.id) {
        ruta = getIdComponente(sender);
        showLoadMaskCategorias(function (load) {
            TreeCore[ruta].CambiarNombre(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: App.jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                        }
                        load.hide();
                        sender.originalValue = sender.value;
                        NombreModificado = '';

                        App.btnNext.setText(jsGuardado);
                        App.btnNext.removeCls("animation-text");
                        App.btnNext.setIconCls("ico-tic-wh");
                    }
                });
        });
    }
}


var rutaDelete;
var rutaPadreDelete;

function ajaxEliminarAtributo(button) {
    if (button == 'yes' || button == 'si') {
        showLoadMaskCategorias(function (load) {
            TreeCore[rutaDelete].EliminarAtributo(
                {
                    success: function (result) {
                        if (result.Result != null && result.Result != '') {
                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            load.hide();
                        }
                        else {
                            if (rutaPadreDelete != "") {
                                TreeCore[rutaPadreDelete].PintarAtributos(true, true, {
                                    success: function (result) {
                                        if (result.Result != null && result.Result != '') {
                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                        }
                                        DragDropCategorias();
                                        DragDropAtributosCategorias();
                                        load.hide();
                                    }
                                });
                            }
                            else {
                                TreeCore.PintarAtributos(true, true, {
                                    success: function (result) {
                                        $('#' + rutaDelete + '_mainContainerAtributos').parent().css('display', 'none');
                                        load.hide();

                                        App.btnNext.setText(jsGuardado);
                                        App.btnNext.removeCls("animation-text");
                                        App.btnNext.setIconCls("ico-tic-wh");
                                    }
                                });
                            }
                        }
                    }
                });
        });
    }
}

function EliminarAtributo(sender, registro, index) {
    rutaDelete = getIdComponente(sender);
    rutaPadreDelete = getIdComponentePadre(sender);
    Ext.Msg.alert(
        {
            title: jsEliminar + ' ' + jsAtributo,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxEliminarAtributo,
            icon: Ext.MessageBox.QUESTION
        });
}

//function MostrarDataSetting(sender) {
//    ruta = sender.id.split('_'); ruta.pop(); ruta = ruta.join('_');
//    TreeCore[ruta].MostrarDataSetting(
//        {
//            success: function (result) {
//                if (!result.Success) {
//                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
//                } else {
//                    if (result.Result) {
//                        App[ruta + '_' + 'menuItemConfiguracionDatos'].enable();
//                    } else {
//                        App[ruta + '_' + 'menuItemConfiguracionDatos'].disable();
//                    }
//                }
//            }
//        });
//}

function MostrarItems(sender, Name) {
    ruta = sender.id.split('_'); ruta.pop(); ruta = ruta.join('_');
    Name = App[ruta + '_' + 'contenedorControl'].getBody().component.items.items[0].xtype;
    if (Name == 'combobox' || Name == 'multicombo' || Name == 'netmulticombo') {
        App[ruta + '_' + 'menuItemConfiguracionDatos'].enable();
    }
    else {
        App[ruta + '_' + 'menuItemConfiguracionDatos'].disable();
    }

    if (Name == 'numberfield') {
        App[ruta + '_' + 'menuItemTrafficLight'].enable();
    }
    else {
        App[ruta + '_' + 'menuItemTrafficLight'].disable();
    }

}

//#region SEMAFORO

function slider() {

    $(function () {

        // function to create slider ticks
        var ID = `#${ruta.split("_")[1]}slider`;

        // create slider  
        $(ID).slider2({
            // set min and maximum values
            // day hours in this example
            min: ValorMinimo,
            max: ValorMaximo,
            // step
            // quarter of an hour in this example
            step: 0.5,
            // range
            range: true,
            // show tooltips
            tooltips: true,
            // current data
            handles: ValoresRangos,
            // display type names
            showTypeNames: true,
            typeNames: {
                'Red': 'Red',
                'Green': 'Green',
                'Orange': 'Orange'
            },
            ModCalidad: false,
            // main css class (of unset data)
            mainClass: ColorDefecto,
            // slide callback
            slide: function (e, ui) {

                Valores = ui.values;
                console.log(e, ui);
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
    });


}

function winFormCenterSimple(obj) {


    obj.center();
    obj.updateLayout();

}

function winSaveSemaforoBotonGuardar(sender, registro, index) {
    ruta = getIdComponente(sender);
    //Montar Json con los a guardar
    ValoresAGuardar = [];
    ValoresAGuardar.push({ value: ValorMinimo, type: ColorDefecto });

    for (let i = 0; i < Valores.length; i++) {

        ValoresAGuardar.push({ value: Valores[i], type: Colores[i] });
    }
    ValoresAGuardar.push({ value: ValorMaximo, type: ColorDefecto });
    var jsonCompleto = JSON.stringify(ValoresAGuardar);

    TreeCore[ruta].GuardarSemaforo(jsonCompleto,
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {

                    App[ruta + '_' + 'winSaveSemaforo'].hide();
                }
            }
        });
}

function FormularioValidoSemaforo(sender, valid) {
    ruta = sender.owner.id.split('_'); ruta.pop(); ruta = ruta.join('_');
    //if (valid) {
    //    App[ruta + '_' + 'btnGuardar'].setDisabled(false);
    //}
    //else {
    //    App[ruta + '_' + 'btnGuardar'].setDisabled(true);
    //}

}

function WinTrafficLight(sender, registro, index) {
    ruta = getIdComponente(sender);
    TreeCore[ruta].ObtenerDatosSemaforo(
        {
            success: function (result) {
                if (!result.Success) {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {

                    var Rangos = result.Result.split(';');

                    if (Rangos[0] != 'No MaxMinValue') {

                        ColorDefecto = Rangos[0];
                        ValorMaximo = parseFloat(Rangos[2]);
                        ValorMinimo = parseFloat(Rangos[1]);
                        ValoresRangos = [];
                        Colores = [];
                        Valores = [];
                        if (Rangos.length == 4) {
                            ValoresRangos = JSON.parse(Rangos[3]);
                            //Obtener los valores de los colores desde el Json
                            Colores = ValoresRangos
                                .map(x => x.type);

                            Valores = ValoresRangos
                                .map(x => x.value);
                        }
                    }// Cuando no tenemos Maxvalue o Minvalue configurado
                    else {
                        ValorMaximo = 100;
                        ValorMinimo = 0;
                        ValoresRangos = [];
                        ColorDefecto = Rangos[1];
                        Colores = [];
                        Valores = [];
                    }
                    slider();
                    App[ruta + '_' + 'winSaveSemaforo'].show();
                    if (Valores.length > 0) {
                        App[ruta + '_' + 'btnBorrarRango'].enable();
                        App[ruta + '_' + 'btnGuardarSemaforo'].enable();
                    }
                }
            }
        });
}

function AbrirVentanaDataSetting(sender) {
    ruta = getIdComponente(sender);

    if (App.AtributosConfiguracion_hdAtributoID != undefined) {
        App.AtributosConfiguracion_hdAtributoID.setValue(App[ruta + '_' + 'hdAtributoID'].value);
        App.AtributosConfiguracion_hdCategoriaAtributoID.setValue(App[ruta + '_' + 'hdCategoriaAtributoID'].value);
    }

    windowDataSetting();
}

function AbrirVentanaFormatos(sender, pos) {
    ruta = getIdComponente(sender);

    if (App.AtributosConfiguracion_hdAtributoID != undefined) {
        App.AtributosConfiguracion_hdAtributoID.setValue(App[ruta + '_' + 'hdAtributoID'].value);
        App.AtributosConfiguracion_hdCategoriaAtributoID.setValue(App[ruta + '_' + 'hdCategoriaAtributoID'].value);
    }

    GridFormatRule(sender.up().el.dom);
}

var ColorSeleccionado;
var ValorMaximo;
var ValorMinimo;
var ValoresRangos = [];
var ColorDefecto;
var Valores = [];
var Colores = [];
var ValoresAGuardar = [];

function SeleccionarColor() {
    ColorSeleccionado = App[ruta + '_' + 'cmbColores'].selection.data.Nombre;
    App[ruta + '_' + 'btnAnadirRango'].enable();
}

function AnadirRango() {
    var ID = `#${ruta.split("_")[1]}slider`;
    var $slider = $(ID);
    var Valor = 0;
    if (Valores.length > 0) {
        Valor = Valores[Valores.length - 1] + 10;
    }
    else {
        Valor = ValorMinimo;
    }

    Colores.push(ColorSeleccionado);

    if (Valor < ValorMaximo) {

        Valores.push(Valor);
    }
    else {
        Valores.push(ValorMaximo);
    }

    $slider.slider2('addHandle', {
        value: Valor,
        type: ColorSeleccionado
    });

    App[ruta + '_' + 'btnBorrarRango'].enable();
    App[ruta + '_' + 'btnGuardarSemaforo'].enable();

}

function BorrarRango() {

    var ID = `#${ruta.split("_")[1]}slider`;
    var $slider = $(ID);
    // trigger removeHandle event on active handle
    $slider.slider2('removeHandle', Valores.length - 1);
    Valores.pop();
    Colores.pop();
    if (Valores.length == 0) {
        App[ruta + '_' + 'btnBorrarRango'].disable();
        App[ruta + '_' + 'btnGuardarSemaforo'].disable();
    }

}

//#endregion
function AbrirVentanaRestricciones(sender, pos) {
    ruta = getIdComponente(sender);

    if (App.AtributosConfiguracion_hdAtributoID != undefined) {
        App.AtributosConfiguracion_hdAtributoID.setValue(App[ruta + '_' + 'hdAtributoID'].value);
        App.AtributosConfiguracion_hdCategoriaAtributoID.setValue(App[ruta + '_' + 'hdCategoriaAtributoID'].value);
    }

    GridAddRestriction(sender.up().el.dom);
}