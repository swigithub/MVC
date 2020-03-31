

app.controller("ngCtrlProject", function ($scope, $filter, angularServiceProject, ngSrvcCommon) {

    $scope.btntext = "Save";
    //$scope.projectScope = [{ id: 1, name: 'SSV' }, { id: 2, name: 'NI' }, { id: 3, name: 'Clusters' }];
    $scope.ProjectScopeID = [];


    GetAllCompany();
    GetAllVendors();
    GetAllProjectScope();
    GetAllProjectStatus();

    function GetAllCompany() {
        var Data = ngSrvcCommon.getCompany();
        Data.then(function (comp) {
            $scope.companies = comp.data;
        }, function () {
            alert('Error');
        });
    }

    function GetAllVendors() {
        var Data = ngSrvcCommon.getVendors();
        Data.then(function (vend) {
            $scope.vendors = vend.data;
        }, function () {
            alert('Error');
        });
    }

    function GetAllProjectScope() {
        var Data = ngSrvcCommon.getProjectScope();
        Data.then(function (scope) {
            $scope.projScope = scope.data;
        }, function () {
            alert('Error');
        });
    }

    function GetAllProjectStatus() {
        var Data = ngSrvcCommon.getProjectStatus();
        Data.then(function (status) {
            $scope.projStatus = status.data;
        }, function () {
            alert('Error');
        });
    }

    $scope.Save = function (project) {
        $scope.btntext = "Please wait...!";

        project.ProjectScopeID = $scope.ProjectScopeID;
        var getMSG = angularServiceProject.Add(project);
        getMSG.then(function (messagefromController) {
            $scope.btntext = "Save";
            $scope.project = null;
            alert('Data Submitted...!');
        }, function () {
            alert('Insert Error');
        });
    }

    if (ProjectID > 0) {

        var Data = angularServiceProject.getProjectByID(ProjectID);
        Data.then(function (project) {
            debugger;
            $scope.project = project.data;
            $scope.project.CompanyID = project.data.CompanyID;
            $scope.project.VendorID = project.data.VendorID;

            var Ids = project.data.ProjectScopeID;
            var id = Ids.split(',');
            for (var i = 0; i < id.length; i++) {
                $scope.ProjectScopeID.push(id[i])
            }


            var date1 = $scope.project.StartDate;
            var bens1 = $filter('dateFilter')(date1);
            var StDate =new Date(bens1);
            $scope.project.StartDate = StDate;


            var date2 = $scope.project.EndDate;
            var bens2 = $filter('dateFilter')(date2);
            var endDate = new Date(new Date(bens2));
            $scope.project.EndDate = endDate;

        }, function () {
            alert('Error');
        });
    }

    $scope.delete = function (project) {
        var getMSG = angularServiceProject.Delete(project.ProjectID);
        getMSG.then(function (messagefromController) {
            alert(messagefromController.data);
        }, function () {
            alert('Delete Error');
        });
    }


});

