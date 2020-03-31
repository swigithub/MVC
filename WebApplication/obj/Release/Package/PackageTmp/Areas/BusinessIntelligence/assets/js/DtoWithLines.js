//#region script


//$json_obj = [];
//var xl_script = (function() {
//        return {
//                /* ===== Dropdown Functionalities ==== */
//                dropdown: function() {
//                        $('html').on('click' ,function(event){
//                                if (!$(event.target).hasClass('multiselect')){
//                                        $('.opt-dropdown').removeClass('active');
//                                        $('.opt-btn').removeClass('active').next('ul.opt-dropdown').removeClass('active');
//                                }
//                        });
//                        $('body').on('click', '.opt-btn' ,function(){
//                                $('.opt-btn').removeClass('active').next('ul.opt-dropdown').removeClass('active');
//                                $(this).toggleClass('active').next('ul.opt-dropdown').toggleClass('active');
//                                return false;
//                        });
//                        $('html').on('click', '.date-range, .daterangepicker, .farbtastic' ,function(e){e.stopPropagation()});
//                },


//                /* ===== Delay Function On Each List Item Of Dropdown ==== */
//                delayeachdropdown: function() {
//                        $('ul.opt-dropdown').each(function(){
//                                var a = 0;
//                                $(this).children('li').each(function(){
//                                        $(this).css({"transition-delay":a + 'ms'});
//                                        a = a + 50;
//                                });
//                        });
//                },



//                /* ===== Sideheader  ==== */
//                sideheaderdropdown: function() {
//                        $('.sideheader > ul > li ul').parent().addClass('has-dropdown');
//                        $('.has-dropdown > a').on('click',function(){
//                                $(this).next('ul').slideToggle();
//                                $(this).parent().siblings('.has-dropdown').find('ul').slideUp();
//                                return false;
//                        });
//                },


//                /* =====  Dropdown ==== */
//                optionsdropdown: function() {
//                        $('ul.opt-dropdown > li ul').parent().addClass('has-children');
//                        $('body').on('click', '.has-children > a' ,function(){
//                                $(this).next('ul').slideToggle();
//                                $(this).parent().siblings('.has-children').find('ul').slideUp();
//                                return false;
//                        });

//                },



//                /* ===== Custom SelectBox ==== */
//                customselectbox: function() {
//                        $('body').on('click', '.custom-selectbox ul.opt-dropdown li a' ,function(){
//                                var updateValue = $(this).text();
//                                $(this).parents('.custom-selectbox').find('.opt-btn i').html(updateValue);
//                                return false;
//                        });
//                        $('body').on('click', '.custom-selectbox  input' ,function(){
//                                return false;
//                        });
//                },


//                /* ===== Progress Bar Dropdown ==== */
//                progressbardropdown: function() {
//                        $("body").on('click', '.pv-progresser > a' ,function(){
//                                if($(this).find('i').hasClass('fa-plus')){
//                                        $(this).find('i').removeClass('fa-plus').addClass('fa-minus');
//                                }
//                                else if($(this).find('i').hasClass('fa-minus')){
//                                        $(this).find('i').removeClass('fa-minus').addClass('fa-plus');
//                                }
//                                $(this).parent().find('.pv-progresser-dropdown').slideToggle();
//                        });
//                },



//                /*================== Table Options =====================*/
//                tableoptions: function() {
//                        $("body").on("click", '.custom-btn.opt-btn' ,function(){
//                                $(this).parents('tr').siblings().find('.opt-dropdown').removeClass('active');
//                                $(this).parents('tr').find('.opt-dropdown').addClass("active");
//                                return false;
//                        });
//                        $("body").on('click', '.more-table-btn' ,function(){
//                                $(this).parents('tr').next('tr.child-table').fadeToggle();
//                                var current_sign = $(this).html();
//                                if(current_sign == '+'){$(this).addClass('active').html('-'); }
//                                else{$(this).removeClass('active').html('+'); }
//                                setTimeout(function(){
//                                        document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
//                                        document.dispatchEvent(new Event('UpdateChildItems')); // broadcast an event when drawing area is resized
//                                }, 200);
//                                return false;
//                        });
//                },


