
///---controller SiteRoute
//----action Index---

//---==global variables==-->
var StartLtd = undefined, StartLng = undefined, EndLtd = undefined, EndLng = undefined;
var MyDataArray = [];
var mapZoom = 5;
var waypts = [];
var waypts2 = [];
var res1, res2;
var MyRoutes;
var poly;
var map;
var Routemarkers = [];
var startpath = 0;
var Routes = [];
var CurrentRoute = [];
var RoutesList = [];
var RoutesList1 = [];
var PolyLines = [];
var centerPt;
var SelectedRouteSite = 0;
var SelectedSiteCode = '';
var PathColor = '#CC0000';
var SelectedColor;
var SelectedScope;
var SelectedScopeId = 0;
var SelectedSiteId = 0;
var SelectedRow = 0;
var Latitude = 0;
var Longitude = 0;
var AzRecord;
var currentClientPrefix = '';


//-----functions-->
function MarketSites(SiteCode, Latitude, Longitude) {
    if (AzRecord == null) {
        $.ajax({
            url: '/Site/MarketSitesAngles?filter=bySiteCode&SiteId=' + SelectedSiteId + '&SiteCode=' + SiteCode + '&Latitude=' + Latitude + '&Longitude=' + Longitude,
            type: "post",
            data: $(this).serialize(),
            success: function (res) {
                // console.log(res);
                AzRecord = res;
                Azmiuth(res);
                if (res.length == 0) {
                    var marker = new google.maps.Marker({
                        map: map,
                        position: centerPt,
                    });
                }
            },
            error: function (err) {
            }
        });
    } else {
        //   debugger;
        Azmiuth(AzRecord);
    }


}

function AddLatLngToArray(lats, longs) {

    CurrentRoute.push({ lat: lats, lng: longs });
    //  console.log(CurrentRoute.length);
}

function AddRouteToArray() {
    // debugger;
    if (CurrentRoute.length > 0) {
        Routes.push({ RoutesList: CurrentRoute, Color: PathColor });
        CurrentRoute = [];
        return true;
    }

    return false;
}


function SaveKml(SiteId) {
    debugger
    var TestTypes = '';
    $("input:checkbox[name=TestType]:checked").each(function () {
        TestTypes = TestTypes + $(this).val() + ',';
    });
    if (TestTypes.length < 3) {
        $('#RouteMsg').text('Select Test Type');
        return false;
    } else {
        $('#RouteMsg').text('');
    }

    //var  rleg = directionsDisplay.directions.routes[0].legs[0];
    //console.log("res1", rleg[0].lat() + "   " + rleg[0].lng());

    var Data;
    debugger;
    MyDataArray = [];
    for (var i = 0; i < waypts.length; i++) {
        MyDataArray.push(waypts[i]);
    }
    for (var i = 0; i < waypts2.length; i++) {
        MyDataArray.push(waypts2[i]);
    }
    var obj = { lat: EndLtd, lng: EndLng };
    MyDataArray.push({
        location: obj,
        stopover: false
    });
    var obj2 = { lat: StartLtd, lng: StartLng };
    MyDataArray.push({
        location: obj2,
        stopover: false
    });

    if (SelectedRow > 0) {
        //$.confirm({
        //    title: 'Confirm!',
        //    content: 'Do you want to Save As new Drive Route? \nPress Yes for Save As new Drive Route! \nPress No for Update Existly Route Cancel!',
        //    buttons: {
        //        Yes: function () {
        //            Data = { 'cordinates': Routes, 'SiteId': SiteId, 'SiteCode': SelectedSiteCode, 'TestType': TestTypes, 'ScopeId': SelectedScopeId, 'RouteId': SelectedRow, 'Filter': 'Insert', 'ClientPrefix': currentClientPrefix }
        //            fnSave(Data)
        //        },
        //        No: function () {
        //            Data = { 'cordinates': Routes, 'SiteId': SiteId, 'SiteCode': SelectedSiteCode, 'TestType': TestTypes, 'ScopeId': SelectedScopeId, 'RouteId': SelectedRow, 'Filter': 'Update', 'ClientPrefix': currentClientPrefix }
        //            fnSave(Data)
        //        },
        //        Cancel: function () {
        //            ShowMapWindow('bySiteCode', SelectedSiteId, SelectedSiteCode, Latitude, Longitude, SelectedScope, SelectedScopeId, currentClientPrefix);
        //        }
        //    }
        //});
        Data = { 'cordinates': MyDataArray, 'SiteId': SiteId, 'SiteCode': SelectedSiteCode, 'TestType': TestTypes, 'ScopeId': SelectedScopeId, 'RouteId': SelectedRow, 'Filter': 'Insert', 'ClientPrefix': currentClientPrefix }
        fnSave(Data)
    } else {
        Data = { 'cordinates': MyDataArray, 'SiteId': SiteId, 'SiteCode': SelectedSiteCode, 'TestType': TestTypes, 'ScopeId': SelectedScopeId, 'RouteId': SelectedRow, 'Filter': 'Insert', 'ClientPrefix': currentClientPrefix }
        fnSave(Data)

    }

}

