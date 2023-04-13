<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkFlowsCustomFields.aspx.cs" Inherits="TreeCore.ModWorkFlow.WorkFlowsCustomFields" %>

<%@ Register Src="/Componentes/AtributosConfiguracion.ascx" TagName="AtributosConfiguracion" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="/Modulos/Inventory/css/styleInventarioCategorias.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/jquery.amsify.suggestags.js"></script>
    <link href="../../CSS/amsify.suggestags.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Componentes/js/Atributos.js"></script>
    <script type="text/javascript" src="/Componentes/js/AtributosConfiguracion.js"></script>
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden runat="server" ID="hdCliID" />
            <ext:Hidden runat="server" ID="hdAtrID" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" DisableViewState="true" ShowWarningOnAjaxFailure="false">
                <Listeners>
                    <AjaxRequestException Fn="winErrorTimeout" />
                </Listeners>
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store runat="server"
                ID="storeClientes"
                AutoLoad="true"
                OnReadData="storeClientes_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="ClienteID">
                        <Fields>
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="Cliente" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Cliente" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <Load Handler="CargarStores();" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storePrincipal"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storePrincipal_Refresh"
                RemoteSort="false">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                    <DataChanged Fn="CargarBuscadorPredictivoCustomFields" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="CustomFieldID">
                        <Fields>
                            <ext:ModelField Name="CustomFieldID" Type="Int" />
                            <ext:ModelField Name="CoreAtributoConfiguracionID" Type="Int" />
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="TipoDato" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTiposDatos"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeTiposDatos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="TipoDatoID">
                        <Fields>
                            <ext:ModelField Name="TipoDatoID" Type="Int" />
                            <ext:ModelField Name="TipoDato" />
                            <ext:ModelField Name="Codigo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="TipoDato" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="400"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextField runat="server"
                                ID="txtNombreCampo"
                                meta:resourceKey="txtNombreCampo"
                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                MaxLength="40"
                                WidthSpec="100%"
                                LabelAlign="Top"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:ComboBox runat="server"
                                ID="cmbTiposDatos"
                                meta:resourceKey="cmbTiposDatos"
                                StoreID="storeTiposDatos"
                                DisplayField="TipoDato"
                                ValueField="TipoDatoID"
                                WidthSpec="100%"
                                LabelAlign="Top"
                                QueryMode="Local"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                FieldLabel="<%$ Resources:Comun, strTipoDato %>">
                                <Listeners>
                                    <Select Fn="SeleccionarCombo" />
                                    <TriggerClick Fn="RecargarCombo" />
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
                        <Listeners>
                            <ValidityChange Handler="FormularioValido(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport ID="vwResp" runat="server">
                <Listeners>
                    <AfterRender Handler="resizeTwoPanels();"></AfterRender>
                    <AfterLayout Handler="showPanelsByWindowSize()"></AfterLayout>
                    <Resize Handler="showPanelsByWindowSize(); resizeTwoPanels();" Buffer="50"></Resize>
                </Listeners>
                <Items>
                    <ext:Toolbar runat="server" ID="tbFiltrosYSliders" Dock="Top" Cls="tbGrey tbNoborder " Hidden="true" Layout="HBoxLayout" Flex="1">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Toolbar runat="server"
                                ID="tbSliders"
                                Dock="Top"
                                Hidden="false"
                                MinHeight="36"
                                Cls="tbGrey tbNoborder">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnPrev"
                                        IconCls="ico-prev-w"
                                        Cls="btnMainSldr SliderBtn"
                                        Handler="loadPanelByBtns('Prev'); resizeTwoPanels();"
                                        Disabled="true">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnNext"
                                        IconCls="ico-next-w"
                                        Cls="SliderBtn"
                                        Handler="loadPanelByBtns('Next'); resizeTwoPanels();"
                                        Disabled="false">
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </Items>
                    </ext:Toolbar>
                    <ext:Container ID="ctSlider" runat="server">
                        <Items>
                            <ext:Container ID="ctMain1" runat="server" MaxWidth="300"
                                Flex="12" Region="West">
                                <Items>
                                    <ext:GridPanel
                                        Hidden="false"
                                        Cls="gridPanel TreePnl"
                                        ID="gridCustomFields"
                                        meta:resourceKey="gridCustomFields"
                                        runat="server"
                                        Scrollable="Vertical"
                                        ForceFit="true"
                                        StoreID="storePrincipal"
                                        Title="<%$ Resources:Comun, strCustomField %>"
                                        EnableColumnHide="false"
                                        EnableColumnMove="false"
                                        EnableColumnResize="false">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" Dock="Top" Cls="toolBarInventario">
                                                <Items>
                                                    <ext:ComboBox runat="server"
                                                        ID="cmbClientes"
                                                        meta:resourceKey="cmbClientes"
                                                        StoreID="storeClientes"
                                                        DisplayField="Cliente"
                                                        ValueField="ClienteID"
                                                        Cls="comboGrid pos-boxGrid"
                                                        WidthSpec="100%"
                                                        LabelAlign="Top"
                                                        QueryMode="Local"
                                                        Hidden="true"
                                                        EmptyText="<%$ Resources:Comun, cmbClientes.EmptyText %>"
                                                        FieldLabel="<%$ Resources:Comun, cmbClientes.FieldLabel %>">
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCliente" />
                                                            <TriggerClick Fn="RecargarClientes" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:ToolbarFill />
                                                </Items>
                                            </ext:Toolbar>

                                            <ext:Toolbar runat="server" Dock="Top" ID="toolbarButtons" Layout="ColumnLayout" Hidden="false" StyleSpec="border-style: hidden !important;">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnAnadir"
                                                        Cls="btnAnadir"
                                                        AriaLabel="Añadir"
                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                        Handler="AgregarEditar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnEditar"
                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                        Cls="btnEditar"
                                                        Handler="MostrarEditar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnEliminar"
                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                        Cls="btnEliminar"
                                                        Handler="Eliminar();" />
                                                    <ext:Button runat="server"
                                                        ID="btnActivar"
                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                        Cls="btnActivar"
                                                        Handler="Activar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnActivos"
                                                        ToolTip="<%$ Resources:Comun, btnActivos.ToolTip %>"
                                                        Cls="btn-toggleGrid"
                                                        EnableToggle="true"
                                                        Pressed="true"
                                                        Width="42"
                                                        Handler="VerActivos();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnRefrescar"
                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                        Cls="btnRefrescar maxHeiInv"
                                                        Handler="Refrescar();" />
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar
                                                runat="server"
                                                ID="toolBarFiltro"
                                                Cls="tlbGrid">
                                                <Items>
                                                    <ext:TextField
                                                        ID="txtSearchCustomFields"
                                                        Cls="txtSearchC"
                                                        runat="server"
                                                        EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                        WidthSpec="100%"
                                                        EnableKeyEvents="true">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Search" />
                                                            <ext:FieldTrigger Handler="ClearfilterCustomFields" Hidden="true" Icon="clear" />
                                                        </Triggers>
                                                        <Listeners>
                                                            <%--<Render Fn="FieldSearch" Buffer="250" />--%>
                                                            <Change Fn="filterCustomFields" Buffer="250" />
                                                        </Listeners>
                                                    </ext:TextField>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Listeners>
                                            <AfterRender Fn="SetMaxHeightSuperior" />
                                        </Listeners>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colActivo"
                                                    DataIndex="Activo"
                                                    Align="Center"
                                                    Cls="col-activo"
                                                    Hidden="true"
                                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                    Width="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colNombre"
                                                    meta:resourceKey="colNombre"
                                                    DataIndex="Codigo"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    MinWidth="100"
                                                    Flex="1" />
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="GridRowSelect"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelect" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="ctMain2" runat="server" Region="Center"
                                Flex="1">
                                <Items>
                                    <ext:Panel
                                        Hidden="false"
                                        Title="<%$ Resources:Comun, strConfigurador %>"
                                        runat="server"
                                        ForceFit="true"
                                        ID="pnConfigurador"
                                        meta:resourceKey="pnConfigurador"
                                        Cls="gridPanelInventario pnConfiguradorAtributos contenedorField"
                                        Scrollable="Vertical"
                                        OverflowX="Auto">
                                        <Items>
                                        </Items>
                                        <Listeners>
                                            <AfterRender Fn="SetMaxHeightSuperior" />
                                        </Listeners>
                                    </ext:Panel>
                                </Items>
                                <Content>
                                    <local:AtributosConfiguracion
                                        ID="AtributosConfiguracion"
                                        runat="server" />
                                </Content>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
