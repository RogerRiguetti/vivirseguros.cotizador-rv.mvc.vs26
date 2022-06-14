(function () {
    localStorage.pagina = 'Role';
        "use strict";

        $("#btnSave").click(function () {

            if ($('#myform').valid()) {
                var strPermissionId = $('input[name=chkPermission]:checked').map(function () {
                    return $(this).attr('data-value');
                }).get().join(",");

                if (strPermissionId.length === 0) {
                    $("#lblRequired").show();
                    $("html, body").animate({ scrollTop: 0 }, "slow");
                }
                else {
                    var fields =
                        {
                            Id: $('#id').val(),
                            strPermissionId: strPermissionId
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