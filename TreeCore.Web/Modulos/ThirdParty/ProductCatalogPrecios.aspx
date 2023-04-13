<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductCatalogPrecios.aspx.cs" Inherits="TreeCore.ModGlobal.ProductCatalogPrecios" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body class="bodyFMenu">
    <link href="css/EmplazamientosHistoricos.css" rel="stylesheet" type="text/css" />

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdServicioPrecioID" runat="server" />
            <ext:Hidden ID="hdListaCatalogos" runat="server" />
            <ext:Hidden ID="hdStringBuscadorPrecios" runat="server" />
            <ext:Hidden ID="hdIDPrecioBuscador" runat="server" />
            <ext:Hidden ID="hdTotalCountGridPrecios" runat="server">
                <Listeners>
                    <Change Fn="setNumMaxPagePrecios"></Change>
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdStringBuscadorTraduccion" runat="server" />
            <ext:Hidden ID="hdIDTraduccionBuscador" runat="server" />
            <ext:Hidden ID="hdTotalCountGridTraduccion" runat="server">
                <Listeners>
                    <Change Fn="setNumMaxPageTraduccion"></Change>
                </Listeners>
            </ext:Hidden>

            <%--FIN HIDDEN --%>

            <%-- INICIO STORES --%>

            <ext:Store runat="server"
                ID="storeEntidades"
                AutoLoad="false"
                OnReadData="storeEntidades_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="EntidadID">
                        <Fields>
                            <ext:ModelField Name="EntidadID" Type="Int" />
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

            <ext:Store
                ID="storeCoreProductCatalog"
                runat="server"
                AutoLoad="false"
                OnReadData="storeCoreProductCatalog_Refresh"
                RemoteSort="true"
                RemotePaging="true"
                RemoteFilter="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="CoreProductCatalogID">
                        <Fields>
                            <ext:ModelField Name="CoreProductCatalogID" Type="Int" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="NombreProductCatalog" Type="String" />
                            <ext:ModelField Name="NombreCoreProductCatalogTipo" Type="String" />
                            <ext:ModelField Name="FechaInicioVigencia" Type="Date" />
                            <ext:ModelField Name="FechaFinVigencia" Type="Date" />
                            <ext:ModelField Name="NombreEntidad" Type="String" />
                            <ext:ModelField Name="FechaInicioReajuste" Type="Date"/>
                            <ext:ModelField Name="FechaProximaReajuste" Type="Date"/>
                            <ext:ModelField Name="FechaFinReajuste" Type="Date"/>
                            <ext:ModelField Name="InflacionID" />
                            <ext:ModelField Name="CantidadFija" />
                            <ext:ModelField Name="PorcentajeFijo" />
                            <ext:ModelField Name="Periodicidad" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreEntidad" Direction="ASC" />
                </Sorters>
                <Parameters>
                    <ext:StoreParameter Name="Cabecera" Value="Codigo" Mode="Value" Action="read" />
                    <ext:StoreParameter Name="Nombre" Value="NombreEntidad" Mode="Value" Action="read" />
                    <ext:StoreParameter Name="Codigo" Value="CoreProductCatalogID" Mode="Value" Action="read" />
                </Parameters>
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaPrecio" />
                </Listeners>
            </ext:Store>

            <ext:Store ID="storeTraducciones"
                runat="server"
                AutoLoad="false"
                OnReadData="storeTraducciones_Refresh"
                RemoteSort="true"
                RemoteFilter="true"
                RemotePaging="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Codigo">
                        <Fields>
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Nombre" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <%-- FIN STORES --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <DocumentReady Fn="bindParamsPrecios" />
                </Listeners>
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Modal="true"
                Width="750"
                MaxWidth="750"
                Height="600"
                Hidden="true"
                OverflowY="Auto"
                Layout="FitLayout"
                Cls="winForm-resp">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formPrecios"
                        Hidden="true"
                        OverflowY="Hidden"
                        Cls="winGestion-panel ctForm-resp">
                        <Items>
                            <ext:ComboBox runat="server"
                                ID="cmbEntidad"
                                FieldLabel="<%$ Resources:Comun, strEntidad %>"
                                LabelAlign="Top"
                                DisplayField="Nombre"
                                ValueField="EntidadID"
                                StoreID="storeEntidades"
                                EmptyCls="cmbModulo"
                                FieldBodyCls="cmbModulo"
                                QueryMode="Local"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                <Listeners>
                                    <Select Fn="SeleccionarEntidad" />
                                    <TriggerClick Fn="RecargarEntidad" />
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
                        ID="btnCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGestion}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        Disabled="true"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%-- FIN  WINDOWS --%>

            <ext:Viewport runat="server"
                ID="MainVwP"
                OverflowY="auto"
                Layout="FitLayout">
                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>

                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="tbFiltersLabels"
                                Dock="Top"
                                Cls="tbGrey  tbNoborder tbFiltrosLabels"
                                Hidden="true"
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
                        <Items>

                            <%-------------TENER EN CUENTA QUE AL CENTER MAIN SE LE PUEDEN ACOPLAR SEGMENTOS PLEGABLES CON LAS "REGIONS"-------------%>

                            <%-- PANEL CENTRAL--%>

                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain">
                                <Items>
                                    <ext:Panel
                                        runat="server"
                                        ID="wrapComponenteCentralServicios"
                                        Hidden="false"
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
                                                MaxWidth="300"
                                                Flex="12"
                                                runat="server"
                                                Cls="gridPanel"
                                                Title="<%$ Resources:Comun, strCatalogos %>"
                                                ID="gridPrecios"
                                                EnableColumnHide="false"
                                                SelectionMemory="false"
                                                Scrollable="Vertical"
                                                StoreID="storeCoreProductCatalog"
                                                Region="West"
                                                OverflowY="Auto">
                                                <Listeners>
                                                    <RowClick Fn="agregarColumnaDinamicaPrecio"></RowClick>
                                                </Listeners>
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbBase"
                                                        Dock="Top"
                                                        Cls="tlbGrid"
                                                        OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnRefrescar"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                Cls="btnRefrescar">
                                                                <Listeners>
                                                                    <Click Fn="RefrescarPrecio" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="toolBarFiltro"
                                                        Cls="tlbGrid"
                                                        Layout="ColumnLayout">
                                                        <Items>
                                                            <ext:TextField
                                                                ID="txtSearchPrecios"
                                                                Cls="txtSearchC"
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                LabelWidth="50"
                                                                Width="250"
                                                                EnableKeyEvents="true">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Search" />
                                                                    <ext:FieldTrigger Icon="clear" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <Render Fn="FieldSearch" Buffer="250" />
                                                                    <Change Fn="FiltrarColumnas" Buffer="250" />
                                                                    <KeyPress Fn="filtrarPorBuscadorPrecios" Buffer="500" />
                                                                    <TriggerClick Fn="LimpiarFiltroBusquedaPrecios" />
                                                                    <FocusEnter Fn="ajaxGetDatosBuscadorPrecios" />
                                                                </Listeners>
                                                            </ext:TextField>
                                                            <ext:FieldContainer runat="server"
                                                                ID="ContButtons"
                                                                Cls="FloatR ContButtons">
                                                                <Items>
                                                                    <ext:Button
                                                                        runat="server"
                                                                        Width="30"
                                                                        ID="btnClearFiltersPrecios"
                                                                        Cls="btn-trans btnRemoveFilters"
                                                                        AriaLabel="Quitar Filtros"
                                                                        Hidden="true"
                                                                        ToolTip="<%$ Resources:Comun, strQuitarFiltro %>">
                                                                        <Listeners>
                                                                            <Click Fn="BorrarFiltrosPrecios"></Click>
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:FieldContainer>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column
                                                            ID="colCodigo"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strCodigo %>"
                                                            Flex="1"
                                                            DataIndex="Codigo"
                                                            Align="Start">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colNombre"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strNombre %>"
                                                            Flex="1"
                                                            Hidden="true"
                                                            DataIndex="NombreProductCatalog"
                                                            Align="Start">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colEntidad"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strEntidad %>"
                                                            Flex="1"
                                                            DataIndex="NombreEntidad"
                                                            Align="Start">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colTipo"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strTipo %>"
                                                            Flex="1"
                                                            Hidden="true"
                                                            DataIndex="NombreCoreProductCatalogTipo"
                                                            Align="Start">
                                                        </ext:Column>
                                                        <ext:WidgetColumn ID="ColMorePrecios"
                                                            runat="server"
                                                            Cls="NoOcultar col-More"
                                                            DataIndex=""
                                                            Align="Center"
                                                            Text="<%$ Resources:Comun, strMas %>"
                                                            Hidden="false"
                                                            Flex="1"
                                                            MinWidth="70"
                                                            MaxWidth="70">
                                                            <Widget>
                                                                <ext:Button runat="server"
                                                                    Width="70"
                                                                    OverCls="Over-btnMore"
                                                                    PressedCls="Pressed-none"
                                                                    FocusCls="Focus-none"
                                                                    Cls="btnColMoreX">
                                                                    <Listeners>
                                                                        <Click Handler="parent.hideAsideR('WrapPrices', App.storeCoreProductCatalog, this)" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Widget>
                                                        </ext:WidgetColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel runat="server"
                                                        ID="GridRowSelectPrecio"
                                                        Mode="Multi">
                                                        <Listeners>
                                                            <Deselect Fn="DeseleccionarGrillaPrecio" />
                                                            <Select Fn="Grid_RowSelectPrecio"></Select>
                                                        </Listeners>
                                                    </ext:CheckboxSelectionModel>
                                                </SelectionModel>
                                                <Plugins>
                                                    <ext:GridFilters runat="server"
                                                        ID="gridFiltersPrecio"
                                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                                </Plugins>
                                                <BottomBar>
                                                    <ext:Toolbar ID="paginPrecios"
                                                        runat="server"
                                                        OverflowHandler="Scroller"
                                                        Cls="bottomBarSites">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnPagingInitPrecios"
                                                                Disabled="true"
                                                                Cls="noBorder"
                                                                IconCls="x-tbar-page-first">
                                                                <Listeners>
                                                                    <Click Fn="pagingInitPrecios" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnPagingPrePrecios"
                                                                Disabled="true"
                                                                Cls="noBorder"
                                                                IconCls="x-tbar-page-prev">
                                                                <Listeners>
                                                                    <Click Fn="pagingPrePrecios" />
                                                                </Listeners>
                                                            </ext:Button>

                                                            <ext:ToolbarSeparator />

                                                            <ext:Label
                                                                runat="server"
                                                                Text="<%$ Resources:Comun, jsPagina %>" />
                                                            <ext:NumberField
                                                                runat="server"
                                                                ID="nfPaginNumberPrecios"
                                                                MinValue="0"
                                                                Width="50"
                                                                HideTrigger="true">
                                                                <Listeners>
                                                                    <BeforeRender Fn="nfPaginNumberBeforeRenderPrecios" />
                                                                    <Change Fn="paginGoToPrecios" />
                                                                </Listeners>
                                                            </ext:NumberField>
                                                            <ext:Label
                                                                runat="server"
                                                                Text="of" />
                                                            <ext:Label
                                                                runat="server"
                                                                ID="lbNumberPagesPrecios"
                                                                Text="0" />
                                                            <ext:ToolbarSeparator />

                                                            <ext:Button runat="server"
                                                                ID="btnPaginNextPrecios"
                                                                Enabled="false"
                                                                Cls="noBorder"
                                                                IconCls="x-tbar-page-next">
                                                                <Listeners>
                                                                    <Click Fn="paginNextPrecios" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnPaginLastPrecios"
                                                                Enabled="false"
                                                                Cls="noBorder"
                                                                IconCls="x-tbar-page-last">
                                                                <Listeners>
                                                                    <Click Fn="paginLastPrecios" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator />

                                                            <ext:ComboBox runat="server"
                                                                Cls="comboGrid"
                                                                MinWidth="80"
                                                                MaxWidth="80"
                                                                ID="cmbNumRegistrosPrecios"
                                                                Flex="2">
                                                                <Items>
                                                                    <ext:ListItem Text="10" />
                                                                    <ext:ListItem Text="20" />
                                                                    <ext:ListItem Text="30" />
                                                                    <ext:ListItem Text="40" />
                                                                </Items>
                                                                <SelectedItems>
                                                                    <ext:ListItem Value="20" />
                                                                </SelectedItems>
                                                                <Listeners>
                                                                    <Select Fn="pageSelectPrecios" />
                                                                </Listeners>
                                                            </ext:ComboBox>

                                                            <ext:ToolbarFill />

                                                            <ext:Label runat="server"
                                                                ID="lbDisplayingPrecios"
                                                                Text="<%$ Resources:Comun, strSinDatosMostrar %>" />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </BottomBar>
                                            </ext:GridPanel>
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
                                                        Height="42"
                                                        Padding="0"
                                                        Cls="tlbGrid c-Grid">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnCloseShowVisorTreeP"
                                                                IconCls="ico-hide-menu"
                                                                Cls="btnSbCategory"
                                                                Height="42"
                                                                Handler="showOnlySecundary();">
                                                            </ext:Button>
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar runat="server"
                                                        ID="Toolbar1"
                                                        Dock="Top"
                                                        Cls="tlbGrid"
                                                        OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnRefrescarPrecio"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                Cls="btnRefrescar">
                                                                <Listeners>
                                                                    <Click Fn="RefrescarContenido" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnDescargarPrecio"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Cls="btnDescargar"
                                                                Handler="ExportarDatos('ProductCatalogPrecios', App.hdCliID.value, #{pnColumnaTraducciones}, '/ModGlobal/pages/ProductCatalogPrecios.aspx', '', App.hdListaCatalogos.value, App.hdStringBuscadorTraduccion.value, App.hdIDTraduccionBuscador.value);" />
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="toolBar2"
                                                        Cls="tlbGrid"
                                                        Layout="ColumnLayout">
                                                        <Items>
                                                            <ext:TextField
                                                                ID="txtSearchTraduccion"
                                                                Cls="txtSearchC"
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                LabelWidth="50"
                                                                Width="250"
                                                                EnableKeyEvents="true">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Search" />
                                                                    <ext:FieldTrigger Icon="clear" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <Render Fn="FieldSearch" Buffer="250" />
                                                                    <Change Fn="FiltrarColumnas" Buffer="250" />
                                                                    <KeyPress Fn="filtrarPorBuscadorTraduccion" Buffer="500" />
                                                                    <TriggerClick Fn="LimpiarFiltroBusquedaTraduccion" />
                                                                    <FocusEnter Fn="ajaxGetDatosBuscadorTraduccion" />
                                                                </Listeners>
                                                            </ext:TextField>
                                                            <ext:FieldContainer runat="server"
                                                                ID="ContButtonsTraduccion"
                                                                Cls="FloatR ContButtons">
                                                                <Items>
                                                                    <ext:Button
                                                                        runat="server"
                                                                        Width="30"
                                                                        ID="btnClearFiltersTraduccion"
                                                                        Cls="btn-trans btnRemoveFilters"
                                                                        AriaLabel="Quitar Filtros"
                                                                        Hidden="true"
                                                                        ToolTip="<%$ Resources:Comun, strQuitarFiltro %>">
                                                                        <Listeners>
                                                                            <Click Fn="BorrarFiltrosTraduccion"></Click>
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:FieldContainer>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Content>
                                                    <ext:GridPanel runat="server"
                                                        ID="pnColumnaTraducciones"
                                                        Cls="pnColumnaTraducciones pnColumnaTradPrices"
                                                        StoreID="storeTraducciones"
                                                        Width="300"
                                                        MarginSpec="0 0 0 10px"
                                                        MinWidth="300">
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:Column
                                                                    ID="colNombreServicio"
                                                                    runat="server"
                                                                    MinWidth="150"
                                                                    ToolTip="<%$ Resources:Comun, strServicio %>"
                                                                    DataIndex="Nombre"
                                                                    Align="Start">
                                                                </ext:Column>
                                                                <ext:Column
                                                                    ID="colCodigoServicio"
                                                                    runat="server"
                                                                    MinWidth="150"
                                                                    ToolTip="<%$ Resources:Comun, strCodigo %>"
                                                                    DataIndex="Codigo"
                                                                    Align="Start">
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <BottomBar>
                                                            <ext:Toolbar ID="paginPTraduccion"
                                                                runat="server"
                                                                OverflowHandler="Scroller"
                                                                Cls="bottomBarSites">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        ID="btnPagingInitTraduccion"
                                                                        Disabled="true"
                                                                        Cls="noBorder"
                                                                        IconCls="x-tbar-page-first">
                                                                        <Listeners>
                                                                            <Click Fn="pagingInitTraduccion" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnPagingPreTraduccion"
                                                                        Disabled="true"
                                                                        Cls="noBorder"
                                                                        IconCls="x-tbar-page-prev">
                                                                        <Listeners>
                                                                            <Click Fn="pagingPreTraduccion" />
                                                                        </Listeners>
                                                                    </ext:Button>

                                                                    <ext:ToolbarSeparator />

                                                                    <ext:Label
                                                                        runat="server"
                                                                        Text="<%$ Resources:Comun, jsPagina %>" />
                                                                    <ext:NumberField
                                                                        runat="server"
                                                                        ID="nfPaginNumberTraduccion"
                                                                        MinValue="0"
                                                                        Width="50"
                                                                        HideTrigger="true">
                                                                        <Listeners>
                                                                            <BeforeRender Fn="nfPaginNumberBeforeRenderTraduccion" />
                                                                            <Change Fn="paginGoToTraduccion" />
                                                                        </Listeners>
                                                                    </ext:NumberField>
                                                                    <ext:Label
                                                                        runat="server"
                                                                        Text="of" />
                                                                    <ext:Label
                                                                        runat="server"
                                                                        ID="lbNumberPagesTraduccion"
                                                                        Text="0" />
                                                                    <ext:ToolbarSeparator />

                                                                    <ext:Button runat="server"
                                                                        ID="btnPaginNextTraduccion"
                                                                        Enabled="false"
                                                                        Cls="noBorder"
                                                                        IconCls="x-tbar-page-next">
                                                                        <Listeners>
                                                                            <Click Fn="paginNextTraduccion" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnPaginLastTraduccion"
                                                                        Enabled="false"
                                                                        Cls="noBorder"
                                                                        IconCls="x-tbar-page-last">
                                                                        <Listeners>
                                                                            <Click Fn="paginLastTraduccion" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:ToolbarSeparator />

                                                                    <ext:ComboBox runat="server"
                                                                        Cls="comboGrid"
                                                                        MinWidth="80"
                                                                        MaxWidth="80"
                                                                        ID="cmbNumRegistrosTraduccion"
                                                                        Flex="2">
                                                                        <Items>
                                                                            <ext:ListItem Text="10" />
                                                                            <ext:ListItem Text="20" />
                                                                            <ext:ListItem Text="30" />
                                                                            <ext:ListItem Text="40" />
                                                                        </Items>
                                                                        <SelectedItems>
                                                                            <ext:ListItem Value="20" />
                                                                        </SelectedItems>
                                                                        <Listeners>
                                                                            <Select Fn="pageSelectTraduccion" />
                                                                        </Listeners>
                                                                    </ext:ComboBox>

                                                                    <ext:ToolbarFill />

                                                                    <ext:Label runat="server"
                                                                        ID="lbDisplayingTraduccion"
                                                                        Text="<%$ Resources:Comun, strSinDatosMostrar %>" />
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </BottomBar>
                                                    </ext:GridPanel>
                                                    <ext:ToolTip runat="server"
                                                        Target="={#{pnColumnaTraducciones}.getView().el}"
                                                        Delegate=".x-grid-cell"
                                                        TrackMouse="true">
                                                        <Listeners>
                                                            <Show Handler="onShow(this, #{pnColumnaTraducciones});" />
                                                        </Listeners>
                                                    </ext:ToolTip>
                                                    <div id="historico_contenedor" class="historico_contenedor" />
                                                </Content>
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
