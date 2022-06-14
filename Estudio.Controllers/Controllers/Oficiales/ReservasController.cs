using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estudio.Logic;
using Estudio.Repository.Core.Domain;
using System.Threading;
using System.IO;
using ExcelDataReader;
using Estudio.Repository.Persistence.Repositories;
using System.Threading.Tasks;
using System.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Estudio.Repository.Helpers;
using log4net;
using System.Reflection;
using log4net.Config;
using SpreadsheetLight;
using Estudio.Process;
using System.Globalization;
using System.IO.Compression;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class ReservasController : Controller
    {
        //Globales
        ReservasLogic _ReservasLogic = new ReservasLogic();
        ReportesReservasLogic _reportesReservasLogic = new ReportesReservasLogic();
        ReservasRepository _ReservasRepository = new ReservasRepository();
        RutinaFlujos _rutinaFlujos = new RutinaFlujos();
        List<string> listLogRutinaFlu = new List<string>();
        static string mensaje;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region NO TRANSACCIONALES
        public ActionResult Index()
        {
            string res = Convert.ToString(this.Session["encryptedTicket"]);
            if (String.IsNullOrEmpty(res))
                return RedirectToAction("Login", "Estudio");

            ViewBag.mensaje = mensaje;
            mensaje = "";

            return View();
        }

        public ActionResult Periodo(string fecha)
        {
            var resultado = _ReservasLogic.PeriodoLogic(fecha);
            return Json(resultado);
        }

        public ActionResult UPeriodo()
        {
            var resultado = _ReservasLogic.ConsultarPeriodoAbierto();
            return Json(resultado);
        }
        #endregion

        #region TRANSACCIONALES
        public ActionResult IniciarCalculo(string fecha)
        {
            string usuario = this.Session["Account"].ToString();
            var resultado = _ReservasLogic.IniCal(fecha, usuario);
            return Json(resultado);
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 05-03-2019
        /// Método para validar que haya registros en BD para proceder al Cálculo de Flujos de Carteras.
        /// </summary>
        /// <returns>Respuesta de validación.</returns>
        public ActionResult Valida_CalculoFlujos()
        {
            var resultado = _ReservasLogic.Valida_CalculoFlujos();
            return Json(resultado);
        }

        public ActionResult Valida_CurvaTasas(string FecCal)
        {
            var resultado = _ReservasLogic.Valida_CurvaTasas(FecCal);
            return Json(resultado);
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 05-03-2019
        /// Método para retornar respuesta de Cálculo de Flujos de Carteras.
        /// </summary>
        /// <param name="FecCal">Fecha de Periodo ingresada en pantalla.</param>
        /// <returns></returns>
        public async Task<ActionResult> Calculo_Flujos(string FecCal)
        {
            XmlConfigurator.Configure();
            string pathLog = Server.MapPath("\\") + "slnVidaCamara.log";

            Response resultado = await _ReservasLogic.Calculo_FlujoCartera(FecCal, pathLog);

            resultado.Message = resultado.Message;

            return Json(resultado);
        }

        public async Task<ActionResult> CalculoReserva(string fecha)
        {
            var resultado = await _ReservasLogic.CalculoReservasLogic(fecha);

            return Json(resultado);
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 20-02-2019
        /// Método reutilizado para descargar documento Excel en el formato requerido.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportarReservas()
        {
            XmlConfigurator.Configure();
            try
            {
                #region Generar Excel con plantilla
                _log.Info("Se generará el Excel de Proceso de Migración, espere por favor...");
                Random r = new Random();
                int aleatorio3 = r.Next(100, 999);
                var date = DateTime.Today;
                string nombreArch = "Reservas - " + date.ToString("yyyyMMdd") + "_" + aleatorio3.ToString() + ".xlsx";

                string pathFile = _ReservasLogic.RutaArchivoMigracion();
                _log.Info("Ruta Migracion " + pathFile);
                string pathFileArchivo = pathFile + nombreArch;

                System.IO.File.Copy(pathFile, pathFileArchivo);

                List<Reservas> ListResMigracion = new List<Reservas>();
                ListResMigracion = _ReservasLogic.ReservasMigracion();
                DateTime vlFecFall; 

                SLDocument sl = new SLDocument(pathFileArchivo);
                _log.Info("Se comenzará a llenar el Excel.");
                sl.SelectWorksheet("Reservas");
                for (int i = 0; i < ListResMigracion.Count; i++)
                {
                    sl.SetCellValue(2 + i, 1, Convert.ToInt32(ListResMigracion[i].Pol_NumPol));
                    sl.SetCellValue(2 + i, 2, ListResMigracion[i].Pol_CUSPP);
                    sl.SetCellValue(2 + i, 3, ListResMigracion[i].Pol_Prestacion);
                    sl.SetCellValue(2 + i, 4, ListResMigracion[i].Pol_CodMod);
                    sl.SetCellValue(2 + i, 5, Convert.ToDateTime(ListResMigracion[i].Pol_FecVig));
                    sl.SetCellValue(2 + i, 6, Convert.ToDateTime(ListResMigracion[i].Pol_FecDev));
                    sl.SetCellValue(2 + i, 7, ListResMigracion[i].Pol_MesesDif);
                    sl.SetCellValue(2 + i, 8, ListResMigracion[i].Pol_MesesGar);
                    sl.SetCellValue(2 + i, 9, ListResMigracion[i].Pol_MesesEsc);
                    sl.SetCellValue(2 + i, 10, ListResMigracion[i].Pol_PrcRentaEsc);
                    sl.SetCellValue(2 + i, 11, ListResMigracion[i].Pol_CodGratif);
                    sl.SetCellValue(2 + i, 12, ListResMigracion[i].Ben_CodSitInv);
                    sl.SetCellValue(2 + i, 13, Convert.ToDateTime(ListResMigracion[i].Ben_FecNac));
                    sl.SetCellValue(2 + i, 14, ListResMigracion[i].Ben_CodSexo);
                    sl.SetCellValue(2 + i, 15, ListResMigracion[i].Ben_CodPar);
                    sl.SetCellValue(2 + i, 16, ListResMigracion[i].Ben_PrcPension);
                    sl.SetCellValue(2 + i, 17, ListResMigracion[i].Num_Estudiante);
                    sl.SetCellValue(2 + i, 18, ListResMigracion[i].Cod_Estudiante);
                    if (ListResMigracion[i].Ben_FecFal != "")
                    {
                        try
                        {
                            vlFecFall = Convert.ToDateTime(ListResMigracion[i].Ben_FecFal);
                            sl.SetCellValue(2 + i, 19, vlFecFall);
                        }
                        catch (Exception)
                        {
                            sl.SetCellValue(2 + i, 19, ListResMigracion[i].Ben_FecFal);
                        }
                    }
                    else
                    {
                        sl.SetCellValue(2 + i, 19, ListResMigracion[i].Ben_FecFal);
                    }
                    
                    sl.SetCellValue(2 + i, 20, ListResMigracion[i].Remuneracion_Ini);
                    sl.SetCellValue(2 + i, 21, ListResMigracion[i].Remuneracion_Ajus);
                    sl.SetCellValue(2 + i, 22, ListResMigracion[i].Pension_Ajus);
                    sl.SetCellValue(2 + i, 23, ListResMigracion[i].Pol_MtoPrima);
                    sl.SetCellValue(2 + i, 24, ListResMigracion[i].Pol_TasaVta);
                    sl.SetCellValue(2 + i, 25, ListResMigracion[i].Pol_TasaLR);
                    sl.SetCellValue(2 + i, 26, ListResMigracion[i].Tasa_VtaProm);
                    sl.SetCellValue(2 + i, 27, ListResMigracion[i].Tasa_Equiv);
                }

                sl.SaveAs(pathFileArchivo);



                byte[] fileBytes = System.IO.File.ReadAllBytes(pathFileArchivo);
                System.IO.File.Delete(pathFileArchivo);

                _log.Info("El Excel de Proceso de Migración se ha generado correctamente.");
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArch);
                #endregion
            }
            catch (Exception ex)
            {
                _log.Info("Error Excel - Proceso de Migración: " + ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        #region Carga de Excel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (!file.FileName.EndsWith(".xls") && !file.FileName.EndsWith(".xlsx") && !file.FileName.EndsWith(".XLS") && !file.FileName.EndsWith(".XLSX"))
                    return View();

                var fileName = file.FileName.Split('.')[0] + DateTime.Now.ToString("yyyyMMddHHmmss.") + file.FileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Last();
                SaveFile(file, fileName);
                mensaje = UploadRecordsToDataBase(fileName);
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

        private string UploadRecordsToDataBase(string fileName)
        {
            XmlConfigurator.Configure();
            _log.Info("****************** CARGA DE EXCEL PARA ACTUALIZACIÓN DE DATOS *******************");
            string errores = "";
            int contErrores = 0;
            int contPol = 0;
            string polstr = "";
            try
            {
                string usuario = Convert.ToString(this.Session["Account"]);

                using (var stream = System.IO.File.Open(Path.Combine(Server.MapPath("~/Files/"), fileName), FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        List<Reservas> listDatos = new List<Reservas>();

                        var result = reader.AsDataSet();
                        for (int i = 1; i <= reader.RowCount - 1; i++) //renglon
                        {
                            var table = result.Tables[0];
                            var row = table.Rows[i];
                            polstr = "";

                            if (row[0].ToString() != "")
                            {
                                Reservas dato = new Reservas();

                                #region Lectura de renglones

                                contPol = row[0].ToString().Trim().Length;
                                for (int p = 1; p <= (10 - contPol); p++)
                                {
                                    polstr += "0";
                                }
                                dato.Pol_NumPol = polstr + row[0].ToString().Trim(); 
                                dato.Pol_CUSPP = row[1].ToString() == "" ? "" : row[1].ToString().Trim();
                                dato.Pol_CodTipPen = row[2].ToString() == "" ? "" : row[2].ToString().Trim();
                                dato.Pol_CodMod = row[3].ToString() == "" ? "" : row[3].ToString().Trim();
                                dato.Pol_FecVig = row[4].ToString() == "" ? "" : row[4].ToString().Trim();
                                dato.Pol_FecDev = row[5].ToString() == "" ? "" : row[5].ToString().Trim();
                                dato.Pol_MesesDif = row[6].ToString() == "" ? 0 : Convert.ToInt32(row[6]);
                                dato.Pol_MesesGar = row[7].ToString() == "" ? 0 : Convert.ToInt32(row[7]);
                                dato.Pol_MesesEsc = row[8].ToString() == "" ? 0 : Convert.ToInt32(row[8]);
                                dato.Pol_PrcRentaEsc = row[9].ToString() == "" ? 0 : Convert.ToDecimal(row[9]);
                                dato.Pol_CodGratif = row[10].ToString() == "" ? "" : row[10].ToString().Trim();
                                dato.Ben_CodSitInv = row[11].ToString() == "" ? "" : row[11].ToString().Trim();
                                dato.Ben_FecNac = row[12].ToString() == "" ? "" : Convert.ToDateTime(row[12].ToString().Trim()).ToString("yyyyMMdd");
                                dato.Ben_CodSexo = row[13].ToString() == "" ? "" : row[13].ToString().Trim();
                                dato.Ben_CodPar = row[14].ToString() == "" ? "" : row[14].ToString().Trim();
                                dato.Ben_PrcPension = row[15].ToString() == "" ? 0 : Convert.ToDecimal(row[15]);
                                dato.CodigoTope18 = row[16].ToString() == "" ? "" : row[16].ToString().Trim();
                                dato.Num_Estudiante = row[17].ToString() == "" ? "" : row[17].ToString().Trim();

                                try { dato.Ben_FecFal = row[18].ToString() == "" ? "" : Convert.ToDateTime(row[18].ToString().Trim()).ToString("yyyyMMdd"); }
                                catch (Exception)
                                {
                                    _log.Info("Error en la lectura del archivo .xls");
                                    _log.Info("Dato incorrecto o no válido para Fecha de Fallecimiento en Póliza No. " + dato.Pol_NumPol);
                                    if (contErrores == 0)
                                    {
                                        errores += "Fecha de Fallecimiento inválida para Póliza(s): ";
                                        errores += dato.Pol_NumPol;
                                        contErrores++;
                                    }
                                    else
                                    {
                                        errores += ", " + dato.Pol_NumPol;
                                    }
                                }

                                dato.Remuneracion_Ini = row[19].ToString() == "" ? 0 : Convert.ToDecimal(row[19]);
                                dato.Remuneracion_Ajus = row[20].ToString() == "" ? 0 : Convert.ToDecimal(row[20]);
                                dato.Pension_Ajus = row[21].ToString() == "" ? 0 : Convert.ToDecimal(row[21]);
                                dato.Pol_MtoPrima = row[22].ToString() == "" ? 0 : Convert.ToDecimal(row[22]);
                                dato.Pol_TasaVta = row[23].ToString() == "" ? 0 : Convert.ToDecimal(row[23]);
                                dato.Pol_TasaLR = row[24].ToString() == "" ? 0 : Convert.ToDecimal(row[24]);
                                dato.Tasa_VtaProm = row[25].ToString() == "" ? 0 : Convert.ToDecimal(row[25]);
                                dato.Tasa_Equiv = row[26].ToString() == "" ? 0 : Convert.ToDecimal(row[26]);
                                #endregion

                                listDatos.Add(dato);
                            }


                        }

                        if (errores != "")
                        {
                            return errores;
                        }
                        else
                        {
                            return _ReservasLogic.Carga_Excel(listDatos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error en la lectura del archivo .xls";
                throw;
            }

        }

        #endregion

        #region Reportes en Excel
        public ActionResult ExportarLog()
        {
            try
            {
                //string fileName = "Log.txt";
                //string pathLog = Server.MapPath("\\") + "slnVidaCamara.log";
                //string pathFile = Server.MapPath("\\") + fileName;

                //var result = _ReservasLogic.descargaLogFTP(pathLog, pathFile);

                //return Json(result);
                string pathLog = Server.MapPath("\\") + "slnVidaCamara.log";
                string pathFile = Server.MapPath("\\") + "slnVidaCamaraTMP.log";

                System.IO.File.Copy(pathLog, pathFile);

                byte[] fileBytes = System.IO.File.ReadAllBytes(pathFile);
                System.IO.File.Delete(pathFile);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "log.txt");
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult ExportarExcel(string nombreArchivo, string pathFile)
        {
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(pathFile);
                //System.IO.File.Delete(pathFile);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static void BuildExcel(DataSet tablas, string ExcelPath)
        {
            using (SpreadsheetDocument myWorkbook =
                SpreadsheetDocument.Create(ExcelPath,
                SpreadsheetDocumentType.Workbook))
            {
                // workbook Part
                WorkbookPart workbookPart = myWorkbook.AddWorkbookPart();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                string relId = workbookPart.GetIdOfPart(worksheetPart);

                var worksheetPartP = workbookPart.AddNewPart<WorksheetPart>();
                string relIdP = workbookPart.GetIdOfPart(worksheetPartP);

                // file Version
                var fileVersion = new FileVersion { ApplicationName = "Microsoft Office Excel" };

                // sheets               
                var sheets = new Sheets();
                var sheetBen = new Sheet { Name = tablas.Tables[0].TableName, SheetId = 1, Id = relId };
                var sheetPol = new Sheet { Name = tablas.Tables[1].TableName, SheetId = 2, Id = relIdP };

                sheets.Append(sheetBen);
                sheets.Append(sheetPol);

                // data
                SheetData sheetDataBen = new SheetData(CreateSheetData(tablas.Tables[0]));
                SheetData sheetDataPol = new SheetData(CreateSheetData(tablas.Tables[1]));

                // add the parts to the workbook and save
                var workbook = new Workbook();
                workbook.Append(fileVersion);
                workbook.Append(sheets);

                var worksheetBen = new Worksheet();
                worksheetBen.Append(sheetDataBen);
                worksheetPart.Worksheet = worksheetBen;
                worksheetPart.Worksheet.Save();

                var worksheetPol = new Worksheet();
                worksheetPol.Append(sheetDataPol);
                worksheetPartP.Worksheet = worksheetPol;
                worksheetPartP.Worksheet.Save();


                myWorkbook.WorkbookPart.Workbook = workbook;
                myWorkbook.WorkbookPart.Workbook.Save();
                myWorkbook.Close();
            }
        }

        private static List<OpenXmlElement> CreateSheetData(DataTable dataTable)
        {
            List<OpenXmlElement> elements = new List<OpenXmlElement>();

            // row header
            var rowHeader = new Row();
            Cell[] cellsHeader = new Cell[dataTable.Columns.Count];

            Cell cellsFormat = new Cell();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                cellsHeader[i] = new Cell();
                cellsHeader[i].DataType = CellValues.String;
                cellsHeader[i].CellValue = new CellValue(dataTable.Columns[i].ColumnName);
            }
            rowHeader.Append(cellsHeader);
            elements.Add(rowHeader);

            // rows data
            foreach (DataRow rowDataTable in dataTable.Rows)
            {
                var row = new Row();
                Cell[] cells = new Cell[dataTable.Columns.Count];

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    cells[i] = new Cell();
                    cells[i].DataType = CellValues.String;
                    cells[i].CellValue = new CellValue(rowDataTable[i].ToString());
                }
                row.Append(cells);
                elements.Add(row);
            }
            return elements;
        }

        #endregion

        #region Reporte Resumen Reservas
        /// <summary>
        /// José Hernández Alvarado.
        /// 22-04-2019
        /// Método que retorna File (Excel) a descargar.
        /// </summary>
        /// <returns></returns>
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

                string auxPathFile = _reportesReservasLogic.ExportarReporteResumenReservas(pFechaPeriodo, strPathFile, "N");

                byte[] fileBytes = System.IO.File.ReadAllBytes(auxPathFile);
                System.IO.File.Delete(auxPathFile);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
            }
            catch (Exception ex)
            {
                _log.Info("Error en Reporte de Resumen de Reservas: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Resumen Adecuaciones
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

                string auxPathFile = _reportesReservasLogic.ExportarReporteResumenAdecuacion(pFechaPeriodo, strPathFile, "N");

                byte[] fileBytes = System.IO.File.ReadAllBytes(auxPathFile);
                System.IO.File.Delete(auxPathFile);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
            }
            catch (Exception ex)
            {
                _log.Info("Error en Reporte de Resumen Adecuación: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Reporte Contable
        public ActionResult ReporteContableReservas(string pFechaPeriodo)
        {
            XmlConfigurator.Configure();
            try
            {
                _log.Info("Se generará el Reporte Contable de Reservas.");

                Random r = new Random();
                int numeroAleatorio = r.Next(100, 999);
                var FechaActual = DateTime.Today;
                string strNombreArchivo = "Reporte Contable - " + FechaActual.ToString("yyyyMMdd") + "_" + numeroAleatorio.ToString() + ".xlsx";
                string strPathFile = Server.MapPath("\\Files\\") + strNombreArchivo;

                System.IO.File.Copy(Server.MapPath("\\Files\\Modelo Reporte Contable.xlsx"), strPathFile);

                string auxPathFile = _reportesReservasLogic.ExportarReporteContable(pFechaPeriodo, strPathFile);

                byte[] fileBytes = System.IO.File.ReadAllBytes(auxPathFile);
                System.IO.File.Delete(auxPathFile);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
            }
            catch (Exception ex)
            {
                _log.Info("Error en Reporte Contable de Reservas: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region Reporte Base SBS
        public ActionResult ReporteSbs(string pFechaPeriodo)
        {
            try
            {
                _log.Info("Se generará el Reporte Base SBS.");

                Random r = new Random();
                int numeroAleatorio = r.Next(100, 999);
                var FechaActual = DateTime.Today;
                string strNombreArchivo = "Base SBS-" + FechaActual.ToString("yyyyMMdd") + "_" + numeroAleatorio.ToString() + ".xlsx";
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

        #endregion

        #region Reporte Flujo de Pasivos
        public ActionResult ReporteFlujosPasivos(string pFechaPeriodo)
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

                string auxPathFile = _reportesReservasLogic.ExportarReporteFlujosPasivos(pFechaPeriodo, strPathFile, "N");


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

        /// <summary>
        /// José Hernández Alvarado.
        /// 11-11-2019
        /// Exporta o descarga archivo de texto con información de Flujos de Pasivos.
        /// </summary>
        /// <param name="pFechaPeriodo">Fecha de periodo de Reservas abierto.</param>
        /// <param name="pTipoArchivo">Tipo de archivo a exportar o descargar.</param>
        /// <returns>Archivo a exportar o descargar.</returns>
        public ActionResult ArchivoTxt(string pFechaPeriodo, string pTipoArchivo)
        {
            XmlConfigurator.Configure();
            try
            {
                _log.Info("Se generará archivo de texto de Flujos de Pasivos.");

                string strNombreArchivo = "02" + pFechaPeriodo.Substring(2, 6) + ".092";
                string strPathFile = Server.MapPath("\\Files\\") + strNombreArchivo;

                string auxPathFile = _reportesReservasLogic.GenerarArchivoTxt(pFechaPeriodo, strPathFile, pTipoArchivo, "N");

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
        #endregion

        #region Reportes de Flujos de Cartera.
        /// <summary>
        /// José Hernández Alvarado.
        /// 27-05-2019
        /// Método para crear el Excel en base a una plantilla, insertando la información en las hojas correspondientes.
        /// </summary>
        /// <returns>Archivo Excel a descargar.</returns>
        public ActionResult exportarFlujosCartera(string FecFlu)
        {
            XmlConfigurator.Configure();
            try
            {
                _log.Info("Se generará el Excel de Flujos Nuevos, espere por favor...");
                Random r = new Random();
                int aleatorio3 = r.Next(100, 999);
                var date = DateTime.Today;
                //string nombreArch = "Flujos Cartera Nuevos - " + date.ToString("yyyyMMdd") + "_" + aleatorio3.ToString() + ".xlsx";
                string pathFile = _ReservasLogic.RutaArchivo();
                _log.Info("Ruta Compartida " + pathFile);
                //System.IO.File.Copy(Server.MapPath("\\Files\\Reporte Flujos.xlsx"), pathFile);

                List<string> ListFluTot = new List<string>();
                List<string> ListFluDetTot = new List<string>();

                List<string> RutasArchivosDescarga = new List<string>();

                _log.Info("Comenzara a generar los primeros flujos");
                ListFluTot = _ReservasLogic.FlujosTotCartera(FecFlu, pathFile);
                _log.Info("Termino de generar los primeros flujos");
                _log.Info("Ruta Primer Flujo" + ListFluTot);
                _log.Info("Comenzara a generar los siguientes flujos");
                ListFluDetTot = _ReservasLogic.FlujosDetTotCartera(FecFlu, pathFile);
                _log.Info("Termino de generar los flujos");
                _log.Info("Ruta Segundo Flujo" + ListFluDetTot);

                RutasArchivosDescarga.AddRange(ListFluTot);
                RutasArchivosDescarga.AddRange(ListFluDetTot);
                string rutaZipDescarga = downloadFolder();
                //byte[] fileBytes = System.IO.File.ReadAllBytes(ListFluDetTot);
                //System.IO.File.Delete(ListFluDetTot);
                return File(rutaZipDescarga, System.Net.Mime.MediaTypeNames.Application.Octet, "ArchivosReservas.zip");
            }
            catch (Exception ex)
            {
                _log.Info("Error en Reporte de Flujos Nuevos: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 18-06-2019
        /// Método para crear el Excel en base a una plantilla, insertando la información en las hojas correspondientes (cálculo antigüo).
        /// </summary>
        /// <returns>Archivo Excel a descargar.</returns>
        public ActionResult exportarFlujosCarteraAnt(string FecFlu)
        {
            XmlConfigurator.Configure();
            try
            {
                _log.Info("Se generará el Excel de Flujos Antigüos, espere por favor...");
                Random r = new Random();
                int aleatorio3 = r.Next(100, 999);
                var date = DateTime.Today;
                //string nombreArch = "Flujos Cartera Nuevos - " + date.ToString("yyyyMMdd") + "_" + aleatorio3.ToString() + ".xlsx";
                string pathFile = _ReservasLogic.RutaArchivo();
                _log.Info("Ruta Compartida Ant" + pathFile);
                //System.IO.File.Copy(Server.MapPath("\\Files\\Reporte Flujos.xlsx"), pathFile);

                List<string> ListFluTot = new List<string>();
                List<string> ListFluDetTot = new List<string>();

                List<string> RutasArchivosDescarga = new List<string>();
                                
                _log.Info("Comenzara a generar los primeros flujos Ant");
                ListFluTot = _ReservasLogic.FlujosTotCarteraAnt(FecFlu, pathFile);
                _log.Info("Termino de generar los primeros flujos Ant");
                _log.Info("Ruta Primer Flujo" + ListFluTot);
                _log.Info("Comenzara a generar los siguientes flujos Ant");
                ListFluDetTot = _ReservasLogic.FlujosDetTotCarteraAnt(FecFlu, pathFile);
                _log.Info("Termino de generar los flujos");
                _log.Info("Ruta Segundo Flujo" + ListFluDetTot);

                RutasArchivosDescarga.AddRange(ListFluTot);
                RutasArchivosDescarga.AddRange(ListFluDetTot);
                string rutaZipDescarga = downloadFolder();
                //byte[] fileBytes = System.IO.File.ReadAllBytes(ListFluDetTot);
                //System.IO.File.Delete(ListFluDetTot);

                return File(rutaZipDescarga, System.Net.Mime.MediaTypeNames.Application.Octet, "ArchivosReservas.zip");
            }
            catch (Exception ex)
            {
                _log.Info("Error en Reporte de Flujos Antigüos: " + ex.Message);
                return null;
            }
        }

        public string downloadFolder()
        {
            try
            {
                string pathFile = _ReservasLogic.RutaArchivo();
                string directoryinfo = Path.GetDirectoryName(pathFile);

                string rutaZipDescarga = Path.GetDirectoryName(directoryinfo) + "\\ArchivosReservas.zip";
                if (System.IO.File.Exists(rutaZipDescarga))
                {
                    System.IO.File.Delete(rutaZipDescarga);
                }
                string[] files = System.IO.Directory.GetFiles(pathFile);
                
                ZipFile.CreateFromDirectory(pathFile, rutaZipDescarga);//"C:\\Downloads\\ArchivosReservas.zip");

                foreach (string s in files)
                {
                    System.IO.File.Delete(s);
                }
                //byte[] fileBytes = System.IO.File.ReadAllBytes(pathFile+"ArchivosReservas.zip");
                //System.IO.File.Delete(ListFluDetTot);
                return rutaZipDescarga;
                
            }
            catch (Exception ex)
            {
                Response.ContentType = "text/HTML";
                Response.Write(ex.Message);
                return null;
            }
        }
        #endregion

        #region Reporte de Cálculo de Reservas
        /// <summary>
        /// José Hernández Alvarado.
        /// 19-06-2019
        /// Método reutilizado para descargar documento Excel en el formato requerido del Cálculo de Reservas (3er Proceso).
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportarCalculoRes()
        {
            XmlConfigurator.Configure();
            try
            {
                _log.Info("Se generará el Excel de Cálculo de Reservas, espere por favor...");

                Random r = new Random();
                int aleatorio3 = r.Next(100, 999);
                var date = DateTime.Today;
                string nombreArch = "Reservas - " + date.ToString("yyyyMMdd") + "_" + aleatorio3.ToString() + ".xlsx";
                string pathFile = _ReservasLogic.RutaArchivoCalculoRes();
                string rutaPlantilla = _ReservasLogic.RutaArchivoCalculoRes();
                pathFile = pathFile + "\\Files\\" + nombreArch;

                System.IO.File.Copy(rutaPlantilla +"\\Files\\Calculo Reservas.xlsx", pathFile);

                List<Reservas> ListCalculoRes = new List<Reservas>();
                List<Reservas> ListCalculoResAnt = new List<Reservas>();
                ListCalculoRes = _ReservasLogic.RptCalculoRes();
                ListCalculoResAnt = _ReservasLogic.RptCalculoResAnt();
                DateTime vlFecFall;

                SLDocument sl = new SLDocument(pathFile);

                #region Llenado de excel
                if (ListCalculoRes != null)
                {
                    #region Nuevas
                    sl.SelectWorksheet("Reservas Nuevas");
                    for (int i = 0; i < ListCalculoRes.Count; i++)
                    {
                        sl.SetCellValue(2 + i, 1, Convert.ToInt32(ListCalculoRes[i].Pol_NumPol));
                        sl.SetCellValue(2 + i, 2, ListCalculoRes[i].Pol_CUSPP);
                        sl.SetCellValue(2 + i, 3, ListCalculoRes[i].Pol_Prestacion);
                        sl.SetCellValue(2 + i, 4, ListCalculoRes[i].Pol_CodMod);
                        sl.SetCellValue(2 + i, 5, Convert.ToDateTime(ListCalculoRes[i].Pol_FecVig));
                        sl.SetCellValue(2 + i, 6, Convert.ToDateTime(ListCalculoRes[i].Pol_FecDev));
                        sl.SetCellValue(2 + i, 7, ListCalculoRes[i].Pol_MesesDif);
                        sl.SetCellValue(2 + i, 8, ListCalculoRes[i].Pol_MesesGar);
                        sl.SetCellValue(2 + i, 9, ListCalculoRes[i].Pol_MesesEsc);
                        sl.SetCellValue(2 + i, 10, ListCalculoRes[i].Pol_PrcRentaEsc);
                        sl.SetCellValue(2 + i, 11, ListCalculoRes[i].Pol_CodGratif);
                        sl.SetCellValue(2 + i, 12, ListCalculoRes[i].Ben_CodSitInv);
                        sl.SetCellValue(2 + i, 13, Convert.ToDateTime(ListCalculoRes[i].Ben_FecNac));
                        sl.SetCellValue(2 + i, 14, ListCalculoRes[i].Ben_CodSexo);
                        sl.SetCellValue(2 + i, 15, ListCalculoRes[i].Ben_CodPar);
                        sl.SetCellValue(2 + i, 16, ListCalculoRes[i].Ben_PrcPension);
                        sl.SetCellValue(2 + i, 17, ListCalculoRes[i].Num_Estudiante);
                        sl.SetCellValue(2 + i, 18, ListCalculoRes[i].Cod_Estudiante);
                        if (ListCalculoRes[i].Ben_FecFal != "")
                        {
                            try
                            {
                                vlFecFall = Convert.ToDateTime(ListCalculoRes[i].Ben_FecFal);
                                sl.SetCellValue(2 + i, 19, vlFecFall);
                            }
                            catch (Exception)
                            {
                                sl.SetCellValue(2 + i, 19, ListCalculoRes[i].Ben_FecFal);
                            }
                        }
                        else
                        {
                            sl.SetCellValue(2 + i, 19, ListCalculoRes[i].Ben_FecFal);
                        }

                        sl.SetCellValue(2 + i, 20, ListCalculoRes[i].Remuneracion_Ini);
                        sl.SetCellValue(2 + i, 21, ListCalculoRes[i].Remuneracion_Ajus);
                        sl.SetCellValue(2 + i, 22, ListCalculoRes[i].Pension_Ajus);
                        sl.SetCellValue(2 + i, 23, ListCalculoRes[i].Pol_MtoPrima);
                        sl.SetCellValue(2 + i, 24, ListCalculoRes[i].Pol_TasaVta);
                        sl.SetCellValue(2 + i, 25, ListCalculoRes[i].Pol_TasaLR);
                        sl.SetCellValue(2 + i, 26, ListCalculoRes[i].Tasa_VtaProm);
                        sl.SetCellValue(2 + i, 27, ListCalculoRes[i].Tasa_Equiv);
                        sl.SetCellValue(2 + i, 28, ListCalculoRes[i].Reserva_Ben);
                        sl.SetCellValue(2 + i, 29, ListCalculoRes[i].Reserva_GastoSep);
                        sl.SetCellValue(2 + i, 30, ListCalculoRes[i].Reserva_TotPol);
                    }
                    #endregion
                }

                if (ListCalculoResAnt != null)
                {
                    #region Antigüas
                    sl.SelectWorksheet("Reservas Antigüas");
                    for (int i = 0; i < ListCalculoResAnt.Count; i++)
                    {
                        sl.SetCellValue(2 + i, 1, Convert.ToInt32(ListCalculoResAnt[i].Pol_NumPol));
                        sl.SetCellValue(2 + i, 2, ListCalculoResAnt[i].Pol_CUSPP);
                        sl.SetCellValue(2 + i, 3, ListCalculoResAnt[i].Pol_Prestacion);
                        sl.SetCellValue(2 + i, 4, ListCalculoResAnt[i].Pol_CodMod);
                        sl.SetCellValue(2 + i, 5, Convert.ToDateTime(ListCalculoResAnt[i].Pol_FecVig));
                        sl.SetCellValue(2 + i, 6, Convert.ToDateTime(ListCalculoResAnt[i].Pol_FecDev));
                        sl.SetCellValue(2 + i, 7, ListCalculoResAnt[i].Pol_MesesDif);
                        sl.SetCellValue(2 + i, 8, ListCalculoResAnt[i].Pol_MesesGar);
                        sl.SetCellValue(2 + i, 9, ListCalculoResAnt[i].Pol_MesesEsc);
                        sl.SetCellValue(2 + i, 10, ListCalculoResAnt[i].Pol_PrcRentaEsc);
                        sl.SetCellValue(2 + i, 11, ListCalculoResAnt[i].Pol_CodGratif);
                        sl.SetCellValue(2 + i, 12, ListCalculoResAnt[i].Ben_CodSitInv);
                        sl.SetCellValue(2 + i, 13, Convert.ToDateTime(ListCalculoResAnt[i].Ben_FecNac));
                        sl.SetCellValue(2 + i, 14, ListCalculoResAnt[i].Ben_CodSexo);
                        sl.SetCellValue(2 + i, 15, ListCalculoResAnt[i].Ben_CodPar);
                        sl.SetCellValue(2 + i, 16, ListCalculoResAnt[i].Ben_PrcPension);
                        sl.SetCellValue(2 + i, 17, ListCalculoResAnt[i].Num_Estudiante);
                        sl.SetCellValue(2 + i, 18, ListCalculoResAnt[i].Cod_Estudiante);
                        if (ListCalculoResAnt[i].Ben_FecFal != "")
                        {
                            try
                            {
                                vlFecFall = Convert.ToDateTime(ListCalculoResAnt[i].Ben_FecFal);
                                sl.SetCellValue(2 + i, 19, vlFecFall);
                            }
                            catch (Exception)
                            {
                                sl.SetCellValue(2 + i, 19, ListCalculoResAnt[i].Ben_FecFal);
                            }
                        }
                        else
                        {
                            sl.SetCellValue(2 + i, 19, ListCalculoResAnt[i].Ben_FecFal);
                        }

                        sl.SetCellValue(2 + i, 20, ListCalculoResAnt[i].Remuneracion_Ini);
                        sl.SetCellValue(2 + i, 21, ListCalculoResAnt[i].Remuneracion_Ajus);
                        sl.SetCellValue(2 + i, 22, ListCalculoResAnt[i].Pension_Ajus);
                        sl.SetCellValue(2 + i, 23, ListCalculoResAnt[i].Pol_MtoPrima);
                        sl.SetCellValue(2 + i, 24, ListCalculoResAnt[i].Pol_TasaVta);
                        sl.SetCellValue(2 + i, 25, ListCalculoResAnt[i].Pol_TasaLR);
                        sl.SetCellValue(2 + i, 26, ListCalculoResAnt[i].Tasa_VtaProm);
                        sl.SetCellValue(2 + i, 27, ListCalculoResAnt[i].Tasa_Equiv);
                        sl.SetCellValue(2 + i, 28, ListCalculoResAnt[i].Reserva_Ben);
                        sl.SetCellValue(2 + i, 29, ListCalculoResAnt[i].Reserva_GastoSep);
                        sl.SetCellValue(2 + i, 30, ListCalculoResAnt[i].Reserva_TotPol);
                    }
                    #endregion
                }
                #endregion

                sl.SaveAs(pathFile);

                byte[] fileBytes = System.IO.File.ReadAllBytes(pathFile);
                System.IO.File.Delete(pathFile);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArch);
            }
            catch (Exception ex)
            {
                _log.Info("Error en Reporte de Cálculo de Reservas: " + ex.Message);
                return null;
            }
        }
        #endregion
        
        #endregion
    }
}