<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoriasAtributos.ascx.cs" Inherits="TreeCore.Componentes.CategoriasAtributos" %>

<%--INICIO HIDDEN --%>

<%--FIN HIDDEN --%>

<%--Stores--%>


<%--Componente--%>
<ext:FieldSet
    runat="server"
    ID="containerAttributes"
    Cls="containerAttributes">
    <Defaults>
        <ext:Parameter Name="labelWidth" Value="89" Mode="Raw" />
    </Defaults>

    <Items>
        <ext:Toolbar runat="server" ID="tlbTooooolbar" Cls="tlbAddAttribute">
            <Items>
                <ext:Button ID="btnMasOrden"
                    runat="server"
                    IconCls="ico-expand-combo-up"
                    Cls="btnArrow"
                    Hidden="true"
                    ToolTip="<%$ Resources:Comun, strBeforeText %>">
                    <Listeners>
                        <Click Fn="OrdenMasCategoria" />
                    </Listeners>
                </ext:Button>
                <ext:Button ID="btnMenosOrden"
                    runat="server"
                    IconCls="ico-expand-combo"
                    Cls="btnArrow"
                    Hidden="true"
                    ToolTip="<%$ Resources:Comun, strAfterText %>">
                    <Listeners>
                        <Click Fn="OrdenMenosCategoria" />
                    </Listeners>
                </ext:Button>
                <ext:Button ID="btnMoverCategoria"
                    runat="server"
                    OverCls="none"
                    Cls="btnNoBorder"
                    PressedCls="none"
                    IconCls="ico-drag-vertical ">
                </ext:Button>
                <ext:Label runat="server"
                    ID="lbNombreCategoria"                    
                    Cls="btnCategory"
                    Height="30" />
                <ext:ToolbarFill />
                <ext:Button runat="server"
                    meta:resourceKey="btnNuevoEstado1"
                    IconCls="ico-addBtn"
                    Cls="btn-ppal btnAdd btnAddAttribute"
                    Text="<%$ Resources:Comun, strAtributo %>"
                    MinWidth="140"
                    ID="btnAddAttribute">
                    <Menu>
                        <ext:Menu runat="server" Cls="menuWidthXS" ID="menuNuevoAtributo">
                            <Items>
                                <ext:Button runat="server"
                                    meta:resourceKey="btnNuevoAttribute"
                                    Cls="btnText"
                                    IconCls="ico-plus-green"
                                    Hidden="true"
                                    Text="<%$ Resources:Comun, strAñadirDesdePlantilla %>">
                                    <Menu>
                                        <ext:Menu runat="server" Cls="menuWidthX">
                                            <Items>
                                                <ext:Label runat="server" Text="<%$ Resources:Comun, strPlantillas %>" Width="210" Cls="labelContent" Height="30" />
                                                <ext:MenuItem runat="server" Text="Critic in Site" Cls="borderTop" />
                                                <ext:MenuItem runat="server" Text="Impact in Site" Cls="borderTop" />
                                            </Items>
                                        </ext:Menu>
                                    </Menu>
                                </ext:Button>
                            </Items>
                            <Loader
                                runat="server"
                                DirectMethod="#{DirectMethods}.CargarTiposDatos"
                                AutoLoad="true"
                                Mode="Component">
                                <LoadMask ShowMask="true" />
                            </Loader>
                            <Listeners>
                                <Click Fn="SelectItemNuevoAtributo" />
                            </Listeners>
                        </ext:Menu>
                    </Menu>
                </ext:Button>

            </Items>
        </ext:Toolbar>
        <ext:Container
            runat="server"
            ID="flexContainer"
            Cls="flexContainer"
            Scrollable="Vertical">
            <Items>
            </Items>
        </ext:Container>
        <ext:Toolbar runat="server" Cls="tlbAddAttributeBot" >
            <Items>
                <ext:Button runat="server"
                    ID="btnEliminarCategorias"
                    Cls="bntBasuraGrey"
                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>">
                    <Listeners>
                        <Click Fn="EliminarCategoria" />
                    </Listeners>
                </ext:Button>
                <%-- <ext:Button runat="server"
                    meta:resourceKey="btnNuevoEstado1"
                    IconCls="ico-head-less"
                    Cls="btnCategory"
                    Text="Category Name"
                    AriaLabel="Nuevo Cambio Estado"
                    ToolTip="Nuevo Cambio de Estado"
                    Handler="anadir();">
                    <Menu>
                        <ext:Menu runat="server">
                            <Items>
                                <ext:MenuItem meta:resourceKey="btnNuevoEstado1" ID="MenuItem4" runat="server" Text="Nuevo atributo 1" Icon="GroupAdd" />
                                <ext:MenuItem meta:resourceKey="btnNuevoEstado2" ID="MenuItem5" runat="server" Text="Nuevo atributo 2" Icon="GroupDelete" />
                                <ext:MenuItem meta:resourceKey="btnNuevoEstado3" ID="MenuItem6" runat="server" Text="Nuevo atributo 3" Icon="GroupEdit" />
                            </Items>
                        </ext:Menu>
                    </Menu>
                </ext:Button> --%>
            </Items>
        </ext:Toolbar>
    </Items>
</ext:FieldSet>
