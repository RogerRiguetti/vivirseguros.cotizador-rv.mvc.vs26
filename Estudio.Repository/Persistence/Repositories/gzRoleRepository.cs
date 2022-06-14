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
public class gzRoleRepository 
{

        /// <summary>
        /// Autor: Heber Solis
        /// Este metodo sirve para cargar la tabla que está en la vista de Index en la carpeta de Roles
        /// </summary>
        /// <returns>regresa la lista que llenará la tabla</returns>
        public List<RoleView> Index()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'R', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUserId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pName", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pActive", SqlDbType.Int, 0, ParameterDirection.Input));


                return VCEDBContext<RoleView>.CallStoreProcedure(StoredProcedures.VCE_CatalogoPerfiles, parameters, x => new RoleView
                {
                    Id = x.GetInt32(0),
                    Description = x.IsDBNull(1) ? "" : x.GetString(1),
                    Status = x.GetByte(2) == 1 ? "Activo" : "Inactivo",
                    Clave =  x.IsDBNull(3) ? "" : x.GetString(3)
                }).ToList();
                
            }
            catch (Exception ex)
            {

                throw;
            }
           
            
        }
        /// <summary>
        /// Autor: Heber Solis
        /// Este metodo sirve para ver los detalles de cierto registro
        /// </summary>
        /// <param name="id">recibe como parametro el id del registro que se quiere conocer</param>
        /// <returns>regresa el registro buscado</returns>
        public gzRole DetailsRole(int id)
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'E', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUserId", SqlDbType.Int, id, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pName", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pActive", SqlDbType.Int, 0, ParameterDirection.Input));


                return VCEDBContext<gzRole>.CallStoreProcedure(StoredProcedures.VCE_CatalogoPerfiles, parameters, x => new gzRole
                {
                    Id = x.GetInt32(0),
                    Description = x.GetString(1),
                    Active = x.GetByte(2) == 1 ? "Activo" : "Inactivo",
                    Clave = x.IsDBNull(3) ? "" : x.GetString(3)
                }).First();

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        /// <summary>
        /// Autor: Heber Solis
        /// este metodo sirve para dar de alta un nuevo rol
        /// </summary>
        /// <param name="description">es el nombre del rol</param>
        /// <param name="clave">la clave del rol creado</param>
        /// <returns>regresa el registro creado</returns>
        public gzRole CreateRole(string description, string clave, int idUsuario)
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'C', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUserId", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pName", SqlDbType.VarChar, description, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pActive", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, idUsuario, ParameterDirection.Input));

                return VCEDBContext<gzRole>.CallStoreProcedure(StoredProcedures.VCE_CatalogoPerfiles, parameters, x => new gzRole
                {
                    Id = x.GetInt32(0),
                    Description = x.GetString(1),
                    Active = x.GetByte(2) == 1 ? "Activo" : "Inactivo"
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        /// <summary>
        /// Autor: Heber Solis
        /// Este metodo hace una baja logica del rol que se quiera desactivar
        /// </summary>
        /// <param name="id">el id del rol que se quiera desactivar</param>
        /// <param name="active">el estado del rol</param>
        /// <returns>regresa una lista ya que en la capa logica se asignan mas valores</returns>
        public List<gzRole> DeleteRole(int id, int active, int idUsuario)
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'D', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUserId", SqlDbType.Int, id, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pName", SqlDbType.VarChar, "", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pActive", SqlDbType.Int, active, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, idUsuario, ParameterDirection.Input));

                return VCEDBContext<gzRole>.CallStoreProcedure(StoredProcedures.VCE_CatalogoPerfiles, parameters, x => new gzRole
                {
                    Id = x.GetInt32(0),
                    Description = x.GetString(1),
                    Active = x.GetByte(2) == 1 ? "Activo" : "Inactivo"
                }).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        /// <summary>
        /// Autor: Heber Solis
        /// Este metodo edita el nombre del Rol
        /// </summary>
        /// <param name="id">id del rol que se va a editar</param>
        /// <param name="description">nombre del rol</param>
        /// <returns>regresa una lista ya que en la capa logica se asignan mas valores</returns>
        public gzRole EditRole(int id, string description, int idUsuario)
        {

            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pBandera", SqlDbType.Char, 'U', ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pUserId", SqlDbType.Int, id, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pName", SqlDbType.VarChar, description, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pActive", SqlDbType.Int, 0, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pIdUsuario", SqlDbType.Int, idUsuario, ParameterDirection.Input));

                return VCEDBContext<gzRole>.CallStoreProcedure(StoredProcedures.VCE_CatalogoPerfiles, parameters, x => new gzRole
                {
                    Id = x.GetInt32(0),
                    Description = x.GetString(1),
                  
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }

        }





    }
}
