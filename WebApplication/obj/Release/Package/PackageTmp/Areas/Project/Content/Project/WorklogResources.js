var app = angular.module('WorklogApp', []);
app.controller('WorklogsCtrl', function ($scope, $rootScope, $window, $http) {

    $('#tblLogCost').hide();
    $('#tblWorklogs').hide();

    angular.element(document).ready(function () {
        $scope.GetWorklogs($scope.selectedOption);
        //$scope.WorkLogsCost();
    });

    $('a[class="ApprovedWorkLog"]').on('shown.bs.tab', function (e) {
        $scope.WorkLogsCost();
    });
    
    $scope.GetWorklogs = function (selectedOpt) {
        var filter = 'WorkLogs';
        var dateRange = $('.daterangepicker-field').val();
        var selectedOpt = selectedOpt;

        var parts = dateRange.split("-");
        var first_date = parts[0];
        var last_date = parts[1];

        $scope.TargetType = filter;

        $.post('/Project/WorkLog/GetWorklogs', { filter: filter, value: first_date, value2: last_date, SelectOption: selectedOpt }, function (res) {
            $scope.Worklogs = res.Worklogs;

            if (res.Worklogs.length) {
                $('#tblWorklogs').show();
                for (var i = 0; i < $scope.Worklogs.length; i++) {
                    $scope.Worklogs[i].LogDate = new Date(moment.utc($scope.Worklogs[i].LogDate));
                }
            }
            else {
                $('#tblWorklogs').hide();
            }

            $scope.$apply();
        }).complete(function () {
        }).error(function () {
        });
    };
    //------------------------------
    $scope.WorkLogsCost = function () {

        var dateRange = $('.daterangepickerCost').val();

        var filter = 'WorkLogsCost';

        var parts = dateRange.split("-");
        var first_date = parts[0];
        var last_date = parts[1];

        $scope.TargetType = filter;

        $.post('/Project/WorkLog/WorkLogsCost', { filter: filter, value: first_date, value2: last_date }, function (res) {

            $scope.wlogsCostChart = res.wlogsCostChart;
            $scope.WlogsCost = res.WlogsCost;

            //console.log($scope.wlogsCostChart);
            //console.log($scope.WlogsCost);
            //-----------------------------

            var totalHours = 0;
            var totalCost = 0;
            
            for (var i = 0; i < $scope.WlogsCost.length; i++) {
                totalHours += $scope.WlogsCost[i].LogHours;
                $scope.totalHours = totalHours;

                totalCost += $scope.WlogsCost[i].LogHours * $scope.WlogsCost[i].RatePerUnit;
                $scope.totalCost = totalCost;
            }
            //-----------------------------

            $('#chart_IssuesTasks').empty();
            $('#chart_IssuesTasks_Cost').empty();

            var sd1 = {};
            var sd2 = {};
            //var worklogStackbar = [];
            //var costStackbar = [];
            var seriesHours = [];
            var seriesCost = [];
            var data = Groupworklogs($scope.wlogsCostChart);
            //console.log(data);
            //debugger;
            if (res.wlogsCostChart.length > 0) {
                $('#ChartsDiv').show();
                $('#tblLogCost').show();
                for (var i = 0; i < data.length; i++) {
                    var HoursStackbar = [];
                    var CostStackbar = [];

                    for (var j = 0; j < data[i].data.length; j++) {
                        sd1 = { label: data[i].data[j].Name, LogHours: data[i].data[j].LogHours, y: data[i].data[j].y, LogHours: data[i].data[j].LogHours };
                        sd2 = { label: data[i].data[j].Name, LogHours: data[i].data[j].LogHours, y: data[i].data[j].Cost, LogHours: data[i].data[j].LogHours };
                        HoursStackbar.push(sd1);
                        CostStackbar.push(sd2);
                    }
                    seriesHours.push({
                        type: "stackedColumn",
                        legendText: data[i].LogType,
                        showInLegend: true,
                        dataPoints: HoursStackbar,
                        indexLabel: "{y} Hours",
                        valueFormatString:"{y} Hours"
                        
                    });
                    //--------------------
                    seriesCost.push({
                        type: "stackedColumn",
                        legendText: data[i].LogType,
                        showInLegend: true,
                        dataPoints: CostStackbar,
                        indexLabel: "${y}",
                        valueFormatString:"{y}$"
                    });
                }

                var chartWorkLogHours = new CanvasJS.Chart("chart_IssuesTasks", {
                    animationEnabled: true,
                    colorSet: "CustomColor",
                    exportEnabled: false,
                    title: {
                        text: "Worklog Hours",
                        fontFamily: "arial black",
                        fontSize: 12,
                        fontColor: "#695A42"
                    },
                    axisX: {
                        interval: 1
                    },
                    axisY: {
                        interval: 25
                    },
                    toolTip: {
                        content: "{label}: {y} Hours"
                    },
                    data: seriesHours
                });
                chartWorkLogHours.render();

                //-------------------------
                var chartWorkLogCost = new CanvasJS.Chart("chart_IssuesTasks_Cost", {
                    animationEnabled: true,
                    colorSet: "CustomColor",
                    exportEnabled: false,
                    title: {
                        text: "Worklog Cost",
                        fontFamily: "arial black",
                        fontSize: 12,
                        fontColor: "#695A42"
                    },
                    axisX: {
                        interval: 1
                    },
                    axisY: {
                        interval: 100
                    },
                    toolTip: {
                        content: "{label}: ${y}"
                    },
                    data: seriesCost
                });
                chartWorkLogCost.render();
                //------------------------------------
            }
            else {
                $('#ChartsDiv').hide();
                $('#tblLogCost').hide();
            }
            $scope.$apply();
        }).complete(function () {

        }).error(function () {

        });
    };


    //----------------------------
    function Groupworklogs(data) {
        var group_to_values = data.reduce(function (obj, item) {
            //debugger;
            obj[item.LogType] = obj[item.LogType.toLowerCase()] || [];
            obj[item.LogType].push({ Name: item.Name, LogType: item.LogType, LogHours: item.LogHours, Cost: item.Cost, y: item.LogHours });
            return obj;
        }, {});

        var groups = Object.keys(group_to_values).map(function (key) {
            return { LogType: key, data: group_to_values[key] };
        });
        return groups;
    }
    //------------------------------

    function Groupworklogs2(data2) {
        //debugger;
        for (var i = 0; i < data2.length; i++) {
            var group_to_values = data2[i].data.reduce(function (obj, item) {
                obj[item.Name] = obj[item.Name] || [];
                obj[item.Name].push({ Name: item.Name, LogType: item.LogType, LogHours: item.LogHours, y: item.LogHours });
                return obj;
            }, {});
        }


        var groups = Object.keys(group_to_values).map(function (key) {
            return { Name: key, data2: group_to_values[key] };
        });
        return groups;
    }
    //----------------------


    $scope.ApproveWorklog = function (WLogId, IsApproved, selectedOption) {
        var filter = 'ApproveWorkLog';
        $.post('/Project/WorkLog/ApproveWorklog', { Filter: filter, WLogId: WLogId, IsApproved: IsApproved }, function (res) {
            $scope.$apply();
            var Status = IsApproved == true ? "Aproved" : "Unaproved";
            $.notify("Log " + Status + " Successfully", {
                className: 'success',
                globalPosition: 'top center'
            });
        }).complete(function () {
        }).error(function () {
        });
        $scope.GetWorklogs(selectedOption);
    };



    //end WorklogsCtrl
});