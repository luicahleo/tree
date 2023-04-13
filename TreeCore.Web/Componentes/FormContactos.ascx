<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormContactos.ascx.cs" Inherits="TreeCore.Componentes.FormContactos" %>

<script src="//maps.googleapis.com/maps/api/js?key=AIzaSyA0k02bRAAmDy-4x0kcN06k1auhLHKGRZs&v=3&libraries=places" type="text/javascript"></script>
<link href="/Componentes/css/FormContactos.css" rel="stylesheet" type="text/css" />
<!--<script type="text/javascript" src="/JS/common.js"></script>-->

<%@ Register Src="/Componentes/Geoposicion.ascx" TagName="Geoposicion" TagPrefix="local" %>

<%--INICIO  HIDDEN --%>

<ext:Hidden ID="hdTipoContacto" runat="server" />
<ext:Hidden ID="hdMunicipio" runat="server" />
<ext:Hidden ID="hdEmplazamientoID" runat="server" />
<ext:Hidden ID="hdEntidadID" runat="server" />
<ext:Hidden ID="hdContactoGlobalID" runat="server" />
<ext:Hidden ID="CurrentControl" runat="server" />
<ext:Hidden ID="hdControlURL" runat="server" />
<ext:Hidden ID="hdControlName" runat="server" />
<ext:Hidden ID="hdClienteID" runat="server" />

<ext:Hidden ID="hdEditando" runat="server" />

<%--FIN  HIDDEN --%>

<%--INICIO  WINDOWS --%>

