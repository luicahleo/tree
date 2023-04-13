<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoChecklist.aspx.cs" Inherits="TreeCore.PaginasComunes.SeguimientoChecklist" %>


<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="css/styleSeguimientoChecklist.css" rel="stylesheet" type="text/css" />
    <script src="js/SeguimientoChecklist.js"></script>
    <!--<script src="js/common.js"></script>
    <script src="../../JS/common.js"></script>-->

    <script runat="server">

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {

                this.GridPanelMain.Store.Primary.DataSource = TreeCore.PaginasComunes.SeguimientoChecklist.DataGridP1;
                this.GridPanelP3.Store.Primary.DataSource = TreeCore.PaginasComunes.SeguimientoChecklist.DataTask2;


            }



        }



    </script>


</head>
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

            <ext:Window meta:resourceKey="winUndone" runat="server" ID="winUndone" Hidden="true" Layout="FormLayout" MinWidth="430" Title="Mark As Undone" BodyCls="winalert-body">
                <Items>
                    <ext:FormPanel runat="server" BodyCls="winalert-body">
                        <Items>
                            <ext:Label meta:resourceKey="msgWindow" ID="msgWindow" runat="server" Text="Cambiar el estado a no hecho, borrara todos los campos y dejara la tarea lista para empezar de nuevo"></ext:Label>


                        </Items>

                        <DockedItems>
                            <ext:Toolbar runat="server" ID="tbButtonsWin1" Dock="Bottom">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button meta:resourceKey="btnCancelWin" runat="server" ID="btnCancelWin" Cls="btn-cancel" MinWidth="150" Text="Cancelar " />
                                    <ext:Button meta:resourceKey="btnAcceptWin" runat="server" ID="btnAcceptWin" Cls="btn-accept" MinWidth="190" Text="Marca como no hecha" />

                                </Items>
                            </ext:Toolbar>
                        </DockedItems>

                    </ext:FormPanel>
                </Items>
            </ext:Window>




            <ext:Viewport ID="vwResp" runat="server" Layout="" Height="900">
                <Items>
                    <ext:Button meta:resourceKey="btnCollapseAsR" runat="server" ID="btnCollapseAsR" Cls="btn-trans" Handler="hidePn();" Hidden="true"></ext:Button>
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

                                    <ext:GridPanel
                                        meta:resourceKey="GridPanelMain"
                                        ID="GridPanelMain"
                                        runat="server"
                                        Height="900"
                                        OverflowY="Scroll"
                                        Cls="grdNoHeader gridPanel cellAlgTop">

                                        <Store>
                                            <ext:Store
                                                ID="Storesearch"
                                                runat="server"
                                                Buffered="true"
                                                RemoteFilter="true"
                                                LeadingBufferZone="1000"
                                                PageSize="50">

                                                <Model>
                                                    <ext:Model runat="server" IDProperty="Id">
                                                        <Fields>
                                                            <ext:ModelField Name="NombreTask" />
                                                            <ext:ModelField Name="DateTask" />
                                                            <ext:ModelField Name="bDone" Type="Boolean" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>

                                            </ext:Store>
                                        </Store>
                                        <ColumnModel runat="server">
                                            <Columns>



                                                <ext:TemplateColumn runat="server" DataIndex="" MenuDisabled="true" Text="" Flex="2">
                                                    <Template runat="server">
                                                        <Html>
                                                            <tpl for=".">
                                                            <div class="customCol1">
                                                                <p style="font-weight:bold !important">{NombreTask}</p> 
                                                                <div  class="customColDiv1">
                                                                    {DateTask}
                                                                                         
                                                                </div>
                                                            </div>
                                                        </tpl>
                                                        </Html>
                                                    </Template>
                                                </ext:TemplateColumn>

                                                <ext:Column
                                                    runat="server"
                                                    DataIndex="bDone"
                                                    MinWidth="120"
                                                    Sortable="false"
                                                    Align="Start"
                                                    Flex="1">
                                                    <Renderer Fn="TrueFalseWHeightRender" />


                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                        <View>
                                            <ext:GridView runat="server" TrackOver="false" EmptyText="<h1 style='margin:20px;'>No matching results</h1>" />
                                        </View>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server" PruneRemoved="false" Mode="Multi" />
                                        </SelectionModel>
                                        <DockedItems>

                                            <ext:Toolbar runat="server" Dock="Top" ID="TBTitle">
                                                <Items>
                                                    <ext:Label meta:resourceKey="TituloTopMain" ID="TituloTopMain" runat="server" Cls="big-lbl-title" Text="Esquina Bellavista HBTS"></ext:Label>
                                                    <ext:ToolbarFill></ext:ToolbarFill>
                                                    <ext:Button runat="server" Cls="btnLocalizacion" OverCls="clear-over" FocusCls="clear-focus" PressedCls="green-pressed-loc" EnableToggle="true" Text="" TextAlign="Right">
                                                        <Listeners>
                                                            <Click Fn="ShowMapPn" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>



                                            <ext:Toolbar runat="server" ID="TBSubtitle" Cls="TBSubtitle">
                                                <Items>
                                                    <ext:Label meta:resourceKey="SubtituloM" ID="SubtituloM" runat="server" Cls="subtitle-code" Text="CODIGO: 200"></ext:Label>
                                                    <ext:Label meta:resourceKey="SubCodSubtitulo" ID="SubCodSubtitulo" runat="server" Cls="subtitle-subcode" Text="TASC - Contract Acquisition"></ext:Label>
                                                    <ext:ToolbarFill></ext:ToolbarFill>
                                                </Items>
                                            </ext:Toolbar>





                                            <ext:Toolbar runat="server" ID="TBTools" Cls="PadTBMid">
                                                <Items>

                                                    <ext:TextField
                                                        meta:resourceKey="TxtBusqPanelMain"
                                                        ID="TxtBusqPanelMain"
                                                        Flex="6"
                                                        runat="server"
                                                        EmptyText="Search">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Clear" />
                                                            <ext:FieldTrigger Icon="Search" />
                                                        </Triggers>

                                                    </ext:TextField>

                                                    <ext:ComboBox
                                                        meta:resourceKey="cmbFiltrosPanelMain"
                                                        ID="cmbFiltrosPanelMain"
                                                        Flex="6"
                                                        runat="server"
                                                        EmptyText="More To Less">
                                                    </ext:ComboBox>

                                                    <ext:ToolbarFill Flex="1"></ext:ToolbarFill>

                                                    <ext:Button meta:resourceKey="btnFiltrosMain" runat="server" ID="btnFiltrosMain" EnableToggle="true" Text="Filtros" Cls="btnfiltGridconicon" MinWidth="90" IconCls="ico-filter-white" IconAlign="Left">
                                                        <Listeners>
                                                            <Click Fn="ShowFiltersTb" />
                                                        </Listeners>
                                                    </ext:Button>

                                                </Items>
                                            </ext:Toolbar>


                                            <ext:Toolbar runat="server" Dock="Top" ID="toolbarfiltros1" Layout="HBoxLayout" Cls="bld-cnt-fonts PadTBMid" Hidden="true">
                                                <Items>



                                                    <ext:ComboBox
                                                        meta:resourceKey="cmbFiltrosCategoria"
                                                        ID="cmbFiltrosCategoria"
                                                        Flex="1"
                                                        FieldLabel="Category"
                                                        runat="server"
                                                        EmptyText="PText"
                                                        LabelAlign="Top">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Clear" />
                                                        </Triggers>
                                                    </ext:ComboBox>

                                                    <ext:ComboBox
                                                        meta:resourceKey="cmbFiltrosFecha"
                                                        ID="cmbFiltrosFecha"
                                                        Flex="1"
                                                        FieldLabel="Date"
                                                        LabelAlign="Top"
                                                        runat="server"
                                                        EmptyText="PText">
                                                    </ext:ComboBox>



                                                </Items>
                                            </ext:Toolbar>



                                            <ext:Toolbar runat="server" Dock="Top" ID="toolbarfiltros2" Layout="HBoxLayout" Cls="bld-cnt-fonts PadTBBot" Hidden="true">
                                                <Items>

                                                    <ext:ComboBox
                                                        meta:resourceKey="cmbFiltrosElementos"
                                                        ID="cmbFiltrosElementos"
                                                        Flex="1"
                                                        runat="server"
                                                        FieldLabel="Element"
                                                        LabelAlign="Top"
                                                        EmptyText="PText">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Clear" />
                                                        </Triggers>

                                                    </ext:ComboBox>

                                                    <ext:Button runat="server"
                                                        meta:resourceKey="btnDoneUndoneTasks"
                                                        Flex="1"
                                                        MinWidth="160"
                                                        ID="btnDoneUndoneTasks"
                                                        EnableToggle="true"
                                                        Cls="btn-toggleGrid-Cntract"
                                                        Text="Show tasks done"
                                                        TextAlign="Left"
                                                        AriaLabel="Ver Inactivos"
                                                        ToolTip="Ver Inactivos">
                                                    </ext:Button>




                                                </Items>
                                            </ext:Toolbar>


                                        </DockedItems>
                                    </ext:GridPanel>

                                </Items>

                            </ext:Container>
                            <ext:Container ID="ctMain2" runat="server">
                                <Items>

                                    <ext:Panel meta:resourceKey="metaPanelP2" runat="server" ID="PanelP2" Title="Measures Task" Cls=" gridPanel" Height="900" Layout="" Hidden="false">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tbBotMainCT1" Cls="" Dock="Bottom">
                                                <Items>
                                                    <ext:ToolbarFill></ext:ToolbarFill>
                                                    <ext:Button meta:resourceKey="btnLimpiarCampos" ID="btnLimpiarCampos" runat="server" Text="Clear Fields" Cls="btn-secondary-winForm" Width="140" IconCls="ico-reset">
                                                        <Listeners>
                                                            <Click Fn="BotonClear" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button meta:resourceKey="btnGuardarCambios" ID="btnGuardarCambios" runat="server" Text="Save Changes" Cls="btn-ppal-winForm" Width="140" IconCls="ico-checked" />

                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>

                                            <ext:Container runat="server" ID="Container1" PaddingSpec="0 10 0 10" Cls="">
                                                <Items>


                                                    <ext:Toolbar meta:resourceKey="toolbarDoneUndone" runat="server" Dock="Top" ID="toolbarDoneUndone" Hidden="false" StyleSpec="border-style: hidden !important;">
                                                        <Items>
                                                            <ext:ToolbarFill></ext:ToolbarFill>
                                                            <ext:Button runat="server"
                                                                meta:resourceKey="btnDondeUndoneP2"
                                                                ID="btnDondeUndoneP2"
                                                                EnableToggle="true"
                                                                Width="110"
                                                                Cls="btn-toggleGrid-Cntract-Small"
                                                                Text="Undone"
                                                                TextAlign="Left"
                                                                AriaLabel="Ver Inactivos"
                                                                ToolTip="Ver Inactivos">
                                                                <Listeners>
                                                                    <Click Handler="BotonDoneP2();" />
                                                                </Listeners>
                                                            </ext:Button>


                                                        </Items>

                                                    </ext:Toolbar>



                                                    <ext:Toolbar meta:resourceKey="toolbarFiltrosP2" runat="server" Dock="Top" ID="toolbarFiltrosP2" Layout="ColumnLayout" Cls="toolbar-noborder" Hidden="true">
                                                        <Items>

                                                            <ext:Container runat="server" ID="Container3" ColumnWidth="1">
                                                                <Items>
                                                                    <ext:Label meta:resourceKey="TbLblFecha" ID="TbLblFecha" runat="server" Text="Date"></ext:Label>

                                                                    <ext:ComboBox
                                                                        meta:resourceKey="txtFiltrosP2"
                                                                        ID="txtFiltrosP2"
                                                                        runat="server"
                                                                        EmptyText="PText">
                                                                    </ext:ComboBox>

                                                                </Items>
                                                            </ext:Container>


                                                            <ext:Container runat="server" ID="Container4" ColumnWidth="1">
                                                                <Items>
                                                                    <ext:Label meta:resourceKey="lblElementP2" ID="lblElementP2" runat="server" Text="Element"></ext:Label>
                                                                    <ext:ComboBox
                                                                        meta:resourceKey="lblElement2P2"
                                                                        ID="lblElement2P2"
                                                                        runat="server"
                                                                        EmptyText="PText">
                                                                        <Triggers>
                                                                            <ext:FieldTrigger Icon="Clear" />
                                                                        </Triggers>
                                                                    </ext:ComboBox>
                                                                </Items>
                                                            </ext:Container>



                                                        </Items>
                                                    </ext:Toolbar>


                                                    <ext:Container runat="server" Cls="Override100InnerH ctFormTask" ID="Cont1Main"
                                                        Html="<h4>TASK DESCRIPTION</h4>Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores">
                                                    </ext:Container>

                                                    <ext:ComboBox meta:resourceKey="cmbForm" runat="server" LabelAlign="Top" FieldLabel="Form" ID="cmbForm" Cls="full-w"></ext:ComboBox>
                                                    <ext:TextArea meta:resourceKey="txtAResult" runat="server" ID="txtAResult" FieldLabel="Result" LabelAlign="Top" Cls="full-w"></ext:TextArea>
                                                    <ext:FileUploadField meta:resourceKey="UpldDocs" runat="server" ID="UpldDocs" FieldLabel="Upload Image" LabelAlign="Top" Cls="full-w" ButtonText="" BaseCls="Testbd" ComponentCls="testbtn" FocusCls="btn-trans" IconCls="OverlapIconUpF"></ext:FileUploadField>

                                                </Items>
                                            </ext:Container>




                                        </Items>
                                    </ext:Panel>

                                </Items>

                            </ext:Container>
                            <ext:Container ID="ctMain3" runat="server">
                                <Items>

                                    <ext:GridPanel
                                        meta:resourceKey="GridPanelP3"
                                        Title="Measurements"
                                        ID="GridPanelP3"
                                        runat="server"
                                        ForceFit="true"
                                        Height="900"
                                        Cls="gridPanel">

                                        <Store>
                                            <ext:Store
                                                ID="StoreGridP3"
                                                runat="server"
                                                Buffered="true"
                                                RemoteFilter="true"
                                                LeadingBufferZone="1000"
                                                PageSize="50">

                                                <Model>
                                                    <ext:Model runat="server" IDProperty="Id">
                                                        <Fields>
                                                            <ext:ModelField Name="Id" />
                                                            <ext:ModelField Name="NombreDato" />
                                                            <ext:ModelField Name="Dato" />
                                                            <ext:ModelField Name="TipoDato" />
                                                            <ext:ModelField Name="bDone" Type="Boolean" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>

                                            </ext:Store>
                                        </Store>
                                        <ColumnModel runat="server">
                                            <Columns>


                                                <ext:TemplateColumn runat="server" DataIndex="" MenuDisabled="true" Text="" Flex="3">
                                                    <Template runat="server">
                                                        <Html>
                                                            <tpl for=".">
                                                            <div class="customCol1">
                                                                <p style="font-weight:bold !important">{NombreDato}</p> 
                                                                <div  class="customColDiv1">
                                                                    {Dato}
                                                                    <div class="TipodatoAlign"  >
                                                                    {TipoDato}
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            </tpl>
                                                        </Html>
                                                    </Template>
                                                </ext:TemplateColumn>

                                                <ext:Column
                                                    Flex="2"
                                                    runat="server"
                                                    DataIndex="bDone"
                                                    MinWidth="120"
                                                    MaxWidth="230"
                                                    Sortable="false"
                                                    Align="Default"
                                                    Height="60">
                                                    <Renderer Fn="TrueFalseWHeightRender" />






                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>

                                        <DockedItems>

                                            <ext:Toolbar runat="server" Dock="Top" StyleSpec="border-style: hidden !important;" Cls="PadTBTop">
                                                <Items>

                                                    <ext:Button meta:resourceKey="btnAnadirMeds" runat="server" ID="btnAnadirMeds" Text="" Cls="btn-trans btnAnadir" IconCls="btnAddMeds">
                                                        <Listeners>
                                                            <Click Fn="ShowPanelAddMeds" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button meta:resourceKey="btnEditar" runat="server" ID="btnEditar" Text="" Cls="btn-trans btnEditar" IconCls="">
                                                    </ext:Button>
                                                    <ext:Button meta:resourceKey="btnEliminar" runat="server" ID="btnEliminar" Text="" Cls="btn-trans btnEliminar" IconCls="">
                                                        <Listeners>
                                                            <Click Fn="HidePanelAddMeds" />
                                                        </Listeners>
                                                    </ext:Button>

                                                    <ext:ToolbarFill />

                                                    <ext:Button meta:resourceKey="btnToggleFiltrosP3" runat="server" ID="btnToggleFiltrosP3" EnableToggle="true" Text="Filtros" Cls="btnfiltGridconicon" MinWidth="100" IconCls="ico-filter-white" IconAlign="Left">
                                                        <Listeners>
                                                            <Click Fn="ShowFiltersP3" />
                                                        </Listeners>
                                                    </ext:Button>

                                                </Items>

                                            </ext:Toolbar>

                                            <ext:Toolbar runat="server" ID="tbPanelAdd" Hidden="true" Layout="VBoxLayout" Dock="Top" Cls="PadTBMid">
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>

                                                </LayoutConfig>

                                                <Items>
                                                    <ext:Toolbar runat="server" Dock="Top" ID="toolbar1" Layout="" Hidden="false" Cls="bld-cnt-fonts toolbar-noborder" Flex="1">
                                                        <Items>
                                                            <ext:TextField
                                                                meta:resourceKey="txtMed"
                                                                Flex="1"
                                                                FieldLabel="Titulo de la medición"
                                                                ID="txtMed"
                                                                runat="server"
                                                                EmptyText="p. e. Grosor del muro"
                                                                LabelAlign="Top">
                                                            </ext:TextField>

                                                            <ext:ComboBox
                                                                meta:resourceKey="cmbUnitMed"
                                                                Flex="1"
                                                                FieldLabel="Unidad De Medida"
                                                                LabelAlign="Top"
                                                                ID="cmbUnitMed"
                                                                runat="server"
                                                                EmptyText="PText">
                                                            </ext:ComboBox>


                                                        </Items>
                                                    </ext:Toolbar>

                                                    <ext:Toolbar runat="server" Layout="" ID="toolbarAddDelete" WidthSpec="100%">
                                                        <Items>
                                                            <ext:ToolbarFill></ext:ToolbarFill>
                                                            <ext:Button meta:resourceKey="btnCancelP3" ID="btnCancelP3" runat="server" Text="Cancelar" Cls="btn-secondary-winForm" IconCls="ico-reset" />
                                                            <ext:Button meta:resourceKey="btnAddMedicionP3" ID="btnAddMedicionP3" runat="server" Text="Añadir Medición" Cls="btn-ppal-winForm" IconCls="ico-checked" />
                                                        </Items>
                                                    </ext:Toolbar>

                                                </Items>
                                            </ext:Toolbar>

                                            <ext:Toolbar runat="server" Dock="Top" ID="toolbarFiltrosP3N1" Layout="HBoxLayout" Cls="bld-cnt-fonts toolbar-noborder PadTBMid" Hidden="true">
                                                <Items>



                                                    <ext:ComboBox
                                                        meta:resourceKey="cmbCategoriaFiltros"
                                                        ID="cmbCategoriaFiltros"
                                                        Flex="1"
                                                        FieldLabel="Category"
                                                        runat="server"
                                                        EmptyText="PText"
                                                        LabelAlign="Top">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Clear" />
                                                        </Triggers>
                                                    </ext:ComboBox>

                                                    <ext:ComboBox
                                                        meta:resourceKey="cmbFechaFiltrosP3"
                                                        ID="cmbFechaFiltrosP3"
                                                        Flex="1"
                                                        FieldLabel="Date"
                                                        LabelAlign="Top"
                                                        runat="server"
                                                        EmptyText="PText">
                                                    </ext:ComboBox>










                                                </Items>
                                            </ext:Toolbar>

                                            <ext:Toolbar runat="server" Dock="Top" ID="toolbarFiltrosP3N2" Layout="HBoxLayout" Cls="bld-cnt-fonts toolbar-noborder PadTBBot" Hidden="true">
                                                <Items>

                                                    <ext:ComboBox
                                                        meta:resourceKey="cmbElementosFiltrosP3"
                                                        ID="cmbElementosFiltrosP3"
                                                        Flex="1"
                                                        runat="server"
                                                        FieldLabel="Element"
                                                        LabelAlign="Top"
                                                        EmptyText="PText">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Clear" />
                                                        </Triggers>

                                                    </ext:ComboBox>

                                                    <ext:Button runat="server"
                                                        meta:resourceKey="cmbTasksDoneFiltrosP3"
                                                        ID="cmbTasksDoneFiltrosP3"
                                                        Flex="1"
                                                        EnableToggle="true"
                                                        Width="175"
                                                        Cls="btn-toggleGrid-Cntract"
                                                        Text="Show tasks done"
                                                        TextAlign="Left"
                                                        AriaLabel="Ver Inactivos"
                                                        ToolTip="Ver Inactivos">
                                                    </ext:Button>



                                                </Items>
                                            </ext:Toolbar>

                                        </DockedItems>

                                    </ext:GridPanel>

                                </Items>

                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Panel ID="pnAsideR" runat="server" Hidden="true">
                        <Items>
                            <ext:Label meta:resourceKey="HeadLabelAs" ID="HeadLabelAs" runat="server" IconCls="ico-head-Notif" Text="Head Label Aside" Cls="lblHeadAside"></ext:Label>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>

