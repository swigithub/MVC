
; (function ($, window, document, undefined) {

    
    Date.prototype.toInputFormat = function () {
        var yyyy = this.getFullYear().toString();
        var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
        var dd = this.getDate().toString();
        return (mm[1] ? mm : "0" + mm[0]) + "/" + (dd[1] ? dd : "0" + dd[0]) + "/" + yyyy; // padding
    };
})(jQuery, this, document);



; (function ($, window, document, undefined) {


    Date.prototype.toMMyyy = function () {
        var m_names = new Array("Jan", "Feb", "Mar",
                                "Apr", "May", "Jun", "Jul", "Aug", "Sep",
                                "Oct", "Nov", "Dec");

        var yyyy = this.getFullYear().toString();
        var mm = (this.getMonth()).toString(); // getMonth() is zero-based
       // var dd = this.getDate().toString();
        return (m_names[mm])  + " " + yyyy; // padding
    };
})(jQuery, this, document);

/*
var m_names = new Array("Jan", "Feb", "Mar", 
"Apr", "May", "Jun", "Jul", "Aug", "Sep", 
"Oct", "Nov", "Dec");

var d = new Date();
var curr_date = d.getDate();
var curr_month = d.getMonth();
var curr_year = d.getFullYear();
document.write(curr_date + "-" + m_names[curr_month] 
+ "-" + curr_year);
*/


var Today = new Date();



//$.ajax({
//    url: '/DateRight/UserDates/' + $('#-uid').val(),
//    async: false,
//    success: function (res) {
//        UserDaysForward = res.DaysForward;
//        UserDayBack = res.DaysBack;
//    }
//});


function AddDays(EnteredDays) {
   // debugger;
    var date = new Date(Today),
              days = EnteredDays;// parseInt(EnteredDays+1, 10);

    if (!isNaN(date.getTime())) {
        date.setDate(date.getDate() + days);
        return date.toInputFormat()
    }
    return null;
}




$('.nextDates').datepicker({
    format: 'mm/dd/yyyy',
    startDate: Today,
    autoclose: true
});

$('.previouseDates').datepicker({
    format: 'mm/dd/yyyy',
   // startDate: FromEndDate,
    endDate: Today,
    autoclose: true
});

$('.date').datepicker({
    format: 'mm/dd/yyyy',
    autoclose: true
});


$(document).on('click', '.LiveDate', function () {
    $(this).datepicker({
        format: 'mm/dd/yyyy',
        autoclose: true
    }).focus();
    $(this).removeClass('datepicker');
});

$('.UserDate').datepicker({
    format: 'mm/dd/yyyy',
    startDate: AddDays(-UserDaysForward),
    endDate: AddDays(UserDayBack),
    autoclose: true
});

$(document).on('click', '.UserLiveDate', function () {
    $(this).datepicker({
        format: 'mm/dd/yyyy',
        startDate: AddDays(-UserDayBack),
        endDate: AddDays(UserDaysForward),
        autoclose: true
    }).focus();
    $(this).removeClass('datepicker');
});
