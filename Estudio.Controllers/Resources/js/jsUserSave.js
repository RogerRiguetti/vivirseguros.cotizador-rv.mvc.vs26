
(function () {

    localStorage.pagina = 'Password';

    //"use strict";
    $("#btnSave").click(function () {
        if ($('#myform').valid() && $("#txtPassword").val() === $("#txtConfirm").val()) {
            $("#lblPasswordConfirm").hide();
            var IdSupervisor = $("#cmbxSupervisores").val();

          
                if (validateMail("txtCorreo")) {
                    var fields =
                   {
                       Id: (($('#url').val().indexOf('Create') > -1) ? 0 : ($('#id').val())),
                       Account: $("#txtAccount").val(),
                       Password: ($("#txtPassword").val().trim() === "") ? "-1" : $("#txtPassword").val().trim(),
                       UserProfileList: $("#cmbxRolesDetails").selectpicker('val'),
                       Correo: $("#txtCorreo").val().trim(),
                       IdSupervisor: IdSupervisor == null || IdSupervisor == "" ? 0 : $("#cmbxSupervisores").val(),
                       Names: $("#txtNombres").val().trim(),
                       LastNames: $("#txtApellidos").val().trim(),
                       NumeroAgente: $("#txtNumeroAgente").val().trim()
                   }
                    sendValues(fields, doSuccessUsuario, doErrorUsuario);
                }
                else {
                    aviso("Aviso", "El correo es incorrecto", null);
                }

        } else {
            $("html, body").animate({ scrollTop: 0 }, "slow");
            if ($("#txtPassword").val() != $("#txtConfirm").val()) { $("#lblPasswordConfirm").show(); }
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
            txtAccount: { required: true },
            txtPassword: { required: true },
            cmbxSupervisores: { required: true },
            txtCorreo: { required: true },
            txtNombres: { required: true },
            txtApellidos: { required: true },
            txtNumeroAgente: { required: true }
        }


    });
    var strUrl = $("#url").val();

    if (strUrl.includes('Create')) {
        llenarComboRolesCrear();
    }
    else {
        
        llenarComboRolesEditar();
    }
  
})();

function doSuccessUsuario(result) {
    aviso("Aviso", result.Message, result);
}

function doErrorUsuario(result) {
    aviso("Aviso", result.Message, result)
}

function aviso(header, body, result) {
    $('#msg_modal_header').text(header);
    var url;

    url = $("#urlIndex").val();

    $('#msg_modal_body').text(body);
    $('#sch_modal').modal('show');

    $("#btn_modal_aceptar").one('click', function () {
        $('#sch_modal').modal('hide');
        if (result.Object == null)
            window.location.href = url;
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

function validateMail(txtCorreo) {
    object = document.getElementById(txtCorreo);
    valueForm = object.value;

    var patron = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,4})+$/;
    if (valueForm.search(patron) == 0) {
        object.style.color = "#000";

        return true;
    }
    else {
        object.style.color = "#f00";

        return false;
    }
}

function IsNumeric(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "0123456789";
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
function CambioClave() {
    var password = $("#password").val();
    var passwordAnt = $("#txtAntPass").val();
    var passwordnuevo = $("#txtPass").val();
    var passwordconfi = $("#txtPassConf").val();

    if (password.trim() != passwordAnt.trim()) {
        aviso2("Aviso", "Antigua Password no coincide con la contraseña actual.", 0)
        return;
    }

    if (passwordnuevo.trim() == passwordAnt.trim()) {
        aviso2("Aviso", "La contraseña debe de ser diferente  a la actual.", 0)
        return;
    }
    if (passwordnuevo.trim() == "") {
        aviso2("Aviso", "Debe llenar el campo de Password Nueva", 0)
        return;
    }
    if (passwordconfi.trim() == "") {
        aviso2("Aviso", "Debe llenar el campo de Confirmar Password Nueva", 0)
        return;
    }
    if (passwordnuevo.length < 8) {
        aviso2("Aviso", "La contraseña debe contener por lo menos 8 caracteres", 0)
        return;
    }
    if (passwordnuevo.length > 20) {
        aviso2("Aviso", "La contraseña no debe contener mas de 20 caracteres", 0)
        return;
    }
    if (passwordnuevo == passwordconfi) {
        confirmarCambioClave("Aviso", "¿ Está seguro que desea modificar su contraseña ?", function () { ModificarClave(); });
        return;
    } else {
        aviso2("Aviso", "La contraseña nueva no coincide con la contraseña confirmada", 0)
        return;
    }
}
function ModificarClave() {
    var url = $("#urlCambioClave").val();
    var id = $("#id").val();
    var passwordnuevo = $("#txtPass").val();
    var fields = {
        Id: id,
        Pass: passwordnuevo.trim()
    };
    sendValues(fields, doSuccessClave, doError, url);
}
function doSuccessClave(result) {
    if (result.Message == "Usuario modificado con éxito") {
        aviso2("Aviso", result.Message, 1);
    } else {
        aviso2("Aviso", result.Message, 0);
    }
}
function doError(result) {
    aviso2("Aviso", result.Message, 0);
}
function confirmarCambioClave(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}

function aviso2(header, body, bandera) {
    $('#msg_modal_header').text(header);
    $('#msg_modal_body').text(body);
    $('#sch_modal').modal('show');

    $("#btn_modal_aceptar").one('click', function () {
        $('#sch_modal').modal('hide');
        if (bandera == 1) {
            var url = $("#urlSalir").val();
            window.location.href = url;
        }
    });
}


//Función para buscar con la tecla Enter.
function pulsar(e) {
    e.preventDefault();
    if (e.keyCode === 13) {
        document.getElementById("btnSave").click();

    }
}

function llenarComboRolesEditar() {
    var url = $("#urlConsultarRoles").val();
    var id = $('#txtId').val();
    var fields = {
        Id: id,
    };
    sendValues(fields, doSuccessConsultarRoles, doError, url);
}

function doSuccessConsultarRoles(result) {

    var rolesCatalogo = result.Object.rolesCatalogoJs;
    var rolesUsuario = result.Object.rolesUsuarioJs;

    var selectCombo = [];

    for (var i = 0; i < result.Object.rolesCatalogoJs.length; i++) {
        var rolItem = result.Object.rolesCatalogoJs[i];
        var o = new Option(rolItem.Name, rolItem.UserId);
        $(o).html(rolItem.Name);
        $("#cmbxRolesDetails").append(o);

        ///for de roles del usuario
        for (var r = 0; r < result.Object.rolesUsuarioJs.length; r++) {
            var itemAsignado = result.Object.rolesUsuarioJs[r];
            if (rolItem.UserId === itemAsignado.UserId) {
                selectCombo.push(itemAsignado.UserId)
            }
        }

    }
    $("#cmbxRolesDetails").selectpicker('val', selectCombo);
    $('#cmbxRolesDetails').selectpicker('refresh');
    selectCombo = [];
}


function llenarComboRolesCrear() {
    var url = $("#urlConsultarRolesCrear").val();
  
    sendValues(null, doSuccessConsultarRolesCrear, doError, url);
}

function doSuccessConsultarRolesCrear(result) {

    for (var i = 0; i < result.Object.rolesCatalogoJs.length; i++) {
        var rolItem = result.Object.rolesCatalogoJs[i];
        var o = new Option(rolItem.Name, rolItem.UserId);
        $(o).html(rolItem.Name);
        $("#cmbxRolesDetails").append(o);

        $('#cmbxRolesDetails').selectpicker('refresh');
    }
}