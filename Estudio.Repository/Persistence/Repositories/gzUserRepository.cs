using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudio.Repository.Core.Domain;
using Estudio.Repository.Core.Domain.Views;
using Estudio.Repository.Core.Repositories;
using Estudio.Repository.Helpers;

using System.Data.SqlClient;
using System.Data;

namespace Estudio.Repository.Persistence.Repositories
{
    public class gzUserRepository
    {

        /// <summary>
        /// Daniel Mercado
        /// 14/06/2021
        /// Este metodo tiene la conexion con el SP en la base de datos, para ELIMINAR
        /// </summary>
        /// <param name="id">Id  del rol </param>
        /// <returns> Regresa un objeto de la clase gzUser que contiene la información del usuario creado </returns>

        public gzUser DeleteRolesUser(int id)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'F', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, id, ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_CatalogoUsuarios, parameters, x => new gzUser
                {
                    Account = x.GetString(1),
                    Password = x.GetString(2)

                }).FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Daniel Mercado
        /// 14/06/2021
        /// Este metodo tiene la conexion con el SP en la base de datos, para editar los roles que tiene el usuario
        /// </summary>
        /// <param name="id">Id  del rol </param>
        /// <returns> Regresa un objeto de la clase gzUser que contiene la información del usuario creado </returns>

