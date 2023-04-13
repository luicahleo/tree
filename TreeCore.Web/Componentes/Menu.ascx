<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="TreeCore.Componentes.Menu" %>


<link href="/Componentes/css/styleMenu.min.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="/Componentes/js/Menu.js"></script>

<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteIDComponente" />
<ext:Hidden ID="hd_IdComponente" runat="server" />

<aside id="asideLeft" class="aside" onmouseover="displayMenu('over')" onmouseleave="displayMenu('out')">

    <%--TOP--%>
    <ext:Button runat="server"
        ID="btnAbierto"
        Cls="ico-menu-plegable-abierto btnDisplayMn"
        EnableToggle="true">
        <Listeners>
            <Toggle Handler="collapsingMenu();" />
        </Listeners>
    </ext:Button>

    <div id="cliente" class="dvLogoCliente">
        <ext:Image
            ID="logoCliente"
            runat="server"
            Cls="logoCliente"
            Alt="<%$ Resources:Comun, strAltLogoCliente %>"
            ImageUrl="/ima/LOGOAtrebo.png">
        </ext:Image>

     <%--   <ext:Label runat="server"
            ID="lblNombreCliente"
            meta:resourcekey="lblNombreCliente"
            Cls="nombreCliente">
        </ext:Label>--%>
    </div>

    <%--<div id="cliente" class="cliente">
        <img id="logoCliente"
            class="logoCliente"
            src="/ima/imgEntornoDemo.svg"
            alt="Logo Cliente" />
        <ext:Label runat="server"
            ID="lblNombreCliente"
            meta:resourcekey="lblNombreCliente"
            Cls="nombreCliente">
        </ext:Label>
    </div>--%>
    <hr />
    <%--NAVIGATION--%>
    <ext:TreePanel
        ID="Tree"
        runat="server"
        Scrollable="Vertical"
        RootVisible="false"
        SingleExpand= "true"
        Cls="treeMenu menuInitial">
        <ColumnModel>
            <Columns>
                <ext:TreeColumn
                    runat="server"
                    Flex="1"
                    DataIndex="text" />
            </Columns>
        </ColumnModel>
        
        <Listeners>
            <ItemClick Fn="SelectItemMenu" />
            <AfterRender Fn="resizeMenu" />
        </Listeners>

    </ext:TreePanel>
    <%--FOOTER--%>

    <div id="infoEntorno" class="info-entorno">
        <ext:Label runat="server"
            ID="lblVersion"
            meta:resourcekey="lblVersion"
            Cls="version"
            Text="">
        </ext:Label>
        <ext:Label runat="server"
            ID="lblEntorno"
            meta:resourcekey="lblEntorno"
            Cls="version"
            Text="">
        </ext:Label>
    </div>
</aside>
