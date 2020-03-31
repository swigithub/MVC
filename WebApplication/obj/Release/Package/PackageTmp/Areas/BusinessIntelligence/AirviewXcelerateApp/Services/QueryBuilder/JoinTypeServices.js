

airviewXcelerateApp.factory('JoinTypeService', function ($http, $timeout, toastr) {

    var JoinTypeServicesHandler = function () {

        var handleMakeJsonForjoinTypeDetail = function (pvm, widgetIndex, JoinDetailIndex, JoinDetailInnerIndex, joinType, operatorType) {
            if (pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[JoinDetailIndex]) {

                pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[JoinDetailIndex].joinType = joinType;
            }
            if (pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[JoinDetailIndex][JoinDetailInnerIndex]) {
                pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[JoinDetailIndex][JoinDetailInnerIndex] = operatorType;
            }

        };
        var handleInitialization = function (pvm, vm, widgetIndex, JoinDetailIndex, JoinDetailInnerIndex, $uibModalInstance, $scope, $state, $compile, toastr, $rootScope, JoinTypeService) {

            //#region join type
            if (pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[JoinDetailIndex]) {
                vm.joinType = pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinTypeWithForeignKeyReference[JoinDetailIndex].joinType;
            }
            else {
                vm.joinType = "INNER";

            }
            //#endregion

            //#region operator type
            if (pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[JoinDetailIndex][JoinDetailInnerIndex]) {
                vm.operatorType = pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.joinOperatorType[JoinDetailIndex][JoinDetailInnerIndex];
            }
            else {
                vm.operatorType = "AND";
            }
            //#endregion

            //#region button events

            vm.saveJoin = function () {
                handleMakeJsonForjoinTypeDetail(pvm, widgetIndex, JoinDetailIndex, JoinDetailInnerIndex, vm.joinType, vm.operatorType);
                $uibModalInstance.dismiss('cancel');
            };
            vm.cancel = function () {

                $uibModalInstance.dismiss('cancel');

            };

            //#endregion

        };
        return {
            initialization: function (pvm, vm, widgetIndex, JoinDetailIndex, JoinDetailInnerIndex, $uibModalInstance, $scope, $state, $compile, toastr, $rootScope, JoinTypeService) {
                handleInitialization(pvm, vm, widgetIndex, JoinDetailIndex, JoinDetailInnerIndex, $uibModalInstance, $scope, $state, $compile, toastr, $rootScope, JoinTypeService);

            }
        };
    }();
    return JoinTypeServicesHandler;
});