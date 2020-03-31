var selectedDevices = [];


//-----------------------------------------------

//addClass
//removeClass
$(function () {

    //when we land on this page from equipments list page

    var val = $('#UEStatusId :selected').val();
    var text = $('#UEStatusId :selected').text();

    loadStatus(text, val);

    if (text == 'Return') {

        $('#UEStatusId').prop('disabled', true);
        $('#FromUserId').prop('disabled', true);
        $('#UserId').prop('disabled', true);
        $('#UETypeId').prop('disabled', true);
        $('#Devices').prop('disabled', true);

        $("#issueUserList").addClass("issueUserList");
        $("#fromUserList").removeClass("fromUserList");
    }
    else if (text == 'Transfer') {

        $('#UEStatusId').prop('disabled', true);
        $('#FromUserId').prop('disabled', true);
        $('#UserId').prop('disabled', false);
        $('#UETypeId').prop('disabled', true);
        $('#Devices').prop('disabled', true);

        $('#fromUserList').removeClass('fromUserList');
        $('#issueUserList').removeClass('issueUserList');
    }
    else if (text == 'Issue') {
        $('#UEStatusId').prop('disabled', true);
        $('#UETypeId').prop('disabled', true);
        $('#Devices').prop('disabled', true);

        $('#issueUserList').removeClass('issueUserList');
        $('#fromUserList').addClass('fromUserList');
    }


    //--------------------------------------------
    $('#UEStatusId').change(function () {

        var text = $(' :selected', this).text();
        var val = $(this).val();

        loadStatus(text, val);

    });


    //--------------------------------------------
    function loadStatus(text, val) {
        if (val > 0) {

            $('#UETypeId').prop('disabled', false);
        } else {

            $('#UETypeId').prop('disabled', 'disabled');
        }

        //hidden text box
        $('#UEStatus').val(text);

        $("#tbl-items tbody").remove();
        $('#Devices').empty();
        $('#Devices').append(' <option value="-1">-Equipment-</option></select>');
     

        if (text == 'Return') {
            Users('DevicesUsers', '');

            $("#issueUserList").addClass("issueUserList");
            $("#fromUserList").removeClass("fromUserList");
        }
        else if (text == 'Transfer') {
            Users('All', '');
            $('#issueUserList').removeClass('issueUserList');
            $('#fromUserList').removeClass('fromUserList');
        }
        else if (text == 'Issue') {
            Users('All', '');
            $('#issueUserList').removeClass('issueUserList');
            $('#fromUserList').addClass('fromUserList');
        }
    }

    //-----------------------------------------------
    $('#frm-Movement').parsley();
    //$('#frm-Movement').submit();
    $('#btn-submit').click(function () {
        var k = -1;
        $('#tbl-items tr').each(function (i) {
            i = i - 1;
            $("td input", this).each(function (j) {
                var name = $(this).attr('name').split(".").pop();
                $(this).attr('name', '[' + k + '].' + name);
            });
            k++;
        });

        $.ajax({
            url: '/Equipment/Movement',
            type: 'post',
            async: false,
            data: $('#frm-Movement').serialize(),
            success: function (res) {
                if (res != 'session expired') {
                    if (res.Status == 'success') {
                        $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });

                        window.location.href = "/equipment/index";

                    } else {
                        $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                    }
                } else {
                    $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }
            },
            error: function (err) {
                $.notify(err.statusText, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6 });
            },
        });
        return false;
    });


    //--------------------------------------------
    $('#frm-Movement').submit(function () {
        var k = -1;
        //debugger;
        $('#tbl-items tr').each(function (i) {
            i = i - 1;
            $("td input", this).each(function (j) {
                var name = $(this).attr('name').split(".").pop();
                $(this).attr('name', '[' + k + '].' + name);
            });
            k++;
        });

        $.ajax({
            url: '/Equipment/Movement',
            type: 'post',
            async: false,
            data: $(this).serialize(),
            success: function (res) {
                if (res != 'session expired') {
                    if (res.Status == 'success') {
                        $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });

                    } else {
                        $.notify(res.Message, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                    }
                } else {
                    $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }
            },
            error: function (err) {
                $.notify(err.statusText, { type: res.Status, color: "#ffffff", background: "#D44950", blur: 0.6 });
            },
        });
        return false;
    });

    setTimeout(
  function () {
      loadEquipment();
  },1000);
  
});


