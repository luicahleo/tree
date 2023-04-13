<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionUsuario.aspx.cs" Inherits="TreeCore.PaginasComunes.GestionUsuario" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html>

<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</head>
<body>
    <script type="text/javascript" src="../Scripts/jquery-3.6.0.js"></script>

    <link href="/CSS/croppie.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/tCore.css" rel="stylesheet" type="text/css" />

    <link href="css/styleUsuarios.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Scripts/croppie.js"></script>
    <script type="text/javascript" src="../JS/common.js"></script>
    <script type="text/javascript" src="js/GestionUsuario.js"></script>


    <form runat="server">
        <div>
            <ext:Hidden ID="hdEditarUsuario" runat="server" />
            <ext:Hidden ID="hdMensajeEditarUsuario" runat="server" />
            <ext:Hidden ID="hdYes" runat="server" />
            <ext:Hidden ID="hdMensajePassword" runat="server" />
            <ext:Hidden ID="hdMensajeClaveDiferente" runat="server" />
            <ext:Hidden ID="hdMensajeClaveRepetida" runat="server" />
            <ext:Hidden ID="hdMensajeClaveIncorrecta" runat="server" />
            <ext:Hidden ID="hdSRC" runat="server" />
            <ext:Hidden ID="hdRutaUsuario" runat="server" />

            <ext:Hidden ID="hdGuardar" runat="server" />
            <ext:Hidden ID="hdSeleccionarImagen" runat="server" />
            <ext:Hidden ID="hdTieneImagen" runat="server" />

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
                <Listeners>
                    <DocumentReady Fn="ajaxAsignarImag"></DocumentReady>
                </Listeners>
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>


            <%--INICIO  VIEWPORT --%>

            <ext:Viewport runat="server"
                ID="vwMain"
                Layout="AnchorLayout"
                AnchorVertical="99%"
                AnchorHorizontal="100%"
                Scrollable="Vertical"
                OverflowY="Scroll"
                OverflowX="Hidden">
                <Items>
                    <ext:Container runat="server"
                        ID="titleCont"
                        AnchorVertical="10%"
                        AnchorHorizontal="100%"
                        Padding="30">
                        <Items>
                            <ext:Label runat="server"
                                ID="lblEditarPerfil"
                                Text=""
                                Cls="BigTlt">
                            </ext:Label>
                        </Items>
                    </ext:Container>
                    <ext:Container runat="server"
                        StyleSpec=""
                        Layout="ColumnLayout"
                        AnchorVertical="75%"
                        AnchorHorizontal="99%"
                        Padding="40">
                        <Items>
                            <ext:FormPanel runat="server"
                                HeightSpec="100%"
                                Layout="VBoxLayout"
                                ColumnWidth="0.7"
                                Padding="20">
                                <DockedItems>
                                    <ext:Toolbar runat="server"
                                        ID="Toolbar1"
                                        Dock="Bottom"
                                        PaddingSpec="30px 0 30px 0"
                                        Layout="ColumnLayout">
                                        <Items>
                                            <ext:Label runat="server"
                                                ID="lblFechaClave"
                                                ColumnWidth=".5"
                                                Cls="SpanFloatR">
                                            </ext:Label>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                                <Items>
                                    <ext:Container runat="server" ID="ctImageProfile" Cls="ctImageProfile">
                                        <Items>
                                            <ext:Container runat="server"
                                                Cls="ctImage"
                                                Height="150"
                                                Layout="HBoxLayout"
                                                ColumnWidth="0.3"
                                                MinWidth="150"
                                                MaxWidth="150">
                                                <LayoutConfig>
                                                    <ext:HBoxLayoutConfig Pack="Center">
                                                    </ext:HBoxLayoutConfig>
                                                </LayoutConfig>
                                                <Items>
                                                    <ext:Image runat="server"
                                                        ID="imgUser"
                                                        Height="100%"
                                                        MinWidth="150"
                                                        MaxWidth="150">
                                                    </ext:Image>
                                                </Items>
                                            </ext:Container>
                                            <ext:Container runat="server" Cls="ctAgregarImage">

                                                <Content>
                                            
                                                    <div class="container" id="ContenedorImagen" onclick="TraduccionesJS()" style="display:none">
                                                        <div class="panel panel-default">
                                                            <div class="panel-body">
                                                                <div class="row">
                                                                    <div class="col-md-4 text-center">
                                                                        <div id="uploadImagen" width: 350px; height: 400px;"></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                      <div class="col-md-4">
                                                            <input type="file" id="upload">
                                                            <br />                                                                
                                                      </div>

                                                    <script type="text/javascript">
                                                        $uploadCrop = $('#uploadImagen').croppie({
                                                            enableExif: true,
                                                            viewport: {
                                                                width: 155,
                                                                height: 155,
                                                                type: 'circle'
                                                            },
                                                            boundary: {
                                                                width: 175,
                                                                height: 175
                                                            }
                                                        });


                                                        $('#upload').on('change', function () {
                                                            var reader = new FileReader();
                                                            document.getElementById("ContenedorImagen").style.display = "";
                                                            reader.onload = function (e) {
                                                                $uploadCrop.croppie('bind', {
                                                                    url: e.target.result
                                                                }).then(function () {
                                                                    console.log('jQuery bind complete');
                                                                });
                                                                App.hdTieneImagen.setValue("Si");
                                                            }
                                                            reader.readAsDataURL(this.files[0]);
                                                        });

                                                    </script>

                                                </Content>

                                            </ext:Container>
                                        </Items>
                                    </ext:Container>

                                    <ext:TextField runat="server"
                                        ID="txtName"
                                        FieldLabel="" 
                                        AllowBlank="false"
                                        LabelAlign="Top"
                                        WidthSpec="100%">
                                    </ext:TextField>
                                    <ext:TextField runat="server"
                                        ID="txtApellidos"
                                        FieldLabel=""
                                        AllowBlank="false"
                                        LabelAlign="Top"
                                        WidthSpec="100%">
                                    </ext:TextField>
                                    <ext:TextField runat="server"
                                        ID="txtEmail"
                                        FieldLabel=""
                                        AllowBlank="false"
                                        LabelAlign="Top"
                                        WidthSpec="100%">
                                    </ext:TextField>
                                    <ext:TextField
                                        LabelAlign="Top"
                                        WidthSpec="100%"
                                        ID="txtPasswordField"
                                        runat="server"
                                        AllowBlank="false"
                                        ValidationGroup="FORM2"
                                        EnableKeyEvents="true"
                                        FieldLabel=""
                                        InputType="Password"
                                        Regex="^(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#.;$&amp;*])(?=.*[0-9]).{8,}$"
                                        RegexText="">
                                        <RightButtons>
                                            <ext:Button runat="server"
                                                ID="btnPassword"
                                                Hidden="true"
                                                IconCls="btnFiltroNegativo16"
                                                AllowDepress="true"
                                                EnableToggle="true">
                                                <Listeners>
                                                    <Toggle Handler="this.up('textfield').passwordMask.setMode(pressed ? 'showall' : 'hideall'); this.setTooltip((pressed ? 'Hide' : 'Show') + ' password');" />
                                                </Listeners>
                                            </ext:Button>
                                        </RightButtons>
                                        <Listeners>
                                            <Focus Fn="mandatoryTooltip" />
                                            <FocusLeave Fn="mandatoryTooltip" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:TextField
                                        WidthSpec="100%"
                                        LabelAlign="Top"
                                        ID="txtPasswordConfirm"
                                        runat="server"
                                        FieldLabel=""
                                        AllowBlank="false"
                                        ValidationGroup="FORM2"
                                        EnableKeyEvents="true"
                                        InputType="Password"
                                        Regex="^(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#.;$&amp;*])(?=.*[0-9]).{8,}$"
                                        RegexText="">
                                        <RightButtons>
                                            <ext:Button runat="server"
                                                ID="PassMode"
                                                IconCls="btnFiltroNegativo16"
                                                AllowDepress="true"
                                                EnableToggle="true"
                                                Hidden="true">
                                                <Listeners>
                                                    <Toggle Handler="this.up('textfield').passwordMask.setMode(pressed ? 'showall' : 'hideall'); this.setTooltip((pressed ? 'Hide' : 'Show') + ' password');" />
                                                </Listeners>
                                            </ext:Button>
                                        </RightButtons>
                                        <Listeners>
                                            <Focus Fn="mandatoryTooltip" />
                                            <FocusLeave Fn="mandatoryTooltip" />
                                        </Listeners>
                                    </ext:TextField>
                                    
                                    <%--<ext:Container runat="server" WidthSpec="100%">
                                        <Content>
                                            
                                            <input type="button" id="btnGuardarPerfil" class="btnSave btn-ppal upload-result" style="height: 35px;"></input>

                                            <script type="text/javascript">
                                                $('.upload-result').on('click', function (ev) {
                                                    $uploadCrop.croppie('result', {
                                                        type: 'canvas',
                                                        size: 'viewport'
                                                    }).then(function (resp) {
                                                        App.hdSRC.setValue(resp);
                                                        GestionBotonGuardar();
                                                        if (hdTieneImagen.value != "") {
                                                            html = '<img src="' + resp + '" />';
                                                            document.getElementById("ContenedorImagen").style.display = "none";
                                                            $("#imgUser").html(html);

                                                        }
                                                    });
                                                });
                                            </script>
                                        </Content>
                                    </ext:Container>--%>
                                  </Items>
                                  <DockedItems>
                                    <ext:Toolbar runat="server"
                                        ID="tlbBotones"
                                        Dock="Bottom">
                                        <Items>
                                            <ext:ToolbarFill />
                                            <ext:Button runat="server"
                                                ID="btnGuardar"
                                                Cls="btnSave btn-ppal upload-result"
                                                meta:resourceKey="btnGuardar"
                                                Height="35">
                                                <Listeners>
                                                    <Click Fn="clicBotonGuardar" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </DockedItems>
                            </ext:FormPanel>
                        </Items>
                    </ext:Container>
                </Items>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
