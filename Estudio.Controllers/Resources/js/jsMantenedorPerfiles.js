var filtroPermiso;
var filtroIdSistema;
var listaPermisos=[];
var strgInsertUpdate;
var FiltroRoles = [];
var comboRoles = [];

(function () {
    localStorage.pagina = 'MantenedorPerfiles';
    initTable();
    cargaDatosTabla();
    $("#btnSearch").click(function () {
        filtro();

    });
    $(".chosen-select").chosen({ width: "95%" });

    // just for the demos, avoids form submit
    jQuery.validator.setDefaults({
        debug: true,
        success: "valid"
    });

    if ($("#MantenedorPerfilesCreate").val().toLowerCase() != "true") {
        $('#btnAdd').prop("href", "#");
        $('#btnAdd').css("cursor", "no-drop");
        $('#btnAdd').prop("title", "You have not permission");
    }

    $("#btnSave").click(function () {     
        var url = $("#UrlUpdatePermiso").val();

        if (listaPermisos.length > 0) {
            var fields =
            {
                ListaPermisos: listaPermisos
            }
            sendValues(fields, doSuccessUpdatePermiso, doErrorDelete, url);
         
        } else {
            var header = "No hay Nuevos Datos Por Guardar";
           
            var body = "Primero debes seleccionar los permisos que deseas guardar";
            
            aviso(header, body);
            
        }
               
        
    });

    llenarComboRoles();
    //$('th').hover(function () {
    //    let indiceColumna = $(this).parent().children().index(this);
    //    $(this).addClass('resaltar');

    //    $(`table td:nth-child(${indiceColumna + 1})`).addClass('resaltar');
    //}, function () {
    //    $('table tr').children().removeClass('resaltar');
    //});

    //$('th').click(function () {
    //    $(this).hide();

    //    let indiceColumna = $(this).parent().children().index(this);

    //    $(`table td:nth-child(${indiceColumna + 1})`).hide();
    //});
   
})();

function initTable() {

    $("#tblCatalog").DataTable({
        "scrollY": "auto",
        //"scrollCollapse": true,
        "scrollX": "auto",
        "bAutoWidth": true,
        "pageLength": 10,
        "order": [[2, 'asc']],
        "fixedColumns": {
            "leftColumns": 5
        },
        "language": {
            "sEmptyTable": "No se encontró ningún dato disponible en esta tabla",
            "sInfoPostFix": "",
            "sSearch": "",
            "sUrl": "",
            "sInfoThousands": ",",
            "searchPlaceholder": "Buscar"
        },
        "responsive": false,
        "columnDefs": [
            { "className": "text-center" }
        ],
        "bDestroy": true,
        "dom": '<lf<t><"col-lg-6 col-md-6 col-sm-12"i>p>'
    });
    var table = $('#tblCatalog').DataTable();
    table.columns.adjust().draw();


}

function filtro() {
    var url = $("#UrlSearch").val();
    var clavePermiso = document.getElementById("BusquedaPermiso").value.trim();
    var idSistema = document.getElementById("cmbxSistemas").value == "" ? 0 : document.getElementById("cmbxSistemas").value;
    
    //if (clavePermiso == ""  ) {
    //    aviso("Aviso", "Debe ingresar al menos un filtro");
    //}
    //else {
        var fields =
               {
                    clavePermiso: clavePermiso,
                    idSistema: idSistema
               };

        sendValues(fields, doSuccessConsulta, doErrorConsulta, url);
    //}
    filtroPermiso = clavePermiso;
    filtroIdSistema = idSistema;

   
}

function cargaDatosTabla() {
    var url = $("#UrlSearch").val();
    listaPermisos = [];
    var fields =
    {
        clavePermiso: '',
        idSistema: 0
    };
    sendValues(fields, doSuccessConsulta, doErrorConsulta, url);
}

