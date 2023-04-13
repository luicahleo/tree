<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AtributosConfiguracion.ascx.cs" Inherits="TreeCore.Componentes.AtributosConfiguracion" %>

<%-- #region Stores--%>

<ext:Store ID="storeTablas"
    runat="server"
    AutoLoad="false"
    OnReadData="storeTablas_Refresh"
    RemoteSort="false">
    <Proxy>
        <ext:PageProxy />
    </Proxy>
    <Model>
        <ext:Model runat="server"
            IDProperty="TablaID">
            <Fields>
                <ext:ModelField Name="TablaID" />
                <ext:ModelField Name="TablaNombre" />
            </Fields>
        </ext:Model>
    </Model>
    <Sorters>
        <ext:DataSorter Property="TablaNombre" />
    </Sorters>
    <Listeners>
        <Load Fn="RecargarTablas" />
    </Listeners>
</ext:Store>

<ext:Store ID="storeColumnas"
    runat="server"
    AutoLoad="false"
    OnReadData="storeColumnas_Refresh"
    RemoteSort="false">
    <Proxy>
        <ext:PageProxy />
    </Proxy>
    <Model>
        <ext:Model runat="server"
            IDProperty="ColumnaTablaID">
            <Fields>
                <ext:ModelField Name="ColumnaTablaID" />
                <ext:ModelField Name="ColumnaNombre" Type="string" />
            </Fields>
        </ext:Model>
    </Model>
    <Sorters>
        <ext:DataSorter Property="ColumnaNombre" />
    </Sorters>
</ext:Store>

<ext:Store runat="server"
    ID="storeRoles"
    AutoLoad="false"
    OnReadData="storeRoles_Refresh"
    RemoteSort="false">
    <Proxy>
        <ext:PageProxy />
    </Proxy>
    <Model>
        <ext:Model runat="server"
            IDProperty="RolID">
            <Fields>
                <ext:ModelField Name="RolID" Type="Int" />
                <ext:ModelField Name="Nombre" Type="string" />
            </Fields>
        </ext:Model>
    </Model>
    <Sorters>
        <ext:DataSorter Property="Nombre" />
    </Sorters>
</ext:Store>

<ext:Store runat="server"
    ID="storeRolesRestringidos"
    AutoLoad="false"
    OnReadData="storeRolesRestringidos_Refresh"
    RemoteSort="false">
    <Proxy>
        <ext:PageProxy />
    </Proxy>
    <Model>
        <ext:Model runat="server"
            IDProperty="AtributoRolRestringidoID">
            <Fields>
                <ext:ModelField Name="AtributoRolRestringidoID" Type="Int" />
                <ext:ModelField Name="Nombre" Type="string" />
                <ext:ModelField Name="Oculto" Type="Boolean" />
                <ext:ModelField Name="Restriccion" />
            </Fields>
        </ext:Model>
    </Model>
    <Sorters>
        <ext:DataSorter Property="Nombre" />
    </Sorters>
</ext:Store>

<ext:Store
    ID="storeFomatos"
    runat="server"
    AutoLoad="false"
    OnReadData="storeFomatos_Refresh"
    RemoteSort="false">
    <Proxy>
        <ext:PageProxy />
    </Proxy>
    <Model>
        <ext:Model runat="server" IDProperty="AtributoTipoDatoPropiedadID">
            <Fields>
                <ext:ModelField Name="AtributoTipoDatoPropiedadID" Type="Int" />
                <ext:ModelField Name="Nombre" />
                <ext:ModelField Name="Valor" />
            </Fields>
        </ext:Model>
    </Model>
    <Sorters>
        <ext:DataSorter Property="Nombre" />
    </Sorters>
</ext:Store>

<ext:Store
    ID="storeTiposPropiedades"
    runat="server"
    AutoLoad="false"
    OnReadData="storeTiposPropiedades_Refresh"
    RemoteSort="false">
    <Proxy>
        <ext:PageProxy />
    </Proxy>
    <Model>
        <ext:Model runat="server" IDProperty="TipoDatoPropiedadID">
            <Fields>
                <ext:ModelField Name="TipoDatoPropiedadID" Type="Int" />
                <ext:ModelField Name="TipoValor" />
                <ext:ModelField Name="Nombre" />
                <ext:ModelField Name="Codigo" />
            </Fields>
        </ext:Model>
    </Model>
    <Sorters>
        <ext:DataSorter Property="Nombre" />
    </Sorters>
