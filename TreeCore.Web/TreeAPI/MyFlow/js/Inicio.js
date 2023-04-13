//import 'ol/ol.css';
//import Draw from 'ol/interaction/Draw';
//import Map from 'ol/Map';
//import View from 'ol/View';
//import { OSM, Vector as VectorSource } from 'ol/source';
//import { Tile as TileLayer, Vector as VectorLayer } from 'ol/layer';

//var map = new ol.Map({
//    target: 'map',
//    layers: [
//        new ol.layer.Tile({
//            source: new ol.source.OSM()
//        })
//    ],
//    view: new ol.View({
//        center: ol.proj.fromLonLat([37.41, 8.82]),
//        zoom: 4
//    })
//});


//import WKT from 'ol/format/WKT';

// Local variables
var map;
var origen;
var vector;
var draw; // global so we can remove it later

function storeClientesRenderizado() {
    //alert("Agregando combo");
    //    tb.add(
}

function Inicia() {
    //    var comboClientes = new Ext.form.ComboBox({
    //        id: 'cmbClientes',
    //        triggerAction: 'all',
    //        mode: 'remote',
    //        store: storeClientes,
    //        valueField: 'ClienteID',
    //        displayField: 'Cliente',
    //        editable: false 
    //    });

    var tb = new Ext.Toolbar({
        items: [
            //{ xtype: 'tbspacer', width: 5 },
            //{
            //    xtype: 'label', // default for Toolbars, same as 'tbbutton'
            //    text: jsCliente
            //},
            { xtype: 'tbspacer', width: 5 },
            cmbClientes,
            { xtype: 'tbspacer', width: 5 },
            {
                xtype: 'label', // default for Toolbars, same as 'tbbutton'
                text: jsProyecto
            },
            { xtype: 'tbspacer', width: 5 },
            cmbProyectos,
            { xtype: 'tbspacer', width: 5 },
            btnCuadoMando,
            { xtype: 'tbspacer', width: 5 },
            btnExcel
        ],

    });

    //    cmbClientes.render();

    new Ext.Viewport({
        layout: 'border',
        items: [
            {
                region: 'north',
                xtype: 'panel',
                tbar: tb,
                autoHeight: true,
                border: false
            },
            {
                region: 'center',
                xtype: 'gmappanel',
                name: 'mapInstalacion',
                id: 'mapInstalacion',
                gmapType: 'map',
                zoomLevel: 5,
                ShowMask: true, //ine
                msg: jsMensajeProcesando,
                //setCenter: {
                //    lat: 40.782686,
                //    lng: -4
                //}
            }]
    });

    CargarMapa();
    //sm_DocumentReady();
}

function ClienteSeleccionar() {
    primerCarga = true;

    hdCliID.setValue(cmbClientes.value);



    cmbProyectos.clearValue();
    //storeProyectos.removeAll();
    storeProyectos.reload();

    //storeEmplazamientos.removeAll();
    //storeEmplazamientos.reload();
}

function BorrarCliente() {
    hdCliID.setValue('');
    cmbClientes.clearValue();

    hdProyectoID.setValue('');
    cmbProyectos.clearValue();
    //storeProyectos.removeAll();

    //storeEmplazamientos.removeAll();
    primerCarga = true;
    //storeEmplazamientos.reload();
}

function cmbProyectos_Select(sender, e) {
    primerCarga = true;
    // activamos cuadro de mando
    btnCuadoMando.setDisabled(false);
    //storeEmplazamientos.reload();
}

function ProyectoSeleccionar() {
    primerCarga = true;
    // activamos cuadro de mando
    btnCuadoMando.setDisabled(false);
    hdProyectoID.setValue(cmbProyectos.value);
    mascara();
    CargarMapa();
}

var TriggerProyectos = function (el, trigger, index) {
    switch (index) {
        case 0:
            hdProyectoID.setValue('');
            BorrarProyecto();
            break;
        case 1:
            storeProyectos.reload();
            break;
    }
}
function BorrarProyecto() {
    cmbProyectos.clearValue();
    btnCuadoMando.setDisabled(true);
    //storeEmplazamientos.removeAll(); //ine
    primerCarga = true;
    hdProyectoID.setValue(null);

    //storeEmplazamientos.reload();
}

function ComboCliente(el, trigger, index) {
    switch (index) {
        case 0:
            BorrarCliente();
            break;
        case 1:
            storeClientes.reload();
            break;

    }
}