function doSuccessConsulta(result) {
 
    var permisos = result.Object.Permisos;
    var roles = result.Object.Roles;
    var rolesPermisos = result.Object.RolesPermisos;
    var permisosTemporal = rolesPermisos;

    $("#tblCatalog").DataTable().destroy();

    var $body = $("#tblCatalog tbody");
    $body.empty();

    permisos.forEach(function (v) {
        var tr = $('<tr>');
        var td = $('<td>');

        var contadorPermiso = 0;
        var permisoCheck;
        $("<td>").html(v.NamePantalla).appendTo(tr);
        $("<td>").html(v.NodoPadre).appendTo(tr);
        $("<td>").html(v.Description).appendTo(tr);
        $("<td>").html(v.Active).appendTo(tr);
        $("<td>").html(v.NombreSistema).appendTo(tr);

        if (rolesPermisos.length > 0) {
            permisosTemporal = rolesPermisos.filter(RolName => RolName.DescripcionPermiso === v.Description);

            roles.forEach(function (r) {
                if (contadorPermiso < permisosTemporal.length) {
                    if (permisosTemporal[contadorPermiso].ClaveRol == r.Clave) {

                        permisoCheck = "checked";
                        contadorPermiso++;
                    }
                    else
                        permisoCheck = "";

                }
                else
                    permisoCheck = "";

                if (v.Active == "Activo") {
                    $("<td style='text-align: center;'>").html("<input style='transform: scale(1.0)' data-toggle='tooltip' data-placement='top' title='" + v.Description + " Pertenece al sistema " + v.NombreSistema   +"'  id = '" + v.Id + "_" + r.Clave + "' type='checkbox'" + permisoCheck + " onclick='onInsertUpdate(\"" + v.Id + "#" + r.Clave + "\")' />").appendTo(tr);

                }
                else {
                    permisoCheck = "";// se limpia el valor para que pueda marcarla como desactivado
                    $("<td style='text-align: center;'>").html("<input  style='transform: scale(1.0)' data-toggle='tooltip' data-placement='top' title='" + v.Description + " Pertenece al sistema " + v.NombreSistema +"'   id = '" + v.Id + "_" + r.Clave + "' type='checkbox' disabled" + permisoCheck + "  onclick='onInsertUpdate(\"" + v.Id + "#" + r.Clave + "\")' />").appendTo(tr);

                }

            });
        } else {
            roles.forEach(function (r) {
               permisoCheck = "";
                if (v.Active == "Activo") {
                    $("<td style=' text-align: center;'>").html("<input style='transform: scale(1.0)'  data-toggle='tooltip' data-placement='top' title='" + v.Description + " Pertenece al sistema " + v.NombreSistema + "'   id = '" + v.Id + "_" + r.Clave + "' type='checkbox'" + permisoCheck + " onclick='onInsertUpdate(\"" + v.Id + "#" + r.Clave + "\")' />").appendTo(tr);
                } else {
                    permisoCheck = ""; // se limpia el valor para que pueda marcarla como desactivado
                    $("<td style=' text-align: center;'> ").html("<input style='transform: scale(1.0)' data-toggle='tooltip' data-placement='top' title='" + v.Description + " Pertenece al sistema " + v.NombreSistema + "'   id = '" + v.Id + "_" + r.Clave + "' type='checkbox' disabled" + permisoCheck + "  onclick='onInsertUpdate(\"" + v.Id + "#" + r.Clave + "\")' />").appendTo(tr);

                }

            });
        }
    
        if ($("#RedirectToDetails").val() != undefined)
            $("<div class='col-xs-3'><a href='javascript:Details(" + v.Id + ");' title='" + 'Detalles '+"(la Clave del permiso es: " + v.Description + " y Pertenece al sistema " + v.NombreSistema  + ")"+ "'><i class='fa fa-search fa-lg icon-vida'></i></a></div>").appendTo(td);
        else
            $("<div class='col-xs-3'><a href='#' disabled='true' title='You have not permission'><i class='fa fa-ban fa-lg icon-vida'></i></a></div>").appendTo(td);

        if ($("#RedirectToEdit").val() != undefined)
            $("<div class='col-xs-3'><a href='javascript:Edit(" + v.Id + ");' title='" + 'Editar ' + "(la Clave del permiso es: " + v.Description + " y Pertenece al sistema " + v.NombreSistema + ")" + "'><i class='fa fa-edit fa-lg icon-vida'></i></a></div>").appendTo(td);
        else
            $("<div class='col-xs-3'><a href='#' disabled='true' title='You have not permission'><i class='fa fa-ban fa-lg icon-vida'></i></a></div>").appendTo(td);

        if ($("#urlDelete").val() != undefined) {
            if (v.Active == "Activo") {
                $("<div class='col-xs-3'><a href='javascript:Delete(" + v.Id + "," + 0 + ");' title='" + 'Desactivar ' + "(la Clave del permiso es: " + v.Description + " y Pertenece al sistema " + v.NombreSistema + ")" + "'><i class='fa fa-remove fa-lg icon-vida'></i></a></div>").appendTo(td);
            }
            else {
                $("<div class='col-xs-3'><a href='javascript:Delete(" + v.Id + ", " + 1 + ");' title='" + 'Activar ' + "(la Clave del permiso es: " + v.Description + " y Pertenece al sistema " + v.NombreSistema + ")" + "'><i class='fa fa-check fa-lg icon-vida'></i></a></div>").appendTo(td);
            }
        }
        else
            $("<div class='col-xs-3'><a href='#' disabled='true' title='You have not permission'><i class='fa fa-ban fa-lg icon-vida'></i></a></div>").appendTo(td);

        tr.append(td);
        $body.append(tr)
    });
    initTable();

    ocultarColumnasRoles();
}

