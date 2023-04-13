<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormContratos.aspx.cs" Inherits="TreeCore.Modulos.Contracts.FormContratos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden runat="server" ID="hdObjeto" />
            <ext:Hidden runat="server" ID="hdContractCode" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <DocumentReady Handler="App.gridSecciones.getSelectionModel().select(App.gridSecciones.getRootNode().childNodes[0])" />
                </Listeners>
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store ID="storeMonedas" runat="server" AutoLoad="true" OnReadData="storeMonedas_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Symbol" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Code" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeGrupos" runat="server" AutoLoad="true" OnReadData="storeGrupos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeTipos" runat="server" AutoLoad="true" OnReadData="storeTipos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeEstados" runat="server" AutoLoad="true" OnReadData="storeEstados_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeLineType" runat="server" AutoLoad="false" OnReadData="storeLineType_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="Single" Type="Boolean" />
                            <ext:ModelField Name="Recurrent" Type="Boolean" />
                            <ext:ModelField Name="Income" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeCompany" runat="server" AutoLoad="false" OnReadData="storeCompany_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="CurrencyCode" />
                            <ext:ModelField Name="LinkedBankAccount" Type="Object" />
                            <ext:ModelField Name="LinkedPaymentMethodCode" Type="Object" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeTaxes" runat="server" AutoLoad="false" OnReadData="storeTaxes_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeInflation" runat="server" AutoLoad="false" OnReadData="storeInflation_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storePaymentMethods" runat="server" AutoLoad="false" OnReadData="storePaymentMethods_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeBankAccounts" runat="server" AutoLoad="false" OnReadData="storeBankAccounts_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="Code"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>



            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Cls="vwContenedor"
                Layout="FitLayout">
                <Items>
                    <ext:Panel runat="server"
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel runat="server"
                                ID="pnSecciones"
                                Region="West"
                                Collapsible="true"
                                Width="300"
                                Header="false"
                                CollapseDirection="Right"
                                CollapseMode="Header"
                                Collapsed="false"
                                Hidden="false"
                                Layout="FitLayout">
                                <DockedItems>
                                </DockedItems>
                                <Items>
                                    <ext:TreePanel
                                        runat="server"
                                        ID="gridSecciones"
                                        SelectionMemory="false"
                                        Cls="treeGridGeneral"
                                        Header="true"
                                        EnableColumnHide="false"
                                        Title="Sections"
                                        AnchorVertical="100%"
                                        Region="Center"
                                        Hidden="false"
                                        RootVisible="false"
                                        AriaRole="main">
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:TreeColumn
                                                    ID="colSeccCode"
                                                    runat="server"
                                                    Filterable="false"
                                                    Flex="1"
                                                    Sortable="true"
                                                    Hidden="false"
                                                    DataIndex="text">
                                                    <Filter>
                                                        <ext:StringFilter />
                                                    </Filter>
                                                </ext:TreeColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <Listeners>
                                        </Listeners>
                                        <SelectionModel>
                                            <ext:TreeSelectionModel runat="server"
                                                ID="selSeccion"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="SelectSeccion" />
                                                </Listeners>
                                            </ext:TreeSelectionModel>
                                        </SelectionModel>
                                    </ext:TreePanel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" Region="Center" Cls="pnForm" Layout="FitLayout">
                                <DockedItems>
                                    <ext:Toolbar Height="0" runat="server" Dock="Top" Cls="tbFlotante">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnGuardarContratos"
                                                Cls="btnSave me-2 mt-3"
                                                Width="100"
                                                Text="<%$ Resources:Comun, strGuardar %>"
                                                Focusable="false"
                                                Disabled="true"
                                                PressedCls="none"
                                                Hidden="false">
                                                <Listeners>
                                                    <Click Fn="SaveContract" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar Height="0" runat="server" Dock="Bottom" Cls="tbFlotante">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnCancelar"
                                                Cls="btnSecundary me-2"
                                                Width="120"
                                                Text="<%$ Resources:Comun, strCancelar %>"
                                                Focusable="false"
                                                PressedCls="none"
                                                Hidden="false">
                                                <Listeners>
                                                    <Click Fn="CancelForm"></Click>
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnContinuar"
                                                Cls="btnSave me-2"
                                                Width="100"
                                                Text="<%$ Resources:Comun, strSiguiente %>"
                                                Focusable="false"
                                                PressedCls="none"
                                                Hidden="false">
                                                <Listeners>
                                                    <Click Fn="SiguienteSeccion"></Click>
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server" ID="formDataModel" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowDataModel" />
                                            <Hide Fn="SaveDataModel" />
                                        </Listeners>
                                        <Items>
                                            <ext:Panel runat="server" Cls="ctForm" Header="true" Title="<%$ Resources:Comun, strIdentificacion %>">
                                                <Items>
                                                    <ext:TextField ID="txtCodigo"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        Cls="required"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtNombre"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtDescripcion"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel runat="server" Cls="ctForm" Header="true" Title="<%$ Resources:Comun, strGeneral %>">
                                                <Items>
                                                    <ext:ComboBox
                                                        ID="cmbTipoContrato"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        ForceSelection="true"
                                                        StoreID="storeTipos"
                                                        FieldLabel="<%$ Resources:Comun, strTipo %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:ComboBox
                                                        ID="cmbEstadoContrato"
                                                        runat="server"
                                                        Cls="required"
                                                        ForceSelection="true"
                                                        Editable="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeEstados"
                                                        FieldLabel="<%$ Resources:Comun, strEstado %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:ComboBox
                                                        ID="cmbGrupoContrato"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ForceSelection="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeGrupos"
                                                        FieldLabel="<%$ Resources:Comun, strGrupos %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:ComboBox
                                                        ID="cmbMonedaContrato"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ForceSelection="true"
                                                        ValueField="Code"
                                                        DisplayField="Code"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeMonedas"
                                                        FieldLabel="<%$ Resources:Comun, strMoneda %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Code}</h5>
                                                                        <p class="subTitle">{Symbol}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel runat="server" Cls="ctForm" Header="true" Title="<%$ Resources:Comun, strValidez  %>">
                                                <Items>
                                                    <ext:DateField ID="dtFechaFirma"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strFechaFirma %>"
                                                        MaxLength="50"
                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                        LabelAlign="Left"
                                                        Editable="false"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Handler="ContractValid(this); SelFechaFirma(this);" Buffer="250" />
                                                        </Listeners>
                                                    </ext:DateField>
                                                    <ext:DateField ID="dtFechaInicio"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strFechaInicio %>"
                                                        MaxLength="50"
                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                        LabelAlign="Left"
                                                        Editable="false"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Handler="ContractValid(this); SelFechaInicio(this);" Buffer="250" />
                                                        </Listeners>
                                                    </ext:DateField>
                                                    <ext:NumberField ID="nbDuracionContratos"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strDuracion %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        AllowDecimals="false"
                                                        MinValue="0"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Handler="ContractValid(this); CambioFinContrato(this);" />
                                                        </Listeners>
                                                    </ext:NumberField>
                                                    <ext:DateField ID="dtFechaFin"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strFechaFin %>"
                                                        MaxLength="50"
                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                        LabelAlign="Left"
                                                        Editable="false"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Handler="ContractValid(this); CambioFinContrato(this);" Buffer="250" />
                                                        </Listeners>
                                                    </ext:DateField>
                                                    <ext:Checkbox ID="chkCierraAlExpirar"
                                                        runat="server"
                                                        Cls="newCheckbox"
                                                        FieldLabel="<%$ Resources:Comun, strCerradoExpirar %>"
                                                        LabelAlign="Left">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:Checkbox>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>

                                    <ext:Panel runat="server" ID="formRenewal" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowRenewal" />
                                            <Hide Fn="SaveRenewal" />
                                        </Listeners>
                                        <Items>
                                            <ext:Panel runat="server" Cls="ctForm" Header="true" Title="<%$ Resources:Comun, strProrroga%>">
                                                <Items>
                                                    <ext:ComboBox
                                                        ID="cmbTipoRenewal"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ForceSelection="true"
                                                        ValueField="Code"
                                                        DisplayField="Code"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        FieldLabel="<%$ Resources:Comun, strTipoProrroga %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <Items>
                                                            <ext:ListItem Text="<%$ Resources:Comun, strAuto %>" Value="Auto" />
                                                            <ext:ListItem Text="<%$ Resources:Comun, strOptional %>" Value="Optional" />
                                                            <ext:ListItem Text="<%$ Resources:Comun, strPreviousNegotiation %>" Value="Previous negotiation" />
                                                        </Items>
                                                        <Listeners>
                                                            <Select Fn="SelRenewal" />
                                                            <TriggerClick Fn="ClearRenewal" />
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:NumberField ID="nbFrecuenciaRenewal"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strFrecuenciaReajustePrecio %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        AllowDecimals="false"
                                                        MinValue="1"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Handler="ContractValid(this); calcRenewal(this);" Buffer="250" />
                                                        </Listeners>
                                                    </ext:NumberField>
                                                    <ext:NumberField ID="nbTotalRenewal"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strNumeroProrrogas %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        AllowDecimals="false"
                                                        MinValue="1"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Handler="ContractValid(this); calcRenewal(this);" Buffer="250" />
                                                        </Listeners>
                                                    </ext:NumberField>
                                                    <ext:NumberField ID="nbActualRenewal"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strNumeroRenovacion %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        AllowDecimals="false"
                                                        MinValue="0"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="">
                                                        <Listeners>
                                                        </Listeners>
                                                    </ext:NumberField>
                                                    <ext:DateField ID="dtFechaRenewal"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strFechaExpiracionProrroga %>"
                                                        MaxLength="50"
                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Editable="false"
                                                        Cls="">
                                                        <Listeners>
                                                        </Listeners>
                                                    </ext:DateField>
                                                    <ext:DateField ID="dtFechaFinRenewal"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strExpiracionContrato %>"
                                                        MaxLength="50"
                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                        LabelAlign="Left"
                                                        Editable="false"
                                                        Cls="">
                                                        <Listeners>
                                                        </Listeners>
                                                    </ext:DateField>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>

                                    <ext:Panel runat="server" ID="formLines" Cls="content100" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowLines" />
                                        </Listeners>
                                        <DockedItems>
                                        </DockedItems>
                                        <Content>
                                            <div id="contLines" class="containerStyle">
                                            </div>
                                        </Content>
                                    </ext:Panel>

                                    <ext:Panel runat="server" ID="formLine" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowLine" />
                                            <Hide Fn="SaveLine" />
                                        </Listeners>
                                        <Items>
                                            <ext:Panel runat="server" Cls="ctForm" Header="true" Title="<%$ Resources:Comun, strGeneral %>">
                                                <Items>
                                                    <ext:TextField ID="txtLineCode"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:TextField ID="txtLineDescripcion"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                    <ext:ComboBox
                                                        ID="cmbLineType"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ForceSelection="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeLineType"
                                                        FieldLabel="<%$ Resources:Comun, strTipoLinea %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Handler="ContractValid(this); SelectLineType(this);" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel runat="server" Cls="ctForm" Header="true" Title="<%$ Resources:Comun, strValidez %>">
                                                <Items>
                                                    <ext:NumberField ID="nbLineFrenuencia"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strFrecuencia %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MinValue="0"
                                                        DecimalPrecision="2"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:NumberField>
                                                    <ext:NumberField ID="nbLineValor"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strValor %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MinValue="0"
                                                        DecimalPrecision="2"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:NumberField>
                                                    <ext:ComboBox
                                                        ID="cmbLineMoneda"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ValueField="Code"
                                                        ForceSelection="true"
                                                        DisplayField="Code"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeMonedas"
                                                        FieldLabel="<%$ Resources:Comun, strMoneda %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Code}</h5>
                                                                        <p class="subTitle">{Symbol}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:DateField ID="dtLineValidFrom"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strValidoDesde %>"
                                                        MaxLength="50"
                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Editable="false"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Handler="ContractValid(this); SelectFechaInicioLinea(this);" Buffer="250" />
                                                        </Listeners>
                                                    </ext:DateField>
                                                    <ext:DateField ID="dtLineValidTo"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strValidoHasta %>"
                                                        MaxLength="50"
                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Editable="false"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Handler="ContractValid(this);" Buffer="250" />
                                                        </Listeners>
                                                    </ext:DateField>
                                                    <ext:DateField ID="dtLineNextPayment"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strFechaProximoPago %>"
                                                        MaxLength="50"
                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Editable="false"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Handler="ContractValid(this); SelectFechaProximoPago(this);" Buffer="250" />
                                                        </Listeners>
                                                    </ext:DateField>
                                                    <ext:Checkbox ID="chkLineApplyRenewal"
                                                        runat="server"
                                                        Cls="newCheckbox"
                                                        FieldLabel="<%$ Resources:Comun, strAplicaRenovacion %>"
                                                        LabelAlign="Left">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:Checkbox>
                                                    <ext:Checkbox ID="chkLineApportionment"
                                                        runat="server"
                                                        Cls="newCheckbox"
                                                        FieldLabel="<%$ Resources:Comun, strProrrateo %>"
                                                        LabelAlign="Left">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:Checkbox>
                                                    <ext:Checkbox ID="chkLinePrepaid"
                                                        runat="server"
                                                        Cls="newCheckbox"
                                                        FieldLabel="<%$ Resources:Comun, strPagoAnticipado %>"
                                                        LabelAlign="Left">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:Checkbox>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>

                                    <ext:Panel runat="server" ID="formLinePriceReadjustment" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowLinePriceReadjustment" />
                                            <Hide Fn="SaveLinePriceReadjustment" />
                                        </Listeners>
                                        <Items>
                                            <ext:Panel runat="server" Cls="ctForm" Header="true" Title="<%$ Resources:Comun, strReajustes %>">
                                                <Items>
                                                    <ext:ComboBox
                                                        ID="cmbLineReajustmentType"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ForceSelection="true"
                                                        ValueField="Code"
                                                        DisplayField="Code"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        FieldLabel="<%$ Resources:Comun, strTipoReajustePrecio %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <Items>
                                                            <ext:ListItem Text="<%$ Resources:Comun, strPCI %>" Value="PCI" />
                                                            <ext:ListItem Text="<%$ Resources:Comun, strFixedAmount %>" Value="FixedAmount" />
                                                            <ext:ListItem Text="<%$ Resources:Comun, strFixedPercentege %>" Value="FixedPercentege" />
                                                            <ext:ListItem Text="<%$ Resources:Comun, strWithoutIncrements %>" Value="WithoutIncrements" />
                                                        </Items>
                                                        <Listeners>
                                                            <Select Fn="SelPriceReadjustment" />
                                                            <TriggerClick Fn="ClearPriceReadjustment" />
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:ComboBox
                                                        ID="cmbLineReajustmentCodeInflation"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ValueField="Code"
                                                        ForceSelection="true"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeInflation"
                                                        FieldLabel="<%$ Resources:Comun, strCodigoInflacion %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:NumberField ID="nbLineReajustmentFixedAmount"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strCantidadFija %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MinValue="0"
                                                        DecimalPrecision="2"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:NumberField>
                                                    <ext:NumberField ID="nbLineReajustmentFixedPercentage"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strPorcentajeFijo %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MinValue="0"
                                                        DecimalPrecision="2"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:NumberField>
                                                    <ext:NumberField ID="nbLineReajustmentFrequency"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strFrecuencia %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MinValue="0"
                                                        DecimalPrecision="2"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:NumberField>
                                                    <ext:DateField ID="dtLineReajustmentStartDate"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strFechaInicio %>"
                                                        MaxLength="50"
                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Editable="false"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Handler="ContractValid(this); SelectFechaInicioReajuste(this);" Buffer="250" />
                                                        </Listeners>
                                                    </ext:DateField>
                                                    <ext:DateField ID="dtLineReajustmentNextDate"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strFechaProximaRevision %>"
                                                        MaxLength="50"
                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Editable="false"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Handler="ContractValid(this); SelectFechaProximoReajuste(this);" Buffer="250" />
                                                        </Listeners>
                                                    </ext:DateField>
                                                    <ext:DateField ID="dtLineReajustmentEndDate"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strFechaFinReajuste %>"
                                                        MaxLength="50"
                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                        LabelAlign="Left"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Editable="false"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Handler="ContractValid(this);" Buffer="250" />
                                                        </Listeners>
                                                    </ext:DateField>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>

                                    <ext:Panel runat="server" ID="formCompanies" Cls="content100" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowCompanies" />
                                        </Listeners>
                                        <DockedItems>
                                        </DockedItems>
                                        <Content>
                                            <div id="contCompanies" class="containerStyle">
                                            </div>
                                        </Content>
                                    </ext:Panel>

                                    <ext:Panel runat="server" ID="formLineCompany" Scrollable="Vertical" Hidden="true">
                                        <Listeners>
                                            <Show Fn="ShowLineCompany" />
                                            <Hide Fn="SaveLineCompany" />
                                        </Listeners>
                                        <Items>
                                            <ext:Panel runat="server" Cls="ctForm" Header="true" Title="<%$ Resources:Comun, strBeneficiario %>">
                                                <Items>
                                                    <ext:ComboBox
                                                        ID="cmbLineCompany"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ForceSelection="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeCompany"
                                                        FieldLabel="<%$ Resources:Comun, strEntidad %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SelCompany" />
                                                            <TriggerClick Fn="ClearCompany" />
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:ComboBox
                                                        ID="cmbLineCompanyPaymentMethod"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ForceSelection="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storePaymentMethods"
                                                        FieldLabel="<%$ Resources:Comun, strMetodoPago %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:ComboBox
                                                        ID="cmbLineCompanyBankAcount"
                                                        runat="server"
                                                        Cls="required"
                                                        Editable="true"
                                                        ForceSelection="true"
                                                        ValueField="Code"
                                                        DisplayField="Name"
                                                        QueryMode="Local"
                                                        StoreID="storeBankAccounts"
                                                        LabelAlign="Left"
                                                        FieldLabel="<%$ Resources:Comun, strBankAccounts %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Name}</h5>
                                                                        <p class="subTitle">{Code}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:ComboBox
                                                        ID="cmbLineCompanyCurrency"
                                                        runat="server"
                                                        Cls="required"
                                                        ForceSelection="true"
                                                        Editable="true"
                                                        ValueField="Code"
                                                        DisplayField="Code"
                                                        QueryMode="Local"
                                                        LabelAlign="Left"
                                                        StoreID="storeMonedas"
                                                        FieldLabel="<%$ Resources:Comun, strMoneda %>"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="cmbItem">
                                                                        <h5 class="title">{Code}</h5>
                                                                        <p class="subTitle">{Symbol}</p>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:NumberField ID="nbLineCompanyPercent"
                                                        runat="server"
                                                        FieldLabel="<%$ Resources:Comun, strPorcentaje %>"
                                                        MaxLength="50"
                                                        LabelAlign="Left"
                                                        MinValue="0"
                                                        MaxValue="100"
                                                        DecimalPrecision="2"
                                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                        Cls="required">
                                                        <Listeners>
                                                            <Change Fn="ContractValid" Buffer="250" />
                                                        </Listeners>
                                                    </ext:NumberField>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>

                                    <ext:Panel runat="server" ID="formLineTaxes" Scrollable="Vertical" Hidden="true">

                                        <Listeners>
                                            <Show Fn="ShowLineTaxes" />
                                            <Hide Fn="SaveLineTaxes" />
                                        </Listeners>
                                        <Items>
                                            <ext:Container runat="server" Cls="ctForm">
                                                <Items>
                                                    <ext:Toolbar runat="server" Dock="Top">
                                                        <Items>
                                                            <ext:ToolbarFill />
                                                            <ext:TextField
                                                                ID="txtFiltroTaxes"
                                                                Cls="txtSearchD"
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                WidthSpec="400px"
                                                                EnableKeyEvents="true">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Search" />
                                                                    <ext:FieldTrigger Handler="ClearfilterTaxes();" Hidden="true" Icon="clear" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <Change Fn="filterTaxes" Buffer="250" />
                                                                </Listeners>
                                                            </ext:TextField>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Items>
                                                <Content>
                                                    <div id="cardTaxes" class="cardMultiSelect"></div>
                                                </Content>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>

                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
