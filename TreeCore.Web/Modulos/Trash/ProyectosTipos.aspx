<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectosTipos.aspx.cs" Inherits="TreeCore.ModGlobal.ProyectosTipos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="ExtensionesPermitidas" runat="server" />

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
                RemoteSort="true"
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
                        IDProperty="ProyectoTipoID">
                        <Fields>
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="ProyectoTipo" />
                            <ext:ModelField Name="ExisteZona" Type="Boolean" />
                            <ext:ModelField Name="Alias" />
                            <ext:ModelField Name="NombreArchivo" />
                            <ext:ModelField Name="IsReporting" Type="Boolean" />
                            <ext:ModelField Name="SoloClientes" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoTipo" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

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
                                ID="txtProyectoTipo"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strTipoProyecto %>"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                ID="txtAlias"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strAlias %>"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                ID="txtKey"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strKeyTraduccion %>"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:FileUploadField
                                ID="FileUploadField2"
                                runat="server"
                                AllowBlank="true"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                FieldLabel="<%$ Resources:Comun, strImagenGrande %>"
                                ButtonText=""
                                Icon="PageAttach">
                                <Listeners>
                                    <Change Fn="ExntesionValida" />
                                </Listeners>
                            </ext:FileUploadField>
                            <ext:FileUploadField
                                ID="FileUploadField1"
                                runat="server"
                                AllowBlank="true"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                FieldLabel="<%$ Resources:Comun, strImagenPequena %>"
                                ButtonText=""
                                Icon="PageAttach">
                                <Listeners>
                                    <Change Fn="ExntesionValida" />
                                </Listeners>
                            </ext:FileUploadField>
                            <ext:Checkbox
                                ID="chkExisteZona"
                                runat="server"
                                Checked="false"
                                FieldLabel="<%$ Resources:Comun, strExisteZona %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:Checkbox
                                ID="chkIsReporting"
                                runat="server"
                                Checked="false"
                                FieldLabel="<%$ Resources:Comun, strIsReporting %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:Checkbox
                                runat="server"
                                ID="chkSoloClientes"
                                FieldLabel="<%$ Resources:Comun, strSoloClientes %>"
                                CausesValidation="true"
                                ValidationGroup="FORM"
                                Hidden="false" />
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

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport
                runat="server"
                ID="vwContenedor"
                Cls="vwContenedor"
                Layout="Anchor">
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
                        AnchorHorizontal="-100"
                        AnchorVertical="100%"
                        AriaRole="main">
                        <DockedItems>
                            <ext:Toolbar
                                runat="server"
                                ID="tlbBase"
                                Dock="Top"
                                Cls="tlbGrid">
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
                                        ID="btnActivar"
                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                        Cls="btnActivar"
                                        Handler="Activar();">
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
                                        Handler="ExportarDatos('ProyectosTipos', hdCliID.value, #{grid}, '');" />
                                </Items>
                            </ext:Toolbar>
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
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    DataIndex="ProyectoTipo"
                                    Header="<%$ Resources:Comun, strTipoProyecto %>"
                                    Width="300"
                                    Flex="1"
                                    ID="colProyectoTipo"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="ExisteZona"
                                    Header="<%$ Resources:Comun, strExisteZona %>"
                                    Width="150"
                                    Align="Center"
                                    ID="colExisteZona"
                                    runat="server">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    DataIndex="Alias"
                                    Header="<%$ Resources:Comun, strAlias %>"
                                    Width="300"
                                    ID="colAlias"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="IsReporting"
                                    Header="<%$ Resources:Comun, strIsReporting %>"
                                    Width="150"
                                    Align="Center"
                                    ID="colIsReporting"
                                    runat="server">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    DataIndex="NombreArchivo"
                                    Header="Icono Grande/Icono pequeño"
                                    Width="300"
                                    meta:resourceKey="columnNombreArchivo"
                                    ID="colNombreArchivo"
                                    runat="server" />
                                <ext:Column
                                    DataIndex="SoloClientes"
                                    Header="<%$ Resources:Comun, strSoloClientes %>"
                                    Width="150"
                                    Align="Center"
                                    ID="colSoloClientes"
                                    runat="server">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
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
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
