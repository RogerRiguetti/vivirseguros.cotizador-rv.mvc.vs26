//var grabarPar = 1;
(function () {
    localStorage.pagina = 'ParametroGasto';
    deshabilitaCampos();
    ConsultaOtrosGatos();
    //Funcion para el llenado del combo de Periodos (Combo Moneda).
    $("#cmbxTipoMoneda").change(function () {
        var id = $("#cmbxTipoMoneda").val();
        if (id == "") {
            document.getElementById("CmbxPeriodos").options.length = 0;
            $("#CmbxPeriodos").append('<option value="">-- Seleccione --</option>');

            document.getElementById("fecIni").value = $("#backFecha").val();
            document.getElementById("fecFin").value = $("#backFecha").val();

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

    //Función para la selección de periodos en el combo.
    $("#CmbxPeriodos").change(function () {
        var strFec = $("#CmbxPeriodos").val();
        if (strFec == "") {
            //actica la caja de fecha inicio para agregar una nueva fecha
            $('#fecIni').attr("disabled", false);
            $('#cmbxTipoMoneda').attr("disabled", false);
            document.getElementById("fecIni").value = $("#backFecha").val();
            document.getElementById("fecFin").value = $("#backFecha").val();
            document.getElementById("txtGastosA").value = "";
            document.getElementById("txtGastosE").value = "";
            document.getElementById("txtGastosCS").value = "";
            document.getElementById("txtNivelE").value = "";
            document.getElementById("txtTasaM").value = "";
            document.getElementById("txtImpuestoR").value = "";
            document.getElementById("btnBuscar").style.display = "";
            document.getElementById("btnBuscar2").style.display = "none";

            //deshabilitaCampos();
        } else {
            deshabilitaCampos();

            var id = $("#cmbxTipoMoneda").val();
            var url = $("#urlBuscaVigencia").val();
            var res = id.split("#");
            var fields = {
                vlMoneda: res[0],
                vlReajuste: res[1],
                strFecIni: strFec
            };
            sendValues(fields, doSuccessBuscarVigencia, doError, url);

            NuevaTasaMercado();
            NuevoImpuestoRenta();
        }
    });

})();

//Función para cargar datos del combo periodos.
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

function ConsultaOtrosGatos() {
    var urlConsultaGastos = $("#urlConsultaOtrosGastos").val();
    var fields = {
    };
    sendValues(fields, doSuccessConsultaGastos, doError, urlConsultaGastos);
}
function doSuccessConsultaGastos(result) {
    var res = result.Object;
    document.getElementById("txtCOMSUP1").value = res.PrccomS1;
    document.getElementById("txtCOMSUP2").value = res.PrccomS2;
    document.getElementById("txtPRCFAC1").value = res.Prcfaclab1;
    document.getElementById("txtPRCFAC2").value = res.Prcfaclab2;
}
//////////////////////BOTONES////////////////////////////

//Función para botón Grabar.
function Grabar() {
    // Validar que no haya campos vacíos.
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

    var gastosA = $("#txtGastosA").val();
    if (gastosA == "") {
        aviso("Aviso", "Debe Ingresar Gastos de Administración");
        document.getElementById("txtGastosA").focus();
        return;
    }

    var gastosE = $("#txtGastosE").val();
    if (gastosE == "") {
        aviso("Aviso", "Debe Ingresar Gastos de Emisión");
        document.getElementById("txtGastosE").focus();
        return;
    }

    var gastosCS = $("#txtGastosCS").val();
    if (gastosCS == "") {
        aviso("Aviso", "Debe Ingresar Gastos Control Supervivencia");
        document.getElementById("txtGastosCS").focus();
        return;
    }

    var nivelE = $("#txtNivelE").val();
    if (nivelE == "") {
        aviso("Aviso", "Debe Ingresar El Nivel de Endeudamiento");
        document.getElementById("txtNivelE").focus();
        return;
    }

    //Validar los valores númericos de los campos.
    if (gastosCS < 0 || gastosCS > 100) {
        aviso("Aviso", "El Valor del Porcentaje debe ser entre 0 y 100");
        document.getElementById("txtGastosCS").focus();
        return;
    }

    if (nivelE < 0 || nivelE > 100) {
        aviso("Aviso", "El Valor del Porcentaje debe ser entre 0 y 100");
        document.getElementById("txtNivelE").focus();
        return;
    }

    //Se busca la vigencia para confirmar si es modificación o nueva inserción.
    var id = $("#cmbxTipoMoneda").val();
    var url = $("#urlBuscaVigencia").val();
    var res = id.split("#");
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
        strFecIni: strFec
    };
    sendValues(fields, doSuccessGrabarPeriodo, doError, url);
}

function GrabarOtrosGastos() {
    // Validar que no haya campos vacíos.


    var COMSUP1 = $("#txtCOMSUP1").val();
    var COMSUP2 = $("#txtCOMSUP2").val();
    var PRCFAC1 = $("#txtPRCFAC1").val();
    var PRCFAC2 = $("#txtPRCFAC2").val();
    if (COMSUP1 == "" || COMSUP2 == "" || PRCFAC1 == "" || PRCFAC2 == "") {
        aviso("Aviso", "No debe tener ningun valor vacio ");
        document.getElementById("txtCOMSUP1").focus();
        document.getElementById("txtCOMSUP2").focus();
        document.getElementById("txtPRCFAC1").focus();
        document.getElementById("txtPRCFAC2").focus();
        return;
    }

    
    //Se busca la vigencia para confirmar si es modificación o nueva inserción.
    var url = $("#urlGuardarOtrosGastos").val();
    var fields = {
        COMSUP1: COMSUP1,
        COMSUP2: COMSUP2,
        PRCFAC1: PRCFAC1,
        PRCFAC2: PRCFAC2
    };
    sendValues(fields, doSuccessGrabarOtrosGastos, doError, url);
}

//Función para el botón de eliminar.
function Eliminar() {
    var id = $("#cmbxTipoMoneda").val();
    if (id == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda para Eliminar");
        document.getElementById("cmbxTipoMoneda").focus();
        return;
    }

    var strFecI = $("#fecIni").val();
    var strFecF = $("#fecFin").val();
    if (strFecI == "" || strFecF == "") {
        aviso("Aviso", "Debe seleccionar un rango de vigencia para Eliminar");
        document.getElementById("fecIni").focus();
        return;
    }

    //Se busca la vigencia para confirmar si es modificación o nueva inserción.
    var id = $("#cmbxTipoMoneda").val();
    var url = $("#urlBuscaVigencia").val();
    var res = id.split("#");
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
        strFecIni: strFecI
    };
    sendValues(fields, doSuccessEliminarPeriodo, doError, url);
}

//Función para botón de limpiar, habilitando y deshabilitando campos.
function Limpiar() {
    $('#cmbxTipoMoneda').val('');
    document.getElementById("CmbxPeriodos").options.length = 0;
    $("#CmbxPeriodos").append('<option value="">-- Seleccione --</option>');

    $('#cmbxTipoMoneda').attr("disabled", false);
    $('#CmbxPeriodos').attr("disabled", false);
    $('#fecIni').attr("disabled", false);
    $('#fecFin').attr("disabled", true);
    $('#txtGastosA').attr("disabled", true);
    $('#txtGastosE').attr("disabled", true);
    $('#txtGastosCS').attr("disabled", true);
    $('#txtNivelE').attr("disabled", true);
    $('#txtTasaM').attr("disabled", true);
    $('#txtImpuestoR').attr("disabled", true);
    document.getElementById("btnBuscar").style.display = "";
    document.getElementById("btnBuscar2").style.display = "none";

    document.getElementById("fecIni").value = $("#backFecha").val();
    document.getElementById("fecFin").value = $("#backFecha").val();
    document.getElementById("txtGastosA").value = "";
    document.getElementById("txtGastosE").value = "";
    document.getElementById("txtGastosCS").value = "";
    document.getElementById("txtNivelE").value = "";
    document.getElementById("txtTasaM").value = "";
    document.getElementById("txtImpuestoR").value = "";
}

//Función para botón de Buscar.
function Buscar() {
    var strMon = $("#cmbxTipoMoneda").val();

    if (strMon == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda");
        document.getElementById("cmbxTipoMoneda").focus();
    } else {
        var fechaIni = $("#fecIni").val();

        if (fechaIni == "") {
            aviso("Aviso", "Debe Ingresar la Fecha de Inicio de vigencia");
            document.getElementById("fecIni").focus();

        } else {
            var id = $("#cmbxTipoMoneda").val();
            var url = $("#urlBuscaVigencia").val();
            var res = id.split("#");
            var fields = {
                vlMoneda: res[0],
                vlReajuste: res[1],
                strFecIni: fechaIni
            };
            sendValues(fields, doSuccessBuscarVigencia, doError, url);
        }
    }
}

//Función para botón de Imprimir.
function Imprimir() {
    var strFec = $("#CmbxPeriodos").val();

    var url = $("#urlReporte").val();
    var id = $("#cmbxTipoMoneda").val();
    var tex = $("#cmbxTipoMoneda option:selected").text();
    var res = id.split("#");

    if (id == "") {
        aviso("Aviso", "Debe seleccionar un Tipo de Moneda");
        document.getElementById("cmbxTipoMoneda").focus();
        return;
    }

    if (strFec == "") {
        aviso("Aviso", "Debe seleccionar un Periodo");
        document.getElementById("CmbxPeriodos").focus();
        return;
    }
    strFec = $("#fecIni").val();
    window.open(url + "?vlMoneda=" + res[0] + "&strMoneda=" + tex + "&vlReajuste=" + res[1] + "&strFecIni=" + strFec);
}

//Función para botón de Salir.
function Salir() {
    var url = $("#urlSalir").val();
    window.location.href = url;
}


////////////////////////////////MÉTODOS DOSUCCESS/////////////////////////////////////////

//Función para búsqueda de la vigencia y los valores de los campos correspondientes.
function doSuccessBuscarVigencia(result) {
    if (result.IsOk) {
        var vigencia = result.Object;
        if (result.Message == "No se encontro información") {
            document.getElementById("fecFin").value = vigencia.FechaTermino;
            confirmarAnclaje("Aviso", "La Fecha Indicada no se encuentra registrada. " + "\n ¿Desea Ingresar esta nueva Fecha de Vigencia?", function () { NuevaTasaMercado(); NuevoImpuestoRenta(); });
            return;

        } else {
            //Asignar los valores a los campos.
            var Vigencia = result.Object;
            document.getElementById("fecIni").value = Vigencia.FechaInicial;
            document.getElementById("fecFin").value = Vigencia.FechaTermino;
            document.getElementById("txtGastosA").value = Vigencia.Mto_GastosAdmin;
            document.getElementById("txtGastosE").value = Vigencia.Mto_GastosEmi;
            document.getElementById("txtGastosCS").value = Vigencia.PRC_GastosCtrlSuper;
            document.getElementById("txtNivelE").value = Vigencia.PRC_Endeudamiento;
            document.getElementById("fecIni").disabled = true;
            document.getElementById("cmbxTipoMoneda").disabled = true;
            document.getElementById("btnBuscar").style.display = "none";
            document.getElementById("btnBuscar2").style.display = "";
            NuevaTasaMercado();
            NuevoImpuestoRenta();
            habilitarCampos();
        }
    }
}

//Función para modificar o insertar nuevos registros de periodos en la BD.
function doSuccessGrabarPeriodo(result) {
    if (result.Message == "No se encontro información") {
        confirmarAnclaje("Aviso", "¿ Está seguro que desea Grabar la Información ?", function () { NuevoParametroGasto(result, 2); });
        return;
    }
    if (result.Message == "Información cargada con éxito") {
        confirmarAnclaje("Aviso", "¿ Está seguro que desea Modificar la Información ?", function () { NuevoParametroGasto(result, 3); });
        return;
    }
}

function doSuccessGrabarOtrosGastos(result) {
    if (result.IsOk == true) {
        aviso("Aviso", "Se a guardado la información correctamente");
    }
    if (result.IsOk == false) {
        aviso("Aviso", "ERROR al guardar la información" + result.Message);
    }
}

//Función para eliminar un registro de periodos y sus valores de la BD.
function doSuccessEliminarPeriodo(result) {
    if (result.Message == "Información cargada con éxito") {
        confirmarAnclaje("Aviso", "¿ Está seguro que desea Eliminar la Información ?", function () { EliminarParametro(result); });
    } else {
        aviso("Aviso", "El Período que está intentando Eliminar no se encuentra en la BD");
    }
}

//Función para asignar el valor de la tasa de mercado.
function doSuccessTasaMercado(result) {
    if (result.IsOk) {
        var Tasa = result.Object;
        document.getElementById("txtTasaM").value = parseFloat(Tasa.PRC_TasaMercado).toFixed(2);
    }
}

//Función para asignar el valor del impuesto de la renta.
function doSuccessImpuestoRen(result) {
    if (result.IsOk) {
        var Tasa = result.Object;
        document.getElementById("txtImpuestoR").value = parseFloat(Tasa.PRC_ImpuestoRenta).toFixed(2);
    }
}

//Función para asignar valores a los campos y mensaje de confirmación al insertar o modificar registros.
function doSuccessGrabarParametros(result) {
    var Vigencia = result.Object;
    document.getElementById("fecFin").value = Vigencia.FechaTermino;
    document.getElementById("txtGastosA").value = Vigencia.Mto_GastosAdmin;
    document.getElementById("txtGastosE").value = Vigencia.Mto_GastosEmi;
    document.getElementById("txtGastosCS").value = Vigencia.PRC_GastosCtrlSuper;
    document.getElementById("txtNivelE").value = Vigencia.PRC_Endeudamiento;
    NuevaTasaMercado();
    NuevoImpuestoRenta();

    aviso("Aviso", result.Message);
    Limpiar();
}

//Función para mensaje de confirmación y limpiar campos al eliminar registros.
function doSuccessEliminarParametros(result) {
    var Vigencia = result.Object;

    document.getElementById("fecIni").value = $("#backFecha").val();
    document.getElementById("fecFin").value = $("#backFecha").val();

    aviso("Aviso", "La Información se ha eliminado Satisfactoriamente");
    Limpiar();
}

//Función para procesos de modifiación o inserción.
function NuevoParametroGasto(result, bandera) {
    var Vigencia = result.Object;
    document.getElementById("fecIni").value = Vigencia.FechaInicial;
    document.getElementById("fecFin").value = Vigencia.FechaTermino;
    /*if (bandera == 1) {
        document.getElementById("txtTasa").value = Vigencia.Tasa;
        document.getElementById("txtTasa").focus();
    }*/

    if (bandera == 2) {//INSERT
        var url = $("#urlGrabarVigencia").val();
        var id = $("#cmbxTipoMoneda").val();
        var valGastosA = $("#txtGastosA").val();
        var valGastosE = $("#txtGastosE").val();
        var valGastosCS = $("#txtGastosCS").val();
        var valNivelE = $("#txtNivelE").val();
        var res = id.split("#");
        var fields = {
            vlMoneda: res[0],
            vlReajuste: res[1],
            strFecIni: Vigencia.FechaInicial,
            strFecFin: Vigencia.FechaTermino,
            gastosCS: valGastosCS,
            gastosA: valGastosA,
            gastosE: valGastosE,
            ctoCapital: 0,
            nivelE: valNivelE,
            bandera: true
        };
        sendValues(fields, doSuccessGrabarParametros, doError, url);
        return;
    }

    if (bandera == 3) {//UPDATE
        document.getElementById("CmbxPeriodos").disabled = false;
        var url = $("#urlGrabarVigencia").val();
        var id = $("#cmbxTipoMoneda").val();
        var valGastosA = $("#txtGastosA").val();
        var valGastosE = $("#txtGastosE").val();
        var valGastosCS = $("#txtGastosCS").val();
        var valNivelE = $("#txtNivelE").val();
        var res = id.split("#");
        var fields = {
            vlMoneda: res[0],
            vlReajuste: res[1],
            strFecIni: Vigencia.FechaInicial,
            strFecFin: Vigencia.FechaTermino,
            gastosCS: valGastosCS,
            gastosA: valGastosA,
            gastosE: valGastosE,
            ctoCapital: 0,
            nivelE: valNivelE,
            bandera: false
        };


        sendValues(fields, doSuccessGrabarParametros, doError, url);
        return;
    }
}

//Función para eliminación de registros.
function EliminarParametro(result) {
    //var Vigencia = result.Object;
    //document.getElementById("fecIni").value = Vigencia.FechaInicial;
    var strFecI = $("#fecIni").val();

    var url = $("#urlEliminarVigencia").val();
    var id = $("#cmbxTipoMoneda").val();
    var res = id.split("#");
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
        strFecIni: strFecI
    };
    sendValues(fields, doSuccessEliminarParametros, doError, url);
    return;

}

