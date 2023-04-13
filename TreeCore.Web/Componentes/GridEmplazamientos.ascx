<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridEmplazamientos.ascx.cs" Inherits="TreeCore.Componentes.GridEmplazamientos" %>

<%@ Register Src="/Componentes/FormEmplazamientos.ascx" TagName="FormEmplazamientos" TagPrefix="local" %>
<%@ Register Src="/Componentes/FormContactos.ascx" TagName="FormContactosEmplazamientos" TagPrefix="local" %>
<%--<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="filtros" %>--%>

<%--<script type="text/javascript" src="/Componentes/js/FormEmplazamientos.js"></script>
<script type="text/javascript" src="/Componentes/js/FormContactos.js"></script>
<script type="text/javascript" src="/Componentes/js/Localizaciones.js"></script>
<link href="/Componentes/css/GridEmplazamientos.css" rel="stylesheet" type="text/css" />--%>

<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteIDComponente" />
<ext:Hidden ID="hdIDComponente" runat="server" />

<ext:Hidden ID="CurrentControl" runat="server" />
<ext:Hidden ID="hdControlURL" runat="server" />
<ext:Hidden ID="hdControlName" runat="server" />
<ext:Hidden ID="hdNombre" runat="server" />
<ext:Hidden ID="hdFiltrosAplicados" runat="server" />

<ext:Hidden ID="hdEmplazamientoSeleccionado" Name="hdEmplazamientoSeleccionado" runat="server" />
<ext:hidden ID="hdTotalCountGrid" runat="server" >
    <Listeners>
        <Change Fn="setNumMaxPageEmplazamientos"></Change>
    </Listeners>
</ext:hidden>

<ext:Container 
    ID="hugeCt" 
    Hidden="true" 
    runat="server" 
    Layout="FitLayout" 
    Cls="hugeMainCt">
</ext:Container>

<%--MENU BOTON DERECHO--%>

<ext:Menu runat="server"
    ID="ContextMenu">
    <Items>
        <ext:MenuItem runat="server"
            IconCls="ico-geolocalizacion-ctxMnu"
            Text="<%$ Resources:Comun, strMapa %>"
            ID="ShowMap" />
        <ext:MenuItem runat="server"
            IconCls="ico-Piechart-ctxMnu"
            Text="Dashboard"
            ID="ShowDashboard"
            Hidden="true" />
        <ext:MenuItem runat="server"
            IconCls="ico-Inventory-ctxMnu"
            Text="<%$ Resources:Comun, strInventario %>"
            ID="ShowInventory" />
        <ext:MenuItem runat="server"
            IconCls="ico-Documents-ctxMnu"
            Text="<%$ Resources:Comun, strDocumentos %>"
            ID="ShowDocuments" />
        <ext:MenuItem runat="server"
            IconCls="ico-Contratos-ctxMnu"
            Text="<%$ Resources:Comun, strContratos %>"
            ID="ShowContracts"
            Hidden="true" />
        <ext:MenuItem runat="server"
            IconCls="ico-historial-ctxMnu"
            Text="<%$ Resources:Comun, strHistorico %>"
            ID="ShowHistorial" />
    </Items>
    <Listeners>
        <Click Fn="OpcionSeleccionada" />
    </Listeners>
</ext:Menu>

<%--FIN MENU BOTON DERECHO--%>

<ext:Window runat="server"
    ID="winGestion"
    Title="<%$ Resources:Comun, winGestion.Title %>"
    Modal="true"
    Width="750"
    Height="600"
    Hidden="true"
    OverflowY="Auto"
    Layout="FitLayout"
    Cls="winForm-resp winGestion">
    <Content>
        <local:FormEmplazamientos ID="formAgregarEditar" runat="server" Requerido="false" PaisDefecto="true" />
    </Content>
    <Listeners>
        <BeforeHide Fn="ClearFormulario" />
        <Hide Fn="actualizar" />
    </Listeners>
</ext:Window>

