<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="TreeCore.Componentes.Header" %>

<link href="/Componentes/css/Header.min.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="/Componentes/js/Header.js"></script>
<!--<script type="text/javascript" src="/JS/common.js"></script>
<script type="text/javascript" src="/JS/common.js"></script>-->

<ext:Hidden ID="hdClienteID" runat="server" Name="hdClienteIDComponente" />

<ext:Window ID="winacerca" runat="server" Title="<%$ Resources:Comun, strAcercaDeTree %>"
    Width="350" Height="375" BodyStyle="padding: 6px;" CloseAction="Hide" Modal="true"
    Collapsible="false" ShowOnLoad="false" Hidden="true">
    <Items>
        <ext:Image runat="server" ID="logoClienteLogin" ImageUrl="/ima/ima-logo-login.svg" Width="300" MarginSpec="17 17 25 17"></ext:Image>
        <ext:Label ID="Version" runat="server" Cls="abouttitles" Text="<%$ Resources:Comun, strVersion %>" />
        <ext:Label ID="lblVersion" runat="server" Cls="aboutcontent" Text="" />

        <ext:Label ID="AssemblyVersion" runat="server" Cls="abouttitles" Text="<%$ Resources:Comun, strAssemblyVersion %>" />
        <ext:Label ID="lblVersionAssembly" runat="server" Cls="aboutcontent" Text="" />

        <ext:Label ID="AssemblyFileVersion" runat="server" Cls="abouttitles" Text="<%$ Resources:Comun, strAssemblyFileVersion %>" />
        <ext:Label ID="lblVersionAssemblyFile" runat="server" Cls="aboutcontent" Text="" />

        <ext:Label ID="BBDDVersion" runat="server" Cls="abouttitles" Text="<%$ Resources:Comun, strBBDDVersion %>" />
        <ext:Label ID="lblBBDDVersion" runat="server" Cls="aboutcontent" Text="" />
    </Items>
    <Buttons>
        <ext:Button ID="Acerca" runat="server" Cls="btn-ppal" Text="<%$ Resources:Comun, strAceptar %>">
            <Listeners>
                <Click Fn="AcercaDeCerrar" />
            </Listeners>
        </ext:Button>
    </Buttons>
</ext:Window>

