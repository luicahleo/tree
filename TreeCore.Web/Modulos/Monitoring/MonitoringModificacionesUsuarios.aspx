<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoringModificacionesUsuarios.aspx.cs" Inherits="TreeCore.ModMonitoring.MonitoringModificacionesUsuarios" %>

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
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdUsuarioID" runat="server" />
            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>

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
                            <ext:Button runat="server" ID="btnCollapseAsRClosed" Cls="btn-trans btnCollapseAsRClosedv3" Handler="hideAsideR();" Disabled="false" Hidden="true"></ext:Button>
                        </Content>
                        <Items>

                            <%-------------TENER EN CUENTA QUE AL CENTER MAIN SE LE PUEDEN ACOPLAR SEGMENTOS PLEGABLES CON LAS "REGIONS"-------------%>


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

                                                    <ext:Label runat="server" ID="lbltituloPrincipal" Cls="TituloCabecera" Text="<%$ Resources:Comun, strModificacionUsuarios %>" Height="25" MarginSpec="10 0 10 0" />

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
                                                        Handler="ExportarDatos('MonitoringModificacionesUsuarios', hdCliID.value, #{grid}, App.cmpFiltro_cmbFechasRango.value,'',App.cmpFiltro_cmbClientes.value);" />

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
                                                        FechaDefecto="Semana"
                                                        Grid="grid"
                                                        MostrarBusqueda="true" />
                                                </Content>
                                            </ext:Container>
                                        </DockedItems>
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storePrincipal"
                                                RemotePaging="false"
                                                AutoLoad="true"
                                                OnReadData="storePrincipal_Refresh"
                                                RemoteSort="true"
                                                shearchBox="cmpFiltro_txtSearch"
                                                listNotPredictive="UsuarioID,UsuarioModificadorID,Defecto,Activo"
                                                PageSize="20">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="MonitoringModificacionUsuarioID">
                                                        <Fields>
                                                            <ext:ModelField Name="UsuarioID" Type="Int" />
                                                            <ext:ModelField Name="UsuarioModificadorID" Type="Int" />
                                                            <ext:ModelField Name="OperacionRealizada" Type="String" />
                                                            <ext:ModelField Name="CambioEfectuado" Type="String" />
                                                            <ext:ModelField Name="NombreCompletoUsuario" Type="String" />
                                                            <ext:ModelField Name="NombreCompletoModificador" Type="String" />
                                                            <ext:ModelField Name="FechaModificacion" Type="Date" />
                                                            <ext:ModelField Name="FechaDesactivacion" Type="Date" />
                                                            <ext:ModelField Name="FechaCreacion" Type="Date" />
                                                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                                                            <ext:ModelField Name="ProyectoTipo" Type="String" />
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
                                                    <ext:DataSorter Property="FechaModificacion" Direction="DESC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel>
                                            <Columns>

                                                <ext:DateColumn
                                                    DataIndex="FechaModificacion"
                                                    Text="<%$ Resources:Comun, strFechaModificacion %>"
                                                    Format="d/m/Y G:i"
                                                    Flex="1"
                                                    MinWidth="120"
                                                    ID="FechaModificacion"
                                                    runat="server" />
                                                <ext:Column
                                                    DataIndex="Alias"
                                                    Text="<%$ Resources:Comun, strAlias %>"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    ID="ProyectoTipo"
                                                    runat="server">
                                                </ext:Column>
                                                <ext:Column
                                                    DataIndex="NombreCompletoUsuario"
                                                    Text="<%$ Resources:Comun, strNombreCompleto %>"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    ID="NombreCompletoUsuario"
                                                    runat="server">
                                                </ext:Column>
                                                <ext:Column
                                                    DataIndex="NombreCompletoModificador"
                                                    Text="<%$ Resources:Comun, strNombreCompletoMod %>"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    ID="NombreCompletoModificador"
                                                    runat="server">
                                                </ext:Column>
                                                <ext:Column
                                                    DataIndex="OperacionRealizada"
                                                    Text="<%$ Resources:Comun, strOperacionRealizada %>"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    ID="OperacionRealizada"
                                                    runat="server">
                                                </ext:Column>
                                                <ext:Column
                                                    DataIndex="CambioEfectuado"
                                                    Text="<%$ Resources:Comun, strCambioEfectuado %>"
                                                    MinWidth="120"
                                                    Flex="1"
                                                    ID="CambioEfectuado"
                                                    runat="server">
                                                </ext:Column>
                                                <ext:DateColumn
                                                    DataIndex="FechaCreacion"
                                                    Text="<%$ Resources:Comun, strFechaCreacion %>"
                                                    Format="d/m/Y G:i"
                                                    Flex="1"
                                                    MinWidth="120"
                                                    ID="FechaCreacion"
                                                    runat="server" />
                                                <ext:DateColumn
                                                    DataIndex="FechaDesactivacion"
                                                    Text="<%$ Resources:Comun, strFechaDesactivacion %>"
                                                    Format="d/m/Y G:i"
                                                    Flex="1"
                                                    MinWidth="120"
                                                    ID="FechaDesactivacion"
                                                    runat="server" />

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

                                        <View>
                                            <ext:GridView runat="server" TrackOver="false" EmptyText="<h1>No matching results</h1>" EnableTextSelection="true">
                                            </ext:GridView>
                                        </View>

                                    </ext:GridPanel>

                                </Items>
                            </ext:Panel>


                            <%-- PANEL LATERAL DESPLEGABLE--%>


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
