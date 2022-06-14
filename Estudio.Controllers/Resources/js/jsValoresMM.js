$(document).ready(function () {
    $('#tblConsultaMonedaMensual tbody').on('click', 'tr', function () {
        var table = $('#tblConsultaMonedaMensual').DataTable();
        if ($(this).hasClass('selected')) {
            table.$('tr.selected').removeClass('selected');
            $(this).removeClass('selected');
            cargarInformacion(0, 0, false);
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
});
(function () {
    localStorage.pagina = 'ValoresMM';
    initTable();
    var url = $("#urlCargarTabla").val();
    var valorMM = $("#CmbxTipoValorMM").val();
    var monedaMM = $("#CmbxTipoMonedaMM").val();
    var fields = {
        vlMoneda: monedaMM,
        cod_tipmon: valorMM
    };
    sendValues(fields, doSuccessConsulta, doError, url);
    $("#CmbxTipoMonedaMM").change(function () {
        var url = $("#urlCargarTabla").val();
        var valorMM = $("#CmbxTipoValorMM").val();
        var monedaMM = $("#CmbxTipoMonedaMM").val();
        var fields = {
            vlMoneda: monedaMM,
            cod_tipmon: valorMM
        };
        sendValues(fields, doSuccessConsulta, doError, url);
    });
})();
function initTable() {
    $("#tblConsultaMonedaMensual").DataTable({
        //"scrollY": "370px",
        "scrollCollapse": true,
        //"paging": true,
        "lengthMenu": [[10, 20, 50, 75, 100], [10, 20, 50, 75, 100]],
        "language": {
            //"sProcessing": "Procesando...",
            //"sLengthMenu": "Mostrar _MENU_ registros",
            //"sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "No se encontró ningún dato disponible en esta tabla",
            "sInfo": "_START_ al _END_ de _TOTAL_ registros",
            //"sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            //"sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "",
            "sUrl": "",
            "sInfoThousands": ",",
            "searchPlaceholder": "Buscar"
            //"sLoadingRecords": "Cargando...",
            /*"oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },*/
            /*"oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }*/
        },
        "bDestroy": true

    });
}
function doError(result) {
    aviso("Aviso", result.Message);
}
function doSuccessConsulta(result) {
    if (result.Message == "Información cargada con éxito") {
        $("#tblConsultaMonedaMensual").DataTable().destroy();
        var $body = $("#tblConsultaMonedaMensual tbody");
        $body.empty();
        var i = 1;
        result.Object.forEach(function (v) {
            //var nFilas = $("#tblConsultaCotizaciones tr").length; <td><span style='display: none;'>20150221</span>21/02/2015</td>
            d = "cargarInformacion('" + v.FechaInicial + "','" + v.Valor + "','true')";
            var tr = $('<tr onclick="' + d + '">');
            var td = $('<td>');
            // $("<td>").html(i).appendTo(tr);
            $("<td>").html(v.FechaInicial).appendTo(tr);
            $("<td>").html(v.Valor).appendTo(tr);
            $body.append(tr)
            i++;
        });
        initTable();
    } else {
        aviso("Aviso", result.Message);
    }
}
function cargarInformacion(a, b, bandera) {
    if (bandera) {
        document.getElementById("txtFechaTMM").value = a;
        document.getElementById("txtFechaTMM").disabled = true;
        document.getElementById("txtValorTMM").value = b;
        document.getElementById("CmbxTipoMonedaMM").disabled = true;
        document.getElementById("txtValorTMM").focus();
    } else {
        document.getElementById("txtFechaTMM").value = "";
        document.getElementById("txtFechaTMM").disabled = false;
        document.getElementById("txtValorTMM").value = "";
        document.getElementById("CmbxTipoMonedaMM").disabled = false;
        document.getElementById("txtValorTMM").focus();
    }
}
function Grabar() {
    var monedaMM = $("#CmbxTipoMonedaMM").val();
    if (monedaMM == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda");
        return;
    }
    var strFec = $("#txtFechaTMM").val();
    if (strFec == "") {
        aviso("Aviso", "Debe ingresar una Fecha para el Valor de Moneda.");
        return;
    }
    if (strFec.length < 7) {
        aviso("Aviso", "El periodo ingresado no es válido..");
        return;
    }
    var fechaActual = new Date();
    var fecha = new Date(strFec);//se formatea la fecha ingresada
    if (fechaActual.getTime() < fecha.getTime()) {
        aviso("Aviso", "La Fecha ingresada es mayor a la fecha actual");
        return;
    }
    if (fecha.getFullYear() < 1900) {
        aviso("Aviso", "La Fecha ingresada es inferior a la Fecha Mínima de Ingreso (1900).");
        return;
    }

    var valor = $("#txtValorTMM").val();
    if (valor == "") {
        aviso("Aviso", "Debe ingresar un Valor para la Moneda seleccionada.");
        return;
    }

    var url = $("#urlConsulta").val();
    var valorMM = $("#CmbxTipoValorMM").val();
    var fields = {
        cod_tipmon: valorMM,
        vlMoneda: monedaMM,
        fec_moneda: strFec
    };
    sendValues(fields, doSuccessConsultaGrabar, doError, url);
}
function doSuccessConsultaGrabar(result) {
    if (result.Message == "No se encontro información") {
        confirmarMonedaMensual("Aviso", "¿ Está seguro que desea Grabar los Datos ?", function () { NuevaMonedaMensual(result, 2); });
        //confirmarMonedaMensual("Aviso", "¿ Está seguro que desea Grabar los Datos ?", result, 2);
        return;
    }
    if (result.Message == "Información cargada con éxito") {
        confirmarMonedaMensual("Aviso", "¿ Está seguro que desea Modificar los Datos ?", function () { NuevaMonedaMensual(result, 3); });
        // confirmarMonedaMensual("Aviso", "¿ Está seguro que desea Modificar los Datos ?", result, 3);
        return;
    }
}
function NuevaMonedaMensual(result, bandera) {

    var url = $("#urlGrabar").val();
    var valorMM = $("#CmbxTipoValorMM").val();
    var monedaMM = $("#CmbxTipoMonedaMM").val();
    var strFec = $("#txtFechaTMM").val();
    var valor = $("#txtValorTMM").val();
    if (bandera == 2) {

        var fields = {
            cod_tipmon: valorMM,
            vlMoneda: monedaMM,
            fec_moneda: strFec,
            valor: valor,
            bandera: true
        };
        sendValues(fields, doSuccessGrabar, doError, url);
        return;
    }
    if (bandera == 3) {
        var fields = {
            cod_tipmon: valorMM,
            vlMoneda: monedaMM,
            fec_moneda: strFec,
            valor: valor,
            bandera: false
        };
        sendValues(fields, doSuccessGrabar, doError, url);

        return;
    }

}
function doSuccessGrabar(result) {
    //recarga la tabla
    var url = $("#urlCargarTabla").val();
    var valorMM = $("#CmbxTipoValorMM").val();
    var monedaMM = $("#CmbxTipoMonedaMM").val();
    var fields = {
        vlMoneda: monedaMM,
        cod_tipmon: valorMM
    };
    sendValues(fields, doSuccessConsulta, doError, url);

    aviso("Aviso", result.Message);
}
function limpiar() {
    var table = $('#tblConsultaMonedaMensual').DataTable();
    table.$('tr.selected').removeClass('selected');
    document.getElementById("txtFechaTMM").value = "";
    document.getElementById("txtFechaTMM").disabled = false;
    document.getElementById("txtValorTMM").value = "";
    document.getElementById("CmbxTipoMonedaMM").disabled = false;
    document.getElementById("CmbxTipoMonedaMM").focus();
}
function Eliminar() {
    var monedaMM = $("#CmbxTipoMonedaMM").val();
    if (monedaMM == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda");
        return;
    }
    var strFec = $("#txtFechaTMM").val();
    if (strFec == "") {
        aviso("Aviso", "Debe ingresar una Fecha para el Valor de Moneda.");
        return;
    }
    if (strFec.length < 7) {
        aviso("Aviso", "El periodo ingresado no es válido..");
        return;
    }

    var url = $("#urlConsulta").val();
    var valorMM = $("#CmbxTipoValorMM").val();
    var fields = {
        cod_tipmon: valorMM,
        vlMoneda: monedaMM,
        fec_moneda: strFec
    };
    sendValues(fields, doSuccessConsultaEliminar, doError, url);
}
function doSuccessConsultaEliminar(result) {
    if (result.Message == "No se encontro información") {
        aviso("Aviso", "La Moneda y Fecha indicada no se encuentran registradas en la BD.");
        return;
    }
    if (result.Message == "Información cargada con éxito") {
        // confirmarMonedaMensual("Aviso", "¿ Está seguro que desea Eliminar los Datos ?", result, 4);
        confirmarMonedaMensual("Aviso", "¿ Está seguro que desea Eliminar los Datos ?", function () { EliminarMonedaMensual(); });
        return;
    }
}
function EliminarMonedaMensual() {
    var url = $("#urlEliminar").val();
    var valorMM = $("#CmbxTipoValorMM").val();
    var monedaMM = $("#CmbxTipoMonedaMM").val();
    var strFec = $("#txtFechaTMM").val();
    var fields = {
        cod_tipmon: valorMM,
        vlMoneda: monedaMM,
        fec_moneda: strFec
    };
    sendValues(fields, doSuccessEliminar, doError, url);

}
function doSuccessEliminar(result) {
    //recarga la tabla
    var url = $("#urlCargarTabla").val();
    var valorMM = $("#CmbxTipoValorMM").val();
    var monedaMM = $("#CmbxTipoMonedaMM").val();
    var fields = {
        vlMoneda: monedaMM,
        cod_tipmon: valorMM
    };
    sendValues(fields, doSuccessConsulta, doError, url);
    limpiar();
    aviso("Aviso", result.Message);
}
function Imprimir() {
    var url = $("#urlReporte").val();
    var valorMM = $("#CmbxTipoValorMM").val();
    var monedaMM = $("#CmbxTipoMonedaMM").val();
    var tex = $("#CmbxTipoMonedaMM option:selected").text();

    if (monedaMM == "") {
        aviso("Aviso", "Debe indicar el Tipo de Moneda a seleccionar para la impresión.");
        document.getElementById("cmbxTipoMoneda").focus();
        return;
    }
    window.open(url + "?vlMoneda=" + monedaMM + "&cod_tipmon=" + valorMM + "&moneda=" + tex);
}
function Salir() {
    var url = $("#urlSalir").val();
    window.location.href = url;
}
function confirmarMonedaMensual(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}
/*
function confirmarMonedaMensual(header, body, result, bandera) {
    $('#msg_modal_header_2').text(header);
    $('#msg_modal_body_2').text(body);
    $('#sch_modal_Anclaje').modal('show');

    $("#btn_modal_aceptar_2").one('click', function () {
        $('#sch_modal_Anclaje').modal('hide');
        if (bandera == 2)//genera el nuevo registro
            NuevaMonedaMensual(result, bandera);
        if (bandera == 3)//modifica el registro
            NuevaMonedaMensual(result, bandera);
        if (bandera == 4)//Elimina el registro
            EliminarMonedaMensual();
    });
}*/



//FUNCIÓN PARA DECIMALES
function validaDecimales(cifra, decimales) {
    var enterolong = cifra.length;
    var entero;
    var pos = 0;
    var numdecimal;
    var falla = "N";

    if ((enterolong == 1) && (IsNumeric(cifra) == false)) {
        cifra = "";
    }

    if ((enterolong > 1)) {
        if (cifra.includes(".") === true) {
            pos = cifra.indexOf(".");
            entero = cifra.substr(0, pos + 1);
            numdecimal = cifra.substr(pos + 1, decimales);

            if ((IsNumeric(entero.replace(".", "")) === false) || (IsNumeric(numdecimal) === false)) {
                falla = "S";
            }
        }
    }

    if (numdecimal == null) {
        if ((IsNumeric(cifra) === false) && (pos == 0)) {
            cifra = "";
        }
    } else {
        if (IsNumeric(numdecimal) === false) {
            cifra = cifra.replace(/(?!-)[^0-9.]/g, "");
            //cifra = entero;
        } else {
            cifra = entero + numdecimal;
        }
    }

    if (falla == "S") {
        cifra = cifra.replace(/(?!-)[^0-9.]/g, "");
    }

    return (cifra);
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
