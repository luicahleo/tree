<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrdersContenedor.aspx.cs" Inherits="TreeCore.ModWorkOrders.WorkOrdersContenedor" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/WorkOrdersContenedor.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden runat="server" ID="hdCliID" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--WINDOWS FILTROS--%>

            <ext:Window ID="winAddTabFilter"
                runat="server"
                Title="Add Tab"
                Height="195"
                Width="380"
                BodyCls=""
                Cls="winForm-respSimple"
                Scrollable="Vertical"
                Hidden="true"
                Layout="VBoxLayout">
                <Defaults>
                    <ext:Parameter Name="margin" Value="0 0 5 0" Mode="Value" />
                </Defaults>
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Center" />
                </LayoutConfig>
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar7" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server" ID="backNewUsers" Cls="btn-secondary " MinWidth="110" Text="Cancel" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>
                            <ext:Button runat="server" ID="nextNewUsers" Cls="btn-ppal " Text="Save Tab" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:TextField runat="server" ID="txtNewTabName" LabelAlign="Top" FieldLabel="Tab Name" WidthSpec="90%"></ext:TextField>
                </Items>
            </ext:Window>

            <ext:Window ID="winSaveQF"
                runat="server"
                Title="Save Quick Filter"
                Height="195"
                Width="380"
                BodyCls=""
                Cls="winForm-respSimple"
                Scrollable="Vertical"
                Hidden="true"
                Layout="VBoxLayout">
                <Defaults>
                    <ext:Parameter Name="margin" Value="0 0 5 0" Mode="Value" />
                </Defaults>
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Center" />
                </LayoutConfig>
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar5" Cls=" greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server" ID="Button3" Cls="btn-secondary " MinWidth="110" Text="Cancel" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>
                            <ext:Button runat="server" ID="Button4" Cls="btn-ppal " Text="Save Quick Filter" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:TextField runat="server" ID="TextField1" LabelAlign="Top" FieldLabel="Filter Name" WidthSpec="90%"></ext:TextField>
                </Items>
            </ext:Window>

            <%--FIN WINDOWS FILTROS--%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Cls=""
                Layout="FitLayout">
                <Content>
                    <ext:Button runat="server" ID="btnCollapseAsR" Cls="btn-trans btnCollapseAsRClosedv2" Hidden="false"
                        ToolTip="<%$ Resources:Comun, strAbrirMenu %>">
                        <Listeners>
                            <Click Fn="hidePnFilters" />
                            <AfterRender Handler="document.getElementById('btnCollapseAsR').style.opacity = 0;" />
                        </Listeners>
                    </ext:Button>
                </Content>
                <Items>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls=""
                        Layout="BorderLayout">
                        <Items>

                            <%-- PANEL CENTRAL CON SLIDERS--%>

                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                PaddingSpec="20 0 20 20"
                                Border="false"
                                Region="Center"
                                Layout="HBoxLayout"
                                Cls="visorInsidePn bckGris">
                                <LayoutConfig>
                                    <ext:HBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <DockedItems>
                                    <ext:Toolbar runat="server"
                                        ID="tbNavNAside"
                                        Dock="Top"
                                        Cls="tbGrey tbNoborder"
                                        Hidden="false"
                                        Padding="10"
                                        OverflowHandler="Scroller"
                                        Flex="1">
                                        <Items>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkWorkOrders"
                                                Cls="lnk-navView lnk-noLine navActivo"
                                                meta:resourceKey="lnkWorkOrders"
                                                Text="WORK ORDERS">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkWorkOrdersSites"
                                                Cls="lnk-navView lnk-noLine"
                                                meta:resourceKey="lnkWOBySites"
                                                Text="WO BY SITES">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkWorkOrdersInventory"
                                                Cls="lnk-navView lnk-noLine"
                                                meta:resourceKey="lnkInventory"
                                                Text="INVENTORY">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkWorkOrdersTickets"
                                                Cls="lnk-navView lnk-noLine"
                                                meta:resourceKey="lnkTickets"
                                                Text="TICKETS">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkWorkOrdersCalendar"
                                                Cls="lnk-navView lnk-noLine"
                                                meta:resourceKey="lnkCalendar"
                                                Text="CALENDAR">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Panel runat="server" ID="tagsContainer" Layout="ColumnLayout">
                                        <Items>
                                        </Items>
                                    </ext:Panel>
                                </DockedItems>
                                <Items>
                                    <ext:Panel ID="ctMain1" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="false">
                                        <Items>
                                            <ext:Container ID="hugeCt" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain"
                                                    runat="server"
                                                    Url="../WorkOrder/WorkOrders.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain2" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt2" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain2"
                                                    runat="server"
                                                    Url="../WorkOrder/WorkOrdersSites.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain3" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt3" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain3"
                                                    runat="server"
                                                    Url="../WorkOrder/WorkOrdersInventory.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain4" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt4" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderTickets"
                                                    runat="server"
                                                    Url="../WorkOrder/WorkOrdersTickets.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain5" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt5" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain5"
                                                    runat="server"
                                                    Url="../WorkOrder/WorkOrdersCalendar.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="FitLayout"
                                Header="false"
                                Border="false"
                                Width="380"
                                Hidden="false">
                                <Listeners>
                                    <%--<AfterRender Handler="ResizerAside(this)"></AfterRender>--%>
                                </Listeners>
                                <DockedItems>
                                    <ext:Label Dock="Top"
                                        MinHeight="60"
                                        MinWidth="300"
                                        PaddingSpec="20 0 0 20"
                                        meta:resourcekey="lblAsideNameR"
                                        ID="lblAsideNameR"
                                        runat="server"
                                        IconCls="ico-head-filters"
                                        Cls="lblHeadAsideDock"
                                        Text="<%$ Resources:Comun, strFiltros %>">
                                    </ext:Label>
                                    <ext:Label Dock="Top"
                                        MinHeight="60"
                                        MinWidth="300"
                                        PaddingSpec="20 0 0 20"
                                        meta:resourcekey="lblAsideNameR"
                                        ID="lblAsideNameInfo"
                                        runat="server"
                                        IconCls="ico-head-info"
                                        Cls="lblHeadAsideDock"
                                        Hidden="true"
                                        Text="More Info">
                                    </ext:Label>
                                </DockedItems>

                                <Items>

                                    <%-- PANEL Filtros--%>

                                    <ext:Panel runat="server" ID="WrapFilterControls" Hidden="false" Cls="tbGrey" AnchorVertical="100%" AnchorHorizontal="100%" Layout="AnchorLayout">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="MenuNavPnFiltros" Cls="TbAsideNavPanel" Padding="0" Dock="Left" Layout="VBoxLayout">
                                                <Items>
                                                    <ext:Button runat="server" Padding="0" Margin="0" meta:resourcekey="btnQuickFilters" ID="btnQuickFilters" Cls="btnquickFilters-asR btnNoBorder" Handler="displayMenu('pnQuickFilters')"></ext:Button>
                                                    <ext:Button runat="server" Padding="0" Margin="0" meta:resourcekey="btnInfoGrid" ID="btnCreateFilters" Cls="btnFiltersPlus-asR btnNoBorder" Handler="displayMenu('pnCFilters')"></ext:Button>
                                                    <ext:Button runat="server" Padding="0" Margin="0" meta:resourcekey="btnVersions" ID="btnMyFilters" Cls="btnMyFilters-asR btnNoBorder" Handler="displayMenu('pnGridsAsideMyFilters')"></ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <%--CREATE FILTERS PANEL--%>
                                            <ext:Panel
                                                AnchorVertical="100%" AnchorHorizontal="100%"
                                                Hidden="true"
                                                runat="server"
                                                PaddingSpec="15 20 20 15"
                                                Layout="VBoxLayout"
                                                Cls="Whitebg"
                                                ID="pnCFilters">
                                                <DockedItems>
                                                    <ext:Label
                                                        MarginSpec="10 0 18 0"
                                                        Dock="Top"
                                                        meta:resourcekey="lblGrid"
                                                        ID="lblGrid"
                                                        runat="server"
                                                        IconCls="btn-CFilter"
                                                        Cls="lblHeadAside"
                                                        Text="<%$ Resources:Comun, strCrearFiltro %>">
                                                    </ext:Label>
                                                </DockedItems>

                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Stretch" />
                                                </LayoutConfig>
                                                <Items>
                                                    <ext:Container runat="server" ID="fila1F" Layout="HBoxLayout">
                                                        <Items>
                                                            <ext:TextField runat="server"
                                                                Flex="3"
                                                                MarginSpec="0 6 0 0"
                                                                meta:resourcekey="pnNewFilter"
                                                                ID="pnNewFilterNombre"
                                                                FieldLabel=""
                                                                LabelAlign="Top"
                                                                AllowBlank="false"
                                                                ValidationGroup="FORM"
                                                                CausesValidation="true"
                                                                EmptyText="<%$ Resources:Comun, strNombreFiltro %>" />
                                                            <ext:Button
                                                                MarginSpec="0 0 0 6"
                                                                Flex="2"
                                                                runat="server"
                                                                ID="btnFilter1"
                                                                Cls="btn-ppal"
                                                                Text="New Filter">
                                                                <Listeners>
                                                                    <%--<Click Fn="newFilter" />--%>
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Container>
                                                    <ext:Container runat="server" ID="fila2F" Layout="HBoxLayout">
                                                        <Items>
                                                            <ext:ComboBox runat="server"
                                                                meta:resourcekey="cmbField"
                                                                MarginSpec="0 6 0 0"
                                                                ID="cmbField"
                                                                FieldLabel="<%$ Resources:Comun, strCampo %>"
                                                                LabelAlign="Top"
                                                                DisplayField="Name"
                                                                EmptyText="<%$ Resources:Comun, strCampo %>"
                                                                Flex="1"
                                                                Cls="pnForm fieldFilter"
                                                                ValueField="Id"
                                                                QueryMode="Local">
                                                                <Store>
                                                                    <ext:Store
                                                                        ID="storeCampos"
                                                                        runat="server"
                                                                        AutoLoad="true">
                                                                        <Model>
                                                                            <ext:Model runat="server" IDProperty="Id">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="Id" />
                                                                                    <ext:ModelField Name="Name" />
                                                                                    <ext:ModelField Name="typeData" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                        <Listeners>
                                                                            <%--         <DataChanged Fn="beforeLoadCmbField" />--%>
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <%--        <Select Fn="selectField" />--%>
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:TextField runat="server"
                                                                Flex="1"
                                                                MarginSpec="0 0 0 6"
                                                                meta:resourcekey="pnSearch"
                                                                ID="textInputSearch"
                                                                FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                LabelAlign="Top"
                                                                AllowBlank="false"
                                                                ValidationGroup="FORM"
                                                                CausesValidation="true"
                                                                EmptyText="<%$ Resources:Comun, strDependeDelCampo %>"
                                                                Cls="pnForm"
                                                                Hidden="false" />
                                                        </Items>
                                                    </ext:Container>
                                                    <ext:Container runat="server" ID="fil3F" Layout="HBoxLayout" MarginSpec="8 0 0 0">
                                                        <Items>
                                                            <ext:Component runat="server" Flex="1"></ext:Component>
                                                            <ext:Button
                                                                runat="server" meta:resourcekey="ColMas"
                                                                ID="btnAdd"
                                                                IconCls="ico-addBtn"
                                                                Cls="btn-mini-ppal btnAdd"
                                                                Text="<%$ Resources:Comun, jsAgregar %>">
                                                                <Listeners>
                                                                    <%--     <Click Fn="addElementFilter" />--%>
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Container>

                                                    <ext:GridPanel
                                                        ID="GridCrearFiltros"
                                                        runat="server"
                                                        Header="false"
                                                        Border="false"
                                                        Cls="GridCreateFilters"
                                                        Scrollable="Vertical"
                                                        Height="180">
                                                        <DockedItems>
                                                            <ext:Toolbar runat="server" ID="tbGridCrearFiltros" Dock="Bottom" MarginSpec="0 -8 0 0">
                                                                <Items>
                                                                    <ext:ToolbarFill></ext:ToolbarFill>
                                                                    <ext:Button
                                                                        runat="server"
                                                                        meta:resourcekey="btnSaveFilter"
                                                                        ID="btnSaveFilter"
                                                                        Cls="btn-save"
                                                                        Text="<%$ Resources:Comun, btnGuardar.Text %>">
                                                                    </ext:Button>

                                                                    <ext:Button
                                                                        runat="server"
                                                                        meta:resourcekey="btnAplyFilter"
                                                                        ID="btnAplyFilter"
                                                                        Cls="btn-end"
                                                                        Text="<%$ Resources:Comun, strAplicar %>">
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <Store>
                                                            <ext:Store
                                                                runat="server"
                                                                PageSize="10"
                                                                AutoLoad="false">
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="ID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="Campo" />
                                                                            <ext:ModelField Name="Busqueda" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:Store>
                                                        </Store>
                                                        <ColumnModel runat="server">
                                                            <Columns>
                                                                <ext:Column runat="server"
                                                                    Sortable="true"
                                                                    DataIndex="Campo"
                                                                    Text="Field"
                                                                    Flex="1"
                                                                    Align="Start">
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    Sortable="true"
                                                                    Text="Search"
                                                                    Flex="1"
                                                                    DataIndex="Busqueda"
                                                                    Align="Start">
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <View>
                                                            <ext:GridView runat="server" LoadMask="false" />
                                                        </View>
                                                        <Plugins>
                                                            <ext:GridFilters runat="server" />
                                                        </Plugins>
                                                    </ext:GridPanel>
                                                </Items>
                                            </ext:Panel>

                                            <%--MY FILTERS PANEL--%>
                                            <ext:Panel
                                                AnchorVertical="100%" AnchorHorizontal="100%"
                                                ID="pnGridsAsideMyFilters"
                                                runat="server"
                                                PaddingSpec="15 0 20 15"
                                                Border="false"
                                                Header="false"
                                                Scrollable="Vertical"
                                                OverflowY="Scroll"
                                                Hidden="true"
                                                Cls="Whitebg">
                                                <DockedItems>
                                                    <ext:Label
                                                        MarginSpec="10 0 12 0"
                                                        Dock="Top"
                                                        meta:resourcekey="lblGrid"
                                                        ID="Label3"
                                                        runat="server"
                                                        IconCls="btn-CFilter"
                                                        Cls="lblHeadAside"
                                                        Text="<%$ Resources:Comun, strMisFiltros %>">
                                                    </ext:Label>
                                                </DockedItems>
                                                <Items>
                                                    <ext:GridPanel
                                                        ID="GridMyFilters"
                                                        runat="server"
                                                        Header="false"
                                                        Border="false"
                                                        Scrollable="Vertical"
                                                        Cls="GridMyFiltersV2">
                                                        <Listeners>
                                                        </Listeners>
                                                        <Store>
                                                            <ext:Store
                                                                runat="server"
                                                                PageSize="10"
                                                                AutoLoad="true">
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="ID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="GestionFiltroID" />
                                                                            <ext:ModelField Name="UsuarioID" />
                                                                            <ext:ModelField Name="NombreFiltro" />
                                                                            <ext:ModelField Name="JsonItemsFiltro" />
                                                                            <ext:ModelField Name="Pagina" />
                                                                            <ext:ModelField Name="check" Type="Boolean" DefaultValue="false" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:Store>
                                                        </Store>
                                                        <ColumnModel runat="server">
                                                            <Columns>
                                                                <ext:TemplateColumn runat="server" DataIndex="" MenuDisabled="true" Text="" Flex="5">
                                                                    <Template runat="server">
                                                                        <Html>
                                                                            <tpl for=".">
                                                                                <div class="GMFdiv1">
                                                                                    <div class="GMFdivNombre" title="{NombreFiltro}">
                                                                                        {NombreFiltro}
                                                                                    </div>
                                                                                    <div class="">
                                                                                        <button class="GMFbtnBorrar" type="button" onclick="alert('Hello world!')"></button>
                                                                                    </div>
                                                                                    <div class="GMFdivbtn2">
                                                                                        <button class="GMFbtnEditar" type="button" onclick="alert('Hello world!')"></button>
                                                                                    </div>
                                                                                    <div class="GMFdivbtn3">
                                                                                        <button class="GMFbtnAplicaFiltro" type="button" onclick="alert('Hello world!')"></button>
                                                                                    </div>
                                                                                </div>
                                                                            </tpl>
                                                                        </Html>
                                                                    </Template>
                                                                </ext:TemplateColumn>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <View>
                                                            <ext:GridView runat="server" LoadMask="false" />
                                                        </View>
                                                        <Plugins>
                                                            <%-- <ext:GridFilters runat="server" />--%>
                                                        </Plugins>
                                                    </ext:GridPanel>
                                                </Items>
                                            </ext:Panel>
                                            <ext:Panel
                                                AnchorHorizontal="100%"
                                                ID="pnQuickFilters"
                                                runat="server"
                                                PaddingSpec="15 20 20 15"
                                                Border="false"
                                                Layout="VBoxLayout"
                                                Header="false"
                                                Scrollable="Vertical"
                                                OverflowY="Scroll"
                                                Hidden="false"
                                                Cls="Whitebg">
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                </LayoutConfig>
                                                <DockedItems>
                                                    <ext:Label
                                                        MarginSpec="10 0 8 0"
                                                        Dock="Top"
                                                        meta:resourcekey="lblGrid"
                                                        ID="Label4"
                                                        runat="server"
                                                        IconCls="btn-QuickFilters"
                                                        Cls="lblHeadAside"
                                                        Text="Quick Filters">
                                                    </ext:Label>
                                                    <ext:Toolbar runat="server" ID="Toolbar4" Dock="Bottom" Padding="0" MarginSpec="8 8 0 0">
                                                        <Items>
                                                            <ext:Button runat="server" ID="Button9" Cls="btn-secondary" Text="Save Filter" Focusable="false" PressedCls="none" Hidden="false" Flex="10" Handler="showwinSaveQF()"></ext:Button>
                                                            <ext:ToolbarFill Flex="1"></ext:ToolbarFill>
                                                            <ext:Button runat="server" ID="Button10" Cls="btn-ppal " Text="Apply Filter" Focusable="false" PressedCls="none" Hidden="false" Flex="10"></ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Items>
                                                    <ext:MultiCombo
                                                        runat="server"
                                                        WrapBySquareBrackets="true"
                                                        LabelAlign="Top"
                                                        Cls="comboGrid"
                                                        FieldLabel="Phase">
                                                        <Items>
                                                            <ext:ListItem Text="Item 1" Value="1" />
                                                            <ext:ListItem Text="Item 2" Value="2" />
                                                            <ext:ListItem Text="Item 3" Value="3" />
                                                            <ext:ListItem Text="Item 4" Value="4" />
                                                            <ext:ListItem Text="Item 5" Value="5" />
                                                        </Items>
                                                        <SelectedItems>
                                                            <ext:ListItem Value="2" />
                                                            <ext:ListItem Index="4" />
                                                        </SelectedItems>
                                                    </ext:MultiCombo>
                                                    <ext:ComboBox
                                                        runat="server"
                                                        LabelAlign="Top"
                                                        Cls="comboGrid"
                                                        FieldLabel="Module">
                                                        <Items>
                                                            <ext:ListItem Text="Item 1" Value="1" />
                                                            <ext:ListItem Text="Item 2" Value="2" />
                                                            <ext:ListItem Text="Item 3" Value="3" />
                                                            <ext:ListItem Text="Item 4" Value="4" />
                                                            <ext:ListItem Text="Item 5" Value="5" />
                                                        </Items>
                                                    </ext:ComboBox>
                                                    <ext:Panel runat="server" ID="wrapSelectoresFecha" Layout="HBoxLayout" MarginSpec="0 17 0 0">
                                                        <Items>

                                                            <ext:DateField
                                                                runat="server"
                                                                LabelAlign="Top"
                                                                Cls=""
                                                                Flex="1"
                                                                MarginSpec="0 8 0 0"
                                                                FieldLabel="Start Date">
                                                            </ext:DateField>
                                                            <ext:DateField
                                                                MarginSpec="0 0 0 8"
                                                                runat="server"
                                                                LabelAlign="Top"
                                                                Flex="1"
                                                                Cls=""
                                                                FieldLabel="End Date">
                                                            </ext:DateField>

                                                        </Items>
                                                    </ext:Panel>
                                                    <ext:ComboBox
                                                        runat="server"
                                                        LabelAlign="Top"
                                                        Cls="comboGrid"
                                                        FieldLabel="User">
                                                        <Items>
                                                            <ext:ListItem Text="Item 1" Value="1" />
                                                            <ext:ListItem Text="Item 2" Value="2" />
                                                            <ext:ListItem Text="Item 3" Value="3" />
                                                            <ext:ListItem Text="Item 4" Value="4" />
                                                            <ext:ListItem Text="Item 5" Value="5" />
                                                        </Items>
                                                    </ext:ComboBox>

                                                    <ext:ComboBox
                                                        runat="server"
                                                        LabelAlign="Top"
                                                        Cls="comboGrid"
                                                        FieldLabel="User">
                                                        <Items>
                                                            <ext:ListItem Text="Item 1" Value="1" />
                                                            <ext:ListItem Text="Item 2" Value="2" />
                                                            <ext:ListItem Text="Item 3" Value="3" />
                                                            <ext:ListItem Text="Item 4" Value="4" />
                                                            <ext:ListItem Text="Item 5" Value="5" />
                                                        </Items>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>

                                    <%-- PANEL GESTION COLUMNAS--%>

                                    <%-- PANEL MORE INFO--%>

                                    <ext:Panel
                                        meta:resourcekey="ctAsideR"
                                        ID="ctAsideRInfo"
                                        runat="server"
                                        Border="false"
                                        Header="false"
                                        Layout="AnchorLayout"
                                        Hidden="true"
                                        Cls="">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="MenuNavPnInfo" Cls="noBorder" Padding="0" Dock="Left" Layout="VBoxLayout">
                                                <Items>
                                                    <ext:Button runat="server" Padding="0" Margin="0" ID="btnMoreInfo" Cls="btnInfo-asR btnNoBorder" Handler="displayMenuWOInfo('pnInfoWO')"></ext:Button>
                                                    <ext:Button runat="server" Padding="0" Margin="0" ID="btnMoreInfoWOSites" Cls="btntask-asR btnNoBorder" Handler="displayMenuWOInfo('pnInfoWOSites')"></ext:Button>
                                                    <ext:Button runat="server" Padding="0" Margin="0" ID="btnMoreInfoWOInventory" Cls="btnInfo-asR btnNoBorder" Handler="displayMenuWOInfo('pnInfoWOInventory')" Hidden="true"></ext:Button>
                                                    <ext:Button runat="server" Padding="0" Margin="0" ID="btnMoreInfoWOTickets" Cls="btnInfo-asR btnNoBorder" Handler="displayMenuWOInfo('pnInfoWOTickets')" Hidden="true"></ext:Button>
                                                    <ext:Button runat="server" Padding="0" Margin="0" ID="btnMoreInfoWOCalendar" Cls="btncalendar-asR btnNoBorder" Handler="displayMenuWOInfo('pnInfoWOCalendar')"></ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                            <%--PANELS--%>
                                            <ext:Panel
                                                MarginSpec="10 0 0 0"
                                                ID="pnGridsAsideInfo"
                                                runat="server"
                                                AnchorVertical="100%" AnchorHorizontal="100%"
                                                Border="false"
                                                OverflowY="Auto"
                                                Header="false"
                                                Cls="d-inline-block"
                                                Layout="AnchorLayout"
                                                Hidden="false">
                                                <Listeners>
                                                </Listeners>
                                                <Items>
                                                    <%--MORE INFO SITE PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoWO"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside Whitebg"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                MarginSpec="10 0 18 16"
                                                                Dock="Top"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoWO"
                                                                runat="server"
                                                                IconCls="ico-head-info"
                                                                Cls="lblHeadAside"
                                                                Text="Work Orders">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoWorkOrders">
                                                                    <tbody id="bodyTablaInfoWorkOrders">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <%--MORE INFO SITE PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoWOSites"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside Whitebg"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                MarginSpec="10 0 18 16"
                                                                Dock="Top"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoWOSite"
                                                                runat="server"
                                                                IconCls="ico-head-info"
                                                                Cls="lblHeadAside"
                                                                Text="Work Orders Site">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoWOStite">
                                                                    <tbody id="bodyTablaInfoWOSite">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <%--MORE INFO SITE PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoWOInventory"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside Whitebg"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                MarginSpec="10 0 18 16"
                                                                Dock="Top"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoWOInventory"
                                                                runat="server"
                                                                IconCls="ico-head-info"
                                                                Cls="lblHeadAside"
                                                                Text="Work Orders Inventory">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoWOInventory">
                                                                    <tbody id="bodyTablaInfoWOInventory">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <%--MORE INFO SITE PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoWOTickets"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside Whitebg"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                MarginSpec="10 0 18 16"
                                                                Dock="Top"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoWOTickets"
                                                                runat="server"
                                                                IconCls="ico-head-info"
                                                                Cls="lblHeadAside"
                                                                Text="Work Orders Tickets">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoWOTickets">
                                                                    <tbody id="bodyTablaInfoWOTickets">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <%--MORE INFO SITE PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoWOCalendar"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside Whitebg"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                MarginSpec="10 0 18 16"
                                                                Dock="Top"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoWOCalendar"
                                                                runat="server"
                                                                IconCls="ico-head-info"
                                                                Cls="lblHeadAside"
                                                                Text="Work Orders Calendar">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoWOCalendar">
                                                                    <tbody id="bodyTablaInfoWOCalendar">
                                                                    </tbody>
                                                                </table>
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

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
