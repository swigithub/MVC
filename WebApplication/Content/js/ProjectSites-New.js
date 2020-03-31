function SectorLatLng() {
    var siteLatitude = 0;
    var siteLongitude = 0;
    if ($('#SectorLatLng').is(':checked')) {
        siteLatitude = $('#siteLatitude').val();
        siteLongitude = $('#siteLongitude').val();

        $('input[name="SectorLatitude"]').each(function () {
            $(this).val(siteLatitude);
        });
        $('input[name="SectorLongitude"]').each(function () {
            $(this).val(siteLongitude);
        });
    }

}
var RowCounter = 0
var formdata = new FormData();


function StateChange(e) {
    // debugger
    var value = $('#State').val();
    $('#Region').empty();
    $('#Region').append(' <option value="0">-Region-</option></select>');
    $.each(Regions, function (i, v) {
        if (v.PDefinationId == value) {
            $('#Region').append(' <option value=' + v.DefinationId + '>' + v.DefinationName + '</option></select>');
        }
    });
}
$(document).on('change', '.State', function () {
    // debugger
    var Id = this.id;
    var Number = Id.split("-r")[1];
    //console.info(Number[1]);

    var value = $('#State-r' + Number).val();
    $('#Region-r' + Number).empty();
    $('#Region-r' + Number).append(' <option value="0">-Region-</option></select>');
    $.each(Regions, function (i, v) {
        if (v.PDefinationId == value) {
            $('#Region-r' + Number).append(' <option value=' + v.DefinationId + '>' + v.DefinationName + '</option></select>');
        }
    });
});
$(document).on('change', '.Region', function () {
    // debugger
    var Id = this.id;
    var Number = Id.split("-r")[1];
    // console.info(Number[1]);

    var value = $('#Region-r' + Number).val();
    $('#Market-r' + Number).empty();
    $('#Market-r' + Number).append(' <option  value="0">-Market-</option></select>');
    $.each(Cities, function (i, v) {
        if (v.PDefinationId == value) {
            $('#Market-r' + Number).append(' <option value=' + v.DefinationId + '>' + v.DefinationName + '</option></select>');
        }
    });
});
function RegionChange(e) {
    // debugger
    var abc = e.val();
    var value = $('#Region').val();
    $('#Market').empty();
    $('#Market').append(' <option  value="0">-Market-</option></select>');
    $.each(Cities, function (i, v) {
        if (v.PDefinationId == value) {
            $('#Market').append(' <option value=' + v.DefinationId + '>' + v.DefinationName + '</option></select>');
        }
    });
}

function CheckListTypeIdChange(RowNumber) {
    var value = $('#CheckListTypeId-' + RowNumber).val();
    $('#CheckListPId-' + RowNumber).empty();
    $('#CheckListPId-' + RowNumber).append(' <option >-Sub Check List-</option></select>');
    $.each(SubCheckList, function (i, v) {
        if (v.PDefinationId == value) {
            $('#CheckListPId-' + RowNumber).append(' <option value=' + v.DefinationId + '>' + v.DefinationName + '</option></select>');
        }
    });
}

