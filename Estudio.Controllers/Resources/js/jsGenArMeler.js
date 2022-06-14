$(document).ready(function () {
    $('#tblRegistrosMeler').DataTable({
        language: { search: '', searchPlaceholder: "Buscar" }
    });

    $('#tblConsulta').DataTable({
        searching: false,
        bSort: false,
        paging: false,
        info: false
    });

    $("#txtFecha").change(function () {
        buscarNumArchivosCal();
    });
});

var Fec;
var arr = [];
var arrNoCotiza = [];
var resultado = "";
var numArchivoSeleccionado = "";
var fechaEnvioSeleccionada = "";
var metodoDeBusqueda = "";

(function () {
    localStorage.pagina = 'GenArMeler';
    var f = new Date();
    var dia = f.getDate()
    var mes = (f.getMonth() + 1);
    if (dia <= 9) { dia = "0" + dia }
    if (mes <= 9) { mes = "0" + mes; }
    var fecha = f.getFullYear() + "-" + mes + "-" + dia;
    document.getElementById('txtFecha').setAttribute("value", fecha)
    //Función para seleccionar todos los registros de la tabla.
    $('#select-all').click(function (event) {

        //Variables para contar los renglones de la tabla.
        var myTable = document.getElementById("tblConsulta");
        var rowCount = myTable.rows.length;

        if (this.checked) {
            //Marcar todos los CheckBox.
            checkboxes = document.getElementsByTagName('input');
            for (i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == "checkbox" && checkboxes[i].name == "checkbox") {
                    checkboxes[i].checked = true;
                }
            }
            for (var x = 0; x < rowCount - 1; x++) {
                arr.push(x);
            }
        } else {
            checkboxes = document.getElementsByTagName('input');
            for (i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == "checkbox" && checkboxes[i].name == "checkbox") {
                    checkboxes[i].checked = false;
                }
            }
            arr.length = 0;
        }
    });
    $('#select-all-NoCotiza').click(function (event) {

        //Variables para contar los renglones de la tabla.
        var myTable = document.getElementById("tblConsulta");
        var rowCount = myTable.rows.length;

        if (this.checked) {
            //Marcar todos los CheckBox.
            checkboxes = document.getElementsByTagName('input');
            for (i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == "checkbox" && checkboxes[i].name == "checkboxNoCotiza") {
                    checkboxes[i].checked = true;
                }
            }
            for (var x = 0; x < rowCount - 1; x++) {
                arrNoCotiza.push(x);
            }
        } else {
            checkboxes = document.getElementsByTagName('input');
            for (i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == "checkbox" && checkboxes[i].name == "checkboxNoCotiza") {
                    checkboxes[i].checked = false;
                }
            }
            arrNoCotiza.length = 0;
        }
    });
    $('#cmbArchivos').attr("disabled", true);
    $("#txtFecha").click(function () {
        $('#cmbArchivos').attr("disabled", true);
    });
    $("#cmbArchivos").change(function () {
        BuscarArchivo('', '');
    });
  

    $("#btnExportar").click(function () {
        exportar();
    });

})();

function borrarNumsArchivos() {
    var x = document.getElementById("cmbArchivos");
    for (var i = 1; i < x.length; i++) {
        x.remove(i);
    }
}
function buscarNumArchivos(e) {
    //See notes about 'which' and 'key'
    if (e.keyCode == 13) {
        arr.length = 0;
        arrNoCotiza.length = 0;
        $('#cmbArchivos').attr("disabled", false);
        borrarNumsArchivos();
        var temp, ano, mes, dia;
        temp = $('#txtFecha').val();
        dia = temp.substring(8, 10);
        mes = temp.substring(5, 7);
        ano = temp.substring(0, 4);
        Fec = ano + "" + mes + "" + dia;

        var urlBuscarArchivo = $("#urlBuscarNumerosArchivo").val();
        var fields = {
            fecha: Fec
        };
        sendValues(fields, doSuccessBuscarNumArchivos, doError, urlBuscarArchivo);
    }
}

