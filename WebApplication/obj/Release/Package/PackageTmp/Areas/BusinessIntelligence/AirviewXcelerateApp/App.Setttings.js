
//#region Global Variable
var getStateSettings = {};
var getWidgetSettings = [];
var getColumnsSize = {};
var getUiColumnInSettingOfDefaultPage = "";
var getUiColumnInAdvanceSettingOfDefaultPage = "";
var getImageControlInAdvanceSettingOfDefaultPage = "";
var createDynamicDiv;
var rootUrl = "http://localhost:4064";
mxBasePath = 'Areas/BusinessIntelligence/assets/mxgraph/src';
var mxGraphEncoderOrDecoder;
var addWidgetOfGridStackItem = {};
var getRowsAndColumnsDefaultPage = "";
var gridInfo = "";
var deleteChildTabWidget = [];
// add variable for temporary work
var isModeForTab = false;
var mainPartialViewType = 'widgetOfPredefined';
var shutterType = { down: 'shtrDown', up: 'shtrUp' };
var currentShutterType = shutterType.down;
var remoteDataSourceinfo = {};
remoteDataSourceinfo['dataSourceName'] = "AIRVIEW";
remoteDataSourceinfo['remoteUserName'] = "sa";
remoteDataSourceinfo['remotePassword'] = "Yarabbi8381!";
remoteDataSourceinfo['remoteDatabase'] = "AirViewX1_DEV_Templates";
remoteDataSourceinfo['remoteProductName'] = "Sql Server";
remoteDataSourceinfo['remoteDatabaseWhereClause'] = ""
//#endregion

