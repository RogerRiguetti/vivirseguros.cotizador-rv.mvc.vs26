var fechaLimite = "9999-12-31";
var valTipoMoneda="";

(function () {
    localStorage.pagina = 'TasaCalce';
    cargaTasasDesc();
    //$("#txtTipoMon").val($('select[name="cmbxTipoMoneda"] option:selected').text());
    

    //$("#AK").on('paste', function (e) {
    //    e.preventDefault();
    //});
    //$("#BK").on('paste', function (e) {
    //    e.preventDefault();
    //});

    //$("#CK").on('paste', function (e) {
    //    e.preventDefault();
    //});

    $("#cmbxTipoMoneda").change(function () {
        //$("#txtTipoMon").val($('select[name="cmbxTipoMoneda"] option:selected').text());
        var tipoMoneda = $("#cmbxTipoMoneda").val();
        $('#fecIni').attr("disabled", false);
        $('#fecIni').val("");

        if (tipoMoneda != "") {
            consultaPeriodos();
        } else {
            var cmbxPeriodos = document.getElementById("cmbxPeriodos");
            $("#cmbxPeriodos").empty();

            cmbxPeriodos.options[0] = new Option("-- Seleccione --", "");
        }
    });

    $("#cmbxPeriodos").change(function () {
        if ($("#cmbxPeriodos").val() == "") {
            $('#fecIni').attr("disabled", false);
        } else {
            $('#fecIni').attr("disabled", true);
            $('#fecIni').val("");
        }
        /*var url = $("#urlConsultaTasaDescAnual").val();
        var tipoMoneda = $("#cmbxTipoMoneda").val();
        var periodo = $("#cmbxPeriodos").val();

        var fields = {
            tipoMoneda: tipoMoneda,
            periodo: periodo
        }

        sendValues(fields, doSuccessCargaTasasDesc, doErrorCargaTasasDesc, url);

        var periodo = $("#cmbxPeriodos").val();

        if (periodo != "") {
            $("#fecIni").attr("disabled", true);
        } else {
            $("#fecIni").attr("disabled", false);
        }*/

        
    });

    $("#btnBuscar").click(function () {
        var urlFecha = $("#urlConsultaPeriodoVigencia").val();
        var urlCombo = $("#urlConsultaTasaDescAnual").val();
        var periodo = $("#cmbxPeriodos").val();
        valTipoMoneda = $("#cmbxTipoMoneda").val();
        var fechaIni = $("#fecIni").val();

        if (valTipoMoneda == "") {
            aviso("Aviso", "Debe Ingresar el Tipo de Moneda");
            return;
        }

        if (periodo == "" && fechaIni == "" && valTipoMoneda != "") {
            aviso("Aviso", "Debe Seleccionar un Periodo o Ingresar la Fecha de Inicio de vigencia");
            return;
        }

        if (periodo != "" && valTipoMoneda != "") {
            var periodoVigencia = periodo.split('*');
            var fechaInicio = periodoVigencia[0].trim().split('/');
            var fechaFin = periodoVigencia[1].trim().split('/');

            var fechaInicio = periodoVigencia[0].trim().split('/');
            var fechaFin = periodoVigencia[1].trim().split('/');

            $("#fecIniRes").val(fechaInicio[2] + "-" + fechaInicio[1] + "-" + fechaInicio[0]);
            $("#fecFinRes").val(fechaFin[2] + "-" + fechaFin[1] + "-" + fechaFin[0]);
            $("#txtTipoMon").val($('select[name="cmbxTipoMoneda"] option:selected').text());
            valTipoMoneda = $("#cmbxTipoMoneda").val();

            var fields = {
                tipoMoneda: valTipoMoneda,
                periodo: periodo
            }

            sendValues(fields, doSuccessCargaTasasDesc, doErrorCargaTasasDesc, urlCombo);
        }
        
        if (fechaIni != "" && valTipoMoneda != "") {
            var fechaInicio = $("#fecIni").val();
            $("#fecIniRes").val($("#fecIni").val());
            $("#txtTipoMon").val($('select[name="cmbxTipoMoneda"] option:selected').text());
            valTipoMoneda = $("#cmbxTipoMoneda").val();

            var fields = {
                tipoMoneda: valTipoMoneda,
                fechaInicio: fechaInicio
            }

            sendValues(fields, doSuccessConsultaPeriodoVigencia, doErrorConsultaPeriodoVigencia, urlFecha);
        }

    });

    $("#btnActualizar").click(function () {
        var tramo = $("#Tramo").val();
        var ak = $("#AK").val();
        var bk = $("#BK").val();
        var ck = $("#CK").val();
        var cpk = $("#CPK").val();

        var table, tr, td;

        table = document.getElementById("tblConsultaDescuentoAnual");
        tr = table.getElementsByTagName("tr");
        for (var i = 0; i < tr.length; i++) {
            td = tr[i].getElementsByTagName("td")[0];
            if (td) {
                if (td.innerHTML == tramo) {
                    tr[i].getElementsByTagName("td")[1].innerText = ak;
                    tr[i].getElementsByTagName("td")[2].innerText = bk;
                    tr[i].getElementsByTagName("td")[3].innerText = ck;
                    tr[i].getElementsByTagName("td")[4].innerText = cpk;
                }
            }
        }

        $("#Tramo").val("");
        $("#AK").val("");
        $("#BK").val("");
        $("#CK").val("");
        $("#CPK").val("");
    });

    $("#btnGuardar").click(function () {
        var url = $("#urlVerificacionTasaCalce").val();
        var tipoMoneda = $("#txtTipoMon").val();
        var periodo = $("#cmbxPeriodos").val();
        var vigencia = $("#fecIniRes").val() + "*" + $("#fecFinRes").val();

        if (tipoMoneda == "") {
            aviso("Aviso", "No se ha realizado ninguna búsqueda");
            return;
        }

        if ($("#fecIniRes").val() == "") {
            aviso("Aviso", "Debe seleccionar un Rango de Vigencia");
            return;
        }

        var fields = {
            tipoMoneda: valTipoMoneda,
            periodo: vigencia,
            eliminar: 0
        }

        sendValues(fields, doSuccessVerificacionPeriodo, doErrorVerificacionPeriodo, url);
    });

    $("#btnEliminar").click(function () {
        var tipoMoneda = $("#txtTipoMon").val();
        var fechaInicio = $("#fecIniRes").val();
        var fechaFin = $("#fecFinRes").val();

        if (tipoMoneda == "") {
            aviso("Aviso", "No se ha realizado ninguna búsqueda");
            return;
        }

        if (fechaInicio == "" || fechaFin == "") {
            aviso("Aviso", "Debe seleccionar un Rango de Vigencia para Eliminar");
            return;
        }

        var url = $("#urlVerificacionTasaCalce").val();
        var vigencia = $("#fecIniRes").val() + "*" + $("#fecFinRes").val();

        var fields = {
            tipoMoneda: valTipoMoneda,
            periodo: vigencia,
            eliminar: 1
        }

        sendValues(fields, doSuccessVerificarEliminacion, doErrorVerificarEliminacion, url);
    });

    $("#btnImprimir").click(function () {
        var url = $("#urlReporte").val();

        var periodo = $("#fecIniRes").val();
        var tipoMoneda = $("#txtTipoMon").val();
     

        if (tipoMoneda == "") {
            aviso("Aviso", "No se ha realizado ninguna búsqueda");
            return;
        }

        if (periodo == "" ) {
            aviso("Aviso", "Debe seleccionar un Rango de Vigencia");
            return;
        }

        var fechaInicio = periodo.replace(new RegExp('-','g'), '');

        var tipoMonedaArray = valTipoMoneda.split('#');
        var codigoMoneda = tipoMonedaArray[0];
        var reajuste = tipoMonedaArray[1];

        window.open(url + "?fechaInicio=" + fechaInicio + "&codigoMoneda=" + codigoMoneda + "&reajuste=" + reajuste);
    });

    $("#btnLimpiar").click(function () {
        limpiar();
    });

    $("#btnSalir").click(function () {
        var url = $("#urlSalir").val();
        window.location.href = url;
    });

})();

