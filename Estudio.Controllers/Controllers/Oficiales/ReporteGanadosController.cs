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
    public class ReporteGanadosController : Controller
    {
        ReporteGanadosLogic _reporteGanadosLogic = new ReporteGanadosLogic();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        // GET: ReporteGanados
        public ActionResult Index()
        {
           // List<Departamento> departamentos = _reporteGanadosLogic.Departamentos();
            //int idDepartamento = 0;
            //idDepartamento = (from d in departamentos where d.Elemento == "LIMA" select d.IdDepartamento).First();
            //ViewBag.Departamentos = new SelectList(departamentos, "IdDepartamento", "Elemento", idDepartamento);

            ViewBag.Pensiones = new SelectList(_reporteGanadosLogic.Pensiones(), "Clave", "Elemento");
            ViewBag.Monedas = new SelectList(_reporteGanadosLogic.Monedas(), "CodigoMoneda", "Elemento");
            ViewBag.TiposRenta = new SelectList(_reporteGanadosLogic.TiposRenta(), "Clave", "Elemento");
            //ViewBag.buckets = new SelectList(_reporteGanadosLogic.TiposRenta(), "Clave", "Elemento");

            return View();
        }

        [HttpPost]
        public JsonResult BusquedaInformacion(FiltrosTotalesReportesGanados filtros)
        {
            try
            {
                var resultado = _reporteGanadosLogic.BusquedaInformacion(filtros);
               
                return Json(resultado);
            }
            catch (Exception ex)
            {
                Response obj = new Response();
                obj.IsOk = false;
                obj.Message = "Ocurrió un error al Buscar los valores";
                return Json(obj);
            }
        }

        [HttpPost]
        public JsonResult CrearExcelReportesGanados(FiltrosTotalesReportesGanados filtros)
        {
            Response res = new Response();
            try
            {
                XmlConfigurator.Configure();

                _log.Info("Se comenzará a generar el Excel, por favor espere...");
                Random r = new Random();
                int aleatorio3 = r.Next(100, 999);
                var date = DateTime.Today;
                string nombreArch = "ReporteGanados - " + date.ToString("yyyyMMdd") + "_" + aleatorio3.ToString() + ".xlsx";
                string pathFile = Server.MapPath("\\Files\\") + nombreArch;

                System.IO.File.Copy(Server.MapPath("\\Files\\ReporteGanados.xlsx"), pathFile);

                List<ReporteGanados> ListReporteGanados = new List<ReporteGanados>();
                ListReporteGanados = _reporteGanadosLogic.BusquedaInformacionExcel(filtros);

                SLDocument sl = new SLDocument(pathFile);

                sl.SelectWorksheet("ReporteGanados");
                for (int i = 0; i < ListReporteGanados.Count; i++)
                {
                    sl.SetCellValue(2 + i, 1, ListReporteGanados[i].CompañiaSegurosVitalicios);
                    sl.SetCellValue(2 + i, 2, ListReporteGanados[i].NumeroCotizado);
                    sl.SetCellValue(2 + i, 3, ListReporteGanados[i].Pension);
                    sl.SetCellValue(2 + i, 4, ListReporteGanados[i].TasaVenta);
                    sl.SetCellValue(2 + i, 5, ListReporteGanados[i].DiferenciaPension);
                    sl.SetCellValue(2 + i, 6, ListReporteGanados[i].DiferenciaTasaVenta);
                    sl.SetCellValue(2 + i, 7, ListReporteGanados[i].NumeroCasosGanados);
                    sl.SetCellValue(2 + i, 8, ListReporteGanados[i].NumeroCasosTotales);
                    sl.SetCellValue(2 + i, 9, ListReporteGanados[i].ParticipacionMercado);
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