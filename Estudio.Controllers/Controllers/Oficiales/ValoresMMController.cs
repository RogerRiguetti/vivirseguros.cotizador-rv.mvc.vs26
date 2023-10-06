using CrystalDecisions.CrystalReports.Engine;
using Estudio.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class ValoresMMController : Controller
    {
        ValoresMonedaMensualLogic _ValoresMonedaMensualLogic = new ValoresMonedaMensualLogic();
        #region  No Transaccionales
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 22/08/2018
        /// </summary>
        /// <returns>retorna la vista</returns>
        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            ViewBag.TipoValor = new SelectList(_ValoresMonedaMensualLogic.TiposValor(), "ClaveMoneda", "Elemento");
            ViewBag.TipoMoneda = new SelectList(_ValoresMonedaMensualLogic.TiposMoneda(), "ClaveMoneda", "Elemento");
            return View();
        }
        #endregion
        #region Transaccionales
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 22/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <returns>retorna la info para la tabla</returns>
        public ActionResult CargarTabla(string vlMoneda, string cod_tipmon)
        {
            try
            {
                var resultado = _ValoresMonedaMensualLogic.CargarTabla(vlMoneda, cod_tipmon);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        ///  Osvaldo Valdez Carrillo
        /// 23/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <param name="fec_moneda">valor de la fecha</param>
        /// <returns>retorna el resultado de la consulta</returns>
        public ActionResult Consulta(string vlMoneda, string cod_tipmon,DateTime fec_moneda)
        {
            try
            {
                var resultado = _ValoresMonedaMensualLogic.Consulta(vlMoneda, cod_tipmon, fec_moneda);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 23/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <param name="fec_moneda">valor de la fecha</param>
        /// <param name="valor">valore del periodo</param>
        /// <param name="bandera">bandera para actualizar o guardar</param>
        /// <returns>retorna el resultado de guardar o actualizar</returns>
        public ActionResult GrabarValoresMoneda(string vlMoneda, string cod_tipmon, DateTime fec_moneda,decimal valor,Boolean bandera)
        {
            try
            {
                string usuario = Convert.ToString(this.Session["Account"]);
                var resultado = _ValoresMonedaMensualLogic.GrabarValoresMoneda(vlMoneda, cod_tipmon, fec_moneda, valor, bandera, usuario);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 23/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <param name="fec_moneda">valor de la fecha</param>
        /// <returns>retorna el resultado de la eliminacion</returns>
        public ActionResult EliminarValoresMoneda(string vlMoneda, string cod_tipmon, DateTime fec_moneda)
        {
            try
            {
                var resultado = _ValoresMonedaMensualLogic.EliminarValoresMoneda(vlMoneda, cod_tipmon, fec_moneda);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 23/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="cod_tipmon">valor del tipo valor</param>
        /// <param name="moneda">valor del texto de la moneda</param>
        /// <returns>retorna el reporte</returns>
        public ActionResult Reporte(string vlMoneda, string cod_tipmon,string moneda)
        {//Informe de Parámetros de Tasa de Anclaje
            var resultExportacion = _ValoresMonedaMensualLogic.ConsultaRpt(vlMoneda, cod_tipmon, moneda);

            ReportDocument rpt = new ReportDocument();
            rpt.Load(Server.MapPath("~/Resources/reports/MA_Rpt_EcoMonedaMen.rpt"));
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