<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventarioCategorias.aspx.cs" Inherits="TreeCore.ModInventario.InventarioCategorias" %>

<%@ Register Src="/Componentes/CategoriasAtributos.ascx" TagName="CategoriasAtributos" TagPrefix="local" %>
<%@ Register Src="/Componentes/Atributos.ascx" TagName="Atributos" TagPrefix="local" %>
<%@ Register Src="/Componentes/AtributosConfiguracion.ascx" TagName="AtributosConfiguracion" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <script type="text/javascript" src="../../Scripts/jquery.amsify.suggestags.js"></script>
    <link href="../../CSS/amsify.suggestags.css" rel="stylesheet" type="text/css" />
    <link href="css/styleInventarioCategorias.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Componentes/js/CategoriasAtributos.js"></script>
    <script type="text/javascript" src="../../Componentes/js/Atributos.js"></script>
    <script type="text/javascript" src="../../Componentes/js/AtributosConfiguracion.js"></script>
    <form id="form1" runat="server">
        <div>
            <ext:ResourcePlaceHolder runat="server" Mode="Script" />
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdCatSelect" runat="server" />
            <ext:Hidden ID="hdListaCategorias" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore" DisableViewState="true" ShowWarningOnAjaxFailure="false">
                <Listeners>
                    <AjaxRequestException Fn="winErrorTimeout" />
                </Listeners>
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store runat="server"
                ID="storeTipoEmplazamientos"
                AutoLoad="false"
                OnReadData="storeTipoEmplazamientos_Refresh"
                RemoteFilter="false"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="Tipo">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTipoID" Type="Int" />
                            <ext:ModelField Name="Tipo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Tipo" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <Load Fn="DeseleccionarGrilla" />
                </Listeners>
            </ext:Store>

            <ext:Store ID="storeIconos" runat="server" AutoLoad="false" OnReadData="storeIconos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model
                        IDProperty="icono"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="iconPath" />
                            <ext:ModelField Name="icono" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store ID="storeCategoriasLibres" runat="server" AutoLoad="false" OnReadData="storeCategoriasLibres_Refresh"
                RemoteSort="false">
                <Model>
                    <ext:Model runat="server" IDProperty="InventarioAtributoCategoriaID">
                        <Fields>
                            <ext:ModelField Name="InventarioAtributoCategoriaID" Type="Int" />
                            <ext:ModelField Name="InventarioAtributoCategoria" />
                            <ext:ModelField Name="EsPlantilla" Type="Boolean" />
                            <ext:ModelField Name="EsSubcategoria" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="EsPlantilla" Direction="DESC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeCategorias" runat="server" AutoLoad="true" OnReadData="storeCategorias_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="InventarioCategoriaID">
                        <Fields>
                            <ext:ModelField Name="InventarioCategoriaID" Type="Int" />
                            <ext:ModelField Name="InventarioCategoria" />
                            <ext:ModelField Name="Activo" />
                            <ext:ModelField Name="Icono" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>



            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window
                Hidden="true"
                runat="server"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                ID="winGestion"
                MaxWidth="400"
                MinWidth="400"
                MinHeight="340"
                MaxHeight="340"
                Scrollable="Vertical"
                AutoDataBind="true"
                Cls="headerGrey">
                <Items>
                    <ext:FormPanel runat="server" ID="formGestion" WidthSpec="100%">
                        <Items>
                            <ext:Container ID="ctForm" runat="server" Cls="containerGrid">
                                <Items>
                                    <ext:Container runat="server" Cls="mainContainer">
                                        <Items>
                                            <%--<ext:Label runat="server" Text="<%$ Resources:Comun, strNombre %>" Cls="headerAlignedV2" />--%>
                                            <ext:TextField runat="server"
                                                ID="txtNombre"
                                                meta:resourceKey="txtNombre"
                                                MaxLength="30"
                                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                WidthSpec="100%"
                                                AllowBlank="false"
                                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                                LabelAlign="Top"
                                                ValidationGroup="FORM"
                                                CausesValidation="true"
                                                Regex="/^[a-zA-Z0-9!@#\$%\^\&\)\(' '+=._-]+$/"
                                                RegexText="<%$ Resources:Comun, strRegExpExcelTabName %>" />
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Cls="mainContainer">
                                        <Items>
                                            <%--<ext:Label runat="server" Text="<%$ Resources:Comun, strCodigo %>" Cls="headerAlignedV2" />--%>
                                            <ext:TextField runat="server"
                                                ID="txtCodigo"
                                                meta:resourceKey="txtCodigo"
                                                MaxLength="30"
                                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                                WidthSpec="100%"
                                                FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                                LabelAlign="Top"
                                                Regex="/^[^$%&|<>/\#]*$/"
                                                RegexText="<%$ Resources:Comun, regexNombreText %>"
                                                AllowBlank="false"
                                                ValidationGroup="FORM"
                                                CausesValidation="true" />
                                        </Items>
                                    </ext:Container>
                                </Items>
                                <Items>
                                    <ext:Container runat="server" Cls="mainContainer">
                                        <Items>
                                            <%--<ext:Label runat="server" Text="<%$ Resources:Comun, strIcono %>" Cls="headerAlignedV2" />--%>
                                            <ext:ComboBox
                                                meta:resourcekey="cmbIconos"
                                                ID="cmbIconos"
                                                runat="server"
                                                StoreID="storeIconos"
                                                AllowBlank="false"
                                                Editable="true"
                                                ValueField="icono"
                                                DisplayField="icono"
                                                WidthSpec="100%"
                                                LabelAlign="Top"
                                                FieldLabel="<%$ Resources:Comun, strIcono %>"
                                                EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                                <ListConfig>
                                                    <ItemTpl runat="server">
                                                        <Html>
                                                            <img src="{iconPath}" alt="{icono}" />
                                                            {icono}
                                                        </Html>
                                                    </ItemTpl>
                                                </ListConfig>
                                                <Listeners>
                                                    <Select Fn="SeleccionarIconos" />
                                                    <TriggerClick Fn="RecargarIconos" />
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
                                    </ext:Container>
                                    <ext:Container runat="server" ID="ContainerChecks" Hidden="true" Cls="item-2ColCssGrd">
                                        <Items>
                                            <ext:Checkbox ID="chkEspacioEnSuelo" meta:resourcekey="chkEspacioEnSuelo" runat="server" Hidden="true" />
                                            <ext:Checkbox ID="chkBloqueoTerceros" meta:resourcekey="chkBloqueoTerceros" runat="server" Hidden="true" />
                                            <ext:Checkbox ID="chkEsContenedor" meta:resourcekey="chkEsContenedor" runat="server" Hidden="true" />
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValido(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        ID="btnGuardar"
                        Cls="btn-accept"
                        Width="100"
                        Handler="winGestionBotonGuardar();" />
                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>


            <ext:Viewport ID="vwResp" runat="server">
                <Listeners>
                    <AfterRender Handler="winFormResize(); resizeTwoPanels();"></AfterRender>
                    <AfterLayout Handler="showPanelsByWindowSize()"></AfterLayout>
                    <Resize Handler="showPanelsByWindowSize(); resizeTwoPanels();" Buffer="50"></Resize>
                </Listeners>
                <Items>
                    <ext:Button runat="server" ID="btnCollapseAsR" Cls="btn-trans" Handler="hidePn();" Hidden="true"></ext:Button>
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
                                        Handler="loadPanelByBtns('Prev'); resizeTwoPanels();"
                                        Disabled="true">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnNext"
                                        IconCls="ico-next-w"
                                        Cls="SliderBtn"
                                        Handler="loadPanelByBtns('Next'); resizeTwoPanels();"
                                        Disabled="false">
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </Items>
                    </ext:Toolbar>
                    <ext:Container ID="ctSlider" runat="server">
                        <Items>
                            <ext:Container ID="ctMain1" runat="server" MaxWidth="300"
                                Flex="12" Region="West">
                                <Items>
                                    <ext:GridPanel
                                        Hidden="false"
                                        Cls="gridPanel TreePnl"
                                        ID="GridPanelCategorias"
                                        meta:resourceKey="GridPanelCategorias"
                                        runat="server"
                                        Scrollable="Vertical"
                                        MinHeight="600"
                                        ForceFit="true"
                                        StoreID="storeCategorias"
                                        RootVisible="false"
                                        EnableColumnHide="false"
                                        EnableColumnMove="false"
                                        EnableColumnResize="false">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" Dock="Top" ID="toolbarButtons" Hidden="false" StyleSpec="border-style: hidden !important;" OverflowHandler="Scroller">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnAnadir"
                                                        meta:resourceKey="btnAnadir"
                                                        Cls="btnAnadir"
                                                        AriaLabel="Añadir"
                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                        Handler="AgregarEditar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnEditar"
                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                        meta:resourceKey="btnEditar"
                                                        Cls="btnEditar"
                                                        Handler="MostrarEditar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnEliminar"
                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                        meta:resourceKey="btnEliminar"
                                                        Cls="btnEliminar"
                                                        Handler="Eliminar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnActivar"
                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                        meta:resourceKey="btnActivar"
                                                        Cls="btnActivar"
                                                        Handler="Activar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnActivos"
                                                        ToolTip="<%$ Resources:Comun, btnActivos.ToolTip %>"
                                                        Cls="btn-toggleGrid"
                                                        EnableToggle="true"
                                                        Pressed="true"
                                                        Width="42"
                                                        Handler="VerActivos();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnRefrescar"
                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                        meta:resourceKey="btnRefrescar"
                                                        Cls="btnRefrescar"
                                                        Handler="Refrescar();" />
                                                    <ext:Button runat="server"
                                                        ID="btnDescargar"
                                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                        meta:resourceKey="btnDescargar"
                                                        Cls="btnDescargar"
                                                        Hidden="true"
                                                        Handler="" />
                                                    <ext:Button runat="server"
                                                        ID="btnExportarModeloDatos"
                                                        ToolTip="<%$ Resources:Comun, btnExportarModeloDatos.ToolTip %>"
                                                        meta:resourceKey="btnExportarModeloDatos"
                                                        Cls="btnDownloadContent"
                                                        Hidden="true"
                                                        Handler="ExportarModelo();" />
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server" Dock="Top" Cls="toolBarInventario">
                                                <Items>
                                                    <ext:ComboBox meta:resourceKey="cmbTipoEmplazamientos"
                                                        ID="cmbTipoEmplazamientos" runat="server"
                                                        StoreID="storeTipoEmplazamientos"
                                                        DisplayField="Tipo"
                                                        ValueField="EmplazamientoTipoID"
                                                        Editable="true"
                                                        FieldLabel="<%$ Resources:Comun, strTipoEmplazamiento %>"
                                                        LabelAlign="Top"
                                                        Scrollable="Vertical"
                                                        OverflowX="Hidden"
                                                        WidthSpec="100%"
                                                        QueryMode="Local"
                                                        Cls="txtSearchC"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                        AllowBlank="true">
                                                        <Listeners>
                                                            <Select Fn="SeleccionarTipoEmplazamientos" />
                                                            <TriggerClick Fn="RecargarTipoEmplazamientos" />
                                                            <Focus Fn="focusField" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Toolbar>

                                            <%-- <ext:Toolbar runat="server" Dock="Top" Cls="toolBarInventario" Hidden="true">
                                                <Items>
                                                    <ext:TextField
                                                        runat="server"
                                                        Width="120"
                                                        EmptyText="Search"
                                                        ID="searchElement"
                                                        LabelWidth="50">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Search" />
                                                        </Triggers>
                                                    </ext:TextField>
                                                    <ext:ToolbarFill />
                                                </Items>
                                            </ext:Toolbar> --%>
                                        </DockedItems>

                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colIcono"
                                                    DataIndex="Icono"
                                                    Align="Center"
                                                    Sortable="false"
                                                    MinWidth="30"
                                                    Width="30">
                                                    <Renderer Fn="RenderIcono" />
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colCategoria"
                                                    meta:resourceKey="colNombre"
                                                    DataIndex="InventarioCategoria"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    MinWidth="100"
                                                    Flex="1" />
                                                <ext:Column runat="server"
                                                    ID="colActivo"
                                                    DataIndex="Activo"
                                                    Align="Center"
                                                    Cls="col-activo"
                                                    Hidden="true"
                                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                    Width="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel runat="server"
                                                ID="GridRowSelectArbol"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelectArbol" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Listeners>
                                            <Select Fn="SelectItemMenu" />
                                            <AfterRender Fn="SetMaxHeight" />
                                        </Listeners>
                                        <Plugins>
                                            <ext:GridFilters runat="server"
                                                ID="gridFilters"
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                meta:resourceKey="gridFilters" />
                                            <ext:CellEditing runat="server"
                                                ClicksToEdit="2" />
                                        </Plugins>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="ctMain2" runat="server" Region="Center"
                                Flex="1">
                                <Items>
                                    <ext:Panel
                                        Hidden="false"
                                        Title="ELEMENT FORM SETTING"
                                        runat="server"
                                        ForceFit="true"
                                        MinHeight="600"
                                        ID="pnConfigurador"
                                        meta:resourceKey="pnConfigurador"
                                        Cls="gridPanelInventario pnConfiguradorAtributos"
                                        OverflowY="Auto"
                                        OverflowX="Auto">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" Cls="toolbarHacked">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnRestriccionActive"
                                                        IconCls="btn-filtGrid ico-checked-16-grey">
                                                        <CustomConfig>
                                                            <ext:ConfigItem Name="Modo" Value="Active" />
                                                        </CustomConfig>
                                                        <Listeners>
                                                            <Click Fn="CambiarRestriccionDefecto" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnRestriccionDisabled"
                                                        IconCls="btn-filtGrid ico-disabled-16-grey">
                                                        <CustomConfig>
                                                            <ext:ConfigItem Name="Modo" Value="Disabled" />
                                                        </CustomConfig>
                                                        <Listeners>
                                                            <Click Fn="CambiarRestriccionDefecto" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnRestriccionHidden"
                                                        IconCls="btn-filtGrid ico-hidden-16-grey">
                                                        <CustomConfig>
                                                            <ext:ConfigItem Name="Modo" Value="Hidden" />
                                                        </CustomConfig>
                                                        <Listeners>
                                                            <Click Fn="CambiarRestriccionDefecto" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarFill />
                                                    <ext:Button runat="server"
                                                        meta:resourceKey="btnNuevaCategorias"
                                                        ID="btnNuevaCategorias"
                                                        IconCls="ico-addBtn"
                                                        Cls="btn-ppal btnAdd btnNwCat"
                                                        Text="Category"
                                                        Hidden="true">
                                                        <Menu>
                                                            <ext:Menu runat="server" ID="menuNuevaCategorias">
                                                                <Items>
                                                                </Items>
                                                                <Listeners>
                                                                    <Click Fn="SelectItemNuevaCategorias" />
                                                                </Listeners>
                                                            </ext:Menu>
                                                        </Menu>
                                                    </ext:Button>
                                                    <ext:ComboBox runat="server"
                                                        ID="cmbNuevaCategorias"
                                                        meta:resourceKey="strComponent"
                                                        Text="<%$ Resources:Comun, strComponent %>"
                                                        Height="32"
                                                        Width="140"
                                                        MarginSpec="0 31 0 0"
                                                        Editable="false"
                                                        IconCls="ico-addBtn"
                                                        Cls="cmbFakeButton"
                                                        StoreID="storeCategoriasLibres"
                                                        DisplayField="InventarioAtributoCategoria"
                                                        ValueField="InventarioAtributoCategoriaID"
                                                        Scrollable="Vertical"
                                                        Disabled="false"
                                                        AllowBlank="true">
                                                        <%--  <Triggers>
                                                            <ext:FieldTrigger IconCls="ico-operator-plus"
                                                                Hidden="true"
                                                                QTip="<%$ Resources:Comun, btnAnadir.ToolTip %>" />
                                                        </Triggers>--%>
                                                        <Listeners>
                                                            <Select Fn="SelectItemNuevaCategorias" />
                                                            <%-- <TriggerClick Fn="AñadirNuevaCategoria" />--%>
                                                            <FocusLeave Handler="this.reset();" />
                                                        </Listeners>
                                                        <ListConfig>
                                                            <ItemTpl runat="server">
                                                                <Html>
                                                                    <div class="icon-combo-item itemCmbCategorias" esplantilla="{EsPlantilla}" essubcategoria="{EsSubcategoria}">
                                                                        <div class="icono"></div>
                                                                        <div class="texto">
                                                                            {InventarioAtributoCategoria}
                                                                        </div>
                                                                    </div>
                                                                </Html>
                                                            </ItemTpl>
                                                        </ListConfig>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:Toolbar>




                                        </DockedItems>
                                        <Items>
                                        </Items>
                                        <Listeners>
                                            <AfterRender Fn="SetMaxHeight" />
                                        </Listeners>
                                    </ext:Panel>
                                </Items>
                            </ext:Container>
                            <%--<ext:Container ID="ctMain3" runat="server">
                                <Items>
                                    <ext:Label runat="server" Text="ct3"></ext:Label>

                                </Items>

                            </ext:Container>--%>
                        </Items>
                        <Content>
                            <local:AtributosConfiguracion
                                ID="AtributosConfiguracion"
                                runat="server" />
                        </Content>
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
    <script type="text/javascript">
        Ext.Ajax.timeout = 120000;
        Ext.net.DirectEvent.timeout = 120000;
    </script>
</body>
</html>
