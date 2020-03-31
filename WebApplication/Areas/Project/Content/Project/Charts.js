var GeoIssueByRegionOptions = {
    animationEnabled: false,
    exportEnabled: true,
    theme: "light2",
    height: 280,
    width: 700,
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

    },
    data: []
};

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
        obj[item.Issue].push({ label: item.DefinationName, count: item.TotalSite, id: item.DefinationId, Issue: item.Issue,ColorCode:item.ColorCode });
        return obj;
    }, {});

    var groups = Object.keys(group_to_values).map(function (key) {
        return { Issue: key, chartdata: group_to_values[key] };
    });

    return groups;
}
function loadChart(chartId, Actual, Planned, Forecast, Target, height, ClicKEvent) {
    
    var colum
    if (ClicKEvent == 'CallMarket') {
        colum = {
            type: "column",
            name: "Actual",
            showInLegend: true,
            click: CallMarket,
            dataPoints: Actual
        }
    }
    else {
        colum = {
            type: "column",
            name: "Actual",
            showInLegend: true,
            click: null,
            dataPoints: Actual
        }
    }




    var chart = new CanvasJS.Chart(chartId, {
        animationEnabled: false,
        theme: "light2",
        height: height,
        exportEnabled: true,

        title:
		{
		    text: ""
		},
        axisX:
		{
		    interval: 1
		},
        axisY: {
            labelFormatter: addSymbols,
        },
        axisY2: {
            interval: 500
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
            axisYType: "primary",
            axisYindex: 0,
            name: "Actual",
            showInLegend: true,
            click: null,

            yValueFormatString: "#,##0",
            dataPoints: Actual
        },
        {
            type: "line",
            type: "line",
            axisYType: "secondary",
            name: "Planned",
            showInLegend: true,
            yValueFormatString: "#,##0",
            dataPoints: Planned

        },
        {
            type: "line",
            axisYType: "primary",
            axisYindex: 0,
            name: "Forecast",
            markerBorderColor: "white",
            markerBorderThickness: 2,
            showInLegend: true,
            yValueFormatString: "#,##0",
            dataPoints: Forecast
        },
        {
            type: "line",
            axisYType: "primary",
            axisYindex: 0,
            name: "Target",
            markerBorderColor: "white",
            markerBorderThickness: 2,
            showInLegend: true,
            yValueFormatString: "#,##0",
            dataPoints: Target
        }]
    });
    chart.render();

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

        showDefaultText(chart, "No Data Available");
        e.chart.render();
    }

    function toolTipFormatter(e) {
        var str = "";
        var total = 0;
        var str3;
        var str2;
        for (var i = 0; i < e.entries.length; i++) {
            var str1 = "<span style= \"color:" + e.entries[i].dataSeries.color + "\">" + e.entries[i].dataSeries.name + "</span>: <strong>" + e.entries[i].dataPoint.y + "</strong> <br />";
            total = e.entries[i].dataPoint.y + total;
            str = str.concat(str1);
        }
        str2 = "<strong>" + e.entries[0].dataPoint.label + "</strong> <br />";
        str3 = "<span style = \"color:Tomato\">Total: </span><strong>" + total + "</strong><br />";
        return (str2.concat(str)).concat(str3);
    }

    function toggleDataSeries(e) {
        if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
            e.dataSeries.visible = false;
        }
        else {
            e.dataSeries.visible = true;
        }
        showDefaultText(chart, "No Data Available");
        chart.render();
    }




    function LoadCharts(id, points) {
        var chart = new CanvasJS.Chart(id, {
            animationEnabled: false,
            exportEnabled: true,
            theme: "light2", // "light1", "light2", "dark1", "dark2"
            title: {
                text: ""
            },
            axisY: {
                title: ""
            },
            toolTip: {
                shared: true
            },
            legend: {
                cursor: "pointer",
                itemclick: toggleDataSeries
            },
            data: [{
                type: "column",
                showInLegend: true,
                legendMarkerColor: "grey",
                legendText: "",
                dataPoints: points
            }]
        });
        showDefaultText(chart, "No Data Available");
        chart.render();
    }






}


function toolTipFormatter(e) {
    var str = "";
    var total = 0;
    var str3;
    var str2;
    for (var i = 0; i < e.entries.length; i++) {
        var str1 = "<span style= \"color:" + e.entries[i].dataSeries.color + "\">" + e.entries[i].dataSeries.name + "</span>: <strong>" + e.entries[i].dataPoint.y + "</strong> <br />";
        total = e.entries[i].dataPoint.y + total;
        str = str.concat(str1);
    }
    str2 = "<strong>" + e.entries[0].dataPoint.label + "</strong> <br />";
    str3 = "<span style = \"color:Tomato\">Total: </span><strong>" + total + "</strong><br />";
    return (str2.concat(str)).concat(str3);
}

