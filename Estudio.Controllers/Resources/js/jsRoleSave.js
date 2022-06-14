(function () {
    localStorage.pagina = 'Role';
    "use strict";

    $("#btnSave").click(function (result) {

        if ($('#myform').valid()) {
            var fields =
                {
                    // Id: (($('#url').val().indexOf('Create') > -1) ? 0 : ($('#id').val())),

                    Description: $("#txtDescription").val(),
                    Clave: $("#txtClave").val().trim(),

                }
            sendValues(fields, doSuccessRol, doErrorRol);

        } else {
            $("html, body").animate({ scrollTop: 0 }, "slow");
        }
    });

    $(".chosen-select").chosen({ width: "95%" });
})();

jQuery.validator.setDefaults({
    debug: true,
    success: "valid"
});

$("#myform").validate({
    errorClass: "requiredClass",
    rules: {
        txtDescription: { required: true },
        txtClave: { required: true }
    }
});

function doSuccessRol (result) {
    aviso("Aviso", result);
}

function doErrorRol (result){

}

function aviso(header, result) {
    $('#msg_modal_header').text(header);
    var url;

    if ($('#url').val().endsWith("Create")) 
        url = $("#urlcreate").val();
    else
        url = $('#urledit').val();

    $('#msg_modal_body').text(result.Message);
    $('#sch_modal').modal('show');

    $("#btn_modal_aceptar").one('click', function () {
        $('#sch_modal').modal('hide');
        if(result.Object == null)
            window.location.href = url;
    });
}

function confirmarRol(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}

function soloLetras(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
    especiales = "8-37-39-46";

    tecla_especial = false
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}
