<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridEmplazamientosContactos.ascx.cs" Inherits="TreeCore.Componentes.GridEmplazamientosContactos" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>
<%@ Register Src="/Componentes/FormContactos.ascx" TagName="FormContactos" TagPrefix="local" %>

<script src="//maps.googleapis.com/maps/api/js?key=AIzaSyA0k02bRAAmDy-4x0kcN06k1auhLHKGRZs&v=3&libraries=places" type="text/javascript"></script>
<link href="/CSS/tCore.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="/PaginasComunes/js/Ext.ux.Map.js"></script>
<script type="text/javascript" src="/PaginasComunes/js/Ext.ux.GMapPanel.js"></script>
<script type="text/javascript" src="/Componentes/js/FormContactos.js"></script> 
<script type="text/javascript" src="/Componentes/js/Geoposicion.js"></script>
<script type="text/javascript" src="/Componentes/js/toolbarFiltros.js"></script>

<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteIDComponente" />
<ext:Hidden ID="hdIDComponente" runat="server" />
<ext:Hidden ID="CurrentControl" runat="server" />
<ext:Hidden ID="hdControlURL" runat="server" />
<ext:Hidden ID="hdControlName" runat="server" />
<ext:Hidden ID="hdNombre" runat="server" />
<ext:Hidden ID="hdFiltrosAplicados" runat="server" />
<ext:hidden ID="hdTotalCountGrid" runat="server" >
    <Listeners>
        <Change Fn="setNumMaxPageContactos"></Change>
    </Listeners>
</ext:hidden>

<ext:Container ID="hugeCt"
    Hidden="true"
    runat="server"
    Layout="FitLayout"
    Cls="hugeMainCt" />

<ext:Window runat="server"
    ID="winGestionContacto"
    Resizable="false"
    Modal="true"
    Width="750px"
    Draggable="true"
    Hidden="true">
    <Content>
        <local:FormContactos ID="formAgregarEditarContacto"
            runat="server" />
    </Content>
    <Listeners>
        <AfterRender Handler="resizeWinForm" />
        <Hide Fn="cerrarWindow" />
    </Listeners>

