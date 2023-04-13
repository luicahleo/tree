<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Localizaciones.ascx.cs" Inherits="TreeCore.Componentes.Localizaciones" %>

<script type="text/javascript" src="/Componentes/js/Localizaciones.js"></script>

<ext:Hidden ID="hdSeleccionado" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteIDComponente">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdRegionPaises" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdPais" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdRegion" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdProvincia" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>
<ext:Hidden ID="hdMunicipio" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>


<%--Stores--%>



<%--Componente--%>
<ext:ComboBox meta:resourceKey="cmbRegionPaises"
    ID="cmbRegionPaises"
    runat="server"
    QueryMode="Local"
    DisplayField="Region"
    ValueField="RegionID"
    LabelAlign="Top"
    FieldLabel="<%$ Resources:Comun, strRegion %>"
    AllowBlank="false"
    Visible="false"
    Cls="item-form comboForm"
    ValidationGroup="FORM">
    <Store>
        <ext:Store runat="server"
            ID="storeRegionPaises"
            AutoLoad="false"
            OnReadData="storeRegionPaises_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="RegionID" runat="server">
                    <Fields>
                        <ext:ModelField Name="RegionID" Type="Int" />
                        <ext:ModelField Name="Region" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>
    </Store>
    <Listeners>
        <Select Fn="SeleccionarRegionPaises" />
        <TriggerClick Fn="RecargarRegionPaises" />
        <KeyUp Fn="ComboBoxKeyUp" />
    </Listeners>
    <Triggers>
        <ext:FieldTrigger IconCls="ico-reload"
            Hidden="true"
            Weight="-1"
            QTip="<%$ Resources:Comun, strRecargarLista %>" />
    </Triggers>
</ext:ComboBox>
<ext:ComboBox meta:resourceKey="cmbPais"
    ID="cmbPais"
    runat="server"
    DisplayField="Pais"
    QueryMode="Local"
    ValueField="PaisID"
    LabelAlign="Top"
    ForceSelection="true"
    FieldLabel="<%$ Resources:Comun, strPais %>"
    AllowBlank="false"
    ValidationGroup="FORM">
    <%--Cls="item-form comboForm">--%>
    <Store>
        <ext:Store runat="server"
            ID="storePaises"
            AutoLoad="false"
            OnReadData="storePaises_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="PaisID" runat="server">
                    <Fields>
                        <ext:ModelField Name="PaisID" Type="Int" />
                        <ext:ModelField Name="Pais" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>
    </Store>
    <Listeners>
        <Select Fn="SeleccionarPais" />
        <TriggerClick Fn="RecargarPais" />
        <KeyUp Fn="ComboBoxKeyUp" />
    </Listeners>
    <Triggers>
        <ext:FieldTrigger IconCls="ico-reload"
            Hidden="true"
            Weight="-1"
            QTip="<%$ Resources:Comun, strRecargarLista %>" />
    </Triggers>
</ext:ComboBox>
<ext:ComboBox meta:resourceKey="cmbRegion"
    runat="server"
    ID="cmbRegion"
    QueryMode="Local"
    ValueField="RegionPaisID"
    DisplayField="RegionPais"
    LabelAlign="Top"
    AllowBlank="false"
    FieldLabel="<%$ Resources:Comun, strRegionPais %>"
    ForceSelection="true"
    ValidationGroup="FORM">
    <Store>
        <ext:Store runat="server"
            ID="storeRegiones"
            AutoLoad="false"
            OnReadData="storeRegiones_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="RegionPaisID" runat="server">
                    <Fields>
                        <ext:ModelField Name="RegionPaisID" Type="Int" />
                        <ext:ModelField Name="RegionPais" />
                        <ext:ModelField Name="PaisID" Type="Int" />
                        <ext:ModelField Name="Activo" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>
    </Store>
    <Listeners>
        <Select Fn="SeleccionarRegion" />
        <TriggerClick Fn="RecargarRegion" />
        <KeyUp Fn="ComboBoxKeyUp" />
    </Listeners>
    <Triggers>
        <ext:FieldTrigger
            IconCls="ico-reload"
            Hidden="true"
            Weight="-1"
            QTip="<%$ Resources:Comun, strRecargarLista %>" />
    </Triggers>
</ext:ComboBox>
<ext:ComboBox meta:resourceKey="cmbProvincia"
    runat="server"
    ID="cmbProvincia"
    QueryMode="Local"
    ValueField="ProvinciaID"
    DisplayField="Provincia"
    LabelAlign="Top"
    AllowBlank="false"
    FieldLabel="<%$ Resources:Comun, strProvincia %>"
    ForceSelection="true"
    ValidationGroup="FORM">
    <Store>
        <ext:Store runat="server"
            ID="storeProvincias"
            AutoLoad="false"
            OnReadData="storeProvincias_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="ProvinciaID" runat="server">
                    <Fields>
                        <ext:ModelField Name="ProvinciaID" Type="Int" />
                        <ext:ModelField Name="Provincia" />
                        <ext:ModelField Name="RegionPaisID" Type="Int" />
                        <ext:ModelField Name="Activo" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>
    </Store>
    <Listeners>
        <Select Fn="SeleccionarProvincia" />
        <TriggerClick Fn="RecargarProvincia" />
        <KeyUp Fn="ComboBoxKeyUp" />
    </Listeners>
    <Triggers>
        <ext:FieldTrigger IconCls="ico-reload"
            Hidden="true"
            Weight="-1"
            QTip="<%$ Resources:Comun, strRecargarLista %>" />
    </Triggers>
</ext:ComboBox>
<ext:ComboBox meta:resourceKey="cmbMunicipio"
    ID="cmbMunicipio"
    runat="server"
    QueryMode="Local"
    DisplayField="Municipio"
    ValueField="MunicipioID"
    LabelAlign="Top"
    FieldLabel="<%$ Resources:Comun, strMunicipio %>"
    AllowBlank="false"
    ForceSelection="true"
    ValidationGroup="FORM">
    <Store>
        <ext:Store runat="server"
            ID="storeMunicipios"
            AutoLoad="false"
            OnReadData="storeMunicipios_Refresh"
            RemoteSort="true">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="MunicipioID" runat="server">
                    <Fields>
                        <ext:ModelField Name="MunicipioID" Type="Int" />
                        <ext:ModelField Name="Municipio" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>
    </Store>
    <Listeners>
        <Select Fn="SeleccionarMunicipio" />
        <TriggerClick Fn="RecargarMunicipio" />
        <KeyUp Fn="ComboBoxKeyUp" />
    </Listeners>
    <Triggers>
        <ext:FieldTrigger IconCls="ico-reload"
            Hidden="true"
            Weight="-1"
            QTip="<%$ Resources:Comun, strRecargarLista %>" />
    </Triggers>
</ext:ComboBox>
