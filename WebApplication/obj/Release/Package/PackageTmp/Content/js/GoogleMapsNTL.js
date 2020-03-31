/*----MoB!----*/
//var gm;
var infoWindow;
var Azmiuth_Circle = $('#Azmiuth_Circle').text();
var SiteCode = '';
var myPloygon;
//var map;

//console.log(Azmiuth_Circle);
function Circle(latlng, color, RadiouSize) {
    // debugger;
    var circle = new google.maps.Circle({
        center: latlng,
        map: map,
        radius: parseFloat(RadiouSize),
        fillColor: color,
        fillOpacity: 1,
        // strokeColor:color,// '#FF0000',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        strokeColor: 'transparent',
    });
}

function DrawCircles(Data, RadiouSize) {

    // 0 = lat
    // 1 = long
    // 2 = color
    if (Data != null) {
        var bounds = new google.maps.LatLngBounds();
        var latlng;



        //  console.time('cache');

        for (i = 0; i < Data.length; i++) {
            latlng = new gm.LatLng(Data[i].Latitude, Data[i].Longitude);
            Circle(latlng, Data[i].Color, RadiouSize);
            bounds.extend(latlng);
        }


        //$.each(Data, function (i, item) {
        //    latlng = new gm.LatLng(item.Latitude, item.Longitude);

        //    Circle(latlng, item.Color, RadiouSize);
        //    bounds.extend(latlng);
        //  //  console.log(item.Latitude);
        //});
        // console.timeEnd('end cache');
        //bounds.extend(latlng);


        map.fitBounds(bounds);

        Data = [];
        Data = undefined;
        bounds = null;
        bounds = undefined;




    }

    return false;
}

function DrawMarker(Data, IconUrl) {
    // 0 = name
    // 1 = lat
    // 2 = long
    // 2 = image

    if (Data != null) {
        var MarkerIcon = {
            url: IconUrl,// '/Content/Images/Colors/handover_x16.png',
            scaledSize: new google.maps.Size(30, 30),
            origin: new google.maps.Point(0, 0),
            anchor: new google.maps.Point(0, 0)
        };
        var bounds = new google.maps.LatLngBounds();
        for (var i = 0; i < Data.length; i++) {
            var latlng = new gm.LatLng(Data[i].Latitude, Data[i].Longitude);
            Marker(latlng, Data[i].Color, MarkerIcon);
            bounds.extend(latlng);
        }
        //  Data = undefined;
        //  MarkerIcon = undefined;
        // Marker = undefined;
        map.fitBounds(bounds);

    }

    function Marker(latlng, name, img) {

        if (img == null && img == '') {
            var marker = new google.maps.Marker({
                map: map,
                position: latlng,
                title: name,
            });
        } else {
            var marker = new google.maps.Marker({
                map: map,
                position: latlng,
                title: name,
                icon: img,
            });
        }
    }
}



function Text(Data) {
    // 0 = name
    // 1 = lat
    // 2 = long
    // 2 = image
    if (Data != null) {
        var latlng;
        for (var i = 0; i < Data.length; i++) {
            // debugger;
            var lat = parseFloat(Data[i].Latitude);
            var long = parseFloat(Data[i].Longitude) + 0.0009990;
            latlng = new gm.LatLng(lat, long);
            customTxt = "<div>" + Data[i].Color + "</div>"
            txt = new TxtOverlay(latlng, customTxt, "pciText", map)
        }

    }

}
function rotate(mapId, degree) {
    var elie = $("#" + mapId);
    elie.css({
        WebkitTransform: 'rotate(' + degree + 'deg)'
    });
    elie.css({
        '-moz-transform': 'rotate(' + degree + 'deg)'
    });
    /* timer = setTimeout(function() {
        rotate(++degree);
    },5); */
    //for (var i = 0; i < gmarkers.length; i++) {
    //    gmarkers[i].setIcon(pinSymbol("red", -degree));
    //}
}