//                /*================== Stripped Rows =====================*/
//                strippedRows: function() {
//                        $('table').each(function(){
//                                var RowList = $(this).children('tbody').children('tr.tablerow');
//                                var tableDataLength = RowList.length;

//                                for(i=0;i < tableDataLength;){
//                                        if($(RowList[i]).hasClass('child-table')){
//                                                continue;
//                                        }
//                                        if(i%2==1){
//                                                $(RowList[i]).addClass("stripped");
//                                        }

//                                        i++;
//                                }
//                        });
//                },


//                /* =============== Donut Chart Label Convert Into Dropdown On Small Screens ===================== */
//                donutchartlabels: function() {
//                        $('body').on('click', ".label-drop" ,function(){
//                                $(this).next('.labeling').toggleClass('active');
//                                return false;
//                        });
//                },



//                /* =============== Header Logo Buttons Convert To Dropdown In Smaller Screen ===================== */
//                headerlogobuttons: function() {
//                        if($(window).width() < 1200){
//                                $("body").on('click', ".more-links > img", function(){
//                                        $(this).next('ul').toggleClass('active');
//                                        return false;
//                                });
//                        }
//                },



//                /* =============== Menu Icon To Open Close Sidemenu ===================== */
//                menuicon: function() {
//                        $('.menu-icon').on('click',function(){
//                                $('.sideheader').toggleClass('active');
//                                setTimeout(function(){
//                                        $( "div[data-highcharts-chart]" ).each(function( index ) {
//                                                $(this).highcharts().reflow();
//                                        });
//                                },300);
//                                document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
//                                document.dispatchEvent(new Event('UpdateChildItems')); // broadcast an event when drawing area is resized
//                                return false;
//                        });

//                        if($(window).width() < 1200){
//                                $('html').on('click',function(){
//                                        $('.sideheader').removeClass('active');
//                                });
//                        }
//                },


//                /* =============== Popup ===================== */
//                popup: function() {
//                        $("body").on("click", ".open-popup" ,function(){
//                                $(".popup").addClass("active");
//                                var checkId = $(this).attr('href');
//                                $('.popup-box').fadeOut();
//                                $(checkId).fadeIn();
//                                return false;
//                        });
//                        $("body").on("click", ".close-popup" ,function(){
//                                $(".popup").removeClass("active");
//                                $('.popup-box').fadeOut();
//                                return false;
//                        });
//                },



//                /* =============== HighCharts Resize On Menu Close and Open ===================== */
//                highchartresize: function() {
//                        $('body').on('resize', '.widget' , function(){
//                                $( "div[data-highcharts-chart]" ).each(function( index ) {
//                                        $(this).highcharts().reflow();
//                                });
//                        });
//                },


//                /* =============== Widget Collapse Button ===================== */
//                collapseWidget: function() {
//                        $("body").on("click", ".collapse-btn" ,function(){
//                                $(this).closest(".widget").find(".content-wrapper").slideToggle();
//                                if($(this).children('i').hasClass('fa-minus')){
//                                        $(this).children('i').removeClass('fa-minus').addClass('fa-plus');
//                                }
//                                else if($(this).children('i').hasClass('fa-plus')){
//                                        $(this).children('i').removeClass('fa-plus').addClass('fa-minus');
//                                }
//                                document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
//                                document.dispatchEvent(new Event('UpdateChildItems')); // broadcast an event when drawing area is resized
//                                return false;
//                        });
//                },



//                /* =============== Header Converter ===================== */
//                headerConverter: function() {
//                        $(".header-converter").on("click",function(){
//                                $(".layout-wrapper").toggleClass('top-nav');
//                                $(".menu-icon").toggleClass("hide");
//                                document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
//                                document.dispatchEvent(new Event('UpdateChildItems')); // broadcast an event when drawing area is resized
//                                return false;
//                        });
//                },


