using Estudio.Logic;
using Estudio.Repository;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using Estudio.WebService.Requests;
using Estudio.WebService.Validator;
using log4net;
using log4net.Config;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;

namespace Estudio.WebService
{
    /// <summary>
    /// Servicio web para oficiales
    /// </summary>
    [WebService(Namespace = "http://eisei.net.mx/webserviceoficiales")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class WebServiceOficiales : System.Web.Services.WebService
    {
        SolicitudCotizacionLogic _realizarCalculoXML = new SolicitudCotizacionLogic();
        GenArMelerLogic _genArMeler = new GenArMelerLogic();
        CalcularAsignacionIntermediarioLogic _realizarCalculoRutina = new CalcularAsignacionIntermediarioLogic();
        CalculoCotizacionLogic _calculoCotizacionLogic = new CalculoCotizacionLogic();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        CorreosLogic _correoLogic = new CorreosLogic();
        public static string pathFileExcel;
        CotizacionLogic _cotizacionLogic = new CotizacionLogic();
        CalculoExtraOficialValidator _calculoEOValidator = new CalculoExtraOficialValidator();


        /// <summary>
        /// Omar Figueroa Flores
        /// 09-11-2018
        /// Realiza el calculo del docuemento xml para generar el calculo
        /// </summary>
        /// <param name="__docXML">Documento xml que se generara el calculo</param>
        /// <param name="nombreArchivo">Nombre del archivo xml</param>
        /// <param name="usuario">Usuario que cargara el docuemtno</param>
        /// <returns> Retorna el documento XML ya procesado</returns>
        [WebMethod]
        public string calculoXML(string __docXML, string nombreArchivo, string usuario)
        {
            try
            {
                XmlConfigurator.Configure();
                DateTime fecha = new DateTime();
                _log.Info("****************** NUEVA CARGA: " + fecha.Day + "/" + fecha.Month + "/" + fecha.Year + " - " + fecha.Hour + ":" + fecha.Minute + " *******************");
                _log.Info("YA LLEGÓ EL ARCHIVO, CON LOS PARAMETROS: ");
                _log.Info("__DOCXML: " + __docXML);
                _log.Info("NOMBREARCHIVO: " + nombreArchivo);
                _log.Info("USUARIO: " + usuario);
                _log.Info("SE MANDARA AL HILO...");
                Task.Run(() => calculoAsyn(__docXML, nombreArchivo, usuario));
                return "0";
            }
            catch (Exception)
            {
                return "-1";
            }
        }
        public async Task calculoAsyn(string __docXML, string nombreArchivo, string usuario)
        {
            webPrueba.AdmIntegracionCotizador wsEnvio = new webPrueba.AdmIntegracionCotizador();
            try
            {
                SolicitudCotizacionLogic _realizarCalculoXML = new SolicitudCotizacionLogic();
                SolicitudCotizacionesRepository _repSolicitud = new SolicitudCotizacionesRepository();
                GenArMelerLogic _genArMeler = new GenArMelerLogic();
                CalcularAsignacionIntermediarioLogic _realizarCalculoRutina = new CalcularAsignacionIntermediarioLogic();
                DateTime fechaCarga = DateTime.Now;
                _log.Info("SE EMPEZARÁ A REALIZAR LA CARGA A BASE DE DATOS");
                var parameters = new List<SqlParameter>();
                wsOficiales dNumArch = new wsOficiales();
                int numArch = 1;
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "NUMENTRADA", ParameterDirection.Input));
                dNumArch = VCEDBContext<wsOficiales>.CallStoreProcedure(StoredProcedures.CO_CatalogoCargaSolicitud, parameters, x => new wsOficiales
                {
                    numArchivo = x.GetInt32(0)
                }).FirstOrDefault();

                if (dNumArch != null)
                {
                    numArch = dNumArch.numArchivo + 1;

                }
                _repSolicitud.CargaThis_entrada(numArch, "", nombreArchivo, usuario, "030 - Solictud de Cotizaciones", fechaCarga.ToString(), fechaCarga.ToString(), 0);

                _log.Info("SE CARGO EN LA TABLA DE ENTRADA, NUMERO DE ARCHIVO UTILIZADO: " + numArch);

                var respuestaCarga = _realizarCalculoXML.wsCargarXML(__docXML, "", nombreArchivo, usuario, "030 - Solictud de Cotizaciones", fechaCarga.ToString(), fechaCarga.ToString(), numArch);
                if (respuestaCarga != "")
                {
                    _log.Info("OCURRIÓ UN ERROR EN LA CARGA DE DATOS: " + respuestaCarga);
                    _log.Info("PARAMETROS A ENVIAR:");
                    _log.Info("contenidoArchivoXML: " + respuestaCarga);
                    _log.Info("nombreArchivoXML: " + nombreArchivo);
                    string respuestaWSenvioCarga = wsEnvio.CargarCotizacionesOficiales(respuestaCarga, nombreArchivo);
                    _log.Info("SE ENVIO EL ERROR POR WEB SERVICE CORRECTAMENTE");
                    _log.Info("RESPUESTA DEL WS DE ENVIO: " + respuestaWSenvioCarga);
                    _log.Info("**************************************************************");
                    return;
                }
                else
                {
                    _log.Info("SE REALIZÓ LA CARGA DE DATOS CON EXITO");
                }

                _log.Info("SE EMPEZARÁ A REALIZAR LAS VALIDACIONES ");
                var resSNC = _realizarCalculoXML.datosSiguiente(numArch);
                if (resSNC.IsOk == false)
                {
                    _log.Info("OCURRIÓ UN ERROR EN LAS VALIDACIONES: " + resSNC.Message);
                    _log.Info("PARAMETROS A ENVIAR:");
                    _log.Info("contenidoArchivoXML: " + resSNC.Message);
                    _log.Info("nombreArchivoXML: " + nombreArchivo);
                    string respuestaWSenvioSNC = wsEnvio.CargarCotizacionesOficiales(resSNC.Message, nombreArchivo);
                    _log.Info("SE ENVIO EL ERROR POR WEB SERVICE CORRECTAMENTE");
                    _log.Info("RESPUESTA DEL WS DE ENVIO: " + respuestaWSenvioSNC);
                    _log.Info("**************************************************************");
                    return;
                }
                //parameters = new List<SqlParameter>();
                //parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "NUMARCH", ParameterDirection.Input));
                //parameters.Add(VCEDBContext<RowAffected>.AddParams("@nomArch", SqlDbType.VarChar, nombreArchivo, ParameterDirection.Input));
                //Calculo
                List<SolicitudesCotizacion> infoAfiliados = (List<SolicitudesCotizacion>)_realizarCalculoRutina.obtenerDatos(numArch).Object;
                _log.Info("COMENZARA EL CALCULO");
                string resRutina = await _realizarCalculoRutina.wsCalcularAsignacionIntermediario(infoAfiliados, numArch, usuario);
                if (resRutina != "")
                {
                    _log.Info("TERMINA CALCULO Y LA RESPUESTA ES: " + resRutina);
                    _log.Info("PARAMETROS A ENVIAR:");
                    _log.Info("contenidoArchivoXML: " + resRutina);
                    _log.Info("nombreArchivoXML: " + nombreArchivo);
                    string respuestaWSenvioRutina = wsEnvio.CargarCotizacionesOficiales(resRutina, nombreArchivo);
                    _log.Info("SE ENVIO EL ERROR POR WEB SERVICE CORRECTAMENTE");
                    _log.Info("RESPUESTA DEL WS DE ENVIO: " + respuestaWSenvioRutina);
                    _log.Info("**************************************************************");
                    return;
                }
                _log.Info("TERMINA CALCULO SIN ERROR, SE REALIZARAN VALIDACIONES SISCO");
                _calculoCotizacionLogic.MostrarGridCalculadas(numArch);
                _log.Info("HICIERON LAS VALIDACIONES SISCO");
                //crear el XML
                List<GenArMeler> informacion = (List<GenArMeler>)_realizarCalculoXML.informacionGenerar(numArch.ToString(), nombreArchivo).Object;
                _log.Info("SE OBTUVO LA INFORMACION PARA LA GENERACION DEL ARCHIVO A ENVIAR");
                string documentoXML = _genArMeler.GenerarXML(informacion, null, "W").Message;
                _log.Info("SE GENERO EL ARCHIVO, AHORA SE PASARA A ENVIAR");
                //_log.Info(documentoXML);
                //Aqui lo mandamos por otro web service
                _log.Info("PARAMETROS A ENVIAR:");
                _log.Info("contenidoArchivoXML: " + documentoXML);
                _log.Info("nombreArchivoXML: " + nombreArchivo);
                string respuestaWSenvio = wsEnvio.CargarCotizacionesOficiales(documentoXML, nombreArchivo).ToString();

                _log.Info("SE ENVIO EL ARCHIVO CORRECTAMENTE");
                _log.Info("RESPUESTA DEL WS DE ENVIO: " + respuestaWSenvio);
                _log.Info("**************************************************************");


                _log.Info("Comenzara el envio del correo electronico con la notificación");

                string asunto = "VC Oficiales - Calculo de archivo desde WebService";
                string cuerpo = "Se ha calculado correctamente el archivo " + numArch + " - " + nombreArchivo;
                List<string> correos = new List<string>();

                string queryCon = "SELECT Parametro FROM Parametros where ClaveParametro = 'CORREOWS'";

                _log.Info("Comenzara la busqueda del correo electronico");

                string DatosCon = VCEDBContext<Parametro>.CallSelectStatement(queryCon, x => new Parametro
                {
                    Elemento = x.GetString(0)
                }).FirstOrDefault().Elemento;
                _log.Info("El correo se enviará a " + DatosCon);

                string correo = DatosCon;
                correos.Add(correo);
                ExportarCalculadas(numArch);
                if (!_correoLogic.envioCorreo(cuerpo, asunto, correos, pathFileExcel))
                {
                    _log.Info("Error al enviar el correo electronico");
                    _log.Info("**************************************************************");
                    return;
                }
                System.IO.File.Delete(pathFileExcel);
            }
            catch (Exception ex)
            {
                _log.Info("OCURRIO UN ERROR: " + ex.Message);
                _log.Info("PARAMETROS A ENVIAR:");
                _log.Info("contenidoArchivoXML: " + ex.Message);
                _log.Info("nombreArchivoXML: " + nombreArchivo);
                string respuestaWSenvioErr = wsEnvio.CargarCotizacionesOficiales("KO#" + ex.Message, nombreArchivo).ToString();
                _log.Info("SE ENVIO EL ERROR POR WEB SERVICE CORRECTAMENTE");
                _log.Info("RESPUESTA DEL WS DE ENVIO: " + respuestaWSenvioErr);
                _log.Info("**************************************************************");
            }
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

        public void ExportarCalculadas(int num_Archivo)
        {
            XmlConfigurator.Configure();

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
                ListCalculadas = _calculoCotizacionLogic.ConsultaCalculadas(num_Archivo);
                ListNoCalculadas = _calculoCotizacionLogic.ConsultaNolculadas(num_Archivo);

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
                //System.IO.File.Delete(pathFile);

                _log.Info("Datos insertados al Excel exitosamente, se procederá a exportar...");
                pathFileExcel = pathFile;

                // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArch);


            }
            catch (Exception ex)
            {
                _log.Info("Error en Generación del Excel(Controller), favor de verificar: " + ex.Message);
                //return null;
            }
            #endregion
        }

