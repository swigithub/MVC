var myApp = angular.module('myApp', ['ngTable']);



myApp.service('ngSrvcWO', function ($http) {
    debugger;
    this.getWOs = function (value1, value2, Status) {
        var res = $http({
            method: "post",
            url: "/WorkOrder/WOListAngular",
            data: JSON.stringify({ value1: value1, value2: value2, value3: Status }),
            dataType: "json"
        });
        return res;
    };
});


myApp.controller('tableController', function ($scope, $filter, ngSrvcWO, ngTableParams) {

    $scope.v1 = "11/01/2017";
    $scope.v2 = "11/15/2017";
    $scope.Status = true;
    $scope.workorders = {};


    //---------------------------------

    $scope.WoRefNo = true;
    $scope.Region = true;
    $scope.Market = true;
    $scope.SiteCode = true;

    $scope.Tester = true;
    $scope.ReceivedOn = true;
    $scope.SubmittedOn = true;
    $scope.ScheduledOn = true;

    $scope.DriveCompletedOn = true;
    $scope.ReportSubmittedOn = true;
    $scope.CompletedOn = true;

    $scope.tblColumns = [];
    //---------------------------------
    $scope.tblColumns =

                        [{ id: "WoRefNo", name: "WoRefNo" },
                        { id: "Region", name: "Region" },
                        { id: "Market", name: "Market" },
                        { id: "SiteCode", name: "SiteCode" },

                        { id: "Tester", name: "Tester" },
                        { id: "ReceivedOn", name: "ReceivedOn" },
                        { id: "SubmittedOn", name: "SubmittedOn" },
                        { id: "ScheduledOn", name: "ScheduledOn" },

                        { id: "DriveCompletedOn", name: "DriveCompletedOn" },
                        { id: "ReportSubmittedOn", name: "ReportSubmittedOn" },
                        { id: "CompletedOn", name: "CompletedOn" }];

    //$scope.selectedIds = [];
    $scope.selectedIds = ["WoRefNo", "Region", "Market", "SiteCode", "Tester", "ReceivedOn", "SubmittedOn", "ScheduledOn", "DriveCompletedOn", "ReportSubmittedOn", "CompletedOn"];

    function GetWorkOrders() {

        var Data = ngSrvcWO.getWOs($scope.v1, $scope.v2, $scope.Status);
        Data.then(function (wo) {
            $scope.workorders = wo.data;

            for (var i = 0; i < $scope.workorders.length; i++) {

                var dtRec = $scope.workorders[i].ReceivedOn;
                var Rec = $filter('dateFilter')(dtRec);
                $scope.workorders[i].ReceivedOn = Rec;


                var dtSub = $scope.workorders[i].SubmittedOn;
                var Sub = $filter('dateFilter')(dtSub);
                $scope.workorders[i].SubmittedOn = Sub;

                var dtSch = $scope.workorders[i].ScheduledOn;
                var Sch = $filter('dateFilter')(dtSch);
                if (Sch != "01/01/0001") {
                    $scope.workorders[i].ScheduledOn = Sch;
                }
                else {
                    $scope.workorders[i].ScheduledOn = "";
                }

                var dtDriveComp = $scope.workorders[i].DriveCompletedOn;
                var DriveComp = $filter('dateFilter')(dtDriveComp);

                if (DriveComp != "01/01/0001") {
                    $scope.workorders[i].DriveCompletedOn = DriveComp;
                }
                else {
                    $scope.workorders[i].DriveCompletedOn = "";
                }

                var dtReportSub = $scope.workorders[i].ReportSubmittedOn;
                var ReportSub = $filter('dateFilter')(dtReportSub);
                if (ReportSub != "01/01/0001") {
                    $scope.workorders[i].ReportSubmittedOn = ReportSub;
                }
                else {
                    $scope.workorders[i].ReportSubmittedOn = "";
                }

                var dtCompletedOn = $scope.workorders[i].CompletedOn;
                var CompletedOn = $filter('dateFilter')(dtCompletedOn);
                if (CompletedOn != "01/01/0001") {
                    $scope.workorders[i].CompletedOn = CompletedOn;
                }
                else {
                    $scope.workorders[i].CompletedOn = "";
                }
            }

            // for orderby and pagination
            $scope.wosTable = new ngTableParams({
                page: 1,
                count: 10 //$scope.workorders.length
            }, {
                total: $scope.workorders.length,
                //counts: [],
                getData: function ($defer, params) {                   
                    $scope.data = params.sorting() ? $filter('orderBy')($scope.workorders, params.orderBy()) : $scope.workorders;
                    $scope.data = params.filter() ? $filter('filter')($scope.data, params.filter()) : $scope.data;
                    $scope.data = $scope.data.slice((params.page() - 1) * params.count(), params.page() * params.count());
                    $defer.resolve($scope.data);
                }
            });

        }, function () {
            alert('Error');
        });
    }

    GetWorkOrders();
});

