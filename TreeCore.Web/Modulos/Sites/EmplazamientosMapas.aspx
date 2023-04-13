<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplazamientosMapas.aspx.cs" Inherits="TreeCore.ModGlobal.pages.EmplazamientosMapas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <script src="//maps.googleapis.com/maps/api/js?key=AIzaSyA0k02bRAAmDy-4x0kcN06k1auhLHKGRZs&v=3&libraries=places" type="text/javascript"></script>
            <script src="https://unpkg.com/@google/markerclustererplus@4.0.1/dist/markerclustererplus.min.js" type="text/javascript"></script>

            <%--RESOURCE MANAGER--%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" DisableViewState="true" ShowWarningOnAjaxFailure="false">
                <Listeners>
                    <AjaxRequestException Fn="winErrorTimeout" />
                    <DocumentReady Fn="bindParams" />
                </Listeners>
            </ext:ResourceManager>

            <%--HIDDEN VARS--%>
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdStringBuscador" runat="server" />
            <ext:Hidden ID="hdIDEmplazamientoBuscador" runat="server" />
            <ext:Hidden ID="hdFiltrosAplicados" runat="server" />
            <ext:Hidden ID="hdEmplazamientoSeleccionado" Name="hdEmplazamientoSeleccionado" runat="server" />
            <ext:Hidden ID="hdTotalCountGrid" runat="server" />
            <ext:Hidden ID="hdLatitudMapa" runat="server" />
            <ext:Hidden ID="hdLongitudMapa" runat="server" />
            <ext:Hidden ID="hdCercanos" runat="server" Name="hdCercanos" />
            <ext:Hidden ID="hdZoom" runat="server" />
            <ext:Hidden ID="hdBounds" runat="server" />
            <ext:Hidden ID="hdOperadoresConIcono" runat="server" />
            <ext:Hidden ID="hdpath" runat="server" />

            <%--VIEW PORT--%>
            <ext:Viewport
                runat="server"
                ID="vwContenedor"
                Cls=""
                Layout="Anchor">
                <Items>
                    <ext:Panel ID="pnMap"
                        runat="server"
                        AnchorVertical="100%"
                        AnchorHorizontal="100%">
                        <Content>
                            <div id="floating-panel"
                                style="position: absolute; z-index: 5; display: none" class="gm-ui-hover-effect">
                                <button
                                    type="button"
                                    onclick="toggleStreetView();"
                                    class="close-streetview">
                                </button>
                            </div>
                            <div id="pano" class="dvMap"></div>
                            <input
                                id="Input-Search"
                                class="FloatingSearchBox"
                                type="text" style="position: relative" />
                            <div id="map" class="dvMap">
                                <ext:Store runat="server"
                                    ID="storeEmplazamientos"
                                    AutoLoad="false"
                                    OnReadData="storeEmplazamientos_Refresh">
                                    <Proxy>
                                        <ext:PageProxy>
                                        </ext:PageProxy>
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
                                    <Sorters>
                                        <ext:DataSorter Property="EmplazamientoID" Direction="ASC" />
                                    </Sorters>
                                    <%--<Listeners>
                                        <DataChanged Fn="initMap" />
                                    </Listeners>--%>
                                </ext:Store>
                            </div>
                            <ext:Store runat="server"
                                ID="storeClusters"
                                AutoLoad="false"
                                OnReadData="storeClusters_Refresh">
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Model>
                                    <ext:Model runat="server">
                                        <Fields>
                                            <ext:ModelField Name="Tipo" />
                                            <ext:ModelField Name="Zoom" Type="Int" />
                                            <ext:ModelField Name="Size" Type="Int" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                            <ext:FormPanel runat="server"
                                Style="left: 600px; top: 10px;"
                                ID="pnFiltersMap"
                                Cls="floatingPn floatFilterPn"
                                IconCls="ico-filter"
                                Width="180"
                                Title="<%$ Resources:Comun, GridFilters.MenuFilterText %>"
                                Collapsible="true"
                                Collapsed="false"
                                Hidden="false">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="numRadio"
                                        FieldLabel="<%$ Resources:Comun, strRadio %>"
                                        LabelAlign="Top"
                                        InputType="Number"
                                        Width="160"
                                        Hidden="true"
                                        AllowDecimals="false"
                                        Text="10"
                                        Regex="/^([1-9]|1[0-9]|2[0-5])$/"
                                        MaxValue="25"
                                        ValidationGroup="FORM" />
                                    <ext:ComboBox runat="server"
                                        ID="cmbClusters"
                                        FieldLabel="<%$ Resources:Comun, strClusters %>"
                                        LabelAlign="Top"
                                        Width="160"
                                        Editable="false"
                                        StoreID="storeClusters"
                                        DisplayField="Tipo"
                                        ValueField="Tipo"
                                        QueryMode="Local">
                                    </ext:ComboBox>
                                    <ext:Button runat="server"
                                        ID="btnAplicar"
                                        Cls="btn-mini-ppal FloatR"
                                        Text="<%$ Resources:Comun, strAplicar %>">
                                        <Listeners>
                                            <Click Fn="CargarEmplazamientos" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                                <Listeners>
                                    <ValidityChange Fn="validarFilterMap" />
                                </Listeners>
                            </ext:FormPanel>
                        </Content>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
    <script type="text/javascript">
        Ext.Ajax.timeout = 120000;
        Ext.net.DirectEvent.timeout = 120000;
    </script>
</body>
</html>
