
var select = false;
var TipoRentaBox = [];
var ModalidadBox = [];
var SexoBox = [];
var EstadoCivilBox = [];
var ClienteGrupoBox = [];
var TipoMonedaBox = [];

(function () {
    localStorage.pagina = 'Filtros';
    cargarCombos();
})();

$(document).ready(function () {
    $(".Num").keydown(function (event) {
        if (event.shiftKey) {
            event.preventDefault();
        }

        if (event.keyCode == 46 || event.keyCode == 8) {
        }
        else {
            if (event.keyCode < 95) {
                if (event.keyCode < 48 || event.keyCode > 57) {
                    event.preventDefault();
                }
            }
            else {
                if (event.keyCode < 96 || event.keyCode > 105) {
                    event.preventDefault();
                }
            }
        }
    });
    $(".Punto").keydown(function (event) {
        if (event.shiftKey) {
            event.preventDefault();
        }

        if (event.keyCode == 46 || event.keyCode == 8) {
        }
        else {
            if (event.keyCode < 95) {
                if (event.keyCode < 48 || event.keyCode > 57) {
                    event.preventDefault();
                }
            }
            else {
                if (event.keyCode == 110 || event.keyCode == 190) {
                } else {
                    if (event.keyCode < 96 || event.keyCode > 105) {
                        event.preventDefault();
                    }
                }
            }
        }
    });
});

function cargarCombos() {
    var urlLoad = $("#filtros_Load").val();
    var fields = {
    };
    sendValues(fields, doSuccessLoad, doError, urlLoad);
}
function doSuccessLoad(result) {
    var array = result.Object;
    llenarInfo("TipoRentaBox", array[0]);
    llenarInfo("ModalidadBox", array[1]);
    llenarInfo("SexoBox", array[2]);
    llenarInfo("EstadoCivilBox", array[3]);
    llenarInfo("ClienteGrupoBox", array[4]);
    llenarInfo("TipoMonedaBox", array[5]);
}
function doError(result) {
    aviso("Error", result.Message);
}

function llenarInfo(combo, datos) {
    for (var i = 0; i < datos.length; i++) {
        var label = document.createElement("label");
        var check = document.createElement("input");
        var idCheck = (datos[i][1] + "#" + datos[i][0].replace(/ /g, "")).toLowerCase();
        var div = document.createElement("div");
        var check = "<input class='chk_filtros' type='checkbox' name='" + combo + "' id='" + idCheck + "' ></input>"
        label.innerHTML = (datos[i][1] + " - " + datos[i][0]);
        label.setAttribute("for", idCheck);
        label.setAttribute("class", "form-label lblFiltrosCheck");
        div.setAttribute("id", "combos");
        div.innerHTML = check;
        div.appendChild(label)
        document.getElementById(combo).append(div);
    }
    switch (combo) {
        case "TipoRentaBox":
            funcionClick(combo, TipoRentaBox); break;
        case "ModalidadBox":
            funcionClick(combo, ModalidadBox); break;
        case "SexoBox":
            funcionClick(combo, SexoBox); break;
        case "EstadoCivilBox":
            funcionClick(combo, EstadoCivilBox); break;
        case "ClienteGrupoBox":
            funcionClick(combo, ClienteGrupoBox); break;
        case "TipoMonedaBox":
            funcionClick(combo, TipoMonedaBox); break;
    }
}
function salir() {
    var url = $("#urlSalir").val();
    window.location.href = url;
}


