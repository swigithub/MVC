
$("document").ready(function () {
    var loading = $.loading();
    var btn_print = $('.print');
    var pan_settings = $('#pan-settings');
    var btn_setting = $('#btn-setting');
    var frm_pci = $('#frm-pci');
    var frm_cw = $('#frm-cw');
    var frm_ccw = $('#frm-ccw');
    var frm_rsrp = $('#frm-rsrp');
    var frm_cwccw = $('#frm-cwccw');
    var frm_rsrq = $('#frm-rsrq');
    var frm_cinr = $('#frm-cinr');
    var frm_chPlot = $('#frm-chPlot');
    var btn_cofSave = $('#btn-cofSave');
    var font = $('.font');
    var btn_mapRoot = $('.btn-mapRoot');
    var txtFromDate = $('#txtFromDate');
    var txtToDate = $('#txtToDate');
    var frm_pcis = $('#frm-pcis');
    var btn_Pcis = $('.btn-Pcis');
    var btn_RemoveDrive = $('.btn-RemoveDrive');
    var ForMail = $('.ForMail');
    var doc = $(document);

    $("#ddlFonts").change(function () {
        font.css("font-family", $(this).val());
    });

    $('.datetime').appendDtpicker();

    btn_setting.click(function () {
        pan_settings.fadeToggle("slow");
        return false;
    });
    btn_print.click(function () {
        //btn_setting.hide();
        //pan_settings.hide();
        frm_pci.hide();
        frm_cw.hide();
        frm_ccw.hide();
        frm_cwccw.hide();
        frm_rsrp.hide();
        frm_rsrq.hide();
        frm_cinr.hide();
        frm_chPlot.hide();
        btn_print.hide();
        $('#btn-setting').hide();
        $('#pan-settings').hide();

        window.print();

        btn_print.show();
        //btn_setting.show();
        //pan_settings.show();
        frm_pci.show();
        frm_cw.show();
        frm_ccw.show();
        frm_rsrp.show();
        frm_rsrq.show();
        frm_cinr.show();
        frm_cwccw.show();
        frm_chPlot.show();
        $('#btn-setting').show();
        $('#pan-settings').show();
        return false;
    });


    function Configurations(keyCode, dataType, value, IsActive) {
        // console.log(keyCode + '-' + dataType + '-' + value + '-' + IsActive);
        if (dataType == 'BOOLEAN') {
            if (IsActive == true) {
                $('#' + keyCode).show();
                $('.' + keyCode).show();
                $('#pan-' + keyCode).show();
            } else {
                $('#' + keyCode).hide();
                $('.' + keyCode).hide();
                $('#pan-' + keyCode).hide();
            }
        }

        //if (dataType == 'BGCOLOR') {
        //    $('.' + c.KeyCode).css({
        //        'background': c.value
        //    });
        //}

        if (dataType == 'STRING') {
            if (value != null || value != '') {
                var preValue = $('#' + keyCode).attr('data-preValue');
                if (preValue != undefined) {
                    $('#' + keyCode).text(preValue + ' ' + value);
                } else {
                    $('#' + keyCode).text(value);
                }

                $('.' + keyCode).text(value);
            }
        }
    }


    $.each(ReportConfiguration, function (i, c) {


        var DataTypes = $('#' + c.KeyCode).attr('data-type');

        //  console.log(c.KeyCode);
        //  console.log(DataTypes);
        // console.log('-----------');

        if (DataTypes == undefined) {
            DataTypes = $('.' + c.KeyCode).attr('data-type');
        }
        //   debugger;
        if (DataTypes != undefined) {
            var types = DataTypes.split(",");

            if (types.length > 0) {
                for (var i = 0; i < types.length; i++) {
                    //     console.log(c.KeyCode + '-' + types[i] + '-' + c.KeyValue + '-' + c.isActive);
                    Configurations(c.KeyCode, types[i], c.KeyValue, c.isActive);
                    if (c.fontColor != null || c.fontColor != '') {
                        $('.' + c.KeyCode).css({
                            'color': c.fontColor
                        });
                    }
                }
            }
        }

    });

    domain = document.location.origin;



});




