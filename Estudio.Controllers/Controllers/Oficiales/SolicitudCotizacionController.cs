using System;
using Estudio.Logic;
using System.Collections.Generic;
using System.Web.Mvc;
using Estudio.Repository.Core.Domain;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain.Views;
using log4net;
using log4net.Config;
using SpreadsheetLight;
using System.Reflection;

namespace Estudio.Controllers.Controllers.Oficiales
{

    public static class GlobalVar
    {
        static object _globalValue;// = new List<SolicitudCotizacion>();;
        static object _globalValueAsesores;
        public static int GlobalNumArchivo;
        public static string GlobalNomArchivo;
        public static List<SolicitudesCotizacion> datosDeCarga;

        public static object GlobalValue
        {
            get
            {
                return _globalValue;
            }
            set
            {
                _globalValue = value;
            }
        }
        public static object GlobalValueAsesores
        {
            get
            {
                return _globalValueAsesores;
            }
            set
            {
                _globalValueAsesores = value;
            }
        }
    }
    public class SolicitudCotizacionController : Controller
    {
        public int NumArchivo { get; set; }

        CalculoCotizacionLogic _CalculoCotizacionLogic = new CalculoCotizacionLogic();
        SolicitudCotizacionLogic _SolicitudCotizacionLogic = new SolicitudCotizacionLogic();
        CalcularAsignacionIntermediarioLogic _CalcularAsignacionIntermediario = new CalcularAsignacionIntermediarioLogic();
        GenArMelerLogic _genArMeler = new GenArMelerLogic();
        CotizacionLogic _cotizacionLogic = new CotizacionLogic();
        public static string pathFileExcel;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        CorreosLogic _correoLogic = new CorreosLogic();

        #region No-Transaccionales 
        public ActionResult Index()
        {
            //clXML pruebas = new clXML();
            //pruebas.calculoMejoras();

            ViewBag.usuario = getUsuario();
            return View();
        }
        public ActionResult AsignacionIntermediario(int numArchivo)
        {
            int idUsuario = Convert.ToInt32(this.Session["UserId"]);
            string rol = this.Session["Name"].ToString();
            int asesor = this.Session["Asesor"] == null ? 0 : Convert.ToInt32(this.Session["Asesor"]);

            ViewBag.usuario = getUsuario();
            cargarDatos(numArchivo);
            cargarAsesores();
            // ViewBag.cmbxAsesores = new SelectList(_cotizacionLogic.Asesores(idUsuario, rol), "Id", "NombreCompleto", asesor);
            ViewBag.cmbxAsesores = GlobalVar.GlobalValueAsesores;
            ViewBag.Informacion = GlobalVar.GlobalValue;
            ViewBag.NumArchivo = numArchivo; //Session["NumArch"].ToString();
            ViewBag.NomArchivo = cargarCabeza(numArchivo);
            return View();
        }
        public ActionResult CalculoCotizacion(int numArch)
        {
            ViewBag.NumArchivo = numArch.ToString();
            ViewBag.NomArchivo = cargarCabeza(numArch);
            ViewBag.usuario = getUsuario();
            return View();
        }

