<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridEmplazamientosDocumentos.ascx.cs" Inherits="TreeCore.Componentes.GridEmplazamientosDocumentos" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>
<%--<%@ Register Src="/Componentes/FormContactos.ascx" TagName="FormContactos" TagPrefix="local" %>--%>

<link href="/Componentes/css/GridEmplazamientos.css" rel="stylesheet" type="text/css" />
<%--<script type="text/javascript" src="../../Componentes/js/FormContactos.js"></script>--%>

<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteIDComponente" />
<ext:Hidden ID="hdIDComponente" runat="server" />
<ext:Hidden ID="CurrentControl" runat="server" />
<ext:Hidden ID="hdControlURL" runat="server" />
<ext:Hidden ID="hdControlName" runat="server" />
<ext:Hidden ID="hdFiltrosAplicados" runat="server" />
<ext:hidden ID="hdTotalCountGrid" runat="server" >
    <Listeners>
        <Change Fn="setNumMaxPageDocumentos"></Change>
    </Listeners>
</ext:hidden>

<ext:Container ID="hugeCt"
    Hidden="true"
    runat="server"
    Layout="FitLayout"
    Cls="hugeMainCt" />

<ext:GridPanel
    runat="server"
    ID="grdDocumentosEmplazamientos"
    OverflowX="Auto"
    Scrollable="Vertical"
    EnableColumnHide="false"
    Cls="gridPanel grdNoHeader">
    <Store>
        <ext:Store runat="server"
            ID="storeDocumentosEmplazamientos"
            AutoLoad="false"
            OnReadData="storeDocumentosEmplazamientos_Refresh"
            RemotePaging="true"
            RemoteSort="true"
            RemoteFilter="true"
            PageSize="20"
            shearchBox="txtSearch"
            listNotPredictive="DocumentoID,UltimaVersion,EmplazamientoID,ProyectoID,Proyecto,CreadorID,Extension,Activo,Observaciones,DocumentoTipoID,DocumentSeguimientoID,Fecha,DocumentEstadoID,EmplazamientoID">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrillaDocumentos" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="DocumentoID">
                    <Fields>
                        <ext:ModelField Name="ProyectoID" Type="Int" />
                        <ext:ModelField Name="Proyecto" Type="String" />
                        <ext:ModelField Name="CreadorID" Type="Int" />
                        <ext:ModelField Name="NombreCompleto" Type="String" />
                        <ext:ModelField Name="Documento" Type="String" />
                        <ext:ModelField Name="Archivo" Type="String" />
                        <ext:ModelField Name="Extension" Type="String" />
                        <ext:ModelField Name="Observaciones" Type="String" />
                        <ext:ModelField Name="DocumentoTipoID" Type="Int" />
                        <ext:ModelField Name="DocumentTipo" Type="String" />
                        <ext:ModelField Name="DocumentSeguimientoID" Type="Int" />
                        <ext:ModelField Name="Fecha" Type="Date" />
                        <ext:ModelField Name="DocumentEstadoID" Type="Int" />
                        <ext:ModelField Name="DocumentEstado" Type="String" />
                        <ext:ModelField Name="EmplazamientoID" Type="Int" />
                        <ext:ModelField Name="Codigo" Type="String" />
                        <ext:ModelField Name="NombreSitio" Type="String" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter
                    Property="DocumentoID"
                    Direction="DESC" />
            </Sorters>
            <Listeners>
                <DataChanged Fn="ajaxGetDatosBuscadorDocumentos" />
            </Listeners>
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
                        <Click Fn="refrescarDocumentos" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnDescargar"
                    Cls="btnDescargar"
                    AriaLabel="Descargar"
                    Hidden="false"
                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                    Handler="ExportarDatos('Emplazamientos',App.hdCliID.value, #{grdDocumentosEmplazamientos}, '../../Componentes/GridEmplazamientosDocumentos.ascx', '',JSON.stringify({ items: filtrosAplicados, tab: 'Documentos' }), App.hdStringBuscador.value, App.hdIDEmplazamientoBuscador.value);">
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnFiltros"
                    Cls="btnFiltros"
                    EnableToggle="true"
                    ToolTip="<%$ Resources:Comun, strMostrarPanelFiltros %>"
                    Handler="hidePnFilters();" />
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
                        <Change Fn="filtrarEmplazamientosPorBuscador" Buffer="500" />
                        <TriggerClick Fn="LimpiarFiltroBusquedaDocumentos" />
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
                                <Click Fn="BorrarFiltrosDocumentos"></Click>
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
                ID="colCodigoEmplazamiento"
                DataIndex="Codigo"
                Text="<%$ Resources:Comun, strCodigoEmplazamiento %>"
                MinWidth="140"
                Flex="1">
            </ext:Column>
            <ext:Column
                ID="colNombreEmplazamiento"
                runat="server"
                Text="<%$ Resources:Comun, strNombre %>"
                DataIndex="NombreSitio"
                MinWidth="140"
                Flex="1" />
            <ext:Column
                ID="colTipoDocumento"
                runat="server"
                Text="<%$ Resources:Comun, strTipoDocumento %>"
                DataIndex="DocumentTipo"
                MinWidth="140"
                Flex="1" />
            <ext:Column
                ID="colArchivo"
                runat="server"
                Text="<%$ Resources:Comun, strDocumento %>"
                DataIndex="Documento"
                MinWidth="140"
                Flex="1" />
            <ext:Column
                ID="colExtension"
                runat="server"
                Text="<%$ Resources:Comun, strExtension %>"
                DataIndex="Extension"
                MinWidth="80" />
            <ext:Column
                ID="colCreador"
                runat="server"
                Text="<%$ Resources:Comun, strCreador %>"
                DataIndex="NombreCompleto"
                MinWidth="140" />
            <ext:DateColumn
                runat="server"
                Format="<%$ Resources:Comun, FormatFecha %>"
                ID="colFecha"
                Text="<%$ Resources:Comun, strFecha %>"
                DataIndex="Fecha"
                MinWidth="140" />
        </Columns>
    </ColumnModel>
    <SelectionModel>
        <ext:RowSelectionModel
            runat="server"
            ID="GridRowSelectDocumentos"
            Mode="Single">
            <Listeners>
                <Select Fn="Grid_RowSelectDocumentos" />
            </Listeners>
        </ext:RowSelectionModel>
    </SelectionModel>
    <ViewConfig StripeRows="true" />
    <Plugins>
        <ext:GridFilters runat="server"
            ID="gridFiltersDocumentos"
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
                        <Click Fn="pagingInitDocumentos" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnPagingPre"
                    Disabled="true"
                    IconCls="x-tbar-page-prev">
                    <Listeners>
                        <Click Fn="pagingPreDocumentos" />
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
                        <BeforeRender Fn="nfPaginNumberBeforeRenderDocumentos" />
                        <Change Fn="paginGoToDocumentos" />
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
                        <Click Fn="paginNextDocumentos" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnPaginLast"
                    Enabled="false"
                    IconCls="x-tbar-page-last">
                    <Listeners>
                        <Click Fn="paginLastDocumentos" />
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
                        <Select Fn="pageSelectDocumentos" />
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
        <BeforeRender Fn="CargarGridDocumentos"></BeforeRender>
    </Listeners>
</ext:GridPanel>
