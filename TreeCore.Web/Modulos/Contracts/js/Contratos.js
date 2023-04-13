var Agregar = false;
var seleccionado;

function ShowRightClickMenu(item, record, node, index, e) {
    e.preventDefault();
    var menu = App.mnOpciones;
    showMenu(record.data, getActualXY(menu, e));
}

function showMenu(data, position) {
    var menu = App.mnOpciones;
    menu.dataRecord = data;
    menu.showAt(position);
}

function showMenuViews(data, position) {
    var menu = App.mnOpcionesViews;
    menu.dataRecord = data;
    menu.showAt(position);
}

//INICIO GESTION GRID 

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        App.btnEliminar.enable();

        App.btnAnadir.setTooltip(jsAgregar);
        App.btnEliminar.setTooltip(jsEliminar);
    }
}

function DeseleccionarGrilla() {
    App.btnAnadir.enable();
    App.GridRowSelect.clearSelections();
    App.btnEliminar.disable();
}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storeContratos.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeContratos.pageSize = wantedPageSize;
        App.storeContratos.load();
    }
}



//FIN GESTION GRID 

//#region Panels

//#region Buscador predictivo Views

var dataViews = [];

function ClearfilterViews() {
    App.txtFiltroViews.reset();
    App.txtFiltroViews.getTrigger(1).hide();
    App.txtFiltroViews.getTrigger(0).show();
}

function CargarBuscadorPredictivoViews() {
    dataViews = [];
    App.storeViews.data.items.forEach(oCtr => {
        dataViews.push({
            key: oCtr.data.Code.toLowerCase(),
            codigo: oCtr.data.Code,
            id: oCtr.id
        });
    });

    dataViews = dataViews.sort(function (a, b) {
        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
    });

    var nameSearchBox = "txtFiltroViews";
    var selectorSearchBox = `#${nameSearchBox}-inputEl`;

    $(function () {
        let textBuscado = "";
        $(selectorSearchBox).autocomplete({
            source: function (request, response) {
                textBuscado = request.term;
                var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                let results = $.grep(dataViews, function (value) {
                    let value1 = value.key;
                    return matcher.test(value1) || matcher.test(normalize(value1));
                });

                response(results.slice(0, 10));

                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {
                    var idCol = $(e.currentTarget).attr("data-emplazamientoID");
                    ViewID = idCol;
                    filterViews();
                });
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            let title = boldQuery(item.codigo, textBuscado);
            return $("<li>")
                .append(`<div class="document-item" data-emplazamientoID="${item.id}">` +
                    `<div class="item-Buscador">` +
                    `<div class="title">${title}</div>` +
                    `</div>` +
                    `<div class="description"></div>` +
                    "</div>")
                .appendTo(ul);
        };
    });
}

var ViewID = '';

function filterViews() {
    var logic = App.storeViews,
        text = App.txtFiltroViews.getRawValue();

    logic.clearFilter();

    if (Ext.isEmpty(text, false)) {
        App.txtFiltroViews.getTrigger(1).hide();
        App.txtFiltroViews.getTrigger(0).show();
        return;
    }
    // this will allow invalid regexp while composing, for example "(examples|grid|color)"
    try {
        var re = new RegExp(".*" + text + ".*", "i");
    } catch (err) {
        return;
    }

    if (ViewID != '') {
        var ContrIDAux = ViewID;
        ViewID = '';
        logic.filterBy(function (node) {
            var valido = false;
            if (ContrIDAux == node.id.toString()) {
                valido = true;
            }
            return valido;
        });
    } else {
        App.txtFiltroViews.getTrigger(0).hide();
        App.txtFiltroViews.getTrigger(1).show();
        logic.filterBy(function (node) {
            var valido = false;
            if (re.test(node.data.Code)) {
                valido = true;
            }
            return valido;
        });

    }
}

//#endregion

function selectView(sender, reg) {
    App.hdActiveView.setValue(reg.data.Code);
    let cols = [];
    reg.data.Columns.forEach(x => {
        cols.push({ 'Code': x.Column, 'Name': x.ColumnName, 'Order': x.Order, 'Active': true });
    });
    App.hdJsonColsNew.setValue(JSON.stringify(cols));
    showLoadMask(App.vwResp, function (load) {
        TreeCore.ActualizarGrid(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    if (!App.pnViews.hidden) {
                        CargarStoresSerie([App.storeColumnas], function Fin(fin) {
                            if (fin) {
                                RenderDragDrop();
                                load.hide();
                            }
                        })
                    } else {
                        load.hide();
                    }
                }
            });
    });
}

