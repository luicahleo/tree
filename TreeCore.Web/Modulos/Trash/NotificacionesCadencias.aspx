<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotificacionesCadencias.aspx.cs" Inherits="TreeCore.ModGlobal.NotificacionesCadencias" %>

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

            <ext:Store 
                runat="server" 
                ID="storeClientes" 
                AutoLoad="true" 
                OnReadData="storeClientes_Refresh" 
                RemoteSort="false">
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

            <ext:Store 
                runat="server" 
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
                    <ext:Model runat="server" 
                        IDProperty="NotificacionCadenciaID">
                        <Fields>
                            <ext:ModelField Name="NotificacionCadenciaID" Type="Int" />
                            <ext:ModelField Name="Lunes" Type="Boolean" />
                            <ext:ModelField Name="Martes" Type="Boolean" />
                            <ext:ModelField Name="Miercoles" Type="Boolean" />
                            <ext:ModelField Name="Jueves" Type="Boolean" />
                            <ext:ModelField Name="Viernes" Type="Boolean" />
                            <ext:ModelField Name="Sabado" Type="Boolean" />
                            <ext:ModelField Name="Domingo" Type="Boolean" />
                            <ext:ModelField Name="NotificacionCadencia" />
                            <ext:ModelField Name="DiaMes" Type="Int" />
                            <ext:ModelField Name="FechaEnvio" Type="Date" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NotificacionCadencia" Direction="ASC" />
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
                            <ext:TextField runat="server"
                                ID="txtNotificacionCadencia"
                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                MaxLength="100"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false" 
                                ValidationGroup="FORM" 
                                CausesValidation="true" />
                            <ext:Checkbox runat="server"
                                ID="chkLunes"
                                FieldLabel="<%$ Resources:Comun, strLunes %>" 
                                Checked="false" >
                            </ext:Checkbox>
                            <ext:Checkbox runat="server"
                                ID="chkMartes"  
                                FieldLabel="<%$ Resources:Comun, strMartes %>" 
                                Checked="false" >
                            </ext:Checkbox>
                            <ext:Checkbox runat="server"
                                ID="chkMiercoles"
                                FieldLabel="<%$ Resources:Comun, strMiercoles %>" 
                                Checked="false" >
                            </ext:Checkbox>
                            <ext:Checkbox runat="server"
                                ID="chkJueves" 
                                FieldLabel="<%$ Resources:Comun, strJueves %>" 
                                Checked="false" >
                            </ext:Checkbox>
                            <ext:Checkbox runat="server"
                                ID="chkViernes"  
                                FieldLabel="<%$ Resources:Comun, strViernes %>" 
                                Checked="false" >
                            </ext:Checkbox>
                            <ext:Checkbox runat="server"
                                ID="chkSabado" 
                                FieldLabel="<%$ Resources:Comun, strSabado %>" 
                                Checked="false" >
                            </ext:Checkbox>
                            <ext:Checkbox runat="server"
                                ID="chkDomingo"
                                FieldLabel="<%$ Resources:Comun, strDomingo %>" 
                                Checked="false"  >
                            </ext:Checkbox>
                            <ext:NumberField runat="server"
                                ID="txtDiaMes"
                                FieldLabel="<%$ Resources:Comun, strDiaDelMes %>" 
                                MaxLength="2" 
                                AllowBlank="true"
                                ValidationGroup="FORM" 
                                CausesValidation="true" 
                                MaxValue="31" 
                                EnforceMaxLength="true"/>
                            <ext:DateField runat="server"
                                ID="txtFechaEnvio" 
                                FieldLabel="<%$ Resources:Comun, strFechaEnvio %>"
                                AllowBlank="true" 
                                Format="<%$ Resources:Comun, FormatFecha %>" 
                                ValidationGroup="FORM"
                                CausesValidation="true"  />
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
                        AnchorHorizontal="100%"
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
                                        Cls="btnActivar"
                                        Handler="Activar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        Cls="btnEliminar"
                                        Handler="Eliminar();" />
                                    <ext:Button runat="server"
                                        ID="btnDefecto"
                                        ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                        meta:resourceKey="btnDefecto"
                                        Cls="btnDefecto"
                                        Handler="Defecto();" />
                                    <ext:Button runat="server"
                                        ID="btnRefrescar"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        Cls="btnRefrescar"
                                        Handler="Refrescar();" />
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        Cls="btnDescargar"
                                        Handler="ExportarDatos('NotificacionesCadencias', hdCliID.value, #{grid}, '');" />
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
                                    ID="colActivo"
                                    DataIndex="Activo"
                                    Align="Center"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                    Cls="col-activo"
                                    Width="50">
                                        <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colDefecto"
                                    DataIndex="Defecto"
                                    ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                    Align="Center"
                                    Cls="col-default"
                                    Width="50">
                                        <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colNotificacionCadencia"
                                    meta:resourceKey="columNotificacionCadencia"
                                    Text="<%$ Resources:Comun, strCadencia %>"
                                    DataIndex="NotificacionCadencia" 
                                    Width="150"    
                                    Flex="1" />
                                <ext:Column runat="server"
                                    ID="colLunes"
                                    DataIndex="Lunes" 
                                    Text="<%$ Resources:Comun, strLunes %>" 
                                    Width="60" 
                                    Flex="1"
                                    Align="Center" >
                                        <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colMartes"
                                    DataIndex="Martes" 
                                    Text="<%$ Resources:Comun, strMartes %>" 
                                    Width="60" 
                                    Flex="1"
                                    Align="Center" >
                                        <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colMiercoles"
                                    DataIndex="Miercoles" 
                                    Text="<%$ Resources:Comun, strMiercoles %>" 
                                    Width="60" 
                                    Flex="1"
                                    Align="Center"   >
                                        <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colJueves"
                                    DataIndex="Jueves" 
                                    Text="<%$ Resources:Comun, strJueves %>" 
                                    Width="60" 
                                    Flex="1"
                                    Align="Center" >
                                        <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colViernes"
                                    DataIndex="Viernes" 
                                    Text="<%$ Resources:Comun, strViernes %>" 
                                    Width="60"
                                    Flex="1"
                                    Align="Center" >
                                        <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colSabado"
                                    DataIndex="Sabado" 
                                    Text="<%$ Resources:Comun, strSabado %>" 
                                    Width="60"
                                    Flex="1"
                                    Align="Center" >
                                        <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colDomingo"
                                    DataIndex="Domingo" 
                                    Text="<%$ Resources:Comun, strDomingo %>" 
                                    Width="60" 
                                    Align="Center"
                                    Flex="1">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colDiaMes"
                                    DataIndex="DiaMes" 
                                    Text="<%$ Resources:Comun, strDiaDelMes %>" 
                                    Width="60" 
                                    Align="Center"
                                    Flex="1" >
                                </ext:Column>
                                <ext:DateColumn runat="server"
                                    ID="colFechaEnvio"
                                    meta:resourceKey="colFechaEnvio"
                                    DataIndex="FechaEnvio" 
                                    Text="<%$ Resources:Comun, strFechaEnvio %>" 
                                    Width="60"
                                    Align="Center"
                                    Flex="1">
                                </ext:DateColumn>
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
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="GridFilters" />                            
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
