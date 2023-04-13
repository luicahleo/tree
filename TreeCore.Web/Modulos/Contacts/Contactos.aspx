<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contactos.aspx.cs" Inherits="TreeCore.ModGlobal.pages.Contactos" %>

<%@ Register Src="../../Componentes/GridEmplazamientosContactosResp.ascx" TagName="GridContactosGlobalesResp" TagPrefix="local" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <link href="/Modulos/Inventory/css/styleInventarioCategorias.css" rel="stylesheet" type="text/css" />
    <link href="/Componentes/css/Geoposicion.css" rel="stylesheet" type="text/css" />
    <link href="css/styleEmplazamientos.css" rel="stylesheet" type="text/css" />
    <link href="/Componentes/css/FormEmplazamientos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Componentes/js/GridEmplazamientosContactosResp.js"></script>

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <DocumentReady Fn="bindParams" />
                </Listeners>
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

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
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="MainVwP"
                OverflowY="auto"
                Layout="FitLayout">
                <Items>
                    <ext:Panel ID="pnWrapMainLayout"
                        runat="server"
                        Header="false"
                        Cls="pnCentralWrap"
                        BodyCls="pnCentralWrap-body"
                        Layout="BorderLayout">
                        <Content>
                            <ext:Button runat="server"
                                ID="btnCollapseAsRClosed"
                                Cls="btn-trans btnCollapseAsRClosedv3"
                                Handler="hideAsideR();"
                                Disabled="false"
                                Hidden="true">
                            </ext:Button>
                        </Content>
                        <Items>
                            <%-- PANEL CENTRAL--%>
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
                                    <ext:Container runat="server" 
                                        ID="WrapAlturaCabecera" 
                                        Dock="Top" 
                                        Layout="VBoxLayout">
                                        <LayoutConfig>
                                            <ext:VBoxLayoutConfig Align="Stretch">
                                            </ext:VBoxLayoutConfig>
                                        </LayoutConfig>
                                        <Items>
                                            <ext:Toolbar runat="server" 
                                                ID="tbTitulo" 
                                                Dock="Top" 
                                                Cls="tbGrey tbTitleAlignBot tbNoborder" 
                                                Hidden="false" 
                                                Layout="ColumnLayout" 
                                                Flex="1">
                                                <Items>
                                                    <ext:Label runat="server" 
                                                        ID="lbltituloPrincipal" 
                                                        Cls="TituloCabecera" 
                                                        Text="<%$ Resources:Comun, strContactos %>" 
                                                        Height="25" 
                                                        MarginSpec="10 0 10 0" />
                                                </Items>
                                            </ext:Toolbar>
                                        </Items>
                                    </ext:Container>
                                </DockedItems>
                                <Items>
                                    <ext:Panel runat="server"
                                        ID="ctMain1"
                                        Flex="4"
                                        Layout="FitLayout"
                                        Cls="tbGrey "
                                        Hidden="false"
                                        Margin="0">
                                        <Content>
                                            <local:GridContactosGlobalesResp ID="gridContactos"
                                                vista="ContactoGlobal"
                                                runat="server" />
                                        </Content>
                                    </ext:Panel>
                                </Items>
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
                                Layout="AnchorLayout"
                                Header="false"
                                Border="false"
                                Hidden="false">
                                <Items>
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
                                                        ID="Label3"
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
                                                <table class="tmpCol-table"
                                                    id="tablaInfoElementos">
                                                    <tbody id="bodyTablaInfoElementos">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </Content>

                                    </ext:Panel>
                                    <ext:Container runat="server" ID="WrapFilterControls" Hidden="true" Cls="tbGrey">
                                        <Items>


                                            <ext:Label meta:resourcekey="lblAsideNameR" ID="lblAsideNameR" runat="server" IconCls="ico-head-info" Cls="lblHeadAside " Text="Additional INFO" PaddingSpec="0 0 20 0"></ext:Label>


                                            <ext:Panel meta:resourcekey="ctAsideR" ID="ctAsideR" runat="server" Border="false" Header="false" Cls="ctAsideR">
                                                <Items>
                                                    <%--LEFT TABS MENU--%>
                                                    <ext:Panel ID="mnAsideR" runat="server" Border="false" Header="false" Hidden="false">
                                                        <Items>
                                                            <ext:Button runat="server" meta:resourcekey="btnInfoGrid" ID="btnCreateFilters" Cls="btnFiltersPlus-asR" Handler="displayMenu('pnCFilters')"></ext:Button>
                                                            <ext:Button runat="server" meta:resourcekey="btnVersions" ID="btnMyFilters" Cls="btnMyFilters-asR" Handler="displayMenu('pnGridsAsideMyFilters')"></ext:Button>

                                                        </Items>
                                                    </ext:Panel>





                                                    <%--CREATE FILTERS PANEL--%>
                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnCFilters"
                                                        Hidden="false">
                                                        <Items>
                                                            <ext:Label
                                                                meta:resourcekey="lblGrid"
                                                                ID="lblGrid"
                                                                runat="server"
                                                                IconCls="btn-CFilter"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strCrearFiltro %>">
                                                            </ext:Label>
                                                            <ext:Panel
                                                                runat="server"
                                                                ID="pnCFiltersContainer">
                                                                <Items>
                                                                    <ext:TextField runat="server"
                                                                        meta:resourcekey="pnNewFilter"
                                                                        ID="pnNewFilter"
                                                                        FieldLabel=""
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        CausesValidation="true"
                                                                        EmptyText="<%$ Resources:Comun, strNombreFiltro %>" />
                                                                    <ext:Button
                                                                        MarginSpec="0 0 0 10"
                                                                        runat="server"
                                                                        ID="btnFilter"
                                                                        Cls="btn-add"
                                                                        Text="New Filter">
                                                                        <Listeners>
                                                                            <%--<Click Fn="newFilter" />--%>
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:ComboBox runat="server"
                                                                        meta:resourcekey="cmbField"
                                                                        ID="cmbField"
                                                                        FieldLabel="<%$ Resources:Comun, strCampo %>"
                                                                        LabelAlign="Top"
                                                                        DisplayField="Name"
                                                                        EmptyText="<%$ Resources:Comun, strCampo %>"
                                                                        Flex="1"
                                                                        MarginSpec="0 10 0 0"
                                                                        Cls="pnForm fieldFilter"
                                                                        ValueField="Id"
                                                                        QueryMode="Local">
                                                                        <Store>
                                                                            <ext:Store
                                                                                ID="storeCampos"
                                                                                runat="server"
                                                                                AutoLoad="true">
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="Id">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="Id" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                            <ext:ModelField Name="typeData" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                                <Listeners>
                                                                                    <%--         <DataChanged Fn="beforeLoadCmbField" />--%>
                                                                                </Listeners>
                                                                            </ext:Store>
                                                                        </Store>
                                                                        <Listeners>
                                                                            <%--        <Select Fn="selectField" />--%>
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                    <ext:TextField runat="server"
                                                                        meta:resourcekey="pnSearch"
                                                                        ID="textInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        CausesValidation="true"
                                                                        EmptyText="<%$ Resources:Comun, strDependeDelCampo %>"
                                                                        Cls="pnForm"
                                                                        Hidden="false" />
                                                                    <ext:DateField
                                                                        runat="server"
                                                                        ID="dateInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        Cls="pnForm"
                                                                        Hidden="true"
                                                                        Format="dd/MM/yyyy">
                                                                    </ext:DateField>
                                                                    <ext:NumberField
                                                                        runat="server"
                                                                        ID="numberInputSearch"
                                                                        FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                        LabelAlign="Top"
                                                                        AllowBlank="false"
                                                                        ValidationGroup="FORM"
                                                                        Cls="pnForm"
                                                                        Hidden="true">
                                                                    </ext:NumberField>
                                                                    <ext:ComboBox
                                                                        ID="cmbOperatorField"
                                                                        runat="server"
                                                                        FieldLabel="<%$ Resources:Comun, strOperador %>"
                                                                        DisplayField="Name"
                                                                        EmptyText="<%$ Resources:Comun, strOperador %>"
                                                                        Cls="pnForm"
                                                                        Flex="1"
                                                                        LabelAlign="Top"
                                                                        Hidden="true"
                                                                        QueryMode="Local">
                                                                        <Items>
                                                                            <ext:ListItem Text="=" Value="IGUAL" />
                                                                            <ext:ListItem Text="<" Value="MENOR" />
                                                                            <ext:ListItem Text=">" Value="MAYOR" />
                                                                        </Items>
                                                                    </ext:ComboBox>
                                                                    <ext:MultiCombo
                                                                        ID="cmbTiposDinamicos"
                                                                        runat="server"
                                                                        FieldLabel="tiposDinamicos"
                                                                        DisplayField="Name"
                                                                        EmptyText="tiposDinamicos"
                                                                        Cls="pnForm"
                                                                        Flex="1"
                                                                        LabelAlign="Top"
                                                                        Hidden="true"
                                                                        ValueField="Id"
                                                                        SelectionMode="Selection"
                                                                        QueryMode="Local">
                                                                        <Store>
                                                                            <ext:Store
                                                                                ID="storeTiposDinamicos"
                                                                                runat="server"
                                                                                AutoLoad="false">
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="Id">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="Id" />
                                                                                            <ext:ModelField Name="Name" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                                <Listeners>
                                                                                    <%--      <DataChanged Fn="beforeLoadCmbField" />--%>
                                                                                </Listeners>
                                                                            </ext:Store>
                                                                        </Store>
                                                                    </ext:MultiCombo>
                                                                    <ext:Button
                                                                        runat="server" meta:resourcekey="ColMas"
                                                                        ID="btnAdd"
                                                                        IconCls="ico-addBtn"
                                                                        Cls="btn-mini-ppal btnAdd"
                                                                        Text="<%$ Resources:Comun, jsAgregar %>">
                                                                        <Listeners>
                                                                            <%--     <Click Fn="addElementFilter" />--%>
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:Panel runat="server" ID="tagsHeader">
                                                                        <Items>
                                                                            <ext:Label
                                                                                meta:resourcekey="lblCampo"
                                                                                runat="server"
                                                                                Cls="tabsLabels"
                                                                                Text="<%$ Resources:Comun, strCampo %>">
                                                                            </ext:Label>
                                                                            <ext:Label
                                                                                meta:resourcekey="lblBuscar"
                                                                                runat="server"
                                                                                Cls="tabsLabels"
                                                                                Text="<%$ Resources:Comun, strBuscar %>">
                                                                            </ext:Label>
                                                                        </Items>
                                                                    </ext:Panel>
                                                                    <ext:Panel runat="server" ID="pnTagContainer" Scrollable="Vertical" MaxHeight="100" MarginSpec="0 190 8 0">
                                                                        <Items>
                                                                            <%-- <ext:Panel runat="server" Cls="pntag" ID="pnTags" Hidden="false">
                                                                        <Items>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagField" Text="Create Filter"></ext:Label>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagSearch" Text="Create Filter"></ext:Label>
                                                                            <ext:Button runat="server" ID="ButtonClose" Cls="btnCloseTag" FocusCls="none"></ext:Button>
                                                                        </Items>
                                                                    </ext:Panel>
                                                                    <ext:Panel runat="server" Cls="pntag">
                                                                        <Items>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagField" Text="Create Filter"></ext:Label>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagSearch" Text="Create Filter"></ext:Label>
                                                                            <ext:Button runat="server" ID="Button1" Cls="btnCloseTag" FocusCls="none" Handler=""></ext:Button>
                                                                        </Items>
                                                                    </ext:Panel>
                                                                    <ext:Panel runat="server" Cls="pntag">
                                                                        <Items>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagField" Text="Create Filter"></ext:Label>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagSearch" Text="Create Filter"></ext:Label>
                                                                            <ext:Button runat="server" ID="Button2" Cls="btnCloseTag" FocusCls="none" Handler=""></ext:Button>
                                                                        </Items>
                                                                    </ext:Panel>
                                                                    <ext:Panel runat="server" Cls="pntag">
                                                                        <Items>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagField" Text="Create Filter"></ext:Label>
                                                                            <ext:Label meta:resourcekey="lblCrearFiltros" runat="server" Cls="pnTagSearch" Text="Create Filter"></ext:Label>
                                                                            <ext:Button runat="server" ID="Button3" Cls="btnCloseTag" FocusCls="none" Handler=""></ext:Button>
                                                                        </Items>
                                                                    </ext:Panel>--%>
                                                                        </Items>
                                                                    </ext:Panel>

                                                                    <ext:Button
                                                                        runat="server"
                                                                        meta:resourcekey="btnAplyFilter"
                                                                        ID="btnAplyFilter"
                                                                        Cls="btn-end"
                                                                        Text="<%$ Resources:Comun, strAplicar %>">
                                                                        <Listeners>
                                                                            <%--          <Click Fn="aplyFilter" />--%>
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                    <ext:Button
                                                                        runat="server"
                                                                        meta:resourcekey="btnSaveFilter"
                                                                        ID="btnSaveFilter"
                                                                        Cls="btn-save"
                                                                        Text="<%$ Resources:Comun, btnGuardar.Text %>">
                                                                        <Listeners>
                                                                            <%--       <Click Fn="saveFilter" />--%>
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Panel>
                                                        </Items>
                                                    </ext:Panel>

                                                    <%--MY FILTERS PANEL--%>
                                                    <ext:Panel
                                                        ID="pnGridsAsideMyFilters"
                                                        runat="server"
                                                        Border="false"
                                                        Header="false"
                                                        Scrollable="Vertical"
                                                        OverflowY="Scroll"
                                                        Hidden="true"
                                                        Cls="">
                                                        <Items>

                                                            <ext:Label
                                                                meta:resourcekey="lblMyFilters"
                                                                ID="Label4"
                                                                runat="server"
                                                                IconCls="ico-head-my-filters"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strMisFiltros %>">
                                                            </ext:Label>
                                                            <ext:Panel
                                                                runat="server"
                                                                ID="pnMFiltersContainer"
                                                                Layout="FitLayout"
                                                                Hidden="false">
                                                                <Items>
                                                                    <ext:GridPanel
                                                                        ID="GridMyFilters"
                                                                        runat="server"
                                                                        Header="false"
                                                                        Border="false"
                                                                        Width="100"
                                                                        Cls="GridMyFilters"
                                                                        Scrollable="Vertical"
                                                                        Height="600">
                                                                        <Store>
                                                                            <ext:Store
                                                                                runat="server"
                                                                                PageSize="10"
                                                                                AutoLoad="false">
                                                                                <Model>
                                                                                    <ext:Model runat="server" IDProperty="ID">
                                                                                        <Fields>
                                                                                            <ext:ModelField Name="GestionFiltroID" />
                                                                                            <ext:ModelField Name="UsuarioID" />
                                                                                            <ext:ModelField Name="NombreFiltro" />
                                                                                            <ext:ModelField Name="JsonItemsFiltro" />
                                                                                            <ext:ModelField Name="Pagina" />
                                                                                            <ext:ModelField Name="check" Type="Boolean" DefaultValue="false" />
                                                                                        </Fields>
                                                                                    </ext:Model>
                                                                                </Model>
                                                                            </ext:Store>
                                                                        </Store>
                                                                        <ColumnModel runat="server">
                                                                            <Columns>

                                                                                <ext:Column runat="server"
                                                                                    Sortable="true"
                                                                                    DataIndex="NombreFiltro"
                                                                                    Width="150"
                                                                                    Align="Start">
                                                                                </ext:Column>

                                                                                <ext:WidgetColumn
                                                                                    meta:resourcekey="ColMas"
                                                                                    ID="ColMas"
                                                                                    runat="server"
                                                                                    Width="15"
                                                                                    Cls="col-More"
                                                                                    Align="Center"
                                                                                    Hidden="false"
                                                                                    MinWidth="45">
                                                                                    <Widget>
                                                                                        <ext:Button
                                                                                            meta:resourcekey="btnColMore"
                                                                                            runat="server"
                                                                                            ID="btnColMore"
                                                                                            Width="16"
                                                                                            OverCls="Over-btnMore"
                                                                                            PressedCls="Pressed-none"
                                                                                            FocusCls="Focus-none"
                                                                                            Cls="BtnDeleteChk">
                                                                                            <Listeners>
                                                                                                <%--          <Click Fn="DeleteFilter" />--%>
                                                                                            </Listeners>
                                                                                        </ext:Button>
                                                                                    </Widget>
                                                                                </ext:WidgetColumn>

                                                                                <ext:WidgetColumn
                                                                                    meta:resourcekey="ColMas"
                                                                                    ID="colBtnEdit"
                                                                                    runat="server"
                                                                                    Width="18"
                                                                                    Cls="col-More"
                                                                                    Align="Center"
                                                                                    Hidden="false"
                                                                                    MinWidth="45">
                                                                                    <Widget>
                                                                                        <ext:Button
                                                                                            meta:resourcekey="btnColMore"
                                                                                            runat="server"
                                                                                            ID="btnCheck"
                                                                                            Width="18"
                                                                                            OverCls="Over-btnMore"
                                                                                            PressedCls="Pressed-none"
                                                                                            FocusCls="Focus-none"
                                                                                            Cls="BtnEditChk">
                                                                                            <Listeners>
                                                                                                <%--     <Click Fn="MostrarEditarFiltroGuardado" />--%>
                                                                                            </Listeners>
                                                                                        </ext:Button>
                                                                                    </Widget>
                                                                                </ext:WidgetColumn>

                                                                                <ext:WidgetColumn
                                                                                    meta:resourcekey="ColMas"
                                                                                    ID="colChkAplyFilter"
                                                                                    runat="server"
                                                                                    Width="18"
                                                                                    Cls="col-Chk"
                                                                                    Align="Center"
                                                                                    Hidden="false"
                                                                                    MinWidth="45">
                                                                                    <Widget>
                                                                                        <ext:Button
                                                                                            runat="server"
                                                                                            ID="chkAplyFilter"
                                                                                            Cls="btn-mini-ppal btnApply ico-tic-wh"
                                                                                            DataIndex="check">
                                                                                            <Listeners>
                                                                                                <%--     <Click Fn="AplyFilterSaved" />--%>
                                                                                            </Listeners>
                                                                                        </ext:Button>
                                                                                    </Widget>
                                                                                </ext:WidgetColumn>

                                                                            </Columns>
                                                                        </ColumnModel>
                                                                        <View>
                                                                            <ext:GridView runat="server" LoadMask="false" />
                                                                        </View>
                                                                        <Plugins>
                                                                            <ext:GridFilters runat="server" />
                                                                        </Plugins>
                                                                    </ext:GridPanel>
                                                                </Items>
                                                            </ext:Panel>

                                                        </Items>
                                                    </ext:Panel>


                                                </Items>
                                            </ext:Panel>


                                        </Items>
                                    </ext:Container>
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