function buscarNumArchivosCal() {
    //See notes about 'which' and 'key'
        arr.length = 0;
        arrNoCotiza.length = 0;
        $('#cmbArchivos').attr("disabled", false);
        borrarNumsArchivos();
        var temp, ano, mes, dia;
        temp = $('#txtFecha').val();
        dia = temp.substring(8, 10);
        mes = temp.substring(5, 7);
        ano = temp.substring(0, 4);
        Fec = ano + "" + mes + "" + dia;

        var urlBuscarArchivo = $("#urlBuscarNumerosArchivo").val();
        var fields = {
            fecha: Fec
        };
        sendValues(fields, doSuccessBuscarNumArchivosCal, doError, urlBuscarArchivo);
    
}


function doSuccessBuscarNumArchivos(result) {
    //cargar options <option value="0" selected>SELECCIONAR...</option>
    var array = result.Object;
    if (array.length > 0 && array[0].toString() != "-1") {
        for (var i = 0; i < array.length; i++) {
            var trOpcion = document.createElement('option');
            trOpcion.setAttribute("value", (array[i].split('-')[0]).toString()); 
            trOpcion.text = (array[i].toString());
            document.getElementById('cmbArchivos').appendChild(trOpcion);
        }
    } else {
        $('#cmbArchivos').attr("disabled", true);
        aviso("AVISO", "No se encontraron registros con esa fecha");
    }
}

function doSuccessBuscarNumArchivosCal(result) {
    //cargar options <option value="0" selected>SELECCIONAR...</option>
    var array = result.Object;
    if (array.length > 0 && array[0].toString() != "-1") {
        for (var i = 0; i < array.length; i++) {
            var trOpcion = document.createElement('option');
            trOpcion.setAttribute("value", (array[i].split('-')[0]).toString());
            trOpcion.text = (array[i].toString());
            document.getElementById('cmbArchivos').appendChild(trOpcion);
        }
    } 
}

function BuscarArchivo(fechaT, archT) {
    
    arr.length = 0;
    arrNoCotiza.length = 0;
    var myTable = document.getElementById("RConsultaMeler");
    var rowCount = myTable.rows.length;
    for (var x = rowCount - 1; x > -1; x--) {
        myTable.deleteRow(x);
    }
    var urlBuscarArchivo = $("#urlBuscarArchivo").val();
    var fields;

    if (fechaT == "" && archT == "") {
        document.getElementById("select-all").checked = true;
        metodoDeBusqueda = "combo";
        var numArchivo = $("#cmbArchivos").val();
        if (numArchivo != "-1") {
            var temp, ano, mes, dia;
            temp = $('#txtFecha').val();
            dia = temp.substring(8, 10);
            mes = temp.substring(5, 7);
            ano = temp.substring(0, 4);
            Fec = ano + "" + mes + "" + dia;
            numArchivoSeleccionado = numArchivo;
            fechaEnvioSeleccionada = Fec;
            fields = {
                fecha: Fec,
                numArchivo: numArchivo,
                caso: metodoDeBusqueda
            };
            sendValues(fields, doSuccessBuscar, doError, urlBuscarArchivo);
        }
    } else {
        document.getElementById("select-all").checked = false;
        metodoDeBusqueda = "tabla";
        numArchivoSeleccionado = archT;
        fechaEnvioSeleccionada = fechaT;
        fields = {
            fecha: fechaT,
            numArchivo: archT,
            caso: metodoDeBusqueda
        };
        fechaT = "";
        archT = "";
        sendValues(fields, doSuccessBuscar, doError, urlBuscarArchivo);
    }

}

function doSuccessBuscar(result) {
    cargaDetabla(result.Object);
    obtenerAlto("ventana", $(document).height());

    //var myTable = document.getElementById("tblConsulta");
    //var rowCount = myTable.rows.length;
    //var checkboxes = document.getElementsByTagName('input');
    //for (i = 0; i < checkboxes.length; i++) {
    //    if (checkboxes[i].type == "checkbox" && checkboxes[i].name == "checkbox") {
    //        checkboxes[i].checked = true;
    //    }
    //}
    //for (var x = 0; x < rowCount - 1; x++) {
    //    arr.push(x);
    //}
}

