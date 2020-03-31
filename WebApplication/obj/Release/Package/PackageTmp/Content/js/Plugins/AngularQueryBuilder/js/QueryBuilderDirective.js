//var queryBuilder = angular.module('queryBuilder', ['ui.bootstrap']);
//queryBuilder.directive('queryBuilder', function ($compile, $http, $timeout, $rootScope, $modal) {
//    return {
//        restrict: 'E',
//        scope: {
//            group: '='
//        },
//        templateUrl: '/Content/js/Plugins/AngularQueryBuilder/html/queryBuilderDirective.html',
//        //controller: function ($scope) {
//        //    var self = {};

//        //$scope.CallMe = function (data) {
//        //    alert(data);
//        //};

//        //$rootScope.$on('callDirective', function (type, data) {
//        //    $scope.CallMe(data);
//        //});
//        //},
//        compile: function (element, attrs) {

//            var content, directive;
//            content = element.contents().remove();
//            return function (scope, element, attrs) {
//                scope.operators = [
//                    { name: 'AND' },
//                    { name: 'OR' }
//                ];
//                scope.fields = [];
//                function Fiedfill() {

//                    scope.fields = [];
//                    //for (var i = 0; i < $rootScope.testing.length; i++) {
//                    //    scope.fields.push({ name: $rootScope.testing[i].DefinationName, type: $rootScope.testing[i].InputType })
//                    //}
//                    scope.fields = $rootScope.testing;

//                }
//                Fiedfill();
//                //scope.fields = [
//                //    { name: 'Firstname', type: 'name' },
//                //    { name: 'Age', type: 'number' },
//                //    { name: 'Birthdate', type: 'date' },
//                //    { name: 'City', type: 'name' },
//                //    { name: 'Country', type: 'name' },
//                //    { name: 'datelimit', type: 'name' }
//                //];

//                scope.conditions = [
//                    { name: '=', type: "string" },
//                    { name: 'isnull', type: "string" },
//                    { name: '>', type: "int" },
//                    { name: '>=', type: "int" },
//                    { name: '<', type: "int" },
//                    { name: '!=', type: "string" }
//                ];
//                scope.CallMe = function (data) {
//                    scope.fields1 = data
//                    scope.fields = data

//                };

//                $rootScope.$on('callDirective', function (type, data) {
//                    scope.CallMe(data);
//                });
//                scope.conditionError = false;
//                scope.addCondition = function () {
//                    scope.group.rules.push({
//                        condition: '=',
//                        field: ' ',
//                        data: '',
//                        operator: '',
//                        zeep: true,
//                        isoperator: true,
//                        isgroupoperator: true,
//                        removefirstcondition: true,
//                        isshow: false,
//                        type: 'string',
//                        groupstart: false,
//                        aftercondition: false,
//                        definition: [],
//                        isString: true

//                    });
//                    Fiedfill();
//                    var idx = scope.group.rules.length;
//                    if (idx > 1) {
//                        if (scope.group.rules[idx - 2].data == "" && scope.group.rules[idx - 2].data == "") {
//                            scope.conditionError = true;
//                            scope.group.rules.splice(idx - 1, 1);
//                            Notify("Plese fill All fields before adding new row");
//                            //$("#notification").fadeIn("slow");//.append(' your message');
//                            //$(".dismiss").click(function () {
//                            //    $("#notification").fadeOut("slow");
//                            //});
//                            //$("#notification").fadeIn("slow");

                            
                            


//                            return;                      
//                        }
//                    }



//                    for (var i = 0; i < scope.group.rules.length; i++) {

//                        if (scope.group.rules[i].group) {
//                            try {
//                                if (scope.group.rules[i + 1].field) {
//                                    scope.group.rules[i + 1].aftercondition = true;
//                                }

//                            } catch (e) {

//                            }

//                        }

//                    }
//                };


//                $(".alert").find(".close").on("click", function (e) {
//                    e.stopPropagation();
//                    e.preventDefault();
//                    $(this).closest(".alert").slideUp(400);
//                });


//                function getRandomColor() {
//                    var letters = 'BCDEF'.split('');
//                    var color = '#';
//                    for (var i = 0; i < 6; i++) {
//                        color += letters[Math.floor(Math.random() * letters.length)];
//                    }
//                    return color;
//                }
//                var count = 0;
//                scope.addGroup = function (flag) {

