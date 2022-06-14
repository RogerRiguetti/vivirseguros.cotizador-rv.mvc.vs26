var mensaje = $("#urlMensaje").val();
(function () {
    localStorage.pagina = 'ManCTRes';
    initTable();
    if (mensaje != null && mensaje != "") {
        $('#modalcargar').modal("hide");
        aviso("AVISO", mensaje);
        mensaje = "";
    }

})();

function initTable() {
    $("#tblConsultaMantenimientoCurvaTasas").DataTable({
        "scrollY": "370px",
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
            "searchPlaceholder": "Buscar",
            "sUrl": "",
            "sInfoThousands": ",",
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

function cambiarFile() {
    var input = document.getElementById('file-input');
    if (input.files && input.files[0]) {
        var nombre = input.files[0].name;
        if (esXML(nombre) == true) {
            // input.addEventListener('change', function (e) {
            const reader = new FileReader();
            reader.onload = function () {
                // console.log(reader.result);
                //json = { datos: reader.result };
                $('#txtArchivoCargado').val(input.value);
                $('#file-submit').prop("disabled", false);
                datos = reader.result;
            }
            reader.readAsText(input.files[0]);
            //}, false);
        }
        else {
            aviso("ERROR", "El archivo no es extencion .xlxs o .xls");
            $('#txtArchivoCargado').val("");
            $('#modalcargar').modal("hide");
        }
    } else {
        $('#txtArchivoCargado').val("");
    }
}

function esXML(doc) {
    var array = doc.split(".");
    var res = false;
    if (array[array.length - 1] == "xlsx" || array[array.length - 1] == "XLSX" || array[array.length - 1] == "XLS" || array[array.length - 1] == "xls") {
        res = true;
    }
    return res;
}

function BuscarPorFecha() {
    var urlCargar = $("#urlCargar").val();
    var fecha = $("#txtCarga").val();
    fecha = fecha + "/01";
    if (fecha != "" && fecha.length == 10) {
        var fields = {
            fecha: fecha
        };
        sendValues(fields, doSuccessCarga, doError, urlCargar);
        $('#modalcargar').modal({
            drop: 'static',
            keyboard: false,
            show: true,
            backdrop: 'static'
        });
    } else {
        aviso("Operación cancelada", "Debe Ingresar una Fecha Para Buscar.\n Asegurese que la Feche Sea Correcta.");
    }
}

function doSuccessCarga(result) {
    CargarTabla(result.Object);
    $('#modalcargar').modal('hide');
}
function doError(result) {
    aviso("ERROR", result.Message);
    $('#modalcargar').modal("hide");
}

function CargarTabla(Datos) {
    var nombCol = ["strNumMes", "strSolesIx", "strSolesAj", "strDolaresAj"];
    var cont = 0;
    var bandMes = true;
    $("#tblConsultaMantenimientoCurvaTasas").DataTable().destroy();
    var $body = $("#tblConsultaMantenimientoCurvaTasas tbody");
    $body.empty();

    for (var i = 0; i < Datos.length; i++) {

        if (bandMes == true) {
            var trDatos = document.createElement('tr');
            trDatos.setAttribute("class", "bordeBajo");
            var thDato1 = document.createElement('th');
            thDato1.setAttribute("class", "panel-heading " + nombCol[0]);
            thDato1.append(Datos[i].Num_Mes);
            bandMes = false;
        }

        if (Datos[i].COD_SCOMP == "S/.") {
            var thDato2 = document.createElement('th');
            thDato2.setAttribute("class", "panel-heading " + nombCol[1]);
            thDato2.append(Datos[i].Mto_valor);
            cont++;
        }
        if (Datos[i].COD_SCOMP == "S/.Aj.") {
            var thDato3 = document.createElement('th');
            thDato3.setAttribute("class", "panel-heading " + nombCol[2]);
            thDato3.append(Datos[i].Mto_valor);
            cont++;
        }
        if (Datos[i].COD_SCOMP == "US$Aj.") {
            var thDato4 = document.createElement('th');
            thDato4.setAttribute("class", "panel-heading " + nombCol[3]);
            thDato4.append(Datos[i].Mto_valor);
            cont++;
        }

        /////
        if (cont == 3) {
            cont = 0;
            bandMes = true;
            trDatos.appendChild(thDato1);
            trDatos.appendChild(thDato2);
            trDatos.appendChild(thDato3);
            trDatos.appendChild(thDato4);
            //document.getElementById('RConsultaCurvaTasas').appendChild(trDatos);

            $body.append(trDatos)
        }

    }
    initTable();
}

function abrirModal() {
    $('#modalcargar').modal({
        drop: 'static',
        keyboard: false,
        show: true,
        backdrop: 'static'
    });
}