        [WebMethod]
        public object calculoMejoras(string cuspp, int num_oper, string modalidad, string moneda, int aniosRT, double porcentajeRVD, int periodoGarantizado, string derechoCrecer, string gratificacion, bool prc_Tv, double prc_Com)
        {
            XmlConfigurator.Configure();
            _log.Info("****************** NUEVO CALCULO MEJORAS ******************");
            _log.Info("Parametros recibidos");
            _log.Info("CUSPP: " + cuspp);
            _log.Info("num_oper: " + num_oper);
            _log.Info("modalidad: " + modalidad);
            _log.Info("moneda: " + moneda);
            _log.Info("aniosRT: " + aniosRT);
            _log.Info("porcentajeRVD: " + porcentajeRVD);
            _log.Info("periodoGarantizado: " + periodoGarantizado);
            _log.Info("derechoCrecer: " + derechoCrecer);
            _log.Info("gratificacion: " + gratificacion);
            _log.Info("prc_Tv: " + prc_Tv);
            _log.Info("prc_Com: " + prc_Com);

            Exceptiones mod = new Exceptiones();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            Response datos = new Response();
            try
            {
                ExcepcionesLogic _ExcepcionesLogic = new ExcepcionesLogic();
                var parameters = new List<SqlParameter>();
                if (periodoGarantizado != 0)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARMODALIDADT", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@periodoGarantizado", SqlDbType.Int, periodoGarantizado, ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARMODALIDAD", ParameterDirection.Input));
                }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCUSPP", SqlDbType.VarChar, cuspp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Int, num_oper, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@modalidad", SqlDbType.VarChar, modalidad, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@moneda", SqlDbType.VarChar, moneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@aniosRT", SqlDbType.Int, aniosRT, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@porcentajeRVD", SqlDbType.Decimal, Convert.ToDecimal(porcentajeRVD), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@derechoCrecer", SqlDbType.VarChar, derechoCrecer, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@gratificacion", SqlDbType.VarChar, gratificacion, ParameterDirection.Input));

