
(function () {
    localStorage.pagina = 'MantenedorIPC';
})();

$(document).ready(function () {
    $('#tblIPC tbody').on('click', 'tr', function () {
        var table = $('#tblIPC').DataTable();
        if ($(this).hasClass('selected')) {
            table.$('tr.selected').removeClass('selected');
            $(this).removeClass('selected');
            cargarInformacion(0, 0,0,0, false);
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    var funciones = document.getElementById('tblIPC_wrapper');
    var todasLasFunciones = funciones.getElementsByClassName('col-sm-6');
    Array.from(todasLasFunciones).forEach(e => {
        e.removeAttribute('class');
        e.setAttribute('class', 'col-md-12');
    }, this);
});
(function () {
    initTable();
    var url = $("#urlCargarTabla").val();
    var fields = {};
    sendValues(fields, doSuccessConsulta, doError, url);

})();
function initTable() {
    $("#tblIPC").DataTable({
        //"scrollY": "370px",
        "scrollCollapse": true,
        "lengthMenu": [[10, 20, 50, 75, 100], [10, 20, 50, 75, 100]],
        "language": {
            "sEmptyTable": "No se encontró ningún dato disponible en esta tabla",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ","
        },
        "bDestroy": true
    });

    var funciones = document.getElementById('tblIPC_wrapper');
    var todasLasFunciones = funciones.getElementsByClassName('col-sm-6');
    Array.from(todasLasFunciones).forEach(e => {
        e.removeAttribute('class');
        e.setAttribute('class', 'col-md-12');
    }, this);
}
function doError(result) {
    aviso("Aviso", result.Message);
}
function doSuccessConsulta(result) {
    if (result.Message == "Información cargada con éxito") {
        $("#tblIPC").DataTable().destroy();
        var $body = $("#tblIPC tbody");
        $body.empty();
        var i = 1;
        result.Object.forEach(function (v) {
            //var nFilas = $("#tblConsultaCotizaciones tr").length; <td><span style='display: none;'>20150221</span>21/02/2015</td>
            d = "cargarInformacion('" + v.FechaIpc + "','" + v.MontoIpc + "','" + v.VariacionIpc + "','" + v.Codigo + "','true')";
            var tr = $('<tr onclick="' + d + '">');
            var td = $('<td>');
            $("<td>").html(i).appendTo(tr);
            $("<td>").html(v.FechaIpc).appendTo(tr);
            $("<td>").html(v.MontoIpc).appendTo(tr);
            $("<td>").html(v.VariacionIpc).appendTo(tr);
            $body.append(tr)
            i++;
        });
        initTable();
        var funciones = document.getElementById('tblIPC_wrapper');
        var todasLasFunciones = funciones.getElementsByClassName('col-sm-6');
        Array.from(todasLasFunciones).forEach(e => {
            e.removeAttribute('class');
            e.setAttribute('class', 'col-md-12');
        }, this);
    } else {
        aviso("Aviso", result.Message);
    }
}
function cargarInformacion(a, b,c,d, bandera) {
    if (bandera) {
        var res = a.split("-");
        document.getElementById("txtMesIPC").disabled = true;
        document.getElementById("txtAnoIPC").disabled = true;
        document.getElementById("txtMesIPC").value = res[1];
        document.getElementById("txtAnoIPC").value = res[2];
        document.getElementById("txtValorIPC").value = b;
        document.getElementById("txtValorTMM").value = c;
        document.getElementById("txtValorTMM").focus();
        document.getElementById("txtCodigo").value = d;
    } else {
        document.getElementById("txtMesIPC").disabled = false;
        document.getElementById("txtAnoIPC").disabled = false;
        document.getElementById("txtMesIPC").value = "";
        document.getElementById("txtAnoIPC").value = "";
        document.getElementById("txtValorIPC").value = "";
        document.getElementById("txtValorTMM").value = "";
        document.getElementById("txtMesIPC").focus();
        document.getElementById("txtCodigo").value = "";
    }
}
function Grabar() {
    var mes = $("#txtMesIPC").val();
    if (mes == "") {
        aviso("Aviso", "Debe Ingresar Mes del Periodo de IPC.");
        return;
    }
    if (mes <=0 ||mes>12) {
        aviso("Aviso", "El Mes Ingresado no es un Valor Válido.");
        return;
    }
    var year = $("#txtAnoIPC").val();
    if (year == "") {
        aviso("Aviso", "Debe Ingresar Año del Periodo de IPC.");
        return;
    }
    var fechaActual = new Date();
    if (year < 1900 || year > fechaActual.getFullYear()) {
        aviso("Aviso", "Debe Ingresar un Año Mayor a 1900 o Menor Igual al Actual.");
        return;
    }
    var valorIPC = $("#txtValorIPC").val();
    if (valorIPC == "") {
        aviso("Aviso", "Debe ingresar un valor para Monto IPC.");
        return;
    }
    if (parseInt(valorIPC) <= 0) {
        aviso("Aviso", "Debe Ingresar un Valor Mayor que Cero para Monto IPC.");
        return;
    }                          
    if (parseFloat(valorIPC) > 999999999.99) {
        aviso("Aviso", "El Valor Ingresado en Monto IPC Excede el Máximo Permitido de 999999999.99 ");
        return;
    }
    var variacionIPC = $("#txtValorTMM").val();
    if (variacionIPC == "") {
        aviso("Aviso", "Debe ingresar un valor para Monto Variación IPC");
        return;
    }
    if (parseFloat(variacionIPC) < -999.99 || parseFloat(variacionIPC)>999.99) {
        aviso("Aviso", "La Variación IPC se encuentra fuera del Rango Permitido de -999,99 a 999,99 .");
        return;
    }
    //realiza la consulta para verificar si se va a modificar o atualizar
    var url = $("#urlConsulta").val();
    var fields = {
        fecha: year + "-" + mes
    };
    sendValues(fields, doSuccessConsultaGrabar, doError, url);
}
function doSuccessConsultaGrabar(result) {
    if (result.Message == "No se encontro información") {
        confirmarMantenedorIPC("Aviso", "¿ Está seguro que desea Grabar los Datos ?", function () { NuevoMantenedorIPC(2); });
      //confirmarMantenedorIPC("Aviso", "¿ Está seguro que desea Grabar los Datos ?",  2);
        return;
    }
    if (result.Message == "Información cargada con éxito") {
        confirmarMantenedorIPC("Aviso", "¿Los Datos Ya Existen en la Base de Datos, Desea Modificarlos ?", function () { NuevoMantenedorIPC(3); });
      //confirmarMantenedorIPC("Aviso", "¿Los Datos Ya Existen en la Base de Datos, Desea Modificarlos ?", 3);
        return;
    }
}
function NuevoMantenedorIPC(bandera) {
    var mes = $("#txtMesIPC").val();
    var year = $("#txtAnoIPC").val();
    var valorIPC = $("#txtValorIPC").val();
    var variacionIPC = $("#txtValorTMM").val();
    var codigo = $("#txtCodigo").val(); 
    var url = $("#urlGrabar").val();
    if (codigo == "")
        codigo = "N"

    if (bandera == 2) {
        var fields = {
            fecha: year + "-" + mes,
            valorIPC: valorIPC,
            variacionIPC: variacionIPC,
            codigo: codigo,
            bandera:true
        };
        sendValues(fields, doSuccessGrabar, doError, url);
    }
    if (bandera == 3) {
        var fields = {
            fecha: year + "-" + mes,
            valorIPC: valorIPC,
            variacionIPC: variacionIPC,
            codigo: codigo,
            bandera: false
        };
        sendValues(fields, doSuccessGrabar, doError, url);
    }
}
function doSuccessGrabar(result) {
    //recarga la tabla
    var url = $("#urlCargarTabla").val();
    var fields = {};
    sendValues(fields, doSuccessConsulta, doError, url);

    aviso("Aviso", result.Message);
}
function Eliminar() {
    var mes = $("#txtMesIPC").val();
    if (mes == "") {
        aviso("Aviso", "Debe Ingresar Mes del Periodo de IPC.");
        return;
    }
    if (mes <= 0 || mes > 12) {
        aviso("Aviso", "El Mes Ingresado no es un Valor Válido.");
        return;
    }
    var year = $("#txtAnoIPC").val();
    if (year == "") {
        aviso("Aviso", "Debe Ingresar Año del Periodo de IPC.");
        return;
    }
    var fechaActual = new Date();
    if (year < 1900 || year > fechaActual.getFullYear()) {
        aviso("Aviso", "Debe Ingresar un Año Mayor a 1900 o Menor Igual al Actual.");
        return;
    }
    //realiza la consulta para verificar si se va a modificar o atualizar
    var url = $("#urlConsulta").val();
    var fields = {
        fecha: year + "-" + mes
    };
    sendValues(fields, doSuccessConsultaEliminar, doError, url);


}
function doSuccessConsultaEliminar(result) {
    if (result.Message == "No se encontro información") {
        aviso("Aviso", "El Registro No Existe en la Base de Datos.");
        return;
    }
    if (result.Message == "Información cargada con éxito") {
        confirmarMantenedorIPC("Aviso", "¿Desea Eliminar el Registro Seleccionado?", function () { EliminarMantenedorIPC(); });
     // confirmarMantenedorIPC("Aviso", "¿Desea Eliminar el Registro Seleccionado?", 4);
        return;
    }
}
function EliminarMantenedorIPC() {
    var url = $("#urlEliminar").val();
    var mes = $("#txtMesIPC").val();
    var year = $("#txtAnoIPC").val();
    var fields = {
        fecha: year + "-" + mes
    };
    sendValues(fields, doSuccessEliminar, doError, url);

}
function doSuccessEliminar(result) {

    //recarga la tabla
    var url = $("#urlCargarTabla").val();
    var fields = {};
    sendValues(fields, doSuccessConsulta, doError, url);
    Limpiar();
    aviso("Aviso", result.Message);
}
function Limpiar() {
    document.getElementById("txtMesIPC").disabled = false;
    document.getElementById("txtAnoIPC").disabled = false;
    document.getElementById("txtMesIPC").value = "";
    document.getElementById("txtAnoIPC").value = "";
    document.getElementById("txtValorIPC").value = "";
    document.getElementById("txtValorTMM").value = "";
    document.getElementById("txtMesIPC").focus();
    document.getElementById("txtCodigo").value = "";
    var table = $('#tblIPC').DataTable();
    table.$('tr.selected').removeClass('selected');
    var funciones = document.getElementById('tblIPC_wrapper');
    var todasLasFunciones = funciones.getElementsByClassName('col-sm-6');
    Array.from(todasLasFunciones).forEach(e => {
        e.removeAttribute('class');
        e.setAttribute('class', 'col-md-12');
    }, this);
}

function Salir() {
    var url = $("#urlSalir").val();
    window.location.href = url;
}
function Imprimir() {
    var url = $("#urlReporte").val();
    window.open(url);
}

function confirmarMantenedorIPC(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}
/*
function confirmarMantenedorIPC(header, body, bandera) {
    $('#msg_modal_header_2').text(header);
    $('#msg_modal_body_2').text(body);
    $('#sch_modal_Anclaje').modal('show');

    $("#btn_modal_aceptar_2").one('click', function () {
        $('#sch_modal_Anclaje').modal('hide');
        if (bandera == 2)//genera el nuevo registro
            NuevoMantenedorIPC(bandera);
        if (bandera == 3)//modifica el registro
            NuevoMantenedorIPC(bandera);
        if (bandera == 4)//Elimina el registro
            EliminarMantenedorIPC();
    });
}*/
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
function soloNumeros(e) {
    var key = window.event ? e.which : e.keyCode;
    if (key < 48 || key > 57) {
        e.preventDefault();
    }
}
var negativo = true;

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