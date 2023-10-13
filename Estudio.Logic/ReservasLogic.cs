using Estudio.Process;
using Estudio.Process.Muestra;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Estudio.Logic
{
    public class ReservasLogic
    {
        ReservasRepository _ReservasRepository = new ReservasRepository();
        ExportarExcelRepository _exportarExcelRepository = new ExportarExcelRepository();
        RutinaReservasRepository _rutinaReservasRepository = new RutinaReservasRepository();
        PaginaFlujos _paginaFlujos = new PaginaFlujos();
        PaginaResMatBas _paginaCalReservas = new PaginaResMatBas();
        Response resPeriodo = new Response();

        RutinaFlujos _rutinaFlujos = new RutinaFlujos();
        List<string> listLogRutinaFlu = new List<string>();

        #region Listas para consultas de información.

        List<beMortalidadDin> LisTabDinPar = new List<beMortalidadDin>();
        List<beMortalidadDinDet> LisTabMD = new List<beMortalidadDinDet>();
        List<beTasasPromedio> ListaTasProm = new List<beTasasPromedio>();
        List<beCurvaTasas> ListaCurvaTasas = new List<beCurvaTasas>();

        List<beDatosPol> LisTabPol = new List<beDatosPol>();
        List<beDatosBen> LisTabBen = new List<beDatosBen>();
        List<Reservas> ListCamb = new List<Reservas>();

        List<beResultadosFlujos> ListaFlu = new List<beResultadosFlujos>(); //Lista para rutina de Cálculo de Reservas (Flujos nuevos).
        List<beResultadosFlujos> ListaFluAnt = new List<beResultadosFlujos>(); //Lista para rutina de Cálculo de Reservas (Flujos antigüos).
        List<beDatosPol> LisTabPolMatPar = new List<beDatosPol>();
        List<beDatosBen> LisTabBenMatPar = new List<beDatosBen>();
        List<beResultadosFlujos> LisFlujosPenPar = new List<beResultadosFlujos>();
        List<beCurvaTasas> ListaGtoSepelio = new List<beCurvaTasas>();

        List<beMortalVar> LisTabMorPar = new List<beMortalVar>();
        List<beMortalidadDet> LisTabMorDet = new List<beMortalidadDet>();

        #endregion

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string resFlujos = "", resReserva = "";
        double tipCam = 0;

        /// <summary>
        /// Migrar pólizas y beneficiarios de Reservas.
        /// José Hernández Alvarado / Jesús Solis / Omar Figueroa
        /// 10-12-2019
        /// </summary>
        /// <param name="fecha">Fecha de periodo abierto.</param>
        /// <param name="usuario">Usuario en el sistema.</param>
        /// <returns>Respuesta de confirmación del proceso de migración.</returns>
        public Response IniCal(string fecha, string usuario)
        {
            XmlConfigurator.Configure();
            Response respuesta = new Response();
            //int lastPol = 0;
            List<Reservas> TasasPol = new List<Reservas>();

            _log.Info("Iniciará el Proceso de Migración de Reservas, por favor espere...");
            try
            {
                //Líneas para seleccionar solo las pólizas que no se encuentran en la tabla de tasas para la generación del excel e insertarlas posteriormente.
                TasasPol = _ReservasRepository.Consulta_PolizasTasas();

                //lastPol = TasasPol.Count != 0 ? _ReservasRepository.Consulta_UltimaPol() : 0;

                respuesta = _ReservasRepository.IniCal(fecha, usuario, TasasPol.Count, TasasPol);

                return respuesta;
            }
            catch (Exception ex)
            {
                _log.Info("Error en capa lógica del Proceso de Migración de Reservas: " + ex.Message);
                respuesta.IsOk = false;
                respuesta.Message = ex.Message;
                return respuesta;
            }
        }

        /// <summary>
        /// Validar fecha de cálculo de proceso de Reservas.
        /// José Hernández Alvarado / Jesús Solis / Omar Figueroa
        /// 11-12-2019
        /// </summary>
        /// <param name="pFecha">Fecha de cálculo de proceso de la pantalla de Reservas.</param>
        /// <returns></returns>
        public Response PeriodoLogic(string pFecha)
        {
            Response respuesta = new Response();
            try
            {
                respuesta = _ReservasRepository.PeriodoRepository(pFecha);
                return respuesta;
            }
            catch (Exception ex)
            {
                Response res2 = new Response();
                res2.IsOk = false;
                res2.Message = ex.Message;
                return res2;
            }
        }

        /// <summary>
        /// Buscar periodo abierto para mostrarlo en campo de pantalla de Reservas.
        /// José Hernández Alvarado / Jesús Solis / Omar Figueroa
        /// 11-12-2019
        /// </summary>
        /// <returns>Fecha de periodo encontrado o mensaje de error.</returns>
        public Response ConsultarPeriodoAbierto()
        {
            XmlConfigurator.Configure();
            Response respuesta = new Response();
            Reservas periodoAbierto = new Reservas();
            string strQuery = "";

            try
            {
                strQuery = "SELECT FEC_CALCULO FROM PR_TMAE_PROCALDEF WHERE COD_CLIENTE = 1 AND COD_ESTPERIODO = 'A'";
                periodoAbierto = _ReservasRepository.ConsultarPeriodoAbierto(strQuery);

                if (periodoAbierto != null)
                {
                    respuesta.IsOk = true;
                    respuesta.Object = periodoAbierto.Cod_ESTPERIODO;
                    respuesta.Message = "Abierto";
                }
                else
                {
                    respuesta.IsOk = false;
                    respuesta.Message = "No se encontró ningún periodo abierto en Base de Datos. Operacion cancelada.";
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR al consultar periodo abierto: " + ex.Message);
                respuesta.IsOk = false;
                respuesta.Message = "Error al consultar periodo abierto: " + ex.Message;
                return respuesta;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 26-02-2019
        /// </summary>
        /// <param name="listDatos">Lista de registros con los datos correspondientes a actualizar en BD.</param>
        /// <returns>Mensaje de confirmación de proceso de actualización.</returns>
        public string Carga_Excel(List<Reservas> listDatos)
        {
            List<Reservas> ListParentesco = new List<Reservas>();
            ListParentesco = _ReservasRepository.Homologa_Parentesco();

            if (ListParentesco == null)
            {
                return "Error en consulta de información para homologación de parentesco.";
            }

            return _ReservasRepository.Carga_Excel(listDatos, ListParentesco);
        }

        #region Reporte Proceso Migración

        /// <summary>
        /// Método para generar Rpt de Proceso de Migración con plantilla.
        /// 27-06-2019
        /// </summary>
        /// <returns>Lista con registros para mostrar en el Rpt.</returns>
        public List<Reservas> ReservasMigracion()
        {
            XmlConfigurator.Configure();

            List<Reservas> _logicResMigracion = new List<Reservas>();
            try
            {
                _logicResMigracion = _ReservasRepository.Consulta_ResMigracion();
                if (_logicResMigracion != null) { _log.Info("Información consultada correctamente."); };
                return _logicResMigracion;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la información para el Reporte de Proceso de Migración : " + ex.Message);
                Console.WriteLine(ex.Message);
                return _logicResMigracion;
            }
        }

        #endregion

        #region Reportes de Flujos de Cartera (2do proceso)
        #region Rpt Cálculo Nuevo
        /// <summary>
        /// José Hernández Alvarado.
        /// 27-05-2019
        /// Método que retorna la lista con los datos consultados o vacía en caso de tener algún error (rutina nueva).
        /// </summary>
        /// <returns>Lista con datos consultados o vacía por algún error.</returns>
        public List<string> FlujosTotCartera(string FecFlu, string ruta)
        {
            XmlConfigurator.Configure();
            List<string> RutasArchivosDescarga = new List<string>();

            int _logicFlujosTot = 0;
            int numeroRegistros = 0;
            int numeroHojas = 0;
            int registrosRestantes = 0;
            int registrosHoja = 600000;
            decimal hojas = 0;
            string nombreArchivo = "";
            string nombreArchivoSP = "";
            bool generarArchivo;
            int contador = 1;

            try
            {
                numeroRegistros = _ReservasRepository.Consulta_FluCartera(FecFlu);
                DataTable archivosDT = new DataTable();
                archivosDT.Columns.Add("Ruta_Archivo", typeof(string));
                archivosDT.Columns.Add("Numero_Archivo", typeof(int));
                archivosDT.Columns.Add("Numero_Registros", typeof(int));
                archivosDT.Columns.Add("Ruta_ArchivoSinExtension", typeof(string));

                hojas = (decimal)numeroRegistros / registrosHoja;
                numeroHojas = hojas % 1 == 0 ? (int)hojas : (int)Math.Truncate(hojas) + 1;

                for (int numeroArchivo = 0; numeroArchivo < numeroHojas; numeroArchivo++)
                {
                    nombreArchivo = numeroArchivo == 0 ? @"Reporte Flujos" : @"Reporte Flujos_" + (numeroArchivo + 1).ToString();
                    nombreArchivoSP = ruta + nombreArchivo;

                    DataRow rowHoja = archivosDT.NewRow();
                    rowHoja["Ruta_Archivo"] = nombreArchivoSP + ".xls";
                    rowHoja["Numero_Archivo"] = numeroArchivo;
                    rowHoja["Numero_Registros"] = registrosHoja;
                    rowHoja["Ruta_ArchivoSinExtension"] = nombreArchivoSP;

                    archivosDT.Rows.Add(rowHoja);
                }

                generarArchivo = _ReservasRepository.CreacionArchivosRPT_FLUPOLTOT_hoja1(archivosDT, "TMP_HOJA_POLIZA_CALNUEVO ORDER BY MES", "TMP_HOJA_POLIZA_CALNUEVO");

                foreach (DataRow row in archivosDT.Rows)
                {
                    GeneracionExcelReservas_FlujosTotCarteraHoja1(row[3].ToString(), "Flupol " + contador.ToString());
                    File.Delete(row[0].ToString());
                    RutasArchivosDescarga.Add(row[3].ToString() + ".xlsx");
                    contador++;
                }

                if (_logicFlujosTot != null) { _log.Info("Se consultó la información correctamente."); };
                return RutasArchivosDescarga;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la información para el Reporte de Flujos de Cartera Totales : " + ex.Message);
                Console.WriteLine(ex.Message);
                return RutasArchivosDescarga;
            }
        }

        public bool GeneracionExcelReservas_FlujosTotCarteraHoja1(string rutaArchivo, string nombreHoja)
        {
            try
            {
                _log.Info("Comenzara a convertir el archivo creado de BD");
                ExcelTextFormat format = new ExcelTextFormat();
                format.Delimiter = '\t';
                format.Culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
                format.Culture.DateTimeFormat.ShortDatePattern = "dd-mm-yyyy";
                format.Encoding = Encoding.GetEncoding(1252);
                format.DataTypes = new eDataTypes[] { eDataTypes.String, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number };

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                _log.Info("Ruta del archivo a convertir " + rutaArchivo);
                //create a new Excel package
                //rutaArchivo = "C:\\Users\\lizbeth.morales\\Desktop\\Vida Camara\\Cotizador\\Reporte Flujos";
                using (ExcelPackage package = new ExcelPackage(new FileInfo(rutaArchivo + ".xlsx")))
                {
                    _log.Info("Entro a convertir " + nombreHoja);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(nombreHoja);

                    worksheet.Cells["A1"].LoadFromText(new FileInfo(rutaArchivo + ".xls"), format, OfficeOpenXml.Table.TableStyles.None, true);

                    worksheet.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:J1"].Style.Fill.BackgroundColor.SetColor(Color.Black);
                    worksheet.Cells["A1:J1"].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells["A1:J1"].Style.Font.Size = 11;
                    worksheet.Cells["A1:J1"].Style.Font.Bold = true;
                    worksheet.Cells["A1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    worksheet.Cells["A1"].Value = " MES ";
                    worksheet.Cells["B1"].Value = " SI_PENSIONES ";
                    worksheet.Cells["C1"].Value = " SI_SEPELIO ";
                    worksheet.Cells["D1"].Value = " SA_PENSIONES ";
                    worksheet.Cells["E1"].Value = " SA_SEPELIO ";
                    worksheet.Cells["F1"].Value = " D_PENSIONES ";
                    worksheet.Cells["G1"].Value = " D_SEPELIO ";
                    worksheet.Cells["H1"].Value = " SI_TOTALES ";
                    worksheet.Cells["I1"].Value = " SA_TOTALES ";
                    worksheet.Cells["J1"].Value = " D_TOTALES ";

                    worksheet.Cells["A1"].AutoFitColumns();
                    worksheet.Cells["B1"].AutoFitColumns();
                    worksheet.Cells["C1"].AutoFitColumns();
                    worksheet.Cells["D1"].AutoFitColumns();
                    worksheet.Cells["E1"].AutoFitColumns();
                    worksheet.Cells["F1"].AutoFitColumns();
                    worksheet.Cells["G1"].AutoFitColumns();
                    worksheet.Cells["H1"].AutoFitColumns();
                    worksheet.Cells["I1"].AutoFitColumns();
                    worksheet.Cells["J1"].AutoFitColumns();
                    package.Save();

                    _log.Info("Termino de ingresar informacion");
                }
                _log.Info("Termino de ingresar toda la informacion");
                return true;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR EN LA GENERACION DEL ARCHIVO FlujosTotCartera: " + ex);
                return false;
            }
        }

        public string RutaArchivo()
        {
            XmlConfigurator.Configure();

            string ruta = "";
            try
            {
                ruta = _ReservasRepository.RutaArchivo();
                return ruta;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la ruta del archivo " + ex.Message);
                Console.WriteLine(ex.Message);
                return ruta;
            }
        }

        public string RutaArchivoCalculoRes()
        {
            XmlConfigurator.Configure();

            string ruta = "";
            try
            {
                ruta = _ReservasRepository.RutaArchivoCalculoRes();
                return ruta;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la ruta del archivo " + ex.Message);
                Console.WriteLine(ex.Message);
                return ruta;
            }
        }

        public string RutaArchivoMigracion()
        {
            XmlConfigurator.Configure();

            string ruta = "";
            try
            {
                ruta = _ReservasRepository.RutaArchivoMigracion();
                return ruta;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la ruta del archivo " + ex.Message);
                Console.WriteLine(ex.Message);
                return ruta;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 27-05-2019
        /// Método que retorna la lista con los datos consultados o vacía en caso de tener algún error (rutina nueva detallando cada póliza).
        /// </summary>
        /// <returns>Lista con datos consultados o vacía por algún error.</returns>
        //public List<Reservas> FlujosDetTotCartera(string FecFlu)
        //{
        //    XmlConfigurator.Configure();

        //    List<Reservas> _logicFlujosDetTot = new List<Reservas>();
        //    try
        //    {
        //        _logicFlujosDetTot = _ReservasRepository.Consulta_FluDetCartera(FecFlu);
        //        if (_logicFlujosDetTot != null) { _log.Info("Se consultó la información correctamente."); };
        //        return _logicFlujosDetTot;
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info("Error al consultar la información para el Reporte de Flujos de Cartera Por Póliza: " + ex.Message);
        //        Console.WriteLine(ex.Message);
        //        return _logicFlujosDetTot;
        //    }
        //}

        public List<string> FlujosDetTotCartera(string FecFlu, string ruta)
        {
            //XmlConfigurator.Configure();

            //string _logicFlujosDetTot = "";
            //try
            //{
            //    _logicFlujosDetTot = _ReservasRepository.Consulta_FluDetCartera(FecFlu);
            //    if (_logicFlujosDetTot != null) { _log.Info("Se consultó la información correctamente."); };
            //    return _logicFlujosDetTot;
            //}
            //catch (Exception ex)
            //{
            //    _log.Info("Error al consultar la información para el Reporte de Flujos de Cartera Por Póliza: " + ex.Message);
            //    Console.WriteLine(ex.Message);
            //    return _logicFlujosDetTot;
            //}

            XmlConfigurator.Configure();
            List<string> RutasArchivosDescarga = new List<string>();
            int _logicFlujosTot = 0;
            int numeroRegistros = 0;
            int numeroHojas = 0;
            int registrosRestantes = 0;
            int registrosHoja = 600000;
            decimal hojas = 0;
            string nombreArchivo = "";
            string nombreArchivoSP = "";
            bool generarArchivo;
            int contador = 1;

            try
            {
                numeroRegistros = _ReservasRepository.Consulta_FluDetCartera(FecFlu);
                DataTable archivosDT = new DataTable();
                archivosDT.Columns.Add("Ruta_Archivo", typeof(string));
                archivosDT.Columns.Add("Numero_Archivo", typeof(int));
                archivosDT.Columns.Add("Numero_Registros", typeof(int));
                archivosDT.Columns.Add("Ruta_ArchivoSinExtension", typeof(string));

                hojas = (decimal)numeroRegistros / registrosHoja;
                numeroHojas = hojas % 1 == 0 ? (int)hojas : (int)Math.Truncate(hojas) + 1;

                for (int numeroArchivo = 0; numeroArchivo < numeroHojas; numeroArchivo++)
                {
                    nombreArchivo = numeroArchivo == 0 ? @"Reporte Flujos_Ben" : @"Reporte Flujos_Ben_" + (numeroArchivo + 1).ToString();
                    nombreArchivoSP = ruta + "\\" + nombreArchivo;

                    DataRow rowHoja = archivosDT.NewRow();
                    rowHoja["Ruta_Archivo"] = nombreArchivoSP + ".xls";
                    rowHoja["Numero_Archivo"] = numeroArchivo;
                    rowHoja["Numero_Registros"] = registrosHoja;
                    rowHoja["Ruta_ArchivoSinExtension"] = nombreArchivoSP;

                    archivosDT.Rows.Add(rowHoja);
                }

                generarArchivo = _ReservasRepository.CreacionArchivosRPT_FLUPOLTOT_hoja1(archivosDT, "TMP_HOJA_DETALLE_CALNUEVO ORDER BY NUM_POLIZA", "TMP_HOJA_DETALLE_CALNUEVO");

                foreach (DataRow row in archivosDT.Rows)
                {
                    GeneracionExcelReservas_FlujosTotCarteraHojaBen(row[3].ToString(), "Fluben " + contador.ToString());
                    File.Delete(row[0].ToString());
                    RutasArchivosDescarga.Add(row[3].ToString() + ".xlsx");
                    contador++;
                }

                if (_logicFlujosTot != null) { _log.Info("Se consultó la información correctamente."); };
                return RutasArchivosDescarga;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la información para el Reporte de Flujos de Cartera Totales : " + ex.Message);
                Console.WriteLine(ex.Message);
                return RutasArchivosDescarga;
            }
        }

        public bool GeneracionExcelReservas_FlujosTotCarteraHojaBen(string rutaArchivo, string nombreHoja)
        {
            try
            {
                _log.Info("Comenzara a convertir el archivo creado de BD");
                ExcelTextFormat format = new ExcelTextFormat();
                format.Delimiter = '\t';
                format.Culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
                format.Culture.DateTimeFormat.ShortDatePattern = "dd-mm-yyyy";
                format.Encoding = Encoding.GetEncoding(1252);
                format.DataTypes = new eDataTypes[] { eDataTypes.String, eDataTypes.String, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number };

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                _log.Info("Ruta del archivo a convertir " + rutaArchivo);
                //create a new Excel package
                //rutaArchivo = "C:\\Users\\lizbeth.morales\\Desktop\\Vida Camara\\Cotizador\\Reporte Flujos_Ben";
                using (ExcelPackage package = new ExcelPackage(new FileInfo(rutaArchivo + ".xlsx")))
                {
                    _log.Info("Entro a convertir " + nombreHoja);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(nombreHoja);

                    worksheet.Cells["A1"].LoadFromText(new FileInfo(rutaArchivo + ".xls"), format, OfficeOpenXml.Table.TableStyles.None, true);

                    worksheet.Cells["A1:R1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:R1"].Style.Fill.BackgroundColor.SetColor(Color.Black);
                    worksheet.Cells["A1:R1"].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells["A1:R1"].Style.Font.Size = 11;
                    worksheet.Cells["A1:R1"].Style.Font.Bold = true;
                    worksheet.Cells["A1:R1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    worksheet.Cells["A1"].Value = " NUM_POLIZA ";
                    worksheet.Cells["B1"].Value = " NUM_MES ";
                    worksheet.Cells["C1"].Value = " GASTO_SEP ";
                    worksheet.Cells["D1"].Value = " BEN_TITULAR ";
                    worksheet.Cells["E1"].Value = " BEN_CONY ";
                    worksheet.Cells["F1"].Value = " BEN_PADRE ";
                    worksheet.Cells["G1"].Value = " BEN_MADRE ";
                    worksheet.Cells["H1"].Value = " BEN_HIJO1 ";
                    worksheet.Cells["I1"].Value = " BEN_HIJO2 ";
                    worksheet.Cells["J1"].Value = " BEN_HIJO3 ";
                    worksheet.Cells["K1"].Value = " BEN_HIJO4 ";
                    worksheet.Cells["L1"].Value = " BEN_HIJO5 ";
                    worksheet.Cells["M1"].Value = " BEN_HIJO6 ";
                    worksheet.Cells["N1"].Value = " BEN_HIJO7 ";
                    worksheet.Cells["O1"].Value = " BEN_HIJO8 ";
                    worksheet.Cells["P1"].Value = " BEN_HIJO9 ";
                    worksheet.Cells["Q1"].Value = " BEN_HIJO10 ";
                    worksheet.Cells["R1"].Value = " MTO_TOTAL ";

                    worksheet.Cells["A1"].AutoFitColumns();
                    worksheet.Cells["B1"].AutoFitColumns();
                    worksheet.Cells["C1"].AutoFitColumns();
                    worksheet.Cells["D1"].AutoFitColumns();
                    worksheet.Cells["E1"].AutoFitColumns();
                    worksheet.Cells["F1"].AutoFitColumns();
                    worksheet.Cells["G1"].AutoFitColumns();
                    worksheet.Cells["H1"].AutoFitColumns();
                    worksheet.Cells["I1"].AutoFitColumns();
                    worksheet.Cells["J1"].AutoFitColumns();
                    worksheet.Cells["K1"].AutoFitColumns();
                    worksheet.Cells["L1"].AutoFitColumns();
                    worksheet.Cells["M1"].AutoFitColumns();
                    worksheet.Cells["N1"].AutoFitColumns();
                    worksheet.Cells["O1"].AutoFitColumns();
                    worksheet.Cells["P1"].AutoFitColumns();
                    worksheet.Cells["Q1"].AutoFitColumns();
                    worksheet.Cells["R1"].AutoFitColumns();
                    package.Save();

                    _log.Info("Termino de ingresar informacion");
                }
                _log.Info("Termino de ingresar toda la informacion");
                return true;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR EN LA GENERACION DEL ARCHIVO FlujosTotCartera: " + ex);
                return false;
            }
        }
        #endregion

        #region Rpt Cálculo Antigüo

        /// <summary>
        /// José Hernández Alvarado.
        /// 18-06-2019
        /// Método que retorna la lista con los datos consultados o vacía en caso de tener algún error (para rutina antigüa).
        /// </summary>
        /// <returns>Lista con datos consultados o vacía por algún error.</returns>
        public List<string> FlujosTotCarteraAnt(string FecFlu, string ruta)
        {
            XmlConfigurator.Configure();
            List<string> RutasArchivosDescarga = new List<string>();

            int _logicFlujosTot = 0;
            int numeroRegistros = 0;
            int numeroHojas = 0;
            int registrosRestantes = 0;
            int registrosHoja = 600000;
            decimal hojas = 0;
            string nombreArchivo = "";
            string nombreArchivoSP = "";
            bool generarArchivo;
            int contador = 1;
            try
            {
                numeroRegistros = _ReservasRepository.Consulta_FluCarteraAnt(FecFlu);
                DataTable archivosDT = new DataTable();
                archivosDT.Columns.Add("Ruta_Archivo", typeof(string));
                archivosDT.Columns.Add("Numero_Archivo", typeof(int));
                archivosDT.Columns.Add("Numero_Registros", typeof(int));
                archivosDT.Columns.Add("Ruta_ArchivoSinExtension", typeof(string));

                hojas = (decimal)numeroRegistros / registrosHoja;
                numeroHojas = hojas % 1 == 0 ? (int)hojas : (int)Math.Truncate(hojas) + 1;
                for (int numeroArchivo = 0; numeroArchivo < numeroHojas; numeroArchivo++)
                {
                    nombreArchivo = numeroArchivo == 0 ? @"Reporte Flujos ANT" : @"Reporte Flujos ANT_" + (numeroArchivo + 1).ToString();
                    nombreArchivoSP = ruta + nombreArchivo;

                    DataRow rowHoja = archivosDT.NewRow();
                    rowHoja["Ruta_Archivo"] = nombreArchivoSP + ".xls";
                    rowHoja["Numero_Archivo"] = numeroArchivo;
                    rowHoja["Numero_Registros"] = registrosHoja;
                    rowHoja["Ruta_ArchivoSinExtension"] = nombreArchivoSP;

                    archivosDT.Rows.Add(rowHoja);
                }

                generarArchivo = _ReservasRepository.CreacionArchivosRPT_FLUPOLTOT_hoja1(archivosDT, "TMP_HOJA_POLIZA_CALANT ORDER BY MES", "TMP_HOJA_POLIZA_CALANT");

                foreach (DataRow row in archivosDT.Rows)
                {
                    GeneracionExcelReservas_FlujosTotCarteraAnt(row[3].ToString(), "FlupolAnt " + contador.ToString());
                    File.Delete(row[0].ToString());
                    RutasArchivosDescarga.Add(row[3].ToString() + ".xlsx");
                    contador++;
                }

                if (_logicFlujosTot != null) { _log.Info("Se consultó la información correctamente."); };
                return RutasArchivosDescarga;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la información para el Reporte de Flujos de Cartera Totales : " + ex.Message);
                Console.WriteLine(ex.Message);
                return RutasArchivosDescarga;
            }
        }

        public bool GeneracionExcelReservas_FlujosTotCarteraAnt(string rutaArchivo, string nombreHoja)
        {
            try
            {
                _log.Info("Comenzara a convertir el archivo creado de BD");
                ExcelTextFormat format = new ExcelTextFormat();
                format.Delimiter = '\t';
                format.Culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
                format.Culture.DateTimeFormat.ShortDatePattern = "dd-mm-yyyy";
                format.Encoding = Encoding.GetEncoding(1252);
                format.DataTypes = new eDataTypes[] { eDataTypes.String, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number };

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                _log.Info("Ruta del archivo a convertir " + rutaArchivo);
                //create a new Excel package
                //rutaArchivo = "C:\\Users\\lizbeth.morales\\Desktop\\Vida Camara\\Cotizador\\Reporte Flujos";
                using (ExcelPackage package = new ExcelPackage(new FileInfo(rutaArchivo + ".xlsx")))
                {
                    _log.Info("Entro a convertir " + nombreHoja);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(nombreHoja);

                    worksheet.Cells["A1"].LoadFromText(new FileInfo(rutaArchivo + ".xls"), format, OfficeOpenXml.Table.TableStyles.None, true);

                    worksheet.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:J1"].Style.Fill.BackgroundColor.SetColor(Color.Black);
                    worksheet.Cells["A1:J1"].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells["A1:J1"].Style.Font.Size = 11;
                    worksheet.Cells["A1:J1"].Style.Font.Bold = true;
                    worksheet.Cells["A1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    worksheet.Cells["A1"].Value = " MES ";
                    worksheet.Cells["B1"].Value = " SI_PENSIONES ";
                    worksheet.Cells["C1"].Value = " SI_SEPELIO ";
                    worksheet.Cells["D1"].Value = " SA_PENSIONES ";
                    worksheet.Cells["E1"].Value = " SA_SEPELIO ";
                    worksheet.Cells["F1"].Value = " D_PENSIONES ";
                    worksheet.Cells["G1"].Value = " D_SEPELIO ";
                    worksheet.Cells["H1"].Value = " SI_TOTALES ";
                    worksheet.Cells["I1"].Value = " SA_TOTALES ";
                    worksheet.Cells["J1"].Value = " D_TOTALES ";

                    worksheet.Cells["A1"].AutoFitColumns();
                    worksheet.Cells["B1"].AutoFitColumns();
                    worksheet.Cells["C1"].AutoFitColumns();
                    worksheet.Cells["D1"].AutoFitColumns();
                    worksheet.Cells["E1"].AutoFitColumns();
                    worksheet.Cells["F1"].AutoFitColumns();
                    worksheet.Cells["G1"].AutoFitColumns();
                    worksheet.Cells["H1"].AutoFitColumns();
                    worksheet.Cells["I1"].AutoFitColumns();
                    worksheet.Cells["J1"].AutoFitColumns();
                    package.Save();

                    _log.Info("Termino de ingresar informacion");
                }
                _log.Info("Termino de ingresar toda la informacion");
                return true;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR EN LA GENERACION DEL ARCHIVO FlujosTotCarteraANT : " + ex);
                return false;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 18-06-2019
        /// Método que retorna la lista con los datos consultados o vacía en caso de tener algún error (para rutina antigüa detallando cada póliza).
        /// </summary>
        /// <returns>Lista con datos consultados o vacía por algún error.</returns>
        //public List<Reservas> FlujosDetTotCarteraAnt(string FecFlu)
        //{
        //    XmlConfigurator.Configure();

        //    List<Reservas> _logicFlujosDetTot = new List<Reservas>();
        //    try
        //    {
        //        _logicFlujosDetTot = _ReservasRepository.Consulta_FluDetCarteraAnt(FecFlu);
        //        if (_logicFlujosDetTot != null) { _log.Info("Se consultó la información correctamente."); };
        //        return _logicFlujosDetTot;
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info("Error al consultar la información para el Reporte de Flujos de Cartera Por Póliza: " + ex.Message);
        //        Console.WriteLine(ex.Message);
        //        return _logicFlujosDetTot;
        //    }
        //}

        public List<string> FlujosDetTotCarteraAnt(string FecFlu, string ruta)
        {
            XmlConfigurator.Configure();
            List<string> RutasArchivosDescarga = new List<string>();
            int _logicFlujosTot = 0;
            int numeroRegistros = 0;
            int numeroHojas = 0;
            int registrosRestantes = 0;
            int registrosHoja = 600000;
            decimal hojas = 0;
            string nombreArchivo = "";
            string nombreArchivoSP = "";
            bool generarArchivo;
            int contador = 1;
            try
            {
                numeroRegistros = _ReservasRepository.Consulta_FluDetCarteraAnt(FecFlu);
                DataTable archivosDT = new DataTable();
                archivosDT.Columns.Add("Ruta_Archivo", typeof(string));
                archivosDT.Columns.Add("Numero_Archivo", typeof(int));
                archivosDT.Columns.Add("Numero_Registros", typeof(int));
                archivosDT.Columns.Add("Ruta_ArchivoSinExtension", typeof(string));

                hojas = (decimal)numeroRegistros / registrosHoja;
                numeroHojas = hojas % 1 == 0 ? (int)hojas : (int)Math.Truncate(hojas) + 1;

                for (int numeroArchivo = 0; numeroArchivo < numeroHojas; numeroArchivo++)
                {
                    nombreArchivo = numeroArchivo == 0 ? @"Reporte Flujos_BenAnt" : @"Reporte Flujos_BenAnt_" + (numeroArchivo + 1).ToString();
                    nombreArchivoSP = ruta + nombreArchivo;

                    DataRow rowHoja = archivosDT.NewRow();
                    rowHoja["Ruta_Archivo"] = nombreArchivoSP + ".xls";
                    rowHoja["Numero_Archivo"] = numeroArchivo;
                    rowHoja["Numero_Registros"] = registrosHoja;
                    rowHoja["Ruta_ArchivoSinExtension"] = nombreArchivoSP;

                    archivosDT.Rows.Add(rowHoja);
                }

                generarArchivo = _ReservasRepository.CreacionArchivosRPT_FLUPOLTOT_hoja1(archivosDT, "TMP_HOJA_DETALLE_CALANT ORDER BY NUM_POLIZA", "TMP_HOJA_DETALLE_CALANT");

                foreach (DataRow row in archivosDT.Rows)
                {
                    GeneracionExcelReservas_FlujosTotCarteraHojaBenANT(row[3].ToString(), "Fluben ANT" + contador.ToString());
                    File.Delete(row[0].ToString());
                    RutasArchivosDescarga.Add(row[3].ToString() + ".xlsx");
                    contador++;
                }

                if (_logicFlujosTot != null) { _log.Info("Se consultó la información correctamente."); };
                return RutasArchivosDescarga;
            }
            catch (Exception ex)
            {
                _log.Info("Error al consultar la información para el Reporte de Flujos de Cartera Totales ANT: " + ex.Message);
                Console.WriteLine(ex.Message);
                return RutasArchivosDescarga;
            }
        }

        public bool GeneracionExcelReservas_FlujosTotCarteraHojaBenANT(string rutaArchivo, string nombreHoja)
        {
            try
            {
                _log.Info("Comenzara a convertir el archivo creado de BD");
                ExcelTextFormat format = new ExcelTextFormat();
                format.Delimiter = '\t';
                format.Culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
                format.Culture.DateTimeFormat.ShortDatePattern = "dd-mm-yyyy";
                format.Encoding = Encoding.GetEncoding(1252);
                format.DataTypes = new eDataTypes[] { eDataTypes.String, eDataTypes.String, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number, eDataTypes.Number };

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                _log.Info("Ruta del archivo a convertir " + rutaArchivo);
                //create a new Excel package
                //rutaArchivo = "C:\\Users\\lizbeth.morales\\Desktop\\Vida Camara\\Cotizador\\Reporte Flujos_Ben";
                using (ExcelPackage package = new ExcelPackage(new FileInfo(rutaArchivo + ".xlsx")))
                {
                    _log.Info("Entro a convertir " + nombreHoja);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(nombreHoja);

                    worksheet.Cells["A1"].LoadFromText(new FileInfo(rutaArchivo + ".xls"), format, OfficeOpenXml.Table.TableStyles.None, true);

                    worksheet.Cells["A1:R1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:R1"].Style.Fill.BackgroundColor.SetColor(Color.Black);
                    worksheet.Cells["A1:R1"].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells["A1:R1"].Style.Font.Size = 11;
                    worksheet.Cells["A1:R1"].Style.Font.Bold = true;
                    worksheet.Cells["A1:R1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    worksheet.Cells["A1"].Value = " NUM_POLIZA ";
                    worksheet.Cells["B1"].Value = " NUM_MES ";
                    worksheet.Cells["C1"].Value = " GASTO_SEP ";
                    worksheet.Cells["D1"].Value = " BEN_TITULAR ";
                    worksheet.Cells["E1"].Value = " BEN_CONY ";
                    worksheet.Cells["F1"].Value = " BEN_PADRE ";
                    worksheet.Cells["G1"].Value = " BEN_MADRE ";
                    worksheet.Cells["H1"].Value = " BEN_HIJO1 ";
                    worksheet.Cells["I1"].Value = " BEN_HIJO2 ";
                    worksheet.Cells["J1"].Value = " BEN_HIJO3 ";
                    worksheet.Cells["K1"].Value = " BEN_HIJO4 ";
                    worksheet.Cells["L1"].Value = " BEN_HIJO5 ";
                    worksheet.Cells["M1"].Value = " BEN_HIJO6 ";
                    worksheet.Cells["N1"].Value = " BEN_HIJO7 ";
                    worksheet.Cells["O1"].Value = " BEN_HIJO8 ";
                    worksheet.Cells["P1"].Value = " BEN_HIJO9 ";
                    worksheet.Cells["Q1"].Value = " BEN_HIJO10 ";
                    worksheet.Cells["R1"].Value = " MTO_TOTAL ";

                    worksheet.Cells["A1"].AutoFitColumns();
                    worksheet.Cells["B1"].AutoFitColumns();
                    worksheet.Cells["C1"].AutoFitColumns();
                    worksheet.Cells["D1"].AutoFitColumns();
                    worksheet.Cells["E1"].AutoFitColumns();
                    worksheet.Cells["F1"].AutoFitColumns();
                    worksheet.Cells["G1"].AutoFitColumns();
                    worksheet.Cells["H1"].AutoFitColumns();
                    worksheet.Cells["I1"].AutoFitColumns();
                    worksheet.Cells["J1"].AutoFitColumns();
                    worksheet.Cells["K1"].AutoFitColumns();
                    worksheet.Cells["L1"].AutoFitColumns();
                    worksheet.Cells["M1"].AutoFitColumns();
                    worksheet.Cells["N1"].AutoFitColumns();
                    worksheet.Cells["O1"].AutoFitColumns();
                    worksheet.Cells["P1"].AutoFitColumns();
                    worksheet.Cells["Q1"].AutoFitColumns();
                    worksheet.Cells["R1"].AutoFitColumns();
                    package.Save();

                    _log.Info("Termino de ingresar informacion");
                }
                _log.Info("Termino de ingresar toda la informacion");
                return true;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR EN LA GENERACION DEL ARCHIVO FlujosTotCartera ANT: " + ex);
                return false;
            }
        }

        #endregion
        #endregion

        #region Reporte de Cálculo de Reserva (3er proceso).
        /// <summary>
        /// José Hernández Alvarado.
        /// 28-06-2019
        /// Método para crear Rpt de Cálculo de Reservas Nuevas.
        /// </summary>
        /// <returns>Lista con registros para mostrar en el Rpt.</returns>
        public List<Reservas> RptCalculoRes()
        {
            List<Reservas> _logicCalculoRes = new List<Reservas>();

            _logicCalculoRes = _ReservasRepository.Consulta_CalculoRes();
            return _logicCalculoRes;
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 28-06-2019
        /// Método para crear Rpt de Cálculo de Reservas Antigüas.
        /// </summary>
        /// <returns>Lista con registros para mostrar en el Rpt.</returns>
        public List<Reservas> RptCalculoResAnt()
        {
            List<Reservas> _logicCalculoResAnt = new List<Reservas>();

            _logicCalculoResAnt = _ReservasRepository.Consulta_CalculoResAnt();
            return _logicCalculoResAnt;
        }
        #endregion

        #region Métodos para Rutina de Flujo de Pensiones.
        /// <summary>
        /// José Hernández Alvarado.
        /// 05-03-2019
        /// Método para validar que haya registros para realizar proceso de Cálculo de Flujos de Carteras.
        /// </summary>
        /// <returns>Respuesta de validación.</returns>
        public Response Valida_CalculoFlujos()
        {
            Response res = new Response();
            List<beDatosPol> LisPolizas = new List<beDatosPol>();
            try
            {
                LisPolizas = _rutinaReservasRepository.ConsultaPolizas("");

                if (LisPolizas.Count != 0)
                {
                    res.IsOk = true;
                    res.Message = "Información consultada con éxito";
                }
                else
                {
                    res.IsOk = false;
                    res.Message = "ERROR.";
                }
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                res.IsOk = false;
                res.Message = "Error en proceso lógico de Cálculo de Flujos.";
                return res;
                throw;
            }
        }

        public Response Valida_CurvaTasas(string FecCal)
        {
            Response res = new Response();
            List<beCurvaTasas> LisPolizas = new List<beCurvaTasas>();
            //FecCal = "20211231";
            try
            {
                FecCal = FecCal.Substring(0, 6) + "01";
                LisPolizas = _rutinaReservasRepository.ConsultaCurvas(FecCal);

                if (LisPolizas.Count > 0)
                {
                    res.IsOk = true;
                    res.Message = "Información consultada con éxito";
                }
                else
                {
                    res.IsOk = false;
                    res.Message = "No existen curvas cargadas para el periodo " + FecCal.Substring(0, 4) + "/" + FecCal.Substring(4, 2);
                }
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                res.IsOk = false;
                res.Message = "Error en proceso lógico de Consulta de Curva de Tasas.";
                return res;
                throw;
            }
        }

        /// <summary>
        /// José Hernández Alvarado
        /// 05-03-2019
        /// </summary>
        /// <param name="FecCal">Fecha de periodo ingresado en pantalla</param>
        /// <returns>Retorna respuesta exitosa o no, según los errores que puedan ocurrir o el término del proceso exitoso./returns>
        public async Task<Response> Calculo_FlujoCartera(string FecCal, string pathLog)
        {
            DateTime fecha = new DateTime();
            string strFecha = "";
            //File.WriteAllText(pathLog, "");
            XmlConfigurator.Configure();
            _log.Info("****************** CÁLCULO DE FLUJO DE PENSIONES *******************");
            resFlujos = "";
            Response res = new Response();
            try
            {
                resPeriodo = new Response();
                resPeriodo = _ReservasRepository.PeriodoRepository(FecCal);
                if (resPeriodo.Message != "Abierto")
                {
                    _log.Info(resPeriodo.Message);
                    _log.Info("PROCESO CANCELADO.");
                    res.IsOk = false;
                    res.Message = resPeriodo.Message;

                    return res;
                }

                ListCamb = _ReservasRepository.Tipo_Cambio(FecCal);
                if (ListCamb.Count == 0)
                {
                    _log.Info("No se ha encontrado el Tipo de Cambio para la fecha ingresada.");
                    _log.Info("PROCESO CANCELADO.");
                    res.IsOk = false;
                    res.Message = "No se ha encontrado el Tipo de Cambio para la fecha ingresada. \nPROCESO CANCELADO.";

                    return res;
                }
                else
                {
                    fecha = DateTime.ParseExact(FecCal, "yyyyMMdd", CultureInfo.InvariantCulture);
                    strFecha = fecha.AddDays(1).ToString("yyyyMMdd");
                    //strFecha = fecha.ToString("yyyyMMdd");

                    _log.Info("Se pasara a obtener las listas que se enviarán a la rutina.");
                    LisTabMorPar = _rutinaReservasRepository.ConsultaTablaMortalidad();
                    LisTabMorDet = _rutinaReservasRepository.ConsultaDetTablaMortalidad();
                    LisTabDinPar = _rutinaReservasRepository.ConsultaTablaMortalidadDinamicas();
                    LisTabMD = _rutinaReservasRepository.ConsultaDetTablaMortalidadDin();
                    ListaTasProm = _rutinaReservasRepository.ConsultaTasasPromedio(FecCal.Substring(0, 6) + "01");
                    ListaCurvaTasas = _rutinaReservasRepository.ConsultaCurvaTasas();
                    LisTabPol = _rutinaReservasRepository.ConsultaPolFlujos(FecCal.Substring(0, 4), Convert.ToInt32(FecCal.Substring(4, 2)), strFecha);
                    LisTabBen = _rutinaReservasRepository.ConsultaBen("");
                    tipCam = ListCamb[0].TipCambio;

                    _log.Info("Se enviarán las listas para realizar el Cálculo de Flujo de Pensiones.");
                    resFlujos = await _paginaFlujos.RutinaReservas(strFecha, LisTabMorPar, LisTabMorDet, LisTabDinPar, LisTabMD, ListaTasProm, ListaCurvaTasas, LisTabPol, LisTabBen, tipCam);

                    _log.Info("Terminó Cálculo de Flujo de Pensiones.");

                    if (resFlujos == "Flujos Guardados con éxito.")
                    {
                        res.IsOk = true;
                        res.Message = resFlujos;
                    }
                    else
                    {
                        _log.Info("ERROR: " + resFlujos);
                        res.IsOk = false;
                        res.Message = resFlujos;
                    }

                    return res;
                }

            }
            catch (Exception ex)
            {
                _log.Info("Error en proceso lógico de Cálculo de Flujos: " + ex.Message);
                Console.WriteLine(ex.Message);
                res.IsOk = false;
                res.Message = "Error en proceso lógico de Cálculo de Flujos.";
                return res;
            }

        }
        #endregion

        #region Métodos para Rutina de Reservas.
        public async Task<Response> CalculoReservasLogic(string FecCal)
        {
            XmlConfigurator.Configure();
            _log.Info("***************************** CÁLCULO RESERVAS *****************************");

            Response res = new Response();
            DateTime fecha = new DateTime();
            string strFecha = "";
            try
            {
                resPeriodo = new Response();
                resPeriodo = _ReservasRepository.PeriodoRepository(FecCal);
                if (resPeriodo.Message != "Abierto")
                {
                    _log.Info(resPeriodo.Message);
                    _log.Info("PROCESO CANCELADO.");
                    res.IsOk = false;
                    res.Message = resPeriodo.Message;

                    return res;
                }

                ListCamb = _ReservasRepository.Tipo_Cambio(FecCal);
                if (ListCamb.Count == 0)
                {
                    _log.Info("No se ha encontrado el Tipo de Cambio para la fecha ingresada.");
                    _log.Info("PROCESO CANCELADO.");
                    res.IsOk = false;
                    res.Message = "No se ha encontrado el Tipo de Cambio para la fecha ingresada. \nPROCESO CANCELADO.";

                    return res;
                }
                else
                {
                    fecha = DateTime.ParseExact(FecCal, "yyyyMMdd", CultureInfo.InvariantCulture);
                    strFecha = fecha.AddDays(-1).ToString("yyyyMM") + "01";
                    ListaFlu = CargaREsultadosFlujos(FecCal); //_rutinaReservasRepository.ConsultaCargaFlujosPensiones("", FecCal.Substring(0, 4), Convert.ToInt32(FecCal.Substring(4, 2)), strFecha);
                    //ListaFluAnt = _rutinaReservasRepository.ConsultaCargaFlujosPenAnt("");
                    LisTabPol = _rutinaReservasRepository.ConsultaPolizas("");
                    LisTabBen = _rutinaReservasRepository.ConsultaBen("");
                    ListaGtoSepelio = _rutinaReservasRepository.ConsultaGtoSepelio();
                    tipCam = ListCamb[0].TipCambio;


                    _log.Info("Se enviarán las listas para realizar el Cálculo de Reservas.");
                    resReserva = await _paginaCalReservas.CalculaResMat(strFecha, tipCam, ListaFlu, LisTabPol, LisTabBen, ListaGtoSepelio, ListaFluAnt);

                    if (resReserva == "Reserva Base Matemática con éxito.")
                    {
                        res.IsOk = true;
                        res.Message = resReserva;
                    }
                    else
                    {
                        _log.Info("ERROR: " + resReserva);
                        res.IsOk = false;
                        res.Message = resReserva;
                    }

                    return res;
                }

            }
            catch (Exception ex)
            {
                _log.Info("Error en proceso lógico de Cálculo de Reservas: " + ex.Message);
                Console.WriteLine(ex.Message);
                res.IsOk = false;
                res.Message = "Error en proceso lógico de Cálculo de Reservas.";
                return res;
            }
        }
        #endregion

        public static DataTable UseNewtonsoftJson(string sampleJson)
        {
            DataTable dataTable = new DataTable();
            if (string.IsNullOrWhiteSpace(sampleJson))
            {
                return dataTable;
            }

            dataTable = JsonConvert.DeserializeObject<DataTable>(sampleJson);

            return dataTable;
        }

        public static List<beResultadosFlujos> CargaREsultadosFlujos(string FecCal)
        {
            List<beResultadosFlujos> fluRes = new List<beResultadosFlujos>();
            DateTime fecha = DateTime.ParseExact(FecCal, "yyyyMMdd", CultureInfo.InvariantCulture);
            string strFecha = fecha.AddDays(-1).ToString("yyyyMM");
            string path = "";
            List<beFluPol_Soles> ListFluPol_s = new List<beFluPol_Soles>();
            List<beFluPol_dolares> ListFluPol_d = new List<beFluPol_dolares>();
            List<beFluBen_soles> ListFluBen_s = new List<beFluBen_soles>();
            List<beFluBen_dolares> ListFluBen_d = new List<beFluBen_dolares>();
            double tce = 0;
            string numpol = "";


            #region Carga Tablas desde los Json
            //****************************************************************
            //******************CARGA LOS FLUJOS DEL REPOSITORIO DE TEXTO************************
            //****************************************************************
            //carga los Json de la carpeta FluPol Soles
            path = System.Web.Hosting.HostingEnvironment.MapPath("~/BD_Reservas/" + strFecha + "/FlujoPolizaSol.json");
            if (File.Exists(path))
            {
                using (JsonTextReader reader = new JsonTextReader(File.OpenText(path)))
                {
                    var serializer = new JsonSerializer();

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            JObject obj = JObject.Load(reader);
                            beFluPol_Soles fluPolSol = obj.ToObject<beFluPol_Soles>();
                            ListFluPol_s.Add(fluPolSol);
                        }
                    }
                }
            }
            //carga los Json de la carpeta FluPol Dolares
            path = System.Web.Hosting.HostingEnvironment.MapPath("~/BD_Reservas/" + strFecha + "/FlujoPolizaDol.json");
            if (File.Exists(path))
            {
                using (JsonTextReader reader = new JsonTextReader(File.OpenText(path)))
                {
                    var serializer = new JsonSerializer();

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            JObject obj = JObject.Load(reader);
                            beFluPol_dolares fluPolDol = obj.ToObject<beFluPol_dolares>();
                            ListFluPol_d.Add(fluPolDol);
                        }
                    }
                }
            }

            //carga los Json de la carpeta FluBen Soles
            path = System.Web.Hosting.HostingEnvironment.MapPath("~/BD_Reservas/" + strFecha + "/FlujoBenefiSol.json");
            if (File.Exists(path))
            {
                using (JsonTextReader reader = new JsonTextReader(File.OpenText(path)))
                {
                    var serializer = new JsonSerializer();

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            JObject obj = JObject.Load(reader);
                            beFluBen_soles fluBenSol = obj.ToObject<beFluBen_soles>();
                            ListFluBen_s.Add(fluBenSol);
                        }
                    }
                }
            }

            //carga los Json de la carpeta FluBen Dolares
            path = System.Web.Hosting.HostingEnvironment.MapPath("~/BD_Reservas/" + strFecha + "/FlujoBenefiDol.json");
            if (File.Exists(path))
            {
                using (JsonTextReader reader = new JsonTextReader(File.OpenText(path)))
                {
                    var serializer = new JsonSerializer();

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            JObject obj = JObject.Load(reader);
                            beFluBen_dolares fluBenDol = obj.ToObject<beFluBen_dolares>();
                            ListFluBen_d.Add(fluBenDol);
                        }
                    }
                }
            }
            #endregion
            #region Proceso Flujos en Soles
            //****************************************************************
            //******************FLUJOS SOLES************************
            //****************************************************************
            //var joined = from Item1 in ListFluBen_s
            //             select Item1;

            //foreach (beFluBen_soles atrib in joined)
            //{
            //    numpol = atrib.numPol;
            //    tce = ListFluPol_s.Where(x => x.numPol.Contains(numpol)).Select(x => x.tasTce).FirstOrDefault();

            //    beResultadosFlujos filaRS = new beResultadosFlujos
            //    {
            //        numPol = atrib.numPol,
            //        numOrd = atrib.numOrd,
            //        numEdad = atrib.numEda,
            //        numMes = atrib.mesFlu,
            //        mtoPen = atrib.mtoPen,
            //        prcFac = atrib.fluPen,
            //        GtoSep = atrib.fluSep,
            //        fluPen = atrib.fluTot,
            //        tasTce = tce
            //    };
            //    fluRes.Add(filaRS);
            //}


            // Crear un Lookup para indexar ListFluPol_s por numPol
            var fluPolLookup = ListFluPol_s.ToLookup(x => x.numPol, x => x.tasTce);

            //var joined = from Item1 in ListFluBen_s
            //             select Item1;


            foreach (beFluBen_soles atrib in ListFluBen_s)
            {
                numpol = atrib.numPol;

                if (fluPolLookup.Contains(numpol))
                {
                    tce = fluPolLookup[numpol].FirstOrDefault();

                    beResultadosFlujos filaRS = new beResultadosFlujos
                    {
                        numPol = atrib.numPol,
                        numOrd = atrib.numOrd,
                        numEdad = atrib.numEda,
                        numMes = atrib.mesFlu,
                        mtoPen = atrib.mtoPen,
                        prcFac = atrib.fluPen,
                        GtoSep = atrib.fluSep,
                        fluPen = atrib.fluTot,
                        tasTce = tce
                    };
                    fluRes.Add(filaRS);
                }
            }

            #endregion
            #region Procesa Flujos en Dolares
            //****************************************************************
            //******************FLUJOS SOLES************************
            //****************************************************************
            //tce = ListFluPol_s.Select(x => x.tasTce).FirstOrDefault();
            //var joined2 = from Item1 in ListFluBen_d
            //              select Item1;

            //foreach (beFluBen_dolares atrib in joined2)
            //{
            //    numpol = atrib.numPol;
            //    tce = ListFluPol_s.Where(x => x.numPol.Contains(numpol)).Select(x => x.tasTce).FirstOrDefault();
            //    beResultadosFlujos filaRD = new beResultadosFlujos
            //    {
            //        numPol = atrib.numPol,
            //        numOrd = atrib.numOrd,
            //        numEdad = atrib.numEda,
            //        numMes = atrib.mesFlu,
            //        mtoPen = atrib.mtoPen,
            //        prcFac = atrib.fluPen,
            //        GtoSep = atrib.fluSep,
            //        fluPen = atrib.fluTot,
            //        tasTce = tce
            //    };
            //    fluRes.Add(filaRD);
            //}

            foreach (beFluBen_dolares atrib in ListFluBen_d)
            {
                numpol = atrib.numPol;

                if (fluPolLookup.Contains(numpol))
                {
                    tce = fluPolLookup[numpol].FirstOrDefault();

                    beResultadosFlujos filaRD = new beResultadosFlujos
                    {
                        numPol = atrib.numPol,
                        numOrd = atrib.numOrd,
                        numEdad = atrib.numEda,
                        numMes = atrib.mesFlu,
                        mtoPen = atrib.mtoPen,
                        prcFac = atrib.fluPen,
                        GtoSep = atrib.fluSep,
                        fluPen = atrib.fluTot,
                        tasTce = tce
                    };
                    fluRes.Add(filaRD);
                }
            }
            #endregion

            return fluRes;
        }
    }
}