function MapCircle(CenterPoint, Map, Radius) {
    var circle2 = new google.maps.Circle({
        center: CenterPoint,
        map: Map,
        radius: Radius,          // IN METERS.
        // fillColor: '#FF6600',
        fillOpacity: 0,
        strokeColor: "#C80014",
        strokeWeight: 3        // DON'T SHOW CIRCLE BORDER.
    });

}

function initialize(MapId, CenterLat, CenterLong, CenterText, MarkerData, CircleData, AzmiuthData, RadiouSize, AzmuthRadius, SignalCrossMarker) {

    debugger;
    //var LocatMap;
    //var ZoomLat1 = 0;
    //var ZoomLng1 = 0;
    //var ZoomLat2 = 0;
    //var ZoomLng2 = 0;
    gm = google.maps,

    centerPt = new gm.LatLng(CenterLat, CenterLong);

    map = new gm.Map(document.getElementById(MapId), {

        mapTypeId: gm.MapTypeId.ROADMAP,
        // mapTypeId: gm.MapTypeId.HYBRID,
        // mapTypeId: 'satellite',
        // heading: 90,
        // tilt: 45,
        // streetViewControl: true,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.SMALL
        },

        overviewMapControl: true,
        rotateControl: true,
        zoom: 16,
        scaleControl: true,
        center: centerPt,
        defaultUI: false,
        tilt: 45,
        styles: [{
            featureType: "poi",
            //elementType: "labels",
            stylers: [{
                visibility: "off"
            }]
        }]
    });//,
    //LocatMap = map;

    //infoWindow = new google.maps.InfoWindow();

    //var txt = 'A2A0013A';
    //customTxt = "<div>" + txt + "</div>"
    //txt = new TxtOverlay(centerPt, customTxt, "customBox", map)
    //customTxt = "<div style='background:white;width:100px !important'>" + CenterText + "</div>"
   // txt = new TxtOverlay(centerPt, customTxt, "customBox", map)
    if (MarkerData != null || MarkerData != undefined) {
        Text(MarkerData)
    }

    if (MarkerData != null || MarkerData != undefined) {
        if (MapId == "PDSCH_map" || MapId == "PUSCH_map") {

            DrawMarker(MarkerData, null);
        }
        else {
            DrawMarker(MarkerData, '/Content/Images/Colors/handover_x16.png');
        }
        MarkerData = null;
    }

    if (SignalCrossMarker != null || SignalCrossMarker != undefined) {
        DrawMarker(SignalCrossMarker, '/Content/Images/Common/plus.png');
        MarkerData = null;
    }

    if (CircleData != null || CircleData != undefined) {
        DrawCircles(CircleData, RadiouSize);
    }


    if (AzmiuthData != null || AzmiuthData != undefined) {

        Azmiuth(AzmiuthData, AzmuthRadius, '0');
        // AzmiuthData = undefined;
    }


    //var circle = new google.maps.Circle({
    //    center: centerPt,
    //    map: map,
    //    radius: 5,
    //    fillColor: 'black',
    //    fillOpacity: 1,
    //    strokeColor: 'orange',
    //});

    var cr = parseInt(AzmuthRadius) + 20;
    if (Azmiuth_Circle == 1) {
        MapCircle(centerPt, map, cr);
    }


    var marker = new google.maps.Circle({
        position: centerPt,
        map: map,
    });
    var infowindow = new google.maps.InfoWindow({
        content: CenterText
    });
    infowindow.open(map, marker);

    // rotate(MapId, 90);
    //google.maps.event.addListener(map, 'click', function (event) {
    //    alert("Latitude: " + event.latLng.lat() + " " + ", longitude: " + event.latLng.lng());
    //});

    //LocatMap.addListener('click', function (arg) {
    //    if (ZoomLat1 == 0 && ZoomLng1==0) {
    //        ZoomLat1 = arg.latLng.lat();
    //        ZoomLng1 = arg.latLng.lng();
    //    } else {
    //        ZoomLat2 = arg.latLng.lat();
    //        ZoomLng2 = arg.latLng.lng();
    //        strictBounds = new google.maps.LatLngBounds(
    //                   new google.maps.LatLng(ZoomLat1, ZoomLng1),
    //                   new google.maps.LatLng(ZoomLat2, ZoomLng2));
    //        var se = new google.maps.LatLng(ZoomLat1, ZoomLng1); // Tasman Sea
    //        var nw = new google.maps.LatLng(ZoomLat2, ZoomLng2); // Indian Ocean
    //        var bs = new google.maps.LatLngBounds(se,nw);
    //        LocatMap.fitBounds(bs);
    //        console.log(ZoomLat1 + '-' + ZoomLng1 + ',' + ZoomLat2 + '-' + ZoomLng2);
    //        ZoomLat1 = 0;
    //        ZoomLng1 = 0;
    //        ZoomLat2 = 0;
    //        ZoomLng2 = 0;
    //    }
    //});
    //map.addListener('zoom_changed', function () {
    // //   var bounds = map.getBounds();
    //    //var ne = bounds.getNorthEast(); // LatLng of the north-east corner
    //   // var sw = bounds.getSouthWest(); // LatLng of the south-west corder
    //   // var bs = new google.maps.LatLngBounds(ne, sw);
    //   // map.fitBounds(map.getBounds());
    //});


    //customTxt = "<div>" + CenterText + "</div>"
    //txt = new TxtOverlay(centerPt, customTxt, "customBox", map)

    //MarkerData = undefined;
    //CircleData = undefined;
    //AzmiuthData = undefined;



    setTimeout(
function () {
    map.setCenter(centerPt);
}, 1000);
    return false;

}

