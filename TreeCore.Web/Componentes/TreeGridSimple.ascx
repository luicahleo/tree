<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreeGridSimple.ascx.cs" Inherits="TreeCore.Componentes.TreeGridSimple" %>

<script type="text/javascript" src="../../Componentes/js/TreeGridSimple.js"></script>

<%@ Register Src="~/Componentes/toolbarFiltrosRes1Combo.ascx" TagName="toolbarFiltrosRes1Combo" TagPrefix="local" %>



<ext:Panel runat="server" ID="wrapComponenteCentral" Layout="BorderLayout">
    <Items>
        <ext:TreePanel
            Hidden="false"
            Flex="1"
            Header="false"
            runat="server"
            Cls="gridPanel grdNoHeader TreePnl"
            Title="HIEARCHIAl VIEW"
            ID="TreePanelV1"
            Region="Center"
            RootVisible="false"
            Scrollable="Vertical"
            OverflowX="Hidden">
            <Listeners>
                <AfterRender Handler="GridColHandler(this)"></AfterRender>
                <Resize Handler="GridColHandler(this)"></Resize>
            </Listeners>


            <DockedItems>



                <ext:Toolbar
                    OverflowHandler="Scroller"
                    runat="server"
                    ID="Toolbar1"
                    Dock="Top"
                    Cls="tlbGrid c-Grid">

                    <Items>


                        <ext:Button runat="server"
                            ID="btnDescargar"
                            Cls="btnAnadir-subM "
                            AriaLabel="Descargar"
                            ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>">
                            <Menu>
                                <ext:Menu runat="server">
                                    <Items>

                                        <ext:MenuItem
                                            meta:resourceKey="mnuDwldIFRS16"
                                            ID="AddFolderMainPanel"
                                            runat="server"
                                            Text="New Folder"
                                            IconCls="ico-CntxMenuExcel" />
                                        <ext:MenuItem
                                            meta:resourceKey="mnuDwldModeloReg"
                                            ID="AddDocuMainPanel"
                                            runat="server"
                                            Text="Add Document"
                                            IconCls="ico-CntxMenuExcel" />
                                    </Items>
                                </ext:Menu>
                            </Menu>

                        </ext:Button>



                        <ext:Button runat="server" ID="Button7" Cls="btnEditar" AriaLabel="Editar" ToolTip="Editar" Handler="editar();"></ext:Button>
                        <ext:Button runat="server" ID="Button1" Cls="btnEliminar" AriaLabel="Eliminar" ToolTip="Eliminar" Handler="eliminar();"></ext:Button>
                        <ext:Button runat="server" ID="Button5" Cls="btnRefrescar" AriaLabel="Refrescar" ToolTip="Refrescar" Handler="refrescar();"></ext:Button>
                        <ext:Button runat="server" ID="Button6" Cls="btnDescargar" AriaLabel="Descargar" ToolTip="Descargar" Handler="this.up('grid').print();"></ext:Button>
                        <ext:Button runat="server" ID="Button13" Cls="btn-trans btnFiltros" AriaLabel="Abrir panel de filtros" ToolTip="Panel de filtros" Handler="hideAsideR('panelFiltros')"></ext:Button>


                    </Items>

                </ext:Toolbar>



                <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top" Layout="AutoLayout">
                    <Content>

                        <local:toolbarfiltrosres1combo
                            id="cmpFiltro"
                            runat="server" />

                    </Content>
                </ext:Container>


            </DockedItems>

            <Fields>
                <ext:ModelField Name="nombre" />
                <ext:ModelField Name="codigo" />
                <ext:ModelField Name="modulo" />
                <ext:ModelField Name="proyecto" />
                <ext:ModelField Name="fecha" />
            </Fields>
            <ColumnModel> 
                <Columns>
                    <ext:TreeColumn
                        ID="TreeColumn1"
                        runat="server"
                        Text="Elements"
                        Flex="3"
                        Cls="excluirPnInfo"
                        MinWidth="150"
                        Sortable="true"
                        Hidden="false"
                        DataIndex="nombre" />
                    <ext:Column
                        ID="ColCodigo"
                        runat="server"
                        Text="Code"
                        Flex="2"
                        MinWidth="150"
                        Sortable="true"
                        DataIndex="codigo"
                        Hidden="false">
                    </ext:Column>
                    <ext:Column
                        ID="colModulo"
                        runat="server"
                        Text="Module"
                        Flex="2"
                        MinWidth="150"
                        Sortable="true"
                        DataIndex="modulo"
                        Hidden="false">
                    </ext:Column>


                    
                    <%-- En el GridRowSelect para que cambie de informacion el panel lateral añadir:
                        let registroSeleccionado = registro;
                        let GridSeleccionado = App.grid;
                        cargarDatosPanelMoreInfoGrid(registroSeleccionado, GridSeleccionado);
                        
                        
                        --%>
                    <ext:WidgetColumn ID="ColMore" runat="server" Cls="col-More" DataIndex="" Align="Center" Text="More" Hidden="false" MinWidth="90">
                        <Widget>
                            <ext:Button runat="server" Width="90" OverCls="Over-btnMore" PressedCls="Pressed-none" FocusCls="Focus-none" Cls="btnColMore">
                                <Listeners>
                                    <Click Fn="hidePanelMoreInfo" />
                                </Listeners>
                            </ext:Button>
                        </Widget>
                    </ext:WidgetColumn>

                </Columns>

            </ColumnModel>
            <Root>
                <ext:Node Text="Tasks">
                    <Children>
                        <ext:Node Expanded="true">
                            <CustomAttributes>
                                <ext:ConfigItem Name="nombre" Value="DocuPadre (2)" Mode="Value" />
                                <ext:ConfigItem Name="codigo" Value="ducop1" Mode="Value" />

                            </CustomAttributes>
                            <Children>
                                <ext:Node Expandable="false" IconCls="DocWord">
                                    <CustomAttributes>
                                        <ext:ConfigItem Name="nombre" Value="Energu_INC WORD " Mode="Value" />
                                        <ext:ConfigItem Name="codigo" Value="981263DSA" />
                                        <ext:ConfigItem Name="modulo" Value="Energy Facturation" Mode="Value" />
                                        <ext:ConfigItem Name="proyecto" Value="Saving 2020" Mode="Value" />
                                        <ext:ConfigItem Name="fecha" Value="11/11/2020 11:11" Mode="Value" />
                                    </CustomAttributes>

                                </ext:Node>
                                <ext:Node Expandable="false" IconCls="DocPowerPoint">
                                    <CustomAttributes>
                                        <ext:ConfigItem Name="nombre" Value="Energy_INC PP " Mode="Value" />
                                        <ext:ConfigItem Name="codigo" Value="981263DSA" />
                                        <ext:ConfigItem Name="modulo" Value="Energy Facturation" Mode="Value" />
                                        <ext:ConfigItem Name="proyecto" Value="Saving 2020" Mode="Value" />
                                        <ext:ConfigItem Name="fecha" Value="11/11/2020 11:11" Mode="Value" />
                                    </CustomAttributes>

                                </ext:Node>

                                <ext:Node Expandable="false" IconCls="DocExcel">
                                    <CustomAttributes>
                                        <ext:ConfigItem Name="nombre" Value="Energy_INC PP " Mode="Value" />
                                        <ext:ConfigItem Name="codigo" Value="981263DSA" />
                                        <ext:ConfigItem Name="modulo" Value="Energy Facturation" Mode="Value" />
                                        <ext:ConfigItem Name="proyecto" Value="Saving 2020" Mode="Value" />
                                        <ext:ConfigItem Name="fecha" Value="11/11/2020 11:11" Mode="Value" />
                                    </CustomAttributes>

                                </ext:Node>
                            </Children>
                        </ext:Node>
                        <ext:Node Expandable="false">
                            <CustomAttributes>
                                <ext:ConfigItem Name="elements" Value="Tower 0022456" Mode="Value" />
                                <ext:ConfigItem Name="code" Value="Tw0022456" />
                                <ext:ConfigItem Name="model" Value="TW0022456" Mode="Value" />
                                <ext:ConfigItem Name="tipo" Value="Middle H" Mode="Value" />
                                <ext:ConfigItem Name="manufacturer" Value="Baltiomoretic" Mode="Value" />
                                <ext:ConfigItem Name="created" Value="2018/02/28" />
                                <ext:ConfigItem Name="modificated" Value="2019/06/12" />
                                <ext:ConfigItem Name="modificated" Value="2019/06/12" />
                                <ext:ConfigItem Name="more" Value="More" />
                            </CustomAttributes>
                        </ext:Node>
                        <ext:Node Expandable="false">
                            <CustomAttributes>
                                <ext:ConfigItem Name="elements" Value="Tower 003456" Mode="Value" />
                                <ext:ConfigItem Name="code" Value="Tw003456" />
                                <ext:ConfigItem Name="model" Value="TW003456" Mode="Value" />
                                <ext:ConfigItem Name="tipo" Value="Middle H" Mode="Value" />
                                <ext:ConfigItem Name="manufacturer" Value="Baltiomoretic" Mode="Value" />
                                <ext:ConfigItem Name="created" Value="2018/02/28" />
                                <ext:ConfigItem Name="modificated" Value="2019/06/12" />
                                <ext:ConfigItem Name="modificated" Value="2019/06/12" />
                                <ext:ConfigItem Name="more" Value="More" />
                            </CustomAttributes>
                        </ext:Node>
                    </Children>
                </ext:Node>
            </Root>
            <BottomBar>
                <ext:PagingToolbar runat="server" StoreID="StoreGridMain" Cls="PgToolBMainGrid" ID="PagingToolbar2" MaintainFlex="true" Flex="8" HideRefresh="true" DisplayInfo="false" OverflowHandler="Scroller">
                    <Items>
                        <ext:ComboBox runat="server" Cls="comboGrid" ID="ComboBox9" MaxWidth="65">
                            <Items>
                                <ext:ListItem Text="10" />
                                <ext:ListItem Text="20" />
                                <ext:ListItem Text="30" />
                                <ext:ListItem Text="40" />
                            </Items>
                            <SelectedItems>
                                <ext:ListItem Value="20" />
                            </SelectedItems>

                        </ext:ComboBox>

                    </Items>

                </ext:PagingToolbar>

            </BottomBar>
        </ext:TreePanel>

    </Items>
</ext:Panel>
