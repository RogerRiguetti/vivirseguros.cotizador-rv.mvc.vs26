var filtroUsuario;
var filtroNombres;
var filtroApellidos;
(function () {
    localStorage.pagina = 'User';
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

    if ($("#UserCreate").val().toLowerCase() != "true") {
        $('#btnAdd').prop("href", "#");
        $('#btnAdd').css("cursor", "no-drop");
        $('#btnAdd').prop("title", "You have not permission");
    }

})();



function initTable() {

    $("#tblCatalog").DataTable({
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
        "bDestroy": true

    });
}
function filtro() {
    // Validaciones (mínimo uno, etc)
    var url = $("#UrlSearch").val();
    var account = document.getElementById("BusquedaUsuario").value.trim();
    var names = document.getElementById("BusquedaNombres").value.trim();
    var lastNames = document.getElementById("BusquedaApellidos").value.trim();

    if (account == "" && names == "" && lastNames == "") {
        aviso("Aviso", "Debe ingresar al menos un filtro");
    }
    else {
        var fields =
               {
                   account: account,
                   names: names,
                   lastNames: lastNames
               };

        sendValues(fields, doSuccessConsulta, doErrorConsulta, url);
    }
    filtroUsuario = account;
    filtroNombres = names;
    filtroApellidos = lastNames;
}

function doSuccessConsulta(result) {
    $("#tblCatalog").DataTable().destroy();
    var $body = $("#tblCatalog tbody");
    $body.empty();

    result.Object.forEach(function (v) {
        var tr = $('<tr>');
        var td = $('<td>');
        var nFilas = $("#tblCatalog tr").length;
        $("<td>").html(nFilas).appendTo(tr);
        $("<td>").html(v.Account).appendTo(tr);
        $("<td>").html(v.Names).appendTo(tr);
        $("<td>").html(v.LastNames).appendTo(tr);
        $("<td>").html(v.Correo).appendTo(tr);
        $("<td>").html(v.Status).appendTo(tr);
        $("<td>").html(v.DateCreatedStr).appendTo(tr);
        $("<td>").html(v.DateModifiedStr).appendTo(tr);
        $("<td>").html(v.RolStr).appendTo(tr);
        if ($("#RedirectToDetails").val() != undefined)
            $("<div class='col-xs-3'><a href='javascript:Details(" + v.Id + ");' title='Detalles'><i class='fa fa-search fa-lg icon-vida'></i></a></div>").appendTo(td);
        else
            $("<div class='col-xs-3'><a href='#' disabled='true' title='You have not permission'><i class='fa fa-ban fa-lg icon-vida'></i></a></div>").appendTo(td);

        if ($("#RedirectToEdit").val() != undefined)
            $("<div class='col-xs-3'><a href='javascript:Edit(" + v.Id + ");' title='Editar'><i class='fa fa-edit fa-lg icon-vida'></i></a></div>").appendTo(td);
        else
            $("<div class='col-xs-3'><a href='#' disabled='true' title='You have not permission'><i class='fa fa-ban fa-lg icon-vida'></i></a></div>").appendTo(td);

        if ($("#url").val() != undefined) {
            if (v.Status == "Activo") {
                $("<div class='col-xs-3'><a href='javascript:Delete(" + v.Id + "," + 0 + ");' title='Desactivar'><i class='fa fa-remove fa-lg icon-vida'></i></a></div>").appendTo(td);
            }
            else {
                $("<div class='col-xs-3'><a href='javascript:Delete(" + v.Id + ", " + 1 + ");' title='Activar'><i class='fa fa-check fa-lg icon-vida'></i></a></div>").appendTo(td);
            }
        }
        else
            $("<div class='col-xs-3'><a href='#' disabled='true' title='You have not permission'><i class='fa fa-ban fa-lg icon-vida'></i></a></div>").appendTo(td);


        tr.append(td);
        $body.append(tr)
    });
    initTable();
}

function doErrorConsulta(result) {
    aviso("Aviso", result.Message);
}

function Configure(id) {
    var url = $("#RedirectToConfigure").val();
    window.location.href = url + "/" + id;
}
function Details(id) {
    var url = $("#RedirectToDetails").val();
    window.location.href = url + "/" + id;

}

function Edit(id, account, password, userProfile_UserId) {
    var url = $("#RedirectToEdit").val();
    window.location.href = url + "/" + id;
}

function Delete(id, active) {
  
    var header = "Confirmar";
    if (active == 1) {
        var body = "¿Deseas activar este usuario?";
    } else {
        var body = "¿Deseas desactivar este usuario?";
    }
    confirmarUsuario(header, body, function () { Eliminar(id, active); });

}

function Eliminar(id, active) {
    var url = $("#url").val();
   
    var method = url;
    var fields = { id: id, active: active }
    sendValues(fields, doSuccessDelete, doErrorDelete, method);
   
}

var doSuccessDelete = function (result) {
    aviso(result.Message, "Se ha cambiado correctamente");
    var url = $("#UrlSearch").val();
    var fieldsEliminar =
            {
                account: filtroUsuario,
                names: filtroNombres,
                lastNames: filtroApellidos
            };

    sendValues(fieldsEliminar, doSuccessConsulta, doErrorConsulta, url);
  
}
var doErrorDelete = function (result) {
    aviso(result.Message, "Error");
}

function aviso(header, body) {
    $('#msg_modal_header').text(header);

    $('#msg_modal_body').text(body);
    $('#sch_modal').modal('show');

    $("#btn_modal_aceptar").one('click', function () {
        $('#sch_modal').modal('hide');
    });
}

function confirmarUsuario(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}

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
//Función para buscar con la tecla Enter.
function pulsar(e) {
    e.preventDefault();
    if (e.keyCode === 13) {
        document.getElementById("btnSearch").click();

    }
}


