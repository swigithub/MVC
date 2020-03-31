/* == Randow JSON ==*/
$QueryBuilderRows = [{
    "code": `<div class="tg-row">
        <span class="small moverow"><i class ="fa fa-arrows-alt"></i></span>
        <span class ="small"><input class ="selectRow" type="checkbox" /><input type="hidden" class ="db-column-data-type" /></span>
        <span class ="custom-width"><input class ="tbl-columnname" value="" type="text"  readonly /></span>
        <span><input class ="tbl-aliasname" value="" type="text" /></span>
        <span> <select class="tbl-function"></select> </span>
        <span class ="small"><input class ="tbl-group" disabled="true" type="checkbox" /></span>
        <span> <select class ="tbl-sort" onChange="sortChangeFun(this)"></select> </span>
        <span class ="small"><a class ="clone" href="javascript:void(0);" title="Clone"><i class ="fa fa-clone"></i></a></span>
  <span class ="small sub-query-icon"><a class ="code" ng-click="vm.openAndRenderSubQuery(chngidx,uniqueKey)" href="javascript:void(0);" title="Open Subquery"><i class ="fas fa-code"></i></a>
  <a class ="trash" ng-click="vm.deleteSubquery($event,chngidx,uniqueKey)" href="javascript:void(0);" title="Delete Subquery"><i class ="fa fa-trash"></i></a></span>
        </div>`
}
];

var SelectedColumnArr = [];
var numbercount = 0;
var aggreagtefunctionCount = 0;

//function LoadViews(Querybuilder) {
//    $.ajax({
//        type: "POST",
//        url: '/Xcelerate/QueryBuilder/GetViews',

//        success: function (result) {
//            console.log(result);
//            $('.' + Querybuilder).find('.querybuilder-datasets').empty();
//            var appenddrpdwnlst = "<option value='0'>Select Dataset</option>";
//            for (var i = 0; i < result.obj.length; i++) {

//                appenddrpdwnlst += "<option value = '" + result.obj[i].ViewName + "'>" + result.obj[i].ViewName + "</option>";
//            }
//            $('.' + Querybuilder).find('.querybuilder-datasets').append(appenddrpdwnlst);
//        }
//    });
//}


//function ChangeDataSetViews(Querybuilder) {
//    debugger
//    $.ajax({
//        type: "POST", 
//        url: '/Xcelerate/QueryBuilder/GetViewColumns',
//        data: { viewname: $('.' + Querybuilder).find('.querybuilder-datasets').val() },
//        success: function (result) {
//            console.log(result);
//            $('.' + Querybuilder).find('.columns-list').empty();
//            var appendcolumnslst = "";
//            for (var i = 0; i < result.obj.length; i++) {
//                appendcolumnslst += '<li><input type="checkbox" id="' + result.obj[i].ColumnName + '" name="' + result.obj[i].ColumnName + '" data-columndatatype="' + result.obj[i].ColumnDataType + '" data-columndefinationtype="' + result.obj[i].ColumnDefinationType + '" onchange="LoadSelectedColumn(this)"><label for="' + result.obj[i].ColumnName + '">' + result.obj[i].ColumnName + '</label></li>';
//            }
//            $('.' + Querybuilder).find('.columns-list').append(appendcolumnslst);
//        }
//    });
//}

//function LoadSelectedColumn(e) {
//    if (e.checked) {
//        var uniqueId = $(e).attr('id');
//        if ($(e).parents('div[querybuilder]').find(".selected-rows .tg-row#" + uniqueId).length < 1) {
//            var newRow = $.parseHTML($QueryBuilderRows[0]['code']);
//            numbercount = numbercount + 1;
//            $(newRow).attr("id", uniqueId);
//            $(newRow).attr("rowid", numbercount);
//            var viewname = $(e).siblings('label').text();
//            var viewcolumndatatype = $(e).attr('data-columndatatype');
//            LoadAggregateFunctions(viewcolumndatatype, $(newRow));
//            LoadSortFunctions($(newRow));
//            $(newRow).find('.tbl-columnname').attr('value', viewname);


