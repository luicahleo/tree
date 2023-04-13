<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridEmplazamientosContactosResp.ascx.cs" Inherits="TreeCore.Componentes.GridEmplazamientosContactosResp" %>

<%@ Register Src="~/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>
<%@ Register Src="/Componentes/FormContactos.ascx" TagName="FormContactos" TagPrefix="local" %>

<script src="//maps.googleapis.com/maps/api/js?key=AIzaSyA0k02bRAAmDy-4x0kcN06k1auhLHKGRZs&v=3&libraries=places" type="text/javascript"></script>
<link href="/CSS/tCore.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="/PaginasComunes/js/Ext.ux.Map.js"></script>
<script type="text/javascript" src="/PaginasComunes/js/Ext.ux.GMapPanel.js"></script>
<script type="text/javascript" src="/Componentes/js/FormContactos.js"></script>
<script type="text/javascript" src="/Componentes/js/Geoposicion.js"></script>

<%--INICIO HIDDEN --%>

<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteIDComponente" />
<ext:Hidden ID="hdIDComponente" runat="server" />
<ext:Hidden ID="CurrentControl" runat="server" />
<ext:Hidden ID="hdControlURL" runat="server" />
<ext:Hidden ID="hdControlName" runat="server" />
<ext:Hidden ID="hdNombre" runat="server" />

<%--FIN HIDDEN --%>

<ext:Container ID="hugeCt"
    Hidden="true"
    runat="server"
    Layout="FitLayout"
    Cls="hugeMainCt" />

<%--INICIO WINDOW --%>

<ext:Window runat="server"
    ID="winGestionContacto"
    Title="Add Contact"
    Modal="true"
    Width="750"
    Height="600"
    Hidden="true" 
    Closable="false"
    Layout="FitLayout"
    Cls="winForm-resp winContacto">
    <Content>
        <local:FormContactos ID="formAgregarEditarContacto"
            runat="server" />
    </Content>
    <Listeners>
        <AfterRender Handler="winResiz(this)"></AfterRender>
        <%--<BeforeHide Fn="cerrarWindow" />--%>
    </Listeners>

</ext:Window>

<%-- FIN WINDOW --%>

<%--INICIO GRID --%>

<ext:GridPanel
    runat="server"
    ID="grdContactos"
    SelectionMemory="false"
    Header="false"
    AriaRole="main"
    Scrollable="Vertical"
    EnableColumnHide="false"
    Cls="gridPanel">
    <Listeners>
        <AfterRender Fn="recargarStore" />
        <Resize Handler="GridColHandler(this)"></Resize>
    </Listeners>
    <Store>
        <ext:Store runat="server"
            ID="storeContactosGlobalesEmplazamientos"
            AutoLoad="false"
            OnReadData="storeContactosGlobalesEmplazamientos_Refresh"
            RemotePaging="false"
            RemoteSort="true"
            PageSize="20"
            shearchBox="cmpFiltro_txtSearch"
            listNotPredictive="EmplazamientoID,EntidadID,ContactoGlobalID,NombreEntidad,Telefono2,Direccion,CP,MunicipioID,Region,CodigoEntidad,Activo,Comentarios">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrillaContactos" />
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
            <Listeners>
                <DataChanged Fn="BuscadorPredictivo" />
            </Listeners>
        </ext:Store>
    </Store>
    <DockedItems>
        <ext:Toolbar runat="server"
            ID="tlbBase"
            Dock="Top"
            Cls="tlbGrid"
            OverflowHandler="Scroller">
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
                    Handler="ExportarDatosSinCliente('Contactos', #{grdContactos}, '', '', #{hdNombre}.value);">
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
            ID="tbfiltros" 
            Cls="" 
            Dock="Top">
            <Content>
                <local:toolbarFiltros
                    ID="cmpFiltro"
                    runat="server"
                    Stores="storeContactosGlobalesEmplazamientos"
                    MostrarComboFecha="false"
                    FechaDefecto="Dia"
                    Grid="grdContactos"
                    MostrarBusqueda="false" />
            </Content>
        </ext:Container>

        <ext:PagingToolbar runat="server"
            StoreID="storeContactosGlobalesEmplazamientos"
            ID="PagingToolbar"
            HideRefresh="true" 
            Dock="Bottom"
            DisplayInfo="true">
            <Items>
                <ext:ComboBox runat="server"
                    Cls="comboGrid"
                    ID="cmbRegistros"
                    Width="80">
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
    </DockedItems>
    <ColumnModel runat="server">
        <Columns>
            <ext:Column runat="server"
                ID="colActivo"
                DataIndex="Activo"
                Align="Center"
                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                Cls="col-activo"
                MinWidth="120">
                <Renderer Fn="DefectoRender" />
            </ext:Column>
            <ext:Column
                meta:resourceKey="strCodigo"
                ID="colCodigo"
                runat="server"
                Flex="8"
                Text="<%$ Resources:Comun, strCodigo %>"
                DataIndex="Codigo"
                MinWidth="120" />
            <ext:Column
                meta:resourceKey="strCodigo"
                ID="colCodigoEntidad"
                runat="server"
                Flex="8"
                Text="<%$ Resources:Comun, strCodigo %>"
                DataIndex="CodigoEntidad"
                MinWidth="120" />
            <ext:Column
                meta:resourceKey="strNombreSitio"
                ID="colNombreSitio"
                Flex="8"
                runat="server"
                Text="<%$ Resources:Comun, strNombreSitio %>"
                DataIndex="NombreSitio"
                MinWidth="120" />
            <ext:Column
                meta:resourceKey="strNombreEntidad"
                ID="colNombreEntidad"
                Flex="8"
                runat="server"
                Text="<%$ Resources:Comun, strNombreEntidad %>"
                DataIndex="NombreEntidad"
                MinWidth="120" />
            <ext:Column
                runat="server"
                ID="colNombre"
                Flex="8"
                Text="<%$ Resources:Comun, strNombre %>"
                DataIndex="Nombre"
                MinWidth="120" />
            <ext:Column runat="server"
                ID="colApellidos"
                DataIndex="Apellidos"
                Text="<%$ Resources:Comun, strApellidos %>"
                MinWidth="120"
                Flex="8">
            </ext:Column>
            <ext:Column
                runat="server"
                ID="colTelefono"
                Text="<%$ Resources:Comun, strTelefono %>"
                DataIndex="Telefono"
                MinWidth="120"
                Flex="8" />
            <ext:Column
                ID="colEmail"
                runat="server"
                Text="<%$ Resources:Comun, strEmail %>"
                DataIndex="Email"
                MinWidth="120"
                Flex="8" />
            <ext:Column
                ID="colProvincia"
                runat="server"
                Text="<%$ Resources:Comun, strProvincia %>"
                DataIndex="Provincia"
                MinWidth="120"
                Flex="8" />
            <ext:Column
                ID="colPais"
                runat="server"
                Text="<%$ Resources:Comun, strPais %>"
                DataIndex="Pais"
                MinWidth="120"
                Flex="8" />
            <ext:Column
                ID="colTipoContacto"
                runat="server"
                MinWidth="120"
                Text="<%$ Resources:Comun, strTipo %>"
                DataIndex="ContactoTipo"
                Flex="8" />
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
</ext:GridPanel>

<%--FIN GRID --%>