function fnSave(Data) {
    // debugger;
    // AddRouteToArray();
    // debugger;
    //if (Routes != null) {
    if (EndLtd != undefined) {

        debugger
        console.log(MyDataArray);
        $.ajax({
            url: '/Dashboard/DriveRoute',
            type: 'POST',
            data: Data,
            // dataType: 'json',
            // contentType: 'application/json',
            success: function (res) {
                if (res != 'session expired') {
                    if (res.Status == 'success') {
                        StartLtd = undefined, StartLng = undefined, EndLtd = undefined, EndLng = undefined;
                        //waypts = [];
                        MyDataArray = [];
                        //$('#map2').empty();
                        //map = null;
                        //modal_body.empty();
                        //modal.modal('hide');
                        $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 2000, });
                        ShowMapWindow('bySiteCode', SelectedSiteId, SelectedSiteCode, Latitude, Longitude, SelectedScope, SelectedScopeId, currentClientPrefix);

                    } else {
                        MyDataArray = [];
                        $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 2000, });
                    }
                } else {
                    MyDataArray = [];
                    $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }



            },
            error: function () {
                MyDataArray = [];
                alert("error");
            }
        });
        SelectedRow = 0;
    }
    else {
        $('#RouteMsg').text('please draw atleast one route before saving...!');
    }
    //} else {
    //    $('#RouteMsg').text('please draw atleast one route before saving...!');
    //    return false;
    //}

}

