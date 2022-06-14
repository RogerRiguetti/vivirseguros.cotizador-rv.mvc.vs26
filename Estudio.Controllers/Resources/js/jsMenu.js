$(document).ready(function () {
    var sessionStorage = localStorage.pagina;
    
    if (sessionStorage == "TasaCalce") {
        //OficialesNav
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#TasaCalce").addClass("activa");
    }
    if (sessionStorage == "Filtros") {
        //OficialesNav
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#Filtros").addClass("activa");
    }
    if (sessionStorage == "TasaRentablidad") {
        //LimiteCotizacionInicial
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#TasaRentabilidad").addClass("activa");
    }
    if (sessionStorage == "LimiteCotizacionInicial") {
        //LimiteCotizacionInicial
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#LimiteCotizacionInicial").addClass("activa");
    }
    if (sessionStorage == "LimiteCotizacionExtraOficial") {
        //LimiteCotizacionExtraOficial
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#LimiteCotizacionExtraOficial").addClass("activa");
    }

    if (sessionStorage == "ParametroGasto") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#ParametroGasto").addClass("activa");
    }

    if (sessionStorage == "LimiteCotizacionMejorada") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#LimiteCotizacionMejorada").addClass("activa");
    }

    if (sessionStorage == "MantenedorIPC") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#MantenedorIPC").addClass("activa");
    }

    if (sessionStorage == "ValoresMoneda") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#ValoresMoneda").addClass("activa");
    }

    if (sessionStorage == "ValoresMM") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#ValoresMM").addClass("activa");
    }

    if (sessionStorage == "GastosSepelio") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#GastosSepelio").addClass("activa");
    }

    if (sessionStorage == "TasasMercado") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#TasasMercado").addClass("activa");
    }

    if (sessionStorage == "TasaAnclaje") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#TasaAnclaje").addClass("activa");
    }

    if (sessionStorage == "TVPro") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#TasaVentaPro").addClass("activa");
    }

    if (sessionStorage == "ProCarArchivo") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#RecepcionResNav").removeClass("collapsed");
        $("#RecepcionRes").removeClass("collapse");
        $("#RecepcionRes").addClass("in");
        $("#ProCarArchivo").addClass("activa");
    }

    if (sessionStorage == "SolicitudCotizacion") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#ProcesoCargaNav").removeClass("collapsed");
        $("#ProcesoCarga").removeClass("collapse");
        $("#ProcesoCarga").addClass("in");
        $("#SolicitudCotizacion").addClass("activa");
    }

    if (sessionStorage == "GenArMeler") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#DescargaResNav").removeClass("collapsed");
        $("#DescargaRes").removeClass("collapse");
        $("#DescargaRes").addClass("in");
        $("#GenArMeler").addClass("activa");
    }

    if (sessionStorage == "ExcepcionesLi") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#ExcepcionesNav").removeClass("collapsed");
        $("#Excepciones").removeClass("collapse");
        $("#Excepciones").addClass("in");
        $("#ExcepcionesLi").addClass("activa");
    }
    if (sessionStorage == "ExcepcionesEx") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#ExcepcionesNav").removeClass("collapsed");
        $("#Excepciones").removeClass("collapse");
        $("#Excepciones").addClass("in");
        $("#ExcepcionesEx").addClass("activa");
    }

    if (sessionStorage == "Mejoras") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MejorasNav").removeClass("collapsed");
        $("#Mejoras").removeClass("collapse");
        $("#Mejoras").addClass("in");
        $("#MejorasLi").addClass("activa");
    }

    if (sessionStorage == "Mantenedor") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#RecepcionResNav").removeClass("collapsed");
        $("#RecepcionRes").removeClass("collapse");
        $("#RecepcionRes").addClass("in");
        $("#ManFecAceptacionCotizacion").addClass("activa");
    }

    if (sessionStorage == "Cotizacion") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#ExtraOficialesNav").removeClass("collapsed");
        $("#ExtraOficiales").removeClass("collapse");
        $("#ExtraOficiales").addClass("in");
        $("#Cotizacion").addClass("activa");
    }

    if (sessionStorage == "Role") {
        $("#CuentaNav").removeClass("collapsed");
        $("#Cuenta").removeClass("collapse");
        $("#Cuenta").addClass("in");
        $("#Role").addClass("activa");
    }

    if (sessionStorage == "User") {
        $("#CuentaNav").removeClass("collapsed");
        $("#Cuenta").removeClass("collapse");
        $("#Cuenta").addClass("in");
        $("#User").addClass("activa");
    }

    if (sessionStorage == "Password") {
        $("#CuentaNav").removeClass("collapsed");
        $("#Cuenta").removeClass("collapse");
        $("#Cuenta").addClass("in");
        $("#Password").addClass("activa");
    }

    if (sessionStorage == "MantenedorPerfiles") {
        $("#CuentaNav").removeClass("collapsed");
        $("#Cuenta").removeClass("collapse");
        $("#Cuenta").addClass("in");
        $("#MantenedorPerfiles").addClass("activa");
    }
    if (sessionStorage == "ManCT") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#ManCT").addClass("activa");
    }
    if (sessionStorage == "EliminarCargas") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#EliminacionCargasNav").removeClass("collapsed");
        $("#EliminacionCargas").removeClass("collapse");
        $("#EliminacionCargas").addClass("in");
        $("#EliminarCargas").addClass("activa");
    }

    if (sessionStorage == "ActualizaInfOfi") {
        $("#CotizadorNav").removeClass("collapsed");
        $("#Cotizador").removeClass("collapse");
        $("#Cotizador").addClass("in");
        $("#OficialesNav").removeClass("collapsed");
        $("#Oficiales").removeClass("collapse");
        $("#Oficiales").addClass("in");
        $("#MantenimientoNav").removeClass("collapsed");
        $("#Mantenimiento").removeClass("collapse");
        $("#Mantenimiento").addClass("in");
        $("#ActualizaInfOfi").addClass("activa");
    }

    if (sessionStorage == "Reservas") {
        $("#ReservasNav").removeClass("collapsed");
        $("#Reservas").removeClass("collapse");
        $("#Reservas").addClass("in");
        $("#CalculoResNav").removeClass("collapsed");
        $("#CalculoRes").removeClass("collapse");
        $("#CalculoRes").addClass("in");
        $("#Reservas").addClass("activa");
    }

    if (sessionStorage == "Periodos") {
        $("#ReservasNav").removeClass("collapsed");
        $("#Reservas").removeClass("collapse");
        $("#Reservas").addClass("in");
        $("#CalculoResNav").removeClass("collapsed");
        $("#CalculoRes").removeClass("collapse");
        $("#CalculoRes").addClass("in");
        $("#Periodos").addClass("activa");
    }

    if (sessionStorage == "ManCTRes") {
        $("#ReservasNav").removeClass("collapsed");
        $("#Reservas").removeClass("collapse");
        $("#Reservas").addClass("in");
        $("#ParametrosResNav").removeClass("collapsed");
        $("#ParametrosRes").removeClass("collapse");
        $("#ParametrosRes").addClass("in");
        $("#ManCTRes").addClass("activa");
    }

    if (sessionStorage == "CargaActivos") {
        $("#ReservasNav").removeClass("collapsed");
        $("#Reservas").removeClass("collapse");
        $("#Reservas").addClass("in");
        $("#ParametrosResNav").removeClass("collapsed");
        $("#ParametrosRes").removeClass("collapse");
        $("#ParametrosRes").addClass("in");
        $("#CargaActivos").addClass("activa");
    }

  

    localStorage.removeItem('pagina');

    $(window).scroll(function () {
        /* A full compatability script from MDN: */
        var supportPageOffset = window.pageXOffset !== undefined;
        var isCSS1Compat = ((document.compatMode || "") === "CSS1Compat");

        /* Set up some variables  */
        var Scrollx = document.getElementById("MenuMove");
        /* Add an event to the window.onscroll event */
        window.addEventListener("scroll", function (e) {

            /* A full compatability script from MDN for gathering the x and y values of scroll: */
            var x = supportPageOffset ? window.pageXOffset : isCSS1Compat ? document.documentElement.scrollLeft : document.body.scrollLeft;
            //var y = supportPageOffset ? window.pageYOffset : isCSS1Compat ? document.documentElement.scrollTop : document.body.scrollTop;

            //Scrollx.style.left = -x + 0 + "px";
        });
    });

    obtenerAlto("ventana", $(window).height());

});

