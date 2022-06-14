
var CodCuspp, anosRT, Modalidad, PerGar, Moneda, porcentajeRVD, CobCony, Gratificacion, DerCre, FecSolicitud, FecEnvio, MtoPension, PrcTasaVta, CodEstCot, TipRen, MtoPrima, FecAcepta;
var Num_MesRtaEsc, Prc_RtaEsc, strNumOpe, strNumCot, strNumCorr, FecCierre;

(function () {
    localStorage.pagina = 'Mantenedor';
})();

function BuscarArchivo() {

    if ($('#txtBCUSPP').val() == "" && $('#txtNumOperacion').val() == "" && $('#txtNumCot').val() == "" && $('#txtNumCor').val() == "") {
        aviso("Error", "Debes de llenar alguno de los campos de búsqueda.");
        return;
    }

    if ($('#txtNumCor').val() != "" && $('#txtBCUSPP').val() == "" && $('#txtNumOperacion').val() == "" && $('#txtNumCot').val() == "") {
        aviso("Error", "Debes de llenar un campo más de búsqueda");
        return;
    }

    var urlBuscarArchivo = $("#urlBuscarArchivo").val();
    var fields = {
        cuspp: $('#txtBCUSPP').val(),
        NumOperacion: $('#txtNumOperacion').val(),
        NumCotizacion: $('#txtNumCot').val(),
        NumCorrelativo: $('#txtNumCor').val(),
    };
    sendValues(fields, doSuccessBuscar, doError, urlBuscarArchivo);
}

function doSuccessBuscar(result) {
    var datos = result.Object.toString();
    var array = (datos).split(",");
    CodCuspp = array[0].toString();
    anosRT = array[1].toString();
    Modalidad = array[2].toString();
    PerGar = array[3].toString();
    Moneda = array[4].toString();
    porcentajeRVD = array[5].toString();
    CobCony = array[6].toString();
    Gratificacion = array[7].toString();
    DerCre = array[8].toString();
    FecSolicitud = array[9].toString();
    FecEnvio = array[10].toString();

    if (array[18].toString() == "08") {
        MtoPension = array[17].toString();
    } else {
        MtoPension = array[13].toString();
    }

    PrcTasaVta = array[14].toString();
    CodEstCot = array[11].toString();
    TipRen = array[15].toString();
    MtoPrima = array[12].toString();
    FecAcepta = array[19].toString();
    Num_MesRtaEsc = array[20].toString();
    Prc_RtaEsc = array[21].toString();

    strNumOpe = array[22].toString();
    strNumCot = array[24].toString();
    strNumCorr = array[23].toString();

    $('#txtCUSPP').val(CodCuspp);
    $('#txtMododalidad').val(Modalidad);
    $('#txtMtoPrima').val(MtoPrima);
    $('#txtMtoPension').val(MtoPension);
    $('#txtModena').val(Moneda);
    $('#txtCoberConyuge').val(CobCony);
    $('#txtDerCre').val(DerCre);
    $('#txtGratificacion').val(Gratificacion);

    if (FecAcepta != "") {
        var tem = FecAcepta, ano, mes, dia;
        ano = tem.substring(0, 4);
        mes = tem.substring(4, 6);
        dia = tem.substring(6, 8);
        tem = dia + "/" + mes + "/" + ano;
        $('#txtFecAceptacionAct').val(tem);
    } else {
        $('#txtFecAceptacionAct').val(FecAcepta);
    }

    $('#btnBuscar').removeAttr('onclick');
    $('#btnBuscar').css('cursor', 'not-allowed');
    document.getElementById("txtNumOperacion").disabled = true;
    document.getElementById("txtNumCot").disabled = true;
    document.getElementById("txtNumCor").disabled = true;
    document.getElementById("txtFecAceptacionMod").disabled = false;

    //Validaciones CodEstCot
    if (CodEstCot == "N" || CodEstCot == "C" || CodEstCot == "S" || CodEstCot == "M") {
        aviso("Aviso", "Número de Solicitud de Oferta No Ha sido Enviada a Meler");
    }
    if (CodEstCot == "E") {
        aviso("Aviso", "Número de Solicitud de Oferta Se Encuentra Enviada a Meler");
    }
    if (CodEstCot == "P") {
        aviso("Aviso", "Número de Solicitud de Oferta Se Encuentra en Producción");
    }

    //solicitudesC = array[1].toString();
    //solicitudesE = array[2].toString();
    //aviso("AVISO", result.Message);
}
function doError(result) {
    aviso("ERROR", result.Message);
}

