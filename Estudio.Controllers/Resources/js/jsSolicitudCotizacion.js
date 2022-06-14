var usuario = $("#urlUsuario").val();
var count = 100;
var bandCarga = false;
var bandPresCargar = false;
var banBarra = false;
var solicitudesT, solicitudesE, solicitudesC;
var mensajeTermino = "";
var mT = "El Proceso de Carga Terminó Correctamente";
var msj2 = "";
var datos;
var paso = 0;
var tamano;
var avance;
var numArchivo;
var bandErrores = false;

(function () {
    localStorage.pagina = 'SolicitudCotizacion';
})();

$(document).ready(function () {
    $(document).bind("ajaxSend", function () {
        if (bandPresCargar == true && banBarra == true) {
            mensajeTermino = "";
            count = 0;
            banBarra = false;
            paso = 1;
            Barra();
        }
    }).bind("ajaxComplete", function () {
        if (bandPresCargar == true && mensajeTermino == mT) {
            msj2 = mensajeTermino;
            mensajeTermino = "";
            banBarra = false;
            paso = 2;
            Barra();
        }
    }).bind("ajaxError", function () {
        if (bandPresCargar == true && banBarra == true) {
            mensajeTermino = "";
            banBarra = false;
            limpiar("solicitudes");
        }
    });

});

//function siguiente() {
//    if ($('#txtSolicitudesT').val() == "") {
//        aviso("Aviso", "¡No puedes pasar a la siguiente pantalla si no hay un archivo cargado!");
//        return;
//    }else{
//        limpiar("todo");
//        var url = $("#urlSiguiente").val();
//        window.location.href = url + "?numArchivo=" + numArchivo;
//    }
//}

function siguiente() {
    if ($('#txtSolicitudesT').val() == "") {
        aviso("Aviso", "¡No puedes pasar a la siguiente pantalla si no hay un archivo cargado!");
        return;
    } else {
        var urlSiguiente = $("#urlDatosSiguiente").val();
        var fields = {
            numArchS: numArchivo
        };
        $('#modalcargar').modal({
            drop: 'static',
            keyboard: false,
            show: true,
            backdrop: 'static'
        });
        sendValues(fields, doSuccessSiguiente, doError, urlSiguiente);
    }
}

function doSuccessSiguiente() {

    limpiar("todo");
    var url = $("#urlSiguiente").val();
    window.location.href = url + "?numArchivo=" + numArchivo;
    $('#modalcargar').modal('hide');
}
function imprimirErrores() {
    if (bandErrores == true) {

        if ($('#txtSolicitudesE').val() == "0") {
            aviso("Aviso", "La carga no tiene errores");
            msj2 = "";
            return;
        } else {
            var url = $("#urlRptErrores").val();
            window.open(url);
        }
    } else {
        aviso("AVISO", "Necesitas cargar un archivo");
    }
}

//Función para generar reporte de resumen de carga.
function imprimirResumen() {
    if ($('#txtArchivoCargado').val() == "") {
        aviso("Aviso", "No se ha buscado un archivo para cargar.");
        return;
    }

    if ($('#txtSolicitudesT').val() == "") {
        aviso("Aviso", "No se completó la carga del archivo.");
        return;
    }

    var url = $("#urlRptResumen").val();
    window.open(url);

}

function Barra() {
    //$('.progress').prop("style", "display:block;");
    $('.bar').prop("style", "display:block;");
    $("#cargaArchivo").modal();
    var myVar = setInterval(function () {
        myTimer();
    }, 1);
    function myTimer() {
        if (paso == 1) {
            if (count < 90 && bandPresCargar == true) {
                $('.progress-bar').css('width', count + "%");
                count += avance;
                document.getElementById("porcentaje").innerHTML = Math.round(count) + "% Completado";
                $('#txtSolicitudesC').val('');
                $('#txtSolicitudesT').val('');
                $('#txtSolicitudesE').val('');
                mensajeTermino = "";
            }
        }
        if (paso == 2) {
            if (count < 100 && bandPresCargar == true) {
                $('.progress-bar').css('width', count + "%");
                count += 0.05;
                document.getElementById("porcentaje").innerHTML = Math.round(count) + "% Completado";
                $('#txtSolicitudesC').val('');
                $('#txtSolicitudesT').val('');
                $('#txtSolicitudesE').val('');
                mensajeTermino = "";
            }
            else if (count > 100) {
                count = 0;
                paso = 0;
                bandPresCargar = false;
                $('#txtSolicitudesC').val(solicitudesC);
                $('#txtSolicitudesT').val(solicitudesT);
                $('#txtSolicitudesE').val(solicitudesE);
                $("#cargaArchivo").modal("hide");
                aviso("AVISO", mT);
            }
        }
    }
}

function cargarsolicitudes() {
    if (bandCarga == true) {
        bandPresCargar = true;
        count = 0;
        banBarra = true;
        analizarXML(datos);
    }
    else {
        aviso("Aviso", "Tienes que buscar un archivo .xml para poder cargar");
        limpiar("solicitudes");
        bandCarga = false;
        bandPresCargar = false;
    }
}

