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
    public class GastosSepelioController : Controller
    {
        GastosSepelioLogic _GastosSepelioLogic = new GastosSepelioLogic();
        #region No Transaccionales
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-20
        /// </summary>
        /// <returns>retorna la vista</returns>
        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");
            DateTime thisDay = DateTime.Today;
            ViewBag.fecha = thisDay.ToString("yyyy-MM-dd");
            return View();
        }
        #endregion
        #region Transaccionales
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-20
        /// </summary>
        /// <returns>Retorna la lista con las fechas</returns>
        public ActionResult CargaFechas()
        {
            try
            {
                var resultado = _GastosSepelioLogic.CargaFechas();

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-20
        /// </summary>
        /// <param name="strFecIni">Fecha inicial</param>
        /// <returns>Retorna la informacion de la fecha consultada</returns>
        public ActionResult ConsultaFecha(DateTime strFecIni)
        {
            try
            {
                var resultado = _GastosSepelioLogic.ConsultaFecha(strFecIni);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-21
        /// </summary>
        /// <param name="strFecIni">>Fecha inicial</param>
        /// <param name="vlGasto">Monto</param>
        /// <param name="bandera">True(graba)  o False(modifica)</param>
        /// <param name="strFecFin">Fecha Termino</param>
        /// <returns>Retotna la respuesta al grabar o Modificar</returns>
        public ActionResult GrabarSepelio(DateTime strFecIni, decimal vlGasto,Boolean bandera,DateTime strFecFin)
        {
            try
            {
                string usuario = Convert.ToString(this.Session["Account"]);
                var resultado = _GastosSepelioLogic.GrabarSepelio(strFecIni, vlGasto,bandera, usuario,strFecFin);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-22
        /// </summary>
        /// <param name="strFecIni">fecha de Inicio</param>
        /// <returns>Retotna la respuesta al eliminar</returns>
        public ActionResult EliminarSepelio(DateTime strFecIni)
        {
            try
            {
                string usuario = Convert.ToString(this.Session["Account"]);
                var resultado = _GastosSepelioLogic.EliminarSepelio(strFecIni);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-22
        /// </summary>
        /// <returns>retorna el reporte</returns>
        public ActionResult Reporte()
        {
            var resultExportacion = _GastosSepelioLogic.ConsultaRpt();

            ReportDocument rpt = new ReportDocument();
            rpt.Load(Server.MapPath("~/Resources/reports/MA_Rpt_EcoCuoMor.rpt"));
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