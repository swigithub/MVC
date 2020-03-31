
//#region Factory of AddMultipleJoinModal
airviewXcelerateApp.factory('AddMultipleJoinModal', function ($http, $timeout, toastr) {

    //#region handler

    var addMultipleJoinServicesHandler = function () {

        //#region Return add new row for source, target and operator type .
        var handleAddSourceAndTargetRow = function () {

            let addNewRow = { operatorType: undefined, source: undefined, target: undefined };
            return addNewRow;
        };

        //#endregion

        //#region Initialization
        var handleInitialization = function (pvm, vm, widgetIndex, uibModalInstance, scope, state, compile, toastr, rootScope, parentScope) {

            //#region Variables
            vm.joinTypeJson = airviewXcelerateAppSettings.setJsonForJoinType();
            vm.sourceAndTargetTablesName = [];
            vm.sourceColumns = [];
            vm.targetColumns = [];
            vm.operators = [{ key: "AND", value: "AND" }, { key: "OR", value: "OR" }];
            vm.multipleJoinSourceTargetList = { joinType: undefined, source: undefined, target: undefined, listOfOperatorSourceAndTargetColumnsName: [] };
            vm.multipleJoinSourceTargetList.listOfOperatorSourceAndTargetColumnsName.push(handleAddSourceAndTargetRow());
            let getlenghtOfDataset = pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews.length;
            //#endregion

            //#region Events

            //#region Save source and target with columns name

            vm.saveMultipleJoin = function () {
                parentScope.$emit("multiJoinSourceAndTargetListE", { multipleJoinSourceTargetListA: vm.multipleJoinSourceTargetList, widgetIndex: widgetIndex });
                uibModalInstance.dismiss('cancel');
            };

            //#endregion

            //#region Change target
            vm.changeTargetEvent = function () {
                var getColumnsOfTarget = pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue.filter(x => x.key == vm.multipleJoinSourceTargetList.target);
                vm.targetColumns = getColumnsOfTarget;
            };

            //#endregion

            //#region Change source
            vm.changeSourceEvent = function () {
                var getColumnsOfSource = pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSchmaDesignerOnKeyValue.filter(x => x.key == vm.multipleJoinSourceTargetList.source);
                vm.sourceColumns = getColumnsOfSource;
            };
            //#endregion

            //#region Remove source and target

            vm.removeSourceAndTargetEvent = function (rowIndex) {
                vm.multipleJoinSourceTargetList.listOfOperatorSourceAndTargetColumnsName.splice(rowIndex, 1);
            };

            //#endregion

            //#region Add source and target

            vm.addSourceAndTargetEvent = function () {
                vm.multipleJoinSourceTargetList.listOfOperatorSourceAndTargetColumnsName.push(handleAddSourceAndTargetRow());
            };

            //#endregion

            //#region Get all Datasets Which are selected

            if (getlenghtOfDataset > 0) {
                for (var idx = 0; idx < getlenghtOfDataset; idx++) {
                    vm.sourceAndTargetTablesName.push({ key: pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[idx]['table'], value: pvm.dashboardAndReportComInformation.widgetList[widgetIndex].widgetConfiguration.multiSchemaDesignerModels.multiSelectedViews[idx]['table'] });
                }

            }

            //#endregion

            //#region cancel modal
            vm.cancel = function () {

                uibModalInstance.dismiss('cancel');

            };

            //#endregion

            //#endregion


        };

        //#endregion

        return {
            initialization: function (pvm, vm, widgetIndex, uibModalInstance, scope, state, compile, toastr, rootScope, parentScope) {
                handleInitialization(pvm, vm, widgetIndex, uibModalInstance, scope, state, compile, toastr, rootScope, parentScope);

            }
        };
    }();
    return addMultipleJoinServicesHandler;

    //#endregion
});

//#endregion