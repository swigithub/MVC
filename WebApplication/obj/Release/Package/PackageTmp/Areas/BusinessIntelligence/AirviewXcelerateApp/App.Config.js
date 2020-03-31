'use strict';
var draggableOnMouseup = false;
var airviewXcelerateAppConfig = airviewXcelerateApp.config(["$stateProvider", "$urlRouterProvider", "$locationProvider", "toastrConfig", "$httpProvider", "ivhTreeviewOptionsProvider", function ($stateProvider, $urlRouterProvider, $locationProvider, toastrConfig, $httpProvider, ivhTreeviewOptionsProvider) {
  
    
  
    //for (var key in getStateSettings) {
    //    $stateProvider.state(getStateSettings[key]);

    //}
    //$urlRouterProvider.otherwise('/Xcelerate');
    //$locationProvider.hashPrefix('!');
   
    angular.extend(toastrConfig, {
        autoDismiss: false,
        containerId: 'toast-container',
        maxOpened: 0,
        newestOnTop: true,
        positionClass: 'toast-bottom-left',
        preventDuplicates: false,
        preventOpenDuplicates: false,
        target: 'body'
    });
    ivhTreeviewOptionsProvider.set({
        defaultSelectedState: true,
        validate: true,
        twistieExpandedTpl: '(-)',
        twistieCollapsedTpl: '(+)',
        twistieLeafTpl: ''
    });
   

    //$httpProvider.interceptors.push(function ($q) {
    //    return {
    //        'request': function (config) {
    //            console.log('start');
    //            return config;
    //        },

    //        'response': function (response) {
    //            console.log('end');
    //            return response;
    //        }
    //    };
    //});
}]);

airviewXcelerateApp.factory('getTemplate', function () {
    var item = function () {
        var itemchil = function () {

        }
        return {
            doSomethingWithModel: function () {
                return "yes";
                //return $.get('/Xcelerate/Home/getTemplate', { id: 10 }, function (res, status, xhr) {

                //    return res;
                //});

            }

        }
    }();
    return item;
});




airviewXcelerateApp.run(['$rootScope', "$uibModalStack", "$state", "$stateParams", function ($rootScope, $uibModalStack, $state, $stateParams) {
    $rootScope.$state = $state;
   
    $rootScope
        .$on('$stateChangeStart',
            function (event, toState, toParams, fromState, fromParams) {
                //   console.log("State Change: transition begins!");
            });

    $rootScope
        .$on('$stateChangeSuccess',
            function (event, toState, toParams, fromState, fromParams) {
               $uibModalStack.dismissAll();

                // console.log("State Change: State change success!");
            });

    $rootScope
        .$on('$stateChangeError',
            function (event, toState, toParams, fromState, fromParams) {
                //  console.log("State Change: Error!");
            });

    $rootScope
        .$on('$stateNotFound',
            function (event, toState, toParams, fromState, fromParams) {
                // console.log("State Change: State not found!");
            });
  

}]);

var getMessage = function () {
    return "that was first request";
}