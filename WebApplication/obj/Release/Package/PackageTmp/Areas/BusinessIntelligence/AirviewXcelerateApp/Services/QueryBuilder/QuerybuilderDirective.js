

var queryBuilder = angular.module('queryBuilder', []);
queryBuilder.directive('queryBuilder', function ($compile, $http, $timeout, $rootScope) {
    return {
        restrict: 'E',
        scope: {
            group: '='
        },
        templateUrl: 'dsrpt/DashboardNReport/getPartialPage/QueryBuilder/QueryBuilderDirective.cshtml',
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
                // Fiedfill();

                scope.CallMe = function (data) {
                    scope.fields = [];
                    $rootScope.testing = data;
                    scope.fields = $rootScope.testing;

                };

                scope.$on('addColumnInWhereClauseB', function (type, data) {
                    // console.log(data);
                    scope.CallMe(data);
                });

                scope.stringconditions = [
                    { name: '=' },
                    { name: '<>' },
                    { name: 'LIKE' },
                    { name: 'NOT LIKE' },
                    { name: 'IS NULL' },
                    { name: 'IS NOT NULL' },
                { name: 'Drilldown' }
                ];
                
                scope.dateconditions = [
                    { name: '=' },
                    { name: '<>' },
                    { name: '>' },
                    { name: '<' },
                    { name: '>=' },
                    { name: '<=' },
                    { name: 'IS NULL' },
                    { name: 'IS NOT NULL' },
                    { name: 'BETWEEN' },
                    { name: 'Drilldown' }

                ];

                scope.intcondtion = [
                    { name: '=' },
                    { name: '<>' },
                    { name: '>' },
                    { name: '<' },
                    { name: '>=' },
                    { name: '<=' },
                    { name: 'LIKE' },
                    { name: 'NOT LIKE' },
                    { name: 'IS NULL' },
                    { name: 'IS NOT NULL' },
                    { name: 'BETWEEN' },
                    { name: 'Drilldown' }
                ];
                if (moduleInformation.ModuleType) {
                    let emsRefId = { name: '{EMSRefId}' };
                    scope.stringconditions.push(emsRefId);
                    scope.dateconditions.push(emsRefId);
                    scope.intcondtion.push(emsRefId);
                }
                scope.setCondition = function (value, index) {
                    //if (value != "string") {
                    //    scope.group.rules[index].isString = false;
                    //}
                    //else {
                    //    scope.group.rules[index].isString = true;
                    //}
                };
                scope.conditionError = false;
                scope.addCondition = function () {
                    scope.group.rules.push({
                        condition: '=',
                        field: '',
                        data: '',
                        databetween: '',
                        isoperator: true,
                        removefirstcondition: true,
                        operator: '',
                        type: 'System.String',
                        definition: []
                    });
                    scope.fields = $rootScope.testing;

                };

                scope.addType = function (index) {
                    scope.group.rules[index].data = '';
                    scope.definationType = scope.group.rules[index].field;
                    scope.type = filterdata(scope.group.rules[index].field);

                    scope.group.rules[index].condition = '=';
                    //scope.setCondition(scope.group.rules[index].type, index);
                    scope.group.rules[index].type = scope.type;
                    scope.getFilters(index);
                };

                scope.getFilters = function (index) {
                    if (scope.definationType.indexOf('.') > -1) {
                        scope.definationType = scope.definationType.split('.')[1];
                    }
                    if (scope.definationType == 'Market') {
                        scope.definationType = 'City';
                    }
                    //    console.log(scope.definationType);

                    if (scope.group.rules[index].type == 'select') {
                        $http({ method: 'GET', url: "/BusinessIntelligence/QueryBuilder/GetFilters?Type=" + scope.definationType }).then(function (result) {

                            //  console.log(result.data);
                            scope.group.rules[index].definition = result.data;
                            // console.log(scope.group.rules[index].definition);
                        }, function (result) {
                            alert("Error: No data returned");
                        });
                    }
                };

                var idx = scope.group.rules.length;
                scope.removeCondition = function (index) {
                    scope.group.rules.splice(index, 1);
                };

                scope.addGroup = function () {
                    scope.group.rules.push({
                        group: {
                            operator: 'AND',
                            rules: [],
                            removefirstcondition: true
                        }
                    });
                };


                function filterdata(item) {

                    for (var i = 0; i < scope.fields.length; i++) {

                        if (scope.fields[i].name == item) {
                            return scope.fields[i].DataType;

                            //if (scope.fields[i].DisplayType != "") {
                            //    return scope.fields[i].DisplayType;
                            //}
                            //else {
                            //    return scope.fields[i].DataType;
                            //}

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

