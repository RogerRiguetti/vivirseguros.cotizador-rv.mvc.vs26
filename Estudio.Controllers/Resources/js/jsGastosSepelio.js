$(document).ready(function () {
    $('#tblConsultaGastosSepelio tbody').on('click', 'tr', function () {
        var table = $('#tblConsultaGastosSepelio').DataTable();
        if ($(this).hasClass('selected')) {
            table.$('tr.selected').removeClass('selected');
            $(this).removeClass('selected');
            cargarInformacion(0, 0, 0, false);
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    var funciones = document.getElementById('tblConsultaGastosSepelio_wrapper');
    var todasLasFunciones = funciones.getElementsByClassName('col-sm-6');
    Array.from(todasLasFunciones).forEach(e => {
        e.removeAttribute('class');
        e.setAttribute('class', 'col-md-12');
    }, this);
});
(function () {
    localStorage.pagina = 'GastosSepelio';
    initTable();
    var url = $("#urlCargarTabla").val();
    var fields = {};
    sendValues(fields, doSuccessConsultaGastoSepelio, doError, url);

})();
function initTable() {
    $("#tblConsultaGastosSepelio").DataTable({
        //"scrollY": "300px",
        "scrollCollapse": true,
        "paging": true,
        "lengthMenu": [[10, 20, 50, 75, 100], [10, 20, 50, 75, 100]],
        "language": {
            //"sProcessing": "Procesando...",
            //"sLengthMenu": "Mostrar _MENU_ registros",
            //"sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "No se encontró ningún dato disponible en esta tabla",
            "sInfo": "Mostrando del _START_ al _END_ de _TOTAL_ registros",
            "sInfoEmpty": "0 al 0 de 0 registros",
            //"sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
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
    var funciones = document.getElementById('tblConsultaGastosSepelio_wrapper');
    var todasLasFunciones = funciones.getElementsByClassName('col-sm-6');
    Array.from(todasLasFunciones).forEach(e => {
        e.removeAttribute('class');
        e.setAttribute('class', 'col-md-12');
    }, this);
}
function doError(result) {
    aviso("Aviso", result.Message);
}
function doSuccessConsultaGastoSepelio(result) {
    if (result.Message == "Información cargada con éxito") {
        $("#tblConsultaGastosSepelio").DataTable().destroy();
        var $body = $("#tblConsultaGastosSepelio tbody");
        $body.empty();
        var i = 1;
        result.Object.forEach(function (v) {
            //var nFilas = $("#tblConsultaCotizaciones tr").length; <td><span style='display: none;'>20150221</span>21/02/2015</td>
            d = "cargarInformacion('" + v.FechaInicial + "','" + v.FechaTermino + "','" + v.Valor + "','true')";
            var tr = $('<tr onclick="' + d + '">');
            var td = $('<td>');
            $("<td>").html(i).appendTo(tr);
            $("<td>").html(v.FechaInicial).appendTo(tr);
            $("<td>").html(v.FechaTermino).appendTo(tr);
            $("<td>").html(v.Valor).appendTo(tr);
            $body.append(tr)
            i++;
        });
        initTable();
        var funciones = document.getElementById('tblConsultaGastosSepelio_wrapper');
        var todasLasFunciones = funciones.getElementsByClassName('col-sm-6');
        Array.from(todasLasFunciones).forEach(e => {
            e.removeAttribute('class');
            e.setAttribute('class', 'col-md-12');
        }, this);
    } else {
        aviso("Aviso", result.Message);
    } 
}
function cargarInformacion(a, b, c, bandera) {
    if (bandera) {
        var res = a.split("/");
        document.getElementById("fecIni").value = res[2] + "-" + res[1] + "-" + res[0];
        res = b.split("/");
        document.getElementById("fecFin").value = res[2] + "-" + res[1] + "-" + res[0];
        document.getElementById("txtValorTMM").value = c;
        document.getElementById("fecIni").disabled = true;
        document.getElementById("txtValorTMM").focus();
    } else {
        document.getElementById("fecIni").value = $("#backFecha").val();
        document.getElementById("fecFin").value = $("#backFecha").val();
        document.getElementById("txtValorTMM").value = "";
        document.getElementById("fecIni").disabled = false;
        document.getElementById("fecIni").focus();
    }
}
function Limpiar() {
    var table = $('#tblConsultaGastosSepelio').DataTable();
    table.$('tr.selected').removeClass('selected');
    var funciones = document.getElementById('tblConsultaGastosSepelio_wrapper');
    var todasLasFunciones = funciones.getElementsByClassName('col-sm-6');
    Array.from(todasLasFunciones).forEach(e => {
        e.removeAttribute('class');
        e.setAttribute('class', 'col-md-12');
    }, this);
    document.getElementById("fecIni").value = $("#backFecha").val();;
    document.getElementById("fecFin").value = $("#backFecha").val();;
    document.getElementById("txtValorTMM").value = "";
    document.getElementById("fecIni").disabled = false;
    document.getElementById("fecIni").focus();
}
function Grabar() {
    var fechaActual = new Date();
    var strFec = $("#fecIni").val();
    if (strFec.trim() == "") {
        aviso("Aviso", "Debe ingresar la Fecha de Inicio de Vigencia.");
        return;
    }

    var fecha = new Date(strFec);//se formatea la fecha ingresada
    if (fechaActual.getTime() < fecha.getTime()) {
        aviso("Aviso", "La Fecha de Inicio es mayor a la fecha actual");
        return;
    }
    if (fecha.getFullYear() < 1900) {
        aviso("Aviso", "La Fecha de Inicio es inferior a la Fecha Mínima de Ingreso (1900).");
        return;
    }
    var Cuota = $("#txtValorTMM").val();
    if (Cuota.trim() == "") {
        aviso("Aviso", "Debe ingresar un Valor para la Cuota Mortuoria seleccionada.");
        return;
    }
    //se ejecuta la funcion de grabar
    var url = $("#urlConsulta").val();
    var fields = {
        strFecIni: strFec
    };
    sendValues(fields, doSuccessConsultaSepelio, doError, url);
}
function doSuccessConsultaSepelio(result) {
    if (result.Message == "No se encontro información") {
     // confirmarSepelio("Aviso", "¿ Está seguro que desea Grabar la Información ?", result, 2);
        confirmarSepelio("Aviso", "¿ Está seguro que desea Grabar la Información ?", function () { NuevoGastoSepelio(result,2); });
        return;
    }
    if (result.Message == "Información cargada con éxito") {
     // confirmarSepelio("Aviso", "¿ Está seguro que desea Modificar la Información ?", result, 3);
        confirmarSepelio("Aviso", "¿ Está seguro que desea Modificar la Información ?", function () { NuevoGastoSepelio(result, 3); });
        return;
    }
}
function NuevoGastoSepelio(result, bandera) {
    if (bandera == 2) {//genera el nuevo registro
        var url = $("#urlGrabarSepelio").val();
        var strFec = $("#fecIni").val();
        var strFecFin = $("#fecFin").val();
        var Cuota = $("#txtValorTMM").val();
        var fields = {
            strFecIni: strFec,
            vlGasto: Cuota,
            bandera: true,
            strFecFin: strFecFin
        };
        sendValues(fields, doSuccessGrabarSepelio, doError, url);

        return;
    }
    if (bandera == 3) {//modifica el registro
        var url = $("#urlGrabarSepelio").val();
        var strFec = $("#fecIni").val();
        var strFecFin = $("#fecFin").val();
        var Cuota = $("#txtValorTMM").val();
        var fields = {
            strFecIni: strFec,
            vlGasto: Cuota,
            bandera: false,
            strFecFin: strFecFin
        };
        sendValues(fields, doSuccessGrabarSepelio, doError, url);

        return;
    }
}
function doSuccessGrabarSepelio(result) {
    var Vigencia = result.Object;
    document.getElementById("fecFin").value = Vigencia.FechaTermino;
    //recarga la tabla
    var url = $("#urlCargarTabla").val();
    var fields = {};
    sendValues(fields, doSuccessConsultaGastoSepelio, doError, url);
    aviso("Aviso", result.Message);
}
function Eliminar() {

    var strFec = $("#fecIni").val();
    if (strFec == "") {
        aviso("Aviso", "Debe ingresar la Fecha de Inicio de Vigencia de la Cuota Mortuoria. Debe Seleccionar el Periodo que Desea Eliminar");
        return;
    }

    var url = $("#urlConsulta").val();
    var fields = {
        strFecIni: strFec
    };
    sendValues(fields, doSuccessConsultaSepelioEliminar, doError, url);


}
function doSuccessConsultaSepelioEliminar(result) {
    if (result.Message == "Información cargada con éxito") {
        var Vigencia = result.Object;
        var res = Vigencia.FechaTermino.split("/");
        document.getElementById("fecFin").value = res[2] + "-" + res[1] + "-" + res[0];
        document.getElementById("txtValorTMM").value = Vigencia.Valor;
     // confirmarSepelio("Aviso", "Desea Eliminar el Periodo " + Vigencia.FechaInicial + " * " + Vigencia.FechaTermino + " ?", result, 4);
        confirmarSepelio("Aviso", "Desea Eliminar el Periodo " + Vigencia.FechaInicial + " * " + Vigencia.FechaTermino + " ?", function () { EliminarSepelio(result); });
    } else {
        aviso("Aviso", "El Periodo No Existe");
    }
}
function EliminarSepelio(Result) {
    var strFec = $("#fecIni").val();
    var url = $("#urlEliminarSepelio").val();
    var fields = {
        strFecIni: strFec
    };
    sendValues(fields, doSuccessSepelioEliminar, doError, url);
    Limpiar();
}
function doSuccessSepelioEliminar() {

    //recarga la tabla
    var url = $("#urlCargarTabla").val();
    var fields = {};
    sendValues(fields, doSuccessConsultaGastoSepelio, doError, url);

    aviso("Aviso", result.Message);
}
function Salir() {
    var url = $("#urlSalir").val();
    window.location.href = url;
}
function Imprimir() {
    //NS - Nuevos Soles"
    var url = $("#urlReporte").val();
    window.open(url);
}
function confirmarSepelio(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}/*
function confirmarSepelio(header, body, result, bandera) {
    $('#msg_modal_header_2').text(header);
    $('#msg_modal_body_2').text(body);
    $('#sch_modal_Anclaje').modal('show');

    $("#btn_modal_aceptar_2").one('click', function () {
        $('#sch_modal_Anclaje').modal('hide');
        if (bandera == 2)//genera el nuevo registro
            NuevoGastoSepelio(result, bandera);
        if (bandera == 3)//modifica el registro
            NuevoGastoSepelio(result, bandera);
        if (bandera == 4)//Elimina el registro
            EliminarSepelio(result);
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