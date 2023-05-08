$(document).ready(function () {
    $('#tblConsulta').DataTable({
        searching: false,
        bSort: false,
        paging: false,
        info: false,
        language: { search: '', searchPlaceholder: "Buscar" }
    });

    $('#tblCalculo').DataTable({
        searching: false,
        bSort: false,
        paging: false,
        info: false,
        language: { search: '', searchPlaceholder: "Buscar" }
    });
});

var checkVal;
var banCheck = false;
var numCot;
var numArchivo;
var numCor = "";
var sumPen = "";
var clic = 1;
var arrayCalculo = [];
(function () {
    localStorage.pagina = 'Mejoras';
    let input = document.getElementById('txtCUSPP');
    input.addEventListener('keyup', onKeyUpHandler);

    //var select = document.getElementById("txtNuevaCO");
    //var length = select.options.length;
    //for (i = 0; i < length; i++) {
    //    select.options[i] = null;
    //}
})();

function cargaDetabla(datos, operacion) {
    

    //var select = document.getElementById("txtNuevaCO");
    //var length = select.options.length;
    //for (i = 0; i < length; i++) {
    //    select.options[i] = null;
    //}
    var nombCol = ["strMoneda", "strModalidad", "strPeriodo", "strRenta", "strGarantizado", "strEscalonada", "strPrima", "strTemporal", "strVitalicia", "strTasa", "strTIR", "strPerdida", "strComisión", "strNumCorrelativo", "strMtoPension", "strSumPen", "strCodTipReajuste"];
    var inicio = 0;
    if (operacion == "busqueda") { inicio = 1; }
    for (var i = inicio; i < datos.length; i++) {
        var trDatos = document.createElement('tr');
        trDatos.setAttribute("class", "bordeBajo");
        for (var j = 0; j < datos[i].length; j++) {
            if (j < 17) {
                var thDato = document.createElement('td');
                thDato.setAttribute("class", nombCol[j]);
                if (j >= 13) { thDato.setAttribute("style", "display:none;"); }
                thDato.append(datos[i][j]);
                trDatos.appendChild(thDato);
            } else {
                arrayCalculo.push(datos[i][j]);
            }
        }
        if (operacion == "busqueda") {
            var radio = document.createElement('input');
            radio.setAttribute("id", "check" + (i - 1));
            radio.setAttribute("name", "check");
            radio.setAttribute("type", "radio");
            radio.setAttribute("value", (i - 1));
            var label = document.createElement('label');
            label.setAttribute("for", "check" + (i - 1));
            var thDato = document.createElement('th');
            thDato.setAttribute("class", "radioBtn");
            var span = document.createElement('span');
            thDato.appendChild(radio);
            label.appendChild(span);
            thDato.appendChild(label)
            trDatos.appendChild(thDato);
            document.getElementById('RConsultaBody').appendChild(trDatos);
        } else {
            document.getElementById('RCalculoBody').appendChild(trDatos);
        }
    }
    $("input[name=check]").click(function () {

        checkVal = $('input:radio[name=check]:checked').val();
        banCheck = true;

        var tabla = document.getElementById("tblConsulta");
        var strComisión = tabla.getElementsByClassName("strComisión");

        $('#txtNuevaCO').val(strComisión[checkVal].innerHTML)
        var param = $('input:radio[name=rdParametros]:checked').val();

        limpiar("reCalculo");
        var dato;
        var tabla = document.getElementById("tblConsulta");
        var strMoneda = tabla.getElementsByClassName("strMoneda");
        var strModalidad = tabla.getElementsByClassName("strModalidad");
        var strPeriodo = tabla.getElementsByClassName("strPeriodo");
        var strRenta = tabla.getElementsByClassName("strRenta");
        var strGarantizado = tabla.getElementsByClassName("strGarantizado");
        var strEscalonada = tabla.getElementsByClassName("strEscalonada");
        var strPrima = tabla.getElementsByClassName("strPrima");
        var strTemporal = tabla.getElementsByClassName("strTemporal");
        var strVitalicia = tabla.getElementsByClassName("strVitalicia");
        var strTasa = tabla.getElementsByClassName("strTasa");
        var strTIR = tabla.getElementsByClassName("strTIR");
        var strPerdida = tabla.getElementsByClassName("strPerdida");
        var strComisión = tabla.getElementsByClassName("strComisión");
        var strCodTipReajuste = tabla.getElementsByClassName("strCodTipReajuste");
        var strNumCorrelativo = tabla.getElementsByClassName("strNumCorrelativo");
        numCor = strNumCorrelativo[checkVal].innerHTML;
        var strMtoPension = tabla.getElementsByClassName("strMtoPension");
        sumPen = strMtoPension[checkVal].innerHTML;
        

            cic= $("#txtCIC").val(),
            moneda= strMoneda[checkVal].innerHTML,
            numOperacion = $("#txtNumOperacion2").val(),
            cod_tipreajuste= strCodTipReajuste[checkVal].innerHTML
        
        var url = $("#urlLlenarCombo").val();
        var fields = {
            parametro: param,
            cic: cic,
            codMoneda: moneda,
            numOp: numOperacion,
            codReaj: cod_tipreajuste
        };
        sendValues(fields, doSuccessLlenarCombo, doError, url);
    });

}
function doSuccessLlenarCombo(result) {
    var tabla = document.getElementById("tblConsulta");
    var strComisión = tabla.getElementsByClassName("strComisión");
    // Carga de Rangos de comisiones
    var rangosCom = document.getElementById("txtNuevaCO");
    $("#txtNuevaCO").empty();
    var cont = 1;

    rangosCom.options[0] = new Option("-- Seleccione --", "");

    result.Object.forEach(function (ad) {
        rangosCom.options[cont] = new Option(ad, ad);
        cont++;
    });
    $('#txtNuevaCO').val(strComisión[checkVal].innerHTML);
}
function busqueda() {
    document.getElementById("datosDiv").style.display = "none";
    clic = 1;
    if ($('#txtNumOperacion').val() != "" || $('#txtCUSPP').val() != "") {
        limpiar("todo");
        var no = "N";
        var url = $("#urlBusqueda").val();
        var fields = {
            codCUSPP: $('#txtCUSPP').val(),
            numCor: no,
            numOperacion: $('#txtNumOperacion').val()
        };
        sendValues(fields, doSuccessBusqueda, doError, url);

        $('#modalcargar').modal({
            drop: 'static',
            keyboard: false,
            show: true
        });
    } else {
        aviso("ERROR", "Debes de ingresar un número de operación o un código CUSPP");
    }
}