//            $selectedCols.add(newRow);
//            if (aggreagtefunctionCount > 0) {
//                $(newRow).find(".tbl-group").prop("checked", true);
//                $(newRow).find(".tbl-group").attr("disabled", true);
//            }
//        }
//    } else {
//        var uniqueId = $(e).attr('id');
//        var findRow = $(".selected-rows .tg-row#" + uniqueId);
//        let arr = [];
//        $.each(findRow, function (idx, ele) {
//            arr.push(ele);
//        });
//        $selectedCols.remove(arr, { removeElements: true });
//    }
//}

function LoadAggregateFunctions(viewcolumndatatype, row) {

    row.find('.tbl-function').html('');
    var FunctionsList = "";
    if (viewcolumndatatype == "numeric" || viewcolumndatatype == "float" || viewcolumndatatype == "real" || viewcolumndatatype == "int") {
        FunctionsList = `<option value="-1" >--Select--</option>
            <option value="COUNT">COUNT</option>
                                    <option value="SUM">SUM</option>
 <option value="MIN">MIN</option>
                                    <option value="MAX">MAX</option>
                                   
                                    <option value="AVG">AVERAGE</option>`;


    }
    else if (viewcolumndatatype == "date" || viewcolumndatatype == "datetime") {
        FunctionsList = `<option value="-1" >--Select--</option>
                                    <option value="year">YEAR</option>
<option value="month">MONTH</option>
<option value="week">WEEK</option>
<option value="day">DAY</option>`;
    }
    else {
        FunctionsList = `<option value="-1" >--Select--</option>
                                    <option value="COUNT">COUNT</option>`;

    }
    row.find('.tbl-function').html(FunctionsList);
}

function LoadSortFunctions(row) {
    row.find('.tbl-sort').html('');
    var FunctionsList = `<option value="-1">--Select--</option>
                                    <option value="ASC">ASC</option>
                                    <option value="DESC">DESC</option>`;
    row.find('.tbl-sort').html(FunctionsList);


}

function DeleteQBSelectedRows() {
    let arr = [];
    var thisRow = $(".selected-rows .tg-row .selectRow:checked").parents(".tg-row");
    thisRow.each(function (idx, ele) {
        arr.push(ele);
        var rowid = $(ele).attr('rowid');
        var getId = $(ele).attr('id');
        var node = $(ele).context;
        var tblfunctionnode = $(node).find('.tbl-function');
        if (aggreagtefunctionCount == 1) {
            $(".tbl-group").prop("checked", false);
            $(".tbl-group").attr("disabled", true);
        }
        if (tblfunctionnode.val() != '-1') {
            aggreagtefunctionCount = aggreagtefunctionCount - 1;
            if (aggreagtefunctionCount == 0) {
                $(".tbl-group").prop("checked", false);
                $(".tbl-group").attr("disabled", false);
            }
        }

        $(".side-panel .select-all-row, .columns-list li #" + getId).attr('checked', false);



    });
    $selectedCols.remove(arr, { removeElements: true });


    return false;
}

function DeleteQBAllRows() {
    var findRow = $(".selected-rows .tg-row");
    let arr = [];
    $.each(findRow, function (idx, ele) {
        arr.push(ele);
        var getId = $(ele).attr('id');
    });
    $(".columns-list li input[type='checkbox'], .side-panel .select-all-row").attr('checked', false);

    $selectedCols.remove(arr, { removeElements: true });
    aggreagtefunctionCount = 0;
    return false;
}

function SelectQBAllRows(e) {

    if ($(e).is(':checked')) {
        $(".selected-rows .tg-row .selectRow").prop('checked', true);
    } else {
        $(".selected-rows .tg-row .selectRow").prop('checked', false);
    }
}

//function ChangeAggregateFunction(e) {

//    var that = e;
//    if (e.value != '-1') {
//        //$(this).find("option:selected").each(function () {
//        //    console.log(this)
//        //    $(this).removeAttr('selected');
//        //});
     
