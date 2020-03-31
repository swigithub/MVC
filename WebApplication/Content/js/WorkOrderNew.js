$("#siteLatitude").on("keypress", function (e) {
    //$(this).val($(this).val().replace(/[^0-9\.]/g, ''));
    if (e.which != 46 && e.which != 45 && e.which != 46 &&
      !(e.which >= 48 && e.which <= 57)) {
        return false;
    }
});
$("#siteLongitude").on("keypress", function (e) {
    //$(this).val($(this).val().replace(/[^0-9\.]/g, ''));
    if (e.which != 46 && e.which != 45 && e.which != 46 &&
      !(e.which >= 48 && e.which <= 57)) {
        return false;
    }
});

$(document).on("keypress", ".SectorLatitude", function (e) {
    if (e.which != 46 && e.which != 45 && e.which != 46 &&
       !(e.which >= 48 && e.which <= 57)) {
        return false;
    }
});
$(document).on("keypress", ".SectorLongitude", function (e) {
    if (e.which != 46 && e.which != 45 && e.which != 46 &&
       !(e.which >= 48 && e.which <= 57)) {
        return false;
    }
});

$(document).on("input", ".bandwidth", function () {
    var value = $(this).val();
    if (value<0) {
        $(this).val('0');
        }
});
$(document).on("input", ".mrbts", function () {
    var value = $(this).val();
    if (value < 0) {
        $(this).val('0');
    }
});


//$(".bandw").on("input", function (event) {
//    debugger;
//    var value=$(this).val();
//    if (value < 0) {
//        alert("not minus");
//        return false;
//    }
//});