function mascara() {
    ////Ext.get('wait').on('click', function () {
    //Ext.Msg.wait('Cargando...');
    //window.setTimeout(function () {
    //    Ext.Msg.hide();
    //}, 6000);
    ////}, this);
    Ext.Msg.alert(jsAlerta, jsCargarMapa);//'Alerta','Se cargarán los emplazamientos... este proceso puede tardar'
}

function esperaCarga() {
    Ext.net.Mask.show({ msg: jsMensajeProcesando });
}

//
// guardar mapa en BBDD
//
function SalvarMapa() {
    var listString = document.getElementById('hdElementos').value;
    PageMethods.insertarmapa(listString);
    alert("Se acabo");
}
//
// guardar mapa en fichero
//
var exportMap = function () {
    canvas = document.getElementsByTagName('canvas')[0];
    canvas.toBlob(function (blob) {
        saveAs(blob, 'map.png');
    })
}

function CargarMapa() {
    //
    // pintar mapa
    //

    var listString = document.getElementById('hdGeometrias').value;
    var geometries = listString.split('~');
    //
    // comprobar proyecto
    //
    //var proyectoid = SelectBox1.value.split('-')[0];
    var proyectoid = hdProyectoID.getValue();



    //var wktReader = new ol.format.WKT();
    ////alert('despues');
    var featureCollection = [];
    ////alert('Geometrias: ' + geometries.length);
    //for (var i = 0; i < geometries.length; i++) {
    //    var name = geometries[i].split(':')[0];
    //    var geo = geometries[i].split(':')[1];
    //    var proj = geometries[i].split(':')[2];
    //    var gid = geometries[i].split(':')[3];

    //    if (proj != proyectoid) { continue };
    //    var feature = wktReader.readFeature(geo);
    //    feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
    //    feature.attributes = {
    //        name: name
    //    }

    //    if (feature.getGeometry().getType() == 'Polygon') {
    //        feature.setStyle(new ol.style.Style({
    //            stroke: new ol.style.Stroke({ color: 'blue', width: 3 }),
    //            fill: new ol.style.Fill({ color: 'rgba(0, 0, 255, 0.1)' })
    //        }));
    //        featureCollection.push(feature);
    //    }
    //    else if (feature.getGeometry().getType() == 'MultiPolygon') {
    //        feature.setStyle(new ol.style.Style({
    //            stroke: new ol.style.Stroke({ color: 'blue', width: 3 }),
    //            fill: new ol.style.Fill({ color: 'rgba(0, 0, 255, 0.1)' }),
    //            text: new ol.style.Text({
    //                font: '10px Calibri,sans-serif',
    //                textAlign: 'center',
    //                textBaseline: 'middle',
    //                overflow: 'true',
    //                stroke: new ol.style.Stroke({ color: '#fff', width: 2 }),
    //                fill: new ol.style.Fill({ color: '#000' }),
    //                text: name
    //            })
    //        }));

    //        featureCollection.push(feature);
    //    }
    //    else if (feature.getGeometry().getType() == 'LineString') {
    //        feature.setStyle(new ol.style.Style({
    //            stroke: new ol.style.Stroke({ color: 'red', width: 3 }),
    //            fill: new ol.style.Fill({ color: 'rgba(0, 0, 255, 0.1)' })
    //        }));
    //        featureCollection.push(feature);
    //    }
    //    else if (feature.getGeometry().getType() == 'MultiLineString') {
    //        feature.setStyle(new ol.style.Style({
    //            stroke: new ol.style.Stroke({ color: 'red', width: 3 }),
    //            fill: new ol.style.Fill({ color: 'rgba(0, 0, 255, 0.1)' }),
    //        }));
    //        featureCollection.push(feature);
    //    }
    //    else if (feature.getGeometry().getType() == 'Point') {
    //        feature.setStyle(new ol.style.Style({
    //            image: new ol.style.Icon(/** @type {olx.style.IconOptions} */({
    //                anchor: [0.5, 46],
    //                anchorXUnits: 'fraction',
    //                anchorYUnits: 'pixels',
    //                opacity: 0.75,
    //                src: 'Icons/marker.png'
    //            }))
    //        }));
    //        featureCollection.push(feature);
    //    }
    //}

    var source2 = new ol.source.Vector({
        features: featureCollection
    });

    var vectorlayer2 = new ol.layer.Vector({
        title: 'Catastro',
        source: source2
    });

    var source3 =
        new ol.source.Vector(
            { wrapX: false }
        );

    var vectorlayer3 = new ol.layer.Vector({
        title: 'Red Distribucion',
        source: source3
    });

    var tilelayer = new ol.layer.Tile({
        title: 'Mapa Base',
        visible: false,
        source: new ol.source.OSM()
    });
    //
    // popup
    //
    var container = document.getElementById('popup');
    var overlay = new ol.Overlay({
        element: container,
        autoPan: true,
        autoPanAnimation: {
            duration: 250
        }
    });
    //
    // Dibujar scaleline
    //
    var scaleLineControl = new ol.control.ScaleLine({
        className: 'ol-scale-line',
        target: document.getElementById('scale-line')
    });
    scaleLineControl.setUnits("metric");

    // Current layer
    origen =
        new ol.source.Vector(
            { wrapX: false }
        );

    vector = new ol.layer.Vector({
        source: origen
    });


    //
    // Definicion del mapa
    //
    map = new ol.Map({
        controls: new ol.control.defaults().extend([
            scaleLineControl
        ]),
        //layers: [tilelayer, vector],
        //    new ol.layer.Group({
        //        title: 'Mapa Base',
        //        layers: [tilelayer]
        //    }),
        //    new ol.layer.Group({
        //        title: 'Catastro',
        //        layers: [vectorlayer2]
        //    }),
        //    new ol.layer.Group({
        //        title: 'Red Distribucion',
        //        layers: [vectorlayer3]
        //    })
        //],
        overlays: [overlay],
        target: 'map',
        view: new ol.View({
            center: ol.proj.fromLonLat([-3.70379, 40.416775]),
            zoom: 17
        })
    });

    // LAYERS
    var capasCadena = hdCapas.getValue();

    //alert(proyectoid);

    var capas = capasCadena.split('$$');

    for (var i = 0; i < capas.length; i++) {
        if (capas[i] != "") {
            var capaDetalle = capas[i].split('#');
            window['Capa' + capaDetalle[0]] = new ol.layer.Vector({
                title: capaDetalle[1],
                source: source3
            });
            map.addLayer(window['Capa' + capaDetalle[0]]);
        }
    }



    // Adds the base map    
    map.addLayer(tilelayer);
    map.addLayer(vector);


    //
    // layerswitcher
    //
    var layerSwitcher = new ol.control.LayerSwitcher({ tipLabel: 'Leyenda' });
    map.addControl(layerSwitcher);
    layerSwitcher.showPanel();

    ////
    //// dibujar geometrias
    ////
    //var typeSelect = document.getElementById('hdComponente');
    //var draw, snap;

    //var modify = new ol.interaction.Modify({ source: source3 });
    //map.addInteraction(modify);

    //function addInteraction() {
    //    var value = typeSelect.value;
    //    if (value !== 'None') {
    //        draw = new ol.interaction.Draw({ source: source3, type: typeSelect.value });
    //        map.addInteraction(draw);
    //        snap = new ol.interaction.Snap({ source: source3 });
    //        map.addInteraction(snap);
    //    }
    //}
    ////
    //// salvar geometria
    ////
    //typeSelect.onchange = function () {
    //    if (draw.type_ = 'LineString') {
    //        var str1 = 'MULTILINESTRING';
    //    }
    //    var str2 = "((";
    //    var str3 = " ";
    //    if (draw.sketchCoords_ != null) {
    //        for (var i = 0; i < draw.sketchCoords_.length; i++) {
    //            str3 = str3.concat(draw.sketchCoords_[i][0]);
    //            str3 = str3.concat(" ");
    //            str3 = str3.concat(draw.sketchCoords_[i][1]);
    //            if (i < draw.sketchCoords_.length - 1) { str3 = str3.concat(","); }
    //        }
    //    }

    //    var str4 = "))";
    //    var str5 = str1.concat(str2.concat(str3));
    //    document.getElementById('hdElementos').value = str5.concat(str4);
    //    map.removeInteraction(draw);
    //    map.removeInteraction(snap);
    //    addInteraction();
    //};
    //addInteraction();
    ////
    //// Pop ups
    ////
    //var content = document.getElementById('popup-content');
    //var closer = document.getElementById('popup-closer');
    //closer.onclick = function () {
    //    overlay.setPosition(undefined);
    //    closer.blur();
    //    return false;
    //};
    //map.on('singleclick', function (evt) {
    //    var coordinate = evt.coordinate;
    //    var hdms = ol.coordinate.toStringHDMS(ol.proj.toLonLat(coordinate));
    //    content.innerHTML = '<p>Las coordenadas son:</p><code>' + hdms + '</code>';
    //    overlay.setPosition(coordinate);
    //});

    //map.on('dblclick', function (evt) {
    //    mostrarSeguimiento(26608);
    //});
    //
    // reticula
    //
    var graticule = new ol.layer.Graticule({
        strokeStyle: new ol.style.Stroke({
            color: 'rgba(0,0,255,1)',
            width: 2,
            lineDash: [0.5, 4]
        }),
        showLabels: true
    });

    graticule.setMap(map);

    //addInteraction();
}



