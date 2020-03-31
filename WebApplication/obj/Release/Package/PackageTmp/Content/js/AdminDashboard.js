
function NewPanel(Id, heading, body,url) {
    $('#' + Id).DragablePanel({
        heading: heading,
        body: body,
        width: '500px'
        , url: url
    });
}

$(function () {
    
    
    var count = 0;
    $('#btnNew').click(function () {
        $('.DragBody').prepend('<div id="pantst' + count + '"  style="position:fix"></div>');

        NewPanel('pantst' + count, 'From Button' + count, 'Button Body')

        count++;

    });



    

   
    //$.removeCookie('name'); // => true
    //console.log($.cookie());

  

    var obj = SortByKey($.cookie())
    

    for (var key in obj) {
        
        var rec = key.split('-');
        if (rec.length > 1) {
          
            if (rec[0] == 'abcpanel') {
                $('.DragBody').prepend('<div id="' + rec[1] + '"  style="position:fix"> ');
                NewPanel(rec[1], rec[2], '', $.cookie()[key])
            } else {
              //  debugger;

                $('#' + rec[0]).css(rec[1], $.cookie()[key]);
            }

            
        }
    }

   // $('.DragBody').css('height','800px');
  

   
});