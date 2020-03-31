var app = angular.module('TargetsApp', []);

app.controller('TargetsCtrl', function ($scope, $rootScope, $window, $http, $timeout) {
    $scope.myTempVal = '';

    var UID = $('#userid').attr('data-UserId');
    var ProjectId = $('#projectId').attr('data-ProjectId');

    $scope.ProjectId = ProjectId;

    $scope.showHidemStone = false;
    $scope.showHideStage = false;

    $scope.showHideLoader = false;

    $scope.radioList2 = [];
    $scope.radioList3 = [];

    $scope.successMessage = false;
    $scope.successMessagebool = false;

    $scope.TargetDates = {};
    $scope.Tar = {};

    $('#mydiv').hide();
    //------------------------------
    $scope.radioList1 = [{
        //    id: '1',
        //    name: "Project",
        //    checked:true
        //}, {
        id: '2',
        name: "Milestone",
        checked: true
    }, {
        id: '3',
        name: "Stage",
        checked: false
    }];

    //------------------------------
    //get project milestones
    $.ajax({
        type: 'post',
        url: '/Project/Targets/ToList',
        async: false,
        data: { Filter: 'ByTaskTypeKeyCode', Value: 'PROJECT_MILESTONE', value2: ProjectId },
        success: function (result) {
            $scope.mStonesData = result;
            debugger;
            for (var i = 0; i < $scope.mStonesData.length; i++) {
                $scope.radioList2.push({
                    MilestoneId: $scope.mStonesData[i].TaskId,
                    name: $scope.mStonesData[i].Title
                });
            }
            $('#mydiv').hide();
        }
    });

    //------------------------------
    $scope.showHidemStone = true;
    $scope.targetTypeClick = function (targetCat) {
        // alert(targetCat.id);
        //if (targetCat == 'Project') {
        //    $scope.showHidemStone = false;
        //    $scope.showHideStage = false;
        //    $scope.showHideTargetType = true;
        //    //$scope.radioList2.length = 0;
        //    $scope.radioList2 = [];
        //    $scope.radioList3.length = 0;
        //}
        if (targetCat == 'Milestone') {
            $scope.showHidemStone = true;
            $scope.showHideStage = false;
            $scope.radioList3 = [];
            $scope.radioList3.length = 0;
        }

        else if (targetCat == 'Stage') {
            $scope.showHidemStone = true;
            $scope.showHideStage = true;
        }
    };
    // -----------------------------------
  
    //------------------------------

    $scope.mStoneClick = function (mStoneId) {

        if ($scope.showHideStage == 'true') {
            $scope.showHideStage = true;
            $scope.radioList3 = [];
            $scope.radioList3.length = 0;
        }
        else {
            $scope.radioList3 = [];
            $scope.radioList3.length = 0;
        }

        //get milestone stages
        $.ajax({
            type: 'post',
            url: '/Project/Targets/ToList',
            async: false,
            data: { Filter: 'ByPTaskId', Value: mStoneId, value2: ProjectId },
            success: function (result) {
                $scope.mStagessData = result;
                for (var i = 0; i < $scope.mStagessData.length; i++) {
                    $scope.radioList3.push({
                        StageId: $scope.mStagessData[i].TaskId,
                        name: $scope.mStagessData[i].Title
                    });
                }
            }
        });
    };

    //----------------------------------
    $scope.TargetType = '';
    $scope.showHideCol = true;
    $scope.showHideLoader = false;

    $scope.GetTargetDates = function (filter, first_date, last_date) {

        //$('#wid-id-11').css({ 'display': "block" });
        //$('#wid-id-12').css({ 'display': "block" });
        $scope.TargetType = filter;
        var MilestoneId = tasksComboTree.getSelectedItemsId();
        var StageId = $('#stageId').attr('data-StageId');
        $scope.MilestoneId = MilestoneId;
        $scope.StageId = StageId;
        if ($scope.MilestoneId > 0) {
            $('#mydiv').show();
            debugger;
            $.post('/Project/Targets/GetTargetDates', { filter: filter, value: first_date, value2: last_date, ProjectId, MilestoneId, StageId }, function (res) {

                $scope.TargetDates = res.targetDates;
                for (var i = 0; i < $scope.TargetDates.length; i++) {
                    $scope.TargetDates[i].StartDate = new Date(moment.utc($scope.TargetDates[i].StartDate));
                    $scope.TargetDates[i].EndDate = new Date(moment.utc($scope.TargetDates[i].EndDate));
                    if (sitecount.length > 0) {
                        $scope.TargetDates[i].SiteCount = sitecount[i];
                    }
if(res.fcHistory !=undefined){
                    for (j = 0; j < res.fcHistory.length; j++) {
                        if (new Date(moment.utc(res.fcHistory[j].TargetValue)).toString() === $scope.TargetDates[i].StartDate.toString()){
                            $scope.TargetDates[i].SiteCount = res.fcHistory[j].SiteCount
                        }
                    }
}
                   
                }
                var a = 0;
                res.fcHistory = res.fcHistory.sort(function (a, b) {
                    var dateA = new Date(a.TargetValue), dateB = new Date(b.TargetValue);
                    return dateA - dateB;
                });
                for (j = 0; j < res.fcHistory.length; j++) {
                    if (new Date(moment.utc(res.fcHistory[j].TargetValue)) >= new Date(moment.utc(first_date)) && new Date(moment.utc(res.fcHistory[j].TargetValue)) <= new Date(moment.utc(last_date))) {
                        siteCount(res.fcHistory);
                        a++;
                    }
                   
                }
                if(a==0){
                    res.fcHistory = [];
                    siteCount(res.fcHistory);
                }
            

                if ($scope.TargetType == 'day') {
                    $scope.showHideCol = false;
                }
                else {
                    $scope.showHideCol = true;
                }
                $scope.$apply();
            }).complete(function () {
                $('#mydiv').hide();
            }).error(function () { //hide
                $('#mydiv').hide();
            });
        }
        else {
            $scope.successMessage = "Please Select Milsestone";
            $scope.successMessagebool = true;
            $timeout(function () {
                $scope.successMessagebool = false;
            }, 3000);
            return;
        }
    };
   
    //------------------------------
    $scope.Save = function (TargetDates) {

        $scope.Tar.MilestoneId = tasksComboTree.getSelectedItemsId();
        for (var i = 0; i < TargetDates.length; i++) {

            TargetDates[i].ProjectId = ProjectId;
            TargetDates[i].MilestoneId = $scope.Tar.MilestoneId;
            TargetDates[i].StageId = $scope.Tar.StageId;
            TargetDates[i].TargetType = $scope.TargetType;
            TargetDates[i].UserId = UID;
            sitecount[i] = TargetDates[i].SiteCount
           
        }
      
        
        if ($scope.Tar.MilestoneId > 0) {
            $('#mydiv').show();
            debugger;
            $http.post("/Project/Targets/NewTarget", TargetDates).then(function (res) {
                $scope.Tar = {};
                $scope.TargetDates = {};

                if (res.status == '200') {
                    $scope.successMessage = "Forecaste submitted successfully";
                    $scope.successMessagebool = true;
                    swal("Success", "Forecaste submitted successfully!", "success");
                } else {
                    $scope.successMessage = "Forecaste not submitted";
                    swal("Alert", "Forecaste not submitted!", "error");
                    //$scope.successMessagebool = true;
                    //$timeout(function () {
                    //    $scope.successMessagebool = false;
                    //}, 3000);
                }
               // $('#mydiv').hide();
            });
        }

        //$scope.successMessage = "Please Select Milsestone";
        //$scope.successMessagebool = true;
        //$timeout(function () {
        //    $scope.successMessagebool = false;
        //}, 3000);
    }
   // $scope.GetTargetDates(Period, First_date, Last_date);
    //end TargetsCtrl
});


app.filter('reverse', function () {
    return function (items) {
        return items.slice().reverse();
    };
});

//--------------------------------

