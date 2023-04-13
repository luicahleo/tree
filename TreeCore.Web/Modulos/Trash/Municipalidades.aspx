<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Municipalidades.aspx.cs" Inherits="TreeCore.ModGlobal.Municipalidades" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>


    <form id="form1" runat="server">

        <%-- INICIO HIDDEN --%>

        <ext:Hidden ID="hdSeleccionado" runat="server" />

        <ext:Hidden ID="hdPais" runat="server" />
        <ext:Hidden ID="hdRegion" runat="server" />
        <ext:Hidden ID="hdRegionPaises" runat="server" />
        <ext:Hidden ID="hdProvincia" runat="server" />
        <ext:Hidden ID="hdMunicipio" runat="server" />
        <ext:Hidden ID="hdMaestroID" runat="server" />
        <ext:Hidden runat="server" ID="hdCliID" />
        <ext:Hidden runat="server" ID="ModuloID" />
        <ext:Hidden runat="server" ID="hdGlopalPartidoID" />
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
                <Load Fn="RecargarPrincipal" />
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
                <ext:Model runat="server" IDProperty="GlobalMunicipalidadID">
                    <Fields>

                        <ext:ModelField Name="GlobalMunicipalidadID" Type="Int" />
                        <ext:ModelField Name="Municipalidad" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="Defecto" Type="Boolean" />
                        <ext:ModelField Name="MunicipioID" Type="Int" />
                        <ext:ModelField Name="Codigo" />
                        <ext:ModelField Name="CategoriaDivipola" />
                        <ext:ModelField Name="TipoPoblado" />
                        <ext:ModelField Name="Latitud" Type="Float" />
                        <ext:ModelField Name="Longitud" Type="Float" />
                        <ext:ModelField Name="Radio" Type="Float" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Municipalidad" Direction="ASC" />
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
                <ext:Model runat="server" IDProperty="GlobalPartidoID">
                    <Fields>

                        <ext:ModelField Name="GlobalPartidoID" Type="Int" />
                        <ext:ModelField Name="Partido" />
                        <ext:ModelField Name="Activo" Type="Boolean" />
                        <ext:ModelField Name="Defecto" Type="Boolean" />
                        <ext:ModelField Name="GlobalMunicipalidadID" Type="Int" />
                        <ext:ModelField Name="Codigo" />
                        <ext:ModelField Name="Radio" Type="Float" />
                    </Fields>
                </ext:Model>
            </Model>
            <Sorters>
                <ext:DataSorter Property="Partido" Direction="ASC" />
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
                        <ext:ModelField Name="Activo" />
                    </Fields>
                </ext:Model>
            </Model>
        </ext:Store>

        <ext:Store ID="storeProvincias" runat="server" AutoLoad="false" OnReadData="storeProvincias_Refresh"
            RemoteSort="false">
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

        <ext:Store ID="storeMunicipios" runat="server" AutoLoad="false" OnReadData="storeMunicipios_Refresh"
            RemoteSort="false">
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

        <%-- FIN STORES --%>


        <%--WIN COMMARCH --%>
        <ext:Window ID="winGestionComarch"
            runat="server"
            Title="<%$ Resources:Comun, winGestion.Title %>"
            Width="400"
            AutoHeight="true"
            Modal="true"
            Hidden="true">
            <Items>
                <ext:FormPanel ID="FormPanelComarch"
                    runat="server"
                    LabelWidth="80"
                    LabelAlign="Left"
                    BodyStyle="padding:10px;"
                    AutoHeight="true"
                    MonitorPoll="500"
                    MonitorValid="true"
                    Border="false">
                    <Items>
                        <ext:TextField ID="txtCategoriaDivipola"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="<%$ Resources:Comun, strCategoriaDivipola %>"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtCategoriaDivipola" />

                        <ext:TextField ID="txtTipoPoblado"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="<%$ Resources:Comun, strTipoPoblado %>"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtTipoPoblado" />

                        <ext:NumberField ID="numbLatitud"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="<%$ Resources:Comun, strLatitud %>"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="numbLatitud" />

                        <ext:NumberField ID="numbLongitud"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="<%$ Resources:Comun, strLongitud %>"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="numbLongitud" />
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

       <%-- <ext:Window ID="winGestionComarchDetalle"
            runat="server"
            Title="<%$ Resources:Comun, winGestion.Title %>"
            Width="400"
            AutoHeight="true"
            Modal="true"
            Hidden="true">
            <Items>

                <ext:FormPanel ID="FormPanelComarchDetalle"
                    runat="server"
                    LabelWidth="80"
                    LabelAlign="Left"
                    BodyStyle="padding:10px;"
                    AutoHeight="true"
                    MonitorPoll="500"
                    MonitorValid="true"
                    Border="false">
                    <Items>

                        <ext:TextField ID="txtZonaCrc"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Zona Crc"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtZonaCrc" />

                        <ext:TextField ID="txtCiudadCapital"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Ciudad Capital"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtCiudadCapital" />

                        <ext:TextField ID="txtMciCiudadPrincipal"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Mci Ciudad Principal"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtMciCiudadPrincipal" />

                        <ext:TextField ID="txtMciCiudadGrupo"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Mci Ciudad Grupo"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="MciCiudadGrupo" />

                        <ext:TextField ID="txtProyeccionPoblacion"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="Proyeccion Poblacion"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtProyeccionPoblacion" />

                        <ext:TextField ID="txtGrupoMpio"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Grupo Mpio"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtGrupoMpio" />

                        <ext:TextField ID="txtCategorization"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Categorization"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtCategorization" />

                        <ext:TextField ID="txtAmbito"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Ambito"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtAmbito" />

                        <ext:TextField ID="txtProcentajeCoberturaLTE"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Procentaje Cobertura LTE"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="true"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="txtProcentajeCoberturaLTE" />

                        <ext:TextField ID="txtMercado"
                            runat="server"
                            MaxLength="50"
                            FieldLabel="Mercado"
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
        </ext:Window>--%>

        <%-- FIN WINCOMARCH --%>


        <%-- WIN RADIOS --%>
        <ext:Window ID="winRadioDetalle"
            runat="server"
            Title="<%$ Resources:Comun, strRadio %>"
            BodyStyle="padding:10px;"
            Width="300"
            Height="200"
            Modal="true"
            ShowOnLoad="false"
            Hidden="true"
            meta:resourceKey="winRadioDetalle">
            <Items>
                <ext:FormPanel ID="formRadioDetalle"
                    runat="server"
                    LabelWidth="50"
                    LabelAlign="Left"
                    BodyStyle="padding:10px;"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:NumberField runat="server"
                            meta:resourceKey="numRadio"
                            ID="numRadioDetalle"
                            AllowBlank="true"
                            AllowDecimals="true"
                            FieldLabel="<%$ Resources:Comun, strRadio %>"
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
            BodyStyle="padding:10px;"
            Width="300"
            Height="200"
            Modal="true"
            ShowOnLoad="false"
            Hidden="true"
            meta:resourceKey="winRadio">
            <Items>
                <ext:FormPanel ID="formRadio"
                    runat="server"
                    LabelWidth="50"
                    LabelAlign="Left"
                    BodyStyle="padding:10px;"
                    MonitorPoll="500"
                    MonitorValid="true">
                    <Items>
                        <ext:NumberField runat="server"
                            meta:resourceKey="numRadio"
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
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            AllowBlank="false"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="Provincia" />
                        <ext:TextField ID="txtCodigoProvincia"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="<%$ Resources:Comun, strCodigo %>"
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
                            FieldLabel="<%$ Resources:Comun, jsPartido %>"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            ValidationGroup="FORM"
                            CausesValidation="true"
                            meta:resourceKey="Municipio" />
                        <ext:TextField ID="txtCodigoMunicipio"
                            runat="server"
                            MaxLength="100"
                            FieldLabel="<%$ Resources:Comun, strCodigo %>"
                            MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                            Text=""
                            AllowBlank="true"
                            CausesValidation="false"
                            meta:resourceKey="txtCodigoMunicipio" />
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
                                <ext:ComboBox meta:resourceKey="cmbPais"
                                    ID="cmbPais"
                                    runat="server"
                                    StoreID="storePaises"
                                    Mode="Local"
                                    Width="280"
                                    DisplayField="Pais"
                                    ValueField="PaisID"
                                    Editable="true"
                                    ForceSelection="true"
                                    LabelAlign="Top"
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
                                    Width="280"
                                    LabelAlign="Top"
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
                                </ext:ComboBox>
                                <ext:ComboBox meta:resourceKey="cmbProvincia"
                                    runat="server"
                                    ID="cmbProvincia"
                                    Width="280"
                                    StoreID="storeProvincias"
                                    Mode="Local"
                                    Editable="true"
                                    AllowBlank="true"
                                    LabelAlign="Top"
                                    QueryMode="Local"
                                    ForceSelection="true"
                                    DisplayField="Provincia"
                                    ValueField="ProvinciaID"
                                    FieldLabel="<%$ Resources:Comun, strProvincia %>"
                                    EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                    <Triggers>
                                        <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                            IconCls="ico-reload"
                                            Hidden="true"
                                            Weight="-1"
                                            QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Fn="SeleccionarProvincias" />
                                        <TriggerClick Fn="RecargarProvincias" />
                                    </Listeners>
                                </ext:ComboBox>

                                <ext:ComboBox meta:resourceKey="cmbMunicipio"
                                    runat="server"
                                    ID="cmbMunicipio"
                                    Width="280"
                                    StoreID="storeMunicipios"
                                    Mode="Local"
                                    AllowBlank="true"
                                    QueryMode="Local"
                                    LabelAlign="Top"
                                    ForceSelection="true"
                                    DisplayField="Municipio"
                                    ValueField="MunicipioID"
                                    Editable="false"
                                    FieldLabel="<%$ Resources:Comun, strMunicipio %>"
                                    EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Clear" QTip="Limpiar Lista" />
                                        <ext:FieldTrigger IconCls="gen_TriggerReload" QTip="Recargar Lista" />
                                    </Triggers>
                                    <Listeners>
                                        <Select Fn="SeleccionarMunicipios" />
                                        <TriggerClick Fn="RecargarMunicipios" />
                                    </Listeners>
                                </ext:ComboBox>

                                <ext:Button runat="server"
                                    ID="btnAnadir"
                                    Disabled="true"
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
                                    Handler="ExportarDatos('Municipalidades', hdCliID.value, #{gridMaestro}, #{cmbMunicipio}.getValue(), '', -1);" />
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
                            <ext:Column DataIndex="Municipalidad"
                                Text="<%$ Resources:Comun, strMunicipalidad %>"
                                Width="150"
                                meta:resourceKey="colMunicipalidad"
                                ID="colMunicipalidad"
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
                                    Handler="ExportarDatos('Municipalidades', hdCliID.value, #{GridDetalle}, #{ModuloID}.value, '', 1);" />
                                <ext:Button runat="server"
                                    ID="btnRadioDetalle"
                                    meta:resourceKey="btnRadioDetalle"
                                    ToolTip="<%$ Resources:Comun, strRadio %>"
                                    Cls="btnRadiofrecuencia"
                                    Handler="BotonRadioDetalle()" />
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

                            <ext:Column DataIndex="Partido"
                                Width="200"
                                meta:resourceKey="colPartido"
                                ID="colPartido"
                                runat="server"
                                Text="<%$ Resources:Comun, strPartido %>"
                                Flex="1" />
                            <ext:Column DataIndex="Codigo"
                                Width="100"
                                Text="<%$ Resources:Comun, strCodigo %>"
                                meta:resourceKey="colCodigo"
                                ID="colCodigoDetalle"
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
