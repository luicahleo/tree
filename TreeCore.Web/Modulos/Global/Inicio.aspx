<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="TreeCore.ModGlobal.pages.Inicio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <!--<script type="text/javascript" src="/ModGlobal/pages/js/Inicio.js" > </script>-->
</head>
<body>
    <link href="/Modulos/Global/css/styleInicio.min.css" rel="stylesheet" type="text/css" />


    <form id="form1" runat="server">
        <div>
            <%-- INICIO HIDDEN --%>

            <ext:Hidden runat="server" ID="hdDepartamentoID" />
            <ext:Hidden runat="server" ID="hdProyectoID" />
            <ext:Hidden runat="server" ID="hdMediaPorcentaje" />
            <ext:Hidden runat="server" ID="hdCliID" />

            <%-- FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <%--<Listeners>
                    <WindowResize Handler="CtSizer();"></WindowResize>
                </Listeners>--%>
            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>

            <%-- INICIO STORES --%>

            <ext:Store runat="server"
                ID="storeTareas"
                RemotePaging="false"
                PageSize="20"
                AutoLoad="false"
                OnReadData="storeTareas_Refresh"
                RemoteSort="true">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeDepartamentos"
                AutoLoad="false"
                OnReadData="storeDepartamentos_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="DepartamentoID">
                        <Fields>
                            <ext:ModelField Name="DepartamentoID" Type="Int" />
                            <ext:ModelField Name="Departamento" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeProyectos"
                AutoLoad="false"
                OnReadData="storeProyectos_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ProyectoID">
                        <Fields>
                            <ext:ModelField Name="ProyectoID" Type="Int" />
                            <ext:ModelField Name="Proyecto" Type="String" />
                            <ext:ModelField Name="Descripcion" Type="String" />
                            <ext:ModelField Name="FechaInicio" Type="Date" />
                            <ext:ModelField Name="FechaFin" Type="Date" />
                            <ext:ModelField Name="ProyectoEstadoID" Type="Int" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="Referencia" Type="String" />
                            <ext:ModelField Name="Cerrado" Type="Boolean" />
                            <ext:ModelField Name="Horas" Type="Int" />
                            <ext:ModelField Name="ProyectoAgrupacionID" Type="Int" />
                            <ext:ModelField Name="ProyectoBase" Type="String" />
                            <ext:ModelField Name="MonedaID" Type="Int" />
                            <ext:ModelField Name="Multiflujo" Type="Boolean" />
                            <ext:ModelField Name="Propio" Type="Boolean" />
                            <ext:ModelField Name="Sharing" Type="Boolean" />
                            <ext:ModelField Name="Torreros" Type="Boolean" />
                            <ext:ModelField Name="TorrerosCollo" Type="Boolean" />
                            <ext:ModelField Name="Ampliacion" Type="Boolean" />
                            <ext:ModelField Name="ProyectoComercial" Type="String" />
                            <ext:ModelField Name="SharingGestionSolicitante" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeNotificaciones"
                AutoLoad="false"
                OnReadData="storeNotificaciones_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="NotificacionID">
                        <Fields>
                            <ext:ModelField Name="NotificacionID" Type="Int" />
                            <ext:ModelField Name="Notificacion" Type="Int" />
                            <ext:ModelField Name="Contenido" Type="String" />
                            <ext:ModelField Name="NombreCompleto" Type="String" />
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeGestFav"
                AutoLoad="false"
                OnReadData="storeGestFav_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="menuID">
                        <Fields>
                            <ext:ModelField Name="menuID" />
                            <ext:ModelField Name="Pagina" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter
                        Property="Pagina"
                        Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeModulos"
                AutoLoad="false"
                OnReadData="storeModulos_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Modulo">
                        <Fields>
                            <ext:ModelField Name="Modulo" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <%-- FIN STORES --%>

            <%--VENTANA GESTIÓN FAVORITOS--%>
            <ext:Window ID="winGestFav"
                runat="server"
                Title="<%$ Resources:Comun, strGestFav %>"
                Height="550"
                Width="380"
                BodyCls=""
                Cls="winForm-respSimple tbGrey"
                Scrollable="Disabled"
                Hidden="true"
                Padding="12">
                <DockedItems>
                    <ext:Toolbar runat="server" ID="tbGestFav" Cls="tlb-base greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server" ID="btnApplyFav" Cls="btn-ppal " Text="<%$ Resources:Comun, strAplicar %>" Focusable="false" PressedCls="none" Hidden="false" Handler="añadirPaginasFavoritas"></ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <Items>
                    <ext:ComboBox runat="server"
                        ID="cbGestFav"
                        LabelAlign="top"
                        FieldLabel="Module"
                        Height="32"
                        EmptyText="Select Module"
                        StoreID="storeModulos"
                        DisplayField="Modulo"
                        ValueField="Modulo"
                        QueryMode="Local">
                        <Listeners>
                            <Select Fn="cargarPaginasFavoritas" />
                        </Listeners>
                    </ext:ComboBox>
                    <ext:GridPanel
                        ID="gridGestFav"
                        runat="server"
                        Cls="gridPanel"
                        StoreID="storeGestFav"
                        MaxHeight="300"
                        Scrollable="Vertical">
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column
                                    runat="server"
                                    Text="<%$ Resources:Comun, strPagina%>"
                                    Width="160"
                                    DataIndex="Pagina"
                                    Resizable="false"
                                    MenuDisabled="true"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel runat="server" Mode="Multi" />
                        </SelectionModel>
                    </ext:GridPanel>

                </Items>
            </ext:Window>
            <%--FIN VETANA GESTIÓN FAVORITOS--%>

            <%--INICIO  VIEWPORT --%>
            <ext:Viewport runat="server"
                ID="vwInicio"
                OverflowY="Auto"
                Layout="FitLayout">
                <Listeners>
                    <AfterRender Handler="CtSizer();"></AfterRender>
                </Listeners>
                <Items>
                    <%--PANEL LATERAL OCULTO EN INICIO POR AHORA --%>
                    <ext:Button runat="server"
                        Hidden="true"
                        ID="btnCollapseAsR"
                        Cls="btn-trans"
                        Handler="hidePn();">
                    </ext:Button>
                    <ext:Panel runat="server"
                        ID="pnMainCt"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Panel runat="server"
                                ID="pnBlkTop"
                                Header="false"
                                MinHeight="160"
                                Flex="2"
                                Border="false">
                                <Items>
                                    <ext:Panel runat="server"
                                        ID="pnFav"
                                        Header="false"
                                        Border="false">
                                        <Content>
                                            <div id="dvHeadFav">
                                                <ext:Label runat="server"
                                                    ID="lblMyFav"
                                                    meta:resourceKey="lblMyFav"
                                                    IconCls="ico-favorites"
                                                    Text="Mis Favoritos">
                                                </ext:Label>
                                                <ext:Container runat="server"
                                                    Hidden="true"
                                                    ID="ctBtnFav">
                                                    <Items>
                                                        <ext:Button runat="server"
                                                            ID="btnPrev"
                                                            IconCls="ico-prev"
                                                            Handler="moveSlider(this);">
                                                        </ext:Button>
                                                        <ext:Button runat="server"
                                                            ID="btnNext"
                                                            IconCls="ico-next"
                                                            Handler="moveSlider(this);">
                                                        </ext:Button>
                                                    </Items>
                                                </ext:Container>
                                                <ext:Button runat="server"
                                                    ID="btnGestFav"
                                                    Hidden="true"
                                                    ToolTip="<%$ Resources:Comun, strGestFav %>"
                                                    Cls="btn-trans btnConfig"
                                                    Handler="showWinGestFav">
                                                </ext:Button>
                                            </div>
                                            <ul id="ulFav">
                                            </ul>
                                        </Content>
                                    </ext:Panel>
                                    <ext:Panel runat="server"
                                        ID="pnChart"
                                        Header="false"
                                        Border="false"
                                        Height="160">
                                        <Content>                                            
                                        </Content>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                            <ext:GridPanel
                                runat="server"
                                ID="grdTask"
                                Title="Mis Tareas"
                                StoreID="storeTareas"
                                meta:resourceKey="grdTask"
                                SelectionMemory="false"
                                EnableColumnHide="false"
                                Flex="6"
                                Cls="gridPanel">
                                <DockedItems>
                                    <ext:Toolbar
                                        runat="server"
                                        ID="tlbBase"
                                        Dock="Top"
                                        Cls="tlbGrid">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnRefrescar"
                                                Cls="btnRefrescar"
                                                AriaLabel="Refrescar"
                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                Handler="Refrescar();" />
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar runat="server"
                                        ID="tlbClientes"
                                        Dock="Top">
                                        <Items>
                                            <ext:ComboBox runat="server"
                                                ID="cmbProyectos"
                                                StoreID="storeProyectos"
                                                QueryMode="Local"
                                                LabelAlign="Right"
                                                Width="200"
                                                Cls="comboGrid pos-boxGrid"
                                                ValueField="ProyectoID"
                                                DisplayField="Proyecto"
                                                EmptyText="<%$ Resources:Comun, strProyecto %>">
                                                <Listeners>
                                                    <Select Fn="ProyectosSeleccionar" />
                                                    <TriggerClick Fn="RecargarProyectos" />
                                                </Listeners>
                                                <Triggers>
                                                    <ext:FieldTrigger
                                                        IconCls="ico-reload"
                                                        Hidden="true"
                                                        Weight="-1"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                </Triggers>
                                            </ext:ComboBox>
                                            <ext:ComboBox runat="server"
                                                ID="cmbDepartamentos"
                                                StoreID="storeDepartamentos"
                                                QueryMode="Local"
                                                Width="200"
                                                Cls="comboGrid pos-boxGrid"
                                                ValueField="DepartamentoID"
                                                DisplayField="Departamento"
                                                EmptyText="<%$ Resources:Comun, strDepartamento %>">
                                                <Listeners>
                                                    <Select Fn="DepartamentosSeleccionar" />
                                                    <TriggerClick Fn="RecargarDepartamentos" />
                                                </Listeners>
                                                <Triggers>
                                                    <ext:FieldTrigger
                                                        IconCls="ico-reload"
                                                        Hidden="true"
                                                        Weight="-1"
                                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                </Triggers>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <ColumnModel runat="server">
                                    <Columns>
                                        <ext:Column
                                            runat="server"
                                            ID="colName"
                                            DataIndex="NombreSitio"
                                            Text="<%$ Resources:Comun, strNombre %>"
                                            Flex="1">
                                        </ext:Column>
                                        <ext:Column
                                            runat="server"
                                            ID="lnkCode"
                                            DataIndex="Codigo"
                                            Flex="1"
                                            Text="<%$ Resources:Comun, strCodigo %>">
                                            <Renderer Fn="linkRendererSites" />
                                        </ext:Column>
                                        <ext:Column
                                            runat="server"
                                            ID="colState"
                                            DataIndex="LegalEmplazamientoEstado_esES"
                                            Text="<%$ Resources:Comun, strEstado %>"
                                            Flex="1" />
                                        <ext:Column
                                            runat="server"
                                            ID="colRegion"
                                            DataIndex="Region"
                                            Flex="1"
                                            Text="<%$ Resources:Comun, strRegion %>">
                                            <%--     <Renderer Fn="linkRendererMaps" />--%>
                                        </ext:Column>
                                        <ext:Column
                                            runat="server"
                                            ID="colProvincia"
                                            DataIndex="Provincia"
                                            Flex="1"
                                            Text="<%$ Resources:Comun, strProvincia %>" />
                                        <ext:Column
                                            runat="server"
                                            ID="colPoblacion"
                                            DataIndex="Municipio"
                                            Flex="1"
                                            Text="<%$ Resources:Comun, strMunicipio %>" />
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
                                        meta:resourceKey="gridFilters" />
                                    <ext:CellEditing runat="server"
                                        ClicksToEdit="2" />
                                </Plugins>
                                <BottomBar>
                                    <ext:PagingToolbar runat="server"
                                        ID="PagingToolBar"
                                        StoreID="storeTareas"
                                        meta:resourceKey="PagingToolBar"
                                        DisplayInfo="true"
                                        HideRefresh="true">
                                        <Items>
                                            <ext:ComboBox runat="server"
                                                Cls="comboGrid"
                                                Width="80">
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
                                <ViewConfig StripeRows="true" EmptyText="<%$ Resources:Comun, strSinDatosMostrar %>">
                                    <Listeners>
                                        <ItemContextMenu Handler="e.stopEvent(); #{ContextMenu}.showAt(e.getXY()); return false;" />
                                    </Listeners>
                                </ViewConfig>
                            </ext:GridPanel>
                        </Items>
                    </ext:Panel>
                    <ext:Panel runat="server"
                        ID="pnAsideR"
                        Header="false"
                        Border="false"
                        Width="360"
                        Hidden="true">
                        <Items>
                            <ext:Label runat="server"
                                ID="lblAsideNameR"
                                IconCls="ico-head-info"
                                Cls="lblHeadAside"
                                meta:resourceKey="lblAsideNameR"
                                Text="NOTES & ALERTS">
                            </ext:Label>
                            <ext:Panel runat="server"
                                ID="ctAsideR"
                                Border="false"
                                Header="false"
                                Cls="ctAsideR">
                                <Items>
                                    <%--LEFT TABS MENU--%>
                                    <ext:Panel runat="server"
                                        ID="mnAsideR"
                                        Border="false"
                                        Header="false">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnAlertas"
                                                Cls="btnAlertas-asR"
                                                ToolTip="<%$ Resources:Comun, strAlertas %>"
                                                FocusCls="none"
                                                Handler="RpanelBTNNotes();">
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnNotas"
                                                Cls="btnNotas-asR"
                                                ToolTip="<%$ Resources:Comun, strNotas %>"
                                                FocusCls="none"
                                                Handler="RpanelBTNAlert();">
                                            </ext:Button>
                                        </Items>
                                    </ext:Panel>
                                    <%--PANELS--%>
                                    <ext:Panel runat="server"
                                        ID="pnGridsAsideR"
                                        Border="false"
                                        StoreID="storeNotificaciones"
                                        Header="false"
                                        Title="etiqgridTitle"
                                        Hidden="false"
                                        Scrollable="Vertical"
                                        OverflowY="Scroll">
                                        <Items>
                                            <%--NOTIFICATIONS PANEL--%>
                                            <ext:Panel runat="server"
                                                ID="pnNotificationsFull"
                                                Hidden="true">
                                                <Items>
                                                    <ext:Label runat="server"
                                                        ID="lblGrid"
                                                        IconCls="ico-head-Note"
                                                        Cls="lblHeadAside"
                                                        Text="<%$ Resources:Comun, strNotas %>">
                                                    </ext:Label>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnNotifications"
                                                        Header="false"
                                                        Hidden="false"
                                                        Disabled="false">
                                                        <Items>
                                                            <ext:Toolbar runat="server"
                                                                ID="tlbNotifications">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        ID="addNotific"
                                                                        IconCls="ico-addBtn"
                                                                        Cls="btn-mini-ppal btnAdd"
                                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                                        Handler="RpanelBTNAdd();">
                                                                    </ext:Button>
                                                                    <ext:Container runat="server"
                                                                        ID="ctnLnkNotif">
                                                                        <Items>
                                                                            <ext:HyperlinkButton runat="server"
                                                                                ID="lnkNoChecked"
                                                                                Text="Delete Checked"
                                                                                Cls="lnkNot">
                                                                            </ext:HyperlinkButton>
                                                                            <ext:HyperlinkButton runat="server"
                                                                                ID="lnkShowChecked"
                                                                                Text="Show Checked"
                                                                                Cls="lnkNot">
                                                                            </ext:HyperlinkButton>
                                                                        </Items>
                                                                    </ext:Container>
                                                                </Items>
                                                            </ext:Toolbar>
                                                            <%-- FORM NOTES--%>
                                                            <ext:FormPanel runat="server"
                                                                ID="frmEditNot"
                                                                Hidden="true"
                                                                Cls="liNot">
                                                                <Items>
                                                                    <ext:Label runat="server"
                                                                        ID="lblNewNote"
                                                                        meta:resourcKey="lblNewNote"
                                                                        Text="New Note"
                                                                        Cls="rpanelTitleForm">
                                                                    </ext:Label>
                                                                    <ext:TextField runat="server"
                                                                        ID="txtTitle"
                                                                        Cls="RsideFORM"
                                                                        DataIndex="Notificacion"
                                                                        LabelAlign="Top"
                                                                        FieldLabel="<%$ Resources:Comun, strTitulo %>">
                                                                    </ext:TextField>
                                                                    <ext:TextArea runat="server"
                                                                        ID="txaText"
                                                                        LabelAlign="Top"
                                                                        DataIndex="Contenido"
                                                                        FieldLabel="<%$ Resources:Comun, strTexto %>"
                                                                        Cls="RsideFORM"
                                                                        Height="80">
                                                                    </ext:TextArea>
                                                                    <ext:TextField runat="server"
                                                                        ID="txtUser"
                                                                        LabelAlign="Top"
                                                                        DataIndex="ClienteID"
                                                                        FieldLabel="<%$ Resources:Comun, strUsuario %>"
                                                                        Cls="RsideFORM">
                                                                    </ext:TextField>
                                                                    <ext:TextField runat="server"
                                                                        ID="txtProfile"
                                                                        LabelAlign="Top"
                                                                        FieldLabel="<%$ Resources:Comun, strPerfil %>"
                                                                        Cls="RsideFORM">
                                                                    </ext:TextField>
                                                                    <ext:Label runat="server"
                                                                        ID="dateForm"
                                                                        Cls="dateNot">
                                                                    </ext:Label>
                                                                    <ext:ButtonGroup runat="server"
                                                                        ID="btnForm">
                                                                        <Items>
                                                                            <ext:Button runat="server"
                                                                                ID="btnDelete"
                                                                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                                                                Cls="btn-mini-secundario"
                                                                                Handler="RpanelBTNForm()">
                                                                            </ext:Button>
                                                                            <ext:Button runat="server"
                                                                                ID="btnSave"
                                                                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                                                                Cls="btn-mini-ppal"
                                                                                Handler="RpanelBTNForm()">
                                                                            </ext:Button>
                                                                        </Items>
                                                                    </ext:ButtonGroup>
                                                                </Items>
                                                            </ext:FormPanel>
                                                            <%--END FORM NOTES--%>
                                                        </Items>
                                                        <Content>
                                                            <ul id="ulNote">
                                                                <li id="liNotif1" class="liNot">
                                                                    <ext:Checkbox runat="server"
                                                                        ID="chkNotif1"
                                                                        Cls="chkNot">
                                                                    </ext:Checkbox>
                                                                    <div id="cntNotif1" class="cntNot">
                                                                        <ext:Label runat="server"
                                                                            ID="titNotif1"
                                                                            Cls="titNot"
                                                                            Text="Expired document">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="txtNotif1"
                                                                            Cls="txtNot"
                                                                            Text="The document KKH-D, has expired. Please check it out.">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="dateNotif1"
                                                                            Cls="dateNot"
                                                                            Text="19/11/2019 :: 17:29h">
                                                                        </ext:Label>
                                                                    </div>
                                                                    <ext:Button runat="server"
                                                                        ID="editNotif1"
                                                                        Cls="btnEdit-item"
                                                                        Handler="gestNotif();">
                                                                    </ext:Button>
                                                                </li>
                                                                <li id="liNotif21" class="liNot">
                                                                    <ext:Checkbox runat="server"
                                                                        ID="chkNotif2"
                                                                        Cls="chkNot">
                                                                    </ext:Checkbox>
                                                                    <div id="cntNotif21" class="cntNot">
                                                                        <ext:Label runat="server"
                                                                            ID="titNotif2"
                                                                            Cls="titNot"
                                                                            Text="Owner Ok">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="txtNotif2"
                                                                            Cls="txtNot"
                                                                            Text="The owner agrees, next step asap.">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="dateNotif2"
                                                                            Cls="dateNot"
                                                                            Text="18/11/2019 :: 12:15h">
                                                                        </ext:Label>
                                                                    </div>
                                                                    <ext:Button runat="server"
                                                                        ID="editNotif2"
                                                                        Cls="btnEdit-item"
                                                                        Handler="gestNotif();">
                                                                    </ext:Button>
                                                                </li>
                                                            </ul>
                                                        </Content>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>
                                            <%--NOTES PANEL--%>
                                            <ext:Panel runat="server"
                                                ID="pnNotesFull"
                                                Hidden="false">
                                                <Items>
                                                    <ext:Label runat="server"
                                                        ID="aLabel1"
                                                        IconCls="ico-head-Notif"
                                                        Cls="lblHeadAside"
                                                        Text="<%$ Resources:Comun, strAlertas %>">
                                                    </ext:Label>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnAlerts"
                                                        Header="false"
                                                        Hidden="false"
                                                        Disabled="false">
                                                        <Items>
                                                            <ext:Toolbar runat="server"
                                                                ID="tlbNotes">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        ID="addNotific2"
                                                                        IconCls="ico-addBtn"
                                                                        Cls="btn-mini-ppal btnAdd"
                                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                                        Handler="RpanelBTNAddAlert();">
                                                                    </ext:Button>
                                                                    <ext:Container runat="server" ID="ctnLnkNotif2">
                                                                        <Items>
                                                                            <ext:HyperlinkButton runat="server"
                                                                                ID="lnkNoChecked2"
                                                                                Text="Delete Checked"
                                                                                Cls="lnkNot">
                                                                            </ext:HyperlinkButton>
                                                                            <ext:HyperlinkButton runat="server"
                                                                                ID="lnkShowChecked2"
                                                                                Text="Show Checked"
                                                                                Cls="lnkNot">
                                                                            </ext:HyperlinkButton>
                                                                        </Items>
                                                                    </ext:Container>
                                                                </Items>
                                                            </ext:Toolbar>
                                                            <%-- FORM ALERT--%>
                                                            <ext:FormPanel runat="server"
                                                                ID="frmEditAlert"
                                                                Hidden="true"
                                                                Cls="liNot">
                                                                <Items>
                                                                    <ext:Label runat="server"
                                                                        ID="lblNewAlert"
                                                                        Text="New Alert"
                                                                        meta:resourceKey="lblNewAlert"
                                                                        Cls="rpanelTitleForm">
                                                                    </ext:Label>
                                                                    <ext:TextField runat="server"
                                                                        ID="txtTitleAlert"
                                                                        Cls="RsideFORM"
                                                                        LabelAlign="Top"
                                                                        FieldLabel="<%$ Resources:Comun, strTitulo %>">
                                                                    </ext:TextField>
                                                                    <ext:TextArea runat="server"
                                                                        ID="TextArea1"
                                                                        LabelAlign="Top"
                                                                        FieldLabel="<%$ Resources:Comun, strTexto %>"
                                                                        Cls="RsideFORM"
                                                                        Height="80">
                                                                    </ext:TextArea>
                                                                    <ext:TextField runat="server"
                                                                        ID="TextField2"
                                                                        LabelAlign="Top"
                                                                        FieldLabel="<%$ Resources:Comun, strUsuario %>"
                                                                        Cls="RsideFORM">
                                                                    </ext:TextField>
                                                                    <ext:TextField runat="server"
                                                                        ID="TextField3"
                                                                        LabelAlign="Top"
                                                                        FieldLabel="<%$ Resources:Comun, strPerfil %>"
                                                                        Cls="RsideFORM">
                                                                    </ext:TextField>
                                                                    <ext:Label runat="server"
                                                                        ID="Label1"
                                                                        Text="16/11/2019 :: 09:14h"
                                                                        Cls="dateNot">
                                                                    </ext:Label>
                                                                    <ext:ButtonGroup runat="server"
                                                                        ID="btnFormAlert">
                                                                        <Items>
                                                                            <ext:Button runat="server"
                                                                                ID="btnDeteleAlert"
                                                                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                                                                Cls="btn-mini-secundario"
                                                                                Handler="RpanelBTNFormAlert()">
                                                                            </ext:Button>
                                                                            <ext:Button runat="server"
                                                                                ID="btnSaveAlert"
                                                                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                                                                Cls="btn-mini-ppal"
                                                                                Handler="RpanelBTNFormAlert()">
                                                                            </ext:Button>
                                                                        </Items>
                                                                    </ext:ButtonGroup>
                                                                </Items>
                                                            </ext:FormPanel>
                                                            <%--END FORM ALERT--%>
                                                        </Items>
                                                        <Content>
                                                            <ul id="ulNotif">
                                                                <li id="aliNotif1" class="liNot">
                                                                    <ext:Checkbox runat="server"
                                                                        ID="aCheckbox1"
                                                                        Cls="chkNot">
                                                                    </ext:Checkbox>
                                                                    <div id="acntNotif1" class="cntNot">
                                                                        <ext:Label runat="server"
                                                                            ID="aLabel2"
                                                                            Cls="titNot"
                                                                            Text="Alert example">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="aLabel3"
                                                                            Cls="txtNot"
                                                                            Text="The document KKH-D, has expired. Please check it out.">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="aLabel4"
                                                                            Cls="dateNot"
                                                                            Text="19/11/2019 :: 17:29h">
                                                                        </ext:Label>
                                                                    </div>
                                                                    <ext:Button runat="server"
                                                                        ID="aButton2"
                                                                        Cls="btnEdit-item"
                                                                        Handler="gestNotif();">
                                                                    </ext:Button>
                                                                </li>
                                                                <li id="liNotif2" class="liNot">
                                                                    <ext:Checkbox runat="server"
                                                                        ID="aCheckbox2"
                                                                        Cls="chkNot">
                                                                    </ext:Checkbox>
                                                                    <div id="cntNotif2" class="cntNot">
                                                                        <ext:Label runat="server"
                                                                            ID="aLabel5"
                                                                            Cls="titNot"
                                                                            Text="Other alert example">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="aLabel6"
                                                                            Cls="txtNot"
                                                                            Text="The owner agrees, next step asap.">
                                                                        </ext:Label>
                                                                        <ext:Label runat="server"
                                                                            ID="aLabel7"
                                                                            Cls="dateNot"
                                                                            Text="18/11/2019 :: 12:15h">
                                                                        </ext:Label>
                                                                    </div>
                                                                    <ext:Button runat="server"
                                                                        ID="aButton3"
                                                                        Cls="btnEdit-item"
                                                                        Handler="gestNotif();">
                                                                    </ext:Button>
                                                                </li>
                                                            </ul>
                                                        </Content>
                                                    </ext:Panel>
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
            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
