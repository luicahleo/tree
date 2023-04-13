<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalidadDQSemaforos.aspx.cs" Inherits="TreeCore.ModCalidad.CalidadDQSemaforos" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body class="bodyFMenu">
    <!--<link data-require="jqueryui@*" data-semver="1.10.0" rel="stylesheet" href="../../Scripts/Semaforo/jquery-ui-1.10.0.custom.min.css" />
    <link data-require="bootstrap@*" data-semver="3.2.0" rel="stylesheet" href="../../Scripts/Semaforo/bootstrap.css" />
    <script data-require="bootstrap@*" data-semver="3.2.0" src="../../Scripts/Semaforo/bootstrap.js"></script>
    <script data-require="jqueryui@*" data-semver="1.10.0" src="../../Scripts/Semaforo/jquery-ui.js"></script>-->

    <link href="/Scripts/Semaforo/Style.css" rel="stylesheet" type="text/css" />
    <!--<script src="/Scripts/Semaforo/slider.js"></script>-->
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>

            <%-- INICIO WINDOWS  --%>

            <ext:Window ID="winSaveSemaforo"
                runat="server"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Height="310"
                Width="460"
                BodyCls=""
                Scrollable="Vertical"
                Hidden="true">
                <Listeners>
                    <%--<AfterRender Handler="slider();" />--%>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>

                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="Toolbar6"
                        Cls=" greytb"
                        Dock="Bottom"
                        Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnCancelar"
                                Cls="btn-secondary "
                                MinWidth="110"
                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="#{winSaveSemaforo}.hide();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnGuardar"
                                Cls="btn-ppal "
                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="winGestionBotonGuardar();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel
                        runat="server"
                        ID="formGestn"
                        BodyCls="tbGrey"
                        Height="192"
                        Border="false">
                        <Items>
                            <ext:TextField runat="server"
                                ID="txtName"
                                EmptyText=""
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                Flex="1"
                                MarginSpec="10 30 8 30"
                                WidthSpec="87%"
                                MaxHeight="60">
                            </ext:TextField>

                            <ext:Panel runat="server"
                                PaddingSpec="0 30 10 30"
                                Cls="tbGrey overflowSlider"
                                BodyCls="tbGrey border1px"
                                BodyPaddingSummary="0 35"
                                Height="125"
                                ID="contCurrently"
                                Flex="1">
                                <DockedItems>
                                    <ext:Label runat="server"
                                        Cls="spanLbl"
                                        Text="<%$ Resources:Comun, strActual %>">
                                    </ext:Label>
                                </DockedItems>
                                <Content>
                                    <div id="slider-range" class="sliderBar"></div>

                                    <div id="wrapcurrently" class="wrapcurrently tbGrey">
                                        <ext:TextField runat="server"
                                            ID="txtInicio"
                                            Width="60"
                                            Disabled="true" />
                                        <ext:Label runat="server"
                                            ID="lblVerde"
                                            Cls="lblGreenFormDQ"
                                            Text="<%$ Resources:Comun, strVerde %>">
                                        </ext:Label>
                                        <ext:TextField runat="server"
                                            ID="txtMedio"
                                            Editable="false"
                                            Width="60" />
                                        <ext:Label runat="server"
                                            ID="lblAmarillo"
                                            Cls="lblYellowFormDQ"
                                            Text="<%$ Resources:Comun, strAmarillo %>">
                                        </ext:Label>

                                        <ext:TextField runat="server"
                                            ID="txtMedio2"
                                            Editable="false"
                                            Width="60" />
                                        <ext:Label runat="server"
                                            ID="lblRed"
                                            Cls="lblRedFormDQ"
                                            Text="<%$ Resources:Comun, strRojo %>">
                                        </ext:Label>

                                        <ext:TextField runat="server"
                                            ID="txtFin"
                                            Text="100%"
                                            Width="60"
                                            Disabled="true" />
                                    </div>
                                </Content>
                            </ext:Panel>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoGestion(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <%-- FIN WINDOWS  --%>

            <ext:Viewport runat="server"
                ID="MainVwP"
                OverflowY="auto"
                Layout="FitLayout">
                <Content>
                    <ext:Button runat="server"
                        ID="btnCollapseAsRClosed"
                        Cls="btn-trans btnCollapseAsRClosedv3"
                        Handler="hideAsideR();"
                        Disabled="false"
                        Hidden="true">
                    </ext:Button>
                </Content>
                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <Items>
                            <%-------------TENER EN CUENTA QUE AL CENTER MAIN SE LE PUEDEN ACOPLAR SEGMENTOS PLEGABLES CON LAS "REGIONS"-------------%>

                            <%-- PANEL CENTRAL--%>

                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                MarginSpec="0 20 20 20"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                BodyCls="tbGrey"
                                Cls="visorInsidePn pnCentralMain">
                                <DockedItems>
                                    <ext:Container runat="server"
                                        ID="WrapAlturaCabecera"
                                        MinHeight="60"
                                        Dock="Top"
                                        Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:Toolbar runat="server"
                                                ID="tbTitulo"
                                                Dock="Top"
                                                Cls="tbGrey tbTitleMiddleBot tbNoborder"
                                                Hidden="false"
                                                Layout="ColumnLayout" Flex="1">
                                                <Items>
                                                    <ext:Label runat="server"
                                                        ID="lbltituloPrincipal"
                                                        Cls="TituloCabecera"
                                                        Text="<%$ Resources:Comun, strSemaforo %>"
                                                        Height="25" />
                                                </Items>
                                            </ext:Toolbar>
                                            <%-- TABS NAVEGACION--%>
                                            <ext:Toolbar runat="server"
                                                ID="tbNavNAside"
                                                Dock="Top"
                                                Cls="tbGrey tbNoborder"
                                                Hidden="true"
                                                PaddingSpec="10 10 10 10"
                                                OverflowHandler="Scroller"
                                                Flex="1">
                                                <Items>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="lnkPerfiles"
                                                        Hidden="false"
                                                        Cls="lnk-navView lnk-noLine navActivo"
                                                        Text="ActiveTab">
                                                        <Listeners>
                                                            <Click Handler="NavegacionTabs(this)"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="HyperlinkButton1"
                                                        Hidden="false"
                                                        Cls="lnk-navView lnk-noLine "
                                                        Text="ActiveTab">
                                                        <Listeners>
                                                            <Click Handler="NavegacionTabs(this)"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server"
                                                ID="tbFiltrosYSliders"
                                                Dock="Top"
                                                Cls="tbGrey tbNoborder "
                                                Hidden="true"
                                                Layout="HBoxLayout"
                                                Flex="1">
                                                <Items>
                                                    <ext:Toolbar runat="server"
                                                        ID="tbFiltersLabels"
                                                        Dock="Top"
                                                        Cls="tbGrey  
                                                        tbNoborder tbFiltrosLabels"
                                                        Hidden="false"
                                                        OverflowHandler="Scroller"
                                                        Flex="1"
                                                        Padding="0"
                                                        MarginSpec="0 0 0 -9">
                                                        <Items>
                                                            <ext:Container
                                                                Cls="tagContainerMinW"
                                                                Height="24"
                                                                Layout="HBoxLayout"
                                                                runat="server"
                                                                ID="tagContainersTemp"
                                                                Hidden="false">
                                                                <Items>
                                                                    <ext:Label
                                                                        runat="server"
                                                                        Cls="TagTempGreen"
                                                                        IconCls="ico-filters-16px"
                                                                        Text="Temporal Filter">
                                                                    </ext:Label>
                                                                    <ext:Button
                                                                        runat="server"
                                                                        ID="Button6"
                                                                        Cls="CloseTempGreen"
                                                                        OverCls="NoOver"
                                                                        PressedCls="None"
                                                                        FocusCls="none"
                                                                        Handler="">
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Container>
                                                            <ext:Container
                                                                Cls="tagContainerMinW"
                                                                Height="24"
                                                                Layout="HBoxLayout"
                                                                runat="server"
                                                                ID="Container1"
                                                                Hidden="false">
                                                                <Items>
                                                                    <ext:Label
                                                                        runat="server"
                                                                        Cls="TagTempGreen"
                                                                        IconCls="ico-filters-16px"
                                                                        Text="Temporal Filter">
                                                                    </ext:Label>
                                                                    <ext:Button
                                                                        runat="server"
                                                                        ID="Button2"
                                                                        Cls="CloseTempGreen"
                                                                        OverCls="NoOver"
                                                                        PressedCls="None"
                                                                        FocusCls="none"
                                                                        Handler="">
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Container>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Container>
                                </DockedItems>
                                <Items>
                                    <ext:GridPanel
                                        Flex="1"
                                        MarginSpec="0 10 0 10"
                                        HideHeaders="true"
                                        Region="Center"
                                        SelectionMemory="false"
                                        Hidden="false"
                                        Title="<%$ Resources:Comun, strSemaforo %>"
                                        MaxWidth="440"
                                        runat="server"
                                        ID="gridSemaforos"
                                        Cls="gridPanel   "
                                        OverflowX="Hidden"
                                        OverflowY="Auto">
                                        <DockedItems>
                                            <ext:Toolbar runat="server"
                                                ID="Toolbar5"
                                                Dock="Top"
                                                Cls="tlbGrid"
                                                OverflowHandler="Scroller">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnAnadir"
                                                        Cls="btnAnadir"
                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                        Handler="AgregarEditar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnEditar"
                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                        Cls="btnEditar"
                                                        Handler="MostrarEditar();" />
                                                    <ext:Button runat="server"
                                                        ID="btnEliminar"
                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                        Cls="btnEliminar"
                                                        Handler="Eliminar();" />
                                                    <ext:Button runat="server"
                                                        ID="btnRefrescar"
                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                        Cls="btnRefrescar"
                                                        Handler="Refrescar();" />
                                                    <ext:Button runat="server"
                                                        ID="btnDescargar"
                                                        Cls="btnDescargar"
                                                        Hidden="false"
                                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                        Handler="ExportarDatos('CalidadDQSemaforos', hdCliID.value, #{gridSemaforos}, App.btnActivo.pressed, '');">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnActivar"
                                                        Cls="btnActivar"
                                                        Hidden="false"
                                                        Handler="Activar();"
                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>">
                                                    </ext:Button>


                                                    <ext:Label runat="server"
                                                        ID="lblToggle"
                                                        Cls="lblBtnActivo"
                                                        PaddingSpec="0 0 25 0"
                                                        Text="<%$ Resources:Comun, strActivo %>" />
                                                    <ext:Button runat="server"
                                                        ID="btnActivo"
                                                        Width="41"
                                                        Pressed="true"
                                                        EnableToggle="true"
                                                        Cls="btn-toggleGrid"
                                                        AriaLabel="<%$ Resources:Comun, strEmplazamientosCliente %>"
                                                        ToolTip="<%$ Resources:Comun, strActivo %>"
                                                        Handler="Refrescar();" />
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Container runat="server"
                                                ID="tbfiltros"
                                                Cls=""
                                                Dock="Top">
                                                <Content>
                                                    <local:toolbarFiltros
                                                        ID="cmpFiltro"
                                                        runat="server"
                                                        Stores="storePrincipal"
                                                        MostrarComboFecha="false"
                                                        Grid="gridSemaforos"
                                                        MostrarBusqueda="false"
                                                        QuitarFiltros="true" />
                                                </Content>
                                            </ext:Container>
                                        </DockedItems>
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storePrincipal"
                                                RemotePaging="false"
                                                AutoLoad="true"
                                                OnReadData="storePrincipal_Refresh"
                                                RemoteSort="true"
                                                shearchBox="cmpFiltro_txtSearch"
                                                listNotPredictive="DQSemaforoID,Activo"
                                                PageSize="20">
                                                <Listeners>
                                                    <BeforeLoad Fn="DeseleccionarGrilla" />
                                                </Listeners>
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server"
                                                        IDProperty="DQSemaforoID">
                                                        <Fields>
                                                            <ext:ModelField Name="DQSemaforoID" Type="Int" />
                                                            <ext:ModelField Name="DQSemaforo" Type="String" />
                                                            <ext:ModelField Name="Activo" Type="Boolean" />
                                                            <ext:ModelField Name="IntervaloVerde" Type="Int" />
                                                            <ext:ModelField Name="IntervaloRojo" Type="Int" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Listeners>
                                                    <DataChanged Fn="BuscadorPredictivo" />
                                                </Listeners>
                                                <Sorters>
                                                    <ext:DataSorter Property="DQSemaforo" Direction="DESC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    MinWidth="120"
                                                    DataIndex="DQSemaforo"
                                                    Flex="4"
                                                    Hidden="false"
                                                    ID="colName">
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colActivo"
                                                    Flex="1"
                                                    DataIndex="Activo"
                                                    Align="Center"
                                                    Hidden="false"
                                                    Text="<%$ Resources:Comun, strActivo %>"
                                                    Cls="col-activo"
                                                    MinWidth="60">
                                                    <Renderer Fn="InactivoRender" />
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="GridRowSelect"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelect" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Plugins>
                                            <ext:GridFilters runat="server"
                                                ID="gridFilters"
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                        </Plugins>
                                        <BottomBar>
                                            <ext:PagingToolbar runat="server"
                                                Cls="PgToolBMainGrid"
                                                ID="PagingToolbar2"
                                                MaintainFlex="true"
                                                DisplayInfo="false"
                                                Flex="8"
                                                HideRefresh="true"
                                                OverflowHandler="Scroller">
                                                <Items>
                                                    <ext:ComboBox runat="server"
                                                        Cls="comboGrid"
                                                        ID="ComboBox9"
                                                        MaxWidth="65">
                                                        <Items>
                                                            <ext:ListItem Text="10" />
                                                            <ext:ListItem Text="20" />
                                                            <ext:ListItem Text="30" />
                                                            <ext:ListItem Text="40" />
                                                        </Items>
                                                        <SelectedItems>
                                                            <ext:ListItem Value="20" />
                                                        </SelectedItems>
                                                        <Listeners>
                                                            <Select Fn="handlePageSizeSelect" />
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
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