//                /* =============== Widget Remove Button ===================== */
//                removeButton: function() {
//                        $("body").on("click", '.remove-btn' ,function(){
//                                $(this).closest(".widget").slideToggle();
//                                document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
//                                document.dispatchEvent(new Event('UpdateChildItems')); // broadcast an event when drawing area is resized
//                                return false;
//                        });
//                },


//                /* =============== Column Size  ===================== */
//                columnSize: function() {
//                        $('body').on('click', '.column-size a' ,function(){
//                                if($(this).hasClass('onethird')){
//                                        $(this).closest('.widget').parent().attr('class', 'col-xl-4 col-lg-12');
//                                }
//                                if($(this).hasClass('onesixth')){
//                                        $(this).closest('.widget').parent().attr('class', 'col-xl-2 col-lg-12');
//                                }
//                                if($(this).hasClass('half')){
//                                        $(this).closest('.widget').parent().attr('class', 'col-xl-6 col-lg-12');
//                                }
//                                if($(this).hasClass('twothird')){
//                                        $(this).closest('.widget').parent().attr('class', 'col-xl-8 col-lg-12');
//                                }
//                                if($(this).hasClass('fivesixth')){
//                                        $(this).closest('.widget').parent().attr('class', 'col-xl-10 col-lg-12');
//                                }
//                                if($(this).hasClass('fullwide')){
//                                        $(this).closest('.widget').parent().attr('class', 'col-xl-12 col-lg-12');
//                                }
//                                document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
//                                document.dispatchEvent(new Event('UpdateChildItems')); // broadcast an event when drawing area is resized
//                                setTimeout(function(){
//                                        $( "div[data-highcharts-chart]" ).each(function( index ) {
//                                                $(this).highcharts().reflow();
//                                        });
//                                },300);
//                                return false;
//                        });
//                },


//                /* =============== Circular Loader  ===================== */
//                CLoader: function() {
//                        $("#tss").circularloader({
//                                progressPercent: 60,
//                                backgroundColor: "#8a70b1",//background colour of inner circle
//                                fontColor: "#fff",//font color of progress text
//                                fontSize: "16px",//font size of progress text
//                                radius:28,//radius of circle
//                                progressBarBackground: "#a290c4",//background colour of circular progress Bar
//                                progressBarColor: "#593e8f",//colour of circular progress bar
//                                progressBarWidth:11,//progress bar width
//                                speed: 10,
//                                progressvalue: 60,
//                                showText: true
//                        });

//                        $("#ssm").circularloader({
//                                progressPercent: 50,
//                                backgroundColor: "#f37d62",//background colour of inner circle
//                                fontColor: "#fff",//font color of progress text
//                                fontSize: "16px",//font size of progress text
//                                radius:28,//radius of circle
//                                progressBarBackground: "#f59585",//background colour of circular progress Bar
//                                progressBarColor: "#d65641",//colour of circular progress bar
//                                progressBarWidth:11,//progress bar width
//                                speed: 10,
//                                progressvalue: 60,
//                                showText: true
//                        });

//                        $("#epl").circularloader({
//                                progressPercent: 70,
//                                backgroundColor: "#3b8655",//background colour of inner circle
//                                fontColor: "#fff",//font color of progress text
//                                fontSize: "16px",//font size of progress text
//                                radius:28,//radius of circle
//                                progressBarBackground: "#579f6c",//background colour of circular progress Bar
//                                progressBarColor: "#277640",//colour of circular progress bar
//                                progressBarWidth:11,//progress bar width
//                                speed: 10,
//                                progressvalue: 70,
//                                showText: true
//                        });

//                        $("#ordered").circularloader({
//                                progressPercent: 30,
//                                backgroundColor: "#f0b828",//background colour of inner circle
//                                fontColor: "#fff",//font color of progress text
//                                fontSize: "16px",//font size of progress text
//                                radius:28,//radius of circle
//                                progressBarBackground: "#f4c76d",//background colour of circular progress Bar
//                                progressBarColor: "#d59529",//colour of circular progress bar
//                                progressBarWidth:11,//progress bar width
//                                speed: 10,
//                                progressvalue: 30,
//                                showText: true
//                        });

