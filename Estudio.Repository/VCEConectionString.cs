using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Estudio.Repository
{
    public class VCEConectionString
    {
        /// <summary>
        /// Antonio Quezada
        /// 2018-02-13
        /// Desencripta la cadena de conexiòn proveniente del Web.config
        /// </summary>
        /// <param name="toDecryt"> Cadena a desencriptar </param>
        /// <param name="status"> Indica si se realizò la desencriptaciòn </param>
        /// <returns> Regresa la cadena de conexiòn a la Base de Datos desencriptada </returns>
        
        public static string Decrypt(string toDecryt, out bool status)
        {
            var result = string.Empty;
            try
            {
                var decryted = Convert.FromBase64String(toDecryt);
                result = System.Text.Encoding.UTF8.GetString(decryted);
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }

            return result;
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-02-13
        /// Encripta la cadena de conexiòn
        /// </summary>
        /// <param name="toEncryt"> Cadena a encriptar </param>
        /// <param name="status"> Indica si se realizò la encriptaciòn </param>
        /// <returns> Regresa la cadena encriptada </returns>
        
        public static string Encrypt(string toEncryt, out bool status)
        {
            var result = string.Empty;
            try
            {
                var encrypted = System.Text.Encoding.UTF8.GetBytes(toEncryt);
                result = System.Convert.ToBase64String(encrypted);

                status = true;
            }
            catch (Exception)
            {
                status = false;
            }

            return result;
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-02-13
        /// Obtiene la cadena de conexiòn a la base de datos de VCEstudioOficiales
        /// </summary>
        /// <returns> Regresa un string con la cadena de conexiòn </returns>
        
        public static string Connection()
        {
            bool status;

            string ConnectionString = ConfigurationManager.ConnectionStrings["VCEConection"].ToString();

            ConnectionString = Decrypt(ConnectionString, out status);

            return ConnectionString;
        }
    }
}
