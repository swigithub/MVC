//$(function () {

//    var start = moment().subtract(29, 'days');
//    var end = moment();

//    function cb(start, end) {
//        FromDate = start.format('MM/DD/YY');
//        ToDate = end.format('MM/DD/YY');
//        $('#reportrange span').html(start.format('MM/DD/YY') + ' - ' + end.format('MM/DD/YY'));
//    }

//    $('#reportrange').daterangepicker({
//        startDate: start,
//        endDate: end,
//        ranges: {
//            'Today': [moment(), moment()],
//            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
//            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
//            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
//            'This Month': [moment().startOf('month'), moment().endOf('month')],
//            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
//        }
//    }, cb);

//    function cb1(start, end) {

//        FromDate = start.format('MM/DD/YY');
//        ToDate = end.format('MM/DD/YY');

//        $('#reportrange1 span').html(start.format('MM/DD/YY') + ' - ' + end.format('MM/DD/YY'));
//    }

//    $('#reportrange1').daterangepicker({
//        startDate: start,
//        endDate: end,
//        ranges: {
//            'Today': [moment(), moment()],
//            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
//            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
//            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
//            'This Month': [moment().startOf('month'), moment().endOf('month')],
//            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
//        }
//    }, cb1);
//    cb1(start, end);
//    function cb3(start, end) {

//        FromDate = start.format('MM/DD/YY');
//        ToDate = end.format('MM/DD/YY');
//        $('#reportrangemarket span').html(start.format('MM/DD/YY') + ' - ' + end.format('MM/DD/YY'));
//    }

//    $('#reportrangemarket').daterangepicker({
//        startDate: start,
//        endDate: end,
//        ranges: {
//            'Today': [moment(), moment()],
//            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
//            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
//            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
//            'This Month': [moment().startOf('month'), moment().endOf('month')],
//            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
//        }
//    }, cb3);
//    cb3(start, end);

//    function cb2(start, end) {

//        FromDate = start.format('MM/DD/YY');
//        ToDate = end.format('MM/DD/YY');

//        $('#reportrange2 span').html(start.format('MM/DD/YY') + ' - ' + end.format('MM/DD/YY'));
//    }

//    $('#reportrange2').daterangepicker({
//        startDate: start,
//        endDate: end,
//        ranges: {
//            'Today': [moment(), moment()],
//            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
//            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
//            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
//            'This Month': [moment().startOf('month'), moment().endOf('month')],
//            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
//        }
//    }, cb2);

//    cb2(start, end);

//    if ($.cookie("FromDate") != undefined) {
//        $('#reportrangemarket span').html($.cookie("FromDate") + ' - ' + $.cookie("ToDate"));
//        $('#reportrange1 span').html($.cookie("FromDate") + ' - ' + $.cookie("ToDate"));
//        $('#reportrange2 span').html($.cookie("FromDate") + ' - ' + $.cookie("ToDate"));

//    }

//});

$(function () {

    $(document).on("click", ".expandable", function () {
        $(this).children('ul').toggle();
        return false;
    });

});

