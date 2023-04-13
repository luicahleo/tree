var map;
var circle;
var markers = [];
var markerCluster;

function initMap() {
    Ext.onReady(function () {

        var centro = { lat: 37.457981, lng: -6.057108 };

        map = new google.maps.Map(document.getElementById('map'), {
            center: centro,
            zoom: 12
        });

        var marker = new google.maps.Marker({
            position: centro,
            map: map,
            animation: google.maps.Animation.DROP,
            title: ''
        });

        circle = new google.maps.Circle({
            strokeColor: "#00B596",
            strokeOpacity: 0.6,
            strokeWeight: 3,
            fillColor: "#fff",
            fillOpacity: 0.5,
            map: map,
            center: centro,
            radius: 10000
        });

    });
}

function store_load() {

    var tamIma = 32;

    if (markerCluster != undefined) {
        markerCluster.setMap(null);
    }

    if (markers != undefined) {
        for (var j = 0; j < markers.length; j++) {
            markers[j].setMap(null);
        }
    }

    markers = [];

    let radius = App.numRadio.value * 1000;
    circle.setRadius(radius);

    for (i = 0; i < App.storeEmplazamientos.data.items.length; i++) {

        var dato = App.storeEmplazamientos.data.items[i].data;
        var point;

        if (dato.Latitud == '' || dato.Longitud == '' || dato.Latitud == '0' || dato.Longitud == '0') continue;

        point = new google.maps.LatLng(dato.Latitud, dato.Longitud);

        let ico = '../../ima/mapicons/ico-' + dato.Operador + '-map.svg';
        if (!imageExists(ico)) {

            ico = '../../ima/mapicons/ico-noOperator-map.svg'
        }

        let image = new google.maps.MarkerImage(ico,
            new google.maps.Size(tamIma, tamIma),
            new google.maps.Point(0, 0),
            new google.maps.Point(0, tamIma));


        let markerFiltro = new google.maps.Marker({
            position: point,
            map: map,
            icon: image,
            id: i
        });

        //var content =
        //    '<div id="dvInfoSiteMap">' +
        //    '<span class="lblMap d-inBlk">' + jsCodigo + ':' +'</span><span id="spCode" class="d-inBlk"><a href="#" class="lnkMap">' + dato.Codigo + '</a></span></br>' +
        //    '<span class="lblMap d-inBlk">' + jsNombreSitio + ':' + '</span><span id="spSite" class="d-inBlk">' + dato.NombreSitio + '</span></br>' +
        //    '<span class="lblMap d-inBlk">' + jsOperador + ':' + '</span><span id="spSite" class="d-inBlk">' + dato.Operador + '</span></br>' +
        //    '<span class="lblMap d-inBlk">' + jsMunicipio + ':' + '</span><span id="spMunicipality" class="d-inBlk">' + dato.Municipio + '</span></br>' +
        //    '<span class="lblMap d-inBlk">' + jsCoordenadas + ':' + '</span><span id="spCoord" class="d-inBlk">' + '(' + dato.Latitud + ',' + dato.Longitud + ')' + '</span></br>' +
        //    '<span class="lblMap d-inBlk">' + jsDistancia + ':' + '</span><span id="spDistance" class="d-inBlk">' + dato.Distancia + '</span></br>' +
        //    '<span class="lblMap d-inBlk lblOperMap">' + jsCategoriaSitio + ':' + '</span><span id="spOperation" class="d-inBlk lblOperMap">' + dato.CategoriaSitio + '</span></br>' +
        //    '<span class="lblMap d-inBlk lblOperMap">' + jsTipoSitio + ':' + '</span><span id="spOperation" class="d-inBlk lblOperMap">' + dato.Tipo + '</span></br>' +
        //    '<span class="lblMap d-inBlk lblOperMap">' + jsTamanoSitio + ':' + '</span><span id="spOperation" class="d-inBlk lblOperMap">' + dato.Tamano + '</span></br>' +
        //    '<span class="lblMap d-inBlk lblEGlbMap">' + jsEstadoGlobal + ':' + '</span><span id="spGlobalState" class="d-inBlk lblEGlbMap">' + dato.EstadoGlobal + '</span>' +
        //    '</div>'; 

        var content =
            `

            <div id="dvInfoSiteMap" >

                <div id="TitleNButtons" class="TagCardtitle">
                    <span id="tltSite" class="lblMap d-inBlk ">SITE</span>
                    <span id="tltLocation" class="lblMap d-inBlk dNone ">LOCATION</span>

                    <span id="divNav">
                        <button id="btnPrev" type="button" class="btnPrev" disabled onclick="SliderSwap('Prev')"></button >
                        <button id="btnNext" type="button" class="btnNext" onclick="SliderSwap('Next')"></button >
                    </span>

                </div>





                <div id="dvSiteInformation" class="">
                   </span><span id="spCode" class=" lblMap d-inBlk"> Code: <a href="#" class="lnkMap">  ${dato.Codigo}  </a></span></br>
                <span class="lblMap d-inBlk">  Site  :  </span><span id="spSite" class="d-inBlk">  ${dato.NombreSitio}  </span></br>
            <progress id="file" value="32" max="100" class="PrgBarCard w3-round-large"> 39% </progress> </br>
                <span class="lblMap d-inBlk">  State  :   </span><span id="spGlobalState" class="d-inBlk ">  ${dato.EstadoGlobal}  </span> </br >
                    <span class="lblMap d-inBlk">  Typology  :  </span><span id="spCode" class="d-inBlk"><span class="">  "TIPOLOGIA1"  </span></span></br >
                        <span class="lblMap d-inBlk">  Department  :  </span><span id="spSite" class="d-inBlk">  ${dato.NombreSitio}  </span></br >
                            <span class="lblMap d-inBlk">  Estado Global :  </span><span id="spGlobalState" class="d-inBlk BlueBold ">  ${dato.EstadoGlobal}  </span> 

            </div >


            <div id="dvLocationInfo" class="dNone">
                <span id="spCode" class="lblMap d-inBlk">Location: <a href="#" class="lnkMap">  ${dato.Codigo}  </a></span></br>
            <span class="lblMap d-inBlk">  Site  :  </span><span id="spSite" class="d-inBlk">  ${dato.NombreSitio}  </span></br >
                <progress id="file" value="32" max="100" class="PrgBarCard w3-round-large"> 39% </progress> </br >
                    <span class="lblMap d-inBlk">  State  :   </span><span id="spGlobalState" class="d-inBlk ">  ${dato.EstadoGlobal}  </span> </br >
                        <span class="lblMap d-inBlk">  Typology  :  </span><span id="spCode" class="d-inBlk"><span class="">  "TIPOLOGIA1"  </span></span></br >
                            <span class="lblMap d-inBlk">  Department  :  </span><span id="spSite" class="d-inBlk">  ${dato.NombreSitio}  </span></br >
                                <span class="lblMap d-inBlk " >  Estado Global  :  </span><span id="spGlobalState" class="d-inBlk BlueBold ">  ${dato.EstadoGlobal}  </span> 

            </div >


            <div id="BtnsFooter" style="position: absolute; bottom:10;">

                  <button id="SatellitMode" class="BasebtnCard ico-GoogleMStreetVw StreetViewSize"></button>
                  <button id="PieCh" class="BasebtnCard ico-Piechart "></button>
                  <button id="Boxes" class="BasebtnCard ico-BoxStack"></button>
                  <button id="Notes" class="BasebtnCard ico-documentacion-simple"></button>


            </div>

            </div > `;






        markerFiltro.content = content;

        google.maps.event.addListener(markerFiltro, 'click', (function () {
            return function () {
                var infowindow = new google.maps.InfoWindow();
                infowindow.setContent(this.content);
                infowindow.open(map, this);
            }
        })());

        markers[i] = markerFiltro;
    }

    var Cluster = App.cmbClusters.getValue();

    if (Cluster != null && Cluster != "") {

        if (Cluster == "OFF") {


        } else if (Cluster == "SMALL") {

            markerCluster = new MarkerClusterer(map, markers,
                {
                    gridSize: 30,
                    imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'
                });

        } else if (Cluster == "MEDIUM") {

            markerCluster = new MarkerClusterer(map, markers,
                {
                    gridSize: 50,
                    imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'
                });

        } else if (Cluster == "LARGE") {

            markerCluster = new MarkerClusterer(map, markers,
                {
                    gridSize: 80,
                    imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'
                });
        }

    }
    else {

        markerCluster = new MarkerClusterer(map, markers,
            {
                imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'
            });
    }

}

