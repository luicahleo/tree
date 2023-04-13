<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrdersInventory.aspx.cs" Inherits="TreeCore.ModWorkOrders.pages.WorkOrdersInventory" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <!--<script type="text/javascript" src="../../JS/common.js"></script>-->
    <script type="text/javascript" src="js/WorkOrdersInventory.js"></script>
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

            <ext:TreeStore ID="TreeStore1" runat="server">
                <Root>
                    <ext:Node Expanded="true">
                        <Children>
                            <ext:Node Text="app">
                                <Children>
                                    <ext:Node Text="Application.js" Leaf="true" />
                                </Children>
                            </ext:Node>
                            <ext:Node Text="button" Expanded="true">
                                <Children>
                                    <ext:Node Text="Button.js" Leaf="true" />
                                    <ext:Node Text="Cycle.js" Leaf="true" />
                                    <ext:Node Text="Split.js" Leaf="true" />
                                </Children>
                            </ext:Node>
                            <ext:Node Text="container">
                                <Children>
                                    <ext:Node Text="ButtonGroup.js" Leaf="true" />
                                    <ext:Node Text="Container.js" Leaf="true" />
                                    <ext:Node Text="Viewport.js" Leaf="true" />
                                </Children>
                            </ext:Node>
                            <ext:Node Text="core">
                                <Children>
                                    <ext:Node Text="dom">
                                        <Children>
                                            <ext:Node Text="Element.form.js" Leaf="true" />
                                            <ext:Node Text="Element.static-more.js" Leaf="true" />
                                        </Children>
                                    </ext:Node>
                                </Children>
                            </ext:Node>
                            <ext:Node Text="dd">
                                <Children>
                                    <ext:Node Text="DD.js" Leaf="true" />
                                    <ext:Node Text="DDProxy.js" Leaf="true" />
                                    <ext:Node Text="DDTarget.js" Leaf="true" />
                                    <ext:Node Text="DragDrop.js" Leaf="true" />
                                    <ext:Node Text="DragDropManager.js" Leaf="true" />
                                    <ext:Node Text="DragSource.js" Leaf="true" />
                                    <ext:Node Text="DragTracker.js" Leaf="true" />
                                    <ext:Node Text="DragZone.js" Leaf="true" />
                                    <ext:Node Text="DragTarget.js" Leaf="true" />
                                    <ext:Node Text="DragZone.js" Leaf="true" />
                                    <ext:Node Text="Registry.js" Leaf="true" />
                                    <ext:Node Text="ScrollManager.js" Leaf="true" />
                                    <ext:Node Text="StatusProxy.js" Leaf="true" />
                                </Children>
                            </ext:Node>
                            <ext:Node Text="core">
                                <Children>
                                    <ext:Node Text="Element.alignment.js" Leaf="true" />
                                    <ext:Node Text="Element.anim.js" Leaf="true" />
                                    <ext:Node Text="Element.dd.js" Leaf="true" />
                                    <ext:Node Text="Element.fx.js" Leaf="true" />
                                    <ext:Node Text="Element.js" Leaf="true" />
                                    <ext:Node Text="Element.position.js" Leaf="true" />
                                    <ext:Node Text="Element.scroll.js" Leaf="true" />
                                    <ext:Node Text="Element.style.js" Leaf="true" />
                                    <ext:Node Text="Element.traversal.js" Leaf="true" />
                                    <ext:Node Text="Helper.js" Leaf="true" />
                                    <ext:Node Text="Query.js" Leaf="true" />
                                </Children>
                            </ext:Node>
                            <ext:Node Text="Action.js" Leaf="true" />
                            <ext:Node Text="Component.js" Leaf="true" />
                            <ext:Node Text="Editor.js" Leaf="true" />
                            <ext:Node Text="Img.js" Leaf="true" />
                            <ext:Node Text="Layer.js" Leaf="true" />
                            <ext:Node Text="LoadMask.js" Leaf="true" />
                            <ext:Node Text="ProgressBar.js" Leaf="true" />
                            <ext:Node Text="Shadow.js" Leaf="true" />
                            <ext:Node Text="ShadowPool.js" Leaf="true" />
                            <ext:Node Text="ZIndexManager.js" Leaf="true" />
                        </Children>
                    </ext:Node>
                </Root>
            </ext:TreeStore>

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
                                            <ext:TreePanel ID="gridWOInventory"
                                                runat="server"
                                                Header="true"
                                                Flex="12"
                                                EnableColumnHide="false"
                                                SelectionMemory="false"
                                                StoreID="TreeStore1"
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
                                                                ID="btnAnadir"
                                                                Cls="btnAnadir"
                                                                AriaLabel="Añadir"
                                                                Handler="AgregarEditar();"
                                                                ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>" />
                                                            <ext:Button runat="server"
                                                                ID="btnEditar"
                                                                Cls="btnEditar"
                                                                AriaLabel="Editar"
                                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                Disabled="true" />
                                                            <ext:Button runat="server"
                                                                ID="btnEliminar"
                                                                Cls="btnEliminar"
                                                                AriaLabel="Eliminar"
                                                                ToolTip="<%$ Resources:Comun,  btnEliminar.ToolTip %>"
                                                                Disabled="true" />
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="toolBarFiltro"
                                                        Cls="tlbGrid">
                                                        <Items>
                                                            <ext:TextField
                                                                ID="txtSearch"
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
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbRuta"
                                                        Cls=""
                                                        Dock="Top">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                Cls="noBorder btnBack"
                                                                IconCls="ico-prev"
                                                                ID="btnBack">
                                                                <Listeners>
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                Text="<%$ Resources:Comun, strComun %>"
                                                                Cls="tbNavPath btnBack"
                                                                ID="lbRutaEmplazamientoTipo">
                                                                <Listeners>
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                Cls="noBorder btnNextRuta"
                                                                IconCls="ico-next"
                                                                Hidden="true"
                                                                ID="btnRaizCarpeta">
                                                            </ext:Button>
                                                            <ext:Button runat="server" Hidden="true" ID="btnMenuRuta" Cls="noBorder btnMenuRuta btnBack" IconCls="ico-nav-folder-gr-16">
                                                                <Menu>
                                                                    <ext:Menu runat="server" ID="menuRuta" Cls="menuRuta">
                                                                        <Items>
                                                                        </Items>
                                                                        <Listeners>
                                                                        </Listeners>
                                                                    </ext:Menu>
                                                                </Menu>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                Cls="noBorder btnNextRuta"
                                                                IconCls="ico-next"
                                                                Hidden="true"
                                                                ID="btnCarpetaActual">
                                                            </ext:Button>
                                                            <ext:Label runat="server"
                                                                ID="lbRutaCategoria"
                                                                Cls="rutaCategoria btnBack"
                                                                Height="32"
                                                                Hidden="true">
                                                            </ext:Label>
                                                            <ext:Button runat="server" Hidden="true" ID="btnPadreCatActucal" Cls="noBorder btnMenuRuta btnBack" IconCls="ico-link-vertical">
                                                                <Menu>
                                                                    <ext:Menu runat="server" ID="menuPadreCatActual" Cls="menuRuta">
                                                                        <Items>
                                                                        </Items>
                                                                        <Listeners>
                                                                        </Listeners>
                                                                    </ext:Menu>
                                                                </Menu>
                                                            </ext:Button>
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Plugins>
                                                    <ext:GridFilters runat="server"
                                                        ID="gridFilters"
                                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                        meta:resourceKey="GridFilters">
                                                    </ext:GridFilters>
                                                    <ext:CellEditing runat="server"
                                                        ClicksToEdit="2" />
                                                </Plugins>
                                            </ext:TreePanel>

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
                                                                Text="RECURSO SELECCIONADO" />
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Items>
                                                    <ext:GridPanel runat="server"
                                                        ID="gridWOInventoryDetails"
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
                                                                        ID="btnAnadirDetails"
                                                                        Cls="btnAnadir"
                                                                        AriaLabel="Añadir"
                                                                        Handler="AgregarEditar();"
                                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnEditarDetails"
                                                                        Cls="btnEditar"
                                                                        AriaLabel="Editar"
                                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                        Disabled="true" />
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
                                                                        ID="btnConvertirCorrectivo"
                                                                        ToolTip=""
                                                                        meta:resourceKey="btnConvertirCorrectivo"
                                                                        Cls="btnConvertirCorrectivo"
                                                                        Handler="" />
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
                                                                        ID="btnImport"
                                                                        Cls="btnImport"
                                                                        AriaLabel="Import"
                                                                        ToolTip=""
                                                                        Disabled="false"
                                                                        Handler="" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnExport"
                                                                        Cls="btnExport"
                                                                        AriaLabel="Export"
                                                                        ToolTip=""
                                                                        Disabled="false"
                                                                        Handler="" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnInfo"
                                                                        Cls="btnInfo"
                                                                        AriaLabel="Info"
                                                                        ToolTip=""
                                                                        Disabled="false"
                                                                        Handler="" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnFiltrosDetalle"
                                                                        ToolTip="Mostrar Filtros"
                                                                        meta:resourceKey="btnFiltros"
                                                                        Cls="btnFiltros"
                                                                        Handler="parent.MostrarPnFiltros();" />
                                                                    <ext:ToolbarFill />
                                                                    <ext:Button runat="server"
                                                                        Text="Only work orders"
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
                                                                            <ext:ModelField Name="Name" />
                                                                            <ext:ModelField Name="Code" />
                                                                            <ext:ModelField Name="Model" />
                                                                            <ext:ModelField Name="NextDate" />
                                                                            <ext:ModelField Name="Ticket" />
                                                                            <ext:ModelField Name="Project" />
                                                                            <ext:ModelField Name="Status" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:Store>
                                                        </Store>
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:Column runat="server"
                                                                    Text="Name"
                                                                    MinWidth="120"
                                                                    DataIndex="Name"
                                                                    Flex="2"
                                                                    ID="ColName" />
                                                                <ext:Column runat="server"
                                                                    Text="Code"
                                                                    MinWidth="120"
                                                                    DataIndex="Code"
                                                                    Flex="1"
                                                                    ID="ColCode" />
                                                                <ext:Column runat="server"
                                                                    Text="Model"
                                                                    MinWidth="120"
                                                                    DataIndex="Model"
                                                                    Flex="1"
                                                                    ID="ColModel" />
                                                                <ext:Column runat="server"
                                                                    MinWidth="120"
                                                                    Text="WO Next Date"
                                                                    DataIndex="NextDate"
                                                                    Flex="1"
                                                                    ID="ColNextDate" />
                                                                <ext:Column runat="server"
                                                                    MinWidth="120"
                                                                    Text="Tickets"
                                                                    DataIndex="Ticket"
                                                                    Flex="1"
                                                                    ID="ColTickets" />
                                                                <ext:Column runat="server"
                                                                    MinWidth="120"
                                                                    Text="Project"
                                                                    DataIndex="Project"
                                                                    Flex="1"
                                                                    ID="ColProject" />
                                                                <ext:Column runat="server"
                                                                    MinWidth="120"
                                                                    Text="Status"
                                                                    DataIndex="Status"
                                                                    Flex="1"
                                                                    ID="ColStatus">
                                                                    <Renderer Fn="StatusRender"></Renderer>
                                                                </ext:Column>
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
