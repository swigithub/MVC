(function (l) {
    l.module("angularTreeview", []).directive("treeModel",
        function($compile, $rootScope) {
            return {
                restrict: "A",
                link: function(b, h, c) {
                    var a = c.treeId,
                        g = c.treeModel,
                        e = c.nodeLabel || "SectionTitle",
                        d = c.nodeChildren || "Sections",
                        e = '<ul><li data-ng-repeat="node in ' +
                            g +
                            '"><i class="collapsed" data-ng-show="node.' +
                            d +
                            '.length && node.collapsed" data-ng-click="' +
                            a +
                            '.selectNodeHead(node)"></i><i class="expanded" data-ng-show="node.' +
                            d +
                            '.length && !node.collapsed" data-ng-click="' +
                            a +
                            '.selectNodeHead(node)"></i><i class="normal" data-ng-hide="node.' +
                            d +
                            '.length"></i> <span data-ng-class="node.selected" data-ng-click="' +
                            a +
                            '.selectNodeChange(node)">{{node.' +
                            e +
                            '}}</span><div data-ng-hide="node.collapsed" data-tree-id="' +
                            a +
                            '" data-tree-model="node.' +
                            d +
                            '" data-node-id=' +
                            (c.nodeId || "id") +
                            " data-node-label=" +
                            e +
                            " data-node-children=" +
                            d +
                            "></div></li></ul>";
                    a &&
                        g &&
                        (c.angularTreeview &&
                        (b[a] = b[a] || {}, b[a].selectNodeHead =
                            b[a].selectNodeHead || function(a) { a.collapsed = !a.collapsed }, b[a].selectNodeLabel =
                            b[a].selectNodeLabel ||
                            function(c) {
                                b[a].currentNode && b[a].currentNode.selected && (b[a].currentNode.selected = void 0);
                                $rootScope.$broadcast('someEvent', [1, 2, 3]);
                                console.log(b[a]);
                                c.selected = "selected";
                                b[a].currentNode = c;
                            }, b[a].selectNodeChange = b[a].selectNodeChange ||
                            function(c) {
                                b[a].currentNode && b[a].currentNode.selected && (b[a].currentNode.selected = void 0);
                                c.selected = "selected";
                                b[a].currentNode = c;
                                $rootScope.$broadcast('changeNode', c);
                                console.log(b[a]);
                            }), h.html('').append($compile(e)(b)));
                }
            }
        });
})(angular);

var app = angular.module('TSS', ['ui.multiselect', 'angular.filter', 'ui.tree']);

app.run(function ($rootScope) {
    $rootScope.ishide = false;
    $rootScope.Sections = {};
    $rootScope.SectionsForTree = {};
    $rootScope.UnitTypes = {};
    $rootScope.SurveyId = SurveyId;
    $rootScope.SelectedUnitSystemId = 0;
    $rootScope.AllClients = [];


});

app.service('service', function ($q, $compile, $http) {
    this.post = function (url, param) {
        var promise = $http.post(url, param);
        promise = promise.then(function (response) {
            return response.data;

        });
        return promise;
    };

    this.get = function (url) {
        var promise = $http.get(url);
        promise = promise.then(function (response) {
            return response.data;

        });
        return promise;
    };

});