//#region airview and Xcelerate App Settings Configuration
var airviewXcelerateAppSettings = function () {
    'use strict';
    //#region State Configurations
    var handleStateSettings = function () {
        var saveStateSettings = {};

        //#region All states Settings

        //#region Login
        saveStateSettings['Login'] = {
            name: "Login",
            url: '/Login',
            //  abstract: true,
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/404.cshtml',
                    controller: "mainController"
                }
            }
        };
        //#endregion

        //#region MainDashboardSetting
        saveStateSettings['Main'] = {
            // abstract: true,
            name: "Main",
            url: '/Xcelerate/:userID',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/Shared/DashboardSetting.cshtml',
                    controller: "mainController"

                }
                ,
                'header@Main': {
                    templateUrl: '~/AirviewXcelerateApp/Views/header.cshtml',
                    controller: 'header as vm'

                },
                'sideheader@Main': {
                    templateUrl: '~/AirviewXcelerateApp/Views/sideheader.cshtml',
                    controller: 'sideheader as vm1'
                },
                'popups@Main': {
                    templateUrl: '~/AirviewXcelerateApp/Views/popups.cshtml'

                }




            }
        };
        //#endregion

        //#region Dashboard one
        saveStateSettings['Dashboard'] = {

            name: "Main.Dashboard",
            url: '/Dashboard',
            views: {

                '': {

                    templateUrl: '~/AirviewXcelerateApp/Views/dashboard.cshtml',
                    controller: 'dashboardController as vm'
                },
                'information-boxes@Main.Dashboard': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/information-boxes.cshtml',
                    controller: 'main-boxes',
                    resolve: {
                        messages: function (getTemplate) {

                            return getTemplate.doSomethingWithModel();
                        }
                    }
                }
                ,
                'program-view@Main.Dashboard': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/program-view.cshtml',
                    controller: "PV"
                }
                ,
                'regional-view@Main.Dashboard': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/regional-view.cshtml',
                    controller: "RV"
                }
                ,
                'issues@Main.Dashboard': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/issues.cshtml',
                    controller: "issues"
                }
                ,
                'map@Main.Dashboard': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/map.cshtml',
                    controller: "map"

                }
                ,
                'calendar@Main.Dashboard': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/calendar.cshtml'

                },
                'TopMenu@Main': {
                    templateUrl: '~/AirviewXcelerateApp/Views/TopSections/TopSection_Dashbaord.cshtml'

                }



            }
        };
        //#endregion

        //#region Dashboard And Report

        saveStateSettings['DashboardAndReport'] = {
            name: "Main.DashboardAndReport",
            url: '/DashboardAndReport',
            views: {

                '': {

                    templateUrl: '~/AirviewXcelerateApp/Views/DashboardAndReport/design-dashboard-and-report.cshtml',
                    controller: 'dashboardAndReportController as vm'
                }
                ,
                'TopMenu@Main': {
                    templateUrl: '~/AirviewXcelerateApp/Views/TopSections/TopSection_DashboardAndReport.cshtml',
                    controller: 'TopSectionDashboardAndReport as vm'

                }


            }
        };
        //#endregion

        //#region only Document Viewer

        saveStateSettings['DocumentViewer'] = {
            name: "Main.DocumentViewer",
            url: '/DocumentViewer',
            views: {

                '': {

                    templateUrl: '/Xcelerate/DashboardNReport/DocumentViewer'

                }



            }
        };
        //#endregion


        //#region Dashboard Two
        saveStateSettings['Dashboard1'] = {
            name: "Main.Dashboard1",
            url: '/Dashboard1',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/dashboard1.cshtml',
                    controller: 'dashboardController as vm'
                },
                'information-boxes@Main.Dashboard1': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/information-boxes.cshtml',
                    controller: 'main-boxes',
                    resolve: {
                        messages: function () {
                            return getMessage();
                        }
                    }
                }
                ,
                'tracking@Main.Dashboard1': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/tracking.cshtml'

                }
                ,
                'regional-view@Main.Dashboard1': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/regional-view.cshtml',
                    controller: 'RV'
                }
                ,
                'issues@Main.Dashboard1': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/issues.cshtml'

                }
                ,
                'map@Main.Dashboard1': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/map.cshtml',
                    controller: 'map'
                }
                ,
                'donut-boxes@Main.Dashboard1': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/donut-boxes.cshtml'

                },
                'progressbars@Main.Dashboard1': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/progressbars.cshtml'

                },
                'sites-table@Main.Dashboard1': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/sites-table.cshtml'

                },
                'issues-table@Main.Dashboard1': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/issues-table.cshtml',
                    controller: 'issues-table'
                }



            }
        };
        //#endregion

        //#region Alert
        saveStateSettings['alerts'] = {
            name: "Main.alerts",
            url: '/alerts',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/alerts.cshtml',
                    controller: 'grids'
                }




            }
        };
        //#endregion

        //#region workflow
        saveStateSettings['workflow'] = {
            name: "Main.workflow",
            url: '/workflow',
            views: {
                '': {
                    templateUrl: '~/Areas/WorkflowAutomation/Workflow.cshtml',
                    controller: 'grids'
                }




            }
        };
        //#endregion

        //#region formbuilder
        saveStateSettings['formbuilder'] = {
            name: "Main.formbuilder",
            url: '/formbuilder',
            views: {
                '': {
                    templateUrl: '~/Areas/WorkflowAutomation/formbuilder.cshtml',
                    controller: 'grids'
                }




            }
        };
        //#endregion

        //#region Modeler
        saveStateSettings['modeler'] = {
            name: "Main.modeler",
            url: '/modeler',
            views: {
                '': {
                    templateUrl: '~/Areas/WorkflowAutomation/modeler.cshtml',
                    controller: 'grids'
                }
            }
        };
        //#endregion

        //#region Dashboard three
        saveStateSettings['Dashboard2'] = {
            name: "Main.Dashboard2",
            url: '/dashboard/dashboard2/',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/dashboard2.cshtml',
                    controller: 'dashboardController'
                }
                ,
                'information-boxes2@Main.Dashboard2': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/information-boxes2.cshtml',
                    controller: 'dashboardController'
                },
                'map2@Main.Dashboard2': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/map2.cshtml',
                    controller: 'dashboardController'
                }



            }
        };
        //#endregion

        //#region  High Charts
        saveStateSettings['Charts'] = {
            name: "Charts",
            url: '/charts/highcharts',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/charts.cshtml',
                    controller: 'grids'
                }
                ,
                'chart1@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart1.cshtml',
                    controller: 'grids'
                },
                'chart2@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart2.cshtml',
                    controller: 'grids'
                }
                ,
                'chart3@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart3.cshtml',
                    controller: 'grids'
                }
                ,
                'chart4@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart4.cshtml',
                    controller: 'grids'
                }
                ,
                'chart5@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart5.cshtml',
                    controller: 'grids'
                }
                ,
                'chart6@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart6.cshtml',
                    controller: 'grids'
                }
                ,
                'chart7@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart7.cshtml',
                    controller: 'grids'
                }
                ,
                'chart8@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart8.cshtml',
                    controller: 'grids'
                }
                ,
                'chart9@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart9.cshtml',
                    controller: 'grids'
                }
                ,
                'chart10@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart10.cshtml',
                    controller: 'grids'
                }
                ,
                'chart12@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart12.cshtml',
                    controller: 'grids'
                }
                ,
                'chart11@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart11.cshtml',
                    controller: 'grids'
                }
                ,
                'chart13@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart13.cshtml',
                    controller: 'grids'
                }
                ,
                'chart14@Charts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/chart14.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Canvas Charts
        saveStateSettings['Canvascharts'] = {
            name: "Canvascharts",
            url: '/charts/canvascharts',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/canvas-charts.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart1@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart1.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart2@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart2.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart3@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart3.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart4@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart4.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart5@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart5.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart6@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart6.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart7@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart7.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart8@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart8.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart9@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart9.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart10@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart10.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart11@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart11.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart12@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart12.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart13@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart13.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart14@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart14.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart15@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart15.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart16@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart16.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart17@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart17.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart18@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart18.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart19@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart19.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart20@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart20.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart21@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart21.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart22@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart22.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart23@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart23.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart24@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart24.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart25@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart25.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart26@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart26.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart27@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart27.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart28@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart28.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart29@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart29.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart30@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart30.cshtml',
                    controller: 'grids'
                }
                ,
                'canvas-chart31@Canvascharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/canvas-chart31.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Am Charts
        saveStateSettings['Amcharts'] = {
            name: "Amcharts",
            url: '/charts/amcharts',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/am-charts.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart1@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart1.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart2@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart2.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart3@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart3.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart4@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart4.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart5@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart5.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart6@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart6.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart7@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart7.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart8@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart8.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart9@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart9.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart10@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart10.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart11@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart11.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart12@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart12.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart13@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart13.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart14@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart14.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart15@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart15.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart16@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart16.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart17@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart17.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart18@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart18.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart19@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart19.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart20@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart20.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart21@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart21.cshtml',
                    controller: 'grids'
                }
                ,
                'am-chart22@Amcharts': {
                    templateUrl: '~/AirviewXcelerateApp/Views/widgets/am-chart22.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region sparkline Charts
        saveStateSettings['Sparkline'] = {
            name: "Sparkline",
            url: '/charts/sparkline',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/sparkline-chart.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region forms 
        saveStateSettings['Forms'] = {
            name: "Forms",
            url: '/forms',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/forms.cshtml',
                    controller: 'grids'
                }


            }
        };
        //#endregion

        //#region Tables
        saveStateSettings['Tables'] = {
            name: "Tables",
            url: '/tables',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/tables.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Profile
        saveStateSettings['Profile'] = {
            name: "Profile",
            url: '/elements/profile',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/profile.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region buttons
        saveStateSettings['Buttons'] = {
            name: "Buttons",
            url: '/elements/buttons',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/buttons.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region gallery
        saveStateSettings['Gallery'] = {
            name: "Gallery",
            url: '/elements/gallery',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/gallery.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Tree
        saveStateSettings['Tree'] = {
            name: "Tree",
            url: '/elements/tree',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/tree.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region nestable
        saveStateSettings['Nestable'] = {
            name: "Nestable",
            url: '/elements/nestable',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/nestable.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region ProgressBar
        saveStateSettings['ProgressBar'] = {
            name: "ProgressBar",
            url: '/elements/progressbars',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/progressbars.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Tabs
        saveStateSettings['Tabs'] = {
            name: "Tabs",
            url: '/elements/tabs',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/tabs.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Maps
        saveStateSettings['Maps'] = {
            name: "Maps",
            url: '/elements/maps',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/maps.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Boxes
        saveStateSettings['Boxes'] = {
            name: "Boxes",
            url: '/elements/boxes',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/boxes.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Editors
        saveStateSettings['Editors'] = {
            name: "Editors",
            url: '/elements/editor',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/editor.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Canlender
        saveStateSettings['Canlender'] = {
            name: "Canlender",
            url: '/elements/calendar',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/calendar.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Chat
        saveStateSettings['Chat'] = {
            name: "Chat",
            url: '/elements/chat',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/chat.cshtml',
                    controller: 'grids'
                }


            }
        };
        //#endregion

        //#region Timeline
        saveStateSettings['Timeline'] = {
            name: "Timeline",
            url: '/elements/timeline',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/timeline.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Range
        saveStateSettings['Range'] = {
            name: "Range",
            url: '/elements/range',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/range-sliders.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Toggle
        saveStateSettings['Toggle'] = {
            name: "Toggle",
            url: '/elements/toggles',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/toggles.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Views
        saveStateSettings['Views'] = {
            name: "Views",
            url: '/elements/views',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/views.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Inbox
        saveStateSettings['Inbox'] = {
            name: "Inbox",
            url: '/email/inbox',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/inbox.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region ComposeEmail
        saveStateSettings['ComposeEmail'] = {
            name: "ComposeEmail",
            url: '/email/compose-mail',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/compose-mail.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region EmailOpen
        saveStateSettings['EmailOpen'] = {
            name: "EmailOpen",
            url: '/email/mail-open',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/mail-open.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region TestSettings
        saveStateSettings['TestSettings'] = {
            name: "Main.TestSettings",
            url: '/dashboard/testsettings',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/airview/testsettings.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region SetupDefination
        saveStateSettings['SetupDefination'] = {
            name: "Main.SetupDefination",
            url: '/dashboard/setupdefinition',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/airview/setupdefinition.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region MarketConfiguration
        saveStateSettings['MarketConfiguration'] = {
            name: "Main.MarketConfiguration",
            url: '/dashboard/marketconfiguration',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/airview/market-configuration.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region WorkOrder
        saveStateSettings['WorkOrder'] = {
            name: "Main.WorkOrder",
            url: '/dashboard/workorder',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/airview/workorder.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region WoStatus
        saveStateSettings['WoStatus'] = {
            name: "Main.WoStatus",
            url: '/dashboard/wostatus',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/airview/WOstatus.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Permission
        saveStateSettings['Permission'] = {
            name: "Main.Permission",
            url: '/dashboard/permissions',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/airview/permissions.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#region Error404
        saveStateSettings['Error404'] = {
            name: "Main.Error404",
            url: '/404',
            views: {
                '': {
                    templateUrl: '~/AirviewXcelerateApp/Views/404.cshtml',
                    controller: 'grids'
                }



            }
        };
        //#endregion

        //#endregion

        return saveStateSettings;
    };
    //#endregion

    //#region widget Configurtion
    var handleWidgetSettings = function () {
        let widgetSettingJsonForm = [{
            "properties": "",
            "type": "widgetOfTable",
            "code": `<div id=-1 data-position='1' data-widgetIndex="{{chngeIdx}}" class="grid-stack-item" data-gs-resize-handles="e,se,s">
    <div class="grid-stack-item-content">
        <div class='widget'>
            <div class='title-bar' style='background:{{titlebg_01}};'>
                <h3><i class='fa fa-table'></i> <span class='update-name'>{{widgetname_01}}</span> <span class='cache-value'></span> <input ng-model='widgetname_01' type='text' /> </h3>
                <div class='buttons-sets'>
                    <div class="searchdiv" style="display:none;"><input type="text" class="form-control txtsearch1" placeholder="Search..." /></div>  <a class='custom-btn icon-only collapse-btn '><i class='fa fa-minus'></i><span class='custom-tooltip'>Collapse</span></a> <a ng-show="vm.modeType=='edit'" class='custom-btn icon-only opt-btn '><i class='fa fa-columns'></i><span class='custom-tooltip'>Panel Color</span></a> <ul class='opt-dropdown '> <li class='has-children'><a href="javascript:void(0);" title=''>Panel Color</a> <ul class='panel-color'> <li> <a href="javascript:void(0);" title=''><input class='bgcolor' type='text' value='#fff' ng-model='titlebg_01' /> <div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul> </li> </ul><a ng-if="vm.dashboardAndReportComInformation.widgetList[chngeIdx].data.length>0" class='custom-btn icon-only open-filters '><i class='fa fa-filter'></i><span class='custom-tooltip'>Filters</span></a>

                    <div class="filters-drop">
                        <h5>Filters</h5>
                        <div ivh-treeview="vm.dashboardAndReportComInformation.widgetList[chngeIdx].columnValueForFilteration"
                             ivh-treeview-on-cb-change="vm.changeNodeOnCheckBox(ivhNode, ivhIsSelected, ivhTree,chngeIdx)" ivh-treeview-options="vm.customOpts">
                        </div>
                        <div class="drop-bottom">
                            <button ng-click="vm.applyFilter(chngeIdx)" class="custom-btn color1 applyFilter">Apply</button>
                        </div>
                    </div> <a ng-show="vm.modeType=='edit'" ng-click="vm.makeCloneOfWidget(chngeIdx)" class="custom-btn icon-only opt-btn hide-clone-in-tab"><i class="fa fa-copy"></i><span class="custom-tooltip">Make Clone</span></a>  <a ng-show="vm.modeType=='edit'" class='custom-btn icon-only remove-btn '><i class='fa fa-times'></i><span class='custom-tooltip'>Remove</span></a>
                </div>
            </div><div class="overlay" style="display:none;"><div class="overlay__inner"><div class="overlay__content"><span class="spinner"></span></div></div></div> <div class='content-wrapper table-padding-remove'>  <div class="center-icon"> <i class="fa fa-table"></i>  </div>  </div>
        </div>
    </div>
</div>`,
            "settings": `<div class="toggle-item widget-settings"> <h2><i class="fa fa-cog"></i> Settings</h2> <div class="content" style="display: none" id="settings"><form> <div class="row"> <div class="col-lg-6 col-md-12"style="display:none"> <label> <input ng-true-value=true ng-false-value=false  class="paginationInput" type="checkbox" />  Pagination</label> </div><div class="col-lg-6 col-md-12"><label><input  ng-true-value="''" ng-false-value="'no-tableheaders'"  class="headerInput" type="checkbox" />  Header</label></div> <div class="col-lg-6 col-md-12"> <label><input ng-true-value="'no-borders'" ng-false-value="''"  class="borderInput" type="checkbox" />  Borders</label> </div> <div class="col-lg-6 col-md-12"> <label style="display:none;"><input class="strippedInput" name="checkbox2" value="stripped"  type="checkbox" />  Stripped</label> </div> <div class="col-lg-12 col-md-12 sampleStyle"> <label class="label">Style</label> <label class="radio"> <input type="radio" value="" name="radio" checked="checked"> <i></i>Simple</label> <label class="radio"> <input type="radio" value="dark" name="radio"> <i></i>Dark</label> <label class="radio"> <input type="radio" value="coloured" name="radio"> <i></i>Coloured</label> </div> </div> </form></div> </div> `,
            "advanced_settings": `<div class="toggle-item widget-settings"> <h2><i class="fa fa-cog"></i> Advanced Settings</h2> <div class="content" style="display: none" id="settings"><form> <div class="row"> <div class="col-lg-12 col-md-12"> <label>Headings Font Size In px</label> <input class="theme4 theadFontSize"  ionmin="8" ionmax="24" ionfrom="8"  rangeslider /> </div> <div class="col-lg-12 col-md-12"> <label>Rows Font Size In px</label> <input class="theme4 trowFontSize" ionmin="8" ionmax="18" ionfrom="8"  rangeslider /> </div> <div class="col-lg-12 col-md-12 "> <label>color of headings</label> <input class="bgcolor"  type="text"  value="#fafafa"  /> <div class="colorpickerwrapper"><div colorpicker></div></div> </div> </div> </form></div></div>`,
            "uiGrid": `<div ui-grid="" class="grid" ui-grid-resize-columns ui-grid-pagination ui-grid-selection  ui-grid-auto-resize></div> `,
            "tableStandardFormat": `<div class="table-container" ><table class="table"><thead><tr><th ng-repeat="(header, value) in vm.data[0]">{{header}}</th> </tr></thead><tbody><tr ng-repeat="row in vm.data"><td ng-repeat="cell in row">{{cell}}</td></tr></tbody></table></div>
`
        },
        {
            "type": "widgetOfChart",
            "code": `<div class="grid-stack-item" data-widgetIndex="{{chngeIdx}}" data-gs-resize-handles="e,se,s">
    <div class="grid-stack-item-content">
        <div class='widget'>
            <div class='title-bar' style='background:{{titlebg_08}};'>
                <h3><i class ='fa fa-table'></i> <span class='update-name'>{{widgetname_08}}</span> <span class ='cache-value'></span> <input ng-model='widgetname_08' type='text' /> </h3> <div class='buttons-sets'>
                <a ng-click="vm.chart.backActionOnDrilldown(chngeIdx)" ng-show="vm.dashboardAndReportComInformation.widgetList[vm.dashboardAndReportComInformation.widgetList[chngeIdx].activeWidgetIndex].widgetConfiguration.isRender==false"  class ='custom-btn icon-only'><i class ='fa fa-arrow-left'></i><span class='custom-tooltip'>Back</span></a>
                    <a class='custom-btn icon-only collapse-btn '><i class='fa fa-minus'></i><span class='custom-tooltip'>Collapse</span></a> <a ng-show="vm.modeType=='edit'" class='custom-btn icon-only opt-btn '><i class='fa fa-columns'></i><span class='custom-tooltip'>Panel Color</span></a> <ul class='opt-dropdown'> <li class='has-children'><a href="javascript:void(0);" title=''>Panel Color</a> <ul class='panel-color'> <li> <a href="javascript:void(0);" title=''><input class='bgcolor' type='text' value='#fff' ng-model='titlebg_08' /> <div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul> </li> </ul>
                    <a ng-if="vm.dashboardAndReportComInformation.widgetList[chngeIdx].data.length>0" class='custom-btn icon-only open-filters '><i class='fa fa-filter'></i><span class='custom-tooltip'>Filters</span></a>

                    <div class="filters-drop">
                        <h5>Filters</h5>
                        <div ivh-treeview="vm.dashboardAndReportComInformation.widgetList[chngeIdx].columnValueForFilteration"
                             ivh-treeview-on-cb-change="vm.changeNodeOnCheckBox(ivhNode, ivhIsSelected, ivhTree,chngeIdx)" ivh-treeview-options="vm.customOpts">
                        </div>
                        <div class="drop-bottom">
                            <button ng-click="vm.applyFilter(chngeIdx)" class="custom-btn color1 applyFilter">Apply</button>
                        </div>
                    </div>
                    <a ng-show="vm.modeType=='edit'" ng-click="vm.makeCloneOfWidget(chngeIdx)" class="custom-btn icon-only opt-btn hide-clone-in-tab"><i class="fa fa-copy"></i><span class="custom-tooltip">Make Clone</span></a><a ng-show="vm.modeType=='edit'" class='custom-btn icon-only remove-btn '><i class='fa fa-times'></i><span class='custom-tooltip'>Remove</span></a>
                </div>
            </div><div class="overlay" style="display:none;"><div class="overlay__inner"><div class="overlay__content"><span class="spinner"></span></div></div></div> <div class='content-wrapper'> <div class="center-icon"> <i class="fa fa-bar-chart"></i>  </div>  </div>
        </div>
    </div>
</div>`,
            "settings": `<div class="toggle-item widget-settings chart_settings">
    <h2><i class="fa fa-cog"></i> Settings</h2> <div class="content" style="display: none" id="settings">
        <form>
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <label>Company Name</label>
                    <ui-select class="companyType" ng-model="test" theme="select2" title="Company type">
                        <ui-select-match placeholder="Please select company type...">{{$select.selected.companyName}}</ui-select-match>
                        <ui-select-choices repeat="company.keycode as company in vm.chart.companiesName | propsFilter: {companyName: $select.search}">
                            <div ng-bind-html="company.companyName | highlight: $select.search"></div>

                        </ui-select-choices>
                    </ui-select>
                </div>
                <div class="col-lg-12 col-md-12">
                    <label>Chart Type Name</label>  <ui-select class="chartType" ng-model="test" theme="select2" title="Charts Type">
                        <ui-select-match placeholder="Please select chart type...">{{$select.selected.chartTypeName}}</ui-select-match>
                        <ui-select-choices group-by="'groupingCharttype'" repeat="chartType.chartType as chartType in vm.chart.nameOfChartTypes | propsFilter: {chartTypeName: $select.search}">
                            <div ng-bind-html="chartType.chartTypeName | highlight: $select.search"></div>

                        </ui-select-choices>
                    </ui-select>
                </div> <div class="col-lg-12 col-md-12">
                    <div class="toggle" toggle>
                        <div class="toggle-item">
                            <h2>Title Settings</h2> <div class="content">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12"> <label>Title</label> <input class="chartTitle" type="text" placeholder="Please enter the chart title" /> </div><div class="col-lg-12 col-md-12 inline"> <label>Font Size</label> <input class="theme4 titleFontSize" ionmin="30" ionmax="60" ionfrom="" rangeslider /> </div><div class="col-lg-12 col-md-12 inline"> <label>Font Color</label> <input class="bgcolor titleFontColor" type="text" value="#fafafa" /> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-12 col-md-12 inline">
                                        <label>Font Family</label> <ui-select class="titleFontFamily" ng-model="test" theme="select2" title="Choose a font family">
                                            <ui-select-match placeholder="font family...">{{$select.selected.fontFamily}}</ui-select-match>
                                            <ui-select-choices repeat="fontFamilyDdl.fontFamily as fontFamilyDdl in  vm.chart.fontFamily | propsFilter: {fontFamily: $select.search}">
                                                <div ng-bind-html="fontFamilyDdl.fontFamily | highlight: $select.search"></div>

                                            </ui-select-choices>
                                        </ui-select>
                                    </div><div class="col-lg-6 col-md-12"> <label><input class="titleFontWeight" ng-false-value="'normal'" ng-true-value="'bold'" type="checkbox" />  Bold</label> </div><div class="col-lg-6 col-md-12"> <label><input class="titleFontStyle" ng-false-value="'normal'" ng-true-value="'italic'" type="checkbox" />  Italic</label> </div>
                                </div>
                            </div>
                        </div><div class="toggle-item">
                            <h2>Subtitle Settings</h2> <div class="content">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12"> <label>Subtitle</label> <input class="chartSubtitle" type="text" placeholder="Please enter the chart subtitle" /> </div><div class="col-lg-12 col-md-12 inline"> <label>Font Size</label> <input class="theme4 subTitleFontSize" ionmin="22" ionmax="60" ionfrom="" rangeslider /> </div><div class="col-lg-12 col-md-12 inline"> <label>Font Color</label>  <input class="bgcolor subTitleFontColor" type="text" value="#fafafa" /> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-12 col-md-12 inline">
                                        <label>Font Family</label><ui-select class="subTitleFontFamily" ng-model="test" theme="select2" title="Choose a font family">
                                            <ui-select-match placeholder="font family...">{{$select.selected.fontFamily}}</ui-select-match>
                                            <ui-select-choices repeat="fontFamilyDdl.fontFamily as fontFamilyDdl in  vm.chart.fontFamily | propsFilter: {fontFamily: $select.search}">
                                                <div ng-bind-html="fontFamilyDdl.fontFamily | highlight: $select.search"></div>

                                            </ui-select-choices>
                                        </ui-select>
                                    </div><div class="col-lg-6 col-md-12">  <label><input class="subTitleFontWeight" ng-false-value="'normal'" ng-true-value="'bold'" type="checkbox" />  Bold</label> </div><div class="col-lg-6 col-md-12"><label><input class="subTitleFontStyle" ng-false-value="'normal'" ng-true-value="'italic'" type="checkbox" />  Italic</label> </div>
                                </div>
                            </div>
                        </div><div class="toggle-item">
                            <h2>Legend Settings</h2> <div class="content">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12"> <label><input class="legendShowInLegend" ng-false-value=false ng-true-value=true type="checkbox" />  On</label> </div><div class="col-lg-12 col-md-12 inline">
                                        <label>Horizental</label> <ui-select class="legendHorizontalAlign" ng-model="test" theme="select2" title="Choose a legend position">
                                            <ui-select-match placeholder="Position horizental...">{{$select.selected.horizontalAlignValue}}</ui-select-match>
                                            <ui-select-choices repeat="horizontalAlign.horizontalAlignValue as horizontalAlign in  vm.chart.horizontalAlign | propsFilter: {horizontalAlignValue: $select.search}">
                                                <div ng-bind-html="horizontalAlign.horizontalAlignValue | highlight: $select.search"></div>

                                            </ui-select-choices>
                                        </ui-select>
                                    </div><div class="col-lg-12 col-md-12 inline">
                                        <label>Vertical</label> <ui-select class="lengendVerticalAlign" ng-model="test" theme="select2" title="Horizental">
                                            <ui-select-match placeholder="Position vertical...">{{$select.selected.verticalAlignValue}}</ui-select-match>
                                            <ui-select-choices repeat="verticalAlign.verticalAlignValue as verticalAlign in  vm.chart.verticalAlign | propsFilter: {verticalAlignValue: $select.search}">
                                                <div ng-bind-html="verticalAlign.verticalAlignValue | highlight: $select.search"></div>

                                            </ui-select-choices>
                                        </ui-select>
                                    </div><div class="col-lg-12 col-md-12 inline"> <label>Font Size</label> <input class="theme4 legendFontSize" ionmin="12" ionmax="30" ionfrom="13" rangeslider /> </div><div class="col-lg-12 col-md-12 inline"> <label>Font Color</label> <input class="bgcolor legendFontColor" type="text" value="#fafafa" /> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-12 col-md-12 inline">
                                        <label>Font Family</label> <ui-select class="legendFontFamily" ng-model="test" theme="select2" title="Vertical">
                                            <ui-select-match placeholder="font family...">{{$select.selected.fontFamily}}</ui-select-match>
                                            <ui-select-choices repeat="fontFamilyDdl.fontFamily as fontFamilyDdl in  vm.chart.fontFamily | propsFilter: {fontFamily: $select.search}">
                                                <div ng-bind-html="fontFamilyDdl.fontFamily | highlight: $select.search"></div>

                                            </ui-select-choices>
                                        </ui-select>
                                    </div><div class="col-lg-6 col-md-12"> <label><input class="legendFontWeight" ng-false-value="'normal'" ng-true-value="'bold'" type="checkbox" />  Bold</label> </div><div class="col-lg-6 col-md-12"> <label><input class="legendFontStyle" ng-false-value="'normal'" ng-true-value="'italic'" type="checkbox" />  Italic</label> </div>
                                </div>
                            </div>
                        </div>
                        <div class="toggle-item">
                            <h2>X Axis</h2>
                            <div class="content">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12"> <label>X Axis Title</label> <input class="axisXTitle" type="text" placeholder="Enter X Axis Title" /> </div><div class="col-lg-12 col-md-12"> <label>Title Font Size</label> <input class="theme4 axisXTitleFontSize" ionmin="6" ionmax="30" ionfrom="13" rangeslider /> </div><div class="col-lg-12 col-md-12"> <label>Title Font Color</label> <input class="bgcolor axisXTitleFontColor" type="text" value="#fafafa" /> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-6 col-md-12"> <label> <input class="axisXTitleFontWeight" ng-false-value="'normal'" ng-true-value="'bold'" type="checkbox" />  Bold</label> </div><div class="col-lg-6 col-md-12"> <label><input class="axisXTitleFontStyle" type="checkbox" ng-false-value="'normal'" ng-true-value="'italic'" />  Italic</label> </div><div class="col-lg-12 col-md-12"> <label>Label Font Size</label> <input class="theme4 axisXLabelFontSize" ionmin="6" ionmax="30" ionfrom="13" rangeslider /> </div><div class="col-lg-12 col-md-12"> <label>Label Font Color</label> <input class="bgcolor axisXLabelFontColor" type="text" value="#fafafa" /> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-6 col-md-12"> <label><input class="axisXLabelFontWeight" ng-false-value="'normal'" ng-true-value="'bold'" type="checkbox" />  Bold</label> </div><div class="col-lg-6 col-md-12"> <label><input class="axisXLabelFontStyle" ng-false-value="'normal'" ng-true-value="'italic'" type="checkbox" />  Italic</label> </div><div class="col-lg-6 col-md-12"> <label>Prefix</label> <input class="axisXPrefix" type="text" placeholder="Prefix" /> </div><div class="col-lg-6 col-md-12"> <label>Suffix</label> <input class="axisXSuffix" type="text" placeholder="Suffix" /> </div>
                                    <div class ="col-lg-12 col-md-12 value-format-settings">
                                        <label>Value Formatting</label>
                                        <ui-select class="xValueFormattingDropdown" ng-model="test" theme="select2" title="Value Formatting">
                                            <ui-select-match placeholder="Value Formatting...">{{$select.selected.text}}</ui-select-match>
                                            <ui-select-choices repeat="valueFormat.value as valueFormat in vm.chart.valueFormattingString | propsFilter: {text: $select.search}">
                                                <div ng-bind-html="valueFormat.text | highlight: $select.search"></div>

                                            </ui-select-choices>
                                        </ui-select>
                                        <input ng-if="vm.dashboardAndReportComInformation.widgetList[chngeIdx].chartConfigurations.settings.XaxisSettings.valueFormattingInput !=''" class ="xValueFormattingInput" type="text" placeholder="Please enter the Value Formatting" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="toggle-item">
                            <h2>Y Axis</h2>
                            <div class="content">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12"> <label>Y Axis Title</label> <input class="axisYTitle" type="text" placeholder="Enter X Axis Title" /> </div><div class="col-lg-12 col-md-12"> <label>Title Font Size</label> <input class="theme4 axisYTitleFontSize" ionmin="6" ionmax="30" ionfrom="13" rangeslider /> </div><div class="col-lg-12 col-md-12"> <label>Title Font Color</label> <input class="bgcolor axisYTitleFontColor" type="text" value="#fafafa" /> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-6 col-md-12"> <label><input class="axisYTitleFontWeight" ng-false-value="'normal'" ng-true-value="'bold'" type="checkbox" />  Bold</label> </div><div class="col-lg-6 col-md-12"> <label><input class="axisYTitleFontStyle" type="checkbox" ng-false-value="'normal'" ng-true-value="'italic'" />  Italic</label> </div><div class="col-lg-12 col-md-12"> <label>Label Font Size</label> <input class="theme4 axisYLabelFontSize" ionmin="6" ionmax="30" ionfrom="13" rangeslider /> </div><div class="col-lg-12 col-md-12"> <label>Label Font Color</label> <input class="bgcolor axisYLabelFontColor" type="text" value="#fafafa" /> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-6 col-md-12"> <label><input class="axisYLabelFontWeight" ng-false-value="'normal'" ng-true-value="'bold'" type="checkbox" />  Bold</label> </div><div class="col-lg-6 col-md-12"> <label><input class="axisYLabelFontStyle" ng-false-value="'normal'" ng-true-value="'italic'" type="checkbox" />  Italic</label> </div><div class="col-lg-6 col-md-12"> <label>Prefix</label> <input class="axisYPrefix" type="text" placeholder="Prefix" /> </div><div class="col-lg-6 col-md-12"> <label>Suffix</label> <input class="axisYSuffix" type="text" placeholder="Suffix" /> </div>
                                    <div class ="col-lg-12 col-md-12 value-format-settings">
                                        <label>Value Formatting</label>
                                        <ui-select class="yValueFormattingDropdown" ng-model="test" theme="select2" title="Value Formatting">
                                            <ui-select-match placeholder="Value Formatting...">{{$select.selected.text}}</ui-select-match>
                                            <ui-select-choices repeat="valueFormat.value as valueFormat in vm.chart.valueFormattingString | propsFilter: {text: $select.search}">
                                                <div ng-bind-html="valueFormat.text | highlight: $select.search"></div>

                                            </ui-select-choices>
                                        </ui-select>
                                        <input ng-if="vm.dashboardAndReportComInformation.widgetList[chngeIdx].chartConfigurations.settings.YaxisSettings.valueFormattingInput !=''" class ="yValueFormattingInput" type="text" placeholder="Please enter the Value Formatting" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6 col-md-12"><label> <input ng-true-value="''" ng-false-value="'no-tableheaders'" class="headerInput" type="checkbox" />  Header</label> </div>
            </div>
        </form>
    </div>
</div>`,
            "advanced_settings": `<div class="toggle-item widget-settings chart_settings">
    <h2><i class="fa fa-cog"></i> Advanced Settings</h2> <div class="content" style="display:none" id="settings">
        <div class="single-chart">
            <form>
                <div class="row"> <div class="col-lg-12 col-md-12"> <label>X axis</label> <ui-select  ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].chartConfigurations.Advancesettings.xAxis" ng-change="vm.chartAxisChange(chngeIdx)" theme="select2" title="Choose a x-axis">
    <ui-select-match placeholder="Please select x-axis...">{{$select.selected.multiSeriesXaxisColumnsName}}</ui-select-match>
    <ui-select-choices repeat="multiSeriesXaxisName.multiSeriesXaxisColumnsName as multiSeriesXaxisName in vm.dashboardAndReportComInformation.widgetList[chngeIdx].multiSeriesXaixSelect | propsFilter: {multiSeriesXaxisColumnsName: $select.search}">
        <div ng-bind-html="multiSeriesXaxisName.multiSeriesXaxisColumnsName | highlight: $select.search"></div>

    </ui-select-choices>
</ui-select> </div> <div class="col-lg-12 col-md-12">
    <label>Y axis</label>  <ui-select ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].chartConfigurations.Advancesettings.yAxis" ng-change="vm.chartAxisChange(chngeIdx)" theme="select2" title="Choose a y-axis">
        <ui-select-match placeholder="Please select y-axis...">{{$select.selected.multiSeriesYaxisColumnsName}}</ui-select-match>
        <ui-select-choices repeat="multiSeriesXaxisName.multiSeriesYaxisColumnsName as multiSeriesXaxisName in vm.dashboardAndReportComInformation.widgetList[chngeIdx].multiSeriesYaixSelect | propsFilter: {multiSeriesYaxisColumnsName: $select.search}">
            <div ng-bind-html="multiSeriesXaxisName.multiSeriesYaxisColumnsName | highlight: $select.search"></div>

        </ui-select-choices>
    </ui-select>
</div><div class="col-lg-12 col-md-12 series-select-condition">
    <label>Series</label>  <ui-select ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].chartConfigurations.Advancesettings.seriesOnGroup" ng-change="vm.chartAxisChange(chngeIdx)" theme="select2" title="Choose a column group">
        <ui-select-match placeholder="Please select x-axis...">{{$select.selected.multiSeriesXaxisColumnsName}}</ui-select-match>
        <ui-select-choices repeat="multiSeriesXaxisName.multiSeriesXaxisColumnsName as multiSeriesXaxisName in vm.dashboardAndReportComInformation.widgetList[chngeIdx].multiSeriesXaixSelect | propsFilter: {multiSeriesXaxisColumnsName: $select.search}">
            <div ng-bind-html="multiSeriesXaxisName.multiSeriesXaxisColumnsName | highlight: $select.search"></div>

        </ui-select-choices>
    </ui-select>
</div>  </div>
            </form>
        </div>

        <div class="multi-series-chart">
            <form>
                <div class="row">
                    <div class="col-lg-12 col-md-12 clonable-part">
                        <label><strong>X axis</strong> <a class="clone-me custom-btn color1" href="javascript:void(0);" title="" ng-click="vm.addMultiSeriesXaxis(chngeIdx)"><i class="fa fa-plus-circle"></i> Add New</a><a ng-click="vm.resetXaxis(chngeIdx)" class="clone-me custom-btn color13 icon-btn-only" href="javascript:void(0);" title=""><i class="fa fa-redo-alt"></i></a></label><div class ="clonable" ng-repeat="item in  vm.dashboardAndReportComInformation.widgetList[chngeIdx].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis track by $index ">
                            <a ng-click="vm.removeXaxisMul(chngeIdx,$index)" ng-if="$index != 0" class ="remove-it" href="javascript:void(0);" title="remove"><i class ="fa fa-minus"></i></a> <ui-select ng-change="vm.chartAxisChange(chngeIdx)" class ="multiseriesxaix" ng-model="item.xAxisColumnNameMul" theme="select2" title="Choose a x-axis columns">
                                <ui-select-match placeholder="Please select x-axis...">{{$select.selected.multiSeriesXaxisColumnsName}}</ui-select-match>
                                <ui-select-choices repeat="multiSeriesXaxisName.multiSeriesXaxisColumnsName as multiSeriesXaxisName in vm.dashboardAndReportComInformation.widgetList[chngeIdx].multiSeriesXaixSelect | propsFilter: {multiSeriesXaxisColumnsName: $select.search}">
                                    <div ng-bind-html="multiSeriesXaxisName.multiSeriesXaxisColumnsName | highlight: $select.search"></div>

                                </ui-select-choices>
                            </ui-select>
                        </div>
                    </div>

                    <div class="col-lg-12 col-md-12 clonable-part">
                        <label><strong>Y axis</strong> <a ng-click="vm.addMultiSeriesYaxis(chngeIdx)" class="clone-me custom-btn color1" href="javascript:void(0);" title=""><i class="fa fa-plus-circle"></i> Add New</a><a ng-click="vm.resetYaxis(chngeIdx)" class="clone-me custom-btn color13 icon-btn-only" href="javascript:void(0);" title=""><i class="fa fa-redo-alt"></i></a></label>
                        <div class="clonable" ng-repeat="item in  vm.dashboardAndReportComInformation.widgetList[chngeIdx].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis track by $index ">
                            <a ng-click="vm.removeYaxisMul(chngeIdx,$index)" ng-if="$index != 0" class ="remove-it" href="javascript:void(0);" title="remove xaxis"><i class ="fa fa-minus"></i></a>
                            <div class="row narrow">
                                <div class="col-lg-12 col-md-12"><input ng-blur="vm.chartAxisChange(chngeIdx)" ng-model="item.columnTitle" type="text" placeholder="Please the column title" value="" /></div>
                                <div class="col-lg-6 col-md-12">

                                    <ui-select ng-change="vm.chartAxisChange(chngeIdx)" class="multiseriesYaxis" ng-model="item.yAxisColumnNameMul" theme="select2" title="Choose a y-axis columns">
                                        <ui-select-match placeholder="Please select y-axis...">{{$select.selected.multiSeriesYaxisColumnsName}}</ui-select-match>
                                        <ui-select-choices repeat="multiSeriesXaxisName.multiSeriesYaxisColumnsName as multiSeriesXaxisName in vm.dashboardAndReportComInformation.widgetList[chngeIdx].multiSeriesYaixSelect | propsFilter: {multiSeriesYaxisColumnsName: $select.search}">
                                            <div ng-bind-html="multiSeriesXaxisName.multiSeriesYaxisColumnsName | highlight: $select.search"></div>

                                        </ui-select-choices>
                                    </ui-select>
                                </div> <div class="col-lg-6 col-md-12">
                                    <ui-select ng-change="vm.chartAxisChange(chngeIdx)" class="chartType" ng-model="item.yAxisChartType" theme="select2" title="Choose a chart type">
                                        <ui-select-match placeholder="Please select chart type...">{{$select.selected.chartTypeName}}</ui-select-match>
                                        <ui-select-choices group-by="'groupingCharttype'" repeat="chartType.chartType as chartType in vm.chart.nameOfChartTypes | propsFilter: {chartTypeName: $select.search}">
                                            <div ng-bind-html="chartType.chartTypeName | highlight: $select.search"></div>

                                        </ui-select-choices>
                                    </ui-select>
                                </div> <div class="col-lg-6 col-md-12"> <input ng-model="item.yAxisColorPicker" class="bgcolor" type="text" ng-change="vm.chartAxisChange(chngeIdx)" value="#fafafa" /> <div class="colorpickerwrapper"> <div colorpicker></div></div> </div> <div class="col-lg-6 col-md-12"> <select ng-change="vm.chartAxisChange(chngeIdx)" ng-model="item.yAxisSecPriType" class="full"><option value="secondary">Secondary</option><option value="primary">Primary</option></select></div>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-12 col-md-12 clonable-part">
                        <label><strong>Group</strong> <a ng-click="vm.resetMulSeries(chngeIdx)" class="clone-me custom-btn color13 icon-btn-only" href="javascript:void(0);" title=""><i class="fa fa-redo-alt"></i></a></label><div class ="clonable">
                            <ui-select ng-change="vm.chartAxisChange(chngeIdx)" class="multiseriesxaix" ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.seriesOnGroup" theme="select2" title="Choose a x-axis">
                                <ui-select-match placeholder="Please select x-axis...">{{$select.selected.multiSeriesXaxisColumnsName}}</ui-select-match>
                                <ui-select-choices repeat="multiSeriesXaxisName.multiSeriesXaxisColumnsName as multiSeriesXaxisName in vm.dashboardAndReportComInformation.widgetList[chngeIdx].multiSeriesXaixSelect | propsFilter: {multiSeriesXaxisColumnsName: $select.search}">
                                    <div ng-bind-html="multiSeriesXaxisName.multiSeriesXaxisColumnsName | highlight: $select.search"></div>

                                </ui-select-choices>
                            </ui-select>
                        </div>
                    </div>

                </div>
            </form>
        </div>
    </div>
</div>`,
            "chartDiv": `<div id="chartContainer"></div>`

        },
        {
            "type": "widgetOfMap",
            "code": `<div id=-1 data-position='1' data-widgetIndex="{{chngeIdx}}" class="grid-stack-item" data-gs-resize-handles="e,se,s">
    <div class="grid-stack-item-content">
        <div class='widget'>
            <div class='title-bar' style='background:{{titlebg_08}};'>
                <h3><i class='fa fa-table'></i> <span class='update-name'>{{widgetname_08}}</span> <span class='cache-value'></span> <input ng-model='widgetname_08' type='text' /> </h3>
                <div class='buttons-sets' >
                    <a class='custom-btn icon-only collapse-btn '><i class='fa fa-minus'></i><span class='custom-tooltip'>Collapse</span></a> <a ng-show="vm.modeType=='edit'" class='custom-btn icon-only opt-btn '><i class='fa fa-columns'></i><span class='custom-tooltip'>Panel Color</span></a> <ul class='opt-dropdown '> <li class='has-children'><a href="javascript:void(0);" title=''>Panel Color</a> <ul class='panel-color'> <li> <a href="javascript:void(0);" title=''><input class='bgcolor' type='text' value='#fff' ng-model='titlebg_08' /> <div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul> </li> </ul>

                    <a ng-if="vm.dashboardAndReportComInformation.widgetList[chngeIdx].data.length>0" class='custom-btn icon-only open-filters'><i class='fa fa-filter'></i><span class='custom-tooltip'>Filters</span></a>
                    <div class="filters-drop">
                        <h5>Filters</h5>
                        <div ivh-treeview="vm.dashboardAndReportComInformation.widgetList[chngeIdx].columnValueForFilteration"
                             ivh-treeview-on-cb-change="vm.changeNodeOnCheckBox(ivhNode, ivhIsSelected, ivhTree,chngeIdx)" ivh-treeview-options="vm.customOpts">
                        </div>
                        <div class="drop-bottom">
                            <button ng-click="vm.applyFilter(chngeIdx)" class="custom-btn color1 applyFilter">Apply</button>
                        </div>
                    </div>
                    <a ng-show="vm.modeType=='edit'" ng-click="vm.makeCloneOfWidget(chngeIdx)" class="custom-btn icon-only opt-btn hide-clone-in-tab"><i class="fa fa-copy"></i><span class="custom-tooltip">Make Clone</span></a><a ng-show="vm.modeType=='edit'" class='custom-btn icon-only remove-btn'><i class='fa fa-times'></i><span class='custom-tooltip'>Remove</span></a>
                </div>
            </div><div class="overlay" style="display:none;"><div class="overlay__inner"><div class="overlay__content"><span class="spinner"></span></div></div></div>  <div class='content-wrapper-map content-wrapper'> <div class=maps><div class="center-icon"> <i class="fa fa-map"></i>  </div></div> </div>
        </div>
    </div>
</div>`,
            "settings": `<div class="toggle-item widget-settings map_settings">
    <h2><i class="fa fa-cog"></i> Settings</h2>
    <div class="content" style="display: none" id="settings">
        <form>
            <div class="row">
                <div class="col-lg-6 col-md-12"> <label><input ng-true-value="''" ng-false-value="'no-tableheaders'" class="headerInput" type="checkbox" />  Header</label> </div>
               
            </div>
        </form>
    </div>
</div>`,
            "advanced_settings": `<div class="toggle-item widget-settings map_settings"> <h2><i class="fa fa-cog"></i> Advanced Settings</h2> <div class="content" style="display: none" id="settings"><form>
              <div class ="row" ng-if="vm.dashboardAndReportComInformation.widgetList[chngeIdx].mapColumnLatLng.length>0">  <div class ="col-lg-12 col-md-12">
    <label>Latitude</label> <ui-select ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].mapConfigurations.mapLatLng.Lat" theme="select2" title="Choose a column">
        <ui-select-match placeholder="Please select Column...">{{$select.selected.latLngName}}</ui-select-match>
        <ui-select-choices repeat="latLngObj.latLngName as latLngObj in vm.dashboardAndReportComInformation.widgetList[chngeIdx].mapColumnLatLng | propsFilter: {latLngName: $select.search}">
            <div ng-bind-html="latLngObj.latLngName | highlight: $select.search"></div>

        </ui-select-choices>
    </ui-select>
</div>
<div class ="col-lg-12 col-md-12">
    <label>Longitude</label> <ui-select ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].mapConfigurations.mapLatLng.Lng" theme="select2" title="Choose a column">
        <ui-select-match placeholder="Please select Column...">{{$select.selected.latLngName}}</ui-select-match>
        <ui-select-choices repeat="latLngObj.latLngName as latLngObj in vm.dashboardAndReportComInformation.widgetList[chngeIdx].mapColumnLatLng | propsFilter: {latLngName: $select.search}">
            <div ng-bind-html="latLngObj.latLngName | highlight: $select.search"></div>

        </ui-select-choices>
    </ui-select>
</div>
</div>
                <div class ="row"><div class ="col-lg-12 col-md-12 mapleveltype"> <label class ="label">Level Type</label> <label class="radio"> <input class="level-check"  type="radio" value="Country" name="radio" class=""> <i></i>Country</label> <label class="radio"> <input class="level-check"  type="radio" value="State" name="radio"> <i></i>State</label> <label class="radio"> <input class="level-check" type="radio" value="Region" name="radio"> <i></i>Region</label><label class="radio"> <input class="level-check market-radio" type="radio" value="Market" name="radio"> <i></i>Market</label></div>

                     <div class ="col-lg-12 col-md-12" ng-If="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.mapleveltype == 'Market'">
                        <select class="ddmarketfiltercountries" style="width:100%;margin-bottom: 10px;">
                        </select>
                    </div>
                    <div class ="col-lg-12 col-md-12 MarketList" ng-If="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.mapleveltype == 'Market' && vm.dashboardAndReportComInformation.widgetList[chngeIdx].IsAllCountriesOptionSelected != true">
                        <select class ="marketsmap multiselect compact-multiselect" ng-Model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.filterSelectedCities" multiple="multiple" multiselect>

                            <option class ="selectRow marketfilterchkbox" ng-click="vm.CountMarketChkbox(chngeIdx)" ng-repeat="x in vm.dashboardAndReportComInformation.widgetList[chngeIdx].MapFilterCitiesList" value="{{x.Value}}:{{x.NoOfRecords}}" data-recordnumber="x.NoOfRecords">{{x.Value}} ({{x.NoOfRecords}}) </option>
                        </select>

                    </div>
                    <div class ="col-lg-12 col-md-12">
                        <button type="button" ng-If="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.mapleveltype == 'Market'" ng-click="vm.Savemarketfilterchkbox(chngeIdx,$event)" class ="custom-btn color1 float-right">Apply</button>
                    </div>
                </div>

                <!-- code start -->
               <div class ="row" ng-If="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.mapleveltype == 'Market'">
					<div class ="col-lg-12 col-md-12">
						<div class ="toggle" ng-If="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.mapleveltype == 'Market'">
							<div class ="toggle-item">
                            <div class ="content">
                                <div class ="colors-list">
									<div class ="row">
										<div class ="col-lg-12 col-md-12 sourceForMapPinSetting">
			                                    <label class ="label">Source</label>
			                                    <label class ="radio"  ng-repeat="item in  vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.historyOfQueryBuilder | filter: vm.filterOnlyStringTypeColumn | filter: vm.filterSeperateAlreadyAddedColumn">
				                                    <input type="radio" value="{{item.columnaliasname}}" name="radio1">
					                                <i></i>{{item.columnaliasname}}
				                                </label>
		                                </div>
									</div>
                                </div>
								</div>
							</div>
						</div>
					</div>
				</div>

                <div class ="row">
					<div class ="col-lg-12 col-md-12" >
						<div class ="toggle" toggle>
							<div class ="toggle-item">
								<div class ="content">
                                <div class="colors-list">
									<div class ="row">
										<div class ="col-lg-12 col-md-12 inline" ng-repeat="item in vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.GMapData[vm.dashboardAndReportComInformation.currentSelectedColumn]">
											<label>{{item.Name}}</label>
											<input class ="bgcolor" type="text" ng-Model="item.PinColor" style="background-color: {{item.PinColor}}" />
											<div class ="colorpickerwrapper">
												<div colorpicker></div>
											</div>
										</div>
									</div>
                               </div>
								</div>
							</div>
						</div>

                        <div class ="settings-footer">
                            <a href="javascript:void(0);" class ="color1 custom-btn ng-binding" ng-click="vm.ApplyMapPinColorSetting(chngeIdx)" style="cursor:pointer" title="Apply Setting">Apply</a>
                        </div>

					</div>
				</div>

                </form>

              </div></div><!--code start-->`
        },
        {
            "type": "widgetOfDefault",
            "code": `<div id=-1 data-position='1' data-widgetIndex="{{chngeIdx}}" class="grid-stack-item" data-gs-resize-handles="e,se,s">
    <div class="grid-stack-item-content">
        <div class='widget'>
            <div class='title-bar' style='background:{{titlebg_08}};'>
                <h3><i class='fa fa-table'></i> <span class='update-name'>{{widgetname_08}}</span> <span class='cache-value'></span> <input ng-model='widgetname_08' type='text' /> </h3>
                <div class='buttons-sets'><a class='custom-btn icon-only collapse-btn'><i class='fa fa-minus'></i><span class='custom-tooltip'>Collapse</span></a> <a ng-show="vm.modeType=='edit'" class='custom-btn icon-only opt-btn'><i class='fa fa-columns'></i><span class='custom-tooltip'>Panel Color</span></a> <ul class='opt-dropdown'><li class='has-children'><a href="javascript:void(0);" title=''>Panel Color</a><ul class='panel-color'> <li> <a href="javascript:void(0);" title=''><input class='bgcolor' type='text' value='#fff' ng-model='titlebg_08' /><div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul></li><li class='has-children'><a href="javascript:void(0);" title=''>Widget Body Color</a><ul class='panel-color panel-color-widget-in'><li> <a href="javascript:void(0);" title=''><input class='bgcolor-content-color' type='text' value='#fff' ng-model='titlebg_08' /><div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul></li></ul><a ng-show="vm.modeType=='edit'" class='custom-btn icon-only remove-btn'><i class='fa fa-times'></i><span class='custom-tooltip'>Remove</span></a> </div>
            </div> <div class="overlay" style="display:none;"><div class="overlay__inner"><div class="overlay__content"><span class="spinner"></span></div></div></div><div class='content-wrapper'>


                <div class="center-icon"> <i class="fa fa-file"></i>  </div>
            </div>
        </div>
    </div>
</div>`,
            "settings": `<div class="toggle-item widget-settings default_settings"> <h2><i class="fa fa-cog"></i> Settings</h2> <div class="content" style="display: none" id="settings"><form><div class="row"> <div class="col-lg-12 col-md-12">
                    <label>Layout Type</label>
                    <ui-select class ="layoutTypeDefaultPage" ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].defaultPageConfiguration.layoutTypeDefaultPage" theme="select2" title="Layout Type">
                        <ui-select-match placeholder="Please select layout type...">{{$select.selected.text}}</ui-select-match>
                        <ui-select-choices repeat="layoutType.value as layoutType in vm.layoutTypeDefaultPage | propsFilter: {text: $select.search}">
                            <div ng-bind-html="layoutType.text | highlight: $select.search"></div>

                        </ui-select-choices>
                    </ui-select>
                </div><div class="col-lg-12 col-md-12">
    <label>Status Color Column</label> <ui-select ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].defaultPageConfiguration.staClName" theme="select2" title="Choose a column">
        <ui-select-match placeholder="Please select Column...">{{$select.selected.statusClrColName}}</ui-select-match>
        <ui-select-choices repeat="statusClrNameLs.statusClrColName as statusClrNameLs in vm.dashboardAndReportComInformation.widgetList[chngeIdx].stClrColNames | propsFilter: {statusClrColName: $select.search}">
            <div ng-bind-html="statusClrNameLs.statusClrColName | highlight: $select.search"></div>

        </ui-select-choices>
    </ui-select>
</div>
<div class ="col-lg-6 col-md-6"><label>S/M Rows </label><input  type="checkbox" ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].defaultPageConfiguration.isSingleOrMultiRow" ng-false-value="false" ng-true-value="true" /></div><div class="col-lg-6 col-md-6"><label>Columns Range </label><input class ="theme4" ng-change="vm.changeColumnReq(chngeIdx)" type="text" ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].defaultPageConfiguration.isColumnRange" ionmin="1" ionmax="6" ionfrom="{{vm.dashboardAndReportComInformation.widgetList[chngeIdx].defaultPageConfiguration.isColumnRange}}" rangeslider /></div><div class="col-lg-6 col-md-12"><label><input ng-true-value="''" ng-false-value="'no-tableheaders'" class="headerInput" type="checkbox" />  Header</label></div><div class ="col-lg-6 col-md-6 small de-apply-mar"><a class ="color7 text-btn mlsta" ng-click="vm.displayDataRowsColumnsForm(chngeIdx)">Apply</a></div> </div><div class="row add-db-columns"></div><div class ="row"><div class ="col-lg-12 col-md-12 db-label-settings"> <div class ="toggle" aircodaccordion></div></div> </div></form></div></div>`,
            "advanced_settings": `<div class="toggle-item widget-advance-settings default_advance_settings">
    <h2><i class ="fa fa-cog"></i> Advanced Settings</h2> <div class ="content" style="display: none" id="settings"><form> <div class ="row add-control"></div><div class="row"><div class="col-lg-12 col-md-12 db-label-settings"> <div class="toggle" aircodaccordion></div><div aircodaccordion><div class ="toggle-item" ng-repeat="item in vm.dashboardAndReportComInformation.widgetList[chngeIdx].defaultPageConfiguration.customImageControlPropertiesConfiguration track by $index"> <h2>Image Control</h2> <div class="content" style="display: none;">  <div class="row"> <div class="col-lg-6 col-md-12" > <input image-index="{{$index}}" class="fileOrImageControl"  onchange="angular.element(this).scope().imageUploadOfPageWidget(this,chngeIdx)" type="file" />   </div></div> </div> </div> </div></div> </div></form></div>
</div>`,
            "statusColorSettings": `<div class="toggle-item widget-settings default_settings" ng-show="vm.dashboardAndReportComInformation.widgetList[chngeIdx ].defaultPageConfiguration.statusColors.length>0">
    <h2><i class="fa fa-cog"></i> Status Colors</h2>
    <div class="content" style="display: none;">
        <form>
            <div class="row header">
                <div class="col-md-4 col-sm-4 col-lg-4"> Status</div>
                <div class="col-md-4 col-lg-4 col-sm-4 "> Bg Clr</div>
                <div class="col-md-4 col-lg-4 col-sm-4 "> Fnt Clr</div>
            </div>
            <div class="row" ng-repeat="item in vm.dashboardAndReportComInformation.widgetList[chngeIdx ].defaultPageConfiguration.statusColors track by $index">
                <div class ="col-md-4 col-sm-4 col-lg-4"> {{vm.dashboardAndReportComInformation.widgetList[chngeIdx].defaultPageConfiguration.statusColors[$index].statusName}}</div>
                <div class ="col-md-4 col-sm-4 col-lg-4 inline">
                    <input class="bgcolor" type="text" ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx ].defaultPageConfiguration.statusColors[$index].backgroundColor"/>
                    <div class="colorpickerwrapper">
                        <div colorpicker></div>
                    </div>
                </div>
                <div class ="col-md-4 col-sm-4 col-lg-4 inline">
                    <input class="bgcolor" type="text"  ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx ].defaultPageConfiguration.statusColors[$index].fontColor" />
                    <div class="colorpickerwrapper">
                        <div colorpicker></div>
                    </div>
                </div>

            </div>
        </form>
    </div>
</div>`
        },
        {
            "type": "widgetOfFormDotIo",
            "code": `<div id=-1 data-position='1' data-widgetIndex="{{chngeIdx}}" class="grid-stack-item" data-gs-resize-handles="e,se,s">
    <div class="grid-stack-item-content">
        <div class='widget'>
            <div class='title-bar' style='background:{{titlebg_08}};'>
                <h3><i class='fa fa-table'></i> <span class='update-name'>{{widgetname_08}}</span> <span class='cache-value'></span> <input ng-model='widgetname_08' type='text' /> </h3>
                <div class='buttons-sets'>
                    <a  ng-if="vm.modeType=='edit'" class ='custom-btn icon-only' ng-click="vm.changeFormOrBuilder(chngeIdx)"><i ng-class ='vm.dashboardAndReportComInformation.widgetList[chngeIdx].builderNFormIcon'></i><span class='custom-tooltip'>Builder/Form</span></a>
                <a class='custom-btn icon-only collapse-btn'><i class='fa fa-minus'></i><span class='custom-tooltip'>Collapse</span></a> <a ng-show="vm.modeType=='edit'" class='custom-btn icon-only opt-btn'><i class='fa fa-columns'></i><span class='custom-tooltip'>Panel Color</span></a> <ul class='opt-dropdown'><li class='has-children'><a href="javascript:void(0);" title=''>Panel Color</a><ul class='panel-color'> <li> <a href="javascript:void(0);" title=''><input class='bgcolor' type='text' value='#fff' ng-model='titlebg_08' /><div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul></li><li class='has-children'><a href="javascript:void(0);" title=''>Widget Body Color</a><ul class='panel-color panel-color-widget-in'><li> <a href="javascript:void(0);" title=''><input class='bgcolor-content-color' type='text' value='#fff' ng-model='titlebg_08' /><div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul></li></ul><a ng-show="vm.modeType=='edit'" class='custom-btn icon-only remove-btn'><i class='fa fa-times'></i><span class='custom-tooltip'>Remove</span></a> </div>
            </div> <div class="overlay" style="display:none;"><div class="overlay__inner"><div class="overlay__content"><span class="spinner"></span></div></div></div><div class='content-wrapper'>

                <div class ="builder-form-io" ng-show="vm.dashboardAndReportComInformation.widgetList[chngeIdx].builderNFormStatus==true"></div>
                <div class="sub-form-io" ng-show="vm.dashboardAndReportComInformation.widgetList[chngeIdx].builderNFormStatus==false"></div>

            </div>
        </div>
    </div>
</div>`,
            "settings": `<div class="toggle-item widget-settings default_settings">
    <h2><i class="fa fa-cog"></i> Settings</h2>
    <div class="content" style="display: none" id="settings">
        <p>No setting available</p>
    </div>
</div>`,
            "advanced_settings": `<div class="toggle-item widget-advance-settings default_advance_settings">
    <h2><i class="fa fa-cog"></i> Advanced Settings</h2>
    <div class="content" style="display: none" id="settings">
       <p>no setting available</p>
    </div>

</div>`
        },
         {
             "type": "widgetOfPredefined",
             "code": `<div id=-1 data-position='1' data-widgetIndex="{{chngeIdx}}" class="grid-stack-item" data-gs-resize-handles="e,se,s">
    <div class="grid-stack-item-content">
        <div class='widget'>
            <div class='title-bar' style='background:{{titlebg_08}};'>
                <h3><i class='fa fa-table'></i> <span class='update-name'>{{widgetname_08}}</span> <span class='cache-value'></span> <input ng-model='widgetname_08' type='text' /> </h3>
                <div class='buttons-sets'>
                    <a class='custom-btn icon-only' ng-click="vm.changeFormOrBuilder(chngeIdx)"><i ng-class='vm.dashboardAndReportComInformation.widgetList[chngeIdx].builderNFormIcon'></i><span class='custom-tooltip'>Builder/Form</span></a>
                    <a class='custom-btn icon-only collapse-btn'><i class='fa fa-minus'></i><span class='custom-tooltip'>Collapse</span></a> <a ng-show="vm.modeType=='edit'" class='custom-btn icon-only opt-btn'><i class='fa fa-columns'></i><span class='custom-tooltip'>Panel Color</span></a> <ul class='opt-dropdown'><li class='has-children'><a href="javascript:void(0);" title=''>Panel Color</a><ul class='panel-color'> <li> <a href="javascript:void(0);" title=''><input class='bgcolor' type='text' value='#fff' ng-model='titlebg_08' /><div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul></li><li class='has-children'><a href="javascript:void(0);" title=''>Widget Body Color</a><ul class='panel-color panel-color-widget-in'><li> <a href="javascript:void(0);" title=''><input class='bgcolor-content-color' type='text' value='#fff' ng-model='titlebg_08' /><div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul></li></ul><a ng-show="vm.modeType=='edit'" class='custom-btn icon-only remove-btn'><i class='fa fa-times'></i><span class='custom-tooltip'>Remove</span></a>
                </div>
            </div> <div class="overlay" style="display:none;"><div class="overlay__inner"><div class="overlay__content"><span class="spinner"></span></div></div></div><div class='content-wrapper'>
            <div class ="center-icon"> <i class ="fa fa-phone"></i>  </div>


            </div>
        </div>
    </div>
</div>`,
             "settings": `<div class="toggle-item widget-settings default_settings">
    <h2><i class="fa fa-cog"></i> Settings</h2>
    <div class="content" style="display: none" id="settings">
        <div class ="col-lg-6 col-md-12"><label><input ng-true-value="''" ng-false-value="'no-tableheaders'" class ="headerInput" type="checkbox" />  Header</label></div>
    </div>
</div>`,
             "advanced_settings": `<div class="toggle-item widget-advance-settings default_advance_settings">
    <h2><i class="fa fa-cog"></i> Advanced Settings</h2>
    <div class="content" style="display: none" id="settings">
       <p>no setting available</p>
    </div>

</div>`
         }
            , {
                "type": "queryBuilder",
                "code": `<input class="switch-qb tempcls" type="checkbox" style="position:absolute;z-index:1;" ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.multiSchemaDesignerModels.multiSchemaEnableOrDisable" ng-false-value="false" ng-true-value="true" />
<div class="querybuilder-wrapper">
    <div class="querybuilder-inner">
        <div class ='query-show-hide'> <div class ="schema-switch show-hide-query"><span><input type="checkbox" switch value="0" class ="query-swith-check"></span> Show Query</div>
        <div class ="schema-switch"><span><input type="checkbox" switch value="0"></span> Show Schema Designer</div>

        </div>
        <div class="data-source-ddl" ng-if="vm.dashboardAndReportComInformation.centralizeDataSourceNDatabaseGroupBysource.length>0">
            <ui-select ng-change="vm.addDatasourceFromClientSide(chngeIdx)" class="data-source-ddl" ng-model="vm.dataSources" theme="select2" title="Choose a database">
                <ui-select-match placeholder="Datasources...">{{$select.selected.text}}</ui-select-match>
                <ui-select-choices group-by="'groupBy'" repeat="dataSourceGrouping as dataSourceGrouping in  vm.dashboardAndReportComInformation.centralizeDataSourceNDatabaseGroupBysource | propsFilter: {text: $select.search}">
                    <div ng-bind-html="dataSourceGrouping.text | highlight: $select.search"></div>

                </ui-select-choices>
            </ui-select>
        </div>

        <div class="select-table">
            <button type="button" class="custom-btn color1 medium data-source-btn " ng-click="vm.addRemoteDataSource(chngeIdx)">Add Remote Datasource</button>
            <button type="button" ng-if="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetType=='widgetOfChart' && vm.dashboardAndReportComInformation.widgetList[vm.dashboardAndReportComInformation.widgetList[chngeIdx].activeWidgetIndex].chartConfigurations.drilldownParams.nextUniqueKey ==null" class="custom-btn color1 medium drill-down-charts" ng-click="vm.chart.addDrilldownChart(baseRendorIdx)">Add Drilldown Chart</button>
            <button type="button" ng-if="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetType=='widgetOfChart' && vm.dashboardAndReportComInformation.widgetList[vm.dashboardAndReportComInformation.widgetList[chngeIdx].activeWidgetIndex].chartConfigurations.drilldownParams.prevUniqueKey !=null" class ="custom-btn color11 medium delete-drilldown" ng-click="vm.chart.deleteDrilldownChart(vm.dashboardAndReportComInformation.widgetList[baseRendorIdx].activeWidgetIndex,baseRendorIdx)">Delete</button>
            <h3 class="simple-title">Select DataSet</h3> <form>
                <div class="row single-query" style="display:none">
                    <div class="col-lg-12">
                        <ui-select class="querybuilder-datasets" ng-model="test" theme="select2" title="Choose a database">
                            <ui-select-match placeholder="Select Dataset...">{{$select.selected.ViewName}}</ui-select-match>
                            <ui-select-choices repeat="viewsName.ViewName as viewsName in  vm.dashboardAndReportComInformation.viewsInformation | propsFilter: {ViewName: $select.search}">
                                <div ng-bind-html="viewsName.ViewName | highlight: $select.search"></div>

                            </ui-select-choices>
                        </ui-select>
                    </div> <div class="col-lg-12"> <ul class="columns-list"></ul> </div>
                </div>

                <div class="row multi-query">
                    <div class="col-lg-12" ng-if="vm.dashboardAndReportComInformation.centralizeDataSourceNDatabaseGroupBysource.length>0">
                        <input type="checkbox" ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].datasetExCoSta" ng-click="vm.datasetExpandCollapse(chngeIdx)" ng-false-value="false" ng-true-value="true" class="data-sets-expands-collapse"> Expand/Collapse
                        <input type="text" placeholder="search datasets" ng-model="vm.dashboardAndReportComInformation.widgetList[chngeIdx].datasetsFilter">
                        <div ivh-treeview="vm.dashboardAndReportComInformation.widgetList[chngeIdx].treeviewOfMultiDatasetsWithItsColumns" ivh-treeview-filter="vm.dashboardAndReportComInformation.widgetList[chngeIdx].datasetsFilter"
                             ivh-treeview-on-cb-change="vm.schmaDesignerNodeChange(ivhNode, ivhIsSelected, ivhTree,chngeIdx)" ivh-treeview-options="vm.customOpts">
                        </div>
                    </div>
                </div>
            </form>
        </div>
        <div class="selected-columns">
            <div class="columns-head">
                <h3 class="simple-title">Selected Columns</h3>
                <div class="buttons-set"> <a class="custom-btn color11 delete-selected-rows" href="javascript:void(0);" title=""><i class="fa fa-times"></i> Delete Selected</a> <a class="custom-btn color11 delete-all-rows" href="javascript:void(0);" title=""><i class="fa fa-times"></i> Delete All</a> </div>
            </div> <div class="table-grid"> <div class="tg-head"> <span class="small"></span> <span class="small"><input class="select-all-row" type="checkbox" /></span> <span class="custom-width-th">Columns</span> <span>Alias</span> <span>Function</span> <span class="small">Group</span> <span>Sort</span> <span class="small">Actions</span>  </div> <div class="selected-rows"> </div> </div>
        </div>
        <div class="create-query">
            <button type="button" class="custom-btn color1 medium subquery-apply " ng-if="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.multiSchemaDesignerModels.subQueryStatus==true" ng-click="vm.applySubQuery(chngeIdx)">Apply</button><button type="button" class="custom-btn color1 medium subquery-back" ng-if="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.multiSchemaDesignerModels.subQueryStatus==true" ng-click="vm.backSubQuery(chngeIdx)">Back</button><button type="button" class="saveQuery custom-btn color1 medium" ng-if="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.multiSchemaDesignerModels.subQueryStatus==false">Query Result</button><button type="button" class="custom-btn color1 medium multiplesourcetarget" ng-click="vm.openModalForMultipleJoins(chngeIdx)">Add Join</button><button type="button" class="custom-btn color1 medium add-subquery" ng-click="vm.addSubQuery(chngeIdx)" ng-if="vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.multiSchemaDesignerModels.subQueryStatus==false">Add Subquery</button><h3 class="simple-title">Create Query</h3> <div class="querybuilderwhere bs_comp"> </div>
        </div>
        <div class ="query-area add-remove-query" style="display:none"><textarea>{{vm.dashboardAndReportComInformation.widgetList[chngeIdx].finalQueryRef}}</textarea></div>
        <div class="multiSchemaDesignerArea airview-schema-designer"> </div>
    </div>
</div>
`
                //Made the following code commented on requirement change, it was placed in the above code
                //<div class="query-area"><textarea></textarea></div>
            },
             {
                 "type": "DBColumnLabel",
                 "code": `<div class="db-col-label"><div style=display:none class="db-label"></div><div  class="db-label-val"></div></div>`,
                 "settings": `<div class="toggle-item">
    <h2></h2>
    <div class="content" style="display: none;">
        <form>
            <div class="row">
                <div class="col-lg-12 col-md-12 inline">
                    <label>Font Size</label>
                    <input class="theme4 dbcolumnfontsize" ionmin="12" ionmax="100" ionfrom="13" rangeslider />
                </div><div class="col-lg-12 col-md-12 inline">
                    <label>Font Color</label> <input class="bgcolor dbcolumncolor" type="text" value="#fafafa" /> <div class="colorpickerwrapper">
                        <div colorpicker>
                        </div>
                    </div>
                </div>
                <div class="col-lg-12 col-md-12 inline"> <label>Background color</label> <input class="bgcolor dbcolumnbackgroundcolor" type="text" value="#fafafa" /> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-12 col-md-12 inline"> <label>Font Family</label> <select class="dbcolumnfontfamily"> <option>arial</option> <option>tahoma</option> <option>verdana</option> </select> </div><div class="col-lg-6 col-md-12"><label> <input class="dbcolumnbold" name="titlebold" type="checkbox" ng-false-value="'normal'" ng-true-value="'bold'" />  Bold</label> </div><div class="col-lg-6 col-md-12"> <label><input name="titleitalic" class="dbcolumnfontstyle" type="checkbox" ng-false-value="'normal'" ng-true-value="'italic'" />  Italic</label> </div>
                </div>
        </form>
    </div>
</div>`,
                 "inputControl": `<div class="inputcontrolclspage"><span class="update-name custominputSpanOfPage"></span><input style="width:200px;display:none" type="text" class="customInputControlOfPage"></div>`,
                 "imageControl": `<div class="image-control"><img class="img-drag-drop"/></div>`

             },
            {
                "type": "widgetOfTab",
                "code": `<div id=-1 data-position='1' data-widgetIndex="{{chngeIdx}}" class="grid-stack-item" data-gs-resize-handles="e,se,s"><div class="grid-stack-item-content"> <div class="widget"> <div class="title-bar" style="background:{{titlebg_05}};"> <h3><i class="fa fa-edit"></i> <span class="update-name">{{widgetname_05}}</span> <span class="cache-value"></span> <input ng-model="widgetname_05" ng-init="widgetname_05='Tabs Style'" type="text" /> </h3> <div class="buttons-sets"  ng-show="vm.modeType=='edit'"> <a class="custom-btn icon-only collapse-btn "><i class="fa fa-minus"></i><span class="custom-tooltip">Collapse</span></a> <a class="custom-btn icon-only opt-btn "><i class="fa fa-columns"></i><span class="custom-tooltip">Panel Color</span></a><ul class="opt-dropdown" >  <li class="has-children"><a href="javascript:void(0);" title="">Panel Color</a> <ul class="panel-color"> <li> <a href="javascript:void(0);" title=""><input class="bgcolor"  type="text"  value="#fff" ng-model="titlebg"  ng-init="titlebg_05 = '#fff'"  /> <div class="colorpickerwrapper style2"><div colorpicker></div></div></a> </li> </ul> </li> </ul>  <a class="custom-btn icon-only remove-btn "><i class="fa fa-times"></i><span class="custom-tooltip">Remove</span></a> </div> </div> <div class="content-wrapper normal draggableTabPanelBox"> <div dynamictabs class="mytab can-add theme1"> <ul class="nav bi nav-tabs horizontalscrollbar tab-overflow-unset"> <li class="order-last"><a ng-show="vm.modeType=='edit'" class="add-tab" href="javascript:void(0);" title=""><i class="fa fa-plus"style="margin-top:4px"></i>  New Tab</a></li> </ul><!-- Selectors --> <div class="tab-content">  </div> </div> </div> </div></div> </div>`,
                "settings": `<div class="toggle-item widget-settings default_settings"> <h2><i class="fa fa-cog"></i> Settings</h2> <div class="content" id="settings" style="display: none;"><form> <div class="row add-db-columns"></div><div class="row"><div class="col-lg-12 col-md-12 db-label-settings"> <div class="toggle" aircodaccordion></div></div><div class="col-lg-6 col-md-12"><label><input  ng-true-value="''" ng-false-value="'no-tableheaders'"  class="headerInput" type="checkbox" />  Header</label></div> </div></form></div></div>`,
                "advanced_settings": `<div class="toggle-item widget-settings"> <h2>Advamced Settings</h2> <div class="content" style="display: none;"> <p>No Settings Available</p> </div> </div> </div>`
            },
            {
                "type": "widgetOfDocument",
                "code": `<div id=-1 data-position='1' data-widgetIndex="{{chngeIdx}}" class="grid-stack-item" data-gs-width='12' data-gs-min-width="12" data-gs-resize-handles="e,se,s"><div class="grid-stack-item-content"> <div class="widget"> <div class="title-bar" style="background:{{titlebg_05}};"> <h3><i class="fa fa-edit"></i> <span class="update-name">{{widgetname_05}}</span> <span class="cache-value"></span> <input ng-model="widgetname_05" ng-init="widgetname_05='Tabs Style'" type="text" /> </h3> <div class="buttons-sets"  ng-show="vm.modeType=='edit'"><a class="custom-btn icon-only collapse-btn "><i class="fa fa-minus"></i><span class="custom-tooltip">Collapse</span></a> <a class="custom-btn icon-only opt-btn "><i class="fa fa-columns"></i><span class="custom-tooltip">Panel Color</span></a> <ul class="opt-dropdown" >  <li class="has-children"><a href="javascript:void(0);" title="">Panel Color</a> <ul class="panel-color"> <li> <a href="javascript:void(0);" title=""><input class="bgcolor"  type="text"  value="#fff" ng-model="titlebg"  ng-init="titlebg_05 = '#fff'"  /> <div class="colorpickerwrapper style2"><div colorpicker></div></div></a> </li> </ul> </li> </ul> <a class="custom-btn icon-only remove-btn "><i class="fa fa-times"></i><span class="custom-tooltip">Remove</span></a>  </div> </div>
                    <div class ="content-wrapper normal">
                        <div class = "doc-path-wrap" >
                            <div class ="doc-path-panel">
                                <div ivh-treeview="vm.dashboardAndReportComInformation.widgetList[chngeIdx].documentPathList" ivh-treeview-on-cb-change="vm.changeDocumentType(ivhNode, ivhIsSelected, ivhTree,chngeIdx)" ivh-treeview-options="vm.customOpts"></div>
                            </div>
                            <div class ="doc-more-selection active" style="display:none;">
                                <ul>
                                    <li><a href="javascript:void(0);" title="">Document Name 1</a></li>
                                    <li><a class ="selected" href="javascript:void(0);" title="">Document Name 2</a></li>
                                    <li><a href="javascript:void(0);" title="">Document Name 3</a></li>
                                    <li><a href="javascript:void(0);" title="">Document Name 4</a></li>
                                    <li><a href="javascript:void(0);" title="">Document Name 5</a></li>
                                </ul>
                            </div>
                            <div class ="document-area">

                            </div>
                        </div>
                    </div>
                </div>
                </div>
                </div>`,
                "settings": `<div class="toggle-item widget-settings default_settings"> <h2><i class="fa fa-cog"></i> Settings</h2> <div class="content" id="settings"><form> <div class="row add-db-columns"></div><div class="row"><div class="col-lg-12 col-md-12 db-label-settings"> <div class="toggle" aircodaccordion></div></div><div class="col-lg-6 col-md-12"><label><input  ng-true-value="''" ng-false-value="'no-tableheaders'"  class="headerInput" type="checkbox" />  Header</label></div> </div></form></div>`,
                "advanced_settings": `<div class="toggle-item widget-settings"> <h2>Advanced Settings</h2> <div class="content" style="display: none;"> <p>No Settings Available</p> </div> </div> </div>`

            }

        ];
        return widgetSettingJsonForm;
    };

    //#endregion

    //#region layout for page widget

    var handleLayoutForDefaultPage = function () {

        return [{ text: 'Layout 1', value: 'Layout 1' }, { text: 'Layout 2', value: 'Layout 2' }];
    }

    //#endregion

    //#region standard panels

    var handleStandardPanel = function () {

        var standardPanelList = [
             { type: 'widgetOfTable', title: 'Table', icon: 'fa fa-table' },
             { type: 'widgetOfMap', title: 'Map', icon: 'fa fa-map' },
             { type: 'widgetOfChart', title: 'Chart', icon: 'fa fa-bar-chart' },
             { type: 'widgetOfDefault', title: 'Page', icon: 'fa fa-file' },
             { type: 'widgetOfTab', title: 'Tabs', icon: 'fa fa-twitch' },
              { type: 'widgetOfDocument', title: 'Documents', icon: 'fa fa-book' },
               { type: 'widgetOfFormDotIo', title: 'Form.io', icon: 'fa fa-code' },
        ];
        return standardPanelList;
    }


    //#endregion 

    //#region dynamic pridefined widgets 

    var handleDynamicPridefinedWidgets = function (ModType) {
        
        var DynamicPriDefined = {};
        DynamicPriDefined['EMS'] = [];
        DynamicPriDefined['EMS'] = [{ type: 'widgetOfPredefined', coreType: 'contactInfo', title: 'Contact Info', url: '/BusinessIntelligence/DashboardNReport/GetPartialForEntity', filePath: '~/Areas/Entity/Views/Entity/_EntityContactInfo.cshtml', icon: 'fa fa-phone', moduleName: 'EMS', renderInMainCanvas: false, RenderInTab: true, actualParams: [{ sP: 'EntityId', cP: 'getDataBaseOnId', vm: true }, { sP: 'FilePath', cP: 'filePath', vm: false }] },
            { type: 'widgetOfPredefined', coreType: 'Ticket', title: 'Tickets', url: '/BusinessIntelligence/DashboardNReport/GetPartialForEntity', filePath: '~/Areas/Ticket/Views/TicketDashboard/_TicketEntity.cshtml', icon: 'fa fa-ticket', moduleName: 'EMS',  renderInMainCanvas: false, RenderInTab: true, actualParams: [{ sP: 'EntityId', cP: 'getDataBaseOnId', vm: true }, { sP: 'FilePath', cP: 'filePath', vm: false }] },
            { type: 'widgetOfPredefined', coreType: 'ProjectIssue', title: 'Project Issues', url: '/BusinessIntelligence/DashboardNReport/GetPartialForEntity', filePath: '~/Areas/Entity/Views/Entity/_EntityContactInfo.cshtml', icon: 'fa fa-sticky-note', moduleName: 'EMS',  renderInMainCanvas: false, RenderInTab: true, actualParams: [{ sP: 'EntityId', cP: 'getDataBaseOnId', vm: true }, { sP: 'FilePath', cP: 'filePath', vm: false }] },
             { type: 'widgetOfPredefined', coreType: 'Inventory', title: 'Inventory', url: '/BusinessIntelligence/DashboardNReport/GetPartialForEntity', filePath: '~/Areas/Inventory/Views/IssueNote/_EntityInventory.cshtml', icon: 'fa fa-server', moduleName: 'EMS', renderInMainCanvas: false, RenderInTab: true, actualParams: [{ sP: 'EntityId', cP: 'getDataBaseOnId', vm: true }, { sP: 'FilePath', cP: 'filePath', vm: false }] },
              { type: 'widgetOfPredefined', coreType: 'AccessInfo', title: 'Access Info', url: '/BusinessIntelligence/DashboardNReport/GetPartialForEntity', filePath: '~/Areas/Entity/Views/EntityDashboard/_EntityAccessInfo.cshtml', icon: 'fa fa-universal-access', moduleName: 'EMS', renderInMainCanvas: false, RenderInTab: true, actualParams: [{ sP: 'EntityId', cP: 'getDataBaseOnId', vm: true }, { sP: 'FilePath', cP: 'filePath', vm: false }] },
              { type: 'widgetOfPredefined', coreType: 'Location', title: 'Location', url: '/Entity/Entity/EntityLocationPartial', filePath: '~/Areas/Entity/Views/Entity/_EntityLocation.cshtml', icon: 'fa fa-map-marker', moduleName: 'EMS', renderInMainCanvas: false, RenderInTab: true, actualParams: [{ sP: 'EntityId', cP: 'getDataBaseOnId', vm: true }, { sP: 'FilePath', cP: 'filePath', vm: false }] },
               { type: 'widgetOfPredefined', coreType: 'Document', title: 'Document', url: '/DMS/Document/ReturnAttachmentPartialView', filePath: '~/Areas/DMS/Views/Document/_DocumentIntegration.cshtml', moduleId: 14, featureId: 1402, icon: 'fa fa-book', moduleName: 'EMS', renderInMainCanvas: false, RenderInTab: true, actualParams: [{ sP: 'InstanceId', cP: 'getDataBaseOnId', vm: true }, { sP: 'FilePath', cP: 'filePath', vm: false }, { sP: 'moduleId', cP: 'moduleId', vm: false }, { sP: 'featureId', cP: 'featureId', vm: false }] }
        ];
        let getWidgetsByModType = DynamicPriDefined[ModType];
        if (getWidgetsByModType)
            return getWidgetsByModType;
        return [];


    }
    //#endregion

    //#region set column size of the panel
    var handleColumnsSize = function () {
        return { onethird: "col-xl-4 col-lg-12", onesixth: "col-xl-2 col-lg-12", half: "col-xl-6 col-lg-12", twothird: "col-xl-8 col-lg-12", fivesixth: "col-xl-10 col-lg-12", fullwide: "col-xl-12 col-lg-12" };
    };
    //#endregion 

    //#region add new widget in grid stack
    var handleAddNewWidgetInGridStack = function () {
        return $('.grid-stack').data('gridstack');
    };
    //#endregion 

    //#region add db column in setting of default page
    var handleUiOfDbColumnInSettingOfDefaultPage = function () {
        let uiOfDbColumn = `<div class="col-lg-6 col-md-12"><label class="draggable_element"></label></div>`;
        return uiOfDbColumn;
    };
    //#endregion

    //#region add image control in advacnce setting of default page
    var handleUiOfImageControlAdvanceSettingOfDefaultPage = function () {
        let uiOfImageControl = `<div class="col-lg-6 col-md-12"><label class="draggable_element_Image_Control"></label></div>`;
        return uiOfImageControl;
    };
    //#endregion

    //#region add db column in advance setting of default page
    var handleUiOfDbColumnInAdvanceSettingOfDefaultPage = function () {
        let uiOfDbColumn = `<div class="col-lg-6 col-md-12"><label class="draggable_element_dbcolumn_advancesetting"></label></div>`;
        return uiOfDbColumn;
    };
    //#endregion

    //#region set rows and columns for default page
    var handleSetRowsAndColumnsForDefaultPage = function () {
        let setRowsAndColumns = `<div class="col-lg-6 col-md-12"><label class="">Rows</label><input type="text" /></div> <div class="col-lg-6 col-md-12"><label class="">Columns</label><input type="text" /></div>`;
        return setRowsAndColumns;
    };
    //#endregion

    //#region create div base on  
    var handleCreateDivBaseOnParams = function (position, overflow, left, top, right, bottom, backgroundImg) {
        let container = document.createElement('div');
        container.style.position = position;
        container.style.overflow = overflow;
        container.style.left = left;
        container.style.top = top;
        container.style.right = right;
        container.style.bottom = right;
        container.style.background = 'url(' + backgroundImg + ')';
        return container;

    };
    //#endregion

    //#region create img 

    var handleCreateImg = function (image) {
        let img = document.createElement('img');
        img.setAttribute('src', image);
        img.style.width = '16px';
        img.style.height = '16px';
        img.style.verticalAlign = 'middle';
        img.style.marginRight = '2px';
        return img;
    };

    //#endregion

    //#region create icon

    var handlefafaIcon = function (className) {
        let faIcon = document.createElement('i');
        faIcon.className += className;
        return faIcon;
    };

    //#endregion


    //#region create button

    var handleCreateButton = function (img, fafaIcon, className, label, buttonClass) {

        let createButton = document.createElement('button');
        createButton.className += buttonClass;
        if (img) {
            createButton.appendChild(handleCreateImg(img));

        }
        if (fafaIcon) {
            createButton.appendChild(handlefafaIcon(className));
        }
        return createButton;
    };

    //#endregion


    //#region set json for join type

    var handleSetJsonForJoinType = function () {

        let makeJson = [{ key: "INNER", value: "INNER" }, { key: "LEFT", value: "LEFT" }, { key: "RIGHT", value: "RIGHT" }, { key: "FULL OUTER", value: "FULL OUTER" }];
        return makeJson;
    };

    //#endregion

    return {
        stateSettings: function () {
            return handleStateSettings();
        },
        widgetSettings: function () {
            return handleWidgetSettings();
        },
        columnsSize: function () {
            return handleColumnsSize();
        },
        uiOfDbColumnInSettingOfDefaultPage: function () {
            return handleUiOfDbColumnInSettingOfDefaultPage();
        },
        uiOfDbColumnInAdvanceSettingOfDefaultPage: function () {
            return handleUiOfDbColumnInAdvanceSettingOfDefaultPage();
        },
        setRowsAndColumnsForDefaultPage: function () {
            return handleSetRowsAndColumnsForDefaultPage();
        },
        uiOfImageControlAdvanceSettingOfDefaultPage: function () {
            return handleUiOfImageControlAdvanceSettingOfDefaultPage();
        },
        createDivBaseOnParams: function (position, overflow, left, top, right, bottom, backgroundImg) {
            return handleCreateDivBaseOnParams(position, overflow, left, top, right, bottom, backgroundImg);
        },
        getLayoutForDefaultWidget: function () {
            return handleLayoutForDefaultPage();

        },
        getDynamicPredefinedWidget: function (ModType) {
            return handleDynamicPridefinedWidgets(ModType);
        },
        setJsonForJoinType: function () { return handleSetJsonForJoinType(); },
        getAddNewWidgetInGridStack: function () { addWidgetOfGridStackItem = handleAddNewWidgetInGridStack(); },
        getCreateButton: function (img, fafaIcon, className, label, buttonClass) {
            return handleCreateButton(img, fafaIcon, className, label, buttonClass);
        },
        getStandardPanel: function () {
           return handleStandardPanel();
        }


    };
}();

getStateSettings = airviewXcelerateAppSettings.stateSettings();
getWidgetSettings = airviewXcelerateAppSettings.widgetSettings();
getColumnsSize = airviewXcelerateAppSettings.columnsSize();
getUiColumnInSettingOfDefaultPage = airviewXcelerateAppSettings.uiOfDbColumnInSettingOfDefaultPage();
getUiColumnInAdvanceSettingOfDefaultPage = airviewXcelerateAppSettings.uiOfDbColumnInAdvanceSettingOfDefaultPage();
getImageControlInAdvanceSettingOfDefaultPage = airviewXcelerateAppSettings.uiOfImageControlAdvanceSettingOfDefaultPage();
createDynamicDiv = airviewXcelerateAppSettings.createDivBaseOnParams('absolute', 'hidden', '0px', '0px', '0px', '0px', 'assets/mxgraph/examples/editors/images/grid.gif');
getRowsAndColumnsDefaultPage = airviewXcelerateAppSettings.setRowsAndColumnsForDefaultPage();


//#endregion
//{{vm.dashboardAndReportComInformation.widgetList[chngeIdx].widgetConfiguration.sqlQueryBuild}}