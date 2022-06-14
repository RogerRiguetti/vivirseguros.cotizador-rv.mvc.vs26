(function () {
    localStorage.pagina = 'TasaAnclaje';
    //Funcion para el llenado del combo de Periodos
    $("#cmbxTipoMoneda").change(function () {
        var id = $("#cmbxTipoMoneda").val();
        if (id == "") {
            document.getElementById("CmbxPeriodos").options.length = 0;
            $("#CmbxPeriodos").append('<option value="">-- Seleccione --</option>');

            document.getElementById("fecIni").value = $("#backFecha").val();
            document.getElementById("fecFin").value = $("#backFecha").val();
            document.getElementById("txtTasa").value = "";

        } else {
            var url = $("#urlPeriodo").val();
            var res = id.split("#");
            var fields = {
                vlMoneda: res[0],
                vlReajuste: res[1]
            };
            sendValues(fields, doSuccessPeriodos, doError, url);
        }
    });
    $("#CmbxPeriodos").change(function () {
        var strFec = $("#CmbxPeriodos").val();
        if (strFec == "") {
            //actica la caja de fecha inicio para agregar una nueva fecha
            document.getElementById("fecIni").disabled = false;//activa
            document.getElementById("cmbxTipoMoneda").disabled = false;
            document.getElementById("fecIni").value = $("#backFecha").val();
            document.getElementById("fecFin").value = $("#backFecha").val();
            document.getElementById("txtTasa").value = "";
            document.getElementById("btnBuscar").style.display = "";
            document.getElementById("btnBuscar2").style.display = "none";
        } else {
            var id = $("#cmbxTipoMoneda").val();
            var url = $("#urlBuscarVigencia").val();
            var res = id.split("#");
            var fields = {
                vlMoneda: res[0],
                vlReajuste: res[1],
                strFecIni: strFec
            };
            sendValues(fields, doSuccessBuscarVigencia, doError, url);
        }
    });
})();
function doSuccessPeriodos(result) {

    document.getElementById("CmbxPeriodos").options.length = 0;
    $("#CmbxPeriodos").append('<option value="">-- Seleccione --</option>');
    result.Object.forEach(function (v) {
        $("#CmbxPeriodos").append('<option value="' + v.IdPeriodo + '">' + v.Elemento + '</option>');
    });
}
function doError(result) {
    aviso("Aviso", result.Message);
}
function doSuccessBuscarVigencia(result) {
    if (result.IsOk) {
        if (result.Message == "No se encontro información") {
            //confirmarAnclaje("Aviso", "La Fecha Indicada no se encuentra registrada. \n" + "\n¿Desea Ingresar esta nueva Fecha de Vigencia?", result, 1);
            confirmarAnclaje("Aviso", "La Fecha Indicada no se encuentra registrada. \n" + "\n¿Desea Ingresar esta nueva Fecha de Vigencia?", function () { NuevaTasaAnclaje(result, 1); });
        } else {
            var Vigencia = result.Object;
            document.getElementById("fecIni").value = Vigencia.FechaInicial;
            document.getElementById("fecFin").value = Vigencia.FechaTermino;
            document.getElementById("txtTasa").value = parseFloat(Vigencia.Tasa).toFixed(2);
            document.getElementById("txtTasa").focus();
            document.getElementById("fecIni").disabled = true;//desactiva
            document.getElementById("cmbxTipoMoneda").disabled = true;
            document.getElementById("btnBuscar").style.display = "none";
            document.getElementById("btnBuscar2").style.display = "";
        }
    }
}
function Buscar() {
    var id = $("#cmbxTipoMoneda").val();
    if (id == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda");
        document.getElementById("cmbxTipoMoneda").focus();
    } else {
        var strFec = $("#fecIni").val();
        if (strFec == "") {
            aviso("Aviso", "Debe Ingresar la Fecha de Inicio");
            document.getElementById("fecIni").focus();
        }
        else {
            var url = $("#urlBuscarVigencia").val();
            var res = id.split("#");
            var fields = {
                vlMoneda: res[0],
                vlReajuste: res[1],
                strFecIni: strFec
            };
            $('#CmbxPeriodos').val('');
            sendValues(fields, doSuccessBuscarVigencia, doError, url);
        }
    }
}
function NuevaTasaAnclaje(result, bandera) {
    var Vigencia = result.Object;
    document.getElementById("fecIni").value = Vigencia.FechaInicial;
    document.getElementById("fecFin").value = Vigencia.FechaTermino;
    if (bandera == 1) {
        document.getElementById("txtTasa").value = parseFloat(Vigencia.Tasa).toFixed(2);
        document.getElementById("txtTasa").focus();
    }
    //desativar los combos
    document.getElementById("fecIni").disabled = true;//desactiva
    document.getElementById("cmbxTipoMoneda").disabled = true;
    document.getElementById("CmbxPeriodos").disabled = true;
    document.getElementById("btnBuscar").style.display = "none";
    document.getElementById("btnBuscar2").style.display = "";

    if (bandera == 2) {//nuevo
        var url = $("#urlGrabarVigencia").val();
        var id = $("#cmbxTipoMoneda").val();
        var vlTasa = $("#txtTasa").val();
        var res = id.split("#");
        var fields = {
            vlMoneda: res[0],
            vlReajuste: res[1],
            strFecIni: Vigencia.FechaInicial,
            strFecFin: Vigencia.FechaTermino,
            tasa: vlTasa,
            bandera: true
        };
        sendValues(fields, doSuccessGrabarVigencia, doError, url);
        return;
    }
    if (bandera == 3) {//uodate
        document.getElementById("CmbxPeriodos").disabled = false;
        var url = $("#urlGrabarVigencia").val();
        var id = $("#cmbxTipoMoneda").val();
        var vlTasa = $("#txtTasa").val();
        var res = id.split("#");
        var fields = {
            vlMoneda: res[0],
            vlReajuste: res[1],
            strFecIni: Vigencia.FechaInicial,
            strFecFin: Vigencia.FechaTermino,
            tasa: vlTasa,
            bandera: false
        };


        sendValues(fields, doSuccessGrabarVigencia, doError, url);
        return;
    }
}
function doSuccessGrabarVigencia(result) {
    var Vigencia = result.Object;
    document.getElementById("fecFin").value = Vigencia.FechaTermino;
    aviso("Aviso", result.Message);
}
function Grabar() {
    var id = $("#cmbxTipoMoneda").val();
    if (id == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda");
        document.getElementById("cmbxTipoMoneda").focus();
        return;
    }
    var strFec = $("#fecIni").val();
    if (strFec == "") {
        aviso("Aviso", "Debe Ingresar la Fecha de Inicio");
        document.getElementById("fecIni").focus();
        return;
    }
    var tasa = $("#txtTasa").val();
    if (tasa == "") {
        aviso("Aviso", "Debe Ingresar la Tasa de Reaseguro");
        document.getElementById("txtTasa").focus();
        return;
    }
    if (tasa < 0 || tasa > 100) {
        aviso("Aviso", "El Valor del Porcentaje debe ser entre 0 y 100");
        document.getElementById("txtTasa").focus();
        return;
    }
    //se ejecuta la funcion de grabar
    var url = $("#urlBuscarVigencia").val();
    var res = id.split("#");
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
        strFecIni: strFec
    };
    sendValues(fields, doSuccessVerificarVigencia, doError, url);
}
function doSuccessVerificarVigencia(result) {
    if (result.Message == "No se encontro información") {
        // confirmarAnclaje("Aviso", "¿ Está seguro que desea Grabar la Información ?", result, 2);
        confirmarAnclaje("Aviso", "¿ Está seguro que desea Grabar la Información ?", function () { NuevaTasaAnclaje(result, 2); });
        return;
    }
    if (result.Message == "Información cargada con éxito") {
        confirmarAnclaje("Aviso", "¿ Está seguro que desea Modificar la Información ?", function () { NuevaTasaAnclaje(result, 3); });
        // confirmarAnclaje("Aviso", "¿ Está seguro que desea Modificar la Información ?", result, 3);
        return;
    }
}
function Eliminar() {
    var id = $("#cmbxTipoMoneda").val();
    if (id == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda");
        document.getElementById("cmbxTipoMoneda").focus();
        return;
    }
    /* var cmb = $("#CmbxPeriodos").val();
     if (cmb == "") {
         aviso("Aviso", "Debe seleccionar un rango de vigencia para Eliminar");
         document.getElementById("CmbxPeriodos").focus();
         return;
     }*/
    var strFec = $("#fecIni").val();
    if (strFec == "") {
        aviso("Aviso", "Debe seleccionar un rango de vigencia para Eliminar");
        document.getElementById("fecIni").focus();
        return;
    }

    var url = $("#urlBuscarVigencia").val();
    var res = id.split("#");
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
        strFecIni: strFec
    };
    sendValues(fields, doSuccessEliminarVigencia, doError, url);
}
function doSuccessEliminarVigencia(result) {
    //Información cargada con éxito
    if (result.Message == "Información cargada con éxito") {
        // confirmarAnclaje("Aviso", "¿ Está seguro que desea Eliminar la Información ?", result, 4);
        confirmarAnclaje("Aviso", "¿ Está seguro que desea Eliminar la Información ?", function () { EliminarPeriodo(result); });
    } else {
        aviso("Aviso", "El Período que está intentando Eliminar no se encuentra en la BD");
    }
}
function EliminarPeriodo(result) {
    var id = $("#cmbxTipoMoneda").val();
    var strFec = $("#fecIni").val();
    var res = id.split("#");
    //se ejecuta la funcion Eliminar
    var url = $("#urlEliminarVigencia").val();
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
        strFecIni: strFec
    };
    sendValues(fields, doSuccess, doError, url);
    //se recarga el combo
    Limpiar();
    $('#cmbxTipoMoneda').val(id);
    url = $("#urlPeriodo").val();
    var fieldsC = {
        vlMoneda: res[0],
        vlReajuste: res[1]
    };
    sendValues(fieldsC, doSuccessPeriodos, doError, url);
}
function doSuccess(result) {
    aviso("Aviso", result.Message);
}
function Imprimir() {
    var url = $("#urlReporte").val();
    var id = $("#cmbxTipoMoneda").val();
    var tex = $("#cmbxTipoMoneda option:selected").text();

    if (id == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda");
        document.getElementById("cmbxTipoMoneda").focus();
        return;
    }
    var res = id.split("#");
    window.open(url + "?vlMoneda=" + res[0] + "&vlReajuste=" + res[1] + "&moneda=" + tex);
}
function Limpiar() {
    $('#cmbxTipoMoneda').val('');
    document.getElementById("CmbxPeriodos").options.length = 0;
    $("#CmbxPeriodos").append('<option value="">-- Seleccione --</option>');

    document.getElementById("fecIni").value = $("#backFecha").val();
    document.getElementById("fecFin").value = $("#backFecha").val();
    document.getElementById("txtTasa").value = "";

    document.getElementById("fecIni").disabled = false;//activa
    document.getElementById("cmbxTipoMoneda").disabled = false;
    document.getElementById("CmbxPeriodos").disabled = false;
    document.getElementById("btnBuscar").disabled = false;
    document.getElementById("btnBuscar").style.display = "";
    document.getElementById("btnBuscar2").style.display = "none";

}
function Salir() {
    var url = $("#urlSalir").val();
    window.location.href = url;
}
//Alertas en Modal
function confirmarAnclaje(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}

/*
function confirmarAnclaje(header, body, result, bandera) {
    $('#msg_modal_header_2').text(header);
    $('#msg_modal_body_2').text(body);
    $('#sch_modal_Anclaje').modal('show');

    $("#btn_modal_aceptar_2").one('click', function () {
        $('#sch_modal_Anclaje').modal('hide');
        if (bandera == 1)//busqueda
            NuevaTasaAnclaje(result, bandera);
        if (bandera == 2)//genera el nuevo registro
            NuevaTasaAnclaje(result, bandera);
        if (bandera == 3)//modifica el registro
            NuevaTasaAnclaje(result, bandera);
        if (bandera == 4)//Elimina el registro
            EliminarPeriodo(result);
    });
}*/

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