<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecuperarContra.aspx.cs" Inherits="TreeCore.PaginasComunes.RecuperarContra" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../ima/generic.css" rel="stylesheet" type="text/css" />

    <!--<script type="text/javascript" src="js/common.js"></script>-->

    <script type="text/javascript" src="js/RecuperarContra.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%-- INICIO HIDDEN --%>

            <ext:Hidden ID="complejidad" runat="server" Text="50" />
            <ext:Hidden ID="jsClaveValidacionMsg1" runat="server" />
            <ext:Hidden ID="jsClaveValidacionMsg2" runat="server" />
            <ext:Hidden ID="jsClaveNoCorrespondencia" runat="server" />
            <ext:Hidden ID="jsTituloAtencion" runat="server" />

            <%-- FIN HIDDEN --%>

            <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="TreeCore" />
            
            <%-- INICIO WINDOWS --%>

            <ext:Window ID="winCambiarClave" runat="server" Title="Agregar" Width="400" AutoHeight="true" Collapsible="false"
                Modal="true" Resizable="false" ShowOnLoad="false" Hidden="false" meta:resourceKey="winCambiarClave" >
                <Items>
                    <ext:FormPanel ID="formClave" runat="server" LabelWidth="100" LabelAlign="Top" BodyStyle="padding:10px;" Border="false"
                        MonitorPoll="500" MonitorValid="true" >
                        <Items>
                            <ext:TextField ID="txtCambiarClave" runat="server" FieldLabel="Clave" Text="" InputType="Password" MaxLength="50"
                                MaxLengthText="Ha superado el máximo número de caracteres" AllowBlank="false" ValidationGroup="FORM2" EnableKeyEvents="true"
                                meta:resourceKey="txtCambiarClave" >
                                <Listeners>
                                    <Render Fn="PasswordField_Render" />
                                    <KeyUp Fn="PasswordField_KeyUp" />
                                    <Focus Fn="PasswordField_Focus" />
                                    <Blur Fn="PasswordField_Blur" />
                                    <Resize Fn="PasswordField_Resize" />
                                    <Disable Fn="PasswordField_KeyUp1" />
                                    <Enable Fn="PasswordField_KeyUp1" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField ID="txtCambiarClave2" runat="server" FieldLabel="Confirmar Clave" Text="" InputType="Password" MaxLength="50"
                                MaxLengthText="Ha superado el máximo número de caracteres" AllowBlank="false" ValidationGroup="FORM2" EnableKeyEvents="true"
                                meta:resourceKey="txtCambiarClave2" />
                        </Items>
                        <Listeners>
                            <ValidityChange Handler="FormularioValidoClave(valid);" />
                        </Listeners>
                    </ext:FormPanel>
                </Items>
                <Buttons>
                    <ext:Button ID="btnCambiar" runat="server" Text="Cambiar" Disabled="true" Icon="Accept" meta:resourceKey="btnCambiar" >
                        <Listeners>
                            <Click Handler="winCambiarClaveBotonCambiar();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <Listeners>
                    <Hide Handler="OcultarCambiarClave()" />
                </Listeners>
            </ext:Window>

            <%-- FIN WINDOWS --%>
        </div>
    </form>
</body>
</html>
