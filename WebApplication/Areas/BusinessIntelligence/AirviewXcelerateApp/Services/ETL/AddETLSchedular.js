
//#region Factory of addETLSchedularModal
airviewXcelerateApp.factory('AddETLSchedular', function ($http, $timeout, toastr) {

    //#region handler

    var addETLSchedularHandler = function () {

       

        //#region Initialization
        var handleInitialization = function (pvm, vm, widgetIndex, uibModalInstance, scope, state, compile, toastr, rootScope, parentScope) {
          

            //#region Events

           

            //#region Save etl Schedular name
            vm.saveETLSchedular = function () {
                alert("yes")
                console.log(vm.dashbaordInfo.ETLTitle);
               // parentScope.$emit("addETLSchedularE", { addETLSchedularA: "yes", widgetIndex: 1 });
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
    return addETLSchedularHandler;

    //#endregion
});

//#endregion