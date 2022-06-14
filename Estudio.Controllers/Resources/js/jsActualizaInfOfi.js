$(document).ready(function () {
    $('#tblConsulta').DataTable({
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
    localStorage.pagina = 'ActualizaInfOfi';
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


        limpiar("reCalculo");

        var url = $("#urlBusquedaMod").val();
        var strNumCorrelativo = tabla.getElementsByClassName("strNumCorrelativo");
        numCor = strNumCorrelativo[checkVal].innerHTML;
        var fields = {
            numCor: numCor,
            numOperacion: $('#txtNumOperacion2').val()
        };
        sendValues(fields, doSuccessBuscarMod, doError, url);
        $('#modalcargar').modal({
            drop: 'static',
            keyboard: false,
            show: true
        });

    });

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

function doSuccessBuscarMod(result) {
    $('#modalcargar').modal('hide');
    var datos = result.Object.Object[0];
    //var array = (datos).split(",");
    //$("#txtMTO_AJUSTEIPC").val(datos.MTO_AJUSTEIPC);
    $("#txtMTO_CTAINDAFP").val(datos.MTO_CTAINDAFP);
    $("#txtMTO_PENSION").val(datos.mtoPensio);
    $("#txtMTO_PRIUNIDIF").val(datos.MTO_PRIUNIDIF);
    $("#txtMTO_RENTATMPAFP").val(datos.MTO_RENTATMPAFP);
    //$("#txtMTO_RESMAT").val(datos.MTO_RESMAT);
    $("#txtPRC_PERCON").val(datos.PRC_PERCON);
    //$("#txtPRC_TASATCE").val(datos.PRC_TASATCE);
    $("#txtPRC_TASATIR").val(datos.PRC_TASATIR);
    $("#txtPRC_TASAVTA").val(datos.PRC_TASAVTA);
    //$("#txtIND_SISCO").val(datos.Ind_SISCO);
    var indcotiza = (datos.IND_FILTROCOTIZA).replace(/ /g,'');
    $("#txtIND_FILTROCOTIZA").val(indcotiza);
    var indcod_estcot = (datos.COD_ESTCOT).replace(/ /g, '')
    $("#txtCOD_ESTCOT").val(indcod_estcot);
    var indMejEx = (datos.Ind_MejEx).replace(/ /g, '')
    $("#txtIND_MEJEX").val(indMejEx);
    $("#txtComision").val(datos.comision);
    $("#txtTipCambio").val(datos.codTipCambio);
    //$("#txtMTO_PENANUAL").val(datos.MTO_PENANUAL);
    //$("#txtMTO_PENSIONGAR").val(datos.MTO_PENSIONGAR);
    //$("#txtMTO_PRIUNISIM").val(datos.MTO_PRIUNISIM);
    //$("#txtMTO_RMGTOSEP").val(datos.MTO_RMGTOSEP);
    //$("#txtMTO_RMGTOSEPRV").val(datos.MTO_RMGTOSEPRV);
    //$("#txtMTO_VALREAJUSTEMEN").val(datos.MTO_VALREAJUSTEMEN);
    //$("#txtMTO_VALREAJUSTETRI").val(datos.MTO_VALREAJUSTETRI);
    //$("#txtMTO_VALPREPENTMP").val(datos.MTO_VALPREPENTMP);
   
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
    cargaDetabla(result.Object, "busqueda");
    banCheck = false;
    if (clic == 1) {
        document.getElementById("caja").style.display = "block";
        clic = clic + 1;
        document.getElementById("datosDiv").style.display = "block";
    }
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
            $("#txtMTO_AJUSTEIPC").val("");
            $("#txtMTO_CTAINDAFP").val("");
            $("#txtMTO_PENSION").val("");
            $("#txtMTO_PRIUNIDIF").val("");
            $("#txtMTO_RENTATMPAFP").val("");
            $("#txtMTO_RESMAT").val("");
            $("#txtPRC_PERCON").val("");
            $("#txtPRC_TASATCE").val("");
            $('#txtPRC_TASATIR').val("");
            $('#txtPRC_TASAVTA').val("");
            $("#txtIND_SISCO").val("");
            $("#txtIND_FILTROCOTIZA").val("");
            $("#txtCOD_ESTCOT").val("");
            $("#txtMTO_PENANUAL").val("");
            $("#txtMTO_PENSIONGAR").val("");
            $("#txtMTO_PRIUNISIM").val("");
            $("#txtMTO_RMGTOSEP").val("");
            $("#txtMTO_RMGTOSEPRV").val("");
            $('#txtMTO_VALREAJUSTEMEN').val("");
            $('#txtMTO_VALREAJUSTETRI').val("");
            $("#txtMTO_VALPREPENTMP").val("");
            $("#txtIND_MEJEX").val("");
            $("#txtComision").val("");
            $("#txtTipCambio").val("");
            document.getElementById("caja").style.display = "none";
            document.getElementById("datosDiv").style.display = "none";
            clic = 1;
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
    var MTO_PRIUNIDIF = $("#txtMTO_PRIUNIDIF").val();
    var MTO_CTAINDAFP = $("#txtMTO_CTAINDAFP").val();
    var MTO_RENTATMPAFP = $("#txtMTO_RENTATMPAFP").val();
    var MTO_PENSION = $("#txtMTO_PENSION").val();
    var PRC_TASAVTA = $("#txtPRC_TASAVTA").val();
    var PRC_TASATIR = $("#txtPRC_TASATIR").val();
    var PRC_PERCON = $("#txtPRC_PERCON").val();
    var COD_ESTCOT = $("#txtCOD_ESTCOT").val().replace(/ /g, '');
    var IND_FILTROCOTIZA = $("#txtIND_FILTROCOTIZA").val().replace(/ /g, '');
    var indMejEx = $("#txtIND_MEJEX").val().replace(/ /g, '');
    var comision = $("#txtComision").val();
   
        var tabla = document.getElementById("tblConsulta");
        var strNumCorrelativo = tabla.getElementsByClassName("strNumCorrelativo");
        numCor = strNumCorrelativo[checkVal].innerHTML;


        dato = {
            MTO_CTAINDAFP: MTO_CTAINDAFP,
            mtoPensio: MTO_PENSION,
            MTO_PRIUNIDIF: MTO_PRIUNIDIF,
            MTO_RENTATMPAFP: MTO_RENTATMPAFP,
            PRC_TASAVTA: PRC_TASAVTA,
            PRC_TASATIR: PRC_TASATIR,
            PRC_PERCON: PRC_PERCON,
            IND_FILTROCOTIZA: IND_FILTROCOTIZA,
            COD_ESTCOT: COD_ESTCOT,
            numCorrelativo: numCor,
            numOperacion: $('#txtNumOperacion2').val(),
            Ind_MejEx: indMejEx,
            comision: comision
        }

        var url = $("#urlGuardar").val();
        var fields = {
            informacion: dato
        };
        sendValues(fields, doSuccessGuardar, doError, url);
    
}


function Validacion() {
    var dato;
    var datosRut;
    var MTO_PRIUNIDIF = $("#txtMTO_PRIUNIDIF").val();
    var MTO_CTAINDAFP = $("#txtMTO_CTAINDAFP").val();
    var MTO_RENTATMPAFP = $("#txtMTO_RENTATMPAFP").val();
    var MTO_PENSION = $("#txtMTO_PENSION").val();
    var PRC_TASAVTA = $("#txtPRC_TASAVTA").val();
    var PRC_TASATIR = $("#txtPRC_TASATIR").val();
    var PRC_PERCON = $("#txtPRC_PERCON").val();
    var COD_ESTCOT = $("#txtCOD_ESTCOT").val().replace(/ /g, '');
    var IND_FILTROCOTIZA = $("#txtIND_FILTROCOTIZA").val().replace(/ /g, '');
    var indMejEx = $("#txtIND_MEJEX").val().replace(/ /g, '');
    var comision = $("#txtComision").val();
    var tipcambio = $("#txtTipCambio").val();

    //var IND_FILTROCOTIZA = $("#txtIND_FILTROCOTIZA").val();
    var sumpen;
    sumpen = MTO_PENSION * 2 * tipcambio;
    sumpen = sumpen.toFixed(2);
    if (MTO_RENTATMPAFP == sumpen) {
        GuardarMe();
    }
    else {
        var header = "Confirmar";
        var body = "Pensión RT no es el doble de Pensión ¿Desea seguir guardando?";

        confirmarCotizacion(header, body, function () { GuardarMe(); });
    }
}

function confirmarCotizacion(header, body, GuardarMe) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        GuardarMe();
        $('#sch_modal_confirm').modal('hide');
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

    $("#txtMTO_AJUSTEIPC").val("");
    $("#txtMTO_CTAINDAFP").val("");
    $("#txtMTO_PENSION").val("");
    $("#txtMTO_PRIUNIDIF").val("");
    $("#txtMTO_RENTATMPAFP").val("");
    $("#txtMTO_RESMAT").val("");
    $("#txtPRC_PERCON").val("");
    $("#txtPRC_TASATCE").val("");
    $('#txtPRC_TASATIR').val("");
    $('#txtPRC_TASAVTA').val("");
    $("#txtIND_SISCO").val("");
    $("#txtIND_FILTROCOTIZA").val("");
    $("#txtIND_MEJEX").val("");
    $("#txtCOD_ESTCOT").val("");
    $("#txtMTO_PENANUAL").val("");
    $("#txtMTO_PENSIONGAR").val("");
    $("#txtMTO_PRIUNISIM").val("");
    $("#txtMTO_RMGTOSEP").val("");
    $("#txtMTO_RMGTOSEPRV").val("");
    $('#txtMTO_VALREAJUSTEMEN').val("");
    $('#txtMTO_VALREAJUSTETRI').val("");
    $("#txtMTO_VALPREPENTMP").val("");
    $("#txtComision").val("");

    //Recargar primer Grid
    var no = "N";
    var url = $("#urlBusqueda").val();
    var fields = {
        codCUSPP: $('#txtCUSPP').val(),
        numCor: no,
        numOperacion: $('#txtNumOperacion').val()
    };
    sendValues(fields, doSuccessBusqueda, doError, url);

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

function filterFloat(evt, input) {
    // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;
    if (key >= 48 && key <= 57) {
        if (filter(tempValue) === false) {
            return false;
        } else {
            return true;
        }
    } else {
        if (key == 8 || key == 13 || key == 0) {
            return true;
        } else if (key == 46) {
            if (filter(tempValue) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    }
}
function filter(__val__) {
    var preg = /^([0-9]+\.?[0-9]{0,2})$/;
    if (preg.test(__val__) === true) {
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

function validanumyletras(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }
    //if (tecla == 192)
    //{
    //    return true;
    //}

    // Patron de entrada, en este caso solo acepta numeros y letras
    patron = /[A-Za-z0-9 Ñ]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

function soloLetras(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúabcdefghijklmnñopqrstuvwxyz0123456789";
    especiales = "8-37-39-46-164-165";

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