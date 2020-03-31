

app.controller("ngCtrlProjectDashBoard", function ($scope, ngSrvcProjectDashBoard, ngSrvcCommon, $filter) {


    $scope.selectedDate = "";

    GetProjectByID(ProjectID);
    GetAllScopes();
    GetAllMarkets();
    GetScopeByProject(ProjectID);
    GetMarketByProject(ProjectID);


    //------------------------------------
    function GetProjectByID(ProjectID) {

        var Data = ngSrvcProjectDashBoard.getProjectByID(ProjectID);
        Data.then(function (data) {
            $scope.projectDetail = data.data.details;

            var date1 = $scope.projectDetail.StartDate;
            var bens1 = $filter('dateFilter')(date1);
            $scope.projectDetail.StartDate = bens1;

            var date1 = $scope.projectDetail.EndDate;
            var bens1 = $filter('dateFilter')(date1);
            $scope.projectDetail.EndDate = bens1;

            siteStatus(data.data.status);

            for (var i = 0; i < data.data.count.length; i++) {
                var date1 = data.data.count[i].SubmittedOn;
                var bens1 = $filter('dateFilter')(date1);
                data.data.count[i].SubmittedOn = bens1;
            }
            siteCount(data.data.count);

        }, function () {
            alert('Error');
        });
    }
    //----------------------------------
    $scope.GetSiteCountByDates = function (selectedDate) {
        debugger;
        var date1 = selectedDate[0];
        var date2 = selectedDate[1];

        var promise = ngSrvcProjectDashBoard.GetSiteCountByDates(date1, date2);
        promise.then(function (data) {

            for (var i = 0; i < data.data.count.length; i++) {
                var date1 = data.data.count[i].SubmittedOn;
                var bens1 = $filter('dateFilter')(date1);
                data.data.count[i].SubmittedOn = bens1;
            }        
            siteCount(data.data.count);

        }, function (err) {
            $scope.Message = "Call Failed " + err.status;
        });
    }


    //------------------------------------
    function GetScopeByProject(ProjectID) {

        var Data = ngSrvcProjectDashBoard.getScopeByProject(ProjectID);
        Data.then(function (s) {
            $scope.scopeByProject = s.data;
        }, function () {
            alert('Error!');
        });
    }

    //------------------------------------
    function GetMarketByProject(ProjectID) {
        var Data = ngSrvcProjectDashBoard.getMarketByProject(ProjectID);
        Data.then(function (m) {
            $scope.marketByProject = m.data;
        }, function () {
            alert('Error!');
        });
    }

    //-------------------------------------

    var ProjectScopes = [];
    var ProjectMarkets = [];

    function GetAllScopes() {

        var Data = ngSrvcCommon.getScopes();
        Data.then(function (scope) {
            $scope.projScope = scope.data;
            $scope.DefType_Scope = scope.data[0].TypeId;
            for (var i = 0; i < $scope.projScope.length; i++) {
                //console.log($scope.projScope[i]);
                ProjectScopes.push({
                    id: $scope.projScope[i].ProjectScopeID,
                    name: $scope.projScope[i].Scope
                });
            }
        }, function () {
            alert('Error');
        });
    }

    //-----------------------------------

    function GetAllMarkets() {
        //debugger;
        var Data = ngSrvcCommon.getMarkets();
        Data.then(function (market) {
            $scope.projMarket = market.data;
            $scope.DefType_Market = market.data[0].TypeId;
            for (var i = 0; i < $scope.projMarket.length; i++) {
                //console.log($scope.projMarket[i]);
                ProjectMarkets.push({
                    id: $scope.projMarket[i].TypeValue,
                    name: $scope.projMarket[i].DefinationName
                });

            }

        }, function () {
            alert('Error');
        });
    }


    //---------------------------------

    // set the default amount of items being displayed

    $scope.limitS = 2;
    $scope.limitM = 2;

    $scope.moreScope = function () {
        $scope.limitS = $scope.scopeByProject.length
    }
    $scope.lessScope = function () {

        $scope.limitS = 2;
    }
    //-------------------------
    $scope.moreMarkets = function () {
        debugger;
        $scope.limitM = $scope.marketByProject.length
    }
    $scope.lessMarkets = function () {
        debugger;
        $scope.limitM = 2;
    }


    //----------------------------------
    $scope.AddScopes = function () {
        //debugger;
        var data = [
            { ProjectID: ProjectID },
            { TypeId: $scope.DefType_Scope },
            { ConfigurationIds: $scope.proj.ProjectScope }
        ]
        var getMSG = ngSrvcProjectDashBoard.AddProjectConfig(data);
        getMSG.then(function (resp) {
            //console.log(resp);
            GetScopeByProject(ProjectID);
            GetMarketByProject(ProjectID);
            //alert('Data Submitted...!');
        }, function (err) {
            alert("Call Failed " + err.status);
        });

    };

    //----------------------------------------
    $scope.AddMarkets = function () {
        //debugger;
        var data = [
            { ProjectID: ProjectID },
            { TypeId: $scope.DefType_Market },
            { ConfigurationIds: $scope.mark.ProjectMarket }
        ]
        var getMSG = ngSrvcProjectDashBoard.AddProjectConfig(data);
        getMSG.then(function (resp) {
            //console.log(resp);
            //alert('Data Submitted...!');
            GetScopeByProject(ProjectID);
            GetMarketByProject(ProjectID);
        }, function (err) {
            alert("Call Failed " + err.status);
        });
    };


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
                    options: data,
                    required: required
                }
            }
        ];
    };

    $scope.multiScopes = multiSelectDropDown("ProjectScope", "Scopes", ProjectScopes, "id", "name", true);
    $scope.multiMarkets = multiSelectDropDown("ProjectMarket", "Markets", ProjectMarkets, "id", "name", true);

});