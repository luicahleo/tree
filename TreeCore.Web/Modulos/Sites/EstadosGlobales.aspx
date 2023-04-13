<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EstadosGlobales.aspx.cs" Inherits="TreeCore.ModGlobal.EstadosGlobales" %>

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
                    <ext:Model runat="server" IDProperty="EstadoGlobalID">
                        <Fields>

                            <ext:ModelField Name="EstadoGlobalID" Type="Int" />
                            <ext:ModelField Name="EstadoGlobal" />
                            <ext:ModelField Name="Imagen" />
                            <ext:ModelField Name="Visible" Type="Boolean" />
                            <ext:ModelField Name="Analytics" Type="Boolean" />
                            <ext:ModelField Name="Desactivo" Type="Boolean" />
                            <ext:ModelField Name="Desinstalado" Type="Boolean" />
                            <ext:ModelField Name="Bloqueante" Type="Boolean" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="EstadoGlobal" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeImagenes" runat="server">
                <Model>
                    <ext:Model runat="server">
                        <Fields>
                            <ext:ModelField Name="iconCls" />
                            <ext:ModelField Name="name" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>
            <ext:Store ID="storeProyectosTipos" runat="server" AutoLoad="false" OnReadData="storeProyectosTipos_Refresh" PageSize="13"
                RemoteSort="true">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaProyectosTipos" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="EstadoGlobalBloqueadoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="EstadoGlobalBloqueadoID" Type="Int" />
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="ProyectoTipo" Type="string" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoTipoID" Direction="ASC" />
                </Sorters>
            </ext:Store>
            <ext:Store ID="storeProyectosTiposLibres" runat="server" AutoLoad="false" OnReadData="storeProyectosTiposLibres_Refresh"
                RemoteSort="true">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaProyectosTiposLibres" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ProyectoTipoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="ProyectoTipo" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoTipoID" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>
            <ext:Window runat="server"
                ID="winGestion"
                meta:resourcekey="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="500"
                BodyPaddingSummary="10 32"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestion"
                        Cls="ctForm-resp ctForm-resp-col2"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>

                            <ext:TextField ID="txtEstadoGlobal"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strEstado %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                Text=""
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                meta:resourceKey="txtEstadoGlobal" />
                            <ext:ComboBox
                                ID="cmbImagen"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strImagen %>"
                                LabelAlign="Top"
                                WidthSpec="100%"
                                StoreID="storeImagenes"
                                Editable="false"
                                DisplayField="name"
                                AllowBlank="false"
                                ValueField="iconCls"
                                QueryMode="Local"
                                TriggerAction="All"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                meta:resourceKey="cmbImagen">
                                <ListConfig>
                                    <ItemTpl runat="server">
                                        <Html>
                                            <div>
                                                <img class="cmbImgIcon" src="../../ima/{iconCls}">
                                                {name}
                                            </div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>
                                <Listeners>
                                    <Change Handler="changeCmbImage(this);" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:FieldContainer runat="server"
                                ID="chkActive"
                                Cls="chkCompleto"
                                Height="100"
                                Layout="HBoxLayout">
                                <Items>
                                    <ext:Checkbox runat="server"
                                        ID="chkActivo"
                                        BoxLabel="<%$ Resources:Comun, strActivo %>"
                                        Cls="chk-form"
                                        Width="200" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server"
                                ID="FieldContainer1"
                                Cls="chkCompleto"
                                Height="100"
                                Layout="HBoxLayout">
                                <Items>
                                    <ext:Checkbox runat="server"
                                        ID="chkVisible"
                                        BoxLabel="<%$ Resources:Comun, strVisible %>"
                                        Cls="chk-form"
                                        Width="200" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server"
                                ID="FieldContainer2"
                                Cls="chkCompleto"
                                Height="100"
                                Layout="HBoxLayout">
                                <Items>
                                    <ext:Checkbox runat="server"
                                        ID="chkAnalytics"
                                        BoxLabel="<%$ Resources:Comun, strAnalytics %>"
                                        Cls="chk-form"
                                        Width="200" 
                                        Checked="true"/>
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server"
                                ID="FieldContainer3"
                                Cls="chkCompleto"
                                Height="100"
                                Layout="HBoxLayout">
                                <Items>
                                    <ext:Checkbox runat="server"
                                        ID="chkDesactivo"
                                        BoxLabel="<%$ Resources:Comun, strDesactivo %>"
                                        Cls="chk-form"
                                        Width="200" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server"
                                ID="FieldContainer4"
                                Cls="chkCompleto"
                                Height="100"
                                Layout="HBoxLayout">
                                <Items>
                                    <ext:Checkbox runat="server"
                                        ID="chkDesinstalado"
                                        BoxLabel="<%$ Resources:Comun, strDesinstalado %>"
                                        Cls="chk-form"
                                        Width="200" />
                                </Items>
                            </ext:FieldContainer>
                            <ext:FieldContainer runat="server"
                                ID="FieldContainer5"
                                Cls="chkCompleto"
                                Height="100"
                                Layout="HBoxLayout">
                                <Items>
                                    <ext:Checkbox runat="server"
                                        ID="chkBloqueante"
                                        BoxLabel="<%$ Resources:Comun, strBloqueante %>"
                                        Cls="chk-form"
                                        Width="200" />
                                </Items>
                            </ext:FieldContainer>

                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValido(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnCancelar"
                        meta:resourceKey="btnCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGestion}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardar"
                        meta:resourceKey="btnGuardar"
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

            <ext:Window
                ID="winProyectosTipos"
                runat="server"
                Title="<%$ Resources:Comun, strProyectosTipos %>"
                Width="500"
                Height="500"
                Resizable="true"
                Modal="true" ShowOnLoad="true" Hidden="true"
                meta:resourceKey="winProyectosTipos"
                Scrollable="Disabled">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="gridProyectosTipos"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storeProyectosTipos"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        AnchorHorizontal="100%"
                        AnchorVertical="100%"
                        AriaRole="main"
                        Scrollable="Disabled">
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="Toolbar1"
                                Dock="Top"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnAgregar"
                                        meta:resourceKey="btnAgregar"
                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                        Cls="btnAnadir"
                                        Handler="BotonAgregarProyectoTipo();" />
                                    <ext:Button runat="server"
                                        ID="btnQuitar"
                                        meta:resourceKey="btnQuitar"
                                        ToolTip="<%$ Resources:Comun, strQuitar %>"
                                        Cls="btnEliminar"
                                        Disabled="true">
                                        <Listeners>
                                            <Click Handler="BotonEliminarProyectoTipo();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel ID="columnModelProyectoTipo" runat="server">
                            <Columns>
                                <ext:Column DataIndex="ProyectoTipo"
                                    ID="colProyectoTipo" runat="server"
                                    Header="<%$ Resources:Comun, strProyectoTipo %>"
                                    Width="350"
                                    meta:resourceKey="colProyectoTipo"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="GridRowSelectProyectosTipos" runat="server" Mode="Multi" EnableViewState="true">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectProyectosTipos" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersProyectosTipos"
                                meta:resourceKey="GridFilters" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>

                    </ext:GridPanel>

                </Items>
                <BottomBar>
                    <ext:PagingToolbar runat="server"
                        ID="PagingToolBar1"
                        meta:resourceKey="PagingToolBar"
                        StoreID="storeProyectosTipos"
                        DisplayInfo="true"
                        HideRefresh="false">
                    </ext:PagingToolbar>
                </BottomBar>
                <Listeners>
                    <Show Handler="#{winProyectosTipos}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window
                ID="winProyectosTiposLibres"
                runat="server"
                Title="<%$ Resources:Comun, strProyectosTiposLibre %>"
                Width="400"
                Height="400"
                Resizable="false"
                Modal="true" ShowOnLoad="true" Hidden="true"
                meta:resourceKey="winProyectosTiposLibres"
                Scrollable="Disabled">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="gridProyectosTiposLibres"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storeProyectosTiposLibres"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        AnchorHorizontal="100%"
                        Height="300"
                        AriaRole="main">

                        <ColumnModel ID="columnModelTiposEdificiosLbres" runat="server">
                            <Columns>
                                <ext:Column DataIndex="ProyectoTipo"
                                    ID="colProyectoTipoLibre" runat="server"
                                    Header="<%$ Resources:Comun, strProyectoTipo %>"
                                    Width="350"
                                    meta:resourceKey="colProyectoTipo"
                                    Flex="1" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="GridRowSelectProyectoTipoLibre" runat="server" Mode="Multi" SingleSelect="false" EnableViewState="true">
                                <Listeners>
                                    <Select Fn="Grid_RowSelectProyectoTipoLibre" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFiltersProyectoTipoLibre"
                                meta:resourceKey="GridFilters" />
                            <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>

                    </ext:GridPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnCancelarEdifLibres" runat="server"
                        meta:resourceKey="btnCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winProyectosTiposLibres}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="btnGuardarProyectosTiposLibre" runat="server"
                        meta:resourceKey="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="BotonGuardarProyectosTiposLibres();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Show Handler="#{winProyectosTiposLibres}.center();" />
                </Listeners>
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
                                        ID="btnActivar"
                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                        meta:resourceKey="btnActivar"
                                        Cls="btn-Activar"
                                        Handler="Activar();">
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnEliminar"
                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                        meta:resourceKey="btnEliminar"
                                        Cls="btn-Eliminar"
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
                                        meta:resourceKey="btnRefrescar"
                                        Cls="btnRefrescar"
                                        Handler="Refrescar();" />
                                    <ext:Button runat="server"
                                        ID="btnBloquear"
                                        ToolTip="<%$ Resources:Comun, btnBloquear.ToolTip %>"
                                        meta:resourceKey="btnBloquear"
                                        Cls="btnBloquear"
                                        Handler="BotonBloquear();" />
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        meta:resourceKey="btnDescargar"
                                        Cls="btnDescargar"
                                        Handler="ExportarDatos('EstadosGlobales', hdCliID.value, #{grid}, '');" />
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server"
                                ID="tlbClientes"
                                Dock="Top">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        ID="cmbClientes"
                                        meta:resourceKey="cmbClientes"
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
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
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
                                    Cls="col-activo"
                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                    meta:resourceKey="colActivo"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column runat="server"
                                    ID="colDefecto"
                                    DataIndex="Defecto"
                                    Align="Center"
                                    Cls="col-default"
                                    ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                    meta:resourceKey="colDefecto"
                                    Width="50">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>

                                <ext:Column DataIndex="EstadoGlobal"
                                    ID="EstadoGlobal" runat="server"
                                    Text="<%$ Resources:Comun, strEstado %>"
                                    Width="300"
                                    meta:resourceKey="ColumnEstadoGlobal"
                                    Flex="1" />
                                <ext:Column DataIndex="Imagen"
                                    ID="Imagen" runat="server"
                                    Text="<%$ Resources:Comun, strImagen %>"
                                    Width="80"
                                    Align="Center"
                                    meta:resourceKey="ColumnImagen">
                                    <Renderer Fn="EstadoGlobalRender" />
                                </ext:Column>

                                <ext:Column DataIndex="Visible"
                                    ID="Visible" runat="server"
                                    Text="<%$ Resources:Comun, strVisible %>"
                                    Width="100"
                                    Align="Center"
                                    meta:resourceKey="Visible">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column DataIndex="Analytics"
                                    ID="Analytics"
                                    runat="server"
                                    Text="<%$ Resources:Comun, strAnalytics %>"
                                    Width="100"
                                    Align="Center"
                                    meta:resourceKey="columnAnalytics">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column DataIndex="Desactivo"
                                    ID="Desactivo" runat="server"
                                    Text="<%$ Resources:Comun, strDesactivo %>"
                                    Width="100"
                                    Align="Center"
                                    meta:resourceKey="columnDesactivo">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column DataIndex="Desinstalado"
                                    ID="Desinstalado" runat="server"
                                    Text="<%$ Resources:Comun, strDesinstalado %>"
                                    Width="100"
                                    Align="Center"
                                    meta:resourceKey="columnDesinstalado">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column DataIndex="Bloqueante"
                                    ID="Bloqueante" runat="server"
                                    Text="<%$ Resources:Comun, strBloqueante %>"
                                    Width="100"
                                    Align="Center"
                                    meta:resourceKey="columnBloqueante">
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
