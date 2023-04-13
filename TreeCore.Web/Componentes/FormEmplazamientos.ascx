<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormEmplazamientos.ascx.cs" Inherits="TreeCore.Componentes.FormEmplazamientos" %>

<link href="/Componentes/css/FormEmplazamientos.css" rel="stylesheet" type="text/css" />

<%@ Register Src="/Componentes/Localizaciones.ascx" TagName="Localizaciones" TagPrefix="local" %>

<%@ Register Src="/Componentes/Geoposicion.ascx" TagName="Geoposicion" TagPrefix="local" %>


<%--INICIO  HIDDEN --%>

<ext:Hidden ID="CurrentControl" runat="server" />
<ext:Hidden ID="hdControlURL" runat="server" />
<ext:Hidden ID="hdControlName" runat="server" />

<ext:Hidden ID="hdOperador" runat="server" />
<ext:Hidden ID="hdSeleccionado" runat="server" />
<ext:Hidden ID="hdEstadoGlobal" runat="server" />
<ext:Hidden ID="hdCategoria" runat="server" />
<ext:Hidden ID="hdTipos" runat="server" />
<ext:Hidden ID="hdTamanos" runat="server" />
<ext:Hidden ID="hdTipoEstructura" runat="server" />
<ext:Hidden ID="hdTipoEdificio" runat="server" />
<ext:Hidden ID="hdProyecto" runat="server" />
<ext:Hidden ID="hdMoneda" runat="server" />
<ext:Hidden ID="hdAlquiler" runat="server" />
<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteIDComponente" />
<ext:Hidden ID="hdEmplazamientoID" runat="server" />
<ext:Hidden ID="hdCodigoEmplazamientoAutogenerado" runat="server" />
<ext:Hidden ID="hdCondicionReglaID" runat="server" />

<ext:Hidden runat="server" ID="hdVistaPlantilla" />

<ext:Hidden ID="hdCodigoDuplicado" runat="server" />

<ext:Hidden ID="hdHistoricoEmplazamiento" runat="server" />

<%--FIN  HIDDEN --%>

<%--INICIO  WINDOWS --%>

<ext:FormPanel runat="server"
    ID="containerFormEmplazamiento"
    OverflowY="Auto"
    Cls="formGris formResp winGestionEmplazamientos"
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
            Cls="winGestion-paneles ctForm-resp ctForm-resp-col3 formSite">
            <Items>
                <ext:TextField runat="server"
                    ID="txtCodigo"
                    FieldLabel="<%$ Resources:Comun, strCodigo %>"
                    MaxLength="100"
                    MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                    LabelAlign="Top"
                    Regex="/^[^$%&|<>/\#]*$/"
                    RegexText="<%$ Resources:Comun, regexNombreText %>"
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
                    ValidationGroup="FORM">
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
                    ValidationGroup="FORM">
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
    <Buttons>
        <ext:Button runat="server"
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
        </ext:Button>
        <ext:Button runat="server"
            Hidden="true"
            ID="btnCancelarAgregarEditarEmplazamiento"
            Text="<%$ Resources:Comun, btnCancelar.Text %>"
            Cls="btn-secondary-winForm">
            <Listeners>
                <Click Fn="closeWindowEmplazamiento" />
            </Listeners>
        </ext:Button>
        <ext:Button runat="server"
            ID="btnGuardarAgregarEditarEmplazamiento"
            Text="<%$ Resources:Comun, btnGuardar.Text %>"
            Cls="btn-ppal-winForm"
            Disabled="true">
            <Listeners>
                <Click Fn="winGestionBotonGuardarEmplazamiento" />
            </Listeners>
        </ext:Button>
    </Buttons>
</ext:FormPanel>

<%--FIN  WINDOWS --%>
