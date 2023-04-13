<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrdersTickets.aspx.cs" Inherits="TreeCore.ModWorkOrders.pages.WorkOrdersTickets" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <!--<script type="text/javascript" src="../../JS/common.js"></script>-->
    <script type="text/javascript" src="js/WorkOrdersTickets.js"></script>
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

            <ext:Window ID="winGestionWorkOrders"
                runat="server"
                Title="Window Form"
                WidthSpec="70vw"
                MaxWidth="650"
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
                            <ext:Container runat="server" ID="ctForm" PaddingSpec="16 32" Cls="ctForm-resp ctForm-resp-col2">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtName"
                                        FieldLabel="Name"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        ID="txtCode"
                                        FieldLabel="Code"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        ID="txtDescription"
                                        FieldLabel="Description"
                                        LabelAlign="Top"
                                        AllowBlank="false"
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
                                    <ext:DateField runat="server"
                                        meta:resourcekey="txtFecha"
                                        ID="txtFecha"
                                        FieldLabel="Execution Date"
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

            <ext:Window ID="winReinvestmentProcess"
                runat="server"
                Title="Window Form"
                WidthSpec="70vw"
                MaxWidth="580"
                HeightSpec="80vh"
                MaxHeight="380"
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
                    <ext:FormPanel ID="FormGestion2"
                        Cls="formGris formResp"
                        runat="server"
                        OverflowY="Auto"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Container runat="server" ID="ctForm2" PaddingSpec="16 32" Cls="ctForm-resp ctForm-resp-col2">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtNameReinv"
                                        FieldLabel="Name"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Text="TicketName"
                                        Editable="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        ID="txtCodeReinv"
                                        FieldLabel="Code"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        Text="TicketCode"
                                        Editable="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:ComboBox runat="server"
                                        ID="cmbProjectReinv"
                                        FieldLabel="Project"
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
                                        ID="cmbSupplierReinv"
                                        FieldLabel="Supplier"
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
                                </Items>
                            </ext:Container>
                        </Items>
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="tbSaveFormReinv"
                                Cls="greytb"
                                Dock="Bottom"
                                Padding="20">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button runat="server"
                                        ID="btnCerrarReinv"
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
                                        ID="btnAgregarReinv"
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
                    <ext:Panel runat="server"
                        ID="pnCol1"
                        Cls="grdNoHeader"
                        Region="Center"
                        Flex="1"
                        MinWidth="200"
                        BodyPadding="16"
                        AnchorVertical="100%"
                        AnchorHorizontal="100%"
                        OverflowX="Auto"
                        OverflowY="Auto">
                        <DockedItems>
                            <ext:Toolbar runat="server" ID="tlbBaseDetalle" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        meta:resourceKey="btnEliminar"
                                        Disabled="true"
                                        Cls="btnEliminar"
                                        Handler="Eliminar();" />
                                    <ext:Button runat="server"
                                        ID="btnRefrescar"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        meta:resourceKey="btnRefrescar"
                                        Cls="btnRefrescar"
                                        Handler="Refrescar();" />
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        ToolTip="Descargar"
                                        meta:resourceKey="btn"
                                        Cls="btnDescargar"
                                        Handler="Descargar();" />
                                    <ext:Button runat="server"
                                        ID="btnConvertirCorrectivo"
                                        ToolTip=""
                                        meta:resourceKey="btnConvertirCorrectivo"
                                        Cls="btnConvertirCorrectivo"
                                        Handler="openForm();" />
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
                                                        IconCls="ico-CntxMenuReinversion"
                                                        Handler="openReinvestment();" />
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
                                        ID="btnFiltros"
                                        ToolTip="Mostrar Filtros"
                                        meta:resourceKey="btnFiltros"
                                        Cls="btnFiltros"
                                        Handler="parent.MostrarPnFiltros();" />
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar
                                runat="server"
                                ID="tbFiltros"
                                Cls="tlbGrid"
                                Layout="ColumnLayout">
                                <Items>
                                    <ext:TextField
                                        ID="txtSearch"
                                        Cls="txtSearchC"
                                        runat="server"
                                        EmptyText="<%$ Resources:Comun, strBuscar %>"
                                        LabelWidth="50"
                                        Width="250"
                                        EnableKeyEvents="true">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" />
                                        </Triggers>
                                        <Listeners>
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:FieldContainer
                                        runat="server"
                                        ID="FCCombos"
                                        Cls="FloatR FCCombos"
                                        Layout="HBoxLayout">
                                        <Defaults>
                                            <ext:Parameter
                                                Name="margin"
                                                Value="0 5 0 0"
                                                Mode="Value" />
                                        </Defaults>
                                        <LayoutConfig>
                                            <ext:HBoxLayoutConfig Align="Middle" />
                                        </LayoutConfig>
                                        <Items>
                                            <ext:ToolbarFill />
                                            <ext:FieldContainer runat="server" ID="ContButtons" Cls="ContButtons">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        Width="30"
                                                        ID="btnClearFilters"
                                                        Cls="btn-trans btnRemoveFilters"
                                                        AriaLabel="Quitar Filtros"
                                                        ToolTip="<%$ Resources:Comun, strQuitarFiltro %>">
                                                        <Listeners>
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        Width="30"
                                                        ID="Button1"
                                                        Cls="btn-trans btnFiltroNegativo"
                                                        AriaLabel="Negative Filter"
                                                        ToolTip="Negative Filter"
                                                        Handler="ShowWorkFlow();"
                                                        Hidden="false">
                                                    </ext:Button>
                                                </Items>
                                            </ext:FieldContainer>
                                            <ext:ComboBox meta:resourceKey="cmbMisFiltros"
                                                runat="server"
                                                ID="cmbMisFiltros"
                                                Cls="comboGrid ClscmbMisFiltros FloatR"
                                                EmptyText="My Filters"
                                                Flex="3"
                                                Hidden="false">
                                                <Items>
                                                    <ext:ListItem Text="Filtro 1" />
                                                    <ext:ListItem Text="Filtro 2" />
                                                    <ext:ListItem Text="Filtro 3" />
                                                    <ext:ListItem Text="Filtro 4" />
                                                    <ext:ListItem Text="Filtro 5" />
                                                </Items>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:FieldContainer>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server" ID="tlbSelectAll" Dock="Top" Height="32">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnSelectAll"
                                        Text="Select All"
                                        Cls="btnSelectAll"
                                        Padding="0"
                                        Handler="selectionAll();"
                                        FocusCls="none" />
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <Items>
                            <ext:DataView
                                runat="server"
                                ID="DataView1"
                                SingleSelect="true"
                                AnchorVertical="100%"
                                ItemSelector="div.tickets">
                                <Store>
                                    <ext:Store ID="Store3" runat="server">
                                        <Model>
                                            <ext:Model runat="server">
                                                <Fields>
                                                    <ext:ModelField Name="Nombre" />
                                                    <ext:ModelField Name="Correo" />
                                                    <ext:ModelField Name="Tipo" />
                                                    <ext:ModelField Name="Fecha" />
                                                    <ext:ModelField Name="Titulo" />
                                                    <ext:ModelField Name="Descripcion" />
                                                    <ext:ModelField Name="Code" />
                                                    <ext:ModelField Name="Site" />
                                                    <ext:ModelField Name="Object" />
                                                    <ext:ModelField Name="Status" />
                                                    <ext:ModelField Name="Image" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <Tpl runat="server">
                                    <Html>
                                        <div id="items-ct">
                                            <tpl for=".">
                                                <div class="contItem">
                                                    <div id="contTickets" class="contTickets">
                                                        <div class="contUsuario">
                                                            <div class="imgTickets" style="background-image: url({Image})"></div>
                                                            <div class="contDetailsNombreUsuario">
                                                                <p class="nombreUsuario">{Nombre}</p>
                                                                <p class="correoUsuario">{Correo}</p>
                                                                <div class="lineUnderUser"></div>
                                                            </div>
                                                        </div>
                                                        <div class="contDescription">
                                                            <div id="chkTickets" class="chkTickets" onclick="marcarTickets(this)"></div>
                                                            <div class="cntDescType">
                                                                <div class="contInfoDescription">
                                                                    <p class="fechaTickets lblGreenFormDQ">{Fecha}</p>
                                                                    <p class="tituloTickets">{Titulo}</p>
                                                                    <p class="descripcionTickets fGrey">{Descripcion}</p>
                                                                </div>
                                                                <div class="cntTipoTickects">
                                                                    <p class="tipoTickets">{Tipo}</p>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="contDetails">
                                                            <p class="itemHeader">Code</p>
                                                            <p class="itemData fGrey underlineItem">{Code}</p>
                                                            <p class="itemHeader">Site</p>
                                                            <p class="itemData fGrey underlineItem">{Site}</p>
                                                            <p class="itemHeader">Object</p>
                                                            <p class="itemData fGrey">{Object}</p>
                                                            <p class="itemHeader">Status</p>
                                                            <p class="itemData fGrey">{Status}</p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </tpl>
                                        </div>
                                    </Html>
                                </Tpl>
                            </ext:DataView>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
