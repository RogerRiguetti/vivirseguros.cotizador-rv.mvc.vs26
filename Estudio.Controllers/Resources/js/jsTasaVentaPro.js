var Fecha;

(function () {
    localStorage.pagina = 'TVPro';
    initTable();
    /*var url = $("#urlCargarTabla").val();
    var valorMM = $("#cmbxTipoMoneda").val();
    var monedaMM = $("#cmbxPresta").val();
    var fields = {
        vlMoneda: monedaMM,
        cod_Pres: valorMM
    };
    sendValues(fields, doSuccessConsulta, doError, url);*/
    $("#cmbxTipoMoneda").change(function () {
        var url = $("#urlCargarTabla").val();
        var valorMM = $("#cmbxTipoMoneda").val().split('#');
        var monedaMM = $("#cmbxPresta").val();
        var fields = {
            vlMoneda: valorMM[0],
            tipReajuste: valorMM[1],
            cod_Pres: monedaMM
        };
        sendValues(fields, doSuccessConsulta, doError, url);
    });

    $("#cmbxPresta").change(function () {
        var TipMoneda = $("#cmbxTipoMoneda").val();

        if (TipMoneda == "") {
            aviso("Aviso", "Debe Seleccionar un Tipo de Moneda.");
            return;
        }

        var url = $("#urlCargarTabla").val();
        var valorMM = $("#cmbxTipoMoneda").val().split('#');
        var monedaMM = $("#cmbxPresta").val();
        var fields = {
            vlMoneda: valorMM[0],
            tipReajuste: valorMM[1],
            cod_Pres: monedaMM
        };
        sendValues(fields, doSuccessConsulta, doError, url);
    });

    $("#btnSalir").click(function () {
        var url = $("#urlSalir").val();
        window.location.href = url;
    });
})();

function initTable() {
    $("#tblConsultaTasaVentaPro").DataTable({
        "order": [[0, "desc"]],
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
    $('.col-sm-6').addClass('col-lg-12');
}

function doError(result) {
    aviso("Aviso", result.Message);
}
function doSuccessConsulta(result) {
    if (result.Message == "Información cargada con éxito") {
        $("#tblConsultaTasaVentaPro").DataTable().destroy();
        var $body = $("#tblConsultaTasaVentaPro tbody");
        $body.empty();
        var i = 1;
        result.Object.forEach(function (v) {
            //var nFilas = $("#tblConsultaCotizaciones tr").length; <td><span style='display: none;'>20150221</span>21/02/2015</td>
            d = "cargarInformacion('" + v.Mes + "','" + v.Prom + "')";
            var tr = $('<tr onclick="' + d + '">');
            var td = $('<td>');
            // $("<td>").html(i).appendTo(tr);
            $("<td>").html(v.Mes).appendTo(tr);
            $("<td>").html(v.Prom).appendTo(tr);
            $body.append(tr)
            i++;
        });
        initTable();
    } else {
        aviso("Aviso", result.Message);
    }
}

function cargarInformacion(a, b) {
    Fecha = a;
    aAux = a;
    document.getElementById("txtMes").value = Fecha;
    document.getElementById("txtTVPro").value = b;
    document.getElementById("txtTVPro").focus();
}

function Limpiar() {
    $("#tblConsultaTasaVentaPro").DataTable().destroy();
    var $body = $("#tblConsultaTasaVentaPro tbody");
    $body.empty();
    document.getElementById("txtMes").value = "";
    document.getElementById("txtTVPro").value = "";

    initTable();
}

function Grabar() {
    var TipMoneda = $("#cmbxTipoMoneda").val();

    if (TipMoneda == "") {
        aviso("Aviso", "Debe Seleccionar un Tipo de Moneda.");
        return;
    }
    
    if ($("#txtMes").val() == "") {
        aviso("Aviso", "Debe ingresar una Fecha.");
        return;
    }

    if ($("#txtTVPro").val() == "") {
        aviso("Aviso", "Debe ingresar una Tasa de Venta.");
        return;
    }

    var valfec = $("#txtMes").val().length;

    if ($("#txtMes").val().length < 7 ) {
        aviso("Aviso", "Debe ingresar una Fecha válida.");
        return;
    }


    var url = $("#urlBuscaTasa").val();
    var valorMM = $("#cmbxTipoMoneda").val().split("#");
    var monedaMM = $("#cmbxPresta").val();
    var valFecha = $("#txtMes").val().split("/");
    var fields = {
        vlMoneda: valorMM[0],
        tipReajuste: valorMM[1],
        cod_Pres: monedaMM,
        fecha: valFecha[0] + valFecha[1] + "01"
    };
    sendValues(fields, doSuccessGuardar, doError, url);
}

function doSuccessGuardar(result) {
    if (result.Message == "No se encontro información") {
        confirmarTasaVta("Aviso", "La Tasa no se encuentra registrada \n ¿Desea Ingresar esta Nueva Tasa de Venta?", function () { insertarTasa(); });
    }
    if (result.Message == "Información encontrada con éxito") {
        confirmarTasaVta("Aviso", "¿Desea modificar la Tasa de Venta?", function () { modificarTasa(); });
    }

    //ActualizarTabla();
}

//Función para Modificar Tasa existente.
function modificarTasa() {
    var url = $("#urlGuardar").val();
    var valorMM = $("#cmbxTipoMoneda").val().split("#");
    var monedaMM = $("#cmbxPresta").val();
    var prom = $("#txtTVPro").val();
    var valFecha = $("#txtMes").val().split("/");
    //var aux = Fecha;
    //var split = aux.split("/");
    //var fecha = split[2] + "" + split[1] + "" + split[0];
    var fields = {
        vlMoneda: valorMM[0],
        tipReajuste: valorMM[1],
        cod_Pres: monedaMM,
        prom: prom,
        fecha: valFecha[0] + valFecha[1] + "01"
    };
    sendValues(fields, doSuccessModifica, doError, url);
}

//Función doSuccess de Modificación de Tasas.
function doSuccessModifica(result) {
    aviso("Aviso", result.Message);
    ActualizarTabla();
}

function insertarTasa() {
    var url = $("#urlNuevaTasa").val();
    var valorMM = $("#cmbxTipoMoneda").val().split("#");
    var monedaMM = $("#cmbxPresta").val();
    var prom = $("#txtTVPro").val();
    var valFecha = $("#txtMes").val().split("/");
    //var aux = Fecha;
    //var split = aux.split("/");
    //var fecha = split[2] + "" + split[1] + "" + split[0];
    var fields = {
        vlMoneda: valorMM[0],
        tipReajuste: valorMM[1],
        cod_Pres: monedaMM,
        fecha: valFecha[0] + valFecha[1] + "01",
        prom: prom
    };
    sendValues(fields, doSuccessModifica, doError, url);
}

//Función doSuccess para Inserción de Tasas.
function doSuccessModifica(result) {
    aviso("Aviso", result.Message);
    ActualizarTabla();
}

function ActualizarTabla() {
    var url = $("#urlCargarTabla").val();
    var valorMM = $("#cmbxTipoMoneda").val().split('#');
    var monedaMM = $("#cmbxPresta").val();
    var fields = {
        vlMoneda: valorMM[0],
        tipReajuste: valorMM[1],
        cod_Pres: monedaMM
    };
    sendValues(fields, doSuccessConsulta, doError, url);
}

//Modal de confirmación para nuevos registros.
function confirmarTasaVta(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
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