window.onload = function () {

    $('.jarviswidget-ctrls').remove();
    var cookie = $.cookie("BookMark");
    var items = cookie ? cookie.split(/,/) : new Array();
    $('.BookMark').each(function () {
        for (var i = 0; i < items.length; i++) {
            if ($(this).attr('data-bookmark') == items[i]) {
                $(this).addClass('Yellow');
                $(this).addClass("fa-check-square-o")
                $(this).removeClass("fa-square-o")
            }
        }
    });




    //tracking map
    $('.jarviswidget-ctrls').remove();
    var Forecast = [];
    var Planned = [];
    var Actual = [];
    var Target = [];
    array = [];

    //$.ajax({
    //    //url: '/Project/Dashboard/GetMilstonesBarchart?ProjectId=' + $("#pId").attr("data-ProjectId") + '&filter=Get_Project_Timeline_Variance',
    //    url: '/Project/Dashboard/GetDashboardCharts?filter=Get_Project_Timeline_Variance&ProjectId=' + $("#pId").attr("data-ProjectId") + '&Value1=&TaskIds' + $.cookie("ProjectTasks")
    //    + '&LocationIds=' + $.cookie("ProjectMarkets") + '&FromDate=' + $.cookie("FromDate") + '&ToDate=' + $.cookie("ToDate") + '&SearchFilter=null',

    //    type: 'Get',
    //    async: false,
    //    success: function (data) {
    //        array = data;
    //        var sd = {};

    //        for (var i = 0; i < data.length ; i++) {

    //            if (data[i].WoDate != null) {
    //                var date = new Date(parseInt(data[i].WoDate.substr(6)));
    //                sd = { label: (date.getDay() + 1) + '/' + (date.getMonth()+1), y: data[i].WoCount };
    //                if (data[i].WoType == 'Forecast') {
    //                    Forecast.push(sd);
    //                }
    //                else if (data[i].WoType == 'Planned') {
    //                    Planned.push(sd);
    //                }
    //                else if (data[i].WoType == 'Actual') {
    //                    Actual.push(sd);
    //                }
    //                else if (data[i].WoType == 'Target') {
    //                    Target.push(sd);
    //                }

    //            }

    //        }

    //    }
    //});
    //var chart = new CanvasJS.Chart("chartContainer", {
    //    animationEnabled: true,
    //    theme: "light2",
    //    height: "300",
    //    title: {
    //        text: ""
    //    },
    //    axisX: {
    //        valueFormatString: "MMM"
    //    },
    //    axisY: {
    //        labelFormatter: addSymbols
    //    },
    //    toolTip: {
    //        shared: true
    //    },
    //    legend: {
    //        cursor: "pointer",
    //        itemclick: toggleDataSeries
    //    },
    //    data: [
    //    {
    //        type: "column",
    //        name: "Actual",
    //        showInLegend: true,
    //        xValueFormatString: "MMMM YYYY",
    //        yValueFormatString: "#,##0",
    //        dataPoints: Actual
    //    },
    //    {
    //        type: "line",
    //        name: "Planned",
    //        showInLegend: true,
    //        yValueFormatString: "#,##0",
    //        dataPoints: Planned

    //    },{
    //        type: "line",
    //        name: "Forecast",
    //        markerBorderColor: "white",
    //        markerBorderThickness: 2,
    //        showInLegend: true,
    //        yValueFormatString: "#,##0",
    //        dataPoints: Forecast
    //    },
    //    {
    //        type: "line",
    //        name: "Target",
    //        markerBorderColor: "white",
    //        markerBorderThickness: 2,
    //        showInLegend: true,
    //        yValueFormatString: "#,##0",
    //        dataPoints: Target
    //    }]
    //});
    //chart.render();

    //function addSymbols(e) {
    //    var suffixes = ["", "K", "M", "B"];
    //    var order = Math.max(Math.floor(Math.log(e.value) / Math.log(1000)), 0);

    //    if (order > suffixes.length - 1)
    //        order = suffixes.length - 1;

    //    var suffix = suffixes[order];
    //    return CanvasJS.formatNumber(e.value / Math.pow(1000, order)) + suffix;
    //}

    //function toggleDataSeries(e) {
    //    if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
    //        e.dataSeries.visible = false;
    //    } else {
    //        e.dataSeries.visible = true;
    //    }
    //    e.chart.render();
    //}


    //function toolTipFormatter(e) {
    //    var str = "";
    //    var total = 0;
    //    var str3;
    //    var str2;
    //    for (var i = 0; i < e.entries.length; i++) {
    //        var str1 = "<span style= \"color:" + e.entries[i].dataSeries.color + "\">" + e.entries[i].dataSeries.name + "</span>: <strong>" + e.entries[i].dataPoint.y + "</strong> <br/>";
    //        total = e.entries[i].dataPoint.y + total;
    //        str = str.concat(str1);
    //    }
    //    str2 = "<strong>" + e.entries[0].dataPoint.label + "</strong> <br/>";
    //    str3 = "<span style = \"color:Tomato\">Total: </span><strong>" + total + "</strong><br/>";
    //    return (str2.concat(str)).concat(str3);
    //}

    //function toggleDataSeries(e) {
    //    if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
    //        e.dataSeries.visible = false;
    //    }
    //    else {
    //        e.dataSeries.visible = true;
    //    }
    //    chart.render();
    //}

}





var FromDate, ToDate;

