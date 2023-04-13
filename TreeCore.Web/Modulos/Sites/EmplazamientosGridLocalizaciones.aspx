<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplazamientosGridLocalizaciones.aspx.cs" Inherits="TreeCore.ModGlobal.pages.EmplazamientosGridLocalizaciones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    
    <title></title>
</head>
<body>
    <script src="//maps.googleapis.com/maps/api/js?key=AIzaSyA0k02bRAAmDy-4x0kcN06k1auhLHKGRZs&v=3&libraries=places" type="text/javascript"></script>
    
    <form id="form1" runat="server">
        <div>

            <%--RESOURCE MANAGER--%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" DisableViewState="true" ShowWarningOnAjaxFailure="false">
                <Listeners>
                    <AjaxRequestException Fn="winErrorTimeout" />
                    <DocumentReady Fn="bindParams" />
                </Listeners>
            </ext:ResourceManager>

            <%--HIDDEN VARS--%>
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdStringBuscador" runat="server" />
            <ext:Hidden ID="hdIDEmplazamientoBuscador" runat="server" />
            <ext:Hidden ID="hdFiltrosAplicados" runat="server" />
            <ext:Hidden ID="hdEmplazamientoSeleccionado" Name="hdEmplazamientoSeleccionado" runat="server" />
            <ext:Hidden ID="hdTotalCountGrid" runat="server">
                <Listeners>
                    <Change Fn="setNumMaxPageLocalizaciones"></Change>
                </Listeners>
            </ext:Hidden>
            
            <ext:Hidden ID="hdZoom" runat="server">
                <Listeners>
                    <Render Fn="OcultarContenedorPadre" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdLatitudForm" runat="server">
                <Listeners>
                    <Render Fn="OcultarContenedorPadre" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdLongitudForm" runat="server">
                <Listeners>
                    <Render Fn="OcultarContenedorPadre" />
                </Listeners>
            </ext:Hidden>
            
            <ext:Hidden ID="hdidsResultados" runat="server" />
            <ext:Hidden ID="hdnameIndiceID" runat="server" />
            <ext:Hidden ID="hdResultadoKPIid" runat="server" />


            <%-- windows --%>

            <ext:Window runat="server"
                Hidden="true"
                ID="contPosicion"
                Height="400"
                Width="600">
                <Content>
                    <div id="MapaTabLocation" class="dvMap" style="width:100%; height:100%;" />
                </Content>
            </ext:Window>

            <%--VIEWPORT--%>
            <ext:Viewport
                runat="server"
                ID="vwContenedor"
                Cls=""
                Layout="Anchor">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="gridEmplazamientosLocalizaciones"
                        EnableColumnHide="false"
                        EnableColumnMove="false"
                        EnableColumnResize="false"
                        Scrollable="Vertical"
                        Cls="gridPanel"
                        AnchorVertical="100%"
                        AnchorHorizontal="100%">
                        <Listeners>
                            <AfterRender Handler="GridColHandlerDinamicoV2(this)"></AfterRender>
                            <Resize Handler="GridColHandlerDinamicoV2(this)"></Resize>
                        </Listeners>
                        <Store>
                            <ext:Store
                                runat="server"
                                ID="storeEmplazamientosLocalizaciones"
                                AutoLoad="true"
                                OnReadData="storeEmplazamientosLocalizaciones_Refresh"
                                RemotePaging="true"
                                RemoteSort="true"
                                RemoteFilter="true"
                                PageSize="20">
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Model>
                                    <ext:Model runat="server" IDProperty="EmplazamientoID">
                                        <Fields>
                                            <ext:ModelField Name="EmplazamientoID" Type="Int" />
                                            <ext:ModelField Name="Codigo" Type="String" />
                                            <ext:ModelField Name="NombreSitio" Type="String" />
                                            <ext:ModelField Name="CodigoPostal" Type="String" />
                                            <ext:ModelField Name="Region" Type="String" />
                                            <ext:ModelField Name="Pais" Type="String" />
                                            <ext:ModelField Name="Provincia" Type="String" />
                                            <ext:ModelField Name="Municipio" Type="String" />
                                            <ext:ModelField Name="Direccion" Type="String" />
                                            <ext:ModelField Name="Longitud" Type="Float" />
                                            <ext:ModelField Name="Latitud" Type="Float" />
                                            <ext:ModelField Name="RegionPais" Type="String" />
                                            <ext:ModelField Name="Geoposicion" Type="String" />
                                            <ext:ModelField Name="Barrio" Type="String" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Sorters>
                                    <ext:DataSorter
                                        Property="EmplazamientoID"
                                        Direction="DESC" />
                                </Sorters>
                                <%--<Listeners>
                                    <DataChanged Fn="ajaxGetDatosBuscadorLocalizaciones" />
                                </Listeners>--%>
                            </ext:Store>
                        </Store>
                        <DockedItems>
                            <ext:Toolbar
                                runat="server"
                                ID="tlbBase"
                                Dock="Top"
                                Cls="tlbGrid c-Grid"
                                Layout="ColumnLayout">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnRefrescar"
                                        Cls="btnRefrescar"
                                        AriaLabel="Actualizar"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>">
                                        <Listeners>
                                            <Click Fn="refrescarLocalizaciones" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        Cls="btnDescargar"
                                        AriaLabel="Descargar"
                                        Hidden="false"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        Handler="ExportarDatos('EmplazamientosGridLocalizaciones',App.hdCliID.value, #{gridEmplazamientosLocalizaciones}, '/ModGlobal/pages/EmplazamientosGridLocalizaciones.aspx', '',App.hdFiltrosAplicados.value, App.hdStringBuscador.value, App.hdIDEmplazamientoBuscador.value);">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnFiltros"
                                        Cls="btnFiltros"
                                        ToolTip="<%$ Resources:Comun, strMostrarPanelFiltros %>"
                                        Handler="parent.MostrarPnFiltros();" />
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar
                                runat="server"
                                ID="tbFiltros"
                                Cls="tlbGrid"
                                Layout="ColumnLayout">
                                <Items>

                                    <ext:TextField
                                        ID="txtSearch"
                                        Cls="txtSearchC"
                                        runat="server"
                                        EmptyText="<%$ Resources:Comun, strBuscar %>"
                                        LabelWidth="50"
                                        Width="250"
                                        EnableKeyEvents="true">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" />
                                        </Triggers>
                                        <Listeners>
                                            <KeyPress Fn="filtrarEmplazamientosPorBuscador" Buffer="500" />
                                            <TriggerClick Fn="LimpiarFiltroBusquedaLocalizaciones" />
                                            <FocusEnter Fn="ajaxGetDatosBuscadorLocalizaciones" />
                                        </Listeners>
                                    </ext:TextField>

                                    <ext:FieldContainer
                                        runat="server"
                                        ID="FCCombos"
                                        Cls="FloatR FCCombos"
                                        Layout="HBoxLayout">
                                        <Defaults>
                                            <ext:Parameter
                                                Name="margin"
                                                Value="0 5 0 0"
                                                Mode="Value" />
                                        </Defaults>
                                        <LayoutConfig>
                                            <ext:HBoxLayoutConfig Align="Middle" />
                                        </LayoutConfig>
                                        <Items>
                                            <ext:ComboBox
                                                runat="server"
                                                ID="cmbProyectos"
                                                Cls="comboGrid  "
                                                EmptyText="Projects" Flex="1">
                                                <Items>
                                                </Items>
                                            </ext:ComboBox>

                                            <ext:ComboBox
                                                runat="server"
                                                ID="cmbEmplazamientos"
                                                Cls="comboGrid  "
                                                EmptyText="Tipologías" Flex="1"
                                                Hidden="true">
                                                <Items>
                                                    <ext:ListItem Text="Tipología 1" />
                                                    <ext:ListItem Text="Tipología 2" />
                                                    <ext:ListItem Text="Tipología 3" />
                                                    <ext:ListItem Text="Tipología 4" />
                                                </Items>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:FieldContainer>

                                    <ext:FieldContainer runat="server" ID="ContButtons" Cls="FloatR ContButtons">
                                        <Items>
                                            <ext:Button runat="server" Width="30" ID="Button1" Cls="btn-trans btnColumnas" AriaLabel="Show Column" ToolTip="Show Column" Hidden="true"></ext:Button>
                                            <ext:Button runat="server" Width="30" ID="btnClearFilters" Cls="btn-trans btnRemoveFilters" AriaLabel="Quitar Filtros" ToolTip="<%$ Resources:Comun, strQuitarFiltro %>">
                                                <Listeners>
                                                    <Click Fn="BorrarFiltrosLozalizaciones"></Click>
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server" Width="30" ID="Button3" Cls="btn-trans btnFiltroNegativo" AriaLabel="Negative Filter" ToolTip="Negative Filter" Handler="ShowWorkFlow();" Hidden="true"></ext:Button>
                                        </Items>
                                    </ext:FieldContainer>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column
                                    meta:resourceKey="strCodigo"
                                    ID="colCodigo"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strCodigo %>"
                                    DataIndex="Codigo"
                                    Filterable = "false"
                                    Flex="1"
                                    MinWidth="120"
                                    MaxWidth="250" />
                                <ext:Column
                                    meta:resourceKey="strNombreSitio"
                                    ID="colNombreSitio"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strNombreSitio %>"
                                    DataIndex="NombreSitio"
                                    Filterable = "false"
                                    Flex="1"
                                    MinWidth="120"
                                    MaxWidth="250" />
                                <ext:Column runat="server"
                                    ID="colMap"
                                    Text="<%$ Resources:Comun, strLocalizacion %>"
                                    DataIndex="Geoposicion"
                                    Filterable = "false"
                                    MinWidth="150"
                                    MaxWidth="250"
                                    Flex="1">
                                    <Commands>
                                        <ext:ImageCommand CommandName="<%$ Resources:Comun, strMapa %>"
                                            IconCls="ico-geolocalizacion-gr">
                                        </ext:ImageCommand>
                                    </Commands>
                                    <Listeners>
                                        <Command Fn="showMap" />
                                    </Listeners>
                                </ext:Column>
                                <ext:Column
                                    ID="colDireccion"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strDireccion %>"
                                    DataIndex="Direccion"
                                    Filterable = "false"
                                    MinWidth="120"
                                    MaxWidth="250"
                                    Flex="1" />
                                <ext:Column
                                    ID="colCodigoPostal"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strCodigoPostal %>"
                                    DataIndex="CodigoPostal"
                                    Filterable = "false"
                                    MinWidth="90"
                                    MaxWidth="250"
                                    Flex="1" />
                                <ext:Column
                                    ID="colMunicipio"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strMunicipio %>"
                                    DataIndex="Municipio"
                                    Filterable = "false"
                                    MinWidth="100"
                                    MaxWidth="250"
                                    Flex="1" />
                                <ext:Column
                                    ID="colBarrio"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strBarrio %>"
                                    DataIndex="Barrio"
                                    Filterable = "false"
                                    MinWidth="100"
                                    MaxWidth="250"
                                    Flex="1" />
                                <ext:Column
                                    ID="colProvincia"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strProvincia %>"
                                    DataIndex="Provincia"
                                    Filterable = "false"
                                    MinWidth="100"
                                    MaxWidth="250"
                                    Flex="1" />
                                <ext:Column
                                    ID="colRegionPais"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strRegionPais %>"
                                    DataIndex="RegionPais"
                                    Filterable = "false"
                                    MinWidth="100"
                                    MaxWidth="250"
                                    Flex="1" />
                                <ext:Column
                                    ID="colPais"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strPais %>"
                                    DataIndex="Pais"
                                    Filterable = "false"
                                    MinWidth="100"
                                    MaxWidth="250"
                                    Flex="1" />
                                <ext:Column
                                    ID="colRegion"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strRegion %>"
                                    DataIndex="Region"
                                    Filterable = "false"
                                    MinWidth="90"
                                    MaxWidth="250"
                                    Flex="1" />
                                <ext:WidgetColumn ID="ColMore" runat="server" Cls="col-More" DataIndex="" Align="Center" Text="More" Hidden="false" MinWidth="90">
                                    <Widget>
                                        <ext:Button meta:resourcekey="btnColMore" runat="server" ID="btnColMore" Width="60" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColMore">
                                            <Listeners>
                                                <Click Fn="MostrarPanelMoreInfo" />
                                            </Listeners>
                                        </ext:Button>
                                    </Widget>
                                </ext:WidgetColumn>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel
                                runat="server"
                                ID="GridRowSelectLocalizaciones"
                                Mode="Multi">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectLocalizaciones" />
                                    <%--<Deselect Fn="DeseleccionarGrilla" />--%>
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <ViewConfig StripeRows="true" />
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFilters"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>">
                            </ext:GridFilters>
                        </Plugins>
                        <BottomBar>

                            <ext:Toolbar ID="pagin" runat="server" OverflowHandler="Scroller" Cls="bottomBarSites">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnPagingInit"
                                        Disabled="true"
                                        Cls="noBorder"
                                        IconCls="x-tbar-page-first">
                                        <Listeners>
                                            <Click Fn="pagingInitLocalizaciones" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnPagingPre"
                                        Disabled="true"
                                        Cls="noBorder"
                                        IconCls="x-tbar-page-prev">
                                        <Listeners>
                                            <Click Fn="pagingPreLocalizaciones" />
                                        </Listeners>
                                    </ext:Button>

                                    <ext:ToolbarSeparator />

                                    <ext:Label
                                        runat="server"
                                        Text="<%$ Resources:Comun, jsPagina %>" />
                                    <ext:NumberField
                                        runat="server"
                                        ID="nfPaginNumber"
                                        MinValue="0"
                                        Width="50"
                                        HideTrigger="true">
                                        <Listeners>
                                            <BeforeRender Fn="nfPaginNumberBeforeRenderLocalizaciones" />
                                            <Change Fn="paginGoToLocalizaciones" />
                                        </Listeners>
                                    </ext:NumberField>
                                    <ext:Label
                                        runat="server"
                                        Text="of" />
                                    <ext:Label
                                        runat="server"
                                        ID="lbNumberPages"
                                        Text="0" />

                                    <ext:ToolbarSeparator />

                                    <ext:Button runat="server"
                                        ID="btnPaginNext"
                                        Enabled="false"
                                        Cls="noBorder"
                                        IconCls="x-tbar-page-next">
                                        <Listeners>
                                            <Click Fn="paginNextLocalizaciones" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnPaginLast"
                                        Enabled="false"
                                        Cls="noBorder"
                                        IconCls="x-tbar-page-last">
                                        <Listeners>
                                            <Click Fn="paginLastLocalizaciones" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />

                                    <ext:ComboBox runat="server"
                                        Cls="comboGrid"
                                        MinWidth="80"
                                        MaxWidth="80"
                                        ID="cmbNumRegistros"
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
                                            <Select Fn="pageSelectLocalizaciones" />
                                        </Listeners>
                                    </ext:ComboBox>

                                    <ext:ToolbarFill />

                                    <ext:Label runat="server"
                                        ID="lbDisplaying"
                                        Text="<%$ Resources:Comun, strSinDatosMostrar %>" />
                                </Items>

                            </ext:Toolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:Viewport>

        </div>
    </form>
    <script type="text/javascript">
        Ext.Ajax.timeout = 120000;
        Ext.net.DirectEvent.timeout = 120000;
    </script>
</body>
</html>
