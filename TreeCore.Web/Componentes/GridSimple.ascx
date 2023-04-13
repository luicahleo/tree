<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridSimple.ascx.cs" Inherits="TreeCore.Componentes.GridSimple" %>

<script type="text/javascript" src="../../Componentes/js/GridSimple.js"></script>


<%--COMPONENTE TOOLBARFILTROS Precargados para ser utilizados --%>

<%@ Register Src="~/Componentes/toolbarFiltrosRes1Combo.ascx" TagName="toolbarFiltrosRes1Combo" TagPrefix="local" %>



<ext:Panel runat="server" ID="wrapComponenteCentral" Layout="BorderLayout">
    <Items>

        <ext:GridPanel
            Region="Center"
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

                    <ext:Column runat="server" Text="Name" MinWidth="120" DataIndex="Name" Flex="8" ID="ColNombre" Cls="excluirPnInfo">
                    </ext:Column>
                    <ext:Column runat="server" Text="Object" MinWidth="120" DataIndex="Code" Flex="8" ID="ColObjeto">
                    </ext:Column>


                    <ext:Column runat="server" Text="Type" MinWidth="120" DataIndex="Next" Flex="8" ID="ColSiguiente">
                    </ext:Column>

                    <ext:Column runat="server" MinWidth="120" Text="User" DataIndex="Group" Flex="8" ID="ColGrupo" />
                    <ext:Column runat="server" MinWidth="120" Text="Creation Date" DataIndex="Department" Flex="8" ID="ColFechaCreado" />
                    <ext:Column runat="server" MinWidth="120" Text="Expected Date" DataIndex="GlobalState" Flex="8" ID="ColFechaExpect" />
                    <ext:Column runat="server" MinWidth="120" Text="Upload Date" DataIndex="GlobalState" Flex="8" ID="ColFechaSubida" />


                    
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
                <ext:RowSelectionModel runat="server" />
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

    </Items>
</ext:Panel>
