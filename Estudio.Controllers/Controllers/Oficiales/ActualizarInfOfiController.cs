using Estudio.Logic;
using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Estudio.Repository.Persistence.Repositories;
using Estudio.Repository.Helpers;
using log4net;
using System.Reflection;
using log4net.Config;


namespace Estudio.Controllers.Controllers.Oficiales
{
    public class ActualizarInfOfiController : Controller
    {
        // GET: ActualizarInfOfi
        ActualizaInfOfiLogic _ActualizarInfOfiLogic = new ActualizaInfOfiLogic();
        RutinaOficialesRepository _rutinaOficialesRepository = new RutinaOficialesRepository();
        ActualizaInfOfiRepository _ActualizarInfORepository = new ActualizaInfOfiRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        ///  Lizbeth Morales
        /// 25-04-2019
        /// Metodo que busca los registros que esten guardados con ese Nro. Operación o CUSPP
        /// </summary>
        /// <param name="numOperacion">Numero de operación con la que se desea obtener información</param>
        /// <param name="codCUSPP">Codigo CUSPP con el que se desea obtener información</param>
        /// <returns> Retorna los datos obtenidos para mostrarlos en la vista </returns>
        public ActionResult busqueda(string codCUSPP, string numCor, string numOperacion)
        {
            try
            {
                var resultado = _ActualizarInfOfiLogic.busqueda(codCUSPP, numCor, numOperacion);
                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public ActionResult Guardar(ActualizaInfOfi informacion)
        //{
        //    try
        //    {
        //        var usuario = Convert.ToString(this.Session["Account"]);
        //        var resultado = _ActualizarInfOfiLogic.Guardar(informacion);
        //        return Json(resultado);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public ActionResult Guardar(ActualizaInfOfi informacion)
        {
            object resultado = new object();
            try
            {
                XmlConfigurator.Configure();
                _log.Info("*****************GUARDAR INFORMACIÓN***********************");
                var usuario = Convert.ToString(this.Session["Account"]);

                _log.Info("Datos" + informacion);
                resultado = _ActualizarInfOfiLogic.Guardar(informacion);
                _log.Info("Se obtuvieron los datos");
                _log.Info("numCor: " + informacion.numCorrelativo);
                _log.Info("Creacion de variable WS");
                wsXML.AdmIntegracionCotizador wsActualizaP = new wsXML.AdmIntegracionCotizador();
                _log.Info("Se genero la variable WS");
                string nroOperacion = informacion.numOperacion.ToString();
                _log.Info("Numero de operacion = " + informacion.numOperacion.ToString());
                _log.Info("JSON = " + (((Response)resultado).Object).ToString() + "NumOperacion = " + informacion.numOperacion.ToString());
                string json = (((Response)resultado).Object).ToString();
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
                    _log.Info("Datos guardados con exito, pero surgio un error: " + respuestaws.Split('#')[1]);
                    ((Response)resultado).Message = "Datos guardados con exito, pero surgio un error: " + respuestaws.Split('#')[1];
                }

                return Json(resultado);
            }
            catch (Exception ex)
            {
                _log.Info("Error a enviar: " + ex.Message);
                ((Response)resultado).Message = "Se guardaron los datos con exito, pero surgio un error al enviar el JSON por web service";
                return Json(resultado);
            }
        }

        public ActionResult busquedaMod(string numCor, string numOperacion)
        {
            try
            {
                var resultado = _ActualizarInfOfiLogic.busquedaMod(numCor, numOperacion);
                Response obj = new Response();
                obj.IsOk = true;
                obj.Object = resultado;
                return Json(obj);
            }
            catch (Exception ex)
            {
                Response obj = new Response();
                obj.IsOk = false;
                obj.Message = "Ocurrió un error al Buscar los valores de esa modalidad";
                return Json(obj);
            }
        }
    }
}