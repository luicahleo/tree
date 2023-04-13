<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgramadorTareasCron.ascx.cs" Inherits="TreeCore.Componentes.ProgramadorTareasCron" %>

<script type="text/javascript" src="/Componentes/js/ProgramadorTareasCron.js"></script>
<link href="/CSS/tCore.css" rel="stylesheet" type="text/css" />

<ext:Hidden ID="hdClienteID" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>

<ext:Hidden ID="hdMinDate" runat="server">
    <Listeners>
        <Render Fn="OcultarContenedorPadre" />
    </Listeners>
</ext:Hidden>

<%--Componente--%>

<ext:ComboBox runat="server"
    ID="cmbFrecuencia"
    FieldLabel="<%$ Resources:Comun, strFrecuencia %>"
    AllowBlank="false"
    ValidationGroup="FORM"
    CausesValidation="true"
    LabelAlign="Top">
    <Items>
        <ext:ListItem Text="<%$ Resources:Comun, strNoSeRepite %>" Value="NoSeRepite" />
        <ext:ListItem Text="<%$ Resources:Comun, strDiario %>" Value="Diario" />
        <ext:ListItem Text="<%$ Resources:Comun, strDiaLaborable %>" Value="DiaLaborable" />
        <ext:ListItem Text="<%$ Resources:Comun, strSemanal %>" Value="Semanal" />
        <ext:ListItem Text="<%$ Resources:Comun, strSemanalCustom %>" Value="SemanalCustom" />
        <ext:ListItem Text="<%$ Resources:Comun, strMensual %>" Value="Mensual" />
        <ext:ListItem Text="<%$ Resources:Comun, strMensualCustom %>" Value="MensualCustom" />
    </Items>
    <Triggers>
        <ext:FieldTrigger meta:resourceKey="RecargarLista"
            IconCls="ico-reload"
            Hidden="true"
            QTip="<%$ Resources:Comun, strRecargarLista %>" />
    </Triggers>
    <Listeners>
        <Select Fn="SeleccionarFrecuencia" />
        <TriggerClick Fn="RecargarComboFrecuencia" />
        <%--<Show Fn="MostrarContenedorPadre" />
        <Hide Fn="OcultarContenedorPadre" />--%>
    </Listeners>
</ext:ComboBox>
<%--Comun--%>

<ext:DateField runat="server"
    meta:resourcekey="txtFechaInicio"
    ID="txtFechaInicio"
    FieldLabel="<%$ Resources:Comun, strFechaInicio %>"
    LabelAlign="Top"
    AllowBlank="false"
    Hidden="true"
    MinDate="<%# DateTime.Now %>"
    AutoDataBind="true"
    Vtype="daterange"
    Format="<%$ Resources:Comun, FormatFecha %>">
    <CustomConfig>
        <ext:ConfigItem Name="endDateField" Value="txtFechaFin" Mode="Value" />
    </CustomConfig>
    <Listeners>
        <ValidityChange Fn="validarFechaInicioRevision" />
        <Change Fn="validarFechaInicioRevision" />
        <Show Fn="MostrarContenedorPadreForm" />
        <Hide Fn="OcultarContenedorPadre" />
        <AfterRender Fn="OcultarContenedorPadreForm" />
    </Listeners>
</ext:DateField>

<ext:TextArea
    runat="server"
    ID="txtPrevisualizar"
    LabelAlign="Top"
    FieldLabel="<%$ Resources:Comun, strSiguientesFechas %>"
    MaxLength="300"
    MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
    Cls="txtAreaFrecuencias"
    AllowBlank="true"
    Hidden="true"
    Scrollable="Disabled"
    Editable="false"
    HeightSpec="100%"
    ValidationGroup="FORM"
    CausesValidation="true">
    <Listeners>
        <Show Fn="MostrarContenedorPadreForm" />
        <Hide Fn="OcultarContenedorPadre" />
        <AfterRender Fn="OcultarContenedorPadreForm" />
    </Listeners>
</ext:TextArea>

<ext:DateField runat="server"
    meta:resourcekey="txtFechaFin"
    ID="txtFechaFin"
    FieldLabel="<%$ Resources:Comun, strFechaFin %>"
    LabelAlign="Top"
    AllowBlank="true"
    Hidden="true"
    MinDate="<%# DateTime.Now %>"
    AutoDataBind="true"
    Vtype="daterange"
    Format="<%$ Resources:Comun, FormatFecha %>">
    <CustomConfig>
        <ext:ConfigItem Name="startDateField" Value="txtFechaInicio" Mode="Value" />
    </CustomConfig>
    <Listeners>
        <Change Fn="validarFechaFinRevision" />
        <Show Fn="MostrarContenedorPadreForm" />
        <Hide Fn="OcultarContenedorPadre" />
        <AfterRender Fn="OcultarContenedorPadreForm" />
    </Listeners>
