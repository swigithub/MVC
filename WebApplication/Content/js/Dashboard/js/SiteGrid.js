////*********Activity***********/
function ActivityPopup(SiteId, NetworkModeId, CarrierId, ScopeId, BandId, TesterId) {
    modal_title.text('Activity');
    modal_content.css({ 'width': '700px' });
    modal.modal('show');
    $.ajax({
        url: '/SiteTrackerIssue/NetLayer',
        //async: false,
        data: { 'SiteId': SiteId, 'NetworkModeId': NetworkModeId, 'CarrierId': CarrierId, 'ScopeId': ScopeId, 'BandId': BandId, 'TesterId': TesterId },
        success: function (suc) {
            if (suc != null || suc != "↵" || suc.trim() != "") {
                modal_body.empty();
                modal_body.html(suc);
                modal_footer.remove();
            }
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}
$(document).on('submit', '#frm-IssueTracker', function () {
    var SiteId = $("input[name='SiteId']", this).val();
    var NetworkModeId = $("input[name='NetworkModeId']", this).val();
    var CarrierId = $("input[name='CarrierId']", this).val();
    var ScopeId = $("input[name='ScopeId']", this).val();
    var BandId = $("input[name='BandId']", this).val();
    var TesterId = $("input[name='TesterId']", this).val();
    $.ajax({
        url: '/SiteTrackerIssue/New',
        data: $(this).serialize(),
        type: 'post',
        success: function (res) {
            ActivityPopup(SiteId, NetworkModeId, CarrierId, ScopeId, BandId, TesterId);
        }

    });
    return false;
});
$(document).on('click', '.btn-resolve', function () {
    var TrackingId = $(this).attr('data-TrakerId');
    $.ajax({
        url: '/SiteTrackerIssue/UpdateStatus?TrakingId=' + TrackingId + '&Status=RESOLVED',
        type: 'post',
        success: function (res) {
            if (res == 'True') {
                $('#btn-resolve-' + TrackingId).text('Acknowledged');
                $('#btn-resolve-' + TrackingId).addClass('btn-success');
                $('#btn-resolve-' + TrackingId).removeClass('btn-danger');
            } else {
                alert(res);
            }

        }

    });
});

/*********Activity End***********/



//function ReportSubmitOrApprove(SiteId, NetworkModeId, CarrierId, ScopeId, BandId, Scope) {


//    return false;
//}



function ReportSubmitOrApprove(SiteId, NetworkModeId, CarrierId, ScopeId, BandId, StatusKeyCode) {
    debugger;
    var url = '';
    var msg = '';
    if (StatusKeyCode == 'REPORT_SUBMITTED') {
        msg = "Do you want to submit report?";
        url = '/NetLayerStatus/MarkReportSubmit';
    }
    if (StatusKeyCode == 'COMPLETED') {
        msg = "Do you want to approve report?";
        url = '/NetLayerStatus/MarkApproved';
    }
    $.confirm({
        title: 'Confirm!',
        content: msg,
        buttons: {
            Yes: function () {
                var data = { 'NetworkModeId': NetworkModeId, 'BandId': BandId, 'SiteId': SiteId, 'CarrierId': CarrierId, 'KeyCode': StatusKeyCode };
                $.ajax({
                    url: url,
                    data: data,
                    type: 'POST',
                    success: function (res) {
                        if (res != 'session expired') {
                            if (res.Status == 'success') {
                                $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                                BtnFilterSitesGrid_Click('', '', '');

                            } else {
                                $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            }
                        } else {
                            $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                        }
                    },
                    error: function (err) {
                        $.notify(err.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                    }
                });
            },
            No: function () { }

        }
    });
    return false;
}

/*********Observations***********/

function ObservationPopup(SiteId, NetworkModeId, CarrierId, ScopeId, BandId) {
    modal_title.text('Observation & Recommendations');
    modal_content.css({ 'width': '500px' });
    modal.modal('show');
    var data = { 'NetworkModeId': NetworkModeId, 'BandId': BandId, 'SiteId': SiteId, 'CarrierId': CarrierId, 'ScopeId': ScopeId };
    ajax('/NetLayerStatus/NewObservation', data, '', function (res) {
        modal_body.empty();
        modal_body.html(res);
    });
    return false;
}
function ObservationSubmit() {

    $.ajax({
        url: '/NetLayerStatus/NewObservation',
        data: $('#frm-Observation').serialize(),
        type: 'POST',
        success: function (res) {
            if (res.Status == "success") {
                fnHideModal();
            }
        },
        error: function (err) { }
    });
    return false;
}

/*********Observations END ***********/


/*********PENDING WITH ISSUES***********/

function PendingIssuesPopup(SiteId, NetworkModeId, CarrierId, ScopeId, BandId) {

    modal_title.text('Pending With Issue');
    modal_content.css({
        'width': '500px'
    });
    modal.modal('show');

    var data = { 'NetworkModeId': NetworkModeId, 'BandId': BandId, 'SiteId': SiteId, 'CarrierId': CarrierId, 'ScopeId': ScopeId };


    ajax('/NetLayerStatus/NewPendingIssue', data, '', function (res) {
        modal_body.html(res);
    });

    return false;
}

function PendingIssuesSubmit() {
    $.ajax({
        url: '/NetLayerStatus/NewPendingIssue',
        data: $('#frm-PendingIssue').serialize(),
        type: 'POST',
        success: function (res) {
            if (res.Status == "success") {
                if (IsShow != 'no') {
                    BtnFilterSitesGrid_Click('', '', '');
                }

                modal.modal('hide');
                fnHideModal();
            }
        },
        error: function (err) { }
    });

    return false;
}
/*********PENDING WITH ISSUES END ***********/


/*********Re-Drive ***********/


function RedrivePopup(SiteId, NetworkModeId, CarrierId, ScopeId, BandId) {

    modal_title.text('Re-Drive Request');
    modal_content.css({ 'width': '500px' });
    modal.modal('show');
    var data = { 'NetworkModeId': NetworkModeId, 'BandId': BandId, 'SiteId': SiteId, 'CarrierId': CarrierId, 'ScopeId': ScopeId };
    ajax('/ReDrive/NewRequest', data, '', function (res) {
        modal_body.empty();
        modal_body.html(res);
    });
    return false;
}

function RedriveSubmit() {
    $('#btn-frm-ReDrive').hide();
    $.ajax({
        url: '/ReDrive/NewRequest',
        data: $('#frm-ReDrive').serialize(),
        async: false,
        type: 'POST',
        success: function (res) {
            if (res.Status == "success") {
                fnHideModal();
                if (IsShow != 'no') {
                    BtnFilterSitesGrid_Click('', '', '');
                }

            }
        },
        error: function (err) { }
    });
    return false;


}



/*********Re-Drive End***********/



/*********LayerWoStatus Movement ***********/
function LayerWoStatus(LayerId) {
    modal_title.text('Change Layer Status');
    modal_content.css({ 'width': '500px' });
    modal.modal('show');
    var data = { 'LayerId': LayerId };
    ajax('/NetLayerStatus/WoStatus', data, '', function (res) {
        modal_body.empty();
        modal_body.html(res);
    });
    return false;
}


//function LayerWoStatusSubmit() {
//    $.ajax({
//        url: '/NetLayerStatus/WoStatus',
//        data: $('#frm-LayerWoStatus').serialize(),
//        type: 'POST',
//        success: function (res) {
//            if (res.Status == "success") {
//                if (IsShow != 'no') {
//                    BtnFilterSitesGrid_Click('', '', '');
//                }

//                modal.modal('hide');
//                fnHideModal();
//            }
//        },
//        error: function (err) { }
//    });

//    return false;
//}


// by junaid , to prevent page refresh

function LayerWoStatusSubmit() {
  
    var statusColor = [];
    var statusColor = new Array();
    statusColor[91] = new Array("#cc99ff", "Scheduled");
    statusColor[92] = new Array("#2517fa", "Drive Completed");
    statusColor[93] = new Array("#e32e2d", "Pending With Issues");
    statusColor[450] = new Array("#59b1f0", "In Progress"); 
    statusColor[451] = new Array("#1dbc08", "Report Submitted"); 
    
    var NetLayerStatusId = $('#frm-LayerWoStatus input#LayerStatusId').val();
    var NetLayerStatus = $('#frm-LayerWoStatus select#Status').val();
    var ParentId = $("[data-netlayerid=" + NetLayerStatusId + "]").parent().parent().attr('id');
    var countChild = $("#" + ParentId).children('tbody').children('tr').length;
    var response = ParentId.split("_");
    var ParentCheck = $("[data-child-value=" + response[1] + "]").children('td:eq(13)').children("span").text();
        
    var cCheck = 0;
    var GetStatus = [];
    var cTimes = 1;
    var cCounter = 0;
    var currentStatus;
         
    if (cTimes == countChild) {
        var childText = $("#" + ParentId).children('tbody').children('tr:eq(0)').children('td:eq(6)').text();
        $("[data-child-value=" + response[1] + "]").children('td:eq(13)').children("span").text(currentStatus).css("background-color", statusColor[NetLayerStatus][0]);
    }

    $.ajax({
        url: '/NetLayerStatus/WoStatus',
        data: $('#frm-LayerWoStatus').serialize(),
        type: 'POST',
        success: function (res) {
            if (res.Status == "success") {
                if (IsShow != 'no') {
                    
                    $("[data-netlayerid=" + NetLayerStatusId + "]").children('td').children('span').text(statusColor[NetLayerStatus][1]).css("background-color", statusColor[NetLayerStatus][0]);
                    for (var x = 1 ; x <= countChild; x++) {
                        var LayerCheck = $("#" + ParentId).children('tbody').children('tr:eq(' + (x - 1) + ')').children('td:eq(6)').text();
                        GetStatus[(x - 1)] = LayerCheck;
                    }

                    for (var i = 0; i < GetStatus.length; i++) {
                        for (var j = i; j < GetStatus.length; j++) {
                            if (GetStatus[i] == GetStatus[j])
                                cCounter++;
                            if (cTimes <= cCounter) {
                                cTimes = cCounter;
                                currentStatus = GetStatus[i];
                            }
                        }
                        cCounter = 0;
                    }

                    if (cTimes == countChild) {
                        var childText = $("#" + ParentId).children('tbody').children('tr:eq(0)').children('td:eq(6)').text();
                        $("[data-child-value=" + response[1] + "]").children('td:eq(13)').children("span").text(currentStatus).css("background-color", statusColor[NetLayerStatus][0]);
                    }
                }

                
                fnHideModal();
            }
        },
        error: function (err) { }
    });
   
    var simpleThis = $("[data-child-value=" + response[1] + "] td:eq(0)")[0];
    var tr = $(simpleThis).closest('tr');
    var row = table.row(tr);
    var TesterId = $(simpleThis).attr('data-TesterId');
    var myScope = $(simpleThis).attr('data-scope');
    var SiteCode = $(simpleThis).attr('data-SiteCode');
    var rec = formatBandsTable(tr.data('child-value'), TesterId, SiteCode, myScope);
    row.child.hide();
    tr.removeClass('shown');


    row.child(rec).show();
    tr.addClass('shown');
    fnHideModal();
    return false;
}
//--------------------------------------------


/*********End LayerWoStatus Movement ***********/

/********* Layer Activation ***********/

function LayerActivation(SiteId) {
    modal_title.text('Change Layer Status');
    modal_content.css({ 'width': '500px' });
    modal.modal('show');
    var data = { 'SiteId': SiteId };
    ajax('/NetLayerStatus/LayerActivation', data, '', function (res) {
        modal_body.empty();
        modal_body.html(res);
    });
    return false;
}
function LayerIsActive(LayerId, Status, SiteId) {
    var msg = 'Do you want to disable Net Layer?';
    if (Status == 'true') {
        msg = 'Do you want to enable Net Layer?';
    }
    $.confirm({
        title: 'Confirm!',
        content: msg,
        buttons: {
            Yes: function () {
                $.ajax({
                    url: '/NetLayerStatus/IsActive?LayerId=' + LayerId + '&Status=' + Status,
                    type: 'post',
                    success: function (res) {
                        if (SiteId > 0) {
                            LayerActivation(SiteId);
                        } else {
                            BtnFilterSitesGrid_Click('', '', '');
                        }
                    }
                });
            },
            No: function () { },


        }
    });

    return false;
}
function SectorIsActive(SectorId, Status, SiteId) {
    var msg = 'Do you want to disable Sector?';
    if (Status == 'true') {
        msg = 'Do you want to enable Sector?';
    }
    $.confirm({
        title: 'Confirm!',
        content: msg,
        buttons: {
            Yes: function () {
                $.ajax({
                    url: '/NetLayerStatus/SectorStatus?SectorId=' + SectorId + '&Status=' + Status,
                    type: 'post',
                    success: function (res) {
                        LayerActivation(SiteId);
                    }
                });
            },
            No: function () { },


        }
    });

    return false;
}
function IsActiveWo(SiteId, Status) {
    $.confirm({
        title: 'Confirm!',
        content: 'Do you want to Deactive Work Order?',
        buttons: {
            Yes: function () {
                $.ajax({
                    url: '/Site/Status?SiteId=' + SiteId + '&Status=' + Status,
                    type: 'post',
                    success: function (res) {
                        if (res != 'session expired') {
                            if (res.Status == 'success') {
                                BtnFilterSitesGrid_Click('', '', '');
                            } else {
                                $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            }
                        } else {
                            $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                        }

                    }
                });
            },
            No: function () { }
        }
    });
    return false;
}

/********* End Layer Activation ***********/


function Script(LayerId, SiteId, NetworkModeId, BandId, CarrierId, ScopeId) {
    modal_title.text('Create Test Script');
    modal_content.css({ 'width': '80%' });
    console.log("sdsdsd", CarrierId);
    var data = { LayerId: LayerId, SiteId: SiteId, NetworkModeId:NetworkModeId,BandId: BandId,CarrierId :CarrierId,ScopeId: ScopeId};
    ajax('/NetLayerStatus/Script', data, '', function (res) {
        modal_body.empty();
        modal_body.html(res);
        modal_footer.hide();
        modal.modal('show');
    });
    return false;
}

function ScriptSubmit() {
    $.ajax({
        url: '/NetLayerStatus/Script',
        type:'post',
        data: $('#frm-script').serialize(),
        success: function (res) {
            console.log(res);
            fnHideModal();
        }
    });
}


