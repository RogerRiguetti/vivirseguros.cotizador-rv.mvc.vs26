var banderaBen = 'C';
var banderaMod = 'C';
var banderaCot = 'C';

var idBeneficiario = 0;
var idModalidad = 0;
var idCotizacion = 0;

var idsBeneficiarios = [];
var idsModalidades = [];

var longitudDoc = 0;
var indicadorLongDoc = 0;

var idProvincia = 0;
var idDistrito = 0;
var valAfp;
var valPension;

var idTitular = 0;
var fecha;

var cargaCombosVal = true;
var idAniosRent = 0;
var idPorRent = 0;
var idAniosGar = 0;
var idPrimerT = 0;
var idSegundoT = 0;
var banderaReg = false;
var banderaRegM = false;

(function () {
    localStorage.pagina = 'Cotizacion';
})();


$("#txtFechaNacimientoCot").mask("00/00/0000", { placeholder: "dd/mm/aaaa" });
$("#txtFechaNacimientoBen").mask("00/00/0000", { placeholder: "dd/mm/aaaa" });
$("#txtDevSolCot").mask("00/00/0000", { placeholder: "dd/mm/aaaa" });
$("#txtFechaEstudioCot").mask("00/00/0000", { placeholder: "dd/mm/aaaa" });
$("#txtFechaInvalidezBen").mask("00/00/0000", { placeholder: "dd/mm/aaaa" });
$("#txtFechaFallecimientoBen").mask("00/00/0000", { placeholder: "dd/mm/aaaa" });
$("#txtFechaInvalidezCot").mask("00/00/0000", { placeholder: "dd/mm/aaaa" });
$("#txtFechaFallecimientoCot").mask("00/00/0000", { placeholder: "dd/mm/aaaa" });

$(document).ready(function () {
    "use strict";
    document.getElementById("cmbxTiposDocumentoBus").focus();
    document.getElementById("btnReporteT").style.display = "none";
    document.getElementById("btnReporte").style.display = "none";
    var hoy = new Date();
    var dd = hoy.getDate();
    var mm = hoy.getMonth() + 1;
    var yyyy = hoy.getFullYear();

    if (dd < 10)
        dd = '0' + dd

    if (mm < 10)
        mm = '0' + mm

    hoy = dd + '/' + mm + '/' + yyyy;

    $("#txtCuspp").autocomplete({
        source: "ConsultaCUSPP",
        minLength: 5,

    });

    $("#txtFechaEstudioCot").val(hoy);

    $("#cmbxPensionesCot").change(function () {
        if ($("#cmbxPensionesCot option:selected").text() == "SOBREVIVENCIA") {
            $('#txtFechaFallecimientoCot').val($("#txtDevSolCot").val());
        }
        else {
            $('#txtFechaFallecimientoCot').attr("disabled", true);
            $('#txtFechaFallecimientoCot').val("");
        }

        if ($("#cmbxPensionesCot option:selected").text() == "INVALIDEZ PARCIAL" || $("#cmbxPensionesCot option:selected").text() == "INVALIDEZ TOTAL") {
            var valorSeleccionado = $("#cmbxPensionesCot option:selected").text();
            $('#txtFechaInvalidezCot').val(hoy);
        }
        else {
            $('#txtFechaInvalidezCot').attr("disabled", true);
            $('#txtFechaInvalidezCot').val("");
        }

        // Cambio de porcentajes
        if (idsBeneficiarios.length > 0) {
            var url = $("#urlPorcentajesBen").val();
            var idPension = $("#cmbxPensionesCot").val();
            var fechaFallecimiento = $('#txtFechaFallecimientoCot').val();
            var FechaDev = $("#txtDevSolCot").val();
            var fields = {
                idsBeneficiarios: idsBeneficiarios,
                idPension: idPension,
                fechaFallecimiento: fechaFallecimiento,
                FecDev: FechaDev
            };

            sendValues(fields, doSuccessPorcentajesBen, doErrorPorcentajesBen, url);
     }
    });

    $("#cmbxModalidadesMod").change(function () {
        if ($("#cmbxModalidadesMod option:selected").text() == "GARANTIZADA") {
            $('#cmbxAniosGarantizadosMod').attr("disabled", false);
            $('#cmbxAniosGarantizadosMod').val("15");
        }
        else {
            $('#cmbxAniosGarantizadosMod').attr("disabled", true);
            $('#cmbxAniosGarantizadosMod').val("0");
        }

    });
    
    $("#cmbxTiposRentaMod").change(function () {
     //Validaciones modalidades
        var url = $("#urlConsultarVM").val();
        var idPension = $("#cmbxPensionesCot").val();
        var idRenta = $("#cmbxTiposRentaMod").val();
        var idMoneda = $("#cmbxMonedasMod").val();

        if (idPension != "" && idRenta != "" && idMoneda != "") {
            var fields = {
                idPension: idPension,
                idRenta: idRenta,
                idMoneda: idMoneda
            };

            sendValues(fields, doSuccessConsultaValidacionesMod, doErrorConsultaValidacionesMod, url);
        }
    });

    $("#btnAgregarPaqMod").click(function () {
        var idPaquete = $("#cmbxPaquetesMod").val();
        var TipoAfp = $("#cmbxAfpCot option:selected").text();

        if (idPaquete == "")
            aviso("Aviso", "Debe seleccionar un paquete");
        else {
            var url = $("#urlRegistrarPaq").val();
            var fields = {
                idPaquete: idPaquete,
                TipoAfp: TipoAfp
            };
            sendValues(fields, doSuccessRegistrarPaq, doErrorRegistrarPaq, url);
        }
    });

    $("#btnCalcularPension").click(function () {
        validaCotizacion();

    });

    $("#btnReporteT").click(function () {

        Reporte(idCotizacion);

    });

    $("#btnReporte").click(function () {

        ReporteT(idCotizacion);

    });

    $("#btnBuscarAsegurado").click(function () {
        var longitudTexto = $("#txtDocumentoBus").val().trim().length;

        var tipoDocumento = $("#cmbxTiposDocumentoBus option:selected").text();
        var documento = $("#txtDocumentoBus").val().trim();
        var cuspp = $("#txtCuspp").val().trim();

        if ($("#cmbxTiposDocumentoBus").val() != "") {
            if (documento == "") {
                aviso("Aviso", "Debe ingresar un Número de Documento");
                document.getElementById("txtDocumentoBus").focus();
                return;
            }
            else {
                if (indicadorLongDoc == 1 && longitudTexto != longitudDoc) {
                    aviso("Aviso", "La longitud del número de documento debe ser de " + longitudDoc);
                    return;
                }
            }
        }

        if (cuspp != "" || (tipoDocumento != "" && documento != "")) {
            for (var i = 0; i < idsBeneficiarios.length; i++) {
                var url = $("#urlEliminarBen").val();
                idBeneficiario = idsBeneficiarios[i];

                var fields = {
                    idBeneficiario: idBeneficiario
                };

                sendValues(fields, doSuccessEliminarBen, doErrorEliminarBen, url)
            }

            idsBeneficiarios.length = 0;

            var url = $("#urlConsultaAse").val();

            var fields = {
                cuspp: cuspp,
                tipoDocumento: tipoDocumento,
                documento: documento
            };

            sendValues(fields, doSuccessConsultarAse, doErrorConsultarAse, url);
        }
        else
            aviso("Aviso", "Ingrese por lo menos un filtro");
    });

    $("#cmbxTiposDocumentoCot").change(function () {
        var url = $("#urlTipoDocumento").val();
        var idTipoDocumento = $("#cmbxTiposDocumentoCot").val();

        if (idTipoDocumento != "") {
            var fields = {
                idTipoDocumento: idTipoDocumento
            };

            sendValues(fields, doSuccessTipoDocumento, doErrorTipoDocumento, url);
        }
    });

    $("#cmbxTiposDocumentoBus").change(function () {
        var url = $("#urlTipoDocumento").val();
        var idTipoDocumento = $("#cmbxTiposDocumentoBus").val();

        if (idTipoDocumento != "") {
            var fields = {
                idTipoDocumento: idTipoDocumento
            };

            sendValues(fields, doSuccessTipoDocumentoBus, doErrorTipoDocumento, url);
        }
    });

    $("#cmbxAfpCot").change(function () {
        var url = $("#urlTipoAfp").val();
        var idTipoAfp = $("#cmbxAfpCot option:selected").text();

        if (idTipoAfp != "") {
            var fields = {
                afp: idTipoAfp,
                idsModalidades: idsModalidades
            };

            sendValues(fields, doSuccessTipoAfp, doErrorTipoAfp, url);
        }
    });

    $("#cmbxTiposDocumentoBen").change(function () {
        var url = $("#urlTipoDocumento").val();
        var idTipoDocumento = $("#cmbxTiposDocumentoBen").val();

        if (idTipoDocumento != "") {
            var fields = {
                idTipoDocumento: idTipoDocumento
            };

            sendValues(fields, doSuccessTipoDocumentoBen, doErrorTipoDocumentoBen, url);
        }
    });

    $("#btnRegresar").click(function () {

        var IdTipoDocumento = $("#cmbxTiposDocumentoCot").val();
        var Documento = $("#txtDocumentoCot").val().trim();
        var Nombres = $("#txtNombresCot").val().trim();
        var ApellidoPaterno = $("#txtApellidoPaternoCot").val().trim();
        var ApellidoMaterno = $("#txtApellidoMaternoCot").val().trim();
        var IdDepartamento = $("#cmbxDepartamentosCot").val();
        var IdProvincia = $("#cmbxProvinciasCot").val();
        var IdDistrito = $("#cmbxDistritosCot").val();
        var IdSexo = $("#cmbxSexoCot").val();
        var FechaNacimiento = $("#txtFechaNacimientoCot").val();
        var CUSPP = $("#txtCUSPPCot").val().trim();
        var IdAfp = $("#cmbxAfpCot").val();
        var IdPension = $("#cmbxPensionesCot").val();
        var Cic = $("#txtCICCot").val().trim();
        var FechaDevengue = $("#txtDevSolCot").val();
        var FechaEstudio = $("#txtFechaEstudioCot").val();
        var IdAsesor = $("#cmbxAsesoresCot").val();
        var GastoSepelio = $("#txtGastoSepelioCot").val().trim();
        var TipoCambio = $("#txtTipoCambio").val();

        if (Nombres == "" || ApellidoPaterno == "" || ApellidoMaterno == "" || IdDepartamento == "" || IdDepartamento == null || IdProvincia == "" || IdProvincia == null
            || IdDistrito == "" || IdDistrito == null || IdSexo == "" || IdSexo == null || FechaNacimiento == "" || CUSPP == "" || IdAfp == "" || IdAfp == null || FechaDevengue == "" || FechaEstudio == ""
            || IdPension == "" || IdPension == null || Cic == "" || IdAsesor == "" || GastoSepelio == "" || TipoCambio == "") {

            aviso("Aviso", "Debe ingresar todos los campos del asegurado");
            return;
        }

        var table, tr, td;
        var titular = true;
        var beneficiarios = false;

        table = document.getElementById("tablaBen");
        tr = table.getElementsByTagName("tr");
        for (var i = 0; i < tr.length; i++) {
            td = tr[i].getElementsByTagName("td")[0];
            if (td) {
                if (tr[i].getElementsByTagName("td")[2].innerText == "") {
                    beneficiarios = true;
                }
                else {
                    if (tr[i].getElementsByTagName("td")[2].innerText == "TITULAR")
                        titular = false;
                }

            }
        }
        if (beneficiarios) {
            aviso("Aviso", "Debe ingresar el parentesco de todos los Beneficiarios");
            return;
        }

        if (titular) {
            aviso("Aviso", "Favor de ingresar un Beneficiario Titular");
            return;
        }

        if ($("#cmbxPensionesCot option:selected").text() == "SOBREVIVENCIA") {
            if (idsBeneficiarios.length <= 1) {
                aviso("Aviso", "Favor de ingresar Beneficiarios");
                return;
            }
        }

        if (idsModalidades.length < 1) {
            aviso("Aviso", "Debe ingresar por lo menos una modalidad");
            return;
        }
        var urlV = $("#urlValidaBen").val();

            var fields = {
                idCotizacion: idCotizacion
            };

            sendValues(fields, doSuccessValidaRegresa, doErrorEliminarBen, urlV);
                    
       });

    $("#pestanaModalidad").click(function () {
        var elemento = document.getElementById("collapseCab");

        if (!$('#collapseCab').is(":visible"))
            $('#myform').valid();
        else
            $('#myform').valid();

    });

    $("#pestanaBeneficiario").click(function () {
        var elemento = document.getElementById("collapseCab");

        if (!$('#collapseCab').is(":visible"))
            $('#myform').valid();
        else
            $('#myform').valid();

        if (idTitular == 0)
            $("#btnAgregarBenTitular").trigger("click");

    });

    $("#btnAgregarBenTitular").click(function () {
        validaAgregarTitular();

    });

    $("#cmbxDepartamentosCot").change(function () {
        var url = $("#urlConsultaProvincias").val();
        var idDepartamento = $("#cmbxDepartamentosCot").val();

        if (idDepartamento != "") {
            var fields = {
                idDepartamento: idDepartamento
            };

            sendValues(fields, doSuccessConsultaProvincias, doErrorConsultaProvincias, url);
        }
    });

    $("#cmbxProvinciasCot").change(function () {
        var url = $("#urlConsultaDistrito").val();
        var idProvincia = $("#cmbxProvinciasCot").val();

        if (idProvincia != "") {
            var fields = {
                idProvincia: idProvincia
            };

            sendValues(fields, doSuccessConsultaDistrito, doErrorConsultaDistrito, url);
        }
    });

    
    // Carga de Situación de Invalidez
    $("#cmbxParentescosBen").change(function () {
        var url = $("#urlConsultarSI").val();
        var idParentesco = $("#cmbxParentescosBen").val();
        var idPension = $("#cmbxPensionesCot").val();

        var fields = {
            idParentesco: idParentesco,
            idPension: idPension
        };

        sendValues(fields, doSuccessConsultaSituacionInv, doErrorConsultaSituacionInv, url);
    });

    // Eliminación de Beneficiarios
    $("#btnEliminarBen").click(function () {
        var url = $("#urlEliminarBens").val();

        var fields = {
            idsBeneficiarios: idsBeneficiarios
        };

        sendValues(fields, doSuccessEliminarBeneficiarios, doErrorEliminarBeneficiarios, url);
    });

    $("#btnEliminarBenC").click(function () {
        var url = $("#urlEliminarBensC").val();

        var fields = {
            idsBeneficiarios: idsBeneficiarios
        };

        sendValues(fields, doSuccessEliminarBeneficiarios, doErrorEliminarBeneficiarios, url);
    });

    // Eliminación de Modalidades
    $("#btnEliminarMod").click(function () {
        var url = $("#urlEliminarMods").val();

        var fields = {
            idsModalidades: idsModalidades
        };

        sendValues(fields, doSuccessEliminarModalidades, doErrorEliminarModalidades, url);
    });

    $("#btnEliminarModC").click(function () {
        var url = $("#urlEliminarModsC").val();

        var fields = {
            idsModalidades: idsModalidades
        };

        sendValues(fields, doSuccessEliminarModalidades, doErrorEliminarModalidades, url);
    });

    if ($("#hdnModificar").val() != undefined) {
        var url = $("#urlConsultarCotizacion").val();
        idCotizacion = obtenerValorParametro('idCotizacion');
        var operacion = obtenerValorParametro('operacion');

        if (operacion == 1)
            banderaCot = 'U';

        var fields = {
            idCotizacion: idCotizacion,
            operacion: operacion
        };

        sendValues(fields, doSuccessConsultarCot, doErrorConsultarCot, url);
    }

    jQuery.validator.setDefaults({
        debug: true,
        success: "valid"
    });

    $("#myform").validate({
        errorClass: "requiredClass",
        rules: {
            cmbxTiposDocumentoCot: { required: true },
            txtDocumentoCot: { required: true },
            txtNombresCot: { required: true },
            txtApellidoPaternoCot: { required: true },
            txtApellidoMaternoCot: { required: true },
            cmbxDepartamentosCot: { required: true },
            cmbxProvinciasCot: { required: true },
            cmbxDistritosCot: { required: true },
            cmbxSexoCot: { required: true },
            txtFechaNacimientoCot: { required: true },
            txtCUSPPCot: { required: true },
            cmbxAfpCot: { required: true },
            cmbxPensionesCot: { required: true },
            txtCICCot: { required: true },
            txtDevSolCot: { required: true },
            txtFechaEstudioCot: { required: true },
            txtGastoSepelioCot: { required: true },
            cmbxAsesoresCot: { required: true },
            txtTipoCambio: { required: true }
        }
    });

    $("#formBen").validate({
        errorClass: "requiredClass",
        rules: {
            cmbxParentescosBen: { required: true },
            cmbxTiposDocumentoBen: { required: true },
            cmbxSexoBen: { required: true },
            txtFechaNacimientoBen: { required: true },
            //txtNumDocumentoBen: { required: true },
            cmbxSitInvalidezBen: { required: true },
        }
    });

    $("#formMod").validate({
        errorClass: "requiredClass",
        rules: {
            cmbxMonedasMod: { required: true },
            cmbxTiposRentaMod: { required: true },
            cmbxAniosDiferidosMod: { required: true },
            txtRentaAfpMod: { required: true },
            txtPrimerTramoMod: { required: true },
            cmbxModalidadesMod: { required: true },
            cmbxAniosGarantizadosMod: { required: true },
            cmbxGratificacionMod: { required: true },
            cmbxRentaTemporalMod: { required: true },
            txtSegundoTramoMod: { required: true },
            cmbxComisionesMod: { required: true },
            cmbxDerGraMod: { required: true }
        }
    });

});