function doSuccessBusqueda(result) {
    $('#modalcargar').modal('hide');
    var datos = result.Object[0].toString();
    var array = (datos).split(",");
    $("#txtNumOperacion2").val(array[0]);
    $("#txtDNI").val(array[1]);
    $("#txtAFP").val(array[2]);
    $("#txtCIC").val(array[3]);
    $("#txtAsegurado").val(array[4]);
    $("#txtCUSPP2").val(array[5]);
    $("#txtSexo").val(array[6]);
    $("#txtFecNac").val(array[7]);
    numCot = array[8];
    numArchivo = array[9];
    $("#txtPrestacion").val(array[10]);
    $("#txtFechaCierre").val(array[11]);
    $("#txtDepartamentoMeler").val(array[12]);
    $("#txtDepartamentoAsignado").val(array[13]);
    $("#txtAsesor").val(array[14]);
    $("#txtSupervisor").val(array[15]);
    cargaDetabla(result.Object, "busqueda");
    banCheck = false;
    if (clic == 1) {
        clic = clic + 1;
        document.getElementById("datosDiv").style.display = "block";
    }

    obtenerAlto("ventana", $(document).height());

}

function calcular() {
    arrayCalculo.length = 0;
    if ($("#txtNuevaTV").val() == "" && $("#txtNuevaCO").val() == "") {
        aviso("ERROR", "Ingrese una nueva Comisión o Tasa de Venta.");
        return;
    }

    var param = $('input:radio[name=rdParametros]:checked').val()

    if (banCheck != false) {
        limpiar("reCalculo");
        var dato;
        var tabla = document.getElementById("tblConsulta");
        var strMoneda = tabla.getElementsByClassName("strMoneda");
        var strModalidad = tabla.getElementsByClassName("strModalidad");
        var strPeriodo = tabla.getElementsByClassName("strPeriodo");
        var strRenta = tabla.getElementsByClassName("strRenta");
        var strGarantizado = tabla.getElementsByClassName("strGarantizado");
        var strEscalonada = tabla.getElementsByClassName("strEscalonada");
        var strPrima = tabla.getElementsByClassName("strPrima");
        var strTemporal = tabla.getElementsByClassName("strTemporal");
        var strVitalicia = tabla.getElementsByClassName("strVitalicia");
        var strTasa = tabla.getElementsByClassName("strTasa");
        var strTIR = tabla.getElementsByClassName("strTIR");
        var strPerdida = tabla.getElementsByClassName("strPerdida");
        var strComisión = tabla.getElementsByClassName("strComisión");
        var strCodTipReajuste = tabla.getElementsByClassName("strCodTipReajuste");
        var strNumCorrelativo = tabla.getElementsByClassName("strNumCorrelativo");
        numCor = strNumCorrelativo[checkVal].innerHTML;
        var strMtoPension = tabla.getElementsByClassName("strMtoPension");
        sumPen = strMtoPension[checkVal].innerHTML;

            dato = {
                numOperacion: $("#txtNumOperacion2").val(),
                dni: $("#txtDNI").val(),
                afp: $("#txtAFP").val(),
                cic: $("#txtCIC").val(),
                asegurado: $("#txtAsegurado").val(),
                cuspp: $("#txtCUSPP2").val(),
                sexo: $("#txtSexo").val(),
                fechaNac: $("#txtFecNac").val(),
                moneda: strMoneda[checkVal].innerHTML,
                modalidad: strModalidad[checkVal].innerHTML,
                periodoDiferido: strPeriodo[checkVal].innerHTML,
                rentaTMP: strRenta[checkVal].innerHTML,
                perGarantizado: strGarantizado[checkVal].innerHTML,
                rentaEsc: strEscalonada[checkVal].innerHTML,
                primaUnica: strPrima[checkVal].innerHTML,
                renTmp1T: strTemporal[checkVal].innerHTML,
                penVit2T: strVitalicia[checkVal].innerHTML,
                tasaVenta: strTasa[checkVal].innerHTML,
                tir: strTIR[checkVal].innerHTML,
                perdida: strPerdida[checkVal].innerHTML,
                comision: strComisión[checkVal].innerHTML,
                numArchivo: numArchivo,
                numCot: numCot,
                numCorrelativo: strNumCorrelativo[checkVal].innerHTML,
                cod_tipreajuste: strCodTipReajuste[checkVal].innerHTML,
                prestacion: $("#txtPrestacion").val(),
                fechaCierre: $("#txtFechaCierre").val(),
                departamentoMeler: $("#txtDepartamentoMeler").val(),
                departamentoAsignado: $("#txtDepartamentoAsignado").val(),
                asesor: $("#txtAsesor").val(),
                supervisor: $("#txtSupervisor").val()
            }
        
        var url = $("#urlCalcular").val();
        var fields = {
            nuevaTV: $('#txtNuevaTV').val(),
            nuevaCO: $('#txtNuevaCO').val(),
            informacion: dato,
            caso: "mejoras",
            parametro: param
        };
        $('#modalcargar').modal({
            drop: 'static',
            keyboard: false,
            show: true
        });
        sendValues(fields, doSuccessCalculo, doError, url);
    } else {
        aviso("ERROR", "No se puede calcular si no seleccionas algún registro");
    }
}