//                        $("#calledout").circularloader({
//                                progressPercent: 80,
//                                backgroundColor: "#03a3a2",//background colour of inner circle
//                                fontColor: "#fff",//font color of progress text
//                                fontSize: "16px",//font size of progress text
//                                radius:28,//radius of circle
//                                progressBarBackground: "#26bcb4",//background colour of circular progress Bar
//                                progressBarColor: "#037d7a",//colour of circular progress bar
//                                progressBarWidth:11,//progress bar width
//                                speed: 10,
//                                progressvalue: 80,
//                                showText: true
//                        });

//                        $("#delivered").circularloader({
//                                progressPercent: 60,
//                                backgroundColor: "#2b9045",//background colour of inner circle
//                                fontColor: "#fff",//font color of progress text
//                                fontSize: "16px",//font size of progress text
//                                radius:28,//radius of circle
//                                progressBarBackground: "#4ab55f",//background colour of circular progress Bar
//                                progressBarColor: "#187d3e",//colour of circular progress bar
//                                progressBarWidth:11,//progress bar width
//                                speed: 10,
//                                progressvalue: 60,
//                                showText: true
//                        });

//                        $("#preinstall").circularloader({
//                                progressPercent: 40,
//                                backgroundColor: "#ae405b",//background colour of inner circle
//                                fontColor: "#fff",//font color of progress text
//                                fontSize: "16px",//font size of progress text
//                                radius:28,//radius of circle
//                                progressBarBackground: "#cc5f7e",//background colour of circular progress Bar
//                                progressBarColor: "#912046",//colour of circular progress bar
//                                progressBarWidth:11,//progress bar width
//                                speed: 10,
//                                progressvalue: 40,
//                                showText: true
//                        });

//                        $("#migration").circularloader({
//                                progressPercent: 50,
//                                backgroundColor: "#0e83ab",//background colour of inner circle
//                                fontColor: "#fff",//font color of progress text
//                                fontSize: "16px",//font size of progress text
//                                radius:28,//radius of circle
//                                progressBarBackground: "#1dafd2",//background colour of circular progress Bar
//                                progressBarColor: "#05667c",//colour of circular progress bar
//                                progressBarWidth:11,//progress bar width
//                                speed: 10,
//                                progressvalue: 50,
//                                showText: true
//                        });
//                },


//                /* =============== Murri  ===================== */
//                murri: function() {
//                        var grid = new Muuri('.grid', {
//                                dragEnabled: true,
//                                // dragStartPredicate: {
//                                //         handle: '.grid  > div > .widget > .title-bar'
//                                // },
//                                dragHammerSettings: {
//                                        touchAction: 'pan-y'
//                                },
//                                dragStartPredicate: (item, hammerEvent) => {
//                                        if (hammerEvent.pointerType === 'mouse') {
//                                                this.isResolved = true;
//                                                return true;
//                                        }
//                                        else {
//                                                if (hammerEvent.deltaTime >= 500) {
//                                                        this.isResolved = true;
//                                                        return true;
//                                                }
//                                        }

//                                },
//                                dragStartPredicate: function (item, event, e) {
//                                        // Prevent first item from being dragged.
//                                        if ($(event.target).is(".widget > .title-bar")) {
//                                                return Muuri.ItemDrag.defaultStartPredicate(item, event);
//                                        }
//                                        if ($(event.target).is("input")) {
//                                                return false;
//                                        }
//                                },
//                                layout: {
//                                        fillGaps: true
//                                }
//                        });


//                        window.addEventListener('load', function () {
//                                grid.refreshItems().layout();
//                        });


//                        document.addEventListener('UpdateItems', function() {
//                                setTimeout(function(){
//                                        grid.refreshItems().layout();
//                                },500);
//                        });
//                        //
//                        // $('html').on('click',function(){
//                        //         document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
//                        // });

//                        $('.nav li a').on('click',function(){
//                                document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
//                        });


//                        // if($(window).width() < 767){
//                        //         grid.destroy();
//                        // }