// Registro de Beneficiario

function fn_agregarBen() {
    var longitudTexto = $("#txtNumDocumentoBen").val().trim().length;
    var Parentesco = $("#cmbxParentescosBen").find('option:selected').text();
    var pension = $('#cmbxPensionesCot').find('option:selected').text();
    var FechaFallecimiento;
    var FechaInvalidez;
    var envia = true;

    if (pension == "SOBREVIVENCIA") {
        if (Parentesco == "TITULAR")
            FechaFallecimiento = $("#txtDevSolCot").val();
        else
            FechaFallecimiento = $("#txtFechaFallecimientoBen").val();
    }
    else
        FechaFallecimiento = $("#txtFechaFallecimientoBen").val();

    if (pension == "INVALIDEZ PARCIAL" || pension == "INVALIDEZ TOTAL") {
        if (Parentesco == "TITULAR")
            FechaInvalidez = $("#txtFechaInvalidezCot").val();
        else
            FechaInvalidez = $("#txtFechaInvalidezBen").val();
    }
    else
        FechaInvalidez = $("#txtFechaInvalidezBen").val();

    if ($('#formBen').valid()) {
        if (longitudTexto > 0) {
            if (indicadorLongDoc == 1 && longitudTexto != longitudDoc) {
                aviso("Aviso", "La longitud del número de documento debe ser de " + longitudDoc);
                return;
            }
        }

        var url = $("#urlRegistrarBen").val();
        var Nombres = $("#txtNombresBen").val().trim();
        var Apellidos = $("#txtApellidosBen").val().trim();
        var Documento = $("#txtNumDocumentoBen").val().trim() == "" ? "00000000" : $("#txtNumDocumentoBen").val().trim();
        var FechaNacimiento = $("#txtFechaNacimientoBen").val();
        var IdSexo = $("#cmbxSexoBen").val();
        var IdParentesco = $("#cmbxParentescosBen").val();
        var IdTipoDocumento = $("#cmbxTiposDocumentoBen").val();
        var IdSituacionInvalidez = $("#cmbxSitInvalidezBen").val();
        var IdPension = $("#cmbxPensionesCot").val();
        var SituacionInvalidez = $("#cmbxSitInvalidezBen").find('option:selected').text();
        var FechaDev = $("#txtDevSolCot").val();
        if (($("#cmbxParentescosBen option:selected").text()) == "HIJOS") {
            if (($("#cmbxSitInvalidezBen option:selected").text()) != "INVALIDEZ TOTAL") {
                if (validaEdad()) {
                    envia = false;
                    aviso("Aviso", "No puede ingresar un hijo mayor de 28 años");
                }
                else {
                    envia = true;
                }
            }
            else
                envia = true;
        }
        else
            envia = true;

        if (envia) {
            var fields =
                {
                    bandera: banderaBen,
                    IdBeneficiario: idBeneficiario,
                    Nombres: Nombres,
                    Apellidos: Apellidos,
                    Documento: Documento,
                    FechaNacimiento: FechaNacimiento,
                    IdSexo: IdSexo,
                    IdParentesco: IdParentesco,
                    IdTipoDocumento: IdTipoDocumento,
                    IdSituacionInvalidez: IdSituacionInvalidez,
                    FechaInvalidezStr: FechaInvalidez,
                    FechaFallecimientoStr: FechaFallecimiento,
                    idPension: IdPension,
                    Parentesco: Parentesco,
                    idTitular: idTitular,
                    idsBeneficiarios: idsBeneficiarios,
                    SituacionInvalidez: SituacionInvalidez,
                    FecDev: FechaDev
                };

            sendValues(fields, doSuccessRegistroBen, doErrorRegistroBen, url);
        }
    }
    else {
        $("html, body").animate({ scrollTop: 0 }, "slow");
    }
};

function fn_agregarBenTitular() {
    document.getElementById("btnAgregarBenTitular").style.display = "none";

    var IdParentesco;
    var selectobject;
    var pension = $('#cmbxPensionesCot').find('option:selected').text();
    if (pension == "SOBREVIVENCIA") {
        $('#txtFechaFallecimientoCot').val($("#txtDevSolCot").val());
    }
    if (pension == "INVALIDEZ TOTAL" || pension == "INVALIDEZ PARCIAL") {
        $('#txtFechaFallecimientoCot').val($("#txtDevSolCot").val());
    }

    selectobject = document.getElementById("cmbxParentescosBen");

    for (var i = 0; i < selectobject.length; i++) {
        if (selectobject.options[i].text == 'TITULAR')
            IdParentesco = selectobject.options[i].value;
    }
    var FechaInvalidez;
    var FechaFallecimiento;

    var longitudTexto = $("#txtNumDocumentoBen").val().length;

    var url = $("#urlRegistrarBen").val();
    var IdTipoDocumento = $("#cmbxTiposDocumentoCot").val() == null ? "" : $("#cmbxTiposDocumentoCot").val();
    var Documento = $("#txtDocumentoCot").val().trim();
    var Nombres = $("#txtNombresCot").val().trim();
    var Apellidos = $("#txtApellidoPaternoCot").val() + ' ' + $("#txtApellidoMaternoCot").val().trim();
    var IdSexo = $("#cmbxSexoCot").val();
    var FechaNacimiento = $("#txtFechaNacimientoCot").val();
    if (pension == "SOBREVIVENCIA") {
        FechaFallecimiento = $("#txtFechaFallecimientoCot").val();
        FechaInvalidez = "";
    }
    else if (pension == "INVALIDEZ TOTAL" || pension == "INVALIDEZ PARCIAL") {
        FechaInvalidez = $("#txtDevSolCot").val();
        FechaFallecimiento = "";
    }
    else {
        FechaInvalidez = "";
        FechaFallecimiento = "";
    }

    
    var IdPension = $("#cmbxPensionesCot").val();
    var Parentesco = "TITULAR";
    var IdSituacionInvalidez = 0;
    var SituacionInvalidez = "";
    var FechaDev = $("#txtDevSolCot").val();
    var fields =
                {
                    bandera: banderaBen,
                    IdBeneficiario: idBeneficiario,
                    Nombres: Nombres,
                    Apellidos: Apellidos,
                    Documento: Documento,
                    FechaNacimiento: FechaNacimiento,
                    IdSexo: IdSexo,
                    IdParentesco: IdParentesco,
                    IdTipoDocumento: IdTipoDocumento,
                    IdSituacionInvalidez: IdSituacionInvalidez,
                    FechaInvalidezStr: FechaInvalidez,
                    FechaFallecimientoStr: FechaFallecimiento,
                    idPension: IdPension,
                    Parentesco: Parentesco,
                    idTitular: idTitular,
                    idsBeneficiarios: idsBeneficiarios,
                    situacionInvalidez: SituacionInvalidez,
                    FecDev: FechaDev
                };

    sendValues(fields, doSuccessRegistroBenTitu, doErrorRegistroBen, url);
};