function initMap(lats, longs) {

    Routes = [];

    $("#tbl-route tr").removeClass("SelectedRow");

    centerPt = new google.maps.LatLng(lats, longs);
    map = new google.maps.Map(document.getElementById('map2'), {
        zoom: mapZoom,
        center: { lat: lats, lng: longs },
        draggable: true
    });
    var directionsService = new google.maps.DirectionsService;
    var directionsService2 = new google.maps.DirectionsService;
    var directionsDisplay = new google.maps.DirectionsRenderer({
        draggable: true,
        map: map
    });
    directionsDisplay.setMap(map);
    function calculateAndDisplayRoute(directionsService, directionsDisplay) {
        directionsService.route({
            origin: { lat: StartLtd, lng: StartLng },
            destination: { lat: EndLtd, lng: EndLng },
            waypoints: waypts,
            travelMode: 'DRIVING'
        }, function (response, status) {
            MyRoutes = response;

            if (status === 'OK') {
                console.log("myroutes", MyRoutes);
                //res1 = response.routes[0].legs[0].via_waypoints;
                //res1 = directionsDisplay.directions.routes[0].legs[0];
                var newRoute = response.routes[0].legs[0].via_waypoints;
                //   console.log("old", newRoute[0].lat() + "   " + newRoute[0].lng());
                if (waypts.length <= 22) {
                    directionsDisplay.setOptions({ preserveViewport: true });
                    directionsDisplay.setDirections(response);
                }
                var route = response.routes.legs;
                // var summaryPanel = document.getElementById('directions-panel');
                //summaryPanel.innerHTML = '';
                //// For each route, display summary information.
                //for (var i = 0; i < route.legs.length; i++) {
                //    var routeSegment = i + 1;
                //    summaryPanel.innerHTML += '<b>Route Segment: ' + routeSegment +
                //        '</b><br>';
                //    summaryPanel.innerHTML += route.legs[i].start_address + ' to ';
                //    summaryPanel.innerHTML += route.legs[i].end_address + '<br>';
                //    summaryPanel.innerHTML += route.legs[i].distance.text + '<br><br>';
                //}
            } else {
                window.alert('Directions request failed due to ' + status);
            }
        });
        if (waypts2.length > 0) {
            debugger
            directionsService2.route({
                origin: { lat: StartLtd, lng: StartLng },
                destination: { lat: EndLtd, lng: EndLng },
                waypoints: waypts2,
                travelMode: 'DRIVING'
            }, function (response, status) {
                if (status === 'OK') {
                    console.log("myroutes", MyRoutes);
                    //  res2 = response.routes[0].legs[0].via_waypoints;
                    //   console.log("res2",res2[0].lat()+"   "+res2[0].lng());
                    for (var i = 0; i < response.routes[0].legs.length; i++) {
                        console.log(response.routes[0].legs[i]);
                        MyRoutes.routes[0].legs.push(response.routes[0].legs[i]);

                    }

                    console.log(MyRoutes);
                    directionsDisplay.setOptions({ preserveViewport: true });
                    directionsDisplay.setDirections(MyRoutes);
                    var route = response.routes[0];
                    // var summaryPanel = document.getElementById('directions-panel');
                    //summaryPanel.innerHTML = '';
                    //// For each route, display summary information.
                    //for (var i = 0; i < route.legs.length; i++) {
                    //    var routeSegment = i + 1;
                    //    summaryPanel.innerHTML += '<b>Route Segment: ' + routeSegment +
                    //        '</b><br>';
                    //    summaryPanel.innerHTML += route.legs[i].start_address + ' to ';
                    //    summaryPanel.innerHTML += route.legs[i].end_address + '<br>';
                    //    summaryPanel.innerHTML += route.legs[i].distance.text + '<br><br>';
                    //}
                } else {
                    window.alert('Directions request failed due to ' + status);
                }
            });
        }


    }
    ///evenyt listener click on map
    function addLatLng(event) {
        debugger
        console.log(event);
        $("#ui-id-16").hide();
        // var path = poly.getPath();
        // path.push(event.latLng);
        AddLatLngToArray(event.latLng.lng(), event.latLng.lat());

        if (StartLtd == undefined) {
            debugger
            StartLtd = event.latLng.lat();
            StartLng = event.latLng.lng();
        }
        else if (EndLtd == undefined) {
            debugger
            EndLtd = event.latLng.lat();
            EndLng = event.latLng.lng();
            calculateAndDisplayRoute(directionsService, directionsDisplay);
        }
        else {
            debugger
            if (waypts.length <= 22) {
                var obj = { lat: EndLtd, lng: EndLng };
                waypts.push({
                    location: obj,
                    stopover: false
                });
            }
            else {
                var obj = { lat: EndLtd, lng: EndLng };
                waypts2.push({
                    location: obj,
                    stopover: false
                });
            }
            EndLtd = event.latLng.lat();
            EndLng = event.latLng.lng();
            calculateAndDisplayRoute(directionsService, directionsDisplay);
        }

        //obj = { lat:49.27816468568685, lng:-123.1263542175293}
        // calculateAndDisplayRoute(directionsService, directionsDisplay,);
        ////
        //if (startpath < 1) {
        //    startpath = 1;
        //}

        //if (CurrentRoute.length > 0) {
        if (waypts.length != 0) {
            $('#Undo').show();
        } else {
            $('#Undo').hide();
        }


    }
    /////distance
    function computeTotalDistance(result) {
        var total = 0;
        var myroute = result.routes[0];
        console.log(myroute);
        //var newRoute = result.routes[0].legs[0].via_waypoints;
        //console.log("new", newRoute[0].lat() + "   " + newRoute[0].lng());

        var w;
        console.log(myroute.legs.length);
        for (var i = 0; i < myroute.legs.length; i++) {
            total += myroute.legs[i].distance.value;
            w = myroute.legs[i].via_waypoints;

        }
        console.log(w);
        waypts = [];
        waypts2 = [];
        for (var i = 0; i < w.length; i++) {
            console.log("jhhjhhjhjhjhjh", w[i].lat() + " " + w[i].lng());
            if (i <= 22) {
                var obj = { lat: w[i].lat(), lng: w[i].lng() };
                waypts.push({
                    location: obj,
                    stopover: false
                });
            }
            else {
                var obj = { lat: EndLtd, lng: EndLng };
                waypts2.push({
                    location: obj,
                    stopover: false
                });
            }
        }
        var hjh = result.routes[0].legs[0];
        console.log(StartLtd, hjh.start_location.lat());
        console.log(EndLtd, hjh.end_location.lat());
        StartLtd = hjh.start_location.lat();
        StartLng = hjh.start_location.lng();
        EndLtd = hjh.end_location.lat();
        EndLng = hjh.end_location.lng();
        total = total / 1000;
        //document.getElementById('total').innerHTML = total + ' km';
    }
    directionsDisplay.addListener('directions_changed', function () {
        debugger
        $(".ui-tooltip").hide();
        $("#ui-id-16").hide();
        computeTotalDistance(directionsDisplay.getDirections());
    });
    //undo
    $('#Undo').click(function (ev) {

        debugger;
        var lngth = CurrentRoute.length - 1;
        //console.log(lngth);
        // poly.getPath().removeAt(lngth);
        CurrentRoute.splice(lngth, 1);
        if (waypts2.length != 0) {
            debugger
            var MyLastDestination = waypts2[waypts2.length - 1];
            console.log(MyLastDestination);
            EndLtd = MyLastDestination.location.lat;
            EndLng = MyLastDestination.location.lng;
            waypts2.splice(waypts2.length - 1, 1);
            calculateAndDisplayRoute(directionsService, directionsDisplay);
            map.setZoom(17);
            //if (StartLtd == undefined) {
            //    debugger
            //    StartLtd = event.latLng.lat();
            //    StartLng = event.latLng.lng();
            //}
            //else if (EndLtd == undefined) {
            //    debugger
            //    EndLtd = event.latLng.lat();
            //    EndLng = event.latLng.lng();
            //    calculateAndDisplayRoute(directionsService, directionsDisplay);
            //}
            //else {
            //    debugger
            //    var obj = { lat: EndLtd, lng: EndLng };
            //    waypts.push({
            //        location: obj,
            //        stopover: true
            //    });
            //    EndLtd = event.latLng.lat();
            //    EndLng = event.latLng.lng();
            //    calculateAndDisplayRoute(directionsService, directionsDisplay);
            //}
        }
        else {
            debugger
            var MyLastDestination = waypts[waypts.length - 1];
            console.log(MyLastDestination);
            EndLtd = MyLastDestination.location.lat;
            EndLng = MyLastDestination.location.lng;
            waypts.splice(waypts.length - 1, 1);
            calculateAndDisplayRoute(directionsService, directionsDisplay);
            map.setZoom(17);
        }
        if (waypts.length != 0) {
            $('#Undo').show();
        } else {
            $('#Undo').hide();
        }

    });
    $(".ui-tooltip ui-widget ui-corner-all ui-widget-content").click(function (ev) {
        debugger
        $(".ui-tooltip").hide();
        $("#ui-id-16").hide();
    });
    poly = new google.maps.Polyline({
        strokeColor: PathColor,
        strokeOpacity: 1.0,
        strokeWeight: 3
    });
    poly.setMap(map);
    PolyLines.push(poly);


    map.panTo(centerPt);
    map.setZoom(17);

    map.addListener('click', addLatLng);

    $('#clear').click(function (ev) {
        $('#map2').empty();
        map = null;
        centerPt = null;
        var PathColor = '#CC0000';
        StartLtd = undefined, StartLng = undefined, EndLtd = undefined, EndLng = undefined;
        waypts = [];
        waypts2 = [];
        ShowMapWindow('bySiteCode', SelectedSiteId, SelectedSiteCode, Latitude, Longitude, SelectedScope, SelectedScopeId, currentClientPrefix);
        //LoadMapWithAzimuth(Latitude, Longitude, SelectedSiteCode)
        SelectedRow = 0;

    });

    $('#new').click(function (ev) {

        if (AddRouteToArray()) {
            startpath = 0;
            PathColor = SelectedColor;
            poly = new google.maps.Polyline({
                strokeColor: PathColor,
                strokeOpacity: 1.0,
                strokeWeight: 3
            });
            poly.setMap(map);
            PolyLines.push(poly);
        }


    });
    $('#Undo').hide();
    //Close

    $('#Close').click(function () {
        $('#map2').empty();
        map = null;
        centerPt = null;
        var PathColor = '#CC0000';
        StartLtd = undefined, StartLng = undefined, EndLtd = undefined, EndLng = undefined;
        waypts = [];
        waypts2 = [];
        modal_body.empty();
        modal.modal('hide');
    });

    //    $('#Undo').click(function (ev) {

    //        if (waypts.length <= 22)
    //        {
    //            waypts.splice(waypts.length-1,1)
    //        }
    //        else {
    //            waypts2.splice(waypts2.length - 1, 1)
    //        }
    //        //   debugger;
    //        var lngth = CurrentRoute.length -1;
    //        //console.log(lngth);
    //        poly.getPath().removeAt(lngth);
    //        CurrentRoute.splice(lngth, 1);

    //        if (CurrentRoute.length > 0) {
    //            $('#Undo').show();
    //        } else {
    //            $('#Undo').hide();
    //    }

    //});

    //poly.addListener('click', polyOnClick);
    google.maps.event.addListener(poly, 'click', function (event) {
        alert(this.id);

    });

    google.maps.event.addListenerOnce(map, 'idle', function () {
        //alert('after initialize function');
        google.maps.event.trigger(map, 'resize');
        map.setCenter({
            lat: lats,
            lng: longs
        });
    });


}
function polyOnClick(event) {

}