//                        var pgname2 = $(".page").attr("data-pagename");
//                        grid.on('layoutEnd', function (items) {
//                                var panels = grid.getItems();
//                                var items = [];
//                                $(panels).each(function(i,e){
//                                        items.push($(e._element).attr("data-position"));
//                                });
//                                setTimeout(function(){
//                                        $.cookie("posCookie" + pgname2  ,items);
//                                },3000)
//                        });
//                        setTimeout(function(){
//                                if($.cookie("posCookie" + pgname2)){
//                                        var loadPos = $.cookie("posCookie" + pgname2).split(",");
//                                        var panels = grid.getItems();
//                                        var position_data = [];
//                                        $(loadPos).each(function(i,e){
//                                                $(panels).each(function(ii,ee){
//                                                        if($($(panels[ii])[0]._element).attr('data-position') == e) {
//                                                                position_data[i] = $(panels[ii])[0]
//                                                        }
//                                                });
//                                        });
//                                        grid.sort(position_data);
//                                }
//                        },2000);



//                },

//                /* =============== Murri  =========     ============ */
//                childmuuri: function() {
//                        var childgrid = new Muuri('.childgrid', {
//                                dragEnabled: true,
//                                dragStartPredicate: {
//                                        handle: '.widget > .title-bar'

//                                },
//                                layout: {
//                                        fillGaps: true
//                                }
//                        });
//                        window.addEventListener('load', function () {
//                                childgrid.refreshItems().layout();
//                        });


//                        document.addEventListener('UpdateChildItems', function() {
//                                setTimeout(function(){
//                                        childgrid.refreshItems().layout();
//                                },500);
//                        });

//                        // $('html').on('click',function(){
//                        //         document.dispatchEvent(new Event('UpdateChildItems')); // broadcast an event when drawing area is resized
//                        // });
//                },


//                /* =============== Scrollbar  ===================== */
//                scrollbar: function() {
//                        $('body .scrollbar').enscroll({
//                                showOnHover: true,
//                                scrollIncrement:30,
//                                easingDuration:300,
//                                addPaddingToPane:false,
//                                verticalTrackClass: 'track4',
//                                verticalHandleClass: 'handle4'
//                        });
//                },








//                /* =============== Map  ===================== */
//                map:function(){
//                        var map = '';

//                        // Creating a LatLng object containing the coordinate for the center of the map
//                        var latlng = new google.maps.LatLng(53.385846,-1.471385);

//                        // Creating an object literal containing the properties we want to pass to the map
//                        var options = {
//                                zoom: 15, // This number can be set to define the initial zoom level of the map
//                                center: latlng,
//                        };
//                        // Calling the constructor, thereby initializing the map
//                        map = new google.maps.Map(document.getElementById('map_div'), options);
//                        document.dispatchEvent(new Event('UpdateItems')); // broadcast an event when drawing area is resized
//                },

//                /* =============== Canvas Chart  ===================== */
//                CanvasChart:function(){
//                        $("div[canvascharts]").each(function(){
//                                var chartHeight = $(this).find("canvas").height();
//                                $(this).css({"height" :chartHeight})
//                        });
//                },


//                /* =============== Canvas Chart  ===================== */
//                CanvasChartRender:function(){
//                        $('body').on('click', '.column-size a' ,function(e){
//                                setTimeout(function(){
//                                        var grabController = $(e.target).parents('.widget').find("div[canvascharts]");
//                                        angular.element(grabController).scope().chart.render();
//                                        return false;
//                                },300);
//                        });
//                        $('body').on('click', '.menu-icon' ,function(e){
//                                setTimeout(function(){
//                                        var elem = angular.element(document.querySelector('[ng-app]'));
//                                        var injector = elem.injector();
//                                        var $rootScope = injector.get('$rootScope');
//                                        $rootScope.resizeCharts();
//                                },300);
//                        });
//                },



