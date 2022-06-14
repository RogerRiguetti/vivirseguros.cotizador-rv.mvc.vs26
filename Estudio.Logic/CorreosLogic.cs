using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Persistence.Repositories;
using System.Net;

namespace Estudio.Logic
{
    public class CorreosLogic
    {
        ParametrosRepository _parametroRepository = new ParametrosRepository();

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-27
        /// Envia notificaciones vía correo electrónico
        /// </summary>
        /// <param name="cuerpo"> Cuerpo del correo </param>
        /// <param name="asunto"> Asunto del correo </param>
        /// <param name="destinatarios"> Lista de correos a quienes se enviara el correo </param>
        /// <param name="archivoAdjuntar"> Dirección del archivo que se desea enviar </param>
        /// <returns> Regresa un valor que indica si se envió el correo </returns>
        
        public bool envioCorreo(string cuerpo, string asunto, List<string> destinatarios, string archivoAdjuntar)
        {
            try
            {
                MailMessage mensaje = new MailMessage();
                SmtpClient cliente = new SmtpClient();

                string correoNotificacion = _parametroRepository.ConsultaParametro("CORREONOTIF").Elemento;
                string hostNotifiacion = _parametroRepository.ConsultaParametro("HOSTNOTIF").Elemento;
                int portNotificacion = Convert.ToInt32(_parametroRepository.ConsultaParametro("PORTNOTIF").Elemento);
                string pswNotificacion = _parametroRepository.ConsultaParametro("PSWNOTIF").Elemento;

                mensaje.From = new MailAddress(correoNotificacion);

                foreach (var item in destinatarios)
                    mensaje.To.Add(new MailAddress(item));

                mensaje.Subject = asunto;

                if (System.IO.File.Exists(archivoAdjuntar))
                    mensaje.Attachments.Add(new Attachment(archivoAdjuntar));

                mensaje.Body = cuerpo;
                mensaje.IsBodyHtml = false;
                mensaje.Priority = MailPriority.Normal;

                cliente.Credentials = new NetworkCredential(correoNotificacion, pswNotificacion);

                cliente.Host = hostNotifiacion;
                cliente.Port = portNotificacion;
                cliente.EnableSsl = true;
                cliente.Send(mensaje);

                return true;
            }
            catch (Exception ex)
            {
                Console.Write("Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas");
                return false;
            }
        }
    }
}
