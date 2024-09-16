using System;
using System.Collections.Generic;
using Estudio.Repository.Persistence.Repositories;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Helpers;
using Estudio.Repository;

namespace Estudio.Logic
{
    public class UserLogic
    {
        gzUserRepository _gzUserRepository = new gzUserRepository();
        ParametrosRepository _parametroRepository = new ParametrosRepository();
        CorreosLogic _correoLogic = new CorreosLogic();

        /// <summary>
        /// Lizbeth Morales 13/02/2018
        /// Nos realizara la conexion entre el repositorio y el controlador
        /// Este metodo mandara la informacion obtenida de todos los usuarios a la vista principal
        /// </summary>
        /// <returns>Regresa los datos obtenidos en el Reposotory de usuarios para mostrarlos en la vista Index </returns>
        public Response Index(string account, string names, string lastNames)
        {
            try
            {
                Response response = new Response();
                response.IsOk = true;
                response.Object = _gzUserRepository.Index(account, names, lastNames);
                response.Message = "Consulta Exitosa";
                return response;
            }
            catch (Exception ex)
            {

                Response response = new Response();
                response.IsOk = false;
                response.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return response;
            }
        }
        /// <summary>
        /// Lizbeth Morales 13/02/2018
        /// Nos realizara la conexion entre el repositorio y el controlador
        /// Metodo que tiene todos los datos detallados del usuario seleccionado
        /// </summary>
        /// <param name="id">Id del usuario a observar</param>
        /// <returns>Nos regresara la informacion del usuario seleccionado</returns>
        public gzUser Details(int id)
        {
            gzUser usuario = new gzUser();
            bool status = true;

            usuario = _gzUserRepository.Details(id);
            string passwordEncryp = VCEConectionString.Decrypt(usuario.Password, out status);
            usuario.Password = passwordEncryp;

            return usuario;
        }

        /// <summary>
        /// Lizbeth Morales 14/02/2018
        /// Metodo que nos realizara la conexion entre el repositorio y el controlador, Nos guardara la informacion del nuevo usuario por agregar
        /// 
        /// Antonio Quezada 2018-03-28
        /// Se agregan los parámetros de correo, idSupervisor, names y lastNames
        /// Se agrega la funcionalidad para mandar notificaciones por correo electrónico
        ///  Daniel Mercado 11/06/2021
        /// se agrega prametro UserProfileList
        /// </summary>
        /// <param name="id">Id del nuevo usuario(Este campo no sera agregado)</param>
        /// <param name="account">Nombre del nuevo usuario</param>
        /// <param name="password">Constraseña del nuevo usuario</param>
        /// <param name="active">Este campo indica si el usuario es activo o no(Este campo aparecera automaticamente como activo al momento de guardar el nuevo usuario)</param>
        /// <param name="UserProfileList">Roles que el usuario desempeñara</param>
        /// <param name="correo"> Correo del usuario creado </param>
        /// <param name="idSupervisor"> Id del supervisor relacionado al asesor </param>
        /// <param name="Names"> Nombres del usuario </param>
        /// <param name="LastNames"> Apellidos del usuario </param>
        /// <param name="NumeroAgente"> Número del usuario en la tabla agente de JUBILARE </param>
        /// <returns>Nos regresa la informacion ya guardada del usuario añadido</returns>

        public Response Create(int id, string account, string password, int active, int[] UserProfileList, string correo, int idSupervisor, string names, string lastNames, int numeroAgente, int idUsuario)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                bool status = true;
                string passwordEncryp = VCEConectionString.Encrypt(password, out status);