</ext:Store>

<ext:Store
    runat="server"
    ID="storeSelectCamposVinculados"
    AutoLoad="false"
    OnReadData="storeSelectCamposVinculados_Refresh">
    <Proxy>
        <ext:PageProxy />
    </Proxy>
    <Model>
        <ext:Model IDProperty="ID"
            runat="server">
            <Fields>
                <ext:ModelField Name="ID" />
                <ext:ModelField Name="Name" />
                <ext:ModelField Name="ObjectID" />
                <ext:ModelField Name="ColumnaID" />
                <ext:ModelField Name="IndiceColumna" />
                <ext:ModelField Name="esCarpeta" Type="Boolean" />
            </Fields>
        </ext:Model>
    </Model>
    <Sorters>
        <ext:DataSorter Property="esCarpeta" Direction="DESC" />
        <ext:DataSorter Property="Name" />
    </Sorters>
</ext:Store>

<ext:Store
    runat="server"
    ID="storeColumnasVinculadas"
    AutoLoad="false"
    OnReadData="storeColumnasVinculadas_Refresh">
    <Proxy>
        <ext:PageProxy />
    </Proxy>
    <Model>
        <ext:Model IDProperty="ColumnaModeloDatoID"
            runat="server">
            <Fields>
                <ext:ModelField Name="ID" />
                <ext:ModelField Name="Name" />
                <ext:ModelField Name="AtributoID" />
                <ext:ModelField Name="ColumnaModeloDatoID" />
                <ext:ModelField Name="JsonRuta" />
                <ext:ModelField Name="Orden" />
            </Fields>
        </ext:Model>
    </Model>
    <Listeners>
        <DataChanged Fn="ValidarLinkedList" />
    </Listeners>
</ext:Store>

<%-- #endregion --%>

<%-- #region Windows --%>

<ext:Window
    Hidden="true"
    runat="server"
    Title="<%$ Resources:Comun, strConfiguracionDatos %>"
    ID="winAddColumns"
    Scrollable="Vertical"
    OverflowY="Auto"
    Width="500"
    Height="400"
    Modal="true"
    Resizable="false"
    Layout="FitLayout">
    <Items>
        <ext:GridPanel runat="server"
            ID="gridCamposVinculados"
            StoreID="storeSelectCamposVinculados"
            Flex="1"
            Layout="FitLayout"
            Cls="gridPanel noBorder"
            Header="false"
            Hidden="false"
            Reorderable="true"
            OverflowY="Auto"
            HeightSpec="100%">
            <DockedItems>
                <ext:Toolbar
                    runat="server"
                    ID="tlbRuta"
                    Cls="tlbGrid"
                    Dock="Top">
                    <Items>
                        <ext:Button runat="server"
                            Cls="noBorder btnBack"
                            IconCls="ico-prev"
                            ID="btnBack">
                            <Listeners>
                                <Click Fn="VolverAtras" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button runat="server"
                            Text="<%$ Resources:Comun, strRaiz %>"
                            Cls="tbNavPath btnBack"
                            ID="lbRutaEmplazamientoTipo">
                            <Listeners>
                                <Click Fn="IrRutaRaiz" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button runat="server"
                            Cls="noBorder btnNextRuta"
                            IconCls="ico-next"
                            Hidden="true"
                            ID="btnRaizCarpeta">
                        </ext:Button>
                        <ext:Button
                            runat="server"
                            Hidden="true"
                            ID="btnMenuRuta"
                            Cls="noBorder btnMenuRuta btnBack"
                            IconCls="ico-nav-folder-gr-16">
                            <Menu>
                                <ext:Menu
                                    runat="server"
                                    ID="menuRuta"
                                    Cls="menuRuta">
                                    <Items>
                                    </Items>
                                    <Listeners>
                                        <Click Fn="SeleccionarRuta" />
                                    </Listeners>
                                </ext:Menu>
                            </Menu>
                        </ext:Button>
                        <ext:Button runat="server"
                            Cls="noBorder btnNextRuta"
                            IconCls="ico-next"
                            Hidden="true"
                            ID="btnCarpetaActual">
                        </ext:Button>
                        <ext:Label runat="server"
                            ID="lbRutaCategoria"
                            Cls="rutaCategoria btnBack"
                            Height="32"
                            Hidden="true">
                        </ext:Label>
                        <ext:ToolbarFill />
                    </Items>
                </ext:Toolbar>
            </DockedItems>
            <ColumnModel>
                <Columns>
                    <ext:Column runat="server"
                        ID="colImg"
                        DataIndex="esCarpeta"
                        Width="55"
                        Align="Start">
                        <Renderer Fn="RenderCarpetas" />
                    </ext:Column>
                    <ext:Column runat="server"
                        ID="colLinkedName"
                        DataIndex="Name"
                        Flex="3"
                        Align="Start">
                    </ext:Column>
                </Columns>
            </ColumnModel>
            <Listeners>
                <RowDblClick Fn="AccionSelectCol" />
            </Listeners>
            <Plugins>
                <ext:GridFilters runat="server"
                    ID="gridFilters"
                    MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                    meta:resourceKey="GridFilters">
                </ext:GridFilters>
            </Plugins>
        </ext:GridPanel>
    </Items>
