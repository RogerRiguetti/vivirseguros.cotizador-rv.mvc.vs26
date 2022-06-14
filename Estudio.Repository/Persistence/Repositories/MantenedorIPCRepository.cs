using Estudio.Repository.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Persistence.Repositories
{
    public class MantenedorIPCRepository
    {
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-23
        /// </summary>
        /// <returns>retorna la informacion para cargar la tabla inicial</returns>
        public List<MantenedorIPC> CargarTabla()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CARGARTABLA", ParameterDirection.Input));

                return VCEDBContext<MantenedorIPC>.CallStoreProcedure(StoredProcedures.CO_CatalogosMantenedorIPC, parameters, x => new MantenedorIPC
                {
                    FechaIpc = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy"),
                    MontoIpc = x.GetDecimal(1),
                    VariacionIpc=x.GetDecimal(2),
                    Codigo= x.GetString(3)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-24
        /// </summary>
        /// <param name="fecha">fecha de busqueda</param>
        /// <returns>retorna la informacion si existe</returns>
        public MantenedorIPC Consulta(DateTime fecha)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "CONSULTA", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFecha", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));

                return VCEDBContext<MantenedorIPC>.CallStoreProcedure(StoredProcedures.CO_CatalogosMantenedorIPC, parameters, x => new MantenedorIPC
                {
                    FechaIpc = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM-yyyy")
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        ///  Osvaldo Valdez Carrillo
        /// 2018-08-24
        /// </summary>
        /// <param name="fechaI">fecha IPC</param>
        /// <param name="valorIPC">valor ipc</param>
        /// <param name="variacionIPC"> variacion ipc</param>
        /// <param name="codigo">codigo N</param>
        /// <param name="usuario">Nombre del usuario</param>
        /// <param name="clave">clave de guardado o actualizar</param>
        /// <returns>retorna si se pudo grabar o actualizar</returns>
        public MantenedorIPC GrabarMantenedorIPC(string clave, DateTime fechaI, decimal valorIPC, decimal variacionIPC, string codigo, string usuario)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, clave, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFecha", SqlDbType.VarChar, fechaI.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vmto_ipc", SqlDbType.Decimal, valorIPC, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vprc_ipc", SqlDbType.Decimal, variacionIPC, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vcod_indestcal", SqlDbType.VarChar, codigo, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vcod_usuario", SqlDbType.VarChar, usuario, ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vfec_modi", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vhor_modi", SqlDbType.VarChar, fecha.ToString("hhmmss"), ParameterDirection.Input));

                return VCEDBContext<MantenedorIPC>.CallStoreProcedure(StoredProcedures.CO_CatalogosMantenedorIPC, parameters, x => new MantenedorIPC
                {
                    FechaIpc = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM-yyyy")
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Osvaldo Valdez Carrillo
        /// 2018-08-24
        /// </summary>
        /// <param name="fecha">fecha IPC</param>
        /// <returns>retorna si se elimino el registro</returns>
        public object EliminarMantenedorIPC(DateTime fecha)
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "ELIMINAR", ParameterDirection.Input));
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@vFecha", SqlDbType.VarChar, fecha.ToString("yyyyMMdd"), ParameterDirection.Input));

                return VCEDBContext<MantenedorIPC>.CallStoreProcedure(StoredProcedures.CO_CatalogosMantenedorIPC, parameters, x => new MantenedorIPC
                {
                    FechaIpc = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM-yyyy")
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        ///  Osvaldo Valdez Carrillo
        /// 2018-08-24
        /// </summary>
        /// <returns>genera el reporte</returns>
        public List<MantenedorIPC> ConsultaRpt()
        {
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(VCEDBContext<RowAffected>.AddParams("@pClave", SqlDbType.VarChar, "REPORTE", ParameterDirection.Input));

                return VCEDBContext<MantenedorIPC>.CallStoreProcedure(StoredProcedures.CO_CatalogosMantenedorIPC, parameters, x => new MantenedorIPC
                {
                    FechaIpc = DateTime.ParseExact(x.GetString(0), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM-yyyy"),
                    MontoIpc = x.GetDecimal(1),
                    VariacionIpc = x.GetDecimal(2),
                    Codigo = x.GetString(3)
                }).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
