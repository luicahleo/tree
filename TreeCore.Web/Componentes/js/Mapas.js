var panorama;
var markers = [];
var markerCluster;
var infowindow;
let map;
let imgPath = 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m';
var gridSize = 50;
let searchBox;
let circle;
let circleEmplazamiento;

function initMap(sender, registro, index) {
    
    Ext.onReady(function () {

        if (document.getElementById("Input-Search") != null) {
            document.getElementById("Input-Search").placeholder = jsBuscarDireccionMapa;
        }

        var idComponente = sender.storeId.split('_')[0];

        var centro = { lat: parseFloat(App[idComponente + "_hdLatitudMapa"].value), lng: parseFloat(App[idComponente + "_hdLongitudMapa"].value) };

        if (map == null) {
            map = new google.maps.Map(document.getElementById('map'), {
                center: centro,
                zoom: parseFloat(App[idComponente + "_hdZoom"].value)
            });
        }

        if (App[idComponente + "_hdCercanos"].value != "false") {
            if (circle == null) {
                circle = new google.maps.Circle({
                    strokeColor: "#00B596",
                    strokeOpacity: 0.6,
                    strokeWeight: 3,
                    fillColor: "#fff",
                    fillOpacity: 0.5,
                    map: map,
                    center: centro,
                    radius: parseFloat(App[idComponente + "_numRadio"].value) * 1000
                });
            }
            else {
                circle.setRadius(parseFloat(App[idComponente + "_numRadio"].value) * 1000)
            }

            if (App[idComponente + '_hdEmplazamientoID'].value != null && App[idComponente + '_hdEmplazamientoID'].value != "")
            {
                if (circleEmplazamiento == null) {
                    circleEmplazamiento = new google.maps.Circle({
                        strokeColor: "#FF0033",
                        strokeOpacity: 1,
                        strokeWeight: 3,
                        fillColor: "#fff",
                        fillOpacity: 0.5,
                        map: map,
                        center: centro,
                        radius: 70
                    });
                }
            }
        }

        // Create the search box and link it to the UI element.
        if (searchBox == null) {
            const input = document.getElementById("Input-Search");
            searchBox = new google.maps.places.SearchBox(input);
            map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
        }

        if (sender.data.items.length > 0) {
            store_load(sender, registro, index);
        }
        
    });
}

