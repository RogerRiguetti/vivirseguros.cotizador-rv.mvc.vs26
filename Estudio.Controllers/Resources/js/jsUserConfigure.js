(function () {
    localStorage.pagina = 'User';
    "use strict";

    $("#btnSave").click(function () {

        if ($('#myform').valid()) {
            var strId = $('input[name=chkRole]:checked').map(function () {
                return $(this).attr('data-value');
            }).get().join(",");

            if (strId.length === 0) {
                $("#lblRequired").show();
                $("html, body").animate({ scrollTop: 0 }, "slow");
            }
            else {
                var fields =
                    {
                        Id: $('#id').val(),
                        strId: strId
                    }
                sendValues(fields);
            }
        } else {
            $("html, body").animate({ scrollTop: 0 }, "slow");
        }
    });

})();

function checkRequired(obj) {
    if (obj.checked)
        $("#lblRequired").hide();
}
function success() {
    $("#btnSave").hide();
    alertas("success", "Success");
}
function error() {
    alertas("danger", "Error");
}