//+++++++++++++++++++++++++++++++++++++++++++++++++
$('#FromUserId').change(function () {

    $('#UETypeId').val(0);
    $('#tbl-items tbody').empty();

    //var FromUserId = $('#FromUserId').val();
    //var FromUserName = $('#FromUserId :selected').text();
    //var UEId = $('#Devices').val();

    //loadGridData(FromUserId, FromUserName, UEId);
});


//--------------------------------------------
$('#UserId').change(function () {

    //$('#UETypeId').val(0);
    $('#tbl-items tbody').empty();

    var FromUserId = $('#FromUserId').val();
    var UserId = $('#UserId').val();
    var UserName = $('#UserId :selected').text();
    var UEId = $('#Devices').val();

    loadGridData(FromUserId,UserId, UserName, UEId);
});


//-----------------------------------------------
function Users(filter, value) {
    $('#FromUserId').empty();
    //$('#FromUserId').append(' <option value="-1">-Users-</option></select>');

    $('#UserId').empty();
    //$('#UserId').append(' <option value="-1">-Users-</option></select>');

 
    $.ajax({
        url: '/User/SelectableList/',
        type: 'post',
        data: { filter: filter, value: value },
        success: function (res) {
            if (res.length > 0) {
                $.each(res, function (i, v) {
                    $('#FromUserId').append(' <option value=' + v.Value + '>' + v.Text + '</option></select>');
                    $('#UserId').append(' <option value=' + v.Value + '>' + v.Text + '</option></select>');
                });
            }
            $('#FromUserId').val(UEUserId);
            $('#UserId').val(UEUserId);
        }
    });
}

//-----------------------------------------------
$('#UETypeId').change(function () {

    loadEquipment();

    //var val = $(this).val();
    //var filter = $('#UEStatus').val();
    //if (val > 0) {
    //    $('#Devices').prop('disabled', false);
    //    if (filter == 'Return') {
    //        //  debugger;
    //        val = $('#Users').val();
    //        LoadDevices(filter, val);
    //    } else {
    //        LoadDevices(filter, val);
    //    }
    //} else {
    //    $('#Devices').prop('disabled', 'disabled');
    //    $('#Devices').empty();
    //    $('#Devices').append(' <option value="-1">-Equipment-</option></select>');
    //}

});


//--------------------------------------------
function loadEquipment() {
    //var val = $(this).val();
    $('#Devices').empty();
    var val = $('#UETypeId :selected').val();

    var filter = $('#UEStatus').val();
    debugger;
    if (val > 0) {
        $('#Devices').prop('disabled', false);
        // because Return OR Transfer devices must be associated with user
        if (filter == 'Return') {
            
            var usrId = $('#FromUserId :selected').val();
            LoadDevices(filter, usrId);

        }
        else {
            LoadDevices(filter, val);
        }
    } else {
        $('#Devices').prop('disabled', 'disabled');
        $('#Devices').empty();
        $('#Devices').append(' <option value="-1">-Equipment-</option></select>');
    }
}


//-----------------------------------------------
function LoadDevices(filter, value) {
    $('#Devices').empty();
    // $('#Devices').append('<option value="-1">-Equipment-</option></select>');
    $.ajax({
        url: '/Equipment/Devices/',
        type: 'post',
        data: { filter: filter, value: value },
        success: function (res) {
            if (res.length > 0) {
                $.each(res, function (i, v) {
                    $('#Devices').append(' <option value=' + v.Value + '>' + v.Text + '</option></select>');
                    $('#Devices').val(UEDeviceId);
                });
            }

            var FromUserId = 0;
            var UserId = 0;
            var UserName = "";

            if ($('#UserId').val() > 0) {
                UserId = $('#UserId').val();
                UserName = $('#UserId :selected').text();
            }

            if ($('#FromUserId').val() > 0) {
                FromUserId = $('#FromUserId').val();
                UserName = $('#FromUserId :selected').text();
            }

            var UEId = $('#Devices').val();

            loadGridData(FromUserId,UserId, UserName, UEId);
        }
    });
}


