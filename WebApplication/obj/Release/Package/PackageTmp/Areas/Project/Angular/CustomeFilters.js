
//-------------01---------------------
app.filter("dateFilter", function ($filter) {
    return function (item) {
        if (item != null) {
            var parsedDate = new Date(parseInt(item.substr(6)));
            return $filter('date')(parsedDate, 'yyyy-MM-dd');
        }
        return "";
    };
});

//------------02----------------------
app.filter('getProjectById', function () {
    return function (input, id) {
        var i = 0, len = input.length;
        for (; i < len; i++) {
            if (input[i].ProjectName == id) {
                return input[i];
            }
        }
        return null;
    }
});
//------------03----------------------
app.filter('getProjectByCompany', function () {
    return function (input, name) {
        var i = 0, len = input.length;
        var array = [];
        for (; i < len; i++) {
            if (input[i].Company == name) {
                array.push(input[i]);
            }
        }
        return array;
    }
});
//------------04----------------------
app.filter('getProjectByVendor', function () {
    return function (input, name) {
        var i = 0, len = input.length;
        var array = [];
        for (; i < len; i++) {
            if (input[i].Vendor == name) {
                array.push(input[i]);
            }
        }
        return array;
    }
});
//------------05----------------------
app.filter('getProjectByStatus', function () {
    return function (input, id) {
        var i = 0, len = input.length;
        for (; i < len; i++) {
            if (input[i].Status == id) {
                return input[i];
            }
        }
        return null;
    }
});


//----------------------------------





//------------06----------------------
//app.filter("myfilter", function () {
//    return function (items, from, to) {
//        var df = parseDate(from);
//        var dt = parseDate(to);
//        var result = [];
//        for (var i = 0; i < items.length; i++) {
//            var tf = new Date(items[i].date1 * 1000),
//                tt = new Date(items[i].date2 * 1000);
//            if (tf > df && tt < dt) {
//                result.push(items[i]);
//            }
//        }
//        return result;
//    };
//});

////-----------------
//function parseDate(input) {
//    var parts = input.split('-');
//    return new Date(parts[2], parts[1] - 1, parts[0]);
//}