//                    scope.group.rules.push({
//                        group: {
//                            operator: 'AND',
//                            rules: [],
//                            removefirstcondition: true,
//                            firstgroup: false,
//                            mainclass: flag

//                        }

//                    });
//                    //Fiedfill();
//                    $timeout(function () {
//                        if (flag == 'm') {
//                            debugger
//                            Fiedfill();
//                            $('.removeborber').last().addClass('MainGroup');
//                        }
//                    }, 100);
//                };
//                scope.submit = function () {
//                    alert()
//                    scope.group.rules = [];
//                    //$modalInstance.dismiss('cancel');
//                }
//                //scope.ClearAllGroup = function () {
//                scope.open = function () {
//                    //
//                    $modal.open({
//                        templateUrl: 'myModalContent.html',
//                        backdrop: true,
//                        windowClass: 'modal',
//                        controller: function ($scope, $modalInstance, $log) {

//                            $scope.submit = function (flag) {
//                                if (flag == 'delete') {
//                                    scope.group.rules = [];
//                                    $modalInstance.dismiss('cancel');
//                                }

//                            }
//                            $scope.cancel = function (flag) {
//                                if (flag == 'cancil') {
//                                    $modalInstance.dismiss('cancel');
//                                }

//                            };
//                        }
//                    });
//                };
//                //if (confirm("Are you sure?")) {
//                //    scope.group.rules = [];
//                //}

//                //}
//                scope.Edit = function (data) {
//                    data.zeep = true
//                    data.isedit = true;
//                }

//                scope.removeCondition = function (index) {
//                    scope.group.firstgroup = false;
//                    var idx = scope.group.rules.length;


//                    if (index == idx - 1) {
//                        scope.group.rules[index - 1].operator = '';
//                        scope.group.rules[index - 1].isoperator = true;
//                    }
//                    scope.group.rules.splice(index, 1);


//                };
//                scope.addType = function (index) {

//                    scope.definationType = scope.group.rules[index].field
//                    scope.Type = filterdata(scope.group.rules[index].field);
//                    scope.getFilters(index);
//                    scope.setCondition(scope.group.rules[index].type, index)
//                    scope.group.rules[index].type = scope.Type;


//                }
//                scope.datecondition = [
//                     { name: '=' },
//                    { name: 'isnull' },
//                    { name: '>' },
//                    { name: '>=' },
//                    { name: '<' },
//                    { name: '!=' },
//                    { name: 'between' },
//                ];

//                scope.intcondtion = [
//                    { name: '=', },
//                    { name: 'isnull' },
//                    { name: '>' },
//                    { name: '>=' },
//                    { name: '<' },
//                    { name: '!=' }
//                ];
//                scope.setCondition = function (value, index) {
//                    if (value != "string") {
//                        scope.group.rules[index].isString = false;
//                    }
//                }
//                scope.getFilters = function (index) {
//                    if (scope.definationType == 'Market') {
//                        scope.definationType = 'City'
//                    }
//                    $http({ method: 'GET', url: "/ReportTest/GetFilters?Type=" + scope.definationType }).then(function (result) {
//                        scope.group.rules[index].definition = result.data;
//                    }, function (result) {
//                        alert("Error: No data returned");
//                    });
//                }

//                function filterdata(item) {
//                    for (var i = 0; i < scope.fields.length; i++) {
//                        if (scope.fields[i].name == item) {
//                            if (scope.fields[i].DisplayType != "") {
//                                return scope.fields[i].DisplayType;
//                            }
//                            else {
//                                return scope.fields[i].DataType;
//                            }
//                        }
//                    }
//                }

//                scope.removeGroup = function () {

//                    "group" in scope.$parent && scope.$parent.group.rules.splice(scope.$parent.$index, 1);

//                    //This is for removing operator after group deletion.
//                    for (var i = 0; i < scope.$parent.group.rules.length; i++) {

//                        try {
//                            if (scope.$parent.group.rules[i].field) {
//                                if (scope.$parent.group.rules[i - 1].group == undefined) {
//                                    scope.$parent.group.rules[i].aftercondition = false;

//                                }

//                            }
//                        } catch (e) {
//                            scope.$parent.group.rules[i].aftercondition = false;

//                        }

//                        try {
//                            if (!scope.$parent.group.rules[i].group) {
//                                scope.$parent.group.rules[i].groupstart = false
//                            }


//                        } catch (e) {

//                        }
//                    }

//                };

//                directive || (directive = $compile(content));

