var app = angular.module('app', ['ngSanitize', 'queryBuilder', 'ui.sortable', 'ui.multiselect', 'ui.bootstrap']);
app.controller('ReportController', ['$scope', '$http', '$modal', '$log', '$timeout', '$rootScope', 'Excel', function ($scope, $http, $modal, $log, $timeout, $rootScope, Excel) {
    $rootScope.testing = "junaid";
    $scope.searchText = ""
    $('#mydiv').hide();
    $scope.open = function () {

        $modal.open({
            templateUrl: 'myModalContent.html',
            backdrop: true,
            windowClass: 'modal',
            controller: function ($scope, $modalInstance, $log, user) {
                $scope.user = user;
                $scope.submit = function () {
                    $log.log('Submiting user info.');
                    $log.log(user);
                    $modalInstance.dismiss('cancel');
                }
                $scope.cancel = function () {
                    $modalInstance.dismiss('cancel');
                };
            },
            resolve: {
                user: function () {
                    return $scope.user;
                }
            }
        });
    };
    $scope.rules = [];
    //$scope.group = [];
    $scope.json = {};
    $scope.group = {
        operator: "",
        rules: []
    }
   
    $scope.exportToExcel = function (tableId) { // ex: '#my-table'
        var exportHref = Excel.tableToExcel(tableId, 'WireWorkbenchDataExport');
        $timeout(function () { location.href = exportHref; }, 100); // trigger download
    }
    $scope.json.string = "{\n  \"id\": \"0001\",\n  \"type\": \"donut\",\n  \"name\": \"Cake\",\n  \"ppu\": 0.55,\n  \"batters\":\n    {\n      \"batter\":\n        [\n          { \"id\": \"1001\", \"type\": \"Regular\" },\n          { \"id\": \"1002\", \"type\": \"Chocolate\" },\n          { \"id\": \"1003\", \"type\": \"Blueberry\" },\n          { \"id\": \"1004\", \"type\": \"Devil's Food\" }\n        ]\n    },\n  \"topping\":\n    [\n      { \"id\": \"5001\", \"type\": \"None\" },\n      { \"id\": \"5002\", \"type\": \"Glazed\" },\n      { \"id\": \"5005\", \"type\": \"Sugar\" },\n      { \"id\": \"5007\", \"type\": \"Powdered Sugar\" },\n      { \"id\": \"5006\", \"type\": \"Chocolate with Sprinkles\" },\n      { \"id\": \"5003\", \"type\": \"Chocolate\" },\n      { \"id\": \"5004\", \"type\": \"Maple\" }\n    ]\n}";
    //var string = "[{\n \"FirstName\":\"junaid\" ,\n \"Age\":\"25\"},{\n \"FirstName\":\"junaid\" ,\n \"Age\":\"25\"}]"
    $scope.json.object = angular.fromJson($scope.json.string);

    //This code is for calling directive fucntion

    $scope.CallDirective = function () {
        $rootScope.$emit('callDirective', $scope.Column);
    };


    $scope.list = [];

    //sortable code

    var tmpList = [];

    var str1;

    $scope.list = tmpList;

    $scope.Object1 = []


    $scope.sortingLog = [];

    $scope.sortableOptions = {
        update: function (e, ui) {


            var logEntry = tmpList.map(function (i) {
                return i.value;
            }).join(', ');
            $scope.sortingLog.push('Update: ' + logEntry);
        },
        stop: function (e, ui) {


            // this callback has the changed model
            var logEntry = tmpList.map(function (i) {
                return i.value;
            }).join(', ');
            $scope.sortingLog.push('Stop: ' + logEntry);
        }

       
    };
    $('div').removeClass("wrapper");
    //sortable end
    $scope.managCss = function () {

        if ($('#collapseOne').hasClass('in')) {
            $('.artical').css({ 'margin-top': '31px', 'margin-left': '-3px' })
            $('.innerartical').css({ "height": "700px", "max-height": "700px" })
            //$('#output1').css("height", "637px")
            //$('.pvtUi').css({"height": "637px",'margin-top':'205px','max-height':'637px'})
        }
        else {
            $('.artical').css({ 'margin-top': '-8px', 'margin-left': '-3px' })

            $('.innerartical').css({ "height": "600px", "max-height": "600px" })
            //$('#output1').css("height", "345px")
            //$('.pvtUi').css({ "height": "345px", 'margin-top': '0px', 'max-height': '347px' })
        }



    }

    function CSS() {
        //$('body').notify({
        //    message: 'Hello World',
        //    type: 'danger'
        //});
        $('div').removeClass("wrapper");
        $('.content-wrapper').css("min-height", "0");
        $('footer').remove();
        $('.removeborber').each(function (i) {

            if (i % 2 == 0) {

                $(this).css({ 'background': "white" });
            }
            else {
                $(this).css({ 'background': "#d2d2d2" });

            }
        });

        $('.MainGroup').css({ "background": "#eee" });
        //$('.MainGroup').first().css("background", "white");
    }


    $scope.jsonobject = []

    var data = '{"group": {"operator": "AND","rules": []}}';

    function htmlEntities(str) {
        return String(str)//.replace(/</g, '&lt;').replace(/>/g, '&gt;');
    }
    function addremoveqoute(str, flag) {
        if (flag == "System.DateTime") {
            dt = new Date(str);
            str = dt.getFullYear() + "/" + (dt.getMonth() + 1) + "/" + dt.getDate();
        }
        if (flag == "System.Integer") {
            return String(str);
        }
        else {
            str = "'" + str + "'"
            return String(str);
        }

    }

    $scope.isShow = 0;
    var haskey = null;


    var str
    function Test(group) {
        for (var i = 0; i < group.rules.length; i++) {
            if (group.rules[i].group.mainclass == 'm') {
                $('.removeborber').last().addClass('MainGroup');
            }

        }

    }
    function computed(group) {
        $timeout(function () {

            CSS();
        }, 100);
        $scope.group.rules = []

        $timeout(function () {

            if (group.rules.length == 0 && $scope.isShow == 0) {
                $('.first').last().hide();
                $('.first1').last().hide();
                $('.first2').last().hide();
                $('.first3').last().hide();
                $('.removeborber').first().css("border", "0");


                $scope.isShow = 1;
            }
        }, 100);
        $('.table1').first().css("margin-top", "-20px");
        $('.removeborber').first().css("border", "0");


        for (i = 0; i < group.rules.length; i++) {
            //This code is for the buttons on the last row of condition
            try {
                if (group.rules[i].group) {
                    group.rules[i - 1].groupstart = true;
                }
            } catch (e) {

            }

            //This code is for removing the buttons from the group top
            try {
                if (group.rules[i].group) {
                    if (group.rules[i].group.rules[0].field != undefined) {
                        group.rules[i].group.firstgroup = true;
                        //group.rules[1].group.rules[0].group.firstgroup = true;
                        if (group.rules[i].group.rules[group.rules[i].group.rules.length - 1].group.rules[0].field != "") {

                            group.rules[1].group.rules[0].group.firstgroup = true;

                        }
                    }

                }
            } catch (e) {

            }


        }


        if (!group) return "";


        for (var str = "(", i = 0; i < group.rules.length; i++) {



            if (group.rules[0].group) {
                $('.btnremove').hide();
                $('.btnremove:first').show();
                $('.btnremoveclrAll').hide();
                $('.btnremoveclrAll:first').show();
                $('.btnDiv').first().next().css("margin-top", "-22px");
                group.rules[0].group.removefirstcondition = false;
            }

            //This code is for removing condition operator before sub group.
            if (!group.rules[i].group) {
                try {
                    if (group.rules[i + 1].group != undefined) {

                        group.rules[i].isoperator = false;
                    }
                    else {
                        group.rules[i].isoperator = true;
                    }
                } catch (e) {
                    group.rules[i].isoperator = true;

                }


            }

            // This code is adding group operator in string 
            if (i > 0 && group.rules[i].group) {

                (str += group.rules[i].group.operator)
                if (i > 0) {
                    group.rules[i].isgroupoperator = false
                }
            }



            str += group.rules[i].group ?
                computed(group.rules[i].group) :
                group.rules[i].field + " " + htmlEntities(group.rules[i].condition) + " " + addremoveqoute(group.rules[i].data, group.rules[i].type) + " " + group.rules[i].operator + " ";
        }

        str1 = str;

        return str + ")";

    }
    var array = [];
    $scope.col_defs = [];
    $scope.Columselect = true;
    var jsonarray = '';
    var array = []
    var rows = [];
    var cols = [];
    var vals = [];
    function load_js() {
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        //script.type = 'text/javascript';
        script.src = '/Content/js/Plugins/AngularQueryBuilder/js/pivot.js';
        head.appendChild(script);
    }

    
   
    $scope.Search = function () {
       
        rows = [];
        vals = [];
        cols = [];
        array = [];
        $scope.selectstring = " ";
        $scope.strGroup = " ";

        //$("#output1").empty();
        //load_js();
        $scope.Group = "";
        var cnt = 0;
        
        
        
        if ($scope.isPivot) {
            
            $('#mydiv').show();
            cnt = 1;
           
            for (var i = 0; i < $scope.Object1.length; i++) {
           
                    if ($scope.Object1[i].pivot != undefined) {
                        if ($scope.Object1[i].pivot == "Row") {
                            rows.push($scope.Object1[i].name);
                        }
                        else if ($scope.Object1[i].pivot == "Col") {
                            cols.push($scope.Object1[i].name);
                        }
                        else if ($scope.Object1[i].pivot == "Val") {
                            vals.push($scope.Object1[i].name);
                        }
                    }
                    else {
                        Notify("Fill All Pivot Colunms")
                        return;
                    }
                }
           
               
            
            $('#mydiv').hide();

        }



        for (var i = 0; i < $scope.Object1.length; i++) {
           
            if ($scope.Object1[i].function == undefined) {
                $scope.Group = $scope.Group + $scope.Object1[i].name + ' ' + ','
            }
            if ($scope.Object1[i].function != null && $scope.Object1[i].function != "" && $scope.Object1[i].function != undefined) {
                $scope.selectstring = $scope.selectstring + ' ,' + $scope.Object1[i].function + '(' + $scope.Object1[i].name + ')';
            }
            else {
                $scope.selectstring = $scope.selectstring + ' ,' + $scope.Object1[i].name;
            }

            if ($scope.Object1[i].alias != null || $scope.Object1[i].alias != "" && $scope.Object1[i].alias != undefined) {
                $scope.selectstring = $scope.selectstring + ' AS ' + $scope.Object1[i].alias;
            }
        }
        $('#mydiv').hide();
        $scope.selectstring = $scope.selectstring.substring(3, $scope.selectstring.length);

        $scope.Group = $scope.Group.slice(0, -1);

        $scope.selectstring = encodeURIComponent($scope.selectstring);
        $scope.Group = encodeURIComponent($scope.Group);



        if ($scope.Object1.length == 0) {
            $scope.Columselect = false;
            $scope.selectstring = "*";
        }
        else {
            $scope.Columselect = true;
        }
       
        var out = {};
        $('#mydiv').show();
        $http.get("/ReportTest/GetReportrecord?reportFilter=" + $scope.MapColumn + "&where=" + str1 + ')' + "&select=" + $scope.selectstring + "&group=" + $scope.Group).then(function (response) {

            $scope.selectedColumns = []
            for (var i = 0; i < response.data[0].length; i++) {
                $scope.selectedColumns.push({ name: response.data[0][i].Key });
            }
            for (var i = 0; i < response.data.length; i++) {
                out = {}
                for (var j = 0; j < response.data[i].length; j++) {
                    out[response.data[i][j].Key] = response.data[i][j].Value;
                }
                array.push(out);
                $('#mydiv').hide();
            }
            jsonarray = JSON.stringify(array)


            $scope.Table = response.data;
            $("#output1").pivotUI(
                      array, {
                          rows: rows,
                          cols: cols,
                          vals: vals,
                          aggregatorName: "Count",
                          rendererName: "Table",
                          renderers: $.extend(
                              $.pivotUtilities.renderers,
                              $.pivotUtilities.c3_renderers
                              )
                      });

        });
        //debugger

        //$('script').each(function () {
        //    if ($(this).attr('src') == '/Content/js/Plugins/AngularQueryBuilder/js/pivot.js') {
        //        var old_src = $(this).attr('src');
        //        $(this).attr('src', '');
        //        setTimeout(function () { $(this).attr('src', old_src + '?' + new Date()); }, 1000);
        //    }
        //});
        //if ($scope.isPivot == true) {
        //    $timeout(function () {
        //        $("#output1").pivotUI(
        //            array, {
        //                rows: rows,
        //                cols: cols,
        //                vals: vals,                        
        //                aggregatorName: "Count",
        //                rendererName: "Table",
        //                renderers: $.extend(
        //                    $.pivotUtilities.renderers,
        //                    $.pivotUtilities.c3_renderers
        //                    )
        //            });
        //    }, 3000)
        //}

       

    }

    $scope.RemoveRow = function (index) {

        $scope.Uncheck($scope.Object1[index].name)
        $scope.Object1.splice(index, 1);
    }
    $scope.Uncheck = function (value) {
        for (var i = 0; i < $scope.Column.length; i++) {
            if ($scope.Column[i].name == value) {
                $scope.Column[i].selected = 'NO';
            }

        }
    }
    $scope.Group = ""
    $scope.GroupByField = function () {
        $scope.Group = ""
        for (var i = 0; i < $scope.Object1.length; i++) {
            if ($scope.Object1[i].function == "") {
                $scope.Group = $scope.Group + $scope.Object1[i].name + ' ' + ','
            }
        }
        $scope.Group = $scope.Group.splice(0, -1);

    }

    $scope.exportData = function () {

        alasql('SELECT * INTO XLSX("john.xlsx",{headers:true}) FROM ?', [$scope.Table]);
    };

    //$scope.paginate = function (value) {
    //    var begin, end, index;
    //    begin = ($scope.currentPage - 1) * $scope.numPerPage;
    //    end = begin + $scope.numPerPage;
    //    index = $scope.objExpando.indexOf(value);
    //    return (begin <= index && index < end);
    //};
    $scope.hidefromtabel = false;
    $scope.printToCart = function (printSectionId) {

        //$(".tbl").css("border-bottom", "1px solid black");
        $("tr.tbl").css("border-bottom", "2px solid red !important");
        $(".tbl").css("border-bottom", "2px solid red !important");
        $("#row").css("border-bottom", "2px solid red !important");
        $(".pvtAxisContainer, .pvtAttrDropdown, .pvtRowOrder, .pvtColOrder,.pvtVals,.pvtUnused,.pvtRenderer ").hide();
        //$("tr").css("border", "1px solid");

        $("td").css({ "border": "1px solid" }, { "border-collapse": "collapse" }, { "padding": "0" });


        $scope.hidefromtabel = true;
        $('.hidefromtabel').hide();

        var innerContents = document.getElementById(printSectionId).innerHTML;
        var popupWinindow = window.open('', '_blank', 'width=600,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
        popupWinindow.document.open();
        popupWinindow.document.write('<html><head><link rel="stylesheet" type="text/css" href="../css/StyleQueryBuilder.css" /></head><body onload="window.print()">' + innerContents + '</html>');
        $scope.hidefromtabel = false;
        popupWinindow.document.close();
        $("td").css({ "border": "0" });
        $('.hidefromtabel').show();
        $(".pvtAxisContainer, .pvtAttrDropdown, .pvtRowOrder, .pvtColOrder,.pvtVals,.pvtUnused,.pvtRenderer ").show();
        //$(".tbl").css("border", "1px");
        //$("tr.tbl").css("border-bottom-color", "#808080");
    }
    $scope.json = null;

    $scope.filter = JSON.parse(data);

    $scope.$watch('filter', function (newValue) {
        $(".branch-value").remove("ng-hide")
        $scope.json = JSON.stringify(newValue, null, 2);
        $scope.json.object = newValue;

        $scope.output = computed(newValue.group);

    }, true);
    $scope.Report;
    $scope.Column = [];
    $scope.ColumnId = [];

    $scope.getAllReports = function () {
        $http.get("/ReportTest/GetReport?Filter=byDefinationType").then(function (response) {

            $scope.Report = response.data;

        })

    }

    $scope.getColumnByReportId = function () {
        $('#mydiv').show();
        $scope.Column = [];
        $scope.selectedColumns = [];
        $scope.Table = [];
        $scope.list = [];
        $scope.ColumnId = [];
        $scope.Object1 = [];
        $scope.Columselect = true;
        $http.get("/ReportTest/GetReportrecord?reportFilter=" + $scope.MapColumn + '&where=1=0' + '&select=*&group=null').then(function (response) {

            $scope.Column = response.data;

            $scope.CallDirective();

            $rootScope.testing = response.data;
            var obj = [];




        })
        $('#mydiv').hide();
    }
    $scope.Object1 = [];
    $scope.GroupBy = function (index) {
       
        //$rootScope.$emit('callDirective', $scope.ColumnId);
        $('#mydiv').show();
        var obj = [];


        $scope.selectedColumns = [];
        //for (var i = 0; i < $scope.Column.length; i++) {
        if ($scope.Column[index].selected == "YES") {
            $scope.Object1.push({ name: $scope.Column[index].name, DataType: $scope.Column[index].DataType, index: index });

        }
        else {
            for (var i = 0; i < $scope.Object1.length; i++) {
                if ($scope.Object1[i].index == index) {
                    $scope.Object1.splice(i, 1);

                }
            }
            //$scope.Object1[].slice(0,index)
        }

        //}
        //for (var i = 0; i < $scope.ColumnId.length; i++) {
        //    if ($scope.ColumnId[i]) {

        //    }
        //    obj.push({ name: $scope.ColumnId[i] })
        //    $scope.Object1.push({ name: $scope.ColumnId[i] });
        //}
        $scope.selectedColumns = obj;
        $scope.list = $scope.selectedColumnId;
        $('#mydiv').hide();
    }
    $scope.ReorderList = function () {
        debugger;
        $scope.list = $scope.selectedColumnId;
    }
    $scope.getAllReports();
}]);


app.directive("regExInput", function () {
    "use strict";
    return {
        restrict: "A",
        require: "?regEx",
        scope: {},
        replace: false,
        link: function (scope, element, attrs, ctrl) {
            element.bind('keypress', function (event) {
                var regex = new RegExp(attrs.regEx);
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            });
        }
    };
});
app.factory('Excel', function ($window) {
    var uri = 'data:application/vnd.ms-excel;base64,',
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
        base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
        format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
    return {
        tableToExcel: function (tableId, worksheetName) {
            var table = $(tableId),
                ctx = { worksheet: worksheetName, table: table.html() },
                href = uri + base64(format(template, ctx));
            return href;
        }
    };
})
    .controller('MyCtrl', function (Excel, $timeout, $scope) {
        $scope.exportToExcel = function (tableId) { // ex: '#my-table'
            var exportHref = Excel.tableToExcel(tableId, 'WireWorkbenchDataExport');
            $timeout(function () { location.href = exportHref; }, 100); // trigger download
        }
    });


