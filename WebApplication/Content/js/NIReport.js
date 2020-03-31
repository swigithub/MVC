var app = angular.module('NetLayerApp', []);
app.controller('NetLayerCtrl', function ($scope, $http, $timeout) {

    // function definitions
 //------------Start Sector Swap Charts-------------

    $scope.SectorSwapCharts = function () {
        var SiteId = $("#siteId").attr("data-siteId");
        //alert(SiteId);
        //var SiteId = "628511";

        $http.get('/Site/ListNISectorSwap?Filter=' + 'SectorSwapCharts' + '&SiteId=' + SiteId).then(function (rec) {
            if (rec.data == ""){
                return false
            }
            else{
                var sd1 = {};
                var SectorTables = rec.data[0].Tables;
                var data1 = GroupSectorLayers(rec.data);
                //console.log(data1);
                for (var i = 0; i < data1.length; i++) {

                    var netWorkLayerID = data1[i].NetLayer;
                    var networkTitle = data1[i].Sectors[0].NetLayerTitle;
                    var count = 0;
                    var mainDivSections = '<div  class="page"><h1 style="text-align:center;color:#3b39e2;">' + 'Stationary Idle Cross Sector Analysis: ' + networkTitle + '</h1><div class="page" style="width:100%;" id=' + netWorkLayerID + '></div></div>';

                    //$('#SectorCharts-' + networkTitle.split("_"[0] +'-'+networkTitle.split("_"[1] .append(mainDivSections);
                    $('#SectorCharts_' + netWorkLayerID).append(mainDivSections);
    
                    if (i == data1.length - 1) {
                        getWCPage();
                    }

                    var data2 = GroupSectorLayers2(data1[i].Sectors);
                    // console.log(data2);

                    for (var j = 0; j < data2.length; j++) {
                        var series1 = [];
                        var sectorCode = '';

                        var chartSections = [];
                        count++;
                        sectorCode = data2[j].SectorsData[0].SectorCode;
                        var data3 = GroupSectorLayers3(data2[j].SectorsData);
                        //console.log(data3);


                        for (var k = 0; k < data3.length; k++) {
                            var PCI = '';
                            var Stackbar1 = [];
                            var subItem = data3[k].SectorsPCIs;
                            PCI = data3[k].PCI;
                            var pcicolor;

                            for (var picIdx = 0; picIdx < subItem.length; picIdx++) {
                                var dateTime = moment(subItem[picIdx].serverTimeStamp).format("HH:mm:ss");
                                sd1 = { label: dateTime, y: Number(subItem[picIdx].SignalStrength) };
                                pcicolor = subItem[picIdx].ColorCode;
                                Stackbar1.push(sd1);
                            }
                            var sectorID = data2[j].SectorId;
                            series1.push({
                                type: "line",
                                legendText: PCI,
                                color: pcicolor,
                                showInLegend: true,
                                dataPoints: Stackbar1
                            });

                        }

                        var divSec = '';
                        //if (count % 2 != 0 && data2.length != count) {
                        //    divSec += '<h1>' + '' + '</h1><div style="height:300px;width:50%;float: left;" id=' + sectorID + '></div>';
                        //}
                        //else if(data2.length == count) {
                        //    divSec += '<h1>' + '' + '</h1><div style="height:300px;width:50%;" id=' + sectorID + '></div>';

                        //}
                        //else {
                        divSec += '<h1>' + '' + '</h1><div style="height:250px;width:50%;display:inline-block;vertical-align:top" id=' + sectorID + '></div>';

                        // }
                        $('#' + netWorkLayerID + '').append(divSec);
                        if (j == 0) {
                            var SecTable = '<div style="height:250px;width:50%;display:inline-block;padding-top:30px;" ><table id="sectorcharttable" style="width:100%" ><thead><tr class="tbl-heading-charttable" ><th>PCI</th><th>Sample Count</th><th>Sample Percentage</th><th>Reselection Count</th></tr></thead><tbody>';
                        }
                        else {
                            var SecTable = '<div style="height:250px;width:50%;display:inline-block;padding-top:30px;" ><table id="sectorcharttable" style="width:100%" ><thead><tr class="tbl-heading-charttable" ><th>PCI</th><th>Sample Count</th><th>Sample Percentage</th><th>Reselection Count</th></tr></thead><tbody>';
                        }
                        for (var s = 0; s < SectorTables.length; s++) {
                            if (parseInt(data2[j].SectorId) == SectorTables[s].SectorId) {
                                const Total = SectorTables.filter(word => word.SectorId == data2[j].SectorId).reduce((total, amount, index, array) => {
                                    return total + parseInt(amount.SampleCount);
                                },0);
                                // var Percentage = SectorTables.filter(word => word.SectorId == data2[j].SectorId);
                                SecTable += '<tr><td>' + SectorTables[s].PCI + '</td><td>' + SectorTables[s].SampleCount + '</td><td>' + ((SectorTables[s].SampleCount * 100) / Total) + '</td><td>' + SectorTables[s].ReselectionCount + '</td></tr>';
                            }
                        }
                        SecTable += '</tbody></table></div>';
                        $('#' + netWorkLayerID + '').append(SecTable);

                        chartSections[j] = new CanvasJS.Chart(sectorID, {
                            animationEnabled: true,
                            colorSet: "CustomColor",
                            exportEnabled: false,
                            height: "250",
                            title: {
                                text: sectorCode,
                                fontFamily: "arial black",
                                fontSize: 18,
                                fontColor: "#695A42"
                            },

                            axisX: {
                                interval: 10,
                                labelAngle: -50
                            },
                            axisY: {
                                interval: 10
                            },
                            toolTip: {
                            },
                            data: series1
                        });
                        chartSections[j].render();
                    }

                }
            }
        });

    }

    function getWCPage() {

        var wcDivSections = '<div class="WELCOME_PAGE_COLOR" id="thank-you" style="background:#004B99;padding-top:150px;height:800px;margin-left: -50px;margin-right: -20px;margin-top: 10px;padding-bottom: 50px;">' +
                                '<img src="/Content/Images/Report/Nokia-bg.png" style="margin-top: 215px;margin-left: 381px;" /></div></div>';

        $('#SectorChartsend').append(wcDivSections);

        return '';
    }

    function GroupSectorLayers(data) {

        var group_to_values = data.reduce(function (obj, item) {
            obj[item.NetLayer] = obj[item.NetLayer] || [];
            obj[item.NetLayer].push({ NetLayerTitle: item.NetLayerTitle, SignalStrength: item.SignalStrength, serverTimeStamp: item.serverTimeStamp, SectorCode: item.SectorCode, SectorId: item.SectorId, ColorCode: item.ColorCode, NetworkMode: item.NetworkMode, Band: item.Band, Carrier: item.Carrier, PCI: item.PCI });
            return obj;
        }, {});
        var groups = Object.keys(group_to_values).map(function (key) {
            return { NetLayer: key, Sectors: group_to_values[key] };
        });
        return groups;
    }

    function GroupSectorLayers2(data) {

        var group_to_values = data.reduce(function (obj, item) {
            obj[item.SectorId] = obj[item.SectorId] || [];
            obj[item.SectorId].push({ NetLayerTitle: item.NetLayerTitle, SignalStrength: item.SignalStrength, serverTimeStamp: item.serverTimeStamp, SectorCode: item.SectorCode, SectorId: item.SectorId, ColorCode: item.ColorCode, NetworkMode: item.NetworkMode, Band: item.Band, Carrier: item.Carrier, PCI: item.PCI });
            return obj;
        }, {});
        var groups = Object.keys(group_to_values).map(function (key) {
            return { SectorId: key, SectorsData: group_to_values[key] };
        });
        return groups;
    }

    function GroupSectorLayers3(data) {

        var group_to_values = data.reduce(function (obj, item) {
            obj[item.PCI] = obj[item.PCI] || [];
            obj[item.PCI].push({ NetLayerTitle: item.NetLayerTitle, SignalStrength: item.SignalStrength, serverTimeStamp: item.serverTimeStamp, SectorCode: item.SectorCode, SectorId: item.SectorId, ColorCode: item.ColorCode, NetworkMode: item.NetworkMode, Band: item.Band, Carrier: item.Carrier, PCI: item.PCI });
            return obj;
        }, {});
        var groups = Object.keys(group_to_values).map(function (key) {
            return { PCI: key, SectorsPCIs: group_to_values[key] };
        });
        return groups;
    }

    function UniqueSectors(data) {
        var uniqueSectors = [];
        for (i = 0; i < data.length; i++) {
            if (uniqueSectors.indexOf(data[i].SectorCode) === -1) {
                uniqueSectors.push(data[i].SectorCode);
            }
        }
        return uniqueSectors;
    }

    //------------End Sector Swap Charts-------------

    //------------Start TIME STAMP-------------
    $scope.GetNILayersTimeStamp = function () {

        var SiteId = $("#siteId").attr("data-siteId");
        $http.get('/Site/ListNetLayersTimeStamp?Filter=' + 'NILayersTimeStamp' + '&SiteId=' + SiteId).then(function (rec) {

            var option = ''
            var optgroup = '';
            var items = '';
            var group = GroupNetLayersTimeStamp(rec.data);
            for (var i = 0; i < group.length; i++) {
                option = '';
                for (var j = 0; j < group[i].TimeStamps.length; j++) {

                    var dateTime = moment(group[i].TimeStamps[j].serverTimeStamp).format("YYYY-MM-DD HH:mm:ss");

                    if (checkselected(items, group[i].TimeStamps[j].DefinationId)) {
                        option += '<option value="' + dateTime + '">' + group[i].TimeStamps[j].NetLayer + '</option>'
                    }
                    else {
                        //var dateTime = moment(group[i].TimeStamps[j].serverTimeStamp).format("YYYY-MM-DD HH:mm:ss");
                        option += '<option value="' + dateTime + '" selected="selected">' + dateTime + '</option>'
                    }
                }
                optgroup += '<optgroup label="' + group[i].NetLayer + '">' + option + '</optgroup>'
            }

            var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_NetLayersTimeStamp").multiselect({enableClickableOptGroups: true,allSelectedText: "Drive Timestamp"});' +
                        '});' +
                        '</script>' +
                        '<select class="form-control" style="width:0px;height:0px;" id="example-xss-html_NetLayersTimeStamp" multiple="multiple">' + optgroup + '</select>';


            $("#lstNetLayersTimeStamp").empty();
            $("#lstNetLayersTimeStamp").html(txt1);
        });
    }

    function GroupNetLayersTimeStamp(data) {
        var group_to_values = data.reduce(function (obj, item) {
            obj[item.NetLayer] = obj[item.NetLayer] || [];
            obj[item.NetLayer].push({ NetLayer: item.NetLayer, serverTimeStamp: item.serverTimeStamp });
            return obj;
        }, {});
        var groups = Object.keys(group_to_values).map(function (key) {
            return { NetLayer: key, TimeStamps: group_to_values[key] };
        });
        return groups;
    }
    //------------End TIME STAMP----------------

    $scope.GetNIReportLayers = function () {

        var SiteId = $("#siteId").attr("data-siteId");
        $http.get('/Site/ListNetLayers?Filter=' + 'NIReportLayers' + '&SiteId=' + SiteId).then(function (rec) {

            var option1 = ''
            var optgroup1 = '';

            var option2 = ''
            var optgroup2 = '';
            var items = '';

            //console.log(rec.data);
            var group = GroupNetLayers(rec.data);

            // console.log(group);
            //---------------------------------------
            for (var i = 0; i < group.length; i++) {
                option1 += '<option value="' + group[i].NetLayerIds[0].NetLayerId + '" selected="selected">' + group[i].NetLayer + '</option>'
            }
            //---------------------------------------


            for (var i = 0; i < group.length; i++) {
                option2 = '';
                for (var j = 0; j < group[i].NetLayerIds.length; j++) {
                    if (checkselected(items, group[i].NetLayerIds[j].DefinationId)) {
                        option2 += '<option value="' + group[i].NetLayerIds[j].PciId + '">' + group[i].NetLayerIds[j].PciId + '</option>'
                    }
                    else {
                        option2 += '<option value="' + group[i].NetLayerIds[j].PciId + '" selected="selected">' + group[i].NetLayerIds[j].PciId + '</option>'
                    }
                }
                optgroup2 += '<optgroup label="' + group[i].NetLayer + '">' + option2 + '</optgroup>'
            }


            var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_NetLayersName").multiselect({enableClickableOptGroups: true,allSelectedText: "Network Layers"}); });' +
                       '</script>' +
                       '<select class="form-control netlayer" style="width:0px;height:0px;"  id="example-xss-html_NetLayersName" multiple="multiple">' + option1 + '</select>';

            $("#lstNetLayersName").empty();
            $("#lstNetLayersName").html(txt1);

            //---------------------

            var txt2 = '<script type="text/javascript">$(document).ready(function() {$("#example--xss-html_NetLayersPci").multiselect({enableClickableOptGroups: true,allSelectedText: "PCI/PSC/BCCH"}); $(".multiselect").on("click", function () { $(this).parent(".btn-group").find(".dropdown-menu").slideToggle();});});' +
                       '</script>' +
                       '<select class="form-control" style="width:0px;height:0px;" id="example--xss-html_NetLayersPci" multiple="multiple">' + optgroup2 + '</select>';

            $("#lstNetLayersPci").empty();
            $("#lstNetLayersPci").html(txt2);
        });
    }

    function GroupNetLayers(data) {
        var group_to_values = data.reduce(function (obj, item) {
            obj[item.NetLayer] = obj[item.NetLayer] || [];
            obj[item.NetLayer].push({ NetLayer: item.NetLayer, NetLayerId: item.NetLayerId, PciId: item.PciId });
            return obj;
        }, {});
        var groups = Object.keys(group_to_values).map(function (key) {
            return { NetLayer: key, NetLayerIds: group_to_values[key] };
        });
        return groups;
    }
    //----------------------------

    function checkselected(data, value) {
        if (value != undefined) {
            for (var i = 0; i < data.length; i++) {
                if (data[i] == value.toString()) {
                    return true;
                }
            }
            return false;
        }
        return false;
    }
    //------------------------------
    // function calls
    $scope.GetNILayersTimeStamp();
    $timeout(function () {
        $scope.SectorSwapCharts();
        $scope.GetNIReportLayers();
        var newPage = $("#WelcomePage").html();
        $("#WelcomePage").html(newPage);

    }, 3000);

});