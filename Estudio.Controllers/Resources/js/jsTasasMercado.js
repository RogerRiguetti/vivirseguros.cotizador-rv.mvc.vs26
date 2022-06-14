(function () {
    localStorage.pagina = 'TasasMercado';
    Meses(true);
    //Funcion para el llenado del combo de Periodos
    $("#CmbxTipoMonedaTMH").change(function () {
        var id = $("#CmbxTipoMonedaTMH").val();
        if (id == "") {
            document.getElementById("cmbxYear").options.length = 0;
            $("#cmbxYear").append('<option value="">-- Seleccione --</option>');

        } else {
            var url = $("#urlYear").val();
            var res = id.split("#");
            var fields = {
                vlMoneda: res[0],
                vlReajuste: res[1]
            };
            sendValues(fields, doSuccessYear, doError, url);
        }
    });
    $("#cmbxYear").change(function () {
        var vlAnno = $("#cmbxYear").val();
        if (vlAnno == "") {
            //limpia
            document.getElementById("CmbxTipoMonedaTMH").disabled = false;
            document.getElementById("txtYear").disabled = false;
            Meses(true);
            document.getElementById("txtYear").value = "";

        } else {
            document.getElementById("txtYear").value = vlAnno;
            //realiza la consulta para el lledo de los campos
            var id = $("#CmbxTipoMonedaTMH").val();
            var url = $("#urlBuscarYear").val();
            var res = id.split("#");
            var fields = {
                vlMoneda: res[0],
                vlReajuste: res[1],
                vlAnno: vlAnno
            };
            sendValues(fields, doSuccessConsultaYear, doError, url);
        }
    });
})();
function doSuccessYear(result) {

    document.getElementById("cmbxYear").options.length = 0;
    $("#cmbxYear").append('<option value="">-- Seleccione --</option>');
    result.Object.forEach(function (v) {
        $("#cmbxYear").append('<option value="' + v.Year + '">' + v.Year + '</option>');
    });
}
function doError(result) {
    aviso("Aviso", result.Message);
}
function doSuccessConsultaYear(result) {
    if (result.IsOk) {
        if (result.Message == "No se encontro información") {
            Meses(false);
            //confirmarTasaMercado("Aviso", "El Año indicado no se encuentra registrado.  ¿ Desea ingresar este nuevo Año ?", result, 1);
            confirmarTasaMercado("Aviso", "El Año indicado no se encuentra registrado.  ¿ Desea ingresar este nuevo Año ?", function () { NuevaTasaMercado(result, 1); });

        } else {
            // desactiva los campos 
            document.getElementById("CmbxTipoMonedaTMH").disabled = true;//desactiva
            document.getElementById("txtYear").disabled = true;
            var Vigencia = result.Object;
            //llena los campos de cada mes
            document.getElementById("txtEneroTMH").value = parseFloat(Vigencia.Enero).toFixed(2);
            document.getElementById("txtFebreroTMH").value = parseFloat(Vigencia.Febrero).toFixed(2);
            document.getElementById("txtMarzoTMH").value = parseFloat(Vigencia.Marzo).toFixed(2);
            document.getElementById("txtAbrilTMH").value = parseFloat(Vigencia.Abril).toFixed(2);
            document.getElementById("txtMayoTMH").value = parseFloat(Vigencia.Mayo).toFixed(2);
            document.getElementById("txtJunioTMH").value = parseFloat(Vigencia.Junio).toFixed(2);
            document.getElementById("txtJulioTMH").value = parseFloat(Vigencia.Julio).toFixed(2);
            document.getElementById("txtAgostoTMH").value = parseFloat(Vigencia.Agosto).toFixed(2);
            document.getElementById("txtSeptiembreTMH").value = parseFloat(Vigencia.Septiembre).toFixed(2);
            document.getElementById("txtOctubreTMH").value = parseFloat(Vigencia.Octubre).toFixed(2);
            document.getElementById("txtNoviembreTMH").value = parseFloat(Vigencia.Noviembre).toFixed(2);
            document.getElementById("txtDiciembreTMH").value = parseFloat(Vigencia.Diciembre).toFixed(2);
            Meses(false);

        }
    }
}
function Meses(bandera) {
    //habilita o desavilita los meses
    if (bandera) {//desactiva
        document.getElementById("txtEneroTMH").disabled = true;
        document.getElementById("txtFebreroTMH").disabled = true;
        document.getElementById("txtMarzoTMH").disabled = true;
        document.getElementById("txtAbrilTMH").disabled = true;
        document.getElementById("txtMayoTMH").disabled = true;
        document.getElementById("txtJunioTMH").disabled = true;
        document.getElementById("txtJulioTMH").disabled = true;
        document.getElementById("txtAgostoTMH").disabled = true;
        document.getElementById("txtSeptiembreTMH").disabled = true;
        document.getElementById("txtOctubreTMH").disabled = true;
        document.getElementById("txtNoviembreTMH").disabled = true;
        document.getElementById("txtDiciembreTMH").disabled = true;
        //limpia
        document.getElementById("txtEneroTMH").value = "";
        document.getElementById("txtFebreroTMH").value = "";
        document.getElementById("txtMarzoTMH").value = "";
        document.getElementById("txtAbrilTMH").value = "";
        document.getElementById("txtMayoTMH").value = "";
        document.getElementById("txtJunioTMH").value = "";
        document.getElementById("txtJulioTMH").value = "";
        document.getElementById("txtAgostoTMH").value = "";
        document.getElementById("txtSeptiembreTMH").value = "";
        document.getElementById("txtOctubreTMH").value = "";
        document.getElementById("txtNoviembreTMH").value = "";
        document.getElementById("txtDiciembreTMH").value = "";
        document.getElementById("btnBuscar").style.display = "";
        //document.getElementById("btnBuscar2").style.display = "none";

    } else {//activa
        document.getElementById("txtEneroTMH").disabled = false;
        document.getElementById("txtFebreroTMH").disabled = false;
        document.getElementById("txtMarzoTMH").disabled = false;
        document.getElementById("txtAbrilTMH").disabled = false;
        document.getElementById("txtMayoTMH").disabled = false;
        document.getElementById("txtJunioTMH").disabled = false;
        document.getElementById("txtJulioTMH").disabled = false;
        document.getElementById("txtAgostoTMH").disabled = false;
        document.getElementById("txtSeptiembreTMH").disabled = false;
        document.getElementById("txtOctubreTMH").disabled = false;
        document.getElementById("txtNoviembreTMH").disabled = false;
        document.getElementById("txtDiciembreTMH").disabled = false;
        document.getElementById("btnBuscar").style.display = "none";
        //document.getElementById("btnBuscar2").style.display = "";
    }
}
function confirmarTasaMercado(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}/*
function confirmarTasaMercado(header, body, result, bandera) {
    $('#msg_modal_header_2').text(header);
    $('#msg_modal_body_2').text(body);
    $('#sch_modal_Mercado').modal('show');

    $("#btn_modal_aceptar_2").one('click', function () {
        $('#sch_modal_Mercado').modal('hide');
        if (bandera == 1)//busqueda
           NuevaTasaMercado(result, bandera);
        if (bandera == 2)//genera el nuevo registro
           NuevaTasaMercado(result, bandera);
        if (bandera == 3)//modifica el registro
           NuevaTasaMercado(result, bandera);
        if (bandera == 4)//Elimina el registro
            EliminarTasaMercado(result);
    });
}*/
function Buscar() {
    var id = $("#CmbxTipoMonedaTMH").val();
    if (id == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda");
        document.getElementById("cmbxTipoMoneda").focus();
    } else {
        var vlAnno = $("#txtYear").val();
        if (vlAnno == "") {
            aviso("Aviso", "Debe ingresar el Año a buscar o ingresar.");
        }
        else {
            var fecha = new Date();
            var ano = fecha.getFullYear();
            if (vlAnno < 1900 || vlAnno > ano) {
                aviso("Aviso", "El valor del Año ingresado no es válido");
            } else {
                var url = $("#urlBuscarYear").val();
                var res = id.split("#");
                var fields = {
                    vlMoneda: res[0],
                    vlReajuste: res[1],
                    vlAnno: vlAnno
                };
                sendValues(fields, doSuccessConsultaYear, doError, url);

            }
        }
    }
}
function NuevaTasaMercado(result, bandera) {
    // desactiva los campos 
    document.getElementById("CmbxTipoMonedaTMH").disabled = true;//desactiva
    document.getElementById("txtYear").disabled = true;
    if (bandera == 1) {//carga los campos para el nuevo registro
        var Vigencia = result.Object;
        //llena los campos de cada mes
        document.getElementById("txtEneroTMH").value = parseFloat(Vigencia.Enero).toFixed(2);
        document.getElementById("txtFebreroTMH").value = parseFloat(Vigencia.Febrero).toFixed(2);
        document.getElementById("txtMarzoTMH").value = parseFloat(Vigencia.Marzo).toFixed(2);
        document.getElementById("txtAbrilTMH").value = parseFloat(Vigencia.Abril).toFixed(2);
        document.getElementById("txtMayoTMH").value = parseFloat(Vigencia.Mayo).toFixed(2);
        document.getElementById("txtJunioTMH").value = parseFloat(Vigencia.Junio).toFixed(2);
        document.getElementById("txtJulioTMH").value = parseFloat(Vigencia.Julio).toFixed(2);
        document.getElementById("txtAgostoTMH").value = parseFloat(Vigencia.Agosto).toFixed(2);
        document.getElementById("txtSeptiembreTMH").value = parseFloat(Vigencia.Septiembre).toFixed(2);
        document.getElementById("txtOctubreTMH").value = parseFloat(Vigencia.Octubre).toFixed(2);
        document.getElementById("txtNoviembreTMH").value = parseFloat(Vigencia.Noviembre).toFixed(2);
        document.getElementById("txtDiciembreTMH").value = parseFloat(Vigencia.Diciembre).toFixed(2);
        Meses(false);
        return;
    }
    if (bandera == 2) {//genera el nuevo registro

        var mes = {
            Year: $("#txtYear").val(),
            Enero: $("#txtEneroTMH").val(),
            Febrero: $("#txtFebreroTMH").val(),
            Marzo: $("#txtMarzoTMH").val(),
            Abril: $("#txtAbrilTMH").val(),
            Mayo: $("#txtMayoTMH").val(),
            Junio: $("#txtJunioTMH").val(),
            Julio: $("#txtJulioTMH").val(),
            Agosto: $("#txtAgostoTMH").val(),
            Septiembre: $("#txtSeptiembreTMH").val(),
            Octubre: $("#txtOctubreTMH").val(),
            Noviembre: $("#txtNoviembreTMH").val(),
            Diciembre: $("#txtDiciembreTMH").val()
        };
        var url = $("#urlGrabarYear").val();
        var id = $("#CmbxTipoMonedaTMH").val();
        var res = id.split("#");
        var fields = {
            vlMoneda: res[0],
            vlReajuste: res[1],
            Meses: mes,
            bandera: true
        };
        sendValues(fields, doSuccessGrabarYear, doError, url);
        return;
    }
    if (bandera == 3) {//genera el nuevo registro
        var mes = {
            Year: $("#txtYear").val(),
            Enero: $("#txtEneroTMH").val(),
            Febrero: $("#txtFebreroTMH").val(),
            Marzo: $("#txtMarzoTMH").val(),
            Abril: $("#txtAbrilTMH").val(),
            Mayo: $("#txtMayoTMH").val(),
            Junio: $("#txtJunioTMH").val(),
            Julio: $("#txtJulioTMH").val(),
            Agosto: $("#txtAgostoTMH").val(),
            Septiembre: $("#txtSeptiembreTMH").val(),
            Octubre: $("#txtOctubreTMH").val(),
            Noviembre: $("#txtNoviembreTMH").val(),
            Diciembre: $("#txtDiciembreTMH").val()
        };
        var url = $("#urlGrabarYear").val();
        var id = $("#CmbxTipoMonedaTMH").val();
        var res = id.split("#");
        var fields = {
            vlMoneda: res[0],
            vlReajuste: res[1],
            Meses: mes,
            bandera: false
        };
        sendValues(fields, doSuccessGrabarYear, doError, url);
        return;
    }
}
function doSuccessGrabarYear(result) {

    if (result.IsOk) {
        if (result.Message == "No se encontro información") {
            aviso("Aviso", result.Message);
            Meses(true);
        } else {
            aviso("Aviso", result.Message);
            Meses(true);
            document.getElementById("CmbxTipoMonedaTMH").disabled = false;
            document.getElementById("txtYear").disabled = false;
            document.getElementById("cmbxYear").options.length = 0;
            $("#cmbxYear").append('<option value="">-- Seleccione --</option>');
            document.getElementById("cmbxYear").disabled = false;
            result.Object.forEach(function (v) {
                $("#cmbxYear").append('<option value="' + v.Year + '">' + v.Year + '</option>');
            });
        }
    }
}
function Grabar() {
    var id = $("#CmbxTipoMonedaTMH").val();
    if (id == "") {
        aviso("Aviso", "Debe indicar el Tipo de Moneda a registrar.");
        document.getElementById("cmbxTipoMoneda").focus();
        return;
    }
    var vlAnno = $("#txtYear").val();
    if (vlAnno == "") {
        aviso("Aviso", "Debe ingresar el Año a registrar.");
        return;
    }
    //verifica que esten activos los meses
    if (document.getElementById("txtEneroTMH").disabled) {
        aviso("Aviso", "Existen errores generales de información en este Año. Vuelva a ingresarlo nuevamente.");
        return;
    }
    if (($("#txtEneroTMH").val() > 100) || $("#txtEneroTMH").val() < 0) {
        aviso("Aviso", "Debe ingresar para el mes de Enero valores de tasas entre 0 y 100.");
        return;
    }
    if (($("#txtFebreroTMH").val() > 100) || $("#txtFebreroTMH").val() < 0) {
        aviso("Aviso", "Debe ingresar para el mes de Febrero valores de tasas entre 0 y 100.");
        return;
    }
    if (($("#txtMarzoTMH").val() > 100) || $("#txtMarzoTMH").val() < 0) {
        aviso("Aviso", "Debe ingresar para el mes de Marzo valores de tasas entre 0 y 100.");
        return;
    }
    if (($("#txtAbrilTMH").val() > 100) || $("#txtAbrilTMH").val() < 0) {
        aviso("Aviso", "Debe ingresar para el mes de Abril valores de tasas entre 0 y 100.");
        return;
    }
    if (($("#txtMayoTMH").val() > 100) || $("#txtMayoTMH").val() < 0) {
        aviso("Aviso", "Debe ingresar para el mes de Mayo valores de tasas entre 0 y 100.");
        return;
    }
    if (($("#txtJunioTMH").val() > 100) || $("#txtJunioTMH").val() < 0) {
        aviso("Aviso", "Debe ingresar para el mes de Junio valores de tasas entre 0 y 100.");
        return;
    }
    if (($("#txtJulioTMH").val() > 100) || $("#txtJulioTMH").val() < 0) {
        aviso("Aviso", "Debe ingresar para el mes de Julio valores de tasas entre 0 y 100.");
        return;
    }
    if (($("#txtAgostoTMH").val() > 100) || $("#txtAgostoTMH").val() < 0) {
        aviso("Aviso", "Debe ingresar para el mes de Agosto valores de tasas entre 0 y 100.");
        return;
    }
    if (($("#txtSeptiembreTMH").val() > 100) || $("#txtSeptiembreTMH").val() < 0) {
        aviso("Aviso", "Debe ingresar para el mes de Septiembre valores de tasas entre 0 y 100.");
        return;
    }
    if (($("#txtOctubreTMH").val() > 100) || $("#txtOctubreTMH").val() < 0) {
        aviso("Aviso", "Debe ingresar para el mes de Octubre valores de tasas entre 0 y 100.");
        return;
    }
    if (($("#txtNoviembreTMH").val() > 100) || $("#txtNoviembreTMH").val() < 0) {
        aviso("Aviso", "Debe ingresar para el mes de Noviembre valores de tasas entre 0 y 100.");
        return;
    }
    if (($("#txtDiciembreTMH").val() > 100) || $("#txtDiciembreTMH").val() < 0) {
        aviso("Aviso", "Debe ingresar para el mes de Diciembre valores de tasas entre 0 y 100.");
        return;
    }
    //se ejecuta la funcion de grabar
    var url = $("#urlBuscarYear").val();
    var res = id.split("#");
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
        vlAnno: vlAnno
    };
    sendValues(fields, doSuccessVerificarYear, doError, url);
}
function doSuccessVerificarYear(result) {
    if (result.Message == "No se encontro información") {
        confirmarTasaMercado("Aviso", "¿ Está seguro que desea Grabar la Información ?", function () { NuevaTasaMercado(result, 2); });
        //confirmarTasaMercado("Aviso", "¿ Está seguro que desea Grabar la Información ?", result, 2);
        return;
    }
    if (result.Message == "Información cargada con éxito") {
        confirmarTasaMercado("Aviso", "¿Está seguro que desea Modificar los Datos del Año del Histórico de Tasas ?", function () { NuevaTasaMercado(result, 3); });
        //confirmarTasaMercado("Aviso", "¿Está seguro que desea Modificar los Datos del Año del Histórico de Tasas ?", result, 3);
        return;
    }
}
function Limpiar() {
    Meses(true);
    $('#CmbxTipoMonedaTMH').val('');
    document.getElementById("CmbxTipoMonedaTMH").disabled = false;
    document.getElementById("txtYear").disabled = false;
    document.getElementById("txtYear").value = "";
    document.getElementById("cmbxYear").options.length = 0;
    $("#cmbxYear").append('<option value="">-- Seleccione --</option>');
    document.getElementById("cmbxYear").disabled = false;
}
function Eliminar() {
    var id = $("#CmbxTipoMonedaTMH").val();
    if (id == "") {
        aviso("Aviso", "Debe indicar el Tipo de Moneda.");
        document.getElementById("cmbxTipoMoneda").focus();
        return;
    }
    var vlAnno = $("#txtYear").val();
    if (vlAnno == "") {
        aviso("Aviso", "Debe ingresar el Año del Histórico de Tasas a eliminar.");
        return;
    }
    //se ejecuta la funcion
    var url = $("#urlBuscarYear").val();
    var res = id.split("#");
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
        vlAnno: vlAnno
    };
    sendValues(fields, doSuccessEliminarYear, doError, url);
}
function doSuccessEliminarYear(result) {
    //Información cargada con éxito
    if (result.Message == "Información cargada con éxito") {
        //confirmarTasaMercado("Aviso", "¿ Está seguro que desea Eliminar el Año del Tipo de Moneda seleccionada ? ", result, 4);
        confirmarTasaMercado("Aviso", "¿ Está seguro que desea Eliminar el Año del Tipo de Moneda seleccionada ?", function () { EliminarTasaMercado(result); });
    } else {
        aviso("Aviso", "El Año que está intentando Eliminar no se encuentra en la BD");
    }
}
function EliminarTasaMercado(result) {
    var id = $("#CmbxTipoMonedaTMH").val();
    var vlAnno = $("#txtYear").val();
    var res = id.split("#");
    //se ejecuta la funcion Eliminar
    var url = $("#urlEliminarYear").val();
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
        year: vlAnno
    };
    sendValues(fields, doSuccess, doError, url);
}
function doSuccess(result) {
    if (result.IsOk) {
        if (result.Message == "No se encontro información") {
            aviso("Aviso", "No se pudo elimimar la información");
            Meses(true);
        } else {
            aviso("Aviso", result.Message);
            Meses(true);
            document.getElementById("CmbxTipoMonedaTMH").disabled = false;
            document.getElementById("txtYear").disabled = false;
            document.getElementById("cmbxYear").options.length = 0;
            $("#cmbxYear").append('<option value="">-- Seleccione --</option>');
            document.getElementById("cmbxYear").disabled = false;
            result.Object.forEach(function (v) {
                $("#cmbxYear").append('<option value="' + v.Year + '">' + v.Year + '</option>');
            });
        }
    }
}
function Imprimir() {
    var url = $("#urlReporte").val();
    var id = $("#CmbxTipoMonedaTMH").val();
    var tex = $("#CmbxTipoMonedaTMH option:selected").text();

    if (id == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda");
        document.getElementById("cmbxTipoMoneda").focus();
        return;
    }
    var res = id.split("#");
    window.open(url + "?vlMoneda=" + res[0] + "&vlReajuste=" + res[1] + "&moneda=" + tex);
}
function Salir() {
    var url = $("#urlSalir").val();
    window.location.href = url;
}
function soloNumeros(e) {
    var key = window.event ? e.which : e.keyCode;
    if (key < 48 || key > 57) {
        e.preventDefault();
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