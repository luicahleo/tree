var ActualLine;
var ActualCompany;
var ActualNode;

var contLines = 0;
var contCompanies = {};

$(document).ready(function ConfigurarToast() {
    Toast.enableTimers(true);
    Toast.setTheme(TOAST_THEME.LIGHT);
    Toast.setPlacement(TOAST_PLACEMENT.TOP_RIGHT);
    Toast.setMaxCount(2);
})

function CancelForm() {
    parent.App.tabPpal.activeTab.close()
}

function SaveContract() {
    try {
        App[ActualNode.data.Panel].events.hide.listeners[0].fn();
    } catch (e) {

    }
    TreeCore.UploadContract(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    Toast.create(jsInfo, jsGuardado, TOAST_STATUS.SUCCESS, 3000);
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
}

function ControlDuplicidadLine(sender) {
    if (GetNodeLines().childNodes.filter(x => x.data.text == sender.value && x.id != ActualNode.id).length > 0) {
        sender.setValue(ActualNode.data.OriginalCode);
        Toast.create(jsInfo, 'Line Code Already exist', TOAST_STATUS.INFO, 3000);
    }
}

function ControlDuplicidadCompany(sender) {
    if (GetNodeCompanies().childNodes.filter(x => x.data.text == sender.value && x.id != ActualNode.id).length > 0) {
        sender.setValue(ActualNode.data.OriginalCode);
        Toast.create(jsInfo, 'Company Already exist', TOAST_STATUS.INFO, 3000);
    }
}

function ContractValid(sender) {
    let btnSave = App.btnGuardarContratos;
    btnSave.disable();
    let valid = true;
    if (sender) {
        if (sender.id == App.txtLineCode.id) {
            ControlDuplicidadLine(sender);
        } else if (sender.id == App.cmbLineCompany.id) {
            ControlDuplicidadCompany(sender);
        }
    }

    //#region ActualForm

    Ext.each(App[ActualNode.data.Panel].body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && c.isFormField) {
            if (c.disabled) {
                document.getElementById(c.id).setAttribute('invalid', false);

            } else {
                if (c.cls.includes('required') && !c.value) {
                    document.getElementById(c.id).setAttribute('invalid', true);
                    valid = false;
                } else {
                    document.getElementById(c.id).setAttribute('invalid', false);
                }
            }
        }
    });

    if (!valid) {
        ActualNode.addCls('errorNode');
        return;
    } else {
        ActualNode.removeCls('errorNode');
    }

    try {
        App[ActualNode.data.Panel].events.hide.listeners[0].fn();
    } catch (e) {

    }

    //#endregion

    let oContract = JSON.parse(App.hdObjeto.value);

    //#region DataModel

    if (oContract.Code == undefined || oContract.Code == '') {
        valid = false;
        return;
    }
    if (oContract.Name == undefined || oContract.Name == '') {
        valid = false;
        return;
    }
    if (oContract.Description == undefined || oContract.Description == '') {
        valid = false;
        return;
    }
    /*if (oContract.SiteCode == undefined || oContract.SiteCode == undefined) {
        valid = false;
        return;
    }*/

    if (oContract.ContractStatusCode == undefined || oContract.ContractStatusCode == '') {
        valid = false;
        return;
    }
    if (oContract.CurrencyCode == undefined || oContract.CurrencyCode == '') {
        valid = false;
        return;
    }
    if (oContract.ContractGroupCode == undefined || oContract.ContractGroupCode == '') {
        valid = false;
        return;
    }
    if (oContract.ContractTypeCode == undefined || oContract.ContractTypeCode == '') {
        valid = false;
        return;
    }

    if (oContract.ExecutionDate == undefined || oContract.ExecutionDate == '' || isNaN(Date.parse(oContract.ExecutionDate)) || new Date(oContract.ExecutionDate).getYear() < 0) {
        valid = false;
        return;
    }
    if (oContract.StartDate == undefined || oContract.StartDate == '' || isNaN(Date.parse(oContract.StartDate)) || new Date(oContract.StartDate).getYear() < 0) {
        valid = false;
        return;
    }
    if (oContract.FirstEndDate == undefined || oContract.FirstEndDate == '' || isNaN(Date.parse(oContract.FirstEndDate)) || new Date(oContract.FirstEndDate).getYear() < 0) {
        valid = false;
        return;
    }

    //#endregion

    //#region RenewalClause


    if (oContract.RenewalClause == undefined || oContract.RenewalClause == '') {
        valid = false;
        return;
    }

    if (oContract.RenewalClause.Type == undefined || oContract.RenewalClause.Type == '') {
        valid = false;
        return;
    }

    switch (oContract.RenewalClause.Type) {
        case 'Auto':
        case 'Optional':
            if (oContract.RenewalClause.Frequencymonths == undefined || oContract.RenewalClause.Frequencymonths == '') {
                valid = false;
                return;
            }
            if (oContract.RenewalClause.TotalRenewalNumber == undefined || oContract.RenewalClause.TotalRenewalNumber == '') {
                valid = false;
                return;
            }
            break;
        case 'Previous negotiation':
            break;
    }

    //#endregion

    //#region Lines

    if (oContract.contractline != undefined && oContract.contractline.length > 0) {
        oContract.contractline.forEach(line => {
            if (line.Code == undefined || line.Code == '') {
                valid = false;
                return;
            }
            if (line.Description == undefined || line.Description == '') {
                valid = false;
                return;
            }
            if (line.Value == undefined || line.Value == '') {
                valid = false;
                return;
            }
            if (line.lineTypeCode == undefined || line.lineTypeCode == '') {
                valid = false;
                return;
            }
            if (line.CurrencyCode == undefined || line.CurrencyCode == '') {
                valid = false;
                return;
            }

            if (line.ValidFrom == undefined || line.ValidFrom == '' || isNaN(Date.parse(line.ValidFrom)) || new Date(line.ValidFrom).getYear() < 0) {
                valid = false;
                return;
            }
            if (line.ValidTo == undefined || line.ValidTo == '' || isNaN(Date.parse(line.ValidTo)) || new Date(line.ValidTo).getYear() < 0) {
                valid = false;
                return;
            }
            if (line.NextPaymentDate == undefined || line.NextPaymentDate == '' || isNaN(Date.parse(line.NextPaymentDate)) || new Date(line.NextPaymentDate).getYear() < 0) {
                valid = false;
                return;
            }

            if (line.PricesReadjustment == undefined || line.PricesReadjustment == '') {
                valid = false;
                return;
            }

            if (line.PricesReadjustment.Type == undefined || line.PricesReadjustment.Type == '') {
                valid = false;
                return;
            }

            switch (line.PricesReadjustment) {
                case 'PCI':
                    if (line.StartDate == undefined || line.StartDate == '' || isNaN(Date.parse(line.StartDate)) || new Date(line.StartDate).getYear() < 0) {
                        valid = false;
                        return;
                    }
                    if (line.NextDate == undefined || line.NextDate == '' || isNaN(Date.parse(line.NextDate)) || new Date(line.NextDate).getYear() < 0) {
                        valid = false;
                        return;
                    }
                    if (line.CodeInflation == undefined || line.CodeInflation == '') {
                        valid = false;
                        return;
                    }
                    break;
                case 'FixedAmount':
                    if (line.StartDate == undefined || line.StartDate == '' || isNaN(Date.parse(line.StartDate)) || new Date(line.StartDate).getYear() < 0) {
                        valid = false;
                        return;
                    }
                    if (line.NextDate == undefined || line.NextDate == '' || isNaN(Date.parse(line.NextDate)) || new Date(line.NextDate).getYear() < 0) {
                        valid = false;
                        return;
                    }
                    if (line.EndDate == undefined || line.EndDate == '' || isNaN(Date.parse(line.EndDate)) || new Date(line.EndDate).getYear() < 0) {
                        valid = false;
                        return;
                    }
                    if (line.Frequency == undefined || line.Frequency == '') {
                        valid = false;
                        return;
                    }
                    if (line.FixedAmount == undefined || line.FixedAmount == '') {
                        valid = false;
                        return;
                    }
                    break;
                case 'FixedPercentege':
                    if (line.StartDate == undefined || line.StartDate == '' || isNaN(Date.parse(line.StartDate)) || new Date(line.StartDate).getYear() < 0) {
                        valid = false;
                        return;
                    }
                    if (line.NextDate == undefined || line.NextDate == '' || isNaN(Date.parse(line.NextDate)) || new Date(line.NextDate).getYear() < 0) {
                        valid = false;
                        return;
                    }
                    if (line.EndDate == undefined || line.EndDate == '' || isNaN(Date.parse(line.EndDate)) || new Date(line.EndDate).getYear() < 0) {
                        valid = false;
                        return;
                    }
                    if (line.Frequency == undefined || line.Frequency == '') {
                        valid = false;
                        return;
                    }
                    if (line.FixedPercentage == undefined || line.FixedPercentage == '') {
                        valid = false;
                        return;
                    }
                    break;
                case 'WithoutIncrements':
                    break;
            }

            if (line.Payees != undefined && line.Payees.length > 0) {
                line.Payees.forEach(paye => {
                    if (paye.CompanyCode == undefined || paye.CompanyCode == '') {
                        valid = false;
                        return;
                    }
                    if (paye.PaymentMethodCode == undefined || paye.PaymentMethodCode == '') {
                        valid = false;
                        return;
                    }
                    if (paye.currencyCode == undefined || paye.currencyCode == '') {
                        valid = false;
                        return;
                    }
                    if (paye.Percent == undefined || paye.Percent == '') {
                        valid = false;
                        return;
                    }
                });
            }

        });
    }

    //#endregion

    if (valid) {
        btnSave.enable();
    }
}