function RenderIcono(ruta) {
    return '<img src="/ima/infricons/' + ruta + '" width="20" height="20">';
}

function RenderDef(value) {
    if (value) {
        return '<img src="/ima/ico_default_gr.svg" width="20" height="20">';
    } else {
        return '';
    }
}

function panelVistas() {
    if (App.pnGridViews.width == 300) {
        App.pnGridViews.setWidth(50);
        App.tblFiltroViews.hide();
        App.tblFillViews.hide();
        App.colViewDef.hide();
        App.colViewCode.hide();
        App.colEditView.hide();
        App.colOptionsView.hide();
        App.tlViews.hide();
    } else {
        App.pnGridViews.setWidth(300);
        App.pnDetails.collapse();
        App.tblFiltroViews.show();
        App.tblFillViews.show();
        App.colViewCode.show();
        App.colViewDef.show();
        App.colEditView.show();
        App.colOptionsView.show();
        App.tlViews.show();
    }
}

function MostrarDetalleContrato(data) {
    App.lbPnDetail.setText(jsModeloDatos);
    App.lbPnDetail.setIconCls('btnDatamodelgreen');
    try {
        $("#vsDtContratcs").accordion("destroy");    // Removes the accordion bits
        $("#vsDtContratcs").empty();                // Clears the contents
    } catch (e) {

    }
    App.pnDetails.selectItem = data.Code;
    let tablaBody = document.getElementById('vsDtContratcs');
    let html = '';
    let trad = JSON.parse(App.hdTraducciones.value);
    for (let val in data) {
        if (trad[val] != undefined) {
            let valor = data[val];
            if (!isNaN(Date.parse(valor)) && valor.length == 19) {
                valor = formatDate(Date.parse(valor));
            } if (valor === true || valor === false || valor === 'true' || valor === 'false') {
                valor = RenderColBool(valor);
            }
            html += '<div class="tmpCol-td">' +
                '<span class="lblGrd">' + trad[val] + '</span>' +
                '<span class="dataGrd">' + valor + '</span>' +
                '</div >';
        }
    }
    tablaBody.innerHTML = html;
    App.pnDetails.expand();
}

function MostrarDetalleContratoLineas(lineas) {
    App.lbPnDetail.setText(jsLineaContrato);
    App.lbPnDetail.setIconCls('btnContrLineBlue');
    try {
        $("#vsDtContratcs").accordion("destroy");    // Removes the accordion bits
        $("#vsDtContratcs").empty();                // Clears the contents
    } catch (e) {

    }
    let tablaBody = document.getElementById('vsDtContratcs');
    let html = '';
    let trad = JSON.parse(App.hdTraducciones.value);
    for (let lID in lineas) {
        let linea = lineas[lID]
        html += '<h3 class="titleAcor">' + linea.Code + '</h3><div>';
        for (let val in linea) {
            if (val == 'ContractLineTaxes' && linea[val] != null && linea[val].length > 0) {
                html += '<div class="tmpCol-td">' +
                    '<span class="lblGrd">' + trad[val] + '</span>';
                for (var taxID in linea[val]) {
                    html += '<span class="dataGrd">' + linea[val][taxID].TaxCode + '</span>';
                }
                html += '</div>';
            } else if (val == 'PricesReadjustment') {
                for (let valID in linea[val]) {
                    let valor = linea[val][valID];
                    if (valor == undefined || valor == null || valor == null) {
                        continue;
                    }
                    if (!isNaN(Date.parse(valor)) && valor.length >= 19) {
                        valor = formatDate(Date.parse(valor));
                    } if (valor === true || valor === false || valor === 'true' || valor === 'false') {
                        valor = RenderColBool(valor);
                    }
                    html += '<div class="tmpCol-td">' +
                        '<span class="lblGrd">' + trad[valID] + '</span>' +
                        '<span class="dataGrd">' + valor + '</span>' +
                        '</div>';
                }                
            } else if (val == 'Payees' && linea[val] != null && linea[val].length > 0) {
                html += '<div class="cntSubSecc">';
                for (var CompanyID in linea[val]) {
                    html += '<h4 class="titleAcorSubSecc">' + linea[val][CompanyID].CompanyCode + '</h4><div>';
                    for (var CompValID in linea[val][CompanyID]) {
                        let valor = linea[val][CompanyID][CompValID];
                        if (!isNaN(Date.parse(valor)) && valor.length >= 19) {
                            valor = formatDate(Date.parse(valor));
                        } if (valor === true || valor === false || valor === 'true' || valor === 'false') {
                            valor = RenderColBool(valor);
                        }
                        html += '<div class="tmpCol-td">' +
                            '<span class="lblGrd">' + trad[CompValID] + '</span>' +
                            '<span class="dataGrd">' + valor + '</span>' +
                            '</div>';
                    }
                    html += '</div>';
                }
                html += '</div>';
            }
            else if (trad[val] != undefined) {
                let valor = linea[val];
                if (!isNaN(Date.parse(valor)) && valor.length >= 19) {
                    valor = formatDate(Date.parse(valor));
                } if (valor === true || valor === false || valor === 'true' || valor === 'false') {
                    valor = RenderColBool(valor);
                }
                html += '<div class="tmpCol-td">' +
                    '<span class="lblGrd">' + trad[val] + '</span>' +
                    '<span class="dataGrd">' + valor + '</span>' +
                    '</div>';
            }
        }
        html += '</div>';
    }
    tablaBody.innerHTML = html;
    $("#vsDtContratcs").accordion();
}