//        $(e).find('option[value=' + $(e).val() + ']').attr("selected", "selected");
//        if (aggreagtefunctionCount == 0) {
//            $(".tbl-group").prop("checked", true);
//            $(".tbl-group").attr("checked", true);
//            $(".tbl-group").attr("disabled", true);
//        }
//        aggreagtefunctionCount = aggreagtefunctionCount + 1;
//        var nodesort = $(event.target).closest('.tg-row').find('.tbl-sort');
//        var node = $(event.target).closest('.tg-row').find('.tbl-group');

//        node.prop('checked', false);
//        node.attr('checked');
//        node.attr("disabled", true);
//        nodesort.attr("disabled", true);
//        nodesort.val('-1');
//    }
//    else {
//        $(e).find('option').removeAttr("selected");
//        aggreagtefunctionCount = aggreagtefunctionCount - 1;
//        if (aggreagtefunctionCount == 0) {
//            $(".tbl-group").prop("checked", false);
//            $(".tbl-group").attr("disabled", true);
//            $(".tbl-group").attr("checked", false);
//            $(".tbl-sort").attr("disabled", false);
//            $(".tbl-sort").val('-1');
//        }
//        else {
//            var node = $(event.target).closest('.tg-row').find('.tbl-group');
//            var nodesort = $(event.target).closest('.tg-row').find('.tbl-sort');
//            node.prop('checked', true);
//            node.attr('checked', true);
//            node.attr("disabled", true);
//            nodesort.attr("disabled", false);
//            nodesort.val('-1');
//        }
//    }

//}


function CreateQuery() {
    var columnarr = [];
    var querycolumns = ''
    var querygroupby = '';
    var querysortyby = '';

    $(".tg-row").each(function () {
        var columnAliasname = '';
        var columnName = $(this).find(".tbl-columnname").val();

        var aliasName = $(this).find(".tbl-aliasname").val();

        if (aliasName == "") {
            columnAliasname = columnName;
        }
        else {
            columnAliasname = aliasName;
        }
        var aggregateFunction = $(this).find(".tbl-function").val();
        var groupBy = $(this).find(".tbl-group").prop("checked");

        var sortBy = $(this).find(".tbl-sort").val();
        columnarr.push({
            columnname: columnName,
            aliasname: aliasName,
            columnaliasname: columnAliasname,
            aggregatefunction: aggregateFunction,
            groupby: groupBy,
            sortby: sortBy

        });

    });

    for (var i = 0; i < columnarr.length; i++) {
        if (columnarr[i]["aggregatefunction"] != "-1") {
            querycolumns += ' ' + columnarr[i]["aggregatefunction"] + '([' + columnarr[i]["columnname"] + ']) AS [' + columnarr[i]["columnaliasname"] + '],';
        }
        else {
            querycolumns += ' ([' + columnarr[i]["columnname"] + ']) AS [' + columnarr[i]["columnaliasname"] + '],';
        }
        if (columnarr[i]["groupby"] == true) {
            querygroupby += ' [' + columnarr[i]["columnname"] + '],';
        }

        if (columnarr[i]["sortby"] != '-1') {
            querysortyby += ' [' + columnarr[i]["columnname"] + '] ' + columnarr[i]["sortby"] + ',';
        }

    }
    var dataset = $('.querybuilder-datasets').val();
    querycolumns = querycolumns.substring(0, querycolumns.length - 1);
    var finalquery = 'Select' + querycolumns + ' from ' + dataset;
    if (querygroupby != '') {
        querygroupby = querygroupby.substring(0, querygroupby.length - 1);
        finalquery += ' GROUP BY ' + querygroupby;
    }
    if (querysortyby != '') {
        querysortyby = querysortyby.substring(0, querysortyby.length - 1);
        finalquery += ' ORDER BY ' + querysortyby;
    }


    


    $.ajax({
        type: "POST",
        url: '/Xcelerate/QueryBuilder/GetQueryResult',
        data: { query: finalquery },
        success: function (result) {
            var d = JSON.parse(result);
        //    console.log(d);
        }
    });


}


