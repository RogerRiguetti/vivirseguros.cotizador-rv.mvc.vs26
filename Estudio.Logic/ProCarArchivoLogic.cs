using System;
using System.Xml;
using Estudio.Repository.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using System.IO;
using System.Xml.Schema;
using System.Xml.Linq;
using log4net;
using log4net.Config;
using System.Reflection;
using Estudio.Repository;

namespace Estudio.Logic
{
    public class ProCarArchivoLogic
    {
        public string TipoArchivo;
        public string scriptAFP, scriptEESS;
        public object resDatosXML;
        private static bool bandValidarErrores = false;
        public static bool bandResDatosXML = false;
        public static int numArch, NoAceptadas, Aceptadas, iCount;
        private static string usuario, NumCotizacion;
        private static bool bandXML = false;
        private static ProCarArchivoRepository _ProCarArchivoRepository = new ProCarArchivoRepository();
        private static SolicitudCotizacionesRepository _SolicitudCotizacionesRepository = new SolicitudCotizacionesRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static int posError;
        //Obtiene el Código de la Compañia guardado en la BD
        public string CodCia = _ProCarArchivoRepository.vgCodInternoCia();
        //Variable para obtener nombre del archivo XML desde la BD.
        public string nomArch = "";
        public static List<ProCarArchivo> erroresXML;
        List<ProCarArchivo> ListaRep = new List<ProCarArchivo>();
        List<ProCarArchivo> ListaRep2 = new List<ProCarArchivo>();
        public static ProCarArchivo datosErrXML = new ProCarArchivo();
        public static string docXML;
        public Response cargarXML(string __doc, string archivo, string nombre, string us, string tipo, string fecha, string hora)
        {
            //Genera los datos de la carga a registrar
            usuario = us;
            Response res = new Response();
            DateTime FecCarConv = DateTime.Parse(fecha);
            DateTime HoraConv = DateTime.Parse(hora);
            string FecCar = FecCarConv.ToString("yyyyMMdd"); //checar
            string HorCar = HoraConv.ToString("hhmmss");
            TipoArchivo = "050";
            __doc = __doc.Replace("�", "Ñ").Replace("Ã", "Ñ");
            erroresXML = new List<ProCarArchivo>();
            resDatosXML = new List<ProCarArchivo>();
            datosErrXML = new ProCarArchivo();
            docXML = __doc;
            try
            {
                _log.Info("Comenzara la validacion del XML (Resultados)");
                validarXML(__doc); //Valida schema xml
                if (bandXML == false)
                {
                    Response res2 = new Response();
                    resDatosXML = erroresXML; //Para usar los errores en el reporte de errores
                    res2.IsOk = false;
                    res2.Message = "Archivo no corresponde a Carga de Resultados";
                    return res2;
                }

                string queryNombre = "";
                queryNombre = "SELECT GLS_NOMARCH FROM PT_THIS_ENTRADA WHERE GLS_NOMARCH = '" + nombre +"'";

                _log.Info("Query ConsultaNombre entrada" + queryNombre);
                List <SolicitudesCotizacion> nombrearch;

                nombrearch = SRVDBContext<SolicitudesCotizacion>.CallSelectStatement(queryNombre, x => new SolicitudesCotizacion
                {
                    nomArchivo = x.GetString(0)
                }).ToList();

                if (nombrearch.Count() > 0)
                {
                    Response res2 = new Response();
                    res2.IsOk = false;
                    res2.Message = "El archivo ya fue cargado anteriormente";
                    return res2;
                }

                

                _log.Info("Numero de Entrada (Resultados)");
                numArch = _ProCarArchivoRepository.NumEntrada(TipoArchivo, nombre, FecCar, HorCar, usuario); //sacar el ultimo numero de archivo de la tabla de entrada
                //carga XML a tablas temporales
                _log.Info("Cargar informacion en tablas temporales (Resultados)");
                resDatosXML = cargarDatosXML(__doc); //Respuesta de los valores que se obtendran al cargar los datos
                if (bandResDatosXML == false)
                {
                    Response res2 = new Response();
                    res2.IsOk = false;
                    //Elimina los registros de las tablas TTMP por numero de archivo(Si hubo error en la carga)
                    string err = _ProCarArchivoRepository.EliminarNumArchivo(numArch);
                    if (err == "")
                    {
                        res2.Message = "Proceso Cancelado";
                    }
                    else
                    {
                        res2.Message = err;
                    }
                    return res2;
                }
                //Si existe un error en la solicitud (pt_ttmp_cierresol) transfiere a los demas registros asociados
                string errT = _ProCarArchivoRepository.transfError(numArch); //validar errores
                if (errT != "")
                {
                    Response res2 = new Response();
                    res2.IsOk = false;
                    res2.Message = errT;
                    return res2;
                }
                _log.Info("Comenzara a cargar en las tablas THIS (Resultados)");
                //Cargar a tablas THIS: Cargar la información de las Tablas Temporales TTMP sin error a las THIS
                string errThis = _ProCarArchivoRepository.CargarTHIS(numArch, TipoArchivo, nombre, FecCar, HorCar, usuario);
                if (errThis != "")
                {
                    Response res2 = new Response();
                    res2.IsOk = false;
                    res2.Message = errThis;
                    return res2;
                }
                _log.Info("Calculo de estadisticas (Resultados)");
                //Calcula Etadistica de Registros Correctos y Erroneos y los guarda en pt_this_estcarcie
                var estadisticas = _ProCarArchivoRepository.estadisticas(numArch, usuario, CodCia);
                if (estadisticas.IsOk != true)
                {
                    Response res2 = new Response();
                    res2.IsOk = false;
                    res2.Message = "Error en el calculo de cotizacion";
                    return res2;
                }
                _log.Info("Elimina registros de las tablas temporales (Resultados)");
                //Elimina los Registros Sin Error de las Tablas Temporales TTMP
                string errEliminar = _ProCarArchivoRepository.eliminaSinError();
                if (errEliminar != "")
                {
                    Response res2 = new Response();
                    res2.IsOk = false;
                    res2.Message = errEliminar;
                    return res2;
                }
                _log.Info("Actualizando cotizaciones Aceptadas (Resultados)");
                //Genera la Aceptación - Actualizando la Cotizacion a Aceptadas (TMAE)
                string aceptCot = _ProCarArchivoRepository.aceptaCotizacion(CodCia, numArch, usuario, FecCar);
                if (aceptCot != "")
                {
                    Response res2 = new Response();
                    res2.IsOk = false;
                    res2.Message = aceptCot;
                    return res2;
                }

                res.IsOk = true;
                res.Object = estadisticas.Object;
                res.Message = "El Proceso de Carga, Terminó Correctamente";
                return res;
            }
            catch (Exception ex)
            {
                Response res2 = new Response();
                res2.IsOk = false;
                res2.Message = ex.Message;
                return res2;
            }
        }
        public static void validarXML(string doc)
        {
            posError = -1;
            string ruta = AppContext.BaseDirectory + @"Resources\handler\descargaResultados22.xsd";
            StringReader sr = new StringReader(doc);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add("", ruta);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

            XmlReader reader = XmlReader.Create(sr, settings);

            bandXML = true;
            while (reader.Read())
            {
                if (reader.LocalName == "nroOperacion")
                {
                    posError = posError + 1;
                }
            };
        }
        public static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            bandXML = false;
            if (args.Severity == XmlSeverityType.Error)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(docXML);
                XmlNodeList xmlListNodo = xDoc.GetElementsByTagName("descargaResultados");
                XmlNodeList xNodo = ((XmlElement)xmlListNodo[0]).GetElementsByTagName("resultadoSol");

