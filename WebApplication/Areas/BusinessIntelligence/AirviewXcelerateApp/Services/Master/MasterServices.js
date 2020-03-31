

airviewXcelerateApp.factory('MasterService', function ($http, $timeout, toastr) {

    var MasterServiceHandler = function () {

       
        //#region Initialization
        var handleInitialization = function (scope, compile, rootscope) {
            rootscope.charts = [];
            rootscope.resizeCharts = function () {
                console.log(rootscope.charts);
                rootscope.charts.forEach(function (item) {
                    item.render();
                });
            };
            //scope.iconsclass = [];
            //rootscope.simpleCompile = function (element, parentElement) {
            //    var compiledcode = $.parseHTML(element);
            //     compiledcode = compile(compiledcode)(rootscope);
            //    $(parentElement).append(compiledcode);
            //};

            xl_script.themeChange();
            scope.$on("initializationOfGridStack", function (event) {

                xl_script.initGridStack();
                //xl_script.murri();
                //xl_script.colorCookies();
                //xl_script.titleCookies();
                //xl_script.themeChange();
                //xl_script.updateJSON();

            });
            scope.$on("dashboardAndReportE", function (event, dashboardInfo) {

                scope.$broadcast('dashboardAndReportB', dashboardInfo);

            });

            scope.$on("LoadForTempE", function (event, dashboardInfo) {

                scope.$broadcast('LoadForTempB', dashboardInfo);

            });
            scope.$on("saveTemplateE", function (event) {

                scope.$broadcast('saveTemplateB');

            });
            scope.$on("refreshDropdownList", function (event) {
                scope.$broadcast('refreshDropdownListB');

            });
            
            scope.$on("changeModeTypeE", function (event,modeType) {

                scope.$broadcast('changeModeTypeB', modeType);

            });
            scope.$on("ResizeAllWidgetE", function (event) {

                scope.$broadcast('ResizeAllWidgetB');

            });
            scope.$on('addColumnInWhereClauseE', function (event, columnsList) {
                scope.$broadcast('addColumnInWhereClauseB', columnsList);
            });
            scope.$on('multiJoinSourceAndTargetListE', function (event, multiJoinList) {
                scope.$broadcast('multiJoinSourceAndTargetListB', multiJoinList);
            });
            scope.$on("changeModeTypeInTopHeaderE", function (event, modeType) {

                scope.$broadcast('changeModeTypeInTopHeaderB', modeType);

            });
            scope.$on("setDefaultTemplateFromTopHeaderE", function (event, setDefaultTemplate) {

                scope.$broadcast('setDefaultTemplateFromTopHeaderB', setDefaultTemplate);

            });
            scope.$on("setPublicTemplateFromTopHeaderE", function (event, setDefaultTemplate) {

                scope.$broadcast('setPublicTemplateFromTopHeaderB', setDefaultTemplate);

            });

            scope.$on("setDefaultTemplateFromDashboardReportE", function (event, setDefaultTemplate) {

                scope.$broadcast('setDefaultTemplateFromDashboardReportB', setDefaultTemplate);

            });
            scope.$on("setPublicTemplateFromDashboardReportE", function (event, setDefaultTemplate) {

                scope.$broadcast('setPublicTemplateFromDashboardReportB', setDefaultTemplate);

            });

            scope.$on("createEtlE", function (event, setDefaultTemplate) {

                scope.$broadcast('createEtlB', setDefaultTemplate);

            });
            scope.$on("updateEtlE", function (event, setDefaultTemplate) {

                scope.$broadcast('updateEtlB', setDefaultTemplate);

            });
            
            scope.$on("setEtlJobParamsForEditE", function (event, EtlJobParams) {

                scope.$broadcast('setEtlJobParamsForEditB', EtlJobParams);

            });
            
            scope.$on('postViewsNTablesNColumnsE', (event,dataSourceNViewsNTablesNColumns) => {
                scope.$broadcast('postViewsNTablesNColumnsB', dataSourceNViewsNTablesNColumns);

            })
            scope.$on('InOtherModuleTempNotaddE', function (event, OtherModulesParams) {
                scope.$broadcast('InOtherModuleTempNotaddB', OtherModulesParams);

            });
            scope.$on('changeShutterUpDownStatusE', (event,shutterUpDownStutas) => {
                scope.$broadcast('changeShutterUpDownStatusB', shutterUpDownStutas)
            })
            scope.$on('bindEntityTemplateIdOnAllWidgetE', (event, EntityInformation) => {
                scope.$broadcast('bindEntityTemplateIdOnAllWidgetB', EntityInformation)
            })
            scope.$on('displayEntityTemplateNEntityOnLayoutE', (event, EntityInformation) => {
                scope.$broadcast('displayEntityTemplateNEntityOnLayoutB', EntityInformation)
            })
            
            scope.$on('enableTopHeaderE', (event, enableTHedr) => {
                scope.$broadcast('enableTopHeaderB', enableTHedr)
            })

            scope.$on('postViewsNTablesNColumnsRPTE', (event, dataSourceRPT) => {
                scope.$broadcast('postViewsNTablesNColumnsRPTB', dataSourceRPT)
            })
            
        };

        //#endregion


        return {
            initialization: function (scope, compile, rootScope) {
                handleInitialization( scope, compile, rootScope);

            }
        };
    }();
    return MasterServiceHandler;
});