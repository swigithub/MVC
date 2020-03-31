var testerId = 0;
var siteId = 0;
var tempTesterId = 0;
var RegionSiteTitle = 'Market View';
var SelectedRegion = '';
var SelectedRegionOrMarket = '0';
var SelectedViewTester = '0';
var RegionLoaded = 0;

var TesterSiteTitle = 'Drive Tester View';
var tester_id = '1';
var region_id = '1';
var table;
var tblBands;

var doc = $(document);
var RouteInfo = '';
var RouteInfoDistance = '';
var RouteInfoDuration = '';

var loading = $.loading();
var IsMapZoom = 0;
var IsLatLng = 0;
var lat = 0;
var lng = 0;
var markers = {};
fnWoStatus();

//function handleError() {
//    this.src = ResolveUrl("~/Content/Images/Profile/Default.svg");
//}


//$(document).ready(function () {
//    $("img").one("load", function () {
       
//    }).each(function () {
//        if (this.complete)
//        {
//            $(this).load();
//        }
//        else {
//            console.log("hide");
//            $(this).hide()
//        };
//    });

//});



function Selectdmultiselect(Selecter, Values, Concate) {
    var selectedOptions = $(Values).val().split(",");
    for (var x = 0; x < selectedOptions.length; x++) {
        if (selectedOptions[x] != 0) {
            $(Selecter).find("option[value=" + Concate + selectedOptions[x] + "]").prop("selected", "selected");
        }
    }
    //for (var i in selectedOptions) {
    //    var optionVal = selectedOptions[i];
    //    $(Selecter).find("option[value="+Concate + optionVal + "]").prop("selected", "selected");
    //}
    $(Selecter).multiselect('refresh');
}

    


$('#ddlCountry').multiselect({
    //includeSelectAllOption: true,
    //nonSelectedText: '-Country-',
    selectedClass: 'multiselect-selected',
    allSelectedText: 'selected',
    numberDisplayed: 1,
    enableFiltering: true,
    maxHeight: 400,
    buttonWidth: '80%',
    enableClickableOptGroups: true,
    enableCollapsibleOptGroups: true,
    onDropdownHide: function (event) {
        debugger;
        var selected = $("#ddlCountry option:selected");
        var Countries = "";
        var Clients = "";
        var Scopes = "";
        var Markets = "";
        var Projects = "";
        selected.each(function () {
            var value = $(this).val();
           if (value.startsWith("cntr")) {
                Countries += $(this).val().slice(5) + ',';
            } else if (value.startsWith("clnt")) {
                Clients += $(this).val().slice(5) + ',';
            }
            else if (value.startsWith("scop")) {
                Scopes += $(this).val().slice(5) + ',';
            } else if (value.startsWith("city")) {
                Markets += $(this).val().slice(5) + ',';
            } else if (value.startsWith("proj")) {
                Projects += $(this).val().slice(5) + ',';
            }
            
        });
        if (Countries.trim().length > 0) {
            $("#Countries").val(Countries.slice(0, -1))
            
        } else {
            $("#Countries").val('0')
          
        }

        if (Clients.trim().length > 0) {
            $("#Clients").val(Clients.slice(0, -1))
        } else {
            $("#Clients").val('0')
        }


        if (Scopes.trim().length > 0) {
            $("#Scopes").val(Scopes.slice(0, -1))
        } else {
            $("#Scopes").val('0')
        }

        if (Markets.trim().length > 0) {
            $("#Markets").val(Markets.slice(0, -1))
        } else {
            $("#Markets").val('0')
        }
        if (Projects.trim().length > 0) {
            $("#Projects").val(Projects.slice(0, -1))
        } else {
            $("#Projects").val('0')
        }
    },

});
Selectdmultiselect('#ddlCountry', '#Countries', 'cntr-');
Selectdmultiselect('#ddlCountry', '#Clients', 'clnt-');
Selectdmultiselect('#ddlCountry', '#Scopes', 'scop-');
Selectdmultiselect('#ddlCountry', '#Markets', 'city-');
Selectdmultiselect('#ddlCountry', '#Projects', 'proj-');




//function showCoords(event,SiteId) {

//    var x = event.clientX;
//    var y = event.clientY;
//    console.log(SiteId);
//    $('#m-'+SiteId).closest('ul.dropdown-menu').css({
//        'position': 'fixed',
//        'left': x,
//        'top':y,
//    });
//}

//console.log(getDistanceFromLatLonInKm( 30.448528, -86.616381,28.073013, -82.416627));
//function getDistanceFromLatLonInKm(lat1, lon1, lat2, lon2) {
//    var R = 6371; // Radius of the earth in km
//    var dLat = deg2rad(lat2 - lat1);  // deg2rad below
//    var dLon = deg2rad(lon2 - lon1);
//    var a =
//      Math.sin(dLat / 2) * Math.sin(dLat / 2) +
//      Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
//      Math.sin(dLon / 2) * Math.sin(dLon / 2)
//    ;
//    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
//    var d = R * c; // Distance in km
//    return d;
//}

//function deg2rad(deg) {
//    return deg * (Math.PI / 180)
//}

//var rad = function (x) {
//    return x * Math.PI / 180;
//};

//var getDistance = function (p1, p2) {
//    var R = 6378137; // Earth’s mean radius in meter
//    var dLat = rad(p2.lat() - p1.lat());
//    var dLong = rad(p2.lng() - p1.lng());
//    var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
//      Math.cos(rad(p1.lat())) * Math.cos(rad(p2.lat())) *
//      Math.sin(dLong / 2) * Math.sin(dLong / 2);
//    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
//    var d = R * c;
//    return d; // returns the distance in meter
//};

//console.log(getDistance(new google.maps.LatLng(30.448528, -86.616381), new google.maps.LatLng(28.073013, -82.416627)));


