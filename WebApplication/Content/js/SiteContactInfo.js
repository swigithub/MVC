var app = angular.module('AV', []);
app.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});
app.controller('ContantInfo', function ($scope, $http,$timeout) {

    $scope.AccessInfo = {};

    $scope.SiteId = window.location.href.substr(window.location.href.lastIndexOf('/') + 1);
    $scope.Contacts = [];
   
    $scope.ContactTypes = [];
    $scope.ContactDesignations = [];

    var newItemNo = $scope.Contacts.length + 1;

    $scope.GetContactInfo = function () {
        debugger;
        $("div#divLoading").addClass('show');
        $http({
            method: 'POST',
            url: '/Site/GetContactInfo',
            data:{'SiteId': $scope.SiteId }
        }).then(function successCallback(res) {

            $scope.AccessInfo = res.data.AI;
            if ($scope.AccessInfo.AccessDateTime != null && $scope.AccessInfo.AccessDateTime != undefined && $scope.AccessInfo.AccessDateTime != "")
            {
                $scope.AccessInfo.AccessDateTime = new Date(parseInt($scope.AccessInfo.AccessDateTime.split('(')[1].split(')')[0]));
            }

            if (res.data.Contacts.length > 0) {
                $scope.Contacts = res.data.Contacts;
                if (res.data.Contacts.length > 1) {
                    $scope.Contacts.length = res.data.Contacts.length;
                }
                $.each($scope.Contacts, function (i, v) {
                    debugger;
                        $scope.Contacts[i].ContactTypeID = $scope.Contacts[i].ContactTypeID.toString();
                        $scope.Contacts[i].DesignationID = $scope.Contacts[i].DesignationID.toString();

                    });

                    $("div#divLoading").removeClass('show');
            }
            $("div#divLoading").removeClass('show');
        }, function errorCallback(response) {
            $("div#divLoading").removeClass('show');
        });
    }

    $scope.Contacts.push({
        'id': 'Contact' + newItemNo
    });

    $scope.NewRow = function () {
        if (event.keyCode===9) {
            var newItemNo = $scope.Contacts.length + 1;

            $scope.Contacts.push({
                'id': 'Contact' + newItemNo
            });
        }

        else if (event.keyCode === 46) {
            var lastItem = $scope.Contacts.length - 1;
            $scope.Contacts.splice(lastItem);
        }
       
    }

    $scope.DelRow = function (index) {
            $.confirm({
                title: 'Confirm!',
                content: 'Do you want to Delete!',
                buttons: {
                    Yes: function () {
                            $timeout($scope.Contacts.splice(index, 1));
                    },
                    No: function () {
                    },
                }
            });
    }
    $scope.DeldateTime = function () {
        delete $scope.AccessInfo.AccessDateTime;
    }
    $scope.Submit = function () {
        debugger;
        if ($scope.AccessInfo.IsSpecialAccess == "1") {
            $scope.AccessInfo.IsSpecialAccess = true;
        }
        else {
            $scope.AccessInfo.IsSpecialAccess = false;
        }
        //$http.post("/Site/ContactInfo", { 'con': $scope.Contacts, 'sector': $scope.Sectorarray }).then(function (data) {
        $http({
            method: "post",
            url: "/Site/ContactInfo",
            data: { 'con': $scope.Contacts, 'SiteId': $scope.SiteId,'AI': $scope.AccessInfo }
        }).then(function Success(result) {
            var res = result.data;
            if ($scope.AccessInfo.IsSpecialAccess) {
                $scope.AccessInfo.IsSpecialAccess = "1";
            }
            else {
                $scope.AccessInfo.IsSpecialAccess = "0";
            }
           
            if (res !== 'session expired') {
                if (res.Status === 'success') {
                    $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });

                } else {
                    $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }
            } else {
                $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
            }

        }, function myError(response) {
            $.notify(response.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
        });
    };


    $http({
        url: "/Defnation/ToList?filter=byDefinationType&value=Contact Types"
    }).then(function success(res) {
        $scope.ContactTypes = res.data;

    });

    $http({
        url: "/Defnation/ToList?filter=byDefinationType&value=Contact Designations"
    }).then(function success(res) {
        $scope.ContactDesignations = res.data;

    });

    $scope.GetDateTimeControl = function (response) {
        debugger;
        if (response != "Invalid Date" && response!=null && response!=undefined) {
            var date = new Date(response);
        }

        else {
            var date = new Date();
        }

        var newDate = date.toLocaleString(undefined, {
            day: '2-digit',
            month: 'numeric',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
        $scope.AccessInfo.AccessDateTime = newDate;
        $('.datetimepicker1').datetimepicker({
            defaultDate: newDate,
            sideBySide: true,
            toolbarPlacement: "bottom",
            showClose: true,
            showTodayButton:true,
            format: "M/D/YYYY, HH:mm A",
            tooltips: {
                today: 'Go to today',
                clear: 'Clear selection',
                close: 'Close the picker',
                selectMonth: 'Select Month',
                prevMonth: 'Previous Month',
                nextMonth: 'Next Month',
                selectYear: 'Select Year',
                prevYear: 'Previous Year',
                nextYear: 'Next Year',
                selectDecade: 'Select Decade',
                prevDecade: 'Previous Decade',
                nextDecade: 'Next Decade',
                prevCentury: 'Previous Century',
                nextCentury: 'Next Century'
            }


        }).on('dp.change', function (e) {
            debugger;
            var d = new Date(e.date._d);
            var tempdate = d.toLocaleString(undefined, {
                day: '2-digit',
                month: 'numeric',
                year: 'numeric',
                hour: '2-digit',
                minute: '2-digit'
            });
            $timeout($scope.AccessInfo.AccessDateTime = tempdate);
        })

    }
});