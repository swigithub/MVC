function Configurations(keyCode, dataType, value, IsActive) {

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
$("document").ready(function () {
    var loading = $.loading();
    var btn_setting = $('#btn-setting');
    var btn_print = $('.print');
    var pan_settings = $('#pan-settings');
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
    var btn_RemoveDrive = $('.btn-RemoveDrive');
    var btn_PORSites = $('#btn-PORSites');
    var ForMail = $('.ForMail');
    var doc = $(document);
    
    ForMail.hide();

    $.each(ReportConfiguration, function (i, c) {

        var DataTypes = $('#' + c.KeyCode).attr('data-type');
        if (DataTypes == undefined) {
            DataTypes = $('.' + c.KeyCode).attr('data-type');
        }
        //   debugger;
        if (DataTypes != undefined) {
            var types = DataTypes.split(",");

            if (types.length > 0) {
                for (var i = 0; i < types.length; i++) {
                    // this function in _ntlFieldTest.cshtml
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

    Array.prototype.removeValue = function (name, value) {
        var array = $.map(this, function (v, i) {
            return v[name] === value ? null : v;
        });
        this.length = 0; //clear original array
        this.push.apply(this, array); //push all elements except the one we want to delete
    }


    $('.Note').html($('#Note').html());

    frm_pcis.submit(function () {

        $("input:checkbox.ch-pci:not(:checked)").each(function () {
            var pci = $(this).attr("data-value");

            $.each(NetLayerReport, function (i, c) {
                NetLayerReport[i].removeValue('PCI', pci);
            });
        });
        return false;
    });


    btn_RemoveDrive.click(function () {
        var type = $(this).attr('data-value');
        var Timestamp = '';
        $("input:checkbox.ch-Timestamp:checked").each(function () {
            Timestamp = Timestamp + $(this).attr("data-value") + ',';
        });
        $.ajax({
            url: '/Site/DisableMapRoot',
            type: 'post',
            data: { 'filter': 'DisableServerTimestamp', 'SiteId': SiteId, 'NetworkModeId': NetworkModeId, 'BandId': BandId, 'CarrierId': CarrierId, 'ScopeId': ScopeId, 'Status': type, 'FromTime': Timestamp, 'ToTime': '' },
            success: function (res) {
                if (res.Status == 'success') {
                    location.reload();
                } else {
                    alert(res.Message);
                }
            },
            error: function (err) { }
        });
        return false;
    });

    $("#ddlFonts").change(function () {
        font.css("font-family", $(this).val());
    });

    $('.datetime').appendDtpicker();

    btn_setting.click(function () {
        pan_settings.fadeToggle("slow");
        return false;
    });

    btn_mapRoot.click(function () {
        var type = btn_mapRoot.attr('data-value');
        $.ajax({
            url: '/Site/DisableMapRoot',
            type: 'post',
            data: { 'filter': 'DisableMapRoot', 'SiteId': SiteId, 'NetworkModeId': NetworkModeId, 'BandId': BandId, 'CarrierId': CarrierId, 'ScopeId': ScopeId, 'Status': type, 'FromTime': txtFromDate.val(), 'ToTime': txtToDate.val() },
            success: function (res) {
                if (res.Status == 'success') {
                    location.reload();
                } else {
                    alert(res.Message);
                }
            },
            error: function (err) { }
        });
        return false;
    });

    btn_cofSave.click(function () {
        var KeyCode;
        $('input:checkbox.checkboxcontroll').each(function () {
            KeyCode = $(this).attr('data-keycode');

            if ($(this).is(":checked")) {
                // this function in _ntlFieldTest.cshtml
                Configurations(KeyCode, 'BOOLEAN', '', true);
            }
            else if ($(this).is(":not(:checked)")) {
                // this function in _ntlFieldTest.cshtml
                Configurations(KeyCode, 'BOOLEAN', '', false);
            }
        });


        $('input:text.panelItem').each(function () {
            KeyCode = $(this).attr('data-keycode');
            var value = $(this).val();
            // this function in _ntlFieldTest.cshtml
            Configurations(KeyCode, 'STRING', value, false);
        });
    });



    btn_print.click(function () {
        btn_setting.hide();
        pan_settings.hide();
        frm_pci.hide();
        frm_cw.hide();
        frm_ccw.hide();
        frm_cwccw.hide();
        frm_rsrp.hide();
        frm_rsrq.hide();
        frm_cinr.hide();
        frm_chPlot.hide();
        btn_print.hide();

        window.print();

        btn_print.show();
        btn_setting.show();
        pan_settings.show();
        frm_pci.show();
        frm_cw.show();
        frm_ccw.show();
        frm_rsrp.show();
        frm_rsrq.show();
        frm_cinr.show();
        frm_cwccw.show();
        frm_chPlot.show();
        return false;
    });

    $('body').keydown(function (e) {
        if (e.keyCode == 17) {
            btn_setting.hide();
            pan_settings.hide();
        }

    });
    $('body').keyup(function (e) {
        if (e.keyCode == 17) {
            btn_setting.show();
        }

    });

    initializeHybrid('site_data_map', SiteLatitude, SiteLongitude, SiteCode, null, null, NetLayerReport.Azmiuth, AzmuthRadius);

    domain = document.location.origin;

    var Hokml = domain + '/Content/AirViewLogs/TMO/' + NetLayer + '/Ho.kml';
    Hokml = Hokml.replace(' ', '%20');
    try {

        initializekml('HO_Plot', SiteLatitude, SiteLongitude, SiteCode, Hokml, NetLayerReport.Azmiuth, $('#pci-Radius').val(), AzmuthRadius);
    } catch (e) {
        initializekml('HO_Plot', SiteLatitude, SiteLongitude, SiteCode, Hokml, NetLayerReport.Azmiuth, $('#pci-Radius').val(), AzmuthRadius);
    }


    var Pcikml = domain + '/Content/AirViewLogs/TMO/' + NetLayer + '/pci.kml';
    Pcikml = Pcikml.replace(' ', '%20');
    try {

        initializekml('PCI_Plot', SiteLatitude, SiteLongitude, SiteCode, Pcikml, NetLayerReport.Azmiuth, $('#pci-Radius').val(), AzmuthRadius);
    } catch (e) {
        initializekml('PCI_Plot', SiteLatitude, SiteLongitude, SiteCode, Pcikml, NetLayerReport.Azmiuth, $('#pci-Radius').val(), AzmuthRadius);
    }

    var Chkml = domain + '/Content/AirViewLogs/TMO/' + NetLayer + '/ch.kml';
    Chkml = Chkml.replace(' ', '%20')
    try {

        initializekml('CH_Plot', SiteLatitude, SiteLongitude, SiteCode, Chkml, NetLayerReport.Azmiuth, $('#chPlot-Radius').val(), AzmuthRadius);
    } catch (e) {
        initializekml('CH_Plot', SiteLatitude, SiteLongitude, SiteCode, Chkml, NetLayerReport.Azmiuth, $('#chPlot-Radius').val(), AzmuthRadius);
    }

    var Rsrpkml = domain + '/Content/AirViewLogs/TMO/' + NetLayer + '/Rsrp.kml';
    Rsrpkml = Rsrpkml.replace(' ', '%20')
    try {

        initializekml('RSRP_Plot', SiteLatitude, SiteLongitude, SiteCode, Rsrpkml, NetLayerReport.Azmiuth, $('#rsrp-Radius').val(), AzmuthRadius);
    } catch (e) {
        initializekml('RSRP_Plot', SiteLatitude, SiteLongitude, SiteCode, Rsrpkml, NetLayerReport.Azmiuth, $('#rsrp-Radius').val(), AzmuthRadius);
    }

    try {
        var CINRkml = domain + '/Content/AirViewLogs/TMO/' + NetLayer + '/Cinr.kml';
        CINRkml = CINRkml.replace(' ', '%20')
        initializekml('CINR_Plot', SiteLatitude, SiteLongitude, SiteCode, CINRkml, NetLayerReport.Azmiuth, $('#cinr-Radius').val(), AzmuthRadius);
    } catch (e) {

    }

    var Rsrqkml = domain + '/Content/AirViewLogs/TMO/' + NetLayer + '/Rsrq.kml';
    Rsrqkml = Rsrqkml.replace(' ', '%20')
    try {

        initializekml('RSRQ_Plot', SiteLatitude, SiteLongitude, SiteCode, Rsrqkml, NetLayerReport.Azmiuth, $('#rsrq-Radius').val(), AzmuthRadius);
    } catch (e) {

    }
    try {
        initializekml('SummeryPlot', SiteLatitude, SiteLongitude, SiteCode, Rsrpkml, NetLayerReport.Azmiuth, $('#rsrp-Radius').val(), AzmuthRadius);

    } catch (e) {

    }

    $('#tbl-summary').hide();
    btn_PORSites.click(function () {
        var type = btn_mapRoot.attr('data-value');
        var Sites = [];
        $('#tbl-summary tbody').empty();
        //console.log(PORSites);
        $("input:checkbox.ch-Timestamp:checked").each(function () {

            var lat = $(this).attr("data-lat");
            var lng = $(this).attr("data-lng");
            var site = $(this).attr("data-Site");
            var obj = { lat, lng };
            Sites.push(obj);
            $.each(PORSites, function (i, v) {
                if (v.SiteCode == site) {
                    $('#tbl-summary').show();
                    v.SiteType = (v.SiteType != null) ? v.SiteType : '';
                    v.PORCheck = (v.PORCheck != null) ? v.PORCheck : '';
                    v.SiteCode = (v.SiteCode != null) ? v.SiteCode : '';
                    v.RingId = (v.RingId != null) ? v.RingId : '';
                    v.POR = (v.POR != null) ? v.POR : '';
                    v.BandCapable = (v.BandCapable != null) ? v.BandCapable : '';
                    v.BandPOR = (v.BandPOR != null) ? v.BandPOR : '';
                    v.BandOA = (v.BandOA != null) ? v.BandOA : '';
                    v.Comments = (v.Comments != null) ? v.Comments : '';
                    $('#tbl-summary').append('<tr style="background-color:#E7F5FF;font-size:small"><td>' + v.SiteType + '</td><td>' + v.PORCheck + '</td><td>' + v.SiteCode + '</td><td>' + v.RingId + '</td><td>' + v.POR + '</td>' +
                            +'<td>' + v.BandCapable + '</td><td>' + v.BandPOR + '</td><td>' + v.BandOA + '</td><td>' + v.Comments + '</td><td>' + v.Comments + '</td></tr>');
                }
            });


        });

        $('#SummeryPlot').empty();
        try {
            var Rsrpkml1 = domain + '/Content/AirViewLogs/TMO/' + NetLayer + '/Rsrp.kml';
            Rsrpkml1 = Rsrpkml1.replace(' ', '%20')
            initializekml('SummeryPlot', SiteLatitude, SiteLongitude, SiteCode, Rsrpkml1, NetLayerReport.Azmiuth, $('#rsrp-Radius').val(), AzmuthRadius, Sites);
        } catch (e) {

        }
        for (var i = 0; i < Sites.length; i++) {
            latlng = new google.maps.LatLng(Sites[i].lat, Sites[i].lng);
            MapCircle(latlng, LastMap, parseInt(AzmuthRadius) + 50, "#0000ff");
        }
        return false;
    });



    var scaleInterval = setInterval(function () {
        var scale = $(".gm-style-cc:not(.gmnoprint):contains(' km')") || $(".gm-style-cc:not(.gmnoprint):contains(' m')");
        if (scale.length) {
            scale.click();
            //console.log(scale.text());
            clearInterval(scaleInterval);
        }
    }, 100);
});
