//Funciones para acciones al cargar la página
(function () {
    localStorage.pagina = 'SolicitudCotizacion';
    initTable();
    var num = $("#txtNumArchivo").val();
    var url = $("#urlCalculadas").val();
    var fields = {
        numA: num
    };
    sendValues(fields, doSuccessTablaCalculadas, doError, url);

    var urlNC = $("#urlNoCalculadas").val();
    var fields = {
        numA: num
    };
    sendValues(fields, doSuccessTablaNoCalculadas, doError, urlNC);


    $("#btnExportar").click(function () {
        exportar();
    });

    $('#modalcargar').modal({
        drop: 'static',
        keyboard: false,
        show: true,
        backdrop: 'static'
    });

})();


//Funcion para enviar la informacion en un xml
function enviar() {
    var numArch = $("#txtNumArchivo").val();
    var nomArch = $("#txtNomArchivo").val();
    var url = $("#urlcrearXML").val();
    var fields = {
        strNum : numArch,
        strNom: nomArch
    };
    sendValues(fields, doSuccessXML, doError, url);
    $('#modalcargar').modal({
        drop: 'static',
        keyboard: false,
        show: true,
        backdrop: 'static'
    });
}
function doSuccessXML(result) {
    $('#modalcargar').modal('hide');
    if (result.Message == "OK" || result.Message == "OK#") {
        aviso("Aviso", "Se enviaron los datos correctamente");
    }
    else {
        aviso("ERROR", result.Message.split("#")[1]);
        //aviso("ERROR", "Surgío un error durante el envio");
    }
}

function doSuccessExcel(result) {
    aviso("Aviso", "El archivo ha sido exportado correctamente");
}
//Función para imprimir reporte de solicitudes calculadas.
function ImprimirCalculadas() {
    var numArch = $("#txtNumArchivo").val();
    var url = $("#urlRptCalculadas").val();

    window.open(url + "?strNum=" + numArch);
}



//Función para imprimir reporte de solicitudes no calculadas.
function ImprimirNoCalculadas() {
    var numArch = $("#txtNumArchivo").val();
    var url = $("#urlRptNoCalculadas").val();

    window.open(url + "?strNum=" + numArch);
}

function doError(result) {
    aviso("Aviso", result.Message);
}

//Función para llenar tablas de resultados.
function doSuccessTablaCalculadas(result) {
    if (result.IsOk) {
        if (result.Message == "No se encontro información") {
            alert("No existen registros para el tipo de moneda seleccionado.");
        } else {

            $("#tbltabCalculadas").DataTable().destroy();
            var $body = $("#tbltabCalculadas tbody");
            $body.empty();
            var i = 1;
            result.Object.forEach(function (v) {
                var tr = $("<tr class='textoTablas'>");
                var td = $('<td>');
                var nFilas = $("#tbltabCalculadas tr").length;
                $("<td>").html(v.Num_Orden).appendTo(tr);
                $("<td>").html(v.Num_Cotizacion).appendTo(tr);
                $("<td>").html(v.Num_Corr).appendTo(tr);
                $("<td>").html(v.Num_Operacion).appendTo(tr);
                $("<td>").html(v.CUSPP).appendTo(tr);
                $("<td>").html(v.Tipo_Pension).appendTo(tr);
                $("<td>").html(v.Tipo_Renta).appendTo(tr);
                $("<td>").html(v.Years_Dif).appendTo(tr);
                $("<td>").html(v.Modalidad).appendTo(tr);
                $("<td>").html(v.Years_Gar).appendTo(tr);
                //$("<td>").html(v.Cob_Cony).appendTo(tr);
                //$("<td>").html(v.D_Crecer).appendTo(tr);
                //$("<td>").html(v.D_Gratif).appendTo(tr);
                $("<td>").html(v.Moneda).appendTo(tr);
                $("<td>").html(parseFloat(v.TIR).toFixed(2)).appendTo(tr);
                $("<td>").html(parseFloat(v.Renta_Esc).toFixed(2)).appendTo(tr);
                $("<td>").html(parseFloat(v.Tasa_Venta).toFixed(2)).appendTo(tr);
                $("<td>").html(parseFloat(v.Mto_Pension).toFixed(2)).appendTo(tr);
                $("<td>").html(parseFloat(v.Tasa_RT).toFixed(2)).appendTo(tr);
                $("<td>").html(parseFloat(v.Mto_PensionRT).toFixed(2)).appendTo(tr);
                $("<td>").html(parseFloat(v.Prima_Unica).toFixed(2)).appendTo(tr);
                $("<td>").html(parseFloat(v.Perdida_Contable).toFixed(2)).appendTo(tr);
                $("<td>").html(v.Intermediario).appendTo(tr);
                $("<td>").html(parseFloat(v.PRC_Com).toFixed(2)).appendTo(tr);
                $("<td>").html(v.Ind_Mej).appendTo(tr);
                $body.append(tr)
                i++;
            });
            $('#modalcargar').modal('hide');
        }
    }
    initTable();
}

