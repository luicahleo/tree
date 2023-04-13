<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Geoposicion.ascx.cs" Inherits="TreeCore.Componentes.Geoposicion" %>

<%--<link href="../../Componentes/css/geoposicion.css" rel="stylesheet" type="text/css" />
<link href="/CSS/tCore.css" rel="stylesheet" type="text/css" />--%>

<%-- INICIO HIDDEN --%>

<ext:Hidden ID="hdNombreMapa" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdMapa" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdPaisCodForm" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdMunicipioForm" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdProvinciaForm" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdDireccionForm" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdLatitudForm" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdLongitudForm" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdPaisCodPadre" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdProvinciaPadre" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdMunicipioPadre" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdLatitudPadre" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdLongitudPadre" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteID">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdZoom" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>

<%-- FIN HIDDEN --%>

<%-- INICIO STORES --%>

<ext:Store ID="storeCoreMunicipios"
    runat="server"
    AutoLoad="true"
    OnReadData="storeCoreMunicipios_Refresh"
    RemoteSort="true">
    <Proxy>
        <ext:PageProxy />
    </Proxy>
    <Model>
        <ext:Model runat="server"
            IDProperty="MunicipioID">
            <Fields>
                <ext:ModelField Name="MunicipioID" Type="Int" />
                <ext:ModelField Name="Municipio" Type="String" />
                <ext:ModelField Name="NombreMunicipio" Type="String" />
                <ext:ModelField Name="NombreProvincia" Type="String" />
                <ext:ModelField Name="PaisCod" Type="String" />
                <ext:ModelField Name="ClienteID" Type="Int" />
            </Fields>
        </ext:Model>
    </Model>
    <Listeners>
        <Load Fn="cargarCombo" />
    </Listeners>
</ext:Store>

<%-- FIN STORES --%>

<ext:Button
    ID="toolPosicionar"
    runat="server"
    TextAlign="Right"
    Text="<%$ Resources:Comun, strMostrarMapa %>"
    Cls="btn-ppal-winForm btnMap"
    IconCls="btnGeoposicionBlanco"
    ToolTip="<%$ Resources:Comun, strLocalizacion %>">
    <Listeners>
        <Click Fn="showMap" />
        <Render Fn="OcultarContenedorPadreForm" />
    </Listeners>
</ext:Button>

<ext:TextField runat="server"
    ID="txtDireccion"
    FieldLabel="<%$ Resources:Comun, strDireccion %>"
    MaxLength="100"
    MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
    LabelAlign="Top"
    Cls="item-form ico-exclamacion-10px-grey"
    AllowBlank="false"
    ValidationGroup="FORM"
    CausesValidation="true">
</ext:TextField>
<ext:ComboBox ID="cmbMunicipioProvincia"
    runat="server"
    FieldLabel="<%$ Resources:Comun, strMunicipiosProvincias %>"
    QueryMode="Local"
    Cls="item-form comboForm ico-exclamacion-10px-grey"
    LabelAlign="Top"
    DisplayField="Completo"
    AllowBlank="false"
    ValueField="MunicipioID"
    EmptyText="<%$ Resources:Comun, strSeleccionar %>"
    StoreID="storeCoreMunicipios"
    Visible="true"
    ValidationGroup="FORM">
    <ListConfig>
        <ItemTpl runat="server">
            <Html>
                <div class="item-form">
                    <p>{NombreMunicipio}, {NombreProvincia} ({PaisCod})</p>
                </div>
            </Html>
        </ItemTpl>
    </ListConfig>
    <Listeners>
        <Select Fn="SeleccionarCombo" />
        <TriggerClick Fn="RecargarCombo" />
    </Listeners>
    <Triggers>
        <ext:FieldTrigger meta:resourceKey="RecargarLista"
            IconCls="ico-reload"
            Hidden="true"
            Weight="-1"
            QTip="<%$ Resources:Comun, strRecargarLista %>" />
    </Triggers>
</ext:ComboBox>
<ext:TextField runat="server"
    ID="txtBarrio"
    FieldLabel="<%$ Resources:Comun, strBarrio %>"
    MaxLength="100"
    MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
    LabelAlign="Top"
    Cls="item-form"
    AllowBlank="true"
    ValidationGroup="FORM">
    <Listeners>
        <Render Fn="OcultarContenedorPadreForm" />
    </Listeners>
</ext:TextField>
<ext:TextField runat="server"
    ID="txtCodigoPostal"
    FieldLabel="<%$ Resources:Comun, strCodigoPostal %>"
    MaxLength="100"
    MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
    LabelAlign="Top"
    Cls="item-form"
    AllowBlank="true"
    ValidationGroup="FORM" />

<ext:TextField ID="txtLatitud"
    runat="server"
    FieldLabel="<%$ Resources:Comun, strLatitud %>"
    Text=""
    MaxLength="50"
    DecimalSeparator=","
    Cls="item-form txtLatitud ico-exclamacion-10px-grey"
    LabelAlign="Top"
    Regex="/^(-?(90|(([0-8]?[0-9])([.]\d{1,6})?)))$$/"
    MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
    ValidationGroup="FORM"
    CausesValidation="true">
    <Listeners>
        <Show Fn="MostrarContenedorPadre" />
        <Hide Fn="OcultarContenedorPadre" />
        <Focus Fn="anadirClsNoValido" />
        <FocusLeave Fn="anadirClsNoValido" />
        <Render Fn="OcultarContenedorPadreForm" />
    </Listeners>
</ext:TextField>
<ext:TextField ID="txtLongitud"
    runat="server"
    FieldLabel="<%$ Resources:Comun, strLongitud %>"
    Text=""
    MaxLength="50"
    DecimalSeparator=","
    Cls="item-form ico-exclamacion-10px-grey"
    LabelAlign="Top"
    Regex="/^(-?(180|((1[0-7][0-9]|[0-9]?[0-9])([.]\d{1,6})?)))$/"
    MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
    ValidationGroup="FORM"
    CausesValidation="true">
    <Listeners>
        <Show Fn="MostrarContenedorPadre" />
        <Hide Fn="OcultarContenedorPadre" />
        <Focus Fn="anadirClsNoValido" />
        <FocusLeave Fn="anadirClsNoValido" />
        <Render Fn="OcultarContenedorPadreForm" />
    </Listeners>
</ext:TextField>

<%-- INICIO WINDOWS --%>

<ext:Window runat="server"
    ID="winPosicionar"
    Title="<%$ Resources:Comun, strLocalizacion %>"
    Modal="true"
    Width="620"
    Height="545"
    Resizable="false"
    Hidden="true"
    Cls="winForm-resp">
    <Items>
        <ext:Panel ID="pnFormMap"
            Height="500"
            Border="false"
            runat="server">
            <Content>
                <input
                    id="Input-Search"
                    class="FloatingSearchBox"
                    type="text"
                    style="position: relative" />
                <input id="btnFijar"
                    type="button"
                    value="Fijar"
                    class="btn-mini-ppal btnAsignar"
                    style="position: relative" />
                <div id="<%=NombreMapa%>" class="dvMap" />
            </Content>
        </ext:Panel>
    </Items>
</ext:Window>


<%-- FIN WINDOWS --%>
