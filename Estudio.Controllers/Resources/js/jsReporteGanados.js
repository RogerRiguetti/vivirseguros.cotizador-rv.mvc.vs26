(function () {
    initTable();

    //$("#bucketTrackEfic").dropdownchecklist({ firstItemChecksAll: true, maxDropHeight: 150 });
    //$("#bucketTrackEfic").dropdownchecklist({ firstItemChecksAll: true, explicitClose: '...close' });
    $('select[multiple]').multiselect({
        allSelectedText: 'TODOS',
        maxHeight: 200,
        includeSelectAllOption: true,
        selectAllText: "TODOS",
        nonSelectedText: "NINGUNO SELECCIONADO",
        buttonWidth: '100%'

    });

    $("#btnSearch").click(function () {
        var departamento = $("#cboDepartamento").val();
        var decisionAfiliado = $("#cboDecisionAfiliado").val();
        var desde = $("#txtDesde").val();
        var hasta = $("#txtHasta").val();
        var fechaDesde = $("#txtFechaDesde").val();
        var fechaHasta = $("#txtFechaHasta").val();
        var moneda = $("#cboMoneda").val();
        var prestacion = $("#cboPrestacion").val();
        var modalidad = $("#cboModalidad").val();
        var cotiza = $("#cboCotiza").val();
        var gana = $("#cboGana").val();

        fechaDesde = fechaDesde.replace(/^(\d{4})-(\d{2})-(\d{2})$/g, '$1$2$3');
        fechaHasta = fechaHasta.replace(/^(\d{4})-(\d{2})-(\d{2})$/g, '$1$2$3');
        //if (departamento == "" || decisionAfiliado == "" || desde == "" || hasta == "" || fecha == "" ||
        //    moneda == "" || prestacion == "" || modalidad == "" || cotiza == "" || gana == "") {
        //    aviso("Aviso", "Favor de capturar todos los filtros de Búsqueda");

        //    return;
        //}
        var url = $("#urlBusquedaInformacion").val();
        var fields = {
            Departamento: departamento,
            DecisionAfiliado: decisionAfiliado,
            Desde: desde,
            Hasta: hasta,
            FechaDesde: fechaDesde,
            FechaHasta: fechaHasta,
            Moneda: moneda,
            Prestacion: prestacion,
            Modalidad: modalidad,
            Cotiza: cotiza,
            Gana: gana
        };
        sendValues(fields, doSuccessBuscarInformacion, doError, url);
    });

    $("#btnExportar").click(function () {
        var departamento = $("#cboDepartamento").val();
        var decisionAfiliado = $("#cboDecisionAfiliado").val();
        var desde = $("#txtDesde").val();
        var hasta = $("#txtHasta").val();
        var fechaDesde = $("#txtFechaDesde").val();
        var fechaHasta = $("#txtFechaHasta").val();
        var moneda = $("#cboMoneda").val();
        var prestacion = $("#cboPrestacion").val();
        var modalidad = $("#cboModalidad").val();
        var cotiza = $("#cboCotiza").val();
        var gana = $("#cboGana").val();


        fechaDesde = fechaDesde.replace(/^(\d{4})-(\d{2})-(\d{2})$/g, '$1$2$3');
        fechaHasta = fechaHasta.replace(/^(\d{4})-(\d{2})-(\d{2})$/g, '$1$2$3');
        //if (departamento == "" || decisionAfiliado == "" || desde == "" || hasta == "" || fecha == "" ||
        //    moneda == "" || prestacion == "" || modalidad == "" || cotiza == "" || gana == "") {
        //    aviso("Aviso", "Favor de capturar todos los filtros de Búsqueda");

        //    return;
        //}
        var url = $("#urlCrearExcel").val();
        var fields = {
            Departamento: departamento,
            DecisionAfiliado: decisionAfiliado,
            Desde: desde,
            Hasta: hasta,
            FechaDesde: fechaDesde,
            FechaHasta: fechaHasta,
            Moneda: moneda,
            Prestacion: prestacion,
            Modalidad: modalidad,
            Cotiza: cotiza,
            Gana: gana
        };
        sendValues(fields, doSuccessCrearExcel, doError, url);
    });
})();

function doSuccessCrearExcel(result) {
    var nombreArchivo = result.Object;
    var url = $("#urlGuardarExcel").val();

    window.open(url + "/?nombreArchivo=" + nombreArchivo);
}

function initTable() {

    $("#tblConsulta").DataTable({
        "order": [[ 3, "desc" ]],
        "scrollY": "auto",
        //"scrollCollapse": true,
        "scrollX": true,
        "bAutoWidth": true,
        "language": {
            //"sProcessing": "Procesando...",
            //"sLengthMenu": "Mostrar _MENU_ registros",
            //"sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "No se encontró ningún dato disponible en esta tabla",
            //"sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            //"sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            //"sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "",
            "sUrl": "",
            "sInfoThousands": ",",
            "searchPlaceholder": "Buscar"
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
        "dom": '<lf<t><"col-lg-6 col-md-6 col-sm-12"i>p>'
    });

    var table = $('#tblConsulta').DataTable();
    table.columns.adjust().draw();
}

function doSuccessBuscarInformacion(result) {
    var ganadosCompania = result.Object.GanadosCompania;
    var totalNumeroCotizado = result.Object.Totales.TotalNumeroCotizado;
    var totalPension = result.Object.Totales.TotalPension;
    var totalTasaVenta = result.Object.Totales.TotalTasaVenta;

    $("#tblConsulta").DataTable().destroy();
    var $body = $("#tblConsulta tbody");
    $body.empty();
    var celdaColor;
    ganadosCompania.forEach(function (v) {
        var tr = $("<tr style='background-color: " + v.color + ";'>'");
        var td = $('<td>');

        $("<td style='text-align: center;'><span class='dot' style='background-color: " + v.color + "'></span></td>").appendTo(tr);
        $("<td>").html(v.CompañiaSegurosVitalicios).appendTo(tr);
        $("<td>").html(v.NumeroCotizado).appendTo(tr);
        $("<td>").html(v.Pension).appendTo(tr);
        $("<td>").html(v.TasaVenta).appendTo(tr);
        $("<td>").html(v.DiferenciaPension).appendTo(tr);
        $("<td>").html(v.DiferenciaTasaVenta).appendTo(tr);
        $("<td>").html(v.NumeroCasosGanados).appendTo(tr);
        $("<td>").html(v.NumeroCasosTotales).appendTo(tr);
        $("<td>").html(v.ParticipacionMercado).appendTo(tr);

        $body.append(tr)
    });

    var $foot = $("#tblConsulta tfoot");
    $foot.empty();
    var tr = $('<tr>');
    var td = $('<td>');

    $("<td>").html('').appendTo(tr);
    $("<td>").html('TOTALES').appendTo(tr);
    $("<td>").html(totalNumeroCotizado).appendTo(tr);
    $("<td>").html(totalPension).appendTo(tr);
    $("<td>").html(totalTasaVenta).appendTo(tr);
    $("<td>").html('').appendTo(tr);
    $("<td>").html('').appendTo(tr);
    $("<td>").html('').appendTo(tr);
    $("<td>").html('').appendTo(tr);
    $("<td>").html('').appendTo(tr);

    $foot.append(tr)

    // $("#totalNumeroCotizado").val(totalNumeroCotizado);
    //$("#totalNumeroCotizado").val(totalPension);
    //$("#totalNumeroCotizado").val(totalTasaVenta);

    initTable();
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