function SectorLatLng() {
    var siteLatitude = 0;
    var siteLongitude = 0;
    if ($('#SectorLatLng').is(':checked')){
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
$(document).on("click", ".delSurvey", function () {
    debugger;
    var val = $(this).attr("data-SId");
    var SList = [];
    var svalues = $("#hfSurveyId").val();
    SList = svalues.split(',');
    var i = SList.indexOf(val);
    SList.splice(i, 1);
    $("#hfSurveyId").val(SList.join(','));

    $(this).parent().next().remove();
    $(this).parent().remove();
})
function AssignValues() {
    debugger;

    if ($('#SurveyId-r0').select2("val") != "0") {
    var values = $("#hfSurveyId").val();
    if (values != "" && values != null) {
        $("#hfSurveyId").val(values + "," + $("#SurveyId-r0").val());
    }
    else {
        $("#hfSurveyId").val($("#SurveyId-r0").val());
    }
    
    $("#listOfSurveys p").append('<span class="SurveyList">' + $("#SurveyId-r0 option:selected").text() + ' &nbsp; <i class="fa fa-remove delSurvey" data-SId='+$("#SurveyId-r0 option:selected").val()+'></i>  &nbsp;</span><br/>')

        $('#SurveyId-r0').select2("val", $('#SurveyId-r0 option:eq(0)').val());
        $("#SurveyId-r0 ").find('option:first').attr('selected', 'selected');
        $("#SurveyId-r0 ").val(0);
    }


};
var RowCounter = 0
var formdata = new FormData();
//console.log(Surveys);

function split(val) {
    return val.split(/,\s*/);
}
function extractLast(term) {
    return split(term).pop();
}

function ScopeChange() {
    $("#SiteTypeId option:selected").prop("selected", false);
    $("#SiteClassId option:selected").prop("selected", false);
    $(".CLSHIDE").show();
    SectorLatLng();
    var count = 1;
    var TssCount = 1;
    var ClusterCount = 1;
    var value = $('#Scope :selected').text();
    $('#pan-sector').hide();
    $('#pan-TSS').hide();
    $('#pan-Cluster').hide(); 
    if (value == 'SSV' || value == 'NI' || value == 'IND') {
        $('#NewDeviceRow').empty();
        $('#NewDeviceRow').html(NewDeviceRowCLONE)
        $('#pan-sector').show();
    } else if (value == 'TSS') {
        // TssNewRow();
        $("#SurveyId-r0").addClass('required');
        $("#SurveyId-r0").prop('required', true);
        $('.NewRow').empty();
        
        TssNewDeviceRow = $('#TssNewDeviceRow').html();
        $('#TssNewDeviceRow').empty();
        $('#TssNewDeviceRow').html(TssNewDeviceRowCLONE);
        $('#pan-TSS').show();
        var value = $('#Client').val();
        $('.Survey').empty();
        $('.Survey').append(' <option value="0">-Survey Id-</option></select>');
        $.each(Surveys, function (i, v) {
           // debugger
            if (v.ClientId == parseInt(value) || v.IsGlobal) {
                $('.Survey').append(' <option value=' + v.SurveyId + '>' + v.SurveyTitle + '</option></select>');
            }
        });
        $(".Survey").select2();
    }
    else if (value == 'CLS') {
        $("#SurveyId-r0").addClass('required');
        $("#SurveyId-r0").prop('required', false);
        $('#ClusterNewDeviceRow').empty();
        $('#ClusterNewDeviceRow').html(ClusterNewDeviceRowCLONE);
        $('#pan-Cluster').show();
        $(".CLSHIDE").hide();
        $("#SiteTypeId option:contains(N/A)").attr('selected', 'selected');
        $("#SiteClassId option:contains(N/A)").attr('selected', 'selected');
        $('.mymultiselect').multiselect({
            selectedClass: 'multiselect-selected',
            allSelectedText: 'selected',
            numberDisplayed: 1,
            maxHeight: 400,
            buttonWidth: '80%',
            onDropdownHide: function (event) {

            },

        });

    }
}

function StateChange() {
    var value = $('#State').val();
    $('#Region').empty();
    $('#Region').append(' <option value="0">-Region-</option></select>');
    $.each(Regions, function (i, v) {
        if (v.PDefinationId == value) {
            $('#Region').append(' <option value=' + v.DefinationId + '>' + v.DefinationName + '</option></select>');
        }
    });
}

function ClientChange() {
    debugger;
    var value = $('#Client').val();
    $('.Survey').empty();
    $('.Survey').append(' <option value="0">-Survey Id-</option></select>');
    $.each(Surveys, function (i, v) {
        if (v.ClientId == parseInt(value) || v.IsGlobal) {
            $('.Survey').append(' <option value=' + v.SurveyId + '>' + v.SurveyTitle + '</option></select>');
        }
    });
    $(".Survey").select2();
    var scope = $('#Scope :selected').text();
    if(scope == 'TSS')
        {
        $('#pan-TSS').hide();
        $('.NewRow').empty();
       // $("#TssNewDeviceRow").append(TssNewDeviceRow);
        $('#pan-TSS').show();
       // TssNewRow();
    $('.Survey').empty();
    $('.Survey').append(' <option value="0">-Survey Id-</option></select>');
    $.each(Surveys, function (i, v) {
       // debugger
        if (v.ClientId == parseInt(value) || v.IsGlobal) {
            $('.Survey').append(' <option value=' + v.SurveyId + '>' + v.SurveyTitle + '</option></select>');
        }
    });
    TssNewDeviceRow = $('#TssNewDeviceRow').html();
}
}

function RegionChange() {
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
function NewMarketRow(object) {
   var str = NewDeviceRow.replace("Band-r0", "Band-r" + count + "");

    var RowsFields = (str.match(new RegExp("r0", "g")) || []).length;

    for (var i = 0; i < RowsFields; i++) {
        str = str.replace("r0", "r" + count + "");
    }


    $('#tbl-wo').append('<tr class="NewRow">' + str + '</tr>');
    $('.RowFirst').last().focus();

    $('#siteCode' ).val(object.SiteCode);
    $('#Antenna-r' + count).val(object.Antenna);
    $('#Azimuth-r' + count).val(object.Azimuth);
    $('#BandWidth-r' + count).val(object.BandWidth);
    $('#BeamWidth-r' + count).val(object.BeamWidth);
    $('#siteLatitude').val(object.Latitude);
    $('#siteLongitude').val(object.Longitude);
    $('#MTilt-r' + count).val(object.MTilt);
    $('#ETilt-r' + count).val(object.ETilt);
    $('#CellId-r' + count).val(object.CellId);
    $('#PCI-r' + count).val(object.PCI);
    $('#RFHeight-r' + count).val(object.RFHeight);
    $('#Client').val(object.ClientId);
    $('#networkMode-r' + count).val(object.NetworkModeId);
    LoadBands('r' + count, object.NetworkModeId);
    $('#Band-r' + count).val(object.BandId);
    LoadCarriers('r' + count, object.BandId)
    $('#Carrier-r' + count).val(object.CarrierId);

    //Band-r1
    $('#NewDeviceRow').remove();


    count++;
}

function SiteExistOrNot(SiteCode, NetworkModeId, BandId, CarrierId, ScopeId) {
  
    if (SiteCode == "" || SiteCode == null) {
        return false;
    }
    var data = { 'SiteCode': SiteCode, 'NetworkModeId': NetworkModeId, 'BandId': BandId, 'CarrierId': CarrierId, 'ScopeId': ScopeId }
    ajax('/WorkOrder/SiteExistOrNot', data, 'post', function (res) {
        var data = res;
        if (data.length > 0) {
                      var html="Site Id Already Exist,";
                      for (var i = 0; i < data.length; i++) {
                          var obj = data[i];
                      html +=obj+",";

                      }
                      $('#lblSiteCode').html(html);
                      $('#btn-submit').hide();
                      ShowSaveBtn = 1;
                  }
                  else {
                      $('#lblSiteCode').html("");
                      $('#btn-submit').show();
                      ShowSaveBtn = 0;
                  }
        //if (res == 'True') {
        //    $.notify("Site Code & Network Layer Already Exists!", { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
        
        //} else {
          
        //}
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
  
}

function LoadBands(row, value) {
    $('#Band-' + row).empty();
    $('#Band-' + row).append(' <option  value="0">-Band-</option></select>');
    $.each(Bands, function (i, v) {
        if (v.PDefinationId == value) {

            $('#Band-' + row).append(' <option value=' + v.DefinationId + '>' + v.DefinationName + '</option></select>');
        }
    });
}

function LoadMarketes(row, value) {
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


function TssNewRow() {
    var tempCount = parseInt(TssCount) - 1;
    var RowsFields = (TssNewDeviceRow.match(new RegExp("r" + tempCount, "g")) || []).length;
    for (var i = 0; i < RowsFields; i++) {
        TssNewDeviceRow = TssNewDeviceRow.replace("r" + tempCount, "r" + TssCount + "");
    }
    $('#tbl-tss').append('<tr class="NewRow">' + TssNewDeviceRow + '</tr>');
    $('.TssRowFirst').last().focus();
    TssCount++;
}
function ClusterNewRow() {
    return false;
    RowCounter++;
    var tempCount = parseInt(ClusterCount) - 1;
    var RowsFields = (ClusterNewDeviceRow.match(new RegExp("r" + tempCount, "g")) || []).length;
    for (var i = 0; i < RowsFields; i++) {
        ClusterNewDeviceRow = ClusterNewDeviceRow.replace("r" + tempCount, "r" + ClusterCount + "");
    }
    $('#tbl-cluster').append('<tr class="ClusterNewDeviceRow">' + ClusterNewDeviceRow + '</tr>');
    $('.ClusterRowFirst').last().focus();
    ClusterCount++;
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


    $('#pan-sector').hide();
    $('#pan-TSS').hide();
    $('#pan-Cluster').hide();
    // $(".select2").select2();

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
   
   
    btn_AddMarketSites.click(function () {
        $.ajax({
            url: '/workorder/UniqueMarketSiteCodes',
            success: function (res) {

                if (res != 'session expired') {
                    modal_title.text('Add Market Site');
                    modal_content.css({ 'width': '400px' });
                    modal_body.html(res);
                    modal.modal('show');
                    modal_footer.hide();
                    $(".select2").select2();
                } else {
                    $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                }
            },
            error: function (err) {
                $.notify(err.statusText, { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6 });
            },
        });



    });

    $('#btn-upload').click(function () {
        var html = $('#pan-upload').html();
        modal_title.text('Upload Market Sites');
        modal_content.css({ 'width': '400px' });
        modal_body.html(html);
        modal.modal('show');
        modal_footer.hide();
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



    $('#frm-FileUpload').submit(function () {

        loading.open();

    });


    //  $.notify('tedfadfa', { type: 'success', color: "#ffffff", background: "#20D67B", blur: 0.6,delay : 0, });


    //
    $(document).on('keypress', '.siteCode', function (e) {
        if (e.which === 95)
            return false;
        else if (e.which === 32)
            return false;
    });

    $(document).on('blur', '.siteCode', function (e) {
        SiteCode = $(this).val();
        //SiteExistOrNot(SiteCode, SelectednetworkMode, SelectedBand, SelectedCarrier, SelectedScope);
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



    $(document).on('change', '.Client', function () {
       SelectedClient = $('#Client').val();
    });



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
        //SiteExistOrNot(SiteCode, SelectednetworkMode, SelectedBand, SelectedCarrier, SelectedScope);
    });

    $(document).on('change', '.Band', function () {
        SelectedBand = $(this).val();
        var row = $(this).attr('data-row');
        LoadCarriers(row, SelectedBand);
        SelectedCarrier = $('#Carrier-' + row).val();
        //SiteExistOrNot(SiteCode, SelectednetworkMode, SelectedBand, SelectedCarrier, SelectedScope);
    });



    $(document).on('change', '.Carrier', function () {
        SelectedCarrier = $(this).val();
        
    });


    $(document).on('change', '.required', function () {
        $(this).removeClass('required');
    });
    //new row
    
    $(document).on('keydown', '.RowLast', function (e) {
        var keyCode = e.keyCode || e.which;

        if (keyCode == 9) {
            e.preventDefault();
            var $row = $(this).closest("tr");
            OnTabNewRow($row);
            SectorLatLng();
            
        }
    });


    $(document).on('keydown', '.TssRowLast', function (e) {

        var keyCode = e.keyCode || e.which;

        if (keyCode == 9) {
            e.preventDefault();

            //var $row = $(this).closest("tr");

            //var data = {};
            //$("td .form-control", $row).each(function (j) {
            //    var name = $(this).attr('data-name');
            //    var value = $(this).val();
            //    data[name] = value;

            //});
            //RowsData.push(data);
            TssNewRow();
        }
    });
    //cluster keydown
    $(document).on('keydown', '.ClusterRowLast', function (e) {

        var keyCode = e.keyCode || e.which;

        if (keyCode == 9) {
            e.preventDefault();

            ClusterNewRow();
        }
    });

    // delete row
    $(document).on('keydown', '.Row', function (e) {
        var keyCode = e.keyCode || e.which;
       

        if (e.keyCode == 46) {
            e.preventDefault();
            // $('.newrow').last().remove();
            $(this).closest('tr').remove();
            $('.RowLast').last().focus();
        }

    });

    $(document).on('submit', '#frm-wo', function () {
        var hit = 0;
        var Scope = $('#Scope :selected').text();
        var pciresult;
        
        
        
        
        $('#pan-SiteInfo select').each(function () {
            var value = $.trim($(this).val())
            var required = $(this).attr('data-required');
            if (required != 'no' || required != undefined) {
                if (value == "0" || value == '' || value == null) {
                    $(this).addClass('required');
                    hit = 1;
                    $.notify('Select ' + $(this).attr('data-name'), { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                    return false;
                } else {
                    $(this).removeClass('required');
                }
            }
       });
        if (hit == 0) {
            $('#pan-SiteInfo input').each(function (e, i) {
                var textBox = $.trim($(this).val())
                var picValue = $(this).val()
                var required = $(this).attr('data-required');
                if (required != 'no') {
                    if (textBox == "") {
                        if ($(this).attr('type') != "hidden" && $(this).attr('type') != undefined) {

                            if ($('Select#Scope option:selected').text() == 'CLS') {
                                console.log($('Select#Scope option:selected').text());
                                if (e!= 6 && e!= 7 && e!= 8) {
                                    $(this).addClass('required');
                                    hit = 1;
                                    $.notify('Enter Cluster Id', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                                    console.log($('Select#Scope option:selected').text()+'|'+e + '|' + $(this)[0].nodeName);
                                    return false;
                                }
                            }
                            else {
                                console.log($('Select#Scope option:selected').text());
                                $(this).addClass('required');
                                hit = 1;
                                $.notify('Enter Cluster Id', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                                console.log(e + '|' + $(this)[0].nodeName);
                                return false;

                            }
                                
                            
                            
                        }
                        
                    } else {
                        $(this).removeClass('required');
                    }
                }
            });

        }
        if (hit == 0) {
            if(Scope=="CLS")
            {
                $('#pan-Cluster input').each(function () {
                    var textBox = $.trim($(this).val())
                    var required = $(this).attr('data-required');
                    if (required != 'no') {
                        if (textBox == "") {
                            $(this).addClass('required');
                            hit = 1;
                            $.notify('Enter Required Fields', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            return false;
                        } else {
                            $(this).removeClass('required');
                        }
                    }
                });
                
            }
        }
        if (hit == 0) {
            if (Scope == "CLS") {
                $('#pan-Cluster select').each(function () {
                    var textBox = $.trim($(this).val())
                    var required = $(this).attr('data-required');
                    if (required != 'no') {
                        if (textBox == "") {
                            $(this).addClass('required');

                            hit = 1;
                          $.notify('Select Network Mode', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            return false;
                        } else {
                            $(this).removeClass('required');
                        }
                    }
                });
            }
        }
        if (hit == 0) {
            if (Scope == "TSS") {
             
                var textBox = $.trim($("#hfSurveyId").val())
                    var required = $(this).attr('data-required');
                    if (required != 'no') {
                        if (textBox == "") {
                            $("#SurveyId-r0").addClass('required');
                            $("#SurveyId-r0").prop('required', true);
                            hit = 1;
                            $.notify('Select Survey', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            return false;
                        } else {
                            $(this).removeClass('required');
                        }
                    }
            } else {
                $("#SurveyId-r0").addClass('required');
                $("#SurveyId-r0").prop('required', false);
            }
        }


        if (hit == 0) {
            if (Scope == 'SSV' || Scope == 'NI' || Scope == 'IND') {
                $('#pan-sector select').each(function () {
                    var value = $.trim($(this).val())
                    var required = $(this).attr('data-required');
                    if (required != 'no' || required != undefined) {
                        if (value == "0" || value == '' || value == null) {
                            $(this).addClass('required');
                            hit = 1;
                            $.notify('Select ' + $(this).attr('data-name'), { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            return false;
                        } else {
                            $(this).removeClass('required');
                        }
                    }
                });
                
                $('#tbl-wo tbody tr').each(function () {
                    debugger;
                    var pciInput = $(this).find("input[name='PCI']");
                    var pcivalue = pciInput.val();
                    var networkmodetext = $(this).find(".networkMode option:selected").val();

                    if (networkmodetext == "14") {
                        if (pcivalue < 0 || pcivalue > 1008) {
                            pciInput.addClass('required');
                            alert("Please Enter PCI Value 0 to 1008");
                            return pciresult = false;
                        }
                        else {
                            pciInput.removeClass('required');
                            return pciresult = true;
                        }
                    }
                    else if (networkmodetext == "15") {
                        if (pcivalue < 0 || pcivalue > 503) {
                            alert("Please Enter Value 0 to 503");
                            pciInput.addClass('required');
                            return pciresult = false;
                        }
                        else {
                            pciInput.removeClass('required');
                            return pciresult = true;
                        }
                    }
                    else if (networkmodetext == "7") {
                        if (pcivalue < 0 || pcivalue > 502 && pcivalue < 612 || pcivalue > 1500) {
                            alert("Please Enter Value 0 to 503 or 612 to 1500");
                            pciInput.addClass('required');
                            return pciresult = false;
                        }
                        else {
                            pciInput.removeClass('required');
                            return pciresult = true;
                        }
                    }
                    else {
                        if (pcivalue < 0 || pcivalue > 503) {
                            alert("Please Enter Value 0 to 503");
                            pciInput.addClass('required');
                            return pciresult = false;
                        } else {
                            pciInput.removeClass('required');
                            return pciresult = true;
                        }
                    }



                });
                $('#pan-sector Input').each(function () {
                       
                    var value = $.trim($(this).val())
                    var dn=$(this).attr('data-name');
                    if (dn == "Azimuth" || dn == "BeamWidth" || dn == "Antenna" || dn == "CellId" || dn == "ETilt" || dn == "MTilt" || dn == "RFHeight") {
                        $(this).removeClass('required');
                    }
                    else {
                    var required = $(this).attr('data-required');
                    if (required != 'no' || required != undefined) {
                        if (value == "0" || value == '' || value == null) {
                            if (dn == "BandWidth" || dn == "MRBTS") {
                                if (value >= "0") {
                                    $(this).removeClass('required');
                                    return true;
                                }
                                

                            }
                            
                            $(this).addClass('required');
                            hit = 1;
                            $.notify('Input ' + $(this).attr('data-name'), { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                            return false;
                        } else {
                            $(this).removeClass('required');
                        }
                    
                    }
                    }
                });
               
            }
        }

        //if (hit == 0) {
        //    if (Scope == 'SSV' || Scope == 'NI' || Scope == 'IND') {
        //        $('#pan-sector Input').each(function () {
        //            var value = $.trim($(this).val())
        //            var required = $(this).attr('data-required');
        //            if (required != 'no' || required != undefined) {
        //                if (value == "0" || value == '' || value == null) {
        //                    $(this).addClass('required');
        //                    hit = 1;
        //                    $.notify('Select ' + $(this).attr('data-name'), { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
        //                    return false;
        //                } else {
        //                    $(this).removeClass('required');
        //                }
        //            }
        //        });


        //    }
        //}
        if (hit == 0) {
            $('#ProjectId').attr("disabled", false);
            var Form = '';
            if (Scope == 'SSV' || Scope == 'NI' || Scope == 'IND') {
                Form = '#tbl-wo tr';
            } else if (Scope == 'TSS') {
                Form = '#tbl-tss tr';
            }
            else if (Scope == 'CLS') {
                
                
                Form = '#tbl-cluster tr';
            }
            var DataObjects = [];
            $(Form).each(function (i) {
                
                var abc = "";
                var def = "";
                   if (i > 0 && Scope == 'CLS')
                   {
                       var selected = $(this).find(".networkmodefake option:selected");
                       selected.each(function () {
                           var value = $(this).val();
                           var sname = $(this).text();
                         //  console.log(sname);
                           if (abc == "") {
                               def = sname;
                               abc = value;
                           }
                           else {
                               def = def + "_" + sname;
                               abc = abc + "," + value;
                           }
                       });
                       //var hjk = $(".filer");
                       //if(hjk.length >0)
                       //{
                       //    var fname = hjk[i - 1].files[0].name.split(".")[0];
                       //}
                      
                   }
                // var abc = $(this).find("#networkmodefake").find(":selected").text();
                   i = i - 1;
                   if (Scope == 'CLS') {
                       var opq = $(this).find("#networkmode-r" + i);
                      $(this).find("#networkmode-r" + i).val(abc);
                       $(this).find("#networkmodename-r" + i).val(def);
                       //if(hjk.length >0)
                       //{
                         //  $(this).find("#filename-r" + i).val(fname);
                      //}
                   }

                $("td input", this).each(function (j) {
                    
                    var name = $(this).attr('data-name');
                  
                    $(this).attr('name', '[' + i + '].' + name);
                });
                $("td select", this).each(function (j) {
                    var name = $(this).attr('data-name');
                    $(this).attr('name', '[' + i + '].' + name);
                });
            });
            
            debugger;
            if (pciresult || pciresult == undefined) {
                $.ajax({
                    url: '/workorder/New',
                    type: 'post',
                    async: false,
                    data: $(this).serialize(),
                    success: function (response) {
                        var res = response.response
                        //  console.log(res.Status,res);
                        if (res != 'session expired') {
                            if (res.Status == 'success') {
                                if (Scope == 'CLS') {
                                    for (var i = 0; i <= RowCounter; i++) {
                                        var file = $('#clusterfile-r' + i)[0];
                                        formdata.append('#clusterfile-r' + i, file.files[0]);
                                    }
                                    formdata.append('mydata', $(this).serialize());
                                    if (file.files.length > 0) {
                                        $.ajax({
                                            url: '/workorder/NewFiles',
                                            type: 'POST',
                                            data: formdata,
                                            contentType: false,
                                            processData: false,
                                            success: function (d) {
                                                $("#reset").click();
                                                //  location.reload();
                                                formdata = new FormData();
                                                RowCounter = 0
                                                $.notify("Save successfully !", { type: "success", color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                                                setTimeout(function () { location.reload(); }, 1000);
                                            },
                                            error: function () {
                                                $.notify("Error occured !", { type: "error", color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });

                                            }
                                        });
                                    }
                                    else {
                                        formdata = new FormData();
                                        RowCounter = 0
                                        $.notify("Save successfully !", { type: "success", color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                                        setTimeout(function () { location.reload(); }, 1000);

                                    }

                                }
                                else {
                                    $.notify("Save successfully !", { type: "success", color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                                    setTimeout(function () { location.reload(); }, 1000);

                                    formdata = new FormData();
                                    RowCounter = 0
                                }
                            } else {
                                $.notify("Save successfully !", { type: "success", color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                                setTimeout(function () { location.reload(); }, 1000);

                                formdata = new FormData();
                                RowCounter = 0
                            }
                        } else {
                            $.notify('Session Expired. Open New Tab & Login Again. Then Press Save Button.', { type: 'danger', color: "#ffffff", background: "#D44950", blur: 0.6, delay: 0, });
                        }
                        //  alert(res);
                    },
                    error: function (response) {
                        $.notify("Save successfully !", { type: "success", color: "#ffffff", background: "#20D67B", blur: 0.6, delay: 0, });
                        setTimeout(function () { location.reload(); }, 1000);

                        formdata = new FormData();
                        RowCounter = 0
                    },

                });
            }
            
            return false;
        }

        
        return false;
    });

  
   

    $(document).on('click', '.multiselect', function () {
     
var jk = $(this).parent().hasClass("open");
        if(jk == false)
        {
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

});
