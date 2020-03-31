/*----MoB!----*/
//var gm;
function Circle(latlng, color, RadiouSize) {
    // debugger;
    var circle = new google.maps.Circle({
        center: latlng,
        map: map,
        radius: parseFloat(RadiouSize),
        fillColor: color,
        fillOpacity: 1,
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


    //var MarkerIcon={
    //      url: '/Content/Images/Colors/handover_x16.png',
    //    scaledSize: new google.maps.Size(30, 30),
    //    origin: new google.maps.Point(0, 0),
    //    anchor: new google.maps.Point(0, 0)
    //};
    function DrawMarker(Data,IconUrl) {
        // 0 = name
        // 1 = lat
        // 2 = long
        // 2 = image
     
        if (Data != null) {
            var MarkerIcon={
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


    function initialize(MapId, CenterLat, CenterLong, CenterText, MarkerData, CircleData, AzmiuthData, RadiouSize, AzmuthRadius) {

        //  debugger;
        gm = google.maps,
        centerPt = new gm.LatLng(CenterLat, CenterLong);

        map = new gm.Map(document.getElementById(MapId), {

            mapTypeId: gm.MapTypeId.ROADMAP,
            // mapTypeId: gm.MapTypeId.HYBRID,
            zoom: 16,
            center: centerPt
        });//,

       // infoWindow = new google.maps.InfoWindow();

        //var txt = 'A2A0013A';
        //customTxt = "<div>" + txt + "</div>"
        //txt = new TxtOverlay(centerPt, customTxt, "customBox", map)

        if (MarkerData != null || MarkerData != undefined) {
            Text(MarkerData)
        }
        
        if (MarkerData != null || MarkerData != undefined) {
            DrawMarker(MarkerData, '/Content/Images/Colors/handover_x16.png');
            MarkerData = null;
        }

        if (CircleData != null || CircleData != undefined) {
            DrawCircles(CircleData, RadiouSize);
        }
       


        if (AzmiuthData != null || AzmiuthData != undefined) {
            Azmiuth(AzmiuthData, centerPt, AzmuthRadius);
           // AzmiuthData = undefined;
        }


        var circle = new google.maps.Circle({
            center: centerPt,
            map: map,
            radius: 5,
            fillColor: 'black',
            fillOpacity: 1,
            strokeColor: 'orange',
        });
    
        
  
        //customTxt = "<div>" + CenterText + "</div>"
        //txt = new TxtOverlay(centerPt, customTxt, "customBox", map)

        //MarkerData = undefined;
        //CircleData = undefined;
        //AzmiuthData = undefined;

        return false;

    }
   

    function initializeHybrid(MapId, CenterLat, CenterLong, CenterText, MarkerData, CircleData, AzmiuthData, AzmuthRadius) {
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
            DrawMarker(MarkerData, '/Content/Images/Colors/handover_x16.png');
        }


        if (CircleData != null || CircleData != undefined) {
            DrawCircles(CircleData);
        }
     


        if (AzmiuthData != null || AzmiuthData != undefined) {
            Azmiuth(AzmiuthData, centerPt, AzmuthRadius);
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


    function Azmiuth(Angles, CenterPoint,Radius) {
        // 0 = start angle
        // 1 = end angle
        // 2 = color
        if (Radius == undefined) {
            Radius = 100;
        }

        polys = [],
        i = 0,
        radiusMeters = Radius;

        for (; i < Angles.length; i++) {
            var path = getArcPath(CenterPoint, radiusMeters, Angles[i][0], Angles[i][1]);
           // console.log(path);
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