function loadIssueStackBarchart(chartId, data) {
    var visitorsData = {
        "Geo Issue By Region": data,
        "Market Issue": [],
        "Resource Issue": []
    };
    var GeoIssueByRegionOptions = {
        animationEnabled: false,
        exportEnabled: true,
        theme: "light2",
        height: 280,
        width: 700,
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

        },
        data: []
    };


    var chart = new CanvasJS.Chart(chartId, GeoIssueByRegionOptions);
    chart.options.data = visitorsData["Geo Issue By Region"];
    chart.render();

}

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


function ChartGroupData(data, searchFilter) {
    var array = { Forecast: [], Planned: [], Actual: [], Target: [] }
    var label, WoType;
    var count;
    for (var i = 0; i < data.length ; i++) {
        if (data[i].WoDate != null || data[i].DefinationName != null) {
            if (data[i].WoDate != null) {
                if (searchFilter == 'Daily') {
                    label = parseInt(data[i].WoDate.substr(6))
                }
                else {
                    label = data[i].WoDate;
                }
            }
            else {
                if (searchFilter == 'Daily') {
                    label = parseInt(data[i].DefinationName.substr(6))
                }
                else {
                    label = data[i].DefinationName;
                }
                //label = data[i].DefinationName
            }

            if (data[i].WoCount != null) {
                count = data[i].WoCount
            }
            else {
                count = data[i].TotalSites
            }

            if (data[i].WoType != null) {
                WoType = data[i].WoType
            }
            else {
                WoType = data[i].Type
            }

            if (searchFilter == 'Daily') {
                var date = new Date(label);
                sd = { label: (date.getDate()) + '/' + (date.getMonth() + 1), y: count };
            }
            else {
                sd = { label: label, y: count };
            }

            if (WoType == 'Forecast') {
                array.Forecast.push(sd);
            }
            else if (WoType == 'Planned') {
                array.Planned.push(sd);
            }
            else if (WoType == 'Actual') {
                array.Actual.push(sd);
            }
            else if (WoType == 'Target') {
                array.Target.push(sd);
            }
        }

    }

    return array;
}

function StackBarchartData(data, SearchFilter) {
    if (SearchFilter == 'Daily') {
        for (var i = 0; i < data.length; i++) {
            var date = new Date(moment.utc(data[i].DefinationName));
            data[i].DefinationName = (date.getDate()) + '/' + (date.getMonth() + 1);

        }
    }

    tempdata = []
    var uniqueNames = UniquNamesList(data)
    var grpIssues = GroupByIssues(data)

    for (var i = 0; i < grpIssues.length; i++) {

        var issueStackbar = [];
        var col = ''
        for (var j = 0; j < uniqueNames.length; j++) {

            if (GetNames(grpIssues[i].chartdata, uniqueNames[j])) {
                var index = grpIssues[i].chartdata.map(function (v) { return v.label; }).indexOf(uniqueNames[j]);
                col = grpIssues[i].chartdata[index].ColorCode;
                issueStackbar.push({ label: grpIssues[i].chartdata[index].label, y: grpIssues[i].chartdata[index].count, name: 'Market Issue', id: grpIssues[i].chartdata[index].id, toolTipContent: grpIssues[i].chartdata[index].Issue + ': ' + grpIssues[i].chartdata[index].count, color: grpIssues[i].chartdata[index].ColorCode });
            }
            else {
                issueStackbar.push({ label: uniqueNames[j], y: 0 });
            }


        }

        tempdata.push({
            type: "stackedColumn",
            legendText: grpIssues[i].Issue,
            name: "New vs Returning Visitors",
            legendMarkerColor: col,
            click: visitorsChartDrilldownHandler,
            showInLegend: true,
            dataPoints: issueStackbar
        })

    }
    return tempdata;
}

function visitorsChartDrilldownHandler(e) {

    chart = new CanvasJS.Chart("Issue_By_Geo", GeoIssueByRegionOptions);

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

//$("#backButton").click(function () {
//    $('.issue-button').hide();
//    $(this).toggleClass("invisible");
//    chart = new CanvasJS.Chart("Issue_By_Geo", GeoIssueByRegionOptions);
//    chart.options.data = visitorsData["Geo Issue By Region"];
//    chart.render();
//});

function GetData(Id, name) {
    var MarketId = Id;
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
        $('.issue-button').show();
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
                            issueStackbar.push({ label: grpMarket[i].chartdata[index].label, y: grpMarket[i].chartdata[index].count, color: grpMarket[i].chartdata[index].count, name: 'Resource Issue', id: grpMarket[i].chartdata[index].id, toolTipContent: grpMarket[i].chartdata[index].Issue + ': ' + grpMarket[i].chartdata[index].count });
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
function CallMarket(e) {
    RegionId = e.dataPoint.id;
    $.ajax({
        url: '/Project/Dashboard/GetMarkets?filter=' + 'Get_MarketView_WO' + '&ProjectId=' + $("#pId").attr("data-ProjectId") + '&Value=' + e.dataPoint.id,
        type: 'get',
        async: false,
        success: function (data) {
            debugger
            $('#region').hide()
            $('#market').show()
            $('#marketregion').empty();
            $('#marketregion').html(data);
            $('#marketviewchart').show()
        }
    })
}