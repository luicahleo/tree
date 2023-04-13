<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplazamientosGridAtributosDinamicos.aspx.cs" Inherits="TreeCore.ModGlobal.pages.EmplazamientosGridAtributosDinamicos" %>

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
                    <Change Fn="setNumMaxPageAtributos"></Change>
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdMaxColumns" runat="server" />
            
            <ext:Hidden ID="hdidsResultados" runat="server" />
            <ext:Hidden ID="hdnameIndiceID" runat="server" />
            <ext:Hidden ID="hdResultadoKPIid" runat="server" />


            <%--VIEW PORT--%>
            <ext:Viewport
                runat="server"
                ID="vwContenedor"
                Cls=""
                Layout="Anchor">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="grdEmplazamientosAtributos"
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
                                ID="storeEmplazamientosAtributos"
                                AutoLoad="false"
                                OnReadData="storeEmplazamientosAtributos_Refresh"
                                RemotePaging="true"
                                RemoteSort="true"
                                RemoteFilter="true"
                                PageSize="20">
                                <Listeners>
                                    <BeforeLoad Fn="DeseleccionarGrillaEmplazamientosAtributos" />
                                </Listeners>
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Model>
                                    <ext:Model runat="server" IDProperty="EmplazamientoID">
                                        <Fields>
                                            <ext:ModelField Name="EmplazamientoID" Type="Int" />
                                            <ext:ModelField Name="Codigo" Type="String" />
                                            <ext:ModelField Name="NombreSitio" Type="String" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <Sorters>
                                    <ext:DataSorter
                                        Property="EmplazamientoID"
                                        Direction="DESC" />
                                </Sorters>
                                <%--<Listeners>
                                    <DataChanged Fn="ajaxGetDatosBuscadorAtributos" />
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
                                            <Click Fn="refrescarEmplazamientosAtributos" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        Cls="btnDescargar"
                                        AriaLabel="Descargar"
                                        Hidden="false"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        Handler="ExportarDatos('EmplazamientosGridAtributosDinamicos', App.hdCliID.value, #{grdEmplazamientosAtributos}, '/ModGlobal/pages/EmplazamientosGridAtributosDinamicos.aspx', '',App.hdFiltrosAplicados.value, App.hdStringBuscador.value, App.hdIDEmplazamientoBuscador.value);">
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
                                        EmptyText="Search"
                                        LabelWidth="50"
                                        Width="250"
                                        EnableKeyEvents="true">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" />
                                        </Triggers>
                                        <Listeners>
                                            <KeyPress Fn="filtrarEmplazamientosPorBuscador" Buffer="500" />
                                            <TriggerClick Fn="LimpiarFiltroBusquedaEmplazamientosAtributos" />
                                            <FocusEnter Fn="ajaxGetDatosBuscadorAtributos" />
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
                                            <ext:Button runat="server" Width="30" ID="btnClearFilters" Cls="btn-trans btnRemoveFilters" AriaLabel="Quitar Filtro" ToolTip="<%$ Resources:Comun, strQuitarFiltro %>">
                                                <Listeners>
                                                    <Click Fn="BorrarFiltrosEmplazamientosAtributos"></Click>
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
                                <ext:WidgetColumn ID="ColMore" runat="server" Cls="col-More" DataIndex="" Align="Center" Text="More" Hidden="false" MinWidth="90">
                                    <Widget>
                                        <ext:Button meta:resourcekey="btnColMore" runat="server" ID="btnColMore" Width="60" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColMore">
                                            <Listeners>
                                                <Click Fn="MostrarPanelMoreInfo" />
                                            </Listeners>
                                        </ext:Button>
                                    </Widget>
                                </ext:WidgetColumn>
                                <ext:Column runat="server"
                                    ID="colCodigoEmplazamiento"
                                    DataIndex="Codigo"
                                    Filterable="false"
                                    Text="<%$ Resources:Comun, strCodigoEmplazamiento %>"
                                    MinWidth="120"
                                    Flex="1">
                                </ext:Column>
                                <ext:Column
                                    ID="colNombreEmplazamiento"
                                    runat="server"
                                    Filterable="false"
                                    Text="<%$ Resources:Comun, strNombre %>"
                                    DataIndex="NombreSitio"
                                    MinWidth="120"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel
                                runat="server"
                                ID="GridRowSelectEmplazamientosAtributos"
                                Mode="Single">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectEmplazamientosAtributos" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <ViewConfig StripeRows="true" />
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersEmplazamientosAtributos"
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
                                            <Click Fn="pagingInitAtributos" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnPagingPre"
                                        Disabled="true"
                                        Cls="noBorder"
                                        IconCls="x-tbar-page-prev">
                                        <Listeners>
                                            <Click Fn="pagingPreAtributos" />
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
                                            <BeforeRender Fn="nfPaginNumberBeforeRenderAtributos" />
                                            <Change Fn="paginGoToAtributos" />
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
                                            <Click Fn="paginNextAtributos" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnPaginLast"
                                        Enabled="false"
                                        Cls="noBorder"
                                        IconCls="x-tbar-page-last">
                                        <Listeners>
                                            <Click Fn="paginLastAtributos" />
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
                                            <Select Fn="pageSelectAtributos" />
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
