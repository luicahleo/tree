<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginSettings.aspx.cs" Inherits="TreeCore.ModGlobal.LoginSettings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>

    <script type="text/javascript" src="/Scripts/jquery-3.5.1.js"></script>
    <link href="../../CSS/amsify.suggestags.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/jquery.amsify.suggestags.js"></script>
    <script type="text/javascript" src="/Scripts/slick.min.js"></script>

    <form id="form1" runat="server">
        <div>
            <%--INICIO  HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdGrupoAccesoWeb" runat="server" />

            <%--FIN  HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store runat="server"
                ID="storeRoles"
                AutoLoad="true"
                OnReadData="storeRoles_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="RolID">
                        <Fields>
                            <ext:ModelField Name="RolID" Type="Int" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Nombre" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Codigo" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeRolesAsignados"
                AutoLoad="false"
                OnReadData="storeRolesAsignados_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="RolID">
                        <Fields>
                            <ext:ModelField Name="RolID" Type="Int" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Nombre" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Codigo" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storePrincipal"
                RemotePaging="false"
                AutoLoad="false"
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
                    <ext:Model runat="server" IDProperty="GrupoAccesoWebRolID">
                        <Fields>
                            <ext:ModelField Name="GrupoAccesoWebRolID" Type="Int" />
                            <ext:ModelField Name="GrupoAcceso" Type="String" />
                            <ext:ModelField Name="CodigoRol" Type="String" />
                            <ext:ModelField Name="URL" Type="String" />
                            <ext:ModelField Name="ActivoRol" Type="Boolean" />
                            <ext:ModelField Name="ActivoGrupo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="GrupoAcceso" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeToolConexiones"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeToolConexiones_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaLDAP" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ToolConexionID">
                        <Fields>
                            <ext:ModelField Name="ToolConexionID" Type="Int" />
                            <ext:ModelField Name="Servidor" Type="String" />
                            <ext:ModelField Name="Usuario" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ToolConexionID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="450"
                Height="420"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        BodyStyle="padding:10px;"
                        Height="310"
                        Cls="unaColumna"
                        Border="false">
                        <Items>
                            <ext:TextField ID="txtGrupoAcceso"
                                runat="server"
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strGruposAccesosWeb %>"
                                Text=""
                                MaxLength="100"
                                Cls="margenBottom"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:Label runat="server"
                                ID="lblRol"
                                Height="27"
                                Cls="lblHeadForm lblFormRoles"
                                Text="<%$ Resources:Comun, strRoles %>">
                            </ext:Label>
                            <ext:Container runat="server"
                                ID="cnRoles"
                                Cls="txtUsers"
                                Height="70">
                                <Content>
                                    <div class="form-group">
                                        <input type="text"
                                            class="form-control input-control-form"
                                            name="roles">
                                    </div>
                                </Content>
                            </ext:Container>
                            <ext:TextField ID="txtURL"
                                runat="server"
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strURL %>"
                                Text=""
                                MaxLength="500"
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
                    <ext:Button runat="server"
                        ID="btnCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGestion}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <ext:Window runat="server"
                ID="winGestionLDAP"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="450"
                Height="420"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formLDAP"
                        BodyStyle="padding:10px;"
                        Height="310"
                        Cls="unaColumna"
                        Border="false">
                        <Items>
                            <ext:TextField runat="server"
                                ID="txtServer"
                                FieldLabel="<%$ Resources:Comun, strServidor %>"
                                AllowBlank="false"
                                LabelAlign="Top">
                                <Listeners>
                                    <Focus Fn="anadirClsNoValido" />
                                    <FocusLeave Fn="anadirClsNoValido" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField runat="server"
                                ID="txtUsuario"
                                FieldLabel="<%$ Resources:Comun, strUsuario %>"
                                AllowBlank="false"
                                LabelAlign="Top">
                                <Listeners>
                                    <Focus Fn="anadirClsNoValido" />
                                    <FocusLeave Fn="anadirClsNoValido" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField
                                LabelAlign="Top"
                                ID="txtPasswordField"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strContraseña %>"
                                InputType="Password"
                                AllowBlank="false"
                                Regex=""
                                RegexText="">
                                <RightButtons>
                                    <ext:Button runat="server"
                                        ID="btnPassword"
                                        Hidden="true"
                                        IconCls="btnFiltroNegativo16"
                                        AllowDepress="true"
                                        EnableToggle="true">
                                        <Listeners>
                                            <Toggle Handler="this.up('textfield').passwordMask.setMode(pressed ? 'showall' : 'hideall'); this.setTooltip((pressed ? 'Hide' : 'Show') + ' password');" />
                                        </Listeners>
                                    </ext:Button>
                                </RightButtons>
                                <Listeners>
                                    <Focus Fn="anadirClsNoValido" />
                                    <FocusLeave Fn="anadirClsNoValido" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField
                                LabelAlign="Top"
                                ID="txtPasswordConfirm"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strConfirmarContraseña %>"
                                MsgTarget="Side"
                                AllowBlank="false"
                                LabelCls="ConfirmLbl"
                                InputType="Password"
                                Regex=""
                                RegexText="">
                                <RightButtons>
                                    <ext:Button runat="server"
                                        ID="PassMode"
                                        IconCls="btnFiltroNegativo16"
                                        AllowDepress="true"
                                        EnableToggle="true"
                                        Hidden="true">
                                        <Listeners>
                                            <Toggle Handler="this.up('textfield').passwordMask.setMode(pressed ? 'showall' : 'hideall'); this.setTooltip((pressed ? 'Hide' : 'Show') + ' password');" />
                                        </Listeners>
                                    </ext:Button>
                                </RightButtons>
                                <Listeners>
                                    <Focus Fn="anadirClsNoValido" />
                                    <FocusLeave Fn="anadirClsNoValido" />
                                </Listeners>
                            </ext:TextField>
                        </Items>
                        <Listeners>
                            <AfterRender Fn="addlistenerValidacion" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnCancelarLDAP"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGestionLDAP}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardarLDAP"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardarLDAP();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="vwMain"
                Layout="AnchorLayout"
                AnchorVertical="99%"
                AnchorHorizontal="100%"
                Scrollable="Vertical"
                OverflowY="Scroll"
                OverflowX="Hidden">
                <Items>
                    <ext:Container runat="server"
                        ID="titleCont"
                        AnchorVertical="10%"
                        AnchorHorizontal="100%"
                        Padding="30">
                        <Items>
                            <ext:Label runat="server"
                                ID="lblSettings"
                                Text="Login Settings"
                                Cls="BigTlt TituloCabecera">
                            </ext:Label>
                        </Items>
                    </ext:Container>
                    <ext:Container runat="server"
                        Layout="ColumnLayout"
                        AnchorVertical="75%"
                        AnchorHorizontal="99%"
                        PaddingSpec="10 40">
                        <Items>
                            <ext:Panel runat="server"
                                HeightSpec="100%"
                                Layout="VBoxLayout"
                                ColumnWidth="0.7">
                                <Items>
                                    <ext:Container runat="server"
                                        WidthSpec="100%"
                                        Cls="ctForm-resp ctForm-resp-col2 tbGrey">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnToggleFactor"
                                                Width="150"
                                                Text="<%$ Resources:Comun, strDobleFacto %>"
                                                EnableToggle="true"
                                                Pressed="false"
                                                TextAlign="Right"
                                                Focusable="false"
                                                OverCls="none"
                                                PressedCls="btnActivarDesactivarV3Pressed"
                                                Cls="btnActivarDesactivarV3 tbGrey">
                                                <Listeners>
                                                    <Click Fn="cambiarFactor" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnToggleLDAP"
                                                Width="150"
                                                Text="LDAP"
                                                EnableToggle="true"
                                                Pressed="true"
                                                TextAlign="Center"
                                                Focusable="false"
                                                OverCls="none"
                                                PressedCls="btnActivarDesactivarV3Pressed"
                                                Cls="btnActivarDesactivarV3 tbGrey">
                                                <Listeners>
                                                    <Click Fn="cambiarLDAP" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Panel>

                            <ext:GridPanel
                                runat="server"
                                ID="gridLDAP"
                                SelectionMemory="false"
                                Cls="gridPanel grdNoHeader "
                                StoreID="storeToolConexiones"
                                Title="etiqgridTitle"
                                Header="false"
                                WidthSpec="100%"
                                EnableColumnHide="false"
                                ColumnWidth="0.7"
                                Height="400px"
                                OverflowX="Hidden"
                                OverflowY="Auto">
                                <DockedItems>
                                    <ext:Toolbar runat="server"
                                        ID="Toolbar1"
                                        Dock="Top"
                                        Cls="tlbGrid">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnAnadirLDAP"
                                                Cls="btnAnadir"
                                                AriaLabel="Añadir"
                                                Disabled="true"
                                                ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                Handler="AgregarEditarLDAP();">
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnEditarLDAP"
                                                Disabled="true"
                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                Cls="btnEditar"
                                                Handler="MostrarEditarLDAP();">
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnEliminarLDAP"
                                                ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                Cls="btnEliminar"
                                                Disabled="true"
                                                Handler="EliminarLDAP();" />
                                            <ext:Button runat="server"
                                                ID="btnRefrescarLDAP"
                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                Cls="btnRefrescar"
                                                Disabled="true"
                                                Handler="RefrescarLDAP();" />
                                            <ext:Button runat="server"
                                                ID="btnDescargarLDAP"
                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                Cls="btnDescargar"
                                                Disabled="true"
                                                Handler="ExportarDatos('LoginSettings', hdCliID.value, #{gridLDAP}, '', '', -1);" />
                                            <ext:Label runat="server"
                                                ID="lbTgLoginCreated"
                                                Cls="lblBtnActivo"
                                                PaddingSpec="0 0 25 0"
                                                Disabled="true"
                                                Text="Automatic creating users" />
                                            <ext:Button
                                                runat="server"
                                                ID="btnTgLoginCreated"
                                                Text=""
                                                EnableToggle="true"
                                                Pressed="false"
                                                Disabled="true"
                                                Focusable="false"
                                                Cls="btn-toggleGrid"
                                                Width="41"
                                                AriaLabel="">
                                                <Listeners>
                                                    <Click Fn="cambiarUsuario" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <ColumnModel runat="server">
                                    <Columns>
                                        <ext:Column runat="server"
                                            ID="colServidor"
                                            DataIndex="Servidor"
                                            Text="<%$ Resources:Comun, strServidor %>"
                                            Width="80"
                                            Flex="1"
                                            Align="Center" />
                                        <ext:Column runat="server"
                                            ID="colUsuario"
                                            DataIndex="Usuario"
                                            Text="<%$ Resources:Comun, strUsuario %>"
                                            Width="250"
                                            Flex="1" />
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel runat="server"
                                        ID="GridRowSelectLDAP"
                                        Mode="Single">
                                        <Listeners>
                                            <Select Fn="Grid_RowSelectLDAP" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <Plugins>
                                    <ext:GridFilters runat="server"
                                        ID="gridFiltersLDAP"
                                        MenuFilterText="<%$ Resources:Comun, strFiltros %>" />
                                    <ext:CellEditing runat="server"
                                        ClicksToEdit="2" />
                                </Plugins>
                                <BottomBar>
                                    <ext:PagingToolbar runat="server"
                                        ID="PagingToolBar1"
                                        StoreID="storeToolConexiones"
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
                                                    <Select Fn="handlePageSizeSelectLDAP" />
                                                </Listeners>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:PagingToolbar>
                                </BottomBar>
                            </ext:GridPanel>

                            <ext:Panel runat="server"
                                HeightSpec="100%"
                                Layout="VBoxLayout"
                                ColumnWidth="0.7">
                                <Items>
                                    <ext:Container runat="server"
                                        WidthSpec="100%"
                                        Cls="ctForm-resp ctForm-resp-col2 tbGrey">
                                        <Items>
                                            <ext:Button runat="server"
                                                ID="btnToggleMultihoming"
                                                Width="150"
                                                EnableToggle="true"
                                                Text="Multihoming"
                                                Pressed="false"
                                                TextAlign="Right"
                                                Focusable="false"
                                                OverCls="none"
                                                PressedCls="btnActivarDesactivarV3Pressed"
                                                Cls="btnActivarDesactivarV3 tbGrey">
                                                <Listeners>
                                                    <Click Fn="cambiarMultihoming" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Panel>

                            <ext:GridPanel
                                runat="server"
                                ID="grid"
                                SelectionMemory="false"
                                Cls="gridPanel grdNoHeader "
                                StoreID="storePrincipal"
                                Title="etiqgridTitle"
                                Header="false"
                                WidthSpec="100%"
                                EnableColumnHide="false"
                                ColumnWidth="0.7"
                                Height="400px"
                                OverflowX="Hidden"
                                OverflowY="Auto">
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
                                                Disabled="true"
                                                ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                Handler="AgregarEditar();">
                                            </ext:Button>
                                            <ext:Button runat="server"
                                                ID="btnEditar"
                                                Disabled="true"
                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                Cls="btnEditar"
                                                Handler="MostrarEditar();">
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
                                                Disabled="true"
                                                Handler="Refrescar();" />
                                            <ext:Button runat="server"
                                                ID="btnDescargar"
                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                Cls="btnDescargar"
                                                Disabled="true"
                                                Handler="ExportarDatos('LoginSettings', hdCliID.value, #{grid}, '');" />
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <ColumnModel runat="server">
                                    <Columns>
                                        <ext:Column runat="server"
                                            ID="colGroup"
                                            DataIndex="GrupoAcceso"
                                            Text="<%$ Resources:Comun, strGruposAccesosWeb %>"
                                            Width="80"
                                            Flex="1"
                                            Align="Center" />
                                        <ext:Column runat="server"
                                            ID="colRol"
                                            DataIndex="CodigoRol"
                                            Text="<%$ Resources:Comun, strRoles %>"
                                            Width="250"
                                            Flex="1" />
                                        <ext:Column runat="server"
                                            ID="colURL"
                                            DataIndex="URL"
                                            Text="URL"
                                            Width="250"
                                            Flex="1" />
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
                    </ext:Container>
                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