function SeleccionarCliente() {

    App.storeOperadores.reload();
    App.storeEstadosGlobales.reload();
    App.storeCategoriasSitios.reload();
    App.storeEmplazamientosTipos.reload();
    App.storeEmplazamientosTamanos.reload();

}

function imageExists(image_url) {

    var http = new XMLHttpRequest();
    http.open('HEAD', image_url, false);
    http.send();

    return http.status != 404;
}


var getValueOperadores = function () {

    var ids = "";

    if (App.multiOperadores != undefined) {

        var values = App.multiOperadores.getSelectedValues();

        for (var i = 0; i < values.length; i++) {
            ids += values[i] + ",";
        }
        ids = ids.slice(0, -1);

    }

    return ids;
}

var getValueEstadosGlobales = function () {

    var ids = "";
    if (App.multiEstadosGlobales != undefined) {

        var values = App.multiEstadosGlobales.getSelectedValues();

        for (var i = 0; i < values.length; i++) {
            ids += values[i] + ",";
        }
        ids = ids.slice(0, -1);
    }

    return ids;

}

var getValueCategoriasSitios = function () {

    var ids = "";
    if (App.multiCategoriasSitios != undefined) {

        var values = App.multiCategoriasSitios.getSelectedValues();
        for (var i = 0; i < values.length; i++) {
            ids += values[i] + ",";
        }

        ids = ids.slice(0, -1);
    }

    return ids;
}

