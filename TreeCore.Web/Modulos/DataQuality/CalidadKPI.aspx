<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalidadKPI.aspx.cs" Inherits="TreeCore.ModCalidad.CalidadKPI" %>

<%@ Register Src="/Componentes/toolbarFiltros.ascx" TagName="toolbarFiltros" TagPrefix="local" %>
<%@ Register Src="~/Componentes/ProgramadorTareasCron.ascx" TagName="Programador" TagPrefix="local" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <script type="text/javascript" src="../../Scripts/cRonstrue/cronstrue.min.js"></script>
    <script type="text/javascript" src="../../Scripts/cRonstrue/cronstrue-i18n.min.js"></script>

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdKPISeleccionado" runat="server" />
            <ext:Hidden ID="hdGroupSeleccionado" runat="server" />
            <ext:Hidden ID="hdNombreColumna" runat="server" />
            <ext:Hidden ID="hdTablaModeloDatos" runat="server" />
            <ext:Hidden ID="hdToggleFiltro" runat="server" />
            <ext:Hidden ID="hdRuleTablaModeloDatoID" runat="server" />
            <ext:Hidden ID="hdColumnaReglaID" runat="server" />
            <ext:Hidden ID="hdOperadorReglaID" runat="server" />
            <ext:Hidden ID="hdColumnaFiltroID" runat="server" />
            <ext:Hidden ID="hdOperadorFiltroID" runat="server" />
            <ext:Hidden ID="hdLocale" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO STORES --%>

            <ext:Store runat="server"
                ID="storeDQTablasPaginas"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeDQTablasPaginas_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="DQTablaPaginaID">
                        <Fields>
                            <ext:ModelField Name="DQTablaPaginaID" Type="Int" />
                            <ext:ModelField Name="Alias" Type="String" />
                            <ext:ModelField Name="ClaveRecurso" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="TablaModeloDatosID" Type="Int" />
                            <ext:ModelField Name="NombreTabla" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Alias" Direction="DESC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTablasModelosDatos"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeTablasModelosDatos_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="TablaModeloDatosID">
                        <Fields>
                            <ext:ModelField Name="TablaModeloDatosID" Type="Int" />
                            <ext:ModelField Name="ClaveRecurso" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="Indice" Type="String" />
                            <ext:ModelField Name="NombreTabla" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="ClaveRecurso" Direction="DESC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeColumnasModelosDatos"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeColumnasModelosDatos_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="ColumnaModeloDatosID">
                        <Fields>
                            <ext:ModelField Name="ColumnaModeloDatosID" Type="Int" />
                            <ext:ModelField Name="NombreColumna" Type="String" />
                            <ext:ModelField Name="ClaveRecursoColumna" Type="String" />
                            <ext:ModelField Name="TipoDato" Type="String" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreColumna" Direction="DESC" />
                </Sorters>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeDQKpisFiltros"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeDQKpisFiltros_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrillaFiltro" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="DQKpiFiltroID">
                        <Fields>
                            <ext:ModelField Name="DQKpiFiltroID" Type="Int" />
                            <ext:ModelField Name="DQKpiID" Type="Int" />
                            <ext:ModelField Name="ColumnaModeloDatoID" Type="Int" />
                            <ext:ModelField Name="NombreColumna" Type="String" />
                            <ext:ModelField Name="Operador" Type="String" />
                            <ext:ModelField Name="ClaveRecursoColumna" Type="String" />
                            <ext:ModelField Name="TipoDatoOperadorID" Type="Int" />
                            <ext:ModelField Name="Valor" Type="String" />
                            <ext:ModelField Name="Codigo" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="IsDataTable" Type="Boolean" />
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTiposDatosOperadores"
                RemotePaging="false"
                AutoLoad="false"
                OnReadData="storeTiposDatosOperadores_Refresh"
                RemoteSort="true"
                PageSize="20">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="TipoDatoOperadorID">
                        <Fields>
                            <ext:ModelField Name="TipoDatoOperadorID" Type="Int" />
                            <ext:ModelField Name="TipoDatoID" Type="Int" />
                            <ext:ModelField Name="Operador" Type="String" />
                            <ext:ModelField Name="ClaveRecurso" Type="String" />
                            <ext:ModelField Name="Nombre" Type="String" />
                            <ext:ModelField Name="Activo" Type="Boolean" />
                            <ext:ModelField Name="RequiereValor" Type="Boolean" />
                            <ext:ModelField Name="Codigo" Type="String" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Nombre" Direction="DESC" />
                </Sorters>
            </ext:Store>

            <ext:Store
                ID="storeTiposDinamicosFiltros"
                runat="server"
                AutoLoad="false"
                OnReadData="storeTiposDinamicosFiltros_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Id">
                        <Fields>
                            <ext:ModelField Name="Id" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <DataChanged Fn="beforeLoadCmbColumnaFiltro" />
                </Listeners>
            </ext:Store>

            <ext:Store
                ID="storeTiposDinamicosReglas"
                runat="server"
                AutoLoad="false"
                OnReadData="storeTiposDinamicos_Refresh">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="Id">
                        <Fields>
                            <ext:ModelField Name="Id" />
                            <ext:ModelField Name="Name" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Listeners>
                    <DataChanged Fn="beforeLoadCmbColumna" />
                </Listeners>
            </ext:Store>


            <%-- FIN STORES --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%-- INICIO WINDOWS --%>

            <ext:Window ID="winGestion"
                runat="server"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Modal="true"
                Width="750"
                Height="600"
                Closable="false"
                Hidden="true"
                Layout="FitLayout"
                Cls="winForm-resp winGestion">
                <Items>
                    <ext:FormPanel
                        runat="server"
                        ID="formGestn"
                         WidthSpec="100%"
                        OverflowY="Auto"
                        Cls="formGris formResp"
                        Layout="VBoxLayout">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Container runat="server"
                                ID="ctForm"
                                MonitorPoll="500"
                                MonitorValid="true"
                                ActiveIndex="2"
                                Border="false"
                                Cls="pnNavVistas pnVistasForm"
                                Height="50"
                                AriaRole="navigation">
                                <Items>
                                    <ext:Container runat="server"
                                        ID="cntNavVistasFormKPI"
                                        Cls="nav-vistas nav-vistasObligatorio ctForm-tab-resp-col3"
                                        ActiveIndex="1"
                                        Height="50"
                                        ActiveItem="1">
                                        <Items>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkFormKPI"
                                                meta:resourceKey="lnkKPI"
                                                Cls="lnk-navView lnk-noLine navActivo"
                                                Text="<%$ Resources:Comun, strKPI %>">
                                                <Listeners>
                                                    <Click Fn="showFormsFormKPI" />
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                            <ext:HyperlinkButton runat="server"
                                                ID="lnkFormFrecuencia"
                                                meta:resourceKey="lnkFrecuencia"
                                                Cls="lnk-navView lnk-noLine"
                                                Text="<%$ Resources:Comun, strFrecuencia %>">
                                                <Listeners>
                                                    <Click Fn="showFormsFormKPI" />
                                                </Listeners>
                                            </ext:HyperlinkButton>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="formKPI"
                                Hidden="false"
                                HeightSpec="80%"
                                Scrollable="Vertical"
                                OverflowY="Auto"
                                Cls="winGestion-paneles ctForm-resp ctForm-resp-col3">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtName"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        FieldLabel="<%$ Resources:Comun, strNombre %>"
                                        ValidationGroup="FORM"
                                        CausesValidation="true"
                                        EmptyText="">
                                        <Listeners>
                                            <Change Fn="ValidarFormulario" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:ComboBox
                                        ID="cmbCategory"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strCategoria %>"
                                        Mode="Local"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        LabelAlign="Top"
                                        DisplayField="DQCategoria"
                                        ValueField="DQCategoriaID"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        Editable="true"
                                        QueryMode="Local"
                                        Hidden="false">
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeCategorias"
                                                AutoLoad="false"
                                                OnReadData="storeCategorias_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="DQCategoriaID">
                                                        <Fields>
                                                            <ext:ModelField Name="DQCategoriaID" Type="Int" />
                                                            <ext:ModelField Name="DQCategoria" Type="String" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                    <ext:DataSorter Property="Nombre" Direction="ASC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarCategoria" />
                                            <TriggerClick Fn="RecargarCategoria" />
                                            <Change Fn="ValidarFormulario" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox
                                        ID="cmbTraffic"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strSemaforo %>"
                                        Mode="Local"
                                        AllowBlank="false"
                                        LabelAlign="Top"
                                        ValidationGroup="FORM"
                                        DisplayField="DQSemaforo"
                                        ValueField="DQSemaforoID"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        Editable="true"
                                        QueryMode="Local"
                                        Hidden="false">
                                        <Store>
                                            <ext:Store runat="server"
                                                ID="storeSemaforos"
                                                AutoLoad="false"
                                                OnReadData="storeSemaforos_Refresh"
                                                RemoteSort="false">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Model>
                                                    <ext:Model runat="server" IDProperty="DQSemaforoID">
                                                        <Fields>
                                                            <ext:ModelField Name="DQSemaforoID" Type="Int" />
                                                            <ext:ModelField Name="DQSemaforo" Type="String" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                                <Sorters>
                                                    <ext:DataSorter Property="DQSemaforo" Direction="ASC" />
                                                </Sorters>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Fn="SeleccionarSemaforo" />
                                            <TriggerClick Fn="RecargarSemaforo" />
                                            <Change Fn="ValidarFormulario" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox
                                        ID="cmbTablasPaginas"
                                        runat="server"
                                        FieldLabel="<%$ Resources:Comun, strTabla %>"
                                        Mode="Local"
                                        StoreID="storeDQTablasPaginas"
                                        AllowBlank="false"
                                        LabelAlign="Top"
                                        ValidationGroup="FORM"
                                        DisplayField="ClaveRecurso"
                                        ValueField="DQTablaPaginaID"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        Editable="true"
                                        QueryMode="Local"
                                        Hidden="false">
                                        <Listeners>
                                            <Select Fn="SeleccionarTablasPaginas" />
                                            <Change Fn="ValidarFormulario" />
                                            <TriggerClick Fn="RecargarTablasPaginas" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                                IconCls="ico-reload"
                                                Hidden="true"
                                                Weight="-1" />
                                        </Triggers>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server"
                                ID="FormFrecuencia"
                                Hidden="true"
                                BodyStyle="padding:10px;"
                                OverflowY="Auto"
                                Cls="winGestion-paneles ctForm-resp">
                                <Items>
                                    <ext:FieldContainer runat="server"
                                        ID="contenedorCategorias"
                                        Cls="containerAttributes ctForm-resp ctForm-content-resp-col3">
                                        <Content>
                                            <local:Programador ID="ProgramadorKPI"
                                                runat="server" />
                                        </Content>
                                    </ext:FieldContainer>
                                </Items>
                            </ext:Container>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="ValidarFormulario(valid);" />
                        </Listeners>
                        <DockedItems>
                            <ext:Toolbar runat="server"
                                ID="Toolbar5"
                                Cls="tbGrey"
                                Dock="Bottom"
                                PaddingSpec="0 10 16 10">
                                <Items>
                                    <ext:ToolbarFill Flex="8"></ext:ToolbarFill>
                                    <ext:Button runat="server"
                                        ID="btnCancelar"
                                        Cls="btn-secondary"
                                        MinWidth="120"
                                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                        Focusable="false"
                                        PressedCls="none"
                                        Flex="2"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="#{winGestion}.hide();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnGuardar"
                                        Cls="btn-ppal"
                                        Disabled="true"
                                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                        Focusable="false"
                                        PressedCls="none"
                                        Hidden="false">
                                        <Listeners>
                                            <Click Handler="winGestionBotonGuardar();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Window ID="winGestionFiltro"
                runat="server"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="372"
                Height="420"
                MaxWidth="372"
                Modal="true"
                Centered="true"
                Cls="winForm-respSimple winGestion-Calidad"
                Scrollable="Vertical"
                Hidden="true">
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="Toolbar2"
                        Cls="tbGrey"
                        Dock="Bottom"
                        PaddingSpec="0 10 16 10">
                        <Items>
                            <ext:ToolbarFill Flex="8"></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnCancelarFiltro"
                                Cls="btn-secondary"
                                MinWidth="120"
                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="2"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="#{winGestionFiltro}.hide();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnGuardarFiltro"
                                Cls="btn-ppal btnBold "
                                MinWidth="120"
                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Disabled="true"
                                Flex="2"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="winGestionBotonGuardarFiltro();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel
                        runat="server"
                        ID="formGestionFiltro"
                        Cls="formGris"
                        PaddingSpec="0 15px"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:ComboBox
                                ID="cmbColumnasFiltro"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strColumna %>"
                                Mode="Local"
                                StoreID="storeColumnasModelosDatos"
                                AllowBlank="false"
                                LabelAlign="Top"
                                PaddingSpec="10 10 0 10"
                                DisplayField="ClaveRecursoColumna"
                                ValueField="ColumnaModeloDatosID"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                QueryMode="Local"
                                Editable="true"
                                Hidden="false">
                                <Listeners>
                                    <Select Fn="SeleccionarColumnasFiltro" />
                                    <TriggerClick Fn="RecargarColumnasFiltro" />
                                    <Change Fn="FormularioValidoGestionFiltro" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:ComboBox
                                ID="cmbOperadorFiltro"
                                Cls="cmbOperator"
                                runat="server"
                                StoreID="storeTiposDatosOperadores"
                                Disabled="true"
                                FieldLabel="<%$ Resources:Comun, strOperador %>"
                                Mode="Local"
                                AllowBlank="false"
                                DisplayField="Nombre"
                                ValueField="TipoDatoOperadorID"
                                LabelAlign="Top"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                QueryMode="Local"
                                Editable="true"
                                Hidden="false">
                                <Listeners>
                                    <Select Fn="SeleccionarOperadorFiltro" />
                                    <TriggerClick Fn="RecargarOperadorFiltro" />
                                    <Change Fn="FormularioValidoGestionFiltro" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:TextField runat="server"
                                ID="txtValorFiltro"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                Hidden="true"
                                Cls="pnForm"
                                FieldLabel="<%$ Resources:Comun, strValor %>"
                                PaddingSpec="10 10 0 10"
                                EmptyText="">
                                <Listeners>
                                    <Change Fn="FormularioValidoGestionFiltro" />
                                </Listeners>
                            </ext:TextField>
                            <ext:DateField runat="server"
                                ID="dateValorFiltro"
                                AllowBlank="false"
                                FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                LabelAlign="Top"
                                PaddingSpec="10 10 0 10"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                SubmitFormat="<%$ Resources:Comun, FormatFecha %>"
                                Cls="pnForm"
                                Hidden="true"
                                Format="<%$ Resources:Comun, FormatFecha %>">
                                <Listeners>
                                    <Change Fn="FormularioValidoGestionFiltro" />
                                </Listeners>
                            </ext:DateField>
                            <ext:NumberField runat="server"
                                ID="numberValorFiltro"
                                AllowBlank="false"
                                FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                LabelAlign="Top"
                                PaddingSpec="10 10 0 10"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                Cls="pnForm"
                                Hidden="true">
                                <Listeners>
                                    <Change Fn="FormularioValidoGestionFiltro" />
                                </Listeners>
                            </ext:NumberField>
                            <ext:Button runat="server"
                                ID="chkValorFiltro"
                                Text="<%$ Resources:Comun, jsActivar %>"
                                EnableToggle="true"
                                MinWidth="145"
                                Pressed="false"
                                TextAlign="Left"
                                Focusable="false"
                                OverCls="none"
                                PressedCls="btnActivarDesactivarV2Pressed"
                                Cls="btnActivarDesactivarV2 checkCalidadKPI"
                                Hidden="true">
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnToggleFiltro"
                                Width="50"
                                EnableToggle="true"
                                Pressed="false"
                                Hidden="true"
                                Cls="btn-toggleGrid"
                                AriaLabel="<%$ Resources:Comun, strColumna %>"
                                ToolTip="<%$ Resources:Comun, strColumna %>">
                                <Listeners>
                                    <Click Fn="cambiarComboFiltro" />
                                </Listeners>
                            </ext:Button>
                            <ext:ComboBox runat="server"
                                ID="cmbTiposDinamicosFiltro"
                                Hidden="true"
                                AllowBlank="false"
                                PaddingSpec="10 10 0 10"
                                FieldLabel="<%$ Resources:Comun, strTipoDinamico %>"
                                LabelAlign="Top"
                                DisplayField="Name"
                                EmptyText="<%$ Resources:Comun, strTipoDinamico %>"
                                Flex="1"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                StoreID="storeTiposDinamicosFiltros"
                                Cls="pnForm"
                                ValueField="Id"
                                QueryMode="Local"
                                Editable="true">
                                <Listeners>
                                    <Select Fn="SeleccionarTiposDinamicosFiltros" />
                                    <TriggerClick Fn="RecargarTiposDinamicosFiltros" />
                                    <Change Fn="FormularioValidoGestionFiltro" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:MultiCombo runat="server"
                                ID="cmbMultiTiposDinamicosFiltro"
                                Hidden="true"
                                AllowBlank="false"
                                PaddingSpec="10 10 0 10"
                                FieldLabel="<%$ Resources:Comun, strTipoDinamico %>"
                                LabelAlign="Top"
                                DisplayField="Name"
                                MultiSelect="true"
                                EmptyText="<%$ Resources:Comun, strTipoDinamico %>"
                                Flex="1"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                Cls="pnForm"
                                StoreID="storeTiposDinamicosFiltros"
                                ValueField="Id"
                                QueryMode="Local"
                                Editable="false">
                                <Listeners>
                                    <Select Fn="SeleccionarMultiTiposDinamicosFiltros" />
                                    <TriggerClick Fn="RecargarMultiTiposDinamicosFiltros" />
                                    <Change Fn="FormularioValidoGestionFiltro" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:MultiCombo>
                            <ext:ComboBox
                                ID="cmbTablasPaginasAsociacionFiltro"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strTabla %>"
                                StoreID="storeTablasModelosDatos"
                                AllowBlank="false"
                                LabelAlign="Top"
                                Flex="1"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                Cls="pnForm"
                                QueryMode="Local"
                                Editable="true"
                                PaddingSpec="10 10 0 10"
                                DisplayField="ClaveRecurso"
                                ValueField="TablaModeloDatosID"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                Hidden="true">
                                <Listeners>
                                    <Select Fn="SeleccionarTablasPaginasAsociacionFiltros" />
                                    <TriggerClick Fn="RecargarTablasPaginasAsociacionFiltros" />
                                    <Change Fn="FormularioValidoGestionFiltro" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoGestionFiltro(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Window ID="winGestionCondicion"
                runat="server"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="372"
                Height="275"
                MaxWidth="372"
                Modal="true"
                Centered="true"
                Cls="winForm-respSimple winGestion-Calidad"
                Scrollable="Vertical"
                Hidden="true">
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
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
                                ID="btnCancelarCondition"
                                Cls="btn-secondary"
                                MinWidth="120"
                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="2"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="#{winGestionCondicion}.hide();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnGuardarCondition"
                                Cls="btn-ppal btnBold "
                                MinWidth="120"
                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Disabled="true"
                                Flex="2"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="winGestionBotonGuardarCondition();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel
                        runat="server"
                        ID="formGestionCondition"
                        Cls="formGris"
                        PaddingSpec="0 15px"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:TextField runat="server"
                                ID="txtNameCondition"
                                LabelAlign="Top"
                                AllowBlank="false"
                                FieldLabel="<%$ Resources:Comun, strNombre %>"
                                PaddingSpec="10 10 0 10"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                EmptyText="">
                            </ext:TextField>
                            <ext:ComboBox
                                ID="cmbGrupos"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strGrupo %>"
                                Mode="Local"
                                AllowBlank="false"
                                LabelAlign="Top"
                                PaddingSpec="10 10 0 10"
                                DisplayField="DQGroup"
                                ValueField="DQGroupID"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                QueryMode="Local"
                                Editable="true"
                                Hidden="false">
                                <Store>
                                    <ext:Store runat="server"
                                        ID="storeDQGroups"
                                        AutoLoad="false"
                                        OnReadData="storeDQGroups_Refresh"
                                        RemoteSort="false">
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model runat="server" IDProperty="DQGroupID">
                                                <Fields>
                                                    <ext:ModelField Name="DQGroupID" Type="Int" />
                                                    <ext:ModelField Name="DQGroup" Type="String" />
                                                    <ext:ModelField Name="Activo" Type="Boolean" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Sorters>
                                            <ext:DataSorter Property="DQGroup" Direction="ASC" />
                                        </Sorters>
                                    </ext:Store>
                                </Store>
                                <Listeners>
                                    <Select Fn="SeleccionarGrupo" />
                                    <TriggerClick Fn="RecargarGrupo" />
                                    <Change Fn="FormularioValidoGestionCondition" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoGestionCondition(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Window ID="winGestionRule"
                runat="server"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="372"
                MaxWidth="372"
                Height="420"
                Modal="true"
                Centered="true"
                Cls="winForm-respSimple winGestion-Calidad"
                Scrollable="Vertical"
                OverflowY="Scroll"
                Hidden="true">
                <Listeners>
                    <Resize Handler="winFormCenterSimple(this);"></Resize>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server"
                        ID="Toolbar11"
                        Cls="tbGrey"
                        Dock="Bottom"
                        PaddingSpec="0 10 16 10">
                        <Items>
                            <ext:ToolbarFill Flex="8"></ext:ToolbarFill>
                            <ext:Button runat="server"
                                ID="btnCancelarRegla"
                                Cls="btn-secondary"
                                MinWidth="120"
                                Text="<%$ Resources:Comun, btnCancelar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Flex="2"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="#{winGestionRule}.hide();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnGuardarReglas"
                                Cls="btn-ppal btnBold "
                                MinWidth="120"
                                Text="<%$ Resources:Comun, btnGuardar.Text %>"
                                Focusable="false"
                                PressedCls="none"
                                Disabled="true"
                                Flex="2"
                                Hidden="false">
                                <Listeners>
                                    <Click Handler="winGestionBotonGuardarRule();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>
                    <ext:FormPanel
                        runat="server"
                        ID="formGestionReglas"
                        Cls="formGris"
                        PaddingSpec="0 15px"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <Items>
                            <ext:ComboBox
                                ID="cmbTablasPaginasReglas"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strTabla %>"
                                StoreID="storeDQTablasPaginas"
                                AllowBlank="false"
                                LabelAlign="Top"
                                PaddingSpec="10 10 0 10"
                                DisplayField="ClaveRecurso"
                                ValueField="DQTablaPaginaID"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                QueryMode="Local"
                                Editable="true"
                                Hidden="false">
                                <Listeners>
                                    <Select Fn="SeleccionarTablasPaginasRule" />
                                    <TriggerClick Fn="RecargarTablasPaginasRule" />
                                    <Change Fn="FormularioValidoGestionReglas" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:ComboBox
                                ID="cmbColumnasReglas"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strColumna %>"
                                Mode="Local"
                                StoreID="storeColumnasModelosDatos"
                                AllowBlank="false"
                                Disabled="true"
                                LabelAlign="Top"
                                PaddingSpec="10 10 0 10"
                                DisplayField="ClaveRecursoColumna"
                                ValueField="ColumnaModeloDatosID"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                QueryMode="Local"
                                Editable="true"
                                Hidden="false">
                                <Listeners>
                                    <Select Fn="SeleccionarColumnasReglas" />
                                    <TriggerClick Fn="RecargarColumnasReglas" />
                                    <Change Fn="FormularioValidoGestionReglas" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:ComboBox
                                ID="cmbOperadorReglas"
                                Cls="cmbOperator"
                                runat="server"
                                Disabled="true"
                                StoreID="storeTiposDatosOperadores"
                                FieldLabel="<%$ Resources:Comun, strOperador %>"
                                Mode="Local"
                                DisplayField="Nombre"
                                ValueField="TipoDatoOperadorID"
                                AllowBlank="false"
                                LabelAlign="Top"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                QueryMode="Local"
                                Editable="true"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                Hidden="false">
                                <Listeners>
                                    <Select Fn="SeleccionarOperadorReglas" />
                                    <TriggerClick Fn="RecargarOperadorReglas" />
                                    <Change Fn="FormularioValidoGestionReglas" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:TextField runat="server"
                                ID="txtValorRule"
                                PaddingSpec="10 10 0 10"
                                LabelAlign="Top"
                                Cls="pnForm"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                Hidden="true"
                                FieldLabel="<%$ Resources:Comun, strValor %>"
                                EmptyText="">
                                <Listeners>
                                    <Change Fn="FormularioValidoGestionReglas" />
                                </Listeners>
                            </ext:TextField>
                            <ext:DateField runat="server"
                                ID="dateValorRule"
                                FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                LabelAlign="Top"
                                PaddingSpec="10 10 0 10"
                                ValidationGroup="FORM"
                                Cls="pnForm"
                                CausesValidation="true"
                                Hidden="true"
                                Format="<%$ Resources:Comun, FormatFecha %>">
                                <Listeners>
                                    <Change Fn="FormularioValidoGestionReglas" />
                                </Listeners>
                            </ext:DateField>
                            <ext:NumberField runat="server"
                                ID="numberValorRule"
                                FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                LabelAlign="Top"
                                PaddingSpec="10 10 0 10"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                Cls="pnForm"
                                Hidden="true">
                                <Listeners>
                                    <Change Fn="FormularioValidoGestionReglas" />
                                </Listeners>
                            </ext:NumberField>
                            <ext:Button runat="server"
                                ID="chkValorRule"
                                Text="<%$ Resources:Comun, jsActivar %>"
                                EnableToggle="true"
                                MinWidth="145"
                                Pressed="false"
                                TextAlign="Left"
                                Focusable="false"
                                OverCls="none"
                                PressedCls="btnActivarDesactivarV2Pressed"
                                Cls="btnActivarDesactivarV2 checkCalidadKPI"
                                Hidden="true">
                            </ext:Button>
                            <ext:Button runat="server"
                                ID="btnToggleTabRule"
                                Width="50"
                                EnableToggle="true"
                                Hidden="true"
                                Pressed="false"
                                Cls="btn-toggleGrid"
                                AriaLabel="<%$ Resources:Comun, strColumna %>"
                                ToolTip="<%$ Resources:Comun, strColumna %>">
                                <Listeners>
                                    <Click Fn="cambiarComboRule" />
                                </Listeners>
                            </ext:Button>
                            <ext:ComboBox runat="server"
                                ID="cmbTiposDinamicosReglas"
                                Hidden="true"
                                PaddingSpec="10 10 0 10"
                                FieldLabel="<%$ Resources:Comun, strTipoDinamico %>"
                                LabelAlign="Top"
                                DisplayField="Name"
                                EmptyText="<%$ Resources:Comun, strTipoDinamico %>"
                                Flex="1"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                StoreID="storeTiposDinamicosReglas"
                                Cls="pnForm"
                                ValueField="Id"
                                QueryMode="Local"
                                Editable="true">
                                <Listeners>
                                    <Select Fn="SeleccionarTiposDinamicosReglas" />
                                    <TriggerClick Fn="RecargarTiposDinamicosReglas" />
                                    <Change Fn="FormularioValidoGestionReglas" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                            <ext:MultiCombo runat="server"
                                ID="cmbMultiTiposDinamicosReglas"
                                Hidden="true"
                                PaddingSpec="10 10 0 10"
                                FieldLabel="<%$ Resources:Comun, strTipoDinamico %>"
                                LabelAlign="Top"
                                DisplayField="Name"
                                MultiSelect="true"
                                EmptyText="<%$ Resources:Comun, strTipoDinamico %>"
                                Flex="1"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                StoreID="storeTiposDinamicosReglas"
                                Cls="pnForm"
                                ValueField="Id"
                                QueryMode="Local"
                                Editable="false">
                                <Listeners>
                                    <Select Fn="SeleccionarMultiTiposDinamicosReglas" />
                                    <TriggerClick Fn="RecargarMultiTiposDinamicosReglas" />
                                    <Change Fn="FormularioValidoGestionReglas" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:MultiCombo>
                            <ext:ComboBox
                                ID="cmbTablasPaginasAsociacionRule"
                                runat="server"
                                FieldLabel="<%$ Resources:Comun, strTabla %>"
                                StoreID="storeTablasModelosDatos"
                                AllowBlank="false"
                                Flex="1"
                                LabelAlign="Top"
                                ValidationGroup="FORM"
                                CausesValidation="true"
                                PaddingSpec="10 10 0 10"
                                Cls="pnForm"
                                QueryMode="Local"
                                Editable="true"
                                DisplayField="ClaveRecurso"
                                ValueField="TablaModeloDatosID"
                                EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                Hidden="true">
                                <Listeners>
                                    <Select Fn="SeleccionarTablasPaginasAsociacionRule" />
                                    <TriggerClick Fn="RecargarTablasPaginasAsociacionRule" />
                                    <Change Fn="FormularioValidoGestionReglas" />
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1" />
                                </Triggers>
                            </ext:ComboBox>
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoGestionReglas(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <%-- FIN WINDOWS --%>

            <ext:Viewport runat="server"
                ID="MainVwP"
                OverflowY="auto"
                Layout="FitLayout">
                <Content>
                    <ext:Button runat="server"
                        ID="btnCollapseAsRClosed"
                        Cls="btn-trans btnCollapseAsRClosedv3"
                        OnDirectClick="ShowHidePnAsideR"
                        Handler="hidePnLite();"
                        Disabled="false"
                        Hidden="false">
                    </ext:Button>
                </Content>

                <Items>
                    <%-----------------------Panel WRAP TODA LA PAGINA---------------------%>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <Items>

                            <%-------------TENER EN CUENTA QUE AL CENTER MAIN SE LE PUEDEN ACOPLAR SEGMENTOS PLEGABLES CON LAS "REGIONS"-------------%>


                            <%-- PANEL CENTRAL--%>

                            <ext:Panel ID="CenterPanelMain"
                                Visible="true"
                                runat="server"
                                Header="false"
                                MarginSpec="0 20 20 20"
                                Border="false"
                                Region="Center"
                                Layout="FitLayout"
                                Cls="visorInsidePn pnCentralMain"
                                BodyCls="tbGrey">
                                <DockedItems>
                                    <ext:Container runat="server"
                                        ID="WrapAlturaCabecera"
                                        MinHeight="60"
                                        Layout="VBoxLayout"
                                        Dock="Top">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:Toolbar runat="server"
                                                ID="tbTitulo"
                                                Dock="Top"
                                                Cls="tbGrey tbTitleAlignBot tbNoborder"
                                                Hidden="false"
                                                Flex="1">
                                                <Items>
                                                    <ext:Label runat="server"
                                                        ID="lbltituloPrincipal"
                                                        Cls="TituloCabecera"
                                                        Text="<%$ Resources:Comun, strCalidadKPI %>"
                                                        Height="25" />
                                                </Items>
                                            </ext:Toolbar>

                                            <%-- TABS NAVEGACION--%>

                                            <ext:Toolbar runat="server"
                                                ID="tbNavNAside"
                                                Dock="Top"
                                                Cls="tbGrey tbNoborder"
                                                Hidden="false"
                                                PaddingSpec="10 10 10 10"
                                                OverflowHandler="Scroller"
                                                Flex="1">
                                                <Items>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="tabKPI"
                                                        Hidden="false"
                                                        Cls="lnk-navView lnk-noLine navActivo"
                                                        Text="<%$ Resources:Comun, strCalidadKPI %>"
                                                        tabID="0">
                                                        <Listeners>
                                                            <Click Handler="showForms(this, '/Modulos/DataQuality/CalidadKpi.aspx')"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="tabResults"
                                                        Hidden="false"
                                                        Cls="lnk-navView lnk-noLine "
                                                        Text="<%$ Resources:Comun, strCalidadKPIResults %>"
                                                        tabID="1">
                                                        <Listeners>
                                                            <Click Handler="showForms(this, '/Modulos/DataQuality/CalidadKPIresults.aspx', 'CalidadKPIresults', '/Modulos/DataQuality/js/CalidadKPIresults.js')"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="tabReport"
                                                        Hidden="false"
                                                        Cls="lnk-navView lnk-noLine "
                                                        Text="<%$ Resources:Comun, strCalidadKPIReport %>"
                                                        tabID="2">
                                                        <Listeners>
                                                            <Click Handler="showForms(this, '/Modulos/DataQuality/CalidadKPIReport.aspx', 'CalidadKPIReport', '/Modulos/DataQualityjs/CalidadKPIReport.js')"></Click>
                                                        </Listeners>
                                                    </ext:HyperlinkButton>
                                                </Items>
                                                <Listeners>
                                                    <BeforeRender Fn="prepareTabs" />
                                                </Listeners>
                                            </ext:Toolbar>

                                        </Items>
                                    </ext:Container>
                                </DockedItems>
                                <Content>

                                    <ext:Panel runat="server"
                                        ID="wrapComponenteCentral"
                                        Layout="HBoxLayout"
                                        BodyCls="tbGrey"
                                        Cls="calidadTabPanel">
                                        <DockedItems>
                                            <ext:Toolbar runat="server"
                                                ID="tbFiltrosYSliders"
                                                Dock="Top"
                                                Cls="tbGrey tbNoborder "
                                                Hidden="false"
                                                Layout="HBoxLayout"
                                                Flex="1">
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
                                                                Handler="SliderMove('Prev');"
                                                                Disabled="true">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnNext"
                                                                IconCls="ico-next-w"
                                                                Cls="SliderBtn"
                                                                Handler="SliderMove('Next');"
                                                                Disabled="false">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Listeners>
                                            <AfterRender Handler="ControlSlider(this)"></AfterRender>
                                            <Resize Handler="ControlSlider(this)"></Resize>
                                        </Listeners>
                                        <LayoutConfig>
                                            <ext:HBoxLayoutConfig Align="Stretch"></ext:HBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:GridPanel
                                                Region="Center"
                                                Hidden="false"
                                                Flex="8"
                                                SelectionMemory="false"
                                                EnableColumnHide="false"
                                                Title=""
                                                runat="server"
                                                Header="false"
                                                ID="grdmain"
                                                Cls="gridPanel grdNoHeader "
                                                OverflowX="Hidden"
                                                OverflowY="Auto">
                                                <Listeners>
                                                    <AfterRender Handler="GridColHandler(this)"></AfterRender>
                                                    <Resize Handler="GridColHandler(this)"></Resize>
                                                    <AfterLayout Fn="addEventoLinkPaginaKPI" />
                                                </Listeners>
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbBase"
                                                        Dock="Top"
                                                        Cls="tlbGrid"
                                                        OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnAnadir"
                                                                Cls="btnAnadir"
                                                                ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                                                Handler="AgregarEditar();" />
                                                            <ext:Button runat="server"
                                                                ID="btnEditar"
                                                                Cls="btnEditar"
                                                                ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                Handler="MostrarEditar();" />
                                                            <ext:Button runat="server"
                                                                ID="btnEliminar"
                                                                Cls="btnEliminar"
                                                                ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                Handler="Eliminar();" />
                                                            <ext:Button runat="server"
                                                                ID="btnRefrescar"
                                                                Cls="btnRefrescar"
                                                                ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                Handler="Refrescar();" />
                                                            <ext:Button runat="server"
                                                                ID="btnDescargar"
                                                                Cls="btnDescargar"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Handler="ExportarDatos('CalidadKPI', hdCliID.value, #{grdmain}, App.btnActivos.pressed, '');" />
                                                            <ext:Button runat="server"
                                                                ID="btnActivar"
                                                                Cls="btnActivate"
                                                                ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                                Handler="Activar();" />
                                                            <ext:Button runat="server"
                                                                ID="btnTime"
                                                                Cls="btnTime"
                                                                ToolTip="<%$ Resources:Comun, jsEjecutar %>">
                                                                <Listeners>
                                                                    <Click Fn="ejecutarKPI" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnDetalleKPI"
                                                                ToolTip="<%$ Resources:Comun, strDetalleKPI %>"
                                                                Cls="btnDetalleKPI"
                                                                Handler="hideAsideRCalidad('panelFiltros');" />
                                                            <ext:ToolbarFill></ext:ToolbarFill>
                                                            <ext:Button runat="server"
                                                                Text="<%$ Resources:Comun, strMostrarInactivos %>"
                                                                EnableToggle="true"
                                                                ID="btnActivos"
                                                                Cls="btnActivarDesactivarV2"
                                                                PressedCls="btnActivarDesactivarV2Pressed"
                                                                MinWidth="145"
                                                                Pressed="false"
                                                                TextAlign="Left"
                                                                Focusable="false"
                                                                OverCls="none">
                                                                <Listeners>
                                                                    <Click Fn="Refrescar" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Container runat="server"
                                                        ID="tbfiltros"
                                                        Cls=""
                                                        Dock="Top">
                                                        <Content>
                                                            <local:toolbarFiltros
                                                                ID="cmpFiltro"
                                                                runat="server"
                                                                Stores="storePrincipal"
                                                                MostrarComboFecha="false"
                                                                Grid="grdmain"
                                                                MostrarBusqueda="false" />
                                                        </Content>
                                                    </ext:Container>
                                                </DockedItems>
                                                <Store>
                                                    <ext:Store runat="server"
                                                        ID="storePrincipal"
                                                        RemotePaging="false"
                                                        AutoLoad="true"
                                                        OnReadData="storePrincipal_Refresh"
                                                        RemoteSort="true"
                                                        shearchBox="cmpFiltro_txtSearch"
                                                        listNotPredictive="DQKpiID,CreadorID,FechaModificacion,Activo,TieneGrupo,TieneBloqueConfigurado,TieneElementoConfigurado,IsAnd"
                                                        PageSize="20">
                                                        <Listeners>
                                                            <BeforeLoad Fn="DeseleccionarGrilla" />
                                                            <DataChanged Fn="BuscadorPredictivo" />
                                                        </Listeners>
                                                        <Proxy>
                                                            <ext:PageProxy />
                                                        </Proxy>
                                                        <Model>
                                                            <ext:Model runat="server"
                                                                IDProperty="DQKpiID">
                                                                <Fields>
                                                                    <ext:ModelField Name="DQKpiID" Type="Int" />
                                                                    <ext:ModelField Name="DQKpi" Type="String" />
                                                                    <ext:ModelField Name="RutaPagina" Type="String" />
                                                                    <ext:ModelField Name="NombreCompleto" Type="String" />
                                                                    <ext:ModelField Name="CreadorID" Type="Int" />
                                                                    <ext:ModelField Name="FechaModificacion" Type="Date" />
                                                                    <ext:ModelField Name="Activo" Type="Boolean" />
                                                                    <ext:ModelField Name="DQCategoria" Type="String" />
                                                                    <ext:ModelField Name="DQSemaforo" Type="String" />
                                                                    <ext:ModelField Name="ClaveRecurso" Type="String" />
                                                                    <ext:ModelField Name="AliasTablaPagina" Type="String" />
                                                                    <ext:ModelField Name="TieneGrupo" Type="Boolean" />
                                                                    <ext:ModelField Name="TieneBloqueConfigurado" Type="Boolean" />
                                                                    <ext:ModelField Name="TieneElementoConfigurado" Type="Boolean" />
                                                                    <ext:ModelField Name="IsAnd" Type="Boolean" />
                                                                    <ext:ModelField Name="AliasModulo" Type="String" />
                                                                    <ext:ModelField Name="CronFormat" Type="String" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                        <Sorters>
                                                            <ext:DataSorter Property="DQKpi" Direction="ASC" />
                                                        </Sorters>
                                                    </ext:Store>
                                                </Store>
                                                <ColumnModel>
                                                    <Columns>
                                                        <ext:Column
                                                            ID="ColKPI"
                                                            runat="server"
                                                            Text="KPI"
                                                            Flex="2"
                                                            AlignTarget="Center"
                                                            MinWidth="110"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            DataIndex="DQKpi" />
                                                        <ext:Column
                                                            ID="colCategory"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strCategoria %>"
                                                            Flex="2"
                                                            AlignTarget="Center"
                                                            MinWidth="110"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            DataIndex="DQCategoria" />
                                                        <ext:Column
                                                            ID="colSemaforo"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strSemaforo %>"
                                                            Flex="2"
                                                            AlignTarget="Center"
                                                            MinWidth="110"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            DataIndex="DQSemaforo" />
                                                        <ext:Column
                                                            ID="colTabla"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strTabla %>"
                                                            Flex="2"
                                                            AlignTarget="Center"
                                                            MinWidth="110"
                                                            Sortable="true"
                                                            Hidden="false"
                                                            DataIndex="ClaveRecurso" />
                                                        <ext:Column
                                                            ID="ColActivo"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strActivo %>"
                                                            Flex="2"
                                                            MinWidth="60"
                                                            Sortable="true"
                                                            AlignTarget="Center"
                                                            Hidden="false"
                                                            DataIndex="Activo"
                                                            Align="Center">
                                                            <Renderer Fn="DefectoRender" />
                                                        </ext:Column>
                                                        <ext:Column runat="server"
                                                            ID="colCronFormat"
                                                            DataIndex="CronFormat"
                                                            Text="<%$ Resources:Comun, strFrecuencia %>"
                                                            Flex="2"
                                                            Sortable="false"
                                                            Hidden="false">
                                                            <Renderer Fn="renderFrecuenciaCronFormat" />
                                                        </ext:Column>
                                                        <ext:HyperlinkColumn
                                                            ID="coLaunchLink"
                                                            runat="server"
                                                            Text="<%$ Resources:Comun, strEnlace %>"
                                                            Flex="2"
                                                            AlignTarget="Center"
                                                            Sortable="true"
                                                            TdCls="linkColumn"
                                                            HrefPattern="#"
                                                            DataIndex=""
                                                            Align="Center">
                                                            <Renderer Fn="enlaceRender" />
                                                        </ext:HyperlinkColumn>
                                                        <ext:WidgetColumn ID="colMore"
                                                            runat="server"
                                                            Cls="col-More"
                                                            DataIndex=""
                                                            Align="Center"
                                                            AlignTarget="Center"
                                                            Text="<%$ Resources:Comun, strMas %>"
                                                            Hidden="false"
                                                            MinWidth="90"
                                                            Flex="99999">
                                                            <Widget>
                                                                <ext:Button ID="btnMore"
                                                                    runat="server"
                                                                    Width="60"
                                                                    OverCls="Over-btnMore"
                                                                    PressedCls="Pressed-none"
                                                                    FocusCls="Focus-none"
                                                                    Cls="btnColMore">
                                                                    <Listeners>
                                                                        <Click Fn="hideAsidePnMore" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Widget>
                                                        </ext:WidgetColumn>
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
                                                </Plugins>
                                                <BottomBar>
                                                    <ext:PagingToolbar runat="server"
                                                        Cls="PgToolBMainGrid"
                                                        ID="PagingToolbar2"
                                                        MaintainFlex="true"
                                                        Flex="8"
                                                        HideRefresh="true"
                                                        OverflowHandler="Scroller">
                                                        <Items>
                                                            <ext:ComboBox runat="server"
                                                                Cls="comboGrid"
                                                                ID="ComboBox9"
                                                                MaxWidth="65">
                                                                <Items>
                                                                    <ext:ListItem Text="10" />
                                                                    <ext:ListItem Text="20" />
                                                                    <ext:ListItem Text="30" />
                                                                    <ext:ListItem Text="40" />
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
                                                <%--<Listeners>
                                                    <AfterLayout Fn="addEventoLinkPagina" />
                                                </Listeners>--%>
                                            </ext:GridPanel>

                                            <ext:Panel runat="server"
                                                ID="pnFilters"
                                                Hidden="true"
                                                Flex="5"
                                                Layout="VBoxLayout"
                                                BodyCls="tbGrey">
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                </LayoutConfig>
                                                <Items>
                                                    <ext:GridPanel
                                                        Flex="1"
                                                        MarginSpec="0 10 5 10"
                                                        HideHeaders="false"
                                                        Region="Center"
                                                        Hidden="false"
                                                        SelectionMemory="false"
                                                        EnableColumnHide="false"
                                                        Title="<%$ Resources:Comun, strFiltros %>"
                                                        runat="server"
                                                        StoreID="storeDQKpisFiltros"
                                                        ID="gridFiltros"
                                                        Cls="gridPanel grdNoHeader"
                                                        OverflowX="Hidden"
                                                        OverflowY="Auto">
                                                        <DockedItems>
                                                            <ext:Toolbar runat="server"
                                                                ID="Toolbar13"
                                                                Dock="Top"
                                                                Cls="tlbGrid"
                                                                OverflowHandler="Scroller">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        MarginSpec="7 7 7 10"
                                                                        ID="btnAnadirFiltros"
                                                                        Cls="btnAnadir"
                                                                        Handler="AgregarEditarFiltro();"
                                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnEditarFiltros"
                                                                        Cls="btnEditar"
                                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                        Handler="MostrarEditarFiltro();">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnEliminarFiltros"
                                                                        Cls="btnEliminar"
                                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                        Handler="EliminarFiltro();">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnRefrescarFiltros"
                                                                        Cls="btnRefrescar"
                                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                        Handler="RefrescarFiltro();">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnActivarFiltros"
                                                                        Cls="btnActivate"
                                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                                        Handler="ActivarFiltro();">
                                                                    </ext:Button>
                                                                    <ext:ToolbarFill></ext:ToolbarFill>
                                                                    <ext:Button runat="server"
                                                                        Text="<%$ Resources:Comun, strMostrarInactivos %>"
                                                                        EnableToggle="true"
                                                                        ID="btnActivosFiltros"
                                                                        Cls="btnActivarDesactivarV2"
                                                                        PressedCls="btnActivarDesactivarV2Pressed"
                                                                        MinWidth="145"
                                                                        Pressed="false"
                                                                        TextAlign="Left"
                                                                        Focusable="false"
                                                                        OverCls="none">
                                                                        <Listeners>
                                                                            <Click Fn="RefrescarFiltro" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:TemplateColumn runat="server"
                                                                    DataIndex=""
                                                                    MenuDisabled="true"
                                                                    Text="<%$ Resources:Comun, strFiltros %>"
                                                                    Flex="4"
                                                                    AlignTarget="Center"
                                                                    MaxHeight="60">
                                                                    <Template runat="server">
                                                                        <Html>
                                                                            <tpl for=".">
                                                                                <div style="float: left;">
                                                                                    <p style=" line-height:10px;" class="titlespan3">
                                                                                        {ClaveRecursoColumna} {Operador} {Valor}
                                                                                        
                                                                                    </p>
                                                                                </div>
                                                                            </tpl>
                                                                        </Html>
                                                                    </Template>
                                                                    <Renderer Fn="asignarTraduccionFiltro" />
                                                                </ext:TemplateColumn>
                                                                <ext:Column runat="server"
                                                                    Flex="2"
                                                                    AlignTarget="Center"
                                                                    Text="<%$ Resources:Comun, btnActivos.ToolTip %>"
                                                                    ID="colActivoFiltros"
                                                                    Sortable="false"
                                                                    DataIndex="Activo"
                                                                    Align="Center">
                                                                    <Renderer Fn="DefectoRender" />
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server"
                                                                ID="GridRowSelectFiltro"
                                                                Mode="Single">
                                                                <Listeners>
                                                                    <Select Fn="Grid_RowSelectFiltro" />
                                                                </Listeners>
                                                            </ext:RowSelectionModel>
                                                        </SelectionModel>
                                                        <BottomBar>
                                                            <ext:PagingToolbar runat="server"
                                                                Cls="PgToolBMainGrid"
                                                                ID="PagingToolbar4"
                                                                MaintainFlex="true"
                                                                Flex="8"
                                                                HideRefresh="true"
                                                                OverflowHandler="Scroller">
                                                                <Items>
                                                                    <ext:ComboBox runat="server"
                                                                        Cls="comboGrid"
                                                                        ID="ComboBox3"
                                                                        MaxWidth="65">
                                                                        <Items>
                                                                            <ext:ListItem Text="10" />
                                                                            <ext:ListItem Text="20" />
                                                                            <ext:ListItem Text="30" />
                                                                            <ext:ListItem Text="40" />
                                                                        </Items>
                                                                        <SelectedItems>
                                                                            <ext:ListItem Value="20" />
                                                                        </SelectedItems>
                                                                        <Listeners>
                                                                            <Select Fn="handlePageSizeSelectFiltro" />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                </Items>
                                                            </ext:PagingToolbar>
                                                        </BottomBar>
                                                    </ext:GridPanel>
                                                </Items>
                                            </ext:Panel>

                                            <ext:Panel runat="server"
                                                ID="pnCondition"
                                                Hidden="true"
                                                Flex="5"
                                                Layout="VBoxLayout"
                                                BodyCls="tbGrey">
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                                                </LayoutConfig>
                                                <Items>
                                                    <ext:GridPanel
                                                        Flex="1"
                                                        MarginSpec="0 10 5 10"
                                                        HideHeaders="false"
                                                        Region="Center"
                                                        Hidden="false"
                                                        SelectionMemory="false"
                                                        EnableColumnHide="false"
                                                        Title="<%$ Resources:Comun, strCondiciones %>"
                                                        runat="server"
                                                        ID="gridCondition"
                                                        Cls="gridPanel"
                                                        OverflowX="Hidden"
                                                        OverflowY="Auto">
                                                        <DockedItems>
                                                            <ext:Toolbar runat="server"
                                                                ID="Toolbar3"
                                                                Dock="Top"
                                                                Cls="tlbGrid"
                                                                OverflowHandler="Scroller">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        MarginSpec="7 7 7 10"
                                                                        ID="btnAnadirCondition"
                                                                        Cls="btnAnadir"
                                                                        Handler="AgregarEditarCondition();"
                                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnEditarCondition"
                                                                        Cls="btnEditar"
                                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                        Handler="MostrarEditarCondition();">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnEliminarCondition"
                                                                        Cls="btnEliminar"
                                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                        Handler="EliminarCondition();">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnRefrescarCondition"
                                                                        Cls="btnRefrescar"
                                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                        Handler="RefrescarCondition();">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnActivarCondition"
                                                                        Cls="btnActivate"
                                                                        ToolTip="<%$ Resources:Comun, btnActivar.ToolTip %>"
                                                                        Handler="ActivarCondition()">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnToggleCondition"
                                                                        Width="50"
                                                                        EnableToggle="true"
                                                                        Pressed="false"
                                                                        Cls="btn-toggleGrid-OR-Small"
                                                                        AriaLabel="And"
                                                                        ToolTip="And">
                                                                        <Listeners>
                                                                            <Click Fn="cambiarConsultaKPI" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:ToolbarFill></ext:ToolbarFill>
                                                                    <ext:Button runat="server"
                                                                        Text="<%$ Resources:Comun, strMostrarInactivos %>"
                                                                        EnableToggle="true"
                                                                        ID="btnActivosCondition"
                                                                        Cls="btnActivarDesactivarV2"
                                                                        PressedCls="btnActivarDesactivarV2Pressed"
                                                                        MinWidth="145"
                                                                        Pressed="false"
                                                                        TextAlign="Left"
                                                                        Focusable="false"
                                                                        OverCls="none"
                                                                        Handler="RefrescarCondition();" />
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <Store>
                                                            <ext:Store runat="server"
                                                                ID="storeDQKpiGroups"
                                                                RemotePaging="false"
                                                                AutoLoad="false"
                                                                OnReadData="storeDQKpiGroups_Refresh"
                                                                RemoteSort="true"
                                                                PageSize="20">
                                                                <Listeners>
                                                                    <BeforeLoad Fn="DeseleccionarGrillaCondition" />
                                                                </Listeners>
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Model>
                                                                    <ext:Model runat="server"
                                                                        IDProperty="DQKpiGroupID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="DQKpiGroupID" Type="Int" />
                                                                            <ext:ModelField Name="DQKpiID" Type="Int" />
                                                                            <ext:ModelField Name="FechaModificacion" Type="Date" />
                                                                            <ext:ModelField Name="Activo" Type="Boolean" />
                                                                            <ext:ModelField Name="DQGroup" Type="String" />
                                                                            <ext:ModelField Name="NombreCondicion" Type="String" />
                                                                            <ext:ModelField Name="IsAnd" Type="Boolean" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:Store>
                                                        </Store>
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:TemplateColumn runat="server"
                                                                    DataIndex=""
                                                                    MenuDisabled="true"
                                                                    Text="<%$ Resources:Comun, strCondicion %>"
                                                                    Flex="4"
                                                                    AlignTarget="Center"
                                                                    MaxHeight="60">
                                                                    <Template runat="server">
                                                                        <Html>
                                                                            <tpl for=".">
                                                                                <div style="float: left;">
                                                                                    <p style=" line-height:10px;" class="titlespan3">{NombreCondicion}</p>
                                                                                    <p style=" line-height:10px;" class="titlespan4">{DQGroup}</p>
                                                                                </div>
                                                                            </tpl>
                                                                        </Html>
                                                                    </Template>
                                                                </ext:TemplateColumn>
                                                                <ext:DateColumn runat="server"
                                                                    Hidden="true"
                                                                    Text="<%$ Resources:Comun, strFechaModificacion %>"
                                                                    ID="colDate"
                                                                    Format="<%$ Resources:Comun, FormatFecha %>"
                                                                    AlignTarget="Center"
                                                                    DataIndex="FechaModificacion">
                                                                </ext:DateColumn>
                                                                <ext:Column runat="server"
                                                                    Flex="2"
                                                                    Text="<%$ Resources:Comun, btnActivos.ToolTip %>"
                                                                    ID="colActivos"
                                                                    AlignTarget="Center"
                                                                    DataIndex="Activo"
                                                                    Sortable="false"
                                                                    Align="Center">
                                                                    <Renderer Fn="DefectoRender" />
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server"
                                                                ID="GridRowSelectCondition"
                                                                Mode="Single">
                                                                <Listeners>
                                                                    <Select Fn="Grid_RowSelectCondition" />
                                                                </Listeners>
                                                            </ext:RowSelectionModel>
                                                        </SelectionModel>
                                                        <BottomBar>
                                                            <ext:PagingToolbar runat="server"
                                                                Cls="PgToolBMainGrid"
                                                                ID="PagingToolbar1"
                                                                MaintainFlex="true"
                                                                Flex="8"
                                                                HideRefresh="true"
                                                                OverflowHandler="Scroller">
                                                                <Items>
                                                                    <ext:ComboBox runat="server"
                                                                        Cls="comboGrid"
                                                                        ID="ComboBox1"
                                                                        MaxWidth="65">
                                                                        <Items>
                                                                            <ext:ListItem Text="10" />
                                                                            <ext:ListItem Text="20" />
                                                                            <ext:ListItem Text="30" />
                                                                            <ext:ListItem Text="40" />
                                                                        </Items>
                                                                        <SelectedItems>
                                                                            <ext:ListItem Value="20" />
                                                                        </SelectedItems>
                                                                        <Listeners>
                                                                            <Select Fn="handlePageSizeSelectCondition" />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                </Items>
                                                            </ext:PagingToolbar>
                                                        </BottomBar>
                                                    </ext:GridPanel>

                                                    <ext:GridPanel
                                                        Flex="1"
                                                        MarginSpec="5 10 0 10"
                                                        HideHeaders="false"
                                                        Region="Center"
                                                        Hidden="true"
                                                        Title="<%$ Resources:Comun, strReglas %>"
                                                        runat="server"
                                                        ID="gridRule"
                                                        SelectionMemory="false"
                                                        EnableColumnHide="false"
                                                        Cls="gridPanel"
                                                        OverflowX="Hidden"
                                                        OverflowY="Auto">
                                                        <DockedItems>
                                                            <ext:Toolbar runat="server"
                                                                ID="Toolbar12"
                                                                Dock="Top"
                                                                Cls="tlbGrid"
                                                                OverflowHandler="Scroller">
                                                                <Items>
                                                                    <ext:Button runat="server"
                                                                        MarginSpec="7 7 7 10"
                                                                        ID="btnAnadirRule"
                                                                        Cls="btnAnadir"
                                                                        Handler="AgregarEditarRule();"
                                                                        ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnEditarRule"
                                                                        Cls="btnEditar"
                                                                        ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                        Handler="MostrarEditarRule();">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnEliminarRule"
                                                                        Cls="btnEliminar"
                                                                        ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                        Handler="EliminarRule();">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnRefrescarRule"
                                                                        Cls="btnRefrescar"
                                                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                                                        Handler="RefrescarRule();">
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        ID="btnToggleRule"
                                                                        Width="50"
                                                                        EnableToggle="true"
                                                                        Pressed="false"
                                                                        Cls="btn-toggleGrid-OR-Small"
                                                                        ToolTip="Or">
                                                                        <Listeners>
                                                                            <Click Fn="cambiarConsultaCondition" />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:ToolbarFill />
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </DockedItems>
                                                        <Store>
                                                            <ext:Store runat="server"
                                                                ID="storeDQKpisGroupsRules"
                                                                RemotePaging="false"
                                                                AutoLoad="false"
                                                                OnReadData="storeDQKpisGroupsRules_Refresh"
                                                                RemoteSort="true"
                                                                PageSize="20">
                                                                <Listeners>
                                                                    <BeforeLoad Fn="DeseleccionarGrillaRule" />
                                                                </Listeners>
                                                                <Proxy>
                                                                    <ext:PageProxy />
                                                                </Proxy>
                                                                <Model>
                                                                    <ext:Model runat="server"
                                                                        IDProperty="DQKpiGroupRuleID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="DQKpiGroupRuleID" Type="Int" />
                                                                            <ext:ModelField Name="ColumnaModeloDatosID" Type="Int" />
                                                                            <ext:ModelField Name="NombreColumna" Type="String" />
                                                                            <ext:ModelField Name="NombreTabla" Type="String" />
                                                                            <ext:ModelField Name="Operador" Type="String" />
                                                                            <ext:ModelField Name="ClaveRecursoColumna" Type="String" />
                                                                            <ext:ModelField Name="TipoDatoOperadorID" Type="String" />
                                                                            <ext:ModelField Name="Codigo" Type="String" />
                                                                            <ext:ModelField Name="Activo" Type="Boolean" />
                                                                            <ext:ModelField Name="Valor" Type="String" />
                                                                            <ext:ModelField Name="IsDataTable" Type="Boolean" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:Store>
                                                        </Store>
                                                        <ColumnModel>
                                                            <Columns>
                                                                <ext:TemplateColumn runat="server"
                                                                    DataIndex=""
                                                                    MenuDisabled="true"
                                                                    Text="<%$ Resources:Comun, strReglas %>"
                                                                    Flex="4"
                                                                    AlignTarget="Center"
                                                                    MaxHeight="60">
                                                                    <Template runat="server">
                                                                        <Html>
                                                                            <tpl for=".">
                                                                                <div style="float: left;">
                                                                                    <p style=" line-height:10px;" class="titlespan3">
                                                                                        {ClaveRecursoColumna} {Operador} {Valor}
                                                                                    </p>
                                                                                </div>
                                                                            </tpl>
                                                                        </Html>
                                                                    </Template>
                                                                    <Renderer Fn="asignarTraduccionRule" />
                                                                </ext:TemplateColumn>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel runat="server"
                                                                ID="GridRowSelectRule"
                                                                Mode="Single">
                                                                <Listeners>
                                                                    <Select Fn="Grid_RowSelectRule" />
                                                                </Listeners>
                                                            </ext:RowSelectionModel>
                                                        </SelectionModel>
                                                        <BottomBar>
                                                            <ext:PagingToolbar runat="server"
                                                                Cls="PgToolBMainGrid"
                                                                ID="PagingToolbar3"
                                                                MaintainFlex="true"
                                                                Flex="8"
                                                                HideRefresh="true"
                                                                OverflowHandler="Scroller">
                                                                <Items>
                                                                    <ext:ComboBox runat="server"
                                                                        Cls="comboGrid"
                                                                        ID="ComboBox2"
                                                                        MaxWidth="65">
                                                                        <Items>
                                                                            <ext:ListItem Text="10" />
                                                                            <ext:ListItem Text="20" />
                                                                            <ext:ListItem Text="30" />
                                                                            <ext:ListItem Text="40" />
                                                                        </Items>
                                                                        <SelectedItems>
                                                                            <ext:ListItem Value="20" />
                                                                        </SelectedItems>
                                                                        <Listeners>
                                                                            <Select Fn="handlePageSizeSelectRule" />
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

                                    <ext:Container
                                        ID="ctMain2"
                                        runat="server"
                                        Cls="calidadTabPanel">
                                        <Loader ID="LoaderMain2"
                                            runat="server"
                                            Url="/Modulos/DataQuality/CalidadKPIresults.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>
                                    </ext:Container>

                                    <ext:Container
                                        ID="ctMain3"
                                        runat="server"
                                        Cls="calidadTabPanel">
                                        <Loader ID="LoaderMain3"
                                            runat="server"
                                            Url="/Modulos/DataQuality/CalidadKPIReport.aspx"
                                            Mode="Frame">
                                            <LoadMask ShowMask="true" />
                                        </Loader>
                                    </ext:Container>

                                </Content>
                            </ext:Panel>

                            <%-- PANEL LATERAL DESPLEGABLE--%>

                            <ext:Panel runat="server"
                                ID="pnAsideR"
                                Visible="true"
                                Split="false"
                                Region="East"
                                Collapsed="true"
                                Width="380"
                                CollapseMode="Header"
                                Collapsible="true"
                                Header="false"
                                Layout="AnchorLayout"
                                Border="false"
                                Hidden="false">
                                <Items>

                                    <%-- PANEL Filtros--%>

                                    <ext:Panel runat="server"
                                        ID="WrapFilterControls"
                                        Hidden="true"
                                        Cls="tbGrey grdIntoAside"
                                        AnchorVertical="100%"
                                        AnchorHorizontal="100%">
                                        <DockedItems>
                                            <ext:Toolbar runat="server"
                                                ID="Toolbar4"
                                                Cls="tbGrey"
                                                Dock="Top"
                                                Padding="0">
                                                <Items>
                                                    <ext:Label
                                                        ID="lblKPIName"
                                                        runat="server"
                                                        IconCls="btnkpigreen"
                                                        Cls="lblHeadAside lblHeadAside tbGrey"
                                                        Text="KPI NAME"
                                                        MarginSpec="36 15 30 15">
                                                    </ext:Label>
                                                </Items>
                                            </ext:Toolbar>
                                            <ext:Toolbar runat="server"
                                                ID="MenuNavPnFiltros"
                                                Cls="TbAsideNavPanel"
                                                Padding="0"
                                                Dock="Left"
                                                Layout="VBoxLayout">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ID="btnNotesFull"
                                                        ToolTip="<%$ Resources:Comun, strHistorico %>"
                                                        Cls="btntime-asR btnNoBorder"
                                                        Handler="displayMenu('pnNotesFull')">
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        Padding="0"
                                                        Margin="0"
                                                        ID="btnNotificationsFull"
                                                        ToolTip="<%$ Resources:Comun, strSemaforo %>"
                                                        Cls="btnsemaforo-asR btnNoBorder"
                                                        Handler="displayMenu('pnNotificationsFull')">
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>

                                            <%--TRAFFIC LIGHT PANEL--%>

                                            <ext:Panel AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                runat="server"
                                                ID="pnNotificationsFull"
                                                PaddingSpec="15 0 20 10"
                                                Border="false"
                                                Header="false"
                                                OverflowY="Auto"
                                                Cls="Whitebg"
                                                Hidden="true">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        Dock="Top"
                                                        ID="AsidetoolSemaforo">
                                                        <Items>
                                                            <ext:Label runat="server"
                                                                ID="lblSemaforo"
                                                                IconCls="btnsemaforogreen"
                                                                Cls="lblHeadAside"
                                                                MarginSpec="10 0 12 0"
                                                                Dock="Top"
                                                                Text="<%$ Resources:Comun, strSemaforo %>"
                                                                Height="20">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Items>
                                                    <ext:Panel runat="server"
                                                        Cls="liNot pntraffic"
                                                        Width="302">
                                                        <Items>
                                                            <ext:GridPanel
                                                                runat="server"
                                                                Header="false"
                                                                ID="gridCurrently"
                                                                Cls="gridPanel gridPanelNoBorder">
                                                                <Store>
                                                                    <ext:Store runat="server"
                                                                        ID="storeKpiSemaforos"
                                                                        RemotePaging="false"
                                                                        AutoLoad="false"
                                                                        OnReadData="storeKpiSemaforos_Refresh"
                                                                        RemoteSort="true"
                                                                        PageSize="20">
                                                                        <Proxy>
                                                                            <ext:PageProxy />
                                                                        </Proxy>
                                                                        <Model>
                                                                            <ext:Model runat="server"
                                                                                IDProperty="DQKpiID">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="DQKpiID" Type="Int" />
                                                                                    <ext:ModelField Name="DQKpi" Type="String" />
                                                                                    <ext:ModelField Name="Colour" Type="String" />
                                                                                    <ext:ModelField Name="Porcentaje" Type="String" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                        <Sorters>
                                                                            <ext:DataSorter Property="DQKpi" Direction="DESC" />
                                                                        </Sorters>
                                                                    </ext:Store>
                                                                </Store>
                                                                <ColumnModel>
                                                                    <Columns>
                                                                        <ext:Column runat="server"
                                                                            ID="colPorcentaje"
                                                                            Flex="1"
                                                                            DataIndex="Porcentaje"
                                                                            Align="Center">
                                                                        </ext:Column>
                                                                        <ext:Column runat="server"
                                                                            ID="colCurrently"
                                                                            Text="<%$ Resources:Comun, strActual %>"
                                                                            Flex="1"
                                                                            DataIndex="Colour"
                                                                            Align="Center">
                                                                            <Renderer Fn="ColorRender" />
                                                                        </ext:Column>
                                                                    </Columns>
                                                                </ColumnModel>
                                                            </ext:GridPanel>
                                                        </Items>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>

                                            <%--HISTORICAL PANEL--%>

                                            <ext:Panel AnchorVertical="100%"
                                                AnchorHorizontal="100%"
                                                runat="server"
                                                PaddingSpec="15 0 20 10"
                                                Border="false"
                                                Header="false"
                                                Cls="Whitebg AsideHistoricalKPI"
                                                ID="pnNotesFull"
                                                OverflowY="Auto"
                                                Hidden="true">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        Dock="Top"
                                                        ID="AsidetoolHistorical">
                                                        <Items>
                                                            <ext:Label runat="server"
                                                                ID="AsidettlHistorical"
                                                                IconCls="btnHistorygreen"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strHistorico %>"
                                                                MarginSpec="10 0 18 0"
                                                                Dock="Top"
                                                                Height="20">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar runat="server"
                                                        Dock="Top"
                                                        ID="toolbar7"
                                                        Hidden="false"
                                                        MaxWidth="305"
                                                        Cls="tlbGrid">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnDescargarFiltro"
                                                                Cls="btnDescargar"
                                                                ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                                                Handler="ExportarDatos('CalidadKPI', hdCliID.value, #{gridVersiones}, hdKPISeleccionado.value, '', -1, App.btnActivosPanel.pressed);">
                                                            </ext:Button>
                                                            <ext:ToolbarFill></ext:ToolbarFill>
                                                            <ext:Button runat="server"
                                                                Text="<%$ Resources:Comun, strMostrarInactivos %>"
                                                                EnableToggle="true"
                                                                ID="btnActivosPanel"
                                                                Cls="btnActivarDesactivarV2"
                                                                PressedCls="btnActivarDesactivarV2Pressed"
                                                                MinWidth="145"
                                                                Pressed="false"
                                                                TextAlign="Left"
                                                                Focusable="false"
                                                                OverCls="none">
                                                                <Listeners>
                                                                    <Click Fn="RefrescarVersiones" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Items>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnAlerts"
                                                        Header="false"
                                                        Hidden="false"
                                                        Disabled="false">
                                                        <Items>
                                                            <ext:Container runat="server"
                                                                ID="ctFiltros"
                                                                Cls=""
                                                                Dock="Top">
                                                                <Content>
                                                                    <local:toolbarFiltros
                                                                        ID="cmpVersiones"
                                                                        runat="server"
                                                                        Stores="storeDQKpisMonitoring"
                                                                        MostrarComboFecha="false"
                                                                        Grid="gridVersiones"
                                                                        MostrarBusqueda="false" />
                                                                </Content>
                                                            </ext:Container>
                                                            <ext:GridPanel
                                                                ID="gridVersiones"
                                                                runat="server"
                                                                Height="532"
                                                                Border="false"
                                                                Cls="grdPnColIcons grdIntoAside"
                                                                Scrollable="Vertical">
                                                                <Store>
                                                                    <ext:Store runat="server"
                                                                        ID="storeDQKpisMonitoring"
                                                                        RemotePaging="false"
                                                                        AutoLoad="false"
                                                                        OnReadData="storeDQKpisMonitoring_Refresh"
                                                                        RemoteSort="true"
                                                                        shearchBox="cmpVersiones_txtSearch"
                                                                        listNotPredictive="Colour,FechaEjecucion,Activa,DQKpiMonitoringID,DQKpi,Ultima"
                                                                        PageSize="20">
                                                                        <Proxy>
                                                                            <ext:PageProxy />
                                                                        </Proxy>
                                                                        <Model>
                                                                            <ext:Model runat="server"
                                                                                IDProperty="DQKpiMonitoringID">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="DQKpiMonitoringID" Type="Int" />
                                                                                    <ext:ModelField Name="DQKpi" Type="String" />
                                                                                    <ext:ModelField Name="Current" Type="String" />
                                                                                    <ext:ModelField Name="Colour" Type="String" />
                                                                                    <ext:ModelField Name="Version" Type="Int" />
                                                                                    <ext:ModelField Name="FechaEjecucion" Type="Date" />
                                                                                    <ext:ModelField Name="NombreCompleto" Type="String" />
                                                                                    <ext:ModelField Name="Activa" Type="Boolean" />
                                                                                    <ext:ModelField Name="Ultima" Type="Boolean" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                        <Listeners>
                                                                            <DataChanged Fn="BuscadorPredictivo" />
                                                                        </Listeners>
                                                                        <Sorters>
                                                                            <ext:DataSorter Property="DQKpi" Direction="DESC" />
                                                                        </Sorters>
                                                                    </ext:Store>
                                                                </Store>
                                                                <ColumnModel runat="server">
                                                                    <Columns>
                                                                        <ext:TemplateColumn
                                                                            runat="server"
                                                                            ID="templateVersiones"
                                                                            DataIndex=""
                                                                            MenuDisabled="true"
                                                                            HideTitleEl="true" Selectable="false"
                                                                            Text=""
                                                                            Flex="5">
                                                                            <Template runat="server">
                                                                                <Html>
                                                                                    <tpl for=".">
                                                                                        <div  class="d-flx" >
                                                                                            <ul class="ulboxes">
                                                                                                <li class="liNot">
                                                                                                    <div class=cntNot>
                                                                                                        <h5>Version {Version}</h5>
                                                                                                        <p>{FechaEjecucion:date('d/m/Y')}</p>
                                                                                                        <p>{NombreCompleto}</p>
                                                                                                    </div>
                                                                                                    <div style="float:right;">
                                                                                                        <p>{Current} {Activa}</p>
                                                                                                    </div>
                                                                                                </li>
                                                                                            </ul>
                                                                                        </div>
                                                                                    </tpl>
                                                                                </Html>
                                                                            </Template>
                                                                            <Renderer Fn="CheckRender" />
                                                                        </ext:TemplateColumn>
                                                                        <ext:ComponentColumn runat="server"
                                                                            DataIndex="DQKpiMonitoringID"
                                                                            HideTitleEl="true"
                                                                            MarginSpec="0 0 1.5rem 0"
                                                                            Flex="1">
                                                                            <Component>
                                                                                <ext:Button runat="server"
                                                                                    ID="btnToggle"
                                                                                    Width="41"
                                                                                    EnableToggle="true"
                                                                                    Cls="btn-toggleGrid"
                                                                                    Pressed="false"
                                                                                    AriaLabel=""
                                                                                    ToolTip="<%$ Resources:Comun, jsActivar %>">
                                                                                    <Listeners>
                                                                                        <Click Fn="cambiarActivo" />
                                                                                    </Listeners>
                                                                                </ext:Button>
                                                                            </Component>
                                                                            <Listeners>
                                                                                <Bind Fn="SetToggleValue" />
                                                                            </Listeners>
                                                                        </ext:ComponentColumn>
                                                                        <ext:Column
                                                                            ID="colVersion"
                                                                            runat="server"
                                                                            Text="<%$ Resources:Comun, strVersion %>"
                                                                            Flex="2"
                                                                            AlignTarget="Center"
                                                                            MinWidth="110"
                                                                            Sortable="true"
                                                                            Hidden="true"
                                                                            DataIndex="Version" />
                                                                        <ext:Column
                                                                            ID="colFechaEjecucion"
                                                                            runat="server"
                                                                            Text="<%$ Resources:Comun, strFechaEjecucion %>"
                                                                            Flex="2"
                                                                            AlignTarget="Center"
                                                                            MinWidth="110"
                                                                            Sortable="true"
                                                                            Hidden="true"
                                                                            DataIndex="FechaEjecucion" />
                                                                        <ext:Column
                                                                            ID="colName"
                                                                            runat="server"
                                                                            Text="<%$ Resources:Comun, strNombre %>"
                                                                            Flex="2"
                                                                            AlignTarget="Center"
                                                                            MinWidth="110"
                                                                            Sortable="true"
                                                                            Hidden="true"
                                                                            DataIndex="NombreCompleto" />
                                                                        <ext:Column
                                                                            ID="colPorc"
                                                                            runat="server"
                                                                            Text="<%$ Resources:Comun, strPorcentaje %>"
                                                                            Flex="2"
                                                                            AlignTarget="Center"
                                                                            MinWidth="110"
                                                                            Sortable="true"
                                                                            Hidden="true"
                                                                            DataIndex="Current" />
                                                                        <ext:Column
                                                                            ID="colActiv"
                                                                            runat="server"
                                                                            Text="<%$ Resources:Comun, strActivo %>"
                                                                            Flex="2"
                                                                            AlignTarget="Center"
                                                                            MinWidth="110"
                                                                            Sortable="true"
                                                                            Hidden="true"
                                                                            DataIndex="Activa" />
                                                                    </Columns>
                                                                </ColumnModel>
                                                                <SelectionModel>
                                                                    <ext:RowSelectionModel runat="server"
                                                                        ID="GridRowSelectVersiones"
                                                                        Mode="Single">
                                                                        <Listeners>
                                                                            <Select Fn="Grid_RowSelectVersiones" />
                                                                        </Listeners>
                                                                    </ext:RowSelectionModel>
                                                                </SelectionModel>
                                                            </ext:GridPanel>
                                                        </Items>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>
                                        </Items>
                                    </ext:Panel>

                                    <%-- PANEL GESTION COLUMNAS--%>

                                    <ext:Panel runat="server"
                                        ID="WrapGestionColumnas"
                                        Hidden="true"
                                        Layout="VBoxLayout"
                                        AnchorVertical="100%"
                                        AnchorHorizontal="100%"
                                        OverflowY="Auto">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch" />
                                        </LayoutConfig>
                                        <DockedItems>
                                            <ext:Toolbar runat="server"
                                                ID="tbTitlePanelColumnas"
                                                Cls="tbGrey"
                                                Dock="Top"
                                                Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR"
                                                        ID="Label6"
                                                        runat="server"
                                                        IconCls="ico-head-columns-gr"
                                                        Cls="lblHeadAside lblHeadAside tbGrey"
                                                        Text="COLUMN SETTINGS"
                                                        MarginSpec="36 15 30 15">
                                                    </ext:Label>
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Items>

                                            <%--   <ext:Label ID="Label5" runat="server" Cls="lblRecordName" Text="Site Name"></ext:Label>--%>
                                            <ext:ComboBox runat="server"
                                                ID="ComboBox4"
                                                Cls="comboGrid comboCustomTrigger  "
                                                EmptyText="Profiles"
                                                LabelAlign="Top"
                                                FieldLabel="Profiles"
                                                Padding="15">
                                                <Items>
                                                    <ext:ListItem Text="Profile 1" />
                                                    <ext:ListItem Text="Profile 2" />
                                                </Items>
                                                <Triggers>
                                                    <ext:FieldTrigger IconCls="ico-trigger-reload"
                                                        ExtraCls="none">
                                                    </ext:FieldTrigger>
                                                </Triggers>
                                            </ext:ComboBox>

                                            <ext:GridPanel
                                                ID="GridColumnas"
                                                Padding="15"
                                                BodyPadding="2"
                                                runat="server"
                                                OverflowY="Auto"
                                                OverflowX="Hidden"
                                                BodyCls=""
                                                Cls=" gridPanel  toolbar-noborder">
                                                <DockedItems>
                                                    <ext:Toolbar runat="server"
                                                        ID="Toolbar9"
                                                        Dock="Bottom"
                                                        Padding="0"
                                                        MarginSpec="8 -8 0 0">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="Button2"
                                                                Cls="btn-ppal "
                                                                Text="Save in new Tab"
                                                                Focusable="false"
                                                                PressedCls="none"
                                                                Hidden="false"
                                                                Flex="1"
                                                                Handler="showwinAddTab()">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>

                                                    <ext:Toolbar runat="server"
                                                        ID="tbColumnasSaveApply"
                                                        Dock="Bottom"
                                                        Padding="0"
                                                        MarginSpec="8 -8 0 0">
                                                        <Items>
                                                            <ext:Button runat="server"
                                                                ID="btnApplicar"
                                                                Cls="btn-ppal "
                                                                Text="Apply"
                                                                Focusable="false"
                                                                PressedCls="none"
                                                                Hidden="false"
                                                                Flex="1">
                                                            </ext:Button>
                                                            <ext:Button runat="server"
                                                                ID="btnGuardarCols"
                                                                Cls="btn-ppal "
                                                                Text="Save"
                                                                Focusable="false"
                                                                PressedCls="none"
                                                                Hidden="false"
                                                                Flex="1">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </DockedItems>
                                                <Store>
                                                    <ext:Store runat="server">
                                                        <Model>
                                                            <ext:Model runat="server" IDProperty="ID">
                                                                <Fields>
                                                                    <ext:ModelField Name="ID" />
                                                                    <ext:ModelField Name="NombreCategoria" />
                                                                    <ext:ModelField Name="NombreItemTarea" />
                                                                    <ext:ModelField Name="CosteItemUnidad" />
                                                                    <ext:ModelField Name="CosteItemTipoUnidad" />
                                                                    <ext:ModelField Name="NumItems" />
                                                                    <ext:ModelField Name="CosteTotalFila" Type="Int" />
                                                                </Fields>
                                                            </ext:Model>
                                                        </Model>
                                                    </ext:Store>
                                                </Store>
                                                <ColumnModel runat="server">
                                                    <Columns>
                                                        <ext:WidgetColumn ID="WidgetColumn4"
                                                            runat="server"
                                                            Cls="col-More"
                                                            DataIndex=""
                                                            Align="Center"
                                                            Hidden="false"
                                                            MaxWidth="50">
                                                            <Widget>
                                                                <ext:Label runat="server"
                                                                    ID="DragBtn"
                                                                    IconCls="btnMoverFila"
                                                                    Cls=" btn-trans "
                                                                    OverCls="none">
                                                                </ext:Label>
                                                            </Widget>
                                                        </ext:WidgetColumn>
                                                        <ext:TemplateColumn runat="server"
                                                            DataIndex=""
                                                            MenuDisabled="true"
                                                            Text=""
                                                            Flex="5">
                                                            <Template runat="server">
                                                                <Html>
                                                                    <tpl for=".">
                                                                        <div class="customCol1">
                                                                            <div class="LabelColumnGridRow">{NombreCategoria}
                                                                            </div> 
                                                                        </div>
                                                                    </tpl>
                                                                </Html>
                                                            </Template>
                                                        </ext:TemplateColumn>
                                                        <ext:WidgetColumn ID="WidgetColumn3"
                                                            runat="server"
                                                            Cls="col-More"
                                                            DataIndex=""
                                                            Align="Center"
                                                            Hidden="false"
                                                            MaxWidth="40">
                                                            <Widget>
                                                                <ext:Button runat="server"
                                                                    OverCls="Over-btnMore"
                                                                    PressedCls="Pressed-none"
                                                                    FocusCls="Focus-none"
                                                                    Cls=" btn-trans btnVisible"
                                                                    Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                                            </Widget>
                                                        </ext:WidgetColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <View>
                                                    <ext:GridView runat="server">
                                                        <Plugins>
                                                            <ext:GridDragDrop runat="server"
                                                                DragText="Drag To Re-order">
                                                            </ext:GridDragDrop>
                                                        </Plugins>
                                                    </ext:GridView>
                                                </View>
                                            </ext:GridPanel>
                                        </Items>
                                    </ext:Panel>

                                    <%-- PANEL MORE INFO--%>

                                    <ext:Panel runat="server"
                                        ID="pnMoreInfo"
                                        Hidden="true"
                                        Cls="tbGrey grdIntoAside"
                                        AnchorVertical="100%"
                                        AnchorHorizontal="100%"
                                        OverflowY="Auto">
                                        <DockedItems>
                                            <ext:Toolbar runat="server"
                                                ID="Toolbar1"
                                                Cls="tbGrey"
                                                Dock="Top"
                                                Padding="0">
                                                <Items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR"
                                                        ID="Label1"
                                                        runat="server"
                                                        IconCls="ico-head-info"
                                                        Cls="lblHeadAside "
                                                        Text="MORE INFO"
                                                        MarginSpec="36 15 30 15">
                                                    </ext:Label>
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
