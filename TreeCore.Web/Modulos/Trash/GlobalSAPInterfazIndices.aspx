<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GlobalSAPInterfazIndices.aspx.cs" Inherits="TreeCore.ModGlobal.GlobalSAPInterfazIndices" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>

            <ext:Hidden ID="hdCliID" runat="server" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server" ID="ResourceManagerTreeCore" DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store runat="server" ID="storePrincipal" RemotePaging="false" AutoLoad="true" OnReadData="storePrincipal_Refresh" RemoteSort="true" PageSize="20">
                <Listeners>
                    <BeforeLoad Fn="DeseleccionarGrilla" />
                </Listeners>
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server" IDProperty="SAPInterfazIndiceID">
                        <Fields>

                            <ext:ModelField Name="SAPInterfazIndiceID" Type="Int" />
                            <ext:ModelField Name="NombreIndice" />
                            <ext:ModelField Name="Sociedad" />
                            <ext:ModelField Name="SociedadID" Type="Int" />
                            <ext:ModelField Name="Mes" Type="Int" />
                            <ext:ModelField Name="AnyoBase" Type="Int" />
                            <ext:ModelField Name="Pais" />
                            <ext:ModelField Name="Generado" Type="Boolean" />
                            <ext:ModelField Name="Valor" Type="Float" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="NombreIndice" Direction="ASC" />
                </Sorters>
            </ext:Store>


            <%--FIN  STORES --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="vwContenedor"
                Cls="vwContenedor"
                Layout="Anchor">
                <Items>
                    <ext:GridPanel
                        runat="server"
                        ID="grid"
                        meta:resourceKey="grid"
                        SelectionMemory="false"
                        Cls="gridPanel"
                        StoreID="storePrincipal"
                        Title="etiqgridTitle"
                        Header="false"
                        EnableColumnHide="false"
                        AnchorHorizontal="-100"
                        AnchorVertical="100%"
                        AriaRole="main"><DockedItems>
                            <ext:Toolbar runat="server"
                                ID="tlbBase"
                                Dock="Top"
                                Cls="tlbGrid">
                                <Items>
                                    <ext:Button runat="server"
                                        ID="btnRefrescar"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        Cls="btnRefrescar"
                                        Handler="Refrescar();" />
                                    <ext:Button runat="server"
                                        ID="btnDescargar"
                                        ToolTip="<%$ Resources:Comun, btnDescargar.ToolTip %>"
                                        Cls="btnDescargar"
                                        Handler="ExportarDatos('GlobalSAPInterfazIndices', hdCliID.value, #{grid}, '');" />
                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column DataIndex="Generado" 
                                    Text="Generado"
                                    Width="80"
                                    Align="Center"
                                    meta:resourceKey="colGenerado"
                                    ID="colGenerado"
                                    runat="server">
                                    <Renderer Fn="DefectoRender" />
                                </ext:Column>
                                <ext:Column
                                    DataIndex="NombreIndice"
                                    Text="<%$ Resources:Comun, strNombre %>"
                                    Width="150"
                                    meta:resourceKey="colNombreIndice" 
                                    ID="colNombreIndice" 
                                    runat="server" 
                                    Flex="1" />
                                <ext:Column 
                                    DataIndex="Sociedad" 
                                    Text="<%$ Resources:Comun, strSociedad %>" 
                                    Width="150" 
                                    meta:resourceKey="colSociedad" 
                                    ID="colSociedad" 
                                    runat="server" />
                                <ext:Column 
                                    DataIndex="Mes"
                                    Text="<%$ Resources:Comun, strMes %>"
                                    Width="100"
                                    meta:resourceKey="colMes" 
                                    ID="colMes"
                                    runat="server" />
                                <ext:Column 
                                    DataIndex="AnyoBase"
                                    Text="<%$ Resources:Comun, strAnyoBase %>"
                                    Width="100"
                                    meta:resourceKey="colAnyoBase"
                                    ID="colAnyoBase" 
                                    runat="server" />
                                <ext:Column 
                                    DataIndex="Pais"
                                    Text="<%$ Resources:Comun, strPais %>"
                                    Width="150"
                                    meta:resourceKey="colPais" 
                                    ID="colPais"
                                    runat="server" />
                                <ext:Column 
                                    DataIndex="Valor" 
                                    Text="<%$ Resources:Comun, strValor %>"
                                    Width="150"
                                    meta:resourceKey="colValor"
                                    ID="colValor" 
                                    runat="server" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server"
                                ID="GridRowSelect"
                                Mode="Single">
                                <Listeners>
                                    <Select Fn="Grid_RowSelect" />
                                </Listeners>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Plugins>
                            <ext:GridFilters runat="server"
                                ID="gridFilters"
                                MenuFilterText="<%$ Resources:Comun, strFiltros %>"
                                meta:resourceKey="GridFilters" />                            
                                <ext:CellEditing runat="server"
                                ClicksToEdit="2" />
                        </Plugins>
                        <BottomBar>
                            <ext:PagingToolbar runat="server"
                                ID="PagingToolBar"
                                meta:resourceKey="PagingToolBar"
                                StoreID="storePrincipal"
                                DisplayInfo="true"
                                HideRefresh="true">
                                <Items>
                                    <ext:ComboBox runat="server"
                                        Cls="comboGrid"
                                        Width="80">
                                        <Items>
                                            <ext:ListItem Text="10" />
                                            <ext:ListItem Text="20" />
                                            <ext:ListItem Text="30" />
                                            <ext:ListItem Text="40" />
                                            <ext:ListItem Text="50" />
                                        </Items>
                                        <SelectedItems>
                                            <ext:ListItem Value="20" />
                                        </SelectedItems>
                                        <Listeners>
                                            <Select Fn="handlePageSizeSelect" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