function onMapLoadHandler(args) {
    map = args.map;
    markers = args.markers;
    // var length = Object.keys(markers).length

    if (SelectedRegion.trim()) {
        if (RegionLoaded == 0) {
            geocodeAddress(SelectedRegion);
            RegionLoaded = 1;
        }
    }

    $.each(markers, function (i) {
        if (this.title == 'IN_PROGRESS') {
            this.setAnimation(google.maps.Animation.BOUNCE)
            //   google.maps.event.trigger(this, 'click');
        }
    });
}


function geocodeAddress(address) {

    try {
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status === 'OK') {
                lat = results[0].geometry.location.lat();
                lng = results[0].geometry.location.lng();
                return false;
                //resultsMap.setCenter(results[0].geometry.location);
                //var marker = new google.maps.Marker({
                //    map: resultsMap,
                //    position: results[0].geometry.location
                //});
            } else {
                //console.log('Geocode was not successful for the following reason: ' + status);
                lat = 0;
                lng = 0;
            }
        });
    } catch (e) {
        lat = 0;
        lng = 0;
    }

    return false;
}

function OnMapIdle(args) { }


function OnZoomChanged(args) {
    var length = Object.keys(markers).length
    $.each(markers, function () {
        if (this.title == 'IN_PROGRESS') {
            this.setAnimation(google.maps.Animation.BOUNCE)
            //   google.maps.event.trigger(this, 'click');
        }
    });
}
function OnMouseOver(args) {

    if (SelectedRegion.trim()) {
        if (IsMapZoom == 0) {
            if (lat != 0 && lng != 0) {
                var arizonaCoor = new google.maps.LatLng(lat, lng);
                map.setZoom(10);
                map.panTo(arizonaCoor);
                IsMapZoom++;
            }
        }
    }

    if (testerId > 0) {
        var waypts = [];
        $.ajax({
            url: '/Dashboard/GetRoutePlan',
            type: 'post',
            data: { 'filter': 'GET_TESTER_ROUTE', 'TesterId': testerId },
            success: function (res) {

                   // console.log(res);
                if (res.length > 1) {
                    RemoveGoogleMapRoute();
                    directionsService = new google.maps.DirectionsService;
                    directionsDisplay = new google.maps.DirectionsRenderer({ suppressMarkers: true });
                    for (var i = 1; i < res.length - 1; i++) {
                        waypts.push({
                            location: res[i].Latitude + ',' + res[i].Longitude,
                            //stopover: true
                        });
                    }
                    var origin = res[0].Latitude + ',' + res[0].Longitude;
                    var destination = res[res.length - 1].Latitude + ',' + res[res.length - 1].Longitude;
                    calculateAndDisplayRoute(origin, destination, waypts, directionsService, directionsDisplay)
                    directionsDisplay.setMap(args.map);
                    // directionsDisplay.setMap(null);
                } else {
                    RemoveGoogleMapRoute();
                }

            }
        });

    }

}

var directionsService;
var directionsDisplay;

function getMiles(i) {
    return i * 0.000621371192;
}

function getMinutes(second) {
    return second / 60;
}

function RemoveGoogleMapRoute() {
    if (directionsService != null) {
        directionsDisplay.setMap(null);
        RouteInfo = '';
        $('.RouteInfo').text(RouteInfo);
        RouteInfoDistance = '0';
        RouteInfoDuration = '0';
        // directionsDisplay = null;
    }
}


function calculateAndDisplayRoute(origin, destination, waypts, directionsService, directionsDisplay) {
    var icons = {
        start: new google.maps.MarkerImage(
         'https://chart.googleapis.com/chart?chst=d_map_xpin_icon&chld=pin_star|car-dealer|00FFFF|FF0000',
         new google.maps.Size(44, 32),
         new google.maps.Point(0, 0),
         new google.maps.Point(22, 32)
        ),
        end: new google.maps.MarkerImage(
         'https://chart.googleapis.com/chart?chst=d_map_xpin_icon&chld=pin_star|car-dealer|00FFFF|FF0000',
         // (width,height)
         new google.maps.Size(44, 32),
         // The origin point (x,y)
         new google.maps.Point(0, 0),
         // The anchor point (x,y)
         new google.maps.Point(22, 32)
        )
    };

    directionsService.route({
        origin: origin,//'Chicago, IL, United States',
        destination: destination,// '38.627003,-90.199404',// 'St. Louis, MO, United States',
        waypoints: waypts,
        optimizeWaypoints: true,
        travelMode: 'DRIVING'
    }, function (response, status) {
        if (status === 'OK') {
            directionsDisplay.setDirections(response);
          //  console.log(response);
            //   var leg = response.routes[0].legs[0];
            //    console.log(leg);
            //makeMarker(leg.start_location, icons.start, "title");
            //makeMarker(leg.end_location, icons.end, 'title');
            //var latlng = new google.maps.LatLng(33.8882, -84.0598306);

            //makeMarker(latlng, icons.end, 'title');
            var route = response.routes[0];
            var TotalMiles = 0;
            var TotalTime = 0;
            for (var i = 0; i < route.legs.length; i++) {
               // var routeSegment = i + 1;
                //  console.log(route);
            
                TotalMiles = TotalMiles + getMiles(route.legs[i].distance.value);
                TotalTime = TotalTime + getMinutes(route.legs[i].duration.value);

                RouteInfoDistance = route.legs[i].distance.text;
                RouteInfoDuration = route.legs[i].duration.text;
                RouteInfo = 'Distance: ' + TotalMiles.toFixed(2) + ' mi, Duration: ' + TotalTime.toFixed(2) + ' mins';
                $('.RouteInfo').text(RouteInfo);//'Distance: ' + RouteInfoDistance + ', Duration: ' + RouteInfoDuration
               // modal_title.append('<span style="font-size: small;" class="pull-right">   Dist:' + TotalMiles.toFixed(2) + ' mi, Dur:' + TotalTime.toFixed(2) + ' mins</span>');
                //console.log(route.legs);

            }

        } else {
           // window.console.log('Directions request failed due to ' + status);
        }
    });


    function makeMarker(position, icon, title) {
        new google.maps.Marker({
            position: position,
            map: map,
            icon: icon,
            title: title
        });
    }
}