//                /* =============== Alert Auto Hide  ===================== */
//                AlertAutoHide:function(){
//                        $(".alert.hidden").hide();
//                        $("body").on("click",".showAlert", function() {
//                                var alert = $(this).attr("data-target");
//                                $(".alert#" + alert).fadeIn().delay(5000).queue(function(n) {
//                                        $(this).fadeOut(); n();
//                                });
//                        });
//                },


//                /* =============== Selected Filters ===================== */
//                ApplyFilters:function(){
//                        function selectedFilters(element){
//                                $(".multiselect-selected-text").each(function(){
//                                        $(this).addClass("getValue");
//                                });
//                                var list_array = [];
//                                $(element).parents("ul").children("li").each(function(){
//                                        if($(this).find("span.getValue").text()){
//                                                list_array.push("<span>" + $(this).find("span.getValue").text() + "</span>");
//                                        }
//                                });

//                                $(element).parents(".title-bar").find(".selected-filters").children().remove();
//                                $(element).parents(".title-bar").find(".selected-filters").append(list_array);
//                        }

//                        $(".filter-btn").each(function(){
//                                selectedFilters(this);
//                        });

//                        $("body").on("click",".filter-btn",function(){
//                                selectedFilters(this);
//                        });
//                },




//                /* =============== Theme Select ===================== */
//                themeSelect:function(){
//                        $(".themes-list li a").on("click",function(){
//                                var getTheme = $(this).attr("data-theme");
//                                $("body").attr("class",getTheme);
//                                $(this).parent().siblings().find('a').removeClass("active");
//                                $(this).addClass("active");
//                        });
//                },


//                /* =============== Color Picker Position ===================== */
//                colorPickerPosition:function(){       // Color Picker Position
//                        function getOffset(el) {
//                                el = el.getBoundingClientRect();
//                                return {
//                                        left: el.left + window.scrollX,
//                                        top: el.top + window.scrollY
//                                }
//                        }

//                        $("body").on("focus",'.bgcolor', function(){
//                                var posTop = getOffset(this).top - 35;
//                                var posLeft = getOffset(this).left;
//                                $(this).next('.colorpickerwrapper').css({
//                                        "top":posTop,
//                                        "left":posLeft
//                                });
//                        });
//                },


//                /* =============== Title Update ===================== */
//                TitleUpdate:function(){
//                        $("body").on("click",".title-bar h3 span",function(){
//                                $(this).hide();
//                                $(this).parent().find("input").show().focus();
//                        });

//                        $("body").on("focus",".title-bar h3 input",function(){
//                                $(this).on( 'keyup', function (e) {
//                                        if (e.keyCode == 13) {
//                                                $(this).hide();
//                                                $(this).parent().find('span').show();
//                                                $(this).parent().find("span.cache-value").hide();
//                                        }
//                                });
//                        });

//                        $("body").on("blur",".title-bar h3 input",function() {
//                                $(this).parent().find("span.update-name").show();
//                                $(this).hide();
//                                $(this).parent().find("span.cache-value").hide();
//                        });
//                },


//                /* =============== Title Cookies ===================== */
//                titleCookies:function(){
//                        $pageName = $(".page").attr("data-pagename");
//                        $("body").on("blur change", "div[data-pagename='"+ $pageName + "'] .title-bar h3 input", function(){
//                                $titlecookie = "{";
//                                $(".title-bar h3 input").each(function(i,e){
//                                        $titlecookie += '"widget-title_' + i + '":"' + e.value +'",';
//                                });
//                                $titlecookie = $titlecookie.substring(0, $titlecookie.length - 1);
//                                $titlecookie += '}';
//                                $.cookie("widgets_titles" + $pageName  ,$titlecookie)
//                        });

//                        $parsed_data = null;
//                        if($.cookie("widgets_titles" + $pageName)) {
//                                $parsed_data = JSON.parse($.cookie("widgets_titles" + $pageName));
//                                $("div[data-pagename='"+ $pageName + "'] .title-bar h3 input").each(function(i,e){
//                                        $(e).val($parsed_data['widget-title_'+i]);
//                                });
//                                $(".title-bar h3 span.cache-value").each(function(i,e){
//                                        $(e).text($parsed_data['widget-title_'+i]);
//                                });
//                        }

