<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoContrato.aspx.cs" Inherits="TreeCore.PaginasComunes.SeguimientoContrato" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Collections.Generic" %>


<script runat="server">
    public static string TEST_HTML_1 = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed egestas gravida nibh, quis porttitor felis
                    venenatis id. Nam sodales mollis quam eget venenatis. Aliquam metus lorem, tincidunt ut egestas imperdiet, convallis
                    lacinia tortor. Mauris accumsan, nisl et sodales tristique, massa dui placerat erat, at venenatis tortor libero nec
                    tortor. Pellentesque quis elit ac dolor commodo tincidunt. Curabitur lorem eros, tincidunt quis viverra id, lacinia
                    sed nisl. Quisque viverra ante eu nisl consectetur hendrerit.";

    public static string TEST_HTML_2 = @"<b>This tab is scrollable.</b><br /><br />
                    Aenean sit amet quam ipsum. Nam aliquet ullamcorper lorem, vel commodo neque auctor quis. Vivamus ac purus in
                    tortor tempor viverra eget a magna. Nunc accumsan dolor porta mauris consequat nec mollis felis mattis. Nunc ligula nisl,
                    tempor ut pellentesque et, viverra eget tellus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus sodales
                    rhoncus massa, sed lobortis risus euismod at. Suspendisse dictum, lectus vitae aliquam egestas, quam diam consequat augue,
                    non porta odio ante a dui. Vivamus lacus mi, ultrices sed feugiat elementum, ultrices et lectus. Donec aliquet hendrerit magna,
                    in venenatis ante faucibus ut. Duis non neque magna. Quisque iaculis luctus nibh, id pellentesque lorem egestas non. Phasellus
                    id risus eget felis auctor scelerisque. Fusce porttitor tortor eget magna pretium viverra. Sed tempor vulputate felis aliquam
                    scelerisque. Quisque eget libero non lectus tempus varius eu a tortor.
                    <br /><br />
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed egestas gravida nibh, quis porttitor felis
                    venenatis id. Nam sodales mollis quam eget venenatis. Aliquam metus lorem, tincidunt ut egestas imperdiet, convallis
                    lacinia tortor. Mauris accumsan, nisl et sodales tristique, massa dui placerat erat, at venenatis tortor libero nec
                    tortor. Pellentesque quis elit ac dolor commodo tincidunt. Curabitur lorem eros, tincidunt quis viverra id, lacinia
                    sed nisl. Quisque viverra ante eu nisl consectetur hendrerit.";
</script>


<!DOCTYPE html>

<html>
<head runat="server">
    <title>Seguimiento Contrato</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="css/styleSeguimientoContrato.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet">
    <script type="text/javascript" src="js/SeguimientoContrato.js"></script>