function doSuccessRegistroBenTitu(result) {
    var beneficiario = result.Object.pensionBen;
    var beneficiarios = { Object: result.Object.beneficiarios };
    var mensaje = result.Message;
    var SituacionInvalidez;
    var selectobject;
    var pension = $('#cmbxPensionesCot').find('option:selected').text();
    var idTipoDocumento = $('#cmbxTiposDocumentoCot').val();
    var tipoDocumento = idTipoDocumento == "" ? "" : $('#cmbxTiposDocumentoCot').find('option:selected').text();

    var nFilas = $("#tablaBen tr").length;
    var cadena = "<tr>";
    idTitular = beneficiario.IdBeneficiario;
    cadena = cadena + "<td style = 'display:none'>" + beneficiario.IdBeneficiario + "</td>";
    cadena = cadena + "<td>" + nFilas + "</td>";
    cadena = cadena + "<td>TITULAR</td>";
    cadena = cadena + "<td>" + tipoDocumento + "</td>";
    cadena = cadena + "<td>" + $("#txtDocumentoCot").val() + "</td>";
    cadena = cadena + "<td>" + $("#txtFechaNacimientoCot").val() + "</td>";
    cadena = cadena + "<td>" + $("#cmbxSexoCot").find('option:selected').text() + "</td>";
    cadena = cadena + "<td>" + beneficiario.SituacionInvalidez + "</td>";
    cadena = cadena + "<td>" + beneficiario.PorcentajeBen + "</td>";

    if ($("#hdnCrear").val() != undefined || $("#hdnPermisos").val() != undefined) {
        cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_eliminarBen(" + beneficiario.IdBeneficiario + ");' title='Eliminar'><i class='fa fa-remove fa-lg icon-vida'></i></a></td>";
        cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_modificarBen(" + beneficiario.IdBeneficiario + ");' title='Modificar'><i class='fa fa-edit fa-lg icon-vida'></i></a></td>";
    }
    else {
        cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
        cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
    }

    $("#tablaBen tbody").prepend(cadena);

    var table, tr, td;

    table = document.getElementById("tablaBen");
    tr = table.getElementsByTagName("tr");

    for (var i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            tr[i].getElementsByTagName("td")[1].innerText = i;
        }
    }

    idsBeneficiarios.push(String(beneficiario.IdBeneficiario));

    doSuccessPorcentajesBen(beneficiarios);

    if (mensaje != null && mensaje != "")
        aviso("Aviso", mensaje);

    $('#btnAgregarBenTitular').attr("disabled", true);
}

function doSuccessRegistroBen(result) {
    var beneficiario = result.Object.pensionBen;
    var beneficiarios = { Object: result.Object.beneficiarios };
    var mensaje = result.Message;
    var numDocumento = $("#txtNumDocumentoBen").val().trim() == "" ? "00000000" : $("#txtNumDocumentoBen").val().trim();
    if (banderaBen == 'U') {
        var table, tr, td;

        table = document.getElementById("tablaBen");
        tr = table.getElementsByTagName("tr");
        for (var i = 0; i < tr.length; i++) {
            td = tr[i].getElementsByTagName("td")[0];
            if (td) {
                if (td.innerHTML == idBeneficiario) {
                    tr[i].getElementsByTagName("td")[2].innerText = $('#cmbxParentescosBen').find('option:selected').text();
                    tr[i].getElementsByTagName("td")[3].innerText = $('#cmbxTiposDocumentoBen').find('option:selected').text();
                    tr[i].getElementsByTagName("td")[4].innerText = numDocumento;
                    tr[i].getElementsByTagName("td")[5].innerText = $("#txtFechaNacimientoBen").val();
                    tr[i].getElementsByTagName("td")[6].innerText = $("#cmbxSexoBen").find('option:selected').text();
                    tr[i].getElementsByTagName("td")[7].innerText = $("#cmbxSitInvalidezBen").find('option:selected').text();
                    tr[i].getElementsByTagName("td")[8].innerText = beneficiario.PorcentajeBen;
                }
            }
        }
    }
    else {
        var nFilas = $("#tablaBen tr").length;
        var cadena = "<tr>";

        cadena = cadena + "<td style = 'display:none'>" + beneficiario.IdBeneficiario + "</td>";
        cadena = cadena + "<td>" + nFilas + "</td>";
        cadena = cadena + "<td>" + $('#cmbxParentescosBen').find('option:selected').text() + "</td>";
        cadena = cadena + "<td>" + $('#cmbxTiposDocumentoBen').find('option:selected').text() + "</td>";
        cadena = cadena + "<td>" + numDocumento + "</td>";
        cadena = cadena + "<td>" + $("#txtFechaNacimientoBen").val() + "</td>";
        cadena = cadena + "<td>" + $("#cmbxSexoBen").find('option:selected').text() + "</td>";
        cadena = cadena + "<td>" + $("#cmbxSitInvalidezBen").find('option:selected').text() + "</td>";
        cadena = cadena + "<td>" + beneficiario.PorcentajeBen + "</td>";

        if ($("#hdnCrear").val() != undefined || $("#hdnPermisos").val() != undefined) {
            cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_eliminarBen(" + beneficiario.IdBeneficiario + ");' title='Eliminar'><i class='fa fa-remove fa-lg icon-vida'></i></a></td>";
            cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_modificarBen(" + beneficiario.IdBeneficiario + ");' title='Modificar'><i class='fa fa-edit fa-lg icon-vida'></i></a></td>";
        }
        else {
            cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
            cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
        }

        if ($('#cmbxParentescosBen').find('option:selected').text() == "TITULAR") {
            idTitular = beneficiario.IdBeneficiario;

            $("#tablaBen tbody").prepend(cadena);

            var table, tr, td;

            table = document.getElementById("tablaBen");
            tr = table.getElementsByTagName("tr");

            for (var i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[0];
                if (td) {
                    tr[i].getElementsByTagName("td")[1].innerText = i;
                }
            }
        }
        else
            $("#tablaBen tbody").append(cadena);

        idsBeneficiarios.push(String(beneficiario.IdBeneficiario));
    }

    doSuccessPorcentajesBen(beneficiarios);

    if (mensaje != null && mensaje != "")
        aviso("Aviso", mensaje);

    limpiarBen();
    $("#ModalBen").modal('hide');
}

function doErrorRegistroBen(result) {
    aviso("Aviso", result.Message);
}

function limpiarBen() {
    $("#txtNombresBen").val("");
    $("#txtApellidosBen").val("");
    $("#txtNumDocumentoBen").val("");
    $("#txtFechaNacimientoBen").val("");
    $("#cmbxSexoBen").val("");
    $("#cmbxParentescosBen").val("");
    $("#cmbxTiposDocumentoBen").val("");

    banderaBen = 'C';
}

// Eliminación de Beneficiario

function fn_dar_eliminarBen(paramIdBeneficiario) {
    var url = $("#urlEliminarBen").val();

    idBeneficiario = paramIdBeneficiario;
    var fields = {
        idBeneficiario: idBeneficiario
    };

    sendValues(fields, doSuccessEliminarBen, doErrorEliminarBen, url);
}

function fn_ValidaRegresa(paramidCotizacion) {
    var url = $("#urlValidaBen").val();

    idCotizacion = paramidCotizacion;
    var fields = {
        idCotizacion: idCotizacion
    };

    sendValues(fields, doSuccessValidaRegresa, doErrorEliminarBen, url);
}

function doSuccessValidaRegresa(result) {
    var urlM = $("#urlValidaMod").val();

    var fields2 = {
        idCotizacion: idCotizacion
    };

    sendValues(fields2, doSuccessValidaRegresaM, doErrorEliminarBen, urlM);
}
function fn_ValidaRegresaMod(paramidCotizacion) {
    var url = $("#urlValidaMod").val();

    idCotizacion = paramidCotizacion;
    var fields = {
        idCotizacion: idCotizacion
    };

    sendValues(fields, doSuccessValidaRegresaM, doErrorEliminarBen, url);
}

function doSuccessValidaRegresaM(result) {
    var url = $("#urlIndex").val();
    window.location.href = url;
}


function doSuccessEliminarBen(result) {
    var table, tr, td;

    table = document.getElementById("tablaBen");
    tr = table.getElementsByTagName("tr");
    for (var i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            if (td.innerHTML == result.Object) {
                if (tr[i].getElementsByTagName("td")[2].innerHTML == "TITULAR") {
                    $('#btnAgregarBenTitular').attr("disabled", false);
                    document.getElementById("btnAgregarBenTitular").style.display = "";
                    idTitular = 0;
                }

                tr[i].remove();
            }

        }
    }

    for (var i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            tr[i].getElementsByTagName("td")[1].innerText = i;
        }
    }

    var indice = idsBeneficiarios.indexOf(String(result.Object));

    if (indice > -1)
        idsBeneficiarios.splice(indice, 1);

}

function doErrorEliminarBen(result) {
    aviso("Aviso", result.Message);
}

// Eliminación de todos los Beneficiarios

function doSuccessEliminarBeneficiarios(result) {
    if (result.Object != null) {
        result.Object.forEach(function (b) {
            var table, tr, td;

            table = document.getElementById("tablaBen");
            tr = table.getElementsByTagName("tr");
            for (var i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[0];
                if (td) {
                    if (td.innerHTML == b) {
                        tr[i].remove();
                    }
                }
            }

            var indice = idsBeneficiarios.indexOf(b);

            if (indice > -1)
                idsBeneficiarios.splice(indice, 1);
        });

        document.getElementById("btnAgregarBenTitular").style.display = "";
        $('#btnAgregarBenTitular').attr("disabled", false);
        idTitular = 0;
    }
}

function doErrorEliminarBeneficiarios(result) {
    aviso("Aviso", result.Message);
}

// Modificación de Beneficiario

function fn_dar_modificarBen(paramIdBeneficiario) {
    var url = $("#urlConsultarBen").val();
    var idPension = $("#cmbxPensionesCot").val();

    idBeneficiario = paramIdBeneficiario;

    var fields = {
        idBeneficiario: idBeneficiario,
        idPension: idPension
    };

    sendValues(fields, doSuccessConsultarBen, doErrorConsultarBen, url);
}

