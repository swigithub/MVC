
var app = angular.module('appProject', ['formly', 'formlyBootstrap', 'ui.select', 'ngSanitize', 'ui.bootstrap'], function config(formlyConfigProvider) {
    formlyConfigProvider.setType({
        name: 'ui-select-multiple',
        extends: 'select',
        templateUrl: '/Areas/Project/Pages/ui-select-multiple.html'
    });
});




app.service('ngSrvcCommon', function ($http) {

    this.getProjectsNames = function () {
        return res = $http.get("/Project/Defination/AllProject");
    };

    this.getCompany = function () {
        return res = $http.get("/Project/Defination/AllCompany");
    };

    //---------------------------------
    this.getVendors = function () {
        return res = $http.get("/Project/Defination/AllVendors");
    };
 
    ////-------------------------- ?ProjectId=" + ProjectId
    //this.getProjects = function (pageIndex, pageSizeSelected) {
    //    return $http.get("/Project/Defination/getAll?pageIndex=" + pageIndex + "&pageSize=" + pageSizeSelected);
    //};


    this.getProjects = function (SearchProject, pageIndex, pageSizeSelected) {
        var response = $http({
            method: "post",
            url: "/Project/Defination/getAll",
            data: JSON.stringify({ SearchProject: SearchProject, pageIndex: pageIndex, pageSize: pageSizeSelected }),
            dataType: "json"
        });
        return response;
    }

    //----------------------------
    this.getScopes = function () {
        return $http.get("/Project/Defination/AllProjectScope");
    };
    //----------------------------
    this.getStatus = function () {
        return res = $http.get("/Project/Defination/AllProjectStatus");
    };
    this.getProjectStatus = function () {
        return $http.get("/Project/Defination/AllProjectStatus");
    };
    //-------------------------------
    this.getProjectScope = function () {
        return $http.get("/Project/Defination/AllProjectScope");
    };
    //------------------------------
    this.getMarkets = function () {
        return $http.get("/Project/Defination/AllCitiesMarkets");
    };
   
});

//------------PROJECT-----------
app.service('angularServiceProject', function ($http) {

    this.getProjectByID = function (ProjectID) {
        return $http.get("/Project/Defination/Edit/" + ProjectID);
    };

    this.Add = function (project) {
        var response = $http({
            method: "post",
            url: "/Project/Defination/New",
            data: JSON.stringify({ project: project, Ids: project.ProjectScopeID }),
            dataType: "json"
        });
        return response;
    }

    this.Delete = function (ProjectID) {
        var response = $http({
            method: "post",
            url: "/Defination/Delete",
            params: {
                id: ProjectID
            }
        });
        return response;
    }
});


//------------SEARCH-----------
app.service('ngSrvcSearch', function ($http) {

    this.getSearchItems = function (SearchProject) {
        return res = $http.post("/Project/Defination/SearchInputs", SearchProject);
    };

    this.updateStatus = function (project) {
        var response = $http({
            method: "post",
            url: "/Project/Defination/updateProjectStatus",
            data: JSON.stringify({ project: project}),
            dataType: "json"
        });
        return response;
    }
});

//--------------------------------------------
app.service('ngSrvcProjectDashBoard', function ($http) {

    //this.getProjects = function () {
    //    return $http.get("/Project/Defination/getAll");
    //};

    //this.getScopes = function () {
    //    return $http.get("/Project/Defination/AllProjectScope");
    //};


    //this.getMarkets = function () {
    //    return $http.get("/Project/Defination/AllCitiesMarkets");
    //};

    this.getProjectByID = function (ProjectID) {
        return $http.get("/Project/Defination/GetProjectDetails?ProjectId=" + ProjectID);
    };

    this.GetSiteCountByDates = function (date1, date2) {

        return $http.get("/Project/Defination/GetSiteCountByDates?date1=" + date1 + "&date2=" + date2);
    };

    this.getScopeByProject = function (ProjectId) {
        return $http.get("/Project/Defination/GetScopeByProject?ProjectId=" + ProjectId);
    };

    this.getMarketByProject = function (ProjectId) {
        return $http.get("/Project/Defination/GetMarketByProject?ProjectId=" + ProjectId);
    };

//-----------------------------------
    this.AddProjectConfig = function (scope) {
        var response = $http({
            method: "post",
            url: "/Project/Defination/AddProjectConfig",
            data: JSON.stringify(
                {
                    projectId: scope[0].ProjectID,
                    TypeId: scope[1].TypeId,
                    configIds: scope[2].ConfigurationIds
                }),
            dataType: "json"
        });
        return response;
    }
//----------------------------
    //this.AddMarkets = function (market) {
    //    var response = $http({
    //        method: "post",
    //        url: "/Project/Defination/AddMarkets",
    //        data: JSON.stringify(
    //            {
    //                projectId: market[0].ProjectID,
    //                DefinationTypeId: scope[1].DefinationTypeId,
    //                MarketIds: market[2].MarketID
    //            }),
    //        dataType: "json"
    //    });
    //    return response;
    //}
//-------------------------  
});