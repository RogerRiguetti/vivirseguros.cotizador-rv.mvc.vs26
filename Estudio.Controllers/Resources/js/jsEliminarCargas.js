var btnClickB = "";
var caso = "CARGA";
var Tipo_Archivo = "030";
var MfechaCarga = "";
var MnumArchivo = "";
var MnomArchivo = "";

(function () {
    localStorage.pagina = 'EliminarCargas';

    var f = new Date();
    var dia = f.getDate()
    var mes = (f.getMonth() + 1);
    if (dia <= 9) { dia = "0" + dia }
    if (mes <= 9) { mes = "0" + mes; }
    var fecha = f.getFullYear() + "-" + mes + "-" + dia;
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
            $('#modalFechaCar').val(dia+"/"+mes+"/"+ano);
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
            dia = temp.substring(8, 10);
            mes = temp.substring(5, 7);
            ano = temp.substring(0, 4);
            MfechaCarga = ano + "" + mes + "" + dia;
            var casoTMP = "";
            if (btnClickB == "carga" && caso == "ENVIOMELER") {
                casoTMP = "CARGA";
            }
            else {
                casoTMP = caso;
            }
            var urlBuscarArchivo = $("#urlBuscarNumerosArchivo").val();
            var fields = {
                fecha: MfechaCarga,
                caso: casoTMP
            };
            $('#modalcargar').modal({
                drop: 'static',
                keyboard: false,
                show: true,
                backdrop: 'static'
            });
            sendValues(fields
                , result => {
                    $('#modalcargar').modal('hide');
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
                , result => {
                    $('#modalcargar').modal('hide');
                    aviso("ERROR", result.Message);
                    limpiar();
                    $("#btnCancelarM").click();
                }
                , urlBuscarArchivo);
        }
     });
     $("#btnAcpetarM").click(function () {
         if ($('#modalNumArchivo').val() != "") {
             urlValidar = $("#urlCargarArchivoB").val();
             busqueda(btnClickB, "modal");
             $("#btnCancelarM").click();
         }
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
         limpiar();
     });

     $(".close").click(function () {
         limpiar();
     });
})();
$(document).ready(function () {
     $('#checkCarga').click(function(){
         caso = "CARGA";
         Tipo_Archivo = "030";
         $('#tituloBusqueda').text("Selección de archivo de entrada");
         $("#divCarga").attr("style", "display:none;");
         $('#txtN1').text("Número total de solicitudes calculadas");
         $('#txtN2').text("Monto total primas de solicitud calculadas");
         limpiar();
    });
    /*$('#checkIntermediarios').checked(function () {
        caso = "INTERMEDIARIOS";
    });*/
    $('#checkEnvioMeler').click(function () {
        caso = "ENVIOMELER";
        Tipo_Archivo = "040";
        $('#tituloBusqueda').text("Selección de archivo de entrada");
        $("#divCarga").attr("style", "display:block;");
        $('#txtN1').text("Número total de solicitudes cotizadas");
        $('#txtN2').text("Monto total primas de solicitud cotizadas");
        limpiar();
    });
    $('#checkResultados').click(function () {
        caso = "RESULTADOS";
        Tipo_Archivo = "050";
        $("#divCarga").attr("style", "display:none;");
        $('#tituloBusqueda').text("Selección de archivo de resultados (Aceptaciones)");
        $('#txtN1').text("Número total de solicitudes aceptadas");
        $('#txtN2').text("Número de solicitudes ganadas otras cías.");
        limpiar();
    });
});
function busqueda(b, x) {
    btnClickB = b;
    var numArch = "";
    var numArchS = "";
    var nomArch = "";
    var nomArchS = "";
    if(x == "modal"){
        numArch = $('#modalNumArchivo').val();
        nomArch = $('#modalNombArchivo').val();
    }
    else{
        numArch = $("#txtNumArchC").val();
        numArchS = $("#txtNumArchS").val();
        nomArch = $("#txtNomArchC").val();
        nomArchS = $("#txtNomArchS").val();
    }
    limpiar();
    $("#txtNumArchC").attr("disabled", true);
    $("#txtNumArchS").attr("disabled", true);
    $("#txtNomArchC").attr("disabled", true);
    $("#txtNomArchS").attr("disabled", true);
    var casoTMP = "";
    var tipArchTMP = "";
    if (btnClickB == "carga" && caso == "ENVIOMELER") {
        casoTMP = "CARGA";
        tipArchTMP = "030";
        //btnClickB = "ENVIOMELER";
    }
    else {
        casoTMP = caso;
        tipArchTMP = Tipo_Archivo;
    }
    if (numArch != "" || nomArch != "" || numArchS != "" || nomArchS != "") {
        var url = $("#urlBusqueda").val();
        var fields = {
            caso: casoTMP,
            numArch: numArch != "" ? numArch : numArchS,
            nomArch: nomArch != "" ? nomArch : nomArchS,
            tipoArchivo: tipArchTMP
        };
        $('#modalcargar').modal({
            drop: 'static',
            keyboard: false,
            show: true,
            backdrop: 'static'
        });
        sendValues(fields, doSuccessBusqueda, doError, url);
    } else {
        $('#ModalBusArchivo').modal('show');
    }
}
function doSuccessBusqueda(result) {
    $('#modalcargar').modal('hide');
    $("#txtNumArchC").attr("disabled", true);
    $("#txtNumArchS").attr("disabled", true);
    $("#txtNomArchC").attr("disabled", true);
    $("#txtNomArchS").attr("disabled", true);

    var array = result.Object;
    var archivo = array[0].split('#');
    $('#txtNumArchC').val(archivo[0]);
    $('#txtNomArchC').val(archivo[1]);

    $('#txtNumArchS').val(archivo[2] == "0" ? "" : archivo[2]);
    $('#txtNomArchS').val(archivo[3]);

    if (array[1] == "030" || array[1] == "050") {
        $('#txtNomArchB').val(archivo[1]);
    }
    else {
        $('#txtNomArchB').val(archivo[3]);
    }

    $('#txtTipoArchB').val(array[1]);
    $('#txtUsuarioB').val(array[2]);
    $('#txtFecCargaB').val(array[3]);
    $('#txtHorCargaB').val(array[4]);
    $('#txtNumero1B').val(array[5]);
    $('#txtNumero2B').val(array[6]);
    $('#txtNumero3B').val(array[7]);
    aviso("AVISO",result.Message);
}
function doError(result) {
    $('#modalcargar').modal('hide');
    limpiar();
    aviso("ERROR", result.Message);
}
function limpiar() {
    $("#txtNumArchC").attr("disabled", false);
    $("#txtNumArchS").attr("disabled", false);
    $("#txtNomArchC").attr("disabled", false);
    $("#txtNomArchS").attr("disabled", false);
    $('#txtNumArchC').val("");
    $('#txtNomArchC').val("");
    $('#txtNumArchS').val("");
    $('#txtNomArchS').val("");
    $('#txtNomArchB').val("");
    $('#txtTipoArchB').val("");
    $('#txtUsuarioB').val("");
    $('#txtFecCargaB').val("");
    $('#txtHorCargaB').val("");
    $('#txtNumero1B').val("");
    $('#txtNumero2B').val("");
    $('#txtNumero3B').val("");
}
function salir() {
    var url = $("#urlSalir").val();
    window.location.href = url;
}
function eliminar(){
    if ($('#txtNumero3B').val() == "") {
        aviso("AVISO","¡Debes buscar un archivo primero!");
    } else {

        var numArch = $("#txtNumArchC").val();
        var numArchS = $("#txtNumArchS").val();
        if (numArchS == "" && caso == "ENVIOMELER") {
            aviso("AVISO", "Este archivo solo lo puedes eliminar desde la parte de 'CARGA'");
            return;
         }

        confirmarE("Aviso", "¿Está seguro que desea eliminar el archivo seleccionado?",
             function () {
                var url = $("#urlEliminar").val();
                var fields = {
                    caso: caso,
                    numArch: numArch,
                    numArchS: numArchS
                };
                $('#modalcargar').modal({
                    drop: 'static',
                    keyboard: false,
                    show: true,
                    backdrop: 'static'
                });
                sendValues(fields, doSuccessEliminar, doError, url);
            });
    }
}
function doSuccessEliminar(result) {
    $('#modalcargar').modal('hide');
    limpiar();
    aviso("AVISO", result.Message);
}
function cancelar() {
    if ($('#txtNumero3B').val() == "") {
        aviso("AVISO", "¡Debes buscar un archivo primero!");
    } else {
        limpiar();
    }
}
function validaNumericos(event) {
    if (event.charCode >= 48 && event.charCode <= 57) {
        return true;
    }
    return false;
}
function confirmarE(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {

        fnaceptar(); $('#sch_modal_confirm').modal('hide');
        $('#modalcargar').modal({
            drop: 'static',
            keyboard: false,
            show: true,
            backdrop: 'static'
        });


    });
}