using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace Estudio.Controllers.Assets
{
    public class Utils
    {



        private static string _modal_delete_header = "Warning";
        private static string _modal_delete_body = "¿Desea eliminar el registro?";
        private static string _text_delete_role = "¿Desea eliminar el rol {0}?";

        public static string modal_deleterole_Text
        {
            get
            {
                return _text_delete_role;
            }
        }




        public static string modal_delete_header
        {
            get
            {
                //logica de base de si trae mensaje o no
                //if
                //else
                return _modal_delete_header;
            }

        }
        public static string modal_delete_body
        {
            get
            {
                //logica de base de si trae mensaje o no
                //if
                //else
                return _modal_delete_body;
            }

        }





        public static bool ValidateStr(string strId)
        {
            return (strId.Length == 0 ? false : true);
        }
        public static bool VerifyPassword(string pass, string hashOriginalPass)
        {
            var hashInputPass = CreatePassword(pass);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOriginalPass, hashInputPass))
                return true;
            else
                return false;
        }
        public static string CreatePassword(string password)
        {
            MD5 crypto = MD5.Create();
            byte[] data = crypto.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sbuilder = new StringBuilder();
            foreach (var b in data)
                sbuilder.Append(b.ToString("x2"));
            return sbuilder.ToString();
        }

    }
}