function mostrarSeguimiento(id) {
    //var record = storeEmplazamientos.getById(id);
    //if (record == null) return;
    if (PanelSeguimiento.hidden == false) {
        PanelSeguimiento.load({
            url: '../../Inventario/pages/InventarioEmplazamientosInfraestructuras.aspx',
            params: {
                EmplazamientoID: id,
                EscritorioClienteID: true
            },
            callback: function (el, sucess, resp, opts) { if (!sucess) { Ext.Msg.alert("", ""); } },
            scrpts: true,
            mode: 'iframe',
            ShowMask: true,
            text: jsMensajeProcesando, //strCargando,
            PassParentSize: 'true'
        });
        PanelSeguimiento.show();
        winSeguimiento.setTitle("Inventario" + ' ' + " Sitio Historico 4");
        winSeguimiento.show();
    }
}

function ComponenteSeleccionar() {

    if (cmbComponentes.value != null && cmbComponentes.value != "" && typeof cmbComponentes.value !== 'undefined') {
        var elementos = cmbComponentes.value.split('$');

        hdComponente.value = elementos[1];
        hdIcono.value = elementos[2];
        hdColor.value = elementos[3];
        hdAncho.value = elementos[4];
    }

    Sites.CambiarComponente(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }

            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        });
    if (typeof draw !== 'undefined') {
        map.removeInteraction(draw);
    }
    addInteraction();
}

