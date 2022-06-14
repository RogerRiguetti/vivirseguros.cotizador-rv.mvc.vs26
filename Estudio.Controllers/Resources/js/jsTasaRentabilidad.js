var valMon = "";
var bandManual = false;
var tablaTasaRentabilidad;
$(document).ready(function () {

    $('#tblConsultaDescuentoAnual').DataTable({
        paging: true,
        pageLength: 20,
        scrollY: "400px",
        scrollCollapse: true,
        language: {
            search: '',
            searchPlaceholder: "Buscar"
        },
        dom: '<lf<t>ip>'
    });
    
    document.getElementById("fecIni").value = "";
    document.getElementById("fecFin").value = "";
    document.getElementById("fecIniRes").value = "";
    document.getElementById("fecFinRes").value = "";

    $("#txtDesde").on('paste', function (e) {
        e.preventDefault();
    });
    $("#txtHasta").on('paste', function (e) {
        e.preventDefault();
    });

    $("#txtTasa").on('paste', function (e) {
        e.preventDefault();
    });

    $('#txtTipoMon').val("");

    $("#cmbxTipoMoneda").change(function () {
        var url = $("#urlConsultaPeriodos").val();
        var tipoMoneda = $("#cmbxTipoMoneda").val();
        $('#fecIni').attr("disabled", false);
        $('#fecIni').val("");
        //$("#txtTipoMon").val($('select[name="cmbxTipoMoneda"] option:selected').text());
        if (tipoMoneda != "") {
            var fields = {
                tipoMoneda: tipoMoneda
            };

            sendValues(fields, doSuccessConsultaPeriodos, doError, url);
        }
    });

    $("#cmbxPeriodos").change(function () {
        if ($("#cmbxPeriodos").val() == "") {
            $('#fecIni').attr("disabled", false);
        } else {
            $('#fecIni').attr("disabled", true);
            $('#fecIni').val("");
        }
        /*var fechaIni = $("#cmbxPeriodos").val();
        if (fechaIni!="") {
            $('#txtDesde').attr("disabled", false);
            $('#txtHasta').attr("disabled", false);
            $('#txtTasa').attr("disabled", false);
            $('#fecIni').attr("disabled", true);

            var url = $("#urlConsultaRentabilidad").val();
            var tipoMoneda = $("#cmbxTipoMoneda").val();
            var cmbxPeriodos = $("#cmbxPeriodos option:selected").text();
            var periodoVigencia = cmbxPeriodos.split('*');
            var fechaInicio = periodoVigencia[0].trim().split('/');
            var fechaFin = periodoVigencia[1].split('/');
            $('#btnBuscar').attr("disabled", true);
            $('#cmbxTipoMoneda').attr("disabled", true);
            $('#btnActualizar').attr("disabled", false);

            $("#fecIni").val(fechaInicio[2] + "-" + fechaInicio[1] + "-" + fechaInicio[0]);
            $("#fecFin").val(fechaFin[2] + "-" + fechaFin[1] + "-" +
            fechaFin[0].trim());

            if (tipoMoneda != "" && fechaInicio !="") {
                var fields = {
                    tipoMoneda: tipoMoneda,
                    fechaIni: fechaIni
                };

                sendValues(fields, doSuccessConsultaRentabilidad, doError, url);
            }
        }*/
    });

    $("#btnBuscar").click(function () {
        $('#txtDesde').attr("disabled", true);
        $('#txtHasta').attr("disabled", true);
        $('#txtTasa').attr("disabled", true);
        $('#txtTipoMon').val("");
        $('#txtDesde').val("")
        $('#txtHasta').val("")
        $('#txtTasa').val("")
        $("#fecIniRes").val("");
        $("#fecFinRes").val("");
        var url = $("#urlConsultaRentabilidad").val();
        var periodo = $("#cmbxPeriodos").val();
        valMon = $("#cmbxTipoMoneda").val();
        var fechaIni = $("#fecIni").val();
        if (valMon == "") {
            aviso("Aviso", "Debe Ingresar el Tipo de Moneda");
            return;
        }
        if (periodo == "" && fechaIni == "" && valMon != "") {
            aviso("Aviso", "Debe Seleccionar un Periodo o Ingresar la Fecha de Inicio de vigencia");
            return;
        }
        if (periodo != "" && valMon != "") {
            var periodoVigencia = periodo.split('*');
            var fechaInicio = periodoVigencia[0].trim().split('/');
            var fechaFin = periodoVigencia[1].trim().split('/');

            $("#fecIniRes").val(fechaInicio[2] + "-" + fechaInicio[1] + "-" + fechaInicio[0]);
            $("#fecFinRes").val(fechaFin[2] + "-" + fechaFin[1] + "-" + fechaFin[0]);
            $("#txtTipoMon").val($('select[name="cmbxTipoMoneda"] option:selected').text());
            valTipoMoneda = $("#cmbxTipoMoneda").val();

            var fields = {
                tipoMoneda: valMon,
                fechaIni: $("#fecIniRes").val()
            }

            sendValues(fields, doSuccessConsultaRentabilidad, doError, url);

        }

        if (fechaIni != "" && valMon != "") {
            var fechaInicio = $("#fecIni").val();
            $("#fecIniRes").val($("#fecIni").val());
            $("#txtTipoMon").val($('select[name="cmbxTipoMoneda"] option:selected').text());
            valTipoMoneda = $("#cmbxTipoMoneda").val();
            bandManual = true;
            var fields = {
                tipoMoneda: valMon,
                fechaIni: $("#fecIniRes").val()
            }

            sendValues(fields, doSuccessConsultaRentabilidad, doError, url);
        }

    });
    $("#btnSalir").click(function () {
        var url = $("#urlSalir").val();
        window.location.href = url;
    });
    $("#btnImprimir").click(function () {
        var url = $("#urlReporte").val();


        valMon = $("#cmbxTipoMoneda").val();
        var periodo = $("#cmbxPeriodos").val();

        if ($('#txtTipoMon').val() == "") {
            aviso("Aviso", "No se ha realizado ninguna búsqueda ");
            return;
        }

        if (periodo == "") {
            aviso("Aviso", "Debe seleccionar un Rango de Vigencia");
            return;
        }
        var tex = $("#cmbxTipoMoneda option:selected").text();
        var res = valMon.split("#");
        periodo = $("#fecIniRes").val();
        window.open(url + "?codMoneda=" + res[0] + "&reajuste=" + res[1] + "&moneda=" + tex + "&fechaInicial=" + periodo);
    });

});

