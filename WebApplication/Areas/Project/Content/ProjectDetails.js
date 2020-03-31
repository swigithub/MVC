function DateFormat(date, format) {
    return date.getDate() + '/' + (parseInt(date.getMonth()) + 1) + '/' + date.getFullYear();
}

var app = angular.module('calendarDemoApp', ['ui.calendar', 'ui.bootstrap', 'ngSanitize', 'ui.select']);// 'ui.select2'
app.service('service', function ($q, $compile, $http) {
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

app.controller('CalendarCtrl', function ($scope, $compile, uiCalendarConfig, service) {
    var vm = this;

    $scope.s = {};
    $scope.IsStage = false;
    $scope.ForecastedSites = 'visible';
    $scope.TaskTypeId = 0;
    $scope.Tasks = [];
    $scope.events = [];// calander scope
    $scope.GanttView = true;
    $scope.CalenderView = false;
    $scope.SelectedView = 'Gantt Chart';
    $scope.FormTypes = [];
    $scope.p = { ActualEndDate: '' };
    $scope.p.SortOrder = '';
    $scope.p.ProjectResources = [];
    $scope.multiple = {};
    $scope.multiple.AssignToId = [];

    $scope.select2Options = {
        width: '100%',
        allowClear: true,
        multiple: true,
        initSelection: function (element, callback) {
        }
    };

    service.post("/Project/Defination/Details?Id=" + ProjectId + '&Key=asdfnilwenvowe').then(function (res) {
        log(res);
        if (res != null) {
            $scope.s = res.Project;

            if ($scope.s != null) {
                if ($scope.s.ActualStartDate != null) {
                    $scope.s.ActualStartDate = moment($scope.s.ActualStartDate).format("DD MMM YYYY")
                }
                if (res.EstimateEndDate != null) {
                    $scope.s.EstimateEndDate = moment($scope.s.EstimateEndDate).format("DD MMM YYYY")
                } else {
                  //  $scope.s.EstimateEndDate = 'Continue';
                }
            }

            $scope.ProjectStatus = res.ProjectStatus;
         //   console.log("Status",$scope.ProjectStatus);
            if ($scope.ProjectStatus.filter(x=>x.DefinationName == "Planned").length > 0) {
                DefaultStatusId = $scope.ProjectStatus.filter(x=>x.DefinationName == "Planned")[0].DefinationId;
            }
            $scope.Scopes = res.UserScopes;
            $scope.Priorities = res.Priorities;
            DefaultPriorityId = $scope.Priorities[0].DefinationId;
            $scope.FormTypes = res.FormTypes;
            $scope.Users = res.Users;

            $scope.TaskTypes = res.TaskTypes;
            if ($scope.TaskTypes.length > 0) {
                $scope.setTaskTypeId($scope.TaskTypes[0].DefinationId, $scope.TaskTypes[0].KeyCode, $scope.TaskTypes[0].ColorCode, $scope.TaskTypes[0].DefinationName);
            }
        }
    });
    var param = { Filter: 'ByTaskTypeKeyCode', Value: 'PROJECT_MILESTONE', value2: ProjectId }
    service.post("/Project/Task/ToList", param).then(function (res) {
        $scope.PTasks = res;//.filter(x=>x.PTaskId == 0);
        $scope.Parents = res;
        // console.log($scope.PTasks);
    });
    //service.get("/Defnation/ToList?filter=byDefinationType&value=Task Types").then(function (res) {
    //    $scope.TaskTypes = res;
    //    if ($scope.TaskTypes.length > 0) {
    //        $scope.setTaskTypeId($scope.TaskTypes[0].DefinationId, $scope.TaskTypes[0].KeyCode, $scope.TaskTypes[0].ColorCode, $scope.TaskTypes[0].DefinationName);
    //    }
    //});

    //var param = { Filter: 'ByProjectId', Value: ProjectId }
    //service.post("/Project/Defination/ToSingle", param).then(function (res) {
    //    if (res != null) {
    //        $scope.s = res;
    //        if (res.EstimateStartDate != null) {
    //            $scope.s.EstimateStartDate = moment(res.EstimateStartDate).format("DD MMM YYYY")
    //        }

    //        if (res.EstimateEndDate != null) {
    //            $scope.s.EstimateEndDate = moment(res.EstimateEndDate).format("DD MMM YYYY")
    //        } else {
    //            $scope.s.EstimateEndDate = 'Continue';
    //        }
    //    }
    //});

    //service.get("/Defnation/ToList?filter=byDefinationType&value=Project Status").then(function (res) {
    //    $scope.Status = res;
    //});

    //service.get("/Defnation/ToList?filter=UserScopes&value=" + UserId).then(function (res) {
    //    $scope.Scopes = res;
    //});

    //service.get("/Defnation/ToList?filter=byDefinationType&value=Priority").then(function (res) {
    //    $scope.Priorities = res;
    //});

    //service.get("/Defnation/ToList?filter=byDefinationType&value=FormType").then(function (res) {
    //    $scope.FormTypes = res;
    //});
    //service.post("/User/ToList", { filter: 'ByProjectId', value: ProjectId }).then(function (res) {
    //    $scope.Users = res;
    //});

    $scope.fnProjectResource = function () {
        $scope.p.ProjectResources = [];
        Resources = [];
        for (var i = 0; i < $scope.multiple.AssignToId.length; i++) {
            var Id = $scope.multiple.AssignToId[i].UserId;
            $scope.p.ProjectResources.push({ AssignToId: Id });
            Resources.push({ resourceId: Id });
        }
    }

    $scope.View = function (filter) {
        $scope.GanttView = false;
        $scope.CalenderView = false;
        if (filter == 'gantt') {
            $scope.GanttView = true;
            $scope.SelectedView = 'Gantt Chart';
        }
        else if (filter == 'calander') {
            $scope.CalenderView = true;
            $scope.SelectedView = 'Calender';
            LoadEvents();
        }
    }

    $scope.GetTaskTypeById = function (Id) {
        var obj = {};
        $.each($scope.TaskTypes, function (i, v) {
            if (v.DefinationId == Id) {
                obj = v;
                return obj;
            }
        });
        return obj;
    }

    function FormTemplate(KeyCode, KeyId) {
        KeyCode = (KeyCode == 'PROJECT_MILESTONE') ? 'MILESTONE' : 'STAGE';
        var tempfrmtyp = $scope.GetFormTypesByKeyCode(KeyCode);

        $.ajax({
            url: '/project/Template/FormBuilderPartial?FormTypeId=' + tempfrmtyp.DefinationId + '&KeyId=' + KeyId,
            success: function (res) {
                $('#formBuilder').html(res);
                $('#btn-FormBuilderSubmit').hide();
            }
        });
    }


    function getMaxSortOrder(ProjectId, TaskTypeId) {
        $.ajax({
            url: '/Project/Task/getMaxSortOrder?filter=maxSortOrder&value=' + ProjectId + '&value2=' + TaskTypeId,
            success: function (rec) {
                $scope.p.SortOrder = rec[0].maxSortOrder;
            }
        });
    }



    $scope.GetFormTypesByKeyCode = function (key) {
        var obj = {};
        $.each($scope.FormTypes, function (i, v) {
            if (v.KeyCode == key) {
                obj = v;
                return obj;
            }
        });
        return obj;
    }

    $scope.GetUserById = function (Id) {
        var obj = {};
        $.each($scope.Users, function (i, v) {
            if (v.UserId == Id) {
                obj = v;
                return obj;
            }
        });
        return obj;
    }
    var TemporaryTasks = [];
    let TasksGroupingdynatree = function (Tasks) {
         var Child = [];
        for (var i = 0; i < Tasks.length; i++) {
            var Isparent = TemporaryTasks.filter(x=>x.PTaskId == Tasks[i].id)

            if (Isparent.length > 0) {
                var option = TasksGroupingdynatree(Isparent);
                if ($scope.p.PTaskId == Tasks[i].id) {
                    var Obj = {
                        title: Tasks[i].name, userid: Tasks[i].id,
                        children: option, select: true
                    };
                }
                else {
                    var Obj = {
                        title: Tasks[i].name, userid: Tasks[i].id,
                        children: option
                    };
                }
                Child.push(Obj);
            }
            else {
                if ($scope.p.PTaskId == Tasks[i].id) {
                    var Obj = {
                        title: Tasks[i].name, userid: Tasks[i].id,
                        select: true
                    };
                }
                else {
                    var Obj = {
                        title: Tasks[i].name, userid: Tasks[i].id,

                    };
                }
                Child.push(Obj);
            }
        }
        return Child;
    }
    $scope.Edit = function (TaskId) {
        $("#Taber1").click();
        $("#TASKSAVEButton").show();
        $("#KPISave").hide();
        $("#TASKPARAMETERSave").hide();

         res = MyTasks.filter(x=>x.id == TaskId)[0];
        var t = $scope.GetTaskTypeById(res.TaskTypeId);
        $scope.p = {};
        $scope.p = res;
        if ($scope.p.IsActive)
            $("#_IsActive").prop("checked", true)
        else
            $("#_IsActive").prop("checked", false)
        $scope.p.Title = res.name;
        $scope.p.Color = res.Color;
        $scope.p.CompletionPercent = res.progress;
        $scope.p.Description = res.description;
        $scope.p.IsEstimate = res.IsEstimate;
        $scope.p.IsActive = res.IsActive;
        if ($scope.p.PTaskId == "" || $scope.p.PTaskId == undefined)
            $scope.p.PTaskId = 0;
   Resources = res.assigs;
         var NewTask = TaskId.toString().indexOf('tmp_');
        if (NewTask == -1) {

            $scope.setTaskTypeId(t.DefinationId, t.KeyCode, t.ColorCode);
        }
        else {
            if ($scope.p.StatusId == undefined || $scope.p.StatusId == 0) {

                $scope.p.StatusId = $scope.ProjectStatus[0].DefinationId;
                 $scope.p.PriorityId = $scope.Priorities[0].DefinationId;
                $scope.p.TaskId = 0;
                $scope.p.PTaskId = 0;
                 }
            TaskId = 0;
            $scope.p.TaskTypeId = 0;
        }
           $scope.Header = "Edit Task";
        if (res.start != null && res.start != undefined) {
            $scope.p.EstimatedStartDate = moment(res.start).format("MM/DD/YYYY");
        }
        if (res.depends == "" || res.depends == 0 || res.depends == "0")
            $("#EstimatedStartDate").prop("disabled", false)
        else
            $("#EstimatedStartDate").prop("disabled",true)
        if (res.end != null && res.end != undefined) {
            $scope.p.EstimatedEndDate = moment(res.end).format("MM/DD/YYYY");
        }
        if (res.IsEstimate) {
            $scope.p.ActualStartDate = moment(res.start).format("MM/DD/YYYY");
        }
        else {
            $scope.p.ActualStartDate = moment(res.ActualStartDate).format("MM/DD/YYYY");
        }
        if (res.IsEstimate) {
            $scope.p.ActualEndDate = moment(res.start).format("MM/DD/YYYY");
        }
        else {
            $scope.p.ActualEndDate = moment(res.ActualEndDate).format("MM/DD/YYYY");
        }
        if (res.IsEstimate) {
            $("#ActualStartDate").attr('disabled', true);
            $("#ActualEndDate").attr('disabled', true);
        }
        else {
            $("#ActualStartDate").attr('disabled', false);
            $("#ActualEndDate").attr('disabled', false);
        }
        if (res.TargetDate != null && res.TargetDate != undefined) {
            $scope.p.TargetDate = moment(res.TargetDate).format("MM/DD/YYYY");
        }
        if (res.PlannedDate != null && res.PlannedDate != undefined) {
            $scope.p.PlannedDate = moment(res.PlannedDate).format("MM/DD/YYYY");
        }
        $(".datepicker").datepicker("option", "minDate", moment(res.EstimatedStartDate).add(1, "d").format("MM/DD/YYYY"));
        $("#Color").spectrum("set", $scope.p.Color);
        document.getElementById("_taskStatus").className.replace(/\bMyClass\b/, '');
        $("#_taskStatus")[0].className = '';
        $("#_taskStatus").addClass($scope.p.status);
        $("#_taskStatus").addClass("_statusLabel");
       if (NewTask == -1) {
        $.ajax({
            url: "/Project/KPI/GetKPI?Id=" + TaskId,
            type: "get",
            async: false,
        }).done(function (result) {
            $('#kpitablebody').html("");
            var Local1 = KPIFirstRow;
            var oldV = "[$].";
            var newV = "" + 0 + "]";
            var str1 = Local1.replace(RegExp(oldV, "gi"), newV);
            $('#kpitablebody').append(result);
            var str = ""
            $(".ExistedKpiType").each(function (entry, index, array) {
                if ($(this).find("option:selected").text() == "Formula") {
                    $(this).parent().parent().find(".demo-input-custom-labels").tokenInput(availableTags, {
                        theme: "facebook",
                        onAdd: function (item) {
                            var CurrentName = $(this).parent().parent().parent().find(".Kpi_Name").val();
                            if (item.name == CurrentName) {
                                $(this).tokenInput("remove", { name: CurrentName });
                                swal("Cannot Refer Itself in formula !");
                            }
                        }
                    });
                    $(this).parents('tr').next('tr').addClass('phisaljao');
                }
                else {
                    $(this).parents('tr').next('tr').removeClass('phisaljao');
                }

            });
            $(".newkpirow").hide();
            $('#TaskModal').modal('show');

            $("#KpiConfigurationform").show();
            $(".KPITAB").show();
            $(".TaskParameter").show();
            $("#TaskParameters").show();
            $("#TASKTAB").show();
            $(".TaskStagesTab").show();
        });
        }
        else {
            $("#KpiConfigurationform").hide();
            $(".KPITAB").hide();
            $("#TaskParameters").hide();
            $(".TaskParameter").hide();
            $("#TASKTAB").hide();
            $(".newkpirow").hide();
            $('#TaskModal').modal('show');
            $("#TASKPARAMETERSave").hide(); 
            $(".TaskStagesTab").hide();
}
        $("#TaskId").val(TaskId);
        FormTemplate($scope.Title, TaskId);

        $scope.multiple.AssignToId = [];
        for (var i = 0; i < res.assigs.length; i++) {
            var tmp = $scope.GetUserById(res.assigs[i].resourceId);
            $scope.multiple.AssignToId.push(tmp);
        }

       // if (IsParent)
         $scope.$apply();
        var IsParent = $scope.PTasks.filter(x=>x.PTaskId == TaskId);
        if (IsParent.length > 0) {
            $scope.IsParent = true;
        }
        else {
            $scope.IsParent = false;
        }
    }
    $('#_IsActive').change(function () {
        if ($(this).is(':unchecked')) {
            var CurrentIsActiveTask = MyTasks.filter(x=>x.id == $scope.p.id);
            if (CurrentIsActiveTask.length > 0 && CurrentIsActiveTask[0].CompletedSiteTasks > 0) {
                $("#_IsActive").prop('checked', true);
                swal(`${CurrentIsActiveTask[0].Title} has ${CurrentIsActiveTask[0].CompletedSiteTasks} Completed Site Tasks !`, "", "warning")
            }
        }
        else {

        }
    });
    $scope.GetMilestoneById = function (Id) {
        var obj = {};
        $.each($scope.PTasks, function (i, v) {
            if (v.TaskId == Id) {
                obj = v;
                return obj;
            }

        });
        return obj;
    }

    $scope.setTaskTypeId = function (value, KeyCode, ColorCode, Name) {

        if ($scope.p.TaskType) {
            var CurentType = $scope.TaskTypes.filter(x => x.KeyCode == 'PROJECT_STAGE');
            if (CurentType.length > 0) {
                //    $scope.P.TaskTypeId = CurentType[0].DefinationId;
                $scope.Title = CurentType[0].KeyCode;
                $scope.TaskTypeColor = CurentType[0].ColorCode;
                $scope.TaskTypeId = CurentType[0].DefinationId;
                KeyCode = 'PROJECT_STAGE';
                $scope.Header = "Create New Task";

            }
        }
        else {
            var CurentType = $scope.TaskTypes.filter(x => x.KeyCode == 'PROJECT_MILESTONE');
            if (CurentType.length > 0) {
                //      $scope.P.TaskTypeId = CurentType[0].DefinationId;
                $scope.Title = CurentType[0].KeyCode;
                $scope.TaskTypeColor = CurentType[0].ColorCode;
                $scope.TaskTypeId = CurentType[0].DefinationId;
                KeyCode = 'PROJECT_MILESTONE';
                $scope.Header = "Create New Task";
            }
        }
        $("#id_" + value).css("background", ColorCode);
   //    $scope.LoadEvents();
      //  return false;
        //$scope.Title = KeyCode;
        //$scope.TaskTypeColor = ColorCode;
        //$scope.TaskTypeId = value;

        //    getMaxSortOrder(ProjectId, $scope.TaskTypeId);


        //   $scope.StagesDisable = false;
     

        //if (KeyCode == 'PROJECT_STAGE') {
        //    $scope.IsStage = true;
        //    $scope.ForecastedSites = 'hidden';
        //    // $scope.Predecessors.length = 0;
        //    //var param = { Filter: 'ByTaskTypeKeyCode', Value: 'PROJECT_MILESTONE', value2: ProjectId }
        //    //service.post("/Project/Task/ToList", param).then(function (res) {
        //    //    $scope.PTasks = res;//.filter(x=>x.PTaskId == 0);
        //    //   // console.log($scope.PTasks);
        //    //});
        //    //$scope.p.ActualStartDate = null;
        //    //$scope.p.ActualEndDate = null;
        //} else {
        //    // $scope.IsStage = false;
        //    //  $scope.PTasks = [];
        //    $scope.ForecastedSites = 'visible';
        //    //  $scope.LoadPredecessors(value);
        //}
     //   $scope.LoadEvents();

        //FormTemplate($scope.Title, 0);
    }

    service.get("/Defnation/ToList?filter=byDefinationType&value=Task Types").then(function (res) {
        $scope.TaskTypes = res;

        if ($scope.TaskTypes.length > 0) {
            $scope.setTaskTypeId($scope.TaskTypes[0].DefinationId, $scope.TaskTypes[0].KeyCode, $scope.TaskTypes[0].ColorCode, $scope.TaskTypes[0].DefinationName);
        }
    });
    let countdown = function (value) {
        let Pre = $scope.PTasks.filter(x=>x.TaskId == value);
        if (Pre.length > 0) {
            let IsPre = $scope.PTasks.filter(x=>x.TaskId == Pre[0].PredecessorId);
            if (IsPre.length > 0) {
                countdown(IsPre[0].TaskId);

            }
            else {
                if (value == $scope.p.PTaskId) {
                    //    $(".datepicker").datepicker("option", "minDate", new Date(1990, 1, 1));
                }
                else {

                    $(".datepicker").datepicker("option", "minDate", moment(Pre[0].ActualEndDate).add(1, "d").format("MM/DD/YYYY"));
                }

            }
        }
        else {
            // $(".datepicker").datepicker("option", "minDate", new Date(1990, 1, 1));
        }
    }

    $scope.LoadPredecessors = function (value) {
        var param = { Filter: 'ByTaskTypeId', Value: value, value2: ProjectId }
        service.post("/Project/Task/ToList", param).then(function (res) {
            $scope.Predecessors = res;//.filter(x=>x.TaskId != value && x.PTaskId == $scope.p.PTaskId && x.PredecessorId != value);

        });
    }

    $scope.LoadPredecessorsStages = function (Value, Self) {
        $scope.Predecessors = MyTasks.filter(x=>x.PTaskId == Value && x.id != Self)
    }
  
    $scope.NewTaskEvent = function () {
        NewThis = true;
        $(".emptyRow")[0].click();
         $(".gdfTable").find(".edit").last().click();
       // return false;
        //var param = { Filter: 'ByTaskTypeKeyCode', Value: 'PROJECT_MILESTONE', value2: ProjectId }
        //service.post("/Project/Task/ToList", param).then(function (res) {
        //    $scope.PTasks = res;//.filter(x=>x.PTaskId == 0);
        //    $scope.Parents = res;
        //    // console.log($scope.PTasks);
        //});
        //$scope.Predecessors = [];
        //$scope.Header = "Create New Task";
        //$scope.p = { ActualEndDate: '' };
        //$scope.p.IsEstimate = true;
        //$scope.p.IsActive = true;
        //$scope.p.StatusId = $scope.ProjectStatus[0].DefinationId;
        //$scope.p.PriorityId = $scope.Priorities[0].DefinationId;
        //$scope.p.CompletionPercent = 0;
        //FormTemplate($scope.Title, 0);
        //$scope.multiple.AssignToId = [];
        //$scope.p.ActualStartDate = $scope.s.EstimateStartDate;
        //$scope.p.ActualEndDate = $scope.s.EstimateEndDate;
        //getMaxSortOrder(ProjectId, $scope.TaskTypeId);
        //$("#KpiConfigurationform").hide();
        //$(".KPITAB").hide();
        //$(".datepicker").datepicker("option", "minDate", new Date(1990, 1, 1));
    }
    $scope.DateChangeCapture = function (type, current) {
    }
    $scope.StartDateChange = function () {
       
        //$scope.p.ActualStartDate = $("#ActualStartDate").val();
        //var Dater = moment($scope.p.ActualStartDate).add(1, "d").format("MM/DD/YYYY");
        //$("#ActualEndDate").datepicker("option", "minDate", Dater);
    }
    $scope.EndDateChange = function () {
        
       // $(".datepicker").datepicker("option", "minDate", moment(Pre[0].ActualEndDate).add(1, "d").format("MM/DD/YYYY"));
    }
    $scope.Save = function (ef) {
var res = [];
        for (var i = 0; i < MyTasks.length; i++) {
            var TempRes = [];
            for (var m = 0; m < MyTasks[i].assigs.length; m++) {
                TempRes.push({ AssignToId: MyTasks[i].assigs[m].resourceId });
            }

            if (MyTasks[i].Color == "" || MyTasks[i].Color == null || MyTasks[i].Color == undefined) {
                MyTasks[i].Color = "#000000";
            }
             res.push({
                TaskId: MyTasks[i].id, PTaskId: MyTasks[i].PTaskId, ProjectId: ProjectId,
                TaskTypeColor: "", StatusId: MyTasks[i].StatusId, Status: MyTasks[i].status, StatusColor: MyTasks[i].StatusColor, PredecessorId: MyTasks[i].depends, PriorityId: MyTasks[i].PriorityId, Title: MyTasks[i].name,
                EstimatedStartDate: moment(MyTasks[i].start).format("MM/DD/YYYY"), ActualEndDate: moment(MyTasks[i].ActualEndDate).format("MM/DD/YYYY"),
                ActualStartDate: moment(MyTasks[i].ActualStartDate).format("MM/DD/YYYY"), PTaskId: MyTasks[i].PTaskId,
                EstimatedEndDate: moment(MyTasks[i].end).format("MM/DD/YYYY"),
                PlannedDate: moment(MyTasks[i].PlannedDate).format("MM/DD/YYYY"), IsMilestone: MyTasks[i].IsMilestone,
                TargetDate: moment(MyTasks[i].TargetDate).format("MM/DD/YYYY"), Description: MyTasks[i].description, CompletionPercent: MyTasks[i].progress, BudgetCost: MyTasks[i].BudgetCost, ActualCost: MyTasks[i].ActualCost, MapCode: MyTasks[i].MapCode,
                IsStartMilestone: MyTasks[i].startIsMilestone, IsEndMilestone: MyTasks[i].endIsMilestone, SortOrder: MyTasks[i].SortOrder, maxSortOrder: 0, ProjectResources: TempRes, TaskTypeId: MyTasks[i].TaskTypeId, ForecastedSites: MyTasks[i].ForeCastedSites, MapColumn: MyTasks[i].MapColumn
                , Color: MyTasks[i].Color, id: MyTasks[i].id, Level: MyTasks[i].level, IsEstimate: MyTasks[i].IsEstimate, Duration: MyTasks[i].duration, IsActive:MyTasks[i].IsActive
            });
        }
      
        var prj = ge.saveProject();
        if (localStorage) {
            localStorage.setObject("teamworkGantDemo", prj);
        }
        service.post("/Project/Task/new", res).then(function (res) {

          //  $scope.LoadEvents();
            swal("Timeline Saved Successfully !", "", "success");
            clearGantt();
            project = loadFromLocalStorage();
            ge.loadProject(project);
            $('#TaskModal').modal('hide');
            $scope.p = {};

        });
        // }
        //else {
        //    //$scope.TaskForm.$error.required.forEach(function (e) {
        //    //    e.$$element.css("border", "2px solid red");
        //    //});
        //  }
        //}
    }
    $scope.TaskParameterSave = function (Value) {
        let RequiredFields = true;
        $('#formBuilder').find('input').each(function () {
            if (!$(this).prop('required')) {
               // console.log("NR");
            } else {
                //console.log("IR");
                if ($(this).val() == "") {
                    RequiredFields = false
                }
            }
        });
        $('#formBuilder').find('select').each(function () {
            if (!$(this).prop('required')) {

            } else {
                if ($(this).val() == "" || $(this).val() == "-1") {
                    RequiredFields = false
                                          }
               
            }
        });
       $('#definationTypeId').val($scope.p.id);
        if (RequiredFields == true) {
                $.ajax({
            url: '/Project/Template/FormBuilder',
            type: 'post',
            data: $('#formBuilder').serialize(),
            success: function (frm) {

                        if (frm.Status == true) {
                            var definationType = $('#definationTypeId').val();
                            if (definationType != null) {
                                $.ajax({
            url: "/Project/Template/SavedFormInfo/",
            type: "post",
            data: {
            FormId: definationType,
        },
            async: false,
        }).done(function (result) {
                                    var GeneratedForm = result.Form;
                                    $('#FormBuilderFields').html(GeneratedForm);
                                    swal("Success !", "", "success");
        });
        }
        else {
                                swal("Success !", "", "success");
        }
        }
        else {
                            swal("Error !", "Error occured ", "error");
        }
        },
            error: function () {
                        swal("Error !", "Error occured", "error");
        }
        });
        }
        else {
            //  swal("Fields with <i style='color:red' >*</i> are Compulsary !", "", "error");
            swal({
                title: "Fields with * are Compulsary !",
                type: "warning",
                border: "3px solid red;"
            });
}
    }



    $scope.LoadEvents = function () {
        var param = { Filter: 'ByTaskTypeId', Value: $scope.TaskTypeId, value2: ProjectId }
        service.post("/Project/Task/ToList", param).then(function (res) {
            $scope.Tasks = res;
            $scope.events.length = 0;
            if ($scope.Tasks.length > 0) {
                for (var i = 0; i < $scope.Tasks.length; i++) {
                    var StartDate = new Date(parseInt($scope.Tasks[i].EstimatedStartDate.substr(6)));
                    StartDate = DateFormat(StartDate, '');
                    $scope.Tasks[i].ActualEndDate = ($scope.Tasks[i].ActualEndDate == null) ? $scope.Tasks[i].EstimatedStartDate : $scope.Tasks[i].ActualEndDate;
                    var EndDate = new Date(parseInt($scope.Tasks[i].ActualEndDate.substr(6)));
                    EndDate = DateFormat(EndDate, '');

                    $scope.events.push({ title: $scope.Tasks[i].Title + '\n From: ' + StartDate + ' To: ' + EndDate, start: $scope.Tasks[i].EstimatedStartDate, end: $scope.Tasks[i].ActualEndDate, sortOrder: $scope.Tasks[i].SortOrder });
                }

            }
        });
    }

    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    $scope.changeTo = 'Hungarian';
    /* event source that pulls from google.com */
    $scope.eventSource = {
        //url: "http://www.google.com/calendar/feeds/usa__en%40holiday.calendar.google.com/public/basic",
        //className: 'gcal-event',           // an option!
        //currentTimezone: 'America/Chicago' // an option!
    };

    //$scope.events.push({ title: 'All s Day Event', start: new Date(y, m, 1) });
    /* event source that calls a function on every view switch */
    $scope.eventsF = function (start, end, timezone, callback) {
        var s = new Date(start).getTime() / 1000;
        var e = new Date(end).getTime() / 1000;
        var m = new Date(start).getMonth();
        var events = [{ title: 'Feed Me ' + m, start: s + (50000), end: s + (100000), allDay: false, className: ['customFeed'] }];
        callback($scope.events);
    };

    $scope.calEventsExt = {
        color: '#f00',
        textColor: 'yellow',
        events: [
            { type: 'party', title: 'Lunch', start: new Date(y, m, d, 12, 0), end: new Date(y, m, d, 14, 0), allDay: false },
            { type: 'party', title: 'Lunch 2', start: new Date(y, m, d, 12, 0), end: new Date(y, m, d, 14, 0), allDay: false },
            { type: 'party', title: 'Click for Google', start: new Date(y, m, 28), end: new Date(y, m, 29), url: 'http://google.com/' }
        ]
    };
    /* alert on eventClick */
    $scope.alertOnEventClick = function (date, jsEvent, view) {
        $scope.alertMessage = (date.title + ' was clicked ');
    };
    /* alert on Drop */
    $scope.alertOnDrop = function (event, delta, revertFunc, jsEvent, ui, view) {
        //   alert('Event Droped to make dayDelta ' + delta);
        $scope.alertMessage = ('Event Droped to make dayDelta ' + delta);
    };
    /* alert on Resize */
    $scope.alertOnResize = function (event, delta, revertFunc, jsEvent, ui, view) {
        $scope.alertMessage = ('Event Resized to make dayDelta ' + delta);
    };
    /* add and removes an event source of choice */
    $scope.addRemoveEventSource = function (sources, source) {
        var canAdd = 0;
        angular.forEach(sources, function (value, key) {
            if (sources[key] === source) {
                sources.splice(key, 1);
                canAdd = 1;
            }
        });
        if (canAdd === 0) {
            sources.push(source);
        }
    };
    /* add custom event*/
    $scope.addEvent = function () {
        $scope.events.push({
            title: 'Open Sesame',
            start: new Date(y, m, 28),
            end: new Date(y, m, 29),
            className: ['openSesame']
        });
    };
    /* remove event */
    $scope.remove = function (index) {
        $scope.events.splice(index, 1);
    };
    /* Change View */
    $scope.changeView = function (view, calendar) {
        uiCalendarConfig.calendars[calendar].fullCalendar('changeView', view);
    };
    /* Change View */
    $scope.renderCalender = function (calendar) {
        if (uiCalendarConfig.calendars[calendar]) {
            uiCalendarConfig.calendars[calendar].fullCalendar('render');
        }
    };
    /* Render Tooltip */
    $scope.eventRender = function (event, element, view) {
        element.attr({
            'tooltip': event.title,
            'tooltip-append-to-body': true
        });
        element.css('background-color', $scope.TaskTypeColor);
        $compile(element)($scope);
    };
    /* config object */
    $scope.uiConfig = {
        calendar: {
            height: 739,
            // editable: true,
            header: {
                left: 'title',
                center: '',
                right: 'month, agendaWeek, agendaDay,today prev,next,'
            },
            eventClick: $scope.alertOnEventClick,
            eventDrop: $scope.alertOnDrop,
            eventResize: $scope.alertOnResize,
            eventRender: $scope.eventRender
        }
    };

    $scope.changeLang = function () {
        if ($scope.changeTo === 'Hungarian') {
            $scope.uiConfig.calendar.dayNames = ["Vasárnap", "Hétfő", "Kedd", "Szerda", "Csütörtök", "Péntek", "Szombat"];
            $scope.uiConfig.calendar.dayNamesShort = ["Vas", "Hét", "Kedd", "Sze", "Csüt", "Pén", "Szo"];
            $scope.changeTo = 'English';
        } else {
            $scope.uiConfig.calendar.dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            $scope.uiConfig.calendar.dayNamesShort = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
            $scope.changeTo = 'Hungarian';
        }
    };
    /* event sources array*/
    $scope.eventSources = [$scope.events, $scope.eventSource, $scope.eventsF];

    $scope.eventSources2 = [$scope.calEventsExt, $scope.eventsF, $scope.events];


});
app.filter("dateFilter", function ($filter) {
    return function (item) {
        if (item != null) {
            var parsedDate = new Date(parseInt(item.substr(6)));
            return $filter('date')(parsedDate, 'yyyy-MM-dd');
        }
        return "";
    };
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
                         autoclose: true,
                     }).on('changeDate', function (dateText) {
                         updateModel(dateText.date);
                     });
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
                        hideAfterPaletteSelect: true,
                        change: function () {
                            updateModel($(this).val());
                        }
                    });
                }
            }
        },
    };
}]);