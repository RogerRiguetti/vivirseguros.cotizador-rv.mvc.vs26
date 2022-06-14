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
    public class MantenedorIPCController : Controller
    {
        MantenedorIPCLogic _MantenedorIPCLogic = new MantenedorIPCLogic();
        #region No Transaccionales
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-23
        /// </summary>
        /// <returns>retorna la vista</returns>
        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");
            return View();
        }
        #endregion
        #region Transaccionales
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-23
        /// </summary>
        /// <returns>retorna la informacion para cargar la tabla inicial</returns>
        public ActionResult CargarTabla()
        {
            try
            {
                var resultado = _MantenedorIPCLogic.CargarTabla();

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-24
        /// </summary>
        /// <param name="fecha">fecha de busqueda</param>
        /// <returns>retorna la informacion si existe</returns>
        public ActionResult Consulta(DateTime fecha)
        {
            try
            {
                var resultado = _MantenedorIPCLogic.Consulta(fecha);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        ///  Osvaldo Valdez Carrillo
        /// 2018-08-24
        /// </summary>
        /// <param name="fecha">fecha IPC</param>
        /// <param name="valorIPC">valor ipc</param>
        /// <param name="variacionIPC"> variacion ipc</param>
        /// <param name="codigo">codigo N</param>
        /// <param name="bandera">true o false</param>
        /// <returns>retorna si se pudo grabar o actualizar</returns>
        public ActionResult GrabarMantenedorIPC(DateTime fecha,decimal valorIPC, decimal variacionIPC, string codigo,Boolean bandera)
        {
            try
            {
                string usuario = Convert.ToString(this.Session["Account"]);
                var resultado = _MantenedorIPCLogic.GrabarMantenedorIPC(fecha, valorIPC, variacionIPC, codigo, usuario, bandera);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-24
        /// </summary>
        /// <param name="fecha">fecha IPC</param>
        /// <returns>retorna si se elimino el registro</returns>
        public ActionResult EliminarMantenedorIPC(DateTime fecha)
        {
            try
            {
                string usuario = Convert.ToString(this.Session["Account"]);
                var resultado = _MantenedorIPCLogic.EliminarMantenedorIPC(fecha);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        ///  Osvaldo Valdez Carrillo
        /// 2018-08-24
        /// </summary>
        /// <returns>genera el reporte</returns>
        public ActionResult Reporte()
        {
            var resultExportacion = _MantenedorIPCLogic.ConsultaRpt();

            ReportDocument rpt = new ReportDocument();
            rpt.Load(Server.MapPath("~/Resources/reports/MA_Rpt_EcoIPC.rpt"));
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