function SelectSeccion(sender, panel) {
    if (panel.id == 'root') {
        panel = App.gridSecciones.getRootNode().childNodes[0];
    }
    if (panel.childNodes.length > 0 && !panel.isExpanded()) {
        App.btnContinuar.show();
    }
    App.formDataModel.hide();
    App.formRenewal.hide();
    App.formLines.hide();
    App.formLine.hide();
    App.formLinePriceReadjustment.hide();
    App.formCompanies.hide();
    App.formLineCompany.hide();
    App.formLineTaxes.hide();
    ActualLine = panel.data.LineCode;
    ActualCompany = panel.data.CompanyCode;
    ActualNode = panel;
    ClearSeccion(panel.data.Panel);
    App[panel.data.Panel].show();
}

function ClearSeccion(seccion) {
    Ext.each(App[seccion].body.query('*'), function (item) {
        var c = Ext.getCmp(item.id);
        if (c && c.isFormField) {
            if (c.value != null || c.value != undefined) {
                c.reset();
            }
        }
    });
}

function SiguienteSeccion() {
    if (ActualNode.childNodes.length > 0) {
        ActualNode.expand();
    }
    let oNodo = SiguienteNodo(ActualNode);
    if (!oNodo) {
        App.gridSecciones.getSelectionModel().select(App.gridSecciones.getRootNode().childNodes[0]);
        App.btnContinuar.hide();
    } else {
        App.gridSecciones.getSelectionModel().select(oNodo);
    }
}

//#region Control Nodos

function SiguienteNodo(ActualNode) {
    let esSig = false;
    let node;
    App.gridSecciones.getRootNode().childNodes.forEach(a => {
        if (esSig) {
            node = a;
            esSig = false
        }
        if (a.id == ActualNode.id) {
            esSig = true;
        }
        if (!node) {
            node = RecorrerNodo(a, ActualNode, esSig);
        }
        if (node) {
            return node;
        }
    });
    return node;
}

function RecorrerNodo(Node, ActualNode, esSig) {
    let node;
    Node.childNodes.forEach(a => {
        if (esSig) {
            node = a;
            esSig = false
        }
        if (a.id == ActualNode.id) {
            esSig = true;
        }
        if (!node) {
            node = RecorrerNodo(a, ActualNode, esSig);
        }
        if (node) {
            return node;
        }
    });
    return node;
}



function GetNodeLineActual() {
    let Node;
    Node = GetNodeLines().childNodes.filter(x => x.data.LineCode == ActualLine)[0];
    return Node;
}

function GetNodeLineCompanyActual() {
    let Node;
    Node = GetNodeCompanies().childNodes.filter(x => x.data.CompanyCode == ActualCompany)[0];
    return Node;
}

function GetNodeLines() {
    let Node;
    Node = App.gridSecciones.getRootNode().childNodes.filter(x => x.data.Panel == 'formLines')[0];
    return Node;
}

function GetNodeCompanies() {
    let Node;
    Node = GetNodeLineActual().childNodes.filter(x => x.data.Panel == 'formCompanies')[0];
    return Node;
}

function AddItemContract() {

}

//#endregion

//#region TapDataModel

function SelFechaFirma(sender) {
    if (sender.value && sender.value != '') {
        App.dtFechaInicio.setMinValue(sender.value)
        App.dtFechaInicio.enable();
    }
    if (sender.value > App.dtFechaInicio.value) {
        App.dtFechaInicio.reset();
        App.dtFechaFin.reset();
        App.nbDuracionContratos.reset();
    }
}

function SelFechaInicio(sender) {
    if (sender.value && sender.value != '') {
        App.dtFechaFin.setMinValue(sender.value)
        App.dtFechaFin.enable();
        App.nbDuracionContratos.enable();
    }
    if (sender.value > App.dtFechaFin.value) {
        App.dtFechaFin.reset();
        App.nbDuracionContratos.reset();
    }
}

