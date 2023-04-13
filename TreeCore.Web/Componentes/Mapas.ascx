<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Mapas.ascx.cs" Inherits="TreeCore.Componentes.Mapas" %>


<link href="/CSS/tCore.css" rel="stylesheet" type="text/css" />


<%--INICIO HIDDEN --%>

<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteID" />
<ext:Hidden ID="hdLatitudMapa" runat="server" />
<ext:Hidden ID="hdLongitudMapa" runat="server" />
<ext:Hidden ID="hdEmplazamientoID" runat="server" />
<ext:Hidden ID="hdCercanos" runat="server" Name="hdCercanos" />
<ext:Hidden ID="hdZoom" runat="server" />

<%--FIN HIDDEN --%>

<%--INICIO  STORES --%>

<ext:Panel ID="pnMap" runat="server">
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
                        <RequestConfig>
                            <EventMask ShowMask="true" />
                        </RequestConfig>
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
                <Listeners>
                    <DataChanged Fn="initMap" />
                </Listeners>
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
            Collapsed="true"
            Hidden="false">
            <Items>
                <ext:TextField runat="server"
                    ID="numRadio"
                    FieldLabel="<%$ Resources:Comun, strRadio %>"
                    LabelAlign="Top"
                    InputType="Number"
                    Width="160"
                    AllowDecimals="false"
                    Text="10"
                    Regex="/^([1-9]|1[0-9]|2[0-5])$/"
                    MaxValue="25" 
                    ValidationGroup="FORM"/>
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
                        <Click Fn="RecargarEmplazamientos" />
                    </Listeners>
                </ext:Button>
            </Items>
            <Listeners>
                <ValidityChange Fn="validarFilterMap" />
            </Listeners>
        </ext:FormPanel>
    </Content>
</ext:Panel>