// Consulta de Periodos

function consultaPeriodos() {
    var url = $("#urlConsultaPeriodos").val();
    var tipoMoneda = $("#cmbxTipoMoneda").val();

    var fields = {
        tipoMoneda: tipoMoneda
    }

    sendValues(fields, doSuccessConsultaPeriodos, doErrorConsultaPeriodos, url);
}

function doSuccessConsultaPeriodos(result) {
    var cmbxPeriodos = document.getElementById("cmbxPeriodos");
    $("#cmbxPeriodos").empty();
    var cont = 1;

    cmbxPeriodos.options[0] = new Option("-- Seleccione --", "");

    result.Object.forEach(function (b) {
        cmbxPeriodos.options[cont] = new Option(b.Elemento, b.Elemento);
        cont++;
    });
}

function doErrorConsultaPeriodos(result) {
    aviso("Aviso", result.Message);
}

// Carga de la tabla de Tasas de Descuento Anual

function cargaTasasDesc() {
    var url = $("#urlConsultaTasaDescAnual").val();

    var fields = {
        tipoMoneda: "",
        periodo: ""
    }

    sendValues(fields, doSuccessCargaTasasDesc, doErrorCargaTasasDesc, url);
}

function doSuccessCargaTasasDesc(result) {
    var tasasDesc = result.Object;

    $("#tblConsultaDescuentoAnual tbody").empty();

    tasasDesc.forEach(function (t) {
        var nFilas = $("#tblConsultaDescuentoAnual tr").length;

        var cadena = "<tr id='" +  t.Tramo +"' onClick='javascript:fn_cargaTramo(" + t.Tramo + ");'>";
        cadena = cadena + "<td>" +  t.Tramo+ "</td>";
        cadena = cadena + "<td>" + parseFloat( t.AK ).toFixed(3)+ "</td>";
        cadena = cadena + "<td>" + parseFloat( t.BK ).toFixed(3)+ "</td>";
        cadena = cadena + "<td>" + parseFloat( t.CK ).toFixed(3)+ "</td>";
        cadena = cadena + "<td>" + t.CPK+ "</td>";

        $("#tblConsultaDescuentoAnual tbody").append(cadena);

    });
}