function markerClick(args) {
    if (args.eventName === 'click') {
        //  args.marker.setAnimation(google.maps.Animation.BOUNCE);
    }
}


function ddlTestTypeChange(row) {
    
    var val = $('#ddlTestType-' + row).val();
    $('#TestTypes-' + row).val(val.join(","));
    //console.log(val.join(","));
    
}

//$("select").select2();

var TesterScheduled = $('.TesterScheduled').length;
$('#TesterScheduled').text(TesterScheduled);
var TesterAvailable = $('.TesterAvailable').length;
$('#TesterAvailable').text(TesterAvailable);



$('#breadcrumspan').css({
    'visibility': 'visible'
});
var UploadSiteLength = $('.UploadSite').length;
if (UploadSiteLength == 0) {
    $('#UploadTitle').text('');
}
$(".UploadSite:first").click();




/*----MoB!----*/
$("#divtestersites").fadeToggle();

doc.on('click', '#btn-PickDate', function () {
    $('#pan-date').css({
        "visibility": "visible"
    });
});



/**********WO STATUS ****************/
$('.wo-date-box').css({
    'visibility': 'hidden'
});
doc.on('click', '.txt-status', function () {
    $('#wo-status').text($(this).attr('data-txt'));
});
doc.on('click', '.date-filter', function () {
    var value = $(this).attr('data-DisplayValue');
    $('#date-filter-txt').text(value);

    $('#date-filter-title-txt').text($(this).attr('data-displayText'));
    $('.wo-date-box').css({
        'visibility': 'hidden'
    });
});

doc.on('click', '.btn-date-filter-search', function () {
    var value = $('#fromdate').val() + ' - ' + $('#todate').val();
    $('#date-filter-txt').text(value);

    $('#date-filter-title-txt').text('Date Range');

});
doc.on('click', '#btn-wo-PickRange', function () {
    $('.wo-date-box').css({
        'visibility': 'visible'
    });
});

/**********WO STATUS END****************/
doc.on('click', '.region-val-txt', function () {
    $('#Region-txt').text(RegionSiteTitle + ' : ' + $(this).text());
    SelectedRegion = $(this).text().trim();
    RegionLoaded = 0;
});

doc.on('click', '.hideRange', function () {
    $('#divSiteRangeFilter').css({
        "display": "none",
        "margin-left": "-82px"
    });
});

doc.on('click', '.ApproveWithComment', function () {
    $('#SiteId-ApproveWithComment').val($(this).attr('data-siteid'));

    var css = { 'width': '500px' };
    fnOpenModal('Approve with comment', $('#sec-ApproveWithComment').html(), css);


    return false;
});


//frm-ApproveWithComment
$('.close').click(function () {
    fnHideModal();
});
doc.on('submit', '#frm-ApproveWithComment', function () {
    $.ajax({
        url: '/Site/ChangeStatus',//?SiteId=' + SiteId + '&Status=' + Status,
        type: "post",
        data: $(this).serialize(),
        success: function (res) {
            if (res.Status == 'success') {
                BtnFilterSitesGrid_Click('', '', '');
                fnHideModal();
            } else {
                $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
            }
        },
        error: function (err) {
            $.notify(err.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });

        },
    });
    return false;
});

// $('#frm-UploadSiteLogs').hide();
doc.on('click', '.UploadSite', function () {
    var value = $(this).attr('data-value');
    if (value == 'wo') {
        $('#UploadTitle').text('Upload Work Orders');
        // $('#frm-UploadSiteLogs').hide();
        $('#frm-UploadSiteLogs').css({
            'visibility': 'hidden'
        });
        $('#frmuploadfile').show();
    } else if (value == 'log') {
        $('#frmuploadfile').hide();
        $('#frm-UploadSiteLogs').css({
            'visibility': 'visible'
        });
        //  $('#frm-UploadSiteLogs').show();
        $('#UploadTitle').text('Upload SiteLogs');


    }

});
doc.on('click', '.tester-val-txt', function () {
    $('#tester-txt').text(TesterSiteTitle + ' : ' + $(this).text());
    return false;
});




/************USER COUNTRIES OR CLIENTS***************/
var PrevCountry = '';
if (UserClients != null) {
    $.each(UserClients, function (i, v) {
        if (v.CountryName != PrevCountry) {
            //  $('#ddlCountry').append('<option value="' + v.CountryId + '">' + v.CountryName + '</option>');
            $('#country-menu').append('<li><a class="CountryVelue" data-value="' + v.CountryId + '">' + v.CountryName + '</a></li>');
            //
            PrevCountry = v.CountryName;
        }
    });
}

doc.on('click', '.CountryVelue', function () {

    var CountryId = $(this).attr('data-value');
    var CountryTitle = $(this).text();

    $('#client-menu').empty();
    $('#client-menu').append('<li><a class="client-value" data-value="0">All</a></li>');
    if (UserClients != null) {
        $.each(UserClients, function (i, v) {

            if (v.CountryId == CountryId) {
                $('#client-menu').append('<li><a class="client-value" data-value="' + v.ClientId + '">' + v.ClientName + '</a></li>');
            }
        });
    }

    $('#SelectedCountry').text(CountryTitle);
    $('#SelectedClient').text("All Clients");
    BtnFilterSitesGrid_Click('Country', CountryId, '');
});

