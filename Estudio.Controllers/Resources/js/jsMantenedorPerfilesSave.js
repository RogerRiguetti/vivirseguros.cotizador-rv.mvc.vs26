
(function () {

    localStorage.pagina = 'MantenedorPerfiles';
    $("#btnSave").click(function () {
        if ($('#myform').valid()) {
            if ($('#myform').valid()) {
                var id = obtenerValorParametro('id');
                var url = $("#url").val();
                var fields =
                {
                    Id: id,
                    NamePantalla: $("#txtNamePantalla").val().trim(),
                    clave: $("#txtClave").val().trim(),
                    descripcionLarga: $("#txtDescripcion").val().trim(),
                    sistemas_Idsistema: $("#cmbxSistemas").val(),
                    nodoPadre: $("#txtNodoPadre").val()
                    
                }
                sendValues(fields, doSuccessPerfil, doErrorPerfil, url);
                $("#txtNamePantalla").val("");
                $("#txtClave").val("");
                $("#txtDescripcion").val("");
                $("#txtNodoPadre").val("");

            }
        }
    });

    $(".chosen-select").chosen({ width: "95%" });

    // just for the demos, avoids form submit
    jQuery.validator.setDefaults({
        debug: true,
        success: "valid"
    });

    $("#myform").validate({
        errorClass: "requiredClass",
        rules: {
            txtNamePantalla: { required: true },
            txtClave: { required: true },
            txtDescripcion: { required: true },
            cmbxSistemas: { required: true },
            txtNodoPadre: { required: true },
        }


    });   

})();

function obtenerValorParametro(sParametroNombre) {
    var sPaginaURL = window.location.search.substring(1);
    var sURLVariables = sPaginaURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParametro = sURLVariables[i].split('=');
        if (sParametro[0] == sParametroNombre) {
            return sParametro[1];
        }
    }
    return null;
}


function doSuccessPerfil(result) {
    aviso("Aviso", result.Message, result);
}

function doErrorPerfil(result) {
    aviso("Aviso", result.Message, result)
}