//#region DIRECT METHOD

var arrayWorkFlowsAsignadosBusiness = [];
var contador = 0;

function btnAgregar() {

    App.winGestionBusinessProcess.setTitle(jsAgregar + " " + 'Business Process');
    arrayWorkFlowsAsignadosBusiness = [];
    App.cmbWorkflow.DataSource = null;
    Agregar = true;
    VaciarFormulario();
    App.storeWorkFlows.reload();
    App.storeBusinessProcessTipos.reload();
    cargarComboWorkFlows();
    App.cmbStatus.disable();
    App.cmbWorkflow.disable();
    App.winGestionBusinessProcess.show();
}

function VerActivos() {
    if (App.colActivo.hidden) {
        App.colActivo.show();
    } else {
        App.colActivo.hide();
    }
    //Refrescar();
}

function ajaxEditar() {
    App.storeBusinessProcessWorkFlowsAdd.reload();
    VaciarFormulario();

    for (var i = 0; i < App.cmbWorkflow.triggerCell.elements.length; i++) {
        App.cmbWorkflow.removeByIndex(i);
    }

    Agregar = false;

    App.storeWorkFlows.reload();


    App.cmbWorkflow.enable();
    App.cmbStatus.enable();

    showLoadMask(App.winGestionBusinessProcess, function (load) {
        recargarCombos([App.cmbType], function Fin(fin) {
            if (fin) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.show({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            }
                            else {

                                cargarComboWorkFlows();
                                App.winGestionBusinessProcess.setTitle(jsEditar + " " + 'Business Process');
                                App.winGestionBusinessProcess.show();

                                App.txtName.focus();
                            }
                            App.storeWorkFlowsEstados.reload();
                            load.hide();
                        }
                    });
            }
        });

    });
}

function VaciarFormulario() {
    App.FormBusiness.getForm().reset();

    arrayWorkFlowsAsignadosBusiness = [];
    App.cmbWorkflow.DataSource = null;

    Ext.each(App.FormBusiness.body.query('*'), function (value) {
        Ext.each(value, function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && c.isFormField) {
                c.reset();

                if (c.triggerWrap != undefined) {
                    c.triggerWrap.removeCls("itemForm-novalid");
                }

                if (!c.allowBlank && c.xtype != "checkboxfield") {
                    if (!c.cls.includes('itemForm-BusinessMandatory')) {
                        c.addListener("change", anadirClsNoValido, false);
                        c.addListener("focusleave", anadirClsNoValido, false);
                        c.addListener("change", FormularioValido, false);

                        c.addListener("change", cambiarLiteral, false);

                        c.removeCls("ico-exclamacion-10px-red");
                        c.addCls("ico-exclamacion-10px-grey");
                    }
                    else {
                        c.addCls("ico-exclamacion-10px-grey");
                    }
                }

                if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                }
            }
        });
    });

    //let longitud = App.cmbWorkflow.getRefItems().at().all.elements.length;

    for (i = 0; i < contador; i++) {
        App.cmbWorkflow.removeByIndex(0);
    }
}

function FormularioValido(sender, valid) {

    if (valid != null) {
        App.btnAgregar.setDisabled(false);

        Ext.each(App.FormBusiness.body.query('*'), function (item) {
            var c = Ext.getCmp(item.id);
            if (c && c.isFormField && (!c.isValid() || (!c.allowBlank && (c.xtype == "combobox" || c.xtype == "multicombo")
                && (c.selection == null || c.rawValue != c.selection.data[c.displayField])))) {
                App.btnAgregar.setDisabled(true);
            }
        });

    }
    else {
        App.btnAgregar.setDisabled(true);
    }

}

function cambiarLiteral() {
    App.btnAgregar.setText(jsGuardar);
    App.btnAgregar.setIconCls("");
    App.btnAgregar.removeCls("btnDisableClick");
    App.btnAgregar.addCls("btnEnableClick");
    App.btnAgregar.removeCls("animation-text");
}

function cerrarWinGestion() {
    App.storePrincipal.reload();
    App.GridRowSelectBusinessProcess.clearSelections();
    App.winGestionBusinessProcess.hide();
    arrayWorkFlowsAsignadosBusiness = [];
    App.cmbWorkflow.DataSource = null;
}