function cambiarFile() {
    var input = document.getElementById('file-input');
    if (input.files && input.files[0]) {
        var nombre = input.files[0].name;
        tamano = input.files[0].size;
        if (tamano > 0 && tamano < 40000) { avance = 0.0008 }
        else if (tamano > 40000 && tamano < 70000) { avance = 0.0002 }
        else { avance = 0.00007 }
        if (esXML(nombre) == true) {
            // input.addEventListener('change', function (e) {
            console.log("-----------------------------------------------------------------");
            console.log(input.files);
            const reader = new FileReader();
            reader.onload = function () {
                // console.log(reader.result);
                //json = { datos: reader.result };

                $('#txtArchivoCargado').val(input.value);
                $('#txtNombreArchivo').val(nombre);
                $('#txtUsuario').val(usuario);
                $('#txtTipoArchivo').val("030 - Solictud de Cotizaciones");
                var f = new Date();
                var dia = f.getDate()
                var mes = (f.getMonth() + 1);
                var hora = f.getHours();
                var minuto = f.getMinutes();
                var segundo = f.getSeconds();
                if (dia <= 9) { dia = "0" + dia }
                if (mes <= 9) { mes = "0" + mes; }
                if (hora <= 9) { hora = "0" + hora; }
                if (minuto <= 9) { minuto = "0" + minuto; }
                if (segundo <= 9) { segundo = "0" + segundo; }
                $('#txtFechaCarga').val(dia + "/" + mes + "/" + f.getFullYear());
                $('#txtHoraCarga').val(hora + ":" + minuto + ":" + segundo);
                bandCarga = true;
                //Limpiamos la parte de las solicitudes por si es un cambio de xml
                limpiar("solicitudes");
                // console.log("File Seleccionado : ", input.files[0]);  <--Para checar todo lo que trae el documento
                datos = reader.result;
            }
            reader.readAsText(input.files[0]);
            //}, false);
        }
        else {
            aviso("ERROR", "El archivo no es extensión .xml");
            limpiar("todo");
        }
    } else {
        limpiar("todo");
    }
}

function esXML(doc) {
    var array = doc.split(".");
    var res = false;
    if (array[array.length - 1] == "xml" || array[array.length - 1] == "XML") {
        res = true;
    }
    return res;
}

function analizarXML(datos) {

    var urlCargarXML = $("#urlCargarXML").val();
    var fields = {
        __doc: datos,
        archivo: $('#txtArchivoCargado').val(),
        nombre: $('#txtNombreArchivo').val(),
        us: $('#txtUsuario').val(),
        tipo: $('#txtTipoArchivo').val().toString("yyyyMMdd"),
        fecha: $('#txtFechaCarga').val().toString("hhmmss"),
        hora: $('#txtHoraCarga').val()
    };
    sendValues(fields, doSuccessAnalisis, doError, urlCargarXML);
}
function doSuccessAnalisis(result) {
    var solicitudes = result.Object.toString();
    var array = (solicitudes).split(",");
    solicitudesT = array[0].toString();
    solicitudesC = array[1].toString();
    solicitudesE = array[2].toString();
    numArchivo = array[3].toString();
    mensajeTermino = result.Message.toString();
    if ($('#txtSolicitudesT').val() != "") {
        $("#btnCargaSol").attr("onclick", "");
    }
    bandErrores = true;
}
function doError(result) {
    $('#cargaArchivo').modal('hide');
    $('#cargaArchivo').modal({
        drop: 'static',
        keyboard: false,
        show: false,
        backdrop: 'static'
    });
    if (result.Message == "Archivo no corresponde a Carga de Solicitudes") {
        mensajeTermino = result.Message;
        aviso("ERROR", mensajeTermino);
        limpiar("solicitudes");
        bandErrores = false;
    }
    else if (result.Message == "Referencia a objeto no establecida como instancia de un objeto." || result.Message ==  "Object reference not set to an instance of an object.") {
        aviso("ERROR", "Archivo no corresponde a Carga de Solicitudes");
        limpiar("solicitudes");
    }
    else {
        aviso("ERROR", result.Message);
        limpiar("solicitudes");
        bandErrores = true;
    }
}
function limpiar(esto) {

    switch (esto) {
        case "todo":
            bandErrores = false;
            count = 0;
            bandCarga = false;
            bandPresCargar = false;
            $('#txtArchivoCargado').val("");
            $('#txtArchivoCargado').val("");
            $('#txtNombreArchivo').val("");
            $('#txtUsuario').val("");
            $('#txtTipoArchivo').val("");
            $('#txtFechaCarga').val("");
            $('#txtHoraCarga').val("");
            //$('.progress').prop("style", "display:none;");
            $('#txtSolicitudesC').val('');
            $('#txtSolicitudesT').val('');
            $('#txtSolicitudesE').val('');
            break;
        case "solicitudes":
            count = 0;
            bandErrores = false;
            //$('.progress').prop("style", "display:none;");
            $('#txtSolicitudesC').val('');
            $('#txtSolicitudesT').val('');
            $('#txtSolicitudesE').val('');
            break;
    }
}
