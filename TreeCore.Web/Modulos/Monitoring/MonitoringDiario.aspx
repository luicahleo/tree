<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoringDiario.aspx.cs" Inherits="TreeCore.ModMonitoring.MonitoringDiario" %>

<%@ Register Src="~/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Grid Simple</title>

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
            <ext:Hidden ID="FormatType" runat="server" />
            <ext:Hidden ID="hdUsuarioID" runat="server">
            </ext:Hidden>
            <ext:Hidden ID="hdProyectoTipoID" runat="server">
            </ext:Hidden>
            <ext:Hidden ID="Hidden1" runat="server">
            </ext:Hidden>
            <ext:Hidden ID="hdFiltro" runat="server">
            </ext:Hidden>
            <ext:Hidden ID="hdCount" runat="server">
            </ext:Hidden>
            <ext:Hidden ID="hdSort" runat="server">
            </ext:Hidden>
            <ext:Hidden ID="hdFechaInicio" runat="server">
            </ext:Hidden>
            <ext:Hidden ID="hdFechaFin" runat="server">
            </ext:Hidden>
            <ext:Store runat="server"
                ID="storePrincipal"
                RemotePaging="false"
                AutoLoad="true"
                OnReadData="storePrincipal_Refresh"
                RemoteSort="true"
                shearchBox="cmpFiltro_txtSearch"
                listNotPredictive="EmplazamientosSeguimientosGenericoID,UsuarioID,DocumentoID,Activo,Interno"
                PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="EmplazamientosSeguimientosGenericoID">
                        <Fields>
                            <ext:ModelField Name="EmplazamientosSeguimientosGenericoID" Type="Int" />
                            <ext:ModelField Name="UsuarioID" Type="Int" />
                            <ext:ModelField Name="ProyectoTipo" />
                            <ext:ModelField Name="Fecha" Type="Date" />
                            <ext:ModelField Name="EmplazamientoNombre" />
                            <ext:ModelField Name="EmplazamientoCodigo" />
                            <ext:ModelField Name="EmplazamientoEstado_esES" />
                            <ext:ModelField Name="NombreUsuario" />
                            <ext:ModelField Name="Email" />
                            <ext:ModelField Name="Interno" Type="Boolean" />
                            <ext:ModelField Name="Nota" />
                            <ext:ModelField Name="Cambios" />
                            <ext:ModelField Name="DocumentoID" Type="Int" />
                            <ext:ModelField Name="Documento" />
                            <ext:ModelField Name="Soporte" Type="Boolean" />
                            <ext:ModelField Name="EmpresaProveedora" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="Alias" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <DataChanged Fn="BuscadorPredictivo" />
                </Listeners>
                <Sorters>
                    <ext:DataSorter Property="Fecha" Direction="DESC" />
                </Sorters>
            </ext:Store>



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

                                                    <ext:Label runat="server" ID="lbltituloPrincipal" Cls="TituloCabecera" Text="<%$ Resources:Comun, strMonitoringDiario %>" Height="25" MarginSpec="10 0 10 0" />

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
                                                        ToolTip="<%$ Resources:Comun, btnDescargarSinFiltro.ToolTip %>"
                                                        Cls="btnDescargar"
                                                        Handler="ExportarDatos('MonitoringDiario', hdCliID.value, #{grid}, App.cmpFiltro_cmbFechasRango.value,'',App.cmpFiltro_cmbClientes.value);" />
                                                </Items>
                                            </ext:Toolbar>

                                            <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top">
                                                <Content>

                                                    <local:toolbarFiltros
                                                        ID="cmpFiltro"
                                                        runat="server"
                                                        Stores="storePrincipal"
                                                        MostrarComboFecha="true"
                                                      
                                                        FechaDefecto="Dia"
                                                        Grid="grid"
                                                        QuitarFiltros="true"
                                                        MostrarBusqueda="false" />

                                                </Content>
                                            </ext:Container>

                                        </DockedItems>
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column
                                                    DataIndex="DocumentoID"
                                                    Text="<%$ Resources:Comun, strDocumento %>"
                                                    Cls="excluirPnInfo"
                                                    MinWidth="120"
                                                    Align="Center"
                                                    runat="server">
                                                    <Renderer Fn="DescargaDocumentoRender" />
                                                </ext:Column>
                                                <ext:DateColumn
                                                    DataIndex="Fecha"
                                                    Text="<%$ Resources:Comun, strFechaHora %>"
                                                    Format="d/m/Y G:i"
                                                    MinWidth="120"
                                                    Align="Center"
                                                    Flex="1"
                                                    ID="Fecha"
                                                    runat="server">
                                                </ext:DateColumn>
                                                <ext:Column
                                                    DataIndex="Alias"
                                                    MinWidth="120"
                                                    Text="<%$ Resources:Comun, strAlias %>"
                                                    ID="Alias"
                                                    Flex="1"
                                                    runat="server" />
                                                <ext:Column
                                                    DataIndex="NombreUsuario"
                                                    Text="<%$ Resources:Comun, strNombreUsuario %>"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    ID="NombreUsuario"
                                                    runat="server" />
                                                <ext:Column
                                                    DataIndex="Email"
                                                    Text="<%$ Resources:Comun, strEmail %>"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    ID="Email"
                                                    runat="server" />
                                                <ext:Column
                                                    DataIndex="EmpresaProveedora"
                                                    Text="<%$ Resources:Comun, strEmpresaProveedora %>"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    Align="Center"
                                                    ID="EmpresaProveedora"
                                                    runat="server">
                                                </ext:Column>
                                                <ext:Column
                                                    DataIndex="EmplazamientoCodigo"
                                                    Text="<%$ Resources:Comun, strEmplazamientoCodigo %>"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    ID="EmplazamientoCodigo"
                                                    runat="server">
                                                </ext:Column>
                                                <ext:Column
                                                    DataIndex="EmplazamientoNombre"
                                                    Text="<%$ Resources:Comun, strEmplazamientoNombre %>"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    ID="EmplazamientoNombre"
                                                    runat="server">
                                                </ext:Column>
                                                <ext:Column
                                                    DataIndex="EmplazamientoEstado_esES"
                                                    Text="<%$ Resources:Comun, strEstado %>"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    ID="EmplazamientoEstado_esES"
                                                    runat="server">
                                                </ext:Column>
                                                <ext:Column
                                                    DataIndex="Interno"
                                                    Text="<%$ Resources:Comun, strInterno %>"
                                                    MinWidth="120"
                                                    Align="Center"
                                                    Flex="1"
                                                    ID="Interno"
                                                    runat="server">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column
                                                    DataIndex="Soporte"
                                                    Text="<%$ Resources:Comun, strSoporte %>"
                                                    MinWidth="120"
                                                    Align="Center"
                                                    Flex="1"
                                                    ID="Soporte"
                                                    runat="server">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column
                                                    DataIndex="Nota"
                                                    Text="<%$ Resources:Comun, strNotas %>"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    ID="Nota"
                                                    runat="server">
                                                </ext:Column>
                                                <ext:Column
                                                    Text="<%$ Resources:Comun, strCambios %>"
                                                    DataIndex="Cambios"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    ID="Cambios"
                                                    runat="server">
                                                </ext:Column>
                                                <ext:WidgetColumn ID="ColMore" 
                                                    runat="server" 
                                                    Cls="col-More" 
                                                    DataIndex="" 
                                                    Align="Center" 
                                                    Text="More"
                                                    Hidden="false"
                                                    MaxWidth="90" 
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
                                                OverflowHandler="Scroller"
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
        </div>
    </form>
</body>
</html>