function doErrorConsulta(result) {
    aviso("Aviso", result.Message);
}

function Delete(id, active) {
  
    var header = "Confirmar";
    if (active == 1) {
        var body = "¿Deseas activar este Permiso?";
    } else {
        var body = "¿Deseas desactivar este Permiso?";
    }
    confirmarPermiso(header, body, function () { EliminarPermiso(id, active); });

}

function EliminarPermiso(id, active) {
    var url = $("#urlDelete").val();
   
    var method = url;
    var fields = { id: id, active: active }
    sendValues(fields, doSuccessDelete, doErrorDelete, method);
   
}

var doSuccessDelete = function (result) {
    avisoRecarga(result.Message, "Se ha cambiado correctamente");
    var url = $("#UrlSearch").val();
    var fieldsEliminar =
            {
                    clavePermiso: filtroPermiso,
                    idSistema: filtroIdSistema
            }; 
    sendValues(fieldsEliminar, doSuccessConsulta, doErrorConsulta, url);
      
}

var doErrorDelete = function (result) {
    aviso(result.Message, "Error");
}

function aviso(header, body) {
    $('#msg_modal_header').text(header);

    $('#msg_modal_body').text(body);
    $('#sch_modal').modal('show');

    $("#btn_modal_aceptar").one('click', function () {
        $('#sch_modal').modal('hide');
    });
}

function confirmarPermiso(header, body, fnaceptar) {
    $('#msg_modal_header_confirm').text(header);
    $('#msg_modal_body_confirm').text(body);
    $('#sch_modal_confirm').modal('show');

    $("#btn_modal_aceptar_confirm").one('click', function () {
        fnaceptar(); $('#sch_modal_confirm').modal('hide');
    });
}

function Edit(id) {
    var url = $("#RedirectToEdit").val();
    window.location.href = url + "?id=" + id;
}

function onInsertUpdate(strgInsertUpdate) {
   
    listaPermisos.push(strgInsertUpdate);
    strgInsertUpdate = "";
}

var doSuccessUpdatePermiso = function (result) {
    avisoRecarga(result.Message, "Se ha cambiado correctamente");
  
}

function Details(id) {
    var url = $("#RedirectToDetails").val();
    window.location.href = url + "?id=" + id;
}

function llenarComboRoles() {
    var url = $("#urlConsultarRoles").val();

    sendValues(null, doSuccessConsultarRoles, doErrorConsulta, url);
}

function doSuccessConsultarRoles(result) {
     
    comboRoles = [];

    comboRoles = result.Object.rolesCatalogoJs;

    for (var i = 0; i < result.Object.rolesCatalogoJs.length; i++) {
        var rolItem = result.Object.rolesCatalogoJs[i];
        var o = new Option(rolItem.Name, rolItem.UserId);
        $(o).html(rolItem.Name);
        $("#cmbxRolesDetails").append(o);

        $('#cmbxRolesDetails').selectpicker('refresh');
    }
}

function ocultarColumnasRoles() {
    FiltroRoles = [];
    FiltroRoles = $("#cmbxRolesDetails").selectpicker('val'); // regresa algo asi  ['1','2','3']

    if (FiltroRoles.length > 0) {
        comboRoles.forEach(function (q) {

            //comparo el valor de el areglo Filtroroles con el areglo comboRoles
            var x = FiltroRoles.find(element => element === q.UserId.toString());

            //si x tiene valor es porque si encontro algo en el arrray
            // si x = undefined es por que no tiene datos
            var table = $('#tblCatalog').DataTable();
            var column = table.column(q.IndexRol);
            //si el filtro de roles falla ir a la consulta
          //  SELECT UserId, Name, Clave, Active, ROW_NUMBER() OVER(ORDER BY name ASC) + 4  AS indexRol FROM UserProfiles WHERE Active = 1 ORDER BY Name ASC
          //  y modificar el IndexRol  sumarle o reslatarle al row_number ya que se vasa en el numero de columnas que hay antes de los filtros contando de cero en adelante
            ///esa consulta esta en [VCE_Catalogos]
            // si es indefinido lo esconde
            if (x === undefined) {
                // oculta
                column.visible(false);
            }  //si no lo muestra
            else {
                // muestra
                column.visible(true);
            }

        });
    }


    
}


function avisoRecarga(header, body) {
    $('#msg_modal_header').text(header);

    $('#msg_modal_body').text(body);
    $('#sch_modal').modal('show');

    $("#btn_modal_aceptar").one('click', function () {
        $('#sch_modal').modal('hide');
        location.reload();
    });
}