var date = new Date(), y = date.getFullYear(), m = date.getMonth();
var firstDay = new Date(y, m, 1);
var lastDay = new Date(y, m + 1, 0);
firstDay = moment(firstDay).format('MM/DD/YY');
lastDay = moment(lastDay).format('MM/DD/YY');


var pstart, pend, mstart, mend, istart, iend
if ($.cookie('FromDate') == undefined) {

    $.cookie("FromDate", firstDay, {
        expires: 2000
    });
    $.cookie("ToDate", lastDay, {
        expires: 2000
    });
}
if ($.cookie('IssueFromDate') == undefined) {
    fromdate = firstDay;
    todate = lastDay;
    $.cookie("IssueFromDate", firstDay, {
        expires: 2000
    });
    $.cookie("IssueToDate", lastDay, {
        expires: 2000
    });
}
if ($.cookie('MarketFromDate') == undefined) {
    mfromdate = firstDay;
    mtodate = lastDay;
    $.cookie("MarketFromDate", firstDay, {
        expires: 2000
    });
    $.cookie("MarketToDate", lastDay, {
        expires: 2000
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
var marketarray = [];
function GroupMarket(data) {

    var group_to_values = data.reduce(function (obj, item) {
        obj[item.PDefinationName] = obj[item.PDefinationName] || [];
        obj[item.PDefinationName].push({ DefinationName: item.DefinationName, DefinationId: item.DefinationId });
        marketarray.push(item.DefinationId);
        return obj;
    }, {});

    var groups = Object.keys(group_to_values).map(function (key) {
        return { PDefinationName: key, cities: group_to_values[key] };
    });

    return groups;
}

function LoadStatusIssue() {
    var option = '';
    $.ajax({
        type: 'Get',
        url: '/Defnation/ToList?filter=' + 'By_KeyCode' + '&value=' + parseInt($("#UserId").attr("data-UserId")),
        async: false,
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                option += '<option value="' + result[i].DefinationId + '" selected="selected">' + result[i].DefinationName + '</option>'
             }

            var text = '<script type="text/javascript">$(document).ready(function() {$("#example-getting-started").multiselect();});</script><select class="formcontrol"  id="example-getting-started" multiple="multiple">' + option + '</select>';

            $('#lstIssueStatus').append(text);

        }
    });


}
LoadStatusIssue()

function getFilterMarkets() {
    $.ajax({
        type: 'Get',
        url: '/Defnation/ToList?filter=' + 'UserLocations' + '&value=' + parseInt($("#UserId").attr("data-UserId")),
        async: false,
        success: function (result) {


            var count = -1;

            var list = '';
            var option = ''
            var optgroup = '';

            var group = GroupMarket(result);

            marketstring = marketarray.join(',');
            if ($.cookie("ProjectMarkets") == undefined) {
                $.cookie("ProjectMarkets", marketarray.join(','), {
                    expires: 2000
                });
            }
            if ($.cookie("IssueMarkets") == undefined) {
                marketid = marketarray.join(',');
                $.cookie("IssueMarkets", marketarray.join(','), {
                    expires: 2000
                });
            }
            if ($.cookie("MarketMarkets") == undefined) {
                mmarketid = marketarray.join(',');
                $.cookie("MarketMarkets", marketarray.join(','), {
                    expires: 2000
                });
            }
            var ProjectMarketscookie = $.cookie("ProjectMarkets");
            var IssueMarketscookie = $.cookie("IssueMarkets");
            var MarketMarketscookie = $.cookie("MarketMarkets");
            var MapMarketscookie = $.cookie("MapMarkets");
            var items = ProjectMarketscookie ? ProjectMarketscookie.split(/,/) : new Array();
            var items1 = IssueMarketscookie ? IssueMarketscookie.split(/,/) : new Array();
            var items2 = MarketMarketscookie ? MarketMarketscookie.split(/,/) : new Array();
            var items4 = MapMarketscookie ? MapMarketscookie.split(/,/) : new Array();

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

            var optgroup4 = '';
            for (var i = 0; i < group.length; i++) {
                option = '';
                for (var j = 0; j < group[i].cities.length; j++) {
                    if (checkselected(items4, group[i].cities[j].DefinationId)) {
                        option += '<option value="' + group[i].cities[j].DefinationId + '" selected="selected">' + group[i].cities[j].DefinationName + '</option>'
                    }
                    else {
                        option += '<option value="' + group[i].cities[j].DefinationId + '">' + group[i].cities[j].DefinationName + '</option>'
                    }


                }
                optgroup4 += '<optgroup label="' + group[i].PDefinationName + '">' + option + '</optgroup>'
            }
            var text = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Project").multiselect({enableClickableOptGroups: true,includeSelectAllOption: true});});</script><select class="form-control"  id="example-xss-html_Project" multiple="multiple">' + optgroup + '</select>';
            var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Project").multiselect({enableClickableOptGroups: true});});</script><select class="form-control"  id="example-xss-html_Project" multiple="multiple">' + optgroup + '</select>';
            var txt2 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Issue").multiselect({enableClickableOptGroups: true});});</script><select class="form-control"  id="example-xss-html_Issue" multiple="multiple">' + optgroup1 + '</select>';
            var txt3 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Market").multiselect({enableClickableOptGroups: true});});</script><select class="form-control"  id="example-xss-html_Market" multiple="multiple">' + optgroup2 + '</select>';
            var txt4 = '<script type="text/javascript">$(document).ready(function() {$("#example-xss-html_Map").multiselect({enableClickableOptGroups: true});});</script><select class="form-control"  id="example-xss-html_Map" multiple="multiple">' + optgroup4 + '</select>';
            $("#lstProjectMarkets").append(txt1);
            $("#lstProjectMarkets").empty();
            $("#lstProjectMarkets").html(txt1);
            $("#lstMapMarkets").html(txt4);

            //$("#lstProjectMarkets").append(text);
            $("#lstIssueMarkets").append(txt2);
            $("#lstMarketMarkets").append(txt3);
        }
    });
}

function getFilterTask() {
    //type: 'Get',
    //url: '/Project/Dashboard/GetDashboardCharts?Filter=Get_Project_Tasks&projectId=' + parseInt($("#pId").attr("data-ProjectId")),

    //$http.post('/Project/Task/getAllTask?Filter=Get_Project_Tasks&projectId=' + parseInt($("#pId").attr("data-ProjectId"))).then(function (res) {

    $.ajax({
        //type: 'post',
        //url: '/Project/Task/ToList',
        //async: false,
        //data: { Filter: 'ByTaskTypeKeyCode', Value: 'PROJECT_MILESTONE', value2: parseInt($("#pId").attr("data-ProjectId")), 'Resources': true },
        type: 'post',
        url: '/Project/Task/getAllTask?Filter=Get_Project_Tasks&projectId=' + parseInt($("#pId").attr("data-ProjectId")),
        async: false,
        success: function (result) {
           
            TasksList = result;
            //$scope.JsonResult = result.data;
            var list = '';
            var ProjectTaskscookie = $.cookie("ProjectTasks");
            var items = ProjectTaskscookie ? ProjectTaskscookie.split(/,/) : new Array();
            var option = '';
            var marketarray = []
            var marketstring = '';
            for (var i = 0; i < result.length; i++) {
                if (i < 1) {
                    if ($.cookie("IssueTasks") == undefined) {
                        taskid = result[0].TaskId
                        $.cookie("IssueTasks", result[0].TaskId, {
                            expires: 2000
                        });
                    }
                    if ($.cookie("MarketTasks") == undefined) {
                        mtaskid = result[0].TaskId;
                        $.cookie("MarketTasks", result[0].TaskId, {
                            expires: 2000
                        });

                    }
                    if ($.cookie("ProjectTasks") == undefined) {
                        $.cookie("ProjectTasks", result[0].TaskId, {
                            expires: 2000
                        });

                    }
                }
                //if (checkselected(items, result[i].TaskId)) {
                if (result[i].TaskId == parseInt($.cookie("ProjectTasks"))) {
                    option += '<option value="' + result[i].TaskId + '" " selected="selected">' + result[i].Title + '</option>'
                }
                else {
                    option += '<option value="' + result[i].TaskId + '">' + result[i].Title + '</option>'
                }

            }
            var option1 = '';
            for (var i = 0; i < result.length; i++) {

                if (result[i].TaskId == parseInt($.cookie("IssueTasks"))) {
                    option1 += '<option value="' + result[i].TaskId + '" " selected="selected">' + result[i].Title + '</option>'
                }
                else {
                    option1 += '<option value="' + result[i].TaskId + '">' + result[i].Title + '</option>'
                }

            }
            var option2 = '';
            for (var i = 0; i < result.length; i++) {

                if (result[i].TaskId == parseInt($.cookie("MarketTasks"))) {
                    option2 += '<option value="' + result[i].TaskId + '" " selected="selected">' + result[i].Title + '</option>'
                }
                else {
                    option2 += '<option value="' + result[i].TaskId + '">' + result[i].Title + '</option>'
                }

            }
            //debugger
            //var optgroup = '';
            //for (var i = 0; i < result.length; i++) {
            //    option = '';
            //    for (var j = 0; j < result[i].Tasks.length; j++) {
            //        option += '<option value="' + result[i].Tasks[j].Task + '">' + result[i].Tasks[j].Task + '</option>'

            //    }
            //    optgroup += '<optgroup label="' + result[i].Task + '">' + option + '</optgroup>'
            //}
            //var text = '<script type="text/javascript">$(document).ready(function() {$(".groupMultiSelect2").groupMultiSelect({onChange: function (parent, children) {console.log(parent);console.log(children);}});});</script><select class="form-control"  id="example-xss-html_test" multiple="multiple">' + optgroup + '</select>';

            var txt1 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Project").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Project">' + option + '</select>';
            var txt2 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Issue").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Issue" >' + option1 + '</select>';
            var txt3 = '<script type="text/javascript">$(document).ready(function() {$("#example-multiple-optgroups_Market").multiselect();});</script><select  class="form-control" id="example-multiple-optgroups_Market" >' + option2 + '</select>';
            //$("#ListProjectTask").append(txt1);
            //$("#ListProjectTask").append(text);
            //$("#lstIssueTasks").append(txt2);
            //$("#ListMarketIssue").append(txt3);
        }
    });
}

getFilterMarkets();
getFilterTask();