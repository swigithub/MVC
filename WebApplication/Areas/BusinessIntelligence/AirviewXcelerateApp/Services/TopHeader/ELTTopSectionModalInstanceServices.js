

airviewXcelerateApp.factory('ETLTopSectionModalInstanceService', function ($http, $timeout, toastr) {

    var TopSectionModalInstanceServiceHandler = function () {
        var hanldeInitialization = function (vm, scope, compile, parentScope, PVm, uibModalInstance) {
            vm.dashbaordInfo = {};
            vm.dashbaordInfo.templateTitle = "";
            vm.dashbaordInfo.templateType = "ETL";
            vm.dashbaordInfo.weekDay = ['MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT', 'SUN'];
            vm.dashbaordInfo.RecFreqMonthCriteria = "Once";
            vm.dashbaordInfo.DayFreqDayCriteria = "Once";
            vm.dashbaordInfo['JobID'] = null;
            vm.dashbaordInfo.DurisContinous = true;
            vm.dashbaordInfo.isPublic = true;
            vm.dashbaordInfo.jobEnable = true;
            vm.dashbaordInfo.RecFreqInterval = 1;
            vm.dashbaordInfo.Dayfreqcusinterval = 1;
            vm.dashbaordInfo.RecFreqDayInterval = 1;
            vm.dashbaordInfo.RecFreqMonthDayInterval = 1;
            vm.dashbaordInfo.RecFreqMonthMonthInterval = 1;
            vm.dashbaordInfo.RecFreqMonthDayMonthInterval = 1;
            vm.addETL = function () {
                vm.dashbaordInfo.OnceDate = $("#once_date").val();
                vm.dashbaordInfo.OnceTime = $("#once_time").val();
                vm.dashbaordInfo.DayFreqCusStartTme = $("#df-start-time").val();
                vm.dashbaordInfo.DayFreqCusEndTime = $("#df-end-time").val();
                vm.dashbaordInfo.DurStartDate = $("#ds-date").val();
                //added by sohail for check
                var durStartDate = $("#ds-date").val();
                var durEndDate = $("#de-date").val();
                vm.dashbaordInfo.SummaryDescription = $('#sum_description').val();
                vm.dashbaordInfo.DayFreqIntervalTime = $('#occur-once-time').val()
                vm.dashbaordInfo.DurEndDate = $('#de-date').val();
                vm.dashbaordInfo.weekDays = vm.dashbaordInfo.weekDay.filter(x=>x != '' && x != 'false').join(',');
                if (vm.dashbaordInfo.DurisContinous == 'false') {
                    if ((moment(durStartDate) >= moment(durEndDate)) && ($("#Scheduletype").val() != 'OneTime')) {
                        toastrs.error("Duration end date must greater than start Date. ");
                        return
                    }
                }
                if (vm.dashbaordInfo.DayFreqDayCriteria == 'Custom') {
                    if ($('#df-start-time').val() > $('#df-end-time').val()) {
                        toastrs.error("Ending time must greater or equal than start time. ");
                        return
                    }
                }
                 

                if (PVm.isEtlModeType == 'edit') {
                    parentScope.$emit('updateEtlE', vm.dashbaordInfo);
                    createOrUpdateEtl(vm);
                }

                    //comment by sohail
                    //else
                    //    parentScope.$emit('createEtlE', vm.dashbaordInfo);
                    //toastr.success(vm.dashbaordInfo.templateTitle + ' has been created', vm.dashbaordInfo.templateType);
                    //uibModalInstance.close();
                    //PVm.isEtlModeType = 'new';

                    //end comment by sohail


                    // updated by sohail to check if etl dataset already exist or not 16 sep 2019
                else {
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
                                parentScope.$emit('createEtlE', vm.dashbaordInfo);
                                createOrUpdateEtl(vm);
                            }
                        }

                    }, function errorCallback(response) {
                        // called asynchronously if an error occurs
                        // or server returns response with an error status.
                    });

                }

            };
            vm.cancel = function () {
                PVm.isEtlModeType = "";
                uibModalInstance.dismiss('cancel');

            };
            function createOrUpdateEtl(vm) {
                toastr.success(vm.dashbaordInfo.templateTitle + ' has been created', vm.dashbaordInfo.templateType);
                uibModalInstance.close();
                PVm.isEtlModeType = 'new';
            }

            if (PVm.isEtlModeType == 'edit') {
                vm.dashbaordInfo = {};
                vm.dashbaordInfo['DayFreqCusEndTime'] = PVm.getEtljobdataFromMainPage['DayFreqCusEndTime'];
                vm.dashbaordInfo['DayFreqCusStartTme'] = PVm.getEtljobdataFromMainPage['DayFreqCusStartTme'];
                vm.dashbaordInfo['DayFreqDayCriteria'] = PVm.getEtljobdataFromMainPage['DayFreqDayCriteria'];
                vm.dashbaordInfo['DayFreqIntervalTime'] = PVm.getEtljobdataFromMainPage['DayFreqIntervalTime'];
                vm.dashbaordInfo['Dayfreqcusinterval'] = PVm.getEtljobdataFromMainPage['Dayfreqcusinterval'];
                vm.dashbaordInfo['Dayfreqcusintervaltype'] = PVm.getEtljobdataFromMainPage['Dayfreqcusintervaltype'];
                vm.dashbaordInfo['DurEndDate'] = PVm.getEtljobdataFromMainPage['DurEndDate'];
                vm.dashbaordInfo['DurStartDate'] = PVm.getEtljobdataFromMainPage['DurStartDate'];
                vm.dashbaordInfo['DurisContinous'] = PVm.getEtljobdataFromMainPage['DurisContinous'];
                vm.dashbaordInfo['OnceDate'] = PVm.getEtljobdataFromMainPage['OnceDate'];
                vm.dashbaordInfo['OnceTime'] = PVm.getEtljobdataFromMainPage['OnceTime'];
                vm.dashbaordInfo['RecFreqDayInterval'] = PVm.getEtljobdataFromMainPage['RecFreqDayInterval'];
                vm.dashbaordInfo['RecFreqInterval'] = PVm.getEtljobdataFromMainPage['RecFreqInterval'];
                vm.dashbaordInfo['RecFreqIntervalType'] = PVm.getEtljobdataFromMainPage['RecFreqIntervalType'];
                vm.dashbaordInfo['RecFreqMonthCriteria'] = PVm.getEtljobdataFromMainPage['RecFreqMonthCriteria'];
                vm.dashbaordInfo['RecFreqMonthDayInterval'] = PVm.getEtljobdataFromMainPage['RecFreqMonthDayInterval'];
                vm.dashbaordInfo['RecFreqMonthDayMonthInterval'] = PVm.getEtljobdataFromMainPage['RecFreqMonthDayMonthInterval'];
                vm.dashbaordInfo['RecFreqMonthMonthInterval'] = PVm.getEtljobdataFromMainPage['RecFreqMonthMonthInterval'];
                vm.dashbaordInfo['Recfreqweekinterval'] = PVm.getEtljobdataFromMainPage['Recfreqweekinterval'];
                vm.dashbaordInfo['description'] = PVm.getEtljobdataFromMainPage['description'];
                vm.dashbaordInfo['UniqueKey'] = PVm.getEtljobdataFromMainPage['UniqueKey'];
                vm.dashbaordInfo['isPublic'] = PVm.getEtljobdataFromMainPage['isPublic'];
                vm.dashbaordInfo['scheduletype'] = PVm.getEtljobdataFromMainPage['scheduletype'];
                vm.dashbaordInfo['templateTitle'] = PVm.getEtljobdataFromMainPage['templateTitle'];
                vm.dashbaordInfo['templateType'] = PVm.getEtljobdataFromMainPage['templateType'];
                vm.dashbaordInfo['JobID'] = PVm.getEtljobdataFromMainPage['JobID'];
                vm.dashbaordInfo['Query'] = PVm.getEtljobdataFromMainPage['Query'];
                
                if (PVm.getEtljobdataFromMainPage['weekDays'] != null && PVm.getEtljobdataFromMainPage['RecFreqIntervalType'] == 'Weekly') {
                    vm.getweekName = PVm.getEtljobdataFromMainPage['weekDays'].split(',');
                    //"MON,TUE,WED,THU,FRI,SAT,SUN"

                    for (var i = 0; i < 7; i++) {
                        if (i == 0) {
                            if (vm.getweekName.findIndex(x=>x == 'MON') == -1) {
                                vm.getweekName.splice(i, 0, 'false')
                            }
                        }
                        else if (i == 1) {
                            if (vm.getweekName.findIndex(x=>x == 'TUE') == -1) {
                                vm.getweekName.splice(i, 0, 'false')
                            }
                        }
                        else if (i == 2) {
                            if (vm.getweekName.findIndex(x=>x == 'WED') == -1) {
                                vm.getweekName.splice(i, 0, 'false')
                            }
                        }
                        else if (i == 3) {
                            if (vm.getweekName.findIndex(x=>x == 'THU') == -1) {
                                vm.getweekName.splice(i, 0, 'false')
                            }
                        }
                        else if (i == 4) {
                            if (vm.getweekName.findIndex(x=>x == 'FRI') == -1) {
                                vm.getweekName.splice(i, 0, 'false')
                            }
                        }
                        else if (i == 5) {
                            if (vm.getweekName.findIndex(x=>x == 'SAT') == -1) {
                                vm.getweekName.splice(i, 0, 'false')
                            }
                        }
                        else if (i == 6) {
                            if (vm.getweekName.findIndex(x=>x == 'SUN') == -1) {
                                vm.getweekName.splice(i, 0, 'false')
                            }
                        }
                    }

                } else {
                    vm.getweekName = ['MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT', 'SUN'];
                }
                vm.dashbaordInfo['weekDay'] = vm.getweekName;

              //  console.log(vm.dashbaordInfo)
               // scope.$apply();
            }


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
            //$('.datepicker').datetimepicker({

            //    format: 'MM/DD/YYYY'

            //});
        };
        return {
            initialization: function (vm, scope, compile, parentScope, PVm, $uibModalInstance) {
                hanldeInitialization(vm, scope, compile, parentScope, PVm, $uibModalInstance);

            }
        };
    }();
    return TopSectionModalInstanceServiceHandler;
});