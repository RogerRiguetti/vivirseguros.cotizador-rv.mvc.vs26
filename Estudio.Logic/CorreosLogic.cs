using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Persistence.Repositories;
using System.Net;
using log4net;
using System.Reflection;

namespace Estudio.Logic
{
    public class CorreosLogic
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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

        //public bool envioCorreo(string cuerpo, string asunto, List<string> destinatarios, string archivoAdjuntar)
        //{
        //    try
        //    {
        //        MailMessage mensaje = new MailMessage();
        //        SmtpClient cliente = new SmtpClient();

        //        string correoNotificacion = _parametroRepository.ConsultaParametro("CORREONOTIF").Elemento;
        //        string hostNotifiacion = _parametroRepository.ConsultaParametro("HOSTNOTIF").Elemento;
        //        int portNotificacion = Convert.ToInt32(_parametroRepository.ConsultaParametro("PORTNOTIF").Elemento);
        //        string pswNotificacion = _parametroRepository.ConsultaParametro("PSWNOTIF").Elemento;
        //        _log.Info("Correo de remitente: "+ correoNotificacion);
        //        mensaje.From = new MailAddress(correoNotificacion);

        //        foreach (var item in destinatarios)
        //        {
        //            mensaje.To.Add(new MailAddress(item));
        //            _log.Info("Destinatarios " + item);
        //        }
        //        _log.Info("Archivo adjunto: " + archivoAdjuntar);
        //        mensaje.Subject = asunto;

        //        if (System.IO.File.Exists(archivoAdjuntar))
        //            mensaje.Attachments.Add(new Attachment(archivoAdjuntar));

        //        mensaje.Body = cuerpo;
        //        mensaje.IsBodyHtml = false;
        //        mensaje.Priority = MailPriority.Normal;

        //        cliente.Credentials = new NetworkCredential(correoNotificacion, pswNotificacion);

        //        cliente.Host = hostNotifiacion;
        //        cliente.Port = portNotificacion;
        //        cliente.EnableSsl = false;
        //        cliente.Send(mensaje);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write("Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas");
        //        _log.Info("ERROR al enviar correo electronico: " + ex);
        //        return false;
        //    }
        //}

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
                string correoNotificacion = _parametroRepository.ConsultaParametro("CORREONOTIF").Elemento;
                string hostNotifiacion = _parametroRepository.ConsultaParametro("HOSTNOTIF").Elemento;
                int portNotificacion = Convert.ToInt32(_parametroRepository.ConsultaParametro("PORTNOTIF").Elemento);
                string pswNotificacion = _parametroRepository.ConsultaParametro("PSWNOTIF").Elemento;

                _log.Info("Correo de remitente: " + correoNotificacion + " |hostNotifiacion: " + hostNotifiacion + " |portNotificacion: " + Convert.ToString(portNotificacion) + " | pswNotificacion:" + pswNotificacion);

                SmtpClient client = new SmtpClient(hostNotifiacion, portNotificacion);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(correoNotificacion, pswNotificacion);

                MailAddress from = new MailAddress(correoNotificacion, String.Empty, System.Text.Encoding.UTF8);

                MailMessage message = new MailMessage();
                foreach (var item in destinatarios)
                {
                    message.To.Add(new MailAddress(item));
                    _log.Info("Destinatarios " + item);
                }

                _log.Info("Message From ");
                message.From = from;
                message.Body = cuerpo;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = asunto;
                message.SubjectEncoding = System.Text.Encoding.UTF8;


                if (System.IO.File.Exists(archivoAdjuntar))
                {
                    _log.Info("Existe Archivo");
                    message.Attachments.Add(new Attachment(archivoAdjuntar));
                }
                _log.Info("Security");
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                _log.Info("Send");

                client.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                Console.Write("Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas");
                _log.Info("ERROR al enviar correo electronico: " + ex);
                return false;
            }
        }
    }
}
