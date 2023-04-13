<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Emplazamientos.aspx.cs" Inherits="TreeCore.ModGlobal.pages.Emplazamientos" %>

<%@ Register Src="/Componentes/GridEmplazamientos.ascx" TagName="GridEmplazamientos" TagPrefix="local" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <%--<link href="../../CSS/tCore.min.css" rel="stylesheet" type="text/css" />--%>
    <link href="css/styleEmplazamientos.min.css" rel="stylesheet" type="text/css" />
    <script src="//maps.googleapis.com/maps/api/js?key=AIzaSyA0k02bRAAmDy-4x0kcN06k1auhLHKGRZs&v=3&libraries=places" type="text/javascript"></script>
    <script src="https://unpkg.com/@google/markerclustererplus@4.0.1/dist/markerclustererplus.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Componentes/js/FormEmplazamientos.js"></script>
    <script type="text/javascript" src="/Componentes/js/FormContactos.js"></script>
    <script type="text/javascript" src="/Componentes/js/toolbarFiltros.js"></script>
    <script type="text/javascript" src="/PaginasComunes/js/Ext.ux.Map.js"></script>
    <script type="text/javascript" src="/PaginasComunes/js/Ext.ux.GMapPanel.js"></script>
    <script type="text/javascript" src="/Componentes/js/Localizaciones.js"></script>
    <script type="text/javascript" src="/Componentes/js/Geoposicion.js"></script>
    <%--<script src="../../JS/common.js"></script>--%>
    <script type="text/javascript" src="/Componentes/js/FormGestionElementos.js"></script>
    <script type="text/javascript" src="/Componentes/js/GestionCategoriasAtributos.js"></script>
    <script type="text/javascript" src="/Componentes/js/GestionAtributos.js"></script>
    <link href="/ModInventario/pages/css/styleInventarioCategorias.css" rel="stylesheet" type="text/css" />

    <form id="form1" runat="server">
        <div>
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="CurrentControl" runat="server" />
            <ext:Hidden ID="hdTabName" runat="server" />
            <ext:Hidden ID="hdPageName" runat="server" />
            <ext:Hidden ID="hdStringBuscador" runat="server" >
                <Listeners>
                    <Change Fn="filtrarEmplazamientosPorBuscador" />
                </Listeners>
            </ext:Hidden>
            <ext:Hidden ID="hdIDEmplazamientoBuscador" runat="server" >
                <Listeners>
                    <Change Fn="filtrarEmplazamientosPorBuscador" />
                </Listeners>
            </ext:Hidden>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <WindowResize Handler="GridResizer()" />

                </Listeners>
            </ext:ResourceManager>

            <ext:Window ID="WinIncludeNewProjForm"
                runat="server"
                Title="Include in the project"
                Height="500"
                Width="872"
                Modal="true"
                Centered="false"
                BodyCls="winFormIncludeProj"
                Cls="winForm-resp"
                Scrollable="Vertical"
                Hidden="true">

                <Listeners>
                    <Resize Handler="winFormResizeWinIncludeNewProjForm(this);"></Resize>
                    <Render Handler="winFormResizeWinIncludeNewProjForm(this);"></Render>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar3" Cls="tbGrey tbExtraBottomSpace" Dock="Bottom">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button
                                runat="server"
                                ID="Button15"
                                Cls="btn-ppal "
                                Text="+ Add Site to this project"
                                Focusable="false"
                                PressedCls="none"
                                Hidden="false">
                            </ext:Button>

                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>


                    <%--PANEL FORM ADD PROYECTO INCLUIDO --%>

                    <ext:FormPanel ID="FormAddProyectoIncluido" Cls="formGris FormGrid FormAddProyectoIncluido" runat="server" Hidden="false">
                        <Content>

                            <ext:ComboBox runat="server"
                                meta:resourcekey="cmbTipologia"
                                ID="cmbTipologia"
                                FieldLabel="Typology"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true">

                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" QTip="<b>Title</b><br/>Custom title" />
                                </Triggers>


                            </ext:ComboBox>



                            <ext:ComboBox runat="server"
                                meta:resourcekey="cmbFases"
                                ID="cmbFases"
                                FieldLabel="Phases"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" QTip="<b>Title</b><br/>Custom title" />
                                </Triggers>


                            </ext:ComboBox>

                            <ext:ComboBox runat="server"
                                meta:resourcekey="cmbProveedores"
                                ID="cmbProveedores"
                                FieldLabel="Suppliers"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" QTip="<b>Title</b><br/>Custom title" />
                                </Triggers>


                            </ext:ComboBox>

                            <ext:ComboBox runat="server"
                                meta:resourcekey="cmbZonas"
                                ID="cmbZonas"
                                FieldLabel="Zones"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" QTip="<b>Title</b><br/>Custom title" />
                                </Triggers>


                            </ext:ComboBox>








                            <ext:TextArea runat="server" ID="txtareaComments" EmptyText="Enter your comment" Cls="" FieldLabel="Comment" LabelAlign="Top"></ext:TextArea>




                        </Content>



                    </ext:FormPanel>


                </Items>

            </ext:Window>

            <ext:Window ID="windAddIncProjects"
                runat="server"
                Height="300"
                Width="400"
                Modal="true"
                Centered="true"
                BodyCls=""
                Closable="false"
                Header="false"
                Cls=""
                Scrollable="Vertical"
                Hidden="true"
                Layout="FitLayout">


                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar2" Dock="Top">
                        <Items>


                            <ext:Button runat="server"
                                meta:resourcekey="btnAddIncludedProjects"
                                ID="btnAddIncludedProjects"
                                Cls=" btnAddIncProjects "
                                PressedCls="none"
                                Focusable="false"
                                TextAlign="Right"
                                Text="+ Add to new Process" />


                        </Items>
                    </ext:Toolbar>
                </DockedItems>


                <Listeners>
                    <%--<Resize Handler="winFormResizeIncProjects(this);"></Resize>--%>
                </Listeners>
                <Items>
                    <ext:GridPanel runat="server"
                        ID="gridIncludedProjects"
                        BodyCls=""
                        Cls="gridPanel gridWHeader ctForm-resp">


                        <Store>
                            <ext:Store
                                ID="Store4"
                                runat="server"
                                Buffered="true"
                                RemoteFilter="true"
                                LeadingBufferZone="1000"
                                PageSize="50">
                                <Model>
                                    <ext:Model runat="server" IDProperty="Nombre">
                                        <Fields>
                                            <ext:ModelField Name="Codigo" />
                                            <ext:ModelField Name="Proceso" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column runat="server" ID="Column3" Text="Code" DataIndex="Codigo" Flex="4"></ext:Column>
                                <ext:Column runat="server" ID="Column4" Text="Process" DataIndex="Proceso" Flex="3"></ext:Column>

                                <ext:WidgetColumn meta:resourcekey="ColDirecciones" ID="WidgetColumn4" runat="server" Cls="" Filterable="false" Align="Center" Hidden="false" MinWidth="65" Flex="1">
                                    <Widget>
                                        <ext:Button meta:resourcekey="btnColMore" runat="server" ID="Button14" IconCls="ico-editar" Scale="Medium" Width="40" OverCls="none" PressedCls="Pressed-none" FocusCls="Focus-none" Cls=" transBckBtn" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                    </Widget>
                                </ext:WidgetColumn>



                            </Columns>
                        </ColumnModel>
                        <View>
                            <ext:GridView runat="server" EnableTextSelection="true">
                            </ext:GridView>
                        </View>
                        <Items>
                        </Items>

                    </ext:GridPanel>




                </Items>

            </ext:Window>

            <ext:Window ID="winIncludeProjects"
                runat="server"
                Title="Include in the Project"
                Height="500"
                Width="872"
                Modal="true"
                Centered="true"
                BodyCls=""
                Cls="winForm-resp"
                Scrollable="Vertical"
                Hidden="true"
                Layout="FitLayout">

                <Listeners>
                    <Resize Handler="winFormResizeIncProjects(this);"></Resize>
                </Listeners>
                <Items>
                    <ext:GridPanel runat="server"
                        ID="GridIncluirenProyectos"
                        Title="PROJECTS"
                        Margin="30"
                        BodyCls="ImgAdjustCenter"
                        Cls="gridPanel gridWHeader ctForm-resp">

                        <DockedItems>
                            <ext:Toolbar runat="server" ID="tbMainIncludeP" Layout="ColumnLayout">
                                <Items>

                                    <ext:TextField
                                        meta:resourceKey="txtSearchTB"
                                        ID="txtSearchTB"
                                        Cls="txtSearchTBCls"
                                        runat="server"
                                        EmptyText="Search"
                                        LabelWidth="50"
                                        MinWidth="260">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Search" />
                                        </Triggers>

                                    </ext:TextField>


                                    <ext:ComboBox meta:resourceKey="cmbProyectosFiltros" runat="server" ID="cmbProyectosFiltros" Cls="comboGrid cmbProyectosF" EmptyText="Type of Projects" Hidden="false">
                                        <Items>
                                            <ext:ListItem Text="Proyecto 1" />
                                            <ext:ListItem Text="Proyecto 2" />
                                        </Items>
                                    </ext:ComboBox>


                                    <ext:Button runat="server"
                                        ID="btnActivos"
                                        EnableToggle="true"
                                        Cls="PosUnsFloatR btn-toggleGrid-withLabel "
                                        TextAlign="Left"
                                        MinWidth="135"
                                        Text="Only Included" />


                                </Items>
                            </ext:Toolbar>
                        </DockedItems>
                        <Store>
                            <ext:Store
                                ID="Store3"
                                runat="server"
                                Buffered="true"
                                RemoteFilter="true"
                                LeadingBufferZone="1000"
                                PageSize="50">
                                <Model>
                                    <ext:Model runat="server" IDProperty="Nombre">
                                        <Fields>
                                            <ext:ModelField Name="Nombre" />
                                            <ext:ModelField Name="Tipo" />
                                            <ext:ModelField Name="SitioIncluido" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column runat="server" ID="colNombre" Text="Name" DataIndex="Nombre" Flex="4"></ext:Column>
                                <ext:Column runat="server" ID="colTipo" Text="Type" DataIndex="Tipo" Flex="3"></ext:Column>
                                <ext:Column runat="server" ID="colIncluido" Text="Site Included" DataIndex="SitioIncluido" Align="Center" Flex="2">
                                    <Renderer Fn="DefectoRender" />

                                </ext:Column>
                                <ext:WidgetColumn meta:resourcekey="ColDirecciones" ID="WidgetColumn3" runat="server" Cls="" DataIndex="" Text="Add" Align="Center" Hidden="false" MinWidth="65" Flex="1">
                                    <Widget>
                                        <ext:Button meta:resourcekey="btnColMore" runat="server" ID="Button11" IconCls="btn-add-plus-gr" Scale="Medium" Width="40" OverCls="none" PressedCls="Pressed-none" FocusCls="Focus-none" Cls=" transBckBtn" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                    </Widget>
                                </ext:WidgetColumn>



                            </Columns>
                        </ColumnModel>
                        <View>
                            <ext:GridView runat="server" EnableTextSelection="true">
                            </ext:GridView>
                        </View>
                        <Items>
                        </Items>

                    </ext:GridPanel>




                </Items>

            </ext:Window>

            <%-- 
            <ext:Window ID="WinAddContactos"
                runat="server"
                Title="New Contact"
                Height="500"
                Width="872"
                Modal="true"
                Centered="true"
                BodyCls="winAddDirBody"
                Cls="winForm-resp"
                Scrollable="Vertical"
                Hidden="true">

                <Listeners>
                    <Resize Handler="winFormResizeContacts(this);"></Resize>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server" ID="Toolbar1" Cls="tbGrey" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server" ID="backContacts" Cls="btn-secondary " MinWidth="80" Text="Back" Focusable="false" PressedCls="none" Hidden="true"></ext:Button>
                            <ext:Button runat="server" ID="nextContacts" Cls="btn-ppal " Text="Next" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>
                            <ext:Button runat="server" ID="saveContacts" Cls="btn-ppal " Text="Save Contact" Focusable="false" PressedCls="none" Hidden="true"></ext:Button>

                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>

                    <ext:Panel runat="server" ID="pnNavVistasNewContactos" Cls="pnNavVistas pnVistasForm" AriaRole="navigation">
                        <Items>
                            <ext:Container runat="server" ID="conNavVistasNewContactos" Cls="nav-vistas">
                                <Items>
                                    <ext:HyperlinkButton runat="server" ID="lnkContactos" Cls="lnk-navView  lnk-noLine navActivo" Text="CONTACT" Handler="showWinFormContacts(this);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkDirContactos" Cls="lnk-navView  lnk-noLine" Text="ADDRESS" Handler="showWinFormContacts(this);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkNotasContactos" Cls="lnk-navView  lnk-noLine" Text="NOTES" Handler="showWinFormContacts(this);"></ext:HyperlinkButton>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>


                    

                    <ext:FormPanel ID="FormContacto" Cls="formGris FormGrid FormContacto" runat="server" Hidden="false">
                        <Content>

                            <ext:TextField runat="server"
                                meta:resourcekey="NombreCompany"
                                ID="NombreCompany"
                                FieldLabel="Name / Company"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />

                            <ext:TextField runat="server"
                                meta:resourcekey="Apellidos"
                                ID="Apellidos"
                                FieldLabel="Last Name"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />

                            <ext:TextField runat="server"
                                meta:resourcekey="Telefono1"
                                ID="Telefono1"
                                FieldLabel="Telephone 1"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />

                            <ext:TextField runat="server"
                                meta:resourcekey="Telefono2"
                                ID="Telefono2"
                                FieldLabel="Telephone 2"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />

                            <ext:TextField runat="server"
                                meta:resourcekey="Telefono3"
                                ID="Telefono3"
                                FieldLabel="Telephone 3"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />

                            <ext:TextField runat="server"
                                meta:resourcekey="txtEmail"
                                ID="txtEmail"
                                FieldLabel="Email"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />

                            <ext:ComboBox runat="server"
                                meta:resourcekey="TipoContrato"
                                ID="TipoContrato"
                                FieldLabel="Type Of Contract"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />




                        </Content>
                    </ext:FormPanel>

                    
                    <ext:FormPanel ID="FormDirContacto" Cls="formGris FormGrid" runat="server" Hidden="true">
                        <Content>

                            <ext:TextField runat="server"
                                ID="TextField1"
                                FieldLabel="Address Name"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField runat="server"
                                ID="TextField2"
                                FieldLabel="Address"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />

                            <ext:Container runat="server" ID="Container2" Cls="grupoCortoCls unsetW" StyleSpec="width:unset;" Layout="HBoxLayout">
                                <Items>
                                    <ext:NumberField runat="server"
                                        FieldLabel="No."
                                        LabelAlign="Top"
                                        ID="NumberField1"
                                        MinWidth="20"
                                        Cls="PaddLR5 unsetPos" />
                                    <ext:NumberField runat="server"
                                        FieldLabel="Floor"
                                        LabelAlign="Top"
                                        ID="NumberField2"
                                        MinWidth="20"
                                        Cls="PaddLR5 unsetPos" />
                                    <ext:NumberField runat="server"
                                        FieldLabel="Door"
                                        LabelAlign="Top"
                                        ID="NumberField3"
                                        MinWidth="20"
                                        Cls="PaddLR5 unsetPos" />
                                    <ext:NumberField runat="server"
                                        FieldLabel="Postal Code"
                                        LabelAlign="Top"
                                        ID="NumberField4"
                                        MinWidth="20"
                                        Cls="PaddLR5 unsetPos" />

                                </Items>
                            </ext:Container>





                            <ext:TextField runat="server"
                                ID="TextField3"
                                FieldLabel="Town"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField runat="server"
                                ID="TextField4"
                                FieldLabel="Province"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField runat="server"
                                ID="TextField5"
                                FieldLabel="Country"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />


                        </Content>
                    </ext:FormPanel>


                    

                    <ext:FormPanel ID="FormNotasContacto" Cls="formGris FormGrid FormComentarios" runat="server" Hidden="true" Layout="AnchorLayout">
                        <Content>

                            <ext:TextArea runat="server"
                                AnchorHorizontal="100%"
                                Height="200"
                                ID="TextArea1"
                                FieldLabel="Comments"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />


                        </Content>
                    </ext:FormPanel>





                </Items>

            </ext:Window>
            --%>

            <%--
            <ext:Window ID="winAddDirecciones"
                runat="server"
                Title="New Address"
                Width="872"
                Height="500"
                Modal="true"
                Centered="true"
                BodyCls="winAddDirBody"
                Cls="winForm-resp"
                Scrollable="Vertical"
                Hidden="true">

                <Listeners>
                    <Resize Handler="winFormResizeDirs(this);"></Resize>
                </Listeners>
                <DockedItems>
                    <ext:Toolbar runat="server" ID="tbMainFormAddDirecciones" Cls="tbGrey" Dock="Bottom" Padding="20">
                        <Items>
                            <ext:Button runat="server" ID="btnBorrar" Cls="btnNoborder redTxt" CtCls="testred" IconCls="ico-trash-red" Scale="Medium" Text="Delete" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>


                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button runat="server" ID="BtnAtras" Cls="btn-secondary " MinWidth="80" Text="Back" Focusable="false" PressedCls="none" Hidden="true"></ext:Button>
                            <ext:Button runat="server" ID="BtnSiguiente" Cls="btn-ppal " Text="Next" Focusable="false" PressedCls="none" Hidden="false"></ext:Button>
                            <ext:Button runat="server" ID="BtnGuardarDir" Cls="btn-ppal " Text="Save Address" Focusable="false" PressedCls="none" Hidden="true"></ext:Button>

                        </Items>
                    </ext:Toolbar>
                </DockedItems>
                <Items>

                    <ext:Panel runat="server" ID="pnVistasForm" Cls="pnNavVistas pnVistasForm" AriaRole="navigation">
                        <Items>
                            <ext:Container runat="server" ID="cntNavVistasForm" Cls="nav-vistas">
                                <Items>
                                    <ext:HyperlinkButton runat="server" ID="lnkAddDirecciones" Cls="lnk-navView  lnk-noLine navActivo" Text="ADDRESS" Handler="showWinForms(this);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkAccesos" Cls="lnk-navView  lnk-noLine" Text="ACCESS" Handler="showWinForms(this);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkComentarios" Cls="lnk-navView  lnk-noLine" Text="COMMENTS" Handler="showWinForms(this);"></ext:HyperlinkButton>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>

                    
                    <ext:FormPanel ID="FormAddDirecciones" Cls="formGris FormGrid" runat="server" Hidden="false">
                        <Content>

                            <ext:TextField runat="server"
                                ID="txtNombreDir"
                                FieldLabel="Address Name"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField runat="server"
                                ID="txtDireccion"
                                FieldLabel="Address"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:Container runat="server" ID="grupoCorto" Cls="grupoCortoCls unsetW" StyleSpec="width:unset;" Layout="HBoxLayout">
                                <Items>
                                    <ext:NumberField runat="server"
                                        FieldLabel="No."
                                        LabelAlign="Top"
                                        ID="nbrNo"
                                        MinWidth="20"
                                        Cls="PaddLR5 unsetPos " />
                                    <ext:NumberField runat="server"
                                        FieldLabel="Floor"
                                        LabelAlign="Top"
                                        ID="nbrPiso"
                                        MinWidth="20"
                                        Cls="PaddLR5 unsetPos " />
                                    <ext:NumberField runat="server"
                                        FieldLabel="Door"
                                        LabelAlign="Top"
                                        ID="nbrPuerta"
                                        MinWidth="20"
                                        Cls="PaddLR5  unsetPos" />
                                    <ext:NumberField runat="server"
                                        FieldLabel="Postal Code"
                                        LabelAlign="Top"
                                        ID="nbrPostalCode"
                                        MinWidth="20"
                                        Cls="PaddLR5 unsetPos " />

                                </Items>
                            </ext:Container>





                            <ext:TextField runat="server"
                                ID="txtCiudad"
                                FieldLabel="Town"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField runat="server"
                                ID="txtProvincia"
                                FieldLabel="Province"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:TextField runat="server"
                                ID="txtPais"
                                FieldLabel="Country"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />


                        </Content>
                    </ext:FormPanel>

                    

                    <ext:FormPanel ID="FormAcceso" Cls="formGris FormGrid FormAcceso" runat="server" Hidden="true">
                        <Content>

                            <ext:ComboBox runat="server"
                                ID="txtTipoCamino"
                                FieldLabel="Road Type"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                            <ext:ComboBox runat="server"
                                ID="txtEstadoCamino"
                                FieldLabel="Road State"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />





                            <ext:ComboBox runat="server"
                                ID="txtCondicionCamino"
                                FieldLabel="Road Condition"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />

                            <ext:ComboBox runat="server"
                                ID="txtTipoLlave"
                                FieldLabel="Key Type"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />

                            <ext:ComboBox runat="server"
                                ID="txtTipoAcceso"
                                FieldLabel="Access Type"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />


                        </Content>
                    </ext:FormPanel>

                    

                    <ext:FormPanel ID="FormComentarios" Cls="formGris FormGrid FormComentarios" runat="server" Hidden="true" Layout="AnchorLayout">
                        <Content>

                            <ext:TextArea runat="server"
                                AnchorHorizontal="100%"
                                Height="200"
                                ID="txtComentarios"
                                FieldLabel="Comments"
                                LabelAlign="Top"
                                AllowBlank="false"
                                ValidationGroup="FORM"
                                CausesValidation="true" />


                        </Content>
                    </ext:FormPanel>





                </Items>

            </ext:Window>
            --%>

            <ext:Window ID="WinDirecciones"
                runat="server"
                Title="Locations"
                Width="572"
                Height="400"
                Modal="true"
                Centered="true"
                Cls="winDireccionesCls winForm-resp"
                Scrollable="Vertical"
                Layout="FitLayout"
                Hidden="true"
                Padding="20">
                <DockedItems>
                    <ext:Toolbar runat="server" ID="TBWinLocalizacionesMain" Cls="TBWinLocalizacionesMain" Dock="Top" Height="50">
                        <Items>
                            <ext:ToolbarFill></ext:ToolbarFill>
                            <ext:Button
                                runat="server"
                                ID="Button12"
                                IconCls="ico-addBtn"
                                Cls="btn-mini-ppal btnAdd noOffsetbtn"
                                Width="100"
                                Text="<%$ Resources:Comun, jsAgregar %>">
                            </ext:Button>

                        </Items>
                    </ext:Toolbar>
                </DockedItems>

                <Items>
                    <ext:GridPanel runat="server"
                        ID="grdLocations"
                        Cls="grdLocationsCls ctForm-resp">
                        <Store>
                            <ext:Store
                                ID="Store2"
                                runat="server"
                                Buffered="true"
                                RemoteFilter="true"
                                LeadingBufferZone="1000"
                                PageSize="50">
                                <Model>
                                    <ext:Model runat="server" IDProperty="Id">
                                        <Fields>
                                            <ext:ModelField Name="Id" />
                                            <ext:ModelField Name="Direccion" />
                                            <ext:ModelField Name="Provincia" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel runat="server">
                            <Columns>
                                <ext:Column runat="server" ID="Column1" DataIndex="Direccion" Flex="4"></ext:Column>
                                <ext:Column runat="server" ID="Column2" DataIndex="Provincia" Flex="3"></ext:Column>
                                <ext:WidgetColumn meta:resourcekey="ColDirecciones" ID="ColDirecciones" runat="server" Width="15" Cls="" DataIndex="" Align="Center" Hidden="false" MinWidth="45" Flex="1">
                                    <Widget>
                                        <ext:Button meta:resourcekey="btnColMore" runat="server" ID="Button17" IconCls="ico-head-direcciones-gr" Scale="Medium" Width="40" OverCls="none" PressedCls="Pressed-none" FocusCls="Focus-none" Cls=" whiteBckBtn" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                    </Widget>
                                </ext:WidgetColumn>

                                <ext:WidgetColumn meta:resourcekey="ColContactos" ID="ColContactos" runat="server" Width="15" Cls="col-More" DataIndex="" Align="Center" Hidden="false" MinWidth="45" Flex="1">
                                    <Widget>
                                        <ext:Button meta:resourcekey="btnColMore" runat="server" ID="Button9" IconCls="ico-head-users" Scale="Medium" Width="40" OverCls="none" PressedCls="Pressed-none" FocusCls="Focus-none" Cls=" whiteBckBtn" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                    </Widget>
                                </ext:WidgetColumn>

                                <ext:WidgetColumn meta:resourcekey="ColHorariosAcceso" ID="ColHorariosAcceso" runat="server" Width="15" Cls="col-More" DataIndex="" Align="Center" Hidden="false" MinWidth="45" Flex="1">
                                    <Widget>
                                        <ext:Button meta:resourcekey="btnColMore" runat="server" ID="Button10" IconCls="ico-head-reloj-gr" Scale="Medium" Width="40" OverCls="none" PressedCls="Pressed-none" FocusCls="Focus-none" Cls=" whiteBckBtn" Handler="Ext.Msg.alert('Button clicked', 'Hey TESTMORE! ' + this.getWidgetRecord().get('name'));" />
                                    </Widget>
                                </ext:WidgetColumn>


                            </Columns>
                        </ColumnModel>
                        <View>
                            <ext:GridView runat="server" EnableTextSelection="true">
                            </ext:GridView>
                        </View>
                        <Items>
                        </Items>

                    </ext:GridPanel>
                </Items>
            </ext:Window>

            <ext:Viewport ID="vwResp" runat="server" Cls="vwContenedor">
                <Listeners>
                    <Resize Handler="ResizerAside()"></Resize>
                </Listeners>
                <Items>
                    <ext:Button
                        runat="server"
                        ID="btnCollapseAsR"
                        Cls="btn-trans"
                        Handler="hidePnFilters();">
                    </ext:Button>
                    <ext:Panel runat="server"
                        ID="pnNavVistas"
                        Cls="pnNavVistas"
                        AnchorVertical="15%"
                        AriaRole="navigation">
                        <Items>
                            <ext:Container runat="server"
                                ID="conNavVistas"
                                Cls="nav-vistas">
                                <Items>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkSites"
                                        meta:resourceKey="lnkSites"
                                        Cls="lnk-navView lnk-noLine navActivo"
                                        Text="<%$ Resources:Comun, strEmplazamiento %>">
                                        <Listeners>
                                            <Click Handler="showForms(this,'../../Componentes/GridEmplazamientos.ascx', 'GridEmplazamientos', '/Componentes/js/GridEmplazamientos.js');" />
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkLocation"
                                        meta:resourceKey="lnkLocation"
                                        Handler="showForms(this,'../../Componentes/GridEmplazamientosLocalizaciones.ascx', 'GridEmplazamientosLocalizaciones', '/Componentes/js/GridEmplazamientosLocalizaciones.js');"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="<%$ Resources:Comun, strLocalizacion %>">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkAtributos"
                                        meta:resourceKey="lnkInventario"
                                        Handler="showForms(this,'../../Componentes/GridEmplazamientosAtributos.ascx', 'GridEmplazamientosAtributos', '../../Componentes/js/GridEmplazamientosAtributos.js');"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="<%$ Resources:Comun, strAdicional %>">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkMaps"
                                        Handler="showForms(this,'../../Componentes/Mapas.ascx', 'Mapas', '../../Componentes/js/Mapas.js');"
                                        meta:resourceKey="lnkMaps"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="<%$ Resources:Comun, strMapa %>">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkContracts"
                                        Handler="showForms(this,'ModGlobal/pages/EmplazamientosContratos', 'EmplazamientosContratos');"
                                        meta:resourceKey="lnkContracts"
                                        Cls="lnk-navView lnk-noLine"
                                        Hidden="true"
                                        Text="<%$ Resources:Comun, strContratos %>">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkContacts"
                                        meta:resourceKey="lnkContacts"
                                        Handler="showForms(this,'../../Componentes/GridEmplazamientosContactos.ascx', 'GridEmplazamientosContactos', '../../Componentes/js/GridEmplazamientosContactos.js');"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="<%$ Resources:Comun, strContactos %>">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkInventario"
                                        meta:resourceKey="lnkInventario"
                                        Handler="showForms(this,'../../Componentes/GridEmplazamientosInventarios.ascx', 'GridEmplazamientosInventario', '../../Componentes/js/GridEmplazamientosInventario.js');"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="<%$ Resources:Comun, strInventario %>">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkDocumentos"
                                        meta:resourceKey="lnkDocumentos"
                                        Handler="showForms(this,'../../Componentes/GridEmplazamientosDocumentos.ascx', 'GridEmplazamientosDocumentos', '../../Componentes/js/GridEmplazamientosDocumentos.js');"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="<%$ Resources:Comun, strDocumentos %>">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="btnPruebaExcel"
                                        Hidden="True"
                                        Handler="GeneralPruebaExcel();"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="Prueba Excel">
                                    </ext:HyperlinkButton>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                    <ext:Panel runat="server" ID="tagsContainer" Layout="ColumnLayout">
                        <Items>
                            <ext:ComboBox
                                runat="server"
                                ID="cmbClientes"
                                meta:resourceKey="cmbClientes"
                                DisplayField="Cliente"
                                ValueField="ClienteID"
                                Cls="comboGrid pos-boxGrid"
                                QueryMode="Local"
                                Hidden="true"
                                EmptyText="<%$ Resources:Comun, cmbClientes.EmptyText %>"
                                FieldLabel="<%$ Resources:Comun, cmbClientes.FieldLabel %>">
                                <Listeners>
                                    <%--<Select Fn="SeleccionarCliente" />--%>
                                    <%--<TriggerClick Fn="RecargarClientes" />--%>
                                </Listeners>
                                <Triggers>
                                    <ext:FieldTrigger
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </Triggers>
                                <Store>
                                    <ext:Store runat="server"
                                        AutoLoad="true"
                                        OnReadData="storeClientes_Refresh"
                                        RemoteSort="false">
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Model>
                                            <ext:Model runat="server" IDProperty="ClienteID">
                                                <Fields>
                                                    <ext:ModelField Name="ClienteID" Type="Int" />
                                                    <ext:ModelField Name="Cliente" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                        <Sorters>
                                            <ext:DataSorter Property="Cliente" Direction="ASC" />
                                        </Sorters>
                                        <Listeners>
                                            <%--<Load Handler="CargarStores();" />--%>
                                        </Listeners>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                            <%--<ext:Panel 
                                runat="server"
                                ID="tagContainersTemp"
                                Hidden="false">
                                <Items>
                                    <ext:Label
                                        runat="server"
                                        Cls="TagTemp"
                                        IconCls="ico-filters-16px" 
                                        Text="Temporal Filter"></ext:Label>
                                    <ext:Button
                                        runat="server"
                                        ID="Button6"
                                        Cls="CloseTemp"
                                        FocusCls="none"
                                        Handler=""></ext:Button>
                                </Items>
                            </ext:Panel>
                            <ext:Panel 
                                runat="server"
                                ID="tagContainersSaved" 
                                Hidden="false">
                                <Items>
                                    <ext:Label
                                        runat="server"
                                        Cls="TagSaved" 
                                        IconCls="ico-filters-16px"
                                        Text="Saved Filter"></ext:Label>
                                    <ext:Button 
                                        runat="server" 
                                        ID="Button7"
                                        Cls="CloseSaved"
                                        FocusCls="none"
                                        Handler=""></ext:Button>
                                </Items>
                            </ext:Panel>--%>
                        </Items>
                    </ext:Panel>

                    <ext:Container ID="hugeCt" runat="server" Layout="FitLayout" Cls="hugeMainCt">
                    </ext:Container>

                    <ext:Panel runat="server" ID="pnAsideR"
                        Header="false"
                        Border="false"
                        Width="360"
                        Layout="FitLayout"
                        Hidden="false">
                        <Listeners>
                            <AfterRender Handler="ResizerAside(this)"></AfterRender>
                        </Listeners>
                        <DockedItems>
                            <ext:Label Dock="Top"
                                MinHeight="60"
                                MinWidth="300"
                                PaddingSpec="20 0 0 20"
                                meta:resourcekey="lblAsideNameR"
                                ID="lblAsideNameR"
                                runat="server"
                                IconCls="ico-head-filters"
                                Cls="lblHeadAsideDock"
                                Text="<%$ Resources:Comun, strFiltros %>">
                            </ext:Label>
                            <ext:Label
                                runat="server"
                                Hidden="true"
                                ID="lbButtonSitesVisibles"
                                Text="<%$ Resources:Comun, jsMostrarEmplazamientosOcultos %>" />
                            <ext:Button
                                runat="server"
                                
                                ID="btnTgSitesVisible"
                                Text="<%$ Resources:Comun, jsMostrarEmplazamientosOcultos %>"
                                ToolTip="<%$ Resources:Comun, jsMostrarEmplazamientosOcultos %>"
                                EnableToggle="true"
                                Pressed="false"
                                Focusable="false"
                                Cls="btn-toggleGrid BtnFiltrosAlignR"
                                MaxWidth="200"
                                AriaLabel="<%$ Resources:Comun, jsMostrarEmplazamientosOcultos %>">
                                <Listeners>
                                    <Click Fn="btnTgSitesVisible" />
                                </Listeners>
                            </ext:Button>
                        </DockedItems>
                        <Items>

                            <ext:Panel
                                meta:resourcekey="ctAsideR"
                                ID="ctAsideR"
                                runat="server"
                                Border="false"
                                Header="false"
                                Layout="AnchorLayout"
                                Cls="">
                                <Items>
                                    <%--LEFT TABS MENU--%>
                                    <ext:Panel ID="mnAsideR" runat="server" Border="false" Header="false" AnchorVertical="100%" AnchorHorizontal="14%">
                                        <Items>
                                            <ext:Button
                                                runat="server"
                                                meta:resourcekey="btnMyF2"
                                                ID="btnMyF2"
                                                Cls="btnFiltersPlus-asR"
                                                ToolTip="<%$ Resources:Comun, strCrearFiltro %>"
                                                Handler="displayMenuSites('pnCFilters')">
                                            </ext:Button>
                                            <ext:Button
                                                runat="server"
                                                meta:resourcekey="btnMyFilters"
                                                ID="btnMyFilters"
                                                Cls="btnMyFilters-asR"
                                                ToolTip="<%$ Resources:Comun, strMisFiltros %>"
                                                Handler="displayMenuSites('pnGridsAsideMyFilters')">
                                            </ext:Button>
                                            <ext:Button
                                                runat="server"
                                                ID="btnMapFilters"
                                                Cls="btnFiltrosMapas-asR"
                                                Hidden="true"
                                                Handler="displayMenuSites('pnMapFilters')">
                                            </ext:Button>
                                        </Items>
                                    </ext:Panel>
                                    <%--PANELS--%>
                                    <ext:Panel
                                        MarginSpec="10 0 0 0"
                                        ID="pnGridsAside"
                                        runat="server"
                                        AnchorVertical="100%" AnchorHorizontal="86%"
                                        Border="false"
                                        OverflowY="Auto"
                                        Header="false"
                                        Layout="AnchorLayout"
                                        Hidden="false">
                                        <Listeners>
                                        </Listeners>
                                        <Items>



                                            <%--CREATE FILTERS PANEL--%>
                                            <ext:Panel
                                                runat="server"
                                                ID="pnCFilters"
                                                Margin="0"
                                                AnchorVertical="100%" AnchorHorizontal="100%"
                                                Hidden="false">
                                                <Items>

                                                    <ext:Panel
                                                        runat="server"
                                                        ID="pnCFiltersContainer">

                                                        <DockedItems>
                                                            <ext:Label
                                                                Dock="Top"
                                                                MarginSpec="0 8 20 0"
                                                                meta:resourcekey="lblGrid"
                                                                ID="lblGrid"
                                                                runat="server"
                                                                IconCls="btn-CFilter"
                                                                Cls="lblHeadAside"
                                                                Text="<%$ Resources:Comun, strCrearFiltro %>">
                                                            </ext:Label>
                                                        </DockedItems>
                                                        <Items>
                                                            <ext:TextField runat="server"
                                                                meta:resourcekey="pnNewFilter"
                                                                ID="pnNewFilter"
                                                                FieldLabel=""
                                                                LabelAlign="Top"
                                                                AllowBlank="false"
                                                                ValidationGroup="FORM"
                                                                CausesValidation="true"
                                                                EmptyText="<%$ Resources:Comun, strNombreFiltro %>" />
                                                            <ext:Button
                                                                runat="server"
                                                                ID="btnFilter"
                                                                Cls="btn-add"
                                                                Text="<%$ Resources:Comun, strNuevoFiltro %>">
                                                                <Listeners>
                                                                    <Click Fn="newFilter" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ComboBox runat="server"
                                                                meta:resourcekey="cmbField"
                                                                ID="cmbField"
                                                                FieldLabel="<%$ Resources:Comun, strCampo %>"
                                                                LabelAlign="Top"
                                                                DisplayField="Name"
                                                                EmptyText="<%$ Resources:Comun, strCampo %>"
                                                                Flex="1"
                                                                Cls="pnForm fieldFilter"
                                                                ValueField="Id"
                                                                QueryMode="Local">
                                                                <Store>
                                                                    <ext:Store
                                                                        ID="storeCampos"
                                                                        runat="server"
                                                                        AutoLoad="true"
                                                                        OnReadData="storeCampos_Refresh">
                                                                        <Model>
                                                                            <ext:Model runat="server" IDProperty="Id">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="Id" />
                                                                                    <ext:ModelField Name="Name" />
                                                                                    <ext:ModelField Name="typeData" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                        <Listeners>
                                                                            <DataChanged Fn="beforeLoadCmbField" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Fn="selectField" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:TextField runat="server"
                                                                meta:resourcekey="pnSearch"
                                                                ID="textInputSearch"
                                                                FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                LabelAlign="Top"
                                                                AllowBlank="false"
                                                                ValidationGroup="FORM"
                                                                CausesValidation="true"
                                                                EmptyText="<%$ Resources:Comun, strDependeDelCampo %>"
                                                                Cls="pnForm"
                                                                Hidden="false" />
                                                            <ext:DateField
                                                                runat="server"
                                                                ID="dateInputSearch"
                                                                FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                LabelAlign="Top"
                                                                AllowBlank="false"
                                                                ValidationGroup="FORM"
                                                                Cls="pnForm"
                                                                Hidden="true"
                                                                Format="dd/MM/yyyy">
                                                            </ext:DateField>
                                                            <ext:NumberField
                                                                runat="server"
                                                                ID="numberInputSearch"
                                                                FieldLabel="<%$ Resources:Comun, strBuscar %>"
                                                                LabelAlign="Top"
                                                                AllowBlank="false"
                                                                ValidationGroup="FORM"
                                                                Cls="pnForm"
                                                                Hidden="true">
                                                            </ext:NumberField>
                                                            <ext:ComboBox
                                                                ID="cmbOperatorField"
                                                                runat="server"
                                                                FieldLabel="<%$ Resources:Comun, strOperador %>"
                                                                DisplayField="Name"
                                                                EmptyText="<%$ Resources:Comun, strOperador %>"
                                                                Cls="pnForm"
                                                                Flex="1"
                                                                LabelAlign="Top"
                                                                Hidden="true"
                                                                QueryMode="Local">
                                                                <Items>
                                                                    <ext:ListItem Text="=" Value="IGUAL" />
                                                                    <ext:ListItem Text="<" Value="MENOR" />
                                                                    <ext:ListItem Text=">" Value="MAYOR" />
                                                                </Items>
                                                            </ext:ComboBox>
                                                            <ext:MultiCombo runat="server"
                                                                Hidden="true"
                                                                ID="cmbTiposDinamicos"
                                                                FieldLabel="<%$ Resources:Comun, strTipoDinamico %>"
                                                                LabelAlign="Top"
                                                                DisplayField="Name"
                                                                EmptyText="<%$ Resources:Comun, strTipoDinamico %>"
                                                                Flex="1"
                                                                Cls="pnForm"
                                                                ValueField="Id"
                                                                QueryMode="Local">
                                                                <Store>
                                                                    <ext:Store
                                                                        ID="storeTiposDinamicos"
                                                                        runat="server"
                                                                        AutoLoad="false"
                                                                        OnReadData="storeTiposDinamicos_Refresh">
                                                                        <Proxy>
                                                                            <ext:PageProxy />
                                                                        </Proxy>
                                                                        <Model>
                                                                            <ext:Model runat="server" IDProperty="Id">
                                                                                <Fields>
                                                                                    <ext:ModelField Name="Id" />
                                                                                    <ext:ModelField Name="Name" />
                                                                                </Fields>
                                                                            </ext:Model>
                                                                        </Model>
                                                                        <Listeners>
                                                                            <DataChanged Fn="beforeLoadCmbField" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:MultiCombo>
                                                            <ext:Button
                                                                runat="server" meta:resourcekey="ColMas"
                                                                ID="btnAdd"
                                                                IconCls="ico-addBtn"
                                                                Cls="btn-mini-ppal btnAdd"
                                                                Text="<%$ Resources:Comun, jsAgregar %>">
                                                                <Listeners>
                                                                    <Click Fn="addElementFilter" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Panel runat="server" ID="tagsHeader">
                                                                <Items>
                                                                    <ext:Label
                                                                        meta:resourcekey="lblCampo"
                                                                        runat="server"
                                                                        Cls="tabsLabels"
                                                                        Text="<%$ Resources:Comun, strCampo %>">
                                                                    </ext:Label>
                                                                    <ext:Label
                                                                        meta:resourcekey="lblBuscar"
                                                                        runat="server"
                                                                        Cls="tabsLabels"
                                                                        Text="<%$ Resources:Comun, strBuscar %>">
                                                                    </ext:Label>
                                                                </Items>
                                                            </ext:Panel>
                                                            <ext:Panel runat="server" ID="pnTagContainer" Scrollable="Vertical" MaxHeight="100">
                                                                <Items>
                                                                </Items>
                                                            </ext:Panel>

                                                            <ext:Button
                                                                runat="server"
                                                                meta:resourcekey="btnAplyFilter"
                                                                ID="btnAplyFilter"
                                                                Cls="btn-end"
                                                                Text="<%$ Resources:Comun, strAplicar %>">
                                                                <Listeners>
                                                                    <Click Fn="aplyFilter" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button
                                                                runat="server"
                                                                meta:resourcekey="btnSaveFilter"
                                                                ID="btnSaveFilter"
                                                                Cls="btn-save"
                                                                Text="<%$ Resources:Comun, btnGuardar.Text %>">
                                                                <Listeners>
                                                                    <Click Fn="saveFilter" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Panel>
                                                </Items>
                                            </ext:Panel>


                                            <%--                                                                                                <ext:Label
                                                        meta:resourcekey="lblMyFilters"
                                                        ID="Label1"
                                                        runat="server"
                                                        IconCls="ico-head-my-filters"
                                                        Cls="lblHeadAside"
                                                        Text="<%$ Resources:Comun, strMisFiltros %>">
                                                    </ext:Label>--%>


                                            <%--MY FILTERS PANEL--%>


                                            <ext:Panel
                                                runat="server"
                                                ID="pnGridsAsideMyFilters"
                                                Layout="FitLayout"
                                                OverflowY="Auto"
                                                AnchorVertical="100%" AnchorHorizontal="100%"
                                                Hidden="true">

                                                <DockedItems>
                                                    <ext:Label
                                                        Dock="Top"
                                                        MarginSpec="0 8 20 8"
                                                        meta:resourcekey="lblGrid"
                                                        ID="Label1"
                                                        runat="server"
                                                        IconCls="ico-head-my-filters"
                                                        Cls="lblHeadAside"
                                                        Text="<%$ Resources:Comun, strMisFiltros %>">
                                                    </ext:Label>
                                                </DockedItems>


                                                <Items>
                                                    <ext:GridPanel
                                                        MarginSpec="8 8 120 8"
                                                        ID="GridMyFilters"
                                                        runat="server"
                                                        Header="false"
                                                        Border="false"
                                                        Scrollable="Vertical"
                                                        Cls="GridMyFilters">
                                                        <Store>
                                                            <ext:Store
                                                                runat="server"
                                                                PageSize="10"
                                                                AutoLoad="false"
                                                                OnReadData="storeMyFilters_Refresh">
                                                                <Model>
                                                                    <ext:Model runat="server" IDProperty="ID">
                                                                        <Fields>
                                                                            <ext:ModelField Name="GestionFiltroID" />
                                                                            <ext:ModelField Name="UsuarioID" />
                                                                            <ext:ModelField Name="NombreFiltro" />
                                                                            <ext:ModelField Name="JsonItemsFiltro" />
                                                                            <ext:ModelField Name="Pagina" />
                                                                            <ext:ModelField Name="check" Type="Boolean" DefaultValue="false" />
                                                                        </Fields>
                                                                    </ext:Model>
                                                                </Model>
                                                            </ext:Store>
                                                        </Store>
                                                        <ColumnModel runat="server">
                                                            <Columns>

                                                                <ext:Column runat="server"
                                                                    Sortable="true"
                                                                    DataIndex="NombreFiltro"
                                                                    Width="150"
                                                                    Align="Start">
                                                                </ext:Column>

                                                                <ext:WidgetColumn
                                                                    meta:resourcekey="ColMas"
                                                                    ID="ColMas"
                                                                    runat="server"
                                                                    Width="15"
                                                                    Cls="col-More"
                                                                    Align="Center"
                                                                    Hidden="false"
                                                                    MinWidth="45">
                                                                    <Widget>
                                                                        <ext:Button
                                                                            meta:resourcekey="btnColMore"
                                                                            runat="server"
                                                                            ID="btnColMore"
                                                                            Width="16"
                                                                            ToolTip="<%$ Resources:Comun, btnEliminar.ToolTip %>"
                                                                            OverCls="Over-btnMore"
                                                                            PressedCls="Pressed-none"
                                                                            FocusCls="Focus-none"
                                                                            Cls="BtnDeleteChk">
                                                                            <Listeners>
                                                                                <Click Fn="DeleteFilter" />
                                                                            </Listeners>
                                                                        </ext:Button>
                                                                    </Widget>
                                                                </ext:WidgetColumn>

                                                                <ext:WidgetColumn
                                                                    meta:resourcekey="ColMas"
                                                                    ID="colBtnEdit"
                                                                    runat="server"
                                                                    Width="18"
                                                                    Cls="col-More"
                                                                    Align="Center"
                                                                    Hidden="false"
                                                                    MinWidth="45">
                                                                    <Widget>
                                                                        <ext:Button
                                                                            meta:resourcekey="btnColMore"
                                                                            runat="server"
                                                                            ID="btnCheck"
                                                                            Width="18"
                                                                            ToolTip="<%$ Resources:Comun, btnEditar.ToolTip %>"
                                                                            OverCls="Over-btnMore"
                                                                            PressedCls="Pressed-none"
                                                                            FocusCls="Focus-none"
                                                                            Cls="BtnEditChk">
                                                                            <Listeners>
                                                                                <Click Fn="MostrarEditarFiltroGuardado" />
                                                                            </Listeners>
                                                                        </ext:Button>
                                                                    </Widget>
                                                                </ext:WidgetColumn>

                                                                <ext:WidgetColumn
                                                                    meta:resourcekey="ColMas"
                                                                    ID="colChkAplyFilter"
                                                                    runat="server"
                                                                    Width="18"
                                                                    Cls="col-Chk"
                                                                    Align="Center"
                                                                    Hidden="false"
                                                                    MinWidth="45">
                                                                    <Widget>
                                                                        <ext:Button
                                                                            runat="server"
                                                                            ID="chkAplyFilter"
                                                                            Cls="btn-trans btnApply-filter"
                                                                            ToolTip="<%$ Resources:Comun, strAplicar %>"
                                                                            DataIndex="check">
                                                                            <Listeners>
                                                                                <Click Fn="AplyFilterSaved" />
                                                                            </Listeners>
                                                                        </ext:Button>
                                                                    </Widget>
                                                                </ext:WidgetColumn>

                                                            </Columns>
                                                        </ColumnModel>
                                                        <View>
                                                            <ext:GridView runat="server" LoadMask="false" />
                                                        </View>
                                                        <Plugins>
                                                            <ext:GridFilters runat="server" />
                                                        </Plugins>
                                                    </ext:GridPanel>
                                                </Items>
                                            </ext:Panel>



                                            <%--MAP FILTERS PANEL--%>



                                            <ext:FormPanel ID="pnMapFilters" runat="server" Hidden="true" Layout="VBoxLayout"
                                                AnchorVertical="100%" AnchorHorizontal="100%" OverflowY="Auto">
                                                <DockedItems>
                                                    <ext:Label ID="Label3" runat="server" IconCls="ico-head-my-filters" Cls="lblHeadAside" Text="Map Filters" Dock="Top"></ext:Label>
                                                </DockedItems>
                                                <LayoutConfig>
                                                    <ext:VBoxLayoutConfig Align="Center"></ext:VBoxLayoutConfig>
                                                </LayoutConfig>
                                                <Defaults>
                                                    <ext:Parameter Name="width" Value="90%" Mode="Auto" />
                                                </Defaults>
                                                <Items>



                                                    <ext:NumberField runat="server"
                                                        ID="numRadio"
                                                        meta:resourceKey="numRadio"
                                                        FieldLabel="Radio (Km)"
                                                        LabelAlign="Top"
                                                        AllowDecimals="false"
                                                        Number="10"
                                                        MaxValue="25" />
                                                    <ext:ComboBox runat="server"
                                                        ID="cmbClusters"
                                                        meta:resourceKey="cmbClusters"
                                                        FieldLabel="Cluster"
                                                        LabelAlign="Top"
                                                        Editable="false"
                                                        QueryMode="Local">
                                                    </ext:ComboBox>
                                                    <ext:ComboBox runat="server"
                                                        ID="cmbClientesPanel"
                                                        meta:resourceKey="cmbClientes"
                                                        FieldLabel="Clientes"
                                                        LabelAlign="Top"
                                                        Editable="false"
                                                        QueryMode="Local">
                                                        <Listeners>
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                    <ext:MultiCombo runat="server"
                                                        ID="multiOperadores"
                                                        meta:resourceKey="multiOperadores"
                                                        FieldLabel="Operadores"
                                                        LabelAlign="Top"
                                                        QueryMode="Remote"
                                                        Editable="false"
                                                        SelectionMode="Selection" />
                                                    <ext:MultiCombo runat="server"
                                                        ID="multiEstadosGlobales"
                                                        meta:resourceKey="multiEstadosGlobales"
                                                        FieldLabel="Estados Globales"
                                                        LabelAlign="Top"
                                                        Editable="false"
                                                        SelectionMode="Selection" />
                                                    <ext:MultiCombo runat="server"
                                                        ID="multiCategoriasSitios"
                                                        meta:resourceKey="multiCategoriasSitios"
                                                        FieldLabel="Categorias Sitios"
                                                        LabelAlign="Top"
                                                        Editable="false"
                                                        SelectionMode="Selection" />
                                                    <ext:MultiCombo runat="server"
                                                        ID="multiEmplazamientosTipos"
                                                        meta:resourceKey="multiEmplazamientosTipos"
                                                        FieldLabel="Emplazamientos Tipos"
                                                        LabelAlign="Top"
                                                        Editable="false"
                                                        SelectionMode="Selection" />
                                                    <ext:MultiCombo runat="server"
                                                        ID="multiEmplazamientosTamanos"
                                                        meta:resourceKey="multiEmplazamientosTamanos"
                                                        FieldLabel="Emplazamientos Tamaños"
                                                        LabelAlign="Top"
                                                        Editable="false"
                                                        SelectionMode="Selection" />

                                                    <ext:Button runat="server"
                                                        MarginSpec="0 0 80 0"
                                                        ID="btnAplicar"
                                                        Cls="btn-end btnApplyFiltersMap"
                                                        meta:resourceKey="btnAplicar"
                                                        Text="Aplicar">
                                                        <Listeners>
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:FormPanel>




                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>

</body>
</html>