doc.on('click', '.client-value', function () {

    var ClientId = $(this).attr('data-value');
    var ClientTitle = $(this).text();
    $('#SelectedClient').text(ClientTitle);
    BtnFilterSitesGrid_Click('Client', ClientId, '');
});
/************USER COUNTRIES OR CLIENTS END***************/
var divRegionalSites = $('#divsitelocations');
doc.on('click', '#pan-tester', function () {
    RemoveGoogleMapRoute();
    IsMapZoom = 0;
    $("#divtestersites").fadeToggle('slow', function () {
        if ($("#divtestersites").is(":visible")) {
            //var ur = '@Url.Action("SiteLocationsForSchedule", "Dashboard")';
            $.ajax({
                url: '/Dashboard/SiteLocationsForSchedule',
                type: 'POST',
                async: false,
                dataType: 'text',
                processData: false,
                success: function (data) {
                    if (data == "success") {

                    }
                    else {

                        divRegionalSites.empty();
                        divRegionalSites.html(data);
                    }
                }
            });
        }
        else { }
    });

});

doc.on('click', '.TesterTitle', function () {
    var text = $(this).text();
    TesterSiteTitle = text;
    var filter = $(this).attr("data-filter");
    $('#TesterTitle').text(text);

    if (SelectedViewTester == '0') {
        $.ajax({
            url: '/Dashboard/TesterSummery?filter=' + filter,
            type: 'POST',
            success: function (data) {
                if (data == "") {
                    window.location.reload();
                }
                else {
                    var divdrivetesterview = $('#divdrivetesterview');
                    divdrivetesterview.empty();
                    divdrivetesterview.html(data);
                    fnWoStatus();
                    $('#TesterTitle').text(TesterSiteTitle);
                }
            }
        });
    } else {
        $('#TesterTitle').text(TesterSiteTitle);
        BtnFilterSitesGrid_Click('Panel2Filter', filter, '');
        SelectedViewTester = '0';
    }

    $('#tester-txt').text(text + ' : All');

});

doc.on('click', '.RegionalSiteTitle', function () {
    SelectedRegion = '';
    var t = $(this).text();
    RegionSiteTitle = t;
    var id = $(this).attr("data-value");
    region_id = id;

    $('#Region-txt').text(t + ' : All');
    var filter = $(this).attr("data-filter");
    if (SelectedRegionOrMarket=='0') {
        var ur = '/Dashboard/RegionalSummery?Filter=' + filter;
        $.ajax({
            url: ur,
            type: 'POST',
            success: function (data) {
                if (data == "") {
                    window.location.reload();
                }
                else {
                    var divRegionalSites = $('#divregionalsites');
                    divRegionalSites.empty();
                    divRegionalSites.html(data);
                    fnWoStatus();
                    $('#RegionalSiteTitle').text(t);

                }
            }
        });
    } else {

        BtnFilterSitesGrid_Click('RegionFilter', filter, '');
        SelectedRegionOrMarket = '0';
        $('#RegionalSiteTitle').text(t);
    }
});



$(document).on('click', '.dateSelection', function () {
    var value = $(this).attr('data-value');
    $('#schduledate').val(value);
    $('.dateSelection').removeClass('selectedDate');
    $(this).addClass('selectedDate');
    //  $('#EmptyClick').click();
});


$(document).on('keydown', '#schduledate', function () {
    $("input[id='schduledate']").removeClass("error");
});

       

$(document).on('click', '.SiteWoHold', function () {

    var SiteId=$(this).attr('data-SiteId');
    $.confirm({
        title: 'Confirm!',
        content: 'Do you want to Hold Workorder?',
        buttons: {
            Yes: function () {
                $.ajax({
                    url: '/Site/WoHold?SiteId=' + SiteId,
                    type: 'post',
                    success: function (res) {
                        if (res != 'session expired') {
                            if (res.Status == 'success') {
                                BtnFilterSitesGrid_Click('', '', '');
                            } else {
                                $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            }
                        } else {
                            $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                        }
                        
                    }
                });
            },
            No: function () { }

        }
    });



    //var r = confirm('Do you want to Hold Workorder ?');
    //if (r == true) {
        
    //}
    return false;

});

$(document).on('keydown', '#fromdate', function () {
    $("input[id='fromdate']").removeClass("error");
    $("#divDateRangeError").css("display", "none");
});

$(document).on('keydown', '#todate', function () {
    $("input[id='todate']").removeClass("error");
    $("#divDateRangeError").css("display", "none");
});

var ReportWindow;
var ReportOpen = false;
$(document).on('click', '.NetLayerReport', function () {
    if (ReportOpen == true) {
        ReportWindow.close();
    }

    var link = $(this).attr('href');
    ReportWindow = window.open(link, "_blank");//, "width=500, height=500"
    ReportOpen = true;
    return false;
});


//   loading.open();
/*********SITE TRACKER ISSUE OR ACTIVITY ***********/

       

/*********SITE TRACKER ISSUE  OR ACTIVITY END ***********/




