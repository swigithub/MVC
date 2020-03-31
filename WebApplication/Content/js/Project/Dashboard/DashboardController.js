var app = angular.module('DashboardApp', ['ui.multiselect', 'ui.select', 'ui.bootstrap', 'ngSanitize']);


app.controller('DashboardCtrl', function ($scope, $http, $timeout, FileUploadService, $compile) {
    $scope.TaskChildLength = 0;
    $scope.CookieIdentifier = 0;
    $scope.siteIdTemp = 0;
    $scope.IsFormSubmited = false;
    $scope.TaskIdIssue = 0;
    $scope.ProjectSiteIdIssue = 0;
    $scope.LogTypeIsssue = 0;
    $scope.CurrentRow;
    $scope.IsNewPage = true;
    $scope.submitted = false;
    $scope.Issue = {};
    $scope.Priority = [];
    $scope.s = {};
    $scope.Task = [];
    $scope.Status = [];
    $scope.Task = [];
    $scope.User = [];
    $scope.TaskType = [];
    $scope.Stages = [];
    $scope.Reasons = [];
    $scope.WorkLogList = [];
    $scope.Severity = [];
    $scope.GNG = [];
    $scope.IssueType = [];
    $scope.psiteid = 0;
    $scope.Critical = [];
    $scope.Completed = [];
    $scope.ProjectSiteStatus = [];
    $scope.ProjectDelivery = [];
    $scope.MilestoneWindow = [];
    $scope.IssueCategory = [];
    $scope.Important = [];
    $scope.Milstone = {};
    $scope.workLog = {};
    $scope.multiple = {};
    $scope.multiple.AssignToId = [];
    $scope.ProjectClients = [];
    $scope.AssignedToIds = [];

    $scope.ToDoDateTime;
    $scope.Alert = '';
    $scope.logdays = '';
    $scope.choseoption = [{ name: "Miletone" }, { name: "Stage" }]
    $scope.selectedIndex = 0;
    $scope.fileloaderfile = false;
    var StatusId = 0;
    $scope.Attachments = [{ Files: '', Description: '', CategoryId: 0, Tags: '', SiteTaskId: 0, file: '', IsDeleted: 0, FileName: '', FileExtension: '', ProjectId: 0, AttachmentId: 0, SiteTaskId: 0 }]
    //  $scope.Attachments = []
    $scope.CleatIssue = function () {
        $scope.Issue = {};
    }
    $scope.setlogdays = function (d) {
        $scope.logdays = d;
    }
    //Upload Files
    $scope.AddNewDiv = function () {
        $scope.Attachments.push({ Files: '', Description: '', Tags: '', SiteTaskId: 0, file: '', IsDeleted: 0, FileName: '', FileExtension: '', ProjectId: 0, AttachmentId: 0, SiteTaskId: 0 });
    }
    //delete row
    $scope.RemoveDiv = function (X, event) {
        X.IsDeleted = 1;
        $(event.target).parents('tr').remove();
    }
    $(document).on("click", ".deleterowattachment", function (e) { //user click on remove text
        e.preventDefault();
        var Delete = 0;
        $(".PlanFloorNewRow").each(function () {
            Delete++
        });
        if (Delete > 1) {
            $(this).parent().parent().remove();
        }
    })
    $scope.fileList = [];
    $scope.curFile;
    $scope.ImageProperty = {
        file: ''
    }

    $(document).ready(function () {
        $scope.GetProjectSites();
        $scope.RefreshToDoTask();
    });

    $scope.setFile = function (element, X) {
        var reader = new FileReader();

        //$scope.fileList = [];
        // get the files
        var files = element.files;

        if (element.files[0].size > 3072000) {
            swal("File is too big !", "Maximum 3 Mb Allowed", "error");
            element.value = "";
            this.x.FileExtension = '';
            this.x.FileName = '';
            this.x.file = '';
        }
        else {
            let MyThis = this;
            reader.addEventListener("load", function () {
                MyThis.x.file = reader.result;
            }, false);
            reader.readAsDataURL(files[0])
            MyThis.x.FileExtension = files[0].name.split('.')[1];
            MyThis.x.FileName = files[0].name.split('.')[0];

        }
        $scope.$apply();


        //for (var i = 0; i < files.length; i++) {
        //    $scope.ImageProperty.file = files[i];

        //    $scope.fileList.push($scope.ImageProperty);
        //    $scope.ImageProperty = {};
        //    $scope.$apply();

        //}
    }
    $scope.UploadFileIndividual = function (fileToUpload, name, type, size, index) {
        //Create XMLHttpRequest Object
        var reqObj = new XMLHttpRequest();

        //open the object and set method of call(get/post), url to call, isAsynchronous(true/False)
        reqObj.open("POST", "/Project/Dashboard/UploadFiles", true);

        //set Content-Type at request header.for file upload it's value must be multipart/form-data
        reqObj.setRequestHeader("Content-Type", "multipart/form-data");

        //Set Other header like file name,size and type
        reqObj.setRequestHeader('X-File-Name', name);
        reqObj.setRequestHeader('X-File-Type', type);
        reqObj.setRequestHeader('X-File-Size', size);

        // send the file
        reqObj.send(fileToUpload);

    }
    $scope.OpenAttachmentModal = function (SiteTaskId) {
        $.ajax({
            url: '/Project/Dashboard/ProjectAttachment?ProjectId=' + $("#pId").attr("data-ProjectId") + '&SiteTaskId=' + SiteTaskId,
            type: 'Get',
            async: false,
            success: function (res) {
                $('#ProjectAttachmentTableBody').html("");
                $('#ProjectAttachmentTableBody').html(res);
                $("#AttachmentModal").modal({
                    show: 'true'
                });
                $scope.$apply();
            }
        });

    }
    var SiteTaskId = 0;

    $scope.GetAttachs = function (_SiteTaskId) {
        SiteTaskId = _SiteTaskId
        $http.get('/Project/Dashboard/SiteTaskAttachments?SiteTaskId=' + SiteTaskId).then(function (resd) {
            if (resd.data.Status == "success") {
                $scope.Attachments = [];
                $scope.Attachments = resd.data.Value;
                if ($scope.Attachments.length ==0)
                    $scope.Attachments.push({ Files: '', Description: '', Tags: '', SiteTaskId: 0, file: '', IsDeleted: 0, FileName: '', FileExtension: '', ProjectId: 0, AttachmentId: 0, SiteTaskId: 0 });
            }
            else {
                swal("Error Occured While Fetching Attachments !", "", "error");
            }
        });
    }
    $("#frm-projectattachmentform").submit(function (evt) {
        evt.stopPropagation();
        let RequiredFields = true;
        $('#frm-projectattachmentform').find('input').each(function () {
            if (!$(this).prop('required')) {
                // console.log("NR");
            } else {
                //console.log("IR");
                if ($(this).val() == "") {
                    RequiredFields = false
                }
            }
        });
        $('#frm-projectattachmentform').find('select').each(function () {
            if (!$(this).prop('required')) {

            } else {
                if ($(this).val() == "" || $(this).val() == "0" || $(this).val() == "?") {
                    RequiredFields = false
                }

            }
        });
        if (RequiredFields == true) {
            $scope.Attachments.every(x => x.SiteTaskId = SiteTaskId);
            $scope.Attachments.every(x => x.ProjectId = $("#pId").attr("data-ProjectId"));
            $http.post('/Project/Dashboard/SaveAttachment', $scope.Attachments).then(function (res) {
                if (res.data.Status == "success") {
                    $scope.GetAttachs(SiteTaskId);
                    swal("Attachment Saved !", "", "success");

                }
                else if (res.data.Status == "danger") {
                    swal("Error !", "", "error");
                }
                else  {
                    swal("Session Expired !", "", "error");
                    location.reload();
                }

            });
            return false;
        }
        else {
            swal({
                title: "Fields with * are Compulsary !",
                type: "warning",
                border: "3px solid red;"
            });
            return false
        }
    });


    $scope.SaveAttachment = function () {

        for (var i = 0; i < $scope.fileList.length; i++) {

            $scope.UploadFileIndividual($scope.fileList[i].file,
                                        $scope.fileList[i].file.name,
                                        $scope.fileList[i].file.type,
                                        $scope.fileList[i].file.size,
                                        i);
        }
        for (var i = 0; i < $scope.fileList.length; i++) {
            $scope.Attachments[i].Files = $scope.fileList[i].name;
            $scope.Attachments[i].Files1 = $scope.fileList[i].type;
            $scope.Attachments[i].Files2 = $scope.fileList[i].file;
            $scope.Attachments[i].Files3 = $scope.fileList[i].file;
        }
        $http.post('/Project/Dashboard/SaveAttachment', $scope.Attachments).then(function (res) {

        });
    }
    //Upload Files End
    $scope.fnProjectResource = function () {


        $scope.AssignedToIds = [];
        for (var i = 0; i < $scope.multiple.AssignToId.length; i++) {
            var Id = $scope.multiple.AssignToId[i].UserId;
            $scope.AssignedToIds.push(Id);
        }
        $scope.AssignedToIds = $scope.AssignedToIds.join(',')

    }
  
    //$scope.Test= function () {
    //    ets = $("#example-xss-html_Market").val();
    //    var issuetasks = $("#example-multiple-optgroups_Market").val();
    //    var issuemarkets = $("#example-xss-html_Market").val();
    //    mfromdate = FromDate;
    //    mtodate = ToDate;
    //    //mmarketid = $("#example-xss-html_Market").val();
    //    mmarketid = issuemarkets.join(',');
    //    mtaskid = $("#example-multiple-optgroups_Market").val();
    //    $.ajax({
    //        url: '/Project/Dashboard/GetMarkets?filter=' + 'Get_RegionView_WO&ProjectId=' + $("#pId").attr("data-ProjectId") + '&Value=null',

    //        type: 'get',
    //        async: false,
    //        success: function (data) {

    //            $('#regionviewchart').empty();
    //            $('#regionviewchart').html(data);
    //        }
    //    })


    //}
    //$(".issue").click(function () {
    //    debugger
    //    $scope.SelectedPriority = {}
    //    $scope.Issue = {}
    //    $scope.SelectedStatus = {}
    //    $scope.SelectedPriority = $(this).attr('data-Priority');
    //    $scope.SelectedStatus = $(this).attr('data-Status');
    //    $scope.AssignedToIds = $(this).attr('data-AssingedTo');
    //    $scope.AssignedToIds = $scope.AssignedToIds.split(',');
    //    $scope.Issue = {
    //        IssueId: parseInt($(this).attr('data-IssueId')), IssuePriorityId: parseInt($(this).attr('data-PriorityId')), IssueStatusId: parseInt($(this).attr('data-StatusId')), TaskId: parseInt($(this).attr('data-TaskId')), TaskTypeId: parseInt($(this).attr('data-TaskType')),
    //        Description: $(this).attr('data-Description'), AssignedToId: parseInt($(this).attr('data-AssingedTo')), TargetDate: $(this).attr('data-TargetDate'), ProjectSiteId: $(this).attr('data-ProjectSiteId'), ForecastDate: $(this).attr('data-ForecastDate'),
    //        IssueById: parseInt($(this).attr('data-IssueById')), ReasonId: parseInt($(this).attr('data-ReasonId'))
    //    }

    //    $http.get('/Project/Issue/GetTasks?projectId=' + $("#pId").attr("data-ProjectId") + '&TaskId=' + parseInt($(this).attr('data-TaskType'))).then(function (res) {
    //        $scope.Task = res.data;
    //    })


    //    //$('#IssueModal').modal('hide');
    //    $('#IssueModal').modal({
    //        show: 'true'
    //    });

    //});
    $scope.IssueLog = {};

    $scope.ChangeIssueStatus = function () {
        $scope.IssueLog.IssueId = IssueId;
        $http.post('/Project/Issue/ChangeIssueStatus', $scope.IssueLog).then(function (res) {
            $scope.LogIssueList.push($scope.IssueLog);
            $('#IssueStatusModal').modal('hide');
            ShowSuccessMessage();
            $scope.IssueLog = {}
        });
    }
    $scope.GetUserById = function (Id) {
        var obj = {};
        $.each($scope.User, function (i, v) {
            if (v.UserId == Id) {
                obj = v;
                return obj;
            }
        });
        return obj;
    }
    $scope.LogIssueList = [];
    $scope.isDisable = false;
    $scope.GetIssueLog = function () {
        $http.get('/Issue/GetIssueLog/?IssueId=' + IssueId).then(function (result) {
            if (LogStatus == 'Close') {
                $scope.isDisable = true;
            }
            for (var i = 0; i < result.data.length; i++) {
                result.data[i].CreatedOn = moment.utc(result.data.CreatedOn).format('DD MMM, YYYY HH:MM');
            }
            $scope.LogIssueList = result.data;
        })
    }
    $scope.IssueById;
    $scope.tickettype = function (type) {
        for(i=0;i<$scope.TicketType.length;i++){
            if ($scope.TicketType[i].DefinationName == type) {
                $scope.Issue.TicketTypeId = $scope.TicketType[i].DefinationId;

            }
           
        }
      

    }
    $scope.GetIssueLogId = function (IssueId) {
        $scope.GetIssueType();
        $http.get('/Issue/GetIssue_ById/?IssueId=' + IssueId).then(function (result) {
            $scope.IssueById = result.data[0];
            $scope.Issue.IssueCategoryId = $scope.IssueById.IssueCategoryId;
            //  
            $scope.Issue.eNB = $scope.IssueById.eNB;
            $scope.Issue.ExtendedeNB = $scope.IssueById.ExtendedeNB;
            $scope.Issue.AOTSCR = $scope.IssueById.AOTSCR;
            $scope.Issue.EquipmentId = $scope.IssueById.EquipmentId;
            $scope.Issue.ActivityTypeId = $scope.IssueById.ActivityTypeId;
            $scope.Issue.TicketTypeId = $scope.IssueById.TicketTypeId;
            $scope.Issue.MSWindowId = $scope.IssueById.MSWindowId;
            $scope.Issue.AlarmId = $scope.IssueById.AlarmId;
            $scope.Issue.IsUnavoidable = $scope.IssueById.IsUnavoidable;
            $scope.Issue.SeverityId = $scope.IssueById.SeverityId;
            $scope.Issue.RequestedBy = $scope.IssueById.RequestedBy;
            $scope.Issue.ItemTypeId = $scope.IssueById.ItemTypeId;
            $scope.Issue.TicketTypeId = $scope.IssueById.TicketTypeId;
            $scope.Issue.RequestedDate = moment.utc($scope.IssueById.RequestedDate).format('DD MMM, YYYY');
            $scope.Issue.ReasonId = $scope.IssueById.ReasonId;
            //setTimeout(function () {
            //    $scope.Issue.ReasonId = $scope.IssueById.ReasonId;
            //}, 2000);

        })
    }
    //$scope.GetIssueLog();
    $scope.GetDefinitions = function () {
        $http.get('/Project/Dashboard/GetDefination').then(function (result) {
            $scope.Reasons = [];
            $scope.TaskType = [];
            $scope.Status = [];
            $scope.Priority = [];
            $scope.ActivityType = [];
            $scope.ItemType = [];
            $scope.ProjectSiteStatus = [];
            $scope.MilestoneWindow = [];
            $scope.IssueCategory = [];
            $scope.Alarm = [];
            $scope.Severity = [];
            $scope.GNG = [];
            $scope.Categories = [];
            $scope.ProjectDelivery = [];
            $scope.TicketType = [];
            for (var i = 0; i < result.data.length; i++) {
                if (result.data[i].KeyCode == 'ISSUE_STATUS') {
                    if (result.data[i].DefinationName == 'Open') {
                        StatusId = result.data[i].DefinationId;
                    }
                    $scope.Status.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'ISSUE_PRIORITY') {
                    $scope.Priority.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'PROJECT_MILESTONE' || result.data[i].KeyCode == 'PROJECT_STAGE') {
                    $scope.TaskType.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'REASON') {
                    $scope.Reasons.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'ISSUE CATEGORY') {
                    $scope.IssueCategory.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'PROJECT SITE STATUS') {
                    $scope.ProjectSiteStatus.push({ DefinationId: result.data[i].DefinationId, DisplayText: result.data[i].DisplayText, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'PROJECT DELIVERY') {
                    if (result.data[i].DefinationName != "Ongoing") {
                        $scope.ProjectDelivery.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                    }
                }
                else if (result.data[i].KeyCode == 'MILESTONE WINDOW') {
                    $scope.MilestoneWindow.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'ALARMS') {
                    $scope.Alarm.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'GNG STATUS') {
                    $scope.GNG.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'SEVERITY') {
                    $scope.Severity.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'ACTIVITY TYPE') {
                    $scope.ActivityType.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'TICKET TYPE') {
                    $scope.TicketType.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'ITEM TYPE') {
                    $scope.ItemType.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }
                else if (result.data[i].KeyCode == 'Task_Attachment_Category') {
                    $scope.Categories.push({ DefinationId: result.data[i].DefinationId, DefinationName: result.data[i].DefinationName, KeyCode: result.data[i].KeyCode, ColorCode: result.data[i].ColorCode })
                }



            }
        }, function (result) {
        });
    }


    $scope.FilterStatus = function () {
        $scope.GetDefinitions();
        $scope.GetDefinitions();
        $timeout(function () {
            var option2 = '';
            //for (var i = 0; i < $scope.ProjectSiteStatus.length; i++) {
            //    option2 += '<option value="' + $scope.ProjectSiteStatus[i].DefinationId + '">' + $scope.ProjectSiteStatus[i].DefinationName + '</option>'
            //}
            var optgroup5 = '';
            for (var i = 0; i < $scope.ProjectSiteStatus.length; i++) {
                option = '';
                //  for (var j = 0; j < res.data[i].Tasks.length; j++) {
                option2 += '<option value="' + $scope.ProjectSiteStatus[i].DefinationId + '">' + $scope.ProjectSiteStatus[i].DisplayText + '</option>'
                //}
                // optgroup5 += '<optgroup label="' + $scope.ProjectSiteStatus[i].DefinationId + '">' + option + '</optgroup>'
            }
            optgroup5 += '<optgroup label="Projects">' + option2 + '</optgroup>'
            //  var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Map").multiselect();});<' + '/' + 'script><select class="form-control" id="example-multiple-optgroups_Map">' + option2 + '</select>';
            var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Map").multiselect({enableClickableOptGroups: true,includeSelectAllOption: true});});</script><select class="form-control"  id="example-multiple-optgroups_Map" multiple="multiple">' + option2 + '</select>';

            //$('#ListMapStatus').html(txt1)
            $('#ListMapStatus').append(txt1)

        }, 6000)

    }
    $scope.FilterType = function () {
        $scope.GetDefinitions();
        $timeout(function () {
            var option7 = '';
            //for (var i = 0; i < $scope.ProjectSiteStatus.length; i++) {
            //    option2 += '<option value="' + $scope.ProjectSiteStatus[i].DefinationId + '">' + $scope.ProjectSiteStatus[i].DefinationName + '</option>'
            //}
            var optgroup7 = '';
            for (var i = 0; i < $scope.ProjectDelivery.length; i++) {
                option = '';
                //  for (var j = 0; j < res.data[i].Tasks.length; j++) {
                option7 += '<option value="' + $scope.ProjectDelivery[i].DefinationId + '">' + $scope.ProjectDelivery[i].DefinationName + '</option>'
                //}
                // optgroup5 += '<optgroup label="' + $scope.ProjectSiteStatus[i].DefinationId + '">' + option + '</optgroup>'
            }
            optgroup7 += '<optgroup label="Projectdelivery">' + option7 + '</optgroup>'
            //  var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Map").multiselect();});<' + '/' + 'script><select class="form-control" id="example-multiple-optgroups_Map">' + option2 + '</select>';
            var txt7 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Map7").multiselect({enableClickableOptGroups: true,includeSelectAllOption: true});});</script><select class="form-control"  id="example-xss-html_Map7" multiple="multiple">' + option7 + '</select>';

            //$('#ListMapStatus').html(txt1)
            $('#ListMapType').append(txt7)

        }, 4000)

    }
    $scope.FilterStatus();
    $scope.FilterType();
    $scope.EditIssue = function (data, priority) {

        $scope.Issue = data;
        if ($scope.Issue.ProjectSiteId == 0 || $scope.Issue.ProjectSiteId == undefined) {
            $scope.Issue.ProjectSiteId = $scope.psiteid;
        }
        $scope.multiple.AssignToId = [];
        for (var i = 0; i < data.AssignedToId.length; i++) {
            var tmp = $scope.GetUserById(data.AssignedToId[i]);
            $scope.multiple.AssignToId.push(tmp);
        }
        //$http.get('/Project/Issue/GetTasks?projectId=' + $("#pId").attr("data-ProjectId") + '&TaskId=' + parseInt($(this).attr('data-TaskType'))).then(function (res) {
        //    $scope.Task = res.data;
        //})

        $scope.SelectedPriority = priority;
        if ($scope.Issue.ItemFilePath != null && $scope.Issue.ItemFilePath != undefined && $scope.Issue.ItemFilePath !="")
            $scope.fileloaderfile = true;
        else {
            $scope.fileloaderfile = false;
    }
        //$('#IssueModal').modal('hide');
        $('#IssueModal').modal({
            show: 'false'
        });
        ///   $scope.$apply()
    }

    $('.issue').click(function () {
        $("header").append(`<div id="ajaxLoading" style="position: fixed; top: 0px; bottom: 0px; left: 0px; right: 0px; margin: auto; padding: 8px; text-align: center; vertical-align: middle; width: 140px; height: 95px; z-index: 2000; border-radius: 4px; display: block;"><img src="/Content/Images/Application/Logo_Loading.gif" style="margin-bottom:8px;width:100px;height:100px"><p style="margin:0;font-size:15px;color:#fff;background:#88887B">Please Wait...</p></div>`);
        $scope.SelectedPriority = {}
        $scope.Issue = {}
        $scope.SelectedStatus = {}
        $scope.SelectedPriority = $(this).attr('data-Priority');
        $scope.SelectedStatus = $(this).attr('data-Status');
        $scope.AssignedToIds = $(this).attr('data-AssingedTo');
        $scope.AssignedToIds = $scope.AssignedToIds.split(',');
        $scope.Issue = {
            IssueId: parseInt($(this).attr('data-IssueId')), IssuePriorityId: parseInt($(this).attr('data-PriorityId')), IssueStatusId: parseInt($(this).attr('data-StatusId')), TaskId: parseInt($(this).attr('data-TaskId')), TaskTypeId: parseInt($(this).attr('data-TaskType')),
            Description: $(this).attr('data-Description'), AssignedToId: parseInt($(this).attr('data-AssingedTo')), TargetDate: $(this).attr('data-TargetDate'), ProjectSiteId: $(this).attr('data-ProjectSiteId'), ForecastEndDate: $(this).attr('data-ForecastEndDate'),
            IssueById: parseInt($(this).attr('data-IssueById')), ReasonId: parseInt($(this).attr('data-ReasonId'))
        }

        $http.get('/Project/Issue/GetTasks?projectId=' + $("#pId").attr("data-ProjectId") + '&TaskId=' + parseInt($(this).attr('data-TaskType'))).then(function (res) {
            $scope.Task = res.data;
        })
        $timeout(function () { $('#mai').trigger('click'); }, 2000)

        //$('#IssueModal').modal('hide');
        $('#IssueModal').modal({
            show: 'true'
        });
        //   $("#ajaxLoading").remove();
    });

    $('.worklog').click(function () {
        $scope.workLog = {};
        $scope.LogDate = {};
        workLog = {
            ProjectId: parseInt($("#pId").attr("data-ProjectId")), ProjectSiteId: parseInt($(this).attr('data-ProjectSiteId')), TaskId: parseInt($(this).attr('data-TaskId')), LogType: $(this).attr('data-LogType')
        }
    });
    //$(window).resize(function () {
    //    var width = $("#BookMarkList").width();

    //}).resize();
    $scope.IsCompleted = false;
    function FindStausName(id, data) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].DefinationId == id) {
                return data[i].DefinationName;
            }
        }
    }
    $scope.CheckStatusCount = function (id) {
        var name = FindStausName(id, $scope.ProjectSiteStatus)
        if (name == 'Completed') {


            $http.get("/Project/Dashboard/GetDashboardCharts?Filter=GET_COUNT_SITE_STATUS&ProjectId=" + parseInt($("#pId").attr("data-ProjectId")) + '&Value1=' + ProjectSiteId).then(function (data) {

                if (data.data[0].Column1 > 0) {
                    $scope.IsCompleted = true;
                }
                else {
                    $scope.IsCompleted = false;
                }
            });
        }
    }
    //$scope.CheckStatusCount();

    var param = {
        Filter: 'ByProjectId', Value: parseInt($("#pId").attr("data-ProjectId"))
    }
    $http.post("/Project/Defination/ToSingle", { Filter: 'ByProjectId', Value: parseInt($("#pId").attr("data-ProjectId")) }).then(function (res) {
        $scope.s = res.data;

        if ($scope.s != null) {
            $scope.ProjectClients.push({
                Id: $scope.s.ClientId, Text: $scope.s.Client
            });
            if ($scope.s.ClientId != $scope.s.EndClientId) {
                $scope.ProjectClients.push({
                    Id: $scope.s.EndClientId, Text: $scope.s.EndClient
                });
            }
        }
        var EndDate = "Continue";
        if ($scope.s.EstimateEndDate != null && $scope.s.EstimateEndDate != undefined) {
            EndDate = moment($scope.s.EstimateEndDate).format('DD MMM, YYYY');
        }
        var ProjectInfo = '<span style="font-size: 16px;font-family: inherit;color:#000;"><strong>Project: </strong>' + $scope.s.ProjectName + '</span>' +
            '<span style="margin-left:1%;font-size: 16px;font-family: inherit;color:#000;">' +
            '<strong style="margin-left:2%;color:#000;">Start Date: </strong><span class="ProjectDate" style="font-size: 16px;font-family: inherit;color:#000;;margin-right: 27px;">' + moment($scope.s.EstimateStartDate).format('DD MMM, YYYY') + ' </span> <strong> Expected End Date: </strong> ' +
            '<span class="ProjectDate" style="font-size: 16px;font-family: inherit;color:#000;">' + EndDate + '</span>'

        var detail = '<span> <a title="Project Timeline" href="/Project/Defination/Details/' + $("#pId").attr("data-ProjectId") + '"><i class="fa fa-calendar"></i></a> </span>'
        $('#detail').html(detail);
        var Edit = '<span> <a title="Edit Project" href="/Project/Defination/new/' + $("#pId").attr("data-ProjectId") + '"><i class="fa fa-edit"></i></a> </span>'
        $('#edit').html(Edit);
        var template = '<span> <a title="Project Template" href="/Project/Template/list?ProjectId=' + $("#pId").attr("data-ProjectId") + '"><i class="fa fa-list-alt"></i></a> </span>'
        $('#template').html(template);

        var seting = '<span> <a  title="Project Chart Settings" href= "/Project/Dashboard/Configuration?Id=' + $("#pId").attr("data-ProjectId") + '"><i class="fa fa-cog"></i></a> </span>'
        $('#setting').html(seting);

        var _worklogs = '<span> <a title="Worklog" href="/Project/Worklog/Index?projectid=' +$("#pId").attr("data-ProjectId") + '"><i class="fa fa-tasks"></i></a> </span>'
  
        $('#worklogs').html(_worklogs);

        //var excel = '<span> <a href="/Project/Dashboard/Export?ProjectId=' + $("#pId").attr("data-ProjectId") + '" title="Project Status Report"><i class="fa fa-file-excel-o"></i></a> </span>'
        //$('#excel').html(excel);

        //var ProjectIcons = '<span class="widget-icon" style="color:#000;float:right;">' +
        //        '<a href="/Project/Defination/Details/' + $("#pId").attr("data-ProjectId") + '" style="margin-right:4px;margin-left:4px;color:#000;">' +
        //            '<span class="fa fa-pencil" style="color:#000;"></span>' +
        //        '</a> &thinsp; &thinsp;' +
        //        '<a href="/Project/Dashboard/Configuration/' + '" style="margin-right:4px;margin-left:4px;color:#000;">' +
        //            '<span class="fa fa-cog" style="color:#000;"></span>' +
        //        '</a> &thinsp; &thinsp;' +
        //        '<a href="" style="margin-right:4px;margin-left:4px;color:#000;">' +
        //            '<span class="fa fa-file-excel-o" style="color:#000;font-size: 18px;"></span>' +
        //        '</a> &thinsp;' +

        //    '</span>'




        //+
        //'<span class="widget-icon" style="color:#fff;">' +
        //    '<a href="/project/template/list?projectId=' + $("#pId").attr("data-ProjectId") + '&scopeId=0" style="margin-right:4px;margin-left:4px;color:#fff;">' +
        //        '<span class="fa fa-file" style="color:#fff;"></span>' +
        //    '</a>' +
        //'</span>'
        $('#ProjectInfo').html(ProjectInfo);
        //$('#Project_Icons').html(ProjectIcons);
    });
    $scope.ResetMultipleAssignids = function () {
        $scope.multiple.AssignToId = [];
    }
    $('.siteid').click(function () {
        $('#Issuelbl').text("Task Issue Ticket");
        $scope.Issue.TaskTypeId = parseInt($(this).attr("data-Milestone"));
        $scope.Issue.TaskId = parseInt($(this).attr("data-TaskId"));
        ProjectSiteId = $(this).attr("data-ProjectSiteId");
      
        $scope.GetUsers(UserID);

    });
    $scope.SaveIssue = function () {
        $scope.IsFormSubmitted = true;
        $scope.Message = "";
        $scope.Issue.RequestedDate = $("#RequestedDate").val();
        $scope.Issue.TargetDate = $("#TargetDate").val();
        //$scope.Issue.AssignedToId = $scope.AssignedToIds.join(',');
        if ($scope.AssignedToIds.length > 0) {
            $scope.Issue.AssignedToId = $scope.AssignedToIds
        } else {
            $scope.Issue.AssignedToId = ""
        }

        $scope.Issue.ProjectId = $("#pId").attr("data-ProjectId");
        if ($scope.Issue.ProjectSiteId == undefined || $scope.Issue.ProjectSiteId == null) {
            $scope.Issue.ProjectSiteId = ProjectSiteId;
            $scope.Issue.TaskId = TaskID
            $scope.Issue.IssueStatusId = StatusId
        }
        if ($scope.Issue.ProjectSiteId == 0 || $scope.Issue.ProjectSiteId == undefined) {
            $scope.Issue.ProjectSiteId = $scope.psiteid;
        }

        //var validFormats = ['jpg', 'gif','jpge','png','csv'];
        //return {
        //    require: 'ngModel',
        //    link: function (scope, elem, attrs, ctrl) {
        //        ctrl.$validators.validFile = function () {
        //            elem.on('change', function () {
        //                var value = elem.val(),
        //                    ext = value.substring(value.lastIndexOf('.') + 1).toLowerCase();

        //                return validFormats.indexOf(ext) !== -1;
        //            });
        //        };
        //    }
        //};
        $scope.Issue.Status = 'Open';
        FileUploadService.UploadFile($scope.SelectedFileForUpload, $scope.Issue, "issue").then(function (d) {

            $('#IssueModal').modal('hide');
            ShowSuccessMessage("Save Issue Ticket");
         
        }, function (e) {

        });
        //$http.post('/Project/Issue/New', $scope.Issue).then(function (res) {

        //    $('#IssueModal').modal('hide');
        //    ShowSuccessMessage("Save Issue Ticket");
        //    FillIssues();
        //    $scope.Issue = {}
        //});
        FillIssues();
        angular.element(document.getElementById('widget-grid')).scope().clearfields();
    }

    $scope.openWorkLogTab = function (WLogId, LogDate, LogHours, tt, Description) {
        //$scope.EditWLogId = WLogId;
        //$scope.LogDate = LogDate;
        //$scope.LogHours = LogHours;
        //$scope.Description = Description;
        //$scope.CurrentRow = '';
        //  $scope.IsNewPage = false;
        $scope.ddd=[];
        $scope.ddd.push(moment(LogDate).format('LLLL'));
        $('#WorkLogTabArea a[href="#WorkLogForm"]').tab('show');
        $scope.GetSelectedSiteWorkLog($scope.ddd,tt);

        $('.pickdate').datepicker('hide');
        $(".dated-input").find('span').detach();
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        /* === Paint The Hours Div According To The Result === */
        $.each(selectedDates, function (i, e) {
            let date = moment(selectedDates[i].date);
            date = new Date(date);
            date = date.getDate() + "-" + monthNames[date.getMonth()] + "-" + date.getFullYear();
            let approval = selectedDates[i].isApproved;
            let description = selectedDates[i].description;
            $("#work-desc").find('section').detach();
            if (approval == true) {
                $(".dated-input").append(`<span><i>${date}</i><input required type="number" value="${selectedDates[i].hours}" data-WLogId="${selectedDates[i].WLogId}" data-isApproved="${selectedDates[i].isApproved} " data-date="${selectedDates[i].date} " readonly></span>`);
                if (selectedDates.length == 1) {
                    $("#work-desc").append(`<section><label class="label">Comment <sup style="color:red">*</sup></label><div class="textarea"><textarea rows="3" maxlength="300" ng-model="Description" id="logdes" placeholder="Comment" class ="form-control ng-pristine ng-untouched ng-valid ng-empty" readonly>${description}</textarea></div></section>`);
                }
            } else {
                $(".dated-input").append(`<span><i>${date}</i><input required type="number" value="${selectedDates[i].hours}" data-WLogId="${selectedDates[i].WLogId}" data-isApproved="${selectedDates[i].isApproved} " data-date="${selectedDates[i].date} " ></span>`);
                if (selectedDates.length == 1) {
                    $("#work-desc").append(`<section><label class="label">Comment <sup style="color:red">*</sup></label><div class="textarea"><textarea  required rows="3" maxlength="300" ng-model="Description"  id="logdes"placeholder="Comment" class ="form-control ng-pristine ng-untouched ng-valid ng-empty">${description}</textarea></div></section>`);
                }
            }
        });
    }

    $scope.SaveLog = function () {
        $scope.workLogs = [];

        $(".dated-input :input").each(function (i, e) {
            $scope.workLog = {};
            $scope.workLog.TaskId = workLog.TaskId;
            $scope.workLog.ProjectId = $("#ProjectIdLogs").val();
            $scope.workLog.ProjectSiteId = workLog.ProjectSiteId;
            $scope.workLog.LogType = workLog.LogType;
            //if (workLog != undefined && workLog != null) {
            //    $scope.workLog = workLog;
            //}
            if ($('#divId input').length < 2 ) { 
                $scope.workLog.Description = $("#logdes").val();
            }
          
            
            $scope.workLog.LogDate = $(this).attr("data-date");
            $scope.workLog.LogHours = $(this).val();
            $scope.workLog.WLogId = $(this).attr("data-WLogId");
            $scope.workLog.LogType = $(this).attr("data-WLogType");
            $scope.workLog.isApproved = $(this).attr("data-isApproved");
            if ($scope.workLog.WLogId != "0" ||  $scope.workLog.LogHours!=""){
                $scope.workLogs.push($scope.workLog);
            }
        });
        
        if ($scope.workLogs.length >0) {
            $http.post('/Project/WorkLog/New', $scope.workLogs).then(function (res) {

                $('#LogModal').modal('hide');
                $scope.workLog = {
                }
                $scope.LogDate = '';
                $scope.LogHours = '';
                $scope.Description = '';
                $(".worklogdate").val('');
                $('#LogModal').modal('hide');
                swal("Success", "Work Log added Successfully", "success");
                //ShowSuccessMessage("Save Work Log Successfully");
            });
        }
        //else {
        //    $scope.workLog.WLogId = $scope.EditWLogId;
        //    $http.post('/Project/WorkLog/Edit', $scope.workLogs).then(function (res) {
        //       if (res.data == true) {
        //            $('#LogModal').modal('hide');
        //            swal("Success", "Work Log updated Successfully", "success");
        //            //var tbRow = $scope.CurrentRow;
        //            //$scope.CurrentRow.Description = workLog.Description;
        //            //$scope.CurrentRow.LogDate = moment.utc(workLog.LogDate).format('MM/DD/YYYY');
        //            //$scope.CurrentRow.LogHours = workLog.LogHours;
        //            // ShowSuccessMessage("Save Work Log Successfully");
        //        }
        //    });
        //}

    }
    //alert('From CTR', pageNum);
    function FillIssues() {
        var RDate = $('#reportrange3').find('.changingSelector').html();
        var Datees = RDate.split('-');
        $http.get('/Project/Dashboard/GetProjectIssue?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + $.cookie($scope.CookieIdentifier + 'ProjectTasks')
+ '&LocationIds=' + $.cookie($scope.CookieIdentifier + "ProjectMarkets") + '&FromDate=' + Datees[0] + '&ToDate=' + Datees[1]).then(function (res) {

    $('#divissues').html(res.data);
});
        //var req = {
        //    method: 'POST',
        //    url: '/Project/Dashboard/GetProjectIssue',

        //    params: { ProjectId: $("#pId").attr("data-ProjectId"), 'Page': pageNum }
        //}

        //$http(req).then(function (responce) {

        //    $('#divissues').empty();
        //    $('#divissues').html(responce.data);
        //    //fnWoStatus();

        //    //var showing = parseInt(page) * 5;
        //    //$('#GridShowing').text(showing);
        //}, function (responce) {
        //    // Failure Function
        //});


    }
    $scope.setSelected = function (flag, issue) {
        //$scope.selectedIndex = i;
        if (flag == 'prio') {
            $scope.SelectedPriority = issue.DefinationName;
            $scope.Issue.IssuePriorityId = issue.DefinationId;

        }
        else if (flag == 'task') {
            $scope.SelectedTask = issue.DefinationName;
            $scope.Issue.TaskId = issue.DefinationId;
        }
        else if (flag == 'status') {
            $scope.SelectedStatus = issue.DefinationName;
            $scope.Issue.IssueStatusId = issue.DefinationId;
            $scope.IssueLog.StatusId = issue.DefinationId;
        }
    }

    $scope.Alarm = [];
    $scope.Severity = [];
    $scope.GNG = [];
    $scope.ActivityType = [];
    $scope.ItemType = [];
    var TaskName = '';
    function CreateTaskObject(data) {
        var options = [];[{
            label: '', subItems: []
        }];
        var child = []
        for (var i = 0; i < data.length; i++) {
            TaskName = data[0].Task
            child = [];
            for (var j = 0; j < data[i].Tasks.length; j++) {
                child.push(data[i].Tasks[j].Task);
            }
            options.push({
                label: data[i].Task, subItems: child
            })
        }
        return options;
    }
    function GegInitailTasks(name) {
        var initial = {
        }
        Initail = [];
        if ($.cookie(name) != undefined) {
            Initail = $.cookie(name).split(',')
        }
        if (Initail.length >= 1) {
            //tsks = '{initial: {parent:"' + Initail[0] + '",children:["' + Initail[1] + '"]}'
            initial.parent = Initail[0];
            initial.children = [];
        }
        else if (Initail.length >= 2) {
            initial.parent = Initail[0];
            initial.children = [1];
        }
        if (Initail.length == 0) {
            initial.parent = TaskName;
            initial.children = [];
        }
        return initial;

    }



    let TasksGrouping = function (Tasks) {
        var optgroup2 = '';
        var options = '';
        for (var i = 0; i < Tasks.length; i++) {
            //   var option = TasksGrouping(Tasks[i].Tasks);
            if (Tasks[i].Tasks.length > 0) {
                var option = TasksGrouping(Tasks[i].Tasks);
                options += '<optgroup label="' + Tasks[i].Task + '">' + option + '</optgroup>';
            }
            else {
                options += '<option value="' + Tasks[i].TaskId + '" selected="selected">' + Tasks[i].Task + '</option>';
            }
        }
        return options;
    }
    let TasksGroupingdynatree = function (Tasks) {

        var optgroup2 = '';
        var options = '';
        for (var i = 0; i < Tasks.length; i++) {
            if (Tasks[i].Tasks.length > 0) {
                var option = TasksGroupingdynatree(Tasks[i].Tasks);
                options += '<li id="' + Tasks[i].TaskId + '">' + Tasks[i].Task + '<ul>' + option + '</ul></li>'; //'<li><ul id="' + Tasks[i].Task + '"></ul>' + option + '</li>';

            }
            else {
                options += '<li id="' + Tasks[i].TaskId + '" selected="selected">' + Tasks[i].Task + '</li>';
            }
        }
        return options;
    }
    //$http.get('/Project/Dashboard/GetTaskList?Filter=Get_Project_Tasks&projectId=' + parseInt($("#pId").attr("data-ProjectId"))).then(function (res) {
    $http.post('/Project/Task/getAllTask?Filter=Get_Project_Tasks&projectId=' + parseInt($("#pId").attr("data-ProjectId"))).then(function (res) {
        var grpOptions = CreateTaskObject(res.data)
        var init = GegInitailTasks("ProjectTasksTitle");
        var optgroup2 = '';
        for (var i = 0; i < res.data.length; i++) {
            var ParentOption = ''
            //var option = TasksGrouping(res.data[i].Tasks);
            var option = TasksGroupingdynatree(res.data[i].Tasks);

            // optgroup2 += '<optgroup label="' + res.data[i].Task + '">' + option + '</optgroup>';
            if (res.data[i].Tasks.length > 0) {
                optgroup2 += '<li id="' + res.data[i].TaskId + '">' + res.data[i].Task + '<ul>' + option + '</ul></li>';
            }
            else {
                optgroup2 += '<li id="' + res.data[i].TaskId + '">' + res.data[i].Task + '</li>';
            }
        }
        $(".dynatree123").append('<ul>' + optgroup2 + '</ul>');
        $(".dynatree123").dynatree({
            persist: true,
            checkbox: true
        });

        $('#dynatreetracking').dynatree({
            checkbox: true,
            selectMode: 3,
            onSelect: function (flag, node) {
                // get selected Tasks
                var selectedNodes = node.tree.getSelectedNodes();
                var selectedKeys = $.map(selectedNodes, function (node) {
                    return node.data.key;
                });
                SelectedTasks = selectedKeys;
                var MyTasks = '';
                if (SelectedTasks.length > 0) {
                    MyTasks = SelectedTasks.join(",");
                }
                $.removeCookie($scope.CookieIdentifier + "ProjectTasks");
                $.cookie($scope.CookieIdentifier + "ProjectTasks", MyTasks, {
                    expires: 2000
                });
            }
        });

        var SelectedTasks = $.map($("#dynatreetracking").dynatree("getSelectedNodes"), function (node) {
            return node.data.key;
        });


        var MyTasks = '';
        if (SelectedTasks.length > 0) {
            MyTasks = SelectedTasks.join(",");
        }
        $.removeCookie($scope.CookieIdentifier + "ProjectTasks");
        $.cookie($scope.CookieIdentifier + "ProjectTasks", MyTasks, {
            expires: 2000
        });






        $('#dynatreemap').dynatree({
            checkbox: true,
            selectMode: 3,

            onSelect: function (flag, node) {
                // get selected Tasks
                var selectedNodes = node.tree.getSelectedNodes();
                var selectedKeys = $.map(selectedNodes, function (node) {
                    return node.data.key;
                });
                SelectedMapTasks = selectedKeys;
                var SelectedTasks = $.map($("#dynatreemap").dynatree("getSelectedNodes"), function (node) {
                    return node.data.key;
                });
                var MyTasks = '';
                if (SelectedTasks.length > 0) {
                    MyTasks = SelectedTasks.join(",");
                }
                $.removeCookie($scope.CookieIdentifier + "MapTasksTitle");
                $.cookie($scope.CookieIdentifier + "MapTasksTitle", MyTasks, {
                    expires: 2000
                });

            }
        });
        if ($.cookie($scope.CookieIdentifier + "MVDynaTaskId") != "" && $.cookie($scope.CookieIdentifier + "MVDynaTaskId") != undefined) {
            var dynaCookie = $.cookie($scope.CookieIdentifier + "MVDynaTaskId").split(", ");

            /* == Remove Selected Nodes == */
            var SelecetdNodes = $("#dynatreemap").dynatree("getSelectedNodes").length;

            if (SelecetdNodes > 0) {
                for (var SelecetdNodesC = 0; SelecetdNodesC < SelecetdNodes; SelecetdNodesC++) {

                    if ($("#dynatreemap").dynatree("getSelectedNodes")[SelecetdNodesC].data.key != undefined) {
                        var TempKey = $("#dynatreemap").dynatree("getSelectedNodes")[SelecetdNodesC].data.key;
                        $("#dynatreemap").dynatree("getTree").selectKey(TempKey, false);
                    }

                }
            }


            for (var cc = 0; cc < dynaCookie.length; cc++) {
                $("#dynatreemap").dynatree("getTree").selectKey(dynaCookie[cc]);
                if (dynaCookie.length == 1) {
                    var Title = $("#dynatreemap").dynatree("getTree").selectKey(dynaCookie[cc]).data.title;
                    $('#ListMapIssues').children().eq(0).text(Title);
                } else {

                    $('#ListMapIssues').children().eq(0).text('Multiple Selected');
                }

            }
        }
        var SelectedTasks = $.map($("#dynatreemap").dynatree("getSelectedNodes"), function (node) {
            return node.data.key;
        });
        var MyTasks = '';
        if (SelectedTasks.length > 0) {
            MyTasks = SelectedTasks.join(",");
        }
        $.removeCookie($scope.CookieIdentifier + "MapTasksTitle");
        $.cookie($scope.CookieIdentifier + "MapTasksTitle", MyTasks, {
            expires: 2000
        });



        $('#dynatreemap').dynatree({
            checkbox: true,
            selectMode: 3,
            onActivate: function (node) {
                if ($.cookie($scope.CookieIdentifier + "MVDynaTaskId") != "") {
                    var dynaCookie = $.cookie($scope.CookieIdentifier + "MVDynaTaskId").split(", ");
                    for (var cc = 0; cc < dynaCookie.length; cc++) {
                        $("#dynatreemap").dynatree("getTree").selectKey(dynaCookie[cc]);
                    }
                }

                //$.cookie("MVDynaTaskId");

            },
            onSelect: function (flag, node) {
                // get selected Tasks
                var selectedNodes = node.tree.getSelectedNodes();
                var selectedKeys = $.map(selectedNodes, function (node) {
                    return node.data.key;
                });
                SelectedMapTasks = selectedKeys;
                var SelectedTasks = $.map($("#dynatreemap").dynatree("getSelectedNodes"), function (node) {
                    return node.data.key;
                });
                var MyTasks = '';
                if (SelectedTasks.length > 0) {
                    MyTasks = SelectedTasks.join(",");
                }
                $.removeCookie($scope.CookieIdentifier + "MapTasksTitle");
                $.cookie($scope.CookieIdentifier + "MapTasksTitle", MyTasks, {
                    expires: 2000
                });

            }
        });
        var SelectedTasks = $.map($("#dynatreemap").dynatree("getSelectedNodes"), function (node) {
            return node.data.key;
        });
        var MyTasks = '';
        if (SelectedTasks.length > 0) {
            MyTasks = SelectedTasks.join(",");
        }
       $.removeCookie($scope.CookieIdentifier + "MapTasksTitle");
        $.cookie($scope.CookieIdentifier + "MapTasksTitle", MyTasks, {
            expires: 2000
        });



        //$('#dynatreeregional').dynatree({
        //    checkbox: true,
        //    selectMode: 3,
        //    onSelect: function (flag, node) {
        //        // get selected Tasks
        //        var selectedNodes = node.tree.getSelectedNodes();
        //        var selectedKeys = $.map(selectedNodes, function (node) {
        //            return node.data.key;
        //        });
        //        SelectedRegionalTasks = selectedKeys;
        //        var SelectedTasks = $.map($("#dynatreeregional").dynatree("getSelectedNodes"), function (node) {
        //            return node.data.key;
        //        });
        //        var MyTasks = '';
        //        if (SelectedTasks.length > 0) {
        //            MyTasks = SelectedTasks.join(",");
        //        }
        //        $.removeCookie("MarketTasksTitle");
        //        $.cookie("MarketTasksTitle", MyTasks, {
        //            expires: 2000
        //        });

        //    }
        //});
        //var SelectedTasks = $.map($("#dynatreeregional").dynatree("getSelectedNodes"), function (node) {
        //    return node.data.key;
        //});
        //var MyTasks = '';
        //if (SelectedTasks.length > 0) {
        //    MyTasks = SelectedTasks.join(",");
        //}
        //$.removeCookie("MarketTasksTitle");
        //$.cookie("MarketTasksTitle", MyTasks, {
        //    expires: 2000
        //});


        //$.removeCookie("MarketTasksTitle");
        //$.cookie("MarketTasksTitle", MyTasks, {
        //    expires: 2000
        //});


        //$(".groupMultiSelect").groupMultiSelect({
        //    options: grpOptions,
        //    placeholder: "Tasks"
        //  //  initial: init
        //    //{
        //    //parent: 'TSS',
        //    //children: []
        //    //}
        //    ,

        //    onChange: function (parent, children) {
        //        if (parent != "") {
        //            projecttasks = [];
        //            projecttasks.push(parent);
        //        } for (var i = 0; i < children.length; i++) {

        //            projecttasks.push(children[i]);
        //        }
        //    }



        //});
        var grpOptions = CreateTaskObject(res.data)
        var init = GegInitailTasks("MarketTasksTitle");
        $(".groupMultiSelect2").groupMultiSelect({
            options: grpOptions,
            placeholder: "Reasons",
            //initial: {
            //    parent: 'TSS',
            //    children: []
            //},
            initial: init,
            onChange: function (parent, children) {
                if (parent != "") {
                    markettsks = [];
                    markettsks.push(parent);
                } for (var i = 0; i < children.length; i++) {

                    markettsks.push(children[i]);
                }
            }



        });

        var grpOptions = CreateTaskObject(res.data)
        var init = GegInitailTasks("MapTasksTitle");


        $(".groupMultiSelect3").groupMultiSelect({
            options: grpOptions,
            placeholder: "Reasons",
            initial: init,

            onChange: function (parent, children) {
                if (parent != "") {
                    maptsks = [];
                    maptsks.push(parent);
                } for (var i = 0; i < children.length; i++) {

                    maptsks.push(children[i]);
                }
            }



        });




        var option7 = '';
        //for (var i = 0; i < $scope.ProjectSiteStatus.length; i++) {
        //    option2 += '<option value="' + $scope.ProjectSiteStatus[i].DefinationId + '">' + $scope.ProjectSiteStatus[i].DefinationName + '</option>'
        //}
        var optgroup7 = '';
        for (var i = 0; i < res.data.length; i++) {
            option = '';
            //option7 += '<option value="' + res.data[i].TaskId + '" selected="selected">' + res.data[i].Task + '</option>'
            option7 += '<option value="' + res.data[i].TaskId + '">' + res.data[i].Task + '</option>'
        }
        optgroup7 += '<optgroup label="Milestones">' + option7 + '</optgroup>'
        //  var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Map").multiselect();});<' + '/' + 'script><select class="form-control" id="example-multiple-optgroups_Map">' + option2 + '</select>';
        var txt7 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Map5").multiselect({enableClickableOptGroups: true,includeSelectAllOption: true});});</script><select class="form-control"  id="example-xss-html_Map5" multiple="multiple">' + option7 + '</select>';

        //  $("#ListMapIssues").append(txt7);

        //var options = [];[{ label: '', subItems: [] }];
        //var child = []
        //for (var i = 0; i < res.data.length; i++) {
        //    child = [];
        //    for (var j = 0; j < res.data[i].Tasks.length; j++) {
        //        child.push(res.data[i].Tasks[j].Task);
        //    }
        //    options.push({ label: res.data[i].Task, subItems: child })
        //}


        //$('#ListMapIssues .multiselect .multiselect-selected-text', this.$container).text("All Milestones");



    })

    $scope.Showfileloader = false;
    $scope.Showfileloaderissue = false;
    $scope.Showfileloaderissuejpg = false;
    $scope.Showfileloaderissuefile = false;

    $scope.showFileLoader = function () {

        $scope.Showfileloader = true;
    }
    $scope.ShowfileloaderIssue = function (selected) {
        $scope.SelectedFileForUpload = null;
        $scope.Showfileloaderissuejpg = false;
        $scope.Showfileloaderissuefile = false;
        $scope.TypeSelected = $scope.ItemType.filter(x=>x.DefinationId == selected);
        if ($scope.TypeSelected[0].DefinationName == "Pictures") {
            $scope.Showfileloaderissuejpg = true;

        }
        else if ($scope.TypeSelected[0].DefinationName == "Alarm Log")
            $scope.Showfileloaderissuefile = true;
    }

    $scope.GetIssueType = function () {
        $scope.IssueType = [];
        $http.get('/Defnation/ToList?filter=' + 'PDefinationId' + '&value=173433').then(function (res) {
            $scope.IssueType = res.data;
        })
    }
    $scope.GetTasks = function (id) {
        $scope.Task = [];
        $http.get('/Project/Issue/GetTasks?projectId=' + $("#pId").attr("data-ProjectId") + '&TaskId=' + $scope.Issue.TaskTypeId).then(function (res) {
            $scope.Task = res.data;
        })
    }
    $scope.SiteStatus = {
    }
    $scope.SaveSiteStatus = function () {
        $scope.IsFormSubmited = true;
        $scope.SiteStatus.ProjectSiteId = parseInt(ProjectSiteId);
        FileUploadService.UploadFile($scope.SelectedFileForUpload, $scope.SiteStatus, "Status").then(function (d) {
            angular.element(document.getElementById('widget-grid')).scope().clearfields();
        }, function (e) {
        });





        //$http.post('/Project/ProjectSite/Update', $scope.SiteStatus).then(function (res) {
        //    $scope.SiteStatus = {};
        //    $('#SiteStatus').modal('hide');
        //    $.ajax({
        //        //url: '/Project/Dashboard/GetMilstonesBarchart?ProjectId=' + $("#pId").attr("data-ProjectId") + '&filter=Get_Project_Timeline_Variance',
        //        url: '/Project/Dashboard/TableResult?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + $.cookie('ProjectTasks')
        // + '&LocationIds=' + $.cookie("ProjectMarkets") + '&FromDate=' + $.cookie("FromDate") + '&ToDate=' + $.cookie("ToDate"),

        //        type: 'Get',
        //        async: false,
        //        success: function (data) {


        //            $('#divclientsites').html(data);

        //        }
        //    });

        //})
    }
    function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel) {
        //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
        var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

        var CSV = '';
        //Set Report title in first row or line

        CSV += ReportTitle + '\r\n\n';

        //This condition will generate the Label/Header
        if (ShowLabel) {
            var row = "";

            //This loop will extract the label from 1st index of on array
            for (var index in arrData[0]) {

                //Now convert each value to string and comma-seprated
                row += index + ',';
            }

            row = row.slice(0, -1);

            //append Label row with line break
            CSV += row + '\r\n';
        }

        //1st loop is to extract each row
        for (var i = 0; i < arrData.length; i++) {
            var row = "";

            //2nd loop will extract each column and convert it in string comma-seprated
            for (var index in arrData[i]) {
                row += '"' + arrData[i][index] + '",';
            }

            row.slice(0, row.length - 1);

            //add a line break after each row
            CSV += row + '\r\n';
        }

        if (CSV == '') {
            alert("Invalid data");
            return;
        }

        //Generate a file name
        var fileName = "MyReport_";
        //this will remove the blank-spaces from the title and replace it with an underscore
        fileName += ReportTitle.replace(/ /g, "_");

        //Initialize file format you want csv or xls
        var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

        // Now the little tricky part.
        // you can use either>> window.open(uri);
        // but this will not work in some browsers
        // or you will not get the correct file extension    

        //this trick will generate a temp <a /> tag
        var link = document.createElement("a");
        link.href = uri;

        //set the visibility hidden so it will not effect on your web-layout
        link.style = "visibility:hidden";
        link.download = fileName + ".csv";

        //this part will append the anchor tag and remove it after automatic click
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
    $scope.GetSiteLog = function (id) {
        $scope.siteIdTemp = id;
        $scope.xxx = 0;
        $scope.ProjectDelivery = [];
        $scope.ProjectSiteStatus = [];
        $http.post('/Project/ProjectSite/ToList?Filter=GET_SiteLog&projectId=' + $("#pId").attr("data-ProjectId") + '&SiteId=' + id + '&UserId='+0+'Value1='+'').then(function (res) {
            $scope.ProjectSiteStat = res.data;
            $scope.SiteStatus = res.data[$scope.xxx];
            // $scope.SiteStatus.GNGId = res.data[$scope.xxx].GngId;
            if (res.data.length > 0) {
                if (res.data[0].IsAddionalSite != undefined ){
                    $scope.SiteStatus.IsAddionalSite = res.data[0].IsAddionalSite.toString();
                }
                if (res.data[0].Description != undefined && res.data[0].Description != "" && res.data[0].Description != null) {
                    $scope.SiteStatus.Notes = res.data[0].Description
                }
            }
        })
    }
    //$scope.GetSiteLog();
    function ShowSuccessMessage(text) {
        $("#SuccessText").text(text)
        $("#SuccessMessage").fadeIn(1000);
        $("#SuccessMessage").fadeOut(5000);
    }
    $scope.GetUsers = function (id) {

        $http.get('/Project/Issue/GetUser?projectId=' + $("#pId").attr("data-ProjectId")).then(function (res) {
            $scope.User = res.data;
            //JSONToCSVConvertor(res.data, "Vehicle Report", true);
                   
            if (id != undefined && id != "" && id != null && id != 0) {
            var tmp = $scope.GetUserById(id);
            $scope.multiple.AssignToId.push(tmp);
            $scope.AssignedToIds = id;
        }
        })
    }

    $scope.projectclient = function () {

        $.ajax({
            url: '/Client/ToList',
            type: 'post',
            data: {
                'filter': 'ProjectClients', 'value': $("#pId").attr("data-ProjectId")
            },
            success: function (res) {
                $scope.projectclients = res;

            }
        });

    }
    $scope.projectclients = []
    $scope.projectclient();

    $scope.StagesObject = function (value) {
        for (var i = 0; i < $scope.Stages.length; i++) {
            if ($scope.Stages[i].TaskId == value) {
                return $scope.Stages[i];
            }
        }
    }

    var FD = toDate($.cookie($scope.CookieIdentifier + "FromDate"))

    $scope.GetAllSites = function () {

        $.ajax({
            //url: '/Project/Dashboard/GetMilstonesBarchart?ProjectId=' + $("#pId").attr("data-ProjectId") + '&filter=Get_Project_Timeline_Variance',
            url: '/Project/Dashboard/TableResult?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + $.cookie($scope.CookieIdentifier + 'ProjectTasks')
     + '&LocationIds=' + $.cookie($scope.CookieIdentifier + "SiteProjectMarkets") + '&FromDate=' + $.cookie($scope.CookieIdentifier + "SiteFromDate") + '&ToDate=' + $.cookie($scope.CookieIdentifier + "SiteToDate"),

            type: 'Get',
            async: false,
            success: function (data) {
                $('#divclientsites').html(data);
                SiteGridPagination();
            }
        });
    }
    //    $http.get('/Project/Dashboard/TableResult?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + $.cookie('ProjectTasks')
    //+ '&LocationIds=' + $.cookie("ProjectMarkets") + '&FromDate=' + FromDate+ '&ToDate=' + toDate($.cookie("ToDate"))).then(function (res) {


    //});
    $scope.GetAllIssues = function () {
        //var IssuesStatus = $("#example-getting-started").val()
        //IssuesStatus = IssuesStatus.join(',');
        var RDate = $('#reportrange3').find('.changingSelector').html();
        var Datees = RDate.split('-');
        $.ajax({
            //url: '/Project/Dashboard/GetMilstonesBarchart?ProjectId=' + $("#pId").attr("data-ProjectId") + '&filter=Get_Project_Timeline_Variance',
            url: '/Project/Dashboard/GetProjectIssue?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + $.cookie($scope.CookieIdentifier + "IssueStaus")
     + '&LocationIds=' + $.cookie($scope.CookieIdentifier + "PMIssuesProjectMarkets") + '&FromDate=' + $.cookie($scope.CookieIdentifier + "PMIssuesFromDate") + '&ToDate=' + $.cookie($scope.CookieIdentifier + "PMIssuesToDate"),

            type: 'Get',
            async: false,
            success: function (data) {
                $('#divissues').html(data);

                IssueGridPagination();
            }
        });

        //$http.get('/Project/Dashboard/GetProjectIssue?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + $.cookie('ProjectTasks')
        //    + '&LocationIds=' + $.cookie("ProjectMarkets") + '&FromDate=' + $.cookie("FromDate") + '&ToDate=' + $.cookie("ToDate")).then(function (res) {

        //        $('#divissues').html(res.data);
        //        IssueGridPagination();

        //    });
    }

  , $scope.currentPage = 1
  , $scope.numPerPage = 7
  , $scope.maxSize = 5;

    $scope.GetSiteIssues = function (mid, sid) {
     //  $scope.GetUsers(UserID);
        if (sid == 0) {
            $http.get('/Project/Dashboard/GetDashboardCharts?Filter=Get_WO_Issues&ProjectId=' + $("#pId").attr("data-ProjectId") + '&MilestoneId=' + mid + '&TaskIds=' + mid + '&Value1=' + mid
            + '&LocationIds=' + $.cookie($scope.CookieIdentifier + "ProjectMarkets") + '&FromDate=' + $.cookie($scope.CookieIdentifier + "FromDate") + '&ToDate=' + $.cookie($scope.CookieIdentifier + "ToDate")).then(function (res) {
                $scope.Project_Issue = res.data;
                if (sid == 0) {
                    $scope.psiteid = mid;

                }
              
                for (var i = 0; i < $scope.Project_Issue.length; i++) {

                    $scope.Project_Issue[i].TargetDate = moment.utc($scope.Project_Issue[i].TargetDate).format('DD MMM, YYYY');
                 
                }

                $scope.$watch('currentPage + numPerPage', function () {
                    var begin = (($scope.currentPage - 1) * $scope.numPerPage)
                    , end = begin + $scope.numPerPage;

                    $scope.Filtered_Project_Issue = $scope.Project_Issue.slice(begin, end);
                });

            });
        } else {
            $http.get('/Project/Dashboard/GetDashboardCharts?Filter=Get_WO_Issues&ProjectId=' + $("#pId").attr("data-ProjectId") + '&MilestoneId=' + mid + '&TaskIds=' + mid + '&Value1=' + sid
          + '&LocationIds=' + $.cookie($scope.CookieIdentifier + "ProjectMarkets") + '&FromDate=' + $.cookie($scope.CookieIdentifier + "FromDate") + '&ToDate=' + $.cookie($scope.CookieIdentifier + "ToDate")).then(function (res) {
              $scope.Project_Issue = res.data;

              for (var i = 0; i < $scope.Project_Issue.length; i++) {

                  $scope.Project_Issue[i].TargetDate = moment.utc($scope.Project_Issue[i].TargetDate).format('DD MMM, YYYY');
              }

              $scope.$watch('currentPage + numPerPage', function () {
                  var begin = (($scope.currentPage - 1) * $scope.numPerPage)
                  , end = begin + $scope.numPerPage;

                  $scope.Filtered_Project_Issue = $scope.Project_Issue.slice(begin, end);
              });

          });
        }
          }

  , $scope._currentPage = 1
  , $scope._numPerPage = 10
  , $scope._maxSize = 50;

    $scope.GetSiteWorkLog = function (mid, sid, logType) {
        var dataList = "";
        $scope.WorkLogList = [];
        angular.element(document.getElementById('widget-grid')).scope().setlogdays(worklogdates);
        $http.get('/Project/WorkLog/GetWorkLogList?TaskId=' + mid + '&ProjectSiteId=' + sid + "&LogType=" + logType + "&ProjectId=" + $("#ProjectIdLogs").val()).then(function (res) {
            $scope.CurrentWeek = [];
            if (res.data.length > 0) {
           
                $scope.WorkLogList = res.data;
                var ddddt = $("#workLogHisotryTable").dataTable({
                    "columnDefs": [
                                   { "width": "78px", "targets": 0 },
                                   { "width": "442px", "targets": 2 }
                    ]
                }).api();
                ddddt.clear().draw();
                for (var i = 0; i < $scope.WorkLogList.length; i++) {
                    var _LogDate = moment($scope.WorkLogList[i].LogDate).format('MM-DD-YYYY');
                    var _IsApproved = $scope.WorkLogList[i].IsApproved == true ? 'Aproved' : 'Unaproved';

                    var WLogId = $scope.WorkLogList[i].WLogId;
                    var WLogType = $scope.WorkLogList[i].LogType;
                    var LogDate = moment($scope.WorkLogList[i].LogDate).format('MM-DD-YYYY')
                    var LogHours = $scope.WorkLogList[i].LogHours;
                    var Description = $scope.WorkLogList[i].Description;

                    ddddt.row.add([
                        LogDate,
                        LogHours,
                        Description,
                        _IsApproved,
                        `<span onclick="openWorkLog(` + WLogId + `, ` + "'" + LogDate + "'" + `, ` + "'" + LogHours + "'" + `, ` + "'" + logType + "'" + `,` + "'" + Description + "'" + ` )" class="glyphicon glyphicon-edit"></span>`
                    ]).draw(false);

                    $scope.logdatelst = $scope.logdays.split(',');
                    
                    for (var j = 1; j < $scope.logdatelst.length; j++) {
                        if (moment($scope.logdatelst[j]).format('MM-DD-YYYY') == LogDate) {
                            $scope.obj={
                                date:LogDate,
                                description:Description,
                                isApproved:_IsApproved =="Aproved"? true :false,
                                WLogId: WLogId,
                                WLogType: logType,
                                hours:LogHours
                            }
                            $scope.CurrentWeek.push($scope.obj);
                        } 
                    }
        
                
                }
                $scope.gd=[];
                for (var i = 0; i < $scope.CurrentWeek.length; i++) {
                    $scope.gd.push($scope.CurrentWeek[i].date);
                }
                for (var i = 1; i < $scope.logdatelst.length; i++) {
                    $scope.logdatelst[i] = moment($scope.logdatelst[i]).format('MM-DD-YYYY');
                }

                var res = $scope.logdatelst.filter(function (n) { return !this.has(n) }, new Set($scope.gd));
                for (var i = 1; i < res.length; i++) {
                    
                     $scope.obj = {
                         date: res[i],
                         description: "",
                         isApproved: false,
                         hours: "",
                         WLogType: logType,
                         WLogId: 0
                     }
                     $scope.CurrentWeek.push($scope.obj);
                    
                }
                $scope.CurrentWeek.sort(function (a, b) {
                    // Turn your strings into dates, and then subtract them
                    // to get a value that is either negative, positive, or zero.
                    return new Date(b.date) - new Date(a.date);
                });
                $scope.CurrentWeek.reverse();
                var ghh = $("#workLogHisotryTable");
                $compile(ghh)($scope);
            } else {
                var dt = $("#workLogHisotryTable").dataTable().api();
                dt.clear().draw(false);

                $scope.logdatelst = $scope.logdays.split(',');
                for (var i = 1; i < $scope.logdatelst.length; i++) {
                    $scope.logdatelst[i] = moment($scope.logdatelst[i]).format('MM-DD-YYYY');
                }

                var res = $scope.logdatelst;;
                for (var i = 1; i < res.length; i++) {

                    $scope.obj = {
                        date: res[i],
                        description: "",
                        isApproved: false,
                        hours: "",
                        WLogType: logType,
                        WLogId: 0
                    }
                    $scope.CurrentWeek.push($scope.obj);

                }
                $scope.CurrentWeek.sort(function (a, b) {
                    // Turn your strings into dates, and then subtract them
                    // to get a value that is either negative, positive, or zero.
                    return new Date(b.date) - new Date(a.date);
                });
                $scope.CurrentWeek.reverse();
            }

            currentWeek = [];
            currentWeek = angular.element($('[ng-controller="DashboardCtrl"]')).scope().CurrentWeek;
            $(".current-week").html("");
            $.each(currentWeek, function (i, e) {
                let date = moment(currentWeek[i].date);
                date = new Date(date);
                date = date.getDate();
                $(".current-week").append(`<button type="button" class="btn btn-xs btn-default" data-WLogId="${currentWeek[i].WLogId}"data-WLogType="${currentWeek[i].WLogType}" data-description="${currentWeek[i].description}" data-hours="${currentWeek[i].hours}" data-approved="${currentWeek[i].isApproved}" data-fulldate="${currentWeek[i].date}">${date}</button>`)
            });


        });

    }

    $scope.GetSelectedSiteWorkLog = function (days,Typeee) {
        $scope.days = days;
        $scope.selectedDates = [];
        for (var i = 0; i < $scope.WorkLogList.length; i++) {
            var _LogDate = moment($scope.WorkLogList[i].LogDate).format('MM-DD-YYYY');
            var _IsApproved = $scope.WorkLogList[i].IsApproved == true ? 'Aproved' : 'Unaproved';

            var WLogId = $scope.WorkLogList[i].WLogId;
            var WLogType = $scope.WorkLogList[i].LogIdType;
            var LogDate = moment($scope.WorkLogList[i].LogDate).format('MM-DD-YYYY')
            var LogHours = $scope.WorkLogList[i].LogHours;
            var Description = $scope.WorkLogList[i].Description;
           

            for (var j = 0; j < $scope.days.length; j++) {
                $scope.obj = {};
                if (moment($scope.days[j]).format('MM-DD-YYYY') == LogDate) {
                    $scope.obj = {
                        date: LogDate,
                        description: Description,
                        isApproved: _IsApproved == "Aproved" ? true : false,
                        WLogId: WLogId,
                        WLogType: Typeee,
                        hours: LogHours
                    }
                    $scope.selectedDates.push($scope.obj);
                }
            }
        }
            $scope.gy = [];
            for (var i = 0; i < $scope.selectedDates.length; i++) {
                $scope.gy.push($scope.selectedDates[i].date);
            }
            for (var i = 0; i < $scope.days.length; i++) {
                $scope.days[i] = moment($scope.days[i]).format('MM-DD-YYYY');
            }

            var res = $scope.days.filter(function (n) { return !this.has(n) }, new Set($scope.gy));
            for (var i = 0; i < res.length; i++) {

                $scope.obj = {
                    date: res[i],
                    description: "",
                    isApproved: false,
                    hours: "",
                    WLogType: Typeee,
                    WLogId: 0
                }
                $scope.selectedDates.push($scope.obj);

            }
            $scope.selectedDates.sort(function (a, b) {
                // Turn your strings into dates, and then subtract them
                // to get a value that is either negative, positive, or zero.
                return new Date(b.date) - new Date(a.date);
            });
            $scope.selectedDates.reverse();
            selectedDates = [];
            selectedDates = angular.element($('[ng-controller="DashboardCtrl"]')).scope().selectedDates;
        
    }


    $scope._currentPageTask = 1;
    $scope._numPerPageTask = 7;
    $scope._maxSizeTask = 5;
    // #region Task Entries
    $scope.taskEntrySiteId = 0
    $scope.getSavedFormTemplate = function (_taskId, _siteId) {
        $.ajax({
            url: '/Project/Template/GetFormBuilderRenderedHTML?FormTypeId=Milestone&NodeId=' + _taskId + '&TemplateId=""&SiteId=' + _siteId,
            type: 'Get',
            async: false,
            // dataType: 'text',
            //   processData: false,
            success: function (data) {
                $('#CustomForm').html(data.FormHtml);

                $('#FormModal').modal({
                    show: 'true'
                });
                if (data == "") {
                }
                else {
                    milestone = data;
                }
            }
        });
    }
    $('#frmTaskEntry').submit(function () {
        var TaskEntry = [];
        $(".frm-dynamic").each(function () {
            TaskEntry.push({ FormId: $(this).attr('data-formid'), ProjectSiteId: $scope.taskEntrySiteId, TaskId: TskID, FormValue: $(this).val(), ProjectId: $("#pId").attr("data-ProjectId"), Comments: $('#sortOrderInputComments' + $(this).attr("data-formid")).val() });
            log($(this).val());
            log($(this).attr('data-formid'));
        });
        log(TaskEntry);
        $.ajax({
            url: '/Project/Task/TaskEntry',
            type: 'post',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(TaskEntry),

            success: function (res) {
               $scope.getSavedFormTemplate(TskID, $scope.taskEntrySiteId)
                swal("Saved !", "", "success");
                $scope.GetTaskEntries(0,TskID);
             //   angular.element(document.getElementById('widget-grid')).scope().GetTaskEntries($('#siteId').attr('data-siteId'), TskID);
            },
            error: function (e) {
                $scope.getSavedFormTemplate(TskID, $scope.taskEntrySiteId)
                swal("Saved !", "", "success");
               $scope.GetTaskEntries(0,TskID)
              //  angular.element(document.getElementById('widget-grid')).scope().GetTaskEntries($('#siteId').attr('data-siteId'), TskID);
            }
        });
        return false;
    });
    $scope.GetTaskEntries = function (_SiteId, _TaskId) {
        $http.get('/Project/Template/GetTaskEntries?&SiteId=' + $scope.taskEntrySiteId + "&TaskId=" + _TaskId).then(function (res) {
            $scope.TaskEntries = [];
            $scope.TaskEntries = res.data.Data;
            $scope.TaskEntriesTitles = [];
             let Count = 0;
            for (var i = 0; i < $scope.TaskEntries.length; i++) {
                 if ($scope.TaskEntries[i].CurrentRevision.length > Count) {
                    $scope.TaskEntriesTitles = $scope.TaskEntries[i].CurrentRevision;
                    Count = $scope.TaskEntries[i].CurrentRevision.length;

                }
            }
           $scope.$watch('_currentPageTask + _numPerPageTask', function () {
                var _begin = (($scope._currentPageTask - 1) * $scope._numPerPageTask)
                , _end = _begin + $scope._numPerPageTask;

                $scope.Filtered_TaskEntries = $scope.TaskEntries.slice(_begin, _end);
            });

        });
    }
    $(document).on("click", ".customfrm", function (e) {
        var TskNme = $(this).parent().siblings("td:eq(1)").text();
            FillLabel2($('#lablevalues').attr('data-FACode'), $('#lablevalues').attr('data-SiteName'), TskNme);
        var TaskId = parseInt($(this).attr('data-Task'))
        $scope.taskEntrySiteId = $(this).attr('data-siteId');
        TskID = TaskId;
        $scope.getSavedFormTemplate(TskID, $scope.taskEntrySiteId)
        $scope.GetTaskEntries($(this).attr('data-siteId'), TskID);
    });

    $(document).ready(function () {
        $(document).on("change", ".childTaskStages", function () {
            var count = 0;
            var changedStage = $(this).val();
            $(".childTaskStages").each(function () {
                if (changedStage == $(this).val()) {
                    count++;
                }
            });
            if ($(".childTaskStages").length == count++) {
                $('.planningTaskStages').prop("disabled", false);
                var changedStageType = typeof changedStage;
                var changedStageId;
                if (changedStageType == "string") {
                    if (changedStage.split(":").length > 0)
                        changedStageId = changedStage.split(":")[1];
                }
                $scope.Milstone.TaskStageId = parseInt(changedStageId);
                $('.planningTaskStages option[value="' + changedStage + '"]').prop('selected', true);
                
                $('.planningTaskStages').prop("disabled", true);
            } else {
                $('.planningTaskStages').prop("disabled", false);
            }
        });
    });

    // #endregion Task Entries
    $("#PlanningModal").on('shown.bs.modal', function () {
        if ($scope.Stages.length > 0) {
            $("#PlannedDate").datepicker('disable');
            $("#TargetDatee").datepicker('disable');
            $("#ForecastStartDate").datepicker('disable');
            $("#ForecastEndDate").datepicker('disable');
            $("#ActualStartDate").datepicker('disable');
            $("#ActualEndDate").datepicker('disable');
            $(".MilestoneStatus").val($("#MyStatus").val());
            }
        else {
            $("#PlannedDate").datepicker('enable');
            $("#TargetDatee").datepicker('enable');
            $("#ForecastStartDate").datepicker('enable');
            $("#ForecastEndDate").datepicker('enable');
            $("#ActualStartDate").datepicker('enable');
            $("#ActualEndDate").datepicker('enable');
            $(".MilestoneStatus").val($("#MyStatus").val());
}
        $('.MilestoneStatus option[value="' + $("#MyStatus").val() + '"]').prop('selected', true);
    });
    $scope.getStages = function (p, s, t, StId, AssignTo, _Current,tStageid) {
        document.getElementById("TrackerTaskID").value = t;
        //$("#").val(t);
        $http.get('/Project/Dashboard/GetStages?ProjecId=' + $("#pId").attr("data-ProjectId") + '&MilestoneId=' + t + '&SiteId=' + s).then(function (res) {
            $('.planningTaskStages').prop("disabled", false);
            $scope.Milstone = stages;
            $scope.Milstone.ate = stages.ForecastEndDate
            $scope.Milstone.SiteTaskId = t;
            $scope.Milstone.ProjectSiteId = s;
            $scope.Milstone.Current = _Current;
            $scope.Milstone.ActualStartDate = stages.ActualStartDate
            $scope.Milstone.ActualEndDate = stages.ActualEndDate
            $scope.Milstone.ForecastStartDate = stages.ForecastStartDate
            $scope.Milstone.ForecastEndDTitle = stages.Title;;
            if (AssignTo != "") {
                var ss = AssignTo.split(',').map(Number);;
                $scope.Milstone.AssignTo = ss;
            }
            //$(".Assigned").val($("#Assigned").val());
            //$scope.Milstone.AssignTo = AssignTo;
            //var abc = $(".TypeSelected option:selected").text();
            //if (abc != "Stage") {
            //    $scope.isMilestoneEdit = true;
            //}
            $scope.isMilestoneEdit = true;
            $scope.Milstone.StatusId = "number:" + StId;
            
        
            $scope.Stages = res.data;
            if (res.data != "undefined" && res.data != null) {
                if (res.data.length != "0") {
                    for (var i = 0; i < $scope.Stages.length; i++) {
                        var Data = $scope.WorkFlowListReturn(res.data[i].ProjectId, res.data[i].TId);
                        
                        if (Data != undefined && Data != "undefined") {
                            $scope.Stages[i].TaskStagesList = Data;
                        } else {
                            $scope.Stages[i].TaskStagesList = [];
                        }
                    }
                }

            }
            $scope.TaskChildLength = 0;
           
            if (res.data != "undefined" && res.data != null) {
                if (res.data.length != "0") {
                    $scope.TaskChildLength = res.data.length;
                    console.log(Boolean("Boolean(JSON.stringify(res.data[0].Stage))   :", JSON.stringify(res.data[0].Stage)))
                    if (Boolean(JSON.stringify(res.data[0].Stage))) {

                        //$("#PlannedDate").datepicker('disable');
                        //$("#TargetDatee").datepicker('disable');
                        //$("#ForecastStartDate").datepicker('disable');
                        //$("#ForecastEndDate").datepicker('disable');
                        //$("#ActualStartDate").datepicker('disable');
                        //$("#ActualEndDate").datepicker('disable');
                           
                    }
                    for (var i = 0; i < $scope.Stages.length; i++) {
                        $scope.Stages[i].isEdit = true;
                        if ($scope.Stages[i].AssignTo != "") {
                            var ss = $scope.Stages[i].AssignTo.split(',').map(Number);
                            $scope.Stages[i].AssignTo = ss;
                        }

                        $scope.Stages[i].ProjectSiteId = s;
                        $scope.Stages[i].SiteTaskId = t;
                        $scope.Stages[i].ActualStartDate = moment($scope.Stages[i].ActualStartDate, 'DD/MM/YYYY').format('MM/DD/YYYY');
                        $scope.Stages[i].ActualEndDate = moment($scope.Stages[i].ActualEndDate, 'DD/MM/YYYY').format('MM/DD/YYYY');
                        $scope.Stages[i].ForecastStartDate = moment($scope.Stages[i].ForecastStartDate, 'DD/MM/YYYY').format('MM/DD/YYYY');
                        $scope.Stages[i].ForecastEndDate = moment($scope.Stages[i].ForecastEndDate, 'DD/MM/YYYY').format('MM/DD/YYYY');
                        $scope.Stages[i].PlannedDate = moment($scope.Stages[i].PlanDate, 'DD/MM/YYYY').format('MM/DD/YYYY');
                        $scope.Stages[i].PlanDate = moment($scope.Stages[i].PlanDate, 'DD/MM/YYYY').format('MM/DD/YYYY');
                        $scope.Stages[i].TargetDate = moment($scope.Stages[i].TargetDate, 'DD/MM/YYYY').format('MM/DD/YYYY');
                    }
                }
                else {
                   
                }
            }
            $(".MilestoneStatus").val($("#MyStatus").val());
            $('.MilestoneStatus option[value="' +$("#MyStatus").val() + '"]').prop('selected', true);
        })
        setTimeout(
          function () {
              $scope.Milstone.TaskStageId = "number:" + tStageid;
              $('.planningTaskStages option[value="number:' + tStageid + '"]').prop('selected', true);
             
          }, 4500);
       
    }

    $scope.checkPlanType = function () {
        //$scope.flag = $scope.flag.replace(/\s/g, '');
        if ($scope.flag.name == 'Miletone') {
            $scope.isMilestoneEdit = true;
            $scope.isStageEdit = false;
            setTimeout(function () { $(".MilestoneStatus").val($("#MyStatus").val()); }, 1000)
        }
        if ($scope.flag.name == 'Stage') {
            $scope.isStageEdit = true;
            $scope.isMilestoneEdit = false;
            setTimeout(function () { $(".MilestoneStatus").val($("#MyStatus").val()); }, 1000)
        }
    }

    $scope.MilestoneEdit = function (index) {
        $scope.Stages[index].isEdit = true;
        //$scope.isEdit = true;
    }
    $scope.SavePlan = function (index, obj, CurrentChilds) {

        //  return false;
        if (obj != null && obj != undefined) {
            obj.ActualStartDate = $("#ActualStartDate").val();
            obj.ActualEndDate = $("#ActualEndDate").val();
            obj.PlannedDate = $("#PlannedDate").val();
            obj.ForecastStartDate = $("#ForecastStartDate").val();
            obj.ForecastEndDate = $("#ForecastEndDate").val();
            obj.TargetDate = $("#TargetDatee").val();
           
            var typecheckStatusTaskStageId = typeof obj.TaskStageId;
            if (typecheckStatusTaskStageId == "string") {
                if (obj.TaskStageId.split(":").length > 0)
                    obj.TaskStageId = obj.TaskStageId.split(":")[1];
            }

            var typecheck = typeof obj.AssignTo;
            if (typecheck == "object")
                obj.AssignTo = obj.AssignTo.join(',');


            var typecheckStatus = typeof obj.StatusId;
            if (typecheckStatus == "string") {
                if (obj.StatusId.split(":").length > 0)
                    obj.StatusId = obj.StatusId.split(":")[1];
            }
         //   obj.ProjectSiteId = $('#siteId').attr('data-siteId');
            var Myarray = CurrentChilds;
            for (var i = 0; i < CurrentChilds.length; i++) {
                var Stgs = {};
                Myarray[i].ActualStartDate = $("[data-indexer=" + i + "_ActualStartDate]").val();
                Myarray[i].ActualEndDate = $("[data-indexer=" + i + "_ActualEndDate]").val();
                Myarray[i].PlannedDate = $("[data-indexer=" + i + "_PlannedDate]").val();
                Myarray[i].ForecastStartDate = $("[data-indexer=" + i + "_ForecastStartDate]").val();
                Myarray[i].ForecastEndDate = $("[data-indexer=" + i + "_ForecastEndDate]").val();
                Myarray[i].TargetDate = $("[data-indexer=" + i + "_TargetDate]").val();
                Myarray[i].SiteTaskId = Myarray[i].TaskId; 
                Myarray[i].TaskStageId = Myarray[i].TaskStageId;
                var typecheck = typeof Myarray[i].AssignTo;
                if (typecheck == "object")
                    Myarray[i].AssignTo = Myarray[i].AssignTo.join(',');

                var typecheckStatus = typeof Myarray[i].StatusId;
                if (typecheckStatus == "string") {
                    if (Myarray[i].StatusId.split(":").length > 0)
                        Myarray[i].StatusId = Myarray[i].StatusId.split(":")[1];
                }
              //  Myarray[i].ProjectSiteId = $('#siteId').attr('data-siteId');
            }
            Myarray.push(obj);
            $http.post('/Project/Task/PlanningNew', Myarray).then(function () {
                $('#PlanningModal').modal('hide');
                ShowSuccessMessage("Save Plan Successfully");

                var temp;
                temp = format(parseInt($("#pId").attr("data-ProjectId")), obj.PTaskId, obj.ProjectSiteId); //$('#siteId').attr('data-siteId')

                if (parseInt(obj.PTaskId) > 0) {
                    obj.Current.parent().parent().parent().parent().parent().parent().parent().prev().after("<tr> <td colspan='12' style='width:100%'>" + temp + "</td></tr>");
                    obj.Current.parent().parent().parent().parent().parent().parent().parent().remove();
                }
                else {
                    obj.Current.parent().parent().parent().parent().parent().parent().parent().prev().after("<tr> <td colspan='17' style='width:100%'>" + temp + "</td></tr>");
                    obj.Current.parent().parent().parent().parent().parent().parent().parent().remove();
                }
                //  obj.Current.parent().parent().parent().parent().parent().parent().parent().remove();

            })
        }
        // if ($scope.Stages.length > 0)
        //   $scope.Stages[index].isEdit = false;
        //$scope.isEdit = true;
    }
    $scope.clearfields = function () {
        $scope.Issue = {
        };
        $scope.workLog = {
        };
        $scope.IssueLog = {
        };
        $scope.IssueLog = {
        };
        $scope.SiteStatus = {
        };
        $scope.SelectedStatus = '';
        $scope.SelectedPriority = '';
        $scope.StageStatusId = null;
        $scope.statusChange = {
        };
        $scope.Milstone = {
        };
        $scope.multiple.AssignToId = {
        };
        $scope.Description = '';
        $scope.LogDate = '';
        $scope.LogHours = '';
        $scope.Description = '';

    }

    function toDate(selector) {
        if (selector != null) {
            var from = selector.split("/");
            return new Date(from[2], from[1] - 1, from[0]);
        }
    }

    $http.post('/Defnation/ToList?filter=byDefinationType&value=Project Status').then(function (res) {
        $scope.ProjectStatus = res.data;
        log(res);
    })

    $http.post('/Defination/GetTaskStages?ProjectId=' + parseInt($("#pId").attr("data-ProjectId"))).then(function (res) {
        $scope.TaskStagesList = [];
    })

    $scope.WorkFlowList = function (ProjectId, TaskId, taskStageId) {
        $http.post('/Defination/GetTaskOrProjectStages?ProjectId=' + ProjectId + "&TaskId=" + TaskId).then(function (res) {
            var response = res.data;
            var list = [];
            for (var i = 0; i < res.data.length; i++) {
                
                list.push({ StageId: response[i].StageId, Title: response[i].Title, Selected: false });
            }
            $scope.TaskStagesList = list;
            $scope.Milstone.TaskStageId = taskStageId;
        })
    }

    $scope.WorkFlowListReturn = function (ProjectId, TaskId) {
        var list = [];
        if (TaskId == null || TaskId == "undefined" || TaskId == undefined) {
            TaskId = 0;
        }
        $http.post('/Defination/GetTaskOrProjectStages?ProjectId=' + ProjectId + "&TaskId=" + TaskId).then(function (res) {
            var response = res.data;
            for (var i = 0; i < res.data.length; i++) {

                list.push({ StageId: response[i].StageId, Title: response[i].Title, Selected: false });
            }
        })
        return list;
    }

    $scope.fnStageStatusId = function () {
        $scope.Description = true;
        var obj = $scope.StagesObject($scope.StageStatusId);
        $scope.statusChange = obj;
        $scope.statusChange.ActualStartDate = obj.ActualStartDate;
        $scope.statusChange.ForecastEndDate = obj.ForecastEndDate;
        $scope.statusChange.PlannedDate = obj.PlanDate;
        $scope.statusChange.TargetDate = obj.TargetDate;
        $scope.statusChange.Stage = obj.Stage;
        $scope.statusChange.TaskId = obj.TaskId;
    }
    $scope.SaveStatus = function () {
        $scope.statusChange.ProjectSiteId = $('#siteId').attr('data-siteId');
        $scope.statusChange.SiteTaskId = TaskID;
        $scope.statusChange.ActualStartDate = toDate($scope.statusChange.ActualStartDate);
        $scope.statusChange.ForecastEndDate = toDate($scope.statusChange.ForecastEndDate);
        $scope.statusChange.PlannedDate = toDate($scope.statusChange.PlannedDate);
        $scope.statusChange.TargetDate = toDate($scope.statusChange.TargetDate);
        $http.post('/Project/Task/StatusChange', $scope.statusChange).then(function (res) {
            $('#ChangeStatus').modal('hide');
            ShowSuccessMessage("Save Status Successfully");
            $scope.fnStageStatusId();
        })
    }

    $scope.GetProjectSites = function () {
        $http.get('/project/Todo/GetSitesList?ProjectId=' + parseInt($("#pId").attr("data-ProjectId"))).then(function (result) {
            $scope.SitesList  = result.data;
        });
       
    }

    $(document).on('click', 'button[data-target="#TodoModal"]', function () {
        // $scope.Todo.SiteId = 0;
        $(".projectsitedp").val('');
        $(".projectsitetaskdp").val('');
        $scope.multiple.AssignToId = [];
        $scope.GetUsers(UserID);
        $scope.AssignedToIds.push(UserID);
     
    });

    $scope.GetProjectSitesTasks = function (SiteId) {
        $scope.SiteTask = [];
        $http.get('/project/Todo/GetSitesTask?ProjectSiteId=' + parseInt(SiteId)).then(function (result) {
            $scope.SiteTask = result.data;
        });
    }

    $scope.getTodo = function () {
        $scope.GetUsers();
        $scope.assignids = [];
        $scope.multiple.AssignToId = [];
        var checkuser = false;

        $http.get('/project/Todo/Index?ProjectId=' + parseInt($("#pId").attr("data-ProjectId")) + '&DateRange=' + $("#ToDoDateRange").html()).then(function (result) {
            for (var i = 0; i < result.data.length; i++) {
                $scope.Resources = "";
                if (result.data[i].AssignedToIds != "") {
                    $scope.assignids = result.data[i].AssignedToIds.split(',');
                    for (var j = 0; j < $scope.assignids.length; j++) {
                        if ($scope.assignids[j] == $('#UserId').attr('data-UserId')) {
                            checkuser = true;
                        }
                        var obbj = $scope.GetUserById($scope.assignids[j]);
                        if (obbj != null && obbj != undefined) {
                            $scope.Resources = $scope.Resources + obbj.UserName + " , ";
                        }
                    }
                }
                if (checkuser == true) {
                    var obj = {
                        toDoDateTime: moment(result.data[i].ToDoDateTime).format('MM/DD/YYYY'), title: result.data[i].Type, Resources: $scope.Resources, toDoTtitle: result.data[i].ToDoTitle, description: result.data[i].Description, start: new Date(moment.utc(result.data[i].ToDoDateTime)), SiteName: result.data[i].SiteName, TaskName: result.data[i].TaskName, className: ["event", "bg-color-greenLight"], icon: 'fa-check',
                    }
                    $('#calendar').fullCalendar('renderEvent', obj, true);
                    if (result.data[i].Status == 'Important') {
                        $scope.Important.push({
                            toDoTtitle: result.data[i].ToDoTitle, AssignedToIds: result.data[i].AssignedToIds, Resources: $scope.Resources, TaskId: result.data[i].TaskId, SiteId: result.data[i].SiteId, TodoId: result.data[i].TodoId, Type: result.data[i].Type, Status: result.data[i].Status, Description: result.data[i].Description, CreatedOn: moment.utc(result.data[i].CreatedOn).format('YYYY-MM-DD HH:mm:ss A'), ToDoDateTime: moment(result.data[i].ToDoDateTime).format('MM/DD/YYYY'), SiteName: result.data[i].SiteName, TaskName: result.data[i].TaskName
                        });
                    }
                    else if (result.data[i].Status == 'Crititcal') {
                        $scope.Critical.push({
                            toDoTtitle: result.data[i].ToDoTitle, AssignedToIds: result.data[i].AssignedToIds, Resources: $scope.Resources, TaskId: result.data[i].TaskId, SiteId: result.data[i].SiteId, TodoId: result.data[i].TodoId, Type: result.data[i].Type, Status: result.data[i].Status, Description: result.data[i].Description, CreatedOn: moment.utc(result.data[i].CreatedOn).format('YYYY-MM-DD HH:mm:ss A'), ToDoDateTime: moment(result.data[i].ToDoDateTime).format('MM/DD/YYYY'), SiteName: result.data[i].SiteName, TaskName: result.data[i].TaskName
                        });
                    }
                    else if (result.data[i].Status == 'Completed') {
                        $scope.Completed.push({
                            toDoTtitle: result.data[i].ToDoTitle, AssignedToIds: result.data[i].AssignedToIds, Resources: $scope.Resources, TaskId: result.data[i].TaskId, SiteId: result.data[i].SiteId, TodoId: result.data[i].TodoId, Type: result.data[i].Type, Status: result.data[i].Status, Description: result.data[i].Description, CreatedOn: moment.utc(result.data[i].CreatedOn).format('YYYY-MM-DD HH:mm:ss A'), ToDoDateTime: moment(result.data[i].ToDoDateTime).format('MM/DD/YYYY'), SiteName: result.data[i].SiteName, TaskName: result.data[i].TaskName
                        });
                    }
                }
            }
            
        });
    }

    $scope.RefreshToDoTask = function () {
       $http.get('/project/Todo/Index?ProjectId=' + parseInt($("#pId").attr("data-ProjectId")) + '&DateRange=' + $("#ToDoDateRange").html()).then(function (result) {
            $('#calendar').fullCalendar('removeEvents');
            $scope.Important = [];
            $scope.Critical = [];
            $scope.Completed = [];
            $scope.assignids = [];
            var checkuser =false;
            for (var i = 0; i < result.data.length; i++) {

                $scope.Resources = "";
                if (result.data[i].AssignedToIds !=""){
                    $scope.assignids = result.data[i].AssignedToIds.split(',');
                    for (var j = 0; j < $scope.assignids.length; j++) {
                        if ($scope.assignids[j] == $('#UserId').attr('data-UserId')) {
                            checkuser = true;
                        }
                    var obbj = $scope.GetUserById($scope.assignids[j]);
                    $scope.Resources = $scope.Resources + obbj.UserName + " , ";
                }
                }
                if (checkuser == true) {
                    console.log(result.data[i].ToDoTitle + " }}} " + moment.utc(result.data[i].ToDoDateTime).format('MM/DD/YYYY'));
                var obj = {
                    toDoDateTime: moment(result.data[i].ToDoDateTime).format('MM/DD/YYYY'), title: result.data[i].Type, Resources: $scope.Resources, toDoTtitle: result.data[i].ToDoTitle, description: result.data[i].Description, start: new Date(moment.utc(result.data[i].ToDoDateTime)), SiteName: result.data[i].SiteName, TaskName: result.data[i].TaskName, className: ["event", "bg-color-greenLight"], icon: 'fa-check',
                }
                $('#calendar').fullCalendar('renderEvent', obj, true);
                if (result.data[i].Status == 'Important') {
                    $scope.Important.push({
                        toDoTtitle: result.data[i].ToDoTitle, TodoId: result.data[i].TodoId, AssignedToIds: result.data[i].AssignedToIds, TaskId: result.data[i].TaskId, SiteId: result.data[i].SiteId, Type: result.data[i].Type, Resources: $scope.Resources, Status: result.data[i].Status, Description: result.data[i].Description, CreatedOn: moment.utc(result.data[i].CreatedOn).format('YYYY-MM-DD HH:mm:ss A'), ToDoDateTime: moment(result.data[i].ToDoDateTime).format('MM/DD/YYYY'), SiteName: result.data[i].SiteName, TaskName: result.data[i].TaskName
                    });
                }
                else if (result.data[i].Status == 'Crititcal') {
                    $scope.Critical.push({
                        toDoTtitle: result.data[i].ToDoTitle, TodoId: result.data[i].TodoId, AssignedToIds: result.data[i].AssignedToIds, TaskId: result.data[i].TaskId, SiteId: result.data[i].SiteId, Type: result.data[i].Type, Resources: $scope.Resources, Status: result.data[i].Status, Description: result.data[i].Description, CreatedOn: moment.utc(result.data[i].CreatedOn).format('YYYY-MM-DD HH:mm:ss A'), ToDoDateTime: moment(result.data[i].ToDoDateTime).format('MM/DD/YYYY'), SiteName: result.data[i].SiteName, TaskName: result.data[i].TaskName
                    });
                }
                else if (result.data[i].Status == 'Completed') {
                    $scope.Completed.push({
                        toDoTtitle: result.data[i].ToDoTitle, TodoId: result.data[i].TodoId, AssignedToIds: result.data[i].AssignedToIds, TaskId: result.data[i].TaskId, SiteId: result.data[i].SiteId, Type: result.data[i].Type, Resources: $scope.Resources, Status: result.data[i].Status, Description: result.data[i].Description, CreatedOn: moment.utc(result.data[i].CreatedOn).format('YYYY-MM-DD HH:mm:ss A'), ToDoDateTime: moment(result.data[i].ToDoDateTime).format('MM/DD/YYYY'), SiteName: result.data[i].SiteName, TaskName: result.data[i].TaskName
                    });
                }
                }
            }
            $('#calendar').fullCalendar('refresh');
        });
    }

    $scope.SaveTodo = function () {
        $scope.submitted = true;
        if ($('#datepickerInput').val() != null && $('#datepickerInput').val() != "" && $('#datepickerInput').val() != "undefined") {
            $scope.Todo.ToDoDateTime = $('#datepickerInput').val();
        }
        //  $scope.Todo.ToDoDateTime = null;
        if ($scope.AssignedToIds != null) {
          
                $scope.Todo.AssignedToIds = $scope.AssignedToIds
                $scope.AssignedToIds = null;
            } else {
                $scope.Todo.AssignedToId = 0
            }
      
        
        $scope.Todo.ProjectId = $('#TodoProjectId').val();
        $http.post('/project/Task/SaveTodo', $scope.Todo).then(function (result) {
            if (result.data == "success") {
                swal("Success", "Todo Task added successfully", "success");
                $scope.Todo = {
                };
                $("#TodoModal").modal('hide');
            } else {
                swal("Error", "There is an error. Please try again!", "error");
            }
            $scope.submitted = false;
        });
        $scope.RefreshToDoTask();
        
    }
    $scope.clearMsgs = function () {
        $scope.Alert = '';
    }
    $scope.EditToDo = function (x) {
        $scope.Alert = '';
        $scope.Todo = {
            TodoId: x.TodoId,
            ToDoTitle: x.toDoTtitle,
            Description: x.Description,
            Status: x.Status,
            Type: x.Type,
            ToDoDateTime: x.ToDoDateTime,
            SiteId:x.SiteId ,
            TaskId: x.TaskId,
            AssignedToIds:x.AssignedToIds
        };
        $scope.multiple.AssignToId = [];
        $scope.AssignedToId = [];
        if ($scope.Todo.AssignedToIds !=""){
        $scope.AssignedToId = $scope.Todo.AssignedToIds.split(',');

        for (var i = 0; i < $scope.AssignedToId.length; i++) {
            var tmp = $scope.GetUserById($scope.AssignedToId[i]);
            $scope.multiple.AssignToId.push(tmp);
        }
        $scope.fnProjectResource();
        }

        $scope.GetProjectSitesTasks($scope.Todo.SiteId);
        setTimeout(function () {
            $scope.Todo.TaskId = x.TaskId;
            $scope.$digest();
        }, 2000);
        var stdat = ($scope.Todo.ToDoDateTime).toString();
        $('#datepickerInput').val(stdat);
        //$('#Editdatepicker').datetimepicker({
        //    defaultDate: stdat,
        //});
        $('#EditTodoModal').modal('show');
    }

    $scope.PostEditToDo = function () {
        $scope.submitted = true;
       // $scope.AssignedToIds = $scope.multiple.AssignToId.join(',');
        if ($scope.AssignedToIds.length > 0 && $scope.AssignedToIds !=null) {
            $scope.Todo.AssignedToIds = $scope.AssignedToIds
        //    $scope.AssignedToIds = null;
        } else {
            $scope.Todo.AssignedToId = null
        }
        if ($('#datepickerInput').val() != null && $('#datepickerInput').val() != "" && $('#datepickerInput').val() != "undefined") {
            $scope.Todo.ToDoDateTime = $('#datepickerInput').val();
        }
        $http.post('/project/Task/PostEditToDo', $scope.Todo).then(function (result) {
            if (result.data == "success") {
                swal("Success", "Todo Task updated successfully", "success");
                $scope.Todo = {
                };
                $("#EditTodoModal").modal('hide');
            } else {
                swal("Error", "There is an error. Please try again!", "error");
            }
        });
        $scope.RefreshToDoTask();
    }

    $scope.DeleteTodo = function (obj, index, type) {

        obj.Status = 'Completed'
        if (type == 'Critical') {
            $scope.Critical.splice(index, 1);
        }
        if (type == 'Important') {
            $scope.Important.splice(index, 1);
        }
        $http.post('/project/Task/SaveTodo', obj).then(function (result) {
            $scope.Todo = {
            };
        });
    }

    $scope.FilterbyDateandTime = function (SearchFilter, filter, type) {


        if (type == 'Issue') {
            $.ajax({
                url: '/Project/Dashboard/GetDashboardCharts?Filter=' + filter + '&ProjectId=' + parseInt($("#pId").attr("data-ProjectId")) + '&Value1=' + MarketId + '&TaskIds=' + $.cookie($scope.CookieIdentifier + "ProjectTasks") + '&LocationIds=' + $.cookie($scope.CookieIdentifier + "ProjectMarkets")
                + '&FromDate=' + $.cookie($scope.CookieIdentifier + "IssueFromDate") + '&ToDate=' + $.cookie($scope.CookieIdentifier + "IssueToDate") + '&SearchFilter=' + SearchFilter,
                type: 'Get',
                async: false,
                success: function (data) {

                    if (type == 'Issue') {
                        var res = StackBarchartData(data, SearchFilter);
                        loadIssueStackBarchart("Issue_By_pie", res)
                    }
                    else {
                        var res = ChartGroupData(data, SearchFilter);
                        loadChart("chartContainer", res.Actual, res.Planned, res.Forecast, res.Target)
                    }
                }
            });
        }
        else {

            $.ajax({
                url: '/Project/Dashboard/GetDashboardCharts?Filter=' + filter + '&ProjectId=' + parseInt($("#pId").attr("data-ProjectId")) + '&Value1=null&TaskIds=' + $.cookie($scope.CookieIdentifier + "ProjectTasks") + '&LocationIds=' + $.cookie($scope.CookieIdentifier + "ProjectMarkets")
                + '&FromDate=' + $.cookie($scope.CookieIdentifier + "FromDate") + '&ToDate=' + $.cookie($scope.CookieIdentifier + "ToDate") + '&SearchFilter=' + SearchFilter,
                type: 'Get',
                async: false,
                success: function (data) {
                    if (type == 'Issue') {
                        var res = StackBarchartData(data, SearchFilter);
                        loadIssueStackBarchart("Issue_By_Geo", res)
                    }
                    else {
                        var res = ChartGroupData(data, SearchFilter);
                        loadChart("chartContainer", res.Actual, res.Planned, res.Forecast, res.Target, "300")
                    }
                }
            });
        }
    }

    var cookie = $.cookie($scope.CookieIdentifier + "BookMark");
    //cookie.splice(0, 1);
    var items = cookie ? cookie.split(/,/) : new Array();
    $scope.BookmarkTasks = [];
    var count = -1;

    //$http.get('/Project/Dashboard/GetTasks?filter=Dashboard_Tasks_Summary&ProjectId=' + parseInt($("#pId").attr("data-ProjectId")) + '&Value1=' + items + '&TaskIds=' + $.cookie($scope.CookieIdentifier + 'ProjectTasks') + '&LocationIds=' + $.cookie($scope.CookieIdentifier + "ProjectMarkets") + '&FromDate=' + $.cookie($scope.CookieIdentifier + "FromDate") + '&ToDate=' + $.cookie($scope.CookieIdentifier + "ToDate")).then(function (result) {
    //    $scope.BookmarkTasks = result.data
    //});

    $scope.GetProgramLabel = function () {

        $http.get('/Project/Dashboard/GetDashboardCharts?Filter=Get_Progam_Status&ProjectId=' + $("#pId").attr("data-ProjectId") + '&TaskIds=' + $.cookie($scope.CookieIdentifier + 'ProjectTasks') + '&LocationIds=' + $.cookie($scope.CookieIdentifier + "ProjectMarkets") + '&FromDate=' + $.cookie($scope.CookieIdentifier + "FromDate") + '&ToDate=' + $.cookie($scope.CookieIdentifier + "ToDate")).then(function (res) {
            $scope.ForecastedMTD = res.data[0].MlsForecastMTD;
            $scope.ActualMTD = res.data[0].MlsActualMTD;
            $scope.ProjectForecastTD = res.data[0].PrjForecastTD;
            $scope.ProjectActualTD = res.data[0].PrjActualTD

        });
    }
    $scope.GetProgramLabel();

    $scope.StatusMilestone = function (milestone) {
        $scope.StatusMilestone.Milestone = milestone.Milestone
        $scope.StatusMilestone.ActualEndDate = milestone.ActualEndDate
        $scope.StatusMilestone.ActualStartDate = milestone.ActualStartDate
        $scope.StatusMilestone.ForecastStartDate = milestone.ForecastStartDate
        $scope.StatusMilestone.ForecastEndDate = milestone.ForecastEndDate
        $scope.StatusMilestone.PlannedDate = milestone.PlannedDate
        $scope.StatusMilestone.TargetDate = milestone.TargetDate
        $scope.StatusMilestone.ProjectSiteId = milestone.SiteId
    }
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
    $scope.formatDate = function (date) {
        var dateOut = new Date(date);
        return dateOut;
    };
    function GroupMarket(data) {
        var group_to_values = data.reduce(function (obj, item) {
            obj[item.PDefinationName] = obj[item.PDefinationName] || [];
            obj[item.PDefinationName].push({
                DefinationName: item.DefinationName, DefinationId: item.DefinationId
            });
            return obj;
        }, {
        });

        var groups = Object.keys(group_to_values).map(function (key) {
            return {
                PDefinationName: key, cities: group_to_values[key]
            };
        });

        return groups;
    }
    $scope.getFilterMarkets = function () {
        $http.get('/Defnation/ToList?filter=' + 'UserLocations' + '&value=' + parseInt($("#UserId").attr("data-UserId"))).then(function (result) {
            $scope.UserLocation = [];
            $scope.Region = [];
            $scope.Market = [];
            var count = -1;

            var list = '';
            var option = ''
            var optgroup = '';
            var ProjectMarketscookie = $.cookie($scope.CookieIdentifier + "ProjectMarkets");
            var IssueMarketscookie = $.cookie($scope.CookieIdentifier + "IssueMarkets");
            var MarketMarketscookie = $.cookie($scope.CookieIdentifier + "MarketMarkets");
            var items = ProjectMarketscookie ? ProjectMarketscookie.split(/,/) : new Array();
            var items1 = IssueMarketscookie ? IssueMarketscookie.split(/,/) : new Array();
            var items2 = MarketMarketscookie ? MarketMarketscookie.split(/,/) : new Array();
            var group = GroupMarket(result.data);
            var marketarray = []
            var marketstring = '';


            for (var i = 0; i < group.length; i++) {
                option = '';
                for (var j = 0; j < group[i].cities.length; j++) {
                    var marketstring = marketstring + group[i].cities[j].DefinationId + ',';
                    marketarray.push(group[i].cities[j].DefinationId);
                    if (checkselected(items, group[i].cities[j].DefinationId)) {
                        option += '<option value="' + group[i].cities[j].DefinationId + '" selected="selected">' + group[i].cities[j].DefinationName + '</option>'
                    }
                    else {
                        option += '<option value="' + group[i].cities[j].DefinationId + '">' + group[i].cities[j].DefinationName + '</option>'
                    }


                }
                optgroup += '<optgroup label="' + group[i].PDefinationName + '">' + option + '</optgroup>'
            }
            marketstring = marketarray.join(',');
            var optgroup1 = '';
            for (var i = 0; i < group.length; i++) {
                option = '';
                for (var j = 0; j < group[i].cities.length; j++) {
                    if (checkselected(items1, group[i].cities[j].DefinationId)) {
                        option += '<option value="' + group[i].cities[j].DefinationId + '" selected="selected">' + group[i].cities[j].DefinationName + '</option>'
                    }
                    else {
                        option += '<option value="' + group[i].cities[j].DefinationId + '">' + group[i].cities[j].DefinationName + '</option>'
                    }


                }
                optgroup1 += '<optgroup label="' + group[i].PDefinationName + '">' + option + '</optgroup>'
            }
            var optgroup2 = '';
            for (var i = 0; i < group.length; i++) {
                option = '';
                for (var j = 0; j < group[i].cities.length; j++) {
                    if (checkselected(items2, group[i].cities[j].DefinationId)) {
                        option += '<option value="' + group[i].cities[j].DefinationId + '" selected="selected">' + group[i].cities[j].DefinationName + '</option>'
                    }
                    else {
                        option += '<option value="' + group[i].cities[j].DefinationId + '">' + group[i].cities[j].DefinationName + '</option>'
                    }


                }
                optgroup2 += '<optgroup label="' + group[i].PDefinationName + '">' + option + '</optgroup>'
            }


            var text = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Project").multiselect({enableClickableOptGroups: true,includeSelectAllOption: true});});</script><select class="form-control"  id="example-xss-html_Project" multiple="multiple">' + optgroup + '</select>';
            var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Project").multiselect({enableClickableOptGroups: true});});</script><select class="form-control"  id="example-xss-html_Project" multiple="multiple">' + optgroup + '</select>';
            var txt2 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Issue").multiselect({enableClickableOptGroups: true});});</script><select class="form-control"  id="example-xss-html_Issue" multiple="multiple">' + optgroup1 + '</select>';
            var txt3 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Market").multiselect({enableClickableOptGroups: true});});</script><select class="form-control"  id="example-xss-html_Market" multiple="multiple">' + optgroup2 + '</select>';
            $("#lstProjectMarkets").append(txt1);
            $("#lstProjectMarkets").empty();
            $("#lstProjectMarkets").html(txt1);
            //$("#lstProjectMarkets").append(text);
            $("#lstIssueMarkets").append(txt2);
            $("#lstMarketMarkets").append(txt3);
        });
    }
    $scope.getFilterTask = function () {
        $.ajax({
            type: 'post',
            url: '/Project/Task/ToList',
            async: false,
            data: {
                Filter: 'ByTaskTypeKeyCode', Value: 'PROJECT_MILESTONE', value2: parseInt($("#pId").attr("data-ProjectId")), 'Resources': true
            },
            success: function (result) {

                //$scope.JsonResult = result.data;
                var list = '';
                var ProjectTaskscookie = $.cookie($scope.CookieIdentifier + "ProjectTasks");
                var items = ProjectTaskscookie ? ProjectTaskscookie.split(/,/) : new Array();
                var option = '';
                var marketarray = []
                var marketstring = '';
                for (var i = 0; i < result.length; i++) {

                    //if (checkselected(items, result[i].TaskId)) {
                    if (result[i].TaskId == parseInt($.cookie($scope.CookieIdentifier + "ProjectTasks"))) {
                        option += '<option value="' + result[i].TaskId + '" " selected="selected">' + result[i].Title + '</option>'
                    }
                    else {
                        option += '<option value="' + result[i].TaskId + '">' + result[i].Title + '</option>'
                    }

                }
                var option1 = '';
                for (var i = 0; i < result.length; i++) {

                    if (result[i].TaskId == parseInt($.cookie($scope.CookieIdentifier + "IssueTasks"))) {
                        option1 += '<option value="' + result[i].TaskId + '" " selected="selected">' + result[i].Title + '</option>'
                    }
                    else {
                        option1 += '<option value="' + result[i].TaskId + '">' + result[i].Title + '</option>'
                    }

                }
                var option2 = '';
                for (var i = 0; i < result.length; i++) {

                    if (result[i].TaskId == parseInt($.cookie($scope.CookieIdentifier + "MarketTasks"))) {
                        option2 += '<option value="' + result[i].TaskId + '" " selected="selected">' + result[i].Title + '</option>'
                    }
                    else {
                        option2 += '<option value="' + result[i].TaskId + '">' + result[i].Title + '</option>'
                    }

                }



                //  var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Project").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Project">' + option + '</select>';
                //  var txt4 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Tasks").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Tasks">' + option + '</select>';
                //  var txt2 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Issue").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Issue" >' + option1 + '</select>';
                //  var txt3 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Market").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Market" >' + option2 + '</select>';
                ////  $("#ListProjectTask").append(txt1);

                //    $("#lstIssueTasks").append(txt2);
                // $("#ListMarketIssue").append(txt3);
                //  $("#ListProjectTaskSandbox").append(txt4);
            }
        });
    }
    //$scope.getFilterTask();
    //$scope.getFilterMarkets();
    $scope.getTodo();

    //$scope.GetDefinitions();
    $scope.SaveFile = function () {
        $scope.IsFormSubmitted = true;
        $scope.Message = "";

        FileUploadService.UploadFile($scope.SelectedFileForUpload, $scope.FileDescription).then(function (d) {
            alert(d.Message);
            ClearForm();
        }, function (e) {
            alert(e);
        });

    };
    $scope.selectFileforUpload = function (file) {
        if (file[0].size > 3007200) {
            // alert("File is too big!");
            swal("File is too big !", "Maximum 3 Mb Allowed", "error");
            file.value = "";
            $scope.SelectedFileForUpload = null;
        }
        else {
            $scope.SelectedFileForUpload = file[0];
        }
    }

    $scope.formatDate = function (date) {
        var dateOut = new Date(date);
        return dateOut;
    };
    $("#datepicker").on("dp.change", function () {
        $scope.Todo.ToDoDateTime = $("#datepickerInput").val();
    });
    $("#Editdatepicker").on("dp.change", function () {
        $scope.Todo.ToDoDateTime = $("#EditdatepickerInput").val();
    });

});

