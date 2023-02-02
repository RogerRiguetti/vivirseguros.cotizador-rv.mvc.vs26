using Estudio.Logic;
using Estudio.Repository;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Estudio.Controllers.Controllers.Oficiales
{
    public class MejorasController : Controller
    {
        ExcepcionesLogic _ExcepcionesLogic = new ExcepcionesLogic();
        RutinaOficialesRepository _rutinaOficialesRepository = new RutinaOficialesRepository();
        ExcepcionesRepository _excepcionesRepository = new ExcepcionesRepository();
        CorreosLogic _correoLogic = new CorreosLogic();

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 25-10-2018
        /// Metodo que busca los registros que esten guardades con esa operacion o el cusspp
        /// </summary>
        /// <param name="numOperacion">Numero de operación con la que se desea obtener información</param>
        /// <param name="codCUSPP">Codigo CUSPP con el que se desea obtener información</param>
        /// <returns> Retorna los datos obtenidos para mostrarlos en la vista </returns>
        public ActionResult busqueda(string codCUSPP, string numCor, string numOperacion)
        {
            try
            {
                var resultado = _ExcepcionesLogic.busqueda(codCUSPP, numCor, numOperacion);
                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Omar Figueroa Flores
        /// 25-10-2018
        /// Metodo que realiza el re-calculo con los datos obtenidos
        /// </summary>
        /// <param name="nuevaTV">Nueva tasa vitalsia para realizar el re-calculo</param>
        /// <param name="nuevaCO">Nueva cootizacion para realizar el re-calculo</param>
        /// <returns> Retorna los nuevos datos re-calculados </returns>
        public async Task<ActionResult> calculo(string nuevaTV, string nuevaCO, Exceptiones informacion, string caso, string parametro)
        {
            try
            {
                var usuario = Convert.ToString(this.Session["Account"]);
                var resultado = await _ExcepcionesLogic.calculo(nuevaTV, nuevaCO, informacion, caso, parametro);
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public ActionResult Guardar(Exceptiones informacion, string caso, Exceptiones infoRut)
        //{
        //    try
        //    {
        //        var usuario = Convert.ToString(this.Session["Account"]);
        //        var resultado = _ExcepcionesLogic.Guardar(informacion, caso, infoRut);
        //        return Json(resultado);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public ActionResult Guardar(Exceptiones informacion, string caso, Exceptiones infoRut)
        {
            object resultado = new object();
            string json = "";
            string nroOperacion = "";

            try
            {
                XmlConfigurator.Configure();
                _log.Info("*****************GUARDAR INFORMACIÓN DE MEJORAS***********************");
                var usuario = Convert.ToString(this.Session["Account"]);

                _log.Info("Datos" + informacion);
                _log.Info("Caso" + caso);
                _log.Info("Información " + infoRut);
                resultado = _ExcepcionesLogic.Guardar(informacion, caso, infoRut);
                if ((((Response)resultado).IsOk) != true)
                {
                    _log.Info("Informacion No guardada");
                    ((Response)resultado).Message.ToString();
                }
                else
                {
                    _log.Info("Se obtuvieron los datos");
                    _log.Info("numCor: " + informacion.numCorrelativo);
                    wsXML.AdmIntegracionCotizador wsActualizaP = new wsXML.AdmIntegracionCotizador();
                    nroOperacion = informacion.numOperacion.ToString();
                    json = (((Response)resultado).Object).ToString();
                    _log.Info("Datos a enviar: ");
                    _log.Info("numOperacion: " + nroOperacion);
                    _log.Info("JSON: " + json);
                    string respuestaws = wsActualizaP.ActualizarProducto(nroOperacion, json);
                    if (respuestaws == "OK")
                    {
                        _log.Info("Datos guardados y enviados con exito");
                        ((Response)resultado).Message = "Datos guardados y enviados con exito";
                    }
                    else
                    {
                        _log.Info("Comenzara el envio del correo electronico con la notificación");

                        string asuntos = "Error al actualizar producto.";

                        JToken response = JToken.FromObject(json);

                        string cuerpos = "No se pudo actualizar el número de operación: " + nroOperacion + " - " + response;

                        List<string> correoss = new List<string>();

                        string queryCons = "SELECT Parametro FROM Parametros where ClaveParametro = 'CORREOWS'";

                        _log.Info("Comenzara la busqueda del correo electronico");

                        string DatosCons = VCEDBContext<Parametro>.CallSelectStatement(queryCons, x => new Parametro
                        {
                            Elemento = x.GetString(0)
                        }).FirstOrDefault().Elemento;
                        _log.Info("El correo se enviará a " + DatosCons);

                        string correoe = DatosCons;
                        correoss.Add(correoe);

                        if (!_correoLogic.envioCorreo(cuerpos, asuntos, correoss, null))
                        {
                            _log.Info("Error al enviar el correo electronico");
                            _log.Info("**************************************************************");
                            _log.Info("Datos guardados con exito, pero surgio un error: " + respuestaws.Split('#')[1]);
                            ((Response)resultado).Message = "Datos guardados con éxito, pero surgió un error: " + respuestaws.Split('#')[1];
                        }
                    }
                }
                return Json(resultado);
            }
            catch (Exception ex)
            {
                _log.Info("Error a enviar: " + ex.Message);
                _log.Info("Comenzara el envio del correo electronico con la notificación");

                string asuntos = "Error al actualizar producto.";

                JToken response = JToken.FromObject(json);

                string cuerpos = "No se pudo actualizar el número de operación: " + nroOperacion + " - " + response;

                List<string> correoss = new List<string>();

                string queryCons = "SELECT Parametro FROM Parametros where ClaveParametro = 'CORREOWS'";

                _log.Info("Comenzara la busqueda del correo electronico");

                string DatosCons = VCEDBContext<Parametro>.CallSelectStatement(queryCons, x => new Parametro
                {
                    Elemento = x.GetString(0)
                }).FirstOrDefault().Elemento;
                _log.Info("El correo se enviará a " + DatosCons);

                string correoe = DatosCons;
                correoss.Add(correoe);

                if (!_correoLogic.envioCorreo(cuerpos, asuntos, correoss, null))
                {
                    _log.Info("Error al enviar el correo electronico");
                    _log.Info("**************************************************************");
                    _log.Info("Datos guardados con exito, pero surgio un error: ");
                }
                ((Response)resultado).Message = "Se guardaron los datos con exito, pero surgio un error al enviar el JSON por web service.";
                return Json(resultado);
            }
        }

        public ActionResult ConsultarRangoComision(string parametro, double cic, string codMoneda, string numOp, int codReaj)
        {
            try
            {
                Exceptiones cotizacionExcepciones = new Exceptiones();
                if (codMoneda == "S/.Aj." || codMoneda == "S/.")
                {
                    codMoneda = "NS";
                }
                else
                {
                    codMoneda = "US";
                }

                cotizacionExcepciones = _excepcionesRepository.ConsultarCotizacion(numOp.ToString());

                List<beDatosModalidad> VarREGP = new List<beDatosModalidad>();
                VarREGP = _rutinaOficialesRepository.ConsultaRegionTasas(0);

                beDatosModalidad VarREG = new beDatosModalidad();
                // VarREG = _rutinaOficialesRepository.ConsultaRegionTasas(Convert.ToDouble(cotizacion.Cic));
                VarREG = (from reg in VarREGP
                          where reg.MTO_MINIMO <= (Convert.ToDecimal(cic)) &&
                            (Convert.ToDecimal(cic)) <= reg.MTO_MAXIMO &&
                            reg.CodReg_Asoc == cotizacionExcepciones.Cod_region
                          select reg).FirstOrDefault();

                string region = VarREG.CodReg;
                DateTime Fecha = DateTime.Today;
                string FecCal = Fecha.ToString("yyyyMMdd");

                List<beDatosTasasPar> ListaTas2 = new List<beDatosTasasPar>();
                string com = ".4";
                List<string> rangos = new List<string>();
                if (parametro == "Mej")
                {
                    double comisionMax = 0;
                    double comisionMin = 0;
                    ListaTas2 = _rutinaOficialesRepository.ConsultaGastosTasasIndMej(FecCal, cotizacionExcepciones.CodigoPension);
                    List<beDatosTasasPar> comisionErrorl = (from l in ListaTas2 where (l.CodMon == codMoneda && l.TipPen == cotizacionExcepciones.CodigoPension && l.CodReg == region && l.TipRea == codReaj) select l).ToList();
                    //List<string> rangos = new List<string>();
                    string ranIn = "";
                    string ran = "";
                    for (int b = 0; b < comisionErrorl.Count; b++)
                    {
                        comisionMin = ((comisionErrorl[b].ComMin / 1.42) - .2);
                        comisionMin = Convert.ToInt32(comisionMin);

                        comisionMax = ((comisionErrorl[b].ComMax / 1.42) - .2);
                        comisionMax = Convert.ToInt32(comisionMax);

                        double nuecomi = ((comisionErrorl[b].ComMax / 1.42) - .2);
                        ranIn = comisionMin.ToString() + com;
                        rangos.Add(ranIn);
                        double comisMax = comisionMax + .4;
                        for (b = 0; (Convert.ToDouble(ranIn) + 1) <= nuecomi; b++)
                        {
                            if ((Convert.ToDouble(ranIn) + 1) <= comisMax)
                            {
                                ranIn = (Convert.ToDouble(ranIn) + 1).ToString();
                                rangos.Add(ranIn);
                            }
                        }


                    }
                }
                else
                {
                    double comisionMax = 0;
                    double comisionMin = 0;

                    ListaTas2 = _rutinaOficialesRepository.ConsultaGastosTasasIndMejo(FecCal, cotizacionExcepciones.CodigoPension);
                    List<beDatosTasasPar> comisionErrorl = (from l in ListaTas2 where (l.CodMon == codMoneda && l.TipPen == cotizacionExcepciones.CodigoPension && l.CodReg == region && l.TipRea == codReaj) select l).ToList();

                    string ranIn = "";
                    string ran = "";
                    for (int b = 0; b < comisionErrorl.Count; b++)
                    {
                        comisionMin = ((comisionErrorl[b].ComMin / 1.42) - .2);
                        comisionMin = Convert.ToInt32(comisionMin);

                        comisionMax = ((comisionErrorl[b].ComMax / 1.42) - .2);
                        comisionMax = Convert.ToInt32(comisionMax);

                        double nuecomi = ((comisionErrorl[b].ComMax / 1.42) - .2);
                        ranIn = comisionMin.ToString() + com;
                        rangos.Add(ranIn);
                        double comisMax = comisionMax + .4;
                        for (b = 0; (Convert.ToDouble(ranIn) + 1) <= nuecomi; b++)
                        {
                            if ((Convert.ToDouble(ranIn) + 1) <= comisMax)
                            {
                                ranIn = (Convert.ToDouble(ranIn) + 1).ToString();
                                rangos.Add(ranIn);
                            }
                        }
                    }
                }

                var resultado = rangos;
                Response obj = new Response();
                obj.IsOk = true;
                obj.Object = resultado;
                return Json(obj);
            }
            catch (Exception ex)
            {
                Response obj = new Response();
                obj.IsOk = false;
                obj.Message = "Ocurrió un error al cargar combo de comisiones";
                return Json(obj);
            }
        }
    }
}