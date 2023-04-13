// #region VARIABLES
let centro;
let promesa;
let direccion;
let Latitud = "";
let Longitud = "";
let dragLatitud = null;
let dragLongitud = null;
let dragDireccion = "";
let dragMunicipio;
let dragBarrio;
let dragCodigoPostal;
let latitudPadre;
let longitudPadre;
let mapID;
let markersForm = [];
let numero = "";
let calle = "";
let municipio = "";
let address = "";
let barrio = "";
let codigoPostal = "";
let paisCodPadre;

// #endregion

function showMap(sender, registro, index) {

    const geocoder = new google.maps.Geocoder();
    let idComponente = getIdComponente(sender);

    Latitud = "";
    Longitud = "";
    dragLatitud = null;
    dragLongitud = null;

    // #region LATITUD, LONGITUD
    if (idComponente == "UCGridEmplazamientosLocalizaciones") {

        latitudPadre = App[idComponente + "_geoPosicion_hdLatitudPadre"].value;
        longitudPadre = App[idComponente + "_geoPosicion_hdLongitudPadre"].value;
        paisCodPadre = App[idComponente + "_geoPosicion_hdPaisCodPadre"].value;

        if (index.data.Latitud != 0 && index.data.Longitud != 0) {
            Latitud = index.data.Latitud;
            Longitud = index.data.Longitud;
        }
        if (index.data.Direccion != "") {
            direccion = index.data.Direccion;
        }

        mapID = document.getElementById(App[idComponente + "_geoPosicion_hdNombreMapa"].value);
        App[idComponente + "_geoPosicion_winPosicionar"].show();

    }
    else {

        latitudPadre = App[idComponente + "_hdLatitudPadre"].value;
        longitudPadre = App[idComponente + "_hdLongitudPadre"].value;
        paisCodPadre = App[idComponente + "_hdPaisCodPadre"].value;

        Latitud = App[idComponente + "_txtLatitud"].getValue();
        Longitud = App[idComponente + "_txtLongitud"].getValue();
        direccion = App[idComponente + "_txtDireccion"].getValue();
        barrio = App[idComponente + "_txtBarrio"].getValue();
        codigoPostal = App[idComponente + "_txtCodigoPostal"].getValue();

        mapID = document.getElementById(App[idComponente + "_hdNombreMapa"].value);
        App[idComponente + "_winPosicionar"].show();
    }

    if ((Latitud != "" && Longitud != "") || direccion != "") {

        if (Latitud != "" && Longitud != "") {
            App[idComponente + "_hdLatitudForm"].setValue(Latitud);
            App[idComponente + "_hdLongitudForm"].setValue(Longitud);
        }
        else {

            App[idComponente + "_hdLatitudForm"].setValue(latitudPadre);
            App[idComponente + "_hdLongitudForm"].setValue(longitudPadre);

            geocoder.geocode({ address: direccion }, (results, status) => {
                if (status === "OK") {
                    const Latitud = results[0].geometry.location.lat();
                    const Longitud = results[0].geometry.location.lng();

                    centro = { lat: parseFloat(Latitud), lng: parseFloat(Longitud) };
                }
                else {
                    centro = {
                        lat: parseFloat(App[idComponente + "_hdLatitudForm"].getValue()),
                        lng: parseFloat(App[idComponente + "_hdLongitudForm"].getValue())
                    };
                }
            });
        }

    }
    else {

        App[idComponente + "_hdLatitudForm"].setValue(latitudPadre);
        App[idComponente + "_hdLongitudForm"].setValue(longitudPadre);
    }

    // #endregion

    markersForm.forEach((marker) => {
        marker.setMap(null);
    });
    markersForm = [];

    if (document.getElementById("Input-Search") != null) {

        document.getElementById("Input-Search").value = "";
        document.getElementById("Input-Search").placeholder = jsBuscarDireccionMapa;
        document.getElementById("btnFijar").value = jsFijar;
    }

    // #region CENTRO

    pCentro = new Promise((resolve, reject) => {
        if (direccion != "" && Latitud === "" && Longitud === "") {

            geocoder.geocode({ address: direccion }, (results, status) => {
                if (status === "OK") {
                    const Latitud = results[0].geometry.location.lat();
                    const Longitud = results[0].geometry.location.lng();

                    centro = { lat: parseFloat(Latitud), lng: parseFloat(Longitud) };
                }
                else {
                    centro = {
                        lat: parseFloat(App[idComponente + "_hdLatitudForm"].getValue()),
                        lng: parseFloat(App[idComponente + "_hdLongitudForm"].getValue())
                    };
                }

                resolve(centro);
            });

        }
        else {
            centro = {
                lat: parseFloat(App[idComponente + "_hdLatitudForm"].getValue()),
                lng: parseFloat(App[idComponente + "_hdLongitudForm"].getValue())
            };
            resolve(centro);
        }
    });

    // #endregion

    pCentro.then((centro) => {

        if (mapForm[mapID.id] === undefined) {
            mapForm[mapID.id] = new google.maps.Map(document.getElementById(mapID.id), {
                center: centro,
                zoom: parseFloat(App[idComponente + "_hdZoom"].value)
            });

        }

        var marker = new google.maps.Marker({
            position: centro,
            map: mapForm[mapID.id],
            draggable: true
        });

        // #region DRAGEND

        marker.addListener('dragend', function (evt) {
            dragLatitud = evt.latLng.lat().toPrecision('6');
            dragLongitud = evt.latLng.lng().toPrecision('6');

            pBusqueda = new Promise((resolve, reject) => {
                geocoder.geocode({
                    latLng: evt.latLng
                }, function (responses) {
                    if (responses && responses.length > 0) {

                        for (let i = 0; i < responses[0].address_components.length; i++) {

                            address = responses[0].address_components[i];

                            if (address.types.includes('street_number')) {
                                numero = address.short_name;
                            }
                            else if (address.types.includes('route')) {
                                calle = address.long_name;
                            }
                            else if (address.types.includes('locality')) {
                                municipio = address.long_name;
                            }
                            else if (address.types.includes('postal_code')) {
                                codigoPostal = address.long_name;
                            }
                            else if (address.types.includes('neighborhood')) {
                                barrio = address.long_name;
                            }
                            else if (address.types.includes('postal_town')) {
                                municipio = address.long_name;
                            }
                        }

                        dragDireccion = calle + ', ' + numero;
                        dragBarrio = barrio;
                        dragCodigoPostal = codigoPostal;
                        dragMunicipio = municipio;

                        if (municipio != "") {
                            TreeCore[idComponente].BusquedaMunicipio(municipio, {
                                success: function (result) {
                                    if (!result.Success) {
                                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                    }
                                    else {
                                        bExiste = result.Result.split(',')[0];
                                        provincia = result.Result.split(',')[1];
                                        paisCod = result.Result.split(',')[2];

                                        if (bExiste == "True") {
                                            if (provincia != "" && paisCod != "") {
                                                dragMunicipio = municipio + ', ' + provincia + ' (' + paisCod + ')';
                                            }
                                            else {
                                                dragMunicipio = municipio;
                                            }
                                        }
                                        else {
                                            dragMunicipio = "";
                                        }

                                        resolve(dragMunicipio);
                                    }
                                }
                            });
                        }

                        else {
                            resolve(dragMunicipio);
                        }
                    }
                    else {
                        marker.formatted_address = jsDireccionNoEncontrada;
                    }
                });
            });

            pBusqueda.then((dragMunicipio) => { });

        });

        // #endregion

        markersForm.push(marker);
        mapForm[mapID.id].setCenter(centro);


        // #region BUSQUEDA

        // Create the search box and link it to the UI element.
        let input = document.getElementById("Input-Search");
        let searchBox = new google.maps.places.SearchBox(input);
        mapForm[mapID.id].controls[google.maps.ControlPosition.TOP_LEFT].push(input);
        // Bias the SearchBox results towards current map's viewport.
        mapForm[mapID.id].addListener("bounds_changed", () => {
            searchBox.setBounds(mapForm[mapID.id].getBounds());
        });

        searchBox.addListener("places_changed", () => {

            $('.pac-container')[0].style.zIndex = 100000;

            const places = searchBox.getPlaces();

            if (places.length == 0) {
                return;
            }
            // Clear out the old markers.
            markersForm.forEach((marker) => {
                marker.setMap(null);
            });
            markersForm = [];
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

                var marker = new google.maps.Marker({
                    position: place.geometry.location,
                    map: mapForm[mapID.id],
                    draggable: true
                });

                // #region DRAGEND

                marker.addListener('dragend', function (evt) {
                    dragLatitud = evt.latLng.lat().toPrecision('6');
                    dragLongitud = evt.latLng.lng().toPrecision('6');

                    pDragend = new Promise((resolve, reject) => {
                        geocoder.geocode({
                            latLng: evt.latLng
                        }, function (responses) {
                            if (responses && responses.length > 0) {

                                for (let i = 0; i < responses[0].address_components.length; i++) {

                                    address = responses[0].address_components[i];

                                    if (address.types.includes('street_number')) {
                                        numero = address.short_name;
                                    }
                                    else if (address.types.includes('route')) {
                                        calle = address.long_name;
                                    }
                                    else if (address.types.includes('locality')) {
                                        municipio = address.long_name;
                                    }
                                    else if (address.types.includes('postal_code')) {
                                        codigoPostal = address.long_name;
                                    }
                                    else if (address.types.includes('neighborhood')) {
                                        barrio = address.long_name;
                                    }
                                    else if (address.types.includes('postal_town')) {
                                        municipio = address.long_name;
                                    }
                                }

                                dragDireccion = calle + ', ' + numero;
                                dragBarrio = barrio;
                                dragCodigoPostal = codigoPostal;

                                if (municipio != "") {
                                    TreeCore[idComponente].BusquedaMunicipio(municipio, {
                                        success: function (result) {
                                            if (!result.Success) {
                                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                            }
                                            else {
                                                bExiste = result.Result.split(',')[0];
                                                provincia = result.Result.split(',')[1];
                                                paisCod = result.Result.split(',')[2];

                                                if (bExiste == "True") {
                                                    if (provincia != "" && paisCod != "") {
                                                        dragMunicipio = municipio + ', ' + provincia + ' (' + paisCod + ')';
                                                    }
                                                    else {
                                                        dragMunicipio = municipio;
                                                    }
                                                }
                                                else {
                                                    dragMunicipio = "";
                                                }

                                                resolve(dragMunicipio);
                                            }
                                        }
                                    });
                                }

                                else {
                                    resolve(dragMunicipio);
                                }
                            }
                            else {
                                marker.formatted_address = jsDireccionNoEncontrada;
                            }
                        });
                    });

                    pDragend.then((dragMunicipio) => { });
                });

                // #endregion

                markersForm.push(marker);
            });

            mapForm[mapID.id].fitBounds(bounds);
            mapForm[mapID.id].setZoom(16);
        });

        // #endregion

        // #region FIJAR

        let fijar = document.getElementById("btnFijar");

        if (fijar != null) {

            fijar.hidden = false;
            mapForm[mapID.id].controls[google.maps.ControlPosition.TOP_RIGHT].push(fijar);

            fijar.addEventListener("click", () => {
                var obj;
                if (App.formLocation) {
                    obj = App.formLocation;
                } else {
                    obj = App[getIdComponentePadre(sender) + "_formLocation"];
                }
                showLoadMask(obj, function (load) {
                    let searchPlaces = searchBox.getPlaces();

                    if (dragLatitud === null && dragLongitud === null) {

                        if (searchPlaces != undefined) {
                            if (searchPlaces.length > 0) {

                                var direccion = searchPlaces[0].geometry.location;
                                var formatDir = searchPlaces[0].formatted_address;

                                // #region DRAGMUNICIPIO

                                pDragMunicipio = new Promise((resolve, reject) => {
                                    geocoder.geocode({
                                        latLng: direccion
                                    }, function (responses) {
                                        if (responses && responses.length > 0) {

                                            for (let i = 0; i < responses[0].address_components.length; i++) {

                                                address = responses[0].address_components[i];

                                                if (address.types.includes('street_number')) {
                                                    numero = address.short_name;
                                                }
                                                else if (address.types.includes('route')) {
                                                    calle = address.long_name;
                                                }
                                                else if (address.types.includes('locality')) {
                                                    municipio = address.long_name;
                                                }
                                                else if (address.types.includes('postal_code')) {
                                                    codigoPostal = address.long_name;
                                                }
                                                else if (address.types.includes('neighborhood')) {
                                                    barrio = address.long_name;
                                                }
                                                else if (address.types.includes('postal_town')) {
                                                    municipio = address.long_name;
                                                }
                                            }

                                            if (calle != "" || numero != "") {
                                                dragDireccion = calle + ', ' + numero;
                                            }

                                            dragBarrio = barrio;
                                            dragCodigoPostal = codigoPostal;
                                            dragMunicipio = municipio;

                                            if (municipio != "") {
                                                TreeCore[idComponente].BusquedaMunicipio(municipio, {
                                                    success: function (result) {
                                                        if (!result.Success) {
                                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                                        }
                                                        else {
                                                            bExiste = result.Result.split(',')[0];
                                                            provincia = result.Result.split(',')[1];
                                                            paisCod = result.Result.split(',')[2];

                                                            if (bExiste == "True") {
                                                                if (provincia != "" && paisCod != "") {
                                                                    dragMunicipio = municipio + ', ' + provincia + ' (' + paisCod + ')';
                                                                }
                                                                else {
                                                                    dragMunicipio = municipio;
                                                                }
                                                            }
                                                            else {
                                                                dragMunicipio = "";
                                                            }

                                                            resolve(dragMunicipio);
                                                        }
                                                    }
                                                });
                                            }
                                            else {
                                                resolve(dragMunicipio);
                                            }

                                            searchPlaces.pop();
                                        }
                                    });

                                });

                                pDragMunicipio.then((dragMunicipio) => {

                                    const Latitud = direccion.lat().toPrecision('6');
                                    const Longitud = direccion.lng().toPrecision('6');

                                    App[idComponente + "_txtLatitud"].setValue(Latitud);
                                    App[idComponente + "_txtLongitud"].setValue(Longitud);
                                    App[idComponente + "_txtBarrio"].setValue(dragBarrio);
                                    App[idComponente + "_txtCodigoPostal"].setValue(dragCodigoPostal);

                                    App[idComponente + "_cmbMunicipioProvincia"].setValue(dragMunicipio);

                                    if (dragDireccion != "") {
                                        App[idComponente + "_txtDireccion"].setValue(dragDireccion);
                                    }
                                    else {
                                        App[idComponente + "_txtDireccion"].setValue(formatDir);
                                    }

                                    searchPlaces.pop();
                                    load.hide();
                                });

                                // #endregion
                            }
                            else {
                                App[idComponente + "_txtLatitud"].setValue(App[idComponente + "_hdLatitudForm"].getValue());
                                App[idComponente + "_txtLongitud"].setValue(App[idComponente + "_hdLongitudForm"].getValue());

                                direccion = (App[idComponente + "_hdLatitudForm"].getValue() + ', ' + App[idComponente + "_hdLongitudForm"].getValue());

                                // #region DRAGMUNICIPIO

                                pBuscarClick = new Promise((resolve, reject) => {
                                    geocoder.geocode({
                                        address: direccion
                                    }, function (responses) {
                                        if (responses && responses.length > 0) {

                                            for (let i = 0; i < responses[0].address_components.length; i++) {

                                                address = responses[0].address_components[i];

                                                if (address.types.includes('street_number')) {
                                                    numero = address.short_name;
                                                }
                                                else if (address.types.includes('route')) {
                                                    calle = address.long_name;
                                                }
                                                else if (address.types.includes('locality')) {
                                                    municipio = address.long_name;
                                                }
                                                else if (address.types.includes('postal_code')) {
                                                    codigoPostal = address.long_name;
                                                }
                                                else if (address.types.includes('neighborhood')) {
                                                    barrio = address.long_name;
                                                }
                                                else if (address.types.includes('postal_town')) {
                                                    municipio = address.long_name;
                                                }
                                            }

                                            if (calle != "" || numero != "") {
                                                dragDireccion = calle + ', ' + numero;
                                            }

                                            dragBarrio = barrio;
                                            dragCodigoPostal = codigoPostal;
                                            dragMunicipio = municipio;

                                            if (municipio != "") {
                                                TreeCore[idComponente].BusquedaMunicipio(municipio, {
                                                    success: function (result) {
                                                        if (!result.Success) {
                                                            Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                                        }
                                                        else {
                                                            bExiste = result.Result.split(',')[0];
                                                            provincia = result.Result.split(',')[1];
                                                            paisCod = result.Result.split(',')[2];

                                                            if (bExiste == "True") {
                                                                if (provincia != "" && paisCod != "") {
                                                                    dragMunicipio = municipio + ', ' + provincia + ' (' + paisCod + ')';
                                                                }
                                                                else {
                                                                    dragMunicipio = municipio;
                                                                }
                                                            }
                                                            else {
                                                                dragMunicipio = "";
                                                            }

                                                            resolve(dragMunicipio);
                                                        }
                                                    }
                                                });
                                            }

                                            else {
                                                resolve(dragMunicipio);
                                            }
                                        }
                                    });

                                });

                                pBuscarClick.then((dragMunicipio) => {

                                    App[idComponente + "_cmbMunicipioProvincia"].setValue(dragMunicipio);
                                    App[idComponente + "_txtDireccion"].setValue(dragDireccion);
                                    App[idComponente + "_txtBarrio"].setValue(dragBarrio);
                                    App[idComponente + "_txtCodigoPostal"].setValue(dragCodigoPostal);

                                    load.hide();
                                });

                                // #endregion
                            }
                        }
                        else {
                            App[idComponente + "_txtLatitud"].setValue(App[idComponente + "_hdLatitudForm"].getValue());
                            App[idComponente + "_txtLongitud"].setValue(App[idComponente + "_hdLongitudForm"].getValue());

                            if (App[idComponente + "_txtDireccion"].getValue() != "") {
                                direccion = App[idComponente + "_txtDireccion"].getValue();
                            }
                            else {
                                direccion = (App[idComponente + "_hdLatitudForm"].getValue() + ', ' + App[idComponente + "_hdLongitudForm"].getValue());
                            }

                            // #region DRAGMUNICIPIO

                            pBuscarFijar = new Promise((resolve, reject) => {
                                geocoder.geocode({
                                    address: direccion
                                }, function (responses) {
                                    if (responses && responses.length > 0) {

                                        for (let i = 0; i < responses[0].address_components.length; i++) {

                                            address = responses[0].address_components[i];

                                            if (address.types.includes('street_number')) {
                                                numero = address.short_name;
                                            }
                                            else if (address.types.includes('route')) {
                                                calle = address.long_name;
                                            }
                                            else if (address.types.includes('locality')) {
                                                municipio = address.long_name;
                                            }
                                            else if (address.types.includes('postal_code')) {
                                                codigoPostal = address.long_name;
                                            }
                                            else if (address.types.includes('neighborhood')) {
                                                barrio = address.long_name;
                                            }
                                            else if (address.types.includes('postal_town')) {
                                                municipio = address.long_name;
                                            }
                                        }

                                        if (calle != "" || numero != "") {
                                            dragDireccion = calle + ', ' + numero;
                                        }

                                        dragBarrio = barrio;
                                        dragCodigoPostal = codigoPostal;
                                        dragMunicipio = municipio;
                                        latitud = responses[0].geometry.location.lat().toPrecision("6");
                                        longitud = responses[0].geometry.location.lng().toPrecision("6");

                                        if (municipio != "") {
                                            TreeCore[idComponente].BusquedaMunicipio(municipio, {
                                                success: function (result) {
                                                    if (!result.Success) {
                                                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                                    }
                                                    else {
                                                        bExiste = result.Result.split(',')[0];
                                                        provincia = result.Result.split(',')[1];
                                                        paisCod = result.Result.split(',')[2];

                                                        if (bExiste == "True") {
                                                            if (provincia != "" && paisCod != "") {
                                                                dragMunicipio = municipio + ', ' + provincia + ' (' + paisCod + ')';
                                                            }
                                                            else {
                                                                dragMunicipio = municipio;
                                                            }
                                                        }
                                                        else {
                                                            dragMunicipio = "";
                                                        }

                                                        resolve(dragMunicipio);
                                                    }
                                                }
                                            });
                                        }

                                        else {
                                            resolve(dragMunicipio);
                                        }
                                    }
                                });

                            });

                            pBuscarFijar.then((dragMunicipio) => {

                                App[idComponente + "_cmbMunicipioProvincia"].setValue(dragMunicipio);
                                App[idComponente + "_txtDireccion"].setValue(dragDireccion);
                                App[idComponente + "_txtBarrio"].setValue(dragBarrio);
                                App[idComponente + "_txtCodigoPostal"].setValue(dragCodigoPostal);
                                App[idComponente + "_txtLatitud"].setValue(latitud);
                                App[idComponente + "_txtLongitud"].setValue(longitud);

                                load.hide();
                            });

                            // #endregion
                        }

                    }

                    else if (dragLatitud != undefined && dragLongitud != undefined && dragDireccion != undefined
                        && dragBarrio != undefined && dragCodigoPostal != undefined) {

                        App[idComponente + "_txtLatitud"].setValue(dragLatitud);
                        App[idComponente + "_txtLongitud"].setValue(dragLongitud);

                        direccion = (dragLatitud + ', ' + dragLongitud);

                        // #region DRAGMUNICIPIO

                        pDragFijar = new Promise((resolve, reject) => {
                            geocoder.geocode({
                                address: direccion
                            }, function (responses) {
                                if (responses && responses.length > 0) {

                                    for (let i = 0; i < responses[0].address_components.length; i++) {

                                        address = responses[0].address_components[i];

                                        if (address.types.includes('street_number')) {
                                            numero = address.short_name;
                                        }
                                        else if (address.types.includes('route')) {
                                            calle = address.long_name;
                                        }
                                        else if (address.types.includes('locality')) {
                                            municipio = address.long_name;
                                        }
                                        else if (address.types.includes('postal_code')) {
                                            codigoPostal = address.long_name;
                                        }
                                        else if (address.types.includes('neighborhood')) {
                                            barrio = address.long_name;
                                        }
                                        else if (address.types.includes('postal_town')) {
                                            municipio = address.long_name;
                                        }
                                    }

                                    if (calle != "" || numero != "") {
                                        dragDireccion = calle + ', ' + numero;
                                    }

                                    dragBarrio = barrio;
                                    dragCodigoPostal = codigoPostal;
                                    dragMunicipio = municipio;

                                    if (municipio != "") {
                                        TreeCore[idComponente].BusquedaMunicipio(municipio, {
                                            success: function (result) {
                                                if (!result.Success) {
                                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                                }
                                                else {
                                                    bExiste = result.Result.split(',')[0];
                                                    provincia = result.Result.split(',')[1];
                                                    paisCod = result.Result.split(',')[2];

                                                    if (bExiste == "True") {
                                                        if (provincia != "" && paisCod != "") {
                                                            dragMunicipio = municipio + ', ' + provincia + ' (' + paisCod + ')';
                                                        }
                                                        else {
                                                            dragMunicipio = municipio;
                                                        }
                                                    }
                                                    else {
                                                        dragMunicipio = "";
                                                    }

                                                    resolve(dragMunicipio);
                                                }
                                            }
                                        });
                                    }

                                    else {
                                        resolve(dragMunicipio);
                                    }
                                }
                            });

                        });

                        pDragFijar.then((dragMunicipio) => {

                            App[idComponente + "_txtDireccion"].setValue(dragDireccion);
                            App[idComponente + "_txtBarrio"].setValue(dragBarrio);
                            App[idComponente + "_txtCodigoPostal"].setValue(dragCodigoPostal);
                            App[idComponente + "_cmbMunicipioProvincia"].setValue(dragMunicipio);

                            load.hide();
                        });

                        // #endregion
                    }

                    else {

                        App[idComponente + "_txtLatitud"].setValue(App[idComponente + "_hdLatitudForm"].getValue());
                        App[idComponente + "_txtLongitud"].setValue(App[idComponente + "_hdLongitudForm"].getValue());

                        direccion = (App[idComponente + "_hdLatitudForm"].getValue() + ', ' + App[idComponente + "_hdLongitudForm"].getValue());

                        // #region DRAGMUNICIPIO

                        pFijar = new Promise((resolve, reject) => {
                            geocoder.geocode({
                                address: direccion
                            }, function (responses) {
                                if (responses && responses.length > 0) {

                                    for (let i = 0; i < responses[0].address_components.length; i++) {

                                        address = responses[0].address_components[i];

                                        if (address.types.includes('street_number')) {
                                            numero = address.short_name;
                                        }
                                        else if (address.types.includes('route')) {
                                            calle = address.long_name;
                                        }
                                        else if (address.types.includes('locality')) {
                                            municipio = address.long_name;
                                        }
                                        else if (address.types.includes('postal_code')) {
                                            codigoPostal = address.long_name;
                                        }
                                        else if (address.types.includes('neighborhood')) {
                                            barrio = address.long_name;
                                        }
                                        else if (address.types.includes('postal_town')) {
                                            municipio = address.long_name;
                                        }
                                    }

                                    if (calle != "" || numero != "") {
                                        dragDireccion = calle + ', ' + numero;
                                    }

                                    dragBarrio = barrio;
                                    dragCodigoPostal = codigoPostal;
                                    dragMunicipio = municipio;

                                    if (municipio != "") {
                                        TreeCore[idComponente].BusquedaMunicipio(municipio, {
                                            success: function (result) {
                                                if (!result.Success) {
                                                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                                                }
                                                else {
                                                    bExiste = result.Result.split(',')[0];
                                                    provincia = result.Result.split(',')[1];
                                                    paisCod = result.Result.split(',')[2];

                                                    if (bExiste == "True") {
                                                        if (provincia != "" && paisCod != "") {
                                                            dragMunicipio = municipio + ', ' + provincia + ' (' + paisCod + ')';
                                                        }
                                                        else {
                                                            dragMunicipio = municipio;
                                                        }
                                                    }
                                                    else {
                                                        dragMunicipio = "";
                                                    }

                                                    resolve(dragMunicipio);
                                                }
                                            }
                                        });
                                    }

                                    else {
                                        resolve(dragMunicipio);
                                    }
                                }
                            });

                        });

                        pFijar.then((dragMunicipio) => {

                            App[idComponente + "_cmbMunicipioProvincia"].setValue(dragMunicipio);
                            App[idComponente + "_txtDireccion"].setValue(dragDireccion);
                            App[idComponente + "_txtBarrio"].setValue(dragBarrio);
                            App[idComponente + "_txtCodigoPostal"].setValue(dragCodigoPostal);

                            load.hide();
                        });

                        // #endregion
                    }

                    pWindow = new Promise((resolve, reject) => {
                        if (idComponente == "UCGridEmplazamientosLocalizaciones") {
                            App[idComponente + "_geoPosicion_winPosicionar"].hide();
                        }
                        else {
                            App[idComponente + "_winPosicionar"].hide();
                        }
                        resolve();
                    });
                    pWindow.then(() => { });
                });
            });

            if (idComponente == "UCGridEmplazamientosLocalizaciones") {
                fijar.hidden = true;
            }

        }
        // #endregion

    });

}

function cargarCombo(sender, registro, index) {
    App[sender.config.storeId].data.items.forEach(p => p.data.Completo = p.data.NombreMunicipio + ', '
        + p.data.NombreProvincia + ' (' + p.data.PaisCod + ')');
}