function SiteExistOrNot(SiteCode, NetworkModeId, BandId, CarrierId, ScopeId) {

    if (SiteCode == "" || SiteCode == null) {
        return false;
    }
    var data = { 'SiteCode': SiteCode, 'NetworkModeId': NetworkModeId, 'BandId': BandId, 'CarrierId': CarrierId, 'ScopeId': ScopeId }
    $.ajax('/WorkOrder/SiteExistOrNot', data, 'post', function (res) {
        if (res == 'True') {
            $.notify("Site Code & Network Layer Already Exists!", { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
            $('#btn-submit').hide();
            ShowSaveBtn = 1;
        } else {
            $('#btn-submit').show();
            ShowSaveBtn = 0;
        }
    });
    if (ShowSaveBtn == 0) {
        $('#btn-submit').show();
    } else {
        $('#btn-submit').hide();
    }
}

function LoadCarriers(row, value) {

    $('#Carrier-' + row).empty();
    $.each(Carriers, function (i, v) {
        if (v.PDefinationId == value && v.IsActive == true) {
            $('#Carrier-' + row).append(' <option value=' + v.DefinationId + '>' + v.DefinationName + '</option></select>');
        }
    });
    SiteExistOrNot(SiteCode, SelectednetworkMode, SelectedBand, SelectedCarrier, SelectedScope);
}

function LoadBands(row, value) {
    $('#Band-' + row).empty();
    $('#Band-' + row).append(' <option  value="0">-Band-</option></select>');
    $.each(Bands, function (i, v) {
        if (v.PDefinationId == value) {

            $('#Band-' + row).append(' <option value=' + v.DefinationId + '>' + v.DefinationName + '</option></select>');
        }
    });
    SiteExistOrNot(SiteCode, SelectednetworkMode, SelectedBand, SelectedCarrier, SelectedScope);
}

function LoadMarketes(row, value) {
    return
    $('#Market-' + row).empty();
    $('#Market-' + row).append(' <option  value="0">-Market-</option></select>');
    $.each(Cities, function (i, v) {
        if (v.PDefinationId == value) {
            $('#Market-' + row).append(' <option value=' + v.DefinationId + '>' + v.DefinationName + '</option></select>');
        }
    });
}

function LoadRegions(row, value) {
    $('#Region-' + row).empty();
    $('#Region-' + row).append(' <option  value="0">-Region-</option></select>');
    $.each(Regions, function (i, v) {
        if (v.PDefinationId == value) {
            $('#Region-' + row).append(' <option value=' + v.DefinationId + '>' + v.DefinationName + '</option></select>');
        }
    });

}

function ProjectSitesNewRow() {
    RowCounter++;
    var tempCount = parseInt(RowCount) - 1;
    var NewRow = "";
    var RowsFields = (ProjectSitesNewRowD.match(new RegExp("r" + tempCount, "g")) || []).length;
    for (var i = 0; i < RowsFields; i++) {
        ProjectSitesNewRowD = ProjectSitesNewRowD.replace("r" + tempCount, "r" + RowCount + "");
    }
    $('#TBody').append('<tr>' + ProjectSitesNewRowD + '</tr>');
    $('.ClusterRowFirst').last().focus();
    setTimeout(function () {
        if (ProjectId != 0) {
            $('.ProjectId').val(ProjectId);
            $('.ProjectId').attr("disabled", true);

        }
    }, 1000);
    RowCount++;
    ///////
    //$('.mymultiselect').multiselect({
    //    selectedClass: 'multiselect-selected',
    //    allSelectedText: 'selected',
    //    numberDisplayed: 1,
    //    maxHeight: 400,
    //    buttonWidth: '80%',
    //    onDropdownHide: function (event) {

    //    },

    //});
    //var nnnn = $("#networkmodefake-r" + tempCount).parent().parent().find(".btn-group");
    //nnnn.remove();

}
function BindBody() {
    $('body').bind('cut copy paste', function (e) {
        e.preventDefault();
    });
}

function NewRow() {

    var str = NewDeviceRow.replace("Band-r0", "Band-r" + count + "");

    var RowsFields = (str.match(new RegExp("r0", "g")) || []).length;

    for (var i = 0; i < RowsFields; i++) {
        str = str.replace("r0", "r" + count + "");
    }


    $('#tbl-wo').append('<tr class="NewRow">' + str + '</tr>');
    $('.RowFirst').last().focus();
    count++;


    var LastRowNumber = $('.RowFirst').last().attr('data-row');
    var obj = RowsData[(RowsData.length - 1)];
    //read object
    //for (var key in obj) {
    //    console.log('key: ' + key + '\n' + 'value: ' + obj[key]);
    //}


    var $row = $('#tbl-wo tr').last();
    $("td .form-control", $row).each(function (j) {
        var name = $(this).attr('data-name');
        // console.log(obj[name]);
        var value = $(this).val(obj[name]);

        if (name == 'Azimuth') {
            $(this).val('');
        }
        if (name == 'PCI') {
            $(this).val('');
        }
        if (name == 'sectorCode') {
            $(this).val(SelectedSector + 1);
        }
    });

    LoadBands(LastRowNumber, SelectednetworkMode)
    $('.networkMode').last().val(SelectednetworkMode);
    $('.Scope').last().val(SelectedScope);
    LoadCarriers(LastRowNumber, SelectedBand)
    $('.Band').last().val(SelectedBand);
    $('.Carrier').last().val(SelectedCarrier);

}

RowsData = [];

function OnTabNewRow($row) {
    if (ShowSaveBtn == 0) {
        $('#btn-submit').show();
    } else {
        $.notify("Site Code & Network Layer Already Exists!", { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
        $('#btn-submit').hide();
        return false;
    }

    //var $row = $(this).closest("tr");

    var data = {};
    $("td .form-control", $row).each(function (j) {
        var name = $(this).attr('data-name');
        var value = $(this).val();
        data[name] = value;

    });
    RowsData.push(data);
    NewRow();
}

$(function () {

    BindBody();

    $(".AllowPaste").focusin(function () {
        $("body").unbind();
    });

    $(".AllowPaste").focusout(function () {
        BindBody();
    });

    $(document).on('blur', '.blur', function () {
        SectorLatLng();
    });

    $(document).on('change', '#ddlMarkets', function () {
        $('#ddlMarketSites').empty();
        $.ajax({
            url: '/workorder/UniqueMarketSiteCodes',
            type: 'post',
            data: { 'Filter': 'UniqueSiteCode', 'value': $(this).val() },
            success: function (res) {
                if (res != 'session expired') {
                    $.each(res, function (i, v) {
                        $('#ddlMarketSites').append(' <option value=' + v.SiteCode + '>' + v.SiteCode + '</option>');
                    });
                    // $(".select2").select2();
                } else {
                    $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }
            },
            error: function (err) {
                $.notify(err.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
            },
        });

    });

    $(document).on('click', '#btn-SiteCode', function () {

        $.ajax({
            url: '/workorder/UniqueMarketSiteCodes',
            type: 'post',
            data: { 'Filter': 'AllbySiteCode', 'value': $('#ddlMarketSites').val() },
            success: function (res) {
                if (res != 'session expired') {
                    $('#pan-sector').show();
                    $.each(res, function (i, v) {
                        NewMarketRow(v);
                    });
                    //  console.log(res);
                    fnHideModal();
                } else {
                    $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }
            },
            error: function (err) {
                $.notify(err.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
            },
        });
    });

    //$(document).on('keypress', '.siteCode', function (e) {
    //    if (e.which === 95)
    //        return false;
    //    else if (e.which === 32)
    //        return false;
    //});

    $(document).on('blur', '.siteCode', function (e) {
        SiteCode = $(this).val();
       // SiteExistOrNot(SiteCode, SelectednetworkMode, SelectedBand, SelectedCarrier, SelectedScope);
    });
    $(document).on('click', '#btn-NewRow', function () {
        NewRow();
    });

    //$(document).on('change', '.State', function () {
    //    var value = $(this).val();
    //    SelectedState = value;
    //    var row = $(this).attr('data-row');
    //    LoadRegions(row, value);
    //});
    //
    $(document).on('change', '.sectorCode', function () {
        SelectedSector = $(this).val();
        //  console.log($(this).prop('selectedIndex'));
    });

    //$(document).on('change', '.Market', function () {
    //    SelectedMarket = $(this).val();
    //});

    //$(document).on('change', '.Client', function () {
    //    SelectedClient = $('#Client').val();
    //});

    //$(document).on('change', '.Region', function () {
    //    var value = $(this).val();
    //    SelectedRegion = value;
    //    var row = $(this).attr('data-row');
    //    LoadMarketes(row, value);
    //});

    $(document).on('change', '.networkMode', function () {
        SelectednetworkMode = $(this).val();
        var row = $(this).attr('data-row');
        LoadBands(row, SelectednetworkMode);

    });

    $(document).on('change', '.Scope', function () {
        SelectedScope = $(this).val();
        SiteExistOrNot(SiteCode, SelectednetworkMode, SelectedBand, SelectedCarrier, SelectedScope);
    });

    $(document).on('change', '.Band', function () {
        SelectedBand = $(this).val();
        var row = $(this).attr('data-row');
        LoadCarriers(row, SelectedBand);
        SelectedCarrier = $('#Carrier-' + row).val();
        SiteExistOrNot(SiteCode, SelectednetworkMode, SelectedBand, SelectedCarrier, SelectedScope);
    });

    $(document).on('change', '.Carrier', function () {
        SelectedCarrier = $(this).val();
        SiteExistOrNot(SiteCode, SelectednetworkMode, SelectedBand, SelectedCarrier, SelectedScope);
    });

    $(document).on('change', '.required', function () {
        $(this).removeClass('required');
    });

    $(document).on('keydown', '.RowLast', function (e) {
        var keyCode = e.keyCode || e.which;

        if (keyCode == 9) {
            e.preventDefault();
            var $row = $(this).closest("tr");
            OnTabNewRow($row);
            SectorLatLng();

        }
    });

    //New Row keydown
    $(document).on('keydown', '.ProjectSitesRowLast', function (e) {
        var keyCode = e.keyCode || e.which;

        if (keyCode == 9) {
            e.preventDefault();
            var hit = 0;
            $('#pan-SiteInfo select').each(function () {
                var value = $.trim($(this).val())
                var required = $(this).attr('data-required');
                if (required != 'no' && required != undefined) {
                    if (value == "0" || value == '' || value == null) {
                        $(this).addClass('required');
                        hit = 1;
                        // $.notify('Select ' + $(this).attr('data-name'), { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                        $.notify('Please Select an option first!', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                        return false;
                    } else {
                        $(this).removeClass('required');
                    }
                }
            });
            if (hit == 0) {
                $('#pan-SiteInfo input').each(function () {
                    var textBox = $.trim($(this).val())
                    var required = $(this).attr('data-required');
                    if (required != 'no') {
                        if (textBox == "") {
                            $(this).addClass('required');
                            hit = 1;
                            // $.notify('Enter Cluster Id', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            $.notify('Please fill required field.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            return false;
                        } else {
                            $(this).removeClass('required');
                        }
                    }
                });

            }
            if (hit == 0) {
                ProjectSitesNewRow();
            }
        }
    });
  // delete row
    //$(document).on('keydown', '.Row', function (e) {
    //    var keyCode = e.keyCode || e.which;


    //    if (e.keyCode == 46) {
    //        e.preventDefault();
    //        // $('.newrow').last().remove();
    //        $(this).closest('tr').remove();
    //        $('.RowLast').last().focus();
    //    }

    //});

    $(document).on('submit', '#frm-wo', function (evt) {
        var hit = 0;
        var Scope = $('#Scope :selected').text();

        $('#pan-SiteInfo select').each(function () {

            var value = $.trim($(this).val());
            var txtClient = $.trim($(this).text());

            var required = $(this).attr('data-required');
            if (required != 'no' && required != undefined) {
                if (value == "0" || value == '' || value == null) { //to make client optional:::::   txtClient == '--Select Client--'
                    var inputName = $(this).attr('data-inputName');
                    $(this).addClass('required');
                    hit = 1;
                    //$.notify('Select ' + $(this).attr('data-name'), { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                    $.notify('Please select ' + inputName, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                    return false;
                } else {
                    $(this).removeClass('required');
                }
            }
        });

        if (hit == 0) {
            $('#pan-SiteInfo input').each(function () {
                debugger
                var textBox = $.trim($(this).val())
                var required = $(this).attr('data-required');
                if (required != 'no') {
                    if (textBox == "") {
                        var inputName2 = $(this).attr('data-inputName');
                        $(this).addClass('required');
                        hit = 1;
                        //$.notify('Enter Cluster Id', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                        $.notify('Please fill ' + inputName2, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                        return false;
                    } else {
                        $(this).removeClass('required');
                    }
                }
            });

        }
       
        if (hit == 0) {
            var Form = '#TBody tr';
            var DataObjects = [];
            $(Form).each(function (i) {
                $(".ProjectId").attr("disabled", false);

                var abc = "";
                var def = "";

                $("td input", this).each(function (j) {

                    var name = $(this).attr('data-name');

                    $(this).attr('name', '[' + i + '].' + name);
                });
                $("td select", this).each(function (j) {
                    var name = $(this).attr('data-name');
                    $(this).attr('name', '[' + i + '].' + name);
                });
                $("td select", this).each(function (j) {
                    var name = $(this).attr('data-name');
                   
                    $(this).attr('name', '[' + i + '].' + name);
                });
                $("td input[type='checkbox']", this).each(function (j) {
                    var name = $(this).attr('data-name');
                    console.log(name);
                    $(this).attr('name', '[' + i + '].' + name);
                });
            });
            
            $.ajax({
                url: '/ProjectSites/New',
                type: 'POST',
                async: false,
                dataType:"json",
                data: $(this).serialize(),

                success: function (response) {
                    $(".ProjectId").attr("disabled", true);
                    console.log(response);
                    var res = response.response
                    if (res != 'session expired') {
                        if (res.Status == 'success') {
                            $.notify("Save successfully !", { type: "success", color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                            setTimeout(function () { location.reload(); }, 1000);

                            formdata = new FormData();
                            RowCounter = 0

                        }else if (res.Status == 'exist') {
                            $.notify(res.Message, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            evt.preventDefault();
                        }
                        else if (res.Status == 'exception') {
                            $.notify('There is an Problem. Please try again!!', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            evt.preventDefault();
                        }
                        else {
                            $.notify(res.Message, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });

                        }
                    } else {
                        $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                    }
                    //  alert(res);
                },
                error: function (response) {
                    $(".ProjectId").attr("disabled", true);
                    $.notify(res.Message, { type: "danger", color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                    setTimeout(function () { location.reload(); }, 1000);

                    formdata = new FormData();
                    RowCounter = 0
                },

            });
            return false;
        }


        return false;
    });

    $(document).on('click', '.multiselect', function () {

        var jk = $(this).parent().hasClass("open");
        if (jk == false) {
            $(this).parent().addClass("open");
        }
        else {
            $(this).parent().removeClass("open")
        }
    });

    $(document).on('click', '.checkbox', function () {
        return
        // $(this).parent().parent().parent().parent().remove();
        $('.mymultiselect').multiselect({
            selectedClass: 'multiselect-selected',
            allSelectedText: 'selected',
            numberDisplayed: 1,
            maxHeight: 400,
            buttonWidth: '80%',
            onDropdownHide: function (event) {

            },

        });

    });
    //reset
    $(document).on('click', '#reset', function () {
        $("#TBody").html('');
        $('#TBody').append('<tr>' + ProjectSitesNewRowD + '</tr>');

        setTimeout(function () {
            if (ProjectId != 0) {
                $('.ProjectId').val(ProjectId);
                $('.ProjectId').attr("disabled", true);

            }
        }, 1000);
    });

});
