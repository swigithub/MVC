/*----MoB!----*/
//var gm;
var Azmiuth_Circle = $('#Azmiuth_Circle').text();
var ThisMapId='';
//console.log(Azmiuth_Circle);
function Circle(latlng, color, RadiouSize) {
    debugger;
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

    function MapCircle(CenterPoint, Map, Radius, StockColor) {
     
        var circle2 = new google.maps.Circle({
            center: CenterPoint,
            map: Map,
            radius: Radius,          // IN METERS.
            // fillColor: '#FF6600',
            fillOpacity: 0,
            strokeColor:StockColor,
            strokeWeight: 3        // DON'T SHOW CIRCLE BORDER.
        });
    }



    function initializekml(MapId, CenterLat, CenterLong, CenterText, kmlUrl, AzmiuthData, RadiouSize, AzmuthRadius,Circles) {

        var myOptions = {
            center: new google.maps.LatLng(CenterLat, CenterLong),
            zoom: 11,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            scaleControl: true,
        };
        ThisMapId = MapId;
        map = new google.maps.Map(document.getElementById(MapId), myOptions);
      
        var ThisMap = map;
      //  SiteCode = CenterText;
        var myParser = new geoXML3.parser({
            map: ThisMap,
                processStyles: true,
                suppressInfoWindows: true
        });
        myParser.parse(kmlUrl);
         if (AzmiuthData != null || AzmiuthData != undefined) {
             Azmiuth(AzmiuthData, AzmuthRadius, '0', ThisMap);
         }
         var cr = parseInt(AzmuthRadius)+200;
         if (Azmiuth_Circle == 1) {
             MapCircle(centerPt, ThisMap, cr, "#C80014");
         }


         //if (Circles != null || Circles != undefined) {
         //    for (var i = 0; i < Circles.length; i++) {
         //        latlng = new google.maps.LatLng(Circles[i].lat, Circles[i].lng);
         //        MapCircle(latlng, map, parseInt(AzmuthRadius)+50, "#0000ff");
         //    }
         //}
         return false;
    }
    function initializeHybrid(MapId, CenterLat, CenterLong, CenterText, MarkerData, CircleData, AzmiuthData, AzmuthRadius) {
        gm = google.maps, centerPt = new gm.LatLng(CenterLat, CenterLong);
        map = new gm.Map(document.getElementById(MapId), {
            mapTypeId: gm.MapTypeId.HYBRID,
            zoom: 13,
            center: centerPt,
            scaleControl: true,
        }),
        map.setTilt(90);
        var ThisMap = map;
        if (AzmiuthData != null || AzmiuthData != undefined) {
            Azmiuth(AzmiuthData, AzmuthRadius, '0', ThisMap);
        }
        var circle = new google.maps.Circle({
            center: centerPt,
            map: ThisMap,
            radius: 5,
            fillColor: 'black',
            fillOpacity: 1,
            strokeColor: 'orange',
        });

        ////var txt = 'A2A0013A';
        //customTxt = "<div>" + CenterText + "</div>"
        //txt = new TxtOverlay(centerPt, customTxt, "customBox", ThisMap)



        var cr = parseInt(AzmuthRadius) + 200;
        if (Azmiuth_Circle == 1) {
            MapCircle(centerPt, ThisMap, cr, "#C80014");
        }

      //  $('.gm-style-cc:first').trigger('click');
       

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
   


    var SiteSplit = false;
    function Azmiuth(Angles, Radius, hybrid, ThisMap) {
        //debugger;
        if (Radius == undefined) {
            Radius = 100;
        }
        polys = [],
        i = 0,
        radiusMeters = Radius;
        var latlng;
        
        if (SiteSplit == false) {
            SiteCode = SiteCode.substring(0, SiteCode.indexOf(' '));
            SiteSplit = true;
        }
        
       
        for (; i < Angles.length; i++) {           
            if (SiteCode == Angles[i].Site) {
                radiusMeters = parseInt(radiusMeters) + 150;
               
            } else {
              //  debugger;
                radiusMeters = Radius;
            }
            latlng = new google.maps.LatLng(Angles[i].Latitude, Angles[i].Longitude);
            var path = getArcPath(latlng, radiusMeters, Angles[i].StartAngle, Angles[i].EndAngle);
            
            path.unshift(latlng);
            path.push(latlng);
            var poly = new google.maps.Polygon({
                path: path,
                map: ThisMap,
                fillColor: Angles[i].Color,
                fillOpacity: 1,
                strokeWeight: 0.7,
            });
            radiusMeters = Radius;
            polys.push(poly);

           
            if (ThisMapId == 'SummeryPlot') {
                LastMap = ThisMap;
                //if (Angles[i].IsPOR == true) {
                //    MapCircle(latlng, ThisMap, parseInt(radiusMeters) + 50, "#0000ff");
                //}
            }

            


            if (Angles[i].Sector == 'Alpha' || Angles[i].Sector == '1') {
                customTxt = "<div>" + Angles[i].Site + "</div>";
                txt = new TxtOverlay(latlng, customTxt, "customBox", ThisMap);
            }

            if (SiteCode == Angles[i].Site) {
                latlng = new google.maps.LatLng(path[path.length - 9].lat(), path[path.length - 5].lng());
                customTxt = "<div>" + Angles[i].PCI + "</div>";
                txt = new TxtOverlay(latlng, customTxt, "customBox", ThisMap);
            }
           
            }      
           
    }

    function getArcPath(center, radiusMeters, startAngle, endAngle, direction) {
        // debugger;
        try {
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

        } catch (e) {
            console.log(e);
        }
        
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