using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Persistence.Repositories;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Persistence;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Helpers;

namespace Estudio.Logic
{
    public class RolesLogic
    {
        gzRoleRepository _RolesRepo = new gzRoleRepository();

       /// <summary>
       /// Autor: Heber Solis
       /// este metodo sirve para hacer la consulta inicial de la tabla de roles
       /// </summary>
       /// <returns>regresa el resultado de el metodo Index de Rolesrepository</returns>
        public List<RoleView> Search()
        {
            try
            {
                return _RolesRepo.Index(); 
               
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Autor: Heber Solis
        /// Este metodo sirve para regresar los detalles del usuario al controlador
        /// </summary>
        /// <param name="id">el id del role que se quiere consultar</param>
        /// <returns>regresa un dato de tipo gzRole</returns>
        public gzRole DetailsRole(int id)
        {
            try
            {
                return _RolesRepo.DetailsRole(id);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Autor: Heber Solis
        /// Este metodo regresa el registro creado al controlador
        /// </summary>
        /// daniel mercado
        /// se agrega nuevo parametro Clave
        /// <param name="description">la descripcion del rol creado</param>
        /// <param name="clave">la clave del rol creado</param>
        /// <returns>regresa un objeto del tipo Response</returns>
        public Response CreateRole(string description, string clave, int idUsuario)
        {
            try
            {
                Response _res = new Response();
                _res.IsOk = true;

                _res.Object=_RolesRepo.CreateRole(description, clave, idUsuario);
                if (_res.Object == null)
                    _res.Message = "Rol creado con éxito!";
                else
                    _res.Message = "El rol ya existe, favor de ingresar otro";
                return _res;
            }
            catch (Exception ex)
            {
                Response _res = new Response();

                _res.IsOk = false;
                _res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return _res;
            }
        }
        /// <summary>
        /// Autor: Heber Solis
        /// este metodo regresa el rol a desactivar o activar al controlador
        /// </summary>
        /// <param name="id">id del registro</param>
        /// <param name="active">estado del rol ya sea activo o inactivo</param>
        /// <returns>un objeto del tipo response</returns>
        public Response DeleteRole(int id, int active, int idUsuario)
        {
            try
            {
                Response _res = new Response();
               _res.Object=_RolesRepo.DeleteRole(id, active, idUsuario);
                _res.IsOk = true;
                _res.Message = "Rol actualizado con exito";
                return _res;
            }
            catch (Exception ex)
            {
                Response _res = new Response();

                _res.IsOk = false;
                _res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return _res;
            }
        }
        /// <summary>
        /// Autor: Heber Solis
        /// Este metodo regresa el rol editado al controlador
        /// </summary>
        /// <param name="id">id del rol</param>
        /// <param name="description">descripcion del rol editado</param>
        /// <returns>objeto del tipo response</returns>
        public Response EditRole(int id,string description, int idUsuario)
        {
            try
            {
                Response _res = new Response();
                _res.IsOk = true;
                _res.Object = _RolesRepo.EditRole(id, description, idUsuario);
                if (_res.Object == null)
                    _res.Message = "Rol editado con exito";
                else
                    _res.Message = "El rol ya existe, favor de ingresar otro";
                return _res;
            }
            catch (Exception ex)
            {

                Response _res = new Response();
               
                _res.IsOk = false;
                _res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return _res;
            }
        }




    }
}
