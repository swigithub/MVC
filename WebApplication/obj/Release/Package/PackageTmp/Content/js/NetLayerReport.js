// =================  Global Reporting variables =====================
var imgwlcmdata = "";
var imgfooterdata = "";
var imgsitedatatbl = "";
var imgsitedatamap = "";
var imgfiltest5g = "";
var imgsummaryrslt = "";
var imgcrplot = "";
var imgpciplot = "";
var imgpciLTEplot = "";
var imgdlplot = "";
var imgcwhoplot = "";
var imgccwhoplot = "";
var imghoplot = "";
var imgdrpeventplot = "";
var imgrsrpplot = "";
var imgrsrqplot = "";
var imgcinrplot = "";
var imgcwhoplotmap = "";
var imgccwhoplotmap = "";
var imgthanks_footer = "";
var imgnrspdtest = "";
var imgltespdtest = "";
var imglocimages = "";

// =================  Global Reporting variables =====================

function GetPreDefinedImages() {
    
}

$("document").ready(function () {
    var loading = $.loading();
    var btn_setting = $('#btn-setting');
    var btn_print = $('.print');
    var pan_settings = $('#pan-settings');
    var frm_pci = $('#frm-pci');
    var frm_pciLTE = $('#frm-pciLTE');
    var frm_dl = $('#frm-dl');
    var frm_cw = $('#frm-cw');
    var frm_ccw = $('#frm-ccw');
    var frm_rsrp = $('#frm-rsrp');
    var frm_cwccw = $('#frm-cwccw');
    var frm_rsrq = $('#frm-rsrq');
    var frm_cinr = $('#frm-cinr');
    var frm_chPlot = $('#frm-chPlot');
    var frm_beamGroup = $("#frm-beamGroup");
    var frm_beam = $("#frm-beam");
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


        var DataTypes = $('#' + c.KeyCode).attr('data-type');

        //  console.log(c.KeyCode);
        //  console.log(DataTypes);

        // console.log('-----------');

        if (DataTypes == undefined) {
            DataTypes = $('.' + c.KeyCode).attr('data-type');
        }
        if (DataTypes != undefined) {
            var types = DataTypes.split(",");

            if (types.length > 0) {
                for (var i = 0; i < types.length; i++) {
                    // this function in _ntlFieldTest.cshtml

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
        frm_pciLTE.hide();
        frm_dl.hide();
        frm_cw.hide();
        frm_ccw.hide();
        frm_cwccw.hide();
        frm_rsrp.hide();
        frm_rsrq.hide();
        frm_cinr.hide();
        frm_chPlot.hide();
        frm_beam.hide();
        frm_beamGroup.hide();
        btn_print.hide();
        setTimeout(function () {
            window.print();

            btn_print.show();
            btn_setting.show();
            //  pan_settings.show();
            frm_pci.show();
            frm_pciLTE.show();
            frm_dl.show();
            frm_cw.show();
            frm_ccw.show();
            frm_rsrp.show();
            frm_rsrq.show();
            frm_cinr.show();
            frm_cwccw.show();
            frm_chPlot.show();
            frm_beam.show();
            frm_beamGroup.show();

        }, 2000);




        return false;
    });




    //  console.log(NetLayerReport);

    initializeHybrid('site_data_map', SiteLatitude, SiteLongitude, SiteCodes, null, null, NetLayerReport.Azmiuth, AzmuthRadius);
    //initialize('release_map', SiteLatitude, SiteLongitude, SiteCodes, null, null, NetLayerReport.Azmiuth, AzmuthRadius);
    LoadMaps();
    domain = document.location.origin;
    var DomainPath = domain + '/Content/AirViewLogs/' + ClientPrefix + '/' + NetLayer;

    doc.on('click', '#btn-cwccw', function () {
        if (AfterDate == 'False') {
            initialize('HO_Plot_CwCcw', SiteLatitude, SiteLongitude, SiteCodes, NetLayerReport.CwCcw_Marker, NetLayerReport.PCI_Circles, NetLayerReport.Azmiuth, $('#cwccw-Radius').val(), $('#cwccw-Azmuth').val());
        } else {

            var kml = DomainPath + '/ho.kml';
            kml = kml.replace(' ', '%20')
            initializekml('HO_Plot_CwCcw', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#cwccw-Radius').val(), $('#cwccw-Azmuth').val());
        }

        return false;

    });
    doc.on('click', '#btn-pci', function () {

        if (AfterDate == 'False') {
            initialize('PCI_Plot', SiteLatitude, SiteLongitude, SiteCodes, null, NetLayerReport.PCI_Circles, NetLayerReport.Azmiuth, $('#pci-Radius').val(), $('#pci-Azmuth').val());
        } else {
            //  var kml = 'http://96.57.107.148/Content/AirViewLogs/TMO/CH41662A-D1/LTE_LTE%201900_1150/pci.kml';
            var kml = DomainPath + '/pci.kml';
            kml = kml.replace(' ', '%20')
            //console.log(kml);
            console.log(NetLayerReport.Azmiuth)
            initializekml('PCI_Plot', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#pci-Radius').val(), $('#pci-Azmuth').val());
        }
        return false;
    });
    doc.on('click', '#btn-pciLTE', function () {
        debugger
        if (AfterDate == 'False') {
            initialize('PCI_PlotLTE', SiteLatitude, SiteLongitude, SiteCodes, null, NetLayerReport.PCI_Circles, NetLayerReport.Azmiuth, $('#pciLTE-Radius').val(), $('#pciLTE-Azmuth').val());
        } else {
            //  var kml = 'http://96.57.107.148/Content/AirViewLogs/TMO/CH41662A-D1/LTE_LTE%201900_1150/pci.kml';
            var kml = DomainPath + '/pciLTE.kml';
            kml = kml.replace(' ', '%20')
            //console.log(kml);
            console.log(NetLayerReport.Azmiuth)
            initializekml('PCI_PlotLTE', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#pciLTE-Radius').val(), $('#pciLTE-Azmuth').val());
        }
        return false;
    });
    doc.on('click', '#btn-dl', function () {
        debugger
        if (AfterDate == 'False') {
            initialize('DL_PLOT', SiteLatitude, SiteLongitude, SiteCodes, null, NetLayerReport.PCI_Circles, NetLayerReport.Azmiuth, $('#dl-Radius').val(), $('#dl-Azmuth').val());
        } else {
            //  var kml = 'http://96.57.107.148/Content/AirViewLogs/TMO/CH41662A-D1/LTE_LTE%201900_1150/pci.kml';
            var kml = DomainPath + '/dl.kml';
            kml = kml.replace(' ', '%20')
            //console.log(kml);
            console.log(NetLayerReport.Azmiuth)
            initializekml('DL_PLOT', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#dl-Radius').val(), $('#dl-Azmuth').val());
        }
        return false;
    });
    doc.on('click', '#btn-cw', function () {
        if (AfterDate == 'False') {
            initialize('HO_Plot', SiteLatitude, SiteLongitude, SiteCodes, NetLayerReport.CW_Marker, NetLayerReport.CW_Circles, NetLayerReport.Azmiuth, $('#cw-Radius').val(), $('#cw-Azmuth').val(), NetLayerReport.CWDropSignal);
        } else {
            var kml = DomainPath + '/cw.kml';
            kml = kml.replace(' ', '%20')
            initializekml('HO_Plot', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#cw-Radius').val(), $('#cw-Azmuth').val());
        }


        return false;
    });

    doc.on('click', '#btn-ccw', function () {

        if (AfterDate == 'False') {
            initialize('HO_Plot_CCW', SiteLatitude, SiteLongitude, SiteCodes, NetLayerReport.CCW_Marker, NetLayerReport.CCW_Circles, NetLayerReport.Azmiuth, $('#ccw-Radius').val(), $('#ccw-Azmuth').val())
        } else {
            var kml = DomainPath + '/ccw.kml';
            kml = kml.replace(' ', '%20')
            initializekml('HO_Plot_CCW', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#ccw-Radius').val(), $('#ccw-Azmuth').val());
        }



        return false;
    });
    doc.on('click', '#btn-rsrp', function () {
        if (AfterDate == 'False') {
            initialize('RSRP_Plot', SiteLatitude, SiteLongitude, SiteCodes, null, NetLayerReport.RsRp_Circles, NetLayerReport.Azmiuth, $('#rsrp-Radius').val(), $('#rsrp-Azmuth').val())
        } else {
            var kml = DomainPath + '/Rsrp.kml';
            kml = kml.replace(' ', '%20')
            //console.log(kml);
            initializekml('RSRP_Plot', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#rsrp-Radius').val(), $('#rsrp-Azmuth').val());
        }


        return false;
    });

    doc.on('click', '#btn-rsrq', function () {
        if (AfterDate == 'False') {
            initialize('RSRQ_Plot', SiteLatitude, SiteLongitude, SiteCodes, null, NetLayerReport.RSRQ_Circles, NetLayerReport.Azmiuth, $('#rsrq-Radius').val(), $('#rsrq-Azmuth').val())
        } else {
            var kml = DomainPath + '/Rsrq.kml';
            kml = kml.replace(' ', '%20')
            initializekml('RSRQ_Plot', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#rsrq-Radius').val(), $('#rsrq-Azmuth').val());
        }

        return false;
    });

    doc.on('click', '#btn-cinr', function () {
        if (AfterDate == 'False') {
            initialize('CINR_Plot', SiteLatitude, SiteLongitude, SiteCodes, null, NetLayerReport.CINR_Circles, NetLayerReport.Azmiuth, $('#cinr-Radius').val(), $('#cinr-Azmuth').val());
        } else {
            var kml = DomainPath + '/Cinr.kml';
            kml = kml.replace(' ', '%20')
            initializekml('CINR_Plot', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#cinr-Radius').val(), $('#cinr-Azmuth').val());
        }


        return false;
    });
    // Beam Group Click Function
    doc.on('click', '#btn-bmg', function () {
        if (AfterDate == 'False') {
            initialize('bmg_Plot', SiteLatitude, SiteLongitude, SiteCodes, null, NetLayerReport.CINR_Circles, NetLayerReport.Azmiuth, $('#bmg-Radius').val(), $('#bmg-Azmuth').val());
        } else {
            var kml = DomainPath + '/bmg.kml';
            kml = kml.replace(' ', '%20')
            initializekml('bmg_Plot', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#bmg-Radius').val(), $('#bmg-Azmuth').val());
        }


        return false;
    });
    // Beam Click Function
    doc.on('click', '#btn-bm', function () {
        if (AfterDate == 'False') {
            initialize('bm_Plot', SiteLatitude, SiteLongitude, SiteCodes, null, NetLayerReport.CINR_Circles, NetLayerReport.Azmiuth, $('#bm-Radius').val(), $('#bm-Azmuth').val());
        } else {
            var kml = DomainPath + '/bm.kml';
            kml = kml.replace(' ', '%20')
            initializekml('bm_Plot', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#bm-Radius').val(), $('#bm-Azmuth').val());
        }


        return false;
    });


    doc.on('click', '#btn-chPlot', function () {
        if (AfterDate == 'False') {
            initialize('CH_Plot', SiteLatitude, SiteLongitude, SiteCodes, null, NetLayerReport.CH_Circles, NetLayerReport.Azmiuth, $('#chPlot-Radius').val(), $('#chPlot-Azmuth').val());
        } else {
            var kml = DomainPath + '/ch.kml';
            kml = kml.replace(' ', '%20')
            initializekml('CH_PLOT', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#chPlot-Radius').val(), $('#chPlot-Azmuth').val());
        }


        return false;
    });
    doc.on('click', '#btn-brrel', function () {

        if (AfterDate == 'False') {
            initialize('release_map', SiteLatitude, SiteLongitude, SiteCodes, null, NetLayerReport.CH_Circles, NetLayerReport.Azmiuth, $('#brrel-Radius').val(), $('#brrel-Azmuth').val());
        } else {
            var kml = DomainPath + '/brrel.kml';
            kml = kml.replace(' ', '%20')
            initializekml('release_map', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, $('#brrel-Radius').val(), $('#brrel-Azmuth').val());
        }


        return false;
    });

    //if (document.readyState === "complete") {
    //    GetPreDefinedImages();
    //}
    
});


//$("#dnldpdf").click(function () {
   


//});




function ConvertPDF() {
    debugger
    $('body').loader('show', '<img src="../Content/Images/Application/Logo_Loading.gif">');
    $("#ccwhoplotmap_title").show();
    $("#cwhoplotmap_title").show();
    if (document.getElementById("cw_plottbl").rows.length <= 0) {
        $("#cwhoplot_title").hide();
    }
    if (document.getElementById("ccw_plottbl").rows.length <= 0) {
        $("#ccwhoplot_title").hide();
    }
    $(".hide_form").hide();
    $(".gmnoprint").hide();
    $("body").addClass("OnPdfGeneration");
    var pdf = new jsPDF('l', 'pt', 'a4');
    pdf.setProperties({
        title: "NetLayerReport"
    });
    imgwlcmdata = "";
    imgfooterdata = "";
    imgsitedatatbl = "";
    imgfiltest5g = "";
    imgsummaryrslt = "";
    imgdlplot = "";
    imgsitedatamap = "";
    imgcrplot = "";
    imgpciplot = "";
    imgpciLTEplot = "";
    imgcwhoplot = "";
    imgccwhoplot = "";
    imghoplot = "";
    imgdrpeventplot = "";
    imgrsrpplot = "";
    imgrsrqplot = "";
    imgcinrplot = "";
    imgcwhoplotmap = "";
    imgccwhoplotmap = "";
    imgthanks_footer = "";
    imgnrspdtest = "";
    imgltespdtest = "";
    imglocimages = "";
    imgnrmanualspdtest = "";
    html2canvas(document.querySelector("#delImage"), {
        scale: 3,
        allowTaint: false,
        useCORS: false

    }).then(canvas => {
        var dumb = canvas.toDataURL("image/jpeg", 1);
        pdf.addImage(dumb, 'JPEG', 10, 20, 822, 550);
        html2canvas(document.querySelector("#welcome_header_PDF"), {
            scale: 3,
        allowTaint: false,
        useCORS: false

    }).then(canvas => {
        imgwlcmdata = canvas.toDataURL("image/jpeg", 1);
        pdf.addImage(imgwlcmdata, 'JPEG', 0, 0, 845, 600);
        //2
        html2canvas(document.querySelector("#report_foooter"), {
            scale: 3,
            allowTaint: true,
            useCORS: true

        }).then(canvas => {


            imgfooterdata = canvas.toDataURL("image/jpeg", 1);
            //3
            html2canvas(document.querySelector("#sitedatatbl"), {
                scale: 3,
                allowTaint: true,
                useCORS: true

            }).then(canvas => {




                imgsitedatatbl = canvas.toDataURL("image/jpeg", 1);
                pdf.addPage();
                //footer();
                pdf.addImage(imgsitedatatbl, 'JPEG', 10, 20, 822, 530);
                pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                //document.getElementById("imgtbl1").src = imgageData;
                //4
                html2canvas(document.querySelector("#sitedatamap"), {
                    scale: 3,
                    allowTaint: true,
                    useCORS: true

                }).then(canvas => {



                    imgsitedatamap = canvas.toDataURL("image/jpeg", 1);
                    //pdf.addPage();
                    //footer();
                    //pdf.addImage(imgsitedatamap, 'JPEG', 10, 20, 822, 530);
                    //pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                    //5
                    html2canvas(document.querySelector("#filedtest5g"), {
                        scale: 3,
                        allowTaint: true,
                        useCORS: true

                    }).then(canvas => {
                        if ($('#pan-FIELD_TEST1:visible').length > 0) {
                            imgfiltest5g = canvas.toDataURL("image/jpeg", 1);
                            pdf.addPage();
                            //footer();
                            pdf.addImage(imgfiltest5g, 'JPEG', 10, 20, 822, 0);
                            pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                        }
                        
                        //6
                        html2canvas(document.querySelector("#summaryrslt"), {
                            scale: 3,
                            allowTaint: true,
                            useCORS: true

                        }).then(canvas => {



                            imgsummaryrslt = canvas.toDataURL("image/jpeg", 1);
                            pdf.addPage();
                            //footer();
                            pdf.addImage(imgsummaryrslt, 'JPEG', 10, 20, 822, 0);
                            pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                            //7

                            html2canvas(document.querySelector("#crplot"), {
                                scale: 3,
                                allowTaint: true,
                                useCORS: true

                            }).then(canvas => {
                                debugger
                                if ($('#pan-CH_PLOT:visible').length > 0) {
                                    imgcrplot = canvas.toDataURL("image/jpeg", 1);
                                    pdf.addPage();

                                    pdf.addImage(imgcrplot, 'JPEG', 10, 20, 822, 530);
                                    pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                }

                                
                                //8
                                html2canvas(document.querySelector("#pciplot"), {
                                    scale: 3,
                                    allowTaint: true,
                                    useCORS: true

                                }).then(canvas => {


                                    if ($('#pan-PCI_PLOT:visible').length > 0) {
                                        imgpciplot = canvas.toDataURL("image/jpeg", 1);
                                        pdf.addPage();
                                        //footer();
                                        pdf.addImage(imgpciplot, 'JPEG', 10, 20, 822, 530);
                                        pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                    }
                                   
                                    //9
                                    html2canvas(document.querySelector("#pciplotLTE"), {
                                        scale: 3,
                                        allowTaint: true,
                                        useCORS: true

                                    }).then(canvas => {


                                        if ($('#pan-PCI_PLOTLTE:visible').length > 0) {
                                            imgpciLTEplot = canvas.toDataURL("image/jpeg", 1);
                                            pdf.addPage();
                                            //footer();
                                            pdf.addImage(imgpciLTEplot, 'JPEG', 10, 20, 822, 530);
                                            pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                        }

                                    //9.1


                                    html2canvas(document.querySelector("#cwhoplot"), {
                                        scale: 3,
                                        allowTaint: true,
                                        useCORS: true

                                    }).then(canvas => {

                                        if ($('#pan-CW_PLOT:visible').length > 0) {
                                            if (document.getElementById("cw_plottbl").rows.length > 0) {
                                                imgcwhoplot = canvas.toDataURL("image/jpeg", 1);
                                                pdf.addPage();
                                                //footer();
                                                pdf.addImage(imgcwhoplot, 'JPEG', 10, 20, 822, 0);
                                            } else {
                                                pdf.addPage();
                                            }
                                        }
                                        
                                        //10
                                        html2canvas(document.querySelector("#cwhoplotmap"), {
                                            scale: 3,
                                            allowTaint: true,
                                            useCORS: true

                                        }).then(canvas => {


                                            if ($('#pan-CW_PLOT:visible').length > 0) {
                                                imgcwhoplotmap = canvas.toDataURL("image/jpeg", 1);
                                                if (document.getElementById("cw_plottbl").rows.length > 0) {
                                                    pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                    pdf.addPage();
                                                    pdf.addImage(imgcwhoplotmap, 'JPEG', 10, 20, 822, 530);
                                                }
                                                else {

                                                    pdf.addImage(imgcwhoplotmap, 'JPEG', 10, 50, 822, 500);
                                                }
                                                pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                            }
                                            //11

                                            html2canvas(document.querySelector("#ccwhoplot"), {
                                                scale: 3,
                                                allowTaint: true,
                                                useCORS: true

                                            }).then(canvas => {


                                                if ($('#pan-CCW_PLOT:visible').length > 0) {
                                                    if (document.getElementById("ccw_plottbl").rows.length > 0) {
                                                        imgccwhoplot = canvas.toDataURL("image/jpeg", 1);
                                                        pdf.addPage();
                                                        //footer();
                                                        pdf.addImage(imgccwhoplot, 'JPEG', 10, 20, 822, 0);
                                                    } else {
                                                        pdf.addPage();
                                                    }
                                                }
                                                
                                                //12

                                                html2canvas(document.querySelector("#ccwhoplotmap"), {
                                                    scale: 3,
                                                    allowTaint: true,
                                                    useCORS: true

                                                }).then(canvas => {


                                                    if ($('#pan-CCW_PLOT:visible').length > 0) {
                                                        imgccwhoplotmap = canvas.toDataURL("image/jpeg", 1);
                                                        if (document.getElementById("ccw_plottbl").rows.length > 0) {
                                                            pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                            pdf.addPage();
                                                            pdf.addImage(imgccwhoplotmap, 'JPEG', 10, 20, 822, 530);
                                                        }
                                                        else {
                                                            pdf.addImage(imgccwhoplotmap, 'JPEG', 10, 50, 822, 500);
                                                        }

                                                        //footer();

                                                        pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                    }
                                                    //13
                                                    html2canvas(document.querySelector("#hoplot"), {
                                                        scale: 3,
                                                        allowTaint: true,
                                                        useCORS: true

                                                    }).then(canvas => {

                                                        if ($('#pan-HANDOVER_PLOT:visible').length > 0) {
                                                            imghoplot = canvas.toDataURL("image/jpeg", 1);
                                                                pdf.addPage();
                                                                //footer();
                                                                pdf.addImage(imghoplot, 'JPEG', 10, 20, 822, 530);
                                                                pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                        }
                                                        //14

                                                        html2canvas(document.querySelector("#dlplot"), {
                                                            scale: 3,
                                                            allowTaint: true,
                                                            useCORS: true

                                                        }).then(canvas => {

                                                            if ($('#pan-DL_PLOT:visible').length > 0) {
                                                                imgdlplot = canvas.toDataURL("image/jpeg", 1);
                                                                pdf.addPage();
                                                                //footer();
                                                                pdf.addImage(imgdlplot, 'JPEG', 10, 20, 822, 530);
                                                                pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                            }
                                                            html2canvas(document.querySelector("#drpeventplot"), {
                                                                scale: 3,
                                                                allowTaint: true,
                                                                useCORS: true

                                                            }).then(canvas => {
                                                            if ($('#pan-NR_BR:visible').length > 0) {
                                                                imgdrpeventplot = canvas.toDataURL("image/jpeg", 1);
                                                                pdf.addPage();
                                                                //footer();
                                                                pdf.addImage(imgdrpeventplot, 'JPEG', 10, 20, 822, 530);
                                                                pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                            }
                                                            //15


                                                            html2canvas(document.querySelector("#rsrpplot"), {
                                                                scale: 3,
                                                                allowTaint: true,
                                                                useCORS: true

                                                            }).then(canvas => {


                                                                if ($('#pan-RSRP_PLOT:visible').length > 0) {
                                                                    imgrsrpplot = canvas.toDataURL("image/jpeg", 1);
                                                                    pdf.addPage();
                                                                    //footer();
                                                                    pdf.addImage(imgrsrpplot, 'JPEG', 10, 20, 822, 530);
                                                                    pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                }
                                                                
                                                                //16
                                                                html2canvas(document.querySelector("#rsrqplot"), {
                                                                    scale: 3,
                                                                    allowTaint: true,
                                                                    useCORS: true

                                                                }).then(canvas => {


                                                                    if ($('#pan-RSRQ_PLOT:visible').length > 0) {
                                                                        imgrsrqplot = canvas.toDataURL("image/jpeg", 1);
                                                                        pdf.addPage();
                                                                        //footer();
                                                                        pdf.addImage(imgrsrqplot, 'JPEG', 10, 20, 822, 530);
                                                                        pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                    }

                                                                    //17
                                                                    html2canvas(document.querySelector("#cinrplot"), {
                                                                        scale: 3,
                                                                        allowTaint: true,
                                                                        useCORS: true

                                                                    }).then(canvas => {


                                                                        if ($('#pan-CINR_PLOT:visible').length > 0) {
                                                                            imgcinrplot = canvas.toDataURL("image/jpeg", 1);
                                                                            pdf.addPage();
                                                                            //footer();
                                                                            pdf.addImage(imgcinrplot, 'JPEG', 10, 20, 822, 530);
                                                                            pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                        }

                                                                        //18





                                                                        if (document.getElementById("locimages") != null) {
                                                                            html2canvas(document.querySelector("#locimages"), {
                                                                                scale: 3,
                                                                                allowTaint: true,
                                                                                useCORS: true

                                                                            }).then(canvas => {
                                                                                imglocimages = canvas.toDataURL("image/jpeg", 1);
                                                                                pdf.addPage();
                                                                                //footer();
                                                                                pdf.addImage(imglocimages, 'JPEG', 10, 20, 822, 350);
                                                                                pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                if (document.getElementById("nrimages") != null) {
                                                                                    html2canvas(document.querySelector("#nrimages"), {
                                                                                        scale: 3,
                                                                                        allowTaint: true,
                                                                                        useCORS: true

                                                                                    }).then(canvas => {


                                                                                        imgnrspdtest = canvas.toDataURL("image/jpeg", 1);
                                                                                        pdf.addPage();
                                                                                        //footer();
                                                                                        pdf.addImage(imgnrspdtest, 'JPEG', 10, 20, 822, 350);
                                                                                        pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                        //19
                                                                                        if (document.getElementById("nrimagesManual") != null) {
                                                                                            html2canvas(document.querySelector("#nrimagesManual"), {
                                                                                                scale: 3,
                                                                                                allowTaint: true,
                                                                                                useCORS: true

                                                                                            }).then(canvas => {


                                                                                                imgnrmanualspdtest = canvas.toDataURL("image/jpeg");
                                                                                                pdf.addPage();
                                                                                                //footer();
                                                                                                pdf.addImage(imgnrmanualspdtest, 'JPEG', 0, 0, 822, 350);
                                                                                                pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                                //20
                                                                                                if (document.getElementById("lteimages") != null) {
                                                                                                    html2canvas(document.querySelector("#lteimages"), {
                                                                                                        scale: 3,
                                                                                                        allowTaint: true,
                                                                                                        useCORS: true

                                                                                                    }).then(canvas => {


                                                                                                        imgltespdtest = canvas.toDataURL("image/jpeg");
                                                                                                        pdf.addPage();
                                                                                                        //footer();
                                                                                                        pdf.addImage(imgltespdtest, 'JPEG', 0, 0, 822, 350);
                                                                                                        pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                                        //20
                                                                                                        html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                            scale: 3,
                                                                                                            allowTaint: true,
                                                                                                            useCORS: true

                                                                                                        }).then(canvas => {

                                                                                                            debugger

                                                                                                            imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                            pdf.addPage();
                                                                                                            //footer();
                                                                                                            pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                            //pdf.deletePage(1);
                                                                                                            window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                            //pdf.output('dataurlnewwindow')
                                                                                                            $(".hide_form").show();

                                                                                                            $(".gmnoprint").show();
                                                                                                            $('body').loader('hide');
                                                                                                            $("#cwhoplotmap_title").hide();
                                                                                                            $("#ccwhoplotmap_title").hide();
                                                                                                            $("#cwhoplot_title").show();
                                                                                                            $("#ccwhoplot_title").show();
                                                                                                            $("body").removeClass("OnPdfGeneration");
                                                                                                        });
                                                                                                    });
                                                                                                }
                                                                                                else {
                                                                                                    html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                        scale: 3,
                                                                                                        allowTaint: true,
                                                                                                        useCORS: true

                                                                                                    }).then(canvas => {

                                                                                                        debugger

                                                                                                        imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                        pdf.addPage();
                                                                                                        //footer();
                                                                                                        pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                        window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                        //pdf.output('dataurlnewwindow')
                                                                                                        $(".hide_form").show();

                                                                                                        $(".gmnoprint").show();
                                                                                                        $('body').loader('hide');
                                                                                                        $("#cwhoplotmap_title").hide();
                                                                                                        $("#ccwhoplotmap_title").hide();
                                                                                                        $("#cwhoplot_title").show();
                                                                                                        $("#ccwhoplot_title").show();
                                                                                                        $("body").removeClass("OnPdfGeneration");
                                                                                                    });
                                                                                                }
                                                                                            });
                                                                                        }
                                                                                        else {
                                                                                            if (document.getElementById("lteimages") != null) {
                                                                                                html2canvas(document.querySelector("#lteimages"), {
                                                                                                    scale: 3,
                                                                                                    allowTaint: true,
                                                                                                    useCORS: true

                                                                                                }).then(canvas => {


                                                                                                    imgltespdtest = canvas.toDataURL("image/jpeg");
                                                                                                    pdf.addPage();
                                                                                                    //footer();
                                                                                                    pdf.addImage(imgltespdtest, 'JPEG', 0, 0, 822, 350);
                                                                                                    pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                                    //20
                                                                                                    html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                        scale: 3,
                                                                                                        allowTaint: true,
                                                                                                        useCORS: true

                                                                                                    }).then(canvas => {

                                                                                                        debugger

                                                                                                        imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                        pdf.addPage();
                                                                                                        //footer();
                                                                                                        pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                        //pdf.deletePage(1);
                                                                                                        window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                        //pdf.output('dataurlnewwindow')
                                                                                                        $(".hide_form").show();

                                                                                                        $(".gmnoprint").show();
                                                                                                        $('body').loader('hide');
                                                                                                        $("#cwhoplotmap_title").hide();
                                                                                                        $("#ccwhoplotmap_title").hide();
                                                                                                        $("#cwhoplot_title").show();
                                                                                                        $("#ccwhoplot_title").show();
                                                                                                        $("body").removeClass("OnPdfGeneration");
                                                                                                    });
                                                                                                });
                                                                                            }
                                                                                            else {
                                                                                                html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                    scale: 3,
                                                                                                    allowTaint: true,
                                                                                                    useCORS: true

                                                                                                }).then(canvas => {

                                                                                                    debugger

                                                                                                    imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                    pdf.addPage();
                                                                                                    //footer();
                                                                                                    pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                    window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                    //pdf.output('dataurlnewwindow')
                                                                                                    $(".hide_form").show();

                                                                                                    $(".gmnoprint").show();
                                                                                                    $('body').loader('hide');
                                                                                                    $("#cwhoplotmap_title").hide();
                                                                                                    $("#ccwhoplotmap_title").hide();
                                                                                                    $("#cwhoplot_title").show();
                                                                                                    $("#ccwhoplot_title").show();
                                                                                                    $("body").removeClass("OnPdfGeneration");
                                                                                                });
                                                                                            }
                                                                                        }
                                                                                    });
                                                                                }

                                                                                else {
                                                                                    if (document.getElementById("nrimagesManual") != null) {
                                                                                        html2canvas(document.querySelector("#nrimagesManual"), {
                                                                                            scale: 3,
                                                                                            allowTaint: true,
                                                                                            useCORS: true

                                                                                        }).then(canvas => {


                                                                                            imgnrmanualspdtest = canvas.toDataURL("image/jpeg");
                                                                                            pdf.addPage();
                                                                                            //footer();
                                                                                            pdf.addImage(imgnrmanualspdtest, 'JPEG', 0, 0, 822, 350);
                                                                                            pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                            //20
                                                                                            if (document.getElementById("lteimages") != null) {
                                                                                                html2canvas(document.querySelector("#lteimages"), {
                                                                                                    scale: 3,
                                                                                                    allowTaint: true,
                                                                                                    useCORS: true

                                                                                                }).then(canvas => {


                                                                                                    imgltespdtest = canvas.toDataURL("image/jpeg");
                                                                                                    pdf.addPage();
                                                                                                    //footer();
                                                                                                    pdf.addImage(imgltespdtest, 'JPEG', 0, 0, 822, 350);
                                                                                                    pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                                    //20
                                                                                                    html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                        scale: 3,
                                                                                                        allowTaint: true,
                                                                                                        useCORS: true

                                                                                                    }).then(canvas => {

                                                                                                        debugger

                                                                                                        imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                        pdf.addPage();
                                                                                                        //footer();
                                                                                                        pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                        //pdf.deletePage(1);
                                                                                                        window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                        //pdf.output('dataurlnewwindow')
                                                                                                        $(".hide_form").show();

                                                                                                        $(".gmnoprint").show();
                                                                                                        $('body').loader('hide');
                                                                                                        $("#cwhoplotmap_title").hide();
                                                                                                        $("#ccwhoplotmap_title").hide();
                                                                                                        $("#cwhoplot_title").show();
                                                                                                        $("#ccwhoplot_title").show();
                                                                                                        $("body").removeClass("OnPdfGeneration");
                                                                                                    });
                                                                                                });
                                                                                            }
                                                                                            else {
                                                                                                html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                    scale: 3,
                                                                                                    allowTaint: true,
                                                                                                    useCORS: true

                                                                                                }).then(canvas => {

                                                                                                    debugger

                                                                                                    imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                    pdf.addPage();
                                                                                                    //footer();
                                                                                                    pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                    window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                    //pdf.output('dataurlnewwindow')
                                                                                                    $(".hide_form").show();

                                                                                                    $(".gmnoprint").show();
                                                                                                    $('body').loader('hide');
                                                                                                    $("#cwhoplotmap_title").hide();
                                                                                                    $("#ccwhoplotmap_title").hide();
                                                                                                    $("#cwhoplot_title").show();
                                                                                                    $("#ccwhoplot_title").show();
                                                                                                    $("body").removeClass("OnPdfGeneration");
                                                                                                });
                                                                                            }
                                                                                        });
                                                                                    }
                                                                                    else {
                                                                                        if (document.getElementById("lteimages") != null) {
                                                                                            html2canvas(document.querySelector("#lteimages"), {
                                                                                                scale: 3,
                                                                                                allowTaint: true,
                                                                                                useCORS: true

                                                                                            }).then(canvas => {


                                                                                                imgltespdtest = canvas.toDataURL("image/jpeg");
                                                                                                pdf.addPage();
                                                                                                //footer();
                                                                                                pdf.addImage(imgltespdtest, 'JPEG', 0, 0, 822, 350);
                                                                                                pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                                //20
                                                                                                html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                    scale: 3,
                                                                                                    allowTaint: true,
                                                                                                    useCORS: true

                                                                                                }).then(canvas => {

                                                                                                    debugger

                                                                                                    imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                    pdf.addPage();
                                                                                                    //footer();
                                                                                                    pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                    //pdf.deletePage(1);
                                                                                                    window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                    //pdf.output('dataurlnewwindow')
                                                                                                    $(".hide_form").show();

                                                                                                    $(".gmnoprint").show();
                                                                                                    $('body').loader('hide');
                                                                                                    $("#cwhoplotmap_title").hide();
                                                                                                    $("#ccwhoplotmap_title").hide();
                                                                                                    $("#cwhoplot_title").show();
                                                                                                    $("#ccwhoplot_title").show();
                                                                                                    $("body").removeClass("OnPdfGeneration");
                                                                                                });
                                                                                            });
                                                                                        }
                                                                                        else {
                                                                                            html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                scale: 3,
                                                                                                allowTaint: true,
                                                                                                useCORS: true

                                                                                            }).then(canvas => {

                                                                                                debugger

                                                                                                imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                pdf.addPage();
                                                                                                //footer();
                                                                                                pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                //pdf.output('dataurlnewwindow')
                                                                                                $(".hide_form").show();

                                                                                                $(".gmnoprint").show();
                                                                                                $('body').loader('hide');
                                                                                                $("#cwhoplotmap_title").hide();
                                                                                                $("#ccwhoplotmap_title").hide();
                                                                                                $("#cwhoplot_title").show();
                                                                                                $("#ccwhoplot_title").show();
                                                                                                $("body").removeClass("OnPdfGeneration");
                                                                                            });
                                                                                        }
                                                                                    }

                                                                                }
                                                                            })
                                                                        }
                                                                        else {
                                                                            if (document.getElementById("nrimages") != null) {
                                                                                html2canvas(document.querySelector("#nrimages"), {
                                                                                    scale: 3,
                                                                                    allowTaint: true,
                                                                                    useCORS: true

                                                                                }).then(canvas => {


                                                                                    imgnrspdtest = canvas.toDataURL("image/jpeg", 1);
                                                                                    pdf.addPage();
                                                                                    //footer();
                                                                                    pdf.addImage(imgnrspdtest, 'JPEG', 10, 20, 822, 350);
                                                                                    pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                    //19
                                                                                    if (document.getElementById("nrimagesManual") != null) {
                                                                                        html2canvas(document.querySelector("#nrimagesManual"), {
                                                                                            scale: 3,
                                                                                            allowTaint: true,
                                                                                            useCORS: true

                                                                                        }).then(canvas => {


                                                                                            imgnrmanualspdtest = canvas.toDataURL("image/jpeg");
                                                                                            pdf.addPage();
                                                                                            //footer();
                                                                                            pdf.addImage(imgnrmanualspdtest, 'JPEG', 0, 0, 822, 350);
                                                                                            pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                            //20
                                                                                            if (document.getElementById("lteimages") != null) {
                                                                                                html2canvas(document.querySelector("#lteimages"), {
                                                                                                    scale: 3,
                                                                                                    allowTaint: true,
                                                                                                    useCORS: true

                                                                                                }).then(canvas => {


                                                                                                    imgltespdtest = canvas.toDataURL("image/jpeg");
                                                                                                    pdf.addPage();
                                                                                                    //footer();
                                                                                                    pdf.addImage(imgltespdtest, 'JPEG', 0, 0, 822, 350);
                                                                                                    pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                                    //20
                                                                                                    html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                        scale: 3,
                                                                                                        allowTaint: true,
                                                                                                        useCORS: true

                                                                                                    }).then(canvas => {

                                                                                                        debugger

                                                                                                        imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                        pdf.addPage();
                                                                                                        //footer();
                                                                                                        pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                        //pdf.deletePage(1);
                                                                                                        window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                        //pdf.output('dataurlnewwindow')
                                                                                                        $(".hide_form").show();

                                                                                                        $(".gmnoprint").show();
                                                                                                        $('body').loader('hide');
                                                                                                        $("#cwhoplotmap_title").hide();
                                                                                                        $("#ccwhoplotmap_title").hide();
                                                                                                        $("#cwhoplot_title").show();
                                                                                                        $("#ccwhoplot_title").show();
                                                                                                        $("body").removeClass("OnPdfGeneration");
                                                                                                    });
                                                                                                });
                                                                                            }
                                                                                            else {
                                                                                                html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                    scale: 3,
                                                                                                    allowTaint: true,
                                                                                                    useCORS: true

                                                                                                }).then(canvas => {

                                                                                                    debugger

                                                                                                    imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                    pdf.addPage();
                                                                                                    //footer();
                                                                                                    pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                    window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                    //pdf.output('dataurlnewwindow')
                                                                                                    $(".hide_form").show();

                                                                                                    $(".gmnoprint").show();
                                                                                                    $('body').loader('hide');
                                                                                                    $("#cwhoplotmap_title").hide();
                                                                                                    $("#ccwhoplotmap_title").hide();
                                                                                                    $("#cwhoplot_title").show();
                                                                                                    $("#ccwhoplot_title").show();
                                                                                                    $("body").removeClass("OnPdfGeneration");
                                                                                                });
                                                                                            }
                                                                                        });
                                                                                    }
                                                                                    else {
                                                                                        if (document.getElementById("lteimages") != null) {
                                                                                            html2canvas(document.querySelector("#lteimages"), {
                                                                                                scale: 3,
                                                                                                allowTaint: true,
                                                                                                useCORS: true

                                                                                            }).then(canvas => {


                                                                                                imgltespdtest = canvas.toDataURL("image/jpeg");
                                                                                                pdf.addPage();
                                                                                                //footer();
                                                                                                pdf.addImage(imgltespdtest, 'JPEG', 0, 0, 822, 350);
                                                                                                pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                                //20
                                                                                                html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                    scale: 3,
                                                                                                    allowTaint: true,
                                                                                                    useCORS: true

                                                                                                }).then(canvas => {

                                                                                                    debugger

                                                                                                    imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                    pdf.addPage();
                                                                                                    //footer();
                                                                                                    pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                    //pdf.deletePage(1);
                                                                                                    window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                    //pdf.output('dataurlnewwindow')
                                                                                                    $(".hide_form").show();

                                                                                                    $(".gmnoprint").show();
                                                                                                    $('body').loader('hide');
                                                                                                    $("#cwhoplotmap_title").hide();
                                                                                                    $("#ccwhoplotmap_title").hide();
                                                                                                    $("#cwhoplot_title").show();
                                                                                                    $("#ccwhoplot_title").show();
                                                                                                    $("body").removeClass("OnPdfGeneration");
                                                                                                });
                                                                                            });
                                                                                        }
                                                                                        else {
                                                                                            html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                scale: 3,
                                                                                                allowTaint: true,
                                                                                                useCORS: true

                                                                                            }).then(canvas => {

                                                                                                debugger

                                                                                                imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                pdf.addPage();
                                                                                                //footer();
                                                                                                pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                //pdf.output('dataurlnewwindow')
                                                                                                $(".hide_form").show();

                                                                                                $(".gmnoprint").show();
                                                                                                $('body').loader('hide');
                                                                                                $("#cwhoplotmap_title").hide();
                                                                                                $("#ccwhoplotmap_title").hide();
                                                                                                $("#cwhoplot_title").show();
                                                                                                $("#ccwhoplot_title").show();
                                                                                                $("body").removeClass("OnPdfGeneration");
                                                                                            });
                                                                                        }
                                                                                    }
                                                                                });
                                                                            }

                                                                            else {
                                                                                if (document.getElementById("nrimagesManual") != null) {
                                                                                    html2canvas(document.querySelector("#nrimagesManual"), {
                                                                                        scale: 3,
                                                                                        allowTaint: true,
                                                                                        useCORS: true

                                                                                    }).then(canvas => {


                                                                                        imgnrmanualspdtest = canvas.toDataURL("image/jpeg");
                                                                                        pdf.addPage();
                                                                                        //footer();
                                                                                        pdf.addImage(imgnrmanualspdtest, 'JPEG', 0, 0, 822, 350);
                                                                                        pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                        //20
                                                                                        if (document.getElementById("lteimages") != null) {
                                                                                            html2canvas(document.querySelector("#lteimages"), {
                                                                                                scale: 3,
                                                                                                allowTaint: true,
                                                                                                useCORS: true

                                                                                            }).then(canvas => {


                                                                                                imgltespdtest = canvas.toDataURL("image/jpeg");
                                                                                                pdf.addPage();
                                                                                                //footer();
                                                                                                pdf.addImage(imgltespdtest, 'JPEG', 0, 0, 822, 350);
                                                                                                pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                                //20
                                                                                                html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                    scale: 3,
                                                                                                    allowTaint: true,
                                                                                                    useCORS: true

                                                                                                }).then(canvas => {

                                                                                                    debugger

                                                                                                    imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                    pdf.addPage();
                                                                                                    //footer();
                                                                                                    pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                    //pdf.deletePage(1);
                                                                                                    window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                    //pdf.output('dataurlnewwindow')
                                                                                                    $(".hide_form").show();

                                                                                                    $(".gmnoprint").show();
                                                                                                    $('body').loader('hide');
                                                                                                    $("#cwhoplotmap_title").hide();
                                                                                                    $("#ccwhoplotmap_title").hide();
                                                                                                    $("#cwhoplot_title").show();
                                                                                                    $("#ccwhoplot_title").show();
                                                                                                    $("body").removeClass("OnPdfGeneration");
                                                                                                });
                                                                                            });
                                                                                        }
                                                                                        else {
                                                                                            html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                scale: 3,
                                                                                                allowTaint: true,
                                                                                                useCORS: true

                                                                                            }).then(canvas => {

                                                                                                debugger

                                                                                                imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                pdf.addPage();
                                                                                                //footer();
                                                                                                pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                //pdf.output('dataurlnewwindow')
                                                                                                $(".hide_form").show();

                                                                                                $(".gmnoprint").show();
                                                                                                $('body').loader('hide');
                                                                                                $("#cwhoplotmap_title").hide();
                                                                                                $("#ccwhoplotmap_title").hide();
                                                                                                $("#cwhoplot_title").show();
                                                                                                $("#ccwhoplot_title").show();
                                                                                                $("body").removeClass("OnPdfGeneration");
                                                                                            });
                                                                                        }
                                                                                    });
                                                                                }
                                                                                else {
                                                                                    if (document.getElementById("lteimages") != null) {
                                                                                        html2canvas(document.querySelector("#lteimages"), {
                                                                                            scale: 3,
                                                                                            allowTaint: true,
                                                                                            useCORS: true

                                                                                        }).then(canvas => {


                                                                                            imgltespdtest = canvas.toDataURL("image/jpeg");
                                                                                            pdf.addPage();
                                                                                            //footer();
                                                                                            pdf.addImage(imgltespdtest, 'JPEG', 0, 0, 822, 350);
                                                                                            pdf.addImage(imgfooterdata, 'JPEG', 10, 550, 822, 20);
                                                                                            //20
                                                                                            html2canvas(document.querySelector("#thanks_footer"), {
                                                                                                scale: 3,
                                                                                                allowTaint: true,
                                                                                                useCORS: true

                                                                                            }).then(canvas => {

                                                                                                debugger

                                                                                                imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                                pdf.addPage();
                                                                                                //footer();
                                                                                                pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                                //pdf.deletePage(1);
                                                                                                window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                                //pdf.output('dataurlnewwindow')
                                                                                                $(".hide_form").show();

                                                                                                $(".gmnoprint").show();
                                                                                                $('body').loader('hide');
                                                                                                $("#cwhoplotmap_title").hide();
                                                                                                $("#ccwhoplotmap_title").hide();
                                                                                                $("#cwhoplot_title").show();
                                                                                                $("#ccwhoplot_title").show();
                                                                                                $("body").removeClass("OnPdfGeneration");
                                                                                            });
                                                                                        });
                                                                                    }
                                                                                    else {
                                                                                        html2canvas(document.querySelector("#thanks_footer"), {
                                                                                            scale: 3,
                                                                                            allowTaint: true,
                                                                                            useCORS: true

                                                                                        }).then(canvas => {

                                                                                            debugger

                                                                                            imgthanks_footer = canvas.toDataURL("image/jpeg");
                                                                                            pdf.addPage();
                                                                                            //footer();
                                                                                            pdf.addImage(imgthanks_footer, 'JPEG', 0, 0, 845, 600);
                                                                                            window.open(pdf.output('bloburl', 'NetLayerReport.pdf'));
                                                                                            //pdf.output('dataurlnewwindow')
                                                                                            $(".hide_form").show();

                                                                                            $(".gmnoprint").show();
                                                                                            $('body').loader('hide');
                                                                                            $("#cwhoplotmap_title").hide();
                                                                                            $("#ccwhoplotmap_title").hide();
                                                                                            $("#cwhoplot_title").show();
                                                                                            $("#ccwhoplot_title").show();
                                                                                            $("body").removeClass("OnPdfGeneration");
                                                                                        });
                                                                                    }
                                                                                }

                                                                            }
                                                                        }



                                                                    });
                                                                });
                                                                });


                                                            });

                                                        });


                                                    });

                                                });
                                            });

                                        });

                                    });
                                    //9end
                                });
                                });

                            });


                        });
                    });
                });
            });

        });

    });
});

}


