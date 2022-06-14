using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Estudio.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class TasaAnclajeController : Controller
    {
        TasaAnclajeLogic _TasaAnclajeLogic = new TasaAnclajeLogic();
        #region No Transaccionales
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-08
        /// </summary>
        /// <returns>retorna la vista con el combo de tipo moneda cargado</returns>
        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");
            ViewBag.TipoMoneda = new SelectList(_TasaAnclajeLogic.TiposMoneda(), "ClaveMoneda", "Elemento");
            ViewBag.Periodos = new SelectList("");
            DateTime thisDay = DateTime.Today;
            ViewBag.fecha = thisDay.ToString("yyyy-MM-dd");
            return View();
        }
        #endregion

        #region Transaccionales
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-13
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste de la moneda</param>
        /// <returns>retorna la lista de los periodos a la vista</returns>
        public ActionResult ListaPeriodos(string vlMoneda, int vlReajuste)
        {
            try
            {
                var resultado = _TasaAnclajeLogic.ListaPeriodos(vlMoneda, vlReajuste);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-13
        /// </summary>
        /// <param name="strFecIni">fecha de inicio</param>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste de la moneda</param>
        /// <returns>retorna los valores para el llenado de la vista</returns>
        public ActionResult BuscarVigencia(DateTime strFecIni,string vlMoneda, int vlReajuste)
        {
            try
            {
                var resultado = _TasaAnclajeLogic.BuscarVigencia(strFecIni,vlMoneda, vlReajuste);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-14
        /// </summary>
        /// <param name="strFecIni">fecha de Inicio</param>
        /// <param name="strFecFin">fecha fin</param>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste de la moneda</param>
        /// <param name="tasa">valor de la tasa</param>
        /// <param name="bandera">bandera para saber si se va a guardar o actualizar</param>
        /// <returns>retorna un json con la respuesta</returns>
        public ActionResult GrabarTasaAnclaje(DateTime strFecIni, DateTime strFecFin, string vlMoneda, int vlReajuste,decimal tasa,Boolean bandera)
        {
            try
            {
               
                var resultado = _TasaAnclajeLogic.GrabarTasaAnclaje(strFecIni, strFecFin, vlMoneda, vlReajuste,tasa,bandera);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-15
        /// </summary>
        /// <param name="strFecIni">fecha inicio</param>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste de la moneda</param>
        /// <returns>retorna un json con la respuesta</returns>
        public ActionResult EliminarTasaAnclaje(DateTime strFecIni,string vlMoneda, int vlReajuste)
        {
            try
            {

                var resultado = _TasaAnclajeLogic.EliminarTasaAnclaje(strFecIni, vlMoneda, vlReajuste);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-16
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="moneda">valor del texto del combo moneda</param>
        /// <returns>retorna el reporte</returns>
        public ActionResult Reporte(string vlMoneda, int vlReajuste, string moneda)
        {//Informe de Parámetros de Tasa de Anclaje
            var resultExportacion = _TasaAnclajeLogic.ConsultaRpt(vlMoneda, vlReajuste,moneda);

            ReportDocument rpt = new ReportDocument();
            rpt.Load(Server.MapPath("~/Resources/reports/MA_Rpt_ParTasaAnclaje.rpt"));
            rpt.SetDataSource(resultExportacion);
            try
            {
                Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}