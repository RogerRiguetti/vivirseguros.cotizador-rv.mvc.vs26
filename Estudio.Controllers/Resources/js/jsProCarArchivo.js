var usuario = $("#urlUsuario").val();
var datos;
var bandCarga = false;
var MfechaCarga = "";
var MnumArchivo = "";
var MnomArchivo = "";
var msjListoImprimir = "";

(function () {
    localStorage.pagina = 'ProCarArchivo';
    var f = new Date();
    var dia = f.getDate()
    var mes = (f.getMonth() + 1);
    if (dia <= 9) { dia = "0" + dia }
    if (mes <= 9) { mes = "0" + mes; }
    var fecha = dia +"/"+ mes +"/" + f.getFullYear() //+ "-" + mes + "-" + dia;
    $("#modalFechaB").val(fecha);
    $('#cmbModalArchivos').attr("disabled", true);
    $("#modalFechaB").click(function () {
        $('#cmbModalArchivos').attr("disabled", true);
        var x = document.getElementById("cmbModalArchivos");
        for (var i = 1; i < x.length; i++) {
            x.remove(i);
        }
        MfechaCarga = "";
        MnumArchivo = "";
        MnomArchivo = "";
        $('#modalFechaCar').val("");
        $('#modalNumArchivo').val("");
        $('#modalNombArchivo').val("");
    });
    $("#cmbModalArchivos").change(function () {
        var valorCombo = $("#cmbModalArchivos").val();
        if (valorCombo != "-1") {
            var temp, ano, mes, dia;
            temp = $('#modalFechaB').val();
            dia = temp.substring(8, 10);
            mes = temp.substring(5, 7);
            ano = temp.substring(0, 4);
            MfechaCarga = ano + "" + mes + "" + dia;
            var datos = valorCombo.split('-');
            MnumArchivo = datos[0];
            MnomArchivo = datos[1];
            $('#modalFechaCar').val(temp);
            $('#modalNumArchivo').val(datos[0]);
            $('#modalNombArchivo').val(datos[1]);

        } else {
            MfechaCarga = "";
            MnumArchivo = "";
            MnomArchivo = "";
            $('#modalFechaCar').val("");
            $('#modalNumArchivo').val("");
            $('#modalNombArchivo').val("");
        }

    });

    $("#modalFechaB").keypress(enter => {
        if (enter.keyCode == 13) {
            $('#cmbModalArchivos').attr("disabled", false);
            var x = document.getElementById("cmbModalArchivos");
            for (var i = 1; i < x.length; i++) {
                x.remove(i);
            }
            var temp = $('#modalFechaB').val();
            dia = temp.substring(0, 2);
            mes = temp.substring(3, 5);
            ano = temp.substring(6, 11);
            MfechaCarga = ano + "" + mes + "" + dia;
            var urlBuscarArchivo = $("#urlBuscarNumerosArchivo").val();
            var fields = {
                fecha: MfechaCarga
            };
            sendValues(fields, doSuccessBuscarArchivo, doErrorBuscarArchivo, urlBuscarArchivo);
        }
    });

   

    //$("#modalFechaB").click(function () {
        
    //        $('#cmbModalArchivos').attr("disabled", false);
    //        var x = document.getElementById("cmbModalArchivos");
    //        for (var i = 1; i < x.length; i++) {
    //            x.remove(i);
    //        }
    //        var temp = $('#modalFechaB').val();
    //        dia = temp.substring(8, 10);
    //        mes = temp.substring(5, 7);
    //        ano = temp.substring(0, 4);
    //        MfechaCarga = ano + "" + mes + "" + dia;
    //        var urlBuscarArchivo = $("#urlBuscarNumerosArchivo").val();
    //        var fields = {
    //            fecha: MfechaCarga
    //        };
    //        sendValues(fields, doSuccessBuscarArchivo, doErrorBuscarArchivo, urlBuscarArchivo);
        
    //});

    $("#btnAcpetarM").click(function () {
        if ($('#modalNumArchivo').val() != "") {
            urlValidar = $("#urlCargarArchivoB").val();
            var fields = {
                numArch: MnumArchivo
            };
            sendValues(fields, doSuccessCargaB, doError, urlValidar);
            $('#modalcargar').modal('hide');
            $("#btnCancelarM").click();
        }
        else { aviso("AVISO", "No se tiene algún archivo seleccionado");  }
    });
    $("#btnCancelarM").click(function () {
        var MfechaCarga = "";
        var MnumArchivo = "";
        var MnomArchivo = "";
        $('#modalFechaCar').val("");
        $('#modalNumArchivo').val("");
        $('#modalNombArchivo').val("");
        $("#modalFechaB").val(fecha);
        $('#cmbModalArchivos').attr("disabled", true);
        var x = document.getElementById("cmbModalArchivos");
        for (var i = 1; i < x.length; i++) {
            x.remove(i);
        }
        $('#cmbArchivos').attr("disabled", true);
    });

    $('#txtFecha').datepicker();
})();


