

airviewXcelerateApp.factory('TopSectionModalInstanceService', function ($http, $timeout, toastr) {

    var TopSectionModalInstanceServiceHandler = function () {
        var hanldeInitialization = function (vm, scope, compile, parentScope, PVm, uibModalInstance) {

            vm.dashbaordInfo = {};
            vm.dashbaordInfo.templateTitle = "";
            vm.dashbaordInfo.templateType = "";
            vm.templateTypeStatus = true;
            console.log(moduleInformation)
            if (moduleInformation.ModuleType != null) {
                vm.dashbaordInfo.templateType = moduleInformation.ModuleType;
                vm.templateTypeStatus = false;
            }
            vm.addDashboardOrReport = function () {
                
                $http({
                    method: 'POST',
                    url: '/BusinessIntelligence/DashboardNReport/CheckTemplateNameIsExistOrNot',
                    data: { userID: userIDByX1, templateName: vm.dashbaordInfo.templateTitle }
                }).then(function successCallback(response) {
                    if (response.data.status) {
                        if (response.data.temaplateStatus) {
                            toastrs.error(response.data.msg);
                        }
                        else {
                            parentScope.$emit('dashboardAndReportE', vm.dashbaordInfo);
                            toastr.success(vm.dashbaordInfo.templateTitle + ' has been created', vm.dashbaordInfo.templateType);
                            uibModalInstance.close();
                            PVm.open = true;
                            PVm.btnText = "Save";
                            PVm.isModeType = "edit";
                            isModeForTab = true;
                        }
                    }

                }, function errorCallback(response) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });




            };
            vm.cancel = function () {

                uibModalInstance.dismiss('cancel');

            };

            scope.$on("$stateChangeSuccess", function (event, next, current) {
                $json_obj2 = [{
                    "type": "alerts",
                    "widgetList": {
                        "0": {
                            "data_position": "fourth_alert", "style": "background : black; color : #FFF", "widgetname": "My Alert", "size": "col-xl-3 col-lg-12"
                        },
                        "1": {
                            "data_position": "second_alert", "style": "background : blue; color : #FFF", "widgetname": "My Alert 2", "size": "col-xl-3 col-lg-12"
                        },
                        "2": {
                            "data_position": "first_alert", "style": "background : red; color : #FFF", "widgetname": "My Alert 3", "size": "col-xl-3 col-lg-12"
                        },
                        "3": {
                            "data_position": "third_alert", "style": "background : black; color : #FFF", "widgetname": "My Alert 4", "size": "col-xl-3 col-lg-12"
                        }
                    }
                }];



                // xl_script.murri();
                console.log($json_obj);
            });
        };
        return {
            initialization: function (vm, scope, compile, parentScope, PVm, $uibModalInstance) {
                hanldeInitialization(vm, scope, compile, parentScope, PVm, $uibModalInstance);

            }
        };
    }();
    return TopSectionModalInstanceServiceHandler;
});