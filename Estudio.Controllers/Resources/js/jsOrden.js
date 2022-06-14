(function () {
    "use strict";

    GetRegion();

    $(".chosen-select").chosen({ width: "95%" });

     documentEvents();


})();



function GetRegion() {
    var regionId = $("#_SessionRegionId").val();

    $.ajax({
        url: $("#UrlGetRegion").val(),
        type: "GET",
        data: "{}"
    })
    .done(function (data) {
        $("#lstRegion").html('<option value="0" >[Opcional]</option>');
        $.each(data, function (index, value) {
            $("#lstRegion").append('<option value="' + data[index].RegionId + '">' + data[index].Descripcion + '</option>');
        });

        $('select[name="lstRegion"]').val($("#_SessionRegionId").val());

        if (regionId > "0") {
            $("#lstRegion").prop('disabled', true);
          
        }

        $("#lstRegion").trigger("chosen:updated");
        $("#lstRegion").trigger("liszt:updated");

        //Buscar();


    })
    .fail(function (er) {
        console.log(er);
    })
}



function Buscar() {
    var _url = $("#urlBuscar").val();

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
            "semail": "Ingrese un correo válido",
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
            "url": _url,
            "type": "GET",
            "datatype": "json",
            "data":
                {
                    regionId: ($("#lstRegion").val() == null ? -1 : $("#lstRegion").val()),
                    nombre: $("#txtNombre").val(),
                    departamentoId: (($("#lstDepartamento").val() == null) ? 0 : $("#lstDepartamento").val()),
                    puestoId: (($("#lstPuesto").val() == null) ? 0 : $("#lstPuesto").val()),
                    activo: (($("#lstEstatus").val() == null) ? 0 : $("#lstEstatus").val())
                }
        },
        "columns": [
                { "data": "UsuarioId", "autoWidth": true },
                { "data": "Region", "autoWidth": true },
                { "data": "NombreCompleto", "autoWidth": true },
                { "data": "Usuario", "autoWidth": true },
                { "data": "Rol", "autoWidth": true },
                { "data": "Estatus", "autoWidth": true }



                ,

                    {
                        "mRender": function (data, type, full) {

                            return ($("#UsuariosDetalles").val().toLowerCase() === "true" ?
                            ("<div class='row'><div class='col-xs-3'><a " + ((full.Estatus === "-") ? "disabled='disabled'" : "") + "  " + ((full.Estatus === "-") ? "" : "href=\"javascript:Details('" + full.UsuarioId + "');\"") + "><i class='fa fa-search fa-lg'></i></a></div>"

                            +
                             ($("#UsuariosEdicion").val().toLowerCase() === "true" ?
                                "<div class='col-xs-3'><a " + ((full.Estatus === "-" || full.Estatus === "Inactivo") ? "disabled='disabled'" : "") + "  " + ((full.Estatus === "-" || full.Estatus === "Inactivo") ? "" : "href=\"javascript:Edit('" + full.UsuarioId + "');\"") + "><i class='fa fa-edit fa-lg'></i></a></div>"
                            : "<div class='col-xs-3'><a title='You have not permission' disabled='disabled'  " + ((full.Estatus === "-" || full.Estatus === "Inactivo") ? "" : "href='#'") + "><i class='fa fa-ban fa-lg'></i></a></div>")



                            +

                            ($("#UsuariosBaja").val().toLowerCase() === "true" ?
                               "<div class='col-xs-3'><a " + ((full.Estatus === "-" || full.Estatus === "Inactivo") ? "disabled='disabled'" : "") + "  " + ((full.Estatus === "-" || full.Estatus === "Inactivo") ? "" : "href=\"javascript:Delete('" + full.UsuarioId + "');\"") + "><i class='fa fa-trash fa-lg'></i></a></div>"
                            : "<div class='col-xs-3'><a title='You have not permission' disabled='disabled' class=\"btn btn-block btn-danger\" " + ((full.Estatus === "-" || full.Estatus === "Inactivo") ? "" : "href='#'") + "><i class='fa fa-ban fa-lg'></i></a></div>")

                            +

                            ($("#UsuariosRegistro").val().toLowerCase() === "true" ?
                            "<div class='col-xs-3'><a " + ((full.Estatus != "-") ? "disabled='disabled'" : "") + "  " + ((full.Estatus != "-") ? "" : "href=\"javascript:Create('" + full.UsuarioId + "');\"") + "><i class='fa fa-plus fa-lg'></i></a></div></div>"
                        : "<div class='col-xs-3'><a title='You have not permission' disabled='disabled'  " + ((full.Estatus != "-") ? "" : "href='#'") + "><i class='fa fa-ban fa-lg'></i></a></div></div>"))

                            :
                            ("<div class='row'><div class='col-xs-3'><a title='You have not permission' disabled='disabled'  " + ((full.Estatus === "-") ? "" : "href='#'") + "><i class='fa fa-ban fa-lg'></i></a></div>"

                            +

                              ($("#UsuariosEdicion").val().toLowerCase() === "true" ?
                                "<div class='col-xs-3'><a " + ((full.Estatus === "-" || full.Estatus === "Inactivo") ? "disabled='disabled'" : "") + "  " + ((full.Estatus === "-" || full.Estatus === "Inactivo") ? "" : "href=\"javascript:Edit('" + full.UsuarioId + "');\"") + "><i class='fa fa-edit fa-lg'></i></a></div>"
                            : "<div class='col-xs-3'><a title='You have not permission' disabled='disabled'  " + ((full.Estatus === "-" || full.Estatus === "Inactivo") ? "" : "href='#'") + "><i class='fa fa-ban fa-lg'></i></a></div>")

                             +

                            ($("#UsuariosBaja").val().toLowerCase() === "true" ?
                               "<div class='col-xs-3'><a " + ((full.Estatus === "-" || full.Estatus === "Inactivo") ? "disabled='disabled'" : "") + "  " + ((full.Estatus === "-" || full.Estatus === "Inactivo") ? "" : "href=\"javascript:Delete('" + full.UsuarioId + "');\"") + "><i class='fa fa-trash fa-lg'></i></a></div>"
                            : "<div class='col-xs-3'><a title='You have not permission' disabled='disabled'  " + ((full.Estatus === "-" || full.Estatus === "Inactivo") ? "" : "href='#'") + "><i class='fa fa-ban fa-lg'></i></a></div>")

                            +

                            ($("#UsuariosRegistro").val().toLowerCase() === "true" ?
                            "<div class='col-xs-3'><a " + ((full.Estatus != "-") ? "disabled='disabled'" : "") + "  " + ((full.Estatus != "-") ? "" : "href=\"javascript:Create('" + full.UsuarioId + "');\"") + "><i class='fa fa-plus fa-lg'></i></a></div></div>"
                        : "<div class='col-xs-3'><a title='You have not permission' disabled='disabled'  " + ((full.Estatus != "-") ? "" : "href='#'") + "><i class='fa fa-ban fa-lg'></i></a></div></div>")));










                        }
                    }
                    /*
                ,
                {
                    "mRender": function (data, type, full)
                    {
                        return ($("#UsuariosEdicion").val().toLowerCase() === "true" ?
                                "<a " + ((full.Data4 === "-" || full.Data4 === "Inactivo") ? "disabled='disabled'" : "") + " class=\"btn btn-block btn-warning\" " + ((full.Data4 === "-" || full.Data4 === "Inactivo") ? "" : "href=\"javascript:Edit('" + full.Id + "');\"") + "><i class='fa fa-edit'></i></a>"
                            :   "<a title='You have not permission' disabled='disabled' class=\"btn btn-block btn-warning\" " + ((full.Data4 === "-" || full.Data4 === "Inactivo") ? "" : "href='#'") + "><i class='fa fa-ban'></i></a>");
                    }
                },
                {
                    "mRender": function (data, type, full)
                    {
                        return ($("#UsuariosBaja").val().toLowerCase() === "true" ?
                               "<a " + ((full.Data4 === "-" || full.Data4 === "Inactivo") ? "disabled='disabled'" : "") + " class=\"btn btn-block btn-danger\" " + ((full.Data4 === "-" || full.Data4 === "Inactivo") ? "" : "href=\"javascript:Delete('" + full.Id + "');\"") + "><i class='fa fa-trash'></i></a>"
                            : "<a title='You have not permission' disabled='disabled' class=\"btn btn-block btn-danger\" " + ((full.Data4 === "-" || full.Data4 === "Inactivo") ? "" : "href='#'") + "><i class='fa fa-ban'></i></a>");
                    }
                },
                {
                    "mRender": function (data, type, full)
                    {
                        return ($("#UsuariosRegistro").val().toLowerCase() === "true" ?
                            "<a " + ((full.Data4 != "-") ? "disabled='disabled'" : "") + " class=\"btn btn-block btn-success\" " + ((full.Data4 != "-") ? "" : "href=\"javascript:Create('" + full.Id + "');\"") + "><i class='fa fa-plus'></i></a>"
                        : "<a title='You have not permission' disabled='disabled' class=\"btn btn-block btn-success\" " + ((full.Data4 != "-") ? "" : "href='#'") + "><i class='fa fa-ban'></i></a>");
                    }
                }
                */
        ]
    });
}


function Details(id) {
    var _url = $("#RedirectToDetails").val();
    window.location.href = _url + "/" + id;
}

function Edit(id) {
    var _url = $("#RedirectToEdit").val();
    window.location.href = _url + "/" + id;
}

function Delete(id) {
    var header = $("#modal_delete_header").val();
    var body = $("#modal_delete_body").val();
    confirmar(header, body, function () { Eliminar(id); });
}
function Create(id) {
    var _url = $("#RedirectToCreate").val();

    window.location.href = _url + "/" + id;
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


// Eventos adicionales para la carga de documentos
function documentEvents() {
    var d = new Date();
    

    $(".input-group.date.dateFechaIni").datepicker({
        language: "es",
        format: "dd/mm/yyyy",
        autoclose: true
    }).on('changeDate', function (e) {
        $(".input-group.date.dateFechaFin").datepicker('setStartDate', e.date);
    });

    $(".input-group.date.dateFechaFin").datepicker({
        language: "es",
        format: "dd/mm/yyyy",
        autoclose: true
    }).on('changeDate', function (e) {
        $(".input-group.date.dateFechaIni").datepicker('setEndDate', e.date);
    });
}