function setMapOnAll(map) {
    for (var i = 0; i < Routemarkers.length; i++) {
        Routemarkers[i].setMap(map);
    }
}

function NewTabDrive(filter, siteid, SiteCode, lat, lng, Scope, ScopeId, ClientPrefix) {
    debugger
    window.open("/Dashboard/Drive?SiteId=" + siteid + '&Scope=' + Scope + '&SelectedSiteCode=' + SiteCode + '&Latitude=' + lat + '&Longitude=' + lng + '&SelectedScopeId=' + ScopeId + '&currentClientPrefix=' + ClientPrefix + '&filter=' + filter, "_blank");
}

function ShowMapWindow(filter, siteid, SiteCode, lat, lng, Scope, ScopeId, ClientPrefix) {
    Routemarkers = [];
    startpath = 0;
    Routes = [];
    CurrentRoute = [];
    RoutesList = [];
    RoutesList1 = [];
    PolyLines = [];
    centerPt;
    SelectedRouteSite = 0;
    Latitude = lat;
    Longitude = lng;
    SelectedScope = Scope;
    PathColor = '#CC0000';
    // debugger;
    startpath = 0;
    AzRecord = null
    currentClientPrefix = ClientPrefix;

    SelectedSiteId = siteid;
    var body = '<div id="Site-Save-Alert" class="alert alert-success fade in alert-dismissable" style="margin-top:18px; display:none;">' +
                    '<a href="#" class="close" data-dismiss="alert" aria-label="close" title="close">×</a>' +
                    '<strong>Success!</strong> Route saved successfully...!' +
                '</div>' +
        '<div style="width:100%;height:500px; margin-bottom:20px;" id="map2"></div>' +

'<button style="margin-right:5px;" class="btn btn-success" id="1050"  id="" onclick=SaveKml();>Export To Kml</button>' +
'<button style="margin-right:5px;" class="btn btn-primary" id="new">Create new path</button>' +
        '<button style="margin-right:5px;" class="btn btn-danger" id="clear">Clear</button>';

    $.ajax({
        url: '/Dashboard/DriveRoute?SiteId=' + siteid + '&Scope=' + Scope,
        async: false,
        success: function (res) {
            body = res;
        }
    });
    modal_title.text('Plan Drive Route');
    modal
    modal_content.css({ 'width': '80%' });
    modal
    modal_footer.hide();
    modal_body.empty();
    modal_body.html(body);
    modal.modal('show');


    ColorPicker("#RouteColor");
    LoadMapWithAzimuth(lat, lng, SiteCode)

    SelectedRouteSite = siteid;
    SelectedSiteCode = SiteCode;
    SelectedScopeId = ScopeId
}

