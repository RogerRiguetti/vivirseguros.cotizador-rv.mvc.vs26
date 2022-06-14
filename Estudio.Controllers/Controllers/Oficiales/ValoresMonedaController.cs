using System;
using Estudio.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class ValoresMonedaController : Controller
    {
        ValoresMonedaLogic _ValoresMonedaLogic = new ValoresMonedaLogic();


        #region No Transacionales
        // GET: ValoresMoneda
        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            DateTime thisDay = DateTime.Today;
            ViewBag.fecha = thisDay.ToString("yyyy-MM-dd");
            ViewBag.TipoMoneda = new SelectList(_ValoresMonedaLogic.TiposMoneda(), "ClaveMoneda", "Elemento");
            return View();
        }


        #endregion

        #region Transacionales
        public ActionResult ListaValoresVM(string vlMoneda)
        {
            try
            {
                var resultado = _ValoresMonedaLogic.ListaValoresVM(vlMoneda);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult BuscarValor(string vlMoneda, DateTime fechaVM)
        {
            try
            {
                var resultado = _ValoresMonedaLogic.BuscarValorVM(vlMoneda, fechaVM.ToString("yyyyMMdd"));

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult GrabarValor(string vlMoneda, DateTime fechaVM, decimal valorM, bool bandera)
        {
            try
            {
                var resultado = _ValoresMonedaLogic.GrabarValorVM(vlMoneda, fechaVM.ToString("yyyyMMdd"), valorM, bandera);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult EliminarValor(string vlMoneda, DateTime fechaVM)
        {
            try
            {
                var resultado = _ValoresMonedaLogic.EliminaValorVM(vlMoneda, fechaVM.ToString("yyyyMMdd"));

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult ReporteVM(string vlMoneda, string strMoneda)
        {
            var resultExportacion = _ValoresMonedaLogic.ReporteVM(vlMoneda, strMoneda);

            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/MA_Rpt_EcoMoneda.rpt"));
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