                if (UserProfileList.Length == 0)
                {
                    res.IsOk = false;
                    res.Message = "Debe seleccionar al menos un Rol.";
                    return res;
                }
                else
                {
                    res.Object = _gzUserRepository.Create(id, account, passwordEncryp, active, 0, correo, idSupervisor, names, lastNames, numeroAgente, idUsuario);

                    if (res.Object == null)
                    {

                        for (int a = 0; a < UserProfileList.Length; a++)
                        {
                            var idRol = UserProfileList[a];

                            gzUser ejecuta = _gzUserRepository.CreateRoles(idRol);
                        }


                        res.Message = "Usuario creado con éxito";

                        string urlAplicacion = _parametroRepository.ConsultaParametro("DIRAPP").Elemento;
                        string asunto = "VC Estudio - Registro de Usuario";
                        string cuerpo = "Favor de ingresar a la dirección " + urlAplicacion + " con las siguientes credenciales: \n\n Usuario: " +
                            account + "\n Contraseña: " + password;
                        List<string> correos = new List<string>();
                        correos.Add(correo);

                        if (!_correoLogic.envioCorreo(cuerpo, asunto, correos, ""))
                            res.Message = "Usuario creado con éxito. Error al mandar el correo";
                    }
                    else
                        res.Message = "El Usuario o el Número de Agente ya existe, favor de ingresar otro";
                    return res;
                }

            }
            catch (Exception ex)
            {
                Response res = new Response();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                return res;
            }

        }
        /// <summary>
        /// Lizbeth Morales 15/02/2018
        /// Metodo el cual nos permitira modificar la informacion de un usuario, este metodo nos conecta el repositorio con el controlador
        /// 
        /// Antonio Quezada 2018-03-28
        /// Se agregan los parámetros de correo, idSupervisor, names y lastNames
        /// 
        ///  Daniel Mercado 11/06/2021
        /// se agrega prametro UserProfileList
        /// </summary>
        /// <param name="id">Id del usuario a modificar(Este parametro no puede ser modificado) </param>
        /// <param name="account">Nombre del usuario</param>
        /// <param name="password">Password del usuario</param>
        /// <param name="UserProfileList">Rol del usuario</param>
        /// <param name="correo"> Correo del usuario creado </param>
        /// <param name="idSupervisor"> Id del supervisor relacionado al asesor </param>
        /// <param name="Names"> Nombres del usuario </param>
        /// <param name="LastNames"> Apellidos del usuario </param>
        /// <param name="NumeroAgente"> Número del usuario en la tabla agente de JUBILARE </param>
        /// <returns> Regresa un objeto que contiene la información del usuario modificado </returns>

        public Response Edit(int id, string account, string password, int[] UserProfileList, string correo, int idSupervisor, string names, string lastNames, int numeroAgente, int idUsuario)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                bool status = true;
                string passwordEncryp = VCEConectionString.Encrypt(password, out status);


                if (UserProfileList.Length == 0)
                {
                    res.IsOk = false;
                    res.Message = "Debe seleccionar al menos un Rol.";
                    return res;
                }
                else
                {
                    res.Object = _gzUserRepository.Edit(id, account, passwordEncryp, correo, idSupervisor, names, lastNames, numeroAgente, idUsuario);


                    if (res.Object == null)
                    {
                        gzUser delete = _gzUserRepository.DeleteRolesUser(id);

                        for (int a = 0; a < UserProfileList.Length; a++)
                        {
                            var idRol = UserProfileList[a];
                           
                            gzUser ejecuta = _gzUserRepository.UpdateRolesUser(id, idRol);
                        }

                        res.Message = "Usuario modificado con éxito";

                        string urlAplicacion = _parametroRepository.ConsultaParametro("DIRAPP").Elemento;
                        string asunto = "VC Estudio - Modificación de Usuario";
                        string cuerpo = "Favor de ingresar a la dirección " + urlAplicacion + " con las siguientes credenciales: \n\n Usuario: " +
                            account + "\n Contraseña: " + password;
                        List<string> correos = new List<string>();
                        correos.Add(correo);

                        if (!_correoLogic.envioCorreo(cuerpo, asunto, correos, ""))
                            res.Message = "Usuario modificado con éxito. Error al mandar el correo";

                    }

                }

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

        /// <summary>
        /// Lizbeth Morales 16/02/2018
        /// Metodo que realizara la comunicacion entre el reposotorio y el controlador para activar o desactivar un usuario
        /// </summary>
        /// <param name="id">Id del usuario a activar o desactivar</param>
        /// <param name="active">status del usuario (Activo o Inactivo)</param>
        /// <returns>Regresa un mensaje con la baja del usuario</returns>
        public Response Delete(int id, int active, int idUsuario)
        {
            try
            {
                Response res = new Response();
                res.Object = _gzUserRepository.Delete(id, active, idUsuario);
                res.IsOk = true;
                res.Message = "Usuario actualizado con éxito";
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
        /// <summary>
        /// Lizbeth Morales 14/02/2018
        /// Este metodo nos obtiene los roles que existen para despues agregarlos a los nuevos usuarios
        /// </summary>
        /// <returns>Regresa los roles existentes</returns>
        public List<UserProfile> ObtenerRoles()
        {

            return _gzUserRepository.ObtenerRoles("ROLES");

        }

    
        public List<gzUser> ObtenerSupervisores()
        {
            return _gzUserRepository.ObtenerSupervisores();
        }

        /// <summary>
        /// Indica que permisos tiene el usuario
        /// </summary>
        /// <param name="userId"> Id del usuario </param>
        /// <returns> Regresa en un object los permisos del usuario </returns>

        public Response GetPermissions(int userId)
        {
            Response res = new Response();
            Identity permissions = new Identity();

            try
            {
                var authorize = _gzUserRepository.GetPermissions(userId);

                foreach (var item in authorize)
                {
                    if (item.Authorize == "SystemIndex") permissions.SystemIndex = true;
                    if (item.Authorize == "SystemDetails") permissions.SystemDetails = true;
                    if (item.Authorize == "SystemCreate") permissions.SystemCreate = true;
                    if (item.Authorize == "SystemEdit") permissions.SystemEdit = true;
                    if (item.Authorize == "SystemDelete") permissions.SystemDelete = true;

                    if (item.Authorize == "ModuleIndex") permissions.ModuleIndex = true;
                    if (item.Authorize == "ModuleDetails") permissions.ModuleDetails = true;
                    if (item.Authorize == "ModuleCreate") permissions.ModuleCreate = true;
                    if (item.Authorize == "ModuleEdit") permissions.ModuleEdit = true;
                    if (item.Authorize == "ModuleDelete") permissions.ModuleDelete = true;

                    if (item.Authorize == "PageIndex") permissions.PageIndex = true;
                    if (item.Authorize == "PageDetails") permissions.PageDetails = true;
                    if (item.Authorize == "PageCreate") permissions.PageCreate = true;
                    if (item.Authorize == "PageEdit") permissions.PageEdit = true;
                    if (item.Authorize == "PageDelete") permissions.PageDelete = true;
                    if (item.Authorize == "PageConfigure") permissions.PageConfigure = true;

                    if (item.Authorize == "RoleIndex") permissions.RoleIndex = true;
                    if (item.Authorize == "RoleDetails") permissions.RoleDetails = true;
                    if (item.Authorize == "RoleCreate") permissions.RoleCreate = true;
                    if (item.Authorize == "RoleEdit") permissions.RoleEdit = true;
                    if (item.Authorize == "RoleDelete") permissions.RoleDelete = true;
                    if (item.Authorize == "RoleConfigure") permissions.RoleConfigure = true;

                    if (item.Authorize == "UserIndex") permissions.UserIndex = true;
                    if (item.Authorize == "UserDetails") permissions.UserDetails = true;
                    if (item.Authorize == "UserCreate") permissions.UserCreate = true;
                    if (item.Authorize == "UserEdit") permissions.UserEdit = true;
                    if (item.Authorize == "UserDelete") permissions.UserDelete = true;
                    if (item.Authorize == "UserConfigure") permissions.UserConfigure = true;
                    if (item.Authorize == "UserProfile") permissions.UserProfile = true;

                    if (item.Authorize == "CotizacionIndex") permissions.CotizacionIndex = true;
                    if (item.Authorize == "CotizacionEdit") permissions.CotizacionEdit = true;

                    //Nuevos
                    if (item.Authorize == "ExcepcionIndex") permissions.ExcepcionIndex = true;
                    if (item.Authorize == "ExcepcionExterna") permissions.ExcepcionExterna = true;
                    if (item.Authorize == "GastosSepelioIndex") permissions.GastosSepelioIndex = true;
                    if (item.Authorize == "GenArMelerIndex") permissions.GenArMelerIndex = true;
                    if (item.Authorize == "LimiteCotIniIndex") permissions.LimiteCotIniIndex = true;
                    if (item.Authorize == "LimiteCotMejIndex") permissions.LimiteCotMejIndex = true;
                    if (item.Authorize == "ManFecAcepCotIndex") permissions.ManFecAcepCotIndex = true;
                    if (item.Authorize == "MantenedorIpcIndex") permissions.MantenedorIpcIndex = true;
                    if (item.Authorize == "MejorasIndex") permissions.MejorasIndex = true;
                    if (item.Authorize == "ParametroGastoIndex") permissions.ParametroGastoIndex = true;
                    if (item.Authorize == "ProCarArchivoIndex") permissions.ProCarArchivoIndex = true;
                    if (item.Authorize == "SolicitudCotIndex") permissions.SolicitudCotIndex = true;
                    if (item.Authorize == "TasaAnclajeIndex") permissions.TasaAnclajeIndex = true;
                    if (item.Authorize == "TasaCalceIndex") permissions.TasaCalceIndex = true;
                    if (item.Authorize == "TasaRentabilidadIndex") permissions.TasaRentabilidadIndex = true;
                    if (item.Authorize == "TasaMercadoIndex") permissions.TasaMercadoIndex = true;
                    if (item.Authorize == "ValoresMmIndex") permissions.ValoresMmIndex = true;
                    if (item.Authorize == "ValoresMonedaIndex") permissions.ValoresMonedaIndex = true;
                    if (item.Authorize == "EliCargasIndex") permissions.EliminarCargasIndex = true;
                    if (item.Authorize == "TCDiarioIndex") permissions.TCDiarioIndex = true;
                    if (item.Authorize == "OficialesIndex") permissions.OficialesIndex = true;

                    //Reservas
                    if (item.Authorize == "ReservasIndex") permissions.ReservasIndex = true;

                    // Renta Privada
                    if (item.Authorize == "RPMenu") permissions.RPMenu = true;

                    // Emision y Pago de Pensiones
                    if (item.Authorize == "EmisionPPMenu") permissions.EmisionPPMenu = true;


                    if (item.Authorize == "MantenedorPerfilesIndex") permissions.MantenedorPerfilesIndex = true;
                    if (item.Authorize == "MantenedorPerfilesCreate") permissions.MantenedorPerfilesCreate = true;
                    if (item.Authorize == "MantenedorPerfilesEdit") permissions.MantenedorPerfilesEdit = true;
                    if (item.Authorize == "MantenedorPerfilesDelete") permissions.MantenedorPerfilesDelete = true;
                    if (item.Authorize == "MantenedorPerfilesDetails") permissions.MantenedorPerfilesDetails = true;

                    if (item.Authorize == "MantenimientosIndex") permissions.MantenimientosIndex = true;
                    if (item.Authorize == "LimiteCotExtraOIndex") permissions.LimiteCotExtraOIndex = true;
                    if (item.Authorize == "TasaVtaPromIndex") permissions.TasaVtaPromIndex = true;
                    if (item.Authorize == "MantenedorFiltrosIndex") permissions.MantenedorFiltrosIndex = true;
                    if (item.Authorize == "MantenedorCurvasIndex") permissions.MantenedorCurvasIndex = true;
                    if (item.Authorize == "ActualizaInfOfiIndex") permissions.ActualizaInfOfiIndex = true;

                    //FondosMAX
                    if (item.Authorize == "FMMenu") permissions.FMMenu = true;

                }

                res.Object = permissions;
                return res;

            }
            catch (Exception ex)
            {
                Identity noaccess = new Identity();
                res.IsOk = false;
                res.Message = "Ocurrió un error. Por favor vuelve a intentar o contacta al área de Sistemas ";
                res.Object = noaccess;
                return res;
            }
        }

        public gzUser ConsultaClave(int id)
        {
            gzUser usuario = new gzUser();
            bool status = true;

            usuario = _gzUserRepository.ConsultaClave(id);
            string passwordEncryp = VCEConectionString.Decrypt(usuario.Password, out status);
            usuario.Password = passwordEncryp;

            return usuario;
        }

        public Response CambioClave(int id, string pass, int idUsuario)
        {
            try
            {
                Response res = new Response();
                res.IsOk = true;
                bool status = true;
                string passwordEncryp = VCEConectionString.Encrypt(pass, out status);
                res.Object = _gzUserRepository.CambiaClave(id, passwordEncryp, idUsuario);
                res.Message = "Usuario modificado con éxito";


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
        /// <summary>
        /// Daniel Mercado
        /// 11/06/2021
        /// Este metodo nos obtiene los roles que existen para despues agregarlos a los nuevos usuario
        /// <param name="Id">Parametro id del usuario con el  cual obtiene el rol selecionado del usuario</param>
        /// </summary>
        /// <returns>Regresa los roles existentes y los roles que tiene asignado el usuario</returns>
        public Response ConsultarRoles(int id)
        {
            try
            {
                Response _res = new Response();
                List<UserProfile> rolesCatalogo = new List<UserProfile>();
                List<UserProfile> rolesUsuario = new List<UserProfile>();

                rolesCatalogo = _gzUserRepository.ObtenerRoles("ROLES");
                rolesUsuario = _gzUserRepository.ObtenerRolesUsuario(id);

                _res.Object = new { rolesCatalogoJs = rolesCatalogo, rolesUsuarioJs = rolesUsuario }; ;
                _res.IsOk = true;
                _res.Message = "Consulta Exitosa";

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
        /// Daniel Mercado
        /// Este metodo nos obtiene los roles que tiene asignados el usuario
        /// </summary>
        /// <returns>Regresa los roles asignados al usuario</returns>
        public List<UserProfile> ObtenerRolesUsuario(int id)
        {
            return _gzUserRepository.ObtenerRolesUsuario(id);
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-27
        /// Obtiene todos los supervisores
        /// </summary>
        /// <returns> Regresa una lista de usuarios con los supervisores </returns>


        /// <summary>
        /// Daniel Mercado
        /// 14/06/2021
        /// Este metodo nos obtiene los roles que existen
        /// </summary>
        /// <returns>Regresa los roles existentes</returns>
        public Response ConsultarRolesCrear()
        {
            try
            {
                Response _res = new Response();
                List<UserProfile> rolesCatalogo = new List<UserProfile>();

                rolesCatalogo = _gzUserRepository.ObtenerRoles("ROLES");

                _res.Object = new { rolesCatalogoJs = rolesCatalogo }; ;
                _res.IsOk = true;
                _res.Message = "Consulta Exitosa";

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