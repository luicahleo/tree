<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplazamientosGridContactos.aspx.cs" Inherits="TreeCore.ModGlobal.pages.EmplazamientosGridContactos" %>

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
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdStringBuscador" runat="server" />
            <ext:Hidden ID="hdIDEmplazamientoBuscador" runat="server" />
            <ext:Hidden ID="hdFiltrosAplicados" runat="server" />
            <ext:Hidden ID="hdEmplazamientoSeleccionado" Name="hdEmplazamientoSeleccionado" runat="server" />
            <ext:Hidden ID="hdTotalCountGrid" runat="server">
                <Listeners>
                    <Change Fn="setNumMaxPageContactos"></Change>
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
                        ID="grdContactos"
                        AnchorVertical="99%"
                        AnchorHorizontal="100%"
                        Scrollable="Vertical"
                        SelectionMemory="false"
                        EnableColumnHide="false"
                        Cls="gridPanel grdNoHeader">
                        <Listeners>
                            <%--<AfterRender Fn="recargarStore" />--%>
                            <AfterRender Handler="GridColHandlerDinamicoV2(this)"></AfterRender>
                            <Resize Handler="GridColHandlerDinamicoV2(this)"></Resize>
                        </Listeners>
                        <Store>
                            <ext:Store runat="server"
                                ID="storeContactosGlobalesEmplazamientos"
                                AutoLoad="false"
                                OnReadData="storeContactosGlobalesEmplazamientos_Refresh"
                                RemotePaging="true"
                                RemoteSort="true"
                                RemoteFilter="true"
                                PageSize="20">
                                
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Model>
                                    <ext:Model runat="server" IDProperty="ContactoEmplazamientoID">
                                        <Fields>
                                            <ext:ModelField Name="EmplazamientoID" Type="Int" />
                                            <ext:ModelField Name="EntidadID" Type="Int" />
                                            <ext:ModelField Name="ContactoGlobalID" Type="Int" />
                                            <ext:ModelField Name="Nombre" Type="String" />
                                            <ext:ModelField Name="NombreEntidad" Type="String" />
                                            <ext:ModelField Name="NombreSitio" Type="String" />
                                            <ext:ModelField Name="Apellidos" Type="String" />
                                            <ext:ModelField Name="Telefono" Type="String" />
                                            <ext:ModelField Name="Telefono2" Type="String" />
                                            <ext:ModelField Name="Email" Type="String" />
                                            <ext:ModelField Name="Direccion" Type="String" />
                                            <ext:ModelField Name="CP" Type="String" />
                                            <ext:ModelField Name="MunicipioID" Type="Int" />
                                            <ext:ModelField Name="Pais" Type="String" />
                                            <ext:ModelField Name="Region" Type="String" />
                                            <ext:ModelField Name="Codigo" Type="String" />
                                            <ext:ModelField Name="CodigoEntidad" Type="String" />
                                            <ext:ModelField Name="Activo" Type="Boolean" />
                                            <ext:ModelField Name="Provincia" Type="String" />
                                            <ext:ModelField Name="Comentarios" Type="String" />
                                            <ext:ModelField Name="ContactoTipo" Type="String" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Sorters>
                                    <ext:DataSorter
                                        Property="ContactoGlobalID"
                                        Direction="DESC" />
                                </Sorters>
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
                                            <Click Fn="refrescarContactos" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        Cls="btnDescargar"
                                        AriaLabel="Descargar"
                                        Hidden="false"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        Handler="ExportarDatos('EmplazamientosGridContactos',App.hdCliID.value, #{grdContactos}, '/ModGlobal/pages/EmplazamientosGridContactos.aspx', '',App.hdFiltrosAplicados.value, App.hdStringBuscador.value, App.hdIDEmplazamientoBuscador.value);">
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
                                            <TriggerClick Fn="LimpiarFiltroBusquedaContactos" />
                                            <FocusEnter Fn="ajaxGetDatosBuscadorContactos" />
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
                                <ext:Column runat="server"
                                    ID="colActivo"
                                    DataIndex="Activo"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                    Cls="col-activo"
                                    Flex="1"
                                    MinWidth="120">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    meta:resourceKey="strCodigo"
                                    ID="colCodigo"
                                    runat="server"
                                    Flex="1"
                                    Text="<%$ Resources:Comun, strCodigo %>"
                                    DataIndex="Codigo"
                                    Filterable = "false"
                                    MinWidth="120" />
                                <ext:Column
                                    meta:resourceKey="strNombreSitio"
                                    ID="colNombreSitio"
                                    runat="server"
                                    Flex="1"
                                    Text="<%$ Resources:Comun, strNombreSitio %>"
                                    DataIndex="NombreSitio"
                                    Filterable = "false"
                                    MinWidth="120" />
                                <ext:Column
                                    runat="server"
                                    ID="colNombre"
                                    Text="<%$ Resources:Comun, strNombre %>"
                                    DataIndex="Nombre"
                                    Filterable = "false"
                                    MinWidth="120"
                                    Flex="1" />
                                <ext:Column runat="server"
                                    ID="colApellidos"
                                    DataIndex="Apellidos"
                                    Filterable = "false"
                                    Text="<%$ Resources:Comun, strApellidos %>"
                                    MinWidth="120"
                                    Flex="1">
                                </ext:Column>
                                <ext:Column
                                    runat="server"
                                    ID="colTelefono"
                                    MinWidth="120"
                                    Text="<%$ Resources:Comun, strTelefono %>"
                                    DataIndex="Telefono"
                                    Filterable = "false"
                                    Flex="1" />
                                <ext:Column
                                    ID="colEmail"
                                    runat="server"
                                    MinWidth="120"
                                    Text="<%$ Resources:Comun, strEmail %>"
                                    DataIndex="Email"
                                    Filterable = "false"
                                    Flex="1" />
                                <ext:Column
                                    ID="colProvincia"
                                    runat="server"
                                    MinWidth="120"
                                    Text="<%$ Resources:Comun, strProvincia %>"
                                    DataIndex="Provincia"
                                    Filterable = "false"
                                    Flex="1" />
                                <ext:Column
                                    ID="colPais"
                                    runat="server"
                                    MinWidth="120"
                                    Text="<%$ Resources:Comun, strPais %>"
                                    DataIndex="Pais"
                                    Filterable = "false"
                                    Flex="1" />
                                <ext:Column
                                    ID="colTipoContacto"
                                    runat="server"
                                    MinWidth="120"
                                    Text="<%$ Resources:Comun, strTipo %>"
                                    DataIndex="ContactoTipo"
                                    Filterable = "false"
                                    Flex="1" />
                                <ext:WidgetColumn ID="ColMore"
                                    runat="server"
                                    Cls="col-More"
                                    DataIndex=""
                                    Align="Center"
                                    Filterable = "false"
                                    Text="More"
                                    Hidden="false"
                                    MinWidth="90">
                                    <Widget>
                                        <ext:Button ID="btnMore"
                                            runat="server"
                                            Width="90"
                                            OverCls="Over-btnMore"
                                            PressedCls="Pressed-none"
                                            FocusCls="Focus-none"
                                            Cls="btnColMore">
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
                                ID="GridRowSelectContacto"
                                Mode="Single">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectContactos" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersContactos"
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
                                            <Click Fn="pagingInitContactos" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnPagingPre"
                                        Disabled="true"
                                        Cls="noBorder"
                                        IconCls="x-tbar-page-prev">
                                        <Listeners>
                                            <Click Fn="pagingPreContactos" />
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
                                            <BeforeRender Fn="nfPaginNumberBeforeRenderContactos" />
                                            <Change Fn="paginGoToContactos" />
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
                                            <Click Fn="paginNextContactos" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnPaginLast"
                                        Enabled="false"
                                        Cls="noBorder"
                                        IconCls="x-tbar-page-last">
                                        <Listeners>
                                            <Click Fn="paginLastContactos" />
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
                                            <Select Fn="pageSelectContactos" />
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