        public gzUser UpdateRolesUser(int id, int IdRol)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'A', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, id, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUserProfile_UserId", SqlDbType.Int, IdRol, ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_CatalogoUsuarios, parameters, x => new gzUser
                {
                    Account = x.GetString(1),
                    Password = x.GetString(2)

                }).FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Daniel Mercado
        /// 10/06/2021
        /// </summary>
        /// <param name="id"> id del usuario</param>
        /// <returns>Devuelve los roles que tiene asignados el usuario </returns>
        public List<UserProfile> ObtenerRolesUsuario(int id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'P', ParameterDirection.Input));
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, id, ParameterDirection.Input));

            return VCEDBContext<UserProfile>.CallStoreProcedure(StoredProcedures.VCE_CatalogoUsuarios, parameters, x => new UserProfile
            {
                UserId = x.GetInt32(0),
                Name = x.GetString(1)
            }).ToList();
        }

        /// <summary>
        /// Daniel Mercado
        /// 10/06/2021
        /// Este metodo tiene la conexion con el SP en la base de datos, por lo tanto ete metodo guardara la informacion ingresada del usuario nuevo
        /// </summary>
        /// <param name="id">Id  del rol </param>
        /// <returns> Regresa un objeto de la clase gzUser que contiene la información del usuario creado </returns>

        public gzUser CreateRoles(int idRol)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'G', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUserProfile_UserId", SqlDbType.Int, idRol, ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_CatalogoUsuarios, parameters, x => new gzUser
                {
                    Account = x.GetString(1),
                    Password = x.GetString(2)

                }).FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// Antonio Quezada
        /// 2018-02-13
        /// Valida el inicio de sesiòn
        /// </summary>
        /// <param name="usuario"> Nombre del usuario (Account) </param>
        /// <param name="password"> Contraseña </param>
        /// <param name="bandera"> Valor que indica si se va a consultar el usuario (0) o usuario y contraseña (1) </param>
        /// <returns> Regresa la informaciòn del usuario validado </returns>

        public UserView ValidateLogin(string usuario, string password, byte bandera)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPassword", SqlDbType.VarChar, password, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.TinyInt, bandera, ParameterDirection.Input));

                return VCEDBContext<UserView>.CallStoreProcedure(StoredProcedures.VCE_ValidaLogin, parameters, x => new UserView
                {
                    Id = x.GetInt32(0),
                    Account = x.GetString(1),
                    Name = x.GetString(2),
                    Active = x.GetByte(3),
                    Pass = x.GetString(4)
                }).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Lizbeth Morales 13/02/2018
        /// Consulta los usuarios para despues mostrarlos en la pantalla de Index (usuarios) 
        /// </summary>
        /// <returns>Regresa la informacion de los usuarios a la pantalla Index</returns>
        public List<gzUser> Index(string account, string names, string lastNames)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'R', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAccount", SqlDbType.VarChar, account, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNames", SqlDbType.VarChar, names, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pLastNames", SqlDbType.VarChar, lastNames, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPassword", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCorreo", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pActive", SqlDbType.TinyInt, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUserProfile_UserId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSupervisor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumeroAgente", SqlDbType.Int, 0, ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_CatalogoUsuarios, parameters, x => new gzUser
                {
                    Account = x.GetString(0),
                    Names = x.GetString(1),
                    LastNames = x.GetString(2),
                    Correo = x.GetString (3),
                    Status = x.GetByte(4) == 1 ? "Activo" : "Inactivo",
                    DateCreated = x.GetDateTime(5),
                    DateCreatedStr = x.GetDateTime(5).ToString("yyyy/MM/dd"),
                    DateModified = x.GetDateTime(6),
                    DateModifiedStr = x.GetDateTime(6).ToString("yyyy/MM/dd"),
                    RolStr = x.IsDBNull(7) ? "" : x.GetString(7),
                    Id = x.GetInt32(8)
                    
                }).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// Lizbeth Morales 14/02/2018
        /// Muestra los detalles del usuario indicado
        /// 
        /// Antonio Quezada 2018-03-28
        /// Se agregan valores para mostrar en la la vista (Names, LastNames e IdSupervisor)
        /// </summary>
        /// <param name="id"> Id del usuario</param>
        /// <returns>
        /// Regresa la informacion del usuario seleccionado
        /// </returns>
        public gzUser Details(int id)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'V', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, id, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAccount", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNames", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pLastNames", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPassword", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCorreo", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pActive", SqlDbType.TinyInt, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUserProfile_UserId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSupervisor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumeroAgente", SqlDbType.Int, 0, ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_CatalogoUsuarios, parameters, x => new gzUser
                {
                    Id = x.GetInt32(0),
                    Account = x.GetString(1),
                    Password = x.GetString(2),
                    Active = x.GetByte(3) == 1? "Activo" : "Inactivo",
                    DateCreated = x.GetDateTime(4),
                    DateModified = x.GetDateTime(5),
                    Names = x.GetString(6),
                    LastNames = x.GetString(7),
                    Correo = x.GetString(8),
                    IdSupervisor = x.IsDBNull(9) ? 0 : x.GetInt32(9),
                    Supervisor = x.IsDBNull(10) ? "" : x.GetString(10),
                    NumeroAgente = x.GetInt32(11)
                }).First();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// Lizbeth Morales 14/02/2018
        /// </summary>
        /// <param name="claveCatalogo"> clave que indicara que catalogo se mostrara</param>
        /// <returns>Devuelve los roles que se manejan para despues mostrarlos en un ComboBox </returns>
        public List<UserProfile> ObtenerRoles(string claveCatalogo)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, claveCatalogo, ParameterDirection.Input));

            return VCEDBContext<UserProfile>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new UserProfile
            {
                UserId = x.GetInt32(0),
                Name = x.GetString(1)
            }).ToList();
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-03-27
        /// Obtiene todos los supervisores
        /// </summary>
        /// <returns> Regresa una lista de usuarios con los supervisores </returns>

        public List<gzUser> ObtenerSupervisores()
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, "SUPERVISORES", ParameterDirection.Input));

            return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new gzUser
            {
                Id = x.GetInt32(0),
                NombreCompleto = x.GetString(1)
            }).ToList();
        }

        /// <summary>
        /// Lizbeth Morales 14/02/2018 
        /// Este metodo tiene la conexion con el SP en la base de datos, por lo tanto ete metodo guardara la informacion ingresada del usuario nuevo
        /// 
        /// Antonio Quezada 2018-03-28
        /// Se agregan los parámetros de Correo, IdSupervisor, names y lastNames
        /// </summary>
        /// <param name="id">Id del usuario nuevo(El id es autoincrementable por lo tanto al usuario no le aparecera la opcion de agregarle un valor en especifico)</param>
        /// <param name="account">Nombre que tendra el nuevo usuario</param>
        /// <param name="password">Password con la cual el nuevo usuario podra tener acceso al sistema</param>
        /// <param name="active">Este campo tiene si el usuario creado esta activo o no, pero por default al agregar un nuevo usuario su Status es Activo</param>
        /// <param name="UserProfileList">Es la lista de roles que  registrara para el nuevo usuario a ingresar en el sistema </param>
        /// <param name="correo"> Correo del usuario creado </param>
        /// <param name="idSupervisor"> Id del supervisor relacionado al asesor </param>
        /// <param name="Names"> Nombres del usuario </param>
        /// <param name="LastNames"> Apellidos del usuario </param>
        /// <param name="NumeroAgente"> Número del usuario en la tabla agente de JUBILARE </param>
        /// <returns> Regresa un objeto de la clase gzUser que contiene la información del usuario creado </returns>

        public gzUser Create(int id, string account, string password, int active, int  UserProfile_UserId, string correo, int idSupervisor, string names, string lastNames, int numeroAgente, int idUsuario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'C', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, id, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAccount", SqlDbType.VarChar, account, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNames", SqlDbType.VarChar, names, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pLastNames", SqlDbType.VarChar, lastNames, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPassword", SqlDbType.VarChar, password, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCorreo", SqlDbType.VarChar, correo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pActive", SqlDbType.TinyInt, active, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUserProfile_UserId", SqlDbType.Int, UserProfile_UserId, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSupervisor", SqlDbType.Int, idSupervisor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumeroAgente", SqlDbType.Int, numeroAgente, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, idUsuario, ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_CatalogoUsuarios, parameters, x => new gzUser
                {
                    Account = x.GetString(1),
                    Password = x.GetString(2)

                }).FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Antonio Quezada
        /// 2018-02-14
        /// Obtiene los permisos que tiene el usuario logeado
        /// </summary>
        /// <param name="userId"> Id del usuario </param>
        /// <returns> Regresa una lista que contiene los permisos del usuario </returns>

        public List<AuthorizationView> GetPermissions(int userId)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUser", SqlDbType.Int, userId, ParameterDirection.Input));

                return VCEDBContext<AuthorizationView>.CallStoreProcedure(StoredProcedures.VCE_ObtenerPermisos, parameters, x => new AuthorizationView
                {
                    Authorize = x.GetString(0)
                }).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Lizbeth Morales 15/02/2018
        /// Este metodo obtiene la informacion del usuario a editar para posteriormente guardar los cambios realizados
        /// 
        /// Antonio Quezada 2018-03-28
        /// Se agregan los parámetros de Correo, IdSupervisor, names y lastNames
        /// </summary>
        /// <param name="id">Id del usuario a modificar</param>
        /// <param name="account">Nombre del usuario (Este campo puede ser modificado)</param>
        /// <param name="password">Password del usuario(Este campo no se mostrara la informacion actual por lo tanto se tendra que agregar una nueva password o poner la actual)</param>
        /// <param name="userProfile_UserId">Rol que desempeñara el usuario</param>
        /// <param name="correo"> Correo del usuario creado </param>
        /// <param name="idSupervisor"> Id del supervisor relacionado al asesor </param>
        /// <param name="Names"> Nombres del usuario </param>
        /// <param name="LastNames"> Apellidos del usuario </param>
        /// <param name="NumeroAgente"> Número del usuario en la tabla agente de JUBILARE </param>
        /// <returns> Regresa un objeto de la clase gzUser que contiene la información del usuario creado </returns>

        public gzUser Edit(int id, string account, string password, string correo, int idSupervisor, string names, string lastNames, int numeroAgente, int idUsuario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'U', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, id, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAccount", SqlDbType.VarChar, account, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNames", SqlDbType.VarChar, names, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pLastNames", SqlDbType.VarChar, lastNames, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPassword", SqlDbType.VarChar, password, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCorreo", SqlDbType.VarChar, correo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pActive", SqlDbType.TinyInt, 1, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSupervisor", SqlDbType.Int, idSupervisor, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumeroAgente", SqlDbType.Int, numeroAgente, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, idUsuario, ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_CatalogoUsuarios, parameters, x => new gzUser
                {
                    Id = x.GetInt32(0),
                    Account = x.GetString(1),
                    Password = x.GetString(2)

                }).FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 16/02/2018 Lizbeth Morales 
        /// Este meotdo activara o desactivara al usuario seleccionado 
        /// </summary>
        /// <param name="id">id del usuario a Activar o desactivar</param>
        /// <param name="active"> Status del usuario (Si esta activo cambiara a inactivo y viceversa)</param>
        /// <returns>Nos regresara la informacion guardada del status del usuario </returns>

        public gzUser Delete(int id, int active, int idUsuario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'D', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pId", SqlDbType.Int, id, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pAccount", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNames", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pLastNames", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPassword", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pCorreo", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pActive", SqlDbType.TinyInt, active, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUserProfile_UserId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdSupervisor", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNumeroAgente", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, idUsuario, ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_CatalogoUsuarios, parameters, x => new gzUser
                {
                    Id = x.GetInt32(0),
                    Active = x.GetByte(1) == 1 ? "Activo" : "Inactivo"

                }).First();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public gzUser ConsultaClave(int id)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTACLAVE", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, id, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPassword", SqlDbType.VarChar, "", ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_CatalogoClave, parameters, x => new gzUser
                {

                    Password = x.GetString(0)
                }).First();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public gzUser CambiaClave(int id, string pass, int idUsuario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "MODIFICACLAVE", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, id, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPassword", SqlDbType.VarChar, pass, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuarioBitacora", SqlDbType.Int, idUsuario, ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_CatalogoClave, parameters, x => new gzUser
                {
                    Password = x.GetString(0)

                }).FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public int getUserIdAsesor(string usuario)
        {
            try
            {
                gzUser asesor = new gzUser();
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "GETUSERIDASESOR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUsuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));

                asesor = VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_CatalogoClave, parameters, x => new gzUser
                {
                    Id = x.GetInt32(0)
                }).FirstOrDefault();

                return asesor.Id;
            }
            catch
            {
                return -1;
            }
        }
        public gzUser ConsultarUsuario(int idUsuario, byte bandera)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, idUsuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.TinyInt, bandera, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUsuario", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pPassword", SqlDbType.VarChar, 0, ParameterDirection.Input));

                return VCEDBContext<gzUser>.CallStoreProcedure(StoredProcedures.VCE_ValidaLogin, parameters, x => new gzUser()
                {
                    Account = x.GetString(0),
                    RolStr = x.GetString(1)

                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
