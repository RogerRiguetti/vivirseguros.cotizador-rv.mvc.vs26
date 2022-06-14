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
    public class TasaRentabilidadController : Controller
    {
        TasaRentabilidadLogic _tasaRentabilidadLogic = new TasaRentabilidadLogic();
        // GET: TasaRentabilidad
        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            ViewBag.TipoMoneda = new SelectList(_tasaRentabilidadLogic.Moneda(), "ClaveMoneda", "Elemento");
            ViewBag.Periodos = new SelectList("");
            DateTime thisDay = DateTime.Today;
            ViewBag.fecha = thisDay.ToString("yyyy-MM-dd");
            return View();
        }

        public ActionResult ConsultaPeriodos(string tipoMoneda)
        {
            try
            {
                var resultado = _tasaRentabilidadLogic.ListaPeriodos(tipoMoneda);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult ConsultaRentabilidad(string tipoMoneda, DateTime fechaIni)
        {
            try
            {
                var resultado = _tasaRentabilidadLogic.ListaRentabilidad(tipoMoneda, fechaIni);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult ConsultaTasaRen(string tipoMoneda, DateTime fechaIni, int numAnno)
        {
            try
            {
                var resultado = _tasaRentabilidadLogic.ConsultaTasaRen(tipoMoneda, fechaIni, numAnno);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult GrabarTasaRentabilidad(List<beRentabilidad> informacion, string tipoMoneda, DateTime fechaIni, string clave)
        {
            try
            {
                string usuario = Convert.ToString(this.Session["Account"]);
                var resultado = _tasaRentabilidadLogic.Grabar(informacion, tipoMoneda, fechaIni, clave, usuario);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult EliminarTasaRentabilidad(string tipoMoneda, DateTime fechaIni)
        {
            try
            {
                var resultado = _tasaRentabilidadLogic.EliminarTasaRentabilidad(tipoMoneda, fechaIni);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult Reporte(string codMoneda, int reajuste, string moneda,DateTime fechaInicial)
        {//Informe de Parámetros de Tasa de Anclaje
            var resultExportacion = _tasaRentabilidadLogic.ConsultaRpt(codMoneda, reajuste, moneda, fechaInicial);

            ReportDocument rpt = new ReportDocument();
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_PrcRentabilidad.rpt"));
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


    }
}