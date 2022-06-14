using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Helpers;
using Estudio.Repository.Persistence.Repositories;
using Estudio.Repository;

namespace Estudio.Logic
{
    public class EstudioLogic
    {
        gzUserRepository _gzUserRepository = new gzUserRepository();

        /// <summary>
        /// Antonio Quezada
        /// 2018-02-13
        /// Valida el inicio de sesiòn
        /// </summary>
        /// <param name="usuario"> Nombre del usuario (Account) </param>
        /// <param name="password"> Contraseña </param>
        /// <returns> Regresa la respuesta que aparecera en la vista </returns>

        public Response ValidateLogin(string usuario, string password)
        {
            Response res = new Response();
            try
            {
                UserView userView = new UserView();
                bool status = true;

                string passwordEncryp = VCEConectionString.Encrypt(password, out status);
                userView = _gzUserRepository.ValidateLogin(usuario, passwordEncryp, 0);
                if (userView == null)
                {
                    res.Message = "El usuario no fue encontrado.";
                    res.IsOk = false;
                    return res;
                }

                if(userView.Active == 0)
                {
                    res.Message = "El usuario esta inactivo.";
                    res.IsOk = false;
                    return res;
                }

                userView = _gzUserRepository.ValidateLogin(usuario, passwordEncryp, 1);

                if (userView == null)
                {
                    res.Message = "El password es incorrecto.";
                    res.IsOk = false;
                    return res;
                }

                res.Object = userView;

                return res;
            }
            catch (Exception ex)
            {
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                res.IsOk = false;
                return res;
            }
        }

        public int getUserIdAsesor(string usuario)
        {
            try
            {
                return _gzUserRepository.getUserIdAsesor(usuario);
            }
            catch
            {
                return -1;
            }
        }

        public Response ValidateLoginAD(string usuario, string password)
        {
            Response res = new Response();
            try
            {
                UserView userView = new UserView();
                bool status = true;

                string passwordEncryp = VCEConectionString.Encrypt(password, out status);
                userView = _gzUserRepository.ValidateLogin(usuario, passwordEncryp, 0);
                if (userView == null)
                {
                    res.Message = "El usuario no fue encontrado.";
                    res.IsOk = false;

                }

                if (userView.Active == 0)
                {
                    res.Message = "El usuario esta inactivo.";
                    res.IsOk = false;

                }
                res.Object = userView;
                return res;
            }
            catch (Exception ex)
            {
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                res.IsOk = false;
                return res;
            }
        }

        public gzUser ConsultarUsuario(int idUsuario)
        {
            gzUser usuario = new gzUser();
            try
            {
                usuario = _gzUserRepository.ConsultarUsuario(idUsuario, 2);
                return usuario;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
