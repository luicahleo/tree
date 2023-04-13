<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="windowModalForm.ascx.cs" Inherits="TreeCore.Componentes.windowModalForm" %>

 <link href="../../../CSS/tCore.css" rel="stylesheet" type="text/css" />
<link href="../../Componentes/css/windowModalForm.css" rel="stylesheet" type="text/css" />
 <link href="https://fonts.googleapis.com/css?family=Roboto:200,400,500,700&display=swap" rel="stylesheet" />
<script type="text/javascript" src="../../Componentes/js/windowModalForm.js"></script>



<ext:Window ID="winGestion"
                runat="server"
                Title="Window Form"
                Width="872"
                Height="500"
                Modal="true"
                Centered="true"
                Cls="winForm-resp"
                Scrollable="Vertical"
                Hidden="false">
                <Listeners>
                    <AfterRender Handler="winFormResize(this);" />
                    
                </Listeners>

                <Items>
                    <ext:Panel runat="server" ID="pnVistasForm" Cls="pnNavVistas pnVistasForm" AriaRole="navigation">
                        <Items>
                            <ext:Container runat="server" ID="cntNavVistasForm" Cls="nav-vistas">
                                <Items>
                                    <ext:HyperlinkButton runat="server" ID="lnkCurrent" Cls="lnk-navView  lnk-noLine navActivo" Text="CURRENTFORM" Handler="showForms(this);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkSubprocess" Cls="lnk-navView  lnk-noLine" Text="FORM1" Handler="showForms(this);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkNext" Cls="lnk-navView  lnk-noLine" Text="FORM2" Handler="showForms(this);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkLinks" Cls="lnk-navView  lnk-noLine" Text="FORM3" Handler="showForms(this);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkNots" Cls="lnk-navView  lnk-noLine" Text="FORM4" Handler="showForms(this);"></ext:HyperlinkButton>
                                    <ext:HyperlinkButton runat="server" ID="lnkDocs" Cls="lnk-navView  lnk-noLine" Text="FORM5" Handler="showForms(this);"></ext:HyperlinkButton>
                                </Items>
                            </ext:Container>
                        </Items>
                    </ext:Panel>
                    <ext:FormPanel ID="FormGestion1" Cls="formGris" runat="server" Hidden="false">
                        <Items>
                            <ext:Container runat="server" ID="ctForm" Cls="ctForm-resp ctForm-resp-col3">
                                <Items>
                                    <ext:TextField runat="server"
                                        ID="txtName"
                                        FieldLabel="Name"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        ID="txtCode"
                                        FieldLabel="Code"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:TextField runat="server"
                                        ID="txtGroup"
                                        FieldLabel="Group"
                                        LabelAlign="Top"
                                        AllowBlank="false"
                                        ValidationGroup="FORM"
                                        CausesValidation="true" />
                                    <ext:ComboBox runat="server"
                                        ID="cmbGlobalState"
                                        FieldLabel="Global State"
                                        LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="Global State 1" />
                                            <ext:ListItem Text="Global State 2" />
                                            <ext:ListItem Text="Global State 3" />
                                            <ext:ListItem Text="Global State 4" />
                                        </Items>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="LimpiarLista"
                                                Icon="Clear"
                                                QTip="Limpiar Lista" />
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                QTip="Recargar Lista" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:ComboBox runat="server"
                                        ID="cmbDepartment"
                                        FieldLabel="Department"
                                        LabelAlign="Top">
                                        <Items>
                                            <ext:ListItem Text="Departament 1" />
                                            <ext:ListItem Text="Departament 2" />
                                            <ext:ListItem Text="Departament 3" />
                                            <ext:ListItem Text="Departament 4" />
                                        </Items>
                                        <Triggers>
                                            <ext:FieldTrigger meta:resourceKey="LimpiarLista"
                                                Icon="Clear"
                                                QTip="Limpiar Lista" />
                                            <ext:FieldTrigger meta:resourceKey="RecargarLista"
                                                IconCls="ico-reload"
                                                QTip="Recargar Lista" />
                                        </Triggers>
                                    </ext:ComboBox>
                                    <ext:NumberField runat="server"
                                        FieldLabel="Red"
                                        LabelAlign="Top"
                                        ID="txtRed"
                                        Width="72" />
                                    <ext:NumberField runat="server"
                                        FieldLabel="Yellow"
                                        LabelAlign="Top"
                                        ID="txtAmarillo"
                                        Width="72" />
                                    <ext:NumberField runat="server"
                                        FieldLabel="Average"
                                        LabelAlign="Top"
                                        ID="txtAverage"
                                        Width="74" />

                                </Items>
                            </ext:Container>
                        </Items>
                     </ext:FormPanel>
                   <ext:ButtonGroup runat="server" Cls="btnWin" >
                        <Items>
                            <ext:Button runat="server" ID="Button1" Cls="btn-secondary" Text="Previous"></ext:Button>
                             <ext:Button runat="server" ID="Button2" Cls="btn-ppal" Text="Next"></ext:Button>
                        </Items>
                   </ext:ButtonGroup>
                </Items>

            </ext:Window>