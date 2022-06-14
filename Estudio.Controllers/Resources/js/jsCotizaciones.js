(function () {
    localStorage.pagina = 'Cotizacion';
    initTable();
    $("#btnSearch").click(function () {
        filtro();
    });

    $(".chosen-select").chosen({ width: "95%" });

    // just for the demos, avoids form submit
    jQuery.validator.setDefaults({
        debug: true,
        success: "valid"
    });

    $("#cmbxTiposDocumento").change(function () {

        var obj = document.getElementById('Buscar');
        var valor = document.getElementById('cmbxTiposDocumento').options[document.getElementById('cmbxTiposDocumento').selectedIndex].value;

        if (valor == 0) {
            $('#Buscar').prop('disabled', true);
            $('#Buscar').val("");

        }
        else {
            $('#Buscar').attr('disabled', false);
            var url = $("#urlTipoDocumento").val();
            var idTipoDocumento = $("#cmbxTiposDocumento").val();

            var fields = {
                idTipoDocumento: idTipoDocumento
            };

            sendValues(fields, doSuccessTipoDocumento, doErrorTipoDocumento, url);
        }
    });

    $("#cmbxTiposDocumento").change(function () {
        var url = $("#urlTipoDocumento").val();
        var idTipoDocumento = $("#cmbxTiposDocumento").val();

        if (idTipoDocumento != "") {
            var fields = {
                idTipoDocumento: idTipoDocumento
            };

            sendValues(fields, doSuccessTipoDocumento, doErrorTipoDocumento, url);
        }
    });

    if ($("#cmbxTiposDocumento").val() != 0 || $("#Buscar").val() != "" || $("#BuscarN").val() != "" || $("#BuscarAp").val() != "" || $("#cmbxAsesores").val() != 0 || $("#BuscarCuspp").val() != "") {
        if ($("#cmbxTiposDocumento").val() != 0) {
            $('#Buscar').attr("disabled", false);
        }

        $("#btnSearch").trigger("click");
    }

})();