(function () {
    localStorage.pagina = 'TasaRentablidad';
    cargaTabla();//carga la tabla con los 112 registros de default
})();

function doSuccessConsultaPeriodos(result) {
    var cmbxPeriodos = document.getElementById("cmbxPeriodos");
    $("#cmbxPeriodos").empty();
    var cont = 1;

    cmbxPeriodos.options[0] = new Option("-- Seleccione --", "");

    result.Object.forEach(function (b) {
        cmbxPeriodos.options[cont] = new Option(b.Periodo);
        cont++;
    });
}

function doError(result) {
    aviso("Aviso", result.Message);
}

function doSuccessConsultaRentabilidad(result) {
    tablaTasaRentabilidad = result.Object.tasasRent;
    if (result.Object.fechaTermino == "") {
        confirmarTasaRentabilidad("Aviso", "La Fecha Indicada no se encuentra registrada ¿ Desea Ingresar esta Nueva Fecha de Vigencia ?", function () { NuevaTasaRentabilidad(1); });
    } else {
        var fechaTermino = result.Object.fechaTermino;
        var tasaRent = result.Object.tasasRent
        //carga la info
        if (bandManual == true) {
            document.getElementById("fecFin").value = fechaTermino;
            document.getElementById("fecFinRes").value = fechaTermino;
            bandManual = false;
        }
        $('#txtDesde').attr("disabled", false);
        $('#txtHasta').attr("disabled", false);
        $('#txtTasa').attr("disabled", false);
        $("#tblConsultaDescuentoAnual").DataTable().destroy();
        var $body = $("#tblConsultaDescuentoAnual tbody");
        $body.empty();
        tasaRent.forEach(function (v) {
            d = "javascript:fn_CargarTasaRen('" + v.NUM_ANNO + "','true'" +",'" + v.PRC_TASAREN + "');";
            var fila = '<tr id="' + v.NUM_ANNO + '" onclick="' + d + '">  <td class="posicion" >' + v.NUM_ANNO + '</td>'
                                                                                     + ' <td class="valor">' + parseFloat(v.PRC_TASAREN).toFixed(2) + '</td>'
                                                                                     + '</tr>';
            $body.append(fila);
        });
        $('#tblConsultaDescuentoAnual').DataTable({
            paging: true,
            pageLength: 20,
            scrollY: "400px",
            scrollCollapse: true,
            language: {
                search: '',
                searchPlaceholder: "Buscar"
            },
            dom: '<lf<t>ip>'
        });

        //var funciones = document.getElementById('tblConsultaDescuentoAnual_wrapper');
        //var todasLasFunciones = funciones.getElementsByClassName('col-sm-6');
        //Array.from(todasLasFunciones).forEach(e => {
        //    e.removeAttribute('class');
        //    e.setAttribute('class', 'col-md-12');
        //}, this);
    }

    //$('#tblConsultaDescuentoAnual').DataTable({
    //    paging: true,
    //    pageLength: 20,
    //    scrollY: "400px",
    //    scrollCollapse: true,
    //    language: {
    //        search: '',
    //        searchPlaceholder: "Buscar"
    //    },
    //    dom: '<lf<t>ip>'

    //});

}

