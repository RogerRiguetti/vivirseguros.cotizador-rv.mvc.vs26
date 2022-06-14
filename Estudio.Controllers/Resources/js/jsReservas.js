var count = 100;
var bandPresCargar = false;
var bandPresCargarflujo = false;
var bandPresCargarcalcular = false;
var banBarra = false;
var banBarraflujo = false;
var banBarracalcular = false;
var banAviso = false;
var banAvisoflujo = false;
var banAvisocalcular = false;
var mensajeTermino = "";
var mT = "Carga Terminada con Éxito";
var mTflujo = "Flujos Guardados con éxito.";
var mTcalcular = "Reserva Base Matemática con éxito.";
var paso = 0;
var mensaje = $("#urlMensaje").val();
var pathFile = "";
var nombreArchivo = "";

(function () {
    localStorage.pagina = 'Reservas';
    //Funcion para traer el ultimo periodo abierto
    var urlUPeriodo = $("#urlUPeriodo").val();
    var fields = {
    };
    sendValues(fields, doSuccessUPeriodo, doError, urlUPeriodo);
    //Funcion de la carga masiva
    $('#tituloCargaMasiva').click(() => {
        $('#contCargaMasiva').slideToggle('normal');
    });

    if (mensaje != null && mensaje != "") {
        $('#modalcargar').modal("hide");
        aviso("AVISO", mensaje);
        mensaje = "";
    }
    $("#btnProCartera").click(function () {
        validarCalculoFlujos();
    });

    $("#btnProCalcular").click(function () {
        CalculoReservaValidacion();
    });

    $("#btnReporte2").click(function () {
        ReporteResumenReservas();
    });

    $("#btnReporteContable").click(function () {
        ReporteContable();
    });

    $("#btnResCartera").click(function () {
        RptFlujosCartera();
    });

    $("#btnResCarteraAnt").click(function () {
        RptFlujosCarteraAnt();
    });

    $("#btnResCalcular").click(function () {
        exportarCalculoRes();
    });

})();

//dosuccess ultimo periodo abierto
function doSuccessUPeriodo(result) {
    var mes = result.Object;
    $('#txtMesCal').val(mes.substr(0, 4) + "/" + mes.substr(4, 2));
}

$(document).ready(function () {
    //$(document).bind("ajaxSend", function () {
    //    //Validaciones barra proceso de migracion
    //    //if (bandPresCargar == true && banBarra == true) {
    //    //    mensajeTermino = "";
    //    //    banBarra = false;
    //    //    paso = 1;
    //    //    //Barra();
    //    //}
    //    //Validaciones barra calculo de flujo
    //    //if (bandPresCargarflujo == true && banBarraflujo == true) {
    //    //    mensajeTermino = "";
    //    //    banBarraflujo = false;
    //    //    paso = 1;
    //    //    //Barraflujo();
    //    //}
    //    ////Validaciones barra calcular
    //    //if (bandPresCargarcalcular == true && banBarracalcular == true) {
    //    //    mensajeTermino = "";
    //    //    banBarracalcular = false;
    //    //    paso = 1;
    //    //    //Barracalcular();
    //    //}
    //}).bind("ajaxComplete", function () {
    //    //Validaciones borra proceso de migracion
    //    //if (bandPresCargar == true && mensajeTermino == mT) {
    //    //    mensajeTermino = "";
    //    //    banBarra = false;
    //    //    paso = 2;
    //    //    //Barra();
    //    //}
    //    //valiadaciones barra calculo de flujo
    //    if (bandPresCargarflujo == true && mensajeTermino == mTflujo) {
    //        mensajeTermino = "";
    //        banBarraflujo = false;
    //        paso = 2;
    //        //Barraflujo();
    //    }
    //    //Validaciones barra calcular
    //    if (bandPresCargarcalcular == true && mensajeTermino == mTcalcular) {
    //        mensajeTermino = "";
    //        banBarracalcular = false;
    //        paso = 2;
    //        //Barracalcular();
    //    }
    //});

});

$("#txtMesCal").keydown(function (event) {
    if (event.shiftKey) {
        event.preventDefault();
    }

    if (event.keyCode == 46 || event.keyCode == 8) {
    }
    else {
        if (event.keyCode < 95) {
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.preventDefault();
            }
        }
        else {
            if (event.keyCode < 96 || event.keyCode > 105) {
                event.preventDefault();
            }
        }
    }
});

