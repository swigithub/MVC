var app = angular.module('ProjectManagment', []);

app.service('service', function ($q, $compile, $http) {

    // Get Data
    this.post = function (url, param) {
        $(".spinner").show();
        //var promise = $http.post(constants.mainPath + url, param);
        var promise = $http.post(url, param);
        promise = promise.then(function (response) {
            $(".spinner").hide();
            return response.data;

        });
        return promise;
    };

    this.get = function (url) {
        $(".spinner").show();
        //var promise = $http.post(constants.mainPath + url, param);
        var promise = $http.get(url);
        promise = promise.then(function (response) {
            $(".spinner").hide();
            return response.data;

        });
        return promise;
    };

});

app.controller('DefinationList', function ($scope,$filter ,$http, service) {

    $scope.Definations = [];
    $scope.testValues = [];
    $scope.TotalEntries = 0;
    $scope.ShowEntries = -1;

    service.post("/project/defination/ToList", { Filter: 'ByStatus', Value: true }).then(function (res) {
        $scope.Definations = res;
        $scope.testValues = $scope.Definations;
        $scope.TotalEntries = $scope.testValues.length;
        $scope.ShowEntries = 0;
       // console.log($scope.Definations);
    });
    $scope.rowsPerPage = 10;
    $scope.rowsPPage = 10;
    $scope.currentPage = 0;
    $scope.pageSize = 10;
    $scope.getData = [];


    $scope.totalPages = 0;
   
    $scope.q = '';
    function countWatchers() {
        var root = angular.element(document).injector().get('q');
        var count = root.$$watchers ? root.$$watchers.length : 0; // include the current scope
        var pendingChildHeads = [root.$$childHead];
        var currentScope;

        while (pendingChildHeads.length) {
            currentScope = pendingChildHeads.shift();

            while (currentScope) {
                count += currentScope.$$watchers ? currentScope.$$watchers.length : 0;
                pendingChildHeads.push(currentScope.$$childHead);
                currentScope = currentScope.$$nextSibling;
            }
        }

        return count;
    }

    $scope.$watch('q', function (newValue, oldValue) {
        if (oldValue != newValue) {
            $scope.currentPage = 1;
            $scope.ShowEntries = 0;
            //var r = countWatchers();
            //console.log(r);
        }
    }, true);
    $scope.getData = function () {

        return $filter('filter')($scope.testValues, $scope.q)
    }
  
    $scope.numberOfPages = function () {
        $scope.totalPages = Math.ceil($scope.getData().length / $scope.pageSize);
        return Math.ceil($scope.getData().length / $scope.pageSize);
    }

    $scope.dicrement = function (i) {
        if (i != 0) {
            $scope.currentPage = $scope.currentPage - 1
        }
    
    };
    $scope.increment = function (i) {
        $scope.numberOfPages();
        if (i < $scope.totalPages - 1) {
            $scope.currentPage = $scope.currentPage + 1

        }
    };
    $scope.pagesizes = function(i) {
        if (i != 0) {
            $scope.rowsPerPage = i;
            if (i > 1000) {
                $scope.rowsPPage = "All";
            } else {
                $scope.rowsPPage = i;
}
            $scope.ShowEntries = 0;
        }
    };
    $scope.lastPages = function () {
        if ($scope.ShowEntries != $scope.TotalEntries - $scope.rowsPerPage) {
            $scope.add = $scope.rowsPerPage-($scope.TotalEntries % $scope.rowsPerPage)
            $scope.ShowEntries = $scope.TotalEntries - $scope.rowsPerPage + $scope.add -1;
        }
    }
    $scope.FirstPage = function () {
            $scope.ShowEntries = 0;
    }
    $scope.AnyPage = function (i) {
        $scope.ShowEntries = i * $scope.rowsPerPage;
    }
    $scope.NextPages = function () {
        if ($scope.ShowEntries + $scope.rowsPerPage <= $scope.TotalEntries) {
            $scope.ShowEntries = $scope.ShowEntries + $scope.rowsPerPage
        }
    }
    $scope.PriousPages = function () {
        if ($scope.ShowEntries > 0) {
            if (($scope.ShowEntries + $scope.rowsPerPage) <= $scope.TotalEntries) {
                $scope.ShowEntries = $scope.ShowEntries - $scope.rowsPerPage;
            } else {
                $scope.ShowEntries = $scope.ShowEntries-$scope.rowsPerPage;
}
        }
    }
    $scope.showeentries = function (data) {
        if (data < $scope.rowsPerPage) {
            if (data != 0) {
                $scope.ShowEntries = 0;
            }
            else {
                if($scope.TotalEntries !=0){
                    $scope.ShowEntries = -1;
                }
            }
            return data;

        }else{
        if ($scope.ShowEntries + $scope.rowsPerPage <= $scope.TotalEntries) {
            return $scope.ShowEntries + $scope.rowsPerPage;
        } else {
            return $scope.TotalEntries;
}
        }
    }

});

app.filter('paginate', function(Paginator) {
    return function(input, rowsPerPage) {
        if (!input) {
            return input;
        }

        if (rowsPerPage) {
            Paginator.rowsPerPage = rowsPerPage;
        }
            
        Paginator.itemCount = input.length;

        return input.slice(parseInt(Paginator.page * Paginator.rowsPerPage), parseInt((Paginator.page + 1) * Paginator.rowsPerPage + 1) - 1);
    }
})

.filter('forLoop', function() {
    return function(input, start, end) {
        input = new Array(end - start);
        for (var i = 0; start < end; start++, i++) {
            input[i] = start;
        }

        return input;
    }
})

.service('Paginator', function ($rootScope) {
    this.page = 0;
    this.rowsPerPage = 50;
    this.itemCount = 0;
    this.limitPerPage = 5;

    this.setPage = function (page) {
        if (page > this.pageCount()) {
            return;
        }

        this.page = page;
    };

    this.nextPage = function () {
        if (this.isLastPage()) {
            return;
        }

        this.page++;
    };

    this.perviousPage = function () {
        if (this.isFirstPage()) {
            return;
        }

        this.page--;
    };

    this.firstPage = function () {
        this.page = 0;
    };

    this.lastPage = function () {
        this.page = this.pageCount() - 1;
    };

    this.isFirstPage = function () {
        return this.page == 0;
    };

    this.isLastPage = function () {
       
        return this.page == this.pageCount() - 1;
        
    };

    this.pageCount = function () {
        return Math.ceil(parseInt(this.itemCount) / parseInt(this.rowsPerPage));
    };
        
    this.lowerLimit = function() { 
        var pageCountLimitPerPageDiff = this.pageCount() - this.limitPerPage;
            
        if (pageCountLimitPerPageDiff < 0) { 
            return 0; 
        }
            
        if (this.page > pageCountLimitPerPageDiff + 1) { 
            return pageCountLimitPerPageDiff; 
        } 
            
        var low = this.page - (Math.ceil(this.limitPerPage/2) - 1); 
            
        return Math.max(low, 0);
    };
})

.directive('paginator', function factory() {
    return {
        restrict:'E',
        controller: function ($scope, Paginator) {
            $scope.paginator = Paginator;
        }
    };
});


//app.controller('MyCtrl', ['$scope', '$filter', function ($scope, $filter) {
//    $scope.currentPage = 0;
//    $scope.pageSize = 10;
//    $scope.data = [];
//    $scope.q = '';

   
//}]);

app.filter('startFrom', function () {
    return function (input, start) {
        start = +start;
        return input.slice(start);
    }
});
