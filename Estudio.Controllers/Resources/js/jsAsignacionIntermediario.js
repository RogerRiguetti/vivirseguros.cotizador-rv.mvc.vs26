$(document).ready(function () {
    $('#tblAsignacionIntemediario').DataTable({
        language: { search: '', searchPlaceholder: "Buscar" }
    });

});


(function () {
    localStorage.pagina = 'SolicitudCotizacion';
    $("#btnCalcular").click(function () {
        confirmarCalculo("Aviso", "¿ Está seguro que desea Realizar el Cálculo Masivo?", function () { NuevoCAlculo(); });
    });
})();

function NuevoCAlculo() {
    var Intermediario = [];
    var dato;
    var table = $('#tblAsignacionIntemediario').DataTable();
    var tabla2 = table.data();
    var tabla = document.getElementById("tblAsignacionIntemediario");
    //var strNumCot = tabla2[0];
    //var intNumOpe = tabla2.getElementsByClassName("intNumOpe");
    //var strCussp = tabla2.getElementsByClassName("strCussp");
    //var strTipoDoc = tabla2.getElementsByClassName("strTipoDoc");
    //var strNom = tabla2.getElementsByClassName("strNom");
    //var strLugCita = tabla2.getElementsByClassName("strLugCita");
    //var strMonPro = tabla2.getElementsByClassName("strMonPro");

    //if (intNumOpe.length == 0) {
    //    aviso("Aviso", "No hay datos en la tabla para calcular");
    //    return;
    //}
    table.data().each(function (d) {
        var intNumOpe2 = d[1];
        var id = $("#" + intNumOpe2).val();

        //alert(id);tblAsignacionIntemediario
        dato = {
            strNumCot: d[0],
            intNumOpe: intNumOpe2,
            strCussp: d[2],
            strTipoDoc: d[3],
            strNom: d[5],
            strLugCita: d[6],
            strMonPro: d[7],
            idAsignado: id
        }
        Intermediario.push(dato);
    })

    //for (var i = 0; i < intNumOpe.length; i++) {
    //    var intNumOpe2 = intNumOpe[i].innerHTML;
    //    var id = $("#" + intNumOpe2).val();

    //    //alert(id);tblAsignacionIntemediario
    //    dato = {
    //        strNumCot: strNumCot[i].innerHTML,
    //        intNumOpe: intNumOpe2,
    //        strCussp: strCussp[i].innerHTML,
    //        strTipoDoc: strTipoDoc[i].innerHTML,
    //        strNom: strNom[i].innerHTML,
    //        strLugCita: strLugCita[i].innerHTML,
    //        strMonPro: strMonPro[i].innerHTML,
    //        idAsignado: id
    //    }
    //    Intermediario.push(dato);
    //}

    var url = $("#urlCalcularAsignacionIntermediario").val();

    var fields = {
        informacion: Intermediario,
        numArch: $("#Numero").val()
    };
    sendValues(fields, doSuccessCalcular, doError, url);
}


function doSuccessCalcular(result) {
    $('#modalcargar').modal('hide');
    modalConfirmacion("Aviso", result.Message, function () { vistaReportes(); });
}

function vistaReportes(){
    var urlRep = $("#urlReportes").val();
    var num = $("#Numero").val();
    window.location.href = urlRep + "?numArch=" + num;
}
function doError(result) {
    $('#modalcargar').modal('hide');
    aviso("Aviso", result.Message);
}
function confirmarCalculo(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
      
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
        $('#modalcargar').modal({
            drop: 'static',
            keyboard: false,
            show: true,
            backdrop: 'static'
        });
       

    });
}

function modalConfirmacion(header, body, fnaceptar) {
    $('#modal_aviso_confirm').text(header);
    $('#msg_modal_aviso_confirm').text(body);
    $('#modal_aviso_dos').modal('show');

    $("#btn_modal_aviso_confirm").one('click', function () {
        fnaceptar(); $('#modal_aviso_dos').modal('hide');
    });
}