//valiadaciones periodos
function Buscar() {

    if ($('#txtMesCal').val().length < 7 || $('#txtMesCal').val() == "") {
        count = 0;
        //$('#barMigracion').css('width', count + "%");
        //document.getElementById("demo").innerHTML = count + "%";

        bandPresCargar = false;
        aviso("ERROR", "Fecha Incorrecta");
    } else {
        count = 0;
        //$('#barMigracion').css('width', count + "%");
        //document.getElementById("demo").innerHTML = count + "%";

        var urlPeriodo = $("#urlPeriodo").val();
        var fechaPeriodo = cadenaFechaPeriodo();

        var fields = {
            fecha: fechaPeriodo
        };
        sendValues(fields, doSuccessPeriodo, doError, urlPeriodo);
    }

}
//Inicio proceso de migracion
function calculo() {
    $('#ProMigracion').removeClass('fa-circle processing fa-check-circle');
    $('#ProMigracion').addClass('fa-pulse fa-spinner unprocessed');
    $(".panel-body button").attr("disabled", "disabled");//Deshabilitar botones
    bandPresCargar = true;
    count = 0;
    //banBarra = true;
    var urlIniciar = $("#urlIniciar").val();
    var fechaPeriodo = cadenaFechaPeriodo();

    var fields = {
        fecha: fechaPeriodo
    };
    sendValues(fields, doSuccessIniciar, doError, urlIniciar);
}

