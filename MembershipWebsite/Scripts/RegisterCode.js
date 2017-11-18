$(function () {
    // store value from register code panel
    var code = $(".register-code-panel input");

    // Replace 'alert' text with 'message' text and format display
    function displayMessage(success, message) {
        var alert_div = $(".register-code-panel .alert");
        alert_div.text(message);
        if (success)
            alert_div.removeClass('alert-danger').addClass('alert-success');
        else
            alert_div.removeClass('alert-success').addClass('alert-danger');

        alert_div.removeClass('hidden');
    }

    $(".register-code-panel button").click(function (e) {
        $(".register-code-panel .alert").addClass('hidden');

        // Default message displayed in register code panel
        if (code.val().length == 0) {
            displayMessage(false, "Enter a code");
            return;
        }

        // Asynchronous AJAX call to register action
        $.post('/RegisterCode/Register', { code: code.val() },
        function (data) {
            displayMessage(true, "The code was successfully added. \n\r Please reload the page.");
            code.val('');
        }).fail(function (xlr, status, error) {
            displayMessage(false, "Could not register the code");
        });
    });
});