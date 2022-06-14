(function () {
    "use strict";


    $(".input-group.date").each(function (i, e) {
        $(this).datepicker({
            language: "es",
            format: 'dd/mm/yyyy',
            autoclose: true
        });
    });

    
    $("#btnSave").click(function () {


        if ($('#myform').valid() && $("#lstTipoServicio").val() != null) {
            
            var fields =
            {
                ServicioId: (($('#_url').val().indexOf('Create') > -1) ? 0 : ($('#id').val())),
                Descripcion: $("#txtDescription").val(),
                Fecha: $("#txtFecha").val(),
                TipoServicioId: $("#lstTipoServicio").val()
            }
            sendValues(fields);
        } else {
            $("html, body").animate({ scrollTop: 0 }, "slow");
            if ($("#lstTipoServicio").val() === null) { $("#lblServicioRequerido").show(); }
            
        }
    });

  

})();

//function GetTipoServicios() {
    
//    $.ajax({
//        url: $("#UrlGetTipoServicios").val(),
//        type: "GET",
//        data: "{}"
//    })
//    .done(function (data) {
//        $("#lstTipoServicio").html('');
//        $("#lstTipoServicio").append('<option value="" disabled selected>Tipo de servicio</option>');
//        $.each(data, function (index, value) {
          
//            $("#lstTipoServicio").append('<option value="' + data[index].TipoServicioId + '">' + data[index].Descripcion + '</option>');
//        });

//        if (!$('#_url').val().indexOf('Create') > -1) 
//            $('select[name="lstTipoServicio"]').val($("#modelTipoServicioId").val());
            
     
//        [].slice.call(document.querySelectorAll('select.cs-select')).forEach(function (el) {
//            new SelectFx(el);
//        });

//        //$("#lstTipoServicio").trigger("chosen:updated");
//        //$("#lstTipoServicio").trigger("liszt:updated");
//    })
//    .fail(function (er) {
//        console.log(er);
//    })
    
//}

function success() {
    $("#btnSave").hide();
    alertas("success", "La información ha sido modificada exitosamente");
    $('#txtDescription').attr('disabled', 'true');
    $('#lstTipoServicio').attr('disabled', true);
    $("#lstTipoServicio").trigger("chosen:updated");
    $("#lstTipoServicio").trigger("liszt:updated");
}

function error() {
    alertas("danger", "Error al ejecutar la transacción.");
}