function doError(result) {
    $('#modalcargar').modal('hide');
    aviso("ERROR", result.Message);
}
function cargaDetabla(datos) {
    var nombCol = ["strNumOperacion", "strCorr", "strFecCal", "strAsegurado", "strMoneda", "strModalidad", "strPeriodo", "strRenta", "strGarantizado", "strEscalonada", "strPrima",
                   "strTemporal", "strVitalicia", "strTasa", "strTIR", "strPerdida", "strComisión", "strNomArchivo", "strTipoArchivo", "strFecCrea", "strHoraCrea", "strUsuario",
                   "strNumArchivo", "strNumCot", "strIndEstado", "indFiltroCotiza"];

    for (var i = 0; i < datos.length; i++) {
        var trDatos = document.createElement('tr');
        trDatos.setAttribute("class", "bordeBajo textoTablas");
        var bandE = false;
        var bandC = false;
        for (var j = 0; j < datos[i].length; j++) {
            var thDato = document.createElement('td');
            thDato.setAttribute("class", nombCol[j]);

            if (j > 16) { thDato.setAttribute("style", "display:none;"); }
            if (j == 24) {
                if (datos[i][j] != "I") { bandE = true; }
            }
            if (j == 25) {
                if (datos[i][j].trim() != "S") { bandC = true; }
            }
            thDato.append(datos[i][j]);
            trDatos.appendChild(thDato);
        }
        //check box de No Cotiza
        var radio2 = document.createElement('input');
        radio2.setAttribute("id", "checkboxNoCotiza" + (i));
        radio2.setAttribute("name", "checkboxNoCotiza");
        radio2.setAttribute("type", "checkbox");
        radio2.setAttribute("value", (i));
        if (bandC == true) {
            radio2.setAttribute("checked", true);
            arrNoCotiza.push(i);
            bandC = false;
        }
        var label2 = document.createElement('label');
        label2.setAttribute("name", "LcheckboxNoCotiza");
        label2.setAttribute("for", "checkboxNoCotiza" + (i));
        var thDato2 = document.createElement('td');
        thDato2.setAttribute("class", "radioBtn");
        var span2 = document.createElement('span');
        thDato2.setAttribute("style", "text-align: center;");
        thDato2.appendChild(radio2);
        label2.appendChild(span2);
        thDato2.appendChild(label2)
        trDatos.appendChild(thDato2);
        //check box para generar el xml
        var radio = document.createElement('input');
        radio.setAttribute("id", "check" + (i));
        radio.setAttribute("name", "checkbox");
        radio.setAttribute("type", "checkbox");
        radio.setAttribute("value", (i));
        if (bandE == true) {
            radio.setAttribute("checked", true);
            arr.push(i);
            bandE = false;
        }
        var label = document.createElement('label');
        label.setAttribute("name", "Lcheckbox");
        label.setAttribute("for", "check" + (i));
        var thDato = document.createElement('td');
        thDato.setAttribute("class", "radioBtn");
        var span = document.createElement('span');
        thDato.setAttribute("style", "text-align: center;");
        thDato.appendChild(radio);
        label.appendChild(span);
        thDato.appendChild(label)
        trDatos.appendChild(thDato);
        /////
        document.getElementById('RConsultaMeler').appendChild(trDatos);

    }
    $("input[name=checkboxNoCotiza]").click(function () {
        var checkValNoCotiza = this.value;
        var ban = true;
        if (this.checked) {
            if (arrNoCotiza.length > 0) {
                for (var i = 0; i < arrNoCotiza.length; i++) {
                    if (checkValNoCotiza == arrNoCotiza[i]) {
                        ban = false;
                        break;
                    } else {
                        ban = true;
                    }
                }
                if (ban == true) {
                    arrNoCotiza.push(checkValNoCotiza);
                }
            } else {
                arrNoCotiza.push(checkValNoCotiza);
            }
        } else {
            for (var i = 0; i < arrNoCotiza.length; i++) {
                if (checkValNoCotiza == arrNoCotiza[i]) {
                    arrNoCotiza.splice(i, 1);
                    break;
                }
            }
        }
    });
    $("input[name=checkbox]").click(function () {
        var checkVal = this.value;
        var ban = true;
        if (this.checked) {
            if (arr.length > 0) {
                for (var i = 0; i < arr.length; i++) {
                    if (checkVal == arr[i]) {
                        ban = false;
                        break;
                    } else {
                        ban = true;
                    }
                }
                if (ban == true) {
                    arr.push(checkVal);
                }
            } else {
                arr.push(checkVal);
            }
        } else {
            for (var i = 0; i < arr.length; i++) {
                if (checkVal == arr[i]) {
                    arr.splice(i, 1);
                    break;
                }
            }
        }
    });

}

