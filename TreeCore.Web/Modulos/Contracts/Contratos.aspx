<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contratos.aspx.cs" Inherits="TreeCore.Modulos.Contracts.Contratos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="/CSS/mainStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/iconPicker/css/bootstrap-iconpicker.css" rel="stylesheet" type="text/css" />
    <link href="css/styleContratos.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <div>
            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" DisableViewState="true" ShowWarningOnAjaxFailure="false">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <ext:Hidden runat="server" ID="hdActiveView" />
            <ext:Hidden runat="server" ID="hdJsonCols" />
            <ext:Hidden runat="server" ID="hdJsonColsNew" />
            <ext:Hidden runat="server" ID="hdTraducciones" />


            <ext:Window runat="server"
                ID="winGestionViews"
                Title="<%$ Resources:Comun, strVistas %>"
                Width="300"
                Height="270"
                Resizable="false"
                Draggable="false"
                Modal="true"
                Cls="winForm-resp"
                PaddingSpec="10px 15px"
                Hidden="true">
                <DockedItems>
                    <ext:Toolbar runat="server" Dock="Bottom" Cls="tbGrey">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnGuardarView"
                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                Cls="btn-ppal"
                                CausesValidation="true"
                                Focusable="false"
                                Disabled="false"
                                ValidationGroup="FORM">
                                <Listeners>
                                    <Click Fn="FormSaveView" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:TextField ID="txtCodigo"
                        runat="server"
                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                        MaxLength="50"
                        LabelAlign="Top"
                        WidthSpec="100%"
                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                        AllowBlank="false"
                        ValidationGroup="FORM"
                        CausesValidation="true" />
                    <ext:ComboBox
                        ID="cmbIconos"
                        runat="server"
                        AllowBlank="false"
                        Editable="true"
                        ValueField="icono"
                        DisplayField="icono"
                        QueryMode="Local"
                        WidthSpec="100%"
                        LabelAlign="Top"
                        FieldLabel="<%$ Resources:Comun, strIcono %>"
                        EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                        <Store>
                            <ext:Store ID="storeIconos" runat="server" AutoLoad="true" OnReadData="storeIconos_Refresh"
                                RemoteSort="false">
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Model>
                                    <ext:Model
                                        IDProperty="icono"
                                        runat="server">
                                        <Fields>
                                            <ext:ModelField Name="iconPath" />
                                            <ext:ModelField Name="icono" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ListConfig>
                            <ItemTpl runat="server">
                                <Html>
                                    <img src="{iconPath}" alt="{icono}" />
                                    {icono}
                                </Html>
                            </ItemTpl>
                        </ListConfig>
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
            </ext:Window>

            <ext:Menu runat="server"
                ID="mnOpciones">
                <Items>
                    <ext:MenuItem runat="server"
                        Text="<%$ Resources:Comun, strDetalles %>"
                        ID="mnoDetalles" 
                        Hidden="true"
                        Handler="MostrarDetalleContrato(this.up().dataRecord)"/>
                    <ext:MenuItem runat="server"
                        Text="<%$ Resources:Comun, jsEliminar %>"
                        ID="mnoEliminar"
                        Cls="spanRedV2"
                        Handler="EliminarContrato(this.up().dataRecord.Code)" />
                </Items>
            </ext:Menu>
            
            <ext:Menu runat="server"
                ID="mnOpcionesViews">
                <Items>
                    <ext:MenuItem runat="server"
                        Text="<%$ Resources:Comun, strDefecto %>"
                        Handler="DefaultView(this.up().dataRecord.Code);"/>
                    <ext:MenuItem runat="server"
                        Hidden="true"
                        Text="<%$ Resources:Comun, jsEliminar %>"
                        Handler="DeleteView(this.up().dataRecord.Code);"/>
                    </Items>
            </ext:Menu>

            <ext:Viewport ID="vwResp"
                runat="server"
                Layout="FitLayout">
                <Items>
                    <ext:Panel runat="server"
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel runat="server"
                                ID="pnGridViews"
                                Region="West"
                                Collapsible="true"
                                Width="50"
                                CollapseDirection="Right"
                                CollapseMode="Header"
                                Header="false"
                                Collapsed="false"
                                Hidden="false"
                                Layout="FitLayout">
                                <DockedItems>
                                   <ext:Toolbar runat="server"
                                       Cls="headerTitle"
                                       Height="45">
                                       <Items>
                                           <ext:Label runat="server" 
                                               ID="tlViews"
                                               text="Booksmarks"
                                               Hidden="true"
                                               Cls="headerText">
                                               
                                           </ext:Label>
                                           <ext:ToolbarFill ID="tblFillViews" Hidden="true" />
                                            <ext:Button runat="server"
                                                ID="btnControlpnViews"
                                                Cls="btnControlPanel"
                                                EnableToggle="true"
                                                Handler="panelVistas();">
                                            </ext:Button>
                                       </Items>
                                   </ext:Toolbar>
                                    <ext:Toolbar runat="server"
                                        Dock="Top"
                                        ID="tblFiltroViews"
                                        Hidden="true"
                                        Height="45">
                                        <Items>
                                            <ext:TextField
                                                ID="txtFiltroViews"
                                                Cls="txtSearchD"
                                                runat="server"
                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                WidthSpec="100%"
                                                EnableKeyEvents="true">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Search" />
                                                    <ext:FieldTrigger Handler="ClearfilterViews();" Hidden="true" Icon="clear" />
                                                </Triggers>
                                                <Listeners>
                                                    <Change Handler="App.storeContratos.reload();" Buffer="250" />
                                                </Listeners>
                                            </ext:TextField>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:GridPanel
                                        runat="server"
                                        ID="gridViews"
                                        SelectionMemory="false"
                                        Cls="gridPanel gridGeneral"
                                        Header="false"
                                        EnableColumnHide="false"
                                        AnchorVertical="100%"
                                        Region="Center"
                                        Hidden="false"
                                        AriaRole="main">
                                        <Store>
                                            <ext:Store runat="server" ID="storeViews" RemotePaging="false" AutoLoad="true" OnReadData="storeViews_Refresh" RemoteSort="false" PageSize="20">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Listeners>
                                                    <DataChanged Handler="CargarBuscadorPredictivoViews(); seleccionarVistaActiva();" />
                                                </Listeners>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="Code">
                                                        <Fields>
                                                            <ext:ModelField Name="Code" />
                                                            <ext:ModelField Name="Columns" Type="Object" />
                                                            <ext:ModelField Name="Icon" />
                                                            <ext:ModelField Name="Default" Type="Boolean" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel runat="server" >
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colIcon"
                                                    DataIndex="Icon"
                                                    Align="Center"
                                                    Sortable="false"
                                                    Width="45">
                                                    <Renderer Fn="RenderIcono" />
                                                </ext:Column>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colViewCode"
                                                    DataIndex="Code"
                                                    Hidden="true"
                                                    Text="<%$ Resources:Comun, strCodigo %>"
                                                    Width="150"
                                                    Flex="1" />
                                                <ext:WidgetColumn 
                                                    ID="colEditView"
                                                    runat="server"
                                                    Filterable="false"
                                                    Hidden="true"
                                                    Width="50">
                                                    <Widget>
                                                        <ext:Button runat="server"
                                                            OverCls="Over-btnMore"
                                                            PressedCls="Pressed-none"
                                                            FocusCls="Focus-none"
                                                            Cls="btnColumna Hidden btnEditar">
                                                            <Listeners>
                                                                <Click Handler="EditView(this.getWidgetRecord().data.Code);" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Widget>
                                                </ext:WidgetColumn>
                                                <ext:WidgetColumn 
                                                    ID="colOptionsView"
                                                    runat="server"
                                                    Filterable="false"
                                                    Hidden="true"
                                                    Width="50">
                                                    <Widget>
                                                        <ext:Button runat="server"
                                                            OverCls="Over-btnMore"
                                                            PressedCls="Pressed-none"
                                                            FocusCls="Focus-none"
                                                            Cls="btnColumna Hidden btnMoreOptions">
                                                            <Listeners>
                                                                <Click Fn="optionsViews" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Widget>
                                                </ext:WidgetColumn>
                                                <ext:Column runat="server"
                                                    ID="colViewDef"
                                                    DataIndex="Default"
                                                    Align="Center"
                                                    Sortable="false"
                                                    Hidden="true"
                                                    Width="40">
                                                    <Renderer Fn="RenderDef" />
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="selViews"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="selectView" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" Region="Center" Layout="FitLayout">
                                <DockedItems>
                                    <ext:Toolbar runat="server"
                                        ID="tlbBase"
                                        Dock="Top"
                                        Cls="tlbGrid">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnAnadir"
                                                Cls="btnAnadir"
                                                Hidden="true"
                                                ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                Handler="AgregarEditar();">
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnEliminar"
                                                Hidden="true"
                                                ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                Cls="btn-Eliminar"
                                                Handler="Eliminar();" />
                                            <ext:Button runat="server"
                                                ID="btnRefrescar"
                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                Cls="btnRefrescar"
                                                Handler="RefrescarContrato();" />
                                            <ext:Button runat="server"
                                                ID="btnDescargar"
                                                Hidden="true"
                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                Cls="btnDescargar"
                                                Handler="" />
                                            <ext:ToolbarFill />
                                            <ext:TextField
                                                ID="txtFiltroContratos"
                                                Cls="txtSearchD"
                                                runat="server"
                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                Width="300"
                                                EnableKeyEvents="true">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Search" />
                                                    <ext:FieldTrigger Handler="ClearfilterContratos();" Hidden="true" Icon="clear" />
                                                </Triggers>
                                                <Listeners>
                                                    <Change Fn="filterContracts" Buffer="250" />
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:Button runat="server"
                                                ID="btnGuardarVista"
                                                Cls="btnSave GuardarVista"
                                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                                Focusable="false"
                                                MarginSpec="0 1 0 10"
                                                PressedCls="none"
                                                Hidden="false">
                                                <Listeners>
                                                    <Click Fn="GuardarViews" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnOpcionesGuardarVista"
                                                Cls="btnSave OpcionesGuardarVista btnMenuDes"
                                                Focusable="false"
                                                MarginSpec="0 10 0 0"
                                                PressedCls="none"
                                                Hidden="false">
                                                <Menu>
                                                    <ext:Menu runat="server">
                                                        <Items>
                                                            <ext:MenuItem
                                                                ID="mnuEditView"
                                                                runat="server"
                                                                Text="Edit View"
                                                                Handler="EditView(App.hdActiveView.value);" />
                                                            <ext:MenuItem
                                                                ID="mnuAddnewView"
                                                                runat="server"
                                                                Text="Save as"
                                                                Handler="NewView();" />
                                                        </Items>
                                                    </ext:Menu>
                                                </Menu>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnViews"
                                                ToolTip="<%$ Resources:Comun, strVistas %>"
                                                EnableToggle="true"
                                                Cls="btnColumnas"
                                                Handler="OpenViews();" />
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar runat="server"
                                        ID="tlbFilter"
                                        Height="40"
                                        Hidden="true"
                                        Dock="Top">
                                        <Items>
                                            <ext:ComboBox
                                                ID="cmbFiltroEstados"
                                                Cls="txtSearchD"
                                                runat="server"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strEstados %>"
                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                ValueField="Code"
                                                DisplayField="Code"
                                                QueryMode="Local"
                                                Hidden="true"
                                                Mode="Local"
                                                AllowBlank="true"
                                                Width="250">
                                                <Store>
                                                    <ext:Store runat="server" ID="storeEstados" RemotePaging="false" AutoLoad="false" OnReadData="storeEstados_Refresh" RemoteSort="false" PageSize="20">
                                                        <Proxy>
                                                            <ext:PageProxy />
                                                        </Proxy>
                                                        <Model>
                                                            <ext:Model runat="server" IDProperty="Code">
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
                                                </Store>
                                                <Triggers>
                                                    <ext:FieldTrigger IconCls="ico-reload"
                                                        Hidden="true"
                                                        Weight="-1"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                </Triggers>
                                                <Listeners>
                                                    <Select Fn="SelectCmbFiltro" />
                                                    <TriggerClick Fn="ReloadCmbFiltro" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox
                                                ID="cmbFiltroTipos"
                                                Cls="txtSearchD"
                                                runat="server"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strTipos %>"
                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                ValueField="Code"
                                                DisplayField="Code"
                                                QueryMode="Local"
                                                Mode="Local"
                                                Hidden="true"
                                                AllowBlank="true"
                                                Width="250">
                                                <Store>
                                                    <ext:Store runat="server" ID="storeTipos" RemotePaging="false" AutoLoad="false" OnReadData="storeTipos_Refresh" RemoteSort="false" PageSize="20">
                                                        <Proxy>
                                                            <ext:PageProxy />
                                                        </Proxy>
                                                        <Model>
                                                            <ext:Model runat="server" IDProperty="Code">
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
                                                </Store>
                                                <Triggers>
                                                    <ext:FieldTrigger IconCls="ico-reload"
                                                        Hidden="true"
                                                        Weight="-1"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                </Triggers>
                                                <Listeners>
                                                    <Select Fn="SelectCmbFiltro" />
                                                    <TriggerClick Fn="ReloadCmbFiltro" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox
                                                ID="cmbFiltroGrupos"
                                                Cls="txtSearchD"
                                                runat="server"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strGrupos %>"
                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                ValueField="Code"
                                                DisplayField="Code"
                                                QueryMode="Local"
                                                Mode="Local"
                                                Hidden="true"
                                                AllowBlank="true"
                                                Width="250">
                                                <Store>
                                                    <ext:Store runat="server" ID="storeGrupos" RemotePaging="false" AutoLoad="false" OnReadData="storeGrupos_Refresh" RemoteSort="false" PageSize="20">
                                                        <Proxy>
                                                            <ext:PageProxy />
                                                        </Proxy>
                                                        <Model>
                                                            <ext:Model runat="server" IDProperty="Code">
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
                                                </Store>
                                                <Triggers>
                                                    <ext:FieldTrigger IconCls="ico-reload"
                                                        Hidden="true"
                                                        Weight="-1"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                </Triggers>
                                                <Listeners>
                                                    <Select Fn="SelectCmbFiltro" />
                                                    <TriggerClick Fn="ReloadCmbFiltro" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox
                                                ID="cmbFiltroMonedas"
                                                Cls="txtSearchD"
                                                runat="server"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strMonedas %>"
                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                ValueField="Code"
                                                DisplayField="Code"
                                                QueryMode="Local"
                                                Mode="Local"
                                                Hidden="true"
                                                AllowBlank="true"
                                                Width="250">
                                                <Store>
                                                    <ext:Store runat="server" ID="storeMonedas" RemotePaging="false" AutoLoad="false" OnReadData="storeMonedas_Refresh" RemoteSort="false" PageSize="20">
                                                        <Proxy>
                                                            <ext:PageProxy />
                                                        </Proxy>
                                                        <Model>
                                                            <ext:Model runat="server" IDProperty="Code">
                                                                <Fields>
                                                                    <ext:ModelField Name="Code" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                        <Sorters>
                                                            <ext:DataSorter Property="Code" Direction="ASC" />
                                                        </Sorters>
                                                    </ext:Store>
                                                </Store>
                                                <Triggers>
                                                    <ext:FieldTrigger IconCls="ico-reload"
                                                        Hidden="true"
                                                        Weight="-1"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                </Triggers>
                                                <Listeners>
                                                    <Select Fn="SelectCmbFiltro" />
                                                    <TriggerClick Fn="ReloadCmbFiltro" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:Button runat="server"
                                                ID="btnClearFilters"
                                                Hidden="true"
                                                ToolTip="<%$ Resources:Comun, strLimpiarFiltro %>"
                                                Cls="btnNoFiltros"
                                                Handler="ClearFilters();" />
                                            <ext:ToolbarFill />
                                            <%--<ext:Button runat="server"
                                                ID="btnGuardarVista"
                                                Cls="btnSave GuardarVista"
                                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                                Focusable="false"
                                                MarginSpec="0 1 0 0"
                                                PressedCls="none"
                                                Hidden="false">
                                                <Listeners>
                                                    <Click Fn="GuardarViews" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnOpcionesGuardarVista"
                                                Cls="btnSave OpcionesGuardarVista"
                                                Focusable="false"
                                                MarginSpec="0 70 0 0"
                                                PressedCls="none"
                                                Hidden="false">
                                                <Menu>
                                                    <ext:Menu runat="server">
                                                        <Items>
                                                            <ext:MenuItem
                                                                ID="mnuEditView"
                                                                runat="server"
                                                                Text="Edit View"
                                                                Handler="EditView();" />
                                                            <ext:MenuItem
                                                                ID="mnuAddnewView"
                                                                runat="server"
                                                                Text="Save as"
                                                                Handler="NewView();" />
                                                        </Items>
                                                    </ext:Menu>
                                                </Menu>
                                            </ext:Button>--%>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:GridPanel
                                        runat="server"
                                        ID="gridContratos"
                                        meta:resourceKey="grid"
                                        SelectionMemory="false"
                                        Cls="gridPanel gridGeneral"
                                        Header="false"
                                        EnableColumnHide="false"
                                        Region="Center"
                                        Hidden="false"
                                        AriaRole="main">
                                        <Listeners>                                            
                                            <RowContextMenu Fn="ShowRightClickMenu" />
                                        </Listeners>
                                        <Store>
                                            <ext:Store runat="server" ID="storeContratos" RemotePaging="true" AutoLoad="true" OnReadData="storeContratos_Refresh" RemoteSort="true" PageSize="20" RemoteFilter="true">
                                                <Listeners>
                                                    <BeforeLoad Fn="DeseleccionarGrilla" />
                                                    <%--<DataChanged Fn="CargarBuscadorPredictivoContracts" />--%>
                                                </Listeners>
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="Code">
                                                        <Fields>
                                                            <ext:ModelField Name="Code" />
                                                            <ext:ModelField Name="Name" />
                                                            <ext:ModelField Name="ContractStatusCode" />
                                                            <ext:ModelField Name="SiteCode" />
                                                            <ext:ModelField Name="CurrencyCode" />
                                                            <ext:ModelField Name="ContractGroupCode" />
                                                            <ext:ModelField Name="ContractTypeCode" />
                                                            <ext:ModelField Name="Description" />
                                                            <ext:ModelField Name="MasterContractNumber" />
                                                            <ext:ModelField Name="ExecutionDate" />
                                                            <ext:ModelField Name="StartDate" />
                                                            <ext:ModelField Name="FirsEndDate" />
                                                            <ext:ModelField Name="Duration" />
                                                            <ext:ModelField Name="ClosedAtExpiration" />
                                                            <ext:ModelField Name="RenewalClause">
                                                                <Model>
                                                                    <ext:Model runat="server">
                                                                        <Fields>
                                                                            <ext:ModelField Name="Type" />
                                                                            <ext:ModelField Name="Frequencymonths" />
                                                                            <ext:ModelField Name="TotalRenewalNumber" />
                                                                            <ext:ModelField Name="CurrentRenewalNumber" />
                                                                            <ext:ModelField Name="NotificationNumberDays" />
                                                                            <ext:ModelField Name="RenewalDate" />
                                                                            <ext:ModelField Name="Renewalnotificationdate" />
                                                                            <ext:ModelField Name="ExpirationDate" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:ModelField>
                                                            <ext:ModelField Name="contractline" Type="Object" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                    <ext:DataSorter Property="Code" Direction="ASC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel runat="server">
                                            <Columns>
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
                                        <Plugins>
                                            <ext:GridFilters runat="server"
                                                ID="gridFilters"
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                meta:resourceKey="GridFilters" />
                                        </Plugins>
                                        <BottomBar>
                                            <ext:PagingToolbar runat="server"
                                                ID="PagingToolBar"
                                                meta:resourceKey="PagingToolBar"
                                                StoreID="storeContratos"
                                                DisplayInfo="true"
                                                HideRefresh="true">
                                                <Items>
                                                    <ext:ComboBox runat="server"
                                                        Cls="comboGrid"
                                                        Width="80">
                                                        <Items>
                                                            <ext:ListItem Text="10" />
                                                            <ext:ListItem Text="20" />
                                                            <ext:ListItem Text="30" />
                                                            <ext:ListItem Text="40" />
                                                            <ext:ListItem Text="50" />
                                                        </Items>
                                                        <SelectedItems>
                                                            <ext:ListItem Value="20" />
                                                        </SelectedItems>
                                                        <Listeners>
                                                            <Select Fn="handlePageSizeSelect" />
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
                                    <ext:Panel ID="pnViews" runat="server" Hidden="true">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" Dock="Top">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnSalirViews"
                                                        Cls="btnMoveLeft"
                                                        Handler="App.btnViews.click()" />
                                                    <ext:Label runat="server" Text="Setting Columns" Cls="settingsColumns">
                                                    </ext:Label>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server" Dock="Bottom" Padding="10">
                                                <Items>
                                                    <ext:ToolbarFill />
                                                    <ext:Button runat="server"
                                                        ID="btnAplicarView"
                                                        Cls="btnSave"
                                                        Text="<%$ Resources:Comun, strAplicar %>"
                                                        Focusable="false"
                                                        MarginSpec="0 15 0 0"
                                                        Width="100"
                                                        PressedCls="none"
                                                        Hidden="false">
                                                        <Listeners>
                                                            <Click Fn="ApplyCols" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <ext:Panel runat="server">
                                                <Content>
                                                    <div class="ctColsSel">
                                                        <ul id="ctOrderCols">
                                                        </ul>
                                                    </div>
                                                </Content>
                                            </ext:Panel>
                                            <ext:Panel runat="server">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server" Dock="Top">
                                                        <Items>
                                                            <ext:ToolbarFill />
                                                            <ext:TextField
                                                                ID="txtFiltroColumnas"
                                                                Cls="txtSearchD"
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                Width="600"
                                                                EnableKeyEvents="true">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Search" />
                                                                    <ext:FieldTrigger Handler="ClearfilterColumnas();" Hidden="true" Icon="clear" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <Change Fn="filterColumnas" Buffer="250" />
                                                                </Listeners>
                                                            </ext:TextField>
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Items>
                                                    <ext:DataView runat="server"
                                                        ID="dtvColumnas"
                                                        SingleSelect="true"
                                                        ItemSelector="div.columna">
                                                        <Store>
                                                            <ext:Store runat="server" ID="storeColumnas" RemotePaging="false" AutoLoad="false" OnReadData="storeColumnas_Refresh" RemoteSort="false" PageSize="20">
                                                                <Listeners>
                                                                    <DataChanged Handler="CargarBuscadorPredictivoColumnas();" />
                                                                </Listeners>
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="Code">
                                                                        <Fields>
                                                                            <ext:ModelField Name="Code" />
                                                                            <ext:ModelField Name="Name" />
                                                                            <ext:ModelField Name="Order" Type="Int" />
                                                                            <ext:ModelField Name="Active" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                                <Sorters>
                                                                </Sorters>
                                                            </ext:Store>
                                                        </Store>
                                                        <Tpl runat="server">
                                                            <Html>
                                                                <div id="ctColumns">
                                                                    <tpl for=".">
                                                                        <div class="ctCol">
                                                                            <input type="checkbox" id="chkDone{Code}" class="chkDone" code="{Code}" {Active} onclick="SeleccionarColumna(this)" />
                                                                            <label class="lblNombre spanLbl">
                                                                                {Name}
                                                                            </label>
                                                                        </div>
                                                                    </tpl>
                                                                </div>
                                                            </Html>
                                                        </Tpl>
                                                    </ext:DataView>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server"
                                ID="pnDetails"
                                Region="East"
                                Collapsible="true"
                                Width="300"
                                CollapseDirection="Left"
                                CollapseMode="Header"
                                Layout="FitLayout"
                                Header="false"
                                Collapsed="true">
                                <Listeners>
                                    <Expand Fn="openPanelDetails" />
                                </Listeners>
                                <Items>
                                    <ext:Panel runat="server" Layout="FitLayout" Cls="panelContract">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" Dock="Top">
                                                <Items>
                                                    <ext:ToolbarFill />
                                                    <ext:Label runat="server" Text="Contract" Cls="titleContract">
                                                    </ext:Label>
                                                    <ext:Label runat="server" Text="Site" Hidden="true">
                                                    </ext:Label>
                                                    <ext:Label runat="server" Text="Documents" Hidden="true">
                                                    </ext:Label>
                                                    <ext:ToolbarFill />
                                                    <ext:Button runat="server"
                                                        Cls="btnControlPanel"
                                                        Handler="App.pnDetails.collapse();">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server" Dock="Top" Height="50">
                                                <Items>
                                                    <ext:Label runat="server" Text="<%$ Resources:Comun, jsModeloDatos %>" Cls="dataModel" IconCls="btnDatamodelgreen" ID="lbPnDetail" ></ext:Label>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <ext:Panel runat="server"
                                                ID="pnDetailsContracts"
                                                Layout="AnchorLayout">
                                                <Items>
                                                    <ext:Panel runat="server" Border="false" Header="false" AnchorVertical="100%" AnchorHorizontal="12%"
                                                        Cls="d-inline-block">
                                                        <Items>
                                                            <ext:Button
                                                                runat="server"
                                                                Cls="btnDatamodel"
                                                                
                                                                ToolTip="<%$ Resources:Comun, jsModeloDatos %>"
                                                                Handler="MostrarDetalleContrato(App.storeContratos.getById(App.pnDetails.selectItem).data);">
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                Cls="btnContrLine"
                                                                ToolTip="<%$ Resources:Comun, jsLineaContrato %>"
                                                                Handler="MostrarDetalleContratoLineas(App.storeContratos.getById(App.pnDetails.selectItem).data.contractline);">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Panel>
                                                    <ext:Panel
                                                        runat="server"
                                                        AnchorVertical="100%" AnchorHorizontal="88%"
                                                        Border="false"
                                                        OverflowY="Auto"
                                                        Header="false"
                                                        Cls="d-inline-block pnDetail">
                                                        <Content>
                                                            <div id="vsDtContratcs">
                                                            </div>
                                                        </Content>
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