function doSuccessIniciar(result) {
    $('#ProMigracion').removeClass('fa-pulse fa-spinner unprocessed');
    $('#ProMigracion').addClass('processing fa-check-circle');

    $('#txtProMigracion').removeClass('unprocessed');
    $('#txtProMigracion').addClass('textProcess');
    $(".panel-body button").removeAttr("disabled");
    
    banAviso = true;
    mensajeTermino = result.Message;
    aviso("AVISO", result.Message);
}
//Validaciones periodos
function doSuccessPeriodo(result) {
    var men = result.Message;
    if (men == "Abierto") {
        if (banAviso == true) {
            confirmar("Aviso", "¿Seguro Que Desea Volver a Iniciar el Proceso de Migracion?", function () { calculo(); });
        } else {
            calculo();
        }

    }
}
//Validaciones errores
function doError(result) {
   // aviso("ERROR", result.Message);

    if (bandPresCargar == true && banBarra == false) {
        mensajeTermino = "";
        mensaje = "";
        banBarra = false;
        bandPresCargar = false;
        count = 0;

        $('#ProMigracion').removeClass('fa-pulse fa-spinner');
        $('#ProMigracion').addClass('fa-circle');

        $(".panel-body button").removeAttr("disabled");//habilitar botones
    }
    if (bandPresCargarflujo == true && banBarraflujo == false) {
        mensajeTermino = "";
        mensaje = "";
        banBarraflujo = false;
        bandPresCargarflujo = false;
        count = 0;

        $('#ProCartera').removeClass('fa-pulse fa-spinner');
        $('#ProCartera').addClass('fa-circle');
        $(".panel-body button").removeAttr("disabled");
        //$('#barCartera').css('width', 0 + "%");
        //document.getElementById("demoflujo").innerHTML = count + "%";
        //$("input[type=button]").removeAttr("disabled");//habilitar botones
    }
    if (bandPresCargarcalcular == true && banBarracalcular == false) {
        mensajeTermino = "";
        mensaje = "";
        banBarracalcular = false;
        bandPresCargarcalcular = false;
        count = 0;
        
        $('#ProReserva').removeClass('fa-pulse fa-spinner');
        $('#ProReserva').addClass('fa-circle');
        $(".panel-body button").removeAttr("disabled");

        //$('#barCalcular').css('width', 0 + "%");
        //document.getElementById("democalcular").innerHTML = count + "%";
        //$("input[type=button]").removeAttr("disabled");//habilitar botones
    }
}
//Validaciones barra proceso de migracion
//function Barra() {
//    var myVar = setInterval(function () {
//        myTimer();
//    }, 1);
//    function myTimer() {
//        if (paso == 1) {
//            if (count < 90 && bandPresCargar == true) {
//                count += avance = 0.001;
//                $('#barMigracion').css('width', Math.round(count) + "%");
//                document.getElementById("demo").innerHTML = Math.round(count) + "%";
//                mensajeTermino = "";
//            }
//        }
//        if (paso == 2) {
//            if (count < 100 && bandPresCargar == true) {
//                count += 0.05;
//                $('#barMigracion').css('width', Math.round(count) + "%");
//                document.getElementById("demo").innerHTML = Math.round(count) + "%";
//                mensajeTermino = "";
//            }
//            else if (count > 100) {
//                count = 0;
//                paso = 0;
//                bandPresCargar = false;
//                banAviso = true;
//                $("input[type=button]").removeAttr("disabled");//habilitar botones
//                aviso("AVISO", mT);
//            }
//        }
//    }
//}
//valiadaciones barra calculo de flujo
function Barraflujo() {
    //var myVar = setInterval(function () {
    //    myTimer();
    //}, 1);
    //function myTimer() {
    //    if (paso == 1) {
    //        if (count < 90 && bandPresCargarflujo == true) {
    //            count += avance = 0.0001;
    //            //$('#barCartera').css('width', Math.round(count) + "%");
    //            //document.getElementById("demoflujo").innerHTML = Math.round(count) + "%";
    //            mensajeTermino = "";
    //        }
    //    }
    //    if (paso == 2) {
    //        if (count < 100 && bandPresCargarflujo == true) {
    //            count += 0.05;
    //            //$('#barCartera').css('width', Math.round(count) + "%");
    //            //document.getElementById("demoflujo").innerHTML = Math.round(count) + "%";
    //            mensajeTermino = "";
    //        }
    //        else if (count > 100) {
    //            count = 0;
    //            paso = 0;
    //            bandPresCargarflujo = false;
    //            banAvisoflujo = true;
    //            $("input[type=button]").removeAttr("disabled");//habilitar botones
    //            aviso("AVISO", mTflujo);
    //        }
    //    }
    //}
}
//Validaciones barra calcular
function Barracalcular() {
    //var myVar = setInterval(function () {
    //    myTimer();
    //}, 1);
    //function myTimer() {
    //    if (paso == 1) {
    //        if (count < 90 && bandPresCargarcalcular == true) {
    //            count += avance = 0.00015;
    //            //$('#barCalcular').css('width', Math.round(count) + "%");
    //            //document.getElementById("democalcular").innerHTML = Math.round(count) + "%";
    //            mensajeTermino = "";
    //        }
    //    }
    //    if (paso == 2) {
    //        if (count < 100 && bandPresCargarcalcular == true) {
    //            count += 0.05;
    //            //$('#barCalcular').css('width', Math.round(count) + "%");
    //            //document.getElementById("democalcular").innerHTML = Math.round(count) + "%";
    //            mensajeTermino = "";
    //        }
    //        else if (count > 100) {
    //            count = 0;
    //            paso = 0;
    //            bandPresCargarcalcular = false;
    //            banAvisocalcular = true;
    //            $("input[type=button]").removeAttr("disabled");//habilitar botones
    //            aviso("AVISO", mTcalcular);
    //        }
    //    }
    //}
}

function doErrorCurvaTasas(result) {
   // $('#modalcargar').modal('hide');
    aviso("ERROR", result.Message);
}

function exportarExcelReservas() {
    var url = $("#ExportarCsv").val();
    window.open(url); // + "?FecPeriodo=" + fecha.getFullYear() + "" + mes);
}

function exportarCalculoRes() {
    var url = $("#ExportarCalculoRes").val();
    window.open(url);
}

//Función para exportar Rpt Resumen Reservas
function exportarResumenRes() {
    var url = $("#urlRptResumenRes").val();
    window.open(url);
}

//Funciones para carga de archivo Excel.
function cambiarFile() {
    var input = document.getElementById('file-input');
    if (input.files && input.files[0]) {
        var nombre = input.files[0].name;
        if (esEXCEL(nombre) == true) {
            $('#txtArchivoCargado').val(nombre);
            $('#file-submit').prop("disabled", false);
        }
        else {
            aviso("ERROR", "El archivo no es extensión .xlxs o .xls");
            $('#txtArchivoCargado').val("");
        }
    } else {
        $('#txtArchivoCargado').val("");
    }
}