app.directive('datePicker', [function () {
    return {
        restrict: "A",
        require: "ngModel",
        compile: function compile(tElement, tAttrs, transclude) {
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
                        autocomplete :"off"
                    }).on('changeDate', function (dateText) {
                        updateModel(dateText.date);
                    });;
                }
            }
        },
        link: function (scope, element, attr, ngModel) {
            var dateFormat = attr.uibDatepickerPopup || uibDatepickerPopupConfig.datepickerPopup;
            ngModel.$validators.date = function (modelValue, viewValue) {
                var value = viewValue || modelValue;

                if (!attr.ngRequired && !value) {
                    return true;
                }

                if (angular.isNumber(value)) {
                    value = new Date(value);
                }
                if (!value) {
                    return true;
                } else if (angular.isDate(value) && !isNaN(value)) {
                    return true;
                } else if (angular.isString(value)) {
                    var date = new Date(value);
                    return !isNaN(date);
                } else {
                    return false;
                }
            }
            ngModel.$formatters.push(function (value) {
                return new Date(value);
            });
        }
    };
}]);

app.factory('FileUploadService', function ($http, $q) { // explained abour controller and service in part 2

    var fac = {};
    var Url;

    fac.UploadFile = function (file, data, flag) {
        var valid = true;
      
        var formData = new FormData();
        formData.append("file", file);
        //We can send more data to server using append
        if (flag == "issue") {
            if (data.RequestedDate == null || data.RequestedDate == "") {
                swal("Error", "Please Insert Requested Date!", "error");
                valid = false
            }
            if ($('.Priority').text() == "  ") {
                swal("Error", "Please Select Priority!", "error");
                valid = false
            }
            if (data.Description == null || data.Description == "") {
                swal("Error", "Please Insert Description!", "error");
                valid = false
            }
            if (data.AssignedToId == null|| data.AssignedToId == "") {
                swal("Error", "Please select Assigned To!", "error");
                valid = false
            }
            if (valid) {
                formData.append("Issue", JSON.stringify(data));
                Url = "/Project/Issue/New"
             

            }
        }
        else {
            Url = "/Project/ProjectSite/Update"
            formData.append("Status", JSON.stringify(data));
       
        }


        var defer = $q.defer();
        //$http.post("/Project/Issue/New", Issue,
        //    {
        //        withCredentials: true,
        //        headers: { 'Content-Type': undefined },
        //        transformRequest: angular.identity
        //    })
        //.then(function (d) {
        //    defer.resolve(d);
        //});

        $http({
            method: 'POST',
            url: Url,
            transformRequest: angular.identity,
            headers: {
                'Content-Type': undefined
            },
            data: formData,
        }).then(function successCallback(response) {
            $.ajax({
                //url: '/Project/Dashboard/GetMilstonesBarchart?ProjectId=' + $("#pId").attr("data-ProjectId") + '&filter=Get_Project_Timeline_Variance',
                url: '/Project/Dashboard/TableResult?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + $.cookie(angular.element(document.getElementById('widget-grid')).scope().CookieIdentifier + 'ProjectTasks')
         + '&LocationIds=' + $.cookie(angular.element(document.getElementById('widget-grid')).scope().CookieIdentifier + "ProjectMarkets") + '&FromDate=' + $.cookie(angular.element(document.getElementById('widget-grid')).scope().CookieIdentifier + "FromDate") + '&ToDate=' + $.cookie(angular.element(document.getElementById('widget-grid')).scope() + "ToDate") + '&IsActive=' + $("#activeAndInactiveForSite").val(),

                type: 'Get',
                async: false,
                success: function (res) {

                    $('#divclientsites').html(res);
                    SiteGridPagination();

                }
            });
            swal("Success", "Site has been Updated Successfully!", "success");
            $('#SiteStatus').modal('hide');
            $('#IssueModal').modal('hide');
        }, function errorCallback(response) {
            swal("Error", "There is an error. Please try again!", "error");
        });

        // angular.element(document.getElementById('widget-grid')).scope().GetSiteLog(angular.element(document.getElementById('widget-grid')).scope().siteIdTemp);

        //        $http({
        //            method: "POST",
        //            url: Url,
        //            transformRequest: angular.identity,
        //            headers: {
        //                'Content-Type': undefined
        //            },
        //            data: formData,
        //        }).then(
        //function success(data) {
        //    // $('.modal').modal('hide');

        //    $.ajax({
        //        //url: '/Project/Dashboard/GetMilstonesBarchart?ProjectId=' + $("#pId").attr("data-ProjectId") + '&filter=Get_Project_Timeline_Variance',
        //        url: '/Project/Dashboard/TableResult?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + $.cookie('ProjectTasks')
        // + '&LocationIds=' + $.cookie("ProjectMarkets") + '&FromDate=' + $.cookie("FromDate") + '&ToDate=' + $.cookie("ToDate") + '&IsActive=' + $("#activeAndInactiveForSite").val(),

        //        type: 'Get',
        //        async: false,
        //        success: function (res) {

        //            $('#divclientsites').html(res);
        //            SiteGridPagination();

        //        }
        //    });
        //    swal("Success","Site has been Updated Successfully!", "success");
        //    //$('#SiteStatus').modal('hide');
        //},
        //function error(data) {

        //});

        return defer.promise;

    }
    return fac;

});
app.filter('pagination', function () {
    return function (input, start) {
        if (!angular.isArray(input)) {
            return [];
        }
        start = +start; //parse to int
        return input.slice(start);
    };
});