//------------------------------
app.factory('Comman', function ($http, $rootScope) {
    var GetSections =
        function (tempSurveyId) {
           //   //debugger;
            var Result = $http({
                method: "get",
                url: "/survey/section/list?value=" + tempSurveyId,
            }).then(function success(res) {
               // //debugger;
                Section = res.data.list2;
                $rootScope.Sections = res.data.list2;
                $rootScope.SectionsForTree = res.data.list1;
                console.log($rootScope.SectionsForTree);
                //console.log(res.data);
                return res.data.list2;
            });
            return Result;
        }

    var LoadUnitTypes = function (tempUnitSystemId) {
        if (tempUnitSystemId != undefined) {
            var Result = $http({
                url: "/Defnation/ToList?filter=PDefinationId&value=" + tempUnitSystemId
            }).then(function success(res2) {
                $rootScope.UnitTypes = res2.data;
                return res2.data
            })
            return Result;
        }
    }
    return { GetSections: GetSections, LoadUnitTypes: LoadUnitTypes };
});
//------------------------------------
app.controller('Document', function ($scope, $http, Comman, $rootScope, $window, $location, service) {

    $scope.objDoc = {};


    $scope.clients = []
    $scope.cities = []
    $scope.Categories = {};
    $scope.SubCategories = {};
    $scope.UnitSystemId = {};


    //$scope.LoadClients = function (data) {
    //    $http({
    //        url: "/Survey/Document/Clients"
    //    }).then(function success(res) {
    //        $scope.clients = res.data;
    //    });
    //}

    $scope.LoadClients = function (data) {
        service.get("/Client/ToList?filter=UserClients&value=" + UserId).then(function (res) {
            $scope.clients = res;
            $rootScope.AllClients = res;
        });
    }



    $scope.LoadCities = function (data) {
        service.get("/Defnation/ToList?filter=UserCities&value=" + UserId).then(function (res) {
            $scope.cities = res;
        });
        //$http({
        //    url: "/Survey/Document/Cities"
        //}).then(function success(res) {
        //    $scope.cities = res.data;
        //});
    }

    $scope.LoadSurvey = function () {
        $http({
            url: "/Survey/Document/Edit/" + $rootScope.SurveyId
        }).then(function success(res) {
            debugger;
            $scope.objDoc = res.data;
            $scope.LoadSubCategories();
            $rootScope.ishide = true;
            Comman.GetSections($rootScope.SurveyId);
            $scope.LoadUnitSystem();

        });
    }

    $scope.LoadUnitSystem = function () {
        //  //debugger;
        $http({
            url: "/Defnation/ToList?filter=byDefinationType&value=Unit Stystem",
        }).then(function success(res) {
            $scope.UnitSystem = res.data;
            Comman.LoadUnitTypes($scope.objDoc.UnitSystemId);
            $rootScope.SelectedUnitSystemId = $scope.objDoc.UnitSystemId;

        });
    }

    $scope.LoadCommonUnitTypes = function () {
        Comman.LoadUnitTypes($scope.objDoc.UnitSystemId);
        $rootScope.SelectedUnitSystemId = $scope.objDoc.UnitSystemId;
    }


    $http({
        url: "/Defnation/ToList?filter=byDefinationType&value=Survey Category"
    }).then(function success(res) {
        $scope.Categories = res.data;
        if ($rootScope.SurveyId > 0) {
            $scope.LoadSurvey($scope.objDoc.UnitSystemId);
        }
    });

    $scope.LoadSubCategories = function () {
        $http({
            url: "/Defnation/ToList?filter=PDefinationId&value=" + $scope.objDoc.CategoryId
        }).then(function success(res) {
            $scope.SubCategories = res.data;
        });
    }




    $scope.LoadUnitSystem();
    $scope.LoadClients();
    $scope.LoadCities();
    $('#frm-survey').parsley();
    $scope.SubmitDocument = function () {

        $http({
            method: "post",
            url: "/Survey/Document/New",
            data: $scope.objDoc
        }).then(function Success(result) {
            var res = result.data;
            if (res !== 'session expired') {
                if (res.Status === 'success') {
                    $rootScope.SurveyId = res.Value;
                    $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                    $rootScope.ishide = true;

                    if ($scope.objDoc.SurveyId == undefined) {
                        //$(location).attr('href', $location.$$absUrl + '/' + res.Value);                                             
                    }
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

    $scope.GlobalChange=function(val)
    {
        if (val) {
            $scope.objDoc.ClientId = 0;
        }
    }
});

//-----------------------------------
app.controller('Section', function ($scope, $http, Comman, $rootScope)
{
    $scope.sec = {};
    $scope.sec.SurveyId = $rootScope.SurveyId;
    $scope.sec.IsActive = true;
    $scope.yello = ['TextBox', 'DropDownList', 'RadioButton', 'CheckBox'];

    $scope.options = {
        accept: function (sourceNodeScope, destNodesScope, destIndex) {
            sourceNodeScope.$element.attr('data-order');
            destNodesScope.$element.attr('data-order');
            var srctype = sourceNodeScope.$element.attr('parent-data');
            var dsttype = destNodesScope.$element.attr('parent-data');
            if (srctype === dsttype && srctype !== undefined) {
                return true;
            } else {
                return false;
            }

        },
        dropped: function () {
            $scope.SaveSectionsOrder();
        },
        beforeDrop: function (sourceNodeScope, destNodesScope, destIndex) {
          
        }
    };

    $scope.EditSection = function (item) {
        debugger;
        $("#CreateSectionli").addClass("active");
        $("#CreateQuestionLogicli").removeClass("active");
        $("#AddQuestion").hide();
        $("#AddQuestionli").removeClass("active");
        $("#CreateSection").show();
        $("#CreateQuestionLogic").hide();
        //debugger;
        var newCart = {};
        angular.forEach(item, function (value, key) {
            
            if (key == "PSectionId" && value == 0) {

            }
            else {

                newCart[key] = value;
            }
        });

        item = newCart;

        $scope.sec = item;
        $rootScope.TempSectionForEdit = item;
        //debugger;
        if ($rootScope.TempSectionsHolder == undefined) {
            $rootScope.TempSectionsHolder = [];
            angular.forEach($rootScope.Sections, function (val, key) {
                $rootScope.TempSectionsHolder.push(val);
            }); 
        }
        else {
            $rootScope.Sections = [];
            angular.forEach($rootScope.TempSectionsHolder, function (val, key) {
                $rootScope.Sections.push(val);
            });
           // $rootScope.Sections = $scope.TempSectionsHolder;
        }
       
        angular.forEach($rootScope.Sections, function (val, key) {

            if (val.SectionId == item.SectionId) {
                $rootScope.Sections.splice(key, 1);
            }
          
        });
       
      
    };
    $scope.test = function () {
        alert();
    };
   

    $scope.SectionEditIsActive = function (item) {
        // //debugger;
        Save(item, false);
    };

    $scope.clearFields = function () {
        debugger;
        //alert("Set S id= 0?");
        $rootScope.TempSectionForEdit = null;
        $scope.sec = {};
        $scope.sec.SectionId = 0;
        $rootScope.Sections = [];
        if ($rootScope.TempSectionsHolder != undefined &&  $rootScope.TempSectionsHolder.length > 0) {
            angular.forEach($rootScope.TempSectionsHolder, function (val, key) {
                $rootScope.Sections.push(val);
            });
        }
        //$scope.sec.SectionTitle = "";
        //$scope.sec.PSectionId = 0;
        //$scope.sec.Description = "";
    }

    $scope.DeleteSection = function (Id) {
        //debugger;
        if (Id > 0) {
            $.confirm({
                title: 'Confirm!',
                content: 'Do you want to Delete!',
                buttons: {
                    Yes: function () {
                        $http({
                            url: "/Survey/Section/Delete/" + Id
                        }).then(function success(result) {
                            var res = result.data;
                            if (res !== 'session expired') {
                                if (res.Status === 'success') {
                                    Comman.GetSections($scope.sec.SurveyId);
                                } else {
                                    $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                                }
                            } else {
                                $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            }

                        });
                    },
                    No: function () { },
                }
            });
        }

    };

    $scope.FromTemplate = function (Id) {
        window.location.href = "/Survey/Document/Index/" + $rootScope.SurveyId;
    };

    function Save(obj, message) {
        $scope.sec.SurveyId = $rootScope.SurveyId;
        //debugger;
        $http({
            method: "post",
            url: "/Survey/Section/New",
            data: obj,
            // headers: { 'Content-Type': undefined},
        }).then(function Success(result) {
            var res = result.data;
            if (res !== 'session expired') {
                if (res.Status === 'success') {
                    Comman.GetSections($scope.sec.SurveyId);
                    $scope.sec = {};
                    
                    if (message == true) {
                        // $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                    }

                } else {
                    $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }
            } else {
                $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
            }
        }, function myError(response) {
            $.notify(response.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
        });
    }

    $scope.SaveSectionsOrder = function () {

        ////debugger;
        $http({
            method: "POST",
            url: "/Survey/Document/SaveSectionsOrder",
            data: { sections: $rootScope.SectionsForTree },
            // headers: { 'Content-Type': undefined},
        }).then(function Success(result) {
           
        }, function myError(response) {
            $.notify("Internal Server Error", { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
        });
    }
    $scope.SaveQuestionsOrder = function () {

        //debugger;
        $http({
            method: "POST",
            url: "/Survey/Document/SaveQuestionsOrder",
            data: { questions: $scope.QuestionList },
            // headers: { 'Content-Type': undefined},
        }).then(function Success(result) {

        }, function myError(response) {
            $.notify("Internal Server Error", { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
        });
    }

    $('#frm-section').parsley();
    $scope.SubmitSection = function () {

        Save($scope.sec, true);
    };

    //$http({
    //    url: "/Survey/Section/Tree?Filter=By_SurveyId&Value=" + $rootScope.SurveyId
    //}).then(function success(res) {       
    //    $scope.List = res.data.treeList;
    //});

    ////watch for 
    //$scope.$watch('sec.SectionTitle', function (newValue, oldValue, scope) {
    //    //debugger;
    //    scope.sec.SectionTitle = newValue;
    //});

    $scope.questionTree = {
        dropped: function () {
            $scope.SaveQuestionsOrder();
            
        }
    }
    $scope.DeleteSection = function (item) {
        $.confirm({
            title: 'Confirm!',
            content: 'Do you want to Delete!',
            buttons: {
                Yes: function () {
                    $http({
                        url: "/Document/DeleteSection?sectionId=" + item.SectionId
                    }).then(function success(res) {
                        $.notify(res.data.Message, { type: "Please Select Option", color: "#ffffff", background: res.data.Status, blur: 0.6, delay: 0, });
                        Comman.GetSections($rootScope.SurveyId);
                        $scope.QuestionList = [];
                        $scope.sec = {};
                    }, function () {
                        $.notify("Internal Server Error", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
                    });
                },
                No: function () { },
            }
        });
       

    }
    $scope.GetStyle = function (item) {
        var result = "";
        if (item) {
            result = "Background-color:#fffdaf";
        }
        return result;
    }
});

//------------------------------------
app.controller('Question', function ($scope, $http, Comman, $filter, $rootScope, $timeout) {
  
    
    $scope.obj = {};
    $scope.IsEdit = false;
    $scope.Responses = [];
    $scope.QuestionList = [];
    $scope.$on('changeNode', function (event, data) {
        $scope.LoadQuestions(data);
    }
    );
    
    // Code For Table Type
    
    $scope.isColumnRows = false;
    $scope.isColumn = false;
    $scope.isRows = false;
    $scope.arrStringforDropDown = new Array();
    $scope.arrStringforCheckBox = new Array();
    $scope.arrString = new Array();
    $scope.arrStringForTextBox = new Array();
    $scope.ddItem = '';
    $scope.rbItem = '';
    $scope.cbItem = '';
    $scope.ddlSelected = '';
    $scope.rbSelected = '';
    $scope.cbItems = [];
    $scope.TableHeaderType = ['TextBox', 'DropDownList', 'RadioButton', 'CheckBox','Map'];

    

    $scope.ColumnChanged = function (data) {
        debugger;
        if (data != null) {
            $scope.isColumn = true;
            if ($scope.isColumn && $scope.isRows) {
                $scope.isColumnRows = true;
            }
            if (data < $scope.Responses.length) {
                $scope.Responses.length = data;

                if ($scope.arrStringforDropDown != undefined && $scope.arrStringforDropDown.length > 0) $scope.arrStringforDropDown.length = data;

                if ($scope.arrStringForTextBox != undefined && $scope.arrStringForTextBox.length > 0) $scope.arrStringForTextBox.length = data;

                if ($scope.arrStringforCheckBox != undefined && $scope.arrStringforCheckBox.length > 0) $scope.arrStringforCheckBox.length = data;

                if ($scope.arrString != undefined && $scope.arrString.length > 0) $scope.arrString.length = data;
                
            }
        }
        else {
            $scope.isColumn = false;
            $scope.isColumnRows = false;
        }
    }
    $scope.RowChanged = function (data) {
       // //debugger;
        if (data != null) {
            $scope.isRows = true;
            if ($scope.isColumn && $scope.isRows) {
                $scope.isColumnRows = true;
            }
            angular.forEach($scope.Responses, function (res, i) {
                if (res.ResponseValue == "TextBox") {
                    var Temp = res.UserValues.split(',');
                    if (Temp.length > data) {
                        Temp.length = data;
                        res.UserValues = Temp.join(',');
                    }
                    $scope.arrStringForTextBox[i] = Temp;
                }
                if (res.ResponseValue == "DropDownList") {
                    var Temp = res.UserValues.split(',');
                    if (Temp.length > data) {
                        Temp.length = data;
                        res.UserValues = Temp.join(',');
                    }


                }
                if (res.ResponseValue == "RadioButton") {
                    var Temp = res.UserValues.split(',');
                    if (Temp.length > data) {
                        Temp.length = data;
                        res.UserValues = Temp.join(',');
                    }

                }
                if (res.ResponseValue == "CheckBox") {
                    var Temp = res.UserValues.split(',');
                    if (Temp.length > data) {
                        Temp.length = data;
                        res.UserValues = Temp.join(',');
                    }

                }
            });
        }
        else {
            $scope.isRows = false;
            $scope.isColumnRows = false;
        }
    }

    $scope.DynamicRowsChanged = function (data, rows) {
      //  //debugger;
        if (data) {
            if (rows > 0) {
                $scope.isRows = true;
            }
            else {
                $scope.isRows = false;
            }
            if ($scope.isColumn) {
                $scope.isColumnRows = true;
                $scope.RowChanged(1);
                return 1;
            }
        }
        else if (data == false) {
            if ($scope.isColumn && $scope.isRows) {
                $scope.isColumnRows = true;
                $scope.RowChanged(1);
                return 1;
            } else {
                $scope.isColumnRows = false;
                return 0;
            }
        }

    }
    $scope.AddNewDropDownData = function (data, index) {
        //debugger;
        var a = angular.element(document).find('#ddItem');
        
        if (data != '' && data != undefined && data != null) {
            if ($scope.Responses[index].UserValues === undefined || $scope.Responses[index].UserValues=="") {

                $scope.Responses[index].UserValues = data;
                $scope.ddItem = '';
            }
            else {
                $scope.Responses[index].UserValues = $scope.Responses[index].UserValues + "," + data;
            }
            $scope.arrStringforDropDown[index] = $scope.Responses[index].UserValues.split(',');
            var a = angular.element(document).find('#ddItem');
            a[0].value = '';
            return '';
        }
    }

    $scope.AddRadioButtonItems = function (data, index) {
        debugger;
        var a = angular.element(document).find('#rbItem');
        
        if (data!="" && data!=undefined && data!=null) {
            if ($scope.Responses[index].UserValues === undefined || $scope.Responses[index].UserValues=="") {

                $scope.Responses[index].UserValues = data;
                $scope.rbItem = '';
            }
            else {
                $scope.Responses[index].UserValues = $scope.Responses[index].UserValues + "," + data;
            }
            $scope.arrString[index] = $scope.Responses[index].UserValues.split(',');
            
            a[0].value = '';

            return '';
        }
    }

    $scope.AddCheckBoxItems = function (data, index) {
        var a = angular.element(document).find('#cbItem');
        
        if (data != '' && data != undefined && data != null) {
            if ($scope.Responses[index].UserValues === undefined || $scope.Responses[index].UserValues=="") {

                $scope.Responses[index].UserValues = data;
                $scope.cbItem = '';
            }
            else {
                $scope.Responses[index].UserValues = $scope.Responses[index].UserValues + "," + data;
            }
            $scope.arrStringforCheckBox[index] = $scope.Responses[index].UserValues.split(',');
            var a = angular.element(document).find('#cbItem');
            a[0].value = '';

            return '';
        }
    }

    $scope.AddDataForTextBox = function (index, data, rowIndex) {
        if ($scope.Responses[index].UserValues != null & $scope.Responses[index].UserValues != undefined && $scope.Responses[index].UserValues != '') {
            $scope.TArray = $scope.Responses[index].UserValues.split(',');
            if (rowIndex > -1) {
                $scope.TArray.splice(rowIndex, 1, data);
                $scope.Responses[index].UserValues = '';
                angular.forEach($scope.TArray, function (value, key) {
                    if ($scope.Responses[index].UserValues == '') {
                        $scope.Responses[index].UserValues = value;
                    }
                    else {
                        $scope.Responses[index].UserValues = $scope.Responses[index].UserValues + "," + value;
                    }
                });
                $scope.arrStringForTextBox[index][rowIndex] = data;
                console.log($scope.Responses[index].UserValues);
            }
        }
        else {
            $scope.Responses[index].UserValues = data;
        }
    }
    $scope.SetDropDownItem = function (data) {
        $scope.ddlSelected = data;
    }
    $scope.RemoveDropDownItem = function (index) {
        //debugger;
        var i = $scope.arrStringforDropDown[index].indexOf($scope.ddlSelected);
        if (i != -1) {
            $scope.arrStringforDropDown[index].splice(i, 1);
            $scope.Responses[index].UserValues = '';
            angular.forEach($scope.arrStringforDropDown[index], function (value, key) {
                if ($scope.Responses[index].UserValues === '') {

                    $scope.Responses[index].UserValues = value;
                }
                else {
                    $scope.Responses[index].UserValues = $scope.Responses[index].UserValues + "," + value;
                }
            });
        }
    }

    $scope.SetRadioButtonItem = function (data) {
        $scope.rbSelected = data;
    }
    $scope.RemoveRadioButtonItem = function (index) {
        //debugger;
        var i = $scope.arrString[index].indexOf($scope.rbSelected);
        if (i != -1) {
            $scope.arrString[index].splice(i, 1);
            $scope.Responses[index].UserValues = '';
            angular.forEach($scope.arrString[index], function (value, key) {
                if ($scope.Responses[index].UserValues === '') {

                    $scope.Responses[index].UserValues = value;
                }
                else {
                    $scope.Responses[index].UserValues = $scope.Responses[index].UserValues + "," + value;
                }
            });

        }
        else {
            console.log('No Item Selected');
        }
    }

    $scope.SetCheckBoxItem = function (data, IsChecked) {
        if (IsChecked) {
            $scope.cbItems.push(data);
        } else {
            var toDel = $scope.cbItems.indexOf(data);
            $scope.cbItems.splice(toDel);
        }
    }
    $scope.RemoveCheckBoxItem = function (index) {
        //debugger;
        angular.forEach($scope.cbItems, function (value, key) {
            var i = $scope.arrStringforCheckBox[index].indexOf(value);
            if (i > -1) {
                $scope.arrStringforCheckBox[index].splice(i, 1);
            }
        });

        $scope.Responses[index].UserValues = '';
        angular.forEach($scope.arrStringforCheckBox[index], function (value, key) {
            if ($scope.Responses[index].UserValues === '') {

                $scope.Responses[index].UserValues = value;
            }
            else {
                $scope.Responses[index].UserValues = $scope.Responses[index].UserValues + "," + value;
            }
        });
    }

    $scope.DeleteColumn = function (index) {
        debugger;
        $.confirm({
            title: 'Confirm!',
            content: 'Are You Sure You Want To Delete This Column!',
            buttons: {
                Yes: function () {
                    if (index < $scope.Responses.length) {
                        try{
                            $scope.Responses.splice(index, 1);
                            $timeout($scope.obj.TotalColumn = $scope.obj.TotalColumn - 1);
                            if (index < $scope.Responses.length) {
                                angular.forEach($scope.Responses, function (value, key) {
                                    if (key >= index) {
                                        if ($scope.Responses[key].ResponseValue == "DropDownList") {
                                            if ($scope.arrStringforDropDown[key + 1] != null && $scope.arrStringforDropDown[key + 1] != undefined && $scope.arrStringforDropDown[key + 1] != "") {
                                                $scope.arrStringforDropDown[key] = $scope.arrStringforDropDown[key + 1];
                                                $scope.arrStringforDropDown.splice(key + 1, 1);

                                            }
                                        }
                                        else if ($scope.Responses[key].ResponseValue == "RadioButton") {

                                            if ($scope.arrString[key + 1] != null && $scope.arrString[key + 1] != undefined && $scope.arrString[key + 1] != "") {
                                                $scope.arrString[key] = $scope.arrString[key + 1];
                                                $scope.arrString.splice(key + 1, 1);
                                            }
                                        }
                                        else if ($scope.Responses[key].ResponseValue == "CheckBox") {
                                            if ($scope.arrStringforCheckBox[key + 1] != null && $scope.arrStringforCheckBox[key + 1] != undefined && $scope.arrStringforCheckBox[key + 1] != "") {
                                                $scope.arrStringforCheckBox[key] = $scope.arrStringforCheckBox[key + 1];
                                                $scope.arrStringforCheckBox.splice(key + 1, 1);
                                            }
                                        }
                                        else if ($scope.Responses[key].ResponseValue == "TextBox") {
                                            if ($scope.arrStringForTextBox[key + 1] != null && $scope.arrStringForTextBox[key + 1] != undefined && $scope.arrStringForTextBox[key + 1] != "") {
                                                $scope.arrStringForTextBox[key] = $scope.arrStringForTextBox[key + 1];
                                                $scope.arrStringForTextBox.splice(key + 1, 1);
                                            }
                                        }
                                    }
                                });
                            }
                            else {

                                var Name = $scope.Responses[index].ResponseValue;
                                if (Name == "DropDownList") $scope.arrStringforDropDown.splice(index, 1);
                                if (Name == "RadioButton") $scope.arrString.splice(index, 1);
                                if (Name == "CheckBox") $scope.arrStringforCheckBox.splice(index, 1);
                                if (Name == "TextBox") $scope.arrStringForTextBox.splice(index, 1);
                            }
                        }
                        catch (err) {
                           
                        }
                    }
                    else {
                        $timeout($scope.obj.TotalColumn = $scope.obj.TotalColumn - 1);
                    }
                },
                No: function () {
                },
            }
        });
       
    }


    // Code End For Table type

    $scope.clearFieldsForQuestions = function () {
        //debugger;
        $scope.isSeignleMultiselect = false;
        $scope.minmax = false;
        $scope.ResponseText = false;
        $scope.IsNumaric = false;
        $scope.InputResponseText = false;
        $scope.IsFile = false;
        $scope.IsPassed = false;
        $scope.obj = {};
        $scope.Responses = [];
        $scope.showValidations = false;
        $scope.SectionIdForTemplate = '';
        $scope.obj.Weightage = '1';
        $scope.arrStringforDropDown = [];
        $scope.arrString = [];
        $scope.arrStringforCheckBox = [];
        $scope.arrStringForTextBox = [];

    }
    function NewItem() {

        var newItem = $scope.Responses.length + 1;

        $scope.Responses.push({
            'id': 'res' + newItem
        });
        console.log($scope.Responses);
    }

    NewItem();

  

    $scope.ResponseNewRow = function () {
        if (event.keyCode === 9) {
            if ($scope.obj.QuestionTypeId != 103297 && $scope.obj.QuestionTypeId != 103298 && $scope.obj.QuestionTypeId != 103299) {
                NewItem();
            }
        }

        //else if (event.keyCode === 46) {
        //    var lastItem = $scope.Responses.length - 1;
        //    $scope.Responses.splice(lastItem);
        //}

    }



    $scope.DeleteResponse = function (item) {
        debugger;
        var id = item.id == undefined ? item.ResponseId : item.id;
        $.confirm({
            title: 'Confirm!',
            content: 'Do you want to Delete!',
            buttons: {
                Yes: function () {
                    debugger;
                    angular.forEach($scope.Responses, function (e, i) {
                        if ((e.id==undefined?e.ResponseId:e.id) == id) {
                           $timeout($scope.Responses.splice(i, 1));
                        }
                    });
                },
                No: function () { },
            }
        });
    }

    $scope.IsRepeatable = function () {

        var found = $filter('Definations').GetByKeyCode($scope.QuestionTypeId, "NUMERIC_INPUT");
        if (found != null) {

            $scope.obj.QuestionTypeId = found.DefinationId;
            $scope.ShowRespose($scope.obj.QuestionTypeId);

        }
        // console.log(found);

    }

    //------------------ 
    
    $scope.fillddlUnits = function () {
        $http({
            url: "/Defnation/ToList?filter=PDefinationId&value=" + $scope.obj.UnitTypeId
        }).then(function success(res) {
            $scope.UnitId = res.data;
        });
    }
    //-----------------

    //added by junaid for Preview

    $scope.LoadQuestionsForPreview = function (item, e) {
        $('.preview-options').find('.highlight-text').removeClass('highlight-text');
        $(e.currentTarget).addClass('highlight-text');
        $scope.LoadQuestions(item);
      
    }

    $scope.LoadQuestions = function (item) {
        //$scope.item = {}

        $http({
            url: "/Survey/Question/ToList?Filter=GET_BY_SECTIONID&Value=" + item.SectionId
        }).then(function success(res) {
            $scope.QuestionList = res.data;
            //console.log(res.data);
        });
    }

    $scope.LoadQuestionsForPreview = function (item, e) {
    
        $('.preview-options').find('.highlight-text').removeClass('highlight-text');
        var parentele = $(e.currentTarget).find('.list-node');
        $(parentele.context.parentElement).addClass('highlight-text');
        $scope.LoadQuestions(item);

    }

    $scope.DeleteQuestion = function (item, QuestionId) {
        //    //debugger;
        $.confirm({
            title: 'Confirm!',
            content: 'Do you want to Delete!',
            buttons: {
                Yes: function () {
                    $http({
                        url: "/Survey/Question/Delete?Id=" + QuestionId
                    }).then(function success(result) {
                        var res = result.data;
                        if (res !== 'session expired') {
                            if (res.Status === 'success') {
                                $scope.LoadQuestions(item);
                                $scope.clearFieldsForQuestions();
                            } else {
                                $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            }
                        } else {
                            $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                        }

                    });
                },
                No: function () { },
            }
        });

    };

    //End


    $scope.QuestionTypeId = {};


    $scope.EditQuestion = function (data) {

        $scope.IsEdit = true;
        //debugger;
        $scope.SectionIdForTemplate = data.SectionId;
        $scope.GetResponse(data.QuestionId);
        $("#CreateSectionli").removeClass("active");
        $("#CreateQuestionLogicli").removeClass("active");
        $("#AddQuestion").show();
        $("#AddQuestionli").addClass("active");
        $("#CreateSection").hide();
        $("#CreateQuestionLogic").hide();

        Comman.LoadUnitTypes($rootScope.SelectedUnitSystemId);

        $scope.obj = data;
        $scope.fillddlUnits();

        $scope.ShowRespose(data.QuestionTypeId);

        if (data.UnitTypeId != undefined) {
            $("#UnitType").show();
        }
    }

    $scope.EditIsActive = function (q) {
        // alert(q.IsActive);
        $http({
            method: "post",
            url: "/Survey/Question/IsActive",
            data: q,
        }).then(function Success(result) {
            var res = result.data;
        }, function myError(response) {
            $.notify(response.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
        });
    };
    $scope.ChangeIsRequired = function (q) {
        // alert(q.IsActive);
        $http({
            method: "post",
            url: "/Survey/Question/IsActive",
            data: q,
        }).then(function Success(result) {
            var res = result.data;
        }, function myError(response) {
            $.notify(response.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
        });
    };

    $scope.ShowSection = function () {
        $("#AddQuestion").hide();
        $("#CreateQuestionLogic").hide();
        $("#CreateSection").show();
        if ($rootScope.TempSectionForEdit != undefined && $rootScope.TempSectionForEdit!=null) {
            angular.forEach($rootScope.Sections, function (val, key) {
                if (val.SectionId == $rootScope.TempSectionForEdit.SectionId) {
                    $rootScope.Sections.splice(key, 1);
                }

            });
        }
    }

    $scope.tabChanged = function () {
        debugger;
        if ($rootScope.TempSectionsHolder != undefined && $rootScope.TempSectionsHolder.length > 0 && $rootScope.Sections.length < $rootScope.TempSectionsHolder.length) {
            $rootScope.Sections = [];
            angular.forEach($rootScope.TempSectionsHolder, function (val, key) {
                $rootScope.Sections.push(val);
            });
        }
    }
    $scope.ShowQuestionLogic = function () {
        $("#CreateQuestionLogic").show();
        $("#AddQuestion").hide();
        $("#CreateSection").hide();
        $scope.tabChanged();
       

    }

    $scope.ShowQuestion = function () {
        $scope.IsEdit = false;
        $scope.obj = {}
        $scope.obj.UnitSystemId = $rootScope.SelectedUnitSystemId
        $("#AddQuestion").show();
        $("#CreateSection").hide();
        $("#CreateQuestionLogic").hide();
        $scope.obj.Weightage = '1';
        $scope.tabChanged();
    }



    $('#frm-question').parsley();
    var formdata = new FormData();
    $scope.getTheFiles = function ($files) {
        angular.forEach($files, function (value, key) {

            formdata.append(key, value);
        });
    };

    //function objectifyForm(formArray) {//serialize data function
    //    //debugger;
    //    var returnArray = {};
    //    for (var i = 0; i < formArray.length; i++) {
    //        returnArray[formArray[i]['name']] = formArray[i]['value'];
    //    }
    //    return returnArray;
    //}
    $scope.SubmitQuestion = function () {
        //debugger;
        $scope.obj.Responses = [];
        if ($scope.obj.Weightage == undefined) {
            $scope.obj.Weightage = 1;
        }
        console.log($scope.obj);
        $scope.obj.Responses = $scope.Responses;
        var sectionid = $scope.obj.SectionId;
        var questiontypeid = $scope.obj.QuestionTypeId;
        //console.log($("#frm-question").serialize());
        $http({
            method: "post",
            url: "/Survey/Question/New",
            data: $scope.obj,
            // headers: { 'Content-Type': undefined},
        }).then(function Success(result) {
            var res = result.data;
            if (res !== 'session expired') {
                if (res.Status === 'success') {

                    $scope.arrStringforDropDown = [];
                    $scope.arrString = [];
                    $scope.arrStringforCheckBox = [];
                    $scope.arrStringForTextBox = [];
                    $scope.isColumnRows = false;
                    $scope.isColumn = false;
                    $scope.isRows=false

                    //$scope.QuestionList.push($scope.obj);

                    //$scope.obj = {};
                    //$scope.Responses = [];

                    /*$scope.obj.QuestionTypeId = 0;
                    $scope.obj.UnitTypeId = 0;
                    $scope.obj.UnitId = 0;
                    $scope.obj.Question = "";
                    $scope.obj.IsRequired = false;
                    $scope.obj.IsRepeatable = false;
                    $scope.obj.Description = "";
                    $scope.obj.IsNoteRequired = false;
                    $scope.obj.IsImageRequired = false;
                    $scope.obj.IsBarCodeRequired = false;
                    $scope.obj.Weightage = "";
                    $scope.Responses = [];*/
                    $scope.obj = {
                    //    SectionId: sectionid,
                    //    QuestionTypeId: questiontypeid,
                        SectionId: undefined,
                        QuestionTypeId: '',
                        UnitTypeId: 0,
                        UnitId: 0,
                        Question: "",
                        IsRequired: false,
                        IsRepeatable: false,
                        Description: "",
                        IsNoteRequired: false,
                        IsImageRequired: false,
                        IsBarCodeRequired: false,
                        Weightage: '1',
                        IsVerificationRequired: false,
                        IsVideoRequired: false,
                        IsAudioRequired: false,
                        IsDocumentRequired: false,
                    };
                    $scope.Responses = [];
                    NewItem();
                    $scope.IsEdit = false;
                    $scope.obj.Weightage = '1';
                    // $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
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

    $scope.GetResponse = function (Id) {
        $http({
            url: "/Survey/Response/ToList?Filter=GET_BY_QUESTIONID&Value=" + Id
        }).then(function success(res) {

            $scope.Responses = [];
            $scope.Responses = res.data;

            // logic to Draw Table Type

            if ($scope.obj.TotalColumn != null && $scope.obj.TotalRows != null && $scope.obj.TotalColumn > 0 && $scope.obj.TotalRows > 0) {
                $scope.isColumn = true;
                $scope.isRows = true;
                $scope.isColumnRows = true;
            }
            if (res.data.length == 0) {
                NewItem();
            }

            else {

                $scope.arrStringforDropDown = new Array();
                $scope.arrStringforCheckBox = new Array();
                $scope.arrString = new Array();
                $scope.arrStringForTextBox = new Array($scope.obj.TotalColumn);
                angular.forEach($scope.Responses, function (value, key) {
                    if (value.ResponseValue == 'DropDownList') {
                        $scope.arrStringforDropDown[key] = value.UserValues.split(',');
                    }
                    if (value.ResponseValue == 'CheckBox') {
                        $scope.arrStringforCheckBox[key] = value.UserValues.split(',');
                    }
                    if (value.ResponseValue == 'RadioButton') {
                        $scope.arrString[key] = value.UserValues.split(',');
                    }
                    if (value.ResponseValue == 'TextBox') {
                        $scope.TempArray = [];
                        $scope.TempArray = value.UserValues.split(',');
                        var rowArray = [];
                        angular.forEach($scope.TempArray, function (innervalue, innerkey) {
                           // //debugger;
                            rowArray.push(innervalue);
                            //$scope.arrStringForTextBox[key].push[innerkey] = innervalue;
                        });
                        $scope.arrStringForTextBox[key] = rowArray;
                        rowArray = [];
                    }


                });
                console.log($scope.arrStringForTextBox);
            }
            // End Logic
        });
    }


    $scope.isSeignleMultiselect = false;
    $scope.minmax = false;
    $scope.ResponseText = false;
    $scope.IsNumaric = false;
    $scope.InputResponseText = false;
    $scope.IsFile = false;
    $scope.IsPassed = false;
    $scope.Latitude = '';
    $scope.Longitude = '';

    $scope.ShowRespose = function (data) {
        debugger;
        $scope.Responses = [];
        var found = $filter('Definations').GetById($scope.QuestionTypeId, data);
        if(found.KeyCode == "IMAGE_OPTION"){
            $scope.obj.IsImageRequired=true;
        }
        if (found.KeyCode == "Bar Code") {
            $scope.obj.IsBarCodeRequired = true;
        }
        if (found.KeyCode == "MULTI_SELECT" || found.KeyCode == "SINGLE_SELECT") {
            $scope.isSeignleMultiselect = true;
            $scope.minmax = false;
            $scope.ResponseText = true;
            $scope.obj.KeyCode = "";
            $scope.obj.IsRepeatable = false;
        }
        else if (found.KeyCode == "RATING" || found.KeyCode == "SCALE" || found.KeyCode == "RANGE") {
            $scope.isSeignleMultiselect = true;
            $scope.minmax = true;
            $scope.ResponseText = false;
            $scope.obj.KeyCode = "";
            $scope.obj.IsRepeatable = false;
        }
        else {
            $scope.minmax = false;
            $scope.isSeignleMultiselect = false;
            $scope.obj.KeyCode = found.KeyCode;
        }
       
        NewItem();

     
          
    }

    $http({
        url: "/Defnation/ToList?filter=byDefinationType&value=Question%20Type"
    }).then(function success(res) {
        $scope.QuestionTypeId = res.data;
    });


   
  
    $scope.LoadListQuestions = function (item) {
        // $scope.item = {}
        //  debugger
        $http({
            url: "/Survey/Question/ToList?Filter=GET_BY_SECTIONID&Value=" + item.SectionId
        }).then(function success(res) {

            item.QuestionList = res.data;
            //console.log(res.data);
        });
    }


    $scope.CreateNewWorkOrder = function () {
        //debugger;
        if ($rootScope.SurveyId != null && $rootScope.SurveyId!=0) {
            $http({
                method: "post",
                url: "/Survey/Document/CreateNewWorkOrder?SurveyId=" + $rootScope.SurveyId,
            }).then(function Success(result) {
                $.notify('Work Order Created Successfully!.', { type: 'success', color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
            }, function myError(response) {
                $.notify('Something Went Wrong!', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
            });
        }
        else {
            $.notify('Please Create a Survey First!', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
        }
       
    }

    $scope.CloneSurvey=function(){
        if ($rootScope.SurveyId != null && $rootScope.SurveyId != 0) {
            $("#CloneSurvey-popup").modal("show");
        }
        else {
            $.notify('Please Create a Survey First!', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
        }
    }

    $scope.SaveCloneSurvey = function (CloneSurveyName, ClientId) {
        debugger;
        $http({
            method:"POST",
            url: "/Survey/Document/CloneSurvey?SurveyId=" + $rootScope.SurveyId + "&SurveyTitle=" + CloneSurveyName+"&ClientId="+ClientId
        }).then(function success(res) {
            $.notify('Clone Success', { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });

        });
    }



    $scope.AddQuestionFromTemplate = function () {
        var SectionId = $scope.SectionIdForTemplate
        if (SectionId != '' && SectionId != null && SectionId != undefined) {
            $scope.showValidations = false;
            window.location.href = "/Survey/Template/Index?id=" + $scope.SurveyId+"&SectionId="+SectionId+"&IdType=Section";

        }
        else {
            $scope.showValidations = true;
        }
    }
    $scope.ChangeValidationStatus = function (SectionId) {
        
        $scope.SectionIdForTemplate = SectionId;
        if (SectionId != '' && SectionId != null && SectionId != undefined) {
            $scope.showValidations = false;
        }
    }

    $scope.ChangeName = function (name) {
        var FinalName = name;
        if(name=="Image Options") 
        {
            FinalName = "Picture";
        }
        else if(name=="Direction & GPS Based Images")
        {
            FinalName = "Map";
        }
        return FinalName;
        
    }
    $scope.ChangeSurveyEnity=function(val)
    {
        if (val) {
            if ($scope.obj.SurveyEntity == "Sector Location") {
                $scope.obj.SurveyEntity = "";
            }
        }
    }
    $scope.MarkMultiLocation = function (val) {
        if (val=="Sector Location") {
            $scope.obj.IsMultiLocation = false;
        }
    }
    $scope.IncreaseWidth=function(columns){
        var getWidth = 100 / columns;
        if (getWidth < 25) {
           return "InputWidthClass";
        }
        else
        {
            return "";
        }
    }
    $scope.Close=function(){
        $scope.ClientId = "";
    }
});

//------------------------------------
app.controller('QuestionLogic', function ($scope, $http, Comman, $rootScope, $compile) {
    $scope.Conditions = {};
    $scope.Responses = {};
    $scope.Actions = {};
    $scope.QuestionLogics = {};
    $scope.ToQuestionId = [];
    $scope.SectionId;
    $scope.objQL = {};
    // alert(localStorage.getItem('test'));
    $scope.Questions = {};


    $scope.LoadQuestions = function () {

        $scope.SectionId = $scope.objQL.SectionId;
        $http({
            url: "/Survey/Question/ToList?filter=GET_BY_SECTIONID&value=" + $scope.objQL.SectionId
        }).then(function success(res) {

            $scope.Questions = res.data;

        });
        $scope.LoadQuestionLogics();

        // was written for partialview
        //$.ajax({
        //    url: '/Survey/QuestionLogic/listQuestionLogic?Filter=GET_BY_SECTIONID&Value=' + $scope.SectionId,
        //    success: function (res) {
        //        var temp = $compile(res)($scope);
        //       // $compile(res)($scope);
        //        $('#pan-logics').html(res);// $compile(res)($scope);;
        //    }
        //});
        //$scope.LoadToQuestionIds();
    }

    $scope.clearLogicFields = function () {
        $scope.objQL = {};
        $scope.Questions = {};
        $scope.ToQuestionId = [];

        $scope.Responses = {};

    }

    $scope.LoadToQuestionIds = function () {
        $http({
            //url: "/Survey/QuestionLogic/ToQuestionIds?filter=GET_ToQuestionIds_BY_SECTIONID&value=" + $scope.SectionId
            url: "/Survey/QuestionLogic/ToQuestionIds?filter=GET_ToQuestionIds_BY_SECTIONID&value=" + $scope.objQL.LogicId
        }).then(function success(res) {

            $scope.objQL.SectionId = res.data[0].SectionId
            $scope.LoadQuestions();
            $scope.objQL.FromQuestionId = res.data[0].FromQuestionId
            $scope.LoadResponses();
            $scope.objQL.ConditionId = res.data[0].ConditionId
            $scope.objQL.ResponseId = res.data[0].ResponseId
            $scope.objQL.ActionId = res.data[0].ActionId


            //if (res.data[0].ToQuestionId.indexOf(',') > -1) {
            $scope.ToQuestionId = res.data[0].ToQuestionIdNew;
            //}
            //else {
            //    $scope.ToQuestionId = res.data[0].ToQuestionId;
            //}

        });
    }
    $scope.LoadResponses = function () {

        $http({
            url: "/Survey/Response/ToList?filter=GET_BY_QUESTIONID&Value=" + $scope.objQL.FromQuestionId
        }).then(function success(res) {

            $scope.Responses = res.data;
            //console.log($scope.Responses);
        });
    }

    $scope.LoadQuestionLogics = function () {
        var arryToQuestion = [];
        $http({
            url: "/Survey/QuestionLogic/ToList?filter=GET_BY_SECTIONID&value=" + $scope.SectionId    //$scope.objQL.SectionId
        }).then(function success(res) {
            //debugger

            $scope.QuestionLogics = res.data;
            console.log($scope.QuestionLogics);
        });
    }

    $http({
        url: "/Defnation/ToList?filter=byDefinationType&value=Conditions"
    }).then(function success(res) {
        $scope.Conditions = res.data;
    });


    $http({
        url: "/Defnation/ToList?filter=byDefinationType&value=Logic Actions"
    }).then(function success(res) {
        $scope.Actions = res.data;
    });

    $scope.EditLogic = function (data) {
        ////debugger;
        //$scope.objQL.FromQuestionId = data
        $scope.objQL.LogicId = data;
        //$scope.LoadResponses();
        //$scope.objQL = data;
        $scope.LoadToQuestionIds();
    }

    $('#frm-questionLogic').parsley();
    $scope.SubmitLogic = function () {
        //debugger;
        $scope.objQL.ToQuestionId = $scope.ToQuestionId;
        if ($scope.objQL.ToQuestionId.length == 0 && $scope.objQL.ActionId != 73300) {
            $scope.ShowQuestionIsRequired = true;
            return;
        }
        else {
            $scope.ShowQuestionIsRequired = false;
        }
        if ($scope.objQL.ActionId == '73300') {
            $scope.objQL.ToQuestionId = $scope.objQL.FromQuestionId;
        }

        $scope.objQL.SurveyId = $rootScope.SurveyId
        $http({
            method: "post",
            url: "/Survey/QuestionLogic/New",
            data: { ql: $scope.objQL, ToQuestions: $scope.objQL.ToQuestionId }
        }).then(function Success(res) {
            //  console.log(res);
            $scope.objQL = {};

            $scope.Responses = {};

            $scope.ToQuestionId = [];
            //$scope.objQL.SectionId = $scope.SectionId;
            $scope.LoadQuestionLogics();
            Comman.GetSections($rootScope.SurveyId);
        }, function myError(response) {
            //console.log(response.statusText);

        });
    };

    $scope.Delete = function (id) {
        //debugger;
        $.confirm({
            title: 'Confirm!',
            content: 'Do you want to Delete!',
            buttons: {
                Yes: function () {
                    $http({
                        method: "post",
                        url: "/Survey/QuestionLogic/Delete/?Id=" + id
                    }).then(function success(result) {
                        var res = result.data;
                        if (res !== 'session expired') {
                            if (res.Status === 'success') {
                                $scope.LoadQuestionLogics();
                                $scope.objQL = {};
                                $scope.objQL.SectionId = $scope.SectionId;

                                Comman.GetSections($rootScope.SurveyId);
                            } else {
                                $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            }
                        } else {
                            $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                        }
                    });
                },
                No: function () { },
            }
        });
    };
});

//------------------------------------
app.controller('SurveyPreview', function ($scope, $http, Comman, $rootScope) {


    $scope.LoadQuestions = function (item) {
        $scope.item = {}
        $http({
            url: "/Survey/Question/ToList?Filter=GET_BY_SECTIONID&Value=" + item.SectionId
        }).then(function success(res) {

            item.QuestionList = res.data;
            //console.log(res.data);
        });
    }

    $scope.Delete = function (item, QuestionId) {
        //    //debugger;
        $.confirm({
            title: 'Confirm!',
            content: 'Do you want to Delete!',
            buttons: {
                Yes: function () {
                    $http({
                        url: "/Survey/Question/Delete/" + QuestionId
                    }).then(function success(result) {
                        var res = result.data;
                        if (res !== 'session expired') {
                            if (res.Status === 'success') {
                                $scope.LoadQuestions(item);
                            } else {
                                $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            }
                        } else {
                            $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                        }

                    });
                },
                No: function () { },
            }
        });

    };

   

});

//-----------------------------------
app.filter('Definations', function () {
    var GetById = function (input, id) {

        var i = 0, len = input.length;
        for (; i < len; i++) {
            if (input[i].DefinationId == id) {
                return input[i];
            }
        }
        return null;
    }

    var GetByKeyCode = function (input, Code) {
        var i = 0, len = input.length;
        for (; i < len; i++) {
            if (input[i].KeyCode == Code) {
                return input[i];
            }
        }
        return null;
    }

    return { GetById: GetById, GetByKeyCode: GetByKeyCode };
});

//-----------------------------------
app.directive('loading', ['$http', function ($http) {
    return {
        restrict: 'A',
        template: '<div style="position: fixed;left: 50%;top: 30%;z-index: 10000;" class="loading-spiner"><img src="/Content/Images/Application/Logo_Loading.gif" /> <div> <span class="label" style="background-color: #808080; font-weight: bold;margin-left: 19%;margin-top: -13%;font-size: medium;">Please wait...</span></div></div>',
        link: function (scope, elm, attrs) {
            scope.isLoading = function () {
                return $http.pendingRequests.length > 0;
            };

            scope.$watch(scope.isLoading, function (v) {
                if (v) {
                    elm.show();
                } else {
                    elm.hide();
                }
            });
        }
    };
}])

//---------------------------------
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

//app.directive('multiselect', [function () {
//    function link(scope, $element) {
//        //  scope.name = scope.name;
//        // console.log(scope);
//        $element.multiselect('refresh');
//        $element.multiselect();
//        $element.multiselect('refresh');
//    }
//    return {
//        restrict: 'A',
//        replace: true,
//        scope: {
//            Questions: '='
//        },
//        link: link,
//       // template: ''
//    }
//}]);
