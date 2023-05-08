$(document).ready(function () {
    $('#tblConsulta').DataTable({
        searching: false,
        bSort: false,
        paging: false,
        info: false
    });

    $('#tblCalculo').DataTable({
        searching: false,
        bSort: false,
        paging: false,
        info: false
    });

    $('#tblExepcionesExternas').DataTable({
        language: { search: '', searchPlaceholder: "Buscar" }
    });
});
var checkVal;
var banCheck = false;
var numCot;
var numArchivo;
var numCor = "";
var clic = 1;
var arrayCalculo = [];
var corExterno = "";
var cusppExterno = "";
var opExterno = "";
var caso = "";

(function () {
    localStorage.pagina = $("#urlPagina").val();
    let input = document.getElementById('txtCUSPP');
    input.addEventListener('keyup', onKeyUpHandler);
})();

function cargaDetabla(datos, operacion) {
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
            var thDato = document.createElement('td');
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
    });
}

function busqueda() {
    document.getElementById("datosDiv").style.display = "none";
    clic = 1;
    caso = "excepciones";
    var no = "N";
    if ($('#txtNumOperacion').val() != "" || $('#txtCUSPP').val() != "") {
        limpiar("todo");
        var url = $("#urlBusqueda").val();
        var fields = {
            codCUSPP: $('#txtCUSPP').val(),
            numCor: no,
            numOperacion: $('#txtNumOperacion').val()
        };
        $('#modalcargar').modal({
            drop: 'static',
            keyboard: false,
            show: true,
            backdrop: 'static'
        });
        sendValues(fields, doSuccessBusqueda, doError, url);
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
        //var strMtoPension = tabla.getElementsByClassName("strMtoPension");
        var strTemporal = tabla.getElementsByClassName("strTemporal");
        var strVitalicia = tabla.getElementsByClassName("strVitalicia");
        var strTasa = tabla.getElementsByClassName("strTasa");
        var strTIR = tabla.getElementsByClassName("strTIR");
        var strPerdida = tabla.getElementsByClassName("strPerdida");
        var strComisión = tabla.getElementsByClassName("strComisión");
        var strNumCorrelativo = tabla.getElementsByClassName("strNumCorrelativo");
        var strCodTipReajuste = tabla.getElementsByClassName("strCodTipReajuste");

        numCor = strNumCorrelativo[checkVal].innerHTML;
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
            //mtoPensio: strMtoPension[checkVal].innerHTML,
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
            departamentoAsignador: $("#txtDepartamentoAsignado").val(),
            asesor: $("#txtAsesor").val(),
            supervisor: $("#txtSupervisor").val()
        }
        var url = $("#urlCalcular").val();
        var fields = {
            nuevaTV: $('#txtNuevaTV').val(),
            nuevaCO: $('#txtNuevaCO').val(),
            informacion: dato,
            caso: "excepciones"
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
    obtenerAlto("ventana", $(document).height());

    aviso("AVISO", result.Message);
}
function doError(result) {
    $('#modalcargar').modal('hide');
    aviso("ERROR", result.Message);
}

function limpiar(caso) {
    switch (caso) {
        case "todo":
            corExterno = "";
            cusppExterno = "";
            opExterno = "";
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
            obtenerAlto("ventana", $(document).height());

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

function fn_cargaTramo(tramo) {

    $("#tblExepcionesExternas tr").click(function () {
        $("#tblExepcionesExternas tr").removeClass('focus');
        $(this).addClass('focus');
    });

    var tabla = document.getElementById("tblExepcionesExternas");

    var cuspp = tabla.rows[tramo].cells[0].innerText;
    var cor = tabla.rows[tramo].cells[1].innerText;
    var ope = tabla.rows[tramo].cells[2].innerText;
    busquedaTramo(cuspp, cor, ope);

}

function busquedaTramo(cuspp, cor, ope) {
    document.getElementById("datosDiv").style.display = "none";
    clic = 1;
    limpiar("todo");
    corExterno = cor;
    cusppExterno = cuspp;
    opExterno = ope;
    caso = "excepcionesExternas";
    var url = $("#urlBusqueda").val();
    var fields = {
        codCUSPP: cuspp,
        numCor: cor,
        numOperacion: ope
    };
    sendValues(fields, doSuccessBusqueda, doError, url);

}


function GuardarEx() {
    var dato;
    var datosRut;
    var tabla = document.getElementById("tblCalculo");
    if (tabla.rows.length != 1) {
        var tablaC = document.getElementById("tblConsulta");
        var strPrima = tabla.getElementsByClassName("strPrima");
        var strVitalicia = tabla.getElementsByClassName("strVitalicia");
        var strModalidad = tabla.getElementsByClassName("strModalidad");
        var strTasa = tabla.getElementsByClassName("strTasa");
        var strTIR = tabla.getElementsByClassName("strTIR");
        var strPerdida = tabla.getElementsByClassName("strPerdida");
        var strComisión = tabla.getElementsByClassName("strComisión");
        var strMtoPension = tabla.getElementsByClassName("strMtoPension");
        var primerTramo = tabla.getElementsByClassName("strTemporal");
        var aniosDif = tabla.getElementsByClassName("strPeriodo");
        var mto = strVitalicia[0].innerHTML;
        mto = parseFloat(mto.replace(",", ""));

        strPrima = (strPrima[0].innerHTML);
        strPrima = parseFloat(strPrima.replace(/,/g, ''));

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

        var url = $("#urlGuardar").val();
        var fields = {
            informacion: dato,
            caso: caso,
            infoRut: datosRut
        };
        sendValues(fields, doSuccessGuardar, doError, url);
    }
    else {
        aviso("AVISO", "Primero debes calcular la modalidad a guardar");
    }
}
function doSuccessGuardarEx(result) {
    modalConfirmacion("Aviso"
        , result.Message
        , function () {
            var url = $("#urlRecargar").val();
            window.location.href = url;
        });
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
    if (corExterno != "" && cusppExterno != "" && opExterno != "") {
        busquedaTramo(cusppExterno, corExterno, opExterno)
    }
    else {
        var numCor = "N";
        var url = $("#urlBusqueda").val();
        var fields = {
            codCUSPP: $('#txtCUSPP').val(),
            numCor: numCor,
            numOperacion: $('#txtNumOperacion').val()
        };
        sendValues(fields, doSuccessBusqueda, doError, url);
    }

    //Limpiar segundo Grid
    var myTable = document.getElementById("RCalculoBody");
    var rowCount = myTable.rows.length;
    for (var x = rowCount - 1; x > -1; x--) {
        myTable.deleteRow(x);
    }
}


function cancelar() {
    var tablaC = document.getElementById("tblConsulta");
    if (tablaC.rows.length != 1) {
        var url = $("#urlCancelar").val();
        var tabla = document.getElementById("tblConsulta");
        var strPeriodo = tabla.getElementsByClassName("strPeriodo");
        var primerTramo = tabla.getElementsByClassName("strTemporal");

        var fields = {
            numCor: corExterno,
            numOperacion: opExterno,
            periodoDiferido: strPeriodo[0].innerHTML,
            primerTramo: primerTramo[0].innerHTML
        };
        sendValues(fields, doSuccessCancelar, doError, url);
    } else {
        aviso("AVISO", "Primero debes seleccionar una modalidad");
    }
}

function doSuccessCancelar(result) {
    modalConfirmacion("Aviso", result.Message, function () {
        var url = $("#urlRecargar").val();
        window.location.href = url;
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