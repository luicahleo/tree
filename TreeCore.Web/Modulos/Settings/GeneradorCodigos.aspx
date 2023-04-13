<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GeneradorCodigos.aspx.cs" Inherits="TreeCore.ModGlobal.GeneradorCodigos" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>LIBRERIA EJEMPLOS</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdCampoDestino" runat="server" />
            <ext:Hidden ID="hdUsuarioID" runat="server" />
            <ext:Hidden ID="hd_MenuSeleccionado" runat="server" />
            <ext:Hidden ID="hdRaiz" runat="server" />
            <ext:Hidden ID="hdDatabase" runat="server" />
            <ext:Hidden ID="hdValue" runat="server" />
            <ext:Hidden ID="hdIndex" runat="server" />
            <ext:Hidden ID="hdFormulario" runat="server" />
            <ext:Hidden ID="hdTabla" runat="server" />

            <ext:Hidden ID="hdRegexConstante" runat="server" />

            <ext:Hidden ID="hdLongitudMaxima" runat="server" />



            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">

                <Listeners>
                    <%--<WindowResize Handler="GridResizer();" />
                    <WindowResize Handler="ColOverrideControl();" />--%>
                </Listeners>

            </ext:ResourceManager>
            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO STORES --%>

            <ext:Store runat="server"
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
                ID="storeGlobalCondicionReglaConfiguracion"
                runat="server"
                AutoLoad="false"
                RemotePaging="false"
                RemoteSort="true"
                RemoteFilter="false"
                PageSize="20"
                OnReadData="storeGlobalCondicionReglaConfiguracion_Refresh">

                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="GlobalCondicionReglaConfiguracionID">
                        <Fields>
                            <ext:ModelField Name="GlobalCondicionReglaConfiguracionID" Type="Int" />
                            <ext:ModelField Name="NombreCampo" />
                            <ext:ModelField Name="LongitudCadena" Type="Int" />
                            <ext:ModelField Name="TipoCondicion" />
                            <ext:ModelField Name="Orden" Type="Int" />
                            <ext:ModelField Name="Valor" />
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="GlobalCondicionReglaID" Type="Int" />
                            <ext:ModelField Name="NombreRegla" />
                            <ext:ModelField Name="ColumnaModeloDatoID" Type="Int" />
                            <ext:ModelField Name="TablaModeloDatosID" Type="Int" />
                            <ext:ModelField Name="ClaveRecursoTabla" />
                            <ext:ModelField Name="ClaveRecursoColumna" />

                        </Fields>
                    </ext:Model>
                </Model>

                <Sorters>
                    <ext:DataSorter Property="Orden" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeGlobalCondicionesTablas"
                runat="server"
                AutoLoad="false"
                RemotePaging="false"
                RemoteSort="false"
                RemoteFilter="false"
                PageSize="20"
                OnReadData="storeGlobalCondicionesTablas_Refresh">

                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="GlobalCondicionTablaID">
                        <Fields>
                            <ext:ModelField Name="GlobalCondicionTablaID" Type="Int" />
                            <ext:ModelField Name="CampoDestino" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="TablaModeloDatosID" Type="Int" />
                            <ext:ModelField Name="NombreTabla" />
                            <ext:ModelField Name="ClaveRecurso" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreTabla" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeGlobalCondicionesColumnasTablas"
                runat="server"
                AutoLoad="false"
                RemotePaging="false"
                RemoteSort="false"
                RemoteFilter="false"
                PageSize="20"
                OnReadData="storeGlobalCondicionesColumnasTablas_Refresh">

                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ColumnaModeloDatosID">
                        <Fields>
                            <ext:ModelField Name="ColumnaModeloDatosID" Type="Int" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="NombreColumna" />
                            <ext:ModelField Name="ClaveRecurso" />
                            <ext:ModelField Name="TablaModeloDatosID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreColumna" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeGlobalCondicionesFormularios"
                runat="server"
                AutoLoad="false"
                RemotePaging="false"
                RemoteSort="false"
                RemoteFilter="false"
                PageSize="20"
                OnReadData="storeGlobalCondicionesFormularios_Refresh">

                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="GlobalCondicionFormularioID">
                        <Fields>
                            <ext:ModelField Name="GlobalCondicionFormularioID" Type="Int" />
                            <ext:ModelField Name="CampoDestino" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="FormularioModeloDatosID" Type="Int" />
                            <ext:ModelField Name="NombreFormulario" />
                            <ext:ModelField Name="ClaveRecurso" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreFormulario" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeGlobalCondicionesColumnasFormularios"
                runat="server"
                AutoLoad="false"
                RemotePaging="false"
                RemoteSort="false"
                RemoteFilter="false"
                PageSize="20"
                OnReadData="storeGlobalCondicionesColumnasFormularios_Refresh">

                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="ColumnaModeloDatosID">
                        <Fields>
                            <ext:ModelField Name="ColumnaModeloDatosID" Type="Int" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="NombreColumna" />
                            <ext:ModelField Name="ClaveRecurso" />
                            <ext:ModelField Name="FormularioModeloDatosID" Type="Int" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreColumna" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <ext:Store ID="storeProyectosTipos" runat="server" AutoLoad="false" OnReadData="storeProyectosTipos_Refresh"
                RemoteSort="true">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model IDProperty="ProyectoTipoID" runat="server">
                        <Fields>
                            <ext:ModelField Name="ProyectoTipoID" Type="Int" />
                            <ext:ModelField Name="ProyectoTipo" />
                            <ext:ModelField Name="Alias" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ProyectoTipo" Direction="ASC" />
                </Sorters>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--WINDOWS EJEMPLO, (BORRAR A NECESIDAD)  --%>
            <ext:Window ID="winGestion"
                runat="server"
                Title="<%$ Resources:Comun, strAnadirNuevoCodigo %>"
                Width="450"
                MaxWidth="550"
                Height="420"
                Modal="true"
                Centered="true"
                Cls="winForm-resp"
                Scrollable="Vertical"
                Hidden="true"
                Resizable="false">
                <Listeners>
                    <AfterRender Handler="winFormResize(this);" />

                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="Toolbar6"
                        Cls="tbGrey"
                        Dock="Bottom"
                        PaddingSpec="0 10 16 10">
                        <Items>
                            <ext:ToolbarFill Flex="8"></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="Button2"
                                Cls="btn-secondary"
                                Text="<%$ Resources:Comun, strCancelar %>"
                                Handler="#{winGestion}.hide();">
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnGuardarDetalle"
                                Cls="btn-ppal"
                                Handler="BotonGuardarDetalle()"
                                Disabled="true"
                                Text="<%$ Resources:Comun, strGuardar%>">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel ID="formGestion" Cls="formGris" runat="server" Hidden="false">
                        <Items>
                            <ext:Container runat="server" ID="ctForm" Cls="ctForm-resp ctForm-resp-col2">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtNombreCodigo"
                                        FieldLabel="<%$ Resources:Comun, strNombreCodigo %>"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        EmptyText="<%$ Resources:Comun, strNombreCodigo %>" />

                                    <ext:ComboBox runat="server"
                                        ID="cmbTipoCondicion"
                                        FieldLabel="<%$ Resources:Comun, strTipoCondicion %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="<%$ Resources:Comun, strAutoCaracter %>" Value="Auto_Caracter" />
                                            <ext:ListItem Text="<%$ Resources:Comun, strAutoNumerico %>" Value="Auto_Numerico" />
                                            <ext:ListItem Text="<%$ Resources:Comun, strConstante %>" Value="Constante" />
                                            <ext:ListItem Text="<%$ Resources:Comun, strSeparador %>" Value="Separador" />
                                            <ext:ListItem Text="<%$ Resources:Comun, strFormulario %>" Value="Formulario" />
                                            <ext:ListItem Text="<%$ Resources:Comun, strTabla %>" Value="Tabla" />
                                        </Items>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                        <Listeners>
                                            <Select Fn="SeleccionarTipoCondicion" />
                                        </Listeners>
                                    </ext:ComboBox>

                                    <ext:TextField runat="server"
                                        FieldLabel="<%$ Resources:Comun, strValor %>"
                                        LabelAlign="Top"
                                        Hidden="true"
                                        AllowBlank="false"
                                        Regex="/^[^$%&|<>/\#]*$/"
                                        RegexText="<%$ Resources:Comun, regexNombreText %>"
                                        MinValue="1"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        meta:resourceKey="txtCodigo"
                                        ID="txtValor">
                                        <Listeners>
                                            <ValidityChange Fn="FormularioValidoGeneradorCodigo" />
                                        </Listeners>
                                    </ext:TextField>

                                    <ext:NumberField
                                        runat="server"
                                        ID="txtLongitud"
                                        LabelAlign="Top"
                                        FieldLabel="<%$ Resources:Comun, strLongitud %>"
                                        MaxLength="50"
                                        MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                        MaxValue="40"
                                        MinValue="1"
                                        AllowBlank="false"
                                        Hidden="true"
                                        ValidationGroup="FORM"
                                        CausesValidation="true">
                                        <Listeners>
                                            <%--<Show Fn="MostrarContenedorPadreForm" />
                                            <Hide Fn="OcultarContenedorPadre" />
                                            <AfterRender Fn="OcultarContenedorPadreForm" />
                                            <Change Fn="Validar" />--%>
                                        </Listeners>
                                    </ext:NumberField>

                                    <ext:ComboBox runat="server"
                                        ID="cmbTabla"
                                        Hidden="true"
                                        FieldLabel="<%$ Resources:Comun, strTabla %>"
                                        LabelAlign="Top"
                                        Mode="Local"
                                        DisplayField="NombreTabla"
                                        ValueField="TablaModeloDatosID"
                                        StoreID="storeGlobalCondicionesTablas"
                                        EmptyText="<%$ Resources:Comun, strNinguno %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        EnableKeyEvents="true">
                                        <Listeners>
                                            <Select Fn="SeleccionarTabla" />
                                            <TriggerClick Fn="RecargarTablas" />
                                            <KeyUp Fn="ComboBoxKeyUp" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                Icon="Clear"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>

                                    <ext:ComboBox runat="server"
                                        ID="cmbColumnaTabla"
                                        Mode="Local"
                                        DisplayField="NombreColumna"
                                        ValueField="ColumnaModeloDatosID"
                                        StoreID="storeGlobalCondicionesColumnasTablas"
                                        Hidden="true"
                                        FieldLabel="<%$ Resources:Comun, strColumnaTabla %>"
                                        EmptyText="<%$ Resources:Comun, strNinguno %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        EnableKeyEvents="true"
                                        LabelAlign="Top">
                                        <Listeners>
                                            <Select Fn="SeleccionarColumnaTabla" />
                                            <TriggerClick Fn="RecargarColumnaTabla" />
                                            <KeyUp Fn="ComboBoxKeyUp" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                Icon="Clear"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>

                                    <ext:ComboBox runat="server"
                                        ID="cmbFormulario"
                                        Hidden="true"
                                        FieldLabel="<%$ Resources:Comun, strFormulario %>"
                                        LabelAlign="Top"
                                        Mode="Local"
                                        DisplayField="NombreFormulario"
                                        ValueField="FormularioModeloDatosID"
                                        StoreID="storeGlobalCondicionesFormularios"
                                        EmptyText="<%$ Resources:Comun, strNinguno %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        EnableKeyEvents="true">
                                        <Listeners>
                                            <Select Fn="SeleccionarFormulario" />
                                            <TriggerClick Fn="RecargarFormularios" />
                                            <KeyUp Fn="ComboBoxKeyUp" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                Icon="Clear"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>

                                    <ext:ComboBox runat="server"
                                        ID="cmbColumnaFormulario"
                                        Mode="Local"
                                        DisplayField="NombreColumna"
                                        ValueField="ColumnaModeloDatosID"
                                        StoreID="storeGlobalCondicionesColumnasFormularios"
                                        Hidden="true"
                                        FieldLabel="<%$ Resources:Comun, strColumnaFormulario %>"
                                        EmptyText="<%$ Resources:Comun, strNinguno %>"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        EnableKeyEvents="true"
                                        LabelAlign="Top">
                                        <Listeners>
                                            <Select Fn="SeleccionarColumnaFormulario" />
                                            <TriggerClick Fn="RecargarColumnaFormulario" />
                                            <KeyUp Fn="ComboBoxKeyUp" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                Icon="Clear"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>

                                </Items>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoDetalle(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                    <%--<ext:ButtonGroup runat="server" Cls="btnWin">
                        <Items>
                            <ext:Button runat="server" ID="Button2" Cls="btn-secondary" Text="<%$ Resources:Comun, strCancelar %>" Handler="#{winGestion}.hide();"></ext:Button>
                            <ext:Button runat="server" ID="btnGuardarDetalle" Cls="btn-ppal" Handler="BotonGuardarDetalle()" Disabled="true" Text="<%$ Resources:Comun, strGuardar%>"></ext:Button>
                        </Items>
                    </ext:ButtonGroup>--%>
                </Items>

            </ext:Window>

            <ext:Window ID="winSaveNewTabCols"
                runat="server"
                Title="<%$ Resources:Comun, strAnadirNuevoCodigo %>"
                Width="300"
                Height="280"
                Modal="true"
                Centered="true"
                Cls="winForm-respSimple winSaveNewTabCols"
                Scrollable="Default"
                Layout="VBoxLayout"
                Hidden="true">
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <LayoutConfig>
                    <ext:VBoxLayoutConfig Align="Stretch" />
                </LayoutConfig>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="Toolbar1"
                        Cls="tbGrey"
                        Dock="Bottom"
                        PaddingSpec="0 10 16 10">
                        <Items>
                            <ext:ToolbarFill Flex="8"></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="Button18"
                                Cls="btn-secondary"
                                MinWidth="120"
                                Text="<%$ Resources:Comun, strCancelar %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="2"
                                Hidden="false"
                                Handler="#{winSaveNewTabCols}.hide();">
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnGuardar"
                                Cls="btn-ppal btnBold "
                                MinWidth="120"
                                Text="<%$ Resources:Comun, strAceptar %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="2"
                                Disabled="true"
                                Hidden="false"
                                Handler="BotonGuardar()">
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel runat="server" ID="FormSaveNewTabCols" Cls="formWorkflow formGris FormGrid">
                        <Items>
                            <ext:TextField runat="server"
                                MarginSpec="10 0 0 0"
                                ID="txtNombreRegla"
                                LabelAlign="top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                EmptyText="<%$ Resources:Comun, strNinguno %>"
                                FieldLabel="<%$ Resources:Comun, strNombreRegla %>">
                            </ext:TextField>

                            <ext:ComboBox runat="server"
                                ID="cmbModulo"
                                meta:resourcekey="cmbModulo"
                                MarginSpec="10 0 0 0"
                                LabelAlign="top"
                                Mode="Local"
                                FieldLabel="<%$ Resources:Comun, strModulo %>"
                                DisplayField="Alias"
                                ValueField="ProyectoTipoID"
                                StoreID="storeProyectosTipos"
                                QueryMode="Local"
                                EmptyText="<%$ Resources:Comun, strNinguno %>"
                                Editable="true"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                                <Listeners>
                                    <Select Fn="SeleccionarProyectoTipo" />
                                    <TriggerClick Fn="RecargarProyectoTipo" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                        Icon="Clear"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </Triggers>
                            </ext:ComboBox>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValido(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Viewport runat="server"
                ID="MainVwP"
                OverflowY="auto"
                Layout="FitLayout">
                <Listeners>
                    <AfterRender Handler="winFormResize();"></AfterRender>
                    <AfterLayout Handler="showPanelsByWindowSize()"></AfterLayout>
                    <Resize Handler="showPanelsByWindowSize(); resizeCenterPanel();" Buffer="50"></Resize>
                </Listeners>
                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Layout="BorderLayout">
                        <Items>
                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                Border="false"
                                Region="Center"
                                Layout="HBoxLayout"
                                BodyCls="bgGrey"
                                Cls="col-pdng visorInsidePn heigPanel">
                                <LayoutConfig>
                                    <ext:HBoxLayoutConfig Align="Stretch" />
                                </LayoutConfig>
                                <DockedItems>
                                    <ext:Toolbar runat="server"
                                        ID="tbCollapseAsR"
                                        Cls="tbGrey"
                                        Dock="Top"
                                        Hidden="true"
                                        MinHeight="20">
                                        <Items>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Button runat="server"
                                                ID="btnCollapseAsRClosed"
                                                Cls="btn-trans btnCollapseAsRClosedv2"
                                                Hidden="true"
                                                ToolTip="<%$ Resources:Comun, strAbrirMenu %>">
                                                <Listeners>
                                                    <Click Fn="hidePnFilters" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar runat="server" ID="tbFiltrosYSliders" Dock="Top" Cls="tbGrey tbNoborder" Hidden="true" PaddingSpec="6 0 0 8" Layout="HBoxLayout" Flex="1">
                                        <Items>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Toolbar runat="server"
                                                ID="tbSliders"
                                                Dock="Top"
                                                Hidden="false"
                                                MinHeight="36"
                                                Cls="tbGrey tbNoborder">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        ID="btnPrev"
                                                        IconCls="ico-prev-w"
                                                        Cls="btnMainSldr SliderBtn"
                                                        Handler="loadPanelByBtns('Prev'); "
                                                        Disabled="true">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        ID="btnNext"
                                                        IconCls="ico-next-w"
                                                        Cls="SliderBtn"
                                                        Handler="loadPanelByBtns('Next'); "
                                                        Disabled="false">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server"
                                        ID="ctMain1"
                                        Flex="2"
                                        Layout="FitLayout"
                                        Padding="0"
                                        Hidden="false"
                                        MinWidth="330"
                                        MaxWidth="400">
                                        <Items>
                                            <ext:TreePanel
                                                Hidden="false"
                                                Cls="gridPanel TreePnl"
                                                ID="TreePanelSideL"
                                                ContextMenuID="ContextMenuTreeL"
                                                runat="server"
                                                Flex="1"
                                                Split="false"
                                                Region="West"
                                                Resizable="false"
                                                Collapsible="false"
                                                CollapseMode="Mini"
                                                Scrollable="Vertical"
                                                RootVisible="false"
                                                Title="<%$ Resources:Comun, strCondicionesCodigo %>"
                                                MinWidth="320">
                                                <DockedItems>

                                                    <ext:Toolbar runat="server" Dock="Top" ID="toolbar4" Hidden="false" Cls="tlbGrid">
                                                        <Items>

                                                            <ext:Button runat="server"
                                                                Disabled="true"
                                                                ID="btnAnadir"
                                                                Cls="btnAnadir"
                                                                AriaLabel="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                                ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                                Handler="AgregarEditar();">
                                                            </ext:Button>

                                                            <ext:Button runat="server"
                                                                Disabled="true"
                                                                ID="btnEditar"
                                                                Cls="btnEditar"
                                                                AriaLabel="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                Handler="MostrarEditar();">
                                                            </ext:Button>

                                                            <ext:Button runat="server"
                                                                Disabled="true"
                                                                ID="btnEliminar"
                                                                Cls="btnEliminar"
                                                                AriaLabel="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                Handler="Eliminar();">
                                                            </ext:Button>

                                                            <ext:Button runat="server"
                                                                Disabled="false"
                                                                ID="btnRefrescar"
                                                                Cls="btnRefrescar"
                                                                AriaLabel="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                Handler="Refrescar();">
                                                            </ext:Button>

                                                            <ext:Button runat="server"
                                                                ID="btnDefecto"
                                                                Disabled="true"
                                                                ToolTip="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                                                AriaLabel="<%$ Resources:Comun, btnDefecto.ToolTip %>"
                                                                meta:resourceKey="btnDefecto"
                                                                Cls="btnDefecto"
                                                                Handler="Defecto();">
                                                            </ext:Button>

                                                            <ext:Button runat="server"
                                                                ID="btnActivar"
                                                                ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                                Cls="btnActivar"
                                                                Handler="Activar();">
                                                            </ext:Button>
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
                                                <Listeners>
                                                </Listeners>
                                                <Fields>
                                                    <ext:ModelField Name="archivo" />
                                                    <ext:ModelField Name="modulo" />
                                                </Fields>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:TreeColumn
                                                            ID="TreeColumn1"
                                                            runat="server"
                                                            Flex="1"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            DataIndex="text" />
                                                        <ext:TemplateColumn runat="server" DataIndex="NombreRegla" MenuDisabled="true" Flex="1">

                                                            <Template runat="server">
                                                                <Html>
                                                                    <tpl for=".">
                                                                        <div class="ContenedorRegla">
                                                                            <div class="">
                                                                                {UltimoGenerado}
                                                                            </div>
                                                                            <div class="defecto details">
                                                                                {Defecto}
                                                                            </div>
                                                                            <div class="modificado details">
                                                                                {Modificada}
                                                                            </div>
                                                                            <div class="activo details">
                                                                                {Activo}
                                                                            </div>
                                                                        </div>
                                                                    </tpl>

                                                                </Html>
                                                            </Template>
                                                        </ext:TemplateColumn>

                                                    </Columns>

                                                </ColumnModel>
                                                <Listeners>
                                                    <Select Fn="Grid_RowSelect" />
                                                </Listeners>
                                                <BottomBar>
                                                    <ext:PagingToolbar runat="server" StoreID="StoreGridMain" Cls="PgToolBMainGrid" ID="PagingToolbar1" MaintainFlex="true" Hidden="true" Flex="8">
                                                        <Items>
                                                            <ext:ComboBox runat="server" Cls="comboGrid" ID="ComboBox2" Flex="2">
                                                                <Items>
                                                                    <ext:ListItem Text="10 Registros" />
                                                                    <ext:ListItem Text="20 Registros" />
                                                                    <ext:ListItem Text="30 Registros" />
                                                                    <ext:ListItem Text="40 Registros" />
                                                                </Items>
                                                                <SelectedItems>
                                                                    <ext:ListItem Value="20 Registros" />
                                                                </SelectedItems>

                                                            </ext:ComboBox>

                                                        </Items>

                                                    </ext:PagingToolbar>
                                                </BottomBar>

                                            </ext:TreePanel>
                                        </Items>
                                    </ext:Panel>

                                    <ext:Panel runat="server" ID="ctMain2" Flex="4" Layout="FitLayout" Cls="col colCt2 grdNoHeader" BodyCls="bgGrey" Hidden="false">
                                        <Items>
                                            <ext:Panel ID="PanelVisorMain"
                                                runat="server"
                                                Header="false"
                                                Border="false"
                                                Region="Center"
                                                Layout="FitLayout"
                                                Cls="visorInsidePn">
                                                <Items>
                                                    <ext:Panel runat="server" ID="WrapGridP2Bot" Cls="gridPanel" Layout="FitLayout" Title="<%$ Resources:Comun, strCodigosReglas %>" Header="false" Flex="1">
                                                        <DockedItems>
                                                            <ext:Toolbar runat="server" Dock="Top" ID="toolbar" Hidden="false" OverflowHandler="Scroller" Cls="tlbGrid">
                                                                <Items>
                                                                    <ext:Button runat="server" Disabled="true" ID="btnAnadirDetalle" Cls="btnAnadir" AriaLabel="<%$ Resources:Comun, btnAnadir.ToolTip %>" ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>" Handler="AgregarEditarDetalle();"></ext:Button>
                                                                    <ext:Button runat="server" Disabled="true" ID="btnEditarDetalle" Cls="btnEditar" AriaLabel="<%$ Resources:Comun, btnEditar.ToolTip %>" ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>" Handler="MostrarEditarDetalle();"></ext:Button>
                                                                    <ext:Button runat="server" Disabled="true" ID="btnEliminarDetalle" Cls="btnEliminar" AriaLabel="<%$ Resources:Comun, btnEliminar.ToolTip %>" ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>" Handler="EliminarDetalle();"></ext:Button>
                                                                    <ext:Button runat="server" Disabled="true" ID="btnRefrescarDetalle" Cls="btnRefrescar" AriaLabel="<%$ Resources:Comun, btnRefrescar.ToolTip %>" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>" Handler="RefrescarDetalle();"></ext:Button>
                                                                    <ext:Button runat="server" Disabled="true" ID="btnDescargarDetalle" Cls="btnDescargar" AriaLabel="<%$ Resources:Comun, btnDescargar.ToolTip %>" ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>" Handler="ExportarDatos('GeneradorCodigos', App.hdCliID.value, #{gridReglasCodigos}, App.hd_MenuSeleccionado.value, '', '');"></ext:Button>
                                                                </Items>
                                                            </ext:Toolbar>
                                                            <ext:Toolbar runat="server" ID="Toolbar2" Cls="tlbGrid" Layout="ColumnLayout" Dock="Top">
                                                                <Items>
                                                                    <ext:TextField
                                                                        ID="txtSearch"
                                                                        Cls="txtSearchC"
                                                                        runat="server"
                                                                        EmptyText="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelWidth="50"
                                                                        Width="250"
                                                                        MaxWidth="300"
                                                                        EnableKeyEvents="true">
                                                                        <Triggers>
                                                                            <ext:FieldTrigger Icon="Clear" />
                                                                        </Triggers>
                                                                        <Listeners>
                                                                            <KeyUp Fn="FiltrarColumnas" Buffer="250" />
                                                                            <TriggerClick Fn="LimpiarFiltroBusqueda" />
                                                                        </Listeners>

                                                                    </ext:TextField>

                                                                    <%--<ext:Button runat="server"
                                                                        Width="30"
                                                                        ID="btnClearFilters"
                                                                        Cls="btn-trans btnRemoveFilters"
                                                                        ToolTip="<%$ Resources:Comun, strQuitarFiltro %>">
                                                                        <Listeners>
                                                                            <Click Handler="BorrarFiltros(#{gridReglasCodigos});"></Click>
                                                                        </Listeners>
                                                                    </ext:Button>--%>
                                                                </Items>
                                                            </ext:Toolbar>
                                                            <ext:Toolbar runat="server" Dock="Bottom" ID="tlBottom" Hidden="false" OverflowHandler="Scroller" Cls="tlbBottomGrid">
                                                                <Items>
                                                                    <ext:ToolbarFill></ext:ToolbarFill>
                                                                    <ext:Button runat="server" Disabled="true" ID="btnSimularCodigo" Cls="btnVisible" AriaLabel="<%$ Resources:Comun, btnRefrescar.ToolTip %>" ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>" Handler="BotonSimularCodigo();"></ext:Button>
                                                                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" Hidden="true">
                                                                    </ext:ToolbarSeparator>
                                                                    <ext:Label runat="server" Text="<%$ Resources:Comun, strCodigoSimulado %>" ID="lblCodigo" Hidden="false" meta:resourceKey="lblCodigo">
                                                                    </ext:Label>
                                                                    <ext:TextField runat="server" ReadOnly="true" Hidden="false" ID="txtSimulacionCodido" Width="120">
                                                                    </ext:TextField>
                                                                    <ext:Label runat="server" Text="<%$ Resources:Comun, strCodigoSiguiente %>" ID="lblCodigoSiguiente" Hidden="false" meta:resourceKey="lblCodigoSiguiente">
                                                                    </ext:Label>
                                                                    <ext:TextField runat="server" ReadOnly="true" Hidden="false" ID="txtSimulacionCodidoSiguinete" Width="120">
                                                                    </ext:TextField>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <Items>
                                                            <ext:GridPanel
                                                                runat="server"
                                                                meta:resourceKey="storeGlobalCondicionReglaConfiguracion"
                                                                EnableColumnHide="false"
                                                                AriaRole="main"
                                                                Hidden="false"
                                                                Header="false"
                                                                ForceFit="true"
                                                                FocusCls="none"
                                                                ID="gridReglasCodigos"
                                                                Cls="gridPanel gridPanelNoBorder "
                                                                Reorderable="true"
                                                                Flex="1"
                                                                Scrollable="Vertical"
                                                                StoreID="storeGlobalCondicionReglaConfiguracion">
                                                                <Listeners>
                                                                    <AfterRender Handler="GridColHandlerDinamicoV2(this)"></AfterRender>
                                                                    <Resize Handler="GridColHandlerDinamicoV2(this)"></Resize>
                                                                </Listeners>
                                                                <ColumnModel>
                                                                    <Columns>
                                                                        <ext:Column runat="server"
                                                                            Flex="2"
                                                                            Text="<%$ Resources:Comun, strNombre %>"
                                                                            ID="colName"
                                                                            DataIndex="NombreCampo"
                                                                            meta:resourceKey="colName"
                                                                            MinWidth="120"
                                                                            Align="Center">
                                                                        </ext:Column>
                                                                        <ext:Column runat="server"
                                                                            Flex="1"
                                                                            Text="<%$ Resources:Comun, strValor %>"
                                                                            ID="colValue"
                                                                            DataIndex="Valor"
                                                                            meta:resourceKey="colValue"
                                                                            MinWidth="120"
                                                                            Align="Center">
                                                                        </ext:Column>
                                                                        <ext:Column runat="server"
                                                                            ID="colChainLenth"
                                                                            Text="<%$ Resources:Comun, strLongitudCadena %>"
                                                                            Flex="1"
                                                                            DataIndex="LongitudCadena"
                                                                            meta:resourceKey="colChainLenth"
                                                                            MinWidth="120"
                                                                            Align="Center">
                                                                        </ext:Column>
                                                                        <ext:Column runat="server"
                                                                            ID="colConditionsType"
                                                                            Text="<%$ Resources:Comun, strTipoCondicion %>"
                                                                            Flex="2"
                                                                            DataIndex="TipoCondicion"
                                                                            meta:resourceKey="colConditionsType"
                                                                            MinWidth="120"
                                                                            Align="Center">
                                                                        </ext:Column>
                                                                        <%--<ext:Column runat="server"
                                                                            ID="colCode"
                                                                            Text="<%$ Resources:Comun, strCodigo %>"
                                                                            Flex="3"
                                                                            DataIndex="Codigo"
                                                                            meta:resourceKey="colCode"
                                                                            Align="Center">
                                                                        </ext:Column>--%>
                                                                        <ext:Column runat="server"
                                                                            ID="colFormulario"
                                                                            Text="<%$ Resources:Comun, strFormulario %>"
                                                                            Flex="2"
                                                                            DataIndex="ClaveRecursoTabla"
                                                                            meta:resourceKey="colTable"
                                                                            MinWidth="120"
                                                                            Align="Center">
                                                                        </ext:Column>
                                                                        <ext:Column runat="server"
                                                                            ID="colColumnaFormulario"
                                                                            Text="<%$ Resources:Comun, strColumnaFormulario %>"
                                                                            Flex="2"
                                                                            DataIndex="ClaveRecursoColumna"
                                                                            meta:resourceKey="colTable"
                                                                            MinWidth="120"
                                                                            MaxWidth="250"
                                                                            Align="Center">
                                                                        </ext:Column>
                                                                        <ext:WidgetColumn ID="ColMore"
                                                                            runat="server"
                                                                            Cls="NoOcultar col-More"
                                                                            DataIndex=""
                                                                            Align="Center"
                                                                            Text="More"
                                                                            Hidden="false"
                                                                            MinWidth="90"
                                                                            MaxWidth="90"
                                                                            Filterable="false">
                                                                            <Widget>
                                                                                <ext:Button runat="server"
                                                                                    Width="90"
                                                                                    OverCls="Over-btnMore"
                                                                                    PressedCls="Pressed-none"
                                                                                    FocusCls="Focus-none"
                                                                                    Cls="btnColMore">
                                                                                    <Listeners>
                                                                                        <Click Handler="hidePanelMoreInfoCodigos(this);" />
                                                                                    </Listeners>
                                                                                </ext:Button>
                                                                            </Widget>
                                                                        </ext:WidgetColumn>
                                                                    </Columns>
                                                                </ColumnModel>
                                                                <View>
                                                                    <ext:GridView ID="ListaReordenada" runat="server">
                                                                        <Plugins>
                                                                            <ext:GridDragDrop runat="server" />
                                                                        </Plugins>
                                                                        <Listeners>
                                                                            <Drop Fn="trackDragDrop" />
                                                                        </Listeners>
                                                                    </ext:GridView>
                                                                </View>
                                                                <SelectionModel>
                                                                    <ext:RowSelectionModel
                                                                        runat="server"
                                                                        ID="GridRowSelectGrid"
                                                                        Mode="Single">
                                                                        <Listeners>
                                                                            <Select Fn="Grid_RowSelectGrid" />
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
                                                            </ext:GridPanel>
                                                        </Items>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>

                            <%-- PANEL LATERAL DESPLEGABLE--%>

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
                                <Items>
                                    <%-- PANEL MORE INFO--%>


                                    <ext:Panel runat="server" ID="pnMoreInfo" Hidden="true" Cls="tbGrey grdIntoAside" AnchorVertical="100%" AnchorHorizontal="100%" OverflowHandler="Scroller" OverflowY="Auto">

                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="Toolbar3" Cls="tbGrey" Dock="Top" Padding="0">
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
