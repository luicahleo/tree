<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridDobleVisor.ascx.cs" Inherits="TreeCore.Componentes.GridDobleVisor" %>

<%--<%@ Register Src="/Componentes/FormEmplazamientos.ascx" TagName="FormEmplazamientos" TagPrefix="local" %>--%>
<script type="text/javascript" src="../../Componentes/js/GridDobleVisor.js"></script>

<%--COMPONENTE TOOLBARFILTROS Precargados para ser utilizados --%>

<%@ Register Src="~/Componentes/toolbarFiltrosRes1Combo.ascx" TagName="toolbarFiltrosRes1Combo" TagPrefix="local" %>


<ext:Panel runat="server" ID="wrapComponenteCentral" Layout="BorderLayout" BodyCls="tbGrey">
    <Listeners>
        <AfterLayout Handler="ControlPaneles(this)"></AfterLayout>
        <Resize Handler="ControlPaneles(this)"></Resize>
    </Listeners>
    <Items>


        <ext:TreePanel
            ForceFit="true"
            Hidden="false"
            MaxWidth="300"
            Flex="12"
            Header="false"
            runat="server"
            Cls="gridPanel grdNoHeader TreePnl TreePL"
            Title="HIEARCHIAl VIEW"
            ID="TreePanelV1"
            Region="West"
            RootVisible="false"
            OverflowX="Auto">

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


                <ext:Container runat="server" ID="Container1" Cls="" Dock="Top" Layout="AutoLayout">
                    <Content>

                        <local:toolbarFiltrosRes1Combo
                            ID="ToolbarFiltrosRes1Combo1"
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
                        Sortable="true"
                        Hidden="false"
                        DataIndex="nombre" />
                    <ext:Column
                        ID="ColCodigo"
                        runat="server"
                        Text="Code"
                        Flex="2"
                        Sortable="true"
                        DataIndex="codigo"
                        Hidden="false">
                    </ext:Column>

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
                <ext:PagingToolbar runat="server" StoreID="StoreGridMain" Cls="PgToolBMainGrid" ID="PagingToolbar1" MaintainFlex="true" Flex="8">
                    <Items>
                        <ext:ComboBox runat="server" Cls="comboGrid" ID="ComboBox1" Flex="2">
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

            <SelectionModel>

                <ext:RowSelectionModel runat="server">
                    <Listeners>
                        <Select Handler="SelectTreepn(this)"></Select>

                    </Listeners>
                </ext:RowSelectionModel>
            </SelectionModel>

        </ext:TreePanel>



        <ext:GridPanel
            Region="Center"
            Flex="1"
            Hidden="false"
            Title="Grid Principal"
            runat="server"
            Header="false"
            ID="gridMain1"
            Cls="gridPanel grdNoHeader "
            OverflowX="Hidden"
            OverflowY="Auto">
            <Listeners>
                <AfterRender Handler="GridColHandler(this)"></AfterRender>
                <Resize Handler="GridColHandler(this)"></Resize>
            </Listeners>

            <DockedItems>

                <ext:Toolbar
                    runat="server"
                    ID="Toolbar5"
                    Dock="Top"
                    Padding="0"
                    Cls="tlbGrid c-Grid">

                    <Items>

                        <ext:Button runat="server" ID="btnCloseShowVisorTreeP" IconCls="ico-hide-menu" Cls="btnSbCategory" Handler="VisorSwitch(this)"></ext:Button>
                        <ext:Label runat="server" Cls="HeaderLblVisor" Text="ENERGY_INVOICE_2020"></ext:Label>



                        <ext:ToolbarFill />



                    </Items>

                </ext:Toolbar>

                <ext:Toolbar runat="server" ID="tlbBase" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                    <Items>
                        <ext:Button
                            runat="server"
                            ID="btnAnadir"
                            meta:resourceKey="btnAnadir"
                            Cls="btnAnadir"
                            AriaLabel="Añadir"
                            ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                            Handler="AgregarEditar();">
                        </ext:Button>

                        <ext:Button runat="server"
                            ID="btnEliminar"
                            ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                            meta:resourceKey="btnEliminar"
                            Cls="btnEliminar"
                            Handler="Eliminar();" />
                        <ext:Button runat="server"
                            ID="btnRefrescar"
                            ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                            meta:resourceKey="btnRefrescar"
                            Cls="btnRefrescar"
                            Handler="Refrescar();" />

                        <ext:Button runat="server"
                            ID="Button3"
                            ToolTip="Descargar"
                            meta:resourceKey="btnRefrescar"
                            Cls="btnDescargar"
                            Handler="Refrescar();" />


                        <ext:Button runat="server"
                            ID="Button4"
                            ToolTip="Mostrar Filtros"
                            meta:resourceKey="btnRefrescar"
                            Cls="btnFiltros"
                            Handler="hideAsideR('panelFiltros')" />



                    </Items>
                </ext:Toolbar>




                <ext:Container runat="server" ID="tbfiltros" Cls="" Dock="Top" Layout="AutoLayout">
                    <Content>

                        <local:toolbarFiltrosRes1Combo
                            ID="cmpFiltro"
                            runat="server" />

                    </Content>
                </ext:Container>


            </DockedItems>
            <Store>
                <ext:Store ID="Store3" runat="server">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="Default" />
                                <ext:ModelField Name="Average" Type="Float" />
                                <ext:ModelField Name="Name" />
                                <ext:ModelField Name="Code" />
                                <ext:ModelField Name="Next" />
                                <ext:ModelField Name="Amarillo" Type="Float" />
                                <ext:ModelField Name="Rojo" Type="Float" />
                                <ext:ModelField Name="Group" />
                                <ext:ModelField Name="Department" />
                                <ext:ModelField Name="GlobalState" />
                                <ext:ModelField Name="Parallel" />
                                <ext:ModelField Name="Link" />
                                <ext:ModelField Name="Notifications" />
                                <ext:ModelField Name="Documents" />
                                <ext:ModelField Name="Functionalities" />

                            </Fields>
                        </ext:Model>
                    </Model>
                </ext:Store>
            </Store>
            <ColumnModel>
                <Columns>

                    <ext:ProgressBarColumn runat="server" DataIndex="Average" Text="" MinWidth="60" Align="Center" ID="BarraProgreso" Flex="6" Cls="excluirPnInfo">
                        <Renderer Fn="barGrid"></Renderer>
                    </ext:ProgressBarColumn>

                    <ext:Column runat="server" Text="Name" MinWidth="120" DataIndex="Name" Flex="8" ID="ColNombre">
                    </ext:Column>
                    <ext:Column runat="server" Text="Object" MinWidth="120" DataIndex="Code" Flex="8" ID="ColObjeto">
                    </ext:Column>


                    <ext:Column runat="server" Text="Type" MinWidth="120" DataIndex="Next" Flex="8" ID="ColSiguiente">
                    </ext:Column>

                    <ext:Column runat="server" MinWidth="120" Text="User" DataIndex="Group" Flex="8" ID="ColGrupo" />
                    <ext:Column runat="server" MinWidth="120" Text="Creation Date" DataIndex="Department" Flex="8" ID="ColFechaCreado" />
                    <ext:Column runat="server" MinWidth="120" Text="Expected Date" DataIndex="GlobalState" Flex="8" ID="ColFechaExpect" />
                    <ext:Column runat="server" MinWidth="120" Text="Upload Date" DataIndex="GlobalState" Flex="8" ID="ColFechaSubida" />


                    <ext:Column runat="server" Cls="" DataIndex="Default" MinWidth="80" Align="Center" ID="ColDefecto" Flex="8">
                        <Renderer Fn="DefectoRender" />
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
            <SelectionModel>

                <ext:RowSelectionModel runat="server">
                    <Listeners>
                        <Select Handler=""></Select>

                    </Listeners>
                </ext:RowSelectionModel>
            </SelectionModel>

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
        </ext:GridPanel>

    </Items>
</ext:Panel>