</ext:Window>

<ext:Window
    Hidden="true"
    runat="server"
    Title="<%$ Resources:Comun, strConfiguracionDatos %>"
    ID="windowSetting"
    Scrollable="Vertical"
    OverflowY="Auto"
    Width="250"
    MinWidth="500"
    MinHeight="180"
    MaxWidth="500"
    BodyPadding="32"
    AutoDataBind="true"
    Modal="true"
    Resizable="false"
    Layout="FitLayout"
    Cls="headerGrey">
    <Items>
        <ext:FormPanel ID="pnFormularioSetting" runat="server" Cls="containerGrid" Layout="VBoxLayout" OverflowY="Auto" Scrollable="Vertical">
            <LayoutConfig>
                <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
            </LayoutConfig>
            <Items>
                <ext:Container runat="server" Cls="mainContainer" Layout="VBoxLayout">
                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                    </LayoutConfig>
                    <Items>
                        <%--<ext:Label runat="server" Text="<%$ Resources:Comun, strTipoLista %>" Cls="headerAlignedV2" />--%>
                        <ext:ComboBox
                            ID="cmbTipoLista"
                            runat="server"
                            LabelAlign="Top"
                            FieldLabel="<%$ Resources:Comun, strTipoLista %>"
                            WidthSpec="100%"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Items>
                                <ext:ListItem Text="<%$ Resources:Comun, strEstatica %>" Value="1" />
                                <ext:ListItem Text="<%$ Resources:Comun, strBaseDeDatos %>" Value="2" />
                                <%--<ext:ListItem Text="<%$ Resources:Comun, strFunciones %>" Value="3" />--%>
                            </Items>
                            <Listeners>
                                <Select Fn="SeleccionarTipoLista" />
                                <TriggerClick Fn="RecargarComboTipoLista" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>
                    </Items>
                </ext:Container>
                <ext:Container runat="server" Cls="mainContainer toolSeparator">
                </ext:Container>

                <ext:Container runat="server" ID="containerTable" Cls="mainContainer" Hidden="true" Layout="VBoxLayout">

                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                    </LayoutConfig>

                    <Items>
                        <ext:Label runat="server" Hidden="true" Text="<%$ Resources:Comun, strTabla %>" Cls="headerAlignedV2" />

                        <ext:ComboBox
                            ID="cmbTable"
                            runat="server"
                            StoreID="storeTablas"
                            DisplayField="TablaNombre"
                            ValueField="TablaID"
                            FieldLabel="<%$ Resources:Comun, strTabla %>"
                            LabelAlign="Top"
                            Mode="Local"
                            QueryMode="Local"
                            AllowBlank="false"
                            Editable="true"
                            WidthSpec="100%"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Listeners>
                                <Select Fn="SeleccionarTable" />
                                <TriggerClick Fn="RecargarTable" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>
                    </Items>
                    <Listeners>
                        <Show Handler="this.items.items[1].store.reload()" />
                    </Listeners>
                </ext:Container>

                <ext:Container runat="server" ID="containerValue" Cls="mainContainer toolSeparator" Hidden="true" Layout="VBoxLayout">
                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                    </LayoutConfig>
                    <Items>
                        <ext:Container runat="server" WidthSpec="100%" PaddingSpec="0 0 15px">
                            <Items>
                                <ext:Button runat="server"
                                    Cls="btn-accept FloatR"
                                    ID="btnAddColumn"
                                    Hidden="false"
                                    Width="150"
                                    Text="Add Column">
                                    <Listeners>
                                        <Click Fn="btnAddColumn" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Container>

                        <ext:GridPanel runat="server"
                            ID="gridColumnasVinculadas"
                            StoreID="storeColumnasVinculadas"
                            Flex="1"
                            Layout="FitLayout"
                            Cls="grdFormElAdded noBorder"
                            Header="false"
                            Hidden="false"
                            OverflowY="Hidden"
                            Height="250"
                            WidthSpec="100%">
                            <ColumnModel>
                                <Columns>
                                    <ext:Column runat="server"
                                        ID="Column1"
                                        DataIndex="Name"
                                        Flex="3"
                                        Align="Start"
                                        Sortable="false">
                                    </ext:Column>
                                    <ext:CommandColumn ID="colEliminarColumnaVin" runat="server" Width="50" Align="Center">
                                        <Commands>
                                            <ext:GridCommand CommandName="EliminarColumnaVin" IconCls="ico-close">
                                            </ext:GridCommand>
                                        </Commands>
                                        <Listeners>
                                            <Command Fn="EliminarColumnaVin" />
                                        </Listeners>
                                    </ext:CommandColumn>
                                </Columns>
                            </ColumnModel>
                            <View>
                                <ext:GridView ID="ColumnasOrdenadas" runat="server">
                                    <Plugins>
                                        <ext:GridDragDrop runat="server" />
                                    </Plugins>
                                    <Listeners>
                                        <Drop Fn="trackDragDrop" />
                                    </Listeners>
                                </ext:GridView>
                            </View>
                            <Plugins>
                                <ext:GridFilters runat="server"
                                    ID="gridFilters1"
                                    MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                    meta:resourceKey="GridFilters">
                                </ext:GridFilters>
                            </Plugins>
                            <SelectionModel>
                                <ext:RowSelectionModel runat="server"
                                    ID="RowSelectionModel1"
                                    Mode="Multi">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                        </ext:GridPanel>

                        <ext:ComboBox
                            ID="cmbValue"
                            runat="server"
                            Width="220"
                            StoreID="storeColumnas"
                            DisplayField="ColumnaNombre"
                            ValueField="ColumnaTablaID"
                            Hidden="true"
                            Mode="local"
                            QueryMode="Local"
                            AllowBlank="false"
                            Editable="true"
                            FieldLabel="<%$ Resources:Comun, strValor %>"
                            LabelAlign="Top"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Listeners>
                                <Select Fn="SeleccionarValue" />
                                <TriggerClick Fn="RecargarCombos" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>

                        <ext:MultiCombo
                            ID="cmbToolTip"
                            runat="server"
                            Width="220"
                            StoreID="storeColumnas"
                            DisplayField="ColumnaNombre"
                            Hidden="true"
                            ValueField="ColumnaTablaID"
                            AllowBlank="true"
                            FieldLabel="<%$ Resources:Comun, strValor %>"
                            LabelAlign="Top"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Listeners>
                                <Select Fn="SeleccionarValue" />
                                <TriggerClick Fn="RecargarCombos" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:MultiCombo>
                    </Items>
                </ext:Container>

                <ext:Container runat="server" ID="containerAux" Cls="mainContainer toolSeparator" Hidden="true">
                </ext:Container>

                <ext:Container runat="server" ID="containerTxtaSetting" Cls="mainContainer toolSeparator" Hidden="true" Layout="VBoxLayout">
                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                    </LayoutConfig>
                    <Items>
                        <ext:Label ID="lbTxtaSetting" runat="server" Cls="headerAlignedV2" />

                        <ext:TextArea
                            ID="txtaSetting"
                            runat="server"
                            WidthSpec="100%"
                            LabelAlign="Top"
                            FieldLabel=""
                            Mode="local"
                            CausesValidation="true"
                            Editable="true">
                        </ext:TextArea>
                    </Items>
                </ext:Container>
            </Items>
            <Listeners>
                <ValidityChange Fn="FormularioSettingValido" />
            </Listeners>
        </ext:FormPanel>

    </Items>

    <Buttons>
        <ext:Button
            runat="server"
            Text="<%$ Resources:Comun, btnGuardar.Text %>"
            Cls="btn-accept"
            Disabled="true"
            ID="btnGuardarWindowSetting"
            Width="100">
            <Listeners>
                <Click Fn="GuardarWindowSetting" />
            </Listeners>
        </ext:Button>
    </Buttons>
    <Listeners>
        <Close Fn="CloseWindowSetting" />
    </Listeners>