function initializekmlDriveRoute(MapId, CenterLat, CenterLong, CenterText, kmlUrl) {

    var myOptions = {
        center: new google.maps.LatLng(CenterLat, CenterLong),
        zoom: 19,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        scaleControl: true,
        // scaleControlOptions: { position: google.maps.ControlPosition.TOP_CENTER }
    };

    map = new google.maps.Map(document.getElementById(MapId), myOptions);
    SiteCode = CenterText;

    var myParser = new geoXML3.parser({
        map: map,
        processStyles: true,
        suppressInfoWindows: true
    });

    myParser.parse(kmlUrl);

    google.maps.event.addListenerOnce(map, 'idle', function () {
        google.maps.event.trigger(map, 'resize');
    });

    return myParser;
}

function initializekml(MapId, CenterLat, CenterLong, CenterText, kmlUrl, AzmiuthData, RadiouSize, AzmuthRadius) {
    //alert('initial kml')
    debugger;
    // kmlUrl = 'http://localhost:18460/Content/AirViewLogs/samplekml/cw.kml';

    var myOptions = {
        center: new google.maps.LatLng(CenterLat, CenterLong),
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        scaleControl: true,
        // scaleControlOptions: { position: google.maps.ControlPosition.TOP_CENTER }
    };
    map = new google.maps.Map(document.getElementById(MapId), myOptions);
    SiteCode = CenterText;
    //var kmzLayer = new google.maps.KmlLayer(kmlUrl);
    //kmzLayer.setMap(map);


    //var myParser = new geoXML3.parser({
    //    map: map,
    //    processStyles: true,
    //    createMarker: addMarkerWithLabel,
    //    suppressInfoWindows: true
    //});
    //myParser.parseKmlString(contents);

    var myParser = new geoXML3.parser({
        map: map,
        processStyles: true,
        //   createMarker: addMarkerWithLabel,
        suppressInfoWindows: true
    });
    myParser.parse(kmlUrl);

    if (AzmiuthData != null || AzmiuthData != undefined) {

        // Azmiuth(AzmiuthData, AzmuthRadius, '0');
        Azmiuth(AzmiuthData, AzmuthRadius, '0');


        // AzmiuthData = undefined;
    }
    var cr = parseInt(AzmuthRadius) + 50;
    if (Azmiuth_Circle == 1) {
        MapCircle(centerPt, map, cr);
    }

    google.maps.event.addListenerOnce(map, 'idle', function () {
        //alert('after initialize function');
        google.maps.event.trigger(map, 'resize');
    });



    // //var src = kmlUrl;// 'http://96.57.107.148/Content/AirViewLogs/TMO/CH41662A-D1/LTE_LTE%201900_1150/pci.kml';

    // //var kmlLayer = new google.maps.KmlLayer(src, {
    // //    suppressInfoWindows: true,
    // //    preserveViewport: false,
    // //    map: map
    // //});

    // //var ctaLayer = new google.maps.KmlLayer({
    // //    url: kmlUrl,
    // //    map: map
    // //});

    // loadKmlLayer(kmlUrl, map);
    setTimeout(
function () {
    map.setCenter(new google.maps.LatLng(CenterLat, CenterLong));
}, 1000);

    return false;

}