function doSuccessCalculo(result) {
    $('#modalcargar').modal('hide');
    cargaDetabla(result.Object, "calculo")
    aviso("AVISO", result.Message);
}
function doError(result) {
    $('#modalcargar').modal('hide');
    aviso("ERROR", result.Message);
}

function limpiar(caso) {
    switch (caso) {
        case "todo":
            var myTable = document.getElementById("RConsultaBody");
            var rowCount = myTable.rows.length;
            for (var x = rowCount - 1; x > -1; x--) {
                myTable.deleteRow(x);
            }
            var myTable2 = document.getElementById("RCalculoBody");
            var rowCount2 = myTable2.rows.length;
            for (var x = rowCount2 - 1; x > -1; x--) {
                myTable2.deleteRow(x);
            }
            $("#txtNumOperacion2").val("");
            $("#txtDNI").val("");
            $("#txtAFP").val("");
            $("#txtCIC").val("");
            $("#txtAsegurado").val("");
            $("#txtCUSPP2").val("");
            $("#txtSexo").val("");
            $("#txtFecNac").val("");
            $('#txtNuevaTV').val("");
            $('#txtNuevaCO').val("");
            $('#txtPrestacion').val("");
            $("#txtFechaCierre").val("");
            $("#txtDepartamentoMeler").val("");
            $("#txtDepartamentoAsignado").val("");
            $("#txtAsesor").val("");
            $("#txtSupervisor").val("");
            document.getElementById("datosDiv").style.display = "none";
            clic = 1;
            break;
        case "reCalculo":
            var myTable2 = document.getElementById("RCalculoBody");
            var rowCount2 = myTable2.rows.length;
            for (var x = rowCount2 - 1; x > -1; x--) {
                myTable2.deleteRow(x);
            }
            break;
    }
}