function cargaTabla() {
    $("#tblConsultaDescuentoAnual").DataTable().destroy();
    var $body = $("#tblConsultaDescuentoAnual tbody");
    $body.empty();
    for (var i = 1; i <= 112; i++) {
        d = "javascript:fn_CargarTasaRen('" + i + "','true'" +",'" + '0.00' +"');";

        var fila = '<tr id="' + i + '" onclick="' + d + '">  <td class="posicion" >' + i + '</td>'
                                                                                 + ' <td class="valor">' + parseFloat(0).toFixed(2) + '</td>'
                                                                                 + '</tr>';
        $body.append(fila);
    }
}

function fn_CargarTasaRen(numAnno, bandera, prc_tasaren) {
    if (numAnno == 0) {
        $("#txtDesde").val("");
        $("#txtHasta").val("");
        $("#txtTasa").val("");
        $('#txtDesde').attr("disabled", false);
        $('#txtHasta').attr("disabled", false);
        $('#txtTasa').attr("disabled", false);
        $("#tblConsultaDescuentoAnual tr").click(function () {
            $("#tblConsultaDescuentoAnual tr").removeClass('focus');
            $(this).addClass('');
        });
        return;
    }
    if (bandera) {
        $("#txtDesde").val(numAnno);
        $("#txtHasta").val(numAnno);
        var tabla = document.getElementById("tblConsultaDescuentoAnual");
        //$("#txtTasa").val(tabla.rows[numAnno].cells[1].innerText);
        $("#txtTasa").val(prc_tasaren);
        $('#txtDesde').attr("disabled", true);
        $('#txtHasta').attr("disabled", true);
        $('#txtTasa').attr("disabled", false);

        $("#tblConsultaDescuentoAnual tr").click(function () {
            $("#tblConsultaDescuentoAnual tr").removeClass('focus');
            $(this).addClass('focus');
        });
        $("#tblConsultaDescuentoAnual tr").dblclick(function () {
            $("#tblConsultaDescuentoAnual tr").removeClass('focus');
            $(this).addClass('');
            fn_CargarTasaRen(numAnno, false, '0.00');
        });
    } else {

        $("#txtDesde").val("");
        $("#txtHasta").val("");
        $("#txtTasa").val("");
        $('#txtDesde').attr("disabled", false);
        $('#txtHasta').attr("disabled", false);
        $('#txtTasa').attr("disabled", false);
    }
}
function ActualizarTasa() {
    var desde = parseInt($("#txtDesde").val());
    var hasta = parseInt($("#txtHasta").val());
    var tasa = $("#txtTasa").val();
    var tabla = document.getElementById("tblConsultaDescuentoAnual");

    if (desde == "") {
        aviso("Aviso", "Debe ingresar valor Inicial del Rango a actualizar.");
        return;
    }
    if (hasta == "") {
        aviso("Aviso", "Debe ingresar valor Final del Rango a actualizar.");
        return;
    }

    if (desde < 1 || desde > 112) {
        aviso("Aviso", "El valor Inicial del Rango debe estar entre 1 y 112.");
        return;
    }

    if (hasta < 1 || hasta > 112) {
        aviso("Aviso", "El valor Final del Rango debe estar entre 1 y 112.");
        return;
    }

    if (desde > hasta) {
        aviso("Aviso", "El valor Final del Rango no debe ser mayor que el Valor Incial.");
        return;
    }//Debe ingresar valor de la Tasa a actualizar
    if (tasa == "") {
        aviso("Aviso", "Debe ingresar valor de la Tasa a actualizar.");
        return;
    }
    if (parseFloat(tasa) < 0 || parseFloat(tasa) > 100) {
        aviso("Aviso", "El valor de la Tasa debe estar entre 0 y 100");
        return;
    }

    if (desde == hasta) {
        for (var i = desde; i <= hasta; i++) {
            var indice = tablaTasaRentabilidad.findIndex(row => row.NUM_ANNO == i);
            tablaTasaRentabilidad[indice].PRC_TASAREN = parseFloat(tasa).toFixed(2);
        }
        $("#tblConsultaDescuentoAnual").DataTable().destroy();
        var $body = $("#tblConsultaDescuentoAnual tbody");
        $body.empty();
        tablaTasaRentabilidad.forEach(function (v) {
            d = "javascript:fn_CargarTasaRen('" + v.NUM_ANNO + "','true'" + ",'" + v.PRC_TASAREN + "');";
            var fila = '<tr id="' + v.NUM_ANNO + '" onclick="' + d + '">  <td class="posicion" >' + v.NUM_ANNO + '</td>'
                                                                                     + ' <td class="valor">' + parseFloat(v.PRC_TASAREN).toFixed(2) + '</td>'
                                                                                     + '</tr>';
            $body.append(fila);
        });
        $('#tblConsultaDescuentoAnual').DataTable({
            paging: true,
            pageLength: 20,
            scrollY: "400px",
            scrollCollapse: true,
            language: {
                search: '',
                searchPlaceholder: "Buscar"
            },
            dom: '<lf<t>ip>'
        });
        //tabla.rows[desde].cells[1].innerText = parseFloat(tasa).toFixed(2);
        //return;
    }
    if (hasta > desde) {
        
        for (var i = desde; i <= hasta; i++) {
            var indice = tablaTasaRentabilidad.findIndex(row => row.NUM_ANNO == i);
            tablaTasaRentabilidad[indice].PRC_TASAREN = parseFloat(tasa).toFixed(2);
            //if (renglon.cells[0].innerText == desde) {
            //        renglon.cells[1].innerText = parseFloat(tasa).toFixed(2);
            //        //return;
            //}
            //tabla.rows[i].cells[1].innerText = parseFloat(tasa).toFixed(2);
        }
        $("#tblConsultaDescuentoAnual").DataTable().destroy();
        var $body = $("#tblConsultaDescuentoAnual tbody");
        $body.empty();
        tablaTasaRentabilidad.forEach(function (v) {
            d = "javascript:fn_CargarTasaRen('" + v.NUM_ANNO + "','true'" + ",'" + v.PRC_TASAREN + "');";
            var fila = '<tr id="' + v.NUM_ANNO + '" onclick="' + d + '">  <td class="posicion" >' + v.NUM_ANNO + '</td>'
                                                                                     + ' <td class="valor">' + parseFloat(v.PRC_TASAREN).toFixed(2) + '</td>'
                                                                                     + '</tr>';
            $body.append(fila);
        });
        $('#tblConsultaDescuentoAnual').DataTable({
            paging: true,
            pageLength: 20,
            scrollY: "400px",
            scrollCollapse: true,
            language: {
                search: '',
                searchPlaceholder: "Buscar"
            },
            dom: '<lf<t>ip>'
        });
    }


}
function Limpiar() {
    $('#txtDesde').attr("disabled", true);
    $('#txtHasta').attr("disabled", true);
    $('#txtTasa').attr("disabled", true);
    cargaTabla();
    $('#tblConsultaDescuentoAnual').DataTable({
        paging: true,
        pageLength: 20,
        scrollY: "400px",
        scrollCollapse: true,
        language: {
            search: '',
            searchPlaceholder: "Buscar"
        },
        dom: '<lf<t>ip>'
    });
    $("#cmbxPeriodos").append('<option value="">-- Seleccione --</option>');
    $('#cmbxTipoMoneda').val("");
    $('#txtTipoMon').val("");
    $('#txtDesde').val("")
    $('#txtHasta').val("")
    $('#txtTasa').val("")
    document.getElementById("cmbxPeriodos").options.length = 0;
    $("#cmbxPeriodos").append('<option value="">-- Seleccione --</option>');
    $('#cmbxPeriodos').val("")
    document.getElementById("fecIni").value = "";
    document.getElementById("fecFin").value = "";
    document.getElementById("fecIniRes").value = "";
    document.getElementById("fecFinRes").value = "";
    $('#fecIni').attr("disabled", false);
    document.getElementById("btnBuscar").style.display = "";
}

