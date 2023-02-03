using Estudio.Logic;
using Estudio.Repository;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using log4net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Estudio.Controllers.Helpers
{
    public class SendEmailJson
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        CorreosLogic _correoLogic = new CorreosLogic();

        public void SendEmailJsonJubilare(string json, string nroOperacion, string respuestaws, object resultado)
        {
            _log.Info("Comenzara el envio del correo electronico con la notificación");

            JToken response = JToken.FromObject(json);

            string asunto = "Error al actualizar producto.";
            string cuerpos = "No se pudo actualizar el número de operación: " + nroOperacion + " - " + response;

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

            if (!_correoLogic.envioCorreo(cuerpos, asunto, correos, null))
            {
                _log.Info("Error al enviar el correo electronico");
                _log.Info("**************************************************************");
                ((Response)resultado).Message = "Error al enviar el correo electronico.";
            }
        }
    }
}