//Función para calcular la Tasa de Mercado.
function NuevaTasaMercado() {
    $('#cmbxTipoMoneda').attr("disabled", true);
    $('#fecIni').attr("disabled", true);
    $('#fecFin').attr("disabled", true);
    $('#txtGastosA').attr("disabled", false);
    $('#txtGastosE').attr("disabled", false);
    $('#txtGastosCS').attr("disabled", false);
    $('#txtNivelE').attr("disabled", false);
    $('#txtTasaM').attr("disabled", true);
    $('#txtImpuestoR').attr("disabled", true);
    document.getElementById("btnBuscar").style.display = "";
    document.getElementById("btnBuscar2").style.display = "none";

    var id = $("#cmbxTipoMoneda").val();
    var url = $("#urlTasaMercado").val();
    var res = id.split("#");
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
    };
    sendValues(fields, doSuccessTasaMercado, doError, url);
}

//Función para calcular el Impuesto de la Renta.
function NuevoImpuestoRenta() {
    var strFec = $("#fecIni").val();
    var url = $("#urlImpuestoRenta").val();
    var fields = {
        strFecIni: strFec
    };
    sendValues(fields, doSuccessImpuestoRen, doError, url);
}

//Función para desactivar y/o activar campos.
function deshabilitaCampos() {
    $('#CmbxPeriodos').attr("disabled", false);
    $('#fecFin').attr("disabled", true);
    $('#txtGastosA').attr("disabled", true);
    $('#txtGastosE').attr("disabled", true);
    $('#txtGastosCS').attr("disabled", true);
    $('#txtNivelE').attr("disabled", true);
    $('#txtTasaM').attr("disabled", true);
    $('#txtImpuestoR').attr("disabled", true);
}

function habilitarCampos() {
    $('#cmbxTipoMoneda').attr("disabled", true);
    $('#fecIni').attr("disabled", true);
    $('#txtGastosA').attr("disabled", false);
    $('#txtGastosE').attr("disabled", false);
    $('#txtGastosCS').attr("disabled", false);
    $('#txtNivelE').attr("disabled", false);
}

//Función para modal de confirmación.
function confirmarAnclaje(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
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