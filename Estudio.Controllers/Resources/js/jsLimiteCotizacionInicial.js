var FilaSelecionada = "";//indica la fila a modificar o insertar
var archivosProcesar;
var mensaje = $("#urlMensaje").val();
(function () {
    localStorage.pagina = 'LimiteCotizacionInicial';
    //llamada para el llenado de los periodos
    var url = $("#urlPeriodo").val();
    var moneda = $("#cmbxTipoMoneda").val();
    var dep = $("#cmbxDepartamentos").val();
    var res = moneda.split("#");
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
        departamento: dep
    };
    sendValues(fields, doSuccessPeriodos, doError, url);
    //Funcion para el llenado del combo de Periodos
    $("#cmbxDepartamentos").change(function () {
        var dep = $("#cmbxDepartamentos").val();
        var id = $("#cmbxTipoMoneda").val();
        var url = $("#urlPeriodo").val();
        var res = id.split("#");
        var fields = {
            vlMoneda: res[0],
            vlReajuste: res[1],
            departamento: dep
        };
        sendValues(fields, doSuccessPeriodos, doError, url);

    });
    //Funcion de la carga masiva
    $('#tituloCargaMasiva').click(() => {
        $('#contCargaMasiva').slideToggle('normal');
    });
    $("#cmbxTipoMoneda").change(function () {
        var dep = $("#cmbxDepartamentos").val();
        var id = $("#cmbxTipoMoneda").val();
        var url = $("#urlPeriodo").val();
        var res = id.split("#");
        var fields = {
            vlMoneda: res[0],
            vlReajuste: res[1],
            departamento: dep
        };
        sendValues(fields, doSuccessPeriodos, doError, url);
    });
    //al seleccionar un periodo
    $("#CmbxPeriodos").change(function () {
        var strFec = $("#CmbxPeriodos").val();
        if (strFec != "") {
            var url = $("#urlBuscar").val();
            var id = $("#cmbxTipoMoneda").val();
            var dep = $("#cmbxDepartamentos").val();
            var res = id.split("#");
            var fields = {
                fechaIni: strFec,
                codMoneda: res[0],
                reajuste: res[1],
                departamento: dep
            };
            sendValues(fields, doSuccessBuscarVigencia, doError, url);
        }
    });
    $("#cmbxRangoTasa").change(function () {
        FilaSelecionada = $("#cmbxRangoTasa").val();
        var tabla = document.getElementById("tblTasaVenta");
        var posicion = tabla.getElementsByClassName("posicion");
        var valor = tabla.getElementsByClassName("valor");
        for (var i = 0; i < posicion.length; i++) {
            var id = posicion[i].innerHTML;
            if (id == FilaSelecionada) {
                $("#txtTV").val(valor[i].innerHTML);
                break;
            }
            else
                $("#txtTV").val("");

        }


    });
    if (mensaje != null && mensaje != "") {
        aviso("AVISO", mensaje);
        mensaje = "";
    }

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
            //confirmarCotizacionMejorada("Aviso", "La Fecha Indicada no se encuentra registrada ¿ Desea Ingresar esta Nueva Fecha de Vigencia ?", 2, result);
            confirmarCotizacionMejorada("Aviso", "La Fecha Indicada no se encuentra registrada ¿ Desea Ingresar esta Nueva Fecha de Vigencia ?", function () { NuevaCotizaccionMejorada(2, result); });
        } else {
            var Vigencia = result.Object;
            //fechas
            document.getElementById("fecIni").value = Vigencia.fechaInicial;
            document.getElementById("fecFin").value = Vigencia.FechaFinal;
            //TIR
            document.getElementById("txtMinTIR").value = parseFloat(Vigencia.Minimo_Tir).toFixed(2);
            document.getElementById("txtMinTIRInmediatas").value = parseFloat(Vigencia.Minimo_TirInmediatas).toFixed(2);
            //Perdida
            document.getElementById("txtMaxPer").value = parseFloat(Vigencia.Maximo_Per).toFixed(2);
            //Comision
            document.getElementById("txtMinCom").value = parseFloat(Vigencia.Minimo_Com).toFixed(2);
            document.getElementById("txtMaxCom").value = parseFloat(Vigencia.Maximo_Com).toFixed(2);
            document.getElementById("txtComInm").value = parseFloat(Vigencia.ComisionInmediata).toFixed(2);
            document.getElementById("txtComDif").value = parseFloat(Vigencia.ComisionDiferida).toFixed(2);

            //Tasa de venta
            // document.getElementById("txtMinTIR").value = Vigencia.Minimo_Tir;
            document.getElementById("fecIni").disabled = true;//desactiva
            document.getElementById("cmbxTipoMoneda").disabled = true;
            document.getElementById("cmbxDepartamentos").disabled = true;
            document.getElementById("btnBuscar").style.display = "none";
            document.getElementById("btnBuscar2").style.display = "";
            //document.getElementById("tabulador").style.display = "block";
            $("#tabulador li").removeClass("disabled");

            //document.getElementById("tabulador2").style.display = "";
            document.getElementById("tabulador3").style.display = "";
            // document.getElementById("txtMinTIR").disabled = false;

            var $body = $("#tblTasaVenta tbody");
            $body.empty();
            Vigencia.RangosTasaVenta.forEach(function (v) {
                var d = "this.id,'" + v.CodPension + "'"
                var fila = '<tr id="' + v.CodPension + '" onclick="seleccionar(' + d + ');">  <td class="posicion" >' + v.NombrePension + '</td>'
                                                                                 + ' <td class="valor">' + parseFloat(v.Maximo_TV).toFixed(2) + '</td>'
                                                                                 + '</tr>';
                $('#tblTasaVenta').append(fila);
            });

        }
    }
}
//funciones de lleado de tabla de Tasa Venta
function seleccionar(id_fila, pension) {

    if (pension == "") {
        $("#tblTasaVenta tr").click(function () {
            $("#tblTasaVenta tr").removeClass('focus');
            $(this).addClass('');
        });
    } else {
        $("#tblTasaVenta tr").click(function () {
            $("#tblTasaVenta tr").removeClass('focus');
            $(this).addClass('focus');
        });
        var tabla = document.getElementById("tblTasaVenta");
        $("#cmbxRangoTasa").val(pension);
        $("#txtTV").val(tabla.rows[pension].cells[1].innerText);
        FilaSelecionada = pension;
    }
}
function AgregarFila() {
    var tabla = document.getElementById("tblTasaVenta");
    var porcentaje = $("#txtTV").val()
    var agregar = true;
    var posicion = tabla.getElementsByClassName("posicion");
    if (FilaSelecionada == "")
        FilaSelecionada = $("#cmbxRangoTasa").val();
    for (var i = 0; i < posicion.length; i++) {//realiza una busqueda 
        var id = posicion[i].innerHTML;
        if (id == FilaSelecionada) {
            agregar = false;
        }
    }
    if (porcentaje == "") {
        aviso("Aviso", "Debe ingresar el porcentaje de Tasa de Venta Máximo.");
        return;
    }
    if (parseFloat(porcentaje) < 0 || parseFloat(porcentaje) > 100) {
        aviso("Aviso", "El Valor del Porcentaje debe ser entre 0 y 100");
        return;
    }

    if (FilaSelecionada == "") {
        FilaSelecionada = $("#cmbxRangoTasa").val();
        if (agregar == false) {//actualiza la info
            tabla.rows[FilaSelecionada].cells[0].innerText = $("#cmbxRangoTasa").val();
            tabla.rows[FilaSelecionada].cells[1].innerText = parseFloat(porcentaje).toFixed(2);
        } else {//agrega el nuevo
            var d = "this.id,'" + $("#cmbxRangoTasa").val() + "'"
            var fila = '<tr id="' + $("#cmbxRangoTasa").val() + '" onclick="seleccionar(' + d + ');">  <td class="posicion" >' + $("#cmbxRangoTasa").val() + '</td>'
                                                                             + ' <td class="valor">' + parseFloat(porcentaje).toFixed(2) + '</td>'
                                                                             + '</tr>';
            $('#tblTasaVenta').append(fila);

        }
    } else {//actualiza la fila
        if (agregar == false) {//actualiza la info
            tabla.rows[FilaSelecionada].cells[0].innerText = $("#cmbxRangoTasa").val();
            tabla.rows[FilaSelecionada].cells[1].innerText = parseFloat(porcentaje).toFixed(2);
        } else {//agrega el nuevo
            var d = "this.id,'" + $("#cmbxRangoTasa").val() + "'"
            var fila = '<tr id="' + $("#cmbxRangoTasa").val() + '" onclick="seleccionar(' + d + ');">  <td class="posicion" >' + $("#cmbxRangoTasa").val() + '</td>'
                                                                             + ' <td class="valor">' + parseFloat(porcentaje).toFixed(2) + '</td>'
                                                                             + '</tr>';
            $('#tblTasaVenta').append(fila);

        }
    }

}
function EliminarFila() {
    if (FilaSelecionada != "") {
        var borra = false;
        var tabla = document.getElementById("tblTasaVenta");
        var posicion = tabla.getElementsByClassName("posicion");
        for (var i = 0; i < posicion.length; i++) {
            var id = posicion[i].innerHTML;
            if (id == FilaSelecionada) {
                //confirmarCotizacionMejorada("Aviso", "¿ Está seguro que desea Eliminar este Tipo de Pensión " + FilaSelecionada + " ?", 1, "");
                confirmarCotizacionMejorada("Aviso", "¿ Está seguro que desea Eliminar este Tipo de Pensión " + FilaSelecionada + " ?", function () {
                    $("#" + FilaSelecionada).remove();
                    $("#txtTV").val("")
                    FilaSelecionada = "";
                });
                break;
            }
        }
        return;
    }
}
function LimpiarFila() {
    $("#cmbxRangoTasa").val("04")
    $("#txtTV").val("")

}
//Fin de funciones de lleado de tabla de Tasa Venta
function NuevaCotizaccionMejorada(bandera, result) {
    if (bandera == 2) {//limpiamos los campos para la nuevos datos a recibir
        var Vigencia = result.Object;
        document.getElementById("fecIni").value = Vigencia.fechaInicial;
        document.getElementById("fecFin").value = Vigencia.FechaFinal;
        document.getElementById("fecIni").disabled = true;
        document.getElementById("cmbxTipoMoneda").disabled = true;
        document.getElementById("cmbxDepartamentos").disabled = true;
        //botones y tabulador
        document.getElementById("btnBuscar").style.display = "none";
        document.getElementById("btnBuscar2").style.display = "";
        //document.getElementById("tabulador").style.display = "none";
        //document.getElementById("tabulador2").style.display = "";
        $("#tabulador li").removeClass("disabled");
        document.getElementById("tabulador3").style.display = "";
        //TIR document.getElementById("txtMinTIR").disabled = false;
        document.getElementById("txtMinTIR").value = "";
        document.getElementById("txtMinTIRInmediata").value = "";
        //Perdida
        document.getElementById("txtMaxPer").value = "";
        //Comision
        document.getElementById("txtMinCom").value = "";
        document.getElementById("txtMaxCom").value = "";
        document.getElementById("txtComInm").value = "";
        document.getElementById("txtComDif").value = "";
        //limpia la tabla de TV
        document.getElementById("txtTV").value = "";
        var $body = $("#tblTasaVenta tbody");
        $body.empty();
        return;
    }
    if (bandera == 3) {
        var tabla = document.getElementById("tblTasaVenta");
        var posicion = tabla.getElementsByClassName("posicion");
        var valor = tabla.getElementsByClassName("valor");
        var infoTabla = [];
        var valorMinimoTV = $("#vgMelerTasaVtaMin").val();
        var valorMaximoTIR = $("#vgMelerTirMax").val();
        var valorMinimoPER = $("#vgMelerPerConMin").val();
        for (var i = 0; i < posicion.length; i++) {
            var id = posicion[i].innerHTML;
            var maximo = valor[i].innerHTML;
            var infoTablas = { CodPension: id, Maximo_TV: maximo, Minimo_TV: valorMinimoTV }
            infoTabla.push(infoTablas);
        }
        var dep = $("#cmbxDepartamentos").val();
        var id = $("#cmbxTipoMoneda").val();
        var res = id.split("#");
        var info = {
            CodDepartamento: dep,
            CodMoneda: res[0],
            CodReajuste: res[1],
            Minimo_Tir: $("#txtMinTIR").val(),
            Minimo_TirInmediatas: $("#txtMinTIRInmediatas").val(),
            Maximo_Tir: valorMaximoTIR,
            Minimo_Per: valorMinimoPER,
            Maximo_Per: $("#txtMaxPer").val(),
            Minimo_Com: $("#txtMinCom").val(),
            Maximo_Com: $("#txtMaxCom").val(),
            ComisionInmediata: $("#txtComInm").val(),
            ComisionDiferida: $("#txtComDif").val(),
            RangosTasaVenta: infoTabla
        };
        var strFec = $("#fecIni").val();
        var url = $("#urlGrabar").val();

        var fields = {
            informacion: info,
            fechaIni: strFec,
            bandera: true
        };
        sendValues(fields, doSuccessGrabarVigencia, doError, url);
        return;
    }
    if (bandera == 4) {//actualiza
        var tabla = document.getElementById("tblTasaVenta");
        var posicion = tabla.getElementsByClassName("posicion");
        var valor = tabla.getElementsByClassName("valor");
        var infoTabla = [];
        var valorMinimoTV = $("#vgMelerTasaVtaMin").val();
        var valorMaximoTIR = $("#vgMelerTirMax").val();
        var valorMinimoPER = $("#vgMelerPerConMin").val();
        for (var i = 0; i < posicion.length; i++) {
            var id = posicion[i].innerHTML;
            var maximo = valor[i].innerHTML;
            var infoTablas = { CodPension: id, Maximo_TV: maximo, Minimo_TV: valorMinimoTV }
            infoTabla.push(infoTablas);
        }
        var dep = $("#cmbxDepartamentos").val();
        var id = $("#cmbxTipoMoneda").val();
        var res = id.split("#");
        var info = {
            CodDepartamento: dep,
            CodMoneda: res[0],
            CodReajuste: res[1],
            Minimo_Tir: $("#txtMinTIR").val(),
            Minimo_TirInmediatas: $("#txtMinTIRInmediatas").val(),
            Maximo_Tir: valorMaximoTIR,
            Minimo_Per: valorMinimoPER,
            Maximo_Per: $("#txtMaxPer").val(),
            Minimo_Com: $("#txtMinCom").val(),
            Maximo_Com: $("#txtMaxCom").val(),
            ComisionInmediata: $("#txtComInm").val(),
            ComisionDiferida: $("#txtComDif").val(),
            RangosTasaVenta: infoTabla
        };
        var strFec = $("#fecIni").val();
        var url = $("#urlGrabar").val();

        var fields = {
            informacion: info,
            fechaIni: strFec,
            bandera: false
        };
        sendValues(fields, doSuccessGrabarVigencia, doError, url);
        return;
    }

}
function doSuccessGrabarVigencia(result) {
    var Vigencia = result.Object;
    document.getElementById("fecIni").value = Vigencia.fechaInicial;
    document.getElementById("fecFin").value = Vigencia.FechaFinal;
    aviso("Aviso", result.Message);


    //llamada para el llenado de los periodos
    var url = $("#urlPeriodo").val();
    var moneda = $("#cmbxTipoMoneda").val();
    var dep = $("#cmbxDepartamentos").val();
    var res = moneda.split("#");
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
        departamento: dep
    };
    sendValues(fields, doSuccessPeriodos, doError, url);
    Limpiar();
}
function Buscar() {
    var strFec = $("#fecIni").val();
    if (strFec == "") {
        aviso("Aviso", "Debe Ingresar la Fecha de Inicio de Vigencia");
    }
    var url = $("#urlBuscar").val();
    var id = $("#cmbxTipoMoneda").val();
    var dep = $("#cmbxDepartamentos").val();
    var res = id.split("#");
    var fields = {
        fechaIni: strFec,
        codMoneda: res[0],
        reajuste: res[1],
        departamento: dep
    };
    sendValues(fields, doSuccessBuscarVigencia, doError, url);
}
function Grabar() {
    var strFec = $("#fecIni").val();
    if (strFec == "") {
        aviso("Aviso", "Debe ingresar Fecha de Inicio de Vigencia");
        return;
    }
    //validadcion TIR
    var Minimo_Tir = $("#txtMinTIR").val()
    var Minimo_TirInmediatas = $("#txtMinTIRInmediatas").val()
    var valorMaximoTIR = $("#vgMelerTirMax").val();
    if (Minimo_Tir.trim() == "") {
        aviso("Aviso", "Debe ingresar valor de la TIR Minima");
        return;
    }
    if (parseFloat(Minimo_Tir) < 0 || parseFloat(Minimo_Tir) > 100) {
        aviso("Aviso", "El Valor del Porcentaje debe ser entre 0 y 100 de la TIR Minima");
        return;
    }
    if (parseFloat(Minimo_Tir) > valorMaximoTIR) {
        aviso("Aviso", "La TIR Máxima debe ser mayor que la TIR Mínima");
        return;
    }
    //validacion Perdida
    var valorMinimoPER = $("#vgMelerPerConMin").val();
    var Maximo_Per = $("#txtMaxPer").val();
    if (Maximo_Per.trim() == "") {
        aviso("Aviso", "Debe ingresar valor de la Perdida Contable  Máxima");
        return;
    }
    if (parseFloat(Maximo_Per) < 0 || parseFloat(Maximo_Per) > 100) {
        aviso("Aviso", "El Valor del Porcentaje debe ser entre 0 y 100 de la Perdida Contable  Máxima");
        return;
    }
    if (valorMinimoPER > Maximo_Per) {
        aviso("Aviso", "La Perdida Máxima debe ser mayor que la Perdida Mínima ");
        return;
    }
    //validacion comision
    var Minimo_Com = $("#txtMinCom").val();
    var Maximo_Com = $("#txtMaxCom").val();
    if (Minimo_Com.trim() == "") {
        aviso("Aviso", "Debe ingresar valor de la Comisión Mínima");
        return;
    }

    if (Maximo_Com.trim() == "") {
        aviso("Aviso", "Debe ingresar valor de la Comisión Máxima");
        return;
    }
    if (parseFloat(Minimo_Com) < 0 || parseFloat(Minimo_Com) > 100) {
        aviso("Aviso", "El Valor del Porcentaje debe ser entre 0 y 100 de la Comisión Mínima");
        return;
    }
    if (parseFloat(Maximo_Com) < 0 || parseFloat(Maximo_Com) > 100) {
        aviso("Aviso", "El Valor del Porcentaje debe ser entre 0 y 100 de la Comisión Máxima");
        return;
    }


    if (parseFloat(Minimo_Com) > parseFloat(Maximo_Com)) {
        aviso("Aviso", "La Comisión Máxima debe ser mayor que la Comisión Mínima");
        return;
    }
    //Debe Ingresar Rangos de Tasa de Venta para Cada Tipo de Pension
    var tabla = document.getElementById("tblTasaVenta");
    var posicion = tabla.getElementsByClassName("posicion");
    if (posicion.length < 5) {
        aviso("Aviso", "Debe Ingresar Rangos de Tasa de Venta para Cada Tipo de Pension");
        return;
    }


    var id = $("#cmbxTipoMoneda").val();
    var dep = $("#cmbxDepartamentos").val();
    var url = $("#urlBuscar").val();
    var res = id.split("#");
    var fields = {
        fechaIni: strFec,
        codMoneda: res[0],
        reajuste: res[1],
        departamento: dep
    };
    sendValues(fields, doSuccessVerificarVigencia, doError, url);

}
function doSuccessVerificarVigencia(result) {
    if (result.Message == "No se encontro información") {
        // confirmarCotizacionMejorada("Aviso", "¿ Está seguro que desea grabar los datos ?", 3, result);
        confirmarCotizacionMejorada("Aviso", "¿ Está seguro que desea grabar los datos ?", function () { NuevaCotizaccionMejorada(3, result); });
        return;
    }
    if (result.Message == "Información cargada con éxito") {
        confirmarCotizacionMejorada("Aviso", "¿ Está seguro que desea Modificar los Datos ?", function () { NuevaCotizaccionMejorada(4, result); });
        // confirmarCotizacionMejorada("Aviso", "¿ Está seguro que desea Modificar los Datos ?", 4, result);
        return;
    }
}
function confirmarCotizacionMejorada(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}
/*
function confirmarCotizacionMejorada(header, body, bandera, result) {
    $('#msg_modal_header_2').text(header);
    $('#msg_modal_body_2').text(body);
    $('#sch_modal_Anclaje').modal('show');

    $("#btn_modal_aceptar_2").one('click', function () {
        $('#sch_modal_Anclaje').modal('hide');
        if (bandera == 1) {//borrado de tabla de TV
            $("#" + FilaSelecionada).remove();
            $("#txtTV").val("")
            FilaSelecionada = "";
        }
        if (bandera == 2)//consulta
            NuevaCotizaccionMejorada(bandera, result);
        if (bandera == 3)//grabar
            NuevaCotizaccionMejorada(bandera, result);
        if (bandera == 4)//Modificar
            NuevaCotizaccionMejorada(bandera, result);
        if (bandera == 5)//Modificar
            EliminarCotizaccionMejorada();
    });
}*/
function Eliminar() {
    var strFec = $("#CmbxPeriodos").val();
    if (strFec == "") {
        aviso("Aviso", "Debe seleccionar un Periodo de la lista para eliminar");
        return;
    }
    var id = $("#cmbxTipoMoneda").val();
    var dep = $("#cmbxDepartamentos").val();
    var url = $("#urlBuscar").val();
    var res = id.split("#");
    var fields = {
        fechaIni: strFec,
        codMoneda: res[0],
        reajuste: res[1],
        departamento: dep
    };
    sendValues(fields, doSuccessVerificarVigenciaEliminar, doError, url);
}
function doSuccessVerificarVigenciaEliminar(result) {
    if (result.Message == "Información cargada con éxito") {
        // confirmarCotizacionMejorada("Aviso", "¿ Está seguro que desea Eliminar la Información ? ", 5, result);
        confirmarCotizacionMejorada("Aviso", "¿ Está seguro que desea Eliminar la Información ?", function () { EliminarCotizaccionMejorada(); });
    } else {
        aviso("Aviso", "El Período que está intentando eliminar no se encuentra en la BD");
    }
}
function EliminarCotizaccionMejorada() {
    var strFec = $("#CmbxPeriodos").val();
    var id = $("#cmbxTipoMoneda").val();
    var dep = $("#cmbxDepartamentos").val();
    var url = $("#urlEliminar").val();
    var res = id.split("#");
    var fields = {
        fechaIni: strFec,
        codMoneda: res[0],
        reajuste: res[1],
        departamento: dep
    };
    sendValues(fields, doSuccessEliminar, doError, url);
}
function doSuccessEliminar(result) {

    aviso("Aviso", result.Message);
    Limpiar();
    //llamada para el llenado de los periodos
    var url = $("#urlPeriodo").val();
    var moneda = $("#cmbxTipoMoneda").val();
    var dep = $("#cmbxDepartamentos").val();
    var res = moneda.split("#");
    var fields = {
        vlMoneda: res[0],
        vlReajuste: res[1],
        departamento: dep
    };
    sendValues(fields, doSuccessPeriodos, doError, url);
}
function Salir() {
    var url = $("#urlSalir").val();
    window.location.href = url;
}
function Imprimir() {
    var periodo = $("#CmbxPeriodos").val();
    if (periodo == "") {
        aviso("Aviso", "Seleccione un perido de los disponibles");

    } else {
        var url = $("#urlReporte").val();
        var moneda = $("#cmbxTipoMoneda").val();
        var dep = $("#cmbxDepartamentos").val();
        var res = moneda.split("#");
        var tex = $("#cmbxTipoMoneda option:selected").text();
        var texdep = $("#cmbxDepartamentos option:selected").text();
        var fecha = periodo.split("/");
        window.open(url + "?codMoneda=" + res[0] + "&reajuste=" + res[1] + "&moneda=" + tex + "&departamento=" + dep + "&nomDepartamento=" + texdep + "&fechaInicial=" + fecha[2] + "-" + fecha[1] + "-" + fecha[0]);
    }

}

