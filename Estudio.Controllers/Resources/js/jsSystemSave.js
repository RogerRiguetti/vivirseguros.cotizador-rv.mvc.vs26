(function () {
    "use strict";

    $("#btnSave").click(function () {

        if ($('#myform').valid()) {
            var fields =
                {
                    Id: (($('#url').val().indexOf('Create') > -1) ? 0 : ($('#id').val())),
                    Description: $("#txtDescription").val()
                }
            sendValues(fields);
        } else {
            $("html, body").animate({ scrollTop: 0 }, "slow");
        }
    });

    $(".chosen-select").chosen({ width: "95%" });

})();

function success() {
    $("#btnSave").hide();
    alertas("success", "Success");
    $('#txtDescription').attr('disabled', 'true');
}

function error() {
    alertas("danger", "Error");
}