<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Formulas.aspx.cs" Inherits="TreeCore.PaginasComunes.Formulas" %>

<%@ Register Src="~/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>
<%@ Register Src="~/Componentes/ReajustePrecios.ascx" TagName="updateSetting" TagPrefix="local" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <link href="../../CSS/expression-builder.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript" src="/Scripts/jquery-3.5.1.min.js" ></script>--%>
    <script type="text/javascript" src="../../Scripts/Expression-Builder/expression-builder-v2.js"></script>

    <form id="form1" runat="server">
        <div>
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdArbol" runat="server" />
            <ext:Hidden ID="hdEditadoObjetoID" runat="server" />
            <ext:Hidden ID="hdObjetoNegocioTipoID" runat="server" />
            <ext:Hidden ID="hdTablaModeloDatoID" runat="server" />
            <ext:Hidden ID="hdCategoriaID" runat="server" />
            <ext:Hidden ID="hdFormulaID" runat="server" />
            <ext:Hidden ID="hdFormula" runat="server" />
            <ext:Hidden ID="hdVariablesFormulas" runat="server" />
            <ext:Hidden ID="hdCampoVinculadoRuta" runat="server" />  
            <ext:Hidden ID="hdCampoVinculadoCategoria" runat="server" />
            <ext:Hidden ID="hdCampoVinculadoCategoriaOriginal" runat="server" />
            <ext:Hidden ID="hdCampoVinculadoTipo" runat="server" />
            <ext:Hidden ID="hdRuta" runat="server" />

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>
            <ext:Store
                runat="server"
                ID="storePrincipal"
                AutoLoad="false"
                OnReadData="storePrincipal_Refresh"
                PageSize="20"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Id">
                        <Fields>
                            <ext:ModelField Name="Id" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Icono" />
                            <ext:ModelField Name="Tipo" />
                            <ext:ModelField Name="TablaModeloDatoID" />
                            <ext:ModelField Name="InventarioCategoriaID" />
                            <ext:ModelField Name="ObjetoTipoID" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <%--<BeforeLoad Fn="DeseleccionarGrilla" />--%>
                </Listeners>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeCampos"
                runat="server"
                OnReadData="storeCampos_Refresh"
                AutoLoad="false">
                <Model>
                    <ext:Model runat="server" IDProperty="Campo">
                        <Fields>
                            <ext:ModelField Name="Campo" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="TypeData" />
                            <ext:ModelField Name="TipoCampo" />
                            <ext:ModelField Name="ColumnaModeloDatoID" />
                            <ext:ModelField Name="QueryValores" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" />
                </Sorters>
                <Listeners>
                    <%--<DataChanged Fn="beforeLoadCmbField" />--%>
                </Listeners>
            </ext:Store>

            <ext:Store
                runat="server"
                ID="storeSelectCamposVinculados"
                AutoLoad="false"
                OnReadData="storeSelectCamposVinculados_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ID"
                        runat="server">
                        <Fields>
                            <ext:ModelField Name="ID" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="TypeDynamicID" />
                            <ext:ModelField Name="Dynamic" />
                            <ext:ModelField Name="DataType" />
                            <ext:ModelField Name="esCarpeta" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="esCarpeta" Direction="DESC" />
                    <ext:DataSorter Property="Name" />
                </Sorters>
            </ext:Store>

            <%-- FIN STORES --%>

            <ext:Window
                runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="400"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel
                        runat="server"
                        ID="formGestion"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextField
                                runat="server"
                                ID="txtNombre"
                                WidthSpec="100%"
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                runat="server"
                                ID="txtCodigo"
                                WidthSpec="100%"
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValido(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button
                        runat="server"
                        ID="btnCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGestion}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button
                        runat="server"
                        ID="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>


            <ext:Window runat="server" 
                ID="winSelectCampoVinculado"
                Title="<%$ Resources:Comun, strElegirCampoVinculado %>"
                Height="350"
                Width="300"
                Cls="gridPanel winSelectCampoVinculado"
                Hidden="true"
                Resizable="false"
                Layout="VBoxLayout"
                Modal="true">
                <Defaults>
                    <ext:Parameter Name="margin"
                        Value="0 0 5 0"
                        Mode="Value" />
                </Defaults>
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Center" />
                </LayoutConfig>
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);" />
                    <%--<Close Fn="closeWinSelectCampoVinculado" />--%>
                </Listeners>
                <Items>
                    <ext:GridPanel runat="server"
                        ID="gridCamposVinculados"
                        StoreID="storeSelectCamposVinculados"
                        Flex="1"
                        Layout="FitLayout"
                        Cls="gridPanel noBorder"
                        Header="false"
                        Hidden="false"
                        Width="300">
                        <DockedItems>
                            <ext:Toolbar
                                runat="server"
                                ID="Toolbar2"
                                Cls="tlbGrid"
                                Dock="Top">
                                <Items>
                                    <ext:Button runat="server"
                                        Cls="noBorder btnBack"
                                        IconCls="ico-prev"
                                        ID="Button1">
                                        <Listeners>
                                            <Click Fn="VolverAtrasCampoVinculado" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        Text="<%$ Resources:Comun, strRaiz %>"
                                        Cls="tbNavPath btnBack"
                                        ID="Button2">
                                        <Listeners>
                                            <Click Fn="IrRutaRaizCampoVinculado" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        Cls="noBorder btnNextRuta"
                                        IconCls="ico-next"
                                        Hidden="true"
                                        ID="btnRaizCarpeta">
                                    </ext:Button>
                                    <ext:Button
                                        runat="server"
                                        Hidden="true"
                                        ID="btnMenuRuta"
                                        Cls="noBorder btnMenuRuta btnBack"
                                        IconCls="ico-nav-folder-gr-16">
                                        <Menu>
                                            <ext:Menu
                                                runat="server"
                                                ID="menuRuta"
                                                Cls="menuRuta">
                                                <Items>
                                                </Items>
                                                <Listeners>
                                                    <Click Fn="SeleccionarRutaCampoVinculado" />
                                                </Listeners>
                                            </ext:Menu>
                                        </Menu>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        Cls="noBorder btnNextRuta"
                                        IconCls="ico-next"
                                        Hidden="true"
                                        ID="Button3">
                                    </ext:Button>
                                    <ext:Label runat="server"
                                        ID="Label12"
                                        Cls="rutaCategoria btnBack"
                                        Height="32"
                                        Hidden="true">
                                    </ext:Label>
                                    <ext:Button
                                        runat="server"
                                        Hidden="true"
                                        ID="btnPadreCarpetaActucal"
                                        Cls="noBorder btnMenuRuta btnBack"
                                        IconCls="ico-link-vertical">
                                        <Menu>
                                            <ext:Menu
                                                runat="server"
                                                ID="menuPadreCarpetaActual"
                                                Cls="menuRuta">
                                                <Items>
                                                </Items>
                                                <Listeners>
                                                    <Click Fn="SeleccionarPadreCampoVinculado" />
                                                </Listeners>
                                            </ext:Menu>
                                        </Menu>
                                    </ext:Button>
                                    <ext:ToolbarFill />
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar
                                runat="server"
                                ID="tlbCampoVinculadoBuscador"
                                Cls="tlbGrid"
                                Dock="Top">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtCampoVinculadoBuscador"
                                        EmptyText="<%$ Resources:Comun, strBuscar %>"
                                        WidthSpec="100%">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear"
                                                Hidden="true"
                                                Handler="limpiarCampoVinculadoBuscador();" />
                                            <ext:FieldTrigger Icon="Search" />
                                        </Triggers>
                                        <Listeners>
                                            <%--<TriggerClick Fn="buscador" />--%>
                                            <Change Fn="FiltrarColumnasCampoVinculadoBuscador" />
                                        </Listeners>
                                    </ext:TextField>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel>
                            <Columns>
                                <ext:Column runat="server"
                                    ID="icon"
                                    DataIndex="DataType"
                                    Flex="1"
                                    MaxWidth="50"
                                    Align="Center"
                                    Sortable="false">
                                    <Renderer Fn="RendererIconLinkedField" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colLinkedName"
                                    DataIndex="Name"
                                    Flex="3"
                                    Align="Start"
                                    Sortable="false">
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <Listeners>
                            <RowDblClick Fn="LinkedGridDoubleClick" />
                        </Listeners>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFilters1"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="GridFilters">
                            </ext:GridFilters>
                        </Plugins>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server"
                                ID="RowSelectionModel1"
                                Mode="Multi">
                                <Listeners>
                                    <Select Fn="gridCamposVinculados_RowSelect" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                    </ext:GridPanel>
                </Items>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <ext:Viewport ID="vwResp" runat="server" Layout="FitLayout" OverflowY="auto">
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

                                    <ext:Toolbar runat="server" ID="tbFiltrosYSliders" Dock="Top" Cls="tbGrey tbNoborder " Hidden="false" Layout="HBoxLayout" Flex="1">
                                        <Items>

                                            <ext:Toolbar runat="server" ID="tbSliders" Dock="Top" Hidden="false" MinHeight="36" Cls="tbGrey tbNoborder PosUnsFloatR">
                                                <Items>

                                                    <ext:Button runat="server" ID="btnPrevGrid" IconCls="ico-prev-w" Cls="btnMainSldr SliderBtn" Handler="SliderMove('Prev');" Disabled="true"></ext:Button>
                                                    <ext:Button runat="server" ID="btnNextGrid" IconCls="ico-next-w" Cls="SliderBtn" Handler="SliderMove('Next');" Disabled="false"></ext:Button>

                                                </Items>
                                            </ext:Toolbar>

                                        </Items>
                                    </ext:Toolbar>

                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server" ID="wrapComponenteCentral" Layout="HBoxLayout" BodyCls="tbGrey" MaxWidth="1700">
                                        <Listeners>
                                            <AfterRender Handler="ControlSlider(this)"></AfterRender>
                                            <Resize Handler="ControlSlider(this)"></Resize>
                                        </Listeners>
                                        <LayoutConfig>
                                            <ext:HBoxLayoutConfig Align="Stretch"></ext:HBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:GridPanel ID="gridElementos"
                                                runat="server"
                                                Header="true"
                                                EnableColumnHide="false"
                                                SelectionMemory="false"
                                                Flex="12"
                                                MaxWidth="350"
                                                Region="West"
                                                Hidden="false"
                                                StoreID="storePrincipal"
                                                Title="Elementos categorias"
                                                Cls="gridPanel grdPnColIcons"
                                                OverflowX="Hidden"
                                                OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server" ID="tlbBase" Dock="top" Cls="tlbGrid" OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:Button runat="server" ID="btnAnadir" Cls="btnAnadir" AriaLabel="Añadir" Disabled="true" ToolTip="<%$ Resources:Comun, strAnadir %>">
                                                                <Listeners>
                                                                    <Click Fn="AgregarEditar" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnEliminar"
                                                                ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                Cls="btnEliminar"
                                                                Disabled="true"
                                                                Handler="Eliminar();" />
                                                            <ext:Button runat="server"
                                                                ID="btnRefrescar"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                Cls="btnRefrescar"
                                                                Handler="Refrescar();" />
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="tlbRuta"
                                                        Cls="tbGrey"
                                                        Dock="Top">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                Cls="noBorder btnBack"
                                                                IconCls="ico-prev"
                                                                ID="btnBack">
                                                                <Listeners>
                                                                    <Click Fn="VolverAtras" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                Text="<%$ Resources:Comun, strRaiz %>"
                                                                Cls="tbNavPath btnBack"
                                                                ID="lbRutaEmplazamientoTipo">
                                                                <Listeners>
                                                                    <Click Fn="IrRutaRaiz" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                Cls="noBorder btnNextRuta"
                                                                IconCls="ico-next"
                                                                Hidden="true"
                                                                ID="btRaiz">
                                                            </ext:Button>
                                                            <ext:Label runat="server"
                                                                ID="lbRaiz"
                                                                Cls="rutaCategoria btnBack"
                                                                Height="32"
                                                                Hidden="true">
                                                            </ext:Label>
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
                                                            <ext:Button runat="server"
                                                                Cls="noBorder btnNextRuta"
                                                                IconCls="ico-next"
                                                                Hidden="true"
                                                                ID="btnNext">
                                                            </ext:Button>
                                                            <ext:Label runat="server"
                                                                ID="lbcategoria"
                                                                Cls="rutaCategoria btnBack"
                                                                Height="32"
                                                                Hidden="true">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>

                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:Column runat="server" Align="Center"
                                                            Sortable="false"
                                                            MinWidth="30"
                                                            MaxWidth="30" DataIndex="Icono" ID="colIcono">
                                                            <Renderer Fn="RenderIcono" />
                                                        </ext:Column>
                                                        <ext:Column runat="server" Flex="8" Text="<%$ Resources:Comun, strNombre %>" MinWidth="120" DataIndex="Nombre" />

                                                    </Columns>
                                                </ColumnModel>
                                                <Listeners>
                                                    <DoubleTap Fn="EntrarEnCarpeta" />
                                                </Listeners>

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
                                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                        meta:resourceKey="GridFilters">
                                                    </ext:GridFilters>
                                                    <ext:CellEditing runat="server"
                                                        ClicksToEdit="2" />
                                                </Plugins>
                                                <BottomBar>
                                                    <ext:PagingToolbar runat="server"
                                                        ID="PagingToolBar"
                                                        meta:resourceKey="PagingToolBar"
                                                        HideRefresh="true"
                                                        OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:ComboBox runat="server"
                                                                Cls="comboGrid"
                                                                Width="60">
                                                                <Items>
                                                                    <ext:ListItem Text="10" />
                                                                    <ext:ListItem Text="20" />
                                                                    <ext:ListItem Text="30" />
                                                                    <ext:ListItem Text="40" />
                                                                    <ext:ListItem Text="50" />
                                                                </Items>
                                                                <SelectedItems>
                                                                    <ext:ListItem Value="20" />
                                                                </SelectedItems>
                                                                <Listeners>
                                                                    <%--<Select Fn="handlePageSizeSelect" />--%>
                                                                </Listeners>
                                                            </ext:ComboBox>

                                                        </Items>

                                                    </ext:PagingToolbar>
                                                </BottomBar>
                                            </ext:GridPanel>

                                            <ext:Panel runat="server"
                                                ID="pnFormulas"
                                                Cls="pnSecundario grdNoHeader"
                                                Region="Center"
                                                Flex="1"
                                                Hidden="true"
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
                                                        ID="Toolbar5"
                                                        Dock="Top"
                                                        Padding="0"
                                                        Cls="tlbGrid c-Grid">
                                                        <Items>
                                                            <ext:Button runat="server" ID="btnCloseShowVisorTreeP" IconCls="ico-hide-menu" Cls="btnSbCategory" Handler="ocultarPanel()" />
                                                            <ext:Label runat="server"
                                                                ID="lblTituloGrid"
                                                                Cls="HeaderLblVisor">
                                                            </ext:Label>
                                                            <ext:ToolbarFill />
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Items>
                                                    <ext:Container runat="server"
                                                        ID="cnPerfilesRol"
                                                        Cls="formGris cnFormulas"
                                                        Hidden="false"
                                                        PaddingSpec="8 16"
                                                        MaxWidth="960">
                                                        <Items>
                                                            <ext:Panel runat="server" Cls="ContenedorComboFormulas" Hidden="false" Height="150">
                                                                <DockedItems>
                                                                    <ext:Toolbar
                                                                        runat="server"
                                                                        ID="Toolbar1"
                                                                        Dock="Top"
                                                                        Height="30"
                                                                        Padding="0"
                                                                        Cls="cnFormulasToolbar">
                                                                        <Items>
                                                                            <ext:Label runat="server"
                                                                                ID="Label1"
                                                                                IconCls="ico-functionalityGrid"
                                                                                Cls="HeaderLblVisor">
                                                                            </ext:Label>
                                                                            <ext:Label runat="server"
                                                                                ID="Label2"
                                                                                Cls="HeaderLblVisor"
                                                                                Text="<%$ Resources:Comun, strFormula %>">
                                                                            </ext:Label>
                                                                            <ext:ToolbarFill />
                                                                        </Items>
                                                                    </ext:Toolbar>
                                                                </DockedItems>
                                                                <Items>
                                                                    <ext:Container runat="server" PaddingSpec="16 32">
                                                                        <Content>
                                                                            <div class="form-group" style="max-width: 85%;">
                                                                                <input type="text" class="form-control" id="txtFormulas" onkeyup="Validar()"  autocomplete="off">
                                                                            </div>
                                                                        </Content>
                                                                        <%--<Content>
                                                                            <div class="">
                                                                                <input type="text" class="" id="txtFormulas">
                                                                            </div>
                                                                        </Content>--%>
                                                                        <Items>
                                                                            <%--<ext:TextField runat="server" ID="txtFormulas"
                                                                                WidthSpec="100%"/>--%>


                                                                            <ext:Button runat="server"
                                                                                Text="<%$ Resources:Comun, strGuardar %>"
                                                                                Width="100"
                                                                                ID="btnGuardarFormula"
                                                                                IconCls="ico-addBtn"
                                                                                Cls="btn-accept btnAddFormulas" Handler="Guardar()" />
                                                                        </Items>
                                                                    </ext:Container>
                                                                </Items>
                                                            </ext:Panel>
                                                        </Items>
                                                    </ext:Container>
                                                    <ext:Container runat="server"
                                                        Cls="ctForm-resp-col3-rows"
                                                        PaddingSpec="8 16"
                                                        OverflowY="Auto"
                                                        MaxWidth="992">
                                                        <Items>
                                                            <ext:Container runat="server"
                                                                Cls="bordesFormulas"
                                                                Padding="8"
                                                                Height="265"
                                                                Layout="VBoxLayout">
                                                                <Items>
                                                                    <ext:Label runat="server"
                                                                        ID="Label3"
                                                                        WidthSpec="100%"
                                                                        Cls="txtNum marginB-Num"
                                                                        Text="1">
                                                                    </ext:Label>
                                                                    <ext:Label runat="server"
                                                                        ID="Label6"
                                                                        WidthSpec="100%"
                                                                        Width="310"
                                                                        Cls="txtCenter marginB"
                                                                        Text="<%$ Resources:Comun, strNumerico %>">
                                                                    </ext:Label>
                                                                    <ext:Label runat="server"
                                                                        ID="Label10"
                                                                        WidthSpec="100%"
                                                                        Cls="HeaderLblVisor txtCenter marginB"
                                                                        Text="1, 2, 3...">
                                                                    </ext:Label>
                                                                    <ext:Label runat="server"
                                                                        ID="Label17"
                                                                        WidthSpec="100%"
                                                                        Padding="8"
                                                                        Cls="txtDescription"
                                                                        Text="<%$ Resources:Comun, strFormulasDescripcionNumeros %>">
                                                                    </ext:Label>
                                                                </Items>
                                                            </ext:Container>
                                                            <ext:Container runat="server"
                                                                Cls="bordesFormulas"
                                                                Padding="8"
                                                                Height="265"
                                                                Layout="VBoxLayout">
                                                                <Items>
                                                                    <ext:Label runat="server"
                                                                        ID="Label4"
                                                                        WidthSpec="100%"
                                                                        Cls="txtNum marginB-Num"
                                                                        Text="2">
                                                                    </ext:Label>
                                                                    <ext:Label runat="server"
                                                                        ID="Label7"
                                                                        WidthSpec="100%"
                                                                        Cls="txtCenter marginB"
                                                                        Text="<%$ Resources:Comun, strSimbolo %>">
                                                                    </ext:Label>
                                                                    <ext:Label runat="server"
                                                                        ID="Label11"
                                                                        WidthSpec="100%"
                                                                        Cls="HeaderLblVisor txtCenter marginB"
                                                                        Text="+, -, *, /, ()...">
                                                                    </ext:Label>
                                                                    <ext:Label runat="server"
                                                                        ID="Label16"
                                                                        WidthSpec="100%"
                                                                        Padding="8"
                                                                        Cls="txtDescription"
                                                                        Text="<%$ Resources:Comun, strFormulasDescripcionOperadores %>">
                                                                    </ext:Label>
                                                                </Items>
                                                            </ext:Container>
                                                            <ext:Container runat="server"
                                                                Cls="bordesFormulas"
                                                                Padding="8"
                                                                Height="265"
                                                                Layout="VBoxLayout">
                                                                <Items>
                                                                    <ext:Label runat="server"
                                                                        ID="Label5"
                                                                        WidthSpec="100%"
                                                                        Cls="txtNum marginB-Num"
                                                                        Text="3">
                                                                    </ext:Label>
                                                                    <ext:Label runat="server"
                                                                        ID="Label8"
                                                                        WidthSpec="100%"
                                                                        Cls="txtCenter marginB"
                                                                        Text="<%$ Resources:Comun, strEnlace %>">
                                                                    </ext:Label>
                                                                    <ext:Label runat="server"
                                                                        ID="Label9"
                                                                        WidthSpec="100%"
                                                                        Cls="marginB"
                                                                        IconCls="btnColLaunch">
                                                                    </ext:Label>
                                                                    <ext:Label runat="server"
                                                                        ID="Label15"
                                                                        WidthSpec="100%"
                                                                        Padding="8"
                                                                        Cls="txtDescription"
                                                                        Text="<%$ Resources:Comun, strFormulasDescripcionAtributos %>">
                                                                    </ext:Label>
                                                                </Items>
                                                            </ext:Container>
                                                        </Items>
                                                    </ext:Container>
                                                    <ext:Label runat="server"
                                                        ID="LbParentesis"
                                                        Hidden="true"
                                                        IconCls="gen_noactivowHeight"
                                                        Cls="fRed bold"
                                                        MarginSpec="0 16"
                                                        Text="Parenthesis not closed">
                                                    </ext:Label>
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
