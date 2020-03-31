var app = angular.module('TaskApp', []);

app.controller('TaskCtrl', function ($scope, $http, $timeout) {




    // function definitions
    $scope.GetFilters = function () {
        $http.get('/Defnation/ToList?filter=' + 'UserLocations' + '&value=' + 11).then(function (result) {
            $scope.UserLocation = [];
            $scope.Region = [];
            $scope.Market = [];
            var count = -1;

            var list = '';
            var option = ''
            var optgroup = '';
            var ProjectMarketscookie = $.cookie("ProjectMarkets");
            var items = ProjectMarketscookie ? ProjectMarketscookie.split(/,/) : new Array();
            var group = GroupMarket(result.data);

            for (var i = 0; i < group.length; i++) {
                option = '';
                for (var j = 0; j < group[i].cities.length; j++) {
                    if (checkselected(items, group[i].cities[j].DefinationId)) {
                        option += '<option value="' + group[i].cities[j].DefinationId + '">' + group[i].cities[j].DefinationName + '</option>'
                    }
                    else {
                        option += '<option value="' + group[i].cities[j].DefinationId + '" selected="selected">' + group[i].cities[j].DefinationName + '</option>'
                    }
                }
                optgroup += '<optgroup label="' + group[i].PDefinationName + '">' + option + '</optgroup>'
            }

            var text = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Project").multiselect({enableClickableOptGroups: true,includeSelectAllOption: true});});</script><select class="form-control"  id="example-xss-html_Project" multiple="multiple">' + optgroup + '</select>';
            var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Project").multiselect({enableClickableOptGroups: true});});</script><select class="form-control"  id="example-xss-html_Project" multiple="multiple">' + optgroup + '</select>';
            var txt2 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Issue").multiselect({enableClickableOptGroups: true});});</script><select class="form-control"  id="example-xss-html_Issue" multiple="multiple">' + optgroup + '</select>';
            var txt3 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Market").multiselect({enableClickableOptGroups: true});});</script><select class="form-control"  id="example-xss-html_Market" multiple="multiple">' + optgroup + '</select>';
            //$("#lstProjectMarkets").append(txt1);
            $("#lstProjectMarkets").empty();
            $("#lstProjectMarkets").html(txt1);
            //$("#lstProjectMarkets").append(text);

            $("#lstIssueMarkets").append(txt2);
            $("#lstMarketMarkets").append(txt3);

            $(".multiselect-selected-text").addClass("changingSelector").attr("data-id", "selector2");
            changingSelector();
        });


        //$http.get('/Project/Dashboard/GetTaskList?Filter=Get_Project_Tasks&ProjectId=' + $('#pId').attr('data-ProjectId') + '&filterOption=').then(function (result) {
        //    $scope.JsonResult = result.data;
        //    var list = '';
        //    var ProjectTaskscookie = $.cookie("ProjectTasks");
        //    var items = ProjectTaskscookie ? ProjectTaskscookie.split(/,/) : new Array();
        //    var option = '';
        //    for (var i = 0; i < result.data.length; i++) {

        //        if (checkselected(items, result.data[i].TaskId)) {
        //            option += '<option value="' + result.data[i].TaskId + '" " selected="selected">' + result.data[i].Task + '</option>'
        //        }
        //        else {
        //            option += '<option value="' + result.data[i].TaskId + '">' + result.data[i].Task + '</option>'
        //        }
        //    }

        //    var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Project").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Project">' + option + '</select>';
        //    var txt2 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Issue").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Issue" >' + option + '</select>';
        //    var txt3 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Market").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Market" >' + option + '</select>';
        // //   $("#ListProjectTask").append(txt1);
        //    $("#lstIssueTasks").append(txt2);
        //    $("#ListMarketIssue").append(txt3);
        //});
    }


    //$scope.GetUsers = function () {
    //    $http.get('/Task/ListUsers?filter=' + 'ByProjectId' + '&value=' + $('#pId').attr('data-ProjectId')).then(function (result) {
    //        var option = ''
    //        var optgroup = '';
    //        var group = GroupUsers(result.data);
    //       // console.log(group);
    //        for (var i = 0; i < group.length; i++) {
    //            option = '';
    //            for (var j = 0; j < group[i].users.length; j++) {
    //                option += '<option value="' + group[i].users[j].UserId + '" selected="selected">' + group[i].users[j].UserName + '</option>'  
    //            }
    //            optgroup += '<optgroup label="' + group[i].RoleName + '">' + option + '</optgroup>'
    //        }
    //        var text = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_User").multiselect({enableClickableOptGroups: true,includeSelectAllOption: true});});</script><select class="form-control"  id="example-xss-html_User" multiple="multiple">' + optgroup + '</select>';
    //        var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_User").multiselect({enableClickableOptGroups: true});});</script><select class="form-control"  id="example-xss-html_User" multiple="multiple">' + optgroup + '</select>';
    //        $("#lstProjectUsers").empty();
    //        $("#lstProjectUsers").html(txt1);
    //    });

    //    $http.get('/Project/Dashboard/GetTaskList?Filter=Get_Project_Tasks&ProjectId=' + $('#pId').attr('data-ProjectId') + '&filterOption=').then(function (result) {
    //        $scope.JsonResult = result.data;
    //        var list = '';
    //        var ProjectTaskscookie = $.cookie("ProjectTasks");
    //        var items = ProjectTaskscookie ? ProjectTaskscookie.split(/,/) : new Array();
    //        var option = '';
    //        for (var i = 0; i < result.data.length; i++) {

    //            if (checkselected(items, result.data[i].TaskId)) {
    //                option += '<option value="' + result.data[i].TaskId + '" " selected="selected">' + result.data[i].Task + '</option>'
    //            }
    //            else {
    //                option += '<option value="' + result.data[i].TaskId + '">' + result.data[i].Task + '</option>'
    //            }
    //        }

    //        var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Project").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Project">' + option + '</select>';
    //        var txt2 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Issue").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Issue" >' + option + '</select>';
    //        var txt3 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Market").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Market" >' + option + '</select>';
    //        $("#ListProjectTask").append(txt1);
    //        $("#lstIssueTasks").append(txt2);
    //        $("#ListMarketIssue").append(txt3);
    //    });
    //}

    $scope.GetMilestone = function () {
        $http({
            url: '/Project/Task/ToList',
            method: "POST",
            data: { 'filter': "ByProjectId", value: '', value2: '', ProjectId: $('#pId').attr('data-ProjectId') }
        }).then(function (response) {
            //console.log(response.data);
        },
       function (response) { // optional
           // failed
       });
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

    function GroupMarket(data) {
        var group_to_values = data.reduce(function (obj, item) {
            obj[item.PDefinationName] = obj[item.PDefinationName] || [];
            obj[item.PDefinationName].push({ DefinationName: item.DefinationName, DefinationId: item.DefinationId });
            return obj;
        }, {});
        var groups = Object.keys(group_to_values).map(function (key) {
            return { PDefinationName: key, cities: group_to_values[key] };
        });
        return groups;
    }


      let TasksGroupingdynatree = function (Tasks) {
        debugger
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
     
    //function GroupUsers(data) {
    //    var group_to_values = data.reduce(function (obj, item) {
    //        obj[item.ClientName] = obj[item.ClientName] || [];

    //        obj[item.RoleName] = obj[item.RoleName] || [];
    //        obj[item.RoleName].push({ UserName: item.FirstName + ' ' + item.LastName, UserId: item.UserId });
    //        return obj;
    //    }, {});
    //    var groups = Object.keys(group_to_values).map(function (key) {
    //        return {
    //            RoleName: key, users: group_to_values[key]
    //        };
    //    });
    //    return groups;
    //}

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


    //$http.get('/Project/Dashboard/GetTaskList?Filter=Get_Project_Tasks&projectId=' + parseInt($("#pId").attr("data-ProjectId"))).then(function (res) {
    $http.post('/Project/Task/getAllTask?Filter=Get_Project_Tasks&projectId=' + parseInt($("#pId").attr("data-ProjectId"))).then(function (res) {
        var grpOptions = CreateTaskObject(res.data)
        var init = GegInitailTasks("ProjectTasksTitle");
        var optgroup2 = '';
        for (var i = 0; i < res.data.length; i++) {
            var ParentOption = ''
            //var option = TasksGrouping(res.data[i].Tasks);
            var option = TasksGroupingdynatree(res.data[i].Tasks);
            debugger
            // optgroup2 += '<optgroup label="' + res.data[i].Task + '">' + option + '</optgroup>';
            if (res.data[i].Tasks.length > 0) {
                optgroup2 += '<li id="' + res.data[i].TaskId + '">' + res.data[i].Task + '<ul>' + option + '</ul></li>';
            }
            else {
                optgroup2 += '<li id="' + res.data[i].TaskId + '">' + res.data[i].Task + '</li>';
            }
        }
        $(".dynatree123").append('<ul>' + optgroup2 + '</ul>');
        $(".dynatree123").each(function () {
            let _this = this;
            $(this).dynatree({
            persist: true,
            checkbox: true,
            selectMode: 1,
            onQueryActivate: function (select, node) {
                setTimeout(function () {
                    var selectedNodes = node.tree.getSelectedNodes();
                    var selectedKeys = $.map(selectedNodes, function (node) {
                        return node.data.title;
                    });
                    if (selectedKeys.length == 0) {
                        $(_this).parents('.dynatree-drop').find(".dynatree-btn").text("No Filter");
                        
                    }
                    else if (selectedKeys.length > 1) {
                        $(_this).parents('.dynatree-drop').find(".dynatree-btn").text("Multiple Selected");
                    
                    }
                    else {
                        $(_this).parents('.dynatree-drop').find(".dynatree-btn").text(selectedKeys);
                    }
                }, 1000)
            },
            onSelect: function (select, node) {
                var selectedNodes = node.tree.getSelectedNodes();
                var selectedKeys = $.map(selectedNodes, function (node) {
                    return node.data.title;
                });
                if (selectedKeys.length == 0) {
                    $(_this).parents('.dynatree-drop').find(".dynatree-btn").text("No Filter");
                   // $('button[data-id="selector4"]').attr('disabled', 'disabled');
                    $('button[data-id="selector4"]').removeAttr('disabled');
                   
                }
                else if (selectedKeys.length > 1) {
                    $(_this).parents('.dynatree-drop').find(".dynatree-btn").text("Multiple Selected");
                    $('button[data-id="selector4"]').attr('disabled', 'disabled');
                 
                    //$('button[data-id="selector4"]').removeAttr('disabled');
                }
                else {
                    $(_this).parents('.dynatree-drop').find(".dynatree-btn").text(selectedKeys);
                }
            }
        });
    });

        //$('#dynatreetracking').dynatree({
        //    checkbox: true,
        //    selectMode: 1,
        //    onSelect: function (flag, node) {
        //        // get selected Tasks
        //        var selectedNodes = node.tree.getSelectedNodes();
        //        var selectedKeys = $.map(selectedNodes, function (node) {
        //            return node.data.key;
        //        });


        //        SelectedTasks = selectedKeys;
        //        var MyTasks = '';
        //        if (SelectedTasks.length > 0) {
        //            MyTasks = SelectedTasks.join(",");
        //        }
        //        $.removeCookie("ProjectTasks");
        //        $.cookie("ProjectTasks", MyTasks, {
        //            expires: 2000
        //        });

        //    }
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
    $http.post('/Defnation/ToList?filter=byDefinationType&value=Project Status').then(function (res) {
        $scope.ProjectStatus = res.data;
        log(res);
    })


    // function calls 
    $scope.GetFilters();
    $scope.GetMilestone();
});




//app.directive('datePicker', [function () {
//    return {
//        restrict: "A",
//        require: "ngModel"
//        , compile: function compile(tElement, tAttrs, transclude) {
//            return {
//                post: function postLink(scope, iElement, iAttrs, controller) {
//                    var updateModel = function (dateText) {
//                        scope.$apply(function () {
//                            controller.$setViewValue(dateText);
//                        });
//                    };
//                    $(iElement).datepicker({
//                        format: 'mm/dd/yyyy',
//                        autoclose: true
//                    }).on('changeDate', function (dateText) {
//                        updateModel(dateText.date);
//                    });;
//                }
//            }
//        },
//    };
//}]);



$("body .dynatree-btn").addClass("changingSelector").attr("data-id", "selector3");

function changingSelector() {
    $('.changingSelector').each(function () {
        var currentId = $(this).attr("data-id");
        var currentDate = $(this).text();
        $(this).parents('header').find('.selected-filters i[data-content="' + currentId + '"]').text(currentDate);
    });

    $('.changingSelector').bind("DOMSubtreeModified", function () {
        var currentId = $(this).attr("data-id");
        var currentDate = $(this).text();
        $(this).parents('.jarviswidget').find('header .selected-filters i[data-content="' + currentId + '"]').text(currentDate);
    });
}
changingSelector();