</ext:DateField>

<ext:TextField
    runat="server"
    ID="txtCronFormat"
    LabelAlign="Top"
    FieldLabel="<%$ Resources:Comun, strCronFormato %>"
    MaxLength="50"
    MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
    AllowBlank="false"
    Hidden="true"
    Editable="false"
    Cls="txtCronFormat"
    ValidationGroup="FORM"
    CausesValidation="true">
    <Listeners>
        <Show Fn="MostrarContenedorPadreForm" />
        <Hide Fn="OcultarContenedorPadre" />
        <AfterRender Fn="OcultarContenedorPadreForm" />
        <%--<Change Handler="MetodoBtnGuardar()" />--%>
    </Listeners>
</ext:TextField>

<%--FIN COMUN--%>

<%--SEMANAL--%>

<ext:MultiCombo
    ID="cmbDias"
    runat="server"
    Width="260"
    FieldLabel="<%$ Resources:Comun, strDias %>"
    AllowBlank="false"
    Hidden="true"
    ValidationGroup="FORM"
    CausesValidation="true"
    LabelAlign="Top">
    <Items>
        <ext:ListItem Text="<%$ Resources:Comun, strLunes %>" Value="1" />
        <ext:ListItem Text="<%$ Resources:Comun, strMartes %>" Value="2" />
        <ext:ListItem Text="<%$ Resources:Comun, strMiercoles %>" Value="3" />
        <ext:ListItem Text="<%$ Resources:Comun, strJueves %>" Value="4" />
        <ext:ListItem Text="<%$ Resources:Comun, strViernes %>" Value="5" />
        <ext:ListItem Text="<%$ Resources:Comun, strSabado %>" Value="6" />
        <ext:ListItem Text="<%$ Resources:Comun, strDomingo %>" Value="0" />
    </Items>
    <Listeners>
        <Show Fn="MostrarContenedorPadreForm" />
        <Hide Fn="OcultarContenedorPadre" />
        <AfterRender Fn="OcultarContenedorPadreForm" />
        <Change Fn="Validar" />
    </Listeners>
</ext:MultiCombo>

<%--FIN SEMANAL--%>

<%--MENSUAL--%>

<ext:NumberField
    runat="server"
    ID="txtDiaCadaMes"
    LabelAlign="Top"
    FieldLabel="<%$ Resources:Comun, strDiaCadaMes %>"
    MaxLength="50"
    MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
    MaxValue="31"
    MinValue="1"
    EmptyText="dd"
    AllowBlank="false"
    Hidden="true"
    ValidationGroup="FORM"
    CausesValidation="true">
    <Listeners>
        <Show Fn="MostrarContenedorPadreForm" />
        <Hide Fn="OcultarContenedorPadre" />
        <AfterRender Fn="OcultarContenedorPadreForm" />
        <Change Fn="Validar" />
    </Listeners>
</ext:NumberField>

<ext:ComboBox runat="server"
    ID="cmbTipoFrecuencia"
    FieldLabel="<%$ Resources:Comun, strFrecuenciaMeses %>"
    AllowBlank="true"
    Hidden="true"
    ValidationGroup="FORM"
    CausesValidation="true"
    LabelAlign="Top">
    <Items>
        <ext:ListItem Text="<%$ Resources:Comun, strCadaMes %>" Value="/1" />
        <ext:ListItem Text="<%$ Resources:Comun, strCadaDosMeses %>" Value="/2" />
        <ext:ListItem Text="<%$ Resources:Comun, strCadaTresMeses %>" Value="/3" />
        <ext:ListItem Text="<%$ Resources:Comun, strCadaSeisMeses %>" Value="/6" />
        <ext:ListItem Text="<%$ Resources:Comun, strCadaAño %>" Value="/12" />
    </Items>
    <Triggers>
        <ext:FieldTrigger meta:resourceKey="RecargarLista"
            IconCls="ico-reload"
            QTip="<%$ Resources:Comun, strRecargarLista %>" />
    </Triggers>
    <Listeners>
        <TriggerClick Fn="RecargarComboTipoFrecuencia" />
        <Select Fn="SeleccionarFrecuenciaMeses" />
        <Show Fn="MostrarContenedorPadreForm" />
        <Hide Fn="OcultarContenedorPadre" />
        <AfterRender Fn="OcultarContenedorPadreForm" />
        <Change Fn="Validar" />
    </Listeners>
