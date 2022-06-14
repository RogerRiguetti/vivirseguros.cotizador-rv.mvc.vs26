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
    public class ParametroGastoController : Controller
    {
        ParametroGastoLogic _ParametroGastoLogic = new ParametroGastoLogic();

        #region No Transaccionales
        // GET: ParametroGasto
        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");
            ViewBag.TipoMoneda = new SelectList(_ParametroGastoLogic.TiposMoneda(), "ClaveMoneda", "Elemento");
            ViewBag.Periodos = new SelectList("");
            DateTime thisDay = DateTime.Today;
            ViewBag.fecha = thisDay.ToString("yyyy-MM-dd");
            return View();
        }
        #endregion

        #region Transaccionales
        /// <summary>
        /// Lista con los periodos existentes para el tipo de moneda seleccionado.
        /// José Hernández Alvarado.
        /// 31-08-2018
        /// </summary>
        /// <param name="vlMoneda"></param>
        /// <param name="vlReajuste"></param>
        /// <returns>Retorna una lista con los periodos.</returns>
        public ActionResult ListaPeriodos(string vlMoneda, int vlReajuste)
        {
            try
            {
                var resultado = _ParametroGastoLogic.ListaPeriodos(vlMoneda, vlReajuste);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Busca el periodo seleccionado en combo o al dar clic en buscar.
        /// José Hernández Alvarado.
        /// 03-09-2018
        /// </summary>
        /// <param name="strFecIni"></param>
        /// <param name="vlMoneda"></param>
        /// <param name="vlReajuste"></param>
        /// <returns>Retorna los valores para asignarlos a los campos correspondientes.</returns>
        public ActionResult BuscarVigencia(DateTime strFecIni, string vlMoneda, int vlReajuste)
        {
            try
            {
                var resultado = _ParametroGastoLogic.BuscarVigencia(strFecIni, vlMoneda, vlReajuste);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Inserta el nuevo periodo en la BD o modifica el ya existente, actualizando fechas de periodos anteriores o posteriores.
        /// José Hernández Alvarado.
        /// 06-09-2018
        /// </summary>
        /// <param name="vlMoneda">Tipo de Moneda.</param>
        /// <param name="vlReajuste">Valor de Reajuste.</param>
        /// <param name="strFecIni">Fecha de Inicio de Vigencia.</param>
        /// <param name="strFecFin">Fecha Fin de Vigencia.</param>
        /// <param name="gastosCS">Gastos de Control de Superviviencia.</param>
        /// <param name="gastosA">Gastos de Administración.</param>
        /// <param name="gastosE">Gastos de Emisión.</param>
        /// <param name="ctoCapital">Capital.</param>
        /// <param name="nivelE">Nivel de Endeudamiento.</param>
        /// <param name="bandera">Bandera.</param>
        /// <returns>Valores modificados o insertados para llenar campos de la vista.</returns>
        public ActionResult GrabarParametro(string vlMoneda, int vlReajuste, DateTime strFecIni, DateTime strFecFin, decimal gastosCS, decimal gastosA, decimal gastosE, decimal ctoCapital, decimal nivelE, Boolean bandera)
        {
            try
            {
                var usuario = Convert.ToString(this.Session["Account"]);
                var resultado = _ParametroGastoLogic.GrabarParametro(vlMoneda, vlReajuste, strFecIni, strFecFin, gastosCS, gastosA, gastosE, ctoCapital, nivelE, usuario, bandera);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Elimina el periodo ingresado si es que existe y actualiza las fechas de los periodos anteriores o posteriores.
        /// José Hernández Alvarado.
        /// 07-09-2018
        /// </summary>
        /// <param name="vlMoneda">Tipo de Moneda.</param>
        /// <param name="vlReajuste">Valor de Reajuste.</param>
        /// <param name="strFecIni">Fecha de Inicio de Vigencia del periodo.</param>
        /// <returns>Mensaje de confirmación.</returns>
        public ActionResult EliminarParametro(string vlMoneda, int vlReajuste, DateTime strFecIni)
        {
            try
            {
                var usuario = Convert.ToString(this.Session["Account"]);
                var resultado = _ParametroGastoLogic.EliminarParametro(vlMoneda, vlReajuste, strFecIni, usuario);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Consulta el valor de la Tasa de Mercado.
        /// José Hernández Alvarado.
        /// 04-09-2018
        /// </summary>
        /// <param name="vlMoneda"></param>
        /// <param name="vlReajuste"></param>
        /// <returns></returns>
        public ActionResult TasaMercado(string vlMoneda, int vlReajuste)
        {
            try
            {
                var resultado = _ParametroGastoLogic.TasaMercado(vlMoneda, vlReajuste);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Consulta el Impuesto de la Renta.
        /// José Hernández Alvarado.
        /// 05-0-2018
        /// </summary>
        /// <param name="strFecIni">Fecha de Inicio de Vigencia.</param>
        /// <returns>Retorna el valor del impuesto de la renta.</returns>
        public ActionResult ImpuestoRenta(DateTime strFecIni)
        {
            try
            {
                var resultado = _ParametroGastoLogic.ImpuestoRenta(strFecIni);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult ReportePG(string vlMoneda, string strMoneda, int vlReajuste, DateTime strFecIni)
        {
            var resultExportacion = _ParametroGastoLogic.ReportePG(vlMoneda, strMoneda, vlReajuste, strFecIni);

            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_Gastos.rpt"));
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

        public ActionResult ConsultarGastos()
        {
            try
            {
                var resultado = _ParametroGastoLogic.ConsultarGastos();

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult GuardarOtrosGastos(decimal COMSUP1, decimal COMSUP2, decimal PRCFAC1, decimal PRCFAC2)
        {
            try
            {
                var usuario = Convert.ToString(this.Session["Account"]);
                var resultado = _ParametroGastoLogic.GuardarOtrosGastos(COMSUP1, COMSUP2, PRCFAC1, PRCFAC2);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}