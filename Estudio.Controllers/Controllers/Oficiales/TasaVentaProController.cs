using System;
using Estudio.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using Estudio.Repository.Core.Domain;


namespace Estudio.Controllers.Controllers.Oficiales
{
    public class TasaVentaProController : Controller
    {
        TasaVentaProLogic _tipoM = new TasaVentaProLogic();
        TasaVentaProLogic _TasaVentaProLogic = new TasaVentaProLogic();
        public ActionResult Index()
        {
            List<TasaVentaPro> tiposMoneda = _tipoM.TiposMoneda();

            ViewBag.TipoMoneda = new SelectList(tiposMoneda, "ClaveMoneda", "Elemento");
            ViewBag.RangoTasa = new SelectList(_tipoM.RangosTasa(), "IdRangoTasa", "Elemento");
            return View();
        }
        public ActionResult CargarTabla(string vlMoneda, string tipReajuste, string cod_Pres)
        {
            try
            {
                var resultado = _TasaVentaProLogic.CargarTabla(vlMoneda, tipReajuste, cod_Pres);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult Guardar(string vlMoneda, string tipReajuste, string cod_Pres, decimal prom, string fecha)
        {
            try
            {
                var resultado = _TasaVentaProLogic.Guardar(vlMoneda, tipReajuste, cod_Pres, prom, fecha);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 02-01-2018
        /// </summary>
        /// <param name="vlMoneda"></param>
        /// <param name="tipReajuste"></param>
        /// <param name="cod_Pres"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public ActionResult BuscarTasa(string vlMoneda, string tipReajuste, string cod_Pres, string fecha)
        {
            try
            {
                var resultado = _TasaVentaProLogic.BuscarTasa(vlMoneda, tipReajuste, cod_Pres, fecha);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 02-01-2018
        /// </summary>
        /// <param name="vlMoneda"></param>
        /// <param name="tipReajuste"></param>
        /// <param name="cod_Pres"></param>
        /// <param name="fecha"></param>
        /// <param name="prom"></param>
        /// <returns></returns>
        public ActionResult NuevaTasaVta(string vlMoneda, string tipReajuste, string cod_Pres, string fecha, decimal prom)
        {
            try
            {
                string usuario = Convert.ToString(this.Session["Account"]);
                var resultado = _TasaVentaProLogic.NuevaTasaVta(vlMoneda, tipReajuste, cod_Pres, fecha, prom, usuario);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}