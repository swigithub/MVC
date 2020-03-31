//$('.date').datepicker({
//    format: 'mm/dd/yyyy',
//    autoclose: true
//});

var app = angular.module('ProjectManagment', []);

app.service('service', function ($q, $compile, $http, constants) {
    // Get Data
    this.post = function (url, param) {
        $(".spinner").show();
        //var promise = $http.post(constants.mainPath + url, param);
        var promise = $http.post(url, param);
        promise = promise.then(function (response) {
            $(".spinner").hide();
            return response.data;
        });
        return promise;
    };

    this.get = function (url) {
        $(".spinner").show();
        //var promise = $http.post(constants.mainPath + url, param);
        var promise = $http.get(url);
        promise = promise.then(function (response) {
            $(".spinner").hide();
            return response.data;
        });
        return promise;
    };
});
// constants
app.constant('constants', {
    //mainPath: '/emr/json'
    basePathDirective: 'https://emremr.com/assets/app/directives/templates/',
    basePath: 'https://emremr.com/',
    mainPath: 'https://emremr.com/api/index.php/api/',
    timeout: 5000,
    user_roles: {
        patient: 1,
        doctor: 2,
    },
})
 .run(function ($rootScope, constants) {
     $rootScope.constants = constants;
 });

app.controller('Defination', function ($scope, $http, service) {
    $scope.Clients = [];
    $scope.Status = [];
    $scope.Entity = [];

    $scope.Priorities = [];
    $scope.Managers = [];
    $scope.WDays = '';
    $scope.p = {};
    $scope.albumNameArray = [];
    $scope.WorkGroupss = [];
    $scope.WorkGroups = [];
    $scope.Groups = '';
  
    // $scope.p.Color = "#A0522D",
    $scope.albums = [{
        id: 1,
        name: 'Monday'
    },
 {
     id: 2,
     name: 'Tuesday'
 },
 {
     id: 3,
     name: 'Wednesday'
 },
 {
     id: 4,
     name: 'Thursday'
 },
 {
     id: 5,
     name: 'Friday'
 },
 {
     id: 6,
     name: 'Saturday'
 },
  {
      id: 7,
      name: 'Sunday'
  }
    ];
    var a = service.post("/Client/ToList?filter=All");

    var b = service.post("/Defnation/ToList?filter=byDefinationType&value=Priority");


    var c = service.post("/Defination/ToList?filter=ProjectManagersWithRolls&value=" + UserId);

    var d = service.get("/Defnation/ToList?filter=byDefinationType&value=Task Types");
    var e = service.get("/Project/Defination/GetEntities?filter=ProjectEntities&value=Project Entities");
    var f = service.get("/Defnation/GetGroup/");
    var g = service.get("/Defnation/ToList?filter=byDefinationType&value=Project Status");
    var h = service.get("/Defnation/ToList?filter=byDefinationType&value=Project Categories");
    var i = service.get("/Defnation/ToList?filter=byDefinationType&value=Currencies");

    Promise.all([a, b, c, d, e, f, g,h,i]).then(function (index) {
        $scope.Clients = index[0];

        $scope.Priorities = index[1];
        $scope.p.CompletionPercent = 0;

        $scope.Managers = index[2];

        $scope.TaskTypes = index[3];

        $scope.Entity = index[4];
        $scope.Status = index[6];
        $scope.Category = index[7];
        $scope.Currency = index[8];

        $scope.$apply(function () {
            $scope.WorkGroups = index[5];
        });

      

        if (ProjectId < 0 || ProjectId == undefined || ProjectId == 0 || ProjectId == "") {
            $('input:checkbox.workday').each(function (index, value) {
                if (xj < 5) {
                    $(this).prop('checked', true);
                    xj++;
                }
            });
            $scope.p.StatusId = $scope.Status[0].DefinationId
            if ($scope.Priorities.length > 0) {
                $scope.p.PriorityId = $scope.Priorities[0].DefinationId;
            }

        }


        if (ProjectId > 0) {
            service.post("/project/Defination/ToSingle", { Filter: 'ByProjectId', Value: ProjectId }).then(function (res) {
                if (res != null) {
                    $scope.p = res;
                    $scope.Groups= res.Groups;
                    if (res.WorkingDays != null) {
                        $scope.albumNameArray = res.WorkingDays.split(',');
                        $('#main #content form  input[type=checkbox]').prop('checked', false);
                        $scope.albumNameArray.forEach(function (e, i) {
                            //$('#main #content form  input[value=' + e + ']').prop('checked', true);
                            if (e) {
                                $('#main #content form  input[value=' + e + '][type=checkbox]').prop('checked', true);
                            }
                        });
                    }
                    if (res.WorkGroups != null) {
                        $scope.WorkGroupss = res.WorkGroups.split(',');
                        $('#main #content form  #WG input[type=checkbox]').prop('checked', false);
                        // $scope.WorkGroups.forEach(function (j, k) {
                        //$('#main #content form  input[value=' + e + ']').prop('checked', true);
                        for (j = 0; j < $scope.WorkGroupss.length ; j++) {
                            if ($scope.WorkGroupss[j]) {
                                $('#main #content form #WG input[value=' + $scope.WorkGroupss[j] + '][type=checkbox][name=wg]').prop('checked', true);
                            }
                        }
                        // });
                    }
                    console.log('$scope.p.ManagerId:' + $scope.p.ManagerId);
                    if (res.ManagerId != null) {
                        $scope.p.ManagerId = res.ManagerId;
                        //console.log('$scope.p.ManagerId :' + $scope.p.ManagerId );
                    }

                    if (res.ActualStartDate != null) {
                        $scope.p.ActualStartDate = moment(res.ActualStartDate).format("MM/DD/YYYY");
                    }

                    if (res.TargetDate != null) {
                        $scope.p.TargetDate = moment(res.TargetDate).format("MM/DD/YYYY");
                    }

                    if (res.ActualEndDate != null) {
                        $scope.p.ActualEndDate = moment(res.ActualEndDate).format("MM/DD/YYYY");
                    }
                    if (res.PlannedDate != null) {
                        $scope.p.PlannedDate = moment(res.PlannedDate).format("MM/DD/YYYY");
                    }
                    if (res.EstimateEndDate != null) {
                        res.EstimateEndDate = moment(res.EstimateEndDate).format("MM/DD/YYYY");
                    }
                    if (res.EstimateStartDate != null) {
                        res.EstimateStartDate = moment(res.EstimateEndDate).format("MM/DD/YYYY");
                    }
                    if (res.GH != null) {
                        if (res.GH.length > 0 && res.GH != null) {
                            var holidaythread = $.parseHTML(`<thead><tr><th>Title</th><th>Date</th><th>Is Off Day</th><th>Delete</th></tr></thead>`);
                            $(".holiday-table ").append(holidaythread);
                            x = 1;
                            for (var i = 0; i < res.GH.length; i++) {
                                var title = '<tr><td><input type="text" required  value="' + res.GH[i].Title + '" /></td>'
                                var Date = '<td><input type="text" date-picker value="' + moment(res.GH[i].Date).format('L') + '" /></td>'
                                var isofday = '';
                                if (res.GH[i].IsOffday == true) {
                                    isofday = '<td><input type="checkbox"   checked value="true" /></td><td><a href="#" class="del-holiday btn btn-danger btn-xs">Delete Row</a></td></tr>'
                                } else {
                                    isofday = '<td><input type="checkbox"  value="false" /></td><td><a href="#" class="del-holiday btn btn-danger btn-xs">Delete Row</a></td></tr>'
                                }
                                var holidayRow = $.parseHTML(title + Date + isofday);
                                $(".holiday-table tbody").append(holidayRow);
                                $('input[date-picker]').datepicker({
                                    format: 'mm/dd/yyyy',
                                    autoclose: true
                                })
                            }
                        }
                    }


                    if (res.TS != null) {
                        if (res.TS.length > 0 && res.TS != null) {
                            var Stagethread = $.parseHTML(`<thead><tr><th>Title</th><th>Description</th><th>Sort Order</th><th>Delete</th></tr></thead>`);
                            $(".TS-table ").append(Stagethread);
                            y = 1;
                            for (var i = 0; i < res.TS.length; i++) {
                                var title = '<tr><td><input type="text" required  value="' + res.TS[i].Title + '" /></td>'
                                var Description = '<td><input type="text" required value="' + res.TS[i].Description + '" /></td>'
                                var SortOrder = '<td><input type="number" min="0" required value="' + res.TS[i].SortOrder + '" /></td>';
                                var StageId = '<td style="display: none"><input type="hidden" value="' + res.TS[i].StageId + '" /></td>';
                                var IsDeleted = '<td><a href="#" data-StageId="' + res.TS[i].StageId + '" data-isDeleted="' + res.TS[i].IsDeleted + '" title="Delete Row" class="del-taskStage btn btn-default bg-color-red txt-color-white"><i class="fa fa-trash-o"></i></a></td></tr>';
                                holidayRow = $.parseHTML(title + Description + SortOrder + StageId + IsDeleted);
                                $(".TS-table tbody").append(holidayRow);
                            }
                        }
                    }



                    $("#Color").spectrum("set", $scope.p.Color);
                    $scope.p.StatusId = res.StatusId;

                }
            });
            ProjectTitles();
        }
    });
    $scope.GroupCheckchange = function (id) {
        $http.post("/project/Defination/ToSingle", { Filter: 'ByProjectId', Value: ProjectId }).then(function (result) {
            $scope.Groups = result.data.Groups;
            $scope.groupids = $scope.Groups.split(',');
            for (i = 0; i < $scope.groupids.length; i++) {
                if (id == $scope.groupids[i]) {
                    $.notify('you can not change it ', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                    $('#main #content form #WG input[value=' + id + '][type=checkbox][name=wg]').prop('checked', true);
                }
            }
        });
      



    }
    $scope.UpdateTabsData = function () {
        if (ProjectId > 0) {
            $http.post("/project/Defination/ToSingle", { Filter: 'ByProjectId', Value: ProjectId }).then(function (result) {

                var res = result.data;
                $scope.Groups = result.data.Groups;
                if (res != null && res != "undefined") {
                    if (res.TS != null && res.TS != "undefined") {
                        if (res.TS.length > 0) {
                            $(".TS-table tbody").empty();
                            for (var i = 0; i < res.TS.length; i++) {
                                var title = '<tr><td><input type="text" required  value="' + res.TS[i].Title + '" /></td>'
                                var Description = '<td><input type="text" required value="' + res.TS[i].Description + '" /></td>'
                                var SortOrder = '<td><input type="number" min="0" max="9999" required value="' + res.TS[i].SortOrder + '" /></td>';
                                var StageId = '<td style="display: none"><input type="hidden" value="' + res.TS[i].StageId + '" /></td>';
                                var IsDeleted = '<td><a href="#" data-StageId="' + res.TS[i].StageId + '" data-isDeleted="' + res.TS[i].IsDeleted + '" title="Delete Row" class="del-taskStage btn btn-default bg-color-red txt-color-white"><i class="fa fa-trash-o"></i></a></td></tr>';
                                var holidayRow = $.parseHTML(title + Description + SortOrder + StageId + IsDeleted);
                                $(".TS-table tbody").append(holidayRow);
                            }
                        }
                    }
                    
                }
            });
          ProjectTitles();
        }

    }

    $scope.Save = function () {
        $scope.GH = [];
        $scope.TS = [];
        $.each($('.aaa').parent().siblings('div.tab-content').find('#holidays table.holiday-table tbody tr'), function (i, e) {
            var cols = $(e).find('td');
            var rowValues = {
                Title: $(cols[0]).find('input').val(),
                Date: $(cols[1]).find('input').val(),
                IsOffday: $(cols[2]).find('input').is(':checked')

            };
            if (rowValues.Title) {
                $scope.GH.push(rowValues);
            }
        });

        $.each($('.aaa').parent().siblings('div.tab-content').find('#TaskStagesTab table.TS-table tbody tr'), function (i, e) {
            var cols = $(e).find('td');
            var rowValues = {
                Title: $(cols[0]).find('input').val(),
                Description: $(cols[1]).find('input').val(),
                SortOrder: $(cols[2]).find('input').val(),
                StageId: $(cols[3]).find('input').val(),
                IsDeleted: $(cols[4]).find('a').attr("data-isDeleted")
            };
            if (rowValues.Title && rowValues.Description && rowValues.SortOrder) {
                $scope.TS.push(rowValues);
            }
        });

        if ($scope.p.Color ==undefined && $scope.p.Color==null ){
            $scope.p.Color = "#a0522d";
        }
        $scope.p.GH = $scope.GH;
        $scope.p.TS = $scope.TS;

        var favorite = [];
        var wrkgrp = [];
        $.each($("input[name='wd']:checked"), function () {
            favorite.push($(this).val());
        });
        $.each($("input[name='wg']:checked"), function () {
            wrkgrp.push($(this).val());
        });
        if (favorite.length == 0) {
            $.notify('Minimum one working day should be selected.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
        }
        else{

            $scope.WDays = favorite + ',';
            $scope.WGroups = wrkgrp + ',';
            $scope.WGroups=  $scope.WGroups.substring(0, $scope.WGroups.length - 1)
        //angular.forEach($scope.albums, function (album) {
        //    if (album.selected) $scope.albumNameArray.push(album.id);
        //});
        //angular.forEach($scope.albumNameArray, function (albumNameArray) {
        //    $scope.WDays =  $scope.albumNameArray+',';
        //});
        $scope.p.WorkingDays = $scope.WDays;
        $scope.p.WorkGroups = $scope.WGroups;
        if (dates.compare($scope.p.ActualStartDate, $scope.p.PlannedDate) == -1 ) {
            $.notify('Start Date Should not be less then Planned Date', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
        } else {
            $scope.p.TaskTypeId = 133320;

            service.post("/project/Defination/New",$scope.p).then(function (res) {
            if (res !== 'session expired') {
                if (res.Status === 'success') {
                    if (res.Value != 0 && res.Value != null) {
                        //$scope.p = {};
                        $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                        var url = window.location.href;
                        var mainurl = url + '/' + res.Value;
                        //window.location.relocate();
                        window.setTimeout(function () { window.location = mainurl; }, 2000);
                    }else
                    {
                        $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                      //  window.location.relocate();
                       $scope.UpdateTabsData();
                    //    ProjectTitles();
                    }
              
                
                } else {
                    $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }
            } else {
                $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
            }
        }).catch(function (err) { console.log('aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa ', err);});
        }
        }
 
      
    }

});

app.directive('datePicker', [function () {
    return {
        restrict: "A",
        require: "ngModel"
        , compile: function compile(tElement, tAttrs, transclude) {
            return {
                post: function postLink(scope, iElement, iAttrs, controller) {
                    var updateModel = function (dateText) {
                        scope.$apply(function () {
                            controller.$setViewValue(dateText);
                        });
                    };
                    $(iElement).datepicker({
                        format: 'mm/dd/yyyy',
                        autoclose: true
                    }).on('changeDate', function (dateText) {
                        updateModel(dateText.date);
                    });;
                }
            }
        },
    };
}]);

app.directive('colorPicker', [function () {
    return {
        restrict: "A",
        require: "ngModel"
        , compile: function compile(tElement, tAttrs, transclude) {
            return {
                post: function postLink(scope, iElement, iAttrs, controller) {
                    var updateModel = function (dateText) {
                        scope.$apply(function () {
                            controller.$setViewValue(dateText);
                        });
                    };
                    $(iElement).spectrum({
                        preferredFormat: "hex",
                        color: "#A0522D",
                        showInput: true,
                        className: "full-spectrum",
                        // showPaletteOnly: true,
                        // showPalette: true,
                        hideAfterPaletteSelect: true,
                        change: function () {
                            updateModel($(this).val());
                        },
                        //palette: [
                        //    ["#A0522D", "#CD5C5C", "#FF4500", "#008B8B"],
                        //     ["#B8860B", "#32CD32", "#FFD700", "#48D1CC"],
                        //     ["#87CEEB", "#FF69B4", "#CD5C5C", "#87CEFA"],
                        //     ["#6495ED", "#DC143C", "#FF8C00", "#C71585"],
                        //     ["#000000"]

                        //]
                    });
                }
            }
        },
    };
}]);

//app.directive('myMaxlength', ['$compile', '$log', function ($compile, $log) {
//        return {
//            restrict: "A",
//            require: "ngModel"
//             , compile: function (scope, elem, attrs, ctrl) {
//                attrs.$set("ngTrim", "false");
//                var maxlength = parseInt(attrs.myMaxlength, 10);
//                ctrl.$parsers.push(function (value) {
//                    $log.info("In parser function value = [" + value + "].");
//                    if (value.length > maxlength) {
//                        $log.info("The value [" + value + "] is too long!");
//                        value = value.substr(0, maxlength);
//                        ctrl.$setViewValue(value);
//                        ctrl.$render();
//                        $log.info("The value is now truncated as [" + value + "].");
//                    }
//                    return value;
//                });
//            }
//        };
//    }]);


