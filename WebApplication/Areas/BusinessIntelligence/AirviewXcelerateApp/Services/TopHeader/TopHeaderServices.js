

airviewXcelerateApp.factory('TopHeaderSerive', function ($http, $timeout, toastr) {

    var TopHeaderSerivcesHandler = function () {
        var hanldeInitialization = function (scope, compile, vm) {
            vm.showEMSTopheader = false;

            vm.entityInformation = {};
            vm.entityInformation['EntityCode'] = "-";
            vm.entityInformation['EntityName'] = "-";
            vm.entityInformation['EntityCategory'] = "-";
            vm.entityInformation['IsActive'] = "-";
            vm.entityInformation['Industry'] = "-";
            vm.entityInformation['Sector'] = "-";
            vm.entityInformation['Owner'] = "-";
           

            scope.$on('displayEntityTemplateNEntityOnLayoutB', (event, entityInformation) => {
                
                vm.entityInformation['EntityCode'] = entityInformation.EntityCode;
                vm.entityInformation['EntityName'] = entityInformation.EntityName;
                vm.entityInformation['EntityCategory'] = entityInformation.EntityCategory;
                vm.entityInformation['IsActive'] = entityInformation.IsActive;
                vm.entityInformation['Industry'] = entityInformation.Industry;
                vm.entityInformation['Sector'] = entityInformation.Sector;
                vm.entityInformation['Owner'] = entityInformation.Owner;
                scope.$apply();

            });
            scope.$on('enableTopHeaderB', (event,enableTopHedr) => {
                vm.showEMSTopheader = enableTopHedr;
            })
            vm.resizeAllWidget = function () {
                scope.$emit('ResizeAllWidgetE');

            };
        };
        return {
            initialization: function (scope, compile, vm) {
                hanldeInitialization(scope, compile, vm);

            }
        };
    }();
    return TopHeaderSerivcesHandler;
});