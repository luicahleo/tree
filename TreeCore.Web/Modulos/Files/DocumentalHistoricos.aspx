<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentalHistoricos.aspx.cs" Inherits="TreeCore.ModGlobal.pages.DocumentosHistoricos" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <!--<script type="text/javascript" src="../../JS/common.js"></script>-->
    <script type="text/javascript" src="../../Componentes/js/GridSimple.js"></script>
    <link href="../../Componentes/css/GridEmplazamientos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/DocumentosHistoricos.js"></script>
</head>
<body class="bodyFMenu">
    <link href="/ModGlobal/pages/css/EmplazamientosHistoricos.css" rel="stylesheet" type="text/css" />

    <%-- INICIO IMPORTS CALENDARIO --%>

    <link href="../../CSS/evo-calendar.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JS/evo-calendar.js"></script>

    <%-- FIN IMPORTS CALENDARIO --%>

    <form id="form1" runat="server">

        <%-- INICIO HIDDEN --%>

        <ext:Hidden ID="hdCliID" runat="server" />
        <ext:Hidden ID="CurrentControl" runat="server" />
        <ext:Hidden ID="hdTabName" runat="server" />
        <ext:Hidden ID="hdPageName" runat="server" />
        <ext:Hidden ID="hdLocale" runat="server" />

        <%-- FIN HIDDEN --%>

        <div>
            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">

                <Listeners>
                </Listeners>

            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>

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
                    <ext:Toolbar runat="server" ID="Toolbar3" Cls=" greytb" Dock="Bottom" Padding="20">
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

            <ext:Viewport runat="server" ID="MainVwP" OverflowY="auto" Layout="FitLayout">
                <%--<Content>
                    <ext:Button runat="server"
                        ID="btnCollapseAsRClosed"
                        Cls="btn-trans btnCollapseAsRClosedv3"
                        OnDirectClick="ShowHidePnAsideR"
                        Handler="hidePnLite();"
                        Disabled="false"
                        Hidden="false">
                    </ext:Button>
                </Content>--%>

                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">

                        <Items>

                            <%-------------TENER EN CUENTA QUE AL CENTER MAIN SE LE PUEDEN ACOPLAR SEGMENTOS PLEGABLES CON LAS "REGIONS"-------------%>


                            <%-- PANEL CENTRAL--%>


                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                MarginSpec="0 20 20 20"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain">

                                <DockedItems>
                                    <ext:Container runat="server"
                                        ID="WrapAlturaCabecera"
                                        MinHeight="60"
                                        Dock="Top"
                                        Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:Toolbar runat="server"
                                                ID="tbTitulo"
                                                Dock="Top"
                                                Cls="tbGrey tbTitleAlignBot tbNoborder"
                                                Hidden="false"
                                                Layout="ColumnLayout"
                                                Flex="1">
                                                <Items>
                                                    <ext:Label runat="server"
                                                        ID="lbltituloPrincipal"
                                                        Cls="TituloCabecera"
                                                        Height="25" />
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Container>
                                    <ext:Toolbar runat="server"
                                        ID="tbFiltersLabels"
                                        Dock="Top"
                                        Cls="tbGrey  tbNoborder tbFiltrosLabels"
                                        Hidden="false"
                                        OverflowHandler="Scroller"
                                        Flex="1"
                                        Padding="0"
                                        MarginSpec="0 0 0 -9">

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

                                <%-- A partir de aqui va el panel central dividido --%>
                                <Content>

                                    <ext:Panel runat="server"
                                        ID="wrapComponenteCentral"
                                        Layout="BorderLayout"
                                        BodyCls="tbGrey">

                                        <Listeners>
                                            <AfterRender Handler="showPanelsByWindowSize();"></AfterRender>
                                            <Resize Handler="showPanelsByWindowSize();"></Resize>
                                        </Listeners>

                                        <Items>

                                            <ext:GridPanel
                                                ForceFit="true"
                                                Hidden="false"
                                                MaxWidth="290"
                                                Flex="12"
                                                HideHeaders="true"
                                                runat="server"
                                                Cls="gridPanel grdNoHeader"
                                                Title="<%$ Resources:Comun, strHistorico %>"
                                                ID="gridPrincipal"
                                                EnableColumnHide="false"
                                                Scrollable="Vertical"
                                                Region="West"
                                                OverflowY="Auto">

                                                <Listeners>
                                                    <AfterRender Handler="GridColHandler(this)"></AfterRender>
                                                    <Resize Handler="GridColHandler(this)"></Resize>
                                                    <RowClick Fn="agregarColumnaDinamicaDocumentos"></RowClick>
                                                </Listeners>

                                                <DockedItems>
                                                    <ext:Toolbar runat="server" ID="tlbBase" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                                                        <Items>

                                                            <ext:Button runat="server"
                                                                ID="btnRefrescar"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                meta:resourceKey="btnRefrescar"
                                                                Cls="btnRefrescar"
                                                                Handler="Refrescar();" />

                                                            <ext:Button runat="server"
                                                                ID="btnDescargar"
                                                                ToolTip="Descargar"
                                                                meta:resourceKey="btnRefrescar"
                                                                Cls="btnDescargar"
                                                                Handler="Refrescar();" />

                                                            <%--<ext:Button runat="server"
                                                                ID="Button5"
                                                                ToolTip="Mostrar Filtros"
                                                                meta:resourceKey="btnRefrescar"
                                                                Cls="btnFiltros"
                                                                Handler="hideAsideR('panelFiltros')" />--%>
                                                        </Items>
                                                    </ext:Toolbar>

                                                    <ext:Container runat="server"
                                                        ID="tbfiltros"
                                                        Cls=""
                                                        Dock="Top">
                                                        <Content>
                                                            <local:toolbarFiltros
                                                                ID="cmpFiltro"
                                                                runat="server"
                                                                Stores="storePrincipal" 
                                                                MostrarComboFecha="false" 
                                                                QuitarBotonesFiltros="true"
                                                                Grid="gridPrincipal"
                                                                MostrarBusqueda="false" />
                                                        </Content>
                                                    </ext:Container>

                                                </DockedItems>

                                                <Store>
                                                    <ext:Store ID="storePrincipal" runat="server"
                                                        AutoLoad="true"
                                                        OnReadData="storePrincipal_Refresh"
                                                        shearchBox="cmpFiltro_txtSearch"
                                                        listNotPredictive="HistoricoCoreDocumentoID,DatosJSON,DocumentoID,UsuarioID,FechaModificacion"
                                                        RemoteSort="true">
                                                        <Proxy>
                                                            <ext:PageProxy />
                                                        </Proxy>
                                                        <Model>
                                                            <ext:Model runat="server"
                                                                IDProperty="HistoricoCoreDocumentoID">
                                                                <Fields>
                                                                    <ext:ModelField Name="HistoricoCoreDocumentoID" Type="Int" />
                                                                    <ext:ModelField Name="FechaModificacion" Type="Date" />
                                                                    <ext:ModelField Name="Documento" Type="String" />
                                                                    <ext:ModelField Name="DatosJson" Type="String" />
                                                                    <ext:ModelField Name="DocumentoID" Type="Int" />
                                                                    <ext:ModelField Name="UsuarioID" Type="Int" />
                                                                    <ext:ModelField Name="NombreCompleto" Type="String" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                        <Sorters>
                                                            <ext:DataSorter Property="FechaModificacion" Direction="DESC" />
                                                        </Sorters>
                                                        <Parameters>
                                                            <ext:StoreParameter Name="Cabecera" Value="FechaModificacion" Mode="Value" Action="read" />
                                                            <ext:StoreParameter Name="Codigo" Value="HistoricoCoreDocumentoID" Mode="Value" Action="read" />
                                                            <ext:StoreParameter Name="Datos" Value="DatosJson" Mode="Value" Action="read" />
                                                        </Parameters>
                                                        <Listeners>
                                                            <Load Fn="cargarEventosCalendario"></Load>
                                                            <DataChanged Fn="BuscadorPredictivo" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>

                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column runat="server"
                                                            ID="colUsuario"
                                                            text="<%$ Resources:Comun, strUsuario %>"
                                                            DataIndex="NombreCompleto"
                                                            MinWidth="120"
                                                            Flex="8">
                                                        </ext:Column>
                                                        <%--<ext:Column runat="server"
                                                            Text="<%$ Resources:Comun, strDocumento %>"
                                                            DataIndex="DatosJson"
                                                            ID="colDocumento" >
                                                            <Renderer Fn="NombreDocumentoRender" />
                                                        </ext:Column>--%>
                                                        <ext:DateColumn runat="server"
                                                            Text="<%$ Resources:Comun, strFecha %>"
                                                            DataIndex="FechaModificacion"
                                                            Format="d/m/Y G:i"
                                                            Hideable="false"
                                                            MinWidth="120"
                                                            Flex="8"
                                                            ID="colFechaModificacion">
                                                        </ext:DateColumn>
                                                    </Columns>
                                                </ColumnModel>

                                                <Plugins>
                                                    <ext:GridFilters runat="server"
                                                        ID="gridFiltersPrincipal"
                                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                                    <ext:CellEditing runat="server"
                                                        ClicksToEdit="2" />
                                                </Plugins>

                                                <BottomBar>
                                                    <ext:PagingToolbar runat="server"
                                                        StoreID="storePrincipal"
                                                        Cls="PgToolBMainGrid"
                                                        ID="PagingToolbarPrincipal"
                                                        MaintainFlex="true"
                                                        Flex="8">
                                                        <Items>
                                                            <ext:ComboBox runat="server"
                                                                Cls="comboGrid"
                                                                Flex="2">
                                                                <Items>
                                                                    <ext:ListItem Text="10 Registros" />
                                                                    <ext:ListItem Text="20 Registros" />
                                                                    <ext:ListItem Text="30 Registros" />
                                                                    <ext:ListItem Text="40 Registros" />
                                                                </Items>
                                                                <SelectedItems>
                                                                    <ext:ListItem Value="20 Registros" />
                                                                </SelectedItems>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:PagingToolbar>
                                                </BottomBar>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server" />
                                                </SelectionModel>

                                            </ext:GridPanel>

                                            <ext:Panel runat="server"
                                                MonitorResize="true"
                                                ForceFit="true"
                                                Hidden="true"
                                                MaxWidth="290"
                                                Flex="12"
                                                Header="false"
                                                Cls="gridPanel grdNoHeader "
                                                ID="pnCalendario"
                                                EnableColumnHide="false"
                                                Region="West">

                                                <Content>
                                                    <div id="evoCalendar" />
                                                </Content>

                                            </ext:Panel>

                                            <ext:Panel runat="server"
                                                ID="pnSecundario"
                                                Cls="pnSecundario grdNoHeader"
                                                Region="Center"
                                                Flex="1"
                                                MinWidth="200"
                                                OverflowX="Auto"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar5"
                                                        Dock="Top"
                                                        Padding="0"
                                                        Cls="tlbGrid c-Grid">
                                                        <Items>
                                                            <ext:Button runat="server" ID="btnCloseShowVisorTreeP" IconCls="ico-hide-menu" Cls="btnSbCategory" Handler="showOnlySecundary();"></ext:Button>
                                                            <ext:ToolbarFill />
                                                            <ext:Button runat="server"
                                                                ID="btnCalendarToGrid"
                                                                Cls="btnLista"
                                                                ToolTip="<%$ Resources:Comun, strVistaCategoria %>">
                                                                <Listeners>
                                                                    <Click Handler="showPrincipalOrCalendar();" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <ext:GridPanel runat="server"
                                                        ID="pnColumnaTraducciones"
                                                        Cls="pnColumnaTraducciones"
                                                        Width="150"
                                                        MarginSpec="0 0 0 16px"
                                                        MinWidth="150">

                                                        <Store>
                                                            <ext:Store ID="storeTraducciones" runat="server"
                                                                AutoLoad="true"
                                                                OnReadData="storeTraducciones_Refresh"
                                                                RemoteSort="true">
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Model>
                                                                    <ext:Model runat="server"
                                                                        IDProperty="Traduccion">
                                                                        <Fields>
                                                                            <ext:ModelField Name="Traduccion" Type="String" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:Store>
                                                        </Store>

                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:Column runat="server"
                                                                    DataIndex="Traduccion"
                                                                    MinWidth="150"
                                                                    ID="colTraduccion" />
                                                            </Columns>
                                                        </ColumnModel>

                                                        <Listeners>
                                                            <AfterRender Fn="crearColumnaEstadoActualDocumentos" />
                                                        </Listeners>
                                                    </ext:GridPanel>

                                                    <div id="historico_contenedor" class="historico_contenedor" />
                                                </Content>
                                            </ext:Panel>

                                        </Items>

                                    </ext:Panel>

                                </Content>

                            </ext:Panel>

                            <%-- PANEL LATERAL DESPLEGABLE--%>

                            <ext:Panel runat="server" ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                Width="380"
                                AnimCollapse="false"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="AnchorLayout"
                                Header="false" Border="false" Hidden="false">
                                <Listeners>
                                    <AfterLayout Handler=""></AfterLayout>
                                    <Resize Handler=""></Resize>
                                </Listeners>
                                <Items>

                                    <%-- PANEL Filtros--%>

                                    <ext:Panel runat="server" ID="WrapFilterControls" Hidden="true" Cls="tbGrey" AnchorVertical="100%" AnchorHorizontal="100%" Layout="AnchorLayout">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="Toolbar2" Cls="tbGrey" Dock="Top" Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label2" runat="server" IconCls="ico-head-info" Cls="lblHeadAside lblHeadAside tbGrey" Text="FILTERS" MarginSpec="36 15 30 15"></ext:Label>
                                                </Items>
                                            </ext:Toolbar>

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
                                                Cls="">
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
                                                                
                                                                                             <div class="GMFdivNombre" title="{NombreFiltro}">{NombreFiltro}
                                                                                             </div> 

                                                                                            <div class="">
                                                                                                <button class="GMFbtnBorrar" type="button" onclick="alert('Hello world!')"> </button>
                                                                                             </div> 

                                                                                            <div class="GMFdivbtn2">
                                                                                                <button class="GMFbtnEditar" type="button" onclick="alert('Hello world!')"></button>
                                                                                             </div> 

                                                                                             <div class="GMFdivbtn3">
                                                                                                <button class="GMFbtnAplicaFiltro" type="button" onclick="alert('Hello world!')"> </button>
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
                                                Cls="">
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

                                                    <ext:Toolbar runat="server" ID="Toolbar4" Dock="Bottom" Padding="0" MarginSpec="20 8 0 0">
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
                                                        FieldLabel="User">
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

                                                </Items>
                                            </ext:Panel>

                                        </Items>
                                    </ext:Panel>

                                    <%-- PANEL GESTION COLUMNAS--%>

                                    <ext:Panel runat="server" ID="WrapGestionColumnas" Hidden="false" Layout="VBoxLayout" AnchorVertical="100%" AnchorHorizontal="100%" OverflowY="Auto">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch" />
                                        </LayoutConfig>

                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tbTitlePanelColumnas" Cls="tbGrey" Dock="Top" Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label6" runat="server" IconCls="ico-head-columns-gr" Cls="lblHeadAside lblHeadAside tbGrey" Text="COLUMN SETTINGS" MarginSpec="36 15 30 15"></ext:Label>

                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>

                                        <Items>

                                            <%--   <ext:Label ID="Label5" runat="server" Cls="lblRecordName" Text="Site Name"></ext:Label>--%>
                                            <ext:ComboBox runat="server" ID="ComboBox4" Cls="comboGrid comboCustomTrigger  " EmptyText="Profiles" LabelAlign="Top" FieldLabel="Profiles" Padding="15">
                                                <Items>
                                                    <ext:ListItem Text="Profile 1" />
                                                    <ext:ListItem Text="Profile 2" />
                                                </Items>
                                                <Triggers>
                                                    <ext:FieldTrigger IconCls="ico-trigger-reload" ExtraCls="none"></ext:FieldTrigger>
                                                </Triggers>
                                            </ext:ComboBox>

                                            <ext:GridPanel
                                                ID="GridColumnas"
                                                Padding="15"
                                                BodyPadding="2"
                                                runat="server"
                                                OverflowY="Auto"
                                                OverflowX="Hidden"
                                                BodyCls=""
                                                Cls=" gridPanel  toolbar-noborder">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server" ID="Toolbar9" Dock="Bottom" Padding="0" MarginSpec="8 -8 0 0">
                                                        <Items>

                                                            <ext:Button runat="server" ID="Button1" Cls="btn-ppal " Text="Save in new Tab" Focusable="false" PressedCls="none" Hidden="false" Flex="1" Handler="showwinAddTab()"></ext:Button>

                                                        </Items>
                                                    </ext:Toolbar>

                                                    <ext:Toolbar runat="server" ID="tbColumnasSaveApply" Dock="Bottom" Padding="0" MarginSpec="8 -8 0 0">
                                                        <Items>

                                                            <ext:Button runat="server" ID="btnApplicar" Cls="btn-ppal " Text="Apply" Focusable="false" PressedCls="none" Hidden="false" Flex="1"></ext:Button>
                                                            <ext:Button runat="server" ID="btnGuardarCols" Cls="btn-ppal " Text="Save" Focusable="false" PressedCls="none" Hidden="false" Flex="1"></ext:Button>

                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>

                                                <Store>
                                                    <ext:Store runat="server">

                                                        <Model>
                                                            <ext:Model runat="server" IDProperty="ID">
                                                                <Fields>
                                                                    <ext:ModelField Name="ID" />
                                                                    <ext:ModelField Name="NombreCategoria" />
                                                                    <ext:ModelField Name="NombreItemTarea" />
                                                                    <ext:ModelField Name="CosteItemUnidad" />
                                                                    <ext:ModelField Name="CosteItemTipoUnidad" />
                                                                    <ext:ModelField Name="NumItems" />
                                                                    <ext:ModelField Name="CosteTotalFila" Type="Int" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>

                                                    </ext:Store>
                                                </Store>
                                                <ColumnModel runat="server">
                                                    <Columns>

                                                        <ext:WidgetColumn ID="WidgetColumn4" runat="server" Cls="col-More" DataIndex="" Align="Center" Hidden="false" MaxWidth="50">
                                                            <Widget>
                                                                <ext:Label runat="server" ID="DragBtn" IconCls="btnMoverFila" Cls=" btn-trans " OverCls="none"></ext:Label>
                                                            </Widget>
                                                        </ext:WidgetColumn>

                                                        <ext:TemplateColumn runat="server" DataIndex="" MenuDisabled="true" Text="" Flex="5">
                                                            <Template runat="server">
                                                                <Html>
                                                                    <tpl for=".">
                                                            <div class="customCol1">
                                                                <div class="LabelColumnGridRow">{NombreCategoria}

                                                                 <%--   <button  name="btnSubmit" id="btnSubmit" runat="server" onserverclick="Submit_Click"/>--%>
                                                                </div> 
                                                             
                                                            </div>
                                                        </tpl>
                                                                </Html>

                                                            </Template>

                                                        </ext:TemplateColumn>

                                                        <ext:WidgetColumn ID="WidgetColumn3" runat="server" Cls="col-More" DataIndex="" Align="Center" Hidden="false" MaxWidth="40">
                                                            <Widget>
                                                                <ext:Button runat="server" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls=" btn-trans btnVisible" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                                            </Widget>
                                                        </ext:WidgetColumn>
                                                    </Columns>
                                                </ColumnModel>

                                                <View>
                                                    <ext:GridView runat="server">
                                                        <Plugins>
                                                            <ext:GridDragDrop runat="server" DragText="Drag To Re-order"></ext:GridDragDrop>
                                                        </Plugins>
                                                    </ext:GridView>
                                                </View>

                                            </ext:GridPanel>
                                        </Items>
                                    </ext:Panel>

                                    <%-- PANEL MORE INFO--%>

                                    <ext:Panel runat="server" ID="pnMoreInfo" Hidden="true" Cls="tbGrey" AnchorVertical="100%" AnchorHorizontal="100%" OverflowY="Auto" Layout="FitLayout">

                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="Toolbar1" Cls="tbGrey" Dock="Top" Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label1" runat="server" IconCls="ico-head-info" Cls="lblHeadAside " Text="MORE INFO" MarginSpec="36 15 30 15"></ext:Label>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>

                                        <Items>
                                            <ext:GridPanel ID="grAsR1" runat="server"
                                                Cls="grdPnColIcons grdIntoAside"
                                                Border="false">

                                                <Store>
                                                    <ext:Store
                                                        ID="Store1"
                                                        runat="server"
                                                        Buffered="true"
                                                        RemoteFilter="true"
                                                        LeadingBufferZone="1000"
                                                        PageSize="50">

                                                        <Model>
                                                            <ext:Model runat="server" IDProperty="Id">
                                                                <Fields>
                                                                    <ext:ModelField Name="Id" />
                                                                    <ext:ModelField Name="Ini" />
                                                                    <ext:ModelField Name="Name" />
                                                                    <ext:ModelField Name="Profile" />
                                                                    <ext:ModelField Name="Company" />
                                                                    <ext:ModelField Name="Email" />
                                                                    <ext:ModelField Name="Project" />
                                                                    <ext:ModelField Name="Authorized" />
                                                                    <ext:ModelField Name="Staff" />
                                                                    <ext:ModelField Name="Support" />
                                                                    <ext:ModelField Name="LDAP" />
                                                                    <ext:ModelField Name="License" />
                                                                    <ext:ModelField Name="KeyExpiration" />
                                                                    <ext:ModelField Name="LastKey" />
                                                                    <ext:ModelField Name="LastAccess" />

                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:TemplateColumn runat="server" DataIndex="" ID="templateColumn2" Text="" Flex="1">
                                                            <Template runat="server">
                                                                <Html>
                                                                    <tpl for=".">
												                        <table class="tmpCol-table">
                                                                    
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span class="dataGrd">{Company}</span></td>
																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span class="dataGrd">{Email}</span></td>

																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="2"><span class="lblGrd">Info Label</span><span class="dataGrd">{Project}</span></td>

																			</tr>
																		
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span class="dataGrd">{License}</span></td>
																				
																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span  class="dataGrd">{KeyExpiration}</span></td>

																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span class="dataGrd">{LastKey}</span></td>

																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td" colspan="3"><span class="lblGrd">Info Label</span><span class="dataGrd">{LastAccess}</span></td>

																			</tr>
																		</table>
                               										</tpl>

                                                                </Html>
                                                            </Template>
                                                        </ext:TemplateColumn>

                                                    </Columns>
                                                </ColumnModel>
                                                <View>
                                                    <ext:GridView runat="server" TrackOver="false" EmptyText="<h1>No matching results</h1>" EnableTextSelection="true">
                                                    </ext:GridView>
                                                </View>
                                            </ext:GridPanel>
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