function doSuccessConsultarBen(result) {
    var beneficiario = result.Object.beneficiario;
    var situacionesInvalidez = result.Object.situacionesInvalidez;

    var cmbxSituacionInv = document.getElementById("cmbxSitInvalidezBen");
    $("#cmbxSitInvalidezBen").empty();
    var cont = 1;

    cmbxSituacionInv.options[0] = new Option("-- Seleccione --", "");

    situacionesInvalidez.forEach(function (b) {
        cmbxSituacionInv.options[cont] = new Option(b.Elemento, b.IdSituacionInvalidez);
        cont++;
    });

    $("#txtNombresBen").val(beneficiario.Nombres);
    $("#txtApellidosBen").val(beneficiario.Apellidos);
    $("#txtNumDocumentoBen").val(beneficiario.Documento);
    $("#txtFechaNacimientoBen").val(beneficiario.FechaNacimientoStr);
    $("#cmbxSexoBen").val(beneficiario.IdSexo);
    $("#cmbxParentescosBen").val(beneficiario.IdParentesco == 0 ? "" : beneficiario.IdParentesco);
    $("#cmbxTiposDocumentoBen").val(beneficiario.IdTipoDocumento == 0 ? "" : beneficiario.IdTipoDocumento);
    $("#cmbxSitInvalidezBen").val(beneficiario.IdSituacionInvalidez);
    $("#txtFechaInvalidezBen").val(beneficiario.FechaInvalidezStr);
    $("#txtFechaFallecimientoBen").val(beneficiario.FechaFallecimientoStr);

    var url = $("#urlTipoDocumento").val();
    var idTipoDocumento = $("#cmbxTiposDocumentoBen").val();

    if (idTipoDocumento != "") {
        var fields = {
            idTipoDocumento: idTipoDocumento
        };

        sendValues(fields, doSuccessTipoDocumentoBen, doErrorTipoDocumentoBen, url);
    }

    banderaBen = 'U';
    $("#btnAgregarBen").trigger("click");
}

function doErrorConsultarBen(result) {
    aviso("Aviso", result.Message);
}

// Cambio de porcentajes para beneficiarios

function doSuccessPorcentajesBen(result) {
    var table, tr, td;

    result.Object.forEach(function (b) {
        table = document.getElementById("tablaBen");
        tr = table.getElementsByTagName("tr");

        for (var i = 0; i < tr.length; i++) {
            td = tr[i].getElementsByTagName("td")[0];
            if (td) {
                if (td.innerHTML == b.IdBeneficiario) {
                    tr[i].getElementsByTagName("td")[7].innerText = b.SituacionInvalidez;
                    tr[i].getElementsByTagName("td")[8].innerText = b.PorcentajeBen;
                }
            }
        }
    });
}

function doErrorPorcentajesBen(result) {
    aviso("Aviso", result.Message);
}

// Registro de Modalidad

function fn_agregarMod() {
    if ($('#formMod').valid()) {
        var url = $("#urlRegistrarMod").val();
        var AniosDiferidos = $("#cmbxAniosDiferidosMod").val().trim();
        var PorcentajeRentabilidadAfp = $("#txtRentaAfpMod").val();
        var AniosGarantizados = $("#cmbxAniosGarantizadosMod").val().trim();
        var Gratificacion = $("#cmbxGratificacionMod").val();
        var PorcentajeRentaTemporal = $("#cmbxRentaTemporalMod").val();
        var IdMoneda = $("#cmbxMonedasMod").val();
        var IdTipoRenta = $("#cmbxTiposRentaMod").val();
        var IdModalidadCat = $("#cmbxModalidadesMod").val();
        var PrimerTramo = $("#txtPrimerTramoMod").val() == null ? "0" : $("#txtPrimerTramoMod").val();
        var SegundoTramo = $("#txtSegundoTramoMod").val() == null ? "0" : $("#txtSegundoTramoMod").val();
        var IdComision = $("#cmbxComisionesMod").val();

        if ($("#cmbxTiposRentaMod option:selected").text() == "RENTA VITALICIA ESCALONADA") {
            if (PrimerTramo == "0" || PrimerTramo == null) {
                document.getElementById('rqPrimerTramo').style.display = 'block';
                $("#rqPrimerTramo").val("Requerido");
                document.getElementById('rqPrimerTramo').innerHTML = 'Requerido';
            }
            else
                document.getElementById('rqPrimerTramo').style.display = 'none';
            if (SegundoTramo == "0" || SegundoTramo == null) {
                document.getElementById('rqSegundoTramo').style.display = 'block';
                $("#rqSegundoTramo").val("Requerido");
                document.getElementById('rqSegundoTramo').innerHTML = 'Requerido';
            }
            else
                document.getElementById('rqSegundoTramo').style.display = 'none';
            if (PrimerTramo == "0" || SegundoTramo == "0")
                return;
        }

        var fields =
                    {
                        bandera: banderaMod,
                        idModalidad: idModalidad,
                        AniosDiferidos: AniosDiferidos,
                        PorcentajeRentabilidadAfp: PorcentajeRentabilidadAfp,
                        AniosGarantizados: AniosGarantizados,
                        Gratificacion: Gratificacion,
                        PorcentajeRentaTemporal: PorcentajeRentaTemporal,
                        IdMoneda: IdMoneda,
                        IdTipoRenta: IdTipoRenta,
                        IdModalidadCat: IdModalidadCat,
                        PrimerTramo: PrimerTramo,
                        SegundoTramo: SegundoTramo,
                        IdComision: IdComision
                    };

        sendValues(fields, doSuccessRegistroMod, doErrorRegistroMod, url);
    }
    else {
        var PrimerTramo = $("#txtPrimerTramoMod").val() == "" ? "0" : $("#txtPrimerTramoMod").val();
        var SegundoTramo = $("#txtSegundoTramoMod").val() == "" ? "0" : $("#txtSegundoTramoMod").val();

        if (PrimerTramo == 0) {
            document.getElementById('rqPrimerTramo').style.display = 'block';
            $("#rqPrimerTramo").val("Requerido");
        }
        else
            document.getElementById('rqPrimerTramo').style.display = "none";

        if (SegundoTramo == 0) {
            document.getElementById('rqSegundoTramo').style.display = 'block';
            $("#rqSegundoTramo").val("Requerido");
        }
        else
            document.getElementById('rqSegundoTramo').style.display = "none";
    }
};

function doSuccessRegistroMod(result) {
    if (banderaMod == 'U') {
        var table, tr, td;

        table = document.getElementById("tablaMod");
        tr = table.getElementsByTagName("tr");
        for (var i = 0; i < tr.length; i++) {
            td = tr[i].getElementsByTagName("td")[0];
            if (td) {
                if (td.innerHTML == idModalidad) {
                    var primerTramo = $("#txtPrimerTramoMod").val() == "" ? "-" : $("#txtPrimerTramoMod").val();
                    var segundoTramo = $("#txtSegundoTramoMod").val() == "" ? "-" : $("#txtSegundoTramoMod").val();
                    var aniosdiferidos = $("#cmbxAniosDiferidosMod").val() == "" ? "-" : $("#cmbxAniosDiferidosMod").val();
                    var aniosgarantizados = $("#cmbxAniosGarantizadosMod").val() == "" ? "-" : $("#cmbxAniosGarantizadosMod").val();

                    tr[i].getElementsByTagName("td")[2].innerText = $('#cmbxMonedasMod').find('option:selected').text();
                    tr[i].getElementsByTagName("td")[3].innerText = $('#cmbxTiposRentaMod').find('option:selected').text();
                    tr[i].getElementsByTagName("td")[4].innerText = aniosdiferidos;
                    tr[i].getElementsByTagName("td")[5].innerText = $('#cmbxModalidadesMod').find('option:selected').text();
                    tr[i].getElementsByTagName("td")[6].innerText = aniosgarantizados;
                    tr[i].getElementsByTagName("td")[7].innerText = $('#cmbxRentaTemporalMod').find('option:selected').text();
                    tr[i].getElementsByTagName("td")[8].innerText = $('#cmbxGratificacionMod').find('option:selected').text();
                    tr[i].getElementsByTagName("td")[9].innerText = primerTramo;
                    tr[i].getElementsByTagName("td")[10].innerText = segundoTramo;
                }
            }
        }
    }
    else {
        var primerTramo = $("#txtPrimerTramoMod").val() == "" ? "-" : $("#txtPrimerTramoMod").val();
        var segundoTramo = $("#txtSegundoTramoMod").val() == "" ? "-" : $("#txtSegundoTramoMod").val();
        var aniosdiferidos = $("#cmbxAniosDiferidosMod").val() == "" ? "-" : $("#cmbxAniosDiferidosMod").val();
        var aniosgarantizados = $("#cmbxAniosGarantizadosMod").val() == "" ? "-" : $("#cmbxAniosGarantizadosMod").val();

        var nFilas = $("#tablaMod tr").length;
        var cadena = "<tr>";
        cadena = cadena + "<td style = 'display:none'>" + result.Object.IdModalidad + "</td>";
        cadena = cadena + "<td>" + nFilas + "</td>";
        cadena = cadena + "<td>" + $("#cmbxMonedasMod").find('option:selected').text() + "</td>";
        cadena = cadena + "<td>" + $("#cmbxTiposRentaMod").find('option:selected').text() + "</td>";
        cadena = cadena + "<td>" + aniosdiferidos + "</td>";
        cadena = cadena + "<td>" + $("#cmbxModalidadesMod").find('option:selected').text() + "</td>";
        cadena = cadena + "<td>" + aniosgarantizados + "</td>";
        cadena = cadena + "<td>" + $("#cmbxRentaTemporalMod").find('option:selected').text() + "</td>";
        cadena = cadena + "<td>" + $("#cmbxGratificacionMod").find('option:selected').text() + "</td>";
        cadena = cadena + "<td>" + primerTramo + "</td>";
        cadena = cadena + "<td>" + segundoTramo + "</td>";

        if ($("#hdnCrear").val() != undefined || $("#hdnPermisos").val() != undefined) {
            cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_eliminarMod(" + result.Object.IdModalidad + ");' title='Eliminar'><i class='fa fa-remove fa-lg icon-vida'></i></a></td>";
            cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_modificarMod(" + result.Object.IdModalidad + ");' title='Modificar'><i class='fa fa-edit fa-lg icon-vida'></i></a></td>";
        }
        else {
            cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
            cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
        }

        $("#tablaMod tbody").append(cadena);

        idsModalidades.push(String(result.Object.IdModalidad));
    }

    limpiarMod();
}

function doErrorRegistroMod(result) {
    aviso("Aviso", result.Message);
}

function limpiarMod() {
    $("#cmbxMonedasMod").val("");
    $("#cmbxTiposRentaMod").val("");
    $("#cmbxAniosDiferidosMod").val("");
    $("#cmbxModalidadesMod").val("");
    $("#cmbxAniosGarantizadosMod").val("");
    $("#cmbxRentaTemporalMod").val("");
    $("#cmbxGratificacionMod").val("No");
    $("#txtPrimerTramoMod").val("");
    $("#txtSegundoTramoMod").val("");
    $("#txtRentaAfpMod").val(valAfp);
    //$("#cmbxComisionesMod").val("");
    //$("#cmbxAniosGarantizadosMod option[value='0']").show();
    $('#cmbxAniosGarantizadosMod').attr("disabled", false);
    $('#txtPrimerTramoMod').attr("disabled", true);
    $('#txtSegundoTramoMod').attr("disabled", true);
    //$("#cmbxAniosDiferidosMod option[value='0']").show();
    //$("#cmbxRentaTemporalMod option[value='0']").show();
    $('#cmbxRentaTemporalMod').attr("disabled", false);
    $('#cmbxAniosDiferidosMod').attr("disabled", false);
    $('#simboloMoneda').val(" ");
    banderaMod = 'C';

    $("#ModalDet").modal('hide');
}

