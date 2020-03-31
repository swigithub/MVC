
///---controller SiteRoute
//----action Index---

//---==global variables==-->

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

    var obj = { lat: lats, lng: longs };
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
    var Data;
    if (SelectedRow > 0) {
        $.confirm({
            title: 'Confirm!',
            content: 'Do you want to Save As new Drive Route? \nPress Yes for Save As new Drive Route! \nPress No for Update Existly Route Cancel!',
            buttons: {
                Yes: function () {
                    Data = { 'cordinates': Routes, 'SiteId': SiteId, 'SiteCode': SelectedSiteCode, 'TestType': TestTypes, 'ScopeId': SelectedScopeId, 'RouteId': SelectedRow, 'Filter': 'Insert', 'ClientPrefix': currentClientPrefix }
                    fnSave(Data)
                },
                No: function () {
                    Data = { 'cordinates': Routes, 'SiteId': SiteId, 'SiteCode': SelectedSiteCode, 'TestType': TestTypes, 'ScopeId': SelectedScopeId, 'RouteId': SelectedRow, 'Filter': 'Update', 'ClientPrefix': currentClientPrefix }
                    fnSave(Data)
                },
                Cancel: function () {
                    ShowMapWindow('bySiteCode', SelectedSiteId, SelectedSiteCode, Latitude, Longitude, SelectedScope, SelectedScopeId, currentClientPrefix);
                }

            }
        });
        
    } else {
        Data = { 'cordinates': Routes, 'SiteId': SiteId, 'SiteCode': SelectedSiteCode, 'TestType': TestTypes, 'ScopeId': SelectedScopeId, 'RouteId': SelectedRow, 'Filter': 'Insert', 'ClientPrefix': currentClientPrefix }
        fnSave(Data)

    }

}

function fnSave(Data) {
    debugger;
    AddRouteToArray();
    // debugger;
    if (Routes != null) {
        $.ajax({
            url: '/Dashboard/DriveRoute',
            type: 'POST',
            data: Data,
            success: function (res) {
                if (res != 'session expired') {
                    if (res.Status == 'success') {
                        //modal_body.empty();
                        //modal.modal('hide');
                        $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 2000, });
                        ShowMapWindow('bySiteCode', SelectedSiteId, SelectedSiteCode, Latitude, Longitude, SelectedScope, SelectedScopeId, currentClientPrefix);

                    } else {
                        $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 2000, });
                    }
                } else {
                    $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }



            },
            error: function () {
                alert("error");
            }
        });
        SelectedRow = 0;
    } else {
        $('#RouteMsg').text('please draw atleast one route before saving...!');
        return false;
    }

}

function initMap(lats, longs) {

    Routes = [];

    $("#tbl-route tr").removeClass("SelectedRow");

    centerPt = new google.maps.LatLng(lats, longs);
    map = new google.maps.Map(document.getElementById('RouteMap'), {
        zoom: 7,
        center: { lat: lats, lng: longs }
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
        $('#RouteMap').empty();
        map = null;
        centerPt = null;
        var PathColor = '#CC0000';

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

        modal_body.empty();
        modal.modal('hide');
    });

    $('#Undo').click(function (ev) {

        //   debugger;
        var lngth = CurrentRoute.length - 1;
        //console.log(lngth);
        poly.getPath().removeAt(lngth);
        CurrentRoute.splice(lngth, 1);

        if (CurrentRoute.length > 0) {
            $('#Undo').show();
        } else {
            $('#Undo').hide();
        }

    });

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


function addLatLng(event) {
    var path = poly.getPath();
    path.push(event.latLng);
    AddLatLngToArray(event.latLng.lng(), event.latLng.lat());

    if (startpath < 1) {
        startpath = 1;
    }

    if (CurrentRoute.length > 0) {
        $('#Undo').show();
    } else {
        $('#Undo').hide();
    }

}

function ShowMapWindow(filter, siteid, SiteCode, lat, lng, Scope, ScopeId,ClientPrefix) {
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
        '<div style="width:100%;height:500px; margin-bottom:20px;" id="map"></div>' +

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
    modal_content.css({ 'width': '80%' });
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


$(document).on('hover', '#RouteMap', function () {

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


