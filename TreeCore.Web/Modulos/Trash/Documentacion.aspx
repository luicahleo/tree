<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Documentacion.aspx.cs" Inherits="TreeCore.PaginasComunes.Documentacion" %>


<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="css/styleDocumentacion.css" rel="stylesheet" type="text/css" />
    <script src="js/Documentacion.js"></script>
    <!--<script src="js/common.js"></script>-->

    <script runat="server">

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {

                this.StoreGridMain.DataSource = this.Data;

            }



        }


        private object[] Data
        {
            get
            {
                return new object[]
                {
                new object[] { "wordico", "Invoice_energy", "7123512RR","Energy Facturacion", "Saving 2020", DateTime.Now },
                new object[] { "PDFico", "Invoice_energy", "7123512RR","Energy Facturacion", "Saving 2020", DateTime.Now },
                new object[] { "excelico", "Invoice_energy", "7123512RR","Energy Facturacion", "Saving 2020", DateTime.Now },
                new object[] { "OtherDoc", "Invoice_energy", "7123512RR","Energy Facturacion", "Saving 2020", DateTime.Now },
                new object[] { "PowerPico", "Invoice_energy", "7123512RR","Energy Facturacion", "Saving 2020", DateTime.Now },


    };
            }
        }



    </script>


    <script type="text/javascript">
        var align = function (tb) {
            tb.alignTo(Ext.getBody(), 'tr-tr', [-10, 10]);
        };
    </script>

</head>



<ext:Action
    runat="server"
    ID="ShowSitesAction"
    IconCls="ico-visible"
    Text="Previsualizar"
    Disabled="true"
    Handler="" />

<ext:Action
    runat="server"
    ID="ShowMapAction"
    IconCls="btnDescargarCtxMenu"
    Text="Descargar"
    Disabled="true"
    Handler="" />

<ext:Action
    ID="ShowTrackingAction"
    runat="server"
    IconCls="btnVersiones"
    Text="Versiones Anteriores"
    Disabled="true"
    Handler="" />



<ext:Action
    ID="Action1"
    runat="server"
    IconCls="btnBasura-ctxMenu"
    Text="Eliminar"
    Disabled="true"
    Handler="" />


<ext:Menu runat="server"
    ID="ContextMenu">
    <Items>
        <ext:ActionRef runat="server" Action="#{ShowSitesAction}" />
        <ext:ActionRef runat="server" Action="#{ShowMapAction}" />
        <ext:ActionRef runat="server" Action="#{ShowTrackingAction}" />
        <ext:ActionRef runat="server" Action="#{Action1}" />
    </Items>
</ext:Menu>