                ProCarArchivo datosErrXML = new ProCarArchivo();
                Console.WriteLine(args.Message);
                datosErrXML.intNumOpe = int.Parse(((XmlElement)xNodo[posError / 2]).GetElementsByTagName("nroOperacion")[0].InnerText);
                datosErrXML.strError = "107";
                datosErrXML.descError = args.Message.ToString();
                erroresXML.Add(datosErrXML);
            }
        }
        public List<ProCarArchivo> cargarDatosXML(string docXML)
        {
            Response res = new Response();
            try
            {
                List<ProCarArchivo> infoArchivo = new List<ProCarArchivo>();
                NoAceptadas = 0;
                Aceptadas = 0;
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(docXML); //abrimos el xml
                XmlNodeList xmlListNodo = xDoc.GetElementsByTagName("descargaResultados");
                XmlNodeList xNodo = ((XmlElement)xmlListNodo[0]).GetElementsByTagName("resultadoSol");
                var cont = 0;
                foreach (XmlElement nodo in xNodo)
                {
                    ProCarArchivo datos = new ProCarArchivo();
                    for (int i = 0; i < nodo.ChildNodes.Count; i++)
                    {
                        switch (nodo.ChildNodes[i].LocalName)
                        {
                            case "nroOperacion": iCount = 0; datos.intNumOpe = int.Parse(((XmlElement)xNodo[cont]).GetElementsByTagName("nroOperacion")[0].InnerText); break;
                            case "CUSPP": datos.strCussp = ((XmlElement)xNodo[cont]).GetElementsByTagName("CUSPP")[0].InnerText; break;
                            case "decisionAfiliado": datos.strDecision = ((XmlElement)xNodo[cont]).GetElementsByTagName("decisionAfiliado")[0].InnerText; break;
                            case "resulProducto":
                                XmlNodeList xresulProducto = ((XmlElement)nodo.ChildNodes[i]).ChildNodes;
                                resulProducto(xresulProducto, datos);
                                break;
                        }
                    }
                    _ProCarArchivoRepository.guardarCIERRESOL(datos, numArch, usuario);
                    infoArchivo.Add(datos);
                    cont++;
                }
                bandResDatosXML = true;

                if (scriptAFP != null) _ProCarArchivoRepository.EjecutarScript(scriptAFP);
                if (scriptEESS != null) _ProCarArchivoRepository.EjecutarScript(scriptEESS);

                return infoArchivo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                bandResDatosXML = false;
                return new List<ProCarArchivo>();
            }
        }
        public void resulProducto(XmlNodeList xNodo, ProCarArchivo datos)
        {
            try
            {
                datos.strTipRen = "";
                datos.strMoneda = "";
                datos.douanosRT = 0;
                datos.douporcentajeRVD = "0";
                datos.strModalidad = "1";
                datos.intPerGar = 0;
                datos.strCobCony = "0";
                datos.strDerCre = "";
                datos.strDerGra = "";
                datos.strParCapital = "";
                for (int i = 0; i < xNodo.Count; i++)
                {
                    switch (((XmlElement)xNodo[i]).LocalName)
                    {
                        case "modalidad": iCount = iCount + 1; datos.strTipRen = ((XmlElement)xNodo[i]).InnerText; break;
                        case "moneda": datos.strMoneda = ((XmlElement)xNodo[i]).InnerText; break;
                        case "anosRT": datos.douanosRT = int.Parse(((XmlElement)xNodo[i]).InnerText); break;
                        case "porcentajeRVD": datos.douporcentajeRVD = ((XmlElement)xNodo[i]).InnerText; break;
                        case "periodoGarantizado": datos.intPerGar = int.Parse(((XmlElement)xNodo[i]).InnerText); break;
                        case "coberturaConyuge": datos.strCobCony = ((XmlElement)xNodo[i]).InnerText; break;
                        case "derechoCrecer": datos.strDerCre = ((XmlElement)xNodo[i]).InnerText; break;
                        case "gratificacion": datos.strDerGra = ((XmlElement)xNodo[i]).InnerText; break;
                        case "particionCapital": datos.strParCapital = ((XmlElement)xNodo[i]).InnerText; break;
                        case "resultadoAFP":
                            datos.strError = "0";
                            XmlNodeList xresultadoAFP = ((XmlElement)xNodo[i]).ChildNodes;
                            scriptAFP += resultadoAFP(xresultadoAFP, datos) + "\n";
                            break;
                        case "resultadoEESS":
                            datos.strError = "0";
                            XmlNodeList xresultadoEESS = ((XmlElement)xNodo[i]).ChildNodes;
                            scriptEESS += resultadoEESS(xresultadoEESS, datos) + "\n";
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                datos.strError = "112"; //errores en solicitud recibida
            }

            _ProCarArchivoRepository.guardarResulProducto(datos, numArch, iCount, NumCotizacion);
        }


        public static string resultadoAFP(XmlNodeList xNodo, ProCarArchivo datos)
        {
            try
            {
                datos.strCodAfp = "";
                datos.strAtiende = "";
                datos.strGana = "";
                datos.strNivBas = "";
                NumCotizacion = "";
                datos.douPension = "0";
                datos.douTasa = "0";
                datos.douPriUnicaAFP = "0";
                for (int i = 0; i < xNodo.Count; i++)
                {
                    switch (((XmlElement)xNodo[i]).LocalName)
                    {
                        case "codigoAFP": datos.strCodAfp = ((XmlElement)xNodo[i]).InnerText; break;
                        case "atiende": datos.strAtiende = ((XmlElement)xNodo[i]).InnerText; break;
                        case "siGanaNoGana": datos.strGana = ((XmlElement)xNodo[i]).InnerText; break;
                        case "nivelBase": datos.strNivBas = ((XmlElement)xNodo[i]).InnerText; break;
                        case "nroCotizacion": NumCotizacion = ((XmlElement)xNodo[i]).InnerText; break;
                        case "primeraPension": datos.douPension = ((XmlElement)xNodo[i]).InnerText; break;
                        case "tasaRPyRT": datos.douTasa = ((XmlElement)xNodo[i]).InnerText; break;
                        case "primaUnicaAFP": datos.douPriUnicaAFP = ((XmlElement)xNodo[i]).InnerText; break;
                    }
                }
            }
            catch (Exception e)
            {
                datos.strError = "110"; //errores en resulProducto
            }
            //grabar en la tabla temporal de la afp
            return _ProCarArchivoRepository.guardarResultadosAFP(datos, numArch, iCount, NumCotizacion);
        }
        public static string resultadoEESS(XmlNodeList xNodo, ProCarArchivo datos)
        {
            try
            {
                datos.strCodCia = "";
                datos.strAtiende = "";
                datos.strGana = "";
                datos.strCotiza = "";
                NumCotizacion = "";
                datos.douPriUniAFPEESS = "0";
                datos.douPrima = "0";
                datos.douPension = "0";
                datos.douTasa = "0";
                datos.douPensionRT = "0";
                datos.douTasaRT = "0";
                for (int i = 0; i < xNodo.Count; i++)
                {
                    switch (((XmlElement)xNodo[i]).LocalName)
                    {
                        case "codigoEESS": datos.strCodCia = ((XmlElement)xNodo[i]).InnerText; break;
                        case "atiende": datos.strAtiende = ((XmlElement)xNodo[i]).InnerText; break;
                        case "siGanaNoGana": datos.strGana = ((XmlElement)xNodo[i]).InnerText; break;
                        case "siCotizaNoCotiza": datos.strCotiza = ((XmlElement)xNodo[i]).InnerText; break;
                        case "nroCotizacion": NumCotizacion = ((XmlElement)xNodo[i]).InnerText; break;
                        case "primaUnicaAFPEESS": datos.douPriUniAFPEESS = ((XmlElement)xNodo[i]).InnerText; break;
                        case "primaUnicaEESS": datos.douPrima = ((XmlElement)xNodo[i]).InnerText; break;
                        case "primeraPensionRV": datos.douPension = ((XmlElement)xNodo[i]).InnerText; break;
                        case "tasaInteresRV": datos.douTasa = ((XmlElement)xNodo[i]).InnerText; break;
                        case "primeraPensionRT": datos.douPensionRT = ((XmlElement)xNodo[i]).InnerText; break;
                        case "tasaInteresRT": datos.douTasaRT = ((XmlElement)xNodo[i]).InnerText; break;
                        case "primeraPensionRVD": datos.douPension = ((XmlElement)xNodo[i]).InnerText; break;
                        case "tasaInteresRVD": datos.douTasa = ((XmlElement)xNodo[i]).InnerText; break;

                    }
                }
            }
            catch (Exception e)
            {
                datos.strError = "110"; //Error al Caragr la temporal cia
            }
            //Grabar en la tabla temporal de cia
            return _ProCarArchivoRepository.guardarResultadosEESS(datos, numArch, iCount, NumCotizacion);

        }
        public Response CargarArchivoB(string numArc)
        {
            GenArMelerRepository _GenArMelerLogic = new GenArMelerRepository();
            try
            {
                Response res = new Response();
                numArch = Convert.ToInt32(numArc);
                return _ProCarArchivoRepository.CargarArchivoB(numArc);

            }
            catch (Exception ex)
            {
                Response res2 = new Response();
                res2.IsOk = false;
                res2.Message = ex.Message;
                return res2;
            }
        }

        public Response BuscarNumerosArchivo(string fecha)
        {
            GenArMelerRepository _GenArMelerLogic = new GenArMelerRepository();
            try
            {
                Response res = new Response();
                //ManFecAceptacionCotizacion Datos = new ManFecAceptacionCotizacion();

                var Datos = _ProCarArchivoRepository.getNumArchivosB(fecha);

                res.Object = Datos;
                res.Message = "";
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                Response res2 = new Response();
                res2.IsOk = false;
                res2.Message = ex.Message;
                return res2;
            }
        }
        #region MÉTODOS PARA REPORTES.

        /// <summary>
        /// José Hernández Alvarado.
        /// 24-10-2018
        /// </summary>
        /// <returns>Lista con registro de valores para reporte de resumen.</returns>
        public List<ProCarArchivo> RptResumen()
        {
            try
            {
                return _ProCarArchivoRepository.RptResumen(numArch.ToString());

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 24-10-2018
        /// </summary>
        /// <returns>Lista con registro de valores para reporte de resumen.</returns>
        public List<ProCarArchivo> RptGanadas()
        {
            nomArch = _ProCarArchivoRepository.nombreArchivo(numArch.ToString());
            try
            {
                ListaRep = _ProCarArchivoRepository.RptGanadas(numArch.ToString(), CodCia);
                for (int b = 0; b < ListaRep.Count; b++)
                {
                    ListaRep[b].Num_Orden = b + 1;
                    ListaRep[b].Nom_Archivo = nomArch;
                }
                return ListaRep;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 29-10-2018
        /// </summary>
        /// <returns>Lista con registro de valores para reporte (Solicitudes Perdidas por la Compañía CIA).</returns>
        public List<ProCarArchivo> RptPerdidasCia()
        {
            nomArch = _ProCarArchivoRepository.nombreArchivo(numArch.ToString());
            try
            {
                ListaRep = _ProCarArchivoRepository.RptPerdidasCia(numArch.ToString(), CodCia);
                for (int b = 0; b < ListaRep.Count; b++)
                {
                    ListaRep[b].Num_Orden = b + 1;
                    ListaRep[b].Nom_Archivo = nomArch;
                }
                return ListaRep;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 30-10-2018
        /// </summary>
        /// <returns>Lista con registro de valores para reporte (Solicitudes Perdidas por la Compañía AFP).</returns>
        public List<ProCarArchivo> RptPerdidasAfp()
        {
            nomArch = _ProCarArchivoRepository.nombreArchivo(numArch.ToString());
            try
            {
                ListaRep2 = _ProCarArchivoRepository.RptPerdidasAfp(numArch.ToString());
                for (int b = 0; b < ListaRep2.Count; b++)
                {
                    ListaRep2[b].Num_Orden = b + 1;
                    ListaRep2[b].Nom_Archivo = nomArch;
                }
                return ListaRep2;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ProCarArchivo> RptOtrosCia(string tipoRpt, string rptTitulo)
        {
            nomArch = _ProCarArchivoRepository.nombreArchivo(numArch.ToString());
            try
            {
                ListaRep = _ProCarArchivoRepository.RptOtrosCia(numArch.ToString(), tipoRpt);
                for (int b = 0; b < ListaRep.Count; b++)
                {
                    ListaRep[b].Num_Orden = b + 1;
                    ListaRep[b].Nom_Archivo = nomArch;
                    ListaRep[b].Titulo_Rpt = "SOLICITUDES " + rptTitulo;
                    ListaRep[b].Subtitulo_Rpt = "DETALLE DE MODALIDADES " + rptTitulo + " CON RESPUESTA DE COMPAÑÍAS";
                }
                return ListaRep;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ProCarArchivo> RptOtrosAfp(string tipoRpt, string rptTitulo)
        {
            nomArch = _ProCarArchivoRepository.nombreArchivo(numArch.ToString());
            try
            {
                ListaRep = _ProCarArchivoRepository.RptOtrosAFP(numArch.ToString(), tipoRpt);
                for (int b = 0; b < ListaRep.Count; b++)
                {
                    ListaRep[b].Num_Orden = b + 1;
                    ListaRep[b].Nom_Archivo = nomArch;
                    ListaRep[b].Titulo_Rpt = "SOLICITUDES " + rptTitulo;
                    ListaRep[b].Subtitulo_Rpt = "DETALLE DE MODALIDADES " + rptTitulo + " CON RESPUESTA DE AFP";
                }
                return ListaRep;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 25-10-2018
        /// Mensaje para validación de registros para reporte de solicitudes ganadas.
        /// </summary>
        /// <returns></returns>
        public Response validaReportes(string bandera)
        {

            try
            {
                Response res = new Response();
                res.IsOk = true;
                res.Object = _ProCarArchivoRepository.validaReportes(numArch.ToString(), CodCia, bandera);

                if(res.Object == null)
                {
                    res.Message = "No se encontro información";
                }
                else
                {
                    res.Message = "Información cargada con éxito";
                }
                
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 29-10-2018
        /// Retorna mensaje de confirmación de verificación de existencia de registros para generar reporte (Solicitudes Perdidas por la Compañía).
        /// </summary>
        /// <returns></returns>
        public Response validaRptPerdidasCia()
        {
            bool rpt;
            try
            {
                Response res = new Response();
                res.IsOk = true;
                rpt = _ProCarArchivoRepository.validaRptPerdidasCia(numArch.ToString(), CodCia);

                if (rpt == false)
                {
                    res.Message = "No se encontro información";
                }
                else
                {
                    res.Message = "Información cargada con éxito";
                }

                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 31-10-2018
        /// Retorna mensaje de confirmación de verificación dinámica de existencia de registros para generar reportes CIA (RECOTIZADAS, DESISTIDAS, CADUCADAS).
        /// </summary>
        /// <returns></returns>
        public Response validaRptsCia(string bandera)
        {
            bool resRpt;
            string tipoRpt = "";

            if(bandera == "RECIA")
            {
                tipoRpt = "RE";
            }
            if (bandera == "DECIA")
            {
                tipoRpt = "DE";
            }
            if (bandera == "CACIA")
            {
                tipoRpt = "CA";
            }

            try
            {
                Response res = new Response();
                res.IsOk = true;
                resRpt = _ProCarArchivoRepository.validaRptsCia(numArch.ToString(), tipoRpt);


                if (bandera == "RECIA")
                {
                    if (resRpt == false)
                    {
                        res.Message = "No se encontro información RE";
                    }
                    else
                    {
                        res.Message = "Información cargada con éxito RE";
                    }
                }

                if (bandera == "DECIA")
                {
                    if (resRpt == false)
                    {
                        res.Message = "No se encontro información DE";
                    }
                    else
                    {
                        res.Message = "Información cargada con éxito DE";
                    }
                }

                if (bandera == "CACIA")
                {
                    if (resRpt == false)
                    {
                        res.Message = "No se encontro información CA";
                    }
                    else
                    {
                        res.Message = "Información cargada con éxito CA";
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }

        public Response validaRptsAfp(string bandera)
        {
            bool resRpt;
            string tipoRpt = "";

            if (bandera == "REAFP")
            {
                tipoRpt = "RE";
            }
            if (bandera == "DEAFP")
            {
                tipoRpt = "DE";
            }
            if (bandera == "CAAFP")
            {
                tipoRpt = "CA";
            }

            try
            {
                Response res = new Response();
                res.IsOk = true;
                resRpt = _ProCarArchivoRepository.validaRptsAfp(numArch.ToString(), tipoRpt);


                if (bandera == "REAFP")
                {
                    if (resRpt == false)
                    {
                        res.Message = "No se encontro información RE";
                    }
                    else
                    {
                        res.Message = "Información cargada con éxito RE";
                    }
                }

                if (bandera == "DEAFP")
                {
                    if (resRpt == false)
                    {
                        res.Message = "No se encontro información DE";
                    }
                    else
                    {
                        res.Message = "Información cargada con éxito DE";
                    }
                }

                if (bandera == "CAAFP")
                {
                    if (resRpt == false)
                    {
                        res.Message = "No se encontro información CA";
                    }
                    else
                    {
                        res.Message = "Información cargada con éxito CA";
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = ex.Message;
                return res;
            }
        }

        #endregion

    }
}