// Eliminación de Modalidad

function fn_dar_eliminarMod(paramIdModalidad) {
    var url = $("#urlEliminarMod").val();
    idModalidad = paramIdModalidad;

    var fields = {
        idModalidad: idModalidad
    };

    sendValues(fields, doSuccessEliminarMod, doErrorEliminarMod, url);
}

function fn_dar_eliminarModC(paramIdModalidad) {
    var url = $("#urlEliminarModC").val();
    idModalidad = paramIdModalidad;

    var fields = {
        idModalidad: idModalidad
    };

    sendValues(fields, doSuccessEliminarMod, doErrorEliminarMod, url);
}

function doSuccessEliminarMod(result) {
    var table, tr, td;

    table = document.getElementById("tablaMod");
    tr = table.getElementsByTagName("tr");
    for (var i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            if (td.innerHTML == result.Object)
                tr[i].remove();
        }
    }

    for (var i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            tr[i].getElementsByTagName("td")[1].innerText = i;
        }
    }

    var indice = idsModalidades.indexOf(String(result.Object));

    if (indice > -1)
        idsModalidades.splice(indice, 1);
}

function doErrorEliminarMod(result) {
    aviso("Aviso", result.Message);
}

// Eliminación de todas las Modalidades

function doSuccessEliminarModalidades(result) {
    if (result.Object != null) {
        result.Object.forEach(function (m) {
            var table, tr, td;

            table = document.getElementById("tablaMod");
            tr = table.getElementsByTagName("tr");
            for (var i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[0];
                if (td) {
                    if (td.innerHTML == m) {
                        tr[i].remove();
                    }
                }
            }

            var indice = idsModalidades.indexOf(m);

            if (indice > -1)
                idsModalidades.splice(indice, 1);
        });
    }
}

function doErrorEliminarModalidades(result) {
    aviso("Aviso", result.Message);
}

// Modificación de Modalidad

function fn_dar_modificarMod(paramIdModalidad) {
    var url = $("#urlConsultarMod").val();
    idModalidad = paramIdModalidad;

    var fields = {
        idModalidad: idModalidad
    };

    sendValues(fields, doSuccessConsultarMod, doErrorConsultarMod, url);

}

function doSuccessConsultarMod(result) {
    var modalidad = result.Object;
    var primerTramo = modalidad.PrimerTramo == 0 ? "" : modalidad.PrimerTramo;
    var segundoTramo = modalidad.SegundoTramo == 0 ? "" : modalidad.SegundoTramo
    var AniosDiferidos = modalidad.AniosDiferidos;
    var AniosGarantizados = modalidad.AniosGarantizados;

    idAniosRent = AniosDiferidos;
    idPorRent = modalidad.PorcentajeRentaTemporal;
    idAniosGar = AniosGarantizados;
    idPrimerT = primerTramo;
    idSegundoT = segundoTramo;

    cargarCombosVal = true;

    //Validaciones modalidades
    var url = $("#urlConsultarVM").val();
    var idPension = $("#cmbxPensionesCot").val();
    var idRenta = modalidad.IdTipoRenta;
    var idMoneda = modalidad.IdMoneda;

    var fields = {
        idPension: idPension,
        idRenta: idRenta,
        idMoneda: idMoneda
    };

    sendValues(fields, doSuccessConsultaValidacionesMod, doErrorConsultaValidacionesMod, url);

    $("#cmbxMonedasMod").val(modalidad.IdMoneda);
    $("#cmbxTiposRentaMod").val(modalidad.IdTipoRenta);
    $("#cmbxAniosDiferidosMod").val(AniosDiferidos);
    $("#cmbxModalidadesMod").val(modalidad.IdModalidadCat);
    $("#cmbxAniosGarantizadosMod").val(AniosGarantizados);
    $("#cmbxRentaTemporalMod").val(modalidad.PorcentajeRentaTemporal);
    //$("#cmbxGratificacionMod").val(modalidad.Gratificacion);
    $("#cmbxGratificacionMod").val("No");
    $("#txtPrimerTramoMod").val(primerTramo);
    $("#txtSegundoTramoMod").val(segundoTramo);
    $("#txtRentaAfpMod").val(modalidad.PorcentajeRentabilidadAfp);
    $("#cmbxComisionesMod").val(modalidad.IdComision);

    if ($("#cmbxTiposRentaMod option:selected").text() == "RENTA VITALICIA ESCALONADA") {
        $('#txtPrimerTramoMod').attr("disabled", false);
        $('#txtSegundoTramoMod').attr("disabled", false);
    }
    if ($("#cmbxTiposRentaMod option:selected").text() == "RENTA VITALICIA") {
        // $('#cmbxAniosDiferidosMod').attr("disabled", true);
        // $('#cmbxAniosGarantizadosMod').attr("disabled", true);
        $('#txtPrimerTramoMod').attr("disabled", true);
        $('#txtSegundoTramoMod').attr("disabled", true);

        $('#txtPrimerTramoMod').val("0");
        $('#txtSegundoTramoMod').val("0");
    }


    if ($("#cmbxTiposRentaMod option:selected").text() == " RENTA TEMPORAL CON RENTA VITALICIA") {
        $('#txtPrimerTramoMod').attr("disabled", true);
        $('#txtSegundoTramoMod').attr("disabled", true);

        $('#txtPrimerTramoMod').val("0");
        $('#txtSegundoTramoMod').val("0");
    }

    banderaMod = 'U';
    $("#btnAgregarMod").trigger("click");
}

function doErrorConsultarMod(result) {
    aviso("Aviso", result.Message);
}

// Registro de Paquetes

function doSuccessRegistrarPaq(result) {
    result.Object.forEach(function (m) {
        var nFilas = $("#tablaMod tr").length;
        var cadena = "<tr>";
        cadena = cadena + "<td style = 'display:none'>" + m.IdModalidad + "</td>";
        cadena = cadena + "<td>" + nFilas + "</td>";
        cadena = cadena + "<td>" + m.Moneda + "</td>";
        cadena = cadena + "<td>" + m.TipoRenta + "</td>";
        cadena = cadena + "<td>" + m.AniosDiferidos + "</td>";
        cadena = cadena + "<td>" + m.ModalidadCat + "</td>";
        cadena = cadena + "<td>" + m.AniosGarantizados + "</td>";
        cadena = cadena + "<td>" + m.PorcentajeRentaTemporal + "</td>";
        cadena = cadena + "<td>" + m.Gratificacion + "</td>";
        cadena = cadena + "<td>" + m.PrimerTramoStr + "</td>";
        cadena = cadena + "<td>" + m.SegundoTramoStr + "</td>";

        if ($("#hdnCrear").val() != undefined || $("#hdnPermisos").val() != undefined) {
            cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_eliminarMod(" + m.IdModalidad + ");' title='Eliminar'><i class='fa fa-remove fa-lg icon-vida'></i></a></td>";
            cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_modificarMod(" + m.IdModalidad + ");' title='Modificar'><i class='fa fa-edit fa-lg icon-vida'></i></a></td>";
        }
        else {
            cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
            cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
        }

        $("#tablaMod tbody").append(cadena);

        idsModalidades.push(String(m.IdModalidad));
    });
}

function doErrorRegistrarPaq(result) {
    aviso("Aviso", result.Message);
}

// Registro de Cotización

function doSuccessRegistrarCot(result) {
    $('#modalcargar').modal('hide');
    aviso("Aviso", result.Message);
    var url = $("#urlIndex").val();
    var res = result.Object;
    //$('#modalcargar').modal('hide');
    $("#btn_modal_aceptar").one('click', function () {
        idCotizacion = res.IdCotizacion;
        $('#sch_modal').modal('hide');
        // Reporte(res.IdCotizacion);
        //document.getElementById("btnReporteT").style.display = "block";
        //document.getElementById("btnReporte").style.display = "block";
        document.getElementById("btnReporteT").style.display = "";
        document.getElementById("btnReporte").style.display = "";

        //window.location.href = url;

    });
}

function doErrorRegistrarCot(result) {
    $('#modalcargar').modal('hide');
    var resultado = result.Object;
    aviso("Aviso", result.Message, resultado);
}

// Consultar Cotización

function doSuccessConsultarCot(result) {
    var cotizacion = result.Object.cotizacion;
    var beneficiarios = result.Object.beneficiarios;
    var modalidades = result.Object.modalidades;

    valAfp = cotizacion.PorAfp;

    $("#cmbxTiposDocumentoCot").val(cotizacion.IdTipoDocumento);
    $("#txtDocumentoCot").val(cotizacion.Documento);
    $("#txtNombresCot").val(cotizacion.Nombres.trim());
    $("#txtApellidoPaternoCot").val(cotizacion.ApellidoPaterno.trim());
    $("#txtApellidoMaternoCot").val(cotizacion.ApellidoMaterno.trim());
    $("#cmbxDepartamentosCot").val(cotizacion.IdDepartamento);
    $("#cmbxSexoCot").val(cotizacion.IdSexo);
    $("#txtFechaNacimientoCot").val(cotizacion.FechaNacimientoStr);
    $("#txtCUSPPCot").val(cotizacion.CUSPP);
    $("#cmbxAfpCot").val(cotizacion.IdAfp);
    $("#cmbxPensionesCot").val(cotizacion.IdPension);
    $("#txtCICCot").val(cotizacion.Cic);
    $("#txtDevSolCot").val(cotizacion.FechaDevengueStr);
    $("#txtFechaEstudioCot").val(cotizacion.FechaEstudioStr);
    $("#txtGastoSepelioCot").val(cotizacion.GastoSepelio);
    $("#cmbxAsesoresCot").val(cotizacion.IdAsesor);
    $("#txtTipoCambio").val(cotizacion.TipoCambio);
    $("#txtRentaAfpMod").val(cotizacion.PorAfp)

    idProvincia = cotizacion.IdProvincia;
    idDistrito = cotizacion.IdDistrito;

    //Provincias

    var url = $("#urlConsultaProvincias").val();
    var idDepartamento = $("#cmbxDepartamentosCot").val();

    var fields = {
        idDepartamento: idDepartamento
    };

    sendValues(fields, doSuccessConsultaProvincias, doErrorConsultaProvincias, url);

    //Distritos

    var url = $("#urlConsultaDistrito").val();
    var idProv = $("#cmbxProvinciasCot").val();

    var fields = {
        idProvincia: idProvincia
    };

    sendValues(fields, doSuccessConsultaDistrito, doErrorConsultaDistrito, url);

    beneficiarios.forEach(function (b) {
        if (b.Parentesco == "TITULAR") {
            idTitular = b.IdBeneficiario;
            $('#btnAgregarBenTitular').attr("disabled", true);
        }

        var nFilas = $("#tablaBen tr").length;
        var cadena = "<tr>";
        cadena = cadena + "<td style = 'display:none'>" + b.IdBeneficiario + "</td>";
        cadena = cadena + "<td>" + nFilas + "</td>";
        cadena = cadena + "<td>" + b.Parentesco + "</td>";
        cadena = cadena + "<td>" + b.TipoDocumento + "</td>";
        cadena = cadena + "<td>" + b.Documento + "</td>";
        cadena = cadena + "<td>" + b.FechaNacimientoStr + "</td>";
        cadena = cadena + "<td>" + b.Sexo + "</td>";
        cadena = cadena + "<td>" + b.SituacionInvalidez + "</td>";
        cadena = cadena + "<td>" + b.PorcentajeBen + "</td>";

        if ($("#hdnCrear").val() != undefined || $("#hdnPermisos").val() != undefined) {
            cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_eliminarBen(" + b.IdBeneficiario + ");' title='Eliminar'><i class='fa fa-remove fa-lg icon-vida'></i></a></td>";
            cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_modificarBen(" + b.IdBeneficiario + ");' title='Modificar'><i class='fa fa-edit fa-lg icon-vida'></i></a></td>";
        }
        else {
            cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
            cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
        }

        $("#tablaBen tbody").append(cadena);

        idsBeneficiarios.push(String(b.IdBeneficiario));
    });

    modalidades.forEach(function (m) {
        var nFilas = $("#tablaMod tr").length;
        var cadena = "<tr>";
        cadena = cadena + "<td style = 'display:none'>" + m.IdModalidad + "</td>";
        cadena = cadena + "<td>" + nFilas + "</td>";
        cadena = cadena + "<td>" + m.Moneda + "</td>";
        cadena = cadena + "<td>" + m.TipoRenta + "</td>";
        cadena = cadena + "<td>" + m.AniosDiferidos + "</td>";
        cadena = cadena + "<td>" + m.ModalidadCat + "</td>";
        cadena = cadena + "<td>" + m.AniosGarantizados + "</td>";
        cadena = cadena + "<td>" + m.PorcentajeRentaTemporal + "</td>";
        cadena = cadena + "<td>" + m.Gratificacion + "</td>";
        cadena = cadena + "<td>" + m.PrimerTramoStr + "</td>";
        cadena = cadena + "<td>" + m.SegundoTramoStr + "</td>";

        if ($("#hdnCrear").val() != undefined || $("#hdnPermisos").val() != undefined) {
            cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_eliminarMod(" + m.IdModalidad + ");' title='Eliminar'><i class='fa fa-remove fa-lg icon-vida'></i></a></td>";
            cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_modificarMod(" + m.IdModalidad + ");' title='Modificar'><i class='fa fa-edit fa-lg icon-vida'></i></a></td>";
        }
        else {
            cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
            cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
        }

        $("#tablaMod tbody").append(cadena);

        idsModalidades.push(String(m.IdModalidad));
    });
}