var TriggerComponentes = function (el, trigger, index) {
    switch (index) {
        case 0:
            hdComponente.setValue('None');
            BorrarComponente();
            break;
        case 1:
            storeGISComponentesGraficos.reload();
            break;
    }
}
function BorrarComponente() {
    cmbComponentes.clearValue();
    hdComponente.setValue('None');
}


function addInteraction() {
    var value = hdComponente.value;
    var iAncho = 1;
    var sColor = '#27b0e8';
    //var sIcono = hdIcono.value;
    var sIcono = '../..' + hdIcono.value;

    alert(sIcono);

    if (value !== 'None' && value !== '' && typeof value != 'undefined') {
        //Style
        var estilo;
        if (value == 'Point') {
            if (sIcono !== null && sIcono !== '' && typeof sIcono != 'undefined') {
                estilo = new ol.style.Style({
                    image: new ol.style.Icon({
                        anchor: [0.5, 46],
                        anchorXUnits: 'fraction',
                        anchorYUnits: 'pixels',
                        src: sIcono,
                    }),
                });
            } else {
                new ol.style.Style({
                    image: new ol.style.Circle({
                        radius: iAncho * 2,
                        fill: new Fill({
                            color: sColor
                        }),
                        stroke: new ol.style.Stroke({
                            color: sColor,
                            width: iAncho / 2
                        })
                    }),
                    zIndex: Infinity
                })
            }
        } else {


            if (hdColor.value !== '' && typeof hdColor.value != 'undefined') {
                sColor = hdColor.value;
            }
            if (hdAncho.value !== '' && typeof hdAncho.value != 'undefined') {
                iAncho = hdAncho.value;
            }
            alert(sColor);
            alert(iAncho);
            estilo = new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: sColor,
                    width: iAncho,
                })
            });
        }

        draw = new ol.interaction.Draw({
            source: origen,
            type: hdComponente.value,
            style: estilo
        });

        draw.on('drawend', function (event) {
            var feature = event.feature;
            alert(estilo);
            feature.setStyle(estilo);
            alert(feature.id);
        });


        map.addInteraction(draw);
    }
}

//addInteraction();

function SaveLayer() {
    var GEOJSON_PARSER = new ol.format.GeoJSON();
    var caracteristicas = vector.getSource().getFeatures();
    alert(caracteristicas);
    var vectorLayerAsJson = GEOJSON_PARSER.writeFeatures(caracteristicas);
    alert(vectorLayerAsJson);
}
