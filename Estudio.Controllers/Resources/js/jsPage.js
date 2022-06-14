(function () {

    if ($("#PageCreate").val().toLowerCase() != "true") {
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
        "columnDefs": [
            { "width": "20xp", "targets": 0 },
            { "width": "200px", "targets": 1 },
            { "width": "160px", "targets": 2 },
            { "width": "160px", "targets": 3 },
            { "width": "160px", "targets": 4 },
            { "width": "90px", "targets": 5 },

        ], 
        "bDestroy": true

    });
}

function Search() {



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
        "sAjaxDataProp": "data",
        "ajax": {
            "url": $('#UrlSearch').val(),
            "type": "GET",
            "datatype": "json",
            "data": { description: "", active: "" }
        },
        "columnDefs": [
            { "width": "20xp", "targets": 0 },
            { "width": "200px", "targets": 1 },
            { "width": "160px", "targets": 2 },
            { "width": "160px", "targets": 3 },
            { "width": "160px", "targets": 4 },
            { "width": "90px", "targets": 5 },
          
        ], 
        "columns": [
            { "data": "Id" },
            { "data": "Description" },
            { "data": "System" },
            { "data": "Module" },
            { "data": "Status" },
            {
                "mRender": function (data, type, full) {

                    return ($("#PageDetails").val().toLowerCase() === "true" ?
                        ("<div class='row'><div class='col-xs-3'><a  href=\"javascript:Details('" + full.Id + "');\"><i class='fa fa-search fa-lg'></i></a></div>"
                            +
                            ($("#PageEdit").val().toLowerCase() === "true" ?
                                "<div class='col-xs-3'><a href=\"javascript:Edit('" + full.Id + "');\"><i class='fa fa-edit'></i></a></div>"
                                : "<div class='col-xs-3'><a title='You have not permission' disabled='disabled'  href='#'><i class='fa fa-ban fa-lg'></i></a></div>")
                            +
                            ($("#PageDelete").val().toLowerCase() === "true" ?
                                "<div class='col-xs-3'><a href=\"javascript:Delete('" + full.Id + "', '" + full.Description + "');\"><i class='fa fa-trash fa-lg'></i></a></div></div>"
                                : "<div class='col-xs-3'><a title='You have not permission' disabled='disabled'  href='#'><i class='fa fa-ban fa-lg'></i></a></div></div>")

                            +
                            ($("#PageConfigure").val().toLowerCase() === "true" ?
                                "<div class='col-xs-3'><a href=\"javascript:Configure('" + full.Id + "');\"><i class='fa fa-gear fa-lg'></i></a></div></div>"
                                : "<div class='col-xs-3'><a title='You have not permission' disabled='disabled'  href='#'><i class='fa fa-ban fa-lg'></i></a></div></div>")

                        )


                        : ("<div class='row'><div class='col-xs-3'><a title='You have not permission' disabled='disabled'  href='#'><i class='fa fa-ban fa-lg'></i></a></div>"
                            +
                            ($("#PageEdit").val().toLowerCase() === "true" ?
                                "<div class='col-xs-3'><a href=\"javascript:Edit('" + full.Id + "');\"><i class='fa fa-edit fa-lg'></i></a></div>"
                                : "<div class='col-xs-3'><a title='You have not permission' disabled='disabled'  href='#'><i class='fa fa-ban fa-lg'></i></a></div>")
                            +
                            ($("#PageDelete").val().toLowerCase() === "true" ?
                                "<div class='col-xs-3'><a href=\"javascript:Delete('" + full.Id + "', '" + full.Description + "');\"><i class='fa fa-trash fa-lg'></i></a></div></div>"
                                : "<div class='col-xs-3'><a title='You have not permission' disabled='disabled'  href='#'><i class='fa fa-ban fa-lg'></i></a></div></div>")

                            +
                            ($("#PageConfigure").val().toLowerCase() === "true" ?
                                "<div class='col-xs-3'><a href=\"javascript:Configure('" + full.Id + "');\"><i class='fa fa-gear fa-lg'></i></a></div></div>"
                                : "<div class='col-xs-3'><a title='You have not permission' disabled='disabled'  href='#'><i class='fa fa-ban fa-lg'></i></a></div></div>")
                        ));

                }
            }

        ]
    });
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

function Delete(id, descripcion) {

    var header = $("#modal_delete_header").val();
    var body = $("#modal_delete_body").val().replace("{0}", "<strong>" + descripcion + "</strong>");
    confirmar(header, body, function () {
        El(id);
    });
}

function El(id) {

    var url = $("#RedirectToDelete").val();
    var method = url;

    var fields = { id: id }
    sendValues(fields, doSuccessDelete, doErrorDelete, method);
}
var doSuccessDelete = function (result) {
    Search();
}
var doErrorDelete = function (result) {
}