function store_load(sender, registro, index) {

    var idComponente = sender.storeId.split('_')[0];

    var storeID = sender.storeId;
    var store = App[storeID];

    var tamIma = 32;

    if (markerCluster) {
        markerCluster.clearMarkers();
    }

    if (markerCluster != undefined) {
        markerCluster.setMap(null);
    }

    if (markers != undefined) {
        for (var j = 0; j < markers.length; j++) {
            if (markers[j] != undefined) {
                markers[j].setMap(null);
            }
        }
    }

    
    // Bias the SearchBox results towards current map's viewport.
    map.addListener("bounds_changed", () => {
        searchBox.setBounds(map.getBounds());
    });

    searchBox.addListener("places_changed", () => {
        const places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }
        // Clear out the old markers.
        markers.forEach((marker) => {
            marker.setMap(null);
        });
        markers = [];
        // For each place, get the icon, name and location.
        const bounds = new google.maps.LatLngBounds();
        places.forEach((place) => {
            if (!place.geometry) {
                console.log("Returned place contains no geometry");
                return;
            }
            const icon = {
                url: place.icon,
                size: new google.maps.Size(71, 71),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(17, 34),
                scaledSize: new google.maps.Size(25, 25),
            };

            bounds.extend(place.geometry.location);
        });
        map.fitBounds(bounds);
        map.setZoom(16);
    });

    markers = [];

    for (var i = 0; i < store.data.items.length; i++) {
        var dato = store.data.items[i].data.site;
        let imagePath = store.data.items[i].data.image;
        var point;

        if (dato == undefined || dato.Latitud == undefined || dato.Longitud == undefined || dato.Latitud == '' || dato.Longitud == '' || dato.Latitud == '0' || dato.Longitud == '0') continue;

        point = new google.maps.LatLng(dato.Latitud, dato.Longitud);

        let image = new google.maps.MarkerImage(imagePath,
            new google.maps.Size(tamIma, tamIma),
            new google.maps.Point(0, 0),
            new google.maps.Point(tamIma / 2, tamIma / 2));


        let markerFiltro = new google.maps.Marker({
            icon: image,
            id: i,
            position: point,
            map: map,
            animation: google.maps.Animation.DROP,
            title: dato.Codigo
        });

        var content =
            `

            <div id="dvInfoSiteMap" class="dvInfoSiteMap-${dato.EmplazamientoID}" >

                <div id="TitleNButtons" class="TagCardtitle">
                    <span id="tltSite" class="lblMap d-inBlk ">`+ jsEmplazamiento + `</span>
                    <span id="tltLocation" class="lblMap d-inBlk dNone ">`+ jsLocalizacion + `</span>

                    <span id="divNav">
                        <button id="btnPrev" type="button" class="btnPrev" disabled onclick="SliderSwap('dvInfoSiteMap-${dato.EmplazamientoID}', 'Prev')"></button >
                        <button id="btnNext" type="button" class="btnNext" onclick="SliderSwap('dvInfoSiteMap-${dato.EmplazamientoID}', 'Next')"></button >
                    </span>

                </div>


                <div id="dvSiteInformation" class="dvSiteInformation">

                    <span id="spCode" class=" lblMap d-inBlk"> `+ jsCodigo + `: <a href="#" class="lnkMap">  ${dato.Codigo}  </a></span></br>
                    <span class="lblMap d-inBlk">  `+ jsEmplazamiento + `:  </span><span id="spSite" class="d-inBlk">  ${dato.NombreSitio}  </span></br>
                    <span class="lblMap d-inBlk">  `+ jsEstadoGlobal + `:   </span><span id="spGlobalState" class="d-inBlk ">  ${dato.EstadoGlobal}  </span> </br >
                    <span class="lblMap d-inBlk">  `+ jsCategoria + `:  </span><span id="spCategoriaSitio" class="d-inBlk"><span class="">  ${dato.CategoriaSitio}  </span></span></br >
                    <span class="lblMap d-inBlk">  `+ jsTipoEdificio + `:  </span><span id="spTipoEdificio" class="d-inBlk"><span class="">  ${dato.TipoEdificio}  </span></span></br >

                </div >


                <div id="dvLocationInfo" class="dNone dvLocationInfo">`
            + ((dato.Codigo != "") ? `<span id="spCode" class="lblMap d-inBlk">` + jsCodigo + `: <a href="#" class="lnkMap">  ${dato.Codigo}  </a></span></br>` : ``)
            + ((dato.NombreSitio != "") ? `<span class="lblMap d-inBlk">  ` + jsEmplazamiento + `:  </span><span id="spSite" class="d-inBlk">  ${dato.NombreSitio}  </span></br > ` : ``)
            + ((dato.Region != "") ? `<span class="lblMap d-inBlk">  ` + jsRegion + `  :   </span><span id="spRegion" class="d-inBlk ">  ${dato.Region}  </span> </br >` : ``)
            + ((dato.RegionPais != "") ? `<span class="lblMap d-inBlk">  ` + jsRegionPais + `  :   </span><span id="spRegionPais" class="d-inBlk ">  ${dato.RegionPais}  </span> </br >` : ``)
            + ((dato.Provincia != "") ? `<span class="lblMap d-inBlk">  ` + jsProvincia + `  :   </span><span id="spProvincia" class="d-inBlk ">  ${dato.Provincia}  </span> </br >` : ``)
            + ((dato.Municipio != "") ? `<span class="lblMap d-inBlk">  ` + jsMunicipio + `  :   </span><span id="spMunicipio" class="d-inBlk ">  ${dato.Municipio}  </span> </br >` : ``)
            + ((dato.Barrio != "") ? `<span class="lblMap d-inBlk">  ` + jsBarrio + `  :   </span><span id="spBarrio" class="d-inBlk ">  ${dato.Barrio}  </span> </br >` : ``)
            + ((dato.Direccion != "") ? `<span class="lblMap d-inBlk lblNoMargin">  ` + jsDireccion + `  :   </span><span id="spDireccion" class="d-inBlk ">  ${dato.Direccion}  </span> </br >` : ``)
             + `
                </div >

 
                <div id="BtnsFooter">
                      
                      <button id="SatellitMode" class="BasebtnCard ico-GoogleMStreetVw StreetViewSize btn-streetview" type="button" onclick="showInStreetView(${dato.Latitud}, ${dato.Longitud})" data-latitud="${dato.Latitud}" data-longitud="${dato.Longitud}"></button>
                      <button id="PieCh" class="BasebtnCard ico-Piechart" type="button" onclick="showDashboard(${dato.EmplazamientoID})"></button>
                      <button id="Boxes" class="BasebtnCard ico-BoxStack" type="button" onclick="showInventarioGestion(${dato.EmplazamientoID})"></button>
                      <button id="Notes" class="BasebtnCard ico-documentacion-simple" type="button" onclick="showDocumentos(${dato.EmplazamientoID})"></button>

                </div>

            </div > `;

        markerFiltro.content = content;

        google.maps.event.addListener(markerFiltro, 'click', (function () {
            return function () {
                if (infowindow != undefined && infowindow != null) {
                    infowindow.close();
                }
                infowindow = new google.maps.InfoWindow();
                infowindow.setContent(this.content);
                infowindow.open(map, this);
            }
        })());

        markers[i] = markerFiltro;
    }

    if (App[idComponente + "_hdCercanos"].value != "false") {

        if (App[idComponente + "_cmbClusters"].store.data.items.length == 0) {
            CargarStoresSerie([App[idComponente + "_cmbClusters"].store], function () {
                clusterEmplazamientosCercanos(idComponente, markers);
                App[idComponente + "_cmbClusters"].setValue('Medium');
            });
        }
        else {
            clusterEmplazamientosCercanos(idComponente, markers);
        }
    }
    else {
        markerCluster = new MarkerClusterer(map, markers, {
            imagePath: imgPath,
            gridSize: gridSize,
            zoomOnClick: false
        });
    }

    if (markerCluster != undefined) {
        google.maps.event.addListener(markerCluster, "click", function (c) {
            var m = c.getMarkers();
            if (m.length > 1 && map.getZoom() == 22) {
                let position;
                let n = 1;

                for (let i = 0; i < m.length; i++) {

                    let infowindow = new google.maps.InfoWindow();
                    infowindow.setContent(m[i].content);
                    infowindow.open(map, m[i]);
                    if (position != null) {
                        infowindow.setPosition({ lat: (position.lat() + 0.000005), lng: (position.lng() + 0.000005) })
                    }
                    position = infowindow.getPosition();
                    infowindow.setZIndex(n);
                }
            }
        });
    }
    
}


