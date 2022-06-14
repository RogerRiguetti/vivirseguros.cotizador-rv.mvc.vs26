$(document).ready(function () {
    $('#tblValoresMoneda tbody').on('click', 'tr', function () {
        var table = $('#tblValoresMoneda').DataTable();
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

//Funciones para acciones al cargar la página
(function () {
    localStorage.pagina = 'ValoresMoneda';
    initTable();
    //doSuccessValoresMoneda();

    // Modal
    $('#sch_modal').modal({
        keyboard: false,
        backdrop: "static",
        show: false
    });

    //Funcion para el llenado del combo de moneda
    $("#cmbxTipoMoneda").change(function () {
        var id = $("#cmbxTipoMoneda").val();
        var strFecVM = $("#txtFecVM").val();
        if (id == "") {
            //Limpiar los campos
            document.getElementById("txtFecVM").value = $("#backFecha").val();
            document.getElementById("txtValorVM").value = "";

        } else {

            var url = $("#urlValorMoneda").val();
            var fields = {
                vlMoneda: id
            };
            sendValues(fields, doSuccessValoresMoneda, doError, url);
        }
    });

})();

//Función para botón de grabar (actualizar o insertar registros)
function Grabar() {
    var id = $("#cmbxTipoMoneda").val();
    if (id == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda.");
        document.getElementById("cmbxTipoMoneda").focus();
        return;
    }

    var strFec = $("#txtFecVM").val();
    if (strFec == "") {
        aviso("Aviso", "Debe ingresar una Fecha para el Valor de Moneda.");
        document.getElementById("txtFecVM").focus();
        return;
    }
    var valorM = $("#txtValorVM").val();
    if (valorM == "") {
        aviso("Aviso", "Debe ingresar un Valor para la Moneda seleccionada.");
        document.getElementById("txtValorVM").focus();
        return;
    }

    var hoy = new Date();
    var dateFec = new Date(strFec);
    var año = dateFec.getFullYear(new Date(strFec));

    if (año < 1900) {
        aviso("Aviso", "La Fecha ingresada es inferior a la Fecha Mínima de Ingreso (1900).");
        document.getElementById("txtFecVM").focus();
        return;
    }

    if (dateFec > hoy) {
        aviso("Aviso", "La Fecha ingresada es mayor a la fecha actual.");
        document.getElementById("txtFecVM").focus();
        return;
    }

    //Verifica la existencia de los datos para determinar inserción nuevos registros o modificación de datos existentes.
    var url = $("#urlBuscarVM").val();
    var res = id.split("#");
    var fields = {
        vlMoneda: res[0],
        fechaVM: strFec
    };
    sendValues(fields, doSuccessVerificarReg, doError, url);
}

//Función para botón eliminar
function Eliminar() {
    var id = $("#cmbxTipoMoneda").val();
    if (id == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda");
        document.getElementById("cmbxTipoMoneda").focus();
        return;
    }

    var strFec = $("#txtFecVM").val();
    if (strFec == "") {
        aviso("Aviso", "Debe ingresar una Fecha para el Valor de Moneda");
        document.getElementById("txtFecVM").focus();
        return;
    }/*
    var valorM = $("#txtValorVM").val();
    if (valorM == "") {
        aviso("Aviso", "Debe ingresar un Valor para la Moneda seleccionada.");
        document.getElementById("txtValorVM").focus();
        return;
    }*/

    var hoy = new Date();
    var dateFec = new Date(strFec);
    var año = dateFec.getFullYear(new Date(strFec));

    if (año < 1900) {
        aviso("Aviso", "La Fecha ingresada es inferior a la Fecha Mínima de Ingreso (1900).");
        document.getElementById("txtFecVM").focus();
        return;
    }

    if (dateFec > hoy) {
        aviso("Aviso", "La Fecha ingresada es mayor a la fecha actual.");
        document.getElementById("txtFecVM").focus();
        return;
    }

    //Verifica la existencia de los datos para determinar inserción nuevos registros o modificación de datos existentes.
    var url = $("#urlBuscarVM").val();
    var fields = {
        vlMoneda: id,
        fechaVM: strFec
    };
    sendValues(fields, doSuccessEliminaVM, doError, url);
}

function Imprimir() {
    var url = $("#urlImprimirVM").val();
    var id = $("#cmbxTipoMoneda").val();
    var tex = $("#cmbxTipoMoneda option:selected").text();

    if (id == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda");
        document.getElementById("cmbxTipoMoneda").focus();
        return;
    }

    window.open(url + "?vlMoneda=" + id + "&strMoneda=" + tex);
}

function ProcesosVM(result, bandera) {

    //desativar los combos
    document.getElementById("txtFecVM").disabled = true;//desactiva
    document.getElementById("cmbxTipoMoneda").disabled = true;
    //document.getElementById("btnBuscar").style.display = "none";
    //document.getElementById("btnBuscar2").style.display = "";

    if (bandera == 2) {//NUEVO REGISTRO
        var url = $("#urlGrabarVM").val();
        var id = $("#cmbxTipoMoneda").val();
        var valorM = $("#txtValorVM").val();
        var fecVM = $("#txtFecVM").val();
        var fields = {
            vlMoneda: id,
            fechaVM: fecVM,
            valorM: valorM,
            bandera: true
        };
        sendValues(fields, doSuccessGrabarVM, doError, url);
        return;
    }
    if (bandera == 3) {//MODIFICAR REGISTRO (UPDATE)
        var url = $("#urlGrabarVM").val();
        var id = $("#cmbxTipoMoneda").val();
        var valorM = $("#txtValorVM").val();
        var fecVM = $("#txtFecVM").val();
        var fields = {
            vlMoneda: id,
            fechaVM: fecVM,
            valorM: valorM,
            bandera: false
        };


        sendValues(fields, doSuccessGrabarVM, doError, url);
        return;
    }
}

//Función para eliminar el registro indicado.
function EliminarVM(result) {
    $('#cmbxTipoMoneda').attr("disabled", true);
    $('#txtFecVM').attr("disabled", true);

    var url = $("#urlEliminaVM").val();
    var id = $("#cmbxTipoMoneda").val();
    var valorM = $("#txtValorVM").val();
    var fecVM = $("#txtFecVM").val();
    var fields = {
        vlMoneda: id,
        fechaVM: fecVM
    };
    sendValues(fields, doSuccessEliminar, doError, url);
    return;
}

//Alerta de confirmación y consulta a BD para observar cambios.
function doSuccessGrabarVM(result) {
    aviso("Aviso", result.Message);

    var url = $("#urlValorMoneda").val();
    var id = $("#cmbxTipoMoneda").val();
    var fields = {
        vlMoneda: id
    };
    sendValues(fields, doSuccessValoresMoneda, doError, url);

}

//Alerta de confirmación y consulta a BD para observar cambios.
function doSuccessEliminar(result) {
    aviso("Aviso", result.Message);

    var url = $("#urlValorMoneda").val();
    var id = $("#cmbxTipoMoneda").val();
    var fields = {
        vlMoneda: id
    };
    sendValues(fields, doSuccessValoresMoneda, doError, url);

}


//Función para confirmar inserción o modificación de datos.
function doSuccessVerificarReg(result) {
    if (result.Message == "No se encontro información") {
        confirmarValorM("Aviso", "¿ Está seguro que desea Grabar la Información ?", function () { ProcesosVM(result, 2); });
        // confirmarValorM("Aviso", "¿ Está seguro que desea Grabar la Información ?", result, 2);
        return;
    }
    if (result.Message == "Información cargada con éxito") {
        confirmarValorM("Aviso", "¿ Está seguro que desea Modificar la Información ?", function () { ProcesosVM(result, 3); });
        // confirmarValorM("Aviso", "¿ Está seguro que desea Modificar la Información ?", result, 3);
        return;
    }
}

//Función para confirmar eliminación de datos.
function doSuccessEliminaVM(result) {
    if (result.Message == "Información cargada con éxito") {
        // confirmarValorM("Aviso", "¿ Está seguro que desea Eliminar los Datos ?", result, 4);
        confirmarValorM("Aviso", "¿ Está seguro que desea Eliminar los Datos ?", function () { EliminarVM(result); });
    } else {
        aviso("Aviso", "La Moneda y Fecha indicada no se encuentran registradas en la BD.");
    }
}

//Función de botón limpiar
function Limpiar() {
    $('#cmbxTipoMoneda').val('');
    $('#cmbxTipoMoneda').attr("disabled", false);
    $('#txtFecVM').attr("disabled", false);
    document.getElementById("txtFecVM").value = $("#backFecha").val();
    document.getElementById("txtValorVM").value = "";
    $("#tblValoresMoneda").DataTable().destroy();
    var $body = $("#tblValoresMoneda tbody");
    $body.empty();

    initTable();

}

function doError(result) {
    aviso("Aviso", result.Message);
}

//Función para cargar los datos de la fila seleccionada en la tabla los campos de fecha y valor.
function cargarInformacion(a, b, bandera) {
    if (bandera) {
        var res = a.split("/");
        document.getElementById("txtFecVM").value = res[2] + "-" + res[1] + "-" + res[0];
        document.getElementById("txtValorVM").value = b;
        document.getElementById("txtFecVM").disabled = true;
        document.getElementById("cmbxTipoMoneda").disabled = true;
        document.getElementById("txtValorVM").focus();
    } else {
        document.getElementById("txtFecVM").value = $("#backFecha").val();
        document.getElementById("txtValorVM").value = "";
        document.getElementById("txtFecVM").disabled = false;
        document.getElementById("cmbxTipoMoneda").disabled = false;
        document.getElementById("txtFecVM").focus();
    }
}

//Alertas en Modal
function confirmarValorM(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}
/*
function confirmarValorM(header, body, result, bandera) {
    $('#msg_modal_header_2').text(header);
    $('#msg_modal_body_2').text(body);
    $('#sch_modal_Anclaje').modal('show');

    $("#btn_modal_aceptar_2").one('click', function () {
        $('#sch_modal_Anclaje').modal('hide');
        if (bandera == 1)//busqueda
            ProcesosVM(result, bandera);
        if (bandera == 2)//genera el nuevo registro
            ProcesosVM(result, bandera);
        if (bandera == 3)//modifica el registro
            ProcesosVM(result, bandera);
        if (bandera == 4)//Elimina el registro
            EliminarVM(result);
    });
}*/

//Función para llenar tabla con registros de BD.
function doSuccessValoresMoneda(result) {
    if (result.IsOk) {
        if (result.Message == "No se encontro información") {
            alert("No existen registros para el tipo de moneda seleccionado.");
        } else {

            $("#tblValoresMoneda").DataTable().destroy();
            var $body = $("#tblValoresMoneda tbody");
            $body.empty();
            var i = 1;
            result.Object.forEach(function (v) {
                d = "cargarInformacion('" + v.FechaVM + "','" + parseFloat(v.ValorVM).toFixed(3) + "','true')";
                var tr = $('<tr onclick="' + d + '">');
                var td = $('<td>');
                var nFilas = $("#tblValoresMoneda tr").length;
                $("<td>").html(i).appendTo(tr);
                $("<td>").html(v.FechaVM).appendTo(tr);
                $("<td>").html(parseFloat(v.ValorVM).toFixed(3)).appendTo(tr);
                $body.append(tr)
                i++;
            });

        }
    }
    initTable();
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

//Función para el estilo de la tabla con jQuery.
function initTable() {
    $("#tblValoresMoneda").DataTable({
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
function Salir() {
    var url = $("#urlSalir").val();
    window.location.href = url;
}