<ext:FormPanel runat="server"
    ID="containerFormContacto"
    Cls="formGris formResp"
    Layout="VBoxLayout">
    <LayoutConfig>
        <ext:VBoxLayoutConfig Align="Stretch" />
    </LayoutConfig>
    <Items>
        <ext:Container runat="server"
            ID="pnVistasFormContacto"
            MonitorPoll="500"
            MonitorValid="true"
            ActiveIndex="2"
            Border="false"
            Cls="pnNavVistas pnVistasForm"
            AriaRole="navigation">
            <Items>
                <ext:Container runat="server"
                    ID="cntNavVistasFormContacto"
                    Cls="nav-vistas"
                    ActiveIndex="1"
                    Height="50"
                    ActiveItem="1">
                    <Items>
                        <ext:HyperlinkButton runat="server"
                            ID="lnkContactos"
                            Cls="lnk-navView lnk-noLine navActivo"
                            Text="<%$ Resources:Comun, strContactos %>">
                            <Listeners>
                                <Click Fn="showFormsFormContactos" />
                            </Listeners>
                        </ext:HyperlinkButton>
                        <ext:HyperlinkButton runat="server"
                            ID="lnkDirContactos"
                            Cls="lnk-navView lnk-noLine"
                            Text="<%$ Resources:Comun, strDireccion %>">
                            <Listeners>
                                <Click Fn="showFormsFormContactos" />
                            </Listeners>
                        </ext:HyperlinkButton>
                        <ext:HyperlinkButton runat="server"
                            ID="lnkNotasContactos"
                            Cls="lnk-navView lnk-noLine"
                            Text="<%$ Resources:Comun, strNotas %>">
                            <Listeners>
                                <Click Fn="showFormsFormContactos" />
                            </Listeners>
                        </ext:HyperlinkButton>
                    </Items>
                </ext:Container>
            </Items>
        </ext:Container>
        <ext:Container runat="server"
            ID="formContacto"
            Hidden="false"
            Scrollable="Vertical"
            Cls="winGestionContacto-panel ctForm-resp ctForm-resp-col3"
            Height="300">
            <Items>
                <ext:TextField runat="server"
                    ID="txtNombre"
                    FieldLabel="<%$ Resources:Comun, strNombre %>"
                    LabelAlign="Top"
                    MaxLength="100"
                    MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                    AllowBlank="false"
                    Width="202"
                    Cls="item-form">
                    <Listeners>
                        <Change Fn="anadirClsNoValido" />
                        <FocusLeave Fn="anadirClsNoValido" />
                    </Listeners>
                </ext:TextField>
                <ext:TextField runat="server"
                    ID="txtApellidos"
                    FieldLabel="<%$ Resources:Comun, strApellidos %>"
                    LabelAlign="Top"
                    MaxLength="100"
                    MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                    AllowBlank="false"
                    Width="202"
                    Cls="item-form">
                    <Listeners>
                        <Change Fn="anadirClsNoValido" />
                        <FocusLeave Fn="anadirClsNoValido" />
                    </Listeners>
                </ext:TextField>
                <ext:DropDownField runat="server"
                    ID="txtTelefono"
                    Cls="txtTelefono"
                    FieldLabel="<%$ Resources:Comun, strTelefono %>"
                    LabelAlign="Top"
                    AllowBlank="false"
                    EmptyText="<%$ Resources:Comun, strSeleccionarPrefijo %>"
                    TriggerCls="icoprefix"
                    FocusCls="testfocus"
                    Width="202"
                    ValidationGroup="FORM"
                    CausesValidation="true"
                    Regex="/^[[+]([0-9])*]? ([0-9]){5,20}$/">
                    <Component>
                        <ext:MenuPanel runat="server"
                            ID="MenuPanelIco"
                            MaxWidth="120">
                            <Menu runat="server">
                                <Items>
                                    <ext:MenuItem 
                                        runat="server" 
                                        IconCls="x-loading-indicator" 
                                        Text="<%$ Resources:Comun, jsMensajeProcesando %>" 
                                        Focusable="false" 
                                        HideOnClick="false" />
                                </Items>
                                <Loader runat="server" 
                                    Mode="Component" 
                                    DirectMethod="#{DirectMethods}.LoadPrefijos" 
                                    RemoveAll="true">
                                </Loader>
                                <Listeners>
                                    <Click Handler="#{txtTelefono}.setValue(menuItem.text);" />
                                </Listeners>
                            </Menu>
                        </ext:MenuPanel>
                    </Component>
                    <Listeners>
                        <Change Fn="anadirClsNoValido" />
                        <FocusLeave Fn="anadirClsNoValido" />
                    </Listeners>
                </ext:DropDownField>
                <ext:DropDownField runat="server"
                    ID="txtTelefono2"
                    Cls="txtTelefono"
                    FieldLabel="<%$ Resources:Comun, strTelefono2 %>"
                    LabelAlign="Top"
                    AllowBlank="false"
                    EmptyText="<%$ Resources:Comun, strSeleccionarPrefijo %>"
                    TriggerCls="icoprefix"
                    FocusCls="testfocus"
                    Width="202"
                    ValidationGroup="FORM"
                    CausesValidation="true"
                    Regex="/^[[+]([0-9])*]? ([0-9]){5,20}$/">
                    <Component>
                        <ext:MenuPanel runat="server"
                            ID="MenuPanel2"
                            MaxWidth="120">
                            <Menu runat="server">
                                <Items>
                                    <ext:MenuItem 
                                        runat="server" 
                                        IconCls="x-loading-indicator" 
                                        Text="<%$ Resources:Comun, jsMensajeProcesando %>" 
                                        Focusable="false" 
                                        HideOnClick="false" />
                                </Items>
                                <Loader runat="server" 
                                    Mode="Component" 
                                    DirectMethod="#{DirectMethods}.LoadPrefijos" 
                                    RemoveAll="true">
                                </Loader>
                                <Listeners>
                                    <Click Handler="#{txtTelefono2}.setValue(menuItem.text);" />
                                </Listeners>
                            </Menu>
                        </ext:MenuPanel>
                    </Component>
                    <Listeners>
                        <FocusLeave Fn="anadirClsNoValido" />
                        <Change Fn="anadirClsNoValido" />
                    </Listeners>
                </ext:DropDownField>
                <ext:TextField runat="server"
                    ID="txtEmail"
                    FieldLabel="<%$ Resources:Comun, strEmail %>"
                    LabelAlign="Top"
                    ValidationGroup="FORM"
                    Vtype="email"
                    AllowBlank="false"
                    Width="202"
                    Cls="item-form">
                    <Listeners>
                        <Change Fn="anadirClsNoValido" />
                        <FocusLeave Fn="anadirClsNoValido" />
                    </Listeners>
                </ext:TextField>
                <ext:ComboBox runat="server"
                    ID="cmbTipoContacto"
                    FieldLabel="<%$ Resources:Comun, strContactosTipos %>"
                    LabelAlign="Top"
                    EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                    Width="202"
                    DisplayField="ContactoTipo"
                    ValueField="ContactoTipoID"
                    QueryMode="Local"
                    AllowBlank="true"
                    ValidationGroup="FORM"
                    Cls="item-form comboForm">
                    <Store>
                        <ext:Store runat="server"
                            ID="storeTiposContactos"
                            AutoLoad="false"
                            OnReadData="storeTiposContactos_Refresh"
                            RemoteSort="false">
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Model>
                                <ext:Model runat="server"
                                    IDProperty="ContactoTipoID">
                                    <Fields>
                                        <ext:ModelField Name="ContactoTipoID" Type="Int" />
                                        <ext:ModelField Name="ContactoTipo" Type="String" />
                                        <ext:ModelField Name="Activo" Type="Boolean" />
                                        <ext:ModelField Name="Defecto" Type="Boolean" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <Listeners>
                        <Select Fn="SeleccionarCombo" />
                        <TriggerClick Fn="RecargarComboTipos" />
                        <Change Fn="FormContactosValido" />
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

        </ext:Container>
        <ext:Container runat="server"
            ID="formLocation"
            Hidden="true"
            Scrollable="Vertical"
            Cls="winGestionContacto-panel ctForm-resp ctForm-content-resp-col3"
            Height="300">
            <Content>
                <local:Geoposicion ID="geoPunto"
                    runat="server"
                    NombreMapa="mapContactos"
                    Contactos="true"
                    Localizacion="false" />
            </Content>


        </ext:Container>
        <ext:Container runat="server"
            ID="formNotasContacto"
            Scrollable="Vertical"
            Hidden="true"
            Cls="winGestionContacto-panel ctForm-resp "
            Height="300">
            <Items>
                <ext:TextArea runat="server"
                    ID="txaComentarios"
                    Cls="formColnoGrid"
                    AnchorHorizontal="100%"
                    Height="150"
                    FieldLabel="<%$ Resources:Comun, strComentarios %>"
                    LabelAlign="Top"
                    ValidationGroup="FORM"
                    CausesValidation="true"
                    AllowBlank="true" />
            </Items>
        </ext:Container>
    </Items>
    <Listeners>
        <AfterRender Fn="addlistenerValidacion" />
    </Listeners>
    <Buttons>
        <ext:Button runat="server"
            ID="btnPrevContacto"
            Cls="btn-secondary-winForm"
            Focusable="false"
            Width="100px"
            IconCls="ico-close"
            Text="<%$ Resources:Comun, jsCerrar %>">
            <Listeners>
                <Click Fn="cerrarWindow" />
            </Listeners>
        </ext:Button>
        <ext:Button runat="server"
            ID="btnNextContacto"
            Cls="btn-ppal-winForm"
            Width="100px"
            Disabled="true"
            Focusable="false"
            Text="<%$ Resources:Comun, strGuardar %>">
            <Listeners>
                <Click Fn="guardarCambios" />
            </Listeners>
        </ext:Button>
    </Buttons>
    <%--<Listeners>
        <ValidityChange Fn="FormContactosValido" />
    </Listeners>--%>
</ext:FormPanel>

<%--FIN  WINDOWS --%>
