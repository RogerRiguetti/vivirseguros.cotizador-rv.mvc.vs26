using CrystalDecisions.CrystalReports.Engine;
using Estudio.Logic;
using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class TasasMercadoController : Controller
    {
        TasasMercadoLogic _TasasMercadoLogic = new TasasMercadoLogic();
        #region No Transaccionales
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-16
        /// </summary>
        /// <returns>genera la vista</returns>
        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");
            ViewBag.TipoMoneda = new SelectList(_TasasMercadoLogic.TiposMoneda(), "ClaveMoneda", "Elemento");
            ViewBag.Year = new SelectList("");
            return View();
        }
        #endregion
        #region Transaccionales
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-16
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste de la moneda</param>
        /// <returns>retorna la lista de los años a la vista</returns>
        public ActionResult ListaYear(string vlMoneda, int vlReajuste)
        {
            try
            {
                var resultado = _TasasMercadoLogic.ListaYear(vlMoneda, vlReajuste);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="vlAnno">valor del año</param>
        /// <returns>Información del año</returns>
        public ActionResult BuscarYear(string vlMoneda, int vlReajuste,int vlAnno)
        {
            try
            {
                var resultado = _TasasMercadoLogic.BuscarYear(vlMoneda, vlReajuste, vlAnno);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="Meses">Arreglo con la informacion del mes</param>
        /// <param name="bandera">bandera para activar el guardado o modificación</param>
        /// <returns>retorna la lista de los años</returns>
        public ActionResult GrabarTasaMercado(string vlMoneda, int vlReajuste, TasaMercado Meses,Boolean bandera)
        {
            try
            {

                var resultado = _TasasMercadoLogic.GrabarTasaMercado( vlMoneda, vlReajuste, Meses, bandera);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="year">valor del año</param>
        /// <returns>lista actual de años</returns>
        public ActionResult EliminarTasaMercado(string vlMoneda, int vlReajuste, int year)
        {
            try
            {

                var resultado = _TasasMercadoLogic.EliminarTasaMercado(vlMoneda, vlReajuste, year);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-10 
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="moneda">valor de la moneda</param>
        /// <returns>lista con la información del reporte</returns>
        public ActionResult Reporte(string vlMoneda, int vlReajuste, string moneda)
        {//Informe 
            var resultExportacion = _TasasMercadoLogic.ConsultaRpt(vlMoneda, vlReajuste, moneda);

            ReportDocument rpt = new ReportDocument();

            rpt.Load(Server.MapPath("~/Resources/reports/MA_Rpt_EcoTasasTM.rpt"));
            rpt.SetDataSource(resultExportacion);
            try
            {
                Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
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