$(document).ready(function () {



    $(document).on('click', '.FloorPlan', function () {
        modal_title.html('<span style="font-size:25px;">Add Floor Plan</span>');
        modal.modal('show');
        modal_content.css({ 'width': '755px' });
        var ur = '/Dashboard/FloorPlan?SiteId=' + $(this).attr('data-SiteId') + '&UserId=' + CurrentUserId + '&Client=' + $(this).attr('wo-ref') + '&SiteCode=' + $(this).attr('Site-Code');
        $.ajax({
            url: ur,
            // async: false,
            success: function (data) {
                modal_body.html(data);
                $(".select").select2();
            }
        });
        return false;
    });

    $(document).on('submit', '.frm-FloorPlan', function () {
        debugger
        return false
        var sdate = $("input[id='schduledate']").val();
        $("input[id='schduledate']").removeClass("error");
        var ur = '/Site/SiteSchedule';
        $.ajax({
            url: ur,
            type: 'POST',
            data: $(this).serialize(),
            // async: false,
            success: function (res) {
                if (res != 'session expired') {
                    if (res.Status == 'success') {
                        fnHideModal();
                        BtnFilterSitesGrid_Click('', '', '');

                    } else {
                        $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                    }
                } else {
                    $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }
            },
            error: function (err) {
                $.notify(err.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
            }
        });

        return false;
    });



    var LogStatus = '';
    function SortByName(a, b) {
        var aName = a.Issue.toLowerCase();
        var bName = b.Issue.toLowerCase();
        return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
    }
    function SortByLabel(a, b) {
        var aName = a.label.toLowerCase();
        var bName = b.label.toLowerCase();
        return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
    }
    var TitleText = '';

    function ShowSuccessMessage() {
        $("#SuccessMessage").fadeIn(3000);
        $("#SuccessMessage").fadeIn(5000);

    }

    $('#example-enableClickableOptGroups-onChange').multiselect({
        enableClickableOptGroups: true,
        onChange: function (option, checked) {
            alert(option.length + ' options ' + (checked ? 'selected' : 'deselected'));
        }
    });
    var Readiness = [];
    function GroupByIssues(data) {
        var group_to_values = data.reduce(function (obj, item) {
            obj[item.Type] = obj[item.Type] || [];
            obj[item.Type].push({ label: item.DefinationName, y: item.WoCount, toolTipContent: item.Type + ': ' + item.WoCount, Type: item.Type });
            return obj;
        }, {});

        var groups = Object.keys(group_to_values).map(function (key) {
            return { DefinationName: key, chartdata: group_to_values[key] };
        });

        return groups;
    }
    $('#Readiness').click(function () {
        $('#regionviewchart1').hide();
        $('#marketregion').hide();
        $('#ProjectReadiness').show();
        $.ajax({
            url: '/Project/Dashboard/GetRegionIssue?Filter=GET_PROJECT_READINESS&ProjectId=' + $("#pId").attr("data-ProjectId"),
            type: 'Get',
            async: false,
            success: function (data) {
                Readiness = [];
                var sd = {};

                var grpIssues = GroupByIssues(data);
                for (var i = 0; i < grpIssues.length; i++) {
                    var issueStackbar = [];
                    for (var j = 0; j < grpIssues[i].chartdata.length; j++) {

                        issueStackbar.push({ label: grpIssues[i].chartdata[j].label, y: grpIssues[i].chartdata[j].y, name: 'Market Issue', toolTipContent: grpIssues[i].chartdata[j].Type + ': ' + grpIssues[i].chartdata[j].y });
                    }

                    Readiness.push({
                        type: "stackedColumn",
                        legendText: grpIssues[i].DefinationName,
                        showInLegend: true,
                        dataPoints: issueStackbar
                    })

                }

            }
        });

        var chart = new CanvasJS.Chart("ProjectReadiness", {
            animationEnabled: false,
            height: 270,
            title: {
                text: "",
                fontFamily: "arial black",
                fontColor: "#695A42"
            },
            axisX: {
                //interval: 1,
                //intervalType: "year"
            },
            axisY: {
                //valueFormatString: "$#0bn",
                //gridColor: "#B6B1A8",
                //tickColor: "#B6B1A8"
            },
            toolTip: {
                //shared: true,
                //content: toolTipContent
            },
            data: Readiness
        });
        chart.render();
    });
    $('#Projects').click(function () {

        $('#regionviewchart1').show();
        $('#marketregion').show();
        $('#ProjectReadiness').hide();
    })


    $('#Issue_By_pie_btn').click(function () {
        LoadpieChart();
        $('#backButton').hide();
        //$('#Issue_By_pie').show();
        //$('#Issue_By_Geo').hide();
    });
    var picchart = []
    function LoadpieChart() {
        $.ajax({
            url: '/Project/Dashboard/GetRegionIssue?Filter=Get_Project_Issue_By&ProjectId=' + $("#pId").attr("data-ProjectId"),
            type: 'Get',
            async: false,
            success: function (data) {
                picchart = []
                var sd = {};
                for (var i = 0; i < data.length ; i++) {

                    sd = { label: data[i].IssueBy, y: data[i].TotalSite, legendText: data[i].IssueBy };
                    picchart.push(sd);


                }
                picchart = picchart.sort(SortByLabel);
                $('#Issue_By_pie').empty();
                //$('#Issue_By_pie').html(data);
            }
        });
        var chart = new CanvasJS.Chart("Issue_By_pie", {
            animationEnabled: false,
            theme: "light2",
            height: "290",
            //width: "700",
            legend: {
                horizontalAlign: "right", // "center" , "right"
                verticalAlign: "center",  // "top" , "bottom"
                fontSize: 15
            },
            data: [{
                type: "pie",
                height: 200,
                //width:705,
                showInLegend: true,
                indexLabelPlacement: "inside",
                indexLabelFontSize: 15,
                indexLabelFontColor: '#fff',
                indexLabel: "{y}",
                yValueFormatString: "##0\"%\"",
                dataPoints: picchart
            }]
        });
        chart.render();
    }
    LoadpieChart();
    $('#Issue_By_Geo_btn').click(function () {

        $.ajax({
            url: '/Project/Dashboard/GetMarkets?filter=' + 'Get_Region_Issue&ProjectId=' + $("#pId").attr("data-ProjectId") + '&Value=null',
            type: 'get',
            async: false,
            success: function (data) {
                //$('#Issue_By_Geo').empty();
                //$('#Issue_By_Geo').html(data);
                $('#Issue_By_pie').empty();
                $('#Issue_By_pie').html(data);
            }
        })

        //$('#backButton').show();
        //$('#Issue_By_Geo').show();
        //$('#Issue_By_pie').hide();
    });


    $('#todo').click(function () {
        $('#calendar').hide();
        $('#Todoview').show();
        $('#widget-body-toolbar').hide();
    });
    $('#cv').click(function () {
        $('#calendar').show();
        $('#Todoview').hide();
        $('#widget-body-toolbar').show();
    });

    var RegionId;
    var ProjectSiteId;
    var MarketId = null;
    var stages;
    $('#market').hide();

    $('#Todoview').hide();
    $('#Issue_By_Geo').hide();
    $('#ProjectReadiness').hide();
    $('.issue-button').hide();
    pageSetUp();



    $(".js-status-update a").click(function () {
        var selText = $(this).text();
        var $this = $(this);
        $this.parents('.btn-group').find('.dropdown-toggle').html(selText + ' <span class="caret"></span>');
        $this.parents('.dropdown-menu').find('li').removeClass('active');
        $this.parent().addClass('active');
    });

    /*
    * TODO: add a way to add more todo's to list
    */

    // initialize sortable
    $(function () {
        $("#sortable1, #sortable2").sortable({
            handle: '.handle',
            connectWith: ".todo",
            update: countTasks
        }).disableSelection();
    });

    // check and uncheck
    $('.todo .checkbox > input[type="checkbox"]').click(function () {
        var $this = $(this).parent().parent().parent();

        if ($(this).prop('checked')) {
            $this.addClass("complete");

            // remove this if you want to undo a check list once checked
            //$(this).attr("disabled", true);
            $(this).parent().hide();

            // once clicked - add class, copy to memory then remove and add to sortable3
            $this.slideUp(500, function () {
                $this.clone().prependTo("#sortable3").effect("highlight", {}, 800);
                $this.remove();
                countTasks();
            });
        } else {
            // insert undo code here...
        }

    })
    // count tasks
    function countTasks() {

        $('.todo-group-title').each(function () {
            var $this = $(this);
            $this.find(".num-of-tasks").text($this.next().find("li").size());
        });

    }

    /*
    * RUN PAGE GRAPHS
    */

    /* TAB 1: UPDATING CHART */
    // For the demo we use generated data, but normally it would be coming from the server

    var data = [], totalPoints = 200, $UpdatingChartColors = $("#updating-chart").css('color');

    function getRandomData() {
        if (data.length > 0)
            data = data.slice(1);

        // do a random walk
        while (data.length < totalPoints) {
            var prev = data.length > 0 ? data[data.length - 1] : 50;
            var y = prev + Math.random() * 10 - 5;
            if (y < 0)
                y = 0;
            if (y > 100)
                y = 100;
            data.push(y);
        }

        // zip the generated y values with the x values
        var res = [];
        for (var i = 0; i < data.length; ++i)
            res.push([i, data[i]])
        return res;
    }

    // setup control widget
    var updateInterval = 1500;
    $("#updating-chart").val(updateInterval).change(function () {

        var v = $(this).val();
        if (v && !isNaN(+v)) {
            updateInterval = +v;
            $(this).val("" + updateInterval);
        }

    });

    // setup plot
    var options = {
        yaxis: {
            min: 0,
            max: 100
        },
        xaxis: {
            min: 0,
            max: 100
        },
        colors: [$UpdatingChartColors],
        series: {
            lines: {
                lineWidth: 1,
                fill: true,
                fillColor: {
                    colors: [{
                        opacity: 0.4
                    }, {
                        opacity: 0
                    }]
                },
                steps: false

            }
        }
    };

    var plot = $.plot($("#updating-chart"), [getRandomData()], options);

    /* live switch */
    $('input[type="checkbox"]#start_interval').click(function () {
        if ($(this).prop('checked')) {
            $on = true;
            updateInterval = 1500;
            update();
        } else {
            clearInterval(updateInterval);
            $on = false;
        }
    });

    function update() {
        if ($on == true) {
            plot.setData([getRandomData()]);
            plot.draw();
            setTimeout(update, updateInterval);

        } else {
            clearInterval(updateInterval)
        }

    }

    var $on = false;


    var checkprojectId = 0;

    $('.task').click(function () {
        var id = $(this).attr('data-ProjectId')
        var eventdata = [];
        if (id != checkprojectId) {
            $.ajax({
                url: '/Project/Dashboard/GetMilstonesBarchart?ProjectId=' + id + '&filter=' + 'Get_Calendar_WO',
                type: 'Get',
                async: false,
                success: function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var date = new Date(parseInt(data[i].ActualDate.substr(6)));
                        var obj = { title: data[i].Title, start: new Date(date), className: ["event", "bg-color-greenLight"], icon: 'fa-check' }
                        $('#calendar').fullCalendar('renderEvent', obj, true);
                        eventdata.push(obj);
                    }
                }
            });
        }
        checkprojectId = id;

    });

    if ($("#calendar").length) {
        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();
        var calendar = $('#calendar').fullCalendar({

            //editable : true,
            //draggable : true,
            selectable: false,
            selectHelper: true,
            unselectAuto: false,
            disableResizing: true,
            height: 530,

            header: {
                left: 'title', //,today
                center: 'prev, next, today',
                right: 'month, agendaWeek, agenDay' //month, agendaDay,
            },

            select: function (start, end, allDay) {
                var title = prompt('Event Title:');
                if (title) {
                    calendar.fullCalendar('renderEvent', {
                        title: title,
                        start: start,
                        end: end,
                        allDay: allDay
                    }, true // make the event "stick"
                    );
                }
                calendar.fullCalendar('unselect');
            },

            events: [],

            eventRender: function (event, element, icon) {
                if (!event.description == "") {
                    element.find('.fc-event-title').append("<br/><span class='ultra-light'>" + event.description + "</span>");
                }
                if (!event.icon == "") {
                    element.find('.fc-event-title').append("<i class='air air-top-right fa " + event.icon + " '></i>");
                }
            }
        });

    };

    /* hide default buttons */
    $('.fc-header-right, .fc-header-center').hide();

    // calendar prev
    $('#calendar-buttons #btn-prev').click(function () {
        $('.fc-button-prev').click();
        return false;
    });

    // calendar next
    $('#calendar-buttons #btn-next').click(function () {
        $('.fc-button-next').click();
        return false;
    });

    // calendar today
    $('#calendar-buttons #btn-today').click(function () {
        $('.fc-button-today').click();
        return false;
    });

    // calendar month
    $('#mt').click(function () {
        $('#calendar').fullCalendar('changeView', 'month');
    });

    // calendar agenda week
    $('#ag').click(function () {
        $('#calendar').fullCalendar('changeView', 'agendaWeek');
    });

    // calendar agenda day
    $('#td').click(function () {
        $('#calendar').fullCalendar('changeView', 'agendaDay');
    });

    /*
     * CHAT
     */

    $.filter_input = $('#filter-chat-list');
    $.chat_users_container = $('#chat-container > .chat-list-body')
    $.chat_users = $('#chat-users')
    $.chat_list_btn = $('#chat-container > .chat-list-open-close');
    $.chat_body = $('#chat-body');

    /*
    * LIST FILTER (CHAT)
    */

    // custom css expression for a case-insensitive contains()
    jQuery.expr[':'].Contains = function (a, i, m) {
        return (a.textContent || a.innerText || "").toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
    };



});


