<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplazamientosGridInventario.aspx.cs" Inherits="TreeCore.ModGlobal.pages.EmplazamientosGridInventario" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
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

            <ext:Hidden ID="hdStringBuscador" runat="server" />
            <ext:Hidden ID="hdIDEmplazamientoBuscador" runat="server" />

            <ext:Hidden ID="hdCliID" runat="server" Name="hdCliIDComponente" />
            <ext:Hidden ID="hdIDComponente" runat="server" />
            <ext:Hidden ID="CurrentControl" runat="server" />
            <ext:Hidden ID="hdControlURL" runat="server" />
            <ext:Hidden ID="hdControlName" runat="server" />
            <ext:Hidden ID="hdFiltrosAplicados" runat="server" />
            <ext:Hidden ID="hdTotalCountGrid" runat="server">
                <Listeners>
                    <Change Fn="setNumMaxPageInventario"></Change>
                </Listeners>
            </ext:Hidden>
            
            <ext:Hidden ID="hdidsResultados" runat="server" />
            <ext:Hidden ID="hdnameIndiceID" runat="server" />
            <ext:Hidden ID="hdResultadoKPIid" runat="server" />

            <%--VIEWPORT--%>
            <ext:Viewport
                runat="server"
                ID="vwContenedor"
                Cls=""
                Layout="Anchor">
                <Items>

                    <ext:GridPanel
                        runat="server"
                        ID="grdInventarioElementosEmplazamientos"
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
                            <ext:Store runat="server"
                                ID="storeInventarioElementosEmplazamientos"
                                AutoLoad="false"
                                OnReadData="storeInventarioElementosEmplazamientos_Refresh"
                                RemotePaging="true"
                                RemoteSort="true"
                                RemoteFilter="true"
                                PageSize="20">
                                <Listeners>
                                    <BeforeLoad Fn="DeseleccionarGrillaInventarioElementos" />
                                </Listeners>
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Model>
                                    <ext:Model runat="server" IDProperty="InventarioElementoID">
                                        <Fields>
                                            <ext:ModelField Name="InventarioElementoID" Type="Int" />
                                            <ext:ModelField Name="Codigo" Type="String" />
                                            <ext:ModelField Name="NombreEmplazamiento" Type="String" />
                                            <ext:ModelField Name="NumeroInventario" Type="String" />
                                            <ext:ModelField Name="NombreElemento" Type="String" />
                                            <ext:ModelField Name="Categoria" Type="String" />
                                            <ext:ModelField Name="EstadoInventarioElemento" Type="String" />
                                            <ext:ModelField Name="Operador" Type="String" />
                                            <ext:ModelField Name="FechaCreaccionElemento" Type="Date" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Sorters>
                                    <ext:DataSorter
                                        Property="InventarioElementoID"
                                        Direction="DESC" />
                                </Sorters>
                                <%--<Listeners>
                                    <DataChanged Fn="ajaxGetDatosBuscadorInventario" />
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
                                            <Click Fn="refrescarInventarioElementos" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        Cls="btnDescargar"
                                        AriaLabel="Descargar"
                                        Hidden="false"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        Handler="ExportarDatos('EmplazamientosGridInventario', App.hdCliID.value, #{grdInventarioElementosEmplazamientos}, '/ModGlobal/pages/EmplazamientosGridInventario.aspx', '',App.hdFiltrosAplicados.value, App.hdStringBuscador.value, App.hdIDEmplazamientoBuscador.value);">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnFiltros"
                                        Cls="btnFiltros"
                                        EnableToggle="true"
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
                                            <TriggerClick Fn="LimpiarFiltroBusquedaInventarioElementos" />
                                            <FocusEnter Fn="ajaxGetDatosBuscadorInventario" />
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
                                                    <Click Fn="BorrarFiltrosInventarioElementos"></Click>
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
                                <ext:Column runat="server"
                                    ID="colCodigoEmplazamiento"
                                    DataIndex="Codigo"
                                    Filterable = "false"
                                    Align="Center"
                                    Text="<%$ Resources:Comun, strCodigoEmplazamiento %>"
                                    MinWidth="140"
                                    Flex="1">
                                </ext:Column>
                                <ext:Column
                                    ID="colNombreEmplazamiento"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strNombre %>"
                                    DataIndex="NombreEmplazamiento"
                                    Filterable = "false"
                                    MinWidth="140"
                                    Flex="1" />
                                <ext:Column
                                    meta:resourceKey="strCodigo"
                                    ID="colCodigoElemento"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strCodigoInventario %>"
                                    DataIndex="NumeroInventario"
                                    Filterable = "false"
                                    MinWidth="140"
                                    Flex="1" />
                                <ext:Column
                                    ID="colNombreElemento"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strNombreInventario %>"
                                    DataIndex="NombreElemento"
                                    Filterable = "false"
                                    MinWidth="140"
                                    Flex="1" />
                                <ext:Column
                                    ID="colCategoria"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strCategoria %>"
                                    DataIndex="Categoria"
                                    Filterable = "false"
                                    MinWidth="140"
                                    Flex="1" />
                                <ext:Column
                                    runat="server"
                                    ID="colEstadoInventario"
                                    Text="<%$ Resources:Comun, strEstadoInventario %>"
                                    DataIndex="EstadoInventarioElemento"
                                    Filterable = "false"
                                    MinWidth="140"
                                    Flex="1" />
                                <ext:Column runat="server"
                                    ID="colOperador"
                                    DataIndex="Operador"
                                    Filterable = "false"
                                    Text="<%$ Resources:Comun, strOperadorInventario %>"
                                    MinWidth="140"
                                    Flex="1">
                                </ext:Column>
                                <ext:DateColumn
                                    runat="server"
                                    Format="<%$ Resources:Comun, FormatFecha %>"
                                    ID="colFechaCreaccion"
                                    Text="<%$ Resources:Comun, strFechaCreacion %>"
                                    DataIndex="FechaCreaccionElemento"
                                    Filterable = "false"
                                    MinWidth="140"
                                    Flex="1" />
                                <ext:WidgetColumn ID="ColMore" runat="server" Cls="col-More" DataIndex="" Align="Center" Text="More" Hidden="false" MinWidth="90" Filterable = "false">
                                    <Widget>
                                        <ext:Button runat="server" Width="90" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColMore">
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
                                ID="GridRowSelectInventarioElementos"
                                Mode="Single">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectInventarioElementos" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <ViewConfig StripeRows="true" />
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersInventarioElementos"
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
                                            <Click Fn="pagingInitInventario" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnPagingPre"
                                        Disabled="true"
                                        Cls="noBorder"
                                        IconCls="x-tbar-page-prev">
                                        <Listeners>
                                            <Click Fn="pagingPreInventario" />
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
                                            <BeforeRender Fn="nfPaginNumberBeforeRenderInventario" />
                                            <Change Fn="paginGoToInventario" />
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
                                            <Click Fn="paginNextInventario" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnPaginLast"
                                        Enabled="false"
                                        Cls="noBorder"
                                        IconCls="x-tbar-page-last">
                                        <Listeners>
                                            <Click Fn="paginLastInventario" />
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
                                            <Select Fn="pageSelectInventario" />
                                        </Listeners>
                                    </ext:ComboBox>

                                    <ext:ToolbarFill />

                                    <ext:Label runat="server"
                                        ID="lbDisplaying"
                                        Text="<%$ Resources:Comun, strSinDatosMostrar %>" />
                                </Items>

                            </ext:Toolbar>
                        </BottomBar>
                        <Listeners>
<%--                            <BeforeRender Fn="CargarGridInventario"></BeforeRender>--%>
                        </Listeners>
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
