
//#region Global Variables
var copyDataOfWidget = {};
//var userIDByX1 = 1;
//#endregion

/* ===== Main Controller ==== */
airviewXcelerateApp.controller('mainController', function ($rootScope, $scope, $compile, $http, MasterService, $stateParams) {
    $scope.iconsclass = [];
   
    xl_script.collapseWidget();
    xl_script.donutchartlabels();
    xl_script.headerlogobuttons();
    xl_script.popup();
    xl_script.highchartresize();
    // xl_script.removeButton();
    //xl_script.columnSize();
    xl_script.dropdown();
    xl_script.customselectbox();
    xl_script.delayeachdropdown();
    xl_script.tableoptions();
    xl_script.progressbardropdown();
    xl_script.AlertAutoHide();
    xl_script.CanvasChartRender();
    xl_script.TitleUpdate();
    xl_script.colorPickerPosition();
    MasterService.initialization($scope, $compile, $rootScope);

});

airviewXcelerateApp.controller('dashboardAndReportController', function ($rootScope, $scope, $compile, DashboardAndReport, uiGridConstants, toastr, $timeout, $http, ivhTreeviewMgr) {
    var vm = this;
    DashboardAndReport.initialization($compile, $scope, vm, uiGridConstants, $timeout, toastr, $rootScope);
});
airviewXcelerateApp.controller('TopSectionModalInstance', function (parentScope, PVm, $uibModalInstance, $scope, $state, $compile, toastr, TopSectionModalInstanceService) {


    var vm = this;
    TopSectionModalInstanceService.initialization(vm, $scope, $compile, parentScope, PVm, $uibModalInstance);
})
airviewXcelerateApp.controller('TopSectionDashboardAndReport', function ($scope, $state, $compile, $uibModal, $http, TopSectionDashboardAndReportService) {

    var vm = this;
    TopSectionDashboardAndReportService.initialization(vm, $scope, $compile, $uibModal);
});


airviewXcelerateApp.controller('ETLTopSectionModalInstance', function (parentScope, PVm, $uibModalInstance, $scope, $state, $compile, toastr, ETLTopSectionModalInstanceService) {
    
    var vm = this;
    ETLTopSectionModalInstanceService.initialization(vm, $scope, $compile, parentScope, PVm, $uibModalInstance);
})
airviewXcelerateApp.controller('ETLTopSectionDashboardAndReport', function ($scope, $state, $compile, $uibModal, $http, ETLTopSectionDashboardAndReportService) {

    var vm = this;
    ETLTopSectionDashboardAndReportService.initialization(vm, $scope, $compile, $uibModal);
});




airviewXcelerateApp.controller('LinkProperties', function (pvm, widgetIndex, JoinDetailIndex, JoinDetailInnerIndex, $uibModalInstance, $scope, $state, $compile, toastr, $rootScope, JoinTypeService) {
    var vm = this;
    JoinTypeService.initialization(pvm, vm, widgetIndex, JoinDetailIndex, JoinDetailInnerIndex, $uibModalInstance, $scope, $state, $compile, toastr, $rootScope, JoinTypeService);

});

airviewXcelerateApp.controller('AddMultipleJoin', function (pvm, widgetIndex, parentScope, $uibModalInstance, $scope, $state, $compile, toastr, $rootScope, AddMultipleJoinModal) {
    var vm = this;
   
    AddMultipleJoinModal.initialization(pvm, vm, widgetIndex, $uibModalInstance, $scope, $state, $compile, toastr, $rootScope, parentScope);

});

airviewXcelerateApp.controller('RemoteDataSource', function (pvm, widgetIndex, parentScope, $uibModalInstance, $scope, $state, $compile, toastr, $rootScope, RemoteDataSourceServices) {
    var vm = this;
    RemoteDataSourceServices.initialization(pvm, vm, widgetIndex, $scope, $compile, toastr, $rootScope, parentScope, $uibModalInstance);

});
airviewXcelerateApp.controller('RemoteTablesNViews', function (pvm, widgetIndex, parentScope, $uibModalInstance,uiGridConstants, $scope, $state, $compile, toastr, $rootScope, RemoteTablesNViewsServices) {
    var vm = this;
    RemoteTablesNViewsServices.initialization(pvm, vm, widgetIndex, $scope, $compile, toastr, $rootScope, parentScope, $uibModalInstance, uiGridConstants);

});
airviewXcelerateApp.controller('AddETLName', function (pvm, widgetIndex, parentScope, $uibModalInstance, $scope, $state, $compile, toastr, $rootScope, AddETLNameModal) {
   
    var vm = this;
    AddETLNameModal.initialization(pvm, vm, widgetIndex, $uibModalInstance, $scope, $state, $compile, toastr, $rootScope, parentScope);

});
airviewXcelerateApp.controller('AddSchedular', function (pvm, widgetIndex, parentScope, $uibModalInstance, $scope, $state, $compile, toastr, $rootScope, AddETLSchedular) {
    var vm = this;
    AddETLSchedular.initialization(pvm, vm, widgetIndex, $uibModalInstance, $scope, $state, $compile, toastr, $rootScope, parentScope);

});
airviewXcelerateApp.controller('ETLManipulateController', function ($rootScope, $scope, $compile, ETLManipulate, uiGridConstants, toastr, $timeout, $http, ivhTreeviewMgr) {
    var vm = this;
    ETLManipulate.initialization($compile, $scope, vm, uiGridConstants, $timeout, toastr, $rootScope);

});
airviewXcelerateApp.controller('testController', function ($scope, messages) {
    console.log(messages)
});
/* ===== Grids ==== */
airviewXcelerateApp.controller('grids', function ($scope) {

    $scope.$on('$stateChangeSuccess', function () {

        setTimeout(function () {

            if ($("body").find("div[canvascharts]")) {
                xl_script.CanvasChart();
            }
            //xl_script.murri();
        }, 0)
    });


    $(".widget").on("webkitTransitionEnd transitionend oTransitionEnd", function (event) {
        document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
    });

});


/* ===== Child Grids ==== */
airviewXcelerateApp.controller('childGrid', function ($scope) {
    setTimeout(function () {
        xl_script.childmuuri();
    }, 0);
    $scope.$on('$stateChangeSuccess', function () {
        setTimeout(function () {
            xl_script.childmuuri();
        }, 0)
    });
});


/* ===== Dashboard Controller ==== */
airviewXcelerateApp.controller('dashboardController', ["$scope", "$http", function ($scope, $http) {
    var vm = this;


    $scope.get = function () {
        //$.get('/Xcelerate/Home/getTemplateList', {}, function (res, status, xhr) {
        //    console.log(res);
        //});
        //var callwithcall = getTemplate.doSomethingWithModel().done(function (response) {
        //    console.log(response)
        //});





    }
    $scope.$on('$stateChangeSuccess', function () {
        setTimeout(function () {
            xl_script.ApplyFilters();
            xl_script.strippedRows();
            if ($("body").find("div[canvascharts]")) {
                xl_script.CanvasChart();
            }
            $scope.$emit('murri');
        }, 0)
    });


}]);


/* ===== Sideheader Controller ==== */
airviewXcelerateApp.controller('sideheader', function ($scope, $location, $state) {
    //  $state.go('Main.Dashboard');
    var vm1 = this;
    vm1.TitleName = "Airview Xcelerate";
    xl_script.sideheaderdropdown();
    $scope.getClass = function (path) {
        return ($location.path().substr(0, path.length) === path) ? 'active' : '';
    }

    /* === Initialize Tooltips ====  */
   


});


/* ===== Header Controller ==== */
airviewXcelerateApp.controller('header', function ($scope, $compile, $http, TopHeaderSerive) {
    var vm = this;
    TopHeaderSerive.initialization($scope, $compile, vm);

    xl_script.optionsdropdown();
    xl_script.headerConverter();
    xl_script.menuicon();
    xl_script.themeSelect();

});


/* ===== Main Boxes Controller ==== */
airviewXcelerateApp.controller('main-boxes', function ($scope, messages) {
    // console.log(messages);
    $scope.boxes = [
        {
            class: "color5",
            number: "17726",
            title: "TSS",
            progressid: "tss"
        },
        {
            class: "color6",
            number: "5869",
            title: "SSM",
            progressid: "ssm"

        },
        {
            class: "color7",
            number: "9452",
            title: "EPL",
            progressid: "epl"

        },
        {
            class: "color8",
            number: "9452",
            title: "Ordered",
            progressid: "ordered"

        },
        {
            class: "color9",
            number: "952",
            title: "Called Out",
            progressid: "calledout"

        },
        {
            class: "color10",
            number: "9452",
            title: "Delivered",
            progressid: "delivered"

        },
        {
            class: "color11",
            number: "3606",
            title: "Pre Install",
            progressid: "preinstall"

        },
        {
            class: "color12",
            number: "4773",
            title: "Migration",
            progressid: "migration"

        }
    ]

    $scope.boxes2 = [
        {
            class: "color7",
            number: "04",
            title: "Approved",
            img: "images/box-icon1.png"
        },
        {
            class: "color10",
            number: "12",
            title: "Reports Submitted",
            img: "images/box-icon2.png"
        },
        {
            class: "color8",
            number: "40",
            title: "Pending Scheduled",
            img: "images/box-icon3.png"
        },
        {
            class: "color2",
            number: "53",
            title: "Scheduled",
            img: "images/box-icon4.png"
        },
        {
            class: "color1",
            number: "05",
            title: "In Progress",
            img: "images/box-icon5.png"
        },
        {
            class: "color12",
            number: "01",
            title: "Drives Completed",
            img: "images/box-icon6.png"
        },
        {
            class: "color11",
            number: "03",
            title: "Pending With Issues",
            img: "images/box-icon7.png"
        },
        {
            class: "color5",
            number: "90",
            title: "Total",
            img: "images/box-icon8.png"
        }
    ]

});

/* ===== Circular Loader ==== */
airviewXcelerateApp.directive('cloader', function () {
    function link(scope, element, attrs) {
        setTimeout(function () {
            xl_script.CLoader();
            document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
        }, 0)
    }
    return {
        restrict: "A",
        link: link
    };
});



/* ===== Program View Controller ==== */
airviewXcelerateApp.controller('PV', function ($scope) {
    /* ===== Sites Table  ==== */
    $scope.sites = [
        {
            code: "10070811",
            id: "OHL03502",
            region: "NorthEast",
            market: "Ohio/Western Pennsylvania",
            usid: "1422",
            vmme: "False",
            controlledintro: "False",
            superbowl: "False",
            isdasinbuild: "N",
            firstnetran: "IOC3",
            status: "Completed",
            color: "color6"
        },
        {
            code: "10070811",
            dropdownid: "table-child",
            id: "OHL03502",
            region: "NorthEast",
            market: "Ohio/Western Pennsylvania",
            usid: "1422",
            vmme: "False",
            controlledintro: "False",
            superbowl: "False",
            isdasinbuild: "N",
            firstnetran: "IOC3",
            status: "Completed",
            color: "color6",
            dropdown: [
                {
                    milestone: "TSS",
                    forecast: "",
                    plandate: "31/05/2017",
                    actualdate: "17/04/2017",
                    targetdate: "",
                    prioritycolor: "color1",
                    priority: "High",
                    statuscolor: "color2",
                    status: "Scheduled"
                },
                {
                    milestone: "SSM",
                    childdropdownid: "table-child2",
                    forecast: "",
                    plandate: "31/05/2017",
                    actualdate: "17/04/2017",
                    targetdate: "",
                    prioritycolor: "color1",
                    priority: "High",
                    statuscolor: "color2",
                    status: "Scheduled",
                    childdropdownrow: [
                        {
                            milestone: "TSS",
                            forecast: "",
                            plandate: "31/05/2017",
                            actualdate: "17/04/2017",
                            targetdate: "",
                            prioritycolor: "color1",
                            priority: "High",
                            statuscolor: "color2",
                            status: "Scheduled"
                        },
                        {
                            milestone: "SSM",
                            forecast: "",
                            plandate: "31/05/2017",
                            actualdate: "17/04/2017",
                            targetdate: "",
                            prioritycolor: "color1",
                            priority: "High",
                            statuscolor: "color2",
                            status: "Scheduled"
                        },
                        {
                            milestone: "TSS",
                            forecast: "",
                            plandate: "31/05/2017",
                            actualdate: "17/04/2017",
                            targetdate: "",
                            prioritycolor: "color1",
                            priority: "High",
                            statuscolor: "color2",
                            status: "Scheduled"
                        }
                    ]
                },
                {
                    milestone: "TSS",
                    forecast: "",
                    plandate: "31/05/2017",
                    actualdate: "17/04/2017",
                    targetdate: "",
                    prioritycolor: "color1",
                    priority: "High",
                    statuscolor: "color2",
                    status: "Scheduled"
                }
            ]

        },
        {
            code: "10070811",
            id: "OHL03502",
            region: "NorthEast",
            market: "Ohio/Western Pennsylvania",
            usid: "1422",
            vmme: "False",
            controlledintro: "False",
            superbowl: "False",
            isdasinbuild: "N",
            firstnetran: "IOC3",
            status: "Completed",
            color: "color6",
            dropdown: [
                {
                    milestone: "asdsa",
                    forecast: "",
                    plandate: "31/05/2017",
                    actualdate: "17/04/2017",
                    targetdate: "",
                    prioritycolor: "color1",
                    priority: "High",
                    statuscolor: "color2",
                    status: "Scheduled"
                },
                {
                    milestone: "TSS",
                    forecast: "",
                    plandate: "31/05/2017",
                    actualdate: "17/04/2017",
                    targetdate: "",
                    prioritycolor: "color1",
                    priority: "High",
                    statuscolor: "color2",
                    status: "Scheduled"
                }
            ]
        },
        {
            code: "10070811",
            id: "OHL03502",
            region: "NorthEast",
            market: "Ohio/Western Pennsylvania",
            usid: "1422",
            vmme: "False",
            controlledintro: "False",
            superbowl: "False",
            isdasinbuild: "N",
            firstnetran: "IOC3",
            status: "Completed",
            color: "color6"
        },
        {
            code: "10070811",
            id: "OHL03502",
            region: "NorthEast",
            market: "Ohio/Western Pennsylvania",
            usid: "1422",
            vmme: "False",
            controlledintro: "False",
            superbowl: "False",
            isdasinbuild: "N",
            firstnetran: "IOC3",
            status: "Completed",
            color: "color6"
        }
    ]

    /* ===== Issue Table ==== */
    $scope.issuestable = [
        {
            color: "color1",
            code: "12985222",
            priority: "High",
            status: "Cancelled",
            task: "",
            description: "Migration cancelled, Power cables are not long enough to reach breakers. Also, there is currently a temp fiber run along the fence and through the door of the shelter. Need ATT OPS assistance to get permanent solution installed a",
            assignedto: "",
            target: "01/08/2018"
        },
        {
            color: "color2",
            code: "12985219",
            priority: "High",
            status: "Cancelled",
            task: "",
            description: "Migration cancelled, Power cables are not long enough to reach breakers. Also, there is currently a temp fiber run along the fence and through the door of the shelter. Need ATT OPS assistance to get permanent solution installed a",
            assignedto: "Rao Adeel	",
            target: ""
        },
        {
            color: "color3",
            code: "12907892",
            priority: "High",
            status: "Cancelled",
            task: "",
            description: "Migration cancelled, Power cables are not long enough to reach breakers. Also, there is currently a temp fiber run along the fence and through the door of the shelter. Need ATT OPS assistance to get permanent solution installed a",
            assignedto: "Super Admin",
            target: "01/08/2018"
        },
        {
            color: "color4",
            code: "12907892",
            priority: "High",
            status: "Cancelled",
            task: "",
            description: "Migration cancelled, Power cables are not long enough to reach breakers. Also, there is currently a temp fiber run along the fence and through the door of the shelter. Need ATT OPS assistance to get permanent solution installed a",
            assignedto: "Super Admin",
            target: "01/08/2018"
        }
    ]

    /*========== Program View HighChart ==========*/
    Highcharts.chart('programview', {

        chart: { type: 'column' },
        title: 'none',
        xAxis: {
            categories: ['1/1', '1/2', '1/3', '1/4', '1/5', '1/6', '1/7', '1/8', '1/9', '1/10', '1/11', '1/12', '1/13', '1/14', '1/15', '1/16', '1/17', '1/18', '1/19', '1/20', '1/21', '1/22', '1/23', '1/24'],
            labels: {
                style: {
                    color: '#888888',
                    fontSize: '14px',
                    fontWeight: 'semibold',
                    fontFamily: 'Poppins'
                }
            }
        },
        yAxis: {
            allowDecimals: false,
            min: 0,
            title: 'none',
            labels: {
                style: {
                    color: '#888888',
                    fontSize: '14px',
                    fontWeight: 'semibold',
                    fontFamily: 'Poppins'
                }
            }
        },
        legend: {
            itemStyle: {
                color: '#888888',
                fontSize: '14px',
                fontWeight: 'semibold',
                fontFamily: 'Poppins'
            }
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.x + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {
            column: {
                stacking: 'normal'
            }
        },
        series: [{
            name: 'Forecast',
            data: [50, 82, 48, 98, 78, 50, 82, 48, 98, 78, 50, 82, 48, 98, 78, 50, 82, 48, 98, 78, 50, 82, 48, 98],
            stack: 'male',
            color: '#8ca2bb'
        }, {
            name: 'Actual',
            data: [43, 84, 54, 12, 55, 43, 84, 54, 12, 55, 43, 84, 54, 12, 55, 43, 84, 54, 12, 55, 43, 84, 54, 12],
            stack: 'male',
            color: '#1c6a90'
        }]
    });

    // /*========== DONUT CHART FOR ISSUE ACCOUNTIBILITY ON SUMMARY TAB ==========*/
    var pieColors4 = (function () {
        var colors = ['#7cb5ec', "#5c5c61", "#90ed7d", "#f7a35c", "#d0d2f7", "#f15c80", "#2b908f", "#7cb5ec"],
            base = Highcharts.getOptions().colors[0],
            i;

        for (i = 0; i < 10; i += 1) {
            // Start out with a darkened base color (negative brighten), and end
            // up with a much brighter color
            colors.push(Highcharts.Color(base).brighten((i - 3) / 7).get());
        }
        return colors;
    }());
    Highcharts.chart('donutchart4', {
        chart: {
            type: 'pie'
        },
        title: {
            text: "Issue Accountibility",
            style: {
                fontSize: '20px',
                fontFamily: 'poppins',
                fontWeight: '400'
            }
        },
        plotOptions: {
            pie: {
                innerSize: 80,
                depth: 200,
                allowPointSelect: true,
                cursor: 'pointer',
                colors: pieColors4,
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    distance: -50,
                    fontSize: 25,
                    style: {
                        fontSize: '15px',
                        textOutline: '0',
                        fontFamily: 'poppins',
                        fontWeight: '400'
                    },
                    filter: {
                        property: 'percentage',
                        operator: '>',
                        value: 5
                    }
                },
            }
        },
        series: [{
            name: '',
            borderWidth: 1,
            data: [
                { name: 'Access', y: 16 },
                { name: 'Engineering', y: 1 },
                { name: 'Execution', y: 39 },
                { name: 'Fiber', y: 3 },
                { name: 'Materials', y: 9 },
                { name: 'Pre-Existing', y: 7 },
                { name: 'Spares', y: 3 },
                { name: 'Weather-FM', y: 25 }
            ]
        }]
    });


    /*========== DONUT CHART FOR ISSUE DISTRIBUTION ON SUMMARY TAB ==========*/
    var pieColors3 = (function () {
        var colors = ['#7cb5ec', "#5c5c61", "#90ed7d", "#f7a35c", "#d0d2f7"],
            base = Highcharts.getOptions().colors[0],
            i;

        for (i = 0; i < 10; i += 1) {
            // Start out with a darkened base color (negative brighten), and end
            // up with a much brighter color
            colors.push(Highcharts.Color(base).brighten((i - 3) / 7).get());
        }
        return colors;
    }());
    Highcharts.chart('donutchart3', {
        chart: {
            type: 'pie'
        },
        title: {
            text: "Issue Distribution",
            style: {
                fontSize: '20px',
                fontFamily: 'poppins',
                fontWeight: '400'
            }
        },
        plotOptions: {
            pie: {
                innerSize: 80,
                depth: 200,
                allowPointSelect: true,
                cursor: 'pointer',
                colors: pieColors3,
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    distance: -50,
                    fontSize: 25,
                    style: {
                        fontSize: '15px',
                        textOutline: '0',
                        fontFamily: 'poppins',
                        fontWeight: '400'
                    },
                    filter: {
                        property: 'percentage',
                        operator: '>',
                        value: 1
                    }
                },
            }
        },
        series: [{
            name: '',
            borderWidth: 3,
            data: [
                { name: 'Un-Avoidable', y: 41 },
                { name: 'Nokia', y: 29 },
                { name: 'Nokia Un-Avoidable', y: 14 },
                { name: 'ATT', y: 5 },
                { name: 'ATT Un-Avoidable', y: 14 }
            ]
        }]
    });


    /*========== DONUT CHART FOR SITE STATUS ON SUMMARY TAB ==========*/
    var pieColors2 = (function () {
        var colors = ['#7cb5ec', "#5c5c61", "#90ed7d", "#f7a35c"],
            base = Highcharts.getOptions().colors[0],
            i;

        for (i = 0; i < 10; i += 1) {
            // Start out with a darkened base color (negative brighten), and end
            // up with a much brighter color
            colors.push(Highcharts.Color(base).brighten((i - 3) / 7).get());
        }
        return colors;
    }());
    Highcharts.chart('donutchart2', {
        chart: {
            type: 'pie'
        },
        title: {
            text: "Site Status On Feb 06,2018",
            style: {
                fontSize: '20px',
                fontFamily: 'poppins',
                fontWeight: '400'
            }
        },
        plotOptions: {
            pie: {
                innerSize: 80,
                depth: 200,
                allowPointSelect: true,
                cursor: 'pointer',
                colors: pieColors2,
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    distance: -50,
                    fontSize: 25,
                    style: {
                        fontSize: '20px',
                        textOutline: '0',
                        fontFamily: 'poppins',
                        fontWeight: '400'
                    },
                    filter: {
                        property: 'percentage',
                        operator: '>',
                        value: 16
                    }
                },
            }
        },
        series: [{
            name: '',
            borderWidth: 3,
            data: [
                { name: 'Planned Sites', y: 304 },
                { name: 'Completed Sites', y: 0 },
                { name: 'Migrated Sites', y: 0 },
                { name: 'Cancelled Sites ', y: 103 }
            ]
        }]
    });


    $scope.progress = [
        {
            class: "color5",
            textclr: "txt-color5",
            borderclr: "bdr-color5",
            id: "prg-tss",
            for: "prg-tss",
            title: "TSS",
            range: "17726/18259",
            percent: "60%"
        },
        {
            class: "color6",
            textclr: "txt-color6",
            borderclr: "bdr-color6",
            id: "prg-ssm",
            for: "prg-ssm",
            title: "SSM",
            range: "5784/6260",
            percent: "50%"
        },
        {
            class: "color7",
            textclr: "txt-color7",
            borderclr: "bdr-color7",
            id: "prg-epl",
            for: "prg-epl",
            title: "EPL",
            range: "9452/9452",
            percent: "70%",
            progressDropdown: [
                {
                    id: "prg-ordered",
                    for: "prg-ordered",
                    title: "Ordered",
                    textclr: "txt-color8",
                    borderclr: "bdr-color8",
                    range: "3592/3700",
                    percent: "30%",
                    class: "color8"
                },
                {
                    id: "prg-calledout",
                    for: "prg-calledout",
                    title: "Calledout",
                    textclr: "txt-color9",
                    borderclr: "bdr-color9",
                    range: "4763/4902",
                    percent: "80%",
                    class: "color9"
                },
                {
                    id: "prg-delivered",
                    for: "prg-delivered",
                    title: "Delivered",
                    textclr: "txt-color10",
                    borderclr: "bdr-color10",
                    range: "3592/3700",
                    percent: "60%",
                    class: "color10"
                }
            ]
        },
        {
            class: "color11",
            textclr: "txt-color11",
            borderclr: "bdr-color11",
            id: "prg-preinstall",
            for: "prg-preinstall",
            title: "Pre Install",
            range: "4762/4802",
            percent: "70%"
        },
        {
            class: "color12",
            textclr: "txt-color12",
            borderclr: "bdr-color12",
            id: "prg-migration",
            for: "prg-migration",
            title: "Migration",
            range: "3606/4546",
            percent: "50%"
        },

    ]

});


/* ===== Tracking Controller ==== */
airviewXcelerateApp.controller('tracking', function ($scope) {
    /*========== Program View HighChart ==========*/
    Highcharts.chart('programview', {

        chart: { type: 'column' },
        title: 'none',
        xAxis: {
            categories: ['1/1', '1/2', '1/3', '1/4', '1/5', '1/6', '1/7', '1/8', '1/9', '1/10', '1/11', '1/12', '1/13', '1/14', '1/15', '1/16', '1/17', '1/18', '1/19', '1/20', '1/21', '1/22', '1/23', '1/24'],
            labels: {
                style: {
                    color: '#888888',
                    fontSize: '14px',
                    fontWeight: 'semibold',
                    fontFamily: 'Poppins'
                }
            }
        },
        yAxis: {
            allowDecimals: false,
            min: 0,
            title: 'none',
            labels: {
                style: {
                    color: '#888888',
                    fontSize: '14px',
                    fontWeight: 'semibold',
                    fontFamily: 'Poppins'
                }
            }
        },
        legend: {
            itemStyle: {
                color: '#888888',
                fontSize: '14px',
                fontWeight: 'semibold',
                fontFamily: 'Poppins'
            }
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.x + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {
            column: {
                stacking: 'normal'
            }
        },
        series: [{
            name: 'Forecast',
            data: [50, 82, 48, 98, 78, 50, 82, 48, 98, 78, 50, 82, 48, 98, 78, 50, 82, 48, 98, 78, 50, 82, 48, 98],
            stack: 'male',
            color: '#8ca2bb'
        }, {
            name: 'Actual',
            data: [43, 84, 54, 12, 55, 43, 84, 54, 12, 55, 43, 84, 54, 12, 55, 43, 84, 54, 12, 55, 43, 84, 54, 12],
            stack: 'male',
            color: '#1c6a90'
        }]
    });
});


/* ===== Progress Bars ==== */
airviewXcelerateApp.controller('progressbars', function ($scope) {
    $scope.progress = [
        {
            class: "color5",
            textclr: "txt-color5",
            borderclr: "bdr-color5",
            id: "prg-tss",
            for: "prg-tss",
            title: "TSS",
            range: "17726/18259",
            percent: "60%"
        },
        {
            class: "color6",
            textclr: "txt-color6",
            borderclr: "bdr-color6",
            id: "prg-ssm",
            for: "prg-ssm",
            title: "SSM",
            range: "5784/6260",
            percent: "50%"
        },
        {
            class: "color7",
            textclr: "txt-color7",
            borderclr: "bdr-color7",
            id: "prg-epl",
            for: "prg-epl",
            title: "EPL",
            range: "9452/9452",
            percent: "70%",
            progressDropdown: [
                {
                    id: "prg-ordered",
                    for: "prg-ordered",
                    title: "Ordered",
                    textclr: "txt-color8",
                    borderclr: "bdr-color8",
                    range: "3592/3700",
                    percent: "30%",
                    class: "color8"
                },
                {
                    id: "prg-calledout",
                    for: "prg-calledout",
                    title: "Calledout",
                    textclr: "txt-color9",
                    borderclr: "bdr-color9",
                    range: "4763/4902",
                    percent: "80%",
                    class: "color9"
                },
                {
                    id: "prg-delivered",
                    for: "prg-delivered",
                    title: "Delivered",
                    textclr: "txt-color10",
                    borderclr: "bdr-color10",
                    range: "3592/3700",
                    percent: "60%",
                    class: "color10"
                }
            ]
        },
        {
            class: "color11",
            textclr: "txt-color11",
            borderclr: "bdr-color11",
            id: "prg-preinstall",
            for: "prg-preinstall",
            title: "Pre Install",
            range: "4762/4802",
            percent: "70%"
        },
        {
            class: "color12",
            textclr: "txt-color12",
            borderclr: "bdr-color12",
            id: "prg-migration",
            for: "prg-migration",
            title: "Migration",
            range: "3606/4546",
            percent: "50%"
        },

    ]
});


/* ===== Donut Boxes Controller ==== */
airviewXcelerateApp.controller('donut-boxes', function ($scope) {
    // /*========== DONUT CHART FOR ISSUE ACCOUNTIBILITY ON SUMMARY TAB ==========*/
    var pieColors4 = (function () {
        var colors = ['#7cb5ec', "#5c5c61", "#90ed7d", "#f7a35c", "#d0d2f7", "#f15c80", "#2b908f", "#7cb5ec"],
            base = Highcharts.getOptions().colors[0],
            i;

        for (i = 0; i < 10; i += 1) {
            // Start out with a darkened base color (negative brighten), and end
            // up with a much brighter color
            colors.push(Highcharts.Color(base).brighten((i - 3) / 7).get());
        }
        return colors;
    }());
    Highcharts.chart('donutchart4', {
        chart: {
            type: 'pie'
        },
        title: {
            text: "Issue Accountibility",
            style: {
                fontSize: '20px',
                fontFamily: 'poppins',
                fontWeight: '400'
            }
        },
        plotOptions: {
            pie: {
                innerSize: 80,
                depth: 200,
                allowPointSelect: true,
                cursor: 'pointer',
                colors: pieColors4,
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    distance: -50,
                    fontSize: 25,
                    style: {
                        fontSize: '15px',
                        textOutline: '0',
                        fontFamily: 'poppins',
                        fontWeight: '400'
                    },
                    filter: {
                        property: 'percentage',
                        operator: '>',
                        value: 5
                    }
                },
            }
        },
        series: [{
            name: '',
            borderWidth: 1,
            data: [
                { name: 'Access', y: 16 },
                { name: 'Engineering', y: 1 },
                { name: 'Execution', y: 39 },
                { name: 'Fiber', y: 3 },
                { name: 'Materials', y: 9 },
                { name: 'Pre-Existing', y: 7 },
                { name: 'Spares', y: 3 },
                { name: 'Weather-FM', y: 25 }
            ]
        }]
    });


    /*========== DONUT CHART FOR ISSUE DISTRIBUTION ON SUMMARY TAB ==========*/
    var pieColors3 = (function () {
        var colors = ['#7cb5ec', "#5c5c61", "#90ed7d", "#f7a35c", "#d0d2f7"],
            base = Highcharts.getOptions().colors[0],
            i;

        for (i = 0; i < 10; i += 1) {
            // Start out with a darkened base color (negative brighten), and end
            // up with a much brighter color
            colors.push(Highcharts.Color(base).brighten((i - 3) / 7).get());
        }
        return colors;
    }());
    Highcharts.chart('donutchart3', {
        chart: {
            type: 'pie'
        },
        title: {
            text: "Issue Distribution",
            style: {
                fontSize: '20px',
                fontFamily: 'poppins',
                fontWeight: '400'
            }
        },
        plotOptions: {
            pie: {
                innerSize: 80,
                depth: 200,
                allowPointSelect: true,
                cursor: 'pointer',
                colors: pieColors3,
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    distance: -50,
                    fontSize: 25,
                    style: {
                        fontSize: '15px',
                        textOutline: '0',
                        fontFamily: 'poppins',
                        fontWeight: '400'
                    },
                    filter: {
                        property: 'percentage',
                        operator: '>',
                        value: 1
                    }
                },
            }
        },
        series: [{
            name: '',
            borderWidth: 3,
            data: [
                { name: 'Un-Avoidable', y: 41 },
                { name: 'Nokia', y: 29 },
                { name: 'Nokia Un-Avoidable', y: 14 },
                { name: 'ATT', y: 5 },
                { name: 'ATT Un-Avoidable', y: 14 }
            ]
        }]
    });


    /*========== DONUT CHART FOR SITE STATUS ON SUMMARY TAB ==========*/
    var pieColors2 = (function () {
        var colors = ['#7cb5ec', "#5c5c61", "#90ed7d", "#f7a35c"],
            base = Highcharts.getOptions().colors[0],
            i;

        for (i = 0; i < 10; i += 1) {
            // Start out with a darkened base color (negative brighten), and end
            // up with a much brighter color
            colors.push(Highcharts.Color(base).brighten((i - 3) / 7).get());
        }
        return colors;
    }());
    Highcharts.chart('donutchart2', {
        chart: {
            type: 'pie'
        },
        title: {
            text: "Site Status On Feb 06,2018",
            style: {
                fontSize: '20px',
                fontFamily: 'poppins',
                fontWeight: '400'
            }
        },
        plotOptions: {
            pie: {
                innerSize: 80,
                depth: 200,
                allowPointSelect: true,
                cursor: 'pointer',
                colors: pieColors2,
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    distance: -50,
                    fontSize: 25,
                    style: {
                        fontSize: '20px',
                        textOutline: '0',
                        fontFamily: 'poppins',
                        fontWeight: '400'
                    },
                    filter: {
                        property: 'percentage',
                        operator: '>',
                        value: 16
                    }
                },
            }
        },
        series: [{
            name: '',
            borderWidth: 3,
            data: [
                { name: 'Planned Sites', y: 304 },
                { name: 'Completed Sites', y: 0 },
                { name: 'Migrated Sites', y: 0 },
                { name: 'Cancelled Sites ', y: 103 }
            ]
        }]
    });

});


