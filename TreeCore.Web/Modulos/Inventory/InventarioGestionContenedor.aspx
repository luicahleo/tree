<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventarioGestionContenedor.aspx.cs" Inherits="TreeCore.ModInventario.InventarioGestionContenedor" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/Modulos/Inventory/css/styleInventarioCategorias.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <link href="css/styleInventarioGestionContenedor.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" DisableViewState="true">
            </ext:ResourceManager>

            <ext:Hidden ID="hdEmplazamientoID" runat="server" />
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdVistaPlantilla" runat="server" />
            <ext:Hidden ID="hdFiltroID" runat="server" />
            <ext:Hidden ID="hdCategoriaActiva" runat="server" />
            <ext:Hidden ID="hdNombreCategoriaActiva" runat="server" />
            <ext:Hidden ID="hdQuery" runat="server" />
            <ext:Hidden ID="hdColumnas" runat="server" />
            <ext:Hidden ID="hdFiltros" runat="server" />

            <ext:Store runat="server"
                ID="storeCategorias"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeCategorias_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="InventarioCategoriaID" runat="server">
                        <Fields>
                            <ext:ModelField Name="InventarioCategoriaID" Type="Int" />
                            <ext:ModelField Name="InventarioCategoria" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="InventarioCategoria" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Window ID="WinCategorias"
                runat="server"
                Title="<%$ Resources:Comun, strCategorias %>"
                meta:resourceKey="lblExportTitle"
                Width="372"
                MaxWidth="372"
                Height="180"
                Modal="true"
                Centered="true"
                Cls="winForm-respSimple"
                Scrollable="Vertical"
                Layout="VBoxLayout"
                Hidden="true">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" />
                </LayoutConfig>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="tlbButtons"
                        Cls="tbGrey"
                        Dock="Bottom"
                        PaddingSpec="0 10 16 10">
                        <Items>
                            <ext:ToolbarFill Flex="8">
                            </ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnCancelar"
                                Cls="btn-cancel"
                                MinWidth="100"
                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="3"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="#{WinConfirmExport}.hide();#{formDownload}.getForm().reset();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnDescargar"
                                Cls="btn-ppal btnBold "
                                MinWidth="120"
                                Text="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="2"
                                Disabled="true"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="DescargarPlantilla();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel runat="server">
                        <Items>
                            <ext:MultiCombo runat="server"
                                ID="cmbCategorias"
                                QueryMode="Local"
                                StoreID="storeCategorias"
                                LabelAlign="Top"
                                DisplayField="InventarioCategoria"
                                ValueField="InventarioCategoriaID"
                                AllowBlank="false"
                                MarginSpec="10"
                                MaxWidth="350"
                                WidthSpec="90%"
                                FieldLabel="<%$ Resources:Comun, strCategorias %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                <CustomConfig>
                                    <ext:ConfigItem Name="MaxSelection" Value="50" />
                                </CustomConfig>
                                <Validator Fn="ValidarCmbCategorias" />
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
                            </ext:MultiCombo>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormValid(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Window ID="winViews"
                runat="server"
                Title="<%$ Resources:Comun, strVista %>"
                meta:resourceKey="lblExportTitle"
                Width="372"
                MaxWidth="372"
                Height="180"
                Modal="true"
                Centered="true"
                Cls="winForm-respSimple"
                Scrollable="Vertical"
                Layout="VBoxLayout"
                Hidden="true">
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" />
                </LayoutConfig>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="Toolbar1"
                        Cls="tbGrey"
                        Dock="Bottom"
                        PaddingSpec="0 10 16 10">
                        <Items>
                            <ext:ToolbarFill Flex="8">
                            </ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="Button1"
                                Cls="btn-cancel"
                                MinWidth="100"
                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="3"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="#{WinConfirmExport}.hide();#{formDownload}.getForm().reset();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="Button2"
                                Cls="btn-ppal btnBold "
                                MinWidth="120"
                                Text="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="2"
                                Disabled="true"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="DescargarPlantilla();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel runat="server">
                        <Items>
                            <ext:MultiCombo runat="server"
                                ID="MultiCombo1"
                                QueryMode="Local"
                                StoreID="storeCategorias"
                                LabelAlign="Top"
                                DisplayField="InventarioCategoria"
                                ValueField="InventarioCategoriaID"
                                AllowBlank="false"
                                MarginSpec="10"
                                MaxWidth="350"
                                WidthSpec="90%"
                                FieldLabel="<%$ Resources:Comun, strCategorias %>"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                <CustomConfig>
                                    <ext:ConfigItem Name="MaxSelection" Value="50" />
                                </CustomConfig>
                                <Validator Fn="ValidarCmbCategorias" />
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
                            </ext:MultiCombo>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormValid(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Cls=""
                Layout="FitLayout">
                <Content>
                    <ext:Button runat="server" ID="btnCollapseAsR" Cls="btn-trans btnCollapseAsRClosedv2" Hidden="false"
                        ToolTip="<%$ Resources:Comun, strAbrirMenu %>">
                        <Listeners>
                            <Click Fn="hidePn" />
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
                                    <ext:Toolbar runat="server" ID="Toolbar3" Cls="tbGrey" Dock="Top" Hidden="false" MinHeight="36">
                                        <Items>
                                            <ext:HyperlinkButton runat="server"
                                                ID="HyperlinkButton3"
                                                Cls="lnk-navView lnk-noLine navActivo"
                                                meta:resourceKey="lnkCategorias"
                                                Text="<%$ Resources:Comun, strVistaCategoria %>">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="HyperlinkButton4"
                                                Cls="lnk-navView lnk-noLine"
                                                meta:resourceKey="lnkVinculaciones"
                                                Text="<%$ Resources:Comun, strVinculacion %>">
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
                                    <ext:Panel ID="ctMain1" runat="server" Flex="1" Layout="FitLayout">
                                        <Items>
                                            <ext:Container ID="hugeCt" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain"
                                                    runat="server"
                                                    Url="../Inventory/InventarioCategoryView.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                    <Params>
                                                        <ext:Parameter Name="EmplazamientoID" Value="#{hdEmplazamientoID.Value}" Mode="Raw" />
                                                        <ext:Parameter Name="VistaPlantilla" Value="#{hdVistaPlantilla.Value}" Mode="Raw" />
                                                    </Params>
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain2" runat="server" Hidden="true" Layout="FitLayout" Flex="1">
                                        <Items>
                                            <ext:Container ID="InventarioGestion_Content" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderGestion_Content"
                                                    runat="server"
                                                    Url="../Inventory/InventarioGestion_Content.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                    <Params>
                                                        <ext:Parameter Name="EmplazamientoID" Value="#{hdEmplazamientoID.Value}" Mode="Raw" />
                                                    </Params>
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
                                Header="false" Border="false" Width="380" Hidden="false">
                                <Listeners>
                                    <AfterRender Handler="ResizerAside(this)"></AfterRender>
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
                                        Text="<%$ Resources:Comun, strAtributos %>">
                                    </ext:Label>
                                </DockedItems>
                                <Items>
                                    <ext:Panel
                                        meta:resourcekey="ctAsideR"
                                        ID="ctAsideR"
                                        runat="server"
                                        Border="false"
                                        Header="false"
                                        Layout="AnchorLayout"
                                        Cls="">
                                        <Items>
                                            <%--LEFT TABS MENU--%>
                                            <ext:Panel ID="mnAsideR" runat="server" Border="false" Header="false" AnchorVertical="100%" AnchorHorizontal="14%">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        meta:resourcekey="btnMyF2"
                                                        ID="btnMyF2"
                                                        Cls="btnFiltersPlus-asR"
                                                        ToolTip="<%$ Resources:Comun, strCrearFiltro %>"
                                                        Handler="displayMenuInventary('pnCFilters')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        meta:resourcekey="btnMyFilters"
                                                        ID="btnMyFilters"
                                                        Cls="btnMyFilters-asR"
                                                        ToolTip="<%$ Resources:Comun, strMisFiltros %>"
                                                        Handler="displayMenuInventary('pnGridsAsideMyFilters')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnQuickFilters"
                                                        Cls="btnquickFilters-asR"
                                                        ToolTip="<%$ Resources:Comun, strFiltrosRapidos %>"
                                                        Handler="displayMenuInventary('pnQuickFilters')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        meta:resourcekey="btnMyViews"
                                                        ID="btnMyViews"
                                                        Cls="btn-columns-asR"
                                                        ToolTip="<%$ Resources:Comun, strVista %>"
                                                        Handler="displayMenuInventary('pnGridsAsideMyViews')">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Panel>
                                            <%--PANELS--%>

                                            <ext:Panel
                                                ID="pnGridsAside"
                                                runat="server"
                                                AnchorVertical="100%" AnchorHorizontal="86%"
                                                Border="false"
                                                OverflowY="Auto"
                                                Header="false"
                                                Layout="AnchorLayout">
                                                <Listeners>
                                                </Listeners>
                                                <Items>
                                                    <%--CREATE FILTERS PANEL--%>
                                                    <ext:Panel
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Hidden="false"
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
                                                                        EmptyText="<%$ Resources:Comun, strNombreFiltro %>">
                                                                        <Listeners>
                                                                            <ValidityChange Fn="NombreFiltroValido" />
                                                                        </Listeners>
                                                                    </ext:TextField>
                                                                    <ext:Button
                                                                        MarginSpec="0 0 0 6"
                                                                        Flex="2"
                                                                        runat="server"
                                                                        ID="btnFilter1"
                                                                        Cls="btn-ppal"
                                                                        Text="New Filter">
                                                                        <Listeners>
                                                                            <Click Fn="newFilter" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Container>
                                                            <ext:Container runat="server" ID="fila2F" MarginSpec="10 0 0 0">
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
                                                                        WidthSpec="100%"
                                                                        Mode="Local"
                                                                        QueryMode="Local"
                                                                        Cls="pnForm fieldFilter"
                                                                        ValueField="Campo">
                                                                        <Store>
                                                                            <ext:Store
                                                                                ID="storeCampos"
                                                                                runat="server"
                                                                                OnReadData="storeCampos_Refresh"
                                                                                AutoLoad="false">
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="ID">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="ID" />
                                                                                            <ext:ModelField Name="Campo" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                            <ext:ModelField Name="TypeData" />
                                                                                            <ext:ModelField Name="TipoCampo" />
                                                                                            <ext:ModelField Name="QueryValores" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                                <Sorters>
                                                                                    <ext:DataSorter Property="Name" Direction="ASC" />
                                                                                </Sorters>
                                                                                <Listeners>
                                                                                    <DataChanged Fn="beforeLoadCmbField" />
                                                                                </Listeners>
                                                                            </ext:Store>
                                                                        </Store>
                                                                        <Listeners>
                                                                            <Select Fn="selectField" />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                    <ext:TextField runat="server"
                                                                        Flex="1"
                                                                        WidthSpec="100%"
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
                                                                    <ext:DateField
                                                                        runat="server"
                                                                        ID="dateInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        Cls="pnForm"
                                                                        WidthSpec="100%"
                                                                        Hidden="true"
                                                                        Format="dd/MM/yyyy">
                                                                    </ext:DateField>
                                                                    <ext:NumberField
                                                                        runat="server"
                                                                        ID="numberInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        Cls="pnForm"
                                                                        WidthSpec="100%"
                                                                        Hidden="true">
                                                                    </ext:NumberField>
                                                                    <ext:ComboBox
                                                                        ID="cmbOperatorField"
                                                                        runat="server"
                                                                        FieldLabel="<%$ Resources:Comun, strOperador %>"
                                                                        DisplayField="Name"
                                                                        EmptyText="<%$ Resources:Comun, strOperador %>"
                                                                        Cls="pnForm"
                                                                        Flex="2"
                                                                        WidthSpec="70%"
                                                                        LabelAlign="Top"
                                                                        Hidden="true"
                                                                        QueryMode="Local">
                                                                        <Items>
                                                                            <ext:ListItem Text="=" Value="IGUAL" />
                                                                            <ext:ListItem Text="<" Value="MENOR" />
                                                                            <ext:ListItem Text=">" Value="MAYOR" />
                                                                        </Items>
                                                                    </ext:ComboBox>
                                                                    <ext:MultiCombo runat="server"
                                                                        Hidden="true"
                                                                        ID="cmbTiposDinamicos"
                                                                        FieldLabel="<%$ Resources:Comun, strTipoDinamico %>"
                                                                        LabelAlign="Top"
                                                                        DisplayField="Name"
                                                                        EmptyText="<%$ Resources:Comun, strTipoDinamico %>"
                                                                        Flex="1"
                                                                        WidthSpec="100%"
                                                                        Cls="pnForm"
                                                                        ValueField="ID"
                                                                        QueryMode="Local">
                                                                        <Store>
                                                                            <ext:Store
                                                                                ID="storeTiposDinamicos"
                                                                                runat="server"
                                                                                AutoLoad="false"
                                                                                OnReadData="storeTiposDinamicos_Refresh">
                                                                                <Proxy>
                                                                                    <ext:PageProxy />
                                                                                </Proxy>
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="ID">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="ID" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                                <Listeners>
                                                                                    <%--<DataChanged Fn="beforeLoadCmbField" />--%>
                                                                                </Listeners>
                                                                            </ext:Store>
                                                                        </Store>
                                                                    </ext:MultiCombo>
                                                                    <ext:Checkbox runat="server"
                                                                        ID="chkTiposDinamicos"
                                                                        FieldLabel="<%$ Resources:Comun, strTipoDinamico %>"
                                                                        Hidden="true">
                                                                    </ext:Checkbox>
                                                                    <ext:Component runat="server" Flex="1"></ext:Component>
                                                                    <ext:Button
                                                                        runat="server" meta:resourcekey="ColMas"
                                                                        ID="btnAdd"
                                                                        MarginSpec="6 0 0 0"
                                                                        Flex="1"
                                                                        IconCls="ico-addBtn"
                                                                        Cls="btn-mini-ppal btnAdd"
                                                                        Text="<%$ Resources:Comun, jsAgregar %>">
                                                                        <Listeners>
                                                                            <Click Fn="addElementFilter" />
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
                                                                                Enabled="false"
                                                                                Text="<%$ Resources:Comun, btnGuardar.Text %>">
                                                                                <Listeners>
                                                                                    <Click Fn="saveFilter" />
                                                                                </Listeners>
                                                                            </ext:Button>
                                                                            <ext:Button
                                                                                runat="server"
                                                                                meta:resourcekey="btnAplyFilter"
                                                                                ID="btnAplyFilter"
                                                                                Cls="btn-end"
                                                                                Enabled="false"
                                                                                Text="<%$ Resources:Comun, strAplicar %>">
                                                                                <Listeners>
                                                                                    <Click Fn="aplyFilter" />
                                                                                </Listeners>
                                                                            </ext:Button>
                                                                        </Items>
                                                                    </ext:Toolbar>
                                                                </DockedItems>
                                                                <Store>
                                                                    <ext:Store
                                                                        runat="server"
                                                                        PageSize="10"
                                                                        ID="storeFiltros"
                                                                        AutoLoad="false">
                                                                        <Model>
                                                                            <ext:Model runat="server" IDProperty="Campo">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="Name" />
                                                                                    <ext:ModelField Name="Campo" />
                                                                                    <ext:ModelField Name="Value" />
                                                                                    <ext:ModelField Name="DisplayValue" />
                                                                                    <ext:ModelField Name="TypeData" />
                                                                                    <ext:ModelField Name="Operador" />
                                                                                    <ext:ModelField Name="TipoCampo" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                    </ext:Store>
                                                                </Store>
                                                                <ColumnModel runat="server">
                                                                    <Columns>
                                                                        <ext:Column runat="server"
                                                                            Sortable="true"
                                                                            DataIndex="Name"
                                                                            Text="Field"
                                                                            Flex="1"
                                                                            Align="Start">
                                                                        </ext:Column>
                                                                        <ext:Column runat="server"
                                                                            Sortable="true"
                                                                            Text="Search"
                                                                            Flex="1"
                                                                            DataIndex="DisplayValue"
                                                                            Align="Start">
                                                                        </ext:Column>

                                                                        <ext:CommandColumn ID="colEliminarFiltro" runat="server" Width="50" Align="Center">
                                                                            <Commands>
                                                                                <ext:GridCommand CommandName="EliminarFiltro" IconCls="ico-close">
                                                                                </ext:GridCommand>
                                                                            </Commands>
                                                                            <Listeners>
                                                                                <Command Fn="EliminarFiltro" />
                                                                            </Listeners>
                                                                        </ext:CommandColumn>
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
                                                                        ID="storeMyFilters"
                                                                        runat="server"
                                                                        PageSize="10"
                                                                        OnReadData="storeMyFilters_Refresh"
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
                                                                                                <button class="GMFbtnBorrar" type="button" filtroid="{GestionFiltroID}" onclick="EliminarFiltroGuardado(this)"></button>
                                                                                            </div>

                                                                                            <div class="GMFdivbtn2">
                                                                                                <button class="GMFbtnEditar" type="button" filtroid="{GestionFiltroID}" jsonfiltros='{JsonItemsFiltro}' nombre="{NombreFiltro}" onclick="EditarFiltro(this)"></button>
                                                                                            </div>

                                                                                            <div class="GMFdivbtn3">
                                                                                                <button class="GMFbtnAplicaFiltro" type="button" jsonfiltros='{JsonItemsFiltro}' onclick="AplicarFiltroGuardado(this)"></button>
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
                                                        Hidden="true"
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
                                                                Text="<%$ Resources:Comun, strFiltrosRapidos %>">
                                                            </ext:Label>
                                                            <ext:Toolbar runat="server" ID="Toolbar4" Dock="Bottom" Padding="0" MarginSpec="8 8 0 0">
                                                                <Items>
                                                                    <ext:Button
                                                                        runat="server"
                                                                        ID="Button10"
                                                                        Cls="btn-ppal "
                                                                        Text="<%$ Resources:Comun, strAplicarFiltro %>"
                                                                        Focusable="false"
                                                                        PressedCls="none"
                                                                        Hidden="false"
                                                                        Flex="1">
                                                                        <Listeners>
                                                                            <Click Fn="AplicaFiltros" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <Items>
                                                            <ext:ComboBox runat="server"
                                                                ID="cmbClientes"
                                                                DisplayField="Cliente"
                                                                ValueField="ClienteID"
                                                                WidthSpec="85%"
                                                                Cls="comboPnFiltros pos-boxGrid"
                                                                Hidden="true"
                                                                EmptyText="<%$ Resources:Comun, cmbClientes.EmptyText %>"
                                                                FieldLabel="<%$ Resources:Comun, cmbClientes.FieldLabel %>">
                                                                <Store>
                                                                    <ext:Store runat="server"
                                                                        ID="storeClientes"
                                                                        AutoLoad="false"
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
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Fn="SeleccionarCliente" />
                                                                    <TriggerClick Fn="RecargarClientes" />
                                                                </Listeners>
                                                                <Triggers>
                                                                    <ext:FieldTrigger
                                                                        IconCls="ico-reload"
                                                                        Hidden="true"
                                                                        QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                                        Weight="-1" />
                                                                </Triggers>
                                                            </ext:ComboBox>
                                                            <ext:MultiCombo
                                                                ID="cmbOperadores"
                                                                runat="server"
                                                                LabelAlign="Top"
                                                                WidthSpec="85%"
                                                                Cls="comboPnFiltros"
                                                                FieldLabel="<%$ Resources:Comun, strOperadores %>"
                                                                DisplayField="Nombre"
                                                                ValueField="EntidadID">
                                                                <Store>
                                                                    <ext:Store runat="server"
                                                                        ID="storeOperadores"
                                                                        AutoLoad="false"
                                                                        RemoteFilter="false"
                                                                        RemoteSort="false"
                                                                        OnReadData="storeOperadores_Refresh">
                                                                        <Proxy>
                                                                            <ext:PageProxy />
                                                                        </Proxy>
                                                                        <Model>
                                                                            <ext:Model runat="server" IDProperty="EntidadID">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="EntidadID" Type="Int" />
                                                                                    <ext:ModelField Name="Nombre" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                        <Sorters>
                                                                            <ext:DataSorter Property="Nombre" Direction="ASC" />
                                                                        </Sorters>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Fn="SeleccionarCombo" />
                                                                    <TriggerClick Fn="RecargarCombo" />
                                                                </Listeners>
                                                                <Triggers>
                                                                    <ext:FieldTrigger
                                                                        IconCls="ico-reload"
                                                                        Hidden="true"
                                                                        QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                                        Weight="-1" />
                                                                </Triggers>
                                                            </ext:MultiCombo>
                                                            <ext:MultiCombo
                                                                ID="cmbEstados"
                                                                runat="server"
                                                                LabelAlign="Top"
                                                                WidthSpec="85%"
                                                                Cls="comboPnFiltros"
                                                                FieldLabel="<%$ Resources:Comun, strEstados %>"
                                                                DisplayField="Nombre"
                                                                ValueField="InventarioElementoAtributoEstadoID">
                                                                <Store>
                                                                    <ext:Store runat="server"
                                                                        ID="storeEstados"
                                                                        AutoLoad="false"
                                                                        RemoteFilter="false"
                                                                        RemoteSort="false"
                                                                        OnReadData="storeEstados_Refresh">
                                                                        <Proxy>
                                                                            <ext:PageProxy />
                                                                        </Proxy>
                                                                        <Model>
                                                                            <ext:Model runat="server" IDProperty="InventarioElementoAtributoEstadoID">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="InventarioElementoAtributoEstadoID" Type="Int" />
                                                                                    <ext:ModelField Name="Nombre" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                        <Sorters>
                                                                            <ext:DataSorter Property="Nombre" Direction="ASC" />
                                                                        </Sorters>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Fn="SeleccionarCombo" />
                                                                    <TriggerClick Fn="RecargarCombo" />
                                                                </Listeners>
                                                                <Triggers>
                                                                    <ext:FieldTrigger
                                                                        IconCls="ico-reload"
                                                                        Hidden="true"
                                                                        QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                                        Weight="-1" />
                                                                </Triggers>
                                                            </ext:MultiCombo>
                                                            <ext:Panel runat="server"
                                                                ID="contenedorFechaCreacion"
                                                                Layout="HBoxLayout"
                                                                Height="100"
                                                                Cls="comboPnFiltros comboPnLateralHeight">
                                                                <DockedItems>
                                                                    <ext:Label runat="server"
                                                                        MarginSpec="10 0 8 0"
                                                                        Cls="lblHeadAside"
                                                                        Text="<%$ Resources:Comun, strFechaCreacion %>">
                                                                    </ext:Label>
                                                                </DockedItems>
                                                                <Items>
                                                                    <ext:DateField runat="server"
                                                                        ID="datMinDateCrea"
                                                                        FieldLabel="<%$ Resources:Comun, strFechaInicio %>"
                                                                        LabelAlign="Top"
                                                                        Flex="1"
                                                                        MarginSpec="0 8 0 0"
                                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                                        Cls="item-form dateField"
                                                                        ValidationGroup="FORM">
                                                                    </ext:DateField>
                                                                    <ext:ToolbarSeparator />
                                                                    <ext:DateField runat="server"
                                                                        ID="datMaxDateCrea"
                                                                        FieldLabel="<%$ Resources:Comun, strFechaFin %>"
                                                                        LabelAlign="Top"
                                                                        Flex="1"
                                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                                        Cls="item-form dateField"
                                                                        ValidationGroup="FORM">
                                                                    </ext:DateField>
                                                                </Items>
                                                            </ext:Panel>
                                                            <ext:Panel runat="server"
                                                                ID="contenedorFechaModificacion"
                                                                Layout="HBoxLayout"
                                                                Height="100"
                                                                Cls="comboPnFiltros comboPnLateralHeight">
                                                                <DockedItems>
                                                                    <ext:Label runat="server"
                                                                        MarginSpec="10 0 8 0"
                                                                        Cls="lblHeadAside"
                                                                        Text="<%$ Resources:Comun, strFechaModificacion %>">
                                                                    </ext:Label>
                                                                </DockedItems>
                                                                <Items>
                                                                    <ext:DateField runat="server"
                                                                        ID="datMinDateMod"
                                                                        FieldLabel="<%$ Resources:Comun, strFechaInicio %>"
                                                                        LabelAlign="Top"
                                                                        Flex="1"
                                                                        MarginSpec="0 8 0 0"
                                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                                        Cls="item-form dateField"
                                                                        ValidationGroup="FORM">
                                                                    </ext:DateField>
                                                                    <ext:ToolbarSeparator />
                                                                    <ext:DateField runat="server"
                                                                        ID="datMaxDateMod"
                                                                        FieldLabel="<%$ Resources:Comun, strFechaFin %>"
                                                                        LabelAlign="Top"
                                                                        Flex="1"
                                                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                                                        Cls="item-form dateField"
                                                                        ValidationGroup="FORM">
                                                                    </ext:DateField>
                                                                </Items>
                                                            </ext:Panel>
                                                            <ext:MultiCombo
                                                                ID="cmbUsuarios"
                                                                runat="server"
                                                                LabelAlign="Top"
                                                                WidthSpec="85%"
                                                                Cls="comboPnFiltros"
                                                                FieldLabel="<%$ Resources:Comun, strUsuarios %>"
                                                                DisplayField="NombreCompleto"
                                                                ValueField="UsuarioID">
                                                                <Store>
                                                                    <ext:Store runat="server"
                                                                        ID="storeUsuarios"
                                                                        AutoLoad="false"
                                                                        RemoteFilter="false"
                                                                        RemoteSort="false"
                                                                        OnReadData="storeUsuarios_Refresh">
                                                                        <Proxy>
                                                                            <ext:PageProxy />
                                                                        </Proxy>
                                                                        <Model>
                                                                            <ext:Model runat="server" IDProperty="UsuarioID">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="UsuarioID" Type="Int" />
                                                                                    <ext:ModelField Name="NombreCompleto" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                        <Sorters>
                                                                            <ext:DataSorter Property="NombreCompleto" Direction="ASC" />
                                                                        </Sorters>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Fn="SeleccionarCombo" />
                                                                    <TriggerClick Fn="RecargarCombo" />
                                                                </Listeners>
                                                                <Triggers>
                                                                    <ext:FieldTrigger
                                                                        IconCls="ico-reload"
                                                                        Hidden="true"
                                                                        QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                                        Weight="-1" />
                                                                </Triggers>
                                                            </ext:MultiCombo>
                                                        </Items>
                                                    </ext:Panel>

                                                    <%--MY VIEWS PANEL--%>
                                                    <ext:Panel
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        ID="pnGridsAsideMyViews"
                                                        runat="server"
                                                        PaddingSpec="15 0 20 15"
                                                        Border="false"
                                                        Header="false"
                                                        Scrollable="Vertical"
                                                        OverflowY="Scroll"
                                                        Hidden="true"
                                                        Cls="Whitebg">
                                                        <DockedItems>
                                                            <ext:Toolbar Hidden="false" runat="server" ID="Toolbar2" Dock="Top" Padding="0" MarginSpec="0">
                                                                <Items>
                                                                    <ext:Label
                                                                        MarginSpec="10 0 12 0"
                                                                        Dock="Top"
                                                                        meta:resourcekey="lblGrid"
                                                                        ID="lblMyViews"
                                                                        runat="server"
                                                                        IconCls="btn-columns-gr"
                                                                        Cls="lblHeadAside"
                                                                        Text="<%$ Resources:Comun, strVista %>">
                                                                    </ext:Label>
                                                                </Items>
                                                            </ext:Toolbar>
                                                            <ext:Toolbar Hidden="false" runat="server" ID="tlbGestionViews" Dock="Top" Padding="0" MarginSpec="8 8 0 0">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        ID="btnFiltosActivos"
                                                                        Cls="btn-toggleGrid"
                                                                        EnableToggle="true"
                                                                        Pressed="true"
                                                                        Width="42">
                                                                    </ext:Button>
                                                                    <ext:Label runat="server"
                                                                        Text="<%$ Resources:Comun, strIncluirFiltros %>">
                                                                    </ext:Label>
                                                                    <ext:ToolbarFill runat="server" />
                                                                    <ext:Button runat="server"
                                                                        Cls="btnOpciones-MnuV2"
                                                                        ID="btnOpciones">
                                                                        <Menu>
                                                                            <ext:Menu runat="server" ID="mnuView">
                                                                                <Items>
                                                                                    <ext:MenuItem
                                                                                        ID="mnuSaveAs"
                                                                                        Cls="borderTopV1 MenuItemNoArrow"
                                                                                        runat="server"
                                                                                        Text="<%$ Resources:Comun, strGuardarComo %>">
                                                                                        <Menu>
                                                                                            <ext:Menu runat="server">
                                                                                                <Items>
                                                                                                    <ext:TextField runat="server"
                                                                                                        ID="txtNameSaveAs"
                                                                                                        EmptyText="<%$ Resources:Comun, strNombre %>"
                                                                                                        Cls="item-form"
                                                                                                        MaxLength="50"
                                                                                                        Margin="8"
                                                                                                        AllowBlank="false">
                                                                                                        <Listeners>
                                                                                                            <ValidityChange Fn="SaveAsValid" />
                                                                                                        </Listeners>
                                                                                                    </ext:TextField>
                                                                                                </Items>
                                                                                                <DockedItems>
                                                                                                    <ext:Toolbar runat="server" Dock="Bottom">
                                                                                                        <Items>
                                                                                                            <ext:Button
                                                                                                                runat="server"
                                                                                                                ID="btnSaveSaveAs"
                                                                                                                Cls="btn-ppal"
                                                                                                                Text="<%$ Resources:Comun, strGuardar %>"
                                                                                                                Focusable="false"
                                                                                                                PressedCls="none"
                                                                                                                Hidden="false"
                                                                                                                Enabled="false"
                                                                                                                Flex="1">
                                                                                                                <Listeners>
                                                                                                                    <Click Fn="SaveAs" />
                                                                                                                </Listeners>
                                                                                                            </ext:Button>
                                                                                                        </Items>
                                                                                                    </ext:Toolbar>
                                                                                                </DockedItems>
                                                                                            </ext:Menu>
                                                                                        </Menu>
                                                                                    </ext:MenuItem>
                                                                                    <ext:MenuItem
                                                                                        meta:resourceKey="mnuDwldExcel"
                                                                                        ID="mnuRename"
                                                                                        Cls="borderTopV1 MenuItemNoArrow"
                                                                                        runat="server"
                                                                                        Text="Rename">
                                                                                        <Menu>
                                                                                            <ext:Menu runat="server">
                                                                                                <Items>
                                                                                                    <ext:TextField runat="server"
                                                                                                        ID="txtNameRename"
                                                                                                        EmptyText="<%$ Resources:Comun, strNuevoNombre %>"
                                                                                                        Cls="item-form"
                                                                                                        MaxLength="50"
                                                                                                        Margin="8"
                                                                                                        AllowBlank="false">
                                                                                                        <Listeners>
                                                                                                            <ValidityChange Fn="RenameValid" />
                                                                                                        </Listeners>
                                                                                                    </ext:TextField>
                                                                                                </Items>
                                                                                                <DockedItems>
                                                                                                    <ext:Toolbar runat="server" Dock="Bottom">
                                                                                                        <Items>
                                                                                                            <ext:Button
                                                                                                                runat="server"
                                                                                                                ID="btnSaveRename"
                                                                                                                Cls="btn-ppal"
                                                                                                                Text="<%$ Resources:Comun, strGuardar %>"
                                                                                                                Focusable="false"
                                                                                                                PressedCls="none"
                                                                                                                Hidden="false"
                                                                                                                Flex="1">
                                                                                                                <Listeners>
                                                                                                                    <Click Fn="Rename" />
                                                                                                                </Listeners>
                                                                                                            </ext:Button>
                                                                                                        </Items>
                                                                                                    </ext:Toolbar>
                                                                                                </DockedItems>
                                                                                            </ext:Menu>
                                                                                        </Menu>
                                                                                    </ext:MenuItem>
                                                                                    <ext:MenuItem
                                                                                        ID="mnuDefault"
                                                                                        Cls="borderTopV1"
                                                                                        Hidden="true"
                                                                                        runat="server"
                                                                                        Text="<%$ Resources:Comun, strMarcarDefecto %>">
                                                                                        <Listeners>
                                                                                            <Click Fn="SetDefault" />
                                                                                        </Listeners>
                                                                                        </ext:MenuItem>
                                                                                    <ext:MenuItem
                                                                                        ID="mnuIsDefault"
                                                                                        Cls="borderTopV1"
                                                                                        IconCls="ico-checked-16-gr"
                                                                                        runat="server"
                                                                                        Disabled="true"
                                                                                        Text="<%$ Resources:Comun, strMarcarDefecto %>">
                                                                                        </ext:MenuItem>
                                                                                    <ext:MenuItem
                                                                                        ID="mnuDelete"
                                                                                        Cls="borderTopV1 spanRedV2"
                                                                                        runat="server"
                                                                                        Text="<%$ Resources:Comun, jsEliminar %>" >
                                                                                        <Listeners>
                                                                                            <Click Fn="DeleteView" />
                                                                                        </Listeners>
                                                                                        </ext:MenuItem>
                                                                                </Items>

                                                                            </ext:Menu>
                                                                        </Menu>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Toolbar>
                                                            <ext:Toolbar Hidden="false" runat="server" ID="tlbCmbViews" Dock="Top" Padding="0" MarginSpec="8 8 0 0">
                                                                <Items>
                                                                    <ext:ComboBox runat="server"
                                                                        ID="cmbViews"
                                                                        LabelAlign="Top"
                                                                        WidthSpec="100%"
                                                                        DisplayField="Nombre"
                                                                        ValueField="CoreGestionVistaID"
                                                                        QueryMode="Local">
                                                                        <Store>
                                                                            <ext:Store runat="server"
                                                                                ID="storeViews"
                                                                                AutoLoad="false"
                                                                                RemoteFilter="false"
                                                                                RemoteSort="false"
                                                                                OnReadData="storeViews_Refresh">
                                                                                <Proxy>
                                                                                    <ext:PageProxy />
                                                                                </Proxy>
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="CoreGestionVistaID">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="CoreGestionVistaID" Type="Int" />
                                                                                            <ext:ModelField Name="Nombre" />
                                                                                            <ext:ModelField Name="JsonColumnas" />
                                                                                            <ext:ModelField Name="JsonFiltros" />
                                                                                            <ext:ModelField Name="Defecto" Type="Boolean" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                                <Sorters>
                                                                                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                                                                                </Sorters>
                                                                            </ext:Store>
                                                                        </Store>
                                                                        <Listeners>
                                                                            <Select Fn="SelectView" />
                                                                            <Change Fn="ChangeView" />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                </Items>
                                                            </ext:Toolbar>

                                                            <ext:Toolbar runat="server" ID="tlbBotonesViews" Dock="Bottom" Padding="0" MarginSpec="10 0 10 0" Cls="toolbar2cols" Height="30">
                                                                <Items>
                                                                    <ext:Button
                                                                        runat="server"
                                                                        ID="btnAplicarView"
                                                                        Cls="btn-ppal btn1col"
                                                                        Text="<%$ Resources:Comun, strAplicarVista %>"
                                                                        Focusable="false"
                                                                        PressedCls="none"
                                                                        Hidden="false"
                                                                        Flex="1">
                                                                        <Listeners>
                                                                            <Click Fn="AplicarView" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:Button
                                                                        runat="server"
                                                                        ID="btnGuardarView"
                                                                        Cls="btn-ppal btn1col"
                                                                        Text="<%$ Resources:Comun, strGuardarVista %>"
                                                                        Focusable="false"
                                                                        PressedCls="none"
                                                                        Hidden="false"
                                                                        Flex="1">
                                                                        <Listeners>
                                                                            <Click Fn="GuardarView" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <Content>
                                                            <ul id="contOdernacionColumnas" class="ordenacionColumnas">
                                                            </ul>
                                                        </Content>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>

                                    <ext:Panel
                                        meta:resourcekey="ctAsideR"
                                        ID="ctAsideRInfo"
                                        runat="server"
                                        Border="false"
                                        Header="false"
                                        Layout="AnchorLayout"
                                        Hidden="true"
                                        Cls="">
                                        <Items>
                                            <%--LEFT TABS MENU--%>
                                            <ext:Panel ID="mnAsideRInfo" Cls="d-inline-block" runat="server" Border="false" Header="false" AnchorVertical="100%" AnchorHorizontal="14%">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnMoreInfo"
                                                        Cls="btnInfo-asR"
                                                        Hidden="false"
                                                        ToolTip="Info Grid"
                                                        Handler="displayMenuInventaryInfo('pnGridInfo')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnMoreInfoVin"
                                                        Cls="btnClip-asR"
                                                        Hidden="false"
                                                        ToolTip="Inventary Info"
                                                        Handler="displayMenuInventaryInfo('pnGridVin')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnMoreInfoSites"
                                                        Cls="btnTorre-asR"
                                                        Hidden="false"
                                                        ToolTip="<%$ Resources:Comun, strEmplazamientos %>"
                                                        Handler="displayMenuInventaryInfo('pnSites')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnMoreInfoLocation"
                                                        Cls="location-asR"
                                                        Hidden="false"
                                                        ToolTip="<%$ Resources:Comun, strLocalizacion %>"
                                                        Handler="displayMenuInventaryInfo('pnInfoLocation')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnMoreInfoAdicional"
                                                        Cls="btnAdditional-asR"
                                                        Hidden="false"
                                                        ToolTip="<%$ Resources:Comun, strAdicional %>"
                                                        Handler="displayMenuInventaryInfo('pnInfoAtributos')">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Panel>

                                            <%--PANELS--%>
                                            <ext:Panel
                                                MarginSpec="0 0 30 0"
                                                ID="pnGridsAsideInfo"
                                                runat="server"
                                                AnchorVertical="100%" AnchorHorizontal="86%"
                                                Border="false"
                                                OverflowY="Auto"
                                                Header="false"
                                                Cls="d-inline-block"
                                                Layout="AnchorLayout"
                                                Hidden="false">
                                                <Listeners>
                                                </Listeners>
                                                <Items>
                                                    <ext:Panel ID="pnGridInfo"
                                                        runat="server"
                                                        Hidden="false"
                                                        Cls="grdIntoAside Whitebg">
                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="10 8 20 8"
                                                                meta:resourcekey="lblAsideNameR"
                                                                ID="Label2"
                                                                runat="server"
                                                                IconCls="ico-head-cog-gr"
                                                                Cls="lblHeadAside Whitebg"
                                                                Text="<%$ Resources:Comun, strAtributos %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoElementos">
                                                                    <tbody id="bodyTablaInfoElementos">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <ext:Panel ID="pnGridVin"
                                                        runat="server"
                                                        Hidden="false"
                                                        Cls="grdIntoAside Whitebg">
                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="10 8 20 8"
                                                                meta:resourcekey="lblAsideNameR"
                                                                ID="LabelVin"
                                                                runat="server"
                                                                IconCls="ico-clip-gr"
                                                                Cls="lblHeadAside Whitebg"
                                                                Text="<%$ Resources:Comun, strVinculacion %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoVinPadres">
                                                                    <tbody id="bodyTablaInfoVinPadres">
                                                                    </tbody>
                                                                </table>
                                                                <table class="tmpCol-table" id="tablaInfoVinHijas">
                                                                    <tbody id="bodyTablaInfoVinHijas">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <ext:Panel ID="pnSites"
                                                        runat="server"
                                                        Hidden="false"
                                                        Cls="grdIntoAside Whitebg">
                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="10 8 20 8"
                                                                meta:resourcekey="lblAsideNameR"
                                                                ID="Label1"
                                                                runat="server"
                                                                IconCls="ico-head-cog-gr"
                                                                Cls="lblHeadAside Whitebg"
                                                                Text="<%$ Resources:Comun, strEmplazamientos %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoSites">
                                                                    <tbody id="bodyTablaInfoSites">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoLocation"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="0 8 20 8"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoLocation"
                                                                runat="server"
                                                                IconCls="ico-head-site"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strLocalizacion %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoLocation">
                                                                    <tbody id="bodyTablaInfoLocation">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoAtributos"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="0 8 20 8"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoAtributos"
                                                                runat="server"
                                                                IconCls="ico-head-additional-gr"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strAdicional %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoAtributos">
                                                                    <tbody id="bodyTablaInfoAtributos">
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
        </div>
    </form>
</body>
</html>