function initializekmlNetLayer(MapId, CenterLat, CenterLong, CenterText, kmlUrl, AzmiuthData, RadiouSize, AzmuthRadius,MarkerData) {
    //alert('initial kml')
    // debugger;
    // kmlUrl = 'http://localhost:18460/Content/AirViewLogs/samplekml/cw.kml';

    var myOptions = {
        center: new google.maps.LatLng(CenterLat, CenterLong),
        zoom: 16,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        scaleControl: true,
        // scaleControlOptions: { position: google.maps.ControlPosition.TOP_CENTER }
    };
    map = new google.maps.Map(document.getElementById(MapId), myOptions);
    SiteCode = CenterText;

    var myParser = new geoXML3.parser({
        map: map,
        processStyles: true,
        suppressInfoWindows: true
    });
    myParser.parse(kmlUrl);

    if (AzmiuthData != null || AzmiuthData != undefined) {

        // Azmiuth(AzmiuthData, AzmuthRadius, '0');
        Azmiuth(AzmiuthData, AzmuthRadius, '0');


        // AzmiuthData = undefined;
    }
    var cr = parseInt(AzmuthRadius) + 50;
    if (Azmiuth_Circle == 1) {
        MapCircle(centerPt, map, cr);
    }

    google.maps.event.addListenerOnce(map, 'idle', function () {
        //alert('after initialize function');
        google.maps.event.trigger(map, 'resize');
    });

    DrawMarker(MarkerData, null);
    return false;

}

function loadKmlLayer(src, map) {
    var kmlLayer = new google.maps.KmlLayer(src, {
        suppressInfoWindows: true,
        preserveViewport: false,
        createMarker: addMarkerWithLabel,
        map: map
    });
    google.maps.event.addListener(kmlLayer, 'click', function (event) {
        var content = event.featureData.infoWindowHtml;
        var testimonial = document.getElementById('capture');
        testimonial.innerHTML = content;
    });
}

function addMarkerWithLabel(placemark) {
    if (typeof placemark.latlng != "undefined" && placemark.name.length > 0) {
        var labelOptions = {
            content: placemark.name,
            boxClass: "mapLabel",
            disableAutoPan: true,
            alignBottom: true,
            pixelOffset: new google.maps.Size(7, -12),
            position: placemark.latlng,
            closeBoxURL: "",
            isHidden: false,
            pane: "floatPane",
            enableEventPropagation: true
        };

        var ibLabel = new InfoBox(labelOptions);
        ibLabel.open(map);
    }
    myParser.createMarker(placemark);
}




//function initializeHybrid(MapId, CenterLat, CenterLong, CenterText, MarkerData, CircleData, AzmiuthData, AzmuthRadius,Latitudes,Lngitudes,CoverageDistance) {
//    gm = google.maps, centerPt = new gm.LatLng(CenterLat, CenterLong);

//    map = new gm.Map(document.getElementById(MapId), {

//        //mapTypeId: gm.MapTypeId.ROADMAP,
//        mapTypeId: gm.MapTypeId.HYBRID,
//        zoom: 17,
//        center: centerPt,
//        scaleControl: true,
//    }),
//     gm.event.addListener(map, 'zoom_changed', function () {
//         zoomLevel = map.getZoom();
//         var MaxZoom = 20;
         
