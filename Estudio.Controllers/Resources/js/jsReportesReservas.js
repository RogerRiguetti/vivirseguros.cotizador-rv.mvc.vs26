var primerslap = false;
var segundoslap = false;

(function () {
    localStorage.pagina = 'ReportesReservas';

    $("#btnFlujosPasivos").click(function () {
        ReporteFlujosPasivos();
    });

    $("#btnBaseSbs").click(function () {
        ReporteSbs();
    });

    $("#btnTxtFlujosPasivosNuevos").click(function () {
        TxtFlujosPasivosNuevos();
    });

    $("#btnTxtFlujosPasivosAntiguos").click(function () {
        TxtFlujosPasivosAntiguos();
    });

    $("#btnTxtFlujosPasivosMixtos").click(function () {
        TxtFlujosPasivosMixtos();
    });

    $("#btnResumenReservas").click(function () {
        ReporteResumenReservas();
    });

    $("#btnResumenAdecuacion").click(function () {
        ReporteResumenAdecuacion();
    });

})();

function ReporteFlujosPasivos() {
    if ($("#txtFechaPeriodo").val() == "") {
        aviso("Aviso", "Favor de ingresar fecha de periodo a buscar.");
        return;
    }

    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlReporteFlujosPasivos").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo);
}

function ReporteSbs() {
    if ($("#txtFechaPeriodo").val() == "") {
        aviso("Aviso", "Favor de ingresar fecha de periodo a buscar.");
        return;
    }

    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlReporteSbs").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo);
}

function TxtFlujosPasivosNuevos() {
    if ($("#txtFechaPeriodo").val() == "") {
        aviso("Aviso", "Favor de ingresar fecha de periodo a buscar.");
        return;
    }

    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlArchivoTxt").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo + "&pTipoArchivo=N");
}

function TxtFlujosPasivosAntiguos() {
    if ($("#txtFechaPeriodo").val() == "") {
        aviso("Aviso", "Favor de ingresar fecha de periodo a buscar.");
        return;
    }

    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlArchivoTxt").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo + "&pTipoArchivo=A");
}

function TxtFlujosPasivosMixtos() {
    if ($("#txtFechaPeriodo").val() == "") {
        aviso("Aviso", "Favor de ingresar fecha de periodo a buscar.");
        return;
    }

    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlArchivoTxt").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo + "&pTipoArchivo=M");
}

function ReporteResumenReservas() {
    if ($("#txtFechaPeriodo").val() == "") {
        aviso("Aviso", "Favor de ingresar fecha de periodo a buscar.");
        return;
    }

    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlReporteResumenReservas").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo);
}

function ReporteResumenAdecuacion() {
    if ($("#txtFechaPeriodo").val() == "") {
        aviso("Aviso", "Favor de ingresar fecha de periodo a buscar.");
        return;
    }

    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlReporteResumenAdecuacion").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo);
}

//Función para obtener cadena de fecha de periodo del input de la vista.
function cadenaFechaPeriodo() {
    var arrayFec = $("#txtFechaPeriodo").val().split("/");
    fecha = new Date(arrayFec[0], arrayFec[1], 0);
    var dia = fecha.getDate()
    var mes = (fecha.getMonth() + 1);
    if (mes <= 9) { mes = "0" + mes; }

    return fecha.getFullYear() + "" + mes + "" + dia
}

////////////////// VALIDACION DE INPUT PARA FECHA DE PERIODO ///////////////////////
function formateafechaYearMes(fecha) {
    var long = fecha.length;
    var dia;
    var mes;
    if ((long >= 4) && (primerslap == false)) {
        dia = fecha.substr(0, 4);
        if ((IsNumeric(dia) == true) && (dia <= 10000) && (dia != "0000")) {
            fecha = fecha.substr(0, 4) + "/" + fecha.substr(5, 2); primerslap = true;
        }
        else {
            fecha = ""; primerslap = false;
        }
    }
    else {
        dia = fecha.substr(0, 4);
        if (IsNumeric(dia) == false) {
            fecha = "";
        }
        if ((long <= 4) && (primerslap = true)) {
            fecha = fecha.substr(0, 4); primerslap = false;
        }
    }
    if (long >= 5) {
        mes = fecha.substr(5, 2);
        if (IsNumeric(mes) == false) {
            fecha = fecha.substr(0, 4) + "/";
        }

        if ((IsNumeric(mes) == true) && (mes <= 12) && (mes != "00")) {
            fecha = fecha.substr(0, 4) + "/" + fecha.substr(5, 2); segundoslap = true;
        }
        else {
            fecha = fecha.substr(0, 6); segundoslap = false;
        }
    }
    return (fecha);
}

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