//Llenado de tabla para solicitudes no calculadas.
function doSuccessTablaNoCalculadas(result) {
    if (result.IsOk) {
        if (result.Message == "No se encontro información") {
            alert("No existen registros para el tipo de moneda seleccionado.");
        } else {

            $("#tbltabNoCalculadas").DataTable().destroy();
            var $body = $("#tbltabNoCalculadas tbody");
            $body.empty();
            var i = 1;
            result.Object.forEach(function (v) {
                var tr = $("<tr class='textoTablas'>");
                var td = $('<td>');
                var nFilas = $("#tbltabNoCalculadas tr").length;
                $("<td>").html(v.Num_Orden).appendTo(tr);
                $("<td>").html(v.Num_Cotizacion).appendTo(tr);
                $("<td>").html(v.Num_Corr).appendTo(tr);
                $("<td>").html(v.Num_Operacion).appendTo(tr);
                $("<td>").html(v.CUSPP).appendTo(tr);
                $("<td>").html(v.Tipo_Pension).appendTo(tr);
                $("<td>").html(v.Tipo_Renta).appendTo(tr);
                $("<td>").html(parseFloat(v.Mto_Cic).toFixed(2)).appendTo(tr);
                $("<td>").html(v.Years_Dif).appendTo(tr);
                $("<td>").html(v.Modalidad).appendTo(tr);
                $("<td>").html(v.Years_Gar).appendTo(tr);
                //$("<td>").html(v.Cob_Cony).appendTo(tr);
                //$("<td>").html(v.D_Crecer).appendTo(tr);
                //$("<td>").html(v.D_Gratif).appendTo(tr);
                $("<td>").html(v.Moneda).appendTo(tr);
                $("<td>").html(parseFloat(v.Renta_Esc).toFixed(2)).appendTo(tr);
                $("<td>").html(parseFloat(v.Prima_Unica).toFixed(2)).appendTo(tr);
                $("<td>").html(v.Intermediario).appendTo(tr);
                $("<td>").html(v.Ind_Mej).appendTo(tr);
                $("<td>").html(v.Error_NoCotizadas).appendTo(tr);
                $body.append(tr)
                i++;
            });

        }
    }
    initTable();
}

//Función para el estilo de la tabla con jQuery.
function initTable() {
    $("#tbltabCalculadas").DataTable({
        //"scrollX": true,
        "scrollCollapse": true,
        "paging": true,
        "lengthMenu": [[10, 20, 50, 75, 100], [10, 20, 50, 75, 100]],
        //"fixedColumns": true,
        "language": {
            //"sProcessing": "Procesando...",
            //"sLengthMenu": "Mostrar _MENU_ registros",
            //"sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "No se encontró ningún dato disponible en esta tabla",
            //"sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            //"sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            //"sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sUrl": "",
            "sInfoThousands": ",",
            "sSearch": '',
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


    $("#tbltabNoCalculadas").DataTable({
        //"scrollY": "300px",
        "scrollCollapse": true,
        "paging": true,
        "lengthMenu": [[10, 20, 50, 75, 100], [10, 20, 50, 75, 100]],
        "width": "20px",
        "padding-right": "0px",
        "padding-left": "0px",
        "language": {
            //"sProcessing": "Procesando...",
            //"sLengthMenu": "Mostrar _MENU_ registros",
            //"sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "No se encontró ningún dato disponible en esta tabla",
            "sInfoPostFix": "",
            "sUrl": "",
            "sInfoThousands": ",",
            "sSearch": '',
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

function exportar() {
    var num_Archivo = $("#txtNumArchivo").val();
    var url = $("#urlCrearExcel").val();

    window.open(url + "?num_Archivo=" + num_Archivo);
}

//var datos = "";

//function cambiarFile() {
//    var input = document.getElementById('file-input');
//    if (input.files && input.files[0]) {
//        var nombre = input.files[0].name;
//        if (esXML(nombre) == true) {
//            const reader = new FileReader();
//            reader.onload = function () {
//                // console.log(reader.result);
//                //json = { datos: reader.result };
//                $('#txtArchivoCargado').val(nombre);
//                datos = reader.result;
//            }
//            reader.readAsText(input.files[0]);
//            //}, false);
//        }
//        else {
//            aviso("ERROR", "El archivo no es extencion .xml");
//        }
//    } else {
//    }
//}

//function esXML(doc) {
//    var array = doc.split(".");
//    var res = false;
//    if (array[array.length - 1] == "xml" || array[array.length - 1] == "XML") {
//        res = true;
//    }
//    return res;
//}


//function enviarB() {
//    var nomArch = $("#txtArchivoCargado").val();
//    var url = $("#urlcrearXMLB").val();
//    var fields = {
//        __strNum: datos,
//        strNom: nomArch
//    };
//    sendValues(fields, doSuccessXML, doError, url);
//}
