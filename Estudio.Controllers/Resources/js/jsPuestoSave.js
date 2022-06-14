(function () {
    "use strict";

    //var regionId = $("#modelRegionId").val();
    
    GetPuestosFk();
  
    GetAreas();

    $(".chosen-select").chosen({ width: "95%" });

    $("#btnSave").click(function () {
        if ($('#myform').valid() && $("#lstArea").val() != null) {
            var fields =
            {
                PuestoId: (($('#_url').val().indexOf('Create') > -1) ? 0 : ($('#id').val())),
                Descripcion: $("#txtDescription").val().trim(),
                Horario: $("#lstHorario").val(),
                Objetivo: $("#txtObjetivo").val().trim(),
                PuestoFk: (($("#lstPuestoFk").val() === null) ? 0 : $("#lstPuestoFk").val()),
                AreaId: $("#lstArea").val()
            }
            sendValues(fields, doSuccessPuesto, doErrorPuesto);

        } else {
            $("html, body").animate({ scrollTop: 0 }, "slow");
            if (lstValidate===1)
                $('#lblDuplicados').show();
          
            if ($("#lstArea").val() === null) { $("#lblAreaRequerido").show(); }
           
            
        }
    });
    $("#lstArea").chosen().change(function () {
        $("#lblAreaRequerido").hide();
        GetResponsabilidadesByArea($(this).val());
    });

   

    $("#lstPuestoFk").chosen().change(function () {
        $('#lblJefeDirectoDuplicado').show();
       
    });
})();

function GetPuestosFk() {
    var url = $("#UrlGetPuestosFk").val();

    $.ajax({
        url: url,
        type: "GET",
        data: "{}"
    })
    .done(function (data) {
        $("#lstPuestoFk").html('<option value="0">[Opcional]</option>');
        $.each(data, function (index, value) {
            $("#lstPuestoFk").append('<option value="' + data[index].PuestoId + '">' + data[index].Descripcion + '</option>');
        });
     
        if ($('#_url').val().indexOf('Create') === -1) 
            $('select[name="lstPuestoFk"]').val($("#modelPuestoIdFk").val());
        

        $("#lstPuestoFk").trigger("chosen:updated");
        $("#lstPuestoFk").trigger("liszt:updated");
    })
    .fail(function (er) {
        console.log(er);
    })
}

function GetAreas() {
    var url = $("#UrlGetAreas").val();
    $.ajax({
        url: url,
        type: "GET",
        data: "{}"
    })
    .done(function (data) {
        $("#lstArea").html('');
        $.each(data, function (index, value) {
            $("#lstArea").append('<option value="' + data[index].AreaId + '">' + data[index].Descripcion + '</option>');
        });
        $('select[name="lstArea"]').val("-1");


        if ($('#_url').val().indexOf('Create') === -1) {
            $('select[name="lstArea"]').val($("#modelAreaId").val());
        }

        $("#lstArea").trigger("chosen:updated");
        $("#lstArea").trigger("liszt:updated");
        

        if ($('#_url').val().indexOf('Create') === -1)
            GetResponsabilidadesByArea($("#modelAreaId").val());
      
    })
    .fail(function (er) {
        console.log(er);
    })
}


function success() {
    $("#btnSave").hide();
 
   
    $('#txtDescription').attr('disabled', 'true');
    $('#txtHorario').attr('disabled', 'true');
    $('#txtObjetivo').attr('disabled', 'true');

    $('#lstPuestoFk').attr('disabled', true);
    $("#lstPuestoFk").trigger("chosen:updated");
    $("#lstPuestoFk").trigger("liszt:updated");

  
    $('#lstArea').attr('disabled', true);
    $("#lstArea").trigger("chosen:updated");
    $("#lstArea").trigger("liszt:updated");

  
    
    alertas("success", "La información ha sido modificada exitosamente");
}

function error(msg) {
    alertas("danger", "Error al ejecutar la transacción: " + msg);
}


var doSuccessPuesto = function (result) {
    $("#id").val(result.Object.PuestoId); success();
}

var doErrorPuesto = function (result) { error(result.Message); }