function CambioFinContrato(sender) {
    let oContract = JSON.parse(App.hdObjeto.value);
    let fechaIncio = (App.dtFechaInicio.value) ? new Date(App.dtFechaInicio.value.toISOString()) : new Date(oContract.StartDate);
    if (sender.id == App.dtFechaFin.id) {
        App.nbDuracionContratos.setValue(calcMonths(sender.value, fechaIncio));
    } else if (sender.id == App.nbDuracionContratos.id && App.dtFechaFin.value.getMonth() != new Date(fechaIncio.setMonth(fechaIncio.getMonth() + sender.value)).getMonth()) {
        fechaIncio = (App.dtFechaInicio.value) ? new Date(App.dtFechaInicio.value.toISOString()) : new Date(oContract.StartDate);
        App.dtFechaFin.setValue(new Date(fechaIncio.setMonth(fechaIncio.getMonth() + sender.value)))
    }
}

function ShowDataModel() {
    let oContract = JSON.parse(App.hdObjeto.value);
    App.txtCodigo.setValue(oContract.Code);
    App.txtNombre.setValue(oContract.Name);
    App.txtDescripcion.setValue(oContract.Description);
    App.cmbTipoContrato.setValue(oContract.ContractTypeCode);
    App.cmbEstadoContrato.setValue(oContract.ContractStatusCode);
    App.cmbGrupoContrato.setValue(oContract.ContractGroupCode);
    App.cmbMonedaContrato.setValue(oContract.CurrencyCode);
    App.dtFechaFirma.setValue(TryGetDate(oContract.ExecutionDate));
    App.dtFechaInicio.setValue(TryGetDate(oContract.StartDate));
    App.dtFechaFin.setValue(TryGetDate(oContract.FirstEndDate));
    App.chkCierraAlExpirar.setValue(oContract.ClosedAtExpiration);
    if (!TryGetDate(oContract.ExecutionDate)) {
        App.dtFechaFin.disable();
        App.nbDuracionContratos.disable();
    }
    if (!TryGetDate(oContract.StartDate)) {
        App.dtFechaFin.disable();
        App.nbDuracionContratos.disable();
    }
    if (!TryGetDate(oContract.FirstEndDate)) {
        //App.dtFechaFin.disable();
    }
}

function SaveDataModel() {
    let oContract = JSON.parse(App.hdObjeto.value);
    oContract.Code = App.txtCodigo.value;
    oContract.Name = App.txtNombre.value;
    oContract.Description = App.txtDescripcion.value;
    oContract.ContractTypeCode = App.cmbTipoContrato.value;
    oContract.ContractStatusCode = App.cmbEstadoContrato.value;
    oContract.ContractGroupCode = App.cmbGrupoContrato.value;
    oContract.CurrencyCode = App.cmbMonedaContrato.value;
    oContract.ExecutionDate = (App.dtFechaFirma.value == undefined || App.dtFechaFirma.value == '') ? undefined : App.dtFechaFirma.value.toDateString();
    oContract.StartDate = (App.dtFechaInicio.value == undefined || App.dtFechaInicio.value == '') ? undefined : App.dtFechaInicio.value.toDateString();
    oContract.FirstEndDate = (App.dtFechaFin.value == undefined || App.dtFechaFin.value == '') ? undefined : App.dtFechaFin.value.toDateString();
    oContract.ClosedAtExpiration = App.chkCierraAlExpirar.checked.toString();
    App.hdObjeto.setValue(JSON.stringify(oContract));
}

//#endregion

//#region TapRenewal

function SelRenewal(sender) {
    SeleccionarCombo(sender);
    App.nbFrecuenciaRenewal.disable();
    App.nbTotalRenewal.disable();
    App.nbActualRenewal.disable();
    App.dtFechaRenewal.disable();
    App.dtFechaFinRenewal.disable();
    App.nbFrecuenciaRenewal.reset();
    App.nbTotalRenewal.reset();
    App.nbActualRenewal.reset();
    App.dtFechaRenewal.reset();
    App.dtFechaFinRenewal.reset();
    if (sender.value == 'Auto' || sender.value == 'Optional') {
        App.nbFrecuenciaRenewal.enable();
        App.nbTotalRenewal.enable();

    } else if (sender.value == 'Previous negotiation') {
    }
}

function calcRenewal() {
    let oContract = JSON.parse(App.hdObjeto.value);
    if (oContract.FirstEndDate) {
        let fechaFin = new Date(oContract.FirstEndDate);
        if (App.nbFrecuenciaRenewal.value && App.nbFrecuenciaRenewal.value != '' && App.nbTotalRenewal.value && App.nbTotalRenewal.value != '') {
            if (App.dtFechaFin.value > new Date()) {
                App.nbActualRenewal.setValue(0);
                App.dtFechaFinRenewal.setValue(App.dtFechaFin.value);
            } else {
                let nRen = Math.ceil(calcMonths(new Date(), fechaFin) / App.nbFrecuenciaRenewal.value);
                let fechaAux = new Date(oContract.FirstEndDate);
                if (new Date(fechaAux.setMonth(fechaAux.getMonth() + (App.nbFrecuenciaRenewal.value * nRen))) < new Date()) {
                    nRen++;
                }
                if (nRen <= App.nbTotalRenewal.value) {
                    App.nbActualRenewal.setValue(nRen);
                    fechaAux = new Date(oContract.FirstEndDate);
                    App.dtFechaRenewal.setValue(new Date(fechaAux.setMonth(fechaAux.getMonth() + (App.nbFrecuenciaRenewal.value * nRen))));
                }
                fechaAux = new Date(oContract.FirstEndDate);
                App.dtFechaFinRenewal.setValue(new Date(fechaAux.setMonth(fechaAux.getMonth() + (App.nbFrecuenciaRenewal.value * App.nbTotalRenewal.value))));
            }
        } else {
            App.nbActualRenewal.reset();
            App.dtFechaRenewal.reset();
            App.dtFechaFinRenewal.reset();
        }
    }
}

function ClearRenewal(sender) {
    RecargarCombo(sender);
    App.nbFrecuenciaRenewal.disable();
    App.nbTotalRenewal.disable();
    App.nbActualRenewal.disable();
    App.dtFechaRenewal.disable();
    App.dtFechaFinRenewal.disable();
    App.nbFrecuenciaRenewal.reset();
    App.nbTotalRenewal.reset();
    App.nbActualRenewal.reset();
    App.dtFechaRenewal.reset();
    App.dtFechaFinRenewal.reset();
}

function ShowRenewal() {
    let oContract = JSON.parse(App.hdObjeto.value);
    if (!oContract.RenewalClause) {
        oContract.RenewalClause = {}
    }
    App.cmbTipoRenewal.setValue(oContract.RenewalClause.Type);
    SelRenewal(App.cmbTipoRenewal);
    App.nbFrecuenciaRenewal.setValue(oContract.RenewalClause.Frequencymonths);
    App.nbTotalRenewal.setValue(oContract.RenewalClause.TotalRenewalNumber);
    App.nbActualRenewal.setValue(oContract.RenewalClause.CurrentRenewalNumber);
    App.dtFechaRenewal.setValue(TryGetDate(oContract.RenewalClause.RenewalExpirationDate));
    App.dtFechaFinRenewal.setValue(TryGetDate(oContract.RenewalClause.ContractExpirationDate));
}

