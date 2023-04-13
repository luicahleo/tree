<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalidadKPIReport.aspx.cs" Inherits="TreeCore.ModCalidad.pages.CalidadKPIReport" %>

<%@ Register Src="~/Componentes/toolbarFiltrosRes1Combo.ascx" TagName="toolbarFiltrosRes1Combo" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>




</head>
<body class="bodyFMenu">
    <script src="https://cdn.amcharts.com/lib/4/core.js"></script>
    <script src="https://cdn.amcharts.com/lib/4/charts.js"></script>
    <script src="https://cdn.amcharts.com/lib/4/themes/animated.js"></script>
    <script src="/Scripts/html2canvas.js"></script>
    <link rel="stylesheet" type="text/css" href="/Scripts/slick.css" />
    <link rel="stylesheet" type="text/css" href="/Scripts/slick-theme.css" />
    <script type="text/javascript" src="/Scripts/slick.min.js"></script>

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdDQKpiID" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>


            <%--INICIO  STORES --%>
            <ext:Store
                ID="storeGrupos"
                runat="server"
                OnReadData="storeGrupos_Refresh"
                AutoLoad="false"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="DQGroupMonitoringID">
                        <Fields>
                            <ext:ModelField Name="DQGroupMonitoringID" />
                            <ext:ModelField Name="Total" />
                            <ext:ModelField Name="NumeroElementos" />
                            <ext:ModelField Name="Version" />
                            <ext:ModelField Name="DQGroup" />
                            <ext:ModelField Name="NombreCompleto" />
                            <ext:ModelField Name="DQGroupID" />
                            <ext:ModelField Name="DQKpiID" />
                            <ext:ModelField Name="NombreTipoCondicion" />
                            <ext:ModelField Name="DQGroupID" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <DataChanged Fn="pintarGraficosGrupos" />
                </Listeners>
            </ext:Store>

            <ext:Store
                ID="storeKpisMonitoring"
                runat="server"
                AutoLoad="false"
                RemoteSort="true"
                OnReadData="storeKpisMonitoring_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="DQKpiMonitoringID">
                        <Fields>
                            <ext:ModelField Name="DQKpiMonitoringID" />
                            <ext:ModelField Name="DQKpi" />
                            <ext:ModelField Name="FechaEjecucion" />
                            <ext:ModelField Name="NumeroElementos" />
                            <ext:ModelField Name="Total" />
                            <ext:ModelField Name="Version" />
                            <ext:ModelField Name="Ultima" />
                            <ext:ModelField Name="Activa" />
                            <ext:ModelField Name="DQCategoria" />
                            <ext:ModelField Name="NombreCompleto" />
                            <ext:ModelField Name="IntervaloVerde" />
                            <ext:ModelField Name="IntervaloRojo" />
                            <ext:ModelField Name="PorcentajeError" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <DataChanged Fn="storeKpisMonitoring" />
                </Listeners>
            </ext:Store>

            <ext:Store
                ID="storeUltimosKpisMonitoring"
                runat="server"
                AutoLoad="false"
                RemoteSort="true"
                OnReadData="storeUltimosKpisMonitoring_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="DQKpiMonitoringID">
                        <Fields>
                            <ext:ModelField Name="DQKpiMonitoringID" />
                            <ext:ModelField Name="DQKpi" />
                            <ext:ModelField Name="FechaEjecucion" />
                            <ext:ModelField Name="NumeroElementos" />
                            <ext:ModelField Name="Total" />
                            <ext:ModelField Name="Version" />
                            <ext:ModelField Name="Ultima" />
                            <ext:ModelField Name="Activa" />
                            <ext:ModelField Name="DQCategoria" />
                            <ext:ModelField Name="NombreCompleto" />
                            <ext:ModelField Name="IntervaloVerde" />
                            <ext:ModelField Name="IntervaloRojo" />
                            <ext:ModelField Name="PorcentajeError" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <DataChanged Fn="storeUltimosKpisMonitoring" />
                </Listeners>
            </ext:Store>



            <%--FIN  STORES --%>

            <ext:Viewport runat="server" ID="MainVwP" OverflowY="auto" Layout="FitLayout">
                <Listeners>
                </Listeners>

                <Content>
                    <ext:Button
                        runat="server"
                        ID="btnCollapseAsRClosed"
                        Cls="btn-trans btnCollapseAsRClosedv3"
                        Handler="hideAsideR();"
                        Disabled="false"
                        Hidden="true">
                    </ext:Button>
                </Content>

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
                                ID="tbFiltrosYSliders"
                                Dock="Top"
                                Cls="tbGrey tbNoborder "
                                Hidden="false"
                                Layout="HBoxLayout"
                                Flex="1">
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
                                                Handler="SliderMove('Prev');"
                                                Disabled="true">
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnNext"
                                                IconCls="ico-next-w"
                                                Cls="SliderBtn"
                                                Handler="SliderMove('Next');"
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
                                        ID="wrapComponenteCentralKPIReport"
                                        Hidden="false"
                                        Layout="BorderLayout"
                                        BodyCls="tbGrey">
                                        <Listeners>
                                            <AfterLayout Handler="ControlPaneles(this)"></AfterLayout>
                                            <Resize Handler="ControlPaneles(this)"></Resize>
                                        </Listeners>
                                        <Items>


                                            <ext:GridPanel
                                                Hidden="false"
                                                Cls="gridPanel GridPnl TreePL"
                                                ID="GridPanelSideL"
                                                runat="server"
                                                MaxWidth="400"
                                                Flex="12"
                                                Region="West"
                                                Resizable="false"
                                                Collapsible="false"
                                                CollapseMode="Mini"
                                                Scrollable="Vertical"
                                                EnableColumnHide="false"
                                                Title="<%$ Resources:Comun, strCalidadKPIReport %>">
                                                <Store>
                                                    <ext:Store
                                                        ID="storePrincipal"
                                                        runat="server"
                                                        AutoLoad="true"
                                                        OnReadData="storePrincipal_Refresh"
                                                        RemoteSort="true"
                                                        RemotePaging="false"
                                                        PageSize="20">
                                                        <Proxy>
                                                            <ext:PageProxy />
                                                        </Proxy>
                                                        <Model>
                                                            <ext:Model runat="server" IDProperty="DQKpiMonitoringID">
                                                                <Fields>
                                                                    <ext:ModelField Name="DQKpiMonitoringID" />
                                                                    <ext:ModelField Name="DQKpi" />
                                                                    <ext:ModelField Name="DQKpiID" />
                                                                    <ext:ModelField Name="Version" />
                                                                    <ext:ModelField Name="Ultima" />
                                                                    <ext:ModelField Name="Activa" />
                                                                    <ext:ModelField Name="DQCategoriaID" />
                                                                    <ext:ModelField Name="NumeroElementos" />
                                                                    <ext:ModelField Name="FechaEjecucion" Type="Date" />
                                                                    <ext:ModelField Name="Total" />
                                                                    <ext:ModelField Name="RutaPagina" />
                                                                    <ext:ModelField Name="idsResults" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                        <Sorters>
                                                            <ext:DataSorter Property="DQKpi" />
                                                        </Sorters>
                                                        <Listeners>
                                                            <BeforeLoad Fn="deseleccionarGrilla" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar8"
                                                        Cls=" "
                                                        Layout="HBoxLayout"
                                                        Dock="Top">
                                                        <Items>
                                                            <ext:ComboBox
                                                                runat="server"
                                                                ID="cmbDQCategorias"
                                                                Cls="comboGrid"
                                                                Flex="1"
                                                                EmptyText="<%$ Resources:Comun, strCategorias %>"
                                                                ValueField="DQCategoriaID"
                                                                DisplayField="DQCategoria">
                                                                <Store>
                                                                    <ext:Store
                                                                        ID="storeDQCategorias"
                                                                        runat="server"
                                                                        AutoLoad="false"
                                                                        OnReadData="storeDQCategorias_Refresh"
                                                                        RemoteSort="true">
                                                                        <Proxy>
                                                                            <ext:PageProxy />
                                                                        </Proxy>
                                                                        <Model>
                                                                            <ext:Model runat="server" IDProperty="DQCategoriaID">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="DQCategoriaID" />
                                                                                    <ext:ModelField Name="DQCategoria" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Fn="cmbDQCategorias" />
                                                                    <TriggerClick Fn="recargarDQCategorias" />
                                                                </Listeners>
                                                                <Triggers>
                                                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                                        IconCls="ico-reload"
                                                                        Hidden="true"
                                                                        Weight="-1"
                                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                                </Triggers>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>

                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:TemplateColumn
                                                            runat="server"
                                                            DataIndex="DQKpi"
                                                            ID="ColName"
                                                            Text="<%$ Resources:Comun, strKPI %>"
                                                            Flex="3">
                                                            <Template runat="server">
                                                                <Html>
                                                                    <tpl for=".">
												                        <table class="tmpCol-table" >
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td colName" colspan="3">
                                                                                    <span style=" line-height:18px;" class="dataGrd">
                                                                                        {DQKpi}
                                                                                    </span>
																				</td>
																			</tr>
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td colVersion" colspan="3">
                                                                                    <span style=" line-height:18px;" class="dataGrd">
                                                                                        v{Version}  -  {FechaEjecucion:date('d/m/Y')}
                                                                                    </span>
																				</td>
																			</tr>
																		</table>
                               										</tpl>
                                                                </Html>
                                                            </Template>
                                                        </ext:TemplateColumn>
                                                        <ext:Column
                                                            ID="colError"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strErrores %>"
                                                            Flex="1"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            TdCls="linkColumn"
                                                            DataIndex="NumeroElementos"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="colCorrect"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, jsCorrectos %>"
                                                            Flex="1"
                                                            MinWidth="80"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            DataIndex="NumeroElementos"
                                                            Align="Center">
                                                            <Renderer Fn="CalcularElementosCorrectos" />
                                                        </ext:Column>
                                                        <ext:Column
                                                            ID="ColActivo"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strActivo %>"
                                                            Flex="1"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            DataIndex="Total"
                                                            Align="Center">
                                                            <Renderer Fn="rendererColActivo" />
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>

                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server" ID="GridRowSelect">
                                                        <Listeners>
                                                            <Select Fn="SelectTreepn"></Select>
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <Plugins>
                                                    <ext:GridFilters runat="server"
                                                        ID="gridFilters"
                                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                                </Plugins>
                                                <BottomBar>
                                                    <ext:PagingToolbar runat="server"
                                                        StoreID="storePrincipal"
                                                        Cls="PgToolBMainGrid"
                                                        ID="PagingToolbar2"
                                                        MaintainFlex="true"
                                                        Flex="8"
                                                        HideRefresh="true"
                                                        OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:ComboBox runat="server"
                                                                Cls="comboGrid"
                                                                ID="ComboBox9"
                                                                MaxWidth="65">
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
                                                                    <Select Fn="handlePageSizeSelect" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:PagingToolbar>
                                                </BottomBar>
                                                <Listeners>
                                                    <AfterLayout Fn="addEventoLinkPagina" />
                                                </Listeners>
                                            </ext:GridPanel>



                                            <ext:Panel
                                                Region="Center"
                                                Flex="3"
                                                Hidden="false"
                                                Title="Grid Principal"
                                                runat="server"
                                                Header="false"
                                                ID="gridMain1"
                                                Cls="gridPanel grdNoHeader "
                                                OverflowX="Hidden"
                                                Layout="VBoxLayout"
                                                OverflowY="Auto">
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                </LayoutConfig>


                                                <DockedItems>

                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="tbCabeceravisor"
                                                        Dock="Top"
                                                        Padding="0"
                                                        Cls="tlbGrid c-Grid">

                                                        <Items>

                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnCloseShowVisorTreeP"
                                                                IconCls="ico-hide-menu"
                                                                Cls="btnSbCategory"
                                                                Handler="VisorSwitch(this)">
                                                            </ext:Button>
                                                            <ext:Label
                                                                runat="server"
                                                                ID="lblTituloReport"
                                                                Cls="HeaderLblVisor noWrapLbl"
                                                                Text="">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />

                                                        </Items>

                                                    </ext:Toolbar>

                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="tlbBase"
                                                        Dock="Top"
                                                        Cls="tlbGrid"
                                                        OverflowHandler="Scroller">
                                                        <Items>

                                                            <ext:Button runat="server"
                                                                ID="btnDescargar"
                                                                ToolTip="<%$ Resources:Comun, strDescarga %>"
                                                                meta:resourceKey="btnRefrescar"
                                                                Cls="btnDescargar"
                                                                Disabled="true">
                                                                <Listeners>
                                                                    <Click Fn="btnDescargar" />
                                                                </Listeners>
                                                            </ext:Button>

                                                            <ext:Button runat="server"
                                                                ID="btnSemaforo"
                                                                ToolTip="<%$ Resources:Comun, strSemaforo %>"
                                                                meta:resourceKey="btnSemaforo"
                                                                Cls="btnSemaforo"
                                                                Disabled="true"
                                                                Hidden="true">
                                                                <Listeners>
                                                                    <Click Fn="btnSemaforo" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnKPI"
                                                                ToolTip="<%$ Resources:Comun, strKPI %>"
                                                                meta:resourceKey="btnKPI"
                                                                Cls="btnKPI"
                                                                Disabled="true">
                                                                <Listeners>
                                                                    <Click Fn="btnKPI" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnDetalleKPI"
                                                                Disabled="true"
                                                                ToolTip="<%$ Resources:Comun, strFiltros %>"
                                                                Cls="btnDetalleKPI"
                                                                Handler="parent.hideAsideRCalidad('panelFiltros');" />

                                                        </Items>
                                                    </ext:Toolbar>

                                                </DockedItems>
                                                <Items>

                                                    <ext:Panel runat="server" ID="cont1" Cls="pnDonuts" Flex="1">
                                                        <Items>
                                                            <ext:Container runat="server">
                                                                <Content>
                                                                    <div id="piecharts-version" class="charts">
                                                                        <div class="content-piechart">
                                                                            <div id="Donut1" class="report-piechart"></div>
                                                                            <div class="text Donut1">
                                                                            </div>
                                                                        </div>
                                                                        <div class="content-piechart">
                                                                            <div id="Donut2" class="report-piechart"></div>
                                                                            <div class="text Donut2">
                                                                            </div>
                                                                        </div>
                                                                        <div class="content-piechart">
                                                                            <div id="Donut3" class="report-piechart"></div>
                                                                            <div class="text Donut3">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div id="piecharts-groups" class="charts">
                                                                    </div>
                                                                    <div id="graficoLineal" class="grafico-lineal charts"></div>
                                                                </Content>
                                                            </ext:Container>
                                                        </Items>
                                                        <Listeners>
                                                            <Resize Fn="ResizeCont1" />
                                                        </Listeners>
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
