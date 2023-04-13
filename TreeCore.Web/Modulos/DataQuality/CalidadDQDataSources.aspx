<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalidadDQDataSources.aspx.cs" Inherits="TreeCore.ModCalidad.pages.CalidadDQDataSources" %>

<%@ Register Src="~/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Data Sources</title>
    <link href="css/CalidadDQDataSources.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <div>

            <ext:Hidden ID="hdCliID" runat="server" />

            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                </Listeners>

            </ext:ResourceManager>

            <!--<script src="../../JS/common.js"></script>-->
            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>


            <ext:Store runat="server"
                ID="storePrincipal"
                RemotePaging="false"
                AutoLoad="true"
                OnReadData="storePrincipal_Refresh"
                RemoteSort="true"
                PageSize="20"
                shearchBox="cmpFiltro_txtSearch"
                listNotPredictive="DQTablaPaginaID">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                    <DataChanged Fn="BuscadorPredictivo" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="DQTablaPaginaID">
                        <Fields>
                            <ext:ModelField Name="DQTablaPaginaID" Type="Int" />
                            <ext:ModelField Name="Alias" />
                            <ext:ModelField Name="ClaveRecurso" />
                            <ext:ModelField Name="NombreTabla" />
                            <ext:ModelField Name="ClaveRecursoTabla" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Alias" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeTablas"
                runat="server"
                AutoLoad="false"
                OnReadData="storeTablas_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="TablaModeloDatosID" runat="server">
                        <Fields>
                            <ext:ModelField Name="TablaModeloDatosID" Type="Int" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="NombreTabla" />
                            <ext:ModelField Name="NombreTablaTraducido" />
                            <ext:ModelField Name="ClaveRecurso" />
                            <ext:ModelField Name="Controlador" />
                            <ext:ModelField Name="Indice" />
                            <ext:ModelField Name="ModuloID" Type="Int" />

                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <%--FIN  STORES --%>


            <ext:Window
                runat="server"
                ID="winGestion"
                Cls="win1Col winGestion"
                meta:resourcekey="winGestion"
                Title=""
                Width="450"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar1" Cls="greytb" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill />
                            <ext:Button runat="server"
                                ID="btnCancelar"
                                Cls="btn-secondary"
                                Text="Cancel"
                                Focusable="false"
                                PressedCls="none"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="#{winGestion}.hide();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button
                                runat="server"
                                ID="btnGuardar"
                                Disabled="true"
                                meta:resourceKey="btnGuardar"
                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                Cls="btn-accept">
                                <Listeners>
                                    <Click Handler="winGestionBotonGuardar();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        Cls="formGris"
                        OverflowY="Auto"
                        Layout="VBoxLayout">

                        <Items>
                            <ext:Container runat="server"
                                ID="ctForm"
                                Cls="form1Col">
                                <Items>
                                    <ext:TextField
                                        runat="server"
                                        ID="txtAlias"
                                        LabelAlign="Top"
                                        meta:resourceKey="txtAlias"
                                        FieldLabel="<%$ Resources:Comun, strAlias %>"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        AllowBlank="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="false" />
                                    <ext:ComboBox runat="server"
                                        ID="cmbTablas"
                                        StoreID="storeTablas"
                                        DisplayField="NombreTablaTraducido"
                                        ValueField="TablaModeloDatosID"
                                        QueryMode="Local"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        FieldLabel="<%$ Resources:Comun, strTabla %>">
                                        <Listeners>
                                            <Select Fn="SeleccionarTablas" />
                                            <TriggerClick Fn="RecargarTablas" />
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
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoGestion(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Viewport runat="server" ID="MainVwP" OverflowY="auto" Cls="vwContenedor" Layout="FitLayout">
                <Listeners>
                </Listeners>
                <Items>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls=""
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="HBoxLayout"
                                Cls="visorInsidePn">
                                <LayoutConfig>
                                    <ext:HBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <DockedItems>
                                    <ext:Toolbar runat="server" ID="tbNavNAside" Dock="Top" Cls="tbGrey" Hidden="true" MinHeight="36">
                                        <Items>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkTable"
                                                Hidden="false"
                                                Cls="lnk-navView lnk-noLine navActivo"
                                                Text="Table">
                                                <Listeners>
                                                    <Click Handler="NavegacionTabs(this)"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkUnactive1"
                                                Hidden="false"
                                                Cls="lnk-navView lnk-noLine"
                                                Text="Unactive">
                                                <Listeners>
                                                    <Click Handler="NavegacionTabs(this)"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkUnactive2"
                                                Hidden="false"
                                                Cls="lnk-navView lnk-noLine"
                                                Text="Unactive">
                                                <Listeners>
                                                    <Click Handler="NavegacionTabs(this)"></Click>
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Button runat="server" ID="btnCollapseAsRClosed" Cls="btn-trans btnCollapseAsRClosedv2" Handler="hidePnLite();" Disabled="false" Hidden="true"></ext:Button>
                                        </Items>
                                    </ext:Toolbar>

                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server" ID="ctMain1" Flex="2" Layout="FitLayout" Cls="col colCt1" Hidden="false">
                                        <Items>
                                            <ext:GridPanel
                                                runat="server"
                                                ID="grid"
                                                meta:resourceKey="grid"
                                                SelectionMemory="false"
                                                Cls="gridPanel"
                                                StoreID="storePrincipal"
                                                Title="Titulo"
                                                Header="false"
                                                EnableColumnHide="false"
                                                AnchorHorizontal="100%"
                                                AnchorVertical="100%"
                                                AriaRole="main">
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
                                                                meta:resourceKey="btnAnadir"
                                                                Cls="btnAnadir"
                                                                Hidden="false"
                                                                Enabled="true"
                                                                ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                                Handler="AgregarEditar();">
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnEditar"
                                                                meta:resourceKey="btnEditar"
                                                                Cls="btnEditar"
                                                                Disabled="true"
                                                                AriaLabel="<%$ Resources:Comun, jsEditar %>"
                                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                Handler="MostrarEditar();">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnEliminar"
                                                                ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                meta:resourceKey="btnEliminar"
                                                                Cls="btn-Eliminar"
                                                                Disabled="true"
                                                                Handler="Eliminar();" />
                                                            <ext:Button runat="server"
                                                                ID="btnRefrescar"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                meta:resourceKey="btnRefrescar"
                                                                Cls="btnRefrescar"
                                                                Handler="Refrescar();" />
                                                            <ext:Button runat="server"
                                                                ID="btnDescargar"
                                                                meta:resourceKey="btnDescargar"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Cls="btnDescargar"
                                                                Handler="ExportarDatosSinCliente('CalidadDQDataSources', #{grid}, '', '', '', '');" />
                                                            <ext:Button runat="server"
                                                                ID="btnActivar"
                                                                Disabled="true"
                                                                ToolTip=""
                                                                meta:resourceKey="btnActivar"
                                                                Cls="btnActivar"
                                                                Handler="ajaxActivar()" />
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top">
                                                        <Content>

                                                            <local:toolbarFiltros
                                                                ID="cmpFiltro"
                                                                runat="server"
                                                                Stores="storePrincipal"
                                                                MostrarComboFecha="false"
                                                                FechaDefecto="Dia"
                                                                Grid="grid"
                                                                MostrarBusqueda="false" />

                                                        </Content>
                                                    </ext:Container>
                                                </DockedItems>

                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:Column
                                                            runat="server"
                                                            ID="colActivo"
                                                            DataIndex="Activo"
                                                            Align="Center"
                                                            Cls="col-activo"
                                                            ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                            meta:resourceKey="colActivo"
                                                            Width="65">
                                                            <Renderer Fn="DefectoRender" />
                                                        </ext:Column>
                                                        <ext:Column runat="server"
                                                            MinWidth="120"
                                                            Text="<%$ Resources:Comun, strAlias %>"
                                                            DataIndex="Alias"
                                                            Flex="1"
                                                            ID="ColAlias" />
                                                        <ext:Column runat="server"
                                                            MinWidth="120"
                                                            Text="<%$ Resources:Comun, strTabla %>"
                                                            DataIndex="ClaveRecursoTabla"
                                                            Flex="1"
                                                            ID="ColNombreTabla" />
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
                                                    <ext:CellEditing
                                                        runat="server"
                                                        ClicksToEdit="2" />
                                                </Plugins>
                                                <BottomBar>
                                                    <ext:PagingToolbar
                                                        runat="server"
                                                        ID="PagingToolBar"
                                                        meta:resourceKey="PagingToolBar"
                                                        StoreID="storePrincipal"
                                                        DisplayInfo="true"
                                                        HideRefresh="true">
                                                        <Items>
                                                            <ext:ComboBox
                                                                runat="server"
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
