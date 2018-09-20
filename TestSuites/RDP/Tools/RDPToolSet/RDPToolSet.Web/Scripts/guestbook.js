$(function () {
    
    $('#guestbook-reset').click(function () {
        $('#input-email').val("");
        $('#input-message').val("");
    });

    $('#guestbook-submit').click(function () {
        var email = $('#input-email').val();
        var message = $('#input-message').val();
        // validate the email and message
        if (email == "") {
            var alertBegin = GenerateAlert("danger"),
                content = "Error! Email field is empty. Please input a valid Email address.",
                alertEnd = "</div>";
            $('#status').html(alertBegin + content + alertEnd);
            return;
        }

        if (message == "") {
            var alertBegin = GenerateAlert("danger"),
                content = "Error! message field is empty. Please leave a message for us.",
                alertEnd = "</div>";
            $('#status').html(alertBegin + content + alertEnd);
            return;
        }

        var data = { email: email, message: message };
        $.ajax({
            type: 'POST',
            url: location.pathname + '/Save',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(data),
        }).done(function () {
            var alertBegin = GenerateAlert("success"),
                content = "Thanks! You message have been send to us successfully.",
                alertEnd = "</div>";
            $('#guestbook-form').hide();
            $('#status').html(alertBegin + content + alertEnd);
        });
    });

    function GenerateAlert(type) {
        return '<div class="alert alert-' + type + ' alert-dismissible" role="alert">' +
                    '<button type="button" class="close" data-dismiss="alert">' +
                      '<span aria-hidden="true">&times;</span>' +
                      '<span class="sr-only">Close</span>' +
                    '</button>';
    }
})