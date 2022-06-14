var doSuccessLogOut = function (result) {

    window.location = $('#UrlLogin').val();
}

var doErrorLogOut = function (result) {
    error('Problems');

}

var date = new Date();
var fechaActual = String(date.getDate()).padStart(2, '0') + "/" + String(date.getMonth() + 1).padStart(2, '0') + "/" + date.getFullYear();
var primerslap = false;
var segundoslap = false;

$(function () {

    $('#modalFechaB').datepicker({
        select: function (e, type) {
            document.getElementById('modalFechaB').focus();
        },
        value: fechaActual,
        format: 'dd/mm/yyyy',
        showOnFocus: false
    });

    
    $("#modalFechaB").val(fechaActual);
    // Peticiones para mantener la sesión activa
    setInterval(function () {
        $.get($("#UrlRenewSession").val());
    }, 1000 * 30);

    $("#btnMenu").click(function () {
        var v = $("#wrapper").attr('data-v');

        if ($("#btnMenu").css('left') == "0px") {
            $("#btnMenu").animate({ "left": 170 }, 1000);
            $("#page-wrapper").animate({ "margin-left": 182 }, 1000);
            $("#menuSidex").fadeIn(1000);

        }
        else {
            $("#btnMenu").animate({ "left": 0 }, 1000);
            $("#page-wrapper").animate({ "margin-left": 12 }, 1000);
            $("#menuSidex").fadeOut(1000);
        }
    });

    $('#lnkLogOut').click(function () {

        var fields = { id: "" }
        var method = $("#UrlLogOut").val();

        sendValues(fields, doSuccessLogOut, doErrorLogOut, method);
    });

    // bootstrap tooltips
    $('[data-toggle="tooltip"]').tooltip();

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

    $(".input .input_field").each(function (i, e) {
        if (e.value.trim() !== '') {
            $(e.parentNode).addClass('input_filled');
        }

        $(e).on('focus', onInputFocus);
        $(e).on('blur', onInputBlur);
    });

    function onInputFocus(ev) {
        $(ev.target.parentNode).addClass('input_filled');
    }

    function onInputBlur(ev) {
        if (ev.target.value.trim() === '') {
            $(ev.target.parentNode).removeClass('input_filled');
        }
    }
});

$.validator.messages.required = "Requerido";


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

// Begin Alerts
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
// End Alerts
function IsNumeric(valor) {
    var log = valor.length; var sw = "S";
    for (x = 0; x < log; x++) {
        v1 = valor.substr(x, 1);
        v2 = parseInt(v1);
        //Compruebo si es un valor numérico 
        if (isNaN(v2)) { sw = "N"; }
    }
    if (sw == "S") { return true; } else { return false; }
}

function formateafecha(fecha) {
    var long = fecha.length;
    var dia;
    var mes;
    var annio;
    if ((long >= 2) && (primerslap == false)) {
        dia = fecha.substr(0, 2);
        if ((IsNumeric(dia) == true) && (dia <= 31) && (dia != "00")) { fecha = fecha.substr(0, 2) + "/" + fecha.substr(3, 7); primerslap = true; }
        else { fecha = ""; primerslap = false; }
    }
    else {
        dia = fecha.substr(0, 1);
        if (IsNumeric(dia) == false)
        { fecha = ""; }
        if ((long <= 2) && (primerslap = true)) { fecha = fecha.substr(0, 1); primerslap = false; }
    }
    if ((long >= 5) && (segundoslap == false)) {
        mes = fecha.substr(3, 2);
        if ((IsNumeric(mes) == true) && (mes <= 12) && (mes != "00")) { fecha = fecha.substr(0, 5) + "/" + fecha.substr(6, 4); segundoslap = true; }
        else { fecha = fecha.substr(0, 3);; segundoslap = false; }
    }
    else { if ((long <= 5) && (segundoslap = true)) { fecha = fecha.substr(0, 4); segundoslap = false; } }
    if (long >= 7) {
        annio = fecha.substr(6, 4);
        if (IsNumeric(annio) == false) { fecha = fecha.substr(0, 6); }
        else { if (long == 10) { if ((annio == 0) || (annio < 1900) || (annio > 2100)) { fecha = fecha.substr(0, 6); } } }
    }
    if (long >= 10) {
        fecha = fecha.substr(0, 10);
        dia = fecha.substr(0, 2);
        mes = fecha.substr(3, 2);
        annio = fecha.substr(6, 4);
        // Año no viciesto y es febrero y el dia es mayor a 28 
        if ((annio % 4 != 0) && (mes == 02) && (dia > 28)) { fecha = fecha.substr(0, 2) + "/"; }
        if ((mes == 02) && (dia > 29)) { fecha = fecha.substr(0, 2) + "/"; }
    }
    return (fecha);
}