function generar() {
    if (arr.length > 0) {
        var Info = [];
        var dato;
        var tabla = document.getElementById("tblConsulta");
        var strNumOperacion = tabla.getElementsByClassName("strNumOperacion");
        var strCorr = tabla.getElementsByClassName("strCorr");
        var strFecCal = tabla.getElementsByClassName("strFecCal");
        var strAsegurado = tabla.getElementsByClassName("strAsegurado");
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
        var strNomArchivo = tabla.getElementsByClassName("strNomArchivo");
        var strTipoArchivo = tabla.getElementsByClassName("strTipoArchivo");
        var strFecCrea = tabla.getElementsByClassName("strFecCrea");
        var strHoraCrea = tabla.getElementsByClassName("strHoraCrea");
        var strUsuario = tabla.getElementsByClassName("strUsuario");
        var strNumArchivo = tabla.getElementsByClassName("strNumArchivo");
        var strNumCot = tabla.getElementsByClassName("strNumCot");
        for (var i = 0; i < arr.length; i++) {

            dato = {
                numOperacion: strNumOperacion[arr[i]].innerHTML,
                Correlativo: strCorr[arr[i]].innerHTML,
                fec: strFecCal[arr[i]].innerHTML,
                asegurado: strAsegurado[arr[i]].innerHTML,
                moneda: strMoneda[arr[i]].innerHTML,
                modalidad: strModalidad[arr[i]].innerHTML,
                periodoDiferido: strPeriodo[arr[i]].innerHTML,
                rentaTMP: strRenta[arr[i]].innerHTML,
                perGarantizado: strGarantizado[arr[i]].innerHTML,
                rentaEsc: strEscalonada[arr[i]].innerHTML,
                primaUnica: strPrima[arr[i]].innerHTML,
                renTmp1T: strTemporal[arr[i]].innerHTML,
                penVit2T: strVitalicia[arr[i]].innerHTML,
                tasaVenta: strTasa[arr[i]].innerHTML,
                tir: strTIR[arr[i]].innerHTML,
                perdida: strPerdida[arr[i]].innerHTML,
                comision: strComisión[arr[i]].innerHTML,
                nomArchivo: strNomArchivo[arr[i]].innerHTML,
                tipoArchivo: strTipoArchivo[arr[i]].innerHTML,
                fechaCrea: strFecCrea[arr[i]].innerHTML,
                horaCrea: strHoraCrea[arr[i]].innerHTML,
                usuarioCrea: strUsuario[arr[i]].innerHTML,
                numArch: strNumArchivo[arr[i]].innerHTML,
                numCot: strNumCot[arr[i]].innerHTML
            }
            Info.push(dato);
        }
        if (arrNoCotiza.length > 0) {
            var infoNoCotiza = [];
            var datoNoCotiza;
            var tablaNoCotiza = document.getElementById("tblConsulta");
            var strNumOperacion = tablaNoCotiza.getElementsByClassName("strNumOperacion");
            var strCorr = tablaNoCotiza.getElementsByClassName("strCorr");
            var strNumArchivo = tablaNoCotiza.getElementsByClassName("strNumArchivo");
            for (var i = 0; i < arrNoCotiza.length; i++) {

                datoNoCotiza = {
                    numOperacion: strNumOperacion[arrNoCotiza[i]].innerHTML,
                    Correlativo: strCorr[arrNoCotiza[i]].innerHTML,
                    numArch: strNumArchivo[arrNoCotiza[i]].innerHTML
                }
                infoNoCotiza.push(datoNoCotiza);
            }
        }

        $('#modalcargar').modal({
            drop: 'static',
            keyboard: false,
            show: true,
            backdrop: 'static'
        });

        var url = $("#urlGenerarXML").val();

        var fields = {
            informacion: Info,
            NoCotiza: infoNoCotiza
        };
        sendValues(fields, doSuccessCalcular, doError, url);

    } else {
        aviso("ERROR", "Ningun dato seleccionado." + "\nOperación Cancelada.");
        return;
    }
}

