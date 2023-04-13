<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoringServiciosWeb.aspx.cs" Inherits="TreeCore.ModMonitoring.MonitoringServiciosWeb" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body class="bodyFMenu">
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />

            <%--FIN HIDDEN --%>
            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">

                <Listeners>
                </Listeners>

            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>

            <ext:Store runat="server"
                ID="storePrincipal"
                RemotePaging="false"
                AutoLoad="true"
                OnReadData="storePrincipal_Refresh"
                RemoteSort="true"
                shearchBox="cmpFiltro_txtSearch"
                listNotPredictive="MonitoringWSRegistroID,ProyectoTipoID,UsuarioID,ClienteID"
                PageSize="20">

                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="MonitoringWSRegistroID">
                        <Fields>
                            <ext:ModelField Name="MonitoringWSRegistroID" Type="Int" />
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="UsuarioID" Type="Int" />
                            <ext:ModelField Name="FechaCreacion" Type="Date" />
                            <ext:ModelField Name="ParametrosEntrada" />
                            <ext:ModelField Name="ParametrosSalida" />
                            <ext:ModelField Name="ToolServicioID" Type="Int" />
                            <ext:ModelField Name="NombreMetodo" />
                            <ext:ModelField Name="Comentarios" />
                            <ext:ModelField Name="ProyectoTipo" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Apellidos" />
                            <ext:ModelField Name="EMail" />
                            <ext:ModelField Name="NombreCompleto" />
                            <ext:ModelField Name="Servicio" />
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="Exito" Type="Boolean" />
                            <ext:ModelField Name="EsServicioPropio" Type="Boolean" />
                            <ext:ModelField Name="CodigoEmplazamiento" />
                            <ext:ModelField Name="NumContrato" />
                            <ext:ModelField Name="CodigoSAP" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <DataChanged Fn="BuscadorPredictivo" />
                </Listeners>
                <Sorters>
                    <ext:DataSorter Property="FechaCreacion" Direction="DESC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeServicios"
                AutoLoad="true"
                OnReadData="storeServicios_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="ProyectoTipoID">
                        <Fields>
                            <ext:ModelField Name="ToolConexionID" Type="Int" />
                            <ext:ModelField Name="Integracion" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Window meta:resourceKey="winParametros"
                ID="winParametros"
                runat="server"
                Title="<%$ Resources:Comun, strParametro %>"
                Width="400"
                Modal="true"
                Hidden="true"
                Height="195"
                BodyCls=""
                Cls="winForm-respSimple"
                Scrollable="Vertical"
                Layout="VBoxLayout">
                <Items>
                    <ext:FormPanel ID="FormPanelParametro"
                        runat="server"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextArea meta:resourceKey="txaParametro"
                                ID="txaParametro"
                                runat="server"
                                Width="380"
                                Height="500"
                                ReadOnly="true"
                                AllowBlank="false" />
                        </Items>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button meta:resourceKey="btnCancelar"
                        ID="btnCancelarParametros"
                        runat="server"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winParametros}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Show Handler="#{winParametros}.center();" />
                </Listeners>
            </ext:Window>



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
                                            <ext:Toolbar runat="server" ID="tbTitulo" Dock="Top" Cls="tbGrey tbTitleAlignBot tbNoborder" Hidden="false" Layout="ColumnLayout" Flex="1" >
                                                <Items>

                                                    <ext:Label runat="server" ID="lbltituloPrincipal" Cls="TituloCabecera" Text="<%$ Resources:Comun, strServiciosWeb %>" Height="25" MarginSpec="10 0 10 0" />

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
                                                        ID="btnRefrescar"
                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                        Cls="btnRefrescar"
                                                        Handler="Refrescar();" />
                                                    <ext:Button runat="server"
                                                        ID="btnDescargar"
                                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                        Cls="btnDescargar"
                                                        Handler="ExportarDatos('MonitoringServiciosWeb', hdCliID.value, #{grid}, App.cmpFiltro_cmbFechasRango.value,'',App.cmpFiltro_cmbClientes.value);" />
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
                                                <ext:CommandColumn runat="server" MinWidth="100" ID="colParametros" Cls="excluirPnInfo">
                                                    <Commands>
                                                        <ext:GridCommand Icon="ZoomIn" CommandName="ParametroEntrada">
                                                            <ToolTip Text="<%$ Resources:Comun, strDocumento %>" />
                                                        </ext:GridCommand>
                                                        <ext:GridCommand Icon="ZoomOut" CommandName="ParametroSalida">
                                                            <ToolTip Text="<%$ Resources:Comun, strDocumento %>" />
                                                        </ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Handler="SeleccionarComando(command, record);" />
                                                    </Listeners>
                                                </ext:CommandColumn>
                                                <ext:DateColumn runat="server"
                                                    ID="colFechaCreacion"
                                                    DataIndex="FechaCreacion"
                                                    Text="<%$ Resources:Comun, strFechaCreacion %>"
                                                    MinWidth="100"
                                                    Flex="1"
                                                    Format="d/m/Y G:i">
                                                </ext:DateColumn>
                                                <ext:Column runat="server"
                                                    ID="colNombreCompleto"
                                                    DataIndex="NombreCompleto"
                                                    Flex="1"
                                                    Text="<%$ Resources:Comun, strNombreCompleto %>"
                                                    MinWidth="100">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colEmail"
                                                    DataIndex="EMail"
                                                    Flex="1"
                                                    Text="<%$ Resources:Comun, strEmail %>"
                                                    MinWidth="100">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colProyectoTipo"
                                                    DataIndex="ProyectoTipo"
                                                    Flex="1"
                                                    Text="<%$ Resources:Comun, strProyectoTipo %>"
                                                    MinWidth="100">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colServicio"
                                                    Flex="1"
                                                    DataIndex="Servicio"
                                                    Text="<%$ Resources:Comun, strServicios %>"
                                                    MinWidth="100">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colNombreMetodo"
                                                    Flex="1"
                                                    DataIndex="NombreMetodo"
                                                    Text="<%$ Resources:Comun, strNombreMetodo %>"
                                                    MinWidth="100">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colCodigoEmplazamiento"
                                                    Flex="1"
                                                    DataIndex="CodigoEmplazamiento"
                                                    Text="<%$ Resources:Comun, strEmplazamientoCodigo %>"
                                                    MinWidth="100">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colNumContrato"
                                                    Flex="1"
                                                    DataIndex="NumContrato"
                                                    Text="<%$ Resources:Comun, strNumContrato %>"
                                                    MinWidth="100">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colCodigoSAP"
                                                    Flex="1"
                                                    DataIndex="CodigoSAP"
                                                    Text="<%$ Resources:Comun, strCodigoSAP %>"
                                                    MinWidth="100">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colExito"
                                                    DataIndex="Exito"
                                                    Flex="1"
                                                    Text="<%$ Resources:Comun, strExito %>"
                                                    MinWidth="100">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colEsServicioPropio"
                                                    Flex="1"
                                                    DataIndex="EsServicioPropio"
                                                    Text="<%$ Resources:Comun, strEsServicioPropio %>"
                                                    MinWidth="100">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:WidgetColumn ID="ColMore" runat="server" Cls="col-More" DataIndex="" Align="Center" Text="More" Hidden="false"  MaxWidth="90" Flex="99999">
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
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                            <ext:CellEditing runat="server"
                                                ClicksToEdit="2" />

                                        </Plugins>
                                        <BottomBar>
                                            <ext:PagingToolbar runat="server"
                                                ID="PagingToolBar"
                                                meta:resourceKey="PagingToolBar"
                                                StoreID="storePrincipal"
                                                DisplayInfo="true"
                                                OverflowHandler="Scroller"
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
        </div>
    </form>
</body>
</html>