function guardarCambios() {

    TreeCore.AgregarEditar(Agregar, arrayWorkFlowsAsignadosBusiness,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsEstado, icon: Ext.MessageBox.INFO, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.storePrincipal.reload();
                    App.cmbWorkflow.DataSource = null;
                    arrayWorkFlowsAsignadosBusiness = [];
                    App.winGestionBusinessProcess.hide();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEditar.enable();
        App.btnEliminar.enable();
        App.btnActivar.enable();
        App.btnPlay.enable();

        App.btnEditar.setTooltip(jsEditar);
        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
        App.btnRefrescar.setTooltip(jsRefrescar);
        App.btnDescargar.setTooltip(jsDescargar);

        //let registroSeleccionado = registro;
        //App.hdEstadoID.setValue(seleccionado.CoreEstadoID);
        //parent.App.hdEstadoPadreID.setValue(seleccionado.CoreEstadoID);

        App.hdBusinessProcess.setValue(seleccionado.CoreBusinessProcessID);

        App.storeBusinessProcessWorkFlowsAdd.reload();
        App.storeWorkflowBP.reload();

        if (seleccionado.Activo) {
            App.btnActivar.setTooltip(jsDesactivar);
        }
        else {
            App.btnActivar.setTooltip(jsActivar);
        }
    }
}

function DeseleccionarGrilla() {

    if (App.hdCliID.value == 0 || App.hdCliID.value == undefined) {
        App.btnAnadir.disable();
    } else {
        App.btnAnadir.enable();
    }

    App.GridRowSelectBusinessProcess.clearSelections();
    App.hdBusinessProcess.setValue("");
    App.storeWorkflowBP.reload();
    App.Panel1.hide();
    App.Panel2.hide();
    App.btnEditar.disable();
    App.btnEliminar.disable();
    App.btnActivar.disable();
    App.btnPlay.disable();
}

