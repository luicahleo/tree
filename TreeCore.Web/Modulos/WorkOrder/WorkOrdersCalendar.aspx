<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrdersCalendar.aspx.cs" Inherits="TreeCore.ModWorkOrders.WorkOrdersCalendar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <!--<script type="text/javascript" src="../../JS/common.js"></script>-->
    <script type="text/javascript" src="js/WorkOrdersCalendar.js"></script>
    <link href="/CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="css/styleWorkOrders.css" rel="stylesheet" type="text/css" />
    <link href='/Scripts/fullcalendar/main.css' rel='stylesheet' />
    <script src='/Scripts/fullcalendar/main.js'></script>
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                </Listeners>
            </ext:ResourceManager>

            <ext:Viewport ID="vwResp"
                runat="server"
                Layout="FitLayout"
                Flex="1"
                OverflowY="auto">
                <Items>
                    <ext:Panel runat="server"
                        ID="pnCol1"
                        Cls="grdNoHeader pnWOCalendar"
                        Region="Center"
                        Flex="1"
                        MinWidth="200"
                        BodyPaddingSummary="0 16 0 0"
                        AnchorVertical="100%"
                        AnchorHorizontal="100%"
                        OverflowX="Auto"
                        OverflowY="Auto">
                        <DockedItems>
                            <ext:Toolbar runat="server" ID="tlbBaseDetalle" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                                <Items>
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
                                        ID="btnCalendar"
                                        ToolTip="Calendar"
                                        meta:resourceKey="btnCalendar"
                                        Cls="btnCalendar-grey"
                                        Handler="parent.NavegacionTabs" />
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
                                        ID="btnFiltros"
                                        ToolTip="Mostrar Filtros"
                                        meta:resourceKey="btnFiltros"
                                        Cls="btnFiltros"
                                        Handler="parent.MostrarPnFiltros();" />
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server" ID="Toolbar1" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                                <Items>
                                    <ext:Label runat="server"
                                        ID="lbMonth"
                                        Width="150"
                                        Cls="lbCalendar"
                                        Text="MONTH 2021">
                                        <Listeners>
                                            <AfterRender Handler="labelCalendar();" Buffer="150" />
                                        </Listeners>
                                    </ext:Label>
                                    <ext:Button runat="server"
                                        ID="Button4"
                                        ToolTip="BEFORE MONTH"
                                        Cls="btnPrev"
                                        FocusCls="none"
                                        PressedCls="none"
                                        Handler="changePrevOrNext('Prev');" />
                                    <ext:Button runat="server"
                                        ID="Button5"
                                        ToolTip="NEXT MONTH"
                                        Cls="btnNext"
                                        FocusCls="none"
                                        PressedCls="none"
                                        Handler="changePrevOrNext('Next');" />
                                    <ext:Button runat="server"
                                        ID="Button6"
                                        ToolTip="TODAY"
                                        Text="TODAY"
                                        Cls="btnToday"
                                        FocusCls="none"
                                        PressedCls="none"
                                        Handler="viewToday();" />
                                    <ext:ToolbarFill />
                                    <ext:ComboBox meta:resourceKey="cmbMonth"
                                        runat="server"
                                        ID="cmbMonth"
                                        Cls="comboGrid"
                                        EmptyText="Select a view"
                                        Flex="2"
                                        MinWidth="120"
                                        MaxWidth="250"
                                        Hidden="false">
                                        <Items>
                                            <ext:ListItem Text="Day" Value="day" />
                                            <ext:ListItem Text="Week" Value="week" />
                                            <ext:ListItem Text="Month" Value="month" />
                                            <ext:ListItem Text="List" Value="list" />
                                        </Items>
                                        <Listeners>
                                            <Select Fn="selectView"></Select>
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Toolbar>
                            <ext:Panel runat="server" ID="pnCategoryCalendar" Dock="Left" Width="200" BodyPadding="24">
                                <Content>
                                    <ext:Checkbox runat="server" ID="chkCategory1" Checked="true" BoxLabel="Calendar 1" AllowBlank="true" Cls="chkLabel chkCalendar">
                                        <Listeners>
                                            <Change Handler="selectCategory(this, 1)" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <div class="lineUnderCHK"></div>
                                    <ext:Checkbox runat="server" ID="chkCategory2" Checked="true" BoxLabel="Calendar 2" AllowBlank="true" Cls="chkLabel chkCalendar">
                                        <Listeners>
                                            <Change Handler="selectCategory(this, 2)" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <div class="lineUnderCHK"></div>
                                    <ext:Checkbox runat="server" ID="chkCategory3" Checked="true" BoxLabel="Calendar 3" AllowBlank="true" Cls="chkLabel chkCalendar">
                                        <Listeners>
                                            <Change Handler="selectCategory(this, 3)" />
                                        </Listeners>
                                    </ext:Checkbox>
                                </Content>
                            </ext:Panel>
                        </DockedItems>
                        <Content>
                            <ext:Button runat="server" ID="btnPanelCategory" Cls="btnCollapseAsRClosedv2 btnCollapsedCategory" PressedCls="none" FocusCls="none" Hidden="true" ToolTip="Mostrar panel categorias" Handler="seePanelCategory();" />
                            <div id="fullCalModal" class="fullCalModal" style="display: none;">
                                <div>
                                    <span id="modalTitle" class="modalTitle"></span>
                                    <div id="btnClose" class="btnClose winClosePopup" onclick="closeWinCalendar();"></div>
                                </div>
                                <div><span id="modalID" class="modalID modalTitle"></span></div>
                                <div><span id="modalWorkFlowText" class="modalTitle">Workflow: </span><span id="modalWorkFlow" class="modalTitle"></span></div>
                                <div class="marginBot"><span id="modalStatusText" class="modalTitle">Status: </span><span id="modalStatus" class="modalTitle"></span></div>
                                <div><span id="modalObject" class=""></span></div>
                                <div class="marginBot"><span id="modalService"></span></div>
                                <div><span id="modalEntity" class=""></span></div>
                                <div style="margin-bottom: .5rem;"><span id="modalSLA" class="modalTitle"></span></div>
                                <div class="marginBot pgDivCalendar">
                                    <ext:ProgressBar runat="server" ID="pbCalendarDescription" Width="60" Text="" Cls="pbNoText modalPercentage" />
                                    <span id="modalPercentage" class="modalPercentage">
                                </div>
                                <div class="modalBtnInfo btnInfo-asR"></div>
                                <div class="modalBtnDocument btnDocument-asR"></div>
                                <div class="modalBtnLocation location-asR"></div>
                            </div>

                            <div id='calendar'></div>
                        </Content>
                        <Listeners>
                            <AfterRender Handler="CalendarRender(); resizeCalendarWO();" Buffer="50" />
                        </Listeners>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>
