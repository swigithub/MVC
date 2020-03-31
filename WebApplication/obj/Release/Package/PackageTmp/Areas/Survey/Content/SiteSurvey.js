var app = angular.module('TSS', ['ui.multiselect', 'angular.filter', 'ngSanitize']);

app.run(function ($rootScope) {
    $rootScope.ishide = false;
    $rootScope.Sections = {};
    $rootScope.UnitTypes = {};
    $rootScope.SurveyId = SurveyId;
    $rootScope.SelectedUnitSystemId = 0;


});

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
app.directive("drawing", function () {
    return {
        restrict: "A",
        link: function (scope, element) {
            var ctx = element[0].getContext('2d');

            // variable that decides if something should be drawn on mousemove
            var drawing = false;

            // the last coordinates before the current move
            var lastX;
            var lastY;

            element.bind('mousedown', function (event) {
                if (event.offsetX !== undefined) {
                    lastX = event.offsetX;
                    lastY = event.offsetY;
                } else {
                    lastX = event.layerX - event.currentTarget.offsetLeft;
                    lastY = event.layerY - event.currentTarget.offsetTop;
                }

                // begins new line
                ctx.beginPath();

                drawing = true;
            });
            element.bind('mousemove', function (event) {
                if (drawing) {
                    // get current mouse position
                    if (event.offsetX !== undefined) {
                        currentX = event.offsetX;
                        currentY = event.offsetY;
                    } else {
                        currentX = event.layerX - event.currentTarget.offsetLeft;
                        currentY = event.layerY - event.currentTarget.offsetTop;
                    }

                    draw(lastX, lastY, currentX, currentY);

                    // set current coordinates to last one
                    lastX = currentX;
                    lastY = currentY;
                }

            });
            element.bind('mouseup', function (event) {
                // stop drawing
                drawing = false;
            });

            // canvas reset
            function reset() {
                element[0].width = element[0].width;
            }

            function draw(lX, lY, cX, cY) {
                // line from
                ctx.moveTo(lX, lY);
                // to
                ctx.lineTo(cX, cY);
                // color
                ctx.strokeStyle = "#4bf";
                // draw it
                ctx.stroke();
            }
        }
    };
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
            //  
            var Result = $http({
                method: "post",
                url: "/survey/section/list/?value=" + tempSurveyId,
            }).then(function success(res) {
                ////debugger;
                Section = res.data;
                $rootScope.Sections = res.data;
                //console.log(res.data);
                return res.data
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
app.controller('SiteSurvey', function ($scope, $http, Comman, $filter, $rootScope, $timeout, $sce) {
    // debugger;
    var canvas = document.querySelector("canvas");
    var signaturePad = new SignaturePad(canvas);
    $scope.ShowMapClearButton = false;
    $scope.ShowMapFanButton = false;
    $scope.SaveButtonShow = true;

    $scope.PreviewButtonShow = true;
    $scope.ShowSignatureQuestion = false;
    $scope.HideQPad = false;
    $scope.sections = [];
    $scope.Responses = [];
    $scope.SelectedSection = {};
    $scope.RepeatableCurentSection = {};
    $scope.Questions = [];
    $scope.ChildCount = '';
    $scope.selectedQuestion = {};
    $scope.currentIndex = 0;
    $scope.endsurvey = false;
    $scope.valuesByColumn = [];
    $scope.ShowPreview = false;
    $scope.ShowNext = true;
    $scope.PreviewSections = [];
    $scope.RequiredQuestions = [];

    $scope.selectedQuestion.Question = 'Please Select a Section';

    $scope.Repeatable = function (data) {
        debugger;
        $scope.RepeatableCurentSection = data;
        $scope.ChildCount = data.RepeatCount;
        $("#repeatable-popup").modal("show");
    };
    
    $scope.DeleteChild = function (data) {
        // //debugger;
        $.confirm({
            title: 'Confirm!',
            content: 'Do you want to delete <b>'+data.SectionTitle+'</b> ?',
            buttons: {
                Yes: function () {
                    $http({
                        url: "/Document/DeleteSectionsWithChild?PSectionId=" + data.PSectionId + "&SectionId=" + data.SectionId
                    }).then(function success(res) {
                        loadSections();
                        $scope.selectedQuestion = {};
                        $scope.selectedQuestion.Question = 'Please Select a Section';
                    }, function () {
                        $.notify("Internal Server Error", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
                    });
                },
                No: function () {
                  
                }
            }
        });
      
    }
    $scope.GetSectionQuestions = function (section) {
        debugger;
        $("div#divLoading").addClass('show');
        $scope.SectionCurrent = section;
        if (section.DefinationName == "Completed") {
            $scope.SelectedSection = section;
            $scope.LoadPreviewData(section);
            $scope.ShowPreview = true;
            $scope.PreviewButtonShow = false;
            var a = angular.element(document).find('#HideShowQuestionBtn');
            a[0].innerText = "Show All Questions";
            $('#loadquestionspanel').removeClass("col-lg-7");
            $('#loadquestionspanel').addClass("col-lg-9");
            $('#allquestionspanel').attr('style', 'display:none');

            $("div#divLoading").removeClass('show');

        }
        else {
            $scope.PreviewButtonShow = true;
            $scope.ShowPreview = false;
            $scope.SectionSignature = section.Signature;
            $scope.IsSignatureRequired = section.IsSignatureRequired;
            if (section.IsRepeatable === true) {
                $("div#divLoading").removeClass('show');
                return;
            }
            $scope.SelectedSection = section;
            $scope.ShowQuestionCanvas();
            $http({
                url: "/Document/GetSectionQuestions_For_Preview?id=" + section.SectionId + "&surveyId=" + SurveyId
            }).then(function success(res) {
                debugger;
                if (res.data.length>3600) {
                    $.notify("Session Expired!", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
                    $("div#divLoading").removeClass('show');
                    return false;
                }
                $scope.RequiredQuestions = [];
                $scope.ShowNext = false;
                $scope.endsurvey = false;
                $scope.Questions = res.data;
                angular.forEach($scope.Questions, function (e, i) {
                    if (e.QuestionType == "DateTime") {
                        angular.forEach(e.Responses, function (re, ri)
                        {
                            var date = new Date(re.ResponseValue);
                            console.log(date);
                            re.ResponseValue = date;

                        });
                    }
                });
                $scope.currentIndex = 0;
                //For Signature of Questions
                var QuestionCanvas = document.getElementById("QuestionCanvas");
                $scope.Qpad = new SignaturePad(QuestionCanvas);
                HideQPad();

                $("div#divLoading").removeClass('show');
                //

                $scope.selectedQuestion = res.data.length > 0 ? $scope.Questions[$scope.currentIndex] : {};
                if ($scope.currentIndex == $scope.Questions.length - 1) {
                    $scope.endsurvey = true;
                }
                angular.forEach($scope.Questions,
                   function (obj, i) {
                       //debugger;
                       if (obj.QuestionTypeId == 103304) {
                           if (obj.Responses[0].Signature != null && obj.Responses[0].Signature != undefined && obj.Responses[0].Signature != "") {
                               $scope.ShowSignatureQuestion = true;
                               $scope.SignatureForQuestion = obj.Responses[0].Signature;
                           }
                           else {
                               $scope.ShowSignatureQuestion = false;
                           }
                       }
                       if (obj.QuestionTypeId === 103305) {
                           $scope.MapLatLngArray = obj.Responses[0].ResponseValue;
                           $scope.ShowLatLongArea = true;
                           
                           obj.Responses[0].IsChecked = true;
                       }
                       if (obj.QuestionType == 'Table') {
                           angular.forEach(obj.Responses,
                               function (obj1, index) {
                                   if (obj.DynamicRows) {
                                       obj.TotalRows = obj.DynamicRowsCount;
                                   }
                                   if (obj1.ResponseValue == 'TextBox') {
                                      
                                       $scope.tempArray = obj1.SelectedRawResponse.split('|');

                                       // Changes For Dynamic rows
                                       if (obj1.SelectedRawResponse === "" && obj1.IsReadOnly) {
                                           $scope.tempArray = obj1.UserValues.split(',');
                                       }
                                       if (obj.DynamicRows) {
                                           if ($scope.tempArray.length < obj.DynamicRowsCount) {
                                               obj.TotalRows = obj.DynamicRowsCount;
                                               for (var i = 1; i < obj.TotalRows; i++) {
                                                   $scope.tempArray.push(obj1.UserValues.split(','));
                                               }
                                           }
                                       }
                                       // End Changes For DynamicRows

                                       obj1.TextBoxList = new Array($scope.tempArray.length);
                                       angular.forEach($scope.tempArray,
                                           function (item, index) {
                                               obj1.TextBoxList[index] = {
                                                   IsChecked: false,
                                                   ResponseValue: $scope.tempArray[index]
                                               };
                                           });
                                   }
                                   if (obj1.ResponseValue == 'Map') {
                                       
                                       $scope.tempArrayMap = obj1.SelectedRawResponse.split('|');

                                       // Changes For Dynamic rows
                                       if (obj1.SelectedRawResponse === "" && obj1.IsReadOnly) {
                                           $scope.tempArrayMap = obj1.UserValues.split(',');
                                       }
                                       if (obj.DynamicRows) {
                                           if ($scope.tempArrayMap.length < obj.DynamicRowsCount) {
                                               obj.TotalRows = obj.DynamicRowsCount;
                                               for (var i = 1; i < obj.TotalRows; i++) {
                                                   $scope.tempArrayMap.push(obj1.UserValues.split(','));
                                               }
                                           }
                                       }
                                       // End Changes For DynamicRows

                                       obj1.LatLngList = new Array($scope.tempArrayMap.length);
                                       angular.forEach($scope.tempArrayMap,
                                           function (item, index) {
                                               obj1.LatLngList[index] = {
                                                   IsChecked: false,
                                                   ResponseValue: $scope.tempArrayMap[index]
                                               };
                                           });
                                   }
                                   obj1.SelectedValues = obj1.SelectedRawResponse == null ? [] : obj1.SelectedRawResponse.split('|');
                               });
                       }
                   });


                //if (res.data.length === 0)
                //    $.notify("No Questions are found in this section", { type: "Please Select Option", color: "#ffffff", background: "#ffd28a", blur: 0.6, delay: 0, });
            }, function () {
                $.notify("Internal Server Error", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
            });

            if ($scope.ShowPreview === true)
                $scope.LoadPreviewData(section);

        }
    }



    function loadSections() {
        $http({
            url: "/Document/GetSections?id=" + SurveyId
        }).then(function success(res) {
            //debugger;
            // Code For Getting Sections and Immediate Child Sections ID's

            $scope.ArrayOfSectionsToShowIcons = [];

            //Getting Sections Id

            angular.forEach(res.data, function (Sections, Index) {
                if (Sections.PSectionId == 0) {
                    $scope.ArrayOfSectionsToShowIcons.push(Sections.SectionId);

                    //Getting Immediate Child Section Id's

                    angular.forEach(Sections.Sections, function (SubSection, SubIndex) {
                        $scope.ArrayOfSectionsToShowIcons.push(SubSection.SectionId);
                    });
                    // 
                }
            });
            //
            // END CODE For Getting Sections and Immediate Child Sections ID's
            console.log($scope.ArrayOfSectionsToShowIcons);
            $scope.sections = res.data;
        });
    }

    loadSections();
    $scope.files = {};
    $scope.IsPreviousDisable = $scope.currentIndex > 0;
    $scope.IsNextDisabled = $scope.selectedQuestion.SiteQuestionId == null ||
        $scope.selectedQuestion.SiteQuestionId == '' ||
        $scope.selectedQuestion.SiteQuestionId == 0;

    $scope.clearFields = function () {
        $scope.obj = {};
        $scope.Responses = [];
    }
    $scope.test = function () {
        alert();
    }

    $scope.GenerateChild = function (childCount) {
        //debugger;
        $http({
            url: "/Document/GenerateChildSections?parentId=" + $scope.RepeatableCurentSection.SectionId + "&childCount=" + childCount
        }).then(function success(res) {
            //if (res.data.length > 0) {
            //item.Sections.push(res.data);
            childCount = 0;
            $scope.ChildCount = 0;
            loadSections();
            $scope.RepeatableCurentSection = {};
            //}
        }, function () {
            $.notify("Internal Server Error", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
        });
    }
    $scope.Finish = function () {
        debugger;
        var dataUrl = "";
        if ($scope.Qpad != null) {
            $scope.Qpad.backgroundColor = "white";
            $scope.Qpad.color = "black";
            dataUrl = $scope.Qpad.toDataURL();
        }
        if ($scope.selectedQuestion.QuestionType == "Multi Line") {
            var data = $("#editor").summernote('code');
            if (data != '' && data != null && data != undefined) {
                try {
                    $scope.selectedQuestion.Responses[0].ResponseValue = data.replace(/<br>/gi, '<br/>');
                }
                catch (err) { }
            }
        }
        var getResult = $scope.MaintainRequiredQuestions(dataUrl);
        if ($scope.RequiredQuestions.length > 0) {
            $('#RequiredQuestions-popup').modal('show');
        }
        else {
            for (var i = 0; i < $scope.selectedQuestion.ReqActions.length; i++) {
                $scope.selectedQuestion.ReqActions[i].IsDBExist = true;
            }
            
            $http({
                url: "/Document/SaveSiteQuestionResponse",
                method: "POST",
                headers: { 'Content-Type': "Application/json" },
                data: { question: $scope.selectedQuestion, IsFinished: true }
            }).then(function success(res) {
                if ($scope.endsurvey == true) {
                    $scope.GetQuestionLogics($scope.selectedQuestion.QuestionId);
                    loadSections();
                }
            }, function () {
                $.notify("Internal Server Error", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
            });
        }
    }
    $scope.Next = function () {
        debugger;
        if ($scope.selectedQuestion.QuestionType == "Multi Line") {
            var data = $("#editor").summernote('code');
            if (data != '' && data != null && data != undefined)
            {
                try{
                    $scope.selectedQuestion.Responses[0].ResponseValue = data.replace(/<br>/gi, '<br/>');
                }

                catch (err) { }
            }
        }
        $("div#divLoading").addClass('show');
        $(".popovercls").popover('hide');
        HideQPad();
        var dataUrl = "";
        if ($scope.Qpad != null) {
            $scope.Qpad.backgroundColor = "white";
            $scope.Qpad.color = "black";
            dataUrl = $scope.Qpad.toDataURL();
        }
       var getResult= $scope.MaintainRequiredQuestions(dataUrl);

       if (getResult) {
           
            var index = $scope.Questions.findIndex(p => p.QuestionId == $scope.selectedQuestion.QuestionId);

            $scope.currentIndex = index + 1;
            $scope.selectedQuestion = $scope.Questions[index + 1];
            if ($scope.currentIndex == $scope.Questions.length - 1) {
                $scope.endsurvey = true;
            }
            if ($scope.selectedQuestion.QuestionType == 'Table') {
                angular.forEach($scope.selectedQuestion.Responses,
                    function (obj, index) {

                        obj.SelectedValues = obj.SelectedRawResponse == null ? [] : obj.SelectedRawResponse.split('|');

                    });
            }
            $("div#divLoading").removeClass('show');
        }
        else {
           
            for (var i = 0; i < $scope.selectedQuestion.ReqActions.length; i++) {
                $scope.selectedQuestion.ReqActions[i].IsDBExist = true;
            }
            $http({
                url: "/Document/SaveSiteQuestionResponse",
                method: "POST",
                headers: { 'Content-Type': "Application/json" },
                data: { question: $scope.selectedQuestion, IsFinished: false }
            }).then(function success(res) {
                HideQPad();
                $("div#divLoading").removeClass('show');
                $scope.GetQuestionLogics($scope.selectedQuestion.QuestionId);
              
            }, function () {

                $("div#divLoading").removeClass('show');
                $.notify("Internal Server Error", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
            });

       }
      
    }
    $scope.MaintainRequiredQuestions = function (dataUrl)
    {
        var Check = false;
        var isQuestionAnswered = false;
        var isQuestionRequired = $scope.selectedQuestion.IsRequired;
        if ($scope.selectedQuestion.QuestionType == "Rating" || $scope.selectedQuestion.QuestionType == "Range" || $scope.selectedQuestion.QuestionType == "Scale" || $scope.selectedQuestion.QuestionType == "DateTime") {
            angular.forEach($scope.selectedQuestion.Responses, function (c) {
                c.IsChecked = true;
            });
        }
        if ($scope.selectedQuestion.QuestionType == "Signature") {
            angular.forEach($scope.selectedQuestion.Responses, function (c) {
                if (c.Signature == "") {
                    c.IsChecked = true;
                    c.Signature = dataUrl;
                }
                else {
                    c.IsChecked = true;
                    c.Signature = $scope.SignatureForQuestion;
                }
            });
        }
        if ($scope.selectedQuestion.QuestionType == "Image Options") {
            angular.forEach($scope.selectedQuestion.ReqActions, function (c) {
                if (c.RequiredAction != "") {
                    isQuestionAnswered = true;
                }
            });
        }
        angular.forEach($scope.selectedQuestion.Responses, function (c) {

            if (c.IsChecked && (c.ResponseValue != "" || c.ResponseText != "" || c.MinValue != "" || $scope.selectedQuestion.QuestionType == "Signature"))
                isQuestionAnswered = true;
        });
        if (isQuestionRequired && !isQuestionAnswered)
        {
            // Pushing in To array of Required Questions
            $scope.RequiredQuestions.push($scope.selectedQuestion);
            //
            Check = true;
        }
        else {
            // Poping out From Array of Required Questions
            angular.forEach($scope.RequiredQuestions, function (val, indexOfQuestion) {
                if ($scope.selectedQuestion.QuestionId == val.QuestionId) {
                    $scope.RequiredQuestions.splice(indexOfQuestion, 1);
                }
            });


            // End Poping Out
            Check = false;
        }
        return Check;
    }
    $scope.CheckActionRequired = function () {
        var result = false;
        $scope.RequiredActionFilled = [];
        $scope.MessagesForRequiredAction = [];
        angular.forEach($scope.selectedQuestion.ReqActions, function (action, key) {
            $scope.RequiredActionFilled.push(action.ActionTypeId);
        });
        $scope.CheckIndexForRequiredAction = function (ind, msg) {
            var index = $scope.RequiredActionFilled.indexOf(ind);
            if (index < 0) {
                $scope.MessagesForRequiredAction.push(msg);
            }
        }
        if ($scope.selectedQuestion.IsAudioRequired) {
            $scope.CheckIndexForRequiredAction(6, "Audio is required.")
        }
        if ($scope.selectedQuestion.IsBarCodeRequired) {
            $scope.CheckIndexForRequiredAction(3, "BarCode is required.")
        }
        if ($scope.selectedQuestion.IsDocumentRequired) {
            $scope.CheckIndexForRequiredAction(5, "Document is required.")
        }
        if ($scope.selectedQuestion.IsImageRequired) {
            $scope.CheckIndexForRequiredAction(2, "Image is required.")
        }
        if ($scope.selectedQuestion.IsNoteRequired) {
            $scope.CheckIndexForRequiredAction(1, "Note is required.")
        }
        if ($scope.selectedQuestion.IsVideoRequired) {
            $scope.CheckIndexForRequiredAction(4, "Video is required.")
        }
        if ($scope.MessagesForRequiredAction.length > 0) {
            $('#RequiredActions-popup').modal('show');
            result = true;
        }
        return result;
    }
    $scope.CallForRequiredActions = function (fname) {
        if (fname === "Next") {
            $scope.Next();
        }
        else {
            $scope.Finish();
        }
    }
    $scope.BeforeNextOrFinish = function (fname) {
        debugger;
        $scope.NameToCallFor = fname;
        if ($scope.selectedQuestion.QuestionType == "Multi Line") {
            var data = $("#editor").summernote('code');
            if (data != '' && data != null && data != undefined) {
                try {
                    $scope.selectedQuestion.Responses[0].ResponseValue = data.replace(/<br>/gi, '<br/>');
                }

                catch (err) { }
            }
        }
        if ($scope.selectedQuestion.QuestionType == "Image Options") {
            if ($scope.selectedQuestion.ReqActions.length > 0) {
                var ci = 0;
                angular.forEach($scope.selectedQuestion.ReqActions, function (e, i) {
                    if (e.ActionTypeId == 2) {
                        ci++;
                        $scope.selectedQuestion.Responses[0].ResponseValue = "Picture Taken";
                        $scope.selectedQuestion.Responses[0].IsChecked = true;
                    }
                });
                 if(ci==0) {

                    $scope.selectedQuestion.Responses[0].ResponseValue = "Picture Not Taken";
                    $scope.selectedQuestion.Responses[0].IsChecked = true;
                }
            }
            else {
                $scope.selectedQuestion.Responses[0].ResponseValue = "Picture Not Taken";
                $scope.selectedQuestion.Responses[0].IsChecked = true;
            }
        }
        var checkRequire = $scope.CheckActionRequired();
        if (!checkRequire) {
            if (fname === "Next") {
                $scope.Next();
            }
            else {
                $scope.Finish();
            }
        }
        else {
            return true;
        }
    }

    $scope.Previous = function () {
        $scope.endsurvey = false;
        HideQPad();

        var index = $scope.Questions.findIndex(p => p.QuestionId == $scope.selectedQuestion.QuestionId);
        if (index == 0) {
            index = 0;
        }
        else {
            index = index - 1;
        }

        //    var reverse = $scope.Questions.slice().reverse();
        for (var i = 0; i < $scope.Questions.length; i++) {
            if (index == i) {
                if ($scope.Questions[i].IsHide == true) {
                    if (index == 0) {
                        index = 0;
                    }
                    else {

                        index = index - 1;
                        i = 0;
                        i--;
                    }

                }
                else {
                    $scope.currentIndex = index;
                    $scope.selectedQuestion = $scope.Questions[index];
                    if ($scope.selectedQuestion.QuestionType == 'Table') {
                        angular.forEach($scope.selectedQuestion.Responses,
                            function (obj, index) {

                                obj.SelectedValues = obj.SelectedRawResponse == null ? [] : obj.SelectedRawResponse.split('|');

                            });
                    }
                }
            }

        }

    }
    $scope.CancelForRequiredAction = function () {
        $scope.Previous();
        $('#RequiredActions-popup').modal('hide');
    }
    $scope.markSelection = function (choices, item) {

        angular.forEach(choices, function (c) {
            c.IsChecked = false;
        });

        item.IsChecked = true;
    }

    $scope.DeleteFiles = function (e) {
        $.confirm({
            title: 'Confirm!',
            content: 'Do you want to Delete this Attachment?',
            buttons: {
                Yes: function () {
                    var element = $(e.currentTarget)[0];

                    var isdbexistval = $(element).attr('data-dbexist');

                    var actionid = $(element).attr('data-actionid');


                    var imagepath = $(element).attr('data-imagepath');

                    var index = $scope.selectedQuestion.ReqActions.findIndex(p => p.ActionId == actionid);
                    $scope.selectedQuestion.ReqActions.splice(index, 1);



                    $http({
                        url: "/Document/DeleteImage",
                        method: "POST",
                        headers: { 'Content-Type': "Application/json" },
                        data: { path: imagepath, actionId: actionid, isdbexist: isdbexistval }
                    }).then(function success(res) {
                        console.log(res);
                        //console.log($scope.selectedQuestion);
                        //$scope.currentIndex += 1;
                        //$scope.selectedQuestion = $scope.Questions[$scope.currentIndex];
                    }, function () {

                    });
                },
                No: function () {
                    
                }
            }
        });
       
    }

    $scope.UploadFiles = function (files, actionType, actionTypeId) {
        $("div#divLoading").addClass('show');
        debugger;
        var SurCode = $scope.SelectedSection.SiteCode;
        var SurTitle = $scope.SelectedSection.SurveyTitle;
        var SecTitle = $scope.SelectedSection.SectionTitle;

        $http({
            url: "/Document/UploadFiles",
            method: "POST",
            headers: { "Content-Type": undefined },
            transformRequest: function (data) {
                var formData = new FormData();
                for (var i = 0; i < data.files.length; i++) {
                    formData.append("files[" + i + "]", data.files[i]);
                }
                formData.append('SiteCode', SurCode);
                formData.append('SurveyTitle', SurTitle);
                formData.append('SectionTitle', SecTitle);
                return formData;
            },
            data: { files: files}
        }).then(function (res) {
            $scope.selectedQuestion.ReqActions = $scope.selectedQuestion.ReqActions == null
                                               ? []
                                               : $scope.selectedQuestion.ReqActions;

            for (var i = 0; i < res.data.length; i++) {

                var newactionid = Math.floor(Math.random() * 10000000000);
                $scope.selectedQuestion.ReqActions.push({
                    RequiredAction: res.data[i].releativePath,
                    Path: res.data[i].releativePath,
                    name: res.data[i].name,
                    SiteSectionId: $scope.selectedQuestion.SiteSectionId,
                    SiteQuestionId: $scope.selectedQuestion.SiteQuestionId,
                    ActionType: actionType,
                    ActionTypeId: actionTypeId,
                    ActionId: newactionid,
                    IsDBExist: false
                });

            }
            angular.element('#pic_input').val('');
            angular.element('#audio_input').val('');
            angular.element('#file_input').val('');
            angular.element('#pic_input1').val('');

            $("div#divLoading").removeClass('show');
        });
    }


    $scope.ShowQuestion = function (questionid) {
        $('#RequiredQuestions-popup').modal('hide');
        HideQPad();
        var index = $scope.Questions.findIndex(p => p.QuestionId == questionid);
        if (index == $scope.Questions.length - 1) {
            $scope.currentIndex = index;
            $scope.selectedQuestion = $scope.Questions[index];
            if ($scope.selectedQuestion.QuestionType == 'Table') {
                angular.forEach($scope.selectedQuestion.Responses,
                    function (obj, index) {

                        obj.SelectedValues = obj.SelectedRawResponse == null ? [] : obj.SelectedRawResponse.split('|');

                    });
            }
            $scope.endsurvey = true;

        }
        else {
            $scope.currentIndex = index;
            $scope.selectedQuestion = $scope.Questions[index];
            if ($scope.selectedQuestion.QuestionType == 'Table') {
                angular.forEach($scope.selectedQuestion.Responses,
                    function (obj, index) {

                        obj.SelectedValues = obj.SelectedRawResponse == null ? [] : obj.SelectedRawResponse.split('|');
                        s
                    });
            }
            $scope.endsurvey = false;

        }
    }

    $scope.ShowAllQuestionPanel = function (e) {
        console.log(e);
        if (e.currentTarget.innerText == 'Show All Questions') {
            e.currentTarget.innerText = 'Hide All Questions';
            $('#loadquestionspanel').removeClass("col-lg-9");
            $('#loadquestionspanel').addClass("col-lg-7");

            $('#allquestionspanel').attr('style', 'display:block');
        }
        else {
            e.currentTarget.innerText = 'Show All Questions';
            $('#loadquestionspanel').removeClass("col-lg-7");
            $('#loadquestionspanel').addClass("col-lg-9");
            $('#allquestionspanel').attr('style', 'display:none');
        }
    }

    $scope.LoadPreviewData = function (section) {
       
        $http({
            url: "/Document/GetSectionQuestions_For_Preview?id=" + section.SectionId + "&surveyId=" + SurveyId
        }).then(function success(res) {
            if (res.data.length > 3600) {
                $.notify("Session Expired!", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
                $("div#divLoading").removeClass('show');
                return false;
            }
            // Do For Success 
            console.log(res.data);
            $scope.PreviewSections = res.data;
            angular.forEach($scope.PreviewSections,
                function (obj, i) {
                    if (obj.QuestionTypeId == 103304) {
                        if (obj.Responses[0].Signature != null || obj.Responses[0].Signature != undefined) {
                            $scope.SignatureForQuestion = obj.Responses[0].Signature;
                        }
                    }
                    //For DateTime
                    if (obj.QuestionType == 'DateTime') {
                                angular.forEach(obj.Responses, function (re, ri) {
                                    var date = new Date(re.ResponseValue);
                                    re.ResponseValue = date;

                                });
                    }
                    // For MAp
                    if (obj.QuestionTypeId === 103305) {
                        $scope.MapLatLngArray = obj.Responses[0].ResponseValue;
                        $scope.ShowLatLongArea = true;
                    }
                    //
                    if (obj.QuestionType == 'Table') {
                        //debugger;
                        angular.forEach(obj.Responses,
                            function (obj1, index) {
                                if (obj1.ResponseValue == 'TextBox') {
                                    if (obj.DynamicRows) {
                                        obj.TotalRows = obj.DynamicRowsCount;
                                    }
                                    $scope.tempArray = obj1.SelectedRawResponse.split('|');

                                    // Changes For Dynamic rows
                                    if (obj1.SelectedRawResponse === "" && obj1.IsReadOnly) {
                                        $scope.tempArray = obj1.UserValues.split(',');
                                    }
                                    if (obj.DynamicRows) {
                                        if ($scope.tempArray.length < obj.DynamicRowsCount) {
                                            obj.TotalRows = obj.DynamicRowsCount;
                                            for (var i = 1; i < obj.TotalRows; i++) {
                                                $scope.tempArray.push(obj1.UserValues.split(','));
                                            }
                                        }
                                    }
                                    // End Changes For DynamicRows

                                    obj1.TextBoxList = new Array($scope.tempArray.length);
                                    angular.forEach($scope.tempArray,
                                        function (item, index) {
                                            obj1.TextBoxList[index] = {
                                                IsChecked: false,
                                                ResponseValue: $scope.tempArray[index]
                                            };
                                        });
                                }
                                obj1.SelectedValues = obj1.SelectedRawResponse == null ? [] : obj1.SelectedRawResponse.split('|');
                            });
                    }
                });

            //
            //if (res.data.length === 0)
            //    $.notify("No Questions are found in this section", { type: "Please Select Option", color: "#ffffff", background: "#ffd28a", blur: 0.6, delay: 0, });
        }, function () {
            $.notify("Please Select Section", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
        });
    }
    // Section To Get Preview Data
    $scope.GetSectionQuestions_For_Preview = function (section) {
        if ($scope.ShowPreview) {
            $scope.ShowPreview = false;
            HideQPad();
            return;
        }
        else {
            $scope.ShowPreview = true;
            HideQPad();
        }
        $scope.LoadPreviewData(section);
        console.log($scope);
    }
    // End Preview Section

    $scope.DropDownChanged = function (value, index, col, totalRows) {
        if (col.SelectedRawResponse != null) {
            col.arrayForDropDownListItems = col.SelectedRawResponse.split('|');
        }
        col.arrayForDropDownListItems = col.arrayForDropDownListItems == null ? new Array(totalRows) : col.arrayForDropDownListItems;
        col.arrayForDropDownListItems.splice(index, 1, value);
        col.SelectedRawResponse = col.arrayForDropDownListItems.join('|');
        console.log(col.SelectedRawResponse);
    }
    $scope.TextBoxChanged = function (value, index, col, totalRows) {
        if (col.SelectedRawResponse != null) {
            col.arrayForTextBoxItems = col.SelectedRawResponse.split('|');
        }
        col.arrayForTextBoxItems = col.arrayForTextBoxItems == null ? new Array(totalRows) : col.arrayForTextBoxItems;
        col.arrayForTextBoxItems.splice(index, 1, value);
        col.SelectedRawResponse = col.arrayForTextBoxItems.join('|');
        console.log(col.SelectedRawResponse);
    }
    $scope.RadioButtonChanged = function (value, index, col, totalRows) {
        if (col.SelectedRawResponse != null) {
            col.arrayForRadioButtonItems = col.SelectedRawResponse.split('|');
        }
        col.arrayForRadioButtonItems = col.arrayForRadioButtonItems == null ? new Array(totalRows) : col.arrayForRadioButtonItems;
        col.arrayForRadioButtonItems.splice(index, 1, value.ResponseValue);
        col.SelectedRawResponse = col.arrayForRadioButtonItems.join('|');
        console.log(col.SelectedRawResponse);
    }

    $scope.CheckBoxChanged = function (value, index, col, totalRows, IsChk) {
        // Logic To Show data 
        if (col.SelectedRawResponse != null) {
            var GetAllRows = col.SelectedRawResponse.split('|');
        }
        //for Rows data

        col.arrayForCheckBoxItemsRow = col.arrayForCheckBoxItemsRow == null ? new Array(totalRows) : col.arrayForCheckBoxItemsRow;
        if (GetAllRows != null) {
            angular.forEach(GetAllRows, function (rows, i) {
                col.arrayForCheckBoxItemsRow[i] = rows;
            });
        }
        if (IsChk) {
            if (!col.arrayForCheckBoxItemsRow[index]) {
                col.arrayForCheckBoxItemsRow[index] = value.ResponseValue;
            } else {
                col.arrayForCheckBoxItemsRow[index] =
                    col.arrayForCheckBoxItemsRow[index] + ',' + value.ResponseValue;
            }
        } else {
            var temp = col.arrayForCheckBoxItemsRow[index].split(',');
            angular.forEach(temp,
                function (val, key) {
                    if (val === value.ResponseValue) {
                        temp.splice(key, 1);
                    }
                });
            col.arrayForCheckBoxItemsRow[index] = temp.join(',')
        }
        // Push Rows with Column Array
        col.arrayForCheckBoxItems = col.arrayForCheckBoxItems == null ? new Array(totalRows) : col.arrayForCheckBoxItems;
        col.arrayForCheckBoxItems.splice(index, 1, value.ResponseValue);
        col.SelectedRawResponse = col.arrayForCheckBoxItemsRow.join('|');
        console.log(col.SelectedRawResponse);
    }

    $scope.MapBoxChanged = function (value, index, col, totalRows) {
        if (col.SelectedRawResponse != null) {
            col.arrayForMapBoxItems = col.SelectedRawResponse.split('|');
        }
        col.arrayForMapBoxItems = col.arrayForMapBoxItems == null ? new Array(totalRows) : col.arrayForMapBoxItems;
        col.arrayForMapBoxItems.splice(index, 1, value);
        col.SelectedRawResponse = col.arrayForMapBoxItems.join('|');
        console.log(col.SelectedRawResponse);
    }

    $scope.ShowMediaFiles = function (e) {
        var mediafiles = $(e.currentTarget).parents('.av-survey').find(".av-mediafiles");
        var classname = mediafiles[0].className;
        if (classname.contains('displayhide')) {
            $(e.currentTarget).parents('.av-survey').find(".av-mediafiles").removeClass("displayhide");
            $(e.currentTarget).parents('.av-survey').find(".av-mediafiles").addClass("displayshow");
        }
        else {
            $(e.currentTarget).parents('.av-survey').find(".av-mediafiles").removeClass("displayshow");
            $(e.currentTarget).parents('.av-survey').find(".av-mediafiles").addClass("displayhide");
        }


    }

    $scope.ShowAttachmentFiles = function (e) {
        var AttachmentFiles = $(e.currentTarget).parents('.av-survey').find(".av-attachments");
        var classname = AttachmentFiles[0].className;
        if (classname.contains('displayhide')) {
            $(e.currentTarget).parents('.av-survey').find(".av-attachments").removeClass("displayhide");
            $(e.currentTarget).parents('.av-survey').find(".av-attachments").addClass("displayshow");
        }
        else {
            $(e.currentTarget).parents('.av-survey').find(".av-attachments").removeClass("displayshow");
            $(e.currentTarget).parents('.av-survey').find(".av-attachments").addClass("displayhide");
        }


    }

    $scope.GetSelectedValue = function (array, index) {
        //debugger;
        if (array == null || array[index] == null) return '';
        else return array[index];
    }

    $scope.GetRadioSelectedValue = function (array, index, val) {

        if (array == null) return false;
        else if (array[index] == val.ResponseValue) return true;
    }

    $scope.GetCheckSelectedValue = function (array, index, v) {
        var result = false;
        if (array == null) return false;
        else {
            var temp = array[index].split(',');
            angular.forEach(temp,
                function (vl, key) {
                    if (vl === v.ResponseValue) {
                        result = true;
                    }
                });
            return result;
        }
    }

    $scope.GetQuestionLogics = function (Id) {
        //debugger;
        $http({
            url: "/QuestionLogic/ToList?Filter=GET_BY_QUESTIONID_QUESTIONLOGIC&Value=" + Id
        }).then(function success(res) {
            HideQPad();
            $scope.GetQuestionLogicsArr = res.data;
            $scope.selectedAnswerResponses = $scope.selectedQuestion.Responses;
            console.log($scope.selectedAnswerResponses);
            if ($scope.GetQuestionLogicsArr.length > 0) {
                //var toquestionids = $scope.GetQuestionLogicsArr.ToQuestionId.split(',');
                //console.log(toquestionids);
                $.each($scope.GetQuestionLogicsArr, function (i, val) {

                    $.each($scope.Questions, function (j, val1) {
                        if (val.FromQuestionId == val1.QuestionId) {
                            $.each($scope.Questions, function (k, val2) {

                                var value = String(val.ToQuestionId.toString());
                                if (value.contains(val2.QuestionId.toString())) {
                                    //Show questions
                                    if (val.ActionId == '73298') {
                                        //Equal
                                        if (val.ConditionId == 73293) {
                                            var answerresponse = false;
                                            var count = 0;
                                            $.each($scope.selectedAnswerResponses, function (p, val3) {
                                                if (val3.IsChecked == true) {
                                                    if (val.Response == val3.ResponseText) {
                                                        answerresponse = false;
                                                        return false;
                                                    }
                                                    else {
                                                        answerresponse = true;
                                                    }

                                                }
                                                else {
                                                    count = count + 1;
                                                }


                                            });

                                            if (count == $scope.selectedAnswerResponses.length) {
                                                val2.IsHide = false;
                                            }
                                            else {
                                                if (answerresponse == false) {
                                                    val2.IsHide = false;

                                                }
                                                else {
                                                    val2.IsHide = true;
                                                }
                                                if (k < j) {
                                                    $scope.arraymove($scope.Questions, k, j);
                                                }
                                                else {
                                                    $scope.arraymove($scope.Questions, k, j + 1);
                                                }

                                            }


                                            return false;

                                        }


                                        //Not Equal
                                        if (val.ConditionId == 73297) {
                                            var answerresponse = false;
                                            var count = 0;
                                            $.each($scope.selectedAnswerResponses, function (p, val3) {
                                                if (val3.IsChecked == true) {
                                                    if (val.Response != val3.ResponseText) {
                                                        answerresponse = false;

                                                    }
                                                    else {
                                                        answerresponse = true;
                                                        return false;
                                                    }

                                                }
                                                else {
                                                    count = count + 1;
                                                }


                                            });

                                            if (count == $scope.selectedAnswerResponses.length) {
                                                val2.IsHide = false;
                                            }
                                            else {
                                                if (answerresponse == false) {
                                                    val2.IsHide = false;

                                                }
                                                else {
                                                    val2.IsHide = true;
                                                }

                                                if (k < j) {
                                                    $scope.arraymove($scope.Questions, k, j);
                                                }
                                                else {
                                                    $scope.arraymove($scope.Questions, k, j + 1);
                                                }
                                            }
                                            return false;
                                        }


                                        //Greater Than
                                        if (val.ConditionId == 193471) {
                                            var answerresponse = false;
                                            var count = 0;
                                            $.each($scope.selectedAnswerResponses, function (p, val3) {
                                                if (val3.IsChecked == true) {
                                                    if (parseFloat(val3.ResponseText) > parseFloat(val.Response)) {
                                                        answerresponse = false;
                                                        return false;
                                                    }
                                                    else {
                                                        answerresponse = true;
                                                    }

                                                }
                                                else {
                                                    count = count + 1;
                                                }


                                            });

                                            if (count == $scope.selectedAnswerResponses.length) {
                                                val2.IsHide = false;
                                            }
                                            else {
                                                if (answerresponse == false) {
                                                    val2.IsHide = false;

                                                }
                                                else {
                                                    val2.IsHide = true;
                                                }
                                                if (k < j) {
                                                    $scope.arraymove($scope.Questions, k, j);
                                                }
                                                else {
                                                    $scope.arraymove($scope.Questions, k, j + 1);
                                                }

                                            }


                                            return false;
                                        }
                                        //Less Than
                                        if (val.ConditionId == 193472) {
                                            var answerresponse = false;
                                            var count = 0;
                                            $.each($scope.selectedAnswerResponses, function (p, val3) {
                                                if (val3.IsChecked == true) {
                                                    if (parseFloat(val3.ResponseText) < parseFloat(val.Response)) {
                                                        answerresponse = false;
                                                        return false;
                                                    }
                                                    else {
                                                        answerresponse = true;
                                                    }

                                                }
                                                else {
                                                    count = count + 1;
                                                }


                                            });

                                            if (count == $scope.selectedAnswerResponses.length) {
                                                val2.IsHide = false;
                                            }
                                            else {
                                                if (answerresponse == false) {
                                                    val2.IsHide = false;

                                                }
                                                else {
                                                    val2.IsHide = true;
                                                }
                                                if (k < j) {
                                                    $scope.arraymove($scope.Questions, k, j);
                                                }
                                                else {
                                                    $scope.arraymove($scope.Questions, k, j + 1);
                                                }

                                            }


                                            return false;
                                        }


                                    }


                                    //skip questions
                                    if (val.ActionId == '73299') {

                                        //Equal
                                        if (val.ConditionId == 73293) {


                                            var answerresponse = true;
                                            var count = 0;
                                            $.each($scope.selectedAnswerResponses, function (p, val3) {
                                                if (val3.IsChecked == true) {
                                                    if (val.Response == val3.ResponseText) {
                                                        answerresponse = true;
                                                        return false;
                                                    }
                                                    else {
                                                        answerresponse = false;
                                                    }

                                                }
                                                else {
                                                    count = count + 1;
                                                }


                                            });
                                            if (count == $scope.selectedAnswerResponses.length) {
                                                val2.IsHide = false;
                                            }
                                            else {
                                                if (answerresponse == true) {
                                                    val2.IsHide = true;

                                                }
                                                else {
                                                    val2.IsHide = false;
                                                }
                                                if (k < j) {
                                                    $scope.arraymove($scope.Questions, k, j);
                                                }
                                                else {
                                                    $scope.arraymove($scope.Questions, k, j + 1);
                                                }
                                            }

                                            return false;

                                        }

                                        //Not Equal
                                        if (val.ConditionId == 73297) {


                                            var answerresponse = false;
                                            var count = 0;
                                            $.each($scope.selectedAnswerResponses, function (p, val3) {
                                                if (val3.IsChecked == true) {
                                                    if (val.Response != val3.ResponseText) {
                                                        answerresponse = false;

                                                    }
                                                    else {
                                                        answerresponse = true;
                                                        return false;
                                                    }

                                                }
                                                else {
                                                    count = count + 1;
                                                }


                                            });
                                            if (count == $scope.selectedAnswerResponses.length) {
                                                val2.IsHide = false;
                                            }
                                            else {
                                                if (answerresponse == true) {
                                                    val2.IsHide = true;

                                                }
                                                else {
                                                    val2.IsHide = false;
                                                }
                                                if (k < j) {
                                                    $scope.arraymove($scope.Questions, k, j);
                                                }
                                                else {
                                                    $scope.arraymove($scope.Questions, k, j + 1);
                                                }
                                            }

                                            return false;

                                        }

                                        //Greater Than
                                        if (val.ConditionId == 193471) {
                                            var answerresponse = false;
                                            var count = 0;
                                            $.each($scope.selectedAnswerResponses, function (p, val3) {
                                                if (val3.IsChecked == true) {
                                                    if (val3.ResponseText > val.Response) {
                                                        answerresponse = true;
                                                        return false;
                                                    }
                                                    else {
                                                        answerresponse = false;

                                                    }

                                                }
                                                else {
                                                    count = count + 1;
                                                }


                                            });
                                            if (count == $scope.selectedAnswerResponses.length) {
                                                val2.IsHide = false;
                                            }
                                            else {
                                                if (answerresponse == true) {
                                                    val2.IsHide = true;

                                                }
                                                else {
                                                    val2.IsHide = false;
                                                }
                                                if (k < j) {
                                                    $scope.arraymove($scope.Questions, k, j);
                                                }
                                                else {
                                                    $scope.arraymove($scope.Questions, k, j + 1);
                                                }
                                            }

                                            return false;
                                        }
                                        //Less Than
                                        if (val.ConditionId == 193472) {
                                            var answerresponse = false;
                                            var count = 0;
                                            $.each($scope.selectedAnswerResponses, function (p, val3) {
                                                if (val3.IsChecked == true) {
                                                    if (parseFloat(val3.ResponseText) < parseFloat(val.Response)) {
                                                        answerresponse = true;
                                                        return false;
                                                    }
                                                    else {
                                                        answerresponse = false;
                                                    }

                                                }
                                                else {
                                                    count = count + 1;
                                                }


                                            });

                                            if (count == $scope.selectedAnswerResponses.length) {
                                                val2.IsHide = false;
                                            }
                                            else {
                                                if (answerresponse == true) {
                                                    val2.IsHide = true;

                                                }
                                                else {
                                                    val2.IsHide = false;
                                                }
                                                if (k < j) {
                                                    $scope.arraymove($scope.Questions, k, j);
                                                }
                                                else {
                                                    $scope.arraymove($scope.Questions, k, j + 1);
                                                }

                                            }


                                            return false;
                                        }


                                    }

                                    //End of Survey
                                    if (val.ActionId == '73300') {
                                        //Equal
                                        if (val.ConditionId == 73293) {
                                            var answerresponse = false;
                                            var valrequired = false;
                                            var count = 0;
                                            $.each($scope.selectedAnswerResponses, function (p, val3) {
                                                if (val3.IsChecked == true) {

                                                    if (val.Response == val3.ResponseText) {
                                                        answerresponse = false;
                                                        return false;
                                                    }
                                                    else {
                                                        answerresponse = true;
                                                    }

                                                }
                                                else {
                                                    count = count + 1;
                                                }



                                            });

                                            if (count == $scope.selectedAnswerResponses.length) {
                                                val2.IsHide = false;
                                            }
                                            else {
                                                if (answerresponse == false) {
                                                    val2.IsHide = false;
                                                    $scope.endsurvey = true;
                                                    if (k < j) {
                                                        $scope.arraymove($scope.Questions, k, j);
                                                    }
                                                    else {
                                                        $scope.arraymove($scope.Questions, k, j + 1);
                                                    }

                                                }
                                                else {

                                                    val2.IsHide = true;
                                                    $scope.endsurvey = false;

                                                }


                                            }



                                            return false;
                                        }


                                        //Not Equal
                                        if (val.ConditionId == 73297) {


                                            var answerresponse = false;
                                            var valrequired = false;
                                            var count = 0;
                                            $.each($scope.selectedAnswerResponses, function (p, val3) {
                                                if (val3.IsChecked == true) {

                                                    if (val.Response != val3.ResponseText) {
                                                        answerresponse = false;
                                                        return false;
                                                    }
                                                    else {
                                                        answerresponse = true;
                                                    }

                                                }
                                                else {
                                                    count = count + 1;
                                                }



                                            });

                                            if (count == $scope.selectedAnswerResponses.length) {
                                                val2.IsHide = false;
                                            }
                                            else {
                                                if (answerresponse == false) {
                                                    val2.IsHide = false;
                                                    $scope.endsurvey = true;
                                                    if (k < j) {
                                                        $scope.arraymove($scope.Questions, k, j);
                                                    }
                                                    else {
                                                        $scope.arraymove($scope.Questions, k, j + 1);
                                                    }

                                                }
                                                else {

                                                    val2.IsHide = true;
                                                    $scope.endsurvey = false;

                                                }


                                            }



                                            return false;
                                        }


                                        //Greater Than
                                        if (val.ConditionId == 193471) {
                                            var answerresponse = false;
                                            var valrequired = false;
                                            var count = 0;
                                            $.each($scope.selectedAnswerResponses, function (p, val3) {
                                                if (val3.IsChecked == true) {

                                                    if (parseFloat(val3.ResponseText) > parseFloat(val.Response)) {
                                                        answerresponse = false;
                                                        return false;
                                                    }
                                                    else {
                                                        answerresponse = true;
                                                    }

                                                }
                                                else {
                                                    count = count + 1;
                                                }



                                            });

                                            if (count == $scope.selectedAnswerResponses.length) {
                                                val2.IsHide = false;
                                            }
                                            else {
                                                if (answerresponse == false) {
                                                    val2.IsHide = false;
                                                    $scope.endsurvey = true;
                                                    if (k < j) {
                                                        $scope.arraymove($scope.Questions, k, j);
                                                    }
                                                    else {
                                                        $scope.arraymove($scope.Questions, k, j + 1);
                                                    }

                                                }
                                                else {

                                                    val2.IsHide = true;
                                                    $scope.endsurvey = false;

                                                }


                                            }



                                            return false;
                                        }
                                        //Less Than
                                        if (val.ConditionId == 193472) {
                                            var answerresponse = false;
                                            var valrequired = false;
                                            var count = 0;
                                            $.each($scope.selectedAnswerResponses, function (p, val3) {
                                                if (val3.IsChecked == true) {

                                                    if (parseFloat(val3.ResponseText) < parseFloat(val.Response)) {
                                                        answerresponse = false;
                                                        return false;
                                                    }
                                                    else {
                                                        answerresponse = true;
                                                    }

                                                }
                                                else {
                                                    count = count + 1;
                                                }



                                            });

                                            if (count == $scope.selectedAnswerResponses.length) {
                                                val2.IsHide = false;
                                            }
                                            else {
                                                if (answerresponse == false) {
                                                    val2.IsHide = false;
                                                    $scope.endsurvey = true;
                                                    if (k < j) {
                                                        $scope.arraymove($scope.Questions, k, j);
                                                    }
                                                    else {
                                                        $scope.arraymove($scope.Questions, k, j + 1);
                                                    }

                                                }
                                                else {

                                                    val2.IsHide = true;
                                                    $scope.endsurvey = false;

                                                }


                                            }



                                            return false;
                                        }


                                    }


                                }

                            });
                            return false;
                        }



                    });
                });

                //if ($scope.endsurvey == true) {
                //    $scope.endsurvey = false;
                //}
                if ($scope.endsurvey == true && $scope.IsSignatureRequired == true) {
                    // Logic For Required Questions
                    if ($scope.RequiredQuestions.length < 1) {
                        $('#singature-popup').modal('show');
                    }
                    else {
                        $('#RequiredQuestions-popup').modal('show');
                    }
                    //End Logic 
                }
                else if ($scope.endsurvey == true) {
                    if ($scope.RequiredQuestions.length > 0) {
                        $('#RequiredQuestions-popup').modal('show');
                    }
                    else {
                        $.notify("Section Completed Successfully", { type: "Please Select Option", color: "white", background: "#49a02d", blur: 0.6, delay: 0, });
                        $scope.LoadPreviewData($scope.SectionCurrent);
                        $scope.ShowPreview = true;
                        $scope.PreviewButtonShow = false;
                    }
                }
                else {


                    var index = $scope.Questions.findIndex(p => p.QuestionId == $scope.selectedQuestion.QuestionId);
                    if (index == $scope.Questions.length - 1) {
                        index = $scope.Questions.length - 1;
                    }
                    else {
                        index = index + 1;
                    }


                    $.each($scope.Questions, function (j, val) {
                        if (index == j) {
                            if (val.IsHide == true) {
                                if (index >= $scope.Questions.length - 1) {
                                    $scope.endsurvey = true;
                                }
                                else {
                                    index = index + 1;
                                }

                            }
                            else {
                                if (index == $scope.Questions.length - 1) {
                                    $scope.currentIndex = index;
                                    $scope.selectedQuestion = $scope.Questions[index];
                                    if ($scope.selectedQuestion.QuestionType == 'Table') {
                                        angular.forEach($scope.selectedQuestion.Responses,
                                            function (obj, index) {

                                                obj.SelectedValues = obj.SelectedRawResponse == null ? [] : obj.SelectedRawResponse.split('|');

                                            });
                                    }


                                    $scope.endsurvey = true;
                                    return false;
                                }
                                else {
                                    $scope.currentIndex = index;
                                    $scope.selectedQuestion = $scope.Questions[index];
                                    if ($scope.selectedQuestion.QuestionType == 'Table') {
                                        angular.forEach($scope.selectedQuestion.Responses,
                                            function (obj, index) {

                                                obj.SelectedValues = obj.SelectedRawResponse == null ? [] : obj.SelectedRawResponse.split('|');

                                            });
                                    }

                                    return false;
                                }

                            }

                        }

                        //  console.log($scope.Questions);


                    });

                }
            }
            else {
                if ($scope.endsurvey == true && $scope.IsSignatureRequired == true) {
                    // Logic For Required Questions
                    if ($scope.RequiredQuestions.length < 1) {
                        $('#singature-popup').modal('show');
                    }
                    else {
                        $('#RequiredQuestions-popup').modal('show');
                    }
                    //End Logic 
                }
                else if ($scope.endsurvey == true) {
                    if ($scope.RequiredQuestions.length > 0) {
                        $('#RequiredQuestions-popup').modal('show');
                    }
                    else {
                        $.notify("Section Completed Successfully", { type: "Please Select Option", color: "white", background: "#49a02d", blur: 0.6, delay: 0, });
                        //$scope.LoadPreviewData($scope.SectionCurrent);
                        //$scope.ShowPreview = true;

                        //$scope.PreviewButtonShow = false;
                    }
                }
                else {


                    var index = $scope.Questions.findIndex(p => p.QuestionId == $scope.selectedQuestion.QuestionId);
                    if (index == $scope.Questions.length - 1) {
                        index = $scope.Questions.length - 1;
                    }
                    else {
                        index = index + 1;
                    }


                    $.each($scope.Questions, function (j, val) {
                        if (index == j) {
                            if (val.IsHide == true) {
                                if (index >= $scope.Questions.length - 1) {
                                    $scope.endsurvey = true;
                                }
                                else {
                                    index = index + 1;
                                }

                            }
                            else {
                                if (index == $scope.Questions.length - 1) {
                                    $scope.currentIndex = index;
                                    $scope.selectedQuestion = $scope.Questions[index];
                                    if ($scope.selectedQuestion.QuestionType == 'Table') {
                                        angular.forEach($scope.selectedQuestion.Responses,
                                            function (obj, index) {

                                                obj.SelectedValues = obj.SelectedRawResponse == null ? [] : obj.SelectedRawResponse.split('|');

                                            });
                                    }


                                    $scope.endsurvey = true;
                                    return false;
                                }
                                else {
                                    $scope.currentIndex = index;
                                    $scope.selectedQuestion = $scope.Questions[index];
                                    if ($scope.selectedQuestion.QuestionType == 'Table') {
                                        angular.forEach($scope.selectedQuestion.Responses,
                                            function (obj, index) {

                                                obj.SelectedValues = obj.SelectedRawResponse == null ? [] : obj.SelectedRawResponse.split('|');

                                            });
                                    }

                                    return false;
                                }

                            }

                        }

                        //  console.log($scope.Questions);


                    });

                }

            }

        });


    }
    $scope.arraymove = function (arr, fromIndex, toIndex) {


        arr.splice(toIndex, 0, arr.splice(fromIndex, 1)[0]);

    }

    $scope.SaveSignature = function () {
        //debugger
        if (signaturePad.isEmpty() && $scope.SectionSignature == null) {
            $.notify("Please Sign to Finish", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
            return;
        }

        if ($scope.SelectedSection.IsCompleted === true) {
            $("#singature-popup").modal('hide');
            $.notify("Section Already Completed", { type: "Please Select Option", color: "#ffffff", background: "f39e43;", blur: 0.6, delay: 0, });
            return;
        }
        else if (!signaturePad.isEmpty()) {
            signaturePad.backgroundColor = "white";
            var dataUrl = signaturePad.toDataURL();
            console.log(dataUrl);
            $scope.SectionSignature = dataUrl;
            $http({
                url: "/Document/MarkSectionCompleted?id=" + $scope.SelectedSection.SectionId,
                method: "POST",
                headers: { 'Content-Type': "Application/json" },
                data: { base64String: dataUrl }
            }).then(function success(res) {
                $("#singature-popup").modal('hide');
                //$scope.SelectedSection.IsCompleted = true;
                $.notify("Section Completed Successfully", { type: "Please Select Option", color: "white", background: "#49a02d", blur: 0.6, delay: 0, });
                //$scope.LoadPreviewData($scope.SectionCurrent);
                //$scope.ShowPreview = true;
                //$scope.PreviewButtonShow = false;
            }, function () {
                $.notify("Internal Server Error", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
            });
            signaturePad.clear();
        }
        else if ($scope.SectionSignature != null && signaturePad.isEmpty()) {
            $("#singature-popup").modal('hide');
            $.notify("Section Completed Successfully", { type: "Please Select Option", color: "white", background: "#49a02d", blur: 0.6, delay: 0, });
            $scope.LoadPreviewData($scope.SectionCurrent);
            $scope.ShowPreview = true;
            $scope.PreviewButtonShow = false;
        }
    }

    $scope.ClearSignature = function () {
        $scope.SectionSignature = "";
        signaturePad.clear();
    }
    // For Dynamic Rows
    $scope.AddNewDynamicRow = function (data) {
        angular.forEach(data.Responses, function (val, index) {
            if (data.Responses[index].ResponseValue == 'TextBox') {
                if (data.Responses[index].UserValues != null && data.Responses[index].UserValues != "") {

                    if (data.Responses[index].IsReadOnly) {
                        val.TextBoxList[data.TotalRows] = {
                            IsChecked: false,
                            ResponseValue: data.Responses[index].UserValues
                        };
                    }
                }
            }
        })
        return data.TotalRows + 1;
    }
    $scope.DelNewDynamicRow = function (data, rowIndex) {
        var c = confirm('Are you sure you want to remove a row.')
        if (c) {
            if (data.TotalRows > 1) {
                //debugger;
                angular.forEach(data.Responses, function (obj, index) {
                    var temp = obj.SelectedRawResponse == null ? [] : obj.SelectedRawResponse.split('|');
                    if (temp.length > 1) {
                        temp.splice(rowIndex, 1);
                        obj.SelectedRawResponse = temp.join('|');
                    }

                    if (obj.TextBoxList != null && obj.TextBoxList != "" && obj.TextBoxList != undefined) {

                        obj.TextBoxList.splice(rowIndex, 1);
                    }
                    if (obj.SelectedValues != null && obj.SelectedValues != "" && obj.SelectedValues != undefined) {

                        obj.SelectedValues.splice(rowIndex, 1);
                    }
                    if (obj.arrayForDropDownListItems != null && obj.arrayForDropDownListItems != "" && obj.arrayForDropDownListItems != undefined && obj.arrayForDropDownListItems.length >= rowIndex) {
                        obj.arrayForDropDownListItems.splice(rowIndex, 1);
                    }
                    if (obj.arrayForCheckBoxItems != null && obj.arrayForCheckBoxItems != "" && obj.arrayForCheckBoxItems != undefined && obj.arrayForCheckBoxItems.length >= rowIndex) {
                        obj.arrayForCheckBoxItems.splice(rowIndex, 1);
                    }
                    if (obj.arrayForRadioButtonItems != null && obj.arrayForRadioButtonItems != "" && obj.arrayForRadioButtonItems != undefined && obj.arrayForRadioButtonItems.length >= rowIndex) {
                        obj.arrayForRadioButtonItems.splice(rowIndex, 1);
                    }
                    if (obj.arrayForTextBoxItems != null && obj.arrayForTextBoxItems != "" && obj.arrayForTextBoxItems != undefined && obj.arrayForTextBoxItems.length >= rowIndex) {
                        obj.arrayForTextBoxItems.splice(rowIndex, 1);
                    }
                });

                /// 
                $scope.Refresh();
                ///

                return data.TotalRows - 1;

            }
            else {
                return data.TotalRows;
            }
        }
        else {
            return data.TotalRows;
        }

    }

    $scope.Refresh = function () {
        $("div#divLoading").addClass('show');
        $scope.Previous();
        $scope.Next();
        $("div#divLoading").removeClass('show');
    }
    // End For Dynamic Rows 


    $scope.formatDate = function (date) {

        var dateOut = new Date(date);

        return dateOut;
    };

    $scope.CompleteSectionStatus = function (sectionId) {
        //$http({
        //    url: "/Document/GetSectionQuestions_For_Preview?id=" + sectionId + "&surveyId=" + SurveyId
        //}).then(function success(res) {
        //    //debugger;
        //    $scope.RequiredQuestions = [];
        //    angular.forEach(res.data, function (question) {
        //        var isQuestionAnswered = false;
        //        angular.forEach(question.Responses, function (c) {
        //            if (c.IsChecked && (c.ResponseValue != "" || c.ResponseText != ""))
        //                isQuestionAnswered = true;
        //        });
        //        if (question.IsRequired && !isQuestionAnswered)
        //            $scope.RequiredQuestions.push(question);
        //        isQuestionAnswered = false;
        //    });
        //    if ($scope.RequiredQuestions.length > 0) {
        //        $('#RequiredQuestions-popup').modal('show');
        //    }
        //    else {
        $http({
            url: "/Document/MarkSectionStatus?sectionId=" + sectionId + "&status=92"
        }).then(function success(res) {
            $.notify(res.data.Message, { type: "Please Select Option", color: "#ffffff", background: res.data.Status, blur: 0.6, delay: 0, });
            loadSections();
            $scope.selectedQuestion = {};
            $scope.selectedQuestion.Question = 'Please Select a Section';
        }, function () {
            $.notify("Internal Server Error", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
        });
        //    }
        //}, function () {
        //    $.notify("Internal Server Error", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
        //    return;
        //});

        if ($scope.SelectedSection.IsCompleted === true) {
            $.notify("Section Already Completed", { type: "Please Select Option", color: "#ffffff", background: "f39e43;", blur: 0.6, delay: 0, });
            return;
        }


    }

    $scope.MarkSectionStatus = function (sectionId) {

        $http({
            url: "/Document/MarkSectionStatus?sectionId=" + sectionId + "&status=93"
        }).then(function success(res) {
            $.notify(res.data.Message, { type: "Please Select Option", color: "#ffffff", background: res.data.Status, blur: 0.6, delay: 0, });
            loadSections();
        }, function () {
            $.notify("Internal Server Error", { type: "Please Select Option", color: "#ffffff", background: "red", blur: 0.6, delay: 0, });
        });
    }

    $scope.ShowConfirmStatusPopUp = function (data, SectionStatus) {
        //$('#ConfirmStatusChanged-popup').modal('show');
        $scope.StatusChangedText = SectionStatus;
        if (SectionStatus == 'Pending') {
            $scope.TextForStatus = 'Pending With Issues';
        }
        else if (SectionStatus == 'Completed') {
            $scope.TextForStatus = 'Completed';
        }
        $scope.SectionIdToChangeStatus = data;
        $.confirm({
            title: 'Confirm!',
            content: 'Do you want to mark this section as <b>'+$scope.TextForStatus+'</b> ?',
            buttons: {
                Yes: function () {
                    $scope.StatusChangedFunctionCall($scope.StatusChangedText, $scope.SectionIdToChangeStatus)
                },
                No: function () {
                  
                }
            }
        });
      
    }

    $scope.StatusChangedFunctionCall = function (text, SectionId) {
        if (text === 'Pending') {
            $scope.MarkSectionStatus(SectionId);
        }
        else if (text === 'Completed') {
            $scope.CompleteSectionStatus(SectionId);
        }

    }

    $scope.OpenMapPopUp = function (section) {
        //debugger;
        $scope.MapLatLngArray = "";
        $scope.ShowMapClearButton = false;
        $scope.ShowMapFanButton = false;
        if ($scope.selectedQuestion.Responses.length > 0) {
            if ($scope.selectedQuestion.Responses[0].ResponseValue != "") {
                $scope.MapLatLngArray = $scope.selectedQuestion.Responses[0].ResponseValue;
            }
        }
        if ($scope.MapLatLngArray == "") {
            $scope.MapLatLngArray = section.Latitude + "," + section.Longitude;
            $scope.selectedQuestion.Responses[0].ResponseValue = section.Latitude + "," + section.Longitude;

        }
        if ($scope.selectedQuestion.Azimuth != "" && $scope.selectedQuestion.Azimuth != null) {
            $scope.ShowMapClearButton = true;
            $scope.ShowMapFanButton = false;
            LoadMapWithPoly($scope.MapLatLngArray, $scope.selectedQuestion.IsMultiLocation, $scope.selectedQuestion.MapZoom, $scope.selectedQuestion.Azimuth)
        }
        else {
            LoadMap($scope.MapLatLngArray, $scope.selectedQuestion.IsMultiLocation, $scope.selectedQuestion.MapZoom);
            if($scope.selectedQuestion.Azimuth != "" && $scope.selectedQuestion.Azimuth != null)
            {
                $scope.ShowMapClearButton = false;
                $scope.ShowMapFanButton = true;
            }
           
        }
        $('#ShowMap-popup').modal('show');
    }
    $scope.SaveLatLong = function () {
        //$scope.SaveButtonShow = false;
        //var base64;
        //html2canvas($('.gm-style>div:first')[0],
        //{
        //    useCORS: true,
        //}).then(function (canvas) {
        //    base64 = canvas.toDataURL();
        //    $scope.selectedQuestion.MapImage = base64;
        //    $('#ShowMap-popup').modal('hide');
        //    $scope.SaveButtonShow = true;
        //});
        $('#ShowMap-popup').modal('hide');
        $scope.SaveButtonShow = true;
    }
    // Code To Check Whether to Show Status Icon or Not

    $scope.CheckIconForStatus = function (SectionID) {
        //debugger;
        var result = false;
        if ($scope.ArrayOfSectionsToShowIcons.indexOf(SectionID) !== -1) {
            result = true;
        }
        else {
            result = false;
        }
        return result;
    }
    $scope.ChangeNames = function (Name) {
        if (Name == 'Drive Completed') {
            Name = 'Completed';
        }
        if (Name == 'Pending With Issues') {
            Name = 'Issues';
        }
        if (Name == 'Pending Schedule') {
            Name = 'Pending';
        }
        if (Name == 'Schedule') {
            Name = 'Pending';
        }
        return Name;

    }

    $scope.GetDocTitle = function (path) {
        //debugger;
        var last = path.substring(path.lastIndexOf("/") + 1, path.length);
        $scope.TextToShowDoc = last;
        return last;

    }

    $scope.GetAudioTitle = function (path) {
        //debugger;
        var last = path.substring(path.lastIndexOf("/") + 1, path.length);
        $scope.TextToShowAudio = last;
        return last;

    }

    $scope.GetDocTitlePreview = function (path) {
        //debugger;
        var last = path.substring(path.lastIndexOf("/") + 1, path.length);
        $scope.TextToShowDocpreview = last;
        return last;

    }

    $scope.GetAudioTitlePreview = function (path) {
        //debugger;
        var last = path.substring(path.lastIndexOf("/") + 1, path.length);
        $scope.TextToShowAudiopreview = last;
        return last;

    }

    $scope.ShowPopUpMedia = function (url, name) {
        $scope.mediaName = name;
        $scope.mediaSrc = url;
        $('#Media-popup').modal('show');
    }

    $scope.showPopOver = function (e) {
        //debugger;
        var element = $(e.currentTarget)[0];
        var id = element.id;
        var actionId = $(element).attr('data-actionid');
        $scope.ActionIdToSave = actionId;
        $scope.LoadRequiredActionData(actionId);
        var content = $("#PopOver_Content_Wrapper").html();
        var idnew = "#" + id;

        $(idnew).popover({
            html: true,
            placement: 'top',
            title: 'Details <span style="float:right" onclick="hidePopOvers(this)"><i class="fa fa-times"></i></span>',
            container: 'body',
            content: function () {
                return content;
            }
        });
    }
    $scope.SaveActionData = function (ActionId, remarks, azumith, lat, long, objView, altitude, gpsAccuracy) {
        //debugger;
        $(".popovercls").popover('hide');
        angular.forEach($scope.selectedQuestion.ReqActions, function (val, key) {
            if (val.ActionId == ActionId) {
                if (val.Azimuth == undefined) {
                    val.Remarks = remarks;
                    val.Azimuth = azumith;
                    val.Latitude = lat;
                    val.Longitude = long;
                    val.ObjectView = objView;
                    val.Altitude = altitude;
                    val.GPSAccuracy = gpsAccuracy;
                }
                else {
                    val.Remarks = remarks;
                    val.Azimuth = azumith;
                    val.Latitude = lat;
                    val.Longitude = long;
                    val.ObjectView = objView;
                    val.Altitude = altitude;
                    val.GPSAccuracy = gpsAccuracy;
                }
            }
        });
        //  $.notify("Success!", { type: "Please Select Option", color: "#ffffff", background: "green", blur: 0.6, delay: 0, });

    }

    $scope.LoadRequiredActionData = function (id) {
        //debugger;
        var popRemarks = '';
        var popAzimuth = '';
        var popLatitude = '';
        var popLongitude = '';
        var objView = '';
        var Altitude = '';
        var GPSAccuracy = '';
        angular.forEach($scope.selectedQuestion.ReqActions, function (val, index) {
            if (val.ActionId == id) {
                if (val.Azimuth != undefined) {
                    popRemarks = val.Remarks;
                    popAzimuth = val.Azimuth;
                    popLatitude = val.Latitude;
                    popLongitude = val.Longitude;
                    objView = val.ObjectView;
                    Altitude = val.Altitude;
                    GPSAccuracy = val.GPSAccuracy;

                }
            }

        });
        LoadDataManually(popRemarks, popAzimuth, popLatitude, popLongitude, objView, Altitude, GPSAccuracy);
    }

    $scope.GetToolTipData = function (rq) {
        ////debugger;
        var Tooltip = "";
        if (rq.Latitude != "") {
            Tooltip = Tooltip + "<b>Latitude:</b>" + rq.Latitude + "<br/>";
        }
        if (rq.Longitude != "") {
            Tooltip = Tooltip + "<b>Longitude:</b>" + rq.Longitude + "<br/>";
        }
        if (rq.Altitude != "") {
            Tooltip = Tooltip + "<b>Altitude:</b>" + rq.Altitude + "<br/>";
        }
        if (rq.Azimuth != "") {
            Tooltip = Tooltip + "<b>Azimuth:</b>" + rq.Azimuth + "<br/>";
        }
        if (rq.ObjectView != "") {
            Tooltip = Tooltip + "<b>View:</b>" + rq.ObjectView + "<br/>";
        }
        if (rq.GPSAccuracy != "") {
            Tooltip = Tooltip + "<b>GPSAccuracy:</b>" + rq.GPSAccuracy + "<br/>";
        }
        if (rq.Remarks != "") {
            Tooltip = Tooltip + "<b>Remarks:</b>" + rq.Remarks + "<br/>";
        }
        if (Tooltip == "") {
            Tooltip = "Image";
        }
        return Tooltip;
    }


    $scope.GetValue = function (data) {
        if (data.ResponseValue != "") {
            var a = parseInt(data.ResponseValue);
            return a;
        }
        else return data.MinValue;
    }

    $scope.Loadit = function () {
        loadMultiple();
    }
    $scope.ShowQuestionCanvas = function () {
        ShowQPad();
    }
    $scope.HideQuestionCanvas = function () {
        HideQPad();
    }

    $scope.UpdateRecords = function (data, mapZoom) {
        //debugger;
        $scope.selectedQuestion.MapZoom = mapZoom;
        $scope.MapLatLngArray = data;
        $scope.selectedQuestion.Responses[0].IsChecked = true;
        $scope.selectedQuestion.Responses[0].ResponseValue = $scope.MapLatLngArray;
    }

    $scope.LoadMapWithoutFan=function()
    {
        $scope.ShowMapFanButton = true;
        $scope.ShowMapClearButton = false;
        LoadMap($scope.MapLatLngArray, $scope.selectedQuestion.IsMultiLocation, $scope.selectedQuestion.MapZoom);
    }
    $scope.LoadMapWithFan = function () {
        $scope.ShowMapFanButton = false;
        $scope.ShowMapClearButton = true;
        LoadMapWithPoly($scope.MapLatLngArray, $scope.selectedQuestion.IsMultiLocation, $scope.selectedQuestion.MapZoom, $scope.selectedQuestion.Azimuth)
    }

    $scope.SaveMinMaxValue = function (data) {
        var MinMaxValues = [];
        MinMaxValues = data.UserValues.split(',');
        var a = parseInt(MinMaxValues[0]);
        var b = parseInt(MinMaxValues[1]);

        // Get Min Max Percentage and Remaining Percentages

        $scope.MinPercentage = Math.round((data.MinValue -a) * 100 / (b-a));
        $scope.MinRemainingPercentage = 100 - $scope.MinPercentage;


        $scope.MaxPercentage = Math.round((data.MaxValue - a) * 100 / (b - a));
        $scope.MaxRemainingPercentage = 100 - $scope.MaxPercentage;


        $scope.MinVal = [];
        $scope.MaxVal = [];
        $scope.MinVal.push(a);
        $scope.MaxVal.push(b);
        
    }
    $scope.paseValue=function(value){
        $("#editor").summernote('code', value);
    }
    //$scope.TestingReplace = function () {
    //    debugger;
    //    var data = "<p><br></p><p><b><br></p>";
    //    var NewString = data.replace(/<br>/gi, '<br/>');
    //    var stooper = "";
    //}
    $scope.OpenMapForTableType = function (LatLng, index, col,rows,section) {
        debugger;
        var latlngcurrent = "";
        if ($scope.selectedQuestion.Responses.length > 0) {
            if (LatLng != "" && LatLng != undefined) {
                latlngcurrent=LatLng;
            }
            else
            {
                latlngcurrent = section.Latitude + "," + section.Longitude;
            }
        }

        $scope.MapTableIndex = index;
        $scope.MapTableCol = col;
        $scope.rows = rows;
        LoadMapTableType(latlngcurrent, index, col, rows);
        $('#ShowMapTableType-popup').modal('show');
    }
    $scope.UpdateRecordsForTableTypeMap = function (latlng, index, col, rows) {
        debugger;
        if (index < col.LatLngList.length) {

            col.LatLngList[index].ResponseValue = latlng;
        }
        else {
            col.LatLngList.push({
                IsChecked: false,
                ResponseValue: latlng
            });
        }
        $scope.MapLatLngArrayTableType = latlng;
        $scope.MapBoxChanged(latlng, index, col, rows);
    }
    $scope.SaveMapForTableType = function () {
        debugger;
        var ln = $scope.MapLatLngArrayTableType;
        var col = $scope.MapTableCol;
        var index = $scope.MapTableIndex;
        var rows = $scope.rows;
        if (index < col.LatLngList.length) {

            col.LatLngList[index].ResponseValue = ln;
        }
        else {
            col.LatLngList.push({
                IsChecked: false,
                ResponseValue: ln
            });
        }
        $scope.MapBoxChanged(ln, index, col, rows);
        $('#ShowMapTableType-popup').modal('hide');
    }
    $scope.GetDateTimeControl = function (response) {
        debugger;
        if (response[0].ResponseValue != "Invalid Date") {
            var date = new Date(response[0].ResponseValue);
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
        $scope.selectedQuestion.Responses[0].ResponseValue = newDate;
            $('.datetimepicker1').datetimepicker({
                defaultDate: date,
                sideBySide: true,
                toolbarPlacement: "bottom",
                showClose: true,
                format: "M/D/YYYY, HH:mm A"
                

            }).on('dp.change', function (e) {
                debugger;
                var d=new Date(e.date._d);
                var tempdate = d.toLocaleString(undefined, {
                    day: '2-digit',
                    month: 'numeric',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit'
                });
                $timeout($scope.selectedQuestion.Responses[0].ResponseValue = tempdate);
            })
        
    }
    $scope.Gethtml = function (encoded) {
        debugger;
        return $sce.trustAsHtml(encoded);

    }
});
