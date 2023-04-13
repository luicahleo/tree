<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoringLog.aspx.cs" Inherits="TreeCore.ModMonitoring.MonitoringLog" %>

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
            <ext:Hidden ID="hdCarpeta" runat="server" />

            <%--FIN HIDDEN --%>

            <script src="js/MonitoringLog.js"></script>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager
                runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store runat="server"
                ID="storePrincipal"
                RemotePaging="false"
                AutoLoad="true"
                OnReadData="storePrincipal_Refresh"
                RemoteSort="false"
                shearchBox="cmpFiltro_txtSearch"
                listNotPredictive="LogID"
                PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="LogID">
                        <Fields>
                            <ext:ModelField Name="LogID" Type="Int" />
                            <ext:ModelField Name="tipo" Type="String" />
                            <ext:ModelField Name="ruta" Type="String" />
                            <ext:ModelField Name="nombrefichero" Type="String" />
                            <ext:ModelField Name="fechaultima" Type="Date" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <DataChanged Fn="BuscadorPredictivo" />
                </Listeners>
                <Sorters>
                    <ext:DataSorter Property="fechaultima" Direction="DESC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>
            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server" ID="MainVwP" OverflowY="auto" Layout="FitLayout">
                <Listeners>
                </Listeners>

                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <Content>
                            <ext:Button runat="server" ID="btnCollapseAsRClosed" Cls="btn-trans btnCollapseAsRClosedv3" Handler="OcultarPanelLateral();" Disabled="false" Hidden="true"></ext:Button>
                        </Content>
                        <Items>

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
                                    <ext:Container runat="server" ID="WrapAlturaCabecera" Dock="Top" Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:Toolbar runat="server" ID="tbTitulo" Dock="Top" Cls="tbGrey tbTitleAlignBot tbNoborder" Hidden="false" Layout="ColumnLayout" Flex="1">
                                                <Items>

                                                    <ext:Label runat="server" ID="lbltituloPrincipal" Cls="TituloCabecera" Text="<%$ Resources:Comun, strLogs %>" Height="25" MarginSpec="10 0 10 0" />

                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Container>
                                </DockedItems>
                                <Items>

                                    <ext:GridPanel
                                        runat="server"
                                        ID="grid"
                                        meta:resourceKey="grid"
                                        SelectionMemory="false"
                                        Cls="gridPanel"
                                        StoreID="storePrincipal"
                                        Title="etiqgridTitle"
                                        Header="false"
                                        Scrollable="Vertical"
                                        EnableColumnHide="false"
                                        AriaRole="main">
                                        <Listeners>
                                            <AfterRender Handler="GridColHandler(this)"></AfterRender>
                                            <Resize Handler="GridColHandler(this)"></Resize>
                                        </Listeners>
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tlbBase" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                                                <Items>                                                    
                                                    <ext:Button runat="server"
                                                        ID="btnError"
                                                        ToolTip="<%$ Resources:Comun, jsError %>"
                                                        Cls="btnerror"
                                                        EnableToggle="true"
                                                        Pressed="true"
                                                        Handler="Logs('error');" />                                                    
                                                    <ext:Button runat="server"
                                                        ID="btnInfo"
                                                        ToolTip="<%$ Resources:Comun, jsInfo %>"
                                                        Cls="btnInfo"
                                                        EnableToggle="true"
                                                        Pressed="true"
                                                        Handler="Logs('info');" />                                                   
                                                    <ext:Button runat="server"
                                                        ID="btnRefrescar"
                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                        Cls="btnRefrescar"
                                                        Handler="Logs('refresh')" />
                                                    <ext:Button runat="server"
                                                        ID="btnDescargar"
                                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                        Cls="btnDescargar"
                                                        Handler="ExportarDatosSinCliente('MonitoringLog', #{grid}, App.cmpFiltro_cmbFechasRango.value, '', App.cmpFiltro_cmbClientes.value, hdCarpeta.value);" />
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top">
                                                <Content>
                                                    <local:toolbarFiltros
                                                        ID="cmpFiltro"
                                                        runat="server"
                                                        Stores="storePrincipal"
                                                        MostrarComboFecha="true"
                                                        QuitarFiltros="true"
                                                        FechaDefecto="Dia"
                                                        Grid="grid"
                                                        MostrarBusqueda="false" />
                                                </Content>
                                            </ext:Container>
                                        </DockedItems>
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:CommandColumn runat="server" Cls="excluirPnInfo">
                                                    <Commands>
                                                        <ext:GridCommand IconCls="ico-download-grid-16" Cls="noBorder" CommandName="DescargarLog">
                                                            <ToolTip Text="<%$ Resources:Comun, strDocumento %>" />
                                                        </ext:GridCommand>
                                                        <ext:GridCommand IconCls="ico-codeGrid" Cls="noBorder" CommandName="VerLog">
                                                            <ToolTip Text="<%$ Resources:Comun, strMostrarArchivo %>" />
                                                        </ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Handler="SeleccionarComando(command, record.data.nombrefichero, record.data.ruta, record.data.tipo);" />
                                                    </Listeners>
                                                </ext:CommandColumn>
                                                <ext:Column runat="server"
                                                    ID="colTipo"
                                                    DataIndex="tipo"
                                                    Text="<%$ Resources:Comun, strTipo %>"
                                                    MinWidth="120"
                                                    Flex="1">
                                                </ext:Column>
                                                <ext:DateColumn runat="server"
                                                    ID="colFechaUltima"
                                                    DataIndex="fechaultima"
                                                    Flex="1"
                                                    Text="<%$ Resources:Comun, strFecha/Hora %>"
                                                    MinWidth="120"
                                                    Format="d/m/Y G:i">
                                                </ext:DateColumn>
                                                <ext:Column runat="server"
                                                    ID="colNombreFichero"
                                                    DataIndex="nombrefichero"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    MinWidth="120"
                                                    Flex="1">
                                                </ext:Column>
                                                <ext:WidgetColumn ID="ColMore" runat="server" Cls="col-More" DataIndex="" Align="Center" Text="More" Hidden="false" MaxWidth="90" Flex="99999">
                                                    <Widget>
                                                        <ext:Button ID="btnMore" runat="server" Width="60" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColMore">
                                                            <Listeners>
                                                                <Click Fn="hidePanelMoreInfo" />
                                                            </Listeners>
                                                        </ext:Button>
                                                    </Widget>
                                                </ext:WidgetColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="GridRowSelect"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelect" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Plugins>
                                            <ext:GridFilters runat="server"
                                                ID="gridFilters"
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                meta:resourceKey="GridFilters" />
                                            <ext:CellEditing runat="server"
                                                ClicksToEdit="2" />
                                        </Plugins>
                                        <BottomBar>
                                            <ext:PagingToolbar runat="server"
                                                ID="PagingToolBar"
                                                meta:resourceKey="PagingToolBar"
                                                StoreID="storePrincipal"
                                                DisplayInfo="true"
                                                HideRefresh="true">
                                                <Items>
                                                    <ext:ComboBox runat="server"
                                                        Cls="comboGrid"
                                                        Width="80">
                                                        <Items>
                                                            <ext:ListItem Text="10" />
                                                            <ext:ListItem Text="20" />
                                                            <ext:ListItem Text="30" />
                                                            <ext:ListItem Text="40" />
                                                            <ext:ListItem Text="50" />
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


                            <ext:Panel runat="server" ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                Width="380"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="AnchorLayout"
                                Header="false" Border="false" Hidden="false">
                                <Listeners>
                                </Listeners>
                                <Items>
                                    <%-- PANEL MORE INFO--%>


                                    <ext:Panel runat="server" ID="pnMoreInfo" Hidden="true" Cls="tbGrey grdIntoAside" AnchorVertical="100%" AnchorHorizontal="100%" OverflowY="Auto">

                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="Toolbar1" Cls="tbGrey" Dock="Top" Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label1" runat="server" IconCls="ico-head-info" Cls="lblHeadAside " Text="MORE INFO" MarginSpec="36 15 30 15"></ext:Label>
                                                </Items>
                                            </ext:Toolbar>
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
