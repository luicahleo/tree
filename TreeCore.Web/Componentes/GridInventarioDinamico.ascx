<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridInventarioDinamico.ascx.cs" Inherits="TreeCore.Componentes.GridInventarioDinamico" %>

<ext:GridPanel
    runat="server"
    ID="gridCategoriaInventario"
    Cls="gridPanel grdNoHeader"
    EnableLocking="True"
    IsDynamic="true">
    <ViewConfig />
    <DockedItems>
        <ext:Toolbar runat="server"
            ID="tlbFiltros"
            Cls="tlbGrid"
            Layout="ColumnLayout">
            <Items>
                <ext:TextField
                    ID="txtSearch"
                    Cls="txtSearchC"
                    runat="server"
                    EmptyText="Search"
                    LabelWidth="50"
                    Width="250">
                    <Triggers>
                        <ext:FieldTrigger Icon="Search" />
                    </Triggers>
                </ext:TextField>
                <ext:FieldContainer runat="server"
                    ID="FCCombos"
                    Cls="FloatR FCCombos"
                    Layout="HBoxLayout">
                    <Defaults>
                        <ext:Parameter Name="margin"
                            Value="0 5 0 0"
                            Mode="Value" />
                    </Defaults>
                    <LayoutConfig>
                        <ext:HBoxLayoutConfig Align="Middle" />
                    </LayoutConfig>
                    <Items>
                        <ext:ComboBox runat="server"
                            ID="cmbProyectos"
                            Cls="comboGrid  "
                            EmptyText="Projects"
                            Flex="1">
                            <Items>
                                <ext:ListItem Text="Proyecto 1" />
                                <ext:ListItem Text="Proyecto 2" />
                            </Items>
                        </ext:ComboBox>

                        <ext:ComboBox runat="server"
                            ID="cmbEmplazamientos"
                            Cls="comboGrid  "
                            EmptyText="Tipologías"
                            Flex="1">
                            <Items>
                                <ext:ListItem Text="Tipología 1" />
                                <ext:ListItem Text="Tipología 2" />
                                <ext:ListItem Text="Tipología 3" />
                                <ext:ListItem Text="Tipología 4" />
                            </Items>
                        </ext:ComboBox>
                    </Items>
                </ext:FieldContainer>
                <ext:FieldContainer runat="server"
                    ID="ContButtons"
                    Cls="FloatR ContButtons">
                    <Items>
                        <ext:Button runat="server"
                            Width="30"
                            ID="Button1"
                            Cls="btn-trans btnColumnas"
                            AriaLabel="Duplicar Tipología"
                            ToolTip="Duplicar Tipología">
                        </ext:Button>
                        <ext:Button runat="server"
                            Width="30"
                            ID="btnClearFilters"
                            Cls="btn-trans btnRemoveFilters"
                            AriaLabel="Quitar Filtros"
                            ToolTip="Quitar Filtros">
                        </ext:Button>
                        <ext:Button runat="server"
                            Width="30"
                            ID="Button3"
                            Cls="btn-trans btnFiltroNegativo"
                            AriaLabel="Ver Workflow"
                            ToolTip="Ver Workflow"
                            Handler="ShowWorkFlow();">
                        </ext:Button>
                    </Items>
                </ext:FieldContainer>
            </Items>
        </ext:Toolbar>
        <ext:PagingToolbar
            runat="server"
            ID="PagingToolbar"
            Dock="Bottom"
            HideRefresh="true">
            <Items>
                <ext:ComboBox runat="server"
                    Cls="comboGrid"
                    MaxWidth="80"
                    ID="cmbNumRegistros"
                    Flex="2">
                    <Items>
                        <ext:ListItem Text="10" />
                        <ext:ListItem Text="20" />
                        <ext:ListItem Text="30" />
                        <ext:ListItem Text="40" />
                    </Items>
                    <SelectedItems>
                        <ext:ListItem Value="20" />
                    </SelectedItems>
                    <Listeners>
                    </Listeners>
                </ext:ComboBox>
            </Items>
        </ext:PagingToolbar>
    </DockedItems>
    <SelectionModel>
        <ext:CheckboxSelectionModel runat="server"
            ID="GridRowSelect"
            Mode="Multi">
            <Listeners>
            </Listeners>
        </ext:CheckboxSelectionModel>
    </SelectionModel>
    <Plugins>
        <ext:GridFilters runat="server"
            ID="gridFilters"
            MenuFilterText="<%$ Resources:Comun, strFiltros %>">
        </ext:GridFilters>
    </Plugins>
</ext:GridPanel>