function clusterEmplazamientosCercanos(idComponente, markers) {
    var Cluster = App[idComponente + "_cmbClusters"].getValue();
    

    if (Cluster != null && Cluster != "") {

        Cluster = Cluster.toUpperCase();

        if (Cluster != "OFF") {

            if (Cluster == "SMALL") {
                gridSize = 30;
            }

            if (Cluster == "MEDIUM") {
                gridSize = 50;
            }

            if (Cluster == "LARGE") {
                gridSize = 80;
            }

            if (markerCluster == null) {

                markerCluster = new MarkerClusterer(map, markers, {
                    imagePath: imgPath,
                    gridSize: gridSize,
                    zoomOnClick: false
                });
            }
            else {
                markerCluster.setGridSize(gridSize)
            }
            

            markerCluster.setMap(map);
        }
        markerCluster.addMarkers(markers);
    }
    else {
        markerCluster = new MarkerClusterer(map, markers, {
            imagePath: imgPath,
            gridSize: gridSize,
            zoomOnClick: false
        });
    }
}

function toggleStreetView() {
    if (panorama != undefined) {
        const toggle = panorama.getVisible();

        if (toggle == false) {
            panorama.setVisible(true);
        } else {
            panorama.setVisible(false);
            let btn = document.getElementById("floating-panel");
            btn.style.display = "none";
        }
    }
}