function soloNumeros(e) {
    var key = window.event ? e.which : e.keyCode;
    if (key < 48 || key > 57) {
        e.preventDefault();
    }
}

function GuardarMe() {
    var dato;
    var datosRut;
    var tabla = document.getElementById("tblCalculo");
    var tablaC = document.getElementById("tblConsulta");
    var strVitalicia = tabla.getElementsByClassName("strVitalicia");
    var strModalidad = tabla.getElementsByClassName("strModalidad");
    var strTasa = tabla.getElementsByClassName("strTasa");
    var strTIR = tabla.getElementsByClassName("strTIR");
    var strPerdida = tabla.getElementsByClassName("strPerdida");
    var strComisión = tabla.getElementsByClassName("strComisión");
    var strMtoPension = tabla.getElementsByClassName("strMtoPension");
    var strPrima = tabla.getElementsByClassName("strPrima");
    var mto = strVitalicia[0].innerHTML;
    mto = parseFloat(mto.replace(",", ""));
    var primerTramo = tabla.getElementsByClassName("strTemporal");
    var aniosDif = tabla.getElementsByClassName("strPeriodo");

    strPrima = (strPrima[0].innerHTML);
    strPrima = parseFloat(strPrima.replace(/,/g, ''));
    //sumPen = strMtoPension.innerHTML;
    dato = {
        numOperacion: $("#txtNumOperacion2").val(),
        modalidad: strModalidad[0].innerHTML,
        primaUnica: strPrima,
        mtoPensio: mto,
        tasaVenta: strTasa[0].innerHTML,
        tir: strTIR[0].innerHTML,
        perdida: strPerdida[0].innerHTML,
        comision: strComisión[0].innerHTML,
        numCorrelativo: numCor,
        mtoSumPension: strMtoPension[0].innerHTML,
        primerTramo: primerTramo[0].innerHTML,
        periodoDiferido: aniosDif[0].innerHTML
    }
    datosRut = {
        MTO_AJUSTEIPC: arrayCalculo[0],
        MTO_CTAINDAFP: arrayCalculo[1],
        MTO_RENTATMPAFP: arrayCalculo[2],
        MTO_RESMAT: arrayCalculo[3],
        PRC_TASATCE: arrayCalculo[4],
        FecCal: arrayCalculo[5],
        MTO_PENANUAL: arrayCalculo[6],
        MTO_PENSIONGAR: arrayCalculo[7],
        MTO_PRIUNISIM: arrayCalculo[8],
        MTO_RMGTOSEP: arrayCalculo[9],
        MTO_RMGTOSEPRV: arrayCalculo[10],
        MTO_VALREAJUSTEMEN: arrayCalculo[11],
        MTO_VALREAJUSTETRI: arrayCalculo[12],
        MTO_VALPREPENTMP: arrayCalculo[13]

    }
    var url = $("#urlGuardarMej").val();
    var fields = {
        informacion: dato,
        caso: "mejoras",
        infoRut: datosRut
    };
    sendValues(fields, doSuccessGuardar, doError, url);
}
function doSuccessGuardar(result) {
    aviso("AVISO", result.Message);

    //Limpiar primer Grid
    var myTable = document.getElementById("RConsultaBody");
    var rowCount = myTable.rows.length;
    for (var x = rowCount - 1; x > -1; x--) {
        myTable.deleteRow(x);
    }

    //Recargar primer Grid
    var no = "N";
    var url = $("#urlBusqueda").val();
    var fields = {
        codCUSPP: $('#txtCUSPP').val(),
        numCor: no,
        numOperacion: $('#txtNumOperacion').val()
    };
    sendValues(fields, doSuccessBusqueda, doError, url);

    //Limpiar segundo Grid
    var myTable = document.getElementById("RCalculoBody");
    var rowCount = myTable.rows.length;
    for (var x = rowCount - 1; x > -1; x--) {
        myTable.deleteRow(x);
    }
}

function filterNonAphaNumeric(str) {
    let code, i, len, result = '';

    for (i = 0, len = str.length; i < len; i++) {
        code = str.charCodeAt(i);
        if ((code > 47 && code < 58) || // numeric (0-9)
            (code > 64 && code < 91) || // upper alpha (A-Z)
            (code > 96 && code < 123) || // lower alpha (a-z)
                (code > 207 && code < 210)) // Ññ
        {
            result += str.charAt(i);
        }
    }
    return result;
};

function onKeyUpHandler(event) {
    let value = this.value.toUpperCase();
    if (value) {
        this.value = filterNonAphaNumeric(value)
    }
}
