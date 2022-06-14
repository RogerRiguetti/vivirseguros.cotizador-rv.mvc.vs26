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
using log4net.Config;
using log4net;
using System.Reflection;
using Estudio.Repository.Core.Domain.Views;

namespace Estudio.Logic
{
    public class SolicitudCotizacionLogic
    {
        public List<SolicitudesCotizacion> resDatosXML;
        public static List<SolicitudesCotizacion> erroresXML;
        public static SolicitudesCotizacion datosErrXML = new SolicitudesCotizacion();
        private static bool bandValidarErrores = false;
        public static int numArch;
        public static string nomArch;
        private static string usuario;
        public static int posError;
        public static string docXML;
        private static bool bandXML = false;
        public int numeroDeArchivo;
        private static SolicitudCotizacionesRepository _SolicitudCotizacionesRepository = new SolicitudCotizacionesRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Response cargarXML(string __doc, string archivo, string nombre, string us, string tipo, string fecha, string hora)
        {
            usuario = us;
            Response res = new Response();
            __doc = __doc.Replace("�", "Ñ").Replace("Ã", "Ñ");
            datosErrXML = new SolicitudesCotizacion();
            erroresXML = new List<SolicitudesCotizacion>();
            resDatosXML = new List<SolicitudesCotizacion>();
            nomArch = nombre;
            docXML = __doc;
            posError = -1;
            int numArchent = 0;
            try
            {
                validarXML(__doc); //Valida schema xml
                _log.Info("Comenzara a Verificar el formato del XML");
                if (bandXML == false)
                {
                    Response res2 = new Response();
                    res2.IsOk = false;
                    erroresXML[0].nomArchivo = nombre;
                    resDatosXML = erroresXML; //Para usar los errores en el reporte de errores
                    if (resDatosXML.Count == 0)
                    {
                        res2.Message = "Archivo no corresponde a Carga de Solicitudes";
                    }
                    else
                    {
                        res2.Message = "No se puede cargar el archivo porque contiene errores";
                    }
                    return res2;
                }
                _log.Info("La validacion fue correcta");
                _log.Info("Entrara a la parte de BD");

                numArch = _SolicitudCotizacionesRepository.NumEntrada(archivo, nombre, us, tipo, fecha, hora, numArchent); //sacar el ultimo numero de archivo de la tabla de entrada
                numeroDeArchivo = numArch;
                cargarDatosXML(__doc); //Respuesta de los valores que se obtendran al cargar los datos

                _SolicitudCotizacionesRepository.transfError(numArch); //Si la Carga tiene error, replica el codigo de error al Fondo, Afiliado, Beneficiario y Producto.
                resDatosXML = erroresXML; //Para usar los errores en el reporte de errores

                _SolicitudCotizacionesRepository.cargaTHIS(numArch, archivo, nombre, us, tipo, fecha, hora, numArchent); //CARGA LA INFO DE LAS TEMPORALES SIN ERROR A LAS THIS
                var resEstadisticas = _SolicitudCotizacionesRepository.estadisticas(numArch, us); //CALCULA ESTADISTICA DE REGISTROS CORRECTOS Y ERRONEOS
                _SolicitudCotizacionesRepository.eliminaSinError(); //ELIMINA SIN ERROR DE TTMP
                _SolicitudCotizacionesRepository.traspasoCotizacion(numArch, us); //pasa los datos a tablas maestras
                _SolicitudCotizacionesRepository.validacionesFinales(numArch, usuario);
                res = resEstadisticas;
                res.Message = "El Proceso de Carga Terminó Correctamente";
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
        public string wsCargarXML(string __doc, string archivo, string nombre, string us, string tipo, string fecha, string hora, int numArchivoWS)
        {
            usuario = us;
            Response res = new Response();
            try
            {
                __doc = __doc.Replace("�", "Ñ").Replace("Ã", "Ñ");
                numArch = numArchivoWS; //_SolicitudCotizacionesRepository.NumEntrada(); //sacar el ultimo numero de archivo de la tabla de entrada
                                        //_SolicitudCotizacionesRepository.transfError(numArch); //Si la Carga tiene error, replica el codigo de error al Fondo, Afiliado, Beneficiario y Producto.
                int numArchent = 0;
                nomArch = nombre;
                _log.Info("INICIA LA CARGA DE DATOS A LAS TMP");
                cargarDatosXML(__doc); //Respuesta de los valores que se obtendran al cargar los datos
                _log.Info("INICIA LA CARGA DE DATOS A LAS THIS");
                _SolicitudCotizacionesRepository.cargaTHIS(numArchivoWS, archivo, nombre, us, tipo, fecha, hora, numArchent); //CARGA LA INFO DE LAS TEMPORALES SIN ERROR A LAS THIS
                                                                                                                              //var resEstadisticas = _SolicitudCotizacionesRepository.estadisticas(numArch, us); //CALCULA ESTADISTICA DE REGISTROS CORRECTOS Y ERRONEOS
                _log.Info("INICIA LA ELIMMINACION SIN ERROR DE TTMP");
                _SolicitudCotizacionesRepository.eliminaSinError(); //ELIMINA SIN ERROR DE TTMP
                _log.Info("INICIA EL TRASPASO DE LAS COTIZACIONES A LAS TABLAS MAESTRAS");
                _SolicitudCotizacionesRepository.traspasoCotizacion(numArchivoWS, us); //pasa los datos a tablas maestras
                _log.Info("INICIA LA EJECUCION DE LAS VALIDACIONES FINALES");
                _SolicitudCotizacionesRepository.validacionesFinales(numArchivoWS, usuario);
                return "";

                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        public static void validarXML(string doc)
        {

            string ruta = AppContext.BaseDirectory + @"Resources\handler\descargaSolicitudesEESS25.xsd";
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
                XmlNodeList xmlListNodo = xDoc.GetElementsByTagName("descargaSolicitudesEESS");
                XmlNodeList xNodo = ((XmlElement)xmlListNodo[0]).GetElementsByTagName("solicitudRecibidaEESS");

                SolicitudesCotizacion datosErrXML = new SolicitudesCotizacion();
                Console.WriteLine(args.Message);
                datosErrXML.nomArchivo = nomArch;
                datosErrXML.intNumOpe = int.Parse(((XmlElement)xNodo[posError/2]).GetElementsByTagName("nroOperacion")[0].InnerText);
                datosErrXML.strError = "107";
                datosErrXML.descError = args.Message.ToString();
                erroresXML.Add(datosErrXML);
            }
        }
        public void cargarDatosXML(string docXML)
        {
            int numOpe = 0;

            Response res = new Response();
            List<SolicitudesCotizacion> infoArchivo = new List<SolicitudesCotizacion>();

            if (bandValidarErrores == false)
            {
                XmlDocument xDoc = new XmlDocument();
                // xDoc.Load(docXML); //abrimos el xml
                xDoc.LoadXml(docXML);

                XmlNodeList xmlListNodo = xDoc.GetElementsByTagName("descargaSolicitudesEESS");
                XmlNodeList xNodo = ((XmlElement)xmlListNodo[0]).GetElementsByTagName("solicitudRecibidaEESS");
                var cont = 0;
                foreach (XmlElement nodo in xNodo)
                {
                    try
                    {
                        SolicitudesCotizacion datosSC = new SolicitudesCotizacion();
                        datosErrXML.numArchivo = numArch;
                        datosErrXML.nomArchivo = nomArch;
                        for (int i = 0; i < nodo.ChildNodes.Count; i++)
                        {
                            switch (nodo.ChildNodes[i].LocalName)
                            {
                                case "nroOperacion":
                                    nroOperacion(((XmlElement)xNodo[cont]).GetElementsByTagName("nroOperacion")[0].InnerText, datosSC);
                                    numOpe = datosSC.intNumOpe;
                                    break;
                                case "afiliado":
                                    XmlNodeList xAfiliado = ((XmlElement)nodo.ChildNodes[i]).ChildNodes;
                                    afiliado(xAfiliado, datosSC);
                                    break;
                                case "fondo":
                                    XmlNodeList xFondo = ((XmlElement)nodo.ChildNodes[i]).ChildNodes;
                                    fondo(xFondo, datosSC);
                                    break;
                                case "beneficiario":
                                    XmlNodeList xBeneficiario = ((XmlElement)nodo.ChildNodes[i]).ChildNodes;
                                    beneficiario(xBeneficiario, datosSC);
                                    break;
                                case "producto":
                                    XmlNodeList xProducto = ((XmlElement)nodo.ChildNodes[i]).ChildNodes;
                                    producto(xProducto, datosSC);
                                    break;
                                default:
                                    elResto(nodo.ChildNodes[i].LocalName, xNodo, datosSC, cont);
                                    break;
                            }
                        }
                        _SolicitudCotizacionesRepository.cargaSol(numArch, datosSC, usuario);
                        infoArchivo.Add(datosSC);
                        cont++;
                    }
                    catch (Exception)
                    {
                        datosErrXML = new SolicitudesCotizacion();
                        datosErrXML.intNumOpe = numOpe;
                        datosErrXML.strError = "102";
                        datosErrXML.descError = "Errores en solicitud recibida";
                        erroresXML.Add(datosErrXML);
                    }
                }
            }
            else //por si no es para cargas de solicitudes
            {
                datosErrXML.strError = "99";
                datosErrXML.descError = "Archivo no corresponde a Carga de Solicitudes";
                erroresXML.Add(datosErrXML);
            }
        }


        public static void nroOperacion(string intNumOpe, SolicitudesCotizacion datosSC)
        {
            datosSC.intNumOpe = int.Parse(intNumOpe);
            if (_SolicitudCotizacionesRepository.ExisteNumOpe(int.Parse(intNumOpe)) == true)
            {
                datosErrXML = new SolicitudesCotizacion();
                datosSC.strError = "100"; //Solicitud ya fue cargada
                datosErrXML.intNumOpe = datosSC.intNumOpe;
                datosErrXML.strError = "100";
                datosErrXML.descError = "Solicitud ya fue cargada";
                erroresXML.Add(datosErrXML);
            }
            if (_SolicitudCotizacionesRepository.ExisteNumArch(int.Parse(intNumOpe), numArch) == true)
            {
                datosErrXML = new SolicitudesCotizacion();
                datosSC.strError = "101"; //registro duplicado
                datosErrXML.intNumOpe = datosSC.intNumOpe;
                datosErrXML.strError = "101";
                datosErrXML.descError = "Registro duplicado";
                erroresXML.Add(datosErrXML);
            }
        }
        public static void afiliado(XmlNodeList xNodo, SolicitudesCotizacion datosSC)
        {
            try
            {
                for (int i = 0; i < xNodo.Count; i++)
                {
                    switch (((XmlElement)xNodo[i]).LocalName)
                    {
                        case "tipoDoc": datosSC.strTipoDoc = ((XmlElement)xNodo[i]).InnerText; break;
                        case "nroDoc": datosSC.strNumDoc = ((XmlElement)xNodo[i]).InnerText; break;
                        case "apellidoPaterno": datosSC.strApPat = ((XmlElement)xNodo[i]).InnerText.Length > 20 ? ((XmlElement)xNodo[i]).InnerText.Substring(0, 20) : ((XmlElement)xNodo[i]).InnerText; break;
                        case "apellidoMaterno": datosSC.strApMat = ((XmlElement)xNodo[i]).InnerText.Length > 20 ? ((XmlElement)xNodo[i]).InnerText.Substring(0, 20) : ((XmlElement)xNodo[i]).InnerText; break;
                        case "primerNombre": datosSC.strNom = ((XmlElement)xNodo[i]).InnerText.Length > 20 ? ((XmlElement)xNodo[i]).InnerText.Substring(0, 20) : ((XmlElement)xNodo[i]).InnerText; break;
                        case "segundoNombre": datosSC.strNomSec = ((XmlElement)xNodo[i]).InnerText.Length > 20 ? ((XmlElement)xNodo[i]).InnerText.Substring(0, 20) : ((XmlElement)xNodo[i]).InnerText; break;
                        case "genero": datosSC.strSexo = ((XmlElement)xNodo[i]).InnerText; break;
                        case "fechaNacimiento":
                            datosSC.datFecNac = DateTime.Parse(((XmlElement)xNodo[i]).InnerText);
                            datosSC.strFecNac = datosSC.datFecNac.ToString("yyyyMMdd");
                            break;
                        case "gradoInvalidez": datosSC.strGraInv = ((XmlElement)xNodo[i]).InnerText; break;
                        case "condicionInvalidez": datosSC.strSitInv = ((XmlElement)xNodo[i]).InnerText; break;
                        case "estadoSobrevivencia": datosSC.strEstSob = ((XmlElement)xNodo[i]).InnerText; break;
                    }
                }
                _SolicitudCotizacionesRepository.insertAfiliado(numArch, datosSC);
            }
            catch (Exception)
            {
                datosSC.strError = "103"; //errores en afiliado
                datosErrXML = new SolicitudesCotizacion();
                datosErrXML.intNumOpe = datosSC.intNumOpe;
                datosErrXML.strError = "103";
                datosErrXML.descError = "Errores al cargar el afiliado";
                erroresXML.Add(datosErrXML);
            }
        }
        public static void fondo(XmlNodeList xNodo, SolicitudesCotizacion datosSC)
        {
            try
            {
                for (int i = 0; i < xNodo.Count; i++)
                {
                    switch (((XmlElement)xNodo[i]).LocalName)
                    {
                        case "moneda": datosSC.strCodMon = ((XmlElement)xNodo[i]).InnerText; break;
                        case "capitalPension": datosSC.strCapPen = ((XmlElement)xNodo[i]).InnerText; break;
                        case "saldoCic": datosSC.strMtoCIC = ((XmlElement)xNodo[i]).InnerText; break;
                        case "valorCuota": datosSC.strMtoCuo = ((XmlElement)xNodo[i]).InnerText; break;
                        case "saldoCuotas": datosSC.strMtoSal = ((XmlElement)xNodo[i]).InnerText; break;
                        case "bonoActualizado": datosSC.strBonAct = ((XmlElement)xNodo[i]).InnerText; break;
                        case "tieneCobertura": datosSC.strCodCob = ((XmlElement)xNodo[i]).InnerText; break;
                        case "tipoCambioCompraAA": datosSC.strTipCamAA = ((XmlElement)xNodo[i]).InnerText; break;
                        case "EESScobertura": datosSC.strCodCiaCob = ((XmlElement)xNodo[i]).InnerText; break;
                        case "aporteAdicional": datosSC.strApoAdi = ((XmlElement)xNodo[i]).InnerText; break;
                    }
                }
                _SolicitudCotizacionesRepository.insertFondo(numArch, datosSC);
            }
            catch (Exception)
            {
                datosSC.strError = "104"; //errores en fondo
                datosErrXML = new SolicitudesCotizacion();
                datosErrXML.intNumOpe = datosSC.intNumOpe;
                datosErrXML.strError = "104";
                datosErrXML.descError = "Errores al cargar el fondo";
                erroresXML.Add(datosErrXML);
            }
        }
        public static void beneficiario(XmlNodeList xNodo, SolicitudesCotizacion datosSC)
        {
            try
            {
                for (int i = 0; i < xNodo.Count; i++)
                {
                    switch (((XmlElement)xNodo[i]).LocalName)
                    {
                        case "apellidoPaterno": datosSC.strPatBen = ((XmlElement)xNodo[i]).InnerText; break;
                        case "apellidoMaterno": datosSC.strMatBen = ((XmlElement)xNodo[i]).InnerText; break;
                        case "primerNombre": datosSC.strNomBen = ((XmlElement)xNodo[i]).InnerText; break;
                        case "segundoNombre": datosSC.strNomSecBen = ((XmlElement)xNodo[i]).InnerText; break;
                        case "parentesco": datosSC.strParBen = ((XmlElement)xNodo[i]).InnerText; break;
                        case "condicionInvalidez": datosSC.strSitInvBen = ((XmlElement)xNodo[i]).InnerText; break;
                        case "fechaNacimiento": datosSC.strFecNacBen = ((XmlElement)xNodo[i]).InnerText; break;
                        case "genero": datosSC.strSexoBen = ((XmlElement)xNodo[i]).InnerText; break;
                    }
                }
                _SolicitudCotizacionesRepository.insertBeneficiario(numArch, datosSC);
                datosSC.strPatBen = null;
                datosSC.strMatBen = null;
                datosSC.strNomBen = null;
                datosSC.strNomSecBen = null;
                datosSC.strParBen = null;
                datosSC.strSitInvBen = null;
                datosSC.strFecNacBen = null;
                datosSC.strSexoBen = null;
            }
            catch (Exception)
            {
                datosSC.strError = "105"; //errores en beneficiario
                datosErrXML = new SolicitudesCotizacion();
                datosErrXML.intNumOpe = datosSC.intNumOpe;
                datosErrXML.strError = "105";
                datosErrXML.descError = "Errores al cargar el beneficiaro";
                erroresXML.Add(datosErrXML);
            }
        }
        public static void producto(XmlNodeList xNodo, SolicitudesCotizacion datosSC)
        {
            try
            {
                for (int i = 0; i < xNodo.Count; i++)
                {
                    switch (((XmlElement)xNodo[i]).LocalName)
                    {
                        case "modalidad": datosSC.strCodMod = ((XmlElement)xNodo[i]).InnerText; break;
                        case "moneda": datosSC.strMonPro = ((XmlElement)xNodo[i]).InnerText; break;
                        case "anosRT": datosSC.strannosRT = ((XmlElement)xNodo[i]).InnerText; break;
                        case "porcentajeRVD": datosSC.strPrcRVD = ((XmlElement)xNodo[i]).InnerText; break;
                        case "periodoGarantizado": datosSC.strPerGar = ((XmlElement)xNodo[i]).InnerText; break;
                        case "coberturaConyuge": datosSC.strCobCon = ((XmlElement)xNodo[i]).InnerText; break;
                        case "derechoCrecer": datosSC.strDerCre = ((XmlElement)xNodo[i]).InnerText; break;
                        case "gratificacion": datosSC.strGratif = ((XmlElement)xNodo[i]).InnerText; break;
                        case "particionCapital": datosSC.strPartCapital = ((XmlElement)xNodo[i]).InnerText; break;
                    }
                }
                _SolicitudCotizacionesRepository.insertProducto(numArch, datosSC);
                datosSC.strCodMod = null;
                datosSC.strMonPro = null;
                datosSC.strannosRT = null;
                datosSC.strPrcRVD = null;
                datosSC.strPerGar = null;
                datosSC.strCobCon = null;
                datosSC.strDerCre = null;
                datosSC.strGratif = null;
                datosSC.strPartCapital = null;
            }
            catch (Exception)
            {
                datosSC.strError = "106"; //errores en producto
                datosErrXML = new SolicitudesCotizacion();
                datosErrXML.intNumOpe = datosSC.intNumOpe;
                datosErrXML.strError = "106";
                datosErrXML.descError = "Errores al cargar el producto";
                erroresXML.Add(datosErrXML);
            }
        }

        public static void elResto(string nombreNodo, XmlNodeList xNodo, SolicitudesCotizacion datosSC, int cont)
        {
            try
            {
                switch (nombreNodo)
                {
                    case "AFP": datosSC.strAfp = ((XmlElement)xNodo[cont]).GetElementsByTagName("AFP")[0].InnerText; break;
                    case "CUSPP": datosSC.strCussp = ((XmlElement)xNodo[cont]).GetElementsByTagName("CUSPP")[0].InnerText; break;
                    case "tipoBeneficio": datosSC.strTipBen = ((XmlElement)xNodo[cont]).GetElementsByTagName("tipoBeneficio")[0].InnerText; break;
                    case "cambioModalidad": datosSC.strCamMod = ((XmlElement)xNodo[cont]).GetElementsByTagName("cambioModalidad")[0].InnerText; break;
                    case "pensionPreliminar": datosSC.strPenPre = ((XmlElement)xNodo[cont]).GetElementsByTagName("pensionPreliminar")[0].InnerText; break;
                    case "tasaRPyRT": datosSC.strTasaRPRT = ((XmlElement)xNodo[cont]).GetElementsByTagName("tasaRPyRT")[0].InnerText; break;
                    case "fechaDevengue":
                        datosSC.datFecDev = DateTime.Parse(((XmlElement)xNodo[cont]).GetElementsByTagName("fechaDevengue")[0].InnerText);
                        datosSC.strFecDev = datosSC.datFecDev.ToString("yyyyMMdd");
                        break;
                    case "fechaSuscripcionIII":
                        datosSC.datFecSus = DateTime.Parse(((XmlElement)xNodo[cont]).GetElementsByTagName("fechaSuscripcionIII")[0].InnerText);
                        datosSC.strFecSus = datosSC.datFecSus.ToString("yyyyMMdd");
                        break;
                    case "devengueSolicitud":
                        datosSC.datFecDevSol = DateTime.Parse(((XmlElement)xNodo[cont]).GetElementsByTagName("devengueSolicitud")[0].InnerText);
                        datosSC.strFecDevSol = datosSC.datFecDevSol.ToString("yyyyMMdd");
                        break;
                    case "fechaEnvio":
                        datosSC.datFecEnv = DateTime.Parse(((XmlElement)xNodo[cont]).GetElementsByTagName("fechaEnvio")[0].InnerText);
                        datosSC.strFecEnv = datosSC.datFecEnv.ToString("yyyyMMdd");
                        break;
                    case "fechaCierre":
                        datosSC.datFecCie = DateTime.Parse(((XmlElement)xNodo[cont]).GetElementsByTagName("fechaCierre")[0].InnerText);
                        datosSC.strFecCie = datosSC.datFecCie.ToString("yyyyMMdd");
                        break;
                    case "tipoCambio": datosSC.strTipCam = ((XmlElement)xNodo[cont]).GetElementsByTagName("tipoCambio")[0].InnerText; break;
                    case "diaCita":
                        datosSC.datFecCita = DateTime.Parse(((XmlElement)xNodo[cont]).GetElementsByTagName("diaCita")[0].InnerText);
                        datosSC.strFecCita = datosSC.datFecCita.ToString("yyyyMMdd");
                        break;
                    case "horaCita": datosSC.strHorCita = ((XmlElement)xNodo[cont]).GetElementsByTagName("horaCita")[0].InnerText; break;
                    case "lugarCita":
                        datosSC.strLugCita = ((XmlElement)xNodo[cont]).GetElementsByTagName("lugarCita")[0].InnerText;

                        if (datosSC.strLugCita.Length != 0)
                        {
                            string[] strDep = datosSC.strLugCita.Split('-');
                            string val = strDep[0];
                            datosSC.strDepto = _SolicitudCotizacionesRepository.BuscaCodDepto(val);
                            if (datosSC.strDepto == "0")
                            {
                                datosSC.strDepto = _SolicitudCotizacionesRepository.BuscaCodDepto("LIMA");
                                //datosSC.strDepto = "0"; //'Departamento no existe en la base de datos
                            }
                        }
                        else
                        {
                            datosSC.strDepto = "0"; //No informa departamento
                        }

                        break;
                    case "numeroMensualidad": datosSC.strNumMensualidad = ((XmlElement)xNodo[cont]).GetElementsByTagName("numeroMensualidad")[0].InnerText; break;
                    case "tipoFondo": datosSC.strTipoFondo = ((XmlElement)xNodo[cont]).GetElementsByTagName("tipoFondo")[0].InnerText; break;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// José Hernández Alvarado.
        /// 13-12-2018
        /// </summary>
        /// <returns>Lista con valores de resumen de carga de solicitudes.</returns>
        public List<SolicitudesCotizacion> RptResumenCarga()
        {
            try
            {
                return _SolicitudCotizacionesRepository.RptResumenCarga(numArch.ToString());

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Response informacionGenerar(string numArchivo, string nomArch)
        {
            GenArMelerRepository _GenArMelerLogic = new GenArMelerRepository();
            try
            {
                Response res = new Response();
                //ManFecAceptacionCotizacion Datos = new ManFecAceptacionCotizacion();
                var fec = _SolicitudCotizacionesRepository.getFechaEnvio(numArchivo);
                var Datos = _SolicitudCotizacionesRepository.informacionGenerar(numArchivo, fec);

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

        public Response datosSiguiente(int numArch)
        {
            Response res = new Response();
            try
            {
                XmlConfigurator.Configure();
                _log.Info("Comenzara la validacion SISCO");
                _log.Info("Numero de archivo " + numArch);
                string queryC = _SolicitudCotizacionesRepository.validacionesSISCO(numArch);
                _log.Info("Se ejecutara el query " + queryC);
                _SolicitudCotizacionesRepository.EjecutarScript(queryC);

                _log.Info("Comenzara la validacion Sicotiza no cotiza");
                string querySN = _SolicitudCotizacionesRepository.validacionesSiNoCotiza(numArch);
                _log.Info("Se ejecutara el query " +querySN);
                if (querySN != "")
                {
                    _SolicitudCotizacionesRepository.EjecutarScript(querySN);
                }
                res.IsOk = true;
                return res;
            }
            catch (Exception ex)
            {
                res.Message = "Surgió un error al realizar los cálculos SISCO o en las validaciones de cotización.";
                res.IsOk = false;
                return res;
            }
        }

    }
}