//-----------------------------------------------
$('#Devices').change(function () {

    var FromUserId = 0;
    var UserId = 0;
    var FromUserName = "";
    var UserName = "";

    debugger;

    if ($('#FromUserId').val() > 0) {
        FromUserId = $('#FromUserId').val();
        FromUserName = $('#FromUserId :selected').text();
    }

    var txt = $('#UEStatusId :selected').text();

    if ($('#UserId').val() > 0 && ((txt == "Issue") || (txt == "Transfer"))) {
        UserId = $('#UserId').val();
        UserName = $('#UserId :selected').text(); 
    }

    if ($('#UserId').val() > 0 && txt == "Return") {
        UserId = $('#UserId').val();
        UserName = $('#FromUserId :selected').text();
    }

    var UEId = $('#Devices').val();

    loadGridData(FromUserId, UserId, UserName, UEId);

    //var UserId = $('#UserId').val();
    //var UEId = $(this).val();

    //var exist = jQuery.inArray(UserId + '-' + UEId, selectedDevices);
    ////console.log(exist);
    //if (exist == -1) {
    //    if (UEId > 0 && UserId > 0) {
    //        var row = '<tr>' +
    //            '<td><input type="hidden" value="' + $('#UEStatusId :selected').val() + '" name="UEStatusId">' + $("#UEStatus").val() + '</td>' +
    //            '<td><input type="hidden" value="' + UserId + '" name="UserId">' + $('#UserId :selected').text() + '</td>' +
    //            '<td><input type="hidden" value="' + $('#UETypeId').val() + '" name="UETypeId">' + $('#UETypeId :selected').text() + '</td>' +
    //            '<td><input type="hidden" value="' + UEId + '" name="UEId">' + $(' :selected', this).text() + '</td>' +
    //            '<td><a href="#" onclick="RemoveRow(this,\'' + UserId + '-' + UEId + '\'); return false;"><i style="color:#dd4b39;text-align:center" class="fa fa-trash"></i></a></td></tr>';

    //        $('#tbl-items').append(row);
    //        selectedDevices.push(UserId + '-' + UEId);
    //    }
    //}

});

function loadGridData(FromUserId, UserId, UserName, UEId) {

    var exist = jQuery.inArray(UserId + '-' + UEId, selectedDevices);

    if (exist == -1) {
        if (UEId > 0 && UserId > 0) {
            var row = '<tr>' +
                '<td><input type="hidden" value="' + $('#UEStatusId :selected').val() + '" name="UEStatusId">' + $("#UEStatus").val() + '</td>' +
                '<td><input type="hidden" value="' + FromUserId + '" name="FromUserId">' +
                    '<input type="hidden" value="' + UserId + '" name="UserId">' + UserName + '</td>' +
                '<td><input type="hidden" value="' + $('#UETypeId').val() + '" name="UETypeId">' + $('#UETypeId :selected').text() + '</td>' +
                '<td><input type="hidden" value="' + UEId + '" name="UEId">' + $('#Devices :selected').text() + '</td>' +
                '<td><a href="#" onclick="RemoveRow(this,\'' + UserId + '-' + UEId + '\'); return false;"><i style="color:#dd4b39;text-align:center" class="fa fa-trash"></i></a></td></tr>';
            $('#tbl-items').append(row);
            selectedDevices.push(UserId + '-' + UEId);
        }
    }
}


//--------------------------------------------
function RemoveRow(e, item) {
    $(e).closest('tr').remove();
    selectedDevices = jQuery.grep(selectedDevices, function (value) {
        return value != item;
    });
}

//-----------------------------------------------
function Reset() {
    $('#UEStatusId').val(0);
    $('#UserId').val(0);
    $('#UserId').val(0);
    $('#UETypeId').prop('disabled', 'disabled');
    $('#Devices').prop('disabled', 'disabled');
}

//--------------------------------------------
//function getURL() {
//    return location.protocol + '//' + location.host + location.pathname
//}


