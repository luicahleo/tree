<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlquileresTiposContrataciones.aspx.cs" Inherits="TreeCore.ModGlobal.AlquileresTiposContrataciones" %>

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

            <ext:Hidden ID="hdCliID" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager
                runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store runat="server"
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
                    <ext:Model runat="server" IDProperty="Code">
                        <Fields>
                            <ext:ModelField Name="Code" />
                            <ext:ModelField Name="Name" />
                            <ext:ModelField Name="Description" />
                            <ext:ModelField Name="Active" Type="Boolean" />
                            <ext:ModelField Name="Default" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Name" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="350"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextField
                                ID="txtCodigo"
                                runat="server"
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                Text=""
                                WidthSpec="100%"
                                MaxLength="150"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                Cls="ico-exclamacion-10px-grey"
                                CausesValidation="true">
                                <Listeners>
                                    <Change Fn="anadirClsNoValido" />
                                    <FocusLeave Fn="anadirClsNoValido" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField
                                ID="txtNombre"
                                runat="server"
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                Text=""
                                WidthSpec="100%"
                                MaxLength="150"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                Cls="ico-exclamacion-10px-grey"
                                CausesValidation="true">
                                <Listeners>
                                    <Change Fn="anadirClsNoValido" />
                                    <FocusLeave Fn="anadirClsNoValido" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextArea
                                ID="txtDescripcion"
                                runat="server"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                FieldLabel="<%$ Resources:Comun, strDescripcion %>"
                                Text=""
                                MaxLength="240"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                Cls="ico-exclamacion-10px-grey"
                                CausesValidation="true"
                                meta:resourceKey="txtDescripcion">
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
                        meta:resourceKey="btnCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGestion}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button
                        runat="server"
                        ID="btnGuardar"
                        meta:resourceKey="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
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

            <ext:Viewport runat="server"
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
                                        AriaLabel="Añadir"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Handler="AgregarEditar();">
                                    </ext:Button>
                                    <ext:Button
                                        runat="server"
                                        ID="btnEditar"
                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                        meta:resourceKey="btnEditar"
                                        Cls="btnEditar"
                                        Handler="MostrarEditar();">
                                    </ext:Button>
                                    <ext:Button
                                        runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        meta:resourceKey="btnEliminar"
                                        Cls="btnEliminar"
                                        Handler="Eliminar();" />
                                    <ext:Button
                                        runat="server"
                                        ID="btnActivar"
                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                        meta:resourceKey="btnActivar"
                                        Cls="btnActivar"
                                        Handler="Activar();">
                                    </ext:Button>
                                    <ext:Button
                                        runat="server"
                                        ID="btnDefecto"
                                        ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                        meta:resourceKey="btnDefecto"
                                        Cls="btnDefecto"
                                        Handler="Defecto();" />
                                    <ext:Button
                                        runat="server"
                                        ID="btnRefrescar"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        meta:resourceKey="btnRefrescar"
                                        Cls="btnRefrescar"
                                        Handler="Refrescar();" />
                                    <ext:Button
                                        runat="server"
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        meta:resourceKey="btnDescargar"
                                        Cls="btnDescargar"
                                        Handler="ExportarDatos('AlquileresTiposContrataciones', hdCliID.value, #{grid}, '');" />
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column
                                    runat="server"
                                    ID="colActivo"
                                    DataIndex="Active"
                                    Align="Center"
                                    Cls="col-activo"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                    meta:resourceKey="colActivo"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    runat="server"
                                    ID="colDefecto"
                                    DataIndex="Default"
                                    Align="Center"
                                    Cls="col-default"
                                    ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                    meta:resourceKey="colDefecto"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    runat="server"
                                    ID="Codigo"
                                    meta:resourceKey="colCodigo"
                                    DataIndex="Code"
                                    Text="<%$ Resources:Comun, strCodigo %>"
                                    Width="150" />
                                <ext:Column
                                    DataIndex="Name"
                                    Width="150"
                                    Text="<%$ Resources:Comun, strNombre %>"
                                    ID="Name"
                                    runat="server" />
                                <ext:Column
                                    runat="server"
                                    ID="Description"
                                    meta:resourceKey="colDescricpcion"
                                    DataIndex="Description"
                                    Text="<%$ Resources:Comun, strDescripcion %>"
                                    Width="150"
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