<ext:Window runat="server"
    ID="winFormContacto"
    Modal="true"
    Draggable="true"
    Cls="winGestionContactoEmplazamiento"
    Title="<%$ Resources:Comun, strAgregarEliminarContactos %>"
    Hidden="True"
    Border="false"
    Resizable="true"
    Closable="true"
    Width="700"
    Height="520"
    MinHeight="220"
    OverflowY="Auto">
    <Items>
        <ext:FormPanel runat="server"
            ID="formGestionContactos"
            Cls="formGestion-resp formGris"
            BodyStyle="padding:10px;"
            Border="false">
            <Items>
                <ext:Container runat="server"
                    ID="AgregarContacto" Cls="ct-add-contact">
                    <Items>
                        <ext:Button runat="server"
                            ID="btnAgregarContacto"
                            Text="<%$ Resources:Comun, jsAgregar %>"
                            IconCls="ico-addBtn"
                            Cls="addContactos btnAgregarContacto">
                            <Listeners>
                                <Click Fn="agregarContactoEmplazamiento" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Container>
                <ext:Container runat="server"
                    ID="Busquedas"
                    Cls="ctFormContactos-resp ctForm-resp-col2">
                    <Items>
                        <ext:TextField runat="server"
                            ID="txtBuscarEmail"
                            EmptyText="<%$ Resources:Comun, strBusqueda %>"
                            MaxLength="50"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear"
                                    Handler="limpiar(this);" />
                                <ext:FieldTrigger Icon="Search" Handler="buscador(this)" />
                            </Triggers>
                            <%--<Listeners>
                                <Change Fn="buscador" />
                            </Listeners>--%>
                        </ext:TextField>
                        <ext:DropDownField runat="server"
                            ID="searchTel"
                            AllowBlank="true"
                            TriggerCls="icoprefix"
                            Cls="searchTel"
                            FocusCls="testfocus"
                            EmptyText="<%$ Resources:Comun, strSeleccionarPrefijo %>"
                            ValidationGroup="FORM"
                            CausesValidation="true">
                            <Component>
                                <ext:MenuPanel runat="server"
                                    ID="MenuPanelIco"
                                    MaxWidth="120">
                                    <Menu runat="server">
                                        <Items>
                                            <ext:MenuItem runat="server"
                                                Text="+34 "
                                                Icon="FlagEs" />
                                            <ext:MenuItem runat="server"
                                                Text="+43 "
                                                Icon="FlagFr" />
                                            <ext:MenuItem runat="server"
                                                Text="+54 "
                                                Icon="FlagDe" />
                                        </Items>
                                        <Listeners>
                                            <Click Handler="#{searchTel}.setValue(menuItem.text);" />
                                        </Listeners>
                                    </Menu>
                                </ext:MenuPanel>
                            </Component>
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear"
                                    Handler="limpiar(this);" />
                                <ext:FieldTrigger Icon="Search" Handler="buscador(this)" />
                            </Triggers>
                        </ext:DropDownField>
                    </Items>
                </ext:Container>
                <ext:GridPanel runat="server"
                    ID="gridContactosEmp"
                    MultiSelect="true"
                    ForceFit="false"
                    Border="false"
                    Header="false"
                    Height="300"
                    Cls="gridPanel grdNoHeader"
                    OverflowY="Hidden"
                    OverflowX="Hidden">
                    <Store>
                        <ext:Store runat="server"
                            ID="storeContactosGlobalesEmplazamientosVinculados"
                            AutoLoad="true"
                            RemoteSort="false"
                            RemoteFilter="false"
                            RemotePaging="true"
                            OnReadData="storeContactosGlobalesEmplazamientosVinculados_Refresh">
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Model>
                                <ext:Model runat="server"
                                    IDProperty="ContactoGlobalID">
                                    <Fields>
                                        <ext:ModelField Name="ContactoGlobalID" Type="Int" />
                                        <ext:ModelField Name="EmplazamientoID" Type="Int" />
                                        <ext:ModelField Name="Nombre" Type="String" />
                                        <ext:ModelField Name="Apellidos" Type="String" />
                                        <ext:ModelField Name="Email" Type="String" />
                                        <ext:ModelField Name="Telefono" Type="String" />
                                        <ext:ModelField Name="ContactoTipoID" Type="Int" />
                                        <ext:ModelField Name="MunicipioID" Type="Int" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server"
                                Text="<%$ Resources:Comun, strNombre %>"
                                DataIndex="Nombre"
                                Flex="3"
                                Width="140" />
                            <ext:Column runat="server"
                                Text="<%$ Resources:Comun, strApellidos %>"
                                DataIndex="Apellidos"
                                Flex="3"
                                Width="140" />
                            <ext:Column runat="server"
                                Text="<%$ Resources:Comun, strEmail %>"
                                DataIndex="Email"
                                Flex="3"
                                Width="140" />
                            <ext:Column runat="server"
                                Text="<%$ Resources:Comun, strTelefono %>"
                                DataIndex="Telefono"
                                Flex="2"
                                Width="140" />
                            <ext:WidgetColumn ID="wdEditar"
                                runat="server"
                                Text="<%$ Resources:Comun, jsEditar %>"
                                Cls="col-More"
                                DataIndex=""
                                Filterable="false"
                                Hidden="false"
                                MinWidth="50"
                                Flex="1">
                                <Widget>
                                    <ext:Button runat="server"
                                        OverCls="Over-btnMore"
                                        PressedCls="Pressed-none"
                                        FocusCls="Focus-none"
                                        Cls="btnEditar btn-trans">
                                        <Listeners>
                                            <Click Fn="editarContactoEmplazamiento" />
                                        </Listeners>
                                    </ext:Button>
                                </Widget>
                            </ext:WidgetColumn>
                            <ext:ComponentColumn runat="server" DataIndex="EmplazamientoID" Flex="1">
                                <Component>
                                    <ext:Button runat="server"
                                        ID="btnToggle"
                                        Width="41"
                                        EnableToggle="true"
                                        Cls="btn-toggleGrid"
                                        Pressed="true"
                                        AriaLabel=""
                                        ToolTip="">
                                        <Listeners>
                                            <Click Fn="cambiarAsignacion" />
                                        </Listeners>
                                    </ext:Button>
                                </Component>
                                <Listeners>
                                    <Bind Fn="SetToggleValue" />
                                </Listeners>
                            </ext:ComponentColumn>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server"
                            ID="GridRowSelectContacto"
                            Mode="Single">
                            <Listeners>
                                <Select Fn="Grid_RowSelectContactos" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                </ext:GridPanel>
            </Items>
        </ext:FormPanel>
    </Items>
    <Listeners>
        <Close Fn="closeWindowContactoEmplazamiento" />
    </Listeners>