//--------------------------
myApp.directive('dropdownMultiselect', function () {
    return {
        restrict: 'E',
     
        scope: {
            model: '=',
            options: '=',          
        },
        template:
                "<div class='btn-group' data-ng-class='{open: open}'>" +
                    "<button class='btn btn-small'>Show/Hide Columns</button>" +
                    "<button class='btn btn-small dropdown-toggle' data-ng-click='openDropdown()'><span class='caret'></span></button>" +
                    "<ul class='dropdown-menu' aria-labelledby='dropdownMenu'>" +
                    "<li><a data-ng-click='selectAll()'><span class='glyphicon glyphicon-ok green' aria-hidden='true'></span> Check All</a></li>" +
                    "<li><a data-ng-click='deselectAll();'> <span class='glyphicon glyphicon-remove red'  aria-hidden='true'></span> Uncheck All</a></li>" +
                    "<li class='divider'></li>" +
                    "<li data-ng-repeat='option in options'><a data-ng-click='toggleSelectItem(option)'><span data-ng-class='getClassName(option)' aria-hidden='true'></span> {{option.name}}</a></li>" +
                    "</ul>" +
                "</div>",
        controller: function ($scope) {
            $scope.openDropdown = function () {
                $scope.open = !$scope.open;
            };

            $scope.selectAll = function () {
                $scope.model = [];
                angular.forEach($scope.options, function (item, index) {
                    $scope.model.push(item.id);
                });
            };

            $scope.deselectAll = function () {
                $scope.model = [];
            };

            $scope.toggleSelectItem = function (option) {
                var intIndex = -1;
                angular.forEach($scope.model, function (item, index) {
                    if (item == option.id) {
                        intIndex = index;
                    }
                });

                if (intIndex >= 0) {
                    $scope.model.splice(intIndex, 1);

                    $('.' + option.name).remove();

                    var obn = $('[name="' + option.name + '"]');
                    $(obn).parent().parent().parent().hide();

                    var spans = $("span:contains(" + option.name + ")")
                    spans.hide();
                    $(spans).parent().parent().hide();
                }
                else {

                    var spans = $("span:contains(" + option.name + ")")
                    $(spans).parent().parent().show();
                    spans.show();

                    var obn = $('[name="' + option.name + '"]');
                    $(obn).parent().parent().parent().show();

                    $('.' + option.name).show();

                    $scope.model.push(option.name);

                }
            };

            $scope.getClassName = function (option) {
                var varClassName = 'glyphicon glyphicon-remove red';
                angular.forEach($scope.model, function (item, index) {
                    if (item == option.id) {
                        varClassName = 'glyphicon glyphicon-ok green';
                    }
                });
                return (varClassName);
            };

        }
    }
});
//-------------------------

//-------------------------------
myApp.filter("dateFilter", function ($filter) {
    return function (item) {
        if (item != null) {
            var parsedDate = new Date(parseInt(item.substr(6)));
            //return $filter('date')(parsedDate, 'yyyy-MM-dd');
            return $filter('date')(parsedDate, 'MM/dd/yyyy');
        }
        return "";
    };
});