var getValueTiposEmplazamientos = function () {

    var ids = "";
    if (App.multiEmplazamientosTipos != undefined) {

        var values = App.multiEmplazamientosTipos.getSelectedValues();
        for (var i = 0; i < values.length; i++) {
            ids += values[i] + ",";
        }
        ids = ids.slice(0, -1);
    }

    return ids;

}

var getValueTamanos = function () {

    var ids = "";

    if (App.multiEmplazamientosTamanos != undefined) {
        var values = App.multiEmplazamientosTamanos.getSelectedValues();
        for (var i = 0; i < values.length; i++) {
            ids += values[i] + ",";
        }
        ids = ids.slice(0, -1);
    }

    return ids;

}

//CONTROL SLIDER CARD SITE
function SliderSwap(NextOrPrev) {



    if (NextOrPrev == "Next") {
        //TITULOS
        var titleEl1 = document.getElementById("tltSite");
        var titleEl2 = document.getElementById("tltLocation");

        titleEl1.classList.add("dNone");
        titleEl2.classList.remove("dNone");


        //CUERPO CONTENIDOS
        var bodyEl1 = document.getElementById("dvSiteInformation");
        var bodyEl2 = document.getElementById("dvLocationInfo");

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
        var bodyEl1 = document.getElementById("dvSiteInformation");
        var bodyEl2 = document.getElementById("dvLocationInfo");

        bodyEl1.classList.remove("dNone");
        bodyEl2.classList.add("dNone");


        //ENABLEDISABLE BOTONES
        document.getElementById("btnPrev").disabled = true;
        document.getElementById("btnNext").disabled = false;

    }






}


// #region PROTOTIPO CON SELECCION EN GOOGLEMAP API (WIP A IMPLEMENTAR)




// This example adds a search box to a map, using the Google Place Autocomplete
// feature. People can enter geographical searches. The search box will return a
// pick list containing a mix of places and predicted search terms.
// This example requires the Places library. Include the libraries=places
// parameter when you first load the API. For example:
// <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBIwzALxUPNbatRBj3Xi1Uhp0fFzwWNBkE&libraries=places">
function initAutocomplete() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 37.457981, lng: -6.057108 },
        zoom: 13,
    });
    // Create the search box and link it to the UI element.
    const input = document.getElementById("Input-Search");
    const searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
    // Bias the SearchBox results towards current map's viewport.
    map.addListener("bounds_changed", () => {
        searchBox.setBounds(map.getBounds());
    });
    let markers = [];
    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
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
            // Create a marker for each place.
            markers.push(
                new google.maps.Marker({
                    map,
                    icon,
                    title: place.name,
                    position: place.geometry.location,
                })
            );

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });
}

// #endregion