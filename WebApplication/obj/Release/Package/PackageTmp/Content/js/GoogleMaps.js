/*----MoB!----*/
//var gm;

function Circle(latlng, color) {
    var circle = new google.maps.Circle({
        center: latlng,
        map: map,
        radius: 17,
        fillColor: color,
        fillOpacity: 1,
        strokeColor: 'transparent',
    });
}
function DrawCircles(Data) {
    // 0 = lat
    // 1 = long
    // 2 = color
    if (Data != null) {
        var bounds = new google.maps.LatLngBounds();


        for (var i = 0; i < Data.length; i++) {

            var latlng = new gm.LatLng(Data[i][0], Data[i][1]);

            Circle(latlng, Data[i][2]);

            bounds.extend(latlng);
        }
        Data = null;
        map.fitBounds(bounds);
    }

    
}


function DrawMarker(Data) {
    // 0 = name
    // 1 = lat
    // 2 = long
    // 2 = image
    if (Data != null) {
        var bounds = new google.maps.LatLngBounds();
        for (var i = 0; i < Data.length; i++) {
            var latlng = new gm.LatLng(Data[i][1], Data[i][2]);
            Marker(latlng, Data[i][0], Data[i][3]);
            bounds.extend(latlng);
        }
        Data = null;
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


function MapWithCircleFunction(MapId, CenterLat, CenterLong, CenterText,DrawOutCircles, MarkerData, AzmiuthData) {
    //  debugger;
    gm = google.maps,
    centerPt = new gm.LatLng(CenterLat, CenterLong);

    map = new gm.Map(document.getElementById(MapId), {

        mapTypeId: gm.MapTypeId.ROADMAP,
        // mapTypeId: gm.MapTypeId.HYBRID,
        zoom: 16,
        center: centerPt
    }),

    infoWindow = new google.maps.InfoWindow();

    //var txt = 'A2A0013A';
    //customTxt = "<div>" + txt + "</div>"
    //txt = new TxtOverlay(centerPt, customTxt, "customBox", map)

    if (MarkerData != null || MarkerData != undefined) {
        DrawMarker(MarkerData);
    }


    DrawOutCircles();

   


    if (AzmiuthData != null || AzmiuthData != undefined) {
        Azmiuth(AzmiuthData, centerPt);
    }


    var circle = new google.maps.Circle({
        center: centerPt,
        map: map,
        radius: 5,
        fillColor: 'black',
        fillOpacity: 1,
        strokeColor: 'orange',
    });


    customTxt = "<div>" + CenterText + "</div>"
    txt = new TxtOverlay(centerPt, customTxt, "customBox", map)

}

function initialize(MapId, CenterLat, CenterLong, CenterText, MarkerData, CircleData, AzmiuthData) {
  //  debugger;
    gm = google.maps,
    centerPt = new gm.LatLng(CenterLat, CenterLong);

    map = new gm.Map(document.getElementById(MapId), {
        
        mapTypeId: gm.MapTypeId.ROADMAP,
       // mapTypeId: gm.MapTypeId.HYBRID,
        zoom: 16,
        center: centerPt
    }),

    infoWindow = new google.maps.InfoWindow();

    //var txt = 'A2A0013A';
    //customTxt = "<div>" + txt + "</div>"
    //txt = new TxtOverlay(centerPt, customTxt, "customBox", map)

    if (MarkerData != null || MarkerData != undefined) {
        DrawMarker(MarkerData);
        MarkerData = null;
    }


    if (CircleData != null || CircleData != undefined) {
        DrawCircles(CircleData);
        CircleData = null;
    }



    if (AzmiuthData != null || AzmiuthData != undefined) {
        Azmiuth(AzmiuthData, centerPt);
        AzmiuthData = null;
    }


    var circle = new google.maps.Circle({
        center: centerPt,
        map: map,
        radius: 5,
        fillColor: 'black',
        fillOpacity: 1,
        strokeColor: 'orange',
    });
    
  
    customTxt = "<div>" + CenterText + "</div>"
    txt = new TxtOverlay(centerPt, customTxt, "customBox", map)

    
    

}

function initializeHybrid(MapId, CenterLat, CenterLong,CenterText, MarkerData, CircleData, AzmiuthData) {
    //debugger;
    gm = google.maps,
    centerPt = new gm.LatLng(CenterLat, CenterLong);

    map = new gm.Map(document.getElementById(MapId), {

        //mapTypeId: gm.MapTypeId.ROADMAP,
         mapTypeId: gm.MapTypeId.HYBRID,
        zoom: 17,
        center: centerPt
    }),

    infoWindow = new google.maps.InfoWindow();

    //var marker = new google.maps.Marker({
    //    map: map,
    //    position: centerPt,

    //});

    if (MarkerData != null || MarkerData != undefined) {
        DrawMarker(MarkerData);
    }


    if (CircleData != null || CircleData != undefined) {
        DrawCircles(CircleData);
    }



    if (AzmiuthData != null || AzmiuthData != undefined) {
        Azmiuth(AzmiuthData, centerPt);
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
    customTxt = "<div>" + CenterText + "</div>"
    txt = new TxtOverlay(centerPt, customTxt, "customBox", map)

}


function Azmiuth(Angles, CenterPoint) {
    // 0 = start angle
    // 1 = end angle
    // 2 = color

    


    polys = [],
    i = 0,
    radiusMeters = 100;

    for (; i < Angles.length; i++) {
        var path = getArcPath(CenterPoint, radiusMeters, Angles[i][0], Angles[i][1]);
        path.unshift(CenterPoint);
        path.push(CenterPoint);
        var poly = new google.maps.Polygon({
            path: path,
            map: map,
            fillColor: Angles[i][2],
            fillOpacity: 0.7,
           
        });
        polys.push(poly);
    }
}

function getArcPath(center, radiusMeters, startAngle, endAngle, direction) {
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