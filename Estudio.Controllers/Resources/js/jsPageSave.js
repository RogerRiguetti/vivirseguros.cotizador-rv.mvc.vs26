(function () {
    "use strict";

    $("#btnSave").click(function () {

        if ($('#myform').valid()) {
            var fields =
                {
                    Id: (($('#url').val().indexOf('Create') > -1) ? 0 : ($('#id').val())),
                    Description: $("#txtDescription").val(),
                    ModuleId: $("#lstModule").val()
                }
            sendValues(fields);
        } else {
            $("html, body").animate({ scrollTop: 0 }, "slow");
        }
    });

    //For class selectFx
    //var lstSystem = document.getElementById("lstSystem");
    //new SelectFx(lstSystem, {
    //    onChange: function (val) {
    //        $("#lblSystemRequired").hide();
    //        GetModulesBySystem(val);
    //        void (0);
    //    }
    //});
    //var lstModule = document.getElementById("lstModule");
    //new SelectFx(lstModule);


   


    $(".chosen-select").chosen({ width: "95%" });

 
    $("#lstSystem").chosen().change(function () {
        $("#lblSystemRequired").hide();
        GetModulesBySystem($(this).val());
    });


    if ($('#url').val().indexOf('Edit') > -1)   {
        GetModulesBySystem($("#modelSystemId").val());         
        setTimeout(function () {
            $('select[name="lstModule"]').val($("#modelModuleId").val());
            $("#lstModule").trigger("chosen:updated");
            $("#lstModule").trigger("liszt:updated");
        }, 500);
    }
 
   

})();


function checkRequired(obj) {
    if (obj.checked)
        $("#lblRequired").hide();
}


function GetModulesBySystem(id) {
    var _url = $("#UrlGetModulesBySystem").val();
   
  
    $.ajax({
        url: _url,
        type: "GET",
        data: { systemId: id }
    })
        .done(function (data) {

            //class SelectFx
            //$("#lstModule").html('<option selected="selected" disabled="disabled">Modules...</option>');            
            $("#lstModule").html('');            
            $.each(data, function (index, value) {
               
                $("#lstModule").append('<option value="' + data[index].Id + '">' + data[index].Description + '</option>');
            });

  
            $("#lstModule").trigger("chosen:updated");
            $("#lstModule").trigger("liszt:updated");
        })
        .fail(function (er) {
            console.log(er);
        })
}





function success() {
    $("#btnSave").hide();
    alertas("success", "Success");
    $('#txtDescription').attr('disabled', 'true');
}

function error() {
    alertas("danger", "Error");
}