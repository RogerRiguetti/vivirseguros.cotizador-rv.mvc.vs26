using System;
using System.Collections.Generic;
using System.Linq;
using Estudio.Repository.Core.Domain;
using log4net;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Estudio.Repository.Persistence.Repositories
{
    public class MantenedorPerfilesRepository
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Daniel Mercado 
        /// 09/06/2021
        /// Este metodo sirve para ver los detalles de cierto registro
        /// </summary>
        /// <param name="id">recibe como parametro el id del registro que se quiere conocer</param>
        /// <returns>regresa el registro buscado</returns>
        public MantenedorPerfiles DetailsMantenedorPerfiles(int id)
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'V', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPermiso", SqlDbType.Int, id, ParameterDirection.Input));


                return VCEDBContext<MantenedorPerfiles>.CallStoreProcedure(StoredProcedures.VCE_CatalogoMantenedorPerfiles, parameters, x => new MantenedorPerfiles
                {
                    Id = x.GetInt32(0),
                    Description = x.IsDBNull(1) ? "" : x.GetString(1),
                    DetailDescription = x.IsDBNull(3) ? "" : x.GetString(3),
                    IdSistema = x.GetInt32(4),
                    NombreSistema = x.IsDBNull(5) ? "" : x.GetString(5),
                    ClaveSistema = x.IsDBNull(6) ? "" : x.GetString(6),
                    NamePantalla = x.IsDBNull(7) ? "" : x.GetString(7),
                    NodoPadre = x.IsDBNull(8) ? "" : x.GetString(8)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                _log.Info("ERROR consulta DetailsMantenedorPerfiles " + ex);
                throw;
            }

        }

        /// <summary>
        /// Daniel Mercado 
        /// 07/06/2021
        /// Este metodo insertara o actualizara el permiso 
        /// </summary>
        /// <param name="rolclave">clave el rol</param>
        /// <param name="idPermiso"> id del Permiso</param>
        /// <returns>Nos regresara la informacion guardada del perfil </returns>

        public CheckPerfiles UpdatePermiso(string rolclave, string idPermiso, int idUsuario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'A', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPermiso", SqlDbType.Int,Convert.ToInt32(idPermiso), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pRolclave", SqlDbType.VarChar, rolclave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, idUsuario, ParameterDirection.Input));

                return VCEDBContext<CheckPerfiles>.CallStoreProcedure(StoredProcedures.VCE_CatalogoMantenedorPerfiles, parameters, x => new CheckPerfiles
                {
                    RolName = x.IsDBNull(0) ? "" : x.GetString(0),
                    ClaveRol = x.IsDBNull(1) ? "" : x.GetString(1),
                    DescripcionPermiso = x.IsDBNull(2) ? "" : x.GetString(2)                   
                   }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                _log.Info("ERROR consulta UpdatePermiso " + ex);
                throw;
            }
        }

        /// <summary>
        /// Daniel Mercado 
        /// 03/06/2021
        /// Este metodo obtiene la informacion del permiso a editar para posteriormente guardar los cambios realizados
        /// </summary>
        /// <param name="id">Parametro el cual obtiene el id del Permiso a modificar</param>
        /// <param name="namePantalla">Parametro que guardara el nombre del la pantalla</param>
        /// <param name="descripcionLarga">Parametro que guardara la descripcion larga del permiso</param>
        /// <param name="sistemas_Idsistema">Parametro que guardara el id del sistema</param>
        ///  <param name="nodoPadre">Parametro que guardara el nodoPadre</param>
        /// <returns> Regresa un objeto de la clase gzPermission que contiene la información del permiso creado </returns>

        public gzPermission Edit(int id,string namePantalla, string descripcionLarga, int sistemas_Idsistema, int idUsuario, string nodoPadre)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'U', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPermiso", SqlDbType.Int, id, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDetailedDescription", SqlDbType.VarChar, descripcionLarga, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdsistema", SqlDbType.Int, sistemas_Idsistema, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNamePantalla", SqlDbType.VarChar, namePantalla, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, idUsuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNodoPadre", SqlDbType.VarChar, nodoPadre, ParameterDirection.Input));

                return VCEDBContext<gzPermission>.CallStoreProcedure(StoredProcedures.VCE_CatalogoMantenedorPerfiles, parameters, x => new gzPermission
                {
                    Id = x.GetInt32(0),
                    Description = x.IsDBNull(1) ? "" : x.GetString(1),
                    DetailDescription = x.IsDBNull(2) ? "" : x.GetString(2),
                    NamePantalla = x.IsDBNull(3) ? "" : x.GetString(3),
                    NodoPadre = x.IsDBNull(4) ? "" : x.GetString(4)
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                _log.Info("ERROR consulta Edit " + ex);
                throw;
            }
        }

        /// <summary>
        /// Daniel Mercado 
        /// 02/06/2021
        /// Muestra los detalles del PERMISO indicado
        /// </summary>
        /// <param name="id"> Id del Permiso</param>
        /// <returns>
        /// Regresa la informacion del Permiso seleccionado
        /// </returns>
        public MantenedorPerfiles Details(int id)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'V', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPermiso", SqlDbType.Int, id, ParameterDirection.Input));

                return VCEDBContext<MantenedorPerfiles>.CallStoreProcedure(StoredProcedures.VCE_CatalogoMantenedorPerfiles, parameters, x => new MantenedorPerfiles
                {
                    Id = x.GetInt32(0),
                    Description = x.IsDBNull(1) ? "" : x.GetString(1),
                    DetailDescription = x.IsDBNull(3) ? "" : x.GetString(3),
                    IdSistema = x.GetInt32(4),
                    NombreSistema = x.IsDBNull(5) ? "" : x.GetString(5),
                    ClaveSistema = x.IsDBNull(6) ? "" : x.GetString(6),
                    NamePantalla = x.IsDBNull(7) ? "" : x.GetString(7),
                    NodoPadre = x.IsDBNull(8) ? "" : x.GetString(8)
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Info("ERROR consulta Details " + ex);
                throw;
            }
        }

        /// <summary>
        /// Daniel Mercado 
        /// 02/06/2021
        /// Este metodo activara o desactivara al Permiso seleccionado 
        /// </summary>
        /// <param name="id">id del Permiso a Activar o desactivar</param>
        /// <param name="active"> Status del Permiso (Si esta activo cambiara a inactivo y viceversa)</param>
        /// <returns>Nos regresara la informacion guardada del status del Permiso </returns>

        public gzPermission Delete(int id, int active, int idUsuario)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'D', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPermiso", SqlDbType.Int, id, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pActive", SqlDbType.Int, active, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, idUsuario, ParameterDirection.Input));

                return VCEDBContext<gzPermission>.CallStoreProcedure(StoredProcedures.VCE_CatalogoMantenedorPerfiles, parameters, x => new gzPermission
                {
                    Id = x.GetInt32(0),
                    Active = x.GetByte(1) == 1 ? "Activo" : "Inactivo"

                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                _log.Info("ERROR consulta Delete " + ex);
                throw;
            }
        }

        /// <summary>
        /// Daniel Mercado 02/06/2021
        /// Consulta los permisos para despues mostrarlos en la pantalla de Index (MantenedorPerfiles) 
        /// </summary>
        /// <returns>Regresa la informacion de los permisos a la pantalla Index</returns>
        public List<MantenedorPerfiles> Index(int idSistema, string clavePermiso)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'R', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdsistema", SqlDbType.Int, idSistema, ParameterDirection.Input));
                if (clavePermiso.Equals(""))
                    {
                    clavePermiso = null;
                    }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDescripcionPermiso", SqlDbType.VarChar, clavePermiso, ParameterDirection.Input));

                return VCEDBContext<MantenedorPerfiles>.CallStoreProcedure(StoredProcedures.VCE_CatalogoMantenedorPerfiles, parameters, x => new MantenedorPerfiles
                {
                    Id = x.GetInt32(0),
                    Description = x.IsDBNull(1) ? "" : x.GetString(1),
                    Active = x.GetByte(2) == 1 ? "Activo" : "Inactivo",
                    DetailDescription = x.IsDBNull(3) ? "" : x.GetString(3),
                    IdSistema = x.GetInt32(4),
                    NombreSistema = x.IsDBNull(5) ? "" : x.GetString(5),
                    ClaveSistema = x.IsDBNull(6) ? "" : x.GetString(6),
                    NamePantalla = x.IsDBNull(7) ? "" : x.GetString(7),
                    NodoPadre =  x.IsDBNull(8) ? "" : x.GetString(8)
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("ERROR consulta Index " + ex);

                throw;
            }

        }


        /// <summary>
        /// Daniel Mercado 02/06/2021
        /// Consulta los perfiles actuales de los permisos para despues mostrarlos en la pantalla de Index (MantenedorPerfiles) 
        /// </summary>
        /// <returns>Regresa la informacion de los permisos a la pantalla Index para la tabla y pintar los check</returns>
        public List<CheckPerfiles> IndexProfilePermissions(int idSistema, string clavePermiso)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'P', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdsistema", SqlDbType.Int, idSistema, ParameterDirection.Input));
               
                if (clavePermiso.Equals(""))
                {
                    clavePermiso = null;
                }
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDescripcionPermiso", SqlDbType.VarChar, clavePermiso, ParameterDirection.Input));

                return VCEDBContext<CheckPerfiles>.CallStoreProcedure(StoredProcedures.VCE_CatalogoMantenedorPerfiles, parameters, x => new CheckPerfiles
                {
                    RolName = x.IsDBNull(0) ? "" : x.GetString(0),
                    ClaveRol = x.IsDBNull(1) ? "" : x.GetString(1),
                    DescripcionPermiso = x.IsDBNull(2) ? "" : x.GetString(2),
                }).ToList();

            }
            catch (Exception ex)
            {
                _log.Info("ERROR consulta IndexProfilePermissions " + ex);
                throw;
            }

        }
        /// <summary>
        /// Daniel Mercado 02/06/2021
        /// </summary>
        /// <param name="claveCatalogo"> clave que indicara que catalogo se mostrara</param>
        /// <returns>Devuelve los roles que se manejan para despues formar una lista y generar las columnas de la tabla </returns>
        public List<UserProfile> ObtenerRoles(string claveCatalogo)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, claveCatalogo, ParameterDirection.Input));

                return VCEDBContext<UserProfile>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new UserProfile
                {
                    UserId = x.GetInt32(0),
                    Name = x.IsDBNull(1) ? "" : x.GetString(1),
                    Clave = x.IsDBNull(2) ? "" : x.GetString(2),
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.Info("ERROR consulta ObtenerRoles " + ex);
                throw;
            }
      
        }

        /// <summary>
        /// Daniel Mercado 01/06/2021
        /// </summary>
        /// <param name="claveCatalogo"> clave que indicara que catalogo se mostrara</param>
        /// <returns>Devuelve los sistemas que se manejan para despues mostrarlos en un ComboBox </returns>
        public List<Sistemas> ObtenerSistemas(string claveCatalogo)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, claveCatalogo, ParameterDirection.Input));

                return VCEDBContext<Sistemas>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new Sistemas
                {
                    IdSistema = x.GetInt32(0),
                    NombreSistema = x.IsDBNull(1) ? "" : x.GetString(1),
                }).ToList();
            }
            catch (Exception ex)
            {
                _log.Info("ERROR consulta ObtenerSistemas " + ex);
                throw;
            }
          
        }

        /// <summary>
        /// Daniel Mercado 01/06/2021 
        /// Este metodo tiene la conexion con el SP en la base de datos, por lo tanto ete metodo guardara la informacion ingresada del usuario permiso
        /// </summary>
        /// <param name="clave">Parametro que guardara el nombre del nuevo permiso</param>
        /// <param name="descripcionLarga">Parametro que guardara la descripcion larga del permiso</param>
        /// <param name="sistemas_Idsistema">Parametro que guardara el id del sistema</param
        /// <param name="nodoPadre">Parametro que guardara el nodoPadre</param>
        /// <returns> Regresa un objeto de la clase gzUser que contiene la información del permiso creado </returns>

        public gzPermission Create(string namePantalla,string clave, string descripcionLarga, int sistemas_Idsistema, int idUsuario, string nodoPadre)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'C', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdPermiso", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDescripcionPermiso", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pDetailedDescription", SqlDbType.VarChar, descripcionLarga, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdsistema", SqlDbType.Int, sistemas_Idsistema, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNamePantalla", SqlDbType.VarChar, namePantalla, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, idUsuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pNodoPadre", SqlDbType.VarChar, nodoPadre, ParameterDirection.Input));
                return VCEDBContext<gzPermission>.CallStoreProcedure(StoredProcedures.VCE_CatalogoMantenedorPerfiles, parameters, x => new gzPermission
                {
                    Id =  x.GetInt32(0),
                    Description = x.IsDBNull(1) ? "" : x.GetString(1),
                    DetailDescription = x.IsDBNull(2) ? "" : x.GetString(2),
                    NamePantalla = x.IsDBNull(3) ? "" : x.GetString(3),
                    NodoPadre = x.IsDBNull(4) ? "" : x.GetString(4)

                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                _log.Info("ERROR consulta Create " + ex);
                throw;
            }
        }

        /// <summary>
        /// Daniel Mercado 02/06/2021
        /// </summary>
        /// <param name="claveCatalogo"> clave que indicara que catalogo se mostrara</param>
        /// <returns>Devuelve los roles que se manejan para despues formar una lista y generar las columnas de la tabla </returns>
        public List<UserProfile> ObtenerRolesFiltro(string claveCatalogo)
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClaveCatalogo", SqlDbType.VarChar, claveCatalogo, ParameterDirection.Input));

                return VCEDBContext<UserProfile>.CallStoreProcedure(StoredProcedures.VCE_Catalogos, parameters, x => new UserProfile
                {
                    UserId = x.GetInt32(0),
                    Name = x.IsDBNull(1) ? "" : x.GetString(1),
                    Clave = x.IsDBNull(2) ? "" : x.GetString(2),
                    IndexRol = x.GetInt64(4)
                }).ToList();
            }
            catch (Exception ex )
            {
                _log.Info("ERROR consulta ObtenerRolesFiltro " + ex);
                throw;
            }
         
        }
    }
}
