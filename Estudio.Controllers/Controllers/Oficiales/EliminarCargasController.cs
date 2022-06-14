using Estudio.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class EliminarCargasController : Controller
    {
        // GET: EliminarCargas
        EliminarCargasLogic _eliCarga = new EliminarCargasLogic();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult busqueda(string caso, string numArch, string nomArch, string tipoArchivo)
        {
            var res = _eliCarga.busqueda(caso, numArch, nomArch, tipoArchivo);
            return Json(res);
        }
        public ActionResult eliminar(string caso, string numArch, string numArchS)
        {
            var res = _eliCarga.eliminar(caso, numArch, numArchS);
            return Json(res);
        }
        public ActionResult busquedaDeNumsArchs(string fecha, string caso)
        {
            var res = _eliCarga.busquedaDeNumsArchs(fecha, caso);
            return Json(res);
        }
    }
}