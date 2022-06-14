using Estudio.Repository.Core.Domain;
using log4net;
using System.Reflection;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estudio.Logic;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class ReportesReservasController : Controller
    {
        //Globales
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ReportesReservasLogic _reportesReservasLogic = new ReportesReservasLogic();

        // GET: ReportesReservas
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Retornar archivo para descarga.
        /// José Hernández Alvarado
        /// 28-10-2019
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha de periodo a buscar</param>
        /// <returns>Archivo</returns>
        public ActionResult ExportarFlujosPasivos(string pFechaPeriodo)
        {
            XmlConfigurator.Configure();
            try
            {
                _log.Info("Se generará el Reporte de Flujos de Pasivos.");

                Random r = new Random();
                int numeroAleatorio = r.Next(100, 999);
                var FechaActual = DateTime.Today;
                string strNombreArchivo = "Flujos de Pasivos - " + FechaActual.ToString("yyyyMMdd") + "_" + numeroAleatorio.ToString() + ".xlsx";
                string strPathFile = Server.MapPath("\\Files\\") + strNombreArchivo;

                System.IO.File.Copy(Server.MapPath("\\Files\\Modelo Flujos de Pasivos.xlsx"), strPathFile);

                string auxPathFile = _reportesReservasLogic.ExportarReporteFlujosPasivos(pFechaPeriodo, strPathFile, "S");

                byte[] fileBytes = System.IO.File.ReadAllBytes(auxPathFile);
                System.IO.File.Delete(auxPathFile);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
            }
            catch (Exception ex)
            {
                _log.Info("Error en Reporte de Flujos de Pasivos: " + ex.Message);
                return null;
            }
        }

        public ActionResult ReporteSbs(string pFechaPeriodo)
        {
            XmlConfigurator.Configure();
            try
            {
                _log.Info("Se generará el Reporte Base SBS.");

                Random r = new Random();
                int numeroAleatorio = r.Next(100, 999);
                var FechaActual = DateTime.Today;
                string strNombreArchivo = "Base SBS -" + FechaActual.ToString("yyyyMMdd") + "_" + numeroAleatorio.ToString() + ".xlsx";
                string strPathFile = Server.MapPath("\\Files\\") + strNombreArchivo;

                System.IO.File.Copy(Server.MapPath("\\Files\\Base SBS.xlsx"), strPathFile);

                string auxPathFile = _reportesReservasLogic.ExportarReporteSbs(pFechaPeriodo, strPathFile);

                byte[] fileBytes = System.IO.File.ReadAllBytes(auxPathFile);
                System.IO.File.Delete(auxPathFile);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
            }
            catch (Exception ex)
            {
                _log.Info("Error en Reporte SBS: " + ex.Message);
                return null;
            }
        }

        public ActionResult ArchivoTxt(string pFechaPeriodo, string pTipoArchivo)
        {
            XmlConfigurator.Configure();
            try
            {
                _log.Info("Se generará archivo de texto de Flujos de Pasivos.");

                string strNombreArchivo = "02" + pFechaPeriodo.Substring(2, 6) + ".092";
                string strPathFile = Server.MapPath("\\Files\\") + strNombreArchivo;

                string auxPathFile = _reportesReservasLogic.GenerarArchivoTxt(pFechaPeriodo, strPathFile, pTipoArchivo, "S");

                byte[] fileBytes = System.IO.File.ReadAllBytes(auxPathFile);
                System.IO.File.Delete(auxPathFile);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
            }
            catch (Exception ex)
            {
                _log.Info("Error en archivo de texto: " + ex.Message);
                return null;
            }
        }

        public ActionResult ReporteResumenReservas(string pFechaPeriodo)
        {
            XmlConfigurator.Configure();
            try
            {
                _log.Info("Se generará el Reporte de Resumen de Reservas.");

                Random r = new Random();
                int numeroAleatorio = r.Next(100, 999);
                var FechaActual = DateTime.Today;
                string strNombreArchivo = "Resumen_Reservas - " + FechaActual.ToString("yyyyMMdd") + "_" + numeroAleatorio.ToString() + ".xlsx";
                string strPathFile = Server.MapPath("\\Files\\") + strNombreArchivo;

                System.IO.File.Copy(Server.MapPath("\\Files\\Modelo Resumen Reservas.xlsx"), strPathFile);

                string auxPathFile = _reportesReservasLogic.ExportarReporteResumenReservas(pFechaPeriodo, strPathFile, "S");

                byte[] fileBytes = System.IO.File.ReadAllBytes(auxPathFile);
                System.IO.File.Delete(auxPathFile);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
            }
            catch (Exception ex)
            {
                _log.Info("Error en Reporte Resumen Reservas: " + ex.Message);
                return null;
            }
        }

        public ActionResult ReporteResumenAdecuacion(string pFechaPeriodo)
        {
            XmlConfigurator.Configure();
            try
            {
                _log.Info("Se generará el Reporte de Resumen Adecuación.");

                Random r = new Random();
                int numeroAleatorio = r.Next(100, 999);
                var FechaActual = DateTime.Today;
                string strNombreArchivo = "Resumen Adecuacion - " + FechaActual.ToString("yyyyMMdd") + "_" + numeroAleatorio.ToString() + ".xlsx";
                string strPathFile = Server.MapPath("\\Files\\") + strNombreArchivo;

                System.IO.File.Copy(Server.MapPath("\\Files\\Resumen_Adecuacion.xlsx"), strPathFile);

                string auxPathFile = _reportesReservasLogic.ExportarReporteResumenAdecuacion(pFechaPeriodo, strPathFile, "S");

                byte[] fileBytes = System.IO.File.ReadAllBytes(auxPathFile);
                System.IO.File.Delete(auxPathFile);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
            }
            catch (Exception ex)
            {
                _log.Info("Error en Reporte Resumen Adecuación: " + ex.Message);
                return null;
            }
        }
    }
}