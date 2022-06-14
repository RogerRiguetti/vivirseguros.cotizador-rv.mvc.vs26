using CrystalDecisions.CrystalReports.Engine;
using Estudio.Logic;
using Estudio.Repository.Core.Domain;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class LimiteCotizacionInicialController : Controller
    {
        LimiteCotizacionInicialLogic _LimiteCotizacionInicialLogic = new LimiteCotizacionInicialLogic();
        static string mensaje;
        /// <summary>
        /// osvaldo valdez
        /// 30/08/2018
        /// </summary>
        /// <returns>retorna la vista al usuario</returns>
        public ActionResult Index()
        {
            // UploadRecordsToDataBase("201901290859.xlsx"); //prueba 1
            //UploadRecordsToDataBase("201901291201.XLSX"); //porcientos
            //UploadRecordsToDataBase("OficialesParámetros20190131093959.XLSX"); //completa 
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");
            ViewBag.Mensaje = mensaje;
            mensaje = "";
            ViewBag.Departamentos = new SelectList(_LimiteCotizacionInicialLogic.TipoDepartamento(), "ClaveDepartamento", "Elemento");
            ViewBag.TipoMoneda = new SelectList(_LimiteCotizacionInicialLogic.TiposMoneda(), "ClaveMoneda", "Elemento");
            ViewBag.Periodos = new SelectList("");
            ViewBag.RangoTasa = new SelectList(_LimiteCotizacionInicialLogic.RangosTasa(), "IdRangoTasa", "Elemento");
            DateTime thisDay = DateTime.Today;
            ViewBag.fecha = thisDay.ToString("yyyy-MM-dd");
            return View();
        }

        #region Transaccionales
        /// <summary>
        /// osvaldo valdez
        /// 30/08/2018
        /// </summary>
        /// <param name="vlMoneda">valor de la moneda</param>
        /// <param name="vlReajuste">valor del reajuste</param>
        /// <param name="departamento">valor del departamento</param>
        /// <returns>retorna la lista de periodos</returns>
        public ActionResult ListaPeriodos(string vlMoneda, int vlReajuste, int departamento)
        {
            try
            {
                var resultado = _LimiteCotizacionInicialLogic.ListaPeriodos(vlMoneda, vlReajuste, departamento);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// osvaldo valdez
        /// 30/08/2018
        /// </summary>
        /// <param name="fechaIni">fecha inicio</param>
        /// <param name="codMoneda">valor de moneda</param>
        /// <param name="reajuste">valor de reajuste</param>
        /// <param name="departamento">valor del departamento</param>
        /// <returns>retorna la informacion a mostrar en pantalla</returns>
        public ActionResult Consulta(DateTime fechaIni, string codMoneda, int reajuste, int departamento)
        {
            try
            {
                var resultado = _LimiteCotizacionInicialLogic.Consulta(fechaIni, codMoneda, reajuste, departamento);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// osvaldo valdez
        /// 30/08/2018
        /// </summary>
        /// <param name="informacion">informacion recolectada de la pantalla</param>
        /// <param name="fechaIni">fecha inicio</param>
        /// <param name="bandera">bandera de grabado o actualizacion</param>
        /// <returns>reorna un mensaje en pantalla despues del proceso</returns>
        public ActionResult GrabarCorizacionInicial(LimiteCotizacionInicial informacion, DateTime fechaIni, Boolean bandera)
        {
            try
            {
                string usuario = Convert.ToString(this.Session["Account"]);
                var resultado = _LimiteCotizacionInicialLogic.Grabar(informacion, fechaIni, bandera, usuario);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// osvaldo valdez
        /// 30/08/2018
        /// </summary>
        /// <param name="fechaIni">fecha inicial</param>
        /// <param name="codMoneda">valor de la moneda</param>
        /// <param name="reajuste">valor del reajuste</param>
        /// <param name="departamento">valor del departamento</param>
        /// <returns>retorna un mensaje en pantalla despues del proceso</returns>
        public ActionResult EliminarCorizacionInicial(DateTime fechaIni, string codMoneda, int reajuste, string departamento)
        {
            try
            {
                string usuario = Convert.ToString(this.Session["Account"]);
                var resultado = _LimiteCotizacionInicialLogic.EliminarCorizacionInicial(fechaIni, codMoneda, reajuste, departamento, usuario);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// osvaldo valdez
        /// 31/08/2018
        /// </summary>
        /// <param name="codMoneda">codigo de moneda</param>
        /// <param name="reajuste">valor del reajuste</param>
        /// <param name="moneda">valor de la moneda</param>
        /// <param name="departamento">valor del departamento</param>
        /// <param name="nomDepartamento">nombre del departamento</param>
        /// <param name="fechaInicial">fecha inicial</param>
        /// <returns>retorna el reporte</returns>
        public ActionResult Reporte(string codMoneda, int reajuste,string moneda, string departamento,string nomDepartamento, DateTime fechaInicial)
        {//Informe de Parámetros de Tasa de Anclaje
              var resultExportacion = _LimiteCotizacionInicialLogic.ConsultaRpt(codMoneda, reajuste, moneda, departamento, nomDepartamento, fechaInicial);

              ReportDocument rpt = new ReportDocument();
              rpt.Load(Server.MapPath("~/Resources/reports/Pt_Rpt_ParCotizacionIni.rpt"));
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
        /// <summary>
        /// Omar Figueroa Flores
        /// 29-01-2019
        /// </summary>
        #region Carga Masiva de Excel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase file, string cmbxTipoRenta)
        {
            if(cmbxTipoRenta == "")
            {
                return RedirectToAction("Index");
            }
            else if (file != null && cmbxTipoRenta != "")
            {
                if (!file.FileName.EndsWith(".xls") && !file.FileName.EndsWith(".xlsx") && !file.FileName.EndsWith(".XLS") && !file.FileName.EndsWith(".XLSX"))
                    return View();

                var fileName = file.FileName.Split('.')[0] + DateTime.Now.ToString("yyyyMMddHHmmss.") + file.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Last();
                SaveFile(file, fileName);
                mensaje = UploadRecordsToDataBase(fileName, cmbxTipoRenta);
                return RedirectToAction("Index");
            }

            return View();

        }

        private void SaveFile(HttpPostedFileBase file, string fileName)
        {
            var path = System.IO.Path.Combine(Server.MapPath("~/Files/"), fileName);
            var data = new byte[file.ContentLength];
            file.InputStream.Read(data, 0, file.ContentLength);

            using (var sw = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                sw.Write(data, 0, data.Length);
            }
        }
        private string UploadRecordsToDataBase(string fileName, string tipoRenta)
        {
            string usuario = Convert.ToString(this.Session["Account"]);
            using (var stream = System.IO.File.Open(Path.Combine(Server.MapPath("~/Files/"), fileName), FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    int rowCount = reader.RowCount;
                    string[] regiones = { "LIMA", "AMAZONAS", "ANCASH", "APURIMAC", "AREQUIPA", "AYACUCHO", "CAJAMARCA", "CALLAO", "CUSCO", "HUANCAVELICA", "HUANUCO", "ICA", "JUNIN", "LA LIBERTAD", "LAMBAYEQUE", "MADRE DE DIOS", "MOQUEGUA", "PASCO", "PIURA", "PUNO", "SAN MARTIN", "TACNA", "TUMBES", "UCAYALI", "EXTRANJERO", "LORETO" };
                    int[] colIni = { 2, 9, 16, 23, 30, 37, 44, 51, 58, 65, 72, 79, 86, 93, 100, 107, 114, 121, 128, 135, 142, 149, 156, 163, 170, 177 };
                    List<LimiteCotizacionInicial> listDatos = new List<LimiteCotizacionInicial>();

                    for (int i = 0; i < regiones.Length; i++)
                    {
                        List<LimiteCotizacionInicial> codReg = _LimiteCotizacionInicialLogic.getCodRegion(regiones[i]);
                        if (codReg.Count != 0)
                        {
                            obtenerInfoRegion(listDatos, reader, rowCount, codReg, colIni[i], regiones[i]);
                        }
                    }
                    return _LimiteCotizacionInicialLogic.gurdarDatosMasivos(listDatos, usuario, tipoRenta).Message;
                }
            }
        }

        public void obtenerInfoRegion(List<LimiteCotizacionInicial> listDatos, IExcelDataReader xlRange, int rowCount, List<LimiteCotizacionInicial> codReg, int colIni, string lugar)
        {
            try
            {
                var result = xlRange.AsDataSet();

                // Ejemplos de acceso a datos
                var table = result.Tables[0];
                for (int i = 4; i <= rowCount; i++) //renglon
                {
                    var row = table.Rows[i];
                    LimiteCotizacionInicial dato = new LimiteCotizacionInicial();
                    dato.lugar = lugar;
                    dato.moneda = (row[1].ToString());

                    if (i >= 4 && i <= 7) { definirReg(dato, codReg, "[0-50>", 0); }
                    if (i >= 8 && i <= 11) { definirReg(dato, codReg, "[50-100>", 1); }
                    if (i >= 12 && i <= 15) { definirReg(dato, codReg, "[100-150>", 2); }
                    if (i >= 16 && i <= 19) { definirReg(dato, codReg, "[150-300>", 3); }
                    if (i >= 20 && i <= 23) { definirReg(dato, codReg, "[300-500>", 4); }
                    if (i >= 24 && i <= 27) { definirReg(dato, codReg, "[500-…>", 5); }

                    if (row != null)
                    {
                        dato.it = row[colIni].ToString() == "" ? 0.00 : Convert.ToDouble((Convert.ToDouble(row[colIni]) * 100).ToString("0.00"));
                        dato.ip = row[colIni + 1].ToString() == "" ? 0.00 : Convert.ToDouble((Convert.ToDouble(row[colIni + 1]) * 100).ToString("0.00"));
                        dato.s = row[colIni + 2].ToString() == "" ? 0.00 : Convert.ToDouble((Convert.ToDouble(row[colIni + 2]) * 100).ToString("0.00"));
                        dato.ja = row[colIni + 3].ToString() == "" ? 0.00 : Convert.ToDouble((Convert.ToDouble(row[colIni + 3]) * 100).ToString("0.00"));
                        dato.jl = row[colIni + 4].ToString() == "" ? 0.00 : Convert.ToDouble((Convert.ToDouble(row[colIni + 4]) * 100).ToString("0.00"));
                        dato.tir = row[colIni + 5].ToString() == "" ? 0.00 : Convert.ToDouble((Convert.ToDouble(row[colIni + 5]) * 100).ToString("0.00"));
                        dato.prd = row[colIni + 6].ToString() == "" ? 0.00 : Convert.ToDouble((Convert.ToDouble(row[colIni + 6]) * 100).ToString("0.00"));
                    }


                    listDatos.Add(dato);
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        public void definirReg(LimiteCotizacionInicial dato, List<LimiteCotizacionInicial> codReg, string cic, int indice)
        {
            dato.cic = cic;
            dato.codRegion = codReg[indice].codRegion.Length == 1 ? "0" + codReg[indice].codRegion : codReg[indice].codRegion;
            dato.accion = codReg[indice].FechaFinal;
        }
        #endregion
    }
}