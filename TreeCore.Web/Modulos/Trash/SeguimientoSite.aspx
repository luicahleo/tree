<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoSite.aspx.cs" Inherits="TreeCore.PaginasComunes.SeguimientoSite" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Collections.Generic" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!X.IsAjaxRequest)
        {

            // this.GridPanelMain.Store.Primary.DataSource = TreeCore.PaginasComunes.SeguimientoPresupuestos.DataGridP1;

        }

    }

</script>


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
    <link href="css/styleSeguimientoSite.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet">
    <script type="text/javascript" src="js/SeguimientoSite.js"></script>

</head>
<body>
    <form runat="server">

        <ext:ResourceManager runat="server" />



        <ext:TabPanel
            Cls="tabPnlCon"
            ID="TabPanelSite"
            runat="server">
            <Defaults>
                <ext:Parameter Name="bodyPadding" Value="10" Mode="Raw" />
                <ext:Parameter Name="scrollable" Value="both" />
            </Defaults>



            <Items>
                <ext:Panel
                    ID="PanelSite"
                    runat="server"
                    Title="Site"
                    AutoDataBind="true">
                    <Items>
                        <ext:FormPanel runat="server" Cls="formWorkflow formGris">
                            <Items>

                                <ext:Container ID="wrapsitec" runat="server" Layout="ColumnLayout">
                                    <Items>


                                        <ext:Container runat="server" MarginSpec="0 12 0 12">
                                            <Items>

                                                <ext:TextField runat="server"
                                                    ID="TextField16"
                                                    FieldLabel="Name"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Landowner"
                                                    ID="TextField223"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Category"
                                                    ID="TextField233"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Type of Structure"
                                                    ID="TextField243"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />


                                            </Items>
                                        </ext:Container>

                                        <ext:Container runat="server" MarginSpec="0 12 0 12">
                                            <Items>

                                                <ext:TextField runat="server"
                                                    ID="TextField23"
                                                    FieldLabel="Code"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Landowner"
                                                    ID="ComboBox1"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Category"
                                                    ID="ComboBox2"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Type of Structure"
                                                    ID="ComboBox3"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />


                                            </Items>
                                        </ext:Container>

                                        <ext:Container runat="server" MarginSpec="-3 12 0 12">
                                            <Items>

                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Operator"
                                                    ID="ComboBox7"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Global State"
                                                    ID="ComboBox4"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Size of Site"
                                                    ID="ComboBox5"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Cluster"
                                                    ID="ComboBox6"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />





                                            </Items>
                                        </ext:Container>



                                    </Items>
                                </ext:Container>
                                <ext:Toolbar runat="server" Cls="toolbarbotContract" ID="toolbarSite">
                                    <Items>
                                        <ext:ToolbarFill></ext:ToolbarFill>
                                        <ext:Button runat="server" ID="Button11" Cls="btn-cancel" Width="150" Text="Restaurar Cambios" />
                                        <ext:Button runat="server" ID="Button12" Cls="btn-accept" Width="150" Text="Guardar Cambios" />
                                    </Items>
                                </ext:Toolbar>

                            </Items>
                        </ext:FormPanel>
                    </Items>
                </ext:Panel>

                <ext:Panel
                    ID="PanelLocation"
                    runat="server"
                    Title="Location"
                    AutoDataBind="true">
                    <Items>
                        <ext:FormPanel ID="PnlLocation" runat="server" Cls="formWorkflow formGris">
                            <Items>

                                <ext:Container runat="server" Layout="ColumnLayout" Flex="1" MaintainFlex="true">
                                    <Items>

                                        <ext:Container runat="server" Cls="FormCont12Marg">
                                            <Items>


                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Zone"
                                                    ID="ComboBox8"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Province"
                                                    ID="ComboBox9"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:TextField runat="server"
                                                    ID="TextField26"
                                                    FieldLabel="Address"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />


                                                <ext:Checkbox
                                                    runat="server"
                                                    BoxLabel="Degrees"
                                                    ID="chkDegrees"
                                                    Cls="chk-ctoTrck" />


                                            </Items>
                                        </ext:Container>

                                        <ext:Container runat="server" Cls="FormCont12Marg">
                                            <Items>

                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Country"
                                                    ID="ComboBox10"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Municipality"
                                                    ID="ComboBox11"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Zip code"
                                                    ID="ComboBox12"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:TextField runat="server"
                                                    ID="TextField30"
                                                    FieldLabel="Latitude"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />


                                            </Items>
                                        </ext:Container>

                                        <ext:Container runat="server" ID="LastCont" Cls="FormCont12Marg">
                                            <Items>

                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Region"
                                                    ID="ComboBox14"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="City"
                                                    ID="ComboBox15"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />



                                                <ext:Label
                                                    runat="server"
                                                    Text="Geolocation"
                                                    ID="ComboBox16"
                                                    IconCls="ico-geolocalizacion-grey" Cls="label-con" />


                                                <ext:TextField runat="server"
                                                    ID="TextField27"
                                                    FieldLabel="Longitude"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />


                                            </Items>
                                        </ext:Container>

                                    </Items>
                                </ext:Container>
                                <ext:Toolbar ID="toolbarLocation" runat="server" Cls="toolbarbotContract">
                                    <Items>
                                        <ext:ToolbarFill></ext:ToolbarFill>
                                        <ext:Button runat="server" ID="Button1" Cls="btn-cancel" Width="150" Text="Restaurar Cambios" />
                                        <ext:Button runat="server" ID="Button2" Cls="btn-accept" Width="150" Text="Guardar Cambios" />
                                    </Items>
                                </ext:Toolbar>

                            </Items>

                        </ext:FormPanel>
                    </Items>
                </ext:Panel>

                <ext:Panel
                    ID="PanelAdditional"
                    runat="server"
                    Title="Additional"
                    AutoDataBind="true">
                    <Items>
                        <ext:FormPanel ID="FormPnlAdditional" runat="server" Cls="formWorkflow formGris">
                            <Items>



                                <ext:Container runat="server" Layout="ColumnLayout">
                                    <Items>

                                        <ext:Container runat="server" MarginSpec="0 12 0 12">
                                            <Items>

                                                <ext:TextField runat="server"
                                                    ID="TextField28"
                                                    FieldLabel="Buyer Company"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:TextField runat="server"
                                                    ID="TextField31"
                                                    FieldLabel="Tower Code"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:TextField runat="server"
                                                    ID="TextField32"
                                                    FieldLabel="Land Registry Code"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:TextField runat="server"
                                                    ID="TextField33"
                                                    FieldLabel="VAR Average Income"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:TextField runat="server"
                                                    ID="TextField34"
                                                    FieldLabel="Number Of Depending Sites"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:TextArea runat="server"
                                                    ID="TextField35"
                                                    FieldLabel="Comments About Building"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />


                                            </Items>
                                        </ext:Container>

                                        <ext:Container runat="server" MarginSpec="0 12 0 12">
                                            <Items>

                                                <ext:TextField runat="server"
                                                    ID="TextField29"
                                                    FieldLabel="Mnemonic"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:TextField runat="server"
                                                    ID="TextField36"
                                                    FieldLabel="Telco Code"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:TextField runat="server"
                                                    ID="TextField37"
                                                    FieldLabel="Potential Of Site"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:TextField runat="server"
                                                    ID="TextField38"
                                                    FieldLabel="Supervisor"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />

                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Activation Date"
                                                    ID="ComboBox23"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />

                                                <ext:TextArea runat="server"
                                                    ID="TextArea1"
                                                    FieldLabel="Engineering Situation"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />


                                            </Items>
                                        </ext:Container>


                                        <ext:Container runat="server" MarginSpec="0 12 0 12">
                                            <Items>
                                                <ext:TextField runat="server"
                                                    ID="TextField39"
                                                    FieldLabel="Site Selling to Third Party"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:TextField runat="server"
                                                    ID="TextField40"
                                                    FieldLabel="SAP Code"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:TextField runat="server"
                                                    ID="TextField41"
                                                    FieldLabel="Court Record"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:TextField runat="server"
                                                    ID="TextField42"
                                                    FieldLabel="Altitude of Site"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />
                                                <ext:ComboBox
                                                    runat="server"
                                                    FieldLabel="Deactivation Date"
                                                    ID="ComboBox25"
                                                    EmptyText="Enter Number"
                                                    LabelAlign="Top" />
                                                <ext:TextArea runat="server"
                                                    ID="TextArea2"
                                                    FieldLabel="Comments about Teams"
                                                    LabelAlign="Top"
                                                    AllowBlank="false"
                                                    ValidationGroup="FORM"
                                                    CausesValidation="true"
                                                    EmptyText="Enter Number" />


                                            </Items>
                                        </ext:Container>


                                    </Items>
                                </ext:Container>
                                <ext:Container runat="server" ID="CajaBottom" Layout="FitLayout" MarginSpec="0 12 0 12">
                                    <Items>

                                        <ext:TextArea runat="server"
                                            ID="TextArea3"
                                            FieldLabel="Comments about Teams"
                                            LabelAlign="Top"
                                            AllowBlank="false"
                                            ValidationGroup="FORM"
                                            CausesValidation="true"
                                            EmptyText="Enter Number" />



                                    </Items>
                                </ext:Container>
                                <ext:Toolbar ID="toolbarAdditional" runat="server" Cls="toolbarbotContract">
                                    <Items>
                                        <ext:ToolbarFill></ext:ToolbarFill>
                                        <ext:Button runat="server" ID="Button3" Cls="btn-cancel" Width="150" Text="Restaurar Cambios" />
                                        <ext:Button runat="server" ID="Button4" Cls="btn-accept" Width="150" Text="Guardar Cambios" />
                                    </Items>
                                </ext:Toolbar>


                            </Items>
                        </ext:FormPanel>
                    </Items>
                </ext:Panel>



            </Items>
        </ext:TabPanel>


    </form>
</body>
</html>
