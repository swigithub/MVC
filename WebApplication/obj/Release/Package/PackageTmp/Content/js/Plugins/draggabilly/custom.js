(function ($) {


    $.fn.DragablePanel = function (options) {

        var thisId = $(this)[0].id;

        var settings = $.extend({

            heading: null,
            body: null,
            width: null,
            height: null,
            url: null,
            click: null
        }, options);
        if (settings.width == null) {
            settings.width = '500px';
        }

        if (settings.height == null) {
            settings.height = '500px';
        }


        $.cookie('abcpanel-' + thisId + '-' + settings.heading, settings.url);


        var ParentWidth = $('#' + thisId).parent().width();
        var widthKey = thisId + "-width";
        var heightKey = thisId + "-height";

        if ($.cookie(widthKey) == undefined) {
            $.cookie(widthKey, settings.width);
        } else {
            settings.width = $.cookie(widthKey);
        }

        if ($.cookie(heightKey) == undefined) {
            $.cookie(heightKey, settings.height);
        } else {
            settings.height = $.cookie(heightKey);
        }

        if ($.cookie(thisId+'-left') == undefined) {

        }

        if (settings.url != null) {
            $.ajax({
                url: settings.url,
                async: false,
                success: function (res) {
                    settings.body = res;
                }
            });
        }
        


        $(this).prepend('<div class="box box-info" style="width:' + settings.width + ';height:' + settings.height + '" data-parent="' + thisId + '"> ' +
                       '<div class="box-header with-border" ><h3 style="cursor:move" class="box-title">' + settings.heading + '</h3>' +
                       '<div class="box-tools pull-right">' +
                       '<a href="#" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></a>' +
                       '<a href="#"  onclick="btnRemove(\'' + $(this)[0].id + '\'); return false;" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></a>' +
                       '<div class="splitter"></div>' +
                       '</div></div>' +
                       '<div class="box-body" style="display: block;">' + settings.body + '</div> ')
        // $(this).addClass('col-sm-' + settings.width);



        $("#"+thisId+" .box").resizable({
            stop: function (event, ui) {
                var panel = event.target.dataset['parent'];
                var h = event.target.offsetHeight;
                var w = event.target.offsetWidth;

                $.cookie(panel + "-width", w + 'px');
                $.cookie(panel + "-height", h + 'px');
            }
        });

        var $draggable = $(this).draggabilly({
            containment: '.DragBody',
            handle: '.box-title',
            grid: [20, 20]
        });

        $draggable.on('dragEnd', function (event, pointer) {
            var draggie = $(this).data('draggabilly');
            $.cookie(event.currentTarget.id + "-left", draggie.position.x + 'px');
            $.cookie(event.currentTarget.id + "-top", draggie.position.y + 'px');
        });



        return this.each(function () {
            if (settings.width) {
                $(this).css('width', settings.width);
            }
            if (settings.height) {
                $(this).css('height', settings.height);
            }
            if (settings.color) {
                $(this).css('color', settings.color);
            }

            if (settings.fontStyle) {
                $(this).css('font-style', settings.fontStyle);
            }

            if ($.isFunction(options.click)) {
                $(this).click(function () {
                    options.click.call(this);
                });
            }
        });

    }

}(jQuery));


function btnRemove(panel) {
   
    for (var key in $.cookie()) {
        if (key.startsWith(panel + '-')) {
            $.removeCookie(key);
        } else if (key.startsWith('abcpanel-' + panel)) {
            $.removeCookie(key);
        }
    }
}

//function btnMenu(panel) {
//    $('#' + panel + '-menu').toggle();
//}

//function ChangeWidth(e, panel) {
//    var intRegex = /^\d+$/;
//    var value = e.value;

//    if (intRegex.test(value)) {
//        if (parseInt(value) < parseInt(e.max)) {

//            $('#' + panel).css('width', value);
//            $.cookie(panel + "-width", value + 'px');
//        }

//    }

//}