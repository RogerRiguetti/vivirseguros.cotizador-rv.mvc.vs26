(function () {
    "use strict";

    //  GetAlmacen();
    Buscar();
    $(".chosen-select").chosen({ width: "95%" });

    
    //$("#btnAdd").click(function () {


    //    Create();


    //});

    

})();


function Buscar() {
    var url = $("#UrlBuscar").val();
    $("#tblCatalogo").DataTable({
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        },
        "bDestroy": true,
        "sAjaxDataProp": "data",
        "ajax": {
            "url": url,
            "type": "GET",
            "datatype": "json",
            "data": "{}"
        },
        "columns": [
                { "data": "ProyectoId", "autoWidth": true },
                { "data": "Descripcion", "autoWidth": true },
                { "data": "Duracion", "autoWidth": true },

                {
                    "mRender": function (data, type, full) {
                        return ($("#ProyectosDetalles").val().toLowerCase() === "true" ?
                                "<a class=\"btn btn-block btn-primary ctrlgray\" href=\"javascript:Details('" + full.ProyectoId + "');\"><i class='fa fa-search'></i></a>"
                            : "<a title='You have not permission' disabled='disabled' class=\"btn btn-block btn-primary ctrlgray\" href='#'><i class='fa fa-ban'></i></a>");
                    }
                }
                ,
                {
                    "mRender": function (data, type, full) {
                        return ($("#ProyectosEdicion").val().toLowerCase() === "true" ?
                                "<a class=\"btn btn-block btn-warning ctrlgray\" href=\"javascript:Edit('" + full.ProyectoId + "',0);\"><i class='fa fa-edit'></i></a>"
                                : "<a title='You have not permission' disabled='disabled' class=\"btn btn-block btn-warning ctrlgray\" href='#'><i class='fa fa-ban'></i></a>");
                    }
                },
                {
                    "mRender": function (data, type, full) {
                        return ($("#ProyectosBaja").val().toLowerCase() === "true" ?
                                "<a class=\"btn btn-block btn-danger ctrlgray\" href=\"javascript:Delete('" + full.ProyectoId + "');\"><i class='fa fa-trash'></i></a>"
                            : "<a title='You have not permission' disabled='disabled' class=\"btn btn-block btn-danger ctrlgray\" href='#'><i class='fa fa-ban'></i></a>");
                    }
                }

        ]
    });
}

function Details(id) {
    var url = $("#RedirectToDetails").val();
    window.location.href = url + "/" + id;


}

function Edit(id, perfiles) {
    var url;
    
    url = $("#RedirectToEdit").val();

    window.location.href = url + "/" + id;

}

function Delete(id) {
    var header = $("#modal_delete_header").val();
    var body = $("#modal_delete_body").val();
    confirmar(header, body, function () { Eliminar(id); });
}
function Create() {
    var url = $("#RedirectToCreate").val();
    window.location.href = url;
}


function Eliminar(id) {
    var method = $("#RedirectToDelete").val();
    var fields = { id: id }
    sendValues(fields, doSuccessDelete, doErrorDelete, method);
}





var doSuccessDelete = function (result) {
    Buscar();
}
var doErrorDelete = function (result) {

}