</ext:Window>

<ext:Window runat="server"
    ID="winGestionContactoEmplazamiento"
    Resizable="false"
    Modal="true"
    Width="750px"
    Draggable="false"
    Layout="FitLayout"
    Hidden="true">
    <Content>
        <local:FormContactosEmplazamientos ID="formAgregarEditarContactoEmplazamiento"
            runat="server" />
    </Content>
    <Listeners>
        <Hide Handler="#{storeContactosGlobalesEmplazamientosVinculados}.reload();" />
    </Listeners>
</ext:Window>

<ext:Window
    runat="server" ID="WinContractDetails"
    meta:resourceKey="WinContractDetails"
    Width="520"
    Height="400"
    MinHeight="220"
    MaxHeight="500"
    Hidden="true"
    Title="<%$ Resources:Comun, strContratos %>"
    Cls="WinTrackDocum WinContractD"
    Border="false"
    Resizable="false"
    Closable="true"
    Layout="FitLayout"
    OverflowY="Auto">
    <Listeners>
        <FocusLeave Fn="HideWinContract" />
    </Listeners>
    <Items>
        <ext:GridPanel
            ID="GridContractsD"
            runat="server"
            MultiSelect="true"
            ForceFit="true"
            Border="false"
            Header="false"
            EnableColumnHide="false"
            Cls="gridPanel"
            OverflowY="Auto">
            <Store>
                <ext:Store
                    ID="storeContractos"
                    runat="server"
                    AutoLoad="false">
                    <Model>
                        <ext:Model runat="server" IDProperty="AlquilerID">
                            <Fields>
                                <ext:ModelField Name="AlquilerID" Type="Int" />
                                <ext:ModelField Name="NumContrato" />
                                <ext:ModelField Name="TipoContrato" />
                                <ext:ModelField Name="Estado" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column
                        runat="server"
                        Text="<%$ Resources:Comun, strTipoContrato %>"
                        DataIndex="TipoContrato"
                        Flex="2">
                        <Filter>
                            <ext:StringFilter EmptyText="<%$ Resources:Comun, strFiltro %>" />
                        </Filter>
                    </ext:Column>
                    <ext:Column
                        runat="server"
                        Text="<%$ Resources:Comun, strNumContrato %>"
                        DataIndex="NumContrato"
                        Flex="2">
                        <Filter>
                            <ext:StringFilter EmptyText="<%$ Resources:Comun, strFiltro %>" />
                        </Filter>
                    </ext:Column>
                    <ext:Column
                        runat="server"
                        Text="<%$ Resources:Comun, strEstado %>"
                        DataIndex="Estado"
                        Flex="2">
                        <Filter>
                            <ext:StringFilter EmptyText="<%$ Resources:Comun, strFiltro %>" />
                        </Filter>
                    </ext:Column>
                    <ext:WidgetColumn
                        meta:resourcekey="ColMostrar"
                        ID="ColMostrar"
                        runat="server"
                        Flex="1"
                        Cls="col-More"
                        DataIndex=""
                        Align="Start"
                        Text=""
                        Hidden="true">
                        <Widget>
                            <ext:Button
                                meta:resourcekey="btnMostrar"
                                runat="server"
                                ID="btnMostrar"
                                Height="40"
                                Width="60"
                                OverCls="Over-btnMore"
                                PressedCls="Pressed-none"
                                FocusCls="Focus-none"
                                Cls="btnColLaunch"
                                Disabled="true"
                                Handler="hidePn()" />
                        </Widget>
                    </ext:WidgetColumn>

                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:RowSelectionModel runat="server" />
            </SelectionModel>
            <Plugins>
                <ext:GridFilters runat="server"
                    ID="gridFilters2"
                    MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                    meta:resourceKey="GridFilters">
                </ext:GridFilters>
            </Plugins>
            <View>
                <ext:GridView runat="server" EnableTextSelection="true" />
            </View>
        </ext:GridPanel>
    </Items>
