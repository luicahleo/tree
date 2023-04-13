<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrdersSites.aspx.cs" Inherits="TreeCore.ModWorkOrders.pages.WorkOrdersSites" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <!--<script type="text/javascript" src="../../JS/common.js"></script>-->
    <script type="text/javascript" src="js/WorkOrdersSites.js"></script>
    <script type="text/javascript" src="/JS/common.js"></script>
</head>
<body>
    <link href="/CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="css/styleWorkOrders.css" rel="stylesheet" type="text/css" />
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                </Listeners>
            </ext:ResourceManager>

            <ext:Window ID="winGestionWOSite"
                runat="server"
                Title="Window Form"
                WidthSpec="70vw"
                MaxWidth="750"
                HeightSpec="80vh"
                MaxHeight="500"
                Modal="true"
                Centered="true"
                Resizable="false"
                Layout="FitLayout"
                Cls="winForm-resp"
                Hidden="true">
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <Items>
                    <ext:FormPanel ID="FormGestion1"
                        Cls="formGris formResp"
                        runat="server"
                        OverflowY="Auto"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Container runat="server" ID="ctForm" PaddingSpec="16 32" Cls="ctForm-resp ctForm-resp-col3">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtName"
                                        FieldLabel="Name"
                                        LabelAlign="Top"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        Disabled="true"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        ID="txtCode"
                                        FieldLabel="Code"
                                        LabelAlign="Top"
                                        AllowBlank="true"
                                        Disabled="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:ComboBox runat="server"
                                        ID="cmbType"
                                        FieldLabel="Type"
                                        LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="Type 1" />
                                            <ext:ListItem Text="Type 2" />
                                            <ext:ListItem Text="Type 3" />
                                            <ext:ListItem Text="Type 4" />
                                        </Items>
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
                                        ID="cmbWorkflow"
                                        FieldLabel="Workflow"
                                        LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="Workflow 1" />
                                            <ext:ListItem Text="Workflow 2" />
                                            <ext:ListItem Text="Workflow 3" />
                                            <ext:ListItem Text="Workflow 4" />
                                        </Items>
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
                                        ID="cmbProject"
                                        FieldLabel="Project (predictive text)"
                                        LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="Project 1" />
                                            <ext:ListItem Text="Project 2" />
                                            <ext:ListItem Text="Project 3" />
                                            <ext:ListItem Text="Project 4" />
                                        </Items>
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
                                        ID="cmbSupplier"
                                        FieldLabel="Supplier (predictive text)"
                                        LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="Supplier 1" />
                                            <ext:ListItem Text="Supplier 2" />
                                            <ext:ListItem Text="Supplier 3" />
                                            <ext:ListItem Text="Supplier 4" />
                                        </Items>
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
                                        ID="cmbPackage"
                                        FieldLabel="Package of Actions"
                                        LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="Package 1" />
                                            <ext:ListItem Text="Package 2" />
                                            <ext:ListItem Text="Package 3" />
                                            <ext:ListItem Text="Package 4" />
                                        </Items>
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
                                        FieldLabel="Start Date"
                                        LabelAlign="Top"
                                        MinDate="<%# DateTime.Now %>"
                                        AutoDataBind="true"
                                        Vtype="daterange"
                                        Format="<%$ Resources:Comun, FormatFecha %>">
                                        <%--<CustomConfig>
                                                    <ext:ConfigItem Name="endDateField" Value="txtFechaFin" Mode="Value" />
                                                </CustomConfig>--%>
                                    </ext:DateField>
                                    <ext:DateField runat="server"
                                        meta:resourcekey="txtFechaFin"
                                        ID="txtFechaFin"
                                        FieldLabel="End Date"
                                        LabelAlign="Top"
                                        MinDate="<%# DateTime.Now %>"
                                        AutoDataBind="true"
                                        Vtype="daterange"
                                        Format="<%$ Resources:Comun, FormatFecha %>">
                                        <%--<CustomConfig>
                                                    <ext:ConfigItem Name="endDateField" Value="txtFechaFin" Mode="Value" />
                                                </CustomConfig>--%>
                                    </ext:DateField>
                                    <ext:ComboBox runat="server"
                                        ID="cmbPeriod"
                                        FieldLabel="Period"
                                        LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="Periodo 1" />
                                            <ext:ListItem Text="Periodo 2" />
                                            <ext:ListItem Text="Periodo 3" />
                                            <ext:ListItem Text="Periodo 4" />
                                        </Items>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="LimpiarLista"
                                                Icon="Clear"
                                                QTip="Limpiar Lista" />
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                QTip="Recargar Lista" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:Checkbox runat="server"
                                        ID="chkUnplanned"
                                        Cls="chkLabel"
                                        BoxLabel="Mark as Unplanned"
                                        BoxLabelAlign="After" />
                                </Items>
                            </ext:Container>
                        </Items>
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="tbSaveForm"
                                Cls="greytb"
                                Dock="Bottom"
                                Padding="20">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button runat="server"
                                        ID="btnCerrar"
                                        Width="100px"
                                        meta:resourceKey="btnPrev"
                                        IconCls="ico-close"
                                        Cls="btn-secondary-winForm"
                                        Text="<%$ Resources:Comun, strCerrar %>"
                                        Focusable="false"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="cerrarWinGestion()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnAgregar"
                                        Cls="btn-ppal-winForm"
                                        Text="<%$ Resources:Comun, strGuardar %>"
                                        PressedCls="none">
                                        <Listeners>
                                            <%--<Click Handler="winGestionGuardar()" />--%>
                                        </Listeners>
                                    </ext:Button>

                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Window ID="winJoinOrders"
                runat="server"
                Title="Window Form"
                WidthSpec="70vw"
                MaxWidth="750"
                Height="270"
                Modal="true"
                Centered="true"
                Resizable="false"
                Layout="FitLayout"
                Cls="winForm-resp"
                Hidden="true">
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <Items>
                    <ext:FormPanel ID="FormPanel1"
                        Cls="formGris formResp"
                        runat="server"
                        OverflowY="Auto"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Container runat="server" ID="Container1" PaddingSpec="16 32" Cls="ctForm-resp ctForm-resp-col2">
                                <Items>
                                    <ext:Checkbox runat="server"
                                        ID="Checkbox2"
                                        Cls="chkLabel chk-form"
                                        MarginSpec="32px 0 0 0"
                                        BoxLabel="Site Name"
                                        BoxLabelAlign="After" />
                                    <ext:ComboBox runat="server"
                                        ID="ComboBox3"
                                        FieldLabel="Element"
                                        LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="Elemento 1" />
                                            <ext:ListItem Text="Elemento 2" />
                                            <ext:ListItem Text="Elemento 3" />
                                            <ext:ListItem Text="Elemento 4" />
                                        </Items>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="LimpiarLista"
                                                Icon="Clear"
                                                QTip="Limpiar Lista" />
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                QTip="Recargar Lista" />
                                        </Triggers>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                        </Items>
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="Toolbar3"
                                Height="32"
                                Cls="greytb"
                                PaddingSpec="16 32 0"
                                Dock="Top">
                                <Items>
                                    <ext:Label runat="server" Cls="bold fGrey" Text="Choose the final work orders" />
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server"
                                ID="Toolbar1"
                                Cls="greytb"
                                Dock="Bottom"
                                Padding="20">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button runat="server"
                                        ID="Button1"
                                        Width="100px"
                                        meta:resourceKey="btnPrev"
                                        IconCls="ico-close"
                                        Cls="btn-secondary-winForm"
                                        Text="<%$ Resources:Comun, strCerrar %>"
                                        Focusable="false"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="cerrarWinGestion()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="Button4"
                                        Cls="btn-ppal-winForm"
                                        Text="<%$ Resources:Comun, strGuardar %>"
                                        PressedCls="none">
                                        <Listeners>
                                            <%--<Click Handler="winGestionGuardar()" />--%>
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Window ID="winSyndates"
                runat="server"
                Title="Window Form"
                WidthSpec="70vw"
                MaxWidth="750"
                Height="300"
                Modal="true"
                Centered="true"
                Resizable="false"
                Layout="FitLayout"
                Cls="winForm-resp"
                Hidden="true">
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <Items>
                    <ext:FormPanel ID="FormPanel2"
                        Cls="formGris formResp"
                        runat="server"
                        OverflowY="Auto"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:FormPanel ID="pnFormElement"
                                Cls="formGris formResp"
                                runat="server">
                                <Items>
                                    <ext:Container runat="server" ID="ctElement" PaddingSpec="0 32" Cls="ctForm-resp ctForm-resp-col3">
                                        <Items>
                                            <ext:Checkbox runat="server"
                                                ID="Checkbox3"
                                                Cls="chkLabel chk-form"
                                                MarginSpec="32px 0 0 0"
                                                BoxLabel="Site Name"
                                                BoxLabelAlign="After" />
                                            <ext:ComboBox runat="server"
                                                ID="ComboBox2"
                                                FieldLabel="Element"
                                                LabelAlign="Top">
                                                <Items>
                                                    <ext:ListItem Text="Elemento 1" />
                                                    <ext:ListItem Text="Elemento 2" />
                                                    <ext:ListItem Text="Elemento 3" />
                                                    <ext:ListItem Text="Elemento 4" />
                                                </Items>
                                                <Triggers>
                                                    <ext:FieldTrigger meta:resourceKey="LimpiarLista"
                                                        Icon="Clear"
                                                        QTip="Limpiar Lista" />
                                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                        IconCls="ico-reload"
                                                        QTip="Recargar Lista" />
                                                </Triggers>
                                            </ext:ComboBox>
                                            <ext:Checkbox runat="server"
                                                ID="Checkbox1"
                                                Cls="chkLabel chk-form"
                                                MarginSpec="32px 0 0 0"
                                                BoxLabel="New Plan"
                                                BoxLabelAlign="After" />
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:FormPanel>
                            <ext:FormPanel ID="pnSchedule"
                                Cls="formGris formResp"
                                runat="server"
                                Hidden="true"
                                OverflowY="Auto">
                                <Items>
                                    <ext:Container runat="server" ID="ctSchedule" PaddingSpec="0 32" Cls="ctForm-resp ctForm-resp-col3">
                                        <Items>
                                            <ext:ComboBox runat="server"
                                                ID="ComboBox4"
                                                FieldLabel="Period"
                                                LabelAlign="Top">
                                                <Items>
                                                    <ext:ListItem Text="Periodo 1" />
                                                    <ext:ListItem Text="Periodo 2" />
                                                    <ext:ListItem Text="Periodo 3" />
                                                    <ext:ListItem Text="Periodo 4" />
                                                </Items>
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
                                                ID="DateField1"
                                                FieldLabel="Start Date"
                                                LabelAlign="Top"
                                                MinDate="<%# DateTime.Now %>"
                                                AutoDataBind="true"
                                                Vtype="daterange"
                                                Format="<%$ Resources:Comun, FormatFecha %>">
                                                <%--<CustomConfig>
                                                    <ext:ConfigItem Name="endDateField" Value="txtFechaFin" Mode="Value" />
                                                </CustomConfig>--%>
                                            </ext:DateField>
                                            <ext:DateField runat="server"
                                                meta:resourcekey="txtFechaFin"
                                                ID="DateField2"
                                                FieldLabel="End Date"
                                                LabelAlign="Top"
                                                MinDate="<%# DateTime.Now %>"
                                                AutoDataBind="true"
                                                Vtype="daterange"
                                                Format="<%$ Resources:Comun, FormatFecha %>">
                                                <%--<CustomConfig>
                                                    <ext:ConfigItem Name="endDateField" Value="txtFechaFin" Mode="Value" />
                                                </CustomConfig>--%>
                                            </ext:DateField>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:FormPanel>
                        </Items>
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="TbNavegacionTabs"
                                Dock="Top"
                                PaddingSpec="0 32 10"
                                Cls="tbGrey nav-vistas nav-vistasObligatorio"
                                Hidden="false">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkElement"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Text="Element">
                                        <Listeners>
                                            <Click Handler="NavegacionWinSyndates(this)"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkSchedule"
                                        Cls="lnk-navView lnk-noLine "
                                        Text="Shedule">
                                        <Listeners>
                                            <Click Handler="NavegacionWinSyndates(this)"></Click>
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server"
                                Height="32"
                                Cls="greytb"
                                PaddingSpec="0 32"
                                Dock="Top">
                                <Items>
                                    <ext:Label runat="server" ID="lblSyndates" Cls="bold fGrey" Text="Please, choose an element to assign the final dates" />
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server"
                                ID="Toolbar2"
                                Cls="greytb"
                                Dock="Bottom"
                                Padding="20">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button runat="server"
                                        ID="Button5"
                                        Width="100px"
                                        meta:resourceKey="btnPrev"
                                        IconCls="ico-close"
                                        Cls="btn-secondary-winForm"
                                        Text="<%$ Resources:Comun, strCerrar %>"
                                        Focusable="false"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="cerrarWinGestion()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="Button6"
                                        Cls="btn-ppal-winForm"
                                        Text="<%$ Resources:Comun, strGuardar %>"
                                        PressedCls="none">
                                        <Listeners>
                                            <%--<Click Handler="winGestionGuardar()" />--%>
                                        </Listeners>
                                    </ext:Button>

                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Window ID="winGestionNewTicket"
                runat="server"
                Title="New Ticket"
                Width="360"
                Height="520"
                Modal="true"
                Centered="true"
                Resizable="false"
                Layout="FitLayout"
                Cls="winForm-resp"
                Hidden="true">
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <Items>
                    <ext:FormPanel ID="FormPanel3"
                        Cls="formGris formResp"
                        runat="server"
                        OverflowY="Auto"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Container runat="server" ID="Container2" PaddingSpec="16 32" Cls="ctForm-resp ctForm-resp-col1">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="TextField1"
                                        FieldLabel="Name"
                                        LabelAlign="Top"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:ComboBox runat="server"
                                        ID="ComboBox5"
                                        FieldLabel="Category"
                                        LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="Category 1" />
                                            <ext:ListItem Text="Category 2" />
                                            <ext:ListItem Text="Category 3" />
                                            <ext:ListItem Text="Category 4" />
                                        </Items>
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
                                        ID="DateField3"
                                        FieldLabel="Date"
                                        LabelAlign="Top"
                                        MinDate="<%# DateTime.Now %>"
                                        AutoDataBind="true"
                                        Vtype="daterange"
                                        Format="<%$ Resources:Comun, FormatFecha %>">
                                        <%--<CustomConfig>
                                                    <ext:ConfigItem Name="endDateField" Value="txtFechaFin" Mode="Value" />
                                                </CustomConfig>--%>
                                    </ext:DateField>
                                    <ext:TextArea
                                        ID="txtDescription"
                                        runat="server"
                                        FieldLabel="Description"
                                        LabelAlign="Top"
                                        Mode="local"
                                        MaxLength="250"
                                        Scrollable="Vertical"
                                        RawText=""
                                        WidthSpec="100%"
                                        Cls=""
                                        Editable="true">
                                        <Listeners>
                                        </Listeners>
                                    </ext:TextArea>
                                </Items>
                            </ext:Container>
                        </Items>
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="Toolbar6"
                                Cls="greytb"
                                Dock="Bottom"
                                Padding="20">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button runat="server"
                                        ID="Button7"
                                        Width="100px"
                                        meta:resourceKey="btnPrev"
                                        IconCls="ico-close"
                                        Cls="btn-secondary-winForm"
                                        Text="<%$ Resources:Comun, strCerrar %>"
                                        Focusable="false"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="cerrarWinGestion()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="Button8"
                                        Cls="btn-ppal-winForm"
                                        Text="<%$ Resources:Comun, strGuardar %>"
                                        PressedCls="none">
                                        <Listeners>
                                            <%--<Click Handler="winGestionGuardar()" />--%>
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Viewport ID="vwResp"
                runat="server"
                Layout="FitLayout"
                Flex="1"
                OverflowY="auto">
                <Items>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain"
                                BodyCls="tbGrey">
                                <DockedItems>
                                    <ext:Toolbar runat="server" ID="tbFiltrosYSliders" Dock="Top" Cls="tbGrey tbNoborder " Hidden="true" Layout="HBoxLayout" Flex="1">
                                        <Items>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Toolbar runat="server"
                                                ID="tbSliders"
                                                Dock="Top"
                                                Hidden="false"
                                                MinHeight="36"
                                                Cls="tbGrey tbNoborder">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnPrev"
                                                        IconCls="ico-prev-w"
                                                        Cls="btnMainSldr SliderBtn"
                                                        Handler="loadPanelByBtns('Prev');"
                                                        Disabled="true">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnNext"
                                                        IconCls="ico-next-w"
                                                        Cls="SliderBtn"
                                                        Handler="loadPanelByBtns('Next');"
                                                        Disabled="false">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server" ID="wrapComponenteCentral" Layout="HBoxLayout" BodyCls="tbGrey" MaxWidth="1700">
                                        <Listeners>
                                            <AfterRender Handler="showPanelsByWindowSize();"></AfterRender>
                                            <Resize Handler="showPanelsByWindowSize();"></Resize>
                                        </Listeners>
                                        <LayoutConfig>
                                            <ext:HBoxLayoutConfig Align="Stretch"></ext:HBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:GridPanel ID="gridWOSites"
                                                runat="server"
                                                Header="true"
                                                Flex="12"
                                                EnableColumnHide="false"
                                                SelectionMemory="false"
                                                StoreID=""
                                                HideHeaders="true"
                                                Region="West"
                                                ForceFit="true"
                                                MaxWidth="300"
                                                Title="SITES"
                                                Cls="gridPanel"
                                                OverflowX="Hidden"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server" ID="tlbBase" Dock="top" Cls="tlbGrid" OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnInfo"
                                                                Cls="btnInfo"
                                                                AriaLabel="Info"
                                                                ToolTip=""
                                                                Disabled="false"
                                                                Handler="NewTickets();" />
                                                            <ext:Button runat="server"
                                                                ID="btnRefrescar"
                                                                Cls="btnRefrescar"
                                                                AriaLabel="Refrescar"
                                                                Handler=""
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>" />
                                                            <ext:Button runat="server"
                                                                ID="btnDescargar"
                                                                Cls="btnDescargar"
                                                                AriaLabel="Descargar"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Handler="">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                Text="With work orders"
                                                                EnableToggle="true"
                                                                ID="btnWithWO"
                                                                Cls="btnActivarDesactivarV2"
                                                                PressedCls="btnActivarDesactivarV2Pressed"
                                                                MinWidth="160"
                                                                Pressed="false"
                                                                TextAlign="Left"
                                                                Focusable="false"
                                                                OverCls="none">
                                                                <Listeners>
                                                                    <%--<Click Fn="" />--%>
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar runat="server"
                                                        Dock="Top"
                                                        Cls="tlbGrid">
                                                        <Items>
                                                            <ext:ComboBox meta:resourceKey="cmbProyecto"
                                                                ID="cmbProyecto"
                                                                runat="server"
                                                                DisplayField="Tipo"
                                                                Editable="true"
                                                                Scrollable="Vertical"
                                                                OverflowX="Hidden"
                                                                WidthSpec="100%"
                                                                MaxWidth="280"
                                                                Cls="txtSearchC"
                                                                EmptyText="Projects"
                                                                AllowBlank="true">
                                                                <Items>
                                                                    <ext:ListItem Text="Project 1" />
                                                                    <ext:ListItem Text="Project 2" />
                                                                    <ext:ListItem Text="Project 3" />
                                                                    <ext:ListItem Text="Project 4" />
                                                                </Items>
                                                                <Triggers>
                                                                    <ext:FieldTrigger meta:resourceKey="LimpiarLista"
                                                                        Icon="Clear"
                                                                        QTip="Limpiar Lista" />
                                                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                                        IconCls="ico-reload"
                                                                        QTip="Recargar Lista" />
                                                                </Triggers>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="toolBarFiltro"
                                                        Cls="tlbGrid">
                                                        <Items>
                                                            <ext:TextField
                                                                ID="txtSearchPrecios"
                                                                Cls="txtSearchC"
                                                                runat="server"
                                                                EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                LabelWidth="50"
                                                                WidthSpec="100%"
                                                                MaxWidth="280"
                                                                EnableKeyEvents="true">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Search" />
                                                                    <ext:FieldTrigger Icon="clear" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <Render Fn="FieldSearch" Buffer="250" />
                                                                    <Change Fn="FiltrarColumnas" Buffer="250" />
                                                                </Listeners>
                                                            </ext:TextField>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Store>
                                                    <ext:Store ID="Store1" runat="server">
                                                        <Model>
                                                            <ext:Model runat="server">
                                                                <Fields>
                                                                    <ext:ModelField Name="Name" />
                                                                    <ext:ModelField Name="Code" />
                                                                    <ext:ModelField Name="Description" />
                                                                    <ext:ModelField Name="Alert" />
                                                                    <ext:ModelField Name="Contact" />
                                                                    <ext:ModelField Name="Location" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:TemplateColumn
                                                            runat="server"
                                                            ID="templateVersiones"
                                                            DataIndex=""
                                                            MenuDisabled="true"
                                                            HideTitleEl="true" Selectable="false"
                                                            Text=""
                                                            Flex="5">
                                                            <Template runat="server">
                                                                <Html>
                                                                    <tpl for=".">
                                                                        <div class="contGridData">
                                                                            <ul class="ulGrid">
                                                                                <li class="liGrid">
                                                                                    <div class="contData">
                                                                                        <p class="name">{Name}</p>
                                                                                        <p class="code fGreen">{Code}</p>
                                                                                        <p class="descriptionItem fGrey">{Description}</p>
                                                                                    </div>
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                    </tpl>
                                                                </Html>
                                                            </Template>
                                                        </ext:TemplateColumn>
                                                        <ext:Column runat="server"
                                                            Cls="col-Alert"
                                                            MinWidth="40"
                                                            MaxWidth="40"
                                                            DataIndex="Alert"
                                                            ToolTip="Alert"
                                                            Align="End"
                                                            ID="colAlert"
                                                            Flex="1"
                                                            TdCls="column">
                                                            <Renderer Fn="AlertRender"></Renderer>
                                                        </ext:Column>
                                                        <ext:Column runat="server"
                                                            Cls="col-Contact"
                                                            MinWidth="40"
                                                            MaxWidth="40"
                                                            DataIndex="Contact"
                                                            ToolTip="Contact"
                                                            Align="Center"
                                                            ID="colContact"
                                                            Flex="1"
                                                            TdCls="column">
                                                            <Renderer Fn="ContactRender"></Renderer>
                                                        </ext:Column>
                                                        <ext:Column runat="server"
                                                            Cls="col-Location"
                                                            MinWidth="40"
                                                            MaxWidth="40"
                                                            DataIndex="Location"
                                                            ToolTip="Location"
                                                            Align="Start"
                                                            ID="colLocation"
                                                            Flex="1"
                                                            TdCls="column">
                                                            <Renderer Fn="LocationRender"></Renderer>
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel runat="server"
                                                        ID="GridRowSelect"
                                                        Mode="Single">
                                                        <Listeners>
                                                            <%--<Select Fn="Grid_RowSelect" />--%>
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <BottomBar>
                                                    <ext:PagingToolbar runat="server" StoreID="StoreGridMain" Cls="PgToolBMainGrid" ID="PagingToolbar1" MaintainFlex="true" Flex="8" HideRefresh="true" DisplayInfo="false" OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:ComboBox runat="server" Cls="comboGrid" ID="ComboBox1" MaxWidth="65">
                                                                <Items>
                                                                    <ext:ListItem Text="10" />
                                                                    <ext:ListItem Text="20" />
                                                                    <ext:ListItem Text="30" />
                                                                    <ext:ListItem Text="40" />
                                                                </Items>
                                                                <SelectedItems>
                                                                    <ext:ListItem Value="20" />
                                                                </SelectedItems>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:PagingToolbar>
                                                </BottomBar>
                                                <Plugins>
                                                    <ext:GridFilters runat="server"
                                                        ID="gridFilters"
                                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                        meta:resourceKey="GridFilters">
                                                    </ext:GridFilters>
                                                    <ext:CellEditing runat="server"
                                                        ClicksToEdit="2" />
                                                </Plugins>
                                            </ext:GridPanel>

                                            <ext:Panel runat="server"
                                                ID="pnCol1"
                                                Cls="grdNoHeader"
                                                Region="Center"
                                                Flex="3"
                                                Layout="VBoxLayout"
                                                MinWidth="200"
                                                OverflowX="Auto"
                                                OverflowY="Auto">
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                </LayoutConfig>
                                                <DockedItems>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="Toolbar4"
                                                        Dock="Top"
                                                        Height="42"
                                                        Padding="0"
                                                        Cls="tlbGrid c-Grid tlbHeader">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnCloseShowVisorTreeP"
                                                                IconCls="ico-hide-menu"
                                                                Cls="btnSbCategory"
                                                                Height="42"
                                                                Handler="showOnlySecundary();">
                                                            </ext:Button>
                                                            <ext:Label runat="server"
                                                                Cls="HeaderLblVisor"
                                                                Text="WO - RECURSO SELECCIONADO" />
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Items>
                                                    <ext:GridPanel runat="server"
                                                        ID="gridWODetails"
                                                        Region="Center"
                                                        Hidden="false"
                                                        Header="false"
                                                        Flex="1"
                                                        StoreID=""
                                                        AutoLoad="false"
                                                        SelectionMemory="false"
                                                        Cls="gridPanel"
                                                        EnableColumnHide="false"
                                                        OverflowX="Hidden"
                                                        OverflowY="Auto">
                                                        <Listeners>
                                                            <AfterRender Handler="GridColHandlerDinamicoV2(this)"></AfterRender>
                                                            <Resize Handler="GridColHandlerDinamicoV2(this)"></Resize>
                                                        </Listeners>
                                                        <DockedItems>
                                                            <ext:Toolbar runat="server" ID="tlbBaseDetalle" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        ID="btnAnadir"
                                                                        Cls="btnAnadir"
                                                                        AriaLabel="Añadir"
                                                                        Handler="AgregarEditar();"
                                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnEliminarDetalle"
                                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                        meta:resourceKey="btnEliminar"
                                                                        Cls="btnEliminar"
                                                                        Handler="" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnRefrescarDetalle"
                                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                        meta:resourceKey="btnRefrescar"
                                                                        Cls="btnRefrescar"
                                                                        Handler="" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnDescargarDetalle"
                                                                        ToolTip="Descargar"
                                                                        meta:resourceKey="btnRefrescar"
                                                                        Cls="btnDescargar"
                                                                        Handler="" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnCalendar"
                                                                        ToolTip="Calendar"
                                                                        meta:resourceKey="btnCalendar"
                                                                        Cls="btnCalendar-grey"
                                                                        Handler="parent.NavegacionTabs" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnContribuyentes"
                                                                        ToolTip="Contribuyentes"
                                                                        meta:resourceKey="btnContribuyentes"
                                                                        Cls="btnContribuyentes"
                                                                        Handler="" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnTiposComparticiones"
                                                                        ToolTip="Tipos Comparticiones"
                                                                        meta:resourceKey="btnTiposComparticiones"
                                                                        Cls="btnTiposComparticiones"
                                                                        Handler="" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnJoinOrders"
                                                                        ToolTip="JoinOrders"
                                                                        meta:resourceKey="btnJoinOrders"
                                                                        Cls="btnJoinOrders"
                                                                        Handler="JoinOrders();" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnSyndates"
                                                                        ToolTip="Syndates"
                                                                        meta:resourceKey="btnSyndates"
                                                                        Cls="btnSyndates"
                                                                        Handler="Syndates();" />
                                                                    <ext:Button runat="server"
                                                                        ID="Button2"
                                                                        Hidden="false"
                                                                        Cls="btnReinversion-subM"
                                                                        ToolTip="Menu Reinversion">
                                                                        <Menu>
                                                                            <ext:Menu runat="server">
                                                                                <Items>
                                                                                    <ext:MenuItem
                                                                                        meta:resourceKey="mnuButton3"
                                                                                        ID="MenuItem1"
                                                                                        runat="server"
                                                                                        Text="Launch Reinvestment process"
                                                                                        IconCls="ico-CntxMenuReinversion" />
                                                                                    <ext:MenuItem
                                                                                        meta:resourceKey="mnuButton4"
                                                                                        ID="MenuItem2"
                                                                                        runat="server"
                                                                                        Text="Merge reinvestment process"
                                                                                        IconCls="ico-CntxMenuReinversion-Merge" />
                                                                                </Items>
                                                                            </ext:Menu>
                                                                        </Menu>
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="Button3"
                                                                        Cls="btnEstadosWorkFlow-subM"
                                                                        AriaLabel=""
                                                                        ToolTip="Workflow">
                                                                        <Menu>
                                                                            <ext:Menu runat="server">
                                                                                <Items>
                                                                                    <ext:MenuItem
                                                                                        meta:resourceKey="mnuButton5"
                                                                                        ID="mnuDwldExcel"
                                                                                        runat="server"
                                                                                        Text="Workflow"
                                                                                        IconCls="ico-CntxMenuEstados" />
                                                                                    <ext:MenuItem
                                                                                        meta:resourceKey="mnuButton6"
                                                                                        ID="mnuDwldLoadSites"
                                                                                        runat="server"
                                                                                        Text="Workflow"
                                                                                        IconCls="ico-CntxMenuEstados" />
                                                                                </Items>
                                                                            </ext:Menu>
                                                                        </Menu>
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnMoverAUsuario"
                                                                        Hidden="false"
                                                                        Cls="btnMoverAUsuario-subM"
                                                                        ToolTip="Menu Mover a usuario">
                                                                        <Menu>
                                                                            <ext:Menu runat="server">
                                                                                <Items>
                                                                                    <ext:MenuItem
                                                                                        meta:resourceKey="mnuButton3"
                                                                                        ID="MenuItem3"
                                                                                        runat="server"
                                                                                        Text="Launch Reinvestment process"
                                                                                        IconCls="ico-CntxMenuMoverUsuario" />
                                                                                    <ext:MenuItem
                                                                                        meta:resourceKey="mnuButton4"
                                                                                        ID="MenuItem4"
                                                                                        runat="server"
                                                                                        Text="Merge reinvestment process"
                                                                                        IconCls="ico-CntxMenuMoverUsuario" />
                                                                                </Items>
                                                                            </ext:Menu>
                                                                        </Menu>
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnFiltrosDetalle"
                                                                        ToolTip="Mostrar Filtros"
                                                                        meta:resourceKey="btnFiltros"
                                                                        Cls="btnFiltros"
                                                                        Handler="parent.MostrarPnFiltros();" />
                                                                    <ext:ToolbarFill />
                                                                    <ext:Button runat="server"
                                                                        Text="<%$ Resources:Comun, strMostrarInactivos %>"
                                                                        EnableToggle="true"
                                                                        ID="btnActivosPanel"
                                                                        Cls="btnActivarDesactivarV2"
                                                                        PressedCls="btnActivarDesactivarV2Pressed"
                                                                        MinWidth="160"
                                                                        Pressed="false"
                                                                        TextAlign="Left"
                                                                        Focusable="false"
                                                                        OverCls="none">
                                                                        <Listeners>
                                                                            <Click Fn="" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Toolbar>
                                                            <ext:Container runat="server"
                                                                ID="tblfiltros"
                                                                Cls=""
                                                                Dock="Top">
                                                                <Content>
                                                                    <local:toolbarFiltros
                                                                        ID="cmpFiltroContactos"
                                                                        runat="server"
                                                                        Stores=""
                                                                        MostrarComboMisFiltros="true"
                                                                        MostrarBotones="true" />
                                                                </Content>
                                                            </ext:Container>
                                                        </DockedItems>
                                                        <Store>
                                                            <ext:Store ID="Store3" runat="server">
                                                                <Model>
                                                                    <ext:Model runat="server">
                                                                        <Fields>
                                                                            <ext:ModelField Name="Action" />
                                                                            <ext:ModelField Name="Type" />
                                                                            <ext:ModelField Name="Affected" />
                                                                            <ext:ModelField Name="Ticket" />
                                                                            <ext:ModelField Name="Status" />
                                                                            <ext:ModelField Name="Project" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:Store>
                                                        </Store>
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:Column runat="server"
                                                                    Text="Action"
                                                                    MinWidth="120"
                                                                    DataIndex="Action"
                                                                    Flex="1"
                                                                    ID="ColAction" />
                                                                <ext:Column runat="server"
                                                                    Text="Type"
                                                                    MinWidth="120"
                                                                    DataIndex="Type"
                                                                    Flex="1"
                                                                    ID="ColType" />
                                                                <ext:Column runat="server"
                                                                    Text="Affected"
                                                                    MinWidth="120"
                                                                    DataIndex="Affected"
                                                                    Flex="1"
                                                                    TdCls="linkNumber"
                                                                    ID="ColAffected" />
                                                                <ext:Column runat="server"
                                                                    Cls="col-Alert"
                                                                    MinWidth="90"
                                                                    MaxWidth="90"
                                                                    DataIndex="Ticket"
                                                                    ToolTip="Ticket"
                                                                    ID="ColTicket"
                                                                    Text="Tickets"
                                                                    Flex="1"
                                                                    TdCls="column">
                                                                    <Renderer Fn="AlertRender"></Renderer>
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    MinWidth="120"
                                                                    Text="Status"
                                                                    DataIndex="Status"
                                                                    Flex="1"
                                                                    ID="ColStatus">
                                                                    <Renderer Fn="StatusRender"></Renderer>
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    MinWidth="120"
                                                                    Text="Project"
                                                                    DataIndex="Project"
                                                                    Flex="1"
                                                                    ID="ColProject" />
                                                                <ext:WidgetColumn ID="ColMore"
                                                                    runat="server"
                                                                    Cls="NoOcultar col-More"
                                                                    TdCls="btnMore"
                                                                    DataIndex=""
                                                                    Align="Center"
                                                                    Text="<%$ Resources:Comun, strMas %>"
                                                                    Hidden="false"
                                                                    MinWidth="90"
                                                                    MaxWidth="90">
                                                                    <Widget>
                                                                        <ext:Button runat="server"
                                                                            Width="90"
                                                                            OverCls="Over-btnMore"
                                                                            PressedCls="Pressed-none"
                                                                            FocusCls="Focus-none"
                                                                            Cls="btnColMoreX">
                                                                            <Listeners>
                                                                                <Click Fn="MostrarPanelMoreInfo" />
                                                                            </Listeners>
                                                                        </ext:Button>
                                                                    </Widget>
                                                                </ext:WidgetColumn>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:CheckboxSelectionModel
                                                                runat="server"
                                                                ID="GridRowSelectDetalle"
                                                                Mode="Multi">
                                                                <Listeners>
                                                                    <%--<Select Fn="Grid_RowSelectDetalle" />--%>
                                                                    <%--<Deselect Fn="DeseleccionarGrilla" />--%>
                                                                </Listeners>
                                                            </ext:CheckboxSelectionModel>
                                                        </SelectionModel>
                                                        <BottomBar>
                                                            <ext:PagingToolbar runat="server" StoreID="StoreGridMain" Cls="PgToolBMainGrid" ID="PagingToolbar2" MaintainFlex="true" Flex="8" HideRefresh="true" DisplayInfo="false" OverflowHandler="Scroller">
                                                                <Items>
                                                                    <ext:ComboBox runat="server" Cls="comboGrid" ID="ComboBox9" MaxWidth="65">
                                                                        <Items>
                                                                            <ext:ListItem Text="10" />
                                                                            <ext:ListItem Text="20" />
                                                                            <ext:ListItem Text="30" />
                                                                            <ext:ListItem Text="40" />
                                                                        </Items>
                                                                        <SelectedItems>
                                                                            <ext:ListItem Value="20" />
                                                                        </SelectedItems>
                                                                    </ext:ComboBox>
                                                                </Items>
                                                            </ext:PagingToolbar>
                                                        </BottomBar>
                                                        <View>
                                                            <ext:GridView runat="server" TrackOver="false" EmptyText="<h1>No matching results</h1>" EnableTextSelection="true">
                                                            </ext:GridView>
                                                        </View>
                                                    </ext:GridPanel>
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
