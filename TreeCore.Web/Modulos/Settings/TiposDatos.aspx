<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TiposDatos.aspx.cs" Inherits="TreeCore.ModGlobal.TiposDatos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
</head>
<body>

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden runat="server" ID="hdCliID" />
            <ext:Hidden runat="server" ID="ModuloID" />

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
                    <ext:DataSorter
                        Property="Cliente"
                        Direction="ASC" />
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
                    <ext:Model runat="server" IDProperty="TipoDatoID">
                        <Fields>
                            <ext:ModelField Name="TipoDatoID" Type="Int" />
                            <ext:ModelField Name="TipoDato" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Defecto" Type="Boolean" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="TipoDato" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeDetalle"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeDetalle_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaDetalle" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="TipoDatoPropiedadID">
                        <Fields>
                            <ext:ModelField Name="TipoDatoPropiedadID" Type="Int" />
                            <ext:ModelField Name="TipoDatoID" Type="Int" />
                            <ext:ModelField Name="ValorDefecto" Type="String" />
                            <ext:ModelField Name="TipoValor" Type="String" />
                            <ext:ModelField Name="Nombre" Type="String" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTiposDatosOperadores"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeTiposDatosOperadores_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaOperadores" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="TipoDatoOperadorID">
                        <Fields>
                            <ext:ModelField Name="TipoDatoOperadorID" Type="Int" />
                            <ext:ModelField Name="TipoDatoID" Type="Int" />
                            <ext:ModelField Name="Nombre" Type="String" />
                            <ext:ModelField Name="ClaveRecurso" Type="String" />
                            <ext:ModelField Name="Operador" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="RequiereValor" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTiposValores"
                AutoLoad="false"
                OnReadData="storeTiposValores_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ValorId" runat="server">
                        <Fields>
                            <ext:ModelField Name="TipoValor" Type="String" />
                            <ext:ModelField Name="TipoValorID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTiposPropiedades"
                AutoLoad="false"
                OnReadData="storeTiposPropiedades_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="PropiedadId" runat="server">
                        <Fields>
                            <ext:ModelField Name="Propiedad" Type="String" />
                            <ext:ModelField Name="PropiedadID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window
                runat="server"
                ID="winGestion"
                meta:resourcekey="winGestion"
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
                                meta:resourceKey="txtTipoDato"
                                ID="txtTipoDato" runat="server"
                                FieldLabel="TipoDato"
                                Text=""
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                meta:resourceKey="txtCodigo"
                                ID="txtCodigo2" runat="server"
                                FieldLabel="<%$ Resources:Comun, strCodigo %>"
                                MinValue="0"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                            </ext:TextField>

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
                        IconCls="ico-cancel"
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
                ID="winGestionDetalle"
                Width="420"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="formGestionDetalle"
                        Cls="form-detalle"
                        Border="false"
                        MonitorPoll="500">
                        <Items>
                            <ext:ComboBox
                                runat="server"
                                ID="cmbTiposPropiedades"
                                StoreID="storeTiposPropiedades"
                                Mode="Local"
                                AllowBlank="false"
                                CausesValidation="true"
                                DisplayField="Propiedad"
                                QueryMode="Local"
                                ValueField="PropiedadID"
                                FieldLabel="<%$ Resources:Comun, strTipoPropiedad %>">
                                <Listeners>
                                    <Select Fn="SeleccionarCombo" />
                                    <TriggerClick Fn="RecargarCombo" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger
                                        meta:resourceKey="RecargarLista"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:ComboBox
                                runat="server"
                                ID="cmbTiposValores"
                                StoreID="storeTiposValores"
                                Mode="Local"
                                DisplayField="TipoValor"
                                QueryMode="Local"
                                AllowBlank="false"
                                CausesValidation="true"
                                ValueField="TipoValorID"
                                FieldLabel="<%$ Resources:Comun, strTipoValor %>">
                                <Listeners>
                                    <Select Fn="SeleccionarCombo" />
                                    <TriggerClick Fn="RecargarCombo" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger
                                        meta:resourceKey="RecargarLista"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:TextField runat="server"
                                ID="txtNombre"
                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                MaxLength="150"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField runat="server"
                                ID="txtValorDefecto"
                                FieldLabel="<%$ Resources:Comun, strValorDefecto %>"
                                MaxLength="150"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="false" />
                            <ext:Checkbox runat="server"
                                ID="chkAplicaReglas"
                                FieldLabel="<%$ Resources:Comun, strReglas %>"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="false" />
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoDetalle(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button runat="server"
                        ID="btnCancelarDetalle"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGestionDetalle}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardarDetalle"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        Disabled="true"
                        IconCls="ico-accept"
                        Cls="btn-accept">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardarDetalle();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Show Handler="#{winGestionDetalle}.center();" />
                </Listeners>
            </ext:Window>

            <ext:Window
                runat="server"
                ID="winGestionOperador"
                meta:resourcekey="winGestion"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="400"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <Items>
                    <ext:FormPanel
                        runat="server"
                        ID="formGestionOperador"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextField
                                ID="txtNombreDatoOperador" 
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                Text=""
                                MaxLength="50"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField
                                meta:resourceKey="txtCodigo"
                                ID="txtClaveRecurso" 
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strClaveRecurso %>"
                                MinValue="0"
                                AllowBlank="true"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                            </ext:TextField>
                            <ext:TextField
                                meta:resourceKey="txtOperador"
                                ID="txtOperador" 
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strOperador %>"
                                MinValue="0"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                            </ext:TextField>
                            <ext:Checkbox
                                meta:resourceKey="txtRequiereValor"
                                ID="chkRequiereValor" 
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strRequiereValor %>">
                            </ext:Checkbox>

                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoOperador(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button
                        runat="server"
                        ID="btnCancelarOperador"
                        meta:resourceKey="btnCancelar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <Listeners>
                            <Click Handler="#{winGestionOperador}.hide();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button
                        runat="server"
                        ID="btnGuardarOperador"
                        meta:resourceKey="btnGuardar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <Listeners>
                            <Click Handler="winGestionBotonGuardarOperador();" />
                        </Listeners>
                    </ext:Button>

                </Buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Layout="FitLayout">
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
                                MarginSpec="0 20 20 20"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain">

                                <DockedItems>
                                    <ext:Container runat="server" ID="WrapAlturaCabecera" MinHeight="60" Dock="Top" Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:Toolbar runat="server" ID="tbTitulo" Dock="Top" Cls="tbGrey tbTitleAlignBot tbNoborder" Hidden="false" Layout="ColumnLayout" Flex="1">
                                                <Items>

                                                    <ext:Label runat="server" ID="lbltituloPrincipal" Cls="TituloCabecera" Text="Tipos Datos" Height="25" />

                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Container>
                                    <ext:Toolbar runat="server" ID="tbFiltrosYSliders" Dock="Top" Cls="tbGrey tbNoborder " Hidden="false" Layout="HBoxLayout" Flex="1">
                                        <Items>


                                            <ext:Toolbar runat="server" ID="tbSliders" Dock="Top" Hidden="false" MinHeight="36" Cls="tbGrey tbNoborder PosUnsFloatR">
                                                <Items>

                                                    <ext:Button runat="server" ID="btnPrevSldr" IconCls="ico-prev-w" Cls="btnMainSldr SliderBtn" Handler="SliderMove('Prev');" Disabled="true"></ext:Button>
                                                    <ext:Button runat="server" ID="btnNextSldr" IconCls="ico-next-w" Cls="SliderBtn" Handler="SliderMove('Next');" Disabled="false"></ext:Button>

                                                </Items>
                                            </ext:Toolbar>



                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>

                                    <ext:Panel runat="server" ID="wrapComponenteCentral" Layout="HBoxLayout" BodyCls="tbGrey">
                                        <Listeners>
                                            <AfterRender Handler="ControlSlider(this)"></AfterRender>
                                            <Resize Handler="ControlSlider(this)"></Resize>
                                        </Listeners>
                                        <LayoutConfig>
                                            <ext:HBoxLayoutConfig Align="Stretch"></ext:HBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:GridPanel
                                                runat="server"
                                                ID="grid"
                                                meta:resourceKey="grid"
                                                Cls="gridPanel grdNoHeader"
                                                Flex="3"
                                                EnableColumnHide="false"
                                                StoreID="storePrincipal">
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
                                                                ID="btnActivar"
                                                                ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                                meta:resourceKey="btnActivar"
                                                                Cls="btn-Activar"
                                                                Handler="Activar();">
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnEliminar"
                                                                ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                meta:resourceKey="btnEliminar"
                                                                Cls="btn-Eliminar"
                                                                Handler="Eliminar();" />
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
                                                                Handler="ExportarDatos('TiposDatos', hdCliID.value, #{grid}, App.btnActivo.pressed, '', '');" />

                                                            <ext:Label runat="server"
                                                                ID="lblToggle"
                                                                Cls="lblBtnActivo"
                                                                PaddingSpec="0 0 25 0"
                                                                Text="<%$ Resources:Comun, strActivo %>" />
                                                            <ext:Button runat="server"
                                                                ID="btnActivo"
                                                                Width="41"
                                                                Pressed="true"
                                                                EnableToggle="true"
                                                                Cls="btn-toggleGrid"
                                                                ToolTip="<%$ Resources:Comun, strActivo %>"
                                                                Handler="Refrescar();" />
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar
                                                        runat="server"
                                                        ID="tlbClientes"
                                                        Dock="Top">
                                                        <Items>
                                                            <ext:ComboBox
                                                                runat="server"
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
                                                                    <ext:FieldTrigger
                                                                        meta:resourceKey="RecargarLista"
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
                                                        <ext:Column
                                                            runat="server"
                                                            ID="Activo"
                                                            DataIndex="Activo"
                                                            Align="Center"
                                                            Cls="col-activo"
                                                            ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                            Width="50">
                                                            <Renderer Fn="DefectoRender" />
                                                        </ext:Column>
                                                        <ext:Column
                                                            runat="server"
                                                            ID="Defecto"
                                                            DataIndex="Defecto"
                                                            Align="Center"
                                                            Cls="col-default"
                                                            ToolTip="<%$ Resources:Comun, colDefecto.ToolTip %>"
                                                            Width="50">
                                                            <Renderer Fn="DefectoRender" />
                                                        </ext:Column>
                                                        <ext:Column
                                                            DataIndex="TipoDato"
                                                            ID="colTipoDato" runat="server"
                                                            meta:resourceKey="colTipoDato"
                                                            Text="TipoDato"
                                                            Width="350" />
                                                        <ext:Column
                                                            DataIndex="Codigo"
                                                            ID="colCodigo" runat="server"
                                                            meta:resourceKey="colCodigo"
                                                            Text="<%$ Resources:Comun, strCodigo %>"
                                                            Width="350"
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

                                            <ext:Panel runat="server" ID="pnCol1" Flex="3" Layout="VBoxLayout" BodyCls="tbGrey" MarginSpec="0 0 0 10">
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                </LayoutConfig>
                                                <Items>
                                                    <ext:GridPanel
                                                        runat="server"
                                                        ID="gridDetalle"
                                                        Cls="gridPanel"
                                                        Flex="1"
                                                        EnableColumnHide="false"
                                                        StoreID="storeDetalle"
                                                        Header="true"
                                                        MarginSpec="0 0 5 0"
                                                        Title="<%$ Resources:Comun, strPropiedad %>"
                                                        Region="Center">
                                                        <DockedItems>
                                                            <ext:Toolbar runat="server"
                                                                ID="tlbDetalle"
                                                                Dock="Top"
                                                                Cls="tlbGrid">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        ID="btnAnadirDetalle"
                                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                                        Cls="btnAnadir"
                                                                        Handler="AgregarEditarDetalle()">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnEditarDetalle"
                                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                        Cls="btnEditar"
                                                                        Handler="MostrarEditarDetalle()">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnEliminarDetalle"
                                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                        Cls="btnEliminar"
                                                                        Handler="EliminarDetalle()" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnActivarDetalle"
                                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                                        Cls="btnActivar"
                                                                        Handler="ActivarDetalle()" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnRefrescarDetalle"
                                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                        Cls="btnRefrescar"
                                                                        Handler="RefrescarDetalle()" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnDescargarDetalle"
                                                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                        Cls="btnDescargar"
                                                                        Handler="ExportarDatos('TiposDatos', hdCliID.value, #{gridDetalle},  #{ModuloID}.value);" />
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:Column runat="server"
                                                                    ID="colActivo"
                                                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                                    Align="Center"
                                                                    DataIndex="Activo"
                                                                    Cls="col-activo"
                                                                    MinWidth="50">
                                                                    <Renderer Fn="DefectoRender" />
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    ID="colTipoValor"
                                                                    Text="<%$ Resources:Comun, strTipoValor %>"
                                                                    DataIndex="TipoValor"
                                                                    Width="150">
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    ID="colCodigoDetalle"
                                                                    Text="<%$ Resources:Comun, strTipoPropiedad %>"
                                                                    DataIndex="Codigo"
                                                                    MinWidth="150">
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    ID="colNombre"
                                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                                    DataIndex="Nombre"
                                                                    MinWidth="150">
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    ID="colValorDefecto"
                                                                    Text="<%$ Resources:Comun, strValorDefecto %>"
                                                                    DataIndex="ValorDefecto"
                                                                    MinWidth="150">
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server"
                                                                ID="GridRowSelectDetalle"
                                                                Mode="Single">
                                                                <Listeners>
                                                                    <Select Fn="Grid_RowSelect_Detalle" />
                                                                    <Deselect Fn="DeseleccionarGrillaDetalle" />
                                                                </Listeners>
                                                            </ext:RowSelectionModel>
                                                        </SelectionModel>
                                                        <Plugins>
                                                            <ext:GridFilters runat="server"
                                                                ID="gridFiltersDetalle"
                                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                                meta:resourceKey="GridFilters" />
                                                            <ext:CellEditing runat="server"
                                                                ClicksToEdit="2" />
                                                        </Plugins>
                                                        <BottomBar>
                                                            <ext:PagingToolbar runat="server"
                                                                ID="PagingToolBar2"
                                                                meta:resourceKey="PagingToolBar1"
                                                                StoreID="storeDetalle"
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
                                                                            <Select Fn="handlePageSizeSelectDetalle" />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                </Items>
                                                            </ext:PagingToolbar>
                                                        </BottomBar>
                                                    </ext:GridPanel>


                                                    <ext:GridPanel
                                                        runat="server"
                                                        ID="gridOperadores"
                                                        Cls="gridPanel"
                                                        Flex="1"
                                                        StoreID="storeTiposDatosOperadores"
                                                        Title="<%$ Resources:Comun, strOperadores %>"
                                                        Header="true"
                                                        EnableColumnHide="false"
                                                        MarginSpec="0 0 5 0"
                                                        Scroll="Vertical"
                                                        Region="Center">
                                                        <DockedItems>
                                                            <ext:Toolbar runat="server"
                                                                ID="Toolbar4"
                                                                Dock="Top"
                                                                Cls="tlbGrid">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        ID="btnAnadirOperador"
                                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                                        Cls="btnAnadir"
                                                                        Handler="AgregarEditarOperador()">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnEditarOperador"
                                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                        Cls="btnEditar"
                                                                        Handler="MostrarEditarOperador()">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnEliminarOperador"
                                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                        Cls="btnEliminar"
                                                                        Handler="EliminarOperador()" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnActivarOperador"
                                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                                        Cls="btnActivar"
                                                                        Handler="ActivarOperador()" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnRefrescarOperador"
                                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                        Cls="btnRefrescar"
                                                                        Handler="RefrescarOperador()" />
                                                                    <ext:Button runat="server"
                                                                        ID="btnDescargarOperador"
                                                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                        Cls="btnDescargar"
                                                                        Handler="ExportarDatos('TiposDatos', hdCliID.value, #{gridOperadores},  #{ModuloID}.value,'', 'operador');" />
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:Column runat="server"
                                                                    ID="colActivoOperador"
                                                                    ToolTip="<%$ Resources:Comun, colActivo.ToolTip %>"
                                                                    Align="Center"
                                                                    DataIndex="Activo"
                                                                    Cls="col-activo"
                                                                    MinWidth="50">
                                                                    <Renderer Fn="DefectoRender" />
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    ID="colNombreOperador"
                                                                    Text="<%$ Resources:Comun, strNombre %>"
                                                                    DataIndex="Nombre"
                                                                    MinWidth="150">
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    ID="colClaveRecurso"
                                                                    Text="<%$ Resources:Comun, strClaveRecurso %>"
                                                                    DataIndex="ClaveRecurso"
                                                                    MinWidth="150">
                                                                </ext:Column>

                                                                <ext:Column runat="server"
                                                                    ID="colOperador"
                                                                    Text="<%$ Resources:Comun, strOperador %>"
                                                                    DataIndex="Operador"
                                                                    MinWidth="150">
                                                                </ext:Column>
                                                                <ext:Column runat="server"
                                                                    ID="colRequiereValor"
                                                                    Text="<%$ Resources:Comun, strRequiereValor %>"
                                                                    Align="Center"
                                                                    DataIndex="RequiereValor"
                                                                    MinWidth="150">
                                                                    <Renderer Fn="DefectoRender" />
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server"
                                                                ID="GridRowSelectOperadores"
                                                                Mode="Single">
                                                                <Listeners>
                                                                    <Select Fn="Grid_RowSelect_Operadores" />
                                                                    <Deselect Fn="DeseleccionarGrillaOperadores" />
                                                                </Listeners>
                                                            </ext:RowSelectionModel>
                                                        </SelectionModel>
                                                        <Plugins>
                                                            <ext:GridFilters runat="server"
                                                                ID="gridFiltersOperadores"
                                                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                                                meta:resourceKey="GridFilters" />
                                                            <ext:CellEditing runat="server"
                                                                ClicksToEdit="2" />
                                                        </Plugins>
                                                        <BottomBar>
                                                            <ext:PagingToolbar runat="server"
                                                                ID="PagingToolBar1"
                                                                meta:resourceKey="PagingToolBar1"
                                                                StoreID="storeTiposDatosOperadores"
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
                                                                            <Select Fn="handlePageSizeSelectOperador" />
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
                    </ext:Panel>

                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
