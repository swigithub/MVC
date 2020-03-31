




var ge;



function loadWithUpdates() {
   
   
    var ret;
  

    ret = {
        "tasks":MyTasks,
        "selectedRow": 2, "deletedTaskIds": [],
        "resources": [],
        "roles": [
        { "id": "tmp_1", "name": "Project Manager" },
        { "id": "tmp_2", "name": "Worker" },
        { "id": "tmp_3", "name": "Stakeholder" },
        { "id": "tmp_4", "name": "Customer" }
        ], "canWrite": true, "canDelete": false, "canWriteOnParent": true, "zoom": "w3"
       , "canAdd": true,
        "canInOutdent": true,
        "canMoveUpDown": true,
        "canSeePopEdit": true,
        "canSeeFullEdit": true,
        "canSeeDep": true,
        "canSeeCriticalPath": true,
        "canAddIssue": false,
        "cannotCloseTaskIfIssueOpen": false
    }

    $.ajax({
        type: 'post',
        url: '/User/ToList',
        async: false,
        data: { filter: 'ByProjectId', value: ProjectId },
        success: function (res) {

            for (var i = 0; i < res.length; i++) {
                ret.resources.push({ "id": res[i].UserId, "name": res[i].FirstName + ' ' + res[i].LastName });
            }

        }
    });

   
    return ret;
}
function loadFromLocalStorage() {
    var ret;
   
    if (!ret || !ret.tasks || ret.tasks.length == 0) {

        ret = {
            "tasks": [],
            "selectedRow": 2, "deletedTaskIds": [],
            "resources": [],
            "roles": [
            { "id": "tmp_1", "name": "Project Manager" },
            { "id": "tmp_2", "name": "Worker" },
            { "id": "tmp_3", "name": "Stakeholder" },
            { "id": "tmp_4", "name": "Customer" }
            ], "canWrite": true, "canDelete": true, "canWriteOnParent": true, "zoom": "w3"
            , "canAdd": true,
            "canInOutdent": true,
            "canMoveUpDown": true,
            "canSeePopEdit": true,
            "canSeeFullEdit": true,
            "canSeeDep": true,
            "canSeeCriticalPath": true,
            "canAddIssue": false,
            "cannotCloseTaskIfIssueOpen": false
        }

        $.ajax({
            type: 'post',
            url: '/User/ToList',
            async: false,
            data: { filter: 'ByProjectId', value: ProjectId },
            success: function (res) {

                for (var i = 0; i < res.length; i++) {
                    ret.resources.push({ "id": res[i].UserId, "name": res[i].FirstName + ' ' + res[i].LastName });
                }

            }
        });

        $.ajax({
            type: 'post',
            url: '/Project/Task/ToList',
            data: { Filter: 'ByTaskTypeKeyCode', Value: 'PROJECT_MILESTONE', value2: ProjectId, Resources: true },
            async: false,
            success: function (res) {
                
                for (var i = 0; i < res.length; i++) {
                    var IsChild = false;
                    var depends = "";
                    var tempStages = res.filter(x=>x.PTaskId == res[i].TaskId) //fnStages(res[i].TaskId);
                    if (tempStages.length > 0) {
                        IsChild = true;
                    }
                    if (res[i].PredecessorId > 0) {
                        depends = res[i].PredecessorId + '';
                    }
                     res[i].ActualEndDate = (res[i].ActualEndDate == null) ? res[i].ActualStartDate : res[i].ActualEndDate;
                    //var StartDate = parseFloat(res[i].ActualStartDate.substr(6));
                    //var EndDate = parseFloat(res[i].ActualEndDate.substr(6));
                    var StartDate = (moment(res[i].ActualStartDate));
                    var EndDate = (moment(res[i].ActualEndDate));
                    var EstimatedStartDate = (moment(res[i].EstimatedStartDate));
                    var EstimatedEndDate = (moment(res[i].EstimatedEndDate))//.add('days',1);
                    var TargetDate = moment(res[i].TargetDate).format("MM/DD/YYYY");//(moment.utc(res[i].TargetDate));
                    var PlannedDate = moment(res[i].PlannedDate).format("MM/DD/YYYY");
                   
                    var TempRes = [];
                    for (var m = 0; m < res[i].ProjectResources.length; m++) {
                        TempRes.push({
                            "resourceId": res[i].ProjectResources[m].AssignToId, "id": "tmp_1345560373990" + m,
                            "roleId": "tmp_1", "effort": 36000000
                        });
                    }

                    ret.tasks.push({
                        "id": res[i].TaskId, "name": res[i].Title, "progress": res[i].CompletionPercent,
                        "ActualStartDate":StartDate,"ActualEndDate":EndDate,
                        "progressByWorklog": false, "relevance": 0, "type": "", "typeId": "", "description": res[i].Description,
                        "code": "", "level": res[i].Level, "status": res[i].Status, "depends": depends, "canWrite": true,
                        "start": EstimatedStartDate, "EstimatedStartDate": EstimatedStartDate, "EstimatedEndDate": EstimatedEndDate,
                        "TargetDate": TargetDate, "PlannedDate": PlannedDate, "StatusId": res[i].StatusId, "ProjectId": res[i].ProjectId,
                        "PriorityId": res[i].PriorityId, "CompletionPercent": res[i].CompletionPercent, "ActualCost": res[i].ActualCost, "BudgetCost": res[i].BudgetCost,
                        "ForecastedSites": res[i].ForecastedSites, "MapColumn": res[i].MapColumn, "MapCode": res[i].MapCode,
                        "duration": res[i].Duration, "end": EstimatedEndDate, "startIsMilestone": res[i].IsStartMilestone, "IsMilestone": res[i].IsMilestone,
                        "endIsMilestone": res[i].IsEndMilestone, "collapsed": false, "assigs": TempRes, "hasChild": IsChild, "StatusColor": "",
                        "SortOrder": res[i].SortOrder, "TaskId": res[i].TaskId, "PTaskId": res[i].PTaskId, "TaskTypeId": res[i].TaskTypeId, "Color": res[i].Color,
                        "IsEstimate": res[i].IsEstimate, "IsActive": res[i].IsActive, CompletedSiteTasks: res[i].CompletedSiteTasks
                    });

                 
                }

                console.log(ret.tasks);
                MyTasks = [];
                MyTasks = ret.tasks;
            },
            error: function (err) { }
        });

    }
     

    //ret = {
    //    "tasks": [
    //      { "id": -1, "name": "Gantt editoruytu", "progress": 0, "progressByWorklog": false, "relevance": 0, "type": "", "typeId": "", "description": "", "code": "", "level": 0, "status": "STATUS_ACTIVE", "depends": "", "canWrite": true, "start": 1396994400000, "duration": 20, "end": 1399586399999, "startIsMilestone": false, "endIsMilestone": false, "collapsed": false, "assigs": [], "hasChild": true },
    //      { "id": -2, "name": "coding", "progress": 0, "progressByWorklog": false, "relevance": 0, "type": "", "typeId": "", "description": "", "code": "", "level": 1, "status": "STATUS_ACTIVE", "depends": "", "canWrite": true, "start": 1396994400000, "duration": 10, "end": 1398203999999, "startIsMilestone": false, "endIsMilestone": false, "collapsed": false, "assigs": [], "hasChild": true },
    //      { "id": -3, "name": "gantt part", "progress": 0, "progressByWorklog": false, "relevance": 0, "type": "", "typeId": "", "description": "", "code": "", "level": 2, "status": "STATUS_ACTIVE", "depends": "", "canWrite": true, "start": 1396994400000, "duration": 2, "end": 1397167199999, "startIsMilestone": false, "endIsMilestone": false, "collapsed": false, "assigs": [], "hasChild": false },
    //      { "id": -4, "name": "editor part", "progress": 0, "progressByWorklog": false, "relevance": 0, "type": "", "typeId": "", "description": "", "code": "", "level": 2, "status": "STATUS_SUSPENDED", "depends": "3", "canWrite": true, "start": 1397167200000, "duration": 4, "end": 1397685599999, "startIsMilestone": false, "endIsMilestone": false, "collapsed": false, "assigs": [], "hasChild": false },
    //      { "id": -5, "name": "testing", "progress": 0, "progressByWorklog": false, "relevance": 0, "type": "", "typeId": "", "description": "", "code": "", "level": 1, "status": "STATUS_SUSPENDED", "depends": "2:5", "canWrite": true, "start": 1398981600000, "duration": 5, "end": 1399586399999, "startIsMilestone": false, "endIsMilestone": false, "collapsed": false, "assigs": [], "hasChild": true },
    //      { "id": -6, "name": "test on safari", "progress": 0, "progressByWorklog": false, "relevance": 0, "type": "", "typeId": "", "description": "", "code": "", "level": 2, "status": "STATUS_SUSPENDED", "depends": "", "canWrite": true, "start": 1398981600000, "duration": 2, "end": 1399327199999, "startIsMilestone": false, "endIsMilestone": false, "collapsed": false, "assigs": [], "hasChild": false },
    //      { "id": -7, "name": "test on ie", "progress": 0, "progressByWorklog": false, "relevance": 0, "type": "", "typeId": "", "description": "", "code": "", "level": 2, "status": "STATUS_SUSPENDED", "depends": "6", "canWrite": true, "start": 1399327200000, "duration": 3, "end": 1399586399999, "startIsMilestone": false, "endIsMilestone": false, "collapsed": false, "assigs": [], "hasChild": false },
    //      { "id": -8, "name": "test on chrome", "progress": 0, "progressByWorklog": false, "relevance": 0, "type": "", "typeId": "", "description": "", "code": "", "level": 2, "status": "STATUS_SUSPENDED", "depends": "6", "canWrite": true, "start": 1399327200000, "duration": 2, "end": 1399499999999, "startIsMilestone": false, "endIsMilestone": false, "collapsed": false, "assigs": [], "hasChild": false }
    //    ], "selectedRow": 2, "deletedTaskIds": [],
    //    "resources": [
    //    { "id": "tmp_1", "name": "Resource 1" },
    //    { "id": "tmp_2", "name": "Resource 2" },
    //    { "id": "tmp_3", "name": "Resource 3" },
    //    { "id": "tmp_4", "name": "Resource 4" }
    //    ],
    //    "roles": [
    //    { "id": "tmp_1", "name": "Project Manager" },
    //    { "id": "tmp_2", "name": "Worker" },
    //    { "id": "tmp_3", "name": "Stakeholder" },
    //    { "id": "tmp_4", "name": "Customer" }
    //    ], "canWrite": true, "canDelete": true, "canWriteOnParent": true, canAdd: true
    //}
    return ret;
}
var project = loadFromLocalStorage();
$(function () {
    var canWrite = true //false; //this is the default for test purposes

    // here starts gantt initialization
    ge = new GanttMaster();
    // ge.set100OnClose=true;

    ge.init($("#workSpace"));
    loadI18n(); //overwrite with localized ones

    //in order to force compute the best-fitting zoom level
    delete ge.gantt.zoom;





    if (!project.canWrite)
        $(".ganttButtonBar button.requireWrite").attr("disabled", "false");

    ge.loadProject(project);
    ge.checkpoint(); //empty the undo stack

    ge.editor.element.oneTime(100, "cl", function () { $(this).find("tr.emptyRow:first").click() });
});



