

setTimeout(function () {
    //SiteGridPagination();
    //IssueGridPagination();
}, 4000);
function toDate(selector) {

    var from = selector.split("/");
    return new Date(from[2], from[1] - 1, from[0]);
}
function SiteGridPagination(TotalRecord) {
    var count = $('#TotalSites').text();
    var ProjectID = $('#ProjectID').text();
    if (TotalRecord != undefined && TotalRecord > 0) {
        count = TotalRecord;
    }

    var pag = parseFloat(count) / parseFloat(5);

    //ceil
    // var TotalPages = Math.round(pag);
    var TotalPages = Math.ceil(pag);
    $('#SiteGridPagination').pagination({
        pages: TotalPages,
        cssStyle: 'compact-theme',
        currentPage: 1,
        hrefTextPrefix: '#',
        displayedPages: 7,
        onPageClick: function (page) {
            if (page !=null && page != undefined) {
                SitepageNum = page;
            }
            
            var search = 0//$('#searchbox').val();
            if (search.length > 1) {
                
                  $.ajax({
                //url: '/Project/Dashboard/GetMilstonesBarchart?ProjectId=' + $("#pId").attr("data-ProjectId") + '&filter=Get_Project_Timeline_Variance',
                      url: '/Project/Dashboard/TableResult?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + $.cookie(CookiesIdentifier + 'ProjectTasks')
     + '&LocationIds=' + $.cookie(CookiesIdentifier + "SiteProjectMarkets") + '&FromDate=' + $.cookie(CookiesIdentifier + "SiteFromDate") + '&ToDate=' + $.cookie(CookiesIdentifier + "SiteToDate"),
                type: 'Get',
                async: false,
                success: function (data) {
                    debugger

                    $('#divclientsites').html(data);
                }
            });

                //$.ajax({
                //    url: '/Project/Dashboard/TableResult',
                //    type: 'post',
                //    data: {
                //        'value': search, 'Page': SitepageNum, 'TaskIds': $.cookie('ProjectTasks')
                //        , 'LocationIds': $.cookie("ProjectMarkets"), 'FromDate': $.cookie("FromDate"), 'ToDate': $.cookie("ToDate"), 'Searchoption':search
                //    },
                //    success: function (res) {
                        
                //        $('#divclientsites').empty();
                //        $('#divclientsites').html(res);
                //        fnWoStatus();
                //        var showing = parseInt(SitepageNum) * 5;
                //        $('#GridShowing').text(showing);

                //    }
                //});
            } else {
                $("body").append(`<div id="ajaxLoading" style="position: fixed; top: 0px; bottom: 0px; left: 0px; right: 0px; margin: auto; padding: 8px; text-align: center; vertical-align: middle; width: 140px; height: 95px; z-index: 2000; border-radius: 4px; display: block;"><img src="/Content/Images/Application/Logo_Loading.gif" style="margin-bottom:8px;width:100px;height:100px"><p style="margin:0;font-size:15px;color:#fff;background:#88887B">Please Wait...</p></div>`);
                try {
                    var fData = toDate($.cookie(CookiesIdentifier + "SiteFromDate"))
                    var tdate = toDate($.cookie(CookiesIdentifier + "SiteToDate"))
                    $.ajax({
                        url: '/Project/Dashboard/TableResult',
                        type: 'post',
                        data: {
                            ProjectId: ProjectID, 'Page': page, 'TaskIds': $.cookie(CookiesIdentifier + 'ProjectTasks')
                            , 'LocationIds': $.cookie(CookiesIdentifier + "SiteProjectMarkets"), 'FromDate': $.cookie(CookiesIdentifier + "SiteFromDate"), 'ToDate': $.cookie(CookiesIdentifier + "SiteToDate"), 'Searchoption': UserSerachValue
                        },
                        success: function (res) {
                            $('#divclientsites').empty();
                            $('#divclientsites').html(res);
                            //fnWoStatus();

                            var showing = parseInt(page) * 5;
                            $('#GridShowing').text(showing);
                            $(".sitesearchoption").val(UserSerachValue);
                            $("#ajaxLoading").remove();
                        },
                        error: function(r){
                            $("#ajaxLoading").remove();
                       }
                    });
                }
                catch (e) {
                    $("#ajaxLoading").remove();
                }
                
            }

        },

    });
    $('#GridShowing').text($('#example1 tr').length - 1);

    if (count > 5) {
        $('#GridShowing').text("5");
        $('#GridShowingTotal').text(count);
    }
    else {
        $('#GridShowing').text(count);
        $('#GridShowingTotal').text(count);
    }
    
}


