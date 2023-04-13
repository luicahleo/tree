<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventarioCategoriaDiagrama.aspx.cs" Inherits="TreeCore.ModInventario.InventarioCategoriaDiagrama" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>


     <form id="form1" runat="server">
         
        <div> 

            <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="Sites">
                <Listeners>
                </Listeners>
            </ext:ResourceManager>
             <ext:Hidden runat="server" ID="hdCliID" />
            <ext:Hidden ID="hdMaximo" runat="server" />
            <ext:Hidden ID="hdFichero" runat="server" AutoCreate="true" />
            <ext:Hidden ID="hdInventarioImagenes" runat="server" AutoCreate="true" />
            <link rel="stylesheet" type="text/css" href="/MyFlow/js/grapheditor.css" />
              <ext:Store runat="server"
                ID="storeTipoEmplazamientos"
                AutoLoad="true"
                OnReadData="storeTipoEmplazamientos_Refresh"
                RemoteSort="false">
                <Proxy>
                    <ext:PageProxy />
                </Proxy>
                <Model>
                    <ext:Model runat="server"
                        IDProperty="EmplazamientoTipoID">
                        <Fields>
                            <ext:ModelField Name="EmplazamientoTipoID" Type="Int" />
                            <ext:ModelField Name="Tipo" />
                        </Fields>
                    </ext:Model>
                </Model>
                <Sorters>
                    <ext:DataSorter Property="Tipo" Direction="ASC" />
                </Sorters>
               
            </ext:Store>
            <script type="text/javascript">
                // Parses URL parameters. Supported parameters are:
                // - lang=xy: Specifies the language of the user interface.
                // - touch=1: Enables a touch-style user interface.
                // - storage=local: Enables HTML5 local storage.
                // - chrome=0: Chromeless mode.
                var urlParams = (function (url) {
                    var result = new Object();
                    var idx = url.lastIndexOf('?');

                    if (idx > 0) {
                        var params = url.substring(idx + 1).split('&');

                        for (var i = 0; i < params.length; i++) {
                            idx = params[i].indexOf('=');

                            if (idx > 0) {
                                result[params[i].substring(0, idx)] = params[i].substring(idx + 1);
                            }
                        }
                    }

                    return result;
                })(window.location.href);

                // Default resources are included in grapheditor resources
                mxLoadResources = false;
            </script>
            <script type="text/javascript" src="/MyFlow/js/Init.js"></script>
            
            <script type="text/javascript">
                mxBasePath = '/MyFlow';
            </script>
            <script type="text/javascript" src="/MyFlow/js/pako.min.js"></script>
            <script type="text/javascript" src="/MyFlow/js/base64.js"></script>
            <script type="text/javascript" src="/MyFlow/js/jscolor.js"></script>
            <script type="text/javascript" src="/MyFlow/js/sanitizer.min.js"></script>
            <script type="text/javascript" src="/MyFlow/js/mxClient.js"></script>
            <script type="text/javascript" src="/MyFlow/js/EditorUi.js"></script>
            <script type="text/javascript" src="/MyFlow/js/Editor.js"></script>
            <script type="text/javascript" src="/MyFlow/js/Sidebar.js"></script>
            <script type="text/javascript" src="/MyFlow/js/Graph.js"></script>
            <script type="text/javascript" src="/MyFlow/js/Format.js"></script>
            <script type="text/javascript" src="/MyFlow/js/Shapes.js"></script>
            <script type="text/javascript" src="/MyFlow/js/Actions.js"></script>
            <script type="text/javascript" src="/MyFlow/js/Menus.js"></script>
            <script type="text/javascript" src="/MyFlow/js/Toolbar.js"></script>
            <script type="text/javascript" src="/MyFlow/js/Dialogs.js"></script>
            <script type="text/javascript" src="/MyFlow/js/mxEditorCodec.js"></script>
            <script type="text/javascript" src="/MyFlow/js/mxUtils.js"></script>
            <script type="text/javascript" src="js/InventarioCategoriaDiagrama.js"></script>
            
            <script type="text/javascript">
                function InitGraphAfter() {
                    // Extends EditorUi to update I/O action states based on availability of backend
                    (function () {
                        var editorUiInit = EditorUi.prototype.init;

                        EditorUi.prototype.init = function () {
                            editorUiInit.apply(this, arguments);
                            this.actions.get('export').setEnabled(false);

                            //previous repeated code collapsed for brevity 

                            this.editor.setFilename('doc1.xml');

                            //save editorUi object
                            var editorUI = this;

                            // Shows the given graph if exits
                            // Workaround because window['mxGraphModel'] is not defined
                            window['mxEditor'] = mxEditor;
                            window['mxGeometry'] = mxGeometry;
                            window['mxDefaultKeyHandler'] = mxDefaultKeyHandler;
                            window['mxDefaultPopupMenu'] = mxDefaultPopupMenu;
                            window['mxGraph'] = mxGraph;
                            window['mxCell'] = mxCell;
                            window['mxCellPath'] = mxCellPath;
                            window['mxGraph'] = mxGraph;
                            window['mxStylesheet'] = mxStylesheet;
                            window['mxDefaultToolbar'] = mxDefaultToolbar;
                            window['mxGraphModel'] = mxGraphModel;
                            //InitGraph();
                            var graph = editorUI.editor.graph;
                            graph.getModel().beginUpdate();
                            try {
                                var sFichero = hdFichero.value;
                                if (sFichero != null && sFichero != "" && sFichero != 'undefined') {

                                    var req = mxUtils.load(sFichero);
                                    var root = req.getDocumentElement();
                                    var dec = new mxCodec(root.OwnerDocument);
                                    dec.decode(root, graph.getModel());
                                    //let layout = new mxFastOrganicLayout(graph);mxCompactTreeLayout
                                    let layout = new mxCompactTreeLayout(graph);
                                    layout.orientation = mxConstants.DIRECTION_SOUTH;
                                    layout.execute(graph.getDefaultParent());
                                    graph.fit();

                                }
                            } finally {
                                graph.getModel().endUpdate();
                            }

                            // Right click menu
                            graph.addListener(mxEvent.DOUBLE_CLICK, function (sender, evt) {
                                var cell = evt.getProperty('cell');
                                if (cell != null) {
                                    if (cell.vertex != null && cell.vertex == 1) {
                                        //console.log(cell.userData);
                                        if (cell.userData && cell.userData.isGroup) {	//custom
                                            //is group, do nothing
                                        } else {
                                            //is cell, open cell edit panel    
                                            openCell(cell);
                                        }
                                    } else if (cell.edge != null && cell.edge == 1) {
                                        //is edge, open edge edit
                                        openEdge(cell);
                                    }
                                }
                            });
                            graph.popupMenuHandler.factoryMethod = function (menu, cell, evt) {
                                //return createPopupMenu(graph, menu, cell, evt);
                                /*if (cell.edge) {
                                    menu.addItem('First edge option', null, function () {
                                        alert('This is the first option of edge ');
                                    })
                                    menu.addItem('Second edge option', null, function () {
                                        alert('This is the second option of edge ');
                                    })
                                }
                                if (cell.vertex) {
                                    menu.addItem('First vertex option', null, function () {
                                        alert('This is the first option of vertex ');
                                    })
                                    menu.addItem('Second vertex option', null, function () {
                                        alert('This is the second option of vertex ');
                                    })
                                }*/
                            }
                            
                          

                            //this part shal be inserted
                            //override EditorUi.saveFile function for customization
                            this.save = saveXml;
                            function saveXml() {

                                if (editorUI.editor.graph.isEditing()) {
                                    editorUI.editor.graph.stopEditing();
                                }

                                var xml = mxUtils.getXml(editorUI.editor.getGraphXml());
                                //xml = encodeURIComponent(xml);

                                if (xml.length < MAX_REQUEST_SIZE) {
                                    //$.ajax({
                                    //    type: "POST",
                                    //    url: "home/save",
                                    //    processData: false,
                                    //    contentType: "application/json; charset=utf-8",
                                    //    data: JSON.stringify({ 'xml': xml }),
                                    //    success: function (response) {
                                    //        //alert(response.message);
                                    //    },
                                    //    error: function (ex) {
                                    //        alert(ex.message);
                                    //    }
                                    //});
                                    SaveDiagram(xml);
                                }
                                else {
                                    mxUtils.alert(mxResources.get('drawingTooLarge'));
                                    mxUtils.popup(xml);

                                    return;
                                }

                            };

                            // Updates action states which require a backend
                            if (!Editor.useLocalStorage) {
                                mxUtils.post(OPEN_URL, '', mxUtils.bind(this, function (req) {
                                    var enabled = req.getStatus() != 404;
                                    this.actions.get('open').setEnabled(enabled || Graph.fileSupport);
                                    this.actions.get('import').setEnabled(enabled || Graph.fileSupport);
                                    this.actions.get('save').setEnabled(true);
                                    this.actions.get('saveAs').setEnabled(enabled);
                                    this.actions.get('export').setEnabled(enabled);
                                }));
                            }
                        };
                        Editor.prototype.resetGraph = function () {
                            this.graph.gridEnabled = true;
                            
                            
                            this.graph.graphHandler.guidesEnabled = true;
                            this.graph.setTooltips(true);
                            this.graph.setConnectable(true);
                            this.graph.foldingEnabled = true;
                            this.graph.scrollbars = true;
                            this.graph.pageVisible = false;
                            this.graph.pageBreaksVisible = false;
                            this.graph.preferPageSize = this.graph.pageBreaksVisible;
                            this.graph.background = null;
                            this.graph.pageScale = mxGraph.prototype.pageScale;
                            this.graph.pageFormat = mxGraph.prototype.pageFormat;
                            this.graph.currentScale = 1;
                            this.graph.currentTranslate.x = 0;
                            this.graph.currentTranslate.y = 0;
                            this.updateGraphComponents();
                            this.graph.view.setScale(1);




                            
                        };
                        EditorUi.prototype.menubarHeight = 38;
                        EditorUi.prototype.formatEnabled = false;
                        //EditorUi.prot.menu.
                        EditorUi.prototype.formatWidth = 38;
                        EditorUi.prototype.toolbarHeight = 38;
                        EditorUi.prototype.sidebarFooterHeight = 240;
                        //EditorUi.prototype.sidebarContainer.style.width = 0;
                        EditorUi.prototype.hsplitPosition = 0;
                        
                        EditorUi.prototype.splitSize = 0;
                        //EditorUi.prototype.setPageVisible(false);
                        //DiagramFormatPanel.showPageView = true;
                        EditorUi.prototype.height = 600;
                        //EditorUi.prototype.layout = "Landscape";
                        //var ui = this.editorUi;
                        //EditorUi.prototype.setPageVisible(false);
                        //EditorUi.prototype.toggleFormatPanel();
                        //EditorUi.prototype.setScrollbars(false);

                        // Adds required resources (disables loading of fallback properties, this can only
                        // be used if we know that all keys are defined in the language specific file)
                        mxResources.loadDefaultBundle = false;
                        var bundle = mxResources.getDefaultBundle(RESOURCE_BASE, mxLanguage) ||
                            mxResources.getSpecialBundle(RESOURCE_BASE, mxLanguage);

                        // Fixes possible asynchronous requests
                        mxUtils.getAll([bundle, STYLE_PATH + '/default.xml'], function (xhr) {
                            // Adds bundle text to resources
                            mxResources.parse(xhr[0].getText());

                            // Configures the default graph theme
                            var themes = new Object();
                            themes[Graph.prototype.defaultThemeName] = xhr[1].getDocumentElement();

                            // Main
                            new EditorUi(new Editor(urlParams['chrome'] == '0', themes));
                        }, function () {
                            document.body.innerHTML = '<center style="margin-top:10%;">Error loading resource files. Please check browser console.</center>';
                        });
                    })();
                }
                /**
                 * Sets the XML node for the current diagram.
                 */
               

                Toolbar.prototype.init = function () {
                    var sw = screen.width;
                  

                    // Takes into account initial compact mode
                    sw -= (screen.height > 740) ? 56 : 0;

                   /* if (sw >= 700) {
                        var formatMenu = this.addMenu('', mxResources.get('view') + ' (' + mxResources.get('panTooltip') + ')', true, 'viewPanels', null, true);
                        this.addDropDownArrow(formatMenu, 'geSprite-formatpanel', 38, 50, -4, -3, 36, -8);
                        this.addSeparator();
                    }*/

                    var viewMenu = this.addMenu (['Zoom'],  mxResources.get('zoom') + ' (Alt+Mousewheel)', true, 'viewZoom', null, true);// = //this.addMenu('', mxResources.get('zoom') + ' (Alt+Mousewheel)', true, 'viewZoom', null, true);
                    viewMenu.showDisabled = true;
                    viewMenu.style.whiteSpace = 'nowrap';
                    viewMenu.style.position = 'relative';
                    viewMenu.style.overflow = 'hidden';

                    if (EditorUi.compactUi) {
                        viewMenu.style.width = (mxClient.IS_QUIRKS) ? '58px' : '50px';
                    }
                    else {
                        viewMenu.style.width = (mxClient.IS_QUIRKS) ? '62px' : '36px';
                    }

                    if (sw >= 420) {
                        this.addSeparator();
                        var elts = this.addItems(['zoomIn', 'zoomOut']);
                        elts[0].setAttribute('title', mxResources.get('zoomIn') + ' (' + this.editorUi.actions.get('zoomIn').shortcut + ')');
                        elts[1].setAttribute('title', mxResources.get('zoomOut') + ' (' + this.editorUi.actions.get('zoomOut').shortcut + ')');
                    }

                    // Updates the label if the scale changes
                    this.updateZoom = mxUtils.bind(this, function () {
                        viewMenu.innerHTML = Math.round(this.editorUi.editor.graph.view.scale * 100) + '%' +
                            this.dropdownImageHtml;

                        if (EditorUi.compactUi) {
                            viewMenu.getElementsByTagName('img')[0].style.right = '1px';
                            viewMenu.getElementsByTagName('img')[0].style.top = '5px';
                        }
                    });
                    /*
                    this.editorUi.editor.graph.view.addListener(mxEvent.EVENT_SCALE, this.updateZoom);
                    this.editorUi.editor.addListener('resetGraphView', this.updateZoom);

                    var elts = this.addItems(['-', 'undo', 'redo']);
                    elts[1].setAttribute('title', mxResources.get('undo') + ' (' + this.editorUi.actions.get('undo').shortcut + ')');
                    elts[2].setAttribute('title', mxResources.get('redo') + ' (' + this.editorUi.actions.get('redo').shortcut + ')');

                    if (sw >= 320) {
                        var elts = this.addItems(['-', 'delete']);
                        elts[1].setAttribute('title', mxResources.get('delete') + ' (' + this.editorUi.actions.get('delete').shortcut + ')');
                    }

                    if (sw >= 550) {
                        this.addItems(['-', 'toFront', 'toBack']);
                    }

                    if (sw >= 740) {
                        this.addItems(['-', 'fillColor']);

                        if (sw >= 780) {
                            this.addItems(['strokeColor']);

                            if (sw >= 820) {
                                this.addItems(['shadow']);
                            }
                        }
                    }
                    
                    if (sw >= 400) {
                        this.addSeparator();

                        if (sw >= 440) {
                            this.edgeShapeMenu = this.addMenuFunction('', mxResources.get('connection'), false, mxUtils.bind(this, function (menu) {
                                this.editorUi.menus.edgeStyleChange(menu, '', [mxConstants.STYLE_SHAPE, 'width'], [null, null], 'geIcon geSprite geSprite-connection', null, true).setAttribute('title', mxResources.get('line'));
                                this.editorUi.menus.edgeStyleChange(menu, '', [mxConstants.STYLE_SHAPE, 'width'], ['link', null], 'geIcon geSprite geSprite-linkedge', null, true).setAttribute('title', mxResources.get('link'));
                                this.editorUi.menus.edgeStyleChange(menu, '', [mxConstants.STYLE_SHAPE, 'width'], ['flexArrow', null], 'geIcon geSprite geSprite-arrow', null, true).setAttribute('title', mxResources.get('arrow'));
                                this.editorUi.menus.edgeStyleChange(menu, '', [mxConstants.STYLE_SHAPE, 'width'], ['arrow', null], 'geIcon geSprite geSprite-simplearrow', null, true).setAttribute('title', mxResources.get('simpleArrow'));
                            }));

                            this.addDropDownArrow(this.edgeShapeMenu, 'geSprite-connection', 44, 50, 0, 0, 22, -4);
                        }

                        this.edgeStyleMenu = this.addMenuFunction('geSprite-orthogonal', mxResources.get('waypoints'), false, mxUtils.bind(this, function (menu) {
                            this.editorUi.menus.edgeStyleChange(menu, '', [mxConstants.STYLE_EDGE, mxConstants.STYLE_CURVED, mxConstants.STYLE_NOEDGESTYLE], [null, null, null], 'geIcon geSprite geSprite-straight', null, true).setAttribute('title', mxResources.get('straight'));
                            this.editorUi.menus.edgeStyleChange(menu, '', [mxConstants.STYLE_EDGE, mxConstants.STYLE_CURVED, mxConstants.STYLE_NOEDGESTYLE], ['orthogonalEdgeStyle', null, null], 'geIcon geSprite geSprite-orthogonal', null, true).setAttribute('title', mxResources.get('orthogonal'));
                            this.editorUi.menus.edgeStyleChange(menu, '', [mxConstants.STYLE_EDGE, mxConstants.STYLE_ELBOW, mxConstants.STYLE_CURVED, mxConstants.STYLE_NOEDGESTYLE], ['elbowEdgeStyle', null, null, null], 'geIcon geSprite geSprite-horizontalelbow', null, true).setAttribute('title', mxResources.get('simple'));
                            this.editorUi.menus.edgeStyleChange(menu, '', [mxConstants.STYLE_EDGE, mxConstants.STYLE_ELBOW, mxConstants.STYLE_CURVED, mxConstants.STYLE_NOEDGESTYLE], ['elbowEdgeStyle', 'vertical', null, null], 'geIcon geSprite geSprite-verticalelbow', null, true).setAttribute('title', mxResources.get('simple'));
                            this.editorUi.menus.edgeStyleChange(menu, '', [mxConstants.STYLE_EDGE, mxConstants.STYLE_ELBOW, mxConstants.STYLE_CURVED, mxConstants.STYLE_NOEDGESTYLE], ['isometricEdgeStyle', null, null, null], 'geIcon geSprite geSprite-horizontalisometric', null, true).setAttribute('title', mxResources.get('isometric'));
                            this.editorUi.menus.edgeStyleChange(menu, '', [mxConstants.STYLE_EDGE, mxConstants.STYLE_ELBOW, mxConstants.STYLE_CURVED, mxConstants.STYLE_NOEDGESTYLE], ['isometricEdgeStyle', 'vertical', null, null], 'geIcon geSprite geSprite-verticalisometric', null, true).setAttribute('title', mxResources.get('isometric'));
                            this.editorUi.menus.edgeStyleChange(menu, '', [mxConstants.STYLE_EDGE, mxConstants.STYLE_CURVED, mxConstants.STYLE_NOEDGESTYLE], ['orthogonalEdgeStyle', '1', null], 'geIcon geSprite geSprite-curved', null, true).setAttribute('title', mxResources.get('curved'));
                            this.editorUi.menus.edgeStyleChange(menu, '', [mxConstants.STYLE_EDGE, mxConstants.STYLE_CURVED, mxConstants.STYLE_NOEDGESTYLE], ['entityRelationEdgeStyle', null, null], 'geIcon geSprite geSprite-entity', null, true).setAttribute('title', mxResources.get('entityRelation'));
                        }));

                        this.addDropDownArrow(this.edgeStyleMenu, 'geSprite-orthogonal', 44, 50, 0, 0, 22, -4);
                    }

                    this.addSeparator();
                    var insertMenu = this.addMenu('', mxResources.get('insert') + ' (' + mxResources.get('doubleClickTooltip') + ')', true, 'insert', null, true);
                    this.addDropDownArrow(insertMenu, 'geSprite-plus', 38, 48, -4, -3, 36, -8);
                    this.addTableDropDown();*/
                };

                Sidebar.prototype.init = function () {
                    var dir = STENCIL_PATH;

                    this.addSearchPalette(false);
                    this.showDisabled = true;
                    //this.addGeneralPalette(true);
                    //this.addMiscPalette(false);
                    //this.addAdvancedPalette(false);
                    //this.addBasicPalette(dir);

                    //this.setCurrentSearchEntryLibrary('arrows');
                    //this.addStencilPalette('arrows', mxResources.get('arrows'), dir + '/arrows.xml',
                    //    ';whiteSpace=wrap;html=1;fillColor=#ffffff;strokeColor=#000000;strokeWidth=2');
                    //this.setCurrentSearchEntryLibrary();

                    //this.addUmlPalette(false);
                    //this.addBpmnPalette(dir, false);
                    //this.addStencilPalette('myflow', 'MyFlow', dir + '/myflow.xml',
                    //    ';whiteSpace=wrap;html=1;fillColor=#ffffff;strokeColor=#000000;strokeWidth=2');

                    //this.setCurrentSearchEntryLibrary('flowchart');
                    //this.addStencilPalette('flowchart', 'Flowchart', dir + '/flowchart.xml',
                    //    ';whiteSpace=wrap;html=1;fillColor=#ffffff;strokeColor=#000000;strokeWidth=2');
                    //this.setCurrentSearchEntryLibrary();
                    var sImagenes = hdInventarioImagenes.value;
                    var array = sImagenes.split(",");
                    //alert(sImagenes);

                    //['Earth_globe', 'Empty_Folder', 'Full_Folder', 'Gear', 'Lock', 'Software', 'Virus', 'Email',
                    //        'Database', 'Router_Icon', 'iPad', 'iMac', 'Laptop', 'MacBook', 'Monitor_Tower', 'Printer',
                    //        'Server_Tower', 'Workstation', 'Firewall_02', 'Wireless_Router_N', 'Credit_Card',
                    //        'Piggy_Bank', 'Graph', 'Safe', 'Shopping_Cart', 'Suit1', 'Suit2', 'Suit3', 'Pilot1',
                    //        'Worker1', 'Soldier1', 'Doctor1', 'Tech1', 'Security1', 'Telesales1']

                    /*this.setCurrentSearchEntryLibrary('inventory');
                    this.addImagePalette('inventory', mxResources.get('inventory'), '/documentos/Inventory/', '.png',
                        array, null,
                        {
                            'Wireless_Router_N': 'wireless router switch wap wifi access point wlan',
                            'Router_Icon': 'router switch'
                        });
                    this.setCurrentSearchEntryLibrary();*/
                };
            </script>
            <script type="text/javascript">
                function InitGraph() {
                    var sFichero = hdFichero.value;
                    if (sFichero != null && sFichero != "" && sFichero != 'undefined') {
                        var graph = editorUI.editor.graph;
                        var req = mxUtils.load(sFichero);
                        var root = req.getDocumentElement();
                        var dec = new mxCodec(root);
                        dec.decode(root, graph.getModel());
                     
                        graph.getModel().endUpdate();
                    }
                }
            </script>

            <ext:Viewport ID="Viewport1" runat="server" Cls="viewport">
                <Items>
                    
                            <ext:Panel runat="server" ID="PanelImpresion">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar2" runat="server">
                                        <Items>
                                        <ext:ComboBox meta:resourceKey="cmbTipoEmplazamientos"
                                        ID="cmbTipoEmplazamientos" runat="server"
                                        StoreID="storeTipoEmplazamientos"
                                        DisplayField="Tipo"
                                        ValueField="EmplazamientoTipoID"
                                        Editable="true"
                                        ForceSelection="true"
                                        FieldLabel="<%$ Resources:Comun, strTiposEmplazamientos %>"
                                        LabelAlign="Left"
                                        Scrollable="Vertical"
                                        WidthSpec="25%"
                                        EmptyText="<%$ Resources:Comun, strSeleccionar %>"
                                        AllowBlank="true"
                                        QueryMode="Local"
                                        Mode="Local"
                                        >
                                        <Listeners>
                                            <Select Fn="SeleccionarTipoEmplazamientos" />
                                            <TriggerClick Fn="ClearTipoEmplazamientos" />
                                        </Listeners>
                                        <Triggers>
                                            <ext:FieldTrigger IconCls="ico-reload"
                                                Icon="Clear"
                                                Hidden="true"
                                                Weight="-1"
                                                QTip="<%$ Resources:Comun, strRecargarLista %>" />
                                        </Triggers>
                                    </ext:ComboBox>
                                           
                                   <ext:Button runat="server"
                                                    ID="btnToggle"
                                                    Width="100"
                                                    EnableToggle="true"
                                                    
                                                    Pressed="true"
                                                    AriaLabel=""
                                                    ToolTip="">
                                                    <Listeners>
                                                        <Click Fn="SeleccionarTipoEmplazamientos" />
                                                    </Listeners>
                                                </ext:Button>
                                            <ext:Button runat="server"
                                                    ID="btnToggle1"
                                                    Width="100"
                                                    EnableToggle="true"
                                                    Cls="btn-toggleGrid"
                                                    Pressed="true"
                                                    AriaLabel=""
                                                    ToolTip="">
                                                    <Listeners>
                                                        <Click Fn="SeleccionarTipoEmplazamientos" />
                                                    </Listeners>
                                                </ext:Button>
                                            <ext:Button runat="server"
                                                    ID="btnToggle2"
                                                    Width="100"
                                                    EnableToggle="true"
                                                    Cls="btn-toggleGrid"
                                                    Pressed="true"
                                                    AriaLabel=""
                                                    ToolTip=""
                                                Handler ="SeleccionarTipoVinculacion">
                                             </ext:Button>
                                            <ext:Button runat="server"
                                                    ID="btnToggle3"
                                                    Width="100"
                                                    EnableToggle="true"
                                                    Cls="btn-toggleGrid"
                                                    Pressed="true"
                                                    AriaLabel=""
                                                    ToolTip=""
                                                Handler ="SeleccionarTipoVinculacion">
                                             </ext:Button>
                                            <ext:Button runat="server"
                                                    ID="btnToggle4"
                                                    Width="100"
                                                    EnableToggle="true"
                                                    Cls="btn-toggleGrid"
                                                    Pressed="true"
                                                    AriaLabel=""
                                                    ToolTip=""
                                                Handler ="SeleccionarTipoVinculacion">
                                             </ext:Button>
                                             <ext:Button runat="server"
                                                    ID="btnToggle5"
                                                    Width="100"
                                                    EnableToggle="true"
                                                    Cls="btn-toggleGrid"
                                                    Pressed="true"
                                                    AriaLabel=""
                                                    ToolTip=""
                                                Handler ="SeleccionarTipoVinculacion">
                                             </ext:Button>
                                            <ext:Button runat="server"
                                                    ID="btnToggle6"
                                                    Width="100"
                                                    EnableToggle="true"
                                                    Cls="btn-toggleGrid"
                                                    Pressed="true"
                                                    AriaLabel=""
                                                    ToolTip=""
                                                Handler ="SeleccionarTipoVinculacion">
                                             </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                 
                                
                                <Content>
                                </Content>
                               

                            </ext:Panel>
                        
                </Items>
            </ext:Viewport>
        </div>

    </form>
</body>
</html>
