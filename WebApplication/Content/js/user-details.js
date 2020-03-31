/*----MoB!----*/
$(function () {
    $('#frm-User').parsley();
    $('#frm-password').parsley();
    $('#msg').hide();
    var Email = $('#Email').val();
    $('#Email').blur(function () {
        if ($(this).val() != Email) {
            IsExist('#Email', 'byEmail', $(this).val());

        }
    });

    $("#frm-password").submit(function (e) {

        e.preventDefault();
        $.ajax({
            url: '/user/UpdatePassword',
            type: 'post',
            data: $(this).serialize(),
            success: function (res) {
                if (res == 'True') {
                    $('#pan-msg').text('Change password successfully.');
                    $('#msg').show();
                  
                }
            },
            error: function (err) { alert(err.statusText) }

        });
        return false;
    });

    function IsExist(selector, filter, value) {
        $.ajax({
            url: '/User/IsExist?filter=' + filter + '&value=' + value,
            success: function (res) {
                if (res == 'True') {
                    $(selector + 'Msg').text('Already Exist ' + value)
                    $(selector).val('');
                } else {
                    $(selector + 'Msg').text('')
                }
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    }

    $("#uploadFile").on("change", function () {
        var files = !!this.files ? this.files : [];
        if (!files.length || !window.FileReader) return; // no file selected, or no FileReader support

        if (/^image/.test(files[0].type)) { // only image file
            var reader = new FileReader(); // instance of the FileReader
            reader.readAsDataURL(files[0]); // read the local file

            reader.onloadend = function () { // set image data as background of div
                $("#imagePreview").empty();
                $("#imagePreview").css("background-image", "url(" + this.result + ")");
            }
        }
    });
});