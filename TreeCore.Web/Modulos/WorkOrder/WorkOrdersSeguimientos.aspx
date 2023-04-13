<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrdersSeguimientos.aspx.cs" Inherits="TreeCore.ModWorkOrders.WorkOrdersSeguimientos" %>

<!DOCTYPE html>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <script src="/JS/dropzone.js"></script>
    <link href="/CSS/dropzone.css" rel="stylesheet" />

    <form id="form1" runat="server">
        <div>
            <%--INICIO HIDDEN --%>
             
            <ext:Hidden ID="hdCliID" runat="server" />
            <ext:Hidden ID="hdUsuarioID" runat="server" />
            <ext:Hidden runat="server" ID="hdWorkOrderID" />
            <ext:Hidden runat="server" ID="hdEstadoActID" />
            <ext:Hidden runat="server" ID="hdTipoVista" />
            <ext:Hidden runat="server" ID="hdEstadoSiguienteID" />
            <ext:Hidden runat="server" ID="hdSegSeleccionadoID" />
            <ext:Hidden runat="server" ID="hdSegPadreID" />
            <ext:Hidden runat="server" ID="hdCulture" />

            <%--FIN HIDDEN --%>

            <%--INICIO  RESOURCEMANAGER --%>

            <ext:ResourceManager
                runat="server"
                ID="ResourceManagerTreeCore"
                DirectMethodNamespace="TreeCore">
            </ext:ResourceManager>

            <%--FIN  RESOURCEMANAGER --%>

            <%--INICIO  STORES --%>

            <ext:Store runat="server"
                ID="storeTareas"
                AutoLoad="true"
                OnReadData="storeTareas_Refresh"
                RemoteSort="false">
                <proxy>
                    <ext:PageProxy />
                </proxy>
                <model>
                    <ext:Model runat="server"
                        IDProperty="TareasID">
                        <fields>
                            <ext:ModelField Name="TareasID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Pagina" />
                            <ext:ModelField Name="Hecho" />
                        </fields>
                    </ext:Model>
                </model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeWorkOrderEstados"
                AutoLoad="true"
                OnReadData="storeWorkOrderEstados_Refresh"
                RemoteSort="false">
                <proxy>
                    <ext:PageProxy />
                </proxy>
                <model>
                    <ext:Model runat="server"
                        IDProperty="TareasID">
                        <fields>
                            <ext:ModelField Name="CoreWorkOrderEstadoID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Codigo" />
                            <ext:ModelField Name="Color" />
                        </fields>
                    </ext:Model>
                </model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeSeguimientos"
                AutoLoad="true"
                OnReadData="storeSeguimientos_Refresh"
                RemoteSort="false">
                <proxy>
                    <ext:PageProxy />
                </proxy>
                <model>
                    <ext:Model runat="server"
                        IDProperty="EstadoID">
                        <fields>
                            <ext:ModelField Name="EstadoID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Seguimientos" Type="Object" />
                        </fields>
                    </ext:Model>
                </model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeSeguimientosAnteriores"
                AutoLoad="true"
                OnReadData="storeSeguimientosAnteriores_Refresh"
                RemoteSort="false">
                <proxy>
                    <ext:PageProxy />
                </proxy>
                <model>
                    <ext:Model runat="server"
                        IDProperty="EstadoID">
                        <fields>
                            <ext:ModelField Name="EstadoID" Type="Int" />
                            <ext:ModelField Name="Nombre" />
                            <ext:ModelField Name="Seguimientos" Type="Object" />
                        </fields>
                    </ext:Model>
                </model>
            </ext:Store>

            <ext:Store runat="server"
                ID="storeTiposDocumentos"
                AutoLoad="true"
                OnReadData="storeTiposDocumentos_Refresh"
                RemoteSort="false">
                <proxy>
                    <ext:PageProxy />
                </proxy>
                <model>
                    <ext:Model runat="server"
                        IDProperty="DocumentTipoID">
                        <fields>
                            <ext:ModelField Name="DocumentTipoID" Type="Int" />
                            <ext:ModelField Name="DocumentTipo" />
                            <ext:ModelField Name="Extensiones" />
                        </fields>
                    </ext:Model>
                </model>
            </ext:Store>

            <%--FIN  STORES --%>

            <%--INICIO  WINDOWS --%>

            <ext:Window runat="server"
                ID="winSiguienteEstado"
                meta:resourcekey="winGestion"
                Title="<%$ Resources:Comun, jsComentario %>"
                Width="400"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <items>
                    <ext:FormPanel runat="server"
                        ID="formSiguienteEstado"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <items>
                            <ext:TextArea
                                runat="server"
                                ID="txtComentarioSiguienteEstado"
                                WidthSpec="100%"
                                meta:resourceKey="SiguienteEstado"
                                FieldLabel="<%$ Resources:Comun, jsComentario %>"
                                MaxLength="400"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                LabelAlign="Top"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                        </items>
                        <listeners>
                            <validitychange handler="FormularioSiguienteEstadoValido(valid);" />
                        </listeners>
                    </ext:FormPanel>
                </items>
                <buttons>
                    <ext:Button runat="server"
                        ID="btnCancelarSiguienteEstado"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <listeners>
                            <click handler="#{winSiguienteEstado}.hide();" />
                        </listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardarSiguienteEstado"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <listeners>
                            <click handler="winSiguienteEstadoBotonGuardar();" />
                        </listeners>
                    </ext:Button>
                </buttons>
            </ext:Window>

            <ext:Window runat="server"
                ID="winReasignar"
                meta:resourcekey="winGestion"
                Title="<%$ Resources:Comun, strReasignarUsuario %>"
                Width="400"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <items>
                    <ext:FormPanel runat="server"
                        ID="formReasignar"
                        BodyStyle="padding:10px;"
                        Border="false">
                        <items>
                            <ext:ComboBox runat="server"
                                ID="cmbUsuariosReasignar"
                                meta:resourceKey="cmbUsuarios"
                                DisplayField="NombreCompleto"
                                ValueField="UsuarioID"
                                Cls="comboGrid"
                                QueryMode="Local"
                                WidthSpec="100%"
                                Hidden="false"
                                LabelAlign="Top"
                                FieldLabel="<%$ Resources:Comun, strUsuario %>">
                                <store>
                                    <ext:Store runat="server"
                                        ID="storeUsuariosReasignar"
                                        RemotePaging="false"
                                        AutoLoad="false"
                                        OnReadData="storeUsuariosReasignar_Refresh"
                                        RemoteSort="false"
                                        PageSize="20"
                                        RemoteFilter="false">
                                        <proxy>
                                            <ext:PageProxy />
                                        </proxy>
                                        <model>
                                            <ext:Model runat="server" IDProperty="UsuarioID">
                                                <fields>
                                                    <ext:ModelField Name="UsuarioID" Type="Int" />
                                                    <ext:ModelField Name="NombreCompleto" />
                                                </fields>
                                            </ext:Model>
                                        </model>
                                    </ext:Store>
                                </store>
                                <listeners>
                                    <select fn="SeleccionarCombo" />
                                    <triggerclick fn="RecargarCombo" />
                                </listeners>
                                <triggers>
                                    <ext:FieldTrigger
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        Weight="-1"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                </triggers>
                            </ext:ComboBox>
                            <ext:TextArea
                                runat="server"
                                ID="txtComentarioReasignar"
                                WidthSpec="100%"
                                meta:resourceKey="SiguienteEstado"
                                FieldLabel="<%$ Resources:Comun, jsComentario %>"
                                MaxLength="400"
                                MaxLengthText="<%$ Resources:Comun, strMaxLengthText %>"
                                AllowBlank="false"
                                LabelAlign="Top"
                                ValidationGroup="FORM"
                                CausesValidation="true" />
                        </items>
                        <listeners>
                            <validitychange handler="FormularioReasignarValido(valid);" />
                        </listeners>
                    </ext:FormPanel>
                </items>
                <buttons>
                    <ext:Button runat="server"
                        ID="btnCancelarReasignar"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <listeners>
                            <click handler="#{winSiguienteEstado}.hide();" />
                        </listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardarReasignar"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <listeners>
                            <click handler="winReasignarBotonGuardar();" />
                        </listeners>
                    </ext:Button>
                </buttons>
            </ext:Window>

            <ext:Window runat="server"
                ID="winDocumentos"
                meta:resourcekey="winDocumentos"
                Title="<%$ Resources:Comun, winGestion.Title %>"
                Width="400"
                Height="450"
                Resizable="false"
                Modal="true"
                Hidden="true">
                <items>
                    <ext:Container runat="server"
                        PaddingSpec="10px 15px">
                        <items>
                            <ext:TextArea
                                runat="server"
                                ID="txtComentarioDocumento"
                                WidthSpec="100%"
                                MaxLength="1000"
                                Cls="textAreaGridFinal"
                                LabelAlign="Top"
                                AllowBlank="false"
                                FieldLabel="<%$ Resources:Comun, jsComentario %>">
                                <listeners>
                                    <validitychange fn="ValidarSubirDocumentos" />
                                </listeners>
                            </ext:TextArea>
                            <ext:ComboBox runat="server"
                                ID="cmbTiposDocumentos"
                                StoreID="storeTiposDocumentos"
                                DisplayField="DocumentTipo"
                                ValueField="DocumentTipoID"
                                WidthSpec="100%"
                                Cls=""
                                LabelAlign="Top"
                                EmptyText="<%$ Resources:Comun, jsDocumentoTipo %>"
                                FieldLabel="<%$ Resources:Comun, jsDocumentoTipo %>"
                                AllowBlank="false">
                                <listeners>
                                    <select fn="SeleccionarTipoDocumento" />
                                    <triggerclick fn="RecargarTipoDocumento" />
                                </listeners>
                                <triggers>
                                    <ext:FieldTrigger
                                        IconCls="ico-reload"
                                        Hidden="true"
                                        QTip="<%$ Resources:Comun, strRecargarLista %>"
                                        Weight="-1" />
                                </triggers>
                            </ext:ComboBox>
                        </items>
                        <content>
                            <div id="dZUpload" class="dropzone dropzoneSeguimiento" action="/file-upload">
                                <div class="dz-default dz-message"></div>
                            </div>
                            <div class="hidden">
                                <div class="dz-preview dz-file-preview well" id="dz-preview-template">
                                    <div class="dz-details">
                                        <div class="hidden imaIco btnformViewMandatory"></div>
                                        <div class="imaIco btndocs16"></div>
                                        <div class="dz-filename"><span data-dz-name></span></div>
                                        <a class="dz-remove" href="javascript:undefined;" data-dz-remove="">
                                            <div class="ico-close"></div>
                                        </a>
                                    </div>
                                    <div class="dz-progress"><span class="dz-upload" data-dz-uploadprogress></span></div>
                                    <div class="dz-success-mark"><span></span></div>
                                    <div class="dz-error-mark"><span></span></div>
                                    <div class="dz-error-message"><span data-dz-errormessage></span></div>
                                </div>
                            </div>
                        </content>
                    </ext:Container>
                </items>
                <buttons>
                    <ext:Button runat="server"
                        ID="btnCancelarDocumentos"
                        Text="<%$ Resources:Comun, btnCancelar.Text %>"
                        IconCls="ico-cancel"
                        Cls="btn-cancel">
                        <listeners>
                            <click handler="#{winDocumentos}.hide();" />
                        </listeners>
                    </ext:Button>
                    <ext:Button runat="server"
                        ID="btnGuardarDocumentos"
                        Text="<%$ Resources:Comun, btnGuardar.Text %>"
                        IconCls="ico-accept"
                        Cls="btn-accept"
                        Disabled="true">
                        <listeners>
                            <click handler="GuardarDocumentos();" />
                        </listeners>
                    </ext:Button>
                </buttons>
            </ext:Window>

            <%--FIN  WINDOWS --%>

            <%--INICIO  VIEWPORT --%>

            <ext:Viewport
                runat="server"
                ID="vwContenedor"
                Cls="vwContenedor contenedorSeguimiento"
                Scrollable="Vertical"
                Layout="Border">
                <items>
                    <ext:Button runat="server" ID="btnPanelLateral" Cls="btn-trans btnPanelLateral btnCollapseAsRClosedv2 btnRotate" Hidden="false"
                        ToolTip="<%$ Resources:Comun, strAbrirMenu %>">
                        <listeners>
                            <click fn="hidePn" />
                        </listeners>
                    </ext:Button>
                    <ext:Container runat="server" Region="Center">
                        <items>
                            <ext:Toolbar runat="server"
                                ID="tlbBotonera"
                                Dock="Top">
                                <items>
                                    <ext:Button runat="server"
                                        ID="btnSiguienteEstado"
                                        ToolTip="<%$ Resources:Comun, strEstadosSiguientes %>"
                                        Cls="btnArrowRight-subM">
                                        <menu>
                                            <ext:Menu runat="server" ID="mnuSiguienteEstado">
                                            </ext:Menu>
                                        </menu>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnReasignar"
                                        ToolTip="<%$ Resources:Comun, strReasignarUsuario %>"
                                        Cls="btnArrowUser">
                                        <listeners>
                                            <click fn="ReasignarUsuario" />
                                        </listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnRefrescar"
                                        ToolTip="<%$ Resources:Comun, btnRefrescar.ToolTip %>"
                                        Cls="btnRefrescar">
                                        <listeners>
                                            <click fn="Refrescar" />
                                        </listeners>
                                    </ext:Button>
                                    <ext:Button runat="server"
                                        ID="btnInfo"
                                        ToolTip="<%$ Resources:Comun, jsInfo %>"
                                        Cls="btnInfo">
                                        <listeners>
                                            <click fn="CargarInfo" />
                                        </listeners>
                                    </ext:Button>
                                </items>
                            </ext:Toolbar>
                            <ext:Toolbar runat="server"
                                ID="tlbSeguimiento">
                                <items>
                                    <ext:Label runat="server"
                                        ID="lblNombreTipologia"
                                        Text="Tipologia"
                                        Cls="spanLbl tituloGris">
                                    </ext:Label>
                                </items>
                            </ext:Toolbar>
                            <ext:Container ID="contGeneral" runat="server" Flex="1" Cls="contenedorBlanco">
                                <items>
                                    <ext:Panel runat="server"
                                        ID="pnlSuperior"
                                        Cls="contenedorChecklist"
                                        Hidden="false"
                                        Layout="HBoxLayout">
                                        <layoutconfig>
                                            <ext:HBoxLayoutConfig Align="Stretch" />
                                        </layoutconfig>
                                        <listeners>
                                            <afterrender handler="this.setMinHeight((window.innerHeight/100)*60);" />
                                        </listeners>
                                        <items>
                                            <ext:Panel runat="server"
                                                ID="pnlDescripcionSeguimiento"
                                                Cls="panelPrincipal"
                                                Scrollable="Vertical"
                                                Flex="7">
                                                <dockeditems>
                                                    <ext:Toolbar runat="server"
                                                        ID="tlbTituloSeguimiento"
                                                        Dock="Top">
                                                        <items>
                                                            <ext:Image runat="server"
                                                                ID="imgUsuarioActual"
                                                                Cls="imgRed"
                                                                Width="45"
                                                                Src="/ima/LOGOAtrebo.png"
                                                                Height="45">
                                                            </ext:Image>
                                                            <ext:Container runat="server"
                                                                ID="cntTituloSeguimiento"
                                                                Cls="contenedorDobleLabel">
                                                                <items>
                                                                    <ext:Label runat="server"
                                                                        ID="lblNombreUsuarioActual"
                                                                        Text="Mariano"
                                                                        Cls="titulo">
                                                                    </ext:Label>
                                                                    <ext:Label runat="server"
                                                                        ID="lblEquipoUsuarioActual"
                                                                        Text="Manchester"
                                                                        Cls="subtitulo">
                                                                    </ext:Label>
                                                                </items>
                                                            </ext:Container>
                                                            <ext:ToolbarFill></ext:ToolbarFill>
                                                            <ext:ComboBox runat="server"
                                                                ID="cmbEstadosWorkOrder"
                                                                Editable="false"
                                                                ValueField="CoreWorkOrderEstadoID"
                                                                DisplayField="Nombre"
                                                                ForceSelection="true"
                                                                StoreID="storeWorkOrderEstados"
                                                                Cls="cmbEstadosWorkOrder"
                                                                Width="100">
                                                                <listconfig>
                                                                    <itemtpl runat="server">
                                                                        <html>
                                                                        <div class="icon-combo-item item-cmbEstadosWorkOrder" style="background-color: {Color}">
                                                                            <div class="texto">
                                                                                {Nombre}
                                                                            </div>
                                                                        </div>
                                                                        </html>
                                                                    </itemtpl>
                                                                </listconfig>
                                                                <listeners>
                                                                    <select fn="SeleccionarEstadoWorkOrder" />
                                                                    <change fn="CambiarEstadoWorkOrder" />
                                                                </listeners>
                                                            </ext:ComboBox>
                                                        </items>
                                                    </ext:Toolbar>
                                                </dockeditems>
                                                <items>
                                                    <ext:Container runat="server"
                                                        Cls="contenedorPadding">
                                                        <items>
                                                            <ext:Label runat="server"
                                                                ID="lblFechaSeguimiento"
                                                                Text="25/02/2021"
                                                                Cls="lblGreenFormDQ lblFecha">
                                                            </ext:Label>
                                                            <ext:Label runat="server"
                                                                ID="lblEstadoActual"
                                                                Text="Pendiente"
                                                                Cls="TituloCabecera lblTitulo">
                                                            </ext:Label>
                                                            <ext:Label runat="server"
                                                                ID="lblDescripcionEstadoActual"
                                                                Text="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed eu nulla eu mauris rutrum placerat at id mi. Curabitur in dolor pulvinar, auctor diam ut, suscipit tortor. Etiam non semper massa, a scelerisque mi. Vestibulum ut ligula dapibus, facilisis urna in, blandit ligula."
                                                                Cls="fGrey lblTexto">
                                                            </ext:Label>
                                                            <ext:Label runat="server"
                                                                ID="lblFaseSeguimiento"
                                                                Text="Instalacion"
                                                                Hidden="true"
                                                                Cls="lblInfo">
                                                            </ext:Label>
                                                            <ext:Label runat="server"
                                                                ID="lblModuloSeguimiento"
                                                                Text="Instalacion"
                                                                Hidden="true"
                                                                Cls="lblInfo">
                                                            </ext:Label>
                                                        </items>
                                                    </ext:Container>
                                                </items>
                                            </ext:Panel>
                                            <ext:Panel runat="server"
                                                Flex="5"
                                                Scrollable="Vertical"
                                                Cls="panelCheckList">
                                                <items>
                                                    <ext:GridPanel runat="server"
                                                        ID="gridTareasSeguimiento"
                                                        StoreID="storeTareas">
                                                        <listeners>
                                                            <afterlayout fn="SetMaxHeightSuperior"></afterlayout>
                                                        </listeners>
                                                        <dockeditems>
                                                            <ext:Toolbar runat="server"
                                                                ID="tlbTituloTareas"
                                                                Cls="tlbTituloCheckList"
                                                                Dock="Top">
                                                                <items>
                                                                    <ext:Label runat="server"
                                                                        ID="lblTituloTareas"
                                                                        Cls="lblTitulo"
                                                                        IconCls="btn-ok-blanco"
                                                                        Text="My Task">
                                                                    </ext:Label>
                                                                </items>
                                                            </ext:Toolbar>
                                                        </dockeditems>
                                                        <columnmodel>
                                                            <columns>
                                                                <ext:TemplateColumn runat="server"
                                                                    ID="coltemplatetareas"
                                                                    Flex="1">
                                                                    <template runat="server">
                                                                        <html>
                                                                        <tpl for=".">
                                                                            <div class="contenedorTarea">
                                                                                <input type="checkbox" id="chkDone" class="chkDone" {Hecho}>
                                                                                <label class="lblNombreTarea">
                                                                                    {Nombre}
                                                                                </label>
                                                                                <a class="btnLaunch btnTarea"></a>
                                                                            </div>
                                                                        </tpl>
                                                                        </html>
                                                                    </template>
                                                                </ext:TemplateColumn>
                                                            </columns>
                                                        </columnmodel>
                                                    </ext:GridPanel>
                                                </items>
                                            </ext:Panel>
                                        </items>
                                    </ext:Panel>
                                    <ext:Panel runat="server"
                                        ID="pnlInferior"
                                        Cls="contenedorSeguimientos">
                                        <dockeditems>
                                            <ext:Toolbar runat="server"
                                                ID="tlbFiltosSeguimientos"
                                                Dock="Top">
                                                <items>
                                                    <ext:ToolbarFill></ext:ToolbarFill>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="btnTodos"
                                                        Text="<%$ Resources:Comun, strTodos %>"
                                                        EnableToggle="true"
                                                        Cls="lnk-navView lnk-noLine">
                                                        <listeners>
                                                            <click handler="SeleccionarFiltro(this, 'Todo')" />
                                                        </listeners>
                                                    </ext:HyperlinkButton>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="btnDocumentos"
                                                        Text="<%$ Resources:Comun, strDocumentos %>"
                                                        EnableToggle="true"
                                                        Cls="lnk-navView lnk-noLine">
                                                        <listeners>
                                                            <click handler="SeleccionarFiltro(this, 'Documentos')" />
                                                        </listeners>
                                                    </ext:HyperlinkButton>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="btnCambios"
                                                        Text="<%$ Resources:Comun, strCambios %>"
                                                        EnableToggle="true"
                                                        Cls="lnk-navView lnk-noLine">
                                                        <listeners>
                                                            <click handler="SeleccionarFiltro(this, 'Cambios')" />
                                                        </listeners>
                                                    </ext:HyperlinkButton>
                                                    <ext:HyperlinkButton runat="server"
                                                        ID="btnComentarios"
                                                        Text="<%$ Resources:Comun, strComentarios %>"
                                                        EnableToggle="true"
                                                        Cls="lnk-navView lnk-noLine">
                                                        <listeners>
                                                            <click handler="SeleccionarFiltro(this, 'Comentarios')" />
                                                        </listeners>
                                                    </ext:HyperlinkButton>
                                                </items>
                                            </ext:Toolbar>
                                        </dockeditems>
                                        <items>
                                            <ext:Panel runat="server"
                                                ID="pnlSeguimientosEstadoActual"
                                                Cls="pnlSeguimientosEstadoActual"
                                                Hidden="false">
                                                <dockeditems>
                                                    <ext:Toolbar runat="server" Dock="Top">
                                                        <items>
                                                            <ext:Button runat="server"
                                                                ID="btnEstadoActual"
                                                                Text="Pendiente"
                                                                Cls="btnEstadoActual">
                                                                <listeners>
                                                                    <click fn="BtnEstadoActual"></click>
                                                                </listeners>
                                                            </ext:Button>
                                                        </items>
                                                    </ext:Toolbar>
                                                </dockeditems>
                                                <items>
                                                    <ext:Panel runat="server"
                                                        ID="pnlseguimientosActual"
                                                        Cls="pnlSeguimientosActual">
                                                        <dockeditems>
                                                            <ext:Toolbar runat="server" Dock="Top">
                                                                <items>
                                                                    <ext:TextArea runat="server"
                                                                        ID="txtaComentario"
                                                                        Cls="txtaComentario"
                                                                        EmptyText="<%$ Resources:Comun, jsComentario %>">
                                                                    </ext:TextArea>
                                                                </items>
                                                            </ext:Toolbar>
                                                            <ext:Toolbar runat="server" Dock="Top" ID="toolbarDocumentos">
                                                            </ext:Toolbar>
                                                            <ext:Toolbar runat="server" Dock="Top">
                                                                <items>
                                                                    <ext:ToolbarFill></ext:ToolbarFill>
                                                                    <ext:Button runat="server"
                                                                        Cls="bntContentUpload">
                                                                        <listeners>
                                                                            <click fn="SubirDocumentos" />
                                                                        </listeners>
                                                                    </ext:Button>
                                                                    <ext:Button runat="server"
                                                                        Text="<%$ Resources:Comun, strAnadir %>"
                                                                        Cls="botonGuardarComentarioActual"
                                                                        IconCls="btn-add-plus-gr">
                                                                        <listeners>
                                                                            <click fn="GuardarSeguimiento" />
                                                                        </listeners>
                                                                    </ext:Button>
                                                                </items>
                                                            </ext:Toolbar>
                                                        </dockeditems>
                                                        <items>
                                                            <ext:DataView
                                                                runat="server"
                                                                ID="DataView1"
                                                                SingleSelect="true"
                                                                ItemSelector="div.estado"
                                                                StoreID="storeSeguimientos">
                                                                <tpl runat="server">
                                                                    <html>
                                                                    <div id="items-ct">
                                                                        <tpl for=".">
                                                                            <div class="estado">
                                                                                <div class="contSeguimientos">
                                                                                    <tpl for="Seguimientos">
                                                                                        <div class="contItem" segid="{SeguimientoID}">
                                                                                            <div class="contSeguimiento" tipo="{Tipo}">
                                                                                                <div class="contSeguimientoUsuario">
                                                                                                    <div class="imgseguimientoUsuario" style="background-image: url({Imagen})"></div>
                                                                                                    <div class="contSeguimientoNombreUsuario">
                                                                                                        <p class="nombreUsuario">{Nombre}</p>
                                                                                                        <p class="subnombreUsuario">{Proyecto}</p>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="contSeguimientoInfo">
                                                                                                    <p class="fechaSeguimiento lblGreenFormDQ">{Fecha}</p>
                                                                                                    <p class="descripcionSeguimiento fGrey">{Comentarios}</p>
                                                                                                </div>
                                                                                                <div class="contGestionSeguimiento" segid="{SeguimientoID}">
                                                                                                    <div class="btnGestionSeguimiento btnEditar" onclick="activarEditar(this)"></div>
                                                                                                    <div class="btnGestionSeguimiento btnEliminar" onclick="EliminarSeguimiento(this)"></div>
                                                                                                    <div class="btnGestionSeguimiento bntContentUpload" onclick="SubirNuevosDocumentos(this)"></div>
                                                                                                    <div class="btnGestionSeguimiento btnGuardar btn-ok-blanco" onclick="EditarSeguimiento(this)"></div>
                                                                                                    <div class="btnGestionSeguimiento btnCancelar btn-close-blanco" onclick="CancelarEditar(this)"></div>
                                                                                                </div>
                                                                                                <div class="contDocumentos">
                                                                                                    <tpl for="Documentos">
                                                                                                        <div class="contDocumento">
                                                                                                            <div class="icoDocumento" documento="{Nombre}"></div>
                                                                                                            <div class="contNombreDocumento">
                                                                                                                <p class="nombreDocumento">{Nombre}</p>
                                                                                                            </div>
                                                                                                            <div class="btnEliminarDocumento" documentoid="{DocumentoID}" onclick="EliminarDocumento(this)"></div>
                                                                                                        </div>
                                                                                                    </tpl>
                                                                                                </div>
                                                                                                <div class="contEditado" editado="{Editado}">
                                                                                                    <p class="lblEdiatado">Edit</p>
                                                                                                </div>
                                                                                                <div class="btnComentario btnAddComment noselect" onclick="mostrarComentarios(this)">
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="contSubSeguimiento hidden">
                                                                                                <tpl for="SubSeguimientos">
                                                                                                    <div class="contSeguimiento" tipo="{Tipo}">
                                                                                                        <div class="contSeguimientoUsuario">
                                                                                                            <div class="imgseguimientoUsuario" style="background-image: url({Imagen})"></div>
                                                                                                            <div class="contSeguimientoNombreUsuario">
                                                                                                                <p class="nombreUsuario">{Nombre}</p>
                                                                                                                <p class="subnombreUsuario">{Proyecto}</p>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div class="contSeguimientoInfo">
                                                                                                            <p class="fechaSeguimiento lblGreenFormDQ">{Fecha}</p>
                                                                                                            <p class="descripcionSeguimiento fGrey">{Comentarios}</p>
                                                                                                        </div>
                                                                                                        <div class="contGestionSeguimiento" segid="{SeguimientoID}">
                                                                                                            <div class="btnGestionSeguimiento btnEditar" onclick="activarEditar(this)"></div>
                                                                                                            <div class="btnGestionSeguimiento btnEliminar" onclick="EliminarSeguimiento(this)"></div>
                                                                                                            <div class="btnGestionSeguimiento bntContentUpload" onclick="SubirNuevosDocumentos(this)"></div>
                                                                                                            <div class="btnGestionSeguimiento btnGuardar btn-ok-blanco" onclick="EditarSeguimiento(this)"></div>
                                                                                                            <div class="btnGestionSeguimiento btnCancelar btn-close-blanco" onclick="CancelarEditar(this)"></div>
                                                                                                        </div>
                                                                                                        <div class="contDocumentos">
                                                                                                            <tpl for="Documentos">
                                                                                                                <div class="contDocumento">
                                                                                                                    <div class="icoDocumento" documento="{Nombre}"></div>
                                                                                                                    <div class="contNombreDocumento">
                                                                                                                        <p class="nombreDocumento">{Nombre}</p>
                                                                                                                    </div>
                                                                                                                    <div class="btnEliminarDocumento" documentoid="{DocumentoID}" onclick="EliminarDocumento(this)"></div>
                                                                                                                </div>
                                                                                                            </tpl>
                                                                                                        </div>
                                                                                                        <div class="contEditado" editado="{Editado}">
                                                                                                            <p class="lblEdiatado">Edit</p>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </tpl>
                                                                                                <div class="contRespSeguimiento">
                                                                                                    <div class="contPanelTexto">
                                                                                                        <textarea class="panelTextoResp">

                                                                                                </textarea>
                                                                                                    </div>
                                                                                                    <div class="contToolbar">
                                                                                                        <div class="contDocumentos">
                                                                                                        </div>
                                                                                                        <div class="btnRespSeguimientos btnAgregarRespuesta" onclick="GuardarRespuesta(this)" segid="{SeguimientoID}"></div>
                                                                                                        <div class="btnRespSeguimientos bntContentUpload" onclick="SubirDocumentoRespuesta(this)" segid="{SeguimientoID}"></div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </tpl>
                                                                                </div>
                                                                            </div>
                                                                        </tpl>
                                                                    </div>
                                                                    </html>
                                                                </tpl>
                                                            </ext:DataView>
                                                        </items>
                                                    </ext:Panel>
                                                </items>
                                            </ext:Panel>
                                            <ext:Panel runat="server"
                                                ID="pnlSeguimientosEstadoAnterior"
                                                Cls="pnlSeguimientosEstadoAnterior">
                                                <items>
                                                    <ext:DataView
                                                        runat="server"
                                                        ID="dtvEstadosAnteriores"
                                                        SingleSelect="true"
                                                        ItemSelector="div.estado"
                                                        StoreID="storeSeguimientosAnteriores">
                                                        <tpl runat="server">
                                                            <html>
                                                            <div id="items-ct">
                                                                <tpl for=".">
                                                                    <div class="estado">
                                                                        <div class="btnEstados noselect" estadoid="{EstadoID}" onclick="mostrarSeguimientos(this)">
                                                                            <span class="lblNombreEstado">{Nombre}
                                                                            </span>
                                                                        </div>
                                                                        <div class="contSeguimientos hidden seguimientosAntiguos">
                                                                            <tpl for="Seguimientos">
                                                                                <div class="contItem">
                                                                                    <div class="contSeguimiento" tipo="{Tipo}">
                                                                                        <div class="contSeguimientoUsuario">
                                                                                            <div class="imgseguimientoUsuario" style="background-image: url({Imagen})"></div>
                                                                                            <div class="contSeguimientoNombreUsuario">
                                                                                                <p class="nombreUsuario">{Nombre}</p>
                                                                                                <p class="subnombreUsuario">{Proyecto}</p>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="contSeguimientoInfo">
                                                                                            <p class="fechaSeguimiento lblGreenFormDQ">{Fecha}</p>
                                                                                            <p class="descripcionSeguimiento fGrey">{Comentarios}</p>
                                                                                        </div>
                                                                                        <div class="contDocumentos">
                                                                                            <tpl for="Documentos">
                                                                                                <div class="contDocumento">
                                                                                                    <div class="icoDocumento" documento="{Nombre}"></div>
                                                                                                    <div class="contNombreDocumento">
                                                                                                        <p class="nombreDocumento">{Nombre}</p>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </tpl>
                                                                                        </div>
                                                                                        <div class="contEditado" editado="{Editado}">
                                                                                            <p class="lblEdiatado">Edit</p>
                                                                                        </div>
                                                                                        <div class="btnComentario btnAddComment noselect" onclick="mostrarComentarios(this)">
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="contSubSeguimiento hidden">
                                                                                        <tpl for="SubSeguimientos">
                                                                                            <div class="contSeguimiento" tipo="{Tipo}">
                                                                                                <div class="contSeguimientoUsuario">
                                                                                                    <div class="imgseguimientoUsuario" style="background-image: url({Imagen})"></div>
                                                                                                    <div class="contSeguimientoNombreUsuario">
                                                                                                        <p class="nombreUsuario">{Nombre}</p>
                                                                                                        <p class="subnombreUsuario">{Proyecto}</p>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="contSeguimientoInfo">
                                                                                                    <p class="fechaSeguimiento lblGreenFormDQ">{Fecha}</p>
                                                                                                    <p class="descripcionSeguimiento fGrey">{Comentarios}</p>
                                                                                                </div>
                                                                                                <div class="contDocumentos">
                                                                                                    <tpl for="Documentos">
                                                                                                        <div class="contDocumento">
                                                                                                            <div class="icoDocumento" documento="{Nombre}"></div>
                                                                                                            <div class="contNombreDocumento">
                                                                                                                <p class="nombreDocumento">{Nombre}</p>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </tpl>
                                                                                                </div>
                                                                                                <div class="contEditado" editado="{Editado}">
                                                                                                    <p class="lblEdiatado">Edit</p>
                                                                                                </div>
                                                                                            </div>
                                                                                        </tpl>
                                                                                    </div>
                                                                            </tpl>
                                                                        </div>
                                                                    </div>
                                                                </tpl>
                                                            </div>
                                                            </html>
                                                        </tpl>
                                                    </ext:DataView>
                                                </items>
                                            </ext:Panel>
                                        </items>
                                    </ext:Panel>
                                </items>
                            </ext:Container>
                        </items>
                    </ext:Container>
                    <ext:Panel runat="server"
                        Region="East"
                        ID="pnLateral"
                        Cls="pnLateralSeguimientos"
                        Width="300"
                        HeightSpec="100%"
                        Header="false"
                        Floatable="false"
                        Layout="Fit"
                        Collapsed="true"
                        Collapsible="true">
                        <items>
                            <ext:Panel meta:resourcekey="ctAsideR" ID="ctAsideR" runat="server" Border="false" Header="false" Cls="ctAsideR">
                                <items>
                                    <%--LEFT TABS MENU--%>
                                    <ext:Panel ID="mnAsideR" runat="server" Border="false" Header="false">
                                        <items>
                                            <ext:Button runat="server" ID="btnPnLatInfo" Cls="btnInfo-asR" Handler="RpanelBTNNotes()"></ext:Button>
                                            <ext:Button runat="server" ID="btnPnLatDoc" Cls="btnClip-asR" Handler="RpanelBTNVin()"></ext:Button>
                                        </items>
                                    </ext:Panel>

                                    <%--PANEL INFO GRID--%>
                                    <ext:Panel ID="pnGridInfo" runat="server" Border="false" Header="false" Hidden="false" Scrollable="Vertical" Cls="grdIntoAside">
                                        <dockeditems>
                                            <ext:Toolbar runat="server" ID="Toolbar1" Cls="tbGrey" Dock="Top" Padding="0">
                                                <items>
                                                    <ext:Label meta:resourcekey="lblAsideNameR" ID="Label1" runat="server" IconCls="ico-head-info" Cls="lblHeadAside" Text="<%$ Resources:Comun, strCategoria %>" MarginSpec="36 15 30 15"></ext:Label>
                                                </items>
                                            </ext:Toolbar>
                                        </dockeditems>
                                        <content>
                                            <div>
                                                <table class="tmpCol-table" id="tablaInfoElementos">
                                                    <tbody id="bodyTablaInfoElementos">
                                                    </tbody>
                                                </table>
                                            </div>
                                        </content>
                                    </ext:Panel>
                                </items>
                            </ext:Panel>
                        </items>
                        <listeners>
                            <afterrender fn="SetMaxHeight" />
                        </listeners>
                    </ext:Panel>
                </items>
                <listeners>
                    <afterrender fn="SetMaxHeight" />
                </listeners>
            </ext:Viewport>

            <%--FIN  VIEWPORT --%>
        </div>
    </form>
</body>
</html>
