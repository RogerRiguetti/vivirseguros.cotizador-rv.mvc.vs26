(function () {
    "use strict";

    if ($("#PuestosRegistro").val().toLowerCase() != "true") {
        $('#btnAdd').prop("disabled", "disabled");
        $('#btnAdd').css("cursor", "no-drop");
        $('#btnAdd').prop("title", "You have not permission");
    }

  //  GetRegion();
    Buscar();
    $(".chosen-select").chosen({ width: "95%" });

    $("#btnBuscar").click(function () {
        Buscar();
    });

    $("#btnAdd").click(function () {
        

        Create();


        //if ($("#lstRegion").val() != "0" && $("#lstRegion").val() != null)
        //    Create($("#lstRegion").val());
        //else
        //{

        //    $("html, body").animate({ scrollTop: 0 }, "slow");
        //    $("#lblCEDIS").show();
        //}
    });

    //$("#lstRegion").chosen().change(function () {
    //    $("#lblCEDIS").hide();
    //    GetDepartamentosByRegion($(this).val());
       
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
            "data":
                {                   
                    puesto: $("#txtDescription").val()
                }
        },
        "columns": [
                { "data": "Id", "autoWidth": true },                
                { "data": "Data1", "autoWidth": true },
                { "data": "Data2", "autoWidth": true },
                
                {
                    "mRender": function (data, type, full)
                    { 
                        return ($("#PuestosDetalles").val().toLowerCase() === "true" ?
                                "<a class=\"btn btn-block btn-primary\" href=\"javascript:Details('" + full.Id + "');\"><i class='fa fa-search'></i></a>"
                            :   "<a title='You have not permission' disabled='disabled' class=\"btn btn-block btn-primary\" href='#'><i class='fa fa-ban'></i></a>");
                    }
                }
                ,
                {
                    "mRender": function (data, type, full)
                    {
                        return ($("#PuestosEdicion").val().toLowerCase() === "true" ?
                                "<a class=\"btn btn-block btn-warning\" href=\"javascript:Edit('" + full.Id + "',0);\"><i class='fa fa-edit'></i></a>"
                            : ($("#Perfiles").val().toLowerCase() === "true" ? "<a class=\"btn btn-block btn-warning\" href=\"javascript:Edit('" + full.Id + "',1);\"><i class='fa fa-edit'></i></a>"
                                : "<a title='You have not permission' disabled='disabled' class=\"btn btn-block btn-warning\" href='#'><i class='fa fa-ban'></i></a>"));
                    }
                },
                {
                    "mRender": function (data, type, full)
                    {
                        return ($("#PuestosBaja").val().toLowerCase() === "true" ?
                                "<a class=\"btn btn-block btn-danger\" href=\"javascript:Delete('" + full.Id + "');\"><i class='fa fa-trash'></i></a>"
                            :   "<a title='You have not permission' disabled='disabled' class=\"btn btn-block btn-danger\" href='#'><i class='fa fa-ban'></i></a>");
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
    if (perfiles===1)
        url = $("#UrlPerfil").val();
    else
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




//function GetRegion() {

//    var regionId = $("#_SessionRegionId").val();
//    var url = $("#UrlGetRegion").val();

//    $.ajax({
//        url: url,
//        type: "GET",
//        data: "{}"
//    })
//    .done(function (data) {
//        $("#lstRegion").html('<option value="0" >Favor de selecccionar</option>');
//        $.each(data, function (index, value) {
//            $("#lstRegion").append('<option value="' + data[index].RegionId + '">' + data[index].Descripcion + '</option>');
//        });
//        $('select[name="lstRegion"]').val($("#_SessionRegionId").val());

//        if (regionId != "0") {
//            $("#lstRegion").prop('disabled', true);

//            $("#lstRegion").trigger("chosen:updated");
//            $("#lstRegion").trigger("liszt:updated");
//            GetDepartamentosByRegion(regionId);
//        } else {
//            Buscar();
//            $("#lstRegion").trigger("chosen:updated");
//            $("#lstRegion").trigger("liszt:updated");
//        }


        

//        //  $("#lstArea").attr("data-placeholder", "Area...");
//    })
//    .fail(function (er) {
//        console.log(er);
//    })
//}




//function GetDepartamentosByRegion(id) {
//    var url = $("#UrlGetDepartamentosByRegion").val();

//    $.ajax({
//        url: url,
//        type: "GET",
//        data: { regionId: id }
//    })
//    .done(function (data) {
//        $("#lstDepartamento").html('');
//        $.each(data, function (index, value) {
//            $("#lstDepartamento").append('<option value="' + data[index].Id + '">' + data[index].Value + '</option>');
//        });

        
//        $("#lstDepartamento").trigger("chosen:updated");
//        $("#lstDepartamento").trigger("liszt:updated");
        
//        Buscar();
//    })
//    .fail(function (er) {
//        console.log(er);
//    })
//}




var doSuccessDelete = function (result) {
    Buscar();
}
var doErrorDelete = function (result) {
    setTimeout(function () { aviso('Aviso', result.Message); }, 1200);
}