$(function () {
    // LÍNEAS PARA EL FUNCIONAMIENTO DEL SCROLL VERTICAL AL ABRIR MENÚ MANTENIMIENTO
    if ($("#Mantenimiento").hasClass("in")) {
        var PGH = $(".container-main").height();
        var NAVMNU = $(".nav-side-menu").height();
        document.body.style.overflowY = 'scroll';
        //$('.nav-side-menu').attr('style', 'position : absolute');
        $('#menuSidex').attr('style', 'position : absolute; top : 0px;');

        if (PGH > NAVMNU) {
            $("#menuSidex").removeAttr("style");
            $('#menuSidex').attr('style', 'position : absolute; top : 0px; height : ' + (PGH + 70) + '' + 'px;' + '');
        }
    }

    $("#MantenimientoNav").click(function () {
        document.body.style.overflowY = 'scroll';
       //$('.nav-side-menu').attr('style', 'position : absolute');
        $('#menuSidex').attr('style', 'position : absolute; top : 0px');

        var PGH = $(".container-main").height();
        var NAVMNU = $(".nav-side-menu").height();

        if (PGH > NAVMNU) {
            $('#menuSidex').attr('style', 'position : absolute; top : 0px; height : ' + (PGH + 70) + '' + 'px;' + '');
        }

        if ($("#Mantenimiento").hasClass("in")) {
            document.body.style.overflowY = 'auto';
            $("#menuSidex").removeAttr("style");
            $(".nav-side-menu").removeAttr("style");

        }
    });

    $("#OficialesNav").click(function () {
        document.body.style.overflowY = 'auto';

        if ($("#Mantenimiento").hasClass("in") && $("#Oficiales").hasClass("collapse")) {
            document.body.style.overflowY = 'scroll';
            $('.nav-side-menu').attr('style', 'position : absolute');
            $('#menuSidex').attr('style', 'position : absolute; top : 0px');

            var PGH = $(".container-main").height();
            var NAVMNU = $(".nav-side-menu").height();
            if (PGH > NAVMNU) {
                $('#menuSidex').attr('style', 'position : absolute; top : 0px; height : ' + (PGH + 70) + '' + 'px;' + '');
            }
            else {
                $("#menuSidex").removeAttr("style");
                $(".nav-side-menu").removeAttr("style");
            }

        }
        else {
            $("#menuSidex").removeAttr("style");
            $(".nav-side-menu").removeAttr("style");
        }

    });
    // TERMINAN LÍNEAS PARA EL FUNCIONAMIENTO DEL SCROLL VERTICAL AL ABRIR MENÚ MANTENIMIENTO

    // elementos de la lista
    var menues = $(".PesPriUl");
    var menuesSec = $(".PesSecUl");
    var menuesTer = $(".PesTerUl");

    // manejador de click sobre todos los elementos
    menues.click(function () {
        // eliminamos active de todos los elementos
        if ($(this).hasClass("click")) {
            menues.addClass("collapsed");
            $(this).removeClass("click");
            $(this).children('.arrow').removeClass("down");


        } else {
            menues.removeClass("click");
            menues.addClass("collapsed");
            menues.children('.arrow').removeClass("down");

            $(this).addClass("click");
            $(this).children('.arrow').addClass("down");


            if ($("#Cuenta").hasClass("in")) {
                $("#Cuenta").removeClass("in");
                $("#Cuenta").addClass("collapse");
            }

            if ($("#Cotizador").hasClass("in")) {
                $("#Cotizador").removeClass("in");
                $("#Cotizador").addClass("collapse");
            }
        }





        // activamos el elemento clicado.
        //$(this).addClass("");
    });

    // manejador de click sobre todos los elementos
    menuesSec.click(function () {
        // eliminamos active de todos los elementos
        if ($(this).hasClass("click")) {
            menuesSec.addClass("collapsed");
            $(this).removeClass("click");
            $(this).children('.arrow').removeClass("down");

        } else {
            menuesSec.removeClass("click");
            menuesSec.addClass("collapsed");
            menuesSec.children('.arrow').removeClass("down");

            $(this).addClass("click");
            $(this).children('.arrow').addClass("down");

            if ($("#ExtraOficiales").hasClass("in")) {
                $("#ExtraOficiales").removeClass("in");
                $("#ExtraOficiales").addClass("collapse");
            }

            if ($("#Oficiales").hasClass("in")) {
                $("#Oficiales").removeClass("in");
                $("#Oficiales").addClass("collapse");
                $("#menuSidex").removeAttr("style");
                $(".nav-side-menu").removeAttr("style");
                document.body.style.overflowY = 'auto';
            }



            //if ($("#ReservasN").hasClass("in")) {
            //    $("#ReservasN").removeClass("in");
            //    $("#ReservasN").addClass("collapse");
            //}
        }



        // activamos el elemento clicado.
        //$(this).addClass("");
    });

    menuesTer.click(function () {
        // eliminamos active de todos los elementos
        if ($(this).hasClass("click")) {
            menuesTer.addClass("collapsed");
            $(this).removeClass("click");
            $(this).children('.arrow').removeClass("down");

        } else {
            menuesTer.removeClass("click");
            menuesTer.addClass("collapsed");
            menuesTer.children('.arrow').removeClass("down");

            $(this).addClass("click");
            $(this).children('.arrow').addClass("down");

            if ($("#Mantenimiento").hasClass("in")) {
                $("#Mantenimiento").removeClass("in");
                $("#Mantenimiento").addClass("collapse");
                $("#menuSidex").removeAttr("style");
                $(".nav-side-menu").removeAttr("style");
            }

            if ($("#ProcesoCarga").hasClass("in")) {
                $("#ProcesoCarga").removeClass("in");
                $("#ProcesoCarga").addClass("collapse");
            }

            if ($("#Mejoras").hasClass("in")) {
                $("#Mejoras").removeClass("in");
                $("#Mejoras").addClass("collapse");
            }

            if ($("#Excepciones").hasClass("in")) {
                $("#Excepciones").removeClass("in");
                $("#Excepciones").addClass("collapse");
            }

            if ($("#DescargaRes").hasClass("in")) {
                $("#DescargaRes").removeClass("in");
                $("#DescargaRes").addClass("collapse");
            }

            if ($("#RecepcionRes").hasClass("in")) {
                $("#RecepcionRes").removeClass("in");
                $("#RecepcionRes").addClass("collapse");
            }

            if ($("#EliminacionCargas").hasClass("in")) {
                $("#EliminacionCargas").removeClass("in");
                $("#EliminacionCargas").addClass("collapse");
            }
        }


        // activamos el elemento clicado.
        //$(this).addClass("");
    });

});

/*$(document).ready(function () {
    $(".cambio li").on("click", function () {
        $(".cambio").find(".activa").removeClass("activa");
        $(this).addClass("activa");
    });
    $(".cambio li").on("click", function () {
        $(".cambio").find(".activa").removeClass("activa");
        $(this).addClass("activa");
    });

    //selector a la clase opcion para darle función al evento click
    $(".collapsed").click(function () {

        //eliminamos la clase activa por si una opción la contiene
        $(".collapsed").removeClass("opcionActiva");

        //asignamos la clase opcionActiva a la opción presionada
        $(this).addClass("opcionActiva")

    })
});*/ 
//<!-- Función para obtener el Ancho(Width) --> 
//function obtenerAncho( obj, ancho ) {
//    alert( "El ancho de la " + obj + " es " + ancho + "px. (Width)" );
//}

//<!-- Función para obtener el Alto(Height) --> 
function obtenerAlto(obj, alto) {
    //alert("El alto de la " + obj + " es " + alto + "px. (Height)");
    if (alto > 767) {
        $(".nav-side-menu").height(alto + 100);
    } else {
        $(".nav-side-menu").css("height", "auto");
    }

}
$(window).resize(function () {
    obtenerAlto("ventana", $(window).height());
});