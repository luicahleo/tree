
var codeStatus;

//#region GESTION

function GetWFObject() {
    return JSON.parse(App.hdObjeto.value);
}

function SaveWFObject(object) {
    App.hdObjeto.setValue(JSON.stringify(object));
}

function WorkFlowValid(sender) {
    let btnSave = App.btnAddWF;
    btnSave.disable();
    let valid = true;

    try {
        App.pnConfWorkFlow.events.hide.listeners[0].fn();
    } catch (e) {

    }

    let oWF = GetWFObject();

    if (oWF.Code == undefined || oWF.Code == '') {
        valid = false;
        return;
    }
    if (oWF.Name == undefined || oWF.Name == '') {
        valid = false;
        return;
    }
    if (oWF.Description == undefined || oWF.Description == '') {
        valid = false;
        return;
    }

    if (valid) {
        btnSave.enable();
    }
}

function VaciarFormulario() {
    Ext.each(App.pnStatus.body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c != undefined && c.isFormField) {
            c.reset();
        }
    });
    ShowConfStatus();
}

function AddStatus() {
    try {
        App.pnConfRoles.events.hide.listeners[0].fn();
    } catch (e) {

    }
    TreeCore.UploadWF(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Toast.create(jsInfo, jsGuardado, TOAST_STATUS.SUCCESS, 3000);
                    EditStatus();
                    ShowCardsStatus();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function InitialStatus(sender) {
    codeStatus = sender.parentElement.parentElement.getAttribute("code");
    App.hdWFStatusCode.setValue(codeStatus);

    let WFObject = GetWFObject();
    let status = WFObject.LinkedStatus.filter(x => x.Code == codeStatus)[0];

    WFObject.LinkedStatus.forEach(x => x.Default = false);

    status.Default = true;

    let index = WFObject.LinkedStatus.findIndex(x => x.Code == codeStatus)[0];
    WFObject.LinkedStatus[index] = status;

    SaveWFObject(WFObject);

    TreeCore.UploadWF(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Toast.create(jsInfo, jsGuardado, TOAST_STATUS.SUCCESS, 3000);
                    ShowCardsStatus();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function ActivarStatus(code) {
    codeStatus = code.value;

    let WFObject = GetWFObject();
    let status = WFObject.LinkedStatus.filter(x => x.Code == codeStatus)[0];

    status.Active = !status.Active;

    let index = WFObject.LinkedStatus.findIndex(x => x.Code == codeStatus)[0];
    WFObject.LinkedStatus[index] = status;

    SaveWFObject(WFObject);

    TreeCore.UploadWF(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Toast.create(jsInfo, jsGuardado, TOAST_STATUS.SUCCESS, 3000);
                    ShowCardsStatus();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function DeleteStatus(code) {
    codeStatus = code.value;
    Ext.Msg.alert(
        {
            title: jsEliminar,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxDeleteStatus,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxDeleteStatus(button) {
    if (button == 'yes' || button == 'si') {

        let WFObject = GetWFObject();
        WFObject.LinkedStatus = WFObject.LinkedStatus.filter(x => x.Code != codeStatus);

        SaveWFObject(WFObject);

        TreeCore.UploadWF(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        Toast.create(jsInfo, jsGuardado, TOAST_STATUS.SUCCESS, 3000);
                        ShowCardsStatus();
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

//#endregion

//#region Panel WorkFlow

//#region ConfWorkFlow

var tagsRolesWF = null;
function cargaTagsWF(datos) {
    let roles = [];
    for (let i = 0; i < datos.length; i++) {
        roles[i] = { 'tag': datos[i].data.Name, 'value': datos[i].data.Code };
    }
    if (tagsRolesWF != null) {
        tagsRolesWF.destroy();
    }

    tagsRolesWF = new AmsifySuggestags($('input[name="inputRolesWF"]'));

    tagsRolesWF._setDefaultLabel(jsRoles);

    tagsRolesWF._settings({
        suggestions: roles,
        whiteList: true,
        showAllSuggestions: true
    })
    document.getElementsByName('inputRolesWF')[0].value = '';
    tagsRolesWF._init();

    let WFObject = GetWFObject();

    WFObject.LinkedRoles.forEach(x => {
        tagsRolesWF.addTag(x);
    });
}

function ShowConfWorkFlow() {
    if (App.storeRoles.data.items.length > 0) {
        let WFObject = GetWFObject();
        App.txtNameWF.setValue(WFObject.Name);
        App.txtCodeWF.setValue(WFObject.Code);
        App.txtDescriptionWF.setValue(WFObject.Description);
        if (WFObject.Public) {
            App.btnPublicWF.click();
        } else {
            App.btnPrivateWF.click();
        };
        cargaTagsWF(App.storeRoles.data.items);
    } else {
        let WFObject = GetWFObject();
        App.txtNameWF.setValue(WFObject.Name);
        App.txtCodeWF.setValue(WFObject.Code);
        App.txtDescriptionWF.setValue(WFObject.Description);
        if (WFObject.Public) {
            App.btnPublicWF.click();
        } else {
            App.btnPrivateWF.click();
        };
        CargarStoresSerie([App.storeRoles], function Fin(fin) {
            if (fin) {
                let WFObject = GetWFObject();
                cargaTagsWF(App.storeRoles.data.items);
            }
        });
    }

}

function HideConfWorkFlow() {
    let WFObject = GetWFObject();
    WFObject.Name = App.txtNameWF.value;
    WFObject.Code = App.txtCodeWF.value;
    WFObject.Description = App.txtDescriptionWF.value;
    WFObject.Public = App.btnPublicWF.pressed;
    let Roles = [];
    document.getElementsByName('inputRolesWF')[0].value.split(',').forEach(x => {
        if (x != '') {
            Roles.push(x);
        }
    })
    //for (var i in document.getElementsByName('inputRolesWF')[0].value.split(',')) {
    //    Roles.push(i);
    //}
    WFObject.LinkedRoles = Roles;
    SaveWFObject(WFObject);
}

function ControlRolWF(sender) {
    App.btnPublicWF.setPressed(false);
    App.btnPrivateWF.setPressed(false);
    sender.setPressed(true);
    if (sender.id == "btnPublicWF") {
        App.lblPrivateRole.hide();
        App.lblPublicRole.show();
    }
    else {
        App.lblPublicRole.hide();
        App.lblPrivateRole.show();
    }
}

function ApplyRolWF(sender) {
    App.pnConfWorkFlow.hide();
    App.pnSumWorkFlow.show();
    ShowSubmitWF();
}

//#endregion

//#region SumWorkFlow

function ShowSumWorkFlow(sender) {
    let oWF = GetWFObject();
    App.lbTitleWF.setText(oWF.Name);
    App.lbDescriptionWF.setText(oWF.Description);
    let divContainer = '';
    divContainer += CreateCard(oWF);
    document.getElementById('dtvTarjeta').innerHTML = divContainer;
}

function HideSumWorkFlow(sender) {

}

function ShowEditWF() {
    App.tblEditWF.show();
    App.tblSubmitWF.hide();

}

function ShowSubmitWF() {
    App.tblEditWF.hide();
    App.tblSubmitWF.show();
}

function SubmitWF() {
    TreeCore.UploadWF(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Toast.create(jsInfo, jsGuardado, TOAST_STATUS.SUCCESS, 3000);
                    ShowEditWF();
                    App.pnStatus.expand();
                    App.pnConfStatus.show();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function EditWF() {
    App.pnConfWorkFlow.show();
    App.pnSumWorkFlow.hide();
}

//#endregion

//#endregion

//#region Panel Diagrama

function ShowCardsStatus() {
    let divContainer = '';
    let WFObject = GetWFObject();
    divContainer += NewCardsStatus();
    if (WFObject.LinkedStatus && WFObject.LinkedStatus.length > 0) {
        WFObject.LinkedStatus.forEach(x => {
            divContainer += CardsStatus(x);
        })
    }
    document.getElementById('dtvTarjetasStatus').innerHTML = divContainer;
}

function NewCardsStatus() {
    let html = '';

    html += '<div class="contStatusCard contStatusCard--new" onclick="VaciarFormulario()">' +
        '<div class="contAddNew">' +
        '<div class="icon-add"></div >' +
        '<div class="textAdd">' + jsNuevoStatus + '</div>' +
        '</div>' +
        '</div>';

    return html;
}

function CardsStatus(status) {
    let hours;
    let time;
    let stringTime;

    if (status.TimeFrame) {
        hours = status.TimeFrame;
        time = Math.trunc(hours / 24);
        if (Math.trunc(time / 7) >= 1) {
            stringTime = Math.trunc(time / 7) + jsSemanas.substring(0, 1);

            if (time % 7 > 0) {
                stringTime += ', ' + time % 7 + jsDias.substring(0, 1);

                if (hours % 24 > 0) {
                    stringTime += ', ' + hours % 24 + jsHoras.substring(0, 1);
                }
            }
        }
        else if (time >= 1) {
            stringTime = time + jsDias.substring(0, 1);

            if (hours % 24 > 0) {
                stringTime += ', ' + hours % 24 + jsHoras.substring(0, 1);
            }
        }
        else {
            stringTime = hours + jsHoras.substring(0, 1);
        }
    }

    let html = '';

    html += '<div class="contStatusCard" code="' + status.Code + '" ondblclick="ShowConfStatusCard(this)">' +
        '<div class="contHeader">' +
        '<div class="contHeader__name">' + status.Name + '</div>' +
        '<div class="contHeader__icon icoInicial" initial="' + status.Default + '" onclick="InitialStatus(this)"></div>' +
        '<div class="contHeader__icon icoAcciones hidden" onclick="ShowMenuStatus(this)"></div>' +
        '<div class="contHeader__duration">' + stringTime + '</div>' +
        '</div>' +
        '<div class="contdata">' +
        '<ul>';
    if (status.LinkedWorkFlowNextStatus != null && status.LinkedWorkFlowNextStatus.length > 0) {
        for (var i = 0; i < 3 && i < status.LinkedWorkFlowNextStatus.length; i++) {
            html += '<li>' + status.LinkedWorkFlowNextStatus[i].WorkFlowStatusCode + '</li>';
        }
    }
    html += '</ul>' +
        '<div>';
    if (status.LinkedRolesReading != null && status.LinkedRolesReading.length > 0) {
        for (var i = 0; i < 2 && i < status.LinkedRolesReading.length; i++) {
            html += '<div class="dataCard dataCard--roles"><img class="dataCard__rol" src="/ima/ico-lectura.svg" alt="WF" />' + status.LinkedRolesReading[i] + '</div>';
        }
    }
    if (status.LinkedRolesWriting != null && status.LinkedRolesWriting.length > 0) {
        for (var i = 0; i < 2 && i < status.LinkedRolesWriting.length; i++) {
            html += '<div class="dataCard dataCard--roles"><img class="dataCard__rol" src="/ima/ico-escritura.svg" alt="WF" />' + status.LinkedRolesWriting[i] + '</div>';
        }
    }
    html += '</div>' +
        '</div>' +
        '</div>';

    return html;
}

function ShowMenuStatus(sender) {
    let StatusCode = sender.parentElement.parentElement.getAttribute("code");
    App.hdWFStatusCode.setValue(StatusCode);
    var menu = App.mnOpciones;
    menu.showAt([sender.getBoundingClientRect().x, sender.getBoundingClientRect().y + 20]);
}

//#endregion

//#region Panel Status

//#region ConfStatus

function ShowConfStatusCard(sender) {
    let codigo;
    if (sender) {
        codigo = sender.getAttribute('code');
        App.hdWFStatusCode.setValue(codigo);
    }
    ShowConfStatus(codigo);
}

function ShowConfStatus(code) {
    if (!App.storeRoles.data.items.length > 0) {
        CargarStoresSerie([App.storeStatusGroup], function Fin(fin) {
        });
    }

    let WFObject = GetWFObject();
    let status = {};
    codeStatus = code;
    if (WFObject.LinkedStatus != null && WFObject.LinkedStatus.length > 0) {
        status = WFObject.LinkedStatus.filter(x => x.Code == codeStatus)[0];
    }
    if (!status) {
        status = {};
    }
    App.txtStatusName.setValue(status.Name);
    App.txtStatusCode.setValue(status.Code);
    App.cmbStatusGroup.setValue(status.StatusGroupCode);
    App.txtStatusDescription.setValue(status.Description);

    App.cmbTime.setValue("horas");
    App.txtTimeDuration.setValue(status.TimeFrame);

    App.pnStatus.expand();
    App.pnConfStatus.show();
}

function HideConfStatus(sender) {
    let WFObject = GetWFObject();
    let status = {};
    if (WFObject.LinkedStatus.length > 0) {
        status = WFObject.LinkedStatus.filter(x => x.Code == App.hdWFStatusCode.value)[0];
    }
    if (!status) {
        status = {};
    }
    status.Name = App.txtStatusName.value;
    status.Code = App.txtStatusCode.value;
    status.StatusGroupCode = App.cmbStatusGroup.value;
    status.Description = App.txtStatusDescription.value;
    let time = App.txtTimeDuration.value;
    switch (App.cmbTime.value) {
        case "semanas":
            status.TimeFrame = time * 7 * 24;
            break;
        case "dias":
            status.TimeFrame = time * 24;
            break;
        default:
            status.TimeFrame = time;
            break;
    }

    App.hdWFStatusCode.setValue(App.txtStatusCode.value);

    if (WFObject.LinkedStatus.length == 0 || !WFObject.LinkedStatus.filter(x => x.Code == App.hdWFStatusCode.value).length) {
        WFObject.LinkedStatus.push(status);
    } else {
        let index = WFObject.LinkedStatus.findIndex(x => x.Code == App.hdWFStatusCode.value)[0];
        WFObject.LinkedStatus[index] = status;
    }

    SaveWFObject(WFObject);
}

function EditRoles() {
    App.pnConfStatus.hide();
    App.pnConfRoles.show();
}

//#endregion

//#region pnConfStatus

function GetRolesStatus() {
    let WFObject = GetWFObject();

    if (WFObject.Public) {
        return App.storeRoles.data.items.filter(x => !WFObject.LinkedRoles.includes(x.data.Code));
    }
    else {
        return App.storeRoles.data.items.filter(x => WFObject.LinkedRoles.includes(x.data.Code));
    }
}

function ShowConfRoles(sender) {
    let WFObject = GetWFObject();
    let status = {};
    var codigo = App.hdWFStatusCode.value;

    if (WFObject.LinkedStatus != null && WFObject.LinkedStatus.length > 0) {
        status = WFObject.LinkedStatus.filter(x => x.Code == codigo)[0];
    }
    if (!status) {
        status = {};
    }

    if (App.storeRoles.data.items.length > 0) {

        if (status.PublicReading) {
            App.btnPublicRead.click();
        } else {
            App.btnPrivateRead.click();
        };
        if (status.PublicWriting) {
            App.btnPublicWrite.click();
        } else {
            App.btnPrivateWrite.click();
        };

        let listaRoles = GetRolesStatus();

        cargaTagsStatusRead(listaRoles);
        cargaTagsStatusEdit(listaRoles);
    } else {

        if (status.PublicReading) {
            App.btnPublicRead.click();
        } else {
            App.btnPrivateRead.click();
        };
        if (status.PublicWriting) {
            App.btnPublicWrite.click();
        } else {
            App.btnPrivateWrite.click();
        };

        CargarStoresSerie([App.storeRoles], function Fin(fin) {
            if (fin) {
                let WFObject = GetWFObject();
                let listaRoles = GetRolesStatus();
                cargaTagsStatusRead(listaRoles);
                cargaTagsStatusEdit(listaRoles);
            }
        });
    }
}

function HideConfRoles(sender) {
    let WFObject = GetWFObject();
    let status = {};
    if (WFObject.LinkedStatus.length > 0) {
        status = WFObject.LinkedStatus.filter(x => x.Code == App.hdWFStatusCode.value)[0];
    }
    if (!status) {
        status = {};
    }

    status.PublicReading = App.btnPublicRead.pressed;
    let RolesR = [];
    document.getElementsByName('inputRolesWFRead')[0].value.split(',').forEach(x => {
        if (x != '') {
            RolesR.push(x);
        }
    });

    status.PublicWriting = App.btnPublicWrite.pressed;
    let RolesW = [];
    document.getElementsByName('inputRolesWFWrite')[0].value.split(',').forEach(x => {
        if (x != '') {
            RolesW.push(x);
        }
    });

    status.LinkedRolesReading = RolesR;
    status.LinkedRolesWriting = RolesW;

    if (WFObject.LinkedStatus.length == 0 || !WFObject.LinkedStatus.filter(x => x.Code == App.hdWFStatusCode.value).length) {
        WFObject.LinkedStatus.push(status);
    } else {
        let index = WFObject.LinkedStatus.findIndex(x => x.Code == App.hdWFStatusCode.value)[0];
        WFObject.LinkedStatus[index] = status;
    }

    SaveWFObject(WFObject);
}

function EditStatus() {
    App.pnConfRoles.hide();
    App.pnConfStatus.show();
}

function ControlRolWFRead(sender) {
    App.btnPublicRead.setPressed(false);
    App.btnPrivateRead.setPressed(false);
    sender.setPressed(true);
}

function ControlRolWFWrite(sender) {
    App.btnPublicWrite.setPressed(false);
    App.btnPrivateWrite.setPressed(false);
    sender.setPressed(true);
}

var tagsRolesRead = null;
function cargaTagsStatusRead(datos) {
    let roles = [];
    for (let i = 0; i < datos.length; i++) {
        roles[i] = { 'tag': datos[i].data.Name, 'value': datos[i].data.Code };
    }
    if (tagsRolesRead != null) {
        tagsRolesRead.destroy();
    }

    tagsRolesRead = new AmsifySuggestags($('input[name="inputRolesWFRead"]'));

    tagsRolesRead._setDefaultLabel(jsRoles);

    tagsRolesRead._settings({
        suggestions: roles,
        whiteList: true,
        showAllSuggestions: true
    })
    document.getElementsByName('inputRolesWFRead')[0].value = '';
    tagsRolesRead._init();

    let WFObject = GetWFObject();
    let status = {};
    var codigo = App.hdWFStatusCode.value;

    if (WFObject.LinkedStatus != null && WFObject.LinkedStatus.length > 0) {
        status = WFObject.LinkedStatus.filter(x => x.Code == codigo)[0];
    }
    if (!status) {
        status = {};
    }

    if (status.LinkedRolesReading != null)
        status.LinkedRolesReading.forEach(x => {
            tagsRolesRead.addTag(x);
        });
}

var tagsRolesEdit = null;
function cargaTagsStatusEdit(datos) {
    let roles = [];
    for (let i = 0; i < datos.length; i++) {
        roles[i] = { 'tag': datos[i].data.Name, 'value': datos[i].data.Code };
    }
    if (tagsRolesEdit != null) {
        tagsRolesEdit.destroy();
    }

    tagsRolesEdit = new AmsifySuggestags($('input[name="inputRolesWFWrite"]'));

    tagsRolesEdit._setDefaultLabel(jsRoles);

    tagsRolesEdit._settings({
        suggestions: roles,
        whiteList: true,
        showAllSuggestions: true
    })
    document.getElementsByName('inputRolesWFWrite')[0].value = '';
    tagsRolesEdit._init();

    let WFObject = GetWFObject();
    let status = {};
    var codigo = App.hdWFStatusCode.value;

    if (WFObject.LinkedStatus != null && WFObject.LinkedStatus.length > 0) {
        status = WFObject.LinkedStatus.filter(x => x.Code == codigo)[0];
    }
    if (!status) {
        status = {};
    }

    if (status.LinkedRolesWriting != null)
        status.LinkedRolesWriting.forEach(x => {
            tagsRolesEdit.addTag(x);
        });
}

//#endregion

//#endregion