$json_obj = [];
$isJson = false;

var xl_script = (function () {
    return {
        /* ===== Accordion ==== */
        accordion: function () {
            $('body').on("click", ".toggle-item > h2", function () {
                if ($(this).next().is(':hidden')) {
                    $(this).parent().parent().find("h2").removeClass('active').next().slideUp(500).parent().removeClass("activate");
                    $(this).toggleClass('active').next().slideDown(500).parent().toggleClass("activate");
                }
                else {
                    $(this).removeClass('active').next().slideUp(500).parent().removeClass("activate");
                }
            });
        },

        /* ===== Dropdown Functionalities ==== */
        dropdown: function () {
            $('html').on('click', function (event) {
                if (!$(event.target).hasClass('multiselect')) {
                    $('.opt-dropdown').removeClass('active');
                    $('.opt-btn').removeClass('active').next('ul.opt-dropdown').removeClass('active');
                }
            });
            $('body').on('click', '.opt-btn', function () {
                $('.opt-btn').removeClass('active').next('ul.opt-dropdown').removeClass('active');
                $(this).toggleClass('active').next('ul.opt-dropdown').toggleClass('active');
                return false;
            });
            $('html').on('click', '.date-range, .daterangepicker, .farbtastic', function (e) { e.stopPropagation() });


            // Open Filters For Chart
            $("body").on("click", ".open-filters", function () {
                $(this).next('.filters-drop').toggleClass('active');
            });

            $("html").on("click", function (e) {
                if ($(e.target).parents().hasClass('filters-drop') || $(e.target).parents().hasClass('open-filters')) {
                    return false;
                }
                else {
                    $(".filters-drop").removeClass('active');
                }
            });

        },


        /* ===== Delay Function On Each List Item Of Dropdown ==== */
        delayeachdropdown: function () {
            $('ul.opt-dropdown').each(function () {
                var a = 0;
                $(this).children('li').each(function () {
                    $(this).css({ "transition-delay": a + 'ms' });
                    a = a + 50;
                });
            });
        },



        /* ===== Sideheader  ==== */
        sideheaderdropdown: function () {
            $('.sideheader > ul > li ul').parent().addClass('has-dropdown');
            $('.has-dropdown > a').on('click', function () {
                $(this).next('ul').slideToggle();
                $(this).parent().siblings('.has-dropdown').find('ul').slideUp();
                return false;
            });
        },


        /* =====  Dropdown ==== */
        optionsdropdown: function () {
            $('ul.opt-dropdown > li ul').parent().addClass('has-children');
            $('body').on('click', '.has-children > a', function () {
                $(this).next('ul').slideToggle();
                $(this).parent().siblings('.has-children').find('ul').slideUp();
                return false;
            });

        },



        /* ===== Custom SelectBox ==== */
        customselectbox: function () {
            $('body').on('click', '.custom-selectbox ul.opt-dropdown li a', function () {
                var updateValue = $(this).text();
                $(this).parents('.custom-selectbox').find('.opt-btn i').html(updateValue);
                return false;
            });
            $('body').on('click', '.custom-selectbox  input', function () {
                return false;
            });
        },


        /* ===== Progress Bar Dropdown ==== */
        progressbardropdown: function () {
            $("body").on('click', '.pv-progresser > a', function () {
                if ($(this).find('i').hasClass('fa-plus')) {
                    $(this).find('i').removeClass('fa-plus').addClass('fa-minus');
                }
                else if ($(this).find('i').hasClass('fa-minus')) {
                    $(this).find('i').removeClass('fa-minus').addClass('fa-plus');
                }
                $(this).parent().children('.pv-progresser-dropdown').slideToggle();
            });
        },



        /*================== Table Options =====================*/
        tableoptions: function () {
            $("body").on("click", '.custom-btn.opt-btn', function () {
                $(this).parents('tr').siblings().find('.opt-dropdown').removeClass('active');
                $(this).parents('tr').find('.opt-dropdown').addClass("active");
                return false;
            });
            $("body").on('click', '.more-table-btn', function () {
                $(this).parents('tr').next('tr.child-table').fadeToggle();
                var current_sign = $(this).html();
                if (current_sign == '+') { $(this).addClass('active').html('-'); }
                else { $(this).removeClass('active').html('+'); }
                return false;
            });
        },


        /*================== Stripped Rows =====================*/
        strippedRows: function () {
            $('table').each(function () {
                var RowList = $(this).children('tbody').children('tr.tablerow');
                var tableDataLength = RowList.length;

                for (i = 0; i < tableDataLength;) {
                    if ($(RowList[i]).hasClass('child-table')) {
                        continue;
                    }
                    if (i % 2 == 1) {
                        $(RowList[i]).addClass("stripped");
                    }

                    i++;
                }
            });
        },


        /* =============== Donut Chart Label Convert Into Dropdown On Small Screens ===================== */
        donutchartlabels: function () {
            $('body').on('click', ".label-drop", function () {
                $(this).next('.labeling').toggleClass('active');
                return false;
            });
        },



        /* =============== Header Logo Buttons Convert To Dropdown In Smaller Screen ===================== */
        headerlogobuttons: function () {
            // if($(window).width() < 1200){
            //         $("body").on('click', ".more-links > img", function(){
            //                 $(this).next('ul').toggleClass('active');
            //                 return false;
            //         });
            // }
        },

        /* =============== Responsive Header ===================== */
        responsiveHeader: function () {
            // Responsive Search open
            $("body").on("click", ".search.responsive-btn", function () {
                $(".responsive-header-search").slideDown();
                $(".responsive-info").removeClass('active');
                $(".responsive-opts").removeClass('active');
            });
            // Responsive Search close
            $("body").on("click", ".close-search", function () {
                $(".responsive-header-search").slideUp();
            });

            // Responsive Menu Button
            $('body').on('click', '.respsonive-menu-btn', function () {
                $('.sideheader').toggleClass('active');
                setTimeout(function () {
                    $("div[data-highcharts-chart]").each(function (index) {
                        $(this).highcharts().reflow();
                    });
                }, 300);
                return false;
            });

            $("body").on("click", ".show-info", function () {
                $(".responsive-header-search").slideUp();
                $(".responsive-opts").removeClass('active');
                $(".responsive-info").toggleClass('active');
            });

            $("body").on("click", ".others-opts", function () {
                $(".responsive-header-search").slideUp();
                $(".responsive-info").removeClass('active');
                $(".responsive-opts").toggleClass('active');
            });
        },




        /* =============== Menu Icon To Open Close Sidemenu ===================== */
        menuicon: function () {
            $('.menu-icon').on('click', function () {
                $('.sideheader').toggleClass('active');
                setTimeout(function () {
                    $("div[data-highcharts-chart]").each(function (index) {
                        $(this).highcharts().reflow();
                    });
                }, 300);
                return false;
            });

            if ($(window).width() < 1200) {
                $('html').on('click', function () {
                    $('.sideheader').removeClass('active');
                });
            }

            /* =======  */
            $("body").on("click", '.menu-btn-secondary a', function () {
                $(".sideheader").toggleClass('active');

                $(".sideheader").hasClass('inactive') ? $(".sideheader").removeClass('inactive') : $(".sideheader").addClass('inactive');

                setTimeout(function () {
                    $("div[data-highcharts-chart]").each(function (index) {
                        $(this).highcharts().reflow();
                    });
                }, 300);
                document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
                document.dispatchEvent(new Event('UpdateChildItems')); // broadcast an event when drawing area is resized
                return false;
            });

            $("body").on("click", ".collapsed-menu", function () {
                $('body').toggleClass('sec-sidemenu');
                document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
                document.dispatchEvent(new Event('UpdateChildItems')); // broadcast an event when drawing area is resized
                return false;
            });

            $("body").on("click", ".show-search-btn", function () {
                $("body").toggleClass('show-search');
                return false;
            });
        },


        /* =============== Popup ===================== */
        popup: function () {
            $("body").on("click", ".open-popup", function () {
                $(".popup").addClass("active");
                var checkId = $(this).attr('href');
                $('.popup-box').fadeOut();
                $(checkId).fadeIn();
                return false;
            });
            $("body").on("click", ".close-popup", function () {
                $(".popup").removeClass("active");
                $('.popup-box').fadeOut();
                return false;
            });
        },



        /* =============== HighCharts Resize On Menu Close and Open ===================== */
        highchartresize: function () {
            $('body').on('resize', '.widget', function () {
                $("div[data-highcharts-chart]").each(function (index) {
                    $(this).highcharts().reflow();
                });
            });
        },


        /* =============== Widget Collapse Button ===================== */
        collapseWidget: function () {
            $("body").on("click", ".collapse-btn", function () {
                $(this).attr('class', 'custom-btn icon-only expand-btn').find('i').attr('class', 'fa fa-plus');
                let parent = $(this).parents('.grid-stack-item');
                let currentHeight = $(parent).attr('data-gs-height');
                parent.attr('data-heighthistory', currentHeight);

                let minHeight = $(parent).attr('data-gs-min-height');
                if (parent.attr('data-gs-min-height')) {
                    parent.attr('data-minheighthistory', minHeight);
                }

                var grid = $('.grid-stack').data('gridstack');
                grid.minHeight(parent, '', 3);
                grid.resize(parent, '', 3);
                grid.resizable(parent, false);
                parent.find('.content-wrapper').hide();
                return false;
            });

            $("body").on("click", ".expand-btn", function () {
                $(this).attr('class', 'custom-btn icon-only collapse-btn').find('i').attr('class', 'fa fa-minus');
                let parent = $(this).parents('.grid-stack-item');
                var grid = $('.grid-stack').data('gridstack');
                if (parent.attr('data-heighthistory')) {
                    let heightHistory = parseInt(parent.attr('data-heighthistory'));
                    grid.resize(parent, '', heightHistory);
                } else {
                    grid.resize(parent, '', 20);
                }

                if (parent.attr('data-minheighthistory')) {
                    let minHeight = parseInt(parent.attr('data-minheighthistory'));
                    grid.minHeight(parent, minHeight)
                }

                debugger;
                if (parent.attr('data-gs-no-resize') == "true") {
                    grid.resizable(parent, false);
                } else {
                    grid.resizable(parent, true);
                }

                parent.find('.content-wrapper').show();
                return false;
            });

        },




        /* =============== Header Converter ===================== */
        headerConverter: function () {
            $(".header-converter").on("click", function () {
                $(".layout-wrapper").toggleClass('top-nav');
                $(".menu-icon").toggleClass("hide");
                return false;
            });
        },


        /* =============== Widget Remove Button ===================== */
        removeButton: function () {
            $("body").on("click", '.remove-btn', function () {
                let parent = $(this).parents('.grid-stack-item');
                var grid = $('.grid-stack').data('gridstack');
                $(this).parents('.widget').slideUp("complete", (function () {
                    grid.removeWidget(parent, true)
                }));
                return false;
            });
        },



        /* =============== Circular Loader  ===================== */
        CLoader: function () {
            $("#tss").circularloader({
                progressPercent: 60,
                backgroundColor: "#8a70b1",//background colour of inner circle
                fontColor: "#fff",//font color of progress text
                fontSize: "16px",//font size of progress text
                radius: 28,//radius of circle
                progressBarBackground: "#a290c4",//background colour of circular progress Bar
                progressBarColor: "#593e8f",//colour of circular progress bar
                progressBarWidth: 11,//progress bar width
                speed: 10,
                progressvalue: 60,
                showText: true
            });

            $("#ssm").circularloader({
                progressPercent: 50,
                backgroundColor: "#f37d62",//background colour of inner circle
                fontColor: "#fff",//font color of progress text
                fontSize: "16px",//font size of progress text
                radius: 28,//radius of circle
                progressBarBackground: "#f59585",//background colour of circular progress Bar
                progressBarColor: "#d65641",//colour of circular progress bar
                progressBarWidth: 11,//progress bar width
                speed: 10,
                progressvalue: 60,
                showText: true
            });

            $("#epl").circularloader({
                progressPercent: 70,
                backgroundColor: "#3b8655",//background colour of inner circle
                fontColor: "#fff",//font color of progress text
                fontSize: "16px",//font size of progress text
                radius: 28,//radius of circle
                progressBarBackground: "#579f6c",//background colour of circular progress Bar
                progressBarColor: "#277640",//colour of circular progress bar
                progressBarWidth: 11,//progress bar width
                speed: 10,
                progressvalue: 70,
                showText: true
            });

            $("#ordered").circularloader({
                progressPercent: 30,
                backgroundColor: "#f0b828",//background colour of inner circle
                fontColor: "#fff",//font color of progress text
                fontSize: "16px",//font size of progress text
                radius: 28,//radius of circle
                progressBarBackground: "#f4c76d",//background colour of circular progress Bar
                progressBarColor: "#d59529",//colour of circular progress bar
                progressBarWidth: 11,//progress bar width
                speed: 10,
                progressvalue: 30,
                showText: true
            });

            $("#calledout").circularloader({
                progressPercent: 80,
                backgroundColor: "#03a3a2",//background colour of inner circle
                fontColor: "#fff",//font color of progress text
                fontSize: "16px",//font size of progress text
                radius: 28,//radius of circle
                progressBarBackground: "#26bcb4",//background colour of circular progress Bar
                progressBarColor: "#037d7a",//colour of circular progress bar
                progressBarWidth: 11,//progress bar width
                speed: 10,
                progressvalue: 80,
                showText: true
            });

            $("#delivered").circularloader({
                progressPercent: 60,
                backgroundColor: "#2b9045",//background colour of inner circle
                fontColor: "#fff",//font color of progress text
                fontSize: "16px",//font size of progress text
                radius: 28,//radius of circle
                progressBarBackground: "#4ab55f",//background colour of circular progress Bar
                progressBarColor: "#187d3e",//colour of circular progress bar
                progressBarWidth: 11,//progress bar width
                speed: 10,
                progressvalue: 60,
                showText: true
            });

            $("#preinstall").circularloader({
                progressPercent: 40,
                backgroundColor: "#ae405b",//background colour of inner circle
                fontColor: "#fff",//font color of progress text
                fontSize: "16px",//font size of progress text
                radius: 28,//radius of circle
                progressBarBackground: "#cc5f7e",//background colour of circular progress Bar
                progressBarColor: "#912046",//colour of circular progress bar
                progressBarWidth: 11,//progress bar width
                speed: 10,
                progressvalue: 40,
                showText: true
            });

            $("#migration").circularloader({
                progressPercent: 50,
                backgroundColor: "#0e83ab",//background colour of inner circle
                fontColor: "#fff",//font color of progress text
                fontSize: "16px",//font size of progress text
                radius: 28,//radius of circle
                progressBarBackground: "#1dafd2",//background colour of circular progress Bar
                progressBarColor: "#05667c",//colour of circular progress bar
                progressBarWidth: 11,//progress bar width
                speed: 10,
                progressvalue: 50,
                showText: true
            });
        },


        /* =============== Grid Stack ===================== */
        grid_stack: function () {
            var options = {
                float: false,
                cellHeight: 1,
                animate: true,
                width: 12,
                verticalMargin: 20,
                draggable: {
                    handle: '.title-bar',
                    scroll: true
                }

            };
            $('.grid-stack').gridstack(options);

            $('.grid-stack').on('gsresizestop', function (event, elem) {
                if ($(this).find('div[data-highcharts-chart]')) {
                    setTimeout(function () {
                        $("div[data-highcharts-chart]").each(function (index) {
                            $(this).highcharts().reflow();
                        });
                    }, 300);
                }

                var elem = angular.element(document.querySelector('[ng-app]'));
                var injector = elem.injector();
                var $rootScope = injector.get('$rootScope');
                $rootScope.resizeCharts();

            });




            new function () {
                this.serializedData = [
                  // x: node.x,
                  // y: node.y,
                  // width: node.width,
                  // height: node.height,
                  // el: $(el)
                ];

                this.grid = $('.grid-stack').data('gridstack');

                this.loadGrid = function () {
                    this.grid.removeAll();
                    var items = GridStackUI.Utils.sort(this.serializedData);
                    _.each(items, function (node) {
                        if (node.el)
                            this.grid.addWidget(node.el, node.x, node.y, node.width, node.height);
                    }.bind(this));

                    // var items = GridStackUI.Utils.sort(this.serializedData);
                    // _.each(items, function (node) {
                    //     // this.grid.addWidget($('<div><div class="grid-stack-item-content" /></div>'),
                    //   this.grid.update(node,node.x, node.y, node.width, node.height);
                    // }.bind(this));
                    return false;
                }.bind(this);

                this.saveGrid = function () {
                    this.serializedData = _.map($('.grid-stack > .grid-stack-item:visible'), function (el) {
                        el = $(el);
                        var node = el.data('_gridstack_node');
                        return {
                            x: node.x,
                            y: node.y,
                            width: node.width,
                            height: node.height,
                            el: $(el)
                        };
                    });
                    console.log(this.serializedData);
                    $('#saved-data').val(JSON.stringify(this.serializedData, null, '    '));
                    return false;
                }.bind(this);

                this.clearGrid = function () {
                    this.grid.removeAll();
                    // var items = GridStackUI.Utils.sort(this.serializedData);
                    // _.each(items, function (node) {
                    //   console.log(node);
                    //     // this.grid.addWidget($('<div><div class="grid-stack-item-content" /></div>'),
                    //   this.grid.update(node,0, 0, 0, 0);
                    // }.bind(this));
                    return false;
                }.bind(this);

                $('#save-grid').click(this.saveGrid);
                $('#load-grid').click(this.loadGrid);
                $('#clear-grid').click(this.clearGrid);
            };

        },


        /* =============== Scrollbar  ===================== */
        scrollbar: function () {
            function scrollbar() {
                $('body .scrollbar').enscroll({
                    showOnHover: true,
                    scrollIncrement: 30,
                    easingDuration: 300,
                    addPaddingToPane: false,
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    horizontalScrolling: true,
                    horizontalTrackClass: 'horizontaltrack4',
                    horizontalHandleClass: 'horizontalhandle4'
                });
            }
            scrollbar();
            $('.panel-box').mousedown(function () { $(".scrollbar").enscroll('destroy') })
            $('.panel-box').mouseup(function () { scrollbar() });
        },


        /* =============== Map  ===================== */
        map: function () {
            var map = '';

            // Creating a LatLng object containing the coordinate for the center of the map
            var latlng = new google.maps.LatLng(53.385846, -1.471385);

            // Creating an object literal containing the properties we want to pass to the map
            var options = {
                zoom: 15, // This number can be set to define the initial zoom level of the map
                center: latlng,
            };
            // Calling the constructor, thereby initializing the map
            map = new google.maps.Map(document.getElementById('map_div'), options);
        },

        /* =============== Canvas Chart  ===================== */
        CanvasChart: function () {
            $("div[canvascharts]").each(function () {
                var chartHeight = $(this).find("canvas").height();
                $(this).css({ "height": chartHeight })
            });
        },


        /* =============== Canvas Chart  ===================== */
        CanvasChartRender: function () {
            $('body').on('click', '.column-size a', function (e) {
                setTimeout(function () {
                    var grabController = $(e.target).parents('.widget').find("div[canvascharts]");
                    angular.element(grabController).scope().chart.render();
                    return false;
                }, 300);
            });
            $('body').on('click', '.menu-icon', function (e) {
                setTimeout(function () {
                    var elem = angular.element(document.querySelector('[ng-app]'));
                    var injector = elem.injector();
                    var $rootScope = injector.get('$rootScope');
                    $rootScope.resizeCharts();
                }, 300);
            });
        },



        /* =============== Alert Auto Hide  ===================== */
        AlertAutoHide: function () {
            $(".alert.hidden").hide();
            $("body").on("click", ".showAlert", function () {
                var alert = $(this).attr("data-target");
                $(".alert#" + alert).fadeIn().delay(5000).queue(function (n) {
                    $(this).fadeOut(); n();
                });
            });
        },


        /* =============== Selected Filters ===================== */
        ApplyFilters: function () {
            function selectedFilters(element) {
                $(".multiselect-selected-text").each(function () {
                    $(this).addClass("getValue");
                });
                var list_array = [];
                $(element).parents("ul").children("li").each(function () {
                    if ($(this).find("span.getValue").text()) {
                        list_array.push("<span>" + $(this).find("span.getValue").text() + "</span>");
                    }
                });

                $(element).parents(".title-bar").find(".selected-filters").children().remove();
                $(element).parents(".title-bar").find(".selected-filters").append(list_array);
            }

            $(".filter-btn").each(function () {
                selectedFilters(this);
            });

            $("body").on("click", ".filter-btn", function () {
                selectedFilters(this);
            });
        },




        /* =============== Theme Select ===================== */
        themeSelect: function () {
            $(".themes-list li a").on("click", function () {
                var getTheme = $(this).attr("data-theme");
                $("body").attr("class", getTheme);
                $(this).parent().siblings().find('a').removeClass("active");
                $(this).addClass("active");
            });
        },


        /* =============== Color Picker Position ===================== */
        colorPickerPosition: function () {       // Color Picker Position
            function getOffset(el) {
                el = el.getBoundingClientRect();
                return {
                    left: el.left + window.scrollX,
                    top: el.top + window.scrollY
                }
            }

            $("body").on("focus", '.bgcolor', function () {
                var posTop = getOffset(this).top - 35;
                var posLeft = getOffset(this).left;
                $(this).next('.colorpickerwrapper').css({
                    "top": posTop,
                    "left": posLeft
                });
            });
        },


        /* =============== Title Update ===================== */
        TitleUpdate: function () {
            $("body").on("click", ".title-bar h3 span", function () {
                $(this).hide();
                $(this).parent().find("input").show().focus();
            });

            $("body").on("focus", ".title-bar h3 input", function () {
                $(this).on('keyup', function (e) {
                    if (e.keyCode == 13) {
                        $(this).hide();
                        $(this).parent().find('span').show();
                        $(this).parent().find("span.cache-value").hide();
                    }
                });
            });

            $("body").on("blur", ".title-bar h3 input", function () {
                $(this).parent().find("span.update-name").show();
                $(this).hide();
                $(this).parent().find("span.cache-value").hide();
            });
        },


        /* =============== Title Cookies ===================== */
        titleCookies: function () {
            $pageName = $(".page").attr("data-pagename");
            $("body").on("blur change", "div[data-pagename='" + $pageName + "'] .title-bar h3 input", function () {
                $titlecookie = "{";
                $(".title-bar h3 input").each(function (i, e) {
                    $titlecookie += '"widget-title_' + i + '":"' + e.value + '",';
                });
                $titlecookie = $titlecookie.substring(0, $titlecookie.length - 1);
                $titlecookie += '}';
                $.cookie("widgets_titles" + $pageName, $titlecookie)
            });

            $parsed_data = null;
            if ($.cookie("widgets_titles" + $pageName)) {
                $parsed_data = JSON.parse($.cookie("widgets_titles" + $pageName));
                $("div[data-pagename='" + $pageName + "'] .title-bar h3 input").each(function (i, e) {
                    $(e).val($parsed_data['widget-title_' + i]);
                });
                $(".title-bar h3 span.cache-value").each(function (i, e) {
                    $(e).text($parsed_data['widget-title_' + i]);
                });
            }

            $(".title-bar").each(function () {
                if ($(this).find("span.cache-value").length > 0 && $(this).find("span.cache-value")[0].innerText) {
                    $(this).find("span.cache-value").show();
                    $(this).find("span.update-name").hide();
                }
            });




        },


        /* =============== Color Cookies ===================== */
        colorCookies: function () {
            $pgname = $(".page").attr("data-pagename");
            $("body").on("change paste keyup", "div[data-pagename='" + $pgname + "'] input.bgcolor", function () {
                $colorcookie = "{";
                $(".title-bar").each(function (i, e) {
                    $colorcookie += '"widget-color_' + i + '":"' + $(this).attr("style") + '",';
                });
                $colorcookie = $colorcookie.substring(0, $colorcookie.length - 1);
                $colorcookie += '}';

                $.cookie("color_cookies_" + $pgname, $colorcookie)

            });


            if ($.cookie("color_cookies_" + $pgname)) {
                $parsed_colordata = JSON.parse($.cookie("color_cookies_" + $pgname));
                $("div[data-pagename='" + $pgname + "'] .title-bar").each(function (i, e) {
                    $(e).attr("style", $parsed_colordata['widget-color_' + i]);
                });
            }

        },



        /* =============== Theme Change Cookies ===================== */
        themeChange: function () {
            $("body").on("click", ".themes-list  a", function () {
                $theme = $(this).attr("data-theme");
                console.log($theme);
                $.cookie("myTheme", $theme);
                return false;
            });

            //$("body").attr("class",$.cookie("myTheme"));
        },




        addWidget: function () {

            // Valdate Popup Form and Add Main Div
            $loadpanel = [{
                "type": "widgetOfTable",

                "code": `<div class="grid-stack-item" data-gs-resize-handles="e,se,s"><div  id="table_settings" class="grid-stack-item-content"> <div class='widget' > <div class='title-bar' style='background:{{titlebg_01}};'> <h3><i class='fa fa-table'></i> <span class='update-name'>{{widgetname_01}}</span> <span class='cache-value'></span> <input ng-model='widgetname_01' ng-init="widgetname_01='Table With Big Headings'" type='text' /> </h3> <div class='buttons-sets'> <a class='custom-btn icon-only remove-btn'><i class='fa fa-times'></i><span class='custom-tooltip'>Remove</span></a> <a class='custom-btn icon-only collapse-btn'><i class='fa fa-minus'></i><span class='custom-tooltip'>Collapse</span></a> <a class='custom-btn icon-only opt-btn'><i class='fa fa-columns'></i><span class='custom-tooltip'>Panel Size</span></a> <ul class='opt-dropdown' > <li class='has-children'><a href='#' title=''>Panel Color</a> <ul class='panel-color'> <li> <a href='#' title=''><input class='bgcolor'  type='text'  value='#fff' ng-model='titlebg_01'  ng-init="titlebg_01 = '#fff'"  /> <div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul> </li> </ul> </div> </div> <div class='content-wrapper'><div class="center-icon"> <i class="fa fa-table"></i>  </div></div></div> </div> </div>`,

                "settings": `<div class="toggle-item widget-settings table_settings"> <h2><i class="fa fa-cog"></i> Settings</h2> <div class="content" style="display: none" id="settings"><form> <div class="row"> <div class="col-lg-12 col-md-12"> <label>Select Data From DB</label> <select class="full"> <option>Table Data 1</option> <option>Table Data 2</option> <option>Table Data 3</option> </select> </div> <div class="col-lg-6 col-md-12"> <input id="checkbox1" name="checkbox1" value="bordered"  type="checkbox" /> <label for="checkbox1">Borders</label> </div> <div class="col-lg-6 col-md-12"> <input id="checkbox2" name="checkbox2" value="stripped"  type="checkbox" /> <label for="checkbox2">Stripped</label> </div> <div class="col-lg-12 col-md-12"> <label class="label">Style</label> <label class="radio"> <input type="radio" value="" name="radio" checked="checked"> <i></i>Simple</label> <label class="radio"> <input type="radio" value="Dark" name="radio"> <i></i>Dark</label> <label class="radio"> <input type="radio" value="coloured" name="radio"> <i></i>Coloured</label> </div> </div> </form></div> </div>`,

                "advanced_settings": `<div class="table_settings toggle-item widget-settings"> <h2><i class="fa fa-cog"></i> Advanced Settings</h2> <div class="content" style="display: none" id="settings"><form> <div class="row"> <div class="col-lg-12 col-md-12"> <label>Headings Font Size In px</label> <input class="theme4" ionmin="6" ionmax="30" ionfrom="13"  rangeslider /> </div> <div class="col-lg-12 col-md-12"> <label>Rows Font Size In px</label> <input class="theme4" ionmin="6" ionmax="24" ionfrom="13"  rangeslider /> </div> <div class="col-lg-12 col-md-12 "> <label>Custom Heading Background</label> <input class="bgcolor"  type="text"  value="#fafafa"  /> <div class="colorpickerwrapper"><div colorpicker></div></div> </div> </div><fieldset><div class="row">  <div class="col-lg-12 col-md-12"> <input id="dt" name=""  type="checkbox" /> <label for="dt">Make Data Table</label> </div> </div>  <div class="dt-options"> <div class="row">  <div class="col-lg-6 col-md-12"> <label>Pages</label> <input type="text" class="number" maskedinput /> </div> <div class="col-lg-6 col-md-12"> <label>Show</label> <select class="full"><option>10</option><option>20</option><option>50</option></select>  </div> <div class="col-lg-12 col-md-12"> <input id="searchable" name=""  type="checkbox" /> <label for="searchable">Searchable</label> </div> </div>  </div> </fieldset> </form></div></div>`,

                "query_builder": `<input class="switch-qb" type="checkbox" checked  style="position:absolute;z-index: 1;" />
                                  <div querybuilder class="querybuilder-wrapper table_settings">
                                     <div class="single-query querybuilder-inner" style="display:none">
                                           <div class="select-table"> <h3 class="simple-title">Select Table</h3> <form> <div class="row"> <div class="col-lg-12"> <select class="full"> <option value="">Category 1</option> <option value="">Category 2</option> <option value="">Category 3</option> <option value="">Category 4</option> <option value="">Category 5</option> </select> </div> <div class="col-lg-12"> <ul class="columns-list"> <li><input type="checkbox" id="qb13" name="qb13"><label for="qb13">Column 1</label></li> <li><input type="checkbox" id="qb14" name="qb14"><label for="qb14">Column 2</label></li> <li><input type="checkbox" id="qb15" name="qb15"><label for="qb15">Column 3</label></li> <li><input type="checkbox" id="qb16" name="qb16"><label for="qb16">Column 4</label></li> <li> <input type="checkbox" id="qb17" name="qb17"><label for="qb17">Column 5</label></li> <li><input type="checkbox" id="qb18" name="qb18"><label for="qb18">Column 6</label></li> </ul> </div> </div> </form> </div> <div class="selected-columns"> <div class="columns-head"> <h3 class="simple-title">Selected Columns</h3> <div class="buttons-set"> <a class="custom-btn color11 delete-selected-rows" href="#" title=""><i class="fa fa-times"></i> Delete Selected</a> <a class="custom-btn color11 delete-all-rows" href="#" title=""><i class="fa fa-times"></i> Delete All</a> </div> </div> <div class="table-grid"> <div class="tg-head"> <span class="small"></span> <span class="small"><input class="select-all-row" type="checkbox" /></span> <span>Columns</span> <span>Alias</span> <span>Function</span> <span class="small">Group</span> <span>Sort</span><span class="small"></span>  </div> <div class="selected-rows"> </div> </div> </div> <div class="create-query"> <h3 class="simple-title">Create Query</h3> <div id="builder" class="bs_comp"> </div> </div> <div class="query-area"><textarea></textarea></div>
                                      </div>
                                      <div class="multi-query querybuilder-inner">
                                          <div class="schema-switch"><span><input type="checkbox"  switch value="0" ></span> Show Schema Designer</div>
                                          <div class="select-table"> <h3 class="simple-title">Select Table</h3> <form> <div class="row"> <div class="col-lg-12"> <div tagsdropdown class="dbTables"> <select style="display:none"  name="" multiple placeholder="Select"> </select> </div> </div> <div class="col-lg-12"><div jstree class="dbTables"></div></div> </div> </form> </div> <div class="selected-columns"> <div class="columns-head"> <h3 class="simple-title">Selected Columns</h3> <div class="buttons-set"> <a class="custom-btn color11 delete-selected-rows" href="#" title=""><i class="fa fa-times"></i> Delete Selected</a> <a class="custom-btn color11 delete-all-rows" href="#" title=""><i class="fa fa-times"></i> Delete All</a> </div> </div> <div class="table-grid"> <div class="tg-head"> <span class="small"></span> <span class="small"><input class="select-all-row" type="checkbox" /></span> <span>Columns</span> <span>Alias</span> <span>Function</span> <span class="small">Group</span> <span>Sort</span> <span class="small"></span> </div> <div class="selected-rows"> </div> </div> </div> <div class="create-query"> <h3 class="simple-title">Create Query</h3> <div id="builder" class="bs_comp"> </div> </div> <div class="query-area"><textarea></textarea></div>
                                          <div class="airview-schema-designer"> </div>
                                      </div>
                                  </div>`
            },
                   {
                       "type": "widgetOfChart",
                       "code": `<div class="grid-stack-item" data-gs-resize-handles="e,se,s"><div  id="chart_settings" class="grid-stack-item-content"><div class='widget' > <div class='title-bar' style='background:{{titlebg_08}};'> <h3><i class='fa fa-table'></i> <span class='update-name'>{{widgetname_08}}</span> <span class='cache-value'></span> <input ng-model='widgetname_08' ng-init="widgetname_08='Charts'" type='text' /> </h3> <div class='buttons-sets'> <a class='custom-btn icon-only remove-btn'><i class='fa fa-times'></i><span class='custom-tooltip'>Remove</span></a> <a class='custom-btn icon-only collapse-btn'><i class='fa fa-minus'></i><span class='custom-tooltip'>Collapse</span></a> <a class='custom-btn icon-only opt-btn'><i class='fa fa-columns'></i><span class='custom-tooltip'>Panel Size</span></a> <ul class='opt-dropdown' > <li class='has-children'><a href='#' title=''>Panel Color</a> <ul class='panel-color'> <li> <a href='#' title=''><input class='bgcolor'  type='text'  value='#fff' ng-model='titlebg_08'  ng-init="titlebg_08 = '#fff'"  /> <div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul> </li> </ul>

                                  <a class='custom-btn icon-only open-filters'><i class='fa fa-filter'></i><span class='custom-tooltip'>Filters</span></a>
                                  <div class="filters-drop">
                                    <h5>Chart Filters</h5>
                                    <div jstree></div>


                                  </div>

                                  </div> </div> <div class='content-wrapper'> <div class="center-icon"> <i class="far fa-chart-bar"></i>  </div> </div></div> </div> </div>`,

                       "settings": `<div class="toggle-item widget-settings chart_settings"> <h2><i class="fa fa-cog"></i> Settings</h2> <div class="content" id="settings"> <form> <div class="row"> <div class="col-lg-12 col-md-12"> <label>Chart Name</label> <select class="full"> <option>Chart Data 1</option> <option>Chart Data 2</option> <option>Chart Data 3</option> </select> </div> <div class="col-lg-12 col-md-12"> <label>Chart Type</label> <select class="full"> <option>Bar Chart</option> <option>Line Chart</option> <option>Donut Chart</option> <option>Pie Chart</option> <option>Spider Chart</option> </select> </div> <div class="col-lg-12 col-md-12"> <div class="toggle" toggle> <div class="toggle-item"> <h2>Title Settings</h2> <div class="content"> <div class="row"> <div class="col-lg-12 col-md-12"> <label>Chart Title</label> <input type="text" placeholder="Enter Title"/> </div><div class="col-lg-12 col-md-12 inline"> <label>Font Size</label> <input class="theme4" ionmin="6" ionmax="30" ionfrom="13" rangeslider/> </div><div class="col-lg-12 col-md-12 inline"> <label>Font Color</label> <input class="bgcolor" type="text" value="#fafafa"/> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-12 col-md-12 inline"> <label>Font Family</label> <select> <option>arial</option> <option>tahoma</option> <option>verdana</option> </select> </div><div class="col-lg-6 col-md-12"> <input id="titlebold" name="titlebold" type="checkbox"/> <label for="titlebold">Bold</label> </div><div class="col-lg-6 col-md-12"> <input id="titleitalic" name="titleitalic" type="checkbox"/> <label for="titleitalic">Italic</label> </div></div></div></div><div class="toggle-item"> <h2>Subtitle Settings</h2> <div class="content"> <div class="row"> <div class="col-lg-12 col-md-12"> <label>Chart Subtitle</label> <input type="text" placeholder="Enter Subtitle"/> </div><div class="col-lg-12 col-md-12 inline"> <label>Font Size</label> <input class="theme4" ionmin="6" ionmax="30" ionfrom="13" rangeslider/> </div><div class="col-lg-12 col-md-12 inline"> <label>Font Color</label> <input class="bgcolor" type="text" value="#fafafa"/> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-12 col-md-12 inline"> <label>Font Family</label> <select> <option>arial</option> <option>tahoma</option> <option>verdana</option> </select> </div><div class="col-lg-6 col-md-12"> <input id="subtitlebold" name="subtitlebold" type="checkbox"/> <label for="subtitlebold">Bold</label> </div><div class="col-lg-6 col-md-12"> <input id="subtitleitalic" name="subtitleitalic" type="checkbox"/> <label for="subtitleitalic">Italic</label> </div></div></div></div><div class="toggle-item"> <h2>Legend Settings</h2> <div class="content"> <div class="row"> <div class="col-lg-12 col-md-12"> <input id="chartlegend" name="chartlegend" type="checkbox"/> <label for="chartlegend">On</label> </div><div class="col-lg-12 col-md-12"> <label>Legend</label> <input type="text" placeholder="Enter Legend"/> </div><div class="col-lg-12 col-md-12 inline"> <label>Position</label> <select> <option>left</option> <option>right</option> <option>top</option> <option>bottom</option> </select> </div><div class="col-lg-12 col-md-12 inline"> <label>Font Size</label> <input class="theme4" ionmin="6" ionmax="30" ionfrom="13" rangeslider/> </div><div class="col-lg-12 col-md-12 inline"> <label>Font Color</label> <input class="bgcolor" type="text" value="#fafafa"/> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-12 col-md-12 inline"> <label>Font Family</label> <select> <option>arial</option> <option>tahoma</option> <option>verdana</option> </select> </div><div class="col-lg-6 col-md-12"> <input id="legendtitlebold" name="legendtitlebold" type="checkbox"/> <label for="legendtitlebold">Bold</label> </div><div class="col-lg-6 col-md-12"> <input id="legendtitleitalic" name="legendtitleitalic" type="checkbox"/> <label for="legendtitleitalic">Italic</label> </div></div></div></div><div class="toggle-item"> <h2>X Axis</h2> <div class="content"> <div class="row"> <div class="col-lg-12 col-md-12"> <label>X Axis Title</label> <input type="text" placeholder="Enter X Axis Title"/> </div><div class="col-lg-12 col-md-12"> <label>Title Font Size</label> <input class="theme4" ionmin="6" ionmax="30" ionfrom="13" rangeslider/> </div><div class="col-lg-12 col-md-12"> <label>Title Font Color</label> <input class="bgcolor" type="text" value="#fafafa"/> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-6 col-md-12"> <input id="xaxisbold" name="xaxisbold" type="checkbox"/> <label for="xaxisbold">Bold</label> </div><div class="col-lg-6 col-md-12"> <input id="xaxisitalic" name="xaxisitalic" type="checkbox"/> <label for="xaxisitalic">Italic</label> </div><div class="col-lg-12 col-md-12"> <label>Label Font Size</label> <input class="theme4" ionmin="6" ionmax="30" ionfrom="13" rangeslider/> </div><div class="col-lg-12 col-md-12"> <label>Label Font Color</label> <input class="bgcolor" type="text" value="#fafafa"/> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-6 col-md-12"> <label>Prefix</label> <input type="text" placeholder="Prefix"/> </div><div class="col-lg-6 col-md-12"> <label>Suffix</label> <input type="text" placeholder="Suffix"/> </div></div></div></div><div class="toggle-item"> <h2>Y Axis</h2> <div class="content"> <div class="row"> <div class="col-lg-12 col-md-12"> <label>Y Axis Title</label> <input type="text" placeholder="Enter Y Axis Title"/> </div><div class="col-lg-12 col-md-12 "> <label>Title Font Size</label> <input class="theme4" ionmin="6" ionmax="30" ionfrom="13" rangeslider/> </div><div class="col-lg-12 col-md-12"> <label>Title Font Color</label> <input class="bgcolor" type="text" value="#fafafa"/> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-6 col-md-12"> <input id="yaxisbold" name="yaxisbold" type="checkbox"/> <label for="yaxisbold">Bold</label> </div><div class="col-lg-6 col-md-12"> <input id="yaxisitalic" name="yaxisitalic" type="checkbox"/> <label for="yaxisitalic">Italic</label> </div><div class="col-lg-12 col-md-12"> <label>Label Font Size</label> <input class="theme4" ionmin="6" ionmax="30" ionfrom="13" rangeslider/> </div><div class="col-lg-12 col-md-12 "> <label>Label Font Color</label> <input class="bgcolor" type="text" value="#fafafa"/> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-6 col-md-12"> <label>Prefix</label> <input type="text" placeholder="Prefix"/> </div><div class="col-lg-6 col-md-12"> <label>Suffix</label> <input type="text" placeholder="Suffix"/> </div></div></div></div></div></div>  </div></form> </div> </div>`,



                       "advanced_settings": `<div class="toggle-item widget-settings chart_settings"> <h2><i class="fa fa-cog"></i> Advanced Settings</h2> <div class="content" style="display:none" id="settings"> <div class="single-chart"> <form> <div class="row"> <div class="col-lg-12 col-md-12"> <label>X axis</label> <select class="full"> <option>Axis 1</option> <option>Axis 2</option> <option>Axis 3</option> </select> </div> <div class="col-lg-12 col-md-12"> <label>Y axis</label> <select class="full"> <option>Axis 1</option> <option>Axis 2</option> <option>Axis 3</option> </select> </div>  </div>  </form></div> <div class="multi-series-chart"><form> <div class="row"> <div class="col-lg-12 col-md-12 clonable-part"><label><strong>X axis</strong> <a class="clone-me custom-btn color1" href="#" title=""><i class="fa fa-plus-circle"></i> Add New</a></label><div class="clonable"> <select class="full"> <option>Axis 1</option> <option>Axis 2</option> <option>Axis 3</option> </select> </div> <div class="clonable"> <a class="remove-it" href="#" title><i class="fa fa-minus"></i></a> <select class="full"> <option>Axis 1</option> <option>Axis 2</option> <option>Axis 3</option> </select> </div>   </div>  <div class="col-lg-12 col-md-12 clonable-part"><label><strong>Y axis</strong> <a class="clone-me custom-btn color1" href="#" title=""><i class="fa fa-plus-circle"></i> Add New</a></label><div class="clonable"><div class="row narrow"> <div class="col-lg-6 col-md-12"> <select class="full"><option>Column 1</option> <option>Column 1</option> <option>Column 1</option> </select></div> <div class="col-lg-6 col-md-12"> <select class="full"> <option>Chart 1</option> <option>Chart 1</option> <option>Chart 1</option> </select></div> <div class="col-lg-6 col-md-12"> <input class="bgcolor" type="text" value="#fafafa"/> <div class="colorpickerwrapper"> <div colorpicker></div></div> </div> <div class="col-lg-6 col-md-12"> <select class="full"><option>Seconday</option><option>Primary</option></select></div>  </div></div> <div class="clonable"> <a class="remove-it" href="#" title><i class="fa fa-minus"></i></a> <div class="row narrow"> <div class="col-lg-6 col-md-12"> <select class="full"><option>Column 1</option> <option>Column 1</option> <option>Column 1</option> </select></div> <div class="col-lg-6 col-md-12">  <select class="full"> <option>Chart 1</option> <option>Chart 1</option> <option>Chart 1</option> </select></div><div class="col-lg-6 col-md-12"> <input class="bgcolor" type="text" value="#fafafa"/> <div class="colorpickerwrapper"> <div colorpicker></div></div> </div> <div class="col-lg-6 col-md-12"> <select class="full"><option>Seconday</option><option>Primary</option></select></div> </div></div>  </div> </form></div>  </div> </div>`,

                       "query_builder": `<input class="switch-qb" type="checkbox" checked style="position:absolute" />
                                  <div querybuilder class="querybuilder-wrapper chart_settings">
                                      <div class="single-query querybuilder-inner" style="display:none">
                                            <div class="select-table"> <h3 class="simple-title">Select Table</h3> <form> <div class="row"> <div class="col-lg-12"> <select class="full"> <option value="">Category 1</option> <option value="">Category 2</option> <option value="">Category 3</option> <option value="">Category 4</option> <option value="">Category 5</option> </select> </div> <div class="col-lg-12"> <ul class="columns-list"> <li><input type="checkbox" id="qb13" name="qb13"><label for="qb13">Column 1</label></li> <li><input type="checkbox" id="qb14" name="qb14"><label for="qb14">Column 2</label></li> <li><input type="checkbox" id="qb15" name="qb15"><label for="qb15">Column 3</label></li> <li><input type="checkbox" id="qb16" name="qb16"><label for="qb16">Column 4</label></li> <li> <input type="checkbox" id="qb17" name="qb17"><label for="qb17">Column 5</label></li> <li><input type="checkbox" id="qb18" name="qb18"><label for="qb18">Column 6</label></li> </ul> </div> </div> </form> </div> <div class="selected-columns"> <div class="columns-head"> <h3 class="simple-title">Selected Columns</h3> <div class="buttons-set"> <a class="custom-btn color11 delete-selected-rows" href="#" title=""><i class="fa fa-times"></i> Delete Selected</a> <a class="custom-btn color11 delete-all-rows" href="#" title=""><i class="fa fa-times"></i> Delete All</a> </div> </div> <div class="table-grid"> <div class="tg-head"> <span class="small"></span> <span class="small"><input class="select-all-row" type="checkbox" /></span> <span>Columns</span> <span>Alias</span> <span>Function</span> <span class="small">Group</span> <span>Sort</span><span class="small"></span>  </div> <div class="selected-rows"> </div> </div> </div> <div class="create-query"> <h3 class="simple-title">Create Query</h3> <div id="builder" class="bs_comp"> </div> </div> <div class="query-area"><textarea></textarea></div>
                                       </div>
                                       <div class="multi-query querybuilder-inner">
                                            <div class="schema-switch"><span><input type="checkbox"  switch value="0" ></span> Show Schema Designer</div>
                                            <div class="select-table"> <h3 class="simple-title">Select Table</h3> <form> <div class="row"> <div class="col-lg-12"> <div tagsdropdown class="dbTables"> <select style="display:none"  name="" multiple placeholder="Select"> </select> </div> </div> <div class="col-lg-12"><div jstree class="dbTables"></div></div> </div> </form> </div> <div class="selected-columns"> <div class="columns-head"> <h3 class="simple-title">Selected Columns</h3> <div class="buttons-set"> <a class="custom-btn color11 delete-selected-rows" href="#" title=""><i class="fa fa-times"></i> Delete Selected</a> <a class="custom-btn color11 delete-all-rows" href="#" title=""><i class="fa fa-times"></i> Delete All</a> </div> </div> <div class="table-grid"> <div class="tg-head"> <span class="small"></span> <span class="small"><input class="select-all-row" type="checkbox" /></span> <span>Columns</span> <span>Alias</span> <span>Function</span> <span class="small">Group</span> <span>Sort</span> <span class="small"></span> </div> <div class="selected-rows"> </div> </div> </div> <div class="create-query"> <h3 class="simple-title">Create Query</h3> <div id="builder" class="bs_comp"> </div> </div> <div class="query-area"><textarea></textarea></div>
                                            <div class="airview-schema-designer"> </div>
                                       </div>
                                  </div>`
                   },
                    {
                        "type": "widgetOfMap",
                        "code": `<div class="grid-stack-item" data-gs-resize-handles="e,se,s"><div  id="map_settings" class="grid-stack-item-content"><div class='widget' > <div class='title-bar' style='background:{{titlebg_08}};'> <h3><i class='fa fa-table'></i> <span class='update-name'>{{widgetname_08}}</span> <span class='cache-value'></span> <input ng-model='widgetname_08' ng-init="widgetname_08='Maps'" type='text' /> </h3> <div class='buttons-sets'> <a class='custom-btn icon-only remove-btn'><i class='fa fa-times'></i><span class='custom-tooltip'>Remove</span></a> <a class='custom-btn icon-only collapse-btn'><i class='fa fa-minus'></i><span class='custom-tooltip'>Collapse</span></a> <a class='custom-btn icon-only opt-btn'><i class='fa fa-columns'></i><span class='custom-tooltip'>Panel Size</span></a> <ul class='opt-dropdown' > <li class='has-children'><a href='#' title=''>Panel Color</a> <ul class='panel-color'> <li> <a href='#' title=''><input class='bgcolor'  type='text'  value='#fff' ng-model='titlebg_08'  ng-init="titlebg_08 = '#fff'"  /> <div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul> </li> </ul> </div> </div> <div class='content-wrapper'> <div class="center-icon"> <i class="far fa-map"></i>  </div>  </div></div> </div> </div>`,

                        "settings": `<div class="toggle-item widget-settings map_settings"> <h2><i class="fa fa-cog"></i> Settings</h2> <div class="content" style="display: none" id="settings"><form> <div class="row"> <div class="col-lg-12 col-md-12"> <label>Select Data From DB</label> <select class="full"> <option>Map Data 1</option> <option>Map Data 2</option> <option>Map Data 3</option> </select> </div></form></div></div>`,

                        "advanced_settings": `<div class="toggle-item widget-settings map_settings"> <h2><i class="fa fa-cog"></i> Advanced Settings</h2> <div class="content" style="display: none" id="settings"><p>No Settings Available</p></div></div>`,

                        "query_builder": `<input class="switch-qb" type="checkbox" checked style="position:absolute" />
                                    <div querybuilder class="querybuilder-wrapper map_settings">
                                       <div class="single-query querybuilder-inner" style="display:none">
                                             <div class="select-table"> <h3 class="simple-title">Select Table</h3> <form> <div class="row"> <div class="col-lg-12"> <select class="full"> <option value="">Category 1</option> <option value="">Category 2</option> <option value="">Category 3</option> <option value="">Category 4</option> <option value="">Category 5</option> </select> </div> <div class="col-lg-12"> <ul class="columns-list"> <li><input type="checkbox" id="qb13" name="qb13"><label for="qb13">Column 1</label></li> <li><input type="checkbox" id="qb14" name="qb14"><label for="qb14">Column 2</label></li> <li><input type="checkbox" id="qb15" name="qb15"><label for="qb15">Column 3</label></li> <li><input type="checkbox" id="qb16" name="qb16"><label for="qb16">Column 4</label></li> <li> <input type="checkbox" id="qb17" name="qb17"><label for="qb17">Column 5</label></li> <li><input type="checkbox" id="qb18" name="qb18"><label for="qb18">Column 6</label></li> </ul> </div> </div> </form> </div> <div class="selected-columns"> <div class="columns-head"> <h3 class="simple-title">Selected Columns</h3> <div class="buttons-set"> <a class="custom-btn color11 delete-selected-rows" href="#" title=""><i class="fa fa-times"></i> Delete Selected</a> <a class="custom-btn color11 delete-all-rows" href="#" title=""><i class="fa fa-times"></i> Delete All</a> </div> </div> <div class="table-grid"> <div class="tg-head"> <span class="small"></span> <span class="small"><input class="select-all-row" type="checkbox" /></span> <span>Columns</span> <span>Alias</span> <span>Function</span> <span class="small">Group</span> <span>Sort</span><span class="small"></span>  </div> <div class="selected-rows"> </div> </div> </div> <div class="create-query"> <h3 class="simple-title">Create Query</h3> <div id="builder" class="bs_comp"> </div> </div> <div class="query-area"><textarea></textarea></div>
                                        </div>
                                        <div class="multi-query querybuilder-inner">
                                            <div class="schema-switch"><span><input type="checkbox"  switch value="0" ></span> Show Schema Designer</div>
                                            <div class="select-table"> <h3 class="simple-title">Select Table</h3> <form> <div class="row"> <div class="col-lg-12"> <div tagsdropdown class="dbTables"> <select style="display:none"  name="" multiple placeholder="Select"> </select> </div> </div> <div class="col-lg-12"><div jstree class="dbTables"></div></div> </div> </form> </div> <div class="selected-columns"> <div class="columns-head"> <h3 class="simple-title">Selected Columns</h3> <div class="buttons-set"> <a class="custom-btn color11 delete-selected-rows" href="#" title=""><i class="fa fa-times"></i> Delete Selected</a> <a class="custom-btn color11 delete-all-rows" href="#" title=""><i class="fa fa-times"></i> Delete All</a> </div> </div> <div class="table-grid"> <div class="tg-head"> <span class="small"></span> <span class="small"><input class="select-all-row" type="checkbox" /></span> <span>Columns</span> <span>Alias</span> <span>Function</span> <span class="small">Group</span> <span>Sort</span> <span class="small"></span> </div> <div class="selected-rows"> </div> </div> </div> <div class="create-query"> <h3 class="simple-title">Create Query</h3> <div id="builder" class="bs_comp"> </div> </div> <div class="query-area"><textarea></textarea></div>
                                            <div class="airview-schema-designer"> </div>
                                        </div>
                                   </div>`
                    },
                    {
                        "type": "widgetOfDefault",
                        "code": `<div class="grid-stack-item" data-gs-resize-handles="e,se,s"><div  id="default_settings" class="grid-stack-item-content"> <div class='widget' > <div class='title-bar' style='background:{{titlebg_08}};'> <h3><i class='fa fa-table'></i> <span class='update-name'>{{widgetname_08}}</span> <span class='cache-value'></span> <input ng-model='widgetname_08' ng-init="widgetname_08='Default Template'" type='text' /> </h3> <div class='buttons-sets'> <a class='custom-btn icon-only remove-btn'><i class='fa fa-times'></i><span class='custom-tooltip'>Remove</span></a> <a class='custom-btn icon-only collapse-btn'><i class='fa fa-minus'></i><span class='custom-tooltip'>Collapse</span></a> <a class='custom-btn icon-only opt-btn'><i class='fa fa-columns'></i><span class='custom-tooltip'>Panel Size</span></a> <ul class='opt-dropdown' > <li class='has-children'><a href='#' title=''>Panel Color</a> <ul class='panel-color'> <li> <a href='#' title=''><input class='bgcolor'  type='text'  value='#fff' ng-model='titlebg_08'  ng-init="titlebg_08 = '#fff'"  /> <div class='colorpickerwrapper style2'><div colorpicker></div></div></a> </li> </ul> </li> </ul> </div> </div> <div class='content-wrapper'> <div class="center-icon"> <i class="fa fa-bolt"></i>  </div></div> </div> </div> </div>`,

                        "settings": `<div class="toggle-item widget-settings default_settings"> <h2><i class="fa fa-cog"></i> Settings</h2> <div class="content" id="settings"> <form> <div class="row"> <div class="col-lg-6 col-md-12"> <label class="draggable_element">Sublime</label> </div> <div class="col-lg-6 col-md-12"> <label class="draggable_element">Aircod</label> </div> <div class="col-lg-12 col-md-12 db-label-settings"> <div class="toggle" aircodaccordion> </div> </div> </div> </form> </div> </div>`,

                        "advanced_settings": `<div class="toggle-item widget-settings default_settings"> <h2><i class="fa fa-cog"></i> Advanced Settings</h2> <div class="content" style="display: none" id="settings"><p>No Settings Available</p></div></div>`,

                        "query_builder": `<input class="switch-qb" type="checkbox" checked style="position:absolute" />
                                  <div querybuilder class="querybuilder-wrapper default_settings">
                                      <div class="single-query querybuilder-inner" style="display:none">
                                            <div class="select-table"> <h3 class="simple-title">Select Table</h3> <form> <div class="row"> <div class="col-lg-12"> <select class="full"> <option value="">Category 1</option> <option value="">Category 2</option> <option value="">Category 3</option> <option value="">Category 4</option> <option value="">Category 5</option> </select> </div> <div class="col-lg-12"> <ul class="columns-list"> <li><input type="checkbox" id="qb13" name="qb13"><label for="qb13">Column 1</label></li> <li><input type="checkbox" id="qb14" name="qb14"><label for="qb14">Column 2</label></li> <li><input type="checkbox" id="qb15" name="qb15"><label for="qb15">Column 3</label></li> <li><input type="checkbox" id="qb16" name="qb16"><label for="qb16">Column 4</label></li> <li> <input type="checkbox" id="qb17" name="qb17"><label for="qb17">Column 5</label></li> <li><input type="checkbox" id="qb18" name="qb18"><label for="qb18">Column 6</label></li> </ul> </div> </div> </form> </div> <div class="selected-columns"> <div class="columns-head"> <h3 class="simple-title">Selected Columns</h3> <div class="buttons-set"> <a class="custom-btn color11 delete-selected-rows" href="#" title=""><i class="fa fa-times"></i> Delete Selected</a> <a class="custom-btn color11 delete-all-rows" href="#" title=""><i class="fa fa-times"></i> Delete All</a> </div> </div> <div class="table-grid"> <div class="tg-head"> <span class="small"></span> <span class="small"><input class="select-all-row" type="checkbox" /></span> <span>Columns</span> <span>Alias</span> <span>Function</span> <span class="small">Group</span> <span>Sort</span><span class="small"></span>  </div> <div class="selected-rows"> </div> </div> </div> <div class="create-query"> <h3 class="simple-title">Create Query</h3> <div id="builder" class="bs_comp"> </div> </div> <div class="query-area"><textarea></textarea></div>
                                       </div>
                                       <div class="multi-query querybuilder-inner">
                                           <div class="schema-switch"><span><input type="checkbox"  switch value="0" ></span> Show Schema Designer</div>
                                           <div class="select-table"> <h3 class="simple-title">Select Table</h3> <form> <div class="row"> <div class="col-lg-12"> <div tagsdropdown class="dbTables"> <select style="display:none"  name="" multiple placeholder="Select"> </select> </div> </div> <div class="col-lg-12"><div jstree class="dbTables"></div></div> </div> </form> </div> <div class="selected-columns"> <div class="columns-head"> <h3 class="simple-title">Selected Columns</h3> <div class="buttons-set"> <a class="custom-btn color11 delete-selected-rows" href="#" title=""><i class="fa fa-times"></i> Delete Selected</a> <a class="custom-btn color11 delete-all-rows" href="#" title=""><i class="fa fa-times"></i> Delete All</a> </div> </div> <div class="table-grid"> <div class="tg-head"> <span class="small"></span> <span class="small"><input class="select-all-row" type="checkbox" /></span> <span>Columns</span> <span>Alias</span> <span>Function</span> <span class="small">Group</span> <span>Sort</span> <span class="small"></span> </div> <div class="selected-rows"> </div> </div> </div> <div class="create-query"> <h3 class="simple-title">Create Query</h3> <div id="builder" class="bs_comp"> </div> </div> <div class="query-area"><textarea></textarea></div>
                                           <div class="airview-schema-designer"> </div>
                                       </div>
                                   </div>`
                    },
                    {
                        "type": "DBColumnLabel",

                        "code": `<div class="db-col-label" draggable="true"><div class="db-label"></div><div class="db-label-val"></div></div>`,

                        "settings": `<div class="toggle-item"> <h2>Title Settings</h2> <div class="content" style="display: none;"> <form> <div class="row"> <div class="col-lg-12 col-md-12 inline"> <label>Font Size</label> <input class="theme4" ionmin="6" ionmax="30" ionfrom="13" rangeslider/> </div><div class="col-lg-12 col-md-12 inline"> <label>Font Color</label> <input class="bgcolor" type="text" value="#fafafa"/> <div class="colorpickerwrapper"> <div colorpicker></div></div></div><div class="col-lg-12 col-md-12 inline"> <label>Font Family</label> <select> <option>arial</option> <option>tahoma</option> <option>verdana</option> </select> </div><div class="col-lg-6 col-md-12"> <input id="titlebold" name="titlebold" type="checkbox"/> <label for="titlebold">Bold</label> </div><div class="col-lg-6 col-md-12"> <input id="titleitalic" name="titleitalic" type="checkbox"/> <label for="titleitalic">Italic</label> </div> </form> </div> </div> </div>`
                    },
                     {
                         "type": "widgetOfTab",

                         "code": `<div class="grid-stack-item" data-gs-resize-handles="e,se,s"><div  id="tabs_settings" class="grid-stack-item-content"> 	<div class="widget"> <div class="title-bar hidden" style="background:{{titlebg_05}};"> <h3><i class="fa fa-edit"></i> <span class="update-name">{{widgetname_05}}</span> <span class="cache-value"></span> <input ng-model="widgetname_05" ng-init="widgetname_05='Tabs Style'" type="text" /> </h3> <div class="buttons-sets"> <a class="custom-btn icon-only remove-btn"><i class="fa fa-times"></i><span class="custom-tooltip">Remove</span></a> <a class="custom-btn icon-only collapse-btn"><i class="fa fa-minus"></i><span class="custom-tooltip">Collapse</span></a> <a class="custom-btn icon-only opt-btn"><i class="fa fa-columns"></i><span class="custom-tooltip">Panel Size</span></a> <ul class="opt-dropdown" > <li class="has-children"><a href="#" title="">Panel Color</a> <ul class="panel-color"> <li> <a href="#" title=""><input class="bgcolor"  type="text"  value="#fff" ng-model="titlebg"  ng-init="titlebg_05 = '#fff'"  /> <div class="colorpickerwrapper style2"><div colorpicker></div></div></a> </li> </ul> </li> </ul> </div> </div> <div class="content-wrapper normal"> <div dynamictabs class="mytab can-add theme1"> <ul class="nav nav-tabs horizontalscrollbar"> <li> <select style="display:none"  class="icon-input" ng-model="selectedicon"> <option ng-repeat="icon in iconsclass" value="{{icon.key}}">{{icon.key}}</option> </select> <input style="display:none" class="newtab_input" type="text" placeholder="Tab Name" ng-model="newtab" ng-init="newtab = 'New Tab'" /> <a style="display:block" class="nav-link" data-toggle="tab" data-target="#newadded"> <i class="font-icon {{selectedicon}}"></i> {{newtab}} </a> <span style="display:none"  class="done-tab custom-btn icon-only"><i class="fa fa-check"></i></span> <span style="display:none"  class="delete-tab custom-btn icon-only"><i class="fa fa-trash"></i></span> </li> <li class="order-last"><a class="add-tab" href="#" title=""><i class="fa fa-plus"></i> New Tab</a></li> </ul><!-- Selectors --> <div class="tab-content "> <div class="tab-pane fade show active tab-canvas" id="newadded"> <div class="center-icon"><i class="fab fa-ioxhost"></i></div>  <div class="grid-stack"> </div> </div> </div> </div> </div></div> </div> </div>`,

                         "settings": `<div class="toggle-item widget-settings  tabs_settings"> <h2>Settings</h2> <div class="content" style="display: none;"> No Settings Available </div> </div> </div>`
                     }
            ];




            /* events fired on the draggable target */
            $(".panel-box").draggable({
                start: function (event, ui) {
                },
                stop: function (event, ui) {
                    $(".panel-box.ui-draggable-dragging").fadeOut(500, function () {
                        $(this).animate({
                            left: 0,
                            top: 0
                        }, 500, function () {
                            $(this).fadeIn(500);
                        });
                    });
                }
            });


            $(".new-template").droppable({
                accept: ".panel-box",
                drop: function (event, ui) {
                    $panelType = $(".panel-box.ui-draggable-dragging").attr("data-type");
                    console.log("New Template");
                    $.each($loadpanel, function (i, e) {
                        if ($loadpanel[i]["type"] == $panelType) {
                            var el = angular.element("[ng-controller='mainController']").scope().compileAngular($loadpanel[i]["code"]);

                            if ($loadpanel[i]["type"] == "widgetOfTab") {
                                var grids = $('.grid-stack').data('gridstack');
                                grids.add_widget(el, 0, 0, 12, 30, true);
                                $('.new-template .grid-stack').gridstack({
                                    float: true,
                                    cellHeight: 10,
                                    animate: false,
                                    width: 12,
                                    verticalMargin: 5
                                });

                            } else {
                                var grids = $('.grid-stack').data('gridstack');
                                grids.add_widget(el, 0, 0, 6, 25, true);
                            }


                            angular.element("[ng-controller='mainController']").scope().simpleCompile($loadpanel[i]["settings"], $(".side-panel .tab-content .tab-pane > .toggle"));
                            angular.element("[ng-controller='mainController']").scope().simpleCompile($loadpanel[i]["advanced_settings"], $(".side-panel .tab-content .tab-pane > .toggle"));
                            angular.element("[ng-controller='mainController']").scope().simpleCompile($loadpanel[i]["query_builder"], $(".side-panel #tab2"));
                        }
                    });

                }
            });






            // Draggable Elements
            function draggableElements() {
                $(".draggable_element").each(function () {
                    $(this).attr('draggable', true);
                });
            }
            draggableElements();


            var labelText;
            /* events fired on the draggable target */
            document.addEventListener("drag", function (event) {
            });

            document.addEventListener("dragstart", function (event) {
                labelText = $(event.target).text();
                $("#default_settings").each(function () {
                    if ($(this).find(".drop-here").length < 1) {
                        $("#default_settings .content-wrapper").html('<div class="drop-here"></div>');
                    }
                    $(".drop-here").addClass('active');
                });
            });

            document.addEventListener("dragend", function (event) {
                $(".drop-here").removeClass('active');
            });

            /* events fired on the drop targets */
            document.addEventListener("dragover", function (event) {
                // prevent default to allow drop
                event.preventDefault();
            });

            document.addEventListener("dragenter", function (event) {
                event.stopPropagation();
            });

            document.addEventListener("dragleave", function (event) {

            });

            document.addEventListener("drop", function (event) {
                event.preventDefault();
                var offset = $(event.target).offset();
                var relativeX = (event.pageX - offset.left);
                var relativeY = (event.pageY - offset.top);

                if ($(event.target).hasClass('drop-here')) {
                    var labelCode = $($.parseHTML($loadpanel[4]["code"]));
                    labelCode.find(".db-label").text(labelText);
                    labelCode.find(".db-label-val").text(labelText);
                    $(event.target).append(labelCode);
                    labelCode.css("left", relativeX + 'px');
                    labelCode.css("top", relativeY + 'px');
                    angular.element("[ng-controller='mainController']").scope().simpleCompile($loadpanel[4]["settings"], $(".widget-settings.default_settings .db-label-settings .toggle"));
                    var newLabelSettings = $(".db-label-settings .toggle .toggle-item:last-child");
                    newLabelSettings.find('h2').text(labelText + ' settings');
                    draggableElements();
                }
                $(".drop-here").removeClass('active');
            });



        },



        dynatree: function () {
            $("html").on("click", function () {
                $("div[dynatree]").removeClass("active");
            });
            $(".dynatree-btn").on("click", function (e) {
                $(this).next("div[dynatree]").toggleClass("active");
            });
            $(".dynatree-drop").on("click", function (e) {
                e.stopPropagation();
            });
        },



        sidepanelfunctions: function () {
            // Update Settings In Side Panel When Click On a Widget
            $(".new-template").on("click", ".widget", function () {
                var wgtSettings = $(this).parent().attr("id");
                $(this).parent().parent().addClass("change-settings").siblings().removeClass("change-settings");
                $.when($(".side-panel .widget-settings").fadeOut()).done(function () {
                    $(".side-panel .widget-settings." + wgtSettings).fadeIn();
                });
                $.when($(".side-panel .querybuilder-wrapper").fadeOut()).done(function () {
                    $(".side-panel .querybuilder-wrapper." + wgtSettings).fadeIn();
                });
            });

            $("body").on("change", "#dt", function () {
                if ($(this).is(":checked")) {
                    $(".dt-options").fadeIn();
                }
                else {
                    $(".dt-options").fadeOut();
                }
            });


            $("body").on("change", "#contained", function () {
                if ($(this).is(":checked")) {
                    $(".new-template").addClass('container');
                }
                else {
                    $(".new-template").removeClass('container');
                }
            });

            $("body").on("change", "#pagecolor", function () {
                $(".content-area").css({ "background": ($(this).val()) });
            });

            /* == Side Panel Button ==*/
            $("body").on("click", ".sidepanel-btn", function () {
                $(this).toggleClass('active').next('.side-panel').toggleClass('active');
                $(".new-template").toggleClass('shrink');
                return false;
            });

            /* == Side Panel Width Variation On Query Builder Select ==*/
            $("body").on("click", ".side-panel", function () {
                setTimeout(function () {
                    if ($('.querybuilder').hasClass('active')) {
                        $(".side-panel").css({ "width": "calc(100% - 240px)" });
                    }
                    else {
                        $(".side-panel").css({ "width": "300px" })
                    }
                }, 300)
            });


            /* == Tabs Function Issue Resolved ==*/
            $(".side-panel ul.nav li a").on("click", function () {
                let target = $(this).attr("data-target");
                $(".side-panel .tab-content").find(".tab-pane" + target).siblings().removeClass('active');
                //$(".tab-content > .tab-pane").removeClass('active');
                //console.log("chalgaya");
                //if (!$(this).hasClass('active')) {
                //    $(".tab-content > .tab-pane").removeClass('active');
                //}
            });



            /* == Side Panel Sticky ==*/
            $(window).on("scroll", function () {
                var scroll = $(window).scrollTop();
                if (scroll > 63) {
                    $(".side-panel, .sidepanel-btn").addClass("sticky");
                } else {
                    $(".side-panel, .sidepanel-btn").removeClass("sticky");
                }
            });


        },


        querybuilder: function () {
            /* == Make Table Rows Sortable With Muuri ==*/
            // setTimeout(function(){
            //         $selectedCols =  new Muuri('.selected-rows', {
            //                 dragEnabled: true,
            //                 dragReleaseEasing: 'ease-out',
            //                 dragHammerSettings: {
            //                         touchAction: 'pan-y'
            //                 },
            //                 dragStartPredicate: (item, hammerEvent) => {
            //                         if (hammerEvent.pointerType === 'mouse') {
            //                                 this.isResolved = true;
            //                                 return true;
            //                         }
            //                         else {
            //                                 if (hammerEvent.deltaTime >= 500) {
            //                                         this.isResolved = true;
            //                                         return true;
            //                                 }
            //                         }
            //
            //                 },
            //                 dragStartPredicate: function (item, event, e) {
            //                         if ($(event.target).is(".moverow")) {
            //                                 return Muuri.ItemDrag.defaultStartPredicate(item, event);
            //                         }
            //                 },
            //                 dragSortPredicate: {
            //                         action: 'swap'
            //                 },
            //                 layout: {
            //                         fillGaps: true
            //                 }
            //         });
            // },1000);

            /* == Randow JSON ==*/
            $QueryBuilderRows = [{
                "code": `<div class="tg-row"> <span class="small moverow"><i class="fa fa-arrows-alt"></i></span> <span class="small"><input class="selectRow" type="checkbox" /></span> <span><input class="tName" readonly type="text" /></span> <span><input type="text" /></span>
                              <span> <select> <option>Option</option> </select> </span> <span class="small"><input type="checkbox" /></span> <span> <select> <option>Option</option> </select> </span> <span class ="small"><a class="clone" href="JavaScript:void(0)" title=""><i class ="far fa-clone"></i></a> </span> </div>`
            }
            ];

            /* == Add and Remove Rows Depending On Checkbox ==*/
            $("body").on("change", ".columns-list li input[type='checkbox']", function () {
                if (this.checked) {
                    var uniqueId = $(this).attr('id');
                    if ($(this).parents('div[querybuilder]').find(".selected-rows .tg-row#" + uniqueId).length < 1) {
                        var newRow = $.parseHTML($QueryBuilderRows[0]['code']);
                        $(newRow).attr("id", uniqueId);
                        console.log(newRow);
                        var tableName = $(this).siblings('label').text();
                        $(newRow).find('.tName').attr('value', tableName);
                        // $selectedCols.add(newRow);
                    }
                } else {
                    var uniqueId = $(this).attr('id');
                    var findRow = $(".selected-rows .tg-row#" + uniqueId);
                    let arr = [];
                    $.each(findRow, function (idx, ele) {
                        arr.push(ele);
                    });
                    // $selectedCols.remove(arr, {removeElements: true});
                }
            });


            /* == Delete All Rows Button ==*/
            $("body").on("click", ".side-panel .delete-all-rows", function () {
                var findRow = $(".selected-rows .tg-row");
                let arr = [];
                $.each(findRow, function (idx, ele) {
                    arr.push(ele);
                    var getId = $(ele).attr('id');
                });
                $(".columns-list li input[type='checkbox'], .side-panel .select-all-row").attr('checked', false);

                $selectedCols.remove(arr, { removeElements: true });
                return false;
            });

            /* == Delete Selected  Rows Button ==*/
            $("body").on("click", ".side-panel .delete-selected-rows", function () {
                let arr = [];
                var thisRow = $(".selected-rows .tg-row .selectRow:checked").parents(".tg-row");
                thisRow.each(function (idx, ele) {
                    arr.push(ele);
                    var getId = $(ele).attr('id');
                    $(".side-panel .select-all-row, .columns-list li input[type='checkbox']#" + getId).attr('checked', false);
                })
                $selectedCols.remove(arr, { removeElements: true });
                return false;
            });

            /* == Delete All  Rows Button ==*/
            $("body").on("click", ".side-panel .select-all-row", function () {
                if ($(this).is(':checked')) {
                    $(".selected-rows .tg-row .selectRow").prop('checked', true);
                } else {
                    $(".selected-rows .tg-row .selectRow").prop('checked', false);
                }
            });

            /* == Clone Row ==*/
            $("body").on("click", "a.clone", function () {
                var thisRow = $(this).parents('.tg-row').clone();
                var parsedRow = [];
                parsedRow.push(thisRow[0]);
                $selectedCols.add(parsedRow);
                return false;
            });

            /* == Switch QueryBuilder To Single Query or Multi Query ==*/
            $("body").on("change", ".switch-qb", function () {
                let activeBuilder = $(".active .querybuilder-wrapper");
                if ($(this).prop('checked') == true) {
                    activeBuilder.find('.single-query').hide();
                    activeBuilder.find('.multi-query').show();
                } else {
                    activeBuilder.find('.single-query').show();
                    activeBuilder.find('.multi-query').hide();
                }
            });

            /* == Schema Switch ==*/
            $("body").on("change", "input.m_switch_check", function () {
                if ($(this).prop('checked') == true) {
                    $(this).parents('.multi-query').find('.airview-schema-designer').addClass('active');
                } else {
                    $(this).parents('.multi-query').find('.airview-schema-designer').removeClass('active');
                }
            });

        },


        dynamic_tabs: function (directiveElement) {

            var tab_count = 0;
            $(directiveElement).on("click", ".add-tab", function () {
                tab_count++;
                angular.element("[ng-controller='mainController']").scope().iconsclass = [];

                // New Tab Code With Icon Input And Tab Input Fields
                var newtab = `
                  <li>
                  <select class="icon-input" ng-model="selectedicon` + tab_count + `">
                      <option ng-repeat="icon in iconsclass" value="{{icon.key}}">{{icon.key}}</option>
                  </select>
                  <input class="newtab_input" type="text" placeholder="Tab Name" ng-model="newtab` + tab_count + `" />
                  <a style="display:none" class="nav-link" data-toggle="tab" data-target="#newadded` + tab_count + `">
                    <i class="font-icon {{selectedicon` + tab_count + `}}"></i>
                    {{newtab` + tab_count + `}}
                  </a>

                  <span class="done-tab custom-btn icon-only"><i class="fa fa-check"></i></span>
  								<span class="delete-tab custom-btn icon-only"><i class="fa fa-trash"></i></span>
                  </li>`;

                $(this).addClass('disabled');
                var parentsTab = $(this).parents('.can-add').find('.nav');

                $.get("assets/js/fontawesome-5.json", function (data) {
                    let tempArr = [];
                    $.each(Object.keys(data), function (i, e) {
                        tempArr.push({ key: e, value: data[e] });
                    });
                    angular.element("[ng-controller='mainController']").scope().iconsclass = tempArr;

                });
                angular.element("[ng-controller='mainController']").scope().simpleCompile(newtab, parentsTab);

                var newtab_content = `<div class="tab-pane fade tab-canvas" id="newadded` + tab_count + `"> <div class="center-icon"><i class="fab fa-ioxhost"></i></div>  <div class="grid-stack"> </div> </i></div>`;
                $(this).parents('.can-add').find('.tab-content').append(newtab_content);
                setTimeout(function () {
                    let height = $("#newadded" + tab_count).parents('.content-wrapper').height();
                    $("#newadded" + tab_count).find('.grid-stack').height(height - 70);
                }, 1000);

                $(".grid-stack").gridstack({
                    float: true,
                    cellHeight: 10,
                    animate: false,
                    width: 12,
                    verticalMargin: 5
                });


                return false;
            });

            $.get("assets/js/fontawesome-5.json", function (data) {
                let tempArr = [];
                $.each(Object.keys(data), function (i, e) {
                    tempArr.push({ key: e, value: data[e] });
                });
                angular.element("[ng-controller='mainController']").scope().iconsclass = tempArr;
            });


            // Actions On New Tab Input Focus Out
            $(directiveElement).on("blur", ".newtab_input", function () {
                // If Icon Field Empty Remove <i> tag
                if ($(this).parents('li').find('.icon-input').val().length < 1) {
                    $(this).parents('li').find('i.font-icon').detach();
                }

                // If Tab Name Field Empty Don't Allow Focus Out or Add New
                if ($(this).val().length > 0) {
                    $(this).parents('.can-add').find('.add-tab').removeClass('disabled');
                    $(this).next('a').show();
                    $(this).parents('li').find('.icon-input').hide();
                    $(this).siblings('span.done-tab').hide();
                    $(this).siblings('span.delete-tab').hide();
                    $(this).hide();
                }
            });

            $(directiveElement).on("dblclick", ".nav li a", function () {
                $(this).hide();
                $(this).siblings('span.delete-tab').show();
                $(this).siblings('span.done-tab').show();
                $(this).siblings('.newtab_input').show();
                $(this).siblings('.icon-input').show();
            });

            $(directiveElement).on("click", ".done-tab", function () {
                // If Icon Field Empty Remove <i> tag
                if ($(this).parents('li').find('.icon-input').val().length < 1) {
                    $(this).parents('li').find('i.font-icon').detach();
                }


                if ($(this).siblings('.newtab_input').val().length > 0) {
                    $(this).hide();
                    $(this).siblings('.newtab_input').hide();
                    $(this).siblings('.icon-input').hide();
                    $(this).siblings('span.delete-tab').hide();
                    $(this).siblings('a').show();
                }
            });

            $(directiveElement).on("click", ".delete-tab", function () {
                let currentTab = $(this).siblings('a').attr('data-target').replace('#', '');
                console.log(currentTab);
                $(directiveElement).find('.tab-content').find('.tab-pane#' + currentTab).detach();
                $(this).parents('li').detach();
            });

            setTimeout(function () {
                $('.horizontalscrollbar').enscroll({
                    showOnHover: true,
                    scrollIncrement: 30,
                    easingDuration: 300,
                    addPaddingToPane: false,
                    horizontalScrolling: true,
                    horizontalTrackClass: 'horizontaltrack4',
                    horizontalHandleClass: 'horizontalhandle4'
                });
            }, 1000)


            $(directiveElement).parent('.content-wrapper').droppable({
                accept: ".panel-box",
                greedy: true,
                drop: function (event, ui) {
                    $panelType = $(".panel-box.ui-draggable-dragging").attr("data-type");
                    $(this).find('.tab-pane.active').children('.center-icon').detach();
                    $.each($loadpanel, function (i, e) {
                        if ($loadpanel[i]["type"] == $panelType) {
                            var el = angular.element("[ng-controller='mainController']").scope().compileAngular($loadpanel[i]["code"]);
                            $('.new-template .grid-stack').gridstack({
                                float: true,
                                cellHeight: 10,
                                animate: false,
                                width: 12,
                                verticalMargin: 5
                            });
                            var grids = $('.tab-canvas.active .grid-stack').data('gridstack');
                            grids.add_widget(el, 0, 0, 6, 25, true);
                            angular.element("[ng-controller='mainController']").scope().simpleCompile($loadpanel[i]["settings"], $(".side-panel .tab-content .tab-pane > .toggle"));
                            angular.element("[ng-controller='mainController']").scope().simpleCompile($loadpanel[i]["advanced_settings"], $(".side-panel .tab-content .tab-pane > .toggle"));
                            angular.element("[ng-controller='mainController']").scope().simpleCompile($loadpanel[i]["query_builder"], $(".side-panel #tab2"));
                        }


                    });


                }
            });


            // $('.grid-stack').on('added', function(event, elem) {
            //   if($(this).find('.tab-canvas')){
            //     let height = $(elem).find('.content-wrapper').height();
            //     $(elem).find('.grid-stack').height(height - 70);
            //   }
            // });
            //

            //
            setTimeout(function () {
                let height = $(directiveElement).parents('.content-wrapper').height();
                $(directiveElement).find('.grid-stack').height(height - 70);
            }, 1000);
            //
        }
    }
})(xl_script || {});


