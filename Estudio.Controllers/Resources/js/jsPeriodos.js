var FechaPeriodo = '';// variable global para fecha del periodo a reabrir

(function () {
    localStorage.pagina = 'Periodos';
    initTable();
    //Funcion para consultar periodos
    CargaTabla();
    $("#btnguardar").click(function () {
        GuardarNuevoPeriodo();
    });
})();

// Cargar tabla con la informacion de los periodos
function CargaTabla() {
    var urlPeriodos = $("#urlPeriodos").val();
    var fields = {
    };
    sendValues(fields, doSuccessPeriodos, doError, urlPeriodos);
}

$(document).ready(function () {
    

    $("#tblConsultaPeriodos tbody").delegate("tr", "click", function() {

        var table = $('#tblConsultaPeriodos').DataTable();

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
});

//dosuccess ultimo periodo abierto
function doSuccessPeriodos(result) {
    CargarTabla(result.Object);
    var mes = result.Object;
    $('#tblConsultaPeriodos');
}

//Validaciones errores
function doError(result) {
    aviso("ERROR", result.Message);
}

//PRUEBA
function CargarTabla(Datos) {
    var nombCol = ["fecha", "estado"];
    var cont = 0;
    var mes = true;
    $("#tblConsultaPeriodos").DataTable().destroy();
    var $body = $("#tblConsultaPeriodos tbody");
    $body.empty();

    for (var i = 0; i < Datos.length; i++) {

        var trDatos = document.createElement('tr');
        trDatos.setAttribute("class", "bordeBajo");
        trDatos.setAttribute("style", "cursor:pointer;");
        trDatos.setAttribute("id", Datos[i].strFec_Calculo);
        trDatos.setAttribute("onClick", "ObtenerReg(" + Datos[i].strFec_Calculo + ")");
           
        var thDato1 = document.createElement('td');
        thDato1.setAttribute("class", "sorting_1 " + nombCol[0]);
        thDato1.append(Datos[i].Fec_Calculo);

        var thDato2 = document.createElement('td');
        thDato2.setAttribute("class", "sorting_1 " + nombCol[1]);
        thDato2.append(Datos[i].Cod_EstPeriodo);

        var thDato3 = document.createElement('td');
        thDato3.setAttribute("class", "sorting_1");
        thDato3.append(Datos[i].Num_TotalPolcar);

        var thDato4 = document.createElement('td');
        thDato4.setAttribute("class", "sorting_1");
        thDato4.append(Datos[i].Num_TotalBencar);
           
       
        trDatos.appendChild(thDato1);
        trDatos.appendChild(thDato2);
        trDatos.appendChild(thDato3);
        trDatos.appendChild(thDato4);
        //document.getElementById('RConsultaCurvaTasas').appendChild(trDatos);

        $body.append(trDatos)
    }

    initTable();
}

//FIN PRUEBA



function initTable() {
    $("#tblConsultaPeriodos").DataTable({
        "scrollY": "370px",
        "scrollCollapse": true,
        //"paging": true,
        "lengthMenu": [[10, 20, 50, 75, 100], [10, 20, 50, 75, 100]],
        "order": [[ 0, "desc" ]],
        "language": {
            //"sProcessing": "Procesando...",
            //"sLengthMenu": "Mostrar _MENU_ registros",
            //"sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "No se encontró ningún dato disponible en esta tabla",
            "sInfo": "_START_ al _END_ de _TOTAL_ registros",
            "search": '',
            "searchPlaceholder": "Buscar"
            //"sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            //"sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            //"sInfoPostFix": "",
            //"sSearch": "Buscar:",
            //"sUrl": "",
            //"sInfoThousands": ",",
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

// Boton Nuevo Periodo
function NuevoPeriodo() {
    //confirmar("Alerta", "Ejemplo de mensaje para las alertas.", function () { Prueba(); });
}

// Boton Periodo Seleccionado de la tabla
function PeriodoSeleccionado() {
    if (FechaPeriodo != '') {
        confirmar("Alerta", "¿Esta seguro que desea reabrir el periodo " + FechaPeriodo.toString().substr(6, 2) + "/" + FechaPeriodo.toString().substr(4, 2) + "/" + FechaPeriodo.toString().substr(0, 4) + "?", function () { AbrirPeriodoSeleccionado(); });
    } else {
        aviso("ERROR","Debe seleccionar una fecha periodo en la tabla.\n\n\n Operación cancelada. ");
    }
}

// Funcion para reabrir periodo seleccionado de la tabla
function AbrirPeriodoSeleccionado() {
    var urlReabrirPeriodo = $("#urlReabrirPeriodo").val();
    var fields = {
        fecha: FechaPeriodo.toString()
    };
    sendValues(fields, doSuccessReabrirPeriodo, doError, urlReabrirPeriodo);
}

//dosuccess reabrir periodo
function doSuccessReabrirPeriodo(result) {
    aviso("Aviso", result.Message)
    initTable();
    CargaTabla();
}

// Funcion para llenar variable global con la fecha del periodo a reabrir
function ObtenerReg(fecha) {
    FechaPeriodo = fecha;
}

// Modal de confirmación.
function confirmar(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar();
        $('#sch_modal_confirm').modal('hide');
    });
}

//dosuccess Guardar Periodo
function duSuccessGuardarPeriodo(result) {
    aviso("aviso", result.Message);
    CargaTabla();
}

//Formato de fecha.
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

//Función para guardar fecha de nuevo periodo
function GuardarNuevoPeriodo() {

    var f = new Date();
    var mesActual = (f.getMonth() + 1);

    var arrayFec = $("#inputNuevoPeriodo").val().split("/");
    fecha = new Date(arrayFec[0], arrayFec[1], 0);
    var mes = (fecha.getMonth() + 1);

    var urlGuardarPeriodo = $("#urlGuardarPeriodo").val();

    var dia = fecha.getDate()
    if (mes <= 9) { mes = "0" + mes; }
    var fields = {
        fecha: fecha.getFullYear() + "" + mes + "" + dia
    }
    sendValues(fields, duSuccessGuardarPeriodo, doError, urlGuardarPeriodo);
    
    $("#inputNuevoPeriodo").val("");
}