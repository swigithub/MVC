$(document).ready(function () {
    function UniquNamesList(data) {
        var uniqueNames = [];
        for (i = 0; i < data.length; i++) {
            if (uniqueNames.indexOf(data[i].DefinationName) === -1) {
                uniqueNames.push(data[i].DefinationName);
            }
        }
        return uniqueNames;
    }
    function GetNames(data, value) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].label == value) {
                return true;
            }

        }
        return false;
    }
    function GroupByIssues(data) {
        var group_to_values = data.reduce(function (obj, item) {
            obj[item.Issue] = obj[item.Issue] || [];
            obj[item.Issue].push({ label: item.DefinationName, count: item.TotalSite, id: item.DefinationId, Issue: item.Issue });
            return obj;
        }, {});

        var groups = Object.keys(group_to_values).map(function (key) {
            return { Issue: key, chartdata: group_to_values[key] };
        });

        return groups;
    }
    var visitorsDrilldownedChartOptions;
    var newVSReturningVisitorsOptions;
    $('#SearchIssue1').click(function () {
        debugger
        var dripdowndata = [];
        var marketissue = [];
        var ResourceIssue = [];
        var dataPoints1 = [];
        var dataPoint = [];
        var dataPoints2 = [];
        var dataPoints3 = [];
        var tempdata = [];
        var issuemarkets = $("#example-xss-html_Issue").val();
        var issuetasks = $("#example-multiple-optgroups_Issue").val();

        $.ajax({
            url: '/Project/Dashboard/GetDashboardCharts?Filter=Get_Region_Issue&ProjectId=' + parseInt($("#pId").attr("data-ProjectId")) + '&Value1=' + '' + '&TaskIds=' + issuetasks + '&LocationIds=' + issuemarkets.join(',')
            + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&SearchFilter=Issue',
            type: 'Get',
            async: false,
            success: function (data) {
                var uniqueNames = UniquNamesList(data)
                var grpIssues = GroupByIssues(data)
                   
                for (var i = 0; i < grpIssues.length; i++) {
                    var issueStackbar = [];
                    for (var j = 0; j < uniqueNames.length; j++) {

                        if (GetNames(grpIssues[i].chartdata, uniqueNames[j])) {
                            var index = grpIssues[i].chartdata.map(function (v) { return v.label; }).indexOf(uniqueNames[j]);
                            issueStackbar.push({ label: grpIssues[i].chartdata[index].label, y: grpIssues[i].chartdata[index].count, name: 'Market Issue', id: grpIssues[i].chartdata[index].id, toolTipContent: grpIssues[i].chartdata[index].Issue + ': ' + grpIssues[i].chartdata[index].count });
                        }
                        else {
                            issueStackbar.push({ label: uniqueNames[j], y: 0 });
                        }


                    }

                    tempdata.push({
                        type: "stackedColumn",
                        legendText: grpIssues[i].Issue,
                        click: visitorsChartDrilldownHandler,
                        showInLegend: true,
                        dataPoints: issueStackbar
                    })

                }
            }
            
        });
        var chartIssue = tempdata;
        var visitorsData = {
            "New vs Returning Visitors": chartIssue,
            "Market Issue": [],
            "Resource Issue": []
        };

        newVSReturningVisitorsOptions = {
            animationEnabled: true,
            theme: "light2",
            title: {
                text: ""
            },
            subtitles: [{
                text: "",
                backgroundColor: "#2eacd1",
                fontSize: 16,
                fontColor: "white",
                padding: 5
            }],
            legend: {
                fontFamily: "calibri",
                fontSize: 14,
                labelAngle: -70
                //itemTextFormatter: function (e) {
                //    return e.dataPoint.name + ": " + Math.round(e.dataPoint.y / totalVisitors * 100) + "%";
                //}
            },
            data: []
        };

        visitorsDrilldownedChartOptions = {
            animationEnabled: true,
            theme: "light2",
            axisX: {
                labelFontColor: "#717171",
                labelAngle: -70,
                lineColor: "#a2a2a2",
                tickColor: "#a2a2a2"
            },
            axisY: {
                gridThickness: 0,
                includeZero: false,
                labelFontColor: "#717171",
                lineColor: "#a2a2a2",
                tickColor: "#a2a2a2",
                lineThickness: 1
            },
            data: []
        };

        var chart = new CanvasJS.Chart("Issue_By_Geo", newVSReturningVisitorsOptions);
        chart.options.data = visitorsData["New vs Returning Visitors"];
        chart.render();

    })
    function visitorsChartDrilldownHandler(e) {

        chart = new CanvasJS.Chart("Issue_By_Geo", visitorsDrilldownedChartOptions);

        //chart.options.title = { text: e.dataPoint.name }
        GetData(e.dataPoint.id, e.dataPoint.name);
        if (e.dataPoint.name == 'Resource Issue') {
            chart.options.data = ResourceIssue
        }
        else {
            chart.options.data = marketissue;
        }
        chart.render();
        $("#backButton").toggleClass("invisible");
        if (e.dataPoint.name == 'Resource Issue') {
            $("#backButton").toggleClass("invisible");
        }
    }

    $("#backButton").click(function () {
        $(this).toggleClass("invisible");
        chart = new CanvasJS.Chart("Issue_By_Geo", newVSReturningVisitorsOptions);
        chart.options.data = visitorsData["New vs Returning Visitors"];
        chart.render();
    });

    $(".critical").click(function () {
        alert($(this).attr('data-TodoId'));
    });


    function GetData(Id, name) {

        if (name == 'Market Issue') {
            $.ajax({
                url: '/Project/Dashboard/GetMarketIssue?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Value1=' + Id,
                type: 'Get',
                async: false,
                success: function (data) {
                    tempdata = []
                    var uniqueNames = UniquNamesList(data)
                    var grpMarket = GroupByIssues(data)

                    for (var i = 0; i < grpMarket.length; i++) {
                        var issueStackbar = [];
                        for (var j = 0; j < uniqueNames.length; j++) {

                            if (GetNames(grpMarket[i].chartdata, uniqueNames[j])) {
                                var index = grpMarket[i].chartdata.map(function (v) { return v.label; }).indexOf(uniqueNames[j]);
                                issueStackbar.push({ label: grpMarket[i].chartdata[index].label, y: grpMarket[i].chartdata[index].count, name: 'Resource Issue', id: grpMarket[i].chartdata[index].id, toolTipContent: grpMarket[i].chartdata[index].Issue + ': ' + grpMarket[i].chartdata[index].count });
                            }
                            else {
                                issueStackbar.push({ label: uniqueNames[j], y: 0 });
                            }


                        }

                        tempdata.push({
                            type: "stackedColumn",
                            legendText: grpMarket[i].Issue,
                            click: visitorsChartDrilldownHandler,
                            showInLegend: true,
                            dataPoints: issueStackbar
                        })

                    }
                    marketissue = tempdata;
                }
            });
        }
        if (name == 'Resource Issue') {
            $.ajax({
                url: '/Project/Dashboard/GetResourceIssue?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Value1=' + Id,
                type: 'Get',
                async: false,
                success: function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var date = new Date(moment.utc(data[i].DefinationName));
                        data[i].DefinationName = (date.getDate()) + '/' + (date.getMonth() + 1);

                    }
                    tempdata = []
                    var uniqueNames = UniquNamesList(data)
                    var grpMarket = GroupByIssues(data)

                    for (var i = 0; i < grpMarket.length; i++) {
                        var issueStackbar = [];
                        for (var j = 0; j < uniqueNames.length; j++) {

                            if (GetNames(grpMarket[i].chartdata, uniqueNames[j])) {

                                var index = grpMarket[i].chartdata.map(function (v) { return v.label; }).indexOf(uniqueNames[j]);
                                issueStackbar.push({ label: grpMarket[i].chartdata[index].label, y: grpMarket[i].chartdata[index].count, name: 'Resource Issue', id: grpMarket[i].chartdata[index].id, toolTipContent: grpMarket[i].chartdata[index].Issue + ': ' + grpMarket[i].chartdata[index].count });
                            }
                            else {
                                issueStackbar.push({ label: uniqueNames[j], y: 0 });
                            }


                        }

                        tempdata.push({
                            type: "stackedColumn",
                            legendText: grpMarket[i].Issue,
                            //click: visitorsChartDrilldownHandler,
                            showInLegend: true,
                            dataPoints: issueStackbar
                        })

                    }
                    ResourceIssue = tempdata;
                }

            });

        }
    }



    //tracking map
    $('.jarviswidget-ctrls').remove();
    var Forecast = [];
    var Planned = [];
    var Actual = [];


    function addSymbols(e) {
        var suffixes = ["", "K", "M", "B"];
        var order = Math.max(Math.floor(Math.log(e.value) / Math.log(1000)), 0);

        if (order > suffixes.length - 1)
            order = suffixes.length - 1;

        var suffix = suffixes[order];
        return CanvasJS.formatNumber(e.value / Math.pow(1000, order)) + suffix;
    }

    function toggleDataSeries(e) {
        if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
            e.dataSeries.visible = false;
        } else {
            e.dataSeries.visible = true;
        }
        e.chart.render();
    }


    function toolTipFormatter(e) {
        var str = "";
        var total = 0;
        var str3;
        var str2;
        for (var i = 0; i < e.entries.length; i++) {
            var str1 = "<span style= \"color:" + e.entries[i].dataSeries.color + "\">" + e.entries[i].dataSeries.name + "</span>: <strong>" + e.entries[i].dataPoint.y + "</strong> <br/>";
            total = e.entries[i].dataPoint.y + total;
            str = str.concat(str1);
        }
        str2 = "<strong>" + e.entries[0].dataPoint.label + "</strong> <br/>";
        str3 = "<span style = \"color:Tomato\">Total: </span><strong>" + total + "</strong><br/>";
        return (str2.concat(str)).concat(str3);
    }

    function toggleDataSeries(e) {
        if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
            e.dataSeries.visible = false;
        }
        else {
            e.dataSeries.visible = true;
        }
        chart.render();
    }



    $('#SearchMarket1').click(function () {
        var SelectedTasks = $.map($("#dynatreeregional").dynatree("getSelectedNodes"), function (node) {
            return node.data.key;
        });
        
        ets = $("#example-xss-html_Market").val();
     //   var issuetasks = $("#example-multiple-optgroups_Market").val();
        var issuemarkets = $("#example-xss-html_Market").val();
        $.ajax({
            url: '/Project/Dashboard/GetDashboardCharts?Filter=Get_RegionView_WO&ProjectId=' + parseInt($("#pId").attr("data-ProjectId")) + '&Value1=' + '' + '&TaskIds=' + SelectedTasks + '&LocationIds=' + issuemarkets
            + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&SearchFilter=Market',
            type: 'Get',
            async: false,
            success: function (data) {
                //console.log(data);
                array = data;
                var sd = {};
                Forecast = [];
                Planned = [];
                Actual = [];
                for (var i = 0; i < data.length ; i++) {
                    if (data[i].DefinationName != null) {

                        sd = { label: data[i].DefinationName, y: data[i].TotalSites, id: data[i].DefinationId };
                        if (data[i].Type == 'Forecast') {
                            Forecast.push(sd);
                        }
                        else if (data[i].Type == 'Planned') {
                            Planned.push(sd);
                        }
                        else if (data[i].Type == 'Actual') {
                            Actual.push(sd);
                        }
                    }
                }
            }
        });

        var chart = new CanvasJS.Chart("regionviewchart", {
            animationEnabled: true,
            theme: "light2",
            height: "290",
            title: {
                text: ""
            },
            axisX: {
                valueFormatString: "MMM"
            },
            axisY: {
                labelFormatter: addSymbols
            },
            toolTip: {
                shared: true
            },
            legend: {
                cursor: "pointer",
                itemclick: toggleDataSeries
            },
            data: [
            {
                type: "column",
                name: "Actual",
                click: CallMarket,
                showInLegend: true,
                dataPoints: Actual
            },
            {
                type: "line",
                name: "Planned",
                //click: CallMarket,
                showInLegend: true,

                dataPoints: Planned

            },
            {
                type: "line",
                name: "Forecast",
                //click: CallMarket,
                markerBorderColor: "white",
                markerBorderThickness: 2,
                showInLegend: true,

                dataPoints: Forecast
            }]
        });
        chart.render();

    });



    $('#SearchProject1').click(function () {
        debugger
       // $.removeCookie("ProjectTasks");
        $.removeCookie("ProjectMarkets");
        $.removeCookie("FromDate");
        $.removeCookie("ToDate");
        var issuemarkets = $("#example-xss-html_Project").val();
        var issuetasks = projecttasks// $("#example-multiple-optgroups_Project").val();
        // ProjectMarkets Cookie
        var cookie = $.cookie("ProjectMarkets");
        var items = cookie ? cookie.split(/,/) : new Array();
        items.push(issuemarkets);
        var uniqueNames = [];
        $.each(items, function (i, el) {
            if ($.inArray(el, uniqueNames) === -1) uniqueNames.push(el);
        });
        items = uniqueNames
        $.cookie("ProjectMarkets", items.join(','), {
            expires: 2000
        });

        // ProjectTasks Cookie
        var Taskscookie = $.cookie("ProjectTasks");
        var Tasksitems = Taskscookie ? Taskscookie.split(/,/) : new Array();

        $.cookie("FromDate", PFromDate, {
            expires: 2000
        });
        $.cookie("ToDate", PToDate, {
            expires: 2000
        });

        Tasksitems.push(issuetasks);
        uniqueNames = [];
        $.each(Tasksitems, function (i, el) {
            if ($.inArray(el, uniqueNames) === -1) uniqueNames.push(el);
        });
        Tasksitems = uniqueNames
        $.cookie("ProjectTasks", Tasksitems.join(','), {
            expires: 2000
        });


        $.ajax({
            url: '/Project/Dashboard/GetDashboardCharts?Filter=Get_Region_Issue&=ProjectId' + parseInt($("#UserId").attr("data-UserId")) + '&Value1=' + '' + '&TaskIds=' + issuetasks + '&LocationIds=' + issuemarkets
            + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&SearchFilter=Global',
            type: 'Get',
            async: false,
            success: function (data) {
                debugger
                array = data;
                var sd = {};
                for (var i = 0; i < data.length ; i++) {
                    if (data[i].WoDate != null) {
                        var date = new Date(parseInt(data[i].WoDate.substr(6)));
                        sd = { label: (date.getDay() + 1) + '/' + date.getMonth(), y: data[i].WoCount };
                        if (data[i].WoType == 'Forecast') {
                            Forecast.push(sd);
                        }
                        else if (data[i].WoType == 'Planned') {
                            Planned.push(sd);
                        }
                        else if (data[i].WoType == 'Actual') {
                            Actual.push(sd);
                        }
                    }

                }
                $('#show-stat-microcharts').html(data);
            }
        });
        location.reload();
    })
});