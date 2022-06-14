$(function () {
    document.getElementById("txtUser").focus();
    // bootstrap tooltips
    $('[data-toggle="tooltip"]').tooltip();

    $('#side-menu').metisMenu();

    // Modal
    $('#sch_modal').modal({
        keyboard: false,
        backdrop: "static",
        show: false
    });

    // Boton Close Alertas
    $("#sch_alert_close").on('click', function () {
        $("#sch_alert").hide('slow', function () {
            $("#sch_alert").attr('class', 'sch_alert hidden');
            $("#sch_alert").attr('style', '');
        });
    });

    // Script para los campos de texto personalizados
    // Add Trim function to String
    if (!String.prototype.trim) {
        (function () {
            var rtrim = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g;
            String.prototype.trim = function () {
                return this.replace(rtrim, '');
            };
        })();
    }

    $('#lnkEnter').click(function () {
        var usuario = $('#txtUser').val().trim();
        var password = $('#txtPassword').val().trim();

        if (usuario == "" || password == "") {
            aviso("Aviso", "Favor de ingresar todos los datos");
            return;
        }

        var fields = {
            usuario: usuario,
            password: password,
        }

        var method = $("#UrlValidateLogin").val();

        $.getScript("../Resources/js/System.js", function () {
            sendValues(fields, doSuccessLogin, doErrorLogin, method);
        });
    });
}); // Fin del Ready

$.validator.messages.required = "requerido";

// Begin Modal
function confirmar(header, body, fnaceptar) {

    $('#msg_modal_header').text(header);
    $('#msg_modal_body').text(body);
    $('#sch_modal_confirm').modal('show');


    $("#btn_modal_aceptar").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}

function aviso(header, body) {

    $('#msg_modal_header').text(header);
    $('#msg_modal_body').text(body);
    $('#sch_modal').modal('show');

    $("#btn_modal_aceptar").one('click', function () {
        $('#sch_modal').modal('hide');
        ///document.getElementById("txtUser").focus();
    });
}
// End Modal

// Begin Alertas
function alertas(type, message) {
    $("#sch_alert").attr('class', 'sch_alert alert alert-' + type);
    $("#sch_alert_message").text(message);
    setTimeout(function () {
        $("#sch_alert").hide('slow', function () {

            $("#sch_alert").attr('class', 'sch_alert hidden');
            $("#sch_alert").attr('style', '');
        });
    }, 5000);
}

var doSuccessLogin = function (result) {

    window.location = $('#UrlIndex').val();
}

var doErrorLogin = function (result) {
    aviso("Aviso", result.Message);

}

function success() { alert('bien'); }

function error() { alert('mal'); }

function pulsar(e) {
    e.preventDefault();
    if (e.keyCode === 13) {
        document.getElementById("lnkEnter").click();
            
    }
}

function pulsarmodal(e) {
    e.preventDefault();
    if (e.keyCode === 13) {
        document.getElementById("btn_modal_aceptar").click();
    }
}