</ext:Window>

<ext:Window
    runat="server" ID="WinProjectsDetails"
    meta:resourceKey="WinProjectsDetails"
    Width="520"
    Height="400"
    MinHeight="220"
    MaxHeight="500"
    Hidden="true"
    Title="<%$ Resources:Comun, strProyectos %>"
    Cls="WinTrackDocum WinContractD"
    Border="false"
    Resizable="false"
    Closable="true"
    Layout="FitLayout"
    OverflowY="Auto">
    <Listeners>
        <FocusLeave Fn="HideWinProjects" />
    </Listeners>
    <Items>
        <ext:GridPanel
            ID="GridProjectsD"
            runat="server"
            ForceFit="true"
            Border="false"
            EnableColumnHide="false"
            Header="false"
            Cls="gridPanel ExtraHeightGrid"
            OverflowY="Auto">
            <Store>
                <ext:Store
                    ID="storeEmplazamientosProyectos"
                    runat="server"
                    AutoLoad="false">
                    <Model>
                        <ext:Model runat="server" IDProperty="EmplazamientoProyectoID">
                            <Fields>
                                <ext:ModelField Name="Proyecto" />
                                <ext:ModelField Name="ProyectoTipo" />
                                <ext:ModelField Name="Descripcion" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>
                    <ext:Column
                        runat="server"
                        Text="<%$ Resources:Comun, strProyectoTipo %>"
                        DataIndex="ProyectoTipo"
                        Flex="1">
                        <Filter>
                            <ext:StringFilter EmptyText="<%$ Resources:Comun, strFiltro %>" />
                        </Filter>
                    </ext:Column>
                    <ext:Column
                        runat="server"
                        Text="<%$ Resources:Comun, strProyecto %>"
                        DataIndex="Proyecto"
                        Flex="1">
                        <Filter>
                            <ext:StringFilter EmptyText="<%$ Resources:Comun, strFiltro %>" />
                        </Filter>
                    </ext:Column>
                    <ext:Column
                        runat="server"
                        Text="<%$ Resources:Comun, strEstado %>"
                        DataIndex="Descripcion"
                        Flex="1">
                        <Filter>
                            <ext:StringFilter EmptyText="<%$ Resources:Comun, strFiltro %>" />
                        </Filter>
                    </ext:Column>
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:RowSelectionModel runat="server" />
            </SelectionModel>
            <Plugins>
                <ext:GridFilters runat="server"
                    ID="gridFilters1"
                    MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                    meta:resourceKey="GridFilters">
                </ext:GridFilters>
            </Plugins>
            <View>
                <ext:GridView
                    runat="server"
                    EnableTextSelection="true" />
            </View>
            <DockedItems>
                <ext:Toolbar
                    ID="tbBottomWinProjects"
                    runat="server"
                    Dock="Bottom"
                    Hidden="true">
                    <Items>
                        <ext:ToolbarFill></ext:ToolbarFill>
                        <ext:Button
                            ID="btnEditProj"
                            runat="server"
                            Text="Editar Proyecto"
                            Disabled="true"
                            Cls="btn-ppal-winForm btnEditWinProj"
                            MarginSpec="0 35 10 0" />
                    </Items>
                </ext:Toolbar>
            </DockedItems>
        </ext:GridPanel>
    </Items>
</ext:Window>


<ext:GridPanel
    runat="server"
    OverflowX="Auto"
    ID="gridEmplazamientos"
    ForceFit="true"
    EnableColumnHide="false"
    Scrollable="Vertical"
    Cls="gridPanel grdNoHeader">
    <Store>
        <ext:Store
            runat="server"
            ID="storeEmplazamientos"
            RemotePaging="true"
            AutoLoad="false"
            OnReadData="storeEmplazamientos_Refresh"
            RemoteSort="true"
            RemoteFilter="true"
            PageSize="20">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model
                    runat="server"
                    IDProperty="EmplazamientoID">
                    <Fields>
                        <ext:ModelField Name="EmplazamientoID" Type="Int" />
                        <ext:ModelField Name="ClienteID" Type="Int" />
                        <ext:ModelField Name="Codigo" Type="String" />
                        <ext:ModelField Name="NombreSitio" Type="String" />
                        <ext:ModelField Name="Proyectos" Type="Int" />
                        <ext:ModelField Name="Contratos" Type="Int" />
                        <ext:ModelField Name="Moneda" Type="String" />
                        <ext:ModelField Name="EstadoGlobal" Type="String" />
                        <ext:ModelField Name="CategoriaSitio" Type="String" />
                        <ext:ModelField Name="Tipo" Type="String" />
                        <ext:ModelField Name="TipoEdificio" Type="String" />
                        <ext:ModelField Name="TipoEstructura" Type="String" />
                        <ext:ModelField Name="FechaActivacion" Type="Date" />
                        <ext:ModelField Name="FechaDesactivacion" Type="Date" />
                        <ext:ModelField Name="Tamano" Type="String" />
                        <ext:ModelField Name="Operador" Type="String" />
                    </Fields>
                </ext:Model>
            </Model>
            <%--<Sorters>
                <ext:DataSorter
                    Property="EmplazamientoID"
                    Direction="DESC" />
            </Sorters>--%>
            <Listeners>
                <DataChanged Fn="ajaxGetDatosBuscadorEmplazamientos" />
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
                    ID="btnAnadir"
                    Cls="btnAnadir"
                    AriaLabel="Añadir"
                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>">
                    <Listeners>
                        <Click Fn="agregar" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnEditar"
                    Cls="btnEditar"
                    AriaLabel="Editar"
                    Disabled="true"
                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>">
                    <Listeners>
                        <Click Fn="editar" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnRefrescar"
                    Cls="btnRefrescar"
                    AriaLabel="Actualizar"
                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>">
                    <Listeners>
                        <Click Fn="actualizar" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnDescargar"
                    Hidden="false"
                    Cls="btnDescargar-subM btnDescargarMenu"
                    AriaLabel="Descargar"
                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>">
                    <Menu>
                        <ext:Menu runat="server">
                            <Items>
                                <ext:MenuItem
                                    meta:resourceKey="mnuDwldExcel"
                                    ID="mnuDwldExcel"
                                    runat="server"
                                    Text="Excel"
                                    IconCls="ico-CntxMenuExcel"
                                    Handler="ExportarDatos('Emplazamientos', App.hdCliID.value, #{gridEmplazamientos}, '/Componentes/GridEmplazamientos.ascx', '', JSON.stringify({ items: filtrosAplicados, tab: 'Sites' }), App.hdStringBuscador.value, App.hdIDEmplazamientoBuscador.value);" />
                                <ext:MenuItem
                                    meta:resourceKey="mnuDwldLoadSites"
                                    ID="mnuDwldLoadSites"
                                    runat="server"
                                    Text="Load Sites"
                                    Hidden="true"
                                    IconCls="ico-CntxMenuExcel" />
                                <ext:MenuItem
                                    meta:resourceKey="mnuDwldLimited"
                                    ID="mnuDwldLimited"
                                    runat="server"
                                    Text="Limited"
                                    Hidden="true"
                                    IconCls="ico-CntxMenuExcel" />
                                <ext:MenuItem
                                    meta:resourceKey="mnuDwldContacts"
                                    ID="mnuDwldContacts"
                                    runat="server"
                                    Text="Contacts"
                                    Hidden="true"
                                    IconCls="ico-CntxMenuExcel" />
                                <ext:MenuItem
                                    meta:resourceKey="mnuDwldExclGlobal"
                                    ID="mnuDwldExclGlobal"
                                    runat="server"
                                    Text="Excel Global"
                                    Hidden="true"
                                    IconCls="ico-CntxMenuExcel" />
                                <ext:MenuItem
                                    meta:resourceKey="mnuDwldIFRS16"
                                    ID="mnuDwldIFRS16"
                                    runat="server"
                                    Text="IFRS16"
                                    Hidden="true"
                                    IconCls="ico-CntxMenuExcel" />
                                <ext:MenuItem
                                    meta:resourceKey="mnuDwldModeloReg"
                                    ID="mnuDwldModeloReg"
                                    runat="server"
                                    Text="Regional Model"
                                    Hidden="true"
                                    IconCls="ico-CntxMenuExcel" />
                            </Items>
                        </ext:Menu>
                    </Menu>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnContactos"
                    Cls="btnContactos"
                    Disabled="true"
                    ToolTip="<%$ Resources:Comun, strContactos %>">
                    <Listeners>
                        <Click Handler="#{winFormContacto}.show(); #{storeContactosGlobalesEmplazamientosVinculados}.reload();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnProjects"
                    Cls="btn-trans"
                    Hidden="true"
                    ToolTip="<%$ Resources:Comun, strAnadirAProyecto %>" />
                <ext:Button runat="server"
                    ID="btnDirecciones"
                    Cls="btn-trans"
                    Hidden="true"
                    ToolTip="<%$ Resources:Comun, strDirecciones %>" />
                <ext:Button runat="server"
                    ID="btnHistorial"
                    Cls="btn-trans"
                    Hidden="true"
                    ToolTip="<%$ Resources:Comun, strHistorial %>" />
                <ext:Button runat="server"
                    ID="btnVandalismo"
                    Hidden="true"
                    Cls="btn-trans"
                    ToolTip="<%$ Resources:Comun, strVandalismo %>" />
                <ext:Button runat="server"
                    ID="btnRenta"
                    Hidden="true"
                    Cls="btn-trans"
                    ToolTip="<%$ Resources:Comun, strActualizarRentas %>" />
                <ext:Button runat="server"
                    ID="btnCompartido"
                    Cls="btn-trans"
                    Hidden="true"
                    ToolTip="<%$ Resources:Comun, strEmplazamientoCompartido %>" />
                <ext:Label runat="server"
                    ID="lblToggle"
                    Hidden="true"
                    Text="<%$ Resources:Comun, strEmplazamientosCliente %>">
                </ext:Label>
                <ext:Button runat="server"
                    ID="btnCliente"
                    Width="41"
                    Hidden="true"
                    EnableToggle="true"
                    Cls="btn-toggleGrid"
                    ToolTip="<%$ Resources:Comun, strEmplazamientosCliente %>" />
                <ext:Button runat="server"
                    ID="btnFiltros"
                    Cls="btnFiltros"
                    EnableToggle="true"
                    ToolTip="<%$ Resources:Comun, strMostrarPanelFiltros %>"
                    Handler="hidePnFilters();" />
                <ext:Button runat="server"
                    ID="btnMasOpciones"
                    Hidden="true"
                    Cls="btn-trans btnOpciones-Mnu "
                    ToolTip="<%$ Resources:Comun, strMasOpciones %>"
                    Width="28">
                    <Menu>
                        <ext:Menu runat="server">
                            <Items>
                                <ext:MenuItem
                                    meta:resourceKey="mnuMoreOptTrafico"
                                    ID="mnuMoreOptTrafico"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strTrafico %>"
                                    IconCls="ico-CntxMenuTraffic" />
                                <ext:MenuItem
                                    meta:resourceKey="mnuMoreOptImpLegalizar"
                                    ID="mnuMoreOptImpLegalizar"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strImposibleLegalizarLosSitios %>"
                                    IconCls="ico-CntxMenuImposLegal" />
                                <ext:MenuItem
                                    meta:resourceKey="mnuMoreOptAddComarch"
                                    ID="mnuMoreOptAddComarch"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strAgregarInformaciónDeComarca %>"
                                    IconCls="ico-CntxMenuAddInfoExcel" />
                                <ext:MenuItem
                                    meta:resourceKey="mnuMoreOptAddAdditional"
                                    ID="mnuMoreOptAddAdditional"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strAñadirInformacionAdicional %>"
                                    IconCls="ico-CntxMenuAddInfoExcel" />
                            </Items>
                        </ext:Menu>
                    </Menu>

                </ext:Button>
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
                        <%--<ext:Button
                            runat="server" 
                            Width="30" 
                            ID="Button1" 
                            Cls="btn-trans btnColumnas" 
                            AriaLabel="Show Column"
                            ToolTip="Show Column" 
                            Hidden="true">
                            <Listeners>
                                <Click Fn="showMenuOrderColumn" />
                            </Listeners>
                        </ext:Button>--%>
                        <ext:Button
                            runat="server"
                            Width="30"
                            ID="btnClearFilters"
                            Cls="btn-trans btnRemoveFilters"
                            AriaLabel="Quitar Filtros"
                            ToolTip="<%$ Resources:Comun, strQuitarFiltro %>">
                            <Listeners>
                                <Click Handler="BorrarFiltros(#{gridEmplazamientos});"></Click>
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
                meta:resourceKey="strOperador"
                ID="colOperador"
                runat="server"
                Text="<%$ Resources:Comun, strOperador %>"
                DataIndex="Operador"
                MaxWidth="250" />
            <ext:ComponentColumn runat="server"
                meta:resourceKey="colProject"
                ID="colProjectAsig"
                Align="Center"
                Text="<%$ Resources:Comun, strProyectos %>"
                DataIndex="Proyectos"
                MinWidth="90"
                MaxWidth="90">
                <Component>
                    <ext:Button runat="server"
                        ID="ProjectBtn"
                        Cls="ico-masInfo-widget btnProjectsNContracts"
                        OverCls="none"
                        PressedCls="none"
                        FocusCls="none"
                        Flat="true"
                        TextAlign="Left">
                        <Listeners>
                            <Click Fn="ClickShowProjectsD" />
                        </Listeners>
                    </ext:Button>
                </Component>
                <Listeners>
                    <Bind Handler="cmp.setText(record.get('Proyectos'))" />
                </Listeners>
                <Filter>
                    <ext:NumberFilter EmptyText="<%$ Resources:Comun, strNumber %>"></ext:NumberFilter>
                </Filter>
            </ext:ComponentColumn>
            <ext:ComponentColumn runat="server"
                meta:resourceKey="colProject"
                ID="colContractsAsig"
                Align="Center"
                Text="<%$ Resources:Comun, strContratos %>"
                DataIndex="Contratos"
                MinWidth="90"
                MaxWidth="90">
                <Component>
                    <ext:Button
                        ID="ContractsBtn"
                        Cls="ico-masInfo-widget btnProjectsNContracts"
                        OverCls="none"
                        PressedCls="none"
                        FocusCls="none"
                        runat="server" Flat="true"
                        TextAlign="Left">
                        <Listeners>
                            <Click Fn="ClickShowContractD" />
                        </Listeners>
                    </ext:Button>
                </Component>
                <Listeners>
                    <Bind Handler="cmp.setText(record.get('Contratos'))" />
                </Listeners>
                <Filter>
                    <ext:NumberFilter EmptyText="<%$ Resources:Comun, strNumber %>"></ext:NumberFilter>
                </Filter>
            </ext:ComponentColumn>
            <ext:DateColumn
                meta:resourceKey="strFechaActivacion"
                ID="colFechaActivacion"
                Format="<%$ Resources:Comun, FormatFecha %>"
                runat="server"
                Text="<%$ Resources:Comun, strFechaActivacion %>"
                DataIndex="FechaActivacion"
                MaxWidth="250" />
            <ext:DateColumn
                meta:resourceKey="strFechaDesactivacion"
                ID="colFechaDesactivacion"
                Format="<%$ Resources:Comun, FormatFecha %>"
                runat="server"
                Text="<%$ Resources:Comun, strFechaDesactivacion %>"
                DataIndex="FechaDesactivacion"
                MaxWidth="250" />
            <ext:Column
                meta:resourceKey="strMoneda"
                ID="colMoneda"
                runat="server"
                Text="<%$ Resources:Comun, strMoneda %>"
                DataIndex="Moneda"
                MaxWidth="250" />
            <ext:Column
                meta:resourceKey="strCategoriaSitio"
                ID="colCategoriaSitio"
                runat="server"
                Text="<%$ Resources:Comun, strCategoriaSitio %>"
                DataIndex="CategoriaSitio"
                MaxWidth="250" />
            <ext:Column
                meta:resourceKey="strTipo"
                ID="colTipo"
                runat="server"
                Text="<%$ Resources:Comun, strTipo %>"
                DataIndex="Tipo"
                MaxWidth="250" />
            <ext:Column
                meta:resourceKey="strTipoEdificio"
                ID="colTipoEdificio"
                runat="server"
                Text="<%$ Resources:Comun, strTipoEdificio %>"
                DataIndex="TipoEdificio"
                MaxWidth="250" />
            <ext:Column
                meta:resourceKey="strEstadoGlobal"
                ID="colEstadoGlobal"
                runat="server"
                Text="<%$ Resources:Comun, strEstadoGlobal %>"
                DataIndex="EstadoGlobal"
                MaxWidth="250" />
            <ext:Column
                meta:resourceKey="strTamano"
                ID="colTamano"
                runat="server"
                Text="<%$ Resources:Comun, strTamano %>"
                DataIndex="Tamano"
                MaxWidth="250" />
            <ext:Column
                meta:resourceKey="strTipoEstructura"
                ID="colTipoEstructura"
                runat="server"
                Text="<%$ Resources:Comun, strTipoEstructura %>"
                DataIndex="TipoEstructura"
                MaxWidth="250" />
        </Columns>
    </ColumnModel>
    <SelectionModel>
        <ext:CheckboxSelectionModel
            runat="server"
            ID="GridRowSelect"
            Mode="Multi">
            <Listeners>
                <Select Fn="Grid_RowSelectEmplazamiento" />
                <Deselect Fn="DeseleccionarGrilla" />
            </Listeners>
        </ext:CheckboxSelectionModel>
    </SelectionModel>
    <ViewConfig StripeRows="true">
        <Listeners>
            <RowContextMenu Fn="ShowRightClickMenu" />
        </Listeners>
    </ViewConfig>
    <Plugins>
        <ext:GridFilters runat="server"
            ID="gridFilters"
            MenuFilterText="<%$ Resources:Comun, strFiltros %>"
            meta:resourceKey="GridFilters">
        </ext:GridFilters>
        <ext:CellEditing runat="server" ClicksToEdit="2" />

    </Plugins>
    <BottomBar>

        <ext:Toolbar ID="pagin" runat="server" >
            <Items>
                <ext:Button runat="server"
                    ID="btnPagingInit"
                    Disabled="true"
                    IconCls="x-tbar-page-first">
                    <Listeners>
                        <Click Fn="pagingInitEmplazamientos" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnPagingPre"
                    Disabled="true"
                    IconCls="x-tbar-page-prev">
                    <Listeners>
                        <Click Fn="pagingPreEmplazamientos" />
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
                    HideTrigger="true" >
                    <Listeners>
                        <BeforeRender Fn="nfPaginNumberBeforeRenderEmplazamientos" />
                        <Change Fn="paginGoToEmplazamientos" />
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
                        <Click Fn="paginNextEmplazamientos" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnPaginLast"
                    Enabled="false"
                    IconCls="x-tbar-page-last">
                    <Listeners>
                        <Click Fn="paginLastEmplazamientos" />
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
                        <Select Fn="pageSelect" />
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
        <BeforeRender Fn="CargarGridEmplazamientos"></BeforeRender>
    </Listeners>
</ext:GridPanel>





