
//#region Factory of AddETLNameModal
airviewXcelerateApp.factory('AddETLNameModal', function ($http, $timeout, toastr) {

    //#region handler

    var addETLNameHandler = function () {

       

        //#region Initialization
        var handleInitialization = function (pvm, vm, widgetIndex, uibModalInstance, scope, state, compile, toastr, rootScope, parentScope) {
          

            //#region Events

           

            //#region Save etl (transaction table) name
            vm.saveETLName = function () {
                console.log(vm.dashbaordInfo.ETLTitle);
                parentScope.$emit("addETLNameE", { addETLNameA: vm.dashbaordInfo.ETLTitle, widgetIndex: widgetIndex });
                uibModalInstance.dismiss('cancel');
            };

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
    return addETLNameHandler;

    //#endregion
});

//#endregion