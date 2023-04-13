<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProvinciasMunicipios.aspx.cs" Inherits="TreeCore.ModGlobal.ProvinciasMunicipios" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>

    <form id="form1" runat="server">

        <%-- INICIO HIDDEN --%>

        <ext:Hidden runat="server" ID="hdCliID" />
        <ext:Hidden runat="server" ID="ModuloID" />
        <ext:Hidden runat="server" ID="RegId" />


        <%-- FIN HIDDEN --%>

        <ext:ResourceManager runat="server" ID="ResourceManagerTreeCore" DirectMethodNamespace="TreeCore">
        </ext:ResourceManager>

        <%-- INICIO STORES --%>

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
                <Load Handler="RecargarPais();" />
            </Listeners>
        </ext:Store>


        <ext:Store runat="server" ID="storePrincipal" AutoLoad="false" RemotePaging="false" OnReadData="storePrincipal_Refresh" RemoteSort="true">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrilla" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="ProvinciaID">
                    <Fields>

                        <ext:ModelField Name="ProvinciaID" Type="Int" />
                        <ext:ModelField Name="Provincia" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="RegionPaisID" Type="Int" />
                        <ext:ModelField Name="Defecto" Type="Boolean" />
                        <ext:ModelField Name="Codigo" />
                        <ext:ModelField Name="Radio" Type="Float" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Provincia" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store runat="server" ID="storeDetalle" AutoLoad="false" RemotePaging="false" OnReadData="storeDetalle_Refresh" RemoteSort="true">
            <Listeners>
                <BeforeLoad Fn="DeseleccionarGrillaDetalle" />
            </Listeners>
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model runat="server" IDProperty="MunicipioID">
                    <Fields>

                        <ext:ModelField Name="MunicipioID" Type="Int" />
                        <ext:ModelField Name="Municipio" />
                        <ext:ModelField Name="Codigo" />
                        <ext:ModelField Name="ProvinciaID" Type="Int" />
                        <ext:ModelField Name="FactorZona" Type="Float" />
                        <ext:ModelField Name="FactorComuna" Type="Float" />
                        <ext:ModelField Name="Factor" Type="Float" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="Defecto" Type="Boolean" />
                        <ext:ModelField Name="Radio" Type="Float" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Municipio" Direction="ASC" />
            </Sorters>
        </ext:Store>

        <ext:Store ID="storePaises" runat="server" AutoLoad="false" OnReadData="storePaises_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="PaisID" runat="server">
                    <Fields>
                        <ext:ModelField Name="PaisID" Type="Int" />
                        <ext:ModelField Name="Pais" />
                        <ext:ModelField Name="Defecto" Type="Boolean" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>
        <ext:Store ID="storeRegiones" runat="server" AutoLoad="false" OnReadData="storeRegiones_Refresh"
            RemoteSort="false">
            <Proxy>
                <ext:PageProxy />
            </Proxy>
            <Model>
                <ext:Model IDProperty="RegionPaisID" runat="server">
                    <Fields>
                        <ext:ModelField Name="RegionPaisID" Type="Int" />
                        <ext:ModelField Name="RegionPais" />
                        <ext:ModelField Name="PaisID" Type="Int" />
                        <ext:ModelField Name="Defecto" Type="Boolean" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <%-- FIN STORES --%>


        <%--WIN COMMARCH --%>
        <ext:Window ID="winGestionComarch"
            runat="server"
            Title="<%$ Resources:Comun, winGestion.Title %>"
            Width="400"
            AutoHeight="true"
            BodyPaddingSummary="10 32"
            Modal="true"
            Hidden="true">
            <Items>
                <ext:FormPanel ID="FormPanelComarch"
                    runat="server"
                    LabelWidth="80"
                    LabelAlign="Left"
                    Cls="formGris"
                    AutoHeight="true"
                    MonitorPoll="500"
                    MonitorValid="true"
                    Border="false">
                    <Items>
                        <ext:TextField ID="txtRegionComercial"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="Region Comercial"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtRegionComercial" />

                        <ext:TextField ID="txtZonaMinisterio"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="Zona Ministerio"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtZonaMinisterio" />

                        <ext:TextField ID="txtRegionalTX"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="Regional TX"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtRegionalTX" />

                        <ext:TextField ID="txtResponsableTX"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="Responsable TX"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtResponsableTX" />
                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValidoGestionComarch(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="btnCancelarComarch"
                    meta:resourceKey="btnCancelar"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestionComarch}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardarComarch"
                    meta:resourceKey="btnGuardar"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    IconCls="ico-accept"
                    Disabled="true"
                    Cls="btn-accept">
                    <Listeners>
                        <Click Handler="winGestionBotonGuardarComarch();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>

        <ext:Window ID="winGestionComarchDetalle"
            runat="server"
            Title="<%$ Resources:Comun, winGestion.Title %>"
            Width="500"
            BodyPaddingSummary="10 32"
            AutoHeight="true"
            Modal="true"
            Hidden="true">
            <Items>

                <ext:FormPanel ID="FormPanelComarchDetalle"
                    runat="server"
                    LabelWidth="80"
                    LabelAlign="Left"
                    Cls="formGris ctForm-resp ctForm-resp-col2"
                    AutoHeight="true"
                    MonitorPoll="500"
                    MonitorValid="true"
                    Border="false">
                    <Items>

                        <ext:TextField ID="txtZonaCrc"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Zona Crc"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtZonaCrc" />

                        <ext:TextField ID="txtCiudadCapital"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Ciudad Capital"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtCiudadCapital" />

                        <ext:TextField ID="txtMciCiudadPrincipal"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Mci Ciudad Principal"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtMciCiudadPrincipal" />

                        <ext:TextField ID="txtMciCiudadGrupo"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Mci Ciudad Grupo"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="MciCiudadGrupo" />

                        <ext:TextField ID="txtProyeccionPoblacion"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="Proyeccion Poblacion"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtProyeccionPoblacion" />

                        <ext:TextField ID="txtGrupoMpio"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Grupo Mpio"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtGrupoMpio" />

                        <ext:TextField ID="txtCategorization"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Categorization"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtCategorization" />

                        <ext:TextField ID="txtAmbito"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Ambito"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtAmbito" />

                        <ext:TextField ID="txtProcentajeCoberturaLTE"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Procentaje Cobertura LTE"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtProcentajeCoberturaLTE" />

                        <ext:TextField ID="txtMercado"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Mercado"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtMercado" />

                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValidoGestionComarchDetalle(valid);" />
                    </Listeners>
                </ext:FormPanel>

            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="btnCancelarComarchDetalle"
                    meta:resourceKey="btnCancelar"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestionComarchDetalle}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardarComarchDetalle"
                    meta:resourceKey="btnGuardar"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    IconCls="ico-accept"
                    Disabled="true"
                    Cls="btn-accept">
                    <Listeners>
                        <Click Handler="winGestionBotonGuardarComarchDetalle();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>

        <%-- FIN WINCOMARCH --%>


        <%-- WIN RADIOS --%>
        <ext:Window ID="winRadioDetalle"
            runat="server"
            Title="<%$ Resources:Comun, strRadio %>"
            BodyPaddingSummary="10 32"
            Width="300"
            Height="180"
            Modal="true"
            ShowOnLoad="false"
            Hidden="true"
            meta:resourceKey="winRadioDetalle">
            <Items>
                <ext:FormPanel ID="formRadioDetalle"
                    runat="server"
                    LabelWidth="50"
                    LabelAlign="Left"
                    Cls="formGris"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:NumberField runat="server"
                            meta:resourceKey="numRadio"
                            ID="numRadioDetalle"
                            AllowBlank="true"
                            AllowDecimals="true"
                            FieldLabel="<%$ Resources:Comun, strRadio %>"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            CausesValidation="true" />
                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValidoRadioDetalle(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="btnCancelarRadioDetalle"
                    meta:resourceKey="btnCancelar"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winRadioDetalle}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardarRadioDetalle"
                    meta:resourceKey="btnGuardar"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    IconCls="ico-accept"
                    Disabled="true"
                    Cls="btn-accept">
                    <Listeners>
                        <Click Handler="winRadioBotonGuardarDetalle();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
        <ext:Window ID="winRadio"
            runat="server"
            Title="<%$ Resources:Comun, strRadio %>"
            BodyPaddingSummary="10 32"
            Width="300"
            Height="180"
            Modal="true"
            ShowOnLoad="false"
            Hidden="true"
            meta:resourceKey="winRadio">
            <Items>
                <ext:FormPanel ID="formRadio"
                    runat="server"
                    LabelWidth="50"
                    LabelAlign="Left"
                    Cls="formGris"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:NumberField runat="server"
                            meta:resourceKey="numRadio"
                            ID="numRadio"
                            AllowBlank="true"
                            AllowDecimals="true"
                            FieldLabel="<%$ Resources:Comun, strRadio %>"
                            LabelAlign="Top"
                            WidthSpec="100%"
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
                    meta:resourceKey="btnCancelar"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winRadio}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardarRadio"
                    meta:resourceKey="btnGuardar"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    IconCls="ico-accept"
                    Disabled="true"
                    Cls="btn-accept">
                    <Listeners>
                        <Click Handler="winRadioBotonGuardar();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>

        <%-- FIN WIN RADIOS --%>


        <%-- INICIO WINDOWS --%>

        <ext:Window runat="server"
            ID="winGestion"
            meta:resourceKey="winGestion"
            Title="<%$ Resources:Comun, winGestion.Title %>"
            Width="400"
            Modal="true"
            Hidden="true">
            <Items>
                <ext:FormPanel runat="server"
                    ID="formGestion"
                    Cls="form-gestion"
                    Border="false"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:TextField ID="txtProvincia"
                            runat="server"
                            MaxLength="100"
                            Text=""
                            FieldLabel="<%$ Resources:Comun, strProvincia %>"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="Provincia" />
                        <ext:TextField ID="txtCodigoProvincia"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="<%$ Resources:Comun, strCodigo %>"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            Text=""
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="false"
                            meta:resourceKey="txtCodigoProvincia" />
                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValido(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="btnCancelar"
                    meta:resourceKey="btnCancelar"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestion}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardar"
                    meta:resourceKey="btnGuardar"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    IconCls="ico-accept"
                    Cls="btn-accept"
                    Disabled="true">
                    <Listeners>
                        <Click Handler="winGestionBotonGuardar();" />
                    </Listeners>
                </ext:Button>

            </Buttons>
        </ext:Window>

        <ext:Window runat="server"
            ID="winGestionDetalle"
            meta:resourceKey="winGestionDetalle"
            Title="<%$ Resources:Comun, winGestion.Title %>"
            Width="420"
            Resizable="false"
            Modal="true"
            Hidden="true">

            <Items>
                <ext:FormPanel runat="server"
                    ID="formGestionDetalle"
                    Cls="form-detalle"
                    Border="false"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>

                        <ext:TextField ID="txtMunicipio"
                            runat="server"
                            MaxLength="100"
                            Text=""
                            FieldLabel="<%$ Resources:Comun, strMunicipio %>"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="Municipio" />
                        <ext:TextField ID="txtCodigoMunicipio"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="<%$ Resources:Comun, strCodigo %>"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            Text=""
                            AllowBlank="true"
                            CausesValidation="false"
                            meta:resourceKey="txtCodigoMunicipio" />
                        <ext:NumberField ID="txtFactorZona"
                            runat="server"
                            FieldLabel="Factor Zona"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLength="7"
                            DecimalPrecision="4"
                            Text="1"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtFactorZona" />
                        <ext:NumberField ID="txtFactorMunicipio"
                            runat="server"
                            FieldLabel="Factor Municipio"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            DecimalPrecision="4"
                            MaxLength="7"
                            Text="1"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtFactorMunicipio" />
                        <ext:NumberField ID="txtFactor"
                            runat="server"
                            FieldLabel="Factor Zona"
                            LabelAlign="Top"
                            WidthSpec="100%"
                            MaxLength="7"
                            DecimalPrecision="4"
                            Text="1"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtFactor" />
                    </Items>
                    <Listeners>
                        <ValidityChange Handler="FormularioValidoDetalle(valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button runat="server"
                    ID="btnCancelarDetalle"
                    meta:resourceKey="btnCancelar"
                    Text="<%$ Resources:Comun, btnCancelar.Text %>"
                    IconCls="ico-cancel"
                    Cls="btn-cancel">
                    <Listeners>
                        <Click Handler="#{winGestionDetalle}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server"
                    ID="btnGuardarDetalle"
                    meta:resourceKey="btnGuardar"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    IconCls="ico-accept"
                    Disabled="true"
                    Cls="btn-accept">
                    <Listeners>
                        <Click Handler="winGestionBotonGuardarDetalle();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            <Listeners>
                <Show Handler="#{winGestionDetalle}.center();" />
            </Listeners>
        </ext:Window>

        <%-- FIN WINDOWS --%>

        <%-- INICIO VIEWPORT --%>

        <ext:Viewport runat="server" ID="vwContenedor" Cls="vwContenedor" Layout="Anchor">
            <Items>
                <ext:GridPanel
                    runat="server"
                    ID="gridMaestro"
                    StoreID="storePrincipal"
                    Cls="gridPanel"
                    EnableColumnHide="false"
                    SelectionMemory="false"
                    AnchorHorizontal="100%"
                    AnchorVertical="58%"
                    AriaRole="main">

                    <DockedItems>
                        <ext:Toolbar runat="server"
                            ID="tlbBase"
                            Dock="Top"
                            Cls="tlbGrid">
                            <Items>
                                <%--  <ext:ComboBox meta:resourceKey="cmbPais"
                                    ID="cmbPais"
                                    runat="server"
                                    StoreID="storePaises"
                                    Mode="Local"
                                    Width="350"
                                    DisplayField="Pais"
                                    ValueField="PaisID"
                                    Editable="true"
                                    ForceSelection="true"
                                    FieldLabel="<%$ Resources:Comun, strPais %>"
                                    EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                    AllowBlank="true"
                                    QueryMode="Local">
                                    <Listeners>
                                        <Select Fn="SeleccionarPais" />
                                        <TriggerClick Fn="RecargarPais" />
                                    </Listeners>
                                    <Triggers>
                                        <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                            IconCls="ico-reload"
                                            Hidden="true"
                                            Weight="-1"
                                            QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                    </Triggers>
                                </ext:ComboBox>
                                <ext:ComboBox meta:resourceKey="cmbRegiones"
                                    ID="cmbRegiones"
                                    runat="server"
                                    StoreID="storeRegiones"
                                    Mode="Local"
                                    Width="350"
                                    DisplayField="RegionPais"
                                    ValueField="RegionPaisID"
                                    Editable="true"
                                    ForceSelection="true"
                                    FieldLabel="<%$ Resources:Comun, strRegion %>"
                                    EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                    AllowBlank="true"
                                    QueryMode="Local">
                                    <Listeners>
                                        <Select Fn="SeleccionarRegiones" />
                                        <TriggerClick Fn="RecargarRegiones" />
                                    </Listeners>
                                    <Triggers>
                                        <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                            IconCls="ico-reload"
                                            Hidden="true"
                                            Weight="-1"
                                            QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                    </Triggers>
                                </ext:ComboBox>--%>
                                <ext:Button runat="server"
                                    ID="btnAnadir"
                                    meta:resourceKey="btnAnadir"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEditar"
                                    meta:resourceKey="btnEditar"
                                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                    Cls="btnEditar"
                                    Handler="MostrarEditar();">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEliminar"
                                    meta:resourceKey="btnEliminar"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
                                <ext:Button runat="server"
                                    ID="btnRefrescar"
                                    meta:resourceKey="btnRefrescar"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />
                                <ext:Button runat="server"
                                    ID="btnDescargar"
                                    meta:resourceKey="btnDescargar"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('ProvinciasMunicipios', hdCliID.value, #{gridMaestro}, #{cmbRegiones}.getValue(), '', -1);" />
                                <ext:Button runat="server"
                                    ID="btnDefecto"
                                    ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                    meta:resourceKey="btnDefecto"
                                    Cls="btnDefecto"
                                    Handler="Defecto();" />
                                <ext:Button runat="server"
                                    ID="btnActivar"
                                    meta:resourceKey="btnActivarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    Cls="btnActivar"
                                    Handler="Activar()" />

                                <ext:Button runat="server"
                                    ID="btnRadio"
                                    meta:resourceKey="btnRadio"
                                    ToolTip="<%$ Resources:Comun, strRadio %>"
                                    Cls="btnRadiofrecuencia"
                                    Handler="BotonRadio();" />
                                <ext:Button runat="server"
                                    ID="btnAgregarInformacionComarch"
                                    meta:resourceKey="btnAgregarInformacionComarch"
                                    ToolTip="Agregar Informacion Comarch"
                                    Cls="btnAnadirComm"
                                    Handler="BotonAgregarInformacionComarch();" />
                            </Items>
                        </ext:Toolbar>
                        <ext:Toolbar runat="server"
                            ID="tlbClientes"
                            Dock="Top">
                            <Items>
                                <ext:ToolbarFill></ext:ToolbarFill>
                                <ext:ComboBox meta:resourceKey="cmbPais"
                                    ID="cmbPais"
                                    runat="server"
                                    StoreID="storePaises"
                                    Mode="Local"
                                    Width="350"
                                    DisplayField="Pais"
                                    ValueField="PaisID"
                                    Editable="true"
                                    ForceSelection="true"
                                    FieldLabel="<%$ Resources:Comun, strPais %>"
                                    EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                    LabelAlign="Right"
                                    AllowBlank="true"
                                    QueryMode="Local">
                                    <Listeners>
                                        <Select Fn="SeleccionarPais" />
                                        <TriggerClick Fn="RecargarPais" />
                                    </Listeners>
                                    <Triggers>
                                        <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                            IconCls="ico-reload"
                                            Hidden="true"
                                            Weight="-1"
                                            QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                    </Triggers>
                                </ext:ComboBox>
                                <ext:ComboBox meta:resourceKey="cmbRegiones"
                                    ID="cmbRegiones"
                                    runat="server"
                                    StoreID="storeRegiones"
                                    Mode="Local"
                                    Width="350"
                                    DisplayField="RegionPais"
                                    ValueField="RegionPaisID"
                                    Editable="true"
                                    ForceSelection="true"
                                    LabelAlign="Right"
                                    FieldLabel="<%$ Resources:Comun, strRegion %>"
                                    EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                    AllowBlank="true"
                                    QueryMode="Local"
                                    MarginSpec="0 15 0 0">
                                    <Listeners>
                                        <Select Fn="SeleccionarRegiones" />
                                        <TriggerClick Fn="RecargarRegiones" />
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
                                    ID="cmbClientes"
                                    meta:resourceKey="cmbClientes"
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
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server"
                                ID="colActivoMaestro"
                                DataIndex="Activo"
                                Align="Center"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Cls="col-activo"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column runat="server"
                                ID="colDefectoMaestro"
                                DataIndex="Defecto"
                                Align="Center"
                                Cls="col-default"
                                ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column DataIndex="Provincia"
                                Text="<%$ Resources:Comun, strProvincia %>"
                                Width="150"
                                meta:resourceKey="colProvincia"
                                ID="colProvincia"
                                runat="server"
                                Flex="1" />
                            <ext:Column DataIndex="Codigo"
                                Text="<%$ Resources:Comun, strCodigo %>"
                                Width="150"
                                meta:resourceKey="colCodigo"
                                ID="colCodigoMaestro"
                                runat="server" />
                            <ext:Column DataIndex="Radio"
                                Text="<%$ Resources:Comun, strRadio %>"
                                Width="150"
                                meta:resourceKey="colRadio"
                                ID="colRadioMaestro"
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
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                            meta:resourceKey="GridFilters" />
                        <ext:CellEditing runat="server"
                            ClicksToEdit="2" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar runat="server"
                            ID="PagingToolBar1"
                            meta:resourceKey="PagingToolBar1"
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
                <ext:GridPanel
                    runat="server"
                    ID="GridDetalle"
                    Cls="gridPanel gridDetalle"
                    SelectionMemory="false"
                    EnableColumnHide="false"
                    StoreID="storeDetalle"
                    AnchorHorizontal="100%"
                    AnchorVertical="42%"
                    AriaRole="main">

                    <DockedItems>
                        <ext:Toolbar runat="server"
                            ID="tlbDetalle"
                            Dock="Top"
                            Cls="tlbGrid">
                            <Items>
                                <ext:Button runat="server"
                                    ID="btnAnadirDetalle"
                                    meta:resourceKey="btnAnadirDetalle"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Cls="btnAnadir"
                                    Handler="AgregarEditarDetalle()">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEditarDetalle"
                                    meta:resourceKey="btnEditarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                    Cls="btnEditar"
                                    Handler="MostrarEditarDetalle()">
                                </ext:Button>
                                <ext:Button runat="server"
                                    ID="btnEliminarDetalle"
                                    meta:resourceKey="btnEliminarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    Cls="btnEliminar"
                                    Handler="EliminarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnRefrescarDetalle"
                                    meta:resourceKey="btnRefrescarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    Cls="btnRefrescar"
                                    Handler="RefrescarDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnDescargarDetalle"
                                    meta:resourceKey="btnDescargarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                    Cls="btnDescargar"
                                    Handler="ExportarDatos('ProvinciasMunicipios', hdCliID.value, #{GridDetalle}, #{ModuloID}.value, '', 1);" />
                                <ext:Button runat="server"
                                    ID="btnDefectoDetalle"
                                    ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                    meta:resourceKey="btnDefecto"
                                    Cls="btnDefecto"
                                    Handler="DefectoDetalle();" />
                                <ext:Button runat="server"
                                    ID="btnActivarDetalle"
                                    meta:resourceKey="btnActivarDetalle"
                                    ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                    Cls="btnActivar"
                                    Handler="ActivarDetalle()" />

                                <ext:Button runat="server"
                                    ID="btnRadioDetalle"
                                    meta:resourceKey="btnRadioDetalle"
                                    ToolTip="<%$ Resources:Comun, strRadio %>"
                                    Cls="btnRadiofrecuencia"
                                    Handler="BotonRadioDetalle()" />
                                <ext:Button runat="server"
                                    ID="btnAgregarInformacionComarchDetalle"
                                    meta:resourceKey="btnAgregarInformacionComarch"
                                    ToolTip="Agregar Informacion Comarch"
                                    Cls="btnAnadirComm"
                                    Handler="BotonAgregarInformacionComarchDetalle();" />
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server"
                                ID="colActivoDetalle"
                                DataIndex="Activo"
                                Align="Center"
                                ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                Cls="col-activo"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>
                            <ext:Column runat="server"
                                ID="colDefectoDetalle"
                                DataIndex="Defecto"
                                Align="Center"
                                Cls="col-default"
                                ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                Width="50">
                                <Renderer Fn="DefectoRender" />
                            </ext:Column>

                            <ext:Column DataIndex="Municipio"
                                Width="200"
                                meta:resourceKey="colMunicipio"
                                ID="colMunicipio"
                                runat="server"
                                Text="<%$ Resources:Comun, strMunicipio %>"
                                Flex="1" />
                            <ext:Column DataIndex="Codigo"
                                Width="100"
                                Text="<%$ Resources:Comun, strCodigo %>"
                                meta:resourceKey="colCodigo"
                                ID="colCodigoDetalle"
                                runat="server" />
                            <ext:NumberColumn DataIndex="FactorZona"
                                Width="200"
                                Text="<%$ Resources:Comun, strFactorZona %>"
                                Format="00.0000"
                                meta:resourceKey="colFactorZona"
                                ID="colFactorZonaDetalle"
                                runat="server" />
                            <ext:NumberColumn DataIndex="FactorComuna"
                                Width="200"
                                Text="<%$ Resources:Comun, strFactorMunicipio %>"
                                Format="00.0000"
                                meta:resourceKey="colFactorComuna"
                                ID="colFactorComunaDetalle"
                                runat="server" />
                            <ext:NumberColumn DataIndex="Factor"
                                Width="200"
                                Text="<%$ Resources:Comun, strFactor %>"
                                Format="00.0000"
                                meta:resourceKey="coluFactor"
                                ID="colFactorDetalle"
                                runat="server" />
                            <ext:Column DataIndex="Radio"
                                Text="<%$ Resources:Comun, strRadio %>"
                                Width="150"
                                meta:resourceKey="columRadio"
                                ID="Radio"
                                runat="server" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server"
                            ID="GridRowSelectDetalle"
                            Mode="Single">
                            <Listeners>
                                <Select Fn="Grid_RowSelect_Detalle" />
                                <Deselect Fn="DeseleccionarGrillaDetalle" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:GridFilters runat="server"
                            ID="gridFiltersDetalle"
                            MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                            meta:resourceKey="gridFiltersDetalle" />
                        <ext:CellEditing runat="server"
                            ClicksToEdit="2" />
                    </Plugins>
                    <BottomBar>
                        <ext:PagingToolbar runat="server"
                            ID="PagingToolBar2"
                            meta:resourceKey="PagingToolBar1"
                            StoreID="storeDetalle"
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
                                        <Select Fn="handlePageSizeSelectDetalle" />
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>

        <%-- FIN VIEWPORT --%>
        <div>
        </div>

    </form>
</body>
</html>
