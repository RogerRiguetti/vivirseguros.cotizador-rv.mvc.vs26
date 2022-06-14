using System;
using System.Collections.Generic;
using Estudio.Repository.Persistence.Repositories;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository;
using log4net;
using log4net.Config;
using System.Reflection;
namespace Estudio.Logic
{
    public class MantenedorPerfilesLogic
    {
        MantenedorPerfilesRepository _mantenedorPerfilesRepository = new MantenedorPerfilesRepository();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Daniel Mercado
        /// 14/06/2021
        /// Este metodo nos obtiene los roles que existen
        /// </summary>
        /// <returns>Regresa los roles existentes</returns>
        public Response ConsultarRoles()
        {
            try
            {
                XmlConfigurator.Configure();
                _log.Info("Ejecuta la consulta de roles: ");
                Response _res = new Response();
                List<UserProfile> rolesCatalogo = new List<UserProfile>();

                rolesCatalogo = _mantenedorPerfilesRepository.ObtenerRolesFiltro("ROLES");

                _res.Object = new { rolesCatalogoJs = rolesCatalogo }; ;
                _res.IsOk = true;
                _res.Message = "Consulta Exitosa";

                return _res;

            }
            catch (Exception ex)
            {
                _log.Info("ERROR " + ex);
                Response _res = new Response();
                _res.IsOk = false;
                _res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return _res;
            }

        }
        /// <summary>
        /// Daniel Mercado Alvarado 
        /// 07/06/2021 
        /// Este metodo sirve para regresar los detalles del permiso al controlador
        /// </summary>
        /// <param name="id">el id del permiso que se quiere consultar</param>
        /// <returns>regresa un dato de tipo MantenedorPerfiles</returns>
        public MantenedorPerfiles DetailsMantenedorPerfiles(int id)
        {
            try
            {
                return _mantenedorPerfilesRepository.DetailsMantenedorPerfiles(id);

            }
            catch (Exception ex)
            {
                _log.Info("ERROR " + ex);
                return null;
            }
        }

        /// <summary>
        /// Daniel Mercado Alvarado 
        /// 07/06/2021 
        /// En este metodo hace update y inserta los nuevos permismos
        /// </summary>
        /// <param name="ListaPermisos">Parametro que contiene una lista con el el id y clave del permiso selecionado en el index</param>
        /// <returns>Regresa un Json con la información que se guardaron cambios del Permiso</returns>
        public Response UpdatePermiso(string [] ListaPermisos, int idUsuario)
        {
            try
            {
                _log.Info("Ejecuta Metodo Modificar Permiso");
                string rolclave;
                string idPermiso;
                CheckPerfiles ejecuta = new CheckPerfiles();
                _log.Info("Ciclo para actualizar permisos");
                for (int a = 0; a < ListaPermisos.Length; a++)
                {
                    var ListaSplitPerfiles = ListaPermisos[a].Split('#');
                    idPermiso = ListaSplitPerfiles[0];
                    rolclave = ListaSplitPerfiles[1];
                    
                    ejecuta = _mantenedorPerfilesRepository.UpdatePermiso(rolclave, idPermiso, idUsuario);
                }
                Response res = new Response();
                res.Object = ejecuta;
                res.IsOk = true;
                res.Message = "Permiso actualizado con éxito";
                return res;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR " + ex);
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }

        }


        /// <summary>
        /// Daniel Mercado 
        /// 03/06/2021
        /// Metodo el cual nos permitira modificar la informacion de un permiso, este metodo nos conecta el repositorio con el controlador
        /// </summary>
        /// <param name="id">Parametro el cual obtiene el id del Permiso a modificar</param>
        /// <param name="namePantalla">Parametro que guardara el nombre del la pantalla</param>
        /// <param name="descripcionLarga">Parametro que guardara la descripcion larga del permiso</param>
        /// <param name="sistemas_Idsistema">Parametro que guardara el id del sistema</param>
        ///  <param name="nodoPadre">Parametro que guardara el nodoPadre</param>
        /// <returns> Regresa un objeto que contiene la información del permiso modificado </returns>