</ext:ComboBox>

<%--<ext:ComboBox runat="server"
    ID="cmbMesInicio"
    FieldLabel="<%$ Resources:Comun, strMesInicio %>"
    AllowBlank="true"
    Hidden="true"
    ValidationGroup="FORM"
    CausesValidation="false"
    LabelAlign="Top">
    <Items>
        <ext:ListItem Text="<%$ Resources:Comun, strEnero %>" Value="1" />
        <ext:ListItem Text="<%$ Resources:Comun, strFebrero %>" Value="2" />
        <ext:ListItem Text="<%$ Resources:Comun, strMarzo %>" Value="3" />
        <ext:ListItem Text="<%$ Resources:Comun, strAbril %>" Value="4" />
        <ext:ListItem Text="<%$ Resources:Comun, strMayo %>" Value="5" />
        <ext:ListItem Text="<%$ Resources:Comun, strJunio %>" Value="6" />
        <ext:ListItem Text="<%$ Resources:Comun, strJulio %>" Value="7" />
        <ext:ListItem Text="<%$ Resources:Comun, strAgosto %>" Value="8" />
        <ext:ListItem Text="<%$ Resources:Comun, strSeptiembre %>" Value="9" />
        <ext:ListItem Text="<%$ Resources:Comun, strOctubre %>" Value="10" />
        <ext:ListItem Text="<%$ Resources:Comun, strNoviembre %>" Value="11" />
        <ext:ListItem Text="<%$ Resources:Comun, strDiciembre %>" Value="12" />
    </Items>
    <Triggers>
        <ext:FieldTrigger meta:resourceKey="RecargarLista"
            IconCls="ico-reload"
            QTip="<%$ Resources:Comun, strRecargarLista %>" />
    </Triggers>
    <Listeners>
        <TriggerClick Fn="RecargarMesInicio" />
        <Select Fn="SeleccionarMesInicio" />
        <Show Fn="MostrarContenedorPadreForm" />
        <Hide Fn="OcultarContenedorPadre" />
        <AfterRender Fn="OcultarContenedorPadreForm" />
        <Change Fn="Validar" />
    </Listeners>
</ext:ComboBox>--%>

<ext:MultiCombo
    ID="cmbMeses"
    runat="server"
    Width="260"
    FieldLabel="<%$ Resources:Comun, strMes %>"
    AllowBlank="false"
    Hidden="true"
    ValidationGroup="FORM"
    CausesValidation="false"
    LabelAlign="Top">
    <Items>
        <ext:ListItem Text="<%$ Resources:Comun, strEnero %>" Value="1" />
        <ext:ListItem Text="<%$ Resources:Comun, strFebrero %>" Value="2" />
        <ext:ListItem Text="<%$ Resources:Comun, strMarzo %>" Value="3" />
        <ext:ListItem Text="<%$ Resources:Comun, strAbril %>" Value="4" />
        <ext:ListItem Text="<%$ Resources:Comun, strMayo %>" Value="5" />
        <ext:ListItem Text="<%$ Resources:Comun, strJunio %>" Value="6" />
        <ext:ListItem Text="<%$ Resources:Comun, strJulio %>" Value="7" />
        <ext:ListItem Text="<%$ Resources:Comun, strAgosto %>" Value="8" />
        <ext:ListItem Text="<%$ Resources:Comun, strSeptiembre %>" Value="9" />
        <ext:ListItem Text="<%$ Resources:Comun, strOctubre %>" Value="10" />
        <ext:ListItem Text="<%$ Resources:Comun, strNoviembre %>" Value="11" />
        <ext:ListItem Text="<%$ Resources:Comun, strDiciembre %>" Value="12" />
    </Items>
    <Listeners>
        <Show Fn="MostrarContenedorPadreForm" />
        <Hide Fn="OcultarContenedorPadre" />
        <AfterRender Fn="OcultarContenedorPadreForm" />
        <Change Fn="Validar" />
    </Listeners>
</ext:MultiCombo>

<%--FIN MENSUAL--%>

<%--<ext:Button runat="server"
    ID="btnGenerar"
    Text="Generate"
    Cls="btn-accept"
    Pressed="false"
    Hidden="true">
    <Listeners>
        <Show Fn="MostrarContenedorPadreForm" />
        <Hide Fn="OcultarContenedorPadre" />
        <AfterRender Fn="OcultarContenedorPadreForm" />
        <Click Fn="BotonGenerar" />
    </Listeners>
</ext:Button>--%>