</head>
<body>
    <form runat="server">

        <ext:ResourceManager runat="server">
        </ext:ResourceManager>


        <ext:Viewport runat="server" ID="MainVw" Layout="FitLayout" OverflowY="Auto">
            <Items>

                <ext:TabPanel
                    MaxWidth="850"
                    MaxHeight="1000"
                    Cls="tabPnlCon"
                    ID="TabPanelContract"
                    runat="server"
                    Layout="FitLayout"
                    MarginSpec="0 0 0 0">
                    <Defaults>
                        <ext:Parameter Name="bodyPadding" Value="10" Mode="Raw" />
                    </Defaults>
                    <Items>
                        <ext:Panel
                            ID="PnlContrato12"
                            runat="server"
                            OverflowY="Auto"
                            Title="Rent"
                            Layout="FitLayout"
                            AutoDataBind="true">
                            <DockedItems>
                                <ext:Toolbar runat="server" Cls="" Dock="Bottom" Layout="">
                                    <Items>
                                        <ext:ToolbarFill Flex="2"></ext:ToolbarFill>
                                        <ext:Button meta:resourceKey="saveCRent" runat="server" ID="saveCRent" Cls="btn-secondary-winForm" MinWidth="100" Flex="1" Text="Restaurar Cambios" />
                                        <ext:Button meta:resourceKey="ResetCRent" runat="server" ID="ResetCRent" Cls="btn-ppal-winForm" MinWidth="90" Flex="1" Text="Guardar Cambios" />
                                    </Items>
                                </ext:Toolbar>
                            </DockedItems>
                            <Items>

                                <ext:Container runat="server" ID="ContWrapMain" Cls="ContWrapForm">
                                    <Items>
                                        <ext:TextField runat="server"
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            ID="TextField1"
                                            FieldLabel="Contract Num"
                                            LabelAlign="Top"
                                            AllowBlank="false"
                                            ValidationGroup="FORM"
                                            CausesValidation="true"
                                            EmptyText="Enter Number" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Contract Type"
                                            ID="TextField2"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Contract Type 2"
                                            ID="TextField3"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Owner Type"
                                            ID="TextField4"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Status"
                                            ID="TextField5"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Forecast Regeneration Date"
                                            ID="TextField6"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Current Expiration Date"
                                            ID="TextField7"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />

                                        <ext:TextField runat="server"
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            ID="TextField8"
                                            FieldLabel="Contract Name"
                                            LabelAlign="Top"
                                            AllowBlank="false"
                                            ValidationGroup="FORM"
                                            CausesValidation="true"
                                            EmptyText="Enter Number" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="CECO Society"
                                            ID="TextField9"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Cost Center (SAP)"
                                            ID="TextField10"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Asset Class (SAP)"
                                            ID="TextField11"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Authorization Group"
                                            ID="TextField12"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />

                                        <ext:Checkbox
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            BoxLabel="Legal Authority"
                                            ID="TextField13"
                                            Cls="chk-ctoTrck " />
                                        <ext:Checkbox
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            BoxLabel="Indefinite Test"
                                            ID="Checkbox2"
                                            Cls="chk-ctoTrck" />

                                        <ext:TextField runat="server"
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            ID="TextField14"
                                            FieldLabel="SAP Code"
                                            LabelAlign="Top"
                                            AllowBlank="false"
                                            ValidationGroup="FORM"
                                            CausesValidation="true"
                                            EmptyText="Enter Number" />

                                        <ext:Checkbox
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            BoxLabel="Expiry Date"
                                            ID="Checkbox1"
                                            Cls="chk-ctoTrck" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Contract Signature Date"
                                            ID="TextField16"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Initial Contract Date"
                                            ID="TextField17"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Contract End Date"
                                            ID="TextField23"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextArea
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            Cls="txtAreaForm"
                                            AllowBlank="false"
                                            FieldLabel="Risks"
                                            ID="TextArea1"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />



                                        <ext:TextField runat="server"
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            ID="TextField15"
                                            FieldLabel="SAP Code"
                                            LabelAlign="Top"
                                            AllowBlank="false"
                                            ValidationGroup="FORM"
                                            CausesValidation="true"
                                            EmptyText="Enter Number" />

                                        <ext:Checkbox
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            BoxLabel="Expiry Date"
                                            ID="Checkbox3"
                                            Cls="chk-ctoTrck" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Contract Signature Date"
                                            ID="TextField18"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Initial Contract Date"
                                            ID="TextField19"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Contract End Date"
                                            ID="TextField2029"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextArea
                                            Cls="txtAreaForm"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Risks"
                                            ID="TextField20"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />



                                        <ext:TextField runat="server"
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            ID="TextField26"
                                            FieldLabel="Currency"
                                            LabelAlign="Top"
                                            AllowBlank="false"
                                            ValidationGroup="FORM"
                                            CausesValidation="true"
                                            EmptyText="Enter Number" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Tax Society"
                                            ID="TextField27"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />

                                        <ext:RadioGroup
                                            Cls="txtAreaForm"
                                            meta:resourceKey="lnkSites"
                                            ID="RadioGroup1"
                                            runat="server"
                                            Width="300"
                                            Layout="VBoxLayout"
                                            GroupName="RadioGroup2">
                                            <Items>

                                                <ext:Component meta:resourceKey="RadioTlt" runat="server" Html="Prorroga:" Cls="radio-title" />
                                                <ext:Radio meta:resourceKey="RdLblAutomatic" runat="server" BoxLabel="Automatic" InputValue="4" Cls="radio-cto" />
                                                <ext:Radio meta:resourceKey="RdLblOptional" runat="server" BoxLabel="Optional" InputValue="5" Cls="radio-cto" />
                                                <ext:Radio meta:resourceKey="RdLblPreviousNegotiation" runat="server" BoxLabel="Previous Negotiation" InputValue="5" Checked="true" Cls="radio-cto" />
                                            </Items>
                                        </ext:RadioGroup>

                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Fecha Devengo"
                                            ID="TextField28"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />
                                        <ext:TextField
                                            Cls="formMaxHeight"
                                            meta:resourceKey="lnkSites"
                                            runat="server"
                                            AllowBlank="false"
                                            FieldLabel="Numer of Extensions Consumed"
                                            ID="TextField29"
                                            EmptyText="Enter Number"
                                            LabelAlign="Top" />


                                    </Items>
                                </ext:Container>

                            </Items>

                        </ext:Panel>

                        <ext:Panel
                            meta:resourceKey="TabAdditionals"
                            runat="server"
                            Title="Additionals"
                            Html="<%# TEST_HTML_1  %>"
                            AutoDataBind="true" />
                        <ext:Panel
                            runat="server"
                            Title="Owners"
                            Html="<%# TEST_HTML_1  %>"
                            AutoDataBind="true" />
                        <ext:Panel
                            runat="server"
                            Title="Contract"
                            Html="<%# TEST_HTML_1  %>"
                            AutoDataBind="true" />
                        <ext:Panel
                            runat="server"
                            Title="Concept"
                            Html="<%# TEST_HTML_1  %>"
                            AutoDataBind="true" />
                        <ext:Panel
                            runat="server"
                            Title="Info"
                            Html="<%# TEST_HTML_1  %>"
                            AutoDataBind="true" />


                    </Items>
                </ext:TabPanel>
            </Items>
        </ext:Viewport>

    </form>
</body>
</html>