function doErrorCargaTasasDesc(result) {
    aviso("Aviso", result.Message);
}

// Carga de Tramo

function fn_cargaTramo(tramo){
    var url = $("#urlConsultaTramo").val();
    var tipoMoneda = $("#cmbxTipoMoneda").val();
    var periodo = $("#cmbxPeriodos").val();
    var vigencia = $("#fecIni").val() + "*" + $("#fecFin").val();



    $("#tblConsultaDescuentoAnual tr").click(function () {
        $("#tblConsultaDescuentoAnual tr").removeClass('focus');
        $(this).addClass('focus');
    });

    //if (document.getElementById("fecIni").disabled == true) {
        var tabla = document.getElementById("tblConsultaDescuentoAnual");

        $("#Tramo").val(tabla.rows[tramo].cells[0].innerText);
        $("#AK").val(tabla.rows[tramo].cells[1].innerText);
        $("#BK").val(tabla.rows[tramo].cells[2].innerText);
        $("#CK").val(tabla.rows[tramo].cells[3].innerText);
        $("#CPK").val(tabla.rows[tramo].cells[4].innerText);

        /*var fields = {
            tramo: tramo,
            tipoMoneda: tipoMoneda,
            periodo: periodo == "" ? vigencia : periodo
        }

        sendValues(fields, doSuccessConsultaTramo, doErrorConsultaTramo, url);*/
    //}
}

function doSuccessConsultaTramo(result) {
    var t = result.Object;

    if (t.Tramo != 0) {
        $("#Tramo").val(t.Tramo);
        $("#AK").val(t.AK);
        $("#BK").val(t.BK);
        $("#CK").val(t.CK);
        $("#CPK").val(t.CPK);
    }
}

function doErrorConsultaTramo(result) {
    aviso("Aviso", result.Message);
}

// Cálculo de CPK

function calculoCPK() {
    var ak = $("#AK").val();
    var bk = $("#BK").val();
    var ck = $("#CK").val();

    var calculoAK = ak > 0 ? ak : 0;
    var calculoBK = bk < calculoAK ? bk : calculoAK;
    var calculoCPK = 0;

    if (bk == 0)
        $("#CPK").val("0.00000000");
    else {
        calculoCPK = calculoBK / bk;
        $("#CPK").val(calculoCPK.toFixed(8));
    }
}

// Consulta de Periodo de Vigencia

function doSuccessConsultaPeriodoVigencia(result) {
    var fechaTermino = result.Object.fechaTermino;
    var tasasDesc = result.Object.tasasDesc;

    if (fechaTermino == "") {
        var header = "Confirmar";
        var body = "La Fecha Indicada no se encuentra registrada \n ¿Desea Ingresar esta Nueva Fecha de Vigencia?";
        confirmarTasaCalce(header, body, function () { cargaFechaTermino(tasasDesc); });
    }
    else {
        $("#fecFin").val(fechaTermino);
        $("#fecFinRes").val(fechaTermino);
        //$("#fecIni").attr("disabled", true);

        var url = $("#urlConsultaTasaDescAnual").val();
        var tipoMoneda = $("#cmbxTipoMoneda").val();
        var periodo = $("#fecIni").val() + "*" + $("#fecFin").val(fechaTermino);

        var fields = {
            tipoMoneda: tipoMoneda,
            periodo: periodo
        }

        sendValues(fields, doSuccessCargaTasasDesc, doErrorCargaTasasDesc, url);
    }
}

function cargaFechaTermino(tasasDesc) {
    $("#fecFin").val(fechaLimite);
    $("#fecFinRes").val(fechaLimite);
    //$("#fecIni").attr("disabled", true);

    var result = { Object: tasasDesc };

    doSuccessCargaTasasDesc(result);
}

function doErrorConsultaPeriodoVigencia(result) {
    aviso("Aviso", result.Message);
}

// Verificación de Periodo