$(document).ready(function () {
    $('input[type="checkbox"]').on('change', function () {
        $(this).siblings('input[type="checkbox"]').not(this).prop('checked', false);
    });
    $("#btnCargar").attr("onclick", "");
    $('#btnCargar').click(function () {
        if (bandCarga == false) {
            aviso("AVISO", "Debes seleccionar algún archivo");
        }
    });

    //$("#modalFechaB").change(function () {
    //    $('#cmbModalArchivos').attr("disabled", false);
    //    var x = document.getElementById("cmbModalArchivos");
    //    for (var i = 1; i < x.length; i++) {
    //        x.remove(i);
    //    }
    //    var temp = $('#modalFechaB').val();
    //    dia = temp.substring(8, 10);
    //    mes = temp.substring(5, 7);
    //    ano = temp.substring(0, 4);
    //    MfechaCarga = ano + "" + mes + "" + dia;
    //    var urlBuscarArchivo = $("#urlBuscarNumerosArchivo").val();
    //    var fields = {
    //        fecha: MfechaCarga
    //    };
    //    if (fields.fecha != "") {
    //        sendValues(fields, doSuccessBuscarArchivo, doErrorBuscarArchivo, urlBuscarArchivo);
    //    }
    //});
    //$("#btnImprimir").attr("onclick", "");
});

function doSuccessCargaB(result) {
    var solicitudes = result.Object.toString();
    var array = (solicitudes).split(",");
    $('#txtEstadoCarga').val("Archivo Cargado");
    $('#txtNumTotalSol').val(array[0].toString());
    $('#txtNumSolAcep').val(array[2].toString());
    $('#txtNumSolGanOtraCia').val(array[1].toString());
    $('#txtNumSolGanAFP').val(array[3].toString());
    $('#txtNumSolReco').val(array[4].toString());
    $('#txtNumSolDesi').val(array[5].toString());
    $('#txtNumSolCadu').val(array[6].toString());
    $('#txtNombreArchivo').val(array[7].toString());
    $('#txtTipoArchivo').val("050 - Resultado de Cotizaciones");
    $('#txtUsuario').val(array[8].toString());
    $('#txtFechaCarga').val(array[9].toString());
    $('#txtHoraCarga').val(array[10].toString());

    $("#btnImprimir").attr("onclick", "imprimir();");
    aviso("AVISO", result.Message);
    msjListoImprimir = result.Message;
    if ($('#txtEstadoCarga').val() != "") {
        $("#btnCargar").attr("onclick", "");
    }
}

function salir() {
    var url = $("#urlSalir").val();
    window.location.href = url;
}

