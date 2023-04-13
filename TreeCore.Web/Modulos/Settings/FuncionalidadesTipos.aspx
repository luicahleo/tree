<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FuncionalidadesTipos.aspx.cs" Inherits="TreeCore.ModGlobal.FuncionalidadesTipos" %>

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

            <ext:ResourceManager runat="server" ID="ResourceManagerTreeCore" DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store runat="server" ID="storeClientes" AutoLoad="true" OnReadData="storeClientes_Refresh" RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ClienteID">
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

            <ext:Store runat="server" ID="storePrincipal" RemotePaging="false" AutoLoad="false" OnReadData="storePrincipal_Refresh" RemoteSort="true" PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="FuncionalidadTipoID">
                        <Fields>
                            <ext:ModelField Name="FuncionalidadTipoID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Alias" />
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="Lectura" Type="Boolean" />
                            <ext:ModelField Name="Usuario" Type="Boolean" />
                            <ext:ModelField Name="Cliente" Type="Boolean" />
                            <ext:ModelField Name="Total" Type="Boolean" />
                            <ext:ModelField Name="Exportar" Type="Boolean" />
                            <ext:ModelField Name="GestionAdicional" Type="Boolean" />
                            <ext:ModelField Name="Otro" Type="Boolean" />
                            <ext:ModelField Name="Super" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="450"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextField ID="txtFuncionalidadTipo"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strTipoFuncionalidad %>"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />

                            <ext:TextField ID="txtAlias"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strAlias %>"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />

                            <ext:TextField ID="txtCodigo"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />

                            <ext:Checkbox ID="chkLectura"
                                runat="server"
                                BoxLabel="<%$ Resources:Comun, strLectura %>"
                                Checked="false"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                            </ext:Checkbox>

                            <ext:Checkbox ID="chkUsuario"
                                runat="server"
                                BoxLabel="<%$ Resources:Comun, strUsuario %>"
                                Checked="false"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                            </ext:Checkbox>

                            <ext:Checkbox ID="chkCliente"
                                runat="server"
                                BoxLabel="<%$ Resources:Comun, strCliente %>"
                                Checked="false"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                            </ext:Checkbox>

                            <ext:Checkbox ID="chkTotal"
                                runat="server"
                                BoxLabel="<%$ Resources:Comun, strTotal %>"
                                Checked="false"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                            </ext:Checkbox>

                            <ext:Checkbox ID="chkExportar"
                                runat="server"
                                BoxLabel="<%$ Resources:Comun, strExportar %>"
                                Checked="false"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                            </ext:Checkbox>

                            <ext:Checkbox ID="chkGestionAdicional"
                                runat="server"
                                BoxLabel="<%$ Resources:Comun, strGestionAdicional %>"
                                Checked="false"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                            </ext:Checkbox>

                            <ext:Checkbox ID="chkOtro"
                                runat="server"
                                BoxLabel="<%$ Resources:Comun, strOtro %>"
                                Checked="false"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                            </ext:Checkbox>

                            <ext:Checkbox ID="chkSuper"
                                runat="server"
                                BoxLabel="<%$ Resources:Comun, strSuper %>"
                                Checked="false"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                            </ext:Checkbox>
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
                        AnchorHorizontal="-100"
                        AnchorVertical="100%"
                        AriaRole="main">
                        <DockedItems>
                            <ext:Toolbar runat="server"
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
                                        Cls="btn-Activar"
                                        Handler="Activar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        Cls="btn-Eliminar"
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
                                        Handler="ExportarDatos('FuncionalidadesTipos', hdCliID.value, #{grid}, '');" />
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server"
                                ID="tlbClientes"
                                Dock="Top">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbClientes"
                                        StoreID="storeClientes"
                                        DisplayField="Cliente"
                                        ValueField="ClienteID"
                                        Cls="comboGrid pos-boxGrid"
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
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column runat="server"
                                    ID="Activo"
                                    DataIndex="Activo"
                                    Align="Center"
                                    Cls="col-activo"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column DataIndex="Nombre"
                                    ID="Nombre"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strNombre %>"
                                    Width="250"
                                    Flex="1" />
                                <ext:Column DataIndex="Alias"
                                    ID="Alias"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strAlias %>"
                                    Width="250"
                                    Flex="1" />
                                    <ext:Column DataIndex="Codigo"
                                    ID="Codigo"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strCodigo %>"
                                    Width="250"
                                    Flex="1" />
                                <ext:Column runat="server"
                                    ID="Lectura"
                                    DataIndex="Lectura"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, strLectura %>"
                                    Text="<%$ Resources:Comun, strLectura %>"                                    
                                    Width="100">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="Usuario"
                                    DataIndex="Usuario"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, strUsuario %>"
                                    Text="<%$ Resources:Comun, strUsuario %>"
                                    Width="100">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="Cliente"
                                    DataIndex="Cliente"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, strCliente %>"
                                    Text="<%$ Resources:Comun, strCliente %>"
                                    Width="100">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="Total"
                                    DataIndex="Total"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, strTotal %>"
                                    Text="<%$ Resources:Comun, strTotal %>"
                                    Width="100">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="Exportar"
                                    DataIndex="Exportar"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, strExportar %>"
                                    Text="<%$ Resources:Comun, strExportar %>"
                                    Width="100">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="GestionAdicional"
                                    DataIndex="GestionAdicional"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, strGestionAdicional %>"
                                    Text="<%$ Resources:Comun, strGestionAdicional %>"
                                    Width="100">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="Otro"
                                    DataIndex="Otro"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, strOtro %>"
                                    Text="<%$ Resources:Comun, strOtro %>"
                                    Width="100">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="Super"
                                    DataIndex="Super"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, strSuper %>"
                                    Text="<%$ Resources:Comun, strSuper %>"
                                    Width="100">
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
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFilters"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                        <BottomBar>
                            <ext:PagingToolbar runat="server"
                                ID="PagingToolBar"
                                meta:resourceKey="PagingToolBar"
                                StoreID="storePrincipal"
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
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