//         var AzmuthRadiusToSend=0;
//         switch (zoomLevel) {
//             case 20:
//                 AzmuthRadiusToSend = 65;
//                 break;
//             case 19:
//                 AzmuthRadiusToSend = 70;
//                 break;
//             case 18:
//                 AzmuthRadiusToSend = 80;
//                 break;
//             case 17:
//                 AzmuthRadiusToSend = 100;
//                 break;
//             case 16:
//                 AzmuthRadiusToSend = 400;
//                 break;
//             case 15:
//                 AzmuthRadiusToSend = 400+600;
//                 break;
//             case 14:
//                 AzmuthRadiusToSend = 800*2;
//                 break;
//             case 13:
//                 AzmuthRadiusToSend = 1600*2;
//                 break;
//             case 12:
//                 AzmuthRadiusToSend = 3200*2;
//                 break;
//             case 11:
//                 AzmuthRadiusToSend = 6400*2;
//                 break;
//             case 10:
//                 AzmuthRadiusToSend = 12800*3;
//                 break;
//             case 9:
//                 AzmuthRadiusToSend = 25600*3;
//                 break;
//             case 8:
//                 AzmuthRadiusToSend = 51200*3;
//                 break;
//             case 7:
//                 AzmuthRadiusToSend = 102400*3;
//                 break;
//             case 6:
//                 AzmuthRadiusToSend = 204800*3;
//                 break;
//             case 5:
//                 AzmuthRadiusToSend = 409600*3;
//                 break;
//             case 4:
//                 AzmuthRadiusToSend = 819200*4;
//                 break;
//             case 3:
//                 AzmuthRadiusToSend = 1638400*4;
//                 break;
//             case 2:
//                 AzmuthRadiusToSend = 1638400 * 4;
//                 break;
//             case 1:
//                 AzmuthRadiusToSend = 1638400 * 4;
//                 break;
//             case 0:
//                 AzmuthRadiusToSend = 1638400 * 4;
//                 break;
//             default:
//                 AzmuthRadiusToSend = 100;

//         }
//         myPloygon.setMap(null);
//         Azmiuth(AzmiuthData, AzmuthRadiusToSend, '0');


//     });
//        map.setTilt(90);
//    // infoWindow = new google.maps.InfoWindow();
//    if (Latitudes != undefined) {

//        var i=0
//        for (; i < Lngitudes.length; i++) {
//            var uhkh = Latitudes[i]
//            markerForNeibour = new google.maps.Marker({
//                // The below line is equivalent to writing:
//                // position: new google.maps.LatLng(-34.397, 150.644)
//                position: { lat: Latitudes[i], lng: Lngitudes[i] },
//                map: map,
//                icon: "/Content/Images/Common/dot_x24.png"

//            });
//        }


//    }
//    //var marker = new google.maps.Marker({
//    //map: map,
//    //position: centerPt,

//    //});
//    if (AzmiuthData != null || AzmiuthData != undefined) {
//        Azmiuth(AzmiuthData, AzmuthRadius, '0');
//    }

//    var circle = new google.maps.Circle({
//        center: centerPt,
//        map: map,
//        radius: 5,
//        fillColor: 'black',
//        fillOpacity: 1,
//        strokeColor: 'orange',
//    });

//    //var txt = 'A2A0013A';
//    customTxt = "<div style='background:white;width:100px !important'>" + CenterText + "</div>"
//    txt = new TxtOverlay(centerPt, customTxt, "customBox", map)



//    var cr = parseInt(AzmuthRadius) + 50;
//    if (Azmiuth_Circle == 1) {
//        MapCircle(centerPt, map, cr);
//    }

//    //var circle1 = new google.maps.Circle({
//    //    map: map,
//    //    radius: 250,    // 10 miles in metres
//    // //   fillColor: '#143CB4'
//    //});
//    //circle1.bindTo('center', marker, 'position');

//}


