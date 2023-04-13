<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReajustePrecios.ascx.cs" Inherits="TreeCore.Componentes.ReajustePrecios" %>

<script type="text/javascript" src="/Componentes/js/ReajustePrecios.js"></script>

<ext:Hidden ID="hdClienteID" runat="server" />

<ext:Hidden ID="hdRadio" runat="server" />
<ext:Hidden ID="hdProximaFecha" runat="server" />
<ext:Hidden ID="hdUltimaFecha" runat="server" />
<ext:Hidden ID="hdInflacion" runat="server" />
<ext:Hidden ID="hdCadencia" runat="server" />
<ext:Hidden ID="hdValor" runat="server" />

<%--Stores--%>

<ext:Store ID="storeInflaciones" runat="server" AutoLoad="false" OnReadData="storeInflaciones_Refresh"
    RemoteSort="true">
    <Proxy>
        <ext:PageProxy />
    </Proxy>
    <Model>
        <ext:Model IDProperty="InflacionID" runat="server">
            <Fields>
                <ext:ModelField Name="InflacionID" />
                <ext:ModelField Name="Inflacion" />
            </Fields>
        </ext:Model>
    </Model>
    <Sorters>
        <ext:DataSorter Property="Inflacion" Direction="ASC" />
    </Sorters>
</ext:Store>

<%--Componente--%>

<ext:RadioGroup
    ID="RGTipo"
    runat="server"
    GroupName="TipoReajuste"
    Cls="x-check-group-alt chkLabel rbGroup">
    <Items>
        <ext:Radio runat="server" BoxLabel="<%$ Resources:Comun, strSinIncremento %>" InputValue="1" Checked="true" ID="RBSinIncremento" Cls="noPaddingRB-L" />
        <ext:Radio runat="server" BoxLabel="<%$ Resources:Comun, strCPI %>" InputValue="2" ID="RBCPI" />
        <ext:Radio runat="server" BoxLabel="<%$ Resources:Comun, strCantidadFija %>" InputValue="3" ID="RBCantidaFija" />
        <ext:Radio runat="server" BoxLabel="<%$ Resources:Comun, strPorcentajeFijo %>" InputValue="4" ID="RBPorcentajeFijo" Cls="noPaddingRB-R" />
    </Items>
    <Listeners>
        <Change Fn="Seleccion" />

    </Listeners>
</ext:RadioGroup>

<ext:Checkbox runat="server" ID="chkControlFechaFin" Checked="true" BoxLabel="<%$ Resources:Comun, strOcultarFechaFin %>" AllowBlank="true" Hidden="true" PaddingSpec="16 0 16 3" Cls="chkLabel">
    <Listeners>
        <Change Fn="OcultarFechaFin" />
    </Listeners>
</ext:Checkbox>

<ext:Container runat="server" Cls="ctSettingProduct ctForm-resp-col2">
    <Items>

        <ext:DateField runat="server"
            meta:resourcekey="txtFechaInicio"
            ID="txtFechaInicioRevision"
            FieldLabel="<%$ Resources:Comun, strFechaInicio %>"
            LabelAlign="Top"
            AllowBlank="false"
            Hidden="true"
            MinDate="<%# DateTime.Now %>"
            AutoDataBind="true"
            ValidationGroup="FORM"
            FormatText=""
            CausesValidation="true"
            Format="<%$ Resources:Comun, FormatFecha %>">
            <Listeners>
                <Change Fn="validarFechaInicioRevision" />
            </Listeners>
        </ext:DateField>

        <ext:DateField runat="server"
            meta:resourcekey="txtFechaInicio"
            ID="txtFechaProxima"
            FieldLabel="<%$ Resources:Comun, strFechaProximaRevision %>"
            LabelAlign="Top"
            AllowBlank="false"
            Hidden="true"
            FormatText=""
            MinDate="<%# DateTime.Now %>"
            AutoDataBind="true"
            ValidationGroup="FORM"
            CausesValidation="true"
            Format="<%$ Resources:Comun, FormatFecha %>">
        </ext:DateField>


        <ext:DateField runat="server"
            meta:resourcekey="txtFechaFin"
            ID="txtFechaFinRevision"
            FieldLabel="<%$ Resources:Comun, strFechaFin %>"
            LabelAlign="Top"
            AllowBlank="true"
            Hidden="true"
            FormatText=""
            MinDate="<%# DateTime.Now %>"
            AutoDataBind="true"
            ValidationGroup="FORM"
            CausesValidation="true"
            Format="<%$ Resources:Comun, FormatFecha %>">
            <Listeners>
                <Change Fn="validarFechaFinRevision" />
            </Listeners>
        </ext:DateField>



        <ext:ComboBox runat="server"
            ID="cmbInflaciones"
            LabelAlign="Top"
            WidthSpec="90%"
            FieldLabel="<%$ Resources:Comun, strInflacion %>"
            EmptyText="Inflaciones"
            ValueField="InflacionID"
            DisplayField="Inflacion"
            StoreID="storeInflaciones"
            ValidationGroup="FORM"
            CausesValidation="true"
            Hidden="true"
            QueryMode="Local"
            Mode="Local"
            AllowBlank="true">
            <Triggers>
                <ext:FieldTrigger IconCls="ico-reload"
                    Hidden="true"
                    Weight="-1"
                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
            </Triggers>
            <Listeners>
                <%--<Select Fn="SeleccionarCombo" />
        <TriggerClick Fn="RecargarCombo" />--%>
            </Listeners>
        </ext:ComboBox>

        <ext:NumberField runat="server"
            ID="txtCadencia"
            FieldLabel="<%$ Resources:Comun, strCadencia %>"
            Hidden="true"
            LabelAlign="Top"
            AllowBlank="true"
            MinValue="0"
            ValidationGroup="FORM"
            CausesValidation="true">
        </ext:NumberField>


        <ext:NumberField runat="server"
            ID="txtPorcentaje"
            FieldLabel="<%$ Resources:Comun, strPorcentajeFijo %>"
            LabelAlign="Top"
            AllowBlank="true"
            Hidden="true"
            MinValue="0"
            ValidationGroup="FORM"
            CausesValidation="true" />

        <ext:NumberField runat="server"
            ID="txtCantidad"
            FieldLabel="<%$ Resources:Comun, strCantidadFija %>"
            Hidden="true"
            LabelAlign="Top"
            AllowBlank="true"
            MinValue="0"
            ValidationGroup="FORM"
            CausesValidation="true">
        </ext:NumberField>
    </Items>
</ext:Container>



