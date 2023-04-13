<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoPresupuestos.aspx.cs" Inherits="TreeCore.PaginasComunes.SeguimientoPresupuestos" %>


<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />
    <link href="css/styleSeguimientoPresupuestos.css" rel="stylesheet" type="text/css" />
    <script src="js/SeguimientoPresupuestos.js"></script>
    <!--<script src="js/common.js"></script>-->

    <script runat="server">

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {

                this.GridPanelMain.Store.Primary.DataSource = TreeCore.PaginasComunes.SeguimientoPresupuestos.DataGridP1;


                this.grdTracking.Store.Primary.DataSource = new object[]
{
                new object[] {"Pedro Ramírez", "Admin", "Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem.","Pending Approval", DateTime.Now },
                new object[] {"Juan Oliveira", "User Acquisitions", "Cum sociis natoque penatibus et magnis dis parturient montes", "Dossier Submitted", DateTime.Now }

};

                this.grdTracking.Store.Primary.DataBind();


                this.GridDocum.Store.Primary.DataSource = new object[]
{
                new object[] { "Docu1", "Form" },
                new object[] { "Docu2","Contract" },

};

            }



        }



    </script>


    <script type="text/javascript">
        var align = function (tb) {
            tb.alignTo(Ext.getBody(), 'tr-tr', [-10, 10]);
        };
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">


                <Listeners>
                    <WindowResize Handler="GridResizer()" />
                </Listeners>
            </ext:ResourceManager>
            <%-- //WINDOWS--%>

         <ext:Window meta:resourceKey="winUndone" runat="server" ID="winUndone" Hidden="true" Layout="FormLayout" MinWidth="430" Title="Mark As Undone" BodyCls="winalert-body">
                <Items>
                    <ext:FormPanel runat="server" BodyCls="winalert-body">
                        <Items>
                            <ext:Label meta:resourceKey="msgWindow" ID="msgWindow" runat="server" Text="Cambiar el estado a no hecho, borrara todos los campos y dejara la tarea lista para empezar de nuevo"></ext:Label>


                        </Items>

                        <DockedItems>
                            <ext:Toolbar runat="server" ID="tbButtonsWin1" Dock="Bottom">
                                <Items>
                                    <ext:ToolbarFill></ext:ToolbarFill>
                                    <ext:Button meta:resourceKey="btnCancelWin" runat="server" ID="btnCancelWin" Cls="btn-cancel" MinWidth="150" Text="Cancelar " />
                                    <ext:Button meta:resourceKey="btnAcceptWin" runat="server" ID="btnAcceptWin" Cls="btn-accept" MinWidth="190" Text="Marca como no hecha" />

                                </Items>
                            </ext:Toolbar>
                        </DockedItems>

                    </ext:FormPanel>
                </Items>
            </ext:Window>

            <ext:Window
                meta:resourceKey="WinTrackChanges"
                runat="server" ID="WinTrackChanges"
                Width="210"
                MinHeight="120"
                Hidden="true"
                Title="Cambios Tracking"
                Closable="false"
                X="200"
                Y="300"
                Cls="WinTrackChanges"
                IconCls="ico-cambiostracking"
                Resizable="false">
                <Items>
                    <ext:FormPanel runat="server"
                        ID="fp1"
                        Cls="formGris">
                        <Items>
                            <ext:FieldSet runat="server" Cls="fielset-noborder">
                                <Defaults>
                                    <ext:Parameter Name="LabelWidth" Value="115" />
                                </Defaults>
                                <Items>
                                    <ext:ToolbarSeparator Cls="sep-subtitulos-trackchanges" />

                                    <ext:Label meta:resourceKey="lblCandidatos" ID="lblCandidatos" Cls="spanLbl" runat="server" Text="Candidatos"></ext:Label>
                                    <ext:ToolbarSeparator />

                                    <ext:Label meta:resourceKey="lblCandidatosOld" ID="lblCandidatosOld" IconCls="ico-cambio-viejo" Name="price" runat="server" Text="OLD CAND" />
                                    <ext:ToolbarSeparator />

                                    <ext:Label meta:resourceKey="lblCandidatosNew" ID="lblCandidatosNew" IconCls="ico-cambio-nuevo" Name="price" runat="server" Text="NUEVO CAND" />

                                    <ext:ToolbarSeparator Cls="sep-subtitulos-trackchanges" />


                                    <ext:Label meta:resourceKey="lblFianza" ID="lblFianza" Cls="spanLbl" runat="server" Text="Fianza"></ext:Label>
                                    <ext:ToolbarSeparator />

                                    <ext:Label  meta:resourceKey="lblPrecioPrev" ID="lblPrecioPrev" IconCls="ico-cambio-viejo" Name="price" runat="server" Text="no" />
                                    <ext:ToolbarSeparator />
                                    <ext:Label meta:resourceKey="lblPrecioNext" ID="lblPrecioNext" IconCls="ico-cambio-nuevo" Name="price" runat="server" Text="si" />
                                    <ext:ToolbarSeparator Cls="sep-subtitulos-trackchanges" />


                                    <ext:Label meta:resourceKey="lblImporteFianza" ID="lblImporteFianza" Cls="spanLbl" runat="server" Text="Importe Fianza"></ext:Label>
                                    <ext:ToolbarSeparator />

                                    <ext:Label meta:resourceKey="lblPrecioPrev2" ID="lblPrecioPrev2" IconCls="ico-cambio-viejo" Name="price" runat="server" Text="0" />
                                    <ext:ToolbarSeparator />
                                    <ext:Label meta:resourceKey="lblPrecioNext2" ID="lblPrecioNext2" IconCls="ico-cambio-nuevo" Name="price" runat="server" Text="430" />


                                </Items>
                            </ext:FieldSet>
                        </Items>
                    </ext:FormPanel>
                </Items>
            </ext:Window>


            <ext:Window
                runat="server" ID="WinTrackDocum"
                meta:resourceKey="WinTrackDocum"
                Width="380"
                MinHeight="120"
                Hidden="true"
                Title="Cambios Documentación"
                IconCls="ico-documentaciontracking"
                Cls="WinTrackDocum"
                Border="false"
                Resizable="false"
                Closable="false"
                X="200"
                Y="300">
                <Items>

                    <ext:GridPanel
                        ID="GridDocum"
                        runat="server"
                        MultiSelect="true"
                        ForceFit="true"
                        Border="false"
                        Header="false"
                        Cls="gridPanel">
                        <Store>
                            <ext:Store ID="StoreDocs" runat="server">
                                <Model>
                                    <ext:Model runat="server">
                                        <Fields>

                                            <ext:ModelField Name="Document" DefaultValue="1" />
                                            <ext:ModelField Name="Type" DefaultValue="1" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                            </ext:Store>
                        </Store>
                        <ColumnModel>
                            <Columns>
                                <ext:Column meta:resourceKey="ColDocumento" ID="ColDocumento" runat="server" Text="Documento" DataIndex="Document" Width="140" />
                                <ext:Column meta:resourceKey="ColTipo" ID="ColTipo" runat="server" Text="Tipo" DataIndex="Type" Width="110">
                                </ext:Column>

                            </Columns>
                        </ColumnModel>


                    </ext:GridPanel>

                </Items>
            </ext:Window>


            <ext:Viewport ID="vwResp" runat="server" Layout="">
                <Items>
                    <ext:Button runat="server" ID="btnCollapseAsR" Cls="btn-trans" Handler="hidePn();" Hidden="true"></ext:Button>
                    <ext:Panel runat="server"
                        Hidden="true"
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
                                        Text="CURRENT VIEW">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkLocation"
                                        meta:resourceKey="lnkLocation"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="VIEW 2">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkAdditional"
                                        meta:resourceKey="lnkAdditional"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="VIEW 3">
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server"
                                        ID="lnkDashboard"
                                        meta:resourceKey="lnkDashboard"
                                        Cls="lnk-navView lnk-noLine"
                                        Text="VIEW 4">
                                    </ext:HyperlinkButton>

                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                    <ext:Container runat="server" ID="ctBtnSldr">
                        <Items>
                            <ext:Button runat="server" ID="btnPrevSldr" IconCls="ico-prev" Cls="btnMainSldr" Handler="moveCtSldr(this);"></ext:Button>
                            <ext:Button runat="server" ID="btnNextSldr" IconCls="ico-next" Handler="moveCtSldr(this);" Disabled="true"></ext:Button>
                        </Items>
                    </ext:Container>
                    <ext:Container ID="ctSlider" runat="server">
                        <Items>
                            <ext:Container ID="ctMain1" runat="server" Layout="">
                                <Items>

                                    <ext:GridPanel
                                        ID="GridPanelMain"
                                        runat="server"
                                        Height="900"
                                        OverflowY="Auto"
                                        OverflowX="Hidden"
                                        Cls="grdNoHeader gridPanel ">

                                        <Store>
                                            <ext:Store runat="server">

                                                <Model>
                                                    <ext:Model runat="server" IDProperty="ID">
                                                        <Fields>
                                                            <ext:ModelField Name="ID" />
                                                            <ext:ModelField Name="NombreCategoria" />
                                                            <ext:ModelField Name="NombreItemTarea" />
                                                            <ext:ModelField Name="CosteItemUnidad" />
                                                            <ext:ModelField Name="CosteItemTipoUnidad" />
                                                            <ext:ModelField Name="NumItems" />
                                                            <ext:ModelField Name="CosteTotalFila" Type="Int" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>

                                            </ext:Store>
                                        </Store>
                                        <ColumnModel runat="server">
                                            <Columns>



                                                <ext:TemplateColumn runat="server" DataIndex="" MenuDisabled="true" Text="" Flex="5">
                                                    <Template runat="server">
                                                        <Html>
                                                            <tpl for=".">
                                                            <div class="customCol1">
                                                                
                                                                <p class="TopTitleCustomCol1">{NombreCategoria}</p> 
                                                                <p class="MainTitleCustomCol1">{NombreItemTarea}</p> 
                                                                <div  class="customColDiv1">
                                                                    {CosteItemUnidad} / {CosteItemTipoUnidad}
                                                                                         
                                                                </div>
                                                            </div>
                                                        </tpl>
                                                        </Html>
                                                    </Template>
                                                </ext:TemplateColumn>

                                                <ext:ComponentColumn
                                                    runat="server"
                                                    Editor="true"
                                                    DataIndex="NumItems"
                                                    Flex="2">
                                                    <Component>
                                                        <ext:NumberField runat="server" />
                                                    </Component>
                                                </ext:ComponentColumn>

                                                <ext:Column
                                                    TdCls="bold"
                                                    Align="Center"
                                                    MinWidth="135"
                                                    runat="server"
                                                    DataIndex="CosteTotalFila"
                                                    Flex="2"
                                                    SummaryType="Sum">
                                                    <Renderer Handler="return value + '€';" />

                                                    <%-- Remove after fixing #563 --%>
                                                    <SummaryRenderer Handler="return 'TOTAL ' + value +'€';" />
                                                </ext:Column>




                                            </Columns>
                                        </ColumnModel>
                                        <Features>

                                            <ext:Summary ID="ResumenTotalPreciosG1" runat="server" Dock="Bottom" Height="100" />
                                        </Features>


                                        <DockedItems>

                                            <ext:Toolbar runat="server" Dock="Top" ID="TBTitle" Layout="" Height="52">

                                                <Items>


                                                    <ext:Label meta:resourceKey="LblMainTlt" ID="LblMainTlt" runat="server" Cls="big-lbl-title LblElipsis" Text="Esquina Bellavista HBTS" Flex="8"></ext:Label>

                                                    <ext:ToolbarFill />

                                                    <ext:Button meta:resourceKey="btnLocation" ID="btnLocation" runat="server" Cls="btnLocation" OverCls="clear-over" FocusCls="clear-focus" PressedCls="green-pressed-loc" EnableToggle="true" Text="" TextAlign="Right">
                                                        <Listeners>
                                                            <Click Fn="ShowMapPn" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button meta:resourceKey="btnImport" ID="btnImport" runat="server" Cls="btnImport" OverCls="clear-over" FocusCls="clear-focus" PressedCls="green-pressed-loc" EnableToggle="true" Text="" TextAlign="Right">
                                                        <Listeners>
                                                            <Click Fn="ShowMapPn" />
                                                        </Listeners>
                                                    </ext:Button>

                                                 




                                                </Items>
                                            </ext:Toolbar>



                                            <ext:Toolbar runat="server" ID="TBSubtitle" Cls="TBSubtitle">
                                                <Items>
                                                    <ext:Label meta:resourceKey="lblSubtitle" ID="lblSubtitle" runat="server" Cls="subtitle-code" Text="CODIGO: 200"></ext:Label>
                                                    <ext:Label meta:resourceKey="lblSubtitleSubcode" ID="lblSubtitleSubcode" runat="server" Cls="subtitle-subcode" Text="TASC - Contract Acquisition"></ext:Label>
                                                    <ext:ToolbarFill></ext:ToolbarFill>
                                                </Items>
                                            </ext:Toolbar>





                                            <ext:Toolbar runat="server" ID="TBTools" Cls="PadTBMid">
                                                <Items>

                                                    <ext:TextField
                                                        meta:resourceKey="txtSearchBox"
                                                        ID="txtSearchBox"
                                                        Flex="6"
                                                        runat="server"
                                                        EmptyText="Search">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Clear" />
                                                            <ext:FieldTrigger Icon="Search" />
                                                        </Triggers>

                                                    </ext:TextField>

                                                    <ext:ComboBox
                                                        meta:resourceKey="cmbMoreToLess"
                                                        ID="cmbMoreToLess"
                                                        Flex="6"
                                                        runat="server"
                                                        EmptyText="More To Less">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Clear" />
                                                        </Triggers>
                                                    </ext:ComboBox>

                                                    <ext:ToolbarFill Flex="1"></ext:ToolbarFill>

                                                    <ext:Button runat="server" meta:resourceKey="btnFiltrosMain" ID="btnFiltrosMain"  EnableToggle="true" Text="Filtros" Cls="btn-ppal-winForm" MinWidth="90" IconCls="ico-filter-white" IconAlign="Left">
                                                        <Listeners>
                                                            <Click Fn="ShowFiltersTb" />
                                                        </Listeners>
                                                    </ext:Button>

                                                </Items>
                                            </ext:Toolbar>


                                            <ext:Toolbar meta:resourceKey="toolbarfiltros1" ID="toolbarfiltros1" runat="server" Dock="Top"  Layout="HBoxLayout" Cls="bld-cnt-fonts PadTBMid" Hidden="true">
                                                <Items>



                                                    <ext:ComboBox
                                                        meta:resourceKey="cmbCategoryF"
                                                        ID="cmbCategoryF"
                                                        Flex="1"
                                                        FieldLabel="Category"
                                                        runat="server"
                                                        EmptyText="PText"
                                                        LabelAlign="Top">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Clear" />
                                                        </Triggers>
                                                    </ext:ComboBox>

                                                    <ext:ComboBox
                                                        meta:resourceKey="cmbDateF"
                                                        ID="cmbDateF"
                                                        Flex="1"
                                                        FieldLabel="Date"
                                                        LabelAlign="Top"
                                                        runat="server"
                                                        EmptyText="PText">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Clear" />
                                                        </Triggers>
                                                    </ext:ComboBox>



                                                </Items>
                                            </ext:Toolbar>



                                            <ext:Toolbar runat="server" Dock="Top" ID="toolbarfiltros2" Layout="" Cls="bld-cnt-fonts PadTBBot" Hidden="true">
                                                <Items>

                                                    <ext:ComboBox
                                                        meta:resourceKey="cmbElementF"
                                                        ID="cmbElementF"
                                                        Flex="1"
                                                        runat="server"
                                                        FieldLabel="Element"
                                                        LabelAlign="Top"
                                                        EmptyText="PText">
                                                        <Triggers>
                                                            <ext:FieldTrigger Icon="Clear" />
                                                        </Triggers>

                                                    </ext:ComboBox>

                                                    <ext:Button runat="server"
                                                        meta:resourceKey="btnDoneUndoneTasks"
                                                        Flex="1"
                                                        MinWidth="160"
                                                        ID="btnDoneUndoneTasks"
                                                        EnableToggle="true"
                                                        Cls="btn-toggleGrid-Cntract"
                                                        Text="Show tasks done"
                                                        TextAlign="Left"
                                                        AriaLabel="Ver Inactivos"
                                                        ToolTip="Ver Inactivos">
                                                    </ext:Button>



                                                </Items>
                                            </ext:Toolbar>


                                        </DockedItems>
                                    </ext:GridPanel>

                                </Items>

                            </ext:Container>
                            <ext:Container ID="ctMain2" runat="server">
                                <Items>


                                    <ext:GridPanel
                                        runat="server"
                                        ID="grdTracking"
                                        Border="false"
                                        Header="false"
                                        ForceFit="true"
                                        Height="340"
                                        Cls="gridPanel grdNoHeader grdOneCol">

                                        <DockedItems>
                                            <ext:Toolbar runat="server" ID="tlbBase"
                                                Dock="Top"
                                                Cls="tlbGrid">
                                                <Items>
                                                    <ext:Button runat="server"
                                                        meta:resourceKey="btnNuevoEstado"
                                                        ID="btnNuevoEstado"
                                                        Cls="btn-trans"
                                                        AriaLabel="Nuevo Cambio Estado"
                                                        ToolTip="Nuevo Cambio de Estado"
                                                        Handler="anadir();">
                                                        <Menu>
                                                            <ext:Menu runat="server">
                                                                <Items>
                                                                    <ext:MenuItem meta:resourceKey="btnNuevoEstado1" ID="mnuItmNuevoEstado1" runat="server" Text="Nuevo Estado 1" Icon="GroupAdd" />
                                                                    <ext:MenuItem meta:resourceKey="btnNuevoEstado2" ID="mnuItmNuevoEstado2" runat="server" Text="Nuevo Estado 2" Icon="GroupDelete" />
                                                                    <ext:MenuItem meta:resourceKey="btnNuevoEstado3" ID="mnuItmNuevoEstado3" runat="server" Text="Nuevo Estado 3" Icon="GroupEdit" />
                                                                </Items>
                                                            </ext:Menu>
                                                        </Menu>
                                                    </ext:Button>
                                                    <ext:Button runat="server"
                                                        meta:resourceKey="btnAnadirComm"
                                                        ID="btnAnadirComm"
                                                        Cls="btn-trans"
                                                        AriaLabel="Añadir Comentario"
                                                        ToolTip="Añadir Comentario" />
                                                    <ext:Button runat="server"
                                                        meta:resourceKey="btnRefrescar"
                                                        ID="btnRefrescar"
                                                        Cls="btn-trans"
                                                        AriaLabel="Actualizar"
                                                        ToolTip="Actualizar" />
                                                    <ext:Button runat="server"
                                                        meta:resourceKey="btnDescargar"
                                                        ID="btnDescargar"
                                                        Cls="btnDescargar"
                                                        AriaLabel="Descargar"
                                                        ToolTip="Descargar" />
                                                    <ext:Button runat="server"
                                                        meta:resourceKey="btnActivar"
                                                        ID="btnActivar"
                                                        Cls="btn-trans"
                                                        AriaLabel="Activar Comentario"
                                                        ToolTip="Activar Comentario" />
                                                    <ext:Button runat="server"
                                                        meta:resourceKey="btnDesactivar"
                                                        ID="btnDesactivar"
                                                        Cls="btn-trans"
                                                        AriaLabel="Desactivar Comentario"
                                                        ToolTip="Desactivar Comentario" />
                                                    <ext:Button runat="server"
                                                        meta:resourceKey="btnActivos"
                                                        ID="btnActivos"
                                                        Width="41"
                                                        EnableToggle="true"
                                                        Cls="btn-toggleGrid"
                                                        AriaLabel="Ver Inactivos"
                                                        ToolTip="Ver Inactivos" />
                                                    <ext:Button runat="server"
                                                        meta:resourceKey="btnVerWorkFlow"
                                                        ID="btnVerWorkFlow"
                                                        Width="41"
                                                        EnableToggle="true"
                                                        AriaLabel="Ver Workflow"
                                                        ToolTip="Ver Workflow"
                                                        Handler="ShowWorkFlow();" />
                                                </Items>
                                            </ext:Toolbar>
                                        </DockedItems>
                                        <Store>
                                            <ext:Store runat="server">
                                                <Model>
                                                    <ext:Model runat="server">
                                                        <Fields>
                                                            <ext:ModelField Name="userNameCol" Type="String" />
                                                            <ext:ModelField Name="userProfileCol" Type="String" />
                                                            <ext:ModelField Name="commentCol" Type="String" />
                                                            <ext:ModelField Name="stateCol" Type="String" />
                                                            <ext:ModelField Name="dateCol" Type="Date" />
                                                        </Fields>
                                                    </ext:Model>
                                                </Model>
                                            </ext:Store>
                                        </Store>


                                        <ColumnModel runat="server">
                                            <Columns>
                                                <ext:TemplateColumn runat="server" Width="" DataIndex="" MenuDisabled="true">
                                                    <Template runat="server">
                                                        <Html>
                                                            <div runat="server" class="dvTrack">
														 <div runat="server" class="dvCntComm">
															 <div runat="server" class="dvUser">
																 <img src="../../../ima/ico-profile-user.svg"></img>
																 <div runat="server" class="userInfo">
																 <span class="userName">{userNameCol}</span>
																 <span class="userProfile">{userProfileCol}</span>
																</div>
															 </div>
															 <div runat="server" class="dvComm">
															<p>{commentCol}</p>
															</div>
														</div>
														<div runat="server" class="dvCntState" id="Testdiv2">
															<h6 runat="server" class="stateName">{stateCol}</h6>
															<button runat="server" class="btnDocUploaded" onmouseover="ShowHoverDocum();" onmouseout="HideHoverDocum();"></button>
															<button id="btnChangesTrack" runat="server" class="btnChangesTrack" onmouseover="ShowHoverChanges();" onmouseout="HideHoverChanges();">
															</button>
															<span runat="server"><img src="../../../ima/ico-cancel.svg" class="ico-inactivo"></img></span>
															<span runat="server" class="dateComm">{dateCol}</span>
														</div>

													</div>
                                                        </Html>

                                                    </Template>
                                                </ext:TemplateColumn>
                                            </Columns>
                                        </ColumnModel>

                                        <Items>
                                        </Items>
                                    </ext:GridPanel>



                                </Items>

                            </ext:Container>
                        </Items>
                    </ext:Container>
                    <ext:Panel ID="pnAsideR" runat="server" Hidden="true">
                        <Items>
                            <ext:Label meta:resourceKey="lblHeadNotif" ID="lblHeadNotif" runat="server" IconCls="ico-head-Notif" Text="Head Label Aside" Cls="lblHeadAside"></ext:Label>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Viewport>
        </div>
    </form>
</body>
</html>