                _log.Info("SE REALIZARÁ LA BUSQUEDA DE LA MODALIDAD");
                mod = VCEDBContext<Exceptiones>.CallStoreProcedure(StoredProcedures.CO_WebService, parameters, x => new Exceptiones
                {
                    numOperacion = Convert.ToInt32(x.GetDecimal(0)),
                    dni = x.GetString(1),
                    afp = x.GetString(2),
                    cic = Convert.ToDouble(x.GetDecimal(3)),
                    asegurado = x.GetString(4),
                    cuspp = x.GetString(5),
                    sexo = x.GetString(6),
                    fechaNac = (x.GetString(7).Substring(6, 2) + "/" + x.GetString(7).Substring(4, 2) + "/" + x.GetString(7).Substring(0, 4)),
                    moneda = x.GetString(8),
                    modalidad = x.GetString(9),
                    periodoDiferido = Convert.ToString(x.GetInt32(10)),
                    rentaTMP = Convert.ToDouble(x.GetDecimal(11)),
                    perGarantizado = Convert.ToString(x.GetInt32(12)),
                    rentaEsc = Convert.ToDouble(x.GetDecimal(13)),
                    primaUnica = Convert.ToDouble(x.GetDecimal(14)),
                    renTmp1T = Convert.ToDouble(x.GetInt32(15)),
                    mtoPensio = Convert.ToDouble(x.GetDecimal(16)),
                    tasaVenta = Convert.ToDouble(x.GetDecimal(17)),
                    tir = Convert.ToDouble(x.GetDecimal(18)),
                    perdida = Convert.ToString(x.GetDecimal(19)),
                    comision = Convert.ToDouble(x.GetDecimal(20)),
                    codTipRen = x.GetString(21),
                    codTipCambio = Convert.ToDouble(x.GetDecimal(22)),
                    numCot = x.GetString(23),
                    numArchivo = x.GetInt32(24),
                    numCorrelativo = x.GetInt32(25),
                    tasaRT = x.GetDecimal(26)
                }).FirstOrDefault();
                string strBand = "";
                if (prc_Tv == true)
                {
                    strBand = "true";
                }
                else
                {
                    strBand = "false";
                }
                if (mod == null)
                {
                    _log.Info("NO SE ENCONTRÓ NINGUNA MODALIDAD CON ESOS DATOS");
                    var datosResult = new
                    {
                        error = "NO SE ENCONTRÓ NINGUNA MODALIDAD CON ESOS DATOS"
                    };
                    return ser.Serialize(datosResult);
                }
                else
                {
                    _log.Info("COMENZARÁ EL CALCULO");
                    mod.moneda = moneda;
                    datos = Task.Run(() => _ExcepcionesLogic.calculo(strBand, prc_Com.ToString(), mod, "mejoras", "")).Result;
                }
                //var datos = await _ExcepcionesLogic.calculo(strBand, prc_Com.ToString(), mod, "mejoras", "");
                if (datos.IsOk == false)
                {
                    _log.Info("OCURRIÓ UN ERROR EN EL CALCULO: " + datos.Message);
                    object datosResult;
                    if (modalidad == "RVE")
                    {
                        mod.tasaRT = 0;
                    }
                    var mensajeMej = "";
                    _log.Info("Comenzara validacion de mensaje");
                    _log.Info("Modalidad " + modalidad);
                    _log.Info("Renta Temporal " + (aniosRT * 12).ToString());
                    int aniort = aniosRT * 12;
                    _log.Info("Periodo Garantizado " + (periodoGarantizado * 12).ToString());
                    int periodoGarantizadomen = periodoGarantizado * 12;
                    if (datos.Message == "La pensión de la modalidad " + modalidad + " con " + aniort.ToString() + " renta temporal y " + periodoGarantizadomen.ToString() + " periodo garantizado ya no se puede mejorar")
                    {
                        mensajeMej = datos.Message;
                        datos.Message = "";
                    }
                    if (modalidad != "RTVD")
                    {
                        datosResult = new
                        {
                            modalidad = modalidad,
                            moneda = moneda,
                            anosRT = aniosRT.ToString(),
                            porcentajeRVD = porcentajeRVD.ToString(),
                            periodoGarantizado = periodoGarantizado.ToString(),
                            derechoCrecer = derechoCrecer,
                            gratificacion = gratificacion,
                            cotizacionEESS = new
                            {
                                siCotizaNoCotiza = "S",
                                nroCotizacion = mod.numCot,
                                primaUnicaAFPEESS = "0",
                                primaUnicaEESS = "0",
                                comision = prc_Com.ToString(),
                                primeraPensionRT = "0",
                                tasaInteresRT = mod.tasaRT.ToString(),
                                primeraPensionRV = "0",
                                tasaInteresRV = "0"
                            },
                            error = datos.Message,
                            mjsMejorada = mensajeMej
                        };
                    }
                    else
                    {
                        datosResult = new
                        {
                            modalidad = modalidad,
                            moneda = moneda,
                            anosRT = aniosRT.ToString(),
                            porcentajeRVD = porcentajeRVD.ToString(),
                            periodoGarantizado = periodoGarantizado.ToString(),
                            derechoCrecer = derechoCrecer,
                            gratificacion = gratificacion,
                            cotizacionEESS = new
                            {
                                siCotizaNoCotiza = "S",
                                nroCotizacion = mod.numCot,
                                primaUnicaAFPEESS = "0",
                                primaUnicaEESS = "0",
                                comision = prc_Com.ToString(),
                                primeraPensionRT = "0",
                                tasaInteresRT = mod.tasaRT.ToString(),
                                primeraPensionRVD = "0",
                                tasaInteresRVD = "0"
                            },
                            error = datos.Message,
                            mjsMejorada = mensajeMej
                        };
                    }
                    _log.Info("JSON MEJORAS" + ser.Serialize(datosResult));
                    return ser.Serialize(datosResult);
                }
                else
                {
                    _log.Info("CALCULO FINALIZADO CON EXITO");

                    var info = (List<string[]>)datos.Object;

                    double primeraPensionRT = 0;

                    _log.Info("Años Dif" + Convert.ToInt32(info[0][2]));
                    _log.Info("Moneda" + moneda);
                    if (aniosRT > 0)
                    {
                        if (moneda == "S/." || moneda == "S/.Aj.")
                        {
                            _log.Info("Segundo Tramo " + Convert.ToDouble(info[0][8].Replace(",", "")));
                            primeraPensionRT = (Convert.ToDouble(info[0][8].Replace(",", "")) * 2);
                        }
                        else
                        {
                            primeraPensionRT = ((Convert.ToDouble(info[0][8].Replace(",", "")) * 2) * mod.codTipCambio);
                        }
                        if (modalidad == "RVE")
                        {
                            mod.tasaRT = 0;
                        }
                    }
                    else
                    {
                        mod.tasaRT = 0;
                    }
                    _log.Info("Prima Unica " + Convert.ToDouble(info[0][6].Replace(",", "")));
                    _log.Info("CIC " + mod.cic);
                    var primaUnicaAFP = (mod.cic - Convert.ToDouble(info[0][6].Replace(",", ""))).ToString();
                    var primaUnicaES = (mod.cic - Convert.ToDouble(primaUnicaAFP)).ToString();

                    object datosResult;
                    if (modalidad != "RTVD")
                    {
                        datosResult = new
                        {
                            modalidad = modalidad,
                            moneda = moneda,
                            anosRT = aniosRT.ToString(),
                            porcentajeRVD = porcentajeRVD.ToString(),
                            periodoGarantizado = periodoGarantizado.ToString(),
                            derechoCrecer = derechoCrecer,
                            gratificacion = gratificacion,
                            cotizacionEESS = new
                            {
                                siCotizaNoCotiza = "S",
                                nroCotizacion = mod.numCot,
                                primaUnicaAFPEESS = primaUnicaAFP.ToString(),//(Convert.ToDouble(info[0][6]) - Convert.ToDouble(info[0][2]) * Convert.ToDouble(info[0][7].Replace(",",""))).ToString(),
                                primaUnicaEESS = primaUnicaES.ToString(),//info[0][6].Replace(",", ""),
                                comision = prc_Com.ToString(),
                                primeraPensionRT = primeraPensionRT.ToString(),
                                tasaInteresRT = mod.tasaRT.ToString(),
                                primeraPensionRV = info[0][8].Replace(",", ""),
                                tasaInteresRV = info[0][9]
                            }

                        };
                    }
                    else
                    {
                        datosResult = new
                        {
                            modalidad = modalidad,
                            moneda = moneda,
                            anosRT = aniosRT.ToString(),
                            porcentajeRVD = porcentajeRVD.ToString(),
                            periodoGarantizado = periodoGarantizado.ToString(),
                            derechoCrecer = derechoCrecer,
                            gratificacion = gratificacion,
                            cotizacionEESS = new
                            {
                                siCotizaNoCotiza = "S",
                                nroCotizacion = mod.numCot,
                                primaUnicaAFPEESS = primaUnicaAFP.ToString(),//(Convert.ToDouble(info[0][6]) - Convert.ToDouble(info[0][2]) * Convert.ToDouble(info[0][7].Replace(",",""))).ToString(),
                                primaUnicaEESS = primaUnicaES.ToString(),//info[0][6].Replace(",", ""),
                                comision = prc_Com.ToString(),
                                primeraPensionRT = primeraPensionRT.ToString(),
                                tasaInteresRT = mod.tasaRT.ToString(),
                                primeraPensionRVD = info[0][8].Replace(",", ""),
                                tasaInteresRVD = info[0][9]
                            }

                        };
                    }
                    _log.Info("JSON A RETORNAR: " + ser.Serialize(datosResult));
                    return ser.Serialize(datosResult);
                }
            }
            catch (Exception ex)
            {
                _log.Info("OCURRIO UN ERROR: " + ex.Message.ToString());
                if (modalidad == "RVE")
                {
                    mod.tasaRT = 0;
                }
                object datosResult;
                if (modalidad != "RTVD")
                {
                    datosResult = new
                    {
                        modalidad = modalidad,
                        moneda = moneda,
                        anosRT = aniosRT.ToString(),
                        porcentajeRVD = porcentajeRVD.ToString(),
                        periodoGarantizado = periodoGarantizado.ToString(),
                        derechoCrecer = derechoCrecer,
                        gratificacion = gratificacion,
                        cotizacionEESS = new
                        {
                            siCotizaNoCotiza = "S",
                            nroCotizacion = mod.numCot,
                            primaUnicaAFPEESS = "0",
                            primaUnicaEESS = "0",
                            comision = prc_Com.ToString(),
                            primeraPensionRT = "0",
                            tasaInteresRT = mod.tasaRT.ToString(),
                            primeraPensionRV = "0",
                            tasaInteresRV = "0"
                        },
                        error = ex.Message
                    };
                }
                else
                {
                    datosResult = new
                    {
                        modalidad = modalidad,
                        moneda = moneda,
                        anosRT = aniosRT.ToString(),
                        porcentajeRVD = porcentajeRVD.ToString(),
                        periodoGarantizado = periodoGarantizado.ToString(),
                        derechoCrecer = derechoCrecer,
                        gratificacion = gratificacion,
                        cotizacionEESS = new
                        {
                            siCotizaNoCotiza = "S",
                            nroCotizacion = mod.numCot,
                            primaUnicaAFPEESS = "0",
                            primaUnicaEESS = "0",
                            comision = prc_Com.ToString(),
                            primeraPensionRT = "0",
                            tasaInteresRT = mod.tasaRT.ToString(),
                            primeraPensionRVD = "0",
                            tasaInteresRVD = "0"
                        },
                        error = ex.Message
                    };
                }
                _log.Info("JSON A RETORNAR: " + ser.Serialize(datosResult));
                return ser.Serialize(datosResult);
            }
        }
        /// <summary>
        /// Omar Figueroa Flores
        /// 09-10-2018
        /// Registra documento xml
        /// </summary>
        /// <param name="pClave">Indica que accion se realizara</param>
        /// <param name="docXML">Parametro que contendra el documento xml en una cadena</param>
        /// <returns>"Retorna el numero de archivo con el cual se podra realizar una busqued"</returns>
        [WebMethod]
        public string grabarExcepcionExterna(string cuspp, int num_operacion, string modalidad, string moneda, int aniosRT, double porcentajeRVD, int periodoGarantizado, string derechoCrecer, string gratificacion, string motivo)
        {

            XmlConfigurator.Configure();
            ExcepcionesRepository _ExcepcionesRepository = new ExcepcionesRepository();
            _log.Info("****************** NUEVO CALCULO EXCEPCIONES ******************");
            _log.Info("Parametros recibidos");
            _log.Info("CUSPP: " + cuspp);
            _log.Info("num_oper: " + num_operacion);
            _log.Info("modalidad: " + modalidad);
            _log.Info("moneda: " + moneda);
            _log.Info("aniosRT: " + aniosRT);
            _log.Info("porcentajeRVD: " + porcentajeRVD);
            _log.Info("periodoGarantizado: " + periodoGarantizado);
            _log.Info("derechoCrecer: " + derechoCrecer);
            _log.Info("gratificacion: " + gratificacion);

            try
            {
                string band = "";
                wsOficiales wsCor = new wsOficiales();
                var parameters = new List<SqlParameter>();
                if (periodoGarantizado != 0)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARMODALIDADT", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@periodoGarantizado", SqlDbType.Int, periodoGarantizado, ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARMODALIDAD", ParameterDirection.Input));
                }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCUSPP", SqlDbType.VarChar, cuspp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Int, num_operacion, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@modalidad", SqlDbType.VarChar, modalidad, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@moneda", SqlDbType.VarChar, moneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@aniosRT", SqlDbType.Int, aniosRT, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@porcentajeRVD", SqlDbType.Decimal, Convert.ToDecimal(porcentajeRVD), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@derechoCrecer", SqlDbType.VarChar, derechoCrecer, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@gratificacion", SqlDbType.VarChar, gratificacion, ParameterDirection.Input));
                _log.Info("SE BUSCARA LA MODALIDAD");
                wsCor = VCEDBContext<wsOficiales>.CallStoreProcedure(StoredProcedures.CO_WebService, parameters, x => new wsOficiales
                {
                    Correlativo = x.GetInt32(25)
                }).FirstOrDefault();
                Exceptiones valida = _ExcepcionesRepository.VALIDACIONGUARDAR(num_operacion, wsCor.Correlativo);
                if (valida.Ind_Estado != "I")
                {
                    _log.Info("No puede ser enviada esta modalidad (Archivo enviado al Meler)");
                    band = "KO#NO No puede ser enviada esta modalidad (Archivo enviado al Meler)";
                }
                else
                {
                    if (wsCor != null)
                    {
                        _log.Info("SE GUARDARAN LOS DATOS DE LA MODALIDAD");
                        parameters = new List<SqlParameter>();
                        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GRABAREXCEPCIONEXTERNA", ParameterDirection.Input));
                        parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCUSPP", SqlDbType.VarChar, cuspp, ParameterDirection.Input));
                        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumCorr", SqlDbType.Int, wsCor.Correlativo, ParameterDirection.Input));
                        parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Int, num_operacion, ParameterDirection.Input));
                        parameters.Add(VCEDBContext<RowAffected>.AddParams("@pMotivo", SqlDbType.VarChar, motivo, ParameterDirection.Input));

                        VCEDBContext<wsOficiales>.CallStoreProcedure(StoredProcedures.CO_WebService, parameters, x => new wsOficiales
                        {
                        }).FirstOrDefault();
                        _log.Info("DATOS GUARDADOS CON EXITO, SE RETORNARÁ: success");
                        band = "success";
                    }
                    else
                    {
                        _log.Info("NO SE ENCONTRÓ NINGUNA MODALIDAD");
                        band = "KO#NO SE ENCONTRÓ NINGUNA MODALIDAD";
                    }
                }
                return band;
            }
            catch (Exception er)
            {
                _log.Info("OCURRIÓ UN ERROR: " + er.Message);
                return "Error al grabar: " + er.Message;
            }
        }
        /// <summary>
        /// Omar Figueroa Flores
        /// 23-11-2018
        /// Guarda los datos calculados de la nueva mejora en excepciones y retorna sus valores
        /// </summary>
        /// <param name="cuspp">Codigo CUSPP para identificar la modalidad</param>
        /// <param name="num_Oper">Numero de operacion para identificar la modalidad</param>
        /// <param name="num_Cor">Numero de correlativo para identificar la modalidad</param>
        /// <param name="prc_Tv">Porcentaje de tasa de venta para realizar el calculo</param>
        /// <param name="prc_Com">porcentaje de comicion para realzar el calculo</param>
        /// <returns>"Retorna un vector con los datos calculados"</returns>
        [WebMethod]
        public object guardarMejoras(string cuspp, int num_oper, string modalidad, string moneda, int aniosRT, double porcentajeRVD, int periodoGarantizado, string derechoCrecer, string gratificacion, bool prc_Tv, double prc_Com)
        {
            XmlConfigurator.Configure();
            _log.Info("****************** NUEVO GUARDAR MEJORAS ******************");
            _log.Info("Parametros recibidos");
            _log.Info("CUSPP: " + cuspp);
            _log.Info("num_oper: " + num_oper);
            _log.Info("modalidad: " + modalidad);
            _log.Info("moneda: " + moneda);
            _log.Info("aniosRT: " + aniosRT);
            _log.Info("porcentajeRVD: " + porcentajeRVD);
            _log.Info("periodoGarantizado: " + periodoGarantizado);
            _log.Info("derechoCrecer: " + derechoCrecer);
            _log.Info("gratificacion: " + gratificacion);
            _log.Info("prc_Tv: " + prc_Tv);
            _log.Info("prc_Com: " + prc_Com);

            Exceptiones mod = new Exceptiones();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            Response datos = new Response();
            try
            {
                ExcepcionesLogic _ExcepcionesLogic = new ExcepcionesLogic();
                var parameters = new List<SqlParameter>();
                if (periodoGarantizado != 0)
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARMODALIDADT", ParameterDirection.Input));
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@periodoGarantizado", SqlDbType.Int, periodoGarantizado, ParameterDirection.Input));
                }
                else
                {
                    parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "BUSCARMODALIDAD", ParameterDirection.Input));
                }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@codCUSPP", SqlDbType.VarChar, cuspp, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@numOperacion", SqlDbType.Int, num_oper, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@modalidad", SqlDbType.VarChar, modalidad, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@moneda", SqlDbType.VarChar, moneda, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@aniosRT", SqlDbType.Int, aniosRT, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@porcentajeRVD", SqlDbType.Decimal, Convert.ToDecimal(porcentajeRVD), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@derechoCrecer", SqlDbType.VarChar, derechoCrecer, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@gratificacion", SqlDbType.VarChar, gratificacion, ParameterDirection.Input));

                _log.Info("SE REALIZARÁ LA BUSQUEDA DE LA MODALIDAD");

                mod = VCEDBContext<Exceptiones>.CallStoreProcedure(StoredProcedures.CO_WebService, parameters, x => new Exceptiones
                {
                    numOperacion = Convert.ToInt32(x.GetDecimal(0)),
                    dni = x.GetString(1),
                    afp = x.GetString(2),
                    cic = Convert.ToDouble(x.GetDecimal(3)),
                    asegurado = x.GetString(4),
                    cuspp = x.GetString(5),
                    sexo = x.GetString(6),
                    fechaNac = (x.GetString(7).Substring(6, 2) + "/" + x.GetString(7).Substring(4, 2) + "/" + x.GetString(7).Substring(0, 4)),
                    moneda = x.GetString(8),
                    modalidad = x.GetString(9),
                    periodoDiferido = Convert.ToString(x.GetInt32(10)),
                    rentaTMP = Convert.ToDouble(x.GetDecimal(11)),
                    perGarantizado = Convert.ToString(x.GetInt32(12)),
                    rentaEsc = Convert.ToDouble(x.GetDecimal(13)),
                    primaUnica = Convert.ToDouble(x.GetDecimal(14)),
                    renTmp1T = Convert.ToDouble(x.GetInt32(15)),
                    mtoPensio = Convert.ToDouble(x.GetDecimal(16)),
                    tasaVenta = Convert.ToDouble(x.GetDecimal(17)),
                    tir = Convert.ToDouble(x.GetDecimal(18)),
                    perdida = Convert.ToString(x.GetDecimal(19)),
                    comision = Convert.ToDouble(x.GetDecimal(20)),
                    codTipRen = x.GetString(21),
                    codTipCambio = Convert.ToDouble(x.GetDecimal(22)),
                    numCot = x.GetString(23),
                    numArchivo = x.GetInt32(24),
                    numCorrelativo = x.GetInt32(25),
                    tasaRT = x.GetDecimal(26)
                }).FirstOrDefault();
                string strBand = "";
                if (prc_Tv == true)
                {
                    strBand = "true";
                }
                else
                {
                    strBand = "false";
                }
                if (mod == null)
                {
                    _log.Info("NO SE ENCONTRÓ NINGUNA MODALIDAD CON ESOS DATOS");
                    var datosResult = new
                    {
                        error = "NO SE ENCONTRÓ NINGUNA MODALIDAD CON ESOS DATOS"
                    };
                    return ser.Serialize(datosResult);
                }
                else
                {
                    _log.Info("COMENZARÁ EL CALCULO");
                    mod.moneda = moneda;
                    datos = Task.Run(() => _ExcepcionesLogic.calculo(strBand, prc_Com.ToString(), mod, "mejoras", "")).Result;
                }
                if (datos.IsOk == false)
                {
                    if (modalidad == "RVE")
                    {
                        mod.tasaRT = 0;
                    }
                    object datosResult;
                    if (modalidad != "RTVD")
                    {
                        datosResult = new
                        {
                            modalidad = modalidad,
                            moneda = moneda,
                            anosRT = aniosRT.ToString(),
                            porcentajeRVD = porcentajeRVD.ToString(),
                            periodoGarantizado = periodoGarantizado.ToString(),
                            derechoCrecer = derechoCrecer,
                            gratificacion = gratificacion,
                            cotizacionEESS = new
                            {
                                siCotizaNoCotiza = "S",
                                nroCotizacion = mod.numCot,
                                primaUnicaAFPEESS = "0",
                                primaUnicaEESS = "0",
                                comision = prc_Com.ToString(),
                                primeraPensionRT = "0",
                                tasaInteresRT = mod.tasaRT.ToString(),
                                primeraPensionRV = "0",
                                tasaInteresRV = "0"
                            },
                            error = datos.Message
                        };
                    }
                    else
                    {
                        datosResult = new
                        {
                            modalidad = modalidad,
                            moneda = moneda,
                            anosRT = aniosRT.ToString(),
                            porcentajeRVD = porcentajeRVD.ToString(),
                            periodoGarantizado = periodoGarantizado.ToString(),
                            derechoCrecer = derechoCrecer,
                            gratificacion = gratificacion,
                            cotizacionEESS = new
                            {
                                siCotizaNoCotiza = "S",
                                nroCotizacion = mod.numCot,
                                primaUnicaAFPEESS = "0",
                                primaUnicaEESS = "0",
                                comision = prc_Com.ToString(),
                                primeraPensionRT = "0",
                                tasaInteresRT = mod.tasaRT.ToString(),
                                primeraPensionRVD = "0",
                                tasaInteresRVD = "0"
                            },
                            error = datos.Message
                        };
                    }
                    _log.Info("JSON A RETORNAR: " + ser.Serialize(datosResult));
                    return ser.Serialize(datosResult);
                }
                else
                {
                    _log.Info("CALCULO FINALIZADO CON EXITO");
                    var info = (List<string[]>)datos.Object;
                    Exceptiones datos1 = new Exceptiones();

                    datos1.numOperacion = mod.numOperacion;
                    datos1.modalidad = info[0][1];
                    datos1.primaUnica = Convert.ToDouble(info[0][6].Replace(",", ""));
                    datos1.mtoPensio = Convert.ToDouble(info[0][8].Replace(",", ""));
                    datos1.tasaVenta = Convert.ToDouble(info[0][9]);
                    datos1.tir = Convert.ToDouble(info[0][10]);
                    datos1.perdida = info[0][11];
                    datos1.comision = Convert.ToDouble(info[0][12]);
                    datos1.numCorrelativo = mod.numCorrelativo;
                    datos1.mtoSumPension = Convert.ToDouble(info[0][14]);
                    datos1.periodoDiferido = aniosRT.ToString();
                    datos1.primerTramo = porcentajeRVD.ToString();

                    Exceptiones datos2 = new Exceptiones();

                    datos2.MTO_AJUSTEIPC = info[0][17];
                    datos2.MTO_CTAINDAFP = info[0][18];
                    datos2.MTO_RENTATMPAFP = info[0][19];
                    datos2.MTO_RESMAT = info[0][20];
                    datos2.PRC_TASATCE = info[0][21];
                    datos2.FecCal = info[0][22];
                    datos2.MTO_PENANUAL = info[0][23];
                    datos2.MTO_PENSIONGAR = info[0][24];
                    datos2.MTO_PRIUNISIM = info[0][25];
                    datos2.MTO_RMGTOSEP = info[0][26];
                    datos2.MTO_RMGTOSEPRV = info[0][27];
                    datos2.MTO_VALREAJUSTEMEN = info[0][28];
                    datos2.MTO_VALREAJUSTETRI = info[0][29];
                    datos2.MTO_VALPREPENTMP = info[0][30];

                    var resGuardad = _ExcepcionesLogic.Guardar(datos1, "mejoras", datos2);
                    var mensajeMej = "";

                    if (resGuardad.IsOk == false && resGuardad.Message.Contains("No puede ser guardado es un caso SISCO, la pensión minima es "))
                    {
                        mensajeMej = resGuardad.Message;
                        resGuardad.Message = "";

                        if (modalidad == "RVE")
                        {
                            mod.tasaRT = 0;
                        }
                        object datosResult;
                        if (modalidad != "RTVD")
                        {
                            datosResult = new
                            {
                                modalidad = modalidad,
                                moneda = moneda,
                                anosRT = aniosRT.ToString(),
                                porcentajeRVD = porcentajeRVD.ToString(),
                                periodoGarantizado = periodoGarantizado.ToString(),
                                derechoCrecer = derechoCrecer,
                                gratificacion = gratificacion,
                                cotizacionEESS = new
                                {
                                    siCotizaNoCotiza = "S",
                                    nroCotizacion = mod.numCot,
                                    primaUnicaAFPEESS = "0",
                                    primaUnicaEESS = "0",
                                    comision = prc_Com.ToString(),
                                    primeraPensionRT = "0",
                                    tasaInteresRT = mod.tasaRT.ToString(),
                                    primeraPensionRV = "0",
                                    tasaInteresRV = "0"
                                },
                                error = resGuardad.Message,
                                mjsMejorada = mensajeMej
                            };
                        }
                        else
                        {
                            datosResult = new
                            {
                                modalidad = modalidad,
                                moneda = moneda,
                                anosRT = aniosRT.ToString(),
                                porcentajeRVD = porcentajeRVD.ToString(),
                                periodoGarantizado = periodoGarantizado.ToString(),
                                derechoCrecer = derechoCrecer,
                                gratificacion = gratificacion,
                                cotizacionEESS = new
                                {
                                    siCotizaNoCotiza = "S",
                                    nroCotizacion = mod.numCot,
                                    primaUnicaAFPEESS = "0",
                                    primaUnicaEESS = "0",
                                    comision = prc_Com.ToString(),
                                    primeraPensionRT = "0",
                                    tasaInteresRT = mod.tasaRT.ToString(),
                                    primeraPensionRVD = "0",
                                    tasaInteresRVD = "0"
                                },
                                error = resGuardad.Message,
                                mjsMejorada = mensajeMej
                            };
                        }
                        _log.Info("JSON A RETORNAR: " + ser.Serialize(datosResult));
                        return ser.Serialize(datosResult);
                    }
                    else if (resGuardad.IsOk == false)
                    {
                        if (modalidad == "RVE")
                        {
                            mod.tasaRT = 0;
                        }
                        object datosResult;
                        if (modalidad != "RTVD")
                        {
                            datosResult = new
                            {
                                modalidad = modalidad,
                                moneda = moneda,
                                anosRT = aniosRT.ToString(),
                                porcentajeRVD = porcentajeRVD.ToString(),
                                periodoGarantizado = periodoGarantizado.ToString(),
                                derechoCrecer = derechoCrecer,
                                gratificacion = gratificacion,
                                cotizacionEESS = new
                                {
                                    siCotizaNoCotiza = "S",
                                    nroCotizacion = mod.numCot,
                                    primaUnicaAFPEESS = "0",
                                    primaUnicaEESS = "0",
                                    comision = prc_Com.ToString(),
                                    primeraPensionRT = "0",
                                    tasaInteresRT = mod.tasaRT.ToString(),
                                    primeraPensionRV = "0",
                                    tasaInteresRV = "0"
                                },
                                error = resGuardad.Message
                            };
                        }
                        else
                        {
                            datosResult = new
                            {
                                modalidad = modalidad,
                                moneda = moneda,
                                anosRT = aniosRT.ToString(),
                                porcentajeRVD = porcentajeRVD.ToString(),
                                periodoGarantizado = periodoGarantizado.ToString(),
                                derechoCrecer = derechoCrecer,
                                gratificacion = gratificacion,
                                cotizacionEESS = new
                                {
                                    siCotizaNoCotiza = "S",
                                    nroCotizacion = mod.numCot,
                                    primaUnicaAFPEESS = "0",
                                    primaUnicaEESS = "0",
                                    comision = prc_Com.ToString(),
                                    primeraPensionRT = "0",
                                    tasaInteresRT = mod.tasaRT.ToString(),
                                    primeraPensionRVD = "0",
                                    tasaInteresRVD = "0"
                                },
                                error = resGuardad.Message
                            };
                        }
                        _log.Info("JSON A RETORNAR: " + ser.Serialize(datosResult));
                        return ser.Serialize(datosResult);
                    }
                    else
                    {
                        double primeraPensionRT = 0;

                        _log.Info("Años Dif" + Convert.ToInt32(info[0][2]));
                        _log.Info("Moneda" + moneda);
                        if (aniosRT > 0)
                        {
                            if (moneda == "S/." || moneda == "S/.Aj.")
                            {
                                _log.Info("Segundo Tramo " + Convert.ToDouble(info[0][8].Replace(",", "")));
                                primeraPensionRT = (Convert.ToDouble(info[0][8].Replace(",", "")) * 2);
                            }
                            else
                            {
                                primeraPensionRT = ((Convert.ToDouble(info[0][8].Replace(",", "")) * 2) * mod.codTipCambio);
                            }
                            if (modalidad == "RVE")
                            {
                                mod.tasaRT = 0;
                            }
                        }
                        else
                        {
                            mod.tasaRT = 0;
                        }
                        var primaUnicaAFP = (mod.cic - Convert.ToDouble(info[0][6].Replace(",", ""))).ToString();
                        var primaUnicaES = (mod.cic - Convert.ToDouble(primaUnicaAFP)).ToString();
                        object datosResult;
                        if (modalidad != "RTVD")
                        {
                            datosResult = new
                            {
                                modalidad = modalidad,
                                moneda = moneda,
                                anosRT = aniosRT.ToString(),
                                porcentajeRVD = porcentajeRVD.ToString(),
                                periodoGarantizado = periodoGarantizado.ToString(),
                                derechoCrecer = derechoCrecer,
                                gratificacion = gratificacion,
                                cotizacionEESS = new
                                {
                                    siCotizaNoCotiza = "S",
                                    nroCotizacion = mod.numCot,
                                    primaUnicaAFPEESS = primaUnicaAFP.ToString(),//(Convert.ToDouble(info[0][6]) - Convert.ToDouble(info[0][2]) * Convert.ToDouble(info[0][7].Replace(",",""))).ToString(),
                                    primaUnicaEESS = primaUnicaES.ToString(),//info[0][6].Replace(",", ""),
                                    comision = prc_Com.ToString(),
                                    primeraPensionRT = primeraPensionRT.ToString(),
                                    tasaInteresRT = mod.tasaRT.ToString(),
                                    primeraPensionRV = info[0][8].Replace(",", ""),
                                    tasaInteresRV = info[0][9]
                                }

                            };
                        }
                        else
                        {
                            datosResult = new
                            {
                                modalidad = modalidad,
                                moneda = moneda,
                                anosRT = aniosRT.ToString(),
                                porcentajeRVD = porcentajeRVD.ToString(),
                                periodoGarantizado = periodoGarantizado.ToString(),
                                derechoCrecer = derechoCrecer,
                                gratificacion = gratificacion,
                                cotizacionEESS = new
                                {
                                    siCotizaNoCotiza = "S",
                                    nroCotizacion = mod.numCot,
                                    primaUnicaAFPEESS = primaUnicaAFP.ToString(),//(Convert.ToDouble(info[0][6]) - Convert.ToDouble(info[0][2]) * Convert.ToDouble(info[0][7].Replace(",",""))).ToString(),
                                    primaUnicaEESS = primaUnicaES.ToString(),//info[0][6].Replace(",", ""),
                                    comision = prc_Com.ToString(),
                                    primeraPensionRT = primeraPensionRT.ToString(),
                                    tasaInteresRT = mod.tasaRT.ToString(),
                                    primeraPensionRVD = info[0][8].Replace(",", ""),
                                    tasaInteresRVD = info[0][9]
                                }

                            };
                        }

                        _log.Info("JSON A RETORNAR: " + ser.Serialize(datosResult));
                        return ser.Serialize(datosResult);
                        //return "DATOS GUARDADOS CON EXITO!";
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Info("OCURRIO UN ERROR: " + ex.Message.ToString());
                if (modalidad == "RVE")
                {
                    mod.tasaRT = 0;
                }
                object datosResult;
                if (modalidad != "RTVD")
                {
                    datosResult = new
                    {
                        modalidad = modalidad,
                        moneda = moneda,
                        anosRT = aniosRT.ToString(),
                        porcentajeRVD = porcentajeRVD.ToString(),
                        periodoGarantizado = periodoGarantizado.ToString(),
                        derechoCrecer = derechoCrecer,
                        gratificacion = gratificacion,
                        cotizacionEESS = new
                        {
                            siCotizaNoCotiza = "S",
                            nroCotizacion = mod.numCot,
                            primaUnicaAFPEESS = "0",
                            primaUnicaEESS = "0",
                            comision = prc_Com.ToString(),
                            primeraPensionRT = "0",
                            tasaInteresRT = mod.tasaRT.ToString(),
                            primeraPensionRV = "0",
                            tasaInteresRV = "0"
                        },
                        error = ex.Message
                    };
                }
                else
                {
                    datosResult = new
                    {
                        modalidad = modalidad,
                        moneda = moneda,
                        anosRT = aniosRT.ToString(),
                        porcentajeRVD = porcentajeRVD.ToString(),
                        periodoGarantizado = periodoGarantizado.ToString(),
                        derechoCrecer = derechoCrecer,
                        gratificacion = gratificacion,
                        cotizacionEESS = new
                        {
                            siCotizaNoCotiza = "S",
                            nroCotizacion = mod.numCot,
                            primaUnicaAFPEESS = "0",
                            primaUnicaEESS = "0",
                            comision = prc_Com.ToString(),
                            primeraPensionRT = "0",
                            tasaInteresRT = mod.tasaRT.ToString(),
                            primeraPensionRVD = "0",
                            tasaInteresRVD = "0"
                        },
                        error = ex.Message
                    };
                }
                _log.Info("JSON A RETORNAR: " + ser.Serialize(datosResult));
                return ser.Serialize(datosResult);
            }
        }

        [WebMethod]
        [XmlInclude(typeof(CalculoExtraOficialRequest))]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object CalculoExtraOficial(CalculoExtraOficialRequest request)
        {
            //XmlConfigurator.Configure();
            var ser = new JavaScriptSerializer();
            var response = new Response();

            var idsBeneficiarios = new List<string>();
            var idsModalidades = new List<string>();
            char bandera = 'C';

            try
            {
                var validationRules = _calculoEOValidator.validator(request);

                if (validationRules.Count() > 0)
                {
                    response.IsOk = false;
                    response.Object = new { Errors = validationRules };
                    return ser.Serialize(response);
                }

                var IdAsesor = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdAsesor", request.Asesor);
                var IdSexo = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdSexo", request.Asegurado.Genero);
                var IdTipoDocumento = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdTipoDocumento", request.Asegurado.NombreDocumento);
                var IdDepartamento = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdDepartamento", request.Asegurado.Departamento);
                var IdProvincia = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdProvincia", request.Asegurado.Provincia);
                var IdDistrito = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdDistrito", request.Asegurado.Distrito);
                var IdAfp = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdAfp", request.Asegurado.TipoAFP);
                var IdPension = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("IdPension", request.Asegurado.TipoPension);
                var CodigoPension = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("CodigoPension", request.Asegurado.TipoPension);
                var PorAfp = _cotizacionLogic.ConsultarDataCotizacionExtraOficial("PorAfp", request.Asegurado.TipoAFP);

                var cotizacion = new Cotizacion
                {
                    IdCotizacion = request.IdCotizacionJubilare,
                    Documento = request.Asegurado.NumeroDocumento,
                    CUSPP = request.Asegurado.CUSPP,
                    Nombres = request.Asegurado.Nombres,
                    ApellidoPaterno = request.Asegurado.ApellidoPaterno,
                    ApellidoMaterno = request.Asegurado.ApellidoMaterno,
                    FechaNacimiento = request.Asegurado.FechaNacimiento,
                    FechaNacimientoStr = null,
                    Cic = request.MontoCIC,
                    FechaDevengue = request.Asegurado.FechaDevengue,
                    FechaDevengueStr = null,
                    FechaEstudio = DateTime.Now,
                    FechaEstudioStr = null,
                    GastoSepelio = request.GastoSepelio,
                    TipoCambio = request.TipoCambio,
                    FechaCotizacion = DateTime.Now,
                    FechaCotizacionStr = null,
                    IdAsesor = int.Parse(IdAsesor),
                    Asesor = request.Asesor,
                    IdSexo = int.Parse(IdSexo),
                    IdTipoDocumento = int.Parse(IdTipoDocumento),
                    TipoDocumento = request.Asegurado.NombreDocumento,
                    IdDepartamento = int.Parse(IdDepartamento),
                    IdProvincia = int.Parse(IdProvincia),
                    IdDistrito = int.Parse(IdDistrito),
                    IdAfp = int.Parse(IdAfp),
                    IdPension = int.Parse(IdPension),
                    Afp = request.Asegurado.TipoAFP,
                    CodigoPension = CodigoPension,
                    ClaveSexo = request.Asegurado.Genero,
                    PorAfp = PorAfp,
                    Estado = 1
                };

                foreach (var ids in request.Beneficiario)
                {
                    idsBeneficiarios.Add(ids.IdBeneficiarioJubilare.ToString());
                }

                foreach (var ids in request.Modalidad)
                {
                    idsModalidades.Add(ids.IdModalidadJubilare.ToString());
                }

                response = _cotizacionLogic.RegistrarModificarCotizacion(bandera, cotizacion, idsBeneficiarios, idsModalidades);

                return ser.Serialize(response);
            }
            catch (Exception ex)
            {
                _log.Info("Error en el cálculo extra oficial" + ex.Message);
                return null;
            }
        }
    }
}