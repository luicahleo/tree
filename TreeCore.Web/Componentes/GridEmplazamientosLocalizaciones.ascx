<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridEmplazamientosLocalizaciones.ascx.cs" Inherits="TreeCore.Componentes.GridEmplazamientosLocalizaciones" %>

<%@ Register Src="/Componentes/Geoposicion.ascx" TagName="Geoposicion" TagPrefix="local" %>

<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteIDComponente" />
<ext:Hidden ID="hdLatitudForm" runat="server" />
<ext:Hidden ID="hdLongitudForm" runat="server" />
<ext:Hidden ID="hdZoom" runat="server" />
<ext:Hidden ID="hdIDComponente" runat="server" />
<ext:Hidden ID="hdFiltrosAplicados" runat="server" />
<ext:hidden ID="hdTotalCountGrid" runat="server" >
    <Listeners>
        <Change Fn="setNumMaxPageLocalizaciones"></Change>
    </Listeners>
</ext:hidden>

<ext:Window runat="server"
    Hidden="true"
    ID="contPosicion">
    <Content>
        <local:Geoposicion runat="server"
            ID="geoPosicion"
            Localizacion="true"
            NombreMapa="mapLocalizaciones" />
    </Content>
</ext:Window>


<ext:GridPanel
    runat="server"
    OverflowX="Auto"
    ID="gridEmplazamientosLocalizaciones"
    EnableColumnHide="false"
    Scrollable="Vertical"
    Cls="gridPanel grdNoHeader">
    <Store>
        <ext:Store
            runat="server"
            ID="storeEmplazamientosLocalizaciones"
            AutoLoad="false"
            OnReadData="storeEmplazamientosLocalizaciones_Refresh"
            RemotePaging="true"
            RemoteSort="true"
            RemoteFilter="true"
            PageSize="20">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="EmplazamientoID">
                    <Fields>
                        <ext:ModelField Name="Codigo" Type="String" />
                        <ext:ModelField Name="NombreSitio" Type="String" />
                        <ext:ModelField Name="CodigoPostal" Type="String" />
                        <ext:ModelField Name="Region" Type="String" />
                        <ext:ModelField Name="Pais" Type="String" />
                        <ext:ModelField Name="Provincia" Type="String" />
                        <ext:ModelField Name="Municipio" Type="String" />
                        <ext:ModelField Name="Direccion" Type="String" />
                        <ext:ModelField Name="Longitud" Type="Float" />
                        <ext:ModelField Name="Latitud" Type="Float" />
                        <ext:ModelField Name="RegionPais" Type="String" />
                        <ext:ModelField Name="Geoposicion" Type="String" />
                        <ext:ModelField Name="Barrio" Type="String" />
                    </Fields>
                </ext:Model>
            </Model>
            <Listeners>
                <DataChanged Fn="ajaxGetDatosBuscadorLocalizaciones" />
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
                        <Click Fn="refrescarLocalizaciones" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnDescargar"
                    Cls="btnDescargar"
                    AriaLabel="Descargar"
                    Hidden="false"
                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                    Handler="ExportarDatos('Emplazamientos',App.hdCliID.value, #{gridEmplazamientosLocalizaciones}, '../../Componentes/GridEmplazamientosLocalizaciones.ascx', '',JSON.stringify({ items: filtrosAplicados, tab: 'Localizaciones' }), App.hdStringBuscador.value, App.hdIDEmplazamientoBuscador.value);">
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
                    EmptyText="<%$ Resources:Comun, strBuscar %>"
                    LabelWidth="50"
                    Width="250"
                    EnableKeyEvents="true">
                    <Triggers>
                        <ext:FieldTrigger Icon="Clear" />
                    </Triggers>
                    <Listeners>
                        <Change Fn="filtrarEmplazamientosPorBuscador" Buffer="500" />
                        <TriggerClick Fn="LimpiarFiltroBusqueda" />
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
                        <ext:Button runat="server" Width="30" ID="btnClearFilters" Cls="btn-trans btnRemoveFilters" AriaLabel="Quitar Filtros" ToolTip="<%$ Resources:Comun, strQuitarFiltro %>">
                            <Listeners>
                                <Click Fn="BorrarFiltrosLozalizaciones"></Click>
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
            <ext:Column
                meta:resourceKey="strCodigo"
                ID="colCodigo"
                runat="server"
                Text="<%$ Resources:Comun, strCodigo %>"
                DataIndex="Codigo"
                MaxWidth="250" />
            <ext:Column
                meta:resourceKey="strNombreSitio"
                ID="colNombreSitio"
                runat="server"
                Text="<%$ Resources:Comun, strNombreSitio %>"
                DataIndex="NombreSitio"
                MaxWidth="250" />
            <ext:Column
                ID="colLongitud"
                runat="server"
                Hidden="true"
                Text="<%$ Resources:Comun, strLongitud %>"
                DataIndex="Longitud"
                MaxWidth="250"
                Flex="1" />
            <ext:Column
                ID="colLatitud"
                runat="server"
                Hidden="true"
                Text="<%$ Resources:Comun, strLatitud %>"
                DataIndex="Latitud"
                MaxWidth="250"
                Flex="1" />
            <ext:Column runat="server"
                ID="colMap"
                MaxWidth="250"
                Text="<%$ Resources:Comun, strLocalizacion %>"
                DataIndex="Geoposicion"
                Flex="1">
                <Commands>
                    <ext:ImageCommand CommandName="<%$ Resources:Comun, strMapa %>"
                        IconCls="ico-geolocalizacion-gr">
                    </ext:ImageCommand>
                </Commands>
                <Listeners>
                    <Command Fn="showMap" />
                </Listeners>
            </ext:Column>
            <ext:Column
                ID="colDireccion"
                runat="server"
                Text="<%$ Resources:Comun, strDireccion %>"
                DataIndex="Direccion"
                MaxWidth="250"
                Flex="1" />
            <ext:Column
                ID="colCodigoPostal"
                runat="server"
                Text="<%$ Resources:Comun, strCodigoPostal %>"
                DataIndex="CodigoPostal"
                MaxWidth="250"
                Flex="1" />
            <ext:Column
                ID="colMunicipio"
                runat="server"
                Text="<%$ Resources:Comun, strMunicipio %>"
                DataIndex="Municipio"
                MaxWidth="250"
                Flex="1" />
            <ext:Column
                ID="colBarrio"
                runat="server"
                Text="<%$ Resources:Comun, strBarrio %>"
                DataIndex="Barrio"
                MaxWidth="250"
                Flex="1" />
            <ext:Column
                ID="colProvincia"
                runat="server"
                Text="<%$ Resources:Comun, strProvincia %>"
                DataIndex="Provincia"
                MaxWidth="250"
                Flex="1" />
            <ext:Column
                ID="colRegionPais"
                runat="server"
                Text="<%$ Resources:Comun, strRegionPais %>"
                DataIndex="RegionPais"
                MaxWidth="250"
                Flex="1" />
            <ext:Column
                ID="colPais"
                runat="server"
                Text="<%$ Resources:Comun, strPais %>"
                DataIndex="Pais"
                MaxWidth="250"
                Flex="1" />
            <ext:Column
                ID="colRegion"
                runat="server"
                Text="<%$ Resources:Comun, strRegion %>"
                DataIndex="Region"
                MaxWidth="250"
                Flex="1" />
        </Columns>
    </ColumnModel>
    <SelectionModel>
        <ext:CheckboxSelectionModel runat="server"
            ID="GridRowSelectLocalizaciones"
            Mode="Multi">
            <Listeners>
                <Select Fn="Grid_RowSelectLocalizaciones" />
            </Listeners>
        </ext:CheckboxSelectionModel>
    </SelectionModel>
    <ViewConfig StripeRows="true" />
    <Plugins>
        <ext:GridFilters runat="server"
            ID="gridFilters"
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
                        <Click Fn="pagingInitLocalizaciones" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnPagingPre"
                    Disabled="true"
                    IconCls="x-tbar-page-prev">
                    <Listeners>
                        <Click Fn="pagingPreLocalizaciones" />
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
                        <BeforeRender Fn="nfPaginNumberBeforeRenderLocalizaciones" />
                        <Change Fn="paginGoToLocalizaciones" />
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
                        <Click Fn="paginNextLocalizaciones" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnPaginLast"
                    Enabled="false"
                    IconCls="x-tbar-page-last">
                    <Listeners>
                        <Click Fn="paginLastLocalizaciones" />
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
                        <Select Fn="pageSelectLocalizaciones" />
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
        <BeforeRender Fn="CargarGridLocalizaciones"></BeforeRender>
    </Listeners>
</ext:GridPanel>