function openPanelDetails() {
    App.pnGridViews.setWidth(50);
    App.btnControlpnViews.setPressed(false);
    App.tblFiltroViews.hide();
    App.tblFillViews.hide();
    App.colViewDef.hide();
    App.colViewCode.hide();
    App.colEditView.hide();
    App.colOptionsView.hide();
    App.tlViews.hide();
}

function optionsViews(sender) {
    showMenuViews(sender.getWidgetRecord().data, [sender.getX() + 20, sender.getY() + 20]);
}

function seleccionarVistaActiva() {

}

//#endregion

//#region TOOLBARS

//#region GESTION

function AddContrato() {

}

function DeleteContrato() {

}

function DescargarContrato() {

}

function RefrescarContrato() {
    App.storeContratos.reload();
    App.GridRowSelect.clearSelections();
}

function OpenViews() {
    if (App.gridContratos.isHidden()) {
        App.pnViews.hide();
        App.gridContratos.show();
    } else {
        App.gridContratos.hide();
        App.pnViews.show();
        CargarStoresSerie([App.storeColumnas], function Fin(fin) {
            if (fin) {
                RenderDragDrop();
            }
        })
    }
}

//#region Buscador predictivo Contratos

var dataGridContr = [];

function ClearfilterContratos() {
    App.txtFiltroContratos.reset();
    App.txtFiltroContratos.getTrigger(1).hide();
    App.txtFiltroContratos.getTrigger(0).show();
}

