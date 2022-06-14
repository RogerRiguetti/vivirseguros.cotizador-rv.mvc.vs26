using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estudio.Logic;
using Estudio.Repository.Helpers;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class ManFecAceptacionCotizacionController : Controller
    {
        ManFecAceptacionCotizacionLogic _ManFecAceptacionCotizacion = new ManFecAceptacionCotizacionLogic();
        // GET: ManFecAceptacionCotizacion
        public ActionResult Index()
        {
            DateTime thisDay = DateTime.Today;
            ViewBag.fecha = thisDay.ToString("yyyy-MM-dd");
            return View();
        }
        public ActionResult BuscarArchivo(string cuspp, string NumOperacion, string NumCotizacion, string NumCorrelativo) {
            try
            {

                //var resultado = _ProCarArchivoLogic.cargarXML(__doc, archivo, nombre, us, tipo, fecha, hora);
                var resultado = _ManFecAceptacionCotizacion.BuscarArchivo(cuspp,NumOperacion, NumCotizacion, NumCorrelativo);
                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult ActualizarArchivo(string NumOperacion, string NumCotizacion, string NumCorrelativo, string FecCierre)
        {
            try
            {

                //var resultado = _ProCarArchivoLogic.cargarXML(__doc, archivo, nombre, us, tipo, fecha, hora);
                var resultado = _ManFecAceptacionCotizacion.ActualizarArchivo(NumOperacion, NumCotizacion, NumCorrelativo,FecCierre);
                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}