</ext:Window>

<ext:Window
    Hidden="true"
    runat="server"
    Title="<%$ Resources:Comun, strFormato %>"
    ID="winFormat"
    Scrollable="Vertical"
    Width="300"
    Height="300"
    MinHeight="150"
    MaxHeight="300"
    BodyPadding="10"
    AutoDataBind="true"
    Cls="headerGrey">
    <Items>
        <ext:FormPanel ID="pnFormularioFormat" runat="server" Cls="containerGrid formFormat">
            <Items>
                <ext:Container runat="server" Cls="mainContainer">
                    <Items>
                        <%--<ext:Label runat="server" Text="<%$ Resources:Comun, strAtributo %>" Cls="headerAlignedV2" />--%>
                        <ext:Container runat="server" Cls="winGestion-panel-1-col">
                            <Items>
                                <ext:ComboBox
                                    ID="cmbTiposPropiedades"
                                    runat="server"
                                    Width="220"
                                    StoreID="storeTiposPropiedades"
                                    DisplayField="Nombre"
                                    ValueField="TipoDatoPropiedadID"
                                    Mode="local"
                                    QueryMode="Local"
                                    AllowBlank="false"
                                    Editable="true"
                                    LabelAlign="Top"
                                    FieldLabel="<%$ Resources:Comun, strAtributo %>"
                                    EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                                    <Listeners>
                                        <Select Fn="SeleccionarTiposPropiedades" />
                                        <TriggerClick Fn="RecargarTiposPropiedades" />
                                    </Listeners>
                                    <Triggers>
                                        <ext:FieldTrigger IconCls="ico-reload"
                                            Hidden="true"
                                            Weight="-1"
                                            QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                    </Triggers>
                                </ext:ComboBox>
                            </Items>
                        </ext:Container>
                    </Items>
                </ext:Container>
                <ext:Container runat="server" Cls="mainContainer">
                    <Items>
                        <ext:Label runat="server" Text="<%$ Resources:Comun, strValor %>" Cls="headerAlignedV2" Hidden="true" />
                        <ext:Container runat="server" ID="ContenedorCampoValueFormat" Cls="winGestion-panel-1-col">
                        </ext:Container>
                    </Items>
                </ext:Container>
            </Items>
            <Listeners>
                <ValidityChange Fn="FormularioFormatValido" />
            </Listeners>
        </ext:FormPanel>
    </Items>
    <Buttons>
        <ext:Button
            runat="server"
            ID="btnGuardarFormat"
            Text="<%$ Resources:Comun, btnGuardar.Text %>"
            Cls="btn-accept"
            Disabled="true"
            Width="100">
            <Listeners>
                <Click Fn="GuardarWinFormat" />
            </Listeners>
        </ext:Button>
    </Buttons>
    <Listeners>
        <Hide Fn="CerrarVentanaFormat" />
    </Listeners>
