<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="toolbarFiltrosCombo.ascx.cs" Inherits="TreeCore.Componentes.toolbarFiltrosCombo" %>

<%--<link href="../../../CSS/tCore.css" rel="stylesheet" type="text/css" />--%>
<%--<link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet" />--%>
<script type="text/javascript" src="/Componentes/js/toolbarFiltros.js"></script>

<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteIDComponente" />
<ext:Hidden ID="hdStoresID" runat="server" Name="hdStores" />
<ext:Hidden ID="hdGrid" runat="server" Name="hdGrid" />



<ext:Store runat="server"
    ID="storeClientes"
    AutoLoad="true"
    OnReadData="storeClientes_Refresh"
    RemoteSort="false">
    <Proxy>
        <ext:PageProxy />
    </Proxy>
    <Model>
        <ext:Model runat="server"
            IDProperty="ClienteID">
            <Fields>
                <ext:ModelField Name="ClienteID" Type="Int" />
                <ext:ModelField Name="Cliente" />
            </Fields>
        </ext:Model>
    </Model>
    <Sorters>
        <ext:DataSorter Property="Cliente" Direction="ASC" />
    </Sorters>
</ext:Store>


<ext:Toolbar runat="server"
    ID="tbFiltros"
    Dock="Top"
    Cls="tlbGrid"
    Layout="ColumnLayout">
    <Listeners>
        <AfterRender Fn="StyleOnResize" />
        <Resize Fn="StyleOnResize"></Resize>
    </Listeners>
    <Items>
        <ext:TextField
            ID="txtSearch"
            Cls="mainSearchBox  "
            runat="server"
            EmptyText="<%$ Resources:Comun, strBuscar %>"
            LabelWidth="50"
            Width="250"
            EnableKeyEvents="true">
            <Triggers>
                <ext:FieldTrigger Icon="Search" />
                <ext:FieldTrigger Icon="clear" Handler="BorrarFiltros(#{hdGrid});" />
            </Triggers>
            <Listeners>
                <Render Fn="FieldSearch" Buffer="250" />
                <Change Fn="FiltrarColumnas" Buffer="250" />
                <TriggerClick Fn="LimpiarFiltroBusqueda" />
            </Listeners>
        </ext:TextField>

        <ext:Component runat="server" Cls="filler"></ext:Component>

        <ext:Container runat="server"
            ID="wrapBotonesYcmbFiltros"
            Cls="wrapBotonesYcmbFiltros "
            Layout="HBoxLayout">
            <Items>
                <ext:Container runat="server"
                    ID="wrapFilterCmbNBtns"
                    Cls="wrapFilterCmbNBtns"
                    Layout="HBoxLayout">
                    <Items>
                        <ext:Container meta:resourceKey="ContainerTBButtons"
                            runat="server"
                            ID="ContainerTBButtons"
                            Layout="HBoxLayout"
                            Cls="GrupoBtnFilters"
                            Flex="1"
                            Hidden="false"
                            Width="90">
                            <Items>
                                <ext:Button meta:resourceKey="btnDuplicarTipo"
                                    runat="server"
                                    Width="30"
                                    ID="btnGestionarColumnas"
                                    Cls="btn-trans btnColumnas"
                                    ToolTip="Gestionar Columnas"
                                    Handler="hideAsideR('panelColumnas');">
                                </ext:Button>
                                <ext:Button
                                    runat="server"
                                    Width="30"
                                    Hidden="true"
                                    ID="btnQuitarFiltros"
                                    Cls="btn-trans btnRemoveFilters"
                                    ToolTip="<%$ Resources:Comun, strQuitarFiltro %>">
                                    <Listeners>
                                        <Click Handler="BorrarFiltros(#{hdGrid});"></Click>
                                    </Listeners>
                                </ext:Button>
                                <ext:Button meta:resourceKey="btnFiltroNegativo"
                                    runat="server"
                                    Width="30"
                                    ID="btnFiltroNegativo"
                                    Cls="btn-trans btnFiltroNegativo"
                                    ToolTip="Filtro Negativo">
                                </ext:Button>
                            </Items>
                        </ext:Container>
                        <ext:ComboBox meta:resourceKey="cmbMisFiltros"
                            runat="server"
                            ID="cmbMisFiltros"
                            Cls="comboGrid ClscmbMisFiltros"
                            EmptyText="My Filters"
                            Flex="3"
                            Hidden="true">
                            <Items>
                                <ext:ListItem Text="Filtro 1" />
                                <ext:ListItem Text="Filtro 2" />
                                <ext:ListItem Text="Filtro 3" />
                                <ext:ListItem Text="Filtro 4" />
                                <ext:ListItem Text="Filtro 5" />
                            </Items>
                        </ext:ComboBox>
                    </Items>
                </ext:Container>

                <ext:Container runat="server"
                    ID="wrapFilterCmbNBtns2"
                    Cls="wrapFilterCmbNBtns2"
                    Layout="HBoxLayout">
                    <Items>
                        <ext:ComboBox
                            runat="server"
                            ID="cmbClientes"
                            Cls="comboGrid cmbClientes"
                            Hidden="true"
                            StoreID="storeClientes"
                            DisplayField="Cliente"
                            ValueField="ClienteID"
                            EmptyText="<%$ Resources:Comun, cmbClientes.EmptyText %>"
                            FieldLabel="<%$ Resources:Comun, cmbClientes.FieldLabel %>">
                            <Listeners>
                                <Select Fn="CargarStores" />
                            </Listeners>
                        </ext:ComboBox>

                        <ext:ComboBox meta:resourceKey="cmbFechasRango"
                            runat="server"
                            ID="cmbFechasRango"
                            Cls="comboGrid cmbFechasRango"
                            EmptyText="Date Range"
                            Hidden="true">
                            <Items>
                                <ext:ListItem
                                    Text="<%$ Resources:Comun, strUltimas24h %> "
                                    Value="Dia" />
                                <ext:ListItem
                                    Text="<%$ Resources:Comun, strUltimaSemana %>"
                                    Value="Semana" />
                                <ext:ListItem
                                    Text="<%$ Resources:Comun, strUltimoMes %>"
                                    Value="Mes" />
                                <ext:ListItem
                                    Text="<%$ Resources:Comun, strUltimoTrimestre %>"
                                    Value="Trimestre" />
                                <ext:ListItem
                                    Text="<%$ Resources:Comun, strUltimoAño %>"
                                    Value="Año" />
                            </Items>
                            <Listeners>
                                <Select Fn="CargarStores" />
                            </Listeners>
                        </ext:ComboBox>

                        <ext:ComboBox runat="server"
                            ID="cmbEstatico"
                            Cls="comboGrid"
                            Hidden="true">
                            <Listeners>
                                <%--<BeforeRender Handler="#{DirectMethods}.CargarCmbEstatico();" />--%>
                                <Select Fn="SelectCmbEstatico" />
                                <TriggerClick Fn="LimpiarCmbEstatico" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger
                                    IconCls="ico-reload"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>
                    </Items>
                </ext:Container>
            </Items>
        </ext:Container>

        <ext:ComboBox runat="server"
            ID="cmbModulos"
            meta:resourceKey="cmbModulos"
            Cls="comboGrid pos-boxGrid cmbModulos"
            EmptyCls="cmbModulos"
            FieldBodyCls="cmbModulos"
            QueryMode="Local"
            EmptyText="Modulos"
            LabelAlign="Right"
            Width="300">
            <Items>
                <ext:ListItem Text="Modulo 1" />
                <ext:ListItem Text="Modulo 2" />
                <ext:ListItem Text="Modulo 3" />
                <ext:ListItem Text="Modulo 4" />
                <ext:ListItem Text="Modulo 5" />
            </Items>
        </ext:ComboBox>
    </Items>
</ext:Toolbar>