function initializeHybrid(MapId, CenterLat, CenterLong, CenterText, MarkerData, CircleData, AzmiuthData, AzmuthRadius) {
   debugger;
    gm = google.maps, centerPt = new gm.LatLng(CenterLat, CenterLong);

    map = new gm.Map(document.getElementById(MapId), {

        //mapTypeId: gm.MapTypeId.ROADMAP,
        mapTypeId: gm.MapTypeId.HYBRID,
        zoom: 17,
        center: centerPt,
        scaleControl: true,
    }),
        map.setTilt(90);
    // infoWindow = new google.maps.InfoWindow();

    //var marker = new google.maps.Marker({
    //map: map,
    //position: centerPt,

    //});

    if (AzmiuthData != null || AzmiuthData != undefined) {
        //debugger;
        Azmiuth(AzmiuthData, AzmuthRadius, '0');
    }
   
    var circle = new google.maps.Circle({
        center: centerPt,
        map: map,
        radius: 5,
        fillColor: 'black',
        fillOpacity: 1,
        strokeColor: 'orange',
    });

    //var txt = 'A2A0013A';
    



    var cr = parseInt(AzmuthRadius) + 50;
    if (Azmiuth_Circle == 1) {
        MapCircle(centerPt, map, cr);
    }
    
    var marker = new google.maps.Circle({
        position: centerPt,
        map: map,
    });
    
    var infowindow = new google.maps.InfoWindow({
        content: CenterText
    });
    infowindow.open(map, marker);
    //customTxt = "<div style='background:white;width:100px !important'>" + CenterText + "</div>"
    //txt = new TxtOverlay(centerPt, customTxt, "customBox", map)
    //var circle1 = new google.maps.Circle({
    //    map: map,
    //    radius: 250,    // 10 miles in metres
    // //   fillColor: '#143CB4'
    //});
    //circle1.bindTo('center', marker, 'position');
    setTimeout(
  function () {
      map.setCenter(centerPt);
  }, 1000);
}




//function Azmiuth(Angles, Radius, hybrid) {
//    //  debugger;
//    // 0 = start angle
//    // 1 = end angle
//    // 2 = color
//    if (Radius == undefined) {
//        Radius = 100;
//    }
//    polys = [],
//    i = 0,
//    radiusMeters = Radius;
//    var latlng;

//    SiteCode = SiteCode.substring(0, SiteCode.indexOf(' '));
//    for (; i < Angles.length; i++) {
//        // debugger;


//        if (Angles[i].EndAngle == 360)
//        {
//            Angles[i].EndAngle = 0;
//        }
//        else if (Angles[i].EndAngle > 360)
//        {
//            var tmpEnd = Angles[i].EndAngle - 360;
//            if (tmpEnd >= 0 && tmpEnd <= 1)
//            {
//                Angles[i].EndAngle = 1;
//            }
//            else
//            {
//                Angles[i].EndAngle =parseInt( Angles[i].EndAngle) - 360;
//            }
//        }



//        //if (Angles[i].Site == SiteCode) {
//        // //   debugger;
//        //    radiusMeters =parseInt( Radius)+50;
//        //} else {
//        //    radiusMeters = Radius;
//        //}
//        latlng = new google.maps.LatLng(Angles[i].Latitude, Angles[i].Longitude);
//        var path = getArcPath(latlng, radiusMeters, Angles[i].StartAngle, Angles[i].EndAngle);
//        // console.log(path);

//        path.unshift(latlng);
//        path.push(latlng);
//        infoWindow = new google.maps.InfoWindow;
//        var poly = new google.maps.Polygon({
//            path: path,
//            map: map,
//            fillColor: Angles[i].Color,
//            fillOpacity: 0.7,
//            //strokeColor: '#FF0000',
//            strokeOpacity: 0.8,
//            strokeWeight: 3,
//            id:1
          
//        });
//        polys.push(poly);


//        if (Angles[i].Sector == 'Alpha' || Angles[i].Sector == '1') {
//            customTxt = "<div>" + Angles[i].Site + "</div>";
//            txt = new TxtOverlay(latlng, customTxt, "customBox", map);
//        }
//       // poly.setMap(map);
//        poly.addListener('click', showArrays);
//        myPloygon = poly;
//        function showArrays(event) {
//            // Since this polygon has only one path, we can call getPath() to return the
//            // MVCArray of LatLngs.
//            var vertices = this.getPath();

