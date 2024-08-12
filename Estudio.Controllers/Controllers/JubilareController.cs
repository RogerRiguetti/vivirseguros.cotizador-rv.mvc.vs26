using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        // GET: Jubilare
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExportData()
        {
            // Nomencatura del nombre del archivo
            // CarteraJubilare_2024_08_12_15_48.xlsx
            string fileName = "CarteraJubilare";
            DateTime now = DateTime.Now;

            string formattedDate = now.ToString("yyyy_MM_dd");
            string formattedTime = now.ToString("HH_mm_ss");
            string fileNameWithDateTime = $"{fileName}_{formattedDate}_{formattedTime}.xlsx";

            string getDataStoredProcedure = GetDataStoredProcedure("usp_Jub_Sel_CarteraCompleta");

            Console.WriteLine(fileNameWithDateTime);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        protected string GetDataStoredProcedure(string nameStoredProcedure)
        {
            string script = $"EXECUTE {nameStoredProcedure}";
            // Determinar el ejecutable para conseguir la data
            //VCEDBContext<DataTable>.CallSelectStatementDt(script, x => new DataTable());
            return script;            
        }



    }
}