</ext:Window>

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
        <AfterRender Handler="GridColHandler(this)"></AfterRender>
        <Resize Handler="GridColHandler(this)"></Resize>
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
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrillaContactos" />
                <DataChanged Fn="ajaxGetDatosBuscadorContactos" />
            </Listeners>
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
                    ID="btnAnadir"
                    Cls="btnAnadir"
                    AriaLabel="Añadir"
                    Hidden="true"
                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>">
                    <Listeners>
                        <Click Fn="agregarContacto" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnEditar"
                    Cls="btnEditar"
                    AriaLabel="Editar"
                    Disabled="true"
                    Hidden="true"
                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>">
                    <Listeners>
                        <Click Fn="editarContacto" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnEliminar"
                    Cls="btnEliminar"
                    AriaLabel="Eliminar"
                    Disabled="true"
                    Hidden="true"
                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>">
                    <Listeners>
                        <Click Fn="eliminarContacto" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnRefrescar"
                    Cls="btnRefrescar"
                    AriaLabel="Actualizar"
                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>">
                    <Listeners>
                        <Click Fn="refrescarContacto" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnDescargar"
                    Cls="btnDescargar"
                    AriaLabel="Descargar"
                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                    Handler="ExportarDatos('Emplazamientos',App.hdCliID.value, #{grdContactos}, '../../Componentes/GridEmplazamientosContactos.ascx', '',JSON.stringify({ items: filtrosAplicados, tab: 'Contactos' }), App.hdStringBuscador.value, App.hdIDEmplazamientoBuscador.value);">
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnDescargarEntidad"
                    Cls="btnDescargar"
                    Hidden="true"
                    AriaLabel="Descargar"
                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                    Handler="ExportarDatos('Entidades',App.hdCliID.value, #{grdContactos}, '../../Componentes/GridEmplazamientosContactos.ascx', '', 'ContactosEntidades');">
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnActivar"
                    Cls="btnActivar"
                    AriaLabel="Activar"
                    Disabled="true"
                    Hidden="true"
                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>">
                    <Listeners>
                        <Click Fn="activarContacto" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnFiltros"
                    Cls="btnFiltros"
                    EnableToggle="true"
                    ToolTip="<%$ Resources:Comun, strMostrarPanelFiltros %>"
                    Handler="hidePnFilters();" />
                <ext:Button runat="server"
                    ID="btnFiltros2"
                    Cls="btnFiltros"
                    EnableToggle="true"
                    Hidden="true"
                    AriaLabel="Mostrar Filtros"
                    ToolTip="Mostrar Panel Filtros"
                    Handler="showPnAsideR(this.id);">
                    <Listeners>
                        <Click Fn="mostrarFiltros" />
                    </Listeners>
                </ext:Button>
            </Items>
        </ext:Toolbar>
        <ext:Container runat="server"
            ID="tblfiltros"
            Cls=""
            Dock="Top">
            <Content>
                <local:toolbarFiltros
                    ID="cmpFiltroContactos"
                    runat="server"
                    Stores="storeContactosGlobalesEmplazamientos"
                    MostrarComboFecha="false"
                    FechaDefecto="Dia"
                    QuitarFiltros="true"
                    Grid="grdContactos"
                    MostrarBusqueda="false" />
            </Content>
        </ext:Container>
        <ext:Toolbar
            runat="server"
            ID="tbFiltros"
            Cls="tlbGrid"
            Hidden="true"
            Layout="ColumnLayout">
            <Items>

                <ext:TextField
                    ID="txtSearchContactos"
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
                        <Change Fn="filtrarEmplazamientosPorBuscador" Buffer="500" />
                        <TriggerClick Fn="LimpiarFiltroBusquedaContactos" />
                    </Listeners>
                </ext:TextField>
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
                MinWidth="120" />
            <ext:Column
                meta:resourceKey="strCodigo"
                ID="colCodigoEntidad"
                runat="server"
                Flex="1"
                Text="<%$ Resources:Comun, strCodigo %>"
                DataIndex="CodigoEntidad"
                MinWidth="120" />
            <ext:Column
                meta:resourceKey="strNombreSitio"
                ID="colNombreSitio"
                runat="server"
                Flex="1"
                Text="<%$ Resources:Comun, strNombreSitio %>"
                DataIndex="NombreSitio"
                MinWidth="120" />
            <ext:Column
                meta:resourceKey="strNombreEntidad"
                ID="colNombreEntidad"
                runat="server"
                Flex="1"
                Text="<%$ Resources:Comun, strNombreEntidad %>"
                DataIndex="NombreEntidad"
                MinWidth="120" />
            <ext:Column
                runat="server"
                ID="colNombre"
                Text="<%$ Resources:Comun, strNombre %>"
                DataIndex="Nombre"
                MinWidth="120"
                Flex="1" />
            <ext:Column runat="server"
                ID="colApellidos"
                DataIndex="Apellidos"
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
                Flex="1" />
            <ext:Column
                ID="colEmail"
                runat="server"
                MinWidth="120"
                Text="<%$ Resources:Comun, strEmail %>"
                DataIndex="Email"
                Flex="1" />
            <ext:Column
                ID="colProvincia"
                runat="server"
                MinWidth="120"
                Text="<%$ Resources:Comun, strProvincia %>"
                DataIndex="Provincia"
                Flex="1" />
            <ext:Column
                ID="colPais"
                runat="server"
                MinWidth="120"
                Text="<%$ Resources:Comun, strPais %>"
                DataIndex="Pais"
                Flex="1" />
            <ext:Column
                ID="colTipoContacto"
                runat="server"
                MinWidth="120"
                Text="<%$ Resources:Comun, strTipo %>"
                DataIndex="ContactoTipo"
                Flex="1" />
            <ext:WidgetColumn ID="ColMore"
                runat="server"
                Cls="col-More"
                DataIndex=""
                Align="Center"
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
                            <Click Handler="parent.hideAsideREntidad('panelMore');" />
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

        <ext:Toolbar ID="pagin" runat="server" >
            <Items>
                <ext:Button runat="server"
                    ID="btnPagingInit"
                    Disabled="true"
                    IconCls="x-tbar-page-first">
                    <Listeners>
                        <Click Fn="pagingInitContactos" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnPagingPre"
                    Disabled="true"
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
                    IconCls="x-tbar-page-next">
                    <Listeners>
                        <Click Fn="paginNextContactos" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnPaginLast"
                    Enabled="false"
                    IconCls="x-tbar-page-last">
                    <Listeners>
                        <Click Fn="paginLastContactos" />
                    </Listeners>
                </ext:Button>
                <ext:ToolbarSeparator />

                <ext:ComboBox runat="server"
                    Cls="comboGrid"
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
    <Listeners>
        <BeforeRender Fn="CargarGridContactos"></BeforeRender>
    </Listeners>
</ext:GridPanel>