function Eliminar() {
    Ext.Msg.alert(
        {
            title: jsEliminar + ' ' + 'Business Process',
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
                    App.storePrincipal.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function Refrescar() {
    App.storePrincipal.reload();
    App.GridRowSelectBusinessProcess.clearSelections();
    App.hdBusinessProcess.setValue("");
    App.storeWorkflowBP.reload();
    App.Panel1.hide();
    App.Panel2.hide();
}

//#region COMBOS

function SeleccionarWorkFlow() {
    App.cmbStatus.enable();
    App.cmbStatus.clearValue();
    App.cmbWorkflow.getTrigger(0).show();
    App.storeWorkFlowsEstados.reload();
    if (App.cmbWorkflow.selection != null) {
        App.cmbWorkflow.triggerWrap.removeCls("itemForm-novalid");
        App.cmbWorkflow.triggerWrap.addCls("itemForm-valid");

        App.cmbWorkflow.removeCls("ico-exclamacion-10px-red");
        App.cmbWorkflow.addCls("ico-exclamacion-10px-grey");
    }
}

function RecargarWorkflow() {
    App.cmbWorkflow.clearValue();
    App.cmbStatus.clearValue();
    App.cmbStatus.disable();
    App.cmbWorkflow.getTrigger(0).hide();

    App.cmbWorkflow.triggerWrap.removeCls("itemForm-valid");
    App.cmbWorkflow.triggerWrap.addCls("itemForm-novalid");

    App.cmbWorkflow.removeCls("ico-exclamacion-10px-grey");
    App.cmbWorkflow.addCls("ico-exclamacion-10px-red");
}


function SeleccionarWorkFlowEstados() {
    App.cmbStatus.getTrigger(0).show();
}

function RecargarWorkflowEstados() {
    App.cmbStatus.clearValue();
    App.cmbStatus.getTrigger(0).hide();
}


function SeleccionarTipo() {
    App.cmbType.getTrigger(0).show();
}

function RecargarTipo() {
    App.cmbType.clearValue();
    App.cmbType.getTrigger(0).hide();
}

//#endregion COMBOS

function Grid_RowSelectWF(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.lbHeaderWorkFlow.setText(seleccionado.Name);
        App.Panel1.show();
        App.Panel2.show();
    }
}

//#endregion DIRECT METHOD

// #region AMSIFY

var amsifySuggestagsWorkFlows = null;

function cargarComboWorkFlows() {

    let arrayWorkFlows = App.storeWorkFlows.data.items;
    let workflow = [];

    for (var i = 0; i < arrayWorkFlows.length; i++) {
        workflow[i] = { 'tag': arrayWorkFlows[i].data.Nombre, 'value': arrayWorkFlows[i].data.CoreWorkFlowID };
    }
    if (amsifySuggestagsWorkFlows != null) {
        amsifySuggestagsWorkFlows.destroy();
    }

    amsifySuggestagsWorkFlows = new AmsifySuggestags($('input[name="workflow"]'));

    amsifySuggestagsWorkFlows._settings({
        suggestions: workflow,
        whiteList: true,
        afterAdd: function (value) {
            arrayWorkFlowsAsignadosBusiness.push(value);
            App.cmbWorkflow.enable();
            App.cmbWorkflow.addItem(App.storeWorkFlows.getById(value).data.Nombre);
            contador++;

        },
        afterRemove: function (value) {
            arrayWorkFlowsAsignadosBusiness.pop(value);
            App.cmbWorkflow.removeByText(App.storeWorkFlows.getById(value).data.Nombre);
            contador -= 1;

        }
    });

    document.getElementsByName('workflow')[0].value = '';
    document.getElementsByName('workflow')[0].placeholder = jsAgregar + ' ' + 'WorkFlow';
    amsifySuggestagsWorkFlows._init();
    document.getElementsByClassName('amsify-suggestags-input-area')[0].style = 'max-height: 60px; overflow-y: scroll;';

    if (Agregar == false) {
        let workFlowsBusinessProcessAsignados = [];
        let workFlowsBusinessProcessAsignadosEditar = [];
        workFlowsBusinessProcessAsignadosEditar = App.storeBusinessProcessWorkFlowsAdd.data.items;
        for (var i = 0; i < workFlowsBusinessProcessAsignadosEditar.length; i++) {
            workFlowsBusinessProcessAsignados = workFlowsBusinessProcessAsignadosEditar[i].data.CoreWorkflowID;
            amsifySuggestagsWorkFlows.addTag(workFlowsBusinessProcessAsignados);
        }
    }
}

// #endregion

// #region DISEÑO

var bShowOnlySecundary = false;
var iSelectedPanel = 0;

function showPanelsByWindowSize() {

    let puntoCorte = 512;
    var tmn = App.CenterPanelMain.getWidth();

    if (tmn < puntoCorte) {
        App.tbFiltrosYSliders.show();
        App.btnPrev.show();
        App.btnNext.show();
        loadPanelByBtns("");
    }
    else {
        App.tbFiltrosYSliders.hide()
        App.btnPrev.hide();
        App.btnNext.hide();
        loadPanels();
    }
}

function loadPanels() {

    if (bShowOnlySecundary) {
        App.gridBusinessProcess.hide();
        //App.btnCloseShowVisorTreeP.setIconCls('ico-moverow-gr');
    }
    else {
        App.pnVinculaciones.show();
        App.gridBusinessProcess.show();
        App.pnInformation.show();
        App.pnRoles.show();
        //App.btnCloseShowVisorTreeP.setIconCls('ico-hide-menu');
    }
}

function loadPanelByBtns(pressedBtn) {

    // CHECK FOR A PRESSED BTN
    if (pressedBtn != "") {
        if (pressedBtn == "Next") {
            iSelectedPanel++;
        }
        else {
            iSelectedPanel--;
        }
    }

    // CHECK FOR DISABLED BUTTONS
    if (iSelectedPanel == 3) {
        App.btnPrev.enable();
        App.btnNext.disable();
    }
    else if (iSelectedPanel == 0) {
        App.btnPrev.disable();
        App.btnNext.enable();
    }
    else {
        App.btnPrev.enable();
        App.btnNext.enable();
    }

    // LOAD PANEL
    if (iSelectedPanel == 0) {
        App.gridBusinessProcess.show();
        App.pnVinculaciones.hide();
        App.pnInformation.hide();
        App.pnRoles.hide();
        //App.btnCloseShowVisorTreeP.setIconCls('ico-hide-menu');
    }
    if (iSelectedPanel == 1) {
        App.gridBusinessProcess.hide();
        App.pnVinculaciones.show();
        App.pnInformation.hide();
        App.pnRoles.hide();
        //App.btnCloseShowVisorTreeP.setIconCls('ico-moverow-gr');
    }
    if (iSelectedPanel == 2) {
        App.gridBusinessProcess.hide();
        App.pnVinculaciones.hide();
        App.pnInformation.show();
        App.pnRoles.hide();
        //App.btnCloseShowVisorTreeP.setIconCls('ico-moverow-gr');
    }
    if (iSelectedPanel == 3) {
        App.gridBusinessProcess.hide();
        App.pnVinculaciones.hide();
        App.pnInformation.hide();
        App.pnRoles.show();
        //App.btnCloseShowVisorTreeP.setIconCls('ico-moverow-gr');
    }
}

function showOnlySecundary() {
    let puntoCorte = 512;
    var tmn = App.CenterPanelMain.getWidth();

    if (tmn < puntoCorte) {
        bShowOnlySecundary = false;
        loadPanelByBtns('Prev')
    }
    else {
        bShowOnlySecundary = !bShowOnlySecundary;
        loadPanels();
    }
}

function winFormCenterSimple(obj) {
    obj.center();
    obj.updateLayout();
}

function winFormResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(650);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(650);
    }

    if (res <= 670) {
        obj.setWidth(380);
    }

    obj.center();
}