$(document).on('submit', '#frm-GridSchedule', function () {
     var sdate = $("input[id='schduledate']").val();
    $("input[id='schduledate']").removeClass("error");
    if (sdate == "") {
        $("input[id='schduledate']").addClass("error");
        return;
    }

    //var ur = '@Url.Action("GridSchedule", "Site")'
    $.ajax({
        url: '/Site/GridSchedule',//ur,
        type: 'POST',
        data: $(this).serialize(),
        // async: false,
        success: function (data) {
            if (data == "success") {
                $('#av-modal').modal('hide');
                BtnFilterSitesGrid_Click('', '', '');
            } else {
                //console.log(data);
            }
        },
        error: function (err) {
           // console.log(err.statusText);

        }
    });


    return false;
});



        
 

      
       

       
//btn-email
$(document).on('click', '.btn-email', function () {

    var SiteId = $(this).attr('data-SiteId');
    var NetworkModeId = $(this).attr('data-NetworkModeId');
    var NetworkMode = $(this).attr('data-NetworkMode');
    var CarrierId = $(this).attr('data-CarrierId');
    var Carrier = $(this).attr('data-Carrier');
    var ScopeId = $(this).attr('data-ScopeId');
    var BandId = $(this).attr('data-BandId');
    var Band = $(this).attr('data-Band');
    var SiteCode = $(this).attr('data-SiteCode');
    var City = $(this).attr('data-City');
    var Region = $(this).attr('data-Region');
    var Scope = $(this).attr('data-Scope');
    var Subject = SiteCode + ' (' + Band + '_' + Carrier + ') Market: ' + Region + ' ' + City;

    var url = '/site/ntlFieldTest';
    var data = { 'SiteId': SiteId, 'BandId': BandId, 'Carrier': CarrierId, 'NetworkMode': NetworkModeId };
    if (Scope=='NI') {
        url = '/site/ntlFieldTestNI';
        data = { 'SiteId': SiteId, 'BandId': BandId, 'Carrier': CarrierId, 'NetworkMode': NetworkModeId,'Scope':Scope };
    }


    $.ajax({
        url: url,
        data: data,
        // async: false,
        success: function (res) {
            $('#tempData').html(res);
            $('.rptFooter').empty();

            if (Scope != 'NI') {
                $.each(ReportConfiguration, function (i, c) {
                    var DataTypes = $('#' + c.KeyCode).attr('data-type');
                    if (DataTypes == undefined) {
                        DataTypes = $('.' + c.KeyCode).attr('data-type');
                    }
                    if (DataTypes != undefined) {
                        var types = DataTypes.split(",");

                        if (types.length > 0) {
                            for (var i = 0; i < types.length; i++) {
                                // this function in _ntlFieldTest.cshtml
                                Configurations(c.KeyCode, types[i], c.KeyValue, c.isActive);
                                if (c.fontColor != null || c.fontColor != '') {
                                    $('.' + c.KeyCode).css({
                                        'color': c.fontColor
                                    });
                                }
                            }
                        }
                    }

                });
            }

           
            $('.ForMail').css({
                'display': 'block'
            });
            $('#FIELD_TEST').empty();
            $.ajax({
                url: '/site/Mail',
                type: 'post',
                data: { 'Subject': 'Site Test Summary: ' + Subject, 'Body': $('#tempData').html(), 'value': SiteId },
                success: function (res2) {
                    $('#tempData').empty();
                    if (res2.Status == 'success') {
                        $.notify(res2.Message, { type: res2.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                    } else {
                        $.notify(res2.Message, { type: res2.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                    }
                    $('#tempData').empty();

                }

            });
        }


    });

    return false;
});

/******************Schedule Site********************************/
$(document).on('click', '.GridShedule', function () {
    modal_title.text('Schedule Site');
    modal.modal('show');
    //modal_content.css({ 'width': 'fit-content' });
    var ur = '/Site/GridSchedule?SiteId=' + $(this).attr('data-SiteId') + '&UserId=' + CurrentUserId + '&Scope=' + $(this).attr('data-scope');
    $.ajax({
        url: ur,
        // async: false,
        success: function (data) {
            modal_body.html(data);
            $(".select").select2();
        }
    });
    return false;
});

var divclientsites = $('#divclientsites');

$('#searchbox').keyup(function (e) {
   
    if (e.keyCode == 13) {

        $.ajax({
            url: '/Dashboard/SiteGridSearch',
            type: 'post',
            data: { 'value': $(this).val() },
            success: function (res) {
                if (res == '' || res == null) {
                    location.reload();
                }
                divclientsites.empty();
                divclientsites.html(res);
                $('.table-responsive').css({
                    'min-height': '280px'
                });
                fnWoStatus();
                SiteGridPagination($('#SitesCount').val());
            }
        });

        return false;

        //console.log($(this).val());
    }

    if ($(this).val() == '' || $(this).val() == null) {
        $.ajax({
            url: '/Dashboard/SiteGridNextPage',
            type: 'post',
            data: { 'Page': 1 },
            success: function (res) {
                divclientsites.empty();
                divclientsites.html(res);
                fnWoStatus();
                SiteGridPagination();
                //   console.log(res);
            }
        });
    }
});
$(document).on('submit', '#frm-SiteSchedule', function () {
    var sdate = $("input[id='schduledate']").val();
    $("input[id='schduledate']").removeClass("error");
    if (sdate == "") {
        $("input[id='schduledate']").addClass("error");
        $.notify('Select Date.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
        return false;
    }
    var ur = '/Site/SiteSchedule';
    console.log('$(this).serialize():' + $(this).serialize());
    
    $.ajax({
        url: ur,
        type: 'POST',
        data: $(this).serialize(),
        // async: false,
        success: function (res) {
            if (res != 'session expired') {
                if (res.Status == 'success') {
                    fnHideModal();
                    BtnFilterSitesGrid_Click('', '', '');

                } else {
                    $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }
            } else {
                $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
            }
        },
        error: function (err) {
            $.notify(err.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
        }
    });

    return false;
});




/******************Schedue Site End********************************/
/*----End----*/

/******************Dashboard/FloorPlan********************************/
$(document).on('click', '.FloorPlan', function () {
    modal_title.html('<span style="font-size:25px;">Add Floor Plan</span>');
    modal.modal('show');
    modal_content.css({ 'width': '755px' });
    var ur = '/Dashboard/FloorPlan?SiteId=' + $(this).attr('data-SiteId') + '&UserId=' + CurrentUserId + '&Client=' + $(this).attr('wo-ref') + '&SiteCode=' + $(this).attr('Site-Code');
    $.ajax({
        url: ur,
        // async: false,
        success: function (data) {
            modal_body.html(data);
            $(".select").select2();
        }
    });
    return false;
});

$(document).on('submit', '.frm-FloorPlan', function () {
    return false
    var sdate = $("input[id='schduledate']").val();
    $("input[id='schduledate']").removeClass("error");
    var ur = '/Site/SiteSchedule';
    $.ajax({
        url: ur,
        type: 'POST',
        data: $(this).serialize(),
        // async: false,
        success: function (res) {
            if (res != 'session expired') {
                if (res.Status == 'success') {
                    fnHideModal();
                    BtnFilterSitesGrid_Click('', '', '');

                } else {
                    $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }
            } else {
                $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
            }
        },
        error: function (err) {
            $.notify(err.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
        }
    });

    return false;
});


/******************Dashboard/FloorPlan End********************************/
/*----End----*/
SiteGridPagination();

function SiteGridPagination(TotalRecord) {

    var count = $('#TotalSites').text();
    if (TotalRecord != undefined && TotalRecord > 0) {
        count = TotalRecord;
    }


    //var count = $('#TotalSites').text();
    var pag = parseFloat(count) / parseFloat(5);

    //ceil
    // var TotalPages = Math.round(pag);
    var TotalPages = Math.ceil(pag);

    $('#SiteGridPagination').pagination({
        pages: TotalPages,
        cssStyle: 'compact-theme',
        currentPage: 1,
        hrefTextPrefix:'#',
        displayedPages: 7,
        onPageClick: function (page) {
            debugger
            var search = $('#searchbox').val();
            var isSearch = $("#IsSearch").val();
            if (search.length > 1) {
                $.ajax({
                    url: '/Dashboard/SiteGridSearch',
                    type: 'post',
                    data: {'value':search, 'Page': page },
                    success: function (res) {
                        divclientsites.empty();
                        divclientsites.html(res);
                        fnWoStatus();
                        var showing = parseInt(page) * 5;
                        $('#GridShowing').text(showing);

                    }
                });
            } else if (isSearch.length > 1) {
                $.ajax({
                    url: '/Dashboard/SiteGridSearch',
                    type: 'post',
                    data: { 'value': search, 'Page': page },
                    success: function (res) {
                        divclientsites.empty();
                        divclientsites.html(res);
                        fnWoStatus();
                        var showing = parseInt(page) * 5;
                        $('#GridShowing').text(showing);

                    }
                });
            } else {
                $.ajax({
                    url: '/Dashboard/SiteGridNextPage',
                    type: 'post',
                    data: { 'Page': page },
                    success: function (res) {
                        divclientsites.empty();
                        divclientsites.html(res);
                        fnWoStatus();

                        var showing = parseInt(page) * 5;
                        $('#GridShowing').text(showing);

                    }
                });
            }
            
        },

    });
    $('#GridShowing').text($('#example1 tr').length - 1);

    if (count > 5) {
        $('#GridShowing').text("5");
        $('#GridShowingTotal').text(count);
    }
    else {
        $('#GridShowing').text(count);
        $('#GridShowingTotal').text(count);
    }
}
function fnWoStatus() {
   
    $.each(WoStatus, function (i, v) {
        if (v.KeyCode == 'PENDING_SCHEDULED') {
            $('.PENDING_SCHEDULED_COLOR').css({
                'background-color': v.ColorCode
            });
        }
        else if (v.KeyCode == 'COMPLETED') {
            $('.COMPLETED_COLOR').css({
                'background-color': v.ColorCode,
                'font-weight': 'bold'
            });
            $('.COMPLETED_TITLE').text(v.DefinationName);
        }
        else if (v.KeyCode == 'SCHEDULED') {
            $('.SCHEDULED_COLOR').css({
                'background-color': v.ColorCode,
                'font-weight': 'bold'
            });
            $('.SCHEDULED_TITLE').text(v.DefinationName);
        }
        else if (v.KeyCode == 'DRIVE_COMPLETED') {
            $('.DRIVE_COMPLETED_COLOR').css({
                'background-color': v.ColorCode,
                'font-weight': 'bold'
            });
            $('.DRIVE_COMPLETED_TITLE').text(v.DefinationName);
        }
        else if (v.KeyCode == 'PENDING_WITH_ISSUE') {
            $('.PENDING_WITH_ISSUE_COLOR').css({
                'background-color': v.ColorCode,
                'font-weight': 'bold'
            });
            $('.PENDING_WITH_ISSUE_TITLE').text(v.DefinationName);
        } else if (v.KeyCode == 'TOTAL_SITES') {
            $('.TOTAL_SITES_COLOR').css({
                'background-color': v.ColorCode,
                'font-weight': 'bold'
            });
            $('.TOTAL_SITES_TITLE').text(v.DefinationName);
        }
        else if (v.KeyCode == 'IN_PROGRESS') {
            $('.IN_PROGRESS_COLOR').css({
                'background-color': v.ColorCode,
                'font-weight': 'bold'
            });
            $('.IN_PROGRESS_TITLE').text(v.DefinationName);
        }
        else if (v.KeyCode == 'REPORT_SUBMITTED') {
            $('.REPORT_SUBMITTED_COLOR').css({
                'background-color': v.ColorCode,
                'font-weight': 'bold'
            });
            $('.REPORT_SUBMITTED_TITLE').text(v.DefinationName);
        }




    });

}




//format



window.onload = function () {
    setTimeout(function () {
        scrollTo(0, 0);
    }, 10); //100ms for example
}

$(document).on('change', '#file', function () {
    BtnUploadFile_Click();
});

function BtnUploadFile_Click() {
    $("#overlay_fileupload").css("display", "");
    $("#frmuploadfile").submit();
}



$(document).on('change', '#SiteLogFile', function () {
    SiteLogFileSubmit();
});

function SiteLogFileSubmit() {
    $("#SiteLogFileLoader").css("display", "");
    $("#frm-UploadSiteLogs").submit();
}
var displayRabgeDiv = false;
var filType = 'Total';
var fil = 'Today';
var txtFromDate;
var txtToDate;

JSON.stringify = JSON.stringify || function (obj) {
    var t = typeof (obj);
    if (t != "object" || obj === null) {
        // simple data type
        if (t == "string") obj = '"' + obj + '"';
        return String(obj);
    }
    else {
        // recurse array or object
        var n, v, json = [], arr = (obj && obj.constructor == Array);
        for (n in obj) {
            v = obj[n]; t = typeof (v);
            if (t == "string") v = '"' + v + '"';
            else if (t == "object" && v !== null) v = JSON.stringify(v);
            json.push((arr ? "" : '"' + n + '":') + String(v));
        }
        return (arr ? "[" : "{") + String(json) + (arr ? "]" : "}");
    }
};



function BtnFilterSitesGrid_Click(filterType, filter, value, btnId) {
    var vm = "";
    var ft = filterType;
    // displayRabgeDiv = false;
    var range = "";

    if (filter == 'Total' && filterType == 'Date') {
        $("#divSiteRangeFilter").css("display", "");
        displayRabgeDiv = true;
        return;
    }

    $("input[id='fromdate']").removeClass("error");
    $("input[id='todate']").removeClass("error");

    if (filterType == 'DateRange') {
        range = filterType;
        var fromdate = $("input[id='fromdate']").val();
        var todate = $("input[id='todate']").val();

        if (fromdate == "") {
            $("input[id='fromdate']").addClass("error");
            return;
        }
        else if (todate == "") {
            $("input[id='todate']").addClass("error");
            return;
        }
        else if (new Date(fromdate) > new Date(todate)) {
            $("input[id='fromdate']").addClass("error");
            $("input[id='todate']").addClass("error");
            $("#divDateRangeError").css("display", "");
            return;
        }
        else {
            filterType = 'Range';
            filter = fromdate;
            value = todate;
            txtFromDate = fromdate;
            txtToDate = todate;
        }

    } else if (filterType == 'RegionFilterValue') {

        IsMapZoom = 0;
        IsLatLng = 0;
        SelectedRegionOrMarket = filter;
    }

    var TotalRecord = 0;

    $.ajax({
        url: '/Dashboard/SiteFilters?FilterType=' + filterType + '&Filter=' + filter + '&value=' + value,//urlGetFilteredSites,
        type: 'POST',
        async: false,
        dataType: 'json',
        success: function (data) {
           
            if (data != "") {
                vm = data;
                
            } else {
                window.location.href = "/";
                
                return false;
            }
        },
        error: function (err) {
            if (err.status==200) {
                window.location.href = "/";
                
                return false;
            }
        }
    });
    $.ajax({
        url: '/Dashboard/SiteGrid',
        type: 'POST',
        success: function (data) {
            if (data != "") {
                $('#divclientsites').empty();
                $('#divclientsites').html(data);
                WoDone++;

            }
        }
    })

    loading.open();

    $.ajax({
        url: '/Dashboard/SitesStatuses',
        type: 'POST',
        success: function (res) {
            if (res != "") {
                var SitesStatuses = $('#divstatus');
                SitesStatuses.empty();
                SitesStatuses.html(res);

                if (filterType == 'Status') {
                    TotalRecord = $('#' + btnId).text();
                }
                SiteGridPagination(TotalRecord);
                WoDone++;
            }
        }
    })

    loading.open();

    if (filterType == 'RegionFilter') {
        $.ajax({
            url: '/Dashboard/Regions',
            type: 'POST',
            data: JSON.stringify(vm),
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (res != "") {
                    var divregionalsites = $('#divregionalsites');
                    divregionalsites.empty();
                    divregionalsites.html(res);
                    $('#RegionalSiteTitle').text(RegionSiteTitle);
                    fnWoStatus();
                }
            }
        })
    }
    loading.open();

    fnWoInterval();

    var divsitelocations = $('#divsitelocations');
    $.ajax({
        url: '/Dashboard/SiteLocations',
        type: 'POST',
        success: function (loc) {
            if (loc != "") {
                divsitelocations.empty();
                divsitelocations.html(loc);
               
            }
        }
    })


    if (filterType == 'Panel2Value') {
        SelectedViewTester = filter;
    }

    $.ajax({
        url: '/Dashboard/DriveTester',
        type: 'POST',
        data: JSON.stringify(vm),
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (res != "") {
                var divTesterSites = $('#divdrivetesterview');
                divTesterSites.empty();
                divTesterSites.html(res);
                fnWoStatus();
                $('#TesterTitle').text(TesterSiteTitle);
            }
        },
        error: function (err) { }
    });


    if (ft == 'Date' || ft == 'DateRange') {
        ft = '';

        if (btnId != undefined && btnId != '' && btnId != null) {
            $.ajax({
                url: '/Dashboard/CurrentDateFilter',
                type: 'post',
                data: { "filter": filter },
                datatype: 'json',
                success: function (res) {
                    var btn_ThisWeek = $('#btn-ThisWeek')

                    if (btnId == 'btn-today') {
                        var date = new Date(parseInt(res.Date.substr(6)));
                        if (res.IsToday == true) {
                            $('#' + btnId).text('Today');
                        }
                        else {
                            $('#' + btnId).text(date.toInputFormat());
                            $('#date-filter-txt').text(date.toInputFormat())
                        }

                        btn_ThisWeek.text('This Week');
                        $('#btn-ThisMonth').text('This Month');

                    } else if (btnId == 'btn-ThisWeek') {
                        if (res.IsThisWeek == true) {
                            $('#' + btnId).text('This Week');
                        }
                        else {
                            $('#' + btnId).text('Week ' + res.WeekOfYear);

                            var WeekStart = new Date(parseInt(res.WeekStart.substr(6)));
                            var WeekEnd = new Date(parseInt(res.WeekEnd.substr(6)));

                            $('#date-filter-txt').text(WeekStart.toInputFormat() + '-' + WeekEnd.toInputFormat())
                        }

                        $('#btn-today').text('Today');
                        $('#btn-ThisMonth').text('This Month');
                    } else if (btnId == 'btn-ThisMonth') {

                        if (res.IsThisMonth == true) {
                            $('#' + btnId).text('This Month');
                        }
                        else {
                            var MonthStart = new Date(parseInt(res.MonthStart.substr(6)));
                            var MonthEnd = new Date(parseInt(res.MonthEnd.substr(6)));

                            $('#' + btnId).text(MonthStart.toMMyyy());
                            $('#date-filter-txt').text(MonthStart.toInputFormat() + '-' + MonthEnd.toInputFormat())
                        }
                        $('#btn-today').text('Today');
                        btn_ThisWeek.text('This Week');
                    }
                }
            });
        }




    }

    if (displayRabgeDiv == true) {
        $("#divSiteRangeFilter").css("display", "");
        $("input[id='todate']").val(txtToDate);
        $("input[id='fromdate']").val(txtFromDate);
        // displayRabgeDiv = true;
        //return;
    }
    filType = filterType;
    fil = filter;




}


var t;
var running = false;
var WoDone = 0;
function fnWoInterval() {


    clearInterval(t);
    running = !running;
    if (!running)
        return;
    t = setInterval(function () {
        loading.open();
        if (WoDone==2) {
            fnWoStatus();
            WoDone = 0;

            fnWoInterval();
            loading.close();
           // console.log('done');
        }
        //console.log('start');
    }, 1000);
}

function testerDragStart(id) {
    var arr = id.split('|');
    testerId = parseInt(arr[0]);
    var img = document.createElement("img");
}
function testerDragEnd() {
    setTimeout(function () {
        testerId = 0;
    }, 50);
}
function SiteMapEvent(args) {
   
    if (RoleNameToLower == 'testlead' || RoleNameToLower == 'admin' || RoleNameToLower == 'coordinator' || RoleNameToLower == 'swi poc') {

        tempTesterId = testerId;
        if (args.eventName === 'mouseover') {
            if (tempTesterId > 0) {
                siteId = args.id;

                modal_content.css({
                    'width': '560px',
                    'height': 'auto'
                });
                modal_title.text('Schedule Site');
              
                modal.modal('show');
                var ur = '@Url.Action("SiteSchedule", "Site")' + "?SiteId=" + siteId + '&TesterId=' + tempTesterId + '&UserId=11' + '&Scope=';
                //var ur = '@Url.Action("SiteSchedule", "Site")' + "?id=" + siteId + '&TesterId=' + tempTesterId;
                $.ajax({
                    url: '/Site/SiteSchedule?SiteId=' + siteId + '&TesterId=' + tempTesterId + '&UserId=11',// ur,
                    //url: '/Site/SiteSchedule?id=' + siteId + '&TesterId=' + tempTesterId,// ur,
                    success: function (data) {
                        modal_body.empty();
                        modal_body.html(data);
                      
                        $("select").select2();

                        $('.select2-selection__choice').css({
                            'color': '#131212'
                        });
                    }
                });

            }
        }
    }
}



function fnOpenModal(title, bodyHtml, css) {
    modal_title.text(title);
    modal_content.css(css);
    modal_body.empty();
    modal_body.html(bodyHtml);
    modal.modal('show');
}

   

//function fnExcelReport() {
//    $('.NoExport').hide();
//    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
//    var textRange; var j = 0;
//    tab = document.getElementById('example1'); // id of table

//    for (j = 0 ; j < tab.rows.length ; j++) {
//        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
//        //tab_text=tab_text+"</tr>";
//    }

//    tab_text = tab_text + "</table>";
//    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
//    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
//    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

//    var ua = window.navigator.userAgent;
//    var msie = ua.indexOf("MSIE ");

//    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
//    {
//        txtArea1.document.open("txt/html", "replace");
//        txtArea1.document.write(tab_text);
//        txtArea1.document.close();
//        txtArea1.focus();
//        sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
//    }
//    else                 //other browser not tested on IE 11
//        sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));
//    $('.NoExport').show();
//    return (sa);
//}
function BtnAssignTester_Click() {
    var sdate = $("input[id='schduledate']").val();
    $("input[id='schduledate']").removeClass("error");
    if (sdate == "") {
        $("input[id='schduledate']").addClass("error");
        return;
    }
    $('#schedule-model').loader('show', '<img src="/AdminLTE/dist/img/spinner.gif">');
    //var ur = '@Url.Action("AssignTester", "Site")' + "?SiteId=" + siteId + "&TesterId=" + tempTesterId + "&Date=" + $("#schduledate").val();
    $.ajax({
        url: '/Site/AssignTester?SiteId=' + siteId + '&TesterId=' + tempTesterId + '&Date=' + $("#schduledate").val(),// ur,
        type: 'POST',
        //  async: false,
        dataType: 'text',
        processData: false,
        success: function (data) {
            if (data == "true") {
                //$('#schduledate').val('');
                //$('#schedule-model').loader('hide');
                //$("#assigntestermodel").modal('toggle');
                $('#av-modal').modal('hide');
                BtnFilterSitesGrid_Click('', '', '');
            } else {
               // console.log(data);
            }
        },
        error: function (err) {
           // console.log(err.statusText);
            $('#schedule-model').loader('hide');
        }
    });
}
