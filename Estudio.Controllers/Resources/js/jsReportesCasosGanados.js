(function () {
    //initTable();


    $("#btnExportar").click(function () {
        var desde = $("#FecDesde").val();
        var hasta = $("#FecHasta").val();
        var paramRP = $('input:radio[name=rdParametrosRP]:checked').val();
        var paramRV = $('input:radio[name=rdParametrosRV]:checked').val()

        var url = $("#urlGenerarReporteGanados").val();
        var fields = {
            FechaDesde: desde,
            FechaHasta: hasta,
            parametroRV: paramRV,
            parametroRP: paramRP
        };
        sendValues(fields, doSuccessCrearExcel, doError, url);
    });
})();

function doSuccessCrearExcel(result) {
    var nombreArchivo = result.Object;
    var url = $("#urlGuardarExcel").val();

    window.open(url + "/?nombreArchivo=" + nombreArchivo);
}

function doError(result) {
    $('#modalcargar').modal('hide');
    aviso("ERROR", result.Message);
}

function aviso(header, body) {
    $('#msg_modal_header').text(header);

    $('#msg_modal_body').text(body);
    $('#sch_modal').modal('show');

    $("#btn_modal_aceptar").one('click', function () {
        $('#sch_modal').modal('hide');
    });
}