$('#searchbox').keyup(function (e) {
    var ProjectID1 = $('#ProjectID').text();
    if (e.keyCode == 13) {

        $.ajax({
            //url: '/Project/Dashboard/TableResult',
            url: '/Project/Dashboard/TableResult?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=' + SitepageNum + '&TaskIds=' + $.cookie(CookiesIdentifier + 'ProjectTasks')
     + '&LocationIds=' + $.cookie(CookiesIdentifier + "SiteProjectMarkets") + '&FromDate=' + $.cookie(CookiesIdentifier + "SiteFromDate") + '&ToDate=' + $.cookie(CookiesIdentifier + "SiteToDate") + '&Searchoption=' + $(this).val(),
            type: 'Get',
            //data: {  ProjectId: ProjectID1, 'Page': 1 },
            success: function (res) {
                if (res == '' || res == null) {
                    location.reload();
                }
                $('#divclientsites').empty();
                $('#divclientsites').html(res);
                //fnWoStatus();

                var showing = parseInt(SitepageNum) * 5;
                $('#GridShowing').text(showing);
                $('.table-responsive').css({
                    'min-height': '280px'
                });
                
                SiteGridPagination($('#SitesCount').val());
            }
        });

        return false;

    }
    if ($(this).val() == "") {
        $.ajax({
            //url: '/Project/Dashboard/GetMilstonesBarchart?ProjectId=' + $("#pId").attr("data-ProjectId") + '&filter=Get_Project_Timeline_Variance',
            url: '/Project/Dashboard/TableResult?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + $.cookie(CookiesIdentifier + 'ProjectTasks')
     + '&LocationIds=' + $.cookie(CookiesIdentifier + "SiteProjectMarkets") + '&FromDate=' + $.cookie(CookiesIdentifier + "SiteFromDate") + '&ToDate=' + $.cookie(CookiesIdentifier + "SiteToDate"),

            type: 'Get',
            async: false,
            success: function (data) {


                $('#divclientsites').html(data);
                SiteGridPagination();
            }
        });

    }


});

$('#searchboxIssue').keyup(function (e) {
    var IssuesStatus = $("#example-getting-started").val();
    IssuesStatus = IssuesStatus.join(',');
    var ProjectID1 = $('#ProjectID').text();
    if (e.keyCode == 13) {

        $.ajax({
            url: '/Project/Dashboard/GetProjectIssue?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + IssuesStatus
        + '&LocationIds=' + $.cookie(CookiesIdentifier + "PMIssuesProjectMarkets") + '&FromDate=' + $.cookie(CookiesIdentifier + "PMIssuesFromDate") + '&ToDate=' + $.cookie(CookiesIdentifier + "PMIssuesToDate") + '&Searchoption=' + $(this).val(),
            type: 'Get',
            //data: { 'Searchoption': $(this).val(), ProjectId: ProjectID1, 'Page': 1 },
            success: function (res) {
               if (res == '' || res == null) {
                    location.reload();
                }
                $('#divissues').empty();
                $('#divissues').html(res);
                //fnWoStatus();

                //var showing = parseInt(page) * 5;
              //  $('#IssueGridShowing').text(showing);
                //$('.table-responsive').css({
                //    'min-height': '280px'
                //});

                IssueGridPagination($('#SitesCount').val());
            }
        });

        return false;

    }
    if ($(this).val() == "") {
        var IssuesStatus = $("#example-getting-started").val();
        IssuesStatus = IssuesStatus.join(',');
        $.ajax({
            //url: '/Project/Dashboard/GetMilstonesBarchart?ProjectId=' + $("#pId").attr("data-ProjectId") + '&filter=Get_Project_Timeline_Variance',
            url: '/Project/Dashboard/GetProjectIssue?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + IssuesStatus
       + '&LocationIds=' + $.cookie(CookiesIdentifier + "PMIssuesProjectMarkets") + '&FromDate=' + $.cookie(CookiesIdentifier + "PMIssuesFromDate") + '&ToDate=' + $.cookie(CookiesIdentifier + "PMIssuesToDate") + '&Searchoption=' + $(this).val(),
            type: 'Get',
            async: false,
            success: function (data) {


                $('#divissues').html(data);
                IssueGridPagination();
            }
        });

    }

});


//IssueGridPagination();

