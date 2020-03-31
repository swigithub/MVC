var app = angular.module('mainApp', ["ngMap", 'ui.bootstrap', 'popoverToggle', 'colorpicker.module', 'ngSanitize', 'queryBuilder']);



app.controller('mapCtrl', function ($scope, $http, $location, $timeout, NgMap) {
    var Id = window.location.href.substr(window.location.href.lastIndexOf('/') + 1);
 
    var Location = location.host;
    $scope.path = null;
    $scope.results = [];
    $scope.title;
    $scope.loader = false;
    $scope.NetworkLayerId;
    $scope.SiteCode;
    $scope.getChilds = function (value) {
        $scope.title = value;
        $scope.results = [];
        $scope.findValue(value)
    }
    //Query Builder Start form here

    function CSS() {
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

    }
    var data = '{"group": {"operator": "AND","rules": []}}';
    function htmlEntities(str) {
        return String(str)//.replace(/</g, '&lt;').replace(/>/g, '&gt;');
    }
    function addremoveqoute(str, flag) {
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
    function computed(group) {
        $timeout(function () {

            CSS();
        }, 100);


        $timeout(function () {

            if (group.rules.length == 0 && $scope.isShow == 0) {
                $('.first').last().hide();
                $('.first1').last().hide();
                $('.first2').last().hide();
                $('.first3').last().hide();

                $('.removeborber').first().css("border", "0");



                $scope.isShow = 1;
            }
        }, 500);
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
    $scope.filter = JSON.parse(data);

    $scope.$watch('filter', function (newValue) {
        $(".branch-value").remove("ng-hide")
        $scope.json = JSON.stringify(newValue, null, 2);
        
        $scope.json.object = newValue;

        $scope.output = computed(newValue.group);

    }, true);





    //Query Builder End here


    $scope.openLeftMenu = function () {
        var tests = testData();
        //if (tests.NetworkModeId>0) {
        $scope.Test = tests;

        //$http({
        //    method: 'GET',
        //    url: '/test/GetAllTest'
        //}).then(function (data) {
        //    $scope.Test = data.data.listTest;
        //    console.log();
        //    alert();
        //}, function (error) {

        //});

        //}
    }


    $scope.findValue = function (enteredValue) {
        angular.forEach($scope.Range, function (value, key) {

            if (value.PlotType == enteredValue) {
                value.PlotType = value.PlotType.replace("_PLOT", '');
                $scope.results.push(value);

            }
        });
    };
    //var map = {};
    $(".panel-body").css("display", "none");
    $(".pull-right").addClass("panel-collapsed")
    $(".pull-right .glyphicon").removeClass("glyphicon-chevron-up")
    $(".pull-right .glyphicon").addClass("glyphicon-chevron-down")
    NgMap.getMap().then(function (map) {

        var myParser = new geoXML3.parser({ map: map });
        if ($scope.path !=null) {
            myParser.parse($scope.path);
        }
        
    });
    $scope.TestId;
    $scope.check = function (data) {
        var arr = [];
        for (var i in data) {
            if (data[i].selected == 'Y') {
                arr.push(data[i].Name);
            }
        }
        console.log(arr);
        // Do more stuffs here
    }
    $scope.DrawKml = function (data) {
        $scope.loader = true;
        //$scope.IsPci = true;
        //alert(a);
        for (var i in data) {

            data[0].Name
            var name = '';
            if (data[i].selected == 'YES') {
                name = data[i].Name;
                $scope.path = 'http://' + Location + '/files/' + name + '.kml';
                break;
            }
        }
        $scope.loader = false;
        
        NgMap.getMap().then(function (map) {
            var myParser = new geoXML3.parser({ map: map });

            myParser.parse($scope.path);




        });
    }

    //start

    $scope.Azmiuth = function (Angles, Radius) {
        
        //console.log(Angles);
        // 0 = start angle
        // 1 = end angle
        // 2 = color
        if (Radius == undefined) {
            Radius = 100;
        }
        polys = [],
        i = 0,
        radiusMeters = Radius;
        var latlng = null;
        for (; i < Angles.length ; i++) {

            //latlng = new google.maps.LatLng(28.54720, -81.719064);
            //var path = $scope.getArcPath(latlng, radiusMeters, 103, 137);

            Angles[i].StartAngle = Angles[i].Azimuth - 23;
            Angles[i].EndAngle = Angles[i].Azimuth + 23;
            latlng = new google.maps.LatLng(Angles[i].Latitude, Angles[i].Longitude);
            var path = $scope.getArcPath(latlng, radiusMeters, Angles[i].StartAngle, Angles[i].EndAngle);
            // console.log(path);

            path.unshift(latlng);
            path.push(latlng);
            var poly = null;
            poly = new google.maps.Polygon({
                path: path,
                map: $scope.map,
                fillColor: Angles[i].sectorColor,
                fillOpacity: 1,
            });
            polys.push(poly);
            
        }
    }


    

    $scope.getArcPath = function (center, radiusMeters, startAngle, endAngle, direction) {

        if (endAngle > 360) {
            endAngle = 360;
        }
        var point, previous,
            atEnd = false,
            points = Array(),
            a = startAngle;
        while (true) {
            point = google.maps.geometry.spherical.computeOffset(center, radiusMeters, a);
            points.push(point);
            if (a == endAngle) {
                break;
            }
            a++;
            if (a > 360) {
                a = 1;
            }
        }
        if (direction == 'counterclockwise') {
            points = points.reverse();
        }
        return points;
    }



    //end



    //query builder start
    $scope.inputs = [];
    $scope.Range = [];
    $scope.ranges = [];
    $scope.addInput = function () {
        //alert(a);
        $scope.results.unshift({ From: '', To: '' });
        //$scope.inputs.push({ field: '', value: '' });
    }

    $scope.removeInput = function (index) {
        $scope.results.splice(index, 1);
    }

    var data = '{"group": {"operator": "AND","rules": []}}';

    $scope.NetworkMode;
    $scope.Band;
    $scope.Carrier
    $scope.Test;
    $scope.color;
    $scope.Sector;
    $scope.Search = {};
    $scope.Pci = {
        done: true,
        pciColor: '#ffffff',
        PciId: 88
    };

    $scope.BandId;
    $scope.SelectedIds = {};

    $scope.getSectors = function () {        
        var d = Params();
        $scope.SiteCode = d.SiteCode;
        $scope.NetworkLayerId = d.NetworkLayerId;
        //if ($scope.Sector == undefined) {
            $http.get('/Optimizations/getSectors?siteId=' + Id + '&sitCode=' + null + '&networkModeId=' + d.NetworkModeId + '&BandId=' + d.BandId + '&CarrierId=' + d.CarrierId + '&scopeId=' + d.ScopeId).then(function (data) {
                $scope.Sector = data.data;
                $scope.siteLat = data.data[0].Latitude
                $scope.siteLong = data.data[0].Longitude
                $scope.Azmiuth(data.data, 200);

            }, function (error) {
                alert("error");
            });
            //$http({
            //    method: 'Get',
            //    url: '/Optimizations/getSectors',
            //    params: { siteId: Id, sitCode: "", networkModeId: d.NetworkModeId, BandId: d.BandId, CarrierId: d.CarrierId, scopeId: d.ScopeId }
            //}).then(function (data) {
            //    $scope.Sector = data.data;
            //    $scope.siteLat = data.data[0].Latitude
            //    $scope.siteLong = data.data[0].Longitude
            //    $scope.Azmiuth(data.data, 200);

            //}, function (error) {
            //    alert("error");
            //});
        //}
    }

    $scope.getRfRange = function () {
        var d = Params();
        if ($scope.Range.length<1) {


            $http({
                method: 'Get',
                url: '/Optimizations/getrfPlot',
                params: { siteId: Id, networkModeId: d.NetworkModeId }
            }).then(function (data) {
                $scope.Range = data.data;
                $scope.title = data.data[0].PlotType;
                $scope.findValue(data.data[0].PlotType);
            }, function (error) {
                alert()
            });
        }
    }
    //$scope.getRfRange();
    $scope.changecolor = {
        NewVlue: '',
        OldValue: '',
        Id: ''
    }
    $scope.getPci = function () {
        $http.get('/Optimizations/getPci?siteId='+Id).then(function (data) {
            $scope.Pci = data.data
            var aa = 0;
            for (var ii = 0; ii < $scope.Pci.length; ii++) {

                $scope.$watch('Pci[' + ii + ']', function (newvalue, oldvalue) {
                    $scope.changecolor.NewVlue = newvalue.pciColor;
                    $scope.changecolor.OldValue = oldvalue.pciColor;
                    $scope.changecolor.Id = newvalue.PciId;
                    
                    if (oldvalue.pciColor != newvalue.pciColor) {

                        $http.post("changeColor", $scope.changecolor);
                    }

                }, true);
            }
        }, function (error) {
            alert("error")
        });
    }
    $scope.getPci();
    $scope.Sectorarray = [];
    $scope.SiteSector= function(data)
    {
        $scope.Sectorarray = [];
        data[0].SiteId = Id;
        data[0].NetworkLayerId = $scope.NetworkLayerId;
        
        for (var i = 0; i < data.length; i++) {
            if (data[i].selected == 'YES') {

                $scope.Sectorarray.push(data[i])
            }
            
        }
        $http.post("/Optimizations/SiteSector", { 'pci': $scope.Pciarray, 'sector': $scope.Sectorarray }).then(function (data) {
                    
                    console.log(data)
                }, function (error) {

                });
        
        
    }
    $scope.Pciarray=[]
    $scope.SelectedPci = function (data) {
        data[0].SiteId = Id;
        data[0].NetworkLayerId = $scope.NetworkLayerId;
        $scope.Pciarray = []
        for (var i = 0; i < data.length; i++) {
            if (data[i].selected == 'YES') {
                $scope.Pciarray.push(data[i])
            }
            
        }

        $http.post("/Optimizations/CollectedPCI", { 'pci':$scope.Pciarray,'sec':$scope.Sectorarray}).then(function (data) {
            
            console.log(data)
        }, function (error) {

        });
       

    }
    $scope.RFLegends = function (data) {

        $http.post("/Optimizations/RFLegends", data).then(function (data) {
            //$scope.Test = data.data.listTest;
            console.log(data)
        }, function (error) {

        });
        

    }

    $scope.count = 0;
    $scope.flag;
    $scope.Bodyfunction = function (value) {

        if ($scope.flag != value) {
            $scope.count = 0
        }
        $scope.flag = value
        $scope.isOpen = false;
        $scope.isOpen1 = false;
        $scope.isOpen2 = false;
        $scope.isOpen3 = false;
        if (value == 'isOpen') {
            if ($scope.count > 0) {
                $scope.isOpen = false;
                $scope.count = 0
            }
            else {
                $scope.isOpen = true;
                $scope.count = 1;
            }


        }
        else if (value == 'isOpen1') {
            if ($scope.count > 0) {
                $scope.isOpen1 = false;
                $scope.count = 0
            }
            else {
                $scope.isOpen1 = true;
                $scope.count = 1;
            }
        }
        else if (value == 'isOpen2') {
            if ($scope.count > 0) {
                $scope.isOpen2 = false;
                $scope.count = 0
            }
            else {
                $scope.isOpen2 = true;
                $scope.count = 1;
            }
        }
        else if (value == 'isOpen3') {
            if ($scope.count > 0) {
                $scope.isOpen3 = false;
                $scope.count = 0
            }
            else {
                $scope.isOpen3 = true;
                $scope.count = 1;
            }
        }
    }
    //$scope.getSectors();

    $scope.geofences = {}
    //file Upload Code
    var formdata = new FormData();
    $scope.getTheFiles = function ($files) {
        angular.forEach($files, function (value, key) {
            formdata.append(key, value);
        });
    };


    $scope.uploadFiles = function () {
        $scope.loader = true;
        $scope.isOpen = false;
        $http({
            method: 'POST',
            url: '/swi/Test/fileupload',
            data: formdata,
            headers: {
                'Content-Type': undefined
            }
        })
    .then(function (response) {
        $scope.geofences = response.data.list;
        $scope.Azmiuth(response.data.list1);
        $scope.loader = false;
    },
    function (response) { // optional
        // failed
    });
        //var request = {
        //    method: 'POST',
        //    url: 'fileupload/',
        //    data: formdata,
        //    headers: {
        //        'Content-Type': undefined
        //    }
        //};

        //// SEND THE FILES.
        //$http(request)
        //    .success(function (d) {
        //        $scope.geofences = d.data;
        //        alert(d);
        //    })
        //    .error(function () {
        //    });
    }
    $scope.Test = function(a)
    { alert(a);}
    //file Upload End
});
app.directive('uiColorpicker', function () {
    return {
        restrict: 'E',
        require: 'ngModel',
        scope: false,
        replace: true,
        template: "<span><input class='input-small' /></span>",
        link: function (scope, element, attrs, ngModel) {
            var input = element.find('input');
            var options = angular.extend({
                color: ngModel.$viewValue,
                change: function (color) {
                    scope.$apply(function () {
                        ngModel.$setViewValue(color.toHexString());
                    });
                }
            }, scope.$eval(attrs.options));

            ngModel.$render = function () {
                input.spectrum('set', ngModel.$viewValue || '');
            };

            input.spectrum(options);
        }
    };
});
app.directive('ngFiles', ['$parse', function ($parse) {

    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, { $files: event.target.files });
        });
    };

    return {
        link: fn_link
    }
}]);

