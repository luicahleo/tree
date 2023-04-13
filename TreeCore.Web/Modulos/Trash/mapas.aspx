<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mapas.aspx.cs" Inherits="TreeCore.PaginasComunes.Mapas" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="css/styleMaps.css" rel="stylesheet" type="text/css" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" />
    <script type="text/javascript" src="js/Mapas.js"></script>
    <%--    <script src="//maps.googleapis.com/maps/api/js?key=AIzaSyA0k02bRAAmDy-4x0kcN06k1auhLHKGRZs&v=3" type="text/javascript"></script>--%>

    <script src="//maps.googleapis.com/maps/api/js?key=AIzaSyA0k02bRAAmDy-4x0kcN06k1auhLHKGRZs&v=3&libraries=places" type="text/javascript"></script>

    <script type="text/javascript" src="js/Ext.ux.Map.js"></script>
    <script src="https://unpkg.com/@google/markerclustererplus@4.0.1/dist/markerclustererplus.min.js"></script>
    <link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet" />
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />

    <form id="form1" runat="server">
        <div>
           <%--INICIO HIDDEN --%>

            <ext:Hidden runat="server"
                ID="hdCliID"
                Name="hdCliID" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%> 

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <DocumentReady Handler="initMap();" />
                </Listeners>
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>
            <ext:Store runat="server" ID="storeEmplazamientos" AutoLoad="true" OnReadData="storeEmplazamientosRefresh" RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="EmplazamientoID">
                        <Fields>
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="NombreSitio" />
                            <ext:ModelField Name="Operador" />
                            <ext:ModelField Name="Tipo" />
                            <ext:ModelField Name="CategoriaSitio" />
                            <ext:ModelField Name="TipoEdificio" />
                            <ext:ModelField Name="EstadoGlobal" />
                            <ext:ModelField Name="Tamano" />
                            <ext:ModelField Name="Region" />
                            <ext:ModelField Name="Pais" />
                            <ext:ModelField Name="RegionPais" />
                            <ext:ModelField Name="Provincia" />
                            <ext:ModelField Name="Municipio" />
                            <ext:ModelField Name="Barrio" />
                            <ext:ModelField Name="Direccion" />
                            <ext:ModelField Name="CodigoPostal" />
                            <ext:ModelField Name="Latitud" Type="Float" />
                            <ext:ModelField Name="Longitud" Type="Float" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Parameters>
                    <ext:StoreParameter Name="Operadores" Mode="Raw" Value="getValueOperadores()"></ext:StoreParameter>
                    <ext:StoreParameter Name="EstadosGlobales" Mode="Raw" Value="getValueEstadosGlobales()"></ext:StoreParameter>
                    <ext:StoreParameter Name="CategoriasSitios" Mode="Raw" Value="getValueCategoriasSitios()"></ext:StoreParameter>
                    <ext:StoreParameter Name="TiposEmplazamientos" Mode="Raw" Value="getValueTiposEmplazamientos()"></ext:StoreParameter>
                    <ext:StoreParameter Name="Tamanos" Mode="Raw" Value="getValueTamanos()"></ext:StoreParameter>
                </Parameters>
                <Sorters>
                    <ext:DataSorter Property="EmplazamientoID" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <Load Handler="store_load();" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server" ID="storeClientes" AutoLoad="true" OnReadData="storeClientesRefresh" RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ClienteID">
                        <Fields>
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="Cliente" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Cliente" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server" ID="storeOperadores" AutoLoad="true" OnReadData="storeOperadoresRefresh" RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="OperadorID">
                        <Fields>
                            <ext:ModelField Name="OperadorID" Type="Int" />
                            <ext:ModelField Name="Operador" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Operador" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server" ID="storeEstadosGlobales" AutoLoad="true" OnReadData="storeEstadosGlobalesRefresh" RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="EstadoGlobalID">
                        <Fields>
                            <ext:ModelField Name="EstadoGlobalID" Type="Int" />
                            <ext:ModelField Name="EstadoGlobal" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="EstadoGlobal" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server" ID="storeCategoriasSitios" AutoLoad="true" OnReadData="storeCategoriasSitiosRefresh" RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="EmplazamientoCategoriaSitioID">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoCategoriaSitioID" Type="Int" />
                            <ext:ModelField Name="CategoriaSitio" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="CategoriaSitio" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server" ID="storeEmplazamientosTipos" AutoLoad="true" OnReadData="storeEmplazamientosTiposRefresh" RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="EmplazamientoTipoID">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTipoID" Type="Int" />
                            <ext:ModelField Name="Tipo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Tipo" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server" ID="storeEmplazamientosTamanos" AutoLoad="true" OnReadData="storeEmplazamientosTamanosRefresh" RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="EmplazamientoTamanoID">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTamanoID" Type="Int" />
                            <ext:ModelField Name="Tamano" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Tamano" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeClusters" runat="server" AutoLoad="true" OnReadData="storeClustersRefresh" RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server">
                        <Fields>
                            <ext:ModelField Name="Tipo" />
                            <ext:ModelField Name="zoom" Type="Int" />
                            <ext:ModelField Name="size" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <%--FIN  STORES --%>
            <%--INICIO  WINDOWS --%>
            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>
            <ext:Viewport runat="server" Layout="Border">
                <Items>

                    <ext:FormPanel ID="pnFiltersMap" runat="server" Cls="floatingPn" IconCls="ico-filter" Width="180" Title="Filters" Collapsible="true" Collapsed="true" Hidden="false">
                        <Items>
                            <ext:NumberField runat="server"
                                ID="numRadio"
                                meta:resourceKey="numRadio"
                                FieldLabel="Radio (Km)"
                                LabelAlign="Top"
                                Width="160"
                                AllowDecimals="false"
                                Number="10"
                                MaxValue="25" />
                            <ext:ComboBox runat="server"
                                ID="cmbClusters"
                                meta:resourceKey="cmbClusters"
                                FieldLabel="Cluster"
                                LabelAlign="Top"
                                Width="160"
                                Editable="false"
                                StoreID="storeClusters"
                                DisplayField="Tipo"
                                ValueField="Tipo"
                                QueryMode="Local">
                            </ext:ComboBox>
                            <ext:ComboBox runat="server"
                                ID="cmbClientes"
                                meta:resourceKey="cmbClientes"
                                FieldLabel="Clientes"
                                LabelAlign="Top"
                                Width="160"
                                QueryMode="Local"
                                Editable="false"
                                StoreID="storeClientes"
                                DisplayField="Cliente"
                                ValueField="ClienteID">
                                <Listeners>
                                    <Select Handler="SeleccionarCliente();" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:MultiCombo runat="server"
                                ID="multiOperadores"
                                meta:resourceKey="multiOperadores"
                                FieldLabel="Operadores"
                                LabelAlign="Top"
                                Width="160"
                                QueryMode="Remote"
                                Editable="false"
                                SelectionMode="Selection"
                                StoreID="storeOperadores"
                                DisplayField="Operador"
                                ValueField="OperadorID" />
                            <ext:MultiCombo runat="server"
                                ID="multiEstadosGlobales"
                                meta:resourceKey="multiEstadosGlobales"
                                FieldLabel="Estados Globales"
                                LabelAlign="Top"
                                Width="160"
                                QueryMode="Local"
                                Editable="false"
                                SelectionMode="Selection"
                                StoreID="storeEstadosGlobales"
                                DisplayField="EstadoGlobal"
                                ValueField="EstadoGlobalID" />
                            <ext:MultiCombo runat="server"
                                ID="multiCategoriasSitios"
                                meta:resourceKey="multiCategoriasSitios"
                                FieldLabel="Categorias Sitios"
                                LabelAlign="Top"
                                Width="160"
                                QueryMode="Local"
                                Editable="false"
                                SelectionMode="Selection"
                                StoreID="storeCategoriasSitios"
                                DisplayField="CategoriaSitio"
                                ValueField="EmplazamientoCategoriaSitioID" />
                            <ext:MultiCombo runat="server"
                                ID="multiEmplazamientosTipos"
                                meta:resourceKey="multiEmplazamientosTipos"
                                FieldLabel="Emplazamientos Tipos"
                                LabelAlign="Top"
                                Width="160"
                                QueryMode="Local"
                                Editable="false"
                                SelectionMode="Selection"
                                StoreID="storeEmplazamientosTipos"
                                DisplayField="Tipo"
                                ValueField="EmplazamientoTipoID" />
                            <ext:MultiCombo runat="server"
                                ID="multiEmplazamientosTamanos"
                                meta:resourceKey="multiEmplazamientosTamanos"
                                FieldLabel="Emplazamientos Tamaños"
                                LabelAlign="Top"
                                Width="160"
                                QueryMode="Local"
                                Editable="false"
                                SelectionMode="Selection"
                                StoreID="storeEmplazamientosTamanos"
                                DisplayField="Tamano"
                                ValueField="EmplazamientoTamanoID" />
                            <ext:Button runat="server"
                                ID="btnAplicar"
                                meta:resourceKey="btnAplicar"
                                Text="Aplicar">
                                <Listeners>
                                    <Click Handler="App.storeEmplazamientos.reload();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:FormPanel>
                    <ext:Container runat="server" ID="map">
                        <TabMenu>
                            <ext:Menu runat="server"
                                ID="ContextMenu">
                                <Items>

                                    <ext:ActionRef runat="server" Action="#{ShowSitesAction}" />
                                    <ext:ActionRef runat="server" Action="#{ShowMapAction}" />
                                    <ext:ActionRef runat="server" Action="#{ShowTrackingAction}" />
                                </Items>
                            </ext:Menu>
                        </TabMenu>

                    </ext:Container>
                </Items>
            </ext:Viewport>
            <%--FIN  VIEWPORT --%>


            <%--  //AQUI HAY QUE LINKAR LA API GOOGLE SEARCHBOX--%>
            <input
                id="Input-Search"
                class="FloatingSearchBox"
                type="text"
                placeholder="Search Address" />

        </div>
    </form>

    <ext:Action
        runat="server"
        ID="ShowSitesAction"
        IconCls="ico-sites"
        Text="Emplazamientos"
        Disabled="true"
         />

    <ext:Action
        runat="server"
        ID="ShowMapAction"
        IconCls="ico-map-sites"
        Text="Cargar Mapa"
        Disabled="true"
         />

    <ext:Action
        ID="ShowTrackingAction"
        runat="server"
        IconCls="ico-seguimiento"
        Text="Seguimiento"
        Disabled="true"
         />

    


</body>




</html>