function CargarBuscadorPredictivoContracts() {
    dataGridContr = [];
    App.storeContratos.data.items.forEach(oCtr => {
        dataGridContr.push({
            key: oCtr.data.Code.toLowerCase(),
            codigo: oCtr.data.Code,
            id: oCtr.id
        });
    });

    dataGridContr = dataGridContr.sort(function (a, b) {
        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
    });

    var nameSearchBox = "txtFiltroContratos";
    var selectorSearchBox = `#${nameSearchBox}-inputEl`;

    $(function () {
        let textBuscado = "";
        $(selectorSearchBox).autocomplete({
            source: function (request, response) {
                textBuscado = request.term;
                var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                let results = $.grep(dataGridContr, function (value) {
                    let value1 = value.key;
                    return matcher.test(value1) || matcher.test(normalize(value1));
                });

                response(results.slice(0, 10));

                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {
                    var idCol = $(e.currentTarget).attr("data-emplazamientoID");
                    ColumnaID = idCol;
                    filterContracts();
                });
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            let title = boldQuery(item.codigo, textBuscado);
            return $("<li>")
                .append(`<div class="document-item" data-emplazamientoID="${item.id}">` +
                    `<div class="item-Buscador">` +
                    `<div class="title">${title}</div>` +
                    `</div>` +
                    `<div class="description"></div>` +
                    "</div>")
                .appendTo(ul);
        };
    });
}

var ContractID = '';

function filterContracts() {
    var logic = App.storeContratos,
        text = App.txtFiltroContratos.getRawValue();

    logic.clearFilter();

    if (Ext.isEmpty(text, false)) {
        App.txtFiltroContratos.getTrigger(1).hide();
        App.txtFiltroContratos.getTrigger(0).show();
        return;
    }
    // this will allow invalid regexp while composing, for example "(examples|grid|color)"
    try {
        var re = new RegExp(".*" + text + ".*", "i");
    } catch (err) {
        return;
    }

    if (ContractID != '') {
        var ContrIDAux = ContractID;
        ContractID = '';
        logic.filterBy(function (node) {
            var valido = false;
            if (ContrIDAux == node.id.toString()) {
                valido = true;
            }
            return valido;
        });
    } else {
        App.txtFiltroContratos.getTrigger(0).hide();
        App.txtFiltroContratos.getTrigger(1).show();
        logic.filterBy(function (node) {
            var valido = false;
            if (re.test(node.data.Code)) {
                valido = true;
            }
            return valido;
        });

    }
}

function GuardarViews() {
    TreeCore.GuardarView(false, App.hdActiveView.value,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    App.storeViews.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

var AddEditView;

function DefaultView(codeView) {
    TreeCore.DefaultView(codeView,
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                } else {
                    App.storeViews.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}
function DeleteView(codeView) {
    if (codeView != App.hdActiveView.value) {
        TreeCore.DeleteView(codeView,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        App.storeViews.reload();
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    } else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: 'You cant delete active view', buttons: Ext.Msg.OK });
    }
}

function EditView(codeView) {
    var actView = App.storeViews.getById(codeView);
    App.txtCodigo.setValue(actView.data.Code);
    App.cmbIconos.setValue(actView.data.Icon);
    App.winGestionViews.setTitle(jsEditar + ' ' + jsVista);
    App.winGestionViews.show();
    App.winGestionViews.codeView = codeView;
    AddEditView = 'Edit';
}

function NewView() {
    App.txtCodigo.reset();
    App.cmbIconos.reset();
    App.winGestionViews.setTitle(jsAgregar + ' ' + jsVista);
    App.winGestionViews.show();
    AddEditView = 'Add';
}

function FormSaveView() {
    if (AddEditView == 'Edit') {
        TreeCore.GuardarView(true, App.winGestionViews.codeView,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        App.winGestionViews.hide();
                        App.storeViews.reload();
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    } else {
        TreeCore.AddNewView(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        App.winGestionViews.hide();
                        App.storeViews.reload();
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

//#endregion

//#region FILTROS

function RecargarCombo(sender, registro, index) {

}

function SeleccionarCombo(sender, registro, index) {

}


function ReloadCmbFiltro(sender) {
    sender.reset();
    App.storeContratos.reload();
    recargarCombos([sender]);
}

function SelectCmbFiltro(sender) {
    sender.getTrigger(0).show();
    App.storeContratos.reload();
}

function ClearFilters() {
    App.cmbFiltroEstados.reset();
    App.cmbFiltroEstados.getTrigger(0).hide();
    App.cmbFiltroTipos.reset();
    App.cmbFiltroTipos.getTrigger(0).hide();
    App.cmbFiltroGrupos.reset();
    App.cmbFiltroGrupos.getTrigger(0).hide();
    App.cmbFiltroMonedas.reset();
    App.cmbFiltroMonedas.getTrigger(0).hide();
    App.storeContratos.reload();
}

function SaveFilters() {

}

//#endregion

//#endregion

//#region GRID

function EditarContrato(sender, registro, aux) {
    var code = sender.$widgetRecord.data.Code;
    var pagina = "/Modulos/Contracts/FormContratos.aspx?ContractCode=" + code;
    parent.addTab(parent.parent.App.tabPpal, jsEditar + code, jsEditar + " " + code, pagina);
}

function DetalleContrato(sender) {
    MostrarDetalleContrato(sender.getWidgetRecord().data);
}

function AccionesContrato(sender) {
    showMenu(sender.getWidgetRecord().data, [sender.getX() + 20, sender.getY() + 20]);
}

var DelContr;

function EliminarContrato(code) {
    DelContr = code;
    Ext.Msg.alert(
        {
            title: jsEliminar,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxEliminarContrato,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxEliminarContrato(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.DeleteContract(DelContr,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        App.storeContratos.reload();
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

//#region Views

//#region Seleccion Columnas

function SeleccionarColumna(sender) {
    let code = sender.getAttribute('code');
    let checked = sender.checked;

    App.storeColumnas.getById(code).data.Active = checked ? 'checked' : '';
    App.storeColumnas.getById(code).data.Order = App.storeColumnas.data.items.filter(word => word.data.Active).length;;

    RenderDragDrop();
}

function RenderDragDrop() {
    var html = '';
    let listCols = App.storeColumnas.getDataSource().items.filter(col => col.data.Active == 'checked').sort(function (a, b) {
        if (a.data.Order > b.data.Order)
            return 1;
        if (a.data.Order < b.data.Order)
            return -1;
        return 0;
    });
    listCols.forEach(x => {
        html += '<li class="dragDropCol" code="' + x.id + '">' +
            '	<div class="colName spanLbl">' + x.data.Name + '</div>' +
            '	<div class="dragItem ico-drag-horizontal"></div>' +
            '</li>	';
    });

    document.getElementById('ctOrderCols').innerHTML = html;

    $("#ctOrderCols").sortable({
        handle: ".dragItem",
        pullPlaceholder: false,
        onDrop: function ($item, container, _super) {
            var $clonedItem = $('<li/>').css({ height: 0 });
            $item.before($clonedItem);
            $clonedItem.animate({ 'height': $item.height() });

            $item.animate($clonedItem.position(), function () {
                $clonedItem.detach();
                _super($item, container);
            });
            UpdateDragDrop();
        }
    });
}

function UpdateDragDrop() {
    Array.from(document.getElementsByClassName("dragDropCol")).forEach(function (item, order) {
        App.storeColumnas.getById(item.getAttribute('code')).data.Order = order;
    });
}

function ApplyCols() {
    let cols = [];
    let listCols = App.storeColumnas.data.items.filter(col => col.data.Active == 'checked').sort(function (a, b) {
        if (a.data.Order > b.data.Order)
            return 1;
        if (a.data.Order < b.data.Order)
            return -1;
        return 0;
    });
    listCols.forEach(x => {
        cols.push(x.data);
    });
    App.hdJsonColsNew.setValue(JSON.stringify(cols));
    TreeCore.ActualizarGrid(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

//#endregion

//#region Buscador predictivo Columnas

var dataColumnas = [];

function ClearfilterColumnas() {
    App.txtFiltroColumnas.reset();
    App.txtFiltroColumnas.getTrigger(1).hide();
    App.txtFiltroColumnas.getTrigger(0).show();
}

function CargarBuscadorPredictivoColumnas() {
    dataColumnas = [];
    App.storeColumnas.data.items.forEach(oCtr => {
        dataColumnas.push({
            key: oCtr.data.Name.toLowerCase(),
            codigo: oCtr.data.Name,
            id: oCtr.id
        });
    });

    dataColumnas = dataColumnas.sort(function (a, b) {
        return a.key.toString().toLowerCase().localeCompare(b.key.toString().toLowerCase());
    });

    var nameSearchBox = "txtFiltroColumnas";
    var selectorSearchBox = `#${nameSearchBox}-inputEl`;

    $(function () {
        let textBuscado = "";
        $(selectorSearchBox).autocomplete({
            source: function (request, response) {
                textBuscado = request.term;
                var matcher = new RegExp($.ui.autocomplete.escapeRegex(normalize(request.term)), "i");
                let results = $.grep(dataColumnas, function (value) {
                    let value1 = value.key;
                    return matcher.test(value1) || matcher.test(normalize(value1));
                });

                response(results.slice(0, 10));

                $(".ui-menu-item>.ui-menu-item-wrapper").click(function (e) {
                    var idCol = $(e.currentTarget).attr("data-emplazamientoID");
                    ColumnaID = idCol;
                    filterColumnas();
                });
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            let title = boldQuery(item.codigo, textBuscado);
            return $("<li>")
                .append(`<div class="document-item" data-emplazamientoID="${item.id}">` +
                    `<div class="item-Buscador">` +
                    `<div class="title">${title}</div>` +
                    `</div>` +
                    `<div class="description"></div>` +
                    "</div>")
                .appendTo(ul);
        };
    });
}

var ColumnaID = '';

function filterColumnas() {
    var logic = App.storeColumnas,
        text = App.txtFiltroColumnas.getRawValue();

    logic.clearFilter();

    if (Ext.isEmpty(text, false)) {
        App.txtFiltroColumnas.getTrigger(1).hide();
        App.txtFiltroColumnas.getTrigger(0).show();
        return;
    }
    // this will allow invalid regexp while composing, for example "(examples|grid|color)"
    try {
        var re = new RegExp(".*" + text + ".*", "i");
    } catch (err) {
        return;
    }

    if (ColumnaID != '') {
        var ContrIDAux = ColumnaID;
        ColumnaID = '';
        logic.filterBy(function (node) {
            var valido = false;
            if (ContrIDAux == node.id.toString()) {
                valido = true;
            }
            return valido;
        });
    } else {
        App.txtFiltroColumnas.getTrigger(0).hide();
        App.txtFiltroColumnas.getTrigger(1).show();
        logic.filterBy(function (node) {
            var valido = false;
            if (re.test(node.data.Name)) {
                valido = true;
            }
            return valido;
        });

    }
}

//#endregion

//#endregion