        public Response Edit(int id, string namePantalla , string descripcionLarga, int sistemas_Idsistema, int idUsuario, string nodoPadre)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                _log.Info("Ejecuta Metodo Modificar Permiso");
                res.Object = _mantenedorPerfilesRepository.Edit(id, namePantalla, descripcionLarga, sistemas_Idsistema, idUsuario, nodoPadre);
                if (res.Object == null)
                {
                    res.Message = "Permiso modificado con éxito";
                    
                }
                else
                    res.Message = "El Permiso  ya existe, favor de ingresar otro";
                return res;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR " + ex);
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }

        }

        /// <summary>
        /// Daniel Mercado
        /// 02/06/2021
        /// Nos realizara la conexion entre el repositorio y el controlador
        /// Metodo que tiene todos los datos detallados del permiso seleccionado
        /// </summary>
        /// <param name="id">Id del permiso a observar</param>
        /// <returns>Nos regresara la informacion del permiso seleccionado</returns>
        public MantenedorPerfiles Details(int id)
        {
            MantenedorPerfiles permisoDetalle = new MantenedorPerfiles();
            _log.Info("Ejecuta consulta para obtener los detalles de el Perfil");
            permisoDetalle = _mantenedorPerfilesRepository.Details(id);            

            return permisoDetalle;
        }

        /// <summary>
        /// Daniel Mercado
        /// 02/06/2021
        /// Metodo que realizara la comunicacion entre el repositorio y el controlador para activar o desactivar un Permiso
        /// </summary>
        /// <param name="id">Id del Permiso a activar o desactivar</param>
        /// <param name="active">status del Permiso (Activo o Inactivo)</param>
        /// <returns>Regresa un mensaje con la baja del Permiso</returns>
        public Response Delete(int id, int active, int idUsuario)
        {
            try
            {
                _log.Info("Ejecuta consulta para desactivar el Perfil");
                Response res = new Response();
                res.Object = _mantenedorPerfilesRepository.Delete(id, active, idUsuario);
                res.IsOk = true;
                res.Message = "Permiso actualizado con éxito";
                return res;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR:" + ex);
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }

        }

        /// <summary>
        /// Daniel Mercado
        /// 02/06/2021
        /// Nos realizara la conexion entre el repositorio y el controlador
        /// Este metodo mandara la informacion obtenida de todos los permisos a la vista principal
        /// </summary>
        /// <returns>Regresa los datos obtenidos en el Reposotory de permisos para mostrarlos en la vista Index </returns>
        public Response Index(int idSistema, string clavePermiso)
        {
            try
            {
                _log.Info("Ejecuta consulta para obetener los permisos de el Perfil");
                List<MantenedorPerfiles> listaPermisos =  _mantenedorPerfilesRepository.Index(idSistema, clavePermiso);
                _log.Info("Ejecuta consulta para obetener los roles ligados a los Perfiles ");
                List<CheckPerfiles> listaRolesPermisos = _mantenedorPerfilesRepository.IndexProfilePermissions(idSistema, clavePermiso);
                _log.Info("Ejecuta consulta para obetener los roles ");
                List<UserProfile> listaRoles = _mantenedorPerfilesRepository.ObtenerRoles("ROLES");

                Response response = new Response();
                response.IsOk = true;
                response.Object = new { Permisos = listaPermisos, Roles = listaRoles, RolesPermisos = listaRolesPermisos };
                response.Message = "Consulta Exitosa";
                return response;
            }
            catch (Exception ex)
            {
                _log.Info("ERROR:" + ex);
                Response response = new Response();
                response.IsOk = false;
                response.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return response;
            }
        }
        /// <summary>
        /// Daniel Mercado 01/06/2021
        /// Este metodo nos obtiene los sistemas que existen 
        /// </summary>
        /// <returns>Regresa los sistemas existentes</returns>
        public List<Sistemas> ObtenerSistemas()
        {
            _log.Info("Ejecuta consulta para obetener los sistemas ");
            return _mantenedorPerfilesRepository.ObtenerSistemas("SISTEMAS");
        }

        /// <summary>
        /// Daniel Mercado 02/06/2021
        /// Este metodo nos obtiene los roles que existen 
        /// </summary>
        /// <returns>Regresa los roles existentes</returns>
        public List<UserProfile> ObtenerRoles()
        {
            _log.Info("Ejecuta consulta para obetener los roles ");
            return _mantenedorPerfilesRepository.ObtenerRoles("ROLES");
        }

        /// <summary>
        /// Daniel Mercado 01/06/2021 
        /// Metodo que nos realizara la conexion entre el repositorio y el controlador, Nos guardara la informacion del nuevo permiso
        /// </summary>
        /// <param name="clave">Parametro que guardara el nombre del nuevo permiso</param>
        /// <param name="descripcionLarga">Parametro que guardara la descripcion larga del permiso</param>
        /// <param name="sistemas_Idsistema">Parametro que guardara el id del sistema</param>
        /// <param name="nodoPadre">Parametro que guardara el nodoPadre</param>
        /// <returns>Nos regresa la informacion ya guardada del permiso añadido</returns>

        public Response Create(string namePantalla,string clave, string descripcionLarga, int sistemas_Idsistema, int idUsuario, string nodoPadre)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                _log.Info("Ejecuta consulta para crear el Perfil");
                res.Object = _mantenedorPerfilesRepository.Create(namePantalla,clave, descripcionLarga, sistemas_Idsistema, idUsuario, nodoPadre);
                if (res.Object == null)
                {
                    res.Message = "Permiso creado con éxito";
                }
                else
                    res.Message = "El Permiso  ya existe, favor de ingresar otro";
                return res;
            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }

        }

    }
}