//            var contentString = '<b>AirCode</b><br>' +
//                'Clicked location: <br>' + event.latLng.lat() + ',' + event.latLng.lng() +
//                '<br>';

//          //   Iterate over the vertices.
//            //for (var i = 0; i < vertices.getLength() ; i++) {
//            //    var xy = vertices.getAt(i);
//            //    contentString += '<br>' + 'Coordinate ' + i + ':<br>' + xy.lat() + ',' +
//            //        xy.lng();
//            //}

//            // Replace the info window's content and position.
//            infoWindow.setContent(contentString);
//            infoWindow.setPosition(event.latLng);

//            infoWindow.open(map,this);
//        }

//        //latlng = new google.maps.LatLng(path[path.length-9].lat(), path[path.length-5].lng());
//        //customTxt = "<div>" + Angles[i].PCI + "</div>";
//        //txt = new TxtOverlay(latlng, customTxt, "customBox", map);

//        //for (var j = 0; j < path.length; j++) {
//        //    console.log(path[j].lng() + ',' + path[j].lat() + ',100');
//        //}
//    }
//}

function Azmiuth(Angles, Radius, hybrid) {
    //debugger;
    // 0 = start angle
    // 1 = end angle
    // 2 = color
    if (Radius == undefined) {
        Radius = 100;
    }
    polys = [],
    i = 0,
    radiusMeters = Radius;
    var latlng;
    var prevSite = '';

    SiteCode = SiteCode.substring(0, SiteCode.indexOf(' '));
    for (; i < Angles.length; i++) {

        if (Angles[i].EndAngle == 360) {
            Angles[i].EndAngle = 0;
        }
        else if (Angles[i].EndAngle > 360) {
            var tmpEnd = Angles[i].EndAngle - 360;
            if (tmpEnd >= 0 && tmpEnd <= 1) {
                Angles[i].EndAngle = 1;
            }
            else {
                Angles[i].EndAngle = parseInt(Angles[i].EndAngle) - 360;
            }
        }


        //if (Angles[i].Site == SiteCode) {
        // //   debugger;
        //    radiusMeters =parseInt( Radius)+50;
        //} else {
        //    radiusMeters = Radius;
        //}
        debugger
        latlng = new google.maps.LatLng(Angles[i].Latitude, Angles[i].Longitude);
        var path = getArcPath(latlng, radiusMeters, Angles[i].StartAngle, Angles[i].EndAngle);
        // console.log(path);

        path.unshift(latlng);
        path.push(latlng);
        var poly = new google.maps.Polygon({
            path: path,
            map: map,
            fillColor: Angles[i].Color,
            fillOpacity: 0.7,
            strokeOpacity: 0.7,
            strokeWeight: 0.5,
        });
        polys.push(poly);
        //if (Angles[i].Sector == 'Alpha' || Angles[i].Sector == '1') {
        //    customTxt = "<div>" + Angles[i].Site + "</div>";
        //    txt = new TxtOverlay(latlng, customTxt, "customBox", map);
        //}
        //latlng = new google.maps.LatLng(path[path.length-9].lat(), path[path.length-5].lng());
        //customTxt = "<div>" + Angles[i].PCI + "</div>";
        //txt = new TxtOverlay(latlng, customTxt, "customBox", map);

        //for (var j = 0; j < path.length; j++) {
        //    console.log(path[j].lng() + ',' + path[j].lat() + ',100');
        //}
        if (Angles[i].Site != prevSite) {
            var marker = new google.maps.Circle({
                position: latlng,
                map: map,
            });
            var infowindow = new google.maps.InfoWindow({
                content: Angles[i].Site
            });
            infowindow.open(map, marker);
        }
        
        prevSite = Angles[i].Site;
    }



}