function SiteGridPaginationSideGrid(TotalRecord) {

    var count = $('#TotalSites').text();
    var ProjectID = $('#ProjectID').text();
    if (TotalRecord != undefined && TotalRecord > 0) {
        count = TotalRecord;
    }

    var pag = parseFloat(count) / parseFloat(5);

    //ceil
    // var TotalPages = Math.round(pag);
    var TotalPages = Math.ceil(pag);
    $('#SiteGridPagination').pagination({
        pages: TotalPages,
        cssStyle: 'compact-theme',
        currentPage: 1,
        hrefTextPrefix: '#',
        displayedPages: 7,
        onPageClick: function (page) {
            if (page != null && page != undefined) {
                SitepageNum = page;
            }

            var search = 0//$('#searchbox').val();
            if (search.length > 1) {

                $.ajax({
                    url: '/Project/Dashboard/TableResult?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=1' + '&TaskIds=' + $.cookie(CookiesIdentifier + 'ProjectTasks')
   + '&LocationIds=' + $.cookie(CookiesIdentifier + "SiteProjectMarkets") + '&FromDate=' + $.cookie(CookiesIdentifier + "SiteFromDate") + '&ToDate=' + $.cookie(CookiesIdentifier + "SiteToDate"),
                    type: 'Get',
                    async: false,
                    success: function (data) {
                        debugger

                        $('#divclientsites').html(data);
                    }
                });
            } else {

                var fData = toDate($.cookie(CookiesIdentifier + "SiteFromDate"))
                var tdate = toDate($.cookie(CookiesIdentifier + "SiteToDate"))
                $.ajax({
                    url: '/Project/Dashboard/TableResult',
                    type: 'post',
                    data: {
                        ProjectId: ProjectID, 'Page': page, 'TaskIds': $.cookie(CookiesIdentifier + 'ProjectTasks')
                        , 'LocationIds': $.cookie(CookiesIdentifier + "SiteProjectMarkets"), 'FromDate': $.cookie(CookiesIdentifier + "SiteFromDate"), 'ToDate': $.cookie(CookiesIdentifier + "SiteToDate")
                    },
                    success: function (res) {
                        $('#divclientsites').empty();
                        $('#divclientsites').html(res);
                        //fnWoStatus();

                        var showing = parseInt(page) * 5;
                        $('#GridShowing').text(showing);

                    }
                });
            }

        },

    });
    $('#GridShowing').text($('#example1 tr').length - 1);

    if (count > 5) {
        $('#GridShowing').text("5");
        $('#GridShowingTotal').text(count);
    }
    else {
        $('#GridShowing').text(count);
        $('#GridShowingTotal').text(count);
    }
}

function IssueGridPagination() {
    
    var count = $('#issuecount').text();
    var ProjectID = $('#ProjectID').text();
    //if (TotalRecord != undefined && TotalRecord > 0) {
    //    count = TotalRecord;
    //}


    //var count = $('#TotalSites').text();
    var pag = parseFloat(count) / parseFloat(5);
    if (count > 5) {
        $('#IssueGridShowing').text("5");
        $('#IssueGridShowingTotal').text(count);
    }
    else {
        $('#IssueGridShowing').text(count);
        $('#IssueGridShowingTotal').text(count);
    }

    //ceil
    // var TotalPages = Math.round(pag);
    var TotalPages = Math.ceil(pag);
    $('#IssueGridPagination').pagination({
        pages: TotalPages,
        cssStyle: 'compact-theme',
        currentPage: 1,
        hrefTextPrefix: '#',
        displayedPages: 7,
        onPageClick: function (page) {
            pageNum = page;
            
            var search = 0//$('#issuecount').val();

            if (search.length > 1) {
                $.ajax({
                    url: '/Dashboard/GetProjectIssue',
                    type: 'post',
                    data: { 'value': search, 'Page': page },
                    success: function (res) {
                        $('#divissues').empty();
                        $('#divissues').html(res);
                        fnWoStatus();
                        var showing = parseInt(page) * 5;
                        $('#GridShowing').text(showing);

                    }
                });
            } else {
                //$.ajax({
                //    url: '/Project/Dashboard/GetProjectIssue',
                //    type: 'post',
                //    data: {
                //        ProjectId: ProjectID, 'Page': page, 'TaskIds': $.cookie('ProjectTasks')
                //        , 'LocationIds': $.cookie("ProjectMarkets"), 'FromDate': $.cookie("FromDate"), 'ToDate': $.cookie("ToDate")
                //    },
                //    success: function (res) {
                //        $('#divissues').empty();
                //        $('#divissues').html(res);
                //fnWoStatus();
                var IssuesStatus = $("#example-getting-started").val();
                IssuesStatus = IssuesStatus.join(',');
                var RDate = $('#reportrange3').find('.changingSelector').html();
                var Datees = RDate.split('-');
                $.ajax({
                    url: '/Project/Dashboard/GetProjectIssue?ProjectId=' + $("#pId").attr("data-ProjectId") + '&Page=' + page + '&TaskIds=' + IssuesStatus
                + '&LocationIds=' + $.cookie(CookiesIdentifier + "PMIssuesProjectMarkets") + '&FromDate=' + Datees[0] + '&ToDate=' + Datees[1],
                    type: 'Get',
                    //data: { 'Searchoption': $(this).val(), ProjectId: ProjectID1, 'Page': 1 },
                    success: function (res) {
                        if (res == '' || res == null) {
                            location.reload();
                        }
                        $('#divissues').empty();
                        $('#divissues').html(res);
                        var showing = parseInt(page) * 5;
                        $('#GridShowing').text(showing);

                    }
                });
            }

        },

    });
    $('#GridShowing').text($('#example1 tr').length - 1);

    if (count > 5) {
        $('#GridShowing').text("5");
        $('#GridShowingTotal').text(count);
    }
    else {
        $('#GridShowing').text(count);
        $('#GridShowingTotal').text(count);
    }
   
}