function Grabar() {

    var url = $("#urlConsultaRentabilidad").val();
    var fechaIni = $("#fecIniRes").val();
    if ($('#txtTipoMon').val() == "") {
        aviso("Aviso", "No se ha realizado ninguna búsqueda ");
        return;
    }
    if (fechaIni == "") {
        aviso("Aviso", "Debe Ingresar un rango de vigencia a Guardar.");
        return;
    }


    var fields = {
        tipoMoneda: valMon,
        fechaIni: fechaIni
    };
    sendValues(fields, doSuccessConsultaGrabar, doError, url);

}
function doSuccessConsultaGrabar(res) {
    if (res.Object.fechaTermino == "") {
        confirmarTasaRentabilidad("Aviso", "¿ Está seguro que desea Grabar la Información ? ", function () { NuevaTasaRentabilidad(2); });
        return;
    } else {
        confirmarTasaRentabilidad("Aviso", "¿ Está seguro que desea Modificar la Información ?", function () { NuevaTasaRentabilidad(3); });
        return;
    }
}

function NuevaTasaRentabilidad(bandera) {
    if (bandera == 1) {
        $("#fecFinRes").val("9999-12-31");
        $("#fecFin").val("9999-12-31");
        $('#txtDesde').attr("disabled", false);
        $('#txtHasta').attr("disabled", false);
        $('#txtTasa').attr("disabled", false);
        cargaTabla();
        
        var tabla = $('#tblConsultaDescuentoAnual').DataTable({
            paging: true,
            pageLength: 20,
            scrollY: "400px",
            scrollCollapse: true,
            language: {
                search: '',
                searchPlaceholder: "Buscar"
            },
            dom: '<lf<t>ip>'
        });

        var data = tabla.rows().data();
        var infoTabla = [];
        //var posicion = data.row();
        //var valor = data.getElementsByClassName("valor");
        data.each(function (value, index) {
            var year = value[0];
            var tasa = value[1];
            var infoTablas = { NUM_ANNO: year, PRC_TASAREN: tasa }
            infoTabla.push(infoTablas);
        });
        tablaTasaRentabilidad = infoTabla;

        return;
    }
    var url = $("#urlGrabarTasaRen").val();
    var fechaIni = $("#fecIniRes").val();
    // var tabla = document.getElementById("tblConsultaDescuentoAnual");
    //cargaTabla();
    var tabla = $('#tblConsultaDescuentoAnual').DataTable();

    var data = tabla.rows().data();
    var infoTabla = [];
    //var posicion = data.row();
    //var valor = data.getElementsByClassName("valor");
    data.each(function (value, index) {
        var year = value[0];
        var tasa = value[1];
        var infoTablas = { NUM_ANNO: year, PRC_TASAREN: tasa }
        infoTabla.push(infoTablas);
    });

    
    //for (var i = 0; i < posicion.length; i++) {
    //    var year = posicion[i].innerHTML;
    //    var tasa = valor[i].innerHTML;
    //    var infoTablas = { NUM_ANNO: year, PRC_TASAREN: tasa }
    //    infoTabla.push(infoTablas);
    //}
    if (bandera == 2) {//guarda la informacion
        var fields = {
            informacion: infoTabla,
            tipoMoneda: valMon,
            fechaIni: fechaIni,
            clave: "INSERT"
        };
        sendValues(fields, doSuccessGrabar, doError, url);
        return;
    }
    if (bandera == 3) {//actualiza la información
        var fields = {
            informacion: infoTabla,
            tipoMoneda: valMon,
            fechaIni: fechaIni,
            clave: "UPDATE"
        };
        sendValues(fields, doSuccessGrabar, doError, url);
        return;
    }
}
function doSuccessGrabar(result) {
    aviso("Aviso", result.Message);
    Limpiar();
}
function Eliminar() {
    var url = $("#urlConsultaRentabilidad").val();
    var fechaIni = $("#fecIniRes").val();
    if ($('#txtTipoMon').val() == "") {
        aviso("Aviso", "No se ha realizado ninguna búsqueda ");
        return;
    }
    if (fechaIni == "") {
        aviso("Aviso", "Debe seleccionar un rango de vigencia para Eliminar.");
        return;
    }
    /*if (periodo == "") {
        aviso("Aviso", "Debe seleccionar un rango de vigencia para Eliminar.");
        return;
    }*/

    var fields = {
        tipoMoneda: valMon,
        fechaIni: fechaIni
    };
    sendValues(fields, doSuccessConsultaEliminar, doError, url);

}
function doSuccessConsultaEliminar(result) {
    if (result.Object == "") {
        aviso("Aviso", "El Período que está intentando eliminar no se encuentra en la BD");
        return;
    } else {
        confirmarTasaRentabilidad("Aviso", " ¿ Está seguro que desea Eliminar la Información ?", function () { EliminarTasaRentabilidad(); });
        return;
    }
}
function EliminarTasaRentabilidad() {
    var url = $("#urlEliminarTasaRen").val();
    var fechaIni = $("#fecIniRes").val();
    var fields = {
        tipoMoneda: valMon,
        fechaIni: fechaIni
    };
    sendValues(fields, doSuccessEliminar, doError, url);
}
function doSuccessEliminar(result) {
    aviso("Aviso", result.Message);
    Limpiar();
}




function confirmarTasaRentabilidad(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}
function filterFloat(evt, input, decimales) {
    // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;

    if (key >= 48 && key <= 57) {  //si son numeros
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

