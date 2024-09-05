using System;
using System.Web.Mvc;
using System.IO;
using log4net;
using System.Reflection;
using Estudio.Logic;

/// <summary>
/// Controlador para acciones de Jubilare
/// </summary>
/// 
/// <remarks>
/// Este controlador proporciona acciones para exportar datos en diferentes formatos.
///
/// Creado por: @wcdz
/// Fecha de creación: 2024-08-12
/// </remarks>
namespace Estudio.Controllers.Controllers
{
    public class JubilareController : Controller
    {
        //// GET: Jubilare
        //public ActionResult Index()
        //{
        //    return View();
        //}

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string GetNameFile()
        {
            string fileName = "CarteraJubilare";
            DateTime now = DateTime.Now;

            string formattedDate = now.ToString("yyyyMMdd");
            string formattedTime = now.ToString("HHmm");
            return $"{fileName}_{formattedDate}_{formattedTime}.xlsx";
        }

        [HttpGet]
        public ActionResult GetCarteraCompleta()
        {
            _log.Info("Inicia solicitud de GetCartera Completa");
            string folderPath = @"D:\ExportacionesJubilare";

            _log.Info("Verificando la existencia del directorio ExportacionesJubilare en disco D");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                _log.Info("Se crea el directorio ExportacionesJubilare en disco D");
            }

            string fileName = GetNameFile();
            string filePath = Path.Combine(folderPath, fileName);
            _log.Info("Se genera el filePath");
            JubilareExportData jubilareExportData = new JubilareExportData();
            _log.Info("Se hace llamado del service de jubilareExportData");
            jubilareExportData.ExportToExcelPagination(filePath);
            _log.Info("Fin de Solicitud de GetCartera Completa");
            return File(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        /*
           Test
         */
        public ActionResult Test()
        {
            return Json("test", JsonRequestBehavior.AllowGet);
        }
    }
}