app.controller('ngCtrlSearch', function ($scope, $filter, ngSrvcSearch, ngSrvcCommon) {

    $scope.maxSize = 5;     // Limit number for pagination display number.
    $scope.totalCount = 0;  // Total number of items in all pages. initialize as a zero
    $scope.pageIndex = 1;   // Current page number. First page is 1.-->
    $scope.pageSizeSelected = 5; // Maximum number of items per page.

    $scope.array2 = [];
    $scope.selectedDate = null;
    $scope.SearchProject = {};

    //$scope.SearchProject = { date: null };
    //$scope.date = { StartDate: null, EndDate: null };

    //+++++++++++++++++++++++++++++++++++++++
    GetFormattedDateR = function (todayTime) {
        // var todayTime = new Date();
        var month = todayTime.getMonth() + 1;
        var day = todayTime.getDate();
        var year = todayTime.getFullYear();
        if (day.toString().length == 1)
            day = "0" + day;
        if (month.toString().length == 1)
            month = "0" + month;
        return month + "/" + day + "/" + year;
    }

    GetFormattedDate = function (todayTime) {
        // var todayTime = new Date();
        var month = todayTime.getMonth() + 1;
        var day = todayTime.getDate();
        var year = todayTime.getFullYear();
        if (day.toString().length == 1)
            day = "0" + day;
        if (month.toString().length == 1)
            month = "0" + month;
        return year + "-" + month + "-" + day;
    }

    var date = new Date();
    var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    $scope.from_date = GetFormattedDate(firstDay);
    $scope.to_date = GetFormattedDate(lastDay);
    //-----------------------------------


    loadProjectsName();
    loadCompany();
    loadVendors();
    loadStatus();

    //Arrays declaration for serch dropdowns list
    var ProjectNames = [];
    var CompanyNames = [];
    var VendorNames = [];
    var ProjectStatus = [];

    $scope.selectedDate = "";
    //----------------------------
    $scope.Search = function (selectedDate) {
       // debugger;
        
        //   ($scope.SearchProject.StartDate != undefined && $scope.SearchProject.EndDate!= undefined)

        if (Object.keys($scope.SearchProject).length === 0 && selectedDate === undefined) {
            alert("Please Select!");
        }
        else {
            $scope.SearchProject.StartDate = null;
            $scope.SearchProject.EndDate = null;

            if (selectedDate != undefined) {
                $scope.SearchProject.StartDate = selectedDate[0];
                $scope.SearchProject.EndDate = selectedDate[1];
            }
            loadProjectsData($scope.SearchProject);
        }
       
    }
    //---------------------------
    function loadProjectsData(SearchProject) {
    
        var promise = ngSrvcCommon.getProjects(SearchProject, $scope.pageIndex, $scope.pageSizeSelected);
        promise.then(function (resp) {
            $scope.Project = resp.data;
      
            $scope.totalCount = $scope.Project[0].totalCount;

            for (var i = 0; i < $scope.Project.length; i++) {
                var date1 = $scope.Project[i].StartDate;
                var bens1 = $filter('dateFilter')(date1);
                //var StDate = new Date(new Date(bens1));
                $scope.Project[i].StartDate = bens1; //GetFormattedDateR(StDate);

                if ($scope.Project[i].EndDate != undefined) {
                    var date2 = $scope.Project[i].EndDate;
                    var bens2 = $filter('dateFilter')(date2);
                    //var endDate = new Date(new Date(bens2));
                    $scope.Project[i].EndDate = bens2; // GetFormattedDateR(endDate);
                }
            }
            $scope.array2 = $scope.Project;
            $scope.Message = "Call is Completed Successfully";

        }, function (err) {
            $scope.Message = "Call Failed " + err.status;
        });
    };
    //---------------------------


    //This method is calling from pagination number
    $scope.pageChanged = function () {
        loadProjectsData($scope.SearchProject);
    };
    //This method is calling from dropDown
    $scope.changePageSize = function () {
        $scope.pageIndex = 1;
        loadProjectsData($scope.SearchProject);
    };

    //-----------------------------
    function loadProjectsName() {
       // debugger;
        var promise = ngSrvcCommon.getProjectsNames();
        promise.then(function (resp) {
            $scope.searchOptionsProject = resp.data;
            for (var i = 0; i < $scope.searchOptionsProject.length; i++) {
                ProjectNames.push({
                    id: $scope.searchOptionsProject[i].ProjectID,
                    name: $scope.searchOptionsProject[i].ProjectName
                });
            }
            $scope.Message = "Call Successfull";
        }, function (err) {
            $scope.Message = "Call Failed " + err.status;
        });
    };

    //-----------------------------
    function loadCompany() {
        var promise = ngSrvcCommon.getCompany();
        promise.then(function (resp) {
            $scope.searchOptionsCompany = resp.data;
            for (var i = 0; i < $scope.searchOptionsCompany.length; i++) {
                CompanyNames.push({
                    id: $scope.searchOptionsCompany[0].CompanyID,
                    name: $scope.searchOptionsCompany[0].Company
                });
            }
            $scope.Message = "Call Successfull";
        }, function (err) {
            $scope.Message = "Call Failed " + err.status;
        });
    };

    //------------------------------
    function loadVendors() {
        var promise = ngSrvcCommon.getVendors();

        promise.then(function (resp) {
            $scope.searchOptionsVendors = resp.data;
            for (var i = 0; i < $scope.searchOptionsVendors.length; i++) {
                VendorNames.push({
                    id: $scope.searchOptionsVendors[i].VendorID,
                    name: $scope.searchOptionsVendors[i].Vendor
                });
            }
            $scope.Message = "Call Successfull";
        }, function (err) {
            $scope.Message = "Call Failed " + err.status;
        });
    };

    //------------------------------
    function loadStatus() {
        debugger;
        var promise = ngSrvcCommon.getStatus();
        promise.then(function (resp) {
            $scope.searchOptionsStatus = resp.data;
            for (var i = 0; i < $scope.searchOptionsStatus.length; i++) {
                ProjectStatus.push({
                    id: $scope.searchOptionsStatus[i].StatusID,
                    name: $scope.searchOptionsStatus[i].Status
                });
            }
            $scope.Message = "Call Successfull";
        }, function (err) {
            $scope.Message = "Call Failed " + err.status;
        });
    };
    //------------------------------

    var multiSelectDropDown = function (key, label, data, valueProp, labelProp, required) {
        return [
            {
                key: key,
                type: 'ui-select-multiple',
                templateOptions: {
                    optionsAttr: 'bs-options',
                    ngOptions: 'option[to.valueProp] as option in to.options | filter: $select.search',
                    valueProp: valueProp,
                    labelProp: labelProp,
                    label: label,
                    placeholder: label,
                    options: data//,
                    //required: required
                }
            }
        ];
    };

    $scope.multiProject = multiSelectDropDown("ProjectIds", "Project", ProjectNames, "id", "name", true);
    $scope.multiCompany = multiSelectDropDown("CompanyIds", "Company", CompanyNames, "id", "name", true);
    $scope.multiVendor = multiSelectDropDown("VendorIds", "Vendor", VendorNames, "id", "name", true);
    $scope.multiStatus = multiSelectDropDown("StatusIds", "Status", ProjectStatus, "id", "name", true);


    //$scope.Search = function (selectedDate) {
    //    var date1 = selectedDate[0];
    //    var date2 = selectedDate[1];

    //    //if (date1 != undefined) {
    //    //    $scope.SearchProject.date = date1[0] + '-' + date2[1]
    //    //}
    //    var promise = ngSrvcSearch.getSearchItems($scope.SearchProject, date1, date2);
    //    promise.then(function (resp) {
    //        $scope.Project = resp.data;        // fill grid data

    //        for (var i = 0; i < $scope.Project.length; i++) {
    //            var date1 = $scope.Project[i].StartDate;
    //            var bens1 = $filter('dateFilter')(date1);
    //            //var StDate = new Date(new Date(bens1));
    //            $scope.Project[i].StartDate = bens1; //GetFormattedDateR(StDate);

    //            if ($scope.Project[i].EndDate != undefined) {
    //                var date2 = $scope.Project[i].EndDate;
    //                var bens2 = $filter('dateFilter')(date2);
    //                //var endDate = new Date(new Date(bens2));
    //                $scope.Project[i].EndDate = bens2; // GetFormattedDateR(endDate);
    //            }
    //        }

    //    }, function (err) {
    //        $scope.Message = "Call Failed " + err.status;
    //    });
    //};

    $scope.changeStatus = function (project) {
        var getMSG = ngSrvcSearch.updateStatus(project);
        getMSG.then(function (messagefromController) {
            //alert('Status Changed');
        }, function () {
            alert('Error!');
        });
    }

    //=======Search functions==============

    //$scope.searchByProject = function (data) {
    //    $scope.array = [];
    //    $scope.Project = [];
    //    for (var i = 0; i < data.length; i++) {
    //        var found = $filter('getProjectById')($scope.array2, data[i]);
    //        $scope.array.push(found);
    //    }
    //    $scope.Project = $scope.array;
    //}

    ////---------------------------------
    //$scope.searchByCompany = function (data) {
    //    //$scope.arrayCompany = [];
    //    $scope.Project = [];
    //    for (var i = 0; i < data.length; i++) {
    //        var found = $filter('getProjectByCompany')($scope.array2, data[i]);
    //        //$scope.arrayCompany.push(found);
    //    }
    //    $scope.Project = found;
    //}

    ////---------------------------------
    //$scope.searchByVendor = function (data) {
    //    // $scope.arrayVendor = [];
    //    $scope.Project = [];
    //    for (var i = 0; i < data.length; i++) {
    //        var found = $filter('getProjectByVendor')($scope.array2, data[i]);
    //        //$scope.arrayVendor.push(found);
    //    }
    //    $scope.Project = found;
    //}

    ////--------------------------------
    //$scope.searchByStatus = function (data) {
    //    $scope.arrayStatus = [];
    //    $scope.Project = [];
    //    if (data.length != undefined) {
    //        for (var i = 0; i < data.length; i++) {
    //            var found = $filter('getProjectByStatus')($scope.array2, data[i]);
    //            $scope.arrayStatus.push(found);
    //        }
    //    }
    //    $scope.Project = $scope.arrayStatus;
    //}

    ////-------------------------------
    //$scope.searchByDate = function (data) {

    //    $scope.arrayStatus = [];
    //    $scope.Project = [];
    //    var found = $filter('dateRange')($scope.array2, $scope.from_date, $scope.to_date);
    //    $scope.arrayStatus.push(found);

    //    $scope.Project = found;
    //}

    ////---------------------------------
    //$scope.search = function (item) {
    //    if ($scope.searchText == undefined) {
    //        return true;
    //    }
    //    else {
    //        var str1 = $filter('dateFilter')(item.StartDate);
    //        if (str1.indexOf($scope.searchText) != -1 ||
    //            item.Status.toLowerCase()
    //                     .indexOf($scope.searchText.toLowerCase()) != -1) {
    //            return true;
    //        }
    //    }
    //    return false;
    //};
    //---------------------------------
});
//====================================

app.directive('datepicker', function () {

    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {
                element.datepicker({
                    dateFormat: 'yyyy/mm/dd',
                    onSelect: function (date) {
                        scope.$apply(function () {
                            ngModelCtrl.$setViewValue(date);
                        });
                    }
                });
            });
        }
    }
});

app.filter('dateRange', function () {
    return function (items, fromDate, toDate) {

        var filtered = [];

        //console.log(fromDate, toDate);
        //var from_date = Date.parse(fromDate);
        //var to_date = Date.parse(toDate);

        var from_date = fromDate;
        var to_date = toDate;

        //var date = new Date();
        //var firstDaya = new Date(date.getFullYear(), date.getMonth(), 1);
        //var lastDayb = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        //var a = GetFormattedDateR(firstDaya);
        //var b = GetFormattedDateR(lastDayb);

        angular.forEach(items, function (item) {

            if ((item.StartDate >= from_date && item.EndDate <= to_date) //|| (item.StartDate >= a && item.StartDate <= b)
                ) {
                filtered.push(item);
            }
        });
        return filtered;
    };
});




