function initTable() {

    $("#tblConsultaCotizaciones").DataTable({
        "scrollY": "400px",
        //"scrollCollapse": true,
        "scrollX": true,
        "bAutoWidth": true,
        "language": {
            //"sProcessing": "Procesando...",
            //"sLengthMenu": "Mostrar _MENU_ registros",
            //"sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "No se encontró ningún dato disponible en esta tabla",
            //"sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
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
        "bDestroy": true,
        "dom": '<lf<t><"col-lg-6 col-md-6 col-sm-12"i>p>'
    });

    var table = $('#tblConsultaCotizaciones').DataTable();
    table.columns.adjust().draw();
}


function filtro() {
    // Validaciones (mínimo uno, etc)
    var url = $("#url").val();
    var cmbxTiposDocumento = document.getElementById("cmbxTiposDocumento").value;
    var filtroNumIdentificador = document.getElementById("Buscar").value.trim();
    var filtroNombres = document.getElementById("BuscarN").value.trim();
    var filtroApellidos = document.getElementById("BuscarAp").value.trim();
    var filtroCuspp = document.getElementById("BuscarAp").value.trim();
    var cmbxAsesores = document.getElementById("BuscarCuspp").value;

    if (cmbxTiposDocumento != "" && filtroNumIdentificador == "")
    {
        aviso("Aviso", "Debe ingresar el Numero de Documento");
    }
    if (cmbxTiposDocumento == "" && filtroNumIdentificador != "" && filtroNombres == "" && filtroApellidos == "" && cmbxAsesores == "" && filtroCuspp == "")
    {

        filtroNumIdentificador = ""
        aviso("Aviso", "Debe ingresar al menos un filtro");
    }
    if (cmbxTiposDocumento == "" && filtroNumIdentificador == "" && filtroNombres == "" && filtroApellidos == "" && cmbxAsesores == "" && filtroCuspp == "") {
        if ($("#urlConsultaAdmin").val() != undefined) {
            var fields =
               {
                   idTipoDocumento: document.getElementById("cmbxTiposDocumento").value == "" ? 0 : document.getElementById("cmbxTiposDocumento").value,
                   documento: document.getElementById("Buscar").value,
                   nombres: document.getElementById("BuscarN").value,
                   apellidos: document.getElementById("BuscarAp").value,
                   asesor: document.getElementById("cmbxAsesores").value == "" ? 0 : document.getElementById("cmbxAsesores").value,
                   cuspp: document.getElementById("BuscarCuspp").value
               };

            sendValues(fields, doSuccessConsulta, doErrorConsulta, url);
        }
        else
            aviso("Aviso", "Debe ingresar al menos un filtro");
    }
    else {
        var fields =
               {
                   idTipoDocumento: document.getElementById("cmbxTiposDocumento").value == "" ? 0 : document.getElementById("cmbxTiposDocumento").value,
                   documento: document.getElementById("Buscar").value,
                   nombres: document.getElementById("BuscarN").value,
                   apellidos: document.getElementById("BuscarAp").value,
                   asesor: document.getElementById("cmbxAsesores").value == "" ? 0 : document.getElementById("cmbxAsesores").value,
                   cuspp: document.getElementById("BuscarCuspp").value
               };

        sendValues(fields, doSuccessConsulta, doErrorConsulta, url);

    }
}

function doSuccessConsulta(result) {
    $("#tblConsultaCotizaciones").DataTable().destroy();
    var $body = $("#tblConsultaCotizaciones tbody");
    $body.empty();

    result.Object.forEach(function (v) {
        var tr = $('<tr>');
        var td = $('<td>');
        var nFilas = $("#tblConsultaCotizaciones tr").length;
        $("<td>").html(nFilas).appendTo(tr);
        $("<td>").html(v.TipoDocumento).appendTo(tr);
        $("<td>").html(v.Documento).appendTo(tr);
        $("<td>").html(v.CUSPP).appendTo(tr);
        $("<td>").html(v.Nombres).appendTo(tr);
        $("<td>").html(v.ApellidoPaterno).appendTo(tr);
        $("<td>").html(v.ApellidoMaterno).appendTo(tr);
        $("<td>").html(v.FechaCotizacionStr).appendTo(tr);
        $("<td>").html(v.Asesor).appendTo(tr);

        $("<div class='col-xs-3'><a href='javascript:Modificar(" + v.IdCotizacion + ");' title='Editar'><i class='fa fa-edit fa-lg icon-vida'></i></a></div>").appendTo(td);

        if ($("#hdnModificar").val() != undefined)
            $("<div class='col-xs-3'><a href='javascript:Clonar(" + v.IdCotizacion + ");' title='Clonar'><i class='fa fa-check-square-o fa-lg icon-vida'></i></a></div>").appendTo(td);
        else
            $("<div class='col-xs-3'><a href='#' disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></div>").appendTo(td);


        if ($("#hdnGenerarReporte")) {
            $("<div class='col-xs-3'><a href='javascript:Reporte(" + v.IdCotizacion + ");' title='Reporte sin Tasa'><i class='fa fa-download fa-lg icon-vida'></i></a></div>").appendTo(td);
            $("<div class='col-xs-3'><a href='javascript:ReporteT(" + v.IdCotizacion + ");' title='Reporte con Tasa'><i class='fa fa-search fa-lg icon-vida'></i></a></div>").appendTo(td);
        } else
            $("<div class='col-xs-3'><a href='#' disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida' ></i></a></div>").appendTo(td);

        //if ($("#hdnGenerarReporte"))
            $("<div class='col-xs-3'><a href='javascript:Delete(" + v.IdCotizacion + ");' title='Eliminar'><i class='fa fa-remove fa-lg icon-vida'></i></a></div>").appendTo(td);
        //else
        //    $("<div class='col-xs-3'><a href='#' disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida' ></i></a></div>").appendTo(td);


        tr.append(td);
        $body.append(tr)
    });
    initTable();
}

function Modificar(idCotizacion) {
    var url = $("#urlModificar").val();
    window.location.href = url + "?idCotizacion=" + idCotizacion + "&operacion=1";
}

function Clonar(idCotizacion) {
    var url = $("#urlClonar").val();
    window.location.href = url + "?idCotizacion=" + idCotizacion + "&operacion=2";
}

function Reporte(idCotizacion)
{
    var url = $("#urlReporte").val();
    window.open(url + "?idCotizacion=" + idCotizacion + "&reporte=1");
}

function ReporteT(idCotizacionReporte) {
    var url = $("#urlReporteT").val();
    window.open(url + "?idCotizacion=" + idCotizacionReporte + "&reporte=1");
}

function Delete(idCotizacion) {
    var header = "Confirmar";
        var body = "¿Deseas Eliminar esta Cotización?";

        confirmarCotizacion(header, body, function () { Eliminar(idCotizacion); });

}


function doErrorConsulta(result) {
    aviso("Aviso", result.Message);
}

//function doSuccessTipoDocumento(result) {
//    var infoDocumento = result.Object;

//    longitudDoc = infoDocumento.Longitud;
//    indicadorLongDoc = infoDocumento.IndicadorLongitudExacta;

//    document.getElementById("Buscar").maxLength = longitudDoc;

//}

//function doErrorTipoDocumento(result) {
//    aviso("Aviso", result.Message);
//}

function doSuccessTipoDocumento(result) {
    var infoDocumento = result.Object;
    var txtDocumentoCot = document.getElementById("Buscar");

    longitudDoc = infoDocumento.Longitud;
    indicadorLongDoc = infoDocumento.IndicadorLongitudExacta;

    document.getElementById("Buscar").maxLength = longitudDoc;

    switch (infoDocumento.Tipo) {
        case 'N':
            txtDocumentoCot.removeEventListener("keypress", letras);
            txtDocumentoCot.removeEventListener("keypress", validanumyletras);

            txtDocumentoCot.addEventListener("keypress", soloNumeros, false);
            break;
        case 'A':
            txtDocumentoCot.removeEventListener("keypress", soloNumeros);
            txtDocumentoCot.removeEventListener("keypress", validanumyletras);

            txtDocumentoCot.addEventListener("keypress", letras, false);
            break;
        default:
            txtDocumentoCot.removeEventListener("keypress", letras);
            txtDocumentoCot.removeEventListener("keypress", soloNumeros);

            txtDocumentoCot.addEventListener("keypress", validanumyletras, false);
            break;
    }

}

function doErrorTipoDocumento(result) {
    aviso("Aviso", result.Message);
}

// Mensaje

function aviso(header, body) {
    $('#msg_modal_header').text(header);
   
    $('#msg_modal_body').text(body);
    $('#sch_modal').modal('show');

    $("#btn_modal_aceptar").one('click', function () {
        $('#sch_modal').modal('hide');
    });
}

// Validaciones

function soloLetras(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
    especiales = "8-37-39-46";

    tecla_especial = false
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}

function pulsar(e) {
    e.preventDefault();
    if (e.keyCode === 13) {
        document.getElementById("btnSearch").click();

    }
}

function pulsarmodal(e) {
    e.preventDefault();
    if (e.keyCode === 13) {
        document.getElementById("btn_modal_aceptar").click();

    }
}


function Eliminar(idCotizacion) {
    var url = $("#urlEliminaCoti").val();

    var method = url;
    var fields = { idCotizacion: idCotizacion }
    sendValues(fields, doSuccessDelete, doErrorDelete, method);

}

var doSuccessDelete = function (result) {
    aviso(result.Message, "Se ha eliminado correctamente");
    var url = $("#url").val();
    var fieldsEliminar =
            {
                idTipoDocumento: document.getElementById("cmbxTiposDocumento").value == "" ? 0 : document.getElementById("cmbxTiposDocumento").value,
                documento: document.getElementById("Buscar").value,
                nombres: document.getElementById("BuscarN").value,
                apellidos: document.getElementById("BuscarAp").value,
                asesor: document.getElementById("cmbxAsesores").value == "" ? 0 : document.getElementById("cmbxAsesores").value,
                cuspp: document.getElementById("BuscarCuspp").value
            };

    sendValues(fieldsEliminar, doSuccessConsulta, doErrorConsulta, url);

}
var doErrorDelete = function (result) {
    aviso(result.Message, "Error");
}

function confirmarCotizacion(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar();
        $('#sch_modal_confirm').modal('hide');
    });
}

function validanumyletras(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patron de entrada, en este caso solo acepta numeros y letras
    patron = /[A-Za-z0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

function soloNumeros(e) {
    var key = window.event ? e.which : e.keyCode;
    if (key < 48 || key > 57) {
        e.preventDefault();
    }
}

function letras(e) {
    var key = window.event ? e.which : e.keyCode;
    if ((key < 65 || key > 90) && (key < 97 || key > 122)) {
        e.preventDefault();
    }
}