function cambiarFile() {
    var input = document.getElementById('file-input');
    msjListoImprimir = "";
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

                $('#txtCargaArchivo').val(input.value);
                $('#txtNombreArchivo').val(nombre);
                $('#txtUsuario').val(usuario);
                $('#txtTipoArchivo').val("050 - Resultado de Cotizaciones");
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
                $("#btnCargar").attr("onclick", "cargar();");
                bandCarga = true;
                //Limpiamos la parte de la caraga por si es un cambio de xml
                limpiar("carga");
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

function limpiar(esto) {

    switch (esto) {
        case "todo":
            msjListoImprimir = "";
            $('#txtCargaArchivo').val("");
            $('#txtNombreArchivo').val("");
            $('#txtUsuario').val("");
            $('#txtTipoArchivo').val("");
            $('#txtFechaCarga').val("");
            $('#txtHoraCarga').val("");

            $('#txtEstadoCarga').val("");
            $('#txtNumTotalSol').val("");
            $('#txtNumSolAcep').val("");
            $('#txtNumSolReco').val("");
            $('#txtNumSolGanOtraCia').val("");
            $('#txtNumSolDesi').val("");
            $('#txtNumSolGanAFP').val("");
            $('#txtNumSolCadu').val("");
            break;
        case "carga":
            $('#txtEstadoCarga').val("");
            $('#txtNumTotalSol').val("");
            $('#txtNumSolAcep').val("");
            $('#txtNumSolReco').val("");
            $('#txtNumSolGanOtraCia').val("");
            $('#txtNumSolDesi').val("");
            $('#txtNumSolGanAFP').val("");
            $('#txtNumSolCadu').val("");
            msjListoImprimir = "";
            break;
    }
}


//Función para botón Imprimir.
function imprimir() {
    var urlRpt;
    var urlValidar;
    var urlValidarCIA;
    var urlValidarAFP;
    if (msjListoImprimir != "") {
        if ($('#checkSolGanAFP').is(':checked')) {
            urlRpt = $("#urlRptResumen").val();
            window.open(urlRpt);
        }

        if ($('#checkAceptadas').is(':checked')) {
            urlValidar = $("#urlValidaRpts").val();
            var fields = {
                bandera: "Ganadas"
            };
            sendValues(fields, doSuccessValidaAceptadas, doError, urlValidar);
        }

        if ($('#checkGanOtrasCia').is(':checked')) {
            urlValidar = $("#urlValidaRptPerdidasCia").val();
            var fields = {};
            sendValues(fields, doSuccessValidaGanadasOtras, doError, urlValidar);
        }

        if ($('#checkRecoti').is(':checked')) {
            urlValidarCIA = $("#urlValidaRptsCIA").val();
            var fields = {
                bandera: "RECIA"
            };
            sendValues(fields, doSuccessValidaRptsCIA, doError, urlValidarCIA);

            urlValidarAFP = $("#urlValidaRptsAFP").val();
            var fields = {
                bandera: "REAFP"
            };
            sendValues(fields, doSuccessValidaRptsAFP, doError, urlValidarAFP);
        }

        if ($('#checkDesistidas').is(':checked')) {
            urlValidarCIA = $("#urlValidaRptsCIA").val();
            var fields = {
                bandera: "DECIA"
            };
            sendValues(fields, doSuccessValidaRptsCIA, doError, urlValidarCIA);

            urlValidarAFP = $("#urlValidaRptsAFP").val();
            var fields = {
                bandera: "DEAFP"
            };
            sendValues(fields, doSuccessValidaRptsAFP, doError, urlValidarAFP);
        }

        if ($('#checkCaducadas').is(':checked')) {
            urlValidarCIA = $("#urlValidaRptsCIA").val();
            var fields = {
                bandera: "CACIA"
            };
            sendValues(fields, doSuccessValidaRptsCIA, doError, urlValidarCIA);

            urlValidarAFP = $("#urlValidaRptsAFP").val();
            var fields = {
                bandera: "CAAFP"
            };
            sendValues(fields, doSuccessValidaRptsAFP, doError, urlValidarAFP);
        }
    }
    else {
        aviso("AVISO", "¡Debes de cargar un archivo para poder imprimir!");
    }
}


//Valida que se haya seleccionado un archivo a cargar
function cargar() {
    if (bandCarga == true) {
        analizarXML(datos);
    }
    else {
        aviso("Operación cancelada", "Debe Buscar el Archivo a Cargar");
        limpiar("carga");
    }
}

function analizarXML(datos) {
    var urlCargarXML = $("#urlCargarXML").val();
    var fields = {
        __doc: datos,
        archivo: $('#txtCargaArchivo').val(),
        nombre: $('#txtNombreArchivo').val(),
        us: $('#txtUsuario').val(),
        tipo: $('#txtTipoArchivo').val().toString("yyyyMMdd"),
        fecha: $('#txtFechaCarga').val().toString("hhmmss"),
        hora: $('#txtHoraCarga').val()
    };
    $('#modalcargar').modal({
        drop: 'static',
        keyboard: false,
        show: true,
        backdrop: 'static'
    });
    sendValues(fields, doSuccessCarga, doError, urlCargarXML);
}

//////////////////////////////////////////////////////////////FUNCIONES DOSUCCESS//////////////////////////////////////////////////////////////////////////

//Función para validar registros para reporte de solicitudes Aceptadas.
function doSuccessValidaAceptadas(result) {
    if (result.Message == "No se encontro información") {
        modalConfirmacion("Aviso", "No existen solicitudes 'ACEPTADAS' a imprimir.", function () { });
        return;
    }
    if (result.Message == "Información cargada con éxito") {
        var urlRpt = $("#urlRptGanadas").val();
        window.open(urlRpt);
    }
}

//Función para validar registros para reporte de solicitudes ganadas otras cia/afp.
function doSuccessValidaGanadasOtras(result) {
    if (result.Message == "No se encontró información") {
        modalConfirmacion("Aviso", "No existen solicitudes 'PERDIDAS' a imprimir.", function () { });
        return;
    }
    if (result.Message == "Información cargada con éxito") {
        var urlRptCIA = $("#urlRptPerdidasCIA").val();
        var urlRptAFP = $("#urlRptPerdidasAFP").val();
        window.open(urlRptCIA);
        window.open(urlRptAFP);
    }
}

//Función para validar registros para reporte de solicitudes Desistidas (CIA).
function doSuccessValidaRptsCIA(result) {
    //Condiciones para mensajes al no encontrar información.
    if (result.Message == "No se encontró información RE") {
        modalConfirmacion("Aviso", "No existen registros RECOTIZADAS a imprimir (CIA).", function () { });
        return;
    }

    if (result.Message == "No se encontró información DE") {
        modalConfirmacion("Aviso", "No existen registros DESISTIDAS a imprimir (CIA).", function () { });
        return;
    }

    if (result.Message == "No se encontró información CA") {
        modalConfirmacion("Aviso", "No existen registros CADUCADAS a imprimir (CIA).", function () { });
        return;
    }

    //Condiciones para mensajes al encontrar información.
    if (result.Message == "Información cargada con éxito RE") {
        var urlRptCIA = $("#urlRptRecotizadasCIA").val();
        window.open(urlRptCIA);
    }

    if (result.Message == "Información cargada con éxito DE") {
        var urlRptCIA = $("#urlRptDesistidasCIA").val();
        window.open(urlRptCIA);
    }

    if (result.Message == "Información cargada con éxito CA") {
        var urlRptCIA = $("#urlRptCaducadasCIA").val();
        window.open(urlRptCIA);
    }
}

//Función para validar registros para reporte de solicitudes Desistidas (AFP).
function doSuccessValidaRptsAFP(result) {
    //Condiciones para mensajes al no encontrar información.
    if (result.Message == "No se encontró información RE") {
        modalConfirmacion("Aviso", "No existen registros RECOTIZADAS a imprimir (AFP).", function () { });
        return;
    }

    if (result.Message == "No se encontró información DE") {
        modalConfirmacion("Aviso", "No existen registros DESISTIDAS a imprimir (AFP).", function () { });
        return;
    }

    if (result.Message == "No se encontró información CA") {
        modalConfirmacion("Aviso", "No existen registros CADUCADAS a imprimir (AFP).", function () { });
        return;
    }

    //Condiciones para mensajes al encontrar información.
    if (result.Message == "Información cargada con éxito RE") {
        var urlRptAFP = $("#urlRptRecotizadasAFP").val();
        window.open(urlRptAFP);
    }

    if (result.Message == "Información cargada con éxito DE") {
        var urlRptAFP = $("#urlRptDesistidasAFP").val();
        window.open(urlRptAFP);
    }

    if (result.Message == "Información cargada con éxito CA") {
        var urlRptAFP = $("#urlRptCaducadasAFP").val();
        window.open(urlRptAFP);
    }
}


function doSuccessCarga(result) {
    var solicitudes = result.Object.toString();
    var array = (solicitudes).split(",");
    $('#txtEstadoCarga').val("Archivo Cargado");
    $('#txtNumTotalSol').val(array[0].toString());
    $('#txtNumSolAcep').val(array[2].toString());
    $('#txtNumSolGanOtraCia').val(array[1].toString());
    $('#txtNumSolGanAFP').val(array[3].toString());
    $('#txtNumSolReco').val(array[4].toString());
    $('#txtNumSolDesi').val(array[5].toString());
    $('#txtNumSolCadu').val(array[6].toString());
    $("#btnImprimir").attr("onclick", "imprimir();");
    aviso("AVISO", result.Message);
    msjListoImprimir = result.Message;
    if ($('#txtEstadoCarga').val() != "") {
        $("#btnCargar").attr("onclick", "");
    }
    $('#modalcargar').modal('hide');
}
function doError(result) {
    $('#modalcargar').modal('hide');
    aviso("ERROR", result.Message);
    limpiar("todo");
}



/////////////////////////////////////////MODALES/////////////////////////////////////////////////////////
//Función para modal de tipo alerta de confirmación.
function modalConfirmacion(header, body, fnaceptar) {
    $('#modal_aviso_confirm').text(header);
    $('#msg_modal_aviso_confirm').text(body);
    $('#modal_aviso_dos').modal('show');

    $("#btn_modal_aviso_confirm").one('click', function () {
        fnaceptar(); $('#modal_aviso_dos').modal('hide');
    });
}

function doSuccessBuscarArchivo(result) {
    var array = result.Object;
    if (array.length > 0 && array[0].toString() != "-1") {
        for (var i = 0; i < array.length; i++) {
            var trOpcion = document.createElement('option');
            trOpcion.setAttribute("value", (array[i]));
            trOpcion.text = (array[i].toString());
            document.getElementById('cmbModalArchivos').appendChild(trOpcion);
        }
    } else {
        $('#cmbModalArchivos').attr("disabled", true);
        $("#btnCancelarM").click();
        aviso("AVISO", "No se encontraron registros con esa fecha");
    }
}

function doErrorBuscarArchivo(result) {
    aviso("ERROR", result.Message);
    limpiar("todo");
    $("#btnCancelarM").click();
}