//                        $(".title-bar").each(function(){
//                                if($(this).find("span.cache-value").length > 0 && $(this).find("span.cache-value")[0].innerText){
//                                        $(this).find("span.cache-value").show();
//                                        $(this).find("span.update-name").hide();
//                                }
//                        });




//                },


//                /* =============== Color Cookies ===================== */
//                colorCookies:function(){
//                        $pgname = $(".page").attr("data-pagename");
//                        $("body").on("change paste keyup","div[data-pagename='"+ $pgname + "'] input.bgcolor", function() {
//                                $colorcookie = "{";
//                                $(".title-bar").each(function(i,e){
//                                        $colorcookie += '"widget-color_' + i + '":"' + $(this).attr("style") +'",';
//                                });
//                                $colorcookie = $colorcookie.substring(0, $colorcookie.length - 1);
//                                $colorcookie += '}';

//                                $.cookie("color_cookies_" + $pgname  ,$colorcookie)

//                        });


//                        if($.cookie("color_cookies_" + $pgname)) {
//                                $parsed_colordata = JSON.parse($.cookie("color_cookies_" + $pgname));
//                                $("div[data-pagename='"+ $pgname + "'] .title-bar").each(function(i,e){
//                                        $(e).attr("style",$parsed_colordata['widget-color_'+i]);
//                                });
//                        }

//                },



//                /* =============== Theme Change Cookies ===================== */
//                themeChange:function(){
//                        $("body").on("click",".themes-list  a",function(){
//                                $theme = $(this).attr("data-theme");
//                                console.log($theme);
//                                $.cookie("myTheme",$theme);
//                                return false;
//                        });

//                        $("body").attr("class",$.cookie("myTheme"));
//                },


//                /* =============== Theme Change Cookies ===================== */
//                updateJSON:function(){

//                        var itemCheckExistOrNot;
//                        var getIndex;
//                        if($pgname)
//                        {
//                                if($json_obj.length>0)
//                                {
//                                        itemCheckExistOrNot=$json_obj.find(x=>x.type==$pgname);
//                                        if(!itemCheckExistOrNot)
//                                        {
//                                                subList={type:$pgname,widgetList:[]};
//                                                $json_obj.push(subList);
//                                        }
//                                }
//                                else
//                                {
//                                        subList={type:$pgname,widgetList:[]};
//                                        $json_obj.push(subList);
//                                }
//                                //$json_obj[$pgname]={};
//                                getIndex=$json_obj.findIndex(x=>x.type==$pgname);
//                                $(".page > div").each(function(){
//                                        obj={};
//                                        obj['data_position']=$(this).attr("data-position");
//                                        obj["style"] = $(this).find(".title-bar").attr("style");
//                                        if($.cookie("widgets_titles" + $pageName)){
//                                                obj["widgetname"] = $(this).find(".title-bar h3 span.cache-value").innerText;
//                                        }else{
//                                                obj["widgetname"] = $(this).find(".title-bar h3 input").attr("ng-init") ? $(this).find(".title-bar h3 input").attr("ng-init").split("=")[1] : '';
//                                        }
//                                        obj["size"] = $(this).attr("class").replace("muuri-item muuri-item-shown", "");
//                                        if($json_obj[getIndex].widgetList.length>0)
//                                        {
//                                                var getIndexOfWidgetList=$json_obj[getIndex].widgetList.findIndex(x=>x.data_position==obj.data_position);
//                                                if(getIndexOfWidgetList !=-1)
//                                                {
//                                                        $json_obj[getIndex].widgetList.splice(getIndexOfWidgetList,1);
//                                                }

//                                                $json_obj[getIndex].widgetList.push(obj);
//                                        }
//                                        else {
//                                                $json_obj[getIndex].widgetList.push(obj);
//                                        }

//                                });
//                        }
//                }

//        }
//})(xl_script||{});


//$(window).load(function(){
//        window.FontAwesomeConfig = {
//                searchPseudoElements: true
//        }


//});
//#endregion