function SaveRenewal() {
    let oContract = JSON.parse(App.hdObjeto.value);
    if (!oContract.RenewalClause) {
        oContract.RenewalClause = {}
    }
    oContract.RenewalClause.Type = App.cmbTipoRenewal.value;
    oContract.RenewalClause.Frequencymonths = App.nbFrecuenciaRenewal.value;
    oContract.RenewalClause.TotalRenewalNumber = App.nbTotalRenewal.value;
    oContract.RenewalClause.CurrentRenewalNumber = App.nbActualRenewal.value;
    oContract.RenewalClause.RenewalExpirationDate = (App.dtFechaRenewal.value == undefined || App.dtFechaRenewal.value == '') ? undefined : App.dtFechaRenewal.value.toDateString();
    oContract.RenewalClause.ContractExpirationDate = (App.dtFechaFinRenewal.value == undefined || App.dtFechaFinRenewal.value == '') ? undefined : App.dtFechaFinRenewal.value.toDateString();
    App.hdObjeto.setValue(JSON.stringify(oContract));
}

//#endregion

//#region TapLines

function AddNewLine() {
    let oContract = JSON.parse(App.hdObjeto.value);
    let code = contLines++;
    while (GetNodeLines().childNodes.filter(x => x.data.text == jsLinea + code).length > 0) {
        code = contLines++
    }
    GetNodeLines().appendChild({
        text: jsLinea + code,
        leaf: false,
        Panel: 'formLine',
        LineCode: code,
        CompanyCode: '#None',
        OriginalCode: jsLinea + code,
        expanded: false,
        expandable: true,
    });
    contCompanies[jsLinea + code] = 0;
    AddLineItems(code);
    let line = {};
    line.Code = jsLinea + code;
    oContract.contractline.push(line);
    App.hdObjeto.setValue(JSON.stringify(oContract));
    ShowLines();
    ContractValid();
}

function AddLineItems(code) {
    let oLineNode = GetNodeLines().childNodes.filter(x => x.data.LineCode == code)[0];
    oLineNode.appendChild({
        text: jsReajustePrecio,
        leaf: true,
        Panel: 'formLinePriceReadjustment',
        LineCode: code,
        CompanyCode: '#None',
    });
    oLineNode.appendChild({
        text: jsImpuestos,
        leaf: true,
        Panel: 'formLineTaxes',
        LineCode: code,
        CompanyCode: '#None',
    });
    oLineNode.appendChild({
        text: jsBeneficiarios,
        leaf: false,
        Panel: 'formCompanies',
        LineCode: code,
        CompanyCode: '#None',
    });
    oLineNode.save();
}

function SelectLine(sender) {
    let lineCode = sender.getAttribute('code');
    GetNodeLines().expand();
    let NodeLine = GetNodeLines().childNodes.filter(x => x.data.text == lineCode)[0];
    App.gridSecciones.getSelectionModel().select(NodeLine);
}

function editLine(sender) {
    let lineCode = sender.parentElement.parentElement.getAttribute('code');
    GetNodeLines().expand();
    let NodeLine = GetNodeLines().childNodes.filter(x => x.data.text == lineCode)[0];
    App.gridSecciones.getSelectionModel().select(NodeLine);
}

function addCompanyToLine(sender) {
    let lineCode = sender.parentElement.parentElement.getAttribute('code');
    let oNodeLine = GetNodeLines().childNodes.filter(x => x.data.text == lineCode)[0];
    let oNodeCompanies = oNodeLine.childNodes.filter(x => x.data.Panel == 'formCompanies')[0];;
    let oContract = JSON.parse(App.hdObjeto.value);
    if (!contCompanies[oNodeLine.OriginalCode]) {
        contCompanies[oNodeLine.OriginalCode] = 0;
    }
    let code = contCompanies[oNodeLine.OriginalCode]++;
    while (oNodeCompanies.childNodes.filter(x => x.data.text == jsBeneficiario + code).length > 0) {
        code = contCompanies[oNodeLine.OriginalCode]++;
    }
    oNodeCompanies.appendChild({
        text: jsBeneficiario + code,
        leaf: true,
        Panel: 'formLineCompany',
        LineCode: oNodeLine.data.LineCode,
        CompanyCode: code,
        OriginalCode: jsBeneficiario + code
    });
    let company = {};
    company.CompanyCode = jsBeneficiario + code;
    let companies = oContract.contractline.filter(x => x.Code == oNodeLine.data.text)[0].Payees;
    if (!companies) {
        companies = [];
    }
    companies.push(company);
    oContract.contractline.filter(x => x.Code == oNodeLine.data.text)[0].Payees = companies;
    App.hdObjeto.setValue(JSON.stringify(oContract));
    oNodeCompanies.expand();
    let Node = oNodeCompanies.childNodes.filter(x => x.data.text == company.CompanyCode)[0];
    App.gridSecciones.getSelectionModel().select(Node);
}

var DelLine;