$(window).load(function () {
    window.FontAwesomeConfig = {
        searchPseudoElements: true
    }

    xl_script.accordion();

    $('.scrollbar-outer').scrollbar();


    $(".content-area").LoadingOverlay("hide");


    function adjustDropdowns() {
        var vh = $(window).height();
        $(".sideheader > ul ul").each(function () {
            let myHeight = $(this).height();
            let myTopGap = $(this).offset().top;
            let sum = myHeight + myTopGap;
            sum = sum + 29;

            if (sum > vh) {
                $(this).addClass('reverse');
            }
        });
    }
    adjustDropdowns();

    $(window).on("resize", function () {
        adjustDropdowns()
    });


    $.LoadingOverlaySetup({
        image: "/Theme/AirViewX/images/loader.png",
        imageAnimation: "1.5s fadeIn",
        imageAutoResize: true,
    });

    $('[data-toggle="tooltip"]').tooltip({
        container: '.content-wrapper'
    });

    $(".av_page_loader").fadeOut().detach();

    $.toaster({ settings: { 'timeout': 5000 } });

    $('[data-toggle="tooltip"]').tooltip({
        container: '.content-wrapper'
    });

    //Collapse Widget Function for RF Optimization Widgets
    $("body").on("click", ".collapse_widget", function () {
        let currentIcon = $(this).find('i').attr('class');
        if (currentIcon == 'fa fa-plus') {
            $(this).find('i').attr('class', 'fa fa-minus');
        }
        else {
            $(this).find('i').attr('class', 'fa fa-plus');
        }
        $(this).parents('.widget').eq(0).find('.content-wrapper').eq(0).slideToggle();
        return false;
    });

    //Add Prevent_close class to parent of Dropdown if want to prevent Dropdown from Closing on Self Click
    $(document).on('click', '.prevent_close .dropdown-menu', function (e) {
        e.stopPropagation();
    });
    //Prevent Dropdown from Closing on Devx Control Click
    $(document).on('click', '.dx-popup-content, .dx-toolbar', function (e) {
        e.stopPropagation();
    });

    // Custom Sidepanel JS
    $('.panel_sidebar .toggle-sidebar').on('click', function () {
        $('.panel_sidebar').toggleClass('hideSidebar');
        $(this).find('i').toggleClass('fa-chevron-circle-right fa-chevron-circle-left')
    });



});
