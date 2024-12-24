using Estudio.Controllers.Helpers;
using Estudio.Logic;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using log4net;
using log4net.Config;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Estudio.Controllers.Controllers.Oficiales
{

    public class ExcepcionesController : Controller
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        ExcepcionesLogic _ExcepcionesLogic = new ExcepcionesLogic();
        CorreosLogic _correoLogic = new CorreosLogic();
        SendEmailJson _sendEmailJson = new SendEmailJson();

        static object _globalValue;
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
        public ActionResult Index()
        {
            ViewBag.pagina = "ExcepcionesLi";
            return View();
        }
        public ActionResult ExcepcionesExternas()
        {
            int idUsuario = Convert.ToInt32(this.Session["UserId"]);
            string rol = this.Session["Name"].ToString();

            cargarDatos();
            ViewBag.pagina = "ExcepcionesEx";
            ViewBag.Informacion = GlobalVar.GlobalValue;
            return View();
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 15-11-2018
        /// Metodo que busca los registros que esten guardados desde el Web service en la base de datos
        /// </summary>
        /// <returns>  </returns>
        public void cargarDatos()
        {
            try
            {
                var resultado = _ExcepcionesLogic.obtenerDatos();
                GlobalVar.GlobalValue = resultado.Object;
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        ///  Omar Figueroa Flores
        /// 25-10-2018
        /// Metodo que busca los registros que esten guardades con esa operacion o el cusspp
        /// </summary>
        /// <param name="numOperacion">Numero de operación con la que se desea obtener información</param>
        /// <param name="codCUSPP">Codigo CUSPP con el que se desea obtener información</param>
        /// <param name="numCor">Numero Correlativo para la busqeuda de exepciones externas</param>
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
        public async Task<ActionResult> calculo(string nuevaTV, string nuevaCO, Exceptiones informacion, string caso)
        {
            try
            {
                var usuario = Convert.ToString(this.Session["Account"]);
                var resultado = await _ExcepcionesLogic.calculo(nuevaTV, nuevaCO, informacion, caso, "");
                return Json(resultado);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult cancelar(string numCor, string numOperacion, string periodoDiferido, string primerTramo)
        {
            object resultado = new object();
            try
            {
                XmlConfigurator.Configure();
                _log.Info("*****************CANCELAR INFORMACIÓN DE EX - EXTERNAS***********************");
                resultado = _ExcepcionesLogic.cancelar(numCor, numOperacion, periodoDiferido, primerTramo);
                _log.Info("Se obtuvieron los datos");
                wsXML.AdmIntegracionCotizador wsActualizaP = new wsXML.AdmIntegracionCotizador();
                _log.Info("Datos a enviar: ");
                _log.Info("numOperacion: " + numOperacion);
                string json = (((Response)resultado).Object).ToString();
                _log.Info("JSON: " + json);
                string respuestaws = wsActualizaP.ActualizarProducto(numOperacion.ToString(), json);
                if (respuestaws == "OK")
                {
                    _log.Info("Datos rechazados y enviados con exito");
                    ((Response)resultado).Message = "Datos rechazados y enviados con exito";
                }
                else
                {
                    _log.Info("Surgio un error en el web service: " + respuestaws.Split('#')[1]);
                    ((Response)resultado).Message = "Surgio un error en el web service: " + respuestaws.Split('#')[1];
                }
                return Json(resultado);
            }
            catch (Exception ex)
            {
                _log.Info("Error a enviar: " + ex.Message);
                ((Response)resultado).Message = "Surgio un error al enviar el JSON por web service";
                return Json(resultado);
            }
        }
        public ActionResult Guardar(Exceptiones informacion, string caso, Exceptiones infoRut)
        {
            object resultado = new object();
            string nroOperacion = "";
            string json = "";

            try
            {
                XmlConfigurator.Configure();
                _log.Info("*****************GUARDAR INFORMACIÓN DE EX - EXTERNAS***********************");
                var usuario = Convert.ToString(this.Session["Account"]);
                resultado = _ExcepcionesLogic.Guardar(informacion, caso, infoRut);
                _log.Info("Se obtuvieron los datos");
                _log.Info("numCor: " + informacion.numCorrelativo);
                wsXML.AdmIntegracionCotizador wsActualizaP = new wsXML.AdmIntegracionCotizador();
                nroOperacion = informacion.numOperacion.ToString();
                json = (((Response)resultado).Object).ToString();
                _log.Info("Datos a enviar: ");
                _log.Info("numOperacion: " + nroOperacion);
                _log.Info("JSON: " + json);
                string respuestaws = wsActualizaP.ActualizarProducto(nroOperacion, json);
                _log.Info("respuestaws: " + respuestaws);
                if (respuestaws == "OK")
                {
                    _log.Info("Datos guardados y enviados con exito");
                    ((Response)resultado).Message = "Datos guardados y enviados con exito";
                }
                else
                {
                    _sendEmailJson.SendEmailJsonJubilare(json, nroOperacion, respuestaws, resultado);
                    ((Response)resultado).Message = "Se guardaron los datos con éxito, pero surgió un error al enviar el JSON por web service.";
                }
                return Json(resultado);
            }
            catch (Exception ex)
            {
                _log.Info("Error a enviar: " + ex.Message);
                _sendEmailJson.SendEmailJsonJubilare(json, nroOperacion, "", resultado);
                ((Response)resultado).Message = "Se guardaron los datos con éxito, pero surgió un error al enviar el JSON por web service.";
                return Json(resultado);
            }
        }
    }
}