function deleteLine(sender) {
    DelLine = sender.parentElement.parentElement.getAttribute('code');
    Ext.Msg.alert(
        {
            title: jsEliminar,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxdeleteLine,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxdeleteLine(button) {
    if (button == 'yes' || button == 'si') {
        let oContract = JSON.parse(App.hdObjeto.value);
        let NodeLine = GetNodeLines().childNodes.filter(x => x.data.text == DelLine)[0];
        oContract.contractline = oContract.contractline.filter(function (value, index, arr) {
            return value.Code != DelLine;
        });
        GetNodeLines().removeChild(NodeLine);
        App.hdObjeto.setValue(JSON.stringify(oContract));
        ShowLines();
        ContractValid();
    }
}

function ShowLines() {
    if (App.storeLineType.data.items.length == 0) {
        App.storeLineType.reload();
    }
    if (App.storeTaxes.data.items.length == 0) {
        App.storeTaxes.reload();
    }
    let html = '';
    let Contenedor = document.getElementById('contLines');
    let oContract = JSON.parse(App.hdObjeto.value);
    html += CreateCardNew(jsAddNew, 'AddNewLine');
    if (!oContract.contractline) {
        oContract.contractline = [];
        App.hdObjeto.setValue(JSON.stringify(oContract));
    }
    oContract.contractline.forEach(x => {
        html += CreateCard(x.Code, 'ico-offering', [
            { title: 'Type', text: x.lineTypeCode },
            { title: 'Type', text: x.Value },
            { title: 'Type', text: x.CurrencyCode }
        ], 'SelectLine', [
            { btn: 'btnEdit', func: 'editLine' },
            { btn: 'btnPaye', func: 'addCompanyToLine' },
            { btn: 'btnDelete', func: 'deleteLine' }
        ]);
    });
    Contenedor.innerHTML = html;
}

//#endregion

//#region TapLine

function SelectLineType(sender) {
    let select = sender.getSelection();
    App.nbLineFrenuencia.disable();
    App.dtLineValidFrom.disable();
    App.dtLineNextPayment.disable();
    App.dtLineValidTo.disable();
    App.chkLineApplyRenewal.disable();
    App.chkLineApportionment.disable();
    App.chkLinePrepaid.disable();
    if (select) {
        if (select.data.Single) {
            App.nbLineFrenuencia.setValue(0);
            App.dtLineValidFrom.enable();
            App.dtLineNextPayment.reset();
            App.dtLineValidTo.reset();
            App.chkLineApplyRenewal.reset();
            App.chkLineApportionment.reset();
            App.chkLinePrepaid.reset();
        } else {
            App.nbLineFrenuencia.enable();
            App.dtLineValidFrom.enable();
            App.dtLineNextPayment.enable();
            App.dtLineValidTo.enable();
            App.chkLineApplyRenewal.enable();
            App.chkLineApportionment.enable();
            App.chkLinePrepaid.enable();
        }
    }
}

function SelectFechaInicioLinea(sender) {
    if (App.cmbLineType.getSelection().data.Single) {
        App.dtLineNextPayment.setValue(sender.value);
        App.dtLineValidTo.setValue(sender.value);
    }
    if (sender.value && sender.value != '') {
        App.dtLineNextPayment.setMinValue(sender.value);
        if (sender.value > App.dtLineNextPayment.value || (!App.dtLineNextPayment.value || App.dtLineNextPayment.value == '')) {
            App.dtLineValidTo.setMinValue(sender.value);
        }
    }
    if (sender.value > App.dtLineNextPayment.value) {
        App.dtLineNextPayment.reset();
    }
    if (sender.value > App.dtLineValidTo.value) {
        App.dtLineValidTo.reset();
    }
}
function SelectFechaProximoPago(sender) {
    if (sender.value && sender.value != '') {
        App.dtLineValidTo.setMinValue(sender.value);
    }
    if (sender.value > App.dtLineValidTo.value) {
        App.dtLineValidTo.reset();
    }
}

function ShowLine() {
    if (App.storeLineType.data.items.length == 0) {
        App.storeLineType.reload();
    }
    if (App.storeTaxes.data.items.length == 0) {
        App.storeTaxes.reload();
    }
    let oContract = JSON.parse(App.hdObjeto.value);
    if (oContract.StartDate) {
        let startDate = new Date(oContract.StartDate);
        App.dtLineValidTo.setMinValue(startDate);
        App.dtLineNextPayment.setMinValue(startDate);
        App.dtLineValidFrom.setMinValue(startDate);
    }
    if (oContract.FirstEndDate) {
        let endDate = new Date(oContract.FirstEndDate);
        App.dtLineValidTo.setMaxValue(endDate);
        App.dtLineNextPayment.setMaxValue(endDate);
        App.dtLineValidFrom.setMaxValue(endDate);
    }
    let oNode = GetNodeLineActual();
    let line = {};
    if (oContract.contractline.length > 0) {
        line = oContract.contractline.filter(x => x.Code == oNode.data.text)[0];
    }
    if (!line) {
        line = {}
    }
    App.txtLineCode.setValue(line.Code);
    App.txtLineDescripcion.setValue(line.Description);
    App.cmbLineType.setValue(line.lineTypeCode);
    SelectLineType(App.cmbLineType);
    App.nbLineFrenuencia.setValue(line.Frequency);
    App.nbLineValor.setValue(line.Value);
    App.cmbLineMoneda.setValue(line.CurrencyCode);
    App.dtLineValidFrom.setValue(TryGetDate(line.ValidFrom));
    App.dtLineValidTo.setValue(TryGetDate(line.ValidTo));
    App.dtLineNextPayment.setValue(TryGetDate(line.NextPaymentDate));
    App.chkLineApplyRenewal.setValue(line.ApplyRenewals);
    App.chkLineApportionment.setValue(line.Apportionment);
    App.chkLinePrepaid.setValue(line.Prepaid);
}

function SaveLine() {
    let oNode = GetNodeLineActual();
    let oContract = JSON.parse(App.hdObjeto.value);
    let line = {};
    if (oContract.contractline.length > 0) {
        line = oContract.contractline.filter(x => x.Code == oNode.data.text)[0];
    }
    if (!line) {
        line = {};
    }
    line.Code = App.txtLineCode.value;
    if (line.Code == '') {
        line.Code = oNode.data.OriginalCode;
    }
    oNode.data.text = line.Code;

    line.Description = App.txtLineDescripcion.value;
    line.lineTypeCode = App.cmbLineType.value;
    line.Frequency = App.nbLineFrenuencia.value;
    line.Value = App.nbLineValor.value;
    line.CurrencyCode = App.cmbLineMoneda.value;
    line.ValidFrom = (App.dtLineValidFrom.value == undefined || App.dtLineValidFrom.value == '') ? undefined : App.dtLineValidFrom.value.toDateString();
    line.ValidTo = (App.dtLineValidTo.value == undefined || App.dtLineValidTo.value == '') ? undefined : App.dtLineValidTo.value.toDateString();
    line.NextPaymentDate = (App.dtLineNextPayment.value == undefined || App.dtLineNextPayment.value == '') ? undefined : App.dtLineNextPayment.value.toDateString();
    line.ApplyRenewals = App.chkLineApplyRenewal.checked.toString();
    line.Apportionment = App.chkLineApportionment.checked.toString();
    line.Prepaid = App.chkLinePrepaid.checked.toString();
    if (oContract.contractline.length == 0 || !oContract.contractline.filter(x => x.Code == oNode.data.text).length) {
        oContract.contractline.push(line);
    } else {
        let index = oContract.contractline.findIndex(x => x.Code == oNode.data.text);
        oContract.contractline[index] = line;
    }
    oNode.save()
    App.hdObjeto.setValue(JSON.stringify(oContract));
}

//#endregion

//#region TapLinePriceReadjustment

function SelectFechaInicioReajuste(sender) {
    if (sender.value && sender.value != '') {
        App.dtLineReajustmentNextDate.setMinValue(sender.value);
        if (sender.value > App.dtLineReajustmentNextDate.value || (!App.dtLineReajustmentNextDate.value || App.dtLineReajustmentNextDate.value == '')) {
            App.dtLineReajustmentEndDate.setMinValue(sender.value);
        }
    }
    if (sender.value > App.dtLineReajustmentNextDate.value) {
        App.dtLineReajustmentNextDate.reset();
    }
    if (sender.value > App.dtLineReajustmentEndDate.value) {
        App.dtLineReajustmentEndDate.reset();
    }
}
function SelectFechaProximoReajuste(sender) {
    if (sender.value && sender.value != '') {
        App.dtLineReajustmentEndDate.setMinValue(sender.value);
    }
    if (sender.value > App.dtLineReajustmentEndDate.value) {
        App.dtLineReajustmentEndDate.reset();
    }
}

function SelPriceReadjustment(sender) {
    SeleccionarCombo(sender);
    App.cmbLineReajustmentCodeInflation.reset();
    App.cmbLineReajustmentCodeInflation.enable();
    App.nbLineReajustmentFixedAmount.reset();
    App.nbLineReajustmentFixedAmount.enable();
    App.nbLineReajustmentFixedPercentage.reset();
    App.nbLineReajustmentFixedPercentage.enable();
    App.nbLineReajustmentFrequency.reset();
    App.nbLineReajustmentFrequency.enable();
    App.dtLineReajustmentStartDate.reset();
    App.dtLineReajustmentStartDate.enable();
    App.dtLineReajustmentNextDate.reset();
    App.dtLineReajustmentNextDate.enable();
    App.dtLineReajustmentEndDate.reset();
    App.dtLineReajustmentEndDate.enable();
    if (sender.value == 'PCI') {
        App.nbLineReajustmentFixedAmount.disable();
        App.nbLineReajustmentFixedPercentage.disable();
    } else if (sender.value == 'FixedAmount') {
        App.cmbLineReajustmentCodeInflation.disable();
        App.nbLineReajustmentFixedPercentage.disable();
    } else if (sender.value == 'FixedPercentege') {
        App.cmbLineReajustmentCodeInflation.disable();
        App.nbLineReajustmentFixedAmount.disable();
    } else if (sender.value == 'WithoutIncrements') {
        App.cmbLineReajustmentCodeInflation.disable();
        App.nbLineReajustmentFixedAmount.disable();
        App.nbLineReajustmentFixedPercentage.disable();
        App.nbLineReajustmentFrequency.disable();
        App.dtLineReajustmentStartDate.disable();
        App.dtLineReajustmentNextDate.disable();
        App.dtLineReajustmentEndDate.disable();
    }
    if (!sender.value || sender.value == '') {
        App.cmbLineReajustmentCodeInflation.disable();
        App.nbLineReajustmentFixedAmount.disable();
        App.nbLineReajustmentFixedPercentage.disable();
        App.nbLineReajustmentFrequency.disable();
        App.dtLineReajustmentStartDate.disable();
        App.dtLineReajustmentNextDate.disable();
        App.dtLineReajustmentEndDate.disable();
    }
}

function ClearPriceReadjustment(sender) {
    RecargarCombo(sender);
    App.cmbLineReajustmentCodeInflation.enable();
    App.nbLineReajustmentFixedAmount.enable();
    App.nbLineReajustmentFixedPercentage.enable();
    App.nbLineReajustmentFrequency.enable();
    App.dtLineReajustmentStartDate.enable();
    App.dtLineReajustmentNextDate.enable();
    App.dtLineReajustmentEndDate.enable();
}

function ShowLinePriceReadjustment() {
    if (App.storeInflation.data.items.length == 0) {
        App.storeInflation.reload();
    }
    let oNode = GetNodeLineActual();
    let oContract = JSON.parse(App.hdObjeto.value);
    let line = {};
    if (oContract.contractline.length > 0) {
        line = oContract.contractline.filter(x => x.Code == oNode.data.text)[0];
    }
    if (!line) {
        line = {};
    }
    if (!line.PricesReadjustment) {
        line.PricesReadjustment = {};
    }
    if (line.ValidFrom) {
        let startDate = new Date(line.ValidFrom);
        App.dtLineReajustmentStartDate.setMinValue(startDate);
        App.dtLineReajustmentNextDate.setMinValue(startDate);
        App.dtLineReajustmentEndDate.setMinValue(startDate);
    }
    if (line.ValidTo) {
        let endDate = new Date(line.ValidTo);
        App.dtLineReajustmentStartDate.setMaxValue(endDate);
        App.dtLineReajustmentNextDate.setMaxValue(endDate);
        App.dtLineReajustmentEndDate.setMaxValue(endDate);
    }
    App.cmbLineReajustmentType.setValue(line.PricesReadjustment.Type);
    SelPriceReadjustment(App.cmbLineReajustmentType);
    App.cmbLineReajustmentCodeInflation.setValue(line.PricesReadjustment.CodeInflation);
    App.nbLineReajustmentFixedAmount.setValue(line.PricesReadjustment.FixedAmount);
    App.nbLineReajustmentFixedPercentage.setValue(line.PricesReadjustment.FixedPercentage);
    App.nbLineReajustmentFrequency.setValue(line.PricesReadjustment.Frequency);
    App.dtLineReajustmentStartDate.setValue(TryGetDate(line.PricesReadjustment.StartDate));
    App.dtLineReajustmentNextDate.setValue(TryGetDate(line.PricesReadjustment.NextDate));
    App.dtLineReajustmentEndDate.setValue(TryGetDate(line.PricesReadjustment.EndDate));
}

function SaveLinePriceReadjustment() {
    let oNode = GetNodeLineActual();
    let oContract = JSON.parse(App.hdObjeto.value);
    line = oContract.contractline.filter(x => x.Code == oNode.data.text)[0];
    if (!line) {
        line = {};
    }
    line.PricesReadjustment = {};
    line.PricesReadjustment.Type = App.cmbLineReajustmentType.value;
    line.PricesReadjustment.CodeInflation = App.cmbLineReajustmentCodeInflation.value;
    line.PricesReadjustment.FixedAmount = App.nbLineReajustmentFixedAmount.value;
    line.PricesReadjustment.FixedPercentage = App.nbLineReajustmentFixedPercentage.value;
    line.PricesReadjustment.Frequency = App.nbLineReajustmentFrequency.value;
    line.PricesReadjustment.StartDate = (App.dtLineReajustmentStartDate.value == undefined || App.dtLineReajustmentStartDate.value == '') ? undefined : App.dtLineReajustmentStartDate.value.toDateString();
    line.PricesReadjustment.NextDate = (App.dtLineReajustmentNextDate.value == undefined || App.dtLineReajustmentNextDate.value == '') ? undefined : App.dtLineReajustmentNextDate.value.toDateString();
    line.PricesReadjustment.EndDate = (App.dtLineReajustmentEndDate.value == undefined || App.dtLineReajustmentEndDate.value == '') ? undefined : App.dtLineReajustmentEndDate.value.toDateString();
    if (oContract.contractline.length == 0 || !oContract.contractline.filter(x => x.Code == oNode.data.text).length) {
        oContract.contractline.push(line);
    } else {
        let index = oContract.contractline.findIndex(x => x.Code == oNode.data.text);
        oContract.contractline[index] = line;
    }
    App.hdObjeto.setValue(JSON.stringify(oContract));
}

//#endregion

//#region Companies

function AddNewLineCompany() {
    let oNodeLine = GetNodeLineActual();
    let oContract = JSON.parse(App.hdObjeto.value);
    if (!contCompanies[oNodeLine.OriginalCode]) {
        contCompanies[oNodeLine.OriginalCode] = 0;
    }
    let code = contCompanies[oNodeLine.OriginalCode]++;
    while (GetNodeCompanies().childNodes.filter(x => x.data.text == jsBeneficiario + code).length > 0) {
        code = contCompanies[oNodeLine.OriginalCode]++;
    }
    GetNodeCompanies().appendChild({
        text: jsBeneficiario + code,
        leaf: true,
        Panel: 'formLineCompany',
        LineCode: ActualLine,
        CompanyCode: code,
        OriginalCode: jsBeneficiario + code
    });
    let company = {};
    company.CompanyCode = jsBeneficiario + code;
    let companies = oContract.contractline.filter(x => x.Code == oNodeLine.data.text)[0].Payees;
    if (!companies) {
        companies = [];
    }
    companies.push(company);
    oContract.contractline.filter(x => x.Code == oNodeLine.data.text)[0].Payees = companies;
    App.hdObjeto.setValue(JSON.stringify(oContract));
    ShowCompanies();
    ContractValid();
}

function SelectCompany(sender) {
    let code = sender.getAttribute('code');
    GetNodeCompanies().expand();
    let Node = GetNodeCompanies().childNodes.filter(x => x.data.text == code)[0];
    App.gridSecciones.getSelectionModel().select(Node);
}

function editCompany(sender) {
    let code = sender.parentElement.parentElement.getAttribute('code');
    GetNodeCompanies().expand();
    let Node = GetNodeCompanies().childNodes.filter(x => x.data.text == code)[0];
    App.gridSecciones.getSelectionModel().select(Node);
}

var DelComp;

function deleteCompany(sender) {
    DelComp = sender.parentElement.parentElement.getAttribute('code');
    Ext.Msg.alert(
        {
            title: jsEliminar,
            msg: jsMensajeEliminar,
            buttons: Ext.Msg.YESNO,
            fn: ajaxdeleteCompany,
            icon: Ext.MessageBox.QUESTION
        });
}

function ajaxdeleteCompany(button) {
    if (button == 'yes' || button == 'si') {
        let oContract = JSON.parse(App.hdObjeto.value);
        let NodeLine = GetNodeLineActual();
        let Node = GetNodeCompanies().childNodes.filter(x => x.data.text == DelComp)[0];
        oContract.contractline.filter(x => x.Code = NodeLine.data.text)[0].Payees = oContract.contractline.filter(x => x.Code = NodeLine.data.text)[0].Payees.filter(function (value, index, arr) {
            return value.CompanyCode != DelComp;
        });
        GetNodeCompanies().removeChild(Node);
        App.hdObjeto.setValue(JSON.stringify(oContract));
        ShowCompanies();
        ContractValid();
    }
}


function ShowCompanies() {
    if (App.storePaymentMethods.data.items.length == 0) {
        App.storePaymentMethods.reload();
    }
    if (App.storeBankAccounts.data.items.length == 0) {
        App.storeBankAccounts.reload();
    }
    let oNodeLine = GetNodeLineActual();
    let html = '';
    let Contenedor = document.getElementById('contCompanies');
    let oContract = JSON.parse(App.hdObjeto.value);
    html += CreateCardNew(jsAddNew, 'AddNewLineCompany');
    let companies = oContract.contractline.filter(x => x.Code == oNodeLine.data.text)[0].Payees;
    if (!companies) {
        companies = [];
    }
    companies.forEach(x => {
        html += CreateCard(x.CompanyCode, 'ico-offering', [
            { text: x.PaymentMethodCode },
            { text: x.BankAcountCode },
            { text: x.currencyCode },
            { text: x.Percent }
        ], 'SelectCompany', [
            { btn: 'btnEdit', func: 'editCompany' },
            { btn: 'btnDelete', func: 'deleteCompany' }
        ]);
    });
    Contenedor.innerHTML = html;
}

//#endregion

//#region TapLineCompany
var LineCall;
function SelCompany(sender, callback) {
    LineCall = callback;
    SeleccionarCombo(sender);
    App.cmbLineCompanyPaymentMethod.reset();
    App.cmbLineCompanyBankAcount.reset();
    App.storePaymentMethods.clearFilter();
    App.storeBankAccounts.clearFilter();
    if (sender.getSelection()) {
        var data = sender.getSelection().data;
        App.storePaymentMethods.filterBy(function (node) {
            var valido = false;
            if (data.LinkedPaymentMethodCode.filter(x => x.PaymentMethodCode == node.data.Code.toString()).length > 0) {
                valido = true;
            }
            return valido;
        });
        CargarStoresSerie([App.storeBankAccounts], function Fin(fin) {
            if (fin) {
                data.LinkedBankAccount.forEach(x => {
                    App.storeBankAccounts.add({
                        Code: x.Code,
                        Name: x.BankCode
                    });
                });
                LineCall(true, null);
            }
        });
    } else if (LineCall) {
        LineCall(true, null);
    }
}

function ClearCompany(sender) {
    RecargarCombo(sender);
    App.cmbLineCompanyPaymentMethod.reset();
    App.cmbLineCompanyBankAcount.reset();
    App.storePaymentMethods.clearFilter();
    App.storePaymentMethods.reload();
    App.storeBankAccounts.clearFilter();
    App.storeBankAccounts.reload();
}

function ShowLineCompany() {    
    if (App.storePaymentMethods.data.items.length == 0) {
        App.storePaymentMethods.reload();
    }
    if (App.storeBankAccounts.data.items.length == 0) {
        App.storeBankAccounts.reload();
    }
    let oNodeLine = GetNodeLineActual();
    let oNodeCompany = GetNodeLineCompanyActual();
    let oContract = JSON.parse(App.hdObjeto.value);
    let line = {};
    let company = {};
    if (oContract.contractline.length > 0) {
        line = oContract.contractline.filter(x => x.Code == oNodeLine.data.text)[0];
    } if (!line) {
        line = {};
    } if (!line.Payees) {
        line.Payees = [];
    }
    if (line.Payees.length > 0) {
        company = line.Payees.filter(x => x.CompanyCode == oNodeCompany.data.text)[0];
    }
    if (!company) {
        company = {};
    }
    App.cmbLineCompany.setValue(company.CompanyCode);
    if (App.storeCompany.data.items.length == 0) {
        showLoadMask(App.formLineCompany, function (load) {
            CargarStoresSerie([App.storeCompany], function FinS(finS) {
                if (finS && App.cmbLineCompany.getSelection()) {
                    SelCompany(App.cmbLineCompany, function Fin(fin) {
                        if (fin) {
                            App.cmbLineCompanyPaymentMethod.setValue(company.PaymentMethodCode);
                            App.cmbLineCompanyBankAcount.setValue(company.BankAcountCode);
                            App.cmbLineCompanyCurrency.setValue(company.currencyCode);
                            App.nbLineCompanyPercent.setValue(company.Percent);
                            load.hide();
                        }
                    });
                } else {
                    load.hide();
                }
            })
        });
    } else {
        SelCompany(App.cmbLineCompany, function Fin(fin) {
            if (fin && App.cmbLineCompany.getSelection()) {
                App.cmbLineCompanyPaymentMethod.setValue(company.PaymentMethodCode);
                App.cmbLineCompanyBankAcount.setValue(company.BankAcountCode);
                App.cmbLineCompanyCurrency.setValue(company.currencyCode);
                App.nbLineCompanyPercent.setValue(company.Percent);
            }
        });
    }    
}

function SaveLineCompany() {
    let oNodeLine = GetNodeLineActual();
    let oNodeCompany = GetNodeLineCompanyActual();
    let oContract = JSON.parse(App.hdObjeto.value);
    let line = {};
    let company = {};
    if (oContract.contractline.length > 0) {
        line = oContract.contractline.filter(x => x.Code == oNodeLine.data.text)[0];
    } if (!line) {
        line = {};
    } if (!line.Payees) {
        line.Payees = [];
    }
    company.CompanyCode = App.cmbLineCompany.value;
    company.PaymentMethodCode = App.cmbLineCompanyPaymentMethod.value;
    company.BankAcountCode = App.cmbLineCompanyBankAcount.value;
    company.currencyCode = App.cmbLineCompanyCurrency.value;
    company.Percent = App.nbLineCompanyPercent.value;
    if (line.Payees.length == 0 || !line.Payees.filter(x => x.CompanyCode == oNodeCompany.data.text).length) {
        line.Payees.push(company);
    } else {
        let index = line.Payees.findIndex(x => x.CompanyCode == oNodeCompany.data.text);
        line.Payees[index] = company;
    }
    if (company.CompanyCode == '' || company.CompanyCode == null) {
        company.CompanyCode = oNodeCompany.data.OriginalCode;
    }
    oNodeCompany.data.text = company.CompanyCode;

    oNodeCompany.save()
    App.hdObjeto.setValue(JSON.stringify(oContract));
}

//#endregion

//#region Taxes

function ShowLineTaxes() {
    ClearfilterTaxes();
    ShowTaxes();
    let oNode = GetNodeLineActual();
    let oContract = JSON.parse(App.hdObjeto.value);
    let line = {};
    if (oContract.contractline.length > 0) {
        line = oContract.contractline.filter(x => x.Code == oNode.data.text)[0];
    }
    if (!line.ContractLineTaxes) {
        line.ContractLineTaxes = [];
    }

    for (let item of document.getElementsByClassName('Taxes')) {
        let Taxe = line.ContractLineTaxes.filter(x => x.TaxCode == item.getAttribute("code"))[0];
        if (Taxe) {
            item.setAttribute("activo", "true");
        } else {
            item.setAttribute("activo", "false");
        }
    }
    App.hdObjeto.setValue(JSON.stringify(oContract));
}

function SaveLineTaxes() {
    let oNode = GetNodeLineActual();
    let oContract = JSON.parse(App.hdObjeto.value);
    let line = {};
    if (oContract.contractline.length > 0) {
        line = oContract.contractline.filter(x => x.Code == oNode.data.text)[0];
    }
    line.ContractLineTaxes = [];
    let Taxes;

    for (let item of document.getElementsByClassName('Taxes')) {
        if (item.getAttribute("activo") == "true") {
            Taxes = {
                TaxCode: item.getAttribute("code")
            };
            line.ContractLineTaxes.push(Taxes);
        }
    }
    let index = oContract.contractline.findIndex(x => x.Code == oNode.data.text);
    oContract.contractline[index] = line;

    App.hdObjeto.setValue(JSON.stringify(oContract));
}

function ShowTaxes() {
    let oContract = JSON.parse(App.hdObjeto.value);
    let divContainer = '';
    let oNode = GetNodeLineActual();
    let line = {};
    if (oContract.contractline.length > 0) {
        line = oContract.contractline.filter(x => x.Code == oNode.data.text)[0];
    }
    if (!line.ContractLineTaxes) {
        line.ContractLineTaxes = [];
    }

    App.storeTaxes.data.items.forEach(x => {
        divContainer += '<div class="contCard Taxes" code="' + x.data.Code + '" onclick="selectLineTaxes(this, event)" activo="' + (line.ContractLineTaxes.filter(c => c.TaxCode == x.data.Code).length > 0) + '">' +
            '<div class="checkCard"><div></div></div>' +
            '<span class="nameCard">' + x.data.Name + '</span>' +
            '</div>';
    });
    document.getElementById('cardTaxes').innerHTML = divContainer;
}

function selectLineTaxes(sender, event) {
    event.stopPropagation();

    if (sender.getAttribute("activo") == "true") {
        sender.setAttribute("activo", "false");
    }
    else {
        sender.setAttribute("activo", "true");
    }
}

function ClearfilterTaxes() {
    App.txtFiltroTaxes.reset();
    App.txtFiltroTaxes.getTrigger(1).hide();
    App.txtFiltroTaxes.getTrigger(0).show();
    filterTaxes(App.txtFiltroTaxes);
}

function filterTaxes(sender) {
    if (sender.value == '') {
        sender.getTrigger(1).hide();
        sender.getTrigger(0).show();
    } else {
        sender.getTrigger(0).hide();
        sender.getTrigger(1).show();
    }
    var $taxes = $('.Taxes');
    var filterVal = sender.value;

    if (!filterVal || filterVal == '') {
        $taxes.show();
    } else {
        $taxes.hide().filter(function (x, e) { return e.getElementsByClassName('nameCard')[0].innerHTML.toLowerCase().includes(filterVal.toLowerCase()) }).show()
    }
}

//#endregion

function TryGetDate(value) {
    if (isNaN(Date.parse(value)) || new Date(value).getYear() < 0) {
        return undefined;
    } else {
        return new Date(value);
    }
}

function calcMonths(d2, d1) {
    var months;
    months = (d2.getFullYear() - d1.getFullYear()) * 12;
    months -= d1.getMonth();
    months += d2.getMonth();
    return months <= 0 ? 0 : months;
}

function RecargarCombo(sender, registro, index) {
    recargarCombos([sender], function Fin(fin) {
        if (fin) {
            sender.reset();
            sender.cancelFocus()
        }
    });
}

function SeleccionarCombo(sender, registro, index) {
    sender.getTrigger(0).show();
}

function CreateCard(title, image, Items, f, btns) {
    let html = '';

    html += '<div class="contCard" code="' + title + '" ondblclick="' + f + '(this)">' +
        '<div class="contHeader">' +
        '<object data="/ima/' + image + '.svg" class="imgTitle"> </object>' +
        '<h4 class="itemCardTitle">' + title + '</h4>' +
        '</div>' +
        '<div class="contDetails">';

    Items.forEach(x => {
        html += '<div class="contLine">' +
            '<div class="iconText">' +
            '<span class="iconSpan">' + ((!x.text) ? '' : x.text) + '</span>' +
            '</div>' +
            '</div>';
    });

    html += '</div>' +
        '<div class="btnAcciones">';
    btns.forEach(x => {
        html += '<div class="' + x.btn + ' Hidden" onclick="' + x.func + '(this)">' +
            '</div>';
    });
    html += '</div ></div>';

    return html;
}

function CreateCardNew(title, f) {
    let html = '';

    html += '<div class="contCard" onclick="' + f + '(this)">' +
        '<div class="contAddNew">' +
        '<div class="icon-add"></div >' +
        '<div class="textAdd">' + title + '</div>' +
        '</div>';


    html += '</div>';

    return html;
}