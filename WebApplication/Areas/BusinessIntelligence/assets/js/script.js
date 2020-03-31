$json_obj = [];
$isJson = false;
var tab_count = 0;

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
                    //return false;
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
                let parent = $(this).closest('.grid-stack-item');
                let currentHeight = $(parent).attr('data-gs-height');
                parent.attr('data-heighthistory', currentHeight);

                let minHeight = $(parent).attr('data-gs-min-height');
                if (parent.attr('data-gs-min-height')) {
                    parent.attr('data-minheighthistory', minHeight);
                }

                var grid = $('.grid-stack').data('gridstack');
                grid.minHeight(parent, '', 6);
                grid.resize(parent, '', 6);
                grid.resizable(parent, false);
                parent.find('.content-wrapper').hide();
                return false;
            });

            $("body").on("click", ".expand-btn", function () {
                $(this).attr('class', 'custom-btn icon-only collapse-btn').find('i').attr('class', 'fa fa-minus');
                // $(this).closest('.grid-stack-item').find('.child-widget-tab').removeClass('active');	
                let parent = $(this).closest('.grid-stack-item');
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
                grid.resizable(parent, true);
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
            //$("body").on("click", '.remove-btn', function () {	
            //    let parent = $(this).parents('.grid-stack-item');	
            //    var grid = $('.grid-stack').data('gridstack');	
            //    $(this).parents('.widget').slideUp("complete", (function () {	
            //        grid.removeWidget(parent, true)	
            //    }));	
            //    return false;	
            //});	
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

        initGridStack: function () {
            gridInfo = $('.new-template .grid-stack').gridstack({
                float: false,
                cellHeight: 6,
                animate: true,
                width: 12,
                verticalMargin: 8,
                draggable: {
                    handle: '.title-bar'
                }
            });
            gridInfo = $('.grid-stack').data('gridstack');
        },
        /* =============== Grid Stack ===================== */
        grid_stack: function () {
            var options = {
                float: false,
                cellHeight: 10,
                animate: true,
                width: 12,
                verticalMargin: 8,
                draggable: {
                    handle: '.title-bar',
                }
            };
            $('.grid-stack').gridstack(options);
            //$('.grid-stack').on('gsresizestop', function (event, elem) {	
            //    if ($(this).find('div[data-highcharts-chart]')) {	
            //        setTimeout(function () {	
            //            $("div[data-highcharts-chart]").each(function (index) {	
            //                $(this).highcharts().reflow();	
            //            });	
            //        }, 300);	
            //    }	
            //    //var elem = angular.element(document.querySelector('[ng-app]'));	
            //    //var injector = elem.injector();	
            //    //var $rootScope = injector.get('$rootScope');	
            //    //$rootScope.resizeCharts();	
            //});


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
                //this.saveGrid = function () {	
                //    this.serializedData = _.map($('.grid-stack > .grid-stack-item:visible'), function (el) {	
                //        el = $(el);	
                //        var node = el.data('_gridstack_node');	
                //        return {	
                //            x: node.x,	
                //            y: node.y,	
                //            width: node.width,	
                //            height: node.height,	
                //            el: $(el)	
                //        };	
                //    });	
                //    console.log(this.serializedData);	
                //    $('#saved-data').val(JSON.stringify(this.serializedData, null, '    '));	
                //    return false;	
                //}.bind(this);	
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


        /* =============== Load Grid into Canvas  ===================== */
        loadGrid: function (data) {
            this.serializedData = data;
            this.grid = $('.grid-stack').data('gridstack');
            this.grid.removeAll();
            var items = GridStackUI.Utils.sort(this.serializedData);
            _.each(items, function (node) {
                if (node.el)
                    this.grid.addWidget(node.el, node.x, node.y, node.width, node.height);
            }.bind(this));
        }.bind(this),
        /* =============== Parse Grid Items  ===================== */
        getGridItems: function () {
            this.serializedData = _.map($('.grid-stack > .grid-stack-item:visible'), function (el) {
                el = $(el);
                var node = el.data('_gridstack_node');
                return {
                    x: node.x,
                    y: node.y,
                    width: node.width,
                    height: node.height,
                    widgetID: $($(el)[0]).attr('id'),
                    el: $(el)
                };
            });
            // var items = GridStackUI.Utils.sort(this.serializedData);	
            // JSON.stringify(this.serializedData, null, '    ')	
            return this.serializedData;
        }.bind(this),


        /* =============== Scrollbar  ===================== */
        scrollbar: function () {
            function scrollbar() {
                $('body .scrollbar').enscroll({
                    showOnHover: true,
                    scrollIncrement: 30,
                    easingDuration: 300,
                    addPaddingToPane: false,
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4'
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
                $(this).css({ "height": chartHeight });
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
            $("body").on("click", ".title-bar h3 span,span.custominputSpanOfPage", function () {
                if (isModeForTab) {
                    $(this).hide();
                    $(this).parent().find("input").show().focus();
                }
            });
            $("body").on("focus", ".title-bar h3 input,input.customInputControlOfPage", function () {
                $(this).on('keyup', function (e) {
                    if (e.keyCode == 13) {
                        $(this).hide();
                        $(this).parent().find('span').show();
                        $(this).parent().find("span.cache-value").hide();
                    }
                });
            });
            $("body").on("blur", ".title-bar h3 input,input.customInputControlOfPage", function () {
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
            //$(".new-template").on("click", ".widget", function () {	
            //    var wgtSettings = $(this).parent().attr("id");	
            //    $(this).parent().parent().addClass("change-settings").siblings().removeClass("change-settings");	
            //    $.when($(".side-panel .widget-settings").fadeOut()).done(function () {	
            //        $(".side-panel .widget-settings." + wgtSettings).fadeIn();	
            //    });	
            //    $.when($(".side-panel .querybuilder-wrapper").fadeOut()).done(function () {	
            //        $(".side-panel .querybuilder-wrapper." + wgtSettings).fadeIn();	
            //    });	
            //});	
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
            $(".content-area").on("click", ".sidepanel-btn", function () {
                $(this).toggleClass('active').next('.side-panel').toggleClass('active');
                $(".new-template").toggleClass('shrink');
                return false;
            });
            /* == Side Panel Width Variation On Query Builder Select ==*/
            $("body").on("click", ".side-panel", function () {
                setTimeout(function () {
                    if ($('.querybuilder').hasClass('active')) {
                        $(".side-panel").css({ "width": "calc(100% - 257px)" });
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
        // parse tabs widgets with tab title and icon
        getTabsWidgetsInfo: function (TabInstance) {
            var Tab = [];
            $.each($("#" + TabInstance).find('.nav.bi li'), function (index, item) {
                //  if ((jQuery.trim($(item).context.outerText)).match(/Select Icon.*/))
                var res = jQuery.trim($(this).children('a').text());
                //&& !res.match(/Select Icon.*/) && !(res === "New Tab") && !(res === "") && !res.match(/fab.*/) && !res.match(/fas.*/) && !res.match(/fad.*/)
                if (index != 0 && res != "") {
                    var TabInfo = {};
                    TabInfo.WidgetList = [];
                    var targetTabId = $(this).children('a').attr('data-target')//$(item).attr('data-target');
                    // comment by sohail
                    //  TabInfo.Icon = jQuery.trim($(this).find('div > a').text())// jQuery.trim($(item).find('i').attr('class'));


                    if ($(this).find('.nav-link >i').attr('class').indexOf('▼') > -1) {
                        iconName = jQuery.trim($(this).find('.nav-link >i').attr('class')).substring(0, jQuery.trim($(this).find('.nav-link >i').attr('class')).length - 1)
                    }
                    else {
                        iconName = jQuery.trim($(this).find('.nav-link >i').attr('class')).substring(0, jQuery.trim($(this).find('.nav-link >i').attr('class')).length - 0)
                    }
                    iconName = $.trim(iconName);

                    TabInfo.Icon = iconName;

                    TabInfo['tabWidgetId'] = targetTabId.substring(1);
                    TabInfo.Title = jQuery.trim($(this).children('a').text())// jQuery.trim($(item).context.innerText);//jQuery.trim($(item).find('span').html());

                    TabInfo.height_attr = $(targetTabId).find('.grid-stack').attr('data-gs-current-height');
                    TabInfo.style = $(targetTabId).find('.grid-stack').attr('style');
                    var TabWidgetList = $(targetTabId).find('.grid-stack-item');
                    $.each($(TabWidgetList), function (i, t) {
                        var WidgetId = $(t).attr('id');
                        TabInfo.WidgetList.push(WidgetId);

                    });

                    Tab.push(TabInfo);
                }
            });
            return Tab;
        },
        handleUniquekey: function (length) {
            var chars = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ', result = "";
            for (var i = length; i > 0; --i)
                result += chars[Math.round(Math.random() * (chars.length - 1))];
            return result;
        },
        dynamic_tabs: function (directiveElement) {
            $(directiveElement).on("click", ".add-tab", function () {
                tab_count++;
                var UniqueTabId = $(this).parents(".grid-stack-item").attr("id") + "" + xl_script.handleUniquekey(4);
                angular.element("[ng-app='Airview.Xcelerate']").scope().iconsclass = [];
                // New Tab Code With Icon Input And Tab Input Fields	
                //var newtab = `	
                //  <li>	
                //  <select class="icon-input" ng-model="selectedicon` + tab_count + `">	
                //    <option  value="" selected>Select Icon</option>	
                //      <option ng-repeat="icon in iconsclass" value="{{icon.key}}">{{icon.key}}</option>	
                //  </select>	
                //  <input class="newtab_input" type="text" placeholder="Tab Name" ng-model="newtab` + tab_count + `" />	
                //  <a style="display:none" class="nav-link" data-toggle="tab" data-target="#newadded` + tab_count + `">	
                //    <i class="font-icon {{selectedicon` + tab_count + `}}"></i>	
                //    {{newtab` + tab_count + `}}	
                //  </a>	
                //  <span ng-show="vm.modeType=='edit'" class="done-tab custom-btn icon-only"><i class="fa fa-check"></i></span>	
                //				<span ng-show="vm.modeType=='edit'" class="delete-tab custom-btn icon-only"><i class="fa fa-trash"></i></span>	
                //  </li>`;	
                var newtab = `
                  <li>
                 <ui-select style="" class ="icon-input" ng-model= "d`+ UniqueTabId + `" theme="select2" title="Choose a icon">
    <ui-select-match class ="add-me" placeholder="Select Icon">{{$select.selected.key}}</ui-select-match>
    <ui-select-choices repeat="icon.key as icon in iconsclass  | propsFilter: {key: $select.search}">
        <i class ="{{icon.key}}"></i>
    </ui-select-choices>
</ui-select>
                  <input class="newtab_input" type="text" placeholder="Tab Name" ng-model="i` + UniqueTabId + `" />
                  <a style="display:none" class="nav-link" data-toggle="tab" data-target="#`+ UniqueTabId + `">
                    <i class = "{{d` + UniqueTabId + `}}" ></i>
                {{i` + UniqueTabId + `}}
                  </a>
                  <span ng-show="vm.modeType=='edit'" class="done-tab custom-btn icon-only"><i class="fa fa-check"></i></span>
  								<span ng-show="vm.modeType=='edit'" class="delete-tab custom-btn icon-only"><i class="fa fa-trash"></i></span>
                  </li>`;
                $(this).addClass('disabled');
                var parentsTab = $(this).parents('.can-add').find('.nav');
                //$.get("Areas/BusinessIntelligence/assets/js/fontawesome-5.json", function (data) {	
                //    let tempArr = [];	
                //    $.each(Object.keys(data), function (i, e) {	
                //        tempArr.push({ key: e, value: data[e] });	
                //    });	
                //    angular.element("[ng-app='Airview.Xcelerate']").scope().iconsclass = tempArr;	
                //});	
                angular.element("[ng-app='Airview.Xcelerate']").scope().simpleCompile(newtab, parentsTab);
                var newtab_content = `<div class="tab-pane fade tab-canvas" id="` + UniqueTabId + `"> <div class="center-icon"><i class="fa fa-ioxhost"></i></div>  <div class="grid-stack"> </div> </i></div>`;
                $(this).parents('.can-add').find('.tab-content').append(newtab_content);
                setTimeout(function () {
                    let height = $("#" + UniqueTabId).parents('.content-wrapper').height();
                    $("#" + UniqueTabId).find('.grid-stack').height(330);
                }, 1000);
                $("#" + UniqueTabId).find(".grid-stack").gridstack({
                    float: true,
                    cellHeight: 10,
                    animate: false,
                    width: 12,
                    verticalMargin: 4
                });
                return false;
            });
            //$.get("Areas/BusinessIntelligence/assets/js/fontawesome-5.json", function (data) {	
            //    let tempArr = [];	
            //    $.each(Object.keys(data), function (i, e) {	
            //        tempArr.push({ key: e, value: data[e] });	
            //    });	
            //    angular.element("[ng-app='Airview.Xcelerate']").scope().iconsclass = tempArr;	
            //});	
            // Actions On New Tab Input Focus Out	
            //$(directiveElement).on("blur", ".newtab_input", function () {	
            //    // If Icon Field Empty Remove <i> tag	
            //    if ($(this).parents('li').find('.icon-input').val().length < 1) {	
            //        $(this).parents('li').find('i.font-icon').detach();	
            //    }	
            //    // If Tab Name Field Empty Don't Allow Focus Out or Add New	
            //    if ($(this).val().length > 0) {	
            //        $(this).parents('.can-add').find('.add-tab').removeClass('disabled');	
            //        $(this).next('a').show();	
            //        $(this).parents('li').find('.icon-input').hide();	
            //        $(this).siblings('span.done-tab').hide();	
            //        $(this).siblings('span.delete-tab').hide();	
            //        $(this).hide();	
            //    }	
            //});	
            $(directiveElement).on("dblclick", ".nav li a", function () {
                // add by sohail	
                // var a = $(".add-me").children()[1].children[0].innerText;	
                //$.get("assets/js/fontawesome-5.json", function (data) {	
                //    let tempArr = [];	
                //    $.each(Object.keys(data), function (i, e) {	
                //        tempArr.push({ key: e, value: data[e] });	
                //    });	
                // angular.element("[ng-app='Airview.Xcelerate']").scope().iconsclass = $.trim(($(this).siblings('.newtab_input').siblings('a:first')[0].children[0].className).replace('Select Icon', ''));// $(".add-me").children()[1].children[0].innerText; //$(this).siblings('.icon-input')[0].innerText	

                //});	
                if (isModeForTab) {
                    $(this).hide();
                    $(this).siblings('span.delete-tab').show();
                    $(this).siblings('span.done-tab').show();
                    $(this).siblings('.newtab_input').show();
                    $(this).siblings('.icon-input').show();
                }
                //angular.element("[ng-app='Airview.Xcelerate']").scope().iconsclass = {	
                //    selected: $.trim(($(this).siblings('.newtab_input').siblings('a:first')[0].children[0].className).replace('Select Icon', ''))	
                //};	

            });
            $(directiveElement).on("click", ".done-tab", function () {
                // If Icon Field Empty Remove <i> tag	
                //if ($(this).parents('li').find('.icon-input').val().length < 1) {	
                //    $(this).parents('li').find('i.font-icon').detach();	
                //}	
                var getSelectIconName = $(this).find('div > a > span').context.children[0].className;
                if ($(this).siblings('.icon-input')[0].innerText.length < 1) {
                    $(this).parents('li').find('i.font-icon').detach();
                }
                //innerHTML $(this).siblings('a')[0].innerHTML	
                if ($(this).siblings('.newtab_input').val().length > 0) {
                    $(this).hide();
                    $(this).parents('.can-add').find('.add-tab').removeClass('disabled');
                    $(this).siblings('.newtab_input').hide();
                    $(this).siblings('.icon-input').hide();
                    $(this).siblings('span.delete-tab').hide();
                    $(this).siblings('a').show();
                    //added by sohail	
                    //$(this).siblings('a').before("<i class=" + $(this).siblings('.icon-input')[0].innerText + "></i>");	
                    $(this).siblings('a:first').children()[0].className = $(this).siblings('.icon-input')[0].innerText;
                }
            });
            $(directiveElement).on("click", ".delete-tab", function () {
                let currentTab = $(this).siblings('a').attr('data-target').replace('#', '');
                $(this).parents('.can-add').find('.add-tab').removeClass('disabled');
                var TabWidgetList = $(directiveElement).find('.tab-pane#' + currentTab).find('.grid-stack-item');
                $.each($(TabWidgetList), function (i, t) {
                    let WidgetId = $(t).attr('id');
                    console.log(WidgetId)
                    deleteChildTabWidget.push(WidgetId);
                });
                $(directiveElement).find('.tab-content').find('.tab-pane#' + currentTab).detach();
                $(this).parents('li').detach();
            });
            setTimeout(function () {
                //Comment by sarfraz bhai	
                //$('.horizontalscrollbar').enscroll({	
                //    showOnHover: true,	
                //    scrollIncrement: 30,	
                //    easingDuration: 300,	
                //    addPaddingToPane: false,	
                //    horizontalScrolling: true,	
                //    horizontalTrackClass: 'horizontaltrack4',	
                //    horizontalHandleClass: 'horizontalhandle4'	
                //});	
            }, 1000);
            //$(directiveElement).parent('.content-wrapper').droppable({	
            //    accept: ".panel-box",	
            //    greedy: true,	
            //    drop: function (event, ui) {	
            //        $panelType = $(".panel-box.ui-draggable-dragging").attr("data-type");	
            //        $(this).find('.tab-pane.active').children('.center-icon').detach();	
            //        $.each($loadpanel, function (i, e) {	
            //            if ($loadpanel[i]["type"] == $panelType) {	
            //                var el = angular.element("[ng-app='Airview.Xcelerate']").scope().compileAngular($loadpanel[i]["code"]);	
            //                $('.new-template .grid-stack').gridstack({	
            //                    float: true,	
            //                    cellHeight: 10,	
            //                    animate: false,	
            //                    width: 12,	
            //                    verticalMargin: 5	
            //                });	
            //                var grids = $('.tab-canvas.active .grid-stack').data('gridstack');	
            //                grids.add_widget(el, 0, 0, 6, 25, true);	
            //                angular.element("[ng-app='Airview.Xcelerate']").scope().simpleCompile($loadpanel[i]["settings"], $(".side-panel .tab-content .tab-pane > .toggle"));	
            //                angular.element("[ng-app='Airview.Xcelerate']").scope().simpleCompile($loadpanel[i]["advanced_settings"], $(".side-panel .tab-content .tab-pane > .toggle"));	
            //                angular.element("[ng-app='Airview.Xcelerate']").scope().simpleCompile($loadpanel[i]["query_builder"], $(".side-panel #tab2"));	
            //            }	
            //        });	
            //    }	
            //});	
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
                $(directiveElement).find('.grid-stack').height(330);
            }, 1000);
            //	
        }
    }
})(xl_script || {});


$(window).load(function () {
    window.FontAwesomeConfig = {
        searchPseudoElements: true
    };
    $('body').on('click', 'button[ref="sidebar-anchor"]', function (e) {
        $(this).toggleClass('collapsed');
        $(this).parents('div').next().toggleClass('show');
        return false;
    })

    $('body').on('blur', '.tbl-aliasname ,.tbl-columnname', function (e) {
        $(this).attr('value', $(this).val());
    });

    $("body").on("click", ".ui-grid-pager-control .pagination li a", function () {
        if ($(this).text() != "...") {
            $(this).addClass('active');
            $(this).parents('li').siblings('li').find('a').removeClass('active');
        }
    });


    xl_script.accordion();


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




    $(document).on('click', '.custom_filter , .dx-overlay-content ', function (e) {
        e.stopPropagation();
    });

    $('body').on('scroll', function (e) {
        var topmargin = $("body").scrollTop();
        if (topmargin > 72) {
            $(".side-panel.active").css("top", "0")
            $("a.sidepanel-btn").css("top", "0")
        } else {
            $(".side-panel.active").css("top", "72px")
            $("a.sidepanel-btn").css("top", "72px")
        }
    });

});




/* === Update Filters Code Starts From Here ===*/
function UpdateFilters(e) {
    // Update From
    let updateFrom = e.element.parents('.applied_filters').find('.updateFrom').dxDateBox('instance');
    if (updateFrom !== undefined) {
        updateFrom = updateFrom._changedValue;
        e.element.parents('.applied_filters').find('.dateFrom').text(updateFrom).attr('title', updateFrom);
    }


    // Update To
    let updateTo = e.element.parents('.applied_filters').find('.updateTo').dxDateBox('instance');
    if (updateTo !== undefined) {
        updateTo = updateTo._changedValue;
        e.element.parents('.applied_filters').find('.dateTo').text(updateTo).attr('title', updateTo);
    }

    //Update Select 1
    let filter_select1 = e.element.parents('.applied_filters').find('.filter_select1').dxDropDownBox('instance');
    if (filter_select1 !== undefined) {
        filter_select1 = filter_select1._changedValue;
        e.element.parents('.applied_filters').find('.filter_selected1').text(filter_select1).attr('title', filter_select1);
    }

    //Update Select 2
    let filter_select2 = e.element.parents('.applied_filters').find('.filter_select2').dxDropDownBox('instance');
    if (filter_select2 !== undefined) {
        filter_select2 = filter_select2._changedValue;
        e.element.parents('.applied_filters').find('.filter_selected2').text(filter_select2).attr('title', filter_select2);
    }
}
/* === Update Filters Code Ends Here ===*/




/* === Required for DevExtreme Treeview Multiselect Dropdown ===*/
function syncTreeViewSelection(treeView, value) {
    if (!value) {
        treeView.unselectAll();
        return;
    }

    value.forEach(function (key) {
        treeView.selectItem(key);
    });
}

function getSelectedItemsKeys(items) {
    var result = [];
    items.forEach(function (item) {
        if (item.selected) {
            result.push(item.key);
        }
        if (item.items.length) {
            result = result.concat(getSelectedItemsKeys(item.items));
        }
    });
    return result;
}

function treeBox_valueChanged(e) {
    //var $treeView = e.component.content().find(".dx-treeview");
    //if ($treeView.length) {
    //    syncTreeViewSelection($treeView.dxTreeView("instance"), e.value);
    //}
}
/* === Required for DevExtreme Treeview Multiselect Dropdown Ends ===*/


//#region this erea for bi

var fnSaveEntitySuccess = function (e) {
   
    
    if (e.entityexist == true) {
        $.toaster({ message: e.msg, title: 'Error', priority: 'danger' });
      
    } else {
        angular.element('[ng-controller="TopSectionDashboardAndReport as vm"]').scope().addEntity(e);
        $.toaster({ message: e.msg, title: 'Success', priority: 'success' });
        $("#new_entity").modal("hide");

        //RestEntityTemplateFields();

    }
}
//#endregion

