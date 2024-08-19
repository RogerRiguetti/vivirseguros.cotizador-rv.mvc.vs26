using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estudio.Repository.Helpers;
using System.Data;
using System.Data.SqlClient;
using Estudio.Repository;
using Newtonsoft.Json;
using System.IO;
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

        public string GetNameFile()
        {
            string fileName = "CarteraJubilare";
            DateTime now = DateTime.Now;

            string formattedDate = now.ToString("yyyyMMdd");
            string formattedTime = now.ToString("HHmm");
            return $"{fileName}_{formattedDate}_{formattedTime}.xlsx";
        }

        public ActionResult GetCarteraCompleta()
        {
            string folderPath = @"D:\ExportacionesJubilare";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = GetNameFile();
            string filePath = Path.Combine(folderPath, fileName);
            JubilareExportData jubilareExportData = new JubilareExportData();
            jubilareExportData.ExportToExcelPagination(filePath);
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