function Seleccionar() {
    if (select == false) {
        //$("input[type=checkbox]").prop('checked', true); //todos los check
        $("#combos input[type=checkbox]").prop('checked', true); //solo los del objeto #combos
        select = true;
        checkboxes = document.getElementsByTagName('input');
        for (i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == "checkbox" && checkboxes[i].name == "TipoRentaBox") {
                funcionLlenarTodo(TipoRentaBox, checkboxes[i].id);
            }
            if (checkboxes[i].type == "checkbox" && checkboxes[i].name == "ModalidadBox") {
                funcionLlenarTodo(ModalidadBox, checkboxes[i].id);
            }
            if (checkboxes[i].type == "checkbox" && checkboxes[i].name == "SexoBox") {
                funcionLlenarTodo(SexoBox, checkboxes[i].id);
            }
            if (checkboxes[i].type == "checkbox" && checkboxes[i].name == "EstadoCivilBox") {
                funcionLlenarTodo(EstadoCivilBox, checkboxes[i].id);
            }
            if (checkboxes[i].type == "checkbox" && checkboxes[i].name == "ClienteGrupoBox") {
                funcionLlenarTodo(ClienteGrupoBox, checkboxes[i].id);
            }
            if (checkboxes[i].type == "checkbox" && checkboxes[i].name == "TipoMonedaBox") {
                funcionLlenarTodo(TipoMonedaBox, checkboxes[i].id);
            }
        }
    } else {
        //$("input[type=checkbox]").prop('checked', false);//todos los check
        $("#combos input[type=checkbox]").prop('checked', false);//solo los del objeto #combos
        select = false;
        TipoRentaBox.length = 0;
        ModalidadBox.length = 0;
        SexoBox.length = 0;
        EstadoCivilBox.length = 0;
        ClienteGrupoBox.length = 0;
        TipoMonedaBox.length = 0;
    }
}

function aceptar() {
    var primaDes = parseFloat($("#txtPrimaDesde").val());
    var primaHas = parseFloat($("#txtPrimaHasta").val());
    var edadMin = parseInt($("#txtEdadIni").val());
    var edadMax = parseInt($("#txtEdadFin").val());

    if ($("#txtPrimaDesde").val() == "" && $("#txtPrimaHasta").val() == "") {
        aviso("Aviso", "Debe ingresar valores para la prima.");
        return;
    }

    if ($("#txtPrimaDesde").val() == "" || $("#txtPrimaHasta").val() == "") {
        aviso("Aviso", "Debe ingresar valores para la prima.");
        return;
    }

    if ($("#txtEdadIni").val() == "" && $("#txtEdadFin").val() == "") {
        aviso("Aviso", "Debe ingresar valores para la edad.");
        return;
    }

    if ($("#txtEdadIni").val() == "" || $("#txtEdadFin").val() == "") {
        aviso("Aviso", "Debe ingresar valores para la edad.");
        return;
    }

    if (primaDes >= primaHas) {
        aviso("Aviso", "Rango de Prima Min incorrecto, no puede ser mayor a la Prima Max.");
        return;
    }

    if ((primaDes < 0) && (primaHas > 150)) {
        aviso("Aviso", "Rangos de prima incorrectos. \nIngrese un rango entre 0 y 99,999,999.99");
        return;
    }

    if (edadMin >= edadMax) {
        aviso("Aviso", "Rango de Edad Min incorrecto, no puede ser mayor a la Edad Max.");
        return;
    }

    if ((edadMin < 0) && (edadMax > 150)) {
        aviso("Aviso", "Rangos de Edad incorrectos. \nIngrese un rango entre 0 y 150");
        return;
    }

    var urlAceptar = $("#urlAceptar").val();
    var fields = {
    };
    sendValues(fields, doSuccessAceptar, doError, urlAceptar);
}
function doSuccessAceptar(result) {
    var array = result.Object;

    confirmarOperacion(array[0], array[1], () => {

        var urllistasSeleccionadas = $("#urllistasSeleccionadas").val();
        var primaDes = $("#txtPrimaDesde").val();
        var primaHas = $("#txtPrimaHasta").val();
        var edadMax = $("#txtEdadIni").val();
        var edadMin = $("#txtEdadFin").val();
        var valor = $("#cmbPrincipal").val();
        var array = [];
        array.push(TipoRentaBox);
        if (ModalidadBox.length == 0) { ModalidadBox.push("-1"); }
        array.push(ModalidadBox);
        if (ClienteGrupoBox.length == 0) { ClienteGrupoBox.push("-1"); }
        array.push(ClienteGrupoBox);
        if (SexoBox.length == 0) { SexoBox.push("-1"); }
        array.push(SexoBox);
        if (EstadoCivilBox.length == 0) { EstadoCivilBox.push("-1"); }
        array.push(EstadoCivilBox);
        if (TipoMonedaBox.length == 0) { TipoMonedaBox.push("-1"); }
        array.push(TipoMonedaBox);

        var fields = {
            combos: array,
            edadDesde: edadMax,
            edadHasta: edadMin,
            primaDesde: primaDes,
            primaHasta: primaHas,
            caso: valor
        };
        sendValues(fields, doSuccessConfirAceptar, doError, urllistasSeleccionadas);
    });
    return;
}
function doSuccessConfirAceptar(result) {
    aviso("Aviso", result.Message);
}
function confirmarOperacion(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}