function doSuccessCalcular(result) {
    $('#modalcargar').modal('hide');
    resultado = result.Message;
    var f = new Date();
    var dia = f.getDate()
    var mes = (f.getMonth() + 1);
    if (dia <= 9) { dia = "0" + dia }
    if (mes <= 9) { mes = "0" + mes; }
    var nombre = "cargaCot_" + (f.getFullYear() + "" + mes + "" + dia + "_ArchE" + numArchivoSeleccionado);
    var elem = document.getElementById('descargar');
    elem.download = nombre + ".xml";
    elem.href = "data:application/octet-stream,"
                         + encodeURIComponent(resultado);
    document.getElementById("descargar").click();
    resultado = "";
    elem.removeAttribute("download");
    elem.removeAttribute("href");

    //Actualizar la grilla
    var urlR = $("#urlRefrescarGrilla").val();
    sendValues("", doSuccessRefrescarGrilla, doError, urlR);
}

function exportar() {
    if (numArchivoSeleccionado != "") {
        var url = $("#urlCrearExcel").val();
        window.open(url + "?num_Archivo=" + numArchivoSeleccionado);
    } else {
        aviso("ERROR", "Se debe seleccionar un archivo.");
    }
}


function doSuccessRefrescarGrilla(result) {
    cargaDeGrilla(result.Object);
}

function cargaDeGrilla(datos) {
    var myTable = document.getElementById("RConsulArchivos");
    var rowCount = myTable.rows.length;
    for (var x = rowCount - 1; x > -1; x--) {
        myTable.deleteRow(x);
    }
    var nombCol = ["strNumArch", "strNomArch", "strFecEnvio", "strFecCierre", "strNrosOperacion", "strValue", "strFecMetodo", "strColo"];

    for (var i = 0; i < datos.length; i++) {
        var trDatos = document.createElement('tr');
        trDatos.setAttribute("id", datos[i][5]);
        trDatos.setAttribute("style", "cursor:pointer;");
        trDatos.setAttribute("onclick", "BuscarArchivo(" + datos[i][6] + "," + datos[i][0] + ");");
        for (var j = 0; j < datos[i].length; j++) {
            var thDato = document.createElement('td');
            thDato.setAttribute("class", nombCol[j]);
            //thDato.setAttribute("style", "width:10%;");
            if (j > 4) { thDato.setAttribute("style", "display:none;"); }

            thDato.append(datos[i][j]);

            trDatos.appendChild(thDato);
        }
        console.log(trDatos);

        //trDatos.prepend("<td><span class='dot' style='background-color: " + datos[i][7] + "'></span></td>");

        document.getElementById('RConsulArchivos').appendChild(trDatos);
        $('#' + datos[i][5]).prepend("<td><span class='dot' style='background-color: " + datos[i][7] + "'></span></td>");

    }
    var myTable = document.getElementById("RConsultaMeler");
    var rowCount = myTable.rows.length;
    for (var x = rowCount - 1; x > -1; x--) {
        myTable.deleteRow(x);
    }
    var urlBuscarArchivo = $("#urlBuscarArchivo").val();
    arr.length = 0;
    arrNoCotiza.length = 0;
    fields = {
        fecha: fechaEnvioSeleccionada,
        numArchivo: numArchivoSeleccionado,
        caso: metodoDeBusqueda
    };
    sendValues(fields, doSuccessBuscar, doError, urlBuscarArchivo);
}