//function getArcPath(center, radiusMeters, startAngle, endAngle, direction) {
//    // debugger;
//    var point, previous,
//        atEnd = false,
//        points = Array(),
//        a = startAngle;
//    while (true) {
//        point = google.maps.geometry.spherical.computeOffset(center, radiusMeters, a);
//        points.push(point);
//        if (a == endAngle) {
//            break;
//        }
//        a++;
//        if (a > 360) {
//            a = 1;
//        }
//    }
//    if (direction == 'counterclockwise') {
//        points = points.reverse();
//    }
//    return points;
//}


///*******************TxtOverlay****************************/
//function TxtOverlay(pos, txt, cls, map) {

//    // Now initialize all properties.
//    this.pos = pos;
//    this.txt_ = txt;
//    this.cls_ = cls;
//    this.map_ = map;

//    // We define a property to hold the image's
//    // div. We'll actually create this div
//    // upon receipt of the add() method so we'll
//    // leave it null for now.
//    this.div_ = null;

//    // Explicitly call setMap() on this overlay
//    this.setMap(map);
//}
function getArcPath(center, radiusMeters, startAngle, endAngle, direction) {
    // debugger;
    var point, previous,
        atEnd = false,
        points = Array(),
        a = startAngle;
    while (true) {
        point = google.maps.geometry.spherical.computeOffset(center, radiusMeters, a);
        points.push(point);
        if (a == endAngle) {
            break;
        }
        a++;
        if (a > 360) {
            a = 1;
        }
    }
    if (direction == 'counterclockwise') {
        points = points.reverse();
    }
    return points;
}


/*******************TxtOverlay****************************/
function TxtOverlay(pos, txt, cls, map) {

    // Now initialize all properties.
    this.pos = pos;
    this.txt_ = txt;
    this.cls_ = cls;
    this.map_ = map;

    // We define a property to hold the image's
    // div. We'll actually create this div
    // upon receipt of the add() method so we'll
    // leave it null for now.
    this.div_ = null;

    // Explicitly call setMap() on this overlay
    this.setMap(map);
}
TxtOverlay.prototype = new google.maps.OverlayView();
TxtOverlay.prototype.onAdd = function () {

    // Note: an overlay's receipt of onAdd() indicates that
    // the map's panes are now available for attaching
    // the overlay to the map via the DOM.

    // Create the DIV and set some basic attributes.
    var div = document.createElement('DIV');
    div.className = this.cls_;

    div.innerHTML = this.txt_;

    // Set the overlay's div_ property to this DIV
    this.div_ = div;
    var overlayProjection = this.getProjection();
    var position = overlayProjection.fromLatLngToDivPixel(this.pos);
    div.style.left = position.x + 'px';
    div.style.top = position.y + 'px';
    // We add an overlay to a map via one of the map's panes.

    var panes = this.getPanes();
    panes.floatPane.appendChild(div);
}
TxtOverlay.prototype.draw = function () {
    var overlayProjection = this.getProjection();
    // Retrieve the southwest and northeast coordinates of this overlay
    // in latlngs and convert them to pixels coordinates.
    // We'll use these coordinates to resize the DIV.
    var position = overlayProjection.fromLatLngToDivPixel(this.pos);
    var div = this.div_;
    div.style.left = position.x + 'px';
    div.style.top = position.y + 'px';
}
//Optional: helper methods for removing and toggling the text overlay.  
TxtOverlay.prototype.onRemove = function () {
    this.div_.parentNode.removeChild(this.div_);
    this.div_ = null;
}
TxtOverlay.prototype.hide = function () {
    if (this.div_) {
        this.div_.style.visibility = "hidden";
    }
}

TxtOverlay.prototype.show = function () {
    if (this.div_) {
        this.div_.style.visibility = "visible";
    }
}

TxtOverlay.prototype.toggle = function () {
    if (this.div_) {
        if (this.div_.style.visibility == "hidden") {
            this.show();
        } else {
            this.hide();
        }
    }
}
TxtOverlay.prototype.toggleDOM = function () {
    if (this.getMap()) {
        this.setMap(null);
    } else {
        this.setMap(this.map_);
    }
}

/*******************END TxtOverlay****************************/