function LoadMapWithAzimuth(lat, lng, SiteCode) {
    initMap(lat, lng);
    google.maps.event.trigger(map, 'resize');
    MarketSites(SiteCode, lat, lng);
}

function Azmiuth(Angles, Radius) {
    //  debugger;
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
    for (; i < Angles.length; i++) {

        latlng = new google.maps.LatLng(Angles[i].Latitude, Angles[i].Longitude);
        var path = getArcPath(latlng, radiusMeters, Angles[i].StartAngle, Angles[i].EndAngle);
        // console.log(path);

        path.unshift(latlng);
        path.push(latlng);
        var poly = new google.maps.Polygon({
            path: path,
            map: map,
            fillColor: Angles[i].Color,
            fillOpacity: 1,
        });
        polys.push(poly);
    }
}

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

function ColorPicker(Selector) {


    $(Selector).spectrum({
        preferredFormat: "hex",
        color: PathColor,
        showInput: true,
        className: "full-spectrum",
        showPaletteOnly: true,
        showPalette: true,
        hideAfterPaletteSelect: true,
        move: function (color) {

        },
        show: function () {
            //$('.full-spectrum').css({
            //    'top': '500px'
            //});


        },
        beforeShow: function () {

        },
        hide: function () {
            startpath = 0;
            debugger;
            AddRouteToArray();

            PathColor = SelectedColor;
            poly = new google.maps.Polyline({
                strokeColor: PathColor,
                strokeOpacity: 1.0,
                strokeWeight: 3
            });
            poly.setMap(map);
            PolyLines.push(poly);

        },
        change: function () {

            SelectedColor = $(this).val();

        },
        palette: [
            ["#A0522D", "#CD5C5C", "#FF4500", "#008B8B"],
             ["#B8860B", "#32CD32", "#FFD700", "#48D1CC"],
             ["#87CEEB", "#FF69B4", "#CD5C5C", "#87CEFA"],
             ["#6495ED", "#DC143C", "#FF8C00", "#C71585"],
             ["#000000"]

        ]
    });

    //  $('.sp-replacer').append('Add Aditional Routes');

}


