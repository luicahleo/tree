
// #region RESPONSIVE PAGINA


let PuntoCorteL = 900;
let PuntoCorteS = 512;
let selectedCol = 0;
let isOnMobC = 0;
let editar = true;

function ControlSlider() {
    let containerSize = App.wrapComponenteCentral.getWidth();


    let pnmain = App.gridElementos;
    let col2 = App.pnFormulas;
    let tbsliders = App.tbSliders;
    let btnPrevSldr = App.btnPrevGrid;
    let btnNextSldr = App.btnNextGrid;


    //state 2 cols

    if (containerSize > PuntoCorteL) {
        pnmain.show();
        if (App.hdFormulaID.value != '') {
            col2.show();
        }

        selectedCol = 1;

        isOnMobC = 0;

    }

    if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {
        pnmain.show();



        if (selectedCol == 3) {
            col2.hide();
        }
        else {
            if (App.hdFormulaID.value != '') {
                col2.show();
            }
        }
        isOnMobC = 0;




    }


    // state 1 col
    if (containerSize < PuntoCorteS && isOnMobC == 0) {
        pnmain.show();
        col2.hide();

        btnPrevSldr.disable();
        btnNextSldr.enable();

        selectedCol = 1;

        isOnMobC = 1;
    }



    //CONTROL SHOW OR HIDE BOTONES SLIDER
    if (pnmain.hidden == true) {

        tbsliders.show();

        if (pnmain.hidden != true && col2.hidden == false) {
            btnPrevSldr.disable();

        }

    }
    else {


        tbsliders.hide();
        btnPrevSldr.disable();
        btnNextSldr.enable();


    }

}

function SliderMove(NextOrPrev) {

    let containerSize = App.wrapComponenteCentral.getWidth();

    let pnmain = App.gridElementos;
    let col1 = App.pnFormulas;
    let btnPrevSldr = App.btnPrevGrid;
    let btnNextSldr = App.btnNextGrid;

    //SELECCION EN 2  PANELES
    if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {

        if (NextOrPrev == 'Next') {
            col1.hide();
            selectedCol = 3;

            btnPrevSldr.enable();
            btnNextSldr.disable();

        }
        else if (NextOrPrev == 'Prev') {

            if (App.hdFormulaID.value != '') {
                col1.show();
            }
            selectedCol = 2;

            btnPrevSldr.disable();
            btnNextSldr.enable();

        }
    }

    //SELECCION EN 1  PANEL
    else {

        if (NextOrPrev == 'Next' && selectedCol == 1) {
            pnmain.hide();
            if (App.hdFormulaID.value != '') {
                col1.show();
            }
            selectedCol = 2;

            btnPrevSldr.enable();
            btnNextSldr.disable();

        }
        else if (NextOrPrev == 'Prev' && selectedCol == 2) {
            pnmain.show();
            col1.hide();
            selectedCol = 1;

            btnPrevSldr.disable();
            btnNextSldr.enable();

        }


    }


}

let handlePageSizeSelect = function (item, records) {
    let curPageSize = App.storePrincipal.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storePrincipal.pageSize = wantedPageSize;
        App.storePrincipal.load();
    }

}


// #endregion

// #region grid

// #region Menu grid ELEMENTOS