function esEXCEL(doc) {
    var array = doc.split(".");
    var res = false;
    if (array[array.length - 1] == "xlsx" || array[array.length - 1] == "XLSX" || array[array.length - 1] == "XLS" || array[array.length - 1] == "xls") {
        res = true;
    }
    return res;
}


//Función para validar que existan registros y proceder al Cálculo de Flujos de Carteras.
function validarCalculoFlujos() {
    if ($('#txtMesCal').val().length < 7 || $('#txtMesCal').val() == "") {
        aviso("ERROR", "Fecha Incorrecta");
        return;
    }
    var fechaPeriodo = cadenaFechaPeriodo();

    var urlCurvaTasas = $("#urlValidaCurvaTasas").val();
    var fields = {
        FecCal: fechaPeriodo
    };
    sendValues(fields, doSuccessValidaCurvaTasas, doErrorCurvaTasas, urlCurvaTasas);
}

function doSuccessValidaCurvaTasas(result)
{
    var urlFlujos = $("#urlValidaCalculoFlujos").val();
    var fields = {

    };
    sendValues(fields, doSuccessValidaFlujos, doError, urlFlujos);
}
//valiadaciones barra calculo de flujo
function doSuccessValidaFlujos(result) {
    var msj = result.Message;

    if (msj != "ERROR.") {
        if (banAvisoflujo == true) {
            count = 0;
            //$('#barCartera').css('width', 0 + "%");
            //document.getElementById("demoflujo").innerHTML = count + "%";
            confirmar("Aviso", "¿Seguro Que Desea Volver a Iniciar el Calculo de Flujo Total de Cartera?", function () { calculo_Flujos(); });
        } else {
            calculo_Flujos();
        }
    } else {
        aviso("ERROR", "Favor de verificar que se haya realizado \nel proceso de migración de pólizas anteriormente.");
        return;
    }
}

//Función para iniciar Cálculo de Flujos Totales de Cartera.
function calculo_Flujos() {
    if ($('#txtMesCal').val().length < 7 || $('#txtMesCal').val() == "") {
        aviso("ERROR", "Fecha Incorrecta");
        return;
    }

    $('#ProCartera').removeClass('fa-circle processing fa-check-circle');
    $('#ProCartera').addClass('fa-pulse fa-spinner unprocessed');
    $(".panel-body button").attr("disabled", "disabled");//Deshabilitar botones

    //$("input[type=button]").attr("disabled", "disabled");
    $("#btnLog").removeAttr("disabled");
    bandPresCargarflujo = true;
    count = 0;
    //banBarraflujo = true;
    var urlFlujos = $("#urlCalculoFlujos").val();
    var fechaPeriodo = cadenaFechaPeriodo();

    var fields = {
        FecCal: fechaPeriodo
    };
    sendValues(fields, doSuccessFlujos, doError, urlFlujos);
}

function doSuccessFlujos(result) {

    $('#ProCartera').removeClass('fa-pulse fa-spinner unprocessed');
    $('#ProCartera').addClass('processing fa-check-circle');

    $('#txtProCartera').removeClass('unprocessed');
    $('#txtProCartera').addClass('textProcess');
    $(".panel-body button").removeAttr("disabled");

    banAvisoflujo = true;
    mensajeTermino = result.Message;
    aviso("AVISO", result.Message);
}

//Función validación Cálculo de Reseervas
function CalculoReservaValidacion() {
    if (banAvisocalcular == true) {
        count = 0;
        //$('#barCalcular').css('width', 0 + "%");
        //document.getElementById("democalcular").innerHTML = count + "%";
        confirmar("Aviso", "¿Seguro Que Desea Volver a Calcular Reserva?", function () { CalculoReserva(); });
    } else {
        CalculoReserva();
    }
}