</ext:Window>

<ext:Window
    Hidden="true"
    runat="server"
    IconCls="ico-gestionV2"
    Title="<%$ Resources:Comun, strNuevaCondicion %>"
    ID="winNewCondition"
    Scrollable="Vertical"
    Width="550"
    Height="400"
    MinWidth="280"
    MinHeight="180"
    MaxWidth="550"
    MaxHeight="400"
    BodyPadding="10"
    AutoDataBind="true"
    Cls="headerGrey">
    <Items>
        <ext:Container runat="server" Cls="containerGrid">

            <Items>
                <ext:Container runat="server" Cls="mainContainer">
                    <Items>
                        <ext:Label runat="server" Text="<%$ Resources:Comun, strNombre %>" Cls="headerAlignedV2" />
                        <ext:TextField
                            runat="server"
                            Width="220">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" QTip="<b>Title</b><br/>Custom title" />
                                <ext:FieldTrigger Icon="Date" QTip="Custom tip" />
                            </Triggers>
                        </ext:TextField>
                    </Items>
                </ext:Container>
                <ext:Container runat="server" Cls="mainContainer">
                    <Items>
                        <ext:Label runat="server" Text="<%$ Resources:Comun, strOperador %>" Cls="headerAlignedV2" />


                        <ext:ComboBox
                            ID="cmbOperador"
                            runat="server"
                            Width="220"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Items>
                                <ext:ListItem Text="=" />
                                <ext:ListItem Text=">" />
                                <ext:ListItem Text="<" />
                            </Items>
                            <Listeners>
                                <Select Fn="SeleccionarOperador" />
                                <TriggerClick Fn="RecargarCombos" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>
                    </Items>
                </ext:Container>
            </Items>
            <Items>
                <ext:Container runat="server" Cls="mainContainer toolSeparator">
                    <Items>
                        <ext:Label runat="server" Text="Value" Cls="headerAlignedV2" />

                        <ext:TextField
                            runat="server"
                            Width="220"
                            EmptyText="Value">
                        </ext:TextField>
                    </Items>
                </ext:Container>
                <ext:Container runat="server" Cls="mainContainer toolSeparator">
                    <Items>
                        <ext:Label runat="server" Text="Value" Cls="headerAlignedV2" />
                        <ext:TextField
                            runat="server"
                            Width="220"
                            EmptyText="Default">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" QTip="<b>Title</b><br/>Custom title" />
                                <ext:FieldTrigger Icon="Date" QTip="Custom tip" />
                            </Triggers>
                        </ext:TextField>

                    </Items>
                </ext:Container>
            </Items>
            <Items>
                <ext:Container runat="server" Cls="mainContainer toolSeparator">
                    <Items>
                        <ext:Label runat="server" Text="Element" Cls="headerAlignedV2" />

                        <ext:ComboBox
                            ID="cmbTablaNewCondition"
                            runat="server"
                            StoreID="storeTablas"
                            DisplayField="TablaNombre"
                            ValueField="TablaID"
                            Mode="local"
                            QueryMode="Local"
                            AllowBlank="false"
                            Editable="true"
                            Width="220"
                            EmptyText="<%$ Resources:Comun, strSeleccionar %>">
                            <Listeners>
                                <Select Fn="SeleccionarTablaNewCondition" />
                                <TriggerClick Fn="RecargarCombos" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:ComboBox>
                    </Items>
                </ext:Container>
                <ext:Container runat="server" Cls="mainContainer toolSeparator">
                    <Items>
                        <ext:Label runat="server" Text="Attribute" Cls="headerAlignedV2" />

                        <ext:TextField
                            runat="server"
                            Width="220"
                            EmptyText="Select a table">
                            <Triggers>
                                <ext:FieldTrigger Icon="Clear" QTip="<b>Title</b><br/>Custom title" />
                                <ext:FieldTrigger Icon="Date" QTip="Custom tip" />
                            </Triggers>
                        </ext:TextField>
                    </Items>
                </ext:Container>
            </Items>
            <Items>
                <ext:Container runat="server" Cls="mainContainer toolSeparator" MarginSpec="">
                    <Items>
                        <ext:Checkbox ID="ckboxMandatory" runat="server" FieldLabel="Mandatory" />

                    </Items>
                </ext:Container>
            </Items>

        </ext:Container>
    </Items>
    <DockedItems>
        <ext:Toolbar runat="server" Dock="Bottom" ID="toolbarConditions">
            <Items>
                <ext:Hyperlink
                    runat="server"
                    NavigateUrl="https://ext.net"
                    Text="Save as a template"
                    Width="150"
                    Target="_blank"
                    ID="btnToolbar" />
                <ext:ToolbarFill />
                <ext:Button
                    runat="server"
                    Text="<%$ Resources:Comun, btnGuardar.Text %>"
                    Cls="btn-accept"
                    Width="100"
                    Handler="GuardarWinNewCondition" />
            </Items>
        </ext:Toolbar>
    </DockedItems>



