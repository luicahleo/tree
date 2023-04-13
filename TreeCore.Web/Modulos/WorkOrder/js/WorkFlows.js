//#region CARDS

function ShowCardsWFBP() {
    let divContainer = '';
    divContainer += CreateCardNew();
    App.storeWorkFlow.data.items.forEach(x => {
        let odato = { Code: x.data.Code, Name: x.data.Name, Description: x.data.Description, Active: x.data.Active, Public: x.data.Public, LinkedRoles: x.data.LinkedRoles };
        divContainer += CreateCard(odato, [
            { btn: 'Delete', func: "DeleteWF('"+ x.data.Code + "')" },
            { btn: 'Edit', func: "EditWF('" + x.data.Code + "')" }]);
    });
    document.getElementById('dtvTarjetas').innerHTML = divContainer;
}

function CreateCardNew() {
    let html = '';

    html += '<div class="contWFCard">' +
        '<div class="contAddNew">' +
        '<div class="icon-add"></div >' +
        '<div class="textAdd">' + jsCrear + '</div>' +
        '<div class="optionsAdd hidden"  onclick="AddWorkFlow()">' + jsWorkFlow + '</div>' +
        //'<div class="optionsAdd hidden" style="margin-left: 1.5rem;" onclick="AddBusinessProcess()">' + jsBusinessProcess + '</div>' +
        '</div>' +
        '</div>';

    return html;
}

//#endregion

//#region PAGINACION

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storeWorkFlow.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeWorkFlow.pageSize = wantedPageSize;
        App.storeWorkFlow.load();
    }
}

//#endregion

//#region GESTION

function AddWorkFlow() {
    addTab(parent.App.tabPpal, jsAgregarWorkFlow, jsAgregarWorkFlow, "/Modulos/WorkOrder/FormularioWorkFlow.aspx");
}

function EditWF(code) {
    var pagina = "/Modulos/WorkOrder/FormularioWorkFlow.aspx?WFCode=" + code;
    parent.addTab(parent.parent.App.tabPpal, jsEditar + code, jsEditar + " " + code, pagina);
}

var codeWorkFlow;

function DeleteWF(code) {
    codeWorkFlow = code
    Ext.Msg.alert(
        {
            title: jsEliminar,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxDeleteWorkflow,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxDeleteWorkflow(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.DeleteWorkflow(codeWorkFlow,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        App.storeWorkFlow.reload();
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