<header id="hdDefault" class="cabecera" role="banner">
    <div id="logoTree" class="d-flx">
        <a onclick="openGlobal()" href="/" style="align-self: center;">
            <img src="/ima/logo-treePlatform.svg"
                alt="logo TREE.platform" class="logo" id="logo-tree"></a>
        <img src="/ima/logo-treePlatform-only.svg"
            alt="logo TREE.platform" class="logo-only">
        <ext:Label runat="server"
            ID="lblModulo"
            Cls="h5">
        </ext:Label>
    </div>

    <nav id="menuCabecera" role="menu">
        <ul id="ulMenuCabecera" class="d-flx">
            <li id="liSearchTB" onmouseover="ExtendCombo();" class="d-none">
                <ext:ComboBox
                    meta:resourcekey="btnSearchTB"
                    ID="btnSearchTB"
                    runat="server"
                    Width="40"
                    Editable="true"
                    DisplayField="state"
                    ValueField="abbr"
                    QueryMode="Local"
                    ForceSelection="true"
                    TriggerAction="All"
                    EmptyText="<%$ Resources:Comun, strBusqueda %>"
                    Cls="cmbExtendTest">
                    <Store>
                        <ext:Store ID="StoreCmbSearch" runat="server">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="abbr" />
                                        <ext:ModelField Name="state" />
                                        <ext:ModelField Name="nick" />
                                        <ext:ModelField Name="price" Type="Float" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <Triggers>
                        <ext:FieldTrigger Icon="Search" QTip="Custom tip" />
                    </Triggers>

                    <Listeners>
                        <Select Handler="ShowGlobalSearch();" />
                    </Listeners>
                    <ListConfig>
                        <ItemTpl runat="server">
                            <Html>
                                <div class="list-item SearchBarGlobal">
                                    <p>{state}</p>
                                    <table>
                                        <tr>
                                            <td class="OrangBack">{nick:ellipsis(25)}</td>
                                            <td class="OrangBack"> {price:usMoney}</td>
                                            <td class="OrangBack">{abbr}</td>
                                        </tr>
                                    </table>                               
                                </div>
                            </Html>
                        </ItemTpl>
                    </ListConfig>
                </ext:ComboBox>
                <span id="ShowTipoSearch" class="Search-Tipo-ico" onmouseover="ExtendCombo();">
                    <ext:ComboBox
                        ID="ComboTipoSearch"
                        runat="server"
                        Width="20"
                        Editable="false"
                        DisplayField="name"
                        ValueField="iconCls"
                        QueryMode="Local"
                        Hidden="true">
                        <Store>
                            <ext:Store runat="server">
                                <Model>
                                    <ext:Model runat="server">
                                        <Fields>
                                            <ext:ModelField Name="iconCls" />
                                            <ext:ModelField Name="name" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ListConfig>
                            <ItemTpl runat="server">
                                <Html>
                                    <div class="icon-combo-item {iconCls}">
                                                    {name}
                                    </div>
                                </Html>
                            </ItemTpl>
                        </ListConfig>
                        <Listeners>
                            <%-- <Change Handler="if(this.valueModels.length>0){this.setIconCls(this.valueModels[0].get('iconCls'));}" />--%>
                            <Select Handler="UpdateIcon();" />
                        </Listeners>
                    </ext:ComboBox>
                </span>
                <span class="d-flx Search-ico" onmouseover="ExtendCombo();" onmousedown="HideCombo();">
                    <img src="/ima/ico-search-topbar.svg" class="showTopbar" id="icosearchtopbar" />
                </span>
            </li>

            <li id="liModulos">
                <ext:Button runat="server"
                    ID="btnModulos"
                    Cls="btn-trans">
                </ext:Button>
                <ul id="mnModulos">
                    <ext:Container runat="server"
                        ID="ctMenuModulos">
                        <Listeners>
                            <AfterRender Fn="ajaxPintarMenuModulos" />
                        </Listeners>
                    </ext:Container>
                    <ext:Container runat="server" Hidden="true" ID="ctlBotonMostrar" Cls="ctlBotonMostrar">
                        <Items>
                            <ext:Button runat="server"
                                ID="btnMostrar"
                                Cls="btnMostrar"
                                Focusable="false"
                                Text="<%$ Resources:Comun, jsMostrarMas %>">
                                <Listeners>
                                    <Click Fn="mostrarElementos" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Container>
                </ul>

            </li>

            <li id="liUsuarios">
                <ext:Button runat="server"
                    ID="btnUsuarios"
                    Cls="btn-trans">
                </ext:Button>
                <ul id="mnUsuarios">
                    <li>
                        <ext:Container runat="server"
                            ID="ContMenuUser"
                            Layout="FormLayout"
                            MaxHeight="300"
                            Width="220"
                            Cls="UserCont1">
                            <Items>
                                <ext:Image runat="server"
                                    ID="imgUser"
                                    Cls="imgUser"
                                    Width="100"
                                    src=""
                                    Height="100">
                                </ext:Image>
                                <ext:Label runat="server"
                                    ID="lblNombre"
                                    Cls="bigSubtitle">
                                </ext:Label>
                                <ext:ToolbarFill></ext:ToolbarFill>
                                <ext:Label runat="server"
                                    ID="lblEmail"
                                    Cls="mnUser-lblEmail">
                                </ext:Label>
                                <ext:ToolbarFill></ext:ToolbarFill>
                                <ext:Button runat="server"
                                    ID="btnEditUser"
                                    Text="<%$ Resources:Comun, strEditarPerfil %>"
                                    Cls="btn-ppal btnedituser"
                                    Focusable="false"
                                    Handler="BotonUsuarios();">
                                </ext:Button>
                            </Items>
                        </ext:Container>
                        <%--    <ext:HyperlinkButton runat="server"
                                        ID="lnkDatosUsuario"
                                        Handler="BotonNavUsuario();"
                                        meta:resourcekey="lnkDatosUsuario"
                                        IconCls="ico-usuario"
                                        Text="Datos Usuario">
                                    </ext:HyperlinkButton>--%>
                    </li>
                    <%--<li>
									<ext:HyperlinkButton runat="server" 
										ID="lnkConfiguracion" 
										meta:resourceKey="lnkConfiguracion"
										IconCls="ico-configuracion" 
										Text="Configuración">
									</ext:HyperlinkButton>
								</li>--%>
                    <li hidden>
                        <ext:HyperlinkButton runat="server"
                            ID="lnkAvisos"
                            Cls="mn-itemLnk"
                            IconCls="ico-avisos"
                            Text="<%$ Resources:Comun, strAvisos %>">
                        </ext:HyperlinkButton>
                    </li>
                    <li hidden>
                        <ext:HyperlinkButton runat="server"
                            ID="lnkSoporte"
                            Cls="mn-itemLnk"
                            IconCls="ico-soporte"
                            Text="<%$ Resources:Comun, strSoporte %>">
                        </ext:HyperlinkButton>
                    </li>
                    <li>
                        <ext:HyperlinkButton runat="server"
                            ID="lnkAcerca"
                            Cls="mn-itemLnk"
                            IconCls="ico-acerca"
                            Text="<%$ Resources:Comun, strAcercaDe %>">
                            <Listeners>
                                <Click Fn="AcercaDe" />
                            </Listeners>
                        </ext:HyperlinkButton>
                    </li>
                    <li>
                        <ext:HyperlinkButton runat="server"
                            ID="lnkCerrar"
                            Cls="mn-itemLnk"
                            IconCls="ico-cerrarSesion"
                            Text="<%$ Resources:Comun, strCerrarSesion %>">
                            <DirectEvents>
                                <Click OnEvent="Logout"
                                    Success="RecargarInicio();">
                                    <EventMask ShowMask="true"
                                        Msg="#{strSalida}" />
                                </Click>
                            </DirectEvents>
                        </ext:HyperlinkButton>
                    </li>
                </ul>
            </li>
        </ul>
    </nav>
</header>