let formulaIDAnterior = '';
let expressionInput = null;
function Grid_RowSelect(sender, registro, index) {
    
    let datos = registro.data;
    if (datos.Tipo != 'FORMULA') {
        App.hdEditadoObjetoID.setValue(datos.Id);
        App.hdTablaModeloDatoID.setValue(datos.TablaModeloDatoID);
        App.btnEliminar.disable();
        App.hdRuta.setValue('');
        App.hdFormula.setValue('');
        App.hdVariablesFormulas.setValue('');
        App.hdFormulaID.setValue('');
        if (expressionInput != undefined) {
            expressionInput.setVariables([]);
        }

    } else {
        App.btnEliminar.enable();
        App.hdFormulaID.setValue(datos.Id);
        App.storeCampos.reload();
        TreeCore.CargarFormula(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        let formula = App.hdFormula.value;
                        let options = null;

                        if (expressionInput != undefined && formulaIDAnterior == datos.Id && expressionInput.getOptions().variables.length > 0) {
                            options = expressionInput.getOptions();
                        }
                        else {
                            options = {
                                variables: [
                                    {
                                        variableId: 0,
                                        name: jsAgregarDeCampoVinculado,

                                        value: 11,
                                        selectorCampoVinculado: true
                                    }
                                ]
                            };
                        }
                        formulaIDAnterior = App.hdFormulaID.value;
                        let store = App.storeCampos.data.items
                        for (let i = 0; i < store.length; i++) {
                            let dato = store[i].data
                            let esDinamico;
                            dato.Name = dato.Name.replace(" ", "").replace("(", "").replace(")", "");
                            if (dato.ColumnaModeloDatoID) {
                                esDinamico = true
                            } else {
                                esDinamico = false
                            }
                            let atributo = {
                                variableId: i + 1,
                                name: dato.Name,
                                campoVinculadoID: dato.Campo,
                                selectorCampoVinculado: false,
                                ruta: "",
                                esDinamico: esDinamico,
                            }


                            if (options.variables.find(a => a.campoVinculadoID == atributo.campoVinculadoID) == undefined) {
                                options.variables.push(atributo);
                            }
                        }
                        let variablesAsignadas = eval(hdVariablesFormulas.value);
                        let controlVariableID;

                        if (variablesAsignadas != undefined && variablesAsignadas.length > 0) {
                            let iterador = 0;
                            for (let i = 0; i < variablesAsignadas.length; i++) {

                                controlVariableID = false;
                                for (let j = 1; j < options.variables.length; j++) {
                                    if (variablesAsignadas[i].campoVinculadoID == options.variables[j].campoVinculadoID) {
                                        controlVariableID = true;
                                        variablesAsignadas[i].variableId = options.variables[j].variableId;



                                        if (variablesAsignadas[i].ruta != "") {
                                            iterador++;
                                            let reemplazar = '[' + variablesAsignadas[i].variableId + ']';
                                            let ruta = variablesAsignadas[i].name;
                                            formula = formula.replace(reemplazar, ruta);

                                        } else {
                                            iterador++;
                                            let reemplazar = '[' + variablesAsignadas[i].variableId + ']';
                                            formula = formula.replace(reemplazar, variablesAsignadas[i].name);

                                        }
                                    }

                                }
                                if (!controlVariableID) {
                                    TreeCore.NombreRuta(variablesAsignadas[i],
                                        {
                                            success: function (result) {
                                                if (result.Result != null && result.Result != '') {
                                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                                } else {
                                                    let atributo = {
                                                        variableId: options.variables.length,
                                                        name: App.hdRuta.value,
                                                        campoVinculadoID: variablesAsignadas[i].campoVinculadoID,
                                                        selectorCampoVinculado: false,
                                                        ruta: variablesAsignadas[i].ruta,
                                                        esDinamico: variablesAsignadas[i].esDinamico,
                                                    }
                                                    options.variables.push(atributo);


                                                    if (variablesAsignadas[i].ruta != "") {
                                                        iterador++;
                                                        let reemplazar = '[' + variablesAsignadas[i].variableId + ']';
                                                        let ruta = App.hdRuta.value;
                                                        formula = formula.replace(reemplazar, ruta);

                                                        if (i == (variablesAsignadas.length - 1)) {
                                                            pintarFormula(formula, options, datos);
                                                        }
                                                    } else {
                                                        iterador++;
                                                        let reemplazar = '[' + variablesAsignadas[i].variableId + ']';
                                                        formula = formula.replace(reemplazar, variablesAsignadas[i].name);

                                                        if (i == (variablesAsignadas.length - 1)) {
                                                            pintarFormula(formula, options, datos);
                                                        }
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
                        }
                        pintarFormula(formula, options, datos);
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

function pintarFormula(formula, options, datos) {
    document.getElementById('txtFormulas').value = formula;
    if (!expressionInput) {
        expressionInput = $('#txtFormulas').expressionBuilder(options);
    }
    else {
        expressionInput.setVariables(options.variables);
    }
    expressionInput.isValid();
    App.hdFormula.setValue('');
    formula = '';
    App.lblTituloGrid.setText(datos.Nombre);
    App.pnFormulas.show();
    App.GridRowSelect.clearSelections();
}

function Validar() {
    if (expressionInput.isValid())
        App.btnGuardarFormula.enable();
    else
        App.btnGuardarFormula.disable();
};

function Refrescar() {
    App.GridRowSelect.clearSelections();
    App.storePrincipal.reload();
}


let listaRuta = [];


function VolverAtras() {
    App.pnFormulas.hide();
    App.hdRuta.setValue('');
    App.hdFormula.setValue('');
    App.hdVariablesFormulas.setValue('');
    App.hdFormulaID.setValue('');
    formulaIDAnterior = '';
    if (listaRuta.length == 3) {
        let ElePadre = listaRuta[listaRuta.length - 2];
        listaRuta.pop();
        App.hdArbol.setValue('InventarioCategorias');
        App.storePrincipal.reload();
        App.lbcategoria.hide();
        App.lbRutaCategoria.setText(ElePadre.text);

    } else if (listaRuta.length == 2) {
        let ElePadre = listaRuta[listaRuta.length - 2];
        listaRuta.pop();
        App.hdArbol.setValue('Inventario');
        App.storePrincipal.reload();
        App.lbcategoria.hide();
        App.lbRutaCategoria.setText(ElePadre.text);

    } else {
        IrRutaRaiz();
    }
}

function EntrarEnCarpeta() {
    if (App.GridRowSelect.selected.items[0] != undefined) {
        let seleccionado = App.GridRowSelect.selected.items[0].data;
        App.hdObjetoNegocioTipoID.setValue(seleccionado.Id);
        App.hdTablaModeloDatoID.setValue(seleccionado.TablaModeloDatoID);


        if (seleccionado.Tipo == 'FORMULA') {
            return;
        } else if (seleccionado.ObjetoTipoID != null) {
            App.hdCategoriaID.setValue(seleccionado.ObjetoTipoID);
            App.btRaiz.show();
            App.lbRaiz.show();
            App.lbcategoria.show();
            App.lbRaiz.setText(seleccionado.Nombre);
            listaRuta.push(seleccionado);
        }
        else {
            App.btRaiz.show();
            App.lbRaiz.show();
            listaRuta.push(seleccionado);
        }
        App.storePrincipal.reload();

    }
}
function IrRutaRaiz() {
    LimpiarRuta();
    App.storePrincipal.reload();
}

function LimpiarRuta() {
    App.btnNext.hide();
    App.lbcategoria.hide();
    App.btnCarpetaActual.hide();
    App.lbRutaCategoria.hide();
    App.btRaiz.hide();
    App.lbRaiz.hide();
    listaRuta = [];
    App.pnFormulas.hide();
    App.hdArbol.setValue('');

    App.hdObjetoNegocioTipoID.setValue('')
    App.hdTablaModeloDatoID.setValue('')

}


// #endregion


// #region FORM GESTION
function AgregarEditar() {
    VaciarFormulario();
    App.winGestion.setTitle(jsAgregar + ' ' + jsFormulas);
    Agregar = true;
    App.winGestion.show();
}

function winGestionBotonGuardar() {
    if (App.formGestion.getForm().isValid()) {
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
                    App.winGestion.hide();
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



function VaciarFormulario() {

    Ext.each(App.winGestion.body.query('*'), function (value) {
        Ext.each(value, function (item) {
            let c = Ext.getCmp(item.id);
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

                if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                }
            }
        });
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


// #region ELIMINAR


function Eliminar() {
    Ext.Msg.alert(
        {
            title: jsEliminar,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxEliminar,
            icon: Ext.MessageBox.QUESTION
        });

}

function ajaxEliminar(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.Eliminar({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    forzarCargaBuscadorPredictivo = true;
                    App.storePrincipal.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

// #endregion

// #endregion

// #endregion

// #region Formula

function ocultarPanel() {
    let panel = App.pnFormulas;
    if (panel.hidden == false) {
        panel.hide();
    } else {
        panel.show();
    }
}

function Guardar() {
    let formulaExpression = expressionInput.getExpression();
    let Variables = expressionInput.getOptions().variables;
    let indices = extraerIndices(formulaExpression);
    let VariablesAsignadas = [];
    for (let i = 0; i < indices.length; i++) {
        VariablesAsignadas.push(Variables[indices[i]]);
    }
    TreeCore.GuardarFormula(formulaExpression, VariablesAsignadas,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    //expressionInput.verVariables(formula,formula.length);
                    App.hdFormula.setValue('');

                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}



function extraerIndices(cadena) {
    let indices = [];
    for (let i = 0; i < cadena.length; i++) {
        control = false;
        if (cadena[i] == '[') {
            for (let j = i; j < cadena.length; j++) {
                if (cadena[j] == ']') {
                    let indice = cadena.slice(i + 1, j);
                    indices.push(indice);
                    break;
                }
            }
        }

    }
    return indices;
}

// #endregion

// #region WinSelectCampoVinculado
function RendererIconLinkedField(value) {
    if (value == "TablaModeloDato") {
        return '<span class="ico-folder-grid-gr">&nbsp;</span>';
    }
    else if (value == "InventarioCategoria") {
        return '<span class="ico-folder-grid">&nbsp;</span>';
    }
    else {
        return '<span>&nbsp;</span>';
    }
}

function LinkedGridDoubleClick(sender, registro, index) {
    let data = registro.data;


    if (data.DataType == "TablaModeloDato" || data.DataType == "InventarioCategoria") {
        if (data.DataType == "InventarioCategoria") {
            App.hdCampoVinculadoCategoria.setValue(data.TypeDynamicID);
            App.hdCampoVinculadoTipo.setValue("DINAMICO");
        }
        else {
            App.hdCampoVinculadoTipo.setValue("ESTATICO");
        }
        let ruta = App.hdCampoVinculadoRuta.getValue();
        if (ruta != "") {
            ruta = JSON.parse(ruta);
        }
        else {
            ruta = {
                path: []
            };
        }
        let idTb = data.TypeDynamicID;
        //ruta += `${idTb}/`;
        let uID = GenerarID();
        ruta.path.push({
            id: idTb,
            tipo: App.hdCampoVinculadoTipo.value,
            uID: uID
        });
        console.log(ruta);
        App.hdCampoVinculadoRuta.setValue(JSON.stringify(ruta));
        App.Label12.show();
        App.Button3.show();
        App.Label12.setText(data.Name);
        if (!listaRuta.some(r => r.TypeDynamicID == data.TypeDynamicID)) {
            listaRuta.push({ Name: data.Name, TypeDynamicID: data.TypeDynamicID, idUnico: uID });
            App.storeSelectCamposVinculados.reload();
        }
        App.menuRuta.items.clear();
        GenerarRutaCampoVinculado();
        App.storeSelectCamposVinculados.reload();
    }

    else {
        console.log("Guardar la columnaseleccionada", data.TypeDynamicID)
        data.Name = data.Name.replace(" ", "").replace("(", "").replace(")", "");

        let ruta = "";
        if (App.hdCampoVinculadoRuta.value != "") {
            ruta = JSON.parse(App.hdCampoVinculadoRuta.value);
        }

        let variableId = expressionInput.getOptions().variables.length;
        let campoVinculadoVentana = {
            variableId: variableId,
            name: data.Name,
            campoVinculadoID: data.TypeDynamicID,
            selectorCampoVinculado: false,
            ruta: ruta,
            esDinamico: data.Dynamic,
        }
        TreeCore.NombreRuta(campoVinculadoVentana,
            {
                success: function (result) {
                    if (!result.Success) {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {

                        campoVinculadoVentana.name = App.hdRuta.value;
                        let variables = expressionInput.getOptions().variables;
                        variables.push(campoVinculadoVentana);
                        expressionInput.setVariables(variables);

                        let opciones = expressionInput.getOptionsHTML();
                        let start = parseInt($(opciones).attr('data-start'));
                        let current = parseInt($(opciones).attr('data-current'))
                        expressionInput.marcarCampo(campoVinculadoVentana, start, current);
                    }
                }
            });




        App.winSelectCampoVinculado.hide();
    }
}

function limpiarCampoVinculadoBuscador() {

}

function FiltrarColumnasCampoVinculadoBuscador() {

}

function gridCamposVinculados_RowSelect() {

}

function openWinSelectCampoVinculado(ruta, categoria) {
    App.hdCampoVinculadoRuta.setValue(ruta);
    App.hdCampoVinculadoCategoriaOriginal.setValue(categoria);
    App.hdCampoVinculadoCategoria.setValue(categoria);
    App.hdCampoVinculadoTipo.setValue(null);

    App.winSelectCampoVinculado.show();
    IrRutaRaizCampoVinculado();
}

function closeWinSelectCampoVinculado() {
    TreeCore.GetModeloPlantilla({
        success: function (result) {
            if (!result.Success) {
                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                limpiarConfiguracionPlantilla(false);
                tempModelo = result.Result;

                printTableModel(tempModelo);
            }
        }
    });
}


let listaRutaCampoVinculado = [];

function VolverAtrasCampoVinculado() {
    if (listaRutaCampoVinculado.length >= 2) {
        let ElePadre = listaRuta[listaRuta.length - 2];
        listaRuta.pop();
        App.storeSelectCamposVinculados.reload();
        App.Label12.show();
        App.Label12.setText(ElePadre.Name);
        App.menuRuta.items.clear();
        GenerarRutaCampoVinculado();
    } else {
        IrRutaRaizCampoVinculado();
    }
}

function GenerarID() {
    return '_' + Math.random().toString(36).substr(2, 9);
}

function IrRutaRaizCampoVinculado() {
    LimpiarRutaCampoVinculado();
    App.hdCampoVinculadoTipo.setValue(null);
    App.hdCampoVinculadoCategoria.setValue(App.hdCampoVinculadoCategoriaOriginal.value);
    App.hdCampoVinculadoRuta.setValue([]);
    App.btnPadreCarpetaActucal.hide();
    App.storeSelectCamposVinculados.reload();

}

function SeleccionarRutaCampoVinculado(sender, select) {
    forzarCargaBuscadorPredictivo = true;

    App.Label12.show();
    App.btnCarpetaActual.show();
    App.Label12.setText(select.text);
    for (let i = 0; i < listaRuta.length; i++) {
        if (listaRuta[i].idUnico == select.IDUnico) {
            let ruta = JSON.parse(App.hdCampoVinculadoRuta.value);
            for (let z = ruta.path.length - 1; z >= 0; z--) {
                console.log(z);
                if (ruta.path[z].uID == listaRuta[i].idUnico) {
                    App.hdCampoVinculadoCategoria.setValue(ruta.path[z].id);
                    break;
                }
                else {
                    ruta.path.pop();
                }
            }
            listaRuta = listaRuta.splice(0, ++i);
            App.hdCampoVinculadoRuta.setValue(JSON.stringify(ruta));
        }
    }
    App.storeSelectCamposVinculados.reload();
    App.menuRuta.items.clear();
    GenerarRutaCampoVinculado();
}

function SeleccionarPadreCampoVinculado(sender, select) {
    LimpiarRutaCampoVinculado();
    App.storeSelectCamposVinculados.reload();
    App.Label12.show();
    App.btnCarpetaActual.show();
    App.Label12.setText(select.text);
}

function LimpiarRutaCampoVinculado() {
    forzarCargaBuscadorPredictivo = true;
    App.btnMenuRuta.hide();
    App.btnRaizCarpeta.hide();
    App.lbRutaCategoria.hide();
    App.btnCarpetaActual.hide();
    App.Button3.hide();
    App.Label12.setText('');
    App.menuRuta.items.clear();
    listaRutaCampoVinculado = [];

}

function GenerarRutaCampoVinculado() {
    App.btnMenuRuta.show();
    App.btnRaizCarpeta.show();
    try {
        document.getElementById('menuRuta-targetEl').innerHTML = '';
    } catch (e) {

    }
    for (let item in listaRuta) {
        App.menuRuta.add(new Ext.menu.TextItem({ text: listaRuta[item].Name, TypeDynamicID: listaRuta[item].TypeDynamicID, IDUnico: listaRuta[item].idUnico }))
    }
    if (App.menuRuta.items.items.length > 1) {
        App.menuRuta.items.last().hide();
    } else {
        App.btnMenuRuta.hide();
        App.btnRaizCarpeta.hide();
    }
}

// #endregion