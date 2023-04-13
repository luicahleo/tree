<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventarioCategoriasAtributos.aspx.cs" Inherits="TreeCore.ModInventario.InventarioCategoriasAtributos" %>

<%@ Register Src="/Componentes/AtributosConfiguracion.ascx" TagName="AtributosConfiguracion" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="css/styleInventarioCategorias.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Componentes/js/CategoriasAtributos.js"></script>
    <script type="text/javascript" src="../../Componentes/js/Atributos.js"></script>
    <script type="text/javascript" src="../../Componentes/js/AtributosConfiguracion.js"></script>
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden runat="server" ID="hdCliID" />
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
                ID="storeClientes"
                AutoLoad="true"
                OnReadData="storeClientes_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="ClienteID">
                        <Fields>
                            <ext:ModelField Name="ClienteID" Type="Int" />
                            <ext:ModelField Name="Cliente" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Cliente" Direction="ASC" />
                </Sorters>
                <Listeners>
                    <Load Handler="CargarStores();" />
                </Listeners>
            </ext:Store>

            <ext:Store runat="server"
                ID="storePrincipal"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storePrincipal_Refresh"
                RemoteSort="false"
                PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="InventarioAtributoCategoriaID">
                        <Fields>
                            <ext:ModelField Name="InventarioAtributoCategoriaID" Type="Int" />
                            <ext:ModelField Name="InventarioAtributoCategoria" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="EsPlantilla" Type="Boolean" />
                            <ext:ModelField Name="EsSubcategoria" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="InventarioAtributoCategoria" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="400"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextField runat="server"
                                ID="txtInventarioCategoria"
                                meta:resourceKey="txtInventarioCategoria"
                                FieldLabel="<%$ Resources:Comun, strCategoria %>"
                                MaxLength="40"
                                WidthSpec="100%"
                                LabelAlign="Top"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:Toolbar Dock="None" runat="server" WidthSpec="100%" StyleSpec="background-color: inherit !important;">
                                <Items>
                                    <ext:ToolbarFill />
                                    <ext:Button runat="server"
                                        ID="btnSeccion"
                                        Cls="btnSeccion"
                                        EnableToggle="true"
                                        Pressed="true"
                                        ToolTip="<%$ Resources:Comun, strSubcategoria %>">
                                        <Listeners>
                                            <Click Fn="SeleccionarTipoSeccion" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnSubcategoria"
                                        EnableToggle="true"
                                        ToolTip="<%$ Resources:Comun, strSubcategoriaAtributos %>"
                                        Cls="btnSubcategoria">
                                        <Listeners>
                                            <Click Fn="SeleccionarTipoSeccion" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnSubcategoriaPlantilla"
                                        EnableToggle="true"
                                        ToolTip="<%$ Resources:Comun, strSubcategoriaPlantillas %>"
                                        Cls="btnSubcategoriaPlantilla">
                                        <Listeners>
                                            <Click Fn="SeleccionarTipoSeccion" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarFill />
                                </Items>
                            </ext:Toolbar>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValido(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGestion}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport ID="vwResp" runat="server">
                <Listeners>
                    <AfterRender Handler="resizeTwoPanels();"></AfterRender>
                    <AfterLayout Handler="showPanelsByWindowSize()"></AfterLayout>
                    <Resize Handler="showPanelsByWindowSize(); resizeTwoPanels();" Buffer="50"></Resize>
                </Listeners>
                <Items>
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
                                        ForceFit="true"
                                        StoreID="storePrincipal"
                                        Title="<%$ Resources:Comun, strComponentes %>"
                                        RootVisible="false"
                                        EnableColumnHide="false"
                                        EnableColumnMove="false"
                                        EnableColumnResize="false">
                                        <DockedItems>
                                            <ext:Toolbar runat="server" Dock="Top" Cls="toolBarInventario">
                                                <Items>
                                                    <ext:ComboBox runat="server"
                                                        ID="cmbClientes"
                                                        meta:resourceKey="cmbClientes"
                                                        StoreID="storeClientes"
                                                        DisplayField="Cliente"
                                                        ValueField="ClienteID"
                                                        Cls="comboGrid pos-boxGrid"
                                                        Width="200"
                                                        QueryMode="Local"
                                                        Hidden="true"
                                                        EmptyText="<%$ Resources:Comun, cmbClientes.EmptyText %>"
                                                        FieldLabel="<%$ Resources:Comun, cmbClientes.FieldLabel %>">
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCliente" />
                                                            <TriggerClick Fn="RecargarClientes" />
                                                        </Listeners>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                Weight="-1"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                                        </Triggers>
                                                    </ext:ComboBox>
                                                    <ext:ToolbarFill />
                                                </Items>
                                            </ext:Toolbar>

                                            <ext:Toolbar runat="server" Dock="Top" ID="toolbarButtons" Layout="ColumnLayout" Hidden="false" StyleSpec="border-style: hidden !important;">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnAnadir"
                                                        Cls="btnAnadir"
                                                        AriaLabel="Añadir"
                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                        Handler="AgregarEditar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnEditar"
                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                        Cls="btnEditar"
                                                        Handler="MostrarEditar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnEliminar"
                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                        Cls="btnEliminar"
                                                        Handler="Eliminar();" />
                                                    <ext:Button runat="server"
                                                        ID="btnActivar"
                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
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
                                                        Cls="btnRefrescar maxHeiInv"
                                                        Handler="Refrescar();" />
                                                    <ext:Button runat="server"
                                                        ID="btnDescargar"
                                                        MaxHeight="32"
                                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                        Cls="btnDescargar"
                                                        Hidden="true"
                                                        Handler="ExportarDatos('InventarioCategoriasAtributos', hdCliID.value, #{GridPanelCategorias}, '');" />
                                                </Items>

                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server" Dock="Top" Cls="toolBarInventario" MinHeight="72">
                                                <Items>
                                                    <ext:MultiCombo
                                                        ID="cmbTiposVinculaciones"
                                                        runat="server"
                                                        LabelAlign="Top"
                                                        Cls="txtSearchC"
                                                        FieldLabel="<%$ Resources:Comun, strTipo %>"
                                                        DisplayField="Nombre"
                                                        WidthSpec="100%"
                                                        OverflowX="Hidden"
                                                        Selectable="true"
                                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                                        ValueField="InventarioTipoVinculacionID">
                                                        <Store>
                                                        </Store>
                                                        <Listeners>
                                                            <Select Fn="SeleccionarCombo" />
                                                            <TriggerClick Fn="RecargarCombo" />
                                                            <FocusLeave Handler="App.storePrincipal.reload()" />
                                                        </Listeners>
                                                        <Items>
                                                            <ext:ListItem Text="<%$ Resources:Comun, strSubcategoria %>" Value="0" />
                                                            <ext:ListItem Text="<%$ Resources:Comun, strSubcategoriaAtributos %>" Value="1" />
                                                            <ext:ListItem Text="<%$ Resources:Comun, strSubcategoriaPlantillas %>" Value="2" />
                                                        </Items>
                                                        <Triggers>
                                                            <ext:FieldTrigger
                                                                IconCls="ico-reload"
                                                                Hidden="true"
                                                                QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                                Weight="-1" />
                                                        </Triggers>
                                                    </ext:MultiCombo>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>

                                        <Listeners>
                                            <AfterRender Fn="SetMaxHeightSuperior" />
                                        </Listeners>
                                        <ColumnModel>
                                            <Columns>
                                                <ext:Column runat="server"
                                                    ID="colTipoPlantilla"
                                                    DataIndex="Activo"
                                                    Align="Center"
                                                    Filterable="false"
                                                    Sortable="false"
                                                    Width="35"
                                                    MaxWidth="35"
                                                    MinWidth="35">
                                                    <Renderer Fn="TipoSubcategoriaRender" />
                                                </ext:Column>
                                                <ext:Column runat="server"
                                                    ID="colCategoria"
                                                    meta:resourceKey="colNombre"
                                                    DataIndex="InventarioAtributoCategoria"
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
                                                ID="GridRowSelect"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelect" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <View>
                                            <ext:GridView runat="server" StripeRows="true" TrackOver="true" />
                                        </View>
                                    </ext:GridPanel>
                                    <ext:ToolTip
                                        runat="server"
                                        Target="=App.GridPanelCategorias.getView().el"
                                        Delegate="=App.GridPanelCategorias.getView().itemSelector"
                                        TrackMouse="true">
                                        <Listeners>
                                            <Show Handler="TooltipGrid(this, #{GridPanelCategorias});" />
                                        </Listeners>
                                    </ext:ToolTip>
                                </Items>
                            </ext:Container>
                            <ext:Container ID="ctMain2" runat="server" Region="Center"
                                Flex="1">
                                <Items>
                                    <ext:Panel
                                        Hidden="false"
                                        Title="<%$ Resources:Comun, strConfigurador %>"
                                        runat="server"
                                        ForceFit="true"
                                        ID="pnConfigurador"
                                        meta:resourceKey="pnConfigurador"
                                        Cls="gridPanelInventario pnConfiguradorAtributos"
                                        Scrollable="Vertical"
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
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>
                                        </Items>
                                        <Listeners>
                                            <AfterRender Fn="SetMaxHeightSuperior" />
                                        </Listeners>
                                    </ext:Panel>
                                </Items>
                                <Content>
                                    <local:AtributosConfiguracion
                                        ID="AtributosConfiguracion"
                                        runat="server" />
                                </Content>
                            </ext:Container>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
    <script type="text/javascript">
        Ext.Ajax.timeout = 120000;
        Ext.net.DirectEvent.timeout = 120000;
    </script>
</body>
</html>
