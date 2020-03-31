var map;
var AzRecord = null;
var infoWindow;
var azmitOfAllLayer = null;
var markerForNeibour;
var removePolygons = [];
function ShowOoklaImage(Path) {
    console.log(Path);

    modal_title.text('Ookla Image');
    modal_content.css({ 'width': '400px' });
    modal.modal('show');
    modal_body.empty();
    modal_body.html(' <img src="' + Path + '" height="400" width="250" />');
    modal_footer.remove();
}


function MarketSites(Latitude, Longitude) {
    if (AzRecord == null) {
        var SiteCode = $(".SiteCode:first").text().split('-');
        console.log(SiteId + ' -' + SiteCode[0] + ' -' + Latitude + '-' + Latitude + ' -')
        $.ajax({
            url: '/Site/MarketSitesAngles?filter=bySiteCode&SiteId=' + SiteId + '&SiteCode=' + SiteCode[0] + '&Latitude=' + Latitude + '&Longitude=' + Latitude,
            type: "post",
            data: $(this).serialize(),
            success: function (res) {
                // console.log(res);
                AzRecord = res;
                azmitOfAllLayer = res;
                //  Azmiuth(res);
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

function Azmiuth(Angles, Radius) {
    
    //  debugger;
    // 0 = start angle
    // 1 = end angle
    // 2 = color
    //flightPath.setMap(null);
    if (Radius == undefined) {
        Radius = 100;
    }
    polys = [],
    i = 0,
    radiusMeters = Radius;
    var latlng;
    // var bounds = new google.maps.LatLngBounds();
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
            fillOpacity: 0.7,
            networkmodeid: Angles[i].NetworkmodeId,
            BandId: Angles[i].BandId,
            CarrierId: Angles[i].CarrierId,
            SectorId: Angles[i].SectorId,
            ScopeId: Angles[i].ScopeId,
            BandId: Angles[i].BandId,
            SiteId: Angles[i].SiteId,
            Sector: Angles[i].Sector,
        });
        markerForNeibour = new google.maps.Marker({
            // The below line is equivalent to writing:
            // position: new google.maps.LatLng(-34.397, 150.644)
            position: { lat: Angles[i].nLatitude, lng: Angles[i].nLongitude },
            map: map,
            icon: "http://localhost:18460/Content/Images/Common/dot_x24.png"

        });
        infoWindow = new google.maps.InfoWindow;
        removePolygons.push(poly);
        polys.push(poly);
        poly.addListener('click', showArrays);
        function showArrays(event) {

            // Since this polygon has only one path, we can call getPath() to return the
            // MVCArray of LatLngs.
            var SiteId = this.SiteId;
            var NetworkModeId = this.networkmodeid;
            var CarrierId = this.CarrierId;
            var ScopeId = this.SectorId;
            var BandId = this.BandId;
            var Sector = this.Sector;
            var ChartTimeFormat = 'HH:mm:ss:ff';
          
            getGraph(SiteId, NetworkModeId, CarrierId, ScopeId, BandId, Sector, ChartTimeFormat);
            var vertices = this.getPath();

            var contentString = '<b>' + this.Sector + '</b><br>';
            //+
            //    'Location: <br>' + event.latLng.lat() + ',' + event.latLng.lng() +
            //    '<br>';



            // Replace the info window's content and position.
            infoWindow.setContent(contentString);
            infoWindow.setPosition(event.latLng);

            //  infoWindow.open(map, this);
        }

        //latlng = new google.maps.LatLng(path[path.length-9].lat(), path[path.length-5].lng());
        //customTxt = "<div>" + Angles[i].PCI + "</div>";
        //txt = new TxtOverlay(latlng, customTxt, "customBox", map);

        //for (var j = 0; j < path.length; j++) {
        //    console.log(path[j].lng() + ',' + path[j].lat() + ',100');
        //}

        //  bounds.extend(latlng);
    }

    // map.fitBounds(bounds);
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



$(function () {
    //http://maps.googleapis.com/maps/api/geocode/json?latlng=31.504303,74.331574&sensor=true
    var btn_map_menu = $('#btn-map-menu');
    initialize('SiteMap', FirstLatitude, FirstLongitude);
    btn_map_menu.click(function () {

    });

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
    fnWoStatus();

    //$.ajax({
    //    url: 'http://maps.googleapis.com/maps/api/geocode/json?latlng=33.348889,-84.112639&sensor=true',
    //    type: 'get',
    //    datatype: 'json',
    //    success: function (res) {
    //        console.log(res);

    //        console.log(res.results[0].formatted_address);

    //        $('#mapAnimation').html('<iframe id="iframeID" src="http://data.mapchannels.com/routemaps2/routemap200.htm?saddr=3813 Gazebo Pond Ln, Tampa, FL 33613, USA&daddr=' + res.results[0].formatted_address + '&maptype=2&units=2&z=15&fi=50&fs=1&refresh=3&timeout=300&draggable=0&sw=240&svc=0&svz=2&atw=160&fgc=000000&bgc=CCCCCC&rc=FF0000&rw=3&ro=0.7" width="100%" height="600" style="padding:0px;border:solid 1px black" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" webkitallowfullscreen mozallowfullscreen allowfullscreen allowtransparency="true"></iframe>');



    //        $('#directionsPanel').hide();
    //    },
    //    error: function (err) {
    //        console.log(err);
    //    }
    //});
    //$('#iframeID').contents().find('#directionsPanel').hide();
    //$(document).on('click', '#playButton', function () {
    //    alert('sdf');
    //});
});


// these are global variable for marker BUBLE
var markerLat = "";
var markerLng = "";

function initialize(MapId, CenterLat, CenterLong, markerLat, markerLng) {

    gm = google.maps,
    centerPt = new gm.LatLng(CenterLat, CenterLong);
    map = new gm.Map(document.getElementById(MapId), {
        mapTypeId: gm.MapTypeId.ROADMAP,
        zoom: 16,
        center: centerPt,
    });

    MarketSites(CenterLat, CenterLat)
    //driveTestView.createMapIcon(map, "chartArea", '<a><i class="fa fa-area-chart" aria-hidden="true"></i></a>');
    //driveTestView.createGraphSection(map, "allGraphArea", 'grahp area ');


    infowindow = new google.maps.InfoWindow({
        content: ConstantVariables
    });
    //var centerControlDiv = document.createElement('div');
    //var centerControl = new CenterControl(centerControlDiv, );
    //console.log(centerControl);
    //centerControlDiv.index = 1;

    //centerControlDiv.addEventListener('click', function () {
    //    $('.allChartArea').toggle();
    //});
    //var centerControlDiv1 = document.createElement('div');
    //var centerControl1 = new CenterControl(centerControlDiv, map, "allChartArea", 'ssss');
    //console.log(centerControl);
    //centerControlDiv1.index = 1;

    //map.controls[google.maps.ControlPosition.RIGHT].push(centerControlDiv1);
    //$('#PingCharts').append('<div class="box box-widget collapsed-box"><div class="box-header with-border"><div class="user-block"><img class="img-circle" src="../dist/img/user1-128x128.jpg" alt="User Image"><span class="username"><a href="#">Jonathan Burke Jr.</a></span><span class="description">Shared publicly - 7:30 PM Today</span></div><!-- /.user-block --><div class="box-tools"> <button type="button" class="btn btn-box-tool" data-toggle="tooltip" title="" data-original-title="Mark as read"> <i class="fa fa-circle-o"></i></button><button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-plus"></i></button> <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button></div><!-- /.box-tools --></div><!-- /.box-header --><div class="box-body" style="display: none;"><img class="img-responsive pad" src="../dist/img/photo2.png" alt="Photo"><p>I took this photo this morning. What do you guys think?</p><button type="button" class="btn btn-default btn-xs"><i class="fa fa-share"></i> Share</button><button type="button" class="btn btn-default btn-xs"><i class="fa fa-thumbs-o-up"></i> Like</button><span class="pull-right text-muted">127 likes - 3 comments</span> </div><!-- /.box-body --><div id="PingChart"></div></div</div></div><!--for test landscape-primary-->')
    // $('.allChartArea').toggle();
    return false;

}

$('.MapPlot').click(function () {
    //document.location.origin
    // var Url = domain + '/Content/AirViewLogs/TMO/' + NetLayer + '/' + $(this).attr('data-file');
    var Url = document.location.origin + '/Content/AirViewLogs/TMO/' + NetLayer + '/' + $(this).attr('data-file');
    Url = Url.replace(' ', '%20');
    try {

        initializekml('SiteMap', FirstLatitude, FirstLongitude, NetLayer, Url, AzRecord, null, null);
    } catch (e) {
    }
    return false;
});


$(document).on('click', '.BandRow', function () {

    $(".BandRow").removeClass("SelectedRow");
    $(this).addClass("SelectedRow");
    NetLayer = $(this).attr('data-NetLayer');
    var SiteId = $(this).attr('data-SiteId');
    var NetworkModeId = $(this).attr('data-NetworkModeId');
    var CarrierId = $(this).attr('data-CarrierId');
    var ScopeId = $(this).attr('data-ScopeId');
    var BandId = $(this).attr('data-BandId');
    $.ajax({
        url: "/Dashboard/getStationaryTab?BandId=" + BandId + "&SiteId=" + SiteId + "&NetworkModId=" + NetworkModeId + "&ScopeId=" + ScopeId + "&CarrierId=" + CarrierId + "&RequestFrom=" + window.location.pathname.split("/")[1],
        type: 'POST',
        async: false,
        success: function (data) {
            $("#site-tab2").empty();
            $("#site-tab2").html(data);
        }
    });
    getPolygons(NetLayer, SiteId, NetworkModeId, CarrierId, ScopeId, BandId);

});

function getPolygons(NetLayer, SiteId, NetworkModeId, CarrierId, ScopeId, BandId)
{
   


    driveTestView.removePolygons();
    driveTestView.drawnPolygons(NetworkModeId, BandId, CarrierId);

    $.ajax({
        url: '/SiteDashboard/SingleSiteData',
        data: { Filter: 'Dashboard_Site_Id', SiteId: SiteId, NetworkModeId: NetworkModeId, BandId: BandId, CarrierId: CarrierId, ScopeId: ScopeId, Sector: '' },
        async: false,
        success: function (res) { console.log("document:" + res); },
        error: function (err) { }
    });

    $.ajax({
        url: '/SiteDashboard/HandOverStatus',
        success: function (resHo) {
            $('#pan-HandoverStatus').empty();
            $('#pan-HandoverStatus').html(resHo);
        },
        error: function (err) { }
    });

    $.ajax({
        url: '/SiteDashboard/MOMTStatus',
        success: function (res) {
            $('#pan-MOMTStatus').empty();
            $('#pan-MOMTStatus').html(res);
        },
        error: function (err) { }
    });

    $.ajax({
        url: '/SiteDashboard/TeamMembers',
        success: function (res) {
            $('#pan-TeamMembers').empty();
            $('#pan-TeamMembers').html(res);
        },
        error: function (err) { }
    });

    $.ajax({
        url: '/SiteDashboard/PingThroughtput',
        data: { 'Filter': 'NetLayer' },
        success: function (res) {
            $('#PING').empty();
            $('#PING').html(res);


        },
        error: function (err) { }
    });



    $.ajax({
        url: '/SiteDashboard/DLThroughtput',
        data: { 'Filter': 'NetLayer' },
        success: function (res) {
//            debugger
            $('#DOWNLINK').empty();
            $('#DOWNLINK').html(res);
        },
        error: function (err) { }
    });


    $.ajax({
        url: '/SiteDashboard/ULThroughtput',
        data: { Filter: 'NetLayer' },
        success: function (res) {
            $('#UPLINK').empty();
            $('#UPLINK').html(res);
        },
        error: function (err) { }
    });

    $.ajax({
        url: '/SiteDashboard/OoklaResult',
        success: function (res) {
            $('#pan-OoklaResults').empty();
            $('#pan-OoklaResults').html(res);
        },
        error: function (err) { }
    });

}

$(document).on('click', '.SectorRow', function () {
    //  debugger;
    $(".SectorRow").removeClass("SelectedRow");
    $(this).addClass("SelectedRow");

    var SiteId = $(this).attr('data-SiteId');
    var NetworkModeId = $(this).attr('data-NetworkModeId');
    var CarrierId = $(this).attr('data-CarrierId');
    var ScopeId = $(this).attr('data-ScopeId');
    var BandId = $(this).attr('data-BandId');
    var Sector = $(this).attr('data-Sector');
    var ChartTimeFormat = 'HH:mm:ss:ff';

    getGraph(SiteId, NetworkModeId, CarrierId, ScopeId, BandId, Sector, ChartTimeFormat);

});
$(document).on('click', '.getGraphStationarySide', function () {
    //  debugger;
    $(".getGraphStationarySide").removeClass("active");
    $(this).addClass("active");

    var SiteId = $(this).attr('data-SiteId');
    var NetworkModeId = $(this).attr('data-NetworkModeId');
    var CarrierId = $(this).attr('data-CarrierId');
    var ScopeId = $(this).attr('data-ScopeId');
    var BandId = $(this).attr('data-BandId');
    var Sector = $(this).attr('data-Sector');
    var ChartTimeFormat = 'HH:mm:ss:ff';

    getGraph(SiteId, NetworkModeId, CarrierId, ScopeId, BandId, Sector, ChartTimeFormat);

});

$(window).on("load", function () {
    $("body").on("click", ".nav li a", function () {
        //setTimeout(function () {
        //    if ($('#CSFB_MT').length > 0) {
        //        chartMT.render();
        //    }
        //    if ($('#CSFB_MO').length > 0) {
        //        chartMO.render();
        //    }
            
        //    if ($('#SMS_MO').length > 0) {
        //        smsMO.render();
        //    }
        //    chart.render();
        //}, 300);

    });
});


$(document).on("click", ".nav li a", function () {
    setTimeout(function () {
        if ($('#CSFB_MT').length > 0) {
            if ($('#CSFB_MT').children('div.canvasjs-chart-container').length > 0) {
                chartMT.render();
            }  
        }
        if ($('#CSFB_MO').length > 0) {
            if ($('#CSFB_MO').children('div.canvasjs-chart-container').length > 0) {
                chartMO.render();
            }
        }

        if ($('#SMS_MO').length > 0) {
            if ($('#SMS_MO').children('div.canvasjs-chart-container').length > 0) {
                smsMO.render();
            }
        }   

        if ($('#PingChart').length > 0) {
            if ($('#PingChart').children('div.canvasjs-chart-container').length > 0) {
                if (typeof Pchart != "undefined") {
                    Pchart.render();
                }
            }
        }

        if ($('#ULChart').length > 0) {
            if ($('#ULChart').children('div.canvasjs-chart-container').length > 0) {
                if (typeof ULchart != "undefined") {
                    ULchart.render();
                } 
            }
        }

        if ($('#DLChart').length > 0) {
            if ($('#DLChart').children('div.canvasjs-chart-container').length > 0) {
                if (typeof DLchart != "undefined") {
                    DLchart.render();
                }
            }
        }
        
        
    }, 300);

});

function drawGraphOfMO(mtmoList, ChartTimeFormat)
{
    
    console.log(mtmoList);
    let mo = mtmoList.find(x=>x.TestTypeGroup == 'MO');
    let finalGraphMo = [];
    console.log(mo);
    if (typeof mo != "undefined") {
        let getlistOfMO = mo.SiteDashboardThroughtputChartmomtList;
        let graphDataOfMO=[]
      
        for (var i = 0; i < getlistOfMO.length; i++) {
          


            var TimeStamp = moment.utc(getlistOfMO[i].TimeStamp).toDate();
                TimeStamp = moment(TimeStamp).format('HH:mm:ss');

              
                obj1 = { label: TimeStamp,indexLabel: "mo", y: Number(getlistOfMO[i].NetworkModeId) };

                graphDataOfMO.push(obj1);
            }

        finalGraphMo.push({
                type: "line",
                legendText: mo.TestTypeGroup,
                showInLegend: true,
                axisYIndex: 2,
                dataPoints: graphDataOfMO
            });
        }

    chartMO = new CanvasJS.Chart("CSFB_MO", {
            animationEnabled: true,
            //colorSet: "CustomColor",
            //height: "300px",
            exportEnabled: false,

            title: {
                text: "",

            },
            axisX: {
                interval: 50,
                valueFormatString: ChartTimeFormat,
                labelAngle: -50
            },
            axisY: {
                //interval: 5


            },
            toolTip: {
            },
            data: finalGraphMo
    });

    if ($('#CSFB_MO').length > 0) {
        chartMO.render();
    }
        

    
}
function drawGraphOfMT(mtmoList, ChartTimeFormat) {

    console.log(mtmoList);
    let mo = mtmoList.find(x=>x.TestTypeGroup == 'MT');
    let finalGraphMo = [];
    console.log(mo);
    if (typeof mo != "undefined") {
        let getlistOfMO = mo.SiteDashboardThroughtputChartmomtList;
        let graphDataOfMO = []

        for (var i = 0; i < getlistOfMO.length; i++) {



            var TimeStamp = moment.utc(getlistOfMO[i].TimeStamp).toDate();
            TimeStamp = moment(TimeStamp).format('HH:mm:ss');


            obj1 = { label: TimeStamp, y: Number(getlistOfMO[i].NetworkModeId) };

            graphDataOfMO.push(obj1);
        }

        finalGraphMo.push({
            type: "line",
            legendText: mo.TestTypeGroup,
            showInLegend: true,
            axisYIndex: 2,
            dataPoints: graphDataOfMO
        });
    }

    chartMT = new CanvasJS.Chart("CSFB_MT", {
        animationEnabled: true,
        //colorSet: "CustomColor",
        //height: "300px",
        exportEnabled: false,

        title: {
            text: "",

        },
        axisX: {
            interval: 50,
            valueFormatString: ChartTimeFormat,
            labelAngle: -50
        },
        axisY: {
            //interval: 5


        },
        toolTip: {
        },
        data: finalGraphMo
    });
    
    if ($('#CSFB_MT').length > 0) {
        chartMT.render();
    }
    


}
function drawGraphOfSMO(mtmoList, ChartTimeFormat) {

    console.log(mtmoList);
    let mo = mtmoList.find(x=>x.TestTypeGroup == 'SMO');
    let finalGraphMo = [];
    console.log(mo);
    if (typeof mo != "undefined") {
        let getlistOfMO = mo.SiteDashboardThroughtputChartmomtList;
        let graphDataOfMO = []

        for (var i = 0; i < getlistOfMO.length; i++) {



            var TimeStamp = moment.utc(getlistOfMO[i].TimeStamp).toDate();
            TimeStamp = moment(TimeStamp).format('HH:mm:ss');


            obj1 = { label: TimeStamp, y: Number(getlistOfMO[i].NetworkModeId) };

            graphDataOfMO.push(obj1);
        }

        finalGraphMo.push({
            type: "line",
            legendText: mo.TestTypeGroup,
            showInLegend: true,
            axisYIndex: 2,
            dataPoints: graphDataOfMO
        });
    }

    smsMO= new CanvasJS.Chart("SMS_MO", {
        animationEnabled: true,
        //colorSet: "CustomColor",
        //height: "300px",
        exportEnabled: false,

        title: {
            text: "",

        },
        axisX: {
            interval: 50,
            valueFormatString: ChartTimeFormat,
            labelAngle: -50
        },
        axisY: {
            //interval: 5


        },
        toolTip: {
        },
        data: finalGraphMo
    });

    if ($('#SMS_MO').length > 0) {
        smsMO.render();
    }
    


}
function drawGraphOfSMT(mtmoList, ChartTimeFormat) {

   
    let mo = mtmoList.find(x=>x.TestTypeGroup == 'SMT');
    let finalGraphMo = [];
    console.log(mo);
    if (typeof mo != "undefined") {
        let getlistOfMO = mo.SiteDashboardThroughtputChartmomtList;
        let graphDataOfMO = []

        for (var i = 0; i < getlistOfMO.length; i++) {



            var TimeStamp = moment.utc(getlistOfMO[i].TimeStamp).toDate();
            TimeStamp = moment(TimeStamp).format('HH:mm:ss');


            obj1 = { label: TimeStamp, y: Number(getlistOfMO[i].NetworkModeId) };

            graphDataOfMO.push(obj1);
        }

        finalGraphMo.push({
            type: "line",
            legendText: mo.TestTypeGroup,
            showInLegend: true,
            axisYIndex: 2,
            dataPoints: graphDataOfMO
        });
    }

    var chartMO = new CanvasJS.Chart("SMS_MT", {
        animationEnabled: true,
        //colorSet: "CustomColor",
        //height: "300px",
        exportEnabled: false,

        title: {
            text: "",

        },
        axisX: {
            interval: 50,
            valueFormatString: ChartTimeFormat,
            labelAngle: -50
        },
        axisY: {
            //interval: 5


        },
        toolTip: {
        },
        data: finalGraphMo
    });
   
    if ($('#CSFB_MO').length > 0) {
        chartMO.render();
    }

}
function getGraph(SiteId, NetworkModeId, CarrierId, ScopeId, BandId, Sector, ChartTimeFormat) {


    $.ajax({
        url: '/SiteDashboard/SingleSiteData',
        data: { Filter: 'Dashboard_Site_Sector', SiteId: SiteId, NetworkModeId: NetworkModeId, BandId: BandId, CarrierId: CarrierId, ScopeId: ScopeId, Sector: Sector },
        async: false,
        success: function (res) {

            var markerData = res.SiteSectorInfo;
            //console.log(markerData);
            var mtMOSMTSMO = res.GraphDataMTMOSMOSMT;
            drawGraphOfMO(mtMOSMTSMO, ChartTimeFormat);
            drawGraphOfMT(mtMOSMTSMO, ChartTimeFormat);
            drawGraphOfSMO(mtMOSMTSMO, ChartTimeFormat);
            drawGraphOfSMT(mtMOSMTSMO, ChartTimeFormat);
            if (markerData.length > 0) {
                for (var itMar = 0; itMar < markerData.length; itMar++) {
                    let item = markerData[itMar];
                    markerLat = item.TestLatitude;
                    markerLng = item.TestLongitude;

                    // --------Start: Marker and Info window-------------
                    var contInfo = '';

                    if (item.NetworkMode == "LTE") {
                        contInfo = '<div id="content">' +
                                 '<div id="bodyContent">' +

                                 '<span><strong>Network Mode: </strong>' + item.NetworkMode + '</span> <br />' +
                                 '<span><strong>Band: </strong>' + item.Band + '</span> <br />' +
                                 '<span><strong>Carrier: </strong>' + item.Carrier + '</span> <br />' +
                                 '<span><strong>RSRP: </strong>' + item.SignalStrength + ' dBm' + '</span> <br />' +
                                 '<span><strong>RSRQ: </strong>' + item.SignalQuality + ' dB' + '</span> <br />' +
                                 '<span><strong>RSNR: </strong>' + round(item.SignalNoise,1) + ' dB</span> <br />' +
                                 '</div>' +
                                '</div>';
                    }
                    else if (item.NetworkMode == "WCDMA") {
                        contInfo = '<div id="content">' +
                                 '<div id="bodyContent">' +

                                 '<span><strong>Network Mode: </strong>' + item.NetworkMode + '</span> <br />' +
                                 '<span><strong>Band: </strong>' + item.Band + '</span> <br />' +
                                 '<span><strong>Carrier: </strong>' + item.Carrier + '</span> <br />' +
                                 '<span><strong>RSCP: </strong>' + item.SignalStrength + ' dBm' + '</span> <br />' +
                                 '<span><strong>Ec/Io: </strong>' + round(item.SignalNoise, 1) + ' dB' + '</span> <br />' +
                                 '</div>' +
                                '</div>';
                    }
                    else if (item.NetworkMode == "GSM") {
                        contInfo = '<div id="content">' +
                                 '<div id="bodyContent">' +

                                 '<span><strong>Network Mode: </strong>' + item.NetworkMode + '</span> <br />' +
                                 '<span><strong>Band: </strong>' + item.Band + '</span> <br />' +
                                 '<span><strong>Carrier: </strong>' + item.Carrier + '</span> <br />' +
                                 '<span><strong>RxL: </strong>' + item.SignalStrength + ' dBm' + '</span> <br />' +
                                 '<span><strong>RxQ: </strong>' + round(item.SignalNoise, 1) + ' dB' + '</span> <br />' +
                                 '</div>' +
                                '</div>';
                    }

                    var myLatLng;
                    console.log(myLatLng);

                    myLatLng = { lat: markerLat, lng: markerLng };
                    console.log(myLatLng);

                    if (markerLat != null) {
                        var marker = new google.maps.Marker({
                            map: map,
                            position: myLatLng
                        });
                    }

                    var infowindow = new google.maps.InfoWindow({
                        content: contInfo
                    });

                    marker.addListener('click', function () {

                        infowindow.open(map, marker);
                    });
                }
            }
            // --------End: Marker and Info window-------------



            // --------Start: Chart 4 -------------------------
            var obj1 = {};
            var seriesPciSignal = [];
            console.log(res.PciSignalStrength);
            var data = GroupData(res.PciSignalStrength);
            
            if (res.PciSignalStrength.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var PciSignals = [];
                    for (var j = 0; j < data[i].data.length; j++) {


                        var TimeStamp = moment.utc(data[i].data[j].TimeStamp).toDate();
                        TimeStamp = moment(TimeStamp).format('HH:mm:ss');

                        //console.log(''+(TimeStamp.getMonth()+1)+' ' + (TimeStamp.getDay()+1)+', '+ TimeStamp.getFullYear() +' '+ TimeStamp.getHours() + ':' + TimeStamp.getMinutes() + ':' + TimeStamp.getSeconds() + ':' + TimeStamp.getMilliseconds());
                        //var tm = new Date('' + (TimeStamp.getMonth() + 1) + ' ' + (TimeStamp.getDay() + 1) + ', ' + TimeStamp.getFullYear() + ' ' + TimeStamp.getHours() + ':' + TimeStamp.getMinutes() + ':' + TimeStamp.getSeconds() + ':' + TimeStamp.getMilliseconds());

                        obj1 = { label: TimeStamp, y: Number(data[i].data[j].SignalStrength) };

                        PciSignals.push(obj1);
                    }

                    seriesPciSignal.push({
                        type: "line",
                        legendText: data[i].PCI,
                        showInLegend: true,
                        axisYIndex: i,
                        dataPoints: PciSignals
                    });
                }

                var chartPciSignals = new CanvasJS.Chart("chartPciStrength", {
                    animationEnabled: true,
                    //colorSet: "CustomColor",
                    //height: "300px",
                    exportEnabled: false,

                    title: {
                        text: "",

                    },
                    axisX: {
                        interval: 50,
                        valueFormatString: ChartTimeFormat,
                        labelAngle: -50
                    },
                    axisY: {
                        //interval: 5


                    },
                    toolTip: {
                    },
                    data: seriesPciSignal
                });
                chartPciSignals.render();

            }




        },
        error: function (a, b, c) {
            console.log('status' + a + b + c);
        }
    });


    //----------------------------
    function GroupData(data) {
        var group_to_values = data.reduce(function (obj, item) {
            //debugger;
            obj[item.PCI] = obj[item.PCI] || [];
            obj[item.PCI].push({ TimeStamp: item.TimeStamp, PCI: item.PCI, SignalStrength: item.SignalStrength });
            return obj;
        }, {});

        var groups = Object.keys(group_to_values).map(function (key) {
            return { PCI: key, data: group_to_values[key] };
        });
        return groups;
    }
    //------------------------------




    //
    $.ajax({
        url: '/SiteDashboard/PingThroughtput',
        data: { 'Filter': 'Sector' },
        success: function (res) {
            if ($('#PING').length > 0) {

                $('#PING').empty();
                $('#PING').html(res);

                Pchart = new CanvasJS.Chart("PingChart",
               {
                   zoomEnabled: true,
                   animationEnabled: true,
                   //toolTip: {
                   //    content: "{x} <br/> {y} "
                   //},
                   title: {
                       text: ""
                   },
                   axisX: {
                       valueFormatString: ChartTimeFormat,
                       labelAngle: -50,
                       interval: 50
                   },
                   axisY: {
                       //  valueFormatString: "#,###"
                       interval: 5
                   },

                   data: [
                   {
                       type: "spline",
                       dataPoints: PingLineChartData
                   }

                   ]
               });

                Pchart.render();
            }
        },
        error: function (err) { }
    });
    //$.ajax({
    //    url: '/SiteDashboard/GraphMTMOSMOSMT',
    //    data: {},
    //    success: function (res) {
    //        console.log(res);

    //    },
    //    error: function (err) { }
    //}).done(function () {


    //});

    
    $.ajax({
        url: '/SiteDashboard/DLThroughtput',
        data: { 'Filter': 'Sector' },
        success: function (res) {
            if ($('#DOWNLINK').length > 0) {
                $('#DOWNLINK').empty();
                $('#DOWNLINK').html(res);

                DLchart = new CanvasJS.Chart("DLChart",
               {
                   zoomEnabled: true,
                   animationEnabled: true,
                   title: {
                       text: ""
                   },
                   axisX: {
                       valueFormatString: ChartTimeFormat,
                       labelAngle: -50
                   },
                   axisY: {
                       //  valueFormatString: "#,###"
                   },

                   data: [
                   {
                       type: "spline",
                       dataPoints: DLLineChartData
                   }

                   ]
               });

                DLchart.render();
            }
        },
        error: function (err) { }
    });


    $.ajax({
        url: '/SiteDashboard/ULThroughtput',
        data: { 'Filter': 'Sector' },
        success: function (res) {
            if ($('#UPLINK').length > 0) {
                $('#UPLINK').empty();
                $('#UPLINK').html(res);

               ULchart = new CanvasJS.Chart("ULChart",
               {
                   zoomEnabled: true,
                   animationEnabled: true,
                   title: {
                       text: ""
                   },
                   axisX: {
                       valueFormatString: ChartTimeFormat,
                       labelAngle: -50
                   },
                   axisY: {
                       //  valueFormatString: "#,###"
                   },

                   data: [
                   {
                       type: "spline",
                       dataPoints: ULLineChartData
                   }

                   ]
               });

                ULchart.render();
            }
        },
        error: function (err) { }
    });
}

//#region driveTestView

var driveTestView = function () {

    // #region Creat General Div function for any icon and text
    var handlecreateDivWithIcon = function (className, innerHtmls) {
        var createGeneralDiv = document.createElement('div');
        createGeneralDiv.style.color = 'rgb(25,250,25)';
        createGeneralDiv.style.fontFamily = 'Roboto,Arial,sans-serif';
        createGeneralDiv.style.fontSize = '26px';
        createGeneralDiv.style.cursor = 'pointer';
        createGeneralDiv.style.color = '#fff';

        createGeneralDiv.innerHTML = innerHtmls;
        createGeneralDiv.className = className;
        createGeneralDiv.id = className;

        //var centerControl = new CenterControls(centerControlDiv2, rightPano);
        createGeneralDiv.index = 1;
        return createGeneralDiv;
    };
    // #endregion

    //#region Create Map Menu
    var handlecreateMapIcon = function (map, className, innerHtmls) {
        var createMapicons = handlecreateDivWithIcon(className, innerHtmls);
        createMapicons.style.marginLeft = '-103px';
        createMapicons.style.marginTop = '50px';
        //controlText.style.lineHeight = '38px';
        createMapicons.style.paddingLeft = '5px';
        createMapicons.style.paddingRight = '5px';
        map.controls[google.maps.ControlPosition.TOP_LEFT].push(createMapicons);
        createMapicons.addEventListener('click', function () {
            document.getElementById("allGraphArea").classList.toggle("hide");

        });
    }
    //#endregion

    //#region create Graph Section On Map
    var handlecreateGraphSection = function (map, className, innerHtmls) {
        var createGraphArea = handlecreateDivWithIcon(className, innerHtmls);
        createGraphArea.style.marginLeft = '-75px';
        createGraphArea.style.marginTop = '0';
        createGraphArea.style.position = 'absolute';
        createGraphArea.style.bottom = '0';
        createGraphArea.style.top = 'auto';
        createGraphArea.style.left = '0';
        createGraphArea.style.height = 'auto';
        createGraphArea.style.width = '100%';
        createGraphArea.innerHTML = '<ul class="selectors"><li class="active"><a href="#tab2" data-toggle="tab"><span class="username">Downlink (Mbps)</span></a></li><li class=""><a href="#tab3" data-toggle="tab"><span class="username">Uplink (Mbps)</span></a></li><li><a href="#tab4" data-toggle="tab"><span class="username">Sector Swap</span></a></li></ul>' +
          '<div class="tab-content"><div class="tab-pane active" id="tab2"> <div class="box-body pan-Downlink" id=> </div>  </div><div class="tab-pane" id="tab3"> <div class="box-body pan-Uplink" id=pan-Uplink></div></div><div class="tab-pane" id="tab4"> <div class="box-body chartPciStrength" id=chartPciStrength style=height:300px;width:100% ></div></div></div>';
        //controlText.style.lineHeight = '38px';
        //createGraphArea.style.paddingLeft = '5px';
        //createGraphArea.style.paddingRight = '5px';
        map.controls[google.maps.ControlPosition.BOTTOM_LEFT].push(createGraphArea);
        createGraphArea.addEventListener('click', function () {

        });

    }
    //#endregion

    //#region all polygons removed which have current draw on map

    var handleremovePolygons = function () {
        
        if (removePolygons.length > 0)
        {
            for (let i = 0; i < removePolygons.length; i++)
            {
                let poly = removePolygons[i];
                poly.setMap(null);
            }
        }
      
    }
    //#endregion

    //#region all polygons drawn which have current layer

    var handledrawnPolygons = function (...data) {
        if (azmitOfAllLayer.length > 0) {
            var filterItemOfAzmit = [];

      console.log(azmitOfAllLayer);

            filterItemOfAzmit = azmitOfAllLayer.filter(x=>x.NetworkmodeId == data[0] && x.BandId == data[1] && x.CarrierId == data[2]);
         
            Azmiuth(filterItemOfAzmit);
        

        }
    }
    //#endregion

    return {

        // #region Creat General Div function for any icon and text
        createDivWithIcon: function () {

            handlecreateDivWithIcon();

        },
        //#endregion

        //#region Create Map Menu
        createMapIcon: function (map, className, innerHtmls) {
            handlecreateMapIcon(map, className, innerHtmls);

        },
        //#endregion

        //#region create Graph Section On Map
        createGraphSection: function (map, className, innerHtmls) {
            handlecreateGraphSection(map, className, innerHtmls);
        }
        //#endregion

        //#region all polygons removed which have current draw on map
        ,removePolygons:function(){
            handleremovePolygons();
    }
        //#endregion

        //#region all polygons drawn which have current layer
        ,drawnPolygons:function(...data){
            handledrawnPolygons(...data);
    }
        //#endregion
    }
}();

//#endregion




$(window).load(function () {
    var FullArea = $('.content-wrapper').innerHeight();

    $(".collapse1").on('click', function () {
        if ($(this).find('i').hasClass('fa-minus')) {
            $("#SiteMap").css({
                "height": FullArea - 180
            });
        }
        if ($(this).find('i').hasClass('fa-plus')) {
            $("#SiteMap").css({
                "height": "900px"
            });
        }

    });

});