<body>
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">


                <Listeners>
                    <WindowResize Handler="GridResizer()" />
                </Listeners>
            </ext:ResourceManager>
            <%-- //WINDOWS--%>



            <ext:Viewport ID="vwResp" runat="server" Layout="">
                <Items>
                    <ext:Button runat="server" ID="btnCollapseAsR" Cls="btn-trans" Handler="hidePn();" Hidden="true"></ext:Button>
                    <ext:Panel runat="server"
                        Hidden="true"
                        ID="pnNavVistas"
                        Cls="pnNavVistas"
                        AnchorVertical="15%"
                        AriaRole="navigation">
                        <Items>
                            <ext:Container runat="server"
                                ID="conNavVistas"
                                Cls="nav-vistas">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkSites"
                                        meta:resourceKey="lnkSites"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Text="CURRENT VIEW">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkLocation"
                                        meta:resourceKey="lnkLocation"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="VIEW 2">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkAdditional"
                                        meta:resourceKey="lnkAdditional"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="VIEW 3">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkDashboard"
                                        meta:resourceKey="lnkDashboard"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="VIEW 4">
                                    </ext:HyperlinkButton>

                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                    <ext:Container runat="server" ID="ctBtnSldr">
                        <Items>
                            <ext:Button runat="server" ID="btnPrevSldr" IconCls="ico-prev" Cls="btnMainSldr" Handler="moveCtSldr(this);"></ext:Button>
                            <ext:Button runat="server" ID="btnNextSldr" IconCls="ico-next" Handler="moveCtSldr(this);" Disabled="true"></ext:Button>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="ctSlider" runat="server">
                        <Items>
                            <ext:Container ID="ctMain1" runat="server" Layout="">
                                <Items>
                                    <ext:TreePanel
                                        Hidden="false"
                                        Cls="gridPanel TreePnl"
                                        ID="TreePanelMain"
                                        runat="server"
                                        Scroll="None"
                                        RootVisible="false"
                                        Title="Files Tree">


                                        <TopBar>

                                            <ext:Toolbar runat="server" Dock="Top" ID="toolbar2" Hidden="false" StyleSpec="border-style: hidden !important;">
                                                <Items>

                                                    <ext:Button runat="server" ID="Button4" Text="" Cls="btn-trans btnAnadir" IconCls="">
                                                        <Listeners>
                                                            <Click Fn="ShowPanelAddMeds" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server" ID="Button5" Text="" Cls="btn-trans btnEditar" IconCls="">
                                                    </ext:Button>
                                                    <ext:Button runat="server" ID="Button6" Text="" Cls="btn-trans btnEliminar" IconCls="">
                                                        <Listeners>
                                                            <Click Fn="HidePanelAddMeds" />
                                                        </Listeners>
                                                    </ext:Button>

                                                    <ext:ToolbarFill />


                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>


                                        <Root>
                                            <ext:Node Text="ROOT" Expanded="false">
                                                <Children>
                                                    <ext:Node Text="Administrative License">
                                                        <Children>
                                                            <ext:Node Text="Concertos">
                                                                <Children>
                                                                    <ext:Node Text="No. 1 - C" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 2 - B-Flat Major" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 3 - C Minor" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 4 - G Major" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 5 - E-Flat Major" Icon="Music" Leaf="true" />
                                                                </Children>
                                                            </ext:Node>
                                                            <ext:Node Text="Quartets">
                                                                <Children>
                                                                    <ext:Node Text="Six String Quartets" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="Three String Quartets" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="Grosse Fugue for String Quartets" Icon="Music" Leaf="true" />
                                                                </Children>
                                                            </ext:Node>
                                                            <ext:Node Text="Sonatas">
                                                                <Children>
                                                                    <ext:Node Text="Sonata in A Minor" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="sonata in F Major" Icon="Music" Leaf="true" />
                                                                </Children>
                                                            </ext:Node>
                                                            <ext:Node Text="Symphonies">
                                                                <Children>
                                                                    <ext:Node Text="No. 1 - C Major" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 2 - D Major" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 3 - E-Flat Major" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 4 - B-Flat Major" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 5 - C Minor" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 6 - F Major" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 7 - A Major" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 8 - F Major" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 9 - D Minor" Icon="Music" Leaf="true" />
                                                                </Children>
                                                            </ext:Node>
                                                        </Children>
                                                    </ext:Node>
                                                    <ext:Node Text="Administrative Widthdrawal" Expandable="false">
                                                        <Children>
                                                            <ext:Node Text="Concertos">
                                                                <Children>
                                                                    <ext:Node Text="Violin Concerto" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="Double Concerto - A Minor" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="Piano Concerto No. 1 - D Minor" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="Piano Concerto No. 2 - B-Flat Major" Icon="Music" Leaf="true" />
                                                                </Children>
                                                            </ext:Node>
                                                            <ext:Node Text="Quartets">
                                                                <Children>
                                                                    <ext:Node Text="Piano Quartet No. 1 - G Minor" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="Piano Quartet No. 2 - A Major" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="Piano Quartet No. 3 - C Minor" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="Piano Quartet No. 3 - B-Flat Minor" Icon="Music" Leaf="true" />
                                                                </Children>
                                                            </ext:Node>
                                                            <ext:Node Text="Sonatas">
                                                                <Children>
                                                                    <ext:Node Text="Two Sonatas for Clarinet - F Minor" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="Two Sonatas for Clarinet - E-Flat Major" Icon="Music" Leaf="true" />
                                                                </Children>
                                                            </ext:Node>
                                                            <ext:Node Text="Symphonies">
                                                                <Children>
                                                                    <ext:Node Text="No. 1 - C Minor" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 2 - D Minor" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 3 - F Major" Icon="Music" Leaf="true" />
                                                                    <ext:Node Text="No. 4 - E Minor" Icon="Music" Leaf="true" />
                                                                </Children>
                                                            </ext:Node>
                                                        </Children>
                                                    </ext:Node>
                                                    <ext:Node Text="Billing" Expanded="true">
                                                        <Children>
                                                            <ext:Node Text="Billing 2019 (3)">
                                                                <Children>
                                                                    <ext:Node Text="Piano Concerto No. 12" Icon="Music" Leaf="true" />

                                                                </Children>
                                                            </ext:Node>
                                                            <ext:Node Text="Billing 2020 (4)">
                                                                <Children>
                                                                    <ext:Node Text="Piano Concerto No. 12" Icon="Music" Leaf="true" />

                                                                </Children>
                                                            </ext:Node>

                                                        </Children>
                                                    </ext:Node>

                                                    <ext:Node Text="Contractor Assignment Certificate (8)"></ext:Node>
                                                    <ext:Node Text="Decommisioning Report (2)"></ext:Node>
                                                    <ext:Node Text="Energy Bill" Expandable="false"></ext:Node>
                                                    <ext:Node Text="Enviromental Permit" Expandable="false"></ext:Node>
                                                    <ext:Node Text="Executive Project (3)"></ext:Node>
                                                </Children>
                                            </ext:Node>

                                        </Root>

                                    </ext:TreePanel>


                                </Items>

                            </ext:Container>
                            <ext:Container ID="ctMain2" runat="server">
                                <Items>


                                    <ext:GridPanel
                                        Hidden="false"
                                        runat="server"
                                        ForceFit="false"
                                        ID="gridMain1"
                                        Height="900"
                                        Cls="gridPanel "
                                        OverflowX="Auto"
                                        ContextMenuID="ContextMenu">

                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tlbBase" Dock="Top" Cls="tlbGrid">
                                                <Items>
                                                    <ext:Button runat="server" ID="btnAnadir" Cls="btn-trans" AriaLabel="Añadir" ToolTip="Añadir" Handler="anadir();"></ext:Button>
                                                    <ext:Button runat="server" ID="btnEditar" Cls="btn-trans" AriaLabel="Editar" ToolTip="Editar" Handler="editar();"></ext:Button>
                                                    <ext:Button runat="server" ID="btnEliminar" Cls="btn-trans" AriaLabel="Eliminar" ToolTip="Eliminar" Handler="eliminar();"></ext:Button>
                                                    <ext:Button runat="server" ID="btnRefrescar" Cls="btn-trans" AriaLabel="Refrescar" ToolTip="Refrescar" Handler="refrescar();"></ext:Button>
                                                    <ext:Button runat="server" ID="btnDescargar" Cls="btnDescargar" AriaLabel="Descargar" ToolTip="Descargar" Handler="this.up('grid').print();"></ext:Button>
                                                    <ext:Button runat="server" ID="btnCrearFiltros" Cls="btn-trans btnFiltros" AriaLabel="Ver Workflow" ToolTip="Ver Workflow" Handler="ShowWorkFlow();"></ext:Button>

                                                </Items>
                                            </ext:Toolbar>


                                            <ext:Toolbar runat="server" ID="tbFiltros" Cls="tlbGrid" Layout="ColumnLayout">
                                                <Items>

                                                    <ext:TextField
                                                        ID="txtSearch"
                                                        Cls="txtSearchC"
                                                        runat="server"
                                                        EmptyText="Search"
                                                        LabelWidth="50">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Clear" />
                                                            <ext:FieldTrigger Icon="Search" />
                                                        </Triggers>
                                                    </ext:TextField>

                                                    <ext:FieldContainer runat="server" ID="FCCombos" Cls="FloatR FCCombos" Layout="HBoxLayout">
                                                        <Defaults>
                                                            <ext:Parameter Name="margin" Value="0 5 0 0" Mode="Value" />
                                                        </Defaults>
                                                        <LayoutConfig>
                                                            <ext:HBoxLayoutConfig Align="Stretch" />
                                                        </LayoutConfig>
                                                        <Items>
                                                            <ext:ComboBox runat="server" ID="cmbProyectos" Cls="comboGrid  " EmptyText="Projects" Flex="1">
                                                                <Items>
                                                                    <ext:ListItem Text="Proyecto 1" />
                                                                    <ext:ListItem Text="Proyecto 2" />
                                                                </Items>
                                                            </ext:ComboBox>

                                                            <ext:ComboBox runat="server" ID="cmbEmplazamientos" Cls="comboGrid  " EmptyText="Tipologías" Flex="1">
                                                                <Items>
                                                                    <ext:ListItem Text="Tipología 1" />
                                                                    <ext:ListItem Text="Tipología 2" />
                                                                    <ext:ListItem Text="Tipología 3" />
                                                                    <ext:ListItem Text="Tipología 4" />
                                                                </Items>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:FieldContainer>




                                                    <ext:FieldContainer runat="server" ID="ContButtons" Cls="FloatR ContButtons">
                                                        <Items>
                                                            <ext:Button runat="server" Width="30" ID="Button1" Cls="btn-trans btnColumnas" AriaLabel="Duplicar Tipología" ToolTip="Duplicar Tipología"></ext:Button>
                                                            <ext:Button runat="server" Width="30" ID="btnClearFilters" Cls="btn-trans btnRemoveFilters" AriaLabel="Quitar Filtros" ToolTip="Quitar Filtros"></ext:Button>
                                                            <ext:Button runat="server" Width="30" ID="Button3" Cls="btn-trans btnFiltroNegativo" AriaLabel="Ver Workflow" ToolTip="Ver Workflow" Handler="ShowWorkFlow();"></ext:Button>
                                                        </Items>
                                                    </ext:FieldContainer>




                                                </Items>
                                            </ext:Toolbar>


                                        </DockedItems>
                                        <Store>
                                            <ext:Store ID="StoreGridMain" runat="server">
                                                <Model>
                                                    <ext:Model runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="Icon" />
                                                            <ext:ModelField Name="Name" />
                                                            <ext:ModelField Name="Code" Type="Float" />
                                                            <ext:ModelField Name="Module" />
                                                            <ext:ModelField Name="Project" />
                                                            <ext:ModelField Name="Date" Type="Date" />

                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel>
                                            <Columns>



                                                <ext:Column runat="server" Cls="col-functionalities" DataIndex="Icon" MaxWidth="60" ID="Colicon" Flex="1">
                                                    <Renderer Fn="DocIconRender"></Renderer>
                                                </ext:Column>

                                              

                                                <ext:Column runat="server" Text="Name" DataIndex="Name" MinWidth="230" Flex="1" ID="ColName">
                                                </ext:Column>



                                                <ext:Column runat="server" Text="Code" DataIndex="Code" Width="200" ID="ColCode">
                                                </ext:Column>


                                                <ext:Column runat="server" Text="Module" DataIndex="Module" Width="200" ID="ColModule" />
                                                <ext:Column runat="server" Text="Project" DataIndex="Project" Width="150" ID="ColProject" />
                                                <ext:DateColumn runat="server" Text="Date" DataIndex="Date" Width="150" ID="ColDate" />
                                                <ext:WidgetColumn ID="ColMore" runat="server" Width="105" Cls="col-More" DataIndex="" Align="Center" Text="More" Hidden="false" MinWidth="70">
                                                    <Widget>
                                                        <ext:Button runat="server" Width="90" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColMore" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                                    </Widget>
                                                </ext:WidgetColumn>



                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server" />
                                        </SelectionModel>

                                        <BottomBar>
                                            <ext:PagingToolbar runat="server" StoreID="StoreGridMain" Cls="PgToolBMainGrid" ID="PGToolBarGrid" MaintainFlex="true" Flex="8">
                                                <Items>
                                                    <ext:ComboBox runat="server" Cls="comboGrid" ID="cmbTbRegistros" Flex="2">
                                                        <Items>
                                                            <ext:ListItem Text="10 Registros" />
                                                            <ext:ListItem Text="20 Registros" />
                                                            <ext:ListItem Text="30 Registros" />
                                                            <ext:ListItem Text="40 Registros" />
                                                        </Items>
                                                        <SelectedItems>
                                                            <ext:ListItem Value="20 Registros" />
                                                        </SelectedItems>

                                                    </ext:ComboBox>

                                                </Items>

                                            </ext:PagingToolbar>
                                        </BottomBar>


                                    </ext:GridPanel>


                                </Items>

                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Panel ID="pnAsideR" runat="server" Hidden="true">
                        <Items>
                            <ext:Label runat="server" IconCls="ico-head-Notif" Text="Head Label Aside" Cls="lblHeadAside"></ext:Label>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>

