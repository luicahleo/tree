<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmplazamientosContenedor.aspx.cs" Inherits="TreeCore.ModGlobal.pages.EmplazamientosContenedor" Async="true" %>

<%@ Register Src="/Componentes/Geoposicion.ascx" TagName="Geoposicion" TagPrefix="local" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>

<body>
    <script src="//maps.googleapis.com/maps/api/js?key=AIzaSyA0k02bRAAmDy-4x0kcN06k1auhLHKGRZs&v=3&libraries=places" type="text/javascript"></script>
    <script src="https://unpkg.com/@google/markerclustererplus@4.0.1/dist/markerclustererplus.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="/Componentes/js/Geoposicion.js"></script>
    <script type="text/javascript" src="/Componentes/js/FormGestionElementos.js"></script>
    <script type="text/javascript" src="/Componentes/js/GestionCategoriasAtributos.js"></script>
    <script type="text/javascript" src="/Componentes/js/GestionAtributos.js"></script>

    <link href="/Modulos/Inventory/css/styleInventarioCategorias.css" rel="stylesheet" type="text/css" />
    <link href="/Componentes/css/Geoposicion.css" rel="stylesheet" type="text/css" />
    <link href="css/styleEmplazamientos.css" rel="stylesheet" type="text/css" />
    <link href="/Componentes/css/FormEmplazamientos.css" rel="stylesheet" type="text/css" />

    <form id="form1" runat="server">
        <div>
            <ext:Hidden runat="server" ID="hdPageLoad" />
            <ext:Hidden ID="hdCliID" runat="server" Name="hdClienteIDComponente" />
            <ext:Hidden ID="hdFiltrosAplicados" runat="server">
                <Listeners>
                    <Change Fn="updateHdFiltrosAplicadosHijos" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdStringBuscador" runat="server">
                <Listeners>
                    <Change Fn="filtrarEmplazamientosPorBuscador" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdIDEmplazamientoBuscador" runat="server">
                <Listeners>
                    <Change Fn="filtrarEmplazamientosPorBuscador" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdidsResultados" runat="server" />
            <ext:Hidden ID="hdnameIndiceID" runat="server" />


            <%--HIDDEN VARS FORMULARIO--%>
            <ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteIDComponente" />
            <ext:Hidden ID="hdEmplazamientoID" runat="server" />
            <ext:Hidden ID="hdCodigoEmplazamientoAutogenerado" runat="server" />
            <ext:Hidden ID="hdCondicionReglaID" runat="server" />
            <ext:Hidden runat="server" ID="hdVistaPlantilla" />
            <ext:Hidden ID="hdCodigoDuplicado" runat="server" />
            <ext:Hidden ID="hdHistoricoEmplazamiento" runat="server" />
            <ext:Hidden ID="hdAdicionalCargado" runat="server" />
            <ext:Hidden ID="hdAllowBlank" runat="server" />

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <DocumentReady Fn="bindInitialFilter" />
                </Listeners>
            </ext:ResourceManager>


            <%--VENTANAS EMERGENTES--%>

            <ext:Window runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Modal="true"
                Width="750"
                Height="600"
                Closable="false"
                Hidden="true"
                Layout="FitLayout"
                Cls="winForm-resp">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="containerFormEmplazamiento"
                        WidthSpec="100%"
                        OverflowY="Auto"
                        Cls="formGris formResp"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Container runat="server"
                                ID="pnVistasFormEmplazamiento"
                                MonitorPoll="500"
                                MonitorValid="true"
                                ActiveIndex="2"
                                Border="false"
                                Cls="pnNavVistas pnVistasForm"
                                Height="50"
                                AriaRole="navigation">
                                <Items>
                                    <ext:Container runat="server"
                                        ID="cntNavVistasFormEmplazamiento"
                                        Cls="nav-vistas ctForm-tab-resp-col3"
                                        ActiveIndex="1"
                                        Height="50"
                                        ActiveItem="1">
                                        <Items>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkFormSite"
                                                meta:resourceKey="lnkSite"
                                                Cls="lnk-navView lnk-noLine navActivo"
                                                Text="<%$ Resources:Comun, strEmplazamiento %>">
                                                <Listeners>
                                                    <Click Fn="showFormsFormEmplazamientos" />
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkFormLocation"
                                                meta:resourceKey="lnkLocation"
                                                Cls="lnk-navView lnk-noLine"
                                                Text="<%$ Resources:Comun, strLocalizacion %>">
                                                <Listeners>
                                                    <Click Fn="showFormsFormEmplazamientos" />
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkFormAdditional"
                                                meta:resourceKey="lnkAdditional"
                                                Cls="lnk-navView  lnk-noLine"
                                                Text="<%$ Resources:Comun, strAdicional %>">
                                                <Listeners>
                                                    <Click Fn="showFormsFormEmplazamientos" />
                                                </Listeners>
                                            </ext:HyperlinkButton>

                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="formSite"
                                Hidden="false"
                                Scrollable="Vertical"
                                Height="400"
                                OverflowY="Auto"
                                Cls="winGestion-paneles ctForm-resp ctForm-resp-col3">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtCodigo"
                                        FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                        MaxLength="100"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        Regex="/^[^$%&|<>/\#]*$/"
                                        RegexText="<%$ Resources:Comun, regexNombreText %>"
                                        LabelAlign="Top"
                                        Cls="item-form ico-exclamacion-10px-grey"
                                        AllowBlank="false"
                                        ValidationGroup="FORM">
                                    </ext:TextField>
                                    <ext:TextField runat="server"
                                        ID="txtNombre"
                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                        MaxLength="100"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        LabelAlign="Top"
                                        Cls="item-form ico-exclamacion-10px-grey"
                                        AllowBlank="false"
                                        ValidationGroup="FORM">
                                    </ext:TextField>
                                    <ext:ComboBox runat="server"
                                        ID="cmbOperadores"
                                        FieldLabel="<%$ Resources:Comun, strOperador %>"
                                        LabelAlign="Top"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        DisplayField="Nombre"
                                        AllowBlank="false"
                                        ValueField="EntidadID"
                                        Cls="item-form comboForm ico-exclamacion-10px-grey"
                                        Mode="Local"
                                        QueryMode="Local">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="Tabla" Value="Entidades" />
                                        </CustomConfig>
                                        <Store>
                                            <ext:Store ID="storeOperadores"
                                                runat="server"
                                                AutoLoad="false"
                                                OnReadData="storeOperadores_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server"
                                                        IDProperty="EntidadID">
                                                        <Fields>
                                                            <ext:ModelField Name="EntidadID" />
                                                            <ext:ModelField Name="Nombre" />
                                                            <ext:ModelField Name="Codigo" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEmplazamiento" />
                                            <TriggerClick Fn="RecargarComboEmplazamiento" />
                                            <Change Fn="FormEmplazamientosValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbEstadosGlobales"
                                        FieldLabel="<%$ Resources:Comun, strEstadoGlobal %>"
                                        LabelAlign="Top"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        DisplayField="EstadoGlobal"
                                        ValueField="EstadoGlobalID"
                                        Cls="item-form comboForm ico-exclamacion-10px-grey"
                                        AllowBlank="false"
                                        Mode="Local"
                                        QueryMode="Local"
                                        ValidationGroup="FORM">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="Tabla" Value="EstadosGlobales" />
                                        </CustomConfig>
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeEstadosGlobales"
                                                AutoLoad="false"
                                                OnReadData="storeEstadosGlobales_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server"
                                                        IDProperty="EstadoGlobalID">
                                                        <Fields>
                                                            <ext:ModelField Name="EstadoGlobalID" Type="Int" />
                                                            <ext:ModelField Name="EstadoGlobal" Type="String" />
                                                            <ext:ModelField Name="Codigo" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEmplazamiento" />
                                            <TriggerClick Fn="RecargarComboEmplazamiento" />
                                            <Change Fn="FormEmplazamientosValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbMonedas"
                                        FieldLabel="<%$ Resources:Comun, strMoneda %>"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        LabelAlign="Top"
                                        DisplayField="Moneda"
                                        ValueField="MonedaID"
                                        Mode="Local"
                                        QueryMode="Local"
                                        Cls="item-form comboForm ico-exclamacion-10px-grey"
                                        AllowBlank="false"
                                        ValidationGroup="FORM">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="Tabla" Value="Monedas" />
                                        </CustomConfig>
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeMonedas"
                                                AutoLoad="false"
                                                OnReadData="storeMonedas_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server"
                                                        IDProperty="MonedaID">
                                                        <Fields>
                                                            <ext:ModelField Name="MonedaID" Type="Int" />
                                                            <ext:ModelField Name="Moneda" Type="String" />
                                                            <ext:ModelField Name="Simbolo" Type="String" />
                                                            <ext:ModelField Name="Codigo" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEmplazamiento" />
                                            <TriggerClick Fn="RecargarComboEmplazamiento" />
                                            <Change Fn="FormEmplazamientosValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbCategorias"
                                        FieldLabel="<%$ Resources:Comun, strCategoria %>"
                                        LabelAlign="Top"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        DisplayField="CategoriaSitio"
                                        ValueField="EmplazamientoCategoriaSitioID"
                                        Cls="item-form comboForm ico-exclamacion-10px-grey"
                                        AllowBlank="false"
                                        Mode="Local"
                                        QueryMode="Local"
                                        ValidationGroup="FORM">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="Tabla" Value="EmplazamientosCategoriasSitios" />
                                        </CustomConfig>
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeCategorias"
                                                AutoLoad="false"
                                                OnReadData="storeCategorias_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server"
                                                        IDProperty="EmplazamientoCategoriaSitioID">
                                                        <Fields>
                                                            <ext:ModelField Name="EmplazamientoCategoriaSitioID" Type="Int" />
                                                            <ext:ModelField Name="CategoriaSitio" Type="String" />
                                                            <ext:ModelField Name="Codigo" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEmplazamiento" />
                                            <TriggerClick Fn="RecargarComboEmplazamiento" />
                                            <Change Fn="FormEmplazamientosValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbTipos"
                                        FieldLabel="<%$ Resources:Comun, strTipo %>"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        LabelAlign="Top"
                                        Mode="Local"
                                        QueryMode="Local"
                                        DisplayField="Tipo"
                                        ValueField="EmplazamientoTipoID"
                                        Cls="item-form comboForm ico-exclamacion-10px-grey"
                                        AllowBlank="false"
                                        ValidationGroup="FORM">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="Tabla" Value="EmplazamientosTipos" />
                                        </CustomConfig>
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeTipos"
                                                AutoLoad="false"
                                                OnReadData="storeTipos_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server"
                                                        IDProperty="EmplazamientoTipoID">
                                                        <Fields>
                                                            <ext:ModelField Name="EmplazamientoTipoID" Type="Int" />
                                                            <ext:ModelField Name="Tipo" Type="String" />
                                                            <ext:ModelField Name="Codigo" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEmplazamiento" />
                                            <TriggerClick Fn="RecargarComboEmplazamiento" />
                                            <Change Fn="FormEmplazamientosValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbTiposEdificios"
                                        FieldLabel="<%$ Resources:Comun, strTipoEdificio %>"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        LabelAlign="Top"
                                        Mode="Local"
                                        QueryMode="Local"
                                        DisplayField="TipoEdificio"
                                        ValueField="EmplazamientoTipoEdificioID"
                                        Cls="item-form comboForm ico-exclamacion-10px-grey"
                                        AllowBlank="false"
                                        ValidationGroup="FORM">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="Tabla" Value="EmplazamientosTiposEdificios" />
                                        </CustomConfig>
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeTiposEdificios"
                                                AutoLoad="false"
                                                OnReadData="storeTiposEdificios_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server"
                                                        IDProperty="EmplazamientoTipoEdificioID">
                                                        <Fields>
                                                            <ext:ModelField Name="EmplazamientoTipoEdificioID" Type="Int" />
                                                            <ext:ModelField Name="TipoEdificio" Type="String" />
                                                            <ext:ModelField Name="Codigo" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEmplazamiento" />
                                            <TriggerClick Fn="RecargarComboEmplazamiento" />
                                            <Change Fn="FormEmplazamientosValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbTiposEstructuras"
                                        FieldLabel="<%$ Resources:Comun, strTipoEstructura %>"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        LabelAlign="Top"
                                        DisplayField="TipoEstructura"
                                        Mode="Local"
                                        QueryMode="Local"
                                        ValueField="EmplazamientoTipoEstructuraID"
                                        Cls="item-form comboForm"
                                        AllowBlank="true"
                                        ValidationGroup="FORM">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="Tabla" Value="EmplazamientosTiposEstructuras" />
                                        </CustomConfig>
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeTiposEstructuras"
                                                AutoLoad="false"
                                                OnReadData="storeTiposEstructuras_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server"
                                                        IDProperty="EmplazamientoTipoEstructuraID">
                                                        <Fields>
                                                            <ext:ModelField Name="EmplazamientoTipoEstructuraID" Type="Int" />
                                                            <ext:ModelField Name="TipoEstructura" Type="String" />
                                                            <ext:ModelField Name="Codigo" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEmplazamiento" />
                                            <TriggerClick Fn="RecargarComboEmplazamiento" />
                                            <Change Fn="FormEmplazamientosValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbTamanos"
                                        LabelAlign="Top"
                                        FieldLabel="<%$ Resources:Comun, strTamano %>"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        DisplayField="Tamano"
                                        Mode="Local"
                                        QueryMode="Local"
                                        ValueField="EmplazamientoTamanoID"
                                        Cls="item-form comboForm"
                                        AllowBlank="true"
                                        ValidationGroup="FORM">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="Tabla" Value="EmplazamientosTamanos" />
                                        </CustomConfig>
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeTamanos"
                                                AutoLoad="false"
                                                OnReadData="storeTamanos_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server"
                                                        IDProperty="EmplazamientoTamanoID">
                                                        <Fields>
                                                            <ext:ModelField Name="EmplazamientoTamanoID" Type="Int" />
                                                            <ext:ModelField Name="Tamano" Type="String" />
                                                            <ext:ModelField Name="Codigo" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarComboEmplazamiento" />
                                            <TriggerClick Fn="RecargarComboEmplazamiento" />
                                            <Change Fn="FormEmplazamientosValido" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:DateField runat="server"
                                        ID="dateActivation"
                                        FieldLabel="<%$ Resources:Comun, strFechaActivacion %>"
                                        LabelAlign="Top"
                                        FormatText=""
                                        LabelWidth="125"
                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                        Cls="item-form dateField ico-exclamacion-10px-grey"
                                        InvalidText="<%$ Resources:Comun, strFechaFormatoIncorrecto %>"
                                        AllowBlank="false"
                                        Vtype="daterange"
                                        ValidationGroup="FORM">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="endDateField" Value="dateDeactivation" Mode="Value" />
                                        </CustomConfig>
                                    </ext:DateField>
                                    <ext:DateField runat="server"
                                        ID="dateDeactivation"
                                        FieldLabel="<%$ Resources:Comun, strFechaDesactivacion %>"
                                        LabelAlign="Top"
                                        LabelWidth="125"
                                        FormatText=""
                                        InvalidText="<%$ Resources:Comun, strFechaFormatoIncorrecto %>"
                                        Format="<%$ Resources:Comun, FormatFecha %>"
                                        Cls="item-form dateField"
                                        AllowBlank="true"
                                        Vtype="daterange"
                                        ValidationGroup="FORM">
                                        <CustomConfig>
                                            <ext:ConfigItem Name="startDateField" Value="dateActivation" Mode="Value" />
                                        </CustomConfig>
                                    </ext:DateField>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="formLocation"
                                Hidden="true"
                                OverflowY="Auto"
                                Cls="winGestion-paneles winGestionLoc-panel">
                                <Items>
                                    <ext:FieldContainer runat="server"
                                        ID="ctLocGeografica"
                                        Cls="ctForm-resp ctForm-content2-resp-col3 ctLocGeografica">
                                        <Content>
                                            <local:Geoposicion ID="geoEmplazamiento"
                                                runat="server"
                                                Localizacion="false"
                                                Contactos="false"
                                                NombreMapa="mapEmplazamientos" />
                                        </Content>
                                    </ext:FieldContainer>
                                </Items>
                                <Listeners>
                                    <AfterRender Fn="addlistenerValidacionFormEmplazamientos" />
                                </Listeners>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="formAdditional"
                                Scrollable="Vertical"
                                Hidden="true"
                                Height="400"
                                Cls="winGestion-paneles ctForm-resp">
                                <Items>
                                    <ext:FieldContainer runat="server"
                                        ID="contenedorCategorias"
                                        Cls="containerAttributes"
                                        Height="80">
                                        <Items>
                                        </Items>
                                    </ext:FieldContainer>
                                </Items>
                            </ext:Container>
                        </Items>
                        <DockedItems>
                            <ext:Toolbar runat="server" Dock="Bottom" Cls="formGris">
                                <Items>
                                    <ext:ToolbarFill />
                                    <%--<ext:Button runat="server"
                                        ID="btnPrevEmplazamiento"
                                        meta:resourceKey="btnPrev"
                                        Cls="btn-secondary-winForm"
                                        Text="<%$ Resources:Comun, strAnterior %>">
                                        <Listeners>
                                            <Click Fn="btnPrevEmplazamiento" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnNextEmplazamiento"
                                        meta:resourceKey="btnNext"
                                        Cls="btn-ppal-winForm"
                                        Text="<%$ Resources:Comun, strSiguiente %>">
                                        <Listeners>
                                            <Click Fn="btnNextEmplazamiento" />
                                        </Listeners>
                                    </ext:Button>--%>
                                    <ext:Button runat="server"
                                        ID="btnCancelarAgregarEditarEmplazamiento"
                                        Text="<%$ Resources:Comun, jsCerrar %>"
                                        IconCls="ico-close"
                                        Width="100px"
                                        Focusable="false"
                                        Cls="btn-secondary-winForm">
                                        <Listeners>
                                            <Click Fn="cerrarWindow" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnGuardarAgregarEditarEmplazamiento"
                                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                        Cls="btn-ppal-winForm"
                                        Width="100px"
                                        Focusable="false"
                                        Disabled="true">
                                        <Listeners>
                                            <Click Fn="guardarCambios" />
                                        </Listeners>
                                    </ext:Button>

                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                    </ext:FormPanel>
                </Items>
                <Listeners>
                    <AfterRender Handler="CargarStores()" />
                    <BeforeShow Fn="CargarPanelAdicional" />
                    <Show Fn="VaciarFormularioEmplazamiento" />
                </Listeners>
            </ext:Window>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Cls=""
                Layout="FitLayout">
                <Content>
                    <ext:Button runat="server" ID="btnCollapseAsR" Cls="btn-trans btnCollapseAsRClosedv2" Hidden="false"
                        ToolTip="<%$ Resources:Comun, strAbrirMenu %>">
                        <Listeners>
                            <Click Fn="hidePnFilters" />
                            <AfterRender Handler="document.getElementById('btnCollapseAsR').style.transform = 'rotate(-180deg)';" />
                        </Listeners>
                    </ext:Button>
                </Content>
                <Listeners>
                    <Resize Handler="ResizerAside()"></Resize>
                </Listeners>
                <Items>
                    <%--<ext:Button
                        runat="server"
                        ID="btnCollapseAsR"
                        Cls="btn-trans"
                        Handler="hidePnFilters();">
                    </ext:Button>--%>
                    <ext:Panel ID="pnContenedor"
                        runat="server"
                        Header="false"
                        Cls=""
                        Layout="BorderLayout">

                        <Items>

                            <%-------------TENER EN CUENTA QUE AL CENTER MAIN SE LE PUEDEN ACOPLAR SEGMENTOS PLEGABLES CON LAS "REGIONS"-------------%>


                            <%-- PANEL CENTRAL CON SLIDERS--%>


                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                PaddingSpec="20 0 20 20"
                                Border="false"
                                Region="Center"
                                Layout="HBoxLayout"
                                Cls="visorInsidePn bckGris">
                                <LayoutConfig>
                                    <ext:HBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <DockedItems>
                                    <ext:Toolbar runat="server" ID="TbNavegacionTabs" Cls="tbGrey" Dock="Top" Hidden="false" MinHeight="36">
                                        <Items>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkSites"
                                                Cls="lnk-navView lnk-noLine navActivo"
                                                Text="<%$ Resources:Comun, strEmplazamiento %>">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkLocation"
                                                Cls="lnk-navView lnk-noLine "
                                                Text="<%$ Resources:Comun, strLocalizacion %>">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkAtributos"
                                                Cls="lnk-navView lnk-noLine "
                                                Text="<%$ Resources:Comun, strAdicional %>">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkMaps"
                                                Cls="lnk-navView lnk-noLine"
                                                Text="<%$ Resources:Comun, strMapa %>">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                Hidden="true"
                                                ID="lnkDocumentos"
                                                Cls="lnk-navView lnk-noLine"
                                                Text="<%$ Resources:Comun, strDocumentos %>">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkInventory"
                                                Cls="lnk-navView lnk-noLine "
                                                Text="<%$ Resources:Comun, strInventario %>">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkContactos"
                                                Hidden="true"
                                                Cls="lnk-navView lnk-noLine "
                                                Text="<%$ Resources:Comun, strContactos %>">
                                                <Listeners>
                                                    <Click Fn="NavegacionTabs"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Panel runat="server" ID="tagsContainer" Layout="ColumnLayout">
                                        <Items>
                                        </Items>
                                    </ext:Panel>
                                </DockedItems>
                                <Items>
                                    <ext:Panel ID="ctMain1" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="false">
                                        <Items>
                                            <ext:Container ID="hugeCt" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain"
                                                    runat="server"
                                                    Url="../Sites/EmplazamientosGridPrincipal.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain2" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt2" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain2"
                                                    runat="server"
                                                    Url="../Sites/EmplazamientosGridLocalizaciones.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain3" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt3" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain3"
                                                    runat="server"
                                                    Url="../Sites/EmplazamientosGridAtributosDinamicos.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain4" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt4" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain4"
                                                    runat="server"
                                                    Url="../Sites/EmplazamientosMapas.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain5" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt5" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain5"
                                                    runat="server"
                                                    Url="../Sites/EmplazamientosGridDocumentos.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain6" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt6" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain6"
                                                    runat="server"
                                                    Url="../Sites/EmplazamientosGridInventario.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel ID="ctMain7" runat="server" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="true">
                                        <Items>
                                            <ext:Container ID="hugeCt7" runat="server" Layout="FitLayout">
                                                <Loader ID="LoaderMain7"
                                                    runat="server"
                                                    Url="../Sites/EmplazamientosGridContactos.aspx"
                                                    Mode="Frame">
                                                    <LoadMask ShowMask="true" />
                                                </Loader>
                                            </ext:Container>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:Panel runat="server" ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="FitLayout"
                                Cls="resizePnMoreSite"
                                Header="false" Border="false" Width="380" Hidden="false">
                                <Listeners>
                                    <AfterRender Handler="ResizerAside(this)"></AfterRender>
                                </Listeners>
                                <DockedItems>
                                    <ext:Label Dock="Top"
                                        MinHeight="60"
                                        MinWidth="300"
                                        PaddingSpec="20 0 0 20"
                                        meta:resourcekey="lblAsideNameR"
                                        ID="lblAsideNameR"
                                        runat="server"
                                        IconCls="ico-head-filters"
                                        Cls="lblHeadAsideDock"
                                        Text="<%$ Resources:Comun, strFiltros %>">
                                    </ext:Label>
                                    <ext:Label Dock="Top"
                                        MinHeight="60"
                                        MinWidth="300"
                                        PaddingSpec="20 0 0 20"
                                        meta:resourcekey="lblAsideNameR"
                                        ID="lblAsideNameInfo"
                                        runat="server"
                                        IconCls="ico-head-info"
                                        Cls="lblHeadAsideDock"
                                        Hidden="true"
                                        Text="<%$ Resources:Comun, strAtributos %>">
                                    </ext:Label>
                                    <ext:Label
                                        runat="server"
                                        Hidden="true"
                                        ID="lbButtonSitesVisibles"
                                        Text="<%$ Resources:Comun, jsMostrarEmplazamientosOcultos %>" />
                                    <ext:Button
                                        runat="server"
                                        ID="btnTgSitesVisible"
                                        Text="<%$ Resources:Comun, jsMostrarEmplazamientosOcultos %>"
                                        ToolTip="<%$ Resources:Comun, jsMostrarEmplazamientosOcultos %>"
                                        EnableToggle="true"
                                        Pressed="false"
                                        Focusable="false"
                                        Cls="btn-toggleGrid BtnFiltrosAlignR"
                                        MaxWidth="200"
                                        AriaLabel="<%$ Resources:Comun, jsMostrarEmplazamientosOcultos %>">
                                        <Listeners>
                                            <Click Fn="btnTgSitesVisible" />
                                        </Listeners>
                                    </ext:Button>
                                </DockedItems>
                                <Items>
                                    <ext:Panel
                                        meta:resourcekey="ctAsideR"
                                        ID="ctAsideR"
                                        runat="server"
                                        Border="false"
                                        Header="false"
                                        Layout="AnchorLayout"
                                        Cls="">
                                        <Items>
                                            <%--LEFT TABS MENU--%>
                                            <ext:Panel ID="mnAsideR" runat="server" Border="false" Header="false" AnchorVertical="100%" AnchorHorizontal="14%">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        meta:resourcekey="btnMyF2"
                                                        ID="btnMyF2"
                                                        Cls="btnFiltersPlus-asR"
                                                        ToolTip="<%$ Resources:Comun, strCrearFiltro %>"
                                                        Handler="displayMenuSites('pnCFilters')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        meta:resourcekey="btnMyFilters"
                                                        ID="btnMyFilters"
                                                        Cls="btnMyFilters-asR"
                                                        ToolTip="<%$ Resources:Comun, strMisFiltros %>"
                                                        Handler="displayMenuSites('pnGridsAsideMyFilters')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnMapFilters"
                                                        Cls="btnFiltrosMapas-asR"
                                                        Hidden="true"
                                                        Handler="displayMenuSites('pnMapFilters')">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Panel>
                                            <%--PANELS--%>

                                            <ext:Panel
                                                MarginSpec="10 0 0 0"
                                                ID="pnGridsAside"
                                                runat="server"
                                                AnchorVertical="100%" AnchorHorizontal="86%"
                                                Border="false"
                                                OverflowY="Auto"
                                                Header="false"
                                                Layout="AnchorLayout"
                                                Hidden="false">
                                                <Listeners>
                                                </Listeners>
                                                <Items>
                                                    <%--CREATE FILTERS PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnCFilters"
                                                        Margin="0"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Hidden="true">
                                                        <Items>

                                                            <ext:Panel
                                                                runat="server"
                                                                ID="pnCFiltersContainer">

                                                                <DockedItems>
                                                                    <ext:Label
                                                                        Dock="Top"
                                                                        MarginSpec="0 8 20 0"
                                                                        meta:resourcekey="lblGrid"
                                                                        ID="lblGrid"
                                                                        runat="server"
                                                                        IconCls="btn-CFilter"
                                                                        Cls="lblHeadAside"
                                                                        Text="<%$ Resources:Comun, strCrearFiltro %>">
                                                                    </ext:Label>
                                                                </DockedItems>
                                                                <Items>
                                                                    <ext:TextField runat="server"
                                                                        meta:resourcekey="pnNewFilter"
                                                                        ID="pnNewFilter"
                                                                        FieldLabel=""
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        CausesValidation="true"
                                                                        EmptyText="<%$ Resources:Comun, strNombreFiltro %>" />
                                                                    <ext:Button
                                                                        runat="server"
                                                                        ID="btnFilter"
                                                                        Cls="btn-add"
                                                                        Text="<%$ Resources:Comun, strNuevoFiltro %>">
                                                                        <Listeners>
                                                                            <Click Fn="newFilter" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:ComboBox runat="server"
                                                                        meta:resourcekey="cmbField"
                                                                        ID="cmbField"
                                                                        FieldLabel="<%$ Resources:Comun, strCampo %>"
                                                                        LabelAlign="Top"
                                                                        DisplayField="Name"
                                                                        EmptyText="<%$ Resources:Comun, strCampo %>"
                                                                        Flex="1"
                                                                        Mode="Local"
                                                                        QueryMode="Local"
                                                                        Cls="pnForm fieldFilter"
                                                                        ValueField="Id">
                                                                        <Store>
                                                                            <ext:Store
                                                                                ID="storeCampos"
                                                                                runat="server"
                                                                                AutoLoad="false"
                                                                                RemoteFilter="false"
                                                                                OnReadData="storeCampos_Refresh">
                                                                                <Proxy>
                                                                                    <ext:PageProxy />
                                                                                </Proxy>
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="Id">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="Id" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                            <ext:ModelField Name="typeData" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                                <Listeners>
                                                                                    <DataChanged Fn="beforeLoadCmbField" />
                                                                                </Listeners>
                                                                            </ext:Store>
                                                                        </Store>
                                                                        <Listeners>
                                                                            <Select Fn="selectField" />
                                                                            <FocusEnter Fn="focusCmbField" />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                    <ext:TextField runat="server"
                                                                        meta:resourcekey="pnSearch"
                                                                        ID="textInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        CausesValidation="true"
                                                                        EmptyText="<%$ Resources:Comun, strDependeDelCampo %>"
                                                                        Cls="pnForm"
                                                                        Hidden="false" />
                                                                    <ext:DateField
                                                                        runat="server"
                                                                        ID="dateInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        Cls="pnForm"
                                                                        Hidden="true"
                                                                        Format="dd/MM/yyyy">
                                                                    </ext:DateField>
                                                                    <ext:NumberField
                                                                        runat="server"
                                                                        ID="numberInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        Cls="pnForm"
                                                                        Hidden="true">
                                                                    </ext:NumberField>
                                                                    <ext:ComboBox
                                                                        ID="cmbOperatorField"
                                                                        runat="server"
                                                                        FieldLabel="<%$ Resources:Comun, strOperador %>"
                                                                        DisplayField="Name"
                                                                        EmptyText="<%$ Resources:Comun, strOperador %>"
                                                                        Cls="pnForm"
                                                                        Flex="1"
                                                                        LabelAlign="Top"
                                                                        Hidden="true"
                                                                        QueryMode="Local">
                                                                        <Items>
                                                                            <ext:ListItem Text="=" Value="IGUAL" />
                                                                            <ext:ListItem Text="<" Value="MENOR" />
                                                                            <ext:ListItem Text=">" Value="MAYOR" />
                                                                        </Items>
                                                                    </ext:ComboBox>
                                                                    <ext:MultiCombo runat="server"
                                                                        Hidden="true"
                                                                        ID="cmbTiposDinamicos"
                                                                        FieldLabel="<%$ Resources:Comun, strTipoDinamico %>"
                                                                        LabelAlign="Top"
                                                                        DisplayField="Name"
                                                                        EmptyText="<%$ Resources:Comun, strTipoDinamico %>"
                                                                        Flex="1"
                                                                        Cls="pnForm"
                                                                        ValueField="Id"
                                                                        QueryMode="Local">
                                                                        <Store>
                                                                            <ext:Store
                                                                                ID="storeTiposDinamicos"
                                                                                runat="server"
                                                                                AutoLoad="false"
                                                                                OnReadData="storeTiposDinamicos_Refresh">
                                                                                <Proxy>
                                                                                    <ext:PageProxy />
                                                                                </Proxy>
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="Id">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="Id" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                                <Listeners>
                                                                                    <DataChanged Fn="beforeLoadCmbField" />
                                                                                </Listeners>
                                                                            </ext:Store>
                                                                        </Store>
                                                                    </ext:MultiCombo>
                                                                    <ext:Button
                                                                        runat="server" meta:resourcekey="ColMas"
                                                                        ID="btnAdd"
                                                                        IconCls="ico-addBtn"
                                                                        Cls="btn-mini-ppal btnAdd"
                                                                        Text="<%$ Resources:Comun, jsAgregar %>">
                                                                        <Listeners>
                                                                            <Click Fn="addElementFilter" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:Panel runat="server" ID="tagsHeader">
                                                                        <Items>
                                                                            <ext:Label
                                                                                meta:resourcekey="lblCampo"
                                                                                runat="server"
                                                                                Cls="tabsLabels"
                                                                                Text="<%$ Resources:Comun, strCampo %>">
                                                                            </ext:Label>
                                                                            <ext:Label
                                                                                meta:resourcekey="lblBuscar"
                                                                                runat="server"
                                                                                Cls="tabsLabels"
                                                                                Text="<%$ Resources:Comun, strBuscar %>">
                                                                            </ext:Label>
                                                                        </Items>
                                                                    </ext:Panel>
                                                                    <ext:Panel runat="server" ID="pnTagContainer" Scrollable="Vertical" MaxHeight="100">
                                                                        <Items>
                                                                        </Items>
                                                                    </ext:Panel>

                                                                    <ext:Button
                                                                        runat="server"
                                                                        meta:resourcekey="btnAplyFilter"
                                                                        ID="btnAplyFilter"
                                                                        Cls="btn-end"
                                                                        Text="<%$ Resources:Comun, strAplicar %>">
                                                                        <Listeners>
                                                                            <Click Fn="aplyFilter" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:Button
                                                                        runat="server"
                                                                        meta:resourcekey="btnSaveFilter"
                                                                        ID="btnSaveFilter"
                                                                        Cls="btn-save"
                                                                        Text="<%$ Resources:Comun, btnGuardar.Text %>">
                                                                        <Listeners>
                                                                            <Click Fn="saveFilter" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Panel>
                                                        </Items>
                                                    </ext:Panel>

                                                    <%--MY FILTERS PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnGridsAsideMyFilters"
                                                        Layout="FitLayout"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Hidden="true">
                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="0 8 20 8"
                                                                meta:resourcekey="lblGrid"
                                                                ID="Label1"
                                                                runat="server"
                                                                IconCls="ico-head-my-filters"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strMisFiltros %>">
                                                            </ext:Label>
                                                        </DockedItems>


                                                        <Items>
                                                            <ext:GridPanel
                                                                MarginSpec="8 8 120 8"
                                                                ID="GridMyFilters"
                                                                runat="server"
                                                                Header="false"
                                                                Border="false"
                                                                Scrollable="Vertical"
                                                                Cls="GridMyFilters">
                                                                <Store>
                                                                    <ext:Store
                                                                        runat="server"
                                                                        PageSize="10"
                                                                        AutoLoad="false"
                                                                        OnReadData="storeMyFilters_Refresh">
                                                                        <Model>
                                                                            <ext:Model runat="server" IDProperty="ID">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="GestionFiltroID" />
                                                                                    <ext:ModelField Name="UsuarioID" />
                                                                                    <ext:ModelField Name="NombreFiltro" />
                                                                                    <ext:ModelField Name="JsonItemsFiltro" />
                                                                                    <ext:ModelField Name="Pagina" />
                                                                                    <ext:ModelField Name="check" Type="Boolean" DefaultValue="false" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                    </ext:Store>
                                                                </Store>
                                                                <ColumnModel runat="server">
                                                                    <Columns>

                                                                        <ext:Column runat="server"
                                                                            Sortable="true"
                                                                            DataIndex="NombreFiltro"
                                                                            Width="150"
                                                                            Align="Start">
                                                                        </ext:Column>

                                                                        <ext:WidgetColumn
                                                                            meta:resourcekey="ColMas"
                                                                            ID="ColMas"
                                                                            runat="server"
                                                                            Width="15"
                                                                            Cls="col-More"
                                                                            Align="Center"
                                                                            Hidden="false"
                                                                            MinWidth="45">
                                                                            <Widget>
                                                                                <ext:Button
                                                                                    meta:resourcekey="btnColMore"
                                                                                    runat="server"
                                                                                    ID="btnColMore"
                                                                                    Width="16"
                                                                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                                    OverCls="Over-btnMore"
                                                                                    PressedCls="Pressed-none"
                                                                                    FocusCls="Focus-none"
                                                                                    Cls="BtnDeleteChk">
                                                                                    <Listeners>
                                                                                        <Click Fn="DeleteFilter" />
                                                                                    </Listeners>
                                                                                </ext:Button>
                                                                            </Widget>
                                                                        </ext:WidgetColumn>

                                                                        <ext:WidgetColumn
                                                                            meta:resourcekey="ColMas"
                                                                            ID="colBtnEdit"
                                                                            runat="server"
                                                                            Width="18"
                                                                            Cls="col-More"
                                                                            Align="Center"
                                                                            Hidden="false"
                                                                            MinWidth="45">
                                                                            <Widget>
                                                                                <ext:Button
                                                                                    meta:resourcekey="btnColMore"
                                                                                    runat="server"
                                                                                    ID="btnCheck"
                                                                                    Width="18"
                                                                                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                                    OverCls="Over-btnMore"
                                                                                    PressedCls="Pressed-none"
                                                                                    FocusCls="Focus-none"
                                                                                    Cls="BtnEditChk">
                                                                                    <Listeners>
                                                                                        <Click Fn="MostrarEditarFiltroGuardado" />
                                                                                    </Listeners>
                                                                                </ext:Button>
                                                                            </Widget>
                                                                        </ext:WidgetColumn>

                                                                        <ext:WidgetColumn
                                                                            meta:resourcekey="ColMas"
                                                                            ID="colChkAplyFilter"
                                                                            runat="server"
                                                                            Width="18"
                                                                            Cls="col-Chk"
                                                                            Align="Center"
                                                                            Hidden="false"
                                                                            MinWidth="45">
                                                                            <Widget>
                                                                                <ext:Button
                                                                                    runat="server"
                                                                                    ID="chkAplyFilter"
                                                                                    Cls="btn-trans btnApply-filter"
                                                                                    ToolTip="<%$ Resources:Comun, strAplicar %>"
                                                                                    DataIndex="check">
                                                                                    <Listeners>
                                                                                        <Click Fn="AplyFilterSaved" />
                                                                                    </Listeners>
                                                                                </ext:Button>
                                                                            </Widget>
                                                                        </ext:WidgetColumn>

                                                                    </Columns>
                                                                </ColumnModel>
                                                                <View>
                                                                    <ext:GridView runat="server" LoadMask="false" />
                                                                </View>
                                                                <Plugins>
                                                                    <ext:GridFilters runat="server" />
                                                                </Plugins>
                                                            </ext:GridPanel>
                                                        </Items>
                                                    </ext:Panel>

                                                    <%--MAP FILTERS PANEL--%>
                                                    <ext:FormPanel ID="pnMapFilters" runat="server" Hidden="true" Layout="VBoxLayout"
                                                        AnchorVertical="100%" AnchorHorizontal="100%" OverflowY="Auto">
                                                        <DockedItems>
                                                            <ext:Label ID="Label3" runat="server" IconCls="ico-head-my-filters" Cls="lblHeadAside" Text="Map Filters" Dock="Top"></ext:Label>
                                                        </DockedItems>
                                                        <LayoutConfig>
                                                            <ext:VBoxLayoutConfig Align="Center"></ext:VBoxLayoutConfig>
                                                        </LayoutConfig>
                                                        <Defaults>
                                                            <ext:Parameter Name="width" Value="90%" Mode="Auto" />
                                                        </Defaults>
                                                        <Items>
                                                            <ext:NumberField runat="server"
                                                                ID="numRadio"
                                                                meta:resourceKey="numRadio"
                                                                FieldLabel="Radio (Km)"
                                                                LabelAlign="Top"
                                                                AllowDecimals="false"
                                                                Number="10"
                                                                MaxValue="25" />
                                                            <ext:ComboBox runat="server"
                                                                ID="cmbClusters"
                                                                meta:resourceKey="cmbClusters"
                                                                FieldLabel="Cluster"
                                                                LabelAlign="Top"
                                                                Editable="false"
                                                                QueryMode="Local">
                                                            </ext:ComboBox>
                                                            <ext:ComboBox runat="server"
                                                                ID="cmbClientesPanel"
                                                                meta:resourceKey="cmbClientes"
                                                                FieldLabel="Clientes"
                                                                LabelAlign="Top"
                                                                Editable="false"
                                                                QueryMode="Local">
                                                                <Listeners>
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:MultiCombo runat="server"
                                                                ID="multiOperadores"
                                                                meta:resourceKey="multiOperadores"
                                                                FieldLabel="Operadores"
                                                                LabelAlign="Top"
                                                                QueryMode="Remote"
                                                                Editable="false"
                                                                SelectionMode="Selection" />
                                                            <ext:MultiCombo runat="server"
                                                                ID="multiEstadosGlobales"
                                                                meta:resourceKey="multiEstadosGlobales"
                                                                FieldLabel="Estados Globales"
                                                                LabelAlign="Top"
                                                                Editable="false"
                                                                SelectionMode="Selection" />
                                                            <ext:MultiCombo runat="server"
                                                                ID="multiCategoriasSitios"
                                                                meta:resourceKey="multiCategoriasSitios"
                                                                FieldLabel="Categorias Sitios"
                                                                LabelAlign="Top"
                                                                Editable="false"
                                                                SelectionMode="Selection" />
                                                            <ext:MultiCombo runat="server"
                                                                ID="multiEmplazamientosTipos"
                                                                meta:resourceKey="multiEmplazamientosTipos"
                                                                FieldLabel="Emplazamientos Tipos"
                                                                LabelAlign="Top"
                                                                Editable="false"
                                                                SelectionMode="Selection" />
                                                            <ext:MultiCombo runat="server"
                                                                ID="multiEmplazamientosTamanos"
                                                                meta:resourceKey="multiEmplazamientosTamanos"
                                                                FieldLabel="Emplazamientos Tamaños"
                                                                LabelAlign="Top"
                                                                Editable="false"
                                                                SelectionMode="Selection" />

                                                            <ext:Button runat="server"
                                                                MarginSpec="0 0 80 0"
                                                                ID="btnAplicar"
                                                                Cls="btn-end btnApplyFiltersMap"
                                                                meta:resourceKey="btnAplicar"
                                                                Text="Aplicar">
                                                                <Listeners>
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:FormPanel>
                                                </Items>
                                            </ext:Panel>

                                        </Items>
                                    </ext:Panel>
                                    <ext:Panel
                                        meta:resourcekey="ctAsideR"
                                        ID="ctAsideRInfo"
                                        runat="server"
                                        Border="false"
                                        Header="false"
                                        Layout="AnchorLayout"
                                        Hidden="true"
                                        Cls="">
                                        <Items>
                                            <%--LEFT TABS MENU--%>
                                            <ext:Panel ID="mnAsideRInfo" Cls="d-inline-block" runat="server" Border="false" Header="false" AnchorVertical="100%" AnchorHorizontal="14%">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnMoreInfo"
                                                        Cls="btnTorre-asR"
                                                        Hidden="false"
                                                        ToolTip="<%$ Resources:Comun, strEmplazamientos %>"
                                                        Handler="displayMenuSitesInfo('pnInfoSite')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnMoreInfoLocation"
                                                        Cls="location-asR"
                                                        Hidden="false"
                                                        ToolTip="<%$ Resources:Comun, strLocalizacion %>"
                                                        Handler="displayMenuSitesInfo('pnInfoLocation')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnMoreInfoAdicional"
                                                        Cls="btnAdditional-asR"
                                                        Hidden="false"
                                                        ToolTip="<%$ Resources:Comun, strAdicional %>"
                                                        Handler="displayMenuSitesInfo('pnInfoAtributos')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnMoreInfoElemento"
                                                        Cls="btnInventaryElement-asR"
                                                        Hidden="false"
                                                        ToolTip="<%$ Resources:Comun, strElemento %>"
                                                        Handler="displayMenuSitesInfo('pnInfoElemento')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnMoreInfoDocumento"
                                                        Cls="btnDocsInfo-asR"
                                                        Hidden="false"
                                                        ToolTip="<%$ Resources:Comun, strDocumento %>"
                                                        Handler="displayMenuSitesInfo('pnInfoDocumento')">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnMoreInfoContacto"
                                                        Cls="btnContacto-asR"
                                                        Hidden="false"
                                                        ToolTip="<%$ Resources:Comun, strContactos %>"
                                                        Handler="displayMenuSitesInfo('pnInfoContacto')">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Panel>

                                            <%--PANELS--%>
                                            <ext:Panel
                                                MarginSpec="10 0 0 0"
                                                ID="pnGridsAsideInfo"
                                                runat="server"
                                                AnchorVertical="100%" AnchorHorizontal="86%"
                                                Border="false"
                                                Header="false"
                                                Cls="d-inline-block"
                                                Layout="AnchorLayout"
                                                Hidden="false">
                                                <Listeners>
                                                </Listeners>
                                                <Items>
                                                    <%--MORE INFO SITE PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoSite"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="0 8 20 8"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoSite"
                                                                runat="server"
                                                                IconCls="ico-head-torre-gr"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strEmplazamientos %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoEmplazamiento">
                                                                    <tbody id="bodyTablaInfoEmplazamiento">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <%--MORE INFO SITE PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoLocation"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="0 8 20 8"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoLocation"
                                                                runat="server"
                                                                IconCls="ico-head-site"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strLocalizacion %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoLocation">
                                                                    <tbody id="bodyTablaInfoLocation">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <%--MORE INFO SITE PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoAtributos"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="0 8 20 8"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoAtributos"
                                                                runat="server"
                                                                IconCls="ico-head-additional-gr"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strAdicional %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoAtributos">
                                                                    <tbody id="bodyTablaInfoAtributos">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <%--MORE INFO SITE PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoElemento"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="0 8 20 8"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoElemento"
                                                                runat="server"
                                                                IconCls="ico-head-inventaryElement-gr"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strElemento %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoElemento">
                                                                    <tbody id="bodyTablaInfoElemento">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <%--MORE INFO SITE PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoContacto"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="0 8 20 8"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoContacto"
                                                                runat="server"
                                                                IconCls="ico-head-users"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strContactos %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoContacto">
                                                                    <tbody id="bodyTablaInfoContacto">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                    <%--MORE INFO SITE PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnInfoDocumento"
                                                        OverflowY="Auto"
                                                        AnchorVertical="100%" AnchorHorizontal="100%"
                                                        Cls="grdIntoAside"
                                                        Hidden="false">
                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="0 8 20 8"
                                                                meta:resourcekey="lblGrid"
                                                                ID="LabelInfoDocumento"
                                                                runat="server"
                                                                IconCls="ico-head-docs-gr"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strDocumento %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Content>
                                                            <div>
                                                                <table class="tmpCol-table" id="tablaInfoDocumento">
                                                                    <tbody id="bodyTablaInfoDocumento">
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </Content>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>

                                        </Items>
                                    </ext:Panel>

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
