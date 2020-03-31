
//#region Global Variables
var mapIndex = 0;
var mapList = [];
var indexOfWidget = 0;
var currentWidgetID = 0;
var makeJsonOfDatabase = {};
var currentIndex = 0;
var requestParams = {
    skipCount: 0,
    maxResultCount: 10,
    sorting: null
};
var uiGridConstantsGloabl = "";
var toastrss = "";
// code start
var isPinColorApplyBtnClicked = false;
// code end
mxGraphEncoderOrDecoder = new mxCodec();
var countt = 0;
//#endregion

//#region Dashboard and Report
airviewXcelerateApp.factory('ETLManipulate', function ($http, $timeout, ivhTreeviewMgr, $uibModal) {

    var dashboardAndReportHandler = function () {

        //#region get unique key
        var handleUniquekey = function (length) {
            var chars = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ', result = "";
            for (var i = length; i > 0; --i)
                result += chars[Math.round(Math.random() * (chars.length - 1))];
            return 'c' + result;
        };
        //#endregion

        //#region Dragged Widget
        var handleDragWidget = function (element, parentElement, settingsAndAdvanceSettingsSelector, settingsElement, advanceSettingsElement, queryBuilderElement, queryBuilerSelector, compile, scope, vm, widgetType, toastrss, parentWidgetID, ChildOfWidgetId) {

            //#region widget object initialization
            let widgetInfo = {};
            widgetInfo.widgetConfiguration = {};
            widgetInfo.widgetConfiguration.ChildOfWidgetId = ChildOfWidgetId;
            widgetInfo.widgetConfiguration.widgetTopLeftWidthAndHeight = {};
            widgetInfo.widgetConfiguration.widgetTopLeftWidthAndHeight.top = 0;
            widgetInfo.widgetConfiguration.widgetTopLeftWidthAndHeight.left = 0;
            widgetInfo.widgetConfiguration.widgetTopLeftWidthAndHeight.width = 6;
            widgetInfo.widgetConfiguration.widgetTopLeftWidthAndHeight.height = 25;
            widgetInfo.widgetConfiguration.sqlQueryBuild = "";
            widgetInfo.widgetConfiguration.historyOfQueryBuilder = [];
            widgetInfo.widgetConfiguration.mapleveltype = "Country";
            widgetInfo.widgetConfiguration.Country = "0";
            widgetInfo.widgetConfiguration.MarketLevel = false;
            widgetInfo.widgetConfiguration.selectedViewName = "";
            widgetInfo.widgetConfiguration.murriHistoryOfSelectedColumns = {};
            widgetInfo.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID = [];
            widgetInfo.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsHtml = [];
            widgetInfo.widgetConfiguration.pagePosition = indexOfWidget;
            widgetInfo.widgetID = handleUniquekey(6);
            widgetInfo.PWidgetID = parentWidgetID;
            widgetInfo.SearchText = '';
            widgetInfo.widgetName = "Panel Name";
            widgetInfo.TemplateNodeID = "";
            widgetInfo.widgetConfiguration.idsInfo = {};
            widgetInfo.widgetConfiguration.idsInfo.DynamicItemID = handleUniquekey(6);
            widgetInfo.widgetConfiguration.classInfo = {};
            //widgetInfo.widgetConfiguration.classInfo.SelectColumnClassForMurri = handleUniquekey(5);
            widgetInfo.widgetConfiguration.classInfo.outerClass = "";
            widgetInfo.widgetConfiguration.classInfo.innerClass = "";
            widgetInfo.widgetConfiguration.styleInfo = {};
            widgetInfo.widgetConfiguration.styleInfo.outerStyle = "#FFF";
            widgetInfo.widgetConfiguration.styleInfo.innerStyle = "#FFF";
            widgetInfo.widgetConfiguration.styleInfo.widgetHeight = 330;
            widgetInfo.widgetConfiguration.styleInfo.contentColorOfWidget = "#FFF";
            widgetInfo.isDeleted = false;
            widgetInfo.widgetType = widgetType;
            widgetInfo.data = [];
            widgetInfo.tempData = [];
            widgetInfo.columnValueForFilteration = [];
            widgetInfo.murriInitializationOfSelectedColumn = [];
            widgetInfo.widgetConfiguration.whereClauseConfiguration = {};
            widgetInfo.widgetConfiguration.whereClauseConfiguration.whereClause = '';
            widgetInfo.widgetConfiguration.whereClauseConfiguration.whereClauseJsonWithRuleAndGroup = JSON.parse(' { "group": { "operator": "AND", "rules": [] } }');
            widgetInfo.widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere = "";
            widgetInfo.widgetConfiguration.whereClauseConfiguration.mapsqlQueryGroupBy = "";
            widgetInfo.widgetConfiguration.whereClauseConfiguration.mapsqlQuerySortBy = "";
            widgetInfo.widgetConfiguration.whereClauseConfiguration.mapsqlQueryColumns = "";
            widgetInfo.widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount = 0;
            widgetInfo.SelectedTablesColumnName = [];
            widgetInfo.widgetConfiguration.classInfo.widgetheader = "";
            widgetInfo.widgetConfiguration.FilterOnKeyValue = {};
            //#region multi schemas designer
            widgetInfo.widgetConfiguration.multiSchemaDesignerModels = {};
            widgetInfo.widgetConfiguration.multiSchemaDesignerModels.currentMultiSelectedViews = [];
            widgetInfo.widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews = [];
            widgetInfo.widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue = [];
            widgetInfo.widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue = [];
            widgetInfo.widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference = {};
            widgetInfo.widgetConfiguration.multiSchemaDesignerModels.joinOperatorType = {};
            widgetInfo.widgetConfiguration.multiSchemaDesignerModels.multiSchemaEnableOrDisable = true;
            widgetInfo.widgetConfiguration.multiSchemaDesignerModels.joinSectoin = "";
            widgetInfo.widgetConfiguration.multiSchemaDesignerModels.subQueriesList = {};
            widgetInfo.widgetConfiguration.multiSchemaDesignerModels.subQueryStatus = false;
            widgetInfo.widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId = undefined;

            widgetInfo.treeviewOfMultiDatasetsWithItsColumns = [];
            widgetInfo.mxModel;
            widgetInfo.mxGraph;
            widgetInfo.mxKeyHandler;
            widgetInfo.mxEdgesStyle;

            //#endregion
            //#endregion

            //#region widget settings
            vm.dashboardAndReportComInformation.widgetList.push(widgetInfo);
            var getLengthOfWidgetList = vm.dashboardAndReportComInformation.widgetList.length - 1;

            var compiledcode = $.parseHTML(element.replace(/chngeIdx/g, getLengthOfWidgetList));
            $(compiledcode).attr("ng-click", 'vm.loadDataOnWidgetClick(' + getLengthOfWidgetList + ')');
            $(compiledcode).attr("id", vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetID);
            $(compiledcode).attr("data-position", "{{" + 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.pagePosition' + "}}");
            //if (widgetType != "widgetOfDocument")
            $(compiledcode).find(".content-wrapper").attr("style", "height:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.styleInfo.widgetHeight' + "}}px" + ";" + "background:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.styleInfo.contentColorOfWidget' + "}}");
            if (widgetInfo.widgetType == "widgetOfMap")
                $(compiledcode).find(".content-wrapper-map").attr("style", "height:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.styleInfo.widgetHeight' + "}}px");
            $(compiledcode).find(".title-bar").attr("style", "background:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.styleInfo.outerStyle' + "}}");
            $(compiledcode).find(".title-bar h3 span.update-name").html("{{" + 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetName' + "}}");
            $(compiledcode).find(".title-bar h3 input").attr("ng-model", 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetName');
            $(compiledcode).find(".title-bar input.bgcolor").attr("ng-model", 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.styleInfo.outerStyle');
            $(compiledcode).find(".buttons-sets a.remove-btn").attr("ng-click", 'vm.removeWidget($event,' + getLengthOfWidgetList + ')');
            $(compiledcode).find(".title-bar").attr('ng-class', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.classInfo.widgetheader');
            $(compiledcode).find(".title-bar input.bgcolor-content-color").attr("ng-model", 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.styleInfo.contentColorOfWidget');



            //#endregion

            //#region set Column size for div not used
            //$(compiledcode).find("ul.column-size a.onethird").attr("ng-click", 'vm.reRenderOrSettingsChange("' + getColumnsSize.onethird + '",' + getLengthOfWidgetList + ')');
            //$(compiledcode).find("ul.column-size a.onesixth").attr("ng-click", 'vm.reRenderOrSettingsChange("' + getColumnsSize.onesixth + '", ' + getLengthOfWidgetList + ')');
            //$(compiledcode).find("ul.column-size a.half").attr("ng-click", 'vm.reRenderOrSettingsChange("' + getColumnsSize.half + '",' + getLengthOfWidgetList + ')');
            //$(compiledcode).find("ul.column-size a.twothird").attr("ng-click", 'vm.reRenderOrSettingsChange("' + getColumnsSize.twothird + '",' + getLengthOfWidgetList + ')');
            //$(compiledcode).find("ul.column-size a.fivesixth").attr("ng-click", 'vm.reRenderOrSettingsChange("' + getColumnsSize.fivesixth + '",' + getLengthOfWidgetList + ')');
            //$(compiledcode).find("ul.column-size a.fullwide").attr("ng-click", 'vm.reRenderOrSettingsChange("' + getColumnsSize.fullwide + '",' + getLengthOfWidgetList + ')');
            //$(compiledcode).find(".widget").parent().attr('ng-class', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.classInfo.outerClass');
            //$(compiledcode).find(".widget").parent().attr('ng-class', '[vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].classInfo.outerClass' + ',' + 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].classInfo.innerClass1]');
            //#endregion

            //#region settings of widget items

            let settings = $.parseHTML(settingsElement.replace(/chngeIdx/g, getLengthOfWidgetList));
            $(settings).addClass(vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetID);
            $(settings).find("input.headerInput").attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.classInfo.widgetheader');
            $(settings).find("input.headerInput").attr('ng-click', 'vm.refreshDashboardReport(' + getLengthOfWidgetList + ')');

            //#region table Setting
            if (widgetType == 'widgetOfTable') {
                widgetInfo.widgetConfiguration.classInfo.border = "";
                widgetInfo.widgetConfiguration.classInfo.simpleStyleClass = "clr";
                widgetInfo.uiGridConfiguration = {};
                widgetInfo.uiGridConfiguration.paginationStatus = true;
                widgetInfo.uiGridConfiguration.paginationParams = {};
                widgetInfo.uiGridConfiguration.paginationParams.orderBy = {};
                widgetInfo.uiGridConfiguration.paginationParams.paginationSize = 10;
                widgetInfo.uiGridConfiguration.paginationParams.pageNumber = 1;
                widgetInfo.uiGridConfiguration.paginationParams.totalItemsOfUiGrid = 0;
                widgetInfo.uiGridConfiguration.uiGridInitailization = {};
                widgetInfo.widgetConfiguration.styleInfo.theadFontSize = 8;
                widgetInfo.widgetConfiguration.styleInfo.trowFontSize = 8;
                $(settings).find("input.borderInput").attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.classInfo.border');
                $(settings).find('.sampleStyle input:eq(0)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.classInfo.simpleStyleClass');
                $(settings).find('.sampleStyle input:eq(1)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.classInfo.simpleStyleClass');
                $(settings).find('.sampleStyle input:eq(2)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.classInfo.simpleStyleClass');
                $(settings).find('.paginationInput').attr('ng-click', 'vm.paginationAction(' + getLengthOfWidgetList + ')');
                $(settings).find('.paginationInput').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].uiGridConfiguration.paginationStatus');
                $(settings).find('.dataPop').attr('ng-click', 'vm.renderGrid(' + getLengthOfWidgetList + ')');

            }
            //#endregion

            //#region map setting
            if (widgetType == "widgetOfMap") {
                $(compiledcode).find(".maps").attr('id', vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetConfiguration.idsInfo.DynamicItemID);
                //  $(compiledcode).find(".maps").css('height', '310px');
            }
            //#endregion

            //#region charts configuration
            if (widgetType == 'widgetOfChart') {
                widgetInfo.chartConfigurations = {};
                widgetInfo.chartConfigurations.chartInitialization = {};
                widgetInfo.chartConfigurations.settings = {};

                //#region settings of the title declaration
                widgetInfo.chartConfigurations.settings.chartTitleSettings = {};
                widgetInfo.chartConfigurations.settings.chartTitleSettings.chartTitleName = "";
                widgetInfo.chartConfigurations.settings.chartTitleSettings.fontSize = 30;
                widgetInfo.chartConfigurations.settings.chartTitleSettings.fontColor = "black";
                widgetInfo.chartConfigurations.settings.chartTitleSettings.fontFamily = "Calibri";
                widgetInfo.chartConfigurations.settings.chartTitleSettings.fontWeight = "normal";
                widgetInfo.chartConfigurations.settings.chartTitleSettings.fontStyle = "normal";
                //#endregion

                //#region settings of the subTitle declaration
                widgetInfo.chartConfigurations.settings.chartSubTitleSettings = {};
                widgetInfo.chartConfigurations.settings.chartSubTitleSettings.chartSubtitleName = "";
                widgetInfo.chartConfigurations.settings.chartSubTitleSettings.fontSize = 22;
                widgetInfo.chartConfigurations.settings.chartSubTitleSettings.fontColor = "black";
                widgetInfo.chartConfigurations.settings.chartSubTitleSettings.fontFamily = "Calibri";
                widgetInfo.chartConfigurations.settings.chartSubTitleSettings.fontWeight = "normal";
                widgetInfo.chartConfigurations.settings.chartSubTitleSettings.fontStyle = "normal";
                //#endregion

                //#region settings of the legend declaration
                widgetInfo.chartConfigurations.settings.legendSettings = {};
                widgetInfo.chartConfigurations.settings.legendSettings.fontSize = 12;
                widgetInfo.chartConfigurations.settings.legendSettings.fontColor = "black";
                widgetInfo.chartConfigurations.settings.legendSettings.fontFamily = "Calibri";
                widgetInfo.chartConfigurations.settings.legendSettings.fontWeight = "normal";
                widgetInfo.chartConfigurations.settings.legendSettings.fontStyle = "normal";
                widgetInfo.chartConfigurations.settings.legendSettings.verticalAlign = "bottom";
                widgetInfo.chartConfigurations.settings.legendSettings.horizontalAlign = "center";
                widgetInfo.chartConfigurations.settings.legendSettings.ShowInLegend = true;
                //#endregion

                //#region settings of the X-axis declaration
                widgetInfo.chartConfigurations.settings.XaxisSettings = {};
                widgetInfo.chartConfigurations.settings.XaxisSettings.title = "";
                widgetInfo.chartConfigurations.settings.XaxisSettings.titleFontSize = 24;
                widgetInfo.chartConfigurations.settings.XaxisSettings.titleFontColor = "black";
                widgetInfo.chartConfigurations.settings.XaxisSettings.titleFontWeight = "normal";
                widgetInfo.chartConfigurations.settings.XaxisSettings.titleFontStyle = "normal";
                widgetInfo.chartConfigurations.settings.XaxisSettings.labelFontColor = "black";
                widgetInfo.chartConfigurations.settings.XaxisSettings.labelFontSize = 12;
                widgetInfo.chartConfigurations.settings.XaxisSettings.labelFontWeight = "normal";
                widgetInfo.chartConfigurations.settings.XaxisSettings.labelFontStyle = "normal";
                widgetInfo.chartConfigurations.settings.XaxisSettings.prefix = "";
                widgetInfo.chartConfigurations.settings.XaxisSettings.suffix = "";

                //#endregion

                //#region settings of the Y-axis declaration
                widgetInfo.chartConfigurations.settings.YaxisSettings = {};
                widgetInfo.chartConfigurations.settings.YaxisSettings.title = "";
                widgetInfo.chartConfigurations.settings.YaxisSettings.titleFontSize = 24;
                widgetInfo.chartConfigurations.settings.YaxisSettings.titleFontColor = "black";
                widgetInfo.chartConfigurations.settings.YaxisSettings.titleFontWeight = "normal";
                widgetInfo.chartConfigurations.settings.YaxisSettings.titleFontStyle = "normal";
                widgetInfo.chartConfigurations.settings.YaxisSettings.labelFontColor = "black";
                widgetInfo.chartConfigurations.settings.YaxisSettings.labelFontSize = 12;
                widgetInfo.chartConfigurations.settings.YaxisSettings.labelFontWeight = "normal";
                widgetInfo.chartConfigurations.settings.YaxisSettings.labelFontStyle = "normal";
                widgetInfo.chartConfigurations.settings.YaxisSettings.prefix = "";
                widgetInfo.chartConfigurations.settings.YaxisSettings.suffix = "";

                //#endregion

                //#region company and chart type declaration
                widgetInfo.chartConfigurations.chartCompanyName = "";
                widgetInfo.chartConfigurations.keycode = "";
                widgetInfo.chartConfigurations.chartTypeName = "";
                widgetInfo.chartConfigurations.chartID = handleUniquekey(6);
                //#endregion

                //#region company and chart type

                //#region binding
                $(settings).find('.companyType').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.keycode');
                $(settings).find('.chartType').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.chartTypeName');
                //#endregion

                //#region events
                $(settings).find('.companyType').attr('ng-change', 'vm.chart.chartCompanyTypeChange(' + getLengthOfWidgetList + ')');
                $(settings).find('.chartType').attr('ng-change', 'vm.chart.chartTypeChange(' + getLengthOfWidgetList + ')');

                //#endregion

                $(settings).find('.dataPop').attr('ng-click', 'vm.renderGrid(' + getLengthOfWidgetList + ')');

                //#endregion

                //#region Title setting

                //#region binding
                $(settings).find('.chartTitle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartTitleSettings.chartTitleName');
                $(settings).find('.titleFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartTitleSettings.fontSize');
                $(settings).find('.titleFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartTitleSettings.fontColor');
                $(settings).find('.titleFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartTitleSettings.fontWeight');
                $(settings).find('.titleFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartTitleSettings.fontStyle');
                $(settings).find('.titleFontFamily').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartTitleSettings.fontFamily');
                $(settings).find('.titleFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartTitleSettings.fontSize');
                //#endregion

                //#region event
                $(settings).find('.chartTitle').attr('ng-blur', ' vm.chart.chartTitleWithBlur(' + getLengthOfWidgetList + ')');
                $(settings).find('.titleFontSize').attr('ng-change', ' vm.chart.settings.chartTitleSettings.fontSize(' + getLengthOfWidgetList + ')');
                $(settings).find('.titleFontColor').attr('ng-change', ' vm.chart.settings.chartTitleSettings.fontColor(' + getLengthOfWidgetList + ')');
                $(settings).find('.titleFontWeight').attr('ng-click', ' vm.chart.settings.chartTitleSettings.fontWeight(' + getLengthOfWidgetList + ')'); $(settings).find('.titleFontStyle').attr('ng-click', ' vm.chart.settings.chartTitleSettings.fontStyle(' + getLengthOfWidgetList + ')');
                $(settings).find('.titleFontFamily').attr('ng-change', ' vm.chart.settings.chartTitleSettings.fontFamily(' + getLengthOfWidgetList + ')');
                //#endregion

                //#endregion

                //#region Subtitle setting

                //#region binding
                $(settings).find('.chartSubtitle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartSubTitleSettings.chartSubtitleName');
                $(settings).find('.subTitleFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartSubTitleSettings.fontSize');
                $(settings).find('.subTitleFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartSubTitleSettings.fontColor');
                $(settings).find('.subTitleFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartSubTitleSettings.fontWeight');
                $(settings).find('.subTitleFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartSubTitleSettings.fontStyle');
                $(settings).find('.subTitleFontFamily').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartSubTitleSettings.fontFamily');
                $(settings).find('.subTitleFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.chartSubTitleSettings.fontSize');
                //#endregion

                //#region event
                $(settings).find('.chartSubtitle').attr('ng-blur', ' vm.chart.chartSubtitleWithBlur(' + getLengthOfWidgetList + ')');
                $(settings).find('.subTitleFontSize').attr('ng-change', ' vm.chart.settings.chartSubTitleSettings.fontSize(' + getLengthOfWidgetList + ')');
                $(settings).find('.subTitleFontColor').attr('ng-change', ' vm.chart.settings.chartSubTitleSettings.fontColor(' + getLengthOfWidgetList + ')');
                $(settings).find('.subTitleFontWeight').attr('ng-click', ' vm.chart.settings.chartSubTitleSettings.fontWeight(' + getLengthOfWidgetList + ')'); $(settings).find('.subTitleFontStyle').attr('ng-click', ' vm.chart.settings.chartSubTitleSettings.fontStyle(' + getLengthOfWidgetList + ')');
                $(settings).find('.subTitleFontFamily').attr('ng-change', ' vm.chart.settings.chartSubTitleSettings.fontFamily(' + getLengthOfWidgetList + ')');
                //#endregion

                //#endregion

                //#region settings of the legend

                //#region binding

                $(settings).find('.legendFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.legendSettings.fontSize');
                $(settings).find('.legendFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.legendSettings.fontColor');
                $(settings).find('.legendFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.legendSettings.fontWeight');
                $(settings).find('.legendFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.legendSettings.fontStyle');
                $(settings).find('.legendFontFamily').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.legendSettings.fontFamily');
                $(settings).find('.legendFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.legendSettings.fontSize');
                $(settings).find('.legendShowInLegend').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.legendSettings.ShowInLegend');
                $(settings).find('.legendHorizontalAlign').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.legendSettings.horizontalAlign');
                $(settings).find('.lengendVerticalAlign').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.legendSettings.verticalAlign');
                //#endregion

                //#region event

                $(settings).find('.legendFontSize').attr('ng-change', ' vm.chart.settings.legendSettings.fontSize(' + getLengthOfWidgetList + ')');
                $(settings).find('.legendFontColor').attr('ng-change', ' vm.chart.settings.legendSettings.fontColor(' + getLengthOfWidgetList + ')');
                $(settings).find('.legendFontWeight').attr('ng-click', ' vm.chart.settings.legendSettings.fontWeight(' + getLengthOfWidgetList + ')'); $(settings).find('.legendFontStyle').attr('ng-click', ' vm.chart.settings.legendSettings.fontStyle(' + getLengthOfWidgetList + ')');
                $(settings).find('.legendFontFamily').attr('ng-change', ' vm.chart.settings.legendSettings.fontFamily(' + getLengthOfWidgetList + ')');
                $(settings).find('.legendShowInLegend').attr('ng-click', ' vm.chart.settings.legendSettings.ShowInLegend(' + getLengthOfWidgetList + ')'); $(settings).find('.legendHorizontalAlign').attr('ng-change', ' vm.chart.settings.legendSettings.horizontalAlign(' + getLengthOfWidgetList + ')');
                $(settings).find('.lengendVerticalAlign').attr('ng-change', ' vm.chart.settings.legendSettings.verticalAlign(' + getLengthOfWidgetList + ')');
                //#endregion

                //#endregion

                //#region settings of the X-axix

                //#region binding

                $(settings).find('.axisXTitle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.title');
                $(settings).find('.axisXTitleFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.titleFontSize');
                $(settings).find('.axisXTitleFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.titleFontSize');
                $(settings).find('.axisXTitleFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.titleFontColor');
                $(settings).find('.axisXTitleFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.titleFontWeight');
                $(settings).find('.axisXTitleFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.titleFontStyle');
                $(settings).find('.axisXLabelFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.labelFontColor');
                $(settings).find('.axisXLabelFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.labelFontSize');
                $(settings).find('.axisXLabelFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.labelFontSize');
                $(settings).find('.axisXLabelFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.labelFontWeight');
                $(settings).find('.axisXLabelFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.labelFontStyle');
                $(settings).find('.axisXPrefix').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.prefix');
                $(settings).find('.axisXSuffix').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.XaxisSettings.suffix');
                //#endregion

                //#region event

                $(settings).find('.axisXTitle').attr('ng-blur', ' vm.chart.settings.XaxisSettings.title(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisXTitleFontSize').attr('ng-change', ' vm.chart.settings.XaxisSettings.titleFontSize(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisXTitleFontColor').attr('ng-change', ' vm.chart.settings.XaxisSettings.titleFontColor(' + getLengthOfWidgetList + ')'); $(settings).find('.axisXTitleFontWeight').attr('ng-click', ' vm.chart.settings.XaxisSettings.titleFontWeight(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisXTitleFontStyle').attr('ng-click', ' vm.chart.settings.XaxisSettings.titleFontStyle(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisXLabelFontColor').attr('ng-change', ' vm.chart.settings.XaxisSettings.labelFontColor(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisXLabelFontSize').attr('ng-change', ' vm.chart.settings.XaxisSettings.labelFontSize(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisXLabelFontWeight').attr('ng-click', ' vm.chart.settings.XaxisSettings.labelFontWeight(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisXLabelFontStyle').attr('ng-click', ' vm.chart.settings.XaxisSettings.labelFontStyle(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisXPrefix').attr('ng-blur', ' vm.chart.settings.XaxisSettings.prefix(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisXSuffix').attr('ng-blur', ' vm.chart.settings.XaxisSettings.suffix(' + getLengthOfWidgetList + ')');
                //#endregion

                //#endregion

                //#region settings of the Y-axix

                //#region binding

                $(settings).find('.axisYTitle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.title');
                $(settings).find('.axisYTitleFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.titleFontSize');
                $(settings).find('.axisYTitleFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.titleFontSize');
                $(settings).find('.axisYTitleFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.titleFontColor');
                $(settings).find('.axisYTitleFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.titleFontWeight');
                $(settings).find('.axisYTitleFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.titleFontStyle');
                $(settings).find('.axisYLabelFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.labelFontColor');
                $(settings).find('.axisYLabelFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.labelFontSize');
                $(settings).find('.axisYLabelFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.labelFontSize');
                $(settings).find('.axisYLabelFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.labelFontWeight');
                $(settings).find('.axisYLabelFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.labelFontStyle');
                $(settings).find('.axisYPrefix').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.prefix');
                $(settings).find('.axisYSuffix').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.settings.YaxisSettings.suffix');
                //#endregion

                //#region event

                $(settings).find('.axisYTitle').attr('ng-blur', ' vm.chart.settings.YaxisSettings.title(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisYTitleFontSize').attr('ng-change', ' vm.chart.settings.YaxisSettings.titleFontSize(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisYTitleFontColor').attr('ng-change', ' vm.chart.settings.YaxisSettings.titleFontColor(' + getLengthOfWidgetList + ')'); $(settings).find('.axisYTitleFontWeight').attr('ng-click', ' vm.chart.settings.YaxisSettings.titleFontWeight(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisYTitleFontStyle').attr('ng-click', ' vm.chart.settings.YaxisSettings.titleFontStyle(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisYLabelFontColor').attr('ng-change', ' vm.chart.settings.YaxisSettings.labelFontColor(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisYLabelFontSize').attr('ng-change', ' vm.chart.settings.YaxisSettings.labelFontSize(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisYLabelFontWeight').attr('ng-click', ' vm.chart.settings.YaxisSettings.labelFontWeight(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisYLabelFontStyle').attr('ng-click', ' vm.chart.settings.YaxisSettings.labelFontStyle(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisYPrefix').attr('ng-blur', ' vm.chart.settings.YaxisSettings.prefix(' + getLengthOfWidgetList + ')');
                $(settings).find('.axisYSuffix').attr('ng-blur', ' vm.chart.settings.YaxisSettings.suffix(' + getLengthOfWidgetList + ')');
                //#endregion

                //#endregion
            }


            //#endregion

            //#region Default Page Configuration
            if (widgetType == 'widgetOfDefault') {

                widgetInfo.defaultPageConfiguration = {};
                widgetInfo.defaultPageConfiguration.dataParams = {};
                widgetInfo.defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration = [];
                widgetInfo.defaultPageConfiguration.customInputControlPropertiesConfiguration = [];
                widgetInfo.defaultPageConfiguration.customImageControlPropertiesConfiguration = [];
                widgetInfo.defaultPageConfiguration.isSingleOrMultiRow = true;
                widgetInfo.defaultPageConfiguration.isColumnRange = 1;
                widgetInfo.fileInformation = [];

            }
            //#endregion

            //#region Document Configuration
            if (widgetType == 'widgetOfDocument') {
                widgetInfo.documentsViewConfiguration = {};
                widgetInfo.documentsViewConfiguration.documentPath = "";

            }
            //#endregion

            //#endregion

            //#region advance settings of widget items
            let advanceSettings = $.parseHTML(advanceSettingsElement.replace(/chngeIdx/g, getLengthOfWidgetList));
            $(advanceSettings).addClass(vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetID);
            $(advanceSettings).attr('data-id', vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetID);
            if (widgetType == 'widgetOfTable') {
                $(advanceSettings).find('.bgcolor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.classInfo.simpleStyleClass');
                $(advanceSettings).find('input.theadFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.styleInfo.theadFontSize');
                $(advanceSettings).find('input.trowFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.styleInfo.trowFontSize');

            }

            //#region advance settings of the map
            if (widgetType == "widgetOfMap") {

                // code start
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(0)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.sourceForMapPinSetting');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(0)').attr('ng-change', 'vm.sourceChangedForMapPinSetting(' + getLengthOfWidgetList + ')');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(1)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.sourceForMapPinSetting');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(1)').attr('ng-change', 'vm.sourceChangedForMapPinSetting(' + getLengthOfWidgetList + ')');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(2)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.sourceForMapPinSetting');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(2)').attr('ng-change', 'vm.sourceChangedForMapPinSetting(' + getLengthOfWidgetList + ')');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(3)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.sourceForMapPinSetting');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(3)').attr('ng-change', 'vm.sourceChangedForMapPinSetting(' + getLengthOfWidgetList + ')');
                // code end


                $(advanceSettings).find('.mapleveltype input:eq(0)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.mapleveltype');
                $(advanceSettings).find('.mapleveltype input:eq(0)').attr('ng-change', 'vm.ChangeMapLevelType(' + getLengthOfWidgetList + ')');
                $(advanceSettings).find('.mapleveltype input:eq(1)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.mapleveltype');
                $(advanceSettings).find('.mapleveltype input:eq(1)').attr('ng-change', 'vm.ChangeMapLevelType(' + getLengthOfWidgetList + ')');
                $(advanceSettings).find('.mapleveltype input:eq(2)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.mapleveltype');
                $(advanceSettings).find('.mapleveltype input:eq(2)').attr('ng-change', 'vm.ChangeMapLevelType(' + getLengthOfWidgetList + ')');
                $(advanceSettings).find('.mapleveltype input:eq(3)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.mapleveltype');
                $(advanceSettings).find('.mapleveltype input:eq(3)').attr('ng-change', 'vm.ChangeMapLevelType(' + getLengthOfWidgetList + ')');
                $(advanceSettings).find('.mapleveltype input:eq(3)').attr('ng-click', 'vm.ClickMarketLevelMap(' + getLengthOfWidgetList + ')');
                $(advanceSettings).find(".maps").attr('id', vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetConfiguration.idsInfo.DynamicItemID);


            }
            //#endregion

            //#region Default Page Configuration
            if (widgetType == 'widgetOfDefault') {
                //let rowsColumns = getRowsAndColumnsDefaultPage;
                //$(advanceSettings).find('.content .add-control').append(getRowsAndColumnsDefaultPage);
                let uiColumnInDefaultPage = $.parseHTML(getUiColumnInAdvanceSettingOfDefaultPage);
                $(uiColumnInDefaultPage).attr('data-type', 'inputcontrol');
                $(uiColumnInDefaultPage).find('.draggable_element_dbcolumn_advancesetting').text('input control');
                $(advanceSettings).find('.content .add-control').append(uiColumnInDefaultPage);
                let uiImageControlInDefaultPage = $.parseHTML(getImageControlInAdvanceSettingOfDefaultPage);
                $(uiImageControlInDefaultPage).attr('data-type', 'imagecontrol');
                $(uiImageControlInDefaultPage).find('.draggable_element_Image_Control').text('Image');
                $(advanceSettings).find('.content .add-control').append(uiImageControlInDefaultPage);


            }
            //#endregion

            //#region charts configuration
            if (widgetType == 'widgetOfChart') {
                widgetInfo.multiSeriesXaixSelect = [];
                widgetInfo.multiSeriesYaixSelect = [];
                widgetInfo.multiSeriesGroupSelect = [];
                widgetInfo.chartConfigurations.Advancesettings = {};
                widgetInfo.chartConfigurations.Advancesettings.xAxis = undefined;
                widgetInfo.chartConfigurations.Advancesettings.yAxis = undefined;
                widgetInfo.chartConfigurations.Advancesettings.seriesOnGroup = undefined;
                widgetInfo.chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot = {};
                let multiSeriesXaxisParams = { xAxisColumnNameMul: undefined };
                let multiSeriesYaxisParams = { yAxisColumnNameMul: undefined, yAxisChartType: undefined, yAxisColorPicker: getRandomColor(), yAxisSecPriType: "secondary", columnTitle: "" };
                widgetInfo.chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis = [];
                widgetInfo.chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis.push(multiSeriesXaxisParams);
                widgetInfo.chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis = [];
                widgetInfo.chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis.push(multiSeriesYaxisParams);
                widgetInfo.chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.seriesOnGroup = undefined;
                $(advanceSettings).find('.series-select-condition').attr('ng-if', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.chartTypeName==' + '"stackedBar" || vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.chartTypeName==' + '"stackedColumn"');
                $(advanceSettings).find('.multi-series-chart').attr('ng-if', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.chartTypeName==' + '"multiseries"');
                $(advanceSettings).find('.single-chart').attr('ng-if', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].chartConfigurations.chartTypeName!=' + '"multiseries"');


            }
            //#endregion

            //#region tab widget configuration
            if (widgetType == 'widgetOfTab') {
                widgetInfo.tabConfiguration = {};
                widgetInfo.tabConfiguration.tabHistoryWithTitleIcon = [];

            }
            //#endregion


            //#endregion

            //#region compile and render section 

            //#region widget compile and render section
            compiledcode = compile(compiledcode)(scope);
            //parentElement.add_widget(compiledcode, widgetInfo.widgetConfiguration.widgetTopLeftWidthAndHeight.left, widgetInfo.widgetConfiguration.widgetTopLeftWidthAndHeight.top, widgetInfo.widgetConfiguration.widgetTopLeftWidthAndHeight.width, widgetInfo.widgetConfiguration.widgetTopLeftWidthAndHeight.height, true);
            //#endregion

            //#region settings of widget compile and render section
            settings = compile(settings)(scope);
            //$(settingsAndAdvanceSettingsSelector).append(settings);
            //#endregion

            //#region advance settings of widget compile and render section
            advanceSettings = compile(advanceSettings)(scope);
            //$(settingsAndAdvanceSettingsSelector).append(advanceSettings);

            //#endregion

            indexOfWidget++;
            //#endregion

            //#region query builder section

            var queryBuilerParse = $.parseHTML(queryBuilderElement.replace(/chngeIdx/g, getLengthOfWidgetList));
            $(queryBuilerParse).addClass(vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetID);
            $(queryBuilerParse).find('.saveQuery').attr('ng-click', 'vm.saveQuery(' + getLengthOfWidgetList + ')');
            $(queryBuilerParse).find('.delete-selected-rows').attr('ng-click', 'vm.DeleteQBSelectedRows(' + getLengthOfWidgetList + ')');
            $(queryBuilerParse).find('.delete-all-rows').attr('ng-click', 'vm.DeleteQBAllRows(' + getLengthOfWidgetList + ')');
            $(queryBuilerParse).find('.querybuilder-datasets').attr('ng-change', 'vm.ChangeDataSetViews(' + getLengthOfWidgetList + ')');
            $(queryBuilerParse).find('.select-all-row').attr('ng-click', 'vm.SelectQBAllRows($event,' + getLengthOfWidgetList + ')');
            $(queryBuilerParse).find('.querybuilder-datasets').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + getLengthOfWidgetList + '].widgetConfiguration.selectedViewName');
            var queryBuilerCompile = compile(queryBuilerParse)(scope);
            $(queryBuilerSelector).append(queryBuilerCompile);
            handleSelectedColumnOfQueryBuilder(vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetID, getLengthOfWidgetList, vm);
            handleInitializationOfMxGraph(vm, getLengthOfWidgetList, scope, compile, $('.' + vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetID + ' .multiSchemaDesignerArea'));

            toastrss.success(widgetType.substr(8) + ' has been created');
            scope.$emit('murri');

            $.when($(".querybuilder-wrapper , .widget-settings,.default_advance_settings ").fadeOut()).done(function () {
                $("." + vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetID).fadeIn();
            });
            //#endregion

            //#region default page draggable and droppable
            if (widgetType == "widgetOfDefault")
                makeDragInputControlOfDefaultPage("." + vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetID + " .content .draggable_element_dbcolumn_advancesetting", "." + vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetID + " .content .draggable_element_Image_Control", vm, getLengthOfWidgetList, scope, compile);
            //#endregion

            //#region default page draggable and droppable
            if (widgetType == "widgetOfTab")
                handleDroppableOnTabWidget(vm, widgetType, compile, scope);
            //#endregion

            if (widgetType == "widgetOfDocument") {
                countt++;
                //console.log(vm.dashboardAndReportComInformation.documentPath);
                scope.$apply();
                //$('.content-wrapper').empty();
                if (countt % 2 == 0) {
                    handleGetDocuments("~/Content/DocumentExcel.xlsx", $('#' + vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetID).find('.document-area'));
                }
                else {
                    handleGetDocuments("~/Content/SamplePPT.ppt", $('#' + vm.dashboardAndReportComInformation.widgetList[getLengthOfWidgetList].widgetID).find('.document-area'));
                }
            }

        };
        //#endregion

        //#region render widget on call
        var handleRenderWidget = function (element, parentElement, settingsAndAdvanceSettingsSelector, settingsElement, advanceSettingsElement, queryBuilderElement, queryBuilerSelector, compile, scope, vm, index, widgetType, uiGridConstants, rootScope, positionAuto) {
            vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.multiSchemaDesignerModels.currentMultiSelectedViews = vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews;
            //#region get color dark or not

            let getRGB = hexToRgb(vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.styleInfo.outerStyle);
            let getDarkRGB = "";
            if (getRGB)
                getDarkRGB = isDark("rgb(" + getRGB.r + "," + getRGB.g + "," + getRGB.b + ")") ? '#fff' : '#4e4e4e';

            //#endregion

            //#region widget settings
            var compiledcode = $.parseHTML(element.replace(/chngeIdx/g, index));
            $(compiledcode).attr("id", vm.dashboardAndReportComInformation.widgetList[index].widgetID);
            $(compiledcode).attr("ng-click", 'vm.loadDataOnWidgetClick(' + index + ')');
            $(compiledcode).attr("data-position", vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.pagePosition);
            $(compiledcode).find(".title-bar").attr("style", "color:" + getDarkRGB + ";" + "background:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.styleInfo.outerStyle' + "}}");
            $(compiledcode).find(".content-wrapper").attr("style", "height:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.styleInfo.widgetHeight' + "}}px" + ";" + "background:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.styleInfo.contentColorOfWidget' + "}}");

            if (widgetType == "widgetOfMap")
                $(compiledcode).find(".content-wrapper-map").attr("style", "height:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.styleInfo.widgetHeight' + "}}px");
            $(compiledcode).find(".title-bar h3 span.update-name").html("{{" + 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetName' + "}}");
            $(compiledcode).find(".title-bar h3 input").attr("ng-model", 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetName');
            $(compiledcode).find(".title-bar input.bgcolor").attr("ng-model", 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.styleInfo.outerStyle');
            $(compiledcode).find(".buttons-sets a.remove-btn").attr("ng-click", 'vm.removeWidget($event,' + index + ')');
            $(compiledcode).find(".title-bar").attr('ng-class', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.classInfo.widgetheader');
            $(compiledcode).find(".title-bar input.bgcolor-content-color").attr("ng-model", 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.styleInfo.contentColorOfWidget');

            //#endregion

            //#region set Column size for div not used
            //$(compiledcode).find("ul.column-size a.onethird").attr("ng-click", 'vm.reRenderOrSettingsChange("' + getColumnsSize.onethird + '",' + index + ')');
            //$(compiledcode).find("ul.column-size a.onesixth").attr("ng-click", 'vm.reRenderOrSettingsChange("' + getColumnsSize.onesixth + '", ' + index + ')');
            //$(compiledcode).find("ul.column-size a.half").attr("ng-click", 'vm.reRenderOrSettingsChange("' + getColumnsSize.half + '",' + index + ')');
            //$(compiledcode).find("ul.column-size a.twothird").attr("ng-click", 'vm.reRenderOrSettingsChange("' + getColumnsSize.twothird + '",' + index + ')');
            //$(compiledcode).find("ul.column-size a.fivesixth").attr("ng-click", 'vm.reRenderOrSettingsChange("' + getColumnsSize.fivesixth + '",' + index + ')');
            //$(compiledcode).find("ul.column-size a.fullwide").attr("ng-click", 'vm.reRenderOrSettingsChange("' + getColumnsSize.fullwide + '",' + index + ')');
            //$(compiledcode).find(".widget").parent().attr('ng-class', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.classInfo.outerClass');
            //#endregion

            //#region settings of widget items
            let settings = $.parseHTML(settingsElement.replace(/chngeIdx/g, index));
            $(settings).addClass(vm.dashboardAndReportComInformation.widgetList[index].widgetID);
            $(settings).find("input.headerInput").attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.classInfo.widgetheader');
            $(settings).find("input.headerInput").attr('ng-click', 'vm.refreshDashboardReport(' + index + ')');
            //#region table Setting
            if (widgetType == 'widgetOfTable') {

                $(settings).find("input.borderInput").attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.classInfo.border');
                $(settings).find('.sampleStyle input:eq(0)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.classInfo.simpleStyleClass');
                $(settings).find('.sampleStyle input:eq(1)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.classInfo.simpleStyleClass');
                $(settings).find('.sampleStyle input:eq(2)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.classInfo.simpleStyleClass');
                $(settings).find('.paginationInput').attr('ng-click', 'vm.paginationAction(' + index + ')');
                $(settings).find('.paginationInput').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].uiGridConfiguration.paginationStatus');
                $(settings).find('.dataPop').attr('ng-click', 'vm.renderGrid(' + index + ')');


            }
            //#endregion

            //#region map setting
            if (widgetType == "widgetOfMap") {
                $(compiledcode).find(".maps").attr('id', vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.idsInfo.DynamicItemID);
                // $(compiledcode).find(".maps").css('height', '310px');
            }


            //#endregion

            //#region charts configuration
            if (widgetType == 'widgetOfChart') {
                //#region company and chart type
                $(settings).find('.companyType').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.keycode');
                $(settings).find('.chartType').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.chartTypeName');
                $(settings).find('.companyType').attr('ng-change', 'vm.chart.chartCompanyTypeChange(' + index + ')');
                $(settings).find('.chartType').attr('ng-change', 'vm.chart.chartTypeChange(' + index + ')');
                $(settings).find('.dataPop').attr('ng-click', 'vm.renderGrid(' + index + ')');
                //#endregion

                //#region Title setting

                //#region binding
                $(settings).find('.chartTitle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartTitleSettings.chartTitleName');
                $(settings).find('.titleFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartTitleSettings.fontSize');
                $(settings).find('.titleFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartTitleSettings.fontColor');
                $(settings).find('.titleFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartTitleSettings.fontWeight');
                $(settings).find('.titleFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartTitleSettings.fontStyle');
                $(settings).find('.titleFontFamily').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartTitleSettings.fontFamily');
                $(settings).find('.titleFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartTitleSettings.fontSize');
                //#endregion

                //#region event
                $(settings).find('.chartTitle').attr('ng-blur', ' vm.chart.chartTitleWithBlur(' + index + ')');
                $(settings).find('.titleFontSize').attr('ng-change', ' vm.chart.settings.chartTitleSettings.fontSize(' + index + ')');
                $(settings).find('.titleFontColor').attr('ng-change', ' vm.chart.settings.chartTitleSettings.fontColor(' + index + ')');
                $(settings).find('.titleFontWeight').attr('ng-click', ' vm.chart.settings.chartTitleSettings.fontWeight(' + index + ')'); $(settings).find('.titleFontStyle').attr('ng-click', ' vm.chart.settings.chartTitleSettings.fontStyle(' + index + ')');
                $(settings).find('.titleFontFamily').attr('ng-change', ' vm.chart.settings.chartTitleSettings.fontFamily(' + index + ')');
                //#endregion

                //#endregion

                //#region Subtitle setting

                //#region binding
                $(settings).find('.chartSubtitle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartSubTitleSettings.chartSubtitleName');
                $(settings).find('.subTitleFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartSubTitleSettings.fontSize');
                $(settings).find('.subTitleFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartSubTitleSettings.fontColor');
                $(settings).find('.subTitleFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartSubTitleSettings.fontWeight');
                $(settings).find('.subTitleFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartSubTitleSettings.fontStyle');
                $(settings).find('.subTitleFontFamily').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartSubTitleSettings.fontFamily');
                $(settings).find('.subTitleFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.chartSubTitleSettings.fontSize');
                //#endregion

                //#region event
                $(settings).find('.chartSubtitle').attr('ng-blur', ' vm.chart.chartSubtitleWithBlur(' + index + ')');
                $(settings).find('.subTitleFontSize').attr('ng-change', ' vm.chart.settings.chartSubTitleSettings.fontSize(' + index + ')');
                $(settings).find('.subTitleFontColor').attr('ng-change', ' vm.chart.settings.chartSubTitleSettings.fontColor(' + index + ')');
                $(settings).find('.subTitleFontWeight').attr('ng-click', ' vm.chart.settings.chartSubTitleSettings.fontWeight(' + index + ')'); $(settings).find('.subTitleFontStyle').attr('ng-click', ' vm.chart.settings.chartSubTitleSettings.fontStyle(' + index + ')');
                $(settings).find('.subTitleFontFamily').attr('ng-change', ' vm.chart.settings.chartSubTitleSettings.fontFamily(' + index + ')');
                //#endregion

                //#endregion

                //#region settings of the legend

                //#region binding

                $(settings).find('.legendFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.legendSettings.fontSize');
                $(settings).find('.legendFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.legendSettings.fontColor');
                $(settings).find('.legendFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.legendSettings.fontWeight');
                $(settings).find('.legendFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.legendSettings.fontStyle');
                $(settings).find('.legendFontFamily').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.legendSettings.fontFamily');
                $(settings).find('.legendFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.legendSettings.fontSize');
                $(settings).find('.legendShowInLegend').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.legendSettings.ShowInLegend');
                $(settings).find('.legendHorizontalAlign').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.legendSettings.horizontalAlign');
                $(settings).find('.lengendVerticalAlign').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.legendSettings.verticalAlign');
                //#endregion

                //#region event

                $(settings).find('.legendFontSize').attr('ng-change', ' vm.chart.settings.legendSettings.fontSize(' + index + ')');
                $(settings).find('.legendFontColor').attr('ng-change', ' vm.chart.settings.legendSettings.fontColor(' + index + ')');
                $(settings).find('.legendFontWeight').attr('ng-click', ' vm.chart.settings.legendSettings.fontWeight(' + index + ')'); $(settings).find('.legendFontStyle').attr('ng-click', ' vm.chart.settings.legendSettings.fontStyle(' + index + ')');
                $(settings).find('.legendFontFamily').attr('ng-change', ' vm.chart.settings.legendSettings.fontFamily(' + index + ')');
                $(settings).find('.legendShowInLegend').attr('ng-click', ' vm.chart.settings.legendSettings.ShowInLegend(' + index + ')'); $(settings).find('.legendHorizontalAlign').attr('ng-change', ' vm.chart.settings.legendSettings.horizontalAlign(' + index + ')');
                $(settings).find('.lengendVerticalAlign').attr('ng-change', ' vm.chart.settings.legendSettings.verticalAlign(' + index + ')');
                //#endregion

                //#endregion

                //#region settings of the X-axix

                //#region binding

                $(settings).find('.axisXTitle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.title');
                $(settings).find('.axisXTitleFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.titleFontSize');
                $(settings).find('.axisXTitleFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.titleFontSize');
                $(settings).find('.axisXTitleFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.titleFontColor');
                $(settings).find('.axisXTitleFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.titleFontWeight');
                $(settings).find('.axisXTitleFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.titleFontStyle');
                $(settings).find('.axisXLabelFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.labelFontColor');
                $(settings).find('.axisXLabelFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.labelFontSize');
                $(settings).find('.axisXLabelFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.labelFontSize');
                $(settings).find('.axisXLabelFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.labelFontWeight');
                $(settings).find('.axisXLabelFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.labelFontStyle');
                $(settings).find('.axisXPrefix').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.prefix');
                $(settings).find('.axisXSuffix').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.XaxisSettings.suffix');
                //#endregion

                //#region event

                $(settings).find('.axisXTitle').attr('ng-blur', ' vm.chart.settings.XaxisSettings.title(' + index + ')');
                $(settings).find('.axisXTitleFontSize').attr('ng-change', ' vm.chart.settings.XaxisSettings.titleFontSize(' + index + ')');
                $(settings).find('.axisXTitleFontColor').attr('ng-change', ' vm.chart.settings.XaxisSettings.titleFontColor(' + index + ')'); $(settings).find('.axisXTitleFontWeight').attr('ng-click', ' vm.chart.settings.XaxisSettings.titleFontWeight(' + index + ')');
                $(settings).find('.axisXTitleFontStyle').attr('ng-click', ' vm.chart.settings.XaxisSettings.titleFontStyle(' + index + ')');
                $(settings).find('.axisXLabelFontColor').attr('ng-change', ' vm.chart.settings.XaxisSettings.labelFontColor(' + index + ')');
                $(settings).find('.axisXLabelFontSize').attr('ng-change', ' vm.chart.settings.XaxisSettings.labelFontSize(' + index + ')');
                $(settings).find('.axisXLabelFontWeight').attr('ng-click', ' vm.chart.settings.XaxisSettings.labelFontWeight(' + index + ')');
                $(settings).find('.axisXLabelFontStyle').attr('ng-click', ' vm.chart.settings.XaxisSettings.labelFontStyle(' + index + ')');
                $(settings).find('.axisXPrefix').attr('ng-blur', ' vm.chart.settings.XaxisSettings.prefix(' + index + ')');
                $(settings).find('.axisXSuffix').attr('ng-blur', ' vm.chart.settings.XaxisSettings.suffix(' + index + ')');
                //#endregion

                //#endregion

                //#region settings of the Y-axix

                //#region binding

                $(settings).find('.axisYTitle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.title');
                $(settings).find('.axisYTitleFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.titleFontSize');
                $(settings).find('.axisYTitleFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.titleFontSize');
                $(settings).find('.axisYTitleFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.titleFontColor');
                $(settings).find('.axisYTitleFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.titleFontWeight');
                $(settings).find('.axisYTitleFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.titleFontStyle');
                $(settings).find('.axisYLabelFontColor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.labelFontColor');
                $(settings).find('.axisYLabelFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.labelFontSize');
                $(settings).find('.axisYLabelFontSize').attr('ionfrom', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.labelFontSize');
                $(settings).find('.axisYLabelFontWeight').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.labelFontWeight');
                $(settings).find('.axisYLabelFontStyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.labelFontStyle');
                $(settings).find('.axisYPrefix').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.prefix');
                $(settings).find('.axisYSuffix').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.settings.YaxisSettings.suffix');
                //#endregion

                //#region event

                $(settings).find('.axisYTitle').attr('ng-blur', ' vm.chart.settings.YaxisSettings.title(' + index + ')');
                $(settings).find('.axisYTitleFontSize').attr('ng-change', ' vm.chart.settings.YaxisSettings.titleFontSize(' + index + ')');
                $(settings).find('.axisYTitleFontColor').attr('ng-change', ' vm.chart.settings.YaxisSettings.titleFontColor(' + index + ')'); $(settings).find('.axisYTitleFontWeight').attr('ng-click', ' vm.chart.settings.YaxisSettings.titleFontWeight(' + index + ')');
                $(settings).find('.axisYTitleFontStyle').attr('ng-click', ' vm.chart.settings.YaxisSettings.titleFontStyle(' + index + ')');
                $(settings).find('.axisYLabelFontColor').attr('ng-change', ' vm.chart.settings.YaxisSettings.labelFontColor(' + index + ')');
                $(settings).find('.axisYLabelFontSize').attr('ng-change', ' vm.chart.settings.YaxisSettings.labelFontSize(' + index + ')');
                $(settings).find('.axisYLabelFontWeight').attr('ng-click', ' vm.chart.settings.YaxisSettings.labelFontWeight(' + index + ')');
                $(settings).find('.axisYLabelFontStyle').attr('ng-click', ' vm.chart.settings.YaxisSettings.labelFontStyle(' + index + ')');
                $(settings).find('.axisYPrefix').attr('ng-blur', ' vm.chart.settings.YaxisSettings.prefix(' + index + ')');
                $(settings).find('.axisYSuffix').attr('ng-blur', ' vm.chart.settings.YaxisSettings.suffix(' + index + ')');
                //#endregion

                //#endregion

            }


            //#endregion

            if (widgetType == 'widgetOfDefault') {

                //code
            }
            //#endregion

            //#region advance settings of widget items
            let advanceSettings = $.parseHTML(advanceSettingsElement.replace(/chngeIdx/g, index));
            $(advanceSettings).addClass(vm.dashboardAndReportComInformation.widgetList[index].widgetID);
            $(advanceSettings).attr('data-id', vm.dashboardAndReportComInformation.widgetList[index].widgetID);
            if (widgetType == 'widgetOfTable') {
                $(advanceSettings).find('.bgcolor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.classInfo.simpleStyleClass');
                $(advanceSettings).find('input.theadFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.styleInfo.theadFontSize');
                $(advanceSettings).find('input.trowFontSize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.styleInfo.trowFontSize');
                $(advanceSettings).find('input.theadFontSize').attr('ionfrom', vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.styleInfo.theadFontSize);
                $(advanceSettings).find('input.trowFontSize').attr('ionfrom', vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.styleInfo.trowFontSize);

            }
            //#region advance setting of the map
            if (widgetType == "widgetOfMap") {
                // code start
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(0)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.sourceForMapPinSetting');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(0)').attr('ng-change', 'vm.sourceChangedForMapPinSetting(' + index + ')');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(1)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.sourceForMapPinSetting');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(1)').attr('ng-change', 'vm.sourceChangedForMapPinSetting(' + index + ')');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(2)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.sourceForMapPinSetting');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(2)').attr('ng-change', 'vm.sourceChangedForMapPinSetting(' + index + ')');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(3)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.sourceForMapPinSetting');
                $(advanceSettings).find('.sourceForMapPinSetting input:eq(3)').attr('ng-change', 'vm.sourceChangedForMapPinSetting(' + index + ')');
                // code end

                $(advanceSettings).find('.mapleveltype input:eq(0)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.mapleveltype');
                $(advanceSettings).find('.mapleveltype input:eq(0)').attr('ng-change', 'vm.ChangeMapLevelType(' + index + ')');
                $(advanceSettings).find('.mapleveltype input:eq(1)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.mapleveltype');
                $(advanceSettings).find('.mapleveltype input:eq(1)').attr('ng-change', 'vm.ChangeMapLevelType(' + index + ')');
                $(advanceSettings).find('.mapleveltype input:eq(2)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.mapleveltype');
                $(advanceSettings).find('.mapleveltype input:eq(2)').attr('ng-change', 'vm.ChangeMapLevelType(' + index + ')');
                $(advanceSettings).find('.mapleveltype input:eq(3)').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.mapleveltype');
                $(advanceSettings).find('.mapleveltype input:eq(3)').attr('ng-change', 'vm.ChangeMapLevelType(' + index + ')');
                $(advanceSettings).find('.mapleveltype input:eq(3)').attr('ng-click', 'vm.ClickMarketLevelMap(' + index + ')');

            }
            //#endregion

            //#region advance setting of the default page
            if (widgetType == 'widgetOfDefault') {
                //let rowsColumns = getRowsAndColumnsDefaultPage;
                //$(advanceSettings).find('.content .add-control').append(getRowsAndColumnsDefaultPage);
                let uiColumnInDefaultPage = $.parseHTML(getUiColumnInAdvanceSettingOfDefaultPage);
                $(uiColumnInDefaultPage).attr('data-type', 'inputcontrol');
                $(uiColumnInDefaultPage).find('.draggable_element_dbcolumn_advancesetting').text('input control');
                $(advanceSettings).find('.content .add-control').append(uiColumnInDefaultPage);
                let uiImageControlInDefaultPage = $.parseHTML(getImageControlInAdvanceSettingOfDefaultPage);
                $(uiImageControlInDefaultPage).attr('data-type', 'imagecontrol');
                $(uiImageControlInDefaultPage).find('.draggable_element_Image_Control').text('Image');
                $(advanceSettings).find('.content .add-control').append(uiImageControlInDefaultPage);
            }
            //#endregion

            //#region charts configuration
            if (widgetType == 'widgetOfChart') {
                $(advanceSettings).find('.series-select-condition').attr('ng-if', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.chartTypeName==' + '"stackedBar" || vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.chartTypeName==' + '"stackedColumn"');
                $(advanceSettings).find('.multi-series-chart').attr('ng-if', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.chartTypeName==' + '"multiseries"');
                $(advanceSettings).find('.single-chart').attr('ng-if', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].chartConfigurations.chartTypeName!=' + '"multiseries"');
                handleAssignXaxisYaxisSeries(vm, index);

            }
            //#endregion
            //#endregion

            //#region compile and render section 

            //#region widget compile and render section
            compiledcode = compile(compiledcode)(scope);
            //parentElement.add_widget(compiledcode, vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.widgetTopLeftWidthAndHeight.left, vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.widgetTopLeftWidthAndHeight.top, vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.widgetTopLeftWidthAndHeight.width, vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.widgetTopLeftWidthAndHeight.height, false);
            //#endregion

            // console.log(vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration);

            //#region settings of widget compile and render section
            settings = compile(settings)(scope);
            //$(settingsAndAdvanceSettingsSelector).append(settings);

            //#endregion

            //#region advance settings of widget compile and render section
            advanceSettings = compile(advanceSettings)(scope);
            //$(settingsAndAdvanceSettingsSelector).append(advanceSettings);
            //#endregion
            //#endregion

            //#region query builder section
            var queryBuilerParse = $.parseHTML(queryBuilderElement.replace(/chngeIdx/g, index));
            $(queryBuilerParse).addClass(vm.dashboardAndReportComInformation.widgetList[index].widgetID);
            $(queryBuilerParse).find('.saveQuery').attr('ng-click', 'vm.saveQuery(' + index + ')');
            $(queryBuilerParse).find('.delete-selected-rows').attr('ng-click', 'vm.DeleteQBSelectedRows(' + index + ')');
            $(queryBuilerParse).find('.delete-all-rows').attr('ng-click', 'vm.DeleteQBAllRows(' + index + ')');

            $(queryBuilerParse).find('.querybuilder-datasets').attr('ng-change', 'vm.ChangeDataSetViews(' + index + ')');
            $(queryBuilerParse).find('.select-all-row').attr('ng-click', 'vm.SelectQBAllRows($event,' + index + ')');
            $(queryBuilerParse).find('.querybuilder-datasets').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + index + '].widgetConfiguration.selectedViewName');
            var queryBuilerCompile = compile(queryBuilerParse)(scope);
            $(queryBuilerSelector).append(queryBuilerCompile);
            handleSelectedColumnOfQueryBuilder(vm.dashboardAndReportComInformation.widgetList[index].widgetID, index, vm);
            handleViewColumns(scope, compile, vm, index, rootScope);
            handleInitializationOfMxGraph(vm, index, scope, compile, $('.' + vm.dashboardAndReportComInformation.widgetList[index].widgetID + ' .multiSchemaDesignerArea'));
            //#endregion

            //#region rerender all widgets
            $timeout(function () {
                if (vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.length > 0) {
                    // console.log(vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID);
                    $.each(vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID, function (i, item) {

                        $('#' + item).prop('checked', true);
                        //console.log(item);
                    });

                }




            }, 1000);
            $timeout(function () {
                handleReloadingOfMurriSelectedColumns(vm, index, scope, compile);
                handleReloadingTreeviewItsColumns(vm, index, scope, compile);

            });


            $(".querybuilder-wrapper,.side-panel .widget-settings,.default_advance_settings").fadeOut();
            $("." + vm.dashboardAndReportComInformation.widgetList[index].widgetID).fadeIn();


            if (vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.pagePosition >= indexOfWidget) {
                indexOfWidget = vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.pagePosition + 1;
            }
            switch (widgetType) {
                case 'widgetOfTab':
                    handleDrawChildWidgetsIfExist(compiledcode, vm, index, compile, scope, uiGridConstants, rootScope);
                    handleDroppableOnTabWidget(vm, widgetType, compile, scope);
                    //$timeout(function () {
                    //    $('#cyC9G3v ul.nav li:first').find('a').addClass('active');
                    //    $('#cyC9G3v ul.nav').each(function () {
                    //        $(this).find('select').hide();
                    //        $(this).find('input').hide();
                    //        $(this).find('a.nav-link').show();
                    //        $(this).find('span.done-tab').hide();
                    //        $(this).find('span.delete-tab').hide();


                    //    });
                    //    $('#cyC9G3v-cVmzpjn').addClass('show active')
                    //}, 50);

                    break;
                case 'widgetOfTable':
                    handleCheckRenderTableOrUiGrid(compile, scope, vm, uiGridConstants, index, false);
                    break;
                case 'widgetOfChart':
                    vm.dashboardAndReportComInformation.widgetList[index].chartConfigurations.chartInitialization = {};
                    setTimeout(function () { handleChartFromDb(compile, scope, vm, uiGridConstants, index, false); }, 300);
                    break;
                case 'widgetOfMap':
                    handleMaps(compile, scope, vm, index, false);
                    break;
                case 'widgetOfDocument':
                    //handleMaps(compile, scope, vm, index, false);
                    break;
                case 'widgetOfDefault':
                    handleLoadColumnsInSettingsOfDefaultPage(vm, index, scope, compile);
                    hanldeDefaultPage(compile, scope, vm, index, false);

                    makeDragInputControlOfDefaultPage("." + vm.dashboardAndReportComInformation.widgetList[index].widgetID + " .content .draggable_element_dbcolumn_advancesetting", "." + vm.dashboardAndReportComInformation.widgetList[index].widgetID + " .content .draggable_element_Image_Control", vm, index, scope, compile);
                    let getLengthDataMember = vm.dashboardAndReportComInformation.widgetList[index].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration.length;
                    let getLengthInputControl = vm.dashboardAndReportComInformation.widgetList[index].defaultPageConfiguration.customInputControlPropertiesConfiguration.length;
                    let getLengthImageControl = vm.dashboardAndReportComInformation.widgetList[index].defaultPageConfiguration.customImageControlPropertiesConfiguration.length;
                    //#region here is checking that content wrapper is empty or not if is not empty then should be empty otherwise already empty
                    if (getLengthDataMember > 0 || getLengthInputControl > 0 || getLengthImageControl > 0) {

                        if ($('#' + vm.dashboardAndReportComInformation.widgetList[index].widgetID + " .widget").find(".content-wrapper .center-icon").length > 0) {
                            $('#' + vm.dashboardAndReportComInformation.widgetList[index].widgetID + " .widget").find('.content-wrapper').empty();
                        }


                    }
                    //#endregion

                    $timeout(function () {

                        if (getLengthDataMember > 0) {

                            for (let dIndex = 0; dIndex < getLengthDataMember; dIndex++) {

                                handleBindDbColumnsSetting(vm, index, scope, compile, '.' + vm.dashboardAndReportComInformation.widgetList[index].widgetID + '.default_settings .db-label-settings .toggle', vm.dashboardAndReportComInformation.widgetList[index].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[dIndex].text, dIndex, $("#" + vm.dashboardAndReportComInformation.widgetList[index].widgetID + " .widget"), false);
                            }
                            handleDrawDataRowsColumnsFormWithDefaultPage(vm, index, scope, compile);
                        }
                    }, 100);
                    //#region binding db columns setting in right side  and redraggable 
                    $timeout(function () {

                        if (getLengthInputControl > 0) {

                            for (let dIndex = 0; dIndex < getLengthInputControl; dIndex++) {
                                handleBindInputControlInAdvanceSetting(vm, index, scope, compile, dIndex, $("#" + vm.dashboardAndReportComInformation.widgetList[index].widgetID + " .widget"));

                            }
                        }
                        if (getLengthImageControl > 0) {

                            for (let dIndex = 0; dIndex < getLengthImageControl; dIndex++) {
                                handleBindImageControlInAdvanceSetting(vm, index, scope, compile, dIndex, $("#" + vm.dashboardAndReportComInformation.widgetList[index].widgetID + " .widget"));

                            }
                        }

                    }, 100);
                    //#endregion



                    break;
                default:
            }


            //#endregion

            // scope.$emit('murri');
            currentIndex = index;
            if (vm.dashboardAndReportComInformation.widgetList[index].widgetType == "widgetOfDocument") {
                countt++;
                if (countt % 2 == 0) {
                    handleGetDocuments("~/Content/DocumentExcel.xlsx", $('#' + vm.dashboardAndReportComInformation.widgetList[index].widgetID).find('.document-area'));
                }
                else
                    handleGetDocuments("~/Content/SamplePPT.ppt", $('#' + vm.dashboardAndReportComInformation.widgetList[index].widgetID).find('.document-area'));
            }
        };
        //#endregion

        //#region widget handler Setting
        var addWidgerHandler = function (scope, compile, vm, toastrs) {
            let _this = this;

            //#region draggable panel box
            $(".panel-box").draggable({
                start: function (event, ui) {
                },
                stop: function (event, ui) {
                    $(".panel-box.ui-draggable-dragging").fadeOut(500, function () {
                        $(this).animate({
                            left: 0,
                            top: 0
                        }, 500, function () {
                            $(this).fadeIn(500);
                        });
                    });
                }
            });
            //#endregion

            //#region droppable panel box on new template only
            $(".new-template").droppable({
                accept: ".panel-box",
                drop: function (event, ui) {
                    let panelType = $(ui.draggable).attr('data-type');
                    let getPanelCodeBaseOnWidgetTypeElement = getWidgetSettings.find(x => x.type == panelType);
                    let getQueryBuilderElement = getWidgetSettings.find(x => x.type == "queryBuilder").code;
                    handleDragWidget(getPanelCodeBaseOnWidgetTypeElement.code, addWidgetOfGridStackItem, $(".side-panel .tab-content .tab-pane > .toggle"), getPanelCodeBaseOnWidgetTypeElement.settings, getPanelCodeBaseOnWidgetTypeElement.advanced_settings, getQueryBuilderElement, $(".side-panel #queryBuilderSection"), compile, scope, vm, panelType, toastrs, null, "-1");
                    scope.$apply();

                }
            });
            //#endregion

            //#region resize callback of grid stack

            $('.grid-stack').on('gsresizestop', function (event, element) {
                if (element != undefined) {
                    if (event != undefined) {
                        var widgetIndex = Number($(element).attr("data-widgetIndex"));
                        var parentHeight = $(element).find('.content-wrapper').height();
                        console.log(parentHeight);
                        // $(".widget").find(".content-wrapper").css({ "height": parentHeight });
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.styleInfo.widgetHeight = (parentHeight);
                        handlereSizeAllWidget(compile, scope, vm, widgetIndex, uiGridConstantsGloabl, $timeout);
                    }
                }
            });

            //#endregion






        };
        //#endregion

        //#region jquery events initialization

        var handlejQueryEventsInitialization = function (scope, compile, vm, toastrs) {
            //#region click event on widget

            $(".new-template").on("click", ".widget", function (e) {
                //  e.preventDefault();
                var wgtSettingsID = $(this).parents('.grid-stack-item').attr('id');
                let widgetIndex = Number($(this).parents('.grid-stack-item').attr('data-widgetindex'));
                if (currentWidgetID != wgtSettingsID) {
                    //if (!vm.dashboardAndReportComInformation.widgetList[widgetIndex].isDeleted) {
                    $(this).parent().parent().addClass("change-settings").siblings().removeClass("change-settings");
                    $.when($(".querybuilder-wrapper , .widget-settings,.default_advance_settings ").fadeOut()).done(function () {
                        $("." + wgtSettingsID).fadeIn();
                    });
                    //$.when($(".side-panel ").fadeOut()).done(function () {
                    //    $("." + wgtSettingsID).fadeIn();
                    //});
                    currentWidgetID = wgtSettingsID;
                    //}
                }
                // e.preventDefault();
                //e.stopPropagation();
            });
            //#endregion

            //#region Switch QueryBuilder To Single Query or Multi Query 
            $("body").on("change", ".switch-qb", function () {
                let activeBuilder = $(".active .querybuilder-wrapper");
                if ($(this).prop('checked') == true) {
                    activeBuilder.find('.single-query').hide();
                    activeBuilder.find('.multi-query').show();
                } else {
                    activeBuilder.find('.single-query').show();
                    activeBuilder.find('.multi-query').hide();
                }
            });
            //#endregion

            //#region Schema Switch

            $("body").on("change", "input.m_switch_check", function () {
                if ($(this)[0].className != "query-swith-check m_switch_check") {
                if ($(this).prop('checked') == true) {
                    $(this).parents('.querybuilder-inner').find('.airview-schema-designer').addClass('active');
                } else {
                    $(this).parents('.querybuilder-inner').find('.airview-schema-designer').removeClass('active');
                }
                }
            });
            //#endregion

            //#region query Switch

            $("body").on("change", ".query-show-hide input.query-swith-check", function () {
                if ($(this)[0].className == "query-swith-check m_switch_check") {
                    if ($(this).prop('checked') == true) {
                        $('.querybuilder-wrapper ').css('padding-bottom', '150px')
                        $('.airview-schema-designer ').css('height', 'calc(100% - 150px)')
                        $('.query-area').css('display', 'block')
                    } else {
                        $('.querybuilder-wrapper ').css('padding-bottom', '0px')
                        $('.airview-schema-designer ').css('height', 'calc(100% - 0px)')
                        $('.query-area').css('display', 'none')
                    }
                }
            });
            //#endregion
        };

        //#endregion

        //#region handleDroppableOnTabWidget

        var handleDroppableOnTabWidget = function (vm, panelType, compile, scope) {
            var $widgetList = getWidgetSettings;
            $(".new-template").find('.content-wrapper.draggableTabPanelBox').droppable({
                accept: ".panel-box",
                greedy: true,
                drop: function (event, ui) {
                    var widget = $(this);
                    $panelType = $(".panel-box.ui-draggable-dragging").attr("data-type");
                    $(this).find('.tab-pane.active').children('.center-icon').detach();// change requirement
                    var tabInstance = $('#' + currentWidgetID).find('.tab-canvas.active .grid-stack'); //Change requirement
                    if (tabInstance.length != 0) {
                        var grids = $(tabInstance).data('gridstack');
                        $.each($widgetList, function (i, e) {
                            if ($widgetList[i]["type"] == $panelType) {
                                if ($widgetList[i]["type"] == "widgetOfTab") {
                                    toastrs.error("Tab Widget can't be used in tab.");
                                } else {
                                    handleDragWidget($widgetList[i]["code"], grids, $(".side-panel .tab-content .tab-pane > .toggle"), $widgetList[i]["settings"], $widgetList[i]["advanced_settings"], $widgetList.find(x => x.type == "queryBuilder").code, $(".side-panel #queryBuilderSection"), compile, scope, vm, $panelType, toastrs, null, currentWidgetID);
                                    scope.$apply();
                                }
                            }
                        });
                    } else {
                        toastrs.error("Please select a Tab fisrt to drag a widget.");
                    }


                }
            });
        };
        //#endregion

        //#region start code
        var handleWidgetCloneJSON = function (WigetIndex, scope, compile, vm) {
            //console.log(vm.dashboardAndReportComInformation.widgetList[WigetIndex]);
            var widgetJson = jQuery.extend(true, {}, vm.dashboardAndReportComInformation.widgetList[WigetIndex]);
            widgetJson.widgetID = handleUniquekey(6);
            widgetJson.widgetConfiguration.idsInfo.DynamicItemID = handleUniquekey(6);
            widgetJson.TemplateNodeID = null;
            widgetJson.widgetConfiguration.pagePosition = generatePagePositionForWidget(vm);
            switch (widgetJson.widgetType) {
                case 'widgetOfMap':
                    handleSelectedColumnsIDAndHtmlForClonedWidget(widgetJson);
                    return widgetJson;
                case 'widgetOfTable':
                    handleSelectedColumnsIDAndHtmlForClonedWidget(widgetJson);
                    return widgetJson;
                case 'widgetOfChart':
                    widgetJson.chartConfigurations.chartID = handleUniquekey(6);
                    handleSelectedColumnsIDAndHtmlForClonedWidget(widgetJson);
                    return widgetJson;
                case 'widgetOfDefault':
                    handleSelectedColumnsIDAndHtmlForClonedWidget(widgetJson);
                    return widgetJson;
                default:
                    break;
            }
        };
        //#endregion end code

        //#region start code
        var handleSelectedColumnsIDAndHtmlForClonedWidget = function (widgetJson) {
            if (widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID != undefined) {
                if (widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.length > 0) {
                    for (var i = 0; i < widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.length; i++) {
                        widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID[i] = widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID[i].substring(0, widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID[i].length - 7) + widgetJson.widgetID;
                    }
                }
            }

            if (widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsHtml != undefined) {
                if (widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsHtml.length == 0) {
                    widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsHtml = [];
                    let allItemsOfWidget = widgetJson.murriInitializationOfSelectedColumn.getItems();
                    if (allItemsOfWidget.length > 0) {
                        for (var x = 0; x < allItemsOfWidget.length; x++) {
                            widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsHtml.push(allItemsOfWidget[x]._element.outerHTML);
                            widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsHtml[x] = regenerateUniqueIdForSelectedColsHtml(widgetJson, x);
                        }
                    }
                } else {
                    for (var i = 0; i < widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsHtml.length; i++) {
                        widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsHtml[i] = regenerateUniqueIdForSelectedColsHtml(widgetJson, i);
                    }
                }
            }
        };
        //#endregion end code

        //#region start code
        var regenerateUniqueIdForSelectedColsHtml = function (widgetJson, index) {
            var $row = $(widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsHtml[index]);
            $row = $row.attr('id', widgetJson.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID[index]);
            return $row[0].outerHTML.toString();
        };
        //#endregion end code

        //#region start code
        var generatePagePositionForWidget = function (vm) {
            return Number(vm.dashboardAndReportComInformation.widgetList[vm.dashboardAndReportComInformation.widgetList.length - 1].widgetConfiguration.pagePosition) + 1;
        };
        //#endregion end code

        //#region Draw Widgets
        var handleDrawWidgets = function (compile, scope, vm, callback, uiGridConstants, rootScope) {
            mapIndex = 0;
            mapList = [];
            indexOfWidget = 0;
            let addMurriMainDiv = "<div class='grid-stack' data-pagename='" + vm.dashboardAndReportComInformation.templateTitle + "'></div>";
            $(".new-template").addClass("start-drawing").append(addMurriMainDiv);
            xl_script.initGridStack();
            airviewXcelerateAppSettings.getAddNewWidgetInGridStack();
            // scope.$emit('murri');
            let getLengthOfWidgetList = vm.dashboardAndReportComInformation.widgetList.length;
            for (let index = 0; index < getLengthOfWidgetList; index++) {
                // let parentElement = $(".new-template .page");
                let settingsAndAdvanceSettingsSelector = $(".side-panel .tab-content .tab-pane > .toggle");
                let settingsElement = getWidgetSettings.find(x => x.type == vm.dashboardAndReportComInformation.widgetList[index].widgetType).settings;
                let advanceSettingsElement = getWidgetSettings.find(x => x.type == vm.dashboardAndReportComInformation.widgetList[index].widgetType).advanced_settings;
                let element = getWidgetSettings.find(x => x.type == vm.dashboardAndReportComInformation.widgetList[index].widgetType).code;
                var queryBuilerEle = getWidgetSettings.find(x => x.type == "queryBuilder").code;
                //if (!vm.dashboardAndReportComInformation.widgetList[index].isDeleted) {
                //    handleRenderWidget(element, addWidgetOfGridStackItem, settingsAndAdvanceSettingsSelector, settingsElement, advanceSettingsElement, queryBuilerEle, $(".side-panel #queryBuilderSection"), compile, scope, vm, index, vm.dashboardAndReportComInformation.widgetList[index].widgetType, uiGridConstants, rootScope);
                //}
                if (typeof vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.ChildOfWidgetId == "undefined") {
                    vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.ChildOfWidgetId = "-1";
                }
                if (!vm.dashboardAndReportComInformation.widgetList[index].isDeleted && vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.ChildOfWidgetId == "-1") {
                    handleRenderWidget(element, addWidgetOfGridStackItem, settingsAndAdvanceSettingsSelector, settingsElement, advanceSettingsElement, queryBuilerEle, $(".side-panel #queryBuilderSection"), compile, scope, vm, index, vm.dashboardAndReportComInformation.widgetList[index].widgetType, uiGridConstants, rootScope, true);
                }

            }
            callback();
        };
        //#endregion

        //#region droppable  for default page
        var handleDroppablForDefaultPage = function (selector, ImageSelector, vm, widgetIndex, scope, compile) {
            $("#" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + " .widget .content-wrapper").droppable({
                accept: selector + ',' + " ." + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + " .content .add-db-columns .draggable_element " + "," + ImageSelector,
                classes: {
                    "ui-droppable-active": "ui-state-active",
                    "ui-droppable-hover": "ui-state-hover"
                },
                drop: function (event, ui) {
                    if ($(ui.draggable).parents().attr('data-type') == 'inputcontrol') {
                        handleDroppablInputControlForDefaultPage(event, ui, vm, widgetIndex, scope, compile);
                    }
                    else if ($(ui.draggable).parents().attr('data-type') == 'dbcolumn') {

                        handleDroppablDbColumnForDefaultPage(event, ui, vm, widgetIndex, scope, compile);
                    }
                    else if ($(ui.draggable).parents().attr('data-type') == 'imagecontrol') {
                        handleDroppablImageControlForDefaultPage(event, ui, vm, widgetIndex, scope, compile);

                    }


                }
            });
        };
        //#endregion

        //#region draggable db coulumns for default page
        var makeDragColumnsOfDefaultPage = function (selector, vm, widgetIndex, scope, compile) {

            $(selector).draggable({
                refreshPositions: true,
                helper: 'clone',
                revert: "invalid",
                cursor: "move",
                start: function (event, ui) {

                },
                drag: function (event, ui) {

                },
                stop: function (event, ui) {

                }


            });
            // handleDroppablDbColumnForDefaultPage(selector, vm, widgetIndex, scope, compile);
        };
        //#endregion

        //#region droppable db columns for default page
        var handleDroppablDbColumnForDefaultPage = function (event, ui, vm, widgetIndex, scope, compile) {

            //#region here is checking that content wrapper is empty or not if is not empty then should be empty otherwise already empty
            if ($(event.target).parents('.widget').find(".content-wrapper .center-icon").length > 0) {
                $(event.target).parents('.widget').find('.content-wrapper').empty();
            }
            //#endregion

            //#region get left and top
            var offset = $(event.target).offset();
            var relativeX = (event.pageX - offset.left);
            var relativeY = (event.pageY - offset.top);
            //#endregion

            //#region binding data in db column label
            let dbColumnSettingAndDataMemberProperty = {};
            dbColumnSettingAndDataMemberProperty.uniqueId = handleUniquekey(8);
            dbColumnSettingAndDataMemberProperty.left = relativeX;
            dbColumnSettingAndDataMemberProperty.top = relativeY;
            dbColumnSettingAndDataMemberProperty.text = ui.draggable.text();
            dbColumnSettingAndDataMemberProperty.columnName = ui.draggable.attr('data-member-name');
            dbColumnSettingAndDataMemberProperty.fontSize = 12;
            dbColumnSettingAndDataMemberProperty.fontColor = '#111';
            dbColumnSettingAndDataMemberProperty.fontFamily = 'arial';
            dbColumnSettingAndDataMemberProperty.fontBold = 'normal';
            dbColumnSettingAndDataMemberProperty.fontItalic = 'normal';
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration.push(dbColumnSettingAndDataMemberProperty);
            let getIndexOfCurrentDbColumnSettingDataMemberProperty = vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration.length - 1;

            //#endregion

            //#region binding db columns setting in right side  and redraggable 
            handleBindDbColumnsSetting(vm, widgetIndex, scope, compile, '.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + '.default_settings .db-label-settings .toggle', ui.draggable.text(), getIndexOfCurrentDbColumnSettingDataMemberProperty, $(event.target).parents('.widget'), true);

            //#endregion

        };
        //#endregion

        //#region redraggable columns on default page widget
        var handleRedraggableColumnsDefaultPage = function (dBColumnLabel, vm, widgetIndex, scope, compile, dbColumnIndex) {
            dBColumnLabel.draggable({
                refreshPositions: true,
                // revert: "invalid",
                containment: "#" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + " .widget .content-wrapper",
                cursor: "move",
                start: function (event, ui) {

                },
                drag: function (event, ui) {

                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[dbColumnIndex].left = ui.position.left;
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[dbColumnIndex].top = ui.position.top;
                },
                stop: function (event, ui) {

                }


            });
            scope.$apply();
        };

        //#endregion

        //#region bind database column setting
        var handleBindDbColumnsSetting = function (vm, widgetIndex, scope, compile, parentElement, ColumnAccTitle, dbSettingIndex, widgetSelector, renderStatus) {

            if (!vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.isSingleOrMultiRow) {
                hanldeBindingOnlySingleRowForDefaultPage(vm, widgetIndex, scope, compile, parentElement, ColumnAccTitle, dbSettingIndex, widgetSelector);
            }
            else {
                if (renderStatus)
                    handleBindingForMultipleRowWithTitle(vm, widgetIndex, scope, compile, parentElement, ColumnAccTitle, dbSettingIndex, widgetSelector);
            }

            //#region modal binding of db column
            var dbColumnSetting = $($.parseHTML(getWidgetSettings.find(x => x.type == "DBColumnLabel").settings));
            $(dbColumnSetting).find('h2:eq(0)').html(ColumnAccTitle);
            $(dbColumnSetting).find('input.dbcolumnfontsize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontSize');
            $(dbColumnSetting).find('input.dbcolumncolor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontColor');
            $(dbColumnSetting).find('input.dbcolumnbold').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontBold');
            $(dbColumnSetting).find('input.dbcolumnfontstyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontItalic');
            $(dbColumnSetting).find('.dbcolumnfontfamily').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontFamily');
            dbColumnSetting = compile(dbColumnSetting)(scope);
            $(parentElement).append(dbColumnSetting);
            //#endregion
        };
        //#endregion

        //#region draw data rows and columns form with default page

        var handleDrawDataRowsColumnsFormWithDefaultPage = function (vm, widgetIndex, scope, compile) {

            if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.isSingleOrMultiRow) {
                let totalNumberOfRows = vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.length;
                let totalNumberOfcolumns = vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.isColumnRange;
                let totalNumberOfRowsAcctoTotalColumns = totalNumberOfRows;
                let dbColumnsCount = vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration.length;
                let setRowInit = 0;
                if (totalNumberOfRows > 0 && totalNumberOfcolumns > 1) {
                    totalNumberOfRowsAcctoTotalColumns = parseInt(totalNumberOfRows / totalNumberOfcolumns);
                    let remainder = totalNumberOfRows % totalNumberOfcolumns;
                    remainder > 0 ? totalNumberOfRowsAcctoTotalColumns++ : totalNumberOfRowsAcctoTotalColumns;
                }
                var divR = `<div class=grid-area-dynamic-column>`;
                for (var r = 0; r < totalNumberOfRowsAcctoTotalColumns; r++) {
                    divR += `<div class=row>`;
                    for (var c = 0; c < totalNumberOfcolumns; c++) {
                        if (totalNumberOfRows > setRowInit) {
                            divR += `<div class="col-md-2 col-lg-2"><div class="status_box"><div class="status_box_left">`;
                            for (var innerC = 0; innerC < vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration.length; innerC++) {
                                if (innerC == 0) {
                                    divR += `<span class="badge color1 status_name" style=` + "font-size:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + innerC + '].fontSize' + "}}px" + ";" + "color:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + innerC + '].fontColor' + "}}" + ";" + "font-weight:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + innerC + '].fontBold' + "}}" + ";" + "font-family:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + innerC + '].fontFamily' + "}}" + ";" + "font-style:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + innerC + '].fontItalic' + "}}" + `>` + vm.dashboardAndReportComInformation.widgetList[widgetIndex].data[setRowInit][vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[innerC].columnName] + `</span>`;
                                }
                                else {
                                    divR += `<h3 class="mt-1 status_count" style=` + "font-size:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + innerC + '].fontSize' + "}}px" + ";" + "color:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + innerC + '].fontColor' + "}}" + ";" + "font-weight:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + innerC + '].fontBold' + "}}" + ";" + "font-family:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + innerC + '].fontFamily' + "}}" + ";" + "font-style:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + innerC + '].fontItalic' + "}}" + `>` + vm.dashboardAndReportComInformation.widgetList[widgetIndex].data[setRowInit][vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[innerC].columnName] + `</h3>`;
                                }

                            }
                            divR += `<p class="mb-0"><span class="text-danger"><i class="fas fa-bars"></i> </span> click to show details below</p>
                            </div>

                            <div class="status_box_right">
                                <span><i class="fa fa-envelope"></i></span>
                                <svg viewBox="0 0 230 147" style="opacity: 0.4;" xmlns="http://www.w3.org/2000/svg" xmlns: xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" width="230px" height="147px" viewBox="0 0 230 147" enable-background="new 0 0 230 147" xml: space="preserve">  <image width="230" height="147" x="0" y="0" xlink: href="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAOYAAACTCAMAAACtW5X7AAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAABg1BMVEXV5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v/V5v////+fuDLNAAAAf3RSTlMAV95SAs5JDdxKtAgs83EBb5sGsgcv60Ndqp3TFtFFdBfZOOgwhqCXI74Eaf21HAsPGa0lThJtA+b5mmgJd4snqMNLDjr1e/tEr2t+E5QYQH9czx5QIBCYKcDxP3hbJLoKZI51NyvaR2M7g7AFpem8EdXIFVH3gBodoypU5IwmRni7agAAAAFiS0dEgGW9nmgAAAAHdElNRQfjBAURCihM5BxJAAADp0lEQVR42uXdZ1fUQBgF4CtEZV10ELN2wQKWRQEFXNQVFOwFXRG7Iir2gmJv/HVCNR88R5Cde5N97y+4zwkhk5nJLJZNVHyqqoFguboFISuAleoOhNQAyKhLEBIxYeD2nGIGq9QtKExk1S04TNSuVvegMLFG3YPDdHXqIhQmsFbdhMOsX6euQmEiVFfhMJFbry5DYWKDugyH6Taq21CYwCZ1HQ4z3KzuQ2ECW9SFOMyt29SNKEwE6kYcJhrUlTjMihwO/YWJRnUpDhPbd6hrUZjYqa7FYSJXoy5GYSLcpW5GYaJJ3YzDRNis7kZhwu1Wl6Mwkd2jbkdhAnv3qftRmMi37Fc3ZDCRP6BuSGHCZSrhev6TGV3QClh4WAATaE39o2VBTLSpa3KYaE/5StICmcDBQ+qqFCbQkeKxwiKYQGdqZxUWxUTX4ZRuUlgcE4WW7iPqygQmXO6oujKDGV3QY+rOFCZQPK5uTWFGo4UedW8KM/qfm65b9H+ZQO8JdXcKE6g9qW5PYaLYl5ZF3yUxgf7mdMzOL5GJ4qme02oDgRlBz0ycVSsITATnzqsVDCaQv6BmUJhA20U1hMIELl1WUyhMYCDBs0VlZCK4ktjZonIygfBqQrcel5eJbOmaWsRgAoOJnBQrOxPBdbWJwozu0ORNovhgAkNJe+X2w8SNhM3Pe2Ki/6ZaRmECt26rbRQmCqU7ah2DiaBRraMwgbtJWVfyy0TbPTWQwoS7rxZSmCgk4rNt70zgwbAaSWHiof4DOwYT7fIP7ChMFEbETxYOE+6RCSaCxyaYQJPyBuUx3RMTTGD0qQkmcjaYyKieK1wmntlguucmmMALG0z30gQTKAl2ZAiYhVcmmHj9xgQTXfTFQQkTvW9NMFGywXR13M8lRUxk35lgYuy9CSZGbTCp5/UImeEHE0yM22Di4ycTTPfZBBNF1tytlkmbSxAzQdpqrGZ+6TbBRLUNZkg5e0mtBLpsMMGYAVMbo3wljIXUxql8s8EcssEM/B9AriZOp9X7xlu1cDqB9+00auFMvvtePVIDZ1Lv+zfb1MDZjNhgDniehFf7ZpP3fCCG2jeXHzaY436/b1Xz5pL3e16hmjefPq/vKWrdfAa9jhDUuvmEP00wC17nhNS6P+m0weywwfS6nqLGxTJsg+nziEK1LRafq2NqWyxjNphZj4f2qW2xhL9MMAOPR5mobbG4Rn+jd7UtHn+7T2vUtHgavL1yZtS0eH57+qOtqsYkdJEF61WY0YUAAAAldEVYdGRhdGU6Y3JlYXRlADIwMTktMDQtMDVUMTc6MTA6NDArMDM6MDDwLlQBAAAAJXRFWHRkYXRlOm1vZGlmeQAyMDE5LTA0LTA1VDE3OjEwOjQwKzAzOjAwgXPsvQAAABl0RVh0U29mdHdhcmUAQWRvYmUgSW1hZ2VSZWFkeXHJZTwAAAAASUVORK5CYII=" /></svg>
                           </div></div>
                        </div>`;
                        }
                        setRowInit++;


                    }
                    divR += `</div>`;
                }
                divR += `</div>`;

                $("#" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + ' .content-wrapper .grid-area-dynamic-column ,' + '#' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + ' .content-wrapper .db-col-label').remove();
                divR = compile($.parseHTML(divR))(scope);
                $("#" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + ' .content-wrapper').append(divR);

            }



        };

        //#endregion

        //#region binding only single row for default page

        var hanldeBindingOnlySingleRowForDefaultPage = function (vm, widgetIndex, scope, compile, parentElement, ColumnAccTitle, dbSettingIndex, widgetSelector) {
            //#region render html of db column
            var dBColumnLabel = $($.parseHTML(getWidgetSettings.find(x => x.type == "DBColumnLabel").code));
            dBColumnLabel.find(".db-label").text(vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[dbSettingIndex].text);
            dBColumnLabel.css("left", vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[dbSettingIndex].left + 'px');
            dBColumnLabel.css("top", vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[dbSettingIndex].top + 'px');
            dBColumnLabel.find(".db-label-val").html('{{ vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dataParams.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[dbSettingIndex].columnName + '}}');
            $(dBColumnLabel).find(".db-label-val").attr("style", "font-size:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontSize' + "}}px" + ";" + "color:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontColor' + "}}" + ";" + "font-weight:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontBold' + "}}" + ";" + "font-family:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontFamily' + "}}" + ";" + "font-style:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontItalic' + "}}");
            compile(widgetSelector.find('.content-wrapper').append(dBColumnLabel))(scope);
            handleRedraggableColumnsDefaultPage(dBColumnLabel, vm, widgetIndex, scope, compile, dbSettingIndex);
            //#endregion


        };

        //#endregion

        //#region binding for multiple row just disply title

        var handleBindingForMultipleRowWithTitle = function (vm, widgetIndex, scope, compile, parentElement, ColumnAccTitle, dbSettingIndex, widgetSelector) {
            //#region render html of db column
            var dBColumnLabel = $($.parseHTML(getWidgetSettings.find(x => x.type == "DBColumnLabel").code));
            dBColumnLabel.find(".db-label").text(vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[dbSettingIndex].text);
            dBColumnLabel.css("left", vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[dbSettingIndex].left + 'px');
            dBColumnLabel.css("top", vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[dbSettingIndex].top + 'px');
            dBColumnLabel.find(".db-label-val").html(vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[dbSettingIndex].text);
            $(dBColumnLabel).find(".db-label-val").attr("style", "font-size:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontSize' + "}}px" + ";" + "color:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontColor' + "}}" + ";" + "font-weight:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontBold' + "}}" + ";" + "font-family:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontFamily' + "}}" + ";" + "font-style:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration[' + dbSettingIndex + '].fontItalic' + "}}");
            compile(widgetSelector.find('.content-wrapper').append(dBColumnLabel))(scope);
            handleRedraggableColumnsDefaultPage(dBColumnLabel, vm, widgetIndex, scope, compile, dbSettingIndex);
            console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dbColumnsSettingsDataMemberPropertiesConfiguration);
            //#endregion
        };
        //#endregion

        //#region draggable input control and image control default page
        var makeDragInputControlOfDefaultPage = function (selector, ImageSelector, vm, widgetIndex, scope, compile) {

            $(selector).draggable({
                refreshPositions: true,
                helper: 'clone',
                revert: "invalid",
                cursor: "move",
                start: function (event, ui) {

                },
                drag: function (event, ui) {

                },
                stop: function (event, ui) {

                }


            });
            $(ImageSelector).draggable({
                refreshPositions: true,
                helper: 'clone',
                revert: "invalid",
                cursor: "move",
                start: function (event, ui) {

                },
                drag: function (event, ui) {

                },
                stop: function (event, ui) {

                }


            });
            handleDroppablForDefaultPage(selector, ImageSelector, vm, widgetIndex, scope, compile);
        };
        //#endregion

        //#region droppable input control for default page
        var handleDroppablInputControlForDefaultPage = function (event, ui, vm, widgetIndex, scope, compile) {

            //#region here is checking that content wrapper is empty or not if is not empty then should be empty otherwise already empty
            if ($(event.target).parents('.widget').find(".content-wrapper .center-icon").length > 0) {
                $(event.target).parents('.widget').find('.content-wrapper').empty();
            }
            //#endregion

            //#region get left and top
            var offset = $(event.target).offset();
            var relativeX = (event.pageX - offset.left);
            var relativeY = (event.pageY - offset.top);
            //#endregion 

            //#region intialization of the data in custom input control 
            let customInputControlProperty = {};
            customInputControlProperty.uniqueId = handleUniquekey(8);
            customInputControlProperty.left = relativeX;
            customInputControlProperty.top = relativeY;
            customInputControlProperty.text = "enter title";
            customInputControlProperty.fontSize = 12;
            customInputControlProperty.fontColor = '#111';
            customInputControlProperty.fontFamily = 'arial';
            customInputControlProperty.fontBold = 'normal';
            customInputControlProperty.fontItalic = 'normal';
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customInputControlPropertiesConfiguration.push(customInputControlProperty);
            let getIndexOfCurrentInputControlProperty = vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customInputControlPropertiesConfiguration.length - 1;

            //#endregion

            //#region binding db columns setting in right side  and redraggable 
            handleBindInputControlInAdvanceSetting(vm, widgetIndex, scope, compile, getIndexOfCurrentInputControlProperty, $(event.target).parents('.widget'));

            //#endregion

        };
        //#endregion

        //#region redraggable input control on default page widget
        var handleRedraggableInputControlDefaultPage = function (dBColumnLabel, vm, widgetIndex, scope, compile, dbColumnIndex) {
            dBColumnLabel.draggable({
                refreshPositions: true,
                // revert: "invalid",
                containment: "#" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + " .widget .content-wrapper",
                cursor: "move",
                start: function (event, ui) {

                },
                drag: function (event, ui) {

                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customInputControlPropertiesConfiguration[dbColumnIndex].left = ui.position.left;
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customInputControlPropertiesConfiguration[dbColumnIndex].top = ui.position.top;
                },
                stop: function (event, ui) {

                }


            });
            scope.$apply();
        };

        //#endregion

        //#region binding and redraggable  custom input controll in advance setting
        var handleBindInputControlInAdvanceSetting = function (vm, widgetIndex, scope, compile, dbSettingIndex, widgetSelector) {

            //#region render html of db column
            let customInputControl = $($.parseHTML(getWidgetSettings.find(x => x.type == "DBColumnLabel").inputControl));
            customInputControl.find(".customInputControlOfPage").attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].text');
            customInputControl.find(".custominputSpanOfPage").attr('ng-bind', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].text');
            customInputControl.css("left", vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customInputControlPropertiesConfiguration[dbSettingIndex].left + 'px');
            customInputControl.css("top", vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customInputControlPropertiesConfiguration[dbSettingIndex].top + 'px');

            customInputControl.find(".custominputSpanOfPage").attr("style", "font-size:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontSize' + "}}px" + ";" + "color:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontColor' + "}}" + ";" + "font-weight:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontBold' + "}}" + ";" + "font-family:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontFamily' + "}}" + ";" + "font-style:{{" + 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontItalic' + "}}");
            compile(widgetSelector.find('.content-wrapper').append(customInputControl))(scope);
            handleRedraggableInputControlDefaultPage(customInputControl, vm, widgetIndex, scope, compile, dbSettingIndex);
            //#endregion

            //#region modal binding of db column
            var dbColumnSetting = $($.parseHTML(getWidgetSettings.find(x => x.type == "DBColumnLabel").settings));
            $(dbColumnSetting).find('h2:eq(0)').text('input control');
            $(dbColumnSetting).find('input.dbcolumnfontsize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontSize');
            $(dbColumnSetting).find('input.dbcolumncolor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontColor');
            $(dbColumnSetting).find('input.dbcolumnbold').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontBold');
            $(dbColumnSetting).find('input.dbcolumnfontstyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontItalic');
            $(dbColumnSetting).find('.dbcolumnfontfamily').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontFamily');
            dbColumnSetting = compile(dbColumnSetting)(scope);
            $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + '.default_advance_settings .db-label-settings .toggle').append(dbColumnSetting);
            //#endregion
        };
        //#endregion

        //#region droppable image control for default page
        var handleDroppablImageControlForDefaultPage = function (event, ui, vm, widgetIndex, scope, compile) {

            //#region here is checking that content wrapper is empty or not if is not empty then should be empty otherwise already empty
            if ($(event.target).parents('.widget').find(".content-wrapper .center-icon").length > 0) {
                $(event.target).parents('.widget').find('.content-wrapper').empty();
            }
            //#endregion

            //#region get left and top
            var offset = $(event.target).offset();
            var relativeX = (event.pageX - offset.left);
            var relativeY = (event.pageY - offset.top);
            //#endregion 

            //#region intialization of the data in custom image control 
            let customImageControlProperty = {};
            customImageControlProperty.uniqueId = handleUniquekey(8);
            customImageControlProperty.left = relativeX;
            customImageControlProperty.top = relativeY;
            customImageControlProperty.height = 100;
            customImageControlProperty.width = 100;
            customImageControlProperty.imageUrl = "/images/logo.png";
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration.push(customImageControlProperty);
            let getIndexOfCurrentImageControlProperty = vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration.length - 1;

            //#endregion

            //#region binding db columns setting in right side  and redraggable 
            handleBindImageControlInAdvanceSetting(vm, widgetIndex, scope, compile, getIndexOfCurrentImageControlProperty, $(event.target).parents('.widget'));

            //#endregion

        };
        //#endregion

        //#region redraggable Image control on default page widget
        var handleRedraggableImageControlDefaultPage = function (customImageControl, vm, widgetIndex, scope, compile, dbColumnIndex) {
            customImageControl.draggable({
                refreshPositions: true,
                // revert: "invalid",
                containment: "#" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + " .widget .content-wrapper",
                cursor: "move",
                start: function (event, ui) {

                },
                drag: function (event, ui) {

                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration[dbColumnIndex].left = ui.position.left;
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration[dbColumnIndex].top = ui.position.top;
                },
                stop: function (event, ui) {

                }


            });
            scope.$apply();
        };

        //#endregion

        //#region binding and redraggable  custom image controll in advance setting
        var handleBindImageControlInAdvanceSetting = function (vm, widgetIndex, scope, compile, dbSettingIndex, widgetSelector) {

            //#region render html of db column
            let customImageControl = $($.parseHTML(getWidgetSettings.find(x => x.type == "DBColumnLabel").imageControl));
            customImageControl.css("left", vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration[dbSettingIndex].left + 'px');
            customImageControl.css("top", vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration[dbSettingIndex].top + 'px');
            customImageControl.css("height", vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration[dbSettingIndex].height + 'px');
            customImageControl.css("width", vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration[dbSettingIndex].width + 'px');
            customImageControl.find('.img-drag-drop').attr("ng-src", '{{vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customImageControlPropertiesConfiguration[' + dbSettingIndex + '].imageUrl }}');
            compile(widgetSelector.find('.content-wrapper').append(customImageControl))(scope);
            handleRedraggableImageControlDefaultPage(customImageControl, vm, widgetIndex, scope, compile, dbSettingIndex);
            handleResizeImageControlDefaultPage(customImageControl, vm, widgetIndex, scope, compile, dbSettingIndex);

            //#endregion

            ////#region modal binding of db column
            //var dbColumnSetting = $($.parseHTML(getWidgetSettings.find(x => x.type == "DBColumnLabel").settings));
            //$(dbColumnSetting).find('h2:eq(0)').text('input control');
            //$(dbColumnSetting).find('input.dbcolumnfontsize').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontSize');
            //$(dbColumnSetting).find('input.dbcolumncolor').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontColor');
            //$(dbColumnSetting).find('input.dbcolumnbold').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontBold');
            //$(dbColumnSetting).find('input.dbcolumnfontstyle').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontItalic');
            //$(dbColumnSetting).find('.dbcolumnfontfamily').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].defaultPageConfiguration.customInputControlPropertiesConfiguration[' + dbSettingIndex + '].fontFamily');
            //dbColumnSetting = compile(dbColumnSetting)(scope);
            //$('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + '.default_advance_settings .db-label-settings .toggle').append(dbColumnSetting);
            ////#endregion
        };
        //#endregion

        //#region resize Image control on default page widget
        var handleResizeImageControlDefaultPage = function (customImageControl, vm, widgetIndex, scope, compile, dbColumnIndex) {
            customImageControl.resizable({

                containment: "#" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + " .widget .content-wrapper",
                //  helper: "ui-state-hover",
                maxHeight: 400,
                maxWidth: 400,
                minHeight: 20,
                minWidth: 20,
                animate: false,
                start: function (event, ui) {

                },
                drag: function (event, ui) {

                },
                stop: function (event, ui) {

                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration[dbColumnIndex].height = ui.size.height;
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration[dbColumnIndex].width = ui.size.width;
                }


            });
            scope.$apply();
        };

        //#endregion

        //#region reloading of murri select columns
        var handleReloadingOfMurriSelectedColumns = function (vm, WigetIndex, scope, compile) {
            let getAllHtml = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsHtml;
            if (getAllHtml.length > 0) {
                for (var x = 0; x < getAllHtml.length; x++) {
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].murriInitializationOfSelectedColumn.add($.parseHTML(getAllHtml[x]));
                }


            }
            $timeout(function () { compile($('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.selected-rows'))(scope); }, 100);

        };
        //#endregion

        //#region check Is Dark or not
        var isDark = function (color) {
            var match = /rgb\((\d+).*?(\d+).*?(\d+)\)/.exec(color);
            return (match[1] & 255)
                + (match[2] & 255)
                + (match[3] & 255)
                < 4 * 256 / 2;
        };
        //#endregion

        //#region hex to RGB
        var hexToRgb = function (hex) {
            var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
            return result ? {
                r: parseInt(result[1], 16),
                g: parseInt(result[2], 16),
                b: parseInt(result[3], 16)
            } : null;
        };
        //#endregion

        //#region InitialLization of the maps
        var initMapDynamic = function (compile, scope, vm, WigetIndex) {
            $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.idsInfo.DynamicItemID).empty();
            $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.idsInfo.DynamicItemID).css('height', '450px');
            mapList[WigetIndex] = new google.maps.Map(document.getElementById(vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.idsInfo.DynamicItemID), {
                center: { lat: 0.000000, lng: 0.000000 },
                zoom: 2,
                minZoom: 2,
                //mapTypeControl: false
            });
            var bounds = new google.maps.LatLngBounds();
            if ("OVER_QUERY_LIMIT" == google.maps.GeocoderStatus.OVER_QUERY_LIMIT) {
                //toastrs.error("Google Map Over Query Limit.");
            }
            var infowindow = new google.maps.InfoWindow();
            var mapdata = vm.dashboardAndReportComInformation.widgetList[WigetIndex].data;
            var GMapColorListTemp = [];
            if (mapdata.length > 0) {
                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype == 'Market') {
                    // code start
                    var selectedColumn = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype;
                    var sourceColumn = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sourceForMapPinSetting;
                    if (sourceColumn != undefined) {
                        selectedColumn = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sourceForMapPinSetting;
                    }
                    vm.dashboardAndReportComInformation.currentSelectedColumn = selectedColumn;
                    if (selectedColumn != 'Market' && selectedColumn != "") {
                        var PinColor = '';
                        var getData = mapdata;
                        for (var i = 0; i < getData.length; i++) {
                            //var IsPinColorExist = false;
                            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData != undefined) {
                                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[selectedColumn] != undefined) {
                                    var GMapObj = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[selectedColumn].find(x => x.Name == getData[i][selectedColumn]);
                                    if (GMapObj != undefined) {
                                        if (GMapObj.PinColor == undefined) {
                                            PinColor = getRandomColor();
                                        } else {
                                            PinColor = GMapObj.PinColor;
                                            //IsPinColorExist = true;
                                        }
                                    } else {
                                        PinColor = getRandomColor();
                                    }
                                } else {
                                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[selectedColumn] = [];
                                    PinColor = getRandomColor();
                                }
                            } else {
                                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData = {};
                                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[selectedColumn] = [];
                                PinColor = getRandomColor();
                            }
                            //if (IsPinColorExist == false) {
                            //    vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[selectedColumn].push({ "Name": getData[i][selectedColumn], "PinColor": PinColor });
                            //}
                            if (GMapColorListTemp.find(x => x.Name == getData[i][selectedColumn]) == undefined) {
                                GMapColorListTemp.push({ "Name": getData[i][selectedColumn], "PinColor": PinColor });
                            }
                        }
                        vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[selectedColumn] = GMapColorListTemp;
                    }
                    // end code 
                    for (var i = 0; i < mapdata.length; i++) {
                        // code start
                        var PinColor = '';
                        if (selectedColumn != 'Market' && selectedColumn != "") {
                            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData != undefined) {
                                var GMapObj = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[selectedColumn].find(x => x.Name == mapdata[i][selectedColumn]);
                                if (GMapObj != undefined) {
                                    PinColor = GMapObj.PinColor;
                                }
                            } else {
                                PinColor = getRandomColor();
                            }

                        } else {
                            PinColor = getRandomColor();
                        }
                        var pinIcon = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|" + PinColor.substr(1);
                        // end code 
                        var marker = new google.maps.Marker({
                            map: mapList[WigetIndex],
                            position: new google.maps.LatLng(parseFloat(mapdata[i].Latitude), parseFloat(mapdata[i].Longitude)),
                            icon: pinIcon
                        });
                        //bounds.extend(marker.getPosition());
                        var markercontent = "";
                        var columnsIn = mapdata[i];

                        for (var key in columnsIn) {
                            markercontent += "<b style='font-weight: bold;'>" + key + ": </b> " + columnsIn[key] + "<br />";
                        }
                        bounds.extend(new google.maps.LatLng(parseFloat(mapdata[i].Latitude), parseFloat(mapdata[i].Longitude)));
                        google.maps.event.addListener(marker, 'click', (function (marker, markercontent) {
                            return function () {
                                infowindow.setContent(markercontent);
                                infowindow.open(mapList, marker);
                            };
                        })(marker, markercontent));
                    }
                    // code end
                }
                else {
                    // code start
                    vm.dashboardAndReportComInformation.currentSelectedColumn = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype;
                    // code end
                    for (let i = 0; i < mapdata.length; i++) {
                        //var IsPinColorExist = false;
                        // code start
                        let PinColor = '';
                        var MapLevelType = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype;
                        if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData != undefined) {
                            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[MapLevelType] != undefined) {
                                let GMapObj = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[MapLevelType].find(x => x.Name == mapdata[i].Name);
                                if (GMapObj != undefined) {
                                    if (GMapObj.PinColor == undefined) {
                                        PinColor = getRandomColor();
                                    } else {
                                        PinColor = GMapObj.PinColor;
                                        //IsPinColorExist = true;
                                    }
                                } else {
                                    PinColor = getRandomColor();
                                }
                            } else {
                                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[MapLevelType] = [];
                                PinColor = getRandomColor();
                            }
                        } else {
                            vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData = {};
                            vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[MapLevelType] = [];
                            PinColor = getRandomColor();
                        }
                        let pinIcon = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|" + PinColor.substr(1);
                        // code end
                        let marker = new google.maps.Marker({
                            map: mapList[WigetIndex],
                            position: new google.maps.LatLng(parseFloat(mapdata[i].Latitude), parseFloat(mapdata[i].Longitude)),
                            icon: pinIcon
                        });
                        // code start
                        // if (IsPinColorExist == false) {
                        // vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[MapLevelType].push({ "Name": mapdata[i].Name, "PinColor": PinColor });
                        //  }
                        if (GMapColorListTemp.find(x => x.Name == mapdata[i].Name) == undefined) {
                            GMapColorListTemp.push({ "Name": mapdata[i].Name, "PinColor": PinColor });
                        }

                        // code end
                        let markercontent = "<b style='font-weight: bold;'>Total Number of Records:</b> " + mapdata[i].TotalRecords + "<br />" + "<b style='font-weight: bold;'>" + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype + ":</b> " + mapdata[i].RecordValue;
                        bounds.extend(new google.maps.LatLng(parseFloat(mapdata[i].Latitude), parseFloat(mapdata[i].Longitude)));

                        google.maps.event.addListener(marker, 'click', (function (marker, markercontent) {
                            return function () {
                                infowindow.setContent(markercontent);
                                infowindow.open(mapList, marker);
                            };
                        })(marker, markercontent));
                    }
                }
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.GMapData[MapLevelType] = GMapColorListTemp;
                mapList[WigetIndex].fitBounds(bounds);
            }


        };
        //#endregion

        //#region map configuration
        var handleMaps = function (compile, scope, vm, WigetIndex, toasterStatus) {
            let GetQuery = '';
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype == 'Country') {
                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere == '()') {
                    GetQuery = 'select TotalRecords, RecordValue, MapLatitude as Latitude, MapLongitude as Longitude, Country as Name,  Country as Country from(Select (COUNT(*)) as TotalRecords, Country As RecordValue, Country from ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + ' Group by Country) b inner join AD_Definations on b.Country = AD_Definations.DefinationName where AD_Definations.DefinationTypeId = 3';
                }
                else {
                    GetQuery = 'select TotalRecords, RecordValue, MapLatitude as Latitude, MapLongitude as Longitude, Country as Name,Country as Country from(Select (COUNT(*)) as TotalRecords, Country As RecordValue, Country from ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + ' Where ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + ' Group by Country) b inner join AD_Definations on b.Country = AD_Definations.DefinationName where AD_Definations.DefinationTypeId = 3';
                }
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.MarketLevel = false;
                LoadFinalDataForMap(compile, scope, vm, WigetIndex, GetQuery, toasterStatus);
            }
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype == 'State') {
                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere == '()') {
                    GetQuery = "select TotalRecords, RecordValue, MapLatitude as Latitude, MapLongitude as Longitude, [State] as Name, [State] as [State] from (Select (COUNT(*)) as TotalRecords, ([State] + ', ' + Country) As RecordValue, [State] from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Group by [State], Country) b inner join AD_Definations on b.[State] = AD_Definations.DefinationName where AD_Definations.DefinationTypeId = 4";
                }
                else {
                    GetQuery = "select TotalRecords, RecordValue, MapLatitude as Latitude, MapLongitude as Longitude, [State] as Name, [State] as [State] from (Select (COUNT(*)) as TotalRecords, ([State] + ', ' + Country) As RecordValue, [State] from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Where " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + " Group by [State], Country) b inner join AD_Definations on b.[State] = AD_Definations.DefinationName where AD_Definations.DefinationTypeId = 4";
                }
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.MarketLevel = false;
                LoadFinalDataForMap(compile, scope, vm, WigetIndex, GetQuery, toasterStatus);
            }
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype == 'Region') {
                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere == '()') {
                    GetQuery = "select TotalRecords, RecordValue, MapLatitude as Latitude, MapLongitude as Longitude, Region as Name, Region as Region from ( Select (COUNT(*)) as TotalRecords, (Region + ', ' + [State] + ', ' + Country) As RecordValue, RegionId, Region from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Group by RegionId, Region, [State], Country) b Left outer join AD_Definations on b.RegionId = AD_Definations.DefinationId";
                }
                else {
                    GetQuery = "select TotalRecords, RecordValue, MapLatitude as Latitude, MapLongitude as Longitude, Region as Name, Region as Region from ( Select (COUNT(*)) as TotalRecords, (Region + ', ' + [State] + ', ' + Country) As RecordValue, RegionId, Region from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Where " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + " Group by RegionId, Region, [State], Country) b Left outer join AD_Definations on b.RegionId = AD_Definations.DefinationId";
                }
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.MarketLevel = false;
                LoadFinalDataForMap(compile, scope, vm, WigetIndex, GetQuery, toasterStatus);
            }
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype == 'Market') {
                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere == '()') {
                    GetQuery = "Select (COUNT(*)) as TotalMarketRecords, Market As RecordValue, Country, Market As Name, Market As Market, (select count(*) from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + ") as TotalRecords from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " GROUP by Market, RegionId, State, Country";
                }
                else {
                    GetQuery = "Select (COUNT(*)) as TotalMarketRecords, Market As RecordValue, Country, Market As Name, Market As Market, (select count(*) from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Where " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + ") as TotalRecords from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Where " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + " GROUP by Market, RegionId, State, Country";
                }


                handleGetDataFromDbForMap(WigetIndex, GetQuery, vm, toasterStatus).done(function (result) {
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].IsAllCountriesOptionSelected = true;
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].MapFilterCitiesList = [];
                    //   vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.selectedMarketsList = [];
                    if (result.status) {
                        if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].data[0].TotalRecords < 10) {
                            GetQuery = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sqlQueryBuild;
                            vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.MarketLevel = false;
                            LoadFinalDataForMap(compile, scope, vm, WigetIndex, GetQuery, toasterStatus);
                        }
                        else {
                            // vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.Country = '0';
                            vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.MarketLevel = true;
                            $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').html('');
                            $('#chkmarketfilters').html('');
                            var marketfilterdata = vm.dashboardAndReportComInformation.widgetList[WigetIndex].data;
                            var ddmarketfiltercountries = $();

                            $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').append('<option value="0">Please Select</option>');
                            $.each(marketfilterdata, function (i, item) {

                                var $option = $('<option value=' + item.Country + '>' + item.Country + '</option>');
                                //Add it to the bucket only if it's not already added!
                                if (!ddmarketfiltercountries.filter(":contains(" + item.Country + ")").length) {
                                    ddmarketfiltercountries = ddmarketfiltercountries.add($option);
                                }

                                vm.dashboardAndReportComInformation.widgetList[WigetIndex].MapFilterCitiesList.push({ Value: item.RecordValue, NoOfRecords: item.TotalMarketRecords, Country: item.Country });
                                //$('#chkmarketfilters').append('<input type="checkbox" ng-click="vm.CountMarketChkbox(' + WigetIndex + ')" class ="selectRow marketfilterchkbox" data-recordnumber="' + item.TotalMarketRecords + '" value="' + item.RecordValue + '"> ' + item.RecordValue + ' <small class="marketfilterchkboxsmall">(' + item.TotalMarketRecords + ')<small>' + '<br> ');
                            });


                            compile($('#chkmarketfilters'))(scope);
                            $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').append(ddmarketfiltercountries);



                            $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].widgetConfiguration.Country');
                            $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').attr('ng-change', 'vm.changemarketfiltercountry(' + WigetIndex + ')');
                            compile($('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries'))(scope);
                            $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').val(vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.Country);
                            $('#btnsavemarketfiltermodal').unbind('click');
                            $('#btncancelmarketfiltermodal').unbind('click');
                            compile($('#btnsavemarketfiltermodal').attr('ng-click', 'vm.Savemarketfilterchkbox(' + WigetIndex + ')'))(scope);
                            compile($('#btncancelmarketfiltermodal').attr('ng-click', 'vm.CancelMarketFilterModal(' + WigetIndex + ')'))(scope);
                            //  LoadFinalDataForMap(compile, scope, vm, WigetIndex, GetQuery, toasterStatus);
                            vm.changemarketfiltercountry(WigetIndex);
                            vm.Savemarketfilterchkbox(WigetIndex, null);
                            //$('#marketfiltermodal').modal('show');
                            //console.log(vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.Country);
                        }

                    }
                });
            }
        };

        //#endregion

        //#region get data of the map from database base on query with final call
        var LoadFinalDataForMap = function (compile, scope, vm, WigetIndex, GetQuery, toasterStatus) {
            if (GetQuery.length > 0) {

                $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.overlay').attr('style', 'display:block;');
                //console.log(GetQuery);
                handleGetDataFromDbForMap(WigetIndex, GetQuery, vm, toasterStatus).done(function (result) {
                    if (result.status) {
                        handleGetDataToGevenValueForMap(vm, WigetIndex, scope, compile);
                        initMapDynamic(compile, scope, vm, WigetIndex);

                    }


                });
            }
            else {
                //  $grid._resizeHandler();
            }

        };
        //#endregion

        //#region page configuration
        var hanldeDefaultPage = function (compile, scope, vm, WigetIndex, toasterStatus) {
            let GetQuery = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sqlQueryBuild;
            if (GetQuery.length > 0) {
                //console.log(GetQuery);
                $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.overlay').attr('style', 'display:block;');
                handleGetDataFromDbForDefaultPage(WigetIndex, GetQuery, vm, toasterStatus).done(function (result) {
                    handleAssignValueOfDefaultPage(vm, WigetIndex, vm.dashboardAndReportComInformation.widgetList[WigetIndex].data[0]);
                    scope.$apply();


                });
            }
            else {
                // $grid._resizeHandler();
            }

        };

        //#endregion

        //#region uiGrid Settings and initialization
        var handleUiGridSettingsAndInitialization = function (compile, scope, vm, uiGridConstants, WigetIndex) {
            let countForUiGrid = 0;
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationStatus) {
                let uiGrid = $.parseHTML(getWidgetSettings.find(x => x.type == vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType).uiGrid);
                $(uiGrid).attr('ui-grid', 'vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].uiGridConfiguration.uiGridInitailization');
                $(uiGrid).attr('ng-class', '[vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].widgetConfiguration.classInfo.border' + ',' + 'vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].widgetConfiguration.classInfo.simpleStyleClass]');
                $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.searchdiv').attr('style', 'display:block');
                //please change this binding from here to initial creating widget when have time
                compile($('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.txtsearch1').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].SearchText'))(scope);
                compile($('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.txtsearch1').attr('ng-keyup', 'vm.SearchUiGrid(vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '])'))(scope);
                $(uiGrid).css('height', vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.styleInfo.widgetHeight - 30);
                uiGrid = compile(uiGrid)(scope);
                $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.content-wrapper').empty();
                $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.content-wrapper').html(uiGrid);
                delete vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.uiGridInitailization;
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.uiGridInitailization = {
                    enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                    enableVerticalScrollbar: uiGridConstants.scrollbars.NEVER,
                    paginationPageSizes: [5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100],
                    paginationPageSize: vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.paginationSize,
                    useExternalPagination: true,
                    useExternalSorting: true,
                    appScopeProvider: vm,
                    enableSorting: true,
                    rowHeight: 30,
                    showColumnFooter: false,
                    enableGridMenu: true,
                    totalItems: vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.totalItemsOfUiGrid,
                    //rowTemplate:
                    //    '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                    columnDefs: [],
                    onRegisterApi: function (gridApi) {

                        vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.gridApi = gridApi;
                        vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.gridApi.core.on.sortChanged(scope, function (grid, sortColumns) {

                            if (!sortColumns.length || !sortColumns[0].field) {
                                requestParams.sorting = null;
                            } else {
                                requestParams.sorting = '[' + sortColumns[0].field + '] ' + sortColumns[0].sort.direction;
                                vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.orderBy = " ORDER BY " + requestParams.sorting;
                            }
                            //console.log(vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.orderBy);
                            handleQueryChangePageOrOrderBy(compile, scope, vm, uiGridConstants, WigetIndex, vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.paginationSize, vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.pageNumber);
                        });
                        vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.gridApi.pagination.on.paginationChanged(scope, function (pageNumber, pageSize) {
                            handleQueryChangePageOrOrderBy(compile, scope, vm, uiGridConstants, WigetIndex, pageSize, pageNumber);

                        });
                        vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.gridApi.grid.registerRowsProcessor(function (renderableRows) {
                            // console.log(vm.dashboardAndReportComInformation.widgetList[WigetIndex].SearchText);

                            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].SearchText != '') {
                                var value = vm.dashboardAndReportComInformation.widgetList[WigetIndex].SearchText ? vm.dashboardAndReportComInformation.widgetList[WigetIndex].SearchText.toLowerCase() : "";
                                var matcher = new RegExp(value);
                                renderableRows.forEach(function (row) {
                                    var match = false;

                                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.uiGridInitailization.columnDefs.forEach(function (field) {


                                        var val = row.entity[field.name] ? row.entity[field.name] : "";


                                        if (String(val.toString().toLowerCase()).match(matcher)) {
                                            match = true;
                                        }
                                    });
                                    if (!match) {
                                        row.visible = false;
                                    }
                                });
                            }

                            return renderableRows;
                        }, 200);
                        //#region thead and trow settings
                        vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.gridApi.core.on.rowsRendered(scope, function () {

                            //   grid.grid.rows[0].grid.columns[0].cellClass = "dark";
                            if (countForUiGrid == 0) {
                                let getDarkForTable = "";
                                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == 'widgetOfTable') {
                                    if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.classInfo.simpleStyleClass[0] == '#') {
                                        let getRGBForTable = hexToRgb(vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.classInfo.simpleStyleClass);
                                        getDarkForTable = isDark("rgb(" + getRGBForTable.r + "," + getRGBForTable.g + "," + getRGBForTable.b + ")") ? '#fff' : '#4e4e4e';
                                    }
                                    else {
                                        //
                                    }

                                }


                                compile($('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find(".ui-grid-header-viewport").attr('style', 'color:' + getDarkForTable + ';' + 'background:{{' + 'vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].widgetConfiguration.classInfo.simpleStyleClass' + '}}' + ';' + 'font-size:{{' + 'vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].widgetConfiguration.styleInfo.theadFontSize' + '}}px'))(scope);

                                compile($('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find(".ui-grid-canvas").attr('style', 'font-size:{{' + 'vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].widgetConfiguration.styleInfo.trowFontSize' + '}}px'))(scope);
                                countForUiGrid = 1;
                            }

                        });
                        //#endregion
                    },

                    data: vm.dashboardAndReportComInformation.widgetList[WigetIndex].data
                };


            }
            else {
                //sss
            }

           // console.log(vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.uiGridInitailization);

            // $grid._resizeHandler();
        };
        //#endregion

        //#region handle query when change page or order by on ui grid
        var handleQueryChangePageOrOrderBy = function (compile, scope, vm, uiGridConstants, WigetIndex, pageSize, pageNumber) {

            vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.paginationSize = pageSize;
            vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.pageNumber = pageNumber;
            let makeQuery = handleMakeQueryAfterRenderGrid(vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sqlQueryBuild, pageSize, pageNumber, vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.orderBy);
            handleGetDataFromDbForTable(WigetIndex, makeQuery, vm, false).done(function (response) {

                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].data.length > 0) {
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.uiGridInitailization.data = vm.dashboardAndReportComInformation.widgetList[WigetIndex].data;
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.gridApi.core.refresh();
                }





            });
        };
        //#endregion

        //#region Initialize generic Table render in Grid
        var handleGenericTableInitialization = function (compile, scope, vm, WigetIndex) {

            //#region get Dark for Table
            let getDarkForTable = "";
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == 'widgetOfTable') {
                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.classInfo.simpleStyleClass[0] == '#') {
                    let getRGBForTable = hexToRgb(vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.classInfo.simpleStyleClass);
                    getDarkForTable = isDark("rgb(" + getRGBForTable.r + "," + getRGBForTable.g + "," + getRGBForTable.b + ")") ? '#fff' : '#4e4e4e';
                }
                else {
                }

            }

            //#endregion

            $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.content-wrapper').empty();
            let genericTable = $.parseHTML(getWidgetSettings.find(x => x.type == vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType).tableStandardFormat);
            $(genericTable).attr('ng-class', '[vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].widgetConfiguration.classInfo.border' + ',' + 'vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].widgetConfiguration.classInfo.simpleStyleClass]');
            $(genericTable).find("thead").attr('style', 'color:' + getDarkForTable + ';' + 'background:{{' + 'vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].widgetConfiguration.classInfo.simpleStyleClass' + '}}' + ';' + 'font-size:{{' + 'vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].widgetConfiguration.styleInfo.theadFontSize' + '}}px');
            $(genericTable).find("tbody tr td:first").attr('style', 'font-size:{{' + 'vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].widgetConfiguration.styleInfo.trowFontSize' + '}}px');
            $(genericTable).find('thead tr th:first').attr('ng-repeat', '(header, value) in vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].data[0]');
            $(genericTable).find('tbody tr:first').attr('ng-repeat', 'row in vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].data');
            genericTable = compile(genericTable)(scope);
            $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.content-wrapper').html(genericTable);
            //$grid._resizeHandler();

        };
        //#endregion

        //#region initialization of the canvas chart
        var handleChartInitializationOfCanvas = function (compile, scope, vm, widgetIndex) {
            let chartDiv = $.parseHTML(getWidgetSettings.find(x => x.type == vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetType).chartDiv);
            $(chartDiv).attr('id', vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartID);
            chartDiv = compile(chartDiv)(scope);
            $('#' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.content-wrapper').empty();
            $('#' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.content-wrapper').html(chartDiv);
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization = new CanvasJS.Chart(vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartID,
                {
                    responsive: true,
                    height: Number(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.styleInfo.widgetHeight) - 30,
                    zoomEnabled: true,
                    //#region axisX
                    axisX: {
                        title: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.title,
                        titleFontSize: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.titleFontSize,
                        titleFontColor: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.titleFontColor,
                        titleFontWeight: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.titleFontWeight,
                        titleFontStyle: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.titleFontStyle,
                        labelFontColor: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.labelFontColor,
                        labelFontSize: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.labelFontSize,
                        labelFontWeight: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.labelFontWeight,
                        labelFontStyle: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.labelFontStyle,
                        prefix: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.prefix,
                        suffix: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.suffix,


                    },
                    //#endregion

                    //#region axisY
                    axisY: {
                        title: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.title,
                        titleFontSize: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.titleFontSize,
                        titleFontColor: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.titleFontColor,
                        titleFontWeight: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.titleFontWeight,
                        titleFontStyle: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.titleFontStyle,
                        labelFontColor: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.labelFontColor,
                        labelFontSize: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.labelFontSize,
                        labelFontWeight: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.labelFontWeight,
                        labelFontStyle: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.labelFontStyle,
                        prefix: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.prefix,
                        suffix: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.suffix,
                    },
                    //#endregion

                    //#region title
                    title: {
                        text: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartTitleSettings.chartTitleName,
                        fontSize: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartTitleSettings.fontSize,
                        fontColor: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartTitleSettings.fontColor,
                        fontFamily: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartTitleSettings.fontFamily,
                        fontWeight: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartTitleSettings.fontWeight,
                        fontStyle: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartTitleSettings.fontStyle,
                    },
                    //#endregion

                    //#region legend
                    legend: {
                        fontSize: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.fontSize,
                        fontColor: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.fontColor,
                        fontFamily: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.fontFamily,
                        fontWeight: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.fontWeight,
                        fontStyle: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.fontStyle,
                        verticalAlign: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.verticalAlign,
                        horizontalAlign: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.horizontalAlign,
                    },
                    //#endregion

                    //#region subtitles
                    subtitles: [
                        {
                            text: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartSubTitleSettings.chartTitleName,
                            fontSize: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartSubTitleSettings.fontSize,
                            fontColor: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartSubTitleSettings.fontColor,
                            fontFamily: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartSubTitleSettings.fontFamily,
                            fontWeight: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartSubTitleSettings.fontWeight,
                            fontStyle: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartSubTitleSettings.fontStyle,
                        }],
                    //#endregion

                    //#region data
                    data: vm.dashboardAndReportComInformation.widgetList[widgetIndex].data

                    //#endregion
                });

            // handleChartRenderOfCanvas(compile, scope, vm, widgetIndex);
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.render();

            // xl_script.widgetHeight(vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartID);
            // $grid._resizeHandler();
        };
        //#endregion

        //#region render of the canvas charts
        var handleChartRenderOfCanvas = function (compile, scope, vm, widgetIndex) {

            //#region data of chart
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.data = vm.dashboardAndReportComInformation.widgetList[widgetIndex].data;
            //#endregion
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.height = Number(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.styleInfo.widgetHeight) - 30;

            //#region settings

            //#region title settings
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.title.text = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartTitleSettings.chartTitleName;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.title.fontSize = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartTitleSettings.fontSize;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.title.fontColor = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartTitleSettings.fontColor;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.title.fontFamily = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartTitleSettings.fontFamily;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.title.fontWeight = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartTitleSettings.fontWeight;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.title.fontStyle = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartTitleSettings.fontStyle;
            //#endregion

            //#region subtitle settings
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.subtitles[0].text = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartSubTitleSettings.chartSubtitleName;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.subtitles[0].fontSize = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartSubTitleSettings.fontSize;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.subtitles[0].fontColor = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartSubTitleSettings.fontColor;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.subtitles[0].fontFamily = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartSubTitleSettings.fontFamily;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.subtitles[0].fontWeight = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartSubTitleSettings.fontWeight;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.subtitles[0].fontStyle = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.chartSubTitleSettings.fontStyle;
            //#endregion

            //#region lengend settings

            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.legend.fontSize = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.fontSize;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.legend.fontColor = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.fontColor;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.legend.fontFamily = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.fontFamily;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.legend.fontWeight = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.fontWeight;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.legend.fontStyle = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.fontStyle;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.legend.verticalAlign = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.verticalAlign;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.legend.horizontalAlign = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.horizontalAlign;
            //#endregion

            //#region X-axis settings
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisX.title = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.title;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisX.titleFontSize = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.titleFontSize;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisX.titleFontColor = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.titleFontColor;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisX.titleFontWeight = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.titleFontWeight;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisX.titleFontStyle = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.titleFontStyle;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisX.labelFontColor = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.labelFontColor;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisX.labelFontSize = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.labelFontSize;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisX.labelFontWeight = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.labelFontWeight;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisX.labelFontStyle = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.labelFontStyle;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisX.prefix = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.prefix;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisX.suffix = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.XaxisSettings.suffix;
            //#endregion

            //#region Y-axis settings
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisY.title = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.title;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisY.titleFontSize = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.titleFontSize;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisY.titleFontColor = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.titleFontColor;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisY.titleFontWeight = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.titleFontWeight;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisY.titleFontStyle = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.titleFontStyle;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisY.labelFontColor = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.labelFontColor;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisY.labelFontSize = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.labelFontSize;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisY.labelFontWeight = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.labelFontWeight;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisY.labelFontStyle = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.labelFontStyle;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisY.prefix = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.prefix;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.options.axisY.suffix = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.YaxisSettings.suffix;
            //#endregion

            //#endregion

            //#region render of the chart and resize of the widger according to the chart widget
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.render();

            //xl_script.widgetHeight(vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartID);
            //$grid._resizeHandler();
            //#endregion
        };
        //#endregion

        //#region check initialization or render of the canvas chart
        var handleCheckInitializationOrRenderOfCanvasChart = function (compile, scope, vm, widgetIndex) {
            //&& vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.length > 0
            if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName.length > 0 && (vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.keycode)) {
                if (typeof vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization.render == "function") {
                    handleChartRenderOfCanvas(compile, scope, vm, widgetIndex);
                }
                else {
                    handleChartInitializationOfCanvas(compile, scope, vm, widgetIndex);
                }

            }
            //console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartInitialization);
        };
        //#endregion

        //#region check the company type of the chart
        var hanldeCheckChartCompanyType = function (compile, scope, vm, widgetIndex) {
            if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.keycode == "canvasCharts") {
                handleCheckInitializationOrRenderOfCanvasChart(compile, scope, vm, widgetIndex);
            }
            else {
                //still pending
            }
        };
        //#endregion

        //#region check the chart type
        var handleCheckChartType = function (compile, scope, vm, widgetIndex) {
            //debugger
            if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.keycode == "canvasCharts") {
                handleSetChartTypeOfCanvas(compile, scope, vm, widgetIndex);
                handleCheckInitializationOrRenderOfCanvasChart(compile, scope, vm, widgetIndex);
            }
            else {
                //
            }
        };
        //#endregion 

        //#region set the chart type of canvas
        var handleSetChartTypeOfCanvas = function (compile, scope, vm, widgetIndex) {
            if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.length > 0) {
                for (let index = 0; vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.length > index; index++) {
                    //vm.dashboardAndReportComInformation.widgetList[widgetIndex].data[index].type = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName;
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].data[index].showInLegend = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend;
                }
            }
        };
        //#endregion

        //#region resize of the all widget when penal size change
        var handlereSizeAllWidget = function (compile, scope, vm, widgetIndex, uiGridConstants, $timeout) {
            switch (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetType) {
                case "widgetOfChart":
                    $timeout(function () { handleCheckInitializationOrRenderOfCanvasChart(compile, scope, vm, widgetIndex); }, 280);
                    break;
                case "widgetOfTable":
                    if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].uiGridConfiguration.paginationStatus && vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.length > 0) {
                        $timeout(function () { handleUiGridSettingsAndInitialization(compile, scope, vm, uiGridConstants, widgetIndex); }, 150);
                    }
                    break;
                default:
            }


        };
        //#endregion

        //#region initialization of the high chart
        var handleChartInitializationOfHight = function (compile, scope, vm, widgetIndex) { };
        //#endregion

        //#region set path to the image controll and flag true
        var handleSetPathToTheImageControll = function (vm, extendCopyOfVm) {
            for (let itemIDx = 0; itemIDx < extendCopyOfVm.length; itemIDx++) {
                if (extendCopyOfVm[itemIDx].widgetType == "widgetOfDefault") {
                    for (let itemInnerIDx = 0; itemInnerIDx < extendCopyOfVm[itemIDx].fileInformation.length; itemInnerIDx++) {
                        extendCopyOfVm[itemIDx].defaultPageConfiguration.customImageControlPropertiesConfiguration[extendCopyOfVm[itemIDx].fileInformation[itemInnerIDx].ImageIndex].imageUrl = extendCopyOfVm[itemIDx].fileInformation[itemInnerIDx].filePath + extendCopyOfVm[itemIDx].fileInformation[itemInnerIDx].fileName + "." + extendCopyOfVm[itemIDx].fileInformation[itemInnerIDx].fileExtension;
                        vm.dashboardAndReportComInformation.widgetList[itemIDx].fileInformation[itemInnerIDx].save = true;
                    }

                }
            }
        };
        //#endregion

        //#region set value top, left, width and right of widget

        var handleSetValueTopLeftWidthAndRightOfWidget = function (getList) {
            let DefCalForWidthHeight = new jQuery.Deferred();
            let GridItems = xl_script.getGridItems();
            try {
                let getLengthOfDashboarAndReport = getList.widgetList.length;
                for (let i = 0; i < getLengthOfDashboarAndReport; i++) {
                    let getItem = getList.widgetList[i];

                    let getPropertiesOfGridStack = GridItems.find(x => x.widgetID == getItem.widgetID);
                    if (getPropertiesOfGridStack) {
                      //  console.log(getPropertiesOfGridStack);
                        getItem.widgetConfiguration.widgetTopLeftWidthAndHeight.top = getPropertiesOfGridStack.y;
                        getItem.widgetConfiguration.widgetTopLeftWidthAndHeight.left = getPropertiesOfGridStack.x;
                        getItem.widgetConfiguration.widgetTopLeftWidthAndHeight.width = getPropertiesOfGridStack.width;
                        getItem.widgetConfiguration.widgetTopLeftWidthAndHeight.height = getPropertiesOfGridStack.height;
                    }
                }

                DefCalForWidthHeight.resolve(getList);
            } catch (e) {
                DefCalForWidthHeight.reject();
            }

            return DefCalForWidthHeight.promise();

        };

        //#endregion

        //#region make json for database
        var handleJsonForDatabase = function (dashboardAndReportJson, vm, toastrs, scope) {
            let dashboardAndReportJsonRec = dashboardAndReportJson;
            makeJsonOfDatabase = {};
            makeJsonOfDatabase.pageID = dashboardAndReportJsonRec.pageID;
            makeJsonOfDatabase.templateTitle = dashboardAndReportJsonRec.templateTitle;
            makeJsonOfDatabase.templateType = dashboardAndReportJsonRec.templateType;
            makeJsonOfDatabase.TemplateID = dashboardAndReportJsonRec.TemplateID;
            makeJsonOfDatabase.save = dashboardAndReportJsonRec.save;
            makeJsonOfDatabase.publicTmp = dashboardAndReportJsonRec.publicTmp;
            makeJsonOfDatabase.IsActive = dashboardAndReportJsonRec.IsActive;
            makeJsonOfDatabase.description = dashboardAndReportJson.description;
            makeJsonOfDatabase.userID = userIDByX1;
            makeJsonOfDatabase.scheduler = dashboardAndReportJson.scheduler;
            makeJsonOfDatabase.widgetList = [];
            var getLengthOfDashboarAndReport = dashboardAndReportJsonRec.widgetList.length;
            if (getLengthOfDashboarAndReport > 0) {
                handleSetPathToTheImageControll(vm, dashboardAndReportJsonRec.widgetList);
                //console.log(dashboardAndReportJsonRec.widgetList);
                for (let i = 0; i < getLengthOfDashboarAndReport; i++) {
                    let getItem = dashboardAndReportJsonRec.widgetList[i];
                    getItem.widgetConfiguration.multiSchemaDesignerModels.subQueryStatus = false;
                    //if (!getItem.isDeleted)
                    //    handleSetValueTopLeftWidthAndRightOfWidget(getItem, getItem.widgetID);
                    let item = {};
                    item.PWidgetID = getItem.PWidgetID;
                    item.TemplateNodeID = getItem.TemplateNodeID;
                    item.usedDatasetsInEtl = getItem.usedDatasetsInEtl;
                    item.datasetName = getItem.datasetName;
                    if (getItem.widgetType == "widgetOfChart") {
                        item.widgetID = getItem.widgetID;
                        item.widgetName = getItem.widgetName;
                        item.isDeleted = getItem.isDeleted;
                        item.widgetType = getItem.widgetType;
                        delete getItem.chartConfigurations.chartInitialization;
                        item.widgetItemConfiguration = JSON.stringify(getItem.chartConfigurations);
                        item.widgetConfiguration = JSON.stringify(getItem.widgetConfiguration);
                        makeJsonOfDatabase.widgetList.push(item);
                    }
                    else if (getItem.widgetType == "widgetOfTable") {
                        item.widgetID = getItem.widgetID;
                        item.widgetName = getItem.widgetName;
                        item.isDeleted = getItem.isDeleted;
                        item.widgetType = getItem.widgetType;
                        if (typeof getItem.uiGridConfiguration.uiGridInitailization != "undefined") {
                            delete getItem.uiGridConfiguration.uiGridInitailization;
                            delete getItem.uiGridConfiguration.gridApi;

                        }
                        item.widgetItemConfiguration = JSON.stringify(getItem.uiGridConfiguration);
                        item.widgetConfiguration = JSON.stringify(getItem.widgetConfiguration);
                        makeJsonOfDatabase.widgetList.push(item);
                    }
                    else if (getItem.widgetType == "widgetOfMap") {
                        item.widgetID = getItem.widgetID;
                        item.widgetName = getItem.widgetName;
                        item.isDeleted = getItem.isDeleted;
                        item.widgetType = getItem.widgetType;
                        item.widgetConfiguration = JSON.stringify(getItem.widgetConfiguration);
                        makeJsonOfDatabase.widgetList.push(item);
                    }
                    else if (getItem.widgetType == "widgetOfDefault") {
                        item.widgetID = getItem.widgetID;
                        item.widgetName = getItem.widgetName;
                        item.isDeleted = getItem.isDeleted;
                        item.fileInformation = getItem.fileInformation.length > 0 ? getItem.fileInformation.filter(x => x.save == false) != undefined ? getItem.fileInformation.filter(x => x.save == false) : [] : [];
                        item.widgetType = getItem.widgetType;
                        item.widgetItemConfiguration = JSON.stringify(getItem.defaultPageConfiguration);
                        item.widgetConfiguration = JSON.stringify(getItem.widgetConfiguration);
                        makeJsonOfDatabase.widgetList.push(item);
                    }
                    else if (getItem.widgetType == "widgetOfTab") {
                        //item.widgetID = getItem.widgetID;
                        //item.widgetName = getItem.widgetName;
                        //item.isDeleted = getItem.isDeleted;
                        //item.widgetType = getItem.widgetType;
                        //item.widgetItemConfiguration = JSON.stringify(getItem.tabConfiguration);
                        //item.widgetConfiguration = JSON.stringify(getItem.widgetConfiguration);
                        //makeJsonOfDatabase.widgetList.push(item);

                        item.widgetID = getItem.widgetID;
                        item.widgetName = getItem.widgetName;
                        item.isDeleted = getItem.isDeleted;
                        item.widgetType = getItem.widgetType;
                        getItem.widgetConfiguration.TabsInfo = xl_script.getTabsWidgetsInfo(getItem.widgetID);
                        item.widgetConfiguration = JSON.stringify(getItem.widgetConfiguration);
                        makeJsonOfDatabase.widgetList.push(item);
                    }
                    else if (getItem.widgetType == "widgetOfDocument") {
                        item.widgetID = getItem.widgetID;
                        item.widgetName = getItem.widgetName;
                        item.isDeleted = getItem.isDeleted;
                        item.widgetType = getItem.widgetType;
                        item.widgetItemConfiguration = JSON.stringify(getItem.documentsViewConfiguration);
                        item.widgetConfiguration = JSON.stringify(getItem.widgetConfiguration);
                        makeJsonOfDatabase.widgetList.push(item);
                    }



                }
            }

         //   console.log(makeJsonOfDatabase)
            $http({
                method: 'POST',
                url: '/BusinessIntelligence/DashboardNReport/PostTemplateForETL',
                data: { GetTemplate: makeJsonOfDatabase }
            }).then(function successCallback(response) {

                toastrs.success(response.data.msg);
                vm.dashboardAndReportComInformation.save = true;
                scope.$emit('refreshDropdownList');
                var etljob = $.extend(true, {}, vm.dashboardAndReportComInformation.scheduler);
                //etljob['description'] = vm.dashboardAndReportComInformation['description'];
                //etljob['templateTitle'] = vm.dashboardAndReportComInformation['templateTitle'];
                //etljob['templateType'] = vm.dashboardAndReportComInformation['templateType'];

                scope.$emit('setEtlJobParamsForEditE', etljob);

                // this callback will be called asynchronously
                // when the response is available
            }, function errorCallback(response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
            });




        };
        //#endregion

        //#region change mode type
        var handleChangeModeType = function (compile, scope, vm, modeType) {

            vm.modeType = modeType;

        };
        //#endregion

        //#region make json for frontend
        var handleJsonForFrontend = function (compile, scope, vm, uiGridConstants, DashboardAndReportJsonFromDb, rootScope) {
            let makeJsonForFrontend = {};
            makeJsonForFrontend.pageID = DashboardAndReportJsonFromDb.pageID;
            makeJsonForFrontend.templateTitle = DashboardAndReportJsonFromDb.templateTitle;
            makeJsonForFrontend.templateType = DashboardAndReportJsonFromDb.templateType;
            makeJsonForFrontend.TemplateID = DashboardAndReportJsonFromDb.TemplateID;
            makeJsonForFrontend.IsActive = DashboardAndReportJsonFromDb.IsActive;
            makeJsonForFrontend.publicTmp = DashboardAndReportJsonFromDb.publicTmp;
            if (DashboardAndReportJsonFromDb.IsActive) {

                scope.$emit('setDefaultTemplateFromDashboardReportE', true);
            }
            else {
                scope.$emit('setDefaultTemplateFromDashboardReportE', false);

            }
            if (DashboardAndReportJsonFromDb.publicTmp) {
                scope.$emit('setPublicTemplateFromDashboardReportE', true);
            }
            else {
                scope.$emit('setPublicTemplateFromDashboardReportE', false);
            }
            makeJsonForFrontend.viewsInformation = [];
            makeJsonForFrontend.widgetList = [];
            makeJsonForFrontend.documentPath = [];
            let getLengthOfDashboarAndReport = DashboardAndReportJsonFromDb.widgetList.length;
            if (getLengthOfDashboarAndReport > 0) {
                for (let i = 0; i < getLengthOfDashboarAndReport; i++) {
                    let getItem = DashboardAndReportJsonFromDb.widgetList[i];
                    let item = {};
                    item.murriInitializationOfSelectedColumn = [];
                    item.SelectedTablesColumnName = [];
                    item.tempData = [];
                    item.treeviewOfMultiDatasetsWithItsColumns = [];
                    item.TemplateNodeID = getItem.TemplateNodeID;
                    item.mxKeyHandler;
                    item.mxModel;
                    item.mxGraph;
                    item.PWidgetID = getItem.PWidgetID;
                    item.mxEdgesStyle;
                    if (getItem.widgetType == "widgetOfChart") {
                        item.widgetID = getItem.widgetID;
                        item.widgetName = getItem.widgetName;
                        item.isDeleted = getItem.isDeleted;
                        item.widgetType = getItem.widgetType;
                        item.chartConfigurations = JSON.parse(getItem.widgetItemConfiguration);
                        item.widgetConfiguration = JSON.parse(getItem.widgetConfiguration);
                        item.chartConfigurations.chartInitialization = {};
                        item.data = [];
                        makeJsonForFrontend.widgetList.push(item);
                    }
                    else if (getItem.widgetType == "widgetOfTable") {
                        item.widgetID = getItem.widgetID;
                        item.widgetName = getItem.widgetName;
                        item.isDeleted = getItem.isDeleted;
                        item.widgetType = getItem.widgetType;
                        item.uiGridConfiguration = JSON.parse(getItem.widgetItemConfiguration);
                        item.widgetConfiguration = JSON.parse(getItem.widgetConfiguration);
                        item.data = [];
                        makeJsonForFrontend.widgetList.push(item);
                    }
                    else if (getItem.widgetType == "widgetOfMap") {
                        item.widgetID = getItem.widgetID;
                        item.widgetName = getItem.widgetName;
                        item.isDeleted = getItem.isDeleted;
                        item.widgetType = getItem.widgetType;
                        item.widgetConfiguration = JSON.parse(getItem.widgetConfiguration);
                        item.data = [];
                        makeJsonForFrontend.widgetList.push(item);
                    }
                    else if (getItem.widgetType == "widgetOfDefault") {
                        item.widgetID = getItem.widgetID;
                        item.widgetName = getItem.widgetName;
                        item.isDeleted = getItem.isDeleted;
                        item.defaultPageConfiguration = JSON.parse(getItem.widgetItemConfiguration);
                        item.widgetType = getItem.widgetType;
                        item.widgetConfiguration = JSON.parse(getItem.widgetConfiguration);
                        item.data = [];
                        delete item.fileInformation;
                        item.fileInformation = [];
                        makeJsonForFrontend.widgetList.push(item);
                    }
                    else if (getItem.widgetType == "widgetOfTab") {
                        //item.widgetID = getItem.widgetID;
                        //item.widgetName = getItem.widgetName;
                        //item.isDeleted = getItem.isDeleted;
                        //item.widgetType = getItem.widgetType;
                        //item.tabConfiguration = JSON.parse(getItem.widgetItemConfiguration);
                        //item.widgetConfiguration = JSON.parse(getItem.widgetConfiguration);
                        //makeJsonForFrontend.widgetList.push(item);
                        item.widgetID = getItem.widgetID;
                        item.widgetName = getItem.widgetName;
                        item.isDeleted = getItem.isDeleted;
                        item.widgetType = getItem.widgetType;
                        getItem.widgetConfiguration.ChildOfWidgetId = "-1";
                        item.widgetConfiguration = JSON.parse(getItem.widgetConfiguration);
                        item.data = [];
                        makeJsonForFrontend.widgetList.push(item);
                    }
                    else if (getItem.widgetType == "widgetOfDocument") {
                        item.widgetID = getItem.widgetID;
                        item.widgetName = getItem.widgetName;
                        item.isDeleted = getItem.isDeleted;
                        item.widgetType = getItem.widgetType;
                        item.documentsViewConfiguration = JSON.parse(getItem.widgetItemConfiguration);
                        item.widgetConfiguration = JSON.parse(getItem.widgetConfiguration);
                        makeJsonForFrontend.widgetList.push(item);
                    }
                }
            }
       //     console.log(makeJsonForFrontend);
            vm.dashboardAndReportComInformation = makeJsonForFrontend;
            //vm.dashboardAndReportComInformation.widgetList.sort(function (a, b) { return a.widgetConfiguration.pagePosition - b.widgetConfiguration.pagePosition; });
            vm.dashboardAndReportComInformation.save = true;
            getViewsOfDb(vm);
            handleDocumentPath(vm, scope, compile);
            handleDrawWidgets(compile, scope, vm, function () {
                setTimeout(function () {
                    //scope.$emit('murri');
                    scope.$emit('initializationOfGridStack');

                }, 100);
            }, uiGridConstants, rootScope);


        };
        //#endregion

        //#region start code
        var handleFrontEndForCloneWidget = function (compile, scope, vm, uiGridConstants, cloneWidgetJson, rootScope, indexOfWidget) {
            vm.dashboardAndReportComInformation.widgetList.push(cloneWidgetJson);
            vm.dashboardAndReportComInformation.widgetList.sort(function (a, b) { return a.widgetConfiguration.pagePosition - b.widgetConfiguration.pagePosition; });
            vm.dashboardAndReportComInformation.save = true;
            handleRenderClonedWidget(compile, scope, vm, function () {
                setTimeout(function () { scope.$emit('murri'); }, 100);
            }, uiGridConstants, rootScope, indexOfWidget);
        };
        //#endregion end code

        //#region start code
        var handleRenderClonedWidget = function (compile, scope, vm, callback, uiGridConstants, rootScope) {

            //scope.$emit('murri');
            var index = vm.dashboardAndReportComInformation.widgetList.length - 1;
            let getLengthOfWidgetList = vm.dashboardAndReportComInformation.widgetList.length;
            let parentElement = addWidgetOfGridStackItem;
            let settingsAndAdvanceSettingsSelector = $(".side-panel .tab-content .tab-pane > .toggle");
            let settingsElement = getWidgetSettings.find(x => x.type == vm.dashboardAndReportComInformation.widgetList[index].widgetType).settings;
            let advanceSettingsElement = getWidgetSettings.find(x => x.type == vm.dashboardAndReportComInformation.widgetList[index].widgetType).advanced_settings;
            let element = getWidgetSettings.find(x => x.type == vm.dashboardAndReportComInformation.widgetList[index].widgetType).code;
            var queryBuilerEle = getWidgetSettings.find(x => x.type == "queryBuilder").code;
            if (!vm.dashboardAndReportComInformation.widgetList[index].isDeleted) {
                handleRenderWidget(element, parentElement, settingsAndAdvanceSettingsSelector, settingsElement, advanceSettingsElement, queryBuilerEle, $(".side-panel #queryBuilderSection"), compile, scope, vm, index, vm.dashboardAndReportComInformation.widgetList[index].widgetType, uiGridConstants, rootScope, true);
            }
            callback();
        };
        //#endregion end code

        //#region get data from query
        var handleGetQueryResult = function (queryBuild) {
            return $.ajax({
                type: "POST",
                url: '/BusinessIntelligence/QueryBuilder/GetQueryResult',
                data: { query: queryBuild }
            });
        };
        //#endregion

        //#region get data from database and set to the dataset for Maps
        var handleGetDataFromDbForMap = function (widgetIndex, QueryBuild, vm, toasterStatus) {
            return handleGetQueryResult(QueryBuild).success(function (result) {
                if (result.status) {
                    var getData = JSON.parse(result.data);
                    if (getData.length > 0) {
                        toastrs.success("Query has been executed");
                    }
                    else {
                        toastrs.error("Data has not been found");
                    }
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].data = getData;
                    // code start
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].tempData = getData;
                    handleFillTreeviewFilterationOfAllWidgets(vm, widgetIndex);
                    //if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.mapleveltype == 'Market')
                    //    $timeout(function () { vm.changemarketfiltercountry(widgetIndex); },200) 
                    // code end
                    //if (toasterStatus)
                    //    toastrs.success("Query has been executed");
                    //}else if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.sqlQueryBuild.length > 0) { }
                    //else {
                    //    toastrs.error("Data has not been found");
                    //}

                    ///
                }
                $('#' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.overlay').attr('style', 'display:none;');

            });
        };
        //#endregion

        //#region get data from database and set to the dataset for page
        var handleGetDataFromDbForDefaultPage = function (widgetIndex, QueryBuild, vm, toasterStatus) {
            return handleGetQueryResult(QueryBuild).success(function (result) {
                if (result.status) {

                    var getData = JSON.parse(result.data);
                  //  console.log(getData);
                    if (getData.length > 0)
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].data = getData;
                    //if (toasterStatus)
                    //    toastrs.success("Query has been executed");
                }
                else {
                    toastrs.error("Data has not been found ");
                }


                $('#' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.overlay').attr('style', 'display:none;');

            });
        };
        //#endregion

        //#region get data from database and set to the dataset for table and ui grid
        var handleGetDataFromDbForTable = function (widgetIndex, QueryBuild, vm, toasterStatus) {
            return handleGetQueryResult(QueryBuild).success(function (result) {
                if (result.status) {
                    var getData = JSON.parse(result.data);

                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].data = getData;
                    if (toasterStatus)
                        toastrs.success("Query has been executed");
                }
                else {
                    toastrs.error("Data has not been found");
                }

                $('#' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.overlay').attr('style', 'display:none;');

            });
        };
        //#endregion

        //#region get chart and company type when got data from database
        var handleGetChartAndCompanytype = function (widgetIndex, vm, getResult, toastrs) {

            if (getResult.status) {
                var getData = JSON.parse(getResult.data);
                if ((getData.length > 0 && vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis != undefined && vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis != undefined) || vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName == "multiseries") {

                    if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.keycode == "canvasCharts") {
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].data = [];
                        switch (vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName) {
                            case "line":
                                handleLineChart(widgetIndex, vm, getData);
                                break;
                            case "pie":
                                handlePieChart(widgetIndex, vm, getData);
                                break;
                            case "bar":
                                handleBarChart(widgetIndex, vm, getData);
                                break;
                            case "area":
                                handleAresChart(widgetIndex, vm, getData);
                                break;
                            case "stackedColumn":
                                handleStackColummnChart(widgetIndex, vm, getData);
                                break;
                            case "stackedBar":
                                handleStackBarChart(widgetIndex, vm, getData);
                                break;
                            case "multiseries":
                                handleMultiSeriesChartWithoutGroup(widgetIndex, vm, getData);
                                break;
                            case "doughnut":
                                handleDoughnutChart(widgetIndex, vm, getData);
                                break;
                            case "funnel":
                                handleFunnelChart(widgetIndex, vm, getData);
                                break;
                            case "column":
                                handleColumnChart(widgetIndex, vm, getData);
                                break;
                            case "waterfall":
                                handleWaterFallChart(widgetIndex, vm, getData);
                                break;
                            case "stepline":
                                handleStepLineChart(widgetIndex, vm, getData);
                                break;
                            case "spline":
                                handleSplineChart(widgetIndex, vm, getData);
                                break;
                            case "splineArea":
                                handleSplineAreaChart(widgetIndex, vm, getData);
                                break;
                            case "steparea":
                                handleStepAreaChart(widgetIndex, vm, getData);
                                break;
                            case "rangebar":
                                handleRangeBarChart(widgetIndex, vm, getData);
                                break;
                            case "pyramid":
                                handlePyramidChart(widgetIndex, vm, getData);
                                break;
                            case "candlestick":
                                handleCandleStickChart(widgetIndex, vm, getData);
                                break;
                            case "scatter":
                                handleScatterChart(widgetIndex, vm, getData);
                                break;
                            case "boxAndWhisker":
                                handleBoxAndWhiskerChart(widgetIndex, vm, getData);
                                break;
                            case "error":
                                handleErrorLineChart(widgetIndex, vm, getData);
                                break;
                            default:

                        }

                    }
                    else if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.keycode == "highCharts") {
                        //logic
                    }
                    else {
                        //logic

                    }
                }
                $('#' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.overlay').attr('style', 'display:none;');
            }

        };
        //#endregion 

        //#region get data from database and set to the dataset for charts
        var handleGetDataFromDbForCharts = function (widgetIndex, QueryBuild, vm, toastrs, scope, compile) {
            return handleGetQueryResult(QueryBuild).success(function (result) {

                vm.dashboardAndReportComInformation.widgetList[widgetIndex].tempData = result;
                //   handleGetChartAndCompanytype(widgetIndex, vm, result, toastrs);
                handleGetDataToGevenValueForCharts(vm, widgetIndex, scope, compile);

            });
        };
        //#endregion

        //#region make data of line chart for canvas
        var handleLineChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis,
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: []
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region make data of Pie chart for canvas
        var handlePieChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: "{indexLabel}",
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: []
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), indexLabel: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region make data of Bar chart for canvas
        var handleBarChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                color: "#DCA978",
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis,
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: []
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);



        };
        //#endregion

        //#region make data of Area chart for canvas
        var handleAresChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis,
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: []
            };

            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region sort by name
        function SortByName(a, b) {
            var aName = a.Issue.toLowerCase();
            var bName = b.Issue.toLowerCase();
            return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));


        }
        //#endregion

        //#region make data of stack column chart for canvas
        var handleStackColummnChart = function (widgetIndex, vm, data) {

            let makeGroupData = hanldeGroupByStackBarColumnChartProperty(widgetIndex, vm, data);
            let GetFormat = handleGetUniqueListWithFormatStackChart(widgetIndex, vm, data, vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis);
            if (makeGroupData.length > 0) {
                for (let idx = 0; idx < makeGroupData.length; idx++) {
                    let getItem = makeGroupData[idx];
                    if (getItem.chartData.length > 0) {

                        let makeStackChart = {

                            showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                            legendText: getItem[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.seriesOnGroup],
                            type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                            dataPoints: []
                        };
                        let getformatForStackOnRunTime = $.extend(true, [], GetFormat);
                        for (let idxInner = 0; idxInner < getItem.chartData.length; idxInner++) {
                            let data = getItem.chartData[idxInner];
                            let getItemOnSeries = getformatForStackOnRunTime.find(x => x.label == data[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis]);
                            if (getItemOnSeries) {
                                getItemOnSeries.y = parseFloat(data[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]);
                                getItemOnSeries.label = data[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis];
                                getItemOnSeries['toolTipContent'] = getItem[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.seriesOnGroup] + ' ' + parseFloat(data[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]);
                            }


                        }
                        makeStackChart.dataPoints = getformatForStackOnRunTime;
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeStackChart);
                    }
                }
            }


            //  console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].data);


        };
        //#endregion

        //#region group for stack bar & column
        var hanldeGroupByStackBarColumnChartProperty = function (widgetIndex, vm, data) {
            var group_to_values = data.reduce(function (obj, item) {
                obj[item[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.seriesOnGroup]] = obj[item[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.seriesOnGroup]] || [];
                let makeJson = {};
                makeJson[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] = item[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis];
                makeJson[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis] = item[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis];
                makeJson[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.seriesOnGroup] = item[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.seriesOnGroup];

                obj[item[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.seriesOnGroup]].push(makeJson);
                return obj;
            }, {});

            var groups = Object.keys(group_to_values).map(function (key) {
                return { [vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.seriesOnGroup]: key, chartData: group_to_values[key] };
            });
            // console.log(groups);
            return groups;

        };
        //#endregion

        //#region group for multi chart
        var hanldeGroupByMultiChartProperty = function (widgetIndex, vm, data, xAxisLength, yAxisLength) {
            var group_to_values = data.reduce(function (obj, item) {
                obj[item[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.seriesOnGroup]] = obj[item[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.seriesOnGroup]] || [];
                let makeJson = {};
                for (let xIndex = 0; xIndex < xAxisLength; xIndex++) {
                    makeJson[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis[xIndex].xAxisColumnNameMul] = item[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis[xIndex].xAxisColumnNameMul];
                }
                for (let yIndex = 0; yIndex < yAxisLength; yIndex++) {
                    makeJson[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisColumnNameMul] = item[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisColumnNameMul];
                }

                makeJson[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.seriesOnGroup] = item[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.seriesOnGroup];

                obj[item[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.seriesOnGroup]].push(makeJson);
                return obj;
            }, {});

            var groups = Object.keys(group_to_values).map(function (key) {
                return { [vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.seriesOnGroup]: key, chartData: group_to_values[key] };
            });

            return groups;

        };
        //#endregion

        //#region make data of stack bar chart for canvas
        var handleStackBarChart = function (widgetIndex, vm, data) {

            let makeGroupData = hanldeGroupByStackBarColumnChartProperty(widgetIndex, vm, data);
            let GetFormat = handleGetUniqueListWithFormatStackChart(widgetIndex, vm, data, vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis);
            if (makeGroupData.length > 0) {
                for (let idx = 0; idx < makeGroupData.length; idx++) {
                    let getItem = makeGroupData[idx];
                    if (getItem.chartData.length > 0) {

                        let makeStackChart = {

                            showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                            legendText: getItem[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.seriesOnGroup],
                            type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                            dataPoints: []
                        };
                        let getformatForStackOnRunTime = $.extend(true, [], GetFormat);
                        for (let idxInner = 0; idxInner < getItem.chartData.length; idxInner++) {
                            let data = getItem.chartData[idxInner];
                            let getItemOnSeries = getformatForStackOnRunTime.find(x => x.label == data[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis]);
                            if (getItemOnSeries) {
                                getItemOnSeries.y = parseFloat(data[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]);
                                getItemOnSeries.label = data[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis];
                                getItemOnSeries['toolTipContent'] = getItem[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.seriesOnGroup] + ' ' + parseFloat(data[vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]);
                            }


                        }
                        makeStackChart.dataPoints = getformatForStackOnRunTime;
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeStackChart);
                    }
                }
            }

        };
        //#endregion

        //#region make data of multi series chart for canvas
        var handleMultiSeriesChartWithoutGroup = function (widgetIndex, vm, data) {
            let checkstatusOfxaxis = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis.find(x => x.xAxisColumnNameMul == undefined);
            let checkstatusOfyaxis = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis.find(x => x.yAxisColumnNameMul == undefined || x.yAxisChartType == undefined);
            if (!checkstatusOfxaxis && !checkstatusOfyaxis) {
                if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.seriesOnGroup)
                    handleMakeDataForMultiSeriesGroup(widgetIndex, vm, data);
                else
                    handleMakeDataForMultiSeriesWithoutGroup(widgetIndex, vm, data);

            }

        };
        //#endregion

        //#region make data for multi series with group
        var handleMakeDataForMultiSeriesGroup = function (widgetIndex, vm, data) {
            var xAxisLength = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis.length;
            var yAxisLength = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis.length;
            let makeGroupData = hanldeGroupByMultiChartProperty(widgetIndex, vm, data, xAxisLength, yAxisLength);
            if (xAxisLength > 0 && yAxisLength > 0) {

                for (let xIndex = 0; xIndex < xAxisLength; xIndex++) {
                    let GetFormat = handleGetUniqueListWithFormatStackChart(widgetIndex, vm, data, vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis[xIndex].xAxisColumnNameMul);

                    for (let yIndex = 0; yIndex < yAxisLength; yIndex++) {
                        let getformatForStackOnRunTime = $.extend(true, [], GetFormat);
                        for (let grpIdx = 0; grpIdx < makeGroupData.length; grpIdx++) {
                            let getChartData = makeGroupData[grpIdx].chartData;
                            let legendFormating = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].columnTitle == "" ? vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisColumnNameMul : vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].columnTitle;
                            let makeMultiSeriesChart = {
                                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                                legendText: makeGroupData[grpIdx][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.seriesOnGroup] + ' ' + legendFormating,
                                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisChartType,
                                color: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisColorPicker,
                                dataPoints: []
                            };

                            makeMultiSeriesChart.dataPoints = handleMakeXAndYDataFormatFromDbWithGroup(getChartData, vm, widgetIndex, xIndex, yIndex, getformatForStackOnRunTime);
                            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeMultiSeriesChart);
                        }

                    }

                }
            }
            //console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].data);
        };

        //#endregion

        //#region make data for multi series for without group
        var handleMakeDataForMultiSeriesWithoutGroup = function (widgetIndex, vm, data) {
            var xAxisLength = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis.length;
            var yAxisLength = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis.length;
            if (xAxisLength > 0 && yAxisLength > 0) {

                for (let xIndex = 0; xIndex < xAxisLength; xIndex++) {
                    let GetFormat = handleGetUniqueListWithFormatStackChart(widgetIndex, vm, data, vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis[xIndex].xAxisColumnNameMul);

                    for (let yIndex = 0; yIndex < yAxisLength; yIndex++) {
                        let getformatForStackOnRunTime = $.extend(true, [], GetFormat);
                        let legendFormating = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].columnTitle == "" ? vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisColumnNameMul : vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].columnTitle;
                        let makeMultiSeriesChart = {
                            showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                            legendText: legendFormating,
                            type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisChartType,
                            color: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisColorPicker,
                            axisYType: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisSecPriType,
                            dataPoints: []
                        };

                        makeMultiSeriesChart.dataPoints = handleMakeXAndYDataFormatFromDbWithoutGroup(data, vm, widgetIndex, xIndex, yIndex, getformatForStackOnRunTime);
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeMultiSeriesChart);

                    }

                }
            }

        };
        //#endregion

        //#region make x and y with format of data without group ,getting data from db
        var handleMakeXAndYDataFormatFromDbWithoutGroup = function (data, vm, widgetIndex, xIndex, yIndex, getformatForStackOnRunTime) {
            let getformatForStackOnRunTimeReturn = $.extend(true, [], getformatForStackOnRunTime);
            for (let dataIndex = 0; dataIndex < data.length; dataIndex++) {
                let getItemOnSeries = getformatForStackOnRunTimeReturn.find(x => x.label == data[dataIndex][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis[xIndex].xAxisColumnNameMul]);
                if (getItemOnSeries) {
                    getItemOnSeries.y = parseFloat(data[dataIndex][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisColumnNameMul]);
                    getItemOnSeries.label = data[dataIndex][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis[xIndex].xAxisColumnNameMul];
                    getItemOnSeries['toolTipContent'] = vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisColumnNameMul + ' ' + parseFloat(data[dataIndex][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisColumnNameMul]);
                }

            }
            return getformatForStackOnRunTimeReturn;
        };
        //#endregion

        //#region make x and y with format of data with group ,getting data from db
        var handleMakeXAndYDataFormatFromDbWithGroup = function (data, vm, widgetIndex, xIndex, yIndex, getformatForStackOnRunTime) {
            let getformatForStackOnRunTimeReturn = $.extend(true, [], getformatForStackOnRunTime);
            for (let dataIndex = 0; dataIndex < data.length; dataIndex++) {
                let getItemOnSeries = getformatForStackOnRunTimeReturn.find(x => x.label == data[dataIndex][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis[xIndex].xAxisColumnNameMul]);
                if (getItemOnSeries) {
                    getItemOnSeries.y = parseFloat(data[dataIndex][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisColumnNameMul]);
                    getItemOnSeries.label = data[dataIndex][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis[xIndex].xAxisColumnNameMul];
                    getItemOnSeries['toolTipContent'] = data[dataIndex][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.seriesOnGroup] + ' ' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisColumnNameMul + ' ' + parseFloat(data[dataIndex][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis[yIndex].yAxisColumnNameMul]);
                }

            }
            return getformatForStackOnRunTimeReturn;
        };
        //#endregion

        //#region make data of waterfall chart for canvas
        var handleStepLineChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: "{y}",
                markerSize: 5,
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: []
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion 

        //#region make data of waterfall chart for canvas
        var handleWaterFallChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: "{label}",
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: []
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region make data of chart Column for canvas
        var handleColumnChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: "{label}",
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: []
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion  

        //#region make data of Funnel chart for canvas
        var handleFunnelChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: "{label}",
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                toolTipContent: "<b>{label}</b>: {y}",
                indexLabel: "{label} ({y})",
                //   toolTipContent: "<b>{label}</b>: {y} <b>({percentage}%)</b>",
                // indexLabel: "{label} ({percentage}%)",
                dataPoints: []
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            makeLineChart.dataPoints.sort(function (a, b) { return b.y - a.y });
            // Calculate Percentage
            //var dataPoint = makeLineChart.dataPoints;
            //if (dataPoint.length > 0) {
            //    var total = dataPoint[0].y;
            //    for (var i = 0; i < dataPoint.length; i++) {
            //        makeLineChart.dataPoints[i].percentage = ((dataPoint[i].y / total) * 100).toFixed(2);
            //    }
            //}

            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region make data of Doughnut chart for canvas
        var handleDoughnutChart = function (widgetIndex, vm, data) {
            let makeDoughnutChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: "{label}",
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                startAngle: 60,
                //innerRadius: 60,
                indexLabelFontSize: 17,
                indexLabel: "{label} - #percent%",
                toolTipContent: "<b>{label}:</b> {y} (#percent%)",
                dataPoints: []
            };
            $(data).each(function (i) {
                makeDoughnutChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeDoughnutChart);

        };
        //#endregion

        //#region make data of StepAreaChart for canvas 
        var handleStepAreaChart = function (widgetIndex, vm, data) {

            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: "{x}",
                type: 'stepArea',
                dataPoints: []
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region make data of Range Bar chart for canvas
        var handleRangeBarChart = function (widgetIndex, vm, data) {
            data.sort(function (a, b) {
                a = parseFloat(vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis);
                b = parseFloat(vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis);
                return a - b
            })
            var s = -1;
            let keepValueOfS = 0;
            var totalDataLength = data.length;
            var lastMaxValue = data[totalDataLength - 1][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis];
            var increament = lastMaxValue / totalDataLength;
            let makeLineChart = {
                type: "rangeBar",
                showInLegend: true,
                yValueFormatString: "$#0.#K",
                indexLabel: "{y[#index]}",
                legendText: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis,
                toolTipContent: "<b>{label}</b>:{y[0]} to {y[1]}",
                dataPoints: []
            };
            $(data).each(function (i) {
                keepValueOfS = s + 1;
                s = s + increament;
                makeLineChart.dataPoints.push({
                    x: s, y: [keepValueOfS, s], label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis]
                });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region make data of pyramid chart for canvas
        var handlePyramidChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis,
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: []
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region make data of candle stick chart for canvas
        var handleCandleStickChart = function (widgetIndex, vm, data) {
            let min = 0, max = 0, average = 0;
            //$(data).each(function (i) {

            //});
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis,
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: []
            };
            $(data).each(function (i) {
                if ((data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]) < min) {
                    min = data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis];
                }
                if ((data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]) > max) {
                    max = data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis];
                }
                let open = data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis];
                makeLineChart.dataPoints.push({ x: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] + (Math.floor((Math.random() * 20) + 1)), y: [open, max, min, (Math.floor((Math.random() * 100) + 1))] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region make data of Scatter Chart for canvas
        var handleScatterChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis,
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: [],
                toolTipContent: "<b> </b>{x}<br/><b></b>{y}",
                //toolTipContent: "<b>Temperature: </b>{x}°C<br/><b>Sales: </b>{y}",
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ x: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis], y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]) });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion 

        //#region make data of Box And WhiskerChart chart for canvas
        var handleBoxAndWhiskerChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis,
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: []
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ x: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis], y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]) });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region make data of Error Line Chart for canvas
        var handleErrorLineChart = function (widgetIndex, vm, data) {
            let min = 10000, max = 0;
            let data1 = []
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis,
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: [],
                markerColor: "white",
                markerBorderColor: "#4F81BC",
                markerSize: 9,
                markerBorderThickness: 3,
            };
            //render orignal y points
            $(data).each(function (i) {
                if (parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]) > max) {
                    max = data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis];
                }
                if (parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]) < min) {
                    min = data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis];
                }
                makeLineChart.dataPoints.push({ y: [min, max], label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);
            //render orignal y points
            makeLineChart.type = "line";
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({
                    y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis])
                    , label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis]
                });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region make data of Spline chart for canvas
        var handleSplineChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: "{y}",
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: []
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region make data of Spline Area chart for canvas
        var handleSplineAreaChart = function (widgetIndex, vm, data) {
            let makeLineChart = {
                showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
                legendText: "{y}",
                type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
                dataPoints: []
            };
            $(data).each(function (i) {
                makeLineChart.dataPoints.push({ y: parseFloat(data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.yAxis]), label: data[i][vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.xAxis] });
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].data.push(makeLineChart);

        };
        //#endregion

        //#region handleResetMultSeriesGroup
        var handleResetMultSeriesGroup = function (vm, widgetIndex) { vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.seriesOnGroup = undefined; };
        //#endregion

        //#region handleResetYaxisMul
        var handleResetYaxisMul = function (vm, widgetIndex) {
            let multiSeriesYaxisParams = { yAxisColumnNameMul: undefined, yAxisChartType: undefined, yAxisColorPicker: getRandomColor(), yAxisSecPriType: "secondary", columnTitle: "" };
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis = [];
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis.push(multiSeriesYaxisParams);
        };
        //#endregion

        //#region handleResetXaxisMul
        var handleResetXaxisMul = function (vm, widgetIndex) {
            let multiSeriesXaxisParams = { xAxisColumnNameMul: undefined };
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis = [];
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis.push(multiSeriesXaxisParams);
        };
        //#endregion

        //#region get unique list with format of stack chart;
        var handleGetUniqueListWithFormatStackChart = function (widgetIndex, vm, data, columnName) {
            var getUniqueSeries = data.map(item => item[columnName])
                .filter((value, index, self) => self.indexOf(value) === index && value != null);
            let dataPoints = [];
            for (let idxInner = 0; idxInner < getUniqueSeries.length; idxInner++) {

                dataPoints.push({ "y": 0, "label": getUniqueSeries[idxInner] });
            }
            return dataPoints;
        };
        //#endregion

        //#region check call to the render for simple table or ui grid
        var handleCheckRenderTableOrUiGrid = function (compile, scope, vm, uiGridConstants, index, toastrStatus) {
            let GetQuery = vm.dashboardAndReportComInformation.widgetList[index].widgetConfiguration.sqlQueryBuild;

            if (GetQuery.length > 0) {
                $('#' + vm.dashboardAndReportComInformation.widgetList[index].widgetID).find('.overlay').attr('style', 'display:block;');
                if (vm.dashboardAndReportComInformation.widgetList[index].uiGridConfiguration.paginationStatus) {
                    GetQuery = handleMakeQueryBeforeRenderGrid(GetQuery, vm.dashboardAndReportComInformation.widgetList[index].uiGridConfiguration.paginationParams.paginationSize, 1, vm.dashboardAndReportComInformation.widgetList[index].uiGridConfiguration.paginationParams.orderBy);
                }
                else {
                    //    $('#' + vm.dashboardAndReportComInformation.widgetList[index].widgetID).find('.overlay').attr('style', 'display:block;');
                    GetQuery += vm.dashboardAndReportComInformation.widgetList[index].uiGridConfiguration.paginationParams.orderBy;
                }

                //console.log(GetQuery);
                handleGetDataFromDbForTable(index, GetQuery, vm, toastrStatus).done(function (response) {
                    if (vm.dashboardAndReportComInformation.widgetList[index].uiGridConfiguration.paginationStatus) {
                        //debugger
                        if (vm.dashboardAndReportComInformation.widgetList[index].data.length > 0) {
                            vm.dashboardAndReportComInformation.widgetList[index].uiGridConfiguration.paginationParams.totalItemsOfUiGrid = vm.dashboardAndReportComInformation.widgetList[index].data[0].ROWCOUNTS;
                            vm.dashboardAndReportComInformation.widgetList[index].data.filter(function (i) {
                                delete i.ROWCOUNTS;
                            });
                        }

                        $timeout(function () { handleUiGridSettingsAndInitialization(compile, scope, vm, uiGridConstants, index); }, 0);
                        $('#' + vm.dashboardAndReportComInformation.widgetList[index].widgetID).find('.searchdiv').attr('style', 'display:none');
                    }
                    else {
                        vm.dashboardAndReportComInformation.widgetList[index].SearchText = '';
                        $('#' + vm.dashboardAndReportComInformation.widgetList[index].widgetID).find('.searchdiv').attr('style', 'display:none');
                        handleGenericTableInitialization(compile, scope, vm, index);

                    }

                });
            }
            else {

                $timeout(function () {
                    //$grid._resizeHandler();
                }, 300);
            }

        };
        //#endregion

        //#region initialization murri for selected column of query builder
        var handleSelectedColumnOfQueryBuilder = function (QueryBuilderUniqueKey, widgetIndex, vm) {

            vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn = new Muuri('.' + QueryBuilderUniqueKey + ' .selected-rows', {
                dragEnabled: true,
                dragReleaseEasing: 'ease-out',
                dragHammerSettings: {
                    touchAction: 'pan-y'
                },
                dragStartPredicate: (item, hammerEvent) => {
                    if (hammerEvent.pointerType === 'mouse') {
                        this.isResolved = true;
                        return true;
                    }
                    else {
                        if (hammerEvent.deltaTime >= 500) {
                            this.isResolved = true;
                            return true;
                        }
                    }

                },
                dragStartPredicate: function (item, event, e) {
                    if ($(event.target).is(".moverow")) {
                        return Muuri.ItemDrag.defaultStartPredicate(item, event);
                    }
                },
                dragSortPredicate: {
                    action: 'swap'
                },
                layout: {
                    fillGaps: true
                }
            });
            //debugger;

        };
        //#endregion

        //#region render selected column check on selected checkbox
        var handleRenderSelectedColumn = function (e, vm, widgetIndex, scope, compile) {
            //debugger
            if (e.target.checked) {
                var uniqueId = $(e.target).attr('id').replace(/ /g, '');
                if ($(e.target).parents('div[querybuilder]').find(".selected-rows .tg-row#" + uniqueId).length < 1) {
                    var newRow = $.parseHTML($QueryBuilderRows[0]['code']);
                    compile($(newRow).find('.clone').attr('ng-click', 'vm.copyselectColumn($event,' + widgetIndex + ')'))(scope);
                    numbercount = numbercount + 1;
                    $(newRow).attr("id", uniqueId);
                    $(newRow).attr("rowid", numbercount);
                    var viewname = $(e.target).siblings('label').text();
                    var viewcolumndatatype = $(e.target).attr('data-columndatatype');
                    LoadAggregateFunctions(viewcolumndatatype, $(newRow));
                    LoadSortFunctions($(newRow));
                    $(newRow).find('.tbl-columnname').attr('value', viewname);
                    $(newRow).find('.db-column-data-type').attr('orginal-data-type', viewcolumndatatype);
                    $(newRow).find('.db-column-data-type').attr('current-data-type', viewcolumndatatype);
                    let getIndexOfunSelectedColumns = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.findIndex(x => x == uniqueId);
                    if (getIndexOfunSelectedColumns < 0)
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.push(uniqueId);
                    // console.log(newRow);
                    $(newRow).find('.tbl-function').attr('onChange', 'ChangeAggregateFunction(this,' + widgetIndex + ')');
                    // newRow = compile(newRow)(scope);

                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn.add(newRow);
                    if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount > 0) {
                        $(newRow).find(".tbl-group").prop("checked", true);
                        $(newRow).find(".tbl-group").attr("disabled", true);
                    }
                }
            } else {
                var uniqueId = $(e.target).attr('id').replace(/ /g, '');
                let getIndexOfunSelectedColumns = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.findIndex(x => x == uniqueId);
                if (getIndexOfunSelectedColumns > -1)
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.splice(getIndexOfunSelectedColumns, 1);
                var findRow = $("." + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + " .selected-rows .tg-row#" + uniqueId);
                let arr = [];
                $.each(findRow, function (idx, ele) {
                    arr.push(ele);
                });
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn.remove(arr, { removeElements: true });
            }

        };
        //#endregion

        //#region load query builder where clause
        var LoadData = function (datat1, scope, compile, rootScope, vm, widgetIndex) {

            $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.querybuilderwhere').empty();
            var queryele = '<query-builder group="filter.group"></query-builder>';
            var queryeleCom = compile(queryele)(scope);
            $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.querybuilderwhere').append(queryeleCom);
            var data = datat1;
            function htmlEntities(str) {
                return str;
            }
            function addremoveqoute(str, flag) {
                if (flag == "System.DateTime") {
                    dt = new Date(str);
                    str = dt.getFullYear() + "/" + (dt.getMonth() + 1) + "/" + dt.getDate();
                }
                if (flag == "System.Integer") {
                    return String(str);
                }
                if (flag == "System.Int32") {
                    return String(str);
                }
                if (flag == "System.Double") {
                    return String(str);
                }
                else {
                    str = "'" + str + "'";
                    return String(str);
                }
            }
            function computed(group) {
                if (!group) return "";
                for (var str = "(", i = 0; i < group.rules.length; i++) {
                    if (group.rules[0].group) {
                        group.rules[0].group.removefirstcondition = false;
                    }
                    if (group.rules.length == 1) {
                        group.rules[i].isoperator = false;

                    }
                    if (i > 0 && group.rules[i].group) {

                        str += group.rules[i].group.operator + " ";

                    }
                    if (group.rules[i].operator) {
                        str += " " + group.rules[i].operator;
                    }

                    if (group.rules[i].type == "System.DateTime") {
                        group.rules[i].data = new Date(group.rules[i].data);
                        group.rules[i].databetween = new Date(group.rules[i].databetween);
                    }

                    if (group.rules[i].condition == 'BETWEEN') {
                        str += group.rules[i].group ?
                            computed(group.rules[i].group) :
                            " [" + group.rules[i].field + "] " + htmlEntities(group.rules[i].condition) + " " + addremoveqoute(group.rules[i].data, group.rules[i].type) + " AND " + addremoveqoute(group.rules[i].databetween, group.rules[i].type) + " ";
                    }
                    else if (group.rules[i].condition == 'IS NULL' || group.rules[i].condition == 'IS NOT NULL') {
                        group.rules[i].data = "";
                        group.rules[i].databetween = "";
                        str += group.rules[i].group ?
                            computed(group.rules[i].group) :
                            " [" + group.rules[i].field + "] " + htmlEntities(group.rules[i].condition) + " ";
                    }
                    else if (group.rules[i].condition == 'LIKE' || group.rules[i].condition == 'NOT LIKE') {
                        str += group.rules[i].group ?
                            computed(group.rules[i].group) :
                            " [" + group.rules[i].field + "] " + htmlEntities(group.rules[i].condition) + " '%" + group.rules[i].data + "%' ";
                    }
                    else {
                        str += group.rules[i].group ?
                            computed(group.rules[i].group) :
                            " [" + group.rules[i].field + "] " + htmlEntities(group.rules[i].condition) + " " + addremoveqoute(group.rules[i].data, group.rules[i].type) + " ";
                    }


                }
                scope.Querystring = str + ")";
                return str + ")";
            }
            scope.json = null;
            scope.filter = data;
            scope.$watch('filter', function (newValue) {
                scope.updateJson = newValue;
                scope.json = JSON.stringify(newValue, null, 2);
                scope.output = computed(newValue.group);
            }, true);
            handleAddColumnsOrRemoveColumnInWhereCluase(vm, widgetIndex, scope, compile, vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName);
            //scope.Report;      
            //rootScope.testing = vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName;
            //scope.getAllReports = function () {
            //    $http.get("/Xcelerate/QueryBuilder/GetReport?Filter=byDefinationType").then(function (response) {

            //        scope.Report = response.data;
            //    });
            //};
        };
        //#endregion

        //#region add or remove columns in where clause for both like multi schema desinger and single schema

        var handleAddColumnsOrRemoveColumnInWhereCluase = function (vm, widgetIndex, scope, compile, columnsList) {
            $timeout(function () { scope.$emit('addColumnInWhereClauseE', columnsList); }, 500);

        };
        //#endregion

        //#region remove columns from selected column list in multi schema designer

        var handleRemoveColumnFromSelectedColumnListInMultiSchemaDesigner = function (vm, widgetIndex, scope, compile, dataset, columnsList) {
            let getLengthOfColumnList = columnsList.length;
            if (getLengthOfColumnList > 0) {
                for (let idx = 0; idx < getLengthOfColumnList; idx++) {
                    let getItem = columnsList[idx];
                    let getIndexSelectedColumnName = vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName.findIndex(x => x.name == dataset + "." + getItem.label);
                    if (getIndexSelectedColumnName > -1) {
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName.splice(getIndexSelectedColumnName, 1);
                    }

                    handleDeleteViewAndItsColumnsNameForSchemaDesigner(vm, widgetIndex, { label: getItem.label, value: dataset }, scope, compile);
                }
                handleAddColumnsOrRemoveColumnInWhereCluase(vm, widgetIndex, scope, compile, vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName);
            }
        };

        //#endregion

        //#region render columns when change dataset of views
        var handleViewColumns = function (scope, compile, vm, widgetIndex, rootScope) {
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].selectColumnTemp = {};
            if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.selectedViewName.length > 0) {
                $.ajax({
                    type: "POST",
                    url: '/BusinessIntelligence/QueryBuilder/GetViewColumns',
                    data: { viewname: vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.selectedViewName },
                    success: function (result) {
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName = [];
                        $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.columns-list').empty();

                        var appendcolumnslst = "";
                        for (var i = 0; i < result.obj.length; i++) {
                            appendcolumnslst += '<li><input type="checkbox" id="' + result.obj[i].ColumnName.replace(/ /g, '') + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + '" name="' + result.obj[i].ColumnName.replace(/ /g, '') + widgetIndex + '" data-columndatatype="' + result.obj[i].ColumnDataType + '" data-columndefinationtype="' + result.obj[i].ColumnDataType + '" ng-model="vm.dashboardAndReportComInformation.widgetList[' + widgetIndex + '].selectColumnTemp.temp' + widgetIndex + 'm' + i + '"  ng-click="vm.LoadSelectedColumn($event,' + widgetIndex + ')"><label for="' + result.obj[i].ColumnName.replace(/ /g, '') + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + '">' + result.obj[i].ColumnName + '</label></li>';
                            var dataType = 'System.String';
                            var DataType = "[" + result.obj[i].ColumnDataType + "]";
                            if (result.obj[i].ColumnDataType == 'select') {
                                dataType = "select";
                            }
                            else if (result.obj[i].ColumnDataType == 'int' || result.obj[i].ColumnDataType == 'numeric') {
                                dataType = "System.Int32";
                            } else if (result.obj[i].ColumnDataType == 'varchar' || result.obj[i].ColumnDataType == 'nvarchar') {
                                dataType = "System.String";
                            }
                            else if (result.obj[i].ColumnDataType == 'float') {
                                dataType = "System.Double";
                            }
                            else if (result.obj[i].ColumnDataType == 'datetime' || result.obj[i].ColumnDataType == 'date') {
                                dataType = "System.DateTime";
                            }

                            vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName.push({ name: result.obj[i].ColumnName, DataType: dataType, DisplayType: result.obj[i].IsSelectType, $$hashKey: "object:" + i });
                        }

                        appendcolumnslst = $.parseHTML(appendcolumnslst);
                        appendcolumnslst = compile(appendcolumnslst)(scope);

                        $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.columns-list').append(appendcolumnslst);
                        LoadData(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.whereClauseJsonWithRuleAndGroup, scope, compile, rootScope, vm, widgetIndex);
                    }
                });
            }
        };
        //#endregion

        //#region get view columns base on dataset or view name

        var handleGetViewColumnsBaseOnDataset = function (NameOfDataset) {

            return $.ajax({
                type: "POST",
                url: '/BusinessIntelligence/QueryBuilder/GetViewColumns',
                data: { viewname: NameOfDataset }
            });
        };

        //#endregion

        //#region get views from database
        var getViewsOfDb = function (vm) {

            $.ajax({
                type: "POST",
                url: '/BusinessIntelligence/QueryBuilder/GetViews',
                success: function (result) {

                    vm.dashboardAndReportComInformation.viewsInformation = result.obj;
                    //$('.' + Querybuilder).find('.querybuilder-datasets').empty();
                    //var appenddrpdwnlst = "<option value='0'>Select Dataset</option>";
                    //for (var i = 0; i < result.obj.length; i++) {

                    //    appenddrpdwnlst += "<option value = '" + result.obj[i].ViewName + "'>" + result.obj[i].ViewName + "</option>";
                    //}
                    //$('.' + Querybuilder).find('.querybuilder-datasets').append(appenddrpdwnlst);
                }
            });
        };
        //#endregion

        //#region delete all row of selected columns
        var handleDeleteAllRowOfSelectedColumn = function (vm, widgetIndex) {

            var findRow = $("." + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + " .selected-rows .tg-row");
            let arr = [];
            $.each(findRow, function (idx, ele) {
                arr.push(ele);

            });
            // $(".columns-list li input[type='checkbox'], .side-panel .select-all-row").attr('checked', false);

            vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn.remove(arr, { removeElements: true });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount = 0;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn._resizeHandler();
            return false;
        };
        //#endregion

        //#region get Unique Value frm list base on columnName
        var handleGetUniqueValueFromListBaseOnColumnName = function (columnName, data, vm, widgetIndex) {
            return data.map(item => item[columnName])
                .filter((value, index, self) => self.indexOf(value) === index && value != null).map(columnData => {
                    let makeDataTreeview = {};
                    makeDataTreeview['selected'] = true;
                    if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[columnData]) {
                        makeDataTreeview['selected'] = false;
                    }
                    makeDataTreeview['label'] = columnData;
                    makeDataTreeview['value'] = columnName;
                    return makeDataTreeview;
                });
        };
        //#endregion 

        //#region fill treeview for charts
        var handleFillTreeviewForCharts = function (vm, widgetIndex, scope, compile) {
            var data = JSON.parse(vm.dashboardAndReportComInformation.widgetList[widgetIndex].tempData.data);
            if (data.length > 0) {

                var getColumnName = data[0];
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].columnValueForFilteration = [];
                for (var key in getColumnName) {
                    let status = true;
                    if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].multiSeriesYaixSelect.length > 0) {
                        let getIndex = vm.dashboardAndReportComInformation.widgetList[widgetIndex].multiSeriesYaixSelect.findIndex(x => x.multiSeriesYaxisColumnsName == key);
                        if (getIndex > -1)
                            status = false;
                    }
                    if (status) {
                        let itemTreeview = {};
                        itemTreeview['label'] = key;
                        itemTreeview['selected'] = true;
                        itemTreeview['value'] = key;
                        itemTreeview['children'] = handleGetUniqueValueFromListBaseOnColumnName(key, data, vm, widgetIndex);
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].columnValueForFilteration.push(itemTreeview);
                    }
                }
                //console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].columnValueForFilteration);
            }
        };
        //#endregion

        //#region  code start
        //#region fill treeview for charts
        var handleFillTreeviewForMap = function (vm, widgetIndex) {
            var data = vm.dashboardAndReportComInformation.widgetList[widgetIndex].data;
            if (data.length > 0) {

                var getColumnName = data[0];
                var historyofCols = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.historyOfQueryBuilder;
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].columnValueForFilteration = [];
                // console.log(getColumnName);
                for (var key in getColumnName) {
                    let status = false;
                    if (key != "Name" && key != "TotalRecords" && key != "Latitude" && key != "Longitude" && key != "RecordValue") {
                        if (historyofCols.length > 0) {
                            let getIndex = historyofCols.findIndex(x => x.columnname == key && (x.currentDataType == "select" || x.currentDataType == "nvarchar"));
                            if (getIndex > -1 || key == "Country" || key == "State" || key == "Region") {
                                status = true;
                            }
                        }

                    }

                    if (status) {
                        let itemTreeview = {};
                        itemTreeview['label'] = key;
                        itemTreeview['selected'] = true;
                        itemTreeview['value'] = key;
                        itemTreeview['children'] = handleGetUniqueValueFromListBaseOnColumnName(key, data, vm, widgetIndex);
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].columnValueForFilteration.push(itemTreeview);
                    }
                }
                //console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].columnValueForFilteration);
            }
        };
        //#endregion
        //#endregion code end

        //#region fill treeview for filteration of All Widgets
        var handleFillTreeviewFilterationOfAllWidgets = function (vm, widgetIndex, scope, compile) {
            switch (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetType) {
                case 'widgetOfChart':
                    handleFillTreeviewForCharts(vm, widgetIndex, scope, compile);
                    break;
                case 'widgetOfMap':
                    // code start
                    handleFillTreeviewForMap(vm, widgetIndex);
                    // code end
                    break;
                default:
            }

        };
        //#endregion 

        //#region get data to given value for chart
        var handleGetDataToGevenValueForCharts = function (vm, widgetIndex, scope, compile) {
            if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].tempData.data) {
                let dataData = $.parseJSON(vm.dashboardAndReportComInformation.widgetList[widgetIndex].tempData.data);
                let FormatSetForChart = {};
                if (Object.keys(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue).length) {
                    let params = "x=>";
                    for (var key in vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue) {
                        params += "x['" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[key]["key"] + "'] != '" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[key]["value"] + "' && ";
                    }
                    params = params.substr(0, params.length - 3);
                    dataData = dataData.filter(eval(params));
                    if (dataData.length == 0)
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].data = [];
                    FormatSetForChart['status'] = true;
                    FormatSetForChart['data'] = JSON.stringify(dataData);
                    handleGetChartAndCompanytype(widgetIndex, vm, FormatSetForChart, toastrs);
                }
                else {
                    handleGetChartAndCompanytype(widgetIndex, vm, vm.dashboardAndReportComInformation.widgetList[widgetIndex].tempData, toastrs);
                }
                hanldeCheckChartCompanyType(compile, scope, vm, widgetIndex);
            }
            // console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue);
        };

        //#endregion

        //#region get data to given value for all widget
        var handleGetDataToGivenValueForAllWidgets = function (vm, widgetIndex, scope, compile) {
            switch (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetType) {
                case 'widgetOfChart':
                    handleGetDataToGevenValueForCharts(vm, widgetIndex, scope, compile);
                    break;
                case 'widgetOfMap':

                    handleGetDataToGevenValueForMap(vm, widgetIndex, scope, compile);
                    initMapDynamic(compile, scope, vm, widgetIndex);
                    break;
                default:
            }
        };
        //#endregion

        //#region start code
        //#region get data to given value for map
        handleGetDataToGevenValueForMap = function (vm, widgetIndex, scope, compile) {
            if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].tempData) {
                let dataData = vm.dashboardAndReportComInformation.widgetList[widgetIndex].tempData;
                let FormatSetForChart = {};
                if (Object.keys(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue).length) {
                    let params = "x=>";
                    for (var key in vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue) {
                        params += "x['" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[key]["key"] + "'] != '" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[key]["value"] + "' && ";
                    }
                    params = params.substr(0, params.length - 3);
                    dataData = dataData.filter(eval(params));
                    if (dataData.length == 0)
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].data = [];
                }
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].data = dataData;

            }
        };
        //#endregion
        //#endregion end code

        //#region delete value of criteria for all widgets
        var handleDeleteValueOfCriteriaForAllWidgets = function (vm, widgetIndex, ivhNode) {
            if (typeof ivhNode.children != "undefined")
                hanldeDeleteAllValueOfColumnGivenCriteria(vm, widgetIndex, ivhNode);
            else
                delete vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[ivhNode['label']];
        };
        //#endregion

        //#region put value of criteria for all widgets
        var hanldePutValueOfCriteriaForAllWidgets = function (vm, widgetIndex, ivhNode) {
            if (typeof ivhNode.children != "undefined")
                hanldePutAllValueOfColumnGivenCriteria(vm, widgetIndex, ivhNode);
            else {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[ivhNode['label']] = {};
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[ivhNode['label']]['key'] = ivhNode['value'];
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[ivhNode['label']]['value'] = ivhNode['label'];
            }
        };
        //#endregion

        //#region delete all value of the column from given criteria
        var hanldeDeleteAllValueOfColumnGivenCriteria = function (vm, widgetIndex, ivhNode) {
            if (ivhNode.children.length > 0) {
                for (let i = 0; i < ivhNode.children.length; i++) {

                    delete vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[ivhNode.children[i]['label']];
                }
            }

        };
        //#endregion

        //#region put all value of the column from given criteria
        var hanldePutAllValueOfColumnGivenCriteria = function (vm, widgetIndex, ivhNode) {
            if (ivhNode.children.length > 0) {
                for (let i = 0; i < ivhNode.children.length; i++) {

                    delete vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[ivhNode.children[i]['label']];
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[ivhNode.children[i]['label']] = {};
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[ivhNode.children[i]['label']]['key'] = ivhNode.children[i]['value'];
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.FilterOnKeyValue[ivhNode.children[i]['label']]['value'] = ivhNode.children[i]['label'];
                }
            }
        };
        //#endregion

        //#region handle ajax call chart
        var handleChartFromDb = function (compile, scope, vm, uiGridConstants, WigetIndex, toastrStatus) {
            let GetQuery = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sqlQueryBuild;

            if (GetQuery.length > 0) {
                $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.overlay').attr('style', 'display:block;');
                handleGetDataFromDbForCharts(WigetIndex, GetQuery, vm, toastrStatus, scope, compile).done(function (res) {
                    if (res.status) {
                        if (toastrStatus) {
                            toastrs.success("Query has been executed");
                        }
                        //debugger
                        handleFillTreeviewFilterationOfAllWidgets(vm, WigetIndex, scope, compile);

                        hanldeCheckChartCompanyType(compile, scope, vm, WigetIndex);

                    }
                    else {
                        toastrs.error("Data has not been found");
                    }
                    $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.overlay').attr('style', 'display:none;');
                });
            }
            else {
                // $grid._resizeHandler();
            }
        };
        //#endregion

        //#region check widget type
        var handleCheckWidgetType = function (compile, scope, vm, uiGridConstants, WigetIndex, toastrs) {
            switch (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType) {
                case 'widgetOfTable':
                    handleCheckRenderTableOrUiGrid(compile, scope, vm, uiGridConstants, WigetIndex, true);
                    break;
                case 'widgetOfChart':
                    handleChartFromDb(compile, scope, vm, uiGridConstants, WigetIndex, true);
                    break;
                case 'widgetOfMap':
                    handleMaps(compile, scope, vm, WigetIndex, true);
                    break;
                case 'widgetOfDefault':
                    handleLoadColumnsInSettingsOfDefaultPage(vm, WigetIndex, scope, compile);
                    hanldeDefaultPage(compile, scope, vm, WigetIndex, true);
                    break;
                default:
            }

        };
        //#endregion

        //#region make query for pagination before render grid
        var handleMakeQueryBeforeRenderGrid = function (queryBuild, pageSize, pageNumber, orderBy) {

            let makeQueryForUiGrid = "WITH selectTable AS (putTable) SELECT (SELECT COUNT(*)  FROM selectTable) AS ROWCOUNTS,* FROM selectTable orderby OFFSET @PageSize * (@PageNumber-1) ROWS  FETCH NEXT @PageSize  ROWS ONLY";
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/putTable/g, queryBuild);
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/@PageSize/g, pageSize);
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/@PageNumber/g, pageNumber);
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/orderby/g, orderBy);
            return makeQueryForUiGrid;

        };
        //#endregion

        //#region make query for pagination after render grid
        var handleMakeQueryAfterRenderGrid = function (queryBuild, pageSize, pageNumber, orderBy) {
            let makeQueryForUiGrid = "putTable orderby OFFSET @PageSize * (@PageNumber-1) ROWS  FETCH NEXT @PageSize  ROWS ONLY";
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/putTable/g, queryBuild);
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/@PageSize/g, pageSize);
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/@PageNumber/g, pageNumber);
            makeQueryForUiGrid = makeQueryForUiGrid.replace(/orderby/g, orderBy);
            return makeQueryForUiGrid;
        };
        //#endregion

        //#region maintain history of query builder
        var handleHistoryOfQueryBuilder = function (vm, wigetIndex, historyOfQueryBuilder) {
            vm.dashboardAndReportComInformation.widgetList[wigetIndex].widgetConfiguration.historyOfQueryBuilder = historyOfQueryBuilder;
            //  console.log(vm.dashboardAndReportComInformation.widgetList[wigetIndex].widgetConfiguration.historyOfQueryBuilder);
        };
        //#endregion

        //#region Assign value to x,y and series axis
        var handleAssignXaxisYaxisSeries = function (vm, WigetIndex) {
            vm.dashboardAndReportComInformation.widgetList[WigetIndex].multiSeriesXaixSelect = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.historyOfQueryBuilder.map(xasixColumn => {
                let xaxisColumnObj = {};
                xaxisColumnObj['multiSeriesXaxisColumnsName'] = xasixColumn.columnaliasname;
                return xaxisColumnObj;
            });
            //console.log(vm.dashboardAndReportComInformation.widgetList[WigetIndex].multiSeriesXaixSelect);
            vm.dashboardAndReportComInformation.widgetList[WigetIndex].multiSeriesYaixSelect = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.historyOfQueryBuilder.filter(x => (x.currentDataType == "numeric" || x.currentDataType == "float" || x.currentDataType == "real" || x.currentDataType == "int")).map(yasixColumn => {
                let yaxisColumnObj = {};
                yaxisColumnObj['multiSeriesYaxisColumnsName'] = yasixColumn.columnaliasname;
                return yaxisColumnObj;
            });


        };
        //#endregion

        //#region set default value for multi series
        var handleSetDefaultValueForMultiSeries = function (vm, widgetIndex) {
            handleResetXaxisMul(vm, widgetIndex);
            handleResetYaxisMul(vm, widgetIndex);
            handleResetMultSeriesGroup(vm, widgetIndex);

        };
        //#endregion 

        //#region load columns in setting of default page when columns are selected in query builder
        var handleLoadColumnsInSettingsOfDefaultPage = function (vm, widgetIndex, scope, compile) {
            let getQueryBuilderHistory = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.historyOfQueryBuilder;
            if (getQueryBuilderHistory.length > 0) {
                $("." + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".content .add-db-columns").empty();
                for (let i = 0; i < getQueryBuilderHistory.length; i++) {
                    let ClumnName = (getQueryBuilderHistory[i].columnaliasname).replace(/ /g, '');
                    let uiColumnInDefaultPage = $.parseHTML(getUiColumnInSettingOfDefaultPage);
                    $(uiColumnInDefaultPage).find('.draggable_element').text(getQueryBuilderHistory[i].columnaliasname);
                    $(uiColumnInDefaultPage).find('.draggable_element').attr('data-member-name', ClumnName);
                    $(uiColumnInDefaultPage).attr('data-type', 'dbcolumn');
                    compile($("." + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + '.default_settings').find(".content .add-db-columns").append(uiColumnInDefaultPage))(scope);
                }
                makeDragColumnsOfDefaultPage("." + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + " .content .add-db-columns .draggable_element", vm, widgetIndex, scope, compile);
            }
        };
        //#endregion

        //#region assign value to default page parameters

        var handleAssignValueOfDefaultPage = function (vm, widgetIndex, data) {
            for (var key in data) {
                let ClumnName = (key).replace(/ /g, '');
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.dataParams[ClumnName] = data[key];
            }

        };

        //#endregion

        //#region get empty x and y axis and fill default value in that not used
        var handleEmptyXAxisYAxisFillDefaultValue = function (vm, WigetIndex) {
            $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.x-axis-select').empty();
            $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.y-axis-select').empty();
            $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.series-select').empty();
            $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.x-axis-select').append($('<option>', { value: -1, text: 'Select X-Axis' }));
            $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.y-axis-select').append($('<option>', { value: -1, text: 'Select Y-Axis' }));
            $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.series-select').append($('<option>', { value: -1, text: 'Select Series' }));
        };
        //#endregion

        //#region fill select of x-axis and y-axis for chart not used
        var handleFillSelectOfXAxisYAxis = function (vm, WigetIndex, valueF, textF, advanceSettings, dataType) {
            $(advanceSettings).find('.x-axis-select').append($('<option>', { value: valueF, text: textF }));
            $(advanceSettings).find('.series-select').append($('<option>', { value: valueF, text: textF }));
            if (dataType == "numeric" || dataType == "float" || dataType == "real" || dataType == "int") {
                $(advanceSettings).find('.y-axis-select').append($('<option>', { value: valueF, text: textF }));
            }
        };
        //#endregion

        //#region refill select for X-axis and Y-axis not used
        var handleRefillXaxisYaxis = function (vm, widgetIndex, scope, compile, advanceSettings) {
            let getQueryBuilderHistory = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.historyOfQueryBuilder;
            if (getQueryBuilderHistory.length > 0) {

                for (let i = 0; i < getQueryBuilderHistory.length; i++) {
                    let ClumnName = getQueryBuilderHistory[i].columnaliasname;
                    handleFillSelectOfXAxisYAxis(vm, widgetIndex, ClumnName, ClumnName, advanceSettings, getQueryBuilderHistory[i].currentDataType);
                }
            }
        };
        //#endregion

        //#region get random color with hexa decimile
        var getRandomColor = function () {
            var letters = '0123456789ABCDEF';
            var color = '#';
            for (var i = 0; i < 6; i++) {
                color += letters[Math.floor(Math.random() * 16)];
            }
            return color;
        };

        //#endregion

        //#region Get file name and extension
        var handleGetFileNameAndExtension = function (fileNameWithExtension) {
            let fileInfo = {};
            fileInfo['fileExtension'] = fileNameWithExtension.substring(fileNameWithExtension.lastIndexOf('.') + 1, fileNameWithExtension.length) || fileNameWithExtension;
            fileInfo['fileName'] = fileNameWithExtension.substring(0, fileNameWithExtension.lastIndexOf('.')) || fileNameWithExtension;
            return fileInfo;
        };
        //#endregion 

        //#region convert file into base64
        var handleFileIntoBase64 = function (file, widgetIndex, imageIndex, compile, scope, vm) {

            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function () {


                vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration[imageIndex].imageUrl = reader.result;
                //console.log(reader.result);
            };
            reader.onerror = function (error) {
              //  console.log('Error: ', error);
            };

        };
        //#endregion

        //#region get image 
        var handleGetImageOfPage = function (fileSec, widgetIndex, imageIndex, compile, scope, vm) {
            if (typeof $(fileSec)[0].files[0] != undefined) {
                handleFileIntoBase64($(fileSec)[0].files[0], widgetIndex, imageIndex, compile, scope, vm);
                if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].fileInformation.length > 0) {
                    var findFile = vm.dashboardAndReportComInformation.widgetList[widgetIndex].fileInformation.findIndex(x => x.widgetIndex == widgetIndex && x.ImageIndex == imageIndex);
                    if (findFile > -1)
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].fileInformation.splice(findFile, 1);

                }
                $timeout(function () {
                    let fileInfo = {};

                    let fileNameExtension = handleGetFileNameAndExtension($(fileSec)[0].files[0].name);
                    fileInfo['widgetIndex'] = widgetIndex;
                    fileInfo['ImageIndex'] = imageIndex;
                    fileInfo['fileName'] = fileNameExtension.fileName;
                    fileInfo['fileExtension'] = fileNameExtension.fileExtension;
                    fileInfo['filePath'] = '/images/';
                    fileInfo['save'] = false;
                    fileInfo['fileBase64'] = vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration[imageIndex].imageUrl.split(',')[1]; vm.dashboardAndReportComInformation.widgetList[widgetIndex].defaultPageConfiguration.customImageControlPropertiesConfiguration[imageIndex].imageUrl;
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].fileInformation.push(fileInfo);


                    scope.$apply();

                }, 30);

            }

        };
        //#endregion

        //#region Execute query for single dataset

        var handleExecuteQueryForSingleDataset = function (vm, WigetIndex, compile, scope, uiGridConstants, toastrs) {
            var columnarr = [];
            var querycolumns = '';
            var querygroupby = '';
            var querysortyby = '';
            vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.whereClauseJsonWithRuleAndGroup = scope.updateJson;
            vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.whereClause = scope.output;
            let getAllItems = vm.dashboardAndReportComInformation.widgetList[WigetIndex].murriInitializationOfSelectedColumn.getItems();
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfDefault") {
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].defaultPageConfiguration.dataParams = {};
            }
            else if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfChart") {

                //
            }
            $(getAllItems).each(function (indx, value) {
                var columnAliasname = '';

                var columnName = $(this._element).find(".tbl-columnname").val();

                var aliasName = $(this._element).find(".tbl-aliasname").val();
                var currentDataType = $(this._element).find(".db-column-data-type").attr('current-data-type');
                var originalDataType = $(this._element).find(".db-column-data-type").attr('orginal-data-type');
                if (aliasName == "") {
                    columnAliasname = columnName;

                }
                else {
                    columnAliasname = aliasName;
                }
                //if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfChart") {
                //    handleFillSelectOfXAxisYAxis(vm, WigetIndex, columnAliasname, columnAliasname, "." + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID, currentDataType);
                //}
                var aggregateFunction = $(this._element).find(".tbl-function").val();
                var groupBy = $(this._element).find(".tbl-group").prop("checked");

                var sortBy = $(this._element).find(".tbl-sort").val();

                columnarr.push({
                    columnname: columnName,
                    aliasname: aliasName,
                    columnaliasname: columnAliasname,
                    aggregatefunction: aggregateFunction,
                    groupby: groupBy,
                    sortby: sortBy,
                    currentDataType: currentDataType,
                    originalDataType: originalDataType

                });
            });


            for (var i = 0; i < columnarr.length; i++) {
                if (columnarr[i]["aggregatefunction"] != "-1") {
                    if (columnarr[i]["aggregatefunction"] == "week") {
                        querycolumns += ' DatePart(' + columnarr[i]["aggregatefunction"] + ',[' + columnarr[i]["columnname"] + ']) AS [' + columnarr[i]["columnaliasname"] + '],';
                    }
                    else {
                        querycolumns += ' ' + columnarr[i]["aggregatefunction"] + '([' + columnarr[i]["columnname"] + ']) AS [' + columnarr[i]["columnaliasname"] + '],';
                    }
                }
                else {
                    querycolumns += ' ([' + columnarr[i]["columnname"] + ']) AS [' + columnarr[i]["columnaliasname"] + '],';
                }
                if (columnarr[i]["groupby"] == true) {
                    querygroupby += ' [' + columnarr[i]["columnname"] + '],';
                }

                if (columnarr[i]["sortby"] != '-1') {
                    querysortyby += ' [' + columnarr[i]["columnname"] + '] ' + columnarr[i]["sortby"] + ',';
                }

            }

            var dataset = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.selectedViewName;
            querycolumns = querycolumns.substring(0, querycolumns.length - 1);
            var finalquery = 'Select' + querycolumns + ' from ' + dataset;
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.whereClause != '' && vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.whereClause != '()') {
                finalquery += ' WHERE ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.whereClause;

            }
            if (querygroupby != '') {
                querygroupby = querygroupby.substring(0, querygroupby.length - 1);
                finalquery += ' GROUP BY ' + querygroupby;
            }


            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfTable") {
                if (querysortyby != '') {
                    querysortyby = querysortyby.substring(0, querysortyby.length - 1);
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.orderBy = ' ORDER BY ' + querysortyby;
                }
                else {
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.orderBy = " ORDER BY(SELECT NULL)";
                }
            }
            else {
                if (querysortyby != '') {
                    querysortyby = querysortyby.substring(0, querysortyby.length - 1);
                    finalquery += ' ORDER BY ' + querysortyby;
                }
            }
            //#region map
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfMap") {


                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryColumns = querycolumns;
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.whereClause;
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryGroupBy = querygroupby;
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQuerySortBy = querysortyby;

            }
            //#endregion               
            handleHistoryOfQueryBuilder(vm, WigetIndex, columnarr);
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfChart") {

                handleAssignXaxisYaxisSeries(vm, WigetIndex);
            }
            vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sqlQueryBuild = finalquery;
            handleCheckWidgetType(compile, scope, vm, uiGridConstants, WigetIndex, toastrs);

        };

        //#endregion

        //#region Schema Designer Section

        //#region select tables name when we don't make one or more than joins

        var handleSelectTablesNameWhenDoNotSelectAtleastOneJoin = function (tableList) {
            let getTablesNameLength = tableList.length;
            let selectTablesName = '';
            if (getTablesNameLength > 0) {
                for (let idx = 0; idx < getTablesNameLength;) {

                    selectTablesName += tableList[idx] + " AS " + tableList[idx];
                    idx++;
                    if (idx != getTablesNameLength)
                        selectTablesName += " , ";
                }
            }
            return selectTablesName;
        };

        //#endregion

        //#regin render only subquery column with main selected column


        var handleRenderOnlySubqueryColumnWithMainSelectedColumn = function (vm, widgetIndex, scope, compile) {

            for (var key in vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList) {

                handleApplySubquery(vm, widgetIndex, 'viewName', key, "subQuery", scope, compile);


            }
        };


        //#endregion


        //#region Render Selected columns by json ,when use main query or subquery

        var handleRenderSelectedColumnUsingJson = function (vm, widgetIndex, compile, scope, historyOfSelectedColumn) {

            if (historyOfSelectedColumn.length > 0) {
                for (let i = 0; i < historyOfSelectedColumn.length; i++) {

                    handleRenderSelectedColumnWithMultiSchemaDesignerUsingJson(vm, widgetIndex, historyOfSelectedColumn[i].viewName, historyOfSelectedColumn[i].columnNameWithoutViewName, historyOfSelectedColumn[i].currentDataType, scope, compile, historyOfSelectedColumn[i].aggregatefunction, historyOfSelectedColumn[i].groupby, historyOfSelectedColumn[i].sortby);
                }
            }


        }

        //#endregion

        //#region Execute query for multi dataset
        var handleExecuteQueryForMultiDataset = function (vm, WigetIndex, compile, scope, uiGridConstants, toastrs, queryExeStatus) {
            let columsNameList = [];
            vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.joinSectoin = handleMakeJoinFromJoinDetailsObject(vm, WigetIndex, scope, compile);
            var columnarr = [];
            var querycolumns = '';
            var querygroupby = '';
            var querysortyby = '';
            vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.whereClauseJsonWithRuleAndGroup = scope.updateJson;
            vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.whereClause = scope.output;
            let getAllItems = vm.dashboardAndReportComInformation.widgetList[WigetIndex].murriInitializationOfSelectedColumn.getItems();
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfDefault") {
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].defaultPageConfiguration.dataParams = {};
            }
            else if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfChart") {

                //
            }
            $(getAllItems).each(function (indx, value) {
                var columnAliasname = '';
                var duplicate = false;
                var columnName = $(this._element).find(".tbl-columnname").val();

                var aliasName = $(this._element).find(".tbl-aliasname").val();
                var currentDataType = $(this._element).find(".db-column-data-type").attr('current-data-type');
                var originalDataType = $(this._element).find(".db-column-data-type").attr('orginal-data-type');
                var uniqueKeyOfSubqeury = $(this._element).find(".db-column-data-type").attr('unique-key-subquery');
                if (aliasName == "") {
                    columnAliasname = columnName;

                }
                else {
                    columnAliasname = aliasName;
                }
                //if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfChart") {
                //    handleFillSelectOfXAxisYAxis(vm, WigetIndex, columnAliasname, columnAliasname, "." + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID, currentDataType);
                //}
                var aggregateFunction = $(this._element).find(".tbl-function").val();
                var groupBy = $(this._element).find(".tbl-group").prop("checked");

                var sortBy = $(this._element).find(".tbl-sort").val();
                let columnSplitWithDot = columnName.split('.');
                let viewName = columnSplitWithDot[0];
                let columnNameWithoutViewName = columnSplitWithDot[1];
                columnName = columnSplitWithDot[0] + ".[" + columnSplitWithDot[1] + "]";
                if (columnSplitWithDot[1] == "Latitude" || columnSplitWithDot[1] == "Longitude") {
                    columnAliasname = columnSplitWithDot[1];
                }
                if (columnAliasname.indexOf('.') > -1) {
                    let columnAliasSplitBaseOnDot = columnAliasname.split('.');
                    //columnAliasname = columnAliasSplitBaseOnDot[0] + "_" + columnAliasSplitBaseOnDot[1];
                    columnAliasname = columnAliasSplitBaseOnDot[1];

                }
                if (columsNameList.findIndex(x=>x == columnAliasname) > -1) {
                    duplicate = true;
                    if (aggregateFunction != "-1") {
                        duplicate = false;
                        let getColumnsDup = columnarr.filter(x=>x.columnaliasname == columnAliasname && x.duplicate == false);
                        if (getColumnsDup) {

                            for (let idx = 0; idx < getColumnsDup.length; idx++) {
                                getColumnsDup[idx].duplicate = true;

                            }
                        }
                    }
                }
                columsNameList.push(columnAliasname);
                if (originalDataType != "subQuery") {
                    columnarr.push({
                        columnname: columnName,
                        aliasname: aliasName,
                        columnaliasname: columnAliasname,
                        aggregatefunction: aggregateFunction,
                        groupby: groupBy,
                        sortby: sortBy,
                        currentDataType: currentDataType,
                        originalDataType: originalDataType,
                        viewName: viewName,
                        columnNameWithoutViewName: columnNameWithoutViewName,
                        duplicate: duplicate


                    });
                } else {
                    querycolumns += '(' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueKeyOfSubqeury].sqlQueryBuild + ') AS ' + columnAliasname + ', ';

                }
            });


            for (var i = 0; i < columnarr.length; i++) {
                if (columnarr[i]["duplicate"] == false) {
                    if (columnarr[i]["aggregatefunction"] != "-1") {
                        if (columnarr[i]["aggregatefunction"] == "week") {
                            querycolumns += 'DatePart(' + columnarr[i]["aggregatefunction"] + ',' + columnarr[i]["columnname"] + ') AS [' + columnarr[i]["columnaliasname"] + '],';
                        }
                        else {
                            querycolumns += ' ' + columnarr[i]["aggregatefunction"] + '(' + columnarr[i]["columnname"] + ') AS [' + columnarr[i]["columnaliasname"] + '],';
                        }
                    }
                    else {
                        querycolumns += ' (' + columnarr[i]["columnname"] + ') AS [' + columnarr[i]["columnaliasname"] + '],';
                    }
                }
                if (columnarr[i]["groupby"] == true) {
                    querygroupby += ' ' + columnarr[i]["columnname"] + ',';
                }

                if (columnarr[i]["sortby"] != '-1') {
                    querysortyby += ' ' + '[' + columnarr[i]["columnaliasname"] + ']' + ' ' + columnarr[i]["sortby"] + ',';
                }

            }

            //  var dataset = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.selectedViewName;
            querycolumns = querycolumns.substring(0, querycolumns.length - 1);
            var finalquery = '';
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.joinSectoin != "") {
                finalquery = 'Select' + querycolumns + ' from ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.joinSectoin;
            }
            else {
                finalquery = 'Select' + querycolumns + ' from ' + handleSelectTablesNameWhenDoNotSelectAtleastOneJoin(vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews);

            }

            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.whereClause != '' && vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.whereClause != '()') {
                finalquery += ' WHERE ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.whereClause;

            }
            if (querygroupby != '') {
                querygroupby = querygroupby.substring(0, querygroupby.length - 1);
                finalquery += ' GROUP BY ' + querygroupby;
            }


            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfTable") {
                if (querysortyby != '') {
                    querysortyby = querysortyby.substring(0, querysortyby.length - 1);
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.orderBy = ' ORDER BY ' + querysortyby;
                }
                else {
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationParams.orderBy = " ORDER BY(SELECT NULL)";
                }
            }
            else {
                if (querysortyby != '') {
                    querysortyby = querysortyby.substring(0, querysortyby.length - 1);
                    finalquery += ' ORDER BY ' + querysortyby;
                }
            }
            //#region map
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfMap") {


                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryColumns = querycolumns;
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.whereClause;
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryGroupBy = querygroupby;
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQuerySortBy = querysortyby;

            }
            //#endregion               
            handleHistoryOfQueryBuilder(vm, WigetIndex, columnarr);
            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfChart") {

                handleAssignXaxisYaxisSeries(vm, WigetIndex);
            }
            vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sqlQueryBuild = finalquery;
            vm.dashboardAndReportComInformation.widgetList[WigetIndex].usedDatasetsInEtl = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews.join(',');
            vm.dashboardAndReportComInformation.widgetList[WigetIndex].datasetName = vm.dashboardAndReportComInformation.templateTitle; vm.dashboardAndReportComInformation.scheduler['Query'] = handleMakeEtlQueryBaseOnCronJob(vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sqlQueryBuild, vm.dashboardAndReportComInformation.widgetList[WigetIndex].datasetName);
            //console.log(vm.dashboardAndReportComInformation.scheduler['Query']);



            if (queryExeStatus)
                handleCheckWidgetType(compile, scope, vm, uiGridConstants, WigetIndex, toastrs);

        };

        //#endregion

        //#region make Elt Query base on cron job

        var handleMakeEtlQueryBaseOnCronJob = function (finalQuery, EtlName) {

            return "SELECT * INTO " + EtlName + " FROM" + " (" + finalQuery + ") AS temp ";


        };

        //#endregion



        //#region add or  render selected column using json
        var handleRenderSelectedColumnWithMultiSchemaDesignerUsingJson = function (vm, widgetIndex, viewName, columnName, columnDataType, scope, compile, aggriFunc, grp, ordrby) {
            //debugger

            var uniqueId = viewName.replace(/ /g, '') + columnName.replace(/ /g, '') + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID;
            if ($('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".selected-rows .tg-row#" + uniqueId).length < 1) {
                var newRow = $.parseHTML($QueryBuilderRows[0]['code']);
                compile($(newRow).find('.clone').attr('ng-click', 'vm.copyselectColumn($event,' + widgetIndex + ')'))(scope);
                numbercount = numbercount + 1;
                $(newRow).attr("id", uniqueId);
                $(newRow).attr("rowid", numbercount);

                var viewcolumndatatype = columnDataType;
                LoadAggregateFunctions(viewcolumndatatype, $(newRow));
                LoadSortFunctions($(newRow));
                $(newRow).find('.tbl-columnname').attr('value', viewName + '.' + columnName);
                $(newRow).find('.db-column-data-type').attr('orginal-data-type', viewcolumndatatype);
                $(newRow).find('.db-column-data-type').attr('current-data-type', viewcolumndatatype);
                //let getIndexOfunSelectedColumns = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.findIndex(x => x == uniqueId);
                //if (getIndexOfunSelectedColumns < 0)
                //    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.push(uniqueId);
                // console.log(newRow);
                $(newRow).find('.tbl-function').attr('onChange', 'ChangeAggregateFunction(this,' + widgetIndex + ')');
                $(newRow).find('.tbl-function').val(aggriFunc);
                $(newRow).find('.tbl-sort').val(ordrby);
                $(newRow).find('.tbl-group').attr('checked', grp)
                // newRow = compile(newRow)(scope);
                if ($.trim(viewcolumndatatype) != "subQuery") {
                    $(newRow).find('.code').remove();
                    $(newRow).find('.trash').remove();
                }
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn.add(newRow);
                if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount > 0) {
                    $(newRow).find(".tbl-group").prop("checked", true);
                    $(newRow).find(".tbl-group").attr("disabled", true);
                }
            }


        };
        //#endregion


        //#region add or  render selected column check on selected checkbox with multi schema designer
        var handleRenderSelectedColumnWithMultiSchemaDesigner = function (vm, widgetIndex, viewName, columnName, columnDataType, scope, compile) {
            //debugger

            var uniqueId = viewName.replace(/ /g, '') + columnName.replace(/ /g, '') + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID;
            // var uniqueId = columnName.replace(/ /g, '') + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID;
            if ($('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".selected-rows .tg-row#" + uniqueId).length < 1) {
                var newRow = $.parseHTML($QueryBuilderRows[0]['code']);
                compile($(newRow).find('.clone').attr('ng-click', 'vm.copyselectColumn($event,' + widgetIndex + ')'))(scope);
                numbercount = numbercount + 1;
                $(newRow).attr("id", uniqueId);
                $(newRow).attr("rowid", numbercount);

                var viewcolumndatatype = columnDataType;
                LoadAggregateFunctions(viewcolumndatatype, $(newRow));
                LoadSortFunctions($(newRow));
                $(newRow).find('.tbl-columnname').attr('value', viewName + '.' + columnName);
                $(newRow).find('.db-column-data-type').attr('orginal-data-type', viewcolumndatatype);
                $(newRow).find('.db-column-data-type').attr('current-data-type', viewcolumndatatype);
                //let getIndexOfunSelectedColumns = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.findIndex(x => x == uniqueId);
                //if (getIndexOfunSelectedColumns < 0)
                //    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.push(uniqueId);
                // console.log(newRow);
                $(newRow).find('.tbl-function').attr('onChange', 'ChangeAggregateFunction(this,' + widgetIndex + ')');
                // newRow = compile(newRow)(scope);
                if ($.trim(viewcolumndatatype) != "subQuery") {
                    $(newRow).find('.code').remove();
                    $(newRow).find('.trash').remove();
                }
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn.add(newRow);
                if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount > 0) {
                    $(newRow).find(".tbl-group").prop("checked", true);
                    $(newRow).find(".tbl-group").attr("disabled", true);
                }
            }


        };
        //#endregion

        //#region delete selected column check on selected checkbox with multi schema designer
        var handleDeleteSelectedColumnWithMultiSchemaDesigner = function (vm, widgetIndex, viewName, columnName, columnDataType, scope, compile) {
            var uniqueId = viewName.replace(/ /g, '') + columnName.replace(/ /g, '') + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID;
            //var uniqueId =columnName.replace(/ /g, '') + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID;
            var findRow = $("." + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + " .selected-rows .tg-row#" + uniqueId);
            let arr = [];
            $.each(findRow, function (idx, ele) {
                arr.push(ele);
            });
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn.remove(arr, { removeElements: true });
        };
        //#endregion

        //#region render treeview for multi schema designer for parent and child
        var handleTreeviewForSchemasDesignerForParentChild = function (vm, widgetIndex, scope, compile, datasetName, columnNameList, drawTableOnCanvasStatuas) {
            if (drawTableOnCanvasStatuas)
                handleDrawTableForSchemaDesigner(vm, widgetIndex, datasetName, vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID);
            let itemTreeview = {};
            itemTreeview['label'] = datasetName;
            itemTreeview['id'] = datasetName;
            itemTreeview['selected'] = true;
            itemTreeview['value'] = datasetName;
            itemTreeview['children'] = handleTreeviewForSchemasDesignerForChildOnly(vm, widgetIndex, scope, compile, columnNameList, datasetName);
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].treeviewOfMultiDatasetsWithItsColumns.push(itemTreeview);
            handleAddColumnsOrRemoveColumnInWhereCluase(vm, widgetIndex, scope, compile, vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName);
            scope.$apply();
        };
        //#endregion

        //#region render treeview for multi schema designer for child only
        var handleTreeviewForSchemasDesignerForChildOnly = function (vm, widgetIndex, scope, compile, columnNameList, datasetName) {
            if (columnNameList.length > 0) {
                let size = 0;
                let makeTreeViewList = [];
                for (let idx = 0; idx < columnNameList.length; idx++) {
                    let columnItem = columnNameList[idx];
                    let makeDataTreeview = {};
                    makeDataTreeview['selected'] = false;
                    let getColumnIndex = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue.findIndex(x => x.key == datasetName && x.value['label'] == columnItem['ColumnName']);
                    if (getColumnIndex > -1) {
                        makeDataTreeview['selected'] = true;
                        size++;
                        handleRedrawColumnInTableForSchemaDesigner(vm, widgetIndex, datasetName, columnItem['ColumnName'], vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID, size);
                    }
                    makeDataTreeview['label'] = columnItem['ColumnName'];
                    makeDataTreeview['id'] = handleGetIdOfColumnInTreeviewForSchemaDesginer(datasetName, columnItem['ColumnName']);
                    makeDataTreeview['value'] = datasetName;
                    makeDataTreeview['dataType'] = columnItem['ColumnDataType'];
                    makeTreeViewList.push(makeDataTreeview);
                    let dataType = handleGetcolumnTypeWhenRenderInBelowDatdaset(columnItem.ColumnDataType);
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName.push({ name: datasetName + "." + columnItem.ColumnName, DataType: dataType, DisplayType: columnItem.IsSelectType });
                }

                return makeTreeViewList;
            }
        };
        //#endregion




        //#region get column type when render in blow the dataset

        var handleGetcolumnTypeWhenRenderInBelowDatdaset = function (dataTypeInDatabase) {
            var dataType = 'System.String';
            if (dataTypeInDatabase == 'select') {
                dataType = "select";
            }
            else if (dataTypeInDatabase == 'int' || dataTypeInDatabase == 'numeric') {
                dataType = "System.Int32";
            } else if (dataTypeInDatabase == 'varchar' || dataTypeInDatabase == 'nvarchar') {
                dataType = "System.String";
            }
            else if (dataTypeInDatabase == 'float') {
                dataType = "System.Double";
            }
            else if (dataTypeInDatabase == 'datetime' || dataTypeInDatabase == 'date') {
                dataType = "System.DateTime";
            }
            return dataType;
        };

        //#endregion 

        //#region Initialization of Where clause when select a dataset
        var handleInitializationOfWhereClauseWhenSelectaDataSet = function (datat1, scope, compile, rootScope, vm, widgetIndex) {

            $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.querybuilderwhere').empty();
            var queryele = '<query-builder group="filter.group"></query-builder>';
            var queryeleCom = compile(queryele)(scope);
            $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.querybuilderwhere').append(queryeleCom);
            var data = datat1;
            function htmlEntities(str) {
                return str;
            }
            function addremoveqoute(str, flag) {
                if (flag == "System.DateTime") {
                    dt = new Date(str);
                    str = dt.getFullYear() + "/" + (dt.getMonth() + 1) + "/" + dt.getDate();
                }
                if (flag == "System.Integer") {
                    return String(str);
                }
                if (flag == "System.Int32") {
                    return String(str);
                }
                if (flag == "System.Double") {
                    return String(str);
                }
                else {
                    str = "'" + str + "'";
                    return String(str);
                }
            }
            function computed(group) {
                if (!group) return "";
                for (var str = "(", i = 0; i < group.rules.length; i++) {
                    if (group.rules[0].group) {
                        group.rules[0].group.removefirstcondition = false;
                    }
                    if (group.rules.length == 1) {
                        group.rules[i].isoperator = false;

                    }
                    if (i > 0 && group.rules[i].group) {

                        str += group.rules[i].group.operator + " ";

                    }
                    if (group.rules[i].operator) {
                        str += " " + group.rules[i].operator;
                    }

                    if (group.rules[i].type == "System.DateTime") {
                        group.rules[i].data = new Date(group.rules[i].data);
                        group.rules[i].databetween = new Date(group.rules[i].databetween);
                    }

                    if (group.rules[i].condition == 'BETWEEN') {
                        let splitBaseOnDotOperator = group.rules[i].field.split('.');
                        let fieldFormat = splitBaseOnDotOperator[0] + ".[" + splitBaseOnDotOperator[1] + "]"
                        str += group.rules[i].group ?
                            computed(group.rules[i].group) :
                            " " + fieldFormat + " " + htmlEntities(group.rules[i].condition) + " " + addremoveqoute(group.rules[i].data, group.rules[i].type) + " AND " + addremoveqoute(group.rules[i].databetween, group.rules[i].type) + " ";
                    }
                    else if (group.rules[i].condition == 'IS NULL' || group.rules[i].condition == 'IS NOT NULL') {
                        let splitBaseOnDotOperator = group.rules[i].field.split('.');
                        let fieldFormat = splitBaseOnDotOperator[0] + ".[" + splitBaseOnDotOperator[1] + "]"
                        group.rules[i].data = "";
                        group.rules[i].databetween = "";
                        str += group.rules[i].group ?
                            computed(group.rules[i].group) :
                            " " + fieldFormat + " " + htmlEntities(group.rules[i].condition) + " ";
                    }
                    else if (group.rules[i].condition == 'LIKE' || group.rules[i].condition == 'NOT LIKE') {
                        let splitBaseOnDotOperator = group.rules[i].field.split('.');
                        let fieldFormat = splitBaseOnDotOperator[0] + ".[" + splitBaseOnDotOperator[1] + "]"
                        str += group.rules[i].group ?
                            computed(group.rules[i].group) :
                            " " + fieldFormat + " " + htmlEntities(group.rules[i].condition) + " '%" + group.rules[i].data + "%' ";
                    }
                    else {
                        let fieldFormat = "";
                        let splitBaseOnDotOperator = "";
                        if (typeof group.rules[i].field != "undefined" && group.rules[i].field != "") {
                            splitBaseOnDotOperator = group.rules[i].field.split('.');
                            fieldFormat = splitBaseOnDotOperator[0] + ".[" + splitBaseOnDotOperator[1] + "]";
                        }
                        str += group.rules[i].group ?
                            computed(group.rules[i].group) :
                            " " + fieldFormat + " " + htmlEntities(group.rules[i].condition) + " " + addremoveqoute(group.rules[i].data, group.rules[i].type) + " ";
                    }


                }
                scope.Querystring = str + ")";
                return str + ")";
            }
            scope.json = null;
            scope.filter = data;
            scope.$watch('filter', function (newValue) {
                scope.updateJson = newValue;
                scope.json = JSON.stringify(newValue, null, 2);
                scope.output = computed(newValue.group);
            }, true);
            //handleAddColumnsOrRemoveColumnInWhereCluase(vm, widgetIndex, scope, compile, vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName);
            //scope.Report;      
            //rootScope.testing = vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName;
            //scope.getAllReports = function () {
            //    $http.get("/Xcelerate/QueryBuilder/GetReport?Filter=byDefinationType").then(function (response) {

            //        scope.Report = response.data;
            //    });
            //};
        };
        //#endregion



        //#region handle treeview for multi view columns based on dataset ,render below the dataset in query builder section
        var handleTreeviewOfViewColumnBelowQueryBuilderSection = function (vm, widgetIndex, scope, compile, datasetName, drawTableOnCanvasStatuas) {

            handleGetViewColumnsBaseOnDataset(datasetName).success(function (response) {
                handleTreeviewForSchemasDesignerForParentChild(vm, widgetIndex, scope, compile, datasetName, response.obj, drawTableOnCanvasStatuas);

            });
        };
        //#endregion

        //#region remove view and its columns name
        var handleRemoveViewAndItsColumns = function (vm, widgetIndex, scope, compile, datasetName) {
            if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].treeviewOfMultiDatasetsWithItsColumns.length > 0) {
                let getViewAndItsColumnsNameIndex = vm.dashboardAndReportComInformation.widgetList[widgetIndex].treeviewOfMultiDatasetsWithItsColumns.findIndex(x => x.value == datasetName);
                let getListOfColumns = $.extend(true, {}, vm.dashboardAndReportComInformation.widgetList[widgetIndex].treeviewOfMultiDatasetsWithItsColumns.find(x => x.value == datasetName));
                if (getViewAndItsColumnsNameIndex > -1) {
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].treeviewOfMultiDatasetsWithItsColumns.splice(getViewAndItsColumnsNameIndex, 1);
                    handleRemoveTableForSchemaDesignerOnCanvas(vm, widgetIndex, datasetName, vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID);
                    handleRemoveColumnFromSelectedColumnListInMultiSchemaDesigner(vm, widgetIndex, scope, compile, datasetName, getListOfColumns.children);
                }
            }
            scope.$apply();
        };
        //#endregion

        //#region reloading the treeview and its columns name if dataset have selected
        var handleReloadingTreeviewItsColumns = function (vm, widgetIndex, scope, compile) {
            let getLengthOfTreeview = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews.length;
            if (getLengthOfTreeview > 0) {
                handleInitializationOfWhereClauseWhenSelectaDataSet(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.whereClauseJsonWithRuleAndGroup, scope, compile, null, vm, widgetIndex);
                for (let idx = 0; idx < getLengthOfTreeview; idx++) {
                    let getItemOfDataSet = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[idx];
                    handleTreeviewOfViewColumnBelowQueryBuilderSection(vm, widgetIndex, scope, compile, getItemOfDataSet, true);
                }
                $timeout(function () {
                    handleRenderEdgesWhenTemplateIsReloadingFromDB(vm, widgetIndex, scope, compile, vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID);

                }, 1000);

            }
        };
        //#endregion

        //#region schema designer save with  view and its columns name when checkbox is checked
        var handleSchemaDesignerSaveViewAndItsColumnsName = function (vm, widgetIndex, ivhNode, scope, compile) {
            if (typeof ivhNode.children != "undefined") {
                hanldeAddAndDrawAllColumnForSchamDesginer(vm, widgetIndex, ivhNode, scope, compile);
            }

            else {
                handleAddColumnOnlyForSchemaDesigner(vm, widgetIndex, ivhNode, scope, compile);

            }
        };
        //#endregion

        //#region add and draw all columns for schema designer;
        var hanldeAddAndDrawAllColumnForSchamDesginer = function (vm, widgetIndex, ivhNode, scope, compile) {
            if (ivhNode.children.length > 0) {
                for (let i = 0; i < ivhNode.children.length; i++) {

                    handleAddColumnOnlyForSchemaDesigner(vm, widgetIndex, ivhNode.children[i]);
                    handleRenderSelectedColumnWithMultiSchemaDesigner(vm, widgetIndex, ivhNode.children[i]['value'], ivhNode.children[i]['label'], ivhNode.children[i]['dataType'], scope, compile);
                }
            }
        };
        //#endregion

        //#region add column only for schema designer
        var handleAddColumnOnlyForSchemaDesigner = function (vm, widgetIndex, ivhNode, scope, compile) {
            let schemaDesignerOnKeyValue = {};
            schemaDesignerOnKeyValue['key'] = ivhNode['value'];
            schemaDesignerOnKeyValue['value'] = {};
            schemaDesignerOnKeyValue['value']['label'] = ivhNode['label'];
            schemaDesignerOnKeyValue['value']['dataType'] = ivhNode['dataType'];
            let getColumnIndex = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue.findIndex(x => x.key == ivhNode['value'] && x.value['label'] == ivhNode['label']);
            if (getColumnIndex < 0) {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue.push(schemaDesignerOnKeyValue);
                handleDrawColumnInTableForSchemaDesigner(vm, widgetIndex, ivhNode['value'], ivhNode['label'], vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID);
                handleRenderSelectedColumnWithMultiSchemaDesigner(vm, widgetIndex, ivhNode['value'], ivhNode['label'], ivhNode['dataType'], scope, compile);
            }

            //console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue);
        };
        //#endregion

        //#region draw column in table for schema designer
        var handleDrawColumnInTableForSchemaDesigner = function (vm, widgetIndex, viewName, columnName, UniqueKey) {
            let ColumnCount = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue.filter(x => x.key == viewName).length;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.insertVertex(vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.model.cells[handleUniqueForTable(viewName, UniqueKey)], handleUniqueForColumn(viewName, columnName, UniqueKey), columnName, 0, ColumnCount * 28, 200, 28, 'gradientColor=#333;fontColor=#FFF;fillColor=#212529;fontSize=12; fontColor=#FFF;fontFamily=Poppins;');
        };
        //#endregion

        //#region redraw column in table for schema designer
        var handleRedrawColumnInTableForSchemaDesigner = function (vm, widgetIndex, viewName, columnName, UniqueKey, size) {
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.insertVertex(vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.model.cells[handleUniqueForTable(viewName, UniqueKey)], handleUniqueForColumn(viewName, columnName, UniqueKey), columnName, 0, size * 28, 200, 28, 'gradientColor=#333;fontColor=#FFF;fillColor=#212529;fontSize=12; fontColor=#FFF;fontFamily=Poppins;');
        };
        //#endregion

        //#region remove column in table for schema designer on canvas
        var handleRemoveColumnInTableForSchemaDesignerCanvas = function (vm, widgetIndex, viewName, columnName, UniqueKey) {
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().beginUpdate();
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.removeCells([vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.model.cells[handleUniqueForColumn(viewName, columnName, UniqueKey)]]);
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().endUpdate();
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.refresh();
            handleRepositioningOfColumnsWhenDeleteFromCanvas(vm, widgetIndex, viewName, columnName, UniqueKey);
        };
        //#endregion

        //#region draw table for schema designer
        var handleDrawTableForSchemaDesigner = function (vm, widgetIndex, viewName, UniqueKey) {
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.insertVertex(vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getDefaultParent(), handleUniqueForTable(viewName, UniqueKey), viewName, 0, 0, 200, 22, 'gradientColor=#FFF;swimlane;fontStyle=0;stroke="#333";fontFamily=Poppins;fontColor=#FFF;childLayout=stackLayout;horizontal=1;startSize=20;fillColor=#009de4;horizontalStack=0;resizeParent=0;resizeParentMax=0;resizeLast=0;collapsible=1;marginBottom=0;swimlaneFillColor=#ffffff;align=center;fontSize=16;verticalAlign="top";font-weight=bold;');
        };
        //#endregion

        //#region remove table for schema designer on canvas
        var handleRemoveTableForSchemaDesignerOnCanvas = function (vm, widgetIndex, viewName, UniqueKey) {
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.removeCells([vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.model.cells[handleUniqueForTable(viewName, UniqueKey)]]);



        };
        //#endregion

        //#region delete view and its columns name for schama designer
        var handleDeleteViewAndItsColumnsNameForSchemaDesigner = function (vm, widgetIndex, ivhNode, scope, compile) {
            if (typeof ivhNode.children != "undefined") {

                handleDeleteAllColumnsFromCanvasAndUncheckToAllCheckBoxAView(vm, widgetIndex, ivhNode);
            }
            else {
                let getColumnIndex = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue.findIndex(x => x.key == ivhNode['value'] && x.value['label'] == ivhNode['label']);
                if (getColumnIndex > -1) {
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue.splice(getColumnIndex, 1);
                    handleRemoveColumnInTableForSchemaDesignerCanvas(vm, widgetIndex, ivhNode['value'], ivhNode['label'], vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID);
                    handleDeleteSelectedColumnWithMultiSchemaDesigner(vm, widgetIndex, ivhNode['value'], ivhNode['label'], ivhNode['dataType'], scope, compile);
                }
                //console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue);
            }

        };
        //#endregion

        //#region delete all columns from canvas area and also uncheck to checkbox which have a view
        var handleDeleteAllColumnsFromCanvasAndUncheckToAllCheckBoxAView = function (vm, widgetIndex, ivhNode) {
            if (ivhNode.children.length > 0) {
                for (let i = 0; i < ivhNode.children.length; i++) {
                    let getColumnIndex = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue.findIndex(x => x.key == ivhNode.children[i]['value'] && x.value['label'] == ivhNode.children[i]['label']);
                    if (getColumnIndex > -1) {
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue.splice(getColumnIndex, 1);
                        handleRemoveColumnInTableForSchemaDesignerCanvas(vm, widgetIndex, ivhNode.children[i]['value'], ivhNode.children[i]['label'], vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID);
                        handleDeleteSelectedColumnWithMultiSchemaDesigner(vm, widgetIndex, ivhNode.children[i]['value'], ivhNode.children[i]['label'], ivhNode.children[i]['dataType'], null, null);

                    }
                }
            }

        };
        //#endregion

        //#region repositioning of the columns when delete the column from the canvas area
        var handleRepositioningOfColumnsWhenDeleteFromCanvas = function (vm, widgetIndex, viewName, columnName, UniqueKey) {
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().beginUpdate();
            let getViewAndItsColumns = vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.model.cells[handleUniqueForTable(viewName, UniqueKey)];
            if (getViewAndItsColumns) {
                getViewAndItsColumns.geometry.height -= 28;
                if (typeof getViewAndItsColumns.children != undefined) {
                    for (let idx = 0, heightCount = 1; idx < getViewAndItsColumns.children.length; idx++, heightCount++) {
                        let getChild = getViewAndItsColumns.children[idx];
                        getChild.geometry.y = heightCount * 28;

                    }
                }
            }
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().endUpdate();
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.refresh();
        };
        //#endregion

        //#region get Unique key for table
        var handleUniqueForTable = function (viewName, uniqueKey) {
            return viewName.replace(/ /g, '') + uniqueKey;
        };

        //#endregion

        //#region get Unique key for column
        var handleUniqueForColumn = function (viewName, columnName, uniqueKey) {
            return viewName.replace(/ /g, '') + columnName.replace(/ /g, '') + uniqueKey;
        };

        //#endregion

        //#region Get Id Of Column In Treeview For Schema Desginer
        var handleGetIdOfColumnInTreeviewForSchemaDesginer = function (viewName, coulumnName) {
            return viewName.replace(/ /g, '') + coulumnName.replace(/ /g, '');
        };

        //#endregion

        //#region add join detail with source and target 
        var handleAddJoinDetailWithSourceAndTarget = function (vm, widgetIndex, scope, compile, sourceTableName, targetTableName, sourceColumnName, targetColumnName) {
            let makeJsonForJoinDetail = {};
            makeJsonForJoinDetail['source'] = {};
            makeJsonForJoinDetail['source']['key'] = sourceTableName + "-" + targetTableName;
            let makejsonForSourceValueOfJoin = {};
            makejsonForSourceValueOfJoin['columnName'] = sourceColumnName;
            makejsonForSourceValueOfJoin['tableName'] = sourceTableName;
            makeJsonForJoinDetail['source']['value'] = [];
            makeJsonForJoinDetail['source']['value'].push(makejsonForSourceValueOfJoin);

            makeJsonForJoinDetail['target'] = {};
            makeJsonForJoinDetail['target']['key'] = targetTableName + "-" + sourceTableName;
            let makejsonForTargetValueOfJoin = {};
            makejsonForTargetValueOfJoin['columnName'] = targetColumnName;
            makejsonForTargetValueOfJoin['tableName'] = targetTableName;
            makeJsonForJoinDetail['target']['value'] = [];
            makeJsonForJoinDetail['target']['value'].push(makejsonForTargetValueOfJoin);
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.push(makeJsonForJoinDetail);
        };
        //#endregion

        //#region add join detail with value of source and target only
        var handleAddJoinDetailWithValueOfSourceAndTargetOnly = function (vm, widgetIndex, scope, compile, sourceTableName, targetTableName, sourceColumnName, targetColumnName) {
            let checkValueOfSourceAndTargetExsitOrNot = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.find(x => x.source.key.indexOf(sourceTableName) > -1 && x.target.key.indexOf(targetTableName) > -1);
            let makejsonForSourceValueOfJoin = {};
            makejsonForSourceValueOfJoin['columnName'] = sourceColumnName;
            makejsonForSourceValueOfJoin['tableName'] = sourceTableName;
            checkValueOfSourceAndTargetExsitOrNot.source.value.push(makejsonForSourceValueOfJoin);
            let makejsonForTargetValueOfJoin = {};
            makejsonForTargetValueOfJoin['columnName'] = targetColumnName;
            makejsonForTargetValueOfJoin['tableName'] = targetTableName;
            checkValueOfSourceAndTargetExsitOrNot.target.value.push(makejsonForTargetValueOfJoin);
        };
        //#endregion

        //#region get index of join detail with source and target 
        var handleGetIndexJoinDetailWithSourceAndTarget = function (vm, widgetIndex, scope, compile, source, target) {

            let getIndexSourceAndTarget = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.findIndex(x => x.source.key.indexOf(source) > -1 && x.target.key.indexOf(target) > -1);
            return getIndexSourceAndTarget;

        };
        //#endregion

        //#region get index of  columns are exist with source and target in join details
        var handleGetIndexColumnExistInJoinSourceAndTarget = function (source, target, sourColumnName, targetColumnName, sourceTableName, targetTableName) {
            let getIndexOfSource = source.findIndex(x => x.columnName == sourColumnName && x.tableName == sourceTableName);
            let getIndexOfTarget = target.findIndex(x => x.columnName == targetColumnName && x.tableName == targetTableName);
            if (getIndexOfSource == getIndexOfTarget)
                return getIndexOfSource;
        };

        //#endregion

        //#region check join detail with source and target are exist or not
        var handleCheckJoinDetailWithSourceAndTargetExistOrNot = function (vm, widgetIndex, scope, compile, sourceTableName, targetTableName) {

            let checkSourceAndTargetExistOrNot = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.find(x => x.source.key.indexOf(sourceTableName) > -1 && x.target.key.indexOf(targetTableName) > -1);
            if (checkSourceAndTargetExistOrNot) { return true; }
            else { return false; }
        };
        //#endregion

        //#region check columns are exist with source and target in join details
        var handleCheckColumnExistInJoinSourceAndTarget = function (source, target, sourceColumnName, targetColumnName) {
            if (source.findIndex(x => x.columnName == sourceColumnName) > -1 && target.findIndex(x => x.columnName == targetColumnName) > -1) {
                return true;
            }
            else {

                return false;
            }
        };

        //#endregion

        //#region check join detail with value of source and target
        var handleCheckJoinDetailWithValueOfSourceAndTarget = function (vm, widgetIndex, scope, compile, sourceTableName, targetTableName, sourceColumnName, targetColumnName) {
            let checkValueOfSourceAndTargetExsitOrNot = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.find(x => x.source.key.indexOf(sourceTableName) > -1 && x.target.key.indexOf(targetTableName) > -1);
            if (handleCheckColumnExistInJoinSourceAndTarget(checkValueOfSourceAndTargetExsitOrNot.source.value, checkValueOfSourceAndTargetExsitOrNot.target.value, sourceColumnName, targetColumnName)) { return true; }
            else { return false; }

        };
        //#endregion

        //#region maintain json for join
        var maintainJsonForJoin = function (vm, widgetIndex, scope, compile) {
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.selectCells(false, true, '');
            let getAllEdges = vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getSelectionCells();
            let gelLengthOfEdges = getAllEdges.length;
            if (gelLengthOfEdges > 0) {
                for (let idx = 0; idx < gelLengthOfEdges; idx++) {
                    let getEdgeObject = getAllEdges[idx];
                    console.log(getEdgeObject);
                    if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.length > 0) {

                        if (!handleCheckJoinDetailWithSourceAndTargetExistOrNot(vm, widgetIndex, scope, compile, getEdgeObject.source.parent.value, getEdgeObject.target.parent.value)) {
                            handleAddJoinDetailWithSourceAndTarget(vm, widgetIndex, scope, compile, getEdgeObject.source.parent.value, getEdgeObject.target.parent.value, getEdgeObject.source.value, getEdgeObject.target.value);
                        } else {
                            if (!handleCheckJoinDetailWithValueOfSourceAndTarget(vm, widgetIndex, scope, compile, getEdgeObject.source.parent.value, getEdgeObject.target.parent.value, getEdgeObject.source.value, getEdgeObject.target.value)) {
                                handleAddJoinDetailWithValueOfSourceAndTargetOnly(vm, widgetIndex, scope, compile, getEdgeObject.source.parent.value, getEdgeObject.target.parent.value, getEdgeObject.source.value, getEdgeObject.target.value);
                            }

                        }
                    }
                    else {
                        handleAddJoinDetailWithSourceAndTarget(vm, widgetIndex, scope, compile, getEdgeObject.source.parent.value, getEdgeObject.target.parent.value, getEdgeObject.source.value, getEdgeObject.target.value);
                    }
                }
            }
            // console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue);

        };
        //#endregion 

        //#region draw an point on edge when make a join between two tables
        var handleDrawAnPointOnEdge = function (vm, widgetIndex, scope, compile, uniqueKey, edgeReference, exeStatus) {

            var vertexPoint = vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.insertVertex(edgeReference, null, '', 0, 0, 19, 10,
                'shape=circle;labelBackgroundColor=#ffffff;labelPosition=left;spacingRight=0;align=right;fontStyle=0;');
            vertexPoint.geometry.offset = new mxPoint(-10, -7);
            vertexPoint.geometry.relative = true;
            vertexPoint.connectable = false;
            if (exeStatus) {
                $timeout(function () {
                    maintainJsonForJoin(vm, widgetIndex, scope, compile);
                    handleCheckJoinTypeExistOrNot(vm, widgetIndex, scope, compile, uniqueKey);
                }, 50);
            }
        };
        //#endregion

        //#region Add Source and Target object if not exist in db object through form of properties link

        var handleAddSourceTargetPropertiesLink = function (vm, widgetIndex, sourceTableName, targetTableName, sourceColumnName, targetColumnName, scope, compile, callBack, uniqueKey, joinType, operatorType) {
            let status = false;
            if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.length > 0) {
                if (!handleCheckJoinDetailWithSourceAndTargetExistOrNot(vm, widgetIndex, scope, compile, sourceTableName, targetTableName)) {
                    status = true;
                    handleAddJoinDetailWithSourceAndTarget(vm, widgetIndex, scope, compile, sourceTableName, targetTableName, sourceColumnName, targetColumnName);
                } else {
                    if (!handleCheckJoinDetailWithValueOfSourceAndTarget(vm, widgetIndex, scope, compile, sourceTableName, targetTableName, sourceColumnName, targetColumnName)) {
                        status = true;
                        handleAddJoinDetailWithValueOfSourceAndTargetOnly(vm, widgetIndex, scope, compile, sourceTableName, targetTableName, sourceColumnName, targetColumnName);
                    }
                }
            }
            else {
                status = true;
                handleAddJoinDetailWithSourceAndTarget(vm, widgetIndex, scope, compile, sourceTableName, targetTableName, sourceColumnName, targetColumnName);
            }
            if (status) {
                callBack(vm, widgetIndex, sourceTableName, targetTableName, sourceColumnName, targetColumnName, scope, compile, uniqueKey, joinType, operatorType);
            }
        };

        //#endregion

        //#region calculate length of listOfSourceTargetColumns
        var calculateLengthOflistOfSourceTargetColumns = function (listOfSourceTargetColumns) {
            return listOfSourceTargetColumns.listOfOperatorSourceAndTargetColumnsName.length;
        };
        //#endregion

        //#region check source,target and operatorType whether source,target and operatorType are undefined or not

        var handleCheckSourceTarterOperatorTypeUndefinedOrNotPropertiesLink = function (listOfSourceTargetColumns) {
            if (listOfSourceTargetColumns.source != undefined && listOfSourceTargetColumns.target != undefined && listOfSourceTargetColumns.joinType != undefined) {
                return true;
            }
            else {

                return false;
            }

        };

        //#endregion

        //#region check source ,target column and operatorType whether source,target column and operatorType are undefined or not

        var handleCheckSourceTarterColumnOperatorTypeUndefinedOrNotPropertiesLink = function (listOfSourceTargetColumns) {
            if (listOfSourceTargetColumns.source != undefined && listOfSourceTargetColumns.target != undefined && listOfSourceTargetColumns.operatorType != undefined) {
                return true;
            }
            else {

                return false;
            }

        };

        //#endregion

        //#region Draw edge on canvas

        var handleDrawEdgeOnCanvas = function (vm, widgetIndex, sourceTableName, targetTableName, sourceColumnName, targetColumnName, scope, compile, uniqueKey, joinType, operatorType) {
            console.log('Run callback now');
            let edgeReference = vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.insertEdge(vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getDefaultParent(), '', null, vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().cells[handleUniqueForColumn(sourceTableName, sourceColumnName, uniqueKey)], vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().cells[handleUniqueForColumn(targetTableName, targetColumnName, uniqueKey)], 'startArrow=oval;endArrow=block;sourcePerimeterSpacing=4;startFill=0;endFill=0;');
            handleDrawAnPointOnEdge(vm, widgetIndex, scope, compile, uniqueKey, edgeReference, false);
            $timeout(function () {
                //  handleCheckJoinTypeExistOrNot(vm, widgetIndex, scope, compile, uniqueKey);
                handleAddJoinTypePropertiesLink(vm, widgetIndex, handleCheckJoinDetailWithSourceAndTargetExistOrNotWithIndex(vm, widgetIndex, scope, compile, sourceTableName, targetTableName), handleCheckJoinDetailWithValueOfSourceAndTargetWithIndex(vm, widgetIndex, scope, compile, sourceTableName, targetTableName, sourceColumnName, targetColumnName), joinType, operatorType, scope, compile);

            }, 0);
        };

        //#endregion

        //#region check columns are exist with source and target in join details with index
        var handleCheckColumnExistInJoinSourceAndTargetWithindex = function (source, target, sourceColumnName, targetColumnName) {
            return source.findIndex(x => x.columnName == sourceColumnName) > -1 && target.findIndex(x => x.columnName == targetColumnName);

        };

        //#endregion

        //#region check join detail with value of source and target with index
        var handleCheckJoinDetailWithValueOfSourceAndTargetWithIndex = function (vm, widgetIndex, scope, compile, sourceTableName, targetTableName, sourceColumnName, targetColumnName) {
            let checkValueOfSourceAndTargetExsitOrNot = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.find(x => x.source.key.indexOf(sourceTableName) > -1 && x.target.key.indexOf(targetTableName) > -1);
            return handleCheckColumnExistInJoinSourceAndTargetWithindex(checkValueOfSourceAndTargetExsitOrNot.source.value, checkValueOfSourceAndTargetExsitOrNot.target.value, sourceColumnName, targetColumnName);

        };
        //#endregion

        //#region check join detail with source and target are exist or not if exist then return index
        var handleCheckJoinDetailWithSourceAndTargetExistOrNotWithIndex = function (vm, widgetIndex, scope, compile, sourceTableName, targetTableName) {

            let checkSourceAndTargetExistOrNotIndex = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.findIndex(x => x.source.key.indexOf(sourceTableName) > -1 && x.target.key.indexOf(targetTableName) > -1);
            return checkSourceAndTargetExistOrNotIndex;
        };
        //#endregion

        //#region Draw edge,point and save json into db Object through form of properties link

        var handleDrawEdgesPointSavejsonIntoDbObject = function (vm, widgetIndex, listOfSourceTargetColumns, scope, compile, uniqueKey) {
            if (handleCheckSourceTarterOperatorTypeUndefinedOrNotPropertiesLink(listOfSourceTargetColumns)) {
                let getlenghtOflistOfSourceTargetColumns = calculateLengthOflistOfSourceTargetColumns(listOfSourceTargetColumns);
                if (getlenghtOflistOfSourceTargetColumns > 0) {
                    for (let idx = 0; idx < getlenghtOflistOfSourceTargetColumns; idx++) {
                        let getItemOfSourceTargetColumn = listOfSourceTargetColumns.listOfOperatorSourceAndTargetColumnsName[idx];
                        if (handleCheckSourceTarterColumnOperatorTypeUndefinedOrNotPropertiesLink(getItemOfSourceTargetColumn)) {
                            handleAddSourceTargetPropertiesLink(vm, widgetIndex, listOfSourceTargetColumns.source, listOfSourceTargetColumns.target, getItemOfSourceTargetColumn.source, getItemOfSourceTargetColumn.target, scope, compile, handleDrawEdgeOnCanvas, uniqueKey, listOfSourceTargetColumns.joinType, getItemOfSourceTargetColumn.operatorType);
                        }

                    }
                }
            }
            else {
                //do some logic here
            }
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().endUpdate();
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.refresh();

        };

        //#endregion

        //#region render edges when template is reloading from database       
        var handleRenderEdgesWhenTemplateIsReloadingFromDB = function (vm, widgetIndex, scope, compile, uniqueKey) {
            let getLengthOfJoin = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.length;
            if (getLengthOfJoin > 0) {
                for (let idx = 0; idx < getLengthOfJoin; idx++) {
                    let getItem = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue[idx];
                    for (let idxColumn = 0; idxColumn < getItem.source.value.length; idxColumn++) {
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().beginUpdate();
                        let edgeReference = vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.insertEdge(vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getDefaultParent(), '', null, vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().cells[handleUniqueForColumn(getItem.source.value[idxColumn].tableName, getItem.source.value[idxColumn].columnName, uniqueKey)], vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().cells[handleUniqueForColumn(getItem.target.value[idxColumn].tableName, getItem.target.value[idxColumn].columnName, uniqueKey)], 'startArrow=oval;endArrow=block;sourcePerimeterSpacing=4;startFill=0;endFill=0;');
                        handleDrawAnPointOnEdge(vm, widgetIndex, scope, compile, uniqueKey, edgeReference, false);
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().endUpdate();
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.refresh();
                        handleAddJoinType(vm, widgetIndex, idx, idxColumn, scope, compile);

                    }

                }

            }

        };
        //#endregion

        //#region check join type is exist or not if join is not exist in current state please add a join type which is missed

        var handleCheckJoinTypeExistOrNot = function (vm, widgetIndex, scope, compile, uniqueKey) {
            let getLengthOfJoin = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.length;
            if (getLengthOfJoin > 0) {
                for (let idx = 0; idx < getLengthOfJoin; idx++) {
                    let getItem = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue[idx];
                    for (let idxColumn = 0; idxColumn < getItem.source.value.length; idxColumn++) {
                        handleAddJoinType(vm, widgetIndex, idx, idxColumn, scope, compile);
                    }

                }

            }
        };

        //#endregion

        //#region make join from join Details object
        var handleMakeJoinFromJoinDetailsObject = function (vm, widgetIndex, scope, compile) {
            let getLengthOfJoin = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.length;
            let makeJoins = "";
            if (getLengthOfJoin > 0) {
                for (let idx = 0; idx < getLengthOfJoin; idx++) {
                    let getItem = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue[idx];
                    if (idx == 0)
                        makeJoins += getItem.source.key.split('-')[0] + " AS " + getItem.source.key.split('-')[0] + " " + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[idx].joinType + " JOIN " + getItem.target.key.split('-')[0] + " AS " + getItem.target.key.split('-')[0] + " ON ";
                    else
                        makeJoins += " " + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[idx].joinType + " JOIN " + getItem.target.key.split('-')[0] + " AS " + getItem.target.key.split('-')[0] + " ON ";

                    for (let idxColumn = 0; idxColumn < getItem.source.value.length;) {
                        makeJoins += getItem.source.value[idxColumn].tableName + "." + "[" + getItem.source.value[idxColumn].columnName + "]" + "=" + getItem.target.value[idxColumn].tableName + "." + "[" + getItem.target.value[idxColumn].columnName + "]";
                        idxColumn++;
                        if (idxColumn != getItem.source.value.length)
                            makeJoins += " " + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[idx][idxColumn] + " ";
                    }

                }

            }
            //console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue)
            //console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType)
            //console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference)
            return makeJoins;
            // vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[idx].joinType 
        };

        //#endregion

        //#region Add join type through properties link

        var handleAddJoinTypePropertiesLink = function (vm, widgetIndex, joinDetailIndex, JoinInnerDetailIndex, joinType, OperatorType, scope, compile) {

            if (!vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[joinDetailIndex]) {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[joinDetailIndex] = {};
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[joinDetailIndex]['joinType'] = joinType;
            }
            else {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[joinDetailIndex]['joinType'] = joinType;
            }
            if (!vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[joinDetailIndex]) {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[joinDetailIndex] = {};
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[joinDetailIndex][JoinInnerDetailIndex] = OperatorType;

            }
            else {
                if (!vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[joinDetailIndex][JoinInnerDetailIndex]) {
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[joinDetailIndex][JoinInnerDetailIndex] = OperatorType;
                }
                else {
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[joinDetailIndex][JoinInnerDetailIndex] = OperatorType;
                }
            }

        };

        //#endregion

        //#region add join type when add an edge between two tables
        var handleAddJoinType = function (vm, widgetIndex, joinDetailIndex, JoinInnerDetailIndex, scope, compile) {
            if (!vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[joinDetailIndex]) {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[joinDetailIndex] = {};
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[joinDetailIndex]['joinType'] = "INNER";
            }
            if (!vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[joinDetailIndex]) {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[joinDetailIndex] = {};
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[joinDetailIndex][JoinInnerDetailIndex] = "AND";

            }
            else {
                if (!vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[joinDetailIndex][JoinInnerDetailIndex]) {
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[joinDetailIndex][JoinInnerDetailIndex] = "AND";
                }
            }

            //  console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType);
        };
        //#endregion

        //#region remove join type when add an edge between two tables
        var handleRemoveJoinType = function (vm, widgetIndex, joinDetailIndex, JoinInnerDetailIndex, scope, compile) {
        };
        //#endregion

        //#region [mouse click] on mxgraph
        var handleMouseClickMxGraph = function (vm, widgetIndex, evtName, me, sender, scope, compile) {
            if (me != null && me["parent"] != null) {
                if (me.parent.source != null && me.parent.target != null) {
                    // console.log(me.parent.source.parent.value + " - " + me.parent.source.value + ' to ' + me.parent.target.parent.value + " - " + me.parent.target.value);
                    let getIndexOfSourceandTarget = handleGetIndexJoinDetailWithSourceAndTarget(vm, widgetIndex, scope, compile, me.parent.source.parent.value, me.parent.target.parent.value);
                    let checkValueOfSourceAndTargetExsitOrNot = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinDetailsKeyValue.find(x => x.source.key.indexOf(me.parent.source.parent.value) > -1 && x.target.key.indexOf(me.parent.target.parent.value) > -1);
                    let getIndexOfColumnInSourceAndTarget = handleGetIndexColumnExistInJoinSourceAndTarget(checkValueOfSourceAndTargetExsitOrNot.source.value, checkValueOfSourceAndTargetExsitOrNot.target.value, me.parent.source.value, me.parent.target.value, me.parent.source.parent.value, me.parent.target.parent.value);

                    vm.openLinkProperteisModal = $uibModal.open({
                        templateUrl: 'dsrpt/DashboardNReport/getPartialPage/QueryBuilder/_JoinType.cshtml',
                        backdrop: 'static',
                        controller: "LinkProperties as vm",
                        resolve: {
                            pvm: function () {
                                return vm;
                            },
                            widgetIndex: function () {
                                return widgetIndex;
                            },
                            JoinDetailIndex: function () {
                                return getIndexOfSourceandTarget;
                            },
                            JoinDetailInnerIndex: function () {
                                return getIndexOfColumnInSourceAndTarget;
                            }
                        }
                    });
                    vm.openLinkProperteisModal.result.then(function () {

                    });
                }
            }
        };
        //#endregion

        //#region [mouse up] on mxgraph
        var handleMouseUpMxGraph = function (vm, widgetIndex, evtName, me, sender) {
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.setCellsLocked(true);
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.setConnectable(true);
        };
        //#endregion

        //#region [mouse down] on mxgraph
        var handleMouseDownMxGraph = function (vm, widgetIndex, evtName, me, sender) {
            if (me.getCell() != null && me.getCell()["children"] != null) {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.setCellsLocked(false);
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.setConnectable(false);
            }
            else if (me.getCell() != null) {
                if (checkViewOrColumnForSchemaDesigner(vm, widgetIndex, me.getCell()['value'])) {
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.setCellsLocked(false);
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.setConnectable(false);
                }
            }

        };
        //#endregion

        //#region change event

        var handleEgesConnectWithVertex = function (vm, widgetIndex, uniqueKey, scope, compile, sender, evt) {
            let edgeReference = evt.getProperty('cell');
            handleDrawAnPointOnEdge(vm, widgetIndex, scope, compile, uniqueKey, edgeReference, true);

        };

        //#endregion

        //#region check view or column for schema designer
        var checkViewOrColumnForSchemaDesigner = function (vm, widgetIndex, viewOrColumnName) {
            if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews.findIndex(x => x == viewOrColumnName) > -1) {
                return true;
            }
            else {
                return false;
            }
        };
        //#endregion

        //#region get xml of mxgraph from mxmodel,encode

        var handleGetXmlMxGraphFromModel = function (vm, widgetIndex, scope, compile) {
            return mxGraphEncoderOrDecoder.encode(vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel());
        };

        //#endregion

        //#region get mxmodel of mxgraph from xml,decode

        var handleGetMxModelMxGraphFromXml = function (vm, widgetIndex, scope, compile) {
            return mxGraphEncoderOrDecoder.decode(mxGraphEncoderOrDecoder.encode(vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel()));
        };

        //#endregion

        //#region Initialization of the mx events for mouse control
        var handleInitializationOfMxEvent = function (vm, widgetIndex, scope, compile) {

            //#region fireMouseEvent

            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.fireMouseEvent = function (evtName, me, sender) {

                switch (evtName) {
                    case mxEvent.MOUSE_UP:
                        handleMouseUpMxGraph(vm, widgetIndex, evtName, me, sender);
                        break;
                    case mxEvent.MOUSE_DOWN:
                        handleMouseDownMxGraph(vm, widgetIndex, evtName, me, sender);
                        break;
                    default:

                }
                mxGraph.prototype.fireMouseEvent.apply(this, arguments);

            };
            //#endregion

            //#region click event
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.addListener(mxEvent.CLICK, function (sender, evt) {
                let event = evt.getProperty('event'); // mouse event
                let cell = evt.getProperty('cell'); // cell may be null

                handleMouseClickMxGraph(vm, widgetIndex, event, cell, sender, scope, compile);
            });
            //#endregion

            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.connectionHandler.addListener(mxEvent.CONNECT, function (sender, evt) {
                handleEgesConnectWithVertex(vm, widgetIndex, vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID, scope, compile, sender, evt);

            });

            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.setCellsLocked(true);


        };
        //#endregion

        //#region Initialization of the mx events for key handler
        var handleInitializationOfKeyHandler = function (vm, widgetIndex, scope, compile) {

            //#region Removes cells when [DELETE] is pressed
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxKeyHandler = new mxKeyHandler(vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph);
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxKeyHandler.bindKey(46, function (evt) {

                if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.isEnabled() && vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.selectionModel.cells.length > 0) {
                    if (!checkViewOrColumnForSchemaDesigner(vm, widgetIndex, vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.selectionModel.cells[0].value) && vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.selectionModel.cells[0].children == null) {
                        let columnName = vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.selectionModel.cells[0].value;
                        let viewName = vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.selectionModel.cells[0].parent.value;
                        handleDeleteViewAndItsColumnsNameForSchemaDesigner(vm, widgetIndex, { label: columnName, value: viewName }, scope, compile);
                        handleUncheckToCheckOfTreeviewWhenColumnIsDeletedFromCanvasArea(vm, widgetIndex, viewName, columnName, scope, compile,true);
                    }
                    //vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.removeCells();
                }
            });
            //#endregion

        };
        //#endregion

        //#region connectable edges between two nodes
        var handleConnectableEdgesTwoNodes = function (vm, widgetIndex, scope, compile) {

            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.setConnectable(true);
        };
        //#endregion

        //#region disconnectable edges between two nodes
        var handleDisconnectableEdgesTwoNodes = function (vm, widgetIndex, scope, compile) {

            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.setConnectable(false);
        };
        //#endregion

        //#region set custom style for edge
        var handleSetCustomStyleForEdge = function (vm, widgetIndex, scope, compile) {

            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxEdgesStyle = vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getStylesheet().getDefaultEdgeStyle();
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxEdgesStyle[mxConstants.STYLE_EDGE] = mxEdgeStyle.ElbowConnector;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxEdgesStyle[mxConstants.STYLE_ELBOW] = mxConstants.ELBOW_HORIZENTAL;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxEdgesStyle[mxConstants.STYLE_ROUNDED] = true;
        };
        //#endregion

        //#region uncheck to the checkbox of the treeview for schema designer when delete column from canvas area
        var handleUncheckToCheckOfTreeviewWhenColumnIsDeletedFromCanvasArea = function (vm, widgetIndex, viewName, ColumnName, scope, compile, applyStatus) {
            let getColumnFromTreeview = vm.dashboardAndReportComInformation.widgetList[widgetIndex].treeviewOfMultiDatasetsWithItsColumns.find(v => v.id === viewName).children.find(x => x.id === handleGetIdOfColumnInTreeviewForSchemaDesginer(viewName, ColumnName));
            ivhTreeviewMgr.deselect(vm.dashboardAndReportComInformation.widgetList[widgetIndex].treeviewOfMultiDatasetsWithItsColumns, getColumnFromTreeview);
            if (applyStatus)
                scope.$apply();
        };
        //#endregion

        //#region mxgraph initialization
        var handleInitializationOfMxGraph = function (vm, widgetIndex, scope, compile, selectorForInitializationOfMxGraph) {

            if (!mxClient.isBrowserSupported()) {
                // Displays an error message if the browser is
                // not supported.
                mxUtils.error('Browser is not supported!', 200, false);
            }
            else {
                let container = airviewXcelerateAppSettings.createDivBaseOnParams('absolute', 'auto', '0px', '0px', '0px', '0px', 'Areas/BusinessIntelligence/assets/mxgraph/examples/editors/images/grid.gif');
                selectorForInitializationOfMxGraph.append(container);
                if (mxClient.IS_QUIRKS) {
                    new mxDivResizer(container);
                }
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxModel = new mxGraphModel();
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph = new mxGraph(container, vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxModel);
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.setAllowDanglingEdges(true);
                new mxRubberband(vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph);
                handleInitializationOfMxEvent(vm, widgetIndex, scope, compile);
                handleInitializationOfKeyHandler(vm, widgetIndex, scope, compile);
                handleConnectableEdgesTwoNodes(vm, widgetIndex, scope, compile);
                handleSetCustomStyleForEdge(vm, widgetIndex, scope, compile);

            }
        };

        //#region Add multiple join in json and do draw edge and point on canvas area
        var handleAddMultipleJoinAndDrawMultipleEdges = function (vm, widgetIndex, scope, compile) {

            vm.openModalForMultipleJoin = $uibModal.open({
                templateUrl: 'dsrpt/DashboardNReport/getPartialPage/QueryBuilder/_AddMultipleJoin.cshtml',
                backdrop: 'static',
                controller: "AddMultipleJoin as vm",
                size: 'lg',
                resolve: {
                    pvm: function () {
                        return vm;
                    },
                    widgetIndex: function () {
                        return widgetIndex;
                    },
                    parentScope: function () { return scope; }
                }
            });
            vm.openModalForMultipleJoin.result.then(function () {

            });
            console.log(vm.openModalForMultipleJoin);


        };
        //#endregion




        //#endregion

        //#region Initialization for subquery variables
        var handleInitializationForSubquery = function (vm, widgetIndex, scope, rootScope, compile, uniqueId) {
            if (!vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId]) {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId] = {};
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId].multiSelectedViews = [];
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId].multiSchmaDesignerOnKeyValue = [];
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId].joinDetailsKeyValue = [];
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId].joinTypeWithForeignKeyReference = {};
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId].joinOperatorType = {};
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId].joinSectoin = "";
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId].whereClause = '';
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId].whereClauseJsonWithRuleAndGroup = JSON.parse(' { "group": { "operator": "AND", "rules": [] } }');
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId].sqlQueryBuild = "";

            }





        };
        //#endregion

        //#region assign value to main queries variables

        var handleAssignValueToMainQueriesVariables = function (vm, widgetIndex, scope, compile, uniqueId, rootScope) {

            vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentMultiSelectedViews;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].treeviewOfMultiDatasetsWithItsColumns = [];
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentMultiSelectedViews = [];
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.whereClauseJsonWithRuleAndGroup = scope.updateJson;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.whereClause = scope.output;
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName = [];
            let whereClausejson = JSON.parse('{ "group": { "operator": "AND", "rules": [] }}');
            if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId]) {
                whereClausejson = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId].whereClauseJsonWithRuleAndGroup;
            }
            //vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[uniqueId].whereClauseJsonWithRuleAndGroup = JSON.parse(' { "group": { "operator": "AND", "rules": [] } }');
            handleInitializationOfWhereClauseWhenSelectaDataSet(whereClausejson, scope, compile, rootScope, vm, widgetIndex);
            handleAddColumnsOrRemoveColumnInWhereCluase(vm, widgetIndex, scope, compile, vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName);

        };

        //#endregion

        //#region assign value to sub query

        var handleAssignValueToSubQuery = function (vm, widgetIndex, scope, compile, currentUnique) {
            //let currentUnique = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId;
            let getObject = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[currentUnique];
            if (getObject) {
                getObject.multiSelectedViews = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentMultiSelectedViews;
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].treeviewOfMultiDatasetsWithItsColumns = [];
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentMultiSelectedViews = [];
                getObject.whereClauseJsonWithRuleAndGroup = scope.updateJson;
                getObject.whereClause = scope.output;
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName = [];
            }


        };

        //#endregion

        //#region render main query
        var handleRenderDatasetAnditsColumnsMainQuery = function (vm, widgetIndex, scope, compile, currentUnique) {

            handleAssignValueToSubQuery(vm, widgetIndex, scope, compile, currentUnique);
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentMultiSelectedViews = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews;
            let getLengthOfTreeview = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews.length;
            if (getLengthOfTreeview > 0) {
                handleInitializationOfWhereClauseWhenSelectaDataSet(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.whereClauseJsonWithRuleAndGroup, scope, compile, null, vm, widgetIndex);
                for (let idx = 0; idx < getLengthOfTreeview; idx++) {
                    let getItemOfDataSet = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[idx];
                    handleTreeviewOfViewColumnBelowQueryBuilderSection(vm, widgetIndex, scope, compile, getItemOfDataSet, false);
                }
            }
        };
        //#endregion

        //#region rerender dataset and its column whether it is main query or subquery
        var reRenderDataSetAndItsColumnWithQueryAndSubquery = function (vm, widgetIndex, scope, compile, currentUnique) {


            //let currentUnique = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId;
            let getObject = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[currentUnique];
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentMultiSelectedViews = getObject.multiSelectedViews;
            let getLengthOfTreeview = getObject.multiSelectedViews.length;
            if (getLengthOfTreeview > 0) {
                handleInitializationOfWhereClauseWhenSelectaDataSet(getObject.whereClauseJsonWithRuleAndGroup, scope, compile, null, vm, widgetIndex);
                for (let idx = 0; idx < getLengthOfTreeview; idx++) {
                    let getItemOfDataSet = getObject.multiSelectedViews[idx];
                    handleTreeviewOfViewColumnBelowQueryBuilderSection(vm, widgetIndex, scope, compile, getItemOfDataSet, false);
                }
            }

        };
        //#endregion


        //#region Make sub query

        var handleAddSubQuery = function (vm, widgetIndex, scope, compile, rootScope) {
            if (!vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueryStatus) {
                let uniqueId = handleUniquekey(5);
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueryStatus = true;
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId = uniqueId;

                handleAssignValueToMainQueriesVariables(vm, widgetIndex, scope, compile, uniqueId, rootScope);


            }



           // console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels);
        };

        //#endregion

        //#region add subquery column in main query
        var handleApplySubquery = function (vm, widgetIndex, viewName, columnName, columnDataType, scope, compile) {
            //debugger

            var uniqueId = viewName.replace(/ /g, '') + columnName.replace(/ /g, '') + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID;
            // var uniqueId = columnName.replace(/ /g, '') + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID;
            if ($('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".selected-rows .tg-row#" + uniqueId).length < 1) {
                let getRowHtml = ($QueryBuilderRows[0]['code']).replace(/chngidx/g, widgetIndex).replace(/uniqueKey/g, "'" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId + "'");
                var newRow = $.parseHTML(getRowHtml);
                compile($(newRow).find('.clone').attr('ng-click', 'vm.copyselectColumn($event,' + widgetIndex + ')'))(scope);
                numbercount = numbercount + 1;
                $(newRow).attr("id", uniqueId);
                $(newRow).attr("rowid", numbercount);

                var viewcolumndatatype = columnDataType;
                LoadAggregateFunctions(viewcolumndatatype, $(newRow));
                LoadSortFunctions($(newRow));
                $(newRow).find('.tbl-columnname').attr('value', viewName + '.' + columnName);
                $(newRow).find('.db-column-data-type').attr('orginal-data-type', viewcolumndatatype);
                $(newRow).find('.db-column-data-type').attr('current-data-type', viewcolumndatatype);
                $(newRow).find('.db-column-data-type').attr('unique-key-subquery', columnName);
                //let getIndexOfunSelectedColumns = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.findIndex(x => x == uniqueId);
                //if (getIndexOfunSelectedColumns < 0)
                //    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsID.push(uniqueId);
                // console.log(newRow);
                $(newRow).find('.tbl-function').attr('onChange', 'ChangeAggregateFunction(this,' + widgetIndex + ')');
                $(newRow).find('.clone').parent().remove();
                newRow = compile(newRow)(scope);

                vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn.add([newRow[0]]);
                //if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount > 0) {
                //    $(newRow).find(".tbl-group").prop("checked", true);
                //    $(newRow).find(".tbl-group").attr("disabled", true);
                //}
            }


        };
        //#endregion


        //#region maintain history of query builder for subquery
        var handleHistoryOfQueryBuilderForSubquery = function (vm, widgetIndex, historyOfQueryBuilder) {
            vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId].historyOfQueryBuilder = historyOfQueryBuilder;
            //  console.log(vm.dashboardAndReportComInformation.widgetList[wigetIndex].widgetConfiguration.historyOfQueryBuilder);
        };
        //#endregion





        //#region get documents

        var handleGetDocuments = function (documentPath, selector) {
            $.get('/BusinessIntelligence/DashboardNReport/DocumentViewer', { documentPath: documentPath }, function (resp, status, error) {
                selector.empty();
                selector.html(resp);

            });

        };

        //#endregion

        //#region Get default template ID which are active agaist by userID

        var handleGetDefaultTemplateIDByUserID = function (filter) {

            return $http({
                method: 'POST',
                url: '/BusinessIntelligence/DashboardNReport/GetTemplate',
                data: { userID: userIDByX1, filter: filter }
            });
        };

        //#endregion

        //#region Get default template by User ID which are actived
        var handleGetDefaultTemplateByUserID = function (templateID) {
            return $http({
                method: 'POST',
                url: '/BusinessIntelligence/DashboardNReport/GetTemplateById',
                data: { TemplateID: templateID }
            });
        };

        //#endregion

        //#region render treeview for document path
        var handleDocumentPath = function (vm, scope, compile) {

            let itemTreeview = {};
            itemTreeview['label'] = 'Documents';
            itemTreeview['id'] = 'Documents';
            itemTreeview['selected'] = false;
            itemTreeview['value'] = 'Documents';
            itemTreeview['children'] = [];
            let makeDataTreeview = {};
            makeDataTreeview['label'] = 'sample.ppt';
            makeDataTreeview['id'] = 'ppt';
            makeDataTreeview['value'] = '~/Content/SamplePPT.ppt';
            makeDataTreeview['selected'] = false;
            makeDataTreeview['children'] = [];
            itemTreeview.children.push(makeDataTreeview);
            let makeDataTreeview1 = {};
            makeDataTreeview1['label'] = 'DocumentNew.pdf';
            makeDataTreeview1['id'] = 'ppt';
            makeDataTreeview1['value'] = '~/Content/DocumentNew.pdf';
            makeDataTreeview1['selected'] = false;
            makeDataTreeview1['children'] = [];
            itemTreeview.children.push(makeDataTreeview1);
            let makeDataTreeview2 = {};
            makeDataTreeview2['label'] = 'DocumentExcel.xlsx';
            makeDataTreeview2['id'] = 'ppt';
            makeDataTreeview2['value'] = '~/Content/DocumentExcel.xlsx';
            makeDataTreeview2['selected'] = false;
            makeDataTreeview2['children'] = [];
            itemTreeview.children.push(makeDataTreeview2);
            vm.dashboardAndReportComInformation.documentPath.push(itemTreeview);

        //    console.log(vm.dashboardAndReportComInformation.documentPath);

        };
        //#endregion

        //#region lock grid stack
        var handleLockGridStack = function () {
            var lockGridStack = $('.grid-stack').data('gridstack');
            lockGridStack.enableMove(false, true);
            lockGridStack.enableResize(false, true);
        };
        //#endregion

        //#region unlock grid stack
        var handleUnlockGridStack = function () {
            var lockGridStack = $('.grid-stack').data('gridstack');
            lockGridStack.enableMove(true, true);
            lockGridStack.enableResize(true, true);
        };
        //#endregion

        //#region add some for classes for left panel

        var handleAddSomeAdditionalClassesForLeftPanel = function () {

            $(".side-panel").css({ "width": "calc(100% - 240px)" });
            $('.side-panel').addClass('active');
            $("#queryBuilderSection").addClass(' active show')
            $("#queryBuilderSection").children(".querybuilder-wrapper").css('display', 'flex');
            $('.sidepanel-btn').addClass('active menu-button');

        };

        //#endregion

        //#region delete all column column when add subquery

        var handleDeleteAllColumnWhenAddSubquery = function (vm, widgetIndex) {

            var findRow = $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".selected-rows .tg-row");
            let arr = [];
            $.each(findRow, function (idx, ele) {
                arr.push(ele);
                var getId = $(ele).attr('id');

            });
            $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".columns-list li input[type='checkbox'], .select-all-row").prop('checked', false);

            vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn.remove(arr, { removeElements: true });


        }

        //#endregion

        //#region Initialization
        var hanldeInitialization = function (compile, scope, vm, uiGridConstants, $timeout, toastr, rootScope) {
            rootScope.simpleCompile = function (element, parentElement) {
                var compiledcode = $.parseHTML(element);
                compiledcode = compile(compiledcode)(scope);
                $(parentElement).append(compiledcode);
            };
            scope.iconsclass = [];
            //$.get("assets/js/fontawesome-5.json", function (data) {
            //    let tempArr = [];
            //    $.each(Object.keys(data), function (i, e) {
            //        tempArr.push({ key: e, value: data[e] });
            //    });
            //    scope.iconsclass = tempArr;

            //});

            //#region load default tempalte by user ID
            handlejQueryEventsInitialization(scope, compile, vm, toastr);
            //handleGetDefaultTemplateIDByUserID("GetPriTmpOnlyActiveByUserID").then(function successCallback(res) {
            //    if (res.data.status) {
            //        var getTemplate = $.parseJSON(res.data.data);
            //        handleGetDefaultTemplateByUserID(getTemplate[0].TemplateId).then(function successCallback(response) {
            //            handleJsonForFrontend(compile, scope, vm, uiGridConstants, response.data.data, rootScope);
            //            handleChangeModeType(compile, scope, vm, 'view');
            //            addWidgerHandler(scope, compile, vm, toastrs);
            //            handleLockGridStack();
            //            xl_script.scrollbar();

            //        }, function errorCallback(response) {
            //            // called asynchronously if an error occurs
            //            // or server returns response with an error status.
            //        });

            //    }

            //    else {
            //        toastr.success('Data has not been found');
            //        scope.$emit('changeModeTypeInTopHeaderE', 'nothings');
            //    }
            //    // this callback will be called asynchronously
            //    // when the response is available
            //}, function errorCallback(response) {
            //    // called asynchronously if an error occurs
            //    // or server returns response with an error status.
            //});

            //#endregion

            //#region variables

            vm.dashboardAndReportComInformation = {};
            vm.dashboardAndReportComInformation.templateTitle = "";
            vm.dashboardAndReportComInformation.templateType = "";
            vm.dashboardAndReportComInformation.TemplateID = "";
            vm.dashboardAndReportComInformation.save = false;
            vm.dashboardAndReportComInformation.viewsInformation = [];
            vm.dashboardAndReportComInformation.widgetList = [];
            vm.dashboardAndReportComInformation.documentPath = [];
            vm.modeType = 'edit';
            vm.chart = {};
            vm.chart.settings = {};
            vm.chart.settings.chartTitleSettings = {};
            vm.chart.settings.chartSubTitleSettings = {};
            vm.chart.settings.legendSettings = {};
            vm.chart.settings.XaxisSettings = {};
            vm.chart.settings.YaxisSettings = {};
            vm.multiSchemasDesigner = {};
            uiGridConstantsGloabl = uiGridConstants;
            toastrs = toastr;
            scope.imageUploadOfPageWidget = function (fileSec, widgetIndex) {
                let imageIndex = $(fileSec).attr('image-index');
                handleGetImageOfPage(fileSec, widgetIndex, imageIndex, compile, scope, vm);
            };
            vm.customOpts = {
                defaultSelectedState: true,
                useCheckboxes: true,
                validate: true,
                twistieExpandedTpl: '<i class="fa fa-caret-down"></i>',
                twistieCollapsedTpl: '<i class="fa fa-caret-right"></i>',
                twistieLeafTpl: ' ',
                expandToDepth: 0
            };
            //#endregion

            //#region change on widget when comes change on filter
            vm.changeNodeOnCheckBox = function (ivhNode, ivhIsSelected, ivhTree, widgetIndex) {
                if (ivhIsSelected) {
                    handleDeleteValueOfCriteriaForAllWidgets(vm, widgetIndex, ivhNode);

                }
                else {

                    hanldePutValueOfCriteriaForAllWidgets(vm, widgetIndex, ivhNode);
                }

                //$timeout(function () { handleGetDataToGivenValueForAllWidgets(vm, widgetIndex, scope, compile); }, 100); 

            };
            //#endregion

            //#region apply filter when click on apply filter button on each widget
            vm.applyFilter = function (widgetIndex) {
                handleGetDataToGivenValueForAllWidgets(vm, widgetIndex, scope, compile);
            };

            //#endregion

            //#region pagination Active and Inactive
            vm.paginationAction = function (WidgetIndex) {

                let GetQuery = vm.dashboardAndReportComInformation.widgetList[WidgetIndex].widgetConfiguration.sqlQueryBuild;

                if (GetQuery.length > 0) {
                    if (vm.dashboardAndReportComInformation.widgetList[WidgetIndex].uiGridConfiguration.paginationStatus) {
                        GetQuery = handleMakeQueryBeforeRenderGrid(GetQuery, vm.dashboardAndReportComInformation.widgetList[WidgetIndex].uiGridConfiguration.paginationParams.paginationSize, 1, vm.dashboardAndReportComInformation.widgetList[WidgetIndex].uiGridConfiguration.paginationParams.orderBy);
                    }
                    // console.log(GetQuery);
                    handleGetDataFromDbForTable(WidgetIndex, GetQuery, vm, true).done(function (response) {
                        if (vm.dashboardAndReportComInformation.widgetList[WidgetIndex].uiGridConfiguration.paginationStatus) {
                            //debugger
                            if (vm.dashboardAndReportComInformation.widgetList[WidgetIndex].data.length > 0) {
                                vm.dashboardAndReportComInformation.widgetList[WidgetIndex].uiGridConfiguration.paginationParams.totalItemsOfUiGrid = vm.dashboardAndReportComInformation.widgetList[WidgetIndex].data[0].ROWCOUNTS;
                                vm.dashboardAndReportComInformation.widgetList[WidgetIndex].data.filter(function (i) {
                                    delete i.ROWCOUNTS;
                                });
                            }

                            handleUiGridSettingsAndInitialization(compile, scope, vm, uiGridConstants, WidgetIndex);
                            toastr.success('Applied pagination');
                        }
                        else {
                            vm.dashboardAndReportComInformation.widgetList[WidgetIndex].SearchText = '';
                            $('#' + vm.dashboardAndReportComInformation.widgetList[WidgetIndex].widgetID).find('.searchdiv').attr('style', 'display:none');
                            handleGenericTableInitialization(compile, scope, vm, WidgetIndex);
                            toastr.success('Removed pagination');
                        }

                    });
                }
                else {

                    if (vm.dashboardAndReportComInformation.widgetList[WidgetIndex].uiGridConfiguration.paginationStatus) {
                        handleUiGridSettingsAndInitialization(compile, scope, vm, uiGridConstants, WidgetIndex);

                    }
                    else {
                        vm.dashboardAndReportComInformation.widgetList[WidgetIndex].SearchText = '';
                        $('#' + vm.dashboardAndReportComInformation.widgetList[WidgetIndex].widgetID).find('.searchdiv').attr('style', 'display:none');
                        handleGenericTableInitialization(compile, scope, vm, WidgetIndex);

                    }
                }

            };
            //#endregion

            //#region render table data
            vm.renderTableData = function (WigetIndex) {



                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.paginationStatus) {
                    // vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.uiGridInitailization.data = [];
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.uiGridInitailization.data = vm.dashboardAndReportComInformation.widgetList[WigetIndex].data;
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].uiGridConfiguration.gridApi.core.refresh();
                }
                else {
                    handleGenericTableInitialization(compile, scope, vm, WigetIndex)
                }


            };
            //#endregion

            //#region render data
            vm.renderGrid = function (WigetIndex) {
                switch (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType) {
                    case 'widgetOfTable':
                        vm.renderTableData(WigetIndex);
                        break;
                    case 'widgetOfChart':
                        vm.chart.renderDataOfChart(WigetIndex);
                        break;
                    default:
                }



            };
            //#endregion


            scope.$on("createEtlB", function (event, etlParams) {
              //  console.log(etlParams)
                getViewsOfDb(vm);

                vm.dashboardAndReportComInformation = {};
                vm.dashboardAndReportComInformation.scheduler = {};
                vm.dashboardAndReportComInformation.scheduler = etlParams;
                vm.dashboardAndReportComInformation.scheduler['UniqueKey'] = handleUniquekey(7);
                vm.dashboardAndReportComInformation.templateTitle = "";
                vm.dashboardAndReportComInformation.templateType = "";
                vm.dashboardAndReportComInformation.TemplateID = "";
                vm.dashboardAndReportComInformation.description = etlParams.description;
                vm.dashboardAndReportComInformation.save = false;
                vm.dashboardAndReportComInformation.IsActive = false;
                vm.dashboardAndReportComInformation.publicTmp = etlParams.isPublic;
                vm.dashboardAndReportComInformation.viewsInformation = [];
                vm.dashboardAndReportComInformation.widgetList = [];
                vm.dashboardAndReportComInformation.templateTitle = etlParams.templateTitle;
                vm.dashboardAndReportComInformation.templateType = etlParams.templateType;
                vm.dashboardAndReportComInformation.pageID = handleUniquekey(6);
                let panelType = "widgetOfTable";
                let getPanelCodeBaseOnWidgetTypeElement = getWidgetSettings.find(x => x.type == panelType);
                let getQueryBuilderElement = getWidgetSettings.find(x => x.type == "queryBuilder").code;
                handleDragWidget(getPanelCodeBaseOnWidgetTypeElement.code, addWidgetOfGridStackItem, $(".side-panel .tab-content .tab-pane > .toggle"), getPanelCodeBaseOnWidgetTypeElement.settings, getPanelCodeBaseOnWidgetTypeElement.advanced_settings, getQueryBuilderElement, $(".side-panel #queryBuilderSection"), compile, scope, vm, panelType, toastrs, null, "-1");
                handleAddSomeAdditionalClassesForLeftPanel();

            });

            scope.$on("updateEtlB", function (event, etlParams) {
               // console.log(etlParams)
                // getViewsOfDb(vm);


                vm.dashboardAndReportComInformation.scheduler = {};
                vm.dashboardAndReportComInformation.scheduler = etlParams;


                vm.dashboardAndReportComInformation.description = etlParams.description;

                vm.dashboardAndReportComInformation.publicTmp = etlParams.isPublic;

                vm.dashboardAndReportComInformation.templateTitle = etlParams.templateTitle;
                vm.dashboardAndReportComInformation.templateType = etlParams.templateType;

              
            });


            //#region temp region
            scope.$on("setDefaultTemplateFromTopHeaderB", function (event, setDefaultTemplate) {

                vm.dashboardAndReportComInformation.IsActive = setDefaultTemplate;
            });
            scope.$on("setPublicTemplateFromTopHeaderB", function (event, setDefaultTemplate) {

                vm.dashboardAndReportComInformation.publicTmp = setDefaultTemplate;
            });


            scope.$on("multiJoinSourceAndTargetListB", function (event, multiJoinList) {
                handleDrawEdgesPointSavejsonIntoDbObject(vm, multiJoinList.widgetIndex, multiJoinList.multipleJoinSourceTargetListA, scope, compile, vm.dashboardAndReportComInformation.widgetList[multiJoinList.widgetIndex].widgetID);
            });
            scope.$on("dashboardAndReportB", function (event, dashboardInfo) {
                handleChangeModeType(compile, scope, vm, 'edit');

                vm.dashboardAndReportComInformation = {};
                vm.dashboardAndReportComInformation.templateTitle = "";
                vm.dashboardAndReportComInformation.templateType = "";
                vm.dashboardAndReportComInformation.TemplateID = "";
                vm.dashboardAndReportComInformation.save = false;
                vm.dashboardAndReportComInformation.IsActive = false;
                vm.dashboardAndReportComInformation.publicTmp = false;
                vm.dashboardAndReportComInformation.viewsInformation = [];
                vm.dashboardAndReportComInformation.widgetList = [];
                vm.dashboardAndReportComInformation.documentPath = [];
                var getPanelsPageSettings = $(".side-panel .tab-content  .tab-pane > .toggle > div").slice(0, 2).detach();
                $(".side-panel .tab-content  .tab-pane > .toggle").empty();
                $(".side-panel .tab-content  .tab-pane > .toggle").append(getPanelsPageSettings);
                $('.new-template').empty();
                $('#queryBuilderSection').empty();
                indexOfWidget = 0;
                var compiled = "<div class='grid-stack' data-pagename='" + dashboardInfo.templateTitle + "'></div>";
                $(".new-template").addClass("start-drawing").append(compiled);
                scope.$emit('initializationOfGridStack');
                vm.dashboardAndReportComInformation.templateTitle = dashboardInfo.templateTitle;
                vm.dashboardAndReportComInformation.templateType = dashboardInfo.templateType;
                vm.dashboardAndReportComInformation.pageID = handleUniquekey(6);
                addWidgerHandler(scope, compile, vm, toastr);
                airviewXcelerateAppSettings.getAddNewWidgetInGridStack();
                xl_script.scrollbar();
                getViewsOfDb(vm);
                handleDocumentPath(vm, scope, compile);
                handleUnlockGridStack();

            });

            var tabInfo = [];
            rootScope.AddTabs = function (tabTitle, tabIcon, target) {
                tabInfo.tab = {};
                tabInfo.tab.target = target;
                tabInfo.tab.faIcon = tabIcon;
                tabInfo.tab.title = tabTitle;
                tabInfo.push(tabInfo.tab);
                console.log(tabInfo);
            };
            scope.$on("LoadForTempB", function (event, dashboardInfo) {
                handleChangeModeType(compile, scope, vm, 'edit');
                vm.dashboardAndReportComInformation = {};
                vm.dashboardAndReportComInformation.templateTitle = "";
                vm.dashboardAndReportComInformation.templateType = "";
                vm.dashboardAndReportComInformation.TemplateID = "";
                vm.dashboardAndReportComInformation.save = true;
                vm.dashboardAndReportComInformation.IsActive = false;
                vm.dashboardAndReportComInformation.publicTmp = false;
                vm.dashboardAndReportComInformation.viewsInformation = [];
                vm.dashboardAndReportComInformation.widgetList = [];
                vm.dashboardAndReportComInformation.documentPath = [];
                var getPanelsPageSettings = $(".side-panel .tab-content  .tab-pane > .toggle > div").slice(0, 2).detach();
                $(".side-panel .tab-content  .tab-pane > .toggle").empty();
                $(".side-panel .tab-content  .tab-pane > .toggle").append(getPanelsPageSettings);
                $('.new-template').empty();
                $('#queryBuilderSection').empty();
                $http({
                    method: 'POST',
                    url: '/BusinessIntelligence/DashboardNReport/GetTemplateById',
                    data: { TemplateID: dashboardInfo }
                }).then(function successCallback(response) {

                    // console.log(response.data.data);
                    handleJsonForFrontend(compile, scope, vm, uiGridConstants, response.data.data, rootScope);
                    addWidgerHandler(scope, compile, vm, toastrs);
                    xl_script.scrollbar();
                    // this callback will be called asynchronously
                    // when the response is available
                }, function errorCallback(response) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
                //DashboardAndReport.drawWidgets(compile, scope, vm, callback, uiGridConstants);
                handleDocumentPath(vm, scope, compile);
                handleUnlockGridStack();
            });
            scope.$on("changeModeTypeB", function (event, modeType) {
                handleChangeModeType(compile, scope, vm, modeType);
                $('.sidepanel-btn').removeClass('active').next('.side-panel').removeClass('active');
                $(".new-template").removeClass('shrink');
                vm.resizeAllWidgetOnCrossButton();
            });
            scope.$on("saveTemplateB", function (event) {
                vm.saveTemplate();
            });
            scope.$on("ResizeAllWidgetB", function (event) {
                if (vm.dashboardAndReportComInformation.widgetList.length > 0) {
                    for (let idx = 0; idx < vm.dashboardAndReportComInformation.widgetList.length; idx++) {

                        handlereSizeAllWidget(compile, scope, vm, idx, uiGridConstants, $timeout);
                    }
                }
            });
            vm.resizeAllWidgetOnCrossButton = function () {
                if (vm.dashboardAndReportComInformation.widgetList.length > 0) {
                    for (let idx = 0; idx < vm.dashboardAndReportComInformation.widgetList.length; idx++) {

                        handlereSizeAllWidget(compile, scope, vm, idx, uiGridConstants, $timeout);
                    }
                }
            };
            vm.saveTemplate = function () {
                var listitem = [];

                let getAllItems = xl_script.getGridItems();
                $(getAllItems).each(function (index, value) {
                    listitem.push(Number(getAllItems[index].el.attr('data-position')));
                });

                for (let i = 0, postionIdx = 0; i < listitem.length; i++) {
                    let getWidget = vm.dashboardAndReportComInformation.widgetList.find(x => x.widgetConfiguration.pagePosition == listitem[i]);
                    if (getWidget && !getWidget.isDeleted) {
                        getWidget.widgetConfiguration.pagePosition = postionIdx;
                        postionIdx++;
                    }
                }

                copyDataOfWidget = vm.dashboardAndReportComInformation;
                var makeJsonOfDatabaseWithExtend = $.extend(true, {}, vm.dashboardAndReportComInformation);
                if (makeJsonOfDatabaseWithExtend.widgetList.length > 0) {
                    for (var i = 0; i < makeJsonOfDatabaseWithExtend.widgetList.length; i++) {
                        let getItem = makeJsonOfDatabaseWithExtend.widgetList[i];
                        getItem.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsHtml = [];
                        let allItemsOfWidget = typeof getItem.murriInitializationOfSelectedColumn.getItems == "undefined" ? [] : getItem.murriInitializationOfSelectedColumn.getItems();
                        if (allItemsOfWidget.length > 0) {
                            for (var x = 0; x < allItemsOfWidget.length; x++) {
                                getItem.widgetConfiguration.murriHistoryOfSelectedColumns.selectedColumnsHtml.push(allItemsOfWidget[x]._element.outerHTML);


                            }
                        }
                    }
                }

                handleSetValueTopLeftWidthAndRightOfWidget(makeJsonOfDatabaseWithExtend).done((res) => {
                    handleJsonForDatabase(res, vm, toastrs, scope);
                });
                //  console.log(makeJsonOfDatabaseWithExtend);

            };
            vm.clear = function () {
                $(".new-template").empty();
                $('.toggle-item').not(':first').remove();




            };
            vm.draw = function () {
                vm.dashboardAndReportComInformation = {};
                handleJsonForFrontend(compile, scope, vm, uiGridConstants, DashboardAndReportJsonFromDb);
                //vm.dashboardAndReportComInformation = copyDataOfWidget;
                //vm.dashboardAndReportComInformation.widgetList.sort(function (a, b) { return a.widgetConfiguration.pagePosition - b.widgetConfiguration.pagePosition });
                //copyDataOfWidget = {};
                //handleDrawWidgets(compile, scope, vm, function () {
                //    setTimeout(function () { scope.$emit('murri') }, 100);
                //}, uiGridConstants);


            };

            vm.chart.companiesName = [{ id: 1, keycode: "highCharts", companyName: `High charts` },
            { id: 2, keycode: "canvasCharts", companyName: `Canvas charts` }];
            vm.chart.nameOfChartTypes = [{ chartType: `line`, chartTypeName: `Line Chart`, groupingCharttype: `Line & Area` }, { chartType: `spline`, chartTypeName: `Spline Chart`, groupingCharttype: `Line & Area` }, { chartType: `splineArea`, chartTypeName: `Spline Area Chart`, groupingCharttype: `Line & Area` }, { chartType: `stepline`, chartTypeName: `Step Line Chart`, groupingCharttype: `Line & Area` }, { chartType: `area`, chartTypeName: `Area Chart`, groupingCharttype: `Line & Area` }, { chartType: `stackedArea`, chartTypeName: `Stacked Area Chart`, groupingCharttype: `Line & Area` }, { chartType: `steparea`, chartTypeName: `Step Area Chart`, groupingCharttype: `Line & Area` }, { chartType: `column`, chartTypeName: `Column Chart`, groupingCharttype: `Column & Bar` }, { chartType: `rangearea`, chartTypeName: `Range Area Chart`, groupingCharttype: `Line & Area` }, { chartType: `stackedColumn`, chartTypeName: `Stacked Column Chart`, groupingCharttype: `Column & Bar` }, { chartType: `stackedcolumn100`, chartTypeName: `Stacked Column 100% Chart`, groupingCharttype: `Column & Bar` }, { chartType: `stackedBar100`, chartTypeName: `Stacked Area 100% Chart`, groupingCharttype: `Column & Bar` }, { chartType: `rangecolumn`, chartTypeName: `Range Column Chart`, groupingCharttype: `Column & Bar` }, { chartType: `waterfall`, chartTypeName: `Waterfall chart`, groupingCharttype: `Column & Bar` }, { chartType: `stackedBar`, chartTypeName: `Stacked Bar Chart`, groupingCharttype: `Column & Bar` }, { chartType: `rangebar`, chartTypeName: `Range Bar Chart`, groupingCharttype: `Column & Bar` }, { chartType: `stackedbar100`, chartTypeName: `Stacked Bar 100% Chart`, groupingCharttype: `Column & Bar` }, { chartType: `bar`, chartTypeName: `Bar Chart`, groupingCharttype: `Column & Bar` }, { chartType: `pie`, chartTypeName: `Pie Chart`, groupingCharttype: `Pie & Funnel` }, { chartType: `funnel`, chartTypeName: `Funnel Chart`, groupingCharttype: `Pie & Funnel` }, { chartType: `doughnut`, chartTypeName: `Doughnut Chart`, groupingCharttype: `Pie & Funnel` }, { chartType: `multiseries`, chartTypeName: `Multi Series`, groupingCharttype: `Multi Series` }];
            vm.chart.fontFamily = [{ fontFamilyID: 1, fontFamily: "Calibri" }, { fontFamilyID: 1, fontFamily: "Optima" }, { fontFamilyID: 1, fontFamily: "Verdana" }, { fontFamilyID: 1, fontFamily: "Candara" }, { fontFamilyID: 1, fontFamily: "Geneva" }, { fontFamilyID: 1, fontFamily: "sans-serif" }, { fontFamilyID: 1, fontFamily: "arial" }, { fontFamilyID: 1, fontFamily: "tahoma" }, { fontFamilyID: 1, fontFamily: "verdana" }];
            vm.chart.verticalAlign = [{ verticalAlignID: 1, verticalAlignValue: "bottom" }, { verticalAlignID: 1, verticalAlignValue: "top" }, { verticalAlignID: 1, verticalAlignValue: "center" }];
            vm.chart.horizontalAlign = [{ horizontalAlignID: 1, horizontalAlignValue: "left" }, { horizontalAlignID: 1, horizontalAlignValue: "center" }, { horizontalAlignID: 1, horizontalAlignValue: "right" }];
            //#endregion

            //#region change chart company
            vm.chart.chartCompanyTypeChange = function (widgetIndex) {
                hanldeCheckChartCompanyType(compile, scope, vm, widgetIndex);
            };
            //#endregion

            //#region change chart Type
            vm.chart.chartTypeChange = function (widgetIndex) {

                // handleGetChartAndCompanytype(widgetIndex, vm, vm.dashboardAndReportComInformation.widgetList[widgetIndex].tempData);
                handleGetDataToGevenValueForCharts(vm, widgetIndex, scope, compile);
                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            //#endregion

            //#region chart title on blur event
            vm.chart.chartTitleWithBlur = function (widgetIndex) {
                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            //#endregion

            //#region chart subtitle on blur event
            vm.chart.chartSubtitleWithBlur = function (widgetIndex) {
                handleCheckChartType(compile, scope, vm, widgetIndex);
            };


            //#endregion

            //#region load data for chart
            //vm.chart.renderDataOfChart = function (widgetIndex) {

            //    vm.dashboardAndReportComInformation.widgetList[widgetIndex].data = [
            //        {
            //            type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,
            //            dataPoints: [
            //                { x: new Date(2012, 00, 1), y: 450 },
            //                { x: new Date(2012, 01, 1), y: 414 },
            //                { x: new Date(2012, 02, 1), y: 520 },
            //                { x: new Date(2012, 03, 1), y: 460 },
            //                { x: new Date(2012, 04, 1), y: 450 },
            //                { x: new Date(2012, 05, 1), y: 500 },
            //                { x: new Date(2012, 06, 1), y: 480 },
            //                { x: new Date(2012, 07, 1), y: 480 },
            //                { x: new Date(2012, 08, 1), y: 410 },
            //                { x: new Date(2012, 09, 1), y: 500 },
            //                { x: new Date(2012, 10, 1), y: 480 },
            //                { x: new Date(2012, 11, 1), y: 510 }
            //            ],
            //            showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
            //            name: "legend"
            //        }
            //    ];



            //}
            //vm.chart.renderDataOfChart1 = function (widgetIndex) {

            //    vm.dashboardAndReportComInformation.widgetList[widgetIndex].data = [
            //        {
            //            type: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.chartTypeName,

            //            dataPoints: [
            //                { x: new Date(2012, 00, 1), y: 450 },
            //                { x: new Date(2012, 01, 1), y: 414 },
            //                { x: new Date(2012, 02, 1), y: 520 },
            //                { x: new Date(2012, 03, 1), y: 460 },
            //                { x: new Date(2012, 04, 1), y: 450 },
            //                { x: new Date(2012, 05, 1), y: 500 },
            //                { x: new Date(2012, 06, 1), y: 480 },
            //                { x: new Date(2012, 07, 1), y: 480 },
            //                { x: new Date(2012, 08, 1), y: 410 },
            //                { x: new Date(2012, 09, 1), y: 500 },
            //                { x: new Date(2012, 10, 1), y: 480 },
            //                { x: new Date(2012, 11, 1), y: 510 }
            //            ],
            //            showInLegend: vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.settings.legendSettings.ShowInLegend,
            //            name: "legend"
            //        }
            //    ];



            //}
            //#endregion

            //#region Rerender or settings changes to the all widget if u change the panel size not used
            vm.reRenderOrSettingsChange = function (panelSize, widgetIndex) {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.classInfo.outerClass = panelSize;
                handlereSizeAllWidget(compile, scope, vm, widgetIndex, uiGridConstants, $timeout);

            };
            //#endregion

            //#region settings of the chart

            //#region setting of the title
            vm.chart.settings.chartTitleSettings.fontSize = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.chartTitleSettings.fontColor = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.chartTitleSettings.fontWeight = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.chartTitleSettings.fontStyle = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.chartTitleSettings.fontFamily = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            //#endregion

            //#region setting of the subtitle
            vm.chart.settings.chartSubTitleSettings.fontSize = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.chartSubTitleSettings.fontColor = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.chartSubTitleSettings.fontWeight = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.chartSubTitleSettings.fontStyle = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.chartSubTitleSettings.fontFamily = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            //#endregion

            //#region setting of the legend
            vm.chart.settings.legendSettings.fontSize = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.legendSettings.fontColor = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.legendSettings.fontWeight = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.legendSettings.fontStyle = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.legendSettings.fontFamily = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.legendSettings.verticalAlign = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.legendSettings.horizontalAlign = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.legendSettings.ShowInLegend = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            //#endregion

            //#region setting of X-axix
            vm.chart.settings.XaxisSettings.title = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.XaxisSettings.titleFontSize = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.XaxisSettings.titleFontColor = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.XaxisSettings.titleFontWeight = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.XaxisSettings.titleFontStyle = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.XaxisSettings.labelFontColor = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.XaxisSettings.labelFontSize = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.XaxisSettings.labelFontWeight = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.XaxisSettings.labelFontStyle = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.XaxisSettings.prefix = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.XaxisSettings.suffix = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };

            //#endregion

            //#region setting of Y-axix
            vm.chart.settings.YaxisSettings.title = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            }
            vm.chart.settings.YaxisSettings.titleFontSize = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.YaxisSettings.titleFontColor = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.YaxisSettings.titleFontWeight = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.YaxisSettings.titleFontStyle = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.YaxisSettings.labelFontColor = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.YaxisSettings.labelFontSize = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            }
            vm.chart.settings.YaxisSettings.labelFontWeight = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.YaxisSettings.labelFontStyle = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.YaxisSettings.prefix = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };
            vm.chart.settings.YaxisSettings.suffix = function (widgetIndex) {

                handleCheckChartType(compile, scope, vm, widgetIndex);
            };

            //#endregion


            //#endregion

            //#region save Query
            vm.saveQuery = function (WigetIndex) {
                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.currentMultiSelectedViews;
                //this condition for checking length of selected datasets
                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews.length > 0) {
                    //this condition for checking length of selected columns
                    if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].murriInitializationOfSelectedColumn.getItems().length > 0) {
                        if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchemaEnableOrDisable) {

                            handleExecuteQueryForMultiDataset(vm, WigetIndex, compile, scope, uiGridConstants, toastrs, true);
                        }
                        else {
                            handleExecuteQueryForSingleDataset(vm, WigetIndex, compile, scope, uiGridConstants, toastrs);
                        }
                    } else {
                        removeDataFromWidgets(WigetIndex);
                        toastrs.error("Select columns from data set");
                    }//end block of columns length
                } else {
                    removeDataFromWidgets(WigetIndex);
                    toastrs.error("Select data set");
                }//end block of datasets length
            };
            //#endregion

            //#region remove data when no data sets or columns are selected
            removeDataFromWidgets = function (WigetIndex) {
                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetType == "widgetOfTable") {
                    $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.searchdiv').empty();
                }

                vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sqlQueryBuild = null;
                $('#' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID).find('.content-wrapper').empty();


            };
            //#endregion
            //#region get selected columns
            vm.LoadSelectedColumn = function ($event, widgetIndex) {

                handleRenderSelectedColumn($event, vm, widgetIndex, scope, compile);
            };
            //#endregion

            //#region get columns of view when come change
            vm.ChangeDataSetViews = function (widgetIndex) {
                handleDeleteAllRowOfSelectedColumn(vm, widgetIndex);
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.whereClauseJsonWithRuleAndGroup = JSON.parse(' { "group": { "operator": "AND", "rules": [] } }');
                handleViewColumns(scope, compile, vm, widgetIndex, rootScope);
            };
            //#endregion

            //#region get views from db
            // getViewsOfDb(vm);
            //#endregion

            //#region on widget Click

            vm.loadDataOnWidgetClick = function (widgetIndex) {
                currentIndex = widgetIndex;
                if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchemaEnableOrDisable) {
                    handleInitializationOfWhereClauseWhenSelectaDataSet(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.whereClauseJsonWithRuleAndGroup, scope, compile, rootScope, vm, widgetIndex);
                    handleAddColumnsOrRemoveColumnInWhereCluase(vm, widgetIndex, scope, compile, vm.dashboardAndReportComInformation.widgetList[widgetIndex].SelectedTablesColumnName);

                }
                else {
                    LoadData(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.whereClauseJsonWithRuleAndGroup, scope, compile, rootScope, vm, widgetIndex);

                }
            };
            //#endregion

            //#region reload murri
            vm.reloadingOfMurri = function () {
                if (typeof vm.dashboardAndReportComInformation.widgetList[currentIndex] != "undefined") {
                    $timeout(function () { vm.dashboardAndReportComInformation.widgetList[currentIndex].murriInitializationOfSelectedColumn._resizeHandler(); }, 100);
                }

            };
            //#endregion

            //#region delete selected columns
            vm.DeleteQBSelectedRows = function (widgetIndex) {
                let arr = [];
                var thisRow = $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".selected-rows .tg-row .selectRow:checked").parents(".tg-row");
                thisRow.each(function (idx, ele) {
                    arr.push(ele);
                    var rowid = $(ele).attr('rowid');
                    var getId = $(ele).attr('id');
                    var node = $(ele).context;
                    var tblfunctionnode = $(node).find('.tbl-function');
                    if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount == 1) {
                        $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".tbl-group").prop("checked", false);
                        $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".tbl-group").attr("disabled", true);
                    }
                    if (tblfunctionnode.val() != '-1') {
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount - 1;
                        if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount == 0) {
                            $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".tbl-group").prop("checked", false);
                            $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".tbl-group").attr("disabled", false);
                        }
                    }

                    $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".side-panel .select-all-row, .columns-list li #" + getId).attr('checked', false);
                    let ViewNameAndColumnName = $(ele).find('.tbl-columnname').val().split('.');
                    handleDeleteViewAndItsColumnsNameForSchemaDesigner(vm, widgetIndex, { label: ViewNameAndColumnName[1], value: ViewNameAndColumnName[0] }, scope, compile);
                    handleUncheckToCheckOfTreeviewWhenColumnIsDeletedFromCanvasArea(vm, widgetIndex, ViewNameAndColumnName[0], ViewNameAndColumnName[1], scope, compile, false);



                });
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn.remove(arr, { removeElements: true });


                return false;
            };
            //#endregion

            //#region delete all columns
            vm.DeleteQBAllRows = function (widgetIndex) {
                var findRow = $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".selected-rows .tg-row");
                let arr = [];
                $.each(findRow, function (idx, ele) {
                    arr.push(ele);
                    var getId = $(ele).attr('id');
                    let ViewNameAndColumnName = $(ele).find('.tbl-columnname').val().split('.');
                    handleDeleteViewAndItsColumnsNameForSchemaDesigner(vm, widgetIndex, { label: ViewNameAndColumnName[1], value: ViewNameAndColumnName[0] }, scope, compile);
                    handleUncheckToCheckOfTreeviewWhenColumnIsDeletedFromCanvasArea(vm, widgetIndex, ViewNameAndColumnName[0], ViewNameAndColumnName[1], scope, compile, false);
                });
                $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".columns-list li input[type='checkbox'], .side-panel .select-all-row").attr('checked', false);

                vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn.remove(arr, { removeElements: true });
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount = 0;
                return false;
            };
            //#endregion

            //#region select all columns
            vm.SelectQBAllRows = function (e, widgetIndex) {

                if ($(e.target).is(':checked')) {
                    $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".selected-rows .tg-row .selectRow").prop('checked', true);
                } else {
                    $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".selected-rows .tg-row .selectRow").prop('checked', false);
                }
            };
            //#endregion 

            //#region aggregate function

            ChangeAggregateFunction = function (e, widgetIndex) {
                if ($(e).parents('.tg-row').find('.db-column-data-type').attr("orginal-data-type") != "datetime" && $(e).parents('.tg-row').find('.db-column-data-type').attr("orginal-data-type") != "date") {
                    if (e.value != '-1') {
                        //$(this).find("option:selected").each(function () {
                        //    console.log(this)
                        //    $(this).removeAttr('selected');
                        //});

                        $(e).parents('.tg-row').find('.db-column-data-type').attr("current-data-type", "int");
                        $(e).find('option[value=' + $(e).val() + ']').attr("selected", "selected");
                        if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount == 0) {
                            $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".tbl-group").prop("checked", true);
                            $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".tbl-group").attr("checked", true);
                            $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find(".tbl-group").attr("disabled", true);
                        }
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount + 1;
                        var nodesort = $(event.target).closest('.tg-row').find('.tbl-sort');
                        var node = $(event.target).closest('.tg-row').find('.tbl-group');

                        node.prop('checked', false);
                        node.removeAttr('checked');
                        node.attr("disabled", true);
                        nodesort.attr("disabled", true);
                        nodesort.val('-1');

                    }
                    else {
                        $(e).parents('.tg-row').find('.db-column-data-type').attr("current-data-type", $(e).parents('.tg-row').find('.db-column-data-type').attr("orginal-data-type"));
                        $(e).find('option').removeAttr("selected");
                        vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount - 1;
                        if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.aggreagteFunctionCount == 0) {
                            $(".tbl-group").prop("checked", false);
                            $(".tbl-group").attr("disabled", true);
                            $(".tbl-group").attr("checked", false);
                            $(".tbl-sort").attr("disabled", false);
                            $(".tbl-sort").val('-1');
                        }
                        else {
                            var node = $(event.target).closest('.tg-row').find('.tbl-group');
                            var nodesort = $(event.target).closest('.tg-row').find('.tbl-sort');
                            node.prop('checked', true);
                            node.attr('checked', true);
                            node.attr("disabled", true);
                            nodesort.attr("disabled", false);
                            nodesort.val('-1');
                        }
                    }
                }
                else {
                    if (e.value != '-1') { $(e).find('option[value=' + $(e).val() + ']').attr("selected", "selected"); }
                    else { $(e).find('option').removeAttr("selected"); }
                }

            };

            //#endregion

            //#region change sort function
            sortChangeFun = function (e) {
                if (e.value != '-1') {
                    $(e).find('option[value=' + $(e).val() + ']').attr("selected", "selected");
                }
                else {
                    $(e).find('option').removeAttr('selected');
                }

            };
            //#endregion

            //#region delete widget with confirmation 
            vm.removeWidget = function (event, widgetIndex) {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].isDeleted = true;
                let parent = $(event.target).parents('.grid-stack-item');
                var grid = $('.grid-stack').data('gridstack');
                $(event.target).parents('.widget').slideUp("complete", function () {
                    grid.removeWidget(parent, true);
                });
                $('.' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).fadeOut();
                event.preventDefault();
                event.stopPropagation();
                //document.dispatchEvent(new Event('UpdateItems'));
                //document.dispatchEvent(new Event('UpdateChildItems'));


            };
            //#endregion

            //#region search of ui grid implemented only frontend
            vm.SearchUiGrid = function (widgetconfig) {

                widgetconfig.uiGridConfiguration.gridApi.grid.refresh();



            };
            //#endregion

            //#region map functionality
            vm.ClickMarketLevelMap = function (WigetIndex) {
                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.MarketLevel == true) {
                    if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere == '()') {
                        GetQuery = "Select (COUNT(*)) as TotalMarketRecords, Market As RecordValue, Country, (select count(*) from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + ") as TotalRecords from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " GROUP by Market, RegionId, State, Country";
                    }
                    else {
                        GetQuery = "Select (COUNT(*)) as TotalMarketRecords, Market As RecordValue, Country, (select count(*) from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Where " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + ") as TotalRecords from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Where " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + " GROUP by Market, RegionId, State, Country";
                    }
                    handleGetDataFromDbForMap(WigetIndex, GetQuery, vm, true).done(function (result) {

                    });
                    $('#marketfiltermodal').modal('show');
                }

            };

            // code start
            vm.sourceChangedForMapPinSetting = function (WigetIndex) {
                var selectedColumn = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sourceForMapPinSetting;
                vm.dashboardAndReportComInformation.currentSelectedColumn = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sourceForMapPinSetting;
                initMapDynamic(compile, scope, vm, WigetIndex);
                handleFillTreeviewForMap(vm, WigetIndex);
            };
            // code end

            // code start
            vm.makeCloneOfWidget = function (WigetIndex) {
                var widgetJson = handleWidgetCloneJSON(WigetIndex, scope, compile, vm);
                // console.log(widgetJson);
                handleFrontEndForCloneWidget(compile, scope, vm, uiGridConstants, widgetJson, rootScope, WigetIndex);
            };
            // code end

            // code start
            vm.ApplyMapPinColorSetting = function (WigetIndex) {
                isPinColorApplyBtnClicked = true;
                vm.ChangeMapLevelType(WigetIndex);
            };
            // code end

            vm.ChangeMapLevelType = function (WigetIndex) {
                // code start
                vm.dashboardAndReportComInformation.currentSelectedColumn = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype;
                // code end
                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].data.length > 0) {
                    let GetQuery = '';
                    if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype == 'Country') {
                        if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere == '()') {
                            GetQuery = 'select TotalRecords, RecordValue, MapLatitude as Latitude, MapLongitude as Longitude, Country As Name, Country as Country from(Select (COUNT(*)) as TotalRecords, Country As RecordValue, Country from ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + ' Group by Country) b inner join AD_Definations on b.Country = AD_Definations.DefinationName where AD_Definations.DefinationTypeId = 3';
                        }
                        else {
                            GetQuery = 'select TotalRecords, RecordValue, MapLatitude as Latitude, MapLongitude as Longitude, Country As Name, Country as Country from(Select (COUNT(*)) as TotalRecords, Country As RecordValue, Country from ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + ' Where ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + ' Group by Country) b inner join AD_Definations on b.Country = AD_Definations.DefinationName where AD_Definations.DefinationTypeId = 3';
                        }
                        vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.MarketLevel = false;
                        LoadFinalDataForMap(compile, scope, vm, WigetIndex, GetQuery, true);
                    }
                    if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype == 'State') {
                        if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere == '()') {
                            GetQuery = "select TotalRecords, RecordValue, MapLatitude as Latitude, MapLongitude as Longitude, [State] as Name, [State] as [State] from (Select (COUNT(*)) as TotalRecords, ([State] + ', ' + Country) As RecordValue, [State] from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Group by [State], Country) b inner join AD_Definations on b.[State] = AD_Definations.DefinationName where AD_Definations.DefinationTypeId = 4";
                        }
                        else {
                            GetQuery = "select TotalRecords, RecordValue, MapLatitude as Latitude, MapLongitude as Longitude, [State] as Name, [State] as [State] from (Select (COUNT(*)) as TotalRecords, ([State] + ', ' + Country) As RecordValue, [State] from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Where " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + " Group by [State], Country) b inner join AD_Definations on b.[State] = AD_Definations.DefinationName where AD_Definations.DefinationTypeId = 4";
                        }
                        vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.MarketLevel = false;
                        LoadFinalDataForMap(compile, scope, vm, WigetIndex, GetQuery, true);
                    }
                    if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype == 'Region') {
                        if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere == '()') {
                            GetQuery = "select TotalRecords, RecordValue, MapLatitude as Latitude, MapLongitude as Longitude, Region as Name,  Region as Region from ( Select (COUNT(*)) as TotalRecords, (Region + ', ' + [State] + ', ' + Country) As RecordValue, RegionId, Region  from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Group by RegionId, Region, [State], Country) b Left outer join AD_Definations on b.RegionId = AD_Definations.DefinationId";
                        }
                        else {
                            GetQuery = "select TotalRecords, RecordValue, MapLatitude as Latitude, MapLongitude as Longitude, Region as Name, Region as Region from ( Select (COUNT(*)) as TotalRecords, (Region + ', ' + [State] + ', ' + Country) As RecordValue, RegionId, Region from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Where " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + " Group by RegionId, Region, [State], Country) b Left outer join AD_Definations on b.RegionId = AD_Definations.DefinationId";
                        }
                        vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.MarketLevel = false;
                        LoadFinalDataForMap(compile, scope, vm, WigetIndex, GetQuery, true);
                    }
                    if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.mapleveltype == 'Market') {
                        // code start
                        if (!isPinColorApplyBtnClicked) {
                            vm.dashboardAndReportComInformation.widgetList[WigetIndex].data = [];
                            if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere == '()') {
                                GetQuery = "Select (COUNT(*)) as TotalMarketRecords, Market As RecordValue, Country, (select count(*) from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + ") as TotalRecords from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " GROUP by Market, RegionId, State, Country";
                            }
                            else {
                                GetQuery = "Select (COUNT(*)) as TotalMarketRecords, Market As RecordValue, Country, (select count(*) from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Where " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + ") as TotalRecords from " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " Where " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + " GROUP by Market, RegionId, State, Country";
                            }

                            handleGetDataFromDbForMap(WigetIndex, GetQuery, vm, true).done(function (result) {
                                vm.dashboardAndReportComInformation.widgetList[WigetIndex].MapFilterCitiesList = [];
                                if (result.status) {
                                    if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].data[0].TotalRecords < 10) {
                                        GetQuery = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sqlQueryBuild;
                                        vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.MarketLevel = true;
                                        LoadFinalDataForMap(compile, scope, vm, WigetIndex, GetQuery, true);
                                    }
                                    else {
                                        //   vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.Country = '0';
                                        vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.MarketLevel = true;
                                        $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').html('');
                                        $('#chkmarketfilters').html('');
                                        var marketfilterdata = vm.dashboardAndReportComInformation.widgetList[WigetIndex].data;
                                        var ddmarketfiltercountries = $();

                                        $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').append('<option value="0">Please select</option>');
                                        $.each(marketfilterdata, function (i, item) {

                                            var $option = $('<option value=' + item.Country + '>' + item.Country + '</option>');
                                            //Add it to the bucket only if it's not already added!
                                            if (!ddmarketfiltercountries.filter(":contains(" + item.Country + ")").length) {
                                                ddmarketfiltercountries = ddmarketfiltercountries.add($option);
                                            }
                                            vm.dashboardAndReportComInformation.widgetList[WigetIndex].MapFilterCitiesList.push({ Value: item.RecordValue, NoOfRecords: item.TotalMarketRecords, Country: item.Country });
                                            $('#chkmarketfilters').append("<option ng-click='" + vm.CountMarketChkbox(' + WigetIndex + ') + "' value='" + item.RecordValue + "'>" + item.RecordValue + "</option>");
                                            //$('#chkmarketfilters').append('<input type="checkbox" ng-click="vm.CountMarketChkbox(' + WigetIndex + ')" class ="selectRow marketfilterchkbox" data-recordnumber="' + item.TotalMarketRecords + '" value="' + item.RecordValue + '"> ' + item.RecordValue + ' <small class="marketfilterchkboxsmall">(' + item.TotalMarketRecords + ')</small>' + '<br> ');
                                        });


                                        // compile($('#chkmarketfilters'))(scope);
                                        $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').append(ddmarketfiltercountries);
                                        $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').attr('ng-model', 'vm.dashboardAndReportComInformation.widgetList[' + WigetIndex + '].widgetConfiguration.Country');
                                        $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').attr('ng-change', 'vm.changemarketfiltercountry(' + WigetIndex + ')');
                                        compile($('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries'))(scope);
                                        $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').val(vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.Country);
                                        $('#btnsavemarketfiltermodal').unbind('click');
                                        $('#btncancelmarketfiltermodal').unbind('click');

                                        compile($('#btnsavemarketfiltermodal').attr('ng-click', 'vm.Savemarketfilterchkbox(' + WigetIndex + ')'))(scope);
                                        compile($('#btncancelmarketfiltermodal').attr('ng-click', 'vm.CancelMarketFilterModal(' + WigetIndex + ')'))(scope);
                                        vm.changemarketfiltercountry(WigetIndex);
                                        //  vm.sourceChangedForMapPinSetting(WigetIndex);
                                        vm.Savemarketfilterchkbox(WigetIndex, null);
                                        //GetQuery = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.sqlQueryBuild;
                                        //handleGetDataFromDbForMap(WigetIndex, GetQuery, vm, true).done(function (result) {

                                        //});
                                        $('#marketfiltermodal').modal('show');
                                    }
                                }
                            });
                        } else {
                            initMapDynamic(compile, scope, vm, WigetIndex);
                        }
                        // code end
                    }

                }
                isPinColorApplyBtnClicked = false;

            };


            vm.CancelMarketFilterModal = function (WigetIndex) {
                var result = confirm("Are you Sure You want to cancel?");
                if (result) {
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.Country = '0';

                    //$('#ddmarketfiltercountries').html('');
                    //$('#chkmarketfilters').html('');
                    //  initMapDynamic(compile, scope, vm, WigetIndex);
                    $('#marketfiltermodal').modal('hide');

                }


            };

            vm.CountMarketChkbox = function (WigetIndex) {
                var countcity = 0;
                var thisRow = $('#chkmarketfilters').find(".selectRow:checked");

                thisRow.each(function (idx, ele) {

                    var node = $(ele).context;

                    countcity = countcity + parseInt($(node).attr("data-recordnumber"));
                });

                $('#totalcounts').html(countcity);
            }

            vm.Savemarketfilterchkbox = function (WigetIndex, event) {
                let arr = [];
                var countcity = 0;
                var marketList = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.filterSelectedCities;
                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].IsAllCountriesOptionSelected == true) {
                    marketList = [];
                }

                var thisRow = marketList;
                var str = '';
                for (let x = 0; x < thisRow.length; x++) {
                    let getValueAndText = thisRow[x].split(':');
                    arr.push(getValueAndText[0]);
                    str += " Market = '" + getValueAndText[0] + "' OR ";
                    countcity = countcity + parseInt(getValueAndText[1]);
                }
                var selectStr = 'SELECT ';
                if (marketList.length == 0) {
                    selectStr = 'Select TOP (1000) ';
                }
                if (countcity >= 0 && countcity < 1000) {
                    str = str.substring(0, str.length - 3);

                    if (str != '') {
                        str = " AND (" + str + ") ";
                    }
                    let GetQuery = '';
                    if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere == '()') {
                        GetQuery = selectStr + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryColumns + ' from ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + ' Where 1 = 1 ' + str;
                        if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryGroupBy != "") {
                            GetQuery += ' Group By ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryGroupBy;
                        }
                        if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQuerySortBy != "") {
                            GetQuery += ' ORDER By ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQuerySortBy;
                        }

                    }
                    else {

                        GetQuery = selectStr + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryColumns + ' from ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[0] + " WHERE " + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryWhere + str;
                        if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryGroupBy != "") {
                            GetQuery += ' Group By ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQueryGroupBy;
                        }
                        if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQuerySortBy != "") {
                            GetQuery += ' ORDER By ' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.whereClauseConfiguration.mapsqlQuerySortBy;
                        }

                    }
                    LoadFinalDataForMap(compile, scope, vm, WigetIndex, GetQuery, true);
                }
                else {
                    toastrs.error("Please Select those Cities which has minimum 1000 records to draw the Map");
                    //alert("please select those cities who have minimum record for draw the map. Less than 1000");
                }


            };

            // code start
            vm.filterOnlyStringTypeColumn = function (item) {
                if (item.currentDataType === 'select' || item.currentDataType === 'nvarchar') {
                    return item;
                }
            }
            // code end

            // code start
            vm.filterSeperateAlreadyAddedColumn = function (item) {
                if (item.columnname.toLowerCase() != 'country' && item.columnname.toLowerCase() != 'latitude' && item.columnname.toLowerCase() != 'longitude' && item.columnname.toLowerCase() != 'market' && item.columnname.toLowerCase() != 'state' && item.columnname.toLowerCase() != 'region') {
                    return item;
                }
            };
            // code end

            vm.changemarketfiltercountry = function (WigetIndex) {
                var marketfilterdata = vm.dashboardAndReportComInformation.widgetList[WigetIndex].MapFilterCitiesList;

                if (vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.Country != '0') {
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].IsAllCountriesOptionSelected = false;
                    var markets = [];
                    var resultItems = marketfilterdata.filter(function (currentItem) {
                        return currentItem.Country == vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.Country;
                    })
                    $.each(resultItems, function (i, item) {
                        markets.push({ Value: item.Value, NoOfRecords: item.NoOfRecords, Country: item.Country });
                    });
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].MapFilterCitiesList = markets;
                }
                else {
                    vm.dashboardAndReportComInformation.widgetList[WigetIndex].IsAllCountriesOptionSelected = true;
                }
                setTimeout(function () {
                    $('.compact-multiselect').multiselect('rebuild');
                    // $('.multiselect-selected-text').text('choose market');
                }, 100);
                $('.' + vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetID + ' .ddmarketfiltercountries').val(vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.Country);
            };
            //#endregion

            //#region draw or render chart when change on x-axis select box
            vm.chartAxisChange = function (widgetIndex) {

                // handleGetChartAndCompanytype(widgetIndex, vm, vm.dashboardAndReportComInformation.widgetList[widgetIndex].tempData, toastrs);
                handleGetDataToGevenValueForCharts(vm, widgetIndex, scope, compile);
                hanldeCheckChartCompanyType(compile, scope, vm, widgetIndex);

            };
            //#endregion

            //#region refresh All widget when any change in whole dashboard
            vm.refreshDashboardReport = function (widgetIndex) {
                //$grid._resizeHandler();
            };
            //#endregion

            //#region Clone any column which are selected in where clause in query builder
            vm.copyselectColumn = function (event, widgetIndex) {
                var thisRow = compile($(event.target).parents('.tg-row').clone())(scope);
                var parsedRow = [];
                parsedRow.push(thisRow[0]);
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn.add(parsedRow);
                return false;
            };
            //#endregion

            //#region Clone X-axis in multi series
            vm.addMultiSeriesXaxis = function (widgetIndex) {
                let multiSeriesXaxisParams = { xAxisColumnNameMul: undefined };
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis.push(multiSeriesXaxisParams);

            };
            //#endregion

            //#region Clone Y-axis in multi series chart
            vm.addMultiSeriesYaxis = function (widgetIndex) {
                let multiSeriesYaxisParams = { yAxisColumnNameMul: undefined, yAxisChartType: undefined, yAxisColorPicker: getRandomColor(), yAxisSecPriType: "secondary", columnTitle: "" };
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis.push(multiSeriesYaxisParams);

            };
            //#endregion

            //#region Remove X-axis in multi series chart
            vm.removeXaxisMul = function (widgetIndex, xAxisIndexMul) {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.xAxis.splice(xAxisIndexMul, 1);
                vm.chartAxisChange(widgetIndex);
            };
            //#endregion

            //#region Remove Y-axis in multi series chart
            vm.removeYaxisMul = function (widgetIndex, yAxisIndexMul) {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].chartConfigurations.Advancesettings.multiSeriesWithGroupOrNot.yAxis.splice(yAxisIndexMul, 1);
                vm.chartAxisChange(widgetIndex);
            };
            //#endregion

            //#region reset and remove all X-axis except of first X-axis
            vm.resetXaxis = function (widgetIndex) {
                handleResetXaxisMul(vm, widgetIndex);

            };
            //#endregion

            //#region reset and remove all Y-axis except of first Y-axis
            vm.resetYaxis = function (widgetIndex) {
                handleResetYaxisMul(vm, widgetIndex);
            };
            //#endregion

            //#region only reset group in multi series chart
            vm.resetMulSeries = function (widgetIndex) {
                handleResetMultSeriesGroup(vm, widgetIndex);
                vm.chartAxisChange(widgetIndex);
            };
            //#endregion


            vm.multiSchemasDesigner.removeDataSet = function (item, model, widgetIndex) {

                handleRemoveViewAndItsColumns(vm, widgetIndex, scope, compile, item['ViewName']);
            };
            vm.multiSchemasDesigner.selectDataSet = function (item, model, widgetIndex) {



                if (!vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueryStatus) {


                    handleTreeviewOfViewColumnBelowQueryBuilderSection(vm, widgetIndex, scope, compile, item['ViewName'], true);
                    if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentMultiSelectedViews.length == 1) {
                        handleInitializationOfWhereClauseWhenSelectaDataSet(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.whereClauseConfiguration.whereClauseJsonWithRuleAndGroup, scope, compile, rootScope, vm, widgetIndex);
                    }
                   // console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels);
                }
                else {
                    let whereClausejson = JSON.parse(' { "group": { "operator": "AND", "rules": [] } }');
                    if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId]) {
                        whereClausejson = vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId].whereClauseJsonWithRuleAndGroup
                    }
                    if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentMultiSelectedViews.length == 1) {
                        handleInitializationOfWhereClauseWhenSelectaDataSet(whereClausejson, scope, compile, rootScope, vm, widgetIndex);
                    }

                    handleTreeviewOfViewColumnBelowQueryBuilderSection(vm, widgetIndex, scope, compile, item['ViewName'], true);
                  //  console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels);

                }
            };

            //#region open the modal for multiple join
            vm.openModalForMultipleJoins = function (widgetIndex) {
                handleAddMultipleJoinAndDrawMultipleEdges(vm, widgetIndex, scope, compile);
                // console.log(widgetIndex);
            };
            //#endregion

            //#region comes change in schema designer when click on checkbox of query builder schema designer
            vm.schmaDesignerNodeChange = function (ivhNode, ivhIsSelected, ivhTree, widgetIndex) {
                if (ivhIsSelected) {

                    handleSchemaDesignerSaveViewAndItsColumnsName(vm, widgetIndex, ivhNode, scope, compile);
                }
                else {

                    handleDeleteViewAndItsColumnsNameForSchemaDesigner(vm, widgetIndex, ivhNode, scope, compile);
                }



            };
            //#endregion

            vm.getXml = function (widgetIndex) {

                //console.log(handleGetXmlMxGraphFromModel(vm, widgetIndex, scope, compile));
                //console.log(mxUtils.getXml(handleGetXmlMxGraphFromModel(vm, widgetIndex, scope, compile), this.linefeed));
                //for (var key in vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().cells) {

                //    console.log(vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getEdges(vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().cells[key]));
                //}
                //vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().beginUpdate();

                //vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.getModel().endUpdate();
                //vm.dashboardAndReportComInformation.widgetList[widgetIndex].mxGraph.refresh();
                maintainJsonForJoin(vm, widgetIndex, scope, compile);
                handleMakeJoinFromJoinDetailsObject(vm, widgetIndex, scope, compile);

            };

            jQuery.get('assets/js/fontawesome-5.json', function (data) {
                let tempArr = [];
                $.each(Object.keys(data), function (i, e) {
                    tempArr.push({ key: e, value: data[e] });
                });
                vm.tabFontawesomeIcon = tempArr;
            });


            vm.addNewTab = function (widgetIndex) {
                let tabItem = { icon: undefined, title: "", renderSectionID: vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID + "-" + handleUniquekey(6) };
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].tabConfiguration.tabHistoryWithTitleIcon.push(tabItem);
                $("#" + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find("." + tabItem.renderSectionID + "").gridstack({
                    float: false,
                    cellHeight: 10,
                    animate: true,
                    width: 12,
                    verticalMargin: 8,
                    draggable: {
                        handle: '.title-bar',
                    }
                });

            };
            handleDocumentPath(vm, scope, compile);
            //#region change the document type
            vm.changeDocumentType = function (ivhNode, ivhIsSelected, ivhTree, widgetIndex) {
                if (ivhIsSelected) {
                    console.log(ivhNode.value);
                    handleGetDocuments(ivhNode.value, $('#' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetID).find('.document-area'));
                }


                //$timeout(function () { handleGetDataToGivenValueForAllWidgets(vm, widgetIndex, scope, compile); }, 100); 

            };
            //#endregion

            vm.changeColumnReq = function (widgetIndex) {

            };


            vm.displayDataRowsColumnsForm = function (widgetIndex) {

                handleDrawDataRowsColumnsFormWithDefaultPage(vm, widgetIndex, scope, compile);

            };
            vm.addSubQuery = function (widgetIndex) {

                handleAddSubQuery(vm, widgetIndex, scope, compile, rootScope);
                handleExecuteQueryForMultiDataset(vm, widgetIndex, compile, scope, uiGridConstants, toastrs, false);
                vm.DeleteQBAllRows(widgetIndex);
            };
            vm.applySubQuery = function (widgetIndex) {
                handleInitializationForSubquery(vm, widgetIndex, scope, rootScope, compile, vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId);
                let columsNameList = [];


                let columnarr = [];
                let querycolumns = '';
                let querygroupby = '';
                let querysortyby = '';
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId].whereClauseJsonWithRuleAndGroup = scope.updateJson;
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId].whereClause = scope.output;
                let getAllItems = vm.dashboardAndReportComInformation.widgetList[widgetIndex].murriInitializationOfSelectedColumn.getItems();
                $(getAllItems).each(function (indx, value) {
                    var columnAliasname = '';
                    var duplicate = false;
                    var columnName = $(this._element).find(".tbl-columnname").val();
                    var aliasName = $(this._element).find(".tbl-aliasname").val();
                    var currentDataType = $(this._element).find(".db-column-data-type").attr('current-data-type');
                    var originalDataType = $(this._element).find(".db-column-data-type").attr('orginal-data-type');
                    if (aliasName == "") {
                        columnAliasname = columnName;

                    }
                    else {
                        columnAliasname = aliasName;
                    }
                    let aggregateFunction = $(this._element).find(".tbl-function").val();
                    let groupBy = $(this._element).find(".tbl-group").prop("checked");
                    let sortBy = $(this._element).find(".tbl-sort").val();
                    let columnSplitWithDot = columnName.split('.');
                    let viewName = columnSplitWithDot[0];
                    let columnNameWithoutViewName = columnSplitWithDot[1];
                    columnName = columnSplitWithDot[0] + ".[" + columnSplitWithDot[1] + "]";
                    if (columnSplitWithDot[1] == "Latitude" || columnSplitWithDot[1] == "Longitude") {
                        columnAliasname = columnSplitWithDot[1];
                    }
                    if (columnAliasname.indexOf('.') > -1) {
                        let columnAliasSplitBaseOnDot = columnAliasname.split('.');
                        //columnAliasname = columnAliasSplitBaseOnDot[0] + "_" + columnAliasSplitBaseOnDot[1];
                        columnAliasname = columnAliasSplitBaseOnDot[1];

                    }
                    if (columsNameList.findIndex(x=>x == columnAliasname) > -1) {
                        duplicate = true;
                        if (aggregateFunction != "-1") {
                            duplicate = false;
                            let getColumnsDup = columnarr.filter(x=>x.columnaliasname == columnAliasname && x.duplicate == false);
                            if (getColumnsDup) {

                                for (let idx = 0; idx < getColumnsDup.length; idx++) {
                                    getColumnsDup[idx].duplicate = true;

                                }
                            }
                        }
                    }
                    columnarr.push({
                        columnname: columnName,
                        aliasname: aliasName,
                        columnaliasname: columnAliasname,
                        aggregatefunction: aggregateFunction,
                        groupby: groupBy,
                        sortby: sortBy,
                        currentDataType: currentDataType,
                        originalDataType: originalDataType,
                        viewName: viewName,
                        columnNameWithoutViewName: columnNameWithoutViewName,
                        duplicate: duplicate


                    });
                });


                for (var i = 0; i < columnarr.length; i++) {
                    if (columnarr[i]["duplicate"] == false) {
                        if (columnarr[i]["aggregatefunction"] != "-1") {
                            if (columnarr[i]["aggregatefunction"] == "week") {
                                querycolumns += ' DatePart(' + columnarr[i]["aggregatefunction"] + ',' + columnarr[i]["columnname"] + ') AS [' + columnarr[i]["columnaliasname"] + '],';
                            }
                            else {
                                querycolumns += ' ' + columnarr[i]["aggregatefunction"] + '(' + columnarr[i]["columnname"] + ') AS [' + columnarr[i]["columnaliasname"] + '],';
                            }
                        }
                        else {
                            querycolumns += ' (' + columnarr[i]["columnname"] + ') AS [' + columnarr[i]["columnaliasname"] + '],';
                        }
                    }
                    if (columnarr[i]["groupby"] == true) {
                        querygroupby += ' ' + columnarr[i]["columnname"] + ',';
                    }

                    if (columnarr[i]["sortby"] != '-1') {
                        querysortyby += ' ' + '[' + columnarr[i]["columnaliasname"] + ']' + ' ' + columnarr[i]["sortby"] + ',';
                    }

                }

                //  var dataset = vm.dashboardAndReportComInformation.widgetList[WigetIndex].widgetConfiguration.selectedViewName;
                querycolumns = querycolumns.substring(0, querycolumns.length - 1);
                let finalquery = '';
                finalquery = 'Select' + querycolumns + ' from ' + handleSelectTablesNameWhenDoNotSelectAtleastOneJoin(vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentMultiSelectedViews);


                if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId].whereClause != '' && vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId].whereClause != '()') {
                    finalquery += ' WHERE ' + vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId].whereClause;

                }
                if (querygroupby != '') {
                    querygroupby = querygroupby.substring(0, querygroupby.length - 1);
                    finalquery += ' GROUP BY ' + querygroupby;
                }

                if (querysortyby != '') {
                    querysortyby = querysortyby.substring(0, querysortyby.length - 1);
                    finalquery += ' ORDER BY ' + querysortyby;
                }

                handleHistoryOfQueryBuilderForSubquery(vm, widgetIndex, columnarr);

                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId].sqlQueryBuild = finalquery;

                console.log(finalquery);

                //handleCheckWidgetType(compile, scope, vm, uiGridConstants, WigetIndex, toastrs);
            };

            vm.backSubQuery = function (widgetIndex) {
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueryStatus = false;

                handleRenderDatasetAnditsColumnsMainQuery(vm, widgetIndex, scope, compile, vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId);
                handleDeleteAllColumnWhenAddSubquery(vm, widgetIndex);
                handleRenderSelectedColumnUsingJson(vm, widgetIndex, compile, scope, vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.historyOfQueryBuilder);

                handleRenderOnlySubqueryColumnWithMainSelectedColumn(vm, widgetIndex, scope, compile);
                vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId = undefined;
            };
            vm.openAndRenderSubQuery = function (widgetIndex, UniqueKey) {
                handleDeleteAllColumnWhenAddSubquery(vm, widgetIndex);
                if (vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueryStatus) {
                    handleAssignValueToSubQuery(vm, widgetIndex, scope, compile, UniqueKey);
                    reRenderDataSetAndItsColumnWithQueryAndSubquery(vm, widgetIndex, scope, compile, UniqueKey);
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId = UniqueKey;
                    handleRenderSelectedColumnUsingJson(vm, widgetIndex, compile, scope, vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[UniqueKey].historyOfQueryBuilder);

                }
                else {
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueryStatus = true;
                    vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.currentSubqueryUniqueId = UniqueKey;
                    handleAssignValueToMainQueriesVariables(vm, widgetIndex, scope, compile, UniqueKey, rootScope);
                    reRenderDataSetAndItsColumnWithQueryAndSubquery(vm, widgetIndex, scope, compile, UniqueKey);
                    handleRenderSelectedColumnUsingJson(vm, widgetIndex, compile, scope, vm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.subQueriesList[UniqueKey].historyOfQueryBuilder);

                }
            };

            scope.renderQueryBuilderForETL = function (NodeId) {

                getViewsOfDb(vm);
                $.ajax({
                    url: '/BusinessIntelligence/DashboardNReport/GetNodeByNodeId',
                    method: 'POST',
                    //contentType: 'application/json; charset=utf-8',
                    //dataType:"json",
                    data: { NodeId: NodeId },
                    success: function (res, status, xhr) {

                        let NodeData = res.data[0];
                      //  console.log(NodeData);
                        $(".side-panel #queryBuilderSection").empty();
                        //sss  console.log(itemtable.rows().data()[item]);
                        // vm.dashboardAndReportComInformation = {};
                        vm.dashboardAndReportComInformation.templateTitle = "";
                        vm.dashboardAndReportComInformation.templateType = "";
                        vm.dashboardAndReportComInformation.TemplateID = "";
                        vm.dashboardAndReportComInformation.save = true;
                        vm.dashboardAndReportComInformation.IsActive = false;
                        vm.dashboardAndReportComInformation.pageID = NodeData['pageID'];
                        vm.dashboardAndReportComInformation.publicTmp = NodeData['PublicTmp'];
                        vm.dashboardAndReportComInformation.widgetList = [];
                        vm.dashboardAndReportComInformation.templateTitle = NodeData['templateTitle'];
                        vm.dashboardAndReportComInformation.templateType = NodeData['templateType'];
                        vm.dashboardAndReportComInformation.TemplateID = NodeData['TemplateID'];
                        let schulderFromDb = {};
                        schulderFromDb['DayFreqCusEndTime'] = NodeData['DayFreqCusEndTime'];
                        schulderFromDb['DayFreqCusStartTme'] = NodeData['DayFreqCusStartTme'];
                        schulderFromDb['DayFreqDayCriteria'] = NodeData['DayFreqDayCriteria'];
                        schulderFromDb['DayFreqIntervalTime'] = NodeData['DayFreqIntervalTime'];
                        schulderFromDb['Dayfreqcusinterval'] = NodeData['Dayfreqcusinterval'];
                        schulderFromDb['Dayfreqcusintervaltype'] = NodeData['Dayfreqcusintervaltype'];
                        schulderFromDb['DurEndDate'] = NodeData['DurEndDate'];
                        schulderFromDb['DurStartDate'] = NodeData['DurStartDate'];
                        schulderFromDb['DurisContinous'] = NodeData['DurisContinous'];
                        schulderFromDb['OnceDate'] = NodeData['OnceDate'];
                        schulderFromDb['OnceTime'] = NodeData['OnceTime'];
                        schulderFromDb['RecFreqDayInterval'] = NodeData['RecFreqDayInterval'];
                        schulderFromDb['RecFreqInterval'] = NodeData['RecFreqInterval'];
                        schulderFromDb['RecFreqIntervalType'] = NodeData['RecFreqIntervalType'];
                        schulderFromDb['RecFreqMonthCriteria'] = NodeData['RecFreqMonthCriteria'];
                        schulderFromDb['RecFreqMonthDayInterval'] = NodeData['RecFreqMonthDayInterval'];
                        schulderFromDb['RecFreqMonthDayMonthInterval'] = NodeData['RecFreqMonthDayMonthInterval'];
                        schulderFromDb['RecFreqMonthMonthInterval'] = NodeData['RecFreqMonthMonthInterval'];
                        schulderFromDb['Recfreqweekinterval'] = NodeData['Recfreqweekinterval'];
                        schulderFromDb['description'] = NodeData['description'];
                        schulderFromDb['UniqueKey'] = NodeData['UniqueKey'];
                        schulderFromDb['isPublic'] = NodeData['isPublic'];
                        schulderFromDb['scheduletype'] = NodeData['scheduletype'];
                        schulderFromDb['templateTitle'] = NodeData['templateTitle'];
                        schulderFromDb['templateType'] = NodeData['templateType'];
                        schulderFromDb['JobID'] = NodeData['JobID'];
                        schulderFromDb['Query'] = NodeData['Query'];
                        vm.dashboardAndReportComInformation.scheduler = $.extend(true, {}, schulderFromDb);


                        let innerItem = {};
                        innerItem.murriInitializationOfSelectedColumn = [];
                        innerItem.SelectedTablesColumnName = [];
                        innerItem.tempData = [];
                        innerItem.treeviewOfMultiDatasetsWithItsColumns = [];
                        innerItem.TemplateNodeID = NodeData['TemplateNodeID'];
                        innerItem.mxKeyHandler;
                        innerItem.mxModel;
                        innerItem.mxGraph;
                        innerItem.PWidgetID = NodeData['PWidgetID'];
                        innerItem.mxEdgesStyle;
                        innerItem.widgetID = NodeData['widgetID'];
                        innerItem.widgetName = NodeData['widgetName'];
                        innerItem.isDeleted = NodeData['isDeleted'];
                        innerItem.widgetType = NodeData['widgetType'];
                        innerItem.uiGridConfiguration = JSON.parse(NodeData['widgetItemConfiguration']);
                        innerItem.widgetConfiguration = JSON.parse(NodeData['widgetConfiguration']);
                        innerItem.data = [];
                        vm.dashboardAndReportComInformation.widgetList.push(innerItem);

                        let settingsAndAdvanceSettingsSelector = $(".side-panel .tab-content .tab-pane > .toggle");
                        let settingsElement = getWidgetSettings.find(x => x.type == vm.dashboardAndReportComInformation.widgetList[0].widgetType).settings;
                        let advanceSettingsElement = getWidgetSettings.find(x => x.type == vm.dashboardAndReportComInformation.widgetList[0].widgetType).advanced_settings;
                        let element = getWidgetSettings.find(x => x.type == vm.dashboardAndReportComInformation.widgetList[0].widgetType).code;
                        var queryBuilerEle = getWidgetSettings.find(x => x.type == "queryBuilder").code;

                        handleRenderWidget(element, addWidgetOfGridStackItem, settingsAndAdvanceSettingsSelector, settingsElement, advanceSettingsElement, queryBuilerEle, $(".side-panel #queryBuilderSection"), compile, scope, vm, 0, vm.dashboardAndReportComInformation.widgetList[0].widgetType, uiGridConstants, rootScope, true);
                        handleInitializationOfWhereClauseWhenSelectaDataSet(vm.dashboardAndReportComInformation.widgetList[0].widgetConfiguration.whereClauseConfiguration.whereClauseJsonWithRuleAndGroup, scope, compile, null, vm, 0);
                        handleAddSomeAdditionalClassesForLeftPanel();
                        if (vm.dashboardAndReportComInformation.publicTmp) {
                            scope.$emit('setPublicTemplateFromDashboardReportE', true);
                        }
                        else {
                            scope.$emit('setPublicTemplateFromDashboardReportE', false);

                        }
                        scope.$emit('setEtlJobParamsForEditE', NodeData);

                    },
                    error: function () { }
                });
                //for (var item in itemtable.rows().data()) {
                //    if (!isNaN(item)) {
                //        if (typeof itemtable.rows().data()[item]['TemplateNodeID'] != "undefined") {
                //            if (itemtable.rows().data()[item]['TemplateNodeID'] == NodeId) {

                //            }

                //        }
                //    }

                //}

            }
            scope.deleteETL = function (NodeId) {
                //console.log(NodeId)
                  $.confirm({
                    title: 'Confirm!',
                    content: 'Do you want to delete this dataset?',

                    buttons: {
                        yes: {
                            btnClass: 'btn-blue',
                            keys: ['enter', 'shift'],
                            action: function () {
                                DeleteDataSet(NodeId);
                                itemtable.ajax.reload();
                            }
                        },
                        no: function () {
                           // $.alert('Canceled!');
                           // toastrs.error("Canceled!")
                        }
                    }
                });

            }

        };
        //#endregion
        //#regin for delete data set
        function DeleteDataSet(NodeId) {
            $.ajax({
                url: '/BusinessIntelligence/DashboardNReport/DeleteDataSet',
                dataType: 'json',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ NodeId: NodeId }),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    if (data.status) {
                        toastrs.success(' Data deleted successfully');
                    }

                },
                error: function (jqXhr, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            });
        }
        //# end region

        //#region handleDrawChildWidgetsIfExist

        var handleDrawChildWidgetsIfExist = function (compiledcode, vm, Index, $compile, $scope, uiGridConstants, rootScope) {

            //console.log(angular.element("[ng-app='Airview.Xcelerate']").scope().iconsclass);
            var TabElement = $(compiledcode).find('.can-add').find('.nav');
            var tab_Id = '';
            var TabsInfo = vm.dashboardAndReportComInformation.widgetList[Index].widgetConfiguration.TabsInfo;
            var newtab = '';
            var tabContent = '';
            tabContent = ``;
            if (TabsInfo.length > 0) {
                $.each(TabsInfo, function (i, obj) { //show active
                    var newTab = '';
                    tab_Id = i + '_' + $(compiledcode).attr('id');
                    console.log(tab_Id);
                    //var FAicon = jQuery.trim(obj.Icon.replace('font-icon', ''));
                    //console.log(FAicon);
                    if (i == 0) {
                        newTab = `<li><select style="display:none"  class="icon-input" ng-model="selectedicon_` + tab_Id + `" ng-init="selectedicon_` + tab_Id + ` = '` + obj.Icon + `'"> <option ng-repeat="icon in iconsclass" value="{{icon.key}}">{{icon.key}}</option> </select> <input style="display:none" class="newtab_input" type="text" placeholder="Tab Name" ng-model="tab_` + tab_Id + `" ng-init="tab_` + tab_Id + ` = '` + obj.Title + `'" /> <a style="display:block" class="nav-link active" data-toggle="tab" data-target="#tab_target_` + tab_Id + `"> <i class="` + obj.Icon + `"></i><span>{{ tab_` + tab_Id + `}}</span></a> <span style="display:none"  class="done-tab custom-btn icon-only"><i class="fa fa-check"></i></span> <span style="display:none"  class="delete-tab custom-btn icon-only"><i class="fa fa-trash"></i></span> </li>`;
                    } else {
                        newTab = `<li><select style="display:none"  class="icon-input" ng-model="selectedicon_` + tab_Id + `" ng-init="selectedicon_` + tab_Id + ` = '` + obj.Icon + `'"> <option ng-repeat="icon in iconsclass" value="{{icon.key}}">{{icon.key}}</option> </select> <input style="display:none" class="newtab_input" type="text" placeholder="Tab Name" ng-model="tab_` + tab_Id + `" ng-init="tab_` + tab_Id + ` = '` + obj.Title + `'" /> <a style="display:block" class="nav-link" data-toggle="tab" data-target="#tab_target_` + tab_Id + `"> <i class="` + obj.Icon + `"></i><span>{{ tab_` + tab_Id + `}}</span></a> <span style="display:none"  class="done-tab custom-btn icon-only"><i class="fa fa-check"></i></span> <span style="display:none"  class="delete-tab custom-btn icon-only"><i class="fa fa-trash"></i></span> </li>`;
                    }

                    var ngCompile = $.parseHTML(newTab);
                    ngCompile = $compile(ngCompile)($scope);
                    $(TabElement).append(ngCompile);
                    if (i == 0) {
                        tabContent = `<div class="tab-pane fade show active tab-canvas" id="tab_target_` + tab_Id + `"> <div class="center-icon"><i class="fab fa-ioxhost"></i></div>  <div class="grid-stack"> </div> </i></div>`;
                    } else {
                        tabContent = `<div class="tab-pane fade tab-canvas" id="tab_target_` + tab_Id + `"> <div class="center-icon"><i class="fab fa-ioxhost"></i></div>  <div class="grid-stack"> </div> </i></div>`;
                    }
                    $(compiledcode).find('.tab-content').append(tabContent);
                    let height = $("#tab_target_" + tab_Id).parents('.content-wrapper').height();
                    $("#tab_target_" + tab_Id).find('.grid-stack').height(height - 70);
                    $("#tab_target_" + tab_Id).find(".grid-stack").gridstack({
                        float: false,
                        cellHeight: 10,
                        animate: true,
                        width: 12,
                        verticalMargin: 5,
                        draggable: {
                            handle: '.title-bar',
                        }
                    });

                    $.each(obj.WidgetList, function (indexInner, wID) {
                        var $loadpanel = getWidgetSettings;
                        var childWidgetInfo = vm.dashboardAndReportComInformation.widgetList.find(x => x.widgetID == wID);
                        var childWidgetIndexOf = vm.dashboardAndReportComInformation.widgetList.findIndex(x => x.widgetID == wID);
                        let settingsAndAdvanceSettingsSelector = $(".side-panel .tab-content .tab-pane > .toggle");
                        let settingsElement = $loadpanel.find(x => x.type == childWidgetInfo.widgetType).settings;
                        let advanceSettingsElement = $loadpanel.find(x => x.type == childWidgetInfo.widgetType).advanced_settings;
                        let element = $loadpanel.find(x => x.type == childWidgetInfo.widgetType).code;
                        var queryBuilerEle = $loadpanel.find(x => x.type == "queryBuilder").code;
                        if (!childWidgetInfo.isDeleted) {
                            $("#tab_target_" + tab_Id).find(".center-icon").hide();
                            handleRenderWidget(element, $("#tab_target_" + tab_Id).find(".grid-stack").data('gridstack'), settingsAndAdvanceSettingsSelector, settingsElement, advanceSettingsElement, queryBuilerEle, $(".side-panel #queryBuilderSection"), $compile, $scope, vm, childWidgetIndexOf, childWidgetInfo.widgetType, uiGridConstants, rootScope, true);
                        }
                    });
                });
            }
        };

        //#endregion

        return {
            addWidget: function (scope, compile, vm, toastrs) {
                addWidgerHandler(scope, compile, vm, toastrs);
            },
            dragWidget: function (element, parentElement, settingsAndAdvanceSettingsSelector, settingsElement, advanceSettingsElement, compile, scope, vm, widgetType, toastrs) {
                handleDragWidget(element, parentElement, settingsAndAdvanceSettingsSelector, settingsElement, advanceSettingsElement, compile, scope, vm, widgetType, toastrs);
            },
            uniquekey: function (length) {
                return handleUniquekey(length);
            },
            drawWidgets: function (compile, scope, vm, callback, uiGridConstants, rootScope) {
                handleDrawWidgets(compile, scope, vm, callback, uiGridConstants, rootScope);
            },
            uiGridSettingsAndInitialization: function (compile, scope, vm, uiGridConstants, WigetIndex) {
                handleUiGridSettingsAndInitialization(compile, scope, vm, uiGridConstants, WigetIndex);
            },
            genericTableInitialization: function (compile, scope, vm, WigetIndex) {
                handleGenericTableInitialization(compile, scope, vm, WigetIndex);
            },
            chartInitializationOfCanvas: function (compile, scope, vm, WigetIndex) { handleChartInitializationOfCanvas(compile, scope, vm, WigetIndex); },
            chartRenderOfCanvas: function (compile, scope, vm, WigetIndex) { handleChartRenderOfCanvas(compile, scope, vm, WigetIndex); },
            checkInitializationOrRenderOfCanvasChart: function (compile, scope, vm, WigetIndex) { handleCheckInitializationOrRenderOfCanvasChart(compile, scope, vm, WigetIndex); },
            checkChartCompanyType: function (compile, scope, vm, widgetIndex) { hanldeCheckChartCompanyType(compile, scope, vm, widgetIndex); },
            setChartTypeOfCanvas: function (compile, scope, vm, widgetIndex) { handleSetChartTypeOfCanvas(compile, scope, vm, widgetIndex); },
            checkChartType: function (compile, scope, vm, widgetIndex) { handleCheckChartType(compile, scope, vm, widgetIndex); },
            resizeAllWidget: function (compile, scope, vm, widgetIndex, uiGridConstants, $timeout) { handlereSizeAllWidget(compile, scope, vm, widgetIndex, uiGridConstants, $timeout); },
            chartInitializationOfHight: function (compile, scope, vm, widgetIndex) { handleChartInitializationOfHight(compile, scope, vm, widgetIndex); },
            JsonForFrontend: function (compile, scope, vm, uiGridConstants, DashboardAndReportJsonFromDb) { handleJsonForFrontend(compile, scope, vm, uiGridConstants, DashboardAndReportJsonFromDb); },
            initialization: function (compile, scope, vm, uiGridConstants, $timeout, toastrs, rootScope) {
                hanldeInitialization(compile, scope, vm, uiGridConstants, $timeout, toastrs, rootScope);
            }
        };
    }();
    return dashboardAndReportHandler;
});

//#endregion
