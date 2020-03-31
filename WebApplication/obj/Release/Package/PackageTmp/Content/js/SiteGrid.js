/*********Activity***********/
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



function ReportSubmitOrApprove(SiteId, NetworkModeId, CarrierId, ScopeId, BandId, Scope) {

    
    return false;
}



function ReportSubmitOrApprove(SiteId, NetworkModeId, CarrierId, ScopeId, BandId, StatusKeyCode) {

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




