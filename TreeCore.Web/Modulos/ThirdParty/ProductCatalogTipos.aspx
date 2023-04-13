<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductCatalogTipos.aspx.cs" Inherits="TreeCore.ModGlobal.ProductCatalogTipos" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarfiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdStore" runat="server">
                <Listeners>
                    <Change Fn="CargarStore" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden runat="server" ID="hdEditando" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager
                runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>
            <ext:Store
                runat="server"
                ID="storePrincipal"
                RemotePaging="false"
                AutoLoad="true"
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
                    <ext:Model
                        runat="server"
                        IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Active" Type="Boolean" />
                            <ext:ModelField Name="Default" Type="Boolean" />
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="Description" />
                            <ext:ModelField Name="IsOffering" />
                            <ext:ModelField Name="IsPurchasing" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter
                        Property="Name"
                        Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--INICIO  WINDOWS --%>

            <ext:Window
                runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="450"
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
                                ID="txtCodigo"
                                FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                                <Listeners>
                                    <Change Fn="anadirClsNoValido" />
                                    <FocusLeave Fn="anadirClsNoValido" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField
                                runat="server"
                                ID="txtNombre"
                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                                <Listeners>
                                    <Change Fn="anadirClsNoValido" />
                                    <FocusLeave Fn="anadirClsNoValido" />
                                </Listeners>
                            </ext:TextField>
                            <ext:RadioGroup
                                ID="RGTipo"
                                runat="server"
                                GroupName="TipoBool"
                                WidthSpec="100%"
                                MarginSpec="20 0 10"
                                Cls="x-check-group-alt chkLabel rbGroup">
                                <Items>
                                    <ext:Radio runat="server"
                                        BoxLabel="isOffering"
                                        InputValue="1"
                                        WidthSpec="100%"
                                        Checked="true"
                                        ID="isOffering"
                                        Cls="noPaddingRB-L" />
                                    <ext:Radio runat="server"
                                        BoxLabel="isPurchasing"
                                        WidthSpec="100%"
                                        InputValue="2"
                                        ID="isPurchasing" />
                                </Items>
                                <Listeners>
                                    <Change Fn="Seleccion" />
                                </Listeners>
                            </ext:RadioGroup>
                            <ext:TextArea
                                runat="server"
                                ID="txtDescripcion"
                                FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                                <Listeners>
                                    <Change Fn="anadirClsNoValido" />
                                    <FocusLeave Fn="anadirClsNoValido" />
                                </Listeners>
                            </ext:TextArea>
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
                        Cls="btn-secondary-winForm"
                        Focusable="false">
                        <Listeners>
                            <Click Handler="#{winGestion}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button
                        runat="server"
                        ID="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        Disabled="true"
                        Focusable="false"
                        Cls="btn-ppal-winForm">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--VENTANAS EMERGENTES --%>

            <ext:Viewport runat="server" ID="vwResp" OverflowY="auto" Layout="FitLayout">
                <Listeners>
                </Listeners>

                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <Content>
                            <ext:Button runat="server" ID="btnCollapseAsRClosed" Cls="btn-trans btnCollapseAsRClosedv3" Handler="OcultarPanelLateral();" Disabled="false" Hidden="true"></ext:Button>
                        </Content>

                        <Items>

                            <%-- PANEL CENTRAL--%>

                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                MarginSpec="0 20 20 20"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain">
                                <Items>
                                    <ext:GridPanel
                                        runat="server"
                                        ID="grid"
                                        meta:resourceKey="grid"
                                        SelectionMemory="false"
                                        Cls="gridPanel"
                                        StoreID="storePrincipal"
                                        Title="etiqgridTitle"
                                        Header="false"
                                        EnableColumnHide="false"
                                        AnchorHorizontal="100%"
                                        AnchorVertical="100%"
                                        AriaRole="main">
                                        <Listeners>
                                            <AfterRender Handler="GridColHandlerDinamicoV2(this);"></AfterRender>
                                            <Resize Handler="GridColHandlerDinamicoV2(this);"></Resize>
                                        </Listeners>
                                        <DockedItems>
                                            <ext:Toolbar
                                                runat="server"
                                                ID="tlbBase"
                                                Dock="Top"
                                                Cls="tlbGrid">
                                                <Items>
                                                    <ext:Button
                                                        runat="server"
                                                        ID="btnAnadir"
                                                        Cls="btnAnadir"
                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                        Handler="AgregarEditar();">
                                                    </ext:Button>
                                                    <ext:Button
                                                        runat="server"
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
                                                        ID="btnRefrescar"
                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                        Cls="btnRefrescar"
                                                        Handler="Refrescar();" />
                                                    <ext:Button runat="server"
                                                        ID="btnDescargar"
                                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                        Cls="btnDescargar"
                                                        Handler="ExportarDatos('ProductCatalogTipos', hdCliID.value, #{grid}, '');" />
                                                    <ext:Button runat="server"
                                                        ID="btnActivar"
                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                        Cls="btnActivar"
                                                        Handler="Activar();">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnDefecto"
                                                        ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                                        Cls="btnDefecto">
                                                        <Listeners>
                                                            <Click Fn="Defecto" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server"
                                                ID="tlbClientes"
                                                Dock="Top">
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colActivo"
                                                    DataIndex="Active"
                                                    Align="Center"
                                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                    Cls="col-activo"
                                                    MinWidth="50"
                                                    MaxWidth="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colDefecto"
                                                    DataIndex="Default"
                                                    Align="Center"
                                                    ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                                    Cls="col-default"
                                                    MinWidth="50"
                                                    MaxWidth="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colOffering"
                                                    DataIndex="IsOffering"
                                                    Align="Center"
                                                    ToolTip="Offering"
                                                    Cls="col-offering"
                                                    MinWidth="50"
                                                    MaxWidth="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column
                                                    runat="server"
                                                    ID="colPurchasing"
                                                    DataIndex="IsPurchasing"
                                                    Align="Center"
                                                    ToolTip="Purchasing"
                                                    Cls="col-purchasing"
                                                    MinWidth="50"
                                                    MaxWidth="50">
                                                    <Renderer Fn="DefectoRender" />
                                                </ext:Column>
                                                <ext:Column
                                                    runat="server"
                                                    ID="Codigo"
                                                    meta:resourceKey="colCodigo"
                                                    DataIndex="Code"
                                                    Text="<%$ Resources:Comun, strCodigo %>"
                                                    MinWidth="150"
                                                    Flex="1" />
                                                <ext:Column
                                                    runat="server"
                                                    ID="Nombre"
                                                    meta:resourceKey="colNombre"
                                                    DataIndex="Name"
                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                    MinWidth="150"
                                                    Flex="1" />
                                                <ext:Column
                                                    runat="server"
                                                    ID="Description"
                                                    meta:resourceKey="colDescricpcion"
                                                    DataIndex="Description"
                                                    Text="<%$ Resources:Comun, strDescripcion %>"
                                                    MinWidth="150"
                                                    Flex="1" />
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel
                                                runat="server"
                                                ID="GridRowSelect"
                                                Mode="Single">
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelect" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Plugins>
                                            <ext:GridFilters
                                                runat="server"
                                                ID="gridFilters"
                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                meta:resourceKey="GridFilters" />
                                            <ext:CellEditing runat="server"
                                                ClicksToEdit="2" />
                                        </Plugins>
                                        <BottomBar>
                                            <ext:PagingToolbar
                                                runat="server"
                                                ID="PagingToolBar"
                                                meta:resourceKey="PagingToolBar"
                                                StoreID="storePrincipal"
                                                OverflowHandler="Scroller"
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
                                                            <ext:ListItem Text="50" />
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
                            <ext:Panel runat="server" ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                Width="380"
                                CollapseMode="Header"
                                Collapsible="true"
                                Layout="AnchorLayout"
                                Header="false" Border="false" Hidden="false">
                                <Listeners>
                                </Listeners>
                                <Items>
                                    <%-- PANEL MORE INFO--%>
                                    <ext:Panel runat="server" ID="pnMoreInfo" Hidden="true" Cls="tbGrey grdIntoAside" AnchorVertical="100%" AnchorHorizontal="100%" OverflowY="Auto">

                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="Toolbar2" Cls="tbGrey" Dock="Top" Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label1" runat="server" IconCls="ico-head-info" Cls="lblHeadAside " Text="MORE INFO" MarginSpec="36 15 30 15"></ext:Label>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Content>
                                            <div>
                                                <table class="tmpCol-table" id="tablaInfoElementos">
                                                    <tbody id="bodyTablaInfoElementos">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </Content>
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