function formResize(obj) {
    var res = window.innerWidth;

    if (res > 1024) {
        obj.setWidth(650);
    }

    if (res <= 1024 && res > 670) {
        obj.setWidth(620);
    }

    if (res <= 670) {
        obj.setWidth(350);
    }
}

window.addEventListener('resize', function () {
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

});

// #region RENDERS

var InitialRender = function (value) {
    if (value == "1" || value == "true") {
        return '<span class="ico-positionGrid">&nbsp;</span>'
    }
    else {
        return '<span></span>'
    }
}

// #endregion

function NavegacionTabsBPorW(sender) {
    var senderid = sender.id;
    tabToUpdate = senderid;

    if (senderid == 'btnBusinessProcess') {
        App.gridBusinessProcess.show();
        App.grdWorkFlowBP.hide();
        App.btnEditWF.hide();
        App.btnBusinessProcess.addCls('boldPath');
        App.btnWorkflow.removeCls('boldPath')
    }
    else if (senderid == 'btnWorkflow') {
        App.gridBusinessProcess.hide();
        App.grdWorkFlowBP.show();
        App.btnEditWF.show();
        App.btnBusinessProcess.removeCls('boldPath')
        App.btnWorkflow.addCls('boldPath');
    }
    else {
    }
}

function NavegacionTabsWBP(sender) {
    var senderid = sender.id;
    tabToUpdate = senderid;

    document.getElementById('lnkInformation').classList.remove("navActivo");
    document.getElementById('lnkInformation').childNodes[1].classList.remove("navActivo");
    document.getElementById('lnkRoles').classList.remove("navActivo");
    document.getElementById('lnkRoles').childNodes[1].classList.remove("navActivo");

    if (senderid == 'lnkInformation') {
        document.getElementById('lnkInformation').classList.add("navActivo");
        document.getElementById('lnkInformation').childNodes[1].classList.add("navActivo");
        App.ctMain1BP.show();
        App.ctMain2BP.hide();
    }
    else if (senderid == 'lnkRoles') {
        document.getElementById('lnkRoles').classList.add("navActivo");
        document.getElementById('lnkRoles').childNodes[1].classList.add("navActivo");
        App.ctMain1BP.hide();
        App.ctMain2BP.show();
    }
    else {
    }
}

function clickOnWorkFlowInitial(sender) {
    var btn = sender.parentElement.parentElement.parentElement;

    if (btn.classList != 'contBusinessProcess contBusinessProcessActive') {
        btn.classList.add('contBusinessProcessActive');
    }
    else {
        btn.classList.remove('contBusinessProcessActive');
    }
}

function clickBTNWorkFlowMenu(sender) {
    var btn = sender.children[0];

    if (btn.classList != 'ulActive') {
        btn.classList.add('ulActive');
    }
    else {
        btn.classList.remove('ulActive');
    }
}

function clickOnWorkFlowInfo(sender) {
    var btn = sender.parentElement.parentElement;

    if (btn.classList != 'contBusinessProcessInfo contBusinessProcessInfoActive') {
        btn.classList.add('contBusinessProcessInfoActive');
    }
    else {
        btn.classList.remove('contBusinessProcessInfoActive');
    }
}

// #endregion