function showInStreetView(latitud, longitud) {
    const fenway = { lat: latitud, lng: longitud };

    panorama = new google.maps.StreetViewPanorama(
        document.getElementById("pano"),
        {
            position: fenway,
            pov: {
                heading: 34,
                pitch: 10,
            },
            addressControlOptions: {
                position: google.maps.ControlPosition.BOTTOM_CENTER,
            },
            linksControl: false,
            panControl: false,
            enableCloseButton: true,
        }
    );
    map.setStreetView(panorama);
    let btn = document.getElementById("floating-panel");
    btn.style.display = "initial";
}

function showInventarioGestion(emplazamientoId) {
    parent.addTab(parent.App.tabPpal, jsInventario + emplazamientoId, jsInventario, "../ModInventario/pages/InventarioGestionContenedor.aspx?EmplazamientoID=" + emplazamientoId);
}

function showDocumentos(emplazamientoId) {

}

function showDashboard(emplazamientoId) {

}

function imageExists(image_url) {

    var http = new XMLHttpRequest();
    http.open('HEAD', image_url, false);
    http.send();

    return http.status != 404;
}

//CONTROL SLIDER CARD SITE
function SliderSwap(classPane, NextOrPrev) {

    if (NextOrPrev == "Next") {
        //TITULOS
        var titleEl1 = document.getElementById("tltSite");
        var titleEl2 = document.getElementById("tltLocation");

        titleEl1.classList.add("dNone");
        titleEl2.classList.remove("dNone");

        //CUERPO CONTENIDOS
        var bodyEl1 = document.getElementsByClassName(classPane)[0].getElementsByClassName("dvSiteInformation")[0];
        var bodyEl2 = document.getElementsByClassName(classPane)[0].getElementsByClassName("dvLocationInfo")[0];

        bodyEl1.classList.add("dNone");
        bodyEl2.classList.remove("dNone");

        //ENABLEDISABLE BOTONES
        document.getElementById("btnPrev").disabled = false;
        document.getElementById("btnNext").disabled = true;
        



    }

    if (NextOrPrev == "Prev") {
        //TITULOS
        var titleEl1 = document.getElementById("tltSite");
        var titleEl2 = document.getElementById("tltLocation");

        titleEl1.classList.remove("dNone");
        titleEl2.classList.add("dNone");

        //CUERPO CONTENIDOS
        var bodyEl1 = document.getElementsByClassName(classPane)[0].getElementsByClassName("dvSiteInformation")[0];
        var bodyEl2 = document.getElementsByClassName(classPane)[0].getElementsByClassName("dvLocationInfo")[0];

        bodyEl1.classList.remove("dNone");
        bodyEl2.classList.add("dNone");

        //ENABLEDISABLE BOTONES
        document.getElementById("btnPrev").disabled = true;
        document.getElementById("btnNext").disabled = false;
    }
}


function SeleccionarCluster() {
    this.getTrigger(0).show();

    var Panel = this.up("formpanel");
    var combosFiltrados = [];
    for (var i = 1; i < Panel.items.items.length; i++) {
        combosFiltrados[i - 1] = Panel.items.items[i];
    }
}

function RecargarCluster() {
    var Panel = this.up("formpanel");
    var combosFiltrados = Panel.items.items;
    recargarCombos(combosFiltrados);
}

function RecargarEmplazamientos(sender, registro, index) {

    var ruta = getIdComponente(sender);
    App[ruta + '_storeEmplazamientos'].reload();
}

// #endregion

function validarFilterMap(sender, registro, index) {
    let ruta = sender.owner.id.split('_'); ruta.pop(); ruta = ruta.join('_');

    if (registro) {
        App[ruta + '_btnAplicar'].enable();
    }
    else {
        App[ruta + '_btnAplicar'].disable();
    }

}