</ext:Window>

<ext:Window
    Hidden="true"
    runat="server"
    Title="<%$ Resources:Comun, strRestriccionRoles %>"
    ID="winAddRestriction"
    Scrollable="Vertical"
    Width="320"
    Height="400"
    MinWidth="280"
    MinHeight="180"
    MaxWidth="340"
    MaxHeight="400"
    BodyPadding="10"
    AutoDataBind="true"
    Layout="FitLayout"
    Cls="headerGrey">
    <Items>
        <ext:FormPanel runat="server" ID="pnFormularioRestriction" Cls="formRestriction" Layout="VBoxLayout">
            <LayoutConfig>
                <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
            </LayoutConfig>
            <Items>
                <ext:Container runat="server" Cls="marginTop" Layout="VBoxLayout">
                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
                    </LayoutConfig>
                    <Items>
                        <ext:Label runat="server" Text="<%$ Resources:Comun, strRoles %>" Cls="headerAlignedV2" />
                        <ext:MultiCombo
                            ID="cmbRoles"
                            runat="server"
                            StoreID="storeRoles"
                            DisplayField="Nombre"
                            ValueField="RolID"
                            Mode="local"
                            QueryMode="Local"
                            AllowBlank="true"
                            Width="280"
                            EmptyText="<%$ Resources:Comun, strDefecto %>">
                            <Listeners>
                                <Select Fn="SeleccionarRoles" />
                                <TriggerClick Fn="RecargarCombos" />
                            </Listeners>
                            <Triggers>
                                <ext:FieldTrigger IconCls="ico-reload"
                                    Hidden="true"
                                    Weight="-1"
                                    QTip="<%$ Resources:Comun, strRecargarLista %>" />
                            </Triggers>
                        </ext:MultiCombo>
                    </Items>
                </ext:Container>
                <ext:RadioGroup runat="server" ID="containerRestrictions" GroupName="radioGroupRoles" AllowBlank="false"
                    ColumnsNumber="1">
                    <Items>
                        <ext:Radio runat="server" ID="radHidden" BoxLabel="<%$ Resources:Comun, strNoMostrarAtributo %>" InputValue="3" Cls="recuadroBordes" Width="280"></ext:Radio>
                        <ext:Radio runat="server" ID="radReadOnly" BoxLabel="<%$ Resources:Comun, strSoloLecturaAtributo %>" InputValue="2" Cls="recuadroBordes" Width="280"></ext:Radio>
                        <ext:Radio runat="server" ID="radActive" BoxLabel="<%$ Resources:Comun, strActivo %>" Checked="true" InputValue="1" Cls="recuadroBordes" Width="280"></ext:Radio>
                    </Items>
                </ext:RadioGroup>
            </Items>
            <Listeners>
                <ValidityChange Fn="FormularioValidoRestriction" />
            </Listeners>
        </ext:FormPanel>
    </Items>
    <Buttons>
        <ext:Button
            runat="server"
            Text="<%$ Resources:Comun, btnGuardar.Text %>"
            ID="btnWinAddRestriction"
            Cls="btn-accept"
            Disabled="true"
            Width="100">
            <Listeners>
                <Click Fn="GuardarWinAddRestriction" />
            </Listeners>
        </ext:Button>
    </Buttons>
    <Listeners>
        <Hide Fn="CerrarVentanaRestriction" />
    </Listeners>
