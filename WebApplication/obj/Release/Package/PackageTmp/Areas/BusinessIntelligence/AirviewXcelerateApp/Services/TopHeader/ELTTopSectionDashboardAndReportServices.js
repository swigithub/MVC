

airviewXcelerateApp.factory('ETLTopSectionDashboardAndReportService', function ($http, $timeout, toastr) {

    var TopSectionDashboardAndReportServiceHandler = function () {

        //#region load private and public tempaltes
        handleLoadPrivateAndPublicTemplate = function (vm) {
            $http({
                method: 'POST',
                url: '/BusinessIntelligence/DashboardNReport/GetTemplate',
                data: { userID: userIDByX1, filter: "GetPriAndPubTmpByUserID" }
            }).then(function successCallback(response) {
                if (response.data.status)
                    vm.loadTemp = JSON.parse(response.data.data);

                // this callback will be called asynchronously
                // when the response is available
            }, function errorCallback(response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
            });
        };
        //#endregion

        var hanldeInitialization = function (vm, scope, compile, uibModal) {
            //#region temp
            vm.classSetDefault = "";
            vm.classSetPublic = "";
            vm.open = false;
            vm.btnText = "Add New";
            vm.isModeType = "view";
            vm.isEtlModeType = "";
            vm.loadTemp = [];

            handleLoadPrivateAndPublicTemplate(vm);

            //$http({
            //    method: 'POST',
            //    url: '/BusinessIntelligence/DashboardNReport/GetTemplate',
            //    data: { userID: 6, filter: "GetAllPriTmpByUserID" }
            //}).then(function successCallback(response) {

            //    console.log(JSON.parse(response.data.data));
            //    // this callback will be called asynchronously
            //    // when the response is available
            //}, function errorCallback(response) {
            //    // called asynchronously if an error occurs
            //    // or server returns response with an error status.
            //});
            vm.loadTempFromDb = function () {
                scope.$emit('LoadForTempE', vm.selectTemp);
                vm.isModeType = "edit";
                vm.classSetDefault = "";
                vm.classSetPublic = "";


            };
            //#endregion
            vm.saveTemplate = function () {
                scope.$emit('saveTemplateE');



            };

            //#region logic for elt job

            vm.initializationOfScheduler = function () {
                $('.datepicker').datetimepicker({

                    format: 'MM/DD/YYYY'

                });
                var d = new Date();
                var month = d.getMonth() + 1;
                var day = d.getDate();
                var year = d.getFullYear();
                var ddate = month + "/" + day + "/" + year


                //    $('.datepicker').attr('ng-model', "vm.dashbaordInfo.once_date")
                $('#once_date').val(ddate);
                $('.datetimepicker-addon').on('click', function () {
                    $(this).prev('input.datepicker').data('DateTimePicker').toggle();
                });
                $('.timepicker').datetimepicker({

                    format: 'HH:mm:ss'
                });
                $('.timepicker-addon').on('click', function () {
                    $(this).prev('input.timepicker').data('DateTimePicker').toggle();
                });
                var dt = new Date();

                var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
                $('#once_time').val(time);
                $('.timepicker2').datetimepicker({
                    format: 'HH:mm:ss'
                });
                $('.timepicker2-addon').on('click', function () {
                    $(this).prev('input.timepicker2').data('DateTimePicker').toggle();

                });
                var dt = new Date();
                //set current time
                var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
                $('#occur-once-time').val(time);
                $('.timepicker3').datetimepicker({
                    format: 'HH:mm:ss',
                    widgetPositioning: {
                        horizontal: 'right'
                    }
                });
                $('.timepicker3-addon').on('click', function () {
                    $(this).prev('input.timepicker3').data('DateTimePicker').toggle();
                });
                var dt = new Date();
                //set current time
                var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
                $('#df-start-time').val(time);

                $('.timepicker4').datetimepicker({
                    format: 'HH:mm:ss',
                    widgetPositioning: {
                        horizontal: 'right'
                    }
                });
                $('.timepicker4-addon').on('click', function () {
                    $(this).prev('input.timepicker4').data('DateTimePicker').toggle();
                });
                var dt = new Date();
                //set current time
                var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
                $('#df-end-time').val(time);
                $('.datepicker3').datetimepicker({
                    format: 'MM/DD/YYYY'
                });
                $('.datepicker3-addon').on('click', function () {
                    $(this).prev('input.datepicker3').data('DateTimePicker').toggle();
                });
                //set default date
                var d = new Date();
                var month = d.getMonth() + 1;
                var day = d.getDate();
                var year = d.getFullYear();
                var ddate = month + "/" + day + "/" + year

                $('#ds-date').val(ddate);
                $('.datepicker2').datetimepicker({
                    format: 'MM/DD/YYYY',
                    widgetPositioning: {
                        horizontal: 'right'
                    }
                });
                $('.datepicker2-addon').on('click', function () {
                    $(this).prev('input.datepicker2').data('DateTimePicker').toggle();
                });
                //set default date
                var d = new Date();
                var month = d.getMonth() + 1;
                var day = d.getDate();
                var year = d.getFullYear();
                var ddate = month + "/" + day + "/" + year

                $('#de-date').val(ddate);


                $("select.otr").change(function () {

                    //$(".recurring").prop('disabled', $(this).val() == 'disable');
                    //$(".onetime").prop('disabled', $(this).val() == 'otdisable');

                    var value = $(this).val();
                    var oncedate = $('#once_date').val();
                    var oncetime = $('#once_time').val();


                    if (value == "Recurring") {

                        $(".recurring").prop('disabled', true);
                        $('.disable').css({
                            'color': '#ccc'

                        });

                    }
                    else {

                        $(".recurring").prop('disabled', false);
                        $('.disable').css({
                            'color': 'black'

                        });

                    }
                    if (value == "OneTime") {

                        $(".onetime").prop('disabled', true);
                        $('#occur-once-time').attr("readonly", true);
                        $("#fodaymonth").attr("readonly", true);
                        $("#fodayday").attr("readonly", true);
                        $('.otdisable').css({
                            'color': '#ccc'

                        });
                        $("#one-time-msg").show();
                        //$("#myTextBox").on("change paste keyup", function () {
                        //    var oncedate = $('#once_date').val();
                        //});
                        $("textarea").val("");
                        $('textarea').val('Occurs on' + ' ' + oncedate + ' ' + 'at' + ' ' + oncetime);

                        $(document).on('dp.change', 'input.otrow', function () {

                            var oncedate = $('#once_date').val();
                            var oncetime = $('#once_time').val();
                            // $('textarea').val('Occurs on' + ' ' + oncedate + ' ' + 'at' + ' ' + oncetime);
                        });
                        //$('#once_date').bind("change paste keyup", function () {
                        //    debugger
                        //    
                        //});



                    }
                    else {
                        $('#occur-once-time').attr("readonly", false);
                        $("#fodayday").attr("readonly", false);
                        $("#fodaymonth").attr("readonly", false);
                        $(".onetime").prop('disabled', false);

                        $('.otdisable').css({
                            'color': 'black'

                        });
                        $("#one-time-msg").hide();
                        $('textarea').val('this is recurring text');
                    }
                    $("input.durationdates:radio:last").prop("checked", true).trigger("change");
                    $("input.occur-once:radio:first").prop("checked", true).trigger("change");
                    $("input.fm:radio:first").prop("checked", true).trigger("change");
                    //update description



                });


                $('select.otr')
                   .val('OneTime')
                   .trigger('change');


                //daily frequncy radio
                $("input.occur-once:radio").change(function () {
                    //var value = $(this).val();
                    //if (value == 'Once') {
                    //    $('.occurs-onced').prop('disabled', true);
                    //}
                    //else {
                    //    $('.occurs-onced').prop('disabled', false);
                    //}
                    $(".occurs-onced").prop('disabled', $(this).val() == 'Once');
                    $(".occurs-every").prop('disabled', $(this).val() == 'Custom');

                })

                //$("input.occur-once:radio").val("disable").trigger("change");
                //Frequency monthday radio
                $("input.fm:radio").change(function () {

                    var value = $(this).val();
                    if (value == "Once") {
                        $(".mday").prop('disabled', true);

                    }
                    else {

                        $(".mday").prop('disabled', false);
                    }
                    if (value == "Custom") {
                        $(".mthe").prop('disabled', true);

                    }
                    else {

                        $(".mthe").prop('disabled', false);
                    }

                })


                //duration radio 
                $("input.durationdates:radio").change(function () {
                    var value = $(this).val();
                    if (value == "true") {
                        $(".noenddate").prop('disabled', true);
                    }
                    else {
                        $(".noenddate").prop('disabled', false);
                    }


                })
                //Frequency
                $("select.re").change(function () {


                    var value = $(this).val();


                    if (value == "Daily") {
                        $(".weeklyrow").hide();
                        $(".monthlyrowday").hide();
                        $(".monthlyrowthe").hide();
                        $(".dailyrow").show();
                    }

                    else if (value == "Weekly") {
                        $(".dailyrow").hide();
                        $(".monthlyrow").hide();
                        $(".monthlyrowday").hide();
                        $(".monthlyrowthe").hide();
                        $(".weeklyrow").show();

                    }

                    else if (value == "Monthly") {
                        $(".dailyrow").hide();
                        $(".weeklyrow").hide();
                        $(".monthlyrowday").show();
                        $(".monthlyrowthe").show();
                    }
                    else {
                        $(".dailyrow").hide();
                        $(".weeklyrow").hide();
                        $(".monthlyrow").hide();
                        $(".monthlyrowday").hide();
                        $(".monthlyrowthe").hide();
                    }

                });
                $('select.re')
                   .val('Daily')
                   .trigger('change');
                // update description


                //$("#dfoccurevery").change(function () {
                //    $("#dfOccurOnce").prop('checked', false);
                //})
                //$("#dfOccurOnce").change(function () {
                //    $("#dfoccurevery").prop('checked', false);
                //})
                //$("#NoEndDate").change(function () {
                //    $("#Duriscontinous").prop('checked', false);
                //})
                //$("#Duriscontinous").change(function () {
                //    $("#NoEndDate").prop('checked', false);
                //})

                //$(document).ready(function () {
                //    $("#isactive").prop('checked', true);
                //})
                if (vm.isEtlModeType == 'edit') {
                    vm.assignValueToDatetimepickerOnEditTime();
                }

            }
            vm.assignValueToDatetimepickerOnEditTime = function () {
                if (vm.getEtljobdataFromMainPage['scheduletype'] == 'Recurring') {
                    $('select.otr')
                      .val(vm.getEtljobdataFromMainPage['scheduletype'])
                      .trigger('change');
                    $('select.re').val(vm.getEtljobdataFromMainPage['RecFreqIntervalType']).trigger('change');
                    if (vm.getEtljobdataFromMainPage['DayFreqDayCriteria'] == 'Once')
                        $('#dfOccurOnce').val(vm.getEtljobdataFromMainPage['DayFreqDayCriteria']).trigger('click');
                    else {
                        $('#dfoccurevery').val(vm.getEtljobdataFromMainPage['DayFreqDayCriteria']).trigger('click');
                    }
                    if (vm.getEtljobdataFromMainPage['RecFreqIntervalType'] == 'Monthly') {
                        if (vm.getEtljobdataFromMainPage['RecFreqMonthCriteria'] == 'Once')
                            $('#mday').val(vm.getEtljobdataFromMainPage['RecFreqMonthCriteria']).trigger('click');
                        else
                            $('#mthe').val(vm.getEtljobdataFromMainPage['RecFreqMonthCriteria']).trigger('click');
                    }
                    if (vm.getEtljobdataFromMainPage['DurisContinous'] == 'true' || vm.getEtljobdataFromMainPage['DurisContinous'] == true) {
                        $('.continous').prop('checked',true).trigger('change');
                    }
                    else {
                        $('.discontinous').prop('checked',true).trigger('change');
                    }
                }

                $("#once_date").val(moment(vm.getEtljobdataFromMainPage['OnceDate']).format('MM/DD/YYYY'));
                $("#once_time").val(vm.getEtljobdataFromMainPage['OnceTime']);
                $("#df-start-time").val(vm.getEtljobdataFromMainPage['DayFreqCusStartTme']);
                $("#df-end-time").val(vm.getEtljobdataFromMainPage['DayFreqCusEndTime']);
                $("#ds-date").val(moment(vm.getEtljobdataFromMainPage['DurStartDate']).format('MM/DD/YYYY'));
                $('#occur-once-time').val(vm.getEtljobdataFromMainPage['DayFreqIntervalTime'])
                $('#de-date').val(moment(vm.getEtljobdataFromMainPage['DurEndDate']).format('MM/DD/YYYY'));
                $('#etlReadonly').attr('readonly', 'readonly');
            }
            //#endregion


            vm.addNewEtl = function () {
                vm.isEtlModeType = 'new';
                vm.getEtljobdataFromMainPage = null;
                vm.openmodel = uibModal.open({
                    templateUrl: 'dsrpt/DashboardNReport/getPartialPage/DashboardAndReport/_ETLcreateDashboadAndReport.cshtml',
                    backdrop: 'static',
                    controller: "ETLTopSectionModalInstance as vm",
                    size: 'lg',
                    resolve: {
                        parentScope: function () {

                            return scope;
                        },
                        PVm: function () {
                            return vm;
                        }
                    }
                });
                vm.openmodel.result.then(function () {



                });
                vm.openmodel.opened.then(function () {
                    setTimeout(vm.initializationOfScheduler, 100)


                });
            };

            vm.etlJobEdit = function () {
               // console.log(vm.getEtljobdataFromMainPage);
                vm.openmodel = uibModal.open({
                    templateUrl: 'dsrpt/DashboardNReport/getPartialPage/DashboardAndReport/_ETLcreateDashboadAndReport.cshtml',
                    backdrop: 'static',
                    controller: "ETLTopSectionModalInstance as vm",
                    size: 'lg',
                    resolve: {
                        parentScope: function () {

                            return scope;
                        },
                        PVm: function () {
                            return vm;
                        }
                    }
                });
                vm.openmodel.result.then(function () {



                });
                vm.openmodel.opened.then(function () {
                    setTimeout(vm.initializationOfScheduler, 100)


                });
            };



            vm.viewToEditMode = function () {
                scope.$emit('changeModeTypeE', 'edit');
                vm.isModeType = "edit";
                handleUnlockGridStack();

            };
            vm.editToViewMode = function () {
                vm.isModeType = "view";
                scope.$emit('changeModeTypeE', 'view');
                handleLockGridStack();
            };
            scope.$on("changeModeTypeInTopHeaderB", function (event, modeType) {
                vm.isModeType = modeType;
            });
            scope.$on("refreshDropdownListB", function (event) {
                handleLoadPrivateAndPublicTemplate(vm);
            });


            vm.setDefaultTemplate = function () {

                if (vm.classSetDefault == "") {
                    vm.classSetDefault = "green";
                    scope.$emit('setDefaultTemplateFromTopHeaderE', true);
                    toastr.success("Template set as default");
                }
                else {
                    vm.classSetDefault = "";
                    scope.$emit('setDefaultTemplateFromTopHeaderE', false);
                    toastr.success("Template not set as default");
                }
            };
            vm.setPublicTemplate = function () {

                if (vm.classSetPublic == "") {
                    vm.classSetPublic = "green";
                    scope.$emit('setPublicTemplateFromTopHeaderE', true);
                }
                else {
                    vm.classSetPublic = "";
                    scope.$emit('setPublicTemplateFromTopHeaderE', false);
                }
            };
            scope.$on("setDefaultTemplateFromDashboardReportB", function (event, setDefaultTemplate) {
                if (setDefaultTemplate) { vm.classSetDefault = 'green'; }
                else { vm.classSetDefault = ""; }
            });
            scope.$on("setPublicTemplateFromDashboardReportB", function (event, setDefaultTemplate) {

                if (setDefaultTemplate) {
                    vm.classSetPublic = "green";
                }
                else { vm.classSetPublic = ""; }
            });
            scope.$on("setEtlJobParamsForEditB", function (event, EtlJobParams) {
                vm.isEtlModeType = 'edit';
                vm.getEtljobdataFromMainPage = EtlJobParams;
              //  console.log(EtlJobParams);
            });



            $('[data-toggle="tooltip"]').tooltip();
            vm.printDashboardAndReport = function () {
                $('body').addClass('when-print');
                window.print();
                $('body').removeClass('when-print');
            };


        };
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
        return {
            initialization: function (vm, scope, compile, $uibModal) {
                hanldeInitialization(vm, scope, compile, $uibModal);

            }
        };
    }();
    return TopSectionDashboardAndReportServiceHandler;
});