//                element.append(directive(scope, function ($compile) {

//                    return $compile;
//                }));
//            }
//        }
//    }
//});



var queryBuilder = angular.module('queryBuilder', ['ui.bootstrap']);
queryBuilder.directive('queryBuilder', function ($compile, $http, $timeout, $rootScope, $modal) {
    return {
        restrict: 'E',
        scope: {
            group: '='
        },
        templateUrl: '/Content/js/Plugins/AngularQueryBuilder/html/queryBuilderDirective.html',
        compile: function (element, attrs) {
            var content, directive;
            content = element.contents().remove();
            return function (scope, element, attrs) {
                scope.operators = [
                    { name: 'AND' },
                    { name: 'OR' }
                ];

                scope.fields = [];
                function Fiedfill() {
                    scope.fields = [];
                    scope.fields = $rootScope.testing;
                }
                Fiedfill();

                scope.CallMe = function (data) {
                                        scope.fields1 = data
                                        scope.fields = data

                                    };

                                    $rootScope.$on('callDirective', function (type, data) {
                                        scope.CallMe(data);
                                    });

                scope.conditions = [
                    { name: '=' },
                    { name: '<>' },
                    { name: '<' },
                    { name: '<=' },
                    { name: '>' },
                    { name: '>=' }
                ];

                scope.datecondition = [
                     { name: '=' },
                    { name: 'isnull' },
                    { name: '>' },
                    { name: '>=' },
                    { name: '<' },
                    { name: '!=' },
                    { name: 'between' },
                ];

                scope.intcondtion = [
                    { name: '=', },
                    { name: 'isnull' },
                    { name: '>' },
                    { name: '>=' },
                    { name: '<' },
                    { name: '!=' }
                ];

                scope.setCondition = function (value, index) {
                                        if (value != "string") {
                                            scope.group.rules[index].isString = false;
                                        }
                                    }
                scope.conditionError = false;
                scope.addCondition = function () {
                    scope.group.rules.push({
                        condition: '=',
                        field: 'Firstname',
                        data: '',
                        isoperator: true,
                        removefirstcondition: true,
                        operator: '',
                        type: 'string',
                        isString: true,
                        definition: []
                    });
                };

                scope.addType = function (index) {

                                        scope.definationType = scope.group.rules[index].field
                                        scope.Type = filterdata(scope.group.rules[index].field);
                                        scope.getFilters(index);
                                        scope.setCondition(scope.group.rules[index].type, index)
                                        scope.group.rules[index].type = scope.Type;


                }

                scope.getFilters = function (index) {
                                        if (scope.definationType == 'Market') {
                                            scope.definationType = 'City'
                                        }
                                        $http({ method: 'GET', url: "/ReportTest/GetFilters?Type=" + scope.definationType }).then(function (result) {
                                            scope.group.rules[index].definition = result.data;
                                        }, function (result) {
                                            alert("Error: No data returned");
                                        });
                                    }

                var idx = scope.group.rules.length;
                                    if (idx > 1) {
                                        if (scope.group.rules[idx - 2].data == "" && scope.group.rules[idx - 2].data == "") {
                                            scope.conditionError = true;
                                            scope.group.rules.splice(idx - 1, 1);
                                            Notify("Plese fill All fields before adding new row");
                                          
                                            return;                      
                                        }
                                    }

                scope.removeCondition = function (index) {
                    scope.group.rules.splice(index, 1);
                };

                scope.addGroup = function () {
                    scope.group.rules.push({
                        group: {
                            operator: 'AND',
                            rules: [],
                            removefirstcondition: true,
                        }
                    });
                };


                function filterdata(item) {
                                        for (var i = 0; i < scope.fields.length; i++) {
                                            if (scope.fields[i].name == item) {
                                                if (scope.fields[i].DisplayType != "") {
                                                    return scope.fields[i].DisplayType;
                                                }
                                                else {
                                                    return scope.fields[i].DataType;
                                                }
                                            }
                                        }
                                    }

                $(".alert").find(".close").on("click", function (e) {
                                        e.stopPropagation();
                                        e.preventDefault();
                                        $(this).closest(".alert").slideUp(400);
                                    });

                scope.removeGroup = function () {
                    "group" in scope.$parent && scope.$parent.group.rules.splice(scope.$parent.$index, 1);
                };

                directive || (directive = $compile(content));

                element.append(directive(scope, function ($compile) {
                    return $compile;
                }));
            }
        }
    }
});