</ext:Window>

<%-- #endregion --%>

<ext:Container runat="server" ID="contenedorGrids">
    <Items>

        <ext:Hidden ID="hdDatabase" runat="server" />
        <ext:Hidden ID="hdValue" runat="server" />
        <ext:Hidden ID="hdIndex" runat="server" />
        <ext:Hidden ID="hdController" runat="server" />
        <ext:Hidden ID="hdFunciones" runat="server" />
        <ext:Hidden ID="hdMostrarDataSetting" runat="server" />
        <ext:Hidden ID="hdTooltip" runat="server" />
        <ext:Hidden ID="hdAtributoID" runat="server" />
        <ext:Hidden ID="hdCategoriaAtributoID" runat="server" />
        <ext:Hidden ID="hdTablaActual" runat="server" />
        <ext:Hidden ID="hdCampoVinculadoRuta" runat="server" />

        <%-- PANELES GRID DESPLEGABLES --%>
        <ext:GridPanel runat="server"
            Hidden="true"
            ID="GridFormatRule"
            Height="200"
            StoreID="storeFomatos"
            Cls="gridPanelSize"
            Width="250"
            Scrollable="Vertical">
            <ColumnModel runat="server">
                <Columns>
                    <ext:Column runat="server" DataIndex="Nombre" Flex="1"></ext:Column>

                    <ext:Column runat="server" DataIndex="Valor" Flex="1"></ext:Column>
                    <ext:CommandColumn ID="colEliminarFormato" runat="server" Width="50" Align="Center">
                        <Commands>
                            <ext:GridCommand CommandName="EliminarFormato" IconCls="ico-close"></ext:GridCommand>
                        </Commands>
                        <Listeners>
                            <Command Fn="EliminarFormato" />
                        </Listeners>
                    </ext:CommandColumn>
                </Columns>
            </ColumnModel>
            <View>
                <ext:GridView runat="server" EnableTextSelection="true">
                </ext:GridView>
            </View>

            <DockedItems>

                <ext:Toolbar runat="server" Height="40" Cls="toolbarHacked">
                    <Items>
                        <ext:Button ID="btnAddFormatRule" Cls="btnAligned" runat="server" IconCls="ico-plus-green" Text="<%$ Resources:Comun, strAnadirReglaFormato %>">
                            <Listeners>
                                <Click Fn="BotonAddFormatRule" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnCloseFormatRule" Cls="btnAlignedClose" runat="server" IconCls="ico-close-gr" ToolTip="<%$ Resources:Comun, jsCerrar %>">
                            <Listeners>
                                <Click Fn="hideGridFormatRule" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </DockedItems>
        </ext:GridPanel>
        <ext:GridPanel runat="server"
            Hidden="true"
            ID="GridAddCondition"
            Height="200"
            Cls="gridPanelSize"
            Width="280"
            Scrollable="Vertical">
            <Listeners>
                <FocusLeave Fn="hideGridAddCondition" />
            </Listeners>

            <Store>
                <ext:Store
                    ID="Store9"
                    runat="server"
                    Buffered="true"
                    RemoteFilter="true"
                    LeadingBufferZone="1000"
                    PageSize="50">
                    <Model>
                        <ext:Model runat="server" IDProperty="Id">
                            <Fields>
                                <ext:ModelField Name="Id" />
                                <ext:ModelField Name="Type" />
                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel runat="server">
                <Columns>
                    <ext:Column runat="server" DataIndex="Type" Flex="1"></ext:Column>
                    <ext:CommandColumn runat="server" Width="50" Align="Center">
                        <Commands>
                            <ext:GridCommand IconCls="ico-close"></ext:GridCommand>
                        </Commands>
                    </ext:CommandColumn>
                </Columns>
            </ColumnModel>
            <View>
                <ext:GridView runat="server" EnableTextSelection="true">
                </ext:GridView>
            </View>

            <DockedItems>

                <ext:Toolbar runat="server" Height="40" Cls="toolbarHacked">
                    <Items>
                        <ext:Button Cls="btnAligned" runat="server" IconCls="ico-plus-green" Text="Add Condition" Handler="winNewCondition()">
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </DockedItems>
        </ext:GridPanel>
        <ext:GridPanel runat="server"
            Hidden="true"
            ID="GridAddRestriction"
            Height="200"
            Cls="gridPanelSize"
            Width="250"
            StoreID="storeRolesRestringidos"
            Scrollable="Vertical">
            <ColumnModel runat="server">
                <Columns>
                    <ext:Column runat="server" DataIndex="Nombre" Flex="1" EmptyCellText="<%$ Resources:Comun, strDefecto %>"></ext:Column>
                    <ext:Column runat="server" DataIndex="Restriccion" Width="50">
                        <Renderer Fn="rendeAddRestriction" />
                    </ext:Column>
                    <ext:CommandColumn ID="colEliminarRestriction" runat="server" Width="50" Align="Center">
                        <Commands>
                            <ext:GridCommand CommandName="EliminarRestriction" IconCls="ico-close">
                            </ext:GridCommand>
                        </Commands>
                        <Listeners>
                            <Command Fn="EliminarRestriccion" />
                        </Listeners>
                    </ext:CommandColumn>
                </Columns>
            </ColumnModel>
            <View>
                <ext:GridView runat="server" EnableTextSelection="true">
                </ext:GridView>
            </View>
            <Listeners>
                <CellClick Fn="winAddRestriction"></CellClick>
            </Listeners>

            <DockedItems>

                <ext:Toolbar runat="server" Height="40" Cls="toolbarHacked">
                    <Items>
                        <ext:Button ID="btnAddRestriction" Cls="btnAligned" runat="server" IconCls="ico-plus-green" Text="<%$ Resources:Comun, strAnadirRestriccion %>">
                            <Listeners>
                                <Click Fn="winAddRestriction" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnCloseRestriction" Cls="btnAlignedClose" runat="server" IconCls="ico-close-gr" ToolTip="<%$ Resources:Comun, jsCerrar %>">
                            <Listeners>
                                <Click Fn="hideGridAddRestriction" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </DockedItems>
        </ext:GridPanel>
    </Items>
</ext:Container>