function LoadMaps() {
    // loading PUSCH Map

    MarkersArray = [];
    var data = $("#PUSCHLatLng").val();
    if (data != null || data != undefined) {
        var a = data.split('|');
        $.each(a, function (i, e) {
            var lat = e.split(',')[0];
            var lng = e.split(',')[1];
            MarkersArray.push({ Latitude: lat, Longitude: lng, Color: "" });

        });
        domain = document.location.origin;
        var DomainPath = domain + '/Content/AirViewLogs/' + ClientPrefix + '/' + NetLayer;
        var kml = DomainPath + '/cw.kml';
        kml = kml.replace(' ', '%20')
        //plotMarker();
        initializekmlNetLayer('PUSCH_map', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, 17, 20, MarkersArray);
    }
    // Loading PDSCH map

    MarkersArray1 = [];
    var data1 = $("#PDSCHLatLng").val();
    if (data1 != null || data1 != undefined) {
        var a1 = data1.split('|');
        $.each(a1, function (i, e) {
            var lat = e.split(',')[0];
            var lng = e.split(',')[1];
            MarkersArray1.push({ Latitude: lat, Longitude: lng, Color: "" });

        });
        initializekmlNetLayer('PDSCH_map', SiteLatitude, SiteLongitude, SiteCodes, kml, NetLayerReport.Azmiuth, 17, 20, MarkersArray1);
    }
}

//document.onreadystatechange = function () {
//    var state = document.readyState
//    if (state == 'complete') {
//        setTimeout(function () {
//            $('#pdf').loader('hide');
//        }, 1000);
//    }
//}