//$(document).on('change', '#RouteColor', function () {


//    SelectedColor = $(this).val();
//});



$(document).on('click', '#RouteColor', function () {
    //  ColorPicker(this);
});

$(document).on('click', '#tbl-route tr', function () {

    LoadMapWithAzimuth(Latitude, Longitude, SelectedSiteCode);
    $("#tbl-route tr").removeClass("SelectedRow");
    var RouteId = $(this).attr('data-RouteId');
    SelectedRow = RouteId;
    var myParser = new geoXML3.parser({ map: map });
    $(this).addClass("SelectedRow");

    try {
        //console.log(domain);
        myParser.parse(domain + '/Content/AirViewLogs/' + currentClientPrefix + '/' + SelectedSiteCode + '/route-' + RouteId + '.kml');
    } catch (e) {
        myParser.parse(domain + '/Content/AirViewLogs/' + currentClientPrefix + '/' + SelectedSiteCode + '/route-' + RouteId + '.kml');
    }




});


$(document).on('hover', '#map2', function () {

    $(this).css({
        'cursor': 'pointer'
    });

});


$(document).on('click', '.IsSelected', function () {
    var IsSelected = false;
    if ($(this).is(":checked")) {
        IsSelected = true;
    }
    $.ajax({
        url: '/Dashboard/ManageDriveRoute',
        type: 'post',
        data: { 'Filter': 'UpdateIsSelected', 'RouteId': $(this).attr('data-RouteId'), 'IsSelected': IsSelected },
        success: function (res) {
            //  console.log(res);
        }

    });

});





