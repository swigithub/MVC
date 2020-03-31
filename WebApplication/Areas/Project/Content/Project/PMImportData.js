
var app = angular.module('PmImportApp', []);

app.controller('PmImportCtrl', function ($scope, $rootScope, $window, $http, $timeout) {

    $scope.PmImportData = {};
    
    $scope.GetPmImportData = function (filter, first_date, last_date) {

        $scope.TargetType = filter;

        var MilestoneId = $('#mileStoneId').attr('data-mileStoneId');
        var StageId = $('#stageId').attr('data-StageId');
        debugger;
        $scope.MilestoneId = MilestoneId;
        $scope.StageId = StageId;

        if ($scope.MilestoneId > 0) {
            $.post('/Project/Targets/GetTargetDates', { filter: filter, value: first_date, value2: last_date, ProjectId, MilestoneId, StageId }, function (res) {

                $scope.TargetDates = res.targetDates;
                for (var i = 0; i < $scope.TargetDates.length; i++) {
                    $scope.TargetDates[i].StartDate = new Date(moment.utc($scope.TargetDates[i].StartDate));
                    $scope.TargetDates[i].EndDate = new Date(moment.utc($scope.TargetDates[i].EndDate));
                }

                siteCount(res.fcHistory);

                if ($scope.TargetType == 'day') {
                    $scope.showHideCol = false;
                }
                else {
                    $scope.showHideCol = true;
                }

                $scope.$apply();
            });
        
        }
});
