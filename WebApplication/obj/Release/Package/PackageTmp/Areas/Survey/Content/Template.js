var app = angular.module('TSS', []);


app.controller('Template', function ($scope, $http) {



    $scope.maxSize = 10;     // Limit number for pagination display number.
    $scope.totalCount = 0;  // Total number of items in all pages. initialize as a zero
    $scope.pageIndex = 1;   // Current page number. First page is 1.-->
    $scope.pageSizeSelected = 10; // Maximum number of items per page.

    $scope.Categories = [];
    $scope.SubCategories = [];
    $scope.Surveys = [];
    $scope.Sections = [];
    $scope.SelectedSections = [];
    $scope.SearchedSections = [];
    $scope.SingleQuestion = {};
    
       
    $scope.LoadSubCategories = function () {
        try{
            $("div#divLoading").addClass('show');
            $http({
                url: "/Defnation/ToList?filter=PDefinationId&value=" + $scope.CategoryId
            }).then(function success(res) {
                $scope.SubCategories = res.data;
                $("div#divLoading").removeClass('show');
            });
        }
        catch (err) {
            $("div#divLoading").removeClass('show');
        }
    }

    $scope.LoadSurveys = function () {
        $("div#divLoading").addClass('show');
        $http({
            url: "/Survey/Document/ToList?filter=By_SubCategoryId&value=" + $scope.SubCategoryId +"&pageIndex=" + $scope.pageIndex + "&pageSize=" + $scope.pageSizeSelected
        }).then(function success(res) {
            $scope.Surveys = res.data;
            $("div#divLoading").removeClass('show');
        });
    }

    $scope.LoadSections = function () {
        // debugger;
        $("div#divLoading").addClass('show');
        if ($scope.SurveyId>0) {
            $http({
                url: "/Survey/Section/List?filter=By_SurveyId&value=" + $scope.SurveyId
            }).then(function success(res) {
                debugger;
                $scope.Sections = res.data.list2;
                $("div#divLoading").removeClass('show');

            });
        }
        
    }
    $scope.GetSingleQuestion = function (id) {
        $("div#divLoading").addClass('show');
        $http({
            //method: 'GET',
            url: '/Question/Single/' + id,
        }).then(function success(res) {
            $scope.SingleQuestion = res.data;
            $("div#divLoading").removeClass('show');
            //console.log($scope.SingleQuestion);
        });


      
    };
    $scope.AddQuestion = function () {

        //public Int64 SurveyId { get; set; }
        //public Int64 SectionId { get; set; }
        //public Int64 QuestionId { get; set; }
        $("div#divLoading").addClass('show');
        $scope.SurveyToReturn = SurveyId;
        debugger;
        $scope.SelectedQuestions = [];
        angular.forEach($scope.SearchedSections, function (sec) {
            angular.forEach(sec.QuestionList, function (que) {
                if (que.selected) {
                    if (IdType == "Section") {
                        SurveyId = SectionId;
                    }
                    var obj = { SurveyId: SurveyId, SectionId: sec.SectionId, QuestionId: que.QuestionId,IdType:IdType };
                    $scope.SelectedQuestions.push(obj);//SurveyId + ',' + sec.SectionId + ',' + que.QuestionId


                }

            });
            
        });

        $http({
            url: "/Survey/Template/index",
            method: "post",
            data: $scope.SelectedQuestions,
        }).then(function success(result) {
            $("div#divLoading").removeClass('show');
            var res = result.data;
            if (res !== 'session expired') {
                if (res.Status === 'success') {
                    window.location.href = "/Survey/Document/Index/" + $scope.SurveyToReturn;
                    $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                    

                } else {
                    $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }
            } else {
                $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
            }

        }, function myError(response) {
            $("div#divLoading").removeClass('show');
            $.notify(response.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
        });
    }


    $scope.Search = function () {
        debugger;
        $("div#divLoading").addClass('show');
        $scope.SelectedSections = [];
        angular.forEach($scope.Sections, function (rec) {
            if (rec.selected) $scope.SelectedSections.push(rec.SectionId);
        });


        $http({
            url: "/Survey/Section/WithQuestions?Value=" + $scope.SelectedSections.join(","),
          //  data: { Value: $scope.SelectedSections.join(",") }
        }).then(function success(res) {
            $scope.SearchedSections = res.data;
            $("div#divLoading").removeClass('show');
        });
    }
    $("div#divLoading").addClass('show');
    $http({
        url: "/Defnation/ToList?filter=byDefinationType&value=Survey Category"
    }).then(function success(res) {
        $scope.Categories = res.data;
        $("div#divLoading").removeClass('show');
        
    });

    $scope.ChangeSelection = function (item, value,index) {
        debugger;
        if (value) {
            angular.forEach(item.QuestionList, function (e, i) {
                e.selected = true;
            });
        }
        else {
            angular.forEach(item.QuestionList, function (e, i) {
                e.selected = false;
            });
        }
    }

    

});