function maxLengthCheck(object) {
    if (object.value.length > object.maxLength)
        object.value = object.value.slice(0, object.maxLength)
}

function Buscar() {
    TipoRentaBox.length = 0;
    ModalidadBox.length = 0;
    SexoBox.length = 0;
    EstadoCivilBox.length = 0;
    ClienteGrupoBox.length = 0;
    TipoMonedaBox.length = 0;
    $("#combos input[type=checkbox]").prop('checked', false);

    var urlBuscar = $("#urlBuscar").val();
    var valor = $("#cmbPrincipal").val();
    var fields = {
        caso: valor
    };
    sendValues(fields, doSuccessBuscar, doError, urlBuscar);
}
function doSuccessBuscar(result) {
    var array = result.Object;

    $("#txtPrimaDesde").val(array[0][0]);
    $("#txtPrimaHasta").val(array[0][1]);
    $("#txtEdadIni").val(array[0][2]);
    $("#txtEdadFin").val(array[0][3]);

    for (var i = 1; i < array.length; i++) {
        for (var j = 0; j < array[i].length; j++) {
            if (array[i][j] != "") {
                var id = array[i][j].replace(/ /g, "").toLowerCase();
                document.getElementById(id).checked = true;
                var name = document.getElementById(id).name;
                switch (name) {
                    case "TipoRentaBox":
                        TipoRentaBox.push(id.split('#')[0]); break;
                    case "ModalidadBox":
                        ModalidadBox.push(id.split('#')[0]); break;
                    case "SexoBox":
                        SexoBox.push(id.split('#')[0]); break;
                    case "EstadoCivilBox":
                        EstadoCivilBox.push(id.split('#')[0]); break;
                    case "ClienteGrupoBox":
                        ClienteGrupoBox.push(id.split('#')[0]); break;
                    case "TipoMonedaBox":
                        TipoMonedaBox.push(id.split('#')[0]); break;
                }
            }
        }
    }
}

function funcionClick(nombreGrupo, array) {
    var id = "input[name=" + nombreGrupo + "]";
    $(id).click(function () {
        var check = this.id;
        var checkVal = check.split('#')[0];
        var ban = true;
        if (this.checked) {
            if (array.length > 0) {
                for (var i = 0; i < array.length; i++) {
                    if (checkVal == array[i]) {
                        ban = false;
                        break;
                    } else {
                        ban = true;
                    }
                }
                if (ban == true) {
                    array.push(checkVal);
                }
            } else {
                array.push(checkVal);
            }
        } else {
            for (var i = 0; i < array.length; i++) {
                if (checkVal == array[i]) {
                    array.splice(i, 1);
                    break;
                }
            }
        }
    });
}

function funcionLlenarTodo(array, id) {
    var check = id;
    var checkVal = check.split('#')[0];
    var ban = true;
    if (array.length > 0) {
        for (var i = 0; i < array.length; i++) {
            if (checkVal == array[i]) {
                ban = false;
                break;
            } else {
                ban = true;
            }
        }
        if (ban == true) {
            array.push(checkVal);
        }
    } else {
        array.push(checkVal);
    }
}