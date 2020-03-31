
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
    var btn_Pcis = $('.btn-Pcis');
    var btn_RemoveDrive = $('.btn-RemoveDrive');
    var ForMail = $('.ForMail');
    var doc = $(document);




    ForMail.hide();
    // console.log(NetLayerReport);
    //  console.log(ReportConfiguration);

    $.each(ReportConfiguration, function (i, c) {


        var DataTypes = $('.' + c.KeyCode).attr('data-type');

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
                    // this function in _ntlFieldTest.cshtml

                    //     console.log(c.KeyCode + '-' + types[i] + '-' + c.KeyValue + '-' + c.isActive);
                    Configurations1(c.KeyCode, types[i], c.KeyValue, c.isActive);
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

    btn_Pcis.click(function () {
        debugger;
        var type = $(this).attr('data-value');
        var Pcis = '';
        $("input:checkbox.ch-pci:checked").each(function () {
            Pcis = Pcis + $(this).attr("data-value") + ',';
        });

        $.ajax({
            url: '/Site/DisablePcis',
            type: 'post',
            data: { 'filter': 'DisablePcis', 'SiteId': SiteId, 'NetworkModeId': NetworkModeId, 'BandId': BandId, 'CarrierId': CarrierId, 'ScopeId': ScopeId, 'Status': type, 'Pcis': Pcis },
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
        $('input:checkbox.checkboxcontroll').each(function (i,e) {
            KeyCode = $(e).attr('data-keycode');

            if ($(e).is(":checked")) {
                // this function in _ntlFieldTest.cshtml
                Configurations1(KeyCode, 'BOOLEAN', '', true);
            }
            else if ($(e).is(":not(:checked)")) {
                // this function in _ntlFieldTest.cshtml
                Configurations1(KeyCode, 'BOOLEAN', '', false);
            }
        });


        $('input:text.panelItem').each(function (i,e) {
            KeyCode = $(this).attr('data-keycode');
            var value = $(this).val();
            // this function in _ntlFieldTest.cshtml
            Configurations1(KeyCode, 'STRING', value, false);
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

        setTimeout(function () {
            window.print();

            btn_print.show();
            btn_setting.show();
            //  pan_settings.show();
            frm_pci.show();
            frm_cw.show();
            frm_ccw.show();
            frm_rsrp.show();
            frm_rsrq.show();
            frm_cinr.show();
            frm_cwccw.show();
            frm_chPlot.show();
        }, 2000);




        return false;
    });




    //  console.log(NetLayerReport);

   // initializeHybrid('site_data_map', SiteLatitude, SiteLongitude, SiteCode, null, null, NetLayerReport.Azmiuth, AzmuthRadius);

    domain = document.location.origin;
    var DomainPath = domain + '/Content/AirViewLogs/' + ClientPrefix + '/' + NetLayer;

    //doc.on('click', '#btn-cwccw', function () {
    //    if (AfterDate == 'False') {
    //        initialize('HO_Plot_CwCcw', SiteLatitude, SiteLongitude, SiteCode, NetLayerReport.CwCcw_Marker, NetLayerReport.PCI_Circles, NetLayerReport.Azmiuth, $('#cwccw-Radius').val(), $('#cwccw-Azmuth').val());
    //    } else {

    //        var kml = DomainPath + '/ho.kml';
    //        kml = kml.replace(' ', '%20')
    //        initializekml('HO_Plot_CwCcw', SiteLatitude, SiteLongitude, SiteCode, kml, NetLayerReport.Azmiuth, $('#cwccw-Radius').val(), $('#cwccw-Azmuth').val());
    //    }

    //    return false;

    //});
    //doc.on('click', '#btn-pci', function () {

    //    debugger;
    //    if (AfterDate == 'False') {
    //        initialize('PCI_Plot', SiteLatitude, SiteLongitude, SiteCode, null, NetLayerReport.PCI_Circles, NetLayerReport.Azmiuth, $('#pci-Radius').val(), $('#pci-Azmuth').val());
    //    } else {
    //        //  var kml = 'http://96.57.107.148/Content/AirViewLogs/TMO/CH41662A-D1/LTE_LTE%201900_1150/pci.kml';
    //        var kml = DomainPath + '/pci.kml';
    //        kml = kml.replace(' ', '%20')
    //        //console.log(kml);
    //        console.log(NetLayerReport.Azmiuth)
    //        initializekml('PCI_Plot', SiteLatitude, SiteLongitude, SiteCode, kml, NetLayerReport.Azmiuth, $('#pci-Radius').val(), $('#pci-Azmuth').val());
    //    }
    //    return false;
    //});

    //doc.on('click', '#btn-cw', function () {
    //    if (AfterDate == 'False') {
    //        initialize('HO_Plot', SiteLatitude, SiteLongitude, SiteCode, NetLayerReport.CW_Marker, NetLayerReport.CW_Circles, NetLayerReport.Azmiuth, $('#cw-Radius').val(), $('#cw-Azmuth').val(), NetLayerReport.CWDropSignal);
    //    } else {
    //        var kml = DomainPath + '/cw.kml';
    //        kml = kml.replace(' ', '%20')
    //        initializekml('HO_Plot', SiteLatitude, SiteLongitude, SiteCode, kml, NetLayerReport.Azmiuth, $('#cw-Radius').val(), $('#cw-Azmuth').val());
    //    }


    //    return false;
    //});

    //doc.on('click', '#btn-ccw', function () {

    //    if (AfterDate == 'False') {
    //        initialize('HO_Plot_CCW', SiteLatitude, SiteLongitude, SiteCode, NetLayerReport.CCW_Marker, NetLayerReport.CCW_Circles, NetLayerReport.Azmiuth, $('#ccw-Radius').val(), $('#ccw-Azmuth').val())
    //    } else {
    //        var kml = DomainPath + '/ccw.kml';
    //        kml = kml.replace(' ', '%20')
    //        initializekml('HO_Plot_CCW', SiteLatitude, SiteLongitude, SiteCode, kml, NetLayerReport.Azmiuth, $('#ccw-Radius').val(), $('#ccw-Azmuth').val());
    //    }



    //    return false;
    //});
    //doc.on('click', '#btn-rsrp', function () {
    //    if (AfterDate == 'False') {
    //        initialize('RSRP_Plot', SiteLatitude, SiteLongitude, SiteCode, null, NetLayerReport.RsRp_Circles, NetLayerReport.Azmiuth, $('#rsrp-Radius').val(), $('#rsrp-Azmuth').val())
    //    } else {
    //        //  debugger;
    //        var kml = DomainPath + '/Rsrp.kml';
    //        kml = kml.replace(' ', '%20')
    //        //console.log(kml);
    //        initializekml('RSRP_Plot', SiteLatitude, SiteLongitude, SiteCode, kml, NetLayerReport.Azmiuth, $('#rsrp-Radius').val(), $('#rsrp-Azmuth').val());
    //    }


    //    return false;
    //});

    //doc.on('click', '#btn-rsrq', function () {
    //    if (AfterDate == 'False') {
    //        initialize('RSRQ_Plot', SiteLatitude, SiteLongitude, SiteCode, null, NetLayerReport.RSRQ_Circles, NetLayerReport.Azmiuth, $('#rsrq-Radius').val(), $('#rsrq-Azmuth').val())
    //    } else {
    //        var kml = DomainPath + '/Rsrq.kml';
    //        kml = kml.replace(' ', '%20')
    //        initializekml('RSRQ_Plot', SiteLatitude, SiteLongitude, SiteCode, kml, NetLayerReport.Azmiuth, $('#rsrq-Radius').val(), $('#rsrq-Azmuth').val());
    //    }

    //    return false;
    //});

    //doc.on('click', '#btn-cinr', function () {
    //    if (AfterDate == 'False') {
    //        initialize('CINR_Plot', SiteLatitude, SiteLongitude, SiteCode, null, NetLayerReport.CINR_Circles, NetLayerReport.Azmiuth, $('#cinr-Radius').val(), $('#cinr-Azmuth').val());
    //    } else {
    //        var kml = DomainPath + '/Cinr.kml';
    //        kml = kml.replace(' ', '%20')
    //        initializekml('CINR_Plot', SiteLatitude, SiteLongitude, SiteCode, kml, NetLayerReport.Azmiuth, $('#cinr-Radius').val(), $('#cinr-Azmuth').val());
    //    }


    //    return false;
    //});


    //doc.on('click', '#btn-chPlot', function () {
    //    if (AfterDate == 'False') {
    //        initialize('CH_Plot', SiteLatitude, SiteLongitude, SiteCode, null, NetLayerReport.CH_Circles, NetLayerReport.Azmiuth, $('#chPlot-Radius').val(), $('#chPlot-Azmuth').val());
    //    } else {
    //        var kml = DomainPath + '/ch.kml';
    //        kml = kml.replace(' ', '%20')
    //        initializekml('CH_PLOT', SiteLatitude, SiteLongitude, SiteCode, kml, NetLayerReport.Azmiuth, $('#chPlot-Radius').val(), $('#chPlot-Azmuth').val());
    //    }


    //    return false;
    //});


    //if ('@ViewBag.Auto' == "1") {

    //    @*initialize('PCI_Plot', SiteLatitude, SiteLongitude, SiteCode, null, NetLayerReport.PCI_Circles, NetLayerReport.Azmuth, '@ViewBag.CircleRadios', '@ViewBag.AzmuthRadius');
    //    initialize('HO_Plot', SiteLatitude, SiteLongitude, SiteCode, NetLayerReport.CW_Marker, NetLayerReport.CW_Circles, '@ViewBag.CircleRadios', '@ViewBag.AzmuthRadius');*@


    //    $('#frm-pci').hide()
    //    $('#frm-cw').hide()
    //    $('#frm-ccw').hide()
    //    $('#frm-rsrp').hide()
    //    $('#frm-rsrq').hide()
    //    $('#frm-cinr').hide()
    //}
});




//document.onreadystatechange = function () {
//    var state = document.readyState
//    if (state == 'complete') {
//        setTimeout(function () {
//            $('#pdf').loader('hide');
//        }, 1000);
//    }
//}