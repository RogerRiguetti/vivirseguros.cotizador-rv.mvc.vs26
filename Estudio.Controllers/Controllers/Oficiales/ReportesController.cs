using Estudio.Logic;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using log4net;
using log4net.Config;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class ReportesController : Controller
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ReportesLogic _reportesLogic = new ReportesLogic();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GenerarReporteGanados(string FechaDesde, string FechaHasta, string parametroRV, string parametroRP)
        {
            Response res = new Response();
            try
            {
                XmlConfigurator.Configure();

                _log.Info("Se comenzará a generar el Excel, por favor espere...");
                Random r = new Random();
                int aleatorio3 = r.Next(100, 999);
                var date = DateTime.Today;
                string nombreArch = "Reporte Ganados - " + date.ToString("yyyyMMdd") + "_" + aleatorio3.ToString() + ".xlsx";
                string pathFile = Server.MapPath("\\Files\\") + nombreArch;

                System.IO.File.Copy(Server.MapPath("\\Files\\Reporte Ganados Plantilla.xlsx"), pathFile);

                List<ReporteCasosGanadosRV> ListReporteGanadosRV = new List<ReporteCasosGanadosRV>();
                ListReporteGanadosRV = _reportesLogic.GenerarReporteGanadosRV(FechaDesde, FechaHasta, parametroRV);

                List<ReporteCasosGanadosRP> ListReporteGanadosRP = new List<ReporteCasosGanadosRP>();
                ListReporteGanadosRP = _reportesLogic.GenerarReporteGanadosRP(FechaDesde, FechaHasta, parametroRP);

                SLDocument sl = new SLDocument(pathFile);

                sl.SelectWorksheet("RV");
                sl.SetCellValue(1, 5, FechaDesde);
                sl.SetCellValue(1, 7, FechaHasta);
                for (int i = 0; i < ListReporteGanadosRV.Count; i++)
                {
                    sl.SetCellValue(4 + i, 1, i+1);
                    sl.SetCellValue(4 + i, 2, ListReporteGanadosRV[i].num_poliza);
                    sl.SetCellValue(4 + i, 3, ListReporteGanadosRV[i].cuspp);
                    sl.SetCellValue(4 + i, 4, ListReporteGanadosRV[i].fechaAdjudicacion);
                    sl.SetCellValue(4 + i, 5, ListReporteGanadosRV[i].fechaTransferencia);
                    sl.SetCellValue(4 + i, 6, ListReporteGanadosRV[i].fechaCotizacion);
                    sl.SetCellValue(4 + i, 7, ListReporteGanadosRV[i].comisionAsesorCotizado);
                    sl.SetCellValue(4 + i, 8, ListReporteGanadosRV[i].comisionSupervisorCotizado);
                    sl.SetCellValue(4 + i, 9, ListReporteGanadosRV[i].prestacion);
                    sl.SetCellValue(4 + i, 10, ListReporteGanadosRV[i].modalidad);
                    sl.SetCellValue(4 + i, 11, ListReporteGanadosRV[i].moneda);
                    sl.SetCellValue(4 + i, 12, ListReporteGanadosRV[i].añosDiferidos);
                    sl.SetCellValue(4 + i, 13, ListReporteGanadosRV[i].añosGarantizados);
                    sl.SetCellValue(4 + i, 14, ListReporteGanadosRV[i].primerTramo);
                    sl.SetCellValue(4 + i, 15, ListReporteGanadosRV[i].segundoTramo);
                    sl.SetCellValue(4 + i, 16, ListReporteGanadosRV[i].gratificacion);
                    sl.SetCellValue(4 + i, 17, ListReporteGanadosRV[i].cobertura);
                    sl.SetCellValue(4 + i, 18, ListReporteGanadosRV[i].cicCotizacion);
                    sl.SetCellValue(4 + i, 19, ListReporteGanadosRV[i].primaCotizacion);
                    sl.SetCellValue(4 + i, 20, ListReporteGanadosRV[i].rentaCotizacion);
                    sl.SetCellValue(4 + i, 21, ListReporteGanadosRV[i].tasaAFP);
                    sl.SetCellValue(4 + i, 22, ListReporteGanadosRV[i].tcCotizacion);
                    sl.SetCellValue(4 + i, 23, ListReporteGanadosRV[i].tv);
                    sl.SetCellValue(4 + i, 24, ListReporteGanadosRV[i].tir);
                    sl.SetCellValue(4 + i, 25, ListReporteGanadosRV[i].perdida);
                    sl.SetCellValue(4 + i, 26, ListReporteGanadosRV[i].tce);
                    sl.SetCellValue(4 + i, 27, ListReporteGanadosRV[i].tlr);
                    sl.SetCellValue(4 + i, 28, ListReporteGanadosRV[i].tasaMercado);
                    sl.SetCellValue(4 + i, 29, ListReporteGanadosRV[i].duracion);
                    sl.SetCellValue(4 + i, 30, ListReporteGanadosRV[i].spread);
                    sl.SetCellValue(4 + i, 31, ListReporteGanadosRV[i].puesto);
                    sl.SetCellValue(4 + i, 32, ListReporteGanadosRV[i].mejora);
                    sl.SetCellValue(4 + i, 33, ListReporteGanadosRV[i].nombreAfiliado);
                    sl.SetCellValue(4 + i, 34, ListReporteGanadosRV[i].nombreAsesor);
                    sl.SetCellValue(4 + i, 35, ListReporteGanadosRV[i].nombreSupervisor);
                    sl.SetCellValue(4 + i, 36, ListReporteGanadosRV[i].Departamento);
                    sl.SetCellValue(4 + i, 37, ListReporteGanadosRV[i].cicRealSoles);
                    sl.SetCellValue(4 + i, 38, ListReporteGanadosRV[i].primaCSV);
                    sl.SetCellValue(4 + i, 39, ListReporteGanadosRV[i].rentaRealMoneda);
                    sl.SetCellValue(4 + i, 40, ListReporteGanadosRV[i].rentaReferencia);
                    sl.SetCellValue(4 + i, 41, ListReporteGanadosRV[i].rentaReferenciaActualizada);
                    sl.SetCellValue(4 + i, 42, ListReporteGanadosRV[i].ComisionAsesorReal);
                    sl.SetCellValue(4 + i, 43, ListReporteGanadosRV[i].ComisionSupervisorReal);
                    sl.SetCellValue(4 + i, 44, ListReporteGanadosRV[i].parrilla);
                }
                sl.SelectWorksheet("RP");
                sl.SetCellValue(1, 4, FechaDesde);
                sl.SetCellValue(1, 6, FechaHasta);
                for (int i = 0; i < ListReporteGanadosRP.Count; i++)
                {
                    sl.SetCellValue(4 + i, 2, ListReporteGanadosRP[i].num_poliza);
                    sl.SetCellValue(4 + i, 3, ListReporteGanadosRP[i].fechaAbono);
                    sl.SetCellValue(4 + i, 4, ListReporteGanadosRP[i].comisionAsesorCotizado);
                    sl.SetCellValue(4 + i, 5, ListReporteGanadosRP[i].comisionSupervisorCotizado);
                    sl.SetCellValue(4 + i, 6, ListReporteGanadosRP[i].moneda);
                    sl.SetCellValue(4 + i, 7, ListReporteGanadosRP[i].añosPlazo);
                    sl.SetCellValue(4 + i, 8, ListReporteGanadosRP[i].añosDiferido);
                    sl.SetCellValue(4 + i, 9, ListReporteGanadosRP[i].primaMoneda);
                    sl.SetCellValue(4 + i, 10, ListReporteGanadosRP[i].rentaMoneda);
                    sl.SetCellValue(4 + i, 11, ListReporteGanadosRP[i].tv);
                    sl.SetCellValue(4 + i, 12, ListReporteGanadosRP[i].tir);
                    sl.SetCellValue(4 + i, 13, ListReporteGanadosRP[i].perdida);
                    sl.SetCellValue(4 + i, 14, ListReporteGanadosRP[i].tasaInversion);
                    sl.SetCellValue(4 + i, 15, ListReporteGanadosRP[i].duracion);
                    sl.SetCellValue(4 + i, 16, ListReporteGanadosRP[i].spread);
                    sl.SetCellValue(4 + i, 17, ListReporteGanadosRP[i].mejora);
                    sl.SetCellValue(4 + i, 18, ListReporteGanadosRP[i].nombreAsegurado);
                    sl.SetCellValue(4 + i, 19, ListReporteGanadosRP[i].nombreAsesor);
                    sl.SetCellValue(4 + i, 20, ListReporteGanadosRP[i].nombreSupervisor);
                    sl.SetCellValue(4 + i, 21, ListReporteGanadosRP[i].departamento);
                    sl.SetCellValue(4 + i, 22, ListReporteGanadosRP[i].comisionAsesorReal);
                    sl.SetCellValue(4 + i, 23, ListReporteGanadosRP[i].comisionSupervisorReal);
                    sl.SetCellValue(4 + i, 24, ListReporteGanadosRP[i].parrilla);
                }
                sl.SaveAs(pathFile);

                res.IsOk = true;
                res.Object = nombreArch;

                return Json(res);
            }
            catch (Exception ex)
            {
                _log.Info("Error en Generación del Excel(Controller), favor de verificar: " + ex.Message);

                res.IsOk = false;
                res.Message = ex.Message;

                return Json(res);
            }
        }

        public ActionResult ExportarReporteGanados(string nombreArchivo)
        {
            XmlConfigurator.Configure();

            try
            {
                string pathFile = Server.MapPath("\\Files\\") + nombreArchivo;

                byte[] fileBytes = System.IO.File.ReadAllBytes(pathFile);
                System.IO.File.Delete(pathFile);

                _log.Info("Datos insertados al Excel exitosamente, se procederá a exportar...");
                //pathFileExcel = pathFile;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
            }
            catch (Exception ex)
            {
                _log.Info("Error en Generación del Excel(Controller), favor de verificar: " + ex.Message);
                return null;
            }
        }

    }
}