function doErrorConsultarCot(result) {
    aviso("Aviso", result.Message);
}

// Buscar Asegurado

function doSuccessConsultarAse(result) {
    var asegurados = result.Object;

    if (asegurados.length == 0) {
        aviso("Aviso", "El asegurado no existe");

        var url = $("#urlConsultarLocalidad").val();
        var fields;

        sendValues(null, doSuccessConsultarLocalidad, doErrorConsultarLocalidad, url);
    }
    else {
        var asegurado = asegurados[0];

        $("#txtNombresCot").val(asegurado.Nombres.trim());
        $("#txtApellidoPaternoCot").val(asegurado.ApellidoPaterno.trim());
        $("#txtApellidoMaternoCot").val(asegurado.ApellidoMaterno.trim());
        $("#cmbxDepartamentosCot").val(asegurado.IdDepartamento == 0 ? "" : asegurado.IdDepartamento);
        $("#cmbxSexoCot").val(asegurado.IdSexo);
        $("#txtFechaNacimientoCot").val(asegurado.FechaNacimientoStr);
        $("#txtCUSPPCot").val(asegurado.CUSPP);
        $("#cmbxAfpCot").val(asegurado.IdAfp == 0 ? "" : asegurado.IdAfp);
        $("#cmbxPensionesCot").val(asegurado.IdPension);
        $("#txtCICCot").val(asegurado.Cic);
        $("#txtDevSolCot").val(asegurado.FechaDevengueStr);
        $("#cmbxAsesoresCot").val(asegurado.IdAsesor == 0 ? "" : asegurado.IdAsesor);
        $("#txtRentaAfpMod").val(asegurado.PorAfp);

        $("#cmbxTiposDocumentoCot").val(asegurado.IdTipoDocumento);
        $("#txtDocumentoCot").val(asegurado.Documento);

        $("#pestanaAsegurado").trigger("click");

        idProvincia = asegurado.IdProvincia;
        idDistrito = asegurado.IdDistrito;
        valAfp = asegurado.PorAfp;

        var url = $("#urlConsultarBens").val();
        var tipoDocumento = $("#cmbxTiposDocumentoBus option:selected").text();
        var numeroDocumento = $("#txtDocumentoBus").val().trim();

        var fields = {
            tipoDocumento: tipoDocumento,
            numeroDocumento: numeroDocumento,
            cuspp: asegurado.CUSPP
        };

        sendValues(fields, doSuccessBenJubilare, doErrorBenJubilare, url);

        //Provincias
        if (asegurado.IdDepartamento != 0) {
            var url = $("#urlConsultaProvincias").val();
            var idDepartamento = $("#cmbxDepartamentosCot").val();

            var fields = {
                idDepartamento: idDepartamento
            };

            sendValues(fields, doSuccessConsultaProvincias, doErrorConsultaProvincias, url);
        }

        //Distritos
        if (idProvincia != 0) {
            var url = $("#urlConsultaDistrito").val();
            var idProv = $("#cmbxProvinciasCot").val();

            var fields = {
                idProvincia: idProvincia
            };

            sendValues(fields, doSuccessConsultaDistrito, doErrorConsultaDistrito, url);
        }
    }

}

function doErrorConsultarAse(result) {
    aviso("Aviso", result.Message);
}

// Beneficiarios Jubilare

function doSuccessBenJubilare(result) {
    result.Object.forEach(function (b) {
        if ($('#cmbxParentescosBen').find('option:selected').text() == "TITULAR")
            idTitular = b.IdBeneficiario;

        var nFilas = $("#tablaBen tr").length;
        var cadena = "<tr>";
        cadena = cadena + "<td style = 'display:none'>" + b.IdBeneficiario + "</td>";
        cadena = cadena + "<td>" + nFilas + "</td>";
        cadena = cadena + "<td>" + b.Parentesco + "</td>";
        cadena = cadena + "<td>" + b.TipoDocumento + "</td>";
        cadena = cadena + "<td>" + b.Documento + "</td>";
        cadena = cadena + "<td>" + b.FechaNacimientoStr + "</td>";
        cadena = cadena + "<td>" + b.Sexo + "</td>";
        cadena = cadena + "<td>" + b.SituacionInvalidez + "</td>";
        cadena = cadena + "<td>0%</td>";

        if ($("#hdnCrear").val() != undefined || $("#hdnPermisos").val() != undefined) {
            cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_eliminarBen(" + b.IdBeneficiario + ");' title='Eliminar'><i class='fa fa-remove fa-lg icon-vida'></i></a></td>";
            cadena = cadena + "<td align = 'center'><a href='javascript:fn_dar_modificarBen(" + b.IdBeneficiario + ");' title='Modificar'><i class='fa fa-edit fa-lg icon-vida'></i></a></td>";
        }
        else {
            cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
            cadena = cadena + "<td align = 'center'><a disabled='true' title='No tiene permisos'><i class='fa fa-ban fa-lg icon-vida'></i></a></td>";
        }

        $("#tablaBen tbody").append(cadena);

        idsBeneficiarios.push(String(b.IdBeneficiario));
    });
}

function doErrorBenJubilare(result) {
    aviso("Aviso", result.Message);
}

// Información de Tipo de Documento

function doSuccessTipoDocumento(result) {
    var infoDocumento = result.Object;
    var txtDocumentoCot = document.getElementById("txtDocumentoCot");

    longitudDoc = infoDocumento.Longitud;
    indicadorLongDoc = infoDocumento.IndicadorLongitudExacta;

    document.getElementById("txtDocumentoCot").maxLength = longitudDoc;

    switch (infoDocumento.Tipo) {
        case 'N':
            txtDocumentoCot.removeEventListener("keypress", letras);
            txtDocumentoCot.removeEventListener("keypress", validanumyletras);

            txtDocumentoCot.addEventListener("keypress", soloNumeros, false);
            break;
        case 'A':
            txtDocumentoCot.removeEventListener("keypress", soloNumeros);
            txtDocumentoCot.removeEventListener("keypress", validanumyletras);

            txtDocumentoCot.addEventListener("keypress", letras, false);
            break;
        default:
            txtDocumentoCot.removeEventListener("keypress", letras);
            txtDocumentoCot.removeEventListener("keypress", soloNumeros);

            txtDocumentoCot.addEventListener("keypress", validanumyletras, false);
            break;
    }

}

function doSuccessTipoDocumentoBus(result) {
    var infoDocumento = result.Object;
    var txtDocumentoCot = document.getElementById("txtDocumentoBus");

    longitudDoc = infoDocumento.Longitud;
    indicadorLongDoc = infoDocumento.IndicadorLongitudExacta;

    document.getElementById("txtDocumentoBus").maxLength = longitudDoc;

    switch (infoDocumento.Tipo) {
        case 'N':
            txtDocumentoCot.removeEventListener("keypress", letras);
            txtDocumentoCot.removeEventListener("keypress", validanumyletras);

            txtDocumentoCot.addEventListener("keypress", soloNumeros, false);
            break;
        case 'A':
            txtDocumentoCot.removeEventListener("keypress", soloNumeros);
            txtDocumentoCot.removeEventListener("keypress", validanumyletras);

            txtDocumentoCot.addEventListener("keypress", letras, false);
            break;
        default:
            txtDocumentoCot.removeEventListener("keypress", letras);
            txtDocumentoCot.removeEventListener("keypress", soloNumeros);

            txtDocumentoCot.addEventListener("keypress", validanumyletras, false);
            break;
    }

}

function doErrorTipoDocumento(result) {
    aviso("Aviso", result.Message);
}

function doSuccessTipoAfp(result) {
    var infoafp = result.Object;
    valAfp = infoafp;

    $("#txtRentaAfpMod").val(infoafp);
}

function doErrorTipoAfp(result) {
    aviso("Aviso", result.Message);
}

function doSuccessTipoPension(result) {
    var infoPension = result.Object;

    valPension = infoPension;
}

function doErrorTipoPension(result) {
    aviso("Aviso", result.Message);
}

function doSuccessTipoDocumentoBen(result) {
    var infoDocumento = result.Object;

    longitudDoc = infoDocumento.Longitud;
    indicadorLongDoc = infoDocumento.IndicadorLongitudExacta;

    document.getElementById("txtNumDocumentoBen").maxLength = longitudDoc;
}

function doErrorTipoDocumentoBen(result) {
    aviso("Aviso", result.Message);
}

// Consulta Provincias

function doSuccessConsultaProvincias(result) {
    var cmbxProvincias = document.getElementById("cmbxProvinciasCot");
    $("#cmbxProvinciasCot").empty();
    var cont = 1;

    cmbxProvincias.options[0] = new Option("-- Seleccione --", "");

    result.Object.forEach(function (b) {
        cmbxProvincias.options[cont] = new Option(b.Elemento, b.IdProvincia);
        cont++;
    });

    if (idProvincia != 0) {
        $("#cmbxProvinciasCot").val(idProvincia);
        idProvincia = 0;
    }
}

function doErrorConsultaProvincias(result) {
    aviso("Aviso", result.Message);
}

//Consulta Distrito 

function doSuccessConsultaDistrito(result) {
    var cmbxDistritos = document.getElementById("cmbxDistritosCot");
    $("#cmbxDistritosCot").empty();
    var cont = 1;

    cmbxDistritos.options[0] = new Option("-- Seleccione --", "");

    result.Object.forEach(function (b) {
        cmbxDistritos.options[cont] = new Option(b.Elemento, b.IdDistrito);
        cont++;
    });

    if (idDistrito != 0) {
        $("#cmbxDistritosCot").val(idDistrito);
        idDistrito = 0;
    }
}

function doErrorConsultaDistrito(result) {
    aviso("Aviso", result.Message);
}

// Consulta de Localidades por default