/* ===== Sites Table Controller ==== */
airviewXcelerateApp.controller('sites-table', function ($scope) {
    $scope.sites = [
        {
            code: "10070811",
            id: "OHL03502",
            region: "NorthEast",
            market: "Ohio/Western Pennsylvania",
            usid: "1422",
            vmme: "False",
            controlledintro: "False",
            superbowl: "False",
            isdasinbuild: "N",
            firstnetran: "IOC3",
            status: "Completed",
            color: "color6"
        },
        {
            code: "10070811",
            dropdownid: "table-child",
            id: "OHL03502",
            region: "NorthEast",
            market: "Ohio/Western Pennsylvania",
            usid: "1422",
            vmme: "False",
            controlledintro: "False",
            superbowl: "False",
            isdasinbuild: "N",
            firstnetran: "IOC3",
            status: "Completed",
            color: "color6",
            dropdown: [
                {
                    milestone: "TSS",
                    forecast: "",
                    plandate: "31/05/2017",
                    actualdate: "17/04/2017",
                    targetdate: "",
                    prioritycolor: "color1",
                    priority: "High",
                    statuscolor: "color2",
                    status: "Scheduled"
                },
                {
                    milestone: "SSM",
                    childdropdownid: "table-child2",
                    forecast: "",
                    plandate: "31/05/2017",
                    actualdate: "17/04/2017",
                    targetdate: "",
                    prioritycolor: "color1",
                    priority: "High",
                    statuscolor: "color2",
                    status: "Scheduled",
                    childdropdownrow: [
                        {
                            milestone: "TSS",
                            forecast: "",
                            plandate: "31/05/2017",
                            actualdate: "17/04/2017",
                            targetdate: "",
                            prioritycolor: "color1",
                            priority: "High",
                            statuscolor: "color2",
                            status: "Scheduled"
                        },
                        {
                            milestone: "SSM",
                            forecast: "",
                            plandate: "31/05/2017",
                            actualdate: "17/04/2017",
                            targetdate: "",
                            prioritycolor: "color1",
                            priority: "High",
                            statuscolor: "color2",
                            status: "Scheduled"
                        },
                        {
                            milestone: "TSS",
                            forecast: "",
                            plandate: "31/05/2017",
                            actualdate: "17/04/2017",
                            targetdate: "",
                            prioritycolor: "color1",
                            priority: "High",
                            statuscolor: "color2",
                            status: "Scheduled"
                        }
                    ]
                },
                {
                    milestone: "TSS",
                    forecast: "",
                    plandate: "31/05/2017",
                    actualdate: "17/04/2017",
                    targetdate: "",
                    prioritycolor: "color1",
                    priority: "High",
                    statuscolor: "color2",
                    status: "Scheduled"
                }
            ]

        },
        {
            code: "10070811",
            id: "OHL03502",
            region: "NorthEast",
            market: "Ohio/Western Pennsylvania",
            usid: "1422",
            vmme: "False",
            controlledintro: "False",
            superbowl: "False",
            isdasinbuild: "N",
            firstnetran: "IOC3",
            status: "Completed",
            color: "color6",
            dropdown: [
                {
                    milestone: "asdsa",
                    forecast: "",
                    plandate: "31/05/2017",
                    actualdate: "17/04/2017",
                    targetdate: "",
                    prioritycolor: "color1",
                    priority: "High",
                    statuscolor: "color2",
                    status: "Scheduled"
                },
                {
                    milestone: "TSS",
                    forecast: "",
                    plandate: "31/05/2017",
                    actualdate: "17/04/2017",
                    targetdate: "",
                    prioritycolor: "color1",
                    priority: "High",
                    statuscolor: "color2",
                    status: "Scheduled"
                }
            ]
        },
        {
            code: "10070811",
            id: "OHL03502",
            region: "NorthEast",
            market: "Ohio/Western Pennsylvania",
            usid: "1422",
            vmme: "False",
            controlledintro: "False",
            superbowl: "False",
            isdasinbuild: "N",
            firstnetran: "IOC3",
            status: "Completed",
            color: "color6"
        },
        {
            code: "10070811",
            id: "OHL03502",
            region: "NorthEast",
            market: "Ohio/Western Pennsylvania",
            usid: "1422",
            vmme: "False",
            controlledintro: "False",
            superbowl: "False",
            isdasinbuild: "N",
            firstnetran: "IOC3",
            status: "Completed",
            color: "color6"
        }
    ]
});


/* ===== Issues Table Controller ==== */
airviewXcelerateApp.controller('issues-table', function ($scope) {
    $scope.issuestable = [
        {
            color: "color1",
            code: "12985222",
            priority: "High",
            status: "Cancelled",
            task: "",
            description: "Migration cancelled, Power cables are not long enough to reach breakers. Also, there is currently a temp fiber run along the fence and through the door of the shelter. Need ATT OPS assistance to get permanent solution installed a",
            assignedto: "",
            target: "01/08/2018"
        },
        {
            color: "color2",
            code: "12985219",
            priority: "High",
            status: "Cancelled",
            task: "",
            description: "Migration cancelled, Power cables are not long enough to reach breakers. Also, there is currently a temp fiber run along the fence and through the door of the shelter. Need ATT OPS assistance to get permanent solution installed a",
            assignedto: "Rao Adeel	",
            target: ""
        },
        {
            color: "color3",
            code: "12907892",
            priority: "High",
            status: "Cancelled",
            task: "",
            description: "Migration cancelled, Power cables are not long enough to reach breakers. Also, there is currently a temp fiber run along the fence and through the door of the shelter. Need ATT OPS assistance to get permanent solution installed a",
            assignedto: "Super Admin",
            target: "01/08/2018"
        },
        {
            color: "color4",
            code: "12907892",
            priority: "High",
            status: "Cancelled",
            task: "",
            description: "Migration cancelled, Power cables are not long enough to reach breakers. Also, there is currently a temp fiber run along the fence and through the door of the shelter. Need ATT OPS assistance to get permanent solution installed a",
            assignedto: "Super Admin",
            target: "01/08/2018"
        }
    ]
});


/* ===== Regional View Controller ==== */
airviewXcelerateApp.controller('RV', function ($scope) {
    Highcharts.chart('regionalview', {
        title: {
            text: null
        },
        yAxis: {
            gridLineWidth: '1px',
            gridLineColor: '#e0e0e0',
            title: { text: null },
            labels: {
                style: {
                    color: '#888888',
                    fontSize: '14px',
                    fontWeight: 'semibold',
                    fontFamily: 'Poppins'
                }
            }

        },
        xAxis: {
            categories: ['North East', 'West'],
            labels: {
                style: {
                    color: '#888888',
                    fontSize: '14px',
                    fontWeight: 'semibold',
                    fontFamily: 'Poppins'
                }
            }
        },
        legend: {
            itemStyle: {
                color: '#888888',
                fontSize: '14px',
                fontWeight: 'semibold',
                fontFamily: 'Poppins'
            }
        },
        series: [{
            type: 'column',
            name: 'Actual',
            data: [600, 200],
            pointPadding: 0,
            borderWidth: 0,
            groupPadding: 0.38,
            color: '#1c6a90'
        }, {
            name: 'Planned',
            data: [800, 450],
            color: '#b57db6'
        }, {
            name: 'Forecast',
            data: [900, 500],
            color: '#f2b82b'
        }]
    });
});


/* ===== Issues Controller ==== */
airviewXcelerateApp.controller('issues', function ($scope) {
    var pieColors = (function () {
        var colors = ['#03a99d', "#b57db6", "#1c6a90", "#ef5551", "#f47d62"],
            base = Highcharts.getOptions().colors[0],
            i;

        for (i = 0; i < 10; i += 1) {
            // Start out with a darkened base color (negative brighten), and end
            // up with a much brighter color
            colors.push(Highcharts.Color(base).brighten((i - 3) / 7).get());
        }
        return colors;
    }());
    Highcharts.chart('donutchart', {
        chart: {
            type: 'pie'
        },
        title: {
            text: null
        },
        plotOptions: {
            pie: {
                innerSize: 80,
                depth: 200,
                allowPointSelect: true,
                cursor: 'pointer',
                colors: pieColors,
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    distance: -40,
                    fontSize: 25,
                    style: {
                        fontSize: '18px',
                        textOutline: '0',
                        fontFamily: 'poppins',
                        fontWeight: '300'
                    },
                    filter: {
                        property: 'percentage',
                        operator: '>',
                        value: 16
                    }
                },
            }
        },
        series: [{
            name: '',
            borderWidth: 3,
            data: [
                { name: 'Un-Avoidable', y: 60 },
                { name: 'ATT', y: 48 },
                { name: 'ATT Un-Avoidable', y: 53 },
                { name: 'Nokia ', y: 49 },
                { name: 'Nokia Un-Avoidable', y: 68 }
            ]
        }]
    });
});


/* ===== Calendar Controller ==== */
airviewXcelerateApp.directive('calendar', function () {
    function link(scope, element, attrs) {
        if ($(element).is("#calendar")) {
            $(element).fullCalendar({
                header: {
                    right: 'next',
                    center: 'title',
                    left: 'prev'
                },
                defaultDate: '2017-11-12',
                navLinks: true, // can click day/week names to navigate views
                selectable: false,
                selectHelper: false,
                height: 530,
                select: function (start, end) {
                    var title = prompt('Event Title:');
                    var eventData;
                    if (title) {
                        eventData = {
                            title: title,
                            start: start,
                            end: end
                        };
                        $(element).fullCalendar('renderEvent', eventData, true); // stick? = true
                    }
                    $(element).fullCalendar('unselect');
                },
                editable: false,
                eventLimit: true, // for all non-agenda views
                views: {
                    agenda: {
                        eventLimit: 1// adjust to 6 only for agendaWeek/agendaDay
                    }
                },
                events: [
                    {
                        title: 'All Day Event',
                        start: '2017-11-01'
                    },
                    {
                        id: 999,
                        title: 'Repeating Event',
                        start: '2017-11-09T16:00:00'
                    },
                    {
                        id: 999,
                        title: 'Repeating Event',
                        start: '2017-11-09T16:00:00'
                    },
                    {
                        title: 'Conference',
                        start: '2017-11-11',
                        end: '2017-11-13'
                    },

                    {
                        title: 'Click for Google',
                        url: 'http://google.com/',
                        start: '2017-11-28'
                    }
                ]
            });
        }

        if ($(element).is("#calendaradvanced")) {
            $(".add-event").on("click", function () {
                if ($("input.eventname").val() !== "") {
                    var eventname = $(this).parents("form").find("input.eventname").val();
                    $(".fc-event-list").append("<div class='fc-event'>" + eventname + "</div>");
                }
                externalEvents();
                return false;
            });

            function externalEvents() {
                $('#external-events .fc-event').each(function () {
                    $(this).data('event', {
                        title: $.trim($(this).text()), // use the element's text as the event title
                        stick: true // maintain when user navigates (see docs on the renderEvent method)
                    });
                    $(this).draggable({
                        zIndex: 999,
                        revert: true,      // will cause the event to go back to its
                        revertDuration: 0  //  original position after the drag
                    });

                });
            }
            externalEvents();

            $(element).fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                editable: true,
                droppable: true, // this allows things to be dropped onto the calendar
                drop: function () {
                    // is the "remove after drop" checkbox checked?
                    if ($('#drop-remove').is(':checked')) {
                        // if so, remove the element from the "Draggable Events" list
                        $(this).remove();
                    }
                }
            });
        }
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Map Controller ==== */
airviewXcelerateApp.controller('map', function ($scope) {
    xl_script.map();
});



/* ===== Highcharts Directive ==== */
airviewXcelerateApp.directive('highcharts', function () {
    function link(scope, element, attrs) {
        if ($(element).is("#time-series")) {
            $.getJSON(
                'https://cdn.rawgit.com/highcharts/highcharts/057b672172ccc6c08fe7dbb27fc17ebca3f5b770/samples/data/usdeur.json',

                function (data) {

                    Highcharts.chart('time-series', {
                        chart: {
                            zoomType: 'x'
                        },
                        title: {
                            text: 'USD to EUR exchange rate over time'
                        },
                        subtitle: {
                            text: document.ontouchstart === undefined ?
                                'Click and drag in the plot area to zoom in' : 'Pinch the chart to zoom in'
                        },
                        xAxis: {
                            type: 'datetime'
                        },
                        yAxis: {
                            title: {
                                text: 'Exchange rate'
                            }
                        },
                        legend: {
                            enabled: false
                        },
                        plotOptions: {
                            area: {
                                fillColor: {
                                    linearGradient: {
                                        x1: 0,
                                        y1: 0,
                                        x2: 0,
                                        y2: 1
                                    },
                                    stops: [
                                        [0, Highcharts.getOptions().colors[0]],
                                        [1, Highcharts.Color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                                    ]
                                },
                                marker: {
                                    radius: 2
                                },
                                lineWidth: 1,
                                states: {
                                    hover: {
                                        lineWidth: 1
                                    }
                                },
                                threshold: null
                            }
                        },

                        series: [{
                            type: 'area',
                            name: 'USD to EUR',
                            data: data
                        }]
                    });
                    document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
                }
            );
        }

        if ($(element).is("#basic-area")) {
            /* Chart 2 */
            Highcharts.chart('basic-area', {
                chart: {
                    type: 'area'
                },
                title: {
                    text: 'US and USSR nuclear stockpiles'
                },
                subtitle: {
                    text: 'Sources: <a href="https://thebulletin.org/2006/july/global-nuclear-stockpiles-1945-2006">' +
                        'thebulletin.org</a> &amp; <a href="https://www.armscontrol.org/factsheets/Nuclearweaponswhohaswhat">' +
                        'armscontrol.org</a>'
                },
                xAxis: {
                    allowDecimals: false,
                    labels: {
                        formatter: function () {
                            return this.value; // clean, unformatted number for year
                        }
                    }
                },
                yAxis: {
                    title: {
                        text: 'Nuclear weapon states'
                    },
                    labels: {
                        formatter: function () {
                            return this.value / 1000 + 'k';
                        }
                    }
                },
                tooltip: {
                    pointFormat: '{series.name} had stockpiled <b>{point.y:,.0f}</b><br/>warheads in {point.x}'
                },
                plotOptions: {
                    area: {
                        pointStart: 1940,
                        marker: {
                            enabled: false,
                            symbol: 'circle',
                            radius: 2,
                            states: {
                                hover: {
                                    enabled: true
                                }
                            }
                        }
                    }
                },
                series: [{
                    name: 'USA',
                    data: [
                        null, null, null, null, null, 6, 11, 32, 110, 235,
                        369, 640, 1005, 1436, 2063, 3057, 4618, 6444, 9822, 15468,
                        20434, 24126, 27387, 29459, 31056, 31982, 32040, 31233, 29224, 27342,
                        26662, 26956, 27912, 28999, 28965, 27826, 25579, 25722, 24826, 24605,
                        24304, 23464, 23708, 24099, 24357, 24237, 24401, 24344, 23586, 22380,
                        21004, 17287, 14747, 13076, 12555, 12144, 11009, 10950, 10871, 10824,
                        10577, 10527, 10475, 10421, 10358, 10295, 10104, 9914, 9620, 9326,
                        5113, 5113, 4954, 4804, 4761, 4717, 4368, 4018
                    ]
                }, {
                    name: 'USSR/Russia',
                    data: [null, null, null, null, null, null, null, null, null, null,
                        5, 25, 50, 120, 150, 200, 426, 660, 869, 1060,
                        1605, 2471, 3322, 4238, 5221, 6129, 7089, 8339, 9399, 10538,
                        11643, 13092, 14478, 15915, 17385, 19055, 21205, 23044, 25393, 27935,
                        30062, 32049, 33952, 35804, 37431, 39197, 45000, 43000, 41000, 39000,
                        37000, 35000, 33000, 31000, 29000, 27000, 25000, 24000, 23000, 22000,
                        21000, 20000, 19000, 18000, 18000, 17000, 16000, 15537, 14162, 12787,
                        12600, 11400, 5500, 4512, 4502, 4502, 4500, 4500
                    ]
                }]
            });
        }

        if ($(element).is("#stacked-bar")) {
            /* Chart 3 */
            Highcharts.chart('stacked-bar', {
                chart: {
                    type: 'bar'
                },
                title: {
                    text: 'Stacked bar chart'
                },
                xAxis: {
                    categories: ['Apples', 'Oranges', 'Pears', 'Grapes', 'Bananas']
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Total fruit consumption'
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: 'John',
                    data: [5, 3, 4, 7, 2]
                }, {
                    name: 'Jane',
                    data: [2, 2, 3, 2, 1]
                }, {
                    name: 'Joe',
                    data: [3, 4, 4, 2, 5]
                }]
            });
        }

        if ($(element).is("#basic-column")) {
            /* Chart 4 */
            Highcharts.chart('basic-column', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Monthly Average Rainfall'
                },
                subtitle: {
                    text: 'Source: WorldClimate.com'
                },
                xAxis: {
                    categories: [
                        'Jan',
                        'Feb',
                        'Mar',
                        'Apr',
                        'May',
                        'Jun',
                        'Jul',
                        'Aug',
                        'Sep',
                        'Oct',
                        'Nov',
                        'Dec'
                    ],
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Rainfall (mm)'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: [{
                    name: 'Tokyo',
                    data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]

                }, {
                    name: 'New York',
                    data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6, 92.3]

                }, {
                    name: 'London',
                    data: [48.9, 38.8, 39.3, 41.4, 47.0, 48.3, 59.0, 59.6, 52.4, 65.2, 59.3, 51.2]

                }, {
                    name: 'Berlin',
                    data: [42.4, 33.2, 34.5, 39.7, 52.6, 75.5, 57.4, 60.4, 47.6, 39.1, 46.8, 51.1]

                }]
            });
        }

        if ($(element).is("#column-with-negative-values")) {
            /* Chart 5 */
            Highcharts.chart('column-with-negative-values', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Column chart with negative values'
                },
                xAxis: {
                    categories: ['Apples', 'Oranges', 'Pears', 'Grapes', 'Bananas']
                },
                credits: {
                    enabled: false
                },
                series: [{
                    name: 'John',
                    data: [5, 3, 4, 7, 2]
                }, {
                    name: 'Jane',
                    data: [2, -2, -3, 2, 1]
                }, {
                    name: 'Joe',
                    data: [3, 4, 4, -2, 5]
                }]

            });
        }

        if ($(element).is("#donut-chart")) {
            /* Chart 6 */
            var colors = Highcharts.getOptions().colors,
                categories = ['MSIE', 'Firefox', 'Chrome', 'Safari', 'Opera'],
                data = [{
                    y: 56.33,
                    color: colors[0],
                    drilldown: {
                        name: 'MSIE versions',
                        categories: ['MSIE 6.0', 'MSIE 7.0', 'MSIE 8.0', 'MSIE 9.0',
                            'MSIE 10.0', 'MSIE 11.0'],
                        data: [1.06, 0.5, 17.2, 8.11, 5.33, 24.13],
                        color: colors[0]
                    }
                }, {
                    y: 10.38,
                    color: colors[1],
                    drilldown: {
                        name: 'Firefox versions',
                        categories: ['Firefox v31', 'Firefox v32', 'Firefox v33',
                            'Firefox v35', 'Firefox v36', 'Firefox v37', 'Firefox v38'],
                        data: [0.33, 0.15, 0.22, 1.27, 2.76, 2.32, 2.31, 1.02],
                        color: colors[1]
                    }
                }, {
                    y: 24.03,
                    color: colors[2],
                    drilldown: {
                        name: 'Chrome versions',
                        categories: ['Chrome v30.0', 'Chrome v31.0', 'Chrome v32.0',
                            'Chrome v33.0', 'Chrome v34.0',
                            'Chrome v35.0', 'Chrome v36.0', 'Chrome v37.0', 'Chrome v38.0',
                            'Chrome v39.0', 'Chrome v40.0', 'Chrome v41.0', 'Chrome v42.0',
                            'Chrome v43.0'],
                        data: [0.14, 1.24, 0.55, 0.19, 0.14, 0.85, 2.53, 0.38, 0.6, 2.96,
                            5, 4.32, 3.68, 1.45],
                        color: colors[2]
                    }
                }, {
                    y: 4.77,
                    color: colors[3],
                    drilldown: {
                        name: 'Safari versions',
                        categories: ['Safari v5.0', 'Safari v5.1', 'Safari v6.1',
                            'Safari v6.2', 'Safari v7.0', 'Safari v7.1', 'Safari v8.0'],
                        data: [0.3, 0.42, 0.29, 0.17, 0.26, 0.77, 2.56],
                        color: colors[3]
                    }
                }, {
                    y: 0.91,
                    color: colors[4],
                    drilldown: {
                        name: 'Opera versions',
                        categories: ['Opera v12.x', 'Opera v27', 'Opera v28', 'Opera v29'],
                        data: [0.34, 0.17, 0.24, 0.16],
                        color: colors[4]
                    }
                }, {
                    y: 0.2,
                    color: colors[5],
                    drilldown: {
                        name: 'Proprietary or Undetectable',
                        categories: [],
                        data: [],
                        color: colors[5]
                    }
                }],
                browserData = [],
                versionsData = [],
                i,
                j,
                dataLen = data.length,
                drillDataLen,
                brightness;
            for (i = 0; i < dataLen; i += 1) {

                // add browser data
                browserData.push({
                    name: categories[i],
                    y: data[i].y,
                    color: data[i].color
                });

                // add version data
                drillDataLen = data[i].drilldown.data.length;
                for (j = 0; j < drillDataLen; j += 1) {
                    brightness = 0.2 - (j / drillDataLen) / 5;
                    versionsData.push({
                        name: data[i].drilldown.categories[j],
                        y: data[i].drilldown.data[j],
                        color: Highcharts.Color(data[i].color).brighten(brightness).get()
                    });
                }
            }
            Highcharts.chart('donut-chart', {
                chart: {
                    type: 'pie'
                },
                title: {
                    text: 'Browser market share, January, 2015 to May, 2015'
                },
                subtitle: {
                    text: 'Source: <a href="http://netmarketshare.com/">netmarketshare.com</a>'
                },
                yAxis: {
                    title: {
                        text: 'Total percent market share'
                    }
                },
                plotOptions: {
                    pie: {
                        shadow: false,
                        center: ['50%', '50%']
                    }
                },
                tooltip: {
                    valueSuffix: '%'
                },
                series: [{
                    name: 'Browsers',
                    data: browserData,
                    size: '60%',
                    dataLabels: {
                        formatter: function () {
                            return this.y > 5 ? this.point.name : null;
                        },
                        color: '#ffffff',
                        distance: -30
                    }
                }, {
                    name: 'Versions',
                    data: versionsData,
                    size: '80%',
                    innerSize: '60%',
                    dataLabels: {
                        formatter: function () {
                            // display only if larger than 1
                            return this.y > 1 ? '<b>' + this.point.name + ':</b> ' +
                                this.y + '%' : null;
                        }
                    },
                    id: 'versions'
                }],
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 400
                        },
                        chartOptions: {
                            series: [{
                                id: 'versions',
                                dataLabels: {
                                    enabled: false
                                }
                            }]
                        }
                    }]
                }
            });
        }

        if ($(element).is("#pie-with-monochrom-fill")) {
            /* Chart 7 */
            var pieColors = (function () {
                var colors = [],
                    base = Highcharts.getOptions().colors[0],
                    i;

                for (i = 0; i < 10; i += 1) {
                    // Start out with a darkened base color (negative brighten), and end
                    // up with a much brighter color
                    colors.push(Highcharts.Color(base).brighten((i - 3) / 7).get());
                }
                return colors;
            }());
            Highcharts.chart('pie-with-monochrom-fill', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Browser market shares at a specific website, 2014'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        colors: pieColors,
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b><br>{point.percentage:.1f} %',
                            distance: -50,
                            filter: {
                                property: 'percentage',
                                operator: '>',
                                value: 4
                            }
                        }
                    }
                },
                series: [{
                    name: 'Brands',
                    data: [
                        { name: 'IE', y: 56.33 },
                        { name: 'Chrome', y: 24.03 },
                        { name: 'Firefox', y: 10.38 },
                        { name: 'Safari', y: 4.77 },
                        { name: 'Opera', y: 0.91 },
                        { name: 'Other', y: 0.2 }
                    ]
                }]
            });
        }

        if ($(element).is("#dual-axis-line-column")) {
            /* Chart 8 */
            Highcharts.chart('dual-axis-line-column', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text: 'Average Monthly Weather Data for Tokyo'
                },
                subtitle: {
                    text: 'Source: WorldClimate.com'
                },
                xAxis: [{
                    categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                        'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                    crosshair: true
                }],
                yAxis: [{ // Primary yAxis
                    labels: {
                        format: '{value}°C',
                        style: {
                            color: Highcharts.getOptions().colors[2]
                        }
                    },
                    title: {
                        text: 'Temperature',
                        style: {
                            color: Highcharts.getOptions().colors[2]
                        }
                    },
                    opposite: true

                }, { // Secondary yAxis
                    gridLineWidth: 0,
                    title: {
                        text: 'Rainfall',
                        style: {
                            color: Highcharts.getOptions().colors[0]
                        }
                    },
                    labels: {
                        format: '{value} mm',
                        style: {
                            color: Highcharts.getOptions().colors[0]
                        }
                    }

                }, { // Tertiary yAxis
                    gridLineWidth: 0,
                    title: {
                        text: 'Sea-Level Pressure',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    labels: {
                        format: '{value} mb',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    opposite: true
                }],
                tooltip: {
                    shared: true
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    x: 80,
                    verticalAlign: 'top',
                    y: 55,
                    floating: true,
                    backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
                },
                series: [{
                    name: 'Rainfall',
                    type: 'column',
                    yAxis: 1,
                    data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4],
                    tooltip: {
                        valueSuffix: ' mm'
                    }

                }, {
                    name: 'Sea-Level Pressure',
                    type: 'spline',
                    yAxis: 2,
                    data: [1016, 1016, 1015.9, 1015.5, 1012.3, 1009.5, 1009.6, 1010.2, 1013.1, 1016.9, 1018.2, 1016.7],
                    marker: {
                        enabled: false
                    },
                    dashStyle: 'shortdot',
                    tooltip: {
                        valueSuffix: ' mb'
                    }

                }, {
                    name: 'Temperature',
                    type: 'spline',
                    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6],
                    tooltip: {
                        valueSuffix: ' °C'
                    }
                }]
            });
        }

        if ($(element).is("#column-line-pie")) {
            /* Chart 9 */
            Highcharts.chart('column-line-pie', {
                title: {
                    text: 'Combination chart'
                },
                xAxis: {
                    categories: ['Apples', 'Oranges', 'Pears', 'Bananas', 'Plums']
                },
                labels: {
                    items: [{
                        html: 'Total fruit consumption',
                        style: {
                            left: '50px',
                            top: '18px',
                            color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                        }
                    }]
                },
                series: [{
                    type: 'column',
                    name: 'Jane',
                    data: [3, 2, 1, 3, 4]
                }, {
                    type: 'column',
                    name: 'John',
                    data: [2, 3, 5, 7, 6]
                }, {
                    type: 'column',
                    name: 'Joe',
                    data: [4, 3, 3, 9, 0]
                }, {
                    type: 'spline',
                    name: 'Average',
                    data: [3, 2.67, 3, 6.33, 3.33],
                    marker: {
                        lineWidth: 2,
                        lineColor: Highcharts.getOptions().colors[3],
                        fillColor: 'white'
                    }
                }, {
                    type: 'pie',
                    name: 'Total consumption',
                    data: [{
                        name: 'Jane',
                        y: 13,
                        color: Highcharts.getOptions().colors[0] // Jane's color
                    }, {
                        name: 'John',
                        y: 23,
                        color: Highcharts.getOptions().colors[1] // John's color
                    }, {
                        name: 'Joe',
                        y: 19,
                        color: Highcharts.getOptions().colors[2] // Joe's color
                    }],
                    center: [100, 80],
                    size: 100,
                    showInLegend: false,
                    dataLabels: {
                        enabled: false
                    }
                }]
            });
        }

        if ($(element).is("#multiple-axis")) {
            /* Chart 10 */
            Highcharts.chart('multiple-axis', {
                chart: {
                    zoomType: 'xy'
                },
                title: {
                    text: 'Average Monthly Temperature and Rainfall in Tokyo'
                },
                subtitle: {
                    text: 'Source: WorldClimate.com'
                },
                xAxis: [{
                    categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                        'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                    crosshair: true
                }],
                yAxis: [{ // Primary yAxis
                    labels: {
                        format: '{value}°C',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    title: {
                        text: 'Temperature',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    }
                }, { // Secondary yAxis
                    title: {
                        text: 'Rainfall',
                        style: {
                            color: Highcharts.getOptions().colors[0]
                        }
                    },
                    labels: {
                        format: '{value} mm',
                        style: {
                            color: Highcharts.getOptions().colors[0]
                        }
                    },
                    opposite: true
                }],
                tooltip: {
                    shared: true
                },
                legend: {
                    layout: 'vertical',
                    align: 'left',
                    x: 120,
                    verticalAlign: 'top',
                    y: 100,
                    floating: true,
                    backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
                },
                series: [{
                    name: 'Rainfall',
                    type: 'column',
                    yAxis: 1,
                    data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4],
                    tooltip: {
                        valueSuffix: ' mm'
                    }

                }, {
                    name: 'Temperature',
                    type: 'spline',
                    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6],
                    tooltip: {
                        valueSuffix: '°C'
                    }
                }]
            });
        }

        if ($(element).is("#3d-column-with-null-value")) {
            /* Chart 11 */
            Highcharts.chart('3d-column-with-null-value', {
                chart: {
                    type: 'column',
                    options3d: {
                        enabled: true,
                        alpha: 10,
                        beta: 25,
                        depth: 70
                    }
                },
                title: {
                    text: '3D chart with null values'
                },
                subtitle: {
                    text: 'Notice the difference between a 0 value and a null point'
                },
                plotOptions: {
                    column: {
                        depth: 25
                    }
                },
                xAxis: {
                    categories: Highcharts.getOptions().lang.shortMonths,
                    labels: {
                        skew3d: true,
                        style: {
                            fontSize: '16px'
                        }
                    }
                },
                yAxis: {
                    title: {
                        text: null
                    }
                },
                series: [{
                    name: 'Sales',
                    data: [2, 3, null, 4, 0, 5, 1, 4, 6, 3]
                }]
            });
        }

        if ($(element).is("#3d-donut")) {
            /* Chart 12 */
            Highcharts.chart('3d-donut', {
                chart: {
                    type: 'pie',
                    options3d: {
                        enabled: true,
                        alpha: 45
                    }
                },
                title: {
                    text: 'Contents of Highsoft\'s weekly fruit delivery'
                },
                subtitle: {
                    text: '3D donut in Highcharts'
                },
                plotOptions: {
                    pie: {
                        innerSize: 100,
                        depth: 45
                    }
                },
                series: [{
                    name: 'Delivered amount',
                    data: [
                        ['Bananas', 8],
                        ['Kiwi', 3],
                        ['Mixed nuts', 1],
                        ['Oranges', 6],
                        ['Apples', 8],
                        ['Pears', 4],
                        ['Clementines', 4],
                        ['Reddish (bag)', 1],
                        ['Grapes (bunch)', 1]
                    ]
                }]
            });
        }

        if ($(element).is("#polar-chart")) {
            /* Chart 13 */
            Highcharts.chart('polar-chart', {

                chart: {
                    polar: true
                },

                title: {
                    text: 'Highcharts Polar Chart'
                },

                pane: {
                    startAngle: 0,
                    endAngle: 360
                },

                xAxis: {
                    tickInterval: 45,
                    min: 0,
                    max: 360,
                    labels: {
                        formatter: function () {
                            return this.value + '°';
                        }
                    }
                },

                yAxis: {
                    min: 0
                },

                plotOptions: {
                    series: {
                        pointStart: 0,
                        pointInterval: 45
                    },
                    column: {
                        pointPadding: 0,
                        groupPadding: 0
                    }
                },

                series: [{
                    type: 'column',
                    name: 'Column',
                    data: [8, 7, 6, 5, 4, 3, 2, 1],
                    pointPlacement: 'between'
                }, {
                    type: 'line',
                    name: 'Line',
                    data: [1, 2, 3, 4, 5, 6, 7, 8]
                }, {
                    type: 'area',
                    name: 'Area',
                    data: [1, 8, 2, 7, 3, 6, 4, 5]
                }]
            });
        }

        if ($(element).is("#spider-web")) {
            /* Chart 14 */
            Highcharts.chart('spider-web', {

                chart: {
                    polar: true,
                    type: 'line'
                },

                title: {
                    text: 'Budget vs spending',
                    x: -80
                },

                pane: {
                    size: '80%'
                },

                xAxis: {
                    categories: ['Sales', 'Marketing', 'Development', 'Customer Support',
                        'Information Technology', 'Administration'],
                    tickmarkPlacement: 'on',
                    lineWidth: 0
                },

                yAxis: {
                    gridLineInterpolation: 'polygon',
                    lineWidth: 0,
                    min: 0
                },

                tooltip: {
                    shared: true,
                    pointFormat: '<span style="color:{series.color}">{series.name}: <b>${point.y:,.0f}</b><br/>'
                },

                legend: {
                    align: 'right',
                    verticalAlign: 'top',
                    y: 70,
                    layout: 'vertical'
                },

                series: [{
                    name: 'Allocated Budget',
                    data: [43000, 19000, 60000, 35000, 17000, 10000],
                    pointPlacement: 'on'
                }, {
                    name: 'Actual Spending',
                    data: [50000, 39000, 42000, 31000, 26000, 14000],
                    pointPlacement: 'on'
                }]

            });
        }

    }
    return {
        restrict: "A",
        link: link
    };
});