//Función para iniciar Cálculo de Reservas.
function CalculoReserva() {

    $('#ProReserva').removeClass('fa-circle processing fa-check-circle');
    $('#ProReserva').addClass('fa-pulse fa-spinner unprocessed');
    $(".panel-body button").attr("disabled", "disabled");//Deshabilitar botones

    bandPresCargarcalcular = true;
    count = 0;
    //banBarracalcular = true;
    var urlCalculoR = $("#urlCalculoReserva").val();
    var fechaPeriodo = cadenaFechaPeriodo();

    var fields = {
        fecha: fechaPeriodo
    };
    sendValues(fields, doSuccessCalculoReserva, doError, urlCalculoR);
}

function doSuccessCalculoReserva(result) {
    $('#ProReserva').removeClass('fa-pulse fa-spinner unprocessed');
    $('#ProReserva').addClass('processing fa-check-circle');

    $('#txtProReserva').removeClass('unprocessed');
    $('#txtProReserva').addClass('textProcess');
    $(".panel-body button").removeAttr("disabled");

    banAvisocalcular = true;
    mensajeTermino = result.Message;
    aviso("AVISO", result.Message);
}

//Modal de confirmación.
function confirmar(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar();
        $('#sch_modal_confirm').modal('hide');
    });
}

//Función para generar Exel
function exportarCalculo() {

    var url = $("#urlExportarReserva").val();
    window.open(url + "?nombreArchivo=" + nombreArchivo + "&pathFile=" + pathFile, "Generando excel");
}

function imprimirLog() {
    var url = $("#urlExportarLog").val();
    window.open(url, "Generando Log");
}

function doSuccessLog(result) {
    aviso(result.Message);
}

function ReporteSbs() {
    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlReporteSbs").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo);
}

function RptFlujosCartera() {
    var arrayFec = $("#txtMesCal").val().split("/");
    fecha = new Date(arrayFec[0], arrayFec[1], 0);
    var dia = fecha.getDate()
    var mes = (fecha.getMonth() + 1);
    if (mes <= 9) { mes = "0" + mes; }

    var url = $("#urlRptFlujosCartera").val();
    window.open(url + "?FecFlu=" + fecha.getFullYear() + "" + mes);
}

function RptFlujosCarteraAnt() {
    var arrayFec = $("#txtMesCal").val().split("/");
    fecha = new Date(arrayFec[0], arrayFec[1], 0);
    var dia = fecha.getDate()
    var mes = (fecha.getMonth() + 1);
    if (mes <= 9) { mes = "0" + mes; }

    var url = $("#urlRptFlujosCarteraAnt").val();
    window.open(url + "?FecFlu=" + fecha.getFullYear() + "" + mes);
}

function ReporteFlujosPasivos() {
    var fechaPeriodo = cadenaFechaPeriodo();
    var url = $("#urlReporteFlujosPasivos").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo);
}

function TxtFlujosPasivosNuevos() {
    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlArchivoTxt").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo + "&pTipoArchivo=N");
}

function TxtFlujosPasivosAntiguos() {
    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlArchivoTxt").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo + "&pTipoArchivo=A");
}

function TxtFlujosPasivosMixtos() {
    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlArchivoTxt").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo + "&pTipoArchivo=M");
}

function ReporteResumenReservas() {
    var fechaPeriodo = cadenaFechaPeriodo();
    var url = $("#urlReporteResumenReservas").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo);
}

function ReporteResumenAdecuacion() {
    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlReporteResumenAdecuacion").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo);
}

function ReporteContable() {
    var fechaPeriodo = cadenaFechaPeriodo();

    var url = $("#urlReporteContable").val();
    window.open(url + "?pFechaPeriodo=" + fechaPeriodo);
}

//Función para obtener cadena de fecha de periodo del input de la vista.
function cadenaFechaPeriodo() {
    var arrayFec = $("#txtMesCal").val().split("/");
    fecha = new Date(arrayFec[0], arrayFec[1], 0);
    var dia = fecha.getDate()
    var mes = (fecha.getMonth() + 1);
    if (mes <= 9) { mes = "0" + mes; }

    return fecha.getFullYear() + "" + mes + "" + dia
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

var primerslap = false;
var segundoslap = false;

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

function abrirModal() {
    $('#modalcargar').modal({
        drop: 'static',
        keyboard: false,
        show: true,
        backdrop: 'static'
    });
}