function doSuccessConsultarLocalidad(result) {
    var idDepartamentoLocal = result.Object.idDepartamento;
    var idProvinciaLocal = result.Object.idProvincia;
    var idDistritoLocal = result.Object.idDistrito;
    var provinciasLocal = result.Object.provincias;
    var distritosLocal = result.Object.distritos;

    // Carga de Provincias
    var cmbxProvincias = document.getElementById("cmbxProvinciasCot");
    $("#cmbxProvinciasCot").empty();
    var cont = 1;

    cmbxProvincias.options[0] = new Option("-- Seleccione --", "");

    provinciasLocal.forEach(function (b) {
        cmbxProvincias.options[cont] = new Option(b.Elemento, b.IdProvincia);
        cont++;
    });

    // Carga de Distritos
    var cmbxDistritos = document.getElementById("cmbxDistritosCot");
    $("#cmbxDistritosCot").empty();
    var cont = 1;

    cmbxDistritos.options[0] = new Option("-- Seleccione --", "");

    distritosLocal.forEach(function (b) {
        cmbxDistritos.options[cont] = new Option(b.Elemento, b.IdDistrito);
        cont++;
    });

    $("#txtNombresCot").val("");
    $("#txtApellidoPaternoCot").val("");
    $("#txtApellidoMaternoCot").val("");
    $("#cmbxDepartamentosCot").val(idDepartamentoLocal);
    $("#cmbxProvinciasCot").val(idProvinciaLocal);
    $("#cmbxDistritosCot").val(idDistritoLocal);
    $("#cmbxSexoCot").val("");
    $("#txtFechaNacimientoCot").val("");
    $("#txtCUSPPCot").val("");
    $("#cmbxAfpCot").val("");
    $("#cmbxPensionesCot").val("");
    $("#txtCICCot").val("");
    $("#txtDevSolCot").val("");
    $("#cmbxAsesoresCot").val("");
}

function doErrorConsultarLocalidad(result) {
    aviso("Aviso", result.Message);
}

// Carga de Situación de Invalidez

function doSuccessConsultaSituacionInv(result) {
    var cmbxSituacionInv = document.getElementById("cmbxSitInvalidezBen");
    $("#cmbxSitInvalidezBen").empty();
    var cont = 1;

    cmbxSituacionInv.options[0] = new Option("-- Seleccione --", "");

    result.Object.forEach(function (b) {
        cmbxSituacionInv.options[cont] = new Option(b.Elemento, b.IdSituacionInvalidez);
        cont++;
    });
}

function doErrorConsultaSituacionInv(result) {
    aviso("Aviso", result.Message);
}


function doSuccessConsultaValidacionesMod(result) {

    var aniosDif = result.Object.AniosDiferidos;
    var porcentajeRenTmp = result.Object.PorcentajeRentTmp;
    var aniosGarantizados = result.Object.AniosGarantizados;
    var primTra = result.Object.PrimerTramo;
    var segTram = result.Object.SegundoTramo;

    // Carga de Años Diferidos
    var cmbxAniosDif = document.getElementById("cmbxAniosDiferidosMod");
    $("#cmbxAniosDiferidosMod").empty();
    var cont = 1;

    cmbxAniosDif.options[0] = new Option("-- Seleccione --", "");

    aniosDif.forEach(function (ad) {
        cmbxAniosDif.options[cont] = new Option(ad, ad);
        cont++;
    });

    // Carga de PorcentajeRentaTemporal
    var cmbxPorRenTmp = document.getElementById("cmbxRentaTemporalMod");
    $("#cmbxRentaTemporalMod").empty();
    var cont = 1;

    cmbxPorRenTmp.options[0] = new Option("-- Seleccione --", "");

    porcentajeRenTmp.forEach(function (pr) {
        cmbxPorRenTmp.options[cont] = new Option(pr, pr);
        cont++;
    });

    // Carga de Años Garantizados 
    var cmbxAniosGar = document.getElementById("cmbxAniosGarantizadosMod");
    $("#cmbxAniosGarantizadosMod").empty();
    var cont = 1;

    cmbxAniosGar.options[0] = new Option("-- Seleccione --", "");

    aniosGarantizados.forEach(function (ag) {
        cmbxAniosGar.options[cont] = new Option(ag, ag);
        cont++;
    });

    //Carga de Años Primer Tramo
    var cmbxPrimerTramo = document.getElementById("txtPrimerTramoMod");
    $("#txtPrimerTramoMod").empty();
    var cont = 1;

    cmbxPrimerTramo.options[0] = new Option("-- Seleccione --", "0");

    primTra.forEach(function (pt) {
        cmbxPrimerTramo.options[cont] = new Option(pt, pt);
        cont++;
    });

    //Carga de Porcentaje en Segundo Tramo
    var cmbxSegundoTramo = document.getElementById("txtSegundoTramoMod");
    $("#txtSegundoTramoMod").empty();
    var cont = 1;

    cmbxSegundoTramo.options[0] = new Option("-- Seleccione --", "0");

    segTram.forEach(function (sg) {
        cmbxSegundoTramo.options[cont] = new Option(sg, sg);
        cont++;
    });


    if (cargaCombosVal) {

        $("#cmbxAniosDiferidosMod").val(idAniosRent);
        $("#cmbxRentaTemporalMod").val(idPorRent);
        $("#cmbxAniosGarantizadosMod").val(idAniosGar);
        $("#txtPrimerTramoMod").val(idPrimerT);
        $("#txtSegundoTramoMod").val(idSegundoT);

        cargaCombosVal = true;
    }

    if ($("#cmbxTiposRentaMod option:selected").text() == "RENTA VITALICIA ESCALONADA") {
        $('#txtPrimerTramoMod').attr("disabled", false);
        $('#txtSegundoTramoMod').attr("disabled", false);

        $('#cmbxRentaTemporalMod').attr("disabled", true);

        $('#cmbxRentaTemporalMod').val("0");
        $('#cmbxAniosDiferidosMod').attr("disabled", true);
        $('#cmbxAniosDiferidosMod').val("0");

        if ($("#cmbxModalidadesMod option:selected").text() == "SIMPLE") {
            $('#cmbxAniosGarantizadosMod').val("0");
            $('#cmbxAniosGarantizadosMod').attr("disabled", true);
        }
        else {
            $('#cmbxAniosGarantizadosMod').val("15");
            $('#cmbxAniosGarantizadosMod').attr("disabled", false);
        }
    }

    else if ($("#cmbxTiposRentaMod option:selected").text() == "RENTA VITALICIA") {
        $('#txtPrimerTramoMod').attr("disabled", true);
        $('#txtSegundoTramoMod').attr("disabled", true);

        $('#txtPrimerTramoMod').val("0");
        $('#txtSegundoTramoMod').val("0");


        $('#cmbxAniosDiferidosMod').attr("disabled", true);
        $('#cmbxAniosDiferidosMod').val("0");
        if ($("#cmbxModalidadesMod option:selected").text() == "SIMPLE") {
            $('#cmbxAniosGarantizadosMod').val("0");
            $('#cmbxAniosGarantizadosMod').attr("disabled", true);
        }
        else {
            $('#cmbxAniosGarantizadosMod').val("15");
            $('#cmbxAniosGarantizadosMod').attr("disabled", false);
        }

        $('#cmbxRentaTemporalMod').attr("disabled", true);
        $('#cmbxRentaTemporalMod').val("0");
    }

    else if ($("#cmbxTiposRentaMod option:selected").text() == "RENTA TEMPORAL CON RENTA VITALICIA") {
        $('#txtPrimerTramoMod').attr("disabled", true);
        $('#txtSegundoTramoMod').attr("disabled", true);
        //$('#cmbxAniosDiferidosMod').attr("disabled", false);
        //$('#cmbxRentaTemporalMod').attr("disabled", false);

        $('#txtPrimerTramoMod').val("0");
        $('#txtSegundoTramoMod').val("0");

        $("#cmbxRentaTemporalMod").val("50");
        $('#cmbxAniosDiferidosMod').attr("disabled", false);
        $('#cmbxRentaTemporalMod').attr("disabled", false);

        if ($("#cmbxModalidadesMod option:selected").text() == "SIMPLE") {
            $('#cmbxAniosGarantizadosMod').val("0");
            $('#cmbxAniosGarantizadosMod').attr("disabled", true);
        }
        else {
            $('#cmbxAniosGarantizadosMod').val("15");
            $('#cmbxAniosGarantizadosMod').attr("disabled", false);
        }
    }

}

function doErrorConsultaValidacionesMod(result) {
    aviso("Aviso", result.Message);
}


// Mensaje 

function aviso(header, body, resultado) {
    $('#msg_modal_header').text(header);

    $('#msg_modal_body').text(body);
    $('#sch_modal').modal('show');
    $("#btn_modal_aceptar").one('click', function () {
        $('#sch_modal').modal('hide');
        document.getElementById("cmbxTiposDocumentoCot").focus();
        if (resultado != undefined && resultado != null) {
            if (resultado.bandera == 'C') {
                var url = $("#urlModificar").val();
                window.location.href = url + "?idCotizacion=" + resultado.idCotizacion + "&operacion=1";
            }
        }

    });
}

// Validaciones

function validaCotizacion() {
    // $("#modalCargando").show();
    var url = $("#urlRegistrarCot").val();
    var IdTipoDocumento = $("#cmbxTiposDocumentoCot").val();
    var Documento = $("#txtDocumentoCot").val().trim();
    var Nombres = $("#txtNombresCot").val().trim();
    var ApellidoPaterno = $("#txtApellidoPaternoCot").val().trim();
    var ApellidoMaterno = $("#txtApellidoMaternoCot").val().trim();
    var IdDepartamento = $("#cmbxDepartamentosCot").val();
    var IdProvincia = $("#cmbxProvinciasCot").val();
    var IdDistrito = $("#cmbxDistritosCot").val();
    var IdSexo = $("#cmbxSexoCot").val();
    var FechaNacimiento = $("#txtFechaNacimientoCot").val();
    var CUSPP = $("#txtCUSPPCot").val().trim();
    var IdAfp = $("#cmbxAfpCot").val();
    var IdPension = $("#cmbxPensionesCot").val();
    var Cic = $("#txtCICCot").val().trim();
    var FechaDevengue = $("#txtDevSolCot").val();
    var FechaEstudio = $("#txtFechaEstudioCot").val();
    var IdAsesor = $("#cmbxAsesoresCot").val();
    var GastoSepelio = $("#txtGastoSepelioCot").val().trim();
    var TipoCambio = $("#txtTipoCambio").val();

    if (Nombres == "" || ApellidoPaterno == "" || ApellidoMaterno == "" || IdDepartamento == "" || IdDepartamento == null || IdProvincia == "" || IdProvincia == null
        || IdDistrito == "" || IdDistrito == null || IdSexo == "" || IdSexo == null || FechaNacimiento == "" || CUSPP == "" || IdAfp == "" || IdAfp == null || FechaDevengue == "" || FechaEstudio == ""
        || IdPension == "" || IdPension == null || Cic == "" || IdAsesor == "" || GastoSepelio == "" || TipoCambio == "") {

        aviso("Aviso", "Debe ingresar todos los campos del asegurado");
        return;
    }

    var table, tr, td;
    var titular = true;
    var beneficiarios = false;

    table = document.getElementById("tablaBen");
    tr = table.getElementsByTagName("tr");
    for (var i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            if (tr[i].getElementsByTagName("td")[2].innerText == "") {
                beneficiarios = true;
            }
            else {
                if (tr[i].getElementsByTagName("td")[2].innerText == "TITULAR")
                    titular = false;
            }

        }
    }
    if (beneficiarios) {
        aviso("Aviso", "Debe ingresar el parentesco de todos los Beneficiarios");
        return;
    }

    if (titular) {
        aviso("Aviso", "Favor de ingresar un Beneficiario Titular");
        return;
    }

    if ($("#cmbxPensionesCot option:selected").text() == "SOBREVIVENCIA") {
        if (idsBeneficiarios.length <= 1) {
            aviso("Aviso", "Favor de ingresar Beneficiarios");
            return;
        }
    }

    if (idsModalidades.length < 1) {
        aviso("Aviso", "Debe ingresar por lo menos una modalidad");
        return;
    }

    $('#modalcargar').modal({
        drop: 'static',
        keyboard: false,
        show: true
    });

    var fields = {
        bandera: banderaCot,
        IdTipoDocumento: IdTipoDocumento,
        Documento: Documento,
        Nombres: Nombres,
        ApellidoPaterno: ApellidoPaterno,
        ApellidoMaterno: ApellidoMaterno,
        IdDepartamento: IdDepartamento,
        IdProvincia: IdProvincia,
        IdDistrito: IdDistrito,
        IdSexo: IdSexo,
        FechaNacimiento: FechaNacimiento,
        CUSPP: CUSPP,
        IdAfp: IdAfp,
        IdPension: IdPension,
        Cic: Cic,
        FechaDevengue: FechaDevengue,
        FechaEstudio: FechaEstudio,
        IdAsesor: IdAsesor,
        GastoSepelio: GastoSepelio,
        idsBeneficiarios: idsBeneficiarios,
        idsModalidades: idsModalidades,
        IdCotizacion: idCotizacion,
        TipoCambio: TipoCambio
    };

    sendValues(fields, doSuccessRegistrarCot, doErrorRegistrarCot, url);

}

