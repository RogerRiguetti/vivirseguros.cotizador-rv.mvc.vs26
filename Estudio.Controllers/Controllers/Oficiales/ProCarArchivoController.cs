using System;
using Estudio.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using log4net;
using log4net.Config;
using System.Reflection;

namespace Estudio.Controllers.Controllers.Oficiales
{
    
    public class ProCarArchivoController : Controller
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ProCarArchivoLogic _ProCarArchivoLogic = new ProCarArchivoLogic();
        public ActionResult Index()
        {
            ViewBag.usuario = getUsuario();
            return View();
        }
        public string getUsuario()
        {
            try
            {
                string usuario = "";
                if (Convert.ToString(this.Session["Account"]).Length < 10)
                {
                    usuario = Convert.ToString(this.Session["Account"]);
                }
                else
                {
                    usuario = Convert.ToString(this.Session["Account"]).Substring(0, 10);
                }
                return usuario;
            }
            catch (Exception) { return null; }
        }

        public ActionResult cargarXML(string __doc, string archivo, string nombre, string us, string tipo, string fecha, string hora)
        {
            try
            {
                XmlConfigurator.Configure();
                _log.Info("Carga de Resultados");
                _log.Info("Comenzara a cargar el XML de Resultados");
                var resultado = _ProCarArchivoLogic.cargarXML(__doc, archivo, nombre, us, tipo, fecha, hora);

                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult CargarArchivoB(string numArch)
        {
            try
            {
                var resultado = _ProCarArchivoLogic.CargarArchivoB(numArch);
                GlobalVar.GlobalValue = resultado.Object;
                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult BuscarNumerosArchivo(string fecha)
        {
            try
            {
                var resultado = _ProCarArchivoLogic.BuscarNumerosArchivo(fecha);
                GlobalVar.GlobalValue = resultado.Object;
                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #region Métodos para reportes.

        /// <summary>
        /// José Hernández Alvarado.
        /// 24-10-2018
        /// Muestra reporte en pantalla con datos consultados desde BD.
        /// </summary>
        /// <returns></returns>
        public ActionResult RptResumen()
        {
            var resultExportacion = _ProCarArchivoLogic.RptResumen();
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_ProCarResRes.rpt"));
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

        /// <summary>
        /// José Hernández Alvarado.
        /// 25-10-2018
        /// Retorna resultado de consulta para existencia de registros para generar reporte.
        /// </summary>
        /// <returns></returns>
        public ActionResult validaReportes(string bandera)
        {
            try
            {
                var resultado = _ProCarArchivoLogic.validaReportes(bandera);

                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 29-10-2018
        /// Retorna respuesta para confirmar que existan registros para generar reporte (Solicitudes Perdidas por la Compañía).
        /// </summary>
        /// <returns></returns>
        public ActionResult validaRptPerdidasCia()
        {
            try
            {
                var resultado = _ProCarArchivoLogic.validaRptPerdidasCia();

                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 31-10-2018
        /// Retorna respuesta para confirmar que existan registros para generar reportes CIA (RECOTIZADAS, DESISTIDAS, CADUCADAS).
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidaRptsCIA(string bandera)
        {
            try
            {
                var resultado = _ProCarArchivoLogic.validaRptsCia(bandera);

                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult ValidaRptsAfp(string bandera)
        {
            try
            {
                var resultado = _ProCarArchivoLogic.validaRptsAfp(bandera);

                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 26-10-2018
        /// Retorna resultado de consulta para existencia de registros para generar reporte de solicitudes ganadas.
        /// </summary>
        /// <returns></returns>
        public ActionResult RptGanadas()
        {
            var resultExportacion = _ProCarArchivoLogic.RptGanadas();
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_SolGanadas.rpt"));
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

        /// <summary>
        /// José Hernández Alvarado.
        /// 29-10-2018
        /// Retorna resultado de consulta para existencia de registros para generar reporte (Solicitudes Perdidas por la Compañía CIA).
        /// </summary>
        /// <returns></returns>
        public ActionResult RptPerdidasCia()
        {
            var resultExportacion = _ProCarArchivoLogic.RptPerdidasCia();
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_SolPerdidasCia.rpt"));
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

        /// <summary>
        /// José Hernández Alvarado.
        /// 30-10-2018
        /// Retorna resultado de consulta para existencia de registros para generar reporte (Solicitudes Perdidas por la Compañía AFP).
        /// </summary>
        /// <returns></returns>
        public ActionResult RptPerdidasAfp()
        {
            var resultExportacion = _ProCarArchivoLogic.RptPerdidasAfp();
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_SolPerdidasAFP.rpt"));
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

        #region Reporte Recotizadas
        /// <summary>
        /// Morales Lizbeth 
        /// 01/11/2018
        /// </summary>
        /// <returns>Informacion para el reporte de Recotizadas CIA</returns>
        public ActionResult RptRecotizadasCia()
        {
            var resultExportacion = _ProCarArchivoLogic.RptOtrosCia("RE", "RECOTIZADAS");
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_SolOtrosCia.rpt"));
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

        /// <summary>
        /// Morales Lizbeth 
        /// 01/11/2018
        /// </summary>
        /// <returns>Informacion para el reporte de Recotizadas AFP</returns>
        public ActionResult RptRecotizadasAfp()
        {
            var resultExportacion = _ProCarArchivoLogic.RptOtrosAfp("RE", "RECOTIZADAS");
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_SolOtrosAFP.rpt"));
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

        #region Reporte Desistidas

        /// <summary>
        /// Morales Lizbeth 
        /// 01/11/2018
        /// </summary>
        /// <returns>Informacion para el reporte de Desistidas CIA</returns>
        public ActionResult RptDesistidasCia()
        {
            var resultExportacion = _ProCarArchivoLogic.RptOtrosCia("DE", "DESISTIDAS");
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_SolOtrosCia.rpt"));
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
        /// <summary>
        /// Morales Lizbeth 
        /// 01/11/2018
        /// </summary>
        /// <returns>Informacion para el reporte de Desistidas CIA</returns>
        public ActionResult RptDesistidasAfp()
        {
            var resultExportacion = _ProCarArchivoLogic.RptOtrosAfp("DE", "DESISTIDAS");
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_SolOtrosAfp.rpt"));
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

        #region Reporte Caducadas

        /// <summary>
        /// Morales Lizbeth 
        /// 01/11/2018
        /// </summary>
        /// <returns>Informacion para el reporte de Caducadas CIA</returns>
        public ActionResult RptCaducadasCia()
        {
            var resultExportacion = _ProCarArchivoLogic.RptOtrosCia("CA", "CADUCADAS");
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_SolOtrosCia.rpt"));
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


        /// <summary>
        /// Morales Lizbeth 
        /// 01/11/2018
        /// </summary>
        /// <returns>Informacion para el reporte de Caducadas AFP</returns>
        public ActionResult RptCaducadasAfp()
        {
            var resultExportacion = _ProCarArchivoLogic.RptOtrosAfp("CA", "CADUCADAS");
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_SolOtrosAfp.rpt"));
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


        #endregion

    }
}