function LimpiarCampos() {
    //limpia los campos llenados
    $('#txtNumOperacion').val("");
    $('#txtNumCot').val("");
    $('#txtNumCor').val("");
    $('#txtCUSPP').val("");
    $('#txtMododalidad').val("");
    $('#txtMtoPrima').val("");
    $('#txtMtoPension').val("");
    $('#txtModena').val("");
    $('#txtCoberConyuge').val("");
    $('#txtDerCre').val("");
    $('#txtGratificacion').val("");
    $('#txtFecAceptacionAct').val("");
    $('#txtBCUSPP').val("");
    //Habilitar y desabilitar campos
    document.getElementById("txtNumOperacion").disabled = false;
    document.getElementById("txtNumCot").disabled = false;
    document.getElementById("txtNumCor").disabled = false;
    document.getElementById("txtFecAceptacionMod").disabled = true;

    //Agregar probliedad onclick y cambiar cursor
    $('#btnBuscar').attr('onClick', 'BuscarArchivo();');
    $('#btnBuscar').css('cursor', 'pointer');

    //Cambiar fecha actual por si se modifico
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var today = yyyy + '-' + mm + '-' + dd;
    $('#txtFecAceptacionMod').val(today);
}

function Aceptar() {
    //Valida que la solicitud haya sido enviada al Meler
    if (CodEstCot == "N" || CodEstCot == "C" || CodEstCot == "S" || CodEstCot == "M") {
        aviso("Aviso", "Número de Solicitud de Oferta No Ha sido Enviada a Meler. \n\nOperación Cancelada.");
        return;
    }
    if (CodEstCot == "E") {
        aviso("Aviso", "Número de Solicitud de Oferta Se Encuentra Enviada a Meler. \n\nOperación Cancelada.");
        return;
    }
    if (CodEstCot == "P") {
        aviso("Aviso", "Número de Solicitud de Oferta Se Encuentra en Producción. \n\nOperación Cancelada.");
        return;
    }

    //Valida que se haya seleccionado una solicitud para modificar la fecha de aceptación
    /*if ($('#txtNumOperacion').val() != "" && $('#txtNumCot').val() != "" && $('#txtNumCor').val() != "") {
        strNumOpe = $('#txtNumOperacion').val();
        strNumCot = $('#txtNumCot').val();
        strNumCorr = $('#txtNumCor').val();
    } else {
        aviso("Aviso", "Falta Información Para realizar el Proceso de Aceptación de la Cotización. \n\nOperación Cancelada.");
        return;
    }*/

    //Valida que se haya ingresado la Fecha de Aceptación a Modificar de la Solicitud
    if ($('#txtFecAceptacionMod').val() == "") {
        aviso("Aviso", "Falta la Fecha de Aceptación para realizar el Proceso. \n\nOperación Cancelada.");
        return;
    }

    //Confirma que se desea modificar la Fecha de Aceptación
    if (CodEstCot == "A") {
        var temp, ano, mes, dia;
        temp = $('#txtFecAceptacionMod').val();
        dia = temp.substring(8, 10);
        mes = temp.substring(5, 7);
        ano = temp.substring(0, 4);
        FecCierre = ano + "" + mes + "" + dia;
        confirmarCalculo("Aviso", "¿ Está seguro que desea Realizar el Proceso de Aceptación al día " + $('#txtFecAceptacionMod').val() + " ?", function () { flModFechaAcepta(); });
    }
}

function confirmarCalculo(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}

function flModFechaAcepta() {
    var urlActualizarArchivo = $("#urlActualizarArchivo").val();
    var fields = {
        NumOperacion: strNumOpe,
        NumCotizacion: strNumCot,
        NumCorrelativo: strNumCorr,
        FecCierre: FecCierre,
    };
    sendValues(fields, doSuccessActualizar, doError, urlActualizarArchivo);
}

function doSuccessActualizar(result) {
    alert("El Proceso de Actualización ha terminado Satisfactoriamente");
}

function salir() {
    var url = $("#urlSalir").val();
    window.location.href = url;
}