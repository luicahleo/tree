<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusinessProcess.aspx.cs" Inherits="TreeCore.ModWorkFlow.pages.BusinessProcess" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <!--<script type="text/javascript" src="../../JS/common.js"></script>-->
</head>
<body>
    <link href="/CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="/CSS/amsify.suggestags.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Scripts/jquery.amsify.suggestags.js"></script>
    <script type="text/javascript" src="js/BusinessProcess.js"></script>
    <link href="css/styleWorkFlows.css" rel="stylesheet" type="text/css" />

    <form id="form1" runat="server">
        <div>

            <%--INICIO HIDDEN --%>

            <ext:Hidden runat="server" ID="hdCliID" />
            <ext:Hidden runat="server" ID="hdListaWorkFlows" />
            <ext:Hidden runat="server" ID="hdBusinessProcess" />

            <%--FIN HIDDEN --%>

            <%--INICIO HIDDEN --%>

            <ext:Store runat="server"
                ID="storeWorkFlows"
                AutoLoad="true"
                OnReadData="storeWorkFlows_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreWorkFlowID">
                        <Fields>
                            <ext:ModelField Name="CoreWorkFlowID" Type="Int" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Nombre" Type="String" />
                            <ext:ModelField Name="Publico" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="ClienteID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeWorkFlowsEstados"
                AutoLoad="false"
                OnReadData="storeWorkFlowsEstados_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreEstadoID">
                        <Fields>
                            <ext:ModelField Name="CoreEstadoID" Type="Int" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="NombreEstado" Type="String" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreEstado" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeBusinessProcessTipos"
                AutoLoad="false"
                OnReadData="storeBusinessProcessTipos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreBusinessProcessTipoID">
                        <Fields>
                            <ext:ModelField Name="CoreBusinessProcessTipoID" Type="Int" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Nombre" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeBusinessProcessWorkFlowsAdd"
                AutoLoad="false"
                OnReadData="storeBusinessProcessWorkFlowsAdd_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreBusinessProcessWorkflowID">
                        <Fields>
                            <ext:ModelField Name="CoreBusinessProcessWorkflowID" Type="Int" />
                            <ext:ModelField Name="CoreBusinessProcessID" Type="Int" />
                            <ext:ModelField Name="CoreWorkflowID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="CoreBusinessProcessID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storePrincipal"
                AutoLoad="true"
                OnReadData="storePrincipal_Refresh"
                RemoteSort="false">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreBusinessProcessID">
                        <Fields>
                            <ext:ModelField Name="CoreBusinessProcessID" Type="Int" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Nombre" Type="String" />
                            <ext:ModelField Name="CoreBusinessProcessTipoID" Type="Int" />
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="EstadoInicialID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeWorkflowBP"
                AutoLoad="false"
                OnReadData="storeWorkflowBP_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreWorkflowID">
                        <Fields>
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Nombre" Type="String" />
                            <%--<ext:ModelField Name="Inicial" Type="Boolean" />--%>
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN HIDDEN --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                </Listeners>
            </ext:ResourceManager>

            <ext:Window ID="winGestionBusinessProcess"
                runat="server"
                Title="Window Form"
                WidthSpec="70vw"
                MaxWidth="750"
                HeightSpec="80vh"
                MaxHeight="500"
                Modal="true"
                Centered="true"
                Resizable="false"
                Layout="FitLayout"
                Cls="winForm-resp"
                Hidden="true">
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <Items>
                    <ext:FormPanel ID="FormBusiness"
                        Cls="formGris formResp"
                        runat="server"
                        OverflowY="Auto"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Container runat="server" ID="ctForm" PaddingSpec="16 32" Cls="ctForm-resp ctForm-resp-col3">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtCode"
                                        FieldLabel="Code"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        WidthSpec="100%"
                                        Cls="item-form"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Listeners>
                                            <Change Fn="FormularioValido" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:TextField runat="server"
                                        ID="txtName"
                                        FieldLabel="Name"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        WidthSpec="100%"
                                        Cls="item-form"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Listeners>
                                            <Change Fn="FormularioValido" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:ComboBox runat="server"
                                        ID="cmbType"
                                        Cls="item-form"
                                        WidthSpec="100%"
                                        StoreID="storeBusinessProcessTipos"
                                        ValueField="CoreBusinessProcessTipoID"
                                        DisplayField="Nombre"
                                        AllowBlank="false"
                                        FieldLabel="Type"
                                        LabelAlign="Top">
                                        <Listeners>
                                            <Select Fn="SeleccionarTipo" />
                                            <TriggerClick Fn="RecargarTipo" />
                                            <Change Fn="FormularioValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:Container runat="server"
                                        ID="cnBPAmsifyWF"
                                        PaddingSpec="8 32"
                                        Cls="ContenedorCombo dosColumnas amsiBP">
                                        <Content>
                                            <div class="form-group" id="pnWorkFLows" style="height: inherit;">
                                                <input type="text"
                                                    class="form-control"
                                                    name="workflow"
                                                    style="height: inherit;">
                                            </div>
                                        </Content>
                                    </ext:Container>
                                    <ext:ComboBox runat="server"
                                        ID="cmbWorkflow"
                                        FieldLabel="Workflow"
                                        Cls="item-form itemForm-BusinessMandatory"
                                        WidthSpec="100%"
                                        AllowBlank="false"
                                        Disabled="true"
                                        LabelAlign="Top">
                                        <Listeners>
                                            <Select Fn="SeleccionarWorkFlow" />
                                            <TriggerClick Fn="RecargarWorkflow" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbStatus"
                                        FieldLabel="Status"
                                        Cls="item-form"
                                        StoreID="storeWorkFlowsEstados"
                                        DisplayField="NombreEstado"
                                        ValueField="CoreEstadoID"
                                        WidthSpec="100%"
                                        AllowBlank="false"
                                        Disabled="true"
                                        LabelAlign="Top">
                                        <Listeners>
                                            <Select Fn="SeleccionarWorkFlowEstados" />
                                            <TriggerClick Fn="RecargarWorkflowEstados" />
                                            <Change Fn="FormularioValido" />
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
                            </ext:Container>
                        </Items>
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="tbSaveForm"
                                Cls="greytb"
                                Dock="Bottom"
                                Padding="20">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button runat="server"
                                        ID="btnCerrar"
                                        Width="100px"
                                        meta:resourceKey="btnPrev"
                                        IconCls="ico-close"
                                        Cls="btn-secondary-winForm"
                                        Text="<%$ Resources:Comun, strCerrar %>"
                                        Focusable="false"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="cerrarWinGestion()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnAgregar"
                                        Cls="btn-ppal-winForm"
                                        Disabled="true"
                                        Text="<%$ Resources:Comun, strGuardar %>"
                                        PressedCls="none">
                                        <Listeners>
                                            <Click Fn="guardarCambios" />
                                        </Listeners>
                                    </ext:Button>

                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Viewport ID="vwResp"
                runat="server"
                Layout="FitLayout"
                Flex="1"
                OverflowY="auto">
                <Items>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain"
                                BodyCls="tbGrey">
                                <DockedItems>
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
                                                        Handler="loadPanelByBtns('Prev');"
                                                        Disabled="true">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnNext"
                                                        IconCls="ico-next-w"
                                                        Cls="SliderBtn"
                                                        Handler="loadPanelByBtns('Next');"
                                                        Disabled="false">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server" ID="wrapComponenteCentral" Layout="HBoxLayout" BodyCls="tbGrey" MaxWidth="1700">
                                        <Listeners>
                                            <%--<AfterRender Handler="showPanelsByWindowSize();"></AfterRender>
                                            <Resize Handler="showPanelsByWindowSize();"></Resize>--%>
                                        </Listeners>
                                        <LayoutConfig>
                                            <ext:HBoxLayoutConfig Align="Stretch"></ext:HBoxLayoutConfig>
                                        </LayoutConfig>
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tbPathBP" Cls="tlbBusinessProcess">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnEditWF"
                                                        Cls="btnEditar"
                                                        AriaLabel="Editar"
                                                        Hidden="true"
                                                        Handler=""
                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>" />
                                                    <ext:Button runat="server"
                                                        Cls="noBorder boldPath"
                                                        Hidden="false"
                                                        Height="32"
                                                        FocusCls="none"
                                                        Text="Business Process"
                                                        ID="btnBusinessProcess">
                                                        <Listeners>
                                                            <Click Fn="NavegacionTabsBPorW"></Click>
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Label runat="server"
                                                        ID="lbRutaCategoria"
                                                        Cls="btnNext"
                                                        Hidden="false">
                                                    </ext:Label>
                                                    <ext:Button runat="server"
                                                        Cls="noBorder"
                                                        Hidden="false"
                                                        Height="32"
                                                        FocusCls="none"
                                                        Text="Workflow"
                                                        ID="btnWorkflow">
                                                        <Listeners>
                                                            <Click Fn="NavegacionTabsBPorW"></Click>
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <ext:GridPanel ID="gridBusinessProcess"
                                                runat="server"
                                                Header="false"
                                                Hidden="false"
                                                Flex="12"
                                                EnableColumnHide="false"
                                                SelectionMemory="false"
                                                Region="West"
                                                ForceFit="true"
                                                MaxWidth="360"
                                                StoreID="storePrincipal"
                                                Cls="gridPanel"
                                                OverflowX="Hidden"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar ID="tblCliente" runat="server" Dock="Top" Cls="toolBarInventario" Hidden="true">
                                                        <Items>
                                                            <ext:ComboBox runat="server"
                                                                ID="cmbClientes"
                                                                meta:resourceKey="cmbClientes"
                                                                DisplayField="Cliente"
                                                                ValueField="ClienteID"
                                                                Cls="comboGrid pos-boxGrid"
                                                                WidthSpec="100%"
                                                                LabelAlign="Top"
                                                                QueryMode="Local"
                                                                EmptyText="<%$ Resources:Comun, cmbClientes.EmptyText %>"
                                                                FieldLabel="<%$ Resources:Comun, cmbClientes.FieldLabel %>">
                                                                <Listeners>
                                                                    <%--<Select Fn="SeleccionarCliente" />--%>
                                                                    <%--<TriggerClick Fn="RecargarClientes" />--%>
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
                                                    <ext:Toolbar runat="server" ID="tlbBase" Dock="top" Cls="tlbGrid" OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnAnadir"
                                                                Cls="btnAnadir"
                                                                AriaLabel="Añadir"
                                                                Handler="btnAgregar();"
                                                                ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>" />
                                                            <ext:Button runat="server"
                                                                ID="btnEditar"
                                                                Cls="btnEditar"
                                                                AriaLabel="Editar"
                                                                Disabled="true"
                                                                Handler="ajaxEditar();"
                                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>" />
                                                            <ext:Button runat="server"
                                                                ID="btnEliminar"
                                                                ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                Disabled="true"
                                                                meta:resourceKey="btnEliminar"
                                                                Cls="btnEliminar"
                                                                Handler="Eliminar();" />
                                                            <ext:Button runat="server"
                                                                ID="btnActivar"
                                                                Disabled="true"
                                                                ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                                Cls="btnActivar"
                                                                Handler="btnActivar();">
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
                                                                Cls="btnRefrescar"
                                                                AriaLabel="Refrescar"
                                                                Handler="Refrescar();"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>" />
                                                            <ext:Button runat="server"
                                                                ID="btnDescargar"
                                                                Cls="btnDescargar"
                                                                AriaLabel="Descargar"
                                                                Hidden="true"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Handler="">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnPlay"
                                                                Cls="btnPlay"
                                                                AriaLabel="Reproducir"
                                                                Disabled="true"
                                                                ToolTip="Reproducir"
                                                                Handler="">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="toolBarFiltro"
                                                        PaddingSpec="6px 8px"
                                                        Cls="tlbGrid search">
                                                        <Items>
                                                            <ext:TextField
                                                                ID="txtSearchWorkFlows"
                                                                Cls="txtSearchC"
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                WidthSpec="100%"
                                                                EnableKeyEvents="true">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Search" />
                                                                    <%--<ext:FieldTrigger Handler="ClearfilterWorkFlows" Hidden="true" Icon="clear" />--%>
                                                                </Triggers>
                                                                <Listeners>
                                                                    <%--<Render Fn="FieldSearch" Buffer="250" />--%>
                                                                    <%--<Change Fn="filterWorkFlows" Buffer="250" />--%>
                                                                </Listeners>
                                                            </ext:TextField>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:Column runat="server"
                                                            ID="colActivo"
                                                            DataIndex="Activo"
                                                            Align="Center"
                                                            Cls="col-activo"
                                                            Hidden="true"
                                                            ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                            MinWidth="45"
                                                            MaxWidth="45">
                                                            <Renderer Fn="DefectoRender" />
                                                        </ext:Column>
                                                        <ext:Column runat="server"
                                                            Text="<%$ Resources:Comun, strNombre %>"
                                                            MinWidth="100"
                                                            DataIndex="Nombre"
                                                            Flex="1"
                                                            ID="colName" />
                                                        <ext:Column runat="server"
                                                            Text="<%$ Resources:Comun, strCodigo %>"
                                                            MinWidth="100"
                                                            DataIndex="Codigo"
                                                            Flex="1"
                                                            CellCls="linkNumber"
                                                            ID="colCode" />
                                                        <ext:WidgetColumn ID="ColMore"
                                                            runat="server"
                                                            Cls="NoOcultar col-More"
                                                            TdCls="btnMore"
                                                            DataIndex=""
                                                            Align="Center"
                                                            Text="<%$ Resources:Comun, strMas %>"
                                                            Hidden="false"
                                                            MinWidth="55"
                                                            MaxWidth="55">
                                                            <Widget>
                                                                <ext:Button runat="server"
                                                                    StyleSpec="margin: 0 auto;"
                                                                    OverCls="Over-btnMore"
                                                                    PressedCls="Pressed-none"
                                                                    FocusCls="Focus-none"
                                                                    Cls="btnColMoreX">
                                                                    <Listeners>
                                                                        <%--<Click Fn="MostrarPanelMoreInfo" />--%>
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Widget>
                                                        </ext:WidgetColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server"
                                                        ID="GridRowSelectBusinessProcess"
                                                        Mode="Single">
                                                        <Listeners>
                                                            <Select Fn="Grid_RowSelect" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <Plugins>
                                                    <ext:GridFilters runat="server"
                                                        ID="gridFilters"
                                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                        meta:resourceKey="GridFilters">
                                                    </ext:GridFilters>
                                                    <ext:CellEditing runat="server"
                                                        ClicksToEdit="2" />
                                                </Plugins>
                                            </ext:GridPanel>

                                            <ext:GridPanel ID="grdWorkFlowBP"
                                                runat="server"
                                                Header="false"
                                                Hidden="true"
                                                Flex="12"
                                                EnableColumnHide="false"
                                                SelectionMemory="false"
                                                Region="West"
                                                ForceFit="true"
                                                MaxWidth="360"
                                                StoreID="storeWorkflowBP"
                                                Cls="gridPanel"
                                                OverflowX="Hidden"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="toolBar5"
                                                        PaddingSpec="6px 8px"
                                                        Cls="tlbGrid search">
                                                        <Items>
                                                            <ext:TextField
                                                                ID="TextField4"
                                                                Cls="txtSearchC"
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                WidthSpec="100%"
                                                                EnableKeyEvents="true">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Search" />
                                                                    <%--<ext:FieldTrigger Handler="ClearfilterWorkFlows" Hidden="true" Icon="clear" />--%>
                                                                </Triggers>
                                                                <Listeners>
                                                                    <%--<Render Fn="FieldSearch" Buffer="250" />--%>
                                                                    <%--<Change Fn="filterWorkFlows" Buffer="250" />--%>
                                                                </Listeners>
                                                            </ext:TextField>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:Column runat="server"
                                                            ID="Column1"
                                                            DataIndex="Activo"
                                                            Align="Center"
                                                            Cls="col-activo"
                                                            ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                            MinWidth="45"
                                                            MaxWidth="45">
                                                            <Renderer Fn="DefectoRender" />
                                                        </ext:Column>
                                                        <ext:Column runat="server"
                                                            Text="<%$ Resources:Comun, strNombre %>"
                                                            MinWidth="100"
                                                            DataIndex="Nombre"
                                                            Flex="1"
                                                            ID="Column2" />
                                                        <ext:Column runat="server"
                                                            Text="<%$ Resources:Comun, strCodigo %>"
                                                            MinWidth="100"
                                                            DataIndex="Codigo"
                                                            Flex="1"
                                                            CellCls="linkNumber"
                                                            ID="Column3" />
                                                        <ext:Column runat="server"
                                                            ID="Column4"
                                                            DataIndex="Inicial"
                                                            Align="Center"
                                                            Cls=""
                                                            ToolTip="WorkFlow Inicial"
                                                            MinWidth="45"
                                                            MaxWidth="45">
                                                            <Renderer Fn="InitialRender" />
                                                        </ext:Column>
                                                        <ext:WidgetColumn ID="WidgetColumn1"
                                                            runat="server"
                                                            Cls="NoOcultar col-More"
                                                            TdCls="btnMore"
                                                            DataIndex=""
                                                            Align="Center"
                                                            Text="<%$ Resources:Comun, strMas %>"
                                                            Hidden="false"
                                                            MinWidth="55"
                                                            MaxWidth="55">
                                                            <Widget>
                                                                <ext:Button runat="server"
                                                                    StyleSpec="margin: 0 auto;"
                                                                    OverCls="Over-btnMore"
                                                                    PressedCls="Pressed-none"
                                                                    FocusCls="Focus-none"
                                                                    Cls="btnColMoreX">
                                                                    <Listeners>
                                                                        <%--<Click Fn="MostrarPanelMoreInfo" />--%>
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Widget>
                                                        </ext:WidgetColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server"
                                                        ID="GridRowSelectWorkFlow"
                                                        Mode="Single">
                                                        <Listeners>
                                                            <Select Fn="Grid_RowSelectWF" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <Plugins>
                                                    <ext:GridFilters runat="server"
                                                        ID="gridFilters1"
                                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                        meta:resourceKey="GridFilters">
                                                    </ext:GridFilters>
                                                    <ext:CellEditing runat="server"
                                                        ClicksToEdit="2" />
                                                </Plugins>
                                            </ext:GridPanel>

                                            <ext:Panel runat="server"
                                                ID="pnCol1"
                                                Cls="grdNoHeader"
                                                Region="Center"
                                                Flex="3"
                                                Layout="HBoxLayout"
                                                MinWidth="200"
                                                OverflowX="Auto"
                                                OverflowY="Auto">
                                                <LayoutConfig>
                                                    <ext:HBoxLayoutConfig Align="Stretch"></ext:HBoxLayoutConfig>
                                                </LayoutConfig>
                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar4"
                                                        Dock="Top"
                                                        Height="42"
                                                        Padding="0"
                                                        Cls="tlbGrid c-Grid border1px">
                                                        <Items>
                                                            <ext:Label runat="server"
                                                                ID="lbHeaderWorkFlow"
                                                                Cls="HeaderLblVisor"
                                                                PaddingSpec="0 16" />
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Items>
                                                    <ext:TreePanel runat="server"
                                                        ID="Panel1"
                                                        Cls="visorInsidePn borderBottom"
                                                        Header="false"
                                                        Region="West"
                                                        BodyPaddingSummary="16 0"
                                                        Flex="3"
                                                        Hidden="true"
                                                        SingleExpand="true"
                                                        HideHeaders="true"
                                                        MinWidth="200"
                                                        OverflowX="Auto"
                                                        OverflowY="Auto">
                                                        <LayoutConfig>
                                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                        </LayoutConfig>
                                                        <DockedItems>
                                                            <ext:Toolbar runat="server" ID="Toolbar2" Dock="top" Cls="tlbGrid tlbWorkflowBP" OverflowHandler="Scroller">
                                                                <Items>
                                                                    <ext:Label runat="server"
                                                                        Cls="lbWFInitial"
                                                                        IconCls="ico-position"
                                                                        Text="Start point selected name and state" />
                                                                    <ext:ToolbarFill />
                                                                    <ext:Label runat="server"
                                                                        Cls="btnWFInitial"
                                                                        Text="Add Step" />
                                                                    <ext:Button runat="server"
                                                                        ID="Button3"
                                                                        Cls="btnAnadir"
                                                                        AriaLabel="Añadir"
                                                                        Handler="btnAgregar();"
                                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>" />
                                                                </Items>
                                                            </ext:Toolbar>
                                                            <ext:Toolbar
                                                                runat="server"
                                                                ID="toolBar1"
                                                                PaddingSpec="6px 8px"
                                                                Cls="tlbGrid search tlbWorkflowBP">
                                                                <Items>
                                                                    <ext:TextField
                                                                        ID="TextField1"
                                                                        Cls="txtSearchC"
                                                                        runat="server"
                                                                        EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                        WidthSpec="100%"
                                                                        MaxWidth="400"
                                                                        EnableKeyEvents="true">
                                                                        <Triggers>
                                                                            <ext:FieldTrigger Icon="Search" />
                                                                            <%--<ext:FieldTrigger Handler="ClearfilterWorkFlows" Hidden="true" Icon="clear" />--%>
                                                                        </Triggers>
                                                                        <Listeners>
                                                                            <%--<Render Fn="FieldSearch" Buffer="250" />--%>
                                                                            <%--<Change Fn="filterWorkFlows" Buffer="250" />--%>
                                                                        </Listeners>
                                                                    </ext:TextField>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <ColumnModel runat="server">
                                                            <Columns>
                                                                <ext:Column runat="server"
                                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                                    MinWidth="100"
                                                                    DataIndex="Nombre"
                                                                    Flex="1"
                                                                    ID="Column6" />
                                                                <ext:WidgetColumn ID="WidgetColumn5"
                                                                    runat="server"
                                                                    Cls=""
                                                                    DataIndex=""
                                                                    Align="Center"
                                                                    Flex="1"
                                                                    Text=""
                                                                    Hidden="false"
                                                                    MinWidth="180">
                                                                    <Widget>
                                                                        <ext:ComboBox meta:resourceKey="cmbProyecto"
                                                                            ID="cmbEstado"
                                                                            runat="server"
                                                                            DisplayField="Tipo"
                                                                            Editable="true"
                                                                            Scrollable="Vertical"
                                                                            OverflowX="Hidden"
                                                                            WidthSpec="100%"
                                                                            MaxWidth="280"
                                                                            Cls="txtSearchC"
                                                                            EmptyText="Estado"
                                                                            AllowBlank="true">
                                                                            <Items>
                                                                                <ext:ListItem Text="Estado 1" />
                                                                                <ext:ListItem Text="Estado 2" />
                                                                                <ext:ListItem Text="Estado 3" />
                                                                                <ext:ListItem Text="Estado 4" />
                                                                            </Items>
                                                                            <Triggers>
                                                                                <ext:FieldTrigger meta:resourceKey="LimpiarLista"
                                                                                    Icon="Clear"
                                                                                    QTip="Limpiar Lista" />
                                                                                <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                                                    IconCls="ico-reload"
                                                                                    QTip="Recargar Lista" />
                                                                            </Triggers>
                                                                        </ext:ComboBox>
                                                                    </Widget>
                                                                </ext:WidgetColumn>
                                                                <ext:WidgetColumn ID="WidgetColumn2"
                                                                    runat="server"
                                                                    Cls=""
                                                                    Align="Center"
                                                                    Text=""
                                                                    Hidden="false"
                                                                    MinWidth="55"
                                                                    MaxWidth="55">
                                                                    <Widget>
                                                                        <ext:Button runat="server"
                                                                            ID="Button1"
                                                                            Cls="btn-trans btnAnadir"
                                                                            AriaLabel="Añadir"
                                                                            FocusCls="none"
                                                                            PressedCls="none"
                                                                            ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                                            Handler="">
                                                                        </ext:Button>
                                                                    </Widget>
                                                                </ext:WidgetColumn>
                                                                <ext:WidgetColumn ID="WidgetColumn3"
                                                                    runat="server"
                                                                    Cls=""
                                                                    Align="Center"
                                                                    Text=""
                                                                    Hidden="false"
                                                                    MinWidth="55"
                                                                    MaxWidth="55">
                                                                    <Widget>
                                                                        <ext:Button runat="server"
                                                                            ID="Button2"
                                                                            FocusCls="none"
                                                                            PressedCls="none"
                                                                            ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                            Cls="btn-trans btnEliminar"
                                                                            Handler="" />
                                                                    </Widget>
                                                                </ext:WidgetColumn>
                                                                <ext:WidgetColumn ID="WidgetColumn4"
                                                                    runat="server"
                                                                    Cls=""
                                                                    Align="Center"
                                                                    Text=""
                                                                    Hidden="false"
                                                                    MinWidth="55"
                                                                    MaxWidth="55">
                                                                    <Widget>
                                                                        <ext:Button runat="server"
                                                                            ID="Button4"
                                                                            FocusCls="none"
                                                                            PressedCls="none"
                                                                            ToolTip="Parar"
                                                                            Cls="btn-trans btnStop"
                                                                            Handler="" />
                                                                    </Widget>
                                                                </ext:WidgetColumn>
                                                            </Columns>
                                                        </ColumnModel>
                                                    </ext:TreePanel>
                                                    <ext:Panel runat="server"
                                                        ID="Panel2"
                                                        Cls="grdNoHeader visorInsidePn borderBottom"
                                                        Region="East"
                                                        Flex="3"
                                                        Hidden="true"
                                                        Layout="VBoxLayout"
                                                        MinWidth="200"
                                                        OverflowX="Auto"
                                                        OverflowY="Auto">
                                                        <LayoutConfig>
                                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                        </LayoutConfig>
                                                        <DockedItems>
                                                            <ext:Toolbar runat="server"
                                                                ID="tbNavNAside"
                                                                Dock="Top"
                                                                Cls="tlbWorkflowBP"
                                                                Hidden="false"
                                                                Padding="11"
                                                                OverflowHandler="Scroller"
                                                                Flex="1">
                                                                <Items>
                                                                    <ext:HyperlinkButton runat="server"
                                                                        ID="lnkInformation"
                                                                        Cls="lnk-navView lnk-noLine navActivo"
                                                                        meta:resourceKey="lnkInformation"
                                                                        Text="Information">
                                                                        <Listeners>
                                                                            <Click Fn="NavegacionTabsWBP"></Click>
                                                                        </Listeners>
                                                                    </ext:HyperlinkButton>
                                                                    <ext:HyperlinkButton runat="server"
                                                                        ID="lnkRoles"
                                                                        Cls="lnk-navView lnk-noLine"
                                                                        meta:resourceKey="lnkRoles"
                                                                        Text="ROLES">
                                                                        <Listeners>
                                                                            <Click Fn="NavegacionTabsWBP"></Click>
                                                                        </Listeners>
                                                                    </ext:HyperlinkButton>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <Items>
                                                            <ext:Panel ID="ctMain1BP" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1 border1px" BodyPadding="16" Hidden="false">
                                                                <DockedItems>
                                                                    <ext:Toolbar
                                                                        runat="server"
                                                                        ID="toolBar3"
                                                                        Hidden="false"
                                                                        Cls="tlbGrid">
                                                                        <Items>
                                                                            <ext:TextField
                                                                                ID="TextField2"
                                                                                Cls="txtSearchC search"
                                                                                runat="server"
                                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                                LabelWidth="50"
                                                                                WidthSpec="100%"
                                                                                MaxWidth="400"
                                                                                EnableKeyEvents="true">
                                                                                <Triggers>
                                                                                    <ext:FieldTrigger Icon="Search" />
                                                                                    <ext:FieldTrigger Icon="clear" />
                                                                                </Triggers>
                                                                                <Listeners>
                                                                                    <Render Fn="FieldSearch" Buffer="250" />
                                                                                    <Change Fn="FiltrarColumnas" Buffer="250" />
                                                                                </Listeners>
                                                                            </ext:TextField>
                                                                        </Items>
                                                                    </ext:Toolbar>
                                                                </DockedItems>
                                                                <Items>
                                                                    <ext:Container ID="hugeCt3" runat="server" Layout="FitLayout">
                                                                        <Items>
                                                                            <ext:DataView
                                                                                runat="server"
                                                                                ID="DataView2"
                                                                                SingleSelect="true"
                                                                                AnchorVertical="100%"
                                                                                ItemSelector="div.informationbp">
                                                                                <Tpl runat="server">
                                                                                    <Html>
                                                                                        <div id="items-ct">
                                                                                            <%--<tpl for=".">--%>
                                                                                                <div class="contBusinessProcessInfo" id="contBusinessProcess">
                                                                                                    <div class="contLabel">
                                                                                                        <div class="contLabelWorkFlowI" onclick="clickOnWorkFlowInfo(this)">Informacion 1</div>
                                                                                                    </div>
                                                                                                    <div class="contAmsify">
                                                                                                        <div class="form-group" id="contRoles">
                                                                                                            <input type="text" class="form-control" name="rolesInfo">
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            <%--</tpl>--%>
                                                                                        </div>

                                                                                        <%--EJEMPLOS--%>

                                                                                        <div id="items-ct-2">
                                                                                            <div class="contBusinessProcessInfo" id="contBusinessProcess">
                                                                                                <div class="contLabel">
                                                                                                    <div class="contLabelWorkFlowI" onclick="clickOnWorkFlowInfo(this)">Informacion 2</div>
                                                                                                </div>
                                                                                                <div class="contAmsify">
                                                                                                    <div class="form-group" id="contRoles">
                                                                                                        <input type="text" class="form-control" name="rolesInfo2">
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>

                                                                                        <div id="items-ct">
                                                                                            <div class="contBusinessProcessInfo" id="contBusinessProcess">
                                                                                                <div class="contLabel">
                                                                                                    <div class="contLabelWorkFlowI" onclick="clickOnWorkFlowInfo(this)">Informacion 3</div>
                                                                                                </div>
                                                                                                <div class="contAmsify">
                                                                                                    <div class="form-group" id="contRoles">
                                                                                                        <input type="text" class="form-control" name="rolesInfo3">
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </Html>
                                                                                </Tpl>
                                                                            </ext:DataView>
                                                                        </Items>
                                                                    </ext:Container>
                                                                </Items>
                                                            </ext:Panel>
                                                            <ext:Panel ID="ctMain2BP" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1 border1px" BodyPadding="16" Hidden="true">
                                                                <DockedItems>
                                                                    <ext:Toolbar
                                                                        runat="server"
                                                                        ID="toolBar6"
                                                                        Hidden="false"
                                                                        Cls="tlbGrid">
                                                                        <Items>
                                                                            <ext:TextField
                                                                                ID="TextField3"
                                                                                Cls="txtSearchC search"
                                                                                runat="server"
                                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                                LabelWidth="50"
                                                                                WidthSpec="100%"
                                                                                MaxWidth="400"
                                                                                EnableKeyEvents="true">
                                                                                <Triggers>
                                                                                    <ext:FieldTrigger Icon="Search" />
                                                                                    <ext:FieldTrigger Icon="clear" />
                                                                                </Triggers>
                                                                                <Listeners>
                                                                                    <Render Fn="FieldSearch" Buffer="250" />
                                                                                    <Change Fn="FiltrarColumnas" Buffer="250" />
                                                                                </Listeners>
                                                                            </ext:TextField>
                                                                        </Items>
                                                                    </ext:Toolbar>
                                                                </DockedItems>
                                                                <Items>
                                                                    <ext:Container ID="hugeCt4" runat="server" Layout="FitLayout">
                                                                        <Items>
                                                                            <ext:DataView
                                                                                runat="server"
                                                                                ID="DataView3"
                                                                                SingleSelect="true"
                                                                                AnchorVertical="100%"
                                                                                ItemSelector="div.informationbp">
                                                                                <Tpl runat="server">
                                                                                    <Html>
                                                                                        <div id="items-ct">
                                                                                            <%--<tpl for=".">--%>
                                                                                                <div class="contBusinessProcessInfo" id="contBusinessProcess">
                                                                                                    <div class="contLabel">
                                                                                                        <div class="contLabelWorkFlowI" onclick="clickOnWorkFlowInfo(this)">Rol 1</div>
                                                                                                        <div class="btnBloquear"></div>
                                                                                                    </div>
                                                                                                    <div class="contAmsify">
                                                                                                        <div class="form-group" id="contRoles">
                                                                                                            <input type="text" class="form-control" name="rolesInfo">
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            <%--</tpl>--%>
                                                                                        </div>

                                                                                        <%--EJEMPLOS--%>

                                                                                        <div id="items-ct-2">
                                                                                            <div class="contBusinessProcessInfo" id="contBusinessProcess">
                                                                                                <div class="contLabel">
                                                                                                    <div class="contLabelWorkFlowI" onclick="clickOnWorkFlowInfo(this)">Rol 2</div>
                                                                                                    <div class="btnBloquear"></div>
                                                                                                </div>
                                                                                                <div class="contAmsify">
                                                                                                    <div class="form-group" id="contRoles">
                                                                                                        <input type="text" class="form-control" name="rolesInfo2">
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>

                                                                                        <div id="items-ct">
                                                                                            <div class="contBusinessProcessInfo" id="contBusinessProcess">
                                                                                                <div class="contLabel">
                                                                                                    <div class="contLabelWorkFlowI" onclick="clickOnWorkFlowInfo(this)">Rol 3</div>
                                                                                                    <div class="btnBloquear"></div>
                                                                                                </div>
                                                                                                <div class="contAmsify">
                                                                                                    <div class="form-group" id="contRoles">
                                                                                                        <input type="text" class="form-control" name="rolesInfo3">
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </Html>
                                                                                </Tpl>
                                                                            </ext:DataView>
                                                                        </Items>
                                                                    </ext:Container>
                                                                </Items>
                                                            </ext:Panel>
                                                        </Items>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