app.filter('groupBy', function () {
    return _.memoize(function (items, field) {
        //console.log(items)

        return _.groupBy(items, field);
    }
    );
});








app.controller('survey', function ($scope) {

    $scope.ResultString1;
    $scope.ResultString2;
    $scope.ResultString3;
    $scope.ResultString4;
    $scope.ResultString5;
    $scope.ResultString6;

    $scope.QueryString = [{ selected_location: null, selected_stage: null }];

    $scope.ishide1 = false;
    $scope.ishide2 = false;
    $scope.ishide3 = false;
    $scope.ishide4 = false;
    $scope.ishide5 = false;
    $scope.ishide6 = false;
    $scope.ishide7 = false;
    $scope.w = [{ A: null, B: null, C: null, D: null }];
    $scope.w1 = [{ A: null, B: null, C: null, D: null }];
    $scope.w2 = [{ A: null, B: null, C: null, D: null }];
    $scope.w3 = [{ A: null, B: null, C: null, D: null }];
    $scope.w4 = [{ A: null, B: null, C: null, D: null }];
    $scope.w5 = [{ A: null, B: null, C: null, D: null }];
    $scope.w6 = [{ A: null, B: null, C: null, D: null }];
    $scope.w7 = [{ A: null, B: null, C: null, D: null }];

    $scope.List = []
    $scope.createStringByArray = function (array) {
        var output = '(';
        angular.forEach(array, function (object) {
            angular.forEach(object, function (value, key) {
                //output += key + ',';
                if (value != null && key != '$$hashKey') {
                    output += value + ' ';
                }
                console.log("dsfsad")
            });

        });
        output = output + ')';
        return output;
    }


    $scope.buildString = function (f) {
        $scope.esadfsad = "fsadf";
        if (f == 1) {
            $scope.QueryString.push($scope.w);
            $scope.ResultString1 = $scope.createStringByArray($scope.w);
        }
        else if (f == 2) {


            $scope.ResultString2 = $scope.createStringByArray($scope.w1);
        }
        else if (f == 3) {


            $scope.ResultString3 = $scope.createStringByArray($scope.w2);
        }
        else if (f == 4) {
            $scope.QueryString.push($scope.w);
            $scope.ResultString4 = $scope.createStringByArray($scope.w3);
        }
        else if (f == 5) {


            $scope.ResultString5 = $scope.createStringByArray($scope.w4);
        }
        else if (f == 6) {


            $scope.ResultString6 = $scope.createStringByArray($scope.w5);
        }
    }

    $scope.add = function (d) {
        if (d == 'f') {
            $scope.w.push({ selected_location: null, selected_stage: null });
            //$scope.QueryString.push($scope.w);
        }
        else if (d == 's') {
            $scope.w1.push({ selected_location: null, selected_stage: null });
        }
        else if (d == 't') {
            $scope.w2.push({ selected_location: null, selected_stage: null });
        }
        else if (d == 'fr') {
            $scope.w3.push({ selected_location: null, selected_stage: null });
        }
        else if (d == 'fi') {
            $scope.w4.push({ selected_location: null, selected_stage: null });
        }

        else if (d == 'si') {
            $scope.w5.push({ selected_location: null, selected_stage: null });
        }
        else if (d == 'se') {
            $scope.w6.push({ selected_location: null, selected_stage: null });
        }
        else if (d == 'ei') {
            $scope.w7.push({ selected_location: null, selected_stage: null });
        }
    };
    var count = 0;


    $scope.remove = function (ww, $event, d) {
        //$event.preventDefault();

        if (d == 'f') {
            var index = $scope.w.indexOf(ww);
            if (index >= 0) $scope.w.splice(index, 1);
            $scope.ResultString1 = $scope.createStringByArray($scope.w);

        }
        else if (d == 's') {
            var index = $scope.w1.indexOf(ww);
            if (index >= 0) $scope.w1.splice(index, 1);
            $scope.ResultString2 = $scope.createStringByArray($scope.w1);
        }
        else if (d == 't') {
            var index = $scope.w2.indexOf(ww);
            if (index >= 0) $scope.w2.splice(index, 1);
            $scope.ResultString3 = $scope.createStringByArray($scope.w2);
        }
        else if (d == 'fr') {
            var index = $scope.w3.indexOf(ww);
            if (index >= 0) $scope.w3.splice(index, 1);
            $scope.ResultString4 = $scope.createStringByArray($scope.w3);
        }
        else if (d == 'fi') {
            var index = $scope.w4.indexOf(ww);
            if (index >= 0) $scope.w4.splice(index, 1);
            $scope.ResultString5 = $scope.createStringByArray($scope.w4);
        }

        else if (d == 'si') {
            var index = $scope.w5.indexOf(ww);
            if (index >= 0) $scope.w5.splice(index, 1);
            $scope.ResultString6 = $scope.createStringByArray($scope.w5);
        }
        else if (d == 'se') {
            var index = $scope.w6.indexOf(ww);
            if (index >= 0) $scope.w6.splice(index, 1);
            $scope.ResultString7 = $scope.createStringByArray($scope.w6);
        }
        else if (d == 'ei') {
            var index = $scope.w7.indexOf(ww);
            if (index >= 0) $scope.w7.splice(index, 1);
            $scope.ResultString8 = $scope.createStringByArray($scope.w7);
        }

    };
    $scope.flag = 0;
    $scope.AddNew = function () {

        if ($scope.flag == 0) {
            $scope.ishide1 = true;
        }
        else if ($scope.flag == 1) {
            $scope.ishide2 = true;
        }
        else if ($scope.flag == 2) {
            $scope.ishide3 = true;
        }
        else if ($scope.flag == 3) {
            $scope.ishide4 = true;
        }


        else if ($scope.flag == 4) {
            $scope.ishide5 = true;
        }
        else if ($scope.flag == 5) {
            $scope.ishide6 = true;
        }
        else if ($scope.flag == 6) {
            $scope.ishide7 = true;
        }
        //for (var i = 0; i < $scope.w.w.length; i++) {
        //$scope.List=[]
        //console.log($scope.w[0]);
        //$scope.List.push($scope.w);
        ////    break;
        ////}

        ////$scope.w =[]
        //console.log($scope.List);

        count = count + 1;
        $scope.flag = count;

    }
    $scope.ishide = true;
    $scope.removeGroup = function (data) {
        if (data == 1) {
            $scope.ResultString1 = null;
            $scope.ishide = false;
        }
        else if (data == 2) {
            $scope.ResultString1 = null;
            $scope.ishide1 = false;
        }
        else if (data == 3) {
            $scope.ResultString1 = null;
            $scope.ishide2 = false;
        }


        else if (data == 4) {
            $scope.ResultString1 = null;
            $scope.ishide3 = false;
        }
        else if (data == 5) {
            $scope.ResultString1 = null;
            $scope.ishide4 = false;
        }
        else if (data == 6) {
            $scope.ResultString1 = null;
            $scope.ishide5 = false;
        }
        else if (data == 7) {
            $scope.ResultString1 = null;
            $scope.ishide6 = false;
        }
        else if (data == 8) {
            $scope.ResultString1 = null;
            $scope.ishide7 = false;

        }

    }

    $scope.Value = [
        'First Name',
       'Last Name',
    ];

    $scope.Operators = ['!=', '<', '>', ]



    $scope.values;

    $scope.Operation = ['AND', 'OR']
});