function soloLetras(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
    especiales = "8-37-39-46";

    tecla_especial = false
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}

function filterFloat(evt, input, decimales) {
    // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;

    if (key >= 48 && key <= 57) {
        if (filter(tempValue, decimales) === false) {
            return false;
        } else {
            return true;
        }
    } else {
        if (key == 8 || key == 13 || key == 0) {
            return true;
        } else if (key == 46) {
            if (filter(tempValue, decimales) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    }
}

function filter(val, decimales) {
    var preg;
    if (decimales == 2)
        preg = /^([0-9]+\.?[0-9]{0,2})$/;
    else
        preg = /^([0-9]+\.?[0-9]{0,3})$/;

    if (preg.test(val) === true) {
        return true;
    } else {
        return false;
    }

}

function IsNumeric(valor) {
    var log = valor.length; var sw = "S";
    for (x = 0; x < log; x++) {
        v1 = valor.substr(x, 1);
        v2 = parseInt(v1);
        //Compruebo si es un valor numérico 
        if (isNaN(v2)) { sw = "N"; }
    }
    if (sw == "S") { return true; } else { return false; }
}

var primerslap = false;
var segundoslap = false;

function formateafecha(fecha) {
    var long = fecha.length;
    var dia;
    var mes;
    var ano;
    if ((long >= 2) && (primerslap == false)) {
        dia = fecha.substr(0, 2);
        if ((IsNumeric(dia) == true) && (dia <= 31) && (dia != "00")) { fecha = fecha.substr(0, 2) + "/" + fecha.substr(3, 7); primerslap = true; }
        else { fecha = ""; primerslap = false; }
    }
    else {
        dia = fecha.substr(0, 1);
        if (IsNumeric(dia) == false)
        { fecha = ""; }
        if ((long <= 2) && (primerslap = true)) { fecha = fecha.substr(0, 1); primerslap = false; }
    }
    if ((long >= 5) && (segundoslap == false)) {
        mes = fecha.substr(3, 2);
        if ((IsNumeric(mes) == true) && (mes <= 12) && (mes != "00")) { fecha = fecha.substr(0, 5) + "/" + fecha.substr(6, 4); segundoslap = true; }
        else { fecha = fecha.substr(0, 3);; segundoslap = false; }
    }
    else { if ((long <= 5) && (segundoslap = true)) { fecha = fecha.substr(0, 4); segundoslap = false; } }
    if (long >= 7) {
        ano = fecha.substr(6, 4);
        if (IsNumeric(ano) == false) { fecha = fecha.substr(0, 6); }
        else { if (long == 10) { if ((ano == 0) || (ano < 1900) || (ano > 2100)) { fecha = fecha.substr(0, 6); } } }
    }
    if (long >= 10) {
        fecha = fecha.substr(0, 10);
        dia = fecha.substr(0, 2);
        mes = fecha.substr(3, 2);
        ano = fecha.substr(6, 4);
        // Año no viciesto y es febrero y el dia es mayor a 28 
        if ((ano % 4 != 0) && (mes == 02) && (dia > 28)) { fecha = fecha.substr(0, 2) + "/"; }
        if ((mes == 02) && (dia > 29)) { fecha = fecha.substr(0, 2) + "/"; }
    }
    return (fecha);
}

function obtenerValorParametro(sParametroNombre) {
    var sPaginaURL = window.location.search.substring(1);
    var sURLVariables = sPaginaURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParametro = sURLVariables[i].split('=');
        if (sParametro[0] == sParametroNombre) {
            return sParametro[1];
        }
    }
    return null;
}

function Reporte(idCotizacionReporte) {
    var url = $("#urlReporte").val();
    window.open(url + "?idCotizacion=" + idCotizacionReporte + "&reporte=1");
}

function ReporteT(idCotizacionReporte) {
    var url = $("#urlReporteT").val();
    window.open(url + "?idCotizacion=" + idCotizacionReporte + "&reporte=1");
}

function NumCheck(e, field, decimales) {
    var key = window.Event ? e.which : e.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = field.value + chark;
    if (parseFloat(tempValue) > 100) {
        field.value = "";
        return false;
    }
    else {
        if (key >= 48 && key <= 57) {

            if (filter(tempValue, decimales) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            if (key == 8 || key == 13 || key == 0) {
                return true;
            } else if (key == 46) {
                if (filter(tempValue, decimales) === false) {
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }
    }
}

function validanumyletras(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patron de entrada, en este caso solo acepta numeros y letras
    patron = /[A-Za-z0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

function soloNumeros(e) {
    var key = window.event ? e.which : e.keyCode;
    if (key < 48 || key > 57) {
        e.preventDefault();
    }
}

function letras(e) {
    var key = window.event ? e.which : e.keyCode;
    if ((key < 65 || key > 90) && (key < 97 || key > 122)) {
        e.preventDefault();
    }
}


function validaAgregarTitular() {
    var url = $("#urlRegistrarCot").val();
    var IdTipoDocumento = $("#cmbxTiposDocumentoCot").val();
    var Documento = $("#txtDocumentoCot").val().trim();
    var Nombres = $("#txtNombresCot").val().trim();
    var ApellidoPaterno = $("#txtApellidoPaternoCot").val().trim();
    var ApellidoMaterno = $("#txtApellidoMaternoCot").val().trim();
    var IdDepartamento = $("#cmbxDepartamentosCot").val();
    var IdProvincia = $("#cmbxProvinciasCot").val();
    var IdDistrito = $("#cmbxDistritosCot").val();
    var IdSexo = $("#cmbxSexoCot").val();
    var FechaNacimiento = $("#txtFechaNacimientoCot").val();
    var CUSPP = $("#txtCUSPPCot").val().trim();
    var IdAfp = $("#cmbxAfpCot").val();
    var IdPension = $("#cmbxPensionesCot").val();
    var Cic = $("#txtCICCot").val().trim();
    var FechaDevengue = $("#txtDevSolCot").val();
    var FechaEstudio = $("#txtFechaEstudioCot").val();
    var IdAsesor = $("#cmbxAsesoresCot").val();
    var GastoSepelio = $("#txtGastoSepelioCot").val().trim();
    var TipoCambio = $("#txtTipoCambio").val();

    if (IdTipoDocumento == "" || Documento == "" || Nombres == "" || ApellidoPaterno == "" || ApellidoMaterno == "" || IdDepartamento == "" || IdDepartamento == null || IdProvincia == "" || IdProvincia == null
        || IdDistrito == "" || IdDistrito == null || IdSexo == "" || IdSexo == null || FechaNacimiento == "" || CUSPP == "" || IdAfp == "" || IdAfp == null || FechaDevengue == "" || FechaEstudio == ""
        || IdPension == "" || IdPension == null || Cic == "" || IdAsesor == "" || GastoSepelio == "" || TipoCambio == "") {

        aviso("Aviso", "Debe ingresar todos los campos del asegurado");
        return;
    } else {
        fn_agregarBenTitular();

    }

}
function pulsar(e) {
    e.preventDefault();
    if (e.keyCode === 13) {
        document.getElementById("btnBuscarAsegurado").click();

    }
}

function pulsarmodal(e) {
    e.preventDefault();
    if (e.keyCode === 13) {
        document.getElementById("btn_modal_aceptar").click();

    }
}

function pulsarBeneficiarios(e) {
    e.preventDefault();
    if (e.keyCode === 13) {
        document.getElementById("saveImage").click();

    }
}

function pulsarModalidades(e) {
    e.preventDefault();
    if (e.keyCode === 13) {
        document.getElementById("saveImageModalidad").click();

    }
}
function validaEdad() {
    //var today = new Date();
    //Fecha de Nacimiento
    var FechaNacimiento = $("#txtFechaNacimientoBen").val();
    var dd = FechaNacimiento.substring(0, 2);
    var mm = FechaNacimiento.substring(3, 5);
    var yyyy = FechaNacimiento.substring(6, 10);

    //FechaDevengue
    var FechaDevengue = $("#txtDevSolCot").val();
    var ddFD = FechaDevengue.substring(0, 2);
    var mmFD = FechaDevengue.substring(3, 5);
    var yyyyFD = FechaDevengue.substring(6, 10);

    //Validación de Fecha de Nacimiento
    if (dd < 10)
        dd = '0' + dd

    if (mm < 10)
        mm = '0' + mm

    FechaNacimiento = mm + '/' + dd + '/' + yyyy;

    //Validación de Fecha de Devengue
    if (ddFD < 10)
        ddFD = '0' + ddFD

    if (mmFD < 10)
        mmFD = '0' + mmFD

    FechaDevengue = mmFD + '/' + ddFD + '/' + yyyyFD;

    var devengueDate = new Date(FechaDevengue);

    //Validación de Edad.
    var birthDate = new Date(FechaNacimiento);
    var age = devengueDate.getFullYear() - birthDate.getFullYear();
    var m = devengueDate.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && devengueDate.getDate() < birthDate.getDate())) {
        age = age - 1;
    }
    if (age >= 28) {
        return true;
    }
    else
        return false;
}

function validaTeclaEscMod(e) {
    e.preventDefault();
    if (e.keyCode === 27) {
        document.getElementById("btnCerrarMod").click();
    }
}

function validaTeclaEscBen(e) {
    e.preventDefault();
    if (e.keyCode === 27) {
        document.getElementById("btnCerrarBen").click();
    }
}

function validaFechaDevengue() {
    var hoy = new Date();
    var FechaDevengue = $("#txtDevSolCot").val();
    var dd = FechaDevengue.substring(0, 2);
    var mm = FechaDevengue.substring(3, 5);
    var yyyy = FechaDevengue.substring(6, 10);

    if (dd < 10)
        dd = '0' + dd

    if (mm < 10)
        mm = '0' + mm

    FechaDevengue = mm + '/' + dd + '/' + yyyy;

    var devengueDate = new Date(FechaDevengue);
    // Comparamos solo las fechas => no las horas!!
    hoy.setHours(0, 0, 0, 0);  // Lo iniciamos a 00:00 horas

    if (hoy <= FechaDevengue) {
        FechaDevengue = hoy;
        return true
    }
    else {
        return false
    }
}

