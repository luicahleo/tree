<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalidadKPIresults.aspx.cs" Inherits="TreeCore.ModCalidad.CalidadKPIresults" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdCategoria" runat="server" />

            <%--FIN HIDDEN --%

            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>

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
                                        ID="wrapComponenteCentralKPIResult"
                                        Hidden="false"
                                        Layout="BorderLayout"
                                        BodyCls="tbGrey">
                                        <Listeners>
                                            <AfterLayout Handler="ControlPaneles(this)"></AfterLayout>
                                            <Resize Handler="ControlPaneles(this)"></Resize>
                                        </Listeners>
                                        <Items>
                                            <ext:GridPanel 
                                                ID="grdMenuKPICategory"
                                                Flex="12"
                                                MaxWidth="300"
                                                MinWidth="200"
                                                Hidden="false"
                                                HideHeaders="true"
                                                Region="West"
                                                Title="<%$ Resources:Comun, strCategoria %>"
                                                runat="server"
                                                Cls="gridPanel gridSimpleMenu TreePL"
                                                OverflowX="Hidden"
                                                OverflowY="Auto">
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="storeDQCategorias"
                                                        AutoLoad="true"
                                                        OnReadData="storeDQCategorias_Refresh"
                                                        RemoteSort="true">
                                                        <Proxy>
                                                            <ext:PageProxy />
                                                        </Proxy>
                                                        <Listeners>
                                                            <BeforeLoad Fn="DeseleccionarGrilla" />
                                                        </Listeners>
                                                        <Model>
                                                            <ext:Model runat="server" IDProperty="ID">
                                                                <Fields>
                                                                    <ext:ModelField Name="ID" Type="Int" />
                                                                    <ext:ModelField Name="Categoria" Type="String" />
                                                                    <ext:ModelField Name="Valores" Type="Int" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:TemplateColumn
                                                            runat="server"
                                                            MinWidth="120"
                                                            DataIndex="Categoria"
                                                            Flex="8"
                                                            ID="Column1">
                                                            <Template runat="server">
                                                                <Html>
                                                                    <tpl for=".">
												                        <table class="tmpCol-table">
																			<tr class="tmpCol-tr">
																				<td class="tmpCol-td colName" colspan="3">
                                                                                    <span class="dataGrd">{Categoria}</span>
                                                                                    <span class="dataGrd"> ({Valores})</span>
																				</td>
																			</tr>
																		</table>
                               										</tpl>
                                                                </Html>
                                                            </Template>
                                                            <Renderer Fn="asignarColumna" />
                                                        </ext:TemplateColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server"
                                                        ID="GridRowSelect">
                                                        <Listeners>
                                                            <Select Fn="Grid_RowSelect"></Select>
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                            </ext:GridPanel>

                                            <ext:GridPanel
                                                Region="Center"
                                                Flex="1"
                                                Hidden="false"
                                                SelectionMemory="false"
                                                EnableColumnHide="false"
                                                Title=""
                                                runat="server"
                                                Header="false"
                                                ID="gridMain1"
                                                Cls="gridPanel grdNoHeader "
                                                OverflowX="Hidden"
                                                OverflowY="Auto">
                                                <Listeners>
                                                    <AfterRender Handler="GridColHandler(this)"></AfterRender>
                                                    <Resize Handler="GridColHandler(this)"></Resize>
                                                </Listeners>
                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar5"
                                                        Dock="Top"
                                                        Padding="0"
                                                        Cls="tlbGrid c-Grid">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnCloseShowVisorTreeP"
                                                                IconCls="ico-hide-menu"
                                                                Cls="btnSbCategory"
                                                                Handler="VisorSwitch(this)">
                                                            </ext:Button>
                                                            <ext:Label runat="server"
                                                                ID="lblTituloGrid"
                                                                Cls="HeaderLblVisor">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>

                                                    <ext:Toolbar runat="server"
                                                        ID="tlbBase"
                                                        Dock="Top"
                                                        Cls="tlbGrid"
                                                        OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnTime"
                                                                Cls="btnTime "
                                                                Disabled="true"
                                                                ToolTip="<%$ Resources:Comun, jsEjecutar %>">
                                                                <Listeners>
                                                                    <Click Fn="ejecutarKPI" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnRefrescarGrid"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                Cls="btnRefrescar"
                                                                Handler="RefrescarGrid();" />
                                                            <ext:Button runat="server"
                                                                ID="btnDescargarGrid"
                                                                Cls="btnDescargar"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Handler="ExportarDatos('CalidadKPIresults', App.hdCliID.value, #{gridMain1}, App.hdCategoria.value, '');" />
                                                            <ext:Button runat="server"
                                                                ID="btnDetalleKPI"
                                                                ToolTip="<%$ Resources:Comun, strFiltros %>"
                                                                Disabled="true"
                                                                Cls="btnDetalleKPI"
                                                                Handler="parent.hideAsideRCalidad('panelFiltros');" />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="storeDQKpisMonitoring"
                                                        AutoLoad="true"
                                                        RemotePaging="true"
                                                        OnReadData="storeDQKpisMonitoring_Refresh"
                                                        RemoteSort="true"
                                                        PageSize="20">
                                                        <Proxy>
                                                            <ext:PageProxy />
                                                        </Proxy>
                                                        <Listeners>
                                                            <BeforeLoad Fn="DeseleccionarGrillaMonitoring" />
                                                        </Listeners>
                                                        <Model>
                                                            <ext:Model runat="server" IDProperty="DQKpi">
                                                                <Fields>
                                                                    <ext:ModelField Name="DQKpiMonitoringID" Type="Int" />
                                                                    <ext:ModelField Name="DQKpi" Type="String" />
                                                                    <ext:ModelField Name="DQKpiID" Type="Int" />
                                                                    <ext:ModelField Name="BeforeLast" Type="String" />
                                                                    <ext:ModelField Name="Last" Type="String" />
                                                                    <ext:ModelField Name="Current" Type="String" />
                                                                    <ext:ModelField Name="Colour" Type="String" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                        <Sorters>
                                                            <ext:DataSorter Property="DqKpi" Direction="ASC" />
                                                        </Sorters>
                                                    </ext:Store>
                                                </Store>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column runat="server"
                                                            Flex="8"
                                                            MinWidth="120"
                                                            Text="KPI"
                                                            ID="colKPI"
                                                            DataIndex="DQKpi"
                                                            Align="Start">
                                                        </ext:Column>
                                                        <ext:Column runat="server"
                                                            ID="colGeneral"
                                                            Text="<%$ Resources:Comun, strGeneral %>"
                                                            Flex="3"
                                                            MinWidth="100"
                                                            DataIndex="Colour"
                                                            Align="Center">
                                                            <Renderer Fn="ColorRender" />
                                                        </ext:Column>
                                                        <ext:Column runat="server"
                                                            ID="colCurrent"
                                                            Text="<%$ Resources:Comun, strActuaal %>"
                                                            Flex="8"
                                                            MinWidth="100"
                                                            DataIndex="Current"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:Column runat="server"
                                                            ID="colLast"
                                                            Text="<%$ Resources:Comun, strUltima %>"
                                                            Flex="8"
                                                            MinWidth="100"
                                                            DataIndex="Last"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:Column runat="server"
                                                            Flex="8"
                                                            MinWidth="100"
                                                            Text="<%$ Resources:Comun, strPenultima %>"
                                                            ID="colBeforeLast"
                                                            DataIndex="BeforeLast"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:WidgetColumn ID="ColMore"
                                                            runat="server"
                                                            Cls="col-More"
                                                            DataIndex=""
                                                            Align="Center"
                                                            Text="More"
                                                            Hidden="false"
                                                            MinWidth="90"
                                                            Flex="99999">
                                                            <Widget>
                                                                <ext:Button ID="btnMore"
                                                                    runat="server"
                                                                    Width="60"
                                                                    OverCls="Over-btnMore"
                                                                    PressedCls="Pressed-none"
                                                                    FocusCls="Focus-none"
                                                                    Cls="btnColMore">
                                                                    <Listeners>
                                                                        <Click Handler="parent.hideAsideRCalidad('panelMore');" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Widget>
                                                        </ext:WidgetColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server"
                                                        ID="GridRowSelectGrilla"
                                                        Mode="Single">
                                                        <Listeners>
                                                            <Select Fn="Grid_RowSelectGrilla" />
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
                                                        StoreID="storeDQKpisMonitoring"
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
                                            </ext:GridPanel>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>

                            <%-- PANEL LATERAL DESPLEGABLE--%>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