function Limpiar() {
    document.getElementById("fecIni").value = $("#backFecha").val();
    document.getElementById("fecFin").value = $("#backFecha").val();
    document.getElementById("fecIni").disabled = false;
    document.getElementById("cmbxTipoMoneda").disabled = false;
    document.getElementById("cmbxDepartamentos").disabled = false;
    $("#CmbxPeriodos").val("");

    document.getElementById("btnBuscar").style.display = "";
    document.getElementById("btnBuscar2").style.display = "none";
    $("#tabulador li").addClass("disabled");

    //document.getElementById("tabulador").style.display = "";
    //document.getElementById("tabulador2").style.display = "none";
    document.getElementById("tabulador3").style.display = "none";

    //TIR document.getElementById("txtMinTIR").disabled = true;
    document.getElementById("txtMinTIR").value = "";
    document.getElementById("txtMinTIRInmediatas").value = "";
    //Perdida
    document.getElementById("txtMaxPer").value = "";
    //Comision
    document.getElementById("txtMinCom").value = "";
    document.getElementById("txtMaxCom").value = "";
    document.getElementById("txtComInm").value = "";
    document.getElementById("txtComDif").value = "";
    //limpia la tabla de TV
    document.getElementById("txtTV").value = "";
    var $body = $("#tblTasaVenta tbody");
    $body.empty();



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

function cambiarFile() {
    var input = document.getElementById('file-input');
    if (input.files && input.files[0]) {
        var nombre = input.files[0].name;
        if (esEXCEL(nombre) == true) {
            $('#txtArchivoCargado').val(nombre);
            $('#file-submit').prop("disabled", false);
        }
        else {
            aviso("ERROR", "El archivo no es extencion .xlxs o .xls");
            $('#txtArchivoCargado').val("");
        }
    } else {
        $('#txtArchivoCargado').val("");
    }
}

function esEXCEL(doc) {
    var array = doc.split(".");
    var res = false;
    if (array[array.length - 1] == "xlsx" || array[array.length - 1] == "XLSX" || array[array.length - 1] == "XLS" || array[array.length - 1] == "xls") {
        res = true;
    }
    return res;
}