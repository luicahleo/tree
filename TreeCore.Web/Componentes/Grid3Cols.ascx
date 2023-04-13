<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Grid3Cols.ascx.cs" Inherits="TreeCore.Componentes.Grid3Cols" %>

<script type="text/javascript" src="../../Componentes/js/Grid3Cols.js"></script>


<%--COMPONENTE TOOLBARFILTROS Precargados para ser utilizados --%>

<%@ Register Src="~/Componentes/toolbarFiltrosRes1Combo.ascx" TagName="toolbarFiltrosRes1Combo" TagPrefix="local" %>



<ext:Panel runat="server" ID="wrapComponenteCentral" Layout="HBoxLayout" BodyCls="tbGrey" MaxWidth="1200">
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
                            ID="Button18"
                            Cls="btnEditar"
                            AriaLabel="Editar"
                            Hidden="false"
                            ToolTip="Editar">
                        </ext:Button>


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
                <ext:RowSelectionModel runat="server"
                    ID="GridRowSelectContacto"
                    Mode="Single">
                    <Listeners>
                        <Select Fn="selectRowDetalle" />
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



            <View>
                <ext:GridView runat="server" TrackOver="false" EmptyText="<h1>No matching results</h1>" EnableTextSelection="true">
                </ext:GridView>

            </View>


        </ext:GridPanel>


        <ext:Panel runat="server" ID="pnCol1" Flex="5" Layout="VBoxLayout" BodyCls="tbGrey">
            <LayoutConfig>
                <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
            </LayoutConfig>
            <Items>



                <ext:GridPanel
                    Flex="1"
                    MarginSpec="0 10 5 10"
                    HideHeaders="true"
                    Region="Center"
                    Hidden="false"
                    Title="Phases"
                    runat="server"
                    ID="gridcol2row1"
                    Cls="gridPanel gridSimple  "
                    OverflowX="Hidden"
                    OverflowY="Auto">

                    <DockedItems>
                        <ext:Toolbar runat="server" ID="Toolbar1" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                            <Items>
                                <ext:Button
                                    runat="server"
                                    ID="Button1"
                                    meta:resourceKey="btnAnadir"
                                    Cls="btnAnadir"
                                    AriaLabel="Añadir"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Handler="AgregarEditar();">
                                </ext:Button>

                                <ext:Button runat="server"
                                    ID="btnEditar"
                                    Cls="btnEditar"
                                    AriaLabel="Editar"
                                    Hidden="false"
                                    ToolTip="Editar">
                                </ext:Button>

                                <ext:Button runat="server"
                                    ID="Button2"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    meta:resourceKey="btnEliminar"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
                                <ext:Button runat="server"
                                    ID="Button5"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    meta:resourceKey="btnRefrescar"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />




                            </Items>
                        </ext:Toolbar>






                    </DockedItems>
                    <Store>
                        <ext:Store ID="Store1" runat="server">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="Default" />
                                        <ext:ModelField Name="Average" Type="Float" />
                                        <ext:ModelField Name="Name" />
                                        <ext:ModelField Name="Code" />

                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>


                            <ext:Column runat="server" Text="Name" MinWidth="120" DataIndex="Name" Flex="8" ID="Column1">
                            </ext:Column>



                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" />
                    </SelectionModel>



                    <View>
                        <ext:GridView runat="server" TrackOver="false" EmptyText="<h1>No matching results</h1>" EnableTextSelection="true">
                        </ext:GridView>
                    </View>


                </ext:GridPanel>



                <ext:GridPanel
                    Flex="1"
                    MarginSpec="5 10 0 10"
                    HideHeaders="true"
                    Region="Center"
                    Hidden="false"
                    Title="Zones"
                    runat="server"
                    ID="gridcol2row2"
                    Cls="gridPanel  "
                    OverflowX="Hidden"
                    OverflowY="Auto">

                    <DockedItems>
                        <ext:Toolbar runat="server" ID="Toolbar2" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                            <Items>
                                <ext:Button
                                    runat="server"
                                    ID="Button6"
                                    meta:resourceKey="btnAnadir"
                                    Cls="btnAnadir"
                                    AriaLabel="Añadir"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Handler="AgregarEditar();">
                                </ext:Button>

                                <ext:Button runat="server"
                                    ID="Button7"
                                    Cls="btnEditar"
                                    AriaLabel="Editar"
                                    Hidden="false"
                                    ToolTip="Editar">
                                </ext:Button>

                                <ext:Button runat="server"
                                    ID="Button8"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    meta:resourceKey="btnEliminar"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
                                <ext:Button runat="server"
                                    ID="Button9"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    meta:resourceKey="btnRefrescar"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />

                                <ext:Button runat="server"
                                    ID="btnRegion"
                                    ToolTip="Regiones"
                                    meta:resourceKey="btnRegion"
                                    Cls="btnRegion"
                                    Handler="Regiones();" />




                            </Items>
                        </ext:Toolbar>






                    </DockedItems>
                    <Store>
                        <ext:Store ID="Store2" runat="server">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="Default" />
                                        <ext:ModelField Name="Average" Type="Float" />
                                        <ext:ModelField Name="Name" />
                                        <ext:ModelField Name="Code" />

                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>


                            <ext:Column runat="server" Text="Name" MinWidth="120" DataIndex="Name" Flex="8" ID="Column2">
                            </ext:Column>



                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" />
                    </SelectionModel>



                    <View>
                        <ext:GridView runat="server" TrackOver="false" EmptyText="<h1>No matching results</h1>" EnableTextSelection="true">
                        </ext:GridView>
                    </View>


                </ext:GridPanel>





            </Items>
        </ext:Panel>


        <ext:Panel runat="server" ID="pnCol2" Flex="5" Layout="VBoxLayout" BodyCls="tbGrey">
            <LayoutConfig>
                <ext:VBoxLayoutConfig Align="Stretch"></ext:VBoxLayoutConfig>
            </LayoutConfig>
            <Items>



                <ext:GridPanel
                    Flex="1"
                    MarginSpec="0 10 5 10"
                    HideHeaders="true"
                    Region="Center"
                    Hidden="false"
                    Title="Suppliers"
                    runat="server"
                    ID="gridcol3row1"
                    Cls="gridPanel  "
                    OverflowX="Hidden"
                    OverflowY="Auto">

                    <DockedItems>
                        <ext:Toolbar runat="server" ID="Toolbar3" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                            <Items>
                                <ext:Button
                                    runat="server"
                                    ID="Button10"
                                    meta:resourceKey="btnAnadir"
                                    Cls="btnAnadir"
                                    AriaLabel="Añadir"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Handler="AgregarEditar();">
                                </ext:Button>

                                <ext:Button runat="server"
                                    ID="Button11"
                                    Cls="btnEditar"
                                    AriaLabel="Editar"
                                    Hidden="false"
                                    ToolTip="Editar">
                                </ext:Button>

                                <ext:Button runat="server"
                                    ID="Button12"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    meta:resourceKey="btnEliminar"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
                                <ext:Button runat="server"
                                    ID="Button13"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    meta:resourceKey="btnRefrescar"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />




                            </Items>
                        </ext:Toolbar>






                    </DockedItems>
                    <Store>
                        <ext:Store ID="Store4" runat="server">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="Default" />
                                        <ext:ModelField Name="Average" Type="Float" />
                                        <ext:ModelField Name="Name" />
                                        <ext:ModelField Name="Code" />

                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>


                            <ext:Column runat="server" Text="Name" MinWidth="120" DataIndex="Name" Flex="8" ID="Column3">
                            </ext:Column>



                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" />
                    </SelectionModel>



                    <View>
                        <ext:GridView runat="server" TrackOver="false" EmptyText="<h1>No matching results</h1>" EnableTextSelection="true">
                        </ext:GridView>
                    </View>


                </ext:GridPanel>



                <ext:GridPanel
                    Flex="1"
                    MarginSpec="5 10 0 10"
                    HideHeaders="true"
                    Region="Center"
                    Hidden="false"
                    Title="Zones"
                    runat="server"
                    ID="gridcol3row2"
                    Cls="gridPanel  "
                    OverflowX="Hidden"
                    OverflowY="Auto">

                    <DockedItems>
                        <ext:Toolbar runat="server" ID="Toolbar4" Dock="Top" Cls="tlbGrid" OverflowHandler="Scroller">
                            <Items>
                                <ext:Button
                                    runat="server"
                                    ID="Button14"
                                    meta:resourceKey="btnAnadir"
                                    Cls="btnAnadir"
                                    AriaLabel="Añadir"
                                    ToolTip="<%$ Resources:Comun, btnAnadir.ToolTip %>"
                                    Handler="AgregarEditar();">
                                </ext:Button>

                                <ext:Button runat="server"
                                    ID="Button15"
                                    Cls="btnEditar"
                                    AriaLabel="Editar"
                                    Hidden="false"
                                    ToolTip="Editar">
                                </ext:Button>

                                <ext:Button runat="server"
                                    ID="Button16"
                                    ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                    meta:resourceKey="btnEliminar"
                                    Cls="btnEliminar"
                                    Handler="Eliminar();" />
                                <ext:Button runat="server"
                                    ID="Button17"
                                    ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                    meta:resourceKey="btnRefrescar"
                                    Cls="btnRefrescar"
                                    Handler="Refrescar();" />




                            </Items>
                        </ext:Toolbar>






                    </DockedItems>
                    <Store>
                        <ext:Store ID="Store5" runat="server">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="Default" />
                                        <ext:ModelField Name="Average" Type="Float" />
                                        <ext:ModelField Name="Name" />
                                        <ext:ModelField Name="Code" />

                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel>
                        <Columns>


                            <ext:Column runat="server" Text="Name" MinWidth="120" DataIndex="Name" Flex="8" ID="Column4">
                            </ext:Column>



                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" />
                    </SelectionModel>



                    <View>
                        <ext:GridView runat="server" TrackOver="false" EmptyText="<h1>No matching results</h1>" EnableTextSelection="true">
                        </ext:GridView>
                    </View>


                </ext:GridPanel>





            </Items>
        </ext:Panel>

    </Items>
</ext:Panel>
