<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrders.aspx.cs" Inherits="TreeCore.ModWorkOrders.pages.WorkOrders" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <!--<script type="text/javascript" src="../../JS/common.js"></script>-->
    <script type="text/javascript" src="js/WorkOrders.js"></script>
    <script type="text/javascript" src="/JS/common.js"></script>
</head>
<body>
    <link href="/CSS/mainStyle.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                </Listeners>
            </ext:ResourceManager>

            <ext:Hidden runat="server" ID="hdActiveView" />
            <ext:Hidden runat="server" ID="hdJsonCols" />
            <ext:Hidden runat="server" ID="hdJsonColsNew" />
            <ext:Hidden runat="server" ID="hdTraducciones" />

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
                                Hidden="true"
                                Layout="FitLayout">
                                <Items>
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
                                                ID="btnRefrescar"
                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                Cls="btnRefrescar"
                                                Handler="RefrescarWO();" />
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar runat="server"
                                        ID="tlbFilter"
                                        Height="80"
                                        Hidden="false"
                                        Dock="Top">
                                        <Items>                                            
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
                                                Hidden="false"
                                                AllowBlank="true"
                                                Width="250">
                                                <Store>
                                                    <ext:Store runat="server" ID="storeTipos" RemotePaging="false" AutoLoad="true" OnReadData="storeTipos_Refresh" RemoteSort="false" PageSize="20">
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
                                                Cls="txtSearchD"
                                                runat="server"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strTipos %>"
                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                ValueField="Code"
                                                DisplayField="Code"
                                                QueryMode="Local"
                                                Mode="Local"
                                                Hidden="false"
                                                AllowBlank="true"
                                                Width="250">
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
                                                Cls="txtSearchD"
                                                runat="server"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strTipos %>"
                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                ValueField="Code"
                                                DisplayField="Code"
                                                QueryMode="Local"
                                                Mode="Local"
                                                Hidden="false"
                                                AllowBlank="true"
                                                Width="250">
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
                                                Cls="txtSearchD"
                                                runat="server"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strTipos %>"
                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                ValueField="Code"
                                                DisplayField="Code"
                                                QueryMode="Local"
                                                Mode="Local"
                                                Hidden="false"
                                                AllowBlank="true"
                                                Width="250">
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
                                                Cls="txtSearchD"
                                                runat="server"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strTipos %>"
                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                ValueField="Code"
                                                DisplayField="Code"
                                                QueryMode="Local"
                                                Mode="Local"
                                                Hidden="false"
                                                AllowBlank="true"
                                                Width="250">
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
                                                Cls="txtSearchD"
                                                runat="server"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strTipos %>"
                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                ValueField="Code"
                                                DisplayField="Code"
                                                QueryMode="Local"
                                                Mode="Local"
                                                Hidden="false"
                                                AllowBlank="true"
                                                Width="250">
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
                                                Hidden="false"
                                                ToolTip="<%$ Resources:Comun, strLimpiarFiltro %>"
                                                Cls="btnNoFiltros"
                                                Handler="ClearFilters();" />
                                            <ext:ToolbarFill />
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:TreePanel
                                        runat="server"
                                        ID="gridWO"
                                        meta:resourceKey="grid"
                                        SelectionMemory="false"
                                        Cls="gridPanel gridGeneral"
                                        Header="false"
                                        EnableColumnHide="false"
                                        RootVisible="false"
                                        Region="Center"
                                        Hidden="false"
                                        AriaRole="main">
                                        <Store>
                                            <ext:TreeStore runat="server" ID="storeWO" RemotePaging="true" AutoLoad="true" RemoteSort="true" PageSize="20" RemoteFilter="true" OnReadData="storeWO_Refresh">
                                                <Listeners>
                                                </Listeners>
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="Code">
                                                        <Fields>
                                                            <ext:ModelField Name="Code" />
                                                            <ext:ModelField Name="Name" />
                                                            <ext:ModelField Name="Type" />
                                                            <ext:ModelField Name="Status" />
                                                            <ext:ModelField Name="Customer" />
                                                            <ext:ModelField Name="Supplier" />
                                                            <ext:ModelField Name="Progress" />
                                                            <ext:ModelField Name="Start" />
                                                            <ext:ModelField Name="Due" />
                                                            <ext:ModelField Name="Program" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                    <ext:DataSorter Property="Code" Direction="ASC" />
                                                </Sorters>
                                            </ext:TreeStore>
                                        </Store>
                                        <ColumnModel runat="server">
                                            <Columns>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:TreeSelectionModel runat="server"
                                                ID="GridRowSelect"
                                                Mode="Single">
                                                <Listeners>
                                                </Listeners>
                                            </ext:TreeSelectionModel>
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
                                                            <%--<Select Fn="handlePageSizeSelect" />--%>
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:TreePanel>
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
                                    <%--                                    <Expand Fn="openPanelDetails" />--%>
                                </Listeners>
                                <Items>
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