function doSuccessVerificacionPeriodo(result) {
    var clave = result.Object;
    var header = "Confirmar";
    var body = result.Message;

    confirmarTasaCalce(header, body, function () { registrarModificarTasa(clave); });
}

function doErrorVerificacionPeriodo(result) {
    aviso("Aviso", result.Message);
}

function registrarModificarTasa(clave) {
    var tasas = [];
    var tasa;

    var table, tr, td;

    table = document.getElementById("tblConsultaDescuentoAnual");
    tr = table.getElementsByTagName("tr");
    for (var i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            tasa = {
                Tramo: tr[i].getElementsByTagName("td")[0].innerText,
                AK: tr[i].getElementsByTagName("td")[1].innerText,
                BK: tr[i].getElementsByTagName("td")[2].innerText,
                CK: tr[i].getElementsByTagName("td")[3].innerText,
                CPK: tr[i].getElementsByTagName("td")[4].innerText
            }

            tasas.push(tasa);
        }
    }

    var url = $("#urlRegModTasaCalce").val();
    var vigencia = $("#fecIniRes").val() + "*" + $("#fecFinRes").val();

    var fields = {
        clave: clave,
        tasas: tasas,
        tipoMoneda: valTipoMoneda,
        periodo: vigencia
    }

    sendValues(fields, doSuccessRegModTasa, doErrorRegModTasa, url);
}

// Registro y Modificación de Tasa

function doSuccessRegModTasa(result) {
    aviso("Aviso", result.Message);
    limpiar();
}

function doErrorRegModTasa(result) {
    aviso("Aviso", result.Message);
}

// Eliminación de Tasa

function doSuccessVerificarEliminacion(result) {
    var clave = result.Object;
    var mensaje = result.Message;

    if (clave == "Confirmar") 
        confirmarTasaCalce(clave, mensaje, function () { eliminarTasa(); });
    else 
        aviso(clave, mensaje);
}

function doErrorVerificarEliminacion(result) {
    aviso("Aviso", result.Message);
}

function eliminarTasa() {
    var url = $("#urlEliminarTasaCalce").val();
    var vigencia = $("#fecIniRes").val() + "*" + $("#fecFinRes").val();

    var fields = {
        tipoMoneda: valTipoMoneda,
        periodo: vigencia
    }

    sendValues(fields, doSuccessEliminarTasa, doErrorEliminarTasa, url);
}

function doSuccessEliminarTasa(result) {
    aviso("Aviso", result.Message);
    limpiar();
}

function doErrorEliminarTasa(result) {
    aviso("Aviso", result.Message);
}

// Limpiar Pantalla

function limpiar() {
    //$("#cmbxTipoMoneda").attr("disabled", false);
    //$("#fecIni").attr("disabled", false);
    $("#cmbxPeriodos").val("");
    $("#cmbxTipoMoneda").val("");
    $("#fecIni").val("");
    $("#fecFin").val("");
    $("#Tramo").val("");
    $("#AK").val("");
    $("#BK").val("");
    $("#CK").val("");
    $("#CPK").val("");

    $("#fecIniRes").val("");
    $("#fecFinRes").val("");
    $("#txtTipoMon").val("");

    //consultaPeriodos();
    cargaTasasDesc();
}

// Mensaje

function aviso(header, body) {
    $('#msg_modal_header').text(header);

    $('#msg_modal_body').text(body);
    $('#sch_modal').modal('show');
    $("#btn_modal_aceptar").one('click', function () {
        $('#sch_modal').modal('hide');
    });
}

function confirmarTasaCalce(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}

// Validaciones

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

//FUNCIÓN PARA DECIMALES
function validaDecimales(cifra, decimales) {
    var enterolong = cifra.length;
    var entero;
    var pos = 0;
    var numdecimal;
    var falla = "N";

    if ((enterolong == 1) && (IsNumeric(cifra) == false)) {
        cifra = "";
    }

    if ((enterolong > 1)) {
        if (cifra.includes(".") === true) {
            pos = cifra.indexOf(".");
            entero = cifra.substr(0, pos + 1);
            numdecimal = cifra.substr(pos + 1, decimales);

            if ((IsNumeric(entero.replace(".", "")) === false) || (IsNumeric(numdecimal) === false)) {
                falla = "S";
            }
        }
    }

    if (numdecimal == null) {
        if ((IsNumeric(cifra) === false) && (pos == 0)) {
            cifra = "";
        }
    } else {
        if (IsNumeric(numdecimal) === false) {
            cifra = cifra.replace(/(?!-)[^0-9.]/g, "");
            //cifra = entero;
        } else {
            cifra = entero + numdecimal;
        }
    }

    if (falla == "S") {
        cifra = cifra.replace(/(?!-)[^0-9.]/g, "");
    }

    return (cifra);
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