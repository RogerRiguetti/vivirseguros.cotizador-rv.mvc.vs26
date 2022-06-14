(function () {
    localStorage.pagina = 'Role';
    if ($("#RoleCreate").val().toLowerCase() != "true") {
        $('#btnAdd').prop("href", "#");
        $('#btnAdd').css("cursor", "no-drop");
        $('#btnAdd').prop("title", "You have not permission");
    }
    initTable();
  
})();


function initTable() {

    $("#tblCatalog").DataTable({
        "language": {
            //"sProcessing": "Procesando...",
            //"sLengthMenu": "Mostrar _MENU_ registros",
            //"sZeroRecords": "No se encontraron resultados",
            //"sEmptyTable": "Ningún dato disponible en esta tabla",
            //"sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            //"sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            //"sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            //"sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "search": '',
            "searchPlaceholder": "Buscar"
            //"sLoadingRecords": "Cargando...",
            /*"oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }*/
        },
        "bDestroy": true

    });
}

function dtConvFromJSON(data) {
    
    if (data == null) return '01/01/1950';
    var r = /\/Date\(([0-9]+)\)\//gi
    var matches = data.match(r);
    if (matches == null) return '01/01/1950';
    var result = matches.toString().substring(6, 19);
    var epochMilliseconds = result.replace(
    /^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/,
    '$1');
    var b = new Date(parseInt(epochMilliseconds));
    var c = new Date(b.toString());
    var curr_date = c.getDate();
    var curr_month = c.getMonth() + 1;
    var curr_year = c.getFullYear();
    var curr_h = c.getHours();
    var curr_m = c.getMinutes();
    var curr_s = c.getSeconds();
    var curr_offset = (c.getTimezoneOffset() / 60);

    if (curr_date < 10)
        curr_date = "0" + curr_date;
    if (curr_month < 10)
        curr_month = "0" + curr_month;

    var d = curr_date + '/' + curr_month + '/' + curr_year + " " + curr_h + ':' + curr_m + ':' + curr_s;
    return d;
}

function Configure(id) {
    var url = $("#RedirectToConfigure").val();
    window.location.href = url + "/" + id;
}
function Details(id) {
    var url = $("#RedirectToDetails").val();
    window.location.href = url + "/" + id;

}

function Edit(id) {
    var url = $("#RedirectToEdit").val();
    window.location.href = url + "/" + id;
    }

    function Delete(id, active) {
       
        var header = "Confirmar";
        if (active == 1) {
            var body ="¿Deseas activar este rol?";
        } else {
            var body = "¿Deseas desactivar este rol?";
        }
        confirmarRol(header, body, function () { Eliminar(id,active); });
    }

    function Eliminar(id,active) {
        var url = $("#RedirectToDelete").val();
        var method = url;
        
        var fields = { id: id, active:active }
        sendValues(fields, doSuccessDelete, doErrorDelete, method);
    }

var doSuccessDelete = function (result) {
    aviso(result.Message, "");
}
var doErrorDelete = function (result) {
}

function aviso(header, body) {
    $('#msg_modal_header').text("Aviso");
    $('#msg_modal_body').text("Rol actualizado con éxito");
    $('#sch_modal').modal('show');

    $("#btn_modal_aceptar").one('click', function () {
        $('#sch_modal').modal('hide');
        location.reload();
    });
}

function confirmarRol(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}