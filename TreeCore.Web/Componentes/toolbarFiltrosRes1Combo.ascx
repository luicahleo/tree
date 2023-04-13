<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="toolbarFiltrosRes1Combo.ascx.cs" Inherits="TreeCore.Componentes.toolbarFiltrosRes1Combo" %>

<link href="../../../CSS/tCore.css" rel="stylesheet" type="text/css" />

<link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet" />
<script type="text/javascript" src="../../Componentes/js/toolbarFiltrosRes1Combo.js"></script>
<!--<script type="text/javascript" src="../JS/common.js"></script>-->

<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteIDComponente" />
<ext:Hidden ID="hdStoresID" runat="server" Name="hdStores" />
<ext:Hidden ID="hdGrid" runat="server" Name="hdGrid" />


<ext:Toolbar runat="server"
    ID="tbFiltros"
    Dock="Top"
    Layout="ColumnLayout"
    Cls="tlbGridRes ">
    <Listeners>
        <AfterRender Fn="StyleOnResize" />
        <Resize Fn="StyleOnResize"></Resize>
    </Listeners>
    <Items>

        <ext:TextField
            ID="txtSearch"
            Cls="mainSearchBox  "
            runat="server"
            EmptyText="Search"
            LabelWidth="50"
            Width="300"
            EnableKeyEvents="true">
            <Triggers>
                <ext:FieldTrigger Icon="Search" />
            </Triggers>
            <Listeners>
                <%--  <KeyUp Fn="FiltrarColumnas" Buffer="250" />
                <TriggerClick Fn="LimpiarFiltroBusqueda" />--%>
            </Listeners>
        </ext:TextField>

        <ext:Component runat="server" Cls="filler"></ext:Component>




        <ext:Container runat="server" ID="wrapBotonesYcmbFiltros" Layout="HBoxLayout" Cls="wrapBotonesYcmbFiltros">
            <Items>

                <ext:Container meta:resourceKey="ContainerTBButtons" runat="server" ID="ContainerTBButtons" Layout="HBoxLayout" Cls="GrupoBtnFilters" Hidden="false" Width="90">
                    <Items>
                        <ext:Button meta:resourceKey="btnDuplicarTipo" runat="server" Width="30" ID="btnGestionarColumnas" Cls="btn-trans btnColumnas" ToolTip="Gestionar Columnas" Handler="hideAsideR('panelColumnas');"></ext:Button>
                        <ext:Button meta:resourceKey="btnQuitarFiltros" runat="server" Width="30" ID="btnQuitarFiltros" Cls="btn-trans btnRemoveFilters" ToolTip="Quitar Filtros"></ext:Button>
                        <ext:Button meta:resourceKey="btnFiltroNegativo" runat="server" Width="30" ID="btnFiltroNegativo" Cls="btn-trans btnFiltroNegativo" ToolTip="Filtro Negativo"></ext:Button>
                    </Items>
                </ext:Container>

                <ext:ComboBox meta:resourceKey="cmbMisFiltros" runat="server" ID="cmbMisFiltros" Cls="comboGrid cmbMisfiltros " EmptyText="My Filters" Hidden="false">
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






    </Items>
</ext:Toolbar>

