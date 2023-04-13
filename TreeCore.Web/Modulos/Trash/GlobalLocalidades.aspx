<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GlobalLocalidades.aspx.cs" Inherits="TreeCore.ModGlobal.GlobalLocalidades" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server" ID="ResourceManagerTreeCore" DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>
            <ext:Hidden ID="FormatType" runat="server" />
            <ext:Hidden ID="ModuloID" runat="server" />
            <ext:Hidden ID="hdPaisID" runat="server" />
            <ext:Hidden ID="hdRegionID" runat="server" />
            <ext:Hidden ID="hdRegionPaisID" runat="server" />
            <ext:Hidden ID="hdProvinciaID" runat="server" />
            <ext:Hidden ID="hdMunicipioID" runat="server" />
            <ext:Hidden ID="hdMunicipalidadID" runat="server" />
            <ext:Hidden ID="hdPartidoID" runat="server" />

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store runat="server" ID="storePrincipal" RemotePaging="false" AutoLoad="false" OnReadData="storePrincipal_Refresh" RemoteSort="true" PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                    <Load Fn="ActivaRadio" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="GlobalLocalidadID">
                        <Fields>

                            <ext:ModelField Name="GlobalLocalidadID" Type="Int" />
                            <ext:ModelField Name="Localidad" />
                            <ext:ModelField Name="GlobalPartidoID" Type="Int" />
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="LocalidadCentroProblado" />
                            <ext:ModelField Name="DDN" />
                            <ext:ModelField Name="Subalm" />
                            <ext:ModelField Name="DescSubalm" />
                            <ext:ModelField Name="CabeceraComercial" />
                            <ext:ModelField Name="RegionComercial" />
                            <ext:ModelField Name="SubregionComercial" />
                            <ext:ModelField Name="ZIC" />
                            <ext:ModelField Name="CabeceraOYM" />
                            <ext:ModelField Name="Zona" />
                            <ext:ModelField Name="DimIntegral" Type="Boolean" />
                            <ext:ModelField Name="CodigoEnacom" />
                            <ext:ModelField Name="Anio" />
                            <ext:ModelField Name="EsEstimado" Type="Boolean" />
                            <ext:ModelField Name="CantidadHabitantes" Type="Float" />
                            <ext:ModelField Name="NBI" />
                            <ext:ModelField Name="RangoEtario" Type="Float" />
                            <ext:ModelField Name="Latitud" Type="Float" />
                            <ext:ModelField Name="Longitud" Type="Float" />
                            <ext:ModelField Name="Radio" Type="Float" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Localidad" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server" ID="storeClientes" AutoLoad="true" OnReadData="storeClientes_Refresh" RemoteSort="false">
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
                <Listeners>
                    <Load Handler="CargarStores();" />
                </Listeners>
            </ext:Store>
            <ext:Store ID="storeRegiones" runat="server" AutoLoad="true" OnReadData="storeRegiones_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="RegionID">
                        <Fields>
                            <ext:ModelField Name="RegionID" Type="Int" />
                            <ext:ModelField Name="Region" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store ID="storePaises" runat="server" AutoLoad="true" OnReadData="storePaises_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="PaisID">
                        <Fields>
                            <ext:ModelField Name="PaisID" Type="Int" />
                            <ext:ModelField Name="Pais" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store ID="storeRegionesPaises" runat="server" AutoLoad="true" OnReadData="storeRegionesPaises_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="RegionPaisID">
                        <Fields>
                            <ext:ModelField Name="RegionPaisID" Type="Int" />
                            <ext:ModelField Name="RegionPais" />
                            <ext:ModelField Name="PaisID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <%--<Listeners>
                <Load Handler="CargarStores();" />
            </Listeners>--%>
            </ext:Store>

            <ext:Store ID="storeProvincias" runat="server" AutoLoad="true" OnReadData="storeProvincias_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ProvinciaID">
                        <Fields>
                            <ext:ModelField Name="ProvinciaID" Type="Int" />
                            <ext:ModelField Name="Provincia" />
                            <ext:ModelField Name="PaisID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <%--<Listeners>
                <Load Handler="CargarStores();" />
            </Listeners>--%>
            </ext:Store>

            <ext:Store ID="storeMunicipios" runat="server" AutoLoad="true" OnReadData="storeMunicipios_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="MunicipioID">
                        <Fields>
                            <ext:ModelField Name="MunicipioID" Type="Int" />
                            <ext:ModelField Name="Municipio" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store ID="storePartidos" runat="server" AutoLoad="true" OnReadData="storePartidos_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="GlobalPartidoID">
                        <Fields>
                            <ext:ModelField Name="GlobalPartidoID" Type="Int" />
                            <ext:ModelField Name="Partido" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store ID="storeMunicipalidad" runat="server" AutoLoad="true" OnReadData="storeMunicipalidad_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="GlobalMunicipalidadID">
                        <Fields>
                            <ext:ModelField Name="GlobalMunicipalidadID" Type="Int" />
                            <ext:ModelField Name="Municipalidad" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store ID="storeAreasOYM" runat="server" AutoLoad="false" OnReadData="storeAreasOYM_Refresh"
                RemoteSort="false" RemotePaging="false" PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="LocalidadAreaOYMID">
                        <Fields>
                            <ext:ModelField Name="LocalidadAreaOYMID" Type="Int" />
                            <ext:ModelField Name="AreaOYM" Type="String" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Parameters>
                    <ext:StoreParameter Name="start" Value="0" Mode="Raw" />
                    <ext:StoreParameter Name="limit" Mode="Raw" Value="25" />
                </Parameters>
            </ext:Store>

            <ext:Store ID="storeAreasOYMLibres" runat="server" AutoLoad="false" OnReadData="storeAreasOYMLibres_Refresh"
                RemoteSort="false" RemotePaging="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="GlobalAreaOYMID">
                        <Fields>
                            <ext:ModelField Name="GlobalAreaOYMID" Type="Int" />
                            <ext:ModelField Name="AreaOYM" Type="String" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>


            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="450"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextField
                                ID="txtLocalidad"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strLocalidad %>"
                                Text=""
                                MaxLength="500"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField ID="txtCodigo"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                Text=""
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                EnforceMaxLength="true" />
                            <ext:TextField ID="txtLocalidadCentroPoblado"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strLocalidadCentroPoblado %>"
                                Text=""
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                EnforceMaxLength="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <%--<ext:NumberField runat="server" meta:resourceKey="numRadio" ID="numRadio" AllowBlank="true" AllowDecimals="true" FieldLabel="Radio" CausesValidation="true" />--%>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValido(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnCancelar"
                        Text="<%$ Resources:Comun, strCancelar %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGestion}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <ext:Window runat="server"
                ID="winRadio"
                Title="<%$ Resources:Comun, strRadio %>"
                BodyStyle="padding:10px;"
                Width="350"
                Height="180"
                Modal="true"
                ShowOnLoad="false"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formRadio"
                        LabelWidth="50"
                        LabelAlign="Left"
                        BodyStyle="padding:10px;"
                        MonitorPoll="500"
                        MonitorValid="true">
                        <Items>
                            <ext:NumberField
                                runat="server"
                                ID="numRadio"
                                AllowBlank="true"
                                AllowDecimals="true"
                                FieldLabel="<%$ Resources:Comun, strRadio %>"
                                CausesValidation="true" />
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoRadio(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnCancelarRadio"
                        Text="<%$ Resources:Comun, strCancelar %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winRadio}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardarRadio"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winRadioBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <ext:Window ID="winAreasOYM"
                runat="server"
                Title="<%$ Resources:Comun, strAreaOandM %>"
                Width="600"
                Height="400"
                Modal="true"
                ShowOnLoad="false"
                Hidden="true">
                <Items>
                    <ext:GridPanel runat="server" ID="gridAreasOYM" Border="false" Header="false" Height="310"
                        StoreID="storeAreasOYM" SelectionMemory="false" EnableColumnHide="false">
                        <TopBar>
                            <ext:Toolbar ID="btnBarAreasOYM" runat="server">
                                <Items>
                                    <ext:Button
                                        ID="btnAgregarAreaOYM"
                                        runat="server"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Cls="btnAnadir">
                                        <Listeners>
                                            <Click Handler="BotonAgregarAreaOYM();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button
                                        ID="btnQuitarFuncionalidad"
                                        runat="server"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        Cls="btnEliminar">
                                        <Listeners>
                                            <Click Handler="BotonEliminarAreaOYM();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnRefrescarAreaOYM"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        Cls="btnRefrescar"
                                        Handler="RefrescarAreaOYM();" />
                                    <%-- <ext:Button ID="toolExportarAreasOYMAsignadas" Text="Excel Areas Asignadas" runat="server" Icon="PageExcel">
                                            <Listeners>
                                                <Click Handler="ExportarDatos('GlobalLocalidades', #{gridFuncionalidades},'','EXPORTAR_AreasOYMAsignadas');" />
                                            </Listeners>
                                        </ext:Button> --%>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel ID="columnModelAreaOYM" runat="server">
                            <Columns>
                                <ext:Column
                                    runat="server"
                                    DataIndex="AreaOYM"
                                    Header="<%$ Resources:Comun, strAreaOandM %>"
                                    Width="350" />
                                <ext:Column
                                    runat="server"
                                    DataIndex="Codigo"
                                    Header="<%$ Resources:Comun, strCodigo %>"
                                    Width="350" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="SeleccionarAreaOYMRowSelection" runat="server" SingleSelect="false"
                                EnableViewState="true">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectAreaOYM" />
                                    <Deselect Fn="DeseleccionarGrillaAreaOYM" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersAreaOYM"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                        <View>
                            <ext:GridView ID="GridAreaOYM"
                                runat="server"
                                SortAscText="Ordenación Ascendente"
                                SortDescText="Ordenación Descendente"
                                ColumnsText="Columnas"
                                ForceFit="true"
                                meta:resourceKey="columOrdenacion" />
                        </View>
                        <BottomBar>
                            <ext:PagingToolbar ID="PagingToolBarAreaOYM"
                                runat="server"
                                PageSize="25"
                                StoreID="storeAreasOYM"
                                DisplayInfo="true"
                                HideRefresh="true">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        Cls="comboGrid"
                                        Width="80">
                                        <Items>
                                            <ext:ListItem Text="1" />
                                            <ext:ListItem Text="20" />
                                            <ext:ListItem Text="30" />
                                            <ext:ListItem Text="40" />
                                            <ext:ListItem Text="50" />
                                        </Items>
                                        <SelectedItems>
                                            <ext:ListItem Value="20" />
                                        </SelectedItems>
                                        <Listeners>
                                            <Select Fn="handlePageSizeSelectAreasOYM" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnAceptarAreaOYM"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="#{winAreasOYM}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Show Handler="#{winAreasOYM}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window
                ID="winAreasOYMLibres"
                runat="server"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="690"
                Height="500"
                Modal="true"
                Resizable="false"
                ShowOnLoad="false"
                Hidden="true">
                <Items>
                    <ext:GridPanel runat="server" ID="gridAreasOYMLibres" Title="AreasOYM" Height="410"
                        Border="false" Frame="false" Header="false" StoreID="storeAreasOYMLibres"
                        SelectionMemory="false" EnableColumnHide="false">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <%-- <ext:Button ID="btnExcelAreasOYMLibres" Text="Excel AreasOYM LIBRES" runat="server" Icon="PageExcel">
                                            <Listeners>
                                                <Click Handler="ExportarDatos('GlobalLocalidades', #{gridAreasOYMLibres},'','EXPORTAR_AreasOYMLibres');" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button ID="btnExcelAreasOYMAllBD" Text="Excel TODAS las AreasOYM de BD" runat="server" Icon="PageExcel">
                                            <Listeners>
                                                <Click Handler="ExportarDatos('GlobalLocalidades', #{gridAreasOYMLibres},'','EXPORTAR_AreasOYMTodas');" />
                                            </Listeners>
                                        </ext:Button>--%>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel ID="ColumnModelAreaOYMLibres" runat="server">
                            <Columns>
                                <ext:Column runat="server"
                                    DataIndex="AreaOYM"
                                    Header="<%$ Resources:Comun, strAreaOandM %>"
                                    Flex="1" />
                                <ext:Column
                                    runat="server"
                                    ColumnID="Codigo"
                                    DataIndex="Codigo"
                                    Header="<%$ Resources:Comun, strAreaOandM %>"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="SeleccionarAreaOYMLIbresRowSelection" runat="server" SingleSelect="false"
                                EnableViewState="true">
                                <Listeners>
                                    <Select Fn="GridAreaOYMLibresSeleccionar_RowSelect" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersAreasOYMLibres"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                    </ext:GridPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnCancelarAreaOYM"
                        runat="server"
                        Text="<%$ Resources:Comun, strCancelar %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winAreasOYMLibres}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnGuardarAreaOYM"
                        runat="server"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="BotonGuardarAreaOYMLibres();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Show Handler="#{winAreasOYMLibres}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window ID="winGestionAdicional"
                runat="server"
                Title="Agregar"
                Width="320"
                AutoHeight="true"
                Modal="true"
                Hidden="true"
                Resizable="true">
                <Items>
                    <ext:FormPanel ID="FormPanelAdicional"
                        runat="server"
                        LabelWidth="60"
                        LabelAlign="Left"
                        BodyStyle="padding:10px;"
                        AutoHeight="false"
                        MonitorPoll="500"
                        MonitorValid="true"
                        Border="false"
                        AutoScroll="true"
                        Height="500">
                        <Items>
                            <ext:TextField
                                ID="txtDDN"
                                runat="server"
                                MaxLength="200"
                                FieldLabel="<%$ Resources:Comun, strDNN %>"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                ID="txtSubalm"
                                runat="server"
                                MaxLength="100"
                                FieldLabel="<%$ Resources:Comun, strSubalm %>"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                ID="txtDescSubalm"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strDescSubalm %>"
                                Text=""
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                ID="txtCabeceraComercial"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strCabeceraComercial %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                ID="txtRegionComercial"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strRegionComercial %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                ID="txtSubregionComercial"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strSubRegionComercial %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                ID="txtZIC"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strZIC %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                ID="txtCabeceraOM"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strCabeceraO&M %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                ID="txtZona"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strZonas %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:Checkbox
                                ID="chkdimIntegral"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strDimIntegral %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                ID="txtcodigoenacom"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strCodigoEnacom %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:NumberField
                                ID="nbrAno"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strAnyo %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:Checkbox
                                ID="chkEstimado"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strEstimado %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:NumberField
                                ID="nbrNumeroHabitnates"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strCantidadHabitantes %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:NumberField
                                ID="PorcentajeNBI"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strNBI %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:NumberField
                                ID="nbrRangoEtario"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strRangoEtario %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:NumberField
                                ID="nbrLatitud"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strLatitud %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:NumberField
                                ID="nbrLongitud"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strLongitud %>"
                                Text=""
                                MaxLength="502"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoGestionAdicional(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button
                        ID="btnCancelarAdicional"
                        runat="server"
                        Text="<%$ Resources:Comun, strCancelar %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGestionAdicional}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button
                        ID="btnGuardarAdicional"
                        runat="server"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardarAdicional();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Cls="vwContenedor"
                Layout="Anchor">
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
                        EnableColumnHide="false"
                        AnchorHorizontal="100%"
                        AnchorVertical="100%"
                        AriaRole="main">
                        <DockedItems>

                            <ext:Toolbar runat="server"
                                ID="Toolbar3"
                                Dock="Top"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnAnadir"
                                        Cls="btnAnadir"
                                        AriaLabel="Añadir"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Handler="AgregarEditar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnEditar"
                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                        Cls="btnEditar"
                                        Handler="MostrarEditar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        Cls="btnEliminar"
                                        Handler="Eliminar();" />
                                    <ext:Button runat="server"
                                        ID="btnRefrescar"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        Cls="btnRefrescar"
                                        Handler="Refrescar();" />
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        Cls="btnDescargar"
                                        Handler="ExportarDatos('GlobalLocalidades', hdCliID.value, #{grid},#{cmbPartido}.getValue(), '');" />
                                    <ext:Button runat="server"
                                        ID="btnActivar"
                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                        Cls="btnActivar"
                                        Handler="Activar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnDefecto"
                                        ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                        Cls="btn-trans"
                                        Handler="Defecto();" />
                                    <ext:Button runat="server"
                                        ID="btnRadio"
                                        Cls="btnRadiofrecuencia"
                                        ToolTip="<%$ Resources:Comun, strRadio %>"
                                        Hidden="true"
                                        Disabled="true"
                                        Handler="BotonRadio()" />
                                    <ext:Button runat="server"
                                        ID="btnAreasOYM"
                                        ToolTip="Areas OYM"
                                        Cls="btnAreas"
                                        Disabled="true"
                                        Hidden="true"
                                        meta:resourceKey="btnAreasOYM"
                                        Handler="BotonAreasOYM()" />
                                    <ext:Button runat="server"
                                        ID="btnAgregarInformacionAdicional"
                                        ToolTip="Agregar Informacion Adicional"
                                        Cls="btnInfo"
                                        Disabled="true"
                                        Hidden="false"
                                        meta:resourceKey="btnAgregarInformacionAdicional"
                                        Handler="BotonAgregarInformacionAdicional();" />
                                </Items>
                            </ext:Toolbar>


                            <ext:Toolbar runat="server"
                                ID="tlbBase"
                                Dock="Top"
                                Layout="ColumnLayout"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:ComboBox MarginSpec="8 0 0 0" MaxWidth="230" meta:resourceKey="cmbMunicipio" runat="server" LabelAlign="Right" ID="cmbMunicipio"  FieldLabel="<%$ Resources:Comun, strMunicipio %>"
                                        StoreID="storeMunicipios" Mode="Local" DisplayField="Municipio" ValueField="MunicipioID"
                                        EmptyText="Seleccione un Municipio" Editable="true" QueryMode="Local" Cls="comboGrid pos-boxGrid">
                                        <Triggers>
                                            <ext:FieldTrigger IconCls="ico-reload" QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                        <Listeners>
                                            <Select Handler="MunicipioSeleccionar();" />
                                            <TriggerClick Fn="TriggerMunicipio" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox MarginSpec="8 0 0 0" MaxWidth="230" meta:resourceKey="cmbProvincia" runat="server" LabelAlign="Right" ID="cmbProvincia"  FieldLabel="Provincia"
                                        StoreID="storeProvincias" Mode="Local" DisplayField="Provincia" ValueField="ProvinciaID"
                                        EmptyText="Seleccione una Provincia" Editable="true" QueryMode="Local" Cls="comboGrid pos-boxGrid">
                                        <Triggers>
                                            <ext:FieldTrigger IconCls="ico-reload" QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                        <Listeners>
                                            <Select Handler="ProvinciaSeleccionar();" />
                                            <TriggerClick Fn="TriggerProvincia" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox MarginSpec="8 0 0 0" MaxWidth="230" meta:resourceKey="cmbRegionPais" runat="server" LabelAlign="Right" ID="cmbRegionPais"  FieldLabel="<%$ Resources:Comun, strRegionPais %>"
                                        StoreID="storeRegionesPaises" Mode="Local" DisplayField="RegionPais" ValueField="RegionPaisID"
                                        EmptyText="Seleccione una Región" Editable="true" QueryMode="Local" Cls="comboGrid pos-boxGrid">
                                        <Triggers>
                                            <ext:FieldTrigger IconCls="ico-reload" QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                        <Listeners>
                                            <Select Handler="RegionPaisSeleccionar();" />
                                            <TriggerClick Fn="TriggerRegionPais" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox MarginSpec="8 0 0 0" MaxWidth="230" runat="server" LabelAlign="Right" ID="cmbPais"  FieldLabel="Pais"
                                        StoreID="storePaises" Mode="Local" DisplayField="Pais" ValueField="PaisID"
                                        EmptyText="<%$ Resources:Comun, strSelecPais %>" Editable="true" QueryMode="Local" Cls="comboGrid pos-boxGrid">
                                        <Triggers>
                                            <ext:FieldTrigger IconCls="ico-reload" QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                        <Listeners>
                                            <Select Handler="PaisSeleccionar();" />
                                            <TriggerClick Fn="TriggerPais" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox MarginSpec="8 0 0 0" MaxWidth="230" meta:resourceKey="cmbRegion" runat="server" LabelAlign="Right" ID="cmbRegion"  FieldLabel="<%$ Resources:Comun, strRegion %>"
                                        StoreID="storeRegiones" Mode="Local" DisplayField="Region" ValueField="RegionID" EmptyText="Seleccione una Region"
                                        Editable="true" QueryMode="Local" Cls="comboGrid pos-boxGrid">
                                        <Triggers>
                                            <ext:FieldTrigger IconCls="ico-reload" QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                        <Listeners>
                                            <Select Handler="RegionSeleccionar();" />
                                            <TriggerClick Fn="TriggerRegiones" />
                                        </Listeners>
                                    </ext:ComboBox>

                                    <ext:ComboBox MarginSpec="8 0 0 0" MaxWidth="230" meta:resourceKey="cmbPartido" LabelAlign="Right" runat="server" ID="cmbPartido"  FieldLabel="Partido"
                                        StoreID="storePartidos" Mode="Local" DisplayField="Partido" ValueField="GlobalPartidoID"
                                        EmptyText="Seleccione un Partido" Editable="true" QueryMode="Local" Cls="comboGrid pos-boxGrid">
                                        <Triggers>
                                            <ext:FieldTrigger IconCls="ico-reload" QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                        <Listeners>
                                            <Select Handler="PartidoSeleccionar();" />
                                            <TriggerClick Fn="TriggerPartido" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox  MarginSpec="8 0 0 0" MaxWidth="230" meta:resourceKey="cmbMunicipalidad" runat="server" LabelAlign="Right" ID="cmbMunicipalidad"  FieldLabel="<%$ Resources:Comun, strMunicipalidad %>"
                                        StoreID="storeMunicipalidad" Mode="Local" DisplayField="Municipalidad" ValueField="GlobalMunicipalidadID"
                                        EmptyText="Seleccione una Municipalidad" Editable="true" QueryMode="Local" Cls="comboGrid pos-boxGrid">
                                        <Triggers>
                                            <ext:FieldTrigger IconCls="ico-reload" QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                        <Listeners>
                                            <Select Handler="MunicipalidadSeleccionar();" />
                                            <TriggerClick Fn="TriggerMunicipalidad" />
                                        </Listeners>
                                    </ext:ComboBox>


                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server"
                                ID="tlbClientes"
                                Dock="Top">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbClientes"
                                        StoreID="storeClientes"
                                        DisplayField="Cliente"
                                        ValueField="ClienteID"
                                        Cls="comboGrid pos-boxGrid"
                                        QueryMode="Local"
                                        Hidden="true"
                                        EmptyText="<%$ Resources:Comun, cmbClientes.EmptyText %>"
                                        FieldLabel="<%$ Resources:Comun, cmbClientes.FieldLabel %>">
                                        <Listeners>
                                            <Select Fn="SeleccionarCliente" />
                                            <TriggerClick Fn="RecargarClientes" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column runat="server"
                                    ID="Activo"
                                    DataIndex="Activo"
                                    ToolTip="<%$ Resources:Comun, strActivo %>"
                                    Align="Center"
                                    Cls="col-activo"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="Defecto"
                                    DataIndex="Defecto"
                                    ToolTip="<%$ Resources:Comun, strDefecto %>"
                                    Align="Center"
                                    Cls="col-default"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    DataIndex="Localidad"
                                    Text="<%$ Resources:Comun, strLocalidad %>"
                                    Width="150"
                                    ID="Localidad"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="Codigo"
                                    Text="<%$ Resources:Comun, strCodigo %>"
                                    Width="150" ID="Codigo"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="LocalidadCentroProblado"
                                    Text="<%$ Resources:Comun, strLocalidadCentroPoblado %>"
                                    Width="250"
                                    ID="LocalidadCentroProblado"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="DDN"
                                    Text="<%$ Resources:Comun, strDNN %>"
                                    Width="150" ID="DDN"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="Subalm"
                                    Text="<%$ Resources:Comun, strSubalm %>"
                                    Width="250"
                                    ID="Subalm"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="DescSubalm"
                                    Text="<%$ Resources:Comun, strDescSubalm %>"
                                    Width="250"
                                    ID="DescSubalm"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="CabeceraComercial"
                                    Text="<%$ Resources:Comun, strCabeceraComercial %>"
                                    Width="200"
                                    ID="CabeceraComercial"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="RegionComercial"
                                    Text="<%$ Resources:Comun, strRegionComercial %>"
                                    Width="200"
                                    ID="RegionComercial"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="SubregionComercial"
                                    Text="<%$ Resources:Comun, strSubRegionComercial %>"
                                    Width="200"
                                    ID="SubregionComercial"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="ZIC"
                                    Text="<%$ Resources:Comun, strZIC %>"
                                    Width="180"
                                    ID="ZIC"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="CabeceraOYM"
                                    Text="<%$ Resources:Comun, strCabeceraO&M %>"
                                    Width="200"
                                    ID="CabeceraOYM"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="Zona"
                                    Text="<%$ Resources:Comun, strLocalidad %>"
                                    Width="200"
                                    ID="Zona"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="DimIntegral"
                                    Text="<%$ Resources:Comun, strDimIntegral %>"
                                    Width="110"
                                    ID="DimIntegral"
                                    runat="server">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    DataIndex="EsEstimado"
                                    Text="<%$ Resources:Comun, strEstimado %>"
                                    Width="110"
                                    ID="EsEstimado"
                                    runat="server">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    DataIndex="CodigoEnacom"
                                    Text="<%$ Resources:Comun, strCodigoEnacom %>"
                                    Width="250"
                                    ID="CodigoEnacom"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="Anio"
                                    Text="<%$ Resources:Comun, strAnio %>"
                                    Width="150"
                                    ID="Anio"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="CantidadHabitantes"
                                    Text="<%$ Resources:Comun, strCantidadHabitantes %>"
                                    Width="100"
                                    ID="CantidadHabitantes"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="NBI"
                                    Text="<%$ Resources:Comun, strNBI %>"
                                    Width="100"
                                    ID="NBI"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="RangoEtario"
                                    Text="<%$ Resources:Comun, strRangoEtario %>"
                                    Width="100"
                                    ID="RangoEtario"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="Latitud"
                                    Text="<%$ Resources:Comun, strLatitud %>"
                                    Width="100"
                                    ID="Latitud"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="Longitud"
                                    Text="<%$ Resources:Comun, strLongitud %>"
                                    Width="100"
                                    ID="Longitud"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="Radio"
                                    Text="<%$ Resources:Comun, strRadio %>"
                                    Width="200"
                                    ID="Radio"
                                    runat="server" />
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
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