/* ===== Data Table Controller ==== */
airviewXcelerateApp.controller('datatable', function ($scope) {

    // DataTable
    var table = $('#datatable').DataTable({
        "dom": '<"top"<"dt-filters"f>>rt<"dt-bottom"<"dt-information"li><"dt-pagination"p>>',
    });


    $('#datatable tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    // Apply the search
    table.columns().every(function () {
        var that = this;
        $('input', this.footer()).on('keyup change', function () {
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });
    var r = $('#datatable tfoot tr');
    $('#datatable thead').prepend(r);


    $('#datatable tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
    });

    $('.toggle-vis').on('click', function () {
        var column = table.column($(this).attr('data-column'));
        column.visible(!column.visible());

        let chkBox = $(this).find("input[type='checkbox']");
        chkBox.prop('checked', !chkBox.is(':checked'));
    });

});





/* ===== Table Controller ==== */
airviewXcelerateApp.controller('table', function ($scope) {
    xl_script.strippedRows();
    xl_script.scrollbar();
});



/* ===== Simple Tags Directive ==== */
airviewXcelerateApp.directive('canvascharts', function ($rootScope) {
    function link(scope, element, attrs) {
        if ($(element).is("#chart_with_animation")) {
            scope.chart = new CanvasJS.Chart("chart_with_animation", {
                theme: "light2", // "light1", "light2", "dark1", "dark2"
                exportEnabled: true,
                animationEnabled: true,
                title: {
                    text: "Desktop Browser Market Share in 2016"
                },
                data: [{
                    type: "pie",
                    startAngle: 25,
                    toolTipContent: "<b>{label}</b>: {y}%",
                    showInLegend: "true",
                    legendText: "{label}",
                    indexLabelFontSize: 16,
                    indexLabel: "{label} - {y}%",
                    dataPoints: [
                        { y: 51.08, label: "Chrome" },
                        { y: 27.34, label: "Internet Explorer" },
                        { y: 10.62, label: "Firefox" },
                        { y: 5.02, label: "Microsoft Edge" },
                        { y: 4.07, label: "Safari" },
                        { y: 1.22, label: "Opera" },
                        { y: 0.44, label: "Others" }
                    ]
                }]
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#dashed_line_chart")) {
            scope.chart = new CanvasJS.Chart("dashed_line_chart", {
                animationEnabled: true,
                theme: "light2",
                title: {
                    text: "Site Traffic"
                },
                axisX: {
                    valueFormatString: "DD MMM",
                    crosshair: {
                        enabled: true,
                        snapToDataPoint: true
                    }
                },
                axisY: {
                    title: "Number of Visits",
                    crosshair: {
                        enabled: true
                    }
                },
                toolTip: {
                    shared: true
                },
                legend: {
                    cursor: "pointer",
                    verticalAlign: "bottom",
                    horizontalAlign: "left",
                    dockInsidePlotArea: true,
                    itemclick: toogleDataSeries
                },
                data: [{
                    type: "line",
                    showInLegend: true,
                    name: "Total Visit",
                    markerType: "square",
                    xValueFormatString: "DD MMM, YYYY",
                    color: "#F08080",
                    dataPoints: [
                        { x: new Date(2017, 0, 3), y: 650 },
                        { x: new Date(2017, 0, 4), y: 700 },
                        { x: new Date(2017, 0, 5), y: 710 },
                        { x: new Date(2017, 0, 6), y: 658 },
                        { x: new Date(2017, 0, 7), y: 734 },
                        { x: new Date(2017, 0, 8), y: 963 },
                        { x: new Date(2017, 0, 9), y: 847 },
                        { x: new Date(2017, 0, 10), y: 853 },
                        { x: new Date(2017, 0, 11), y: 869 },
                        { x: new Date(2017, 0, 12), y: 943 },
                        { x: new Date(2017, 0, 13), y: 970 },
                        { x: new Date(2017, 0, 14), y: 869 },
                        { x: new Date(2017, 0, 15), y: 890 },
                        { x: new Date(2017, 0, 16), y: 930 }
                    ]
                },
                {
                    type: "line",
                    showInLegend: true,
                    name: "Unique Visit",
                    lineDashType: "dash",
                    dataPoints: [
                        { x: new Date(2017, 0, 3), y: 510 },
                        { x: new Date(2017, 0, 4), y: 560 },
                        { x: new Date(2017, 0, 5), y: 540 },
                        { x: new Date(2017, 0, 6), y: 558 },
                        { x: new Date(2017, 0, 7), y: 544 },
                        { x: new Date(2017, 0, 8), y: 693 },
                        { x: new Date(2017, 0, 9), y: 657 },
                        { x: new Date(2017, 0, 10), y: 663 },
                        { x: new Date(2017, 0, 11), y: 639 },
                        { x: new Date(2017, 0, 12), y: 673 },
                        { x: new Date(2017, 0, 13), y: 660 },
                        { x: new Date(2017, 0, 14), y: 562 },
                        { x: new Date(2017, 0, 15), y: 643 },
                        { x: new Date(2017, 0, 16), y: 570 }
                    ]
                }]
            });
            scope.chart.render();
            function toogleDataSeries(e) {
                if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                    e.dataSeries.visible = false;
                } else {
                    e.dataSeries.visible = true;
                }
                scope.chart.render();
            }
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#charts_graph_with_axis_scale_break")) {
            scope.chart = new CanvasJS.Chart("charts_graph_with_axis_scale_break", {
                animationEnabled: true,
                theme: "light2", // "light1", "light2", "dark1", "dark2"
                title: {
                    text: "GDP per Capita - 2016"
                },
                subtitles: [{
                    text: "In USD",
                    fontSize: 16
                }],
                axisY: {
                    prefix: "$",
                    scaleBreaks: {
                        customBreaks: [{
                            startValue: 10000,
                            endValue: 35000
                        }]
                    }
                },
                data: [{
                    type: "column",
                    yValueFormatString: "$#,##0.00",
                    dataPoints: [
                        { label: "USA", y: 57466.787 },
                        { label: "Austraila", y: 49927.82 },
                        { label: "UK", y: 39899.388 },
                        { label: "UAE", y: 37622.207 },
                        { label: "Brazil", y: 8649.948 },
                        { label: "China", y: 8123.181 },
                        { label: "Indonesia", y: 3570.295 },
                        { label: "India", y: 1709.387 }
                    ]
                }]
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#charts_graph_with_crosshair")) {
            scope.chart = new CanvasJS.Chart("charts_graph_with_crosshair", {
                animationEnabled: true,
                title: {
                    text: "Stock Price of BMW - August"
                },
                axisX: {
                    valueFormatString: "DD MMM",
                    crosshair: {
                        enabled: true,
                        snapToDataPoint: true
                    }
                },
                axisY: {
                    title: "Closing Price (in USD)",
                    includeZero: false,
                    valueFormatString: "$##0.00",
                    crosshair: {
                        enabled: true,
                        snapToDataPoint: true,
                        labelFormatter: function (e) {
                            return "$" + CanvasJS.formatNumber(e.value, "##0.00");
                        }
                    }
                },
                data: [{
                    type: "area",
                    xValueFormatString: "DD MMM",
                    yValueFormatString: "$##0.00",
                    dataPoints: [
                        { x: new Date(2016, 07, 01), y: 76.727997 },
                        { x: new Date(2016, 07, 02), y: 75.459999 },
                        { x: new Date(2016, 07, 03), y: 76.011002 },
                        { x: new Date(2016, 07, 04), y: 75.751999 },
                        { x: new Date(2016, 07, 05), y: 77.500000 },
                        { x: new Date(2016, 07, 08), y: 77.436996 },
                        { x: new Date(2016, 07, 09), y: 79.650002 },
                        { x: new Date(2016, 07, 10), y: 79.750999 },
                        { x: new Date(2016, 07, 11), y: 80.169998 },
                        { x: new Date(2016, 07, 12), y: 79.570000 },
                        { x: new Date(2016, 07, 15), y: 80.699997 },
                        { x: new Date(2016, 07, 16), y: 79.686996 },
                        { x: new Date(2016, 07, 17), y: 78.996002 },
                        { x: new Date(2016, 07, 18), y: 78.899002 },
                        { x: new Date(2016, 07, 19), y: 77.127998 },
                        { x: new Date(2016, 07, 22), y: 76.759003 },
                        { x: new Date(2016, 07, 23), y: 77.480003 },
                        { x: new Date(2016, 07, 24), y: 77.623001 },
                        { x: new Date(2016, 07, 25), y: 76.408997 },
                        { x: new Date(2016, 07, 26), y: 76.041000 },
                        { x: new Date(2016, 07, 29), y: 76.778999 },
                        { x: new Date(2016, 07, 30), y: 78.654999 },
                        { x: new Date(2016, 07, 31), y: 77.667000 }
                    ]
                }]
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#chart_with_customized_legends")) {
            scope.chart = new CanvasJS.Chart("chart_with_customized_legends", {
                //theme: "light2", // "light1", "light2", "dark1", "dark2"
                animationEnabled: true,
                title: {
                    text: "Internet users"
                },
                subtitles: [{
                    text: "Try Clicking and Hovering over Legends!"
                }],
                axisX: {
                    lineColor: "black",
                    labelFontColor: "black"
                },
                axisY2: {
                    gridThickness: 0,
                    title: "% of Population",
                    suffix: "%",
                    titleFontColor: "black",
                    labelFontColor: "black"
                },
                legend: {
                    cursor: "pointer",
                    itemmouseover: function (e) {
                        e.dataSeries.lineThickness = e.chart.data[e.dataSeriesIndex].lineThickness * 2;
                        e.dataSeries.markerSize = e.chart.data[e.dataSeriesIndex].markerSize + 2;
                        e.chart.render();
                    },
                    itemmouseout: function (e) {
                        e.dataSeries.lineThickness = e.chart.data[e.dataSeriesIndex].lineThickness / 2;
                        e.dataSeries.markerSize = e.chart.data[e.dataSeriesIndex].markerSize - 2;
                        e.chart.render();
                    },
                    itemclick: function (e) {
                        if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                            e.dataSeries.visible = false;
                        } else {
                            e.dataSeries.visible = true;
                        }
                        e.chart.render();
                    }
                },
                toolTip: {
                    shared: true
                },
                data: [{
                    type: "spline",
                    name: "Sweden",
                    markerSize: 5,
                    axisYType: "secondary",
                    xValueFormatString: "YYYY",
                    yValueFormatString: "#,##0.0'%'",
                    showInLegend: true,
                    dataPoints: [
                        { x: new Date(2000, 00), y: 47.5 },
                        { x: new Date(2005, 00), y: 84.8 },
                        { x: new Date(2009, 00), y: 91 },
                        { x: new Date(2010, 00), y: 90 },
                        { x: new Date(2011, 00), y: 92.8 },
                        { x: new Date(2012, 00), y: 93.2 },
                        { x: new Date(2013, 00), y: 94.8 },
                        { x: new Date(2014, 00), y: 92.5 }
                    ]
                },
                {
                    type: "spline",
                    name: "UK",
                    markerSize: 5,
                    axisYType: "secondary",
                    xValueFormatString: "YYYY",
                    yValueFormatString: "#,##0.0'%'",
                    showInLegend: true,
                    dataPoints: [
                        { x: new Date(2000, 00), y: 26.8 },
                        { x: new Date(2005, 00), y: 70 },
                        { x: new Date(2009, 00), y: 83.6 },
                        { x: new Date(2010, 00), y: 85 },
                        { x: new Date(2011, 00), y: 85.4 },
                        { x: new Date(2012, 00), y: 87.5 },
                        { x: new Date(2013, 00), y: 89.8 },
                        { x: new Date(2014, 00), y: 91.6 }
                    ]
                },
                {
                    type: "spline",
                    name: "UAE",
                    markerSize: 5,
                    axisYType: "secondary",
                    xValueFormatString: "YYYY",
                    yValueFormatString: "#,##0.0'%'",
                    showInLegend: true,
                    dataPoints: [
                        { x: new Date(2000, 00), y: 23.6 },
                        { x: new Date(2005, 00), y: 40 },
                        { x: new Date(2009, 00), y: 64 },
                        { x: new Date(2010, 00), y: 68 },
                        { x: new Date(2011, 00), y: 78 },
                        { x: new Date(2012, 00), y: 85 },
                        { x: new Date(2013, 00), y: 86 },
                        { x: new Date(2014, 00), y: 90.4 }
                    ]
                },
                {
                    type: "spline",
                    showInLegend: true,
                    name: "USA",
                    markerSize: 5,
                    axisYType: "secondary",
                    yValueFormatString: "#,##0.0'%'",
                    xValueFormatString: "YYYY",
                    dataPoints: [
                        { x: new Date(2000, 00), y: 43.1 },
                        { x: new Date(2005, 00), y: 68 },
                        { x: new Date(2009, 00), y: 71 },
                        { x: new Date(2010, 00), y: 71.7 },
                        { x: new Date(2011, 00), y: 69.7 },
                        { x: new Date(2012, 00), y: 79.3 },
                        { x: new Date(2013, 00), y: 84.2 },
                        { x: new Date(2014, 00), y: 87 }
                    ]
                },
                {
                    type: "spline",
                    name: "Switzerland",
                    markerSize: 5,
                    axisYType: "secondary",
                    xValueFormatString: "YYYY",
                    yValueFormatString: "#,##0.0'%'",
                    showInLegend: true,
                    dataPoints: [
                        { x: new Date(2000, 00), y: 47.1 },
                        { x: new Date(2005, 00), y: 70.1 },
                        { x: new Date(2009, 00), y: 81.3 },
                        { x: new Date(2010, 00), y: 83.9 },
                        { x: new Date(2011, 00), y: 85.2 },
                        { x: new Date(2012, 00), y: 85.2 },
                        { x: new Date(2013, 00), y: 86.7 },
                        { x: new Date(2014, 00), y: 87 }
                    ]
                },
                {
                    type: "spline",
                    name: "Honk Kong",
                    markerSize: 5,
                    axisYType: "secondary",
                    xValueFormatString: "YYYY",
                    yValueFormatString: "#,##0.0'%'",
                    showInLegend: true,
                    dataPoints: [
                        { x: new Date(2000, 00), y: 27.8 },
                        { x: new Date(2005, 00), y: 56.9 },
                        { x: new Date(2009, 00), y: 69.4 },
                        { x: new Date(2010, 00), y: 72 },
                        { x: new Date(2011, 00), y: 72.2 },
                        { x: new Date(2012, 00), y: 72.9 },
                        { x: new Date(2013, 00), y: 74.2 },
                        { x: new Date(2014, 00), y: 74.6 }
                    ]
                },
                {
                    type: "spline",
                    name: "Russia",
                    markerSize: 5,
                    axisYType: "secondary",
                    xValueFormatString: "YYYY",
                    yValueFormatString: "#,##0.0'%'",
                    showInLegend: true,
                    dataPoints: [
                        { x: new Date(2000, 00), y: 2 },
                        { x: new Date(2005, 00), y: 15.2 },
                        { x: new Date(2009, 00), y: 29 },
                        { x: new Date(2010, 00), y: 43 },
                        { x: new Date(2011, 00), y: 49 },
                        { x: new Date(2012, 00), y: 63.8 },
                        { x: new Date(2013, 00), y: 61.4 },
                        { x: new Date(2014, 00), y: 70.5 }
                    ]
                },
                {
                    type: "spline",
                    name: "Ukraine",
                    markerSize: 5,
                    axisYType: "secondary",
                    xValueFormatString: "YYYY",
                    yValueFormatString: "#,##0.0'%'",
                    showInLegend: true,
                    dataPoints: [
                        { x: new Date(2000, 00), y: .7 },
                        { x: new Date(2005, 00), y: 3.7 },
                        { x: new Date(2009, 00), y: 17.9 },
                        { x: new Date(2010, 00), y: 23.3 },
                        { x: new Date(2011, 00), y: 28.7 },
                        { x: new Date(2012, 00), y: 35.3 },
                        { x: new Date(2013, 00), y: 41.8 },
                        { x: new Date(2014, 00), y: 43.4 }
                    ]
                },
                {
                    type: "spline",
                    name: "India",
                    markerSize: 5,
                    axisYType: "secondary",
                    xValueFormatString: "YYYY",
                    yValueFormatString: "#,##0.0'%'",
                    showInLegend: true,
                    dataPoints: [
                        { x: new Date(2000, 00), y: .5 },
                        { x: new Date(2005, 00), y: 2.4 },
                        { x: new Date(2009, 00), y: 5.1 },
                        { x: new Date(2010, 00), y: 7.5 },
                        { x: new Date(2011, 00), y: 10.1 },
                        { x: new Date(2012, 00), y: 12.6 },
                        { x: new Date(2013, 00), y: 15.1 },
                        { x: new Date(2014, 00), y: 18 }
                    ]
                }]
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#line_chart_with_zoom_pan")) {
            scope.chart = new CanvasJS.Chart("line_chart_with_zoom_pan", {
                theme: "light2", // "light1", "light2", "dark1", "dark2"
                animationEnabled: true,
                zoomEnabled: true,
                title: {
                    text: "Try Zooming and Panning"
                },
                data: [{
                    type: "area",
                    dataPoints: []
                }]
            });
            addDataPoints(1000);
            scope.chart.render();
            function addDataPoints(noOfDps) {
                var xVal = scope.chart.options.data[0].dataPoints.length + 1, yVal = 100;
                for (var i = 0; i < noOfDps; i++) {
                    yVal = yVal + Math.round(5 + Math.random() * (-5 - 5));
                    scope.chart.options.data[0].dataPoints.push({ x: xVal, y: yVal });
                    xVal++;
                }
            }
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#dynamic_chart")) {

            var dps = []; // dataPoints
            scope.chart = new CanvasJS.Chart("dynamic_chart", {
                title: {
                    text: "Dynamic Data"
                },
                axisY: {
                    includeZero: false
                },
                data: [{
                    type: "line",
                    dataPoints: dps
                }]
            });

            var xVal = 0;
            var yVal = 100;
            var updateInterval = 1000;
            var dataLength = 20; // number of dataPoints visible at any point
            var updateChart = function (count) {

                count = count || 1;

                for (var j = 0; j < count; j++) {
                    yVal = yVal + Math.round(5 + Math.random() * (-5 - 5));
                    dps.push({
                        x: xVal,
                        y: yVal
                    });
                    xVal++;
                }

                if (dps.length > dataLength) {
                    dps.shift();
                }

                scope.chart.render();
            };
            updateChart(dataLength);
            setInterval(function () { updateChart() }, updateInterval);
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#multi_series_chart")) {
            scope.chart = new CanvasJS.Chart("multi_series_chart", {
                animationEnabled: true,
                title: {
                    text: "Daily High Temperature at Different Beaches"
                },
                axisX: {
                    valueFormatString: "DD MMM,YY"
                },
                axisY: {
                    title: "Temperature (in °C)",
                    includeZero: false,
                    suffix: " °C"
                },
                legend: {
                    cursor: "pointer",
                    fontSize: 16,
                    itemclick: toggleDataSeries
                },
                toolTip: {
                    shared: true
                },
                data: [{
                    name: "Myrtle Beach",
                    type: "spline",
                    yValueFormatString: "#0.## °C",
                    showInLegend: true,
                    dataPoints: [
                        { x: new Date(2017, 6, 24), y: 31 },
                        { x: new Date(2017, 6, 25), y: 31 },
                        { x: new Date(2017, 6, 26), y: 29 },
                        { x: new Date(2017, 6, 27), y: 29 },
                        { x: new Date(2017, 6, 28), y: 31 },
                        { x: new Date(2017, 6, 29), y: 30 },
                        { x: new Date(2017, 6, 30), y: 29 }
                    ]
                },
                {
                    name: "Martha Vineyard",
                    type: "spline",
                    yValueFormatString: "#0.## °C",
                    showInLegend: true,
                    dataPoints: [
                        { x: new Date(2017, 6, 24), y: 20 },
                        { x: new Date(2017, 6, 25), y: 20 },
                        { x: new Date(2017, 6, 26), y: 25 },
                        { x: new Date(2017, 6, 27), y: 25 },
                        { x: new Date(2017, 6, 28), y: 25 },
                        { x: new Date(2017, 6, 29), y: 25 },
                        { x: new Date(2017, 6, 30), y: 25 }
                    ]
                },
                {
                    name: "Nantucket",
                    type: "spline",
                    yValueFormatString: "#0.## °C",
                    showInLegend: true,
                    dataPoints: [
                        { x: new Date(2017, 6, 24), y: 22 },
                        { x: new Date(2017, 6, 25), y: 19 },
                        { x: new Date(2017, 6, 26), y: 23 },
                        { x: new Date(2017, 6, 27), y: 24 },
                        { x: new Date(2017, 6, 28), y: 24 },
                        { x: new Date(2017, 6, 29), y: 23 },
                        { x: new Date(2017, 6, 30), y: 23 }
                    ]
                }]
            });
            scope.chart.render();

            function toggleDataSeries(e) {
                if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                    e.dataSeries.visible = false;
                }
                else {
                    e.dataSeries.visible = true;
                }
                scope.chart.render();
            }
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#performance_with_large_number_of_datapoints")) {
            var dataPoints = [];
            var y = 1000;
            var limit = 50000;

            for (var i = 0; i < limit; i++) {
                y += Math.round(10 + Math.random() * (-10 - 10));
                dataPoints.push({ y: y });
            }

            scope.chart = new CanvasJS.Chart("performance_with_large_number_of_datapoints", {
                animationEnabled: true,
                zoomEnabled: true,
                title: {
                    text: "Performance Demo with 50,000 Data Points"
                },
                subtitles: [{
                    text: "Try Zooming and Panning"
                }],
                data: [{
                    type: "line",
                    dataPoints: dataPoints
                }],
                axisY: {
                    includeZero: false
                }
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#drilldown_chart")) {

            var totalVisitors = 883000;
            var visitorsData = {
                "New vs Returning Visitors": [{
                    click: visitorsChartDrilldownHandler,
                    cursor: "pointer",
                    explodeOnClick: false,
                    innerRadius: "75%",
                    legendMarkerType: "square",
                    name: "New vs Returning Visitors",
                    radius: "100%",
                    showInLegend: true,
                    startAngle: 90,
                    type: "doughnut",
                    dataPoints: [
                        { y: 519960, name: "New Visitors", color: "#E7823A" },
                        { y: 363040, name: "Returning Visitors", color: "#546BC1" }
                    ]
                }],
                "New Visitors": [{
                    color: "#E7823A",
                    name: "New Visitors",
                    type: "column",
                    dataPoints: [
                        { x: new Date("1 Jan 2015"), y: 33000 },
                        { x: new Date("1 Feb 2015"), y: 35960 },
                        { x: new Date("1 Mar 2015"), y: 42160 },
                        { x: new Date("1 Apr 2015"), y: 42240 },
                        { x: new Date("1 May 2015"), y: 43200 },
                        { x: new Date("1 Jun 2015"), y: 40600 },
                        { x: new Date("1 Jul 2015"), y: 42560 },
                        { x: new Date("1 Aug 2015"), y: 44280 },
                        { x: new Date("1 Sep 2015"), y: 44800 },
                        { x: new Date("1 Oct 2015"), y: 48720 },
                        { x: new Date("1 Nov 2015"), y: 50840 },
                        { x: new Date("1 Dec 2015"), y: 51600 }
                    ]
                }],
                "Returning Visitors": [{
                    color: "#546BC1",
                    name: "Returning Visitors",
                    type: "column",
                    dataPoints: [
                        { x: new Date("1 Jan 2015"), y: 22000 },
                        { x: new Date("1 Feb 2015"), y: 26040 },
                        { x: new Date("1 Mar 2015"), y: 25840 },
                        { x: new Date("1 Apr 2015"), y: 23760 },
                        { x: new Date("1 May 2015"), y: 28800 },
                        { x: new Date("1 Jun 2015"), y: 29400 },
                        { x: new Date("1 Jul 2015"), y: 33440 },
                        { x: new Date("1 Aug 2015"), y: 37720 },
                        { x: new Date("1 Sep 2015"), y: 35200 },
                        { x: new Date("1 Oct 2015"), y: 35280 },
                        { x: new Date("1 Nov 2015"), y: 31160 },
                        { x: new Date("1 Dec 2015"), y: 34400 }
                    ]
                }]
            };

            var newVSReturningVisitorsOptions = {
                animationEnabled: true,
                theme: "light2",
                title: {
                    text: "New VS Returning Visitors"
                },
                subtitles: [{
                    text: "Click on Any Segment to Drilldown",
                    backgroundColor: "#2eacd1",
                    fontSize: 16,
                    fontColor: "white",
                    padding: 5
                }],
                legend: {
                    fontFamily: "calibri",
                    fontSize: 14,
                    itemTextFormatter: function (e) {
                        return e.dataPoint.name + ": " + Math.round(e.dataPoint.y / totalVisitors * 100) + "%";
                    }
                },
                data: []
            };

            var visitorsDrilldownedChartOptions = {
                animationEnabled: true,
                theme: "light2",
                axisX: {
                    labelFontColor: "#717171",
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

            scope.chart = new CanvasJS.Chart("drilldown_chart", newVSReturningVisitorsOptions);
            scope.chart.options.data = visitorsData["New vs Returning Visitors"];
            scope.chart.render();

            function visitorsChartDrilldownHandler(e) {
                chart = new CanvasJS.Chart("drilldown_chart", visitorsDrilldownedChartOptions);
                scope.chart.options.data = visitorsData[e.dataPoint.name];
                scope.chart.options.title = { text: e.dataPoint.name }
                scope.chart.render();
                $("#backButton").toggleClass("invisible");
            }

            $("#backButton").click(function () {
                $(this).toggleClass("invisible");
                chart = new CanvasJS.Chart("drilldown_chart", newVSReturningVisitorsOptions);
                scope.chart.options.data = visitorsData["New vs Returning Visitors"];
                scope.chart.render();
            });

        }

        if ($(element).is("#dynamic_spline_chart")) {
            var dps = [];
            scope.chart = new CanvasJS.Chart("dynamic_spline_chart", {
                exportEnabled: true,
                title: {
                    text: "Dynamic Spline Chart"
                },
                axisY: {
                    includeZero: false
                },
                data: [{
                    type: "spline",
                    markerSize: 0,
                    dataPoints: dps
                }]
            });

            var xVal = 0;
            var yVal = 100;
            var updateInterval = 1000;
            var dataLength = 50; // number of dataPoints visible at any point

            var updateChart = function (count) {
                count = count || 1;
                // count is number of times loop runs to generate random dataPoints.
                for (var j = 0; j < count; j++) {
                    yVal = yVal + Math.round(5 + Math.random() * (-5 - 5));
                    dps.push({
                        x: xVal,
                        y: yVal
                    });
                    xVal++;
                }
                if (dps.length > dataLength) {
                    dps.shift();
                }
                scope.chart.render();
            };

            updateChart(dataLength);
            setInterval(function () { updateChart() }, updateInterval);

            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#line_chart_with_line_markers")) {
            scope.chart = new CanvasJS.Chart("line_chart_with_line_markers", {
                theme: "light2", // "light1", "light2", "dark1", "dark2"
                animationEnabled: true,
                title: {
                    text: "Share Value - 2016"
                },
                axisX: {
                    interval: 1,
                    intervalType: "month",
                    valueFormatString: "MMM"
                },
                axisY: {
                    title: "Price (in USD)",
                    valueFormatString: "$#0"
                },
                data: [{
                    type: "line",
                    markerSize: 12,
                    xValueFormatString: "MMM, YYYY",
                    yValueFormatString: "$###.#",
                    dataPoints: [
                        { x: new Date(2016, 00, 1), y: 61, indexLabel: "gain", markerType: "triangle", markerColor: "#6B8E23" },
                        { x: new Date(2016, 01, 1), y: 71, indexLabel: "gain", markerType: "triangle", markerColor: "#6B8E23" },
                        { x: new Date(2016, 02, 1), y: 55, indexLabel: "loss", markerType: "cross", markerColor: "tomato" },
                        { x: new Date(2016, 03, 1), y: 50, indexLabel: "loss", markerType: "cross", markerColor: "tomato" },
                        { x: new Date(2016, 04, 1), y: 65, indexLabel: "gain", markerType: "triangle", markerColor: "#6B8E23" },
                        { x: new Date(2016, 05, 1), y: 85, indexLabel: "gain", markerType: "triangle", markerColor: "#6B8E23" },
                        { x: new Date(2016, 06, 1), y: 68, indexLabel: "loss", markerType: "cross", markerColor: "tomato" },
                        { x: new Date(2016, 07, 1), y: 28, indexLabel: "loss", markerType: "cross", markerColor: "tomato" },
                        { x: new Date(2016, 08, 1), y: 34, indexLabel: "gain", markerType: "triangle", markerColor: "#6B8E23" },
                        { x: new Date(2016, 09, 1), y: 24, indexLabel: "loss", markerType: "cross", markerColor: "tomato" },
                        { x: new Date(2016, 10, 1), y: 44, indexLabel: "gain", markerType: "triangle", markerColor: "#6B8E23" },
                        { x: new Date(2016, 11, 1), y: 34, indexLabel: "loss", markerType: "cross", markerColor: "tomato" }
                    ]
                }]
            });

            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#line_chart_with_logarithmic_axis")) {

            var step = Math.pow(10, .05);
            scope.chart = new CanvasJS.Chart("line_chart_with_logarithmic_axis", {
                zoomEnabled: true,
                zoomType: "xy",
                exportEnabled: true,
                title: {
                    text: "Frequency Response of Low Pass Filters"
                },
                subtitles: [{
                    text: "X Axis scale is Logarithmic",
                    fontSize: 14
                }],
                axisX: {
                    logarithmic: true,
                    title: "Frequency \u03C9(rad/s)",
                    minimum: .01,
                    suffix: "\u03C9\u2099",
                    stripLines: [{
                        value: 1,
                        label: "Cutoff Frequency",
                        labelFontColor: "#808080",
                        labelAlign: "near"
                    }]
                },
                axisY: {
                    title: "Type 1 Magnitude (db)",
                    titleFontColor: "#4F81BC",
                    labelFontColor: "#4F81BC"
                },
                axisY2: {
                    title: "Type 2 Magnitude (db)",
                    titleFontColor: "#C0504E",
                    labelFontColor: "#C0504E"
                },
                toolTip: {
                    shared: true
                },
                legend: {
                    cursor: "pointer",
                    itemclick: toogleDataSeries
                },
                data: [{
                    type: "line",
                    name: "Type 1 Filter",
                    showInLegend: true,
                    yValueFormatString: "#,##0.00 db",
                    xValueFormatString: "\u03C9 = #,##0.00#\u03C9\u2099",
                    dataPoints: type1DataPoints(step)
                },
                {
                    type: "line",
                    name: "Type 2 Filter",
                    color: "#C0504E",
                    showInLegend: true,
                    axisYType: "secondary",
                    yValueFormatString: "#,##0.00 db",
                    xValueFormatString: "\u03C9 = #,##0.00#\u03C9\u2099",
                    dataPoints: type2DataPoints(.02, step)
                }]
            });

            function type1DataPoints(step) {
                var dataPoints = [];
                var h;
                for (var w = .01; w < 100; w *= step) {
                    h = -5 * Math.log(w * w + 1);
                    dataPoints.push({ x: w, y: h });
                }
                return dataPoints
            }

            function type2DataPoints(e, step) {
                var dataPoints = [];
                var h;
                for (var w = .01; w < 100; w *= step) {
                    h = -5 * Math.log(Math.pow((1 - w * w), 2) + 4 * e * e * w * w);
                    dataPoints.push({ x: w, y: h });
                }
                return dataPoints;
            }

            function toogleDataSeries(e) {
                if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                    e.dataSeries.visible = false;
                } else {
                    e.dataSeries.visible = true;
                }
                scope.chart.render();
            }

            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#multi_series_step_line_chart")) {

            var step = Math.pow(10, .05);
            scope.chart = new CanvasJS.Chart("multi_series_step_line_chart", {
                animationEnabled: true,
                title: {
                    text: "Multi-Series StepLine Chart with Null Data"
                },
                axisX: {
                    valueFormatString: "DD MMM"
                },
                axisY: {
                    includeZero: false
                },
                axisY2: {
                    includeZero: false,
                    minimum: 25
                },
                toolTip: {
                    shared: true
                },
                data: [{
                    type: "stepLine",
                    connectNullData: true,
                    xValueFormatString: "MMM",
                    dataPoints: [
                        { x: new Date(2008, 02), y: 15.00 },
                        { x: new Date(2008, 03), y: 14.50 },
                        { x: new Date(2008, 04), y: 14.00 },
                        { x: new Date(2008, 05), y: 14.50 },
                        { x: new Date(2008, 06), y: 14.75 },
                        { x: new Date(2008, 07), y: null },
                        { x: new Date(2008, 08), y: 15.80 },
                        { x: new Date(2008, 09), y: 17.50 }
                    ]
                },
                {
                    type: "stepLine",
                    axisYType: "secondary",
                    connectNullData: true,
                    xValueFormatString: "MMM",
                    dataPoints: [
                        { x: new Date(2008, 02), y: 41.00 },
                        { x: new Date(2008, 03), y: 43.50 },
                        { x: new Date(2008, 04), y: 41.00 },
                        { x: new Date(2008, 05), y: null },
                        { x: new Date(2008, 06), y: 47.55 },
                        { x: new Date(2008, 07), y: 45.00 },
                        { x: new Date(2008, 08), y: 40.70 },
                        { x: new Date(2008, 09), y: 37.00 }
                    ]
                }]
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#area_chart")) {
            scope.chart = new CanvasJS.Chart("area_chart", {
                animationEnabled: true,
                title: {
                    text: "Number of iPhones Sold in Different Quarters"
                },
                axisX: {
                    minimum: new Date(2015, 01, 25),
                    maximum: new Date(2017, 02, 15),
                    valueFormatString: "MMM YY"
                },
                axisY: {
                    title: "Number of Sales",
                    titleFontColor: "#4F81BC",
                    suffix: "mn"
                },
                data: [{
                    indexLabelFontColor: "darkSlateGray",
                    name: "views",
                    type: "area",
                    yValueFormatString: "#,##0.0mn",
                    dataPoints: [
                        { x: new Date(2015, 02, 1), y: 74.4, label: "Q1-2015" },
                        { x: new Date(2015, 05, 1), y: 61.1, label: "Q2-2015" },
                        { x: new Date(2015, 08, 1), y: 47.0, label: "Q3-2015" },
                        { x: new Date(2015, 11, 1), y: 48.0, label: "Q4-2015" },
                        { x: new Date(2016, 02, 1), y: 74.8, label: "Q1-2016" },
                        { x: new Date(2016, 05, 1), y: 51.1, label: "Q2-2016" },
                        { x: new Date(2016, 08, 1), y: 40.4, label: "Q3-2016" },
                        { x: new Date(2016, 11, 1), y: 45.5, label: "Q4-2016" },
                        { x: new Date(2017, 02, 1), y: 78.3, label: "Q1-2017", indexLabel: "Highest", markerColor: "red" }
                    ]
                }]
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#multi_series_area_chart")) {
            scope.chart = new CanvasJS.Chart("multi_series_area_chart", {
                animationEnabled: true,
                title: {
                    text: "Daily Email Analysis"
                },
                axisX: {
                    valueFormatString: "DDD",
                    minimum: new Date(2017, 1, 5, 23),
                    maximum: new Date(2017, 1, 12, 1)
                },
                axisY: {
                    title: "Number of Messages"
                },
                legend: {
                    verticalAlign: "top",
                    horizontalAlign: "right",
                    dockInsidePlotArea: true
                },
                toolTip: {
                    shared: true
                },
                data: [{
                    name: "Received",
                    showInLegend: true,
                    legendMarkerType: "square",
                    type: "area",
                    color: "rgba(40,175,101,0.6)",
                    markerSize: 0,
                    dataPoints: [
                        { x: new Date(2017, 1, 6), y: 220 },
                        { x: new Date(2017, 1, 7), y: 120 },
                        { x: new Date(2017, 1, 8), y: 144 },
                        { x: new Date(2017, 1, 9), y: 162 },
                        { x: new Date(2017, 1, 10), y: 129 },
                        { x: new Date(2017, 1, 11), y: 109 },
                        { x: new Date(2017, 1, 12), y: 129 }
                    ]
                },
                {
                    name: "Sent",
                    showInLegend: true,
                    legendMarkerType: "square",
                    type: "area",
                    color: "rgba(0,75,141,0.7)",
                    markerSize: 0,
                    dataPoints: [
                        { x: new Date(2017, 1, 6), y: 42 },
                        { x: new Date(2017, 1, 7), y: 34 },
                        { x: new Date(2017, 1, 8), y: 29 },
                        { x: new Date(2017, 1, 9), y: 42 },
                        { x: new Date(2017, 1, 10), y: 53 },
                        { x: new Date(2017, 1, 11), y: 15 },
                        { x: new Date(2017, 1, 12), y: 12 }
                    ]
                }]
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#range_spline_area_chart")) {
            scope.chart = new CanvasJS.Chart("range_spline_area_chart", {
                exportEnabled: true,
                animationEnabled: true,
                title: {
                    text: "Monthly Average Temperature Variation in New Delhi"
                },
                axisX: {
                    valueFormatString: "MMMM"
                },
                axisY: {
                    title: "Temperature (°C)",
                    suffix: " °C"
                },
                data: [{
                    type: "rangeSplineArea",
                    indexLabel: "{y[#index]}°",
                    xValueFormatString: "MMMM YYYY",
                    toolTipContent: "{x} </br> <strong>Temperature: </strong> </br> Min: {y[0]} °C, Max: {y[1]} °C",
                    dataPoints: [
                        { x: new Date(2016, 00, 01), y: [7, 18] },
                        { x: new Date(2016, 01, 01), y: [11, 23] },
                        { x: new Date(2016, 02, 01), y: [15, 28] },
                        { x: new Date(2016, 03, 01), y: [22, 36] },
                        { x: new Date(2016, 04, 01), y: [26, 39] },
                        { x: new Date(2016, 05, 01), y: [27, 37] },
                        { x: new Date(2016, 06, 01), y: [27, 34] },
                        { x: new Date(2016, 07, 01), y: [26, 33] },
                        { x: new Date(2016, 08, 01), y: [24, 33] },
                        { x: new Date(2016, 09, 01), y: [19, 31] },
                        { x: new Date(2016, 10, 01), y: [13, 27] },
                        { x: new Date(2016, 11, 01), y: [08, 21] }
                    ]
                }]
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#stacked_area_chart")) {
            scope.chart = new CanvasJS.Chart("stacked_area_chart", {
                animationEnabled: true,
                title: {
                    text: "Cumulative App Downloads on iTunes and Play Store"
                },
                axisY: {
                    valueFormatString: "#0,.",
                    suffix: "k"
                },
                axisX: {
                    title: "Months After Launch"
                },
                toolTip: {
                    shared: true
                },
                data: [{
                    type: "stackedArea",
                    showInLegend: true,
                    toolTipContent: "<span style='color:#4F81BC'><strong>{name}: </strong></span> {y}",
                    name: "iOS",
                    dataPoints: [
                        { x: 1, y: 3000 },
                        { x: 2, y: 7000 },
                        { x: 3, y: 10000 },
                        { x: 4, y: 14000 },
                        { x: 5, y: 23000 },
                        { x: 6, y: 31000 },
                        { x: 7, y: 42000 },
                        { x: 8, y: 56000 },
                        { x: 9, y: 64000 },
                        { x: 10, y: 81000 },
                        { x: 11, y: 105000 }
                    ]
                },
                {
                    type: "stackedArea",
                    name: "Android",
                    toolTipContent: "<span style='color:#C0504E'><strong>{name}: </strong></span> {y}<br><b>Total:<b> #total",
                    showInLegend: true,
                    dataPoints: [
                        { x: 4, y: 4000 },
                        { x: 5, y: 6000 },
                        { x: 6, y: 9000 },
                        { x: 7, y: 14000 },
                        { x: 8, y: 21000 },
                        { x: 9, y: 31000 },
                        { x: 10, y: 46000 },
                        { x: 11, y: 61000 }
                    ]
                }]
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#step_area_chart")) {
            scope.chart = new CanvasJS.Chart("step_area_chart", {
                animationEnabled: true,
                title: {
                    text: "Customer Satisfaction Based on Reviews"
                },
                axisY: {
                    title: "Satisfied Customers",
                    suffix: "%"
                },
                data: [{
                    type: "stepArea",
                    markerSize: 5,
                    xValueFormatString: "YYYY",
                    yValueFormatString: "#,##0.##'%'",
                    dataPoints: [
                        { x: new Date(2010, 0), y: 34 },
                        { x: new Date(2011, 0), y: 73 },
                        { x: new Date(2012, 0), y: 78 },
                        { x: new Date(2013, 0), y: 82 },
                        { x: new Date(2014, 0), y: 70 },
                        { x: new Date(2015, 0), y: 86 },
                        { x: new Date(2016, 0), y: 80 }
                    ]
                }]
            });

            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#bar_chart_with_axis")) {
            scope.chart = new CanvasJS.Chart("bar_chart_with_axis", {
                animationEnabled: true,
                title: {
                    text: "Military Expenditure of Countries: 2016"
                },
                axisX: {
                    interval: 1
                },
                axisY: {
                    title: "Expenses in Billion Dollars",
                    scaleBreaks: {
                        type: "wavy",
                        customBreaks: [{
                            startValue: 80,
                            endValue: 210
                        },
                        {
                            startValue: 230,
                            endValue: 600
                        }
                        ]
                    }
                },
                data: [{
                    type: "bar",
                    toolTipContent: "<img src='https://canvasjs.com/wp-content/uploads/images/gallery/javascript-column-bar-charts/'{url}' style='width:40px; height:20px;'> <b>{label}</b><br>Budget: ${y}bn<br>{gdp}% of GDP",
                    dataPoints: [
                        { label: "Israel", y: 17.8, gdp: 5.8, url: "israel.png" },
                        { label: "United Arab Emirates", y: 22.8, gdp: 5.7, url: "uae.png" },
                        { label: "Brazil", y: 22.8, gdp: 1.3, url: "brazil.png" },
                        { label: "Australia", y: 24.3, gdp: 2.0, url: "australia.png" },
                        { label: "South Korea", y: 36.8, gdp: 2.7, url: "skorea.png" },
                        { label: "Germany", y: 41.1, gdp: 1.2, url: "germany.png" },
                        { label: "Japan", y: 46.1, gdp: 1.0, url: "japan.png" },
                        { label: "United Kingdom", y: 48.3, gdp: 1.9, url: "uk.png" },
                        { label: "India", y: 55.9, gdp: 2.5, url: "india.png" },
                        { label: "Russia", y: 69.2, gdp: 5.3, url: "russia.png" },
                        { label: "China", y: 215.7, gdp: 1.9, url: "china.png" },
                        { label: "United States", y: 611.2, gdp: 3.3, url: "us.png" }
                    ]
                }]
            });

            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#column_chart_with_multiple_axis")) {
            scope.chart = new CanvasJS.Chart("column_chart_with_multiple_axis", {
                animationEnabled: true,
                title: {
                    text: "Crude Oil Reserves vs Production, 2016"
                },
                axisY: {
                    title: "Billions of Barrels",
                    titleFontColor: "#4F81BC",
                    lineColor: "#4F81BC",
                    labelFontColor: "#4F81BC",
                    tickColor: "#4F81BC"
                },
                axisY2: {
                    title: "Millions of Barrels/day",
                    titleFontColor: "#C0504E",
                    lineColor: "#C0504E",
                    labelFontColor: "#C0504E",
                    tickColor: "#C0504E"
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
                    name: "Proven Oil Reserves (bn)",
                    legendText: "Proven Oil Reserves",
                    showInLegend: true,
                    dataPoints: [
                        { label: "Saudi", y: 266.21 },
                        { label: "Venezuela", y: 302.25 },
                        { label: "Iran", y: 157.20 },
                        { label: "Iraq", y: 148.77 },
                        { label: "Kuwait", y: 101.50 },
                        { label: "UAE", y: 97.8 }
                    ]
                },
                {
                    type: "column",
                    name: "Oil Production (million/day)",
                    legendText: "Oil Production",
                    axisYType: "secondary",
                    showInLegend: true,
                    dataPoints: [
                        { label: "Saudi", y: 10.46 },
                        { label: "Venezuela", y: 2.27 },
                        { label: "Iran", y: 3.99 },
                        { label: "Iraq", y: 4.45 },
                        { label: "Kuwait", y: 2.92 },
                        { label: "UAE", y: 3.1 }
                    ]
                }]
            });
            scope.chart.render();

            function toggleDataSeries(e) {
                if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                    e.dataSeries.visible = false;
                }
                else {
                    e.dataSeries.visible = true;
                }
                $scope.chart.render();
            }

            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#multi_series_range_column_chart")) {
            scope.chart = new CanvasJS.Chart("multi_series_range_column_chart", {
                animationEnabled: true,
                exportEnabled: true,
                title: {
                    text: "Temperature Comparison of Two Cities"
                },
                axisX: {
                    valueFormatString: "MMM"
                },
                axisY: {
                    includeZero: false,
                    suffix: " °C"
                },
                toolTip: {
                    shared: true
                },
                legend: {
                    cursor: "pointer",
                    itemclick: toggleDataSeries
                },
                data: [{
                    type: "rangeColumn",
                    name: "City 1",
                    showInLegend: true,
                    yValueFormatString: "#0.## °C",
                    xValueFormatString: "MMM, YYYY",
                    dataPoints: [
                        { x: new Date(2016, 00), y: [08, 20] },
                        { x: new Date(2016, 01), y: [10, 24] },
                        { x: new Date(2016, 02), y: [16, 29] },
                        { x: new Date(2016, 03), y: [21, 36] },
                        { x: new Date(2016, 04), y: [26, 39] },
                        { x: new Date(2016, 05), y: [22, 39] },
                        { x: new Date(2016, 06), y: [20, 35] },
                        { x: new Date(2016, 07), y: [20, 34] },
                        { x: new Date(2016, 08), y: [20, 34] },
                        { x: new Date(2016, 09), y: [19, 33] },
                        { x: new Date(2016, 10), y: [13, 28] },
                        { x: new Date(2016, 11), y: [09, 23] }
                    ]
                },
                {
                    type: "rangeColumn",
                    name: "City 2",
                    showInLegend: true,
                    yValueFormatString: "#0.## °C",
                    xValueFormatString: "MMM, YYYY",
                    dataPoints: [
                        { x: new Date(2016, 00), y: [16, 28] },
                        { x: new Date(2016, 01), y: [18, 31] },
                        { x: new Date(2016, 02), y: [20, 33] },
                        { x: new Date(2016, 03), y: [22, 34] },
                        { x: new Date(2016, 04), y: [22, 33] },
                        { x: new Date(2016, 05), y: [20, 29] },
                        { x: new Date(2016, 06), y: [20, 28] },
                        { x: new Date(2016, 07), y: [20, 28] },
                        { x: new Date(2016, 08), y: [20, 28] },
                        { x: new Date(2016, 09), y: [20, 28] },
                        { x: new Date(2016, 10), y: [14, 27] },
                        { x: new Date(2016, 11), y: [11, 26] }
                    ]
                }]
            });

            function toggleDataSeries(e) {
                if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                    e.dataSeries.visible = false;
                } else {
                    e.dataSeries.visible = true;
                }
                e.chart.render();
            }

            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#range_bar_chart")) {
            scope.chart = new CanvasJS.Chart("range_bar_chart", {
                animationEnabled: true,
                exportEnabled: true,
                title: {
                    text: "Employees Salary in a Company"
                },
                axisX: {
                    title: "Departments"
                },
                axisY: {
                    includeZero: false,
                    title: "Salary in USD",
                    interval: 10,
                    suffix: "k",
                    prefix: "$"
                },
                data: [{
                    type: "rangeBar",
                    showInLegend: true,
                    yValueFormatString: "$#0.#K",
                    indexLabel: "{y[#index]}",
                    legendText: "Department wise Min and Max Salary",
                    toolTipContent: "<b>{label}</b>: {y[0]} to {y[1]}",
                    dataPoints: [
                        { x: 10, y: [80, 115], label: "Data Scientist" },
                        { x: 20, y: [95, 141], label: "Product Manager" },
                        { x: 30, y: [98, 115], label: "Web Developer" },
                        { x: 40, y: [90, 160], label: "Software Engineer" },
                        { x: 50, y: [100, 152], label: "Quality Assurance" }
                    ]
                }]
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#email_categories")) {
            scope.chart = new CanvasJS.Chart("email_categories", {
                animationEnabled: true,
                title: {
                    text: "Email Categories",
                    horizontalAlign: "left"
                },
                data: [{
                    type: "doughnut",
                    startAngle: 60,
                    //innerRadius: 60,
                    indexLabelFontSize: 17,
                    indexLabel: "{label} - #percent%",
                    toolTipContent: "<b>{label}:</b> {y} (#percent%)",
                    dataPoints: [
                        { y: 67, label: "Inbox" },
                        { y: 28, label: "Archives" },
                        { y: 10, label: "Labels" },
                        { y: 7, label: "Drafts" },
                        { y: 15, label: "Trash" },
                        { y: 6, label: "Spam" }
                    ]
                }]
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#stacked_bar_chart")) {
            scope.chart = new CanvasJS.Chart("stacked_bar_chart", {
                animationEnabled: true,
                title: {
                    text: "Evening Sales in a Restaurant"
                },
                axisX: {
                    valueFormatString: "DDD"
                },
                axisY: {
                    prefix: "$"
                },
                toolTip: {
                    shared: true
                },
                legend: {
                    cursor: "pointer",
                    itemclick: toggleDataSeries
                },
                data: [{
                    type: "stackedBar",
                    name: "Meals",
                    showInLegend: "true",
                    xValueFormatString: "DD, MMM",
                    yValueFormatString: "$#,##0",
                    dataPoints: [
                        { x: new Date(2017, 0, 30), y: 56 },
                        { x: new Date(2017, 0, 31), y: 45 },
                        { x: new Date(2017, 1, 1), y: 71 },
                        { x: new Date(2017, 1, 2), y: 41 },
                        { x: new Date(2017, 1, 3), y: 60 },
                        { x: new Date(2017, 1, 4), y: 75 },
                        { x: new Date(2017, 1, 5), y: 98 }
                    ]
                },
                {
                    type: "stackedBar",
                    name: "Snacks",
                    showInLegend: "true",
                    xValueFormatString: "DD, MMM",
                    yValueFormatString: "$#,##0",
                    dataPoints: [
                        { x: new Date(2017, 0, 30), y: 86 },
                        { x: new Date(2017, 0, 31), y: 95 },
                        { x: new Date(2017, 1, 1), y: 71 },
                        { x: new Date(2017, 1, 2), y: 58 },
                        { x: new Date(2017, 1, 3), y: 60 },
                        { x: new Date(2017, 1, 4), y: 65 },
                        { x: new Date(2017, 1, 5), y: 89 }
                    ]
                },
                {
                    type: "stackedBar",
                    name: "Drinks",
                    showInLegend: "true",
                    xValueFormatString: "DD, MMM",
                    yValueFormatString: "$#,##0",
                    dataPoints: [
                        { x: new Date(2017, 0, 30), y: 48 },
                        { x: new Date(2017, 0, 31), y: 45 },
                        { x: new Date(2017, 1, 1), y: 41 },
                        { x: new Date(2017, 1, 2), y: 55 },
                        { x: new Date(2017, 1, 3), y: 80 },
                        { x: new Date(2017, 1, 4), y: 85 },
                        { x: new Date(2017, 1, 5), y: 83 }
                    ]
                },
                {
                    type: "stackedBar",
                    name: "Dessert",
                    showInLegend: "true",
                    xValueFormatString: "DD, MMM",
                    yValueFormatString: "$#,##0",
                    dataPoints: [
                        { x: new Date(2017, 0, 30), y: 61 },
                        { x: new Date(2017, 0, 31), y: 55 },
                        { x: new Date(2017, 1, 1), y: 61 },
                        { x: new Date(2017, 1, 2), y: 75 },
                        { x: new Date(2017, 1, 3), y: 80 },
                        { x: new Date(2017, 1, 4), y: 85 },
                        { x: new Date(2017, 1, 5), y: 105 }
                    ]
                },
                {
                    type: "stackedBar",
                    name: "Takeaway",
                    showInLegend: "true",
                    xValueFormatString: "DD, MMM",
                    yValueFormatString: "$#,##0",
                    dataPoints: [
                        { x: new Date(2017, 0, 30), y: 52 },
                        { x: new Date(2017, 0, 31), y: 55 },
                        { x: new Date(2017, 1, 1), y: 20 },
                        { x: new Date(2017, 1, 2), y: 35 },
                        { x: new Date(2017, 1, 3), y: 30 },
                        { x: new Date(2017, 1, 4), y: 45 },
                        { x: new Date(2017, 1, 5), y: 25 }
                    ]
                }]
            });
            function toggleDataSeries(e) {
                if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                    e.dataSeries.visible = false;
                }
                else {
                    e.dataSeries.visible = true;
                }
                scope.chart.render();
            }
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#funnel_chart")) {
            scope.chart = new CanvasJS.Chart("funnel_chart", {
                animationEnabled: true,
                theme: "light2", //"light1", "dark1", "dark2"
                title: {
                    text: "Sales Analysis - June 2016"
                },
                data: [{
                    type: "funnel",
                    indexLabelPlacement: "inside",
                    indexLabelFontColor: "white",
                    toolTipContent: "<b>{label}</b>: {y} <b>({percentage}%)</b>",
                    indexLabel: "{label} ({percentage}%)",
                    dataPoints: [
                        { y: 1400, label: "Leads" },
                        { y: 1212, label: "Initial Communication" },
                        { y: 1080, label: "Customer Evaluation" },
                        { y: 665, label: "Negotiation" },
                        { y: 578, label: "Order Received" },
                        { y: 549, label: "Payment" }
                    ]
                }]
            });
            calculatePercentage();
            scope.chart.render();

            function calculatePercentage() {
                var dataPoint = scope.chart.options.data[0].dataPoints;
                var total = dataPoint[0].y;
                for (var i = 0; i < dataPoint.length; i++) {
                    if (i == 0) {
                        scope.chart.options.data[0].dataPoints[i].percentage = 100;
                    } else {
                        scope.chart.options.data[0].dataPoints[i].percentage = ((dataPoint[i].y / total) * 100).toFixed(2);
                    }
                }
            }
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#candlestick_chart_with_line_chart")) {

            scope.chart = new CanvasJS.Chart("candlestick_chart_with_line_chart", {
                animationEnabled: true,
                theme: "light2", // "light1", "light2", "dark1", "dark2"
                exportEnabled: true,
                title: {
                    text: "Facebook Stock Price - 2016"
                },
                subtitles: [{
                    text: "All Prices are in USD"
                }],
                axisX: {
                    valueFormatString: "MMM"
                },
                axisY: {
                    includeZero: false,
                    prefix: "$",
                    title: "Price"
                },
                axisY2: {
                    prefix: "$",
                    suffix: "bn",
                    title: "Revenue & Income",
                    tickLength: 0
                },
                toolTip: {
                    shared: true
                },
                legend: {
                    reversed: true,
                    cursor: "pointer",
                    itemclick: toggleDataSeries
                },
                data: [{
                    type: "candlestick",
                    showInLegend: true,
                    name: "Stock Price",
                    yValueFormatString: "$#,##0.00",
                    xValueFormatString: "MMMM",
                    dataPoints: [   // Y: [Open, High ,Low, Close]
                        { x: new Date(2016, 0), y: [101.949997, 112.839996, 89.370003, 112.209999] },
                        { x: new Date(2016, 1), y: [112.269997, 117.589996, 96.820000, 106.919998] },
                        { x: new Date(2016, 2), y: [107.830002, 116.989998, 104.400002, 114.099998] },
                        { x: new Date(2016, 3), y: [113.750000, 120.790001, 106.309998, 117.580002] },
                        { x: new Date(2016, 4), y: [117.830002, 121.080002, 115.879997, 118.809998] },
                        { x: new Date(2016, 5), y: [118.500000, 119.440002, 108.230003, 114.279999] },
                        { x: new Date(2016, 6), y: [114.199997, 128.330002, 112.970001, 123.940002] },
                        { x: new Date(2016, 7), y: [123.849998, 126.730003, 122.070000, 126.120003] },
                        { x: new Date(2016, 8), y: [126.379997, 131.979996, 125.599998, 128.270004] },
                        { x: new Date(2016, 9), y: [128.380005, 133.500000, 126.750000, 130.990005] },
                        { x: new Date(2016, 10), y: [131.410004, 131.940002, 113.550003, 118.419998] },
                        { x: new Date(2016, 11), y: [118.379997, 122.500000, 114.000000, 115.050003] }
                    ]
                },
                {
                    type: "line",
                    showInLegend: true,
                    name: "Net Income",
                    axisYType: "secondary",
                    yValueFormatString: "$#,##0.00bn",
                    xValueFormatString: "MMMM",
                    dataPoints: [
                        { x: new Date(2016, 2), y: 1.510 },
                        { x: new Date(2016, 5), y: 2.055 },
                        { x: new Date(2016, 8), y: 2.379 },
                        { x: new Date(2016, 11), y: 3.568 }
                    ]
                },
                {
                    type: "line",
                    showInLegend: true,
                    name: "Total Revenue",
                    axisYType: "secondary",
                    yValueFormatString: "$#,##0.00bn",
                    xValueFormatString: "MMMM",
                    dataPoints: [
                        { x: new Date(2016, 2), y: 5.382 },
                        { x: new Date(2016, 5), y: 6.436 },
                        { x: new Date(2016, 8), y: 7.011 },
                        { x: new Date(2016, 11), y: 8.809 }
                    ]
                }]
            });
            scope.chart.render();

            function toggleDataSeries(e) {
                if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                    e.dataSeries.visible = false;
                } else {
                    e.dataSeries.visible = true;
                }
                e.chart.render();
            }

            scope.chart.render();

            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#scatter_point_chart")) {
            scope.chart = new CanvasJS.Chart("scatter_point_chart", {
                animationEnabled: true,
                zoomEnabled: true,
                title: {
                    text: "Real Estate Rates"
                },
                axisX: {
                    title: "Area (in sq. ft)",
                    minimum: 790,
                    maximum: 2260
                },
                axisY: {
                    title: "Price (in USD)",
                    valueFormatString: "$#,##0k"
                },
                data: [{
                    type: "scatter",
                    toolTipContent: "<b>Area: </b>{x} sq.ft<br/><b>Price: </b>${y}k",
                    dataPoints: [
                        { x: 800, y: 350 },
                        { x: 900, y: 450 },
                        { x: 850, y: 450 },
                        { x: 1250, y: 700 },
                        { x: 1100, y: 650 },
                        { x: 1350, y: 850 },
                        { x: 1200, y: 900 },
                        { x: 1410, y: 1250 },
                        { x: 1250, y: 1100 },
                        { x: 1400, y: 1150 },
                        { x: 1500, y: 1050 },
                        { x: 1330, y: 1120 },
                        { x: 1580, y: 1220 },
                        { x: 1620, y: 1400 },
                        { x: 1250, y: 1450 },
                        { x: 1350, y: 1600 },
                        { x: 1650, y: 1300 },
                        { x: 1700, y: 1620 },
                        { x: 1750, y: 1700 },
                        { x: 1830, y: 1800 },
                        { x: 1900, y: 2000 },
                        { x: 2050, y: 2200 },
                        { x: 2150, y: 1960 },
                        { x: 2250, y: 1990 }
                    ]
                }]
            });
            scope.chart.render();

            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#box_whisker_chart")) {
            scope.chart = new CanvasJS.Chart("box_whisker_chart", {
                animationEnabled: true,
                title: {
                    text: "Daily Sleep Statistics of Age Group 12 - 20"
                },
                axisX: {
                    valueFormatString: "DDD"
                },
                axisY: {
                    title: "Sleep Time (in Hours)"
                },
                data: [{
                    type: "boxAndWhisker",
                    xValueFormatString: "DDDD",
                    yValueFormatString: "#0.0 Hours",
                    dataPoints: [
                        { x: new Date(2017, 6, 3), y: [4, 6, 8, 9, 7] },
                        { x: new Date(2017, 6, 4), y: [5, 6, 7, 8, 6.5] },
                        { x: new Date(2017, 6, 5), y: [4, 5, 7, 8, 6.5] },
                        { x: new Date(2017, 6, 6), y: [3, 5, 6, 9, 5.5] },
                        { x: new Date(2017, 6, 7), y: [6, 8, 10, 11, 8.5] },
                        { x: new Date(2017, 6, 8), y: [5, 7, 9, 12, 7.5] },
                        { x: new Date(2017, 6, 9), y: [4, 6, 8, 9, 7] }
                    ]
                }]
            });
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#column_line_area_chart")) {
            scope.chart = new CanvasJS.Chart("column_line_area_chart", {
                animationEnabled: true,
                theme: "light2",
                title: {
                    text: "Monthly Sales Data"
                },
                axisX: {
                    valueFormatString: "MMM"
                },
                axisY: {
                    prefix: "$",
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
                        name: "Actual Sales",
                        showInLegend: true,
                        xValueFormatString: "MMMM YYYY",
                        yValueFormatString: "$#,##0",
                        dataPoints: [
                            { x: new Date(2016, 0), y: 20000 },
                            { x: new Date(2016, 1), y: 30000 },
                            { x: new Date(2016, 2), y: 25000 },
                            { x: new Date(2016, 3), y: 70000, indexLabel: "High Renewals" },
                            { x: new Date(2016, 4), y: 50000 },
                            { x: new Date(2016, 5), y: 35000 },
                            { x: new Date(2016, 6), y: 30000 },
                            { x: new Date(2016, 7), y: 43000 },
                            { x: new Date(2016, 8), y: 35000 },
                            { x: new Date(2016, 9), y: 30000 },
                            { x: new Date(2016, 10), y: 40000 },
                            { x: new Date(2016, 11), y: 50000 }
                        ]
                    },
                    {
                        type: "line",
                        name: "Expected Sales",
                        showInLegend: true,
                        yValueFormatString: "$#,##0",
                        dataPoints: [
                            { x: new Date(2016, 0), y: 40000 },
                            { x: new Date(2016, 1), y: 42000 },
                            { x: new Date(2016, 2), y: 45000 },
                            { x: new Date(2016, 3), y: 45000 },
                            { x: new Date(2016, 4), y: 47000 },
                            { x: new Date(2016, 5), y: 43000 },
                            { x: new Date(2016, 6), y: 42000 },
                            { x: new Date(2016, 7), y: 43000 },
                            { x: new Date(2016, 8), y: 41000 },
                            { x: new Date(2016, 9), y: 45000 },
                            { x: new Date(2016, 10), y: 42000 },
                            { x: new Date(2016, 11), y: 50000 }
                        ]
                    },
                    {
                        type: "area",
                        name: "Profit",
                        markerBorderColor: "white",
                        markerBorderThickness: 2,
                        showInLegend: true,
                        yValueFormatString: "$#,##0",
                        dataPoints: [
                            { x: new Date(2016, 0), y: 5000 },
                            { x: new Date(2016, 1), y: 7000 },
                            { x: new Date(2016, 2), y: 6000 },
                            { x: new Date(2016, 3), y: 30000 },
                            { x: new Date(2016, 4), y: 20000 },
                            { x: new Date(2016, 5), y: 15000 },
                            { x: new Date(2016, 6), y: 13000 },
                            { x: new Date(2016, 7), y: 20000 },
                            { x: new Date(2016, 8), y: 15000 },
                            { x: new Date(2016, 9), y: 10000 },
                            { x: new Date(2016, 10), y: 19000 },
                            { x: new Date(2016, 11), y: 22000 }
                        ]
                    }]
            });

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
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }

        if ($(element).is("#live_column_chart")) {
            scope.chart = new CanvasJS.Chart("live_column_chart", {
                title: {
                    text: "Temperature of Each Boiler"
                },
                axisY: {
                    title: "Temperature (°C)",
                    suffix: " °C"
                },
                data: [{
                    type: "column",
                    yValueFormatString: "#,### °C",
                    indexLabel: "{y}",
                    dataPoints: [
                        { label: "boiler1", y: 206 },
                        { label: "boiler2", y: 163 },
                        { label: "boiler3", y: 154 },
                        { label: "boiler4", y: 176 },
                        { label: "boiler5", y: 184 },
                        { label: "boiler6", y: 122 }
                    ]
                }]
            });

            function updateChart() {
                var boilerColor, deltaY, yVal;
                var dps = scope.chart.options.data[0].dataPoints;
                for (var i = 0; i < dps.length; i++) {
                    deltaY = Math.round(2 + Math.random() * (-2 - 2));
                    yVal = deltaY + dps[i].y > 0 ? dps[i].y + deltaY : 0;
                    boilerColor = yVal > 200 ? "#FF2500" : yVal >= 170 ? "#FF6000" : yVal < 170 ? "#6B8E23 " : null;
                    dps[i] = { label: "Boiler " + (i + 1), y: yVal, color: boilerColor };
                }
                scope.chart.options.data[0].dataPoints = dps;
                scope.chart.render();
            };
            updateChart();

            setInterval(function () { updateChart() }, 500);
            scope.chart.render();
            $rootScope.charts.push(scope.chart);
        }
    }
    return {
        restrict: "A",
        link: link
    };
});



/* ===== AM Charts Directive ==== */
//airviewXcelerateApp.directive('amcharts', function ($rootScope) {
//    function link(scope, element, attrs) {
//        if ($(element).is("#column_chart_with_rotated_series")) {
//            var chart = AmCharts.makeChart("column_chart_with_rotated_series", {
//                "type": "serial",
//                "theme": "light",
//                "marginRight": 70,
//                "dataProvider": [{
//                    "country": "USA",
//                    "visits": 3025,
//                    "color": "#FF0F00"
//                }, {
//                    "country": "China",
//                    "visits": 1882,
//                    "color": "#FF6600"
//                }, {
//                    "country": "Japan",
//                    "visits": 1809,
//                    "color": "#FF9E01"
//                }, {
//                    "country": "Germany",
//                    "visits": 1322,
//                    "color": "#FCD202"
//                }, {
//                    "country": "UK",
//                    "visits": 1122,
//                    "color": "#F8FF01"
//                }, {
//                    "country": "France",
//                    "visits": 1114,
//                    "color": "#B0DE09"
//                }, {
//                    "country": "India",
//                    "visits": 984,
//                    "color": "#04D215"
//                }, {
//                    "country": "Spain",
//                    "visits": 711,
//                    "color": "#0D8ECF"
//                }, {
//                    "country": "Netherlands",
//                    "visits": 665,
//                    "color": "#0D52D1"
//                }, {
//                    "country": "Russia",
//                    "visits": 580,
//                    "color": "#2A0CD0"
//                }, {
//                    "country": "South Korea",
//                    "visits": 443,
//                    "color": "#8A0CCF"
//                }, {
//                    "country": "Canada",
//                    "visits": 441,
//                    "color": "#CD0D74"
//                }],
//                "valueAxes": [{
//                    "axisAlpha": 0,
//                    "position": "left",
//                    "title": "Visitors from country"
//                }],
//                "startDuration": 1,
//                "graphs": [{
//                    "balloonText": "<b>[[category]]: [[value]]</b>",
//                    "fillColorsField": "color",
//                    "fillAlphas": 0.9,
//                    "lineAlpha": 0.2,
//                    "type": "column",
//                    "valueField": "visits"
//                }],
//                "chartCursor": {
//                    "categoryBalloonEnabled": false,
//                    "cursorAlpha": 0,
//                    "zoomable": false
//                },
//                "categoryField": "country",
//                "categoryAxis": {
//                    "gridPosition": "start",
//                    "labelRotation": 45
//                },
//                "export": {
//                    "enabled": true
//                }

//            });
//        }

//        if ($(element).is("#stacked_column_chart")) {
//            var chart = AmCharts.makeChart("stacked_column_chart", {
//                "type": "serial",
//                "theme": "light",
//                "legend": {
//                    "horizontalGap": 10,
//                    "maxColumns": 1,
//                    "position": "right",
//                    "useGraphSettings": true,
//                    "markerSize": 10
//                },
//                "dataProvider": [{
//                    "year": 2003,
//                    "europe": 2.5,
//                    "namerica": 2.5,
//                    "asia": 2.1,
//                    "lamerica": 0.3,
//                    "meast": 0.2,
//                    "africa": 0.1
//                }, {
//                    "year": 2004,
//                    "europe": 2.6,
//                    "namerica": 2.7,
//                    "asia": 2.2,
//                    "lamerica": 0.3,
//                    "meast": 0.3,
//                    "africa": 0.1
//                }, {
//                    "year": 2005,
//                    "europe": 2.8,
//                    "namerica": 2.9,
//                    "asia": 2.4,
//                    "lamerica": 0.3,
//                    "meast": 0.3,
//                    "africa": 0.1
//                }],
//                "valueAxes": [{
//                    "stackType": "regular",
//                    "axisAlpha": 0.3,
//                    "gridAlpha": 0
//                }],
//                "graphs": [{
//                    "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>",
//                    "fillAlphas": 0.8,
//                    "labelText": "[[value]]",
//                    "lineAlpha": 0.3,
//                    "title": "Europe",
//                    "type": "column",
//                    "color": "#000000",
//                    "valueField": "europe"
//                }, {
//                    "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>",
//                    "fillAlphas": 0.8,
//                    "labelText": "[[value]]",
//                    "lineAlpha": 0.3,
//                    "title": "North America",
//                    "type": "column",
//                    "color": "#000000",
//                    "valueField": "namerica"
//                }, {
//                    "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>",
//                    "fillAlphas": 0.8,
//                    "labelText": "[[value]]",
//                    "lineAlpha": 0.3,
//                    "title": "Asia-Pacific",
//                    "type": "column",
//                    "color": "#000000",
//                    "valueField": "asia"
//                }, {
//                    "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>",
//                    "fillAlphas": 0.8,
//                    "labelText": "[[value]]",
//                    "lineAlpha": 0.3,
//                    "title": "Latin America",
//                    "type": "column",
//                    "color": "#000000",
//                    "valueField": "lamerica"
//                }, {
//                    "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>",
//                    "fillAlphas": 0.8,
//                    "labelText": "[[value]]",
//                    "lineAlpha": 0.3,
//                    "title": "Middle-East",
//                    "type": "column",
//                    "color": "#000000",
//                    "valueField": "meast"
//                }, {
//                    "balloonText": "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>",
//                    "fillAlphas": 0.8,
//                    "labelText": "[[value]]",
//                    "lineAlpha": 0.3,
//                    "title": "Africa",
//                    "type": "column",
//                    "color": "#000000",
//                    "valueField": "africa"
//                }],
//                "categoryField": "year",
//                "categoryAxis": {
//                    "gridPosition": "start",
//                    "axisAlpha": 0,
//                    "gridAlpha": 0,
//                    "position": "left"
//                },
//                "export": {
//                    "enabled": true
//                }

//            });
//        }

//        if ($(element).is("#column_line_mix")) {
//            var chart = AmCharts.makeChart("column_line_mix", {
//                "type": "serial",
//                "addClassNames": true,
//                "theme": "light",
//                "autoMargins": false,
//                "marginLeft": 30,
//                "marginRight": 8,
//                "marginTop": 10,
//                "marginBottom": 26,
//                "balloon": {
//                    "adjustBorderColor": false,
//                    "horizontalPadding": 10,
//                    "verticalPadding": 8,
//                    "color": "#ffffff"
//                },

//                "dataProvider": [{
//                    "year": 2009,
//                    "income": 23.5,
//                    "expenses": 21.1
//                }, {
//                    "year": 2010,
//                    "income": 26.2,
//                    "expenses": 30.5
//                }, {
//                    "year": 2011,
//                    "income": 30.1,
//                    "expenses": 34.9
//                }, {
//                    "year": 2012,
//                    "income": 29.5,
//                    "expenses": 31.1
//                }, {
//                    "year": 2013,
//                    "income": 30.6,
//                    "expenses": 28.2,
//                    "dashLengthLine": 5
//                }, {
//                    "year": 2014,
//                    "income": 34.1,
//                    "expenses": 32.9,
//                    "dashLengthColumn": 5,
//                    "alpha": 0.2,
//                    "additional": "(projection)"
//                }],
//                "valueAxes": [{
//                    "axisAlpha": 0,
//                    "position": "left"
//                }],
//                "startDuration": 1,
//                "graphs": [{
//                    "alphaField": "alpha",
//                    "balloonText": "<span style='font-size:12px;'>[[title]] in [[category]]:<br><span style='font-size:20px;'>[[value]]</span> [[additional]]</span>",
//                    "fillAlphas": 1,
//                    "title": "Income",
//                    "type": "column",
//                    "valueField": "income",
//                    "dashLengthField": "dashLengthColumn"
//                }, {
//                    "id": "graph2",
//                    "balloonText": "<span style='font-size:12px;'>[[title]] in [[category]]:<br><span style='font-size:20px;'>[[value]]</span> [[additional]]</span>",
//                    "bullet": "round",
//                    "lineThickness": 3,
//                    "bulletSize": 7,
//                    "bulletBorderAlpha": 1,
//                    "bulletColor": "#FFFFFF",
//                    "useLineColorForBulletBorder": true,
//                    "bulletBorderThickness": 3,
//                    "fillAlphas": 0,
//                    "lineAlpha": 1,
//                    "title": "Expenses",
//                    "valueField": "expenses",
//                    "dashLengthField": "dashLengthLine"
//                }],
//                "categoryField": "year",
//                "categoryAxis": {
//                    "gridPosition": "start",
//                    "axisAlpha": 0,
//                    "tickLength": 0
//                },
//                "export": {
//                    "enabled": true
//                }
//            });
//        }

//        if ($(element).is("#gantt_charts_with_dates")) {
//            var chart = AmCharts.makeChart("gantt_charts_with_dates", {
//                "type": "gantt",
//                "theme": "light",
//                "marginRight": 70,
//                "period": "DD",
//                "dataDateFormat": "YYYY-MM-DD",
//                "columnWidth": 0.5,
//                "valueAxis": {
//                    "type": "date"
//                },
//                "brightnessStep": 7,
//                "graph": {
//                    "fillAlphas": 1,
//                    "lineAlpha": 1,
//                    "lineColor": "#fff",
//                    "fillAlphas": 0.85,
//                    "balloonText": "<b>[[task]]</b>:<br />[[open]] -- [[value]]"
//                },
//                "rotate": true,
//                "categoryField": "category",
//                "segmentsField": "segments",
//                "colorField": "color",
//                "startDateField": "start",
//                "endDateField": "end",
//                "dataProvider": [{
//                    "category": "Module #1",
//                    "segments": [{
//                        "start": "2016-01-01",
//                        "end": "2016-01-14",
//                        "color": "#b9783f",
//                        "task": "Gathering requirements"
//                    }, {
//                        "start": "2016-01-16",
//                        "end": "2016-01-27",
//                        "task": "Producing specifications"
//                    }, {
//                        "start": "2016-02-05",
//                        "end": "2016-04-18",
//                        "task": "Development"
//                    }, {
//                        "start": "2016-04-18",
//                        "end": "2016-04-30",
//                        "task": "Testing and QA"
//                    }]
//                }, {
//                    "category": "Module #2",
//                    "segments": [{
//                        "start": "2016-01-08",
//                        "end": "2016-01-10",
//                        "color": "#cc4748",
//                        "task": "Gathering requirements"
//                    }, {
//                        "start": "2016-01-12",
//                        "end": "2016-01-15",
//                        "task": "Producing specifications"
//                    }, {
//                        "start": "2016-01-16",
//                        "end": "2016-02-05",
//                        "task": "Development"
//                    }, {
//                        "start": "2016-02-10",
//                        "end": "2016-02-18",
//                        "task": "Testing and QA"
//                    }]
//                }, {
//                    "category": "Module #3",
//                    "segments": [{
//                        "start": "2016-01-02",
//                        "end": "2016-01-08",
//                        "color": "#cd82ad",
//                        "task": "Gathering requirements"
//                    }, {
//                        "start": "2016-01-08",
//                        "end": "2016-01-16",
//                        "task": "Producing specifications"
//                    }, {
//                        "start": "2016-01-19",
//                        "end": "2016-03-01",
//                        "task": "Development"
//                    }, {
//                        "start": "2016-03-12",
//                        "end": "2016-04-05",
//                        "task": "Testing and QA"
//                    }]
//                }, {
//                    "category": "Module #4",
//                    "segments": [{
//                        "start": "2016-01-01",
//                        "end": "2016-01-19",
//                        "color": "#2f4074",
//                        "task": "Gathering requirements"
//                    }, {
//                        "start": "2016-01-19",
//                        "end": "2016-02-03",
//                        "task": "Producing specifications"
//                    }, {
//                        "start": "2016-03-20",
//                        "end": "2016-04-25",
//                        "task": "Development"
//                    }, {
//                        "start": "2016-04-27",
//                        "end": "2016-05-15",
//                        "task": "Testing and QA"
//                    }]
//                }, {
//                    "category": "Module #5",
//                    "segments": [{
//                        "start": "2016-01-01",
//                        "end": "2016-01-12",
//                        "color": "#448e4d",
//                        "task": "Gathering requirements"
//                    }, {
//                        "start": "2016-01-12",
//                        "end": "2016-01-19",
//                        "task": "Producing specifications"
//                    }, {
//                        "start": "2016-01-19",
//                        "end": "2016-03-01",
//                        "task": "Development"
//                    }, {
//                        "start": "2016-03-08",
//                        "end": "2016-03-30",
//                        "task": "Testing and QA"
//                    }]
//                }],
//                "valueScrollbar": {
//                    "autoGridCount": true
//                },
//                "chartCursor": {
//                    "cursorColor": "#55bb76",
//                    "valueBalloonsEnabled": false,
//                    "cursorAlpha": 0,
//                    "valueLineAlpha": 0.5,
//                    "valueLineBalloonEnabled": true,
//                    "valueLineEnabled": true,
//                    "zoomable": false,
//                    "valueZoomable": true
//                },
//                "export": {
//                    "enabled": true
//                }
//            });
//        }

//        if ($(element).is("#clustered_bar_chart")) {
//            var chart = AmCharts.makeChart("clustered_bar_chart", {
//                "type": "serial",
//                "theme": "light",
//                "categoryField": "year",
//                "rotate": true,
//                "startDuration": 1,
//                "categoryAxis": {
//                    "gridPosition": "start",
//                    "position": "left"
//                },
//                "trendLines": [],
//                "graphs": [
//                    {
//                        "balloonText": "Income:[[value]]",
//                        "fillAlphas": 0.8,
//                        "id": "AmGraph-1",
//                        "lineAlpha": 0.2,
//                        "title": "Income",
//                        "type": "column",
//                        "valueField": "income"
//                    },
//                    {
//                        "balloonText": "Expenses:[[value]]",
//                        "fillAlphas": 0.8,
//                        "id": "AmGraph-2",
//                        "lineAlpha": 0.2,
//                        "title": "Expenses",
//                        "type": "column",
//                        "valueField": "expenses"
//                    }
//                ],
//                "guides": [],
//                "valueAxes": [
//                    {
//                        "id": "ValueAxis-1",
//                        "position": "top",
//                        "axisAlpha": 0
//                    }
//                ],
//                "allLabels": [],
//                "balloon": {},
//                "titles": [],
//                "dataProvider": [
//                    {
//                        "year": 2005,
//                        "income": 23.5,
//                        "expenses": 18.1
//                    },
//                    {
//                        "year": 2006,
//                        "income": 26.2,
//                        "expenses": 22.8
//                    },
//                    {
//                        "year": 2007,
//                        "income": 30.1,
//                        "expenses": 23.9
//                    },
//                    {
//                        "year": 2008,
//                        "income": 29.5,
//                        "expenses": 25.1
//                    },
//                    {
//                        "year": 2009,
//                        "income": 24.6,
//                        "expenses": 25
//                    }
//                ],
//                "export": {
//                    "enabled": true
//                }

//            });
//        }

//        if ($(element).is("#3d_column_chart")) {
//            var chart = AmCharts.makeChart("3d_column_chart", {
//                "theme": "light",
//                "type": "serial",
//                "startDuration": 2,
//                "dataProvider": [{
//                    "country": "USA",
//                    "visits": 4025,
//                    "color": "#FF0F00"
//                }, {
//                    "country": "China",
//                    "visits": 1882,
//                    "color": "#FF6600"
//                }, {
//                    "country": "Japan",
//                    "visits": 1809,
//                    "color": "#FF9E01"
//                }, {
//                    "country": "Germany",
//                    "visits": 1322,
//                    "color": "#FCD202"
//                }, {
//                    "country": "UK",
//                    "visits": 1122,
//                    "color": "#F8FF01"
//                }, {
//                    "country": "France",
//                    "visits": 1114,
//                    "color": "#B0DE09"
//                }, {
//                    "country": "India",
//                    "visits": 984,
//                    "color": "#04D215"
//                }, {
//                    "country": "Spain",
//                    "visits": 711,
//                    "color": "#0D8ECF"
//                }, {
//                    "country": "Netherlands",
//                    "visits": 665,
//                    "color": "#0D52D1"
//                }, {
//                    "country": "Russia",
//                    "visits": 580,
//                    "color": "#2A0CD0"
//                }, {
//                    "country": "South Korea",
//                    "visits": 443,
//                    "color": "#8A0CCF"
//                }, {
//                    "country": "Canada",
//                    "visits": 441,
//                    "color": "#CD0D74"
//                }, {
//                    "country": "Brazil",
//                    "visits": 395,
//                    "color": "#754DEB"
//                }, {
//                    "country": "Italy",
//                    "visits": 386,
//                    "color": "#DDDDDD"
//                }, {
//                    "country": "Australia",
//                    "visits": 384,
//                    "color": "#999999"
//                }, {
//                    "country": "Taiwan",
//                    "visits": 338,
//                    "color": "#333333"
//                }, {
//                    "country": "Poland",
//                    "visits": 328,
//                    "color": "#000000"
//                }],
//                "valueAxes": [{
//                    "position": "left",
//                    "title": "Visitors"
//                }],
//                "graphs": [{
//                    "balloonText": "[[category]]: <b>[[value]]</b>",
//                    "fillColorsField": "color",
//                    "fillAlphas": 1,
//                    "lineAlpha": 0.1,
//                    "type": "column",
//                    "valueField": "visits"
//                }],
//                "depth3D": 20,
//                "angle": 30,
//                "chartCursor": {
//                    "categoryBalloonEnabled": false,
//                    "cursorAlpha": 0,
//                    "zoomable": false
//                },
//                "categoryField": "country",
//                "categoryAxis": {
//                    "gridPosition": "start",
//                    "labelRotation": 90
//                },
//                "export": {
//                    "enabled": true
//                }

//            });
//        }

//        if ($(element).is("#stacked_bar_chart_with_negative_values")) {
//            var chart = AmCharts.makeChart("stacked_bar_chart_with_negative_values", {
//                "type": "serial",
//                "theme": "light",
//                "rotate": true,
//                "marginBottom": 50,
//                "dataProvider": [{
//                    "age": "85+",
//                    "male": -0.1,
//                    "female": 0.3
//                }, {
//                    "age": "80-54",
//                    "male": -0.2,
//                    "female": 0.3
//                }, {
//                    "age": "75-79",
//                    "male": -0.3,
//                    "female": 0.6
//                }, {
//                    "age": "70-74",
//                    "male": -0.5,
//                    "female": 0.8
//                }, {
//                    "age": "65-69",
//                    "male": -0.8,
//                    "female": 1.0
//                }, {
//                    "age": "60-64",
//                    "male": -1.1,
//                    "female": 1.3
//                }, {
//                    "age": "55-59",
//                    "male": -1.7,
//                    "female": 1.9
//                }, {
//                    "age": "50-54",
//                    "male": -2.2,
//                    "female": 2.5
//                }, {
//                    "age": "45-49",
//                    "male": -2.8,
//                    "female": 3.0
//                }, {
//                    "age": "40-44",
//                    "male": -3.4,
//                    "female": 3.6
//                }, {
//                    "age": "35-39",
//                    "male": -4.2,
//                    "female": 4.1
//                }, {
//                    "age": "30-34",
//                    "male": -5.2,
//                    "female": 4.8
//                }, {
//                    "age": "25-29",
//                    "male": -5.6,
//                    "female": 5.1
//                }, {
//                    "age": "20-24",
//                    "male": -5.1,
//                    "female": 5.1
//                }, {
//                    "age": "15-19",
//                    "male": -3.8,
//                    "female": 3.8
//                }, {
//                    "age": "10-14",
//                    "male": -3.2,
//                    "female": 3.4
//                }, {
//                    "age": "5-9",
//                    "male": -4.4,
//                    "female": 4.1
//                }, {
//                    "age": "0-4",
//                    "male": -5.0,
//                    "female": 4.8
//                }],
//                "startDuration": 1,
//                "graphs": [{
//                    "fillAlphas": 0.8,
//                    "lineAlpha": 0.2,
//                    "type": "column",
//                    "valueField": "male",
//                    "title": "Male",
//                    "labelText": "[[value]]",
//                    "clustered": false,
//                    "labelFunction": function (item) {
//                        return Math.abs(item.values.value);
//                    },
//                    "balloonFunction": function (item) {
//                        return item.category + ": " + Math.abs(item.values.value) + "%";
//                    }
//                }, {
//                    "fillAlphas": 0.8,
//                    "lineAlpha": 0.2,
//                    "type": "column",
//                    "valueField": "female",
//                    "title": "Female",
//                    "labelText": "[[value]]",
//                    "clustered": false,
//                    "labelFunction": function (item) {
//                        return Math.abs(item.values.value);
//                    },
//                    "balloonFunction": function (item) {
//                        return item.category + ": " + Math.abs(item.values.value) + "%";
//                    }
//                }],
//                "categoryField": "age",
//                "categoryAxis": {
//                    "gridPosition": "start",
//                    "gridAlpha": 0.2,
//                    "axisAlpha": 0
//                },
//                "valueAxes": [{
//                    "gridAlpha": 0,
//                    "ignoreAxisWidth": true,
//                    "labelFunction": function (value) {
//                        return Math.abs(value) + '%';
//                    },
//                    "guides": [{
//                        "value": 0,
//                        "lineAlpha": 0.2
//                    }]
//                }],
//                "balloon": {
//                    "fixedPosition": true
//                },
//                "chartCursor": {
//                    "valueBalloonsEnabled": false,
//                    "cursorAlpha": 0.05,
//                    "fullWidth": true
//                },
//                "allLabels": [{
//                    "text": "Male",
//                    "x": "28%",
//                    "y": "97%",
//                    "bold": true,
//                    "align": "middle"
//                }, {
//                    "text": "Female",
//                    "x": "75%",
//                    "y": "97%",
//                    "bold": true,
//                    "align": "middle"
//                }],
//                "export": {
//                    "enabled": true
//                }

//            });
//        }

//        if ($(element).is("#3d_stacked_column_chart")) {
//            var chart = AmCharts.makeChart("3d_stacked_column_chart", {
//                "theme": "light",
//                "type": "serial",
//                "dataProvider": [{
//                    "country": "USA",
//                    "year2004": 3.5,
//                    "year2005": 4.2
//                }, {
//                    "country": "UK",
//                    "year2004": 1.7,
//                    "year2005": 3.1
//                }, {
//                    "country": "Canada",
//                    "year2004": 2.8,
//                    "year2005": 2.9
//                }, {
//                    "country": "Japan",
//                    "year2004": 2.6,
//                    "year2005": 2.3
//                }, {
//                    "country": "France",
//                    "year2004": 1.4,
//                    "year2005": 2.1
//                }, {
//                    "country": "Brazil",
//                    "year2004": 2.6,
//                    "year2005": 4.9
//                }, {
//                    "country": "Russia",
//                    "year2004": 6.4,
//                    "year2005": 7.2
//                }, {
//                    "country": "India",
//                    "year2004": 8,
//                    "year2005": 7.1
//                }, {
//                    "country": "China",
//                    "year2004": 9.9,
//                    "year2005": 10.1
//                }],
//                "valueAxes": [{
//                    "stackType": "3d",
//                    "unit": "%",
//                    "position": "left",
//                    "title": "GDP growth rate",
//                }],
//                "startDuration": 1,
//                "graphs": [{
//                    "balloonText": "GDP grow in [[category]] (2004): <b>[[value]]</b>",
//                    "fillAlphas": 0.9,
//                    "lineAlpha": 0.2,
//                    "title": "2004",
//                    "type": "column",
//                    "valueField": "year2004"
//                }, {
//                    "balloonText": "GDP grow in [[category]] (2005): <b>[[value]]</b>",
//                    "fillAlphas": 0.9,
//                    "lineAlpha": 0.2,
//                    "title": "2005",
//                    "type": "column",
//                    "valueField": "year2005"
//                }],
//                "plotAreaFillAlphas": 0.1,
//                "depth3D": 60,
//                "angle": 30,
//                "categoryField": "country",
//                "categoryAxis": {
//                    "gridPosition": "start"
//                },
//                "export": {
//                    "enabled": true
//                }
//            });
//        }

//        if ($(element).is("#bar_line_chart_mix")) {
//            var chart = AmCharts.makeChart("bar_line_chart_mix", {
//                "type": "serial",
//                "theme": "light",
//                "handDrawn": true,
//                "handDrawScatter": 3,
//                "legend": {
//                    "useGraphSettings": true,
//                    "markerSize": 12,
//                    "valueWidth": 0,
//                    "verticalGap": 0
//                },
//                "dataProvider": [{
//                    "year": 2005,
//                    "income": 23.5,
//                    "expenses": 18.1
//                }, {
//                    "year": 2006,
//                    "income": 26.2,
//                    "expenses": 22.8
//                }, {
//                    "year": 2007,
//                    "income": 30.1,
//                    "expenses": 23.9
//                }, {
//                    "year": 2008,
//                    "income": 29.5,
//                    "expenses": 25.1
//                }, {
//                    "year": 2009,
//                    "income": 24.6,
//                    "expenses": 25
//                }],
//                "valueAxes": [{
//                    "minorGridAlpha": 0.08,
//                    "minorGridEnabled": true,
//                    "position": "top",
//                    "axisAlpha": 0
//                }],
//                "startDuration": 1,
//                "graphs": [{
//                    "balloonText": "<span style='font-size:13px;'>[[title]] in [[category]]:<b>[[value]]</b></span>",
//                    "title": "Income",
//                    "type": "column",
//                    "fillAlphas": 0.8,

//                    "valueField": "income"
//                }, {
//                    "balloonText": "<span style='font-size:13px;'>[[title]] in [[category]]:<b>[[value]]</b></span>",
//                    "bullet": "round",
//                    "bulletBorderAlpha": 1,
//                    "bulletColor": "#FFFFFF",
//                    "useLineColorForBulletBorder": true,
//                    "fillAlphas": 0,
//                    "lineThickness": 2,
//                    "lineAlpha": 1,
//                    "bulletSize": 7,
//                    "title": "Expenses",
//                    "valueField": "expenses"
//                }],
//                "rotate": true,
//                "categoryField": "year",
//                "categoryAxis": {
//                    "gridPosition": "start"
//                },
//                "export": {
//                    "enabled": true
//                }

//            });
//        }

//        if ($(element).is("#duration_on_value_axis")) {
//            var chart = AmCharts.makeChart("duration_on_value_axis", {
//                "type": "serial",
//                "theme": "light",
//                "legend": {
//                    "equalWidths": false,
//                    "useGraphSettings": true,
//                    "valueAlign": "left",
//                    "valueWidth": 120
//                },
//                "dataProvider": [{
//                    "date": "2012-01-01",
//                    "distance": 227,
//                    "townName": "New York",
//                    "townName2": "New York",
//                    "townSize": 25,
//                    "latitude": 40.71,
//                    "duration": 408
//                }, {
//                    "date": "2012-01-02",
//                    "distance": 371,
//                    "townName": "Washington",
//                    "townSize": 14,
//                    "latitude": 38.89,
//                    "duration": 482
//                }, {
//                    "date": "2012-01-03",
//                    "distance": 433,
//                    "townName": "Wilmington",
//                    "townSize": 6,
//                    "latitude": 34.22,
//                    "duration": 562
//                }, {
//                    "date": "2012-01-04",
//                    "distance": 345,
//                    "townName": "Jacksonville",
//                    "townSize": 7,
//                    "latitude": 30.35,
//                    "duration": 379
//                }, {
//                    "date": "2012-01-05",
//                    "distance": 480,
//                    "townName": "Miami",
//                    "townName2": "Miami",
//                    "townSize": 10,
//                    "latitude": 25.83,
//                    "duration": 501
//                }, {
//                    "date": "2012-01-06",
//                    "distance": 386,
//                    "townName": "Tallahassee",
//                    "townSize": 7,
//                    "latitude": 30.46,
//                    "duration": 443
//                }, {
//                    "date": "2012-01-07",
//                    "distance": 348,
//                    "townName": "New Orleans",
//                    "townSize": 10,
//                    "latitude": 29.94,
//                    "duration": 405
//                }, {
//                    "date": "2012-01-08",
//                    "distance": 238,
//                    "townName": "Houston",
//                    "townName2": "Houston",
//                    "townSize": 16,
//                    "latitude": 29.76,
//                    "duration": 309
//                }, {
//                    "date": "2012-01-09",
//                    "distance": 218,
//                    "townName": "Dalas",
//                    "townSize": 17,
//                    "latitude": 32.8,
//                    "duration": 287
//                }, {
//                    "date": "2012-01-10",
//                    "distance": 349,
//                    "townName": "Oklahoma City",
//                    "townSize": 11,
//                    "latitude": 35.49,
//                    "duration": 485
//                }, {
//                    "date": "2012-01-11",
//                    "distance": 603,
//                    "townName": "Kansas City",
//                    "townSize": 10,
//                    "latitude": 39.1,
//                    "duration": 890
//                }, {
//                    "date": "2012-01-12",
//                    "distance": 534,
//                    "townName": "Denver",
//                    "townName2": "Denver",
//                    "townSize": 18,
//                    "latitude": 39.74,
//                    "duration": 810
//                }, {
//                    "date": "2012-01-13",
//                    "townName": "Salt Lake City",
//                    "townSize": 12,
//                    "distance": 425,
//                    "duration": 670,
//                    "latitude": 40.75,
//                    "dashLength": 8,
//                    "alpha": 0.4
//                }, {
//                    "date": "2012-01-14",
//                    "latitude": 36.1,
//                    "duration": 470,
//                    "townName": "Las Vegas",
//                    "townName2": "Las Vegas"
//                }, {
//                    "date": "2012-01-15"
//                }, {
//                    "date": "2012-01-16"
//                }, {
//                    "date": "2012-01-17"
//                }, {
//                    "date": "2012-01-18"
//                }, {
//                    "date": "2012-01-19"
//                }],
//                "valueAxes": [{
//                    "id": "distanceAxis",
//                    "axisAlpha": 0,
//                    "gridAlpha": 0,
//                    "position": "left",
//                    "title": "distance"
//                }, {
//                    "id": "latitudeAxis",
//                    "axisAlpha": 0,
//                    "gridAlpha": 0,
//                    "labelsEnabled": false,
//                    "position": "right"
//                }, {
//                    "id": "durationAxis",
//                    "duration": "mm",
//                    "durationUnits": {
//                        "hh": "h ",
//                        "mm": "min"
//                    },
//                    "axisAlpha": 0,
//                    "gridAlpha": 0,
//                    "inside": true,
//                    "position": "right",
//                    "title": "duration"
//                }],
//                "graphs": [{
//                    "alphaField": "alpha",
//                    "balloonText": "[[value]] miles",
//                    "dashLengthField": "dashLength",
//                    "fillAlphas": 0.7,
//                    "legendPeriodValueText": "total: [[value.sum]] mi",
//                    "legendValueText": "[[value]] mi",
//                    "title": "distance",
//                    "type": "column",
//                    "valueField": "distance",
//                    "valueAxis": "distanceAxis"
//                }, {
//                    "balloonText": "latitude:[[value]]",
//                    "bullet": "round",
//                    "bulletBorderAlpha": 1,
//                    "useLineColorForBulletBorder": true,
//                    "bulletColor": "#FFFFFF",
//                    "bulletSizeField": "townSize",
//                    "dashLengthField": "dashLength",
//                    "descriptionField": "townName",
//                    "labelPosition": "right",
//                    "labelText": "[[townName2]]",
//                    "legendValueText": "[[value]]/[[description]]",
//                    "title": "latitude/city",
//                    "fillAlphas": 0,
//                    "valueField": "latitude",
//                    "valueAxis": "latitudeAxis"
//                }, {
//                    "bullet": "square",
//                    "bulletBorderAlpha": 1,
//                    "bulletBorderThickness": 1,
//                    "dashLengthField": "dashLength",
//                    "legendValueText": "[[value]]",
//                    "title": "duration",
//                    "fillAlphas": 0,
//                    "valueField": "duration",
//                    "valueAxis": "durationAxis"
//                }],
//                "chartCursor": {
//                    "categoryBalloonDateFormat": "DD",
//                    "cursorAlpha": 0.1,
//                    "cursorColor": "#000000",
//                    "fullWidth": true,
//                    "valueBalloonsEnabled": false,
//                    "zoomable": false
//                },
//                "dataDateFormat": "YYYY-MM-DD",
//                "categoryField": "date",
//                "categoryAxis": {
//                    "dateFormats": [{
//                        "period": "DD",
//                        "format": "DD"
//                    }, {
//                        "period": "WW",
//                        "format": "MMM DD"
//                    }, {
//                        "period": "MM",
//                        "format": "MMM"
//                    }, {
//                        "period": "YYYY",
//                        "format": "YYYY"
//                    }],
//                    "parseDates": true,
//                    "autoGridCount": false,
//                    "axisColor": "#555555",
//                    "gridAlpha": 0.1,
//                    "gridColor": "#FFFFFF",
//                    "gridCount": 50
//                },
//                "export": {
//                    "enabled": true
//                }
//            });
//        }

//        if ($(element).is("#smoothed_line_chart")) {
//            var chart = AmCharts.makeChart("smoothed_line_chart", {
//                "type": "serial",
//                "theme": "light",
//                "marginTop": 0,
//                "marginRight": 80,
//                "dataProvider": [{
//                    "year": "1950",
//                    "value": -0.307
//                }, {
//                    "year": "1951",
//                    "value": -0.168
//                }, {
//                    "year": "1952",
//                    "value": -0.073
//                }, {
//                    "year": "1953",
//                    "value": -0.027
//                }, {
//                    "year": "1954",
//                    "value": -0.251
//                }, {
//                    "year": "1955",
//                    "value": -0.281
//                }, {
//                    "year": "1956",
//                    "value": -0.348
//                }, {
//                    "year": "1957",
//                    "value": -0.074
//                }, {
//                    "year": "1958",
//                    "value": -0.011
//                }, {
//                    "year": "1959",
//                    "value": -0.074
//                }, {
//                    "year": "1960",
//                    "value": -0.124
//                }, {
//                    "year": "1961",
//                    "value": -0.024
//                }, {
//                    "year": "1962",
//                    "value": -0.022
//                }, {
//                    "year": "1963",
//                    "value": 0
//                }, {
//                    "year": "1964",
//                    "value": -0.296
//                }, {
//                    "year": "1965",
//                    "value": -0.217
//                }, {
//                    "year": "1966",
//                    "value": -0.147
//                }, {
//                    "year": "1967",
//                    "value": -0.15
//                }, {
//                    "year": "1968",
//                    "value": -0.16
//                }, {
//                    "year": "1969",
//                    "value": -0.011
//                }, {
//                    "year": "1970",
//                    "value": -0.068
//                }, {
//                    "year": "1971",
//                    "value": -0.19
//                }, {
//                    "year": "1972",
//                    "value": -0.056
//                }, {
//                    "year": "1973",
//                    "value": 0.077
//                }, {
//                    "year": "1974",
//                    "value": -0.213
//                }, {
//                    "year": "1975",
//                    "value": -0.17
//                }, {
//                    "year": "1976",
//                    "value": -0.254
//                }, {
//                    "year": "1977",
//                    "value": 0.019
//                }, {
//                    "year": "1978",
//                    "value": -0.063
//                }, {
//                    "year": "1979",
//                    "value": 0.05
//                }, {
//                    "year": "1980",
//                    "value": 0.077
//                }, {
//                    "year": "1981",
//                    "value": 0.12
//                }, {
//                    "year": "1982",
//                    "value": 0.011
//                }, {
//                    "year": "1983",
//                    "value": 0.177
//                }, {
//                    "year": "1984",
//                    "value": -0.021
//                }, {
//                    "year": "1985",
//                    "value": -0.037
//                }, {
//                    "year": "1986",
//                    "value": 0.03
//                }, {
//                    "year": "1987",
//                    "value": 0.179
//                }, {
//                    "year": "1988",
//                    "value": 0.18
//                }, {
//                    "year": "1989",
//                    "value": 0.104
//                }, {
//                    "year": "1990",
//                    "value": 0.255
//                }, {
//                    "year": "1991",
//                    "value": 0.21
//                }, {
//                    "year": "1992",
//                    "value": 0.065
//                }, {
//                    "year": "1993",
//                    "value": 0.11
//                }, {
//                    "year": "1994",
//                    "value": 0.172
//                }, {
//                    "year": "1995",
//                    "value": 0.269
//                }, {
//                    "year": "1996",
//                    "value": 0.141
//                }, {
//                    "year": "1997",
//                    "value": 0.353
//                }, {
//                    "year": "1998",
//                    "value": 0.548
//                }, {
//                    "year": "1999",
//                    "value": 0.298
//                }, {
//                    "year": "2000",
//                    "value": 0.267
//                }, {
//                    "year": "2001",
//                    "value": 0.411
//                }, {
//                    "year": "2002",
//                    "value": 0.462
//                }, {
//                    "year": "2003",
//                    "value": 0.47
//                }, {
//                    "year": "2004",
//                    "value": 0.445
//                }, {
//                    "year": "2005",
//                    "value": 0.47
//                }],
//                "valueAxes": [{
//                    "axisAlpha": 0,
//                    "position": "left"
//                }],
//                "graphs": [{
//                    "id": "g1",
//                    "balloonText": "[[category]]<br><b><span style='font-size:14px;'>[[value]]</span></b>",
//                    "bullet": "round",
//                    "bulletSize": 8,
//                    "lineColor": "#d1655d",
//                    "lineThickness": 2,
//                    "negativeLineColor": "#637bb6",
//                    "type": "smoothedLine",
//                    "valueField": "value"
//                }],
//                "chartScrollbar": {
//                    "graph": "g1",
//                    "gridAlpha": 0,
//                    "color": "#888888",
//                    "scrollbarHeight": 55,
//                    "backgroundAlpha": 0,
//                    "selectedBackgroundAlpha": 0.1,
//                    "selectedBackgroundColor": "#888888",
//                    "graphFillAlpha": 0,
//                    "autoGridCount": true,
//                    "selectedGraphFillAlpha": 0,
//                    "graphLineAlpha": 0.2,
//                    "graphLineColor": "#c2c2c2",
//                    "selectedGraphLineColor": "#888888",
//                    "selectedGraphLineAlpha": 1

//                },
//                "chartCursor": {
//                    "categoryBalloonDateFormat": "YYYY",
//                    "cursorAlpha": 0,
//                    "valueLineEnabled": true,
//                    "valueLineBalloonEnabled": true,
//                    "valueLineAlpha": 0.5,
//                    "fullWidth": true
//                },
//                "dataDateFormat": "YYYY",
//                "categoryField": "year",
//                "categoryAxis": {
//                    "minPeriod": "YYYY",
//                    "parseDates": true,
//                    "minorGridAlpha": 0.1,
//                    "minorGridEnabled": true
//                },
//                "export": {
//                    "enabled": true
//                }
//            });

//            chart.addListener("rendered", zoomChart);
//            if (chart.zoomChart) {
//                chart.zoomChart();
//            }

//            function zoomChart() {
//                chart.zoomToIndexes(Math.round(chart.dataProvider.length * 0.4), Math.round(chart.dataProvider.length * 0.55));
//            }
//        }

//        if ($(element).is("#date_based_data")) {
//            var chart = AmCharts.makeChart("date_based_data", {
//                "type": "serial",
//                "theme": "light",
//                "marginRight": 40,
//                "marginLeft": 40,
//                "autoMarginOffset": 20,
//                "mouseWheelZoomEnabled": true,
//                "dataDateFormat": "YYYY-MM-DD",
//                "valueAxes": [{
//                    "id": "v1",
//                    "axisAlpha": 0,
//                    "position": "left",
//                    "ignoreAxisWidth": true
//                }],
//                "balloon": {
//                    "borderThickness": 1,
//                    "shadowAlpha": 0
//                },
//                "graphs": [{
//                    "id": "g1",
//                    "balloon": {
//                        "drop": true,
//                        "adjustBorderColor": false,
//                        "color": "#ffffff"
//                    },
//                    "bullet": "round",
//                    "bulletBorderAlpha": 1,
//                    "bulletColor": "#FFFFFF",
//                    "bulletSize": 5,
//                    "hideBulletsCount": 50,
//                    "lineThickness": 2,
//                    "title": "red line",
//                    "useLineColorForBulletBorder": true,
//                    "valueField": "value",
//                    "balloonText": "<span style='font-size:18px;'>[[value]]</span>"
//                }],
//                "chartScrollbar": {
//                    "graph": "g1",
//                    "oppositeAxis": false,
//                    "offset": 30,
//                    "scrollbarHeight": 80,
//                    "backgroundAlpha": 0,
//                    "selectedBackgroundAlpha": 0.1,
//                    "selectedBackgroundColor": "#888888",
//                    "graphFillAlpha": 0,
//                    "graphLineAlpha": 0.5,
//                    "selectedGraphFillAlpha": 0,
//                    "selectedGraphLineAlpha": 1,
//                    "autoGridCount": true,
//                    "color": "#AAAAAA"
//                },
//                "chartCursor": {
//                    "pan": true,
//                    "valueLineEnabled": true,
//                    "valueLineBalloonEnabled": true,
//                    "cursorAlpha": 1,
//                    "cursorColor": "#258cbb",
//                    "limitToGraph": "g1",
//                    "valueLineAlpha": 0.2,
//                    "valueZoomable": true
//                },
//                "valueScrollbar": {
//                    "oppositeAxis": false,
//                    "offset": 50,
//                    "scrollbarHeight": 10
//                },
//                "categoryField": "date",
//                "categoryAxis": {
//                    "parseDates": true,
//                    "dashLength": 1,
//                    "minorGridEnabled": true
//                },
//                "export": {
//                    "enabled": true
//                },
//                "dataProvider": [{
//                    "date": "2012-07-27",
//                    "value": 13
//                }, {
//                    "date": "2012-07-28",
//                    "value": 11
//                }, {
//                    "date": "2012-07-29",
//                    "value": 15
//                }, {
//                    "date": "2012-07-30",
//                    "value": 16
//                }, {
//                    "date": "2012-07-31",
//                    "value": 18
//                }, {
//                    "date": "2012-08-01",
//                    "value": 13
//                }, {
//                    "date": "2012-08-02",
//                    "value": 22
//                }, {
//                    "date": "2012-08-03",
//                    "value": 23
//                }, {
//                    "date": "2012-08-04",
//                    "value": 20
//                }, {
//                    "date": "2012-08-05",
//                    "value": 17
//                }, {
//                    "date": "2012-08-06",
//                    "value": 16
//                }, {
//                    "date": "2012-08-07",
//                    "value": 18
//                }, {
//                    "date": "2012-08-08",
//                    "value": 21
//                }, {
//                    "date": "2012-08-09",
//                    "value": 26
//                }, {
//                    "date": "2012-08-10",
//                    "value": 24
//                }, {
//                    "date": "2012-08-11",
//                    "value": 29
//                }, {
//                    "date": "2012-08-12",
//                    "value": 32
//                }, {
//                    "date": "2012-08-13",
//                    "value": 18
//                }, {
//                    "date": "2012-08-14",
//                    "value": 24
//                }, {
//                    "date": "2012-08-15",
//                    "value": 22
//                }, {
//                    "date": "2012-08-16",
//                    "value": 18
//                }, {
//                    "date": "2012-08-17",
//                    "value": 19
//                }, {
//                    "date": "2012-08-18",
//                    "value": 14
//                }, {
//                    "date": "2012-08-19",
//                    "value": 15
//                }, {
//                    "date": "2012-08-20",
//                    "value": 12
//                }, {
//                    "date": "2012-08-21",
//                    "value": 8
//                }, {
//                    "date": "2012-08-22",
//                    "value": 9
//                }, {
//                    "date": "2012-08-23",
//                    "value": 8
//                }, {
//                    "date": "2012-08-24",
//                    "value": 7
//                }, {
//                    "date": "2012-08-25",
//                    "value": 5
//                }, {
//                    "date": "2012-08-26",
//                    "value": 11
//                }, {
//                    "date": "2012-08-27",
//                    "value": 13
//                }, {
//                    "date": "2012-08-28",
//                    "value": 18
//                }, {
//                    "date": "2012-08-29",
//                    "value": 20
//                }, {
//                    "date": "2012-08-30",
//                    "value": 29
//                }, {
//                    "date": "2012-08-31",
//                    "value": 33
//                }, {
//                    "date": "2012-09-01",
//                    "value": 42
//                }, {
//                    "date": "2012-09-02",
//                    "value": 35
//                }, {
//                    "date": "2012-09-03",
//                    "value": 31
//                }, {
//                    "date": "2012-09-04",
//                    "value": 47
//                }, {
//                    "date": "2012-09-05",
//                    "value": 52
//                }, {
//                    "date": "2012-09-06",
//                    "value": 46
//                }, {
//                    "date": "2012-09-07",
//                    "value": 41
//                }, {
//                    "date": "2012-09-08",
//                    "value": 43
//                }, {
//                    "date": "2012-09-09",
//                    "value": 40
//                }, {
//                    "date": "2012-09-10",
//                    "value": 39
//                }, {
//                    "date": "2012-09-11",
//                    "value": 34
//                }, {
//                    "date": "2012-09-12",
//                    "value": 29
//                }, {
//                    "date": "2012-09-13",
//                    "value": 34
//                }, {
//                    "date": "2012-09-14",
//                    "value": 37
//                }, {
//                    "date": "2012-09-15",
//                    "value": 42
//                }, {
//                    "date": "2012-09-16",
//                    "value": 49
//                }, {
//                    "date": "2012-09-17",
//                    "value": 46
//                }, {
//                    "date": "2012-09-18",
//                    "value": 47
//                }, {
//                    "date": "2012-09-19",
//                    "value": 55
//                }, {
//                    "date": "2012-09-20",
//                    "value": 59
//                }, {
//                    "date": "2012-09-21",
//                    "value": 58
//                }, {
//                    "date": "2012-09-22",
//                    "value": 57
//                }, {
//                    "date": "2012-09-23",
//                    "value": 61
//                }, {
//                    "date": "2012-09-24",
//                    "value": 59
//                }, {
//                    "date": "2012-09-25",
//                    "value": 67
//                }, {
//                    "date": "2012-09-26",
//                    "value": 65
//                }, {
//                    "date": "2012-09-27",
//                    "value": 61
//                }, {
//                    "date": "2012-09-28",
//                    "value": 66
//                }, {
//                    "date": "2012-09-29",
//                    "value": 69
//                }, {
//                    "date": "2012-09-30",
//                    "value": 71
//                }, {
//                    "date": "2012-10-01",
//                    "value": 67
//                }, {
//                    "date": "2012-10-02",
//                    "value": 63
//                }, {
//                    "date": "2012-10-03",
//                    "value": 46
//                }, {
//                    "date": "2012-10-04",
//                    "value": 32
//                }, {
//                    "date": "2012-10-05",
//                    "value": 21
//                }, {
//                    "date": "2012-10-06",
//                    "value": 18
//                }, {
//                    "date": "2012-10-07",
//                    "value": 21
//                }, {
//                    "date": "2012-10-08",
//                    "value": 28
//                }, {
//                    "date": "2012-10-09",
//                    "value": 27
//                }, {
//                    "date": "2012-10-10",
//                    "value": 36
//                }, {
//                    "date": "2012-10-11",
//                    "value": 33
//                }, {
//                    "date": "2012-10-12",
//                    "value": 31
//                }, {
//                    "date": "2012-10-13",
//                    "value": 30
//                }, {
//                    "date": "2012-10-14",
//                    "value": 34
//                }, {
//                    "date": "2012-10-15",
//                    "value": 38
//                }, {
//                    "date": "2012-10-16",
//                    "value": 37
//                }, {
//                    "date": "2012-10-17",
//                    "value": 44
//                }, {
//                    "date": "2012-10-18",
//                    "value": 49
//                }, {
//                    "date": "2012-10-19",
//                    "value": 53
//                }, {
//                    "date": "2012-10-20",
//                    "value": 57
//                }, {
//                    "date": "2012-10-21",
//                    "value": 60
//                }, {
//                    "date": "2012-10-22",
//                    "value": 61
//                }, {
//                    "date": "2012-10-23",
//                    "value": 69
//                }, {
//                    "date": "2012-10-24",
//                    "value": 67
//                }, {
//                    "date": "2012-10-25",
//                    "value": 72
//                }, {
//                    "date": "2012-10-26",
//                    "value": 77
//                }, {
//                    "date": "2012-10-27",
//                    "value": 75
//                }, {
//                    "date": "2012-10-28",
//                    "value": 70
//                }, {
//                    "date": "2012-10-29",
//                    "value": 72
//                }, {
//                    "date": "2012-10-30",
//                    "value": 70
//                }, {
//                    "date": "2012-10-31",
//                    "value": 72
//                }, {
//                    "date": "2012-11-01",
//                    "value": 73
//                }, {
//                    "date": "2012-11-02",
//                    "value": 67
//                }, {
//                    "date": "2012-11-03",
//                    "value": 68
//                }, {
//                    "date": "2012-11-04",
//                    "value": 65
//                }, {
//                    "date": "2012-11-05",
//                    "value": 71
//                }, {
//                    "date": "2012-11-06",
//                    "value": 75
//                }, {
//                    "date": "2012-11-07",
//                    "value": 74
//                }, {
//                    "date": "2012-11-08",
//                    "value": 71
//                }, {
//                    "date": "2012-11-09",
//                    "value": 76
//                }, {
//                    "date": "2012-11-10",
//                    "value": 77
//                }, {
//                    "date": "2012-11-11",
//                    "value": 81
//                }, {
//                    "date": "2012-11-12",
//                    "value": 83
//                }, {
//                    "date": "2012-11-13",
//                    "value": 80
//                }, {
//                    "date": "2012-11-14",
//                    "value": 81
//                }, {
//                    "date": "2012-11-15",
//                    "value": 87
//                }, {
//                    "date": "2012-11-16",
//                    "value": 82
//                }, {
//                    "date": "2012-11-17",
//                    "value": 86
//                }, {
//                    "date": "2012-11-18",
//                    "value": 80
//                }, {
//                    "date": "2012-11-19",
//                    "value": 87
//                }, {
//                    "date": "2012-11-20",
//                    "value": 83
//                }, {
//                    "date": "2012-11-21",
//                    "value": 85
//                }, {
//                    "date": "2012-11-22",
//                    "value": 84
//                }, {
//                    "date": "2012-11-23",
//                    "value": 82
//                }, {
//                    "date": "2012-11-24",
//                    "value": 73
//                }, {
//                    "date": "2012-11-25",
//                    "value": 71
//                }, {
//                    "date": "2012-11-26",
//                    "value": 75
//                }, {
//                    "date": "2012-11-27",
//                    "value": 79
//                }, {
//                    "date": "2012-11-28",
//                    "value": 70
//                }, {
//                    "date": "2012-11-29",
//                    "value": 73
//                }, {
//                    "date": "2012-11-30",
//                    "value": 61
//                }, {
//                    "date": "2012-12-01",
//                    "value": 62
//                }, {
//                    "date": "2012-12-02",
//                    "value": 66
//                }, {
//                    "date": "2012-12-03",
//                    "value": 65
//                }, {
//                    "date": "2012-12-04",
//                    "value": 73
//                }, {
//                    "date": "2012-12-05",
//                    "value": 79
//                }, {
//                    "date": "2012-12-06",
//                    "value": 78
//                }, {
//                    "date": "2012-12-07",
//                    "value": 78
//                }, {
//                    "date": "2012-12-08",
//                    "value": 78
//                }, {
//                    "date": "2012-12-09",
//                    "value": 74
//                }, {
//                    "date": "2012-12-10",
//                    "value": 73
//                }, {
//                    "date": "2012-12-11",
//                    "value": 75
//                }, {
//                    "date": "2012-12-12",
//                    "value": 70
//                }, {
//                    "date": "2012-12-13",
//                    "value": 77
//                }, {
//                    "date": "2012-12-14",
//                    "value": 67
//                }, {
//                    "date": "2012-12-15",
//                    "value": 62
//                }, {
//                    "date": "2012-12-16",
//                    "value": 64
//                }, {
//                    "date": "2012-12-17",
//                    "value": 61
//                }, {
//                    "date": "2012-12-18",
//                    "value": 59
//                }, {
//                    "date": "2012-12-19",
//                    "value": 53
//                }, {
//                    "date": "2012-12-20",
//                    "value": 54
//                }, {
//                    "date": "2012-12-21",
//                    "value": 56
//                }, {
//                    "date": "2012-12-22",
//                    "value": 59
//                }, {
//                    "date": "2012-12-23",
//                    "value": 58
//                }, {
//                    "date": "2012-12-24",
//                    "value": 55
//                }, {
//                    "date": "2012-12-25",
//                    "value": 52
//                }, {
//                    "date": "2012-12-26",
//                    "value": 54
//                }, {
//                    "date": "2012-12-27",
//                    "value": 50
//                }, {
//                    "date": "2012-12-28",
//                    "value": 50
//                }, {
//                    "date": "2012-12-29",
//                    "value": 51
//                }, {
//                    "date": "2012-12-30",
//                    "value": 52
//                }, {
//                    "date": "2012-12-31",
//                    "value": 58
//                }, {
//                    "date": "2013-01-01",
//                    "value": 60
//                }, {
//                    "date": "2013-01-02",
//                    "value": 67
//                }, {
//                    "date": "2013-01-03",
//                    "value": 64
//                }, {
//                    "date": "2013-01-04",
//                    "value": 66
//                }, {
//                    "date": "2013-01-05",
//                    "value": 60
//                }, {
//                    "date": "2013-01-06",
//                    "value": 63
//                }, {
//                    "date": "2013-01-07",
//                    "value": 61
//                }, {
//                    "date": "2013-01-08",
//                    "value": 60
//                }, {
//                    "date": "2013-01-09",
//                    "value": 65
//                }, {
//                    "date": "2013-01-10",
//                    "value": 75
//                }, {
//                    "date": "2013-01-11",
//                    "value": 77
//                }, {
//                    "date": "2013-01-12",
//                    "value": 78
//                }, {
//                    "date": "2013-01-13",
//                    "value": 70
//                }, {
//                    "date": "2013-01-14",
//                    "value": 70
//                }, {
//                    "date": "2013-01-15",
//                    "value": 73
//                }, {
//                    "date": "2013-01-16",
//                    "value": 71
//                }, {
//                    "date": "2013-01-17",
//                    "value": 74
//                }, {
//                    "date": "2013-01-18",
//                    "value": 78
//                }, {
//                    "date": "2013-01-19",
//                    "value": 85
//                }, {
//                    "date": "2013-01-20",
//                    "value": 82
//                }, {
//                    "date": "2013-01-21",
//                    "value": 83
//                }, {
//                    "date": "2013-01-22",
//                    "value": 88
//                }, {
//                    "date": "2013-01-23",
//                    "value": 85
//                }, {
//                    "date": "2013-01-24",
//                    "value": 85
//                }, {
//                    "date": "2013-01-25",
//                    "value": 80
//                }, {
//                    "date": "2013-01-26",
//                    "value": 87
//                }, {
//                    "date": "2013-01-27",
//                    "value": 84
//                }, {
//                    "date": "2013-01-28",
//                    "value": 83
//                }, {
//                    "date": "2013-01-29",
//                    "value": 84
//                }, {
//                    "date": "2013-01-30",
//                    "value": 81
//                }]
//            });

//            chart.addListener("rendered", zoomChart);

//            zoomChart();

//            function zoomChart() {
//                chart.zoomToIndexes(chart.dataProvider.length - 40, chart.dataProvider.length - 1);
//            }
//        }

//        if ($(element).is("#step_line_chart")) {
//            var chart = AmCharts.makeChart("step_line_chart", {
//                "type": "serial",
//                "titles": [{ "text": "Step Line Chart" }],
//                "theme": "light",
//                "autoMarginOffset": 25,
//                "dataProvider": [{
//                    "year": "1950",
//                    "value": -0.307
//                }, {
//                    "year": "1951",
//                    "value": -0.168
//                }, {
//                    "year": "1952",
//                    "value": -0.073
//                }, {
//                    "year": "1953",
//                    "value": -0.027
//                }, {
//                    "year": "1954",
//                    "value": -0.251
//                }, {
//                    "year": "1955",
//                    "value": -0.281
//                }, {
//                    "year": "1956",
//                    "value": -0.348
//                }, {
//                    "year": "1957",
//                    "value": -0.074
//                }, {
//                    "year": "1958",
//                    "value": -0.011
//                }, {
//                    "year": "1959",
//                    "value": -0.074
//                }, {
//                    "year": "1960",
//                    "value": -0.124
//                }, {
//                    "year": "1961",
//                    "value": -0.024
//                }, {
//                    "year": "1962",
//                    "value": -0.022
//                }, {
//                    "year": "1963",
//                    "value": 0
//                }, {
//                    "year": "1964",
//                    "value": -0.296
//                }, {
//                    "year": "1965",
//                    "value": -0.217
//                }, {
//                    "year": "1966",
//                    "value": -0.147
//                }, {
//                    "year": "1967",
//                    "value": -0.15
//                }, {
//                    "year": "1968",
//                    "value": -0.16
//                }, {
//                    "year": "1969",
//                    "value": -0.011
//                }, {
//                    "year": "1970",
//                    "value": -0.068
//                }, {
//                    "year": "1971",
//                    "value": -0.19
//                }, {
//                    "year": "1972",
//                    "value": -0.056
//                }, {
//                    "year": "1973",
//                    "value": 0.077
//                }, {
//                    "year": "1974",
//                    "value": -0.213
//                }, {
//                    "year": "1975",
//                    "value": -0.17
//                }, {
//                    "year": "1976",
//                    "value": -0.254
//                }, {
//                    "year": "1977",
//                    "value": 0.019
//                }, {
//                    "year": "1978",
//                    "value": -0.063
//                }, {
//                    "year": "1979",
//                    "value": 0.05
//                }, {
//                    "year": "1980",
//                    "value": 0.077
//                }, {
//                    "year": "1981",
//                    "value": 0.12
//                }, {
//                    "year": "1982",
//                    "value": 0.011
//                }, {
//                    "year": "1983",
//                    "value": 0.177
//                }, {
//                    "year": "1984",
//                    "value": -0.021
//                }, {
//                    "year": "1985",
//                    "value": -0.037
//                }, {
//                    "year": "1986",
//                    "value": 0.03
//                }, {
//                    "year": "1987",
//                    "value": 0.179
//                }, {
//                    "year": "1988",
//                    "value": 0.18
//                }, {
//                    "year": "1989",
//                    "value": 0.104
//                }, {
//                    "year": "1990",
//                    "value": 0.255
//                }, {
//                    "year": "1991",
//                    "value": 0.21
//                }, {
//                    "year": "1992",
//                    "value": 0.065
//                }, {
//                    "year": "1993",
//                    "value": 0.11
//                }, {
//                    "year": "1994",
//                    "value": 0.172
//                }, {
//                    "year": "1995",
//                    "value": 0.269
//                }, {
//                    "year": "1996",
//                    "value": 0.141
//                }, {
//                    "year": "1997",
//                    "value": 0.353
//                }, {
//                    "year": "1998",
//                    "value": 0.548
//                }, {
//                    "year": "1999",
//                    "value": 0.298
//                }, {
//                    "year": "2000",
//                    "value": 0.267
//                }, {
//                    "year": "2001",
//                    "value": 0.411
//                }, {
//                    "year": "2002",
//                    "value": 0.462
//                }, {
//                    "year": "2003",
//                    "value": 0.47
//                }, {
//                    "year": "2004",
//                    "value": 0.445
//                }, {
//                    "year": "2005",
//                    "value": 0.47
//                }],
//                "valueAxes": [{
//                    "axisAlpha": 0,
//                    "position": "right"
//                }],
//                "graphs": [{
//                    "id": "g1",
//                    "balloonText": "[[category]]<br><b>[[value]] C</b>",
//                    "type": "step",
//                    "lineThickness": 2,
//                    "bullet": "square",
//                    "bulletAlpha": 0,
//                    "bulletSize": 4,
//                    "bulletBorderAlpha": 0,
//                    "valueField": "value"
//                }],
//                "chartScrollbar": {
//                    "graph": "g1",
//                    "gridAlpha": 0,
//                    "color": "#888888",
//                    "scrollbarHeight": 55,
//                    "backgroundAlpha": 0,
//                    "selectedBackgroundAlpha": 0.1,
//                    "selectedBackgroundColor": "#888888",
//                    "graphFillAlpha": 0,
//                    "autoGridCount": true,
//                    "selectedGraphFillAlpha": 0,
//                    "graphLineAlpha": 1,
//                    "graphLineColor": "#c2c2c2",
//                    "selectedGraphLineColor": "#888888",
//                    "selectedGraphLineAlpha": 1
//                },
//                "chartCursor": {
//                    "fullWidth": true,
//                    "categoryBalloonDateFormat": "YYYY",
//                    "cursorAlpha": 0.05,
//                    "graphBulletAlpha": 1
//                },
//                "dataDateFormat": "YYYY",
//                "categoryField": "year",
//                "categoryAxis": {
//                    "minPeriod": "YYYY",
//                    "parseDates": true,
//                    "gridAlpha": 0
//                },
//                "export": {
//                    "enabled": true
//                }
//            });

//            chart.addListener("dataUpdated", zoomChart);

//            function zoomChart() {
//                chart.zoomToDates(new Date(1965, 0), new Date(1975, 0));
//            }
//        }

//        if ($(element).is("#donut_with_radial_gradient")) {
//            var chart = AmCharts.makeChart("donut_with_radial_gradient", {
//                "type": "pie",
//                "theme": "light",
//                "innerRadius": "40%",
//                "gradientRatio": [-0.4, -0.4, -0.4, -0.4, -0.4, -0.4, 0, 0.1, 0.2, 0.1, 0, -0.2, -0.5],
//                "dataProvider": [{
//                    "country": "Lithuania",
//                    "litres": 501.9
//                }, {
//                    "country": "Czech Republic",
//                    "litres": 301.9
//                }, {
//                    "country": "Ireland",
//                    "litres": 201.1
//                }, {
//                    "country": "Germany",
//                    "litres": 165.8
//                }, {
//                    "country": "Australia",
//                    "litres": 139.9
//                }, {
//                    "country": "Austria",
//                    "litres": 128.3
//                }],
//                "balloonText": "[[value]]",
//                "valueField": "litres",
//                "titleField": "country",
//                "balloon": {
//                    "drop": true,
//                    "adjustBorderColor": false,
//                    "color": "#FFFFFF",
//                    "fontSize": 16
//                },
//                "export": {
//                    "enabled": true
//                }
//            });
//        }

//        if ($(element).is("#animated_time_line_pie_chart")) {
//            /**
//            * Define data for each year
//            */
//            var chartData = {
//                "1995": [
//                    { "sector": "Agriculture", "size": 6.6 },
//                    { "sector": "Mining and Quarrying", "size": 0.6 },
//                    { "sector": "Manufacturing", "size": 23.2 },
//                    { "sector": "Electricity and Water", "size": 2.2 },
//                    { "sector": "Construction", "size": 4.5 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 14.6 },
//                    { "sector": "Transport and Communication", "size": 9.3 },
//                    { "sector": "Finance, real estate and business services", "size": 22.5 }],
//                "1996": [
//                    { "sector": "Agriculture", "size": 6.4 },
//                    { "sector": "Mining and Quarrying", "size": 0.5 },
//                    { "sector": "Manufacturing", "size": 22.4 },
//                    { "sector": "Electricity and Water", "size": 2 },
//                    { "sector": "Construction", "size": 4.2 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 14.8 },
//                    { "sector": "Transport and Communication", "size": 9.7 },
//                    { "sector": "Finance, real estate and business services", "size": 22 }],
//                "1997": [
//                    { "sector": "Agriculture", "size": 6.1 },
//                    { "sector": "Mining and Quarrying", "size": 0.2 },
//                    { "sector": "Manufacturing", "size": 20.9 },
//                    { "sector": "Electricity and Water", "size": 1.8 },
//                    { "sector": "Construction", "size": 4.2 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 13.7 },
//                    { "sector": "Transport and Communication", "size": 9.4 },
//                    { "sector": "Finance, real estate and business services", "size": 22.1 }],
//                "1998": [
//                    { "sector": "Agriculture", "size": 6.2 },
//                    { "sector": "Mining and Quarrying", "size": 0.3 },
//                    { "sector": "Manufacturing", "size": 21.4 },
//                    { "sector": "Electricity and Water", "size": 1.9 },
//                    { "sector": "Construction", "size": 4.2 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 14.5 },
//                    { "sector": "Transport and Communication", "size": 10.6 },
//                    { "sector": "Finance, real estate and business services", "size": 23 }],
//                "1999": [
//                    { "sector": "Agriculture", "size": 5.7 },
//                    { "sector": "Mining and Quarrying", "size": 0.2 },
//                    { "sector": "Manufacturing", "size": 20 },
//                    { "sector": "Electricity and Water", "size": 1.8 },
//                    { "sector": "Construction", "size": 4.4 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 15.2 },
//                    { "sector": "Transport and Communication", "size": 10.5 },
//                    { "sector": "Finance, real estate and business services", "size": 24.7 }],
//                "2000": [
//                    { "sector": "Agriculture", "size": 5.1 },
//                    { "sector": "Mining and Quarrying", "size": 0.3 },
//                    { "sector": "Manufacturing", "size": 20.4 },
//                    { "sector": "Electricity and Water", "size": 1.7 },
//                    { "sector": "Construction", "size": 4 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 16.3 },
//                    { "sector": "Transport and Communication", "size": 10.7 },
//                    { "sector": "Finance, real estate and business services", "size": 24.6 }],
//                "2001": [
//                    { "sector": "Agriculture", "size": 5.5 },
//                    { "sector": "Mining and Quarrying", "size": 0.2 },
//                    { "sector": "Manufacturing", "size": 20.3 },
//                    { "sector": "Electricity and Water", "size": 1.6 },
//                    { "sector": "Construction", "size": 3.1 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 16.3 },
//                    { "sector": "Transport and Communication", "size": 10.7 },
//                    { "sector": "Finance, real estate and business services", "size": 25.8 }],
//                "2002": [
//                    { "sector": "Agriculture", "size": 5.7 },
//                    { "sector": "Mining and Quarrying", "size": 0.2 },
//                    { "sector": "Manufacturing", "size": 20.5 },
//                    { "sector": "Electricity and Water", "size": 1.6 },
//                    { "sector": "Construction", "size": 3.6 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 16.1 },
//                    { "sector": "Transport and Communication", "size": 10.7 },
//                    { "sector": "Finance, real estate and business services", "size": 26 }],
//                "2003": [
//                    { "sector": "Agriculture", "size": 4.9 },
//                    { "sector": "Mining and Quarrying", "size": 0.2 },
//                    { "sector": "Manufacturing", "size": 19.4 },
//                    { "sector": "Electricity and Water", "size": 1.5 },
//                    { "sector": "Construction", "size": 3.3 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 16.2 },
//                    { "sector": "Transport and Communication", "size": 11 },
//                    { "sector": "Finance, real estate and business services", "size": 27.5 }],
//                "2004": [
//                    { "sector": "Agriculture", "size": 4.7 },
//                    { "sector": "Mining and Quarrying", "size": 0.2 },
//                    { "sector": "Manufacturing", "size": 18.4 },
//                    { "sector": "Electricity and Water", "size": 1.4 },
//                    { "sector": "Construction", "size": 3.3 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 16.9 },
//                    { "sector": "Transport and Communication", "size": 10.6 },
//                    { "sector": "Finance, real estate and business services", "size": 28.1 }],
//                "2005": [
//                    { "sector": "Agriculture", "size": 4.3 },
//                    { "sector": "Mining and Quarrying", "size": 0.2 },
//                    { "sector": "Manufacturing", "size": 18.1 },
//                    { "sector": "Electricity and Water", "size": 1.4 },
//                    { "sector": "Construction", "size": 3.9 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 15.7 },
//                    { "sector": "Transport and Communication", "size": 10.6 },
//                    { "sector": "Finance, real estate and business services", "size": 29.1 }],
//                "2006": [
//                    { "sector": "Agriculture", "size": 4 },
//                    { "sector": "Mining and Quarrying", "size": 0.2 },
//                    { "sector": "Manufacturing", "size": 16.5 },
//                    { "sector": "Electricity and Water", "size": 1.3 },
//                    { "sector": "Construction", "size": 3.7 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 14.2 },
//                    { "sector": "Transport and Communication", "size": 12.1 },
//                    { "sector": "Finance, real estate and business services", "size": 29.1 }],
//                "2007": [
//                    { "sector": "Agriculture", "size": 4.7 },
//                    { "sector": "Mining and Quarrying", "size": 0.2 },
//                    { "sector": "Manufacturing", "size": 16.2 },
//                    { "sector": "Electricity and Water", "size": 1.2 },
//                    { "sector": "Construction", "size": 4.1 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 15.6 },
//                    { "sector": "Transport and Communication", "size": 11.2 },
//                    { "sector": "Finance, real estate and business services", "size": 30.4 }],
//                "2008": [
//                    { "sector": "Agriculture", "size": 4.9 },
//                    { "sector": "Mining and Quarrying", "size": 0.3 },
//                    { "sector": "Manufacturing", "size": 17.2 },
//                    { "sector": "Electricity and Water", "size": 1.4 },
//                    { "sector": "Construction", "size": 5.1 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 15.4 },
//                    { "sector": "Transport and Communication", "size": 11.1 },
//                    { "sector": "Finance, real estate and business services", "size": 28.4 }],
//                "2009": [
//                    { "sector": "Agriculture", "size": 4.7 },
//                    { "sector": "Mining and Quarrying", "size": 0.3 },
//                    { "sector": "Manufacturing", "size": 16.4 },
//                    { "sector": "Electricity and Water", "size": 1.9 },
//                    { "sector": "Construction", "size": 4.9 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 15.5 },
//                    { "sector": "Transport and Communication", "size": 10.9 },
//                    { "sector": "Finance, real estate and business services", "size": 27.9 }],
//                "2010": [
//                    { "sector": "Agriculture", "size": 4.2 },
//                    { "sector": "Mining and Quarrying", "size": 0.3 },
//                    { "sector": "Manufacturing", "size": 16.2 },
//                    { "sector": "Electricity and Water", "size": 2.2 },
//                    { "sector": "Construction", "size": 4.3 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 15.7 },
//                    { "sector": "Transport and Communication", "size": 10.2 },
//                    { "sector": "Finance, real estate and business services", "size": 28.8 }],
//                "2011": [
//                    { "sector": "Agriculture", "size": 4.1 },
//                    { "sector": "Mining and Quarrying", "size": 0.3 },
//                    { "sector": "Manufacturing", "size": 14.9 },
//                    { "sector": "Electricity and Water", "size": 2.3 },
//                    { "sector": "Construction", "size": 5 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 17.3 },
//                    { "sector": "Transport and Communication", "size": 10.2 },
//                    { "sector": "Finance, real estate and business services", "size": 27.2 }],
//                "2012": [
//                    { "sector": "Agriculture", "size": 3.8 },
//                    { "sector": "Mining and Quarrying", "size": 0.3 },
//                    { "sector": "Manufacturing", "size": 14.9 },
//                    { "sector": "Electricity and Water", "size": 2.6 },
//                    { "sector": "Construction", "size": 5.1 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 15.8 },
//                    { "sector": "Transport and Communication", "size": 10.7 },
//                    { "sector": "Finance, real estate and business services", "size": 28 }],
//                "2013": [
//                    { "sector": "Agriculture", "size": 3.7 },
//                    { "sector": "Mining and Quarrying", "size": 0.2 },
//                    { "sector": "Manufacturing", "size": 14.9 },
//                    { "sector": "Electricity and Water", "size": 2.7 },
//                    { "sector": "Construction", "size": 5.7 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 16.5 },
//                    { "sector": "Transport and Communication", "size": 10.5 },
//                    { "sector": "Finance, real estate and business services", "size": 26.6 }],
//                "2014": [
//                    { "sector": "Agriculture", "size": 3.9 },
//                    { "sector": "Mining and Quarrying", "size": 0.2 },
//                    { "sector": "Manufacturing", "size": 14.5 },
//                    { "sector": "Electricity and Water", "size": 2.7 },
//                    { "sector": "Construction", "size": 5.6 },
//                    { "sector": "Trade (Wholesale, Retail, Motor)", "size": 16.6 },
//                    { "sector": "Transport and Communication", "size": 10.5 },
//                    { "sector": "Finance, real estate and business services", "size": 26.5 }]
//            };


//            /**
//            * Create the chart
//            */
//            var currentYear = 1995;
//            var chart = AmCharts.makeChart("animated_time_line_pie_chart", {
//                "type": "pie",
//                "theme": "light",
//                "dataProvider": [],
//                "valueField": "size",
//                "titleField": "sector",
//                "startDuration": 0,
//                "innerRadius": 80,
//                "pullOutRadius": 20,
//                "marginTop": 30,
//                "titles": [{
//                    "text": "South African Economy"
//                }],
//                "allLabels": [{
//                    "y": "54%",
//                    "align": "center",
//                    "size": 25,
//                    "bold": true,
//                    "text": "1995",
//                    "color": "#555"
//                }, {
//                    "y": "49%",
//                    "align": "center",
//                    "size": 15,
//                    "text": "Year",
//                    "color": "#555"
//                }],
//                "listeners": [{
//                    "event": "init",
//                    "method": function (e) {
//                        var chart = e.chart;

//                        function getCurrentData() {
//                            var data = chartData[currentYear];
//                            currentYear++;
//                            if (currentYear > 2014)
//                                currentYear = 1995;
//                            return data;
//                        }

//                        function loop() {
//                            chart.allLabels[0].text = currentYear;
//                            var data = getCurrentData();
//                            chart.animateData(data, {
//                                duration: 1000,
//                                complete: function () {
//                                    setTimeout(loop, 3000);
//                                }
//                            });
//                        }

//                        loop();
//                    }
//                }],
//                "export": {
//                    "enabled": true
//                }
//            });
//        }

//        if ($(element).is("#combined_bullet_column_line_chart")) {
//            var chart = AmCharts.makeChart("combined_bullet_column_line_chart", {
//                "type": "serial",
//                "theme": "light",
//                "dataDateFormat": "YYYY-MM-DD",
//                "precision": 2,
//                "valueAxes": [{
//                    "id": "v1",
//                    "title": "Sales",
//                    "position": "left",
//                    "autoGridCount": false,
//                    "labelFunction": function (value) {
//                        return "$" + Math.round(value) + "M";
//                    }
//                }, {
//                    "id": "v2",
//                    "title": "Market Days",
//                    "gridAlpha": 0,
//                    "position": "right",
//                    "autoGridCount": false
//                }],
//                "graphs": [{
//                    "id": "g3",
//                    "valueAxis": "v1",
//                    "lineColor": "#e1ede9",
//                    "fillColors": "#e1ede9",
//                    "fillAlphas": 1,
//                    "type": "column",
//                    "title": "Actual Sales",
//                    "valueField": "sales2",
//                    "clustered": false,
//                    "columnWidth": 0.5,
//                    "legendValueText": "$[[value]]M",
//                    "balloonText": "[[title]]<br /><b style='font-size: 130%'>$[[value]]M</b>"
//                }, {
//                    "id": "g4",
//                    "valueAxis": "v1",
//                    "lineColor": "#62cf73",
//                    "fillColors": "#62cf73",
//                    "fillAlphas": 1,
//                    "type": "column",
//                    "title": "Target Sales",
//                    "valueField": "sales1",
//                    "clustered": false,
//                    "columnWidth": 0.3,
//                    "legendValueText": "$[[value]]M",
//                    "balloonText": "[[title]]<br /><b style='font-size: 130%'>$[[value]]M</b>"
//                }, {
//                    "id": "g1",
//                    "valueAxis": "v2",
//                    "bullet": "round",
//                    "bulletBorderAlpha": 1,
//                    "bulletColor": "#FFFFFF",
//                    "bulletSize": 5,
//                    "hideBulletsCount": 50,
//                    "lineThickness": 2,
//                    "lineColor": "#20acd4",
//                    "type": "smoothedLine",
//                    "title": "Market Days",
//                    "useLineColorForBulletBorder": true,
//                    "valueField": "market1",
//                    "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
//                }, {
//                    "id": "g2",
//                    "valueAxis": "v2",
//                    "bullet": "round",
//                    "bulletBorderAlpha": 1,
//                    "bulletColor": "#FFFFFF",
//                    "bulletSize": 5,
//                    "hideBulletsCount": 50,
//                    "lineThickness": 2,
//                    "lineColor": "#e1ede9",
//                    "type": "smoothedLine",
//                    "dashLength": 5,
//                    "title": "Market Days ALL",
//                    "useLineColorForBulletBorder": true,
//                    "valueField": "market2",
//                    "balloonText": "[[title]]<br /><b style='font-size: 130%'>[[value]]</b>"
//                }],
//                "chartScrollbar": {
//                    "graph": "g1",
//                    "oppositeAxis": false,
//                    "offset": 30,
//                    "scrollbarHeight": 50,
//                    "backgroundAlpha": 0,
//                    "selectedBackgroundAlpha": 0.1,
//                    "selectedBackgroundColor": "#888888",
//                    "graphFillAlpha": 0,
//                    "graphLineAlpha": 0.5,
//                    "selectedGraphFillAlpha": 0,
//                    "selectedGraphLineAlpha": 1,
//                    "autoGridCount": true,
//                    "color": "#AAAAAA"
//                },
//                "chartCursor": {
//                    "pan": true,
//                    "valueLineEnabled": true,
//                    "valueLineBalloonEnabled": true,
//                    "cursorAlpha": 0,
//                    "valueLineAlpha": 0.2
//                },
//                "categoryField": "date",
//                "categoryAxis": {
//                    "parseDates": true,
//                    "dashLength": 1,
//                    "minorGridEnabled": true
//                },
//                "legend": {
//                    "useGraphSettings": true,
//                    "position": "top"
//                },
//                "balloon": {
//                    "borderThickness": 1,
//                    "shadowAlpha": 0
//                },
//                "export": {
//                    "enabled": true
//                },
//                "dataProvider": [{
//                    "date": "2013-01-16",
//                    "market1": 71,
//                    "market2": 75,
//                    "sales1": 5,
//                    "sales2": 8
//                }, {
//                    "date": "2013-01-17",
//                    "market1": 74,
//                    "market2": 78,
//                    "sales1": 4,
//                    "sales2": 6
//                }, {
//                    "date": "2013-01-18",
//                    "market1": 78,
//                    "market2": 88,
//                    "sales1": 5,
//                    "sales2": 2
//                }, {
//                    "date": "2013-01-19",
//                    "market1": 85,
//                    "market2": 89,
//                    "sales1": 8,
//                    "sales2": 9
//                }, {
//                    "date": "2013-01-20",
//                    "market1": 82,
//                    "market2": 89,
//                    "sales1": 9,
//                    "sales2": 6
//                }, {
//                    "date": "2013-01-21",
//                    "market1": 83,
//                    "market2": 85,
//                    "sales1": 3,
//                    "sales2": 5
//                }, {
//                    "date": "2013-01-22",
//                    "market1": 88,
//                    "market2": 92,
//                    "sales1": 5,
//                    "sales2": 7
//                }, {
//                    "date": "2013-01-23",
//                    "market1": 85,
//                    "market2": 90,
//                    "sales1": 7,
//                    "sales2": 6
//                }, {
//                    "date": "2013-01-24",
//                    "market1": 85,
//                    "market2": 91,
//                    "sales1": 9,
//                    "sales2": 5
//                }, {
//                    "date": "2013-01-25",
//                    "market1": 80,
//                    "market2": 84,
//                    "sales1": 5,
//                    "sales2": 8
//                }, {
//                    "date": "2013-01-26",
//                    "market1": 87,
//                    "market2": 92,
//                    "sales1": 4,
//                    "sales2": 8
//                }, {
//                    "date": "2013-01-27",
//                    "market1": 84,
//                    "market2": 87,
//                    "sales1": 3,
//                    "sales2": 4
//                }, {
//                    "date": "2013-01-28",
//                    "market1": 83,
//                    "market2": 88,
//                    "sales1": 5,
//                    "sales2": 7
//                }, {
//                    "date": "2013-01-29",
//                    "market1": 84,
//                    "market2": 87,
//                    "sales1": 5,
//                    "sales2": 8
//                }, {
//                    "date": "2013-01-30",
//                    "market1": 81,
//                    "market2": 85,
//                    "sales1": 4,
//                    "sales2": 7
//                }]
//            });

//        }

//        if ($(element).is("#polar_chart")) {
//            var chart = AmCharts.makeChart("polar_chart", {
//                "type": "radar",
//                "theme": "light",
//                "dataProvider": [{
//                    "direction": "N",
//                    "value": 8
//                }, {
//                    "direction": "NE",
//                    "value": 9
//                }, {
//                    "direction": "E",
//                    "value": 4.5
//                }, {
//                    "direction": "SE",
//                    "value": 3.5
//                }, {
//                    "direction": "S",
//                    "value": 9.2
//                }, {
//                    "direction": "SW",
//                    "value": 8.4
//                }, {
//                    "direction": "W",
//                    "value": 11.1
//                }, {
//                    "direction": "NW",
//                    "value": 10
//                }],
//                "valueAxes": [{
//                    "gridType": "circles",
//                    "minimum": 0,
//                    "autoGridCount": false,
//                    "axisAlpha": 0.2,
//                    "fillAlpha": 0.05,
//                    "fillColor": "#FFFFFF",
//                    "gridAlpha": 0.08,
//                    "guides": [{
//                        "angle": 225,
//                        "fillAlpha": 0.3,
//                        "fillColor": "#0066CC",
//                        "tickLength": 0,
//                        "toAngle": 315,
//                        "toValue": 14,
//                        "value": 0,
//                        "lineAlpha": 0,

//                    }, {
//                        "angle": 45,
//                        "fillAlpha": 0.3,
//                        "fillColor": "#CC3333",
//                        "tickLength": 0,
//                        "toAngle": 135,
//                        "toValue": 14,
//                        "value": 0,
//                        "lineAlpha": 0,
//                    }],
//                    "position": "left"
//                }],
//                "startDuration": 1,
//                "graphs": [{
//                    "balloonText": "[[category]]: [[value]] m/s",
//                    "bullet": "round",
//                    "fillAlphas": 0.3,
//                    "valueField": "value"
//                }],
//                "categoryField": "direction",
//                "export": {
//                    "enabled": true
//                }
//            });

//        }

//        if ($(element).is("#solid_guage")) {
//            var gaugeChart = AmCharts.makeChart("solid_guage", {
//                "type": "gauge",
//                "theme": "light",
//                "axes": [{
//                    "axisAlpha": 0,
//                    "tickAlpha": 0,
//                    "labelsEnabled": false,
//                    "startValue": 0,
//                    "endValue": 100,
//                    "startAngle": 0,
//                    "endAngle": 270,
//                    "bands": [{
//                        "color": "#eee",
//                        "startValue": 0,
//                        "endValue": 100,
//                        "radius": "100%",
//                        "innerRadius": "85%"
//                    }, {
//                        "color": "#84b761",
//                        "startValue": 0,
//                        "endValue": 80,
//                        "radius": "100%",
//                        "innerRadius": "85%",
//                        "balloonText": "90%"
//                    }, {
//                        "color": "#eee",
//                        "startValue": 0,
//                        "endValue": 100,
//                        "radius": "80%",
//                        "innerRadius": "65%"
//                    }, {
//                        "color": "#fdd400",
//                        "startValue": 0,
//                        "endValue": 35,
//                        "radius": "80%",
//                        "innerRadius": "65%",
//                        "balloonText": "35%"
//                    }, {
//                        "color": "#eee",
//                        "startValue": 0,
//                        "endValue": 100,
//                        "radius": "60%",
//                        "innerRadius": "45%"
//                    }, {
//                        "color": "#cc4748",
//                        "startValue": 0,
//                        "endValue": 92,
//                        "radius": "60%",
//                        "innerRadius": "45%",
//                        "balloonText": "92%"
//                    }, {
//                        "color": "#eee",
//                        "startValue": 0,
//                        "endValue": 100,
//                        "radius": "40%",
//                        "innerRadius": "25%"
//                    }, {
//                        "color": "#67b7dc",
//                        "startValue": 0,
//                        "endValue": 68,
//                        "radius": "40%",
//                        "innerRadius": "25%",
//                        "balloonText": "68%"
//                    }]
//                }],
//                "allLabels": [{
//                    "text": "First option",
//                    "x": "49%",
//                    "y": "5%",
//                    "size": 15,
//                    "bold": true,
//                    "color": "#84b761",
//                    "align": "right"
//                }, {
//                    "text": "Second option",
//                    "x": "49%",
//                    "y": "15%",
//                    "size": 15,
//                    "bold": true,
//                    "color": "#fdd400",
//                    "align": "right"
//                }, {
//                    "text": "Third option",
//                    "x": "49%",
//                    "y": "24%",
//                    "size": 15,
//                    "bold": true,
//                    "color": "#cc4748",
//                    "align": "right"
//                }, {
//                    "text": "Fourth option",
//                    "x": "49%",
//                    "y": "33%",
//                    "size": 15,
//                    "bold": true,
//                    "color": "#67b7dc",
//                    "align": "right"
//                }],
//                "export": {
//                    "enabled": true
//                }
//            });
//        }

//        if ($(element).is("#angular_guage")) {
//            var gaugeChart = AmCharts.makeChart("angular_guage", {
//                "type": "gauge",
//                "theme": "light",
//                "axes": [{
//                    "axisThickness": 1,
//                    "axisAlpha": 0.2,
//                    "tickAlpha": 0.2,
//                    "valueInterval": 20,
//                    "bands": [{
//                        "color": "#84b761",
//                        "endValue": 90,
//                        "startValue": 0
//                    }, {
//                        "color": "#fdd400",
//                        "endValue": 130,
//                        "startValue": 90
//                    }, {
//                        "color": "#cc4748",
//                        "endValue": 220,
//                        "innerRadius": "95%",
//                        "startValue": 130
//                    }],
//                    "bottomText": "0 km/h",
//                    "bottomTextYOffset": -20,
//                    "endValue": 220
//                }],
//                "arrows": [{}],
//                "export": {
//                    "enabled": true
//                }
//            });

//            setInterval(randomValue, 2000);

//            // set random value
//            function randomValue() {
//                var value = Math.round(Math.random() * 200);
//                if (gaugeChart) {
//                    if (gaugeChart.arrows) {
//                        if (gaugeChart.arrows[0]) {
//                            if (gaugeChart.arrows[0].setValue) {
//                                gaugeChart.arrows[0].setValue(value);
//                                gaugeChart.axes[0].setBottomText(value + " km/h");
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        if ($(element).is("#map_with_curved_lines")) {
//            // svg path for target icon
//            var targetSVG = "M9,0C4.029,0,0,4.029,0,9s4.029,9,9,9s9-4.029,9-9S13.971,0,9,0z M9,15.93 c-3.83,0-6.93-3.1-6.93-6.93S5.17,2.07,9,2.07s6.93,3.1,6.93,6.93S12.83,15.93,9,15.93 M12.5,9c0,1.933-1.567,3.5-3.5,3.5S5.5,10.933,5.5,9S7.067,5.5,9,5.5 S12.5,7.067,12.5,9z";
//            // svg path for plane icon
//            var planeSVG = "M19.671,8.11l-2.777,2.777l-3.837-0.861c0.362-0.505,0.916-1.683,0.464-2.135c-0.518-0.517-1.979,0.278-2.305,0.604l-0.913,0.913L7.614,8.804l-2.021,2.021l2.232,1.061l-0.082,0.082l1.701,1.701l0.688-0.687l3.164,1.504L9.571,18.21H6.413l-1.137,1.138l3.6,0.948l1.83,1.83l0.947,3.598l1.137-1.137V21.43l3.725-3.725l1.504,3.164l-0.687,0.687l1.702,1.701l0.081-0.081l1.062,2.231l2.02-2.02l-0.604-2.689l0.912-0.912c0.326-0.326,1.121-1.789,0.604-2.306c-0.452-0.452-1.63,0.101-2.135,0.464l-0.861-3.838l2.777-2.777c0.947-0.947,3.599-4.862,2.62-5.839C24.533,4.512,20.618,7.163,19.671,8.11z";

//            var map = AmCharts.makeChart("map_with_curved_lines", {
//                "type": "map",
//                "theme": "light",
//                "dataProvider": {
//                    "map": "worldLow",
//                    "zoomLevel": 3.5,
//                    "zoomLongitude": -20.1341,
//                    "zoomLatitude": 49.1712,

//                    "lines": [{
//                        "latitudes": [51.5002, 50.4422],
//                        "longitudes": [-0.1262, 30.5367]
//                    }, {
//                        "latitudes": [51.5002, 46.9480],
//                        "longitudes": [-0.1262, 7.4481]
//                    }, {
//                        "latitudes": [51.5002, 59.3328],
//                        "longitudes": [-0.1262, 18.0645]
//                    }, {
//                        "latitudes": [51.5002, 40.4167],
//                        "longitudes": [-0.1262, -3.7033]
//                    }, {
//                        "latitudes": [51.5002, 46.0514],
//                        "longitudes": [-0.1262, 14.5060]
//                    }, {
//                        "latitudes": [51.5002, 48.2116],
//                        "longitudes": [-0.1262, 17.1547]
//                    }, {
//                        "latitudes": [51.5002, 44.8048],
//                        "longitudes": [-0.1262, 20.4781]
//                    }, {
//                        "latitudes": [51.5002, 55.7558],
//                        "longitudes": [-0.1262, 37.6176]
//                    }, {
//                        "latitudes": [51.5002, 38.7072],
//                        "longitudes": [-0.1262, -9.1355]
//                    }, {
//                        "latitudes": [51.5002, 54.6896],
//                        "longitudes": [-0.1262, 25.2799]
//                    }, {
//                        "latitudes": [51.5002, 64.1353],
//                        "longitudes": [-0.1262, -21.8952]
//                    }, {
//                        "latitudes": [51.5002, 40.4300],
//                        "longitudes": [-0.1262, -74.0000]
//                    }],
//                    "images": [{
//                        "id": "london",
//                        "svgPath": targetSVG,
//                        "title": "London",
//                        "latitude": 51.5002,
//                        "longitude": -0.1262,
//                        "scale": 1
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Brussels",
//                        "latitude": 50.8371,
//                        "longitude": 4.3676,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Prague",
//                        "latitude": 50.0878,
//                        "longitude": 14.4205,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Athens",
//                        "latitude": 37.9792,
//                        "longitude": 23.7166,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Reykjavik",
//                        "latitude": 64.1353,
//                        "longitude": -21.8952,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Dublin",
//                        "latitude": 53.3441,
//                        "longitude": -6.2675,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Oslo",
//                        "latitude": 59.9138,
//                        "longitude": 10.7387,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Lisbon",
//                        "latitude": 38.7072,
//                        "longitude": -9.1355,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Moscow",
//                        "latitude": 55.7558,
//                        "longitude": 37.6176,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Belgrade",
//                        "latitude": 44.8048,
//                        "longitude": 20.4781,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Bratislava",
//                        "latitude": 48.2116,
//                        "longitude": 17.1547,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Ljubljana",
//                        "latitude": 46.0514,
//                        "longitude": 14.5060,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Madrid",
//                        "latitude": 40.4167,
//                        "longitude": -3.7033,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Stockholm",
//                        "latitude": 59.3328,
//                        "longitude": 18.0645,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Bern",
//                        "latitude": 46.9480,
//                        "longitude": 7.4481,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Kiev",
//                        "latitude": 50.4422,
//                        "longitude": 30.5367,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "Paris",
//                        "latitude": 48.8567,
//                        "longitude": 2.3510,
//                        "scale": 0.5
//                    }, {
//                        "svgPath": targetSVG,
//                        "title": "New York",
//                        "latitude": 40.43,
//                        "longitude": -74,
//                        "scale": 0.5
//                    }]
//                },

//                "areasSettings": {
//                    "unlistedAreasColor": "#FFCC00",
//                    "unlistedAreasAlpha": 0.9
//                },

//                "imagesSettings": {
//                    "color": "#CC0000",
//                    "rollOverColor": "#CC0000",
//                    "selectedColor": "#000000"
//                },

//                "linesSettings": {
//                    "arc": -0.7, // this makes lines curved. Use value from -1 to 1
//                    "arrow": "middle",
//                    "color": "#CC0000",
//                    "alpha": 0.4,
//                    "arrowAlpha": 1,
//                    "arrowSize": 4
//                },
//                "zoomControl": {
//                    "gridHeight": 100,
//                    "draggerAlpha": 1,
//                    "gridAlpha": 0.2
//                },

//                "backgroundZoomsToTop": true,
//                "linesAboveImages": true,

//                "export": {
//                    "enabled": true
//                }
//            });
//        }

//        if ($(element).is("#animated_gauge")) {
//            var chart = AmCharts.makeChart("animated_gauge", {
//                "theme": "light",
//                "type": "gauge",
//                "axes": [{
//                    "topTextFontSize": 20,
//                    "topTextYOffset": 70,
//                    "axisColor": "#31d6ea",
//                    "axisThickness": 1,
//                    "endValue": 100,
//                    "gridInside": true,
//                    "inside": true,
//                    "radius": "50%",
//                    "valueInterval": 10,
//                    "tickColor": "#67b7dc",
//                    "startAngle": -90,
//                    "endAngle": 90,
//                    "unit": "%",
//                    "bandOutlineAlpha": 0,
//                    "bands": [{
//                        "color": "#0080ff",
//                        "endValue": 100,
//                        "innerRadius": "105%",
//                        "radius": "170%",
//                        "gradientRatio": [0.5, 0, -0.5],
//                        "startValue": 0
//                    }, {
//                        "color": "#3cd3a3",
//                        "endValue": 0,
//                        "innerRadius": "105%",
//                        "radius": "170%",
//                        "gradientRatio": [0.5, 0, -0.5],
//                        "startValue": 0
//                    }]
//                }],
//                "arrows": [{
//                    "alpha": 1,
//                    "innerRadius": "35%",
//                    "nailRadius": 0,
//                    "radius": "170%"
//                }]
//            });

//            setInterval(randomValue, 2000);

//            // set random value
//            function randomValue() {
//                var value = Math.round(Math.random() * 100);
//                chart.arrows[0].setValue(value);
//                chart.axes[0].setTopText(value + " %");
//                // adjust darker band to new value
//                chart.axes[0].bands[1].setEndValue(value);
//            }
//        }

//        if ($(element).is("#map_with_dynamic_pie_charts")) {
//            /**
//            * Create a map
//            */
//            var map = AmCharts.makeChart("map_with_dynamic_pie_charts", {
//                "type": "map",
//                "theme": "light",
//                "projection": "winkel3",

//                /**
//                * Data Provider
//                * The images contains pie chart information
//                * The handler for `positionChanged` event will take care
//                * of creating external elements, position them and create
//                * Pie chart instances in them
//                */
//                "dataProvider": {
//                    "map": "continentsLow",
//                    "images": [{
//                        "title": "North America",
//                        "latitude": 39.563353,
//                        "longitude": -99.316406,
//                        "width": 150,
//                        "height": 150,
//                        "pie": {
//                            "type": "pie",
//                            "pullOutRadius": 0,
//                            "labelRadius": 0,
//                            "dataProvider": [{
//                                "category": "Category #1",
//                                "value": 1200
//                            }, {
//                                "category": "Category #2",
//                                "value": 500
//                            }, {
//                                "category": "Category #3",
//                                "value": 765
//                            }, {
//                                "category": "Category #4",
//                                "value": 260
//                            }],
//                            "labelText": "[[value]]%",
//                            "valueField": "value",
//                            "titleField": "category"
//                        }
//                    }, {
//                        "title": "Europe",
//                        "latitude": 50.896104,
//                        "longitude": 19.160156,
//                        "width": 200,
//                        "height": 200,
//                        "pie": {
//                            "type": "pie",
//                            "pullOutRadius": 0,
//                            "labelRadius": 0,
//                            "radius": "10%",
//                            "dataProvider": [{
//                                "category": "Category #1",
//                                "value": 200
//                            }, {
//                                "category": "Category #2",
//                                "value": 600
//                            }, {
//                                "category": "Category #3",
//                                "value": 350
//                            }],
//                            "labelText": "",
//                            "valueField": "value",
//                            "titleField": "category"
//                        }
//                    }, {
//                        "title": "Asia",
//                        "latitude": 47.212106,
//                        "longitude": 103.183594,
//                        "width": 200,
//                        "height": 200,
//                        "pie": {
//                            "type": "pie",
//                            "pullOutRadius": 0,
//                            "labelRadius": 0,
//                            "radius": "10%",
//                            "dataProvider": [{
//                                "category": "Category #1",
//                                "value": 352
//                            }, {
//                                "category": "Category #2",
//                                "value": 266
//                            }, {
//                                "category": "Category #3",
//                                "value": 512
//                            }, {
//                                "category": "Category #4",
//                                "value": 199
//                            }],
//                            "labelText": "",
//                            "valueField": "value",
//                            "titleField": "category"
//                        }
//                    }, {
//                        "title": "Africa",
//                        "latitude": 11.081385,
//                        "longitude": 21.621094,
//                        "width": 200,
//                        "height": 200,
//                        "pie": {
//                            "type": "pie",
//                            "pullOutRadius": 0,
//                            "labelRadius": 0,
//                            "radius": "10%",
//                            "dataProvider": [{
//                                "category": "Category #1",
//                                "value": 200
//                            }, {
//                                "category": "Category #2",
//                                "value": 300
//                            }, {
//                                "category": "Category #3",
//                                "value": 599
//                            }, {
//                                "category": "Category #4",
//                                "value": 512
//                            }],
//                            "labelText": "",
//                            "valueField": "value",
//                            "titleField": "category"
//                        }
//                    }]
//                },

//                /**
//                * Add event to execute when the map is zoomed/moved
//                * It also is executed when the map first loads
//                */
//                "listeners": [{
//                    "event": "positionChanged",
//                    "method": updateCustomMarkers
//                }]
//            });

//            /**
//            * Creates and positions custom markers (pie charts)
//            */
//            function updateCustomMarkers(event) {
//                // get map object
//                var map = event.chart;

//                // go through all of the images
//                for (var x = 0; x < map.dataProvider.images.length; x++) {

//                    // get MapImage object
//                    var image = map.dataProvider.images[x];

//                    // Is it a Pie?
//                    if (image.pie === undefined) {
//                        continue;
//                    }

//                    // create id
//                    if (image.id === undefined) {
//                        image.id = "amcharts_pie_" + x;
//                    }
//                    // Add theme
//                    if ("undefined" == typeof image.pie.theme) {
//                        image.pie.theme = map.theme;
//                    }

//                    // check if it has corresponding HTML element
//                    if ("undefined" == typeof image.externalElement) {
//                        image.externalElement = createCustomMarker(image);
//                    }

//                    // reposition the element accoridng to coordinates
//                    var xy = map.coordinatesToStageXY(image.longitude, image.latitude);
//                    image.externalElement.style.top = xy.y + "px";
//                    image.externalElement.style.left = xy.x + "px";
//                    image.externalElement.style.marginTop = Math.round(image.height / -2) + "px";
//                    image.externalElement.style.marginLeft = Math.round(image.width / -2) + "px";
//                }
//            }

//            /**
//            * Creates a custom map marker - a div for container and a
//            * pie chart in it
//            */
//            function createCustomMarker(image) {

//                // Create chart container
//                var holder = document.createElement("div");
//                holder.id = image.id;
//                holder.title = image.title;
//                holder.style.position = "absolute";
//                holder.style.width = image.width + "px";
//                holder.style.height = image.height + "px";

//                // Append the chart container to the map container
//                image.chart.chartDiv.appendChild(holder);

//                // Create a pie chart
//                var chart = AmCharts.makeChart(image.id, image.pie);

//                return holder;
//            }
//        }
//    }
//    return {
//        restrict: "A",
//        link: link
//    };
//});




/* ===== Tags Dropdown Directive ==== */
airviewXcelerateApp.directive('tagsdropdown', function () {
    function link(scope, element, attrs) {
        $(element).dropdown({
            data: [
                {
                    disabled: false,
                    groupId: 1,
                    groupName: "Group Name",
                    id: 1,
                    name: "Edward Shirley Lee",
                    selected: false
                },
                {
                    disabled: false,
                    groupId: 1,
                    groupName: "Group Name",
                    id: 2,
                    name: "Dorothy Thomas Harris",
                    selected: false
                },
                {
                    disabled: false,
                    groupId: 2,
                    groupName: "Group Name",
                    id: 3,
                    name: "Angela Cynthia Anderson",
                    selected: false
                }
            ],
            limitCount: 40,
            multipleMode: 'label',
            choice: function () {
            }
        });
    }

    return {
        restrict: "A",
        link: link
    };
});



/* ===== Simple Tags Directive ==== */
airviewXcelerateApp.directive('simpletags', function () {
    function link(scope, element, attrs) {
        $(element).tagEditor({ initialTags: ['Hello', 'World', 'Example', 'Tags'], delimiter: ', ', placeholder: 'Enter tags ...' }).css('display', 'block').attr('readonly', true);
    }
    return {
        restrict: "A",
        link: link
    };
});



/* ===== Multiselect Directive ==== */
airviewXcelerateApp.directive('multiselect', function () {
    function link(scope, element, attrs) {
        $(element).multiselect({
            enableFiltering: true,
            enableClickableOptGroups: true,
            includeSelectAllOption: true
        });
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Date Range Pickers ==== */
airviewXcelerateApp.directive('datepicker', function () {
    function link(scope, element, attrs) {
        var start = moment().subtract(29, 'days');
        var end = moment();

        $(element).find('span').html(start.format('MM/D/YY') + ' - ' + end.format('MM/D/YY'));
        function cb(start, end) {
            $(this.element).find('span').html(start.format('MM/D/YY') + ' - ' + end.format('MM/D/YY'));
        }

        if ($(element).hasClass("openright")) {
            $(element).daterangepicker({
                startDate: start,
                endDate: end,
                opens: 'right',
                ranges: {
                    'Today': [moment(), moment()],
                    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                }
            }, cb);
        }
        else {
            $(element).daterangepicker({
                startDate: start,
                endDate: end,
                opens: 'left',
                ranges: {
                    'Today': [moment(), moment()],
                    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                    'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                }
            }, cb);
        }

        cb(start, end);
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Date Dropper Directive ==== */
airviewXcelerateApp.directive('datedropper', function () {
    function link(scope, element, attrs) {
        $(element).dateDropper();
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Date Format Directive ==== */
airviewXcelerateApp.directive('dateformat', function () {
    function link(scope, element, attrs) {
        $(element).bootstrapMaterialDatePicker({ format: 'dddd DD MMMM YYYY - HH:mm' });
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Step Form Directive ==== */
airviewXcelerateApp.directive('stepform', function () {
    function link(scope, element, attrs) {
        var $validator = $(element).parent(".commentForm").validate({
            rules: {
                emailfield: {
                    required: true,
                    email: true,
                    minlength: 3
                },
                namefield: {
                    required: true,
                    minlength: 3
                },
                urlfield: {
                    required: true,
                    minlength: 3,
                    url: true
                },
                password: {
                    required: true,
                    minlength: 3
                },
                number: {
                    number: true,
                    minlength: 10
                }
            },
            messages: {
                emailfield: {
                    required: "This is must required",
                    email: "Please enter correct email format"
                },
            }
        });

        $(element).bootstrapWizard({
            'tabClass': 'nav nav-pills',
            'onNext': function (tab, navigation, index) {
                var $valid = $(element).parent(".commentForm").valid();
                if (!$valid) {
                    $validator.focusInvalid();
                    return false;
                }
                $(element).find('.stepforms-selectors li.disabled:first').removeClass('disabled');
            },
            onTabShow: function (tab, navigation, index) {
                var $total = navigation.find('li').length;
                var $current = index + 1;
                var $percent = ($current / $total) * 100;
                $(element).find(".progress-bar").css({ width: $percent + '%' })
            }
        });



        $(element).find(".finish").click(function () {
            alert('Finished!, Starting over!');
            $(element).find(".stepforms-selectors li:first-child a").trigger('click');
        });

    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Masked Input Directive ==== */
airviewXcelerateApp.directive('maskedinput', function () {
    function link(scope, element, attrs) {
        if ($(element).hasClass('date')) { $(element).mask('00/00/0000'); }
        if ($(element).hasClass('time')) { $(element).mask('00:00:00'); }
        if ($(element).hasClass('date_time')) { $(element).mask('00/00/0000 00:00:00'); }
        if ($(element).hasClass('money')) { $(element).mask('#.##0,00', { reverse: true }); }
        if ($(element).hasClass('phone')) { $(element).mask('0000-0000'); }
        if ($(element).hasClass('phone_with_ddd')) { $(element).mask('(00) 0000-0000'); }
        if ($(element).hasClass('phone_us')) { $(element).mask('(000) 000-0000'); }
        if ($(element).hasClass('mixed')) { $(element).mask('AAA 000-S0S'); }
        if ($(element).hasClass('ip_address')) { $(element).mask('099.099.099.099'); }
        if ($(element).hasClass('percent')) { $(element).mask('##0,00%', { reverse: true }); }
        if ($(element).hasClass('placeholder')) { $(element).mask("00/00/0000", { placeholder: "__/__/____" }); }
        if ($(element).hasClass('clear-if-not-match')) { $(element).mask("00/00/0000", { clearIfNotMatch: true }); }
    }
    return {
        restrict: "A",
        link: link
    };
});



/* ===== Form Validator ==== */
airviewXcelerateApp.directive('formvalidator', function () {
    function link(scope, element, attrs) {
        $(element).validator()
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Searchable List ==== */
airviewXcelerateApp.directive('searchablelist', function () {
    function link(scope, element, attrs) {
        function myFunction() {
            var ul = $(".searchable-list");
            var li = $(".searchable-list li");
            var input = $(element);
            var filter = input.val().toUpperCase();

            for (i = 0; i < li.length; i++) {
                a = li[i].getElementsByTagName("a")[0];
                if (a.innerHTML.toUpperCase().indexOf(filter) > -1) {
                    li[i].style.display = "";
                } else {
                    li[i].style.display = "none";

                }
            }
        }

        $(element).on("keyup", function () {
            myFunction();
        });
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Carousel Gallery ==== */
airviewXcelerateApp.directive('carouselgallery', function () {
    function link(scope, element, attrs) {
        $(element).slick({
            slidesToShow: 3,
            slidesToScroll: 1,
            autoplay: true,
            arrows: false,
            dots: false
        });
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Slider Gallery ==== */
airviewXcelerateApp.directive('slidergallery', function () {
    function link(scope, element, attrs) {
        $(element).slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            autoplay: true,
            fade: true,
            dots: false
        });
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Tree View ==== */
airviewXcelerateApp.directive('jstree', function () {
    function link(scope, element, attrs) {
        $(element).jstree({

            plugins: ["checkbox", "types"],
            core: {
                expand_selected_onload: false,
                themes: {
                    responsive: !1
                },
                data: [

                    {
                        "id": "ajson2", "parent": "#", "text": "Root node 2", "icon": 'fas fa-database',
                    },
                    { "id": "ajson3", "parent": "ajson2", "text": "Child 1", "icon": "fa fa-table" },
                    { "id": "ajson4", "parent": "ajson2", "text": "Child 2", "icon": "fa fa-table" },
                    { "id": "ajson5", "parent": "ajson3", "text": "Grand Child 1", "icon": "fa fa-columns" },
                    { "id": "ajson6", "parent": "ajson3", "text": "Grand Child 2", "icon": "fa fa-columns" },
                    { "id": "ajson7", "parent": "ajson4", "text": "Second Grand Child 1", "icon": "fa fa-columns" },
                    { "id": "ajson8", "parent": "ajson4", "text": "Second Grand Child 2", "icon": "fa fa-columns" },

                ],

            },
            types: {
                "default": {
                    icon: "fa fa-file"
                },
                file: {
                    icon: "fa fa-file"
                }
            }, checkbox: {
                three_state: false, // to avoid that fact that checking a node also check others
                whole_node: false,  // to avoid checking the box just clicking the node 
                //tie_selection: false // for checking without selecting and selecting without checking
            }
        });
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Heirachy Chart ==== */
airviewXcelerateApp.directive('heirachy', function () {
    function link(scope, element, attrs) {
        new Treant(chart_config);
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Nestable ==== */
airviewXcelerateApp.directive('nestable', function () {
    function link(scope, element, attrs) {
        $('#nestable').nestable({
            group: 1
        });
        $('#nestable2').nestable({
            group: 1
        });
    }
    return {
        restrict: "A",
        link: link
    };
});

/* ===== Text Editor ==== */
airviewXcelerateApp.directive('texteditor', function () {
    function link(scope, element, attrs) {
        $(element).editable({ inlineMode: false, autosave: true })
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Color Picker ==== */
airviewXcelerateApp.directive('colorpicker', function () {
    function link(scope, element, attrs) {
        $(element).farbtastic($(element).parent(".colorpickerwrapper").prev("input"));

        function isDark(color) {

            var match = /rgb\((\d+).*?(\d+).*?(\d+)\)/.exec(color);
            return (match[1] & 255)
                + (match[2] & 255)
                + (match[3] & 255)
                < 4 * 256 / 2;
        }
        $(element).parents(".widget").find(".title-bar .panel-color input").on("change", function () {

            $(this).parents(".widget").find(".title-bar").css("color", isDark($(this).css("background-color")) ? '#fff' : '#4e4e4e');

        });

        $(element).parents(".widget").find(".title-bar .panel-color-widget-in input").on("change", function () {

            $(this).parents(".widget").find(".content-wrapper").css("color", isDark($(this).css("background-color")) ? '#fff' : '#4e4e4e');

        });

        $(element).parents(".widget-settings").find("input.bgcolor").on("change", function () {
            var data_id = $(this).parents('.widget-settings').attr('data-id');
            $('#' + data_id).find('table thead').css("color", isDark($(this).css("background-color")) ? '#fff' : '#4e4e4e');

        });
    }
    return {
        restrict: "A",
        link: link
    };
});




/* ===== Multiple Emails ==== */
airviewXcelerateApp.directive('multipleemails', function () {
    function link(scope, element, attrs) {
        $(element).multiple_emails({ theme: "SemanticUI" });
    }
    return {
        restrict: "A",
        link: link
    };
});



/* ===== Sparkline ==== */
airviewXcelerateApp.directive('sparkline', function () {
    function link(scope, element, attrs) {
        $(element).sparkline('html', {
            enableTagOptions: true,
            disableHiddenCheck: true
        });
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Range Slider ==== */
airviewXcelerateApp.directive('rangeslider', function () {
    function link(scope, element, attrs) {
        $(element).ionRangeSlider({
            min: attrs.ionmin,
            max: attrs.ionmax,
            from: attrs.ionfrom,
            to: attrs.ionto,
            prefix: attrs.ionprefix,
            type: attrs.iontype,
            grid: attrs.iongrid,
            step: attrs.ionstep,
            values: attrs.ionvalues ? JSON.parse(attrs.ionvalues) : '',
            prettify_enabled: attrs.ionprettify,
            prettify_separator: attrs.ionseparator,
            postfix: attrs.ionpostfix,
            decorate_both: attrs.iondecorate_both ? JSON.parse(attrs.ionvalues) : '',
            hide_min_max: attrs.hide_min_max ? JSON.parse(attrs.ionvalues) : '',
            hide_from_to: attrs.hide_from_to ? JSON.parse(attrs.ionvalues) : ''

        });
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Select 2 ==== */
airviewXcelerateApp.directive('select2', function () {
    function link(scope, element, attrs) {
        $(element).select2();
    }
    return {
        restrict: "A",
        link: link
    };
});

/* ===== Switch ==== */
airviewXcelerateApp.directive('switch', function () {
    function link(scope, element, attrs) {
        $(element).mSwitch();
    }
    return {
        restrict: "A",
        link: link
    };
});

/* ===== Toggles ==== */
airviewXcelerateApp.directive('aircodaccordion', function () {
    function link(scope, element, attrs) {
        $(element).each(function () {
            $(this).find('.content').hide();
            //$(this).find('h2:first').addClass('active').next().slideDown(500).parent().addClass("activate");
        });
    }
    return {
        restrict: "A",
        link: link
    };
});

/* ===== Dynamic Tree ==== */
airviewXcelerateApp.directive('dynatree', function () {
    function link(scope, element, attrs) {
        $(element).dynatree({
            persist: true,
            checkbox: true
        });
    }
    xl_script.dynatree();
    return {
        restrict: "A",
        link: link
    };
});
airviewXcelerateApp.filter('propsFilter', function () {
    return function (items, props) {
        var out = [];

        if (angular.isArray(items)) {
            var keys = Object.keys(props);

            items.forEach(function (item) {
                var itemMatches = false;

                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }

                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }

        return out;
    };
});
/* ===== Query Builder ==== */
airviewXcelerateApp.directive('sidepanel', function () {
    function link(scope, element, attrs) {
        xl_script.sidepanelfunctions();
    }
    return {
        restrict: "A",
        link: link
    };
});





/* ===== Dynamic Tabs ==== */
airviewXcelerateApp.directive('dynamictabs', function () {
    function link(scope, element, attrs) {
        xl_script.dynamic_tabs(element);
    }
    return {
        restrict: "A",
        link: link
    };
});


/* ===== Workflow Automation ==== */
airviewXcelerateApp.controller('workflowcontroller', function ($scope, $compile, toastr) {

    $scope.$on('$stateChangeSuccess', function () {

        setTimeout(function () {

            if ($("body").find("div[canvascharts]")) {
                xl_script.CanvasChart();
            }
            //xl_script.murri();
            xl_script.grid_stack();
        }, 0)
    });


    $(".widget").on("webkitTransitionEnd transitionend oTransitionEnd", function (event) {
        document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
    });




    // Simple Compile Function Used For WorkFlow Automation Popup Screen    
    $scope.simpleCompile = function (element, parentElement) {
        var compiledcode = $.parseHTML(element);
        var compiledcode = $compile(compiledcode)($scope);
        $compile($(parentElement).append(compiledcode))($scope);
    }


    $scope.afterCompile = function (element, parentElement) {
        var compiledcode = $.parseHTML(element);
        compiledcode = $compile(compiledcode)($scope);
        $(parentElement).after(compiledcode)
        $compile($(parentElement))($scope);
    }

    $scope.showToastr = function (contents, settings) {
        let defaultSettings = { action: 'success' };
        $.extend(defaultSettings, settings);

        switch (defaultSettings.action) {
            case 'success':
                toastr.success(contents.message, contents.title);
                break;
            case 'error':
                toastr.error(contents.message, contents.title);
                break;
            case 'info':
                toastr.info(contents.message, contents.title);
                break;
            case 'warning':
                toastr.warning(contents.message, contents.title);
                break;
            default:
                toastr.success(contents.message, contents.title);
        }
    }

});



/* ===== Query Builder ==== */
//airviewXcelerateApp.directive('querybuilder', function () {
//    function link(scope, element, attrs) {
//        //xl_script.querybuilder();
//        //$(element).find('#builder').queryBuilder({
//        //    filters:
//        //        [
//        //            /* * string with separator */
//        //            {
//        //                id: 'name',
//        //                field: 'username',
//        //                label: {
//        //                    en: 'Name',
//        //                    fr: 'Nom'
//        //                },
//        //                icon: 'glyphicon glyphicon-user',
//        //                value_separator: ',',
//        //                type: 'string',
//        //                optgroup: 'core',
//        //                default_value: 'Mistic',
//        //                size: 30,
//        //                validation: {
//        //                    allow_empty_value: true
//        //                },
//        //                unique: true
//        //            }
//        //        ]
//        //});
//    }
//    return {
//        restrict: "A",
//        link: link
//    };
//});