function loadGanttFromServer(taskId, callback) {

    //this is a simulation: load data from the local storage if you have already played with the demo or a textarea with starting demo data
    loadFromLocalStorage();

    //this is the real implementation
    /*
    //var taskId = $("#taskSelector").val();
    var prof = new Profiler("loadServerSide");
    prof.reset();
  
    $.getJSON("ganttAjaxController.jsp", {CM:"LOADPROJECT",taskId:taskId}, function(response) {
      //console.debug(response);
      if (response.ok) {
        prof.stop();
  
        ge.loadProject(response.project);
        ge.checkpoint(); //empty the undo stack
  
        if (typeof(callback)=="function") {
          callback(response);
        }
      } else {
        jsonErrorHandling(response);
      }
    });
    */
}


function saveGanttOnServer() {

    //this is a simulation: save data to the local storage or to the textarea
    saveInLocalStorage();

    /*
    var prj = ge.saveProject();
  
    delete prj.resources;
    delete prj.roles;
  
    var prof = new Profiler("saveServerSide");
    prof.reset();
  
    if (ge.deletedTaskIds.length>0) {
      if (!confirm("TASK_THAT_WILL_BE_REMOVED\n"+ge.deletedTaskIds.length)) {
        return;
      }
    }
  
    $.ajax("ganttAjaxController.jsp", {
      dataType:"json",
      data: {CM:"SVPROJECT",prj:JSON.stringify(prj)},
      type:"POST",
  
      success: function(response) {
        if (response.ok) {
          prof.stop();
          if (response.project) {
            ge.loadProject(response.project); //must reload as "tmp_" ids are now the good ones
          } else {
            ge.reset();
          }
        } else {
          var errMsg="Errors saving project\n";
          if (response.message) {
            errMsg=errMsg+response.message+"\n";
          }
  
          if (response.errorMessages.length) {
            errMsg += response.errorMessages.join("\n");
          }
  
          alert(errMsg);
        }
      }
  
    });
    */
}

function newProject() {
    clearGantt();
}


//-------------------------------------------  Create some demo data ------------------------------------------------------
function setRoles() {
    ge.roles = [
      {
          id: "tmp_1",
          name: "Project Manager"
      },
      {
          id: "tmp_2",
          name: "Worker"
      },
      {
          id: "tmp_3",
          name: "Stakeholder"
      },
      {
          id: "tmp_4",
          name: "Customer"
      }
    ];
}

function setResource() {
    var res = [];
    for (var i = 1; i <= 10; i++) {
        res.push({ id: "tmp_" + i, name: "Resource " + i });
    }
    ge.resources = res;
}


function editResources() {

}

function clearGantt() {
    ge.reset();
}

function loadI18n() {
    GanttMaster.messages = {
        "CANNOT_WRITE": "CANNOT_WRITE",
        "CHANGE_OUT_OF_SCOPE": "NO_RIGHTS_FOR_UPDATE_PARENTS_OUT_OF_EDITOR_SCOPE",
        "START_IS_MILESTONE": "START_IS_MILESTONE",
        "END_IS_MILESTONE": "END_IS_MILESTONE",
        "TASK_HAS_CONSTRAINTS": "TASK_HAS_CONSTRAINTS",
        "GANTT_ERROR_DEPENDS_ON_OPEN_TASK": "GANTT_ERROR_DEPENDS_ON_OPEN_TASK",
        "GANTT_ERROR_DESCENDANT_OF_CLOSED_TASK": "GANTT_ERROR_DESCENDANT_OF_CLOSED_TASK",
        "TASK_HAS_EXTERNAL_DEPS": "TASK_HAS_EXTERNAL_DEPS",
        "GANTT_ERROR_LOADING_DATA_TASK_REMOVED": "GANTT_ERROR_LOADING_DATA_TASK_REMOVED",
        "ERROR_SETTING_DATES": "ERROR_SETTING_DATES",
        "CIRCULAR_REFERENCE": "CIRCULAR_REFERENCE",
        "CANNOT_DEPENDS_ON_ANCESTORS": "CANNOT_DEPENDS_ON_ANCESTORS",
        "CANNOT_DEPENDS_ON_DESCENDANTS": "CANNOT_DEPENDS_ON_DESCENDANTS",
        "INVALID_DATE_FORMAT": "INVALID_DATE_FORMAT",
        "TASK_MOVE_INCONSISTENT_LEVEL": "TASK_MOVE_INCONSISTENT_LEVEL",

        "GANTT_QUARTER_SHORT": "trim.",
        "GANTT_SEMESTER_SHORT": "sem."
    };
}



//-------------------------------------------  Get project file as JSON (used for migrate project from gantt to Teamwork) ------------------------------------------------------
function getFile() {
    $("#gimBaPrj").val(JSON.stringify(ge.saveProject()));
    $("#gimmeBack").submit();
    $("#gimBaPrj").val("");

    /*  var uriContent = "data:text/html;charset=utf-8," + encodeURIComponent(JSON.stringify(prj));
     neww=window.open(uriContent,"dl");*/
}

function GetDays(startDate, endDate) {
    var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
    var firstDate = new Date(startDate);// new Date(2008, 01, 12);
    var secondDate = new Date(endDate);// new Date(2008, 01, 22);
    var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
    return diffDays;
}

var Stages = [];
function fnStages(PTaskId) {
    var tempStages = [];
    $.each(Stages, function (i, v) {

        if (v.PTaskId == PTaskId) {
            tempStages.push(v);
        }
    });
    return tempStages;

}


//


function saveInLocalStorage() {

    var prj = ge.saveProject();
    if (localStorage) {
        localStorage.setObject("teamworkGantDemo", prj);
    }
}


//-------------------------------------------  Open a black popup for managing resources. This is only an axample of implementation (usually resources come from server) ------------------------------------------------------
function editResources() {

    //make resource editor

    var resourceEditor = $.JST.createFromTemplate({}, "RESOURCE_EDITOR");
    var resTbl = resourceEditor.find("#resourcesTable");

    for (var i = 0; i < ge.resources.length; i++) {
        var res = ge.resources[i];
        resTbl.append($.JST.createFromTemplate(res, "RESOURCE_ROW"))
    }


    //bind add resource
    resourceEditor.find("#addResource").click(function () {
        resTbl.append($.JST.createFromTemplate({ id: "new", name: "resource" }, "RESOURCE_ROW"))
    });

    //bind save event
    resourceEditor.find("#resSaveButton").click(function () {
        var newRes = [];
        //find for deleted res
        for (var i = 0; i < ge.resources.length; i++) {
            var res = ge.resources[i];
            var row = resourceEditor.find("[resId=" + res.id + "]");
            if (row.length > 0) {
                //if still there save it
                var name = row.find("input[name]").val();
                if (name && name != "")
                    res.name = name;
                newRes.push(res);
            } else {
                //remove assignments
                for (var j = 0; j < ge.tasks.length; j++) {
                    var task = ge.tasks[j];
                    var newAss = [];
                    for (var k = 0; k < task.assigs.length; k++) {
                        var ass = task.assigs[k];
                        if (ass.resourceId != res.id)
                            newAss.push(ass);
                    }
                    task.assigs = newAss;
                }
            }
        }

        //loop on new rows
        var cnt = 0
        resourceEditor.find("[resId=new]").each(function () {
            cnt++;
            var row = $(this);
            var name = row.find("input[name]").val();
            if (name && name != "")
                newRes.push(new Resource("tmp_" + new Date().getTime() + "_" + cnt, name));
        });

        ge.resources = newRes;

        closeBlackPopup();
        ge.redraw();
    });


    var ndo = createModalPopup(400, 500).append(resourceEditor);
}




$.JST.loadDecorator("RESOURCE_ROW", function (resTr, res) {
    resTr.find(".delRes").click(function () { $(this).closest("tr").remove() });
});

$.JST.loadDecorator("ASSIGNMENT_ROW", function (assigTr, taskAssig) {
    var resEl = assigTr.find("[name=resourceId]");
    var opt = $("<option>");
    resEl.append(opt);
    for (var i = 0; i < taskAssig.task.master.resources.length; i++) {
        var res = taskAssig.task.master.resources[i];
        opt = $("<option>");
        opt.val(res.id).html(res.name);
        if (taskAssig.assig.resourceId == res.id)
            opt.attr("selected", "true");
        resEl.append(opt);
    }
    var roleEl = assigTr.find("[name=roleId]");
    for (var i = 0; i < taskAssig.task.master.roles.length; i++) {
        var role = taskAssig.task.master.roles[i];
        var optr = $("<option>");
        optr.val(role.id).html(role.name);
        if (taskAssig.assig.roleId == role.id)
            optr.attr("selected", "true");
        roleEl.append(optr);
    }

    if (taskAssig.task.master.permissions.canWrite && taskAssig.task.canWrite) {
        assigTr.find(".delAssig").click(function () {
            var tr = $(this).closest("[assId]").fadeOut(200, function () { $(this).remove() });
        });
    }

});


function loadI18n() {
    GanttMaster.messages = {
        "CANNOT_WRITE": "No permission to change the following task:",
        "CHANGE_OUT_OF_SCOPE": "Project update not possible as you lack rights for updating a parent project.",
        "START_IS_MILESTONE": "Start date is a milestone.",
        "END_IS_MILESTONE": "End date is a milestone.",
        "TASK_HAS_CONSTRAINTS": "Task has constraints.",
        "GANTT_ERROR_DEPENDS_ON_OPEN_TASK": "Error: there is a dependency on an open task !",
        "GANTT_ERROR_DESCENDANT_OF_CLOSED_TASK": "Error: due to a descendant of a closed task.",
        "TASK_HAS_EXTERNAL_DEPS": "This task has external dependencies.",
        "GANNT_ERROR_LOADING_DATA_TASK_REMOVED": "GANNT_ERROR_LOADING_DATA_TASK_REMOVED",
        "CIRCULAR_REFERENCE": "Circular reference.",
        "CANNOT_DEPENDS_ON_ANCESTORS": "Cannot depend on ancestors.",
        "INVALID_DATE_FORMAT": "The data inserted are invalid for the field format.",
        "GANTT_ERROR_LOADING_DATA_TASK_REMOVED": "An error has occurred while loading the data. A task has been trashed.",
        "CANNOT_CLOSE_TASK_IF_OPEN_ISSUE": "Cannot close a task with open issues",
        "TASK_MOVE_INCONSISTENT_LEVEL": "You cannot exchange tasks of different depth.",
        "GANTT_QUARTER_SHORT": "Quarter",
        "GANTT_SEMESTER_SHORT": "Sem",
        "CANNOT_MOVE_TASK": "CANNOT_MOVE_TASK",
        "PLEASE_SAVE_PROJECT": "PLEASE_SAVE_PROJECT"
    };
}



function createNewResource(el) {
    var row = el.closest("tr[taskid]");
    var name = row.find("[name=resourceId_txt]").val();
    var url = contextPath + "/applications/teamwork/resource/resourceNew.jsp?CM=ADD&name=" + encodeURI(name);

    openBlackPopup(url, 700, 320, function (response) {
        //fillare lo smart combo
        if (response && response.resId && response.resName) {
            //fillare lo smart combo e chiudere l'editor
            row.find("[name=resourceId]").val(response.resId);
            row.find("[name=resourceId_txt]").val(response.resName).focus().blur();
        }

    });
}