        public String getUsuario()
        {
            try { return Convert.ToString(this.Session["Account"]); }
            catch (Exception) { return null; }
        }
        #endregion
        #region Transaccionales
        [HandleError]
        public ActionResult cargarXML(string __doc, string archivo, string nombre, string us, string tipo, string fecha, string hora)
        {
            try
            {
                //clXML _clXML = new clXML();
                //_clXML.calculo(__doc, nombre, us);
                //return Json("");

                GlobalVar.GlobalNomArchivo = nombre;
                Session["NomArch"] = nombre;
                var resultado = _SolicitudCotizacionLogic.cargarXML(__doc, archivo, nombre, us, tipo, fecha, hora);
                GlobalVar.datosDeCarga = _SolicitudCotizacionLogic.resDatosXML;
                //GlobalVar.GlobalNumArchivo = _SolicitudCotizacionLogic.numeroDeArchivo;
                Session["NumArch"] = _SolicitudCotizacionLogic.numeroDeArchivo;
                NumArchivo = _SolicitudCotizacionLogic.numeroDeArchivo;

                return Json(resultado);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string cargarCabeza(int numArchivo)
        {
            try
            {
                return _CalcularAsignacionIntermediario.datosCabeza(numArchivo);

                //GlobalVar.GlobalNomArchivo = nom.strNom;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void cargarDatos(int numArchivo)
        {
            try
            {
                var resultado = _CalcularAsignacionIntermediario.obtenerDatos(numArchivo/*GlobalVar.GlobalNumArchivo*/);
                GlobalVar.GlobalValue = resultado.Object;
            }
            catch (Exception)
            {

            }
        }
        public void cargarAsesores()
        {
            try
            {
                var resultado = _CalcularAsignacionIntermediario.asesores();
                GlobalVar.GlobalValueAsesores = resultado.Object;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 13-09-2018
        /// metodo que calcula
        /// </summary>
        /// <param name="informacion"></param>
        /// <returns></returns>
        public async Task<ActionResult> CalcularAsignacionIntermediario(List<SolicitudesCotizacion> informacion, int numArch)
        {
            try
            {
                var usuario = Convert.ToString(this.Session["Account"]);
                var resultado = await _CalcularAsignacionIntermediario.CalcularAsignacionIntermediario(informacion, numArch/*GlobalVar.GlobalNumArchivo*/, usuario);
                GlobalVar.GlobalValue = resultado.Object;
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 09-10-2018
        /// </summary>
        /// <returns>Retorna lista con registros de cotizaciones "Calculadas" para mostrarlas en el Grid correspondiente.</returns>
        public ActionResult ListaCalculadas(int numA)
        {
            try
            {
                var resultado = _CalculoCotizacionLogic.MostrarGridCalculadas(numA/*GlobalVar.GlobalNumArchivo*/);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 09-10-2018
        /// </summary>
        /// <returns>Retorna lista con registros de cotizaciones "No Calculadas" para mostrarlas en el Grid correspondiente.</returns>
        public ActionResult ListaNoCalculadas(int numA)
        {
            try
            {
                var resultado = _CalculoCotizacionLogic.MostrarGridNoCalculadas(numA/*GlobalVar.GlobalNumArchivo*/);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 09-10-2018
        /// Método para ejecutar consulta y mostrar reporte con filas de la tabla de cotizaciones "Calculadas".
        /// </summary>
        /// <param name="strNum">Número de archivo XML leído (String).</param>
        /// <param name="strNom">Nombre de archivo XML leído (String).</param>
        /// <returns></returns>
        public ActionResult RptCalculadas(string strNum)
        {
            string strNom = cargarCabeza(int.Parse(strNum));
            var resultExportacion = _CalculoCotizacionLogic.RptCalculadas(strNum, strNom);
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_SolOfeCal.rpt"));
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
        /// 09-10-2018
        /// Método para ejecutar consulta y mostrar reporte con filas de la tabla de cotizaciones "No Calculadas".
        /// </summary>
        /// <param name="strNum">Número de archivo XML leído (String).</param>
        /// <param name="strNom">Nombre de archivo XML leído (String).</param>
        /// <returns></returns>
        public ActionResult RptNoCalculadas(string strNum)
        {
            string strNom = cargarCabeza(int.Parse(strNum));
            var resultExportacion = _CalculoCotizacionLogic.RptNoCalculadas(strNum, strNom);
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_SolOfeNoEnv.rpt"));
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
        /// 09-10-2018
        /// </summary>
        public ActionResult crearXML(string strNum, string strNom)
        {
            clXML _clXML = new clXML();
            List<GenArMeler> informacion = (List<GenArMeler>)_SolicitudCotizacionLogic.informacionGenerar(strNum, strNom).Object;
            string documento = _genArMeler.GenerarXML(informacion, null, "W").Message;
            //Envio
            var respuesta = _clXML.cargaCotizacionesOficiales(documento, strNom);
            _log.Info("XML:" + documento);
            return Json(respuesta);
        }


        /// <summary>
        /// Omar Figueroa Flores
        /// 09-10-2018
        /// </summary>
        public void crearElementoXML(XmlDocument docXML, XmlElement elementoPri, string txtNodo, XmlElement elementoNuevo, XmlElement elementoSec)
        {
            if (txtNodo == "")
            {
                elementoPri.AppendChild(elementoNuevo);
            }
            else
            {
                XmlText txtElemento = docXML.CreateTextNode(txtNodo);
                elementoNuevo.AppendChild(txtElemento);
                elementoSec.AppendChild(elementoNuevo);
            }
        }

        public ActionResult ExportarCalculadas(int num_Archivo)
        {
            XmlConfigurator.Configure();

            #region Excel con NPOI
            //try
            //{
            //    //Nombre de archivo y path
            //    string path = Server.MapPath("\\Files\\");

            //    string nombreArchivo = "SolicitudesCalculadas";
            //    List<string> nombresHojas = new List<string>();

            //    ExportarExcel expExcel = new ExportarExcel();

            //    Random r = new Random();
            //    int aleatorio3 = r.Next(100, 999);

            //    var date = DateTime.Today;

            //    nombreArchivo = string.Format("{0}_{1}-{2}.xls", nombreArchivo, date.ToString("yyyyMMdd"), aleatorio3.ToString());

            //    Descargas datosDescarga = new Descargas();
            //    datosDescarga.NombreArchivo = nombreArchivo;
            //    datosDescarga.Path = path;
            //    datosDescarga.Estatus = 1; //descargando


            //    //inserttamos la descarga para el historial
            //    // descargaMetodos.InsertDescarga(datosDescarga); *****

            //    //Obtenemos la lista de la exportacion a excel
            //    var resultExportacion = _CalculoCotizacionLogic.ExportarExcelCalculadas(num_Archivo);
            //    var resultExportacionN = _CalculoCotizacionLogic.ExportarExcelNoCalculadas(num_Archivo);

            //    List<List<List<Dictionary<string, object>>>> listaResultado = new List<List<List<Dictionary<string, object>>>>();
            //    listaResultado.Add(resultExportacion);
            //    listaResultado.Add(resultExportacionN);

            //    nombresHojas.Add("Solicitudes Calculadas");
            //    nombresHojas.Add("Solicitudes No Calculadas");

            //    if (resultExportacion.Count > 0)
            //    {
            //        //Se ejecuta el metodo del excel dentro de un hilo
            //        Thread myThread = new Thread(delegate ()
            //        {

            //            ExportarExcel exportar = new ExportarExcel();
            //            exportar.ExportExcel(path + nombreArchivo, listaResultado, nombresHojas);

            //        });

            //        myThread.Start();
            //        myThread.Join();
            //    }
            //    else
            //    {


            //    }

            //    byte[] fileBytes = System.IO.File.ReadAllBytes(path + nombreArchivo);
            //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
            #endregion

            #region Excel con Plantilla
            try
            {
                _log.Info("Se comenzará a generar el Excel, por favor espere...");
                Random r = new Random();
                int aleatorio3 = r.Next(100, 999);
                var date = DateTime.Today;
                string nombreArch = "Solicitudes Calculadas - " + date.ToString("yyyyMMdd") + "_" + aleatorio3.ToString() + ".xlsx";
                string pathFile = Server.MapPath("\\Files\\") + nombreArch;

                System.IO.File.Copy(Server.MapPath("\\Files\\Solicitudes Calculadas.xlsx"), pathFile);

                List<AsignacionIntermediario> ListCalculadas = new List<AsignacionIntermediario>();
                List<AsignacionIntermediario> ListNoCalculadas = new List<AsignacionIntermediario>();
                ListCalculadas = _CalculoCotizacionLogic.ConsultaCalculadas(num_Archivo);
                ListNoCalculadas = _CalculoCotizacionLogic.ConsultaNolculadas(num_Archivo);

                SLDocument sl = new SLDocument(pathFile);

                sl.SelectWorksheet("Solicitudes Calculadas");
                for (int i = 0; i < ListCalculadas.Count; i++)
                {
                    sl.SetCellValue(2 + i, 1, ListCalculadas[i].Num_Orden);
                    sl.SetCellValue(2 + i, 2, Convert.ToInt32(ListCalculadas[i].Num_Cot));
                    sl.SetCellValue(2 + i, 3, ListCalculadas[i].Num_Correlativo);
                    sl.SetCellValue(2 + i, 4, ListCalculadas[i].Num_Operacion);
                    sl.SetCellValue(2 + i, 5, ListCalculadas[i].CUSPP);
                    sl.SetCellValue(2 + i, 6, ListCalculadas[i].Tipo_Pension);
                    sl.SetCellValue(2 + i, 7, ListCalculadas[i].Tipo_Renta);
                    sl.SetCellValue(2 + i, 8, ListCalculadas[i].MesesDif);
                    sl.SetCellValue(2 + i, 9, ListCalculadas[i].Modalidad);
                    sl.SetCellValue(2 + i, 10, ListCalculadas[i].MesesGar);
                    sl.SetCellValue(2 + i, 11, ListCalculadas[i].CobCony);
                    sl.SetCellValue(2 + i, 12, ListCalculadas[i].Cod_DerCre);
                    sl.SetCellValue(2 + i, 13, ListCalculadas[i].Cod_DerGra);
                    sl.SetCellValue(2 + i, 14, ListCalculadas[i].Cod_Moneda);
                    sl.SetCellValue(2 + i, 15, (double)ListCalculadas[i].Prc_MinimoTir); //TasaTIR
                    sl.SetCellValue(2 + i, 16, ListCalculadas[i].Prc_RentaEsc);
                    sl.SetCellValue(2 + i, 17, (double)ListCalculadas[i].TasaV); //TasaVta
                    sl.SetCellValue(2 + i, 18, (double)ListCalculadas[i].Prc_Pension); //Mto_Pension
                    sl.SetCellValue(2 + i, 19, ListCalculadas[i].Prc_TasaRPRT);
                    sl.SetCellValue(2 + i, 20, ListCalculadas[i].Mto_PensionRT);
                    sl.SetCellValue(2 + i, 21, ListCalculadas[i].Prima_Unica);
                    sl.SetCellValue(2 + i, 22, ListCalculadas[i].Prc_PerCon);
                    sl.SetCellValue(2 + i, 23, ListCalculadas[i].Intermediario);
                    sl.SetCellValue(2 + i, 24, ListCalculadas[i].Prc_CorCom);
                    sl.SetCellValue(2 + i, 25, ListCalculadas[i].Ind_Mej);
                    sl.SetCellValue(2 + i, 26, ListCalculadas[i].Gls_Region);
                }

                sl.SelectWorksheet("Solicitudes No Calculadas");
                for (int i = 0; i < ListNoCalculadas.Count; i++)
                {
                    sl.SetCellValue(2 + i, 1, Convert.ToInt32(ListNoCalculadas[i].Num_Cot));
                    sl.SetCellValue(2 + i, 2, ListNoCalculadas[i].Num_Correlativo);
                    sl.SetCellValue(2 + i, 3, ListNoCalculadas[i].Num_Operacion);
                    sl.SetCellValue(2 + i, 4, ListNoCalculadas[i].CUSPP);
                    sl.SetCellValue(2 + i, 5, ListNoCalculadas[i].Tipo_Pension);
                    sl.SetCellValue(2 + i, 6, ListNoCalculadas[i].Tipo_Renta);
                    sl.SetCellValue(2 + i, 7, ListNoCalculadas[i].MesesDif);
                    sl.SetCellValue(2 + i, 8, ListNoCalculadas[i].Modalidad);
                    sl.SetCellValue(2 + i, 9, ListNoCalculadas[i].MesesGar);
                    sl.SetCellValue(2 + i, 10, ListNoCalculadas[i].CIC);
                    sl.SetCellValue(2 + i, 11, ListNoCalculadas[i].Cod_Moneda);
                    sl.SetCellValue(2 + i, 12, ListNoCalculadas[i].Prc_RentaEsc);
                    sl.SetCellValue(2 + i, 13, ListNoCalculadas[i].Prima_Unica);
                    sl.SetCellValue(2 + i, 14, ListNoCalculadas[i].Intermediario);
                    sl.SetCellValue(2 + i, 15, Convert.ToInt32(ListNoCalculadas[i].Cod_Rechazo));
                    sl.SetCellValue(2 + i, 16, ListNoCalculadas[i].Error_Descrip);
                }

                sl.SaveAs(pathFile);


                byte[] fileBytes = System.IO.File.ReadAllBytes(pathFile);
                System.IO.File.Delete(pathFile);

                _log.Info("Datos insertados al Excel exitosamente, se procederá a exportar...");
                //pathFileExcel = pathFile;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArch);
            }
            catch (Exception ex)
            {
                _log.Info("Error en Generación del Excel(Controller), favor de verificar: " + ex.Message);
                return null;
            }
            #endregion
        }

        /// <summary>
        /// Omar Figueroa Flores
        /// 28-11-2018
        /// Método para mostrar el reporte de errores de la carga de solicitud
        /// </summary>
        /// <param name="strNum">Número de archivo XML leído (String).</param>
        /// <param name="strNom">Nombre de archivo XML leído (String).</param>
        /// <returns></returns>
        public ActionResult Reporte_De_Errores_En_La_Carga()
        {
            List<SolicitudesCotizacion> datos = new List<SolicitudesCotizacion>();
            datos = GlobalVar.datosDeCarga;
            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_ErroresCargaCotizacion.rpt"));
            rpt.SetDataSource(datos);
            try
            {
                Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                return File(stream, "application/pdf");
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// José Hernández Alvarado
        /// 13-12-2018
        /// Método para mostrar reporte generado de resumen de carga de cotizaciones.
        /// </summary>
        /// <returns></returns>
        public ActionResult Reporte_De_Resumen_De_Carga()
        {
            var resultExportacion = _SolicitudCotizacionLogic.RptResumenCarga();

            ReportDocument rpt = new ReportDocument();
            //rpt.FileName = Server.MapPath("~/Resources/reports/CotizacionReporte.rpt");
            rpt.Load(Server.MapPath("~/Resources/reports/PT_Rpt_ProCarSolRes.rpt"));
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

        public ActionResult datosSiguiente(int numArchS)
        {
            try
            {
                return Json(_SolicitudCotizacionLogic.datosSiguiente(numArchS));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }

}
