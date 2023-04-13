<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectosGestion.aspx.cs" Inherits="TreeCore.PaginasComunes.ProyectosGestion" %>

<%@ Register Src="~/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>


</head>
<body>
    <link href="css/styleProyectosProyectos.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <div>
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdProyectoSeleccionado" runat="server" />
            <ext:Hidden ID="hdProyectoFaseID" runat="server" />
            <ext:Hidden ID="hdProyectoTipoID" runat="server" />
            <ext:Hidden ID="hdProyectoProyectoTipoID" runat="server" />
            <ext:Hidden ID="hdProyectoGlobalZonasID" runat="server" />
            <ext:Hidden ID="hdProyectoEmpresasProveedorasID" runat="server" />
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <ext:Store runat="server"
                ID="storeClientes"
                AutoLoad="true"
                OnReadData="storeClientes_Refresh"
                RemoteSort="false">
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
                <%--<Listeners>
                    <Load Handler="CargarStores();" />
                </Listeners>--%>
            </ext:Store>
            <ext:Store runat="server"
                ID="storeProyectoAgrupacion"
                AutoLoad="false"
                OnReadData="storeProyectoAgrupacion_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ProyectoAgrupacionID">
                        <Fields>
                            <ext:ModelField Name="ProyectoAgrupacionID" Type="Int" />
                            <ext:ModelField Name="ProyectoAgrupacion" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoAgrupacionID" Direction="ASC" />
                </Sorters>

            </ext:Store>
            <ext:Store runat="server"
                ID="storeProyectosProyectosTipos"
                AutoLoad="false"
                OnReadData="storeProyectosProyectosTipos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ProyectoProyectoTipoID">
                        <Fields>
                            <ext:ModelField Name="ProyectoProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="ProyectoID" Type="Int" />
                            <ext:ModelField Name="Alias" />
                            <ext:ModelField Name="Activo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoProyectoTipoID" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaProyectosTipos" />
                </Listeners>
            </ext:Store>
            <ext:Store runat="server"
                ID="storeProyectosTiposLibres"
                AutoLoad="false"
                OnReadData="storeProyectosTiposLibres_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ProyectoTipoID">
                        <Fields>
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="Alias" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Alias" Direction="ASC" />
                </Sorters>

            </ext:Store>

            <ext:Store runat="server"
                ID="storeProyectosEmpresaProveedora"
                AutoLoad="false"
                OnReadData="storeProyectosEmpresaProveedora_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ProyectoEmpresaProveedoraID">
                        <Fields>
                            <ext:ModelField Name="ProyectoEmpresaProveedoraID" Type="Int" />
                            <ext:ModelField Name="ProyectoID" Type="Int" />
                            <ext:ModelField Name="EmpresaProveedoraID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Activo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarEmpresaProveedoras" />
                </Listeners>
                <Sorters>
                    <ext:DataSorter Property="ProyectoID" Direction="ASC" />
                </Sorters>

            </ext:Store>

            <ext:Store
                ID="storeFases"
                runat="server"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeFases_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ProyectoFaseID">
                        <Fields>
                            <ext:ModelField Name="ProyectoFaseID" />
                            <ext:ModelField Name="Fase" />
                            <ext:ModelField Name="ProyectoID" />

                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaFases" />
                </Listeners>

            </ext:Store>
            <ext:Store runat="server"
                ID="storeEmpresasProveedorasLibres"
                AutoLoad="false"
                OnReadData="storeEmpresasProveedorasLibres_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="EmpresaProveedoraID">
                        <Fields>
                            <ext:ModelField Name="EmpresaProveedoraID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>

            </ext:Store>

            <ext:Store runat="server"
                ID="storeProyectosGlobalZona"
                AutoLoad="false"
                OnReadData="storeProyectosGlobalZona_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ProyectoGlobalZonaID">
                        <Fields>
                            <ext:ModelField Name="ProyectoGlobalZonaID" Type="Int" />
                            <ext:ModelField Name="ProyectoID" Type="Int" />
                            <ext:ModelField Name="GlobalZonaID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Activo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaZonas" />
                </Listeners>
                <Sorters>
                    <ext:DataSorter Property="ProyectoGlobalZonaID" Direction="ASC" />
                </Sorters>

            </ext:Store>

            <ext:Store runat="server"
                ID="storeGlobalZonasLibres"
                AutoLoad="false"
                OnReadData="storeGlobalZonasLibres_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="GlobalZonaID">
                        <Fields>
                            <ext:ModelField Name="GlobalZonaID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="GlobalZonaID" Direction="ASC" />
                </Sorters>

            </ext:Store>

            <ext:Store runat="server"
                ID="storeMonedas"
                AutoLoad="false"
                OnReadData="storeMonedas_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="MonedaID">
                        <Fields>
                            <ext:ModelField Name="MonedaID" Type="Int" />
                            <ext:ModelField Name="Moneda" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Moneda" Direction="ASC" />
                </Sorters>

            </ext:Store>
            <ext:Store runat="server"
                ID="storeProyectosEstados"
                AutoLoad="false"
                OnReadData="storeProyectosEstados_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ProyectoEstadoID">
                        <Fields>
                            <ext:ModelField Name="ProyectoEstadoID" Type="Int" />
                            <ext:ModelField Name="ProyectoEstado" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoEstadoID" Direction="ASC" />
                </Sorters>

            </ext:Store>

            <ext:Window ID="winGestion"
                runat="server"
                Title="<%$ Resources:Comun, jsAgregar %>"
                Width="872"
                Height="500"
                Modal="true"
                Resizable="false"
                Centered="true"
                Cls="winForm-respSimple formularioProyectos winGestion"
                Scrollable="Vertical"
                Hidden="true">
                <Listeners>
                    <%--<AfterRender Fn="winFormResize" />--%>
                </Listeners>

                <Items>
                    <ext:Panel runat="server" ID="pnVistasForm" Cls="pnNavVistas pnVistasForm" AriaRole="navigation">
                        <Items>
                            <ext:Container runat="server" ID="cntNavVistasForm" Cls="nav-vistas">
                                <Items>
                                    <ext:HyperlinkButton runat="server" ID="lnkProyectos" Cls="lnk-navView  lnk-noLine" Text="<%$ Resources:Comun, strProyecto %>" ToolTip="<%$ Resources:Comun, strProyecto %>" Handler="NavegacionTab(0);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkProyectosTipos" Cls="lnk-navView  lnk-noLine" Text="<%$ Resources:Comun, strProyectosTipos %>" Handler="NavegacionTab(1);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkEmpresaProveedora" Cls="lnk-navView  lnk-noLine" Text="<%$ Resources:Comun, strEmpresasProveedoras %>" Handler="NavegacionTab(2);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkZonas" Cls="lnk-navView  lnk-noLine" Text="<%$ Resources:Comun, strZonas %>" Handler="NavegacionTab(3);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkFases" Cls="lnk-navView  lnk-noLine" Text="<%$ Resources:Comun, strFases %>" Handler="NavegacionTab(4);"></ext:HyperlinkButton>

                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                    <ext:FormPanel ID="pnFormProject" Cls="formGris formResp" runat="server" MinHeight="240" Hidden="false">
                        <Items>
                            <ext:Container runat="server" ID="ctFormProjects" Cls="ctForm-resp-project winGestion-panel ctForm-resp">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtNombre"
                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        ID="txtCodigo"
                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        MaxLength="5"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        ID="txtDescripcion"
                                        FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                        LabelAlign="Top"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:ComboBox runat="server"
                                        ID="cmbGrupo"
                                        FieldLabel="<%$ Resources:Comun, strGrupo %>"
                                        StoreID="storeProyectoAgrupacion"
                                        DisplayField="ProyectoAgrupacion"
                                        ValueField="ProyectoAgrupacionID"
                                        AllowBlank="true"
                                        LabelAlign="Top"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="LimpiarLista"
                                                Icon="Clear"
                                                QTip="Limpiar Lista" />
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                QTip="Recargar Lista" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbMoneda"
                                        FieldLabel="<%$ Resources:Comun, strMonedas %>"
                                        StoreID="storeMonedas"
                                        DisplayField="Moneda"
                                        ValueField="MonedaID"
                                        AllowBlank="false"
                                        LabelAlign="Top"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="LimpiarLista"
                                                Icon="Clear"
                                                QTip="Limpiar Lista" />
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                QTip="Recargar Lista" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbEstado"
                                        FieldLabel="<%$ Resources:Comun, strEstado %>"
                                        StoreID="storeProyectosEstados"
                                        DisplayField="ProyectoEstado"
                                        ValueField="ProyectoEstadoID"
                                        AllowBlank="false"
                                        LabelAlign="Top"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="LimpiarLista"
                                                Icon="Clear"
                                                QTip="Limpiar Lista" />
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                QTip="Recargar Lista" />
                                        </Triggers>
                                    </ext:ComboBox>

                                    <ext:DateField runat="server"
                                        meta:resourcekey="txtFechaInicio"
                                        ID="txtFechaInicio"
                                        FieldLabel="<%$ Resources:Comun, strFechaInicio %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Vtype="daterange"
                                        Format="<%$ Resources:Comun, FormatFecha %>">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="endDateField" Value="txtFechaFin" Mode="Value" />
                                        </CustomConfig>
                                    </ext:DateField>

                                    <ext:DateField runat="server"
                                        meta:resourcekey="txtFechaFin"
                                        ID="txtFechaFin"
                                        FieldLabel="<%$ Resources:Comun, strFechaFin %>"
                                        LabelAlign="Top"
                                        AllowBlank="true"
                                        Vtype="daterange"
                                        Format="<%$ Resources:Comun, FormatFecha %>">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="startDateField" Value="txtFechaInicio" Mode="Value" />
                                        </CustomConfig>
                                    </ext:DateField>

                                    <ext:Button runat="server"
                                        Flex="1"
                                        MinWidth="220"
                                        ID="btnMultiproceso"
                                        AllowBlank="true"
                                        EnableToggle="true"
                                        Cls="btn-toggleGrid togRedOff btnTog"
                                        Text="<%$ Resources:Comun, strMultiprocesos %>"
                                        TextAlign="Left"
                                        AriaLabel="Multiprocess"
                                        ToolTip="<%$ Resources:Comun, strMultiprocesos %>">
                                    </ext:Button>

                                </Items>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoProyecto(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                    <ext:FormPanel ID="pnPrincipalFases" Cls="formGris formResp" runat="server" MinHeight="240" Hidden="true">
                        <Items>
                            <ext:Container runat="server" ID="ctFormPhases" Cls="ctProyectosForm">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtNombreFases"
                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                        LabelAlign="Top"
                                        Cls="cmbProyectos"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        EmptyText="<%$ Resources:Comun, strFase %>" />
                                    <ext:Button runat="server" ID="btnAddPhase" IconCls="ico-addBtn" Cls="btn-mini-ppal btnAdd btnAnadirProyectos" Disabled="true">
                                        <Listeners>
                                            <Click Handler="winGestionFasesAgregarEditar()" />
                                        </Listeners>

                                    </ext:Button>
                                    <ext:GridPanel runat="server"
                                        ID="gridFases"
                                        MinHeight="200"
                                        StoreID="storeFases"
                                        Cls="grdFormElAdded"
                                        Scroll="Vertical">

                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server" ID="colFase" Text="<%$ Resources:Comun, strFase %>" DataIndex="Fase" Flex="9"></ext:Column>
                                                <ext:CommandColumn runat="server" MinWidth="70" MaxWidth="70" Align="Center" Flex="1">
                                                    <Commands>
                                                        <ext:GridCommand CommandName="EliminarProyectosFases" IconCls="ico-close"></ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Fn="EliminarProyectosFases" />
                                                    </Listeners>
                                                </ext:CommandColumn>

                                            </Columns>
                                        </ColumnModel>
                                        <View>
                                            <ext:GridView runat="server" EnableTextSelection="true">
                                            </ext:GridView>
                                        </View>
                                        <Items>
                                        </Items>

                                    </ext:GridPanel>

                                </Items>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoFases(valid);" />
                        </Listeners>

                    </ext:FormPanel>
                    <ext:FormPanel ID="pnPrincipalZonas" Cls="formGris formResp" runat="server" MinHeight="240" Hidden="true" Layout="AnchorLayout">
                        <LayoutConfig>
                            <ext:AnchorLayoutConfig DefaultAnchor="100%"></ext:AnchorLayoutConfig>
                        </LayoutConfig>

                        <Items>
                            <ext:Container runat="server" ID="Container2" Cls="ctProyectosForm">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbGlobalZonasLibres"
                                        FieldLabel="<%$ Resources:Comun, strZonas %>"
                                        LabelAlign="Top"
                                        Cls="cmbProyectos"
                                        MultiSelect="true"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        StoreID="storeGlobalZonasLibres"
                                        DisplayField="Nombre"
                                        ValueField="GlobalZonaID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <TriggerClick Fn="recargarcmbGlobalZonasLibres" />
                                        </Listeners>
                                        <Triggers>

                                            <ext:FieldTrigger meta:resourceKey="RecargarLista" Hidden="false"
                                                IconCls="ico-reload"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:Button runat="server" ID="btnAnadirZona" IconCls="ico-addBtn" Cls="btn-mini-ppal btnAdd btnAnadirProyectos" Disabled="true">
                                        <Listeners>
                                            <Click Handler="BotonGuardarGlobalZonasLibres();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:GridPanel runat="server"
                                        ID="gridProyectosZonasAsignado"
                                        MinHeight="200"
                                        StoreID="storeProyectosGlobalZona"
                                        Cls="grdFormElAdded"
                                        Scroll="Vertical">

                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server" ID="colFases" Text="<%$ Resources:Comun, strZonas %>" DataIndex="Nombre" Flex="9"></ext:Column>
                                                <ext:CommandColumn runat="server" MinWidth="70" MaxWidth="70" Align="Center" Flex="1">
                                                    <Commands>
                                                        <ext:GridCommand CommandName="EliminarProyectosGlobalZonas" IconCls="ico-close"></ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Fn="EliminarProyectosGlobalZonas" />
                                                    </Listeners>
                                                </ext:CommandColumn>

                                            </Columns>
                                        </ColumnModel>
                                        <View>
                                            <ext:GridView runat="server" EnableTextSelection="true">
                                            </ext:GridView>
                                        </View>
                                        <Items>
                                        </Items>

                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>
                        </Items>

                        <Listeners>
                            <ValidityChange Handler="FormularioValidoZonas(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                    <ext:FormPanel ID="pnPrincipalEmpresaProveedora" Cls="formGris" runat="server" MinHeight="240" Hidden="true" Layout="AnchorLayout">
                        <LayoutConfig>
                            <ext:AnchorLayoutConfig DefaultAnchor="100%"></ext:AnchorLayoutConfig>
                        </LayoutConfig>

                        <Items>
                            <ext:Container runat="server" ID="Container3" Cls="ctProyectosForm">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbEmpresasProveedorasLibres"
                                        FieldLabel="<%$ Resources:Comun, strEmpresasProveedoras %>"
                                        LabelAlign="Top"
                                        Cls="cmbProyectos"
                                        StoreID="storeEmpresasProveedorasLibres"
                                        DisplayField="Nombre"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        MultiSelect="true"
                                        ValueField="EmpresaProveedoraID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <TriggerClick Fn="recargarcmbEmpresasProveedorasLibres" />
                                        </Listeners>
                                        <Triggers>

                                            <ext:FieldTrigger meta:resourceKey="RecargarLista" Hidden="false"
                                                IconCls="ico-reload"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:Button runat="server" ID="btnAnadirEmpresaProveedoras" IconCls="ico-addBtn" Cls="btn-mini-ppal btnAdd btnAnadirProyectos" Disabled="true">
                                        <Listeners>
                                            <Click Handler="BotonGuardarEmpresaProveedoraLibres();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:GridPanel runat="server"
                                        ID="gridEmpresaProveedoraAsignada"
                                        Height="200"
                                        StoreID="storeProyectosEmpresaProveedora"
                                        Cls="grdFormElAdded"
                                        Scroll="Vertical">

                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server" ID="colNombreEmpresaProveedoraLibre" Text="<%$ Resources:Comun, strEmpresaProveedora %>" DataIndex="Nombre" Flex="9"></ext:Column>
                                                <ext:CommandColumn runat="server" MinWidth="70" MaxWidth="70" Align="End" Flex="1">
                                                    <Commands>
                                                        <ext:GridCommand CommandName="EliminarProyectosEmpresaProveedora" IconCls="ico-close"></ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Fn="EliminarProyectosEmpresaProveedora" />
                                                    </Listeners>
                                                </ext:CommandColumn>

                                            </Columns>
                                        </ColumnModel>
                                        <View>
                                            <ext:GridView runat="server" EnableTextSelection="true">
                                            </ext:GridView>
                                        </View>
                                        <Items>
                                        </Items>

                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoEmpresaProveedora(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                    <ext:FormPanel ID="pnProyectosTipos" Cls="formGris formResp" runat="server" MinHeight="240" Hidden="true" Layout="AnchorLayout">
                        <LayoutConfig>
                            <ext:AnchorLayoutConfig DefaultAnchor="100%"></ext:AnchorLayoutConfig>
                        </LayoutConfig>

                        <Items>
                            <ext:Container runat="server" ID="Container1" Cls="ctProyectosForm">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbTipoProyecto"
                                        Cls="cmbProyectos"
                                        FieldLabel="<%$ Resources:Comun, strTipoProyecto %>"
                                        LabelAlign="Top"
                                        StoreID="storeProyectosTiposLibres"
                                        DisplayField="Alias"
                                        MultiSelect="true"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        ValueField="ProyectoTipoID"
                                        Model="Local"
                                        QueryMode="Local">
                                        <Listeners>
                                            <TriggerClick Fn="recargarcmbTipoProyecto" />
                                        </Listeners>
                                        <Triggers>

                                            <ext:FieldTrigger meta:resourceKey="RecargarLista" Hidden="false"
                                                IconCls="ico-reload"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:Button runat="server" ID="btnAnadirProyectoTipo" IconCls="ico-addBtn" Cls="btn-mini-ppal btnAdd btnAnadirProyectos" Disabled="true">
                                        <Listeners>
                                            <Click Handler="BotonGuardarProyectosTipoLibres();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:GridPanel runat="server"
                                        ID="gridProyectosTiposAsignados"
                                        MinHeight="200"
                                        StoreID="storeProyectosProyectosTipos"
                                        Cls="grdFormElAdded"
                                        Scroll="Vertical">

                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column runat="server" ID="colProyectosAsignados" Text="<%$ Resources:Comun, strProyectosTipos %>" DataIndex="Alias" Flex="9"></ext:Column>
                                                <ext:CommandColumn runat="server" MinWidth="70" MaxWidth="70" Align="End" Flex="1">
                                                    <Commands>
                                                        <ext:GridCommand CommandName="EliminarProyectosProyectosTipos" IconCls="ico-close"></ext:GridCommand>
                                                    </Commands>
                                                    <Listeners>
                                                        <Command Fn="EliminarProyectoProyectoTipo" />
                                                    </Listeners>
                                                </ext:CommandColumn>

                                            </Columns>
                                        </ColumnModel>
                                        <View>
                                            <ext:GridView runat="server" EnableTextSelection="true">
                                            </ext:GridView>
                                        </View>
                                        <Items>
                                        </Items>

                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoProyectoTipos(valid);" />
                        </Listeners>

                    </ext:FormPanel>
                    <ext:Container runat="server" ID="cntBtnForm">
                        <Items>
                            <ext:Button runat="server" ID="btnPrev" Cls="btn-secondary" Text="<%$ Resources:Comun, strAnterior %>">
                                <Listeners>
                                    <Click Handler="NavegacionTab('btnPrev')" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnNext" Cls="btn-ppal" Text="<%$ Resources:Comun, strSiguiente %>">
                                <Listeners>
                                    <Click Handler="NavegacionTab('btnNext')" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnAnadirProyecto" Cls="btn-ppal " Text="<%$ Resources:Comun, strGuardar %>" Focusable="false" PressedCls="none">
                                <Listeners>
                                    <Click Handler="winGestionAgregarEditar();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Container>
                </Items>


            </ext:Window>

            <ext:Viewport ID="vwResp" runat="server" Layout="FitLayout" Flex="1" OverflowY="auto">
                <Listeners>
                    <AfterRender Handler="ControlSlider()"></AfterRender>
                    <Resize Handler="ControlSlider()"></Resize>
                </Listeners>
                <Items>
                    <ext:Panel runat="server" ID="wrapComponenteCentral" Layout="HBoxLayout" BodyCls="tbGrey" MaxWidth="1200">

                        <LayoutConfig>
                            <ext:HBoxLayoutConfig Align="Stretch"></ext:HBoxLayoutConfig>
                        </LayoutConfig>
                        <DockedItems>
                            <ext:Toolbar runat="server" ID="tbFiltrosYSliders" Dock="Top" Cls="tbGrey tbNoborder " Hidden="false" Layout="HBoxLayout" Flex="1">
                                <Items>

                                    <ext:Toolbar runat="server" ID="tbSliders" Dock="Top" Hidden="false" MinHeight="36" Cls="tbGrey tbNoborder PosUnsFloatR">
                                        <Items>

                                            <ext:Button runat="server" ID="btnPrevGrid" IconCls="ico-prev-w" Cls="btnMainSldr SliderBtn" Handler="SliderMove('Prev');" Disabled="true"></ext:Button>
                                            <ext:Button runat="server" ID="btnNextGrid" IconCls="ico-next-w" Cls="SliderBtn" Handler="SliderMove('Next');" Disabled="false"></ext:Button>

                                        </Items>
                                    </ext:Toolbar>



                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <Items>
                            <ext:GridPanel ID="grdProjects"
                                runat="server"
                                Header="false"
                                Flex="3"
                                MarginSpec="0 10 5 10"
                                HideHeaders="true"
                                SelectionMemory="false"
                                Region="Center"
                                Hidden="false"
                                Title="Suppliers"
                                Cls="gridPanel grdNoHeader grdPnColIcons "
                                OverflowX="Hidden"
                                OverflowY="Auto">
                                <DockedItems>
                                    <ext:Toolbar runat="server" ID="tlbBase" Dock="top" Cls="tlbGrid" OverflowHandler="Scroller">
                                        <Items>
                                            <ext:Button runat="server" ID="btnAnadir" Cls="btnAnadir" AriaLabel="Añadir" ToolTip="<%$ Resources:Comun, strAnadir %>">
                                                <Listeners>
                                                    <Click Fn="AgregarEditar" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server" ID="btnEditar" Cls="btnEditar" AriaLabel="Editar" ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>" Disabled="true">
                                                <Listeners>
                                                    <Click Fn="MostrarEditar" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server" ID="btnEliminar" Cls="btnEliminar" AriaLabel="Eliminar" ToolTip="<%$ Resources:Comun,  btnEliminar.ToolTip %>" Disabled="true">
                                                <Listeners>
                                                    <Click Fn="Eliminar" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server" ID="btnRefrescar" Cls="btnRefrescar" AriaLabel="Refrescar" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>">
                                                <Listeners>
                                                    <Click Handler="refrescar()" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server" ID="btnDescargar" Cls="btnDescargar" AriaLabel="Descargar" ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                Handler="ExportarDatos('ProyectosGestion', hdCliID.value, #{grdProjects}, App.btnActivo.pressed, '', '');">
                                            </ext:Button>
                                            <ext:Button runat="server" ID="btnDuplicar" Cls="btnDuplicar" AriaLabel="Duplicar Registro" ToolTip="<%$ Resources:Comun, btnDuplicar.ToolTip %>" Disabled="true">
                                                <Listeners>
                                                    <Click Handler="Duplicar()" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnActivarProyecto"
                                                Cls="btnActivar"
                                                Disabled="true"
                                                ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                Handler="Activar();">
                                            </ext:Button>
                                            <ext:Label runat="server"
                                                ID="lblToggle"
                                                Cls="lblBtnActivo"
                                                PaddingSpec="0 0 25 0"
                                                Text="<%$ Resources:Comun, strActivo %>" />
                                            <ext:Button runat="server"
                                                ID="btnActivo"
                                                Width="41"
                                                Pressed="true"
                                                EnableToggle="true"
                                                Cls="btn-toggleGrid"
                                                ToolTip="<%$ Resources:Comun, strActivo %>"
                                                Handler="refrescar();" />

                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top">
                                        <Content>

                                            <local:toolbarFiltros
                                                ID="cmpFiltro"
                                                runat="server"
                                                Stores="storeProyectos"
                                                MostrarComboFecha="false"
                                                FechaDefecto="Dia"
                                                Grid="grdProjects"
                                                MostrarBusqueda="false"
                                                QuitarFiltros="true" />

                                        </Content>
                                    </ext:Container>
                                </DockedItems>
                                <Store>
                                    <ext:Store
                                        ID="storeProyectos"
                                        runat="server"
                                        RemotePaging="false"
                                        AutoLoad="true"
                                        OnReadData="storeProyectos_Refresh"
                                        RemoteSort="true"
                                        PageSize="20"
                                        shearchBox="cmpFiltro_txtSearch"
                                        listNotPredictive="ProyectoID,FechaFin,FechaInicio">
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model runat="server" IDProperty="ProyectoID">
                                                <Fields>
                                                    <ext:ModelField Name="ProyectoID" />
                                                    <ext:ModelField Name="Proyecto" />
                                                    <ext:ModelField Name="Descripcion" />
                                                    <ext:ModelField Name="ProyectoAgrupacion" />
                                                    <ext:ModelField Name="Referencia" />
                                                    <ext:ModelField Name="Multiflujo" />
                                                    <ext:ModelField Name="Activo" />
                                                    <ext:ModelField Name="FechaInicio" Type="Date" />
                                                    <ext:ModelField Name="FechaFin" Type="Date" />
                                                    <%--<ext:ModelField Name="Average" />--%>
                                                    <ext:ModelField Name="Cerrado" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Listeners>
                                            <DataChanged Fn="BuscadorPredictivo" />
                                            <BeforeLoad Fn="DeseleccionarGrilla" />
                                        </Listeners>
                                        <Sorters>
                                            <ext:DataSorter Property="Proyecto" Direction="ASC" />
                                        </Sorters>
                                    </ext:Store>

                                </Store>

                                <ColumnModel runat="server">
                                    <Columns>
                                        <ext:Column runat="server" Hidden="true" Text="<%$ Resources:Comun, strNombre %>" DataIndex="Proyecto" />
                                        <ext:Column runat="server" Hidden="true" Text="<%$ Resources:Comun, strDescripcion %>" DataIndex="Descripcion" />
                                        <ext:Column runat="server" Hidden="true" Text="<%$ Resources:Comun, strGrupo %>" DataIndex="ProyectoAgrupacion" />
                                        <ext:Column runat="server" Hidden="true" Text="<%$ Resources:Comun, strCodigo %>" DataIndex="Referencia" />
                                        <ext:Column runat="server" Hidden="true" Text="<%$ Resources:Comun, strMultiprocesos %>" DataIndex="Multiflujo" />
                                        <ext:Column runat="server" Hidden="true" Text="<%$ Resources:Comun, strFechaInicio %>" DataIndex="FechaInicio" />
                                        <ext:Column runat="server" Hidden="true" Text="<%$ Resources:Comun, strFechaFin %>" DataIndex="FechaFin" />
                                        <ext:Column runat="server" Hidden="true" Text="<%$ Resources:Comun, strCerrado %>" DataIndex="Cerrado" />
                                        <ext:Column runat="server" Hidden="true" Text="<%$ Resources:Comun, strActivo %>" DataIndex="Activo" />

                                        <ext:TemplateColumn runat="server" DataIndex="" ID="tempCol2" MenuDisabled="true" Text="" Flex="1">

                                            <Template runat="server">
                                                <Html>

                                                    <tpl for=".">

														</tpl>
                                                </Html>
                                            </Template>
                                            <Renderer Fn="asignarRender" />
                                        </ext:TemplateColumn>
                                    </Columns>
                                </ColumnModel>

                                <SelectionModel>
                                    <ext:RowSelectionModel runat="server" ID="GridRowSelect" PruneRemoved="false" Mode="Single">
                                        <Listeners>
                                            <Select Fn="Grid_RowSelect" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <BottomBar>
                                    <ext:PagingToolbar runat="server"
                                        ID="PagingToolBar"
                                        meta:resourceKey="PagingToolBar"
                                        StoreID="storeProyectos"
                                        HideRefresh="true"
                                        OverflowHandler="Scroller">
                                        <Items>
                                            <ext:ComboBox runat="server"
                                                Cls="comboGrid"
                                                Width="60">
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

                            <ext:Panel runat="server" ID="pnCol1" Flex="2" Layout="VBoxLayout" BodyCls="tbGrey">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                </LayoutConfig>
                                <Items>
                                    <ext:GridPanel ID="gridProyectosTipos"
                                        Title="<%$ Resources:Comun, strProyectosTipos %>"
                                        runat="server"
                                        Flex="1"
                                        MarginSpec="0 10 5 10"
                                        HideHeaders="true"
                                        Region="Center"
                                        SelectionMemory="false"
                                        StoreID="storeProyectosProyectosTipos"
                                        Hidden="false"
                                        Cls="gridPanel "
                                        OverflowX="Hidden"
                                        OverflowY="Auto">

                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="Toolbar5" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnAnadirProyectosTipos"
                                                        meta:resourceKey="btnAnadir"
                                                        Cls="btnAnadir"
                                                        Disabled="true"
                                                        AriaLabel="Añadir"
                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                        Handler="AgregarEditarProyectosTipos();">
                                                    </ext:Button>

                                                    <ext:Button runat="server"
                                                        ID="btnEliminarProyectosTipos"
                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                        meta:resourceKey="btnEliminar"
                                                        Disabled="true"
                                                        Cls="btnEliminar"
                                                        Handler="EliminarProyectoTipo();" />
                                                    <ext:Button runat="server"
                                                        ID="btnRefrescarProyectoTipo"
                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                        meta:resourceKey="btnRefrescar"
                                                        Disabled="true"
                                                        Cls="btnRefrescar"
                                                        Handler="RefrescarProyectosTiposAsignados();" />
                                                    <ext:Button runat="server"
                                                        ID="btnActivar"
                                                        Cls="btnActivar"
                                                        Disabled="true"
                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                        Handler="ActivarProyectoTipo();">
                                                    </ext:Button>

                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <ColumnModel>
                                            <Columns>


                                                <ext:Column runat="server" Text="<%$ Resources:Comun, strNombre %>" MinWidth="120" Selectable="false" DataIndex="Alias" Flex="8" ID="colNombre">
                                                </ext:Column>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colActivo"
                                                    DataIndex="Activo"
                                                    Cls="col-activo"
                                                    Selectable="false"
                                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                    meta:resourceKey="colActivo"
                                                    MinWidth="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>

                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server" ID="RowSelectProyectoTipo" PruneRemoved="false" Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelectProyectoTipo" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>

                                    </ext:GridPanel>

                                    <ext:GridPanel ID="grdZonas"
                                        runat="server"
                                        Title="<%$ Resources:Comun, strZonas %>"
                                        Flex="1"
                                        MarginSpec="0 10 5 10"
                                        HideHeaders="true"
                                        Region="Center"
                                        Hidden="false"
                                        SelectionMemory="false"
                                        StoreID="storeProyectosGlobalZona"
                                        Cls="gridPanel "
                                        OverflowX="Hidden"
                                        OverflowY="Auto">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tlbBaseSuppliers" Dock="Top" Cls="tlbGrid">
                                                <Items>
                                                    <ext:Button runat="server" ID="btnAnadirGlobalZonas" Cls="btnAnadir" AriaLabel="Añadir" Disabled="true" ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>" Handler="AgregarEditarZonas()"></ext:Button>
                                                    <ext:Button runat="server" ID="btnEliminarGlobalZonas" Cls="btnEliminar" AriaLabel="Eliminar" Disabled="true" ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>" Handler="EliminarGlobalZonas()"></ext:Button>
                                                    <ext:Button runat="server" ID="btnRefrescarGlobalZonas" Cls="btnRefrescar" AriaLabel="Refrescar" Disabled="true" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>" Handler="RefrescarGlobalZonas()"></ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnActivarGlobalZonas"
                                                        Cls="btnActivar"
                                                        Disabled="true"
                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                        Handler="ActivarGlobalZonas();">
                                                    </ext:Button>

                                                </Items>
                                            </ext:Toolbar>

                                        </DockedItems>

                                        <ColumnModel>
                                            <Columns>


                                                <ext:Column runat="server" Text="<%$ Resources:Comun, strNombre %>" MinWidth="120" Selectable="false" DataIndex="Nombre" Flex="8" ID="colNombreZona">
                                                </ext:Column>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colActivoZonas"
                                                    DataIndex="Activo"
                                                    Cls="col-activo"
                                                    Selectable="false"
                                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                    meta:resourceKey="colActivo"
                                                    MinWidth="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>

                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server" ID="RowSelectGlobalZonas" PruneRemoved="false" Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelectGlobalZonas" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>

                                    </ext:GridPanel>

                                </Items>
                            </ext:Panel>

                            <ext:Panel runat="server" ID="pnCol2" Flex="2" Layout="VBoxLayout" BodyCls="tbGrey">
                                <LayoutConfig>
                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                </LayoutConfig>
                                <Items>

                                    <ext:GridPanel ID="gridEmpresaProveedora"
                                        Title="<%$ Resources:Comun, strEmpresasProveedoras %>"
                                        runat="server"
                                        Flex="1"
                                        MarginSpec="0 10 5 10"
                                        StoreID="storeProyectosEmpresaProveedora"
                                        HideHeaders="true"
                                        Region="Center"
                                        SelectionMemory="false"
                                        Hidden="false"
                                        Cls="gridPanel"
                                        OverflowX="Hidden"
                                        OverflowY="Auto">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tlbBaseZone" Dock="Top" Cls="tlbGrid">
                                                <Items>
                                                    <ext:Button runat="server" ID="btnAnadirEmpresaProveedora" Cls="btnAnadir" AriaLabel="Añadir" Disabled="true" ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>" Handler="AgregarEditarEmpresaProveedora()"></ext:Button>
                                                    <ext:Button runat="server" ID="btnEliminarEmpresaProveedora" Cls="btnEliminar" AriaLabel="Eliminar" Disabled="true" ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>" Handler="EliminarEmpresaProveedora()"></ext:Button>
                                                    <ext:Button runat="server" ID="btnRefrescarEmpresaProveedora" Cls="btnRefrescar" AriaLabel="Refrescar" Disabled="true" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>" Handler="RefrescarEmpresasProveedoras()"></ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnActivarEmpresaProveedora"
                                                        Cls="btnActivar"
                                                        Disabled="true"
                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                        Handler="ActivarEmpresaProveedora();">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>

                                        </DockedItems>

                                        <ColumnModel>
                                            <Columns>

                                                <ext:Column runat="server" Text="<%$ Resources:Comun, strNombre %>" MinWidth="120" Selectable="false" DataIndex="Nombre" Flex="8" ID="colNombreEmpresaProveedora">
                                                </ext:Column>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colActivoEmpresaProveedora"
                                                    DataIndex="Activo"
                                                    Cls="col-activo"
                                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                    meta:resourceKey="colActivo"
                                                    MinWidth="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>

                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server" ID="RowSelectionModelEmpresaProveedora" PruneRemoved="false" Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelectEmpresaProveedora" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>

                                    </ext:GridPanel>
                                    <ext:GridPanel ID="grdPhases"
                                        runat="server"
                                        Title="<%$ Resources:Comun, strFase %>"
                                        Flex="1"
                                        MarginSpec="0 10 5 10"
                                        HideHeaders="true"
                                        Region="Center"
                                        SelectionMemory="false"
                                        Hidden="false"
                                        StoreID="storeFases"
                                        Cls="gridPanel  "
                                        OverflowY="Auto">
                                        <ColumnModel runat="server">
                                            <Columns>

                                                <ext:TemplateColumn runat="server" DataIndex="ProyectoFaseID" ID="tempCol3" MenuDisabled="true" Text="" Flex="1">

                                                    <Template runat="server">
                                                        <Html>

                                                            <tpl for=".">
												                                 <div class="d-flx">
                                                                                      <ul class="ulGrid"> 
                                                                                        <li><span>{Fase}</span></li>
                                                                                          
                                                                                      </ul>
                                                                                                                                                                        
                                                                                   </div>
                               
														</tpl>

                                                        </Html>
                                                    </Template>
                                                </ext:TemplateColumn>
                                            </Columns>
                                        </ColumnModel>

                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server" ID="gridRowSelectFases" PruneRemoved="false" Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelectFases" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tlbBasePhases" Dock="Top" Cls="tlbGrid">
                                                <Items>
                                                    <ext:Button runat="server" ID="btnAnadirFases" Cls="btnAnadir" AriaLabel="<%$ Resources:Comun, btnAnadir.ToolTip %>" ToolTip="<%$ Resources:Comun, strAnadir %>" Disabled="true">
                                                        <Listeners>
                                                            <Click Fn="AgregarEditarFases" />
                                                        </Listeners>
                                                    </ext:Button>

                                                    <ext:Button runat="server" ID="btnEliminarFases" Cls="btnEliminar" AriaLabel="Eliminar" ToolTip="<%$ Resources:Comun,  btnEliminar.ToolTip %>" Disabled="true">
                                                        <Listeners>
                                                            <Click Fn="EliminarFases" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server" ID="btnRefrescarFases" Cls="btnRefrescar" AriaLabel="Refrescar" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="